# Step 7: Rendering & LOD Strategy Guide

This guide provides comprehensive technical guidance on rendering planet-scale worlds with efficient Level of Detail (LOD) systems, expanding on principles from the [Planet-Scale MMORPG GIS Architecture and Simulation Plan](../../ArchitectureAndSimulationPlan.md).

## Overview

Rendering a planet-sized voxel world requires intelligent LOD management, efficient culling, and adaptive mesh generation. This guide details proven techniques for maintaining 60 FPS while rendering massive terrains.

---

## 1. Multi-Resolution Terrain System

### LOD Level Design

```csharp
/// <summary>
/// LOD levels with automatic transitions based on distance.
/// </summary>
public enum TerrainLOD
{
    Ultra = 0,    // 0.25m voxels - 0-50m distance
    High = 1,     // 1m resolution - 50-200m
    Medium = 2,   // 4m resolution - 200-1000m
    Low = 3,      // 16m resolution - 1-5km
    VeryLow = 4,  // 64m resolution - 5-20km
    Minimal = 5   // 256m resolution - 20km+
}

public class LODSettings
{
    public static readonly float[] LOD_DISTANCES = 
    {
        50f,      // Ultra → High
        200f,     // High → Medium
        1000f,    // Medium → Low
        5000f,    // Low → VeryLow
        20000f    // VeryLow → Minimal
    };
    
    public static readonly float[] VOXEL_SIZES = 
    {
        0.25f,    // Ultra
        1f,       // High
        4f,       // Medium
        16f,      // Low
        64f,      // VeryLow
        256f      // Minimal
    };
}
```

### LOD Selection Algorithm

```csharp
/// <summary>
/// Calculate appropriate LOD level for a chunk based on camera distance.
/// </summary>
public class LODSelector
{
    /// <summary>
    /// Select LOD based on distance with hysteresis to prevent flickering.
    /// </summary>
    public static TerrainLOD SelectLOD(
        Vector3 chunkCenter,
        Vector3 cameraPosition,
        TerrainLOD currentLOD)
    {
        float distance = Vector3.Distance(chunkCenter, cameraPosition);
        
        // Apply hysteresis (10% buffer) to prevent rapid LOD switching
        const float HYSTERESIS = 1.1f;
        
        for (int i = 0; i < LODSettings.LOD_DISTANCES.Length; i++)
        {
            float threshold = LODSettings.LOD_DISTANCES[i];
            
            // If transitioning to lower detail, use larger threshold
            if (i > (int)currentLOD)
                threshold *= HYSTERESIS;
            // If transitioning to higher detail, use smaller threshold
            else if (i < (int)currentLOD)
                threshold /= HYSTERESIS;
            
            if (distance < threshold)
                return (TerrainLOD)i;
        }
        
        return TerrainLOD.Minimal;
    }
    
    /// <summary>
    /// Calculate LOD based on screen space error.
    /// More accurate than simple distance-based LOD.
    /// </summary>
    public static TerrainLOD SelectLODByScreenError(
        Vector3 chunkCenter,
        Vector3 cameraPosition,
        float fieldOfView,
        int screenHeight,
        float maxScreenError = 2.0f)
    {
        float distance = Vector3.Distance(chunkCenter, cameraPosition);
        
        // Calculate projected error in screen space
        // screenError = (geometricError / distance) * (screenHeight / (2 * tan(fov/2)))
        float fovRadians = fieldOfView * Mathf.Deg2Rad;
        float k = screenHeight / (2.0f * Mathf.Tan(fovRadians / 2.0f));
        
        // Test each LOD level
        for (int i = 0; i < LODSettings.VOXEL_SIZES.Length - 1; i++)
        {
            float geometricError = LODSettings.VOXEL_SIZES[i];
            float screenError = (geometricError / distance) * k;
            
            if (screenError > maxScreenError)
                return (TerrainLOD)i;
        }
        
        return TerrainLOD.Minimal;
    }
}
```

### Seamless LOD Transitions

