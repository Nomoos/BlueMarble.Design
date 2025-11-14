# ADR-002: Composition Over Inheritance

## Status
Accepted

## Context
Object-oriented programming offers two primary mechanisms for code reuse: inheritance (is-a) and composition (has-a). While inheritance creates a hierarchical relationship between classes, it can lead to:

- **Fragile Base Class Problem**: Changes to base class affect all derived classes
- **Deep Hierarchies**: Difficult to understand and navigate
- **Tight Coupling**: Derived classes tightly coupled to base implementation
- **Limited Flexibility**: Cannot change relationships at runtime
- **Diamond Problem**: Multiple inheritance causes ambiguity

Composition, where objects contain instances of other objects, offers more flexibility but requires explicit delegation.

## Decision
We adopt a **composition-over-inheritance** policy for BlueMarble systems:

### Rules
1. **Default to Composition**: Use composition unless inheritance is clearly superior
2. **Maximum Inheritance Depth**: 2 levels (base + derived) maximum
3. **Prefer Interfaces**: Use interfaces for polymorphism over base classes
4. **No Multiple Inheritance**: Even where language allows (C# interfaces excepted)
5. **Inject Dependencies**: Constructor injection for composed objects

### When Inheritance is Acceptable
- Implementing interfaces (C# interface inheritance)
- True "is-a" relationships that are unlikely to change
- Framework requirements (e.g., extending Unity MonoBehaviour)
- Shallow hierarchies (≤2 levels) with stable contracts

### When to Use Composition
- "Has-a" or "uses-a" relationships
- Need to swap implementations at runtime
- Multiple behaviors need to be combined
- Want to avoid tight coupling
- Implementation may change over time

## Examples from SpatialData

### ✅ Good: Composition
```csharp
public class DeltaPatchOctree
{
    private readonly OptimizedOctreeNode _baseTree;  // Composition
    private readonly ConcurrentDictionary<Vector3, MaterialDelta> _deltas;
    
    public DeltaPatchOctree(OptimizedOctreeNode baseTree)
    {
        _baseTree = baseTree ?? throw new ArgumentNullException(nameof(baseTree));
    }
}

public class GeomorphologicalOctreeAdapter
{
    private readonly DeltaPatchOctree _deltaOctree;  // Composition
    
    public GeomorphologicalOctreeAdapter(DeltaPatchOctree deltaOctree)
    {
        _deltaOctree = deltaOctree ?? throw new ArgumentNullException(nameof(deltaOctree));
    }
}
```

### ❌ Bad: Deep Inheritance
```csharp
// ANTI-PATTERN - Do not do this
public class BaseOctreeNode { }
public class OptimizedOctreeNode : BaseOctreeNode { }
public class DeltaOctreeNode : OptimizedOctreeNode { }
public class GeologicalOctreeNode : DeltaOctreeNode { }  // Too deep!
```

### ❌ Bad: Inheritance Instead of Composition
```csharp
// ANTI-PATTERN - Do not do this
public class GeomorphologicalOctreeAdapter : DeltaPatchOctree
{
    // Tight coupling, hard to test, inflexible
}
```

## Consequences

### Positive
✅ **Flexibility**: Can change composed objects at runtime
✅ **Loose Coupling**: Changes to composed classes have limited impact
✅ **Easy Testing**: Can inject mocks and stubs easily
✅ **Clear Ownership**: Explicit lifecycle management
✅ **Avoid Fragility**: No base class changes breaking derived classes
✅ **Better Encapsulation**: Implementation details hidden behind interfaces
✅ **Multiple Behaviors**: Can compose multiple objects easily

### Negative
❌ **More Boilerplate**: Requires explicit delegation methods
❌ **Indirection**: One extra method call to delegated object
❌ **More Classes**: Separate classes instead of inheritance hierarchy
❌ **Initial Complexity**: More objects to understand initially

### Mitigations
- Use modern IDE features for auto-generating delegation
- Document composition relationships clearly
- Use dependency injection frameworks to manage lifecycles
- Prioritize clarity over minimizing classes

## Alternatives Considered

### Alternative 1: Inheritance-First Approach
Use inheritance as default, composition when needed.

**Rejected because**:
- Tends to create deep hierarchies over time
- Refactoring from inheritance to composition is hard
- Fragile base class problems are hard to fix
- Goes against modern best practices

### Alternative 2: Pure Functional Approach
No objects, only functions and data structures.

**Rejected because**:
- C# is object-oriented language
- Would conflict with .NET ecosystem
- Team expertise is in OOP
- Not suitable for all problem domains

### Alternative 3: Mixed Approach Without Rules
Let developers choose on case-by-case basis.

**Rejected because**:
- Leads to inconsistent codebase
- Difficult to review and maintain
- No clear guidelines for new developers
- Technical debt accumulates

## Implementation Guidelines

### Refactoring Inheritance to Composition
When you find deep inheritance:

```csharp
// Before: Inheritance
public class AdvancedOctree : BasicOctree
{
    public override void Process() 
    {
        base.Process();
        // Additional logic
    }
}

// After: Composition
public class AdvancedOctree
{
    private readonly IBasicOctree _basicOctree;
    
    public AdvancedOctree(IBasicOctree basicOctree)
    {
        _basicOctree = basicOctree;
    }
    
    public void Process()
    {
        _basicOctree.Process();
        // Additional logic
    }
}
```

### Adding New Functionality
When adding new functionality, prefer composition:

```csharp
// Instead of extending GeomorphologicalOctreeAdapter
// Create a new class that composes it
public class AdvancedGeologicalProcessor
{
    private readonly GeomorphologicalOctreeAdapter _adapter;
    private readonly IWeatherSimulator _weather;
    
    public AdvancedGeologicalProcessor(
        GeomorphologicalOctreeAdapter adapter,
        IWeatherSimulator weather)
    {
        _adapter = adapter;
        _weather = weather;
    }
}
```

### Code Review Checklist
- [ ] Is inheritance truly necessary?
- [ ] Is this a stable "is-a" relationship?
- [ ] Could composition provide more flexibility?
- [ ] Is inheritance depth ≤ 2 levels?
- [ ] Are all base class methods appropriate for derived classes?
- [ ] Could interface + composition work instead?

## Exceptions
These scenarios may warrant inheritance:
1. Framework requirements (Unity MonoBehaviour, ASP.NET controllers)
2. Implementing interfaces (C# allows multiple interface inheritance)
3. Extremely stable domain models with clear "is-a" relationships
4. Performance-critical scenarios where virtual dispatch is unacceptable

All exceptions must be documented and justified in code reviews.

## Related Decisions
- ADR-001: Layered Architecture for Spatial Data Systems
- ADR-003: Strategy Pattern for Consolidation Algorithms
- ADR-004: Dependency Injection Policy

## References
- [Composition over Inheritance](https://en.wikipedia.org/wiki/Composition_over_inheritance)
- [Effective Java, 3rd Edition](https://www.oreilly.com/library/view/effective-java-3rd/9780134686097/) - Item 18
- [Design Patterns: Elements of Reusable Object-Oriented Software](https://en.wikipedia.org/wiki/Design_Patterns)
- BlueMarble.SpatialData implementation (reference example)

## Notes
This policy has been proven in SpatialData module with:
- Zero inheritance hierarchies (except interface implementation)
- High flexibility and testability
- Easy refactoring and extension
- Clear object relationships
- 100% test coverage
