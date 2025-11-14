---
title: ADR-002 Layered Architecture and Conventions
date: 2025-11-14
owner: @copilot
status: proposed
tags: [decision, adr, architecture, conventions, layers, code-review]
---

# ADR-002: Layered Architecture and Conventions

## Context

BlueMarble.Design is a research and design repository that includes C# implementations for spatial data structures, world simulation, and indexing systems. As the codebase grows, maintaining clear architectural boundaries and consistent coding practices becomes critical to prevent technical debt, coupling issues, and implementation drift.

### Background

- The project includes multiple domains: SpatialData, SpatialIndexing, World, and Utils
- Current namespaces: `BlueMarble.SpatialData`, `BlueMarble.SpatialIndexing`, `BlueMarble.World`, `BlueMarble.Utils.Spatial.DistributedStorage`
- Development involves both core team members and contributors
- Code reviews are conducted but lack specific architectural guidance
- No formal architectural conventions or enforcement mechanisms exist

### Problem Statement

**How can we establish, document, and enforce architectural conventions to ensure consistent implementation of the layered architecture across all BlueMarble projects?**

### Constraints

- Must not disrupt existing working code
- Must be practical and enforceable
- Must integrate with existing development workflow
- Must be maintainable as the architecture evolves
- Must support both experienced and new team members

### Requirements

1. **Clear Documentation**: Architecture must be documented and easily accessible
2. **Coding Conventions**: Naming and structure patterns must reflect the architecture
3. **Review Process**: Code reviews must include architectural validation
4. **Automation**: Static analysis must catch common violations
5. **Evolution**: Conventions must be updateable based on feedback

## Layered Architecture Definition

### Layer Structure

```
┌──────────────────────────────────────────────────────────┐
│          Application / Integration Layer                  │
│     (Examples, UI, Game Logic, External Integrations)    │
│              Namespace: BlueMarble.Examples               │
└────────────────────┬─────────────────────────────────────┘
                     │ depends on
                     ▼
┌──────────────────────────────────────────────────────────┐
│              Domain / Business Logic Layer                │
│    (World Simulation, Coordinate Systems, Constants)     │
│               Namespace: BlueMarble.World                 │
│            Namespace: BlueMarble.World.Constants          │
└────────────────────┬─────────────────────────────────────┘
                     │ depends on
                     ▼
┌──────────────────────────────────────────────────────────┐
│            Data Structures / Algorithm Layer              │
│   (Spatial Indexing, Octrees, Delta Overlay Systems)     │
│          Namespace: BlueMarble.SpatialIndexing            │
│            Namespace: BlueMarble.SpatialData              │
└────────────────────┬─────────────────────────────────────┘
                     │ depends on
                     ▼
┌──────────────────────────────────────────────────────────┐
│                  Utility / Foundation Layer               │
│       (Distributed Storage, Core Data Structures)        │
│    Namespace: BlueMarble.Utils.Spatial.DistributedStorage │
└──────────────────────────────────────────────────────────┘
```

### Layer Responsibilities

#### 1. Utility / Foundation Layer
- **Purpose**: Reusable, domain-agnostic utilities and core data structures
- **Examples**: Distributed storage systems, common algorithms, extension methods
- **Namespace Pattern**: `BlueMarble.Utils.*`
- **Dependencies**: May depend on standard libraries only
- **Restrictions**: Must NOT depend on any higher layers

#### 2. Data Structures / Algorithm Layer
- **Purpose**: Generic spatial data structures and algorithms
- **Examples**: Octrees, spatial indexing, Morton encoding, delta overlays
- **Namespace Pattern**: `BlueMarble.SpatialData`, `BlueMarble.SpatialIndexing`
- **Dependencies**: May depend on Utility layer
- **Restrictions**: Must NOT depend on domain logic or application layers

#### 3. Domain / Business Logic Layer
- **Purpose**: World-specific logic, simulation systems, domain models
- **Examples**: World coordinates, accessibility zones, altitude conversion
- **Namespace Pattern**: `BlueMarble.World`, `BlueMarble.World.*`
- **Dependencies**: May depend on Data Structures and Utility layers
- **Restrictions**: Must NOT depend on Application layer

#### 4. Application / Integration Layer
- **Purpose**: End-user applications, examples, external integrations
- **Examples**: Console applications, Unity/Godot integrations, example programs
- **Namespace Pattern**: `BlueMarble.Examples`, `BlueMarble.App.*`
- **Dependencies**: May depend on all lower layers
- **Restrictions**: Lower layers must NOT depend on this layer