```csharp
/// <summary>
/// Manages smooth LOD transitions without popping artifacts.
/// </summary>
public class LODTransitionManager
{
    private class ChunkLODState
    {
        public TerrainLOD CurrentLOD { get; set; }
        public TerrainLOD TargetLOD { get; set; }
        public float TransitionProgress { get; set; } // 0-1
        public Mesh CurrentMesh { get; set; }
        public Mesh NextMesh { get; set; }
    }
    
    private Dictionary<ChunkId, ChunkLODState> chunkStates;
    private const float TRANSITION_DURATION = 0.5f; // 0.5 seconds
    
    /// <summary>
    /// Update LOD transitions with alpha blending.
    /// </summary>
    public void Update(float deltaTime)
    {
        foreach (var state in chunkStates.Values)
        {
            if (state.CurrentLOD != state.TargetLOD)
            {
                // Update transition progress
                state.TransitionProgress += deltaTime / TRANSITION_DURATION;
                
                if (state.TransitionProgress >= 1.0f)
                {
                    // Transition complete
                    state.CurrentLOD = state.TargetLOD;
                    state.CurrentMesh = state.NextMesh;
                    state.NextMesh = null;
                    state.TransitionProgress = 0.0f;
                }
                else
                {
                    // Blend between LODs
                    RenderBlendedLOD(state);
                }
            }
        }
    }
    
    private void RenderBlendedLOD(ChunkLODState state)
    {
        // Render both meshes with alpha blending
        float alpha = state.TransitionProgress;
        
        // Current LOD fades out
        RenderMesh(state.CurrentMesh, 1.0f - alpha);
        
        // Next LOD fades in
        if (state.NextMesh != null)
            RenderMesh(state.NextMesh, alpha);
    }
}
```

---

## 2. Octree/Quadtree Partitioning

### Quadtree for 2D Surface

```csharp
/// <summary>
/// Quadtree node for terrain surface LOD management.
/// </summary>
public class TerrainQuadtreeNode
{
    public Bounds2D Bounds { get; set; }
    public TerrainLOD LODLevel { get; set; }
    public TerrainQuadtreeNode[] Children { get; set; } // 4 children
    public TerrainMesh Mesh { get; set; }
    public bool IsLeaf => Children == null;
    
    private const int MAX_DEPTH = 10;
    
    /// <summary>
    /// Subdivide node based on camera distance.
    /// </summary>
    public void UpdateLOD(Vector3 cameraPosition, int currentDepth)
    {
        Vector3 nodeCenter = new Vector3(
            Bounds.Center.x,
            0,
            Bounds.Center.y
        );
        
        float distance = Vector3.Distance(nodeCenter, cameraPosition);
        float nodeSize = Bounds.Size.x;
        
        // Decide whether to subdivide
        bool shouldSubdivide = ShouldSubdivide(distance, nodeSize, currentDepth);
        
        if (shouldSubdivide && IsLeaf && currentDepth < MAX_DEPTH)
        {
            // Create children
            Subdivide();
        }
        else if (!shouldSubdivide && !IsLeaf)
        {
            // Merge children
            Merge();
        }
        
        // Recursively update children
        if (!IsLeaf)
        {
            foreach (var child in Children)
            {
                child.UpdateLOD(cameraPosition, currentDepth + 1);
            }
        }
    }
    
    private bool ShouldSubdivide(float distance, float nodeSize, int depth)
    {
        // Use geometric error metric
        // Subdivide if node's screen-space error exceeds threshold
        float geometricError = nodeSize;
        float threshold = 50.0f; // meters at viewing distance
        
        return geometricError / distance > threshold / 1000.0f && depth < MAX_DEPTH;
    }
    
    private void Subdivide()
    {
        Children = new TerrainQuadtreeNode[4];
        Vector2 center = Bounds.Center;
        Vector2 halfSize = Bounds.Size / 2f;
        
        // NW, NE, SW, SE
        Children[0] = new TerrainQuadtreeNode
        {
            Bounds = new Bounds2D(
                new Vector2(center.x - halfSize.x / 2, center.y + halfSize.y / 2),
                halfSize
            )
        };
        
        Children[1] = new TerrainQuadtreeNode
        {
            Bounds = new Bounds2D(
                new Vector2(center.x + halfSize.x / 2, center.y + halfSize.y / 2),
                halfSize
            )
        };
        
        Children[2] = new TerrainQuadtreeNode
        {
            Bounds = new Bounds2D(
                new Vector2(center.x - halfSize.x / 2, center.y - halfSize.y / 2),
                halfSize
            )
        };
        
        Children[3] = new TerrainQuadtreeNode
        {
            Bounds = new Bounds2D(
                new Vector2(center.x + halfSize.x / 2, center.y - halfSize.y / 2),
                halfSize
            )
        };
    }
    
    private void Merge()
    {
        Children = null;
    }
}
```

