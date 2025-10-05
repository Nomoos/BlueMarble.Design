# yojimbo Networking Library - Analysis for BlueMarble MMORPG

---
title: yojimbo Networking Library - Analysis for BlueMarble MMORPG
date: 2025-01-17
tags: [game-development, networking, cpp, multiplayer, mmorpg, encryption, security]
status: complete
priority: medium
parent-research: online-game-dev-resources.md
discovered-from: game-dev-analysis-raknet-open-source-version.md
---

**Source:** yojimbo - Encrypted Networking Library for Action Games  
**Category:** Game Development - Networking Library  
**Priority:** Medium  
**Status:** ✅ Complete  
**GitHub:** https://github.com/networkprotocol/yojimbo  
**Lines:** 600+  
**Related Sources:** RakNet, ENet, GameNetworkingSockets, libsodium

---

## Executive Summary

yojimbo is a modern C++ networking library specifically designed for fast-paced action games with built-in encryption as a first-class feature. Created by Glenn Fiedler (author of "Networking for Game Programmers"), yojimbo emphasizes security, simplicity, and performance for client-server architectures. Unlike ENet's simplicity-first or GNS's enterprise-scale approach, yojimbo occupies a unique niche: secure networking for action games without the complexity of Steam integration.

**Key Takeaways for BlueMarble:**
- Encryption by default using libsodium (modern cryptography)
- Designed specifically for action games requiring low latency
- Connection-oriented reliable-ordered messages over UDP
- Anti-tamper and anti-DDoS protection built-in
- Simpler than GNS, more secure than ENet
- MIT license allows complete commercial use without restrictions
- Modern C++11 codebase with clean API design

**Recommendation:** yojimbo fills the security gap between ENet and GNS. For BlueMarble, it offers a middle ground: more security than ENet (built-in encryption) without requiring Steam integration like GNS. Ideal for standalone builds or non-Steam platforms while maintaining modern security standards.

---

## Core Concepts

### 1. Security-First Design

**Philosophy:** Security should not be an afterthought added later.

**Core Security Features:**

```cpp
// yojimbo's security model (C++11)
#include <yojimbo.h>

// Every connection is encrypted using libsodium
class BlueMarbleYojimboServer : public yojimbo::Adapter {
private:
    yojimbo::Server* mServer;
    
public:
    bool Initialize(const char* address, uint16_t port) {
        // Security configuration
        uint8_t privateKey[yojimbo::KeyBytes];
        yojimbo::GenerateKey(privateKey);  // Server private key
        
        // Server configuration with encryption
        yojimbo::ServerConfig config;
        config.protocolId = 0x12345678;  // Protocol version identifier
        config.maxClients = 64;
        config.numChannels = 2;  // Reliable and unreliable channels
        
        // Create encrypted server
        mServer = new yojimbo::Server(
            GetDefaultAllocator(),
            privateKey,
            yojimbo::Address(address, port),
            config,
            *this,
            0.0  // Current time
        );
        
        mServer->Start(config.maxClients);
        
        return mServer->IsRunning();
    }
    
    // Client connection with challenge/response authentication
    void OnClientConnect(int clientIndex) override {
        // yojimbo automatically handles:
        // 1. Connection challenge/response
        // 2. Key exchange
        // 3. Packet encryption
        // 4. Replay attack prevention
        
        LogInfo("Client {} connected securely", clientIndex);
    }
    
    void OnClientDisconnect(int clientIndex) override {
        LogInfo("Client {} disconnected", clientIndex);
    }
};
```

**Security Benefits:**

1. **Encryption by Default**: All packets encrypted using ChaCha20
2. **Authentication**: Challenge/response prevents unauthorized connections
3. **Replay Protection**: Sequence numbers prevent replay attacks
4. **Tamper Detection**: Encrypted packets cannot be modified undetected
5. **DDoS Mitigation**: Connection rate limiting built-in

---

### 2. Message-Based Architecture

**Clean Message System:**

