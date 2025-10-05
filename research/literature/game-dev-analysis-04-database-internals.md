# Database Internals - Analysis for BlueMarble MMORPG

---
title: Database Internals - Analysis for BlueMarble MMORPG
date: 2025-01-15
tags: [database, postgresql, persistence, scalability, mmorpg, optimization]
status: complete
priority: high
parent-research: game-development-resources-analysis.md
discovered-from: game-dev-analysis-02-game-engine-architecture.md
---

**Source:** Database Internals by Alex Petrov  
**Category:** Database Architecture - Persistence Systems  
**Priority:** High  
**Status:** ✅ Complete  
**Lines:** ~700  
**Related Sources:** Game Engine Architecture, Designing Data-Intensive Applications, PostgreSQL Documentation

---

## Executive Summary

This analysis examines "Database Internals" to understand database architecture patterns critical for BlueMarble's MMORPG persistence layer. The book provides comprehensive coverage of database storage engines, indexing structures, replication strategies, and distributed systems fundamentals essential for managing player data, world state, and geological simulation data at scale.

**Key Takeaways for BlueMarble:**
- PostgreSQL with PostGIS recommended for primary datastore (ACID + geospatial queries)
- B-Tree indexes for player lookups, R-Tree (GiST) indexes for spatial queries
- Database sharding by geographic region enables horizontal scaling
- Write-Ahead Logging (WAL) ensures durability for player transactions
- Read replicas distribute query load for non-critical reads
- Connection pooling critical for handling thousands of concurrent players

**Database Strategy:** PostgreSQL primary with geographic sharding, Redis cache layer, and async replication for read scaling.

---

## Part I: Storage Engine Fundamentals

### 1. Storage Structures and File Organization

**Page-Based Storage Architecture:**

Databases organize data into fixed-size pages (typically 8KB in PostgreSQL):

```
Disk Storage Hierarchy:
┌─────────────────────────────────────┐
│        Database Cluster             │
├─────────────────────────────────────┤
│  Database 1 │ Database 2 │ ...      │
├─────────────────────────────────────┤
│  Tables, Indexes, Sequences         │
├─────────────────────────────────────┤
│  Data Files (segmented)             │
├─────────────────────────────────────┤
│  Pages (8KB each)                   │
│  ├─ Header (24 bytes)               │
│  ├─ Item pointers                   │
│  ├─ Free space                      │
│  ├─ Tuples (rows)                   │
│  └─ Special space                   │
└─────────────────────────────────────┘
```

**PostgreSQL Page Structure:**

```cpp
// Simplified PostgreSQL page structure
struct PageHeader {
    uint32_t pd_lsn_high;      // Log sequence number (high 32 bits)
    uint32_t pd_lsn_low;       // Log sequence number (low 32 bits)
    uint16_t pd_checksum;      // Page checksum
    uint16_t pd_flags;         // Flag bits
    uint16_t pd_lower;         // Offset to start of free space
    uint16_t pd_upper;         // Offset to end of free space
    uint16_t pd_special;       // Offset to special space
    uint16_t pd_pagesize_version;  // Page size and version
    uint32_t pd_prune_xid;     // Oldest unpruned XMAX on page
};

struct ItemPointer {
    uint16_t offset;    // Offset to tuple on page
    uint16_t flags;     // Status flags (unused, dead, redirect, normal)
};

// Page layout
struct Page {
    PageHeader header;
    ItemPointer items[MAX_ITEMS];  // Item pointer array
    uint8_t freeSpace[...];         // Free space
    uint8_t tupleData[...];         // Actual tuple data (bottom-up)
    uint8_t specialSpace[...];      // Index-specific data
};
```

**BlueMarble Table Design:**