### Octree for 3D Voxels

```csharp
/// <summary>
/// Octree for 3D voxel LOD management.
/// </summary>
public class VoxelOctreeNode
{
    public Bounds3D Bounds { get; set; }
    public VoxelOctreeNode[] Children { get; set; } // 8 children
    public VoxelMesh Mesh { get; set; }
    public bool IsVisible { get; set; }
    
    /// <summary>
    /// Frustum culling for octree nodes.
    /// </summary>
    public void FrustumCull(Plane[] frustumPlanes)
    {
        // Test if node bounds intersect frustum
        IsVisible = GeometryUtility.TestPlanesAABB(frustumPlanes, Bounds.ToBounds());
        
        // Recursively cull children
        if (IsVisible && Children != null)
        {
            foreach (var child in Children)
            {
                child.FrustumCull(frustumPlanes);
            }
        }
    }
    
    /// <summary>
    /// Collect visible nodes for rendering.
    /// </summary>
    public void CollectVisibleNodes(List<VoxelOctreeNode> visibleNodes)
    {
        if (!IsVisible)
            return;
        
        if (Children == null)
        {
            // Leaf node - add to render list
            visibleNodes.Add(this);
        }
        else
        {
            // Recurse to children
            foreach (var child in Children)
            {
                child.CollectVisibleNodes(visibleNodes);
            }
        }
    }
}
```

---

## 3. Origin-Relative Rendering

### Camera-Relative Mesh Rendering

```csharp
/// <summary>
/// Render meshes relative to camera position to avoid precision loss.
/// </summary>
public class OriginRelativeRenderer
{
    private WorldPosition renderOrigin;
    
    /// <summary>
    /// Render frame with camera-relative coordinates.
    /// </summary>
    public void RenderFrame(
        Camera camera,
        WorldPosition cameraWorldPos,
        List<RenderableChunk> chunks)
    {
        // Update render origin if needed
        if (ShouldShiftOrigin(cameraWorldPos, renderOrigin))
        {
            ShiftRenderOrigin(cameraWorldPos);
        }
        
        // Set camera to local origin
        camera.transform.position = Vector3.zero;
        
        // Render chunks relative to camera
        foreach (var chunk in chunks)
        {
            // Convert world position to local position
            Vector3 localPos = chunk.WorldPos.ToLocalFloat(cameraWorldPos);
            
            // Check if within renderable range (< 10km for precision)
            if (localPos.magnitude < 10000f)
            {
                // Render mesh at local position
                Graphics.DrawMesh(
                    chunk.Mesh,
                    Matrix4x4.TRS(localPos, chunk.Rotation, Vector3.one),
                    chunk.Material,
                    chunk.Layer
                );
            }
        }
    }
    
    private bool ShouldShiftOrigin(WorldPosition current, WorldPosition origin)
    {
        const double SHIFT_THRESHOLD = 5000.0; // 5km
        return current.DistanceTo(origin) > SHIFT_THRESHOLD;
    }
    
    private void ShiftRenderOrigin(WorldPosition newOrigin)
    {
        renderOrigin = newOrigin;
        // Notify all systems of origin shift
        EventBus.Publish(new OriginShiftEvent { NewOrigin = newOrigin });
    }
}
```

---

## 4. Frustum and Occlusion Culling

### Frustum Culling

