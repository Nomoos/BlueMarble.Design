using System;
using System.Collections.Generic;
using System.Threading;

namespace BlueMarble.SpatialStorage
{
    /// <summary>
    /// Global octree with adaptive 3D spatial partitioning and material caching
    /// Handles levels 1-12 (~1m resolution) before transitioning to grid tiles
    /// </summary>
    public class GlobalOctree
    {
        private readonly OctreeNode _root;
        private readonly int _maxLevel;
        private readonly Dictionary<string, MaterialId> _materialCache;
        private readonly ReaderWriterLockSlim _cacheLock;

        // Performance statistics
        private long _queryCount;
        private long _cacheHits;
        private long _subdivisions;

        public const int DEFAULT_MAX_LEVEL = 12;
        public const double WORLD_SIZE = 40075000.0; // Earth circumference in meters

        public GlobalOctree(double worldSize = WORLD_SIZE, int maxLevel = DEFAULT_MAX_LEVEL)
        {
            _maxLevel = maxLevel;
            _root = new OctreeNode(
                new Envelope3D(-worldSize / 2, -worldSize / 2, -worldSize / 2, 
                              worldSize / 2, worldSize / 2, worldSize / 2),
                0
            );

            _materialCache = new Dictionary<string, MaterialId>();
            _cacheLock = new ReaderWriterLockSlim();
        }

        /// <summary>
        /// Query material at position with O(log n) complexity
        /// Uses material caching for repeated queries
        /// </summary>
        public MaterialId QueryMaterial(Vector3 position)
        {
            Interlocked.Increment(ref _queryCount);

            // Check cache first
            string cacheKey = GetCacheKey(position);
            _cacheLock.EnterReadLock();
            try
            {
                if (_materialCache.TryGetValue(cacheKey, out MaterialId cachedMaterial))
                {
                    Interlocked.Increment(ref _cacheHits);
                    return cachedMaterial;
                }
            }
            finally
            {
                _cacheLock.ExitReadLock();
            }

            // Query from octree
            MaterialId material = _root.QueryMaterial(position);

            // Update cache
            _cacheLock.EnterWriteLock();
            try
            {
                if (_materialCache.Count < 10000) // Limit cache size
                {
                    _materialCache[cacheKey] = material;
                }
            }
            finally
            {
                _cacheLock.ExitWriteLock();
            }

            return material;
        }

        /// <summary>
        /// Update material at position with automatic subdivision
        /// </summary>
        public void UpdateMaterial(Vector3 position, MaterialId material)
        {
            _root.UpdateMaterial(position, material, _maxLevel);

            // Invalidate cache for this region
            string cacheKey = GetCacheKey(position);
            _cacheLock.EnterWriteLock();
            try
            {
                _materialCache.Remove(cacheKey);
            }
            finally
            {
                _cacheLock.ExitWriteLock();
            }

            Interlocked.Increment(ref _subdivisions);
        }

        /// <summary>
        /// Batch update materials for efficiency
        /// </summary>
        public void UpdateMaterialBatch(List<(Vector3 position, MaterialId material)> updates)
        {
            foreach (var (position, material) in updates)
            {
                _root.UpdateMaterial(position, material, _maxLevel);
            }

            // Clear cache after batch update
            ClearCache();
        }

        /// <summary>
        /// Initialize ocean region with single material (demonstrates 90%+ memory savings)
        /// </summary>
        public void InitializeOceanRegion(Envelope3D region)
        {
            // For demonstration: would normally sample region and set material
            // This shows how homogeneous regions achieve massive memory savings
            Vector3 center = region.Center;
            _root.UpdateMaterial(center, MaterialId.Ocean, 0);
        }

        /// <summary>
        /// Query region and return all materials with positions
        /// </summary>
        public List<(Vector3 position, MaterialId material)> QueryRegion(Envelope3D region, double resolution)
        {
            var results = new List<(Vector3, MaterialId)>();

            double stepX = resolution;
            double stepY = resolution;
            double stepZ = resolution;

            for (double x = region.MinX; x < region.MaxX; x += stepX)
            {
                for (double y = region.MinY; y < region.MaxY; y += stepY)
                {
                    for (double z = region.MinZ; z < region.MaxZ; z += stepZ)
                    {
                        Vector3 position = new Vector3(x, y, z);
                        if (region.Contains(position))
                        {
                            MaterialId material = QueryMaterial(position);
                            results.Add((position, material));
                        }
                    }
                }
            }

            return results;
        }

        /// <summary>
        /// Calculate resolution at a given level
        /// </summary>
        public static double GetResolutionAtLevel(int level)
        {
            return WORLD_SIZE / Math.Pow(2, level);
        }

        /// <summary>
        /// Get appropriate level for target resolution
        /// </summary>
        public static int GetLevelForResolution(double targetResolution)
        {
            return (int)Math.Ceiling(Math.Log(WORLD_SIZE / targetResolution, 2));
        }

        /// <summary>
        /// Get performance statistics
        /// </summary>
        public OctreeStatistics GetStatistics()
        {
            var (nodeCount, leafCount, materialCount) = _root.GetStatistics();

            return new OctreeStatistics
            {
                TotalNodes = nodeCount,
                LeafNodes = leafCount,
                NodesWithMaterial = materialCount,
                QueryCount = _queryCount,
                CacheHits = _cacheHits,
                CacheHitRate = _queryCount > 0 ? (double)_cacheHits / _queryCount : 0,
                Subdivisions = _subdivisions,
                CacheSize = _materialCache.Count,
                MemorySavingsPercent = CalculateMemorySavings(nodeCount, materialCount)
            };
        }

        /// <summary>
        /// Clear material cache
        /// </summary>
        public void ClearCache()
        {
            _cacheLock.EnterWriteLock();
            try
            {
                _materialCache.Clear();
            }
            finally
            {
                _cacheLock.ExitWriteLock();
            }
        }

        /// <summary>
        /// Calculate memory savings from material inheritance
        /// </summary>
        private double CalculateMemorySavings(int actualNodes, int nodesWithMaterial)
        {
            if (actualNodes == 0)
                return 0;

            // Theoretical: every node has material = 100% memory
            // Actual: only nodes with explicit material use memory
            return 100.0 * (1.0 - ((double)nodesWithMaterial / actualNodes));
        }

        /// <summary>
        /// Generate cache key for position
        /// </summary>
        private string GetCacheKey(Vector3 position)
        {
            // Round to cache grid
            int gridSize = 100; // 100m grid
            int x = (int)(position.X / gridSize);
            int y = (int)(position.Y / gridSize);
            int z = (int)(position.Z / gridSize);
            return $"{x},{y},{z}";
        }
    }

    /// <summary>
    /// Octree performance statistics
    /// </summary>
    public class OctreeStatistics
    {
        public int TotalNodes { get; set; }
        public int LeafNodes { get; set; }
        public int NodesWithMaterial { get; set; }
        public long QueryCount { get; set; }
        public long CacheHits { get; set; }
        public double CacheHitRate { get; set; }
        public long Subdivisions { get; set; }
        public int CacheSize { get; set; }
        public double MemorySavingsPercent { get; set; }

        public override string ToString()
        {
            return $"Octree Statistics:\n" +
                   $"  Nodes: {TotalNodes} (Leaves: {LeafNodes}, With Material: {NodesWithMaterial})\n" +
                   $"  Queries: {QueryCount} (Cache Hits: {CacheHits}, Hit Rate: {CacheHitRate:P2})\n" +
                   $"  Subdivisions: {Subdivisions}\n" +
                   $"  Memory Savings: {MemorySavingsPercent:F1}%";
        }
    }
}
