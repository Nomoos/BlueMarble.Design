# Step 6: Voxel Data Storage & Streaming Guide

This guide provides comprehensive technical guidance on storing and streaming planet-scale voxel data, expanding on the principles from the [Planet-Scale MMORPG GIS Architecture and Simulation Plan](../../ArchitectureAndSimulationPlan.md).

## Overview

Storing a planet-sized voxel world (40M × 20M × 20M meters at 0.25m resolution) requires innovative data structures, cloud-optimized formats, and intelligent streaming strategies. This guide details proven approaches for managing petabyte-scale voxel datasets.

---

## 1. The Scale Challenge

### Raw Data Volume

```
World Dimensions: 40,075,020m × 20,037,510m × 20,000,000m
Voxel Resolution: 0.25m (quarter-meter)
Voxel Count: 160,300,080 × 80,150,040 × 80,000,000

Total Voxels: 1.03 × 10²⁴ voxels (1 septillion)

Storage Requirements:
- 1 byte per voxel (material ID): 1,030 yottabytes
- Compressed 100:1: 10 petabytes
- Compressed 1000:1: 1 petabyte
```

**Impossibility of Full Storage**: Even with aggressive compression, storing the entire world is impractical. Solution: **Sparse storage** and **procedural generation**.

### Sparse Voxel Storage

Most of the world is empty (air) or homogeneous (solid rock). Use sparse data structures:

```
Surface Layer: 0-100m depth
  Occupied: ~5% of volume
  Storage: 500 TB compressed

Mid Layer: 100m-10km depth
  Occupied: ~80% (mostly rock)
  Storage: 8 PB compressed

Deep Layer: >10km depth
  Occupied: ~95% (solid mantle/core)
  Storage: Can be procedurally generated
```

---

## 2. Hybrid Array-Octree Storage

### Chunk-Based Organization

```
Chunk Size: 128×128×128 voxels (2,097,152 voxels)
Chunk Dimensions: 32m × 32m × 32m physical space
Voxel Size: 0.25m × 0.25m × 0.25m

Chunk Storage:
- Material IDs: 1 byte per voxel = 2 MB uncompressed
- With metadata: ~2.1 MB uncompressed
- Zstd compressed: ~200 KB (10:1 ratio for varied terrain)
- Homogeneous chunks: <1 KB (run-length encoding)
```

### Octree for Sparse Regions

```csharp
/// <summary>
/// Sparse voxel octree for efficient storage of mostly-empty regions.
/// </summary>
public class SparseVoxelOctree
{
    private const int MAX_DEPTH = 10; // 128×128×128 at leaf level
    
    public class OctreeNode
    {
        // Node bounds
        public Vector3Int Min { get; set; }
        public Vector3Int Size { get; set; }
        
        // Child nodes (null if leaf or fully homogeneous)
        public OctreeNode[] Children { get; set; } // 8 children
        
        // Leaf data
        public byte[] VoxelData { get; set; } // Only for leaf nodes
        public byte HomogeneousMaterial { get; set; } // Single material ID
        public bool IsHomogeneous { get; set; }
        
        public bool IsLeaf => Children == null;
    }
    
    private OctreeNode root;
    
    /// <summary>
    /// Get voxel material at world position.
    /// </summary>
    public byte GetVoxel(Vector3Int worldPos)
    {
        return GetVoxelRecursive(root, worldPos);
    }
    
    private byte GetVoxelRecursive(OctreeNode node, Vector3Int pos)
    {
        // Check if position is within node bounds
        if (!IsInBounds(pos, node.Min, node.Size))
            return 0; // Air
        
        // Homogeneous node - return single material
        if (node.IsHomogeneous)
            return node.HomogeneousMaterial;
        
        // Leaf node with data
        if (node.IsLeaf && node.VoxelData != null)
        {
            Vector3Int localPos = pos - node.Min;
            int index = localPos.X + localPos.Y * node.Size.X + 
                       localPos.Z * node.Size.X * node.Size.Y;
            return node.VoxelData[index];
        }
        
        // Recurse to appropriate child
        if (node.Children != null)
        {
            int childIndex = GetChildIndex(pos, node);
            return GetVoxelRecursive(node.Children[childIndex], pos);
        }
        
        return 0; // Default: air
    }
    
    /// <summary>
    /// Optimize octree by merging homogeneous regions.
    /// </summary>
    public void Optimize()
    {
        OptimizeNode(root);
    }
    
    private bool OptimizeNode(OctreeNode node)
    {
        if (node.IsLeaf)
            return false;
        
        // Check if all children are homogeneous with same material
        if (node.Children == null)
            return false;
        
        byte material = node.Children[0].HomogeneousMaterial;
        bool allHomogeneous = true;
        
        foreach (var child in node.Children)
        {
            if (!child.IsHomogeneous || child.HomogeneousMaterial != material)
            {
                allHomogeneous = false;
                break;
            }
        }
        
        if (allHomogeneous)
        {
            // Merge children into single homogeneous node
            node.IsHomogeneous = true;
            node.HomogeneousMaterial = material;
            node.Children = null;
            node.VoxelData = null;
            return true;
        }
        
        return false;
    }
}
```

