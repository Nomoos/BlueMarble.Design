# Grid + Vector Hybrid Storage: Test Specifications

## Executive Summary

This document defines comprehensive test specifications for the Grid + Vector hybrid storage system. It covers unit tests, integration tests, performance tests, and validation strategies to ensure the system meets all functional and non-functional requirements.

## Test Strategy

### Testing Pyramid

```
         ┌─────────────────┐
         │   System Tests  │  10% - End-to-end scenarios
         │   (E2E)         │
         ├─────────────────┤
         │  Integration    │  30% - Component interaction
         │  Tests          │
         ├─────────────────┤
         │   Unit Tests    │  60% - Individual components
         └─────────────────┘
```

### Test Categories

1. **Unit Tests** - Individual class and method testing
2. **Integration Tests** - Component interaction testing
3. **Performance Tests** - Benchmarking and profiling
4. **System Tests** - End-to-end scenario validation
5. **Property-based Tests** - Invariant verification

## Unit Test Specifications

### 1. DenseSimulationGrid Tests

#### Test Class: DenseSimulationGridTests

```csharp
namespace BlueMarble.SpatialStorage.GridVector.Tests
{
    [TestFixture]
    public class DenseSimulationGridTests
    {
        [Test]
        [Category("Unit")]
        public void Create_ValidParameters_CreatesGridWithCorrectDimensions()
        {
            // Arrange
            var regionId = "test-region";
            var bounds = new Envelope(0, 0, 1000, 1000);
            var cellSize = 1.0;
            
            // Act
            var grid = DenseSimulationGrid.Create(regionId, bounds, cellSize);
            
            // Assert
            Assert.Multiple(() =>
            {
                Assert.AreEqual(regionId, grid.RegionId);
                Assert.AreEqual(bounds, grid.Bounds);
                Assert.AreEqual(cellSize, grid.CellSize);
                Assert.AreEqual(1000, grid.Size.Width);
                Assert.AreEqual(1000, grid.Size.Height);
                Assert.IsNotNull(grid.MaterialGrid);
                Assert.IsNotNull(grid.ProcessGrid);
            });
        }
        
        [Test]
        [Category("Unit")]
        public void Create_NonUniformBounds_RoundsUpDimensions()
        {
            // Arrange
            var bounds = new Envelope(0, 0, 105.5, 97.3);
            var cellSize = 10.0;
            
            // Act
            var grid = DenseSimulationGrid.Create("test", bounds, cellSize);
            
            // Assert
            Assert.AreEqual(11, grid.Size.Width);  // Ceil(105.5/10)
            Assert.AreEqual(10, grid.Size.Height); // Ceil(97.3/10)
        }
        
        [TestCase(0, 0, 0, 0)]
        [TestCase(999.9, 999.9, 999, 999)]
        [TestCase(500.5, 500.5, 500, 500)]
        [TestCase(0.1, 0.1, 0, 0)]
        [Category("Unit")]
        public void WorldToCell_VariousPositions_ReturnsCorrectCell(
            double worldX, double worldY, int expectedX, int expectedY)
        {
            // Arrange
            var grid = DenseSimulationGrid.Create(
                "test",
                new Envelope(0, 0, 1000, 1000),
                1.0
            );
            
            // Act
            var (x, y) = grid.WorldToCell(worldX, worldY);
            
            // Assert
            Assert.AreEqual(expectedX, x);
            Assert.AreEqual(expectedY, y);
        }
        
        [Test]
        [Category("Unit")]
        public void WorldToCell_OutOfBounds_ClampsToBounds()
        {
            // Arrange
            var grid = DenseSimulationGrid.Create(
                "test",
                new Envelope(0, 0, 100, 100),
                1.0
            );
            
            // Act
            var (x1, y1) = grid.WorldToCell(-10, -10);
            var (x2, y2) = grid.WorldToCell(110, 110);
            
            // Assert
            Assert.AreEqual(0, x1);
            Assert.AreEqual(0, y1);
            Assert.AreEqual(99, x2);
            Assert.AreEqual(99, y2);
        }
        
        [Test]
        [Category("Unit")]
        public void GetCellCenter_ValidCell_ReturnsCorrectWorldCoordinates()
        {
            // Arrange
            var grid = DenseSimulationGrid.Create(
                "test",
                new Envelope(0, 0, 100, 100),
                10.0
            );
            
            // Act
            var (x, y) = grid.GetCellCenter(5, 5);
            
            // Assert
            Assert.AreEqual(55.0, x, 0.001);
            Assert.AreEqual(55.0, y, 0.001);
        }
        
        [Test]
        [Category("Unit")]
        public void GetCellBounds_ValidCell_ReturnsCorrectEnvelope()
        {
            // Arrange
            var grid = DenseSimulationGrid.Create(
                "test",
                new Envelope(0, 0, 100, 100),
                10.0
            );
            
            // Act
            var bounds = grid.GetCellBounds(5, 5);
            
            // Assert
            Assert.AreEqual(50.0, bounds.MinX);
            Assert.AreEqual(50.0, bounds.MinY);
            Assert.AreEqual(60.0, bounds.MaxX);
            Assert.AreEqual(60.0, bounds.MaxY);
        }
        
        [Test]
        [Category("Unit")]
        public void QueryMaterial_ValidPosition_ReturnsMaterial()
        {
            // Arrange
            var grid = DenseSimulationGrid.Create(
                "test",
                new Envelope(0, 0, 10, 10),
                1.0
            );
            var expectedMaterial = new MaterialId(42);
            grid.MaterialGrid[5, 5] = expectedMaterial;
            
            // Act
            var material = grid.QueryMaterial(5.5, 5.5);
            
            // Assert
            Assert.AreEqual(expectedMaterial, material);
        }
        
        [Test]
        [Category("Unit")]
        public void QueryRegion_ValidBounds_ReturnsSubGrid()
        {
            // Arrange
            var grid = DenseSimulationGrid.Create(
                "test",
                new Envelope(0, 0, 100, 100),
                1.0
            );
            
            // Fill with test pattern
            for (int y = 0; y < grid.Size.Height; y++)
            {
                for (int x = 0; x < grid.Size.Width; x++)
                {
                    grid.MaterialGrid[y, x] = new MaterialId(x + y * 100);
                }
            }
            
            // Act
            var subGrid = grid.QueryRegion(new Envelope(10, 10, 20, 20));
            
            // Assert
            Assert.AreEqual(11, subGrid.GetLength(1)); // Width
            Assert.AreEqual(11, subGrid.GetLength(0)); // Height
            Assert.AreEqual(new MaterialId(10 + 10 * 100), subGrid[0, 0]);
        }
    }
}
```

