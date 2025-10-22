# Unity DOTS - ECS for Massive Agent Counts - Analysis for BlueMarble MMORPG

---
title: Unity DOTS - ECS for Massive Agent Counts - Analysis for BlueMarble MMORPG
date: 2025-01-17
tags: [game-design, ecs, dots, unity, performance, data-oriented-design, agents]
status: complete
priority: high
parent-research: research-assignment-group-45.md
---

**Source:** Unity DOTS - ECS for Massive Agent Counts  
**Publisher:** Unity Technologies  
**URL:** unity.com/dots, Unity documentation  
**Category:** GameDev-Tech - Data-Oriented Technology Stack  
**Priority:** High  
**Status:** ✅ Complete  
**Lines:** 1000+  
**Related Sources:** AI Game Programming Wisdom, Game Engine Architecture, Unity ECS/DOTS Documentation

---

## Executive Summary

Unity's Data-Oriented Technology Stack (DOTS) with Entity Component System (ECS) represents a paradigm shift in game engine architecture, moving from traditional object-oriented design to data-oriented design. DOTS enables massive performance improvements through cache-friendly data layouts, automatic multi-threading via the Job System, and SIMD optimization through the Burst compiler.

**Key Takeaways for BlueMarble:**

- **ECS fundamentals**: Entities are IDs, Components are data, Systems process components in batches
- **Performance at scale**: DOTS can handle 100,000+ entities at 60 FPS vs ~1,000 with traditional MonoBehaviours
- **Cache-friendly architecture**: Structure of Arrays (SoA) layout ensures sequential memory access
- **Automatic parallelization**: Job System distributes work across CPU cores with zero manual threading
- **Burst compilation**: Auto-vectorization (SIMD) provides 10-100x speedups for math-heavy code
- **Memory efficiency**: Archetypes group entities with identical components, minimizing memory overhead
- **Chunk iteration**: Process entire chunks of entities (16KB) together for maximum cache locality

**Relevance to BlueMarble:**

For BlueMarble's goal of thousands of concurrent NPCs (researchers, traders, creatures) across a planetary surface, DOTS provides the foundational architecture to achieve this performance. Traditional Unity MonoBehaviours would struggle with even 500-1000 agents, while DOTS can scale to 10,000+ agents while maintaining 60 FPS.

---

## Part I: ECS Architecture Fundamentals

### 1. The Entity Component System Paradigm

**Traditional OOP vs Data-Oriented Design:**

Traditional object-oriented programming in Unity uses MonoBehaviours - C# classes attached to GameObjects. This approach has fundamental performance limitations for large-scale simulations.

**Traditional Unity (MonoBehaviour) - The Problem:**

```csharp
// DON'T: Traditional MonoBehaviour approach (slow at scale)
public class NPCAgent : MonoBehaviour {
    public Vector3 velocity;
    public float health;
    public Transform target;
    public AIState currentState;
    
    void Update() {
        // Virtual call overhead
        // Cache misses (scattered memory)
        // No auto-parallelization
        // No SIMD optimization
        
        transform.position += velocity * Time.deltaTime;
        
        if (Vector3.Distance(transform.position, target.position) < 10f) {
            currentState = AIState.Attacking;
        }
    }
}

// Performance: ~500-1000 agents at 60 FPS (CPU bound)
```

**Problems with MonoBehaviour:**
1. **Poor cache locality**: Each GameObject is scattered in memory
2. **Virtual function calls**: Update() requires vtable lookup
3. **No parallelization**: Update() runs serially on main thread
4. **No SIMD**: Compiler cannot vectorize loops over different objects
5. **Memory overhead**: Large GameObject/MonoBehaviour header data

**DOTS ECS Approach - The Solution:**

```csharp
// DO: DOTS ECS approach (fast at scale)

// Components are pure data (structs)
public struct Translation : IComponentData {
    public float3 Value;
}

public struct Velocity : IComponentData {
    public float3 Value;
}

public struct Health : IComponentData {
    public float Value;
}

public struct Target : IComponentData {
    public Entity TargetEntity;
}

public struct AIStateComponent : IComponentData {
    public AIState State;
}

// Systems process components in batches
[BurstCompile]
public partial struct MovementSystem : IJobEntity {
    public float DeltaTime;
    
    // Burst-compiled: SIMD, multi-threaded, zero GC
    void Execute(ref Translation translation, in Velocity velocity) {
        // Processes thousands of entities in parallel
        // Sequential memory access (cache-friendly)
        // Auto-vectorized (SIMD: 4-8 entities per instruction)
        translation.Value += velocity.Value * DeltaTime;
    }
}

// Performance: 10,000+ agents at 60 FPS (GPU bound, not CPU)
```

