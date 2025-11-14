# Architecture Review Guide

This guide helps code reviewers verify that pull requests adhere to the layered architecture defined in [ADR-002](./adr-002-layered-architecture-conventions.md).

## Quick Reference Checklist

Use this checklist when reviewing code changes:

- [ ] **Layer boundaries respected**: Dependencies flow downward only
- [ ] **Namespace organization correct**: Namespace matches layer
- [ ] **No code duplication**: Common code extracted to appropriate layer
- [ ] **Naming conventions followed**: Files, classes follow standards
- [ ] **Interfaces in correct layer**: Defined at lowest applicable level
- [ ] **No circular dependencies**: Projects don't depend on each other cyclically

## Detailed Review Steps

### Step 1: Identify Changed Layers

Review which layers are affected by the PR:

```
Application Layer (BlueMarble.Examples, BlueMarble.App.*)
     ↓
Domain Layer (BlueMarble.World, BlueMarble.World.*)
     ↓
Data Structures Layer (BlueMarble.SpatialData, BlueMarble.SpatialIndexing)
     ↓
Utility Layer (BlueMarble.Utils.*)
```

### Step 2: Verify Dependencies

For each changed file, check:

1. **Using statements**: Do they reference only lower layers?
2. **Type references**: Are all types from same or lower layers?
3. **Method calls**: Do they invoke only lower layer APIs?

#### ✅ Valid Dependencies

```csharp
// BlueMarble.World (Domain) can use BlueMarble.SpatialData (Data Structures)
namespace BlueMarble.World;

using BlueMarble.SpatialData;  // ✅ Lower layer

public class WorldSimulator
{
    private readonly DeltaPatchOctree _octree;  // ✅ Data structure from lower layer
}
```

#### ❌ Invalid Dependencies

```csharp
// BlueMarble.SpatialData (Data Structures) CANNOT use BlueMarble.World (Domain)
namespace BlueMarble.SpatialData;

using BlueMarble.World;  // ❌ Higher layer

public class Octree
{
    private WorldCoordinate _coord;  // ❌ Domain type from higher layer
}
```

### Step 3: Check for Code Duplication

Look for:

1. **Repeated algorithms**: Should be in Utils layer
2. **Duplicate domain logic**: Consolidate in Domain layer
3. **Copy-pasted code**: Extract to shared location

#### ❌ Duplication Example

```csharp
// In BlueMarble.World/ClassA.cs
public void ProcessData()
{
    var normalized = (value - min) / (max - min);  // ❌ Duplicated
}

// In BlueMarble.World/ClassB.cs
public void TransformData()
{
    var normalized = (value - min) / (max - min);  // ❌ Duplicated
}
```

#### ✅ Extracted to Utility

```csharp
// In BlueMarble.Utils/MathHelpers.cs
public static class MathHelpers
{
    public static double Normalize(double value, double min, double max)
    {
        return (value - min) / (max - min);
    }
}

// Usage in BlueMarble.World
var normalized = MathHelpers.Normalize(value, min, max);  // ✅ Reusable
```

### Step 4: Verify Namespace Organization

Check that:

1. **Namespace matches file location**
2. **Layer is clear from namespace**
3. **No mixing of concerns**

```
File: src/BlueMarble.World/Coordinates/WorldCoordinate.cs
Namespace: BlueMarble.World.Coordinates  ✅

File: src/BlueMarble.SpatialData/Octree/DeltaPatchOctree.cs
Namespace: BlueMarble.SpatialData.Octree  ✅ (or BlueMarble.SpatialData)
```

### Step 5: Review Naming Conventions

Verify names follow conventions:

| Type | Convention | Example |
|------|-----------|---------|
| Interface | I + Descriptive | `IOctreeNode`, `ISpatialIndex` |
| Base Class | Base suffix | `OctreeNodeBase`, `WorldEntityBase` |
| Implementation | Descriptive | `OptimizedOctreeNode`, `AccessibilityZoneClassifier` |
| Service | Service suffix | `WorldSimulatorService` |
| Data Class | Data/Dto suffix | `MaterialData`, `TerrainDto` |