### 2. VectorBoundaryIndex Tests

```csharp
[TestFixture]
public class VectorBoundaryIndexTests
{
    private VectorBoundaryIndex _index;
    private GeometryFactory _factory;
    
    [SetUp]
    public void Setup()
    {
        _index = new VectorBoundaryIndex();
        _factory = new GeometryFactory();
    }
    
    [Test]
    [Category("Unit")]
    public void AddBoundary_ValidBoundary_AddsToIndex()
    {
        // Arrange
        var boundary = CreateTestBoundary("test-1", 0, 0, 100, 100);
        
        // Act
        _index.AddBoundary(boundary);
        _index.BuildIndex();
        
        // Assert
        var retrieved = _index.GetBoundary("test-1");
        Assert.IsNotNull(retrieved);
        Assert.AreEqual("test-1", retrieved.Id);
    }
    
    [Test]
    [Category("Unit")]
    public void UpdateBoundary_ExistingBoundary_UpdatesInIndex()
    {
        // Arrange
        var boundary = CreateTestBoundary("test-1", 0, 0, 100, 100);
        _index.AddBoundary(boundary);
        _index.BuildIndex();
        
        // Modify boundary
        boundary.Priority = 0.9;
        
        // Act
        _index.UpdateBoundary(boundary);
        
        // Assert
        var retrieved = _index.GetBoundary("test-1");
        Assert.AreEqual(0.9, retrieved.Priority);
    }
    
    [Test]
    [Category("Unit")]
    public void RemoveBoundary_ExistingBoundary_RemovesFromIndex()
    {
        // Arrange
        var boundary = CreateTestBoundary("test-1", 0, 0, 100, 100);
        _index.AddBoundary(boundary);
        _index.BuildIndex();
        
        // Act
        _index.RemoveBoundary("test-1");
        
        // Assert
        var retrieved = _index.GetBoundary("test-1");
        Assert.IsNull(retrieved);
    }
    
    [Test]
    [Category("Unit")]
    public void QueryRadius_NearBoundary_ReturnsMatchingBoundaries()
    {
        // Arrange
        var boundary = CreateTestBoundary("test-1", 0, 0, 100, 0);
        _index.AddBoundary(boundary);
        _index.BuildIndex();
        
        // Act
        var results = _index.QueryRadius(new Vector3(50, 5, 0), 10.0);
        
        // Assert
        Assert.AreEqual(1, results.Count);
        Assert.AreEqual("test-1", results[0].Id);
    }
    
    [Test]
    [Category("Unit")]
    public void QueryRadius_FarFromBoundaries_ReturnsEmpty()
    {
        // Arrange
        var boundary = CreateTestBoundary("test-1", 0, 0, 100, 0);
        _index.AddBoundary(boundary);
        _index.BuildIndex();
        
        // Act
        var results = _index.QueryRadius(new Vector3(50, 50, 0), 10.0);
        
        // Assert
        Assert.IsEmpty(results);
    }
    
    [Test]
    [Category("Unit")]
    public void QueryEnvelope_IntersectingBoundaries_ReturnsMatches()
    {
        // Arrange
        _index.AddBoundary(CreateTestBoundary("b1", 0, 50, 100, 50));
        _index.AddBoundary(CreateTestBoundary("b2", 50, 0, 50, 100));
        _index.AddBoundary(CreateTestBoundary("b3", 200, 0, 300, 100));
        _index.BuildIndex();
        
        // Act
        var results = _index.QueryEnvelope(new Envelope(40, 40, 60, 60));
        
        // Assert
        Assert.AreEqual(2, results.Count);
        var ids = results.Select(r => r.Id).ToList();
        CollectionAssert.Contains(ids, "b1");
        CollectionAssert.Contains(ids, "b2");
    }
    
    private BoundaryFeature CreateTestBoundary(
        string id, double x1, double y1, double x2, double y2)
    {
        return new BoundaryFeature
        {
            Id = id,
            Geometry = _factory.CreateLineString(new[]
            {
                new Coordinate(x1, y1),
                new Coordinate(x2, y2)
            }),
            Type = BoundaryType.Fault,
            Priority = 0.5,
            MaterialTransition = new MaterialTransition
            {
                LeftMaterial = new MaterialId(1),
                RightMaterial = new MaterialId(2)
            }
        };
    }
}
```

