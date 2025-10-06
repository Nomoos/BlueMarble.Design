using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace BlueMarble.SpatialStorage
{
    /// <summary>
    /// Hybrid storage coordinator managing intelligent transitions between octree and grid
    /// Seamlessly routes queries based on LOD: octree for levels 1-12, grid for 13+
    /// </summary>
    public class HybridStorageManager
    {
        private readonly GlobalOctree _octree;
        private readonly Dictionary<string, RasterTile> _activeTiles;
        private readonly LinkedList<string> _lruList;
        private readonly ReaderWriterLockSlim _tileLock;

        // Configuration
        private readonly int _transitionLevel;
        private readonly int _maxActiveTiles;
        private readonly double _defaultCellSize;

        // Performance tracking
        private long _octreeQueries;
        private long _gridQueries;
        private long _tileLoads;
        private long _tileEvictions;

        public const int DEFAULT_TRANSITION_LEVEL = 12;
        public const int DEFAULT_MAX_TILES = 100;
        public const double DEFAULT_CELL_SIZE = 0.25; // 25cm resolution

        public HybridStorageManager(
            int transitionLevel = DEFAULT_TRANSITION_LEVEL,
            int maxActiveTiles = DEFAULT_MAX_TILES,
            double defaultCellSize = DEFAULT_CELL_SIZE)
        {
            _transitionLevel = transitionLevel;
            _maxActiveTiles = maxActiveTiles;
            _defaultCellSize = defaultCellSize;

            _octree = new GlobalOctree(GlobalOctree.WORLD_SIZE, transitionLevel);
            _activeTiles = new Dictionary<string, RasterTile>();
            _lruList = new LinkedList<string>();
            _tileLock = new ReaderWriterLockSlim();
        }

        /// <summary>
        /// Query material with intelligent routing based on LOD
        /// </summary>
        public MaterialId QueryMaterial(Vector3 position, int lod)
        {
            // Route to octree for coarse resolution (levels 1-12)
            if (lod <= _transitionLevel)
            {
                Interlocked.Increment(ref _octreeQueries);
                return _octree.QueryMaterial(position);
            }

            // Route to grid for fine resolution (levels 13+)
            Interlocked.Increment(ref _gridQueries);
            string tileKey = RasterTile.CreateTileKey(position, lod);
            RasterTile tile = GetOrCreateTile(tileKey, position, lod);
            return tile.QueryMaterial(position);
        }

        /// <summary>
        /// Update material with automatic routing
        /// </summary>
        public void UpdateMaterial(Vector3 position, MaterialId material, int lod)
        {
            // Route to octree for coarse resolution
            if (lod <= _transitionLevel)
            {
                _octree.UpdateMaterial(position, material);
                return;
            }

            // Route to grid for fine resolution
            string tileKey = RasterTile.CreateTileKey(position, lod);
            RasterTile tile = GetOrCreateTile(tileKey, position, lod);
            tile.UpdateMaterial(position, material);
        }

        /// <summary>
        /// Query region across multiple LODs
        /// </summary>
        public List<(Vector3 position, MaterialId material)> QueryRegion(
            Envelope3D region, int lod, double resolution)
        {
            var results = new List<(Vector3, MaterialId)>();

            for (double x = region.MinX; x < region.MaxX; x += resolution)
            {
                for (double y = region.MinY; y < region.MaxY; y += resolution)
                {
                    Vector3 position = new Vector3(x, y, region.MinZ);
                    if (region.Contains(position))
                    {
                        MaterialId material = QueryMaterial(position, lod);
                        results.Add((position, material));
                    }
                }
            }

            return results;
        }

        /// <summary>
        /// Initialize large homogeneous region (ocean, air, etc.)
        /// Demonstrates memory efficiency through material inheritance
        /// </summary>
        public void InitializeHomogeneousRegion(Envelope3D region, MaterialId material)
        {
            _octree.InitializeOceanRegion(region);
        }

        /// <summary>
        /// Preload tiles for a region (predictive loading)
        /// </summary>
        public void PreloadTiles(Envelope3D region, int lod)
        {
            double tileSize = GlobalOctree.GetResolutionAtLevel(lod) * 1024;

            for (double x = region.MinX; x < region.MaxX; x += tileSize)
            {
                for (double y = region.MinY; y < region.MaxY; y += tileSize)
                {
                    Vector3 position = new Vector3(x, y, region.MinZ);
                    string tileKey = RasterTile.CreateTileKey(position, lod);
                    GetOrCreateTile(tileKey, position, lod);
                }
            }
        }

        /// <summary>
        /// Evict least recently used tiles to free memory
        /// </summary>
        public int EvictLRUTiles(int count)
        {
            int evicted = 0;

            _tileLock.EnterWriteLock();
            try
            {
                while (evicted < count && _lruList.Count > 0)
                {
                    string tileKey = _lruList.First.Value;
                    _lruList.RemoveFirst();
                    
                    if (_activeTiles.Remove(tileKey))
                    {
                        evicted++;
                        Interlocked.Increment(ref _tileEvictions);
                    }
                }
            }
            finally
            {
                _tileLock.ExitWriteLock();
            }

            return evicted;
        }

        /// <summary>
        /// Clear all cached tiles
        /// </summary>
        public void ClearTileCache()
        {
            _tileLock.EnterWriteLock();
            try
            {
                int count = _activeTiles.Count;
                _activeTiles.Clear();
                _lruList.Clear();
                Interlocked.Add(ref _tileEvictions, count);
            }
            finally
            {
                _tileLock.ExitWriteLock();
            }
        }

        /// <summary>
        /// Get comprehensive performance statistics
        /// </summary>
        public HybridStatistics GetStatistics()
        {
            var octreeStats = _octree.GetStatistics();

            _tileLock.EnterReadLock();
            try
            {
                long totalTileCells = 0;
                double avgHomogeneity = 0;
                long totalTileMemory = 0;

                foreach (var tile in _activeTiles.Values)
                {
                    totalTileCells += tile.Width * tile.Height;
                    avgHomogeneity += tile.CalculateHomogeneity();
                    totalTileMemory += tile.GetMemorySize();
                }

                if (_activeTiles.Count > 0)
                    avgHomogeneity /= _activeTiles.Count;

                return new HybridStatistics
                {
                    // Octree stats
                    OctreeNodes = octreeStats.TotalNodes,
                    OctreeMemorySavings = octreeStats.MemorySavingsPercent,
                    OctreeQueries = _octreeQueries,
                    OctreeCacheHitRate = octreeStats.CacheHitRate,

                    // Grid stats
                    ActiveTiles = _activeTiles.Count,
                    TotalTileCells = totalTileCells,
                    AverageTileHomogeneity = avgHomogeneity,
                    TotalTileMemory = totalTileMemory,
                    GridQueries = _gridQueries,

                    // Hybrid stats
                    TransitionLevel = _transitionLevel,
                    TotalQueries = _octreeQueries + _gridQueries,
                    OctreeQueryPercent = CalculatePercent(_octreeQueries, _octreeQueries + _gridQueries),
                    GridQueryPercent = CalculatePercent(_gridQueries, _octreeQueries + _gridQueries),
                    TileLoads = _tileLoads,
                    TileEvictions = _tileEvictions
                };
            }
            finally
            {
                _tileLock.ExitReadLock();
            }
        }

        /// <summary>
        /// Get or create tile with LRU cache management
        /// </summary>
        private RasterTile GetOrCreateTile(string tileKey, Vector3 position, int lod)
        {
            // Try to get existing tile
            _tileLock.EnterUpgradeableReadLock();
            try
            {
                if (_activeTiles.TryGetValue(tileKey, out RasterTile existingTile))
                {
                    // Move to end of LRU list
                    _tileLock.EnterWriteLock();
                    try
                    {
                        _lruList.Remove(tileKey);
                        _lruList.AddLast(tileKey);
                    }
                    finally
                    {
                        _tileLock.ExitWriteLock();
                    }

                    return existingTile;
                }

                // Need to create new tile
                _tileLock.EnterWriteLock();
                try
                {
                    // Check if we need to evict tiles
                    if (_activeTiles.Count >= _maxActiveTiles)
                    {
                        EvictLRUTiles(_maxActiveTiles / 10); // Evict 10%
                    }

                    // Create tile bounds
                    double tileSize = GlobalOctree.GetResolutionAtLevel(lod) * 1024;
                    double tileX = Math.Floor(position.X / tileSize) * tileSize;
                    double tileY = Math.Floor(position.Y / tileSize) * tileSize;

                    Envelope3D bounds = new Envelope3D(
                        tileX, tileY, -1000,
                        tileX + tileSize, tileY + tileSize, 1000
                    );

                    // Create tile from octree
                    RasterTile newTile = RasterTile.FromOctree(tileKey, bounds, _octree, _defaultCellSize);

                    _activeTiles[tileKey] = newTile;
                    _lruList.AddLast(tileKey);
                    Interlocked.Increment(ref _tileLoads);

                    return newTile;
                }
                finally
                {
                    _tileLock.ExitWriteLock();
                }
            }
            finally
            {
                _tileLock.ExitUpgradeableReadLock();
            }
        }

        private double CalculatePercent(long part, long total)
        {
            return total > 0 ? (double)part / total * 100.0 : 0.0;
        }
    }

    /// <summary>
    /// Comprehensive hybrid storage statistics
    /// </summary>
    public class HybridStatistics
    {
        // Octree statistics
        public int OctreeNodes { get; set; }
        public double OctreeMemorySavings { get; set; }
        public long OctreeQueries { get; set; }
        public double OctreeCacheHitRate { get; set; }

        // Grid statistics
        public int ActiveTiles { get; set; }
        public long TotalTileCells { get; set; }
        public double AverageTileHomogeneity { get; set; }
        public long TotalTileMemory { get; set; }
        public long GridQueries { get; set; }

        // Hybrid statistics
        public int TransitionLevel { get; set; }
        public long TotalQueries { get; set; }
        public double OctreeQueryPercent { get; set; }
        public double GridQueryPercent { get; set; }
        public long TileLoads { get; set; }
        public long TileEvictions { get; set; }

        public override string ToString()
        {
            return $"Hybrid Storage Statistics:\n" +
                   $"\nOctree (Levels 1-{TransitionLevel}):\n" +
                   $"  Nodes: {OctreeNodes}\n" +
                   $"  Memory Savings: {OctreeMemorySavings:F1}%\n" +
                   $"  Queries: {OctreeQueries} ({OctreeQueryPercent:F1}%)\n" +
                   $"  Cache Hit Rate: {OctreeCacheHitRate:P2}\n" +
                   $"\nGrid (Levels {TransitionLevel + 1}+):\n" +
                   $"  Active Tiles: {ActiveTiles}\n" +
                   $"  Total Cells: {TotalTileCells:N0}\n" +
                   $"  Avg Homogeneity: {AverageTileHomogeneity:P2}\n" +
                   $"  Memory: {TotalTileMemory / (1024.0 * 1024.0):F2} MB\n" +
                   $"  Queries: {GridQueries} ({GridQueryPercent:F1}%)\n" +
                   $"\nCache Management:\n" +
                   $"  Tile Loads: {TileLoads}\n" +
                   $"  Tile Evictions: {TileEvictions}\n" +
                   $"  Total Queries: {TotalQueries}";
        }
    }
}
