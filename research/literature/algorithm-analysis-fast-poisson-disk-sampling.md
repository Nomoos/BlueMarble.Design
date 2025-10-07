---
title: Algorithm Analysis - Fast Poisson Disk Sampling
date: 2025-01-17
tags: [algorithms, procedural-generation, sampling, distribution, vegetation, resources]
status: complete
priority: medium
source: Discovered from Sebastian Lague analysis
discovered-from: game-dev-analysis-sebastian-lague.md
---

# Algorithm Analysis: Fast Poisson Disk Sampling

## Executive Summary

**Paper Title:** Fast Poisson Disk Sampling in Arbitrary Dimensions  
**Author:** Robert Bridson  
**Published:** SIGGRAPH 2007 Sketches  
**Field:** Computer Graphics, Procedural Generation  
**Primary Application:** Generating evenly-distributed random points with minimum distance constraints

### Core Innovation

Fast Poisson Disk Sampling solves the problem of generating random point distributions that look natural (not too clustered, not too regular) in O(n) time, making it practical for real-time applications. The algorithm guarantees no two points are closer than a specified minimum distance while maintaining randomness.

### Key Characteristics

- **Fast:** O(n) time complexity vs O(n²) for naive approaches
- **Simple:** Easy to implement with clear pseudocode
- **Flexible:** Works in any number of dimensions (2D, 3D, etc.)
- **Quality:** Produces visually pleasing, natural-looking distributions
- **Predictable:** Guaranteed minimum distance between points

### Relevance to BlueMarble MMORPG

**Critical Applications:**
- **Vegetation Placement:** Trees, plants, rocks distributed naturally
- **Resource Distribution:** Ore deposits, herbs, collectibles
- **NPC Spawning:** Enemy and wildlife spawn points
- **Building Placement:** Initial settlement locations
- **Particle Effects:** Natural-looking particle distributions

**Priority Level:** Medium - Important for world generation quality

## Algorithm Overview

### The Problem

Traditional random point generation creates clusters and gaps that look unnatural. Uniform grids look too regular. We need:
1. Random distribution (not predictable patterns)
2. Minimum distance between points (no clustering)
3. Maximum coverage (few large gaps)
4. Fast generation (real-time capable)

### Visual Comparison

**Uniform Random (Bad):**
- Clusters of points too close together
- Large empty regions
- Unnatural appearance

**Grid Pattern (Bad):**
- Too regular
- Obvious patterns
- Artificial look

**Poisson Disk (Good):**
- Even distribution
- Natural randomness
- No clustering or large gaps
- Organic appearance

### Core Concept

The algorithm uses a **background grid** to accelerate distance queries. Instead of checking all existing points (O(n²)), we only check nearby cells in the grid (O(1) average case).

### Mathematical Foundation

**Poisson Disk Property:**
```
For any two points p1, p2 in the distribution:
distance(p1, p2) ≥ r

Where r is the minimum distance (radius)
```

**Grid Cell Size:**
```
cellSize = r / √n

Where:
- r = minimum distance
- n = number of dimensions (2 for 2D, 3 for 3D)
```

This ensures each grid cell contains at most one point, and we only need to check neighboring cells for distance validation.

## Core Implementation

### 2D Implementation

