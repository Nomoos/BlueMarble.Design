# Octree Spatial Indexing - Implementation Specification

---
title: Octree Spatial Indexing System
date: 2025-01-17
status: implementation-ready
priority: high
dependencies: job-system, memory-management
---

## Overview

Detailed implementation specification for BlueMarble's octree spatial indexing system. The octree provides O(log n) spatial queries for entity location, collision detection, and interest management.

**Primary Use Cases:**
- Entity spatial queries (find nearby entities)
- Collision detection broad phase
- Network interest management (AOI)
- Rendering culling

**Performance Target:** Sub-millisecond queries for 10,000+ entities

---

## Data Structure

### Node Structure

```csharp
public class OctreeNode : IResettable
{
    // Spatial bounds of this node
    public Bounds Bounds { get; set; }
    
    // Entities in this node (leaf nodes only)
    public List<int> EntityIds { get; private set; }
    
    // Child nodes (null for leaf nodes)
    public OctreeNode[] Children { get; private set; }
    
    // Parent reference for traversal
    public OctreeNode Parent { get; set; }
    
    // Node depth in tree
    public int Depth { get; set; }
    
    // Is this a leaf node?
    public bool IsLeaf => Children == null;
    
    // Node state
    public bool IsDirty { get; set; }
    
    // Pooling support
    public void Reset()
    {
        EntityIds?.Clear();
        Children = null;
        Parent = null;
        IsDirty = false;
    }
}
```

### Octree Root

```csharp
public class OctreeSystem
{
    // Configuration
    private readonly Bounds worldBounds;
    private readonly int maxDepth;
    private readonly int maxEntitiesPerNode;
    
    // Root node
    private OctreeNode root;
    
    // Entity tracking
    private Dictionary<int, OctreeNode> entityLocations;
    private Dictionary<int, Bounds> entityBounds;
    
    // Dirty nodes for parallel updates
    private ConcurrentBag<OctreeNode> dirtyNodes;
    
    // Memory pools
    private ObjectPool<OctreeNode> nodePool;
    private ObjectPool<List<int>> listPool;
    
    // Statistics
    private OctreeStatistics stats;
}
```

---

## Core Operations

### 1. Insert

```csharp
public void Insert(int entityId, Bounds bounds)
{
    // Validate bounds
    if (!worldBounds.Contains(bounds.Center))
    {
        Logger.Warning($"Entity {entityId} outside world bounds");
        return;
    }
    
    // Find appropriate node
    var node = FindNodeForBounds(root, bounds, 0);
    
    // Add to node
    node.EntityIds.Add(entityId);
    entityLocations[entityId] = node;
    entityBounds[entityId] = bounds;
    
    // Split if necessary
    if (node.EntityIds.Count > maxEntitiesPerNode && node.Depth < maxDepth)
    {
        SplitNode(node);
    }
    
    stats.TotalEntities++;
}

private OctreeNode FindNodeForBounds(OctreeNode node, Bounds bounds, int depth)
{
    // Reached leaf or max depth
    if (node.IsLeaf || depth >= maxDepth)
    {
        return node;
    }
    
    // Find which child octant contains bounds
    int octant = GetOctant(node.Bounds, bounds.Center);
    
    // Ensure child exists
    if (node.Children[octant] == null)
    {
        node.Children[octant] = CreateChildNode(node, octant);
    }
    
    // Recurse if bounds fits entirely in child
    if (node.Children[octant].Bounds.Contains(bounds))
    {
        return FindNodeForBounds(node.Children[octant], bounds, depth + 1);
    }
    
    // Bounds spans multiple octants, stays at this level
    return node;
}
```

### 2. Remove

```csharp
public void Remove(int entityId)
{
    if (!entityLocations.TryGetValue(entityId, out var node))
    {
        return; // Entity not in octree
    }
    
    // Remove from node
    node.EntityIds.Remove(entityId);
    entityLocations.Remove(entityId);
    entityBounds.Remove(entityId);
    
    // Mark parent chain as dirty for potential merge
    MarkDirty(node);
    
    stats.TotalEntities--;
}
```

