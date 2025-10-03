# MMO Architecture: Source Code and Insights - Analysis for BlueMarble MMORPG

---
title: MMO Architecture Source Code and Insights - Analysis for BlueMarble MMORPG
date: 2025-01-17
tags: [mmo, architecture, networking, scalability, server-design, open-source]
status: complete
priority: critical
parent-research: online-game-dev-resources.md
related-documents: [wow-emulator-architecture-networking.md, game-dev-analysis-01-game-programming-cpp.md]
---

**Source:** Open Source MMO Projects and Industry Implementations  
**Category:** MMORPG Architecture - Practical Implementation  
**Priority:** Critical  
**Status:** ✅ Complete  
**Lines:** 600+  
**Related Sources:** TrinityCore, CMaNGOS, AzerothCore, EVE Online Architecture, Ragnarok Online Architecture

---

## Executive Summary

This analysis examines real-world MMO architectures through open-source implementations and documented industry patterns. By studying battle-tested codebases from projects like TrinityCore (World of Warcraft emulator), EVE Online's single-shard architecture, and various other MMO server implementations, we extract practical patterns for building a planet-scale MMORPG like BlueMarble.

**Key Takeaways for BlueMarble:**
- Multi-tier architecture with separation between authentication, world simulation, and database layers
- Zone-based partitioning for horizontal scalability supporting 10,000+ concurrent players
- Interest management systems to reduce network bandwidth by 70-90%
- Event-driven messaging architecture for loose coupling between services
- Database sharding strategies for persistent world state across planetary scale
- Real-time state synchronization patterns for client-server consistency

---

## Part I: Core MMO Server Architecture Patterns

### 1. Multi-Tier Service Architecture

**The Standard MMO Server Stack:**

```
┌─────────────────────────────────────────────────────┐
│                 Load Balancer / Gateway             │
│              (Connection Distribution)              │
└─────────────────────────────────────────────────────┘
                         │
        ┌────────────────┼────────────────┐
        ▼                ▼                ▼
┌──────────────┐  ┌──────────────┐  ┌──────────────┐
│ Auth Server  │  │ Auth Server  │  │ Auth Server  │
│   (Replica)  │  │   (Replica)  │  │   (Replica)  │
└──────────────┘  └──────────────┘  └──────────────┘
        │                │                │
        └────────────────┼────────────────┘
                         ▼
        ┌─────────────────────────────────┐
        │      Realm/World Gateway        │
        │    (Session Management)         │
        └─────────────────────────────────┘
                         │
        ┌────────────────┼────────────────┐
        ▼                ▼                ▼
┌──────────────┐  ┌──────────────┐  ┌──────────────┐
│ World Server │  │ World Server │  │ World Server │
│   Zone 1-10  │  │  Zone 11-20  │  │  Zone 21-30  │
└──────────────┘  └──────────────┘  └──────────────┘
        │                │                │
        └────────────────┼────────────────┘
                         ▼
        ┌─────────────────────────────────┐
        │     Shared Database Cluster     │
        │  (Characters, World, Economy)   │
        └─────────────────────────────────┘
```

**Architecture Components from TrinityCore:**

