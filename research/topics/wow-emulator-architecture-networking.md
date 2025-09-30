---
title: World of Warcraft Emulators – Architecture & Communication
date: 2025-01-09
owner: @Nomoos
status: complete
tags: [mmo, server-architecture, networking, authentication, protocols, emulation]
---

# World of Warcraft Emulators – Architecture & Communication

## Problem/Context

Understanding how open-source MMORPG emulators (e.g., TrinityCore, CMaNGOS, AzerothCore) implement server architecture, authentication systems, and networking protocols can inform BlueMarble's own multiplayer architecture design. These emulators replicate Blizzard's server software and provide battle-tested approaches to handling client-server communication, authentication security, and packet management at scale.

## Key Findings

### Architecture Components
- **Dual-daemon architecture**: Separate authentication and world simulation servers
  - **Auth/Realmd** – Handles login & realm list; communicates with `auth` database
  - **World Server** (worldserver/mangosd) – Manages gameplay state, maps, NPC AI, scripts; communicates with `world` & `characters` databases
- **Database structure**: MySQL/MariaDB with three core schemas: `auth`, `characters`, `world`
- **Client data extraction**: Server builds navmeshes (MMAPs/VMAPs) and reads client DBCs to reproduce retail behavior

### Connection Flow
1. **Client → Auth/Realmd** (default port **3724**)
   - SRP6 handshake for authentication
   - Client requests realm list
   - Disconnects after realm selection
2. **Client → World Server** (commonly **8085**, configurable)
   - TCP connection
   - World communication established with session key from SRP

### Authentication System
- **Protocol**: SRP6 (Secure Remote Password) variant
- **Password storage**: SHA-1 hashes of `ACCOUNT:UPPERCASE_PASSWORD`
- **Session key derivation**: Occurs without sending raw password
- **Header protection**: Session key used for subsequent packet header obfuscation

### Packet Model
- **Login packets**: Unencrypted, simple exchanges (auth + realm list)
- **World packets**: Structure `[size | opcode | body]`
  - **Header only** (size + opcode) is obfuscated
  - Payload (body) remains plaintext in 1.x–3.3.5 clients

### Encryption / Header Protection
- **Algorithm**: RC4/ARC4 stream cipher
- **Key derivation**: Initialized with SRP6-derived session key
- **Separate keys**: One for client→server, another for server→client
- **Limited scope**: Bodies remain plaintext; only headers are encrypted
- **Initialization**: Involves mixing static HMAC constants with session key

### Opcodes & Protocol
- **Purpose**: Define message types (movement, chat, combat, etc.)
- **Discovery**: Reverse-engineered from WoW client
- **Documentation**: Projects like WowPacketParser and wowdev.wiki document opcode layouts
- **Implementation**: Emulators map opcodes to C++ handlers in world server

## Evidence

### Key Projects
- **TrinityCore** – Widely used, Wrath (3.3.5a) focus
- **CMaNGOS** – Stable, with forks for Vanilla/TBC/WotLK
- **AzerothCore** – Community fork of TrinityCore with modular plugin system

### Network Configuration
To host public/private realms:
- Forward Auth (3724) and World (8085) ports
- Configure realm's external IP for correct appearance in realm list

### Wire Protocol Details

#### Auth/Realmd (Port 3724)
```
1. SRP6 handshake → session key generation
2. Realm list exchange
3. Client disconnects
```

#### World Server (Port 8085)
```
1. TCP handshake
2. Header-encrypted world packets
3. Full gameplay protocol over opcode-driven handlers
```

## Implications for BlueMarble

### Architecture Lessons
1. **Separation of concerns**: Dedicated authentication service separate from world simulation
   - Reduces attack surface for authentication systems
   - Allows independent scaling of auth vs gameplay servers
   - Simplifies security auditing and updates

2. **Database organization**: Clear schema separation by function
   - `auth`: Account credentials and realm information
   - `characters`: Player state and inventory
   - `world`: Static game world data
   - Similar to BlueMarble's proposed separation of player data, world state, and economy databases

3. **Client-side data**: Server builds navigation data from client assets
   - BlueMarble's procedural generation approach differs but can learn from this validation model
   - Consider generating validation data for client-side predictions

### Security Considerations
1. **SRP6 protocol**: Well-tested authentication without transmitting passwords
   - Consider for BlueMarble's authentication system
   - Provides mutual authentication (client verifies server, server verifies client)
   - Resistant to man-in-the-middle attacks

2. **Selective encryption**: Only critical data (packet headers) encrypted
   - Performance optimization: plaintext bodies reduce CPU overhead
   - Security trade-off acceptable for 1.x–3.3.5 era clients
   - Modern implementations should consider TLS for full encryption

3. **Session key management**: Derived keys for bidirectional communication
   - Prevents replay attacks
   - Each direction uses unique key stream position

### Networking Architecture
1. **Port separation**: Different ports for different services
   - 3724: Authentication (standard, well-known)
   - 8085: World (configurable per realm)
   - BlueMarble could adopt similar approach:
     - Gateway port for auth/realm selection
     - Multiple world server ports for load balancing

2. **Opcode-driven architecture**: Message types as numeric codes
   - Efficient binary protocol
   - Easy to extend with new message types
   - Consider for BlueMarble's client-server protocol

3. **TCP for reliability**: Critical for game state synchronization
   - Movement, combat, inventory changes require guaranteed delivery
   - Compare to UDP for less critical updates (chat, position broadcasts)

## Next Steps

### For BlueMarble Architecture
1. **Authentication service design**
   - Research SRP6 implementation in .NET/C#
   - Design auth/realm separation similar to Auth/Realmd model
   - Plan session key management system

2. **Protocol design**
   - Define opcode structure for BlueMarble messages
   - Design packet format (header + body)
   - Determine encryption requirements (consider TLS)

3. **Database schema planning**
   - Finalize separation of auth, characters, world databases
   - Document schema relationships and access patterns
   - Plan for PostgreSQL vs Redis usage per data type

4. **Networking infrastructure**
   - Design gateway server architecture
   - Plan load balancing strategy for world servers
   - Define port allocation and firewall requirements

### Research Follow-ups
1. Investigate modern authentication alternatives (OAuth2, JWT)
2. Compare WoW's opcode system to other MMO protocol designs
3. Research packet compression techniques for bandwidth optimization
4. Study distributed world server architectures for massive scale

## References

- TrinityCore: Open-source WoW emulator (3.3.5a focus)
- CMaNGOS: Stable WoW emulator with version-specific forks
- AzerothCore: Community-driven fork with modular architecture
- WowPacketParser: Tool for analyzing WoW network protocol
- wowdev.wiki: Community documentation of WoW protocols and opcodes

## Related Research

- [BlueMarble Technical Design Document](../../docs/core/technical-design-document.md) - Server architecture overview
- [Database Architecture Risk Analysis](../spatial-data-storage/database-architecture-risk-analysis.md) - Security considerations