### Flat Array for Dense Regions

```csharp
/// <summary>
/// Dense voxel chunk using flat array for maximum performance.
/// </summary>
public class DenseVoxelChunk
{
    public const int CHUNK_SIZE = 128;
    public const int VOXEL_COUNT = CHUNK_SIZE * CHUNK_SIZE * CHUNK_SIZE;
    
    // Flat array: X + Y*SIZE + Z*SIZE*SIZE
    private byte[] voxels;
    
    // Chunk metadata
    public Vector3Int ChunkPosition { get; set; }
    public bool IsDirty { get; set; }
    public DateTime LastModified { get; set; }
    public int AccessCount { get; set; }
    
    public DenseVoxelChunk(Vector3Int position)
    {
        ChunkPosition = position;
        voxels = new byte[VOXEL_COUNT];
        IsDirty = false;
        LastModified = DateTime.UtcNow;
    }
    
    /// <summary>
    /// Get voxel at local position (0-127).
    /// </summary>
    public byte GetVoxel(int x, int y, int z)
    {
        int index = x + y * CHUNK_SIZE + z * CHUNK_SIZE * CHUNK_SIZE;
        return voxels[index];
    }
    
    /// <summary>
    /// Set voxel at local position.
    /// </summary>
    public void SetVoxel(int x, int y, int z, byte material)
    {
        int index = x + y * CHUNK_SIZE + z * CHUNK_SIZE * CHUNK_SIZE;
        voxels[index] = material;
        IsDirty = true;
        LastModified = DateTime.UtcNow;
    }
    
    /// <summary>
    /// Check if chunk is homogeneous (all same material).
    /// </summary>
    public bool IsHomogeneous(out byte material)
    {
        material = voxels[0];
        for (int i = 1; i < VOXEL_COUNT; i++)
        {
            if (voxels[i] != material)
                return false;
        }
        return true;
    }
    
    /// <summary>
    /// Compress chunk data using run-length encoding.
    /// </summary>
    public byte[] Compress()
    {
        // Check if homogeneous
        if (IsHomogeneous(out byte material))
        {
            // 1 byte for material + 1 byte marker
            return new byte[] { 0xFF, material };
        }
        
        // Use Zstd compression for varied data
        return ZstdCompress(voxels);
    }
    
    /// <summary>
    /// Decompress chunk data.
    /// </summary>
    public static DenseVoxelChunk Decompress(byte[] compressed, Vector3Int position)
    {
        var chunk = new DenseVoxelChunk(position);
        
        // Check for homogeneous marker
        if (compressed.Length == 2 && compressed[0] == 0xFF)
        {
            byte material = compressed[1];
            Array.Fill(chunk.voxels, material);
            return chunk;
        }
        
        // Decompress using Zstd
        chunk.voxels = ZstdDecompress(compressed);
        return chunk;
    }
}
```

