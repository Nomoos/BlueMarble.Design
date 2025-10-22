# Phase 3 Research Planning - Game Economy & Technical Deep Dives

---
title: Phase 3 Research Planning - Economy, Materials & Technical Sources
date: 2025-01-17
tags: [research, phase-3, planning, economy, materials, game-design]
status: ready
phase: 3-planning
---

**Document Type:** Phase 3 Planning Document  
**Version:** 1.0  
**Total Sources:** 25+ (13 from Phase 2 discoveries + 12+ new economy/material sources)  
**Primary Focus:** Game economy, material sources/sinks, and technical deep dives  
**Estimated Effort:** 150-200 hours  
**Timeline:** 10-14 weeks

---

## Executive Summary

Phase 3 research focuses on two critical areas for BlueMarble's MMORPG development:

1. **Game Economy & Materials (NEW)** - Economic systems, material sources, material sinks, player-driven economies, resource balance
2. **Technical Deep Dives (From Phase 2)** - 13 high-priority sources discovered during Phase 2 research

This phase bridges the technical foundation from Phase 2 with the game design and economic systems needed for a sustainable player-driven MMORPG economy.

---

## Phase 2 Discoveries (13 Sources)

These sources were identified during Phase 2 Group 01 Critical GameDev-Tech research:

### Batch 1 Discoveries (4 sources - Procedural Generation Focus)

1. **GPU Gems 3: Advanced Procedural Techniques**
   - **Priority:** High
   - **Category:** GameDev-Tech
   - **Estimated Effort:** 4-6 hours
   - **Rationale:** Advanced GPU techniques for real-time procedural generation
   - **Discovered From:** GPU Noise Generation research

2. **Shader Toy: Noise Function Library**
   - **Priority:** Medium
   - **Category:** GameDev-Tech
   - **Estimated Effort:** 2-3 hours
   - **Rationale:** Practical shader implementations for terrain and effects
   - **Discovered From:** GPU Noise Generation research

3. **WebGL Noise by Ian McEwan**
   - **Priority:** High
   - **Category:** GameDev-Tech
   - **Estimated Effort:** 3-4 hours
   - **Rationale:** Optimized noise functions for web-based clients
   - **Discovered From:** Advanced Perlin/Simplex Noise research

4. **"Improving Noise" by Ken Perlin**
   - **Priority:** High
   - **Category:** GameDev-Tech
   - **Estimated Effort:** 2-3 hours
   - **Rationale:** Original research from the creator of Perlin noise
   - **Discovered From:** Advanced Perlin/Simplex Noise research

### Batch 2 Discoveries (6 sources - World Building & Engine Focus)

5. **GDC Vault: Guerrilla Games Technical Talks**
   - **Priority:** High
   - **Category:** GameDev-Tech
   - **Estimated Effort:** 6-8 hours
   - **Rationale:** AAA world building and Decima Engine techniques
   - **Discovered From:** Horizon Zero Dawn World Building research

6. **"Building Open Worlds" Collection**
   - **Priority:** Medium
   - **Category:** GameDev-Design
   - **Estimated Effort:** 8-10 hours
   - **Rationale:** Large-scale open world design patterns
   - **Discovered From:** Horizon Zero Dawn World Building research

7. **"AI Game Programming Wisdom" Series**
   - **Priority:** High
   - **Category:** GameDev-Tech
   - **Estimated Effort:** 10-12 hours
   - **Rationale:** Advanced AI and agent behavior systems
   - **Discovered From:** Cities Skylines Traffic Simulation research

8. **Unity DOTS - ECS for Agents**
   - **Priority:** High
   - **Category:** GameDev-Tech
   - **Estimated Effort:** 6-8 hours
   - **Rationale:** Modern data-oriented design for massive agent counts
   - **Discovered From:** Cities Skylines Traffic Simulation research

9. **"Game Engine Architecture" by Jason Gregory**
   - **Priority:** High
   - **Category:** GameDev-Tech
   - **Estimated Effort:** 12-15 hours
   - **Rationale:** Comprehensive engine design patterns
   - **Discovered From:** Godot Engine Architecture research