```csharp
using System.Collections.Generic;
using UnityEngine;

public class PoissonDiskSampling {
    public static List<Vector2> GeneratePoints(float radius, Vector2 sampleRegionSize, int numSamplesBeforeRejection = 30) {
        float cellSize = radius / Mathf.Sqrt(2);
        
        int[,] grid = new int[
            Mathf.CeilToInt(sampleRegionSize.x / cellSize),
            Mathf.CeilToInt(sampleRegionSize.y / cellSize)
        ];
        
        List<Vector2> points = new List<Vector2>();
        List<Vector2> spawnPoints = new List<Vector2>();
        
        // Start with random point in center
        spawnPoints.Add(sampleRegionSize / 2);
        
        while (spawnPoints.Count > 0) {
            int spawnIndex = Random.Range(0, spawnPoints.Count);
            Vector2 spawnCenter = spawnPoints[spawnIndex];
            bool candidateAccepted = false;
            
            // Try to generate point around spawn center
            for (int i = 0; i < numSamplesBeforeRejection; i++) {
                float angle = Random.value * Mathf.PI * 2;
                float distance = Random.Range(radius, 2 * radius);
                Vector2 dir = new Vector2(Mathf.Sin(angle), Mathf.Cos(angle));
                Vector2 candidate = spawnCenter + dir * distance;
                
                if (IsValid(candidate, sampleRegionSize, cellSize, radius, points, grid)) {
                    points.Add(candidate);
                    spawnPoints.Add(candidate);
                    grid[(int)(candidate.x / cellSize), (int)(candidate.y / cellSize)] = points.Count;
                    candidateAccepted = true;
                    break;
                }
            }
            
            if (!candidateAccepted) {
                spawnPoints.RemoveAt(spawnIndex);
            }
        }
        
        return points;
    }
    
    static bool IsValid(Vector2 candidate, Vector2 sampleRegionSize, float cellSize, float radius, List<Vector2> points, int[,] grid) {
        // Check if in bounds
        if (candidate.x >= 0 && candidate.x < sampleRegionSize.x && 
            candidate.y >= 0 && candidate.y < sampleRegionSize.y) {
            
            int cellX = (int)(candidate.x / cellSize);
            int cellY = (int)(candidate.y / cellSize);
            
            // Check neighboring cells
            int searchStartX = Mathf.Max(0, cellX - 2);
            int searchEndX = Mathf.Min(cellX + 2, grid.GetLength(0) - 1);
            int searchStartY = Mathf.Max(0, cellY - 2);
            int searchEndY = Mathf.Min(cellY + 2, grid.GetLength(1) - 1);
            
            for (int x = searchStartX; x <= searchEndX; x++) {
                for (int y = searchStartY; y <= searchEndY; y++) {
                    int pointIndex = grid[x, y] - 1;
                    
                    if (pointIndex != -1) {
                        float sqrDst = (candidate - points[pointIndex]).sqrMagnitude;
                        if (sqrDst < radius * radius) {
                            return false;
                        }
                    }
                }
            }
            
            return true;
        }
        
        return false;
    }
}
```

### 3D Implementation

```csharp
public class PoissonDiskSampling3D {
    public static List<Vector3> GeneratePoints(float radius, Vector3 sampleRegionSize, int numSamplesBeforeRejection = 30) {
        float cellSize = radius / Mathf.Sqrt(3); // √3 for 3D
        
        int[,,] grid = new int[
            Mathf.CeilToInt(sampleRegionSize.x / cellSize),
            Mathf.CeilToInt(sampleRegionSize.y / cellSize),
            Mathf.CeilToInt(sampleRegionSize.z / cellSize)
        ];
        
        List<Vector3> points = new List<Vector3>();
        List<Vector3> spawnPoints = new List<Vector3>();
        
        spawnPoints.Add(sampleRegionSize / 2);
        
        while (spawnPoints.Count > 0) {
            int spawnIndex = Random.Range(0, spawnPoints.Count);
            Vector3 spawnCenter = spawnPoints[spawnIndex];
            bool candidateAccepted = false;
            
            for (int i = 0; i < numSamplesBeforeRejection; i++) {
                // Random point on sphere surface
                Vector3 dir = Random.onUnitSphere;
                float distance = Random.Range(radius, 2 * radius);
                Vector3 candidate = spawnCenter + dir * distance;
                
                if (IsValid(candidate, sampleRegionSize, cellSize, radius, points, grid)) {
                    points.Add(candidate);
                    spawnPoints.Add(candidate);
                    grid[
                        (int)(candidate.x / cellSize), 
                        (int)(candidate.y / cellSize), 
                        (int)(candidate.z / cellSize)
                    ] = points.Count;
                    candidateAccepted = true;
                    break;
                }
            }
            
            if (!candidateAccepted) {
                spawnPoints.RemoveAt(spawnIndex);
            }
        }
        
        return points;
    }
    
    static bool IsValid(Vector3 candidate, Vector3 sampleRegionSize, float cellSize, float radius, List<Vector3> points, int[,,] grid) {
        if (candidate.x >= 0 && candidate.x < sampleRegionSize.x &&
            candidate.y >= 0 && candidate.y < sampleRegionSize.y &&
            candidate.z >= 0 && candidate.z < sampleRegionSize.z) {
            
            int cellX = (int)(candidate.x / cellSize);
            int cellY = (int)(candidate.y / cellSize);
            int cellZ = (int)(candidate.z / cellSize);
            
            int searchStartX = Mathf.Max(0, cellX - 2);
            int searchEndX = Mathf.Min(cellX + 2, grid.GetLength(0) - 1);
            int searchStartY = Mathf.Max(0, cellY - 2);
            int searchEndY = Mathf.Min(cellY + 2, grid.GetLength(1) - 1);
            int searchStartZ = Mathf.Max(0, cellZ - 2);
            int searchEndZ = Mathf.Min(cellZ + 2, grid.GetLength(2) - 1);
            
            for (int x = searchStartX; x <= searchEndX; x++) {
                for (int y = searchStartY; y <= searchEndY; y++) {
                    for (int z = searchStartZ; z <= searchEndZ; z++) {
                        int pointIndex = grid[x, y, z] - 1;
                        
                        if (pointIndex != -1) {
                            float sqrDst = (candidate - points[pointIndex]).sqrMagnitude;
                            if (sqrDst < radius * radius) {
                                return false;
                            }
                        }
                    }
                }
            }
            
            return true;
        }
        
        return false;
    }
}
```

