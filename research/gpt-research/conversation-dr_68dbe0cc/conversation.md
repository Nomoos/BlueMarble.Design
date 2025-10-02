# GPT Conversation: Server-Centric vs Peer-to-Peer in Blizzard Games

**Conversation ID**: dr_68dbe0cc315081918182816df1b6d424  
**Date**: 2025-10-02  
**URL**: https://chatgpt.com/s/dr_68dbe0cc315081918182816df1b6d424  
**Related Issue**: #101

## Conversation Summary

This conversation explores the evolution of Blizzard's networking architecture from peer-to-peer (P2P) to server-centric models, with a focus on cheat prevention and best practices for multiplayer game architecture.

## Key Topics Covered

### Evolution of Blizzard's Networking Architecture

**Early P2P Experimentation (Diablo I)**
- Diablo I (1996) used an asynchronous P2P model where each client was authoritative for its own actions
- This made cheating trivial - clients could send bogus messages that others would trust without verification
- The infamous "Townkill" exploit demonstrated this vulnerability
- Cheats destroyed Diablo's economy and made playing with strangers unsafe

**Synchronous Lockstep P2P (RTS Games)**
- Used in Warcraft II/III and StarCraft
- All game instances run the same simulation and only exchange player inputs
- Prevents "impossible action" cheats since all clients calculate outcomes identically
- However, still vulnerable to information-reveal cheats (e.g., StarCraft's "MapHack")
- Warcraft III addressed IP privacy concerns by using a hosted P2P model where messages are relayed through Blizzard servers

**Server-Authoritative Model (Diablo II onwards)**
- By early 2000s, fully server-authoritative client-server model became standard
- Server simulates the game world, enforces rules, and sends results to clients
- Clients only receive information they should legitimately know
- Diablo II introduced "Closed Battle.net" mode with server-side game logic
- World of Warcraft (2004) used fully server-centric architecture from day one

### Best Practices & Architecture Lessons

**Server Authority Benefits**
- Server decides what players can or cannot do
- Clients can't "lie" about actions - server validates all commands
- Clients can't "peek" at hidden data - server only sends legitimate information
- Greatly reduces cheating compared to P2P models

**Trade-offs**
- Cost and complexity: requires maintaining server farms
- Hardware, bandwidth, and staffing expenses
- Deemed worthwhile to protect game integrity

**Modern Architecture Patterns**
- All player commands go to server first
- Server verifies permissions and game rules
- Server updates world state and notifies other players
- Server is ultimate "source of truth" for game state
- Well-written architecture ensures "clients can't do anything they're not supposed to, and can't see anything they're not supposed to see"

## Relevance to BlueMarble

This research directly informs BlueMarble's multiplayer architecture decisions:

1. **Server Authority**: The planet-scale simulation requires server-authoritative architecture to prevent cheating and maintain world consistency

2. **Information Hiding**: The voxel world engine should only send clients data about their local area, not the entire planet state

3. **Validation**: All player actions (mining, building, terraforming) must be validated server-side before being applied to the world

4. **Trade-offs**: Must balance server costs with game integrity requirements for a persistent shared world

## Context

This conversation discusses networking architecture patterns used by Blizzard Entertainment across their game portfolio, from early P2P experiments to modern server-centric MMO architectures. The analysis focuses on how different networking models address cheating, scalability, and player experience concerns.

## References

The conversation references several Blizzard titles:
- Diablo I (1996)
- Warcraft II/III
- StarCraft
- Diablo II (2000)
- World of Warcraft (2004)

Also mentions other MMOs:
- EverQuest
- Ultima Online
- Asheron's Call
