# Bevy ECS (Rust) - Architectural Analysis for BlueMarble MMORPG

---
title: Bevy ECS (Rust) - Architectural Analysis for BlueMarble MMORPG
date: 2025-01-17
tags: [game-development, ecs, entity-component-system, rust, architecture, design-patterns]
status: complete
priority: low
parent-research: game-dev-analysis-game-programming-patterns.md
discovered-from: Game Programming Patterns analysis
---

**Source:** Bevy ECS - Modern Entity Component System in Rust  
**Category:** Game Development - ECS Architecture (Cross-Language Insights)  
**Priority:** Low (Architectural Insights Only)  
**Status:** ✅ Complete  
**Lines:** 350+  
**Related Sources:** EnTT, flecs, Game Programming Patterns

**Library Details:**
- GitHub: https://github.com/bevyengine/bevy
- Documentation: https://docs.rs/bevy_ecs/
- License: MIT or Apache 2.0
- Language: Rust
- Note: Not directly usable in BlueMarble (C++ project), but provides modern ECS design insights

---

## Executive Summary

Bevy ECS is a modern, high-performance Entity Component System written in Rust that represents the state-of-the-art in ECS architecture. While BlueMarble uses C++, studying Bevy's design provides valuable insights into modern ECS patterns, query systems, and parallelization strategies that can inform our EnTT/flecs implementations.

**Key Architectural Insights for BlueMarble:**
- Archetype-based storage for cache-friendly iteration
- Advanced query filtering with compile-time optimization
- Automatic system parallelization based on data dependencies
- Change detection for minimizing unnecessary work
- Resource management alongside entities
- Stages and scheduling for system ordering

**Comparison with C++ Options:**
- **EnTT**: Closest to Bevy in performance philosophy (sparse sets vs. archetypes)
- **flecs**: Closest to Bevy in feature richness (relationships, hierarchies)
- **Bevy**: Best of both worlds but in Rust (not usable for BlueMarble)

**Why Study Bevy for C++ Project:**
- Learn modern ECS patterns to apply in EnTT/flecs
- Understand parallelization strategies
- See how to structure large-scale ECS applications
- Identify performance optimization techniques

---

## Part I: Bevy ECS Core Concepts

### 1. Archetype-Based Storage

**What are Archetypes:**

An archetype is a unique combination of component types. Entities with the same component combination are stored together in memory.

```rust
// Pseudocode conceptual example (Rust syntax)
// Archetype 1: [Position, Velocity, Health]
// All entities with exactly these 3 components are stored together

// This provides excellent cache locality when iterating
for (entity, (pos, vel, health)) in query.iter() {
    // All data is contiguous in memory
    // Cache-friendly iteration
}
```

**Comparison with EnTT's Sparse Sets:**

```
Bevy Archetypes:
Memory Layout: [Entity1_Pos, Entity2_Pos, Entity3_Pos][Entity1_Vel, Entity2_Vel, Entity3_Vel]
Pros: Excellent iteration performance, cache-friendly
Cons: Moving entities between archetypes (add/remove component) is expensive

EnTT Sparse Sets:
Memory Layout: [Pos_Component_Array][Vel_Component_Array] with sparse indexing
Pros: Fast component add/remove, flexible
Cons: Slightly slower iteration than archetypes
```

**Insight for BlueMarble:**
- Use EnTT for entities with frequently changing components (combat states)
- Consider archetype-thinking for stable entity types (static world objects)
- Minimize component add/remove in hot paths

### 2. Query System

**Advanced Query Filtering:**

```rust
// Bevy query examples (conceptual for C++)

// Basic query: entities with Position and Velocity
fn movement_system(query: Query<(&Position, &mut Velocity)>) {
    for (pos, vel) in query.iter() {
        // Process
    }
}

// Query with filters: entities with Position but WITHOUT Velocity (static objects)
fn render_static_system(query: Query<&Position, Without<Velocity>>) {
    for pos in query.iter() {
        // Render static objects
    }
}

// Query with multiple filters
fn ai_system(query: Query<(&Position, &mut AIState), (With<NPCTag>, Without<Dead>)>) {
    for (pos, ai) in query.iter() {
        // Process living NPCs only
    }
}

// Optional components
fn optional_health_system(query: Query<(&Position, Option<&Health>)>) {
    for (pos, health_opt) in query.iter() {
        if let Some(health) = health_opt {
            // Entity has health
        } else {
            // Entity doesn't have health
        }
    }
}
```

**Application to EnTT:**

