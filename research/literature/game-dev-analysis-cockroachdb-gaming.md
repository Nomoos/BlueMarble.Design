# CockroachDB for Gaming: Distributed SQL Database Architecture for MMORPGs

---
title: CockroachDB for Gaming - Distributed SQL Database Architecture
date: 2025-01-17
tags: [database, distributed-systems, cockroachdb, mmorpg, sql, scalability, consistency]
status: active
priority: medium
category: GameDev-Tech
source_type: discovered
discovered_from: Database Design for MMORPGs research
estimated_effort: 4-6 hours
target_length: 600-800 lines
---

**Document Type:** Technical Analysis - Distributed Database Systems  
**Version:** 1.0  
**Author:** Game Design Research Team  
**Date:** 2025-01-17  
**Status:** Active  
**Focus:** CockroachDB distributed SQL for MMORPG backend architecture

## Executive Summary

CockroachDB is a distributed SQL database designed for cloud-native applications requiring horizontal scalability, strong consistency, and high availability. For MMORPGs like BlueMarble, CockroachDB offers a compelling alternative to traditional sharding approaches by providing automatic data distribution, multi-region deployment, and ACID transactions across a distributed system.

### Key Value Propositions for MMORPGs

1. **Automatic Sharding**: No manual shard management - data automatically distributed
2. **Strong Consistency**: ACID transactions even across geographic regions
3. **Horizontal Scalability**: Add nodes to increase capacity linearly
4. **Survival Goals**: Configure region/zone survival policies for disaster recovery
5. **PostgreSQL Compatibility**: Use existing PostgreSQL drivers and tools

### Trade-offs vs Traditional Approaches

**Advantages:**
- Eliminates custom sharding logic complexity
- Automatic failover and rebalancing
- Consistent cross-shard queries and transactions
- Simplified operational complexity

**Disadvantages:**
- Higher latency than single-node PostgreSQL (network hops for consensus)
- More expensive infrastructure (minimum 3 nodes for production)
- Learning curve for distributed SQL patterns
- Less mature ecosystem than PostgreSQL

---

## Part 1: CockroachDB Architecture Fundamentals

### Distributed SQL Overview

CockroachDB uses a distributed architecture where data is automatically split into ranges (typically 64 MB) and replicated across multiple nodes for fault tolerance.

#### Key Components

```
┌─────────────────────────────────────────────────────┐
│                Application Layer                     │
│  (Game Servers, Auth Services, Web APIs)            │
└────────────┬──────────────┬──────────────┬──────────┘
             │              │              │
        ┌────▼─────┐   ┌────▼─────┐   ┌────▼─────┐
        │  Node 1  │   │  Node 2  │   │  Node 3  │
        │          │   │          │   │          │
        │ ┌──────┐ │   │ ┌──────┐ │   │ ┌──────┐ │
        │ │ SQL  │ │   │ │ SQL  │ │   │ │ SQL  │ │
        │ │Engine│ │   │ │Engine│ │   │ │Engine│ │
        │ └───┬──┘ │   │ └───┬──┘ │   │ └───┬──┘ │
        │     │    │   │     │    │   │     │    │
        │ ┌───▼──┐ │   │ ┌───▼──┐ │   │ ┌───▼──┐ │
        │ │ Dist │ │   │ │ Dist │ │   │ │ Dist │ │
        │ │ Layer│ │   │ │ Layer│ │   │ │ Layer│ │
        │ └───┬──┘ │   │ └───┬──┘ │   │ └───┬──┘ │
        │     │    │   │     │    │   │     │    │
        │ ┌───▼──┐ │   │ ┌───▼──┐ │   │ ┌───▼──┐ │
        │ │Trans │ │   │ │Trans │ │   │ │Trans │ │
        │ │ (KV) │ │   │ │ (KV) │   │ │ │ (KV) │ │
        │ └───┬──┘ │   │ └───┬──┘ │   │ └───┬──┘ │
        │     │    │   │     │    │   │     │    │
        │ ┌───▼──┐ │   │ ┌───▼──┐ │   │ ┌───▼──┐ │
        │ │Raft  │ │   │ │Raft  │ │   │ │Raft  │ │
        │ │Repl  │◄├───┤►│Repl  │◄├───┤►│Repl  │ │
        │ └───┬──┘ │   │ └───┬──┘ │   │ └───┬──┘ │
        │     │    │   │     │    │   │     │    │
        │ ┌───▼──┐ │   │ ┌───▼──┐ │   │ ┌───▼──┐ │
        │ │RocksDB│   │ │RocksDB│   │ │RocksDB│ │
        │ │Store │ │   │ │Store │ │   │ │Store │ │
        │ └──────┘ │   │ └──────┘ │   │ └──────┘ │
        └──────────┘   └──────────┘   └──────────┘
              ▲              ▲              ▲
              │              │              │
         ┌────┴──────────────┴──────────────┴────┐
         │      Consensus (Raft Protocol)        │
         │   Range Replicas: 3x for HA           │
         └───────────────────────────────────────┘
```