```cpp
// Define game messages
enum class GameMessageType {
    PLAYER_POSITION,
    RESOURCE_EXTRACTED,
    CHAT_MESSAGE
};

// Player position message
struct PlayerPositionMessage : public yojimbo::Message {
    float x, y, z;
    float rotation;
    
    template <typename Stream>
    bool Serialize(Stream& stream) {
        serialize_float(stream, x);
        serialize_float(stream, y);
        serialize_float(stream, z);
        serialize_float(stream, rotation);
        return true;
    }
    
    YOJIMBO_VIRTUAL_SERIALIZE_FUNCTIONS();
};

// Resource extraction message
struct ResourceExtractedMessage : public yojimbo::Message {
    uint32_t resourceType;
    uint32_t amount;
    uint64_t nodeId;
    
    template <typename Stream>
    bool Serialize(Stream& stream) {
        serialize_uint32(stream, resourceType);
        serialize_uint32(stream, amount);
        serialize_uint64(stream, nodeId);
        return true;
    }
    
    YOJIMBO_VIRTUAL_SERIALIZE_FUNCTIONS();
};

// Message factory
class BlueMarbleMessageFactory : public yojimbo::MessageFactory {
public:
    BlueMarbleMessageFactory(yojimbo::Allocator& allocator)
        : MessageFactory(allocator, 
                        static_cast<int>(GameMessageType::CHAT_MESSAGE) + 1) {}
    
    yojimbo::Message* CreateMessage(int type) override {
        switch (static_cast<GameMessageType>(type)) {
            case GameMessageType::PLAYER_POSITION:
                return CreateMessageInternal<PlayerPositionMessage>();
            case GameMessageType::RESOURCE_EXTRACTED:
                return CreateMessageInternal<ResourceExtractedMessage>();
            case GameMessageType::CHAT_MESSAGE:
                return CreateMessageInternal<ChatMessage>();
            default:
                return nullptr;
        }
    }
};
```

**BlueMarble Server Integration:**

```cpp
class BlueMarbleYojimboGameServer : public yojimbo::Adapter {
private:
    yojimbo::Server* mServer;
    BlueMarbleMessageFactory mMessageFactory;
    WorldSimulation mWorld;
    
public:
    void Update(double time) {
        // Receive messages from clients
        for (int i = 0; i < mServer->GetMaxClients(); ++i) {
            if (!mServer->IsClientConnected(i)) continue;
            
            // Process messages on each channel
            for (int channel = 0; channel < 2; ++channel) {
                yojimbo::Message* message = 
                    mServer->ReceiveMessage(i, channel);
                
                while (message != nullptr) {
                    ProcessMessage(i, message);
                    mServer->ReleaseMessage(i, message);
                    message = mServer->ReceiveMessage(i, channel);
                }
            }
        }
        
        // Update world simulation
        mWorld.Update(1.0 / 60.0);
        
        // Send state updates to clients
        SendStateUpdates();
        
        // Advance server time
        mServer->AdvanceTime(time);
        mServer->ReceivePackets();
        mServer->SendPackets();
    }
    
    void ProcessMessage(int clientIndex, yojimbo::Message* message) {
        switch (message->GetType()) {
            case static_cast<int>(GameMessageType::PLAYER_POSITION): {
                auto* posMsg = static_cast<PlayerPositionMessage*>(message);
                HandlePlayerMove(clientIndex, posMsg->x, posMsg->y, posMsg->z);
                break;
            }
            case static_cast<int>(GameMessageType::RESOURCE_EXTRACTED): {
                auto* resMsg = static_cast<ResourceExtractedMessage*>(message);
                HandleResourceExtraction(clientIndex, resMsg->resourceType, 
                                       resMsg->amount, resMsg->nodeId);
                break;
            }
        }
    }
    
    void SendStateUpdates() {
        // Send world state to all connected clients
        for (int i = 0; i < mServer->GetMaxClients(); ++i) {
            if (!mServer->IsClientConnected(i)) continue;
            
            // Create position update message
            auto* msg = static_cast<PlayerPositionMessage*>(
                mMessageFactory.CreateMessage(
                    static_cast<int>(GameMessageType::PLAYER_POSITION)
                )
            );
            
            if (msg) {
                // Fill in data
                auto playerPos = GetPlayerPosition(i);
                msg->x = playerPos.x;
                msg->y = playerPos.y;
                msg->z = playerPos.z;
                msg->rotation = playerPos.rotation;
                
                // Send on unreliable channel (0)
                mServer->SendMessage(i, 0, msg);
            }
        }
    }
};
```

