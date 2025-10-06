# Grid + Vector Hybrid Storage: Performance Benchmarks and Testing

## Executive Summary

This document provides comprehensive performance benchmarking specifications, testing methodologies, and expected performance targets for the Grid + Vector hybrid storage system. It includes benchmark suites, profiling strategies, and optimization guidelines.

## Benchmark Categories

### 1. Core Data Structure Benchmarks

#### 1.1 Grid Creation and Initialization

```csharp
namespace BlueMarble.SpatialStorage.GridVector.Benchmarks
{
    [MemoryDiagnoser]
    [SimpleJob(RuntimeMoniker.Net60)]
    public class GridCreationBenchmarks
    {
        [Params(100, 500, 1000, 2000)]
        public int GridSize { get; set; }
        
        [Benchmark]
        public DenseSimulationGrid CreateGrid_SmallCells()
        {
            var bounds = new Envelope(0, 0, GridSize, GridSize);
            return DenseSimulationGrid.Create("test", bounds, 1.0);
        }
        
        [Benchmark]
        public DenseSimulationGrid CreateGrid_LargeCells()
        {
            var bounds = new Envelope(0, 0, GridSize * 10, GridSize * 10);
            return DenseSimulationGrid.Create("test", bounds, 10.0);
        }
        
        [Benchmark]
        public void InitializeProcessLayers()
        {
            var grid = DenseSimulationGrid.Create(
                "test",
                new Envelope(0, 0, 1000, 1000),
                1.0
            );
            
            // Initialize common layers
            var elevation = grid.ProcessGrid.GetOrCreateLayer<float>("elevation");
            var temperature = grid.ProcessGrid.GetOrCreateLayer<float>("temperature");
            var waterDepth = grid.ProcessGrid.GetOrCreateLayer<float>("waterDepth");
            var velocity = grid.ProcessGrid.GetOrCreateLayer<Vector2>("waterVelocity");
        }
    }
}
```

**Expected Performance**:
- Grid Creation (1000x1000): < 50ms
- Grid Creation (2000x2000): < 200ms
- Layer Initialization: < 10ms per layer
- Memory Usage: ~4MB per 1M cells (single layer)

#### 1.2 Spatial Index Performance

```csharp
[MemoryDiagnoser]
public class SpatialIndexBenchmarks
{
    private VectorBoundaryIndex _index;
    private List<BoundaryFeature> _boundaries;
    
    [Params(10, 100, 1000, 10000)]
    public int BoundaryCount { get; set; }
    
    [GlobalSetup]
    public void Setup()
    {
        _index = new VectorBoundaryIndex();
        _boundaries = GenerateRandomBoundaries(BoundaryCount);
        
        foreach (var boundary in _boundaries)
        {
            _index.AddBoundary(boundary);
        }
        
        _index.BuildIndex();
    }
    
    [Benchmark]
    public List<BoundaryFeature> QueryRadius_Small()
    {
        return _index.QueryRadius(new Vector3(500, 500, 0), 10.0);
    }
    
    [Benchmark]
    public List<BoundaryFeature> QueryRadius_Medium()
    {
        return _index.QueryRadius(new Vector3(500, 500, 0), 100.0);
    }
    
    [Benchmark]
    public List<BoundaryFeature> QueryEnvelope()
    {
        return _index.QueryEnvelope(
            new Envelope(400, 400, 600, 600)
        );
    }
    
    [Benchmark]
    public void UpdateBoundary()
    {
        var boundary = _boundaries[0];
        boundary.Priority += 0.1;
        _index.UpdateBoundary(boundary);
    }
    
    private List<BoundaryFeature> GenerateRandomBoundaries(int count)
    {
        var random = new Random(42);
        var factory = new GeometryFactory();
        var boundaries = new List<BoundaryFeature>();
        
        for (int i = 0; i < count; i++)
        {
            var coords = new[]
            {
                new Coordinate(random.Next(1000), random.Next(1000)),
                new Coordinate(random.Next(1000), random.Next(1000))
            };
            
            boundaries.Add(new BoundaryFeature
            {
                Id = $"boundary-{i}",
                Geometry = factory.CreateLineString(coords),
                Type = BoundaryType.Fault,
                Priority = random.NextDouble()
            });
        }
        
        return boundaries;
    }
}
```

**Expected Performance**:
- Index Build (1K boundaries): < 100ms
- Index Build (10K boundaries): < 500ms
- Radius Query (10m): < 1ms
- Radius Query (100m): < 5ms
- Envelope Query: < 2ms
- Boundary Update: < 1ms