```sql
-- Player data table
CREATE TABLE players (
    player_id BIGSERIAL PRIMARY KEY,
    username VARCHAR(50) UNIQUE NOT NULL,
    email VARCHAR(255) UNIQUE NOT NULL,
    password_hash BYTEA NOT NULL,
    created_at TIMESTAMP WITH TIME ZONE DEFAULT NOW(),
    last_login TIMESTAMP WITH TIME ZONE,
    
    -- Gameplay data
    experience_points BIGINT DEFAULT 0,
    level INTEGER DEFAULT 1,
    
    -- Spatial data
    position GEOGRAPHY(POINT, 4326),  -- WGS84 coordinates
    current_region_id INTEGER,
    
    -- JSON for flexible data
    inventory JSONB DEFAULT '[]',
    skills JSONB DEFAULT '{}',
    
    -- Indexes created separately
    CONSTRAINT valid_level CHECK (level >= 1 AND level <= 100)
);

-- World regions table
CREATE TABLE world_regions (
    region_id SERIAL PRIMARY KEY,
    region_name VARCHAR(100) NOT NULL,
    bounds GEOGRAPHY(POLYGON, 4326),  -- Region boundaries
    
    -- Geological data
    terrain_type VARCHAR(50),
    average_elevation REAL,
    
    -- Resource spawns (denormalized for performance)
    resource_spawns JSONB DEFAULT '[]',
    
    -- Metadata
    created_at TIMESTAMP WITH TIME ZONE DEFAULT NOW(),
    last_updated TIMESTAMP WITH TIME ZONE DEFAULT NOW()
);

-- Player inventory items (normalized)
CREATE TABLE player_inventory (
    inventory_id BIGSERIAL PRIMARY KEY,
    player_id BIGINT NOT NULL REFERENCES players(player_id) ON DELETE CASCADE,
    item_id INTEGER NOT NULL,
    quantity INTEGER NOT NULL CHECK (quantity > 0),
    quality REAL CHECK (quality >= 0 AND quality <= 100),
    
    -- Unique constraint: player can't have duplicate items
    UNIQUE(player_id, item_id)
);

-- Crafting recipes
CREATE TABLE crafting_recipes (
    recipe_id SERIAL PRIMARY KEY,
    recipe_name VARCHAR(100) NOT NULL,
    required_items JSONB NOT NULL,  -- [{item_id: 1, quantity: 5}, ...]
    output_item_id INTEGER NOT NULL,
    output_quantity INTEGER NOT NULL,
    crafting_time_seconds INTEGER NOT NULL,
    required_skill_level INTEGER DEFAULT 1
);
```

**Storage Optimization Strategies:**

```cpp
// Connection pooling for MMORPG workloads
class DatabaseConnectionPool {
    struct Connection {
        PGconn* conn;
        bool inUse;
        std::chrono::steady_clock::time_point lastUsed;
    };
    
    std::vector<Connection> connections;
    std::mutex mutex;
    std::condition_variable cv;
    
    static constexpr size_t POOL_SIZE = 100;  // Adjust based on load
    static constexpr auto IDLE_TIMEOUT = std::chrono::minutes(5);
    
public:
    DatabaseConnectionPool() {
        for (size_t i = 0; i < POOL_SIZE; ++i) {
            connections.push_back({
                PQconnectdb("host=localhost dbname=bluemarble user=game"),
                false,
                std::chrono::steady_clock::now()
            });
        }
    }
    
    PGconn* Acquire() {
        std::unique_lock<std::mutex> lock(mutex);
        
        // Wait for available connection
        cv.wait(lock, [this] {
            return std::any_of(connections.begin(), connections.end(),
                             [](const Connection& c) { return !c.inUse; });
        });
        
        // Find free connection
        for (auto& conn : connections) {
            if (!conn.inUse) {
                conn.inUse = true;
                conn.lastUsed = std::chrono::steady_clock::now();
                return conn.conn;
            }
        }
        
        return nullptr;  // Should never reach here
    }
    
    void Release(PGconn* conn) {
        std::lock_guard<std::mutex> lock(mutex);
        
        for (auto& c : connections) {
            if (c.conn == conn) {
                c.inUse = false;
                c.lastUsed = std::chrono::steady_clock::now();
                cv.notify_one();
                return;
            }
        }
    }
    
    void PruneIdleConnections() {
        std::lock_guard<std::mutex> lock(mutex);
        auto now = std::chrono::steady_clock::now();
        
        for (auto& conn : connections) {
            if (!conn.inUse && 
                now - conn.lastUsed > IDLE_TIMEOUT) {
                // Reconnect to avoid connection timeout
                PQreset(conn.conn);
            }
        }
    }
};
```

---

## Part II: Indexing Structures

### 2. B-Tree Indexes for Player Lookups

**B-Tree Structure:**

B-Trees are the default index structure in PostgreSQL, optimized for range queries and equality searches:

```
B-Tree Structure (order 4):
                    [P|Q|R]
                   /   |   \   \
              [K|L] [M|N|O] [S|T|U] [V|W|X]
               / \   / | \   / | \   / | \
           [I][J][K][L][M][N][O][P][Q][R][S][T][U][V][W][X]
           
Each node contains:
- Keys (sorted)
- Child pointers (for internal nodes)
- Data pointers (for leaf nodes, in B+Tree)
```