### Step 6: Check Interface Placement

Interfaces should be in the lowest layer that uses them:

#### ✅ Correct Placement

```csharp
// Interface in Data Structures layer (lowest user)
namespace BlueMarble.SpatialData;
public interface IOctreeNode { }

// Implementation in same layer
namespace BlueMarble.SpatialData;
public class OptimizedOctreeNode : IOctreeNode { }

// Used by Domain layer
namespace BlueMarble.World;
public class WorldSimulator
{
    private readonly IOctreeNode _octree;  // ✅ Uses interface from lower layer
}
```

#### ❌ Incorrect Placement

```csharp
// ❌ Interface in Domain layer
namespace BlueMarble.World;
public interface IOctreeNode { }

// ❌ Data Structures layer can't implement interface from higher layer
namespace BlueMarble.SpatialData;
public class OptimizedOctreeNode : IOctreeNode { }  // ❌ Upward dependency
```

## Common Violations and Corrections

### Violation 1: Upward Dependency

**Problem**: Lower layer depends on higher layer

**How to spot**:
- Using statement references higher layer
- Type from higher layer used in field/property
- Method parameter from higher layer

**Example**:
```csharp
// ❌ In BlueMarble.SpatialData
using BlueMarble.World;

public class Octree
{
    private WorldCoordinate _position;  // ❌
}
```

**Correction**:
```csharp
// ✅ Use generic type
using System.Numerics;

public class Octree
{
    private Vector3 _position;  // ✅
}
```

**Review Comment Template**:
```
Per ADR-002 Section "Dependency Rules", the Data Structures layer cannot depend on the Domain layer.

Please use `Vector3` or create an interface in the Data Structures layer instead of using `WorldCoordinate` directly.

Reference: docs/architecture/adr-002-layered-architecture-conventions.md#dependency-rules
```

### Violation 2: Logic in Wrong Layer

**Problem**: Business logic in application layer or utility code in domain layer

**How to spot**:
- Complex algorithms in Examples/App projects
- Generic utilities in World project
- Domain-specific code in SpatialData project

**Example**:
```csharp
// ❌ In BlueMarble.Examples
public class GameExample
{
    public void UpdateWorld()
    {
        // ❌ Complex world simulation in application layer
        var erosion = CalculateErosion(terrain, rainfall, slope);
        var sediment = TransportSediment(erosion, waterFlow);
    }
}
```

**Correction**:
```csharp
// ✅ In BlueMarble.World
public class GeologicalSimulator
{
    public ErosionResult SimulateErosion(TerrainData terrain)
    {
        var erosion = CalculateErosion(terrain.Rainfall, terrain.Slope);
        return new ErosionResult(erosion);
    }
}

// ✅ In BlueMarble.Examples
public class GameExample
{
    private readonly GeologicalSimulator _simulator;
    
    public void UpdateWorld()
    {
        var result = _simulator.SimulateErosion(terrain);  // ✅ Delegate to domain
    }
}
```

**Review Comment Template**:
```
Per ADR-002 Section "Layer Responsibilities", business logic should be in the Domain layer (BlueMarble.World), not the Application layer.

Please move this simulation logic to a service class in BlueMarble.World and call it from the application layer.

Reference: docs/architecture/adr-002-layered-architecture-conventions.md#layer-responsibilities
```

### Violation 3: Circular Dependency

**Problem**: Two projects depend on each other

**How to spot**:
- Build warnings about circular references
- Project A references B, B references A
- Two classes in same layer depending on each other's internals

**Example**:
```csharp
// ❌ BlueMarble.SpatialData references BlueMarble.SpatialIndexing
// ❌ BlueMarble.SpatialIndexing references BlueMarble.SpatialData
```