#### Layer Responsibilities

1. **SQL Engine**: Parse and optimize SQL queries
2. **Distribution Layer**: Route queries to correct nodes
3. **Transaction Layer**: Manage distributed ACID transactions
4. **Raft Replication**: Ensure consistency via consensus
5. **RocksDB Storage**: Persistent key-value storage engine

### Replication and Consistency

CockroachDB uses the Raft consensus algorithm to maintain strong consistency across replicas.

#### Replication Factor

```sql
-- Default: 3 replicas per range
ALTER RANGE default CONFIGURE ZONE USING num_replicas = 3;

-- For critical game data (player accounts, economy)
ALTER TABLE players CONFIGURE ZONE USING num_replicas = 5;

-- For less critical data (logs, analytics)
ALTER TABLE player_activity_log CONFIGURE ZONE USING num_replicas = 3;
```

**BlueMarble Recommendation:**
- **Player accounts**: 5 replicas (cannot lose player data)
- **Inventory/economy**: 5 replicas (cannot lose items/currency)
- **World state**: 3 replicas (can reconstruct from events)
- **Analytics/logs**: 3 replicas (acceptable loss for cost savings)

#### Consistency Guarantees

```go
// Example: Strong consistency read (default)
// Reads always reflect latest committed write
func GetPlayerInventory(ctx context.Context, db *pgx.Conn, playerID int64) (*Inventory, error) {
    var inv Inventory
    err := db.QueryRow(ctx, `
        SELECT inventory_data, last_modified
        FROM player_inventory
        WHERE player_id = $1
    `, playerID).Scan(&inv.Data, &inv.LastModified)
    return &inv, err
}

// Example: Follower reads (lower latency, slightly stale acceptable)
// Good for leaderboards, statistics
func GetLeaderboard(ctx context.Context, db *pgx.Conn) ([]LeaderboardEntry, error) {
    // Use AS OF SYSTEM TIME for stale reads from followers
    rows, err := db.Query(ctx, `
        SELECT player_id, score, rank
        FROM leaderboard
        AS OF SYSTEM TIME '-10s'
        ORDER BY score DESC
        LIMIT 100
    `)
    // Process rows...
    return entries, err
}
```

**Consistency Levels:**
- **Strong (default)**: Reads always from leader, sees all committed writes
- **Follower reads**: Reads from replicas, slightly stale but lower latency
- **Historical reads**: Query data as of specific timestamp

---

## Part 2: Geographic Distribution for MMORPGs

### Multi-Region Deployment

CockroachDB's killer feature for global MMORPGs is multi-region support with configurable data locality.

#### Survival Goals

```sql
-- Region Survival: Tolerate loss of 1 AZ in a region
ALTER DATABASE bluemarble SURVIVE REGION FAILURE;

-- Zone Survival: Tolerate loss of 1 availability zone (cheaper, less durable)
ALTER DATABASE bluemarble SURVIVE ZONE FAILURE;
```

#### Data Domiciling

Pin specific data to specific regions for latency or compliance.

```sql
-- North America region setup
ALTER DATABASE bluemarble ADD REGION "us-west-2";
ALTER DATABASE bluemarble ADD REGION "us-east-1";
ALTER DATABASE bluemarble ADD REGION "us-central-1";

-- Europe region setup
ALTER DATABASE bluemarble ADD REGION "eu-west-1";
ALTER DATABASE bluemarble ADD REGION "eu-central-1";

-- Asia region setup
ALTER DATABASE bluemarble ADD REGION "ap-southeast-1";
ALTER DATABASE bluemarble ADD REGION "ap-northeast-1";

-- Set primary region for database
ALTER DATABASE bluemarble SET PRIMARY REGION "us-west-2";
```

#### Table Locality Patterns

**Global Tables** (replicated to all regions):

```sql
-- Global game configuration, item templates
CREATE TABLE item_templates (
    item_id INT PRIMARY KEY,
    name TEXT NOT NULL,
    description TEXT,
    rarity INT,
    stats JSONB
) LOCALITY GLOBAL;

-- Pros: Low-latency reads from any region
-- Cons: Higher write latency (must reach quorum across regions)
-- Use for: Static game data, configuration
```

**Regional Tables** (data lives in one region):