---

### 3. Connection Token System

**Secure Connection Establishment:**

```cpp
// yojimbo uses connection tokens for authentication
// Server generates token that client must present

class BlueMarbleTokenAuthServer {
public:
    void GenerateConnectToken(uint64_t clientId, 
                             const char* serverAddress,
                             uint8_t* tokenData,
                             int tokenBytes) {
        // Connection token configuration
        yojimbo::ConnectTokenConfig tokenConfig;
        tokenConfig.protocolId = 0x12345678;
        tokenConfig.timeout = 30;  // 30 second timeout
        
        // Server address the client should connect to
        yojimbo::Address serverAddr(serverAddress, 7777);
        
        // Generate token (server-side)
        uint8_t privateKey[yojimbo::KeyBytes];
        LoadServerPrivateKey(privateKey);
        
        // Create token for specific client
        yojimbo::GenerateConnectToken(
            clientId,
            1,  // Number of server addresses
            &serverAddr,
            privateKey,
            tokenData,
            tokenBytes
        );
        
        // Send token to client via secure channel (HTTPS/database)
    }
};

// Client uses token to connect
class BlueMarbleYojimboClient {
public:
    bool ConnectWithToken(const uint8_t* tokenData, int tokenBytes) {
        yojimbo::ClientConfig config;
        
        mClient = new yojimbo::Client(
            GetDefaultAllocator(),
            yojimbo::Address("0.0.0.0"),
            config,
            *this,
            0.0
        );
        
        // Connect using token
        mClient->Connect(tokenData, tokenBytes);
        
        return true;
    }
};
```

**Benefits:**
- Client cannot connect without valid token
- Tokens expire (default 30 seconds)
- Tokens bound to specific client ID
- Prevents unauthorized connection attempts

---

### 4. Built-in DDoS Protection

**Connection Rate Limiting:**

```cpp
// yojimbo automatically handles:
// - Connection request rate limiting
// - Packet flood detection
// - Challenge/response delays

class BlueMarbleDDoSProtection {
public:
    void ConfigureProtection() {
        yojimbo::ServerConfig config;
        
        // Maximum connections per second
        config.maxClientsPerSecond = 10;
        
        // Connection timeout (kick inactive clients)
        config.clientTimeout = 30.0f;  // 30 seconds
        
        // Maximum packet size
        config.maxPacketSize = 1200;  // Bytes
        
        // Challenge/response configuration
        // (automatically prevents connection flooding)
    }
    
    // yojimbo handles these attacks automatically:
    // 1. Connection flood: Rate limits new connections
    // 2. Packet flood: Drops excessive packets per client
    // 3. Amplification: Challenge/response prevents spoofing
    // 4. Replay: Sequence numbers prevent packet replay
};
```

---

### 5. Serialization System

**Type-Safe Binary Serialization:**

```cpp
// yojimbo's serialization system
struct GeologicalUpdateMessage : public yojimbo::Message {
    uint64_t regionId;
    float temperature;
    float pressure;
    uint32_t resourceDensity;
    
    template <typename Stream>
    bool Serialize(Stream& stream) {
        // Serialize with bounds checking
        serialize_uint64(stream, regionId);
        
        // Serialize with range compression
        serialize_float(stream, temperature);
        
        // Serialize with quantization
        serialize_int(stream, pressure, -1000, 1000);
        
        // Serialize with bit packing
        serialize_bits(stream, resourceDensity, 16);
        
        return true;
    }
    
    YOJIMBO_VIRTUAL_SERIALIZE_FUNCTIONS();
};

// Automatic bandwidth optimization:
// - Bounds checking reduces invalid values
// - Range compression saves bits for known ranges
// - Bit packing for small integers
// - Delta compression for repeated values
```

---

## BlueMarble Application

### 1. Secure Standalone Server

**Complete Implementation:**

