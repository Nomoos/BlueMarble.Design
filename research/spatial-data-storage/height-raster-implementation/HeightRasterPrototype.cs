using System;
using System.Collections.Generic;
using System.Linq;

namespace BlueMarble.SpatialData.HeightRaster
{
    /// <summary>
    /// Prototype implementation of height raster surface representation
    /// Demonstrates key concepts: height field, material layers, cliff detection, delta overlay
    /// </summary>
    public class HeightRasterTile
    {
        // Configuration constants
        public const int TILE_SIZE = 1024;           // 1024x1024 cells per tile
        public const float CELL_SIZE = 0.25f;        // 0.25m per cell (25cm resolution)
        public const int MAX_MATERIAL_LAYERS = 8;    // 8 material layers from surface
        
        // Tile identification
        public Vector2Int TileCoordinate { get; set; }
        public Envelope2D Bounds { get; set; }
        
        // Height field - primary data
        public float[] Heights { get; private set; }
        
        // Material columns - one per cell
        public MaterialColumn[] MaterialColumns { get; private set; }
        
        // Delta overlay - tracks user modifications
        public DeltaOverlay UserModifications { get; private set; }
        
        // Voxel regions - for cliffs and overhangs
        public Dictionary<Vector2Int, VoxelColumn> VoxelRegions { get; private set; }
        
        // Cliff detector
        private CliffDetector _cliffDetector;
        
        public HeightRasterTile(Vector2Int coordinate)
        {
            TileCoordinate = coordinate;
            Bounds = CalculateBounds(coordinate);
            
            Heights = new float[TILE_SIZE * TILE_SIZE];
            MaterialColumns = new MaterialColumn[TILE_SIZE * TILE_SIZE];
            
            for (int i = 0; i < MaterialColumns.Length; i++)
            {
                MaterialColumns[i] = new MaterialColumn();
            }
            
            UserModifications = new DeltaOverlay();
            VoxelRegions = new Dictionary<Vector2Int, VoxelColumn>();
            _cliffDetector = new CliffDetector();
        }
        
        /// <summary>
        /// Get height at world position using bilinear interpolation
        /// </summary>
        public float GetHeight(Vector2 worldPos)
        {
            Vector2 localPos = WorldToLocal(worldPos);
            
            if (!IsInBounds(localPos))
                return float.NaN;
            
            // Get surrounding cell indices
            int x0 = (int)Math.Floor(localPos.X);
            int y0 = (int)Math.Floor(localPos.Y);
            int x1 = Math.Min(x0 + 1, TILE_SIZE - 1);
            int y1 = Math.Min(y0 + 1, TILE_SIZE - 1);
            
            // Bilinear interpolation weights
            float fx = localPos.X - x0;
            float fy = localPos.Y - y0;
            
            // Sample height values
            float h00 = Heights[y0 * TILE_SIZE + x0];
            float h10 = Heights[y0 * TILE_SIZE + x1];
            float h01 = Heights[y1 * TILE_SIZE + x0];
            float h11 = Heights[y1 * TILE_SIZE + x1];
            
            // Interpolate
            float h0 = h00 * (1 - fx) + h10 * fx;
            float h1 = h01 * (1 - fx) + h11 * fx;
            return h0 * (1 - fy) + h1 * fy;
        }
        
        /// <summary>
        /// Get material at 3D world position
        /// Routes to height raster, delta overlay, or voxel regions as appropriate
        /// </summary>
        public byte GetMaterial(Vector3 worldPos)
        {
            Vector2Int cellCoord = WorldToCell(worldPos.XY);
            Vector3Int voxelPos = WorldToVoxel(worldPos);
            
            // Check delta overlay first (user modifications)
            if (UserModifications.TryGetModification(voxelPos, out byte userMaterial))
            {
                return userMaterial;
            }
            
            // Check if this region has voxel storage (cliff/cave)
            if (VoxelRegions.TryGetValue(cellCoord, out var voxelColumn))
            {
                return voxelColumn.GetMaterial(worldPos);
            }
            
            // Use height raster for normal terrain
            float surfaceHeight = GetHeight(worldPos.XY);
            float depthBelowSurface = surfaceHeight - worldPos.Z;
            
            if (depthBelowSurface < 0)
            {
                return MaterialId.Air;  // Above surface
            }
            else
            {
                // Query material layer system
                var materialColumn = MaterialColumns[cellCoord.Y * TILE_SIZE + cellCoord.X];
                return materialColumn.GetMaterialAtDepth(depthBelowSurface);
            }
        }
        
