# Usage Examples for Octree Material Inheritance

## Quick Start

### Building and Running

```bash
cd research/spatial-data-storage/octree-material-inheritance
dotnet build
dotnet run
```

This will run the verification program with 5 demos and the full test suite.

## Basic Usage Examples

### Example 1: Creating a Simple Octree

```csharp
using BlueMarble.SpatialStorage.Research;

// Define spatial bounds (in meters)
var bounds = new BoundingBox3D(
    new Vector3(0, 0, 0),      // min corner
    new Vector3(1000, 1000, 1000)  // max corner (1km³)
);

// Create octree
var octree = new OptimizedOctree(bounds);

// Query material at a point
var material = octree.QueryMaterial(500, 500, 500);
Console.WriteLine($"Material: {material}"); // Output: Air (default)
```

### Example 2: Setting Materials at Points

```csharp
// Set material at specific point
var point = new Vector3(100, 100, 100);
octree.SetMaterial(point, MaterialId.Water);

// Query to verify
var queriedMaterial = octree.QueryMaterial(point);
Console.WriteLine($"Material at point: {queriedMaterial}"); // Output: Water
```

### Example 3: Setting Materials for Regions

```csharp
// Define ocean region
var oceanRegion = new BoundingBox3D(
    new Vector3(0, 0, 0),
    new Vector3(1000, 1000, 500)
);

// Set entire region to water (max depth 10 levels)
octree.SetRegion(oceanRegion, MaterialId.Water, maxDepth: 10);

// Define land region above
var landRegion = new BoundingBox3D(
    new Vector3(0, 0, 500),
    new Vector3(1000, 1000, 1000)
);

// Set land region to dirt
octree.SetRegion(landRegion, MaterialId.Dirt, maxDepth: 10);
```

### Example 4: Material Inheritance in Action

```csharp
// Create a node hierarchy manually
var root = new OptimizedOctreeNode 
{ 
    ExplicitMaterial = MaterialId.Water,
    Level = 0,
    Bounds = new BoundingBox3D(new Vector3(0,0,0), new Vector3(1000,1000,1000))
};

var child = new OptimizedOctreeNode 
{ 
    Parent = root,
    Level = 1
    // Note: ExplicitMaterial is null, so it will inherit
};

// Child inherits from parent
var effectiveMaterial = child.GetEffectiveMaterial();
Console.WriteLine($"Child material: {effectiveMaterial}"); // Output: Water
```

### Example 5: Checking Memory Statistics

```csharp
// Create and populate octree
var octree = new OptimizedOctree(
    new BoundingBox3D(new Vector3(0,0,0), new Vector3(10000,10000,1000))
);

octree.SetRegion(
    new BoundingBox3D(new Vector3(0,0,0), new Vector3(10000,10000,1000)),
    MaterialId.Water,
    maxDepth: 8
);

// Get memory statistics
var stats = octree.CalculateMemoryStatistics();

Console.WriteLine($"Total Nodes: {stats.TotalNodes}");
Console.WriteLine($"Nodes with Explicit Material: {stats.NodesWithExplicitMaterial}");
Console.WriteLine($"Nodes Inheriting Material: {stats.NodesInheritingMaterial}");
Console.WriteLine($"Inheritance Efficiency: {stats.InheritanceEfficiencyRatio:P1}");
Console.WriteLine($"Estimated Memory: {stats.EstimatedMemoryBytes:N0} bytes");

// Material distribution
Console.WriteLine("\nMaterial Distribution:");
foreach (var kvp in stats.MaterialDistribution)
{
    Console.WriteLine($"  {kvp.Key}: {kvp.Value} nodes");
}
```

### Example 6: Memory Optimization