**Performance Comparison:**

| Approach | Agents at 60 FPS | Memory per Agent | CPU Cores Used | SIMD |
|----------|------------------|------------------|----------------|------|
| MonoBehaviour | 500-1,000 | ~1 KB | 1 (main thread) | No |
| ECS + Jobs | 10,000-50,000 | ~100 bytes | All cores | No |
| ECS + Jobs + Burst | 50,000-100,000+ | ~100 bytes | All cores | Yes |

---

### 2. Core ECS Concepts

**Entities:**

Entities are lightweight identifiers (just an int internally). They have no behavior, only an ID that references their components.

```csharp
// Creating entities
EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

// Method 1: Create entity with archetype
var archetype = entityManager.CreateArchetype(
    typeof(Translation),
    typeof(Velocity),
    typeof(Health)
);
Entity entity = entityManager.CreateEntity(archetype);

// Method 2: Create and add components dynamically
Entity entity2 = entityManager.CreateEntity();
entityManager.AddComponentData(entity2, new Translation { Value = new float3(0, 0, 0) });
entityManager.AddComponentData(entity2, new Velocity { Value = new float3(1, 0, 0) });

// Method 3: Instantiate from prefab (EntityPrefab)
Entity prefabEntity = entityManager.Instantiate(npcPrefab);
```

**Components:**

Components are pure data structures (no methods). They implement `IComponentData` interface.

```csharp
// Component design principles:
// 1. Only data, no methods
// 2. Use float3 instead of Vector3 (Burst-compatible)
// 3. Keep size small (ideally < 64 bytes)
// 4. Use blittable types (structs, primitives, no managed references)

// Good component design
public struct NPCResearcherComponent : IComponentData {
    public float3 Position;
    public float3 Velocity;
    public float Speed;
    public Entity TargetSampleLocation;
    public float SampleProgress;
    public int BehaviorState;
}

// Component size: 40 bytes (excellent)

// BAD component design (DON'T DO THIS)
public struct BadComponent : IComponentData {
    public Vector3 Position;  // Don't: Use float3, not Vector3
    public string Name;       // Don't: No managed types (string)
    public List<int> Items;   // Don't: No collections
    public Action OnUpdate;   // Don't: No delegates
}
```

**Component Types:**

```csharp
// 1. IComponentData - Standard component (chunk memory)
public struct Translation : IComponentData {
    public float3 Value;
}

// 2. ISharedComponentData - Shared across entities (split into chunks)
public struct RenderMesh : ISharedComponentData {
    public Mesh mesh;
    public Material material;
}

// 3. IBufferElementData - Dynamic array attached to entity
public struct InventoryItem : IBufferElementData {
    public int ItemId;
    public int Quantity;
}

// 4. ICleanupComponentData - Persists when entity destroyed
public struct CleanupComponent : ICleanupComponentData {
    public int ResourceId;
}

// 5. Tag component (zero-size, just for filtering)
public struct NPCTag : IComponentData { }
public struct PlayerTag : IComponentData { }
```

**Systems:**

Systems contain the logic that processes entities with specific components.

```csharp
// System types:

// 1. SystemBase - Main thread only, managed code
public partial class SimpleSystem : SystemBase {
    protected override void OnUpdate() {
        Entities
            .WithAll<NPCTag>()
            .ForEach((ref Translation translation, in Velocity velocity) => {
                translation.Value += velocity.Value * SystemAPI.Time.DeltaTime;
            }).Schedule();
    }
}

// 2. ISystem - Unmanaged, Burst-compilable, maximum performance
[BurstCompile]
public partial struct OptimizedMovementSystem : ISystem {
    [BurstCompile]
    public void OnUpdate(ref SystemState state) {
        var job = new MovementJob {
            DeltaTime = SystemAPI.Time.DeltaTime
        };
        job.ScheduleParallel();
    }
}

[BurstCompile]
public partial struct MovementJob : IJobEntity {
    public float DeltaTime;
    
    void Execute(ref Translation translation, in Velocity velocity) {
        translation.Value += velocity.Value * DeltaTime;
    }
}
```

**Archetypes:**

An archetype is a unique combination of component types. Entities with the same archetype are stored together in memory.

