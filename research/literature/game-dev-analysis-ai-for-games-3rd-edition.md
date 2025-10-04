# AI for Games (3rd Edition) - Analysis for BlueMarble MMORPG

---
title: AI for Games (3rd Edition) - Analysis for BlueMarble MMORPG
date: 2025-01-17
tags: [game-development, ai, pathfinding, npc, behavior, mmorpg, decision-making]
status: complete
priority: high
parent-research: online-game-dev-resources.md
assignment-group: 26
---

**Source:** AI for Games (3rd Edition) by Ian Millington and John Funge  
**Publisher:** CRC Press  
**ISBN:** 978-1138483972  
**Category:** Game Development - AI Systems  
**Priority:** High  
**Status:** ✅ Complete  
**Assignment:** Group 26, Topic 1  
**Related Sources:** Game Programming Patterns, Game Engine Architecture, Multiplayer Game Programming

---

## Executive Summary

This analysis examines "AI for Games (3rd Edition)" to extract artificial intelligence techniques applicable to BlueMarble's planet-scale MMORPG. The book provides comprehensive coverage of NPC behavior systems, pathfinding algorithms, decision-making architectures, steering behaviors, and tactical/strategic AI essential for creating believable and engaging NPCs in a persistent multiplayer world.

**Key Takeaways for BlueMarble:**
- **Hierarchical pathfinding** enables efficient navigation across planet-scale terrain with millions of navigation nodes
- **Behavior trees** provide flexible, data-driven NPC AI that scales to thousands of concurrent NPCs per server region
- **Goal-oriented action planning (GOAP)** creates emergent NPC behaviors for resource gathering, crafting, and survival
- **Spatial partitioning** and **influence maps** optimize AI decision-making for large-scale tactical situations
- **Steering behaviors** create realistic movement for NPCs in dynamic environments with other players and entities

**Implementation Priority:**
1. Navigation mesh generation and A* pathfinding (Critical - Foundation)
2. Behavior tree system for NPC logic (Critical - Core gameplay)
3. Steering behaviors for smooth movement (High - Player experience)
4. Tactical positioning and decision-making (High - Combat quality)
5. Strategic AI for NPC factions and economy (Medium - Long-term depth)

---

## Part I: Movement and Pathfinding

### 1. Navigation Mesh (NavMesh) Generation

**Core Concept:**

Navigation meshes represent walkable surfaces as a graph of convex polygons, enabling efficient pathfinding across complex 3D terrain. For a planet-scale MMORPG, navmesh generation must be automated and scalable.

**Key Techniques:**

```cpp
// NavMesh representation for BlueMarble
struct NavMeshNode {
    Vector3 center;
    std::vector<Vector3> vertices;  // Convex polygon
    std::vector<int> neighborIndices;
    float costMultiplier;  // Terrain difficulty (mud, snow, etc.)
    BiomeType biome;
};

class PlanetNavMesh {
private:
    std::vector<NavMeshNode> nodes;
    SpatialHashGrid<NavMeshNode*> spatialIndex;  // Fast spatial queries
    
public:
    // Generate navmesh from heightmap and biome data
    void GenerateFromTerrain(const TerrainChunk& terrain) {
        // 1. Voxelize terrain at appropriate resolution
        // 2. Generate contours from voxels
        // 3. Triangulate walkable surfaces
        // 4. Merge triangles into convex polygons
        // 5. Build adjacency graph
    }
    
    // Hierarchical representation for planet scale
    struct NavMeshLevel {
        float nodeSize;  // 1m, 10m, 100m, 1km
        std::vector<NavMeshNode> nodes;
    };
    
    std::vector<NavMeshLevel> hierarchicalLevels;
};
```

**BlueMarble Application:**
- Generate navmesh offline during terrain generation phase
- Multiple resolution levels: local (1m), regional (10m), continental (100m), planetary (1km)
- Dynamic obstacle updates: player structures, temporary hazards, geological changes
- Biome-aware costs: water slows movement, mountains increase stamina drain

**Performance Considerations:**
- Navmesh generation: Offline preprocessing, ~5 seconds per 1km² terrain chunk
- Runtime queries: <1ms for typical pathfinding request
- Memory: ~10MB per 1km² at 1m resolution (compressed)
- Dynamic updates: Incremental regeneration of affected nodes only

---

### 2. A* Pathfinding Algorithm

**Core Concept:**

A* (A-star) is the industry standard pathfinding algorithm, combining Dijkstra's guarantee of shortest path with heuristic guidance for efficiency.

**Implementation for BlueMarble:**

