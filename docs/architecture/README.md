# Architecture Documentation

This directory contains architectural guidelines, patterns, and decision records for the BlueMarble project.

## Overview

BlueMarble follows a **layered modular architecture** that emphasizes:
- Clear separation of concerns
- Composition over inheritance
- Appropriate use of design patterns
- Testability and maintainability
- Team collaboration and understanding

## Key Documents

### Guides
- **[Layered Modular Architecture Guide](./LAYERED_MODULAR_ARCHITECTURE.md)** - Comprehensive guide to the layered architecture pattern used in BlueMarble, including design patterns, best practices, and examples

### Architectural Decision Records (ADRs)
- **[ADR-001: Layered Architecture](./adr/ADR-001-layered-architecture.md)** - Decision to adopt layered architecture for spatial data systems
- **[ADR-002: Composition Over Inheritance](./adr/ADR-002-composition-over-inheritance.md)** - Policy favoring composition over inheritance

## Architecture Principles

### 1. Layered Structure
```
Layer 4: Domain Adapters    (Business logic, use cases)
         ↓
Layer 3: Performance        (Optimization, strategies)
         ↓
Layer 2: Core Structures    (Data structures, algorithms)
         ↓
Layer 1: Data Models        (Value types, entities)
```

### 2. Design Patterns
- **Adapter Pattern**: Domain-specific interfaces to generic systems
- **Strategy Pattern**: Runtime algorithm selection
- **Template Method**: Common structure with varying implementations
- **Dependency Injection**: Constructor-based injection throughout

### 3. Composition Over Inheritance
- Maximum inheritance depth: 2 levels
- Prefer "has-a" over "is-a" relationships
- Use interfaces for polymorphism
- Inject dependencies via constructors

### 4. SOLID Principles
- **Single Responsibility**: Each class has one clear purpose
- **Open/Closed**: Open for extension, closed for modification
- **Liskov Substitution**: Subtypes must be substitutable for base types
- **Interface Segregation**: Many specific interfaces over one general
- **Dependency Inversion**: Depend on abstractions, not concretions

## Reference Implementation

The **BlueMarble.SpatialData** module serves as the reference implementation of these principles:

```
BlueMarble.SpatialData/
├── MaterialData.cs                    (Layer 1: Data Model)
├── OptimizedOctreeNode.cs            (Layer 2: Core Structure)
├── DeltaPatchOctree.cs               (Layer 3: Performance)
└── GeomorphologicalOctreeAdapter.cs  (Layer 4: Domain Adapter)
```

**Metrics**:
- 4 classes, 4 layers
- 0 inheritance hierarchies (composition only)
- 18 comprehensive tests
- 100% test pass rate
- Zero circular dependencies

## Using This Documentation

### For Architects
1. Review layered architecture guide for overall pattern
2. Consult ADRs when making architectural decisions
3. Update ADRs when new patterns or decisions are made
4. Use reference implementation as template for new systems

### For Developers
1. Start with layered architecture guide to understand structure
2. Review relevant ADRs for specific patterns
3. Study reference implementation in SpatialData module
4. Follow code review checklist when submitting changes

### For Reviewers
1. Verify layer boundaries are respected
2. Check that composition is preferred over inheritance
3. Validate appropriate use of design patterns
4. Ensure dependencies flow in correct direction
5. Confirm testability and maintainability

## Adding New Architecture Documentation

### When to Create ADR
Create an ADR when making decisions about:
- Overall system structure and organization
- Design patterns to adopt or avoid
- Technology choices affecting architecture
- Policies that impact multiple teams or systems
- Trade-offs between different approaches

### ADR Template
```markdown
# ADR-XXX: [Title]

## Status
[Proposed | Accepted | Deprecated | Superseded]

## Context
[What problem are we trying to solve?]

## Decision
[What decision have we made and why?]

## Consequences
[What are the positive and negative outcomes?]

## Alternatives Considered
[What other options did we evaluate?]

## References
[Related documents and resources]
```

### Updating Architecture Guides
When updating guides:
1. Keep examples practical and based on real code
2. Include both good and bad examples
3. Explain the "why" behind recommendations
4. Link to relevant ADRs and references
5. Update metrics and statistics

## Architecture Validation

### Code Review Checklist
- [ ] Does class have single, clear responsibility?
- [ ] Are dependencies injected via constructor?
- [ ] Is composition used instead of inheritance?
- [ ] Are design patterns applied appropriately?
- [ ] Can class be tested in isolation?
- [ ] Are layer boundaries respected?
- [ ] Is documentation complete and accurate?

### Automated Checks
Consider implementing:
- Architecture tests using ArchUnitNET
- Dependency analysis tools
- Cyclomatic complexity metrics
- Code coverage requirements
- Static analysis for patterns

## Related Documentation

### Technical Documentation
- [System Architecture Design](../systems/system-architecture-design.md)
- [Database Schema Design](../systems/database-schema-design.md)
- [API Specifications](../systems/api-specifications.md)

### Development Guidelines
- [Contributing Guidelines](../../CONTRIBUTING.md)
- [Documentation Best Practices](../../DOCUMENTATION_BEST_PRACTICES.md)
- [Code Review Guidelines](../../.github/PULL_REQUEST_TEMPLATE.md)

### Project Context
- [Architecture and Simulation Plan](../../ArchitectureAndSimulationPlan.md)
- [Technical Foundation](../TECHNICAL_FOUNDATION.md)
- [Project Roadmap](../../roadmap/project-roadmap.md)

## Questions and Feedback

For questions about architecture:
1. Review this documentation and ADRs
2. Check reference implementation in SpatialData
3. Consult with architecture team
4. Open discussion in appropriate channel

For suggesting improvements:
1. Propose ADR for significant changes
2. Submit PR for documentation updates
3. Discuss in architecture review meetings
4. Share learnings and patterns discovered

## Version History

- **v1.0** (2025-11-14): Initial architecture documentation
  - Layered modular architecture guide
  - ADR-001: Layered architecture
  - ADR-002: Composition over inheritance
  - Reference implementation in SpatialData module
