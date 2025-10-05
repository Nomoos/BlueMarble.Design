# flecs - Fast and Flexible Entity Component System Analysis for BlueMarble MMORPG

---
title: flecs - Fast and Flexible Entity Component System Analysis for BlueMarble MMORPG
date: 2025-01-17
tags: [game-development, ecs, entity-component-system, performance, c, cpp, library, query-system]
status: complete
priority: medium
parent-research: game-dev-analysis-game-programming-patterns.md
discovered-from: Game Programming Patterns analysis
---

**Source:** flecs - A fast entity component system (ECS) for C & C++  
**Category:** Game Development - ECS Implementation Library  
**Priority:** Medium  
**Status:** ✅ Complete  
**Lines:** 600+  
**Related Sources:** Game Programming Patterns, EnTT Analysis

**Online Resources:**
- GitHub: https://github.com/SanderMertens/flecs
- Documentation: https://www.flecs.dev/flecs/
- Examples: https://github.com/SanderMertens/flecs/tree/master/examples
- License: MIT License
- Language: C99 with C++ API

---

## Executive Summary

flecs is a cross-platform Entity Component System library written in C99 with comprehensive C++ bindings. Discovered during the Game Programming Patterns analysis, flecs distinguishes itself through its built-in query system, entity relationships, hierarchies, and reflection capabilities. While EnTT focuses on raw performance, flecs emphasizes flexibility and feature richness.

**Key Takeaways for BlueMarble:**
- Cross-platform C/C++ compatibility enables potential multi-language integration
- Built-in query DSL for complex entity filtering beyond simple component matching
- Native support for entity relationships (parent-child, ownership, etc.)
- Hierarchical entity organization useful for world regions and organizational structures
- Reflection system enables runtime introspection and serialization
- Modular systems with automatic dependency resolution
- Performance: ~50-100ns per entity iteration (slower than EnTT but with more features)

**Primary Application Areas:**
1. **Complex Entity Relationships**: Faction hierarchies, ownership chains, spatial hierarchies
2. **Query-Heavy Systems**: Advanced AI requiring complex entity queries
3. **Multi-Language Integration**: C API enables integration with scripting languages
4. **Hierarchical World Structure**: Region -> Zone -> Area organization
5. **Runtime Inspection**: Debug tools, admin panels, live entity inspection

**Comparison with EnTT:**
- **EnTT**: Faster (2-10ns/entity), more lightweight, C++-only
- **flecs**: More features, relationships, queries, reflection, C/C++ API
- **Recommendation**: EnTT for performance-critical core systems, flecs for complex relationship-heavy systems

---

## Part I: flecs Architecture and Core Concepts

### 1. Core Design Philosophy

**Entity-Relationship Model:**

Unlike pure component-based systems, flecs extends ECS with relationships between entities, enabling more expressive data modeling.

```c
// Traditional ECS: Entity has components
Entity player = ecs.entity();
player.add<Position>();
player.add<Health>();

// flecs: Entity can have relationships with other entities
Entity faction = ecs.entity("NorthernKingdom");
Entity player = ecs.entity();
player.add<Position>();
player.add<Health>();
player.add<MemberOf>(faction);  // Relationship: player is member of faction

// Query entities by relationship
auto query = ecs.query<Position>().with<MemberOf>(faction);
```

**Benefits for BlueMarble:**
- **Explicit Relationships**: Model guild membership, faction allegiance, ownership
- **Hierarchical Queries**: Find all entities in a region or owned by a player
- **Automatic Cleanup**: When faction is destroyed, relationships are cleaned up
- **Type-Safe**: Relationships are compile-time checked like components

### 2. Basic flecs Usage (C++ API)

**Creating Entities and Components:**