```cpp
// Authentication Server (Realmd/Authserver)
class AuthServer {
private:
    uint16_t port = 3724;  // Standard Blizzard auth port
    boost::asio::io_service ioService;
    std::shared_ptr<AuthDatabase> authDB;
    
public:
    void Run() {
        // Accept incoming connections
        AcceptConnections();
        
        // Process authentication requests
        while (running) {
            ioService.run();
            ProcessAuthQueue();
            CleanupExpiredSessions();
        }
    }
    
    // SRP6 authentication handshake
    bool AuthenticateClient(AuthSession& session) {
        // 1. Client sends username
        // 2. Server sends salt + generator
        // 3. Client computes verifier
        // 4. Server verifies and creates session key
        
        std::string username = session.GetUsername();
        
        // Fetch account data
        auto account = authDB->GetAccount(username);
        if (!account) return false;
        
        // SRP6 challenge-response
        BigNumber salt = account->GetSalt();
        BigNumber verifier = account->GetVerifier();
        
        // Session key derived from shared secret
        BigNumber sessionKey = ComputeSRP6SessionKey(
            username, verifier, salt, 
            session.GetClientPublic()
        );
        
        session.SetSessionKey(sessionKey);
        return true;
    }
    
    // Realm list query
    void SendRealmList(AuthSession& session) {
        ByteBuffer packet;
        packet << uint32_t(0); // Unknown value
        packet << uint16_t(realmList.size());
        
        for (auto& realm : realmList) {
            packet << uint8_t(realm.icon);
            packet << uint8_t(realm.flags);
            packet << realm.name;
            packet << realm.address; // IP:Port
            packet << float(realm.population);
            packet << uint8_t(realm.characters);
            packet << uint8_t(realm.timezone);
            packet << uint8_t(realm.id);
        }
        
        session.SendPacket(REALM_LIST, packet);
    }
};

// World Server (Worldserver/Mangosd)
class WorldServer {
private:
    uint16_t port = 8085;  // Configurable per realm
    MapManager mapMgr;
    SessionManager sessionMgr;
    std::shared_ptr<WorldDatabase> worldDB;
    std::shared_ptr<CharacterDatabase> charDB;
    
    // Thread pool for parallel zone updates
    ThreadPool workerThreads;
    
public:
    void Run() {
        // Initialize world state
        LoadMaps();
        LoadSpawnData();
        LoadGameObjects();
        
        // Main server loop
        uint32_t prevTime = GetMSTime();
        uint32_t updateDiff = 0;
        
        while (running) {
            uint32_t currTime = GetMSTime();
            updateDiff = GetMSTimeDiff(prevTime, currTime);
            
            // Network I/O
            ProcessIncomingPackets();
            
            // World simulation update
            UpdateWorld(updateDiff);
            
            // Send state updates to clients
            BroadcastWorldState();
            
            // Database operations
            ProcessDatabaseQueue();
            
            prevTime = currTime;
            
            // Sleep to maintain target tick rate (50ms = 20 TPS)
            uint32_t executionTime = GetMSTimeDiff(currTime, GetMSTime());
            if (executionTime < 50) {
                std::this_thread::sleep_for(
                    std::chrono::milliseconds(50 - executionTime)
                );
            }
        }
    }
    
    void UpdateWorld(uint32_t diff) {
        // Update all active maps in parallel
        std::vector<std::future<void>> mapUpdates;
        
        for (auto& [mapId, map] : mapMgr.GetAllMaps()) {
            if (!map->HasPlayers()) continue;
            
            mapUpdates.push_back(
                workerThreads.Enqueue([&map, diff]() {
                    map->Update(diff);
                })
            );
        }
        
        // Wait for all map updates to complete
        for (auto& future : mapUpdates) {
            future.wait();
        }
    }
};
```

**BlueMarble Application:**
- **Gateway Service**: Load balancing and DDoS protection
- **Auth Service**: Account management, authentication, realm selection
- **World Servers**: Geographic zones (continents, regions) running independently
- **Database Cluster**: Sharded by player ID and geographic region
- **Message Queue**: Inter-server communication (RabbitMQ/Redis)

---

### 2. Zone-Based World Partitioning

**Horizontal Scaling Through Geographic Zones:**

```cpp
// Map/Zone management from TrinityCore
class Map {
private:
    uint32_t mapId;
    uint32_t instanceId;
    
    // Grid-based spatial partitioning
    static const uint32_t GRID_SIZE = 533.33333f; // yards
    static const uint32_t MAX_GRIDS_X = 64;
    static const uint32_t MAX_GRIDS_Y = 64;
    
    Grid grids[MAX_GRIDS_X][MAX_GRIDS_Y];
    
    // Active objects in this map
    std::unordered_map<ObjectGuid, WorldObject*> objects;
    std::unordered_map<ObjectGuid, Player*> players;
    std::unordered_map<ObjectGuid, Creature*> creatures;
    
public:
    // Load/unload grids based on player proximity
    void UpdateGridStates(uint32_t diff) {
        for (uint32_t x = 0; x < MAX_GRIDS_X; ++x) {
            for (uint32_t y = 0; y < MAX_GRIDS_Y; ++y) {
                Grid& grid = grids[x][y];
                
                if (grid.HasPlayers()) {
                    // Active grid - keep loaded and updated
                    grid.SetActive(true);
                    grid.Update(diff);
                    
                    // Ensure adjacent grids are loaded
                    EnsureGridLoaded(x-1, y);
                    EnsureGridLoaded(x+1, y);
                    EnsureGridLoaded(x, y-1);
                    EnsureGridLoaded(x, y+1);
                } else {
                    // Inactive grid - consider unloading
                    grid.IncrementIdleTime(diff);
                    
                    if (grid.GetIdleTime() > GRID_UNLOAD_DELAY) {
                        UnloadGrid(x, y);
                    }
                }
            }
        }
    }
    
    // Insert object into appropriate grid
    void AddObject(WorldObject* obj) {
        float x, y, z;
        obj->GetPosition(x, y, z);
        
        uint32_t gridX = ComputeGridX(x);
        uint32_t gridY = ComputeGridY(y);
        
        if (gridX < MAX_GRIDS_X && gridY < MAX_GRIDS_Y) {
            grids[gridX][gridY].AddObject(obj);
            objects[obj->GetGUID()] = obj;
        }
    }
    
    // Query nearby objects efficiently
    void GetNearbyObjects(const Position& pos, float radius, 
                          std::vector<WorldObject*>& results) {
        uint32_t centerX = ComputeGridX(pos.x);
        uint32_t centerY = ComputeGridY(pos.y);
        
        // Calculate grid radius to search
        uint32_t gridRadius = std::ceil(radius / GRID_SIZE) + 1;
        
        for (int32_t dx = -gridRadius; dx <= gridRadius; ++dx) {
            for (int32_t dy = -gridRadius; dy <= gridRadius; ++dy) {
                int32_t x = centerX + dx;
                int32_t y = centerY + dy;
                
                if (x >= 0 && x < MAX_GRIDS_X && 
                    y >= 0 && y < MAX_GRIDS_Y) {
                    grids[x][y].GetAllObjects(results);
                }
            }
        }
        
        // Filter by actual distance
        results.erase(
            std::remove_if(results.begin(), results.end(),
                [&pos, radius](WorldObject* obj) {
                    return !obj->IsWithinDist(&pos, radius);
                }),
            results.end()
        );
    }
};

// Instance management for dungeons/raids
class MapInstanced : public Map {
private:
    std::unordered_map<uint32_t, Map*> instances;
    
public:
    Map* CreateInstance(Player* player, uint32_t instanceId = 0) {
        // Check if player already has an instance
        if (instanceId == 0) {
            instanceId = player->GetGroup() 
                ? player->GetGroup()->GetInstanceId(mapId)
                : GenerateInstanceId();
        }
        
        // Create or retrieve existing instance
        auto it = instances.find(instanceId);
        if (it != instances.end()) {
            return it->second;
        }
        
        Map* instance = new Map(mapId, instanceId);
        instance->Initialize();
        instances[instanceId] = instance;
        
        return instance;
    }
    
    void DestroyInstance(uint32_t instanceId) {
        auto it = instances.find(instanceId);
        if (it == instances.end()) return;
        
        Map* instance = it->second;
        
        // Teleport all players out
        for (auto& [guid, player] : instance->GetPlayers()) {
            player->TeleportToHomebind();
        }
        
        // Clean up and delete
        instance->UnloadAll();
        delete instance;
        instances.erase(it);
    }
};
```

