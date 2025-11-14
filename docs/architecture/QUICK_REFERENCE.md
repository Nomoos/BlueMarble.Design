# Architecture Quick Reference

## When to Use Which Pattern

### Adapter Pattern
**Use when**: Creating domain-specific interfaces to generic systems

**Example**: `GeomorphologicalOctreeAdapter` provides geological operations on top of generic octree

**Template**:
```csharp
public class DomainAdapter
{
    private readonly GenericSystem _system;
    
    public DomainAdapter(GenericSystem system)
    {
        _system = system;
    }
    
    public void DomainSpecificOperation()
    {
        // Translate domain concepts to system operations
        _system.GenericOperation();
    }
}
```

### Strategy Pattern
**Use when**: Multiple algorithms solve same problem, need runtime selection

**Example**: `DeltaCompactionStrategy` for different consolidation approaches

**Template**:
```csharp
public enum ConsolidationStrategy
{
    ApproachA,
    ApproachB,
    ApproachC
}

public class System
{
    private readonly ConsolidationStrategy _strategy;
    
    private void Execute()
    {
        switch (_strategy)
        {
            case ApproachA: ExecuteA(); break;
            case ApproachB: ExecuteB(); break;
            case ApproachC: ExecuteC(); break;
        }
    }
}
```

### Template Method Pattern
**Use when**: Common algorithm structure, varying steps

**Example**: `TriggerDeltaConsolidation()` with strategy-specific implementations

**Template**:
```csharp
public abstract class Algorithm
{
    public void Execute()
    {
        Step1();
        Step2();  // Varies by implementation
        Step3();
    }
    
    protected abstract void Step2();
}
```

## Layer Assignment Guide

### Layer 1: Data Models
**What belongs here**:
- Value types (structs)
- Enums
- Simple data containers
- Domain entities with no behavior

**What doesn't**:
- Business logic
- Storage operations
- External dependencies

**Example**: `MaterialData`, `MaterialId`

### Layer 2: Core Data Structures
**What belongs here**:
- Fundamental algorithms
- Data structure implementations
- Low-level operations
- No domain knowledge

**What doesn't**:
- Domain-specific operations
- Optimization strategies
- External systems

**Example**: `OptimizedOctreeNode`

### Layer 3: Performance & Strategy
**What belongs here**:
- Optimization layers
- Caching strategies
- Algorithm selection
- Performance-critical code

**What doesn't**:
- Domain logic
- UI concerns
- Business rules

**Example**: `DeltaPatchOctree`

### Layer 4: Domain Adapters
**What belongs here**:
- Business logic
- Domain operations
- Use case implementations
- External integrations

**What doesn't**:
- Low-level data structures
- Performance optimizations
- Storage details

**Example**: `GeomorphologicalOctreeAdapter`

## Composition vs Inheritance Decision Tree

```
Does the relationship represent "is-a"?
├─ No → Use Composition
└─ Yes → Will it change over time?
    ├─ Yes → Use Composition
    └─ No → Is hierarchy depth ≤ 2?
        ├─ No → Use Composition
        └─ Yes → Could interface work?
            ├─ Yes → Use Interface
            └─ No → Inheritance OK
```

## Code Review Checklist

### Architecture
- [ ] Class in correct layer?
- [ ] Dependencies flow downward?
- [ ] No circular dependencies?
- [ ] Appropriate pattern used?

### Design
- [ ] Single responsibility?
- [ ] Composition over inheritance?
- [ ] Dependencies injected?
- [ ] Can be tested in isolation?

### Code Quality
- [ ] Clear naming?
- [ ] Appropriate comments?
- [ ] No code duplication?
- [ ] Follows SOLID principles?

## Common Mistakes

### ❌ Layer Violation
```csharp
// WRONG: Layer 1 depending on Layer 3
public struct MaterialData
{
    public void SaveToOctree(DeltaPatchOctree octree) { }  // BAD!
}
```

### ❌ Deep Inheritance
```csharp
// WRONG: Inheritance depth > 2
public class A { }
public class B : A { }
public class C : B { }  // Too deep!
```

### ❌ Missing Dependency Injection
```csharp
// WRONG: Creating dependencies inside class
public class Adapter
{
    private DeltaPatchOctree _octree = new DeltaPatchOctree();  // BAD!
}
```

### ❌ God Class
```csharp
// WRONG: Too many responsibilities
public class OctreeManager
{
    public void ReadVoxel() { }
    public void WriteVoxel() { }
    public void ApplyErosion() { }
    public void RenderTerrain() { }  // Too much!
    public void SaveToDatabase() { }  // Way too much!
}
```

## Correct Examples

### ✅ Proper Layering
```csharp
// Layer 1: Data Model
public struct MaterialData { }

// Layer 2: Core Structure
public class OptimizedOctreeNode { }

// Layer 3: Performance
public class DeltaPatchOctree
{
    private readonly OptimizedOctreeNode _baseTree;  // Composition
}

// Layer 4: Domain
public class GeomorphologicalOctreeAdapter
{
    private readonly DeltaPatchOctree _deltaOctree;  // Composition
}
```

### ✅ Dependency Injection
```csharp
public class Adapter
{
    private readonly IDeltaOctree _octree;
    
    public Adapter(IDeltaOctree octree)  // Injected!
    {
        _octree = octree ?? throw new ArgumentNullException(nameof(octree));
    }
}
```

### ✅ Single Responsibility
```csharp
// Each class does ONE thing
public class MaterialData { }  // Stores properties
public class OctreeStorage { }  // Manages storage
public class ErosionSimulator { }  // Simulates erosion
```

## Testing Guidelines

### Layer 1: Data Models
```csharp
[Fact]
public void MaterialData_Equality_Works()
{
    var m1 = new MaterialData(MaterialId.Rock, 2700f, 7f);
    var m2 = new MaterialData(MaterialId.Rock, 2700f, 7f);
    Assert.Equal(m1, m2);
}
```

### Layer 2: Core Structures
```csharp
[Fact]
public void OctreeNode_Inheritance_Works()
{
    var parent = new OptimizedOctreeNode { ExplicitMaterial = testMaterial };
    var child = new OptimizedOctreeNode { Parent = parent };
    Assert.Equal(testMaterial, child.GetEffectiveMaterial());
}
```

### Layer 3: Performance
```csharp
[Fact]
public void DeltaOctree_Consolidation_ReducesDeltas()
{
    var octree = new DeltaPatchOctree(baseNode);
    octree.WriteVoxel(pos, material);
    var before = octree.ActiveDeltaCount;
    octree.ConsolidateDeltas();
    Assert.Equal(0, octree.ActiveDeltaCount);
}
```

### Layer 4: Domain Adapters
```csharp
[Fact]
public void Adapter_ApplyErosion_RemovesMaterial()
{
    var adapter = new GeomorphologicalOctreeAdapter(mockOctree);
    adapter.ApplyErosion(positions, 1.0f);
    Assert.Equal(MaterialId.Air, adapter.GetMaterial(position).MaterialType);
}
```

## Quick Reference Links

- [Full Architecture Guide](./LAYERED_MODULAR_ARCHITECTURE.md)
- [ADR-001: Layered Architecture](./adr/ADR-001-layered-architecture.md)
- [ADR-002: Composition Over Inheritance](./adr/ADR-002-composition-over-inheritance.md)
- [Research: Pattern Analysis](../../research/topics/layered-modular-architecture-pattern.md)
- [Reference Implementation](../../src/BlueMarble.SpatialData/)