## BlueMarble Application

### 1. Vegetation System

**Use Case:** Natural forest, plant, and rock distribution

**Implementation:**
```csharp
public class VegetationPlacer : MonoBehaviour {
    [SerializeField] private GameObject[] treePrefabs;
    [SerializeField] private Vector2 regionSize = new Vector2(100, 100);
    [SerializeField] private float minimumTreeDistance = 5f;
    [SerializeField] private float terrainHeightSampleRadius = 2f;
    
    public void GenerateVegetation() {
        List<Vector2> points = PoissonDiskSampling.GeneratePoints(
            minimumTreeDistance, 
            regionSize, 
            30
        );
        
        foreach (Vector2 point in points) {
            Vector3 worldPos = new Vector3(point.x, 0, point.y);
            
            // Sample terrain height
            RaycastHit hit;
            if (Physics.Raycast(worldPos + Vector3.up * 100, Vector3.down, out hit, 200f)) {
                worldPos = hit.point;
                
                // Check slope
                float slope = Vector3.Angle(hit.normal, Vector3.up);
                if (slope < 30f) { // Only place on flat-ish ground
                    // Select tree based on biome/height
                    GameObject treePrefab = SelectTreePrefab(worldPos);
                    
                    // Random rotation
                    Quaternion rotation = Quaternion.Euler(0, Random.Range(0f, 360f), 0);
                    
                    // Random scale variation
                    float scale = Random.Range(0.8f, 1.2f);
                    
                    GameObject tree = Instantiate(treePrefab, worldPos, rotation);
                    tree.transform.localScale = Vector3.one * scale;
                }
            }
        }
    }
    
    GameObject SelectTreePrefab(Vector3 position) {
        // Select based on biome, height, moisture, etc.
        return treePrefabs[Random.Range(0, treePrefabs.Length)];
    }
}
```

**Features:**
- Natural-looking forests
- No tree overlap
- Biome-specific vegetation
- Height and slope constraints
- Random scale and rotation

### 2. Resource Distribution

**Use Case:** Ore deposits, herbs, collectibles