        /// <summary>
        /// Set material at position - handles surface updates and modifications
        /// </summary>
        public void SetMaterial(Vector3 worldPos, byte materialId, Guid? playerId = null)
        {
            Vector2Int cellCoord = WorldToCell(worldPos.XY);
            float surfaceHeight = GetHeight(worldPos.XY);
            float depthBelowSurface = surfaceHeight - worldPos.Z;
            
            // Case 1: Modifying surface layer (within material layer depth)
            if (depthBelowSurface >= 0 && depthBelowSurface <= MaterialColumn.MAX_STORED_DEPTH)
            {
                var materialColumn = MaterialColumns[cellCoord.Y * TILE_SIZE + cellCoord.X];
                materialColumn.SetMaterialAtDepth(depthBelowSurface, materialId);
                
                // Update height if removing surface block
                if (Math.Abs(depthBelowSurface) < 0.01f && materialId == MaterialId.Air)
                {
                    // Surface block removed - lower height
                    Heights[cellCoord.Y * TILE_SIZE + cellCoord.X] -= CELL_SIZE;
                }
            }
            // Case 2: Building above surface or digging deep
            else
            {
                // Store in delta overlay (user-placed blocks)
                Vector3Int voxelPos = WorldToVoxel(worldPos);
                UserModifications.AddBlock(voxelPos, materialId, playerId ?? Guid.Empty);
            }
            
            // Check if region needs voxel conversion (steep cliff created)
            if (_cliffDetector.ShouldConvertToVoxel(this, cellCoord))
            {
                ConvertRegionToVoxels(cellCoord);
            }
        }
        
        /// <summary>
        /// Detect cliffs and convert to voxel storage
        /// </summary>
        public void DetectAndConvertCliffs()
        {
            var cliffRegions = _cliffDetector.DetectCliffRegions(this);
            
            foreach (var region in cliffRegions)
            {
                ConvertRegionToVoxels(region.CellCoordinate);
            }
        }
        
        /// <summary>
        /// Convert a region from height raster to voxel storage
        /// </summary>
        private void ConvertRegionToVoxels(Vector2Int cellCoord)
        {
            if (VoxelRegions.ContainsKey(cellCoord))
                return;  // Already converted
            
            float height = Heights[cellCoord.Y * TILE_SIZE + cellCoord.X];
            var materialColumn = MaterialColumns[cellCoord.Y * TILE_SIZE + cellCoord.X];
            
            // Create voxel column
            var voxelColumn = new VoxelColumn(cellCoord, height, materialColumn);
            VoxelRegions[cellCoord] = voxelColumn;
        }
        
        /// <summary>
        /// Calculate storage size in bytes
        /// </summary>
        public long CalculateStorageSize()
        {
            long size = 0;
            
            // Height field: 4 bytes per height
            size += TILE_SIZE * TILE_SIZE * 4;
            
            // Material columns: 8 bytes per column (8 layers × 1 byte)
            size += TILE_SIZE * TILE_SIZE * 8;
            
            // Delta overlay: ~25 bytes per modification
            size += UserModifications.GetModificationCount() * 25;
            
            // Voxel regions: ~400 bytes per column
            size += VoxelRegions.Count * 400;
            
            return size;
        }
        
        // Helper methods
        private Vector2 WorldToLocal(Vector2 worldPos)
        {
            return new Vector2(
                (worldPos.X - Bounds.MinX) / CELL_SIZE,
                (worldPos.Y - Bounds.MinY) / CELL_SIZE
            );
        }
        
        private Vector2Int WorldToCell(Vector2 worldPos)
        {
            var local = WorldToLocal(worldPos);
            return new Vector2Int(
                (int)Math.Floor(local.X),
                (int)Math.Floor(local.Y)
            );
        }
        
        private Vector3Int WorldToVoxel(Vector3 worldPos)
        {
            return new Vector3Int(
                (int)Math.Floor(worldPos.X / CELL_SIZE),
                (int)Math.Floor(worldPos.Y / CELL_SIZE),
                (int)Math.Floor(worldPos.Z / CELL_SIZE)
            );
        }
        
        private bool IsInBounds(Vector2 localPos)
        {
            return localPos.X >= 0 && localPos.X < TILE_SIZE &&
                   localPos.Y >= 0 && localPos.Y < TILE_SIZE;
        }
        
        private Envelope2D CalculateBounds(Vector2Int coordinate)
        {
            float worldSize = TILE_SIZE * CELL_SIZE;
            return new Envelope2D
            {
                MinX = coordinate.X * worldSize,
                MaxX = (coordinate.X + 1) * worldSize,
                MinY = coordinate.Y * worldSize,
                MaxY = (coordinate.Y + 1) * worldSize
            };
        }
    }
    