```csharp
// Archetype examples:
// Archetype A: [Translation, Velocity, Health]
// Archetype B: [Translation, Velocity, Health, NPCTag]
// Archetype C: [Translation, RenderMesh]

// Entities are organized by archetype for cache efficiency
// Adding/removing components changes archetype (structural change)

// Query entities by archetype
var query = entityManager.CreateEntityQuery(
    typeof(Translation),
    typeof(Velocity)
);

// Get all entities matching query
var entities = query.ToEntityArray(Allocator.Temp);
```

---

### 3. Memory Layout and Chunk Iteration

**Chunk-Based Storage:**

ECS stores entities in 16KB chunks, grouped by archetype. This is the secret to DOTS performance.

**Memory Layout:**

```
Chunk (16KB)
├─ Archetype Metadata
├─ Entity IDs [0...N]
├─ Translation components [0...N]  ← Sequential in memory
├─ Velocity components [0...N]     ← Sequential in memory
└─ Health components [0...N]       ← Sequential in memory

Advantages:
1. Cache-friendly: All components of same type are sequential
2. Efficient iteration: Process entire chunk without cache misses
3. Minimal overhead: Chunk header is small
4. Automatic batching: Systems process chunk at a time
```

**Chunk Capacity Calculation:**

```csharp
// Chunk size = 16KB = 16,384 bytes
// Chunk overhead ≈ 64 bytes
// Available = 16,384 - 64 = 16,320 bytes

// Example archetype:
// Translation (12 bytes) + Velocity (12 bytes) + Health (4 bytes) = 28 bytes per entity
// Entities per chunk = 16,320 / 28 = 582 entities

// Smaller components = More entities per chunk = Better performance
```

**Chunk Iteration Pattern:**

```csharp
[BurstCompile]
public partial struct ChunkIterationExample : IJobEntity {
    // Processes entities chunk by chunk
    // Each chunk: 16KB of sequential memory
    // Cache-friendly: Loads entire chunk into L1/L2 cache
    
    void Execute(ref Translation translation, in Velocity velocity) {
        // This code runs on hundreds of entities sequentially
        // CPU prefetcher loads next cache lines
        // Zero cache misses after first load
        translation.Value += velocity.Value * 0.016f;
    }
}

// Performance characteristics:
// - First entity in chunk: Cache miss (loads 16KB)
// - Next 500-1000 entities: Cache hits (already in L1/L2)
// - Result: 99%+ cache hit rate
```

**Structure of Arrays (SoA) vs Array of Structures (AoS):**

```csharp
// Array of Structures (AoS) - BAD for performance
struct EntityAoS {
    Vector3 position;
    Vector3 velocity;
    float health;
}
EntityAoS[] entities; // Interleaved memory layout

// Problem: When accessing only positions, CPU loads velocity and health too (wasted bandwidth)

// Structure of Arrays (SoA) - GOOD for performance
struct EntitySoA {
    Vector3[] positions;
    Vector3[] velocities;
    float[] healths;
}
EntitySoA entities; // Separated memory layout

// Benefit: Accessing only positions loads contiguous position data (maximum bandwidth)
// ECS automatically uses SoA layout!
```

---

## Part II: Job System and Parallelization

### 4. Unity Job System

**Automatic Multi-Threading:**

The Job System allows you to write multi-threaded code safely without manual thread management, locks, or race conditions.

**Job Types:**

```csharp
// 1. IJob - Single job on worker thread
[BurstCompile]
struct SimpleJob : IJob {
    public float Value;
    
    public void Execute() {
        // Runs once on a worker thread
        Value = math.sqrt(Value);
    }
}

// Schedule job
var job = new SimpleJob { Value = 42f };
JobHandle handle = job.Schedule();
handle.Complete(); // Wait for completion

// 2. IJobParallelFor - Parallel loop over array
[BurstCompile]
struct ParallelJob : IJobParallelFor {
    public NativeArray<float> Values;
    
    public void Execute(int index) {
        // Runs in parallel across worker threads
        // Job system automatically divides work
        Values[index] = math.sqrt(Values[index]);
    }
}

// Schedule parallel job
var values = new NativeArray<float>(10000, Allocator.TempJob);
var job = new ParallelJob { Values = values };
JobHandle handle = job.Schedule(values.Length, 64); // 64 = batch size
handle.Complete();
values.Dispose();

// 3. IJobEntity - Process ECS entities in parallel (most common)
[BurstCompile]
partial struct EntityJob : IJobEntity {
    void Execute(ref Translation translation, in Velocity velocity) {
        // Automatically parallelized by ECS
        // Job system processes entities in parallel chunks
        translation.Value += velocity.Value * 0.016f;
    }
}

// Schedule entity job
new EntityJob().ScheduleParallel();
```