10. **Unity ECS/DOTS Documentation**
    - **Priority:** High
    - **Category:** GameDev-Tech
    - **Estimated Effort:** 8-10 hours
    - **Rationale:** Official documentation for Unity's performance-oriented architecture
    - **Discovered From:** Godot Engine Architecture research

### Batch 3 Discoveries (3 sources - Performance & Networking Focus)

11. **"Optimizing Unity Performance" - Unity Learn**
    - **Priority:** High
    - **Category:** GameDev-Tech
    - **Estimated Effort:** 4-6 hours
    - **Rationale:** Official Unity optimization best practices
    - **Discovered From:** C# Performance Optimization research

12. **"C# Performance Tricks" - Jon Skeet**
    - **Priority:** Medium
    - **Category:** GameDev-Tech
    - **Estimated Effort:** 3-4 hours
    - **Rationale:** Advanced C# performance techniques
    - **Discovered From:** C# Performance Optimization research

13. **"Multiplayer Game Programming" by Joshua Glazer**
    - **Priority:** High
    - **Category:** GameDev-Tech
    - **Estimated Effort:** 8-10 hours
    - **Rationale:** Comprehensive multiplayer networking patterns
    - **Discovered From:** WebSocket vs UDP Communication research

**Phase 2 Discoveries Subtotal:** 76-99 hours

---

## NEW: Game Economy & Material Systems (12+ Sources)

Phase 3 introduces a major focus on economic game design, specifically material sources (where players obtain resources) and material sinks (where resources are consumed), which are critical for a sustainable player-driven economy.

### Critical Economy Sources (3 sources)

14. **"Designing Virtual Worlds" by Richard Bartle**
    - **Priority:** Critical
    - **Category:** GameDev-Design
    - **Estimated Effort:** 12-15 hours
    - **Rationale:** Seminal work on MMORPG design including economic systems, player motivations, and world persistence
    - **Focus Areas:**
      - Economic balance in virtual worlds
      - Player-driven vs. NPC-driven economies
      - Resource generation and consumption
      - Long-term economic sustainability

15. **EVE Online Economic Reports & Developer Blogs**
    - **Priority:** Critical
    - **Category:** GameDev-Design, Economy
    - **Estimated Effort:** 8-10 hours
    - **Rationale:** Real-world data from the most successful player-driven MMORPG economy
    - **Focus Areas:**
      - Material sources (mining, production, exploration)
      - Material sinks (ship destruction, module degradation)
      - Economic indicators and metrics
      - Preventing hyperinflation
      - Player-driven market systems

16. **"Virtual Economies: Design and Analysis" by Vili Lehdonvirta**
    - **Priority:** Critical
    - **Category:** GameDev-Design, Economy
    - **Estimated Effort:** 10-12 hours
    - **Rationale:** Academic research on virtual economy design and player behavior
    - **Focus Areas:**
      - Economic models for virtual worlds
      - Material flow analysis
      - Supply and demand in games
      - Economic data collection and analysis

### High Priority Economy Sources (5 sources)

17. **"Economics of MMORPGs" - GDC Talks Collection**
    - **Priority:** High
    - **Category:** GameDev-Design, Economy
    - **Estimated Effort:** 6-8 hours
    - **Rationale:** Industry perspectives on MMORPG economic design
    - **Focus Areas:**
      - Material sources design (gathering, crafting, drops)
      - Material sinks design (repair, consumption, taxes)
      - Balancing production and destruction
      - Anti-inflation mechanics

18. **"Runescape Economic System" - Jagex Developer Blogs**
    - **Priority:** High
    - **Category:** GameDev-Design, Economy
    - **Estimated Effort:** 4-6 hours
    - **Rationale:** Lessons from 20+ years of MMORPG economy management
    - **Focus Areas:**
      - Resource node design and respawn mechanics
      - Crafting as a resource sink
      - Item degradation systems
      - Grand Exchange (player market) design