**Creating Indexes for BlueMarble:**

```sql
-- Primary key automatically creates B-Tree index
CREATE INDEX idx_players_username ON players(username);
CREATE INDEX idx_players_email ON players(email);

-- Composite index for common queries
CREATE INDEX idx_players_level_xp ON players(level, experience_points);

-- Partial index (only index active players)
CREATE INDEX idx_players_active ON players(player_id) 
WHERE last_login > NOW() - INTERVAL '30 days';

-- Index on JSONB field
CREATE INDEX idx_players_inventory ON players USING GIN(inventory);

-- Spatial index (R-Tree via GiST)
CREATE INDEX idx_players_position ON players USING GIST(position);
CREATE INDEX idx_regions_bounds ON world_regions USING GIST(bounds);

-- Expression index
CREATE INDEX idx_players_username_lower ON players(LOWER(username));
```

**Index Usage Analysis:**

```cpp
// Server-side query optimization
class PlayerDatabase {
    DatabaseConnectionPool* pool;
    
public:
    // Optimized player lookup (uses idx_players_username)
    Player* GetPlayerByUsername(const std::string& username) {
        PGconn* conn = pool->Acquire();
        
        // Prepared statement for safety and performance
        const char* query = 
            "SELECT player_id, username, position, inventory, skills "
            "FROM players "
            "WHERE username = $1";
        
        const char* params[] = { username.c_str() };
        
        PGresult* result = PQexecParams(conn, query, 1, nullptr, params,
                                       nullptr, nullptr, 0);
        
        if (PQresultStatus(result) != PGRES_TUPLES_OK) {
            LOG_ERROR("Query failed: %s", PQerrorMessage(conn));
            PQclear(result);
            pool->Release(conn);
            return nullptr;
        }
        
        if (PQntuples(result) == 0) {
            PQclear(result);
            pool->Release(conn);
            return nullptr;  // Player not found
        }
        
        // Parse result into Player object
        Player* player = ParsePlayerFromResult(result, 0);
        
        PQclear(result);
        pool->Release(conn);
        
        return player;
    }
    
    // Spatial query (uses idx_players_position)
    std::vector<Player*> GetPlayersNearPosition(
        double latitude, double longitude, double radiusMeters) {
        
        PGconn* conn = pool->Acquire();
        
        // Use ST_DWithin for efficient spatial query
        const char* query = 
            "SELECT player_id, username, position "
            "FROM players "
            "WHERE ST_DWithin("
            "    position::geography, "
            "    ST_MakePoint($1, $2)::geography, "
            "    $3"
            ") "
            "ORDER BY ST_Distance(position::geography, "
            "                     ST_MakePoint($1, $2)::geography) "
            "LIMIT 100";
        
        char lonStr[32], latStr[32], radiusStr[32];
        snprintf(lonStr, sizeof(lonStr), "%.6f", longitude);
        snprintf(latStr, sizeof(latStr), "%.6f", latitude);
        snprintf(radiusStr, sizeof(radiusStr), "%.1f", radiusMeters);
        
        const char* params[] = { lonStr, latStr, radiusStr };
        
        PGresult* result = PQexecParams(conn, query, 3, nullptr, params,
                                       nullptr, nullptr, 0);
        
        std::vector<Player*> players;
        
        if (PQresultStatus(result) == PGRES_TUPLES_OK) {
            int nRows = PQntuples(result);
            for (int i = 0; i < nRows; ++i) {
                players.push_back(ParsePlayerFromResult(result, i));
            }
        }
        
        PQclear(result);
        pool->Release(conn);
        
        return players;
    }
};
```

---

## Part III: Transaction Processing

### 3. ACID Guarantees and Concurrency Control

**MVCC (Multi-Version Concurrency Control):**

PostgreSQL uses MVCC to allow concurrent reads and writes without locking:

```
Transaction Timeline:
T1: BEGIN -> SELECT -> UPDATE -> COMMIT
T2:            BEGIN -> SELECT -> COMMIT
T3:                      BEGIN -> SELECT -> UPDATE -> COMMIT

Each transaction sees a consistent snapshot:
- T1 sees data as of T1 start
- T2 sees data as of T2 start (may not see T1's changes)
- T3 sees data as of T3 start (sees T1's commit, not T2's)
```