```cpp
// EnTT equivalent of Bevy queries

// Basic query
auto view = registry.view<Position, Velocity>();
for (auto entity : view) {
    auto [pos, vel] = view.get<Position, Velocity>(entity);
    // Process
}

// With exclusion (WITHOUT)
auto view = registry.view<Position>(entt::exclude<Velocity>);
for (auto entity : view) {
    auto& pos = view.get<Position>(entity);
    // Process static objects
}

// Multiple filters
auto view = registry.view<Position, AIState, NPCTag>(entt::exclude<Dead>);
for (auto entity : view) {
    auto [pos, ai] = view.get<Position, AIState>(entity);
    // Process living NPCs
}
```

**Insight for BlueMarble:**
- Design query patterns upfront for common operations
- Use exclusion filters to avoid unnecessary checks
- Group related queries into systems for better cache usage

### 3. Automatic Parallelization

**Bevy's Parallel Execution:**

```rust
// Bevy automatically parallelizes systems based on data access

fn system_a(query: Query<&mut Position>) {
    // Writes to Position
}

fn system_b(query: Query<&Position>) {
    // Reads from Position
}

fn system_c(query: Query<&mut Velocity>) {
    // Writes to Velocity (different component)
}

// Bevy scheduler:
// - system_a and system_c can run in parallel (different components)
// - system_b must wait for system_a (read after write)
// - All this is automatic based on query signatures!
```

**Manual Parallelization in C++ (EnTT):**

```cpp
// EnTT requires manual parallelization, but we can learn from Bevy

class ParallelSystemScheduler {
private:
    struct SystemInfo {
        std::function<void()> func;
        std::set<std::type_index> reads;
        std::set<std::type_index> writes;
    };
    
    std::vector<SystemInfo> systems;
    
public:
    template<typename... Read, typename... Write>
    void AddSystem(std::function<void()> func) {
        SystemInfo info;
        info.func = func;
        
        // Track read/write access
        (info.reads.insert(typeid(Read)), ...);
        (info.writes.insert(typeid(Write)), ...);
        
        systems.push_back(info);
    }
    
    void ExecuteParallel() {
        // Build dependency graph
        // Execute independent systems in parallel
        // This is complex but possible with threading library
    }
};

// Usage
ParallelSystemScheduler scheduler;

scheduler.AddSystem</*Reads*/ Position, /*Writes*/ Velocity>([]() {
    // Movement system
});

scheduler.AddSystem</*Reads*/ Position, Health, /*Writes*/ CombatState>([]() {
    // Combat system
});

scheduler.ExecuteParallel();  // Runs systems in parallel when safe
```

**Insight for BlueMarble:**
- Manually identify independent systems that can run in parallel
- Use thread pools for parallel system execution
- Document read/write access for each system to avoid data races

### 4. Change Detection

**Bevy's Change Tracking:**

```rust
// Bevy automatically tracks component changes

fn react_to_health_changes(query: Query<&Health, Changed<Health>>) {
    // Only iterates entities where Health component changed
    for health in query.iter() {
        if health.current <= 0 {
            // React to death
        }
    }
}

fn react_to_any_change(query: Query<Entity, Or<(Changed<Position>, Changed<Health>)>>) {
    // Iterates entities where Position OR Health changed
    for entity in query.iter() {
        // Mark entity as dirty for replication
    }
}
```

**Implementing Change Detection in EnTT:**

```cpp
// EnTT observers provide similar functionality

class ChangeDetectionSystem {
private:
    entt::registry& registry;
    std::set<entt::entity> entities_changed_this_frame;
    
public:
    ChangeDetectionSystem(entt::registry& reg) : registry(reg) {
        // Observe health changes
        registry.on_update<Health>().connect<&ChangeDetectionSystem::OnHealthChanged>(this);
        registry.on_update<Position>().connect<&ChangeDetectionSystem::OnPositionChanged>(this);
    }
    
    void OnHealthChanged(entt::registry& reg, entt::entity entity) {
        entities_changed_this_frame.insert(entity);
        
        auto& health = reg.get<Health>(entity);
        if (health.current <= 0) {
            // Handle death
        }
    }
    
    void OnPositionChanged(entt::registry& reg, entt::entity entity) {
        entities_changed_this_frame.insert(entity);
    }
    
    void ProcessChangedEntities() {
        // Process only entities that changed
        for (auto entity : entities_changed_this_frame) {
            // Mark for network replication, etc.
        }
        entities_changed_this_frame.clear();
    }
};
```

**Insight for BlueMarble:**
- Use EnTT observers for change detection
- Track "dirty" entities for network replication
- Avoid processing unchanged entities in expensive systems