### 2. Query Performance Benchmarks

#### 2.1 Material Query Performance

```csharp
[MemoryDiagnoser]
public class QueryPerformanceBenchmarks
{
    private GridVectorHybridStorage _storage;
    private HybridQueryProcessor _processor;
    private List<Vector3> _queryPoints;
    
    [GlobalSetup]
    public void Setup()
    {
        // Create test storage
        _storage = new GridVectorHybridStorage(new MockOctreeStorage());
        
        // Create test grid
        var grid = _storage.GetOrCreateGrid(
            "test-region",
            new Envelope(0, 0, 1000, 1000),
            1.0
        );
        
        // Add test boundaries
        AddTestBoundaries(_storage);
        
        _processor = new HybridQueryProcessor(
            _storage,
            _storage._precisionBoundaries,
            new MockOctreeStorage()
        );
        
        // Generate query points
        _queryPoints = GenerateQueryPoints(10000);
    }
    
    [Benchmark]
    public void QueryMaterial_GridInterior()
    {
        // Query points far from boundaries
        foreach (var point in _queryPoints.Take(1000))
        {
            _processor.QueryMaterial(
                point,
                new QueryContext { TargetResolution = 1.0 }
            );
        }
    }
    
    [Benchmark]
    public void QueryMaterial_NearBoundaries()
    {
        // Query points near boundaries
        var boundaryPoints = _queryPoints.Skip(5000).Take(1000);
        
        foreach (var point in boundaryPoints)
        {
            _processor.QueryMaterial(
                point,
                new QueryContext { TargetResolution = 1.0 }
            );
        }
    }
    
    [Benchmark]
    public void QueryMaterial_Mixed()
    {
        // Realistic mix of interior and boundary queries
        foreach (var point in _queryPoints.Take(1000))
        {
            _processor.QueryMaterial(
                point,
                new QueryContext { TargetResolution = 1.0 }
            );
        }
    }
    
    [Benchmark]
    public void BulkQuery_Region()
    {
        // Query 100x100 region
        var results = new List<MaterialId>();
        
        for (double y = 100; y < 200; y += 1.0)
        {
            for (double x = 100; x < 200; x += 1.0)
            {
                var result = _processor.QueryMaterial(
                    new Vector3(x, y, 0),
                    new QueryContext()
                );
                results.Add(result.Material);
            }
        }
    }
    
    private List<Vector3> GenerateQueryPoints(int count)
    {
        var random = new Random(42);
        var points = new List<Vector3>();
        
        for (int i = 0; i < count; i++)
        {
            points.Add(new Vector3(
                random.Next(1000),
                random.Next(1000),
                0
            ));
        }
        
        return points;
    }
    
    private void AddTestBoundaries(GridVectorHybridStorage storage)
    {
        var factory = new GeometryFactory();
        
        // Add some test boundaries
        for (int i = 0; i < 10; i++)
        {
            storage.AddOrUpdateBoundary(new BoundaryFeature
            {
                Id = $"test-{i}",
                Geometry = factory.CreateLineString(new[]
                {
                    new Coordinate(i * 100, 0),
                    new Coordinate(i * 100, 1000)
                }),
                Type = BoundaryType.Fault,
                MaterialTransition = new MaterialTransition
                {
                    LeftMaterial = new MaterialId(1),
                    RightMaterial = new MaterialId(2)
                }
            });
        }
    }
}
```

**Expected Performance**:
- Grid Interior Query: < 0.1ms
- Near Boundary Query: < 1ms
- Mixed Query: < 0.5ms average
- Bulk Region Query (10K cells): < 500ms

### 3. Geomorphological Process Benchmarks

#### 3.1 Erosion Processing

