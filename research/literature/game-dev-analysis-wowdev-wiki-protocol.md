# wowdev.wiki - WoW Network Protocol Documentation Analysis

---
title: wowdev.wiki Protocol Documentation - Network Protocol Design for BlueMarble
date: 2025-01-17
tags: [networking, protocol, mmorpg, packet-design, opcodes, documentation]
status: complete
priority: critical
parent-research: online-game-dev-resources.md
discovered-from: World of Warcraft Programming (Topic 25)
related-documents: [wow-emulator-architecture-networking.md, game-dev-analysis-world-of-warcraft-programming.md]
---

**Source:** wowdev.wiki (https://wowdev.wiki/)  
**Category:** MMORPG Development - Network Protocol Design  
**Priority:** Critical  
**Status:** ✅ Complete  
**Discovered From:** World of Warcraft Programming analysis (Topic 25)  
**Lines:** 450+  
**Related Sources:** TrinityCore, CMaNGOS, AzerothCore, WoW Emulator Architecture document

---

## Executive Summary

wowdev.wiki is a comprehensive community-maintained documentation of World of Warcraft's network protocol, file formats, and client-server communication patterns. This resource represents decades of reverse engineering effort, providing detailed specifications for opcodes, packet structures, authentication flows, and data serialization that enable private server emulation.

**Key Value for BlueMarble:**
- Battle-tested binary protocol design patterns for real-time MMORPGs
- Opcode-based message routing architecture scalable to hundreds of message types
- Packet structure optimization techniques for bandwidth efficiency
- Authentication and encryption patterns for secure client-server communication
- Lessons learned from 15+ years of WoW protocol evolution across multiple expansions

**Critical Insights:**
1. **Opcode Architecture:** Extensible message-type system supporting 800+ unique operations
2. **Binary Protocol:** Compact binary serialization reduces bandwidth by 60-80% vs JSON/XML
3. **Header Encryption:** Selective encryption of packet headers while keeping payloads readable for debugging
4. **Version Compatibility:** Protocol versioning strategies across 9+ major game expansions
5. **Client-Server Contract:** Clear separation of client-predicted and server-authoritative data

---

## Part I: Protocol Architecture Fundamentals

### 1. Opcode-Based Message System

**Overview:**

WoW uses a numerical opcode (operation code) system to identify packet types, similar to network protocols like TCP/IP. Each unique client-server interaction has a dedicated opcode.

**Opcode Categories:**

```cpp
// Client to Server (CMSG) - Approximately 400 opcodes
enum ClientOpcodes {
    CMSG_AUTH_SESSION        = 0x1ED,  // Initial authentication
    CMSG_CHAR_ENUM           = 0x037,  // Request character list
    CMSG_PLAYER_LOGIN        = 0x03D,  // Enter world with character
    CMSG_LOGOUT_REQUEST      = 0x04B,  // Request logout
    
    // Movement opcodes (20+ variations)
    CMSG_MOVE_START_FORWARD  = 0x0B1,
    CMSG_MOVE_START_BACKWARD = 0x0B2,
    CMSG_MOVE_STOP           = 0x0B3,
    CMSG_MOVE_JUMP           = 0x0BB,
    CMSG_MOVE_FALL_LAND      = 0x0C7,
    
    // Combat opcodes
    CMSG_CAST_SPELL          = 0x12E,
    CMSG_CANCEL_CAST         = 0x12F,
    CMSG_ATTACKSWING         = 0x141,
    CMSG_ATTACKSTOP          = 0x142,
    
    // Chat opcodes
    CMSG_MESSAGECHAT         = 0x095,
    CMSG_JOIN_CHANNEL        = 0x097,
    CMSG_LEAVE_CHANNEL       = 0x098,
    
    // Trading and economy
    CMSG_INITIATE_TRADE      = 0x116,
    CMSG_CANCEL_TRADE        = 0x11C,
    CMSG_SET_TRADE_GOLD      = 0x11F,
    
    // ... 380+ more opcodes
};

// Server to Client (SMSG) - Approximately 400 opcodes
enum ServerOpcodes {
    SMSG_AUTH_CHALLENGE      = 0x1EC,  // Challenge for authentication
    SMSG_AUTH_RESPONSE       = 0x1EE,  // Auth result
    SMSG_CHAR_ENUM           = 0x03B,  // Character list response
    SMSG_LOGIN_VERIFY_WORLD  = 0x236,  // Confirm world entry
    
    // Object updates (critical for world state)
    SMSG_UPDATE_OBJECT       = 0x0A9,  // Entity state changes
    SMSG_COMPRESSED_UPDATE   = 0x1F6,  // Compressed entity updates
    SMSG_DESTROY_OBJECT      = 0x0AA,  // Entity removal
    
    // Combat feedback
    SMSG_SPELL_START         = 0x131,  // Spell cast begins
    SMSG_SPELL_GO            = 0x132,  // Spell executes
    SMSG_ATTACKSTART         = 0x143,  // Combat initiated
    SMSG_ATTACKSTOP          = 0x144,  // Combat ended
    
    // Chat and social
    SMSG_MESSAGECHAT         = 0x096,  // Chat message broadcast
    SMSG_NOTIFICATION        = 0x1CB,  // System notification
    
    // ... 380+ more opcodes
};
```

**Opcode Design Patterns:**

```cpp
// Pattern 1: Request-Response Pairs
// Client requests data, server responds
CMSG_CHAR_ENUM (0x037) → SMSG_CHAR_ENUM (0x03B)
CMSG_NAME_QUERY (0x050) → SMSG_NAME_QUERY_RESPONSE (0x051)
CMSG_ITEM_QUERY_SINGLE (0x056) → SMSG_ITEM_QUERY_SINGLE_RESPONSE (0x058)

// Pattern 2: Server-Initiated Broadcasts
// Server sends without client request (world events, other players' actions)
SMSG_UPDATE_OBJECT (0x0A9)  // Entity state changes
SMSG_MESSAGECHAT (0x096)    // Chat from other players
SMSG_SPELL_GO (0x132)       // Spell casts by others

// Pattern 3: Action-Feedback Pattern
// Client initiates action, server confirms and broadcasts to nearby players
CMSG_CAST_SPELL (0x12E) → SMSG_SPELL_START (0x131) → SMSG_SPELL_GO (0x132)

// Pattern 4: Movement Flooding
// Client sends frequent movement updates, server validates and relays
CMSG_MOVE_* (sent ~10 times/second) → SMSG_MOVE_* (relayed to nearby players)
```

**Benefits for BlueMarble:**

- **Extensibility:** Easy to add new opcodes for geological events (earthquakes, eruptions)
- **Type Safety:** Opcodes prevent misinterpretation of packet intent
- **Routing Efficiency:** Fast dispatch to appropriate handler functions
- **Protocol Evolution:** New opcodes added without breaking old clients (with versioning)
- **Debugging:** Clear packet type identification in network logs

**Implementation Example:**

```cpp
class PacketHandler {
public:
    using HandlerFunc = void (WorldSession::*)(WorldPacket&);
    
    struct OpcodeHandler {
        const char* name;
        HandlerFunc handler;
        OpcodeStatus status;  // ACTIVE, DEPRECATED, UNIMPLEMENTED
        uint32 minClientBuild;
        uint32 maxClientBuild;
    };
    
private:
    std::unordered_map<uint16, OpcodeHandler> mOpcodeTable;
    
public:
    void Initialize() {
        // Register all opcode handlers
        RegisterOpcode(CMSG_PLAYER_LOGIN, "CMSG_PLAYER_LOGIN", 
                      &WorldSession::HandlePlayerLogin, STATUS_ACTIVE, 0, UINT32_MAX);
        RegisterOpcode(CMSG_MOVE_START_FORWARD, "CMSG_MOVE_START_FORWARD",
                      &WorldSession::HandleMoveStartForward, STATUS_ACTIVE, 0, UINT32_MAX);
        RegisterOpcode(CMSG_CAST_SPELL, "CMSG_CAST_SPELL",
                      &WorldSession::HandleCastSpell, STATUS_ACTIVE, 0, UINT32_MAX);
        // ... register 800+ opcodes
    }
    
    bool HandlePacket(WorldSession* session, WorldPacket& packet) {
        uint16 opcode = packet.GetOpcode();
        
        auto it = mOpcodeTable.find(opcode);
        if (it == mOpcodeTable.end()) {
            LOG_ERROR("Unknown opcode: 0x%X", opcode);
            return false;
        }
        
        OpcodeHandler& handler = it->second;
        
        // Version check
        uint32 clientBuild = session->GetClientBuild();
        if (clientBuild < handler.minClientBuild || 
            clientBuild > handler.maxClientBuild) {
            LOG_ERROR("Opcode %s not supported in client build %u", 
                     handler.name, clientBuild);
            return false;
        }
        
        // Status check
        if (handler.status != STATUS_ACTIVE) {
            LOG_WARN("Opcode %s is %s", handler.name, 
                    handler.status == STATUS_DEPRECATED ? "deprecated" : "unimplemented");
        }
        
        // Dispatch to handler
        (session->*handler.handler)(packet);
        return true;
    }
};
```

### 2. Packet Structure and Serialization

**Binary Packet Format:**

```
World Packet Structure (3.3.5a):
┌─────────────────┬──────────────┬─────────────────────────┐
│  Size (2 bytes) │ Opcode (4*)  │   Payload (N bytes)     │
│   (encrypted)   │ (encrypted)  │  (variable, plaintext)  │
└─────────────────┴──────────────┴─────────────────────────┘

*Note: Opcode size varies by expansion (2 bytes in Classic, 4 in WotLK+)

Header Encryption:
- RC4/ARC4 stream cipher initialized from SRP6 session key
- Separate encryption streams for client→server and server→client
- Only size and opcode are encrypted; payload remains plaintext for debugging
```

**Payload Serialization Patterns:**

```cpp
// Pattern 1: Fixed-size primitive types
class WorldPacket {
public:
    // Writing primitives
    WorldPacket& operator<<(uint8 value) {
        Append(&value, sizeof(uint8));
        return *this;
    }
    
    WorldPacket& operator<<(uint32 value) {
        Append(&value, sizeof(uint32));
        return *this;
    }
    
    WorldPacket& operator<<(float value) {
        Append(&value, sizeof(float));
        return *this;
    }
    
    // Reading primitives
    WorldPacket& operator>>(uint8& value) {
        Read(&value, sizeof(uint8));
        return *this;
    }
    
    WorldPacket& operator>>(uint32& value) {
        Read(&value, sizeof(uint32));
        return *this;
    }
};

// Example: Movement packet serialization
void WorldSession::HandleMoveStartForward(WorldPacket& packet) {
    uint64 guid;           // 8 bytes - player GUID
    uint32 moveFlags;      // 4 bytes - movement flags
    uint16 moveFlags2;     // 2 bytes - extended flags
    uint32 moveTime;       // 4 bytes - client timestamp
    float posX, posY, posZ; // 12 bytes - position
    float orientation;     // 4 bytes - facing direction
    
    // Binary deserialization (total: 34 bytes)
    packet >> guid >> moveFlags >> moveFlags2 >> moveTime;
    packet >> posX >> posY >> posZ >> orientation;
    
    // Validation and processing...
}

// Contrast with JSON (would be ~150 bytes):
// {
//   "guid": "0x0000000012345678",
//   "moveFlags": 1,
//   "moveFlags2": 0,
//   "moveTime": 12345678,
//   "position": {"x": 100.5, "y": 200.3, "z": 50.2},
//   "orientation": 3.14
// }
```

**String Serialization:**

```cpp
// Pattern 2: Length-prefixed strings (for variable data)
WorldPacket& WorldPacket::operator<<(const std::string& str) {
    // C-string format: null-terminated, no length prefix
    Append(str.c_str(), str.length());
    Append("\0", 1);
    return *this;
}

WorldPacket& WorldPacket::operator>>(std::string& str) {
    str.clear();
    char c;
    while (Read(&c, 1) && c != '\0') {
        str += c;
    }
    return *this;
}

// Example: Chat message packet
void SendChatMessage(const std::string& message, const std::string& channel) {
    WorldPacket packet(SMSG_MESSAGECHAT);
    packet << uint8(CHAT_MSG_CHANNEL);  // 1 byte
    packet << uint32(LANG_UNIVERSAL);   // 4 bytes
    packet << channel;                  // Variable length + null
    packet << uint64(senderGuid);       // 8 bytes
    packet << uint32(message.length()); // 4 bytes
    packet << message;                  // Variable length + null
    session->SendPacket(&packet);
}
```

**Structured Data Serialization:**

```cpp
// Pattern 3: Nested structures
struct ItemInstance {
    uint64 guid;
    uint32 itemId;
    uint32 randomPropertyId;
    uint32 randomSuffixId;
    uint32 stackCount;
    uint32 durability;
    uint32 maxDurability;
    std::vector<uint32> enchantments; // Variable-length array
};

WorldPacket& operator<<(WorldPacket& packet, const ItemInstance& item) {
    packet << item.guid;
    packet << item.itemId;
    packet << item.randomPropertyId;
    packet << item.randomSuffixId;
    packet << item.stackCount;
    packet << item.durability;
    packet << item.maxDurability;
    
    // Encode array with count prefix
    packet << uint32(item.enchantments.size());
    for (uint32 ench : item.enchantments) {
        packet << ench;
    }
    
    return packet;
}
```

**BlueMarble Application:**

For geological simulation data:

```cpp
// Geological event packet
struct GeologicalEventPacket {
    uint64 eventId;
    uint32 eventType;      // EARTHQUAKE, ERUPTION, TSUNAMI, etc.
    float epicenterX, epicenterY, epicenterZ;
    float magnitude;       // Richter scale or VEI
    uint32 startTime;      // Server timestamp
    uint32 duration;       // Milliseconds
    uint32 affectedRadiusKm;
    
    // Variable data
    std::vector<uint64> affectedZoneIds;
    std::vector<float> seismicWaveform; // Intensity over time
};

// Compact binary: ~50 bytes + variable data
// JSON equivalent: ~300+ bytes
// Bandwidth savings: 80%+ for frequent updates
```

### 3. Update Object System (Delta Compression)

**Overview:**

The `SMSG_UPDATE_OBJECT` packet is WoW's most critical and complex packet, responsible for synchronizing entity state (players, NPCs, objects) to clients. It uses sophisticated delta compression to minimize bandwidth.

**Update Types:**

```cpp
enum UpdateType {
    UPDATE_VALUES           = 0,  // Field updates only
    UPDATE_MOVEMENT         = 1,  // Position/orientation only
    UPDATE_CREATE_OBJECT    = 2,  // Full object creation
    UPDATE_CREATE_OBJECT2   = 3,  // Create + values
    UPDATE_OUT_OF_RANGE     = 4   // Object no longer visible
};

// Update packet structure
class UpdatePacketBuilder {
public:
    void BuildUpdatePacket(WorldPacket& packet, 
                          const std::vector<Entity*>& entities) {
        packet.Initialize(SMSG_UPDATE_OBJECT);
        packet << uint32(1);  // Number of update blocks
        packet << uint8(0);   // Has transport flag
        
        for (Entity* entity : entities) {
            BuildUpdateBlock(packet, entity);
        }
    }
    
private:
    void BuildUpdateBlock(WorldPacket& packet, Entity* entity) {
        packet << uint8(entity->GetUpdateType());
        packet << entity->GetPackedGUID();
        
        switch (entity->GetUpdateType()) {
            case UPDATE_CREATE_OBJECT:
                BuildCreateBlock(packet, entity);
                break;
            case UPDATE_VALUES:
                BuildValuesBlock(packet, entity);
                break;
            case UPDATE_MOVEMENT:
                BuildMovementBlock(packet, entity);
                break;
            case UPDATE_OUT_OF_RANGE:
                // No additional data needed
                break;
        }
    }
};
```

**Values Update System (Bitmask Delta):**

```cpp
// Entity fields are organized in arrays
enum UnitFields {
    UNIT_FIELD_HEALTH              = 0x006,
    UNIT_FIELD_POWER               = 0x008,  // Mana/Energy/Rage
    UNIT_FIELD_MAXHEALTH           = 0x010,
    UNIT_FIELD_MAXPOWER            = 0x012,
    UNIT_FIELD_LEVEL               = 0x014,
    UNIT_FIELD_FLAGS               = 0x016,
    UNIT_FIELD_AURA                = 0x040,  // 56 aura slots
    UNIT_FIELD_ATTACK_POWER        = 0x0B0,
    // ... 256+ fields per entity type
};

class UpdateValues {
private:
    uint32 mValues[MAX_FIELDS];
    std::bitset<MAX_FIELDS> mChangedBits;
    
public:
    void SetValue(uint16 index, uint32 value) {
        if (mValues[index] != value) {
            mValues[index] = value;
            mChangedBits.set(index);
        }
    }
    
    void BuildUpdatePacket(WorldPacket& packet) {
        // Count changed fields
        uint8 changeCount = mChangedBits.count();
        
        if (changeCount == 0)
            return;
        
        // Write bitmask (indicates which fields changed)
        uint8 blockCount = (MAX_FIELDS + 31) / 32;
        packet << uint8(blockCount);
        
        for (uint8 i = 0; i < blockCount; ++i) {
            uint32 mask = 0;
            for (uint8 bit = 0; bit < 32; ++bit) {
                uint16 fieldIndex = i * 32 + bit;
                if (fieldIndex < MAX_FIELDS && mChangedBits[fieldIndex]) {
                    mask |= (1 << bit);
                }
            }
            packet << mask;
        }
        
        // Write only changed values
        for (uint16 i = 0; i < MAX_FIELDS; ++i) {
            if (mChangedBits[i]) {
                packet << mValues[i];
            }
        }
        
        // Clear changed bits
        mChangedBits.reset();
    }
};

// Example: Health change update
// Only 6 bytes sent: [bitmask: 2 bytes] + [health value: 4 bytes]
// vs. sending all 256 fields: 1024+ bytes
// Bandwidth reduction: 99.4%
```

**Movement Update Format:**

```cpp
struct MovementInfo {
    uint32 moveFlags;          // Walking, swimming, flying, etc.
    uint16 moveFlags2;         // Extended flags
    uint32 moveTime;           // Client timestamp
    float position[3];         // X, Y, Z
    float orientation;         // Facing direction
    
    // Optional fields (based on flags)
    float pitch;               // If swimming/flying
    uint32 fallTime;           // If falling
    float jumpVelocity;        // If jumping
    float swimPitch;           // If swimming
    // ... 20+ optional fields
};

void BuildMovementUpdate(WorldPacket& packet, const MovementInfo& info) {
    packet << info.moveFlags;
    packet << info.moveFlags2;
    packet << info.moveTime;
    packet << info.position[0] << info.position[1] << info.position[2];
    packet << info.orientation;
    
    // Conditional serialization based on flags
    if (info.moveFlags & MOVEFLAG_SWIMMING) {
        packet << info.swimPitch;
    }
    if (info.moveFlags & MOVEFLAG_FALLING) {
        packet << info.fallTime;
        packet << info.jumpVelocity;
    }
    // ... handle other conditional fields
}
```

**BlueMarble Application:**

For geological entities (ore deposits, geological formations):

```cpp
enum GeologicalFields {
    GEO_FIELD_TYPE              = 0x00,  // Rock type, ore type
    GEO_FIELD_COMPOSITION       = 0x01,  // Mineral composition
    GEO_FIELD_AGE_MILLIONS_YRS  = 0x02,  // Formation age
    GEO_FIELD_TEMPERATURE_K     = 0x03,  // Current temperature
    GEO_FIELD_PRESSURE_PA       = 0x04,  // Pressure
    GEO_FIELD_DENSITY_KGM3      = 0x05,  // Density
    GEO_FIELD_POROSITY_PCT      = 0x06,  // Porosity
    GEO_FIELD_PERMEABILITY      = 0x07,  // Fluid permeability
    GEO_FIELD_STRESS_VECTOR     = 0x08,  // Tectonic stress (3D)
    GEO_FIELD_EROSION_RATE      = 0x0C,  // Erosion rate
    // ... hundreds of geological properties
};

// Delta updates allow efficient transmission of only changed properties
// Example: Temperature change from geological event
// Send only: [bitmask: 1 byte] + [new temperature: 4 bytes] = 5 bytes
// vs full state: 400+ bytes
```

---

## Part II: Authentication and Security

### 4. SRP6 Authentication Protocol

**Overview:**

WoW uses SRP6 (Secure Remote Password version 6) for authentication, a zero-knowledge proof protocol that never transmits passwords over the network.

**Authentication Flow:**

```
Client                          Auth Server
  |                                  |
  |  1. CMSG_AUTH_LOGON_CHALLENGE    |
  |  [username]                      |
  |--------------------------------->|
  |                                  |
  |  2. SMSG_AUTH_LOGON_CHALLENGE    |
  |  [B, g, N, s]                    |
  |<---------------------------------|
  |                                  |
  | (Client computes A and M1)       |
  |                                  |
  |  3. CMSG_AUTH_LOGON_PROOF        |
  |  [A, M1]                         |
  |--------------------------------->|
  |                                  |
  | (Server verifies M1, computes M2)|
  |                                  |
  |  4. SMSG_AUTH_LOGON_PROOF        |
  |  [M2, SessionKey]                |
  |<---------------------------------|
  |                                  |
  | (Both sides have SessionKey)     |
  |                                  |
```

**SRP6 Implementation Details:**

```cpp
// Server-side SRP6 verification
class SRP6Authentication {
private:
    // Large safe prime (1024 bits)
    static const BigNumber N;
    // Generator
    static const BigNumber g;
    
    BigNumber mSalt;
    BigNumber mVerifier;      // v = g^x mod N, where x = H(s, p)
    BigNumber mPrivateKey;    // b (server's private key)
    BigNumber mPublicKey;     // B = kv + g^b mod N
    BigNumber mSessionKey;    // K (shared session key)
    
public:
    void Initialize(const std::string& username, const std::string& password) {
        // Generate salt
        mSalt = BigNumber::Random(32);
        
        // Compute password hash: x = H(s, H(username:password))
        std::string credentials = username + ":" + password;
        std::transform(credentials.begin(), credentials.end(), 
                      credentials.begin(), ::toupper);
        BigNumber x = BigNumber::FromHash(SHA1(mSalt, SHA1(credentials)));
        
        // Compute verifier: v = g^x mod N
        mVerifier = g.ModExp(x, N);
        
        // Store in database: (username, s, v)
        // Password is NOT stored!
    }
    
    void GenerateChallenge() {
        // Generate server's private key
        mPrivateKey = BigNumber::Random(32);
        
        // Compute public key: B = kv + g^b mod N
        BigNumber k = BigNumber::FromHash(SHA1(N, g));
        BigNumber gb = g.ModExp(mPrivateKey, N);
        mPublicKey = (k * mVerifier + gb) % N;
    }
    
    bool VerifyProof(const BigNumber& clientPublicKey,  // A
                     const BigNumber& clientProof) {    // M1
        // Compute u = H(A, B)
        BigNumber u = BigNumber::FromHash(SHA1(clientPublicKey, mPublicKey));
        
        // Compute session key: S = (A * v^u)^b mod N
        BigNumber S = (clientPublicKey * mVerifier.ModExp(u, N))
                      .ModExp(mPrivateKey, N);
        
        // Derive session key: K = H(S)
        mSessionKey = BigNumber::FromHash(SHA1(S));
        
        // Compute expected client proof: M1 = H(H(N) xor H(g), H(username), s, A, B, K)
        BigNumber expectedM1 = ComputeM1(clientPublicKey, mPublicKey, mSessionKey);
        
        // Verify client proof
        return clientProof == expectedM1;
    }
    
    BigNumber ComputeServerProof() {
        // M2 = H(A, M1, K)
        return BigNumber::FromHash(SHA1(clientPublicKey, clientProof, mSessionKey));
    }
};
```

**Security Benefits:**

1. **No Password Transmission:** Password never sent over network, even encrypted
2. **Server Compromise Protection:** Server doesn't store passwords, only verifier
3. **Replay Attack Prevention:** Each authentication generates new session key
4. **Man-in-the-Middle Resistance:** Both parties prove knowledge without revealing secrets
5. **Forward Secrecy:** Session keys are ephemeral, compromise doesn't affect past sessions

**BlueMarble Application:**

```cpp
// Adapt SRP6 for scientific collaboration authentication
class ScientificAuthSystem {
public:
    // Authenticate researchers with institutional credentials
    bool AuthenticateResearcher(const std::string& institutionId,
                               const std::string& researcherId) {
        // Use SRP6 to verify without exposing credentials
        // Session key can be used to sign research data submissions
        return mSRP6.VerifyProof(clientPublicKey, clientProof);
    }
    
    // Secure data submission with session-based signatures
    void SubmitResearchData(const GeologicalData& data) {
        // Sign data with session key derived from SRP6
        Signature sig = Sign(data, mSRP6.GetSessionKey());
        UploadToServer(data, sig);
    }
};
```

### 5. Header Encryption (RC4)

**Purpose:**

Encrypt packet headers (size + opcode) to prevent:
- Traffic analysis (observers can't see what actions players are taking)
- Packet injection (attackers can't craft valid packet headers)
- Replay attacks (headers are part of stream cipher, position-dependent)

**Implementation:**

```cpp
class PacketCrypt {
private:
    // RC4 (ARC4) stream cipher state
    uint8 mState[256];
    uint8 mIndex1;
    uint8 mIndex2;
    
public:
    void Initialize(const uint8* sessionKey, size_t keyLen) {
        // KSA (Key Scheduling Algorithm)
        for (int i = 0; i < 256; ++i) {
            mState[i] = i;
        }
        
        uint8 j = 0;
        for (int i = 0; i < 256; ++i) {
            j = (j + mState[i] + sessionKey[i % keyLen]) % 256;
            std::swap(mState[i], mState[j]);
        }
        
        mIndex1 = 0;
        mIndex2 = 0;
        
        // Drop first 1024 bytes (RC4 weaknesses in early keystream)
        uint8 discard;
        for (int i = 0; i < 1024; ++i) {
            GetByte(&discard);
        }
    }
    
    void EncryptDecrypt(uint8* data, size_t len) {
        for (size_t i = 0; i < len; ++i) {
            data[i] ^= GetByte();
        }
    }
    
private:
    uint8 GetByte() {
        // PRGA (Pseudo-Random Generation Algorithm)
        mIndex1 = (mIndex1 + 1) % 256;
        mIndex2 = (mIndex2 + mState[mIndex1]) % 256;
        std::swap(mState[mIndex1], mState[mIndex2]);
        
        uint8 t = (mState[mIndex1] + mState[mIndex2]) % 256;
        return mState[t];
    }
};

// Separate ciphers for each direction
class WorldSession {
private:
    PacketCrypt mClientCrypt;  // For incoming packets
    PacketCrypt mServerCrypt;  // For outgoing packets
    
public:
    void InitializeEncryption(const uint8* sessionKey, size_t keyLen) {
        // Derive separate keys for each direction using HMAC
        uint8 clientKey[20];
        uint8 serverKey[20];
        
        HMAC_SHA1(sessionKey, keyLen, 
                 (const uint8*)"client to server", 16, clientKey);
        HMAC_SHA1(sessionKey, keyLen,
                 (const uint8*)"server to client", 16, serverKey);
        
        mClientCrypt.Initialize(clientKey, 20);
        mServerCrypt.Initialize(serverKey, 20);
    }
    
    void SendPacket(WorldPacket* packet) {
        // Encrypt header only
        uint8 header[4];
        *(uint16*)&header[0] = packet->size();
        *(uint16*)&header[2] = packet->GetOpcode();
        
        mServerCrypt.EncryptDecrypt(header, 4);
        
        // Send encrypted header + plaintext payload
        SendBytes(header, 4);
        SendBytes(packet->contents(), packet->size());
    }
};
```

**Why Not Encrypt Payload?**

- **Performance:** Encrypting megabytes of world data would be CPU-intensive
- **Debugging:** Plaintext payloads allow server logs to inspect packet contents
- **Security Trade-off:** Header encryption prevents most attacks; full encryption adds marginal benefit
- **Legacy Compatibility:** Early WoW versions used this approach, maintained for compatibility

**BlueMarble Considerations:**

```cpp
// For sensitive geological data, consider full encryption
class SecureGeologicalPacket {
public:
    void SendEncrypted(WorldPacket* packet) {
        // For proprietary research data, encrypt everything
        if (packet->ContainsPro proprietaryData()) {
            // Use modern cipher (AES-256-GCM)
            EncryptFull(packet);
        } else {
            // Public geological data: header encryption only
            EncryptHeader(packet);
        }
    }
};
```

---

## Part III: Protocol Evolution and Versioning

### 6. Multi-Version Support

**Challenge:**

WoW has released 9+ major expansions over 18 years. Each expansion modifies the protocol:
- New opcodes added
- Packet structures changed
- Authentication flow updated
- Encryption algorithms enhanced

**Version Detection:**

```cpp
enum ClientBuild {
    BUILD_CLASSIC_1_12_1    = 5875,
    BUILD_TBC_2_4_3         = 8606,
    BUILD_WOTLK_3_3_5a      = 12340,
    BUILD_CATACLYSM_4_3_4   = 15595,
    BUILD_MISTS_5_4_8       = 18414,
    BUILD_WOD_6_2_4         = 20779,
    BUILD_LEGION_7_3_5      = 26365,
    BUILD_BFA_8_3_0         = 33062,
    BUILD_SHADOWLANDS_9_1_5 = 40772,
};

class ProtocolVersionManager {
public:
    bool IsSupported(uint32 clientBuild) {
        return clientBuild == BUILD_WOTLK_3_3_5a;  // Example: support one version
    }
    
    OpcodeTable* GetOpcodeTable(uint32 clientBuild) {
        switch (clientBuild) {
            case BUILD_CLASSIC_1_12_1:
                return &mClassicOpcodes;
            case BUILD_WOTLK_3_3_5a:
                return &mWotLKOpcodes;
            // ... handle each version
            default:
                return nullptr;
        }
    }
};
```

**Opcode Mapping Across Versions:**

```cpp
// Same logical opcode, different numerical values
struct OpcodeMapping {
    uint16 classic;
    uint16 tbc;
    uint16 wotlk;
    const char* name;
};

const OpcodeMapping OPCODE_PLAYER_LOGIN = {
    .classic = 0x03D,
    .tbc     = 0x03D,
    .wotlk   = 0x03D,
    .name    = "CMSG_PLAYER_LOGIN"
};

const OpcodeMapping OPCODE_MOVE_START_FORWARD = {
    .classic = 0x0B1,
    .tbc     = 0x0B5,  // Changed in TBC!
    .wotlk   = 0x0B1,  // Reverted in WotLK
    .name    = "CMSG_MOVE_START_FORWARD"
};
```

**Backward Compatibility Strategies:**

```cpp
class BackwardCompatibilityLayer {
public:
    // Strategy 1: Version-specific packet serialization
    void SendPlayerData(WorldSession* session, const Player* player) {
        uint32 build = session->GetClientBuild();
        
        WorldPacket packet(SMSG_UPDATE_OBJECT);
        if (build < BUILD_CATACLYSM_4_3_4) {
            // Pre-Cataclysm format
            SerializePlayerDataOld(packet, player);
        } else {
            // Post-Cataclysm format (more fields)
            SerializePlayerDataNew(packet, player);
        }
        session->SendPacket(&packet);
    }
    
    // Strategy 2: Feature flags
    bool SupportsFeature(WorldSession* session, Feature feature) {
        uint32 build = session->GetClientBuild();
        
        switch (feature) {
            case FEATURE_DUAL_SPEC:
                return build >= BUILD_WOTLK_3_3_5a;
            case FEATURE_TRANSMOGRIFICATION:
                return build >= BUILD_CATACLYSM_4_3_4;
            case FEATURE_WARMODE:
                return build >= BUILD_BFA_8_3_0;
            default:
                return false;
        }
    }
    
    // Strategy 3: Graceful degradation
    void SendSpellEffect(WorldSession* session, Spell* spell) {
        if (SupportsFeature(session, FEATURE_ADVANCED_EFFECTS)) {
            SendAdvancedEffect(session, spell);
        } else {
            SendBasicEffect(session, spell);
        }
    }
};
```

**BlueMarble Application:**

```cpp
// Versioning for geological simulation complexity levels
enum SimulationVersion {
    SIM_VERSION_BASIC    = 1,  // Simple geological processes
    SIM_VERSION_ADVANCED = 2,  // Plate tectonics, erosion
    SIM_VERSION_RESEARCH = 3,  // Full scientific accuracy
};

class GeologicalProtocol {
public:
    void SendGeologicalUpdate(WorldSession* session, const GeologicalRegion& region) {
        SimulationVersion version = session->GetSimulationVersion();
        
        WorldPacket packet(GMSG_GEOLOGICAL_UPDATE);
        
        switch (version) {
            case SIM_VERSION_BASIC:
                // Send simplified data (visual only)
                packet << region.GetBasicVisualData();
                break;
                
            case SIM_VERSION_ADVANCED:
                // Send gameplay-relevant data
                packet << region.GetAdvancedSimulationData();
                break;
                
            case SIM_VERSION_RESEARCH:
                // Send full scientific data with uncertainty estimates
                packet << region.GetFullResearchData();
                packet << region.GetUncertaintyMetrics();
                break;
        }
        
        session->SendPacket(&packet);
    }
};
```

---

## Part IV: Advanced Protocol Patterns

### 7. Interest Management (Area of Interest)

**Problem:**

In a world with thousands of entities, sending updates for ALL entities to ALL players would overwhelm bandwidth and processing.

**Solution:**

Only send updates for entities within a player's "area of interest" (AoI).

**Implementation:**

```cpp
class InterestManager {
private:
    const float AOI_RADIUS = 100.0f;  // meters
    
    struct PlayerInterestSet {
        std::unordered_set<uint64> visibleGuids;
        std::unordered_set<uint64> previousVisibleGuids;
    };
    
    std::unordered_map<uint64, PlayerInterestSet> mPlayerInterests;
    
public:
    void UpdatePlayerInterests(Player* player, const std::vector<Entity*>& nearbyEntities) {
        PlayerInterestSet& interests = mPlayerInterests[player->GetGUID()];
        
        // Swap current to previous
        interests.previousVisibleGuids = interests.visibleGuids;
        interests.visibleGuids.clear();
        
        // Build new interest set
        for (Entity* entity : nearbyEntities) {
            float distance = player->GetDistance(entity);
            if (distance <= AOI_RADIUS) {
                interests.visibleGuids.insert(entity->GetGUID());
            }
        }
        
        // Determine changes
        std::vector<uint64> entered;  // New entities
        std::vector<uint64> left;     // Entities that left AoI
        std::vector<uint64> updated;  // Entities still visible (may have changed)
        
        // Entities that entered AoI
        for (uint64 guid : interests.visibleGuids) {
            if (interests.previousVisibleGuids.find(guid) == 
                interests.previousVisibleGuids.end()) {
                entered.push_back(guid);
            } else {
                updated.push_back(guid);
            }
        }
        
        // Entities that left AoI
        for (uint64 guid : interests.previousVisibleGuids) {
            if (interests.visibleGuids.find(guid) == 
                interests.visibleGuids.end()) {
                left.push_back(guid);
            }
        }
        
        // Send appropriate updates
        SendCreateObjectPackets(player, entered);
        SendUpdateObjectPackets(player, updated);
        SendDestroyObjectPackets(player, left);
    }
};
```

**Optimization: Spatial Partitioning:**

```cpp
class SpatialGrid {
private:
    const float CELL_SIZE = 50.0f;  // 50m cells
    
    struct Cell {
        std::vector<Entity*> entities;
        std::vector<Player*> players;
    };
    
    std::unordered_map<uint64, Cell> mCells;
    
    uint64 GetCellKey(float x, float y) {
        int cellX = (int)(x / CELL_SIZE);
        int cellY = (int)(y / CELL_SIZE);
        return ((uint64)cellX << 32) | (uint64)cellY;
    }
    
public:
    void UpdateEntity(Entity* entity) {
        uint64 cellKey = GetCellKey(entity->GetX(), entity->GetY());
        mCells[cellKey].entities.push_back(entity);
    }
    
    std::vector<Entity*> GetNearbyEntities(Player* player, float radius) {
        std::vector<Entity*> result;
        
        // Calculate which cells are within radius
        int cellsX = (int)(radius / CELL_SIZE) + 1;
        int cellsY = (int)(radius / CELL_SIZE) + 1;
        
        uint64 playerCell = GetCellKey(player->GetX(), player->GetY());
        
        // Check surrounding cells (O(9) instead of O(N))
        for (int dx = -cellsX; dx <= cellsX; ++dx) {
            for (int dy = -cellsY; dy <= cellsY; ++dy) {
                uint64 checkCell = playerCell + ((uint64)dx << 32) + dy;
                if (mCells.find(checkCell) != mCells.end()) {
                    result.insert(result.end(), 
                                mCells[checkCell].entities.begin(),
                                mCells[checkCell].entities.end());
                }
            }
        }
        
        return result;
    }
};
```

**BlueMarble Application:**

```cpp
// Hierarchical interest management for geological scale
class GeologicalInterestManager {
private:
    // Multiple scales of interest
    const float MICRO_SCALE_M     = 1.0f;      // Rock samples
    const float MESO_SCALE_M      = 100.0f;    // Outcrops
    const float MACRO_SCALE_KM    = 10.0f;     // Geological formations
    const float REGIONAL_SCALE_KM = 100.0f;    // Tectonic plates
    const float GLOBAL_SCALE      = INFINITY;   // Planetary events
    
public:
    void UpdateGeologicalInterests(Player* player) {
        // Always receive global events (earthquakes, eruptions)
        SendGlobalEvents(player);
        
        // Regional events based on continent
        SendRegionalEvents(player, player->GetContinent());
        
        // Local formations based on proximity
        SendLocalFormations(player, MACRO_SCALE_KM);
        
        // Detailed features when close
        if (player->IsInResearchMode()) {
            SendDetailedFeatures(player, MESO_SCALE_M);
            
            // Microscopic samples when examining
            if (player->IsExamining()) {
                SendMicroscopicData(player, MICRO_SCALE_M);
            }
        }
    }
};
```

---

## Part V: Lessons Learned and Best Practices

### 8. Protocol Design Principles

**Principle 1: Versioning from Day One**

```cpp
// Always include version in packets
struct PacketHeader {
    uint16 size;
    uint16 opcode;
    uint8  protocolVersion;  // Critical for future updates
};

// Example: Handle version differences gracefully
void HandlePacket(WorldSession* session, PacketHeader header, const uint8* payload) {
    if (header.protocolVersion != CURRENT_PROTOCOL_VERSION) {
        // Try compatibility layer
        if (!HandleLegacyPacket(session, header, payload)) {
            LOG_ERROR("Unsupported protocol version: %u", header.protocolVersion);
            session->Disconnect();
        }
    } else {
        // Current version handling
        HandleCurrentPacket(session, header, payload);
    }
}
```

**Principle 2: Extensible Opcodes**

```cpp
// Design opcode space for growth
enum OpcodeSpace {
    OPCODE_SPACE_CORE       = 0x0000,  // 0x0000-0x0FFF: Core gameplay
    OPCODE_SPACE_COMBAT     = 0x1000,  // 0x1000-0x1FFF: Combat
    OPCODE_SPACE_SOCIAL     = 0x2000,  // 0x2000-0x2FFF: Social
    OPCODE_SPACE_ECONOMY    = 0x3000,  // 0x3000-0x3FFF: Economy
    OPCODE_SPACE_CUSTOM     = 0xE000,  // 0xE000-0xEFFF: Server-specific
    OPCODE_SPACE_DEPRECATED = 0xF000,  // 0xF000-0xFFFF: Deprecated opcodes
};

// Leave gaps for future opcodes
enum CoreOpcodes {
    CMSG_PLAYER_LOGIN       = 0x0010,
    // Reserve 0x0011-0x001F for auth-related
    CMSG_PLAYER_LOGOUT      = 0x0020,
    // Reserve 0x0021-0x002F for session-related
};
```

**Principle 3: Favor Binary Over Text**

```cpp
// Bandwidth comparison for player position update
struct BinaryPosition {
    float x, y, z;        // 12 bytes
    uint16 orientation;   // 2 bytes (quantized: 0-65535 = 0-360°)
};  // Total: 14 bytes

// JSON equivalent
// {"x": 1234.56, "y": 789.01, "z": 234.56, "o": 3.14}
// Total: ~60 bytes (4.3x larger)

// For 100 position updates/second from 1000 players:
// Binary: 14 * 100 * 1000 = 1.4 MB/s
// JSON:   60 * 100 * 1000 = 6.0 MB/s
// Savings: 4.6 MB/s = 37 GB/hour
```

**Principle 4: Delta Compression**

```cpp
// Only send what changed
class DeltaEncoder {
public:
    template<typename T>
    void EncodeField(WorldPacket& packet, const T& oldValue, const T& newValue, 
                     uint16 fieldId) {
        if (oldValue != newValue) {
            packet << uint16(fieldId);
            packet << newValue;
        }
    }
    
    void EncodeEntity(WorldPacket& packet, const Entity& oldState, 
                     const Entity& newState) {
        uint16 changeCount = 0;
        size_t countPos = packet.wpos();
        packet << changeCount;  // Placeholder
        
        // Check each field
        if (oldState.health != newState.health) {
            packet << uint16(FIELD_HEALTH);
            packet << newState.health;
            ++changeCount;
        }
        
        if (oldState.position != newState.position) {
            packet << uint16(FIELD_POSITION);
            packet << newState.position;
            ++changeCount;
        }
        
        // Write actual change count
        packet.put(countPos, changeCount);
    }
};
```

**Principle 5: Separate Concerns**

```cpp
// Separate authentication from gameplay
// Different servers, different protocols, different security requirements

// Auth Server: Focus on security
class AuthServer {
    // Heavy cryptography (SRP6, strong password hashing)
    // Rate limiting (prevent brute force)
    // Audit logging (track login attempts)
    // No game state
};

// World Server: Focus on performance
class WorldServer {
    // Fast packet processing (100,000+ packets/second)
    // Game state management
    // Assumes authenticated session (trusts auth server)
    // Optimized for throughput over security
};
```

### 9. Common Pitfalls and Solutions

**Pitfall 1: No Rate Limiting**

```cpp
// Problem: Client can flood server with packets
// Solution: Per-session rate limiting

class RateLimiter {
private:
    std::unordered_map<uint16, RateLimit> mOpcodeLimits;
    
    struct RateLimit {
        uint32 maxPerSecond;
        uint32 currentCount;
        uint32 lastResetTime;
    };
    
public:
    void Initialize() {
        // Set limits per opcode type
        mOpcodeLimits[CMSG_MOVE_START_FORWARD] = {100, 0, 0};  // 100/sec
        mOpcodeLimits[CMSG_CAST_SPELL] = {10, 0, 0};           // 10/sec
        mOpcodeLimits[CMSG_MESSAGECHAT] = {5, 0, 0};           // 5/sec
    }
    
    bool CheckRate(uint16 opcode) {
        uint32 now = GetCurrentTime();
        RateLimit& limit = mOpcodeLimits[opcode];
        
        // Reset counter if second has passed
        if (now - limit.lastResetTime >= 1000) {
            limit.currentCount = 0;
            limit.lastResetTime = now;
        }
        
        // Check limit
        if (limit.currentCount >= limit.maxPerSecond) {
            LOG_WARN("Rate limit exceeded for opcode 0x%X", opcode);
            return false;
        }
        
        ++limit.currentCount;
        return true;
    }
};
```

**Pitfall 2: No Input Validation**

```cpp
// Problem: Trust client data (exploitable)
// Solution: Server-side validation

void HandleMovePacket(WorldPacket& packet) {
    MovementInfo info;
    packet >> info;
    
    // ALWAYS validate client input!
    
    // Validate position (in world bounds?)
    if (!IsValidPosition(info.position)) {
        LOG_ERROR("Invalid position from client");
        KickPlayer();
        return;
    }
    
    // Validate speed (not speedhacking?)
    float distance = GetDistance(player->GetPosition(), info.position);
    float maxDistance = player->GetSpeed() * deltaTime * 1.1f;  // 10% tolerance
    if (distance > maxDistance) {
        LOG_ERROR("Speedhack detected");
        KickPlayer();
        return;
    }
    
    // Validate flags (legitimate combination?)
    if ((info.flags & MOVEFLAG_FLYING) && !player->CanFly()) {
        LOG_ERROR("Invalid movement flags");
        KickPlayer();
        return;
    }
    
    // Only then apply movement
    player->SetPosition(info.position);
}
```

**Pitfall 3: No Packet Size Limits**

```cpp
// Problem: Malicious client sends gigantic packet
// Solution: Hard limits on packet size

class PacketReader {
private:
    const size_t MAX_PACKET_SIZE = 10 * 1024;  // 10 KB limit
    
public:
    bool ReadPacket(Socket* socket) {
        uint16 size;
        if (!socket->Read(&size, sizeof(size))) {
            return false;
        }
        
        // Check size limit
        if (size > MAX_PACKET_SIZE) {
            LOG_ERROR("Packet size %u exceeds limit %zu", size, MAX_PACKET_SIZE);
            socket->Disconnect();
            return false;
        }
        
        // Safe to allocate
        uint8* buffer = new uint8[size];
        if (!socket->Read(buffer, size)) {
            delete[] buffer;
            return false;
        }
        
        // Process packet...
        delete[] buffer;
        return true;
    }
};
```

---

## Implications for BlueMarble MMORPG

### Recommended Protocol Architecture

```cpp
// BlueMarble Network Protocol Design

// 1. Opcode Space Allocation
enum BlueMarbleOpcodes {
    // Core (0x0000-0x0FFF)
    GMSG_AUTH_CHALLENGE         = 0x0001,
    GMSG_PLAYER_LOGIN           = 0x0010,
    GMSG_PLAYER_LOGOUT          = 0x0011,
    
    // Geological Events (0x1000-0x1FFF)
    GMSG_EARTHQUAKE_START       = 0x1000,
    GMSG_VOLCANIC_ERUPTION      = 0x1010,
    GMSG_TSUNAMI_WARNING        = 0x1020,
    GMSG_PLATE_MOVEMENT         = 0x1030,
    
    // Research Operations (0x2000-0x2FFF)
    GMSG_CONDUCT_SURVEY         = 0x2000,
    GMSG_COLLECT_SAMPLE         = 0x2010,
    GMSG_ANALYZE_DATA           = 0x2020,
    GMSG_PUBLISH_FINDINGS       = 0x2030,
    
    // World State (0x3000-0x3FFF)
    GMSG_GEOLOGICAL_UPDATE      = 0x3000,
    GMSG_CLIMATE_UPDATE         = 0x3010,
    GMSG_RESOURCE_UPDATE        = 0x3020,
    
    // Social/Collaboration (0x4000-0x4FFF)
    GMSG_FORM_RESEARCH_TEAM     = 0x4000,
    GMSG_SHARE_DATA             = 0x4010,
    GMSG_COLLABORATE            = 0x4020,
};

// 2. Packet Structure
struct GeologicalPacket {
    uint16 size;            // 2 bytes
    uint16 opcode;          // 2 bytes
    uint8  version;         // 1 byte (protocol version)
    uint8  priority;        // 1 byte (0=normal, 1=high, 2=critical)
    uint32 timestamp;       // 4 bytes (server timestamp)
    uint8  payload[MAX_PAYLOAD_SIZE];
};

// 3. Authentication
// Use SRP6 for researcher authentication
// Session keys for signing research data
// Institutional affiliation verification

// 4. Interest Management
// Hierarchical AoI:
// - Global: Major geological events (everyone)
// - Regional: Continental-scale processes
// - Local: Nearby formations and resources
// - Microscopic: Sample analysis (research mode)

// 5. Delta Compression
// Critical for geological data:
// - Most properties change slowly (millions of years)
// - Only send changed fields
// - Compress time-series data

// 6. Binary Serialization
// Scientific data in compact binary format
// ~90% bandwidth savings vs JSON
// Essential for real-time geological simulation updates
```

---

## References

### Official Documentation

1. **wowdev.wiki** - https://wowdev.wiki/
   - Protocol documentation
   - Opcode listings
   - Packet structures
   - File format specifications

2. **WowPacketParser** - https://github.com/TrinityCore/WowPacketParser
   - Packet capture and analysis tool
   - Generated protocol documentation

### Related Research Documents

- [wow-emulator-architecture-networking.md](../topics/wow-emulator-architecture-networking.md) - Server architecture overview
- [game-dev-analysis-world-of-warcraft-programming.md](./game-dev-analysis-world-of-warcraft-programming.md) - WoW programming analysis
- [game-dev-analysis-01-game-programming-cpp.md](./game-dev-analysis-01-game-programming-cpp.md) - C++ programming foundations

### Open Source Projects

1. **TrinityCore** - https://github.com/TrinityCore/TrinityCore
   - Reference implementation of WoW protocol
   
2. **CMaNGOS** - https://github.com/cmangos/
   - Clean protocol implementation

3. **AzerothCore** - https://github.com/azerothcore/azerothcore-wotlk
   - Modern C++ protocol implementation

---

**Document Status:** Complete  
**Last Updated:** 2025-01-17  
**Lines:** 1450+  
**Discovered From:** World of Warcraft Programming (Topic 25)  
**Next Steps:**
- Cross-reference with TrinityCore codebase analysis
- Implement prototype BlueMarble protocol based on learnings
- Design geological-specific packet types and structures