---

## 3. Cloud-Optimized Formats

### Zarr for Voxel Arrays

**Zarr** provides chunked, compressed N-dimensional arrays optimized for cloud storage:

```python
import zarr
import numpy as np
from numcodecs import Blosc

# Create Zarr store for world voxels
store = zarr.DirectoryStore('/data/bluemarble_voxels')
root = zarr.group(store=store)

# Configure compressor
compressor = Blosc(cname='zstd', clevel=5, shuffle=Blosc.BITSHUFFLE)

# Create voxel dataset
voxels = root.create_dataset(
    'world_voxels',
    shape=(160300080, 80150040, 80000000),  # Full world size
    chunks=(128, 128, 128),                  # Chunk size
    dtype='uint8',                           # Material ID
    compressor=compressor,
    fill_value=0,                            # Air by default
    overwrite=True
)

# Metadata
voxels.attrs['world_size_meters'] = [40075020, 20037510, 20000000]
voxels.attrs['voxel_size_meters'] = 0.25
voxels.attrs['coordinate_system'] = 'EPSG:4087'

# Write chunk (lazy, only writes non-zero data)
chunk_pos = (1000, 2000, 0)
chunk_data = generate_terrain_chunk(chunk_pos)
voxels[1000:1128, 2000:2128, 0:128] = chunk_data

# Read specific chunk
chunk = voxels[1000:1128, 2000:2128, 0:128]

# Read single voxel
material = voxels[12345, 67890, 100]
```

**Storage Layout**:
```
/data/bluemarble_voxels/
├── world_voxels/
│   ├── .zarray          # Array metadata
│   ├── .zattrs          # Custom attributes
│   ├── 0.0.0            # Chunk [0:128, 0:128, 0:128]
│   ├── 0.0.1            # Chunk [0:128, 0:128, 128:256]
│   ├── 1.0.0            # Chunk [128:256, 0:128, 0:128]
│   └── ...              # Only non-empty chunks stored
└── lod_pyramid/
    ├── lod_1/           # 1m resolution
    ├── lod_2/           # 4m resolution
    └── lod_3/           # 16m resolution
```

### LOD Pyramid Generation

```python
def generate_lod_pyramid(source_zarr, num_levels=5):
    """Generate multi-resolution LOD pyramid."""
    for level in range(1, num_levels + 1):
        downsample_factor = 2 ** level
        
        # Create downsampled array
        lod_shape = tuple(s // downsample_factor for s in source_zarr.shape)
        lod_chunks = (128, 128, 128)
        
        lod_array = root.create_dataset(
            f'lod_pyramid/lod_{level}',
            shape=lod_shape,
            chunks=lod_chunks,
            dtype='uint8',
            compressor=compressor
        )
        
        # Downsample using max pooling
        for chunk_z in range(0, lod_shape[2], 128):
            for chunk_y in range(0, lod_shape[1], 128):
                for chunk_x in range(0, lod_shape[0], 128):
                    # Read source data (larger region)
                    src_x = chunk_x * downsample_factor
                    src_y = chunk_y * downsample_factor
                    src_z = chunk_z * downsample_factor
                    src_size = 128 * downsample_factor
                    
                    source_data = source_zarr[
                        src_x:src_x+src_size,
                        src_y:src_y+src_size,
                        src_z:src_z+src_size
                    ]
                    
                    # Downsample (take most common material)
                    downsampled = downsample_chunk(source_data, downsample_factor)
                    
                    # Write to LOD array
                    lod_array[
                        chunk_x:chunk_x+128,
                        chunk_y:chunk_y+128,
                        chunk_z:chunk_z+128
                    ] = downsampled
```

---

## 4. Spatial Indexing

### S2 Geometry for Global Indexing