### 3. GridVectorSynchronizer Tests

```csharp
[TestFixture]
public class GridVectorSynchronizerTests
{
    private GridVectorSynchronizer _synchronizer;
    private GeometryFactory _factory;
    
    [SetUp]
    public void Setup()
    {
        _synchronizer = new GridVectorSynchronizer();
        _factory = new GeometryFactory();
    }
    
    [Test]
    [Category("Unit")]
    public void SynchronizeBoundaryAffectedCells_SimpleBoundary_UpdatesAffectedCells()
    {
        // Arrange
        var grid = DenseSimulationGrid.Create(
            "test",
            new Envelope(0, 0, 10, 10),
            1.0
        );
        
        var boundary = new BoundaryFeature
        {
            Id = "test-boundary",
            Geometry = _factory.CreateLineString(new[]
            {
                new Coordinate(0, 5),
                new Coordinate(10, 5)
            }),
            MaterialTransition = new MaterialTransition
            {
                LeftMaterial = new MaterialId(10),
                RightMaterial = new MaterialId(20),
                Type = TransitionType.Sharp,
                BlendWidth = 0.5
            }
        };
        
        // Act
        _synchronizer.SynchronizeBoundaryAffectedCells(
            grid,
            new[] { boundary }
        );
        
        // Assert
        Assert.IsTrue(grid.AffectedBoundaries.Contains("test-boundary"));
        
        // Check materials were updated (cells below y=5 should be one material, above another)
        var material4_4 = grid.MaterialGrid[4, 5]; // Below boundary
        var material6_5 = grid.MaterialGrid[6, 5]; // Above boundary
        
        // Materials should be different on opposite sides
        Assert.AreNotEqual(material4_4, material6_5);
    }
    
    [Test]
    [Category("Unit")]
    public void SynchronizeBoundaryAffectedCells_MultipleBoundaries_ProcessesAll()
    {
        // Arrange
        var grid = DenseSimulationGrid.Create(
            "test",
            new Envelope(0, 0, 20, 20),
            1.0
        );
        
        var boundaries = new[]
        {
            CreateHorizontalBoundary("b1", 5),
            CreateHorizontalBoundary("b2", 10),
            CreateHorizontalBoundary("b3", 15)
        };
        
        // Act
        _synchronizer.SynchronizeBoundaryAffectedCells(grid, boundaries);
        
        // Assert
        Assert.AreEqual(3, grid.AffectedBoundaries.Count);
        CollectionAssert.AreEquivalent(
            new[] { "b1", "b2", "b3" },
            grid.AffectedBoundaries
        );
    }
    
    private BoundaryFeature CreateHorizontalBoundary(string id, double y)
    {
        return new BoundaryFeature
        {
            Id = id,
            Geometry = _factory.CreateLineString(new[]
            {
                new Coordinate(0, y),
                new Coordinate(20, y)
            }),
            MaterialTransition = new MaterialTransition
            {
                LeftMaterial = new MaterialId(1),
                RightMaterial = new MaterialId(2),
                Type = TransitionType.Sharp
            }
        };
    }
}
```