```cpp
#include <flecs.h>

// Define components as structs
struct Position {
    float x, y, z;
};

struct Velocity {
    float dx, dy, dz;
};

struct Health {
    int current;
    int maximum;
};

int main() {
    flecs::world ecs;
    
    // Register components (optional, enables reflection)
    ecs.component<Position>();
    ecs.component<Velocity>();
    ecs.component<Health>();
    
    // Create entity and add components
    auto player = ecs.entity("Player")
        .set<Position>({0.0f, 0.0f, 0.0f})
        .set<Velocity>({1.0f, 0.0f, 0.0f})
        .set<Health>({100, 100});
    
    // Access components
    const Position* pos = player.get<Position>();
    
    // Modify component
    player.set<Position>({pos->x + 1.0f, pos->y, pos->z});
    
    // Check if entity has component
    if (player.has<Velocity>()) {
        // ...
    }
    
    // Remove component
    player.remove<Velocity>();
    
    return 0;
}
```

### 3. Systems and Queries

**System Definition:**

```cpp
// Define system that processes entities with Position and Velocity
ecs.system<Position, Velocity>()
    .each([](flecs::entity e, Position& pos, Velocity& vel) {
        pos.x += vel.dx * e.delta_time();
        pos.y += vel.dy * e.delta_time();
        pos.z += vel.dz * e.delta_time();
    });

// System with multiple components and filtering
ecs.system<Position, Health, const CombatState>()
    .with<PlayerTag>()  // Only process players
    .each([](flecs::entity e, Position& pos, Health& health, const CombatState& combat) {
        if (health.current <= 0) {
            e.destruct();  // Remove entity
        }
    });

// Execute all systems
ecs.progress();  // Run one frame
```

**Advanced Queries:**

```cpp
// Query with multiple filters
auto query = ecs.query_builder<Position, Health>()
    .with<PlayerTag>()           // Must have PlayerTag
    .without<DeadTag>()          // Must not have DeadTag
    .with<MemberOf>(faction)     // Must be member of faction
    .build();

query.each([](flecs::entity e, Position& pos, Health& health) {
    // Process living players in this faction
});

// Query with wildcards
auto query = ecs.query_builder<Position>()
    .with(flecs::ChildOf, flecs::Wildcard)  // Has any parent
    .build();

// Query with optional components
auto query = ecs.query_builder<Position>()
    .with<Velocity>().optional()  // Velocity is optional
    .build();

query.each([](flecs::entity e, Position& pos, Velocity* vel) {
    if (vel) {
        // Entity has velocity
    }
});
```

---

## Part II: Advanced Features for MMORPG Development

### 1. Entity Relationships and Hierarchies

**Parent-Child Relationships:**

```cpp
// Create region hierarchy
auto world = ecs.entity("World");
auto continent = ecs.entity("NorthernContinent").child_of(world);
auto region = ecs.entity("ForestRegion").child_of(continent);
auto zone = ecs.entity("DarkForest").child_of(region);

// Create player in zone
auto player = ecs.entity("Player").child_of(zone);

// Query all entities in a zone
auto entities_in_zone = ecs.query_builder<Position>()
    .with(flecs::ChildOf, zone)
    .build();

// Recursive query (includes descendants)
auto all_in_continent = ecs.query_builder<Position>()
    .with(flecs::ChildOf, continent).cascade()
    .build();
```

**Custom Relationships:**

```cpp
// Define relationship types
struct Owns {};
struct Allies {};
struct Enemies {};

// Player owns items
auto player = ecs.entity("Player");
auto sword = ecs.entity("Sword").add<Owns>(player);
auto shield = ecs.entity("Shield").add<Owns>(player);

// Query player's inventory
auto inventory = ecs.query_builder<Item>()
    .with<Owns>(player)
    .build();

// Faction relationships
auto faction_a = ecs.entity("FactionA");
auto faction_b = ecs.entity("FactionB");
auto faction_c = ecs.entity("FactionC");

faction_a.add<Allies>(faction_b);
faction_a.add<Enemies>(faction_c);

// Query allied factions
auto allies_query = ecs.query_builder<FactionData>()
    .with<Allies>(faction_a)
    .build();
```

**BlueMarble Application:**

```cpp
// Guild structure
auto guild = ecs.entity("DragonSlayers");
auto leader = ecs.entity("GuildLeader").add<LeaderOf>(guild);
auto member = ecs.entity("GuildMember").add<MemberOf>(guild);

// Building ownership
auto player = ecs.entity("Player");
auto house = ecs.entity("PlayerHouse")
    .add<Owns>(player)
    .set<Position>({100.0f, 50.0f, 0.0f});

// Resource node control
auto faction = ecs.entity("NorthernKingdom");
auto mine = ecs.entity("IronMine")
    .add<ControlledBy>(faction)
    .set<ResourceNode>({ResourceType::Iron, 1000});
```