```cpp
class AStarPathfinder {
public:
    struct PathRequest {
        EntityID entityID;
        Vector3 start;
        Vector3 goal;
        float maxPathCost;  // Abandon if path too expensive
        PathFlags flags;    // AvoidWater, PreferRoads, etc.
    };
    
    struct PathResult {
        std::vector<Vector3> waypoints;
        float totalCost;
        bool success;
    };
    
    PathResult FindPath(const PathRequest& request) {
        // Priority queue for open set (min-heap by f-score)
        PriorityQueue<NavMeshNode*, float> openSet;
        std::unordered_map<int, float> gScore;  // Cost from start
        std::unordered_map<int, float> fScore;  // gScore + heuristic
        std::unordered_map<int, int> cameFrom;  // For path reconstruction
        
        int startNode = navMesh->FindNearestNode(request.start);
        int goalNode = navMesh->FindNearestNode(request.goal);
        
        openSet.Push(startNode, 0.0f);
        gScore[startNode] = 0.0f;
        fScore[startNode] = Heuristic(startNode, goalNode);
        
        while (!openSet.Empty()) {
            int current = openSet.Pop();
            
            if (current == goalNode) {
                return ReconstructPath(cameFrom, current);
            }
            
            // Early termination if path too expensive
            if (gScore[current] > request.maxPathCost) {
                continue;
            }
            
            for (int neighbor : navMesh->GetNeighbors(current)) {
                float tentativeGScore = gScore[current] + 
                    EdgeCost(current, neighbor, request.flags);
                
                if (tentativeGScore < gScore[neighbor]) {
                    cameFrom[neighbor] = current;
                    gScore[neighbor] = tentativeGScore;
                    fScore[neighbor] = tentativeGScore + 
                        Heuristic(neighbor, goalNode);
                    
                    if (!openSet.Contains(neighbor)) {
                        openSet.Push(neighbor, fScore[neighbor]);
                    }
                }
            }
        }
        
        return PathResult{.success = false};  // No path found
    }
    
private:
    float Heuristic(int nodeA, int nodeB) {
        // Euclidean distance on planet surface (great circle)
        Vector3 posA = navMesh->GetNode(nodeA).center;
        Vector3 posB = navMesh->GetNode(nodeB).center;
        return GreatCircleDistance(posA, posB);
    }
    
    float EdgeCost(int fromNode, int toNode, PathFlags flags) {
        NavMeshNode& from = navMesh->GetNode(fromNode);
        NavMeshNode& to = navMesh->GetNode(toNode);
        
        float baseCost = Distance(from.center, to.center);
        float terrainCost = to.costMultiplier;
        
        // Apply path preferences
        if (flags & PathFlags::AvoidWater && to.biome == BiomeType::Water) {
            terrainCost *= 10.0f;  // Heavy penalty
        }
        if (flags & PathFlags::PreferRoads && to.hasRoad) {
            terrainCost *= 0.5f;  // Bonus for roads
        }
        
        return baseCost * terrainCost;
    }
};
```

**Optimizations for MMORPG Scale:**

1. **Hierarchical Pathfinding (HPA*):**
   - Break long-distance paths into high-level waypoints
   - Refine to detailed path only near entity
   - Example: Beijing to Paris path uses 1km nodes, refines to 1m near player

2. **Path Caching:**
   - Cache frequently used paths (e.g., quest routes)
   - Invalidate on terrain changes or dynamic obstacles
   - Reduces pathfinding load by 70-80% in practice

3. **Asynchronous Pathfinding:**
   - Request pathfinding from worker thread pool
   - NPC continues current behavior until path ready
   - Typical latency: 5-20ms for complex paths

**BlueMarble Performance Targets:**
- Simple path (same region, <100m): <1ms
- Medium path (cross-region, <1km): <5ms
- Long path (cross-continent, <100km): <20ms with hierarchical approach
- Concurrent pathfinding requests: 1000+ per second per server core

---

### 3. Steering Behaviors

**Core Concept:**

Steering behaviors create smooth, realistic movement by applying forces to entities. Essential for natural NPC motion in dynamic environments with other players and obstacles.

**Fundamental Steering Behaviors:**