## Integration Test Specifications

### 1. Hybrid Query Integration Tests

```csharp
[TestFixture]
public class HybridQueryIntegrationTests
{
    private GridVectorHybridStorage _storage;
    private HybridQueryProcessor _queryProcessor;
    
    [SetUp]
    public void Setup()
    {
        var octreeFallback = new MockOctreeStorage();
        _storage = new GridVectorHybridStorage(octreeFallback);
        _queryProcessor = new HybridQueryProcessor(
            _storage,
            _storage._precisionBoundaries,
            octreeFallback
        );
    }
    
    [Test]
    [Category("Integration")]
    public void QueryMaterial_InteriorRegion_UsesGridPath()
    {
        // Arrange
        var grid = _storage.GetOrCreateGrid(
            "test-region",
            new Envelope(0, 0, 100, 100),
            1.0
        );
        grid.MaterialGrid[50, 50] = new MaterialId(42);
        
        // Act
        var result = _queryProcessor.QueryMaterial(
            new Vector3(50.5, 50.5, 0),
            new QueryContext { TargetResolution = 1.0 }
        );
        
        // Assert
        Assert.AreEqual(new MaterialId(42), result.Material);
        Assert.AreEqual(QuerySource.GridInterior, result.Source);
        Assert.Greater(result.Confidence, 0.9f);
    }
    
    [Test]
    [Category("Integration")]
    public void QueryMaterial_NearBoundary_UsesVectorPath()
    {
        // Arrange
        var grid = _storage.GetOrCreateGrid(
            "test-region",
            new Envelope(0, 0, 100, 100),
            1.0
        );
        
        var factory = new GeometryFactory();
        var boundary = new BoundaryFeature
        {
            Id = "test-fault",
            Geometry = factory.CreateLineString(new[]
            {
                new Coordinate(50, 0),
                new Coordinate(50, 100)
            }),
            Type = BoundaryType.Fault,
            Priority = 1.0,
            MaterialTransition = new MaterialTransition
            {
                LeftMaterial = new MaterialId(10),
                RightMaterial = new MaterialId(20)
            }
        };
        
        _storage.AddOrUpdateBoundary(boundary);
        
        // Act - Query near the boundary
        var result = _queryProcessor.QueryMaterial(
            new Vector3(50.1, 50, 0),
            new QueryContext { TargetResolution = 1.0 }
        );
        
        // Assert
        Assert.AreEqual(QuerySource.VectorPrecision, result.Source);
        Assert.AreEqual(1.0f, result.Confidence);
        Assert.AreEqual("test-fault", result.BoundaryId);
    }
    
    [Test]
    [Category("Integration")]
    public void QueryMaterial_MultipleGrids_SelectsCorrectGrid()
    {
        // Arrange
        var grid1 = _storage.GetOrCreateGrid(
            "region-1",
            new Envelope(0, 0, 50, 50),
            1.0
        );
        grid1.MaterialGrid[25, 25] = new MaterialId(100);
        
        var grid2 = _storage.GetOrCreateGrid(
            "region-2",
            new Envelope(50, 50, 100, 100),
            1.0
        );
        grid2.MaterialGrid[25, 25] = new MaterialId(200);
        
        // Act
        var result1 = _queryProcessor.QueryMaterial(
            new Vector3(25.5, 25.5, 0),
            new QueryContext()
        );
        var result2 = _queryProcessor.QueryMaterial(
            new Vector3(75.5, 75.5, 0),
            new QueryContext()
        );
        
        // Assert
        Assert.AreEqual(new MaterialId(100), result1.Material);
        Assert.AreEqual("region-1", result1.GridId);
        Assert.AreEqual(new MaterialId(200), result2.Material);
        Assert.AreEqual("region-2", result2.GridId);
    }
}
```

