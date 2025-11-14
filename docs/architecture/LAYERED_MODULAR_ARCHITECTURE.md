# Layered Modular Architecture Guide

## Overview

This document outlines the layered modular architecture pattern used in BlueMarble, demonstrating how to design systems that are easier to navigate, extend, and maintain through clear hierarchies, appropriate design patterns, and strong separation of concerns.

## Architectural Principles

### 1. Clear Layer Separation

Each layer has a distinct responsibility and communicates through well-defined interfaces:

```
┌─────────────────────────────────────────────────────────┐
│  Layer 4: Domain Adapters                                │
│  (GeomorphologicalOctreeAdapter)                         │
│  - Business logic integration                            │
│  - Domain-specific operations                            │
└──────────────────────┬──────────────────────────────────┘
                       │
┌──────────────────────▼──────────────────────────────────┐
│  Layer 3: Performance & Strategy                         │
│  (DeltaPatchOctree)                                      │
│  - Optimization strategies                               │
│  - Caching and delta management                          │
│  - Configurable algorithms                               │
└──────────────────────┬──────────────────────────────────┘
                       │
┌──────────────────────▼──────────────────────────────────┐
│  Layer 2: Core Data Structures                           │
│  (OptimizedOctreeNode)                                   │
│  - Fundamental data organization                         │
│  - Memory-efficient storage                              │
│  - Inheritance and traversal                             │
└──────────────────────┬──────────────────────────────────┘
                       │
┌──────────────────────▼──────────────────────────────────┐
│  Layer 1: Data Models                                    │
│  (MaterialData, MaterialId)                              │
│  - Value types and primitives                            │
│  - Domain entities                                       │
└─────────────────────────────────────────────────────────┘
```

### 2. Design Patterns in Practice

#### Adapter Pattern
The `GeomorphologicalOctreeAdapter` demonstrates the Adapter pattern by providing a domain-specific interface to the underlying delta octree system:

```csharp
public class GeomorphologicalOctreeAdapter
{
    private readonly DeltaPatchOctree _deltaOctree;
    
    // Adapts generic octree operations to geological domain
    public void ApplyErosion(IEnumerable<Vector3> positions, float erosionRate)
    public void ApplyDeposition(IEnumerable<(Vector3, MaterialData)> depositions)
    public void ApplyVolcanicIntrusion(Vector3 center, float radius, MaterialData material)
}
```

**Benefits**:
- Isolates domain logic from data structure implementation
- Allows swapping underlying storage without changing domain code
- Provides clear, business-focused API

#### Strategy Pattern
The `DeltaCompactionStrategy` enum enables runtime selection of consolidation algorithms:

```csharp
public enum DeltaCompactionStrategy
{
    LazyThreshold,        // Consolidate oldest 50% when threshold hit
    SpatialClustering,    // Group by proximity and consolidate clusters
    TimeBasedBatching     // Consolidate deltas older than 1 hour
}
```

**Benefits**:
- Algorithm selection at runtime based on usage patterns
- Easy to add new strategies without modifying existing code
- Testable in isolation

#### Template Method Pattern
Delta consolidation follows the Template Method pattern:

```csharp
private void TriggerDeltaConsolidation()
{
    // Template method - delegates to strategy-specific implementation
    switch (_compactionStrategy)
    {
        case DeltaCompactionStrategy.LazyThreshold:
            ConsolidateOldestDeltas();
            break;
        case DeltaCompactionStrategy.SpatialClustering:
            ConsolidateSpatialClusters();
            break;
        case DeltaCompactionStrategy.TimeBasedBatching:
            ConsolidateByAge();
            break;
    }
}
```

**Benefits**:
- Common consolidation framework with varying implementations
- Each strategy implements specific selection logic
- Maintains consistent interface

### 3. Composition Over Inheritance

The architecture favors composition over deep inheritance hierarchies:

```csharp
public class DeltaPatchOctree
{
    private readonly OptimizedOctreeNode _baseTree;  // Composition
    private readonly ConcurrentDictionary<Vector3, MaterialDelta> _deltas;
    
    // No inheritance - clean dependency injection
}

public class GeomorphologicalOctreeAdapter
{
    private readonly DeltaPatchOctree _deltaOctree;  // Composition
    
    // Adapter wraps rather than extends
}
```

**Benefits**:
- Flexible relationships that can be changed at runtime
- No fragile base class problems
- Clear ownership and lifecycle management
- Easier to test with mocking

### 4. Single Responsibility Principle

Each class has one clear purpose:

| Class | Responsibility | Why Not More? |
|-------|---------------|---------------|
| `MaterialData` | Represent material properties | Doesn't know about storage or operations |
| `OptimizedOctreeNode` | Hierarchical storage with inheritance | Doesn't know about deltas or domain operations |
| `DeltaPatchOctree` | Delta overlay and consolidation | Doesn't know about geological processes |
| `GeomorphologicalOctreeAdapter` | Geological domain operations | Doesn't know about octree internals |

### 5. Dependency Inversion

Higher-level modules depend on abstractions, not concrete implementations:

```csharp
// GeomorphologicalOctreeAdapter depends on DeltaPatchOctree interface
// Can be tested with mock or alternative implementations
public GeomorphologicalOctreeAdapter(DeltaPatchOctree deltaOctree)
{
    _deltaOctree = deltaOctree ?? throw new ArgumentNullException(nameof(deltaOctree));
}

// DeltaPatchOctree depends on OptimizedOctreeNode interface
public DeltaPatchOctree(
    OptimizedOctreeNode baseTree,
    int consolidationThreshold = 1000,
    DeltaCompactionStrategy compactionStrategy = DeltaCompactionStrategy.LazyThreshold)
```

## Benefits Achieved

### 1. High Reusability
- `OptimizedOctreeNode` can be used independently for any hierarchical spatial data
- `DeltaPatchOctree` can be reused for any delta overlay scenario
- `GeomorphologicalOctreeAdapter` demonstrates how to create domain-specific adapters

### 2. Minimal Duplication
- Common octree logic written once in `OptimizedOctreeNode`
- Delta management logic centralized in `DeltaPatchOctree`
- No duplicate consolidation logic across strategies

### 3. Easy to Test
Each layer can be tested independently:

```csharp
// Test Layer 1: MaterialData in isolation
[Fact]
public void MaterialData_Equality_Works() { ... }

// Test Layer 2: OptimizedOctreeNode with mock parent
[Fact]
public void OptimizedOctreeNode_Inheritance_Works() { ... }

// Test Layer 3: DeltaPatchOctree with mock base tree
[Fact]
public void DeltaPatchOctree_DeltaOverlay_Works() { ... }

// Test Layer 4: GeomorphologicalOctreeAdapter with mock delta octree
[Fact]
public void Adapter_ApplyErosion_UpdatesCorrectVoxels() { ... }
```

### 4. Clear Extension Points

Adding new features is straightforward:

**New Material Types**: Add to `MaterialId` enum in Layer 1
**New Storage Strategy**: Extend `OptimizedOctreeNode` in Layer 2
**New Consolidation Strategy**: Add to `DeltaCompactionStrategy` in Layer 3
**New Geological Process**: Add method to `GeomorphologicalOctreeAdapter` in Layer 4

## Team Guidelines

### When to Use Each Pattern

#### Use Adapter Pattern When:
- Integrating existing classes with incompatible interfaces
- Creating domain-specific views of generic data structures
- Need to support multiple implementations behind single interface

**Example**: `GeomorphologicalOctreeAdapter` adapts octree for geological simulations

#### Use Strategy Pattern When:
- Multiple algorithms solve same problem differently
- Need to select algorithm at runtime
- Want to avoid conditional logic for algorithm selection

**Example**: `DeltaCompactionStrategy` for different consolidation approaches

#### Use Template Method When:
- Multiple classes share same algorithm structure but differ in steps
- Want to avoid code duplication while allowing customization
- Need to enforce algorithm skeleton

**Example**: `TriggerDeltaConsolidation()` with strategy-specific implementations

#### Use Composition When:
- Need flexible relationships between objects
- Want to avoid deep inheritance hierarchies
- Objects have "has-a" rather than "is-a" relationships

**Example**: All classes in the system use composition over inheritance

### Keeping Inheritance Depth Minimal

**Rule**: Maximum inheritance depth of 2 (base + derived)

Current architecture has depth of 0 (no inheritance hierarchies):
- All classes are standalone or use composition
- Interfaces would be used if multiple implementations needed

**Why This Works**:
- No fragile base class problems
- Easy to understand class relationships
- Simple refactoring and maintenance

### Architecture Enforcement

#### Code Review Checklist
- [ ] Does class have single, clear responsibility?
- [ ] Are dependencies injected via constructor?
- [ ] Is composition used instead of inheritance?
- [ ] Are design patterns applied appropriately?
- [ ] Can class be tested in isolation?
- [ ] Are layer boundaries respected?

#### Automated Checks
Consider tools like:
- **Architecture Tests**: Use ArchUnitNET to enforce layer dependencies
- **Code Metrics**: Track cyclomatic complexity and coupling
- **Dependency Analysis**: Visualize and validate dependency graph

#### Documentation Requirements
Every module should have:
- Clear layer identification
- Design patterns used
- Extension points
- Testing strategy

## Anti-Patterns to Avoid

### 1. Layer Violations
❌ **Don't**: Allow Layer 1 (MaterialData) to depend on Layer 3 (DeltaPatchOctree)
✅ **Do**: Keep dependencies flowing downward through layers

### 2. God Classes
❌ **Don't**: Create `OctreeManager` that does everything
✅ **Do**: Separate concerns into focused classes like current architecture

### 3. Primitive Obsession
❌ **Don't**: Use `int` or `string` for material types
✅ **Do**: Use strongly-typed `MaterialId` enum

### 4. Deep Inheritance
❌ **Don't**: Create `BaseOctreeNode -> OptimizedOctreeNode -> DeltaOctreeNode -> ...`
✅ **Do**: Use composition like `DeltaPatchOctree` containing `OptimizedOctreeNode`