**Implementation:**
```csharp
public class ResourceDistributor {
    public void PlaceResources(TerrainChunk chunk, ResourceType resourceType) {
        // Get resource configuration
        ResourceConfig config = GetResourceConfig(resourceType);
        
        // Generate points with Poisson sampling
        List<Vector2> points = PoissonDiskSampling.GeneratePoints(
            config.minDistance,
            new Vector2(chunk.size, chunk.size),
            30
        );
        
        // Apply density filter
        List<Vector2> filteredPoints = FilterByDensity(points, chunk, config);
        
        // Place resources
        foreach (Vector2 point in filteredPoints) {
            Vector3 worldPos = chunk.position + new Vector3(point.x, 0, point.y);
            
            // Sample terrain properties
            float height = GetTerrainHeight(worldPos);
            Biome biome = GetBiome(worldPos);
            
            // Check if valid location for this resource
            if (IsValidResourceLocation(resourceType, height, biome)) {
                PlaceResourceNode(resourceType, worldPos);
            }
        }
    }
    
    List<Vector2> FilterByDensity(List<Vector2> points, TerrainChunk chunk, ResourceConfig config) {
        List<Vector2> filtered = new List<Vector2>();
        
        foreach (Vector2 point in points) {
            // Use noise to create varying density
            float density = Mathf.PerlinNoise(
                (chunk.position.x + point.x) * 0.01f,
                (chunk.position.z + point.y) * 0.01f
            );
            
            // Higher density = more likely to spawn
            if (density > config.densityThreshold) {
                filtered.Add(point);
            }
        }
        
        return filtered;
    }
}
```

**Application:**
- Iron ore deposits
- Gold veins
- Herb patches
- Gem nodes
- Fishing spots

### 3. NPC and Enemy Spawning

**Use Case:** Natural enemy camp and wildlife distribution

**Implementation:**
```csharp
public class NPCSpawner : MonoBehaviour {
    public void SpawnEnemyCamp(Vector3 center, float radius, int minEnemies, int maxEnemies) {
        int enemyCount = Random.Range(minEnemies, maxEnemies);
        
        // Generate spawn points around camp center
        List<Vector2> localPoints = PoissonDiskSampling.GeneratePoints(
            3f, // Minimum 3m between enemies
            new Vector2(radius * 2, radius * 2),
            30
        );
        
        // Convert to world positions (centered on camp)
        List<Vector3> spawnPositions = new List<Vector3>();
        foreach (Vector2 point in localPoints) {
            Vector3 localPos = new Vector3(point.x - radius, 0, point.y - radius);
            Vector3 worldPos = center + localPos;
            
            // Only use points within circle
            if (localPos.magnitude <= radius) {
                spawnPositions.Add(worldPos);
            }
            
            if (spawnPositions.Count >= enemyCount) break;
        }
        
        // Spawn enemies
        foreach (Vector3 pos in spawnPositions) {
            SpawnEnemy(pos);
        }
        
        // Add camp structures (tents, fires, etc.)
        SpawnCampStructures(center, radius, spawnPositions);
    }
    
    void SpawnEnemy(Vector3 position) {
        // Instantiate enemy prefab
        // Set patrol radius
        // Configure AI behavior
    }
}
```

**Benefits:**
- Natural-looking enemy camps
- No overlapping spawn points
- Scalable for different camp sizes
- Works with patrol radius systems

### 4. Particle Effect Distribution

**Use Case:** Natural-looking particle systems

**Implementation:**
```csharp
public class ParticleDistributor : MonoBehaviour {
    public void CreateRainParticles(Vector3 center, float radius, int particleCount) {
        // Generate positions in 3D
        List<Vector3> positions = PoissonDiskSampling3D.GeneratePoints(
            2f, // Minimum distance between raindrops
            new Vector3(radius * 2, 20f, radius * 2), // Region size
            30
        );
        
        ParticleSystem.Particle[] particles = new ParticleSystem.Particle[positions.Count];
        
        for (int i = 0; i < positions.Count; i++) {
            Vector3 localPos = positions[i] - new Vector3(radius, 10f, radius);
            Vector3 worldPos = center + localPos;
            
            particles[i].position = worldPos;
            particles[i].velocity = Vector3.down * Random.Range(5f, 8f);
            particles[i].startSize = Random.Range(0.1f, 0.3f);
            particles[i].startLifetime = 2f;
            particles[i].remainingLifetime = Random.Range(0f, 2f);
        }
        
        ParticleSystem ps = GetComponent<ParticleSystem>();
        ps.SetParticles(particles, particles.Length);
    }
}
```

**Applications:**
- Rain/snow particles
- Dust clouds
- Magic effects
- Explosion debris

### 5. Building and Settlement Placement

**Use Case:** Initial city/settlement generation

