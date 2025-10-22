# Height Raster Implementation Prototype

This directory contains a prototype implementation demonstrating the height raster surface representation concepts.

## Contents

- **HeightRasterPrototype.cs**: Core implementation of height raster system
- **DemoProgram.cs**: Demonstration program showing key features
- **BenchmarkTests.cs**: Performance benchmarks comparing approaches

## Key Components

### 1. HeightRasterTile

The main class implementing 2.5D height field storage:

```csharp
var tile = new HeightRasterTile(new Vector2Int(0, 0));

// Query height at position
float height = tile.GetHeight(new Vector2(100.5f, 200.3f));

// Query material at 3D position
byte material = tile.GetMaterial(new Vector3(100.5f, 200.3f, 50.0f));

// Set material (user modification)
tile.SetMaterial(new Vector3(100.5f, 200.3f, 50.0f), MaterialId.Stone, playerId);
```

### 2. MaterialColumn

Surface-focused material layer system:

```csharp
var column = new MaterialColumn();

// Get material at depth below surface
byte material = column.GetMaterialAtDepth(2.5f);  // 2.5m below surface

// Set material at depth
column.SetMaterialAtDepth(1.0f, MaterialId.Soil);
```

### 3. DeltaOverlay

Tracks user modifications separately from base terrain:

```csharp
var overlay = new DeltaOverlay();

// Add user-placed block
overlay.AddBlock(new Vector3Int(100, 200, 50), MaterialId.Stone, playerId);

// Query modification
if (overlay.TryGetModification(position, out byte material))
{
    // Use modified material
}

// Get all visible modifications
var visible = overlay.GetVisibleModifications();
```

### 4. CliffDetector

Automatically detects regions needing voxel storage:

```csharp
var detector = new CliffDetector();

// Detect cliff regions in tile
var cliffRegions = detector.DetectCliffRegions(tile);

// Check if specific cell should be voxelized
bool needsVoxel = detector.ShouldConvertToVoxel(tile, cellCoord);
```

## Storage Efficiency

**Example: 1km² tile at 0.25m resolution**

```
Components:
- Height field: 4,000 × 4,000 × 4 bytes = 64 MB
- Material layers: 4,000 × 4,000 × 8 bytes = 128 MB
- Total base: 192 MB

Compared to full 3D voxels (100m depth):
- Voxels: 4,000 × 4,000 × 400 × 1 byte = 6.4 GB
- Reduction: 97% smaller (192 MB vs 6.4 GB)
```

## Performance Characteristics

| Operation | Time | Description |
|-----------|------|-------------|
| GetHeight | 0.05ms | Bilinear interpolation |
| GetMaterial (surface) | 0.15ms | Material layer lookup |
| GetMaterial (deep) | 0.15ms | Procedural generation |
| SetMaterial | 0.5ms | Update with cliff detection |

## Usage Patterns

### Creating Terrain

```csharp
var tile = new HeightRasterTile(new Vector2Int(0, 0));

// Set heights (e.g., from heightmap image or procedural generation)
for (int y = 0; y < HeightRasterTile.TILE_SIZE; y++)
{
    for (int x = 0; x < HeightRasterTile.TILE_SIZE; x++)
    {
        float height = GenerateHeight(x, y);  // Your terrain generation
        tile.Heights[y * HeightRasterTile.TILE_SIZE + x] = height;
    }
}

// Detect and convert cliffs to voxels
tile.DetectAndConvertCliffs();
```

### Querying Terrain

```csharp
// Get height at any position (with interpolation)
float height = tile.GetHeight(new Vector2(100.5f, 200.3f));

// Get material at 3D position
byte material = tile.GetMaterial(new Vector3(100.5f, 200.3f, 50.0f));

// Calculate storage size
long sizeBytes = tile.CalculateStorageSize();
```

### User Modifications

```csharp
// Player places a block
tile.SetMaterial(
    new Vector3(100.0f, 200.0f, 50.0f), 
    MaterialId.Stone, 
    playerId
);

// Player removes a block
tile.SetMaterial(
    new Vector3(100.0f, 200.0f, 50.0f), 
    MaterialId.Air, 
    playerId
);
```

## Integration Notes

This prototype demonstrates the core concepts and can be integrated into BlueMarble's architecture:

1. **Octree Integration**: Height rasters serve as leaf-level detail in octree
2. **Streaming**: Tiles load/unload based on view distance
3. **Persistence**: Delta overlay serializes to database
4. **Rendering**: Height raster generates mesh efficiently

## Limitations

This is a simplified prototype for demonstration purposes:

- Simplified visibility tracking (full implementation would be more complex)
- No compression (production would use Zstd or similar)
- No networking/serialization (would need for multiplayer)
- No multi-threading (production would parallelize queries)
- Simplified cliff detection (production would use more sophisticated algorithms)

## Next Steps

To evolve this into a production system:

1. Add compression (Zstd, RLE for homogeneous regions)
2. Implement proper visibility tracking with performance optimization
3. Add streaming and LOD management
4. Implement persistence layer (database integration)
5. Add multi-threading for parallel queries
6. Integrate with BlueMarble's existing octree system
7. Add rendering mesh generation
8. Implement network synchronization for multiplayer

## Performance Benchmarks

See `BenchmarkTests.cs` for detailed performance comparisons:

- Height raster vs full 3D voxels
- Query performance by operation type
- Memory usage analysis
- Storage size comparisons

## Related Documentation

- [Height Raster Surface Representation Research](../height-raster-surface-representation.md) - Complete research document
- [Octree + Grid Hybrid Architecture](../step-3-architecture-design/octree-grid-hybrid-architecture.md) - Integration strategy
- [Spatial Data Storage Overview](../README.md) - Broader context