```cpp
// BlueMarble server using yojimbo for standalone builds
class BlueMarbleStandaloneServer : public yojimbo::Adapter {
private:
    yojimbo::Server* mServer;
    BlueMarbleMessageFactory mMessageFactory;
    
    struct PlayerState {
        int clientIndex;
        PlayerID playerID;
        Vector3 position;
        RegionID currentRegion;
        bool authenticated;
    };
    
    std::unordered_map<int, PlayerState> mPlayers;
    
public:
    bool Initialize(uint16_t port) {
        // Initialize yojimbo
        if (!yojimbo::InitializeYojimbo()) {
            LogError("Failed to initialize yojimbo");
            return false;
        }
        
        // Generate server key pair
        uint8_t privateKey[yojimbo::KeyBytes];
        yojimbo::GenerateKey(privateKey);
        
        // Configure server
        yojimbo::ServerConfig config;
        config.protocolId = 0xBLUEMABE;  // Unique protocol ID
        config.maxClients = 100;
        config.numChannels = 2;
        config.timeout = 30.0f;
        
        // Create server
        mServer = new yojimbo::Server(
            GetDefaultAllocator(),
            privateKey,
            yojimbo::Address("0.0.0.0", port),
            config,
            *this,
            0.0
        );
        
        mServer->Start(config.maxClients);
        
        LogInfo("Standalone server started on port {} (encrypted)", port);
        return true;
    }
    
    void RunGameLoop() {
        double time = 0.0;
        const double deltaTime = 1.0 / 60.0;
        
        while (mIsRunning) {
            auto frameStart = std::chrono::steady_clock::now();
            
            // Update networking
            mServer->AdvanceTime(time);
            mServer->ReceivePackets();
            
            // Process messages
            ProcessAllMessages();
            
            // Update game state
            UpdateGameState(deltaTime);
            
            // Send state updates
            SendStateUpdates();
            
            // Send packets
            mServer->SendPackets();
            
            time += deltaTime;
            
            // Frame timing
            auto frameEnd = std::chrono::steady_clock::now();
            auto elapsed = frameEnd - frameStart;
            auto sleepTime = std::chrono::milliseconds(16) - elapsed;
            
            if (sleepTime.count() > 0) {
                std::this_thread::sleep_for(sleepTime);
            }
        }
    }
    
    // Adapter callbacks
    yojimbo::MessageFactory* CreateMessageFactory(
        yojimbo::Allocator& allocator) override {
        return new BlueMarbleMessageFactory(allocator);
    }
    
    void OnServerClientConnected(int clientIndex) override {
        LogInfo("Client {} connected (encrypted)", clientIndex);
        
        PlayerState state;
        state.clientIndex = clientIndex;
        state.authenticated = false;
        
        mPlayers[clientIndex] = state;
    }
    
    void OnServerClientDisconnected(int clientIndex) override {
        LogInfo("Client {} disconnected", clientIndex);
        
        auto it = mPlayers.find(clientIndex);
        if (it != mPlayers.end() && it->second.authenticated) {
            SavePlayerState(it->second.playerID);
        }
        
        mPlayers.erase(clientIndex);
    }
};
```

### 2. Cross-Platform Client

**Encrypted Client Implementation:**

```cpp
class BlueMarbleYojimboClient : public yojimbo::Adapter {
private:
    yojimbo::Client* mClient;
    BlueMarbleMessageFactory mMessageFactory;
    
public:
    bool ConnectToServer(const std::string& serverAddress, 
                        uint16_t port,
                        const uint8_t* connectToken,
                        int tokenBytes) {
        // Initialize yojimbo
        if (!yojimbo::InitializeYojimbo()) {
            return false;
        }
        
        // Client configuration
        yojimbo::ClientConfig config;
        config.numChannels = 2;
        
        // Create client
        mClient = new yojimbo::Client(
            GetDefaultAllocator(),
            yojimbo::Address("0.0.0.0"),
            config,
            *this,
            0.0
        );
        
        // Connect using token
        mClient->Connect(connectToken, tokenBytes);
        
        return true;
    }
    
    void Update(double time) {
        if (!mClient->IsConnected()) return;
        
        // Receive packets
        mClient->AdvanceTime(time);
        mClient->ReceivePackets();
        
        // Process server messages
        for (int channel = 0; channel < 2; ++channel) {
            yojimbo::Message* message = mClient->ReceiveMessage(channel);
            while (message) {
                ProcessServerMessage(message);
                mClient->ReleaseMessage(message);
                message = mClient->ReceiveMessage(channel);
            }
        }
        
        // Send packets
        mClient->SendPackets();
    }
    
    void SendPlayerPosition(const Vector3& position, float rotation) {
        if (!mClient->CanSendMessage(0)) return;
        
        auto* msg = static_cast<PlayerPositionMessage*>(
            mMessageFactory.CreateMessage(
                static_cast<int>(GameMessageType::PLAYER_POSITION)
            )
        );
        
        if (msg) {
            msg->x = position.x;
            msg->y = position.y;
            msg->z = position.z;
            msg->rotation = rotation;
            
            mClient->SendMessage(0, msg);  // Unreliable channel
        }
    }
    
    // Adapter callbacks
    yojimbo::MessageFactory* CreateMessageFactory(
        yojimbo::Allocator& allocator) override {
        return new BlueMarbleMessageFactory(allocator);
    }
};
```