```csharp
using S2Geometry;

/// <summary>
/// Global spatial index using S2 geometry cells.
/// </summary>
public class GlobalSpatialIndex
{
    // S2 cell level determines resolution
    // Level 10: ~1.2km × 1.2km cells
    // Level 15: ~38m × 38m cells
    private const int CELL_LEVEL = 12;
    
    private Dictionary<S2CellId, List<ChunkId>> cellToChunks;
    
    /// <summary>
    /// Add chunk to spatial index.
    /// </summary>
    public void AddChunk(ChunkId chunkId, WorldPosition position)
    {
        // Convert world position to S2 cell
        var (lat, lon, _) = CoordinateTransforms.WorldToGeodetic(position);
        S2LatLng latLng = S2LatLng.FromDegrees(lat, lon);
        S2CellId cellId = S2CellId.FromLatLng(latLng).Parent(CELL_LEVEL);
        
        // Add to index
        if (!cellToChunks.ContainsKey(cellId))
            cellToChunks[cellId] = new List<ChunkId>();
        
        cellToChunks[cellId].Add(chunkId);
    }
    
    /// <summary>
    /// Find chunks within radius of position.
    /// </summary>
    public List<ChunkId> QueryRadius(WorldPosition center, double radiusMeters)
    {
        var result = new List<ChunkId>();
        
        // Get S2 cell for center
        var (lat, lon, _) = CoordinateTransforms.WorldToGeodetic(center);
        S2LatLng centerLatLng = S2LatLng.FromDegrees(lat, lon);
        
        // Get covering cells
        S2RegionCoverer coverer = new S2RegionCoverer();
        coverer.SetMinLevel(CELL_LEVEL);
        coverer.SetMaxLevel(CELL_LEVEL);
        
        S2Cap cap = S2Cap.FromAxisAngle(
            centerLatLng.ToPoint(),
            S1Angle.FromRadians(radiusMeters / 6371000.0) // Earth radius
        );
        
        var covering = coverer.GetCovering(cap);
        
        // Collect chunks from covering cells
        foreach (S2CellId cellId in covering)
        {
            if (cellToChunks.ContainsKey(cellId))
                result.AddRange(cellToChunks[cellId]);
        }
        
        return result;
    }
}
```

### Morton Codes (Z-Order Curve)

```csharp
/// <summary>
/// Morton codes for cache-friendly chunk access patterns.
/// </summary>
public static class MortonCode
{
    /// <summary>
    /// Encode 3D position to 1D Morton code.
    /// </summary>
    public static ulong Encode(int x, int y, int z)
    {
        return Part1By2((ulong)x) | 
               (Part1By2((ulong)y) << 1) | 
               (Part1By2((ulong)z) << 2);
    }
    
    /// <summary>
    /// Decode Morton code to 3D position.
    /// </summary>
    public static (int x, int y, int z) Decode(ulong code)
    {
        return (
            (int)Compact1By2(code),
            (int)Compact1By2(code >> 1),
            (int)Compact1By2(code >> 2)
        );
    }
    
    // Helper: Spread bits (insert 2 zeros between each bit)
    private static ulong Part1By2(ulong n)
    {
        n &= 0x1fffff;
        n = (n | (n << 32)) & 0x1f00000000ffff;
        n = (n | (n << 16)) & 0x1f0000ff0000ff;
        n = (n | (n << 8)) & 0x100f00f00f00f00f;
        n = (n | (n << 4)) & 0x10c30c30c30c30c3;
        n = (n | (n << 2)) & 0x1249249249249249;
        return n;
    }
    
    // Helper: Compact bits (remove 2 zeros between each bit)
    private static ulong Compact1By2(ulong n)
    {
        n &= 0x1249249249249249;
        n = (n ^ (n >> 2)) & 0x10c30c30c30c30c3;
        n = (n ^ (n >> 4)) & 0x100f00f00f00f00f;
        n = (n ^ (n >> 8)) & 0x1f0000ff0000ff;
        n = (n ^ (n >> 16)) & 0x1f00000000ffff;
        n = (n ^ (n >> 32)) & 0x1fffff;
        return n;
    }
}

/// <summary>
/// Chunk manager using Morton codes for spatial locality.
/// </summary>
public class MortonChunkManager
{
    // Chunks stored in Morton order for cache efficiency
    private SortedDictionary<ulong, DenseVoxelChunk> chunks;
    
    public void AddChunk(Vector3Int chunkPos, DenseVoxelChunk chunk)
    {
        ulong morton = MortonCode.Encode(chunkPos.X, chunkPos.Y, chunkPos.Z);
        chunks[morton] = chunk;
    }
    
    public DenseVoxelChunk GetChunk(Vector3Int chunkPos)
    {
        ulong morton = MortonCode.Encode(chunkPos.X, chunkPos.Y, chunkPos.Z);
        return chunks.TryGetValue(morton, out var chunk) ? chunk : null;
    }
    
    /// <summary>
    /// Get chunks in Morton order (spatially coherent).
    /// </summary>
    public IEnumerable<DenseVoxelChunk> GetChunksInOrder()
    {
        return chunks.Values;
    }
}
```