**Job Dependencies:**

```csharp
// Jobs can depend on other jobs completing first
public partial class DependencyExample : SystemBase {
    protected override void OnUpdate() {
        // Job 1: Update velocities based on AI
        var aiJob = new AIUpdateJob { DeltaTime = Time.DeltaTime };
        JobHandle aiHandle = aiJob.ScheduleParallel(Dependency);
        
        // Job 2: Apply velocities to positions (depends on Job 1)
        var movementJob = new MovementJob { DeltaTime = Time.DeltaTime };
        JobHandle movementHandle = movementJob.ScheduleParallel(aiHandle);
        
        // Job 3: Update spatial hash (depends on Job 2)
        var spatialJob = new SpatialHashJob();
        JobHandle spatialHandle = spatialJob.Schedule(movementHandle);
        
        // Chain dependencies
        Dependency = spatialHandle;
    }
}

// Job system automatically schedules jobs on available threads
// Maximizes CPU utilization without manual thread management
```

**Native Collections:**

Jobs require thread-safe data structures. Unity provides Native containers:

```csharp
// Thread-safe collections for jobs:

// 1. NativeArray<T> - Fixed-size array
var array = new NativeArray<float3>(1000, Allocator.TempJob);
array[0] = new float3(1, 2, 3);
array.Dispose(); // Must manually dispose

// 2. NativeList<T> - Dynamic array (like List<T>)
var list = new NativeList<int>(Allocator.TempJob);
list.Add(42);
list.Dispose();

// 3. NativeHashMap<K, V> - Dictionary
var map = new NativeHashMap<int, float>(100, Allocator.TempJob);
map[1] = 3.14f;
map.Dispose();

// 4. NativeQueue<T> - Thread-safe queue
var queue = new NativeQueue<Entity>(Allocator.TempJob);
queue.Enqueue(entity);
queue.Dispose();

// Allocator types:
// - Allocator.Temp: Single frame (fast)
// - Allocator.TempJob: Up to 4 frames (job lifetime)
// - Allocator.Persistent: Long-term (manual management)
```

**Safety System:**

Unity's job system prevents race conditions at compile time:

```csharp
[BurstCompile]
struct SafetyExample : IJobEntity {
    // [ReadOnly] - Can read from multiple threads safely
    [ReadOnly] public NativeArray<float3> Targets;
    
    // No attribute - Exclusive write access (thread-safe by default)
    public NativeArray<float> Results;
    
    void Execute(int index) {
        // Safe: Reading from ReadOnly array
        float3 target = Targets[index];
        
        // Safe: Writing to exclusive array
        Results[index] = math.length(target);
        
        // Compile error if two jobs write to same array simultaneously
    }
}
```

---

### 5. Burst Compiler

**SIMD Auto-Vectorization:**

The Burst compiler translates C# code to highly optimized native code with automatic SIMD (Single Instruction Multiple Data) vectorization.

**Burst Benefits:**

```csharp
// Without Burst: Regular C# (IL2CPP or Mono JIT)
// - Scalar operations (1 value per instruction)
// - No auto-vectorization
// - Managed code overhead

// With Burst: Optimized native code
// - SIMD operations (4-8 values per instruction with AVX2)
// - Auto-vectorization
// - Zero managed overhead
// - Aggressive inlining

// Performance improvement: 10x-100x for math-heavy code
```

**Burst-Compatible Code:**

```csharp
// Burst-compatible: Uses only unmanaged types
[BurstCompile]
struct BurstFriendly : IJobEntity {
    public float DeltaTime;
    
    void Execute(ref Translation translation, in Velocity velocity) {
        // math.* functions (Unity.Mathematics) are Burst-optimized
        translation.Value += velocity.Value * DeltaTime;
        
        // SIMD: Processes 4-8 entities per instruction (AVX2)
        // Result: 4-8x faster than scalar code
    }
}

// NOT Burst-compatible: Uses managed types
struct NotBurstFriendly : IJobEntity {
    void Execute(ref Translation translation) {
        // Don't: Managed types in Burst code
        string name = "Entity"; // Error: string is managed
        var list = new List<int>(); // Error: List is managed
        Debug.Log("Hello"); // Error: Debug.Log uses managed code
    }
}
```

