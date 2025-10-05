# Geometry Clipmaps - Terrain Rendering Analysis

---
title: Geometry Clipmaps - Terrain Rendering Using Nested Regular Grids
date: 2025-01-15
tags: [game-dev, terrain, LOD, clipmaps, rendering, optimization]
status: complete
priority: high
discovered-from: Real-Time Rendering research (Assignment Group 09)
assignment-group: 09-discovered
source-type: technical-paper
---

## Executive Summary

"Geometry Clipmaps: Terrain Rendering Using Nested Regular Grids" by Losasso and Hoppe (2004) presents an elegant solution for rendering massive terrain datasets by maintaining constant geometric detail around the viewer. This analysis examines the clipmap algorithm and its application to BlueMarble's planetary-scale terrain rendering.

**Key Takeaways:**
- Nested regular grids provide constant detail around viewer regardless of position
- Ring-buffer toroidal topology enables efficient incremental updates
- Predictable memory footprint independent of terrain size
- GPU-friendly regular grid structure ideal for modern hardware
- Seamless integration with texture clipmaps for material data
- Minimal CPU overhead through geometric simplicity

**Relevance to BlueMarble:**
Geometry clipmaps are ideally suited for BlueMarble's planetary terrain requirements, offering predictable performance and memory usage while maintaining high detail near the player and gracefully degrading detail at distance.

---

## Source Overview

**Primary Source:**
- **Title:** Geometry Clipmaps: Terrain Rendering Using Nested Regular Grids
- **Authors:** Frank Losasso, Hugues Hoppe
- **Published:** ACM SIGGRAPH 2004
- **Pages:** 769-776
- **DOI:** 10.1145/1186562.1015799

**Research Focus:**
This analysis concentrates on the clipmap data structure, incremental update algorithms, rendering techniques, and integration strategies relevant to BlueMarble's real-time planetary terrain visualization.

---

## Core Concepts

### 1. Clipmap Data Structure

#### Nested Grid Levels

Clipmaps use nested square grids centered on the viewer:

**Level Structure:**
```cpp
struct ClipmapLevel {
    int level;                    // 0 = finest
    float gridSpacing;            // Meters between vertices
    int gridSize;                 // Typically 255 or 511
    
    std::vector<float> heights;   // Height values (ring buffer)
    Vector2i centerIndex;         // Current center in source data
    
    // Ring buffer offsets for toroidal topology
    int offsetX, offsetY;
};

class GeometryClipmap {
    std::vector<ClipmapLevel> levels;
    int numLevels;
    
    static const int GRID_SIZE = 255;  // Must be odd for centering
    
public:
    GeometryClipmap(int numLevels, float baseSpacing) {
        for (int i = 0; i < numLevels; i++) {
            ClipmapLevel level;
            level.level = i;
            level.gridSpacing = baseSpacing * pow(2.0f, i);
            level.gridSize = GRID_SIZE;
            level.heights.resize(GRID_SIZE * GRID_SIZE);
            levels.push_back(level);
        }
    }
};
```

**Example Configuration:**
```
Level 0:  255x255 grid,   1m spacing = 255m coverage
Level 1:  255x255 grid,   2m spacing = 510m coverage
Level 2:  255x255 grid,   4m spacing = 1020m coverage
Level 3:  255x255 grid,   8m spacing = 2040m coverage
Level 4:  255x255 grid,  16m spacing = 4080m coverage
```

### 2. Toroidal Topology and Updates

#### Ring Buffer Implementation

**Toroidal Indexing:**
```cpp
class ToroidalGrid {
    std::vector<float> data;
    int size;
    int offsetX, offsetY;
    
public:
    // Get value at logical grid position
    float get(int x, int y) const {
        int physicalX = (x + offsetX) % size;
        int physicalY = (y + offsetY) % size;
        if (physicalX < 0) physicalX += size;
        if (physicalY < 0) physicalY += size;
        return data[physicalY * size + physicalX];
    }
    
    // Set value at logical grid position
    void set(int x, int y, float value) {
        int physicalX = (x + offsetX) % size;
        int physicalY = (y + offsetY) % size;
        if (physicalX < 0) physicalX += size;
        if (physicalY < 0) physicalY += size;
        data[physicalY * size + physicalX] = value;
    }
    
    // Update grid center (incremental)
    void updateCenter(const Vector2i& newCenter, 
                      const Vector2i& oldCenter) {
        Vector2i delta = newCenter - oldCenter;
        
        // Update only changed regions
        if (delta.x > 0) {
            updateColumn(newCenter.x + size/2, delta.x);
        } else if (delta.x < 0) {
            updateColumn(newCenter.x - size/2, -delta.x);
        }
        
        if (delta.y > 0) {
            updateRow(newCenter.y + size/2, delta.y);
        } else if (delta.y < 0) {
            updateRow(newCenter.y - size/2, -delta.y);
        }
        
        offsetX += delta.x;
        offsetY += delta.y;
    }
};
```

### 3. Mesh Generation

#### Regular Grid with Gaps

**Level Rendering Geometry:**
```cpp
void generateClipmapMesh(ClipmapLevel& level, 
                         const Vector3& viewerPos) {
    int halfSize = level.gridSize / 2;
    
    // Interior (M×M) region - full resolution
    for (int y = -halfSize; y < halfSize; y++) {
        for (int x = -halfSize; x < halfSize; x++) {
            // Skip area covered by finer level
            if (level.level > 0 && isInFinerRegion(x, y)) {
                continue;
            }
            
            addQuad(x, y, level);
        }
    }
    
    // Transition region (vertical strips)
    // Blend between this level and next coarser level
    addTransitionStrip(level, TransitionSide::Left);
    addTransitionStrip(level, TransitionSide::Right);
    addTransitionStrip(level, TransitionSide::Top);
    addTransitionStrip(level, TransitionSide::Bottom);
}

void addQuad(int x, int y, const ClipmapLevel& level) {
    // Create two triangles for quad
    Vector3 v00 = getVertex(x,   y,   level);
    Vector3 v10 = getVertex(x+1, y,   level);
    Vector3 v01 = getVertex(x,   y+1, level);
    Vector3 v11 = getVertex(x+1, y+1, level);
    
    // Triangle 1
    addTriangle(v00, v10, v11);
    // Triangle 2
    addTriangle(v00, v11, v01);
}
```

#### Transition Regions

**Preventing Cracks Between Levels:**
```cpp
void addTransitionStrip(ClipmapLevel& level, TransitionSide side) {
    // Transition strip uses 2:1 vertex ratio
    // Coarser level has half the vertices
    
    int halfSize = level.gridSize / 2;
    
    if (side == TransitionSide::Right) {
        for (int y = -halfSize; y < halfSize; y += 2) {
            // Inner edge (full resolution)
            Vector3 v0 = getVertex(halfSize-1, y,   level);
            Vector3 v1 = getVertex(halfSize-1, y+1, level);
            Vector3 v2 = getVertex(halfSize-1, y+2, level);
            
            // Outer edge (half resolution - matches coarser level)
            Vector3 v3 = getVertex(halfSize, y+1, level);
            
            // Two triangles forming transition
            addTriangle(v0, v3, v1);
            addTriangle(v1, v3, v2);
        }
    }
    // Similar for other sides...
}
```

### 4. GPU Implementation

#### Vertex Shader with Texture Fetching

**Clipmap Vertex Shader:**
```glsl
#version 450

layout(location = 0) in vec2 gridPosition;  // Logical grid position
layout(location = 1) in int levelIndex;

uniform sampler2D heightTextures[8];  // One per level
uniform vec2 viewerPosition;
uniform float baseGridSpacing;

out vec3 worldPosition;
out vec3 worldNormal;

vec2 toroidalWrap(vec2 pos, float size) {
    return mod(pos + size * 0.5, size) - size * 0.5;
}

void main() {
    float gridSpacing = baseGridSpacing * pow(2.0, float(levelIndex));
    
    // World position
    vec2 worldXZ = (gridPosition + viewerPosition) * gridSpacing;
    
    // Sample height from texture
    vec2 texCoord = toroidalWrap(gridPosition, 255.0) / 255.0;
    float height = texture(heightTextures[levelIndex], texCoord).r;
    
    worldPosition = vec3(worldXZ.x, height, worldXZ.y);
    
    // Calculate normal from neighboring heights
    float heightL = textureOffset(heightTextures[levelIndex], texCoord, ivec2(-1, 0)).r;
    float heightR = textureOffset(heightTextures[levelIndex], texCoord, ivec2(1, 0)).r;
    float heightD = textureOffset(heightTextures[levelIndex], texCoord, ivec2(0, -1)).r;
    float heightU = textureOffset(heightTextures[levelIndex], texCoord, ivec2(0, 1)).r;
    
    vec3 tangent = normalize(vec3(2.0 * gridSpacing, heightR - heightL, 0.0));
    vec3 bitangent = normalize(vec3(0.0, heightU - heightD, 2.0 * gridSpacing));
    worldNormal = normalize(cross(tangent, bitangent));
    
    gl_Position = viewProjection * vec4(worldPosition, 1.0);
}
```

### 5. Streaming and Data Management

#### Asynchronous Height Data Loading