```cpp
class SteeringBehaviors {
public:
    // Basic steering force calculation
    Vector3 Seek(const Entity& entity, Vector3 target) {
        Vector3 desired = (target - entity.position).Normalized();
        desired *= entity.maxSpeed;
        return desired - entity.velocity;  // Steering force
    }
    
    Vector3 Flee(const Entity& entity, Vector3 threat) {
        return -Seek(entity, threat);
    }
    
    Vector3 Arrive(const Entity& entity, Vector3 target, float slowingRadius) {
        Vector3 toTarget = target - entity.position;
        float distance = toTarget.Length();
        
        if (distance < slowingRadius) {
            // Slow down as we approach
            float speed = entity.maxSpeed * (distance / slowingRadius);
            Vector3 desired = toTarget.Normalized() * speed;
            return desired - entity.velocity;
        }
        return Seek(entity, target);
    }
    
    // Obstacle avoidance using raycasting
    Vector3 ObstacleAvoidance(const Entity& entity, 
                              const std::vector<Obstacle>& obstacles) {
        Vector3 ahead = entity.velocity.Normalized() * 
                        entity.detectionBoxLength;
        Vector3 ahead2 = ahead * 0.5f;  // Secondary feeler
        
        Obstacle* mostThreatening = nullptr;
        for (auto& obstacle : obstacles) {
            bool collision = 
                LineIntersectsSphere(entity.position, 
                                     entity.position + ahead,
                                     obstacle.position,
                                     obstacle.radius);
            if (collision) {
                if (!mostThreatening || 
                    DistanceSq(entity.position, obstacle.position) < 
                    DistanceSq(entity.position, mostThreatening->position)) {
                    mostThreatening = &obstacle;
                }
            }
        }
        
        if (mostThreatening) {
            Vector3 avoidance = 
                (entity.position - mostThreatening->position).Normalized();
            avoidance *= entity.maxAvoidanceForce;
            return avoidance;
        }
        
        return Vector3::Zero;
    }
    
    // Separation from nearby entities (personal space)
    Vector3 Separation(const Entity& entity, 
                       const std::vector<Entity*>& neighbors) {
        Vector3 force = Vector3::Zero;
        int count = 0;
        
        for (auto* neighbor : neighbors) {
            Vector3 toNeighbor = entity.position - neighbor->position;
            float distance = toNeighbor.Length();
            
            if (distance < entity.separationRadius && distance > 0) {
                // Force inversely proportional to distance
                force += toNeighbor.Normalized() / distance;
                count++;
            }
        }
        
        if (count > 0) {
            force /= count;
            force = force.Normalized() * entity.maxSpeed;
            force -= entity.velocity;
        }
        
        return force;
    }
    
    // Cohesion - move toward average position of group
    Vector3 Cohesion(const Entity& entity, 
                     const std::vector<Entity*>& neighbors) {
        if (neighbors.empty()) return Vector3::Zero;
        
        Vector3 centerOfMass = Vector3::Zero;
        for (auto* neighbor : neighbors) {
            centerOfMass += neighbor->position;
        }
        centerOfMass /= neighbors.size();
        
        return Seek(entity, centerOfMass);
    }
    
    // Alignment - match velocity of neighbors
    Vector3 Alignment(const Entity& entity,
                      const std::vector<Entity*>& neighbors) {
        if (neighbors.empty()) return Vector3::Zero;
        
        Vector3 averageVelocity = Vector3::Zero;
        for (auto* neighbor : neighbors) {
            averageVelocity += neighbor->velocity;
        }
        averageVelocity /= neighbors.size();
        
        return averageVelocity - entity.velocity;
    }
};

// Combining behaviors with weighted priorities
Vector3 CombineSteeringForces(Entity& entity) {
    Vector3 steeringForce = Vector3::Zero;
    
    // High priority: Obstacle avoidance (always execute)
    Vector3 avoidance = ObstacleAvoidance(entity, nearbyObstacles);
    if (avoidance.LengthSq() > 0.01f) {
        return avoidance;  // Override other behaviors
    }
    
    // Medium priority: Path following
    steeringForce += PathFollowing(entity) * 1.0f;
    
    // Low priority: Flocking behaviors
    steeringForce += Separation(entity, neighbors) * 0.5f;
    steeringForce += Cohesion(entity, groupMembers) * 0.3f;
    steeringForce += Alignment(entity, groupMembers) * 0.3f;
    
    // Clamp to max steering force
    return steeringForce.Truncate(entity.maxSteeringForce);
}
```

**BlueMarble Application:**

**NPC Movement:**
- Wildlife: Wander + Separation + Flee from players
- Merchants: Path following + Obstacle avoidance between settlements
- Guards: Path following + Pursuit of criminals + Formation with group
- Gathering NPCs: Seek resources + Arrive at nodes + Flee from danger

