---
title: "Unity Entities Graphics (Hybrid Renderer V2) - ECS Rendering at Scale"
date: 2025-01-17
tags: [research, phase-3, group-45, discovered-source, unity, ecs, dots, rendering, graphics, hybrid-renderer, gpu-instancing, lod]
source_type: Documentation & Technical Reference
priority: High
status: Complete
estimated_effort: 6-8 hours
batch: 4 (Discovered Sources)
discovered_from: ["Unity ECS/DOTS Documentation", "Building Open Worlds Collection"]
---

# Unity Entities Graphics (Hybrid Renderer V2) - ECS Rendering at Scale

## Executive Summary

Unity Entities Graphics (formerly Hybrid Renderer V2) represents Unity's modern approach to rendering Entity Component System (ECS) entities at massive scale. This analysis examines how to leverage the Scriptable Render Pipeline (SRP) Batcher, GPU instancing, and data-oriented rendering to achieve 10,000+ rendered entities at 60 FPS—critical for BlueMarble's planetary-scale geological simulation with thousands of research samples, geological features, and environmental objects.

### Key Takeaways

1. **Massive-Scale Rendering**: Hybrid Renderer V2 enables rendering 10,000+ entities with GPU instancing and SRP Batcher optimizations
2. **Data-Oriented Rendering**: Component-based material and mesh assignments integrate seamlessly with ECS architecture
3. **LOD at Scale**: Distance-based LOD groups manage detail levels for thousands of objects automatically
4. **GPU Culling**: Frustum and occlusion culling happen on GPU for minimal CPU overhead
5. **Procedural Integration**: Works with procedurally generated content and runtime material modifications
6. **BlueMarble Application**: Perfect for rendering geological samples, terrain features, research stations, and environmental objects

### Performance Characteristics

- **10,000+ entities rendered** at 60 FPS (vs 1,000-2,000 with GameObject rendering)
- **< 2ms CPU overhead** for rendering (vs 5-10ms traditional)
- **GPU instancing** reduces draw calls from 10,000 to 10-100
- **SRP Batcher** eliminates per-object CPU setup
- **Automatic LOD** transitions for 10,000+ objects simultaneously

---

## 1. Hybrid Renderer V2 Architecture

### 1.1 Overview

Entities Graphics replaces traditional GameObject rendering with a component-based system optimized for ECS:

```csharp
// Traditional GameObject rendering (slow at scale)
public class TraditionalRenderer : MonoBehaviour
{
    public MeshRenderer meshRenderer;  // Component per GameObject
    public Material material;          // Individual material instance
    // CPU iterates all GameObjects per frame
    // Separate memory allocations
    // Poor cache locality
}

// ECS Entities Graphics (fast at scale)
[GenerateAuthoringComponent]
public struct RenderMeshComponent : IComponentData
{
    public Mesh mesh;                  // Shared mesh reference
    public Material material;          // Shared material reference
    // GPU instancing automatically applied
    // Contiguous memory layout
    // Excellent cache locality
}

public struct LocalToWorld : IComponentData
{
    public float4x4 Value;            // Transform matrix
}
```

**Key Architecture Principles:**

1. **Component-Based Material Assignment**: Materials and meshes are ECS components
2. **Automatic GPU Instancing**: Entities with same mesh+material batched automatically
3. **SRP Batcher Integration**: Minimal CPU overhead for material property blocks
4. **Culling on GPU**: Frustum and occlusion culling performed on GPU
5. **LOD Groups**: Distance-based detail management with component-based LOD levels

### 1.2 Rendering Pipeline Integration

```csharp
// Entities Graphics rendering flow:
// 1. ECS queries gather all renderable entities
// 2. Culling system (GPU) filters visible entities
// 3. LOD system (GPU) selects appropriate detail level
// 4. SRP Batcher groups by material compatibility
// 5. GPU instancing batches identical mesh+material
// 6. Draw calls issued (10-100 vs 10,000 individual)

public class EntitiesGraphicsSystem : SystemBase
{
    protected override void OnUpdate()
    {
        // Automatic rendering - no manual iteration needed
        // System handles:
        // - Culling (frustum + occlusion)
        // - LOD selection
        // - Batching
        // - Material property updates
        // - Transform updates
    }
}
```

---

## 2. GPU Instancing at Scale

### 2.1 Instancing Fundamentals

GPU instancing renders multiple copies of the same mesh with different transforms in a single draw call:

```csharp
// Setup for GPU instancing with Entities Graphics
public struct GeologicalSampleRendering : IComponentData
{
    // Shared across all instances
    public Mesh sampleMesh;           // Single mesh shared by 1000s
    public Material sampleMaterial;   // Single material shared by 1000s
    
    // Per-instance data (automatically batched)
    // - LocalToWorld (transform matrix)
    // - MaterialPropertyOverrides (color, metallic, etc.)
}

// BlueMarble example: 10,000 rock samples
// Traditional: 10,000 draw calls (60-100ms CPU time)
// GPU Instancing: 1-10 draw calls (< 2ms CPU time)
```

**Performance Scaling:**

| Entity Count | Draw Calls (Traditional) | Draw Calls (Instanced) | CPU Time (Traditional) | CPU Time (Instanced) |
|--------------|--------------------------|------------------------|------------------------|----------------------|
| 100          | 100                      | 1-5                    | 1-2ms                  | < 0.5ms              |
| 1,000        | 1,000                    | 5-10                   | 10-15ms                | < 1ms                |
| 10,000       | 10,000                   | 10-50                  | 100-150ms (unplayable) | 1-2ms                |
| 100,000      | 100,000 (crash)          | 50-200                 | N/A                    | 5-10ms               |

### 2.2 Material Property Overrides

```csharp
// Per-instance material variations without breaking batching
public struct MaterialPropertyOverride : IComponentData
{
    public float4 color;              // Per-instance color
    public float metallic;            // Per-instance metallic value
    public float smoothness;          // Per-instance smoothness
}

// BlueMarble example: Rock sample variations
public class RockSampleRenderingSystem : SystemBase
{
    protected override void OnUpdate()
    {
        Entities.ForEach((ref MaterialPropertyOverride props,
                          in GeologicalType type) =>
        {
            // Set properties based on geological type
            // GPU instancing maintained - single draw call
            props.color = GetRockColor(type);
            props.metallic = GetMetallicValue(type);
            props.smoothness = GetSmoothnessValue(type);
        }).ScheduleParallel();
    }
}
```

---

## 3. LOD (Level of Detail) System

### 3.1 Component-Based LOD

```csharp
// LOD group setup with Entities Graphics
public struct LODGroup : IComponentData
{
    public Entity LOD0;               // High detail (0-50m)
    public Entity LOD1;               // Medium detail (50-200m)
    public Entity LOD2;               // Low detail (200-500m)
    public Entity LOD3;               // Billboard (500m+)
}

public struct LODRange : IComponentData
{
    public float MinDistance;         // Start distance for this LOD
    public float MaxDistance;         // End distance for this LOD
}

// BlueMarble example: Research station LOD
// LOD0 (0-50m): Full detail, interior visible, 5000 tris
// LOD1 (50-200m): Exterior only, 1000 tris
// LOD2 (200-500m): Simplified geometry, 200 tris
// LOD3 (500m+): Billboard quad, 2 tris
```

### 3.2 Automatic LOD Selection

```csharp
// LOD system automatically handles transitions
public class LODSystem : SystemBase
{
    protected override void OnUpdate()
    {
        // Parallel LOD selection for 10,000+ entities
        Entities
            .WithName("LOD_Selection")
            .ForEach((ref RenderMesh renderMesh,
                      in LODGroup lodGroup,
                      in LocalToWorld transform) =>
        {
            float distance = CalculateCameraDistance(transform.Position);
            
            // Select appropriate LOD
            if (distance < 50f)
                renderMesh = GetRenderMesh(lodGroup.LOD0);
            else if (distance < 200f)
                renderMesh = GetRenderMesh(lodGroup.LOD1);
            else if (distance < 500f)
                renderMesh = GetRenderMesh(lodGroup.LOD2);
            else
                renderMesh = GetRenderMesh(lodGroup.LOD3);
            
            // GPU instancing maintained per LOD level
            // Automatic culling if too far
        }).ScheduleParallel();
    }
}
```

### 3.3 LOD Performance Budget

```csharp
// BlueMarble LOD strategy for 10,000 geological samples:
// Camera range 0-1000m visibility

// LOD0 (0-25m): 500 entities × 2000 tris = 1M tris (5ms)
// LOD1 (25-100m): 2000 entities × 500 tris = 1M tris (5ms)
// LOD2 (100-300m): 4000 entities × 100 tris = 400K tris (2ms)
// LOD3 (300-1000m): 3500 entities × 10 tris = 35K tris (< 1ms)
// Total visible: 10,000 entities, 2.5M tris, 13ms GPU budget

// Beyond 1000m: Culled (not rendered)
```

---

## 4. SRP Batcher Optimization

### 4.1 SRP Batcher Overview

The Scriptable Render Pipeline (SRP) Batcher eliminates per-object CPU setup overhead:

```csharp
// Traditional rendering (per-object CPU work)
foreach (var renderer in renderers)  // 10,000 iterations
{
    SetupMaterialProperties();        // CPU work per object
    SetupTransformMatrix();           // CPU work per object
    IssueDrawCall();                  // GPU command per object
}
// Total: 10,000 × (CPU setup + GPU command) = 60-100ms CPU

// SRP Batcher (batched CPU work)
SetupShaderData();                    // One-time CPU setup
foreach (var batch in batches)        // 10-50 batches
{
    UpdateTransformBuffer();          // GPU buffer update
    IssueInstancedDrawCall();         // Single GPU command
}
// Total: 1 × CPU setup + 10-50 × GPU command = < 2ms CPU
```

### 4.2 SRP Batcher Requirements

```csharp
// Shader requirements for SRP Batcher compatibility:
Shader "Custom/SRPBatcherCompatible"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Texture", 2D) = "white" {}
    }
    
    SubShader
    {
        HLSLPROGRAM
        // CRITICAL: Use CBUFFER for material properties
        CBUFFER_START(UnityPerMaterial)
            float4 _Color;
            float4 _MainTex_ST;
        CBUFFER_END
        
        // CRITICAL: Use CBUFFER for per-instance data
        CBUFFER_START(UnityPerDraw)
            float4x4 unity_ObjectToWorld;
            float4x4 unity_WorldToObject;
        CBUFFER_END
        
        // Shader code using these properties
        // SRP Batcher automatically batches compatible materials
        ENDHLSL
    }
}
```

---

## 5. GPU Culling

### 5.1 Frustum Culling on GPU

```csharp
// Traditional CPU frustum culling (slow)
public class CPUFrustumCulling
{
    public void CullObjects(Camera camera, List<GameObject> objects)
    {
        foreach (var obj in objects)  // 10,000 CPU iterations
        {
            if (IsBoundsVisible(camera, obj.bounds))
                obj.SetActive(true);
            else
                obj.SetActive(false);
        }
        // 10,000 objects = 5-10ms CPU time
    }
}

// Entities Graphics GPU culling (fast)
// Culling happens on GPU automatically
// - Compute shader checks bounds visibility
// - Parallel processing on 1000s of GPU threads
// - Only visible entities sent to rendering
// - CPU overhead: < 0.1ms
// - GPU overhead: < 0.5ms

// No manual culling code needed - automatic!
```

### 5.2 Occlusion Culling Integration

```csharp
// Occlusion culling setup for Entities Graphics
public struct OcclusionCullingData : IComponentData
{
    public AABB bounds;               // Bounding box for culling
    public bool isOccluder;           // Can this object occlude others?
}

// BlueMarble example: Terrain occludes geological samples
// - Large terrain chunks marked as occluders
// - GPU occlusion query tests sample visibility
// - Occluded samples not rendered
// - Performance: 10,000 samples → 3,000 visible after occlusion
```

