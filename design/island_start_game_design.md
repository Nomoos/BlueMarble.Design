# Island Start Game Design — Medieval MMO Simulation

**Document Type:** Game Design Document  
**Version:** 1.0  
**Author:** BlueMarble Design Team  
**Date:** 2024  
**Status:** Draft

## Executive Summary

**Title:** Island of Hands (working title)

**Elevator Pitch:** A top-down, web & mobile-first medieval open-world simulation MMO where players farm, hunt, mine, trade and manage labor. Skills are mostly hidden and change gradually depending on concrete activities and materials (e.g., mining coal vs. rock vs. dirt). Players can assign earned experience from a limited XP pool to skills, hire other players or NPC workers via a work-auction, and shape an island economy through production, transport and trade.

**Core Pillars:**
- **Simulation**: Resources, production chains and tool durability matter
- **Emergent Economy**: Player-driven markets, auctions for work, and transport logistics
- **Skill Specialization**: Activity-specific hidden stats that change with each job/material
- **Social MMO**: Hire, trade, form small groups, transport goods across islands

## Platform & Target Audience

**Platforms:** Web (WebGL / WebRTC) and Mobile (iOS/Android) first, later native PC  
**Target Audience:** Players who enjoy slow-burn simulation, economic systems, and social gameplay (ages 16–40)

**Camera & View:** Top-down/isometric (60–90°) with zoom. Prioritize clarity of tiles/resources at small screen sizes.

## Core Gameplay Loop (Island Start)

**Primary Loop:**
1. **Explore** island
2. **Gather** resources (hunt/collect/mine/dig)
3. **Process** or transport goods
4. **Sell** or post jobs on work-auction
5. **Assign** XP from pool or hire workers
6. **Upgrade** tools/structures
7. **Expand** production/market reach

## Key Mechanics

### Hidden Stats (Player Profile)
- **Example Stats:** MiningCoal, MiningRock, DiggingDirt, Hunting, Transport, Farming, Crafting
- **Behavior:** Stats change only when performing related actions and are mostly hidden (players infer by performance)

### XP Pool System
- **Pool Capacity:** Players accumulate a single XP pool from activities up to Pmax points (example: Pmax=1000)
- **Allocation:** At any time the player can allocate points to skills
- **Queue Management:** If pool exceeds Pmax, new XP enters a bounded queue; when full, oldest queued knowledge (unallocated XP) is dropped or replaced by newest depending on chosen policy
- **Optional Feature:** Automatic aging — if pool not used for N days, oldest XPs decay faster

### Skill Gain Rules
**Formula Example:**
```
XP_gain = base_activity_xp * material_modifier * tool_efficiency * (1 - skill_level / skill_cap)
```

**Material Modifiers (Example):**
- Coal: 1.2
- Rock: 1.0  
- Dirt: 0.6

**Tool Efficiency:** Scales speed and durability

### Knowledge Replacement Policy
- **FIFO Queue:** "Unspent XP entries" with timestamps
- **Replacement:** When full, new entries replace oldest entry
- **Player Control:** Players can spend entries to bump skill levels
- **Purpose:** Enables players to commit to new knowledge choices intentionally

### Hiring & Work-Auction System
- **Job Posting:** Players or NPCs post jobs with requirements, duration and pay (coins or goods)
- **Worker Skills:** Workers have their own skill levels; higher skill = faster, higher quality, less tool wear
- **Trade-off:** Hiring someone consumes time and pay; employer avoids skill wear but gains outcomes

### Tools & Durability
- **Wear System:** Tools wear with use; better tools cost more and may provide XP multipliers
- **Economic Sink:** Repairs and material quality factor into long-term economic sinks

### Transport & Logistics
- **Vehicles:** Ships and carts require crew, have capacity and fuel/time costs
- **Gameplay:** Transport routes create income streams and logistics gameplay

### Trading & Economy
- **Markets:** Local market + island-wide auction board
- **Price Discovery:** Driven by supply/demand
- **Controls:** Currency and trade limits; initial economy closed to avoid runaway trade inflation

### Persistence & Progression
- **Island Structures:** Mills, quarries that players upgrade through pooled resources
- **Player Progression:** Slow progression — specialization matters; allow respec costs and hard caps

## MVP Scope (First 2-Year Focus: Island Start)

### Core Infrastructure
- Single island shard with a single authoritative server instance
- Player entity with inventory, top-down movement, basic UI

### Core Activities
- **Hunting:** Animals spawn & drop resources
- **Gathering:** Forage system
- **Mining/Digging:** Rock, coal, dirt with material differences

### Essential Systems
- XP pool mechanics + visible queue UI for allocation (simple)
- One basic market/auction board for hiring and commodity trading
- One hireable NPC worker prototype (AI) and hiring flow
- Tool system (durability, tiers)
- Basic ship/cargo prototype (local transport)
- Simple persistence (cloud save per account)
- Basic analytics: level attempts, job completions, resource flows