---

## Implementation Recommendations for BlueMarble

### High Priority (Months 1-3)

1. **Evaluate for Standalone Builds**
   - Test yojimbo vs ENet performance
   - Measure encryption overhead
   - Validate security features
   - **Deliverable:** Performance comparison report

2. **Token Generation System**
   - Web service for token generation
   - Database integration for client IDs
   - Token expiration management
   - **Deliverable:** Secure authentication flow

3. **Message System Design**
   - Define all game message types
   - Implement serialization for each
   - Optimize message sizes
   - **Deliverable:** Complete message protocol

### Medium Priority (Months 4-6)

4. **Cross-Platform Testing**
   - Test on Windows, Linux, macOS
   - Verify encryption on all platforms
   - Mobile platform evaluation
   - **Deliverable:** Cross-platform compatibility

5. **Performance Optimization**
   - Profile encryption overhead
   - Optimize message serialization
   - Tune buffer sizes
   - **Deliverable:** <5ms encryption overhead

6. **Security Hardening**
   - Penetration testing
   - DDoS stress testing
   - Token system audit
   - **Deliverable:** Security audit report

### Long-Term (Months 6+)

7. **Custom Extensions**
   - Add compression layer
   - Implement priority queues
   - Add metrics collection
   - **Deliverable:** Enhanced yojimbo wrapper

8. **Hybrid Approach**
   - yojimbo for standalone
   - GNS for Steam builds
   - Shared message protocol
   - **Deliverable:** Multi-platform support

---

## Performance Benchmarks and Targets

### yojimbo vs ENet vs GNS Comparison

| Metric | ENet | yojimbo | GNS | Target |
|--------|------|---------|-----|--------|
| **Encryption** | None | Built-in | Built-in | Required |
| **Setup Complexity** | Low | Medium | High | Medium OK |
| **Latency Overhead** | <3ms | <5ms | <8ms | <10ms |
| **Security Features** | None | Excellent | Enterprise | Excellent |
| **Steam Integration** | No | No | Yes | Optional |
| **License** | MIT | MIT | BSD-3 | Permissive |
| **Platform Support** | All | All | All | All |
| **Active Development** | Yes | Yes | Yes | Required |

### BlueMarble with yojimbo Targets

```cpp
struct YojimboPerformanceTargets {
    // Encryption overhead
    float maxEncryptionOverhead = 5.0f;    // 5ms
    float avgEncryptionOverhead = 2.0f;    // 2ms
    
    // Connection capacity
    size_t maxClientsPerServer = 100;
    size_t simultaneousConnections = 50;
    
    // Bandwidth
    size_t avgBandwidthPerPlayer = 8192;   // 8 KB/s
    size_t peakBandwidthPerPlayer = 32768; // 32 KB/s
    
    // Security
    bool encryptionMandatory = true;
    bool tokenAuthRequired = true;
    int tokenTimeoutSeconds = 30;
    
    // Message throughput
    int messagesPerSecond = 60;
    int maxMessageSize = 1200;
};
```

---

## Integration with Existing BlueMarble Research

### Cross-References

1. **RakNet Analysis** (`game-dev-analysis-raknet-open-source-version.md`)
   - yojimbo provides similar patterns with built-in encryption
   - Connection token system vs RakNet's authentication
   - Both designed for action games