**Burst Performance Example:**

```csharp
// Example: Calculate distance for 10,000 agents

// Method 1: Without Burst (C# managed)
// Time: ~5ms per frame

// Method 2: With Burst, no SIMD
// Time: ~1ms per frame (5x faster)

// Method 3: With Burst + SIMD auto-vectorization
// Time: ~0.2ms per frame (25x faster than C#, 5x faster than non-SIMD)

[BurstCompile]
struct DistanceCalculation : IJobEntity {
    public float3 PlayerPosition;
    [WriteOnly] public NativeArray<float> Distances;
    
    void Execute([EntityInQueryIndex] int index, in Translation translation) {
        // Burst auto-vectorizes this loop
        // Processes 4-8 entities per instruction (AVX2)
        float3 delta = translation.Value - PlayerPosition;
        Distances[index] = math.length(delta);
        
        // Assembly output (AVX2):
        // vsubps ymm0, ymm1, ymm2    ; Subtract 8 floats in parallel
        // vmulps ymm0, ymm0, ymm0    ; Square 8 floats in parallel
        // vhaddps ymm0, ymm0, ymm0   ; Horizontal add
        // vsqrtps ymm0, ymm0         ; Square root 8 floats in parallel
    }
}
```

**Burst Compilation Options:**

```csharp
// Configure Burst compilation
[BurstCompile(
    CompileSynchronously = true,  // Compile in editor (slower edit time, accurate profiling)
    FloatMode = FloatMode.Fast,   // Fast math (less precise, more performance)
    FloatPrecision = FloatPrecision.Low,
    OptimizeFor = OptimizeFor.Performance
)]
struct HighPerformanceJob : IJobEntity {
    void Execute(ref Translation translation) {
        // Highly optimized, slightly less precise
    }
}
```

---

## Part III: BlueMarble ECS Implementation

### 6. NPC Agent System Architecture

**Researcher Agent Implementation:**

```csharp
// Component definitions for researcher NPCs

public struct ResearcherComponent : IComponentData {
    public float3 HomeBase;
    public float ExplorationRadius;
    public int SamplesCollected;
    public int SampleCapacity;
    public float Speed;
}

public struct SampleTargetComponent : IComponentData {
    public Entity TargetEntity;
    public float3 TargetPosition;
    public float SampleProgress;
}

public struct InventoryComponent : IComponentData {
    public int GoldValue;
    public float Weight;
    public float MaxWeight;
}

public struct ResearcherInventoryItem : IBufferElementData {
    public int ItemType;
    public int Quantity;
    public float Rarity;
}

public struct ResearcherStateComponent : IComponentData {
    public ResearcherState State;
}

public enum ResearcherState {
    Idle,
    SeekingSample,
    NavigatingToSample,
    Sampling,
    ReturningToBase,
    DepositingSamples
}

// Systems for researcher behavior

[BurstCompile]
public partial struct ResearcherMovementSystem : ISystem {
    [BurstCompile]
    public void OnUpdate(ref SystemState state) {
        var job = new ResearcherMoveJob {
            DeltaTime = SystemAPI.Time.DeltaTime
        };
        state.Dependency = job.ScheduleParallel(state.Dependency);
    }
}

[BurstCompile]
public partial struct ResearcherMoveJob : IJobEntity {
    public float DeltaTime;
    
    void Execute(
        ref Translation translation,
        ref Velocity velocity,
        in ResearcherComponent researcher,
        in SampleTargetComponent target
    ) {
        // Calculate direction to target
        float3 toTarget = target.TargetPosition - translation.Value;
        float distance = math.length(toTarget);
        
        if (distance > 0.1f) {
            // Move towards target
            float3 direction = math.normalize(toTarget);
            velocity.Value = direction * researcher.Speed;
            translation.Value += velocity.Value * DeltaTime;
        } else {
            // Arrived at target
            velocity.Value = float3.zero;
        }
    }
}

// Sample collection system
public partial class ResearcherSamplingSystem : SystemBase {
    protected override void OnUpdate() {
        var ecb = new EntityCommandBuffer(Allocator.TempJob);
        
        Entities
            .WithAll<ResearcherComponent>()
            .ForEach((
                Entity entity,
                ref ResearcherStateComponent state,
                ref SampleTargetComponent target,
                in Translation translation,
                in ResearcherComponent researcher
            ) => {
                if (state.State != ResearcherState.Sampling) return;
                
                float distance = math.distance(translation.Value, target.TargetPosition);
                
                if (distance < 2f) {
                    // In range, collect sample
                    target.SampleProgress += Time.DeltaTime;
                    
                    if (target.SampleProgress >= 3f) {
                        // Sample collected
                        var buffer = GetBuffer<ResearcherInventoryItem>(entity);
                        buffer.Add(new ResearcherInventoryItem {
                            ItemType = 1, // Rock sample
                            Quantity = 1,
                            Rarity = UnityEngine.Random.value
                        });
                        
                        // Destroy sample node
                        ecb.DestroyEntity(target.TargetEntity);
                        
                        // Transition to next state
                        if (buffer.Length >= researcher.SampleCapacity) {
                            state.State = ResearcherState.ReturningToBase;
                        } else {
                            state.State = ResearcherState.SeekingSample;
                        }
                        
                        target.SampleProgress = 0f;
                    }
                }
            }).Run();
        
        ecb.Playback(EntityManager);
        ecb.Dispose();
    }
}
```