**Implementation:**
```csharp
public class SettlementGenerator : MonoBehaviour {
    public void GenerateSettlement(Vector3 center, float radius) {
        // Generate building plots
        List<Vector2> plots = PoissonDiskSampling.GeneratePoints(
            15f, // Minimum distance between buildings
            new Vector2(radius * 2, radius * 2),
            30
        );
        
        // Central plaza
        Vector3 plazaCenter = center;
        
        foreach (Vector2 plot in plots) {
            Vector3 localPos = new Vector3(plot.x - radius, 0, plot.y - radius);
            Vector3 worldPos = center + localPos;
            
            float distanceFromCenter = localPos.magnitude;
            
            // Skip plaza area
            if (distanceFromCenter < 10f) continue;
            
            // Select building type based on distance from center
            BuildingType type = SelectBuildingType(distanceFromCenter, radius);
            
            // Orient toward plaza
            Vector3 directionToPlaza = (plazaCenter - worldPos).normalized;
            Quaternion rotation = Quaternion.LookRotation(directionToPlaza);
            
            PlaceBuilding(worldPos, rotation, type);
        }
        
        // Add roads connecting buildings
        GenerateRoadNetwork(plots, center, radius);
    }
}
```

**Features:**
- Organic settlement layout
- Buildings don't overlap
- Central plaza
- Road network generation
- Density gradients

## Advanced Techniques

### Weighted Distribution

**Concept:** Bias sampling toward certain areas using density maps

```csharp
public class WeightedPoissonSampling {
    public static List<Vector2> GenerateWeightedPoints(
        float radius, 
        Vector2 sampleRegionSize, 
        Texture2D densityMap,
        int numSamplesBeforeRejection = 30) {
        
        // Same as standard algorithm, but add density check
        // in the candidate acceptance phase
        
        List<Vector2> points = new List<Vector2>();
        // ... (standard setup)
        
        for (int i = 0; i < numSamplesBeforeRejection; i++) {
            Vector2 candidate = GenerateCandidate(spawnCenter, radius);
            
            if (IsValid(candidate, sampleRegionSize, cellSize, radius, points, grid)) {
                // Additional density check
                float density = SampleDensityMap(candidate, sampleRegionSize, densityMap);
                
                if (Random.value < density) {
                    points.Add(candidate);
                    // ... (add to grid and spawn points)
                    break;
                }
            }
        }
        
        return points;
    }
    
    static float SampleDensityMap(Vector2 position, Vector2 regionSize, Texture2D densityMap) {
        float u = position.x / regionSize.x;
        float v = position.y / regionSize.y;
        
        Color pixel = densityMap.GetPixelBilinear(u, v);
        return pixel.r; // Use red channel as density
    }
}
```

**BlueMarble Use:**
- More trees in forest biomes
- Resources concentrated near mountains
- Higher enemy density in dangerous zones

### Constrained Sampling

**Concept:** Generate points only within specific shapes or regions

```csharp
public class ConstrainedPoissonSampling {
    public static List<Vector2> GenerateInCircle(
        Vector2 center, 
        float radius, 
        float minDistance) {
        
        List<Vector2> points = PoissonDiskSampling.GeneratePoints(
            minDistance,
            new Vector2(radius * 2, radius * 2),
            30
        );
        
        // Filter to only points within circle
        List<Vector2> filtered = new List<Vector2>();
        foreach (Vector2 point in points) {
            Vector2 localPoint = point - new Vector2(radius, radius);
            if (localPoint.magnitude <= radius) {
                filtered.Add(center + localPoint);
            }
        }
        
        return filtered;
    }
    
    public static List<Vector2> GenerateInPolygon(
        Vector2[] polygon, 
        float minDistance) {
        
        // Get bounding box
        Bounds bounds = CalculateBounds(polygon);
        
        List<Vector2> points = PoissonDiskSampling.GeneratePoints(
            minDistance,
            bounds.size,
            30
        );
        
        // Filter to only points inside polygon
        List<Vector2> filtered = new List<Vector2>();
        foreach (Vector2 point in points) {
            Vector2 worldPoint = bounds.min + point;
            if (IsPointInPolygon(worldPoint, polygon)) {
                filtered.Add(worldPoint);
            }
        }
        
        return filtered;
    }
}
```