**BlueMarble Zone Architecture:**
- **Continental Zones**: Each continent is a separate world server
- **Regional Instances**: Cities and resource-rich areas as instanced zones
- **Cross-Zone Communication**: Message queue for player movement between zones
- **Load Balancing**: Migrate zones between servers based on player density

---

### 3. Interest Management and Network Optimization

**The Problem:** Broadcasting every entity update to every player creates O(n²) network traffic that doesn't scale.

**Solution: Area of Interest (AOI) System**

```cpp
// Interest management from EVE Online-inspired architecture
class InterestManager {
private:
    // Spatial hash for fast proximity queries
    SpatialHash<Entity*> spatialHash;
    
    // Each player tracks their "bubble" of interest
    struct InterestSet {
        std::unordered_set<EntityId> entities;
        Position center;
        float radius;
        uint32_t lastUpdate;
    };
    
    std::unordered_map<PlayerId, InterestSet> playerInterests;
    
public:
    // Update what entities are in range of each player
    void UpdateInterests(uint32_t currentTime) {
        for (auto& [playerId, interest] : playerInterests) {
            // Get entities within interest radius
            std::vector<Entity*> nearby;
            spatialHash.Query(interest.center, interest.radius, nearby);
            
            std::unordered_set<EntityId> newEntities;
            for (auto* entity : nearby) {
                newEntities.insert(entity->GetId());
            }
            
            // Find entities entering interest
            std::vector<EntityId> entering;
            std::set_difference(
                newEntities.begin(), newEntities.end(),
                interest.entities.begin(), interest.entities.end(),
                std::back_inserter(entering)
            );
            
            // Find entities leaving interest
            std::vector<EntityId> leaving;
            std::set_difference(
                interest.entities.begin(), interest.entities.end(),
                newEntities.begin(), newEntities.end(),
                std::back_inserter(leaving)
            );
            
            // Update player's interest set
            interest.entities = std::move(newEntities);
            interest.lastUpdate = currentTime;
            
            // Send appropriate messages to client
            for (auto entityId : entering) {
                SendEntityCreate(playerId, entityId);
            }
            
            for (auto entityId : leaving) {
                SendEntityDestroy(playerId, entityId);
            }
        }
    }
    
    // Broadcast entity update only to interested players
    void BroadcastEntityUpdate(EntityId entityId, const UpdateData& data) {
        auto* entity = GetEntity(entityId);
        if (!entity) return;
        
        Position pos = entity->GetPosition();
        
        // Find all players interested in this position
        std::vector<PlayerId> recipients;
        for (auto& [playerId, interest] : playerInterests) {
            if (interest.entities.count(entityId) > 0) {
                recipients.push_back(playerId);
            }
        }
        
        // Send update to relevant players only
        for (auto playerId : recipients) {
            SendEntityUpdate(playerId, entityId, data);
        }
    }
    
    // Priority-based update throttling
    void UpdateEntityPriorities() {
        for (auto& [playerId, interest] : playerInterests) {
            auto* player = GetPlayer(playerId);
            Position playerPos = player->GetPosition();
            
            for (auto entityId : interest.entities) {
                auto* entity = GetEntity(entityId);
                float distance = playerPos.Distance(entity->GetPosition());
                
                // Calculate update priority based on distance
                UpdatePriority priority;
                if (distance < 10.0f) {
                    priority = UpdatePriority::Critical;  // 20Hz
                } else if (distance < 50.0f) {
                    priority = UpdatePriority::High;      // 10Hz
                } else if (distance < 100.0f) {
                    priority = UpdatePriority::Medium;    // 5Hz
                } else {
                    priority = UpdatePriority::Low;       // 1Hz
                }
                
                entity->SetUpdatePriority(playerId, priority);
            }
        }
    }
};

// Network packet batching for efficiency
class PacketBatcher {
private:
    std::unordered_map<PlayerId, ByteBuffer> batches;
    uint32_t batchInterval = 50; // ms
    uint32_t lastFlush = 0;
    
public:
    void AddPacket(PlayerId playerId, const Packet& packet) {
        ByteBuffer& batch = batches[playerId];
        packet.Serialize(batch);
        
        // Flush if batch is getting large
        if (batch.Size() > MAX_PACKET_SIZE - 1024) {
            FlushBatch(playerId);
        }
    }
    
    void Update(uint32_t currentTime) {
        if (currentTime - lastFlush >= batchInterval) {
            FlushAllBatches();
            lastFlush = currentTime;
        }
    }
    
    void FlushBatch(PlayerId playerId) {
        auto it = batches.find(playerId);
        if (it == batches.end() || it->second.Empty()) return;
        
        // Send batched packet
        SendToClient(playerId, it->second);
        it->second.Clear();
    }
    
    void FlushAllBatches() {
        for (auto& [playerId, batch] : batches) {
            if (!batch.Empty()) {
                SendToClient(playerId, batch);
                batch.Clear();
            }
        }
    }
};
```

