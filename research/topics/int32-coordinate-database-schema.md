# Int32 Coordinate System - Database Schema Design

**Status**: Draft - Design Ready
**Database**: PostgreSQL 15+ (relational), Cassandra 4.0+ (spatial)
**Priority**: High
**Related**: [Int32 Implementation Specification](int32-coordinate-implementation-specification.md)

---

## Executive Summary

This document defines the database schema design for storing and querying Int32 (centimeter) coordinates in BlueMarble. The design optimizes for:

- **Exact precision**: INTEGER type provides exact 1cm representation
- **Fast queries**: Spatial indexes support >1M queries/second
- **Scalability**: Horizontal partitioning for petabyte-scale data
- **Integration**: Seamless with octree spatial indexing

**Storage**: Int32 coordinates use 12 bytes per position (vs 24 bytes for double), saving 50% space.

---

## Table of Contents

1. [PostgreSQL Schema](#postgresql-schema)
2. [Cassandra Schema](#cassandra-schema)
3. [Indexing Strategy](#indexing-strategy)
4. [Query Patterns](#query-patterns)
5. [Partitioning Strategy](#partitioning-strategy)
6. [Migration Plan](#migration-plan)
7. [Performance Optimization](#performance-optimization)

---

## PostgreSQL Schema

### Core Tables

#### spatial_entities Table

```sql
-- Primary table for entity positions
CREATE TABLE spatial_entities (
    -- Primary key
    entity_id BIGINT PRIMARY KEY,
    entity_type VARCHAR(50) NOT NULL,
    
    -- Int32 coordinates (centimeters)
    position_x INTEGER NOT NULL,
    position_y INTEGER NOT NULL,
    position_z INTEGER NOT NULL,
    
    -- Spatial indexing
    chunk_id BIGINT NOT NULL,
    region_id INTEGER NOT NULL,
    morton_code_high INTEGER NOT NULL,
    morton_code_mid INTEGER NOT NULL,
    morton_code_low INTEGER NOT NULL,
    
    -- Metadata
    last_updated TIMESTAMP NOT NULL DEFAULT NOW(),
    is_static BOOLEAN NOT NULL DEFAULT FALSE,
    
    -- Constraints
    CONSTRAINT valid_world_bounds CHECK (
        position_x BETWEEN -2000000000 AND 2000000000 AND
        position_y BETWEEN -2000000000 AND 2000000000 AND
        position_z BETWEEN -2000000000 AND 2000000000
    ),
    CONSTRAINT valid_chunk_id CHECK (chunk_id >= 0)
);

-- Comments
COMMENT ON TABLE spatial_entities IS 'Entity world positions stored as Int32 centimeters for exact precision';
COMMENT ON COLUMN spatial_entities.position_x IS 'X coordinate in centimeters (±21,474 km range)';
COMMENT ON COLUMN spatial_entities.position_y IS 'Y coordinate in centimeters (vertical/height)';
COMMENT ON COLUMN spatial_entities.position_z IS 'Z coordinate in centimeters';
COMMENT ON COLUMN spatial_entities.chunk_id IS 'Spatial chunk ID for fast regional queries';
COMMENT ON COLUMN spatial_entities.morton_code_high IS 'Morton code (Z-order) high 32 bits';
```

#### spatial_chunks Table

```sql
-- Chunk-based spatial partitioning
CREATE TABLE spatial_chunks (
    chunk_id BIGINT PRIMARY KEY,
    
    -- Chunk bounds (Int32 centimeters)
    min_x INTEGER NOT NULL,
    min_y INTEGER NOT NULL,
    min_z INTEGER NOT NULL,
    max_x INTEGER NOT NULL,
    max_y INTEGER NOT NULL,
    max_z INTEGER NOT NULL,
    
    -- Chunk metadata
    region_id INTEGER NOT NULL,
    entity_count INTEGER NOT NULL DEFAULT 0,
    is_active BOOLEAN NOT NULL DEFAULT TRUE,
    last_accessed TIMESTAMP NOT NULL DEFAULT NOW(),
    
    CONSTRAINT valid_chunk_bounds CHECK (
        min_x < max_x AND
        min_y < max_y AND
        min_z < max_z
    )
);

COMMENT ON TABLE spatial_chunks IS 'Spatial chunk boundaries for efficient regional queries';
```

#### spatial_regions Table

```sql
-- Regional grouping for distributed queries
CREATE TABLE spatial_regions (
    region_id INTEGER PRIMARY KEY,
    region_name VARCHAR(100) NOT NULL,
    
    -- Region bounds (Int32 centimeters)
    min_x INTEGER NOT NULL,
    min_y INTEGER NOT NULL,
    min_z INTEGER NOT NULL,
    max_x INTEGER NOT NULL,
    max_y INTEGER NOT NULL,
    max_z INTEGER NOT NULL,
    
    -- Server assignment for distributed system
    server_id INTEGER,
    
    CONSTRAINT valid_region_bounds CHECK (
        min_x < max_x AND
        min_y < max_y AND
        min_z < max_z
    )
);

COMMENT ON TABLE spatial_regions IS 'Large-scale regional divisions for server distribution';
```

### Indexes

```sql
-- Primary spatial index (chunk-based)
CREATE INDEX idx_spatial_entities_chunk 
ON spatial_entities(chunk_id) 
INCLUDE (position_x, position_y, position_z);

-- Region-based index for distributed queries
CREATE INDEX idx_spatial_entities_region 
ON spatial_entities(region_id);

-- Individual coordinate indexes for range queries
CREATE INDEX idx_spatial_entities_x ON spatial_entities(position_x);
CREATE INDEX idx_spatial_entities_y ON spatial_entities(position_y);
CREATE INDEX idx_spatial_entities_z ON spatial_entities(position_z);

-- Composite index for bounding box queries
CREATE INDEX idx_spatial_entities_xyz 
ON spatial_entities(position_x, position_y, position_z);

-- Morton code index for Z-order curve queries
CREATE INDEX idx_spatial_entities_morton 
ON spatial_entities(morton_code_high, morton_code_mid, morton_code_low);

-- Entity type filtering
CREATE INDEX idx_spatial_entities_type 
ON spatial_entities(entity_type);

-- Static vs dynamic entities
CREATE INDEX idx_spatial_entities_static 
ON spatial_entities(is_static) 
WHERE is_static = FALSE;

-- Recently updated entities
CREATE INDEX idx_spatial_entities_updated 
ON spatial_entities(last_updated DESC) 
WHERE last_updated > NOW() - INTERVAL '1 hour';
```

### Partitioning

```sql
-- Partition by region for horizontal scaling
CREATE TABLE spatial_entities_partitioned (
    LIKE spatial_entities INCLUDING ALL
) PARTITION BY RANGE (region_id);

-- Create partitions for different regions
CREATE TABLE spatial_entities_region_0 
PARTITION OF spatial_entities_partitioned 
FOR VALUES FROM (0) TO (100);

CREATE TABLE spatial_entities_region_100 
PARTITION OF spatial_entities_partitioned 
FOR VALUES FROM (100) TO (200);

-- Add more partitions as needed
```

---

## Cassandra Schema

### Keyspace Definition

```cql
-- Create keyspace for spatial data
CREATE KEYSPACE bluemarble_spatial
WITH replication = {
    'class': 'NetworkTopologyStrategy',
    'datacenter1': 3,
    'datacenter2': 3
}
AND durable_writes = true;

USE bluemarble_spatial;
```

### Entity Positions Table

```cql
-- Primary entity position storage
CREATE TABLE entity_positions (
    chunk_id bigint,
    entity_id bigint,
    
    -- Int32 coordinates (stored as int)
    position_x int,
    position_y int,
    position_z int,
    
    -- Morton encoding for Z-order
    morton_high int,
    morton_mid int,
    morton_low int,
    
    -- Entity metadata
    entity_type text,
    is_static boolean,
    last_updated timestamp,
    
    PRIMARY KEY ((chunk_id), entity_id)
) WITH CLUSTERING ORDER BY (entity_id ASC)
  AND compaction = {
      'class': 'LeveledCompactionStrategy',
      'sstable_size_in_mb': 160
  }
  AND compression = {
      'sstable_compression': 'LZ4Compressor',
      'chunk_length_kb': 64
  };

-- Comments
COMMENT ON TABLE entity_positions IS 
'Entity positions stored as Int32 centimeters, partitioned by chunk for efficient spatial queries';
```

### Spatial Index Table

```cql
-- Secondary index for spatial range queries
CREATE TABLE spatial_index (
    region_id int,
    x_bucket int,
    y_bucket int,
    z_bucket int,
    entity_id bigint,
    position_x int,
    position_y int,
    position_z int,
    
    PRIMARY KEY ((region_id, x_bucket, y_bucket, z_bucket), entity_id)
) WITH CLUSTERING ORDER BY (entity_id ASC);

COMMENT ON TABLE spatial_index IS 
'Bucketed spatial index for efficient range queries using Int32 coordinates';
```

### Morton Code Index

```cql
-- Z-order curve index for proximity queries
CREATE TABLE morton_index (
    region_id int,
    morton_high int,
    morton_mid int,
    morton_low int,
    entity_id bigint,
    position_x int,
    position_y int,
    position_z int,
    
    PRIMARY KEY ((region_id, morton_high), morton_mid, morton_low, entity_id)
) WITH CLUSTERING ORDER BY (morton_mid ASC, morton_low ASC, entity_id ASC);

COMMENT ON TABLE morton_index IS 
'Morton-encoded spatial index for efficient proximity queries';
```

---

## Indexing Strategy

### Chunk-Based Indexing

**Concept**: Divide world into fixed-size chunks (e.g., 10km × 10km × 10km).

```sql
-- Calculate chunk ID from coordinates
CREATE OR REPLACE FUNCTION calculate_chunk_id(
    x INTEGER,
    y INTEGER,
    z INTEGER
) RETURNS BIGINT AS $$
DECLARE
    chunk_size INTEGER := 1000000; -- 10 km = 1,000,000 cm
    chunk_x BIGINT;
    chunk_y BIGINT;
    chunk_z BIGINT;
BEGIN
    chunk_x := FLOOR(x::NUMERIC / chunk_size);
    chunk_y := FLOOR(y::NUMERIC / chunk_size);
    chunk_z := FLOOR(z::NUMERIC / chunk_size);
    
    -- Encode as single BIGINT (assumes reasonable chunk counts)
    RETURN (chunk_x + 2000) * 10000000000 + 
           (chunk_y + 2000) * 100000 + 
           (chunk_z + 2000);
END;
$$ LANGUAGE plpgsql IMMUTABLE;

-- Automatically update chunk_id on insert/update
CREATE OR REPLACE FUNCTION update_chunk_id()
RETURNS TRIGGER AS $$
BEGIN
    NEW.chunk_id := calculate_chunk_id(NEW.position_x, NEW.position_y, NEW.position_z);
    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER trigger_update_chunk_id
BEFORE INSERT OR UPDATE ON spatial_entities
FOR EACH ROW
EXECUTE FUNCTION update_chunk_id();
```

### Morton Code Generation

```sql
-- Function to calculate Morton code from coordinates
CREATE OR REPLACE FUNCTION calculate_morton_code(
    x INTEGER,
    y INTEGER,
    z INTEGER,
    OUT high INTEGER,
    OUT mid INTEGER,
    OUT low INTEGER
) AS $$
DECLARE
    -- Normalize to unsigned (shift by 2^31)
    ux BIGINT := x::BIGINT + 2147483648;
    uy BIGINT := y::BIGINT + 2147483648;
    uz BIGINT := z::BIGINT + 2147483648;
    code BIGINT := 0;
    i INTEGER;
BEGIN
    -- Interleave bits (simplified for first 21 bits)
    FOR i IN 0..20 LOOP
        code := code | ((ux & (1::BIGINT << i)) << (2 * i));
        code := code | ((uy & (1::BIGINT << i)) << (2 * i + 1));
        code := code | ((uz & (1::BIGINT << i)) << (2 * i + 2));
    END LOOP;
    
    -- Split into 3 parts
    high := (code >> 42)::INTEGER;
    mid := ((code >> 21) & 2097151)::INTEGER;
    low := (code & 2097151)::INTEGER;
END;
$$ LANGUAGE plpgsql IMMUTABLE;

-- Trigger to update Morton codes
CREATE TRIGGER trigger_update_morton
BEFORE INSERT OR UPDATE ON spatial_entities
FOR EACH ROW
EXECUTE FUNCTION (
    SELECT high, mid, low INTO NEW.morton_code_high, NEW.morton_code_mid, NEW.morton_code_low
    FROM calculate_morton_code(NEW.position_x, NEW.position_y, NEW.position_z)
);
```

---

## Query Patterns

### Spatial Range Queries

```sql
-- Find all entities within bounding box
PREPARE bbox_query (INTEGER, INTEGER, INTEGER, INTEGER, INTEGER, INTEGER) AS
SELECT entity_id, position_x, position_y, position_z
FROM spatial_entities
WHERE position_x BETWEEN $1 AND $2
  AND position_y BETWEEN $3 AND $4
  AND position_z BETWEEN $5 AND $6;

-- Execute
EXECUTE bbox_query(
    -50000,  -- min_x (500m west)
    50000,   -- max_x (500m east)
    0,       -- min_y (ground level)
    100000,  -- max_y (1km up)
    -50000,  -- min_z (500m south)
    50000    -- max_z (500m north)
);
```

### Chunk-Based Queries

```sql
-- Find all entities in a chunk (very fast)
PREPARE chunk_query (BIGINT) AS
SELECT entity_id, position_x, position_y, position_z
FROM spatial_entities
WHERE chunk_id = $1;

-- Find entities across multiple chunks
PREPARE multi_chunk_query (BIGINT[]) AS
SELECT entity_id, position_x, position_y, position_z
FROM spatial_entities
WHERE chunk_id = ANY($1);
```

### Radius Queries

```sql
-- Find entities within radius (using integer distance squared)
PREPARE radius_query (INTEGER, INTEGER, INTEGER, BIGINT) AS
SELECT 
    entity_id,
    position_x,
    position_y,
    position_z,
    (
        (position_x - $1)::BIGINT * (position_x - $1)::BIGINT +
        (position_y - $2)::BIGINT * (position_y - $2)::BIGINT +
        (position_z - $3)::BIGINT * (position_z - $3)::BIGINT
    ) AS distance_squared
FROM spatial_entities
WHERE 
    -- Bounding box pre-filter
    position_x BETWEEN $1 - SQRT($4) AND $1 + SQRT($4)
    AND position_y BETWEEN $2 - SQRT($4) AND $2 + SQRT($4)
    AND position_z BETWEEN $3 - SQRT($4) AND $3 + SQRT($4)
    AND (
        (position_x - $1)::BIGINT * (position_x - $1)::BIGINT +
        (position_y - $2)::BIGINT * (position_y - $2)::BIGINT +
        (position_z - $3)::BIGINT * (position_z - $3)::BIGINT
    ) <= $4
ORDER BY distance_squared
LIMIT 100;

-- Execute: Find entities within 100m (10,000 cm) radius
EXECUTE radius_query(
    0,          -- center_x
    0,          -- center_y
    0,          -- center_z
    100000000   -- radius_squared (10,000^2 = 100,000,000 cm²)
);
```

### Morton Code Proximity

```sql
-- Find entities with similar Morton codes (spatial proximity)
PREPARE morton_proximity (INTEGER, INTEGER, INTEGER, INTEGER) AS
SELECT entity_id, position_x, position_y, position_z
FROM spatial_entities
WHERE morton_code_high = $1
  AND morton_code_mid BETWEEN $2 - $4 AND $2 + $4
ORDER BY morton_code_mid, morton_code_low
LIMIT 100;
```

---

## Partitioning Strategy

### Horizontal Partitioning

```sql
-- Partition function based on region
CREATE OR REPLACE FUNCTION spatial_entities_partition()
RETURNS TRIGGER AS $$
DECLARE
    partition_name TEXT;
    region INT;
BEGIN
    -- Determine region from chunk_id
    region := (NEW.chunk_id / 1000000)::INTEGER;
    partition_name := 'spatial_entities_region_' || region;
    
    -- Route to appropriate partition
    EXECUTE format('INSERT INTO %I VALUES ($1.*)', partition_name) USING NEW;
    RETURN NULL;
END;
$$ LANGUAGE plpgsql;
```

### Time-Based Archiving

```sql
-- Archive old entity positions
CREATE TABLE spatial_entities_archive (
    LIKE spatial_entities INCLUDING ALL
) PARTITION BY RANGE (last_updated);

-- Create monthly archive partitions
CREATE TABLE spatial_entities_archive_2025_01
PARTITION OF spatial_entities_archive
FOR VALUES FROM ('2025-01-01') TO ('2025-02-01');
```

---

## Migration Plan

### Phase 1: Schema Creation

```sql
-- 1. Create new tables with Int32 coordinates
-- 2. Create indexes
-- 3. Create functions and triggers
-- 4. Test with sample data
```

### Phase 2: Data Migration

```sql
-- Convert existing float/double coordinates to Int32
CREATE OR REPLACE FUNCTION migrate_coordinates()
RETURNS void AS $$
BEGIN
    -- Assuming old table has REAL/DOUBLE coordinates
    INSERT INTO spatial_entities (
        entity_id,
        entity_type,
        position_x,
        position_y,
        position_z,
        is_static
    )
    SELECT
        entity_id,
        entity_type,
        ROUND(old_position_x * 100)::INTEGER,  -- meters to centimeters
        ROUND(old_position_y * 100)::INTEGER,
        ROUND(old_position_z * 100)::INTEGER,
        is_static
    FROM old_spatial_entities;
END;
$$ LANGUAGE plpgsql;

-- Execute migration
SELECT migrate_coordinates();
```

### Phase 3: Validation

```sql
-- Validate data integrity
SELECT COUNT(*) FROM spatial_entities WHERE
    position_x < -2000000000 OR position_x > 2000000000 OR
    position_y < -2000000000 OR position_y > 2000000000 OR
    position_z < -2000000000 OR position_z > 2000000000;

-- Should return 0

-- Validate chunk IDs
SELECT COUNT(*) FROM spatial_entities WHERE chunk_id IS NULL;
-- Should return 0
```

---

## Performance Optimization

### Query Performance

```sql
-- Analyze query performance
EXPLAIN (ANALYZE, BUFFERS) 
SELECT entity_id, position_x, position_y, position_z
FROM spatial_entities
WHERE chunk_id = 123456789;

-- Expected: Index Scan, <1ms execution time
```

### Index Maintenance

```sql
-- Reindex periodically for optimal performance
REINDEX INDEX CONCURRENTLY idx_spatial_entities_chunk;
REINDEX INDEX CONCURRENTLY idx_spatial_entities_morton;

-- Analyze tables for query planner
ANALYZE spatial_entities;
ANALYZE spatial_chunks;
```

### Statistics

```sql
-- Update statistics for better query planning
ALTER TABLE spatial_entities 
ALTER COLUMN position_x SET STATISTICS 1000;

ALTER TABLE spatial_entities 
ALTER COLUMN position_y SET STATISTICS 1000;

ALTER TABLE spatial_entities 
ALTER COLUMN position_z SET STATISTICS 1000;

-- Increase default statistics for chunk_id
ALTER TABLE spatial_entities 
ALTER COLUMN chunk_id SET STATISTICS 5000;
```

---

## Monitoring

### Query Performance Monitoring

```sql
-- Create monitoring view
CREATE OR REPLACE VIEW v_spatial_query_stats AS
SELECT
    query,
    calls,
    total_time,
    mean_time,
    max_time
FROM pg_stat_statements
WHERE query LIKE '%spatial_entities%'
ORDER BY mean_time DESC
LIMIT 20;

-- Check slow queries
SELECT * FROM v_spatial_query_stats WHERE mean_time > 10;
```

### Storage Monitoring

```sql
-- Monitor table sizes
SELECT
    schemaname,
    tablename,
    pg_size_pretty(pg_total_relation_size(schemaname||'.'||tablename)) AS size
FROM pg_tables
WHERE tablename LIKE 'spatial_%'
ORDER BY pg_total_relation_size(schemaname||'.'||tablename) DESC;
```

---

## Related Documents

- [Int32 Implementation Specification](int32-coordinate-implementation-specification.md) - Code implementation
- [ADR-001: Coordinate Data Type Selection](adr-001-coordinate-data-type-selection.md) - Architecture decision
- [Database Schema Design](../../docs/systems/database-schema-design.md) - Overall database architecture

---

## Success Metrics

### Performance Targets
- ✅ Spatial queries <10ms (chunk-based)
- ✅ Bounding box queries <50ms
- ✅ Radius queries <100ms
- ✅ Insert/update operations <5ms

### Storage Efficiency
- ✅ 50% reduction vs double coordinates
- ✅ Index size <20% of table size
- ✅ Compression ratio >60%

### Scalability
- ✅ Supports >1B entities
- ✅ Horizontal partitioning ready
- ✅ Linear scaling with partitions