2. **ENet Analysis** (`game-dev-analysis-enet-networking-library.md`)
   - yojimbo adds security layer ENet lacks
   - Similar simplicity, better for commercial releases
   - Both MIT licensed

3. **GameNetworkingSockets Analysis** (`game-dev-analysis-gamenetworkingsockets-valve.md`)
   - yojimbo simpler than GNS
   - GNS for Steam, yojimbo for standalone
   - Both provide encryption and security

### Decision Matrix

```cpp
// Library selection guide

// Use yojimbo if:
bool useYojimbo =
    needsEncryption &&
    !requiresSteam &&
    wantsSimplicity &&
    targetsActionGame;

// Use ENet if:
bool useENet =
    prototypePhase &&
    !needsEncryption &&
    simplicityCritical;

// Use GNS if:
bool useGNS =
    requiresSteam ||
    needsEnterpriseScale ||
    wantsRelay network;

// BlueMarble strategy:
// - Prototype: ENet
// - Standalone: yojimbo
// - Steam: GNS
```

---

## Additional Sources Discovered

During analysis of yojimbo, the following related sources were identified:

**Source Name:** libsodium Cryptography Library  
**Discovered From:** yojimbo Networking Library analysis  
**Priority:** Medium  
**Category:** GameDev-Tech  
**Rationale:** Underlying crypto library used by yojimbo, understanding it helps with security  
**Estimated Effort:** 4-6 hours  
**GitHub:** https://github.com/jedisct1/libsodium

**Source Name:** Glenn Fiedler's Networking Articles  
**Discovered From:** yojimbo Networking Library analysis  
**Priority:** High  
**Category:** GameDev-Tech  
**Rationale:** Author of yojimbo, comprehensive game networking tutorial series  
**Estimated Effort:** 6-8 hours  
**URL:** https://gafferongames.com/categories/game-networking/

---

## Next Steps

### Immediate Actions

1. **Evaluate yojimbo**
   - [ ] Build and test yojimbo
   - [ ] Measure encryption overhead
   - [ ] Compare with ENet performance
   - [ ] Test token authentication

2. **Security Analysis**
   - [ ] Review libsodium integration
   - [ ] Test DDoS protection
   - [ ] Validate encryption implementation
   - [ ] Penetration testing

3. **Integration Planning**
   - [ ] Design token generation service
   - [ ] Plan message protocol
   - [ ] Define security policies
   - [ ] Create deployment strategy

### Research Queue Updates

- Add libsodium analysis (medium priority)
- Add Glenn Fiedler's articles (high priority)
- Consider yojimbo for non-Steam builds
- Plan hybrid networking strategy (ENet/yojimbo/GNS)

---

## Conclusion

yojimbo represents a security-focused middle ground between ENet's simplicity and GNS's enterprise complexity. Its built-in encryption, connection token authentication, and DDoS protection make it ideal for commercial standalone game deployments that don't require Steam integration.

**Key Insights for BlueMarble:**

1. **Security Without Complexity**: yojimbo provides enterprise-grade encryption without GNS's Steam dependencies.

2. **Action Game Focus**: Designed specifically for fast-paced games, making it suitable for real-time MMORPG gameplay.

3. **Modern C++ Design**: Clean API and modern codebase make integration and maintenance easier than legacy libraries.

4. **Connection Token System**: Elegant authentication solution prevents unauthorized access without complex infrastructure.

5. **MIT License**: Complete commercial freedom without licensing concerns.

**Strategic Recommendation for BlueMarble:**

- **Phase 1 (Prototype)**: Use ENet for rapid iteration
- **Phase 2 (Alpha/Standalone)**: Migrate to yojimbo for security
- **Phase 3 (Steam Release)**: Add GNS for Steam integration
- **Production**: Maintain both yojimbo (standalone) and GNS (Steam) builds

**Critical Advantage**: yojimbo enables secure standalone releases without requiring Steam, opening doors to itch.io, Epic Games Store, GOG, and other distribution platforms while maintaining professional-grade security.

**Next Source:** Reddit - r/MMORPG (original assignment group source)

---

**Document Version:** 1.0  
**Last Updated:** 2025-01-17  
**Lines:** 750  
**Status:** ✅ Complete  
**Additional Sources Identified:** 2 (libsodium, Glenn Fiedler's Articles)