### 2. Modules and System Organization

**Defining Modules:**

```cpp
struct PhysicsModule {
    PhysicsModule(flecs::world& ecs) {
        ecs.module<PhysicsModule>();
        
        // Import dependencies
        ecs.import<TransformModule>();
        
        // Define components
        ecs.component<Velocity>();
        ecs.component<Acceleration>();
        
        // Define systems
        ecs.system<Position, Velocity>()
            .kind(flecs::OnUpdate)
            .each([](flecs::entity e, Position& pos, Velocity& vel) {
                pos.x += vel.dx * e.delta_time();
                pos.y += vel.dy * e.delta_time();
                pos.z += vel.dz * e.delta_time();
            });
        
        ecs.system<Velocity, const Acceleration>()
            .kind(flecs::OnUpdate)
            .each([](flecs::entity e, Velocity& vel, const Acceleration& acc) {
                vel.dx += acc.x * e.delta_time();
                vel.dy += acc.y * e.delta_time();
                vel.dz += acc.z * e.delta_time();
            });
    }
};

// Import module
flecs::world ecs;
ecs.import<PhysicsModule>();
```

**System Phases:**

```cpp
// Define custom phases
auto PreUpdate = ecs.entity().add(flecs::Phase);
auto OnUpdate = ecs.entity().add(flecs::Phase).depends_on(PreUpdate);
auto PostUpdate = ecs.entity().add(flecs::Phase).depends_on(OnUpdate);

// Systems in different phases
ecs.system<NetworkComponent>()
    .kind(PreUpdate)  // Runs first
    .each([](NetworkComponent& net) {
        // Process network input
    });

ecs.system<Position, Velocity>()
    .kind(OnUpdate)  // Runs second
    .each([](Position& pos, Velocity& vel) {
        // Update positions
    });

ecs.system<Position, NetworkComponent>()
    .kind(PostUpdate)  // Runs last
    .each([](const Position& pos, NetworkComponent& net) {
        // Send state updates
    });
```

### 3. Reflection and Serialization

**Component Reflection:**

```cpp
// Register component with metadata
ecs.component<Position>()
    .member<float>("x")
    .member<float>("y")
    .member<float>("z");

ecs.component<Health>()
    .member<int>("current")
    .member<int>("maximum");

// Serialize entity to JSON
auto player = ecs.entity("Player")
    .set<Position>({10.0f, 20.0f, 30.0f})
    .set<Health>({80, 100});

std::string json = player.to_json();
// Output: {"Position":{"x":10,"y":20,"z":30},"Health":{"current":80,"maximum":100}}

// Deserialize from JSON
auto loaded_player = ecs.entity().from_json(json.c_str());
```

**Entity Inspection:**

```cpp
// Iterate all components on an entity
player.each([](flecs::id id) {
    std::cout << "Component: " << id.str() << std::endl;
});

// Get component value as string (for debugging)
auto health = player.get<Health>();
std::cout << "Health: " << health->current << "/" << health->maximum << std::endl;
```

### 4. Observers and Triggers

**Reacting to Entity Changes:**

```cpp
// Observer for component addition
ecs.observer<Health>()
    .event(flecs::OnAdd)
    .each([](flecs::entity e, Health& health) {
        std::cout << "Entity " << e.name() << " gained health" << std::endl;
    });

// Observer for component removal
ecs.observer<Health>()
    .event(flecs::OnRemove)
    .each([](flecs::entity e, Health& health) {
        std::cout << "Entity " << e.name() << " lost health" << std::endl;
    });

// Observer for component modification
ecs.observer<Health>()
    .event(flecs::OnSet)
    .each([](flecs::entity e, Health& health) {
        if (health.current <= 0) {
            e.add<DeadTag>();
        }
    });

// Observer for entity relationships
ecs.observer<MemberOf>()
    .event(flecs::OnAdd)
    .each([](flecs::entity e, MemberOf& membership) {
        std::cout << e.name() << " joined guild" << std::endl;
    });
```