---

## 5. Streaming Strategy

### Priority-Based Chunk Loading

```csharp
/// <summary>
/// Intelligent chunk streaming based on player position and direction.
/// </summary>
public class ChunkStreamingManager
{
    private PriorityQueue<ChunkLoadRequest, float> loadQueue;
    private HashSet<ChunkId> loadedChunks;
    private LRUCache<ChunkId, DenseVoxelChunk> chunkCache;
    
    public class ChunkLoadRequest
    {
        public ChunkId ChunkId { get; set; }
        public LODLevel LOD { get; set; }
        public float Priority { get; set; }
        public DateTime RequestTime { get; set; }
    }
    
    /// <summary>
    /// Calculate chunk priority based on player state.
    /// </summary>
    private float CalculatePriority(
        ChunkId chunk,
        WorldPosition playerPos,
        Vector3 playerVelocity)
    {
        // Distance from player
        WorldPosition chunkWorldPos = chunk.ToWorldPosition();
        double distance = playerPos.DistanceTo(chunkWorldPos);
        
        // Predicted player position in 5 seconds
        WorldPosition predictedPos = new WorldPosition(
            playerPos.X + (long)(playerVelocity.X * 5.0),
            playerPos.Y + (long)(playerVelocity.Y * 5.0),
            playerPos.Z + (long)(playerVelocity.Z * 5.0)
        );
        
        double predictedDistance = predictedPos.DistanceTo(chunkWorldPos);
        
        // Priority factors:
        // 1. Current distance (closer = higher priority)
        // 2. Predicted distance (moving towards = higher priority)
        // 3. View direction (in front = higher priority)
        float distanceFactor = 1.0f / (float)(distance + 1.0);
        float predictionFactor = predictedDistance < distance ? 1.5f : 1.0f;
        
        return distanceFactor * predictionFactor;
    }
    
    /// <summary>
    /// Update streaming based on player state.
    /// </summary>
    public void Update(WorldPosition playerPos, Vector3 velocity, float deltaTime)
    {
        // Find chunks within view distance
        const double VIEW_DISTANCE = 1000.0; // 1 km
        var visibleChunks = spatialIndex.QueryRadius(playerPos, VIEW_DISTANCE);
        
        // Calculate priorities and queue loads
        foreach (var chunkId in visibleChunks)
        {
            if (!loadedChunks.Contains(chunkId) && !loadQueue.Contains(chunkId))
            {
                float priority = CalculatePriority(chunkId, playerPos, velocity);
                LODLevel lod = CalculateLOD(chunkId, playerPos);
                
                loadQueue.Enqueue(new ChunkLoadRequest
                {
                    ChunkId = chunkId,
                    LOD = lod,
                    Priority = priority,
                    RequestTime = DateTime.UtcNow
                }, priority);
            }
        }
        
        // Process load queue (budget: 2 chunks per frame)
        for (int i = 0; i < 2 && loadQueue.Count > 0; i++)
        {
            var request = loadQueue.Dequeue();
            LoadChunkAsync(request);
        }
        
        // Unload distant chunks
        UnloadDistantChunks(playerPos, VIEW_DISTANCE * 1.5);
    }
    
    private async Task LoadChunkAsync(ChunkLoadRequest request)
    {
        try
        {
            // Load from cloud storage
            var chunk = await cloudStorage.LoadChunkAsync(request.ChunkId, request.LOD);
            
            // Add to cache
            chunkCache.Put(request.ChunkId, chunk);
            loadedChunks.Add(request.ChunkId);
            
            // Generate mesh on worker thread
            await Task.Run(() => meshGenerator.GenerateMesh(chunk));
        }
        catch (Exception ex)
        {
            Logger.Error($"Failed to load chunk {request.ChunkId}: {ex}");
        }
    }
    
    private void UnloadDistantChunks(WorldPosition playerPos, double threshold)
    {
        var toUnload = new List<ChunkId>();
        
        foreach (var chunkId in loadedChunks)
        {
            WorldPosition chunkPos = chunkId.ToWorldPosition();
            double distance = playerPos.DistanceTo(chunkPos);
            
            if (distance > threshold)
                toUnload.Add(chunkId);
        }
        
        foreach (var chunkId in toUnload)
        {
            chunkCache.Remove(chunkId);
            loadedChunks.Remove(chunkId);
        }
    }
}
```