### 3. Update

```csharp
public void Update(int entityId, Bounds newBounds)
{
    if (!entityLocations.TryGetValue(entityId, out var currentNode))
    {
        // Not in octree, insert it
        Insert(entityId, newBounds);
        return;
    }
    
    var oldBounds = entityBounds[entityId];
    
    // If still in same node, just update bounds
    if (currentNode.Bounds.Contains(newBounds))
    {
        entityBounds[entityId] = newBounds;
        return;
    }
    
    // Moved to different node, remove and re-insert
    Remove(entityId);
    Insert(entityId, newBounds);
}
```

### 4. Query Radius

```csharp
public List<int> QueryRadius(Vector3 center, float radius)
{
    var results = new List<int>();
    var radiusSquared = radius * radius;
    
    QueryRadiusRecursive(root, center, radiusSquared, results);
    
    stats.TotalQueries++;
    return results;
}

private void QueryRadiusRecursive(
    OctreeNode node,
    Vector3 center,
    float radiusSquared,
    List<int> results)
{
    // Check if sphere intersects node bounds
    if (!SphereBoundsIntersect(center, radiusSquared, node.Bounds))
    {
        return;
    }
    
    // Check entities in this node
    foreach (var entityId in node.EntityIds)
    {
        var entityBounds = this.entityBounds[entityId];
        var distanceSquared = Vector3.DistanceSquared(center, entityBounds.Center);
        
        if (distanceSquared <= radiusSquared)
        {
            results.Add(entityId);
        }
    }
    
    // Recurse to children
    if (!node.IsLeaf)
    {
        foreach (var child in node.Children)
        {
            if (child != null)
            {
                QueryRadiusRecursive(child, center, radiusSquared, results);
            }
        }
    }
}
```

### 5. Query AABB

```csharp
public List<int> QueryAABB(Bounds queryBounds)
{
    var results = new List<int>();
    
    QueryAABBRecursive(root, queryBounds, results);
    
    stats.TotalQueries++;
    return results;
}

private void QueryAABBRecursive(
    OctreeNode node,
    Bounds queryBounds,
    List<int> results)
{
    // Check if query bounds intersects node bounds
    if (!node.Bounds.Intersects(queryBounds))
    {
        return;
    }
    
    // Check entities in this node
    foreach (var entityId in node.EntityIds)
    {
        var entityBounds = this.entityBounds[entityId];
        
        if (queryBounds.Intersects(entityBounds))
        {
            results.Add(entityId);
        }
    }
    
    // Recurse to children
    if (!node.IsLeaf)
    {
        foreach (var child in node.Children)
        {
            if (child != null)
            {
                QueryAABBRecursive(child, queryBounds, results);
            }
        }
    }
}
```

---

## Parallel Operations

### Parallel Update

```csharp
public JobHandle UpdateParallel(JobHandle dependency = default)
{
    // Phase 1: Collect entities that moved
    var movedEntities = CollectMovedEntities();
    
    // Phase 2: Process moves in parallel
    var updateHandle = JobSystem.ScheduleBatch(
        movedEntities,
        entity => ProcessEntityMove(entity),
        batchSize: 64,
        dependency
    );
    
    return updateHandle;
}

private void ProcessEntityMove(MovedEntity entity)
{
    Update(entity.Id, entity.NewBounds);
}
```

### Parallel Query

```csharp
public JobHandle QueryRadiusParallel(
    Vector3 center,
    float radius,
    List<int> results,
    JobHandle dependency = default)
{
    return JobSystem.Schedule(() => {
        var queryResults = QueryRadius(center, radius);
        lock (results)
        {
            results.AddRange(queryResults);
        }
    }, dependency);
}
```

---

## Memory Management

### Node Pooling