**Trader Agent Implementation:**

```csharp
// Trader NPC components

public struct TraderComponent : IComponentData {
    public float3 CurrentMarket;
    public float Wealth;
    public float PriceMemoryDecay;
}

public struct TradingGoalComponent : IComponentData {
    public TradeGoalType GoalType;
    public int ItemType;
    public float TargetPrice;
    public Entity TargetMarket;
}

public enum TradeGoalType {
    Buy,
    Sell,
    Travel,
    Wait
}

public struct TraderInventoryItem : IBufferElementData {
    public int ItemType;
    public int Quantity;
    public float PurchasePrice;
}

public struct PriceMemory : IBufferElementData {
    public int ItemType;
    public float LastKnownPrice;
    public float PriceAverage;
    public long Timestamp;
}

// Trader AI decision system
public partial class TraderDecisionSystem : SystemBase {
    protected override void OnUpdate() {
        var deltaTime = Time.DeltaTime;
        
        Entities
            .WithAll<TraderComponent>()
            .ForEach((
                Entity entity,
                ref TradingGoalComponent goal,
                in TraderComponent trader,
                in Translation translation,
                in DynamicBuffer<TraderInventoryItem> inventory,
                in DynamicBuffer<PriceMemory> priceMemory
            ) => {
                // Decision making logic
                if (goal.GoalType == TradeGoalType.Wait) {
                    // Scan market for opportunities
                    var opportunity = FindBestTradeOpportunity(
                        priceMemory, inventory, trader.Wealth
                    );
                    
                    if (opportunity.ExpectedProfit > 100f) {
                        goal.GoalType = opportunity.GoalType;
                        goal.ItemType = opportunity.ItemType;
                        goal.TargetPrice = opportunity.TargetPrice;
                        goal.TargetMarket = opportunity.TargetMarket;
                    }
                }
            }).Run();
    }
    
    private TradeOpportunity FindBestTradeOpportunity(
        DynamicBuffer<PriceMemory> priceMemory,
        DynamicBuffer<TraderInventoryItem> inventory,
        float wealth
    ) {
        // Analyze price memory to find arbitrage opportunities
        // Compare current market prices with remembered prices from other markets
        // Calculate expected profit minus travel costs
        // Return best opportunity
        
        // Simplified example
        return new TradeOpportunity {
            GoalType = TradeGoalType.Buy,
            ItemType = 5,
            TargetPrice = 100f,
            ExpectedProfit = 250f,
            TargetMarket = Entity.Null
        };
    }
}

struct TradeOpportunity {
    public TradeGoalType GoalType;
    public int ItemType;
    public float TargetPrice;
    public float ExpectedProfit;
    public Entity TargetMarket;
}
```

---

### 7. Performance Optimization Patterns

**LOD System for Agents:**