### 2. Geomorphology Integration Tests

```csharp
[TestFixture]
public class GeomorphologyIntegrationTests
{
    private GridVectorHybridStorage _storage;
    private HydraulicErosionProcessor _erosionProcessor;
    
    [SetUp]
    public void Setup()
    {
        _storage = new GridVectorHybridStorage(new MockOctreeStorage());
        _erosionProcessor = new HydraulicErosionProcessor(_storage);
    }
    
    [Test]
    [Category("Integration")]
    [Timeout(5000)]
    public async Task ProcessErosion_ValidGrid_CompletesSuccessfully()
    {
        // Arrange
        var grid = _storage.GetOrCreateGrid(
            "erosion-test",
            new Envelope(0, 0, 100, 100),
            1.0
        );
        
        InitializeErosionTestData(grid);
        
        // Act
        var result = await _erosionProcessor.ProcessErosion(
            grid.RegionId,
            new ErosionParameters(),
            TimeSpan.FromMinutes(1)
        );
        
        // Assert
        Assert.IsNotNull(result);
        Assert.Greater(result.ProcessedCells, 0);
        Assert.GreaterOrEqual(result.TotalEroded, 0);
        Assert.GreaterOrEqual(result.TotalDeposited, 0);
    }
    
    [Test]
    [Category("Integration")]
    public async Task ProcessErosion_ConservationOfMass_Maintained()
    {
        // Arrange
        var grid = _storage.GetOrCreateGrid(
            "conservation-test",
            new Envelope(0, 0, 50, 50),
            1.0
        );
        
        InitializeErosionTestData(grid);
        
        var elevation = grid.ProcessGrid.GetLayer<float>("elevation");
        var initialMass = CalculateTotalMass(elevation);
        
        // Act
        await _erosionProcessor.ProcessErosion(
            grid.RegionId,
            new ErosionParameters(),
            TimeSpan.FromMinutes(1)
        );
        
        var sediment = grid.ProcessGrid.GetLayer<float>("sediment");
        var finalMass = CalculateTotalMass(elevation) + CalculateTotalMass(sediment);
        
        // Assert - Mass should be conserved (within numerical tolerance)
        Assert.AreEqual(initialMass, finalMass, initialMass * 0.01); // 1% tolerance
    }
    
    private void InitializeErosionTestData(DenseSimulationGrid grid)
    {
        var elevation = grid.ProcessGrid.GetOrCreateLayer<float>("elevation");
        var waterDepth = grid.ProcessGrid.GetOrCreateLayer<float>("waterDepth");
        var waterVelocity = grid.ProcessGrid.GetOrCreateLayer<Vector2>("waterVelocity");
        
        var random = new Random(42);
        for (int y = 0; y < grid.Size.Height; y++)
        {
            for (int x = 0; x < grid.Size.Width; x++)
            {
                elevation[y, x] = 10f + (float)(random.NextDouble() * 5);
                waterDepth[y, x] = 0.1f;
                waterVelocity[y, x] = new Vector2(0.1f, 0);
            }
        }
    }
    
    private float CalculateTotalMass(float[,] layer)
    {
        float total = 0;
        for (int y = 0; y < layer.GetLength(0); y++)
        {
            for (int x = 0; x < layer.GetLength(1); x++)
            {
                total += layer[y, x];
            }
        }
        return total;
    }
}
```