```csharp
/// <summary>
/// Efficient frustum culling for chunks.
/// </summary>
public class FrustumCuller
{
    /// <summary>
    /// Extract frustum planes from camera.
    /// </summary>
    public static Plane[] CalculateFrustumPlanes(Camera camera)
    {
        return GeometryUtility.CalculateFrustumPlanes(camera);
    }
    
    /// <summary>
    /// Test if chunk is visible in frustum.
    /// </summary>
    public static bool IsChunkVisible(
        Plane[] frustumPlanes,
        Vector3 chunkCenter,
        float chunkRadius)
    {
        Bounds bounds = new Bounds(chunkCenter, Vector3.one * chunkRadius * 2);
        return GeometryUtility.TestPlanesAABB(frustumPlanes, bounds);
    }
    
    /// <summary>
    /// Batch frustum cull multiple chunks.
    /// </summary>
    public static List<ChunkId> CullChunks(
        Plane[] frustumPlanes,
        List<RenderableChunk> chunks)
    {
        var visibleChunks = new List<ChunkId>();
        
        foreach (var chunk in chunks)
        {
            if (IsChunkVisible(frustumPlanes, chunk.Center, chunk.Radius))
            {
                visibleChunks.Add(chunk.Id);
            }
        }
        
        return visibleChunks;
    }
}
```

### Occlusion Culling

```csharp
/// <summary>
/// Hardware occlusion queries for hidden chunk culling.
/// </summary>
public class OcclusionCuller
{
    private Dictionary<ChunkId, OcclusionQuery> activeQueries;
    
    public class OcclusionQuery
    {
        public ChunkId ChunkId { get; set; }
        public uint QueryId { get; set; }
        public bool IsVisible { get; set; }
        public int FramesSinceUpdate { get; set; }
    }
    
    /// <summary>
    /// Issue occlusion queries for chunks.
    /// </summary>
    public void IssueQueries(List<RenderableChunk> chunks)
    {
        foreach (var chunk in chunks)
        {
            if (!activeQueries.ContainsKey(chunk.Id))
            {
                // Create new query
                uint queryId = GL.GenQuery();
                activeQueries[chunk.Id] = new OcclusionQuery
                {
                    ChunkId = chunk.Id,
                    QueryId = queryId,
                    IsVisible = true // Assume visible initially
                };
            }
            
            var query = activeQueries[chunk.Id];
            
            // Issue query every N frames to reduce overhead
            if (query.FramesSinceUpdate > 5)
            {
                GL.BeginQuery(QueryTarget.SamplesPassed, query.QueryId);
                RenderBoundingBox(chunk.Bounds);
                GL.EndQuery(QueryTarget.SamplesPassed);
                query.FramesSinceUpdate = 0;
            }
            else
            {
                query.FramesSinceUpdate++;
            }
        }
    }
    
    /// <summary>
    /// Check query results and update visibility.
    /// </summary>
    public void UpdateResults()
    {
        foreach (var query in activeQueries.Values)
        {
            // Check if result is ready
            GL.GetQueryObject(query.QueryId, GetQueryObjectParam.QueryResultAvailable, out int available);
            
            if (available == 1)
            {
                // Get result
                GL.GetQueryObject(query.QueryId, GetQueryObjectParam.QueryResult, out int samplesPassed);
                query.IsVisible = samplesPassed > 0;
            }
        }
    }
    
    private void RenderBoundingBox(Bounds bounds)
    {
        // Render bounding box as simple geometry for occlusion test
        // Use minimal shader (no lighting, no textures)
    }
}
```

---

## 5. Multi-Threaded Mesh Generation

### Parallel Mesh Builder

```csharp
/// <summary>
/// Generate meshes for multiple chunks in parallel.
/// </summary>
public class ParallelMeshGenerator
{
    private readonly int workerCount;
    private BlockingCollection<MeshJob> jobQueue;
    private CancellationTokenSource cancellation;
    
    public class MeshJob
    {
        public ChunkId ChunkId { get; set; }
        public VoxelChunk VoxelData { get; set; }
        public TerrainLOD LOD { get; set; }
        public Action<Mesh> OnComplete { get; set; }
    }
    
    public ParallelMeshGenerator(int workers = 4)
    {
        workerCount = workers;
        jobQueue = new BlockingCollection<MeshJob>();
    }
    
    /// <summary>
    /// Start worker threads.
    /// </summary>
    public void Start()
    {
        cancellation = new CancellationTokenSource();
        
        for (int i = 0; i < workerCount; i++)
        {
            int workerId = i;
            Task.Run(() => WorkerLoop(workerId, cancellation.Token));
        }
    }
    
    /// <summary>
    /// Submit mesh generation job.
    /// </summary>
    public void QueueMeshGeneration(MeshJob job)
    {
        jobQueue.Add(job);
    }
    
    private void WorkerLoop(int workerId, CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {
            if (jobQueue.TryTake(out var job, 100))
            {
                try
                {
                    // Generate mesh on worker thread
                    Mesh mesh = GenerateMesh(job.VoxelData, job.LOD);
                    
                    // Callback on main thread
                    MainThreadDispatcher.Enqueue(() => job.OnComplete?.Invoke(mesh));
                }
                catch (Exception ex)
                {
                    Debug.LogError($"Worker {workerId} mesh generation failed: {ex}");
                }
            }
        }
    }
    
    private Mesh GenerateMesh(VoxelChunk voxelData, TerrainLOD lod)
    {
        var vertices = new List<Vector3>();
        var triangles = new List<int>();
        var normals = new List<Vector3>();
        var uvs = new List<Vector2>();
        
        int step = (int)Math.Pow(2, (int)lod); // LOD affects sampling rate
        
        // Greedy meshing algorithm
        for (int z = 0; z < voxelData.Size; z += step)
        {
            for (int y = 0; y < voxelData.Size; y += step)
            {
                for (int x = 0; x < voxelData.Size; x += step)
                {
                    byte voxel = voxelData.GetVoxel(x, y, z);
                    if (voxel == 0) continue; // Skip air
                    
                    // Check each face for visibility
                    AddVisibleFaces(vertices, triangles, normals, uvs,
                                  x, y, z, step, voxelData);
                }
            }
        }
        
        // Create mesh
        Mesh mesh = new Mesh();
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.normals = normals.ToArray();
        mesh.uv = uvs.ToArray();
        
        return mesh;
    }
}
```

---

## 6. GPU Ray-Marching (Advanced)

### Voxel Ray-Marching Shader

```hlsl
// VoxelRayMarcher.shader
// GPU-based voxel rendering using ray-marching

cbuffer Constants : register(b0)
{
    float4x4 InvViewProj;
    float3 CameraPosition;
    float MaxDistance;
};

Texture3D<uint> VoxelData : register(t0);
SamplerState VoxelSampler : register(s0);

struct PSInput
{
    float4 Position : SV_POSITION;
    float2 UV : TEXCOORD0;
};

float4 main(PSInput input) : SV_TARGET
{
    // Reconstruct world position from screen UV
    float4 clipPos = float4(input.UV * 2.0 - 1.0, 0.5, 1.0);
    float4 worldPos = mul(InvViewProj, clipPos);
    worldPos /= worldPos.w;
    
    // Ray direction
    float3 rayDir = normalize(worldPos.xyz - CameraPosition);
    float3 rayPos = CameraPosition;
    
    // Ray-march through voxel grid
    float t = 0.0;
    const float step = 0.25; // 0.25m per step
    const int maxSteps = 1000;
    
    for (int i = 0; i < maxSteps; i++)
    {
        // Sample voxel at current position
        float3 voxelCoord = rayPos / 0.25; // Convert to voxel space
        uint voxel = VoxelData.Sample(VoxelSampler, voxelCoord);
        
        if (voxel != 0)
        {
            // Hit! Calculate lighting and return color
            float3 normal = CalculateNormal(voxelCoord);
            float3 color = GetMaterialColor(voxel);
            float lighting = saturate(dot(normal, normalize(float3(1, 1, 1))));
            
            return float4(color * lighting, 1.0);
        }
        
        // Advance ray
        t += step;
        rayPos = CameraPosition + rayDir * t;
        
        if (t > MaxDistance)
            break;
    }
    
    // Miss - render sky
    return float4(0.5, 0.7, 1.0, 1.0);
}

float3 CalculateNormal(float3 voxelPos)
{
    // Sample neighboring voxels for gradient
    float3 offset = float3(1, 1, 1);
    float x1 = VoxelData.Sample(VoxelSampler, voxelPos + float3(offset.x, 0, 0));
    float x2 = VoxelData.Sample(VoxelSampler, voxelPos - float3(offset.x, 0, 0));
    float y1 = VoxelData.Sample(VoxelSampler, voxelPos + float3(0, offset.y, 0));
    float y2 = VoxelData.Sample(VoxelSampler, voxelPos - float3(0, offset.y, 0));
    float z1 = VoxelData.Sample(VoxelSampler, voxelPos + float3(0, 0, offset.z));
    float z2 = VoxelData.Sample(VoxelSampler, voxelPos - float3(0, 0, offset.z));
    
    return normalize(float3(x1 - x2, y1 - y2, z1 - z2));
}
```

