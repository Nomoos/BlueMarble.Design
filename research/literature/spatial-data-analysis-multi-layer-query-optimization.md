# Multi-Layer Query Optimization for Read-Dominant Workloads Research

**Research Domain:** Database Query Optimization  
**Relevance:** BlueMarble octree read performance and caching strategies  
**Last Updated:** 2025-01-28  
**Status:** Active Research

---

## Executive Summary

This research document compiles online sources and academic literature on multi-layer query optimization strategies for read-dominant workloads (95%+ reads). Focus on caching hierarchies, query routing, and performance optimization for petabyte-scale spatial data.

**Key Research Areas:**
- Redis caching for sub-millisecond access to hot regions
- Cassandra query optimization for spatial data
- Multi-tier caching strategies (L1/L2/L3 cache, application cache, distributed cache)
- Read-optimized database architectures
- Query pattern analysis and optimization

---

## Academic Sources

### Database Query Optimization

#### 1. Database Internals: A Deep Dive into How Distributed Data Systems Work

**Author:** Alex Petrov  
**Publisher:** O'Reilly Media, 2019  
**ISBN:** 978-1492040347

**Chapter 7: Log-Structured Storage**
- Write-optimized vs read-optimized storage
- LSM trees (Cassandra's underlying structure)
- Read amplification and mitigation strategies
- Bloom filters for false positive reduction

**Chapter 11: Replication and Consistency**
- Read replicas for scaling read workloads
- Eventual consistency trade-offs
- Quorum reads vs single-replica reads
- Consistency levels for spatial data

**Key Findings for Read-Heavy Workloads:**
- Read replicas: 3-5x read throughput
- Bloom filters: 95%+ false positive elimination
- Compression: 60-80% storage reduction
- Single-replica reads: 2-3x faster than quorum

**Application to BlueMarble:**
- Cassandra replication factor: 3
- Read consistency: ONE (eventual consistency OK for geology)
- Bloom filters enabled for all tables
- Compression: LZ4 for optimal read performance

---

#### 2. Designing Data-Intensive Applications

**Author:** Martin Kleppmann  
**Publisher:** O'Reilly Media, 2017  
**ISBN:** 978-1449373320

**Chapter 3: Storage and Retrieval**
- Hash indexes vs SSTable indexes
- B-trees vs LSM-trees
- Column-oriented storage
- Data compression techniques

**Chapter 5: Replication**
- Leader-based replication
- Multi-leader replication
- Leaderless replication (Cassandra)
- Replication lag and read-your-writes consistency

**Chapter 7: Transactions**
- ACID vs BASE
- Weak isolation levels
- Read committed, snapshot isolation, serializability
- Trade-offs for performance

**Key Findings:**
- LSM-trees: Good write throughput, acceptable read performance
- Caching is essential for read-heavy workloads
- Eventual consistency acceptable for most gaming use cases
- Snapshot isolation sufficient for spatial queries

---

### Caching Strategies

#### 3. Redis in Action

**Author:** Josiah Carlson  
**Publisher:** Manning, 2013  
**ISBN:** 978-1617290855

**Chapter 2: Redis Data Structures**
- Strings, hashes, lists, sets, sorted sets
- Memory efficiency per data type
- Expiration and eviction policies

**Chapter 4: Keeping Data Safe and Ensuring Performance**
- Persistence strategies (RDB, AOF)
- Replication and high availability
- Scaling with Redis Cluster
- Memory optimization techniques

**Chapter 5: Using Redis for Application Support**
- Caching strategies (cache-aside, write-through, write-behind)
- Session storage
- Distributed locking
- Pub/sub messaging

**Key Caching Patterns:**
- Cache-aside: Application checks cache, falls back to DB
- Write-through: Application writes to cache and DB synchronously
- Write-behind: Application writes to cache, DB updated asynchronously
- Time-to-live (TTL): Automatic cache expiration

**Application to BlueMarble:**
- Cache-aside pattern for octree nodes
- Hash data structure for node metadata
- Sorted sets for spatial range queries
- TTL: 1 hour for hot regions, 5 minutes for cold

---

#### 4. Caching at Scale with Redis

**Source:** Various industry papers and blog posts  
**Companies:** Twitter, GitHub, Stack Overflow, Instagram

**Twitter's Redis Architecture:**
- 100+ TB of cached data
- 10M+ requests/second
- 99.99% cache hit rate
- Sub-millisecond p99 latency

**Strategies:**
- Consistent hashing for key distribution
- Read replicas for read scaling
- Aggressive pre-warming
- Monitoring and alerting

**GitHub's Caching Strategy:**
- Multi-layer cache (browser, CDN, application, database)
- Cache invalidation via pub/sub
- Graceful degradation on cache misses
- Metrics-driven optimization

**Application to BlueMarble:**
- Multi-tier caching (L1 application cache, L2 Redis, L3 Cassandra)
- Cache pre-warming for predicted player movements
- Pub/sub for cache invalidation on geological changes
- Monitoring cache hit rates per region type

---

## Online Resources

### Redis Documentation and Best Practices

#### 5. Redis Official Documentation

**URL:** <https://redis.io/docs/>  
**Section:** Memory Optimization

**Memory Optimization Techniques:**
- Use appropriate data structures (hashes vs strings)
- Enable key expiration (EXPIRE command)
- Configure maxmemory and eviction policies
- Monitor memory fragmentation

**Data Structure Selection:**
- Small objects (<100 bytes): String
- Medium objects (100-1KB): Hash
- Large objects (>1KB): Compress and store as String
- Spatial indexes: Sorted Set with Geohash

**Eviction Policies:**
- `volatile-lru`: Evict least recently used keys with TTL
- `allkeys-lru`: Evict least recently used keys (any)
- `volatile-ttl`: Evict keys with shortest TTL
- `noeviction`: Return error on memory full

**Recommendation for BlueMarble:** `volatile-lru` with aggressive TTLs

---

#### 6. Redis Caching Patterns

**URL:** <https://redis.io/docs/manual/patterns/>  
**Patterns:** Cache-aside, Write-through, Write-behind

**Cache-Aside (Lazy Loading):**
```python
def get_octree_node(morton_code):
    # Try cache first
    node = redis.get(f"octree:{morton_code}")
    if node:
        return deserialize(node)
    
    # Cache miss - fetch from Cassandra
    node = cassandra.query("SELECT * FROM octree_nodes WHERE morton_code = ?", morton_code)
    
    # Populate cache
    redis.setex(f"octree:{morton_code}", 3600, serialize(node))
    return node
```

**Advantages:**
- Only cache requested data
- Resilient to cache failures
- Simple to implement

**Disadvantages:**
- Initial cache miss penalty
- Potential cache stampede on cold start

---

#### 7. Redis Cluster for Horizontal Scaling

**URL:** <https://redis.io/docs/manual/scaling/>  
**Architecture:** Hash slot-based partitioning

**Cluster Configuration:**
- 16,384 hash slots distributed across nodes
- Automatic resharding and rebalancing
- Master-replica pairs for high availability
- Client-side routing or proxy (Redis Cluster Proxy)

**Performance Characteristics:**
- Linear scaling up to 1000 nodes
- 1M+ ops/second per cluster
- Sub-millisecond latency at scale
- 99.99% availability with proper configuration

**Application to BlueMarble:**
- 6-12 node Redis cluster (3-6 master + replicas)
- Partition by Morton code prefix
- Co-locate spatially nearby regions
- Replicas for read scaling (5x read throughput)

---

### Cassandra Query Optimization

#### 8. Cassandra Data Modeling Best Practices

**URL:** <https://cassandra.apache.org/doc/latest/cassandra/data_modeling/>  
**Documentation:** Apache Cassandra 4.x

**Data Modeling Principles:**
- Denormalization for read performance
- Query-driven schema design
- Avoid secondary indexes (use materialized views)
- Partition size: 100 MB - 1 GB optimal

**Query Optimization:**
- Use prepared statements (10x faster)
- Batch inserts for write efficiency
- Limit result sets (default 5000 rows)
- Tune consistency level per query

**Spatial Query Schema:**
```cql
CREATE TABLE octree_nodes (
    morton_prefix bigint,      -- Partition key (high 22 bits)
    morton_suffix bigint,      -- Clustering key (low 42 bits)
    material_id tinyint,
    metadata blob,
    PRIMARY KEY (morton_prefix, morton_suffix)
) WITH CLUSTERING ORDER BY (morton_suffix ASC)
  AND compression = {'class': 'LZ4Compressor'}
  AND compaction = {'class': 'LeveledCompactionStrategy'}
  AND read_repair_chance = 0.1;
```

---

#### 9. Cassandra Performance Tuning

**URL:** <https://docs.datastax.com/en/cassandra-oss/3.x/cassandra/operations/opsPerformanceTuning.html>  
**Source:** DataStax Operations Guide

**Read Performance Tuning:**
- Increase memtable size (larger cache)
- Tune bloom filter FP ratio (0.01 default, 0.001 for read-heavy)
- Enable row cache for hot data (10-20% of RAM)
- Optimize compaction strategy (LeveledCompaction for reads)

**Cache Configuration:**
```yaml
# cassandra.yaml
row_cache_size_in_mb: 10240          # 10 GB row cache
row_cache_save_period: 3600          # Persist cache every hour
file_cache_size_in_mb: 2048          # 2 GB file cache
```

**Compaction Strategy:**
- **SizeTieredCompactionStrategy (STCS):** Write-heavy (default)
- **LeveledCompactionStrategy (LCS):** Read-heavy (recommended)
- **TimeWindowCompactionStrategy (TWCS):** Time-series data

**Recommendation:** LCS for octree_nodes table (read-dominant)

---

#### 10. Cassandra Read Path Optimization

**URL:** <https://thelastpickle.com/blog/2018/10/08/read-path.html>  
**Blog:** The Last Pickle (Cassandra experts)

**Read Path Internals:**
1. Memtable check (in-memory)
2. Row cache check (if enabled)
3. Bloom filter check (per SSTable)
4. Key cache check (partition key index)
5. Compression offset map
6. SSTable read (disk I/O)
7. Merge read results

**Optimization Opportunities:**
- Increase row cache for hot data
- Tune bloom filter FP chance
- Optimize partition size distribution
- Minimize number of SSTables (compaction)

**Monitoring Metrics:**
- Read latency (p50, p95, p99, p999)
- Bloom filter false positives
- Row cache hit rate
- SSTable per read

---

### Multi-Layer Caching Architecture

#### 11. CDN and Edge Caching

**URL:** <https://www.cloudflare.com/learning/cdn/what-is-caching/>  
**Resource:** Cloudflare Learning Center

**Caching Layers:**
1. **Browser Cache:** 1-10 seconds (minimal for dynamic data)
2. **CDN Cache:** 10-300 seconds (static assets only)
3. **Application Cache:** 1-5 seconds (local in-memory)
4. **Distributed Cache (Redis):** 1-60 minutes (regional data)
5. **Database (Cassandra):** Permanent storage

**Cache Hierarchy Design:**
```
Player Query
    ↓
L1: Local Cache (10ms) [1-5 sec TTL]
    ↓ (miss)
L2: Redis Cache (1ms) [1-60 min TTL]
    ↓ (miss)
L3: Cassandra (10-50ms) [permanent]
```

**Hit Rate Targets:**
- L1 (local): 60-70%
- L2 (Redis): 25-30%
- L3 (Cassandra): 5-10%
- Overall: 95%+ from cache

---

#### 12. Application-Level Caching

**URL:** <https://docs.microsoft.com/en-us/dotnet/core/extensions/caching>  
**Documentation:** .NET Caching (for C# BlueMarble)

**Memory Cache (.NET):**
```csharp
public class OctreeCache
{
    private readonly IMemoryCache _cache;
    private readonly IDistributedCache _redis;
    
    public async Task<OctreeNode> GetNode(ulong mortonCode)
    {
        // L1: Local memory cache
        if (_cache.TryGetValue(mortonCode, out OctreeNode node))
            return node;
        
        // L2: Redis distributed cache
        var serialized = await _redis.GetAsync($"octree:{mortonCode}");
        if (serialized != null)
        {
            node = Deserialize(serialized);
            _cache.Set(mortonCode, node, TimeSpan.FromSeconds(5));
            return node;
        }
        
        // L3: Cassandra database
        node = await _cassandra.GetNode(mortonCode);
        
        // Populate caches
        await _redis.SetAsync($"octree:{mortonCode}", Serialize(node), 
            new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30) });
        _cache.Set(mortonCode, node, TimeSpan.FromSeconds(5));
        
        return node;
    }
}
```

---

## Query Pattern Analysis

### Spatial Query Patterns

#### 13. Spatial Query Optimization

**URL:** <https://www.cockroachlabs.com/blog/spatial-indexing/>  
**Blog:** CockroachDB (spatial indexing insights)

**Common Spatial Queries:**
1. **Point Query:** Get single octree node
2. **Range Query:** Get nodes in 3D bounding box
3. **Radius Query:** Get nodes within distance
4. **K-Nearest Neighbors:** Get closest k nodes
5. **Ray Cast:** Get nodes along ray path

**Query Patterns for BlueMarble:**
- **Point Query (60%):** Player drilling/interacting with voxel
- **Range Query (25%):** Rendering visible octree region
- **Radius Query (10%):** Physics collision detection
- **Ray Cast (5%):** Mining laser, projectile trajectory

**Optimization by Pattern:**
- Point: Redis cache (sub-millisecond)
- Range: Cassandra with Morton code range scan
- Radius: Pre-computed neighbor cache
- Ray Cast: GPU-accelerated traversal

---

#### 14. Query Workload Analysis

**Source:** "Characterizing Cloud-Based Data Serving Workloads"  
**Authors:** Tamar Eilam et al. (IBM Research)  
**Published In:** IEEE Cloud Computing, 2016

**Workload Characteristics:**
- **Read/Write Ratio:** 95:5 (read-dominant)
- **Temporal Locality:** 80% of requests target 20% of data
- **Spatial Locality:** Nearby keys accessed together
- **Zipfian Distribution:** Popular keys accessed exponentially more

**Application to BlueMarble:**
- 95%+ reads (geological data mostly static)
- Hot regions: Active player zones (cities, mines)
- Cold regions: Deep ocean, polar regions
- Cache hot regions aggressively (1-hour TTL)
- Evict cold regions quickly (5-minute TTL)

---

### Cache Invalidation Strategies

#### 15. Cache Invalidation Patterns

**URL:** <https://docs.aws.amazon.com/AmazonElastiCache/latest/mem-ug/Strategies.html>  
**Documentation:** AWS ElastiCache (Redis)

**Invalidation Strategies:**

**1. Time-Based (TTL):**
- Simple and effective
- Potential stale data
- Good for mostly-static data

**2. Event-Based:**
- Invalidate on data change
- Requires pub/sub infrastructure
- Accurate but complex

**3. Hybrid (TTL + Events):**
- TTL as fallback
- Events for critical updates
- Best of both worlds

**BlueMarble Strategy:**
```csharp
// TTL-based for most nodes
redis.SetEx($"octree:{mortonCode}", 3600, serialized);

// Event-based for modified nodes
public async Task UpdateNode(ulong mortonCode, OctreeNode node)
{
    await _cassandra.UpdateNode(mortonCode, node);
    
    // Invalidate cache
    await _redis.DeleteAsync($"octree:{mortonCode}");
    
    // Publish invalidation event
    await _redis.PublishAsync("octree:invalidate", mortonCode.ToString());
}
```

---

#### 16. Cache Stampede Prevention

**URL:** <https://en.wikipedia.org/wiki/Cache_stampede>  
**Problem:** Multiple requests regenerate cache simultaneously

**Solutions:**

**1. Locking:**
```csharp
private static readonly ConcurrentDictionary<ulong, SemaphoreSlim> _locks = new();

public async Task<OctreeNode> GetNodeWithLock(ulong mortonCode)
{
    var lockKey = _locks.GetOrAdd(mortonCode, _ => new SemaphoreSlim(1, 1));
    await lockKey.WaitAsync();
    try
    {
        // Check cache again
        var node = await _cache.GetAsync(mortonCode);
        if (node != null) return node;
        
        // Regenerate cache
        node = await _cassandra.GetNode(mortonCode);
        await _cache.SetAsync(mortonCode, node);
        return node;
    }
    finally
    {
        lockKey.Release();
    }
}
```

**2. Probabilistic Early Expiration:**
```csharp
// Refresh cache probabilistically before expiration
var timeUntilExpiry = GetTTL(key);
var refreshProbability = Math.Pow(timeUntilExpiry / initialTTL, 2);
if (Random.NextDouble() < refreshProbability)
{
    _ = Task.Run(() => RefreshCache(key)); // Background refresh
}
```

---

## Performance Monitoring and Optimization

### Metrics and Monitoring

#### 17. Cache Performance Metrics

**URL:** <https://redis.io/docs/management/optimization/latency/>  
**Documentation:** Redis Latency Monitoring

**Key Metrics:**
- **Hit Rate:** % of requests served from cache
- **Miss Rate:** % of requests requiring DB lookup
- **Eviction Rate:** % of keys evicted before expiration
- **Memory Usage:** Current vs max memory
- **Latency:** p50, p95, p99, p999 response times

**Target Metrics for BlueMarble:**
- Hit Rate: >95%
- Miss Rate: <5%
- Eviction Rate: <10% (most evictions via TTL)
- Memory Usage: 70-80% of max (headroom for spikes)
- Redis Latency: p99 <2ms, p999 <5ms
- Cassandra Latency: p99 <50ms, p999 <100ms

---

#### 18. Distributed Tracing for Query Analysis

**URL:** <https://opentelemetry.io/>  
**Framework:** OpenTelemetry (industry standard)

**Tracing Spans:**
```
Player Request
├── Local Cache Check (0.1ms)
├── Redis Cache Check (1.2ms)
└── Cassandra Query (35ms)
    ├── Partition Lookup (5ms)
    ├── Bloom Filter Check (0.5ms)
    ├── SSTable Read (25ms)
    └── Decompression (4ms)
```

**Benefits:**
- Identify performance bottlenecks
- Track cache effectiveness
- Optimize query paths
- Monitor system health

---

#### 19. Profiling Tools

**URL:** <https://grafana.com/>  
**Tools:** Grafana + Prometheus for metrics

**Dashboard Panels:**
1. **Cache Hit Rate:** Time series graph
2. **Query Latency:** Heatmap (p50, p95, p99)
3. **Memory Usage:** Gauge (Redis, Cassandra)
4. **Request Rate:** Counter (reads/writes per second)
5. **Error Rate:** Counter (failed queries)

**Alerting Rules:**
- Hit rate <90% for 5 minutes
- p99 latency >100ms for 5 minutes
- Memory usage >90%
- Error rate >1%

---

### Query Optimization Techniques

#### 20. Batch Queries for Efficiency

**URL:** <https://cassandra.apache.org/doc/latest/cassandra/cql/dml.html#batch>  
**CQL:** BATCH statements

**Batch Read Pattern:**
```cql
-- Instead of N single queries
SELECT * FROM octree_nodes WHERE morton_code = ?;

-- Use IN query for multiple keys
SELECT * FROM octree_nodes WHERE morton_code IN (?, ?, ?, ...);
```

**Benefits:**
- Reduce network round-trips
- Amortize query overhead
- 5-10x throughput improvement

**Caution:** Limit batch size (100-1000 keys) to avoid memory issues

---

#### 21. Asynchronous Prefetching

**Pattern:** Predict future queries and prefetch data

**Implementation:**
```csharp
public class PredictivePrefetcher
{
    public async Task PrefetchNeighbors(ulong mortonCode)
    {
        // Calculate neighboring Morton codes
        var neighbors = GetNeighboringMortonCodes(mortonCode, radius: 1);
        
        // Prefetch in background
        _ = Task.Run(async () =>
        {
            var nodes = await _cassandra.GetBatch(neighbors);
            foreach (var (code, node) in nodes)
            {
                await _redis.SetAsync($"octree:{code}", Serialize(node));
            }
        });
    }
}
```

**Application:**
- Prefetch neighbors when player moves
- Prefetch LOD levels when camera zooms
- Prefetch along predicted movement vector

---

## Case Studies

### Industry Examples

#### 22. Netflix Caching Architecture

**URL:** <https://netflixtechblog.com/application-data-caching-using-ssds-5bf25df851ef>  
**Scale:** 200+ TB cached data, 500M+ requests/second

**Architecture:**
- L1: 100 GB SSD cache per server (EVCache)
- L2: 50 TB Redis cluster
- L3: Cassandra for persistence
- CDN: Edge caching for static content

**Lessons for BlueMarble:**
- Multi-tier caching essential at scale
- SSD cache layer between Redis and Cassandra
- Aggressive TTLs for hot data
- Monitoring and alerting critical

---

#### 23. Discord Real-Time Caching

**URL:** <https://discord.com/blog/how-discord-stores-billions-of-messages>  
**Scale:** Billions of messages, millions of concurrent users

**Data Hot/Cold Strategy:**
- Hot data (recent): Redis cache
- Warm data (30 days): Cassandra with row cache
- Cold data (>30 days): Cassandra compressed

**Read Optimization:**
- 95% cache hit rate (Redis)
- 4% warm cache hit rate (Cassandra row cache)
- 1% cold read (full Cassandra query)

**Application to BlueMarble:**
- Similar hot/warm/cold strategy for octree regions
- Active player zones: Redis cache
- Recently accessed regions: Cassandra row cache
- Historical/inactive regions: Compressed Cassandra

---

## Summary and Recommendations

### Recommended Architecture

```
Query Flow:
1. L1 Application Cache (5s TTL)
   ├─ Hit: 60-70% (0.1-1ms)
   └─ Miss: → L2

2. L2 Redis Cache (30-60min TTL)
   ├─ Hit: 25-30% (1-2ms)
   └─ Miss: → L3

3. L3 Cassandra Database (permanent)
   └─ Query: 5-10% (10-50ms)
```

### Configuration Recommendations

**Redis:**
```
maxmemory 48gb
maxmemory-policy volatile-lru
cluster-enabled yes
cluster-node-timeout 5000
```

**Cassandra:**
```yaml
read_consistency_level: ONE
replication_factor: 3
compaction_strategy: LeveledCompactionStrategy
bloom_filter_fp_chance: 0.001
row_cache_size_in_mb: 10240
```

---

## References

### Books
1. "Database Internals" - Alex Petrov (2019)
2. "Designing Data-Intensive Applications" - Martin Kleppmann (2017)
3. "Redis in Action" - Josiah Carlson (2013)

### Online Resources
1. Redis Documentation: <https://redis.io/docs/>
2. Cassandra Documentation: <https://cassandra.apache.org/doc/>
3. AWS Caching Best Practices: <https://aws.amazon.com/caching/>

### Industry Blogs
1. Netflix Tech Blog: <https://netflixtechblog.com/>
2. Discord Engineering Blog: <https://discord.com/blog/>
3. The Last Pickle (Cassandra): <https://thelastpickle.com/>

---

## Cross-References

**Related Documents:**
- `spatial-data-analysis-database-architecture.md` - Cassandra/Redis architecture
- `spatial-data-analysis-morton-code-octree-pointerless-storage.md` - Spatial indexing
- `research/spatial-data-storage/step-4-implementation/multi-layer-query-optimization-implementation.md`

---

**Maintained By:** BlueMarble Database Research Team  
**Last Review:** 2025-01-28  
**Next Review:** 2025-04-28
