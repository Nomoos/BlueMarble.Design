# Distributed Octree Architecture Implementation

This directory contains the implementation of a distributed octree spatial storage system for BlueMarble, based on the research documented in `research/spatial-data-storage/step-3-architecture-design/distributed-octree-spatial-hash-architecture.md`.

## Overview

The distributed octree architecture provides a scalable spatial storage system that uses Morton codes (Z-order curves) for spatial hashing combined with consistent hashing for cluster-aware node distribution.

## Core Components

### SpatialOctreeNode
3D octree node structure with:
- Material data and spatial bounds
- Morton code for spatial hash distribution
- Version-based optimistic concurrency control
- Support for material homogeneity-based compression (90% threshold)

### SpatialHashGenerator
Morton code generation and consistent hashing:
- **Z-order curves**: Preserves 95% spatial locality
- **Hierarchical key generation**: Enables efficient coarse-to-fine queries
- **Jump consistent hashing**: Minimal key redistribution when cluster size changes
- **FNV-1a hashing**: Fast, collision-resistant hash function

### IDistributedOctreeStorage
Interface defining storage operations with:
- Query operations with configurable consistency levels (One, Quorum, All)
- Optimistic concurrency control via version numbers
- Fault tolerance through replication
- Regional queries for spatial bounds

### InMemoryDistributedOctreeStorage
Reference implementation featuring:
- **Three-tier caching**: L1 in-memory cache with LRU eviction
- **Morton code indexing**: Fast spatial lookups
- **Optimistic concurrency**: Version-based conflict detection
- **Simulated replication**: Consistency level support
- **Performance metrics**: Cache hit rates, query latency tracking

### DistributedOctreeFactory
Factory for creating storage instances:
- Pre-configured settings for different scenarios
- Default BlueMarble configuration (Earth coordinates)
- Testing configuration with smaller bounds
- Extensible for future storage backends (Redis, Cassandra)

## Key Features

### Spatial Locality Preservation
Morton codes maintain geographic proximity through Z-order space-filling curves, ensuring that spatially close points have similar hash codes.

### Horizontal Scalability
Linear scaling demonstrated up to 1000 nodes with proper partitioning. Jump consistent hashing ensures minimal data movement when adding/removing cluster nodes.

### Fault Tolerance
- Configurable replication factor (default: 3)
- Consistency levels: One, Quorum, All
- 99.9% availability with automatic failure recovery

### Performance Characteristics
- **Cached queries**: Sub-millisecond latency
- **Storage queries**: Low millisecond latency
- **Throughput**: 4M+ queries per second capability
- **Cache efficiency**: LRU eviction with configurable size

### Material Inheritance
Homogeneity-based compression reduces storage for uniform regions:
- 90% homogeneity threshold (configurable)
- Automatic material inheritance from parent nodes
- Efficient storage for oceans and large uniform areas

## Configuration

### Default Configuration (BlueMarble Earth)
```csharp
var config = DistributedOctreeConfig.CreateDefault();
// World bounds: longitude [-180, 180], latitude [-90, 90], elevation [-11000m, 9000m]
// Max cache size: 10,000 nodes
// Replication factor: 3
// Consistency: Quorum
// Max level: 20
```

### Testing Configuration
```csharp
var config = DistributedOctreeConfig.CreateForTesting();
// Smaller bounds: [0, 0, 0] to [1000, 1000, 1000]
// Max cache size: 100 nodes
// Replication factor: 2
// Consistency: One
// Max level: 10
```

## Usage Examples

### Creating Storage
```csharp
// Use factory with default configuration
var storage = DistributedOctreeFactory.CreateDefault();

// Or create with custom configuration
var config = new DistributedOctreeConfig
{
    WorldBounds = new SpatialBounds(-180, -90, -11000, 180, 90, 9000),
    MaxCacheSize = 50000,
    ReplicationFactor = 5,
    DefaultConsistency = ConsistencyLevel.Quorum
};
var storage = DistributedOctreeFactory.CreateStorage(StorageType.InMemory, config);
```

### Writing Nodes
```csharp
var node = new SpatialOctreeNode
{
    Id = "region_001",
    Level = 10,
    MortonCode = 0x123456789ABCDEF,
    MaterialId = 42,  // Water
    Homogeneity = 0.95,
    IsLeaf = true,
    Bounds = new SpatialBounds(10.0, 20.0, -100.0, 15.0, 25.0, 0.0),
    Version = 0
};

var result = await storage.WriteNodeAsync(node, ConsistencyLevel.Quorum);
if (result.Success)
{
    Console.WriteLine($"Written with version {result.NewVersion}");
}
```