### Dependency Rules

**Golden Rule**: Layers may only depend on layers below them, never above.

```
✅ Allowed Dependencies (downward):
   Application → Domain → Data Structures → Utilities

❌ Forbidden Dependencies (upward):
   Utilities → Data Structures
   Data Structures → Domain
   Domain → Application

❌ Forbidden Dependencies (sideways between same level):
   SpatialData ↔ SpatialIndexing (minimize, use interfaces)
```

## Coding Conventions

See [CODING_GUIDELINES.md](./CODING_GUIDELINES.md) for detailed coding standards and examples.

### Key Principles

1. **Namespace must reflect layer**: Use consistent namespace patterns
2. **One project per namespace group**: Clear project boundaries
3. **Dependency flow downward only**: No upward or circular dependencies
4. **Common code in lower layers**: Reusable utilities in Utils layer
5. **Clear naming**: Names indicate purpose and layer

## Code Review Guidelines

### Architecture Review Checklist

During code reviews, reviewers must verify:

1. **Layer Boundaries**
   - [ ] All dependencies flow downward (never upward)
   - [ ] No circular dependencies between projects
   - [ ] No sideways dependencies between same-level projects

2. **Namespace Organization**
   - [ ] Namespace matches project structure
   - [ ] Namespace clearly indicates layer
   - [ ] No mixing of concerns in a single namespace

3. **Abstraction and Coupling**
   - [ ] Interfaces are defined at the lowest applicable layer
   - [ ] Implementation details don't leak across layers
   - [ ] Dependencies are through interfaces where appropriate

4. **Code Duplication**
   - [ ] No duplicated logic that exists in lower layers
   - [ ] Common functionality extracted to appropriate layer
   - [ ] Utility functions placed in Utils layer

5. **Naming Conventions**
   - [ ] Files, classes, and namespaces follow conventions
   - [ ] Names clearly indicate purpose and layer
   - [ ] Consistent terminology across the layer

See [ARCHITECTURE_REVIEW_GUIDE.md](./ARCHITECTURE_REVIEW_GUIDE.md) for detailed guidance on conducting architecture reviews.

## Automated Enforcement

### Static Analysis Tools

1. **NET Analyzers**: Built-in code analysis (see [AUTOMATED_ENFORCEMENT_GUIDE.md](./AUTOMATED_ENFORCEMENT_GUIDE.md))
2. **Architecture Tests**: NetArchTest.Rules for dependency validation
3. **CI/CD Integration**: Automated checks on every PR

Configuration details in [AUTOMATED_ENFORCEMENT_GUIDE.md](./AUTOMATED_ENFORCEMENT_GUIDE.md).

## Team Communication

### Onboarding Process

1. **New Developer Checklist**
   - [ ] Read this ADR (Architecture Conventions)
   - [ ] Review [CODING_GUIDELINES.md](./CODING_GUIDELINES.md)
   - [ ] Study existing code in each layer
   - [ ] Complete architecture quiz (questions about layer boundaries)
   - [ ] Pair with senior developer on first PR
   - [ ] Review common violations section in [ARCHITECTURE_REVIEW_GUIDE.md](./ARCHITECTURE_REVIEW_GUIDE.md)

2. **Architecture Training**
   - Monthly architecture review sessions
   - Share real examples of good/bad patterns
   - Discuss recent violations and learnings
   - Update documentation based on feedback

### Communication Channels

1. **Documentation Updates**
   - Architecture changes announced in team meetings
   - Updates to this ADR tracked via Git history
   - Breaking changes require team consensus

2. **Code Review Comments**
   - Reference this ADR when requesting changes
   - Format: "Per ADR-002, this violates [rule]. See [link]"
   - Include link to relevant section

3. **Success Stories**
   - Celebrate refactoring wins in team channels
   - Document cases where architecture prevented bugs
   - Share performance improvements from proper layering

## Evolution Process

### Updating Conventions

1. **Feedback Collection**
   - Developers can suggest changes via GitHub issues
   - Tag with `architectural-convention` label
   - Monthly review of accumulated feedback

2. **Change Process**
   - Propose change to this ADR
   - Discuss with team (minimum 3 reviewers)
   - Update implementation if approved
   - Communicate changes to entire team
   - Update code templates and examples

3. **Backwards Compatibility**
   - Prefer deprecation over breaking changes
   - Provide migration guide for major changes
   - Allow grace period for adoption

### Review Criteria

