# Analysis: Server-Centric vs Peer-to-Peer Networking in Blizzard Games

## Executive Summary

This analysis examines Blizzard Entertainment's evolution from peer-to-peer (P2P) networking to server-authoritative architectures across their game portfolio. The research reveals a clear progression driven by the need to combat cheating, protect player experience, and support persistent shared worlds. Key findings show that while P2P models offer lower costs and simpler implementation, they are fundamentally vulnerable to cheating and information exploitation. Server-centric architectures, despite higher operational costs, provide the only effective solution for cheat-resistant competitive and persistent multiplayer games.

The transition from Diablo I's asynchronous P2P (vulnerable to arbitrary cheats) through RTS lockstep P2P (vulnerable to information reveals) to Diablo II and WoW's server-authoritative models demonstrates industry-wide recognition that game integrity requires centralized validation and information control.

## Key Insights

### 1. **P2P Networking Vulnerabilities**

**Asynchronous P2P (Diablo I model)**:
- Each client is authoritative for its own actions
- Clients trust each other without verification
- Enables trivial cheating: clients can send any bogus message
- Example: "Townkill" exploit allowed killing players in safe zones
- Result: Destroyed game economy and player trust

**Relevance to BlueMarble**: Asynchronous P2P is completely unsuitable for a persistent planet-scale world where players share resources and terrain modifications. Any player could claim to have mined more resources or modified terrain in ways that break game balance.

### 2. **Synchronous Lockstep P2P - Partial Solution**

**Model characteristics**:
- All clients run identical simulations
- Only player inputs are exchanged, not results
- Prevents "impossible action" cheats through consistency checking
- Still vulnerable to information-reveal exploits (e.g., maphacks)
- Used successfully in RTS games (StarCraft, Warcraft)

**Limitations**:
- Every client knows complete game state (for synchronization)
- Fog of war is client-side only, can be bypassed
- IP addresses exposed in pure P2P (mitigated by relay servers)

**Relevance to BlueMarble**: Even with lockstep, clients would know the entire voxel world state, enabling terrain x-ray vision and resource location cheats. Not acceptable for exploration-based gameplay.

### 3. **Server-Authoritative Architecture - The Solution**

**Core principles**:
- Server is sole authority on game state
- Clients send inputs, server validates and computes results
- Server only sends clients information they should legitimately know
- Clients cannot "lie" about actions - server enforces all rules
- Clients cannot "peek" at hidden data - server controls information flow

**Trade-offs**:
- Higher cost: requires server infrastructure, bandwidth, maintenance
- Increased complexity: need robust server-side game logic
- Latency considerations: all actions require server round-trip
- Worth it: protects game integrity and player experience

**Relevance to BlueMarble**: Essential for planet-scale persistent world. Server must:
- Validate all terrain modifications
- Control resource spawning and collection
- Manage visible area for each player
- Maintain authoritative voxel world state

### 4. **Information Control Strategy**

**Best practice identified**:
- "Clients can't do anything they're not supposed to, and can't see anything they're not supposed to see"
- Server sends only locally relevant data (nearby entities, visible terrain)
- Reduces bandwidth and prevents reconnaissance cheats
- Used in WoW: players receive only their zone's data

**Relevance to BlueMarble**: Critical for performance and security:
- Only send voxel data for player's local area (e.g., 1km radius)
- Don't transmit resource locations beyond visible range
- Progressive detail loading based on distance
- Prevents both cheating and reduces network load

### 5. **Cost-Benefit Analysis**

**Why Blizzard accepted server costs**:
- Diablo I's open cheating destroyed player trust
- Unable to monetize or maintain healthy community
- Server infrastructure cost < lost revenue from cheating
- Competitive integrity essential for game longevity

**Relevance to BlueMarble**: Similar calculation applies:
- Persistent world requires player trust in shared environment
- Cheating in terrain/resource gathering breaks core gameplay
- Server costs justified by protecting game experience
- Consider cloud auto-scaling to manage costs

## Recommendations

### Immediate Actions

1. **Adopt Server-Authoritative Architecture**
   - Priority: High
   - Rationale: Only viable option for cheat-resistant persistent world
   - Timeline: Core architecture decision, implement from start
   - Implementation: All gameplay logic runs server-side, clients are display/input only

2. **Implement Information Hiding**
   - Priority: High
   - Rationale: Reduces bandwidth and prevents reconnaissance cheats
   - Timeline: Early implementation
   - Implementation: Server sends only local voxel data (e.g., 500m-2km radius based on player location)

3. **Server-Side Validation Layer**
   - Priority: High
   - Rationale: Prevent impossible actions (mining too fast, building in restricted areas)
   - Timeline: Required before multiplayer alpha
   - Implementation: All player commands validated against game rules before execution

### Long-term Considerations

1. **Latency Optimization**
   - Server-authoritative architecture adds latency
   - Implement client-side prediction for smooth movement
   - Use lag compensation techniques for time-sensitive actions
   - Consider regional server deployment for global player base

2. **Scalability Architecture**
   - Plan for distributed server clusters
   - Spatial partitioning for large planet surface
   - Load balancing based on player density
   - See separate research on voxel world architecture

3. **Anti-Cheat Beyond Network**
   - Even with server authority, need client integrity checks
   - Memory scanning prevention
   - Automated behavior analysis (e.g., detecting bot patterns)
   - Community reporting systems

### Areas for Further Research

1. **Hybrid Models for Different Game Systems**
   - Can some non-critical systems use P2P for cost savings?
   - E.g., voice chat, cosmetic effects
   - Identify what must be server-authoritative vs. what can be client-side

2. **Performance Benchmarks**
   - What's the realistic player count per server instance?
   - Bandwidth requirements per player
   - Server CPU/memory needs for voxel world simulation

3. **Edge Cases**
   - How to handle network partitions (server becomes unreachable)
   - Rollback strategies for disputed actions
   - Cheating detection and response policies

## Impact Assessment

**Technical Impact**: Major - requires server-authoritative architecture from the ground up

**Cost Impact**: Significant - ongoing server infrastructure and bandwidth costs

**Development Timeline**: Moderate - adds complexity but is well-understood pattern

**Player Experience**: Positive - prevents cheating, ensures fair gameplay

## Implementation Priority

**Critical Path**: Yes - core architecture decision that affects all multiplayer systems

**Dependencies**: Must be decided before implementing:
- Multiplayer networking layer
- World state synchronization
- Player action validation
- Terrain modification system

## Related Research

- [Technical Stack for Large-Scale Voxel World](../conversation-dr_68dbe0e4/) - Server architecture specifics
- [Temporal Simulation](../conversation-temporal-simulation/) - Event propagation in planet-scale worlds
- [Spatial Data Storage Research](../../spatial-data-storage/) - Voxel world data structures