## Property-Based Test Specifications

### Invariant Tests

```csharp
[TestFixture]
public class GridVectorInvariantTests
{
    [Test]
    [Category("Property")]
    [Repeat(100)]
    public void WorldToCell_RoundTrip_PreservesLocation()
    {
        // Arrange
        var random = new Random();
        var grid = DenseSimulationGrid.Create(
            "test",
            new Envelope(0, 0, 1000, 1000),
            1.0
        );
        
        var worldX = random.NextDouble() * 1000;
        var worldY = random.NextDouble() * 1000;
        
        // Act
        var (cellX, cellY) = grid.WorldToCell(worldX, worldY);
        var (centerX, centerY) = grid.GetCellCenter(cellX, cellY);
        var (cellX2, cellY2) = grid.WorldToCell(centerX, centerY);
        
        // Assert - Round trip should return same cell
        Assert.AreEqual(cellX, cellX2);
        Assert.AreEqual(cellY, cellY2);
    }
    
    [Test]
    [Category("Property")]
    [Repeat(100)]
    public void QueryRadius_IncreasingRadius_MonotonicResults()
    {
        // Property: Larger radius should return same or more results
        
        // Arrange
        var index = new VectorBoundaryIndex();
        var factory = new GeometryFactory();
        var random = new Random(42);
        
        for (int i = 0; i < 50; i++)
        {
            index.AddBoundary(new BoundaryFeature
            {
                Id = $"b{i}",
                Geometry = factory.CreateLineString(new[]
                {
                    new Coordinate(random.Next(1000), random.Next(1000)),
                    new Coordinate(random.Next(1000), random.Next(1000))
                }),
                Type = BoundaryType.Fault
            });
        }
        index.BuildIndex();
        
        var queryPoint = new Vector3(500, 500, 0);
        
        // Act
        var results10 = index.QueryRadius(queryPoint, 10);
        var results50 = index.QueryRadius(queryPoint, 50);
        var results100 = index.QueryRadius(queryPoint, 100);
        
        // Assert - Monotonic property
        Assert.LessOrEqual(results10.Count, results50.Count);
        Assert.LessOrEqual(results50.Count, results100.Count);
    }
}
```

## Performance Test Specifications

### Load Tests

```csharp
[TestFixture]
[Category("Performance")]
public class PerformanceTests
{
    [Test]
    [Timeout(10000)]
    public void GridCreation_LargeGrid_CompletesInTime()
    {
        // Arrange
        var stopwatch = Stopwatch.StartNew();
        
        // Act
        var grid = DenseSimulationGrid.Create(
            "large-grid",
            new Envelope(0, 0, 2000, 2000),
            1.0
        );
        
        stopwatch.Stop();
        
        // Assert
        Assert.Less(stopwatch.ElapsedMilliseconds, 500);
        TestContext.WriteLine($"Grid creation: {stopwatch.ElapsedMilliseconds}ms");
    }
    
    [Test]
    [Timeout(5000)]
    public void QueryMaterial_ThousandQueries_MeetsPerformanceTarget()
    {
        // Arrange
        var storage = new GridVectorHybridStorage(new MockOctreeStorage());
        var grid = storage.GetOrCreateGrid(
            "perf-test",
            new Envelope(0, 0, 1000, 1000),
            1.0
        );
        
        var processor = new HybridQueryProcessor(
            storage,
            storage._precisionBoundaries,
            new MockOctreeStorage()
        );
        
        var random = new Random(42);
        var queryPoints = Enumerable.Range(0, 1000)
            .Select(_ => new Vector3(
                random.Next(1000),
                random.Next(1000),
                0
            ))
            .ToList();
        
        // Act
        var stopwatch = Stopwatch.StartNew();
        foreach (var point in queryPoints)
        {
            processor.QueryMaterial(point, new QueryContext());
        }
        stopwatch.Stop();
        
        // Assert - 1000 queries in < 1 second
        Assert.Less(stopwatch.ElapsedMilliseconds, 1000);
        var avgMs = stopwatch.ElapsedMilliseconds / 1000.0;
        TestContext.WriteLine($"Average query time: {avgMs:F3}ms");
    }
}
```