**Transaction Isolation Levels:**

```sql
-- Read Committed (PostgreSQL default)
BEGIN TRANSACTION ISOLATION LEVEL READ COMMITTED;
-- Sees committed changes from other transactions during execution

-- Repeatable Read
BEGIN TRANSACTION ISOLATION LEVEL REPEATABLE READ;
-- Sees snapshot as of transaction start only

-- Serializable (strictest)
BEGIN TRANSACTION ISOLATION LEVEL SERIALIZABLE;
-- Enforces serial execution semantics
```

**Implementing Player Inventory Transactions:**

```cpp
// Safe inventory transaction (ACID compliant)
class InventoryTransactionManager {
public:
    bool TransferItem(uint64_t fromPlayerId, uint64_t toPlayerId,
                     uint32_t itemId, uint32_t quantity) {
        
        PGconn* conn = pool->Acquire();
        
        // Begin transaction
        PGresult* result = PQexec(conn, "BEGIN");
        if (PQresultStatus(result) != PGRES_COMMAND_OK) {
            PQclear(result);
            pool->Release(conn);
            return false;
        }
        PQclear(result);
        
        // Check source player has item
        const char* checkQuery = 
            "SELECT quantity FROM player_inventory "
            "WHERE player_id = $1 AND item_id = $2 "
            "FOR UPDATE";  // Lock row for update
        
        char fromIdStr[32], itemIdStr[32];
        snprintf(fromIdStr, sizeof(fromIdStr), "%lu", fromPlayerId);
        snprintf(itemIdStr, sizeof(itemIdStr), "%u", itemId);
        
        const char* checkParams[] = { fromIdStr, itemIdStr };
        
        result = PQexecParams(conn, checkQuery, 2, nullptr, checkParams,
                             nullptr, nullptr, 0);
        
        if (PQresultStatus(result) != PGRES_TUPLES_OK || 
            PQntuples(result) == 0) {
            // Item not found or insufficient quantity
            PQclear(result);
            PQexec(conn, "ROLLBACK");
            pool->Release(conn);
            return false;
        }
        
        uint32_t availableQty = atoi(PQgetvalue(result, 0, 0));
        PQclear(result);
        
        if (availableQty < quantity) {
            PQexec(conn, "ROLLBACK");
            pool->Release(conn);
            return false;
        }
        
        // Remove item from source
        const char* removeQuery = 
            "UPDATE player_inventory "
            "SET quantity = quantity - $3 "
            "WHERE player_id = $1 AND item_id = $2";
        
        char qtyStr[32];
        snprintf(qtyStr, sizeof(qtyStr), "%u", quantity);
        
        const char* removeParams[] = { fromIdStr, itemIdStr, qtyStr };
        
        result = PQexecParams(conn, removeQuery, 3, nullptr, removeParams,
                             nullptr, nullptr, 0);
        
        if (PQresultStatus(result) != PGRES_COMMAND_OK) {
            PQclear(result);
            PQexec(conn, "ROLLBACK");
            pool->Release(conn);
            return false;
        }
        PQclear(result);
        
        // Add item to destination (with upsert)
        char toIdStr[32];
        snprintf(toIdStr, sizeof(toIdStr), "%lu", toPlayerId);
        
        const char* addQuery = 
            "INSERT INTO player_inventory (player_id, item_id, quantity, quality) "
            "VALUES ($1, $2, $3, 100) "
            "ON CONFLICT (player_id, item_id) "
            "DO UPDATE SET quantity = player_inventory.quantity + EXCLUDED.quantity";
        
        const char* addParams[] = { toIdStr, itemIdStr, qtyStr };
        
        result = PQexecParams(conn, addQuery, 3, nullptr, addParams,
                             nullptr, nullptr, 0);
        
        if (PQresultStatus(result) != PGRES_COMMAND_OK) {
            PQclear(result);
            PQexec(conn, "ROLLBACK");
            pool->Release(conn);
            return false;
        }
        PQclear(result);
        
        // Commit transaction
        result = PQexec(conn, "COMMIT");
        bool success = (PQresultStatus(result) == PGRES_COMMAND_OK);
        PQclear(result);
        
        pool->Release(conn);
        return success;
    }
};
```

**Deadlock Handling:**