---

## 7. Atmosphere & Skydome

### Atmospheric Scattering

```csharp
/// <summary>
/// Atmospheric scattering shader parameters.
/// </summary>
public class AtmosphereRenderer
{
    private Material skyMaterial;
    
    // Physical atmosphere parameters
    private const float PLANET_RADIUS = 6371000f; // meters
    private const float ATMOSPHERE_HEIGHT = 100000f; // 100km
    
    public void UpdateAtmosphere(Vector3 sunDirection, Vector3 cameraAltitude)
    {
        // Update shader parameters
        skyMaterial.SetVector("_SunDirection", sunDirection);
        skyMaterial.SetFloat("_CameraAltitude", cameraAltitude.y);
        skyMaterial.SetFloat("_PlanetRadius", PLANET_RADIUS);
        skyMaterial.SetFloat("_AtmosphereRadius", PLANET_RADIUS + ATMOSPHERE_HEIGHT);
        
        // Rayleigh scattering (blue sky)
        skyMaterial.SetVector("_RayleighScattering", new Vector3(5.8e-6f, 13.5e-6f, 33.1e-6f));
        
        // Mie scattering (haze)
        skyMaterial.SetFloat("_MieScattering", 21e-6f);
        skyMaterial.SetFloat("_MieAnisotropy", 0.76f);
    }
    
    public void RenderSky(Camera camera)
    {
        // Render full-screen quad with atmosphere shader
        Graphics.Blit(null, camera.activeTexture, skyMaterial);
    }
}
```

### Day-Night Cycle

```csharp
/// <summary>
/// Manage day-night cycle and celestial bodies.
/// </summary>
public class DayNightCycle
{
    private float timeOfDay = 0.5f; // 0-1, where 0.5 is noon
    private float dayDuration = 1200f; // 20 minutes real-time
    
    public Vector3 SunDirection { get; private set; }
    public Color SunColor { get; private set; }
    public float SunIntensity { get; private set; }
    
    public void Update(float deltaTime)
    {
        // Update time
        timeOfDay += deltaTime / dayDuration;
        timeOfDay %= 1.0f;
        
        // Calculate sun angle
        float angle = timeOfDay * 360f - 90f; // 0° = sunrise, 90° = noon
        float angleRad = angle * Mathf.Deg2Rad;
        
        SunDirection = new Vector3(
            Mathf.Cos(angleRad),
            Mathf.Sin(angleRad),
            0
        ).normalized;
        
        // Calculate sun color and intensity
        float sunHeight = SunDirection.y;
        
        if (sunHeight > 0) // Daytime
        {
            SunColor = Color.Lerp(
                new Color(1.0f, 0.9f, 0.8f), // Sunset orange
                Color.white,                   // Noon white
                sunHeight
            );
            SunIntensity = Mathf.Lerp(0.5f, 1.0f, sunHeight);
        }
        else // Nighttime
        {
            SunColor = new Color(0.1f, 0.1f, 0.2f); // Moonlight blue
            SunIntensity = 0.1f;
        }
    }
}
```

---

## 8. Performance Optimization

### Draw Call Batching

```csharp
/// <summary>
/// Batch multiple chunks into single draw calls.
/// </summary>
public class ChunkBatcher
{
    private const int MAX_INSTANCES = 1000;
    
    public void RenderBatched(List<RenderableChunk> chunks, Material material)
    {
        // Group chunks by material
        var batches = chunks
            .GroupBy(c => c.MaterialId)
            .ToList();
        
        foreach (var batch in batches)
        {
            var batchChunks = batch.ToList();
            
            // Render in groups of MAX_INSTANCES
            for (int i = 0; i < batchChunks.Count; i += MAX_INSTANCES)
            {
                int count = Math.Min(MAX_INSTANCES, batchChunks.Count - i);
                var instances = batchChunks.Skip(i).Take(count).ToList();
                
                // Create instance data
                Matrix4x4[] matrices = instances
                    .Select(c => Matrix4x4.TRS(c.Position, c.Rotation, Vector3.one))
                    .ToArray();
                
                // Instanced rendering
                Graphics.DrawMeshInstanced(
                    instances[0].Mesh,
                    0,
                    material,
                    matrices,
                    count
                );
            }
        }
    }
}
```

