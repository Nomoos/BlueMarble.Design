using System;
using System.Collections.Generic;

namespace BlueMarble.SpatialStorage
{
    /// <summary>
    /// High-resolution raster grid tile for dense material storage
    /// Provides O(1) lookups for levels 13+ with LRU cache tracking
    /// </summary>
    public class RasterTile
    {
        public string TileId { get; set; }
        public Envelope3D Bounds { get; set; }
        public double CellSize { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public MaterialId[,] MaterialGrid { get; set; }
        
        // Cache management
        public DateTime LastAccessed { get; set; }
        public DateTime LastModified { get; set; }
        public int AccessCount { get; set; }

        public RasterTile(string tileId, Envelope3D bounds, double cellSize, int width, int height)
        {
            TileId = tileId;
            Bounds = bounds;
            CellSize = cellSize;
            Width = width;
            Height = height;
            MaterialGrid = new MaterialId[height, width];
            LastAccessed = DateTime.UtcNow;
            LastModified = DateTime.UtcNow;
            AccessCount = 0;
        }

        /// <summary>
        /// Query material at position with O(1) complexity
        /// </summary>
        public MaterialId QueryMaterial(Vector3 position)
        {
            LastAccessed = DateTime.UtcNow;
            AccessCount++;

            if (!Bounds.Contains(position))
                throw new ArgumentException("Position outside tile bounds");

            int gridX = (int)((position.X - Bounds.MinX) / CellSize);
            int gridY = (int)((position.Y - Bounds.MinY) / CellSize);

            // Clamp to grid bounds
            gridX = Math.Max(0, Math.Min(gridX, Width - 1));
            gridY = Math.Max(0, Math.Min(gridY, Height - 1));

            return MaterialGrid[gridY, gridX];
        }

        /// <summary>
        /// Update material at position
        /// </summary>
        public void UpdateMaterial(Vector3 position, MaterialId material)
        {
            if (!Bounds.Contains(position))
                throw new ArgumentException("Position outside tile bounds");

            int gridX = (int)((position.X - Bounds.MinX) / CellSize);
            int gridY = (int)((position.Y - Bounds.MinY) / CellSize);

            gridX = Math.Max(0, Math.Min(gridX, Width - 1));
            gridY = Math.Max(0, Math.Min(gridY, Height - 1));

            MaterialGrid[gridY, gridX] = material;
            LastModified = DateTime.UtcNow;
        }

        /// <summary>
        /// Fill entire tile with a single material
        /// </summary>
        public void FillUniform(MaterialId material)
        {
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    MaterialGrid[y, x] = material;
                }
            }
            LastModified = DateTime.UtcNow;
        }

        /// <summary>
        /// Calculate homogeneity of this tile (for compression decisions)
        /// </summary>
        public double CalculateHomogeneity()
        {
            Dictionary<MaterialId, int> materialCounts = new Dictionary<MaterialId, int>();
            int totalCells = Width * Height;

            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    MaterialId mat = MaterialGrid[y, x];
                    if (!materialCounts.ContainsKey(mat))
                        materialCounts[mat] = 0;
                    materialCounts[mat]++;
                }
            }

            int maxCount = 0;
            foreach (var count in materialCounts.Values)
            {
                if (count > maxCount)
                    maxCount = count;
            }

            return (double)maxCount / totalCells;
        }

        /// <summary>
        /// Get memory size estimate in bytes
        /// </summary>
        public long GetMemorySize()
        {
            // MaterialId is 1 byte, plus overhead
            long gridSize = Width * Height * sizeof(byte);
            long overhead = 256; // Object overhead and metadata
            return gridSize + overhead;
        }

        /// <summary>
        /// Generate tile from octree data
        /// </summary>
        public static RasterTile FromOctree(string tileId, Envelope3D bounds, 
            GlobalOctree octree, double cellSize)
        {
            int width = (int)Math.Ceiling(bounds.Width / cellSize);
            int height = (int)Math.Ceiling(bounds.Height / cellSize);

            var tile = new RasterTile(tileId, bounds, cellSize, width, height);

            // Sample octree at each grid point
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    double worldX = bounds.MinX + x * cellSize + cellSize / 2;
                    double worldY = bounds.MinY + y * cellSize + cellSize / 2;
                    double worldZ = bounds.MinZ + bounds.Depth / 2;

                    Vector3 position = new Vector3(worldX, worldY, worldZ);
                    MaterialId material = octree.QueryMaterial(position);
                    tile.MaterialGrid[y, x] = material;
                }
            }

            return tile;
        }

        /// <summary>
        /// Create a tile key from position and level
        /// </summary>
        public static string CreateTileKey(Vector3 position, int level)
        {
            double tileSize = GlobalOctree.GetResolutionAtLevel(level) * 1024;
            int tileX = (int)(position.X / tileSize);
            int tileY = (int)(position.Y / tileSize);
            return $"tile_{level}_{tileX}_{tileY}";
        }

        /// <summary>
        /// Get tile statistics
        /// </summary>
        public TileStatistics GetStatistics()
        {
            Dictionary<MaterialId, int> materialCounts = new Dictionary<MaterialId, int>();
            int totalCells = Width * Height;

            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    MaterialId mat = MaterialGrid[y, x];
                    if (!materialCounts.ContainsKey(mat))
                        materialCounts[mat] = 0;
                    materialCounts[mat]++;
                }
            }

            return new TileStatistics
            {
                TileId = TileId,
                Width = Width,
                Height = Height,
                TotalCells = totalCells,
                UniqueMaterials = materialCounts.Count,
                Homogeneity = CalculateHomogeneity(),
                MemorySize = GetMemorySize(),
                AccessCount = AccessCount,
                LastAccessed = LastAccessed
            };
        }
    }

    /// <summary>
    /// Tile performance statistics
    /// </summary>
    public class TileStatistics
    {
        public string TileId { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int TotalCells { get; set; }
        public int UniqueMaterials { get; set; }
        public double Homogeneity { get; set; }
        public long MemorySize { get; set; }
        public int AccessCount { get; set; }
        public DateTime LastAccessed { get; set; }

        public override string ToString()
        {
            return $"Tile {TileId}:\n" +
                   $"  Size: {Width}x{Height} ({TotalCells} cells)\n" +
                   $"  Materials: {UniqueMaterials} (Homogeneity: {Homogeneity:P2})\n" +
                   $"  Memory: {MemorySize / 1024.0:F2} KB\n" +
                   $"  Access: {AccessCount} times, Last: {LastAccessed:yyyy-MM-dd HH:mm:ss}";
        }
    }
}