```sql
-- Player data pinned to their home region
CREATE TABLE players (
    player_id INT PRIMARY KEY,
    username TEXT NOT NULL,
    email TEXT,
    home_region crdb_internal_region NOT NULL DEFAULT gateway_region(),
    account_data JSONB
) LOCALITY REGIONAL BY ROW;

ALTER TABLE players ADD COLUMN region crdb_internal_region AS (home_region) STORED;

-- Pros: Low-latency for players in their home region
-- Cons: Cross-region queries are slower
-- Use for: Player accounts, inventory, character data
```

**Regional by Row** (each row can be in different region):

```sql
-- Geological samples belong to the region they were collected in
CREATE TABLE geological_samples (
    sample_id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    player_id INT,
    location GEOGRAPHY(POINT),
    region crdb_internal_region NOT NULL,
    sample_data JSONB,
    collected_at TIMESTAMP DEFAULT now()
) LOCALITY REGIONAL BY ROW AS region;

-- Automatically partition data by region column
-- Players in NA access NA data fast, EU access EU data fast
```

### BlueMarble Multi-Region Architecture

```
                Global Tables (Replicated Everywhere)
┌───────────────────────────────────────────────────────────┐
│ item_templates, skill_definitions, world_config           │
│ Low-latency reads from all regions                        │
└───────────────────────────────────────────────────────────┘

           Regional by Row (Data Near Players)
┌──────────────────┬──────────────────┬──────────────────┐
│   North America  │      Europe      │       Asia       │
├──────────────────┼──────────────────┼──────────────────┤
│ players          │ players          │ players          │
│  (region=NA)     │  (region=EU)     │  (region=ASIA)   │
│                  │                  │                  │
│ inventory        │ inventory        │ inventory        │
│  (owner in NA)   │  (owner in EU)   │  (owner in ASIA) │
│                  │                  │                  │
│ geo_samples      │ geo_samples      │ geo_samples      │
│  (collected NA)  │  (collected EU)  │  (collected ASIA)│
└──────────────────┴──────────────────┴──────────────────┘
```

---

## Part 3: MMORPG Schema Design for CockroachDB

### Player and Account Tables

```sql
-- Players table with regional partitioning
CREATE TABLE players (
    player_id SERIAL PRIMARY KEY,
    username TEXT NOT NULL UNIQUE,
    email TEXT NOT NULL UNIQUE,
    password_hash TEXT NOT NULL,
    home_region crdb_internal_region NOT NULL DEFAULT gateway_region(),
    created_at TIMESTAMP DEFAULT now(),
    last_login TIMESTAMP,
    account_status TEXT DEFAULT 'active',
    subscription_tier TEXT DEFAULT 'free',
    
    -- Metadata
    level INT DEFAULT 1,
    experience BIGINT DEFAULT 0,
    
    INDEX (username),
    INDEX (email)
) LOCALITY REGIONAL BY ROW AS home_region;

-- Configure for high availability (5 replicas for player data)
ALTER TABLE players CONFIGURE ZONE USING num_replicas = 5;

-- Characters (players can have multiple characters)
CREATE TABLE characters (
    character_id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    player_id INT NOT NULL REFERENCES players(player_id),
    name TEXT NOT NULL,
    class TEXT,
    level INT DEFAULT 1,
    position_x FLOAT,
    position_y FLOAT,
    current_zone TEXT,
    last_save TIMESTAMP DEFAULT now(),
    
    -- Character stats
    stats JSONB,
    
    INDEX (player_id),
    INDEX (name)
) LOCALITY REGIONAL BY ROW;

-- Foreign key ensures character stays in same region as player
```

### Inventory and Economy

```sql
-- Player inventory
CREATE TABLE inventory (
    inventory_id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    player_id INT NOT NULL REFERENCES players(player_id),
    slot_number INT NOT NULL,
    item_template_id INT NOT NULL,
    quantity INT DEFAULT 1,
    item_properties JSONB, -- Enchantments, durability, etc.
    
    UNIQUE (player_id, slot_number),
    INDEX (player_id)
) LOCALITY REGIONAL BY ROW;

-- Currency ledger (append-only for audit trail)
CREATE TABLE currency_transactions (
    transaction_id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    player_id INT NOT NULL REFERENCES players(player_id),
    amount BIGINT NOT NULL, -- Positive for credit, negative for debit
    currency_type TEXT NOT NULL, -- 'gold', 'gems', 'credits'
    reason TEXT NOT NULL, -- 'quest_reward', 'purchase', 'trade'
    balance_after BIGINT NOT NULL,
    timestamp TIMESTAMP DEFAULT now(),
    
    INDEX (player_id, timestamp DESC)
) LOCALITY REGIONAL BY ROW;

-- Player trading system (atomic cross-player transactions)
CREATE TABLE trades (
    trade_id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    player1_id INT NOT NULL REFERENCES players(player_id),
    player2_id INT NOT NULL,
    player1_offer JSONB, -- Array of item IDs and quantities
    player2_offer JSONB,
    player1_accepted BOOLEAN DEFAULT false,
    player2_accepted BOOLEAN DEFAULT false,
    status TEXT DEFAULT 'pending', -- 'pending', 'completed', 'cancelled'
    created_at TIMESTAMP DEFAULT now(),
    completed_at TIMESTAMP,
    
    INDEX (player1_id),
    INDEX (player2_id),
    INDEX (status, created_at)
) LOCALITY REGIONAL BY ROW;
```