---

## Part III: BlueMarble-Specific Implementation

### 1. Regional Server Architecture with flecs

```cpp
class ServerRegion {
private:
    flecs::world ecs;
    RegionId regionId;
    
public:
    ServerRegion(RegionId id) : regionId(id) {
        InitializeModules();
        InitializeSystems();
        InitializeObservers();
    }
    
    void InitializeModules() {
        // Import core modules
        ecs.import<TransformModule>();
        ecs.import<PhysicsModule>();
        ecs.import<CombatModule>();
        ecs.import<NetworkModule>();
        ecs.import<GeologyModule>();
    }
    
    void InitializeSystems() {
        // Movement system
        ecs.system<Position, const Velocity>()
            .kind(flecs::OnUpdate)
            .each([](flecs::entity e, Position& pos, const Velocity& vel) {
                float dt = e.delta_time();
                pos.x += vel.dx * dt;
                pos.y += vel.dy * dt;
                pos.z += vel.dz * dt;
            });
        
        // Combat system
        ecs.system<CombatState, const Position>()
            .with<PlayerTag>()
            .each([](flecs::entity e, CombatState& combat, const Position& pos) {
                if (combat.target) {
                    auto target = e.world().entity(combat.target.value());
                    if (target.is_alive()) {
                        const Position* target_pos = target.get<Position>();
                        float distance = Distance(pos, *target_pos);
                        
                        if (distance <= combat.range && combat.cooldown <= 0) {
                            // Deal damage
                            Health* target_health = target.get_mut<Health>();
                            target_health->current -= combat.damage;
                            combat.cooldown = combat.attackSpeed;
                        }
                    }
                }
                combat.cooldown -= e.delta_time();
            });
        
        // Replication system
        ecs.system<const Position, NetworkComponent>()
            .with<DirtyTag>()
            .each([](flecs::entity e, const Position& pos, NetworkComponent& net) {
                // Send position update to clients
                SendPositionUpdate(net.playerId, pos);
                e.remove<DirtyTag>();
            });
    }
    
    void InitializeObservers() {
        // Mark entities dirty when position changes
        ecs.observer<Position>()
            .event(flecs::OnSet)
            .with<NetworkComponent>()
            .each([](flecs::entity e, Position& pos) {
                e.add<DirtyTag>();
            });
        
        // Handle entity death
        ecs.observer<Health>()
            .event(flecs::OnSet)
            .each([](flecs::entity e, Health& health) {
                if (health.current <= 0) {
                    e.add<DeadTag>();
                    // Trigger death event
                    TriggerDeathEvent(e);
                }
            });
    }
    
    void Update(float deltaTime) {
        ecs.progress(deltaTime);
    }
    
    flecs::entity CreatePlayer(PlayerId playerId) {
        auto player = ecs.entity()
            .add<PlayerTag>()
            .set<NetworkComponent>({playerId})
            .set<Position>({0.0f, 0.0f, 0.0f})
            .set<Velocity>({0.0f, 0.0f, 0.0f})
            .set<Health>({100, 100})
            .set<Stamina>({100, 100})
            .set<Inventory>()
            .set<Equipment>();
        
        return player;
    }
};
```

### 2. Guild and Faction System with Relationships

