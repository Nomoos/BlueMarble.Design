---
title: Algorithm Analysis - Marching Cubes
date: 2025-01-17
tags: [algorithms, voxel, terrain, procedural-generation, 3d-graphics, mesh-generation]
status: complete
priority: high
source: Discovered from Sebastian Lague analysis
discovered-from: game-dev-analysis-sebastian-lague.md
---

# Algorithm Analysis: Marching Cubes

## Executive Summary

**Paper Title:** Marching Cubes: A High Resolution 3D Surface Construction Algorithm  
**Authors:** William E. Lorensen, Harvey E. Cline  
**Published:** SIGGRAPH 1987  
**Field:** Computer Graphics, 3D Surface Reconstruction  
**Primary Application:** Converting scalar field data into polygonal mesh representation

### Core Innovation

The Marching Cubes algorithm revolutionized 3D surface extraction by providing an efficient method to generate smooth, continuous surfaces from volumetric data. It processes data in a systematic, cube-by-cube manner ("marching" through the volume), determining how the surface intersects each cube and generating appropriate triangles.

### Key Characteristics

- **Systematic Approach:** Processes volume data in discrete cubic cells
- **Lookup Table Method:** Uses precomputed configurations to determine surface topology
- **Guaranteed Connectivity:** Produces water-tight meshes with proper vertex sharing
- **Local Processing:** Each cube can be processed independently, enabling parallelization
- **Adaptive Resolution:** Can be extended to variable-detail representations

### Relevance to BlueMarble MMORPG

**Critical Applications:**
- **Terrain Modification System:** Enable players to mine, excavate, and build structures
- **Cave Generation:** Create natural underground networks and caverns
- **Dynamic Terrain:** Support real-time terrain deformation from explosions, erosion
- **Smooth Surfaces:** Generate organic-looking terrain without blocky appearance
- **LOD Systems:** Foundation for level-of-detail terrain rendering

**Priority Level:** High - Essential for terrain modification mechanics

## Algorithm Overview

### The Problem

Traditional voxel-based representations (like Minecraft) produce blocky, stepped surfaces. Medical imaging and scientific visualization require smooth surface extraction from 3D scalar fields (e.g., CT scans, MRI data, geological surveys). The challenge is converting volumetric density values into a continuous mesh representation.

### The Solution: Marching Cubes

The algorithm processes a 3D grid of scalar values, treating each 8-corner cube independently:

1. **Sample Corners:** Each cube corner has a scalar value (e.g., density, temperature)
2. **Classify Corners:** Determine if each corner is "inside" or "outside" the surface (above/below threshold)
3. **Lookup Configuration:** The 8 binary states create 256 possible configurations
4. **Generate Triangles:** Use lookup table to determine which triangles to create
5. **Interpolate Positions:** Calculate exact vertex positions along cube edges
6. **Output Mesh:** Combine triangles to form continuous surface

### Mathematical Foundation

**Scalar Field Definition:**
```
f(x, y, z) → ℝ
```
Where f represents density, signed distance, or any scalar property.

**Surface Definition:**
```
Surface S = {(x, y, z) | f(x, y, z) = isovalue}
```

**Binary Classification:**
```
corner_state = f(x, y, z) >= isovalue ? 1 : 0
```

**Cube Index:**
```
cube_index = Σ(corner_state[i] × 2^i) for i = 0 to 7
```
This produces a value from 0-255 identifying the configuration.

### The 15 Base Cases

Through symmetry and rotation, the 256 configurations reduce to 15 fundamental cases:

1. **Case 0:** All corners outside (no surface) - 0 triangles
2. **Case 1:** One corner inside - 1 triangle
3. **Case 2:** Two adjacent corners inside - 1 triangle
4. **Case 3:** Two opposite corners inside - 2 triangles
5. **Case 4:** Three corners inside (L-shape) - 2 triangles
6. **Case 5:** Three corners inside (line) - 2 triangles
7. **Case 6:** Four corners inside (tetrahedron) - 2 triangles
8. **Case 7:** Four corners inside (quad) - 2 triangles
9. **Case 8:** Four corners inside (diagonal) - 4 triangles
10. **Case 9:** Five corners inside - 3 triangles
11. **Case 10:** Six corners inside (L-shape) - 3 triangles
12. **Case 11:** Six corners inside (opposite) - 2 triangles
13. **Case 12:** Seven corners inside - 1 triangle
14. **Case 13:** Ambiguous saddle case - 2-4 triangles
15. **Case 14:** All corners inside (no surface) - 0 triangles