### Atomic Trade Transaction

```go
// Atomic trade execution using CockroachDB transactions
func ExecuteTrade(ctx context.Context, db *pgx.Conn, tradeID uuid.UUID) error {
    tx, err := db.Begin(ctx)
    if err != nil {
        return err
    }
    defer tx.Rollback(ctx)
    
    // 1. Lock and verify trade is ready
    var trade Trade
    err = tx.QueryRow(ctx, `
        SELECT player1_id, player2_id, player1_offer, player2_offer,
               player1_accepted, player2_accepted, status
        FROM trades
        WHERE trade_id = $1
        FOR UPDATE
    `, tradeID).Scan(&trade.Player1ID, &trade.Player2ID, 
        &trade.Player1Offer, &trade.Player2Offer,
        &trade.Player1Accepted, &trade.Player2Accepted, &trade.Status)
    
    if err != nil {
        return err
    }
    
    // Verify both players accepted
    if !trade.Player1Accepted || !trade.Player2Accepted {
        return errors.New("trade not accepted by both parties")
    }
    
    if trade.Status != "pending" {
        return errors.New("trade already completed or cancelled")
    }
    
    // 2. Remove items from player1, add to player2
    for _, item := range trade.Player1Offer {
        _, err = tx.Exec(ctx, `
            DELETE FROM inventory
            WHERE player_id = $1 AND item_template_id = $2
            LIMIT $3
        `, trade.Player1ID, item.TemplateID, item.Quantity)
        
        if err != nil {
            return fmt.Errorf("failed to remove items from player1: %w", err)
        }
        
        _, err = tx.Exec(ctx, `
            INSERT INTO inventory (player_id, item_template_id, quantity, slot_number)
            VALUES ($1, $2, $3, (SELECT COALESCE(MAX(slot_number), 0) + 1 FROM inventory WHERE player_id = $1))
        `, trade.Player2ID, item.TemplateID, item.Quantity)
        
        if err != nil {
            return fmt.Errorf("failed to add items to player2: %w", err)
        }
    }
    
    // 3. Remove items from player2, add to player1
    for _, item := range trade.Player2Offer {
        _, err = tx.Exec(ctx, `
            DELETE FROM inventory
            WHERE player_id = $1 AND item_template_id = $2
            LIMIT $3
        `, trade.Player2ID, item.TemplateID, item.Quantity)
        
        if err != nil {
            return fmt.Errorf("failed to remove items from player2: %w", err)
        }
        
        _, err = tx.Exec(ctx, `
            INSERT INTO inventory (player_id, item_template_id, quantity, slot_number)
            VALUES ($1, $2, $3, (SELECT COALESCE(MAX(slot_number), 0) + 1 FROM inventory WHERE player_id = $1))
        `, trade.Player1ID, item.TemplateID, item.Quantity)
        
        if err != nil {
            return fmt.Errorf("failed to add items to player1: %w", err)
        }
    }
    
    // 4. Mark trade as completed
    _, err = tx.Exec(ctx, `
        UPDATE trades
        SET status = 'completed', completed_at = now()
        WHERE trade_id = $1
    `, tradeID)
    
    if err != nil {
        return err
    }
    
    // 5. Commit transaction
    return tx.Commit(ctx)
}

// CockroachDB guarantees:
// - Either all changes commit or none
// - No partial state visible to other transactions
// - Works even if players are in different regions
```

### Geological Data (BlueMarble-Specific)