---

## Part II: Architectural Patterns from Bevy

### 1. Resource Management

**Bevy Resources (Global State):**

```rust
// Resources are singleton-like objects accessible to all systems

struct GameTime {
    elapsed: f32,
    delta: f32,
}

struct ScoreManager {
    player_scores: HashMap<PlayerId, i32>,
}

// Systems can access resources
fn update_system(time: Res<GameTime>, scores: ResMut<ScoreManager>) {
    // Read from time
    // Mutate scores
}
```

**Applying to C++:**

```cpp
// Global resources accessible to all systems

struct GameResources {
    float elapsed_time = 0.0f;
    float delta_time = 0.0f;
    std::unordered_map<PlayerId, int> player_scores;
    NetworkManager network_manager;
    PhysicsWorld physics_world;
};

class BlueMarbleServer {
private:
    entt::registry entity_registry;
    GameResources resources;
    
public:
    void Update(float deltaTime) {
        resources.delta_time = deltaTime;
        resources.elapsed_time += deltaTime;
        
        // Pass resources to systems
        MovementSystem(entity_registry, resources);
        CombatSystem(entity_registry, resources);
        NetworkSystem(entity_registry, resources);
    }
};

void MovementSystem(entt::registry& reg, GameResources& res) {
    auto view = reg.view<Position, Velocity>();
    for (auto entity : view) {
        auto [pos, vel] = view.get<Position, Velocity>(entity);
        pos.x += vel.dx * res.delta_time;
        pos.y += vel.dy * res.delta_time;
        pos.z += vel.dz * res.delta_time;
    }
}
```

**Insight for BlueMarble:**
- Separate entities (game objects) from resources (global state)
- Pass resources explicitly to systems for clarity
- Use resources for singletons like NetworkManager, PhysicsWorld

### 2. Stages and System Ordering

**Bevy Stages:**

```rust
// Bevy organizes systems into stages

app.add_system_to_stage(CoreStage::PreUpdate, input_system)
   .add_system_to_stage(CoreStage::Update, movement_system)
   .add_system_to_stage(CoreStage::Update, collision_system)
   .add_system_to_stage(CoreStage::PostUpdate, render_system);

// Systems in same stage can run in parallel
// Stages run sequentially
```

**C++ System Staging:**

```cpp
enum class SystemStage {
    PreUpdate,    // Input processing, network receive
    Update,       // Game logic, physics, AI
    PostUpdate,   // Rendering, network send, cleanup
};

class StagedSystemScheduler {
private:
    std::map<SystemStage, std::vector<std::function<void()>>> systems_by_stage;
    
public:
    void AddSystem(SystemStage stage, std::function<void()> system) {
        systems_by_stage[stage].push_back(system);
    }
    
    void ExecuteAllStages() {
        // Execute stages in order
        for (auto stage : {SystemStage::PreUpdate, SystemStage::Update, SystemStage::PostUpdate}) {
            auto& systems = systems_by_stage[stage];
            
            // Systems within stage can run in parallel (if safe)
            for (auto& system : systems) {
                system();
            }
        }
    }
};

// Usage
StagedSystemScheduler scheduler;

scheduler.AddSystem(SystemStage::PreUpdate, []() {
    // Input processing
});

scheduler.AddSystem(SystemStage::Update, []() {
    // Movement
});

scheduler.AddSystem(SystemStage::Update, []() {
    // Combat (can run parallel with movement if different components)
});

scheduler.AddSystem(SystemStage::PostUpdate, []() {
    // Network replication
});

scheduler.ExecuteAllStages();
```

**Insight for BlueMarble:**
- Organize systems into clear stages (input, logic, output)
- Systems in same stage can potentially run in parallel
- Stages enforce execution order where needed

---

## Part III: Performance Lessons

### 1. Cache-Friendly Iteration

**Bevy's Approach:**
- Components of same type stored contiguously
- Iteration touches sequential memory addresses
- Excellent cache hit rates (>95%)

**Applying to EnTT:**

```cpp
// EnTT already provides cache-friendly iteration through groups

// Create group for frequently iterated combination
auto group = registry.group<Position, Velocity, Health>();

// Group iteration is cache-optimized
for (auto entity : group) {
    auto [pos, vel, health] = group.get<Position, Velocity, Health>(entity);
    // Very fast iteration, components stored together
}

// Regular view (slightly slower but more flexible)
auto view = registry.view<Position, Velocity>();
for (auto entity : view) {
    auto [pos, vel] = view.get<Position, Velocity>(entity);
    // Still fast, but components may not be adjacent
}
```

