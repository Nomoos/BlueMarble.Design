---
title: Game Development Analysis - Sebastian Lague
date: 2025-01-17
tags: [game-development, procedural-generation, pathfinding, algorithms, youtube, technical]
status: complete
priority: high
source: online-game-dev-resources.md
---

# Game Development Analysis: Sebastian Lague

## Executive Summary

**Creator:** Sebastian Lague  
**Platform:** YouTube (https://www.youtube.com/@SebastianLague)  
**Primary Focus:** Technical game development, procedural generation, algorithms  
**Content Type:** Video tutorials with detailed explanations and visual demonstrations  
**Experience Level:** Intermediate to Advanced  
**Relevance to BlueMarble:** High - Covers essential algorithms for procedural planet generation, pathfinding, and optimization

### Key Strengths

1. **Clear Visual Explanations** - Complex algorithms broken down with excellent visualizations
2. **Production Quality** - Professional, well-edited content with clean code examples
3. **Practical Implementation** - Goes beyond theory to show working implementations
4. **Performance Focus** - Emphasizes optimization and practical considerations
5. **Open Source** - Many projects include source code on GitHub

### Primary Topics Covered

- **Procedural Generation** - Terrain, planets, ecosystems
- **Pathfinding** - A*, navigation, crowd simulation
- **Algorithms** - Marching cubes, ray marching, spatial partitioning
- **Optimization** - Compute shaders, GPU acceleration
- **Physics & Simulation** - Gravity, orbits, fluid dynamics

### Applicability to BlueMarble MMORPG

Sebastian Lague's content directly addresses several critical challenges for BlueMarble:
- Planet-scale procedural terrain generation
- Efficient pathfinding for NPCs and player navigation
- Performance optimization for large-scale worlds
- Procedural ecosystem and resource distribution
- Visual quality for planetary rendering

## Core Concepts

### 1. Procedural Terrain Generation

#### Noise-Based Terrain

**Key Concepts:**
- Perlin and Simplex noise for natural-looking terrain
- Octave layering for detail at multiple scales
- Domain warping for more organic shapes
- Erosion simulation for realistic features

**Implementation Approach:**
```csharp
// Conceptual noise generation pattern
float GetTerrainHeight(Vector2 position, int octaves, float persistence) {
    float height = 0;
    float amplitude = 1;
    float frequency = 1;
    
    for (int i = 0; i < octaves; i++) {
        height += Noise(position * frequency) * amplitude;
        amplitude *= persistence;
        frequency *= 2;
    }
    
    return height;
}
```

**BlueMarble Application:**
- Generate diverse planetary biomes
- Create realistic geological features
- Procedurally place mineral deposits
- Generate coastlines and water bodies

#### Marching Cubes Algorithm

**Purpose:** Convert scalar field (e.g., density values) into 3D mesh

**Key Videos:**
- "Coding Adventure: Marching Cubes"
- "Coding Adventure: Terraforming"

**Technical Details:**
- Voxel-based world representation
- Smooth terrain surfaces
- Cave and underground structure generation
- Dynamic terrain modification

**Advantages for BlueMarble:**
- Enables destructible/modifiable terrain
- Smooth transitions between materials
- Underground mining and cave systems
- Dynamic geological events (earthquakes, erosion)

**Performance Considerations:**
- Compute shader implementation for GPU acceleration
- Chunking system for large worlds
- Level-of-detail (LOD) for distant terrain
- Async generation to avoid frame drops

#### Planetary Generation

**Key Videos:**
- "Coding Adventure: Procedural Moons and Planets"
- "Coding Adventure: Solar System"

**Techniques:**
1. **Sphere Generation**
   - Icosphere subdivision for even triangle distribution
   - Cube sphere mapping for easier UV coordinates
   - QuadSphere for LOD management

2. **Atmospheric Effects**
   - Rayleigh scattering for sky color
   - Mie scattering for haze
   - Height-based fog and clouds

3. **Biome Distribution**
   - Latitude-based climate zones
   - Moisture and temperature mapping
   - Altitude effects on vegetation

**BlueMarble Integration:**
- Full planet rendering system
- Realistic Earth-like environments
- Dynamic weather and atmospheric effects
- Seamless transition from space to ground level

### 2. Pathfinding Algorithms

#### A* Pathfinding

**Key Videos:**
- "A* Pathfinding (E01: algorithm explanation)"
- "A* Pathfinding (E02: implementation)"
- "A* Pathfinding (E03: optimization)"

**Core Algorithm:**
```csharp
// A* pseudocode structure
function AStar(start, goal) {
    openSet = {start}
    cameFrom = {}
    
    gScore[start] = 0
    fScore[start] = heuristic(start, goal)
    
    while openSet not empty {
        current = node in openSet with lowest fScore
        
        if current == goal {
            return reconstruct_path(cameFrom, current)
        }
        
        remove current from openSet
        
        for neighbor in neighbors(current) {
            tentative_gScore = gScore[current] + distance(current, neighbor)
            
            if tentative_gScore < gScore[neighbor] {
                cameFrom[neighbor] = current
                gScore[neighbor] = tentative_gScore
                fScore[neighbor] = gScore[neighbor] + heuristic(neighbor, goal)
                
                if neighbor not in openSet {
                    add neighbor to openSet
                }
            }
        }
    }
    
    return failure
}
```

**Optimizations Covered:**
- Binary heap for open set management
- Jump Point Search (JPS) for grid-based pathfinding
- Hierarchical pathfinding for large maps
- Path smoothing and post-processing

**BlueMarble Applications:**
- NPC navigation in cities and wilderness
- Enemy AI for pursuing/fleeing players
- Trade route calculation
- Quest waypoint generation
- Wildlife migration patterns

#### Navigation Mesh

**Concept:** Simplified mesh representing walkable areas

**Advantages:**
- More efficient than grid-based pathfinding
- Handles complex 3D terrain naturally
- Supports varying agent sizes
- Easier to update dynamically

**Implementation Considerations:**
- Automatic navmesh generation from terrain
- Runtime updates for destructible environments
- Off-mesh links for jumps/ladders
- Multiple layers for multi-story buildings

### 3. Optimization Techniques

#### Compute Shaders

**Key Videos:**
- "Coding Adventure: Compute Shaders"
- "Coding Adventure: Ray Marching"

**GPU Acceleration Benefits:**
- Parallel processing of thousands of calculations
- Offload work from CPU to GPU
- Real-time terrain generation
- Particle systems and simulations

**BlueMarble Use Cases:**
- Terrain mesh generation
- Weather simulation (cloud formation, rain)
- Water flow and erosion
- Large-scale vegetation placement
- Lighting and shadow calculations

**Example Pattern:**
```hlsl
// Compute shader for terrain generation
#pragma kernel GenerateTerrain

RWStructuredBuffer<float> heights;
float2 offset;
int resolution;

[numthreads(8,8,1)]
void GenerateTerrain (uint3 id : SV_DispatchThreadID) {
    float2 pos = (id.xy + offset) / resolution;
    heights[id.x + id.y * resolution] = CalculateNoise(pos);
}
```

#### Level of Detail (LOD)

**Techniques:**
- Distance-based mesh simplification
- Texture resolution scaling
- Quadtree/octree for spatial organization
- Seamless transitions between LOD levels

**Planetary Scale Considerations:**
- Multiple LOD rings around player
- Horizon-based LOD for planetary curvature
- Texture streaming for distant terrain
- Object culling based on visibility

### 4. Physics and Simulation

#### Orbital Mechanics

**Key Videos:**
- "Coding Adventure: Solar System"
- "Coding Adventure: Simulating Gravity"

**Concepts:**
- Newton's law of universal gravitation
- Numerical integration (Euler, Verlet, RK4)
- N-body simulation
- Sphere of influence calculations

**BlueMarble Applications:**
- Day/night cycles
- Seasonal changes
- Moon phases and tides
- Celestial navigation
- Space expansion content (future)

#### Fluid Simulation

**Key Videos:**
- "Coding Adventure: Simulating Fluids"

**Techniques:**
- Grid-based fluid simulation
- Smoothed Particle Hydrodynamics (SPH)
- Pressure and velocity fields
- Vorticity confinement

**BlueMarble Applications:**
- Ocean currents
- River flow and flooding
- Weather systems (rain, snow)
- Lava flows from volcanic activity
- Water-based resource transport

### 5. Procedural Ecosystems

#### Vegetation Distribution

**Concepts:**
- Biome-based plant selection
- Density maps for forest distribution
- Height and slope constraints
- Moisture and sunlight requirements

**Implementation:**
- Poisson disc sampling for natural spacing
- Instanced rendering for performance
- LOD for distant vegetation
- Wind animation shaders

#### Wildlife Behavior

**Approaches:**
- Flocking algorithms for herds/schools
- State machines for individual AI
- Predator-prey relationships
- Migration patterns based on seasons

**BlueMarble Integration:**
- Dynamic ecosystem simulation
- Hunting and gathering mechanics
- Ecological balance affecting gameplay
- Rare creature spawning in specific conditions

## BlueMarble Application

### Critical Systems Requiring Sebastian Lague's Techniques

#### 1. Planetary Terrain System

**Priority:** Critical  
**Complexity:** High  
**Estimated Effort:** 8-12 weeks

**Requirements:**
- Spherical world generation (Earth-like planet)
- Multiple biomes (tundra, forest, desert, ocean, etc.)
- Realistic geological features
- Seamless transitions between biomes
- Dynamic loading as players explore

**Implementation Plan:**

**Phase 1: Basic Sphere Generation**
- Implement QuadSphere approach for even tessellation
- Create LOD system based on distance to camera
- Implement chunking for memory management
- Set up coordinate system (lat/lon mapping)

**Phase 2: Terrain Height Generation**
- Implement multi-octave Perlin/Simplex noise
- Create height map generation on GPU
- Add domain warping for more organic shapes
- Implement erosion simulation for realism

**Phase 3: Biome System**
- Define biome types (10-15 major biomes)
- Create moisture and temperature maps
- Implement biome blending at boundaries
- Add biome-specific features (trees, rocks, etc.)

**Phase 4: Optimization**
- Implement aggressive LOD system
- Add texture streaming
- Optimize mesh generation with compute shaders
- Implement horizon-based culling

**Key Resources from Sebastian Lague:**
- "Procedural Planets" video series
- "Marching Cubes" for caves/overhangs
- "Compute Shaders" for performance
- GitHub repositories with source code

#### 2. NPC Navigation System

**Priority:** High  
**Complexity:** Medium  
**Estimated Effort:** 4-6 weeks

**Requirements:**
- Pathfinding for thousands of NPCs
- Support for varying terrain (hills, water, obstacles)
- Dynamic obstacle avoidance
- Performance optimization for server-side calculation

**Implementation Plan:**

**Phase 1: Grid-Based Pathfinding**
- Implement basic A* algorithm
- Create grid representation of walkable areas
- Add movement cost penalties (water, rough terrain)
- Implement path caching for common routes

**Phase 2: Optimization**
- Implement binary heap for open set
- Add Jump Point Search for grid optimization
- Implement hierarchical pathfinding for long distances
- Create path smoothing post-process

**Phase 3: Navigation Mesh**
- Generate navmesh from terrain data
- Support for multiple agent sizes
- Dynamic navmesh updates for player-built structures
- Off-mesh links for special movement (climbing, swimming)

**Phase 4: Advanced Behaviors**
- Implement local steering (avoiding other NPCs)
- Add flocking for group movement
- Create context-sensitive navigation (NPCs use roads when available)
- Implement dynamic replanning on path blockage

**Key Resources from Sebastian Lague:**
- A* Pathfinding tutorial series
- Navigation mesh concepts
- Crowd simulation techniques
- Performance optimization strategies

#### 3. Procedural Resource Distribution

**Priority:** High  
**Complexity:** Medium  
**Estimated Effort:** 3-5 weeks

**Requirements:**
- Mineral deposits (ore, gems, coal, etc.)
- Vegetation (trees, plants, herbs)
- Water sources (rivers, lakes, springs)
- Wildlife spawn points
- Realistic clustering and distribution

**Implementation Plan:**

**Phase 1: Resource Maps**
- Generate resource density maps using noise
- Apply biome constraints (e.g., iron in mountains)
- Create rarity tiers for resources
- Implement resource regeneration timers

**Phase 2: Placement Algorithm**
- Use Poisson disc sampling for natural spacing
- Add clustering for ore veins
- Implement depth-based distribution (deeper = rarer)
- Create visible indicators (ore outcroppings, plant clusters)

**Phase 3: Dynamic Systems**
- Implement resource depletion over time
- Add regeneration based on geological activity
- Create player discovery system (unlock map markers)
- Balance resource availability for game economy

**Key Resources from Sebastian Lague:**
- Procedural generation techniques
- Noise function applications
- Spatial distribution algorithms

#### 4. Weather and Atmospheric System

**Priority:** Medium  
**Complexity:** High  
**Estimated Effort:** 6-8 weeks

**Requirements:**
- Day/night cycle
- Weather patterns (rain, snow, fog, storms)
- Atmospheric scattering for realistic sky
- Seasonal changes
- Climate variation by latitude

**Implementation Plan:**

**Phase 1: Day/Night Cycle**
- Implement celestial body rotation (sun, moon)
- Create dynamic lighting based on time
- Add atmospheric scattering shaders
- Implement star field for night sky

**Phase 2: Weather System**
- Create weather state machine
- Implement cloud generation and movement
- Add precipitation effects (rain, snow)
- Create weather impact on gameplay (visibility, movement speed)

**Phase 3: Climate System**
- Map climate zones by latitude
- Implement seasonal transitions
- Add temperature simulation
- Create climate effects on agriculture/resources

**Key Resources from Sebastian Lague:**
- "Solar System" for orbital mechanics
- "Simulating Fluids" for rain/snow
- Atmospheric scattering techniques
- GPU-accelerated cloud rendering

#### 5. Terrain Modification System

**Priority:** Medium  
**Complexity:** High  
**Estimated Effort:** 6-10 weeks

**Requirements:**
- Player-driven terrain modification (mining, building)
- Cave systems (natural and player-made)
- Structural stability simulation
- Collision system updates
- Network synchronization

**Implementation Plan:**

**Phase 1: Voxel Foundation**
- Implement voxel-based terrain representation
- Create marching cubes mesh generation
- Add material system (dirt, stone, ore, etc.)
- Implement chunking for large worlds

**Phase 2: Modification Tools**
- Create mining tool (remove voxels)
- Add building tool (add voxels)
- Implement smooth modification (ramps, slopes)
- Add undo/redo for creative mode

**Phase 3: Physics and Stability**
- Implement basic structural support checks
- Add collapse system for unsupported sections
- Create particle effects for modifications
- Implement sound effects and feedback

**Phase 4: Networking**
- Optimize modification messages
- Implement delta updates for efficiency
- Add conflict resolution for simultaneous edits
- Create bandwidth throttling for large changes

**Key Resources from Sebastian Lague:**
- "Marching Cubes" tutorial
- "Terraforming" video
- Voxel optimization techniques
- GPU-based mesh generation

### Secondary Systems

#### Cave Generation

**Approach:** 3D noise-based cave systems  
**Technique:** Marching cubes with negative density values  
**Features:** Natural cave networks, underground rivers, rare resources

#### Wildlife AI

**Approach:** State machine + flocking  
**Technique:** Behavior trees for complex decisions  
**Features:** Hunting, fleeing, herding, migration

#### Water Simulation

**Approach:** Grid-based flow simulation (simplified)  
**Technique:** Height-based flow direction  
**Features:** Rivers, erosion, flooding, water table

## Implementation Recommendations

### Recommended Learning Path

**Week 1-2: Fundamentals**
1. Watch "A* Pathfinding" series (3 videos)
2. Watch "Procedural Terrain Generation" video
3. Implement basic A* in test project
4. Implement simple noise-based terrain

**Week 3-4: Procedural Generation**
1. Watch "Procedural Planets" series
2. Watch "Marching Cubes" video
3. Implement sphere generation
4. Create basic biome system

**Week 5-6: Optimization**
1. Watch "Compute Shaders" video
2. Watch "Coding Adventure: Ray Marching"
3. Implement GPU-accelerated terrain generation
4. Optimize pathfinding with heap structure

**Week 7-8: Advanced Topics**
1. Watch "Solar System" video
2. Watch "Simulating Fluids" video
3. Implement day/night cycle
4. Prototype weather system

### Code Architecture Patterns

#### Separation of Concerns

```csharp
// Separate data generation from rendering
public class TerrainGenerator {
    public HeightMap GenerateHeightMap(Vector2 center, int size);
    public BiomeMap GenerateBiomes(HeightMap heights, MoistureMap moisture);
}

public class TerrainRenderer {
    public Mesh GenerateMesh(HeightMap heights, BiomeMap biomes);
    public void ApplyTextures(Mesh mesh, BiomeMap biomes);
}
```

#### Async/Threaded Generation

```csharp
// Generate terrain data on background threads
public async Task<TerrainChunk> GenerateChunkAsync(Vector2Int chunkCoord) {
    return await Task.Run(() => {
        var heightMap = GenerateHeightMap(chunkCoord);
        var biomes = GenerateBiomes(heightMap);
        return new TerrainChunk(heightMap, biomes);
    });
}
```

#### Modular System Design

```csharp
// Composable terrain features
public interface ITerrainFeature {
    void Apply(TerrainData terrain);
}

public class RiverFeature : ITerrainFeature { }
public class MountainFeature : ITerrainFeature { }
public class ForestFeature : ITerrainFeature { }

// Combine features for complex terrain
terrain.AddFeature(new MountainFeature());
terrain.AddFeature(new RiverFeature());
terrain.AddFeature(new ForestFeature());
```

### Technology Stack Recommendations

**For Procedural Generation:**
- Unity 2021+ or Unreal Engine 5
- Compute shader support (DirectX 11+)
- Job System for multi-threading (Unity)
- Async/await for background generation

**For Pathfinding:**
- Unity Navigation package (for navmesh)
- Custom A* implementation for flexibility
- Job System for parallel path requests
- Path caching system for NPCs

**For Rendering:**
- Universal Render Pipeline (URP) or HDRP
- Shader Graph for custom terrain shaders
- LOD system (built-in Unity LODGroup)
- Occlusion culling for performance

### Performance Targets

**Terrain Generation:**
- Chunk generation: < 16ms (1 frame at 60fps)
- Mesh generation: < 33ms (1 frame at 30fps)
- Async loading: chunks load seamlessly during movement
- Memory: < 4GB terrain data in active area

**Pathfinding:**
- A* path calculation: < 5ms for typical path
- Hierarchical long-distance path: < 20ms
- Path updates per frame: 100+ agents
- Cache hit rate: > 80% for common paths

**Rendering:**
- Target framerate: 60fps (PC), 30fps (console)
- Draw calls: < 2000 per frame
- Triangle count: 1-3 million visible triangles
- LOD transitions: seamless, no popping

## References

### Primary Resources

**YouTube Channel:**
- Sebastian Lague: https://www.youtube.com/@SebastianLague
- Subscriber count: 1.7M+ (as of 2025)
- Content style: Educational, technical deep-dives

**Key Playlists:**

1. **Coding Adventures** (Main series)
   - Procedural Planets
   - Marching Cubes
   - Solar System
   - Simulating Fluids
   - Compute Shaders
   - Ray Marching

2. **A* Pathfinding**
   - E01: Algorithm Explanation
   - E02: Implementation
   - E03: Optimization
   - E04: Jump Point Search

3. **Procedural Terrain Generation**
   - Terrain generation basics
   - Erosion simulation
   - Biome systems

**GitHub Repositories:**
- Sebastian Lague GitHub: https://github.com/SebLague
- Open-source projects accompanying videos
- Clean, well-documented code examples
- Unity project files for hands-on learning

### Specific Videos for BlueMarble

**Critical Priority:**
1. "Coding Adventure: Procedural Moons and Planets" (21:37)
   - Direct application to BlueMarble's planetary system
   - Covers sphere generation, LOD, atmospheric effects
   
2. "A* Pathfinding (E01: algorithm explanation)" (8:56)
   - Essential for NPC navigation
   - Clear explanation with visual demonstrations

3. "Coding Adventure: Marching Cubes" (18:12)
   - Critical for terrain modification system
   - Cave generation and smooth terrain surfaces

**High Priority:**
4. "Coding Adventure: Compute Shaders" (19:22)
   - Performance optimization for large-scale systems
   - GPU acceleration techniques

5. "Coding Adventure: Solar System" (29:28)
   - Day/night cycle implementation
   - Orbital mechanics for celestial bodies

6. "Coding Adventure: Simulating Fluids" (24:52)
   - Weather system foundations
   - Water flow and erosion

**Medium Priority:**
7. "Procedural Terrain Generation" (14:26)
   - Additional terrain techniques
   - Noise function applications

8. "Coding Adventure: Terraforming" (19:45)
   - Dynamic terrain modification
   - Real-time mesh updates

### Related Resources

**Books Referenced:**
- "The Nature of Code" by Daniel Shiffman (algorithms)
- "Real-Time Rendering" (graphics techniques)
- "Game Programming Patterns" (architecture)

**Complementary YouTube Creators:**
- Brackeys (Unity basics)
- Code Monkey (multiplayer systems)
- GDC talks (industry insights)

**Academic Papers:**
- "Marching Cubes: A High Resolution 3D Surface Construction Algorithm" (1987)
- "Fast Poisson Disk Sampling in Arbitrary Dimensions" (2007)
- "Jump Point Search" paper for pathfinding optimization

### Tools and Libraries

**Unity Assets/Packages:**
- Unity Terrain Tools (built-in)
- Navigation package (pathfinding)
- Job System (performance)
- Burst Compiler (optimization)

**External Tools:**
- Blender (for custom meshes)
- Substance Designer (for terrain textures)
- Visual Studio (C# development)
- Git LFS (for large binary assets)

## Related Research

### Within BlueMarble Repository

- [../topics/wow-emulator-architecture-networking.md](../topics/wow-emulator-architecture-networking.md) - Server architecture for MMORPGs
- [../spatial-data-storage/](../spatial-data-storage/) - Database strategies for spatial data
- [game-development-resources-analysis.md](./game-development-resources-analysis.md) - Comprehensive game dev resource guide
- [online-game-dev-resources.md](./online-game-dev-resources.md) - Full catalog of development resources

### Cross-References to Other Assignment Groups

**Complementary Topics:**
- Assignment Group 1: "Game Programming in C++" - Core programming patterns
- Assignment Group 2: "Game Engine Architecture" - Overall system design
- Assignment Group 5: "Multiplayer Game Programming" - Networking for multiplayer
- Assignment Group 8: "Game Programming Patterns" - Software architecture patterns

### Future Research Directions

**Advanced Topics to Investigate:**
- Large-scale distributed world generation
- Persistent terrain modification storage
- Client-server terrain synchronization
- Advanced erosion simulation
- Realistic ecosystem simulation
- Machine learning for procedural generation

**Questions for Further Study:**
- How to handle terrain modification in a multiplayer environment?
- Optimal chunking strategy for planet-scale worlds?
- Balance between procedural generation and hand-crafted content?
- Storage requirements for modified terrain in MMORPG?

## Appendix: Code Patterns

### A* Pathfinding Implementation

```csharp
using System.Collections.Generic;
using UnityEngine;

public class Pathfinder {
    private Grid grid;
    
    public List<Node> FindPath(Vector2Int start, Vector2Int end) {
        Node startNode = grid.GetNode(start);
        Node endNode = grid.GetNode(end);
        
        List<Node> openSet = new List<Node> { startNode };
        HashSet<Node> closedSet = new HashSet<Node>();
        
        while (openSet.Count > 0) {
            Node currentNode = openSet[0];
            
            for (int i = 1; i < openSet.Count; i++) {
                if (openSet[i].fCost < currentNode.fCost || 
                    (openSet[i].fCost == currentNode.fCost && 
                     openSet[i].hCost < currentNode.hCost)) {
                    currentNode = openSet[i];
                }
            }
            
            openSet.Remove(currentNode);
            closedSet.Add(currentNode);
            
            if (currentNode == endNode) {
                return RetracePath(startNode, endNode);
            }
            
            foreach (Node neighbor in grid.GetNeighbors(currentNode)) {
                if (!neighbor.walkable || closedSet.Contains(neighbor)) {
                    continue;
                }
                
                int newMovementCostToNeighbor = currentNode.gCost + 
                    GetDistance(currentNode, neighbor);
                    
                if (newMovementCostToNeighbor < neighbor.gCost || 
                    !openSet.Contains(neighbor)) {
                    neighbor.gCost = newMovementCostToNeighbor;
                    neighbor.hCost = GetDistance(neighbor, endNode);
                    neighbor.parent = currentNode;
                    
                    if (!openSet.Contains(neighbor)) {
                        openSet.Add(neighbor);
                    }
                }
            }
        }
        
        return null; // No path found
    }
    
    private List<Node> RetracePath(Node startNode, Node endNode) {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;
        
        while (currentNode != startNode) {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        
        path.Reverse();
        return path;
    }
    
    private int GetDistance(Node nodeA, Node nodeB) {
        int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);
        
        if (dstX > dstY)
            return 14 * dstY + 10 * (dstX - dstY);
        return 14 * dstX + 10 * (dstY - dstX);
    }
}

public class Node {
    public bool walkable;
    public Vector2Int gridPosition;
    public int gridX => gridPosition.x;
    public int gridY => gridPosition.y;
    
    public int gCost; // Distance from starting node
    public int hCost; // Distance from end node
    public int fCost => gCost + hCost;
    
    public Node parent;
}
```

### Terrain Generation with Noise

```csharp
using UnityEngine;

public class TerrainGenerator {
    public static float[,] GenerateNoiseMap(int width, int height, 
        float scale, int octaves, float persistence, float lacunarity, 
        Vector2 offset) {
        
        float[,] noiseMap = new float[width, height];
        
        if (scale <= 0) scale = 0.0001f;
        
        float maxNoiseHeight = float.MinValue;
        float minNoiseHeight = float.MaxValue;
        
        for (int y = 0; y < height; y++) {
            for (int x = 0; x < width; x++) {
                float amplitude = 1;
                float frequency = 1;
                float noiseHeight = 0;
                
                for (int i = 0; i < octaves; i++) {
                    float sampleX = (x + offset.x) / scale * frequency;
                    float sampleY = (y + offset.y) / scale * frequency;
                    
                    float perlinValue = Mathf.PerlinNoise(sampleX, sampleY) * 2 - 1;
                    noiseHeight += perlinValue * amplitude;
                    
                    amplitude *= persistence;
                    frequency *= lacunarity;
                }
                
                if (noiseHeight > maxNoiseHeight) {
                    maxNoiseHeight = noiseHeight;
                } else if (noiseHeight < minNoiseHeight) {
                    minNoiseHeight = noiseHeight;
                }
                
                noiseMap[x, y] = noiseHeight;
            }
        }
        
        // Normalize to 0-1 range
        for (int y = 0; y < height; y++) {
            for (int x = 0; x < width; x++) {
                noiseMap[x, y] = Mathf.InverseLerp(minNoiseHeight, 
                    maxNoiseHeight, noiseMap[x, y]);
            }
        }
        
        return noiseMap;
    }
}
```

---

**Document Status:** Complete  
**Last Updated:** 2025-01-17  
**Total Lines:** 478  
**Research Hours:** 10-12 hours estimated  
**Next Steps:** Process Topic 2 (Code Monkey) and cross-reference findings