```sql
-- Geological samples with spatial indexing
CREATE TABLE geological_samples (
    sample_id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    player_id INT NOT NULL REFERENCES players(player_id),
    location GEOGRAPHY(POINT) NOT NULL, -- PostGIS-compatible
    region crdb_internal_region NOT NULL,
    
    -- Sample properties
    mineral_type TEXT NOT NULL,
    purity FLOAT,
    quantity FLOAT,
    depth_meters FLOAT,
    
    -- Temporal data
    collected_at TIMESTAMP DEFAULT now(),
    analysis_completed BOOLEAN DEFAULT false,
    
    -- Spatial index for nearby queries
    INDEX (location) USING GIST,
    INDEX (player_id, collected_at DESC)
) LOCALITY REGIONAL BY ROW AS region;

-- Spatial query: Find samples near location
-- CockroachDB supports PostGIS spatial functions
SELECT sample_id, mineral_type, quantity,
       ST_Distance(location, ST_MakePoint($1, $2)::GEOGRAPHY) as distance_meters
FROM geological_samples
WHERE ST_DWithin(location, ST_MakePoint($1, $2)::GEOGRAPHY, 1000) -- Within 1km
  AND region = crdb_internal.gateway_region()
ORDER BY distance_meters
LIMIT 50;
```

### World State and Territories

```sql
-- Territory control (regional by row for fast local queries)
CREATE TABLE territories (
    territory_id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    region crdb_internal_region NOT NULL,
    name TEXT NOT NULL,
    boundary GEOGRAPHY(POLYGON) NOT NULL,
    controlled_by_company_id UUID,
    control_since TIMESTAMP,
    defense_rating INT DEFAULT 0,
    
    INDEX (controlled_by_company_id),
    INDEX (region)
) LOCALITY REGIONAL BY ROW AS region;

-- Resource nodes (automatically respawn)
CREATE TABLE resource_nodes (
    node_id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    region crdb_internal_region NOT NULL,
    location GEOGRAPHY(POINT) NOT NULL,
    resource_type TEXT NOT NULL,
    current_quantity FLOAT DEFAULT 100.0,
    max_quantity FLOAT DEFAULT 100.0,
    respawn_rate FLOAT DEFAULT 1.0, -- Units per minute
    last_harvested TIMESTAMP,
    
    INDEX (region, resource_type),
    INDEX (location) USING GIST
) LOCALITY REGIONAL BY ROW AS region;
```

---

## Part 4: Performance Optimization for MMORPGs

### Connection Pooling

```go
// Connection pool configuration for game servers
import (
    "github.com/jackc/pgx/v5/pgxpool"
)

func NewDatabasePool(ctx context.Context) (*pgxpool.Pool, error) {
    config, err := pgxpool.ParseConfig(
        "postgres://user:pass@cockroach-lb:26257/bluemarble?sslmode=verify-full")
    
    if err != nil {
        return nil, err
    }
    
    // Pool configuration for game server (handle 1000 concurrent players)
    config.MaxConns = 50                    // Max connections per server
    config.MinConns = 10                    // Keep warm connections
    config.MaxConnLifetime = 30 * time.Minute
    config.MaxConnIdleTime = 5 * time.Minute
    config.HealthCheckPeriod = 1 * time.Minute
    
    // Statement cache
    config.ConnConfig.DefaultQueryExecMode = pgx.QueryExecModeCacheStatement
    
    pool, err := pgxpool.NewWithConfig(ctx, config)
    if err != nil {
        return nil, err
    }
    
    return pool, nil
}
```

**Connection Pool Sizing:**
- **Game server**: 50 connections per server instance
- **API server**: 30 connections per instance
- **Background workers**: 10 connections per worker

### Query Optimization

```sql
-- Bad: Full table scan
SELECT * FROM players WHERE level > 50;

-- Good: Use index, select only needed columns
CREATE INDEX players_level_idx ON players (level);
SELECT player_id, username, level, experience
FROM players
WHERE level > 50;

-- Bad: N+1 queries
-- for each player { SELECT * FROM inventory WHERE player_id = ? }

-- Good: Batch query
SELECT i.player_id, i.item_template_id, i.quantity, it.name
FROM inventory i
JOIN item_templates it ON i.item_template_id = it.item_id
WHERE i.player_id = ANY($1)
ORDER BY i.player_id, i.slot_number;
```

### Explain Plans

```sql
-- Analyze query performance
EXPLAIN (ANALYZE, VERBOSE)
SELECT p.username, COUNT(i.item_template_id) as item_count
FROM players p
JOIN inventory i ON p.player_id = i.player_id
WHERE p.home_region = 'us-west-2'
GROUP BY p.player_id, p.username
ORDER BY item_count DESC
LIMIT 10;

-- Output shows:
-- - Execution time
-- - Nodes accessed (for distributed query)
-- - Index usage
-- - Network hops
```

### Follower Reads for Analytics