---

## 6. BlueMarble-Specific Rendering Applications

### 6.1 Geological Sample Rendering

```csharp
// 10,000+ geological samples across 100km world
[GenerateAuthoringComponent]
public struct GeologicalSampleRendering : IComponentData
{
    public Mesh sampleMesh;           // Shared rock mesh
    public Material rockMaterial;     // Shared rock material
}

public struct SampleProperties : IComponentData
{
    public GeologicalType type;       // Granite, basalt, limestone, etc.
    public float weathering;          // 0-1 weathering factor
    public float3 color;              // Rock color variation
}

public class GeologicalSampleRenderingSystem : SystemBase
{
    protected override void OnUpdate()
    {
        // Update material properties based on sample type
        Entities
            .WithName("UpdateSampleRendering")
            .ForEach((ref MaterialPropertyOverride props,
                      in SampleProperties sample) =>
        {
            // Set color based on geological type
            props.color = GetGeologicalColor(sample.type);
            
            // Apply weathering effects
            props.smoothness = math.lerp(0.8f, 0.3f, sample.weathering);
            props.metallic = math.lerp(0.2f, 0.0f, sample.weathering);
            
            // GPU instancing maintained
            // Single draw call for all granite samples
            // Single draw call for all basalt samples, etc.
        }).ScheduleParallel();
    }
}

// Performance: 10,000 samples, 20 geological types
// Draw calls: 20 (one per type)
// CPU time: < 1ms
// GPU time: 5-8ms
```

### 6.2 Procedural Terrain Integration

```csharp
// Procedurally generated terrain with Entities Graphics
public struct TerrainChunkRendering : IComponentData
{
    public Mesh proceduralMesh;       // Runtime-generated mesh
    public Material terrainMaterial;  // Shared terrain material
}

public class ProceduralTerrainRenderingSystem : SystemBase
{
    protected override void OnUpdate()
    {
        // Generate terrain meshes procedurally
        Entities
            .WithName("GenerateTerrainChunks")
            .WithoutBurst()  // Mesh creation not burstable
            .ForEach((Entity entity,
                      ref TerrainChunkRendering rendering,
                      in ChunkCoordinates coords) =>
        {
            // Generate mesh based on heightmap and coordinates
            rendering.proceduralMesh = GenerateChunkMesh(coords);
            
            // Add to render mesh component
            var renderMesh = new RenderMesh
            {
                mesh = rendering.proceduralMesh,
                material = rendering.terrainMaterial,
                castShadows = ShadowCastingMode.On,
                receiveShadows = true
            };
            
            EntityManager.SetSharedComponentData(entity, renderMesh);
            
            // GPU instancing for chunks with same material
        }).Run();
    }
}

// BlueMarble: 100 terrain chunks (10km × 10km world)
// Each chunk: 10,000 tris
// Total: 1M tris visible
// Draw calls: 10-20 (LOD + material variations)
// CPU time: < 2ms
// GPU time: 8-12ms
```

### 6.3 Dynamic Lighting for Large Worlds

```csharp
// Dynamic lighting setup with Entities Graphics
public struct DynamicLightComponent : IComponentData
{
    public float3 position;           // Light world position
    public float3 color;              // Light color
    public float intensity;           // Light intensity
    public float range;               // Light range
}

public class DynamicLightingSystem : SystemBase
{
    protected override void OnUpdate()
    {
        // Update dynamic lights (day/night cycle, player lights, etc.)
        Entities
            .WithName("UpdateDynamicLights")
            .ForEach((ref DynamicLightComponent light,
                      in LocalToWorld transform) =>
        {
            // Update light position based on transform
            light.position = transform.Position;
            
            // Time-of-day lighting variations
            float timeOfDay = GetTimeOfDay();
            light.intensity = CalculateSunIntensity(timeOfDay);
            light.color = CalculateSunColor(timeOfDay);
            
            // Entities Graphics automatically updates shader light data
        }).ScheduleParallel();
    }
}

// BlueMarble lighting:
// - 1 directional light (sun)
// - 10-20 point lights (research stations, vehicles)
// - 100+ spot lights (player flashlights, vehicle headlights)
// Culling: Only lights affecting visible geometry processed
// Performance: < 1ms CPU overhead
```

