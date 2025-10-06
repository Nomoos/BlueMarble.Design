# Database Architecture Research for Petabyte-Scale 3D Octree Storage

**Research Domain:** Database Architecture and Distributed Systems  
**Relevance:** BlueMarble primary storage with Apache Cassandra and Redis  
**Last Updated:** 2025-01-28  
**Status:** Active Research

---

## Executive Summary

This research document compiles online sources and academic literature on database architecture for petabyte-scale 3D octree storage. Focus on Apache Cassandra as primary storage with 3D Morton code optimization, Redis as caching layer, and LZ4 compression for storage reduction.

**Key Research Areas:**
- Apache Cassandra: Primary storage with spatial clustering
- Redis: Sub-millisecond caching layer for hot regions
- LZ4 Compression: 60-80% storage reduction
- Distributed database architectures
- Consistency models for read-dominant workloads

---

## Academic Sources

### Distributed Database Theory

#### 1. Database Internals: A Deep Dive into How Distributed Data Systems Work

**Author:** Alex Petrov  
**Publisher:** O'Reilly Media, 2019  
**ISBN:** 978-1492040347

**Part I: Storage Engines**
- Chapter 1: Introduction to storage engines
- Chapter 2: B-Tree Variants (traditional RDBMS)
- Chapter 3: LSM Trees (Cassandra's foundation)
- Chapter 4: Column-oriented storage

**Part II: Distributed Systems**
- Chapter 8: Introduction to distributed systems
- Chapter 9: Consistency and consensus
- Chapter 10: Leader election
- Chapter 11: Replication and consistency

**Key Insights for Cassandra:**
- LSM trees optimize write throughput via sequential I/O
- Read amplification: multiple SSTables checked per query
- Compaction strategies impact read/write balance
- Eventual consistency sufficient for most use cases

**Application to BlueMarble:**
- Cassandra's LSM architecture ideal for geological updates (write-optimized)
- Tunable consistency levels (ONE for reads, QUORUM for critical writes)
- Read optimization via bloom filters and row cache
- Horizontal scaling to petabyte scale

---

#### 2. Designing Data-Intensive Applications

**Author:** Martin Kleppmann  
**Publisher:** O'Reilly Media, 2017  
**ISBN:** 978-1449373320

**Chapter 5: Replication**
- Single-leader vs multi-leader vs leaderless
- Cassandra's leaderless (dynamo-style) replication
- Conflict resolution strategies
- Consistency levels and quorum reads

**Chapter 6: Partitioning**
- Partitioning strategies (range, hash, compound)
- Partition key selection for Cassandra
- Rebalancing and hotspots
- Secondary indexes and performance

**Chapter 7: Transactions**
- ACID vs BASE properties
- Distributed transactions and 2PC
- Lightweight transactions (Paxos) in Cassandra
- Trade-offs for gaming workloads

**Key Findings:**
- Cassandra uses consistent hashing with virtual nodes
- Partition key determines data distribution
- Secondary indexes are expensive (avoid for hot queries)
- Eventual consistency acceptable for geological data

**Application to BlueMarble:**
- Morton code as partition key for spatial locality
- Replication factor 3 for durability
- ONE consistency for reads (95% of queries)
- QUORUM for critical writes (player modifications)

---

### NoSQL Database Design

#### 3. NoSQL Distilled: A Brief Guide to the Emerging World of Polyglot Persistence

**Authors:** Pramod J. Sadalage, Martin Fowler  
**Publisher:** Addison-Wesley, 2012  
**ISBN:** 978-0321826626

**Chapter 4: Aggregate Data Models**
- Key-value stores
- Document databases
- Column-family stores (Cassandra)
- Graph databases

**Chapter 9: Consistency**
- CAP theorem implications
- Eventual consistency
- Session consistency
- Strong consistency trade-offs

**Chapter 10: Version Stamps**
- Vector clocks
- Last-write-wins
- Conflict resolution

**Cassandra Characteristics:**
- Column-family store with tunable consistency
- AP in CAP theorem (availability + partition tolerance)
- Eventually consistent by default
- Strong consistency via quorum reads/writes

---

## Apache Cassandra Specific Resources

### Official Documentation

#### 4. Apache Cassandra Documentation

**URL:** <https://cassandra.apache.org/doc/latest/>  
**Version:** 4.x (latest stable)

**Data Modeling:**
- Query-driven schema design
- Partition key and clustering key selection
- Denormalization for performance
- Materialized views and secondary indexes

**Architecture:**
- Ring topology with consistent hashing
- Virtual nodes (vnodes) for load balancing
- Gossip protocol for cluster membership
- Hinted handoff for resilience

**Performance Tuning:**
- Compaction strategies (STCS, LCS, TWCS)
- Compression options (LZ4, Snappy, Zstd)
- Cache configuration (row cache, key cache)
- Memory settings (heap, off-heap)

---

#### 5. Cassandra Data Modeling Best Practices

**URL:** <https://cassandra.apache.org/doc/latest/cassandra/data_modeling/>

**Design Principles:**

**1. One Query = One Table:**
- Denormalize data for each query pattern
- No joins (not supported efficiently)
- Duplicate data across tables if needed

**2. Partition Key Selection:**
- Distribute data evenly across cluster
- Avoid hotspots (popular keys)
- Keep partition size <100 MB (optimal)
- Use compound partition keys for distribution

**3. Clustering Key Optimization:**
- Define sort order at schema level
- Enable efficient range queries
- Support time-series patterns

**Example for BlueMarble:**
```cql
CREATE TABLE octree_nodes (
    morton_prefix bigint,        -- Partition key (high 22 bits)
    morton_suffix bigint,        -- Clustering key (low 42 bits)
    level tinyint,
    material_id tinyint,
    child_mask tinyint,
    metadata blob,
    PRIMARY KEY (morton_prefix, morton_suffix)
) WITH CLUSTERING ORDER BY (morton_suffix ASC)
  AND compression = {'class': 'LZ4Compressor', 'chunk_length_in_kb': 64}
  AND compaction = {'class': 'LeveledCompactionStrategy'}
  AND comment = 'Octree nodes with Morton code spatial indexing';
```

**Rationale:**
- `morton_prefix`: Partition key ensures even distribution
- `morton_suffix`: Clustering key enables range queries
- Spatial locality: Nearby voxels have similar Morton codes
- Compression: LZ4 for 60-80% storage reduction

---

#### 6. Cassandra Performance Tuning

**URL:** <https://docs.datastax.com/en/cassandra-oss/3.x/cassandra/operations/>

**JVM Tuning:**
```yaml
# cassandra-env.sh
MAX_HEAP_SIZE="16G"
HEAP_NEWSIZE="4G"
```

**Compression Configuration:**
```yaml
# LZ4 compression (fastest, good ratio)
compression:
  class: LZ4Compressor
  chunk_length_in_kb: 64
  
# Zstd compression (better ratio, slower)
compression:
  class: ZstdCompressor
  compression_level: 3
  chunk_length_in_kb: 64
```

**Compaction Strategy:**
```yaml
# LeveledCompactionStrategy for read-heavy workloads
compaction:
  class: LeveledCompactionStrategy
  sstable_size_in_mb: 160
  
# SizeTieredCompactionStrategy for write-heavy
compaction:
  class: SizeTieredCompactionStrategy
  min_threshold: 4
  max_threshold: 32
```

**Cache Settings:**
```yaml
# Row cache for hot data
row_cache_size_in_mb: 10240           # 10 GB
row_cache_save_period: 3600           # Save every hour
row_cache_keys_to_save: 100000

# Key cache for partition lookups
key_cache_size_in_mb: 1024            # 1 GB
key_cache_save_period: 14400          # Save every 4 hours
```

---

### Industry Resources

#### 7. DataStax Academy (Free Cassandra Training)

**URL:** <https://www.datastax.com/dev/academy>  
**Courses:** Free video training and certification

**Recommended Courses:**
- DS101: Introduction to Apache Cassandra
- DS201: Foundations of Apache Cassandra
- DS220: Data Modeling with Apache Cassandra
- DS310: DataStax Enterprise Operations

**Key Learnings:**
- Query-first data modeling methodology
- Partition key selection strategies
- Performance troubleshooting
- Production deployment best practices

---

#### 8. The Last Pickle Blog (Cassandra Experts)

**URL:** <https://thelastpickle.com/blog/>  
**Topics:** Cassandra operations, tuning, troubleshooting

**Notable Articles:**
- "Cassandra Read Path" - Query execution internals
- "Compaction Strategies Explained" - When to use each strategy
- "Cassandra Anti-Patterns" - Common mistakes to avoid
- "Monitoring Cassandra at Scale" - Metrics and alerting

**Key Takeaways:**
- Monitor SSTable count per read (target <10)
- Tune bloom filter FP chance (0.001 for read-heavy)
- Use prepared statements (10x performance improvement)
- Avoid large partitions (>100 MB)

---

## Redis as Caching Layer

### Redis Architecture

#### 9. Redis Official Documentation

**URL:** <https://redis.io/docs/>  
**Documentation Sections:**

**Data Structures:**
- Strings: Simple key-value (octree node serialization)
- Hashes: Small objects with fields (node metadata)
- Sets: Unordered collections (material IDs in region)
- Sorted Sets: Spatial indexes (geohash/Morton code)
- Streams: Event logs (cache invalidation)

**Persistence:**
- RDB (point-in-time snapshots)
- AOF (append-only file log)
- No persistence (pure cache)

**Replication:**
- Master-replica async replication
- Sentinel for automatic failover
- Redis Cluster for horizontal scaling

**Recommendation for BlueMarble:** Redis Cluster with RDB persistence

---

#### 10. Redis Cluster Architecture

**URL:** <https://redis.io/docs/management/scaling/>  
**Architecture:** Hash slot-based sharding

**Cluster Configuration:**
- 16,384 hash slots distributed across master nodes
- Each master has 1-2 replicas
- Client-side routing via CRC16 hash
- Automatic failover on master failure

**Deployment Recommendation:**
```
6-node cluster:
├── Master 1 (slots 0-5461) + Replica 1
├── Master 2 (slots 5462-10922) + Replica 2
└── Master 3 (slots 10923-16383) + Replica 3

Total capacity: 48 GB x 3 masters = 144 GB cache
Read throughput: ~500K ops/sec x 6 nodes = 3M ops/sec
Availability: 99.99% (automatic failover)
```

---

#### 11. Redis Memory Optimization

**URL:** <https://redis.io/docs/management/optimization/memory-optimization/>

**Memory Efficiency Techniques:**

**1. Hash Encoding:**
```
# Small hashes stored as ziplist (compressed)
hash-max-ziplist-entries 512
hash-max-ziplist-value 64
```

**2. String Compression:**
```csharp
// Compress large values before storing
byte[] compressed = LZ4.Compress(serialized);
redis.Set($"octree:{mortonCode}", compressed);
```

**3. Eviction Policy:**
```
maxmemory 48gb
maxmemory-policy volatile-lru  # Evict least recently used keys with TTL
```

**4. Expiration Strategy:**
```csharp
// Hot regions: 1 hour TTL
redis.SetEx($"octree:hot:{code}", 3600, data);

// Cold regions: 5 minute TTL
redis.SetEx($"octree:cold:{code}", 300, data);
```

---

### Redis Performance

#### 12. Redis Benchmarking

**URL:** <https://redis.io/docs/management/optimization/benchmarks/>  
**Tool:** redis-benchmark

**Typical Performance:**
- GET: 100,000-200,000 ops/sec per instance
- SET: 80,000-150,000 ops/sec per instance
- Latency: p50 <0.5ms, p99 <2ms, p999 <10ms
- Throughput: Limited by network (10 Gbps = ~1M small ops/sec)

**Optimization Tips:**
- Use pipelining for batch operations (5-10x throughput)
- Enable `tcp-keepalive` for connection stability
- Tune `tcp-backlog` for high connection counts
- Use Redis Cluster for horizontal scaling

**BlueMarble Target Performance:**
- 1M reads/sec across cluster
- p99 latency <2ms
- 95%+ cache hit rate
- 99.99% availability

---

#### 13. Redis Persistence Trade-offs

**URL:** <https://redis.io/docs/management/persistence/>

**RDB (Snapshot):**
- Pros: Fast restart, compact format, minimal performance impact
- Cons: Data loss potential (last snapshot to crash)
- Configuration: `save 900 1` (snapshot after 900s if 1+ keys changed)

**AOF (Append-Only File):**
- Pros: Minimal data loss (fsync every second)
- Cons: Larger file size, slower restart, performance impact
- Configuration: `appendfsync everysec`

**Hybrid (RDB + AOF):**
- Best of both: Fast restart + minimal data loss
- Available in Redis 4.0+
- Recommended for production

**BlueMarble Recommendation:**
- RDB for fast restarts (cache warmup from Cassandra anyway)
- No AOF (cache can be regenerated from Cassandra)
- Aggressive snapshots during off-peak hours

---

## Compression Technologies

### LZ4 Compression

#### 14. LZ4 Compression Library

**URL:** <https://lz4.github.io/lz4/>  
**Author:** Yann Collet (Meta)  
**License:** BSD 2-Clause

**Performance Characteristics:**
- Compression: 500 MB/s per core
- Decompression: 3-5 GB/s per core
- Ratio: 2-3x for typical data
- CPU overhead: Minimal (~5% for decompression)

**Benchmarks (Silesia corpus):**
- LZ4: 2.1x compression, 440 MB/s encode, 3200 MB/s decode
- Zlib: 2.7x compression, 40 MB/s encode, 350 MB/s decode
- Zstd: 2.8x compression, 300 MB/s encode, 800 MB/s decode

**Trade-offs:**
- LZ4: Fast, good ratio, minimal CPU
- Zstd: Better ratio, slower, more CPU
- Snappy: Fastest, lower ratio

**Recommendation for BlueMarble:** LZ4 for primary storage (Cassandra)

---

#### 15. LZ4 in Cassandra

**URL:** <https://cassandra.apache.org/doc/latest/cassandra/operating/compression.html>

**Configuration:**
```cql
CREATE TABLE octree_nodes (
    -- schema
) WITH compression = {
    'class': 'LZ4Compressor',
    'chunk_length_in_kb': 64
};
```

**Chunk Size Selection:**
- 16 KB: Better compression, higher CPU
- 64 KB: Good balance (recommended)
- 256 KB: Lower CPU, worse compression

**Monitoring:**
```bash
# Check compression ratio
nodetool tablestats keyspace.table

# Output includes:
# Space used (live): 1.5 TB
# Space used (total): 1.6 TB
# Compression ratio: 0.35 (65% reduction)
```

**Expected Results:**
- Octree material IDs: 2-3x compression
- Metadata blobs: 2-5x compression
- Overall: 60-80% storage reduction

---

### Alternative Compression

#### 16. Zstandard (Zstd) Compression

**URL:** <https://facebook.github.io/zstd/>  
**Maintainer:** Meta (Facebook)

**Advantages over LZ4:**
- Better compression ratio (2.5-3.5x vs 2-2.5x)
- Tunable compression levels (1-22)
- Dictionary training for domain-specific data
- Fast decompression (800-1500 MB/s)

**Use Cases:**
- Archival storage (high compression level)
- Cold data compression
- Network transfer optimization
- Backup compression

**Cassandra Integration:**
```cql
WITH compression = {
    'class': 'ZstdCompressor',
    'compression_level': 3,
    'chunk_length_in_kb': 64
};
```

**BlueMarble Application:**
- LZ4 for hot/warm data (primary storage)
- Zstd level 9 for cold data archival
- Zstd with trained dictionary for historical snapshots

---

## Spatial Clustering with Morton Codes

### Morton Code Partitioning

#### 17. Z-Order Curve for Database Partitioning

**URL:** <https://en.wikipedia.org/wiki/Z-order_curve>  
**Application:** Spatial data clustering in databases

**Partitioning Strategy:**
```
Morton Code: 64-bit integer
├── High 22 bits: Partition key (4M partitions)
└── Low 42 bits: Clustering key (4T nodes per partition)

Partition Size:
├── Average: 1000-10000 nodes per partition
└── Target: <100 MB per partition
```

**Benefits:**
- Spatial locality: Nearby voxels in same partition
- Even distribution: 22-bit space distributes load
- Range queries: Clustering key enables efficient scans
- Cache efficiency: Related data co-located

---

#### 18. Multi-Dimensional Indexing in Cassandra

**URL:** <https://www.datastax.com/blog/introduction-secondary-indexes-cassandra>  
**Topic:** Secondary indexes and alternatives

**Options for Spatial Queries:**

**1. Secondary Index (Avoid):**
```cql
CREATE INDEX ON octree_nodes (material_id);
-- Problem: Scans entire cluster, slow for large datasets
```

**2. Materialized View:**
```cql
CREATE MATERIALIZED VIEW nodes_by_material AS
    SELECT * FROM octree_nodes
    WHERE material_id IS NOT NULL
    PRIMARY KEY (material_id, morton_prefix, morton_suffix);
-- Better: Dedicated table, fast queries, eventual consistency
```

**3. Manual Denormalization (Best):**
```cql
-- Primary table (query by location)
CREATE TABLE octree_nodes (
    morton_prefix bigint,
    morton_suffix bigint,
    material_id tinyint,
    PRIMARY KEY (morton_prefix, morton_suffix)
);

-- Secondary table (query by material)
CREATE TABLE nodes_by_material (
    material_id tinyint,
    morton_code bigint,
    metadata blob,
    PRIMARY KEY (material_id, morton_code)
);
```

**Recommendation:** Materialized views for most spatial queries

---

## High Availability and Disaster Recovery

### Replication and Backup

#### 19. Cassandra Replication Strategies

**URL:** <https://cassandra.apache.org/doc/latest/cassandra/architecture/dynamo.html>

**Replication Factor (RF):**
```cql
CREATE KEYSPACE bluemarble WITH REPLICATION = {
    'class': 'NetworkTopologyStrategy',
    'datacenter1': 3,
    'datacenter2': 2  -- Optional: remote DR site
};
```

**Consistency Levels:**
- **ONE:** Single replica (fastest, eventual consistency)
- **QUORUM:** Majority (RF/2 + 1) (strong consistency)
- **ALL:** All replicas (slowest, strongest consistency)

**Recommendation:**
- Read consistency: ONE (95% of queries)
- Write consistency: QUORUM (important updates)
- RF: 3 within single datacenter
- Optional: RF 2 in remote datacenter for DR

---

#### 20. Backup and Restore

**URL:** <https://cassandra.apache.org/doc/latest/cassandra/operating/backups.html>

**Backup Strategies:**

**1. Snapshot:**
```bash
nodetool snapshot bluemarble octree_nodes
# Creates hardlinks, no space overhead, instant
```

**2. Incremental Backup:**
```yaml
# cassandra.yaml
incremental_backups: true
# Copies SSTables to backups/ directory on flush
```

**3. Off-site Backup:**
```bash
# Copy snapshots to S3/GCS
aws s3 sync /var/lib/cassandra/data/backups/ s3://bluemarble-backups/
```

**Restore Process:**
```bash
# 1. Stop Cassandra
service cassandra stop

# 2. Restore snapshot
sstableloader -d <node> <snapshot-directory>

# 3. Start Cassandra
service cassandra start

# 4. Run repair
nodetool repair
```

---

## Monitoring and Operations

### Observability

#### 21. Cassandra Monitoring

**URL:** <https://cassandra.apache.org/doc/latest/cassandra/operating/metrics.html>

**Key Metrics:**
- **Read latency:** p50, p95, p99, p999
- **Write latency:** p50, p95, p99, p999
- **Disk space:** Per node, per table
- **Compaction:** Pending compactions, bytes compacted
- **JVM:** Heap usage, GC pauses
- **Cache:** Hit rates (row cache, key cache)

**Tools:**
- Prometheus + Grafana: Metrics visualization
- Cassandra Exporter: Prometheus integration
- nodetool: Command-line metrics
- DataStax OpsCenter: Commercial monitoring

---

#### 22. Redis Monitoring

**URL:** <https://redis.io/docs/management/optimization/latency/>

**Key Metrics:**
- **Latency:** Average, p50, p95, p99
- **Memory:** Used memory, RSS, fragmentation
- **Hit rate:** Hits / (Hits + Misses)
- **Evictions:** Evicted keys per second
- **Connections:** Connected clients, rejected connections

**Commands:**
```bash
# Real-time stats
redis-cli --latency
redis-cli --stat

# Memory analysis
redis-cli INFO memory
redis-cli MEMORY DOCTOR

# Slow log
redis-cli SLOWLOG GET 10
```

---

## Case Studies

### Industry Implementations

#### 23. Netflix: Cassandra at Scale

**URL:** <https://netflixtechblog.com/netflix-global-cloud-part-1-building-a-worldwide-messaging-platform-9f6f6c02c77f>  
**Scale:** 100+ TB, 1000+ nodes, millions of ops/sec

**Architecture:**
- Multi-region Cassandra clusters
- 3x replication per region
- Cross-region async replication
- Netflix Priam for automation

**Lessons Learned:**
- Automate operations (deployment, backups, repairs)
- Monitor everything (metrics, logs, alerts)
- Chaos engineering (failure testing)
- Capacity planning essential

---

#### 24. Discord: Scaling to Trillions of Messages

**URL:** <https://discord.com/blog/how-discord-stores-billions-of-messages>  
**Scale:** Billions of messages, millions of concurrent users

**Evolution:**
- MongoDB → Cassandra migration
- Optimized data model for read performance
- Custom partition key strategy
- Compression enabled (50% reduction)

**Key Decisions:**
- Query-first data modeling
- Aggressive denormalization
- Read consistency ONE
- Monitoring and alerting critical

---

## Summary and Recommendations

### Recommended Technology Stack

```
Database Architecture:
├── Primary Storage: Apache Cassandra 4.x
│   ├── Replication Factor: 3
│   ├── Consistency: ONE (reads), QUORUM (writes)
│   ├── Compression: LZ4Compressor
│   └── Compaction: LeveledCompactionStrategy
│
├── Cache Layer: Redis Cluster 7.x
│   ├── Cluster Size: 6 nodes (3 master + 3 replica)
│   ├── Memory: 48 GB per master (144 GB total)
│   ├── Eviction: volatile-lru
│   └── Persistence: RDB snapshots
│
└── Backup: S3/GCS Off-site
    ├── Snapshots: Daily
    ├── Incremental: Hourly
    └── Retention: 30 days
```

### Performance Targets

| Metric | Target | Measurement |
|--------|--------|-------------|
| Read Latency (p99) | <50ms | Cassandra |
| Cache Latency (p99) | <2ms | Redis |
| Cache Hit Rate | >95% | Application |
| Write Latency (p99) | <100ms | Cassandra |
| Availability | 99.99% | Uptime |
| Storage Reduction | 60-80% | LZ4 Compression |

---

## References

### Books
1. "Database Internals" - Alex Petrov (2019)
2. "Designing Data-Intensive Applications" - Martin Kleppmann (2017)
3. "NoSQL Distilled" - Pramod J. Sadalage, Martin Fowler (2012)

### Online Resources
1. Apache Cassandra: <https://cassandra.apache.org/>
2. Redis: <https://redis.io/>
3. LZ4 Compression: <https://lz4.github.io/lz4/>
4. DataStax Academy: <https://www.datastax.com/dev/academy>

### Industry Blogs
1. Netflix Tech Blog: <https://netflixtechblog.com/>
2. Discord Engineering: <https://discord.com/blog/>
3. The Last Pickle: <https://thelastpickle.com/>

---

## Cross-References

**Related Documents:**
- `spatial-data-analysis-hybrid-compression-strategies.md`
- `spatial-data-analysis-morton-code-octree-pointerless-storage.md`
- `spatial-data-analysis-multi-layer-query-optimization.md`
- `research/spatial-data-storage/step-4-implementation/database-deployment-operational-guidelines.md`

---

**Maintained By:** BlueMarble Database Architecture Team  
**Last Review:** 2025-01-28  
**Next Review:** 2025-04-28