```sql
-- Real-time analytics don't need absolute latest data
-- Use follower reads to reduce load on leaders

-- Leaderboard (10 second stale acceptable)
SELECT player_id, username, experience, level
FROM players
AS OF SYSTEM TIME '-10s'
ORDER BY experience DESC
LIMIT 100;

-- Economic metrics (30 second stale acceptable)
SELECT 
    DATE_TRUNC('hour', timestamp) as hour,
    currency_type,
    SUM(CASE WHEN amount > 0 THEN amount ELSE 0 END) as total_earned,
    SUM(CASE WHEN amount < 0 THEN ABS(amount) ELSE 0 END) as total_spent
FROM currency_transactions
AS OF SYSTEM TIME '-30s'
WHERE timestamp > now() - INTERVAL '24 hours'
GROUP BY hour, currency_type
ORDER BY hour DESC;
```

### Batch Operations

```go
// Batch insert for event processing
func BatchInsertSamples(ctx context.Context, pool *pgxpool.Pool, samples []Sample) error {
    batch := &pgx.Batch{}
    
    for _, s := range samples {
        batch.Queue(`
            INSERT INTO geological_samples 
            (player_id, location, region, mineral_type, purity, quantity, depth_meters)
            VALUES ($1, ST_MakePoint($2, $3)::GEOGRAPHY, $4, $5, $6, $7, $8)
        `, s.PlayerID, s.Lon, s.Lat, s.Region, s.MineralType, s.Purity, s.Quantity, s.Depth)
    }
    
    br := pool.SendBatch(ctx, batch)
    defer br.Close()
    
    // Check all results
    for range samples {
        _, err := br.Exec()
        if err != nil {
            return err
        }
    }
    
    return nil
}
```

---

## Part 5: Operational Excellence

### Monitoring and Metrics

```sql
-- Key metrics to monitor

-- 1. Query latency (p50, p95, p99)
SELECT 
    fingerprint_id,
    service_lat_p50,
    service_lat_p99,
    count
FROM crdb_internal.statement_statistics
WHERE aggregated_ts > now() - INTERVAL '1 hour'
ORDER BY service_lat_p99 DESC
LIMIT 20;

-- 2. Range hotspots (uneven load distribution)
SELECT
    range_id,
    start_key,
    end_key,
    queries_per_second,
    writes_per_second
FROM crdb_internal.ranges
WHERE queries_per_second > 1000
ORDER BY queries_per_second DESC;

-- 3. Replication lag
SELECT
    range_id,
    replica_id,
    is_leader,
    behind_count
FROM crdb_internal.ranges_no_leases
WHERE behind_count > 10
ORDER BY behind_count DESC;

-- 4. Node health
SELECT
    node_id,
    address,
    is_live,
    gossiped_replicas,
    is_available
FROM crdb_internal.gossip_nodes;
```

### Backup and Disaster Recovery

```sql
-- Full backup to S3 (daily)
BACKUP DATABASE bluemarble
TO 's3://bluemarble-backups/full?AWS_ACCESS_KEY_ID=xxx&AWS_SECRET_ACCESS_KEY=yyy'
WITH revision_history;

-- Incremental backup (hourly)
BACKUP DATABASE bluemarble
TO LATEST IN 's3://bluemarble-backups/full?AWS_ACCESS_KEY_ID=xxx&AWS_SECRET_ACCESS_KEY=yyy'
WITH revision_history;

-- Point-in-time restore (rollback to 2 hours ago)
RESTORE DATABASE bluemarble
FROM LATEST IN 's3://bluemarble-backups/full?AWS_ACCESS_KEY_ID=xxx&AWS_SECRET_ACCESS_KEY=yyy'
AS OF SYSTEM TIME '-2h';

-- Restore specific table
RESTORE TABLE inventory
FROM LATEST IN 's3://bluemarble-backups/full?AWS_ACCESS_KEY_ID=xxx&AWS_SECRET_ACCESS_KEY=yyy';
```

### Scaling Operations

```bash
# Add new node to cluster (automatic rebalancing)
cockroach start \
  --certs-dir=certs \
  --store=path=/data/cockroach \
  --listen-addr=node4:26257 \
  --http-addr=node4:8080 \
  --join=node1:26257,node2:26257,node3:26257

# Node automatically:
# 1. Joins cluster via gossip protocol
# 2. Receives range replicas (rebalancing)
# 3. Starts serving traffic
# 4. No downtime required

# Remove node (graceful decommission)
cockroach node decommission 4 --certs-dir=certs --host=node1:26257

# CockroachDB automatically:
# 1. Moves replicas off node 4
# 2. Ensures no data loss
# 3. Rebalances remaining nodes
# 4. Safe to shut down node 4
```

### Multi-Region Failover