```cpp
// Retry logic for deadlock recovery
bool ExecuteWithRetry(std::function<bool(PGconn*)> operation, 
                      int maxRetries = 3) {
    for (int attempt = 0; attempt < maxRetries; ++attempt) {
        PGconn* conn = pool->Acquire();
        bool success = operation(conn);
        
        if (success) {
            pool->Release(conn);
            return true;
        }
        
        // Check if error was deadlock
        const char* sqlState = PQresultErrorField(
            PQgetResult(conn), PG_DIAG_SQLSTATE);
        
        if (sqlState && strcmp(sqlState, "40P01") == 0) {
            // Deadlock detected, retry after backoff
            pool->Release(conn);
            std::this_thread::sleep_for(
                std::chrono::milliseconds(100 * (attempt + 1)));
            continue;
        }
        
        // Other error, don't retry
        pool->Release(conn);
        return false;
    }
    
    return false;  // Max retries exceeded
}
```

---

## Part IV: Replication and High Availability

### 4. Streaming Replication for Read Scaling

**PostgreSQL Replication Architecture:**

```
Primary Server (Read/Write)
    │
    ├─ WAL (Write-Ahead Log)
    │  └─ Streaming Replication
    │
    ├─ Replica 1 (Read-Only)
    │  └─ Serves SELECT queries
    │
    ├─ Replica 2 (Read-Only)
    │  └─ Serves SELECT queries
    │
    └─ Replica 3 (Read-Only, Async)
       └─ Analytics queries
```

**Configuring Replication:**

```bash
# Primary server postgresql.conf
wal_level = replica
max_wal_senders = 10
wal_keep_size = 1GB
synchronous_commit = on  # or 'remote_apply' for sync replication

# Replica server postgresql.conf
hot_standby = on
max_standby_streaming_delay = 30s

# Replica server recovery.conf (or postgresql.auto.conf in PG12+)
primary_conninfo = 'host=primary-server port=5432 user=replicator'
primary_slot_name = 'replica_1_slot'
```

**Query Routing for Read Scaling:**

```cpp
// Route queries to appropriate server
class QueryRouter {
    PGconn* primaryConn;
    std::vector<PGconn*> replicaConns;
    std::atomic<size_t> nextReplica{0};
    
public:
    PGconn* GetConnectionForQuery(const std::string& query) {
        // Detect query type
        std::string upperQuery = ToUpper(query);
        
        if (upperQuery.find("SELECT") == 0 && 
            upperQuery.find("FOR UPDATE") == std::string::npos) {
            // Read query, use replica
            return GetReplicaConnection();
        } else {
            // Write query or SELECT FOR UPDATE, use primary
            return primaryConn;
        }
    }
    
private:
    PGconn* GetReplicaConnection() {
        // Round-robin load balancing
        size_t index = nextReplica.fetch_add(1) % replicaConns.size();
        return replicaConns[index];
    }
};
```

---

## Part V: Database Sharding

### 5. Geographic Sharding for Horizontal Scaling

**Sharding Strategy for BlueMarble:**

```
Shard 1: North America (lat: 15-75, lon: -170 to -50)
├─ Players in region
├─ World data in region
└─ Regional economy data

Shard 2: Europe/Africa (lat: -35-75, lon: -25 to 60)
├─ Players in region
├─ World data in region
└─ Regional economy data

Shard 3: Asia/Oceania (lat: -50-75, lon: 60 to 180)
├─ Players in region
├─ World data in region
└─ Regional economy data

Global Shard: Cross-region data
├─ Player accounts (mapping to shard)
├─ Global market prices
├─ Cross-region trading
└─ Authentication
```

**Shard Routing Logic:**

