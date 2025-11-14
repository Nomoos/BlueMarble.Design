# Architecture Onboarding Guide

Welcome to BlueMarble! This guide will help you understand our layered architecture and start contributing code that follows our conventions.

## Quick Start (15 minutes)

### 1. Read the Core Documents (10 min)

Start with these three documents in order:

1. **[ADR-002: Layered Architecture](./adr-002-layered-architecture-conventions.md)** (5 min read)
   - Understand the 4 layers: Utility → Data Structures → Domain → Application
   - Learn the golden rule: Dependencies flow downward only
   - See the layer responsibilities

2. **[Architecture README](./README.md)** (3 min read)
   - Quick overview and navigation
   - Common patterns at a glance
   - FAQ for quick answers

3. **[Coding Guidelines](./CODING_GUIDELINES.md)** (2 min skim)
   - Namespace conventions
   - Naming patterns
   - Bookmark for reference

### 2. Review Code Examples (5 min)

Look at these real examples in the codebase:

**Data Structures Layer** (`BlueMarble.SpatialData`):
- `src/BlueMarble.SpatialData/OptimizedOctreeNode.cs` - Clean data structure
- `src/BlueMarble.SpatialData/DeltaPatchOctree.cs` - Well-organized implementation
- `src/BlueMarble.SpatialData/MaterialData.cs` - Simple data class

**Domain Layer** (`BlueMarble.World`):
- `src/BlueMarble.World/CoordinateValidator.cs` - Domain validation logic
- `src/BlueMarble.World/AccessibilityZoneClassifier.cs` - Domain service

## Architecture Quiz (Test Your Understanding)

Answer these questions to verify your understanding:

1. **Can the `BlueMarble.SpatialData` project reference `BlueMarble.World`?**
   - [ ] Yes
   - [x] No - Data Structures cannot depend on Domain layer (upward dependency)

2. **Can the `BlueMarble.World` project reference `BlueMarble.SpatialData`?**
   - [x] Yes - Domain can depend on Data Structures (downward dependency)
   - [ ] No

3. **Where should a generic math utility function go?**
   - [ ] BlueMarble.World
   - [ ] BlueMarble.SpatialData
   - [x] BlueMarble.Utils
   - [ ] Doesn't matter

4. **Where should world simulation logic go?**
   - [x] BlueMarble.World (Domain layer)
   - [ ] BlueMarble.Examples (Application layer)
   - [ ] BlueMarble.SpatialData (Data Structures layer)
   - [ ] BlueMarble.Utils (Utility layer)

5. **If two projects in the same layer need to share code, what should you do?**
   - [ ] Create a reference between them
   - [x] Extract a shared interface/abstraction to a lower layer or common project
   - [ ] Duplicate the code
   - [ ] Refactor into one project

**Score**: 5/5 correct? You're ready! <5? Review ADR-002 again.

## Your First Contribution

### Step 1: Choose Your Task

Pick a task appropriate for your experience:

**Beginner** (Good first issue):
- Add a new property to an existing data class
- Fix documentation or comments
- Add unit tests for existing functionality

**Intermediate**:
- Implement a new data structure in `BlueMarble.SpatialData`
- Add a new domain service in `BlueMarble.World`
- Refactor duplicate code into utilities

**Advanced**:
- Design a new layer component
- Implement complex algorithms
- Refactor cross-layer dependencies

### Step 2: Set Up Your Environment

1. **Clone the repository**
   ```bash
   git clone https://github.com/Nomoos/BlueMarble.Design.git
   cd BlueMarble.Design
   ```

2. **Build the project**
   ```bash
   dotnet restore
   dotnet build --configuration Release
   ```

3. **Run tests**
   ```bash
   dotnet test BlueMarble.World.sln
   dotnet test BlueMarble.SpatialData.sln
   ```

4. **Verify analyzers are working**
   ```bash
   dotnet build --configuration Release /p:RunAnalyzers=true
   ```

### Step 3: Make Your Changes

1. **Choose the right template** from `/templates/code/`:
   - `data-structure-template.cs` for spatial data structures
   - `domain-service-template.cs` for world logic
   - `interface-template.cs` for abstractions

2. **Follow the naming conventions**:
   ```csharp
   // ✅ Good names
   public interface IOctreeNode { }
   public class OptimizedOctreeNode : IOctreeNode { }
   public class WorldSimulator { }
   
   // ❌ Bad names
   public interface OctreeNode { }  // Missing 'I' prefix
   public class Manager { }  // Too generic
   public class octreenode { }  // Wrong casing
   ```

3. **Ensure dependencies flow downward**:
   ```csharp
   // ✅ Allowed
   using BlueMarble.SpatialData;  // Domain → Data Structures
   using BlueMarble.Utils;  // Domain → Utilities
   
   // ❌ Forbidden
   using BlueMarble.World;  // Data Structures → Domain
   using BlueMarble.Examples;  // Domain → Application
   ```

4. **Add XML documentation**:
   ```csharp
   /// <summary>
   /// Brief description of the class
   /// </summary>
   /// <remarks>
   /// Detailed notes, performance characteristics, usage examples
   /// </remarks>
   public class MyClass { }
   ```

### Step 4: Self-Review