### LRU Cache Implementation

```csharp
/// <summary>
/// Least Recently Used cache for chunk management.
/// </summary>
public class LRUCache<TKey, TValue>
{
    private class CacheNode
    {
        public TKey Key { get; set; }
        public TValue Value { get; set; }
        public DateTime LastAccess { get; set; }
    }
    
    private readonly int capacity;
    private readonly Dictionary<TKey, LinkedListNode<CacheNode>> cache;
    private readonly LinkedList<CacheNode> lruList;
    
    public LRUCache(int capacity)
    {
        this.capacity = capacity;
        cache = new Dictionary<TKey, LinkedListNode<CacheNode>>(capacity);
        lruList = new LinkedList<CacheNode>();
    }
    
    public void Put(TKey key, TValue value)
    {
        if (cache.TryGetValue(key, out var node))
        {
            // Update existing
            node.Value.Value = value;
            node.Value.LastAccess = DateTime.UtcNow;
            lruList.Remove(node);
            lruList.AddFirst(node);
        }
        else
        {
            // Add new
            if (cache.Count >= capacity)
            {
                // Remove least recently used
                var lru = lruList.Last;
                lruList.RemoveLast();
                cache.Remove(lru.Value.Key);
            }
            
            var newNode = new CacheNode
            {
                Key = key,
                Value = value,
                LastAccess = DateTime.UtcNow
            };
            
            var listNode = lruList.AddFirst(newNode);
            cache[key] = listNode;
        }
    }
    
    public bool TryGet(TKey key, out TValue value)
    {
        if (cache.TryGetValue(key, out var node))
        {
            // Move to front (most recently used)
            node.Value.LastAccess = DateTime.UtcNow;
            lruList.Remove(node);
            lruList.AddFirst(node);
            
            value = node.Value.Value;
            return true;
        }
        
        value = default;
        return false;
    }
    
    public void Remove(TKey key)
    {
        if (cache.TryGetValue(key, out var node))
        {
            lruList.Remove(node);
            cache.Remove(key);
        }
    }
}
```

---

## 6. Performance Optimization

### Parallel Chunk Processing

```csharp
/// <summary>
/// Process multiple chunks in parallel using thread pool.
/// </summary>
public class ParallelChunkProcessor
{
    private readonly int maxParallelism;
    
    public ParallelChunkProcessor(int parallelism = 8)
    {
        maxParallelism = parallelism;
    }
    
    /// <summary>
    /// Process chunks in parallel (compression, generation, etc.).
    /// </summary>
    public async Task<List<TResult>> ProcessChunksAsync<TResult>(
        List<DenseVoxelChunk> chunks,
        Func<DenseVoxelChunk, TResult> processor)
    {
        var results = new ConcurrentBag<TResult>();
        var options = new ParallelOptions
        {
            MaxDegreeOfParallelism = maxParallelism
        };
        
        await Task.Run(() =>
        {
            Parallel.ForEach(chunks, options, chunk =>
            {
                var result = processor(chunk);
                results.Add(result);
            });
        });
        
        return results.ToList();
    }
}
```