```csharp
// Create octree with uniform regions
var octree = new OptimizedOctree(
    new BoundingBox3D(new Vector3(0,0,0), new Vector3(5000,5000,1000))
);

// Set large uniform region
octree.SetRegion(
    new BoundingBox3D(new Vector3(0,0,0), new Vector3(5000,5000,1000)),
    MaterialId.Water,
    maxDepth: 8
);

// Before optimization
var statsBefore = octree.CalculateMemoryStatistics();
Console.WriteLine($"Before: {statsBefore.TotalNodes} nodes");

// Optimize by collapsing homogeneous regions
octree.OptimizeMemory();

// After optimization
var statsAfter = octree.CalculateMemoryStatistics();
Console.WriteLine($"After: {statsAfter.TotalNodes} nodes");
Console.WriteLine($"Collapsed: {statsAfter.CollapsedNodes} nodes");
```

### Example 7: Node Collapse and Expand

```csharp
// Create node with uniform children
var node = new OptimizedOctreeNode
{
    Children = new OptimizedOctreeNode[8],
    ChildMaterialCounts = { [MaterialId.Air] = 8 },
    Bounds = new BoundingBox3D(new Vector3(0,0,0), new Vector3(100,100,100))
};

// Initialize children with same material
for (int i = 0; i < 8; i++)
{
    node.Children[i] = new OptimizedOctreeNode 
    { 
        ExplicitMaterial = MaterialId.Air,
        Parent = node,
        Level = 1
    };
}

// Try to collapse (threshold 90%)
bool collapsed = node.TryCollapse(0.9);
Console.WriteLine($"Collapsed: {collapsed}"); // Output: True

// Expand again if needed
node.Expand();
Console.WriteLine($"Has children: {node.Children != null}"); // Output: True
```

### Example 8: Working with Cache

```csharp
var octree = new OptimizedOctree(
    new BoundingBox3D(new Vector3(0,0,0), new Vector3(1000,1000,1000))
);

// Set up region
octree.SetRegion(
    new BoundingBox3D(new Vector3(0,0,0), new Vector3(500,500,500)),
    MaterialId.Water,
    maxDepth: 8
);

// First query (cache miss)
var point = new Vector3(250, 250, 250);
var material1 = octree.QueryMaterial(point);

// Second query (cache hit - much faster)
var material2 = octree.QueryMaterial(point);

// Get cache statistics
var cacheStats = octree.GetCacheStatistics();
Console.WriteLine($"Cache Hit Rate: {cacheStats.HitRate:P1}");
Console.WriteLine($"Point Cache Size: {cacheStats.PointCacheSize}");
Console.WriteLine($"Morton Cache Size: {cacheStats.MortonCacheSize}");
Console.WriteLine($"Path Cache Size: {cacheStats.PathCacheSize}");
```

## Real-World Scenarios

### Scenario 1: Ocean with Seamounts

```csharp
// Create large ocean volume (10km × 10km × 1km deep)
var bounds = new BoundingBox3D(new Vector3(0,0,-1000), new Vector3(10000,10000,0));
var octree = new OptimizedOctree(bounds);

// Set entire volume to water
octree.SetRegion(bounds, MaterialId.Water, maxDepth: 8);

// Add underwater seamounts (volcanic mountains)
var seamounts = new[]
{
    new BoundingBox3D(new Vector3(2000,2000,-500), new Vector3(2200,2200,-100)),
    new BoundingBox3D(new Vector3(7000,7000,-700), new Vector3(7300,7300,-200)),
    new BoundingBox3D(new Vector3(5000,5000,-600), new Vector3(5100,5100,-50))
};

foreach (var seamount in seamounts)
{
    octree.SetRegion(seamount, MaterialId.Rock, maxDepth: 8);
}

// Optimize memory
octree.OptimizeMemory();

// Query some points
Console.WriteLine($"Deep ocean: {octree.QueryMaterial(1000, 1000, -800)}"); // Water
Console.WriteLine($"Seamount: {octree.QueryMaterial(2100, 2100, -300)}");   // Rock
Console.WriteLine($"Near surface: {octree.QueryMaterial(5000, 5000, -10)}"); // Water

var stats = octree.CalculateMemoryStatistics();
Console.WriteLine($"\nOcean with seamounts:");
Console.WriteLine($"  Inheritance Efficiency: {stats.InheritanceEfficiencyRatio:P0}");
Console.WriteLine($"  Memory: {stats.EstimatedMemoryBytes:N0} bytes");
```