```cpp
// Determine shard based on player position
class ShardRouter {
    struct ShardInfo {
        std::string host;
        int port;
        double minLat, maxLat;
        double minLon, maxLon;
    };
    
    std::vector<ShardInfo> shards;
    
public:
    ShardRouter() {
        shards = {
            {"shard1.db.bluemarble.com", 5432, 15, 75, -170, -50},  // NA
            {"shard2.db.bluemarble.com", 5432, -35, 75, -25, 60},   // EU/AF
            {"shard3.db.bluemarble.com", 5432, -50, 75, 60, 180},   // Asia
        };
    }
    
    PGconn* GetShardForPosition(double latitude, double longitude) {
        for (const auto& shard : shards) {
            if (latitude >= shard.minLat && latitude <= shard.maxLat &&
                longitude >= shard.minLon && longitude <= shard.maxLon) {
                
                // Connect to appropriate shard
                char connStr[256];
                snprintf(connStr, sizeof(connStr),
                        "host=%s port=%d dbname=bluemarble",
                        shard.host.c_str(), shard.port);
                
                return PQconnectdb(connStr);
            }
        }
        
        // Default to global shard
        return PQconnectdb("host=global.db.bluemarble.com dbname=bluemarble");
    }
    
    // Cross-shard query (expensive, avoid if possible)
    std::vector<Player*> GetPlayersGlobal(const std::string& query) {
        std::vector<Player*> allPlayers;
        
        // Query each shard in parallel
        std::vector<std::future<std::vector<Player*>>> futures;
        
        for (const auto& shard : shards) {
            futures.push_back(std::async(std::launch::async, 
                [&shard, &query]() {
                    PGconn* conn = ConnectToShard(shard);
                    auto players = ExecuteQuery(conn, query);
                    PQfinish(conn);
                    return players;
                }));
        }
        
        // Collect results
        for (auto& future : futures) {
            auto players = future.get();
            allPlayers.insert(allPlayers.end(), players.begin(), players.end());
        }
        
        return allPlayers;
    }
};
```

**Player Migration Between Shards:**

```cpp
// Handle player crossing shard boundaries
class PlayerMigrationManager {
public:
    bool MigratePlayer(uint64_t playerId, 
                      PGconn* sourceShard, 
                      PGconn* destShard) {
        
        // Step 1: Begin transaction on both shards
        PQexec(sourceShard, "BEGIN");
        PQexec(destShard, "BEGIN");
        
        // Step 2: Lock player on source
        const char* lockQuery = 
            "SELECT * FROM players WHERE player_id = $1 FOR UPDATE";
        
        char playerIdStr[32];
        snprintf(playerIdStr, sizeof(playerIdStr), "%lu", playerId);
        const char* params[] = { playerIdStr };
        
        PGresult* result = PQexecParams(sourceShard, lockQuery, 1, 
                                       nullptr, params, nullptr, nullptr, 0);
        
        if (PQresultStatus(result) != PGRES_TUPLES_OK || 
            PQntuples(result) == 0) {
            PQclear(result);
            PQexec(sourceShard, "ROLLBACK");
            PQexec(destShard, "ROLLBACK");
            return false;
        }
        
        // Step 3: Copy player data to destination
        // (Using COPY or INSERT with all columns)
        std::string insertQuery = GenerateInsertFromResult(result);
        PQclear(result);
        
        result = PQexec(destShard, insertQuery.c_str());
        if (PQresultStatus(result) != PGRES_COMMAND_OK) {
            PQclear(result);
            PQexec(sourceShard, "ROLLBACK");
            PQexec(destShard, "ROLLBACK");
            return false;
        }
        PQclear(result);
        
        // Step 4: Delete from source
        const char* deleteQuery = 
            "DELETE FROM players WHERE player_id = $1";
        
        result = PQexecParams(sourceShard, deleteQuery, 1,
                             nullptr, params, nullptr, nullptr, 0);
        
        if (PQresultStatus(result) != PGRES_COMMAND_OK) {
            PQclear(result);
            PQexec(sourceShard, "ROLLBACK");
            PQexec(destShard, "ROLLBACK");
            return false;
        }
        PQclear(result);
        
        // Step 5: Commit both transactions
        PQexec(sourceShard, "COMMIT");
        PQexec(destShard, "COMMIT");
        
        return true;
    }
};
```

---

## Part VI: Caching Strategy

### 6. Redis Cache Layer for Hot Data

**Two-Tier Caching Architecture:**

```
Application Server
    ↓
Redis Cache (Hot data, <1ms latency)
    ↓ (on cache miss)
PostgreSQL (Cold data, ~5-50ms latency)
```

**Cache Implementation:**

