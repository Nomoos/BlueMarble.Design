using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BlueMarble.Utils.Spatial.DistributedStorage
{
    /// <summary>
    /// In-memory reference implementation of distributed octree storage.
    /// Suitable for development, testing, and single-node deployments.
    /// Features LRU caching, optimistic concurrency, and simulated replication.
    /// </summary>
    public class InMemoryDistributedOctreeStorage : IDistributedOctreeStorage
    {
        private readonly ConcurrentDictionary<string, SpatialOctreeNode> _storage;
        private readonly ConcurrentDictionary<string, SpatialOctreeNode> _l1Cache;
        private readonly ConcurrentDictionary<ulong, string> _mortonIndex;
        private readonly SpatialHashGenerator _hashGenerator;
        private readonly int _maxCacheSize;
        private readonly object _cacheLock = new object();
        
        // Statistics
        private long _cacheHits;
        private long _cacheMisses;
        private long _totalQueries;
        private long _totalWrites;
        private readonly List<double> _queryLatencies;
        private readonly object _statsLock = new object();

        public InMemoryDistributedOctreeStorage(
            SpatialBounds worldBounds,
            int maxCacheSize = 10000,
            int replicationFactor = 3)
        {
            _storage = new ConcurrentDictionary<string, SpatialOctreeNode>();
            _l1Cache = new ConcurrentDictionary<string, SpatialOctreeNode>();
            _mortonIndex = new ConcurrentDictionary<ulong, string>();
            _hashGenerator = new SpatialHashGenerator(worldBounds, replicationFactor);
            _maxCacheSize = maxCacheSize;
            _queryLatencies = new List<double>();
        }

        public async Task<QueryResult> QueryMaterialAsync(
            double x,
            double y,
            double z,
            int lod,
            ConsistencyLevel consistency = ConsistencyLevel.Quorum,
            CancellationToken cancellationToken = default)
        {
            var stopwatch = Stopwatch.StartNew();
            Interlocked.Increment(ref _totalQueries);

            // Generate hierarchical keys for the position
            var keys = _hashGenerator.GenerateHierarchicalKeys(x, y, z, lod);

            // Try L1 cache first (in-memory, fastest)
            foreach (var key in keys.OrderByDescending(k => k.Level))
            {
                if (_l1Cache.TryGetValue(key.CacheKey, out var cachedNode))
                {
                    Interlocked.Increment(ref _cacheHits);
                    stopwatch.Stop();
                    RecordLatency(stopwatch.Elapsed.TotalMilliseconds);

                    return new QueryResult
                    {
                        Node = cachedNode,
                        Success = true,
                        Source = "L1Cache",
                        Latency = stopwatch.Elapsed,
                        MaterialId = cachedNode.MaterialId
                    };
                }
            }

            // Cache miss - query from storage
            Interlocked.Increment(ref _cacheMisses);

            // Find the most specific node that contains this position
            // First try exact Morton code matches, then fall back to spatial bounds check
            SpatialOctreeNode resultNode = null;
            foreach (var key in keys.OrderByDescending(k => k.Level))
            {
                var mortonKey = GetMortonKey(key.MortonCode, key.Level);
                if (_mortonIndex.TryGetValue(mortonKey, out var nodeId))
                {
                    if (_storage.TryGetValue(nodeId, out var node))
                    {
                        resultNode = node;
                        
                        // Add to cache for future queries
                        PromoteToCache(key.CacheKey, node);
                        break;
                    }
                }
            }

            // If no exact Morton match, try to find any node that contains the position
            if (resultNode == null)
            {
                foreach (var node in _storage.Values.OrderByDescending(n => n.Level))
                {
                    if (node.Bounds != null && node.Bounds.Contains(x, y, z))
                    {
                        resultNode = node;
                        
                        // Add to cache for future queries
                        var cacheKey = $"octree:{node.Level:D2}:{node.MortonCode:X16}";
                        PromoteToCache(cacheKey, node);
                        break;
                    }
                }
            }

            stopwatch.Stop();
            RecordLatency(stopwatch.Elapsed.TotalMilliseconds);

            if (resultNode != null)
            {
                return new QueryResult
                {
                    Node = resultNode,
                    Success = true,
                    Source = "Storage",
                    Latency = stopwatch.Elapsed,
                    MaterialId = resultNode.MaterialId
                };
            }

            return new QueryResult
            {
                Success = false,
                Source = "NotFound",
                Latency = stopwatch.Elapsed
            };
        }

        public async Task<SpatialOctreeNode> GetNodeAsync(
            string nodeId,
            ConsistencyLevel consistency = ConsistencyLevel.Quorum,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(nodeId))
                return null;

            // Check cache first
            var cacheKey = $"node:{nodeId}";
            if (_l1Cache.TryGetValue(cacheKey, out var cachedNode))
            {
                Interlocked.Increment(ref _cacheHits);
                return cachedNode;
            }

            Interlocked.Increment(ref _cacheMisses);

            // Get from storage
            if (_storage.TryGetValue(nodeId, out var node))
            {
                PromoteToCache(cacheKey, node);
                return node;
            }

            return null;
        }

        public async Task<SpatialOctreeNode> GetNodeByMortonAsync(
            ulong mortonCode,
            int level,
            ConsistencyLevel consistency = ConsistencyLevel.Quorum,
            CancellationToken cancellationToken = default)
        {
            var mortonKey = GetMortonKey(mortonCode, level);
            
            if (_mortonIndex.TryGetValue(mortonKey, out var nodeId))
            {
                return await GetNodeAsync(nodeId, consistency, cancellationToken);
            }

            return null;
        }

        public async Task<WriteResult> WriteNodeAsync(
            SpatialOctreeNode node,
            ConsistencyLevel consistency = ConsistencyLevel.Quorum,
            CancellationToken cancellationToken = default)
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));

            Interlocked.Increment(ref _totalWrites);

            // Capture the input version at the start
            var inputVersion = node.Version;

            // Optimistic concurrency control
            if (!string.IsNullOrEmpty(node.Id) && _storage.ContainsKey(node.Id))
            {
                // For existing nodes, check if we have the current version
                // We need to check this before potentially getting a reference to the stored node
                var storedVersion = _storage[node.Id].Version;
                
                if (storedVersion != inputVersion)
                {
                    return new WriteResult
                    {
                        Success = false,
                        ErrorMessage = "Version conflict detected",
                        AcknowledgedReplicas = 0
                    };
                }
            }

            // Increment version and update timestamp
            node.Version++;
            node.LastModified = DateTime.UtcNow;

            // Write to storage
            _storage[node.Id] = node;

            // Update Morton index
            var mortonKey = GetMortonKey(node.MortonCode, node.Level);
            _mortonIndex[mortonKey] = node.Id;

            // Update cache
            var cacheKey = $"octree:{node.Level:D2}:{node.MortonCode:X16}";
            PromoteToCache(cacheKey, node);

            // Simulate replication based on consistency level
            int acknowledgedReplicas = consistency switch
            {
                ConsistencyLevel.One => 1,
                ConsistencyLevel.Quorum => 2,
                ConsistencyLevel.All => 3,
                _ => 1
            };

            return new WriteResult
            {
                Success = true,
                NewVersion = node.Version,
                AcknowledgedReplicas = acknowledgedReplicas
            };
        }

        public async Task<bool> DeleteNodeAsync(
            string nodeId,
            ConsistencyLevel consistency = ConsistencyLevel.Quorum,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(nodeId))
                return false;

            // Remove from storage
            if (_storage.TryRemove(nodeId, out var node))
            {
                // Remove from Morton index
                var mortonKey = GetMortonKey(node.MortonCode, node.Level);
                _mortonIndex.TryRemove(mortonKey, out _);

                // Remove from cache
                var cacheKey = $"octree:{node.Level:D2}:{node.MortonCode:X16}";
                _l1Cache.TryRemove(cacheKey, out _);

                return true;
            }

            return false;
        }

        public async Task<List<SpatialOctreeNode>> QueryRegionAsync(
            SpatialBounds bounds,
            int maxLevel,
            CancellationToken cancellationToken = default)
        {
            var results = new List<SpatialOctreeNode>();

            foreach (var node in _storage.Values)
            {
                if (node.Level <= maxLevel && node.Bounds.Intersects(bounds))
                {
                    results.Add(node);
                }
            }

            return results;
        }

        public async Task<StorageStatistics> GetStatisticsAsync(CancellationToken cancellationToken = default)
        {
            var stats = new StorageStatistics
            {
                TotalNodes = _storage.Count,
                CacheHits = Interlocked.Read(ref _cacheHits),
                CacheMisses = Interlocked.Read(ref _cacheMisses),
                TotalQueries = Interlocked.Read(ref _totalQueries),
                TotalWrites = Interlocked.Read(ref _totalWrites)
            };

            lock (_statsLock)
            {
                if (_queryLatencies.Count > 0)
                {
                    stats.AverageQueryLatencyMs = _queryLatencies.Average();
                }
            }

            // Calculate nodes by level
            var nodesByLevel = _storage.Values
                .GroupBy(n => n.Level)
                .ToDictionary(g => $"Level{g.Key}", g => (long)g.Count());

            stats.NodesByLevel = nodesByLevel;

            return stats;
        }

        public async Task ClearCacheAsync(CancellationToken cancellationToken = default)
        {
            _l1Cache.Clear();
        }

        private void PromoteToCache(string cacheKey, SpatialOctreeNode node)
        {
            lock (_cacheLock)
            {
                // Simple LRU eviction when cache is full
                if (_l1Cache.Count >= _maxCacheSize)
                {
                    // Remove oldest entry (simplified - a real LRU would track access times)
                    var oldestKey = _l1Cache.Keys.First();
                    _l1Cache.TryRemove(oldestKey, out _);
                }

                _l1Cache[cacheKey] = node;
            }
        }

        private void RecordLatency(double latencyMs)
        {
            lock (_statsLock)
            {
                _queryLatencies.Add(latencyMs);
                
                // Keep only last 1000 latencies for average calculation
                if (_queryLatencies.Count > 1000)
                {
                    _queryLatencies.RemoveAt(0);
                }
            }
        }

        private ulong GetMortonKey(ulong mortonCode, int level)
        {
            // Combine level and morton code into a single key
            // Use upper 8 bits for level (supports up to 256 levels)
            return ((ulong)level << 56) | (mortonCode & 0x00FFFFFFFFFFFFFF);
        }
    }
}
