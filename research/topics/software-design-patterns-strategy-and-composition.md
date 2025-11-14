---
title: Software Design Patterns - Strategy Pattern and Composition-Based Design
date: 2025-11-14
tags: [software-architecture, design-patterns, strategy-pattern, composition, inheritance, refactoring]
status: complete
priority: high
author: Research Team
---

# Software Design Patterns - Strategy Pattern and Composition-Based Design

## Problem / Context

As BlueMarble MMORPG grows in complexity, we need to understand architectural patterns that promote flexible, maintainable code. This research focuses on patterns particularly relevant for layered systems where behavior varies across modules:

- How can we vary behavior without creating an explosion of subclasses?
- When should we use composition over inheritance?
- What are the trade-offs between Template Method, Strategy, and composition-based designs?
- How do these patterns apply to a multi-layered MMORPG architecture (scraping, data extraction, storage)?

## Key Findings

### Strategy Pattern (Composition-Based)

**Overview**: The Strategy pattern encapsulates interchangeable behaviors (algorithms or tactics) into separate classes, which are then composed with the context object. Instead of using inheritance to override parts of an algorithm, the context class holds a reference to a strategy interface and delegates work to it.

- **Finding 1: Flexibility Through Composition**
  - Strategy enables runtime behavior changes without modifying context class
  - Context delegates work to strategy objects via interface
  - Multiple strategies can be swapped dynamically based on conditions
  - Follows Open/Closed Principle: open for extension, closed for modification

- **Finding 2: Reduces Subclass Explosion**
  - Instead of creating subclasses for every combination of behaviors, define strategy interfaces
  - Example: Rather than "YouTube+DB" and "YouTube+File" subclasses, use one YouTube class with StorageStrategy
  - Strategies can be mixed and matched independently
  - Promotes code reuse by combining simple, focused components

- **Finding 3: Separates Concerns**
  - Complex logic for each behavior is isolated in its own strategy class
  - Easier to reason about, test, and maintain individual strategies
  - Each strategy focuses on a single responsibility
  - Context remains simple, delegating specialized work to strategies

### Composition and Shallow Inheritance

- **Finding 1: Favor "Has-A" Over Deep "Is-A" Hierarchies**
  - Build classes by aggregating simpler components rather than deep inheritance chains
  - Use helper objects, mixins, or services for shared functionality
  - Keep inheritance hierarchies shallow (a few levels at most)
  - Beyond 2-3 levels, consider refactoring to composition

- **Finding 2: Flexible and Loosely Coupled**
  - Composition provides flexibility to change or replace components
  - Components can be reused across different parts of the system
  - Easier to test: mock or stub out individual components
  - Avoids fragile base class problem where base changes ripple through subclasses

- **Finding 3: Better SOLID Adherence**
  - Single Responsibility: Each component has one job
  - Open/Closed: Add features by composing new components
  - Liskov Substitution: Not forced into complex inheritance hierarchies
  - Interface Segregation: Components use focused interfaces
  - Dependency Inversion: Depend on abstractions (interfaces), not concrete implementations

## Evidence

### Source 1: Refactoring.Guru - Strategy Pattern

- **Link**: https://refactoring.guru/design-patterns/strategy
- **Key Points**:
  - Strategy defines a family of algorithms, encapsulates each one, and makes them interchangeable
  - Context maintains reference to strategy object and delegates work to it
  - Useful when you have similar classes that differ only in how they execute some behavior
  - Client must be aware of differences between strategies to select the appropriate one
- **Relevance**: Directly addresses how to handle varying behaviors without inheritance explosion

### Source 2: Refactoring.Guru - Composition Over Inheritance

- **Link**: https://refactoring.guru/design-patterns (general principles)
- **Key Points**:
  - Composition is more flexible than inheritance
  - Can change behavior at runtime by swapping composed objects
  - Avoids rigid, hard-to-change inheritance hierarchies
  - Strategy pattern is a prime example of composition over inheritance
- **Relevance**: Provides theoretical foundation for why composition-based patterns are preferable

### Source 3: Software Engineering Stack Exchange

- **Link**: softwareengineering.stackexchange.com
- **Key Points**:
  - Strategy pattern recommended for data import workflows with multiple input formats
  - Define ReadStrategy interface with implementations like XMLReadStrategy, CSVReadStrategy
  - Input format handling independent from validation or output handling
  - "Have a generic base class and for each input format a subclass strategy"
  - Adding new format is as simple as implementing new strategy class
- **Relevance**: Concrete example of Strategy in data processing pipelines, similar to BlueMarble's scraping/extraction layers

### Source 4: Medium - Composition vs Inheritance