**Streaming System:**
```cpp
class ClipmapStreaming {
    struct LoadRequest {
        int level;
        Vector2i chunkCoord;
        std::vector<float> data;
        bool completed;
    };
    
    std::queue<LoadRequest> pendingRequests;
    std::queue<LoadRequest> completedRequests;
    std::thread loaderThread;
    
public:
    void requestUpdate(int level, const Vector2i& center) {
        // Determine which chunks need loading
        Vector2i chunkSize(64, 64);
        Vector2i chunkCoord = center / chunkSize;
        
        LoadRequest request;
        request.level = level;
        request.chunkCoord = chunkCoord;
        request.completed = false;
        
        pendingRequests.push(request);
    }
    
    void processCompletedRequests() {
        while (!completedRequests.empty()) {
            auto request = completedRequests.front();
            completedRequests.pop();
            
            // Upload to GPU texture
            uploadToTexture(request.level, request.chunkCoord, 
                          request.data);
        }
    }
    
private:
    void loaderThreadFunc() {
        while (running) {
            if (!pendingRequests.empty()) {
                auto request = pendingRequests.front();
                pendingRequests.pop();
                
                // Load from disk/generate
                request.data = loadHeightData(request.level, 
                                             request.chunkCoord);
                request.completed = true;
                
                completedRequests.push(request);
            }
        }
    }
};
```

---

## BlueMarble Application

### Planetary Clipmap Configuration

**Multi-Scale Setup:**
```cpp
class PlanetaryClipmapSystem {
    static const int NUM_LEVELS = 12;
    static const float BASE_SPACING = 1.0f;  // 1 meter
    
    GeometryClipmap clipmap;
    
public:
    PlanetaryClipmapSystem() 
        : clipmap(NUM_LEVELS, BASE_SPACING) {
        // Level 0:  255m (1m spacing)
        // Level 11: 522km (2048m spacing)
        // Total coverage: ~1044km diameter
    }
    
    void update(const Vector3& playerPosition) {
        for (int level = 0; level < NUM_LEVELS; level++) {
            Vector2i newCenter = worldToGrid(playerPosition, level);
            
            if (needsUpdate(level, newCenter)) {
                updateLevel(level, newCenter);
            }
        }
    }
};
```

### Integration with Texture Clipmaps

**Combined Geometry and Texture Streaming:**
```cpp
struct CombinedClipmap {
    GeometryClipmap geometryLevels;
    TextureClipmap diffuseLevels;
    TextureClipmap normalLevels;
    
    void synchronizedUpdate(const Vector3& position) {
        // Update all clipmaps together
        for (int level = 0; level < numLevels; level++) {
            Vector2i center = worldToGrid(position, level);
            
            geometryLevels.updateLevel(level, center);
            diffuseLevels.updateLevel(level, center);
            normalLevels.updateLevel(level, center);
        }
    }
};
```

---

## Implementation Recommendations

### Phase 1: Core System (Weeks 1-2)

1. **Clipmap Data Structure:**
   - Toroidal grid implementation
   - Multi-level management
   - **Deliverable:** Working clipmap structure

2. **Basic Rendering:**
   - Regular grid mesh generation
   - Transition strip handling
   - **Deliverable:** Crack-free level transitions

### Phase 2: GPU Optimization (Weeks 3-4)

3. **Shader-Based Implementation:**
   - Vertex shader with texture fetching
   - Normal calculation
   - **Deliverable:** GPU-driven rendering

4. **Streaming System:**
   - Asynchronous data loading
   - Texture upload management
   - **Deliverable:** Smooth updates during movement

---

## References

### Primary Paper

1. Losasso, F., & Hoppe, H. (2004). "Geometry Clipmaps: Terrain Rendering Using Nested Regular Grids." *ACM SIGGRAPH*, 769-776.

### Related Work

1. Tanner, C., Migdal, C., & Jones, M. (1998). "The Clipmap: A Virtual Mipmap." *ACM SIGGRAPH*.
   - Original clipmap concept for textures

2. Asirvatham, A., & Hoppe, H. (2005). "Terrain Rendering Using GPU-Based Geometry Clipmaps." *GPU Gems 2*.
   - GPU implementation details

---

## Discovered Sources

**1. GPU Gems 2 - Terrain Rendering Chapter** (NVIDIA)
- **Priority:** High
- **Category:** GameDev-Tech
- **Rationale:** Detailed GPU implementation of geometry clipmaps with code examples. Essential for practical implementation in BlueMarble.
- **Estimated Effort:** 2-3 hours

---

## Related Research

### Internal Documentation

- `game-dev-analysis-real-time-rendering.md` - LOD systems overview
- `game-dev-analysis-bdam-terrain-visualization.md` - Alternative adaptive mesh approach
- `game-dev-analysis-3d-mathematics.md` - Geographic coordinate systems

---

**Document Status:** Complete  
**Last Updated:** 2025-01-15  
**Estimated Research Time:** 3 hours  
**Document Length:** 450+ lines  
**Discovered From:** Real-Time Rendering research (Assignment Group 09, Topic 1)  
**Source Type:** Technical Paper
