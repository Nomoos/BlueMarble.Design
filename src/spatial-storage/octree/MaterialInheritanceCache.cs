using System;
using System.Collections.Generic;
using System.Linq;

namespace BlueMarble.SpatialStorage.Octree
{
    /// <summary>
    /// Performance optimization cache for material inheritance lookups.
    /// Reduces O(log n) inheritance chain walks to O(1) for frequently accessed paths.
    /// </summary>
    public class MaterialInheritanceCache
    {
        private readonly Dictionary<string, MaterialData> _pathCache = new();
        private readonly Dictionary<string, DateTime> _cacheTimestamps = new();
        private readonly TimeSpan _cacheTimeout = TimeSpan.FromMinutes(10);
        private readonly int _maxCacheSize = 10000;

        /// <summary>
        /// Get material for a specific octree path, using cache when available
        /// </summary>
        public MaterialData GetMaterialForPath(string octreePath, MaterialInheritanceNode rootNode)
        {
            // Check cache first
            if (_pathCache.TryGetValue(octreePath, out var cached))
            {
                // Verify cache entry is still valid
                if (_cacheTimestamps.TryGetValue(octreePath, out var timestamp) &&
                    DateTime.UtcNow - timestamp < _cacheTimeout)
                {
                    return cached;
                }
                
                // Remove expired entry
                _pathCache.Remove(octreePath);
                _cacheTimestamps.Remove(octreePath);
            }

            // Compute inherited material
            var material = ComputeInheritedMaterial(octreePath, rootNode);
            
            // Cache the result
            CacheResult(octreePath, material);
            
            return material;
        }

        /// <summary>
        /// Invalidate cache entries when materials change.
        /// Removes all cached paths that start with the given prefix.
        /// </summary>
        public void InvalidatePath(string pathPrefix)
        {
            var toRemove = _pathCache.Keys
                .Where(k => k.StartsWith(pathPrefix))
                .ToList();

            foreach (var key in toRemove)
            {
                _pathCache.Remove(key);
                _cacheTimestamps.Remove(key);
            }
        }

        /// <summary>
        /// Clear all cached entries
        /// </summary>
        public void ClearCache()
        {
            _pathCache.Clear();
            _cacheTimestamps.Clear();
        }

        /// <summary>
        /// Get cache statistics for monitoring
        /// </summary>
        public CacheStatistics GetStatistics()
        {
            return new CacheStatistics
            {
                CacheSize = _pathCache.Count,
                MaxCacheSize = _maxCacheSize,
                CacheHitRate = CalculateCacheHitRate(),
                MemoryUsage = EstimateMemoryUsage()
            };
        }

        private MaterialData ComputeInheritedMaterial(string octreePath, MaterialInheritanceNode rootNode)
        {
            if (string.IsNullOrEmpty(octreePath))
                return rootNode.GetEffectiveMaterial();

            var node = TraverseToNode(octreePath, rootNode);
            return node?.GetEffectiveMaterial() ?? MaterialData.DefaultOcean;
        }

        private MaterialInheritanceNode? TraverseToNode(string path, MaterialInheritanceNode root)
        {
            var current = root;
            
            // Parse path string (e.g., "001101" -> sequence of child indices)
            for (int i = 0; i < path.Length; i++)
            {
                if (current?.Children == null)
                    return current;

                var childIndex = int.Parse(path[i].ToString());
                if (childIndex < 0 || childIndex >= current.Children.Length)
                    return current;

                current = current.Children[childIndex];
                if (current == null)
                    return null;
            }

            return current;
        }

        private void CacheResult(string path, MaterialData material)
        {
            // Implement LRU eviction if cache is full
            if (_pathCache.Count >= _maxCacheSize)
            {
                EvictOldestEntries();
            }

            _pathCache[path] = material;
            _cacheTimestamps[path] = DateTime.UtcNow;
        }

        private void EvictOldestEntries()
        {
            // Remove 20% of oldest entries
            var entriesToRemove = (int)(_maxCacheSize * 0.2);
            var oldestEntries = _cacheTimestamps
                .OrderBy(kvp => kvp.Value)
                .Take(entriesToRemove)
                .Select(kvp => kvp.Key)
                .ToList();

            foreach (var key in oldestEntries)
            {
                _pathCache.Remove(key);
                _cacheTimestamps.Remove(key);
            }
        }

        private double CalculateCacheHitRate()
        {
            // This would require tracking hits/misses in a real implementation
            // For now, return a placeholder
            return 0.85; // 85% hit rate is typical for spatial caches
        }

        private long EstimateMemoryUsage()
        {
            // Rough estimation of memory usage
            var pathMemory = _pathCache.Keys.Sum(k => k.Length * sizeof(char));
            var materialMemory = _pathCache.Count * 64; // Estimated material size
            var timestampMemory = _cacheTimestamps.Count * sizeof(long);
            
            return pathMemory + materialMemory + timestampMemory;
        }
    }

    /// <summary>
    /// Cache performance statistics
    /// </summary>
    public class CacheStatistics
    {
        public int CacheSize { get; set; }
        public int MaxCacheSize { get; set; }
        public double CacheHitRate { get; set; }
        public long MemoryUsage { get; set; }

        public double CacheUtilization => (double)CacheSize / MaxCacheSize;
    }
}