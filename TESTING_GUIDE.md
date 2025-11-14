# Testing and Mocking Guide

This guide describes the testing and mocking patterns implemented in the BlueMarble.Design project, demonstrating how to test layered architectures in isolation using dependency injection and mock implementations.

## Table of Contents

- [Architecture Overview](#architecture-overview)
- [Testing Principles](#testing-principles)
- [Interfaces for Testability](#interfaces-for-testability)
- [Mock Implementations](#mock-implementations)
- [Unit Testing Layers in Isolation](#unit-testing-layers-in-isolation)
- [Integration Testing](#integration-testing)
- [Best Practices](#best-practices)

## Architecture Overview

The BlueMarble.Design project follows a layered architecture:

```
┌─────────────────────────────────────────┐
│  GeomorphologicalOctreeAdapter          │
│  (Geological Processes Layer)           │
│  IGeologicalProcessor                   │
└──────────────┬──────────────────────────┘
               │ uses
               ▼
┌─────────────────────────────────────────┐
│  DeltaPatchOctree                       │
│  (Delta Overlay Layer)                  │
│  IDeltaStorage                          │
└──────────────┬──────────────────────────┘
               │ uses
               ▼
┌─────────────────────────────────────────┐
│  OptimizedOctreeNode                    │
│  (Base Octree Layer)                    │
│  IOctreeStorage                         │
└─────────────────────────────────────────┘
               │
               ▼
┌─────────────────────────────────────────┐
│  MaterialData                           │
│  (Data Structure)                       │
└─────────────────────────────────────────┘
```

## Testing Principles

### 1. Unit Test Layers in Isolation

Each layer should be tested independently with its dependencies mocked. This ensures:
- **Fast tests**: No need to initialize complex dependencies
- **Reliable tests**: Failures are isolated to the layer being tested
- **Clear purpose**: Tests focus on one layer's behavior

Example:
```csharp
[TestMethod]
public void DeltaLayer_WriteVoxel_UsesOctreeStorageForBaseMaterial()
{
    // Arrange - Create mock octree storage
    var mockOctree = new MockOctreeStorage(MaterialData.DefaultOcean);
    var deltaOctree = new DeltaPatchOctree(mockOctree);
    
    var position = new Vector3(10, 20, 30);
    var newMaterial = new MaterialData(MaterialId.Sand, 1600f, 2.5f);
    
    // Act - Write new material
    deltaOctree.WriteVoxel(position, newMaterial);
    
    // Assert - Verify mock was called
    Assert.AreEqual(1, mockOctree.GetMaterialAtCallCount);
    Assert.IsTrue(deltaOctree.HasDelta(position));
}
```

### 2. Use Mocks and Stubs to Isolate Dependencies

Mock implementations provide controlled, predictable behavior for testing:
- **Verify interactions**: Track method calls and parameters
- **Simulate failures**: Test error handling paths
- **Control responses**: Return specific test data

Example:
```csharp
var mockDelta = new MockDeltaStorage(MaterialData.DefaultOcean);
mockDelta.SimulateReadFailure = true; // Test error handling

// This will throw due to simulated failure
Assert.ThrowsException<InvalidOperationException>(() => 
    geoAdapter.ApplyErosion(positions, 1.0f));
```

### 3. Dependency Injection for Testability

All layers accept dependencies through constructor injection:

```csharp
// Production code uses concrete implementations
var baseTree = new OptimizedOctreeNode { ... };
var deltaOctree = new DeltaPatchOctree(baseTree);
var geoAdapter = new GeomorphologicalOctreeAdapter(deltaOctree);

// Test code injects mock implementations
var mockOctree = new MockOctreeStorage(defaultMaterial);
var deltaOctree = new DeltaPatchOctree(mockOctree); // Using interface
```

### 4. Test Behavior, Not Implementation

Tests focus on the public interface and expected behavior:

```csharp
[TestMethod]
public void DeltaStorage_Interface_SatisfiesContract()
{
    // Arrange - Use interface type
    IDeltaStorage deltaStorage = new DeltaPatchOctree(...);
    
    // Act & Assert - Test interface contract
    deltaStorage.WriteVoxel(position, material);
    Assert.IsTrue(deltaStorage.HasDelta(position));
    
    var retrieved = deltaStorage.ReadVoxel(position);
    Assert.AreEqual(material, retrieved);
}
```

### 5. Integration Tests for Layer Interaction

Integration tests verify that layers work together correctly:

```csharp
[TestMethod]
public void FullStack_MaterialToOctreeToDeltoToGeo_WorksEndToEnd()
{
    // Arrange - Build full stack with real implementations
    var baseTree = new OptimizedOctreeNode { ... };
    var deltaOctree = new DeltaPatchOctree(baseTree);
    var geoAdapter = new GeomorphologicalOctreeAdapter(deltaOctree);
    
    // Act - Apply process through full stack
    geoAdapter.ApplyDeposition(positions, sandMaterial);
    
    // Assert - Verify data flows through all layers
    var material = geoAdapter.GetMaterial(position);
    Assert.AreEqual(sandMaterial, material);
}
```

## Interfaces for Testability

### IOctreeStorage

Abstracts octree storage operations for testing the delta layer:

```csharp
public interface IOctreeStorage
{
    MaterialData GetMaterialAt(Vector3 position);
    void SetMaterialAt(Vector3 position, MaterialData material);
    Dictionary<Vector3, MaterialData> QueryRegion(Vector3 min, Vector3 max);
    MaterialData GetEffectiveMaterial();
}
```

### IDeltaStorage

Abstracts delta overlay operations for testing the geological adapter:

```csharp
public interface IDeltaStorage
{
    MaterialData ReadVoxel(Vector3 position);
    void WriteVoxel(Vector3 position, MaterialData newMaterial);
    void WriteMaterialBatch(IEnumerable<(Vector3 Position, MaterialData Material)> updates);
    bool HasDelta(Vector3 position);
    int ActiveDeltaCount { get; }
    void ConsolidateDeltas(int? threshold = null);
}
```

### IGeologicalProcessor

Abstracts geological process operations for integration testing:

```csharp
public interface IGeologicalProcessor
{
    void ApplyErosion(IEnumerable<Vector3> positions, float erosionRate);
    void ApplyDeposition(IEnumerable<Vector3> positions, MaterialData material);
    void ApplyVolcanicIntrusion(Vector3 center, float radius, MaterialData material);
    void ApplyTectonicDeformation(IEnumerable<Vector3> sourceRegion, Vector3 displacement);
    Dictionary<Vector3, MaterialData> QueryRegion(Vector3 min, Vector3 max);
}
```

## Mock Implementations

### MockOctreeStorage

Provides a simple in-memory dictionary-based implementation:

**Features:**
- Tracks method call counts for verification
- Provides default material for undefined positions
- Supports clearing for test setup

**Usage:**
```csharp
var mockOctree = new MockOctreeStorage(MaterialData.DefaultOcean);
mockOctree.SetMaterialAt(position, material);

// Later in test
Assert.AreEqual(expectedCount, mockOctree.GetMaterialAtCallCount);
```

### MockDeltaStorage

Simulates delta storage with controllable behavior:

**Features:**
- Tracks all method calls for verification
- Can simulate read/write failures for error testing
- Provides lightweight consolidation simulation

**Usage:**
```csharp
var mockDelta = new MockDeltaStorage(MaterialData.DefaultAir);
mockDelta.SimulateWriteFailure = true; // Test error path

Assert.ThrowsException<InvalidOperationException>(() =>
    mockDelta.WriteVoxel(position, material));
```

### MockGeologicalProcessor

Captures geological process calls for verification:

**Features:**
- Records all method calls and parameters
- Allows setting simulated region data
- Useful for verifying process orchestration

**Usage:**
```csharp
var mockGeo = new MockGeologicalProcessor();
mockGeo.ApplyErosion(positions, 0.5f);

Assert.AreEqual(1, mockGeo.ApplyErosionCallCount);
Assert.AreEqual(0.5f, mockGeo.LastErosionRate);
```

## Unit Testing Layers in Isolation

### Testing DeltaPatchOctree with Mocked Octree

```csharp
[TestMethod]
public void DeltaLayer_ConsolidateDeltas_WritesToOctreeStorage()
{
    // Arrange - Mock the lower layer (octree storage)
    var mockOctree = new MockOctreeStorage(MaterialData.DefaultOcean);
    var deltaOctree = new DeltaPatchOctree(mockOctree);
    
    // Write several deltas
    var material = new MaterialData(MaterialId.Sand, 1600f, 2.5f);
    deltaOctree.WriteVoxel(new Vector3(1, 1, 1), material);
    deltaOctree.WriteVoxel(new Vector3(2, 2, 2), material);
    deltaOctree.WriteVoxel(new Vector3(3, 3, 3), material);
    
    int setCallsBefore = mockOctree.SetMaterialAtCallCount;
    
    // Act - Consolidate deltas
    deltaOctree.ConsolidateDeltas();
    
    // Assert - Verify interaction with mock
    Assert.AreEqual(3, mockOctree.SetMaterialAtCallCount - setCallsBefore);
    Assert.AreEqual(0, deltaOctree.ActiveDeltaCount);
}
```

### Testing GeomorphologicalOctreeAdapter with Mocked Delta Storage

```csharp
[TestMethod]
public void GeoAdapter_ApplyErosion_UsesDeltaStorageForReadsAndWrites()
{
    // Arrange - Mock the lower layer (delta storage)
    var mockDelta = new MockDeltaStorage(MaterialData.DefaultOcean);
    var geoAdapter = new GeomorphologicalOctreeAdapter(mockDelta);
    
    var positions = new List<Vector3> { /* ... */ };
    
    // Act - Apply geological process
    geoAdapter.ApplyErosion(positions, 0.5f);
    
    // Assert - Verify layer interaction
    Assert.IsTrue(mockDelta.ReadVoxelCallCount >= positions.Count);
    Assert.AreEqual(1, mockDelta.WriteMaterialBatchCallCount);
}
```

### Testing Error Handling

```csharp
[TestMethod]
[ExpectedException(typeof(InvalidOperationException))]
public void GeoAdapter_WithFailingDeltaStorage_PropagatesException()
{
    // Arrange - Mock that simulates failures
    var mockDelta = new MockDeltaStorage(MaterialData.DefaultOcean);
    mockDelta.SimulateReadFailure = true;
    var geoAdapter = new GeomorphologicalOctreeAdapter(mockDelta);
    
    // Act - This should throw due to simulated failure
    geoAdapter.ApplyErosion(new[] { new Vector3(0, 0, 0) }, 1.0f);
    
    // Assert - Exception expected
}
```

## Integration Testing

### Full Stack Integration

Tests that verify all layers work together:

```csharp
[TestMethod]
public void FullStack_MaterialToOctreeToDeltoToGeo_WorksEndToEnd()
{
    // Arrange - Build full stack with real implementations
    var baseTree = new OptimizedOctreeNode
    {
        ExplicitMaterial = MaterialData.DefaultOcean
    };
    var deltaOctree = new DeltaPatchOctree(baseTree);
    var geoAdapter = new GeomorphologicalOctreeAdapter(deltaOctree);
    
    // Act - Apply geological process through full stack
    var positions = new List<Vector3> { /* ... */ };
    var sandMaterial = new MaterialData(MaterialId.Sand, 1600f, 2.5f);
    geoAdapter.ApplyDeposition(positions, sandMaterial);
    
    // Assert - Verify data flows through all layers
    foreach (var pos in positions)
    {
        var material = geoAdapter.GetMaterial(pos);
        Assert.AreEqual(sandMaterial, material);
    }
}
```

### Layer Boundary Tests

Tests that verify correct interaction at layer boundaries:

```csharp
[TestMethod]
public void LayerBoundary_DeltaOverridesBase_HasCorrectPrecedence()
{
    // Arrange
    var baseTree = new OptimizedOctreeNode
    {
        ExplicitMaterial = new MaterialData(MaterialId.Rock, 2700f, 6.0f)
    };
    var deltaOctree = new DeltaPatchOctree(baseTree);
    
    // Act - Override base material with delta
    var position = new Vector3(0, 0, 0);
    var sandMaterial = new MaterialData(MaterialId.Sand, 1600f, 2.5f);
    deltaOctree.WriteVoxel(position, sandMaterial);
    
    // Assert - Delta should override base
    var material = deltaOctree.ReadVoxel(position);
    Assert.AreEqual(MaterialId.Sand, material.MaterialType);
}
```

### Sequential Process Tests

Tests that verify multiple operations work correctly in sequence:

```csharp
[TestMethod]
public void Integration_ErosionThenDeposition_HandlesSequentialProcesses()
{
    // Arrange
    var baseTree = new OptimizedOctreeNode { /* ... */ };
    var deltaOctree = new DeltaPatchOctree(baseTree);
    var geoAdapter = new GeomorphologicalOctreeAdapter(deltaOctree);
    
    var positions = new List<Vector3> { /* ... */ };
    
    // Act - Apply erosion then deposition
    geoAdapter.ApplyErosion(positions, 1.0f); // Complete erosion
    geoAdapter.ApplyDeposition(positions, sandMaterial);
    
    // Assert - Should have sand after both processes
    foreach (var pos in positions)
    {
        var material = geoAdapter.GetMaterial(pos);
        Assert.AreEqual(MaterialId.Sand, material.MaterialType);
    }
}
```

## Best Practices

### 1. Keep Tests Focused

Each test should verify one specific behavior:

✅ **Good:**
```csharp
[TestMethod]
public void DeltaLayer_WriteVoxel_CreatesNewDelta()
{
    // Test one thing: writing creates a delta
}

[TestMethod]
public void DeltaLayer_WriteVoxel_CallsOctreeForBaseMaterial()
{
    // Test another thing: interaction with octree
}
```

❌ **Bad:**
```csharp
[TestMethod]
public void DeltaLayer_WriteVoxel_DoesEverything()
{
    // Tests multiple behaviors in one test
}
```

### 2. Use Descriptive Test Names

Test names should clearly describe what they test:

✅ **Good:**
```csharp
DeltaLayer_ReadVoxel_FallsBackToOctreeStorage()
GeoAdapter_ApplyErosion_UsesDeltaStorageForReadsAndWrites()
```

❌ **Bad:**
```csharp
TestReadVoxel()
TestErosion()
```

### 3. Arrange-Act-Assert Pattern

Structure tests clearly:

```csharp
[TestMethod]
public void ExampleTest()
{
    // Arrange - Set up test data and dependencies
    var mock = new MockOctreeStorage(defaultMaterial);
    var sut = new DeltaPatchOctree(mock);
    
    // Act - Perform the operation being tested
    sut.WriteVoxel(position, material);
    
    // Assert - Verify the expected outcome
    Assert.IsTrue(sut.HasDelta(position));
}
```

### 4. Test Edge Cases

Include tests for boundary conditions and error scenarios:

```csharp
[TestMethod]
public void DeltaLayer_HandlesEmptyBatch_Gracefully()
{
    // Test with empty input
}

[TestMethod]
[ExpectedException(typeof(InvalidOperationException))]
public void GeoAdapter_WithFailingStorage_ThrowsException()
{
    // Test error handling
}
```

### 5. Maintain Backward Compatibility

When adding interfaces, keep original constructors:

```csharp
public class DeltaPatchOctree : IDeltaStorage
{
    // New constructor with interface for testability
    public DeltaPatchOctree(IOctreeStorage octreeStorage, ...)
    {
        // ...
    }

    // Original constructor for backward compatibility
    public DeltaPatchOctree(OptimizedOctreeNode baseTree, ...)
    {
        // ...
    }
}
```

### 6. Use Interfaces in Tests

Test against interfaces, not concrete types:

✅ **Good:**
```csharp
IDeltaStorage deltaStorage = new DeltaPatchOctree(mockOctree);
deltaStorage.WriteVoxel(position, material);
```

❌ **Bad:**
```csharp
var deltaStorage = new DeltaPatchOctree(mockOctree);
deltaStorage.WriteVoxel(position, material);
```

### 7. Document Test Intent

Use comments to explain complex test scenarios:

```csharp
[TestMethod]
public void ComplexScenario_ExplainsIntent()
{
    // This test verifies that when consolidation threshold is reached,
    // the oldest 50% of deltas are merged into the base octree while
    // preserving data integrity and maintaining query performance
    
    // Arrange
    // ...
}
```

## Test Coverage

The current implementation includes:

- **18 Original Tests**: Validate core functionality
- **13 Layer Isolation Tests**: Test each layer independently
- **10 Integration Tests**: Verify layer interactions

**Total: 41 tests, 100% passing**

### Test Distribution

| Layer | Unit Tests | Integration Tests |
|-------|------------|-------------------|
| MaterialData | Implicit | 1 |
| OptimizedOctreeNode | 4 (via mock) | 3 |
| DeltaPatchOctree | 4 | 5 |
| GeomorphologicalOctreeAdapter | 5 | 4 |
| Error Handling | 2 | 1 |
| Interface Contracts | 2 | - |

## Running Tests

```bash
# Run all tests
dotnet test BlueMarble.SpatialData.sln

# Run specific test class
dotnet test --filter "FullyQualifiedName~LayerIsolationTests"

# Run with detailed output
dotnet test --verbosity normal

# Run with code coverage
dotnet test /p:CollectCoverage=true
```

## Conclusion

This testing strategy demonstrates:

1. **Testability through Interfaces**: Clear contracts enable easy mocking
2. **Dependency Injection**: Enables substituting implementations for testing
3. **Mock Implementations**: Provide controlled test environments
4. **Layered Testing**: Unit tests verify individual layers, integration tests verify interaction
5. **Test Behavior**: Focus on public contracts, not implementation details

By following these patterns, you can ensure each layer is thoroughly tested in isolation while also verifying that layers cooperate correctly in the full system.
