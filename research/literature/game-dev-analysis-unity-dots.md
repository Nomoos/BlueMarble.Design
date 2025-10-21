# Unity DOTS - Data-Oriented Technology Stack Analysis

---
title: Unity DOTS - Data-Oriented Technology Stack Analysis for BlueMarble MMORPG
date: 2025-01-17
tags: [unity, dots, ecs, performance, optimization, job-system, burst-compiler, game-development]
status: complete
priority: medium
parent-research: research-assignment-group-31.md
discovered-from: GameDev.tv
source-url: https://unity.com/dots
documentation: https://docs.unity3d.com/Packages/com.unity.entities@latest
---

**Source:** Unity DOTS (Data-Oriented Technology Stack)  
**Category:** Game Development - Performance Architecture  
**Priority:** Medium  
**Status:** ✅ Complete  
**Lines:** 500+  
**Related Sources:** GameDev.tv, Unity Documentation, ECS Architecture Patterns

---

## Executive Summary

Unity DOTS (Data-Oriented Technology Stack) is Unity's high-performance architecture that represents a paradigm shift from traditional object-oriented programming to data-oriented design. DOTS consists of three main technologies: Entity Component System (ECS), C# Job System, and Burst Compiler. Together, these enable games to handle massive numbers of entities with optimal CPU cache usage and multi-threading. For MMORPGs like BlueMarble, DOTS offers the potential to simulate thousands of geological features, resources, and players efficiently.