### Scenario 2: Stratified Geological Layers

```csharp
// Create region for geological column (1km × 1km × 500m deep)
var bounds = new BoundingBox3D(new Vector3(0,0,0), new Vector3(1000,1000,500));
var octree = new OptimizedOctree(bounds);

// Layer 1: Top soil (0-10m)
octree.SetRegion(
    new BoundingBox3D(new Vector3(0,0,0), new Vector3(1000,1000,10)),
    MaterialId.Dirt,
    maxDepth: 10
);

// Layer 2: Clay layer (10-50m)
octree.SetRegion(
    new BoundingBox3D(new Vector3(0,0,10), new Vector3(1000,1000,50)),
    MaterialId.Clay,
    maxDepth: 10
);

// Layer 3: Limestone (50-200m)
octree.SetRegion(
    new BoundingBox3D(new Vector3(0,0,50), new Vector3(1000,1000,200)),
    MaterialId.Limestone,
    maxDepth: 10
);

// Layer 4: Granite bedrock (200-500m)
octree.SetRegion(
    new BoundingBox3D(new Vector3(0,0,200), new Vector3(1000,1000,500)),
    MaterialId.Granite,
    maxDepth: 10
);

// Add intrusive rock formation cutting through layers
octree.SetRegion(
    new BoundingBox3D(new Vector3(400,400,0), new Vector3(600,600,300)),
    MaterialId.Rock,
    maxDepth: 10
);

// Query vertical profile
Console.WriteLine("Vertical profile at (500, 500, depth):");
for (int depth = 5; depth <= 400; depth += 50)
{
    var material = octree.QueryMaterial(500, 500, depth);
    Console.WriteLine($"  {depth}m: {material}");
}

octree.OptimizeMemory();
var stats = octree.CalculateMemoryStatistics();
Console.WriteLine($"\nGeological layers:");
Console.WriteLine($"  Total Nodes: {stats.TotalNodes}");
Console.WriteLine($"  Materials: {stats.MaterialDistribution.Count} types");
```

### Scenario 3: Coastal Transition Zone

```csharp
// Create coastal region (5km × 5km × 500m)
var bounds = new BoundingBox3D(new Vector3(0,0,-250), new Vector3(5000,5000,250));
var octree = new OptimizedOctree(bounds);

// Ocean half (0-2500m)
octree.SetRegion(
    new BoundingBox3D(new Vector3(0,0,-250), new Vector3(2500,5000,0)),
    MaterialId.Water,
    maxDepth: 8
);

// Beach (2500-2700m) - sand from -10m to +10m
octree.SetRegion(
    new BoundingBox3D(new Vector3(2500,0,-10), new Vector3(2700,5000,10)),
    MaterialId.Sand,
    maxDepth: 8
);

// Land (2700-5000m) - layered
octree.SetRegion(
    new BoundingBox3D(new Vector3(2700,0,0), new Vector3(5000,5000,10)),
    MaterialId.Dirt,
    maxDepth: 8
);

octree.SetRegion(
    new BoundingBox3D(new Vector3(2700,0,10), new Vector3(5000,5000,250)),
    MaterialId.Rock,
    maxDepth: 8
);

// Test queries across transition
var points = new[]
{
    (1000.0, 2500.0, -100.0, "Deep ocean"),
    (2600.0, 2500.0, 0.0, "Beach"),
    (3500.0, 2500.0, 5.0, "Land surface"),
    (4000.0, 2500.0, 100.0, "Subsurface rock")
};

Console.WriteLine("Coastal profile:");
foreach (var (x, y, z, label) in points)
{
    var material = octree.QueryMaterial(x, y, z);
    Console.WriteLine($"  {label}: {material}");
}

octree.OptimizeMemory();
var stats = octree.CalculateMemoryStatistics();
Console.WriteLine($"\nCoastal transition:");
Console.WriteLine($"  Inheritance Efficiency: {stats.InheritanceEfficiencyRatio:P0}");
Console.WriteLine($"  Collapsed Nodes: {stats.CollapsedNodes}");
```