**Correction**:
```csharp
// Create BlueMarble.Spatial.Abstractions
public interface ISpatialContainer { }

// BlueMarble.SpatialData implements interface
public class Octree : ISpatialContainer { }

// BlueMarble.SpatialIndexing depends on abstraction
public class Index
{
    private readonly ISpatialContainer _container;  // ✅
}
```

**Review Comment Template**:
```
This creates a circular dependency between BlueMarble.SpatialData and BlueMarble.SpatialIndexing, which violates ADR-002.

Consider extracting a shared interface to a common abstractions project, or restructuring the dependency to flow in one direction only.

Reference: docs/architecture/adr-002-layered-architecture-conventions.md#dependency-rules
```

### Violation 4: God Class

**Problem**: One class with too many responsibilities

**How to spot**:
- Class file > 500 lines
- Many unrelated public methods
- Class name is vague (Manager, Handler, Processor)
- Many dependencies injected

**Example**:
```csharp
// ❌ WorldManager doing everything
public class WorldManager
{
    public void GenerateTerrain() { }
    public void SimulatePhysics() { }
    public void UpdateWeather() { }
    public void ProcessErosion() { }
    public void ManageResources() { }
    // ... 50 more methods
}
```

**Correction**:
```csharp
// ✅ Split into focused classes
public class TerrainGenerator { }
public class PhysicsSimulator { }
public class WeatherSystem { }
public class ErosionProcessor { }
public class ResourceManager { }
```

**Review Comment Template**:
```
This class has too many responsibilities (violates Single Responsibility Principle from SOLID, referenced in ADR-002).

Please split this into multiple focused classes, each handling one specific aspect of the system.

Reference: docs/architecture/CODING_GUIDELINES.md#anti-patterns
```

### Violation 5: Leaky Abstraction

**Problem**: Implementation details exposed in public API

**How to spot**:
- Interface exposes concrete types (Dictionary, List)
- Public methods reveal internal structure
- Implementation-specific exceptions thrown

**Example**:
```csharp
// ❌ Exposing implementation
public interface ISpatialIndex
{
    ConcurrentDictionary<Vector3, Node> GetNodes();  // ❌ Implementation detail
}
```

**Correction**:
```csharp
// ✅ Abstract interface
public interface ISpatialIndex
{
    IEnumerable<Node> QueryNodes(BoundingBox bounds);  // ✅ Abstract operation
}
```

**Review Comment Template**:
```
This interface exposes implementation details (ConcurrentDictionary), which creates a leaky abstraction.

Please use IEnumerable or a custom return type that doesn't expose the internal data structure.

Reference: docs/architecture/CODING_GUIDELINES.md#anti-patterns
```

## Review Comment Templates

### Positive Feedback

```
✅ Excellent adherence to layered architecture. Dependencies flow correctly and abstractions are clean.
```

```
✅ Good use of interfaces to decouple layers. This will make testing and maintenance easier.
```

```
✅ Nice refactoring to extract common functionality to the Utils layer.
```

### Requesting Changes

```
Per ADR-002, [specific violation]. Please [specific fix].

Reference: [link to relevant section]
```

## Escalation Process

If a PR has significant architectural issues:

1. **Tag architecture lead** for consultation
2. **Request design discussion** before approval
3. **Reference this guide** and ADR-002 in comments
4. **Suggest pair programming** if needed
5. **Block merge** until architectural issues resolved

## Quick Tips

✅ **Do**:
- Reference ADR-002 when requesting changes
- Provide specific examples of fixes
- Explain *why* the architecture matters
- Offer to pair on complex refactorings
- Praise good architectural decisions

❌ **Don't**:
- Nitpick minor style issues when architecture is sound
- Block PRs for theoretical future problems
- Require perfect architecture on first PR from new contributor
- Use jargon without explanation

## Related Documents

- [ADR-002: Layered Architecture](./adr-002-layered-architecture-conventions.md)
- [Coding Guidelines](./CODING_GUIDELINES.md)
- [Automated Enforcement Guide](./AUTOMATED_ENFORCEMENT_GUIDE.md)