**BlueMarble Interest Management:**
- **Dynamic Radius**: Adjust based on player activity (combat vs. peaceful)
- **LOD System**: Reduce update frequency for distant objects
- **Culling**: Don't send updates for objects behind terrain/obstacles
- **Compression**: Delta encoding for position updates

**Network Bandwidth Savings:**
- Before interest management: 1000 players × 10,000 entities = 10M updates/sec
- After interest management: 1000 players × 100 entities = 100K updates/sec
- **99% reduction in network traffic**

---

## Part II: Database Architecture and Persistence

### 4. MMO Database Patterns

**Three-Database Architecture from WoW Emulators:**

```sql
-- AUTH DATABASE: Account and realm management
CREATE DATABASE auth;

USE auth;

-- Account credentials
CREATE TABLE account (
    id INT UNSIGNED PRIMARY KEY AUTO_INCREMENT,
    username VARCHAR(32) UNIQUE NOT NULL,
    salt BINARY(32) NOT NULL,
    verifier BINARY(128) NOT NULL,
    email VARCHAR(255),
    joindate TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    last_ip VARCHAR(15),
    last_login TIMESTAMP,
    online TINYINT UNSIGNED DEFAULT 0,
    failed_logins INT UNSIGNED DEFAULT 0,
    locked TINYINT UNSIGNED DEFAULT 0,
    lock_country VARCHAR(2),
    expansion TINYINT UNSIGNED DEFAULT 0,
    INDEX idx_username (username)
);

-- Session management
CREATE TABLE account_session (
    account_id INT UNSIGNED PRIMARY KEY,
    session_key BINARY(40),
    realm_id INT UNSIGNED,
    last_update TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    FOREIGN KEY (account_id) REFERENCES account(id) ON DELETE CASCADE
);

-- Realm list
CREATE TABLE realmlist (
    id INT UNSIGNED PRIMARY KEY AUTO_INCREMENT,
    name VARCHAR(32) UNIQUE NOT NULL,
    address VARCHAR(255) NOT NULL,
    port SMALLINT UNSIGNED DEFAULT 8085,
    icon TINYINT UNSIGNED,
    flags TINYINT UNSIGNED,
    timezone TINYINT UNSIGNED,
    allowedSecurityLevel TINYINT UNSIGNED DEFAULT 0,
    population FLOAT UNSIGNED DEFAULT 0,
    online INT UNSIGNED DEFAULT 0
);

-- CHARACTERS DATABASE: Player state
CREATE DATABASE characters;

USE characters;

-- Core player data
CREATE TABLE characters (
    guid INT UNSIGNED PRIMARY KEY AUTO_INCREMENT,
    account INT UNSIGNED NOT NULL,
    name VARCHAR(12) UNIQUE NOT NULL,
    race TINYINT UNSIGNED,
    class TINYINT UNSIGNED,
    gender TINYINT UNSIGNED,
    level TINYINT UNSIGNED DEFAULT 1,
    xp INT UNSIGNED DEFAULT 0,
    money INT UNSIGNED DEFAULT 0,
    online TINYINT UNSIGNED DEFAULT 0,
    totaltime INT UNSIGNED DEFAULT 0,
    leveltime INT UNSIGNED DEFAULT 0,
    logout_time TIMESTAMP,
    is_logout_resting TINYINT UNSIGNED DEFAULT 0,
    rest_bonus FLOAT DEFAULT 0,
    -- Position
    map SMALLINT UNSIGNED,
    instance_id INT UNSIGNED DEFAULT 0,
    position_x FLOAT,
    position_y FLOAT,
    position_z FLOAT,
    orientation FLOAT,
    -- Appearance
    skin TINYINT UNSIGNED,
    face TINYINT UNSIGNED,
    hairstyle TINYINT UNSIGNED,
    haircolor TINYINT UNSIGNED,
    facialstyle TINYINT UNSIGNED,
    -- Deletion
    deleteDate TIMESTAMP NULL,
    deleteInfos_Account INT UNSIGNED,
    INDEX idx_account (account),
    INDEX idx_name (name),
    INDEX idx_online (online)
);

-- Inventory
CREATE TABLE character_inventory (
    guid INT UNSIGNED NOT NULL,
    bag INT UNSIGNED DEFAULT 0,
    slot TINYINT UNSIGNED,
    item INT UNSIGNED NOT NULL,
    item_template INT UNSIGNED NOT NULL,
    PRIMARY KEY (guid, bag, slot),
    INDEX idx_item (item),
    FOREIGN KEY (guid) REFERENCES characters(guid) ON DELETE CASCADE
);

-- Item instances
CREATE TABLE item_instance (
    guid INT UNSIGNED PRIMARY KEY AUTO_INCREMENT,
    item_template INT UNSIGNED NOT NULL,
    owner_guid INT UNSIGNED NOT NULL,
    count INT UNSIGNED DEFAULT 1,
    charges TEXT,
    flags INT UNSIGNED DEFAULT 0,
    durability SMALLINT UNSIGNED,
    enchantments TEXT,
    randomPropertyId SMALLINT,
    INDEX idx_owner (owner_guid),
    FOREIGN KEY (owner_guid) REFERENCES characters(guid) ON DELETE CASCADE
);

-- WORLD DATABASE: Static game data
CREATE DATABASE world;

USE world;

-- Creature templates
CREATE TABLE creature_template (
    entry INT UNSIGNED PRIMARY KEY,
    name VARCHAR(100),
    subname VARCHAR(100),
    minlevel TINYINT UNSIGNED,
    maxlevel TINYINT UNSIGNED,
    faction INT UNSIGNED,
    npcflag INT UNSIGNED,
    speed_walk FLOAT,
    speed_run FLOAT,
    scale FLOAT DEFAULT 1,
    rank TINYINT UNSIGNED,
    dmg_multiplier FLOAT DEFAULT 1,
    baseattacktime INT UNSIGNED,
    rangeattacktime INT UNSIGNED,
    unit_class TINYINT UNSIGNED,
    unit_flags INT UNSIGNED,
    family TINYINT UNSIGNED,
    type TINYINT UNSIGNED,
    ai_name VARCHAR(64),
    movement_type TINYINT UNSIGNED,
    scriptname VARCHAR(64)
);

-- Creature spawns
CREATE TABLE creature (
    guid INT UNSIGNED PRIMARY KEY AUTO_INCREMENT,
    id INT UNSIGNED NOT NULL,
    map SMALLINT UNSIGNED,
    position_x FLOAT,
    position_y FLOAT,
    position_z FLOAT,
    orientation FLOAT,
    spawntimesecs INT UNSIGNED,
    spawndist FLOAT DEFAULT 0,
    currentwaypoint INT UNSIGNED DEFAULT 0,
    curhealth INT UNSIGNED DEFAULT 1,
    curmana INT UNSIGNED DEFAULT 0,
    movement_type TINYINT UNSIGNED DEFAULT 0,
    INDEX idx_map (map),
    INDEX idx_id (id),
    FOREIGN KEY (id) REFERENCES creature_template(entry)
);

-- Loot tables
CREATE TABLE creature_loot_template (
    entry INT UNSIGNED NOT NULL,
    item INT UNSIGNED NOT NULL,
    reference INT UNSIGNED DEFAULT 0,
    chance FLOAT DEFAULT 100,
    questreq TINYINT DEFAULT 0,
    lootmode SMALLINT UNSIGNED DEFAULT 1,
    groupid TINYINT UNSIGNED DEFAULT 0,
    mincount TINYINT UNSIGNED DEFAULT 1,
    maxcount TINYINT UNSIGNED DEFAULT 1,
    PRIMARY KEY (entry, item)
);
```