- **Link**: medium.com (various articles on composition patterns)
- **Key Points**:
  - Deep inheritance leads to fragile base class problem
  - Hard to manage and understand complex hierarchies
  - Composition allows optional features without forcing them into base class
  - General guideline: keep inheritance shallow, use composition beyond 2-3 levels
  - Highly compositional designs make each piece simpler and more maintainable
- **Relevance**: Provides practical guidance on when to stop using inheritance and switch to composition

## When to Use Each Pattern

### Strategy Pattern

**Use When:**
- Variations in behavior need to be selected or changed at runtime
- A module has multiple aspects that vary independently
- You have many conditional statements switching between behaviors
- Different variations of an algorithm need to be easily swappable
- You want to avoid combinatorial explosion of subclasses

**Example Scenarios:**
- Multi-layer scraper with extraction strategy (HTML parsing vs API fetching)
- Data storage strategy (save to DB vs save to file)
- Compression algorithm selection (ZIP, GZIP, BZIP2)
- Pathfinding algorithms (A*, Dijkstra, BFS)
- Payment processing methods (credit card, PayPal, cryptocurrency)

### Composition

**Use When:**
- Behavior can be cleanly separated as independent component
- Inheritance hierarchy is getting too deep (3+ levels)
- Multiple classes need the same feature but aren't part of same hierarchy
- Features are optional and don't apply to all subclasses
- You need to mix and match features at runtime

**Example Scenarios:**
- Caching behavior: Use CacheHelper instead of CachableYouTubeSource subclass
- Logging: Compose logger into classes that need it
- Authentication: Add authentication component to services that require it
- Validation: Compose validator objects instead of validation inheritance chain

### Template Method (For Comparison)

**Use When:**
- Common process with variant steps per module
- Want to enforce consistent workflow across all implementations
- Algorithm skeleton is stable, only specific steps vary
- Subclasses should only override specific parts, not entire algorithm

**Example Scenarios:**
- Data processing pipeline (load → validate → transform → save)
- Game update loop (input → update → render)
- HTTP request handling (parse → authenticate → process → respond)

## Benefits and Drawbacks

### Strategy Pattern

**Benefits:**
- **Flexibility**: Swap behaviors without changing context (Open/Closed Principle)
- **Separation of Concerns**: Each strategy is a simple, focused class
- **Reduced Subclassing**: One context class instead of many subclasses
- **Runtime Configuration**: Choose strategy dynamically based on conditions
- **Easier Testing**: Test strategies in isolation
- **Promotes Reuse**: Strategies can be shared across different contexts

**Drawbacks:**
- **More Classes**: Additional classes and interfaces to manage
- **Wiring Complexity**: Must configure which strategy to use (dependency injection, factories)
- **Client Awareness**: Clients need to understand strategy differences to choose correctly
- **Slight Overhead**: Extra object creation and indirection (usually negligible)
- **Overkill for Simple Cases**: If only 2-3 variations, simple conditional might suffice

### Composition

**Benefits:**
- **Flexibility**: Change or replace components without affecting others
- **Loose Coupling**: Components are independent
- **Reusability**: Same component used across system
- **Testability**: Easy to mock or stub components
- **Simplicity**: Smaller, focused pieces easier to understand
- **SOLID Compliance**: Better adherence to design principles

**Drawbacks:**
- **More Moving Parts**: Need to understand class and its collaborators
- **Interface Coordination**: Components need well-defined interfaces
- **Initial Complexity**: More upfront design work
- **Slight Performance Overhead**: More objects and indirection (usually negligible)

### Template Method

**Benefits:**
- **Code Reuse**: Common algorithm in base class, avoid duplication
- **Enforced Structure**: All subclasses follow same workflow
- **Clear Override Points**: Subclasses only implement specific steps
- **Simplicity**: Straightforward inheritance-based approach

**Drawbacks:**
- **Rigidity**: Changing algorithm structure affects all subclasses
- **Inheritance Coupling**: Tight coupling between base and subclasses
- **No Runtime Flexibility**: Can't change behavior after object creation
- **Deep Hierarchies**: Can lead to complex inheritance trees if overused

## Concrete Examples

### Example 1: Data Import Module with Strategy Pattern

**Problem**: Import module needs to read various file formats and output to different destinations.

**Solution with Strategy Pattern**:

```csharp
// Strategy interface
public interface IReadStrategy
{
    DataSet Read(string filePath);
}

// Concrete strategies
public class XMLReadStrategy : IReadStrategy
{
    public DataSet Read(string filePath)
    {
        // XML parsing logic
        return ParseXML(filePath);
    }
}

public class CSVReadStrategy : IReadStrategy
{
    public DataSet Read(string filePath)
    {
        // CSV parsing logic
        return ParseCSV(filePath);
    }
}

public class JSONReadStrategy : IReadStrategy
{
    public DataSet Read(string filePath)
    {
        // JSON parsing logic
        return ParseJSON(filePath);
    }
}

// Context that uses strategy
public class DataImporter
{
    private IReadStrategy _readStrategy;
    
    public DataImporter(IReadStrategy readStrategy)
    {
        _readStrategy = readStrategy;
    }
    
    public void SetReadStrategy(IReadStrategy readStrategy)
    {
        _readStrategy = readStrategy;
    }
    
    public void ImportData(string filePath)
    {
        DataSet data = _readStrategy.Read(filePath);
        ValidateData(data);
        ProcessData(data);
    }
}

// Usage
var importer = new DataImporter(new CSVReadStrategy());
importer.ImportData("data.csv");

// Runtime strategy change
importer.SetReadStrategy(new XMLReadStrategy());
importer.ImportData("data.xml");
```

### Example 2: BlueMarble Scraper with Multiple Strategies

**Problem**: Multi-layer scraper needs different extraction methods and storage options. Creating subclasses for every combination would explode: YouTubeDBScraper, YouTubeFileScraper, VimeoDBScraper, VimeoFileScraper, etc.

**Solution with Strategy Pattern**:

```csharp
// Extraction strategies
public interface IExtractionStrategy
{
    ScrapedData Extract(string url);
}

public class HTMLExtractionStrategy : IExtractionStrategy
{
    public ScrapedData Extract(string url)
    {
        // HTML parsing with BeautifulSoup, Selenium, etc.
        return ParseHTML(url);
    }
}

public class APIExtractionStrategy : IExtractionStrategy
{
    public ScrapedData Extract(string url)
    {
        // API calls with REST client
        return FetchFromAPI(url);
    }
}

// Storage strategies
public interface IStorageStrategy
{
    void Save(ScrapedData data);
}

public class DatabaseStorageStrategy : IStorageStrategy
{
    public void Save(ScrapedData data)
    {
        // Save to database
        SaveToDB(data);
    }
}

public class FileStorageStrategy : IStorageStrategy
{
    public void Save(ScrapedData data)
    {
        // Save to file system
        SaveToFile(data);
    }
}

// Context: unified scraper with two strategies
public class WebScraper
{
    private IExtractionStrategy _extractionStrategy;
    private IStorageStrategy _storageStrategy;
    
    public WebScraper(
        IExtractionStrategy extractionStrategy,
        IStorageStrategy storageStrategy)
    {
        _extractionStrategy = extractionStrategy;
        _storageStrategy = storageStrategy;
    }
    
    public void Scrape(string url)
    {
        ScrapedData data = _extractionStrategy.Extract(url);
        ProcessData(data);
        _storageStrategy.Save(data);
    }
}

// Usage - mix and match strategies
var scraper1 = new WebScraper(
    new HTMLExtractionStrategy(),
    new DatabaseStorageStrategy()
);

var scraper2 = new WebScraper(
    new APIExtractionStrategy(),
    new FileStorageStrategy()
);

// Only TWO strategy interfaces instead of FOUR subclasses
// And can easily add: RSSExtractionStrategy, CloudStorageStrategy, etc.
```

### Example 3: Composition vs Deep Inheritance

**Bad: Deep Inheritance**:
```csharp
// Deep hierarchy - fragile and rigid
public class Entity { }
public class MovableEntity : Entity { }
public class AnimatedEntity : MovableEntity { }
public class CombatEntity : AnimatedEntity { }
public class PlayerCharacter : CombatEntity { }
public class WarriorCharacter : PlayerCharacter { }
// Hard to maintain, changes ripple through entire chain
```

**Good: Shallow Inheritance + Composition**:
```csharp
// Shallow hierarchy
public abstract class Entity
{
    public Vector3 Position { get; set; }
}

public class Character : Entity
{
    // Composed components
    public MovementComponent Movement { get; }
    public AnimationComponent Animation { get; }
    public CombatComponent Combat { get; }
    
    public Character()
    {
        Movement = new MovementComponent(this);
        Animation = new AnimationComponent(this);
        Combat = new CombatComponent(this);
    }
}

// Components are independent and reusable
public class MovementComponent
{
    private Entity _owner;
    
    public MovementComponent(Entity owner)
    {
        _owner = owner;
    }
    
    public void Move(Vector3 direction)
    {
        _owner.Position += direction;
    }
}

// Easy to add/remove features
public class Warrior : Character
{
    public WarriorSkills Skills { get; }
    
    public Warrior()
    {
        Skills = new WarriorSkills(this);
    }
}

// Can reuse components in different contexts
public class NPC : Entity
{
    public MovementComponent Movement { get; }  // Reuses same component
    public AIComponent AI { get; }              // NPC-specific component
}
```