19. **"Path of Exile: Designing Sustainable Loot" - GDC Talk**
    - **Priority:** High
    - **Category:** GameDev-Design, Economy
    - **Estimated Effort:** 3-4 hours
    - **Rationale:** Successful model for material sources through loot systems
    - **Focus Areas:**
      - Loot as material source
      - Currency items as sinks
      - Seasonal economy resets
      - Preventing economic stagnation

20. **"Albion Online: Player-Driven Economy Design"**
    - **Priority:** High
    - **Category:** GameDev-Design, Economy
    - **Estimated Effort:** 5-7 hours
    - **Rationale:** Full-loot PvP MMORPG with complete player-driven economy
    - **Focus Areas:**
      - Material sources (gathering, refining)
      - Material sinks (full-loot PvP, gear degradation)
      - Territory control and resources
      - Economic balance through loss

21. **"World of Warcraft Economy Analysis" - Academic Papers**
    - **Priority:** High
    - **Category:** GameDev-Design, Economy
    - **Estimated Effort:** 6-8 hours
    - **Rationale:** Academic analysis of the most studied MMORPG economy
    - **Focus Areas:**
      - Material sources evolution (quests, instances, gathering)
      - Material sinks (repair, consumables, mounts)
      - Auction house dynamics
      - Gold sinks and economic health

### Medium Priority Economy Sources (4 sources)

22. **"Game Balance Concepts" by Ian Schreiber**
    - **Priority:** Medium
    - **Category:** GameDev-Design
    - **Estimated Effort:** 4-6 hours
    - **Rationale:** General game balance principles applicable to economy design
    - **Focus Areas:**
      - Resource balance
      - Positive and negative feedback loops
      - Player progression curves
      - Economic friction points

23. **"Diablo III: Real Money Auction House Post-Mortem"**
    - **Priority:** Medium
    - **Category:** GameDev-Design, Economy
    - **Estimated Effort:** 3-4 hours
    - **Rationale:** Cautionary tale about economic design failures
    - **Focus Areas:**
      - What NOT to do in material sources/sinks
      - Impact of real money trading
      - Loot generation vs. trading
      - Economic incentive misalignment

24. **"Elite Dangerous: Resource Distribution & Mining"**
    - **Priority:** Medium
    - **Category:** GameDev-Design
    - **Estimated Effort:** 4-5 hours
    - **Rationale:** Space-sim approach to resource gathering and distribution
    - **Focus Areas:**
      - Spatial resource distribution
      - Mining mechanics as material source
      - Module damage as material sink
      - Dynamic economy simulation

25. **"Satisfactory: Factory Building Economy"**
    - **Priority:** Medium
    - **Category:** GameDev-Design
    - **Estimated Effort:** 3-4 hours
    - **Rationale:** Production chain design and resource flow optimization
    - **Focus Areas:**
      - Resource node placement and balance
      - Production chains (material transformation)
      - Consumption rates
      - Player-driven optimization

**Economy Sources Subtotal:** 74-93 hours

---

## Phase 3 Total Summary

**Total Sources:** 25  
**Total Estimated Effort:** 150-192 hours  
**Timeline:** 10-14 weeks (with parallel processing)

### Category Distribution

| Category | Count | Percentage |
|----------|-------|------------|
| GameDev-Tech | 13 | 52% |
| GameDev-Design | 9 | 36% |
| Economy | 8 | 32% |
| GameDev-Tech + Design | 3 | 12% |

*(Note: Some sources span multiple categories)*

### Priority Distribution

| Priority | Count | Percentage |
|----------|-------|------------|
| Critical | 3 | 12% |
| High | 16 | 64% |
| Medium | 6 | 24% |
| **Total** | **25** | **100%** |

---

## Phase 3 Focus Areas

### 1. Game Economy Systems

**Material Sources (Where Resources Come From):**
- Gathering nodes and resource generation
- Loot drops and reward systems
- Crafting and production chains
- Player trading and markets
- Quest rewards and daily bonuses

**Material Sinks (Where Resources Go):**
- Item repair and degradation
- Consumable items (food, potions, ammo)
- Crafting costs and failures
- NPC vendors and services
- Territory taxes and fees
- Full-loot PvP mechanics
- Item destruction mechanics