```cpp
struct MemberOf {};
struct LeaderOf {};
struct OfficerOf {};

class GuildSystem {
private:
    flecs::world& ecs;
    
public:
    GuildSystem(flecs::world& world) : ecs(world) {
        InitializeGuildSystems();
    }
    
    flecs::entity CreateGuild(const std::string& name, flecs::entity leader) {
        auto guild = ecs.entity(name.c_str())
            .set<GuildData>({name, 0, 50})  // name, level, max_members
            .add<GuildTag>();
        
        // Set leader relationship
        leader.add<LeaderOf>(guild);
        leader.add<MemberOf>(guild);
        
        return guild;
    }
    
    void AddMember(flecs::entity player, flecs::entity guild) {
        // Check capacity
        auto guild_data = guild.get<GuildData>();
        auto member_count = CountGuildMembers(guild);
        
        if (member_count < guild_data->max_members) {
            player.add<MemberOf>(guild);
        }
    }
    
    int CountGuildMembers(flecs::entity guild) {
        int count = 0;
        auto query = ecs.query_builder<>()
            .with<MemberOf>(guild)
            .build();
        
        query.each([&count](flecs::entity e) {
            count++;
        });
        
        return count;
    }
    
    void BroadcastToGuild(flecs::entity guild, const std::string& message) {
        auto members = ecs.query_builder<NetworkComponent>()
            .with<MemberOf>(guild)
            .build();
        
        members.each([&message](flecs::entity e, NetworkComponent& net) {
            SendChatMessage(net.playerId, message);
        });
    }
    
    void InitializeGuildSystems() {
        // Guild buff system
        ecs.system<Health>()
            .with<MemberOf>(flecs::Wildcard)
            .each([](flecs::entity e, Health& health) {
                // Apply guild bonuses
                health.maximum += 10;  // +10 max health for guild members
            });
    }
};
```

### 3. Hierarchical World Structure

```cpp
class WorldHierarchy {
private:
    flecs::world& ecs;
    flecs::entity worldRoot;
    
public:
    WorldHierarchy(flecs::world& world) : ecs(world) {
        InitializeWorldStructure();
    }
    
    void InitializeWorldStructure() {
        // Create world hierarchy
        worldRoot = ecs.entity("World").add<WorldTag>();
        
        // Continents
        auto north = ecs.entity("NorthernContinent")
            .child_of(worldRoot)
            .set<Bounds>({0, 0, 1000, 1000});
        
        auto south = ecs.entity("SouthernContinent")
            .child_of(worldRoot)
            .set<Bounds>({0, 1000, 1000, 2000});
        
        // Regions in Northern Continent
        auto forest = ecs.entity("ForestRegion")
            .child_of(north)
            .set<Bounds>({0, 0, 500, 500})
            .set<BiomeData>({BiomeType::Forest});
        
        auto mountains = ecs.entity("MountainRegion")
            .child_of(north)
            .set<Bounds>({500, 0, 1000, 500})
            .set<BiomeData>({BiomeType::Mountains});
        
        // Zones in Forest Region
        auto darkForest = ecs.entity("DarkForest")
            .child_of(forest)
            .set<Bounds>({0, 0, 250, 250})
            .set<DangerLevel>({5});
        
        auto peacefulGrove = ecs.entity("PeacefulGrove")
            .child_of(forest)
            .set<Bounds>({250, 0, 500, 250})
            .set<DangerLevel>({1});
    }
    
    flecs::entity GetRegionForPosition(const Position& pos) {
        auto regions = ecs.query_builder<const Bounds>()
            .with(flecs::ChildOf, worldRoot).cascade()
            .build();
        
        flecs::entity result;
        regions.each([&](flecs::entity e, const Bounds& bounds) {
            if (pos.x >= bounds.min_x && pos.x <= bounds.max_x &&
                pos.z >= bounds.min_z && pos.z <= bounds.max_z) {
                result = e;
            }
        });
        
        return result;
    }
    
    void MoveEntityToRegion(flecs::entity entity, flecs::entity region) {
        // Remove from current region
        entity.remove(flecs::ChildOf, flecs::Wildcard);
        
        // Add to new region
        entity.child_of(region);
    }
    
    std::vector<flecs::entity> GetEntitiesInRegion(flecs::entity region) {
        std::vector<flecs::entity> entities;
        
        auto query = ecs.query_builder<Position>()
            .with(flecs::ChildOf, region)
            .build();
        
        query.each([&entities](flecs::entity e, Position& pos) {
            entities.push_back(e);
        });
        
        return entities;
    }
};
```

### 4. Query-Based AI System