Before creating a PR, use this checklist:

- [ ] Code is in the correct layer (namespace matches layer)
- [ ] All dependencies flow downward
- [ ] No circular dependencies
- [ ] Naming follows conventions
- [ ] XML documentation added
- [ ] No code duplication
- [ ] Tests added (if applicable)
- [ ] Build succeeds: `dotnet build --configuration Release`
- [ ] Tests pass: `dotnet test`
- [ ] Analyzers happy: `dotnet build /p:RunAnalyzers=true`

### Step 5: Create Pull Request

1. **Use the PR template** (it's auto-filled)
2. **Check the architecture checklist** in the PR template
3. **Reference ADR-002** if you made architectural decisions
4. **Be ready to discuss** in code review

## Code Review Process

### What Reviewers Will Check

From the [Architecture Review Guide](./ARCHITECTURE_REVIEW_GUIDE.md):

1. **Layer boundaries** - Dependencies flow downward only
2. **Namespace organization** - Matches layer structure
3. **Code duplication** - Common code extracted
4. **Naming conventions** - Follows guidelines
5. **Interface placement** - In lowest applicable layer

### Common Feedback Examples

**Example 1: Upward Dependency**
```
❌ Reviewer: "Per ADR-002, BlueMarble.SpatialData cannot depend on BlueMarble.World. 
Please use Vector3 instead of WorldCoordinate."

✅ You: "Good catch! Changed to Vector3."
```

**Example 2: Code in Wrong Layer**
```
❌ Reviewer: "This simulation logic should be in BlueMarble.World, not BlueMarble.Examples. 
Please create a WorldSimulator service."

✅ You: "Makes sense. Extracted to WorldSimulator.cs in the Domain layer."
```

**Example 3: Missing Documentation**
```
❌ Reviewer: "Please add XML documentation to public methods per Coding Guidelines."

✅ You: "Added XML docs with parameter descriptions and return value notes."
```

## Common Pitfalls (And How to Avoid Them)

### Pitfall 1: Upward Dependencies

**Problem**: Using a higher layer type in a lower layer

**Example**:
```csharp
// In BlueMarble.SpatialData
using BlueMarble.World;  // ❌

public class Octree
{
    private WorldCoordinate _position;  // ❌
}
```

**Solution**: Use generic types or create interface in lower layer
```csharp
// In BlueMarble.SpatialData
using System.Numerics;  // ✅

public class Octree
{
    private Vector3 _position;  // ✅
}
```

### Pitfall 2: Logic in Wrong Layer

**Problem**: Domain logic in Application layer

**Example**:
```csharp
// In BlueMarble.Examples
public class GameExample
{
    public void UpdateWorld()
    {
        // ❌ Complex simulation in application
        var erosion = CalculateErosion(...);
    }
}
```

**Solution**: Move to Domain layer
```csharp
// In BlueMarble.World
public class GeologicalSimulator
{
    public ErosionResult SimulateErosion(...)  // ✅
    {
        return new ErosionResult(...);
    }
}
```

### Pitfall 3: God Classes

**Problem**: One class doing too much

**Example**:
```csharp
public class WorldManager  // ❌
{
    public void GenerateTerrain() { }
    public void SimulatePhysics() { }
    public void ProcessNetworking() { }
    // ... 50 more methods
}
```

**Solution**: Split into focused classes
```csharp
public class TerrainGenerator { }  // ✅
public class PhysicsSimulator { }  // ✅
public class NetworkManager { }  // ✅
```

## Getting Help

### Resources

1. **Documentation**: Start with [Architecture README](./README.md)
2. **Examples**: Look at existing code in the codebase
3. **Templates**: Use code templates in `/templates/code/`
4. **FAQ**: Check [Architecture README FAQ section](./README.md#faq)

### Asking Questions

**GitHub Issues**:
- Tag with `architecture-question` label
- Reference specific code or situation
- Include what you've already tried

**Pull Request Comments**:
- Ask reviewers for clarification
- Reference docs you've read
- Propose solutions for feedback

**Team Channels**:
- Monthly architecture review sessions
- Pair programming available for complex tasks

## Next Steps

After your first contribution:

1. **Attend architecture review session** (monthly)
2. **Volunteer to review code** (learn by reviewing)
3. **Share learnings** (document patterns you discover)
4. **Suggest improvements** (this is living documentation)

## Checklist for Your First PR

- [ ] I've read ADR-002 (Layered Architecture)
- [ ] I've skimmed Coding Guidelines
- [ ] I've reviewed code examples
- [ ] I've taken the architecture quiz (5/5 correct)
- [ ] I've set up my development environment
- [ ] I've used the appropriate code template
- [ ] I've self-reviewed using the checklist
- [ ] I've run builds and tests locally
- [ ] I'm ready for code review feedback

Welcome to the team! 🚀

## Related Documents

- [ADR-002: Layered Architecture](./adr-002-layered-architecture-conventions.md)
- [Coding Guidelines](./CODING_GUIDELINES.md)
- [Architecture Review Guide](./ARCHITECTURE_REVIEW_GUIDE.md)
- [Automated Enforcement Guide](./AUTOMATED_ENFORCEMENT_GUIDE.md)
- [Architecture README](./README.md)