```csharp
// Level of Detail system for AI complexity

public struct AgentLOD : IComponentData {
    public LODLevel Level;
}

public enum LODLevel {
    Full,    // < 100m: Full AI, detailed animation
    Medium,  // 100-500m: Simplified AI, reduced animation
    Low,     // 500-2000m: Basic movement only
    Culled   // > 2000m: Frozen/extrapolated
}

[BurstCompile]
public partial struct LODUpdateSystem : ISystem {
    [BurstCompile]
    public void OnUpdate(ref SystemState state) {
        // Get camera position (single value lookup)
        var cameraEntity = SystemAPI.GetSingletonEntity<MainCamera>();
        var cameraPos = SystemAPI.GetComponent<Translation>(cameraEntity).Value;
        
        // Update LOD levels based on distance to camera
        new LODUpdateJob {
            CameraPosition = cameraPos
        }.ScheduleParallel();
    }
}

[BurstCompile]
partial struct LODUpdateJob : IJobEntity {
    public float3 CameraPosition;
    
    void Execute(ref AgentLOD lod, in Translation translation) {
        float distance = math.distance(translation.Value, CameraPosition);
        
        lod.Level = 
            distance < 100f ? LODLevel.Full :
            distance < 500f ? LODLevel.Medium :
            distance < 2000f ? LODLevel.Low :
            LODLevel.Culled;
    }
}

// Separate systems for each LOD level
[BurstCompile]
public partial struct FullAISystem : ISystem {
    public void OnUpdate(ref SystemState state) {
        // Only process agents with Full LOD
        var query = SystemAPI.QueryBuilder()
            .WithAll<AgentLOD, NPCTag>()
            .Build();
        
        new FullAIJob().ScheduleParallel(query);
    }
}

[BurstCompile]
partial struct FullAIJob : IJobEntity {
    void Execute(
        in AgentLOD lod,
        ref Translation translation,
        ref AIState aiState
    ) {
        // Only execute for Full LOD
        if (lod.Level != LODLevel.Full) return;
        
        // Complex AI logic
        // Behavior tree evaluation
        // Detailed perception
        // Pathfinding
    }
}
```

**Spatial Hashing for Perception:**

```csharp
// Efficient neighbor queries using spatial hash

public struct SpatialHashCell {
    public int2 CellCoord;
    public NativeList<Entity> Entities;
}

public partial class SpatialHashSystem : SystemBase {
    private NativeHashMap<int2, NativeList<Entity>> spatialHash;
    
    protected override void OnCreate() {
        spatialHash = new NativeHashMap<int2, NativeList<Entity>>(
            10000, Allocator.Persistent
        );
    }
    
    protected override void OnDestroy() {
        // Cleanup
        foreach (var list in spatialHash.GetValueArray(Allocator.Temp)) {
            list.Dispose();
        }
        spatialHash.Dispose();
    }
    
    protected override void OnUpdate() {
        // Clear previous frame
        foreach (var kvp in spatialHash) {
            kvp.Value.Clear();
        }
        
        // Rebuild spatial hash
        var hash = spatialHash;
        Entities
            .WithAll<NPCTag>()
            .ForEach((Entity entity, in Translation translation) => {
                int2 cellCoord = GetCellCoord(translation.Value);
                
                if (!hash.ContainsKey(cellCoord)) {
                    hash[cellCoord] = new NativeList<Entity>(16, Allocator.Persistent);
                }
                
                hash[cellCoord].Add(entity);
            }).Run();
        
        // Use spatial hash for perception queries
        PerformPerceptionQueries();
    }
    
    private int2 GetCellCoord(float3 position) {
        const float cellSize = 10f; // 10m cells
        return new int2(
            (int)math.floor(position.x / cellSize),
            (int)math.floor(position.z / cellSize)
        );
    }
    
    private void PerformPerceptionQueries() {
        var hash = spatialHash;
        
        Entities
            .WithAll<NPCTag>()
            .ForEach((Entity entity, ref PerceptionComponent perception, 
                     in Translation translation) => {
                
                int2 cellCoord = GetCellCoord(translation.Value);
                float perceptionRadius = 20f;
                
                perception.VisibleEntities.Clear();
                
                // Check 3x3 grid around entity
                for (int y = -1; y <= 1; y++) {
                    for (int x = -1; x <= 1; x++) {
                        int2 checkCell = cellCoord + new int2(x, y);
                        
                        if (!hash.ContainsKey(checkCell)) continue;
                        
                        var cellEntities = hash[checkCell];
                        foreach (var other in cellEntities) {
                            if (other == entity) continue;
                            
                            var otherPos = GetComponent<Translation>(other).Value;
                            float distance = math.distance(translation.Value, otherPos);
                            
                            if (distance < perceptionRadius) {
                                perception.VisibleEntities.Add(other);
                            }
                        }
                    }
                }
            }).Run();
    }
}

public struct PerceptionComponent : IComponentData {
    public NativeList<Entity> VisibleEntities;
    public float PerceptionRadius;
}
```

---