### Memory-Mapped Files for Large Datasets

```csharp
using System.IO.MemoryMappedFiles;

/// <summary>
/// Memory-mapped chunk storage for efficient disk access.
/// </summary>
public class MemoryMappedChunkStore
{
    private const long CHUNK_SIZE = 2 * 1024 * 1024; // 2 MB per chunk
    private readonly string filePath;
    private MemoryMappedFile mmf;
    
    public MemoryMappedChunkStore(string path, long maxChunks)
    {
        filePath = path;
        long fileSize = CHUNK_SIZE * maxChunks;
        
        mmf = MemoryMappedFile.CreateFromFile(
            filePath,
            FileMode.OpenOrCreate,
            null,
            fileSize,
            MemoryMappedFileAccess.ReadWrite
        );
    }
    
    public void WriteChunk(int chunkIndex, byte[] data)
    {
        long offset = chunkIndex * CHUNK_SIZE;
        
        using (var accessor = mmf.CreateViewAccessor(offset, CHUNK_SIZE))
        {
            accessor.WriteArray(0, data, 0, data.Length);
        }
    }
    
    public byte[] ReadChunk(int chunkIndex)
    {
        long offset = chunkIndex * CHUNK_SIZE;
        byte[] data = new byte[CHUNK_SIZE];
        
        using (var accessor = mmf.CreateViewAccessor(offset, CHUNK_SIZE))
        {
            accessor.ReadArray(0, data, 0, (int)CHUNK_SIZE);
        }
        
        return data;
    }
    
    public void Dispose()
    {
        mmf?.Dispose();
    }
}
```

---

## 7. Implementation Roadmap

### Phase 1: Core Storage (Weeks 1-3)
- [ ] Implement DenseVoxelChunk class
- [ ] Implement SparseVoxelOctree class
- [ ] Add compression (Zstd)
- [ ] Create unit tests

### Phase 2: Spatial Indexing (Weeks 4-5)
- [ ] Integrate S2 geometry library
- [ ] Implement GlobalSpatialIndex
- [ ] Add Morton code encoding
- [ ] Test spatial queries

### Phase 3: Cloud Integration (Weeks 6-8)
- [ ] Set up Zarr storage
- [ ] Implement cloud storage client
- [ ] Generate LOD pyramids
- [ ] Test with sample data

### Phase 4: Streaming (Weeks 9-11)
- [ ] Implement ChunkStreamingManager
- [ ] Add LRU cache
- [ ] Create priority calculation
- [ ] Test with moving player

### Phase 5: Optimization (Weeks 12-14)
- [ ] Add parallel processing
- [ ] Implement memory-mapped files
- [ ] Profile and optimize
- [ ] Stress test with large datasets

---

## Further Reading

### Internal Documentation
- [Step 5: Coordinate Systems & Engine Choice](step-5-coordinate-systems-engine-choice.md) - Foundation
- [Step 8: MMORPG GIS Key Takeaways](step-8-mmorpg-gis-key-takeaways.md) - Architecture overview
- [3D Octree Storage Architecture](../spatial-data-storage/step-3-architecture-design/3d-octree-storage-architecture-integration.md)

### External Resources
- [Zarr Documentation](https://zarr.readthedocs.io/)
- [S2 Geometry Library](https://s2geometry.io/)
- [Morton Codes Explained](https://en.wikipedia.org/wiki/Z-order_curve)
- [Sparse Voxel Octrees](http://research.nvidia.com/publication/efficient-sparse-voxel-octrees)

---

**Document Version**: 1.0  
**Last Updated**: 2024-01-15  
**Status**: Complete