### Edge Interpolation

Vertices are placed along cube edges where the surface crosses:

```
Vertex Position = p1 + (isovalue - v1) / (v2 - v1) × (p2 - p1)
```

Where:
- p1, p2: Corner positions
- v1, v2: Corner scalar values
- isovalue: Target surface value

This linear interpolation creates smooth surfaces.

## Core Concepts

### 1. Voxel Grid Representation

**Data Structure:**
```csharp
public class VoxelGrid {
    private float[,,] densities;
    public int width, height, depth;
    public float cellSize;
    
    public float GetDensity(int x, int y, int z) {
        if (x < 0 || x >= width || y < 0 || y >= height || z < 0 || z >= depth)
            return 0f;
        return densities[x, y, z];
    }
    
    public void SetDensity(int x, int y, int z, float value) {
        densities[x, y, z] = value;
    }
}
```

**Memory Considerations:**
- A 256×256×256 grid requires 16 MB (assuming 1 byte per voxel)
- A 512×512×512 grid requires 128 MB
- Compression techniques essential for large worlds

### 2. Lookup Table Generation

The edge table stores which edges are intersected for each configuration:

```csharp
// Precomputed edge table (256 entries)
// Each entry is a 12-bit value indicating which edges are crossed
private static readonly int[] edgeTable = new int[256] {
    0x0  , 0x109, 0x203, 0x30a, 0x406, 0x50f, 0x605, 0x70c,
    // ... 248 more entries
};

// Triangle table stores which triangles to generate (256 × 16 entries)
// Each configuration can generate up to 5 triangles (15 vertices)
private static readonly int[,] triTable = new int[256, 16] {
    {-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
    { 0,  8,  3, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
    // ... 254 more entries
};
```

### 3. Marching Process

**Pseudocode:**
```
for each cube (x, y, z) in volume:
    # Sample 8 corners
    corners[8] = sample corner densities
    
    # Calculate cube index
    cubeIndex = 0
    for i in 0..7:
        if corners[i] > isovalue:
            cubeIndex |= (1 << i)
    
    # Skip if cube completely inside or outside
    if cubeIndex == 0 or cubeIndex == 255:
        continue
    
    # Find intersected edges
    edges = edgeTable[cubeIndex]
    
    # Interpolate vertex positions
    vertices[12] = array of interpolated positions
    for i in 0..11:
        if edges & (1 << i):
            vertices[i] = interpolate along edge i
    
    # Generate triangles
    triangles = triTable[cubeIndex]
    for each triangle in triangles:
        add triangle to mesh using vertex indices
```

### 4. Optimizations

**Vertex Sharing:**
Multiple cubes share edges. To avoid duplicate vertices:
- Hash table mapping edge coordinates to vertex indices
- Reduces memory and improves rendering performance

**Chunk-Based Processing:**
Divide large volumes into manageable chunks:
- Process chunks independently
- Enable streaming and LOD systems
- Reduce memory footprint

**Parallel Processing:**
Each chunk/cube can be processed independently:
- Multi-threaded CPU implementation
- Compute shader GPU implementation
- Significant speedup for large volumes

### 5. Ambiguous Cases

**The Saddle Problem:**
Some configurations have multiple valid interpretations. Configuration 6 faces (alternating corners) can create a saddle point, leading to two possible surface topologies.

**Solutions:**
1. **Extended Marching Cubes:** Additional tests to resolve ambiguity
2. **Asymptotic Decider:** Use gradient information to determine correct case
3. **Consistent Resolution:** Apply same rule across all ambiguous cases for water-tight mesh

## BlueMarble Application

### 1. Terrain Modification System

**Use Case:** Players mining, excavating, and building

**Implementation Plan:**

**Phase 1: Core Voxel System (3-4 weeks)**
- Implement voxel grid data structure
- Create basic density modification tools (add/remove)
- Implement marching cubes mesh generation
- Test with simple shapes (spheres, boxes)

**Phase 2: Chunking System (2-3 weeks)**
- Divide world into manageable chunks (16×16×16 or 32×32×32)
- Implement chunk loading/unloading
- Create chunk boundary handling
- Add mesh caching for unmodified chunks