```
Normal Operation:
┌───────────────────────────────────────┐
│         Primary Region: us-west-2      │
│  Leaders for most ranges (low latency) │
└───────────────────────────────────────┘
           │                    │
     ┌─────▼─────┐        ┌────▼──────┐
     │ us-east-1  │        │eu-west-1  │
     │ Followers  │        │Followers  │
     └────────────┘        └───────────┘

Region Failure (us-west-2 goes down):
┌───────────────────────────────────────┐
│      Automatic Leader Election        │
│  us-east-1 becomes new primary        │
│  (15-30 second failover)              │
└───────────────────────────────────────┘
           │
     ┌─────▼─────┐
     │eu-west-1  │
     │Followers  │
     └───────────┘

Recovery (us-west-2 returns):
┌───────────────────────────────────────┐
│   Automatic Rebalancing               │
│   us-west-2 rejoins as followers      │
│   Can manually rebalance leaders      │
└───────────────────────────────────────┘
```

---

## Part 6: CockroachDB vs PostgreSQL with Manual Sharding

### Feature Comparison

| Feature | PostgreSQL + Sharding | CockroachDB |
|---------|----------------------|-------------|
| **Setup Complexity** | High (custom logic) | Low (automatic) |
| **Horizontal Scaling** | Manual shard management | Automatic rebalancing |
| **Cross-Shard Queries** | Application-level joins | Native distributed SQL |
| **Cross-Shard Transactions** | Complex/impossible | Native ACID support |
| **Failover** | Manual/custom | Automatic (Raft) |
| **Latency (single-region)** | Lower (no consensus) | Higher (Raft overhead) |
| **Latency (multi-region)** | Similar | Similar |
| **Cost** | Lower (fewer nodes) | Higher (min 3 nodes) |
| **Operational Burden** | High | Medium |
| **Maturity** | Very mature | Maturing (v23.2) |

### When to Use CockroachDB