**Economic Balance:**
- Supply/demand equilibrium
- Inflation prevention
- Long-term sustainability
- Player-driven markets
- Economic metrics and monitoring

### 2. Technical Deep Dives

**Performance & Architecture:**
- Advanced GPU techniques
- Data-oriented design (ECS/DOTS)
- Unity optimization best practices
- C# performance patterns

**World Building & AI:**
- AAA world building techniques
- Large-scale agent AI
- Procedural generation refinement
- Open world design patterns

**Networking:**
- Multiplayer programming patterns
- Network optimization
- Distributed systems

---

## Assignment Group Structure (Proposed)

### Group 01: Critical Economy Foundations (3 sources, 30-37h)
- Designing Virtual Worlds (Bartle)
- EVE Online Economic Reports
- Virtual Economies (Lehdonvirta)

### Group 02: Economy Case Studies (5 sources, 25-33h)
- Economics of MMORPGs (GDC)
- Runescape Economic System
- Path of Exile Loot Design
- Albion Online Player Economy
- World of Warcraft Economy Analysis

### Group 03: Economy Design & Balance (4 sources, 14-19h)
- Game Balance Concepts
- Diablo III RMAH Post-Mortem
- Elite Dangerous Resources
- Satisfactory Production

### Group 04: Advanced GPU & Performance (5 sources, 19-26h)
- GPU Gems 3
- Shader Toy Library
- WebGL Noise
- Improving Noise (Perlin)
- Optimizing Unity Performance

### Group 05: Engine Architecture & AI (5 sources, 36-49h)
- AI Game Programming Wisdom
- Unity DOTS - ECS for Agents
- Game Engine Architecture
- Unity ECS/DOTS Documentation
- Building Open Worlds

### Group 06: Advanced Networking & Polish (3 sources, 15-18h)
- GDC Vault: Guerrilla Games
- C# Performance Tricks
- Multiplayer Game Programming

---

## Integration with BlueMarble

### Economic System Requirements

**Material Sources for BlueMarble:**
- Planet surface resource nodes (mining, foraging)
- Creature/NPC drops from hunting
- Crafting and production chains
- Player-owned structures producing resources
- Salvaging and recycling mechanics

**Material Sinks for BlueMarble:**
- Equipment degradation from use and death
- Food and water consumption (survival)
- Building construction and maintenance
- Ammunition and consumables
- Crafting failures and experimentation
- Territory upkeep costs
- Transportation/fuel costs

**Economic Goals:**
- Prevent hyperinflation through balanced sinks
- Encourage player interaction through trade
- Create meaningful choices in resource allocation
- Support long-term economic sustainability
- Enable player-driven markets and pricing

### Technical Integration

- Apply ECS/DOTS patterns for massive NPC economies
- Use GPU techniques for real-time resource visualization
- Implement efficient economic simulation systems
- Design networking for player market systems
- Create data-driven economic balance tools

---

## Success Metrics

**For Economy Research:**
- ✅ Comprehensive material source taxonomy
- ✅ Material sink design patterns documented
- ✅ Economic balance frameworks established
- ✅ Case studies from successful MMORPGs analyzed
- ✅ BlueMarble economic systems designed

**For Technical Research:**
- ✅ Advanced GPU techniques implemented
- ✅ ECS/DOTS patterns applied to BlueMarble
- ✅ Performance optimization guidelines complete
- ✅ Networking patterns documented
- ✅ All code examples tested and validated

---

## Next Steps

1. **Create 6 assignment group files** (similar to Phase 2 format)
2. **Prioritize within each group** for optimal workflow
3. **Begin with Group 01** (Critical Economy Foundations)
4. **Use same batch workflow** (4 sources → summary → wait)
5. **Track discoveries** for potential Phase 4

---

**Created:** 2025-01-17  
**Status:** Ready for Assignment Creation  
**Last Updated:** 2025-01-17  
**Dependencies:** Phase 2 Group 01 Complete ✅  
**Next Phase:** Create assignment files and begin research