```csharp
private OctreeNode AllocateNode(Bounds bounds, int depth)
{
    var node = nodePool.Get();
    node.Bounds = bounds;
    node.Depth = depth;
    node.EntityIds = listPool.Get();
    
    stats.NodesAllocated++;
    return node;
}

private void FreeNode(OctreeNode node)
{
    listPool.Return(node.EntityIds);
    node.Reset();
    nodePool.Return(node);
    
    stats.NodesFreed++;
}
```

### Memory Budget

```csharp
public class OctreeMemoryConfig
{
    // Maximum nodes before warning
    public int MaxNodes { get; set; } = 100000;
    
    // Node pool initial size
    public int NodePoolSize { get; set; } = 1024;
    
    // List pool initial size
    public int ListPoolSize { get; set; } = 2048;
    
    // Warn when memory usage exceeds threshold
    public long MemoryWarningThreshold { get; set; } = 100 * 1024 * 1024; // 100MB
}
```

---

## Optimization Techniques

### 1. Loose Octree

```csharp
// Expand node bounds by factor to reduce updates
private const float LoosenessFactor = 2.0f;

private Bounds GetLooseBounds(Bounds tightBounds)
{
    var center = tightBounds.Center;
    var size = tightBounds.Size * LoosenessFactor;
    return new Bounds(center, size);
}
```

### 2. Lazy Splitting

```csharp
// Only split when entity count exceeds threshold significantly
private const int SplitThreshold = 16;

private bool ShouldSplit(OctreeNode node)
{
    return node.EntityIds.Count > SplitThreshold
        && node.Depth < maxDepth
        && !node.IsSplitting;
}
```

### 3. Query Caching

```csharp
// Cache recent queries for temporal coherence
private class QueryCache
{
    private Dictionary<QueryKey, CachedQuery> cache;
    private Queue<QueryKey> lruQueue;
    private const int MaxCacheSize = 256;
    
    public bool TryGetCached(QueryKey key, out List<int> results)
    {
        if (cache.TryGetValue(key, out var cached))
        {
            // Check if cache is still valid (frame count)
            if (Time.FrameCount - cached.Frame < 5)
            {
                results = cached.Results;
                return true;
            }
        }
        
        results = null;
        return false;
    }
}
```

---

## Statistics and Monitoring

```csharp
public struct OctreeStatistics
{
    // Entity tracking
    public int TotalEntities;
    public int TotalNodes;
    public int LeafNodes;
    
    // Memory usage
    public long MemoryUsageBytes;
    public int NodesAllocated;
    public int NodesFreed;
    
    // Performance
    public int TotalQueries;
    public double AverageQueryTimeMs;
    public double AverageUpdateTimeMs;
    
    // Efficiency
    public float AverageEntitiesPerLeaf;
    public int MaxDepthReached;
    public float TreeBalanceFactor;
}

public OctreeStatistics GetStatistics()
{
    return new OctreeStatistics
    {
        TotalEntities = entityLocations.Count,
        TotalNodes = CountNodes(root),
        LeafNodes = CountLeafNodes(root),
        MemoryUsageBytes = EstimateMemoryUsage(),
        // ... calculate other stats
    };
}
```

---

## Testing Strategy

### Unit Tests

```csharp
[Test]
public void Insert_AddsEntityToCorrectNode()
{
    var octree = new OctreeSystem(worldBounds, maxDepth: 5);
    var entityBounds = new Bounds(Vector3.Zero, Vector3.One);
    
    octree.Insert(entityId: 1, entityBounds);
    
    var results = octree.QueryAABB(entityBounds);
    Assert.Contains(1, results);
}

[Test]
public void QueryRadius_ReturnsOnlyNearbyEntities()
{
    var octree = new OctreeSystem(worldBounds, maxDepth: 5);
    
    // Add entities at various distances
    octree.Insert(1, new Bounds(new Vector3(0, 0, 0), Vector3.One));
    octree.Insert(2, new Bounds(new Vector3(10, 0, 0), Vector3.One));
    octree.Insert(3, new Bounds(new Vector3(100, 0, 0), Vector3.One));
    
    var results = octree.QueryRadius(Vector3.Zero, radius: 15);
    
    Assert.Contains(1, results);
    Assert.Contains(2, results);
    Assert.DoesNotContain(3, results);
}
```