**Key Value for BlueMarble:**
- Handle 100,000+ entities (resources, geological features, NPCs) efficiently
- Multi-threaded performance on modern CPUs (utilize all cores)
- Optimal CPU cache usage (10-100x faster than OOP for large datasets)
- Built-in job scheduling prevents race conditions
- Future-proof architecture (Unity's recommended approach)
- Burst compiler generates highly optimized native code
- Ideal for planet-scale simulation

**Technology Statistics:**
- Released: 2019 (still evolving, not yet stable)
- Unity 2022 LTS+: Production-ready
- Used in: Mobile strategy games, simulation games, large-scale multiplayer
- Performance: 10-100x faster than traditional Unity for large datasets
- Learning Curve: Steep (different programming paradigm)
- Status: Preview/Experimental (as of 2024, approaching stability)

**Core Components Relevant to BlueMarble:**
1. Entity Component System (ECS) - Data-oriented entity management
2. C# Job System - Safe multi-threading
3. Burst Compiler - Optimized native code generation
4. DOTS Physics - High-performance physics simulation
5. DOTS NetCode - Multiplayer networking for ECS
6. Hybrid Rendering - Bridge between ECS and GameObjects

---

## Core Concepts

### 1. Why Data-Oriented Design?

**Traditional OOP Problem:**

```csharp
// Traditional Unity (GameObject/MonoBehaviour)
public class ResourceNode : MonoBehaviour
{
    public string mineralName;      // 8 bytes (reference)
    public Vector3 position;        // 12 bytes
    public Quaternion rotation;     // 16 bytes
    public float currentYield;      // 4 bytes
    public bool depleted;           // 1 byte
    public MineralType type;        // 4 bytes
    
    // Methods
    void Update() {
        // Runs on main thread, one at a time
        CheckDepletion();
        UpdateVisuals();
    }
}

// Problem: Bad cache locality
// CPU loads entire object (100+ bytes) to access one field (4 bytes)
// Can't process multiple nodes in parallel easily
// Update() calls scattered across memory
```

**DOTS Solution:**

```csharp
// ECS separates data from behavior
// Component (Data Only - struct)
public struct ResourceNodeComponent : IComponentData
{
    public float currentYield;
    public bool depleted;
    public MineralType type;
}

public struct Position : IComponentData
{
    public float3 value;
}

// System (Behavior - operates on all entities in parallel)
public partial class ResourceUpdateSystem : SystemBase
{
    protected override void OnUpdate()
    {
        // Process ALL resource nodes in parallel
        Entities
            .WithAll<ResourceNodeComponent>()
            .ForEach((ref ResourceNodeComponent node, in Position pos) =>
            {
                if (node.currentYield <= 0)
                {
                    node.depleted = true;
                }
            })
            .ScheduleParallel(); // Runs on all CPU cores!
    }
}

// Benefits:
// - Tight memory layout (cache-friendly)
// - Multi-threaded automatically
// - Burst compiler optimizes to native code
// - Can process 100,000+ entities per frame
```

### 2. Entity Component System (ECS)

**Core Principles:**

```csharp
// Entity: Just an ID (lightweight)
Entity resourceEntity = entityManager.CreateEntity();

// Components: Pure data (structs)
entityManager.AddComponentData(resourceEntity, new ResourceNodeComponent
{
    currentYield = 100f,
    depleted = false,
    type = MineralType.Iron
});

entityManager.AddComponentData(resourceEntity, new Position
{
    value = new float3(100, 0, 200)
});

// Systems: Process entities with specific components
[BurstCompile]
public partial class ResourceExtractionSystem : SystemBase
{
    protected override void OnUpdate()
    {
        float deltaTime = Time.DeltaTime;
        
        // Query: Find all entities with both components
        Entities
            .WithAll<ResourceNodeComponent, Position>()
            .ForEach((Entity entity, 
                     ref ResourceNodeComponent node,
                     in Position pos) =>
            {
                // This lambda runs for EVERY matching entity
                // In parallel across CPU cores
                // With optimal cache usage
                
                if (!node.depleted && node.currentYield > 0)
                {
                    // Extraction logic here
                    node.currentYield -= deltaTime * 0.1f;
                    
                    if (node.currentYield <= 0)
                    {
                        node.depleted = true;
                    }
                }
            })
            .ScheduleParallel(); // Magic: automatic parallelization!
    }
}
```

**Component Types:**

```csharp
// 1. IComponentData: Simple data (max 16KB)
public struct Health : IComponentData
{
    public float current;
    public float max;
}

// 2. IBufferElementData: Dynamic arrays
public struct InventoryItem : IBufferElementData
{
    public int itemId;
    public int quantity;
}

// Usage:
DynamicBuffer<InventoryItem> inventory = entityManager.GetBuffer<InventoryItem>(playerEntity);
inventory.Add(new InventoryItem { itemId = 101, quantity = 5 });

// 3. ISharedComponentData: Shared across multiple entities (reduces memory)
public struct RegionTag : ISharedComponentData
{
    public int regionId;
}

// All resources in same region share one instance
// Allows efficient chunking and processing by region

// 4. Tag Component: Zero-size, just marks entities
public struct DestroyedTag : IComponentData { }
```

### 3. Job System - Safe Multi-Threading

**Jobs replace traditional threading:**

```csharp
// IJob: Single-threaded job
[BurstCompile]
public struct CalculateGeologyJob : IJob
{
    public float deltaTime;
    public NativeArray<float> temperatures;
    
    public void Execute()
    {
        for (int i = 0; i < temperatures.Length; i++)
        {
            temperatures[i] += deltaTime * 0.1f;
        }
    }
}

// Schedule job
var job = new CalculateGeologyJob
{
    deltaTime = Time.deltaTime,
    temperatures = temperatureArray
};

JobHandle handle = job.Schedule();
handle.Complete(); // Wait for completion

// IJobParallelFor: Multi-threaded job
[BurstCompile]
public struct UpdateResourceNodesJob : IJobParallelFor
{
    public float deltaTime;
    public NativeArray<ResourceNodeData> nodes;
    
    public void Execute(int index)
    {
        // Runs in parallel for each index
        var node = nodes[index];
        
        if (!node.depleted)
        {
            node.currentYield -= deltaTime * node.depletionRate;
            if (node.currentYield <= 0)
            {
                node.depleted = true;
            }
        }
        
        nodes[index] = node;
    }
}

// Schedule parallel job
var job = new UpdateResourceNodesJob
{
    deltaTime = Time.deltaTime,
    nodes = nodeArray
};

JobHandle handle = job.Schedule(nodeArray.Length, 64); // 64 items per batch
handle.Complete();
```

**Job Dependencies:**

```csharp
// Chain jobs efficiently
JobHandle job1Handle = job1.Schedule();
JobHandle job2Handle = job2.Schedule(job1Handle); // job2 waits for job1
JobHandle job3Handle = job3.Schedule(job2Handle); // job3 waits for job2

// Run in parallel
JobHandle job1Handle = job1.Schedule();
JobHandle job2Handle = job2.Schedule(); // Runs parallel to job1
JobHandle.CombineDependencies(job1Handle, job2Handle); // Wait for both
```

### 4. Burst Compiler

**Native Code Performance:**

```csharp
// Without Burst: Standard C# IL code
public float CalculateDistance(float3 a, float3 b)
{
    float dx = b.x - a.x;
    float dy = b.y - a.y;
    float dz = b.z - a.z;
    return math.sqrt(dx * dx + dy * dy + dz * dz);
}

// With Burst: Optimized native code
[BurstCompile]
public float CalculateDistance(float3 a, float3 b)
{
    // Same code, but Burst generates:
    // - SIMD instructions (process 4-8 floats at once)
    // - Inlined function calls
    // - Optimized loop unrolling
    // - No garbage collection overhead
    
    return math.distance(a, b); // math.distance is Burst-optimized
}

// Result: 10-20x faster!
```

**Burst Restrictions:**

```csharp
// ✅ Allowed in Burst
- Math operations (math library)
- Structs and primitive types
- NativeArray, NativeList
- Fixed-size arrays
- Function pointers

// ❌ Not allowed in Burst
- Managed objects (classes)
- Garbage collection
- Exception handling (throw)
- String operations
- LINQ queries
- Virtual methods
```

---

## BlueMarble Application

### 1. Geological Simulation with DOTS

**Massive-Scale Resource Management:**

```csharp
// Handle 100,000+ resource nodes efficiently
public struct ResourceNodeData : IComponentData
{
    public float currentYield;
    public float maxYield;
    public float regenerationRate;
    public bool depleted;
    public MineralType type;
}

public struct WorldPosition : IComponentData
{
    public float3 value;
}

[BurstCompile]
public partial class ResourceRegenerationSystem : SystemBase
{
    protected override void OnUpdate()
    {
        float deltaTime = Time.DeltaTime;
        
        // Process 100,000+ nodes in parallel
        Entities
            .WithAll<ResourceNodeData>()
            .ForEach((ref ResourceNodeData node) =>
            {
                if (node.depleted && node.currentYield < node.maxYield)
                {
                    node.currentYield += node.regenerationRate * deltaTime;
                    
                    if (node.currentYield >= node.maxYield)
                    {
                        node.currentYield = node.maxYield;
                        node.depleted = false;
                    }
                }
            })
            .ScheduleParallel();
    }
}
```

**Spatial Partitioning with ECS:**

```csharp
// Grid-based spatial hash for planet-scale world
public struct GridCell : ISharedComponentData
{
    public int2 coordinates; // Grid cell coordinates
}

public struct SpatialHashSystem : SystemBase
{
    protected override void OnUpdate()
    {
        // Query players in specific grid cell
        int2 targetCell = new int2(10, 20);
        
        Entities
            .WithSharedComponentFilter(new GridCell { coordinates = targetCell })
            .ForEach((in WorldPosition pos, in PlayerComponent player) =>
            {
                // Only process players in this grid cell
                // Automatic spatial culling!
            })
            .Schedule();
    }
}
```

### 2. Player Management at Scale

**1000+ Concurrent Players:**

```csharp
public struct PlayerComponent : IComponentData
{
    public int playerId;
    public float health;
    public float stamina;
}

public struct PlayerInventory : IBufferElementData
{
    public int itemId;
    public int quantity;
}

[BurstCompile]
public partial class PlayerUpdateSystem : SystemBase
{
    protected override void OnUpdate()
    {
        float deltaTime = Time.DeltaTime;
        
        // Update all players in parallel
        Entities
            .WithAll<PlayerComponent>()
            .ForEach((ref PlayerComponent player) =>
            {
                // Regenerate stamina
                if (player.stamina < 100f)
                {
                    player.stamina += deltaTime * 5f;
                    player.stamina = math.min(player.stamina, 100f);
                }
            })
            .ScheduleParallel();
    }
}
```

### 3. When to Use DOTS vs Traditional Unity

**Decision Matrix:**

| Scenario | Use DOTS | Use Traditional |
|----------|----------|-----------------|
| **10,000+ entities** | ✅ Essential | ❌ Too slow |
| **Complex game logic** | ⚠️ Harder to code | ✅ Easier |
| **Rapid prototyping** | ❌ Slow to iterate | ✅ Fast iteration |
| **Mobile performance** | ✅ Excellent | ⚠️ Limited |
| **Networking** | ⚠️ DOTS NetCode | ✅ Mirror/FishNet |
| **Team experience** | ⚠️ Learning curve | ✅ Familiar |
| **Production stability** | ⚠️ Still evolving | ✅ Mature |

**Recommendation for BlueMarble:**

**Phase 1-3 (Years 1-2): Traditional Unity**
- Use MonoBehaviours and GameObjects
- Easier development, faster iteration
- Stable networking with Mirror/FishNet
- Sufficient for 1,000-5,000 entities

**Phase 4+ (Year 3+): Hybrid DOTS**
- Move resource nodes to ECS (100,000+)
- Move geological simulation to DOTS
- Keep player logic in traditional Unity
- Use DOTS for performance-critical systems only

**Full DOTS: Only if absolutely necessary**
- If targeting 10,000+ concurrent players
- If geological simulation becomes too slow
- If mobile performance is critical

---

## Implementation Recommendations

### 1. Getting Started with DOTS

**Installation:**

```
Unity Package Manager:
1. Install "Entities" package
2. Install "Jobs" package  
3. Install "Burst" package
4. Install "Collections" package

Unity 2022.2 LTS or newer recommended
```

**Hello World in DOTS:**

```csharp
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

// Component
public struct RotationSpeed : IComponentData
{
    public float radiansPerSecond;
}

// System
[BurstCompile]
public partial class RotationSpeedSystem : SystemBase
{
    protected override void OnUpdate()
    {
        float deltaTime = SystemAPI.Time.DeltaTime;
        
        foreach (var (transform, speed) in 
                 SystemAPI.Query<RefRW<LocalTransform>, RefRO<RotationSpeed>>())
        {
            transform.ValueRW.Rotation = math.mul(
                transform.ValueRO.Rotation,
                quaternion.RotateY(speed.ValueRO.radiansPerSecond * deltaTime)
            );
        }
    }
}

// Authoring (bridge to traditional Unity)
public class RotationSpeedAuthoring : MonoBehaviour
{
    public float degreesPerSecond = 360f;
    
    class Baker : Baker<RotationSpeedAuthoring>
    {
        public override void Bake(RotationSpeedAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new RotationSpeed
            {
                radiansPerSecond = math.radians(authoring.degreesPerSecond)
            });
        }
    }
}
```

### 2. Gradual Adoption Strategy

**Step 1: Learn (Month 1)**
- Complete Unity DOTS tutorials
- Build simple ECS prototypes
- Understand limitations and trade-offs

**Step 2: Isolate (Month 2-3)**
- Convert one system to DOTS (e.g., particle effects)
- Measure performance gains
- Identify pitfalls

**Step 3: Expand (Month 4-6)**
- Convert resource node management to DOTS
- Keep game logic in traditional Unity
- Hybrid approach

**Step 4: Optimize (Month 7+)**
- Profile and identify bottlenecks
- Convert critical systems to DOTS
- Maintain hybrid architecture

### 3. Common Pitfalls

**Pitfall 1: Converting everything to ECS**
```csharp
// ❌ Don't do this
// Converting simple UI logic to ECS adds complexity

// ✅ Do this instead
// Use ECS for performance-critical systems only
// Keep UI, game logic, networking in traditional Unity
```

**Pitfall 2: Ignoring structural changes**
```csharp
// ❌ Structural changes in ForEach
Entities.ForEach((Entity entity, in Position pos) =>
{
    // This breaks parallelization!
    EntityManager.AddComponent<Tag>(entity);
}).Schedule();

// ✅ Use EntityCommandBuffer
EntityCommandBuffer ecb = new EntityCommandBuffer(Allocator.TempJob);

Entities.ForEach((Entity entity, in Position pos) =>
{
    ecb.AddComponent<Tag>(entity); // Deferred
}).Schedule();

ecb.Playback(EntityManager); // Apply changes
ecb.Dispose();
```

---

## References

### Primary Sources

1. **Unity DOTS Official**
   - Website: https://unity.com/dots
   - Documentation: https://docs.unity3d.com/Packages/com.unity.entities@latest
   - GitHub: https://github.com/Unity-Technologies/EntityComponentSystemSamples

2. **Learning Resources**
   - Unity Learn: https://learn.unity.com/search?k=%5B%22tag%3A5ee22030edbc2a001f6bec6b%22%5D
   - Official Tutorials: YouTube Unity channel
   - Community Forum: https://forum.unity.com/forums/data-oriented-technology-stack.147/

3. **Sample Projects**
   - Megacity: https://github.com/Unity-Technologies/Megacity-Metro
   - NetCode Samples: https://github.com/Unity-Technologies/EntityComponentSystemSamples

### Supporting Documentation

1. **ECS Architecture**
   - Overwatch GDC Talk: ECS architecture
   - Data-Oriented Design Book: Richard Fabian
   - CppCon: Mike Acton's "Data-Oriented Design"

2. **Performance**
   - Unity Profiler docs
   - Burst Compiler manual
   - Job System best practices

---

## Related Research

### Within BlueMarble Repository

- [game-dev-analysis-gamedev.tv.md](./game-dev-analysis-gamedev.tv.md) - Source of DOTS discovery
- [game-dev-analysis-fish-networking.md](./game-dev-analysis-fish-networking.md) - Networking (DOTS NetCode compatible)
- [research-assignment-group-31.md](./research-assignment-group-31.md) - Parent research assignment

---

## New Sources Discovered During Analysis

### 1. Mike Acton's "Data-Oriented Design" Talk
- **Type:** CppCon conference talk
- **URL:** https://www.youtube.com/watch?v=rX0ItVEVjHc
- **Priority:** Medium
- **Rationale:** Foundational talk that inspired Unity DOTS. Essential for understanding data-oriented design principles beyond just Unity.
- **Next Action:** Watch and document key principles

---

**Document Status:** ✅ Complete  
**Last Updated:** 2025-01-17  
**Word Count:** ~3,000 words  
**Lines:** 650+  
**Batch Complete:** All 4 sources processed

---

## Conclusion

Unity DOTS is a powerful but complex technology. For BlueMarble:

**Pros:**
- Handle 100,000+ resource nodes efficiently
- Multi-threaded geological simulation
- Future-proof architecture
- Excellent mobile performance

**Cons:**
- Steep learning curve (6-12 months to master)
- Still evolving (not fully stable)
- Networking story unclear (DOTS NetCode experimental)
- Harder to debug and iterate

**Verdict:** Worth learning for long-term, but **not critical for MVP**. Start with traditional Unity + Mirror/FishNet, evaluate DOTS in Year 2 when project scales beyond 10,000 concurrent entities.

**Priority:** **Low for Phase 1-3, High for Phase 4+**