## Technical & Architecture Suggestions

### Game Engine
- **Recommended:** Unity (WebGL + mobile) for tooling & mobile parity
- **Alternative:** Godot (lighter Web export)

### Network Architecture
- **Server Model:** Authoritative server for simulation
- **Initial Setup:** Single-shard authoritative server that simulates island state

### Server Technology
- **Backend Options:** Node.js + TypeScript or Go for server logic
- **Communication:** WebSockets (or WebRTC data) for real-time sync
- **MMO Backend Consideration:** Nakama, PlayFab, Photon, Colyseus to reduce build time

### Database & Persistence
- **Canonical Data:** PostgreSQL
- **Ephemeral State:** Redis
- **Large Assets:** Object storage

### Scaling Approach
- **Initial:** Single-shard for first island to simplify
- **Future:** Shard-by-island or subzone servers

### Hosting & Deployment
- **Cloud Platform:** GCP/AWS
- **Architecture:** Dockerized services and CI/CD pipelines

### Analytics
- **Event Tracking:** Job posted, job accepted, XP gained, item crafted, coin transfers

## Economy & Balancing Notes

### Economic Controls
- **Sinks:** Strong sinks (repair, taxes, construction) to avoid inflation
- **Limits:** Stack sizes and transport throughput early
- **Caps:** Skill caps and diminishing returns to force specialization

## Safety & MMO Considerations

### Security
- **Authoritative Simulation:** Server prevents client cheating
- **PvP Policy:** For first 2-year island start, make PvP limited or consensual to avoid griefing
- **Moderation:** Player reporting and rate limits on auctions/trades

## Monetization (Live Service)

### Revenue Streams
- **Primary:** Cosmetic shop, convenience subscriptions (extra storage), vanity ships/houses
- **Policy:** No pay-to-win core progression
- **Future:** Paid DLC islands or expansions later

## Development Milestones (First 12 Months)

### Months 0–2: Prototyping (Single-player Local Sim)
**Deliverables:** Player movement, harvesting, mining, XP pool mechanics, small UI

### Months 3–5: Server Prototype + Persistence
**Deliverables:** Authoritative server for island, simple market & job posting, one hireable NPC

### Months 6–8: Economy & Tools
**Deliverables:** Trading UI, tool durability, transport prototype (small boats), initial balancing

### Months 9–12: Beta Island Shard & Playtest
**Deliverables:** Cloud deploy of single island shard, telemetry, player onboarding, repeated playtests

## Success Metrics (Early Targets)

### Player Retention
- **Day 1 Retention:** > 30% (target for genre)
- **Day 7 Retention:** > 10% (target for genre)

### Gameplay Metrics
- **First Job Completion:** Average time < 30 minutes
- **Auction Fulfill Rate:** > 60% for posted jobs in early economy
- **Economic Health:** Resource sink usage (repairs/construction) > 10% of currency flux

## Risks & Mitigations

### Economic Risks
- **Runaway Economy:** Start closed economy test environment, tune sinks
- **Inflation Control:** Monitor currency flows and adjust sinks dynamically

### Security Risks
- **Cheating/Bots:** Authoritative server and bot detection; early anti-exploit telemetry
- **Player Behavior:** Moderation tools and community guidelines

### Development Risks
- **Scope Creep:** Strict MVP cut and progressive feature gating
- **Technical Complexity:** Start simple, iterate based on player feedback

## Open Questions (Need Answers to Proceed)

1. **PvP Policy:** Fully enabled, consensual, or purely cooperative curation for the island start?
2. **Concurrent Users:** Expected concurrent users at launch (to plan shards & hosting)?
3. **Monetization Strategy:** Preference now (F2P with cosmetics, paid app, subscription)?
4. **Team Resources:** Team size and tech skill focus (Unity/JS/Go)?
5. **Player Permanence:** Do you want player characters to be permanent (permadeath) or soft-respawn?

## Related Documentation

- [Main Game Design Document](../docs/core/game-design-document.md) - Complete GDD
- [Technical Design Document](../docs/core/technical-design-document.md) - Technical architecture
- [Player Progression System](../docs/gameplay/spec-player-progression-system.md) - Detailed progression mechanics
- [Project Roadmap](../roadmap/project-roadmap.md) - Development timeline and priorities

## Revision History

| Version | Date | Author | Changes |
|---------|------|--------|---------|
| 1.0 | 2024 | BlueMarble Design Team | Initial version based on one-page design |

---

*This document establishes the foundation for the island start scope of BlueMarble's medieval MMO simulation game, focusing on core mechanics, technical architecture, and development priorities for the first two years of development.*