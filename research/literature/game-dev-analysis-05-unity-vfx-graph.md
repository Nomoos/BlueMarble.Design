# Unity VFX Graph - Analysis for BlueMarble MMORPG

---
title: Unity VFX Graph Documentation - Node-Based VFX Workflow Analysis
date: 2025-01-15
tags: [vfx, unity, node-based, gpu-driven, visual-programming, workflow]
status: complete
priority: medium
parent-research: game-dev-analysis-vfx-compositing.md
---

**Source:** Unity VFX Graph Official Documentation  
**Category:** GameDev-Tech  
**Priority:** Medium  
**Status:** ✅ Complete  
**Lines:** 500+  
**Related Sources:** GPU Gems Series, Unreal Engine Niagara, Visual Effects and Compositing

---

## Executive Summary

This analysis examines Unity's VFX Graph system, a modern node-based visual effects creation tool that leverages compute shaders for GPU-driven particle simulation. While BlueMarble uses a custom engine, Unity's VFX Graph demonstrates production-proven patterns for artist-friendly VFX workflows, GPU-driven simulation architectures, and performance optimization strategies that can be adapted for BlueMarble's geological and MMORPG VFX needs.

**Key Takeaways for BlueMarble:**
- Node-based authoring reduces VFX iteration time from hours to minutes
- Compute shader-driven simulation enables millions of particles
- Event system allows complex particle behaviors without CPU overhead
- Spawner/Context/Block architecture provides clear separation of concerns
- GPU event culling system automatically manages particle budgets
- Shader Graph integration enables custom particle rendering
- Instanced rendering reduces draw calls by 90%+

---

## Part I: VFX Graph Architecture

### 1. Core Concepts

**System Structure:**

```
VFX Graph
├── Spawners (When to create particles)
├── Initialize Context (Set initial particle state)
├── Update Context (Per-frame particle logic)
├── Output Context (Rendering configuration)
└── Event System (Inter-context communication)
```

**Node-Based Workflow:**

```cpp
// Conceptual representation of VFX Graph structure
class VFXGraph {
public:
    struct Node {
        NodeType type;
        std::vector<Input> inputs;
        std::vector<Output> outputs;
        std::function<void()> execute;
    };
    
    struct Context {
        ContextType type;  // Spawner, Initialize, Update, Output
        std::vector<Node*> nodes;
        ComputeShader* shader;  // Generated from nodes
    };
    
    std::vector<Context> contexts;
    EventSystem eventSystem;
    
    void Compile() {
        // Generate compute shaders from node graph
        for (auto& context : contexts) {
            context.shader = GenerateComputeShader(context.nodes);
        }
    }
    
    void Execute() {
        // Dispatch compute shaders
        for (auto& context : contexts) {
            context.shader->Dispatch();
        }
    }
};
```

**BlueMarble Adaptation:**

```cpp
class BlueMarbleVFXGraph {
public:
    // Implement similar node-based system for geological VFX
    
    struct GeologicalVFXNode {
        enum class Type {
            EarthquakeMagnitude,
            LavaTemperature,
            ErosionRate,
            WindVelocity,
            ParticleSpawn,
            ParticleUpdate,
            ParticleRender
        };
        
        Type type;
        std::map<std::string, float> inputs;
        std::map<std::string, float> outputs;
    };
    
    void CreateEarthquakeVFX(float magnitude) {
        VFXGraph graph;
        
        // Spawner: dust particles based on magnitude
        auto spawner = graph.AddSpawner();
        spawner->SetSpawnRate(magnitude * 10000.0f);
        spawner->SetBurst(true);
        
        // Initialize: particle properties
        auto initialize = graph.AddInitialize();
        initialize->SetLifetime(5.0f + magnitude);
        initialize->SetVelocity({0, magnitude * 3.0f, 0});
        initialize->SetSize(0.1f, 0.5f);
        initialize->SetColor({0.6, 0.5, 0.4, 0.8});
        
        // Update: gravity and turbulence
        auto update = graph.AddUpdate();
        update->AddForce({0, -9.8f, 0});  // Gravity
        update->AddTurbulence(magnitude * 2.0f);
        
        // Output: render as billboards
        auto output = graph.AddOutput();
        output->SetRenderMode(RenderMode::Billboard);
        output->SetBlendMode(BlendMode::Additive);
        
        graph.Compile();
        graph.Execute();
    }
};
```

---

## Part II: GPU Event System

### 2. Event-Driven Particle Behaviors

**Unity VFX Graph Events:**

Events allow particles to communicate and trigger behaviors without CPU involvement:

```cpp
class GPUEventSystem {
public:
    struct Event {
        uint32_t eventID;
        vec3 position;
        vec4 attributes;  // Custom data
    };
    
    // GPU buffer storing events
    GPUBuffer eventBuffer;
    
    // Compute shader reads/writes events
    void SpawnEventOnGPU() {
        // In compute shader:
        // if (particle.collided) {
        //     WriteEvent(COLLISION_EVENT, particle.position);
        // }
    }
    
    void HandleEventsOnGPU() {
        // Different context reads events:
        // Event e = ReadEvent();
        // if (e.eventID == COLLISION_EVENT) {
        //     SpawnDebrisParticles(e.position);
        // }
    }
};
```

**BlueMarble Geological Events:**

```glsl
// Compute Shader: Earthquake Event System
#version 450

layout (local_size_x = 256) in;

struct Particle {
    vec3 position;
    vec3 velocity;
    float life;
    uint eventFlags;
};

struct GeologicalEvent {
    uint eventType;
    vec3 epicenter;
    float magnitude;
    float timestamp;
};

layout(std430, binding = 0) buffer ParticleBuffer {
    Particle particles[];
};

layout(std430, binding = 1) buffer EventBuffer {
    GeologicalEvent events[];
};

layout(binding = 0, offset = 0) uniform atomic_uint eventCount;

const uint EVENT_GROUND_IMPACT = 1;
const uint EVENT_ROCK_BREAK = 2;
const uint EVENT_DUST_SPAWN = 3;

void main() {
    uint id = gl_GlobalInvocationID.x;
    Particle p = particles[id];
    
    // Check for ground collision
    if (p.position.y < 0.0 && p.velocity.y < 0.0) {
        // Write event to buffer
        uint eventIdx = atomicCounterIncrement(eventCount);
        
        events[eventIdx].eventType = EVENT_GROUND_IMPACT;
        events[eventIdx].epicenter = p.position;
        events[eventIdx].magnitude = length(p.velocity);
        events[eventIdx].timestamp = currentTime;
        
        // Spawn dust on impact
        if (length(p.velocity) > 5.0) {
            uint dustEventIdx = atomicCounterIncrement(eventCount);
            events[dustEventIdx].eventType = EVENT_DUST_SPAWN;
            events[dustEventIdx].epicenter = p.position;
            events[dustEventIdx].magnitude = length(p.velocity) * 0.1;
        }
        
        // Bounce particle
        p.position.y = 0.0;
        p.velocity.y *= -0.3;  // Damping
    }
    
    particles[id] = p;
}
```

---

## Part III: Attribute System

### 3. Custom Particle Attributes

**Unity's Flexible Attribute System:**

```cpp
class ParticleAttributes {
public:
    // Standard attributes
    struct StandardAttributes {
        vec3 position;
        vec3 velocity;
        vec4 color;
        float lifetime;
        float age;
        float size;
    };
    
    // Custom attributes (defined per effect)
    struct CustomAttributes {
        float temperature;     // For lava particles
        float erosionRate;     // For water particles
        vec3 windInfluence;    // For debris
        uint rockType;         // For geological particles
    };
    
    // Attributes stored in structured buffer
    GPUBuffer attributeBuffer;
};
```

**BlueMarble Geological Attributes:**

```glsl
// Compute Shader: Lava Particle with Custom Attributes
#version 450

struct LavaParticle {
    // Standard
    vec3 position;
    vec3 velocity;
    vec4 color;
    float life;
    float size;
    
    // Custom geological attributes
    float temperature;      // 700-1200°C
    float viscosity;        // Affects flow behavior
    float coolingRate;      // How fast it solidifies
    bool isSolidified;      // Become rock when cooled
};

layout(std430, binding = 0) buffer LavaBuffer {
    LavaParticle lava[];
};

uniform float ambientTemperature;
uniform float deltaTime;

void main() {
    uint id = gl_GlobalInvocationID.x;
    LavaParticle p = lava[id];
    
    if (p.isSolidified) return;
    
    // Cool over time
    p.temperature -= p.coolingRate * deltaTime;
    
    if (p.temperature < 600.0) {
        // Solidify into rock
        p.isSolidified = true;
        p.velocity = vec3(0.0);
        p.color = vec4(0.3, 0.2, 0.2, 1.0);  // Dark rock
        
        // Spawn "rock created" event for geological simulation
        SpawnRockCreatedEvent(p.position);
    } else {
        // Still liquid - flow behavior
        float flowSpeed = 1.0 / p.viscosity;
        p.velocity.y -= 9.8 * deltaTime;  // Gravity
        p.position += p.velocity * flowSpeed * deltaTime;
        
        // Color based on temperature
        float heat = (p.temperature - 600.0) / 600.0;
        p.color = mix(vec4(0.5, 0.2, 0.1, 1.0),  // Cooling
                     vec4(1.0, 0.6, 0.1, 1.0),   // Hot
                     heat);
    }
    
    lava[id] = p;
}
```

---

## Part IV: Output Rendering Modes

### 4. Flexible Rendering Options

**Unity VFX Graph Output Modes:**

```cpp
enum class OutputMode {
    Billboard,           // Camera-facing quads
    OrientedBillboard,   // Aligned to velocity
    Mesh,               // Instanced mesh rendering
    Line,               // Particle trails
    Strip,              // Connected particle strip
};

class ParticleOutput {
public:
    void RenderParticles(OutputMode mode) {
        switch (mode) {
            case OutputMode::Billboard:
                RenderBillboards();
                break;
            case OutputMode::Mesh:
                RenderInstancedMeshes();
                break;
            case OutputMode::Line:
                RenderTrails();
                break;
        }
    }
    
    void RenderInstancedMeshes() {
        // Each particle renders a full 3D mesh
        // Efficient instanced rendering
        
        std::vector<mat4> instanceMatrices;
        for (auto& particle : particles) {
            mat4 transform = CalculateTransform(particle);
            instanceMatrices.push_back(transform);
        }
        
        meshInstanceBuffer->Upload(instanceMatrices);
        glDrawElementsInstanced(GL_TRIANGLES, 
                               mesh->indexCount,
                               GL_UNSIGNED_INT, 
                               0,
                               instanceMatrices.size());
    }
};
```

**BlueMarble Output Applications:**

```cpp
class GeologicalOutputModes {
public:
    void RenderRockDebris() {
        // Use mesh rendering for rock chunks
        output.SetMode(OutputMode::Mesh);
        output.SetMesh("rock_chunk.mesh");
        output.EnableShadowCasting(true);
        output.EnableCollision(true);
    }
    
    void RenderDust() {
        // Use billboards for dust clouds
        output.SetMode(OutputMode::Billboard);
        output.SetTexture("dust_particle.png");
        output.SetBlendMode(BlendMode::Additive);
        output.EnableSoftParticles(true);
    }
    
    void RenderLavaFlow() {
        // Use strips for flowing lava
        output.SetMode(OutputMode::Strip);
        output.SetWidth(0.5f);
        output.SetEmissive(true);
        output.SetEmissionIntensity(10.0f);
    }
    
    void RenderLightning() {
        // Use lines for lightning bolts
        output.SetMode(OutputMode::Line);
        output.SetWidth(0.1f);
        output.SetColor({1.0, 1.0, 0.8, 1.0});
        output.SetEmissive(true);
    }
};
```

---

## Part V: Shader Graph Integration

### 5. Custom Particle Shading

**Unity's Shader Graph Integration:**

VFX Graph particles can use custom shaders created in Shader Graph, enabling:
- Per-particle PBR shading
- Custom UV distortion
- Noise-based patterns
- Emissive animations

```cpp
class CustomParticleShader {
public:
    // Generated from Shader Graph
    void FragmentShader() {
        // Sample custom attributes from particle
        float temperature = GetParticleAttribute("temperature");
        float erosion = GetParticleAttribute("erosion");
        
        // Custom shading logic
        vec3 baseColor = SampleTexture(albedoMap, uv);
        vec3 emission = vec3(temperature / 1200.0, 0.3, 0.1);
        float alpha = erosion > 0.5 ? 0.5 : 1.0;
        
        // Output
        fragColor = vec4(baseColor + emission, alpha);
    }
};
```

**BlueMarble Custom Shaders:**

```glsl
// Fragment Shader: Geological Particle
#version 450

in vec2 texCoord;
in vec4 particleColor;
in float particleTemperature;
in float particleLife;

out vec4 fragColor;

uniform sampler2D particleTexture;
uniform float time;

void main() {
    vec4 tex = texture(particleTexture, texCoord);
    
    // Temperature-based emission
    vec3 emission = vec3(0.0);
    if (particleTemperature > 600.0) {
        float heat = (particleTemperature - 600.0) / 600.0;
        emission = vec3(1.0, 0.5, 0.1) * heat * 5.0;
    }
    
    // Fade over lifetime
    float fade = particleLife / maxLife;
    
    // Animated noise for organic feel
    float noise = fbm(texCoord * 5.0 + time * 0.1);
    
    vec3 finalColor = particleColor.rgb * tex.rgb + emission;
    float finalAlpha = tex.a * particleColor.a * fade * (0.8 + noise * 0.2);
    
    fragColor = vec4(finalColor, finalAlpha);
}
```

---

## Part VI: Performance Optimizations

### 6. VFX Graph Built-in Optimizations

**Automatic Optimizations:**

1. **GPU Culling:** Particles outside frustum automatically disabled
2. **Instanced Rendering:** All particles in batch use single draw call
3. **Compute Shader Dispatch:** Optimal thread group sizes
4. **Memory Pooling:** Reuse particle buffers
5. **LOD System:** Automatic quality reduction with distance

```cpp
class VFXGraphOptimizations {
public:
    // Automatic frustum culling (no CPU overhead)
    void GPUFrustumCull() {
        // In compute shader:
        // if (!IsInFrustum(particle.position, frustumPlanes)) {
        //     particle.active = false;
        // }
    }
    
    // Automatic LOD based on screen coverage
    void AutomaticLOD() {
        // Calculate screen space size
        float screenSize = CalculateScreenSize(particle);
        
        if (screenSize < 2.0) {
            // Too small - disable
            particle.active = false;
        } else if (screenSize < 10.0) {
            // Low detail
            particle.updateRate = 15;  // 15 FPS update
        } else {
            // Full detail
            particle.updateRate = 60;  // 60 FPS update
        }
    }
    
    // Instanced rendering
    void InstancedDraw() {
        // Single draw call for all particles
        // GPU automatically handles per-instance data
        DrawParticlesInstanced(particleCount);
    }
};
```

**BlueMarble Performance Strategies:**

```cpp
class BlueMarbleVFXPerformance {
public:
    void OptimizeForMMORPG() {
        // Combine VFX Graph techniques with MMORPG-specific needs
        
        // 1. Player-based culling (like Guild Wars 2)
        EnablePlayerCountScaling(true);
        
        // 2. GPU frustum culling (like VFX Graph)
        EnableGPUCulling(true);
        
        // 3. Geological event priority
        SetGeologicalPriority({
            {EarthquakeMagnitude > 7.0, VFXPriority::Critical},
            {EarthquakeMagnitude > 5.0, VFXPriority::High},
            {EarthquakeMagnitude < 5.0, VFXPriority::Low}
        });
        
        // 4. Automatic instancing
        EnableInstancing(true);
        
        // 5. Compute shader-driven updates
        UseComputeShaders(true);
    }
};
```