### 6.4 Research Station Rendering with LOD

```csharp
// Research station with multiple LOD levels
public struct ResearchStationRendering : IComponentData
{
    // LOD0: Full detail with interior (5000 tris)
    public Mesh detailedExterior;
    public Mesh detailedInterior;
    public Material stationMaterial;
    
    // LOD1: Exterior only (1000 tris)
    public Mesh simplifiedExterior;
    
    // LOD2: Low detail (200 tris)
    public Mesh lowPolyStation;
    
    // LOD3: Billboard (2 tris)
    public Mesh billboardQuad;
}

public class ResearchStationLODSystem : SystemBase
{
    protected override void OnUpdate()
    {
        float3 cameraPos = GetCameraPosition();
        
        Entities
            .WithName("ResearchStationLOD")
            .ForEach((ref RenderMesh renderMesh,
                      in ResearchStationRendering station,
                      in LocalToWorld transform) =>
        {
            float distance = math.distance(cameraPos, transform.Position);
            
            if (distance < 50f)
            {
                // LOD0: Show exterior + interior
                renderMesh.mesh = station.detailedExterior;
                // Interior rendered separately with interior entities
            }
            else if (distance < 200f)
            {
                // LOD1: Exterior only
                renderMesh.mesh = station.simplifiedExterior;
            }
            else if (distance < 500f)
            {
                // LOD2: Low poly
                renderMesh.mesh = station.lowPolyStation;
            }
            else
            {
                // LOD3: Billboard
                renderMesh.mesh = station.billboardQuad;
            }
            
            renderMesh.material = station.stationMaterial;
        }).ScheduleParallel();
    }
}

// BlueMarble: 100 research stations across world
// Simultaneous LOD transitions for all stations
// Performance: < 0.5ms CPU overhead
```

---

## 7. Performance Optimization Strategies

### 7.1 Material Batching

```csharp
// Strategy: Minimize material variations to maximize batching
public enum RockMaterialType
{
    Igneous,      // Granite, basalt, obsidian
    Sedimentary,  // Limestone, sandstone, shale
    Metamorphic   // Marble, slate, gneiss
}

// Use material property overrides for variations within type
// Result: 3 draw calls (one per type) instead of 20 (one per rock)

public class MaterialBatchingSystem : SystemBase
{
    protected override void OnUpdate()
    {
        Entities.ForEach((ref MaterialPropertyOverride props,
                          in GeologicalType type) =>
        {
            // Set base material type
            RockMaterialType materialType = GetMaterialType(type);
            
            // Use property overrides for specific variations
            props.color = GetTypeSpecificColor(type);
            props.metallic = GetTypeSpecificMetallic(type);
            
            // GPU instancing maintained within material type
        }).ScheduleParallel();
    }
}
```

### 7.2 Frustum Culling Optimization

```csharp
// Strategy: Hierarchical bounds for efficient culling
public struct HierarchicalBounds : IComponentData
{
    public AABB localBounds;          // Object-local bounds
    public AABB worldBounds;          // World-space bounds (updated)
}

// Parent entities have bounds encompassing children
// Cull parent → automatically culls all children
// Performance: O(log n) instead of O(n)

public class HierarchicalCullingSystem : SystemBase
{
    protected override void OnUpdate()
    {
        // Update world bounds
        Entities
            .WithName("UpdateWorldBounds")
            .ForEach((ref HierarchicalBounds bounds,
                      in LocalToWorld transform) =>
        {
            bounds.worldBounds = TransformBounds(bounds.localBounds, transform.Value);
        }).ScheduleParallel();
        
        // GPU culling uses hierarchical bounds
        // Performance: 10,000 objects → 1,000 bound checks → 500 rendered
    }
}
```

### 7.3 LOD Hysteresis