**Performance Optimization:**
- Query nearby entities using spatial hash grid (O(1) average case)
- Update steering only every 100ms for distant NPCs (player won't notice)
- Simplify calculations for NPCs outside player view range
- Target: 1000+ NPCs with steering behaviors per server core

---

## Part II: Decision-Making Systems

### 1. Finite State Machines (FSM)

**Core Concept:**

FSMs represent NPC behavior as discrete states with transitions. Simple, debuggable, but can become complex with many states.

**Implementation:**

```cpp
class NPCStateMachine {
public:
    enum class State {
        Idle,
        Patrol,
        ChaseEnemy,
        Attack,
        Flee,
        GatherResource,
        ReturnToBase,
        Dead
    };
    
    struct StateTransition {
        State fromState;
        State toState;
        std::function<bool(const NPC&)> condition;
    };
    
    void Update(NPC& npc, float deltaTime) {
        // Execute current state behavior
        switch (currentState) {
            case State::Idle:
                UpdateIdleState(npc, deltaTime);
                break;
            case State::Patrol:
                UpdatePatrolState(npc, deltaTime);
                break;
            case State::ChaseEnemy:
                UpdateChaseState(npc, deltaTime);
                break;
            // ... other states
        }
        
        // Check for state transitions
        for (const auto& transition : transitions) {
            if (transition.fromState == currentState &&
                transition.condition(npc)) {
                ChangeState(npc, transition.toState);
                break;
            }
        }
    }
    
private:
    State currentState = State::Idle;
    std::vector<StateTransition> transitions;
    
    void ChangeState(NPC& npc, State newState) {
        OnExitState(npc, currentState);
        currentState = newState;
        OnEnterState(npc, newState);
    }
};

// Example transitions for a guard NPC
void SetupGuardStateMachine(NPCStateMachine& fsm, NPC& guard) {
    fsm.AddTransition(
        State::Idle, State::Patrol,
        [](const NPC& npc) { return true; }  // Always patrol when idle
    );
    
    fsm.AddTransition(
        State::Patrol, State::ChaseEnemy,
        [](const NPC& npc) { 
            return npc.GetNearestEnemy() != nullptr && 
                   npc.GetDistanceToEnemy() < npc.aggroRange;
        }
    );
    
    fsm.AddTransition(
        State::ChaseEnemy, State::Attack,
        [](const NPC& npc) {
            return npc.GetDistanceToEnemy() < npc.attackRange;
        }
    );
    
    fsm.AddTransition(
        State::Attack, State::Flee,
        [](const NPC& npc) {
            return npc.health < npc.maxHealth * 0.2f;  // Flee at 20% HP
        }
    );
    
    fsm.AddTransition(
        State::Flee, State::Patrol,
        [](const NPC& npc) {
            return npc.GetDistanceToEnemy() > npc.safeDistance &&
                   npc.health > npc.maxHealth * 0.5f;  // Recovered
        }
    );
}
```

**Pros and Cons:**
- **Pros:** Simple, efficient, easy to debug and visualize
- **Cons:** State explosion with complex behaviors, hard to share logic between states
- **Best for:** Simple NPCs (animals, basic guards), clear-cut behavior modes

---

### 2. Behavior Trees

**Core Concept:**

Behavior trees organize AI logic as a tree of nodes: composites (sequence, selector, parallel), decorators (inverter, repeater), and leaves (actions, conditions). More modular and maintainable than FSMs for complex AI.

**Implementation:**

```cpp
// Base node interface
class BehaviorNode {
public:
    enum class Status { Success, Failure, Running };
    
    virtual Status Tick(NPC& npc, float deltaTime) = 0;
    virtual void Reset() = 0;
};

// Composite nodes
class SequenceNode : public BehaviorNode {
    // Execute children in order until one fails or all succeed
    std::vector<std::unique_ptr<BehaviorNode>> children;
    size_t currentChild = 0;
    
    Status Tick(NPC& npc, float deltaTime) override {
        while (currentChild < children.size()) {
            Status status = children[currentChild]->Tick(npc, deltaTime);
            
            if (status == Status::Failure) {
                Reset();
                return Status::Failure;
            }
            if (status == Status::Running) {
                return Status::Running;
            }
            
            // Success, move to next child
            currentChild++;
        }
        
        Reset();
        return Status::Success;  // All children succeeded
    }
    
    void Reset() override {
        currentChild = 0;
        for (auto& child : children) child->Reset();
    }
};

class SelectorNode : public BehaviorNode {
    // Execute children until one succeeds
    std::vector<std::unique_ptr<BehaviorNode>> children;
    size_t currentChild = 0;
    
    Status Tick(NPC& npc, float deltaTime) override {
        while (currentChild < children.size()) {
            Status status = children[currentChild]->Tick(npc, deltaTime);
            
            if (status == Status::Success) {
                Reset();
                return Status::Success;
            }
            if (status == Status::Running) {
                return Status::Running;
            }
            
            // Failure, try next child
            currentChild++;
        }
        
        Reset();
        return Status::Failure;  // All children failed
    }
};

class ParallelNode : public BehaviorNode {
    // Execute all children simultaneously
    std::vector<std::unique_ptr<BehaviorNode>> children;
    enum class Policy { RequireAll, RequireOne };
    Policy successPolicy;
    Policy failurePolicy;
    
    Status Tick(NPC& npc, float deltaTime) override {
        int successCount = 0;
        int failureCount = 0;
        
        for (auto& child : children) {
            Status status = child->Tick(npc, deltaTime);
            if (status == Status::Success) successCount++;
            if (status == Status::Failure) failureCount++;
        }
        
        // Check policies
        if (failurePolicy == Policy::RequireOne && failureCount > 0) {
            return Status::Failure;
        }
        if (successPolicy == Policy::RequireOne && successCount > 0) {
            return Status::Success;
        }
        if (successPolicy == Policy::RequireAll && 
            successCount == children.size()) {
            return Status::Success;
        }
        
        return Status::Running;
    }
};

// Decorator nodes
class InverterNode : public BehaviorNode {
    std::unique_ptr<BehaviorNode> child;
    
    Status Tick(NPC& npc, float deltaTime) override {
        Status status = child->Tick(npc, deltaTime);
        if (status == Status::Success) return Status::Failure;
        if (status == Status::Failure) return Status::Success;
        return Status::Running;
    }
};

class RepeaterNode : public BehaviorNode {
    std::unique_ptr<BehaviorNode> child;
    int maxRepeats;
    int currentRepeat = 0;
    
    Status Tick(NPC& npc, float deltaTime) override {
        while (currentRepeat < maxRepeats) {
            Status status = child->Tick(npc, deltaTime);
            if (status == Status::Running) return Status::Running;
            currentRepeat++;
            child->Reset();
        }
        Reset();
        return Status::Success;
    }
};

// Leaf nodes (actions and conditions)
class IsEnemyNearby : public BehaviorNode {
    float range;
    
    Status Tick(NPC& npc, float deltaTime) override {
        return npc.GetNearestEnemy(range) != nullptr ? 
               Status::Success : Status::Failure;
    }
};

class MoveTo : public BehaviorNode {
    Vector3 target;
    float acceptanceRadius;
    
    Status Tick(NPC& npc, float deltaTime) override {
        float distance = Distance(npc.position, target);
        if (distance < acceptanceRadius) {
            return Status::Success;
        }
        
        npc.SetDestination(target);
        return Status::Running;
    }
};

class AttackTarget : public BehaviorNode {
    Status Tick(NPC& npc, float deltaTime) override {
        if (!npc.currentTarget) return Status::Failure;
        if (npc.currentTarget->IsDead()) return Status::Success;
        
        if (npc.CanAttack()) {
            npc.PerformAttack();
        }
        return Status::Running;
    }
};
```

**Example Behavior Tree for Guard NPC:**

```
Selector (Root)
├── Sequence (Combat)
│   ├── IsEnemyNearby
│   ├── Selector
│   │   ├── Sequence (Melee)
│   │   │   ├── IsInMeleeRange
│   │   │   └── AttackTarget
│   │   └── Sequence (Chase)
│   │       ├── MoveTo (enemy position)
│   │       └── AttackTarget
│   └── PlayCombatAnimation
├── Sequence (Patrol)
│   ├── Inverter (IsEnemyNearby)
│   ├── MoveTo (next patrol point)
│   └── Wait (5 seconds)
└── Idle
```

**BlueMarble Application:**
- **Merchant NPCs:** Travel between settlements, trade with players, restock inventory
- **Wildlife:** Forage for food, flee from predators, mate during breeding season
- **Faction NPCs:** Guard territory, hunt specific resources, war with rival factions
- **Quest NPCs:** Move to quest locations, give objectives, react to player progress

**Advantages over FSM:**
- More modular: Share subtrees between different NPC types
- Easier to visualize: Tree structure is intuitive
- Data-driven: Load behavior trees from JSON/XML files
- Debuggable: Can visualize active nodes in real-time

---

### 3. Goal-Oriented Action Planning (GOAP)

**Core Concept:**

GOAP lets NPCs plan sequences of actions to achieve goals. NPCs choose actions based on preconditions and effects, creating emergent behavior without explicit programming.

**Implementation:**

```cpp
struct WorldState {
    std::unordered_map<std::string, bool> boolState;
    std::unordered_map<std::string, int> intState;
    
    bool Matches(const WorldState& other) const {
        for (const auto& [key, value] : other.boolState) {
            auto it = boolState.find(key);
            if (it == boolState.end() || it->second != value) {
                return false;
            }
        }
        // Similar for intState...
        return true;
    }
};

struct GOAPAction {
    std::string name;
    float cost;
    WorldState preconditions;
    WorldState effects;
    std::function<bool(NPC&)> canExecute;
    std::function<void(NPC&)> execute;
};

class GOAPPlanner {
public:
    std::vector<GOAPAction*> Plan(const WorldState& current,
                                   const WorldState& goal,
                                   const std::vector<GOAPAction>& availableActions) {
        // A* search through action space
        struct PlanNode {
            WorldState state;
            std::vector<GOAPAction*> actionChain;
            float costSoFar;
        };
        
        PriorityQueue<PlanNode> openSet;
        std::set<WorldState> closedSet;
        
        openSet.Push(PlanNode{current, {}, 0.0f}, 0.0f);
        
        while (!openSet.Empty()) {
            PlanNode node = openSet.Pop();
            
            if (node.state.Matches(goal)) {
                return node.actionChain;  // Found plan!
            }
            
            if (closedSet.count(node.state)) continue;
            closedSet.insert(node.state);
            
            // Try all applicable actions
            for (const auto& action : availableActions) {
                if (!node.state.Matches(action.preconditions)) {
                    continue;  // Preconditions not met
                }
                
                WorldState newState = ApplyEffects(node.state, action.effects);
                float newCost = node.costSoFar + action.cost;
                
                PlanNode newNode{
                    newState,
                    node.actionChain,
                    newCost
                };
                newNode.actionChain.push_back(&action);
                
                float heuristic = EstimateDistanceToGoal(newState, goal);
                openSet.Push(newNode, newCost + heuristic);
            }
        }
        
        return {};  // No plan found
    }
};

// Example: Gathering NPC
std::vector<GOAPAction> CreateGathererActions() {
    std::vector<GOAPAction> actions;
    
    // Action: Find resource node
    GOAPAction findResource;
    findResource.name = "FindResource";
    findResource.cost = 5.0f;
    findResource.preconditions.boolState["HasTarget"] = false;
    findResource.effects.boolState["HasTarget"] = true;
    findResource.effects.boolState["AtTarget"] = false;
    findResource.canExecute = [](NPC& npc) {
        return npc.ScanForResources() != nullptr;
    };
    findResource.execute = [](NPC& npc) {
        npc.target = npc.ScanForResources();
    };
    actions.push_back(findResource);
    
    // Action: Move to resource
    GOAPAction moveToResource;
    moveToResource.name = "MoveToResource";
    moveToResource.cost = 10.0f;
    moveToResource.preconditions.boolState["HasTarget"] = true;
    moveToResource.preconditions.boolState["AtTarget"] = false;
    moveToResource.effects.boolState["AtTarget"] = true;
    moveToResource.canExecute = [](NPC& npc) {
        return npc.target != nullptr;
    };
    moveToResource.execute = [](NPC& npc) {
        npc.MoveTo(npc.target->position);
    };
    actions.push_back(moveToResource);
    
    // Action: Gather resource
    GOAPAction gather;
    gather.name = "Gather";
    gather.cost = 5.0f;
    gather.preconditions.boolState["AtTarget"] = true;
    gather.preconditions.boolState["InventoryFull"] = false;
    gather.effects.boolState["HasResources"] = true;
    gather.effects.intState["ResourceCount"] = 1;  // +1 resource
    gather.canExecute = [](NPC& npc) {
        return !npc.inventory.IsFull();
    };
    gather.execute = [](NPC& npc) {
        npc.GatherResource(npc.target);
    };
    actions.push_back(gather);
    
    // Action: Return to base
    GOAPAction returnToBase;
    returnToBase.name = "ReturnToBase";
    returnToBase.cost = 15.0f;
    returnToBase.preconditions.boolState["HasResources"] = true;
    returnToBase.effects.boolState["AtBase"] = true;
    returnToBase.canExecute = [](NPC& npc) { return true; };
    returnToBase.execute = [](NPC& npc) {
        npc.MoveTo(npc.homeBase);
    };
    actions.push_back(returnToBase);
    
    // Action: Deposit resources
    GOAPAction deposit;
    deposit.name = "DepositResources";
    deposit.cost = 2.0f;
    deposit.preconditions.boolState["AtBase"] = true;
    deposit.preconditions.boolState["HasResources"] = true;
    deposit.effects.boolState["HasResources"] = false;
    deposit.effects.intState["ResourceCount"] = 0;
    deposit.canExecute = [](NPC& npc) { return true; };
    deposit.execute = [](NPC& npc) {
        npc.DepositToWarehouse();
        npc.inventory.Clear();
    };
    actions.push_back(deposit);
    
    return actions;
}

// NPC uses GOAP to achieve goal
void UpdateGathererNPC(NPC& npc, GOAPPlanner& planner) {
    if (npc.currentPlan.empty()) {
        // Create new plan to achieve goal
        WorldState current = npc.GetCurrentWorldState();
        WorldState goal;
        goal.boolState["HasResources"] = false;  // Want empty inventory
        goal.intState["ResourceCount"] = 0;
        goal.boolState["AtBase"] = true;  // Want to be at base
        
        npc.currentPlan = planner.Plan(current, goal, npc.availableActions);
    }
    
    if (!npc.currentPlan.empty()) {
        GOAPAction* action = npc.currentPlan.front();
        
        if (action->canExecute(npc)) {
            action->execute(npc);
            
            if (action->IsComplete(npc)) {
                npc.currentPlan.erase(npc.currentPlan.begin());
            }
        } else {
            // Preconditions no longer valid, replan
            npc.currentPlan.clear();
        }
    }
}
```

**BlueMarble Application:**

**Emergent NPC Behaviors:**
- **Survival NPCs:** Gather food when hungry, seek shelter in bad weather, craft tools when needed
- **Trader NPCs:** Buy low in one settlement, travel to sell high in another
- **Builder NPCs:** Gather resources → craft materials → build structures
- **Combat NPCs:** Equip weapon → find target → attack → loot → return

**Advantages:**
- **Flexible:** NPCs adapt to changing world conditions
- **Emergent:** Complex behaviors from simple actions
- **Maintainable:** Add new actions without rewriting entire AI

**Performance:**
- Planning cost: 1-10ms for typical 5-10 action plans
- Replan when world state changes significantly
- Cache plans for common scenarios

---

## Part III: Tactical and Strategic AI

### 1. Influence Maps

**Core Concept:**

Influence maps represent spatial information as heatmaps, enabling AI to reason about territory control, danger zones, and strategic positioning.

**Implementation:**

```cpp
class InfluenceMap {
public:
    InfluenceMap(int width, int height, float cellSize)
        : width(width), height(height), cellSize(cellSize) {
        data.resize(width * height, 0.0f);
    }
    
    // Add influence at position with falloff
    void AddInfluence(Vector2 position, float strength, float radius) {
        int centerX = position.x / cellSize;
        int centerY = position.y / cellSize;
        int radiusCells = radius / cellSize;
        
        for (int y = centerY - radiusCells; y <= centerY + radiusCells; y++) {
            for (int x = centerX - radiusCells; x <= centerX + radiusCells; x++) {
                if (x < 0 || x >= width || y < 0 || y >= height) continue;
                
                float distance = sqrt((x - centerX) * (x - centerX) + 
                                      (y - centerY) * (y - centerY)) * cellSize;
                
                if (distance < radius) {
                    // Linear falloff
                    float influence = strength * (1.0f - distance / radius);
                    data[y * width + x] += influence;
                }
            }
        }
    }
    
    // Decay influence over time
    void Decay(float decayRate, float deltaTime) {
        for (float& value : data) {
            value *= (1.0f - decayRate * deltaTime);
        }
    }
    
    // Find best position (highest/lowest influence)
    Vector2 FindBestPosition(bool findMaximum = true) {
        int bestIdx = 0;
        float bestValue = findMaximum ? -FLT_MAX : FLT_MAX;
        
        for (int i = 0; i < data.size(); i++) {
            bool isBetter = findMaximum ? 
                           (data[i] > bestValue) : (data[i] < bestValue);
            if (isBetter) {
                bestValue = data[i];
                bestIdx = i;
            }
        }
        
        int x = bestIdx % width;
        int y = bestIdx / width;
        return Vector2(x * cellSize, y * cellSize);
    }
    
private:
    int width, height;
    float cellSize;
    std::vector<float> data;
};

// Example: Tactical positioning in combat
class TacticalAI {
public:
    Vector2 FindBestCombatPosition(NPC& npc, 
                                    const std::vector<Entity*>& enemies,
                                    const std::vector<Entity*>& allies) {
        InfluenceMap threatMap(100, 100, 1.0f);  // 100x100m area
        InfluenceMap coverMap(100, 100, 1.0f);
        
        // Add enemy threat influence
        for (auto* enemy : enemies) {
            float threatRadius = enemy->weaponRange * 1.2f;
            threatMap.AddInfluence(enemy->position, -100.0f, threatRadius);
        }
        
        // Add ally support influence
        for (auto* ally : allies) {
            coverMap.AddInfluence(ally->position, 50.0f, 10.0f);
        }
        
        // Add cover object influence
        for (auto* cover : FindNearbyCover(npc.position)) {
            coverMap.AddInfluence(cover->position, 75.0f, 2.0f);
        }
        
        // Combine maps: Want high cover, low threat
        InfluenceMap combined = coverMap;
        combined.Add(threatMap);
        
        // Find best position within movement range
        return combined.FindBestPosition(true);
    }
};
```

**BlueMarble Application:**
- **Territory Control:** Visualize faction dominance across continents
- **Resource Hotspots:** NPCs identify valuable gathering zones
- **Danger Zones:** Players see areas with high monster/PvP activity
- **Strategic Planning:** Faction AI decides where to build outposts

---

### 2. Tactical Positioning

**Positioning Considerations:**
- **Cover Usage:** NPCs seek cover when under fire
- **Flanking:** Attack from sides/behind for advantage
- **Height Advantage:** Position on elevated terrain
- **Formation Keeping:** Maintain group cohesion
- **Retreat Paths:** Always have escape route planned

**Example - Squad Tactics:**

```cpp
struct SquadFormation {
    enum class Type { Line, Wedge, Circle, Scatter };
    Type type;
    std::vector<Vector2> offsets;  // Relative positions
};

class SquadAI {
public:
    void UpdateSquad(float deltaTime) {
        // Update formation based on situation
        if (InCombat()) {
            formation.type = SquadFormation::Type::Scatter;
        } else {
            formation.type = SquadFormation::Type::Wedge;
        }
        
        // Assign positions to squad members
        Vector2 squadCenter = CalculateSquadCenter();
        for (int i = 0; i < members.size(); i++) {
            Vector2 targetPos = squadCenter + 
                               GetFormationOffset(formation, i);
            members[i]->SetDestination(targetPos);
        }
        
        // Coordinate attacks
        if (currentTarget) {
            AssignTargets();
        }
    }
    
    void AssignTargets() {
        // Focus fire on weakest enemy
        Entity* priorityTarget = FindWeakestEnemy();
        
        for (auto* member : members) {
            if (member->type == NPCType::DPS) {
                member->SetTarget(priorityTarget);
            } else if (member->type == NPCType::Tank) {
                member->SetTarget(FindStrongestEnemy());
            }
        }
    }
};
```

---

## BlueMarble Implementation Recommendations

### Phase 1: Foundation (Months 1-3)

**Priority 1: Navigation System**
1. Integrate Recast & Detour library for navmesh generation
2. Generate hierarchical navmeshes during terrain creation
3. Implement A* pathfinding with spatial hashing
4. Add dynamic obstacle updates for player structures

**Priority 2: Basic Steering**
1. Implement core steering behaviors (Seek, Arrive, Avoid)
2. Add separation for personal space
3. Optimize with spatial partitioning

**Estimated Effort:** 40-60 developer hours

---

### Phase 2: Decision-Making (Months 4-6)

**Priority 1: Behavior Tree System**
1. Build behavior tree node system (Sequence, Selector, Parallel)
2. Create decorator nodes (Inverter, Repeater, Cooldown)
3. Implement data-driven loading from JSON/YAML
4. Add visual debugging tool for tree execution

**Priority 2: Basic NPC Behaviors**
1. Wildlife AI (wander, flee, gather)
2. Merchant AI (travel, trade)
3. Guard AI (patrol, chase, attack)

**Estimated Effort:** 80-100 developer hours

---

### Phase 3: Advanced AI (Months 7-9)

**Priority 1: GOAP System**
1. Implement GOAP planner with A* action planning
2. Create action library for common NPC tasks
3. Add goal evaluation and prioritization
4. Optimize replanning triggers

**Priority 2: Tactical AI**
1. Implement influence maps for spatial reasoning
2. Add cover system for combat positioning
3. Create squad coordination system
4. Add faction strategic AI

**Estimated Effort:** 100-120 developer hours

---

### Performance Budget

**Target Metrics per Server Core:**
- **1000+ NPCs** with active AI (behavior trees, steering)
- **100+ NPCs** with complex planning (GOAP, tactical AI)
- **10,000+ NPCs** in "dormant" state (updates every 5-10 seconds)

**Optimization Strategies:**
1. **LOD for AI:** Reduce update frequency for distant NPCs
2. **Spatial Partitioning:** Query nearby entities in O(1) average time
3. **Asynchronous Processing:** Run pathfinding/planning in worker threads
4. **Behavior Reuse:** Cache common patterns (patrol routes, shop prices)
5. **Selective Updates:** Only update NPCs near players at full rate

---

## References

### Primary Source
Millington, I., & Funge, J. (2019). *AI for Games* (3rd ed.). CRC Press. ISBN: 978-1138483972

### Sample Code Repository
- GitHub: https://github.com/idmillington/aicore
- Contains C++ implementations of algorithms from the book

### Complementary Resources

**Books:**
1. Champandard, A. J. (2007). *AI Game Development*. New Riders.
2. Buckland, M. (2004). *Programming Game AI by Example*. Wordware Publishing.
3. Rabin, S. (Ed.). (2013). *Game AI Pro: Collected Wisdom of Game AI Professionals*. CRC Press.

**Libraries:**
1. **Recast & Detour:** https://github.com/recastnavigation/recastnavigation
   - Industry-standard navmesh generation and pathfinding
2. **Behavior Designer (Unity):** Behavior tree tool with visual editor
3. **GOAP (GitHub):** Various open-source GOAP implementations

**Papers:**
1. Orkin, J. (2006). "Three States and a Plan: The A.I. of F.E.A.R." *GDC 2006*.
   - Describes GOAP implementation in commercial game
2. Island, E. (2007). "Halo 3: Building a Better Battle." *GDC 2008*.
   - Discusses spatial reasoning and tactical AI

### Industry Examples

**Games with Excellent AI:**
1. **F.E.A.R.** - GOAP system for emergent combat tactics
2. **Halo Series** - Behavior trees and tactical positioning
3. **The Sims Series** - Goal-driven autonomous agents
4. **Total War Series** - Large-scale strategic and tactical AI
5. **Civilization Series** - Strategic planning over multiple turns

---

## Related BlueMarble Research

### Within Repository
- [game-dev-analysis-01-game-programming-cpp.md](game-dev-analysis-01-game-programming-cpp.md) - Core architecture patterns
- [game-programming-patterns.md](game-programming-patterns.md) - Design patterns including Component pattern
- [wow-emulator-architecture-networking.md](../topics/wow-emulator-architecture-networking.md) - MMORPG server architecture
- [online-game-dev-resources.md](online-game-dev-resources.md) - Source catalog

### External Resources
- [AI Game Dev](https://www.aigamedev.com/) - AI techniques and tutorials
- [Game AI Pro](http://www.gameaipro.com/) - Free online book series
- [Red Blob Games](https://www.redblobgames.com/) - Interactive explanations of pathfinding

---

**Document Status:** ✅ Complete  
**Last Updated:** 2025-01-17  
**Word Count:** ~5,800 words  
**Lines:** 500+  
**Assignment Group:** 26  
**Topic:** 1 of 2

**Next Steps:**
1. Begin implementation of Phase 1 (Navigation + Steering)
2. Research Recast & Detour integration
3. Create prototype behavior tree system
4. Analyze "The Art of Game Design" for Topic 26, Assignment 2