**BlueMarble Database Strategy:**
- **Account DB**: Authentication, billing, realm selection (PostgreSQL)
- **Player DB**: Character data, inventory, skills (PostgreSQL with sharding)
- **World DB**: Terrain, resources, NPCs (PostgreSQL + Redis cache)
- **Economy DB**: Market transactions, trade history (TimescaleDB for time-series)
- **Analytics DB**: Player behavior, metrics (ClickHouse for analytics)

**Sharding Strategy:**
```cpp
// Shard player data by account ID range
uint32_t GetPlayerShard(uint64_t accountId) {
    const uint32_t NUM_SHARDS = 16;
    return accountId % NUM_SHARDS;
}

// Shard world data by geographic region
uint32_t GetWorldShard(float x, float y) {
    const uint32_t GRID_SIZE = 1000.0f;
    uint32_t gridX = static_cast<uint32_t>(x / GRID_SIZE);
    uint32_t gridY = static_cast<uint32_t>(y / GRID_SIZE);
    return (gridX << 16) | gridY;
}
```

---

## Part III: Network Protocol Design

### 5. Opcode-Based Binary Protocol

**Packet Structure from WoW Protocol:**

```cpp
// Packet header (encrypted in WoW 3.3.5+)
struct PacketHeader {
    uint16_t size;    // Packet size excluding header
    uint16_t opcode;  // Message type identifier
};

// Example opcodes
enum Opcodes : uint16_t {
    // Authentication
    CMSG_AUTH_SESSION         = 0x01ED,
    SMSG_AUTH_RESPONSE        = 0x01EE,
    
    // Movement
    MSG_MOVE_START_FORWARD    = 0x00B5,
    MSG_MOVE_STOP             = 0x00B7,
    MSG_MOVE_JUMP             = 0x00BB,
    
    // Combat
    CMSG_CAST_SPELL           = 0x012E,
    SMSG_SPELL_START          = 0x0131,
    SMSG_SPELL_GO             = 0x0132,
    
    // Chat
    CMSG_MESSAGECHAT          = 0x0095,
    SMSG_MESSAGECHAT          = 0x0096,
    
    // Inventory
    CMSG_SWAP_INV_ITEM        = 0x010F,
    SMSG_ITEM_PUSH_RESULT     = 0x0166,
};

// Packet handler system
class PacketHandler {
private:
    using HandlerFunction = void(*)(WorldSession*, ByteBuffer&);
    std::unordered_map<uint16_t, HandlerFunction> handlers;
    
public:
    void RegisterHandler(uint16_t opcode, HandlerFunction handler) {
        handlers[opcode] = handler;
    }
    
    void ProcessPacket(WorldSession* session, uint16_t opcode, ByteBuffer& data) {
        auto it = handlers.find(opcode);
        if (it == handlers.end()) {
            LOG_ERROR("Unknown opcode: 0x%04X", opcode);
            return;
        }
        
        // Call registered handler
        it->second(session, data);
    }
};

// Example: Movement packet handling
void HandleMoveStartForward(WorldSession* session, ByteBuffer& data) {
    MovementInfo movementInfo;
    
    // Parse movement data
    data >> movementInfo.flags;
    data >> movementInfo.time;
    data >> movementInfo.pos.x;
    data >> movementInfo.pos.y;
    data >> movementInfo.pos.z;
    data >> movementInfo.pos.o;
    
    Player* player = session->GetPlayer();
    if (!player) return;
    
    // Server-side validation
    if (!player->IsValidMovement(movementInfo)) {
        // Reject and correct client
        player->SendTeleport(player->GetPosition());
        return;
    }
    
    // Update player position
    player->SetPosition(movementInfo.pos);
    player->SetMovementFlags(movementInfo.flags);
    
    // Broadcast to nearby players
    ByteBuffer packet;
    packet << player->GetGUID();
    packet << movementInfo.flags;
    packet << movementInfo.time;
    packet << movementInfo.pos.x;
    packet << movementInfo.pos.y;
    packet << movementInfo.pos.z;
    packet << movementInfo.pos.o;
    
    player->SendMessageToSet(MSG_MOVE_START_FORWARD, packet, false);
}
```