```cpp
class AISystem {
private:
    flecs::world& ecs;
    
public:
    AISystem(flecs::world& world) : ecs(world) {
        InitializeAI();
    }
    
    void InitializeAI() {
        // NPC idle behavior
        ecs.system<Position, AIState>()
            .with<NPCTag>()
            .each([this](flecs::entity e, Position& pos, AIState& ai) {
                if (ai.currentState == AIState::Idle) {
                    // Look for nearby enemies
                    auto enemy = FindNearestEnemy(e, pos, 20.0f);
                    if (enemy.is_alive()) {
                        ai.currentState = AIState::Combat;
                        ai.target = enemy;
                    } else {
                        // Wander after idle timeout
                        ai.stateTimer -= e.delta_time();
                        if (ai.stateTimer <= 0) {
                            ai.currentState = AIState::Wander;
                        }
                    }
                }
            });
        
        // NPC combat behavior
        ecs.system<Position, AIState, CombatState>()
            .with<NPCTag>()
            .each([](flecs::entity e, Position& pos, AIState& ai, CombatState& combat) {
                if (ai.currentState == AIState::Combat && ai.target) {
                    auto target = e.world().entity(ai.target.value());
                    if (!target.is_alive()) {
                        ai.currentState = AIState::Idle;
                        ai.target = std::nullopt;
                        combat.target = std::nullopt;
                        return;
                    }
                    
                    const Position* target_pos = target.get<Position>();
                    float distance = Distance(pos, *target_pos);
                    
                    if (distance > 50.0f) {
                        // Target too far, give up
                        ai.currentState = AIState::Idle;
                        ai.target = std::nullopt;
                        combat.target = std::nullopt;
                    } else if (distance > combat.range) {
                        // Move towards target
                        auto direction = Normalize(Vector3{
                            target_pos->x - pos.x,
                            target_pos->y - pos.y,
                            target_pos->z - pos.z
                        });
                        e.set<Velocity>({direction.x * 5.0f, direction.y * 5.0f, direction.z * 5.0f});
                    } else {
                        // In range, stop and attack
                        e.set<Velocity>({0, 0, 0});
                        combat.target = target;
                    }
                }
            });
    }
    
    flecs::entity FindNearestEnemy(flecs::entity npc, const Position& npc_pos, float range) {
        flecs::entity nearest;
        float nearest_distance = range;
        
        // Query for players (enemies of NPCs)
        auto players = ecs.query_builder<const Position, const Health>()
            .with<PlayerTag>()
            .without<DeadTag>()
            .build();
        
        players.each([&](flecs::entity e, const Position& pos, const Health& health) {
            float distance = Distance(npc_pos, pos);
            if (distance < nearest_distance) {
                nearest_distance = distance;
                nearest = e;
            }
        });
        
        return nearest;
    }
};
```

---

## Part IV: Performance Characteristics and Comparison

### 1. Performance Profile

**Iteration Performance (approximate):**
```
Query iteration (single component): ~50-100ns per entity
Query iteration (2 components): ~80-150ns per entity
Query with relationships: ~100-200ns per entity

For 10,000 entities:
Simple query: 500-1500 microseconds
Complex query with relationships: 1000-2000 microseconds
```

**Comparison with EnTT:**
- EnTT is ~5-10x faster for simple component iteration
- flecs adds ~50-100ns overhead for relationship tracking
- flecs queries are more flexible but slower than EnTT views

### 2. Memory Overhead

**Memory per Entity:**
- Entity ID: 8 bytes (64-bit)
- Component storage: Archetype-based (grouped by component combination)
- Relationship storage: Additional tables for each relationship type
- Overhead: ~20-30% more than EnTT due to relationship tracking

**Example for 10,000 entities:**
```
Entities: 10,000 × 8 bytes = 80 KB

Components (Position, Velocity, Health):
- Position: 10,000 × 16 bytes = 160 KB
- Velocity: 10,000 × 12 bytes = 120 KB
- Health: 10,000 × 8 bytes = 80 KB

Relationship tables: ~50-100 KB (depends on relationship count)

Total: ~490-540 KB (vs ~400 KB for EnTT)
```

### 3. Feature Comparison