## Test Data Generators

### Helper Classes

```csharp
public static class TestDataGenerator
{
    public static DenseSimulationGrid CreateTestGrid(
        int size = 100,
        double cellSize = 1.0)
    {
        return DenseSimulationGrid.Create(
            $"test-{Guid.NewGuid()}",
            new Envelope(0, 0, size, size),
            cellSize
        );
    }
    
    public static BoundaryFeature CreateRandomBoundary(
        string id = null,
        Random random = null)
    {
        random ??= new Random();
        id ??= Guid.NewGuid().ToString();
        
        var factory = new GeometryFactory();
        var coords = new[]
        {
            new Coordinate(random.Next(1000), random.Next(1000)),
            new Coordinate(random.Next(1000), random.Next(1000))
        };
        
        return new BoundaryFeature
        {
            Id = id,
            Geometry = factory.CreateLineString(coords),
            Type = (BoundaryType)random.Next(6),
            Priority = random.NextDouble(),
            MaterialTransition = new MaterialTransition
            {
                LeftMaterial = new MaterialId(random.Next(100)),
                RightMaterial = new MaterialId(random.Next(100)),
                Type = (TransitionType)random.Next(3)
            }
        };
    }
    
    public static void FillGridWithPattern(
        DenseSimulationGrid grid,
        Func<int, int, MaterialId> pattern)
    {
        for (int y = 0; y < grid.Size.Height; y++)
        {
            for (int x = 0; x < grid.Size.Width; x++)
            {
                grid.MaterialGrid[y, x] = pattern(x, y);
            }
        }
    }
}
```

## Test Coverage Targets

| Component | Target Coverage | Priority |
|-----------|----------------|----------|
| DenseSimulationGrid | > 95% | Critical |
| VectorBoundaryIndex | > 90% | Critical |
| GridVectorSynchronizer | > 85% | High |
| HybridQueryProcessor | > 90% | Critical |
| ErosionProcessor | > 80% | High |
| ThermalDiffusionProcessor | > 75% | Medium |

## Continuous Integration

### Test Execution Strategy

```yaml
# .github/workflows/tests.yml
name: Grid Vector Tests

on: [push, pull_request]

jobs:
  unit-tests:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v3
      - name: Setup .NET
        uses: actions/setup-dotnet@v3
        with:
          dotnet-version: '6.0.x'
      
      - name: Run Unit Tests
        run: dotnet test --filter Category=Unit --logger "trx;LogFileName=unit-tests.trx"
      
      - name: Publish Test Results
        uses: EnricoMi/publish-unit-test-result-action@v2
        if: always()
        with:
          files: '**/unit-tests.trx'
  
  integration-tests:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v3
      - name: Setup .NET
        uses: actions/setup-dotnet@v3
        with:
          dotnet-version: '6.0.x'
      
      - name: Run Integration Tests
        run: dotnet test --filter Category=Integration
  
  performance-tests:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v3
      - name: Setup .NET
        uses: actions/setup-dotnet@v3
        with:
          dotnet-version: '6.0.x'
      
      - name: Run Performance Tests
        run: dotnet test --filter Category=Performance
```

## Validation Criteria

### Acceptance Criteria

1. **Functional Correctness**
   - All unit tests pass
   - All integration tests pass
   - Property-based invariants hold

2. **Performance**
   - Grid creation < 500ms for 2000x2000 cells
   - Material queries < 1ms average
   - Erosion processing > 1M cells/second

3. **Reliability**
   - No memory leaks detected
   - Thread-safe operations verified
   - Edge cases handled correctly

4. **Code Quality**
   - Test coverage > 80% overall
   - Critical paths > 95% coverage
   - All tests documented

## Conclusion

This comprehensive test specification ensures the Grid + Vector hybrid storage system meets all quality, performance, and reliability requirements. The test suite covers unit, integration, property-based, and performance testing to validate the system from multiple angles.

**Key Points**:
- 60% unit tests, 30% integration tests, 10% system tests
- Property-based testing for invariants
- Performance tests with specific targets
- Comprehensive test data generators
- CI/CD integration for automated testing