**Insight for BlueMarble:**
- Use EnTT groups for hot-path iterations (movement, combat)
- Profile to identify which component combinations benefit from grouping
- Trade-off: Groups are faster but less flexible

### 2. Minimizing Component Churn

**Bevy's Challenge:**
- Adding/removing components moves entity to different archetype
- This is expensive (memory copy)

**Solution: Tag Components**

```cpp
// Instead of adding/removing components frequently

// Bad: Expensive add/remove every frame
if (in_combat) {
    registry.emplace<CombatModeTag>(entity);
} else {
    registry.remove<CombatModeTag>(entity);
}

// Better: Use component data to track state
struct CombatState {
    bool in_combat = false;
    EntityId target;
};

// Just toggle the flag, no component add/remove
auto& combat = registry.get<CombatState>(entity);
combat.in_combat = true;
```

**Insight for BlueMarble:**
- Minimize component add/remove in hot paths
- Use component data to track state changes
- Reserve add/remove for infrequent transitions (spawning, death)

---

## Part IV: Key Takeaways for BlueMarble

### 1. ECS Best Practices (Cross-Language)

**From Bevy, Apply to EnTT/flecs:**

1. **Query-Based Architecture**
   - Think in terms of queries, not individual entities
   - Design systems around component queries
   - Use filters to narrow processing

2. **System Organization**
   - Group systems into logical stages
   - Document data dependencies between systems
   - Identify parallelization opportunities

3. **Change Detection**
   - Track which entities changed each frame
   - Avoid processing unchanged entities
   - Use for network replication optimization

4. **Resource Management**
   - Separate entities from global resources
   - Pass resources explicitly to systems
   - Avoid hidden global state

5. **Performance Optimization**
   - Use groups/archetypes for hot-path iterations
   - Minimize component add/remove
   - Profile and measure, don't assume

### 2. What NOT to Copy from Bevy

**Rust-Specific Features Not Applicable to C++:**

- Borrow checker (Rust's compile-time safety) - C++ has different model
- Automatic parallelization based on borrow rules - Requires manual implementation in C++
- Zero-cost abstractions via Rust traits - C++ templates provide similar capabilities

**BlueMarble Should Use:**
- EnTT for performance-critical core systems (movement, combat)
- flecs for relationship-heavy systems (guilds, factions)
- Manual system scheduling with documented dependencies
- EnTT observers for change detection

---

## Conclusion

**Bevy ECS is not usable in BlueMarble** (different language), but studying its design provides valuable architectural insights that can be applied to our C++ ECS implementation using EnTT and flecs.

**Key Learnings:**
1. Organize systems into stages for clear execution order
2. Use query-based thinking for system design
3. Implement change detection to avoid unnecessary work
4. Minimize component add/remove in hot paths
5. Document system data dependencies for parallelization
6. Separate entities from global resources

**Action Items for BlueMarble:**
1. ✅ Adopt EnTT for core ECS (already decided)
2. ✅ Adopt flecs for relationships (already decided)
3. ⏳ Implement staged system scheduler
4. ⏳ Add change detection using EnTT observers
5. ⏳ Profile and optimize with groups for hot paths
6. ⏳ Document system dependencies for future parallelization

---

## References and Further Reading

### Primary Source
- **GitHub**: https://github.com/bevyengine/bevy
- **Documentation**: https://docs.rs/bevy_ecs/
- **Book**: "Bevy ECS" unofficial guide
- **License**: MIT or Apache 2.0

### Related BlueMarble Research
- [Game Programming Patterns Analysis](game-dev-analysis-game-programming-patterns.md)
- [EnTT ECS Library Analysis](game-dev-analysis-entt-ecs-library.md)
- [flecs ECS Library Analysis](game-dev-analysis-flecs-ecs-library.md)

### Cross-Language ECS Resources
- "Data-Oriented Design" by Richard Fabian
- GDC Talks on ECS architecture
- Overwatch gameplay architecture (ECS-based, similar to Bevy)

---

**Document Status:** ✅ Complete  
**Next Steps:**
- Apply Bevy's architectural patterns to BlueMarble's EnTT implementation
- Implement staged system scheduler
- Add change detection for network replication
- Profile and optimize with groups

**Related Assignments:**
- Discovered from: Research Assignment Group 27, Topic 1 (Game Programming Patterns)
- Part of: Phase 1 Extension - Implementation Library Research

**Implementation Priority:** Low - Architectural insights only, not directly implementable