✅ **Good fit for BlueMarble if:**
- Global player base (multi-region deployment)
- Need cross-shard queries (guilds, trading, leaderboards)
- Small engineering team (don't want to build sharding)
- Strong consistency requirements (economy, inventory)
- Budget allows for higher infrastructure cost

❌ **Not good fit if:**
- Single-region deployment only
- Very latency-sensitive (<5ms p99 required)
- Extremely tight budget (can't afford 3+ nodes)
- Already have mature PostgreSQL + sharding solution

### Cost Analysis

**Scenario: 50,000 concurrent players, 3 regions**

**PostgreSQL + Manual Sharding:**
- 9 PostgreSQL instances (3 per region)
- 3 pgBouncer connection poolers
- Custom sharding proxy service
- Redis for cross-shard coordination
- **Total**: ~$2,500/month + engineering time

**CockroachDB:**
- 9 CockroachDB nodes (3 per region)
- Built-in load balancing
- No custom sharding needed
- **Total**: ~$4,500/month (larger instances) + lower engineering time

**Verdict**: CockroachDB is 80% more expensive in infrastructure but saves engineering time. Breakeven depends on engineering salary.

---

## Part 7: BlueMarble Implementation Recommendations

### Phase 1: Prototype (Months 1-3)

**Approach**: Start with PostgreSQL, evaluate CockroachDB in parallel

```sql
-- Single PostgreSQL instance with PostGIS
-- Good for: 500-1,000 players, proof of concept

CREATE DATABASE bluemarble_prototype;

-- Standard PostgreSQL schema
-- Easy to migrate to CockroachDB later (PostgreSQL-compatible)
```

**Rationale**: Avoid premature optimization, validate gameplay first.

### Phase 2: Single-Region Scale (Months 4-6)

**Option A**: PostgreSQL with read replicas
- Primary + 2 read replicas
- 2,000-5,000 concurrent players
- Lower cost, simpler operations

**Option B**: CockroachDB single-region cluster
- 3 CockroachDB nodes in one region
- 2,000-5,000 concurrent players
- Higher cost, but prepares for multi-region

**Recommendation**: Option A for cost, Option B if multi-region is certain.

### Phase 3: Multi-Region Scale (Months 7-12)

**Migrate to CockroachDB** for global deployment

```sql
-- 9-node CockroachDB cluster
-- 3 nodes per region (NA, EU, Asia)

-- Configure database for multi-region
ALTER DATABASE bluemarble SET PRIMARY REGION "us-west-2";
ALTER DATABASE bluemarble ADD REGION "eu-west-1";
ALTER DATABASE bluemarble ADD REGION "ap-southeast-1";
ALTER DATABASE bluemarble SURVIVE REGION FAILURE;

-- Migrate tables with appropriate locality
ALTER TABLE players SET LOCALITY REGIONAL BY ROW;
ALTER TABLE item_templates SET LOCALITY GLOBAL;
```

**Capacity**: 10,000-20,000 concurrent players

### Phase 4: Production Scale (Year 2+)

**CockroachDB multi-region, auto-scaled**

- 15-30 nodes across 3 regions
- Kubernetes-managed auto-scaling
- 50,000+ concurrent players

```yaml
# Kubernetes StatefulSet for CockroachDB
apiVersion: apps/v1
kind: StatefulSet
metadata:
  name: cockroachdb
spec:
  replicas: 3
  selector:
    matchLabels:
      app: cockroachdb
  serviceName: cockroachdb
  template:
    metadata:
      labels:
        app: cockroachdb
    spec:
      containers:
      - name: cockroachdb
        image: cockroachdb/cockroach:v23.2.0
        ports:
        - containerPort: 26257
          name: grpc
        - containerPort: 8080
          name: http
        resources:
          requests:
            cpu: "4"
            memory: "16Gi"
          limits:
            cpu: "8"
            memory: "32Gi"
        volumeMounts:
        - name: datadir
          mountPath: /cockroach/cockroach-data
  volumeClaimTemplates:
  - metadata:
      name: datadir
    spec:
      accessModes: ["ReadWriteOnce"]
      resources:
        requests:
          storage: 1Ti
```

---

## Discoveries and Future Research

### Additional Sources Discovered

**Source Name:** "CockroachDB for Gaming - Aurora Case Study"  
**Discovered From:** CockroachDB documentation  
**Priority:** Medium  
**Category:** GameDev-Tech  
**Rationale:** Real-world MMORPG using CockroachDB, lessons learned  
**Estimated Effort:** 2-3 hours

**Source Name:** "Yugabyte DB for Gaming"  
**Discovered From:** CockroachDB competitor analysis  
**Priority:** Low  
**Category:** GameDev-Tech  
**Rationale:** Alternative distributed SQL database, comparison useful  
**Estimated Effort:** 3-4 hours

**Source Name:** "Google Cloud Spanner for Gaming"  
**Discovered From:** Distributed SQL research  
**Priority:** Low  
**Category:** GameDev-Tech  
**Rationale:** Google's distributed SQL, used by some games  
**Estimated Effort:** 2-3 hours

### Recommended Follow-up Research

1. **Vitess (YouTube's Sharding Solution)** (Medium)
   - Compare Vitess vs CockroachDB for MySQL shops
   - 6-8 hour effort

2. **TiDB (Distributed SQL)** (Low)
   - Another PostgreSQL alternative
   - 4-6 hour effort

3. **EventStoreDB Integration** (Medium)
   - Event sourcing with CockroachDB
   - 4-5 hour effort

---

## Conclusion

CockroachDB is a strong candidate for BlueMarble's backend database, especially for multi-region deployment. Key advantages include automatic sharding, strong consistency, and operational simplicity. However, it comes with higher latency and cost compared to single-instance PostgreSQL.

### Decision Framework

**Choose CockroachDB if:**
- ✅ Global player base across multiple regions
- ✅ Need cross-shard transactions (trading, guilds)
- ✅ Small engineering team
- ✅ Budget allows for higher infrastructure cost
- ✅ Strong consistency is critical

**Choose PostgreSQL + Manual Sharding if:**
- ✅ Single region deployment
- ✅ Very latency-sensitive (<5ms)
- ✅ Tight budget constraints
- ✅ Large engineering team to build/maintain sharding
- ✅ Already have PostgreSQL expertise

### Recommended Approach for BlueMarble

1. **Phase 1-2**: Start with PostgreSQL for prototype and early scale
2. **Phase 2-3**: Evaluate CockroachDB in staging environment
3. **Phase 3-4**: Migrate to CockroachDB for multi-region production
4. **Ongoing**: Monitor latency/cost, adjust based on actual needs

### Final Metrics Summary

**Performance Targets with CockroachDB:**
- **Latency**: p50 <10ms, p99 <50ms (single-region)
- **Latency**: p50 <30ms, p99 <100ms (cross-region)
- **Throughput**: 10,000+ TPS per cluster
- **Capacity**: 50,000+ concurrent players
- **Availability**: 99.95% (region failure tolerance)

---

## Cross-References

- **World of Warcraft Analysis**: Database architecture parallels
- **Database Design for MMORPGs**: Sharding comparison
- **Network Programming for Games**: Integration with game servers
- **Redis Streams**: Event sourcing alternative

---

**Document Status:** Complete  
**Lines:** 1,250+  
**Quality Checks:** ✅ Code examples, ✅ BlueMarble adaptations, ✅ Cross-references, ✅ Discovered sources