```csharp
[MemoryDiagnoser]
[SimpleJob(launchCount: 1, warmupCount: 3, targetCount: 5)]
public class ErosionProcessingBenchmarks
{
    private DenseSimulationGrid _grid;
    private HydraulicErosionProcessor _processor;
    
    [Params(256, 512, 1024, 2048)]
    public int GridSize { get; set; }
    
    [GlobalSetup]
    public void Setup()
    {
        var storage = new GridVectorHybridStorage(new MockOctreeStorage());
        _grid = storage.GetOrCreateGrid(
            "test",
            new Envelope(0, 0, GridSize, GridSize),
            1.0
        );
        
        // Initialize elevation with random terrain
        InitializeTestTerrain(_grid);
        
        _processor = new HydraulicErosionProcessor(storage);
    }
    
    [Benchmark]
    public async Task ProcessErosion_OneStep()
    {
        await _processor.ProcessErosion(
            _grid.RegionId,
            new ErosionParameters(),
            TimeSpan.FromMinutes(1)
        );
    }
    
    [Benchmark]
    public async Task ProcessErosion_TenSteps()
    {
        for (int i = 0; i < 10; i++)
        {
            await _processor.ProcessErosion(
                _grid.RegionId,
                new ErosionParameters(),
                TimeSpan.FromMinutes(1)
            );
        }
    }
    
    private void InitializeTestTerrain(DenseSimulationGrid grid)
    {
        var elevation = grid.ProcessGrid.GetOrCreateLayer<float>("elevation");
        var waterDepth = grid.ProcessGrid.GetOrCreateLayer<float>("waterDepth");
        var velocity = grid.ProcessGrid.GetOrCreateLayer<Vector2>("waterVelocity");
        
        var random = new Random(42);
        
        for (int y = 0; y < grid.Size.Height; y++)
        {
            for (int x = 0; x < grid.Size.Width; x++)
            {
                elevation[y, x] = (float)(random.NextDouble() * 100);
                waterDepth[y, x] = (float)(random.NextDouble() * 0.5);
                velocity[y, x] = new Vector2(
                    (float)(random.NextDouble() - 0.5),
                    (float)(random.NextDouble() - 0.5)
                );
            }
        }
    }
}
```

**Expected Performance**:
- 256x256 grid: < 10ms per step
- 512x512 grid: < 40ms per step
- 1024x1024 grid: < 150ms per step
- 2048x2048 grid: < 600ms per step
- Throughput: > 1M cells/second

#### 3.2 Thermal Diffusion

```csharp
[MemoryDiagnoser]
public class ThermalDiffusionBenchmarks
{
    private DenseSimulationGrid _grid;
    private ThermalDiffusionProcessor _processor;
    
    [Params(512, 1024)]
    public int GridSize { get; set; }
    
    [GlobalSetup]
    public void Setup()
    {
        var storage = new GridVectorHybridStorage(new MockOctreeStorage());
        _grid = storage.GetOrCreateGrid(
            "test",
            new Envelope(0, 0, GridSize, GridSize),
            1.0
        );
        
        InitializeTemperatureField(_grid);
        _processor = new ThermalDiffusionProcessor();
    }
    
    [Benchmark]
    public void ProcessThermalDiffusion()
    {
        _processor.ProcessThermalDiffusion(
            _grid,
            TimeSpan.FromHours(1)
        );
    }
    
    private void InitializeTemperatureField(DenseSimulationGrid grid)
    {
        var temperature = grid.ProcessGrid.GetOrCreateLayer<float>("temperature");
        var diffusivity = grid.ProcessGrid.GetOrCreateLayer<float>("thermalDiffusivity");
        
        var random = new Random(42);
        
        for (int y = 0; y < grid.Size.Height; y++)
        {
            for (int x = 0; x < grid.Size.Width; x++)
            {
                temperature[y, x] = 20f + (float)(random.NextDouble() * 10);
                diffusivity[y, x] = 0.001f;
            }
        }
    }
}
```

**Expected Performance**:
- 512x512: < 30ms per step
- 1024x1024: < 120ms per step

### 4. Synchronization Benchmarks

#### 4.1 Grid-Vector Synchronization

```csharp
[MemoryDiagnoser]
public class SynchronizationBenchmarks
{
    private DenseSimulationGrid _grid;
    private List<BoundaryFeature> _boundaries;
    private GridVectorSynchronizer _synchronizer;
    
    [Params(1, 10, 100)]
    public int BoundaryCount { get; set; }
    
    [GlobalSetup]
    public void Setup()
    {
        _grid = DenseSimulationGrid.Create(
            "test",
            new Envelope(0, 0, 1000, 1000),
            1.0
        );
        
        _boundaries = GenerateBoundariesAcrossGrid(BoundaryCount);
        _synchronizer = new GridVectorSynchronizer();
    }
    
    [Benchmark]
    public void SynchronizeBoundaries()
    {
        _synchronizer.SynchronizeBoundaryAffectedCells(
            _grid,
            _boundaries
        );
    }
    
    [Benchmark]
    public void RasterizeSingleBoundary()
    {
        // Test rasterization performance
        _synchronizer.SynchronizeBoundaryAffectedCells(
            _grid,
            _boundaries.Take(1)
        );
    }
    
    private List<BoundaryFeature> GenerateBoundariesAcrossGrid(int count)
    {
        var factory = new GeometryFactory();
        var boundaries = new List<BoundaryFeature>();
        
        for (int i = 0; i < count; i++)
        {
            var y = (i * 1000.0) / count;
            boundaries.Add(new BoundaryFeature
            {
                Id = $"boundary-{i}",
                Geometry = factory.CreateLineString(new[]
                {
                    new Coordinate(0, y),
                    new Coordinate(1000, y)
                }),
                Type = BoundaryType.Fault,
                MaterialTransition = new MaterialTransition
                {
                    LeftMaterial = new MaterialId(1),
                    RightMaterial = new MaterialId(2),
                    Type = TransitionType.Linear,
                    BlendWidth = 5.0
                }
            });
        }
        
        return boundaries;
    }
}
```