### Performance Tests

```csharp
[Test]
public void QueryPerformance_10000Entities()
{
    var octree = CreateOctreeWith10000Entities();
    
    var sw = Stopwatch.StartNew();
    
    for (int i = 0; i < 1000; i++)
    {
        var results = octree.QueryRadius(RandomPoint(), radius: 50);
    }
    
    sw.Stop();
    var avgTimeMs = sw.ElapsedMilliseconds / 1000.0;
    
    Assert.Less(avgTimeMs, 1.0); // <1ms average
}
```

---

## Integration Examples

### Example: Entity System Integration

```csharp
public class EntitySystem
{
    private OctreeSystem octree;
    private Dictionary<int, Entity> entities;
    
    public void Update(float deltaTime)
    {
        // Update entity positions
        foreach (var entity in entities.Values)
        {
            entity.Update(deltaTime);
            
            // Update octree
            var newBounds = entity.GetBounds();
            octree.Update(entity.Id, newBounds);
        }
    }
    
    public List<Entity> GetNearbyEntities(Entity entity, float radius)
    {
        var nearbyIds = octree.QueryRadius(entity.Position, radius);
        return nearbyIds.Select(id => entities[id]).ToList();
    }
}
```

### Example: Network Interest Management

```csharp
public class InterestManager
{
    private OctreeSystem octree;
    
    public void UpdatePlayerInterest(Player player)
    {
        var oldInterest = player.InterestedEntities;
        var newInterest = octree.QueryRadius(player.Position, radius: 100);
        
        var added = newInterest.Except(oldInterest);
        var removed = oldInterest.Except(newInterest);
        
        foreach (var entityId in added)
        {
            SendEntitySpawn(player, entityId);
        }
        
        foreach (var entityId in removed)
        {
            SendEntityDespawn(player, entityId);
        }
        
        player.InterestedEntities = newInterest.ToHashSet();
    }
}
```

---

## Performance Tuning

### Configuration for Different Scales

```csharp
// Small world (< 1,000 entities)
var config = new OctreeConfig
{
    MaxDepth = 6,
    MaxEntitiesPerNode = 16,
    LoosenessFactor = 1.5f
};

// Medium world (1,000 - 10,000 entities)
var config = new OctreeConfig
{
    MaxDepth = 8,
    MaxEntitiesPerNode = 32,
    LoosenessFactor = 2.0f
};

// Large world (> 10,000 entities)
var config = new OctreeConfig
{
    MaxDepth = 10,
    MaxEntitiesPerNode = 64,
    LoosenessFactor = 2.5f
};
```

### Profiling

```csharp
public class OctreeProfiler
{
    public void ProfileQueries()
    {
        using (Profiler.Profile("Octree.QueryRadius"))
        {
            octree.QueryRadius(center, radius);
        }
    }
    
    public void PrintHotspots()
    {
        var stats = octree.GetStatistics();
        
        Console.WriteLine($"Total queries: {stats.TotalQueries}");
        Console.WriteLine($"Avg query time: {stats.AverageQueryTimeMs}ms");
        Console.WriteLine($"Memory usage: {stats.MemoryUsageBytes / 1024}KB");
    }
}
```

---

## Next Steps

1. **Week 5:** Implement basic octree structure
2. **Week 6:** Add parallel updates with job system
3. **Week 7:** Optimize and tune performance
4. **Week 8:** Integration testing with entity system

---

**Status:** Implementation Ready  
**Dependencies:** Job System, Memory Management  
**Timeline:** 4 weeks  
**Priority:** High