**BlueMarble Protocol Design:**
- **Binary Protocol**: Compact and efficient for high-frequency updates
- **Versioned Opcodes**: Support multiple client versions simultaneously
- **Compression**: zlib/lz4 for large packets (>1KB)
- **Delta Encoding**: Send only changed data for position updates

---

## Part IV: Scalability and Performance

### 6. Horizontal Scaling Strategies

**EVE Online's Single-Shard Architecture:**

```cpp
// Node-based cluster architecture inspired by EVE Online
class ClusterNode {
private:
    NodeId nodeId;
    std::vector<uint32_t> solarSystems;  // Maps/zones owned by this node
    
    // Inter-node communication
    std::shared_ptr<MessageBus> messageBus;
    
public:
    // Transfer zone to another node (for load balancing)
    void TransferZone(uint32_t zoneId, NodeId targetNode) {
        // 1. Pause zone updates
        auto* zone = GetZone(zoneId);
        zone->Pause();
        
        // 2. Serialize zone state
        ByteBuffer zoneState;
        zone->Serialize(zoneState);
        
        // 3. Send to target node
        messageBus->Send(targetNode, MSG_ZONE_TRANSFER, zoneState);
        
        // 4. Wait for acknowledgment
        auto response = messageBus->WaitForResponse(targetNode, 30000);
        
        if (response.type == MSG_ZONE_TRANSFER_ACK) {
            // 5. Redirect players to new node
            for (auto* player : zone->GetPlayers()) {
                player->TransferToNode(targetNode, zoneId);
            }
            
            // 6. Unload zone locally
            UnloadZone(zoneId);
        } else {
            // Transfer failed, resume locally
            zone->Resume();
        }
    }
    
    // Handle incoming zone transfer
    void ReceiveZoneTransfer(NodeId sourceNode, ByteBuffer& zoneState) {
        // 1. Deserialize zone state
        auto* zone = new Zone();
        zone->Deserialize(zoneState);
        
        // 2. Load into local memory
        LoadZone(zone);
        
        // 3. Acknowledge transfer
        messageBus->Send(sourceNode, MSG_ZONE_TRANSFER_ACK);
    }
};

// Load balancer monitors and redistributes zones
class LoadBalancer {
private:
    std::vector<ClusterNode*> nodes;
    
    struct NodeMetrics {
        float cpuUsage;
        float memoryUsage;
        uint32_t playerCount;
        std::vector<uint32_t> zones;
    };
    
    std::unordered_map<NodeId, NodeMetrics> metrics;
    
public:
    void RebalanceCluster() {
        // Find overloaded and underloaded nodes
        auto overloaded = FindOverloadedNodes();
        auto underloaded = FindUnderloadedNodes();
        
        if (overloaded.empty() || underloaded.empty()) return;
        
        // Transfer zones from overloaded to underloaded
        for (auto* sourceNode : overloaded) {
            auto& sourceMetrics = metrics[sourceNode->GetId()];
            
            // Find least populated zone to transfer
            uint32_t zoneToTransfer = FindLeastPopulatedZone(sourceMetrics.zones);
            
            // Find best target node
            auto* targetNode = underloaded.front();
            
            LOG_INFO("Transferring zone %u from node %u to node %u",
                     zoneToTransfer, sourceNode->GetId(), targetNode->GetId());
            
            sourceNode->TransferZone(zoneToTransfer, targetNode->GetId());
            
            // Update metrics
            sourceMetrics.zones.erase(
                std::remove(sourceMetrics.zones.begin(), 
                           sourceMetrics.zones.end(), 
                           zoneToTransfer),
                sourceMetrics.zones.end()
            );
            
            metrics[targetNode->GetId()].zones.push_back(zoneToTransfer);
        }
    }
};
```