This ADR should be reviewed when:
- Team grows beyond 10 developers
- New major components are added
- Consistent violations indicate confusion
- Performance or maintainability suffers
- Technology stack changes significantly

## Decision

**Chosen Approach**: Implement comprehensive architectural conventions with four pillars:

1. **Documentation**: This ADR plus detailed guidelines
2. **Code Reviews**: Enhanced PR template with architecture checklist
3. **Automation**: Static analysis and architecture tests
4. **Culture**: Regular communication and training

### Rationale

1. **Multi-layered Defense**: Catches violations at multiple stages
2. **Practical**: Integrates with existing workflows
3. **Scalable**: Works for small and large teams
4. **Educational**: Helps developers learn good patterns
5. **Maintainable**: Living documentation that evolves

**Decision Makers**:
- @copilot (Initial proposal)
- Awaiting approval from BlueMarble team leads

**Date Decided**: 2025-11-14 (Proposed)

## Consequences

### Positive Consequences

1. **Reduced Technical Debt**: Early violation detection prevents accumulation
2. **Faster Onboarding**: Clear documentation helps new developers
3. **Better Design**: Architectural thinking becomes habitual
4. **Easier Refactoring**: Clear boundaries make changes safer
5. **Consistent Codebase**: All code follows same patterns

### Negative Consequences

1. **Initial Setup Cost**: Requires time to implement tooling and tests
   - *Mitigation*: Implement incrementally, starting with documentation
2. **Learning Curve**: Developers must learn architectural rules
   - *Mitigation*: Provide examples, templates, and training
3. **Review Overhead**: More items to check in code reviews
   - *Mitigation*: Automate what's possible, focus humans on complex cases
4. **Potential Over-Engineering**: Risk of excessive layering
   - *Mitigation*: Start simple, add layers only when needed

### Impact Areas

- **Code Reviews**: +15-20 minutes per PR initially (decreases over time)
- **CI/CD**: +2-3 minutes for architecture tests
- **Development**: +5-10 minutes upfront design per feature
- **Maintenance**: -30-50% time on debugging architectural issues
- **Onboarding**: +2 hours initial training, -50% ramp-up time overall

## Implementation

### Phase 1: Documentation (Days 1-3)
- [x] Create this ADR (Architecture Conventions)
- [ ] Create [CODING_GUIDELINES.md](./CODING_GUIDELINES.md)
- [ ] Create [ARCHITECTURE_REVIEW_GUIDE.md](./ARCHITECTURE_REVIEW_GUIDE.md)
- [ ] Create [AUTOMATED_ENFORCEMENT_GUIDE.md](./AUTOMATED_ENFORCEMENT_GUIDE.md)
- [ ] Add architecture section to CONTRIBUTING.md

### Phase 2: Code Review Enhancement (Days 4-5)
- [ ] Update PR template with architecture checklist
- [ ] Add layer-specific code templates

### Phase 3: Automated Enforcement (Days 6-10)
- [ ] Add .NET analyzers to all projects
- [ ] Create architecture test project
- [ ] Add architecture validation to CI/CD

### Phase 4: Team Adoption (Days 11-15)
- [ ] Conduct architecture training session
- [ ] Review and refactor any existing violations
- [ ] Create onboarding checklist for new developers
- [ ] Establish feedback collection process

## Review Date

**Next Review**: 2026-02-14 (3 months after implementation)

**Review Criteria**:
- Number of architectural violations caught
- Time to fix violations
- Team feedback on usability
- Impact on PR review time
- Impact on code quality metrics

## Related Documents

- [CONTRIBUTING.md](../../CONTRIBUTING.md) - Contribution guidelines
- [PULL_REQUEST_TEMPLATE.md](../../.github/PULL_REQUEST_TEMPLATE.md) - PR template
- [ADR-001: Coordinate Data Type Selection](../../research/topics/adr-001-coordinate-data-type-selection.md) - Example ADR
- [ARCHITECTURE.md](../../ARCHITECTURE.md) - Delta Overlay System Architecture
- [Decision Record Template](../../templates/decision-record.md) - ADR template
- [CODING_GUIDELINES.md](./CODING_GUIDELINES.md) - Detailed coding standards
- [ARCHITECTURE_REVIEW_GUIDE.md](./ARCHITECTURE_REVIEW_GUIDE.md) - Code review guidelines
- [AUTOMATED_ENFORCEMENT_GUIDE.md](./AUTOMATED_ENFORCEMENT_GUIDE.md) - Static analysis setup