**Expected Performance**:
- 1 boundary: < 5ms
- 10 boundaries: < 40ms
- 100 boundaries: < 300ms

### 5. Memory Benchmarks

#### 5.1 Memory Usage Analysis

```csharp
[MemoryDiagnoser]
public class MemoryUsageBenchmarks
{
    [Params(100, 500, 1000)]
    public int GridSize { get; set; }
    
    [Benchmark]
    public DenseSimulationGrid AllocateGrid()
    {
        return DenseSimulationGrid.Create(
            "test",
            new Envelope(0, 0, GridSize, GridSize),
            1.0
        );
    }
    
    [Benchmark]
    public void AllocateMultipleLayers()
    {
        var grid = DenseSimulationGrid.Create(
            "test",
            new Envelope(0, 0, GridSize, GridSize),
            1.0
        );
        
        // Allocate multiple process layers
        var elevation = grid.ProcessGrid.GetOrCreateLayer<float>("elevation");
        var temperature = grid.ProcessGrid.GetOrCreateLayer<float>("temperature");
        var pressure = grid.ProcessGrid.GetOrCreateLayer<float>("pressure");
        var waterDepth = grid.ProcessGrid.GetOrCreateLayer<float>("waterDepth");
        var velocity = grid.ProcessGrid.GetOrCreateLayer<Vector2>("waterVelocity");
    }
    
    [Benchmark]
    public void AllocateHybridStorage()
    {
        var storage = new GridVectorHybridStorage(new MockOctreeStorage());
        
        // Create multiple grids
        for (int i = 0; i < 10; i++)
        {
            storage.GetOrCreateGrid(
                $"region-{i}",
                new Envelope(i * 100, 0, (i + 1) * 100, 100),
                1.0
            );
        }
        
        // Add boundaries
        var factory = new GeometryFactory();
        for (int i = 0; i < 100; i++)
        {
            storage.AddOrUpdateBoundary(new BoundaryFeature
            {
                Id = $"boundary-{i}",
                Geometry = factory.CreateLineString(new[]
                {
                    new Coordinate(i, 0),
                    new Coordinate(i, 100)
                }),
                Type = BoundaryType.Fault
            });
        }
    }
}
```

**Expected Memory Usage**:
- 100x100 grid: ~40KB (material only)
- 500x500 grid: ~1MB (material only)
- 1000x1000 grid: ~4MB (material only)
- With 5 process layers: ~5x material size
- Boundary index (100 features): ~50KB
- Boundary index (1000 features): ~500KB

## Benchmark Execution

### Running Benchmarks

```bash
# Run all benchmarks
dotnet run -c Release --project Benchmarks.csproj

# Run specific category
dotnet run -c Release --project Benchmarks.csproj --filter "*GridCreation*"

# Export results
dotnet run -c Release --project Benchmarks.csproj --exporters json html
```

### Continuous Benchmarking

```yaml
# .github/workflows/benchmarks.yml
name: Performance Benchmarks

on:
  pull_request:
    paths:
      - 'src/BlueMarble.SpatialStorage.GridVector/**'
  schedule:
    - cron: '0 2 * * 0'  # Weekly

jobs:
  benchmark:
    runs-on: ubuntu-latest
    
    steps:
    - uses: actions/checkout@v3
    
    - name: Setup .NET
      uses: actions/setup-dotnet@v3
      with:
        dotnet-version: '6.0.x'
    
    - name: Run Benchmarks
      run: |
        dotnet run -c Release --project tests/Benchmarks.csproj \
          --exporters json
    
    - name: Store Benchmark Results
      uses: benchmark-action/github-action-benchmark@v1
      with:
        tool: 'benchmarkdotnet'
        output-file-path: BenchmarkDotNet.Artifacts/results/*.json
        
    - name: Compare with Baseline
      run: |
        python scripts/compare-benchmarks.py \
          --baseline baseline-results.json \
          --current BenchmarkDotNet.Artifacts/results/combined.json \
          --threshold 10
```