## Performance Testing

```csharp
using System.Diagnostics;

// Create large region
var bounds = new BoundingBox3D(new Vector3(0,0,0), new Vector3(10000,10000,1000));
var octree = new OptimizedOctree(bounds);

// Populate with water
octree.SetRegion(bounds, MaterialId.Water, maxDepth: 8);

// Benchmark query performance
var testPoint = new Vector3(5000, 5000, 500);
int iterations = 100000;

var sw = Stopwatch.StartNew();
for (int i = 0; i < iterations; i++)
{
    octree.QueryMaterial(testPoint);
}
sw.Stop();

double avgMs = sw.Elapsed.TotalMilliseconds / iterations;
double qps = iterations / sw.Elapsed.TotalSeconds;

Console.WriteLine($"Performance:");
Console.WriteLine($"  Iterations: {iterations:N0}");
Console.WriteLine($"  Total Time: {sw.Elapsed.TotalMilliseconds:F2} ms");
Console.WriteLine($"  Average: {avgMs:F6} ms per query");
Console.WriteLine($"  Throughput: {qps:N0} queries/second");

var cacheStats = octree.GetCacheStatistics();
Console.WriteLine($"  Cache Hit Rate: {cacheStats.HitRate:P1}");
```

## Advanced: Custom Material Types

```csharp
// While the research prototype has 9 predefined materials,
// you can extend the MaterialId enum for custom materials:

// In MaterialSystem.cs, add to MaterialId enum:
// CustomMaterial1 = 9,
// CustomMaterial2 = 10,

// And add corresponding MaterialData entries:
// public static readonly MaterialData CustomMaterial1 = new MaterialData(
//     MaterialId.CustomMaterial1, "Custom Material 1",
//     new MaterialProperties(density, hardness, porosity, permeability, thermal)
// );

// Then use in your code:
// octree.SetMaterial(point, MaterialId.CustomMaterial1);
```

## Tips and Best Practices

1. **Choose appropriate maxDepth**: Deeper levels = more detail but more nodes
   - For large homogeneous regions: maxDepth 8-10
   - For detailed heterogeneous regions: maxDepth 12-15
   - For fine-grained queries: maxDepth 16-20

2. **Optimize after bulk operations**: Call `OptimizeMemory()` after setting many regions

3. **Use SetRegion for bulk operations**: Much faster than setting individual points

4. **Monitor cache statistics**: Ensure high hit rates (>80%) for optimal performance

5. **Be mindful of stack depth**: Very deep recursion can cause stack overflow
   - Use maxDepth parameter to limit recursion
   - For Earth-scale applications, consider iterative approaches

6. **Material distribution matters**: More homogeneous = better compression
   - Ocean regions: 90-95% compression
   - Continental regions: 60-80% compression
   - Coastal/transition zones: 40-60% compression

## Running Tests

To run the test suite:

```bash
dotnet run
```

Tests cover:
- Material system operations
- Node inheritance chains
- Homogeneity calculations
- Collapse/expand operations
- Cache functionality
- High-level API operations
- Memory optimization
- Real-world scenarios
- Performance benchmarks

## Further Reading

- `README.md` - Project overview and architecture
- `RESEARCH_FINDINGS.md` - Detailed research results and analysis
- `../step-4-implementation/material-inheritance-implementation.md` - Full implementation spec
- `../step-3-architecture-design/octree-optimization-guide.md` - Architecture decisions