**Phase 3: Performance Optimization (2-3 weeks)**
- Implement compute shader version for GPU acceleration
- Add vertex sharing across chunk boundaries
- Implement dirty flag system for selective updates
- Profile and optimize critical paths

**Phase 4: Visual Polish (2 weeks)**
- Add smooth shading with normal calculation
- Implement texture mapping based on voxel material
- Add ambient occlusion for depth perception
- Create particle effects for modifications

**Technical Specifications:**
- **Chunk Size:** 32×32×32 voxels (optimal for GPU processing)
- **Voxel Resolution:** 0.5m per voxel (allows detailed construction)
- **Update Rate:** < 16ms for single chunk regeneration
- **Memory Budget:** 2-4 MB per loaded chunk

### 2. Cave Generation System

**Use Case:** Natural underground networks, dungeons, mines

**Implementation Approach:**

**3D Noise-Based Generation:**
```csharp
public float GenerateCaveDensity(Vector3 position) {
    // Base terrain height
    float surfaceHeight = GetTerrainHeight(position.x, position.z);
    
    // Underground caves using 3D noise
    float caveNoise = Perlin3D(position * 0.05f);
    
    // Density decreases underground
    float undergroundFactor = Mathf.Max(0, surfaceHeight - position.y);
    
    // Cave threshold - lower value creates caves
    float density = undergroundFactor + caveNoise - 0.3f;
    
    return density;
}
```

**Features:**
- **Procedural Networks:** Connected cave systems using noise
- **Size Variation:** Small tunnels to large caverns
- **Resource Deposits:** Ore veins visible in cave walls
- **Structural Support:** Physics-based ceiling collapse

**Performance Targets:**
- Cave generation during terrain loading
- No runtime performance impact (pre-generated)
- Smooth transitions between surface and underground

### 3. Dynamic Terrain Events

**Use Case:** Explosions, earthquakes, volcanic activity

**Implementation:**

**Sphere-Based Modification:**
```csharp
public void CreateExplosion(Vector3 center, float radius, float power) {
    // Calculate affected chunk range
    Vector3Int minChunk = WorldToChunkCoord(center - Vector3.one * radius);
    Vector3Int maxChunk = WorldToChunkCoord(center + Vector3.one * radius);
    
    // Modify voxels in range
    for (int cx = minChunk.x; cx <= maxChunk.x; cx++) {
        for (int cy = minChunk.y; cy <= maxChunk.y; cy++) {
            for (int cz = minChunk.z; cz <= maxChunk.z; cz++) {
                ModifyChunkForExplosion(cx, cy, cz, center, radius, power);
            }
        }
    }
}

private void ModifyChunkForExplosion(int cx, int cy, int cz, 
    Vector3 center, float radius, float power) {
    
    VoxelChunk chunk = GetChunk(cx, cy, cz);
    
    for (int x = 0; x < chunkSize; x++) {
        for (int y = 0; y < chunkSize; y++) {
            for (int z = 0; z < chunkSize; z++) {
                Vector3 worldPos = ChunkToWorldPosition(cx, cy, cz, x, y, z);
                float dist = Vector3.Distance(worldPos, center);
                
                if (dist < radius) {
                    float falloff = 1f - (dist / radius);
                    float damage = power * falloff * falloff;
                    
                    // Reduce density (create crater)
                    float currentDensity = chunk.GetDensity(x, y, z);
                    chunk.SetDensity(x, y, z, currentDensity - damage);
                }
            }
        }
    }
    
    // Mark chunk for mesh regeneration
    chunk.MarkDirty();
}
```

**Event Types:**
- **Explosions:** Sphere-based removal
- **Earthquakes:** Noise-based displacement
- **Erosion:** Gradual density reduction over time
- **Lava Flow:** Additive density modification

### 4. Building and Construction

**Use Case:** Player-built structures, fortifications, cities

**Implementation:**

**Additive Construction:**
```csharp
public void PlaceBlock(Vector3 position, MaterialType material) {
    Vector3Int voxelCoord = WorldToVoxelCoord(position);
    
    // Set voxel density to solid
    SetVoxelDensity(voxelCoord, 1.0f);
    SetVoxelMaterial(voxelCoord, material);
    
    // Regenerate affected chunks
    RegenerateChunksAround(voxelCoord);
}

public void PlaceStructure(Vector3 position, StructureTemplate template) {
    // Structures are predefined voxel patterns
    for (int x = 0; x < template.width; x++) {
        for (int y = 0; y < template.height; y++) {
            for (int z = 0; z < template.depth; z++) {
                if (template.IsSolid(x, y, z)) {
                    Vector3Int voxelPos = new Vector3Int(
                        (int)position.x + x,
                        (int)position.y + y,
                        (int)position.z + z
                    );
                    SetVoxelDensity(voxelPos, 1.0f);
                    SetVoxelMaterial(voxelPos, template.GetMaterial(x, y, z));
                }
            }
        }
    }
    
    RegenerateLargeArea(position, template.GetBounds());
}
```

**Features:**
- **Material Types:** Stone, wood, metal, glass (different textures)
- **Structural Templates:** Pre-built structures (walls, towers, buildings)
- **Snap-to-Grid:** Optional alignment for easier building
- **Collision Detection:** Prevent overlapping structures

### 5. Level of Detail (LOD) System

**Use Case:** Render distant terrain efficiently

**Implementation Approach:**

**Variable Resolution Marching Cubes:**
```csharp
public class LODVoxelTerrain {
    private Dictionary<Vector3Int, VoxelChunk> chunks;
    private int[] lodSizes = { 32, 16, 8, 4 }; // Decreasing detail
    
    public void UpdateLOD(Vector3 cameraPosition) {
        foreach (var chunk in chunks.Values) {
            float distance = Vector3.Distance(cameraPosition, chunk.center);
            
            int lodLevel = GetLODLevel(distance);
            
            if (chunk.currentLOD != lodLevel) {
                RegenerateMeshAtLOD(chunk, lodLevel);
            }
        }
    }
    
    private void RegenerateMeshAtLOD(VoxelChunk chunk, int lodLevel) {
        int stepSize = lodSizes[lodLevel];
        
        // Sample voxels at lower resolution
        for (int x = 0; x < chunkSize; x += stepSize) {
            for (int y = 0; y < chunkSize; y += stepSize) {
                for (int z = 0; z < chunkSize; z += stepSize) {
                    // Run marching cubes on larger cubes
                    MarchCube(chunk, x, y, z, stepSize);
                }
            }
        }
    }
}
```

**LOD Levels:**
- **LOD 0 (0-50m):** Full resolution (32³ voxels per chunk)
- **LOD 1 (50-100m):** Half resolution (16³ effective)
- **LOD 2 (100-200m):** Quarter resolution (8³ effective)
- **LOD 3 (200m+):** Eighth resolution (4³ effective)

**Transition Handling:**
- Smooth transitions between LOD levels
- Hysteresis to prevent LOD flickering
- Async mesh generation to avoid stuttering

## Implementation Recommendations

### Recommended Technology Stack

**For CPU Implementation:**
- **Language:** C# (Unity) or C++ (Unreal)
- **Threading:** Job System (Unity) or Task Parallel Library
- **Data Structures:** 3D arrays or flat arrays with indexing math

**For GPU Implementation:**
- **Compute Shaders:** HLSL/GLSL for GPU acceleration
- **Indirect Rendering:** GPU-driven mesh generation
- **Buffer Management:** Structured buffers for vertex data

### Architecture Pattern

**Separation of Concerns:**
```csharp
// Data layer
public class VoxelGrid { /* voxel storage */ }

// Algorithm layer
public class MarchingCubes {
    public Mesh GenerateMesh(VoxelGrid grid, float isovalue);
}

// Rendering layer
public class VoxelRenderer {
    public void RenderMesh(Mesh mesh, Material material);
}

// Modification layer
public class TerrainModifier {
    public void Modify(VoxelGrid grid, ModificationData data);
}
```

### Performance Optimization Strategies

**1. Spatial Hashing:**
Use hash maps to track modified chunks:
```csharp
private HashSet<Vector3Int> dirtyChunks = new HashSet<Vector3Int>();

public void MarkChunkDirty(Vector3Int chunkCoord) {
    dirtyChunks.Add(chunkCoord);
}

public void UpdateFrame() {
    // Process limited number per frame
    int processed = 0;
    foreach (var coord in dirtyChunks) {
        if (processed++ > maxChunksPerFrame) break;
        RegenerateChunk(coord);
    }
}
```

**2. Vertex Pooling:**
Reuse vertex buffers to reduce GC pressure:
```csharp
private Stack<List<Vector3>> vertexBufferPool = new Stack<List<Vector3>>();

public List<Vector3> GetVertexBuffer() {
    if (vertexBufferPool.Count > 0) {
        var buffer = vertexBufferPool.Pop();
        buffer.Clear();
        return buffer;
    }
    return new List<Vector3>(1024);
}

public void ReturnVertexBuffer(List<Vector3> buffer) {
    vertexBufferPool.Push(buffer);
}
```

**3. Asynchronous Generation:**
Generate meshes on background threads:
```csharp
public async Task<Mesh> GenerateMeshAsync(VoxelGrid grid) {
    // Compute on background thread
    var meshData = await Task.Run(() => {
        return MarchingCubes.ComputeMeshData(grid);
    });
    
    // Create Unity mesh on main thread
    return meshData.ToUnityMesh();
}
```

### Quality Assurance

**Testing Strategy:**
1. **Unit Tests:** Test individual cube configurations
2. **Visual Tests:** Compare output with reference implementations
3. **Performance Tests:** Measure mesh generation time
4. **Memory Tests:** Monitor memory usage under load
5. **Stress Tests:** Test with maximum voxel modifications

**Common Pitfalls:**
- **Ambiguous Cases:** Ensure consistent resolution of saddle configurations
- **Vertex Duplication:** Implement proper vertex sharing
- **Normal Calculation:** Smooth normals for organic appearance
- **Edge Cases:** Handle chunk boundaries correctly
- **Precision Issues:** Use appropriate floating-point precision

## References

### Primary Source

**Original Paper:**
- Lorensen, W.E. and Cline, H.E. (1987). "Marching Cubes: A High Resolution 3D Surface Construction Algorithm." *ACM SIGGRAPH Computer Graphics*, 21(4), pp. 163-169.
- DOI: 10.1145/37402.37422
- Available: ACM Digital Library, ResearchGate

### Extensions and Improvements

**Addressing Ambiguities:**
- Nielson, G.M. and Hamann, B. (1991). "The Asymptotic Decider: Resolving the Ambiguity in Marching Cubes." *Visualization*, pp. 83-91.
- Montani, C., et al. (1994). "Discretized Marching Cubes." *Visualization*, pp. 281-287.

**Performance Optimization:**
- Schaefer, S. and Warren, J. (2005). "Dual Marching Cubes: Primal Contouring of Dual Grids." *Computer Graphics Forum*, 24(2), pp. 195-201.
- Newman, T.S. and Yi, H. (2006). "A Survey of the Marching Cubes Algorithm." *Computers & Graphics*, 30(5), pp. 854-879.

**GPU Implementation:**
- Tatarchuk, N., et al. (2007). "Advanced Rendering Techniques Using DirectX 11." *SIGGRAPH Course Notes*.
- Johansson, M. and Carr, H. (2006). "Accelerating Marching Cubes with Graphics Hardware." *CASCON*, pp. 378-382.

### Implementation References

**Open Source Implementations:**
1. **Paul Bourke's Implementation**
   - URL: http://paulbourke.net/geometry/polygonise/
   - Classic C implementation with lookup tables
   - Excellent explanation and visual diagrams

2. **Sebastian Lague's Tutorials**
   - YouTube: "Coding Adventure: Marching Cubes"
   - URL: https://www.youtube.com/watch?v=M3iI2l0ltbE
   - Unity C# implementation with GPU compute shader

3. **Dual Contouring Comparison**
   - GitHub: Various implementations comparing MC with DC
   - Shows strengths/weaknesses of each approach

4. **PolyVox Library**
   - URL: http://www.volumesoffun.com/polyvox-about/
   - C++ voxel terrain library
   - Includes optimized marching cubes implementation

### Game Development Resources

**Voxel Game Implementations:**
- **Minecraft-style Voxels:** Blocky terrain (not using marching cubes)
- **Dual Universe:** Voxel-based MMORPG with extensive terrain modification
- **Space Engineers:** Real-time voxel destruction and construction
- **Astroneer:** Smooth voxel terrain using marching cubes variant

**Unity Asset Store:**
- Voxel Play - Voxel terrain system
- Cubiquity - Smooth voxel terrain
- uTerrains - Multi-resolution terrain system

### Academic Resources

**Computer Graphics Textbooks:**
- Shirley, P. and Marschner, S. (2009). *Fundamentals of Computer Graphics* (3rd ed.). Chapter on Surface Reconstruction.
- Akenine-Möller, T., et al. (2018). *Real-Time Rendering* (4th ed.). Chapter 13.10 on Voxelization.

**Survey Papers:**
- Newman, T.S. and Yi, H. (2006). "A Survey of the Marching Cubes Algorithm." Comprehensive review of variants and applications.

## Related Research

### Within BlueMarble Repository

- [game-dev-analysis-sebastian-lague.md](./game-dev-analysis-sebastian-lague.md) - Source of discovery, video tutorials on implementation
- [../topics/wow-emulator-architecture-networking.md](../topics/wow-emulator-architecture-networking.md) - Network synchronization for terrain modifications
- [../spatial-data-storage/](../spatial-data-storage/) - Database storage for persistent terrain changes
- [online-game-dev-resources.md](./online-game-dev-resources.md) - Additional graphics and algorithm resources

### Cross-References to Other Topics

**Complementary Algorithms:**
- **Dual Contouring:** Alternative surface extraction with sharper features
- **Surface Nets:** Simpler algorithm with different characteristics
- **Transvoxel Algorithm:** Seamless LOD transitions for voxel terrain

**Related Systems:**
- **Octree Structures:** Hierarchical voxel organization for LOD
- **Signed Distance Fields:** Alternative volumetric representation
- **GPU Compute Shaders:** Parallel processing for real-time generation

### Discovered Sources

**Alternative Algorithms:**
1. **Dual Contouring**
   - Better handling of sharp features
   - More complex but can preserve edges
   - Consider for BlueMarble if crisp building edges needed

2. **Surface Nets**
   - Simpler than marching cubes
   - Faster but less control over mesh quality
   - Good for prototyping

3. **Transvoxel Algorithm**
   - Specifically designed for seamless LOD transitions
   - Essential if implementing LOD voxel terrain
   - Patent expired, free to use

### Future Research Directions

**Advanced Topics:**
1. **Network Synchronization:** Efficient replication of voxel modifications in MMORPG
2. **Persistent Storage:** Database design for storing modified terrain
3. **Collision Generation:** Automatic collision mesh generation from voxels
4. **Physics Integration:** Structural stability and realistic collapse
5. **Texture Blending:** Smooth material transitions on voxel surfaces

**Open Questions:**
- How to handle player-modified terrain in 100+ player battles?
- Optimal chunk size for network bandwidth and memory?
- Storage requirements for planet-scale modified terrain?
- Anti-griefing mechanisms for terrain modification?

## Appendix: Implementation Example

### Complete Marching Cubes Implementation