```cpp
// Redis cache for player sessions
class PlayerCache {
    redisContext* redis;
    DatabaseConnectionPool* dbPool;
    
    static constexpr int CACHE_TTL = 3600;  // 1 hour
    
public:
    Player* GetPlayer(uint64_t playerId) {
        char key[64];
        snprintf(key, sizeof(key), "player:%lu", playerId);
        
        // Try cache first
        redisReply* reply = (redisReply*)redisCommand(redis, "GET %s", key);
        
        if (reply && reply->type == REDIS_REPLY_STRING) {
            // Cache hit, deserialize
            Player* player = DeserializePlayer(reply->str, reply->len);
            freeReplyObject(reply);
            return player;
        }
        
        freeReplyObject(reply);
        
        // Cache miss, query database
        PGconn* conn = dbPool->Acquire();
        Player* player = QueryPlayerFromDB(conn, playerId);
        dbPool->Release(conn);
        
        if (player) {
            // Store in cache
            std::string serialized = SerializePlayer(player);
            redisCommand(redis, "SETEX %s %d %b", 
                        key, CACHE_TTL, 
                        serialized.data(), serialized.size());
        }
        
        return player;
    }
    
    void InvalidatePlayer(uint64_t playerId) {
        char key[64];
        snprintf(key, sizeof(key), "player:%lu", playerId);
        redisCommand(redis, "DEL %s", key);
    }
    
    void UpdatePlayerPosition(uint64_t playerId, 
                             double lat, double lon) {
        // Update cache
        char key[64];
        snprintf(key, sizeof(key), "player:%lu:position", playerId);
        
        redisCommand(redis, "GEOADD player_positions %f %f %lu",
                    lon, lat, playerId);
        
        // Async update to database (batched)
        QueuePositionUpdate(playerId, lat, lon);
    }
    
    std::vector<uint64_t> GetPlayersNearby(double lat, double lon, 
                                           double radiusMeters) {
        // Use Redis geospatial queries for fast proximity search
        redisReply* reply = (redisReply*)redisCommand(redis,
            "GEORADIUS player_positions %f %f %f m",
            lon, lat, radiusMeters);
        
        std::vector<uint64_t> playerIds;
        
        if (reply && reply->type == REDIS_REPLY_ARRAY) {
            for (size_t i = 0; i < reply->elements; ++i) {
                playerIds.push_back(atoll(reply->element[i]->str));
            }
        }
        
        freeReplyObject(reply);
        return playerIds;
    }
};
```

---

## Part VII: Performance Optimization

### 7. Query Optimization and Monitoring

**EXPLAIN ANALYZE for Query Tuning:**

```sql
-- Analyze query performance
EXPLAIN (ANALYZE, BUFFERS) 
SELECT p.player_id, p.username, p.position
FROM players p
WHERE ST_DWithin(p.position::geography, 
                 ST_MakePoint(-122.4194, 37.7749)::geography,
                 1000)
ORDER BY ST_Distance(p.position::geography,
                     ST_MakePoint(-122.4194, 37.7749)::geography)
LIMIT 100;

-- Output shows:
-- - Execution time
-- - Index usage
-- - Rows scanned
-- - Buffer usage
```

**Vacuum and Analyze:**

```sql
-- Regular maintenance for MVCC cleanup
VACUUM ANALYZE players;

-- Autovacuum configuration (postgresql.conf)
autovacuum = on
autovacuum_max_workers = 3
autovacuum_naptime = 30s
autovacuum_vacuum_scale_factor = 0.1
autovacuum_analyze_scale_factor = 0.05
```

**Monitoring Critical Metrics:**

```cpp
// Database health monitoring
class DatabaseMonitor {
public:
    void CollectMetrics() {
        PGconn* conn = pool->Acquire();
        
        // Connection count
        auto connCount = QueryScalar(conn,
            "SELECT count(*) FROM pg_stat_activity");
        LOG_INFO("Active connections: %d", connCount);
        
        // Replication lag
        auto lagBytes = QueryScalar(conn,
            "SELECT pg_wal_lsn_diff(pg_current_wal_lsn(), replay_lsn) "
            "FROM pg_stat_replication");
        LOG_INFO("Replication lag: %ld bytes", lagBytes);
        
        // Cache hit ratio
        auto hitRatio = QueryScalar(conn,
            "SELECT round(100.0 * sum(blks_hit) / sum(blks_hit + blks_read), 2) "
            "FROM pg_stat_database WHERE datname = 'bluemarble'");
        LOG_INFO("Cache hit ratio: %.2f%%", hitRatio);
        
        // Long-running queries
        QueryAndLog(conn,
            "SELECT pid, now() - query_start as duration, query "
            "FROM pg_stat_activity "
            "WHERE state = 'active' AND now() - query_start > interval '5 seconds'");
        
        // Table bloat
        QueryAndLog(conn,
            "SELECT schemaname, tablename, "
            "pg_size_pretty(pg_total_relation_size(schemaname||'.'||tablename)) as size "
            "FROM pg_tables "
            "WHERE schemaname = 'public' "
            "ORDER BY pg_total_relation_size(schemaname||'.'||tablename) DESC "
            "LIMIT 10");
        
        pool->Release(conn);
    }
};
```