### 5. Circular Dependencies
❌ **Don't**: Have Layer 2 depend on Layer 3 which depends on Layer 2
✅ **Do**: Maintain clear unidirectional dependencies

## Real-World Example: Adding a New Feature

### Scenario: Add Climate Simulation

Following the layered architecture:

#### Step 1: Data Model (Layer 1)
```csharp
public struct ClimateData
{
    public float Temperature { get; set; }
    public float Humidity { get; set; }
    public float Pressure { get; set; }
}
```

#### Step 2: Storage (Layer 2)
Reuse existing `OptimizedOctreeNode` - no changes needed!

#### Step 3: Performance (Layer 3)
```csharp
public class ClimateDeltaOctree
{
    private readonly DeltaPatchOctree _baseOctree;
    private readonly ConcurrentDictionary<Vector3, ClimateData> _climateDeltas;
    
    // Similar delta overlay pattern
}
```

#### Step 4: Domain Adapter (Layer 4)
```csharp
public class ClimateSimulationAdapter
{
    private readonly ClimateDeltaOctree _climateOctree;
    
    public void ApplyWeatherPattern(WeatherSystem pattern) { ... }
    public void SimulateAirflow(Vector3 wind) { ... }
}
```

**Total Impact**:
- 3 new classes
- 0 modifications to existing classes
- All existing tests still pass
- New feature is independently testable

## Performance Considerations

### Layer Overhead
- **Layer 1**: Zero overhead (value types)
- **Layer 2**: O(log n) traversal
- **Layer 3**: O(1) delta lookup + O(log n) fallback
- **Layer 4**: Thin adapter, minimal overhead

### Memory Efficiency
- Layer 2: 80-95% memory reduction through inheritance
- Layer 3: Only stores deltas, not full state
- Layer 4: No additional storage

### Optimization Guidelines
1. Profile before optimizing
2. Optimize hot paths within layer boundaries
3. Use composition to inject optimized implementations
4. Don't sacrifice architecture for premature optimization

## Testing Strategy

### Unit Tests
Test each layer independently:

```csharp
// Layer 1: Data Model Tests
public class MaterialDataTests
{
    [Fact] public void Equality_Works() { ... }
    [Fact] public void DefaultMaterials_Valid() { ... }
}

// Layer 2: Core Structure Tests
public class OptimizedOctreeNodeTests
{
    [Fact] public void Inheritance_Works() { ... }
    [Fact] public void Homogeneity_Calculated() { ... }
}

// Layer 3: Performance Layer Tests
public class DeltaPatchOctreeTests
{
    [Fact] public void DeltaOverlay_Works() { ... }
    [Fact] public void Consolidation_Reduces_Deltas() { ... }
}

// Layer 4: Domain Adapter Tests
public class GeomorphologicalOctreeAdapterTests
{
    [Fact] public void ApplyErosion_UpdatesVoxels() { ... }
    [Fact] public void ApplyDeposition_AddsVoxels() { ... }
}
```

### Integration Tests
Test layer interactions:

```csharp
public class DeltaOverlaySystemTests
{
    [Fact]
    public void EndToEnd_ErosionWorkflow_Works()
    {
        // Setup all layers
        var node = new OptimizedOctreeNode();
        var delta = new DeltaPatchOctree(node);
        var adapter = new GeomorphologicalOctreeAdapter(delta);
        
        // Execute workflow across layers
        adapter.ApplyErosion(...);
        
        // Verify results
        Assert.Equal(expected, adapter.GetMaterial(...));
    }
}
```

### Architecture Tests
Validate architectural rules:

```csharp
[Fact]
public void Layer4_DoesNotDependOn_Layer4()
{
    // Use ArchUnitNET or similar
    var layer4 = Types.InAssembly(typeof(GeomorphologicalOctreeAdapter).Assembly)
        .That().ResideInNamespace("*.Layer4");
    
    Assert.DoesNotDependOn(layer4, layer4);
}
```

## Conclusion

A layered modular architecture provides:

✅ **Easier to Navigate**: Clear structure shows where code belongs
✅ **Easier to Extend**: Well-defined extension points at each layer
✅ **Easier to Maintain**: Changes isolated to affected layers
✅ **Easier to Test**: Each layer testable independently
✅ **Better Team Collaboration**: Clear ownership and conventions

The BlueMarble.SpatialData module demonstrates these principles:
- 4 clear layers with distinct responsibilities
- Composition over inheritance throughout
- Appropriate design patterns (Adapter, Strategy, Template Method)
- Zero circular dependencies
- 100% test coverage
- Minimal code duplication

**Remember**: Architecture is about people. Ensure the team understands the layering scheme, follows conventions, and uses code reviews to maintain standards. Treat the architecture as a contract - each module does its part without overstepping.

## References

- [Design Patterns: Elements of Reusable Object-Oriented Software](https://en.wikipedia.org/wiki/Design_Patterns) (Gang of Four)
- [Clean Architecture](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html) by Robert C. Martin
- [SOLID Principles](https://en.wikipedia.org/wiki/SOLID)
- BlueMarble.SpatialData Implementation (reference implementation)