    /// <summary>
    /// Material column storing surface material layers
    /// Only stores visible/near-surface materials - deep materials are procedural
    /// </summary>
    public class MaterialColumn
    {
        private const int LAYER_COUNT = 8;
        public const float MAX_STORED_DEPTH = 20.0f;  // Store top 20m only
        
        private static readonly float[] LAYER_DEPTHS = { 
            0.0f,    // Surface
            0.25f,   // 25cm down
            0.5f,    // 50cm down
            1.0f,    // 1m down
            2.0f,    // 2m down
            5.0f,    // 5m down
            10.0f,   // 10m down
            20.0f    // 20m down
        };
        
        public byte[] Materials { get; private set; } = new byte[LAYER_COUNT];
        
        public MaterialColumn()
        {
            // Initialize with default materials
            Materials[0] = MaterialId.Grass;
            for (int i = 1; i < 4; i++)
                Materials[i] = MaterialId.Soil;
            for (int i = 4; i < LAYER_COUNT; i++)
                Materials[i] = MaterialId.Stone;
        }
        
        /// <summary>
        /// Get material at specific depth below surface
        /// For depths beyond stored layers, use procedural generation
        /// </summary>
        public byte GetMaterialAtDepth(float depthBelowSurface)
        {
            if (depthBelowSurface < 0)
                return MaterialId.Air;  // Above surface
            
            // Find layer depth bracket
            for (int i = 0; i < LAYER_COUNT - 1; i++)
            {
                if (depthBelowSurface < LAYER_DEPTHS[i + 1])
                {
                    return Materials[i];
                }
            }
            
            // Deeper than stored layers - use procedural generation
            return GenerateDeepMaterial(depthBelowSurface);
        }
        
        /// <summary>
        /// Set material at specific depth
        /// </summary>
        public void SetMaterialAtDepth(float depthBelowSurface, byte materialId)
        {
            // Find closest layer
            int closestLayer = 0;
            float minDiff = float.MaxValue;
            
            for (int i = 0; i < LAYER_COUNT; i++)
            {
                float diff = Math.Abs(depthBelowSurface - LAYER_DEPTHS[i]);
                if (diff < minDiff)
                {
                    minDiff = diff;
                    closestLayer = i;
                }
            }
            
            Materials[closestLayer] = materialId;
        }
        
        /// <summary>
        /// Procedural generation for deep materials based on geology
        /// </summary>
        private byte GenerateDeepMaterial(float depth)
        {
            // Simple layered geology model
            if (depth < 50)
                return MaterialId.Soil;
            else if (depth < 200)
                return MaterialId.Bedrock;
            else if (depth < 1000)
                return MaterialId.Stone;
            else
                return MaterialId.DeepRock;
        }
    }
    
    /// <summary>
    /// Delta overlay - tracks user modifications separately from base terrain
    /// </summary>
    public class DeltaOverlay
    {
        private Dictionary<Vector3Int, BlockModification> _modifications;
        
        public struct BlockModification
        {
            public byte MaterialId;
            public DateTime PlacedTime;
            public Guid PlayerId;
            public bool IsVisible;
        }
        
        public DeltaOverlay()
        {
            _modifications = new Dictionary<Vector3Int, BlockModification>();
        }
        
        public void AddBlock(Vector3Int position, byte materialId, Guid playerId)
        {
            var modification = new BlockModification
            {
                MaterialId = materialId,
                PlacedTime = DateTime.UtcNow,
                PlayerId = playerId,
                IsVisible = true  // Simplified - would need visibility check
            };
            
            _modifications[position] = modification;
        }
        
        public void RemoveBlock(Vector3Int position)
        {
            _modifications.Remove(position);
        }
        
        public bool TryGetModification(Vector3Int position, out byte materialId)
        {
            if (_modifications.TryGetValue(position, out var mod))
            {
                materialId = mod.MaterialId;
                return true;
            }
            
            materialId = 0;
            return false;
        }
        
        public int GetModificationCount()
        {
            return _modifications.Count;
        }
        
        public IEnumerable<BlockModification> GetVisibleModifications()
        {
            return _modifications.Values.Where(m => m.IsVisible);
        }
    }
    
    /// <summary>
    /// Voxel column for regions that can't be represented by height raster
    /// Used for cliffs, overhangs, caves
    /// </summary>
    public class VoxelColumn
    {
        private Vector2Int _cellCoordinate;
        private float _baseHeight;
        private Dictionary<int, byte> _voxels;  // Z coordinate -> material
        