---

## Implications for BlueMarble

### Database Architecture Recommendations

**Recommended Database Stack:**

```
Layer 1: Application Servers (C++ game server)
    ↓
Layer 2: Redis Cache Cluster (hot player data)
    ├─ Player sessions
    ├─ Position data (geospatial)
    └─ Leaderboards
    ↓ (on cache miss)
Layer 3: PostgreSQL Primary (write operations)
    ├─ Player accounts
    ├─ Inventory/crafting
    ├─ World state (persistent)
    └─ Transaction logs
    ↓ (streaming replication)
Layer 4: PostgreSQL Replicas (read operations)
    ├─ Replica 1: General queries
    ├─ Replica 2: Geospatial queries
    └─ Replica 3: Analytics (async)
```

**Sharding Plan:**

**Phase 1: Single Database (Alpha, <1000 players)**
- Single PostgreSQL instance
- Redis cache for sessions
- Establish schema and optimize queries

**Phase 2: Read Replicas (Beta, <10K players)**
- Add 2-3 read replicas
- Route read queries to replicas
- Monitor replication lag

**Phase 3: Geographic Sharding (Launch, <100K players)**
- Shard by continent (NA, EU, Asia)
- Global shard for cross-region data
- Player migration on region change

**Phase 4: Fine-Grained Sharding (Scale, 100K+ players)**
- Shard by smaller regions (states/countries)
- Automated rebalancing
- Multi-region replication

**Performance Targets:**

- Query latency: <10ms for cached reads, <50ms for DB reads
- Write throughput: 10K+ transactions/second
- Connection pool: 100-500 connections per server
- Cache hit rate: >95% for player lookups
- Replication lag: <1 second for sync replicas

---

## References

### Books

1. Petrov, A. (2019). *Database Internals*. O'Reilly Media.
   - Comprehensive coverage of database architecture
   
2. Kleppmann, M. (2017). *Designing Data-Intensive Applications*. O'Reilly Media.
   - Distributed systems and data architecture patterns
   
3. PostgreSQL Official Documentation. *PostgreSQL 15 Documentation*.
   - Complete reference for PostgreSQL features

### Papers

1. Stonebraker, M., et al. (2007). "The End of an Architectural Era"
2. DeCandia, G., et al. (2007). "Dynamo: Amazon's Highly Available Key-value Store"
3. Chang, F., et al. (2008). "Bigtable: A Distributed Storage System"

### Online Resources

1. PostgreSQL Wiki - <https://wiki.postgresql.org/>
2. Use The Index, Luke - <https://use-the-index-luke.com/> - SQL indexing guide
3. PostgreSQL Performance Blog - <https://www.cybertec-postgresql.com/en/blog/>

---

## Related Research

### Within BlueMarble Repository

- [game-dev-analysis-02-game-engine-architecture.md](game-dev-analysis-02-game-engine-architecture.md) - Engine architecture fundamentals
- [game-dev-analysis-unity-overview.md](game-dev-analysis-unity-overview.md) - Unity engine evaluation

### Next Research Steps

- **Network Programming for Games** - Multiplayer networking patterns
- **Unreal Engine Documentation** - Alternative engine comparison

---

## Discovered Sources

During this research, the following sources were identified for future investigation:

1. **PostgreSQL High Performance** by Gregory Smith
   - PostgreSQL tuning and optimization techniques
   - Priority: Medium | Estimated Effort: 6-8 hours

2. **Redis in Action** by Josiah Carlson
   - Advanced Redis caching patterns and data structures
   - Priority: Medium | Estimated Effort: 5-6 hours

3. **Cassandra: The Definitive Guide** by Jeff Carpenter
   - Alternative distributed database for high write throughput
   - Priority: Low | Estimated Effort: 6-8 hours

These sources have been logged in the research-assignment-group-16.md file for future research phases.

---

**Document Status:** Complete  
**Discovered From:** Game Engine Architecture analysis (Topic 16 follow-up)  
**Last Updated:** 2025-01-15  
**Next Steps:** Network Programming for Games analysis for multiplayer networking

**Implementation Priority:** Critical - Database architecture must be established early as it impacts all persistent game systems. Begin with single PostgreSQL instance during prototyping, plan for sharding before beta launch.