---

## Part V: BlueMarble-Specific Recommendations

### 7. Implementing Planet-Scale Architecture

**Recommended Technology Stack:**

```yaml
# BlueMarble Server Architecture
Services:
  Gateway:
    Technology: NGINX + custom load balancer
    Instances: 3+ (with auto-scaling)
    Purpose: DDoS protection, TLS termination, connection routing
    
  Authentication:
    Technology: ASP.NET Core Web API
    Instances: 3+ (stateless, horizontally scalable)
    Database: PostgreSQL (replicated)
    Cache: Redis (session storage)
    Purpose: Account management, login, realm selection
    
  World Servers:
    Technology: .NET 8 + custom game server
    Instances: 10-50 (one per continental zone)
    Database: PostgreSQL (sharded by zone)
    Cache: Redis (entity state cache)
    Purpose: Game simulation, physics, AI
    
  Database Cluster:
    Primary: PostgreSQL 16 (ACID transactions)
    Time-Series: TimescaleDB (economy, analytics)
    Cache: Redis Cluster (hot data)
    Sharding: By player ID and geographic region
    
  Message Queue:
    Technology: RabbitMQ or Apache Kafka
    Purpose: Inter-server communication, event sourcing
    
  Monitoring:
    Metrics: Prometheus + Grafana
    Logging: ELK Stack (Elasticsearch, Logstash, Kibana)
    Tracing: Jaeger (distributed tracing)
```

**Performance Targets:**
- **Concurrent Players**: 10,000 per world server
- **Total Capacity**: 500,000 concurrent players (50 servers)
- **Tick Rate**: 20 TPS (50ms per tick)
- **Network Latency**: <100ms within region, <200ms cross-region
- **Database Queries**: <10ms for cached reads, <50ms for writes
- **Zone Transfer**: <3 seconds for seamless transition

---

## Core Concepts Summary

1. **Multi-Tier Architecture**: Separate auth, gateway, world servers, and databases
2. **Zone-Based Partitioning**: Horizontal scaling through geographic distribution
3. **Interest Management**: Reduce network traffic by 90%+ through AOI systems
4. **Database Sharding**: Split data by player ID and geographic region
5. **Opcode Protocol**: Binary protocol for efficient network communication
6. **Dynamic Load Balancing**: Migrate zones between servers based on load

---

## BlueMarble Application Guidelines

### Implementation Priorities

1. **Phase 1: Core Infrastructure** (Months 1-3)
   - Authentication service with SRP6 protocol
   - Basic world server with single-zone support
   - PostgreSQL database with core schemas
   - Redis caching layer

