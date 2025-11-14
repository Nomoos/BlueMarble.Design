# Coding Guidelines for BlueMarble

This document provides detailed coding standards and conventions for the BlueMarble project, supporting the layered architecture defined in [ADR-002](./adr-002-layered-architecture-conventions.md).

## Table of Contents

1. [Namespace Conventions](#namespace-conventions)
2. [Naming Conventions](#naming-conventions)
3. [File Organization](#file-organization)
4. [Code Templates](#code-templates)
5. [Common Patterns](#common-patterns)
6. [Anti-Patterns](#anti-patterns)

## Namespace Conventions

### Layer-Based Namespaces

Namespaces must clearly indicate which architectural layer they belong to:

**Utility Layer** - `BlueMarble.Utils.*`
```csharp
namespace BlueMarble.Utils.Spatial.DistributedStorage
namespace BlueMarble.Utils.Collections
namespace BlueMarble.Utils.Extensions
```

**Data Structures Layer** - `BlueMarble.SpatialData.*` and `BlueMarble.SpatialIndexing.*`
```csharp
namespace BlueMarble.SpatialData
namespace BlueMarble.SpatialData.Octree
namespace BlueMarble.SpatialIndexing
namespace BlueMarble.SpatialIndexing.Morton
```

**Domain Layer** - `BlueMarble.World.*`
```csharp
namespace BlueMarble.World
namespace BlueMarble.World.Constants
namespace BlueMarble.World.Simulation
namespace BlueMarble.World.Coordinates
```

**Application Layer** - `BlueMarble.Examples.*` and `BlueMarble.App.*`
```csharp
namespace BlueMarble.Examples
namespace BlueMarble.Examples.Spatial
namespace BlueMarble.App.Unity
namespace BlueMarble.App.Godot
```

### Project Structure

One C# project per major namespace:

```
src/
├── BlueMarble.Utils/
│   └── Spatial/
│       └── DistributedStorage/
├── BlueMarble.SpatialData/
├── BlueMarble.SpatialIndexing/
├── BlueMarble.World/
└── BlueMarble.Examples/
```

## Naming Conventions

### Interfaces

- Start with 'I'
- Use descriptive names that indicate purpose
- Place in the lowest applicable layer

```csharp
// Data Structures Layer
public interface IOctreeNode { }
public interface ISpatialIndex { }
public interface IDeltaOverlay { }

// Domain Layer
public interface IWorldCoordinate { }
public interface IAccessibilityZone { }
public interface IWorldSimulator { }
```

### Classes

**Base Classes**: Use 'Base' suffix or descriptive prefixes
```csharp
public abstract class OctreeNodeBase { }
public abstract class SpatialIndexBase { }
public abstract class WorldEntityBase { }
```

**Concrete Implementations**: Clear, descriptive names
```csharp
public class OptimizedOctreeNode : IOctreeNode { }
public class DeltaPatchOctree : IDeltaOverlay { }
public class AccessibilityZoneClassifier { }
public class CoordinateValidator { }
```

**Services**: Use descriptive suffixes
```csharp
public class WorldSimulatorService { }
public class DataPersistenceService { }
public class TerrainGenerationService { }
```

**Data Transfer Objects**: Use 'Dto' or 'Data' suffix
```csharp
public class WorldCoordinateData { }
public class MaterialData { }
public class TerrainDto { }
```

### Methods

- Use PascalCase for public methods
- Use camelCase for private methods
- Use verb-noun patterns

```csharp
// Good
public void ProcessWorld() { }
public MaterialData ReadVoxel(Vector3 position) { }
public void ConsolidateDeltas() { }

private void validateInput() { }
private void calculateBounds() { }
```

### Fields and Properties

```csharp
// Private fields: camelCase with underscore prefix
private readonly int _consolidationThreshold;
private Vector3 _worldOrigin;

// Public properties: PascalCase
public int ActiveDeltaCount { get; }
public MaterialData Material { get; set; }
```

## File Organization

### File Structure

1. **One class per file** (exceptions for tightly coupled nested classes)
2. **File name matches class name**: `DeltaPatchOctree.cs`
3. **Group related files in folders by feature**

Example structure:
```
BlueMarble.SpatialData/
├── Octree/
│   ├── OptimizedOctreeNode.cs
│   ├── DeltaPatchOctree.cs
│   ├── MaterialData.cs
│   └── MaterialDelta.cs
├── Compression/
│   ├── DeltaCompactionStrategy.cs
│   └── ConsolidationResult.cs
├── README.md
└── BlueMarble.SpatialData.csproj
```

### Code File Template

```csharp
using System;
using System.Collections.Generic;
// Group usings: System, then third-party, then project

namespace BlueMarble.SpatialData.Octree;

/// <summary>
/// Brief one-line description of the class
/// </summary>
/// <remarks>
/// Detailed description including:
/// - Performance characteristics
/// - Usage patterns
/// - Important notes
/// </remarks>
public class ExampleClass
{
    #region Private Fields
    
    private readonly int _field;
    
    #endregion
    
    #region Public Properties
    
    public int Property { get; }
    
    #endregion
    
    #region Constructors
    
    public ExampleClass(int value)
    {
        _field = value;
    }
    
    #endregion
    
    #region Public Methods
    
    public void DoSomething()
    {
        // Implementation
    }
    
    #endregion
    
    #region Private Methods
    
    private void helperMethod()
    {
        // Implementation
    }
    
    #endregion
}
```

## Code Templates

### Data Structure Class Template

```csharp
namespace BlueMarble.SpatialData;

/// <summary>
/// [Brief description]
/// </summary>
/// <remarks>
/// Performance: [O(n), O(log n), etc.]
/// Memory: [memory characteristics]
/// Thread-safety: [thread-safe/not thread-safe]
/// </remarks>
public class DataStructureName
{
    private readonly DataType _data;
    
    public PropertyType Property { get; }
    
    /// <summary>
    /// Creates a new instance
    /// </summary>
    /// <param name="param">Parameter description</param>
    public DataStructureName(ParamType param)
    {
        _data = param ?? throw new ArgumentNullException(nameof(param));
    }
    
    /// <summary>
    /// [Method description]
    /// </summary>
    /// <param name="input">Input description</param>
    /// <returns>Return value description</returns>
    public ResultType MethodName(InputType input)
    {
        // Implementation
        return new ResultType();
    }
}
```

### Domain Service Class Template

```csharp
namespace BlueMarble.World;

/// <summary>
/// [Brief description of domain service]
/// </summary>
public class ServiceName
{
    private readonly IDependency _dependency;
    
    /// <summary>
    /// Creates a new instance with dependencies
    /// </summary>
    /// <param name="dependency">Dependency description</param>
    public ServiceName(IDependency dependency)
    {
        _dependency = dependency ?? throw new ArgumentNullException(nameof(dependency));
    }
    
    /// <summary>
    /// [Business logic description]
    /// </summary>
    /// <param name="input">Input description</param>
    /// <returns>Result description</returns>
    public Result ProcessOperation(Input input)
    {
        if (input == null)
            throw new ArgumentNullException(nameof(input));
            
        // Business logic implementation
        var intermediate = _dependency.Transform(input);
        return new Result(intermediate);
    }
}
```

### Interface Template

```csharp
namespace BlueMarble.SpatialData;

/// <summary>
/// [Interface purpose]
/// </summary>
public interface IInterfaceName
{
    /// <summary>
    /// [Property description]
    /// </summary>
    PropertyType Property { get; }
    
    /// <summary>
    /// [Method description]
    /// </summary>
    /// <param name="input">Input description</param>
    /// <returns>Return value description</returns>
    ResultType MethodName(InputType input);
}
```

## Common Patterns

### Dependency Injection

Always inject dependencies through constructors:

```csharp
public class WorldSimulator
{
    private readonly ISpatialIndex _spatialIndex;
    private readonly ITerrainGenerator _terrainGenerator;
    
    public WorldSimulator(
        ISpatialIndex spatialIndex,
        ITerrainGenerator terrainGenerator)
    {
        _spatialIndex = spatialIndex ?? throw new ArgumentNullException(nameof(spatialIndex));
        _terrainGenerator = terrainGenerator ?? throw new ArgumentNullException(nameof(terrainGenerator));
    }
}
```

### Factory Pattern

For complex object creation:

```csharp
namespace BlueMarble.SpatialData;

public class OctreeFactory
{
    public IOctreeNode CreateOctree(OctreeConfig config)
    {
        // Validate configuration
        if (config == null)
            throw new ArgumentNullException(nameof(config));
            
        // Create and configure octree
        var octree = new OptimizedOctreeNode(
            config.MaxDepth,
            config.WorldSize,
            config.WorldOrigin);
            
        return octree;
    }
}
```

### Builder Pattern

For objects with many optional parameters:

```csharp
namespace BlueMarble.SpatialData;

public class OctreeBuilder
{
    private int _maxDepth = 20;
    private float _worldSize = 65536f;
    private Vector3 _worldOrigin = Vector3.Zero;
    
    public OctreeBuilder WithMaxDepth(int depth)
    {
        _maxDepth = depth;
        return this;
    }
    
    public OctreeBuilder WithWorldSize(float size)
    {
        _worldSize = size;
        return this;
    }
    
    public OctreeBuilder WithWorldOrigin(Vector3 origin)
    {
        _worldOrigin = origin;
        return this;
    }
    
    public OptimizedOctreeNode Build()
    {
        return new OptimizedOctreeNode(_maxDepth, _worldSize, _worldOrigin);
    }
}

// Usage
var octree = new OctreeBuilder()
    .WithMaxDepth(25)
    .WithWorldSize(100000f)
    .Build();
```

## Anti-Patterns

### ❌ Upward Dependencies

**Bad**: Data structure layer depending on domain layer
```csharp
// In BlueMarble.SpatialData
using BlueMarble.World; // ❌ Wrong layer dependency

public class Octree
{
    private WorldCoordinate _position; // ❌ Using higher layer type
}
```

**Good**: Use generic types or interfaces
```csharp
// In BlueMarble.SpatialData
using System.Numerics;

public class Octree
{
    private Vector3 _position; // ✅ Generic type
}
```

### ❌ God Classes

**Bad**: One class doing too much
```csharp
public class WorldManager
{
    public void GenerateTerrain() { }
    public void ProcessPhysics() { }
    public void HandleNetworking() { }
    public void RenderGraphics() { }
    public void SaveData() { }
    // ❌ Too many responsibilities
}
```

**Good**: Single Responsibility Principle
```csharp
public class TerrainGenerator { }
public class PhysicsProcessor { }
public class NetworkManager { }
public class GraphicsRenderer { }
public class DataPersistence { }
// ✅ Each class has one clear purpose
```

### ❌ Circular Dependencies

**Bad**: Two classes depending on each other
```csharp
// OctreeManager.cs
public class OctreeManager
{
    private IndexManager _indexManager; // ❌ Circular dependency
}

// IndexManager.cs
public class IndexManager
{
    private OctreeManager _octreeManager; // ❌ Circular dependency
}
```

**Good**: Extract interface or introduce mediator
```csharp
// IOctreeProvider.cs
public interface IOctreeProvider
{
    IOctreeNode GetOctree();
}

// OctreeManager.cs
public class OctreeManager : IOctreeProvider
{
    public IOctreeNode GetOctree() { return _octree; }
}

// IndexManager.cs
public class IndexManager
{
    private readonly IOctreeProvider _provider; // ✅ Depends on interface
}
```

### ❌ Leaky Abstractions

**Bad**: Implementation details exposed
```csharp
public interface ISpatialIndex
{
    ConcurrentDictionary<Vector3, Node> GetInternalData(); // ❌ Exposes implementation
}
```

**Good**: Clean interface hiding implementation
```csharp
public interface ISpatialIndex
{
    IEnumerable<Node> Query(BoundingBox bounds); // ✅ Abstract operation
}
```

## Related Documents

- [ADR-002: Layered Architecture](./adr-002-layered-architecture-conventions.md)
- [Architecture Review Guide](./ARCHITECTURE_REVIEW_GUIDE.md)
- [Automated Enforcement Guide](./AUTOMATED_ENFORCEMENT_GUIDE.md)
- [CONTRIBUTING.md](../../CONTRIBUTING.md)
