# Code Templates

This directory contains C# code templates that follow the architectural conventions defined in [ADR-002](../../docs/architecture/adr-002-layered-architecture-conventions.md).

## Available Templates

### data-structure-template.cs
Use this template when creating new data structures in the **Data Structures Layer** (`BlueMarble.SpatialData`, `BlueMarble.SpatialIndexing`).

**When to use**:
- Creating octree implementations
- Building spatial indexing structures
- Implementing algorithms for data organization

**Key features**:
- Performance documentation in remarks
- Clear region organization
- Proper exception handling
- XML documentation

### domain-service-template.cs
Use this template when creating domain services in the **Domain Layer** (`BlueMarble.World`, `BlueMarble.World.*`).

**When to use**:
- Implementing world simulation logic
- Creating coordinate transformation services
- Building domain-specific validators or classifiers

**Key features**:
- Dependency injection via constructor
- Business rule validation
- Proper dependency management
- Domain logic organization

### interface-template.cs
Use this template when defining new interfaces.

**When to use**:
- Defining contracts for data structures
- Creating abstractions for domain services
- Establishing layer boundaries

**Key features**:
- Comprehensive XML documentation
- Exception documentation
- Clear contract definition
- Placement guidance (lowest applicable layer)

## How to Use

1. **Choose the appropriate template** based on what you're creating
2. **Copy the template** to your target location
3. **Rename the file** to match your class name (e.g., `WorldSimulator.cs`)
4. **Update the namespace** to match your layer and location
5. **Replace placeholder names** (ExampleDataStructure, etc.)
6. **Fill in the XML documentation**
7. **Implement the logic**
8. **Follow the conventions** from [Coding Guidelines](../../docs/architecture/CODING_GUIDELINES.md)

## Template Selection Guide

```
What are you building?

├─ Generic data structure (octree, index, cache)
│  └─ Use: data-structure-template.cs
│     └─ Namespace: BlueMarble.SpatialData or BlueMarble.SpatialIndexing
│
├─ World-specific logic (simulation, coordinates, domain rules)
│  └─ Use: domain-service-template.cs
│     └─ Namespace: BlueMarble.World or BlueMarble.World.*
│
├─ Contract/abstraction for any layer
│  └─ Use: interface-template.cs
│     └─ Namespace: Lowest layer that needs it
│
└─ Utility/helper for any layer
   └─ Use: data-structure-template.cs (simplified)
      └─ Namespace: BlueMarble.Utils.*
```

## Customization Tips

### For Performance-Critical Code
Add performance characteristics to remarks:
```csharp
/// <remarks>
/// Performance:
/// - Query: O(log n)
/// - Insert: O(log n) amortized
/// - Memory: O(n) where n is number of elements
/// </remarks>
```

### For Thread-Safe Code
Document thread-safety guarantees:
```csharp
/// <remarks>
/// Thread-Safety:
/// - This class is thread-safe for concurrent reads
/// - Writes must be synchronized externally
/// - Uses lock-free data structures internally
/// </remarks>
```

### For Complex Algorithms
Add algorithm description and references:
```csharp
/// <remarks>
/// Algorithm: Modified Red-Black Tree with spatial extensions
/// Reference: Smith et al. (2020) - Spatial Data Structures
/// Time Complexity: O(log n) for all operations
/// </remarks>
```

## Code Review Checklist

Before submitting PR with new code:

- [ ] Used appropriate template for the layer
- [ ] Namespace matches layer and location
- [ ] All public members have XML documentation
- [ ] Dependencies flow downward only
- [ ] No references to higher layers
- [ ] Naming follows conventions
- [ ] Regions used consistently
- [ ] Null checks for parameters
- [ ] Appropriate exception types

## Related Documents

- [ADR-002: Layered Architecture](../../docs/architecture/adr-002-layered-architecture-conventions.md)
- [Coding Guidelines](../../docs/architecture/CODING_GUIDELINES.md)
- [Architecture Review Guide](../../docs/architecture/ARCHITECTURE_REVIEW_GUIDE.md)
- [CONTRIBUTING.md](../../CONTRIBUTING.md)