**BlueMarble Use:**
- Resources within specific territories
- NPCs within town boundaries
- Vegetation in designated zones

## Performance Optimization

### Chunked Generation

```csharp
public class ChunkedPoissonSampling {
    public static Dictionary<Vector2Int, List<Vector2>> GenerateChunked(
        Vector2 totalSize,
        int chunkSize,
        float radius) {
        
        Dictionary<Vector2Int, List<Vector2>> chunks = new Dictionary<Vector2Int, List<Vector2>>();
        
        int chunksX = Mathf.CeilToInt(totalSize.x / chunkSize);
        int chunksY = Mathf.CeilToInt(totalSize.y / chunkSize);
        
        for (int cx = 0; cx < chunksX; cx++) {
            for (int cy = 0; cy < chunksY; cy++) {
                Vector2Int chunkCoord = new Vector2Int(cx, cy);
                Vector2 chunkOffset = new Vector2(cx * chunkSize, cy * chunkSize);
                
                List<Vector2> points = PoissonDiskSampling.GeneratePoints(
                    radius,
                    new Vector2(chunkSize, chunkSize),
                    30
                );
                
                // Convert to world coordinates
                for (int i = 0; i < points.Count; i++) {
                    points[i] += chunkOffset;
                }
                
                chunks[chunkCoord] = points;
            }
        }
        
        return chunks;
    }
}
```

### Async Generation

```csharp
public class AsyncPoissonSampling : MonoBehaviour {
    public async Task<List<Vector2>> GenerateAsync(float radius, Vector2 regionSize) {
        return await Task.Run(() => {
            return PoissonDiskSampling.GeneratePoints(radius, regionSize, 30);
        });
    }
    
    public async void GenerateVegetationAsync() {
        List<Vector2> points = await GenerateAsync(5f, new Vector2(100, 100));
        
        // Back on main thread - instantiate objects
        foreach (Vector2 point in points) {
            PlaceTree(point);
        }
    }
}
```

## References

### Primary Source

**Paper:** "Fast Poisson Disk Sampling in Arbitrary Dimensions"  
**Author:** Robert Bridson  
**Published:** SIGGRAPH 2007  
**PDF:** Available from University of British Columbia  

### Implementation References

**Sebastian Lague Tutorial:**
- "Coding Adventure: Procedural Terrain Generation"
- Uses Poisson sampling for vegetation placement
- Visual demonstration of algorithm

**Online Resources:**
1. **Paul Bourke's Website**
   - http://paulbourke.net/texture/randomtile/
   - Visual examples and discussion

2. **Interactive Demos**
   - https://www.jasondavies.com/poisson-disc/
   - Live visualization of algorithm

3. **Unity Asset Store**
   - Several packages implement Poisson sampling
   - "Poisson Disc Sampling" by various authors

### Related Algorithms

**Blue Noise:**
- More complex but higher quality
- Better for high-quality rendering
- Slower generation

**Bridson's Algorithm Variants:**
- Adaptive sampling (varying radius)
- Boundary-aware sampling
- Multi-class sampling (different object types)

## Related Research

### Within BlueMarble Repository

- [game-dev-analysis-sebastian-lague.md](./game-dev-analysis-sebastian-lague.md) - Source of discovery
- [algorithm-analysis-marching-cubes.md](./algorithm-analysis-marching-cubes.md) - Terrain modification
- [code-analysis-sebastian-lague-github.md](./code-analysis-sebastian-lague-github.md) - Code examples

### Applications in Game Development

**Vegetation Systems:**
- SpeedTree integration
- Unity Terrain Trees
- Custom foliage systems

**Procedural Generation:**
- Dungeon room placement
- City building generation
- Object scattering

### Future Research

**Advanced Topics:**
1. **Multi-class Sampling** - Different objects with different radii
2. **Temporal Sampling** - Animation and dynamic distribution
3. **GPU Implementation** - Real-time generation on GPU
4. **Adaptive Sampling** - Varying density based on terrain

---

**Document Status:** Complete  
**Last Updated:** 2025-01-17  
**Research Hours:** 2-3 hours  
**Lines:** 563  
**Next Steps:** Implement in BlueMarble vegetation system