2. **Phase 2: Scalability** (Months 4-6)
   - Multi-zone support with zone transfer
   - Interest management system
   - Database sharding implementation
   - Load balancing service

3. **Phase 3: Optimization** (Months 7-9)
   - Network protocol compression
   - Delta encoding for state updates
   - Advanced caching strategies
   - Performance profiling and tuning

### Code Quality Standards

```csharp
// Example: World server in C# for BlueMarble
public class WorldServer
{
    private readonly IConfiguration _config;
    private readonly ILogger<WorldServer> _logger;
    private readonly WorldDatabase _worldDb;
    private readonly CharacterDatabase _charDb;
    private readonly IMessageQueue _messageQueue;
    
    private readonly MapManager _mapManager;
    private readonly SessionManager _sessionManager;
    private readonly InterestManager _interestManager;
    
    private const int TARGET_TPS = 20;
    private const int TICK_MS = 1000 / TARGET_TPS;
    
    public async Task RunAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("World server starting...");
        
        // Initialize world state
        await LoadMapsAsync();
        await LoadSpawnDataAsync();
        
        var stopwatch = Stopwatch.StartNew();
        long previousTick = 0;
        
        while (!cancellationToken.IsCancellationRequested)
        {
            long currentTick = stopwatch.ElapsedMilliseconds;
            int deltaMs = (int)(currentTick - previousTick);
            
            // Network I/O
            await ProcessIncomingPacketsAsync();
            
            // World simulation
            await UpdateWorldAsync(deltaMs);
            
            // Broadcast state updates
            await BroadcastWorldStateAsync();
            
            // Database operations
            await ProcessDatabaseQueueAsync();
            
            previousTick = currentTick;
            
            // Maintain target tick rate
            int executionTime = (int)(stopwatch.ElapsedMilliseconds - currentTick);
            int sleepTime = Math.Max(0, TICK_MS - executionTime);
            
            if (sleepTime > 0)
            {
                await Task.Delay(sleepTime, cancellationToken);
            }
            else
            {
                _logger.LogWarning(
                    "Server tick took {ExecutionTime}ms (target: {TargetTime}ms)",
                    executionTime, TICK_MS
                );
            }
        }
        
        _logger.LogInformation("World server shutting down...");
    }
    
    private async Task UpdateWorldAsync(int deltaMs)
    {
        var updateTasks = new List<Task>();
        
        // Update all active maps in parallel
        foreach (var map in _mapManager.GetActiveMaps())
        {
            updateTasks.Add(Task.Run(() => map.Update(deltaMs)));
        }
        
        await Task.WhenAll(updateTasks);
        
        // Update interest sets
        _interestManager.UpdateInterests();
    }
}
```

---

## Implementation Recommendations

### Security Best Practices

1. **Authentication**:
   - Use SRP6 or modern equivalent (OAuth2 + JWT for web)
   - Never transmit passwords in plaintext
   - Implement rate limiting on login attempts
   - Use hardware security modules (HSM) for key storage

2. **Network Security**:
   - TLS 1.3 for all connections
   - Header encryption for game protocol
   - DDoS protection at gateway level
   - IP whitelisting for server-to-server communication

3. **Data Validation**:
   - Server-authoritative game state
   - Validate all client inputs
   - Anti-cheat: movement validation, resource verification
   - Audit logging for sensitive operations

### Performance Optimization

1. **Memory Management**:
   - Object pooling for frequently created objects
   - Minimize garbage collection with struct types
   - Memory-mapped files for large static data

2. **Database Optimization**:
   - Connection pooling (min: 10, max: 100 per server)
   - Prepared statements for common queries
   - Batch inserts/updates where possible
   - Asynchronous database operations

3. **Network Optimization**:
   - UDP for non-critical updates (position broadcasts)
   - TCP for critical updates (inventory, combat)
   - Packet batching (50ms intervals)
   - Compression for packets >1KB

---

## References

1. **Open Source Projects**:
   - TrinityCore: https://github.com/TrinityCore/TrinityCore
   - CMaNGOS: https://github.com/cmangos
   - AzerothCore: https://github.com/azerothcore/azerothcore-wotlk
   
2. **Documentation**:
   - WoW Protocol: https://wowdev.wiki/
   - SRP6 Authentication: https://en.wikipedia.org/wiki/Secure_Remote_Password_protocol
   
3. **Industry Resources**:
   - EVE Online Architecture: GDC talks on single-shard design
   - World of Warcraft: GDC postmortems on server architecture
   - Networking for Game Programmers: https://gafferongames.com/

4. **Related BlueMarble Research**:
   - [WoW Emulator Architecture](../../topics/wow-emulator-architecture-networking.md)
   - [Game Programming in C++](./game-dev-analysis-01-game-programming-cpp.md)
   - [Spatial Data Storage](../../spatial-data-storage/)

---

**Document Status**: Complete  
**Last Updated**: 2025-01-17  
**Next Review**: Phase 2 Planning  
**Contributors**: BlueMarble Research Team