| Feature | flecs | EnTT |
|---------|-------|------|
| Raw performance | Slower | Faster |
| Entity relationships | ✅ Native | ❌ Manual |
| Hierarchies | ✅ Built-in | ❌ Manual |
| Query DSL | ✅ Advanced | ⚠️ Basic |
| Reflection | ✅ Built-in | ❌ None |
| Serialization | ✅ JSON/Binary | ❌ Manual |
| C API | ✅ Yes | ❌ C++ only |
| Observers | ✅ Built-in | ✅ Built-in |
| Modules | ✅ Yes | ❌ Manual |

---

## Part V: Integration Recommendations for BlueMarble

### 1. Use Cases for flecs

**Recommended for:**
- Guild and faction management systems
- World hierarchy (continent -> region -> zone)
- Ownership and item systems
- Quest systems with complex entity relationships
- Admin tools and debugging (reflection)
- Complex AI requiring relationship queries

**Not recommended for:**
- Core movement/physics systems (use EnTT)
- High-frequency combat calculations (use EnTT)
- Network replication hot path (use EnTT)

### 2. Hybrid Approach

**Best Strategy: Use Both Libraries**

```cpp
class BlueMarbleServer {
private:
    // EnTT for performance-critical systems
    entt::registry coreRegistry;
    
    // flecs for relationship-heavy systems
    flecs::world relationshipWorld;
    
public:
    void Update(float deltaTime) {
        // Phase 1: Core systems with EnTT (fast)
        UpdatePhysics(coreRegistry, deltaTime);
        UpdateMovement(coreRegistry, deltaTime);
        UpdateCombat(coreRegistry, deltaTime);
        
        // Phase 2: Social systems with flecs (flexible)
        relationshipWorld.progress(deltaTime);
        
        // Phase 3: Sync between systems (when needed)
        SyncEntityStates();
    }
};
```

### 3. Build Integration

**CMakeLists.txt:**
```cmake
# Add flecs
include(FetchContent)

FetchContent_Declare(
    flecs
    GIT_REPOSITORY https://github.com/SanderMertens/flecs.git
    GIT_TAG v4.0.0  # Use latest stable version
)

FetchContent_MakeAvailable(flecs)

# Link to your target
target_link_libraries(BlueMarbleServer PRIVATE flecs::flecs_static)
```

### 4. Migration Path

**Phase 1: Prototype (Weeks 1-2)**
- Integrate flecs alongside EnTT
- Implement guild system with relationships
- Test relationship queries and performance

**Phase 2: Expand (Weeks 3-4)**
- Add world hierarchy system
- Implement ownership system (players own items/buildings)
- Add faction relationship system

**Phase 3: Optimize (Weeks 5-6)**
- Profile and optimize query performance
- Decide which systems stay in flecs vs. move to EnTT
- Implement synchronization between systems

**Phase 4: Production (Weeks 7-8)**
- Load test with 1,000+ guilds and relationships
- Benchmark query performance under load
- Finalize architecture decisions

---

## References and Further Reading

### Primary Source
- **GitHub**: https://github.com/SanderMertens/flecs
- **Documentation**: https://www.flecs.dev/flecs/
- **License**: MIT License

### Related Documentation
- Query Manual: https://www.flecs.dev/flecs/md_docs_2Queries.html
- Relationships: https://www.flecs.dev/flecs/md_docs_2Relationships.html
- Systems: https://www.flecs.dev/flecs/md_docs_2Systems.html
- Examples: https://github.com/SanderMertens/flecs/tree/master/examples

### Related BlueMarble Research
- [Game Programming Patterns Analysis](game-dev-analysis-game-programming-patterns.md)
- [EnTT ECS Library Analysis](game-dev-analysis-entt-ecs-library.md)

### Community Resources
- Discord: https://discord.gg/BEzP5Rgrrp
- Twitter: @ajmmertens (library author)

---

**Document Status:** ✅ Complete  
**Next Steps:**
- Prototype guild system with flecs relationships
- Compare performance with EnTT-based implementation
- Decision point: Use flecs for social systems or implement relationships manually in EnTT

**Related Assignments:**
- Discovered from: Research Assignment Group 27, Topic 1 (Game Programming Patterns)
- Part of: Phase 1 Extension - Implementation Library Research

**Implementation Priority:** Medium - Recommended for relationship-heavy systems after EnTT core is stable
