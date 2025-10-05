# TrinityCore MMORPG Server Implementation - Code Analysis for BlueMarble

---
title: TrinityCore Server Architecture - Production MMORPG Implementation Analysis
date: 2025-01-17
tags: [mmorpg, server-architecture, cpp, database, networking, wow-emulator]
status: complete
priority: high
parent-research: online-game-dev-resources.md
discovered-from: World of Warcraft Programming (Topic 25)
related-documents: [wow-emulator-architecture-networking.md, game-dev-analysis-world-of-warcraft-programming.md, game-dev-analysis-wowdev-wiki-protocol.md]
---

**Source:** TrinityCore - WoW 3.3.5a Emulator (https://github.com/TrinityCore/TrinityCore)  
**Category:** MMORPG Development - Server Implementation  
**Priority:** High  
**Status:** ✅ Complete  
**Discovered From:** World of Warcraft Programming analysis (Topic 25)  
**Lines:** 500+  
**Related Sources:** CMaNGOS, AzerothCore, wowdev.wiki, WoW Emulator Architecture document

---

## Executive Summary

TrinityCore is a production-quality, open-source MMORPG server implementation that emulates World of Warcraft 3.3.5a (Wrath of the Lich King). With over 15 years of development and hundreds of contributors, it represents one of the most mature and battle-tested MMORPG codebases available for study. The project demonstrates real-world solutions to complex problems in server architecture, database design, network protocols, and game state management.

**Key Value for BlueMarble:**
- Production-ready C++ codebase handling thousands of concurrent players
- Comprehensive database schema for persistent MMORPG world state
- Battle-tested networking protocol implementation with packet handlers
- Modular architecture separating auth, world, and game logic concerns
- Advanced features: scripting system, instance management, spatial queries, AI framework

**Critical Learnings:**
1. **Dual-Server Architecture:** Separate authentication and world servers for security and scalability
2. **Database Design:** Multi-database approach (auth, characters, world) with optimized schemas
3. **Packet Handling:** Opcode-based routing system processing 100,000+ packets/second
4. **Spatial Management:** Grid-based world partitioning for efficient entity queries
5. **Scripting System:** C++ scripting framework for content without core modifications

---

## Part I: Server Architecture

### 1. Dual-Server Model

**Architecture Overview:**

```
TrinityCore Architecture:
┌─────────────────────────────────────────────────────────────┐
│                    Client Connections                        │
└──────────────────┬────────────────────┬─────────────────────┘
                   │                    │
            Port 3724 (Auth)     Port 8085 (World)
                   │                    │
         ┌─────────▼─────────┐  ┌──────▼────────────────┐
         │  authserver       │  │   worldserver          │
         │  (Authentication) │  │   (Game Simulation)    │
         └─────────┬─────────┘  └──────┬────────────────┘
                   │                    │
         ┌─────────▼─────────┐  ┌──────▼────────────────┐
         │  auth database    │  │  characters database   │
         │  - accounts       │  │  - character data      │
         │  - realm list     │  │  - inventory           │
         │  - bans           │  │  - guilds              │
         └───────────────────┘  │  world database        │
                                │  - creature templates  │
                                │  - gameobject data     │
                                │  - quest definitions   │
                                └────────────────────────┘
```

**AuthServer Responsibilities:**

```cpp
// Simplified authserver main loop
class AuthServer {
private:
    std::vector<AuthSession*> mActiveSessions;
    DatabasePool mAuthDatabase;
    
public:
    void Run() {
        while (mRunning) {
            // Accept new connections
            if (Socket* newSocket = mAcceptor.Accept()) {
                AuthSession* session = new AuthSession(newSocket);
                mActiveSessions.push_back(session);
            }
            
            // Update all active sessions
            for (AuthSession* session : mActiveSessions) {
                if (!session->Update()) {
                    // Session complete or failed, remove it
                    RemoveSession(session);
                }
            }
            
            // Rate limiting and DoS protection
            ApplyRateLimiting();
            
            std::this_thread::sleep_for(std::chrono::milliseconds(10));
        }
    }
    
    void ApplyRateLimiting() {
        // Limit login attempts per IP
        // Prevent brute force attacks
        for (auto& [ip, attempts] : mLoginAttempts) {
            if (attempts > MAX_ATTEMPTS_PER_MINUTE) {
                BanIP(ip, 300); // 5 minute ban
            }
        }
    }
};

// AuthSession handles SRP6 authentication
class AuthSession {
public:
    enum State {
        STATUS_CHALLENGE,     // Send auth challenge
        STATUS_PROOF,         // Verify proof
        STATUS_REALMLIST,     // Send realm list
        STATUS_CLOSED         // Session complete
    };
    
    bool Update() {
        switch (mState) {
            case STATUS_CHALLENGE:
                SendAuthChallenge();
                mState = STATUS_PROOF;
                break;
                
            case STATUS_PROOF:
                if (VerifyAuthProof()) {
                    mState = STATUS_REALMLIST;
                } else {
                    LogFailedLogin();
                    return false; // Close session
                }
                break;
                
            case STATUS_REALMLIST:
                SendRealmList();
                mState = STATUS_CLOSED;
                break;
                
            case STATUS_CLOSED:
                return false; // Signal session complete
        }
        return true;
    }
};
```

**WorldServer Responsibilities:**

```cpp
// Simplified worldserver main loop
class World {
private:
    uint32 mUpdateInterval = 50; // 50ms = 20 Hz tick rate
    std::vector<Map*> mMaps;
    std::unordered_map<uint32, WorldSession*> mSessions;
    
public:
    void Run() {
        uint32 lastUpdate = getMSTime();
        uint32 updateCount = 0;
        
        while (mRunning) {
            uint32 currentTime = getMSTime();
            uint32 diff = currentTime - lastUpdate;
            
            if (diff >= mUpdateInterval) {
                Update(diff);
                lastUpdate = currentTime;
                ++updateCount;
                
                // Performance metrics
                if (updateCount % 1200 == 0) { // Every minute
                    LogPerformanceStats();
                }
            } else {
                std::this_thread::sleep_for(
                    std::chrono::milliseconds(mUpdateInterval - diff)
                );
            }
        }
    }
    
    void Update(uint32 diff) {
        // 1. Process incoming packets from all sessions
        ProcessIncomingPackets();
        
        // 2. Update all active maps (zones)
        for (Map* map : mMaps) {
            if (map->HasPlayers()) {
                map->Update(diff);
            }
        }
        
        // 3. Update global systems
        UpdateGameTime(diff);
        UpdateAuctions(diff);
        UpdateBattlegrounds(diff);
        UpdateGuildSystem(diff);
        
        // 4. Process outgoing packets
        ProcessOutgoingPackets();
        
        // 5. Database operations (async)
        ProcessDatabaseQueue();
        
        // 6. Script system updates
        sScriptMgr->OnWorldUpdate(diff);
    }
};
```

**BlueMarble Application:**

```cpp
// Adapt dual-server for geological simulation
class BlueMarbleAuthServer {
    // Handle researcher authentication
    // Institutional credential verification
    // Access control for sensitive geological data
};

class BlueMarbleWorldServer {
    // Geological simulation engine
    // Real-time tectonic activity
    // Erosion and weathering systems
    // Player research activities
    // Collaborative science features
};
```

### 2. Database Architecture

**Schema Organization:**

TrinityCore uses three primary databases:

```sql
-- Database: auth (Authentication Server)
CREATE TABLE account (
    id INT UNSIGNED PRIMARY KEY AUTO_INCREMENT,
    username VARCHAR(32) UNIQUE NOT NULL,
    sha_pass_hash VARCHAR(40) NOT NULL,  -- SHA1(username:password)
    sessionkey VARCHAR(80),
    v VARCHAR(64),
    s VARCHAR(64),
    email VARCHAR(255),
    reg_mail VARCHAR(255),
    joindate TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    last_ip VARCHAR(15),
    last_login TIMESTAMP,
    locked TINYINT DEFAULT 0,
    lock_country VARCHAR(2),
    expansion TINYINT DEFAULT 2,  -- 0=Vanilla, 1=TBC, 2=WotLK
    mutetime BIGINT DEFAULT 0,
    mutereason VARCHAR(255),
    locale TINYINT DEFAULT 0,
    os VARCHAR(3),
    recruiter INT UNSIGNED DEFAULT 0
);

CREATE TABLE realmlist (
    id INT UNSIGNED PRIMARY KEY AUTO_INCREMENT,
    name VARCHAR(32) NOT NULL,
    address VARCHAR(255) NOT NULL,
    localAddress VARCHAR(255) NOT NULL,
    localSubnetMask VARCHAR(255) NOT NULL,
    port SMALLINT UNSIGNED DEFAULT 8085,
    icon TINYINT UNSIGNED DEFAULT 0,
    flag TINYINT UNSIGNED DEFAULT 2,
    timezone TINYINT UNSIGNED DEFAULT 0,
    allowedSecurityLevel TINYINT UNSIGNED DEFAULT 0,
    population FLOAT UNSIGNED DEFAULT 0,
    gamebuild INT UNSIGNED DEFAULT 12340
);

CREATE TABLE account_banned (
    id INT UNSIGNED PRIMARY KEY DEFAULT 0,
    bandate INT UNSIGNED NOT NULL DEFAULT 0,
    unbandate INT UNSIGNED NOT NULL DEFAULT 0,
    bannedby VARCHAR(50) NOT NULL,
    banreason VARCHAR(255) NOT NULL,
    active TINYINT UNSIGNED NOT NULL DEFAULT 1
);

-- Database: characters (Per-Realm Player Data)
CREATE TABLE characters (
    guid INT UNSIGNED PRIMARY KEY AUTO_INCREMENT,
    account INT UNSIGNED NOT NULL,
    name VARCHAR(12) NOT NULL,
    race TINYINT UNSIGNED NOT NULL,
    class TINYINT UNSIGNED NOT NULL,
    gender TINYINT UNSIGNED NOT NULL,
    level TINYINT UNSIGNED NOT NULL DEFAULT 1,
    xp INT UNSIGNED NOT NULL DEFAULT 0,
    money INT UNSIGNED NOT NULL DEFAULT 0,
    
    -- Position
    map SMALLINT UNSIGNED NOT NULL DEFAULT 0,
    instance_id INT UNSIGNED NOT NULL DEFAULT 0,
    position_x FLOAT NOT NULL DEFAULT 0,
    position_y FLOAT NOT NULL DEFAULT 0,
    position_z FLOAT NOT NULL DEFAULT 0,
    orientation FLOAT NOT NULL DEFAULT 0,
    
    -- Stats
    health INT UNSIGNED NOT NULL DEFAULT 100,
    power1 INT UNSIGNED NOT NULL DEFAULT 0,  -- Mana
    power2 INT UNSIGNED NOT NULL DEFAULT 0,  -- Rage
    power3 INT UNSIGNED NOT NULL DEFAULT 100, -- Energy
    
    -- Played time
    totaltime INT UNSIGNED NOT NULL DEFAULT 0,
    leveltime INT UNSIGNED NOT NULL DEFAULT 0,
    
    -- Customization
    skin TINYINT UNSIGNED NOT NULL DEFAULT 0,
    face TINYINT UNSIGNED NOT NULL DEFAULT 0,
    hairstyle TINYINT UNSIGNED NOT NULL DEFAULT 0,
    haircolor TINYINT UNSIGNED NOT NULL DEFAULT 0,
    
    -- Flags
    at_login SMALLINT UNSIGNED NOT NULL DEFAULT 0,
    online TINYINT UNSIGNED NOT NULL DEFAULT 0,
    
    INDEX idx_account (account),
    INDEX idx_name (name),
    INDEX idx_online (online)
);

CREATE TABLE character_inventory (
    guid INT UNSIGNED NOT NULL,
    bag TINYINT UNSIGNED NOT NULL DEFAULT 0,
    slot TINYINT UNSIGNED NOT NULL DEFAULT 0,
    item MEDIUMINT UNSIGNED NOT NULL DEFAULT 0,
    item_template INT UNSIGNED NOT NULL DEFAULT 0,
    PRIMARY KEY (guid, bag, slot),
    INDEX idx_item (item)
);

CREATE TABLE character_skills (
    guid INT UNSIGNED NOT NULL,
    skill SMALLINT UNSIGNED NOT NULL,
    value SMALLINT UNSIGNED NOT NULL,
    max SMALLINT UNSIGNED NOT NULL,
    PRIMARY KEY (guid, skill)
);

CREATE TABLE guild (
    guildid INT UNSIGNED PRIMARY KEY AUTO_INCREMENT,
    name VARCHAR(24) NOT NULL,
    leaderguid INT UNSIGNED NOT NULL,
    EmblemStyle TINYINT UNSIGNED NOT NULL DEFAULT 0,
    EmblemColor TINYINT UNSIGNED NOT NULL DEFAULT 0,
    BorderStyle TINYINT UNSIGNED NOT NULL DEFAULT 0,
    BorderColor TINYINT UNSIGNED NOT NULL DEFAULT 0,
    BackgroundColor TINYINT UNSIGNED NOT NULL DEFAULT 0,
    info TEXT,
    motd VARCHAR(128) NOT NULL DEFAULT '',
    createdate INT UNSIGNED NOT NULL DEFAULT 0,
    BankMoney BIGINT UNSIGNED NOT NULL DEFAULT 0
);

-- Database: world (Static Game Data - Shared Across Realms)
CREATE TABLE creature_template (
    entry MEDIUMINT UNSIGNED PRIMARY KEY,
    difficulty_entry_1 MEDIUMINT UNSIGNED NOT NULL DEFAULT 0,
    difficulty_entry_2 MEDIUMINT UNSIGNED NOT NULL DEFAULT 0,
    difficulty_entry_3 MEDIUMINT UNSIGNED NOT NULL DEFAULT 0,
    KillCredit1 INT UNSIGNED NOT NULL DEFAULT 0,
    KillCredit2 INT UNSIGNED NOT NULL DEFAULT 0,
    modelid1 MEDIUMINT UNSIGNED NOT NULL DEFAULT 0,
    modelid2 MEDIUMINT UNSIGNED NOT NULL DEFAULT 0,
    modelid3 MEDIUMINT UNSIGNED NOT NULL DEFAULT 0,
    modelid4 MEDIUMINT UNSIGNED NOT NULL DEFAULT 0,
    name VARCHAR(100) NOT NULL DEFAULT '',
    subname VARCHAR(100),
    IconName VARCHAR(100),
    gossip_menu_id MEDIUMINT UNSIGNED NOT NULL DEFAULT 0,
    minlevel TINYINT UNSIGNED NOT NULL DEFAULT 1,
    maxlevel TINYINT UNSIGNED NOT NULL DEFAULT 1,
    exp SMALLINT NOT NULL DEFAULT 0,
    faction SMALLINT UNSIGNED NOT NULL DEFAULT 0,
    npcflag INT UNSIGNED NOT NULL DEFAULT 0,
    speed_walk FLOAT NOT NULL DEFAULT 1,
    speed_run FLOAT NOT NULL DEFAULT 1.14286,
    scale FLOAT NOT NULL DEFAULT 1,
    rank TINYINT UNSIGNED NOT NULL DEFAULT 0,
    minhealth INT UNSIGNED NOT NULL DEFAULT 1,
    maxhealth INT UNSIGNED NOT NULL DEFAULT 1,
    minmana INT UNSIGNED NOT NULL DEFAULT 0,
    maxmana INT UNSIGNED NOT NULL DEFAULT 0,
    armor INT UNSIGNED NOT NULL DEFAULT 0,
    ScriptName VARCHAR(64) NOT NULL DEFAULT '',
    AIName VARCHAR(64) NOT NULL DEFAULT ''
);

CREATE TABLE creature (
    guid INT UNSIGNED PRIMARY KEY AUTO_INCREMENT,
    id MEDIUMINT UNSIGNED NOT NULL DEFAULT 0, -- References creature_template
    map SMALLINT UNSIGNED NOT NULL DEFAULT 0,
    spawnMask TINYINT UNSIGNED NOT NULL DEFAULT 1,
    phaseMask INT UNSIGNED NOT NULL DEFAULT 1,
    position_x FLOAT NOT NULL DEFAULT 0,
    position_y FLOAT NOT NULL DEFAULT 0,
    position_z FLOAT NOT NULL DEFAULT 0,
    orientation FLOAT NOT NULL DEFAULT 0,
    spawntimesecs INT UNSIGNED NOT NULL DEFAULT 120,
    wander_distance FLOAT NOT NULL DEFAULT 0,
    currentwaypoint INT UNSIGNED NOT NULL DEFAULT 0,
    curhealth INT UNSIGNED NOT NULL DEFAULT 1,
    curmana INT UNSIGNED NOT NULL DEFAULT 0,
    MovementType TINYINT UNSIGNED NOT NULL DEFAULT 0,
    INDEX idx_map (map),
    INDEX idx_id (id)
);

CREATE TABLE quest_template (
    ID MEDIUMINT UNSIGNED PRIMARY KEY,
    QuestType TINYINT UNSIGNED NOT NULL DEFAULT 2,
    QuestLevel SMALLINT NOT NULL DEFAULT 1,
    MinLevel TINYINT UNSIGNED NOT NULL DEFAULT 0,
    QuestSortID SMALLINT NOT NULL DEFAULT 0,
    RequiredRaces SMALLINT UNSIGNED NOT NULL DEFAULT 0,
    RequiredClasses SMALLINT UNSIGNED NOT NULL DEFAULT 0,
    ObjectiveText1 TEXT,
    ObjectiveText2 TEXT,
    ObjectiveText3 TEXT,
    ObjectiveText4 TEXT,
    Title TEXT,
    Details TEXT,
    Objectives TEXT,
    OfferRewardText TEXT,
    RequestItemsText TEXT,
    LogTitle TEXT,
    LogDescription TEXT,
    QuestDescription TEXT
);
```

**Database Access Patterns:**

```cpp
// Prepared statements for performance and security
class CharacterDatabase : public DatabaseWorkerPool<CharacterDatabaseConnection> {
public:
    enum PreparedStatements {
        CHAR_SEL_CHARACTER = 0,
        CHAR_INS_CHARACTER,
        CHAR_UPD_CHARACTER,
        CHAR_DEL_CHARACTER,
        CHAR_SEL_CHARACTER_INVENTORY,
        CHAR_INS_ITEM_INSTANCE,
        // ... 300+ prepared statements
    };
    
    void LoadCharacter(uint32 guid, QueryCallback callback) {
        PreparedStatement* stmt = GetPreparedStatement(CHAR_SEL_CHARACTER);
        stmt->setUInt32(0, guid);
        
        // Async query with callback
        QueryAsync(stmt, callback);
    }
    
    void SaveCharacter(const Character* character) {
        SQLTransaction trans = BeginTransaction();
        
        // Save character base data
        PreparedStatement* stmt = GetPreparedStatement(CHAR_UPD_CHARACTER);
        stmt->setUInt32(0, character->GetLevel());
        stmt->setUInt32(1, character->GetMoney());
        stmt->setFloat(2, character->GetPositionX());
        stmt->setFloat(3, character->GetPositionY());
        stmt->setFloat(4, character->GetPositionZ());
        stmt->setUInt32(5, character->GetGUID());
        trans->Append(stmt);
        
        // Save inventory items
        for (auto& item : character->GetInventory()) {
            PreparedStatement* itemStmt = GetPreparedStatement(CHAR_INS_ITEM_INSTANCE);
            itemStmt->setUInt32(0, item->GetGUID());
            itemStmt->setUInt32(1, item->GetEntry());
            // ... set other fields
            trans->Append(itemStmt);
        }
        
        // Execute all statements in single transaction
        CommitTransaction(trans);
    }
};
```

**BlueMarble Database Design:**

```sql
-- Geological database schema inspired by TrinityCore
CREATE TABLE geological_formations (
    guid BIGINT UNSIGNED PRIMARY KEY AUTO_INCREMENT,
    formation_type SMALLINT UNSIGNED NOT NULL, -- Rock type ID
    map_id SMALLINT UNSIGNED NOT NULL,
    continent VARCHAR(32),
    position_x DOUBLE NOT NULL,
    position_y DOUBLE NOT NULL,
    position_z DOUBLE NOT NULL,
    age_millions_years FLOAT NOT NULL,
    composition_primary SMALLINT UNSIGNED NOT NULL,
    composition_secondary SMALLINT UNSIGNED,
    temperature_kelvin FLOAT,
    pressure_pascals FLOAT,
    density_kg_m3 FLOAT,
    discovery_date TIMESTAMP,
    researcher_id INT UNSIGNED,
    INDEX idx_map_position (map_id, position_x, position_y),
    INDEX idx_formation_type (formation_type),
    INDEX idx_age (age_millions_years)
);

CREATE TABLE geological_events (
    event_id BIGINT UNSIGNED PRIMARY KEY AUTO_INCREMENT,
    event_type TINYINT UNSIGNED NOT NULL, -- Earthquake, eruption, etc.
    start_time TIMESTAMP NOT NULL,
    duration_seconds INT UNSIGNED NOT NULL,
    epicenter_x DOUBLE NOT THE,
    epicenter_y DOUBLE NOT NULL,
    epicenter_z DOUBLE NOT NULL,
    magnitude FLOAT NOT NULL,
    affected_radius_km FLOAT NOT NULL,
    status TINYINT UNSIGNED NOT NULL DEFAULT 1, -- Active, completed
    INDEX idx_time_status (start_time, status),
    INDEX idx_position (epicenter_x, epicenter_y)
);

CREATE TABLE researcher_data (
    guid INT UNSIGNED PRIMARY KEY AUTO_INCREMENT,
    account_id INT UNSIGNED NOT NULL,
    character_name VARCHAR(50) NOT NULL,
    institution VARCHAR(100),
    specialization TINYINT UNSIGNED NOT NULL,
    research_level TINYINT UNSIGNED NOT NULL DEFAULT 1,
    publications_count INT UNSIGNED NOT NULL DEFAULT 0,
    samples_collected INT UNSIGNED NOT NULL DEFAULT 0,
    position_x DOUBLE NOT NULL,
    position_y DOUBLE NOT NULL,
    position_z DOUBLE NOT NULL,
    online TINYINT UNSIGNED NOT NULL DEFAULT 0,
    INDEX idx_account (account_id),
    INDEX idx_online (online)
);
```

---

## Part II: Network Protocol Implementation

### 3. Opcode Handler System

**Opcode Registration:**

```cpp
// OpcodeTable.cpp - Opcode handler registration
class OpcodeTable {
private:
    struct OpcodeHandler {
        char const* name;
        SessionStatus status;
        PacketProcessing packetProcessing;
        void (WorldSession::*handler)(WorldPacket&);
    };
    
    std::array<OpcodeHandler, NUM_OPCODE_HANDLERS> opcodeTable;
    
public:
    void Initialize() {
        // Movement opcodes
        StoreOpcode(CMSG_MOVE_START_FORWARD, "CMSG_MOVE_START_FORWARD",
                   STATUS_LOGGEDIN, PROCESS_THREADUNSAFE,
                   &WorldSession::HandleMoveStartForward);
        
        StoreOpcode(CMSG_MOVE_STOP, "CMSG_MOVE_STOP",
                   STATUS_LOGGEDIN, PROCESS_THREADUNSAFE,
                   &WorldSession::HandleMoveStop);
        
        // Combat opcodes
        StoreOpcode(CMSG_CAST_SPELL, "CMSG_CAST_SPELL",
                   STATUS_LOGGEDIN, PROCESS_THREADUNSAFE,
                   &WorldSession::HandleCastSpellOpcode);
        
        StoreOpcode(CMSG_ATTACKSWING, "CMSG_ATTACKSWING",
                   STATUS_LOGGEDIN, PROCESS_INPLACE,
                   &WorldSession::HandleAttackSwingOpcode);
        
        // Chat opcodes
        StoreOpcode(CMSG_MESSAGECHAT, "CMSG_MESSAGECHAT",
                   STATUS_LOGGEDIN, PROCESS_THREADSAFE,
                   &WorldSession::HandleMessagechatOpcode);
        
        // ... 400+ more opcodes
    }
    
    void StoreOpcode(uint16 opcode, char const* name,
                    SessionStatus status, PacketProcessing processing,
                    void (WorldSession::*handler)(WorldPacket&)) {
        opcodeTable[opcode] = {name, status, processing, handler};
    }
    
    OpcodeHandler const* GetHandler(uint16 opcode) const {
        if (opcode >= opcodeTable.size())
            return nullptr;
        return &opcodeTable[opcode];
    }
};
```

**Packet Processing:**

```cpp
// WorldSession.cpp - Packet processing
class WorldSession {
private:
    std::queue<WorldPacket*> mRecvQueue;
    Player* mPlayer;
    AccountTypes mSecurity;
    uint32 mLatency;
    
public:
    void QueuePacket(WorldPacket* packet) {
        std::lock_guard<std::mutex> guard(mRecvQueueLock);
        mRecvQueue.push(packet);
    }
    
    bool Update(uint32 diff) {
        WorldPacket* packet = nullptr;
        
        while (!mRecvQueue.empty()) {
            {
                std::lock_guard<std::mutex> guard(mRecvQueueLock);
                packet = mRecvQueue.front();
                mRecvQueue.pop();
            }
            
            OpcodeHandler const* handler = 
                sOpcodeTable->GetHandler(packet->GetOpcode());
            
            if (!handler) {
                LOG_ERROR("No handler for opcode %u", packet->GetOpcode());
                delete packet;
                continue;
            }
            
            // Security check
            if (handler->status > GetSecurity()) {
                LOG_WARN("Player %u tried to use opcode %s without permission",
                        GetAccountId(), handler->name);
                delete packet;
                continue;
            }
            
            // State check
            if (!IsValidStateForOpcode(handler->status)) {
                LOG_WARN("Invalid state for opcode %s", handler->name);
                delete packet;
                continue;
            }
            
            // Execute handler
            try {
                (this->*handler->handler)(*packet);
            } catch (std::exception& e) {
                LOG_ERROR("Exception in handler %s: %s", 
                         handler->name, e.what());
            }
            
            delete packet;
        }
        
        return true;
    }
};
```

**Example Handler:**

```cpp
void WorldSession::HandleCastSpellOpcode(WorldPacket& recvData) {
    uint32 spellId;
    uint8 castCount;
    uint8 castFlags;
    
    recvData >> castCount >> spellId >> castFlags;
    
    // Validation
    if (!mPlayer) {
        LOG_ERROR("Player not found for cast spell");
        return;
    }
    
    SpellInfo const* spellInfo = sSpellMgr->GetSpellInfo(spellId);
    if (!spellInfo) {
        LOG_ERROR("Unknown spell %u", spellId);
        return;
    }
    
    // Check if player can cast
    if (!mPlayer->IsAlive()) {
        SendCastFailed(spellId, SPELL_FAILED_CASTER_DEAD);
        return;
    }
    
    if (mPlayer->GetPower(spellInfo->PowerType) < spellInfo->PowerCost) {
        SendCastFailed(spellId, SPELL_FAILED_NOT_ENOUGH_MANA);
        return;
    }
    
    // Anti-cheat: spell cooldown check
    if (!mPlayer->GetSpellHistory()->IsReady(spellInfo)) {
        SendCastFailed(spellId, SPELL_FAILED_NOT_READY);
        return;
    }
    
    // Create and cast spell
    Spell* spell = new Spell(mPlayer, spellInfo, TRIGGERED_NONE);
    
    SpellCastTargets targets;
    targets.Read(recvData, mPlayer);
    
    spell->m_cast_count = castCount;
    spell->prepare(&targets);
}
```

**BlueMarble Handler Example:**

```cpp
void ResearchSession::HandleConductSurveyOpcode(WorldPacket& recvData) {
    uint32 surveyType;
    float radius;
    
    recvData >> surveyType >> radius;
    
    Researcher* researcher = GetResearcher();
    if (!researcher) return;
    
    // Validate survey parameters
    if (radius > MAX_SURVEY_RADIUS) {
        SendSurveyFailed(SURVEY_FAILED_RADIUS_TOO_LARGE);
        return;
    }
    
    // Check equipment
    if (!researcher->HasEquipment(EQUIPMENT_SEISMOMETER)) {
        SendSurveyFailed(SURVEY_FAILED_MISSING_EQUIPMENT);
        return;
    }
    
    // Check cooldown
    if (!researcher->CanStartSurvey()) {
        SendSurveyFailed(SURVEY_FAILED_COOLDOWN);
        return;
    }
    
    // Conduct survey
    GeologicalSurvey* survey = new GeologicalSurvey(
        researcher, surveyType, radius
    );
    survey->Execute();
    
    // Send results to client
    SendSurveyResults(survey);
}
```

---

## Part III: Spatial Management

### 4. Grid System

**Map and Grid Architecture:**

```cpp
// Map.h - Grid-based world partitioning
class Map {
private:
    static const uint32 MAX_GRIDS = 64;
    static const float SIZE_OF_GRIDS = 533.33333f; // yards
    static const float CENTER_GRID_OFFSET = (MAX_GRIDS / 2);
    
    Grid mGrids[MAX_GRIDS][MAX_GRIDS];
    std::set<Player*> mPlayers;
    std::set<Creature*> mCreatures;
    
public:
    Grid* GetGrid(float x, float y) {
        int gx = (int)((x - CENTER_GRID_OFFSET) / SIZE_OF_GRIDS);
        int gy = (int)((y - CENTER_GRID_OFFSET) / SIZE_OF_GRIDS);
        
        if (gx < 0 || gx >= MAX_GRIDS || gy < 0 || gy >= MAX_GRIDS)
            return nullptr;
            
        return &mGrids[gx][gy];
    }
    
    void Update(uint32 diff) {
        // Only update grids with active players
        for (Player* player : mPlayers) {
            Grid* grid = GetGrid(player->GetPositionX(), 
                                player->GetPositionY());
            if (grid && !grid->IsUpdated()) {
                grid->Update(diff);
                grid->SetUpdated(true);
            }
        }
        
        // Reset updated flags
        for (int x = 0; x < MAX_GRIDS; ++x) {
            for (int y = 0; y < MAX_GRIDS; ++y) {
                mGrids[x][y].SetUpdated(false);
            }
        }
    }
};

class Grid {
private:
    static const uint32 MAX_CELLS = 8;
    Cell mCells[MAX_CELLS][MAX_CELLS];
    bool mIsActive;
    bool mIsUpdated;
    
public:
    void Update(uint32 diff) {
        for (int x = 0; x < MAX_CELLS; ++x) {
            for (int y = 0; y < MAX_CELLS; ++y) {
                mCells[x][y].Update(diff);
            }
        }
    }
    
    void Visit(Visitor& visitor) {
        for (int x = 0; x < MAX_CELLS; ++x) {
            for (int y = 0; y < MAX_CELLS; ++y) {
                mCells[x][y].Visit(visitor);
            }
        }
    }
};

class Cell {
private:
    std::vector<WorldObject*> mObjects;
    
public:
    void Update(uint32 diff) {
        for (WorldObject* obj : mObjects) {
            obj->Update(diff);
        }
    }
    
    void Visit(Visitor& visitor) {
        for (WorldObject* obj : mObjects) {
            visitor.Visit(obj);
        }
    }
};
```

**Spatial Queries:**

```cpp
// Finding nearby entities
class NearbyObjectSearcher {
private:
    WorldObject* mSource;
    float mRange;
    std::vector<WorldObject*> mResults;
    
public:
    void Visit(WorldObject* obj) {
        if (obj == mSource)
            return;
            
        float distance = mSource->GetDistance(obj);
        if (distance <= mRange) {
            mResults.push_back(obj);
        }
    }
    
    std::vector<WorldObject*> const& GetResults() const {
        return mResults;
    }
};

// Usage
std::vector<Unit*> GetNearbyEnemies(Unit* source, float range) {
    NearbyObjectSearcher searcher(source, range);
    
    // Visit only nearby grids (optimization)
    int gridX = GetGridX(source->GetPositionX());
    int gridY = GetGridY(source->GetPositionY());
    
    for (int x = gridX - 1; x <= gridX + 1; ++x) {
        for (int y = gridY - 1; y <= gridY + 1; ++y) {
            if (Grid* grid = GetGrid(x, y)) {
                grid->Visit(searcher);
            }
        }
    }
    
    std::vector<Unit*> enemies;
    for (WorldObject* obj : searcher.GetResults()) {
        if (Unit* unit = obj->ToUnit()) {
            if (source->IsHostileTo(unit)) {
                enemies.push_back(unit);
            }
        }
    }
    
    return enemies;
}
```

**BlueMarble Spatial System:**

```cpp
// Geological grid system for planet-scale simulation
class GeologicalGrid {
private:
    static const uint32 CONTINENTAL_GRIDS = 100;
    static const float GRID_SIZE_KM = 100.0f;
    
    struct GridData {
        std::vector<GeologicalFormation*> formations;
        TectonicPlate* plate;
        ClimateZone* climate;
        float averageElevation;
        float seismicActivity;
        uint32 lastUpdateTime;
    };
    
    GridData mGrids[CONTINENTAL_GRIDS][CONTINENTAL_GRIDS];
    
public:
    void UpdateGeologicalProcesses(uint32 diff) {
        // Only update grids with active researchers or ongoing events
        for (int x = 0; x < CONTINENTAL_GRIDS; ++x) {
            for (int y = 0; y < CONTINENTAL_GRIDS; ++y) {
                GridData& grid = mGrids[x][y];
                
                if (ShouldUpdate(grid)) {
                    UpdateTectonicActivity(grid, diff);
                    UpdateErosion(grid, diff);
                    UpdateWeathering(grid, diff);
                    
                    grid.lastUpdateTime = getCurrentTime();
                }
            }
        }
    }
    
    bool ShouldUpdate(const GridData& grid) {
        // Update if:
        // 1. Has researchers nearby
        // 2. Has high seismic activity
        // 3. Hasn't been updated in long time
        return HasNearbyResearchers(grid) ||
               grid.seismicActivity > THRESHOLD ||
               (getCurrentTime() - grid.lastUpdateTime) > MAX_UPDATE_INTERVAL;
    }
};
```

---

## Part IV: Scripting System

### 5. C++ Scripting Framework

**Script Registration:**

```cpp
// ScriptMgr.h - Script registration system
class ScriptMgr {
private:
    std::vector<CreatureScript*> mCreatureScripts;
    std::vector<GameObjectScript*> mGameObjectScripts;
    std::vector<QuestScript*> mQuestScripts;
    std::map<uint32, CreatureScript*> mCreatureScriptsByEntry;
    
public:
    void AddScript(CreatureScript* script) {
        mCreatureScripts.push_back(script);
    }
    
    void OnCreatureCreate(Creature* creature) {
        auto it = mCreatureScriptsByEntry.find(creature->GetEntry());
        if (it != mCreatureScriptsByEntry.end()) {
            it->second->OnSpawn(creature);
        }
    }
    
    void OnCreatureUpdate(Creature* creature, uint32 diff) {
        if (CreatureScript* script = creature->GetScript()) {
            script->OnUpdate(creature, diff);
        }
    }
};

// Base script classes
class CreatureScript {
public:
    virtual void OnSpawn(Creature* creature) {}
    virtual void OnUpdate(Creature* creature, uint32 diff) {}
    virtual void OnDeath(Creature* creature, Unit* killer) {}
    virtual void OnCombatStart(Creature* creature, Unit* target) {}
    virtual void OnCombatEnd(Creature* creature) {}
};

// Example boss script
class Boss_LichKing : public CreatureScript {
public:
    Boss_LichKing() : CreatureScript("boss_lich_king") {}
    
    struct Boss_LichKingAI : public BossAI {
        enum Phases {
            PHASE_ONE = 1,
            PHASE_TWO = 2,
            PHASE_THREE = 3
        };
        
        enum Spells {
            SPELL_REMORSELESS_WINTER = 68895,
            SPELL_INFEST = 70541,
            SPELL_NECROTIC_PLAGUE = 70337,
            SPELL_SUMMON_VALKYR = 69037
        };
        
        Boss_LichKingAI(Creature* creature) : BossAI(creature, BOSS_LICH_KING) {
            mPhase = PHASE_ONE;
        }
        
        void Reset() override {
            _Reset();
            mPhase = PHASE_ONE;
            events.ScheduleEvent(EVENT_REMORSELESS_WINTER, 30000);
            events.ScheduleEvent(EVENT_INFEST, 20000);
        }
        
        void DamageTaken(Unit* attacker, uint32& damage) override {
            if (me->HealthBelowPctDamaged(70, damage) && mPhase == PHASE_ONE) {
                mPhase = PHASE_TWO;
                DoScriptText(SAY_PHASE_TWO, me);
                events.ScheduleEvent(EVENT_SUMMON_VALKYR, 5000);
            }
            
            if (me->HealthBelowPctDamaged(40, damage) && mPhase == PHASE_TWO) {
                mPhase = PHASE_THREE;
                DoScriptText(SAY_PHASE_THREE, me);
                events.ScheduleEvent(EVENT_NECROTIC_PLAGUE, 10000);
            }
        }
        
        void UpdateAI(uint32 diff) override {
            if (!UpdateVictim())
                return;
            
            events.Update(diff);
            
            while (uint32 eventId = events.ExecuteEvent()) {
                switch (eventId) {
                    case EVENT_REMORSELESS_WINTER:
                        DoCastAOE(SPELL_REMORSELESS_WINTER);
                        events.ScheduleEvent(EVENT_REMORSELESS_WINTER, 60000);
                        break;
                        
                    case EVENT_INFEST:
                        DoCastAOE(SPELL_INFEST);
                        events.ScheduleEvent(EVENT_INFEST, 25000);
                        break;
                        
                    case EVENT_SUMMON_VALKYR:
                        for (int i = 0; i < 3; ++i) {
                            DoCast(SPELL_SUMMON_VALKYR);
                        }
                        events.ScheduleEvent(EVENT_SUMMON_VALKYR, 45000);
                        break;
                        
                    case EVENT_NECROTIC_PLAGUE:
                        if (Unit* target = SelectTarget(SELECT_TARGET_RANDOM)) {
                            DoCast(target, SPELL_NECROTIC_PLAGUE);
                        }
                        events.ScheduleEvent(EVENT_NECROTIC_PLAGUE, 30000);
                        break;
                }
            }
            
            DoMeleeAttackIfReady();
        }
        
    private:
        uint8 mPhase;
    };
    
    CreatureAI* GetAI(Creature* creature) const override {
        return new Boss_LichKingAI(creature);
    }
};

// Register script
void AddSC_boss_lich_king() {
    new Boss_LichKing();
}
```

**BlueMarble Scripting:**

```cpp
// Geological event scripts
class GeologicalEventScript {
public:
    virtual void OnEventStart(GeologicalEvent* event) {}
    virtual void OnEventUpdate(GeologicalEvent* event, uint32 diff) {}
    virtual void OnEventEnd(GeologicalEvent* event) {}
};

class Event_MountVesuviusEruption : public GeologicalEventScript {
public:
    Event_MountVesuviusEruption() 
        : GeologicalEventScript("event_vesuvius_eruption") {}
    
    void OnEventStart(GeologicalEvent* event) override {
        // Announce eruption to nearby researchers
        AnnounceToRegion("Mount Vesuvius eruption imminent!");
        
        // Begin seismic activity increase
        IncreaseSeismicActivity(event->GetEpicenter(), 50.0f);
        
        // Schedule eruption phases
        event->SchedulePhase(PHASE_PRECURSORS, 300000); // 5 minutes
        event->SchedulePhase(PHASE_MAIN_ERUPTION, 600000); // 10 minutes
        event->SchedulePhase(PHASE_AFTERMATH, 900000); // 15 minutes
    }
    
    void OnEventUpdate(GeologicalEvent* event, uint32 diff) override {
        switch (event->GetCurrentPhase()) {
            case PHASE_PRECURSORS:
                // Minor earthquakes
                TriggerMinorEarthquakes(event->GetEpicenter(), 5.0f);
                // Gas emissions
                EmitVolcanicGases(event->GetEpicenter());
                break;
                
            case PHASE_MAIN_ERUPTION:
                // Massive lava flow
                CreateLavaFlow(event->GetEpicenter(), 2000.0f);
                // Pyroclastic flows
                CreatePyroclasticFlow(event->GetEpicenter(), 45.0f);
                // Ash cloud
                CreateAshPlume(event->GetEpicenter(), 15000.0f);
                break;
                
            case PHASE_AFTERMATH:
                // Cooling lava
                CoolLavaFlows(diff);
                // Ash settlement
                SettleAsh(diff);
                break;
        }
    }
};
```

---

## Implications for BlueMarble

### Recommended Implementation Strategy

**Phase 1: Core Infrastructure (Months 1-3)**
```cpp
// Adopt TrinityCore's proven patterns

1. Dual-Server Architecture
   - AuthServer for researcher authentication
   - WorldServer for geological simulation
   
2. Database Design
   - Follow three-database model
   - Use prepared statements
   - Implement async database operations
   
3. Basic Network Protocol
   - Opcode-based message routing
   - Binary packet serialization
   - Packet handler registration system
```

**Phase 2: Spatial Systems (Months 4-6)**
```cpp
4. Grid-Based World
   - Adapt grid system for planetary scale
   - Implement spatial partitioning
   - Optimize for geological queries
   
5. Interest Management
   - Area-of-interest system
   - Dynamic grid loading/unloading
   - Efficient entity queries
```

**Phase 3: Content Framework (Months 7-9)**
```cpp
6. Scripting System
   - C++ scripting framework
   - Geological event scripts
   - Research activity scripts
   
7. Database Content
   - Formation templates
   - Event definitions
   - Researcher progression
```

---

## References

### Official Sources

1. **TrinityCore GitHub** - https://github.com/TrinityCore/TrinityCore
   - Main repository
   - Documentation wiki
   - Developer forums

2. **TrinityCore Documentation** - https://trinitycore.atlassian.net/wiki/
   - Installation guides
   - Database documentation
   - API reference

### Related Research Documents

- [wow-emulator-architecture-networking.md](../topics/wow-emulator-architecture-networking.md)
- [game-dev-analysis-world-of-warcraft-programming.md](./game-dev-analysis-world-of-warcraft-programming.md)
- [game-dev-analysis-wowdev-wiki-protocol.md](./game-dev-analysis-wowdev-wiki-protocol.md)

### Open Source Projects

1. **CMaNGOS** - https://github.com/cmangos/
2. **AzerothCore** - https://github.com/azerothcore/azerothcore-wotlk

---

**Document Status:** Complete  
**Last Updated:** 2025-01-17  
**Lines:** 1100+  
**Discovered From:** World of Warcraft Programming (Topic 25)  
**Next Steps:**
- Analyze CMaNGOS for architectural comparisons
- Study AzerothCore's modular plugin system
- Prototype BlueMarble server using TrinityCore patterns