## Comparison Table

| Approach | Mechanism | Use When | Pros | Cons |
|----------|-----------|----------|------|------|
| **Template Method** | Base class defines algorithm skeleton; subclasses override specific steps (inheritance) | - Common process with variant steps per module<br>- Want to enforce consistent workflow across all subclasses | - Eliminates duplicate code by reusing base algorithm<br>- Subclasses focus only on their unique logic<br>- Ensures uniform sequence (easy to follow one template) | - Rigid: changing algorithm affects all subclasses<br>- Inheritance coupling (less flexible at runtime)<br>- Can lead to deep hierarchies if overused |
| **Strategy Pattern** | Context holds strategy object; behavior is delegated to it (composition) | - Need to swap or vary behavior at runtime<br>- Avoid combinatorial subclasses for independent variations | - Flexible: can change behavior without modifying context (follows Open/Closed)<br>- Encourages composition and separation of concerns (each strategy is simpler)<br>- Reduces subclass explosion (one context class instead of many) | - More classes and interfaces to manage (added complexity)<br>- Client must select or be aware of appropriate strategy<br>- Slight overhead in wiring strategies into context |
| **Composition (General)** | Build classes by containing other components or using mixins, rather than deep inheritance | - Behavior can be cleanly separated as independent component<br>- Inheritance hierarchy getting too deep (3+ levels)<br>- Optional features that don't apply to all subclasses | - Flexible: can change or replace components without affecting others<br>- Loose coupling: components are independent<br>- Promotes reusability across system<br>- Makes testing easier (mock components) | - More moving parts to understand (class + collaborators)<br>- Need well-defined interfaces between components<br>- Initial design complexity |

## Implications for BlueMarble Design

### Implication 1: Scraping/Extraction Layer Architecture
- **Design Consideration**: Use Strategy pattern for extraction methods (HTML, API, RSS)
- **Design Consideration**: Use Strategy pattern for storage targets (database, file, cache)
- **Potential Impact**: Can add new platforms or storage methods without modifying core scraper logic
- **Potential Impact**: Mix and match extraction and storage strategies independently

### Implication 2: Game Systems with Variable Behavior
- **Design Consideration**: Combat system with different AI behaviors (aggressive, defensive, tactical)
- **Design Consideration**: Crafting system with different recipe strategies (standard, advanced, experimental)
- **Design Consideration**: Quest system with different reward strategies (XP, items, currency, reputation)
- **Potential Impact**: Systems become more modular and easier to balance/test
- **Potential Impact**: Can A/B test different strategies with players

### Implication 3: Entity Component System (ECS)
- **Design Consideration**: Use composition heavily instead of deep entity hierarchies
- **Design Consideration**: Components for movement, rendering, combat, inventory, etc.
- **Design Consideration**: Entities are containers of components, not deep inheritance chains
- **Potential Impact**: More flexible entity creation (players, NPCs, objects all use same component system)
- **Potential Impact**: Better performance through data-oriented design

### Implication 4: Avoid Deep Inheritance
- **Design Consideration**: Keep entity hierarchies to 2-3 levels maximum
- **Design Consideration**: Use composition for features beyond that depth
- **Design Consideration**: Critically evaluate each inheritance level: "Is this truly an 'is-a' relationship?"
- **Potential Impact**: Easier to maintain and understand class relationships
- **Potential Impact**: Reduced risk of breaking changes cascading through hierarchy

## Open Questions / Next Steps

### Open Questions
- How do we decide between Strategy pattern and simple polymorphism for BlueMarble systems?
- What is the performance overhead of Strategy pattern vs Template Method in C# game context?
- Should we use dependency injection framework for strategy wiring, or manual factory patterns?
- How do we document strategy choices for other developers to understand when to use each?

### Next Steps
- [ ] Review BlueMarble's current architecture for opportunities to apply Strategy pattern
- [ ] Identify any deep inheritance hierarchies (3+ levels) that could benefit from composition refactoring
- [ ] Create architectural decision record (ADR) for when to use each pattern
- [ ] Develop coding guidelines for Strategy pattern and composition in BlueMarble
- [ ] Create code examples specific to BlueMarble's codebase
- [ ] Consider creating custom scaffolding tools or templates for common strategy implementations

## Related Documents

- [Game Programming Patterns Analysis](../literature/game-dev-analysis-game-programming-patterns.md)
- [Entity Component System Research](../literature/game-dev-analysis-entt-entity-component-system.md)
- [Software Architecture Best Practices](../literature/game-dev-analysis-design-patterns-project.md)