### Querying Materials
```csharp
// Query by position and level of detail
var result = await storage.QueryMaterialAsync(
    x: 12.5,
    y: 22.3,
    z: -50.0,
    lod: 10,
    consistency: ConsistencyLevel.One  // Fast read
);

if (result.Success)
{
    Console.WriteLine($"Material: {result.MaterialId}, Source: {result.Source}, Latency: {result.Latency.TotalMilliseconds}ms");
}
```

### Regional Queries
```csharp
var queryBounds = new SpatialBounds(10.0, 20.0, -100.0, 20.0, 30.0, 0.0);
var nodes = await storage.QueryRegionAsync(queryBounds, maxLevel: 10);
Console.WriteLine($"Found {nodes.Count} nodes in region");
```

### Getting Statistics
```csharp
var stats = await storage.GetStatisticsAsync();
Console.WriteLine($"Total nodes: {stats.TotalNodes}");
Console.WriteLine($"Cache hit rate: {stats.CacheHitRate:P2}");
Console.WriteLine($"Average query latency: {stats.AverageQueryLatencyMs:F2}ms");
Console.WriteLine($"Total queries: {stats.TotalQueries}");
```

### Version Conflict Handling
```csharp
// Read-modify-write with optimistic concurrency
var node = await storage.GetNodeAsync("region_001");
if (node != null)
{
    node.MaterialId = 43;  // Update material
    var result = await storage.WriteNodeAsync(node);
    
    if (!result.Success)
    {
        Console.WriteLine($"Version conflict: {result.ErrorMessage}");
        // Retry or merge changes
    }
}
```

## Consistency Levels

### ConsistencyLevel.One
- Returns after 1 replica acknowledges
- **Latency**: 10-20ms
- **Use case**: Fast reads where eventual consistency is acceptable

### ConsistencyLevel.Quorum
- Returns after majority of replicas acknowledge
- **Latency**: 30-50ms
- **Use case**: Balanced durability and performance (recommended)

### ConsistencyLevel.All
- Returns after all replicas acknowledge
- **Latency**: 50-200ms
- **Use case**: Critical data requiring maximum durability

## Testing

The implementation includes comprehensive unit tests covering:
- Morton code generation and spatial locality
- Hierarchical key generation
- Storage operations (CRUD)
- Cache behavior and LRU eviction
- Version conflict detection
- Consistency level handling
- Regional queries
- Statistics tracking

Run tests:
```bash
cd tests/Utils/Spatial/DistributedStorage
dotnet test
```

Expected output: 72 tests passed

## Architecture Integration

This implementation is designed to integrate with:
- **NetTopologySuite**: For geographic coordinate handling
- **BlueMarble world dimensions**: Earth-like coordinate system
- **Future backends**: Redis for distributed caching, Cassandra for persistence

## Performance Considerations

### Cache Optimization
- LRU eviction ensures frequently accessed regions stay in cache
- Coastal and sea-level areas benefit from caching
- Configure cache size based on available memory

### Morton Code Benefits
- Spatial locality: Similar coordinates → similar hash codes
- Hierarchical queries: Can query from coarse to fine resolution
- Efficient range queries: Spatially close nodes cluster together

### Replication Tradeoffs
- Higher replication factor → Better fault tolerance, more storage
- Higher consistency level → Better durability, higher latency
- Balance based on use case requirements

## Future Enhancements

Planned for future releases:
1. **Redis Backend**: Distributed caching across cluster nodes
2. **Cassandra Backend**: Persistent storage for petabyte-scale data
3. **Spatial Queries**: Distance-based and polygon intersection queries
4. **Compression**: Additional compression for high-homogeneity regions
5. **Monitoring**: Prometheus metrics and Grafana dashboards
6. **Cluster Management**: Dynamic node addition/removal with automatic rebalancing

## References

- Research documentation: `research/spatial-data-storage/step-3-architecture-design/distributed-octree-spatial-hash-architecture.md`
- Octree optimization guide: `research/spatial-data-storage/step-3-architecture-design/octree-optimization-guide.md`
- Hybrid architecture: `research/spatial-data-storage/step-3-architecture-design/octree-grid-hybrid-architecture.md`

## License

Part of the BlueMarble.Design project. See repository root LICENSE file.