### Level of Detail Budget

```csharp
/// <summary>
/// Manage rendering budget to maintain target framerate.
/// </summary>
public class RenderBudgetManager
{
    private const float TARGET_FRAME_TIME = 16.67f; // 60 FPS
    private const float RENDERING_BUDGET = 8.0f;    // 8ms for rendering
    
    private float lastFrameTime;
    private int maxVisibleChunks = 500;
    
    public void Update()
    {
        // Measure frame time
        lastFrameTime = Time.deltaTime * 1000f; // Convert to ms
        
        // Adjust budget if needed
        if (lastFrameTime > TARGET_FRAME_TIME)
        {
            // Frame took too long - reduce quality
            maxVisibleChunks = (int)(maxVisibleChunks * 0.9f);
            maxVisibleChunks = Math.Max(100, maxVisibleChunks); // Minimum 100 chunks
        }
        else if (lastFrameTime < TARGET_FRAME_TIME * 0.8f)
        {
            // Frame has headroom - increase quality
            maxVisibleChunks = (int)(maxVisibleChunks * 1.05f);
            maxVisibleChunks = Math.Min(2000, maxVisibleChunks); // Maximum 2000 chunks
        }
    }
    
    public List<RenderableChunk> PrioritizeChunks(
        List<RenderableChunk> allChunks,
        Vector3 cameraPosition)
    {
        // Sort by distance
        var sorted = allChunks
            .OrderBy(c => Vector3.Distance(c.Center, cameraPosition))
            .Take(maxVisibleChunks)
            .ToList();
        
        return sorted;
    }
}
```

---

## 9. Implementation Roadmap

### Phase 1: Core LOD System (Weeks 1-3)
- [ ] Implement LOD level definitions
- [ ] Create LOD selector with hysteresis
- [ ] Build quadtree/octree partitioning
- [ ] Test LOD transitions

### Phase 2: Culling Systems (Weeks 4-5)
- [ ] Implement frustum culling
- [ ] Add occlusion culling (optional)
- [ ] Optimize culling performance
- [ ] Profile and validate

### Phase 3: Rendering Pipeline (Weeks 6-8)
- [ ] Origin-relative rendering
- [ ] Multi-threaded mesh generation
- [ ] Draw call batching
- [ ] Shader optimization

### Phase 4: Atmosphere & Lighting (Weeks 9-10)
- [ ] Atmospheric scattering shader
- [ ] Day-night cycle system
- [ ] Dynamic lighting
- [ ] Weather effects (optional)

### Phase 5: Optimization (Weeks 11-12)
- [ ] Performance profiling
- [ ] Budget management system
- [ ] GPU optimization
- [ ] Stress testing

---

## Further Reading

### Internal Documentation
- [Step 5: Coordinate Systems & Engine Choice](step-5-coordinate-systems-engine-choice.md) - Origin shifting
- [Step 6: Voxel Data Storage & Streaming](step-6-voxel-data-storage-streaming.md) - Data management
- [Step 8: MMORPG GIS Key Takeaways](step-8-mmorpg-gis-key-takeaways.md) - Architecture overview

### External Resources
- [GPU Gems - LOD Techniques](http://developer.nvidia.com/GPUGems)
- [Volumetric Rendering](https://developer.nvidia.com/gpugems/gpugems3/part-i-geometry/chapter-1-generating-complex-procedural-terrains-using-gpu)
- [Atmospheric Scattering](https://developer.nvidia.com/gpugems/gpugems2/part-ii-shading-lighting-and-shadows/chapter-16-accurate-atmospheric-scattering)

---

**Document Version**: 1.0  
**Last Updated**: 2024-01-15  
**Status**: Complete
