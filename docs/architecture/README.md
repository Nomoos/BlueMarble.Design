# Architecture Documentation

This directory contains architectural decisions, conventions, and guidelines for the BlueMarble project.

## Quick Start

### For New Developers
1. Start with [ADR-002: Layered Architecture](./adr-002-layered-architecture-conventions.md) - understand the layers
2. Read [Coding Guidelines](./CODING_GUIDELINES.md) - learn naming and organization conventions
3. Review [Architecture Review Guide](./ARCHITECTURE_REVIEW_GUIDE.md) - understand what to expect in code reviews

### For Code Contributors
Before writing code:
- [ ] Read ADR-002 to understand layer structure
- [ ] Review Coding Guidelines for naming conventions
- [ ] Check code templates in `/templates/code/`

Before submitting PR:
- [ ] Self-review using Architecture Review Guide
- [ ] Run architecture tests locally (if available)
- [ ] Verify all dependencies flow downward

### For Code Reviewers
- Use [Architecture Review Guide](./ARCHITECTURE_REVIEW_GUIDE.md) as a checklist
- Reference ADR-002 when requesting changes
- Check for common violations listed in the review guide

## Documents

### Architecture Decision Records (ADRs)

- **[ADR-002: Layered Architecture and Conventions](./adr-002-layered-architecture-conventions.md)** ⭐ **Start Here**
  - Defines the layered architecture (Utility → Data Structures → Domain → Application)
  - Documents layer responsibilities and dependency rules
  - Provides rationale for architectural decisions
  - Status: Proposed
  - Date: 2025-11-14

### Guidelines and Standards

- **[Coding Guidelines](./CODING_GUIDELINES.md)**
  - Namespace conventions for each layer
  - Naming conventions (interfaces, classes, methods)
  - File organization patterns
  - Code templates and examples
  - Common patterns and anti-patterns

- **[Architecture Review Guide](./ARCHITECTURE_REVIEW_GUIDE.md)**
  - Code review checklist for architecture compliance
  - Common violations with corrections
  - Review comment templates
  - Step-by-step review process

- **[Automated Enforcement Guide](./AUTOMATED_ENFORCEMENT_GUIDE.md)**
  - .NET Analyzer configuration
  - Architecture test examples using NetArchTest.Rules
  - CI/CD integration instructions
  - Troubleshooting guide

## Architecture Overview

### Layer Structure

```
┌──────────────────────────────────────────────┐
│        Application / Integration Layer        │
│           BlueMarble.Examples.*              │
│             BlueMarble.App.*                 │
└──────────────────┬───────────────────────────┘
                   │ depends on
                   ▼
┌──────────────────────────────────────────────┐
│        Domain / Business Logic Layer          │
│             BlueMarble.World.*               │
└──────────────────┬───────────────────────────┘
                   │ depends on
                   ▼
┌──────────────────────────────────────────────┐
│      Data Structures / Algorithm Layer        │
│         BlueMarble.SpatialData.*             │
│       BlueMarble.SpatialIndexing.*           │
└──────────────────┬───────────────────────────┘
                   │ depends on
                   ▼
┌──────────────────────────────────────────────┐
│        Utility / Foundation Layer             │
│            BlueMarble.Utils.*                │
└──────────────────────────────────────────────┘
```

### Dependency Rules

✅ **Allowed** (downward):
- Application → Domain → Data Structures → Utilities

❌ **Forbidden** (upward or circular):
- Data Structures → Domain
- Utilities → any higher layer
- Any circular dependencies

## Code Templates

Ready-to-use templates in `/templates/code/`:
- `data-structure-template.cs` - For spatial data structures
- `domain-service-template.cs` - For domain/business logic
- `interface-template.cs` - For defining contracts

See [Code Templates README](../../templates/code/README.md) for usage guide.

## Common Patterns

### ✅ Good Patterns

**Downward Dependency**:
```csharp
// BlueMarble.World (Domain) using BlueMarble.SpatialData (Data Structures)
namespace BlueMarble.World;
using BlueMarble.SpatialData;  // ✅ Lower layer

public class WorldSimulator
{
    private readonly IOctreeNode _octree;  // ✅
}
```

**Interface in Lowest Layer**:
```csharp
// Interface in Data Structures layer
namespace BlueMarble.SpatialData;
public interface IOctreeNode { }

// Used by Domain layer
namespace BlueMarble.World;
public class WorldSimulator
{
    private readonly IOctreeNode _octree;  // ✅
}
```

### ❌ Anti-Patterns

**Upward Dependency**:
```csharp
// BlueMarble.SpatialData (Data Structures) using BlueMarble.World (Domain)
namespace BlueMarble.SpatialData;
using BlueMarble.World;  // ❌ Higher layer

public class Octree
{
    private WorldCoordinate _position;  // ❌
}
```

**God Class**:
```csharp
// ❌ One class doing too much
public class WorldManager
{
    public void GenerateTerrain() { }
    public void SimulatePhysics() { }
    public void ProcessNetworking() { }
    // ... 50 more methods
}
```

## Enforcement

### Automated Checks
1. **.NET Analyzers** - Built-in code analysis
2. **Architecture Tests** - NetArchTest.Rules for dependency validation
3. **CI/CD Pipeline** - Automatic validation on every PR

### Manual Checks
1. **Code Review** - Architecture checklist in PR template
2. **Design Review** - Major changes reviewed by architecture lead
3. **Team Communication** - Monthly architecture review sessions

## FAQ

**Q: Which layer should my code go in?**
A: See the decision tree in [Coding Guidelines](./CODING_GUIDELINES.md#namespace-conventions) and [Code Templates README](../../templates/code/README.md#template-selection-guide).

**Q: Can I create a dependency between two projects in the same layer?**
A: Minimize this. If needed, use interfaces and consider if they should be in the same project.

**Q: What if I need to use a Domain type in the Data Structures layer?**
A: Use a generic type (like `Vector3`) or create an interface in the Data Structures layer.

**Q: How do I handle circular dependencies?**
A: Extract a common interface to a lower layer, or restructure to make dependencies flow in one direction.

**Q: Can I bypass these rules for a prototype?**
A: Document the violation and create a TODO to fix it. Prototypes in `/research` have more flexibility.

## Related Documents

- [CONTRIBUTING.md](../../CONTRIBUTING.md) - General contribution guidelines
- [PULL_REQUEST_TEMPLATE.md](../../.github/PULL_REQUEST_TEMPLATE.md) - PR checklist including architecture
- [Code Templates](../../templates/code/) - Ready-to-use code templates
- [Decision Record Template](../../templates/decision-record.md) - Template for new ADRs

## Evolution

This architecture documentation is a living system. To propose changes:

1. Open a GitHub issue with tag `architectural-convention`
2. Discuss with team (minimum 3 reviewers)
3. Update ADR-002 and related docs if approved
4. Communicate changes to entire team
5. Update code templates and examples

**Next Review**: 2026-02-14 (3 months after implementation)

## Support

- **Questions**: Open a GitHub issue with tag `architecture-question`
- **Violations**: Reference ADR-002 and related docs in code review
- **Training**: Monthly architecture review sessions
- **Pair Programming**: Available for complex refactorings
