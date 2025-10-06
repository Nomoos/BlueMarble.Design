using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace BlueMarble.SpatialStorage.Research
{
    /// <summary>
    /// Cache entry with TTL support
    /// </summary>
    public class MaterialCacheEntry
    {
        public MaterialId Material { get; set; }
        public DateTime Timestamp { get; set; }
        public TimeSpan TTL { get; set; }
        
        public bool IsValid() => DateTime.UtcNow - Timestamp < TTL;
    }

    /// <summary>
    /// Simple LRU cache implementation
    /// </summary>
    public class LRUCache<TKey, TValue>
    {
        private readonly int _maxEntries;
        private readonly Dictionary<TKey, LinkedListNode<CacheItem>> _cache;
        private readonly LinkedList<CacheItem> _lruList;
        private int _hits;
        private int _misses;
        
        private class CacheItem
        {
            public TKey Key { get; set; }
            public TValue Value { get; set; }
        }
        
        public LRUCache(int maxEntries)
        {
            _maxEntries = maxEntries;
            _cache = new Dictionary<TKey, LinkedListNode<CacheItem>>();
            _lruList = new LinkedList<CacheItem>();
        }
        
        public bool TryGet(TKey key, out TValue value)
        {
            if (_cache.TryGetValue(key, out var node))
            {
                _hits++;
                _lruList.Remove(node);
                _lruList.AddFirst(node);
                value = node.Value.Value;
                return true;
            }
            
            _misses++;
            value = default;
            return false;
        }
        
        public void Put(TKey key, TValue value)
        {
            if (_cache.TryGetValue(key, out var existingNode))
            {
                _lruList.Remove(existingNode);
                existingNode.Value.Value = value;
                _lruList.AddFirst(existingNode);
            }
            else
            {
                if (_cache.Count >= _maxEntries)
                {
                    var lastNode = _lruList.Last;
                    _lruList.RemoveLast();
                    _cache.Remove(lastNode.Value.Key);
                }
                
                var newItem = new CacheItem { Key = key, Value = value };
                var newNode = _lruList.AddFirst(newItem);
                _cache[key] = newNode;
            }
        }
        
        public void Clear()
        {
            _cache.Clear();
            _lruList.Clear();
            _hits = 0;
            _misses = 0;
        }
        
        public int Count => _cache.Count;
        
        public double HitRate => _hits + _misses > 0 ? (double)_hits / (_hits + _misses) : 0.0;
    }

    /// <summary>
    /// Cache performance statistics
    /// </summary>
    public class CacheStatistics
    {
        public int PathCacheSize { get; set; }
        public int MortonCacheSize { get; set; }
        public int PointCacheSize { get; set; }
        public double HitRate { get; set; }
        public DateTime LastClearTime { get; set; }
    }

    /// <summary>
    /// High-performance caching system for material inheritance chains
    /// Optimizes repeated queries to the same spatial regions
    /// </summary>
    public class MaterialInheritanceCache
    {
        private readonly Dictionary<string, MaterialCacheEntry> _pathCache;
        private readonly Dictionary<ulong, MaterialId> _mortonCache;
        private readonly LRUCache<Vector3, MaterialId> _pointCache;
        private readonly ReaderWriterLockSlim _cacheLock;
        
        public MaterialInheritanceCache(int maxEntries = 10000)
        {
            _pathCache = new Dictionary<string, MaterialCacheEntry>();
            _mortonCache = new Dictionary<ulong, MaterialId>();
            _pointCache = new LRUCache<Vector3, MaterialId>(maxEntries);
            _cacheLock = new ReaderWriterLockSlim();
        }
        
        /// <summary>
        /// Get material for specific octree path with caching
        /// </summary>
        public MaterialId GetMaterialForPath(string octreePath, OptimizedOctreeNode rootNode)
        {
            _cacheLock.EnterReadLock();
            try
            {
                if (_pathCache.TryGetValue(octreePath, out var cached) && cached.IsValid())
                {
                    return cached.Material;
                }
            }
            finally
            {
                _cacheLock.ExitReadLock();
            }
            
            // Cache miss - compute material
            var material = ComputeInheritedMaterial(octreePath, rootNode);
            
            _cacheLock.EnterWriteLock();
            try
            {
                _pathCache[octreePath] = new MaterialCacheEntry
                {
                    Material = material,
                    Timestamp = DateTime.UtcNow,
                    TTL = TimeSpan.FromMinutes(5)
                };
            }
            finally
            {
                _cacheLock.ExitWriteLock();
            }
            
            return material;
        }
        
        /// <summary>
        /// Get material for specific point with multi-level caching
        /// </summary>
        public MaterialId GetMaterialForPoint(Vector3 point, OptimizedOctreeNode rootNode, int level = 26)
        {
            // Check point cache first
            _cacheLock.EnterReadLock();
            try
            {
                if (_pointCache.TryGet(point, out var cachedMaterial))
                    return cachedMaterial;
            }
            finally
            {
                _cacheLock.ExitReadLock();
            }
            
            // Check Morton code cache
            var morton = EncodeMorton3D(point, level);
            _cacheLock.EnterReadLock();
            try
            {
                if (_mortonCache.TryGetValue(morton, out var mortonCached))
                {
                    _pointCache.Put(point, mortonCached);
                    return mortonCached;
                }
            }
            finally
            {
                _cacheLock.ExitReadLock();
            }
            
            // Cache miss - compute material
            var material = rootNode.GetMaterialAtPoint(point);
            
            _cacheLock.EnterWriteLock();
            try
            {
                _mortonCache[morton] = material;
                _pointCache.Put(point, material);
            }
            finally
            {
                _cacheLock.ExitWriteLock();
            }
            
            return material;
        }
        
        /// <summary>
        /// Invalidate cache entries for specific path prefix
        /// Used when materials are updated
        /// </summary>
        public void InvalidatePath(string pathPrefix)
        {
            _cacheLock.EnterWriteLock();
            try
            {
                var toRemove = _pathCache.Keys
                    .Where(k => k.StartsWith(pathPrefix))
                    .ToList();
                
                foreach (var key in toRemove)
                    _pathCache.Remove(key);
                
                // Clear related caches
                _mortonCache.Clear();
                _pointCache.Clear();
            }
            finally
            {
                _cacheLock.ExitWriteLock();
            }
        }
        
        /// <summary>
        /// Get cache statistics for performance monitoring
        /// </summary>
        public CacheStatistics GetStatistics()
        {
            _cacheLock.EnterReadLock();
            try
            {
                return new CacheStatistics
                {
                    PathCacheSize = _pathCache.Count,
                    MortonCacheSize = _mortonCache.Count,
                    PointCacheSize = _pointCache.Count,
                    HitRate = _pointCache.HitRate,
                    LastClearTime = DateTime.UtcNow
                };
            }
            finally
            {
                _cacheLock.ExitReadLock();
            }
        }
        
        private MaterialId ComputeInheritedMaterial(string octreePath, OptimizedOctreeNode rootNode)
        {
            var current = rootNode;
            
            foreach (char direction in octreePath)
            {
                if (current.Children == null)
                    break;
                
                var childIndex = direction - '0';
                if (childIndex >= 0 && childIndex < 8 && current.Children[childIndex] != null)
                {
                    current = current.Children[childIndex];
                }
                else
                {
                    break;
                }
            }
            
            return current.GetEffectiveMaterial();
        }
        
        private ulong EncodeMorton3D(Vector3 point, int level)
        {
            // Simplified Morton encoding for demonstration
            uint x = (uint)(point.X * 1000) & 0x1FFFFF;
            uint y = (uint)(point.Y * 1000) & 0x1FFFFF;
            uint z = (uint)(point.Z * 1000) & 0x1FFFFF;
            
            ulong result = 0;
            for (int i = 0; i < 21; i++)
            {
                result |= ((ulong)(x & (1u << i)) << (2 * i));
                result |= ((ulong)(y & (1u << i)) << (2 * i + 1));
                result |= ((ulong)(z & (1u << i)) << (2 * i + 2));
            }
            return result;
        }
        
        public void Dispose()
        {
            _cacheLock?.Dispose();
        }
    }
}