```csharp
// Strategy: Prevent LOD "popping" with hysteresis
public struct LODHysteresis : IComponentData
{
    public int currentLOD;            // Current LOD level
    public float transitionBuffer;    // Distance buffer (5-10%)
}

public class LODHysteresisSystem : SystemBase
{
    protected override void OnUpdate()
    {
        Entities.ForEach((ref RenderMesh renderMesh,
                          ref LODHysteresis lod,
                          in LODGroup lodGroup,
                          in LocalToWorld transform) =>
        {
            float distance = CalculateCameraDistance(transform.Position);
            
            // Add hysteresis buffer to prevent oscillation
            float buffer = lod.transitionBuffer;
            
            if (lod.currentLOD == 0 && distance > 50f + buffer)
            {
                // Transition to LOD1
                renderMesh = GetRenderMesh(lodGroup.LOD1);
                lod.currentLOD = 1;
            }
            else if (lod.currentLOD == 1 && distance < 50f - buffer)
            {
                // Transition back to LOD0
                renderMesh = GetRenderMesh(lodGroup.LOD0);
                lod.currentLOD = 0;
            }
            // ... similar logic for other LOD levels
        }).ScheduleParallel();
    }
}
```

---

## 8. Integration with Other Systems

### 8.1 Physics Integration

```csharp
// Rendering and physics share same entity
public struct RenderAndPhysicsEntity
{
    // Rendering components
    public RenderMesh renderMesh;
    public MaterialPropertyOverride materialProps;
    public LocalToWorld renderTransform;
    
    // Physics components
    public PhysicsCollider collider;
    public PhysicsVelocity velocity;
    public PhysicsMass mass;
}

// Single source of truth for position
// Physics updates LocalToWorld → rendering automatically updates
```

### 8.2 AI Integration

```csharp
// AI agents use rendering for visual representation
public struct AIAgentRendering
{
    // AI components
    public BehaviorTreeState aiState;
    public NavigationAgent navigation;
    
    // Rendering components
    public RenderMesh characterMesh;
    public AnimationClip currentAnimation;
    
    // Shared transform
    public LocalToWorld worldTransform;
}

// Animation updates mesh deformation
// Rendering shows current animation state
// LOD system reduces AI visual complexity at distance
```

### 8.3 Networking Integration

```csharp
// Networked entities with rendering
[GhostComponent(PrefabType = GhostPrefabType.All)]
public struct NetworkedRendering : IComponentData
{
    // Networked data
    [GhostField] public float3 position;
    [GhostField] public quaternion rotation;
    
    // Local rendering (not networked)
    public RenderMesh renderMesh;
    public MaterialPropertyOverride materialProps;
}

// Server: Updates position/rotation
// Clients: Receive updates + local rendering
// Rendering LOD applied locally (not networked)
```

---

## 9. Implementation Roadmap for BlueMarble

### Phase 1: Basic Entities Graphics Setup (2 weeks)

**Week 1: Foundation**
- Install Entities Graphics package
- Create basic RenderMesh components
- Setup shared materials and meshes
- Test rendering 1,000 simple entities

**Week 2: GPU Instancing**
- Implement material property overrides
- Setup SRP Batcher compatible shaders
- Test rendering 10,000 instanced entities
- Measure performance baseline

**Deliverables:**
- Working Entities Graphics rendering for 10,000+ entities
- SRP Batcher optimized shaders
- Performance metrics documented

### Phase 2: LOD System (2 weeks)

**Week 3: LOD Implementation**
- Create LOD component structure
- Implement automatic LOD selection system
- Setup 3-4 LOD levels for each object type
- Test LOD transitions

**Week 4: LOD Optimization**
- Implement LOD hysteresis to prevent popping
- Optimize LOD distance calculations
- Profile LOD system performance
- Fine-tune LOD distances for BlueMarble

**Deliverables:**
- Complete LOD system for all renderable types
- Smooth LOD transitions without popping
- Performance budget: < 0.5ms CPU overhead

### Phase 3: BlueMarble Integration (3 weeks)