        public VoxelColumn(Vector2Int cellCoord, float baseHeight, MaterialColumn baseColumn)
        {
            _cellCoordinate = cellCoord;
            _baseHeight = baseHeight;
            _voxels = new Dictionary<int, byte>();
            
            // Initialize from material column
            InitializeFromMaterialColumn(baseColumn);
        }
        
        public byte GetMaterial(Vector3 worldPos)
        {
            int z = (int)Math.Floor(worldPos.Z / HeightRasterTile.CELL_SIZE);
            
            if (_voxels.TryGetValue(z, out byte material))
                return material;
            
            return MaterialId.Air;
        }
        
        private void InitializeFromMaterialColumn(MaterialColumn column)
        {
            // Convert material column to voxel representation
            for (int z = 0; z < 100; z++)  // 100 voxels = 25m height
            {
                float worldZ = _baseHeight - z * HeightRasterTile.CELL_SIZE;
                float depth = _baseHeight - worldZ;
                
                byte material = column.GetMaterialAtDepth(depth);
                if (material != MaterialId.Air)
                {
                    _voxels[z] = material;
                }
            }
        }
    }
    
    /// <summary>
    /// Cliff detector - identifies regions that need voxel storage
    /// </summary>
    public class CliffDetector
    {
        private const float VERTICAL_CLIFF_THRESHOLD = 70.0f;  // degrees
        
        public struct CliffRegion
        {
            public Vector2Int CellCoordinate;
            public float Slope;
        }
        
        public List<CliffRegion> DetectCliffRegions(HeightRasterTile tile)
        {
            var cliffRegions = new List<CliffRegion>();
            
            for (int y = 0; y < HeightRasterTile.TILE_SIZE - 1; y++)
            {
                for (int x = 0; x < HeightRasterTile.TILE_SIZE - 1; x++)
                {
                    float slope = CalculateSlope(tile, x, y);
                    
                    if (slope > VERTICAL_CLIFF_THRESHOLD)
                    {
                        cliffRegions.Add(new CliffRegion
                        {
                            CellCoordinate = new Vector2Int(x, y),
                            Slope = slope
                        });
                    }
                }
            }
            
            return cliffRegions;
        }
        
        public bool ShouldConvertToVoxel(HeightRasterTile tile, Vector2Int cellCoord)
        {
            // Check if already converted
            if (tile.VoxelRegions.ContainsKey(cellCoord))
                return false;
            
            // Check slope
            float slope = CalculateSlope(tile, cellCoord.X, cellCoord.Y);
            return slope > VERTICAL_CLIFF_THRESHOLD;
        }
        
        private float CalculateSlope(HeightRasterTile tile, int x, int y)
        {
            if (x >= HeightRasterTile.TILE_SIZE - 1 || y >= HeightRasterTile.TILE_SIZE - 1)
                return 0;
            
            float h0 = tile.Heights[y * HeightRasterTile.TILE_SIZE + x];
            float h1 = tile.Heights[y * HeightRasterTile.TILE_SIZE + (x + 1)];
            float h2 = tile.Heights[(y + 1) * HeightRasterTile.TILE_SIZE + x];
            
            // Calculate slope in both directions
            float slopeX = Math.Abs(h1 - h0) / HeightRasterTile.CELL_SIZE;
            float slopeY = Math.Abs(h2 - h0) / HeightRasterTile.CELL_SIZE;
            
            // Convert to angle
            float maxSlope = Math.Max(slopeX, slopeY);
            return (float)(Math.Atan(maxSlope) * 180.0 / Math.PI);
        }
    }
    
    // Supporting types
    public static class MaterialId
    {
        public const byte Air = 0;
        public const byte Grass = 1;
        public const byte Soil = 2;
        public const byte Stone = 3;
        public const byte Bedrock = 4;
        public const byte DeepRock = 5;
        public const byte Sand = 6;
        public const byte Water = 7;
    }
    
    public struct Vector2
    {
        public float X, Y;
        public Vector2(float x, float y) { X = x; Y = y; }
    }
    
    public struct Vector2Int
    {
        public int X, Y;
        public Vector2Int(int x, int y) { X = x; Y = y; }
    }
    
    public struct Vector3
    {
        public float X, Y, Z;
        public Vector3(float x, float y, float z) { X = x; Y = y; Z = z; }
        public Vector2 XY => new Vector2(X, Y);
    }
    
    public struct Vector3Int
    {
        public int X, Y, Z;
        public Vector3Int(int x, int y, int z) { X = x; Y = y; Z = z; }
    }
    
    public struct Envelope2D
    {
        public float MinX, MaxX, MinY, MaxY;
    }
}