---

## Part VII: Workflow Benefits

### 7. Artist-Friendly Authoring

**Key Workflow Improvements:**

1. **Visual Programming:** No code required for complex effects
2. **Real-time Preview:** See changes immediately
3. **Iterative Design:** Tweak values without recompilation
4. **Template System:** Reusable effect components
5. **Version Control Friendly:** Graph stored as text asset

**BlueMarble Workflow Adoption:**

```cpp
class VFXEditorTool {
public:
    // Build visual editor for VFX artists
    
    void CreateGraphEditor() {
        // Node-based editor UI
        ImGui::Begin("VFX Graph Editor");
        
        // Node palette
        if (ImGui::BeginMenu("Add Node")) {
            if (ImGui::MenuItem("Spawner")) AddSpawnerNode();
            if (ImGui::MenuItem("Force")) AddForceNode();
            if (ImGui::MenuItem("Collision")) AddCollisionNode();
            if (ImGui::MenuItem("Event")) AddEventNode();
            ImGui::EndMenu();
        }
        
        // Canvas
        DrawNodeCanvas();
        
        // Properties panel
        DrawSelectedNodeProperties();
        
        // Preview
        DrawEffectPreview();
        
        ImGui::End();
    }
    
    void LivePreview() {
        // Real-time preview while editing
        if (graphChanged) {
            RecompileGraph();
            RestartEffect();
        }
    }
};
```

---

## Part VIII: Lessons for BlueMarble

### 8. Key Takeaways

**Adopt:**
1. ✅ Compute shader-driven simulation (already using)
2. ✅ GPU event system for particle communication
3. ✅ Custom attribute system for geological properties
4. ✅ Instanced rendering for performance
5. ✅ Node-based authoring for artist workflow

**Adapt:**
1. 🔄 Unity's event system → BlueMarble geological events
2. 🔄 Output modes → geological-specific renderers
3. 🔄 Shader Graph → custom particle shader system
4. 🔄 LOD system → player-count-aware LOD (Guild Wars 2 style)

**Skip:**
1. ❌ Unity-specific runtime (use custom engine)
2. ❌ Component system (already have ECS)

---

## Discovered Sources

During this research, the following source was identified:

**Source Name:** Houdini VFX for Games  
**Priority:** Low  
**Category:** GameDev-Content  
**Rationale:** Professional VFX creation tool used in AAA game development. Offers procedural VFX generation and simulation capabilities that could inform BlueMarble's geological effect authoring.  
**Estimated Effort:** 6-8 hours

---

## References

### Primary Source

1. **Unity VFX Graph Documentation**
   - URL: <https://docs.unity3d.com/Packages/com.unity.visualeffectgraph@latest>
   - Unity Technologies
   - Version: 2023.x

### Related BlueMarble Research

- [game-dev-analysis-vfx-compositing.md](game-dev-analysis-vfx-compositing.md) - VFX fundamentals
- [game-dev-analysis-02-gpu-gems.md](game-dev-analysis-02-gpu-gems.md) - GPU particle systems
- [game-dev-analysis-04-guild-wars-2-vfx.md](game-dev-analysis-04-guild-wars-2-vfx.md) - MMORPG VFX

### External Resources

- Unity Blog: VFX Graph Technical Deep Dives
- Unity Learn: VFX Graph Tutorials
- GitHub: Unity VFX Graph Samples

---

**Document Status:** Complete  
**Last Updated:** 2025-01-15  
**Total Lines:** 530+  
**Next Steps:** Design node-based VFX editor, implement GPU event system, create geological VFX templates

**Implementation Priority:**
1. GPU event system (High - enables complex behaviors)
2. Custom attribute system (High - geological properties)
3. Node-based editor (Medium - artist workflow)
4. Output mode variety (Medium - visual diversity)
5. Shader Graph integration (Low - can use existing shaders initially)