## Part IV: Advanced ECS Patterns

### 8. Entity Command Buffers

**Deferred Structural Changes:**

Adding/removing components or creating/destroying entities are "structural changes" that cannot happen during job execution. Entity Command Buffers (ECB) record these operations for playback later.

```csharp
public partial class SpawnSystem : SystemBase {
    protected override void OnUpdate() {
        // Create ECB to record deferred operations
        var ecb = new EntityCommandBuffer(Allocator.TempJob);
        
        Entities
            .WithAll<SpawnerComponent>()
            .ForEach((Entity entity, in SpawnerComponent spawner) => {
                
                if (spawner.ShouldSpawn) {
                    // Deferred: Create new entity
                    Entity newEntity = ecb.CreateEntity();
                    
                    // Deferred: Add components
                    ecb.AddComponent(newEntity, new Translation { Value = spawner.SpawnPosition });
                    ecb.AddComponent(newEntity, new Velocity { Value = float3.zero });
                    ecb.AddComponent(newEntity, new NPCTag());
                    
                    // Deferred: Remove spawner component
                    ecb.RemoveComponent<SpawnerComponent>(entity);
                }
            }).Schedule();
        
        // Playback all recorded operations
        Dependency.Complete();
        ecb.Playback(EntityManager);
        ecb.Dispose();
    }
}
```

---

### 9. Hybrid Rendering

**Integration with GameObjects:**

DOTS can interoperate with traditional Unity GameObjects for rendering while keeping logic in ECS.

```csharp
// Convert GameObject to Entity
public class NPCAuthoring : MonoBehaviour {
    public float Speed = 5f;
    public float Health = 100f;
    
    class Baker : Baker<NPCAuthoring> {
        public override void Bake(NPCAuthoring authoring) {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            
            AddComponent(entity, new ResearcherComponent {
                Speed = authoring.Speed,
                ExplorationRadius = 100f,
                SampleCapacity = 10,
                SamplesCollected = 0
            });
            
            AddComponent(entity, new Health {
                Value = authoring.Health
            });
        }
    }
}

// ECS updates transform, GameObject follows
// No need to manually sync - DOTS handles it
```

---

## Part V: Discovered Sources & Conclusion

### Discovered Sources for Phase 4

**From Source 2: Unity DOTS - ECS for Agents**

1. **Unity DOTS Physics Package**
   - Discovered From: DOTS documentation - Physics integration
   - Priority: High
   - Category: GameDev-Tech
   - Rationale: Stateless physics for massive entity counts
   - Estimated Effort: 6-8 hours

2. **Unity NetCode for DOTS**
   - Discovered From: DOTS documentation - Multiplayer section
   - Priority: High
   - Category: GameDev-Tech
   - Rationale: Network replication for ECS entities
   - Estimated Effort: 10-12 hours

3. **ECS Best Practices and Patterns**
   - Discovered From: DOTS community resources
   - Priority: Medium
   - Category: GameDev-Tech
   - Rationale: Design patterns specific to ECS architecture
   - Estimated Effort: 4-6 hours

4. **Unity DOTS Streaming**
   - Discovered From: DOTS documentation - World streaming
   - Priority: High
   - Category: GameDev-Tech
   - Rationale: Stream entities in/out for open worlds
   - Estimated Effort: 8-10 hours

---

## Conclusion

Unity DOTS with ECS provides the performance foundation BlueMarble needs for massive-scale agent simulation. Key implementation priorities:

1. **Start with ECS architecture** from day one (don't convert later)
2. **Use Burst compiler** for all math-heavy systems
3. **Implement LOD system** for AI complexity scaling
4. **Leverage Job System** for automatic parallelization
5. **Design small components** (< 64 bytes) for cache efficiency
6. **Use spatial hashing** for efficient neighbor queries
7. **Profile early and often** to identify bottlenecks

With DOTS, BlueMarble can achieve **10,000+ concurrent NPCs at 60 FPS**, enabling the rich, living world required for a geological simulation MMORPG.

---

**Cross-References:**
- See `game-dev-analysis-ai-game-programming-wisdom.md` for AI behavior patterns
- See `game-engine-architecture.md` for subsystem integration
- See `unity-ecs-dots-documentation.md` for detailed API reference

**Status:** ✅ Complete  
**Next:** Write Batch 1 Summary, then process Source 3 (Game Engine Architecture)  
**Document Length:** 1000+ lines  
**BlueMarble Applicability:** Critical - Core performance architecture

---