```csharp
using System.Collections.Generic;
using UnityEngine;

public class MarchingCubes {
    // Edge table and triangle table (abbreviated for space)
    private static readonly int[] edgeTable = new int[256] {
        0x0  , 0x109, 0x203, 0x30a, 0x406, 0x50f, 0x605, 0x70c,
        // ... (full 256 entry table)
    };
    
    private static readonly int[,] triTable = new int[256, 16] {
        {-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
        { 0,  8,  3, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
        // ... (full 256 entry table)
    };
    
    // Corner positions in local cube space
    private static readonly Vector3[] cornerOffsets = new Vector3[8] {
        new Vector3(0, 0, 0),
        new Vector3(1, 0, 0),
        new Vector3(1, 0, 1),
        new Vector3(0, 0, 1),
        new Vector3(0, 1, 0),
        new Vector3(1, 1, 0),
        new Vector3(1, 1, 1),
        new Vector3(0, 1, 1)
    };
    
    // Edge connections (which two corners each edge connects)
    private static readonly int[,] edgeConnections = new int[12, 2] {
        {0,1}, {1,2}, {2,3}, {3,0},  // Bottom face
        {4,5}, {5,6}, {6,7}, {7,4},  // Top face
        {0,4}, {1,5}, {2,6}, {3,7}   // Vertical edges
    };
    
    public static Mesh GenerateMesh(float[,,] voxels, float isovalue, float voxelSize) {
        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();
        
        int width = voxels.GetLength(0) - 1;
        int height = voxels.GetLength(1) - 1;
        int depth = voxels.GetLength(2) - 1;
        
        // March through each cube
        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                for (int z = 0; z < depth; z++) {
                    MarchCube(voxels, x, y, z, isovalue, voxelSize, 
                             vertices, triangles);
                }
            }
        }
        
        // Create Unity mesh
        Mesh mesh = new Mesh();
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
        
        return mesh;
    }
    
    private static void MarchCube(float[,,] voxels, int x, int y, int z,
        float isovalue, float voxelSize, 
        List<Vector3> vertices, List<int> triangles) {
        
        // Sample 8 corners
        float[] corners = new float[8];
        for (int i = 0; i < 8; i++) {
            Vector3 offset = cornerOffsets[i];
            corners[i] = voxels[
                x + (int)offset.x, 
                y + (int)offset.y, 
                z + (int)offset.z
            ];
        }
        
        // Calculate cube index
        int cubeIndex = 0;
        for (int i = 0; i < 8; i++) {
            if (corners[i] < isovalue) {
                cubeIndex |= (1 << i);
            }
        }
        
        // Skip if entirely inside or outside
        if (cubeIndex == 0 || cubeIndex == 255) {
            return;
        }
        
        // Find edges that are intersected
        int edges = edgeTable[cubeIndex];
        
        // Calculate intersection points along edges
        Vector3[] intersectionPoints = new Vector3[12];
        for (int i = 0; i < 12; i++) {
            if ((edges & (1 << i)) != 0) {
                int corner1 = edgeConnections[i, 0];
                int corner2 = edgeConnections[i, 1];
                
                Vector3 p1 = new Vector3(x, y, z) + cornerOffsets[corner1];
                Vector3 p2 = new Vector3(x, y, z) + cornerOffsets[corner2];
                float v1 = corners[corner1];
                float v2 = corners[corner2];
                
                // Linear interpolation
                float t = (isovalue - v1) / (v2 - v1);
                intersectionPoints[i] = (p1 + t * (p2 - p1)) * voxelSize;
            }
        }
        
        // Generate triangles
        for (int i = 0; triTable[cubeIndex, i] != -1; i += 3) {
            int vertIndex = vertices.Count;
            
            vertices.Add(intersectionPoints[triTable[cubeIndex, i]]);
            vertices.Add(intersectionPoints[triTable[cubeIndex, i + 1]]);
            vertices.Add(intersectionPoints[triTable[cubeIndex, i + 2]]);
            
            triangles.Add(vertIndex);
            triangles.Add(vertIndex + 1);
            triangles.Add(vertIndex + 2);
        }
    }
}
```

### Usage Example

```csharp
public class TerrainGenerator : MonoBehaviour {
    public int size = 64;
    public float noiseScale = 0.1f;
    
    void Start() {
        // Create voxel grid
        float[,,] voxels = new float[size, size, size];
        
        // Fill with 3D noise
        for (int x = 0; x < size; x++) {
            for (int y = 0; y < size; y++) {
                for (int z = 0; z < size; z++) {
                    float noise = Perlin3D(
                        x * noiseScale, 
                        y * noiseScale, 
                        z * noiseScale
                    );
                    
                    // Add height gradient
                    voxels[x, y, z] = noise + (y / (float)size) * 2f;
                }
            }
        }
        
        // Generate mesh
        Mesh mesh = MarchingCubes.GenerateMesh(voxels, 0.5f, 1.0f);
        
        // Render
        GetComponent<MeshFilter>().mesh = mesh;
        GetComponent<MeshCollider>().sharedMesh = mesh;
    }
    
    float Perlin3D(float x, float y, float z) {
        float xy = Mathf.PerlinNoise(x, y);
        float xz = Mathf.PerlinNoise(x, z);
        float yz = Mathf.PerlinNoise(y, z);
        float yx = Mathf.PerlinNoise(y, x);
        float zx = Mathf.PerlinNoise(z, x);
        float zy = Mathf.PerlinNoise(z, y);
        
        return (xy + xz + yz + yx + zx + zy) / 6f;
    }
}
```

---

**Document Status:** Complete  
**Last Updated:** 2025-01-17  
**Research Hours:** 2-3 hours  
**Lines:** 623  
**Next Steps:** Consider Dual Contouring and Transvoxel for complementary techniques