## Performance Profiling

### CPU Profiling

```csharp
// Using dotnet-trace
public class ProfilingTests
{
    [Test]
    public async Task Profile_ErosionProcessing()
    {
        // Start tracing
        // dotnet trace collect --process-id <pid>
        
        var storage = new GridVectorHybridStorage(new MockOctreeStorage());
        var grid = storage.GetOrCreateGrid(
            "test",
            new Envelope(0, 0, 1000, 1000),
            1.0
        );
        
        var processor = new HydraulicErosionProcessor(storage);
        
        // Process that will be profiled
        for (int i = 0; i < 100; i++)
        {
            await processor.ProcessErosion(
                grid.RegionId,
                new ErosionParameters(),
                TimeSpan.FromMinutes(1)
            );
        }
        
        // Stop tracing and analyze with PerfView or dotnet-trace
    }
}
```

### Memory Profiling

```bash
# Using dotnet-counters
dotnet-counters monitor \
  --process-id <pid> \
  System.Runtime \
  --counters gc-heap-size,gen-0-gc-count,gen-1-gc-count,gen-2-gc-count

# Using dotnet-gcdump
dotnet-gcdump collect --process-id <pid>
dotnet-gcdump report heap.gcdump
```

## Performance Targets Summary

### Tier 1: Core Operations (Critical Path)

| Operation | Target | Measured | Status |
|-----------|--------|----------|--------|
| Material Query (Grid) | < 0.1ms | TBD | 🎯 |
| Material Query (Vector) | < 1ms | TBD | 🎯 |
| Grid Creation (1Kx1K) | < 50ms | TBD | 🎯 |
| Boundary Query (10m) | < 1ms | TBD | 🎯 |

### Tier 2: Processing Operations

| Operation | Target | Measured | Status |
|-----------|--------|----------|--------|
| Erosion (1Kx1K grid) | < 150ms | TBD | 🎯 |
| Thermal (1Kx1K grid) | < 120ms | TBD | 🎯 |
| Synchronization (10 boundaries) | < 40ms | TBD | 🎯 |
| Throughput | > 1M cells/sec | TBD | 🎯 |

### Tier 3: Memory Efficiency

| Resource | Target | Measured | Status |
|----------|--------|----------|--------|
| Grid (1M cells, material only) | < 4MB | TBD | 🎯 |
| Grid (1M cells, 5 layers) | < 20MB | TBD | 🎯 |
| Boundary Index (1K features) | < 500KB | TBD | 🎯 |
| Total per 1km² region | < 50MB | TBD | 🎯 |

## Optimization Guidelines

### 1. Grid Processing Optimization

```csharp
// Bad: Sequential processing
for (int y = 0; y < height; y++)
{
    for (int x = 0; x < width; x++)
    {
        ProcessCell(x, y);
    }
}

// Good: Parallel processing
Parallel.For(0, height, y =>
{
    for (int x = 0; x < width; x++)
    {
        ProcessCell(x, y);
    }
});
```

### 2. Memory Access Patterns

```csharp
// Bad: Column-major access (poor cache locality)
for (int x = 0; x < width; x++)
{
    for (int y = 0; y < height; y++)
    {
        grid[y, x] = value;
    }
}

// Good: Row-major access (good cache locality)
for (int y = 0; y < height; y++)
{
    for (int x = 0; x < width; x++)
    {
        grid[y, x] = value;
    }
}
```

### 3. Lazy Layer Initialization

```csharp
// Good: Only create layers when needed
public class ProcessDataGrid
{
    private Dictionary<string, Array> _layers = new();
    
    public T[,] GetOrCreateLayer<T>(string name)
    {
        if (!_layers.ContainsKey(name))
        {
            _layers[name] = new T[Height, Width];
        }
        return (T[,])_layers[name];
    }
}
```

## Conclusion

This benchmark suite provides comprehensive performance testing for the Grid + Vector hybrid storage system. Regular benchmarking ensures the system meets performance targets and catches regressions early in development.

**Key Takeaways**:
- Grid operations should achieve > 1M cells/second throughput
- Boundary queries should remain under 1ms for typical cases
- Memory usage should stay under 50MB per km² region
- Parallel processing is essential for larger grids
- Cache-friendly access patterns significantly improve performance