**Week 5: Geological Sample Rendering**
- Implement GeologicalSampleRendering component
- Setup material variations for rock types
- Integrate with sample collection system
- Test 10,000 samples across world

**Week 6: Terrain and Environment**
- Implement procedural terrain rendering
- Setup dynamic lighting system
- Integrate weather effects with rendering
- Test 100km world rendering

**Week 7: Research Stations and Structures**
- Implement ResearchStationRendering with LOD
- Setup interior/exterior rendering
- Integrate with player interaction system
- Test multiple stations with interiors

**Deliverables:**
- Complete rendering for all BlueMarble object types
- 100km world rendering at 60 FPS
- 10,000+ geological samples rendered

### Phase 4: Polish and Optimization (1 week)

**Week 8: Final Optimization**
- Profile and optimize critical rendering paths
- Implement hierarchical culling
- Fine-tune material batching
- Load testing with maximum entity counts

**Deliverables:**
- Fully optimized rendering pipeline
- Performance targets met (60 FPS with 10,000+ entities)
- Documentation and handoff

---

## 10. Performance Targets

### Target Frame Budget (60 FPS = 16.67ms)

| System | Budget | Current | Status |
|--------|--------|---------|--------|
| Rendering (Entities Graphics) | 5-8ms | TBD | Target |
| - SRP Batcher overhead | < 1ms | TBD | Target |
| - GPU instancing | < 1ms | TBD | Target |
| - LOD system | < 0.5ms | TBD | Target |
| - Culling | < 0.5ms | TBD | Target |
| - Material updates | < 1ms | TBD | Target |
| - GPU rendering | 5-6ms | TBD | Target |

### Scalability Targets

| Metric | Target | Notes |
|--------|--------|-------|
| Rendered entities | 10,000+ | At 60 FPS stable |
| Draw calls | 50-200 | Via instancing + batching |
| Triangle budget | 2-5M | With LOD optimization |
| Texture memory | 2-4 GB | With streaming |
| Material count | 20-50 | Minimize for batching |

---

## 11. Discovered Sources for Phase 4

### Rendering & Graphics

1. **Unity Shader Graph for DOTS**
   - Priority: Medium
   - Effort: 4-6 hours
   - Visual shader authoring for Entities Graphics
   - Material property block integration

2. **GPU Occlusion Culling Techniques**
   - Priority: High
   - Effort: 6-8 hours
   - Advanced occlusion culling strategies
   - Hi-Z buffer techniques for massive scenes

---

## 12. Conclusion

Unity Entities Graphics (Hybrid Renderer V2) provides the rendering foundation for BlueMarble's planetary-scale simulation. The combination of GPU instancing, SRP Batcher, automatic LOD, and GPU culling enables rendering 10,000+ entities at 60 FPS—impossible with traditional GameObject rendering.

**Key Implementation Points for BlueMarble:**

1. **Geological Samples**: Use GPU instancing with material property overrides for 10,000+ rock samples
2. **Terrain**: Procedural mesh generation with ECS rendering for 100km world
3. **Research Stations**: Multi-level LOD (interior + exterior) for detailed structures
4. **Performance**: Target 5-8ms rendering budget, achievable with optimizations
5. **Integration**: Seamless integration with physics, AI, and networking systems

The 8-week implementation roadmap provides a clear path from basic setup to full BlueMarble integration with all systems working together at massive scale.

---

## References

- Unity Entities Graphics Package Documentation
- Unity SRP Batcher Technical Guide
- GPU Instancing Best Practices
- LOD System Design Patterns
- BlueMarble Phase 3 Architecture Documents

**Cross-References:**
- See "Unity DOTS - ECS for Agents" for ECS fundamentals
- See "Unity ECS/DOTS Documentation" for system architecture
- See "Building Open Worlds Collection" for LOD strategies
- See "Naughty Dog Engine Architecture" for frame graph integration

---

**Document Status:** Complete  
**Total Lines:** 900+  
**Analysis Depth:** Comprehensive  
**BlueMarble Applicability:** High - Critical for rendering 10,000+ entities  
**Implementation Ready:** Yes - 8-week roadmap provided
