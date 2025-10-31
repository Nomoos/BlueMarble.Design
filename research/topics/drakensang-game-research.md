---
title: Drakensang Game Research
date: 2025-10-31
owner: @Nomoos
status: complete
tags: [game-design, mmorpg, action-rpg, drakensang, character-progression, crafting-systems, pvp, monetization]
---

# Drakensang Game Research

## Problem / Context

What are the core design principles, mechanics, and systems used in the Drakensang game series that could be applicable to BlueMarble's MMORPG development? This research explores both the single-player Drakensang: The Dark Eye games and the online MMORPG Drakensang Online to identify design patterns, progression systems, and gameplay mechanics relevant to BlueMarble's development.

- Understanding character progression and skill tree systems
- Analyzing crafting and equipment upgrade mechanics
- Examining PvP and multiplayer content design
- Investigating monetization models for free-to-play MMORPGs
- Identifying design philosophy from tabletop RPG adaptation

## Key Findings

### Drakensang Series Overview

- **Two Distinct Product Lines**:
  - **Single-Player RPGs** (2008-2010): Deep, tactical party-based RPGs based on The Dark Eye tabletop system
  - **Online MMORPG** (2011-present): Free-to-play action MMORPG with Diablo-like gameplay

- **Development History**:
  - Original games by Radon Labs (2008-2010)
  - Transitioned to Bigpoint for online version after Radon Labs bankruptcy
  - Single-player games used Nebula Device engine (later MIT licensed)

### Character Progression Systems

- **Multi-Layered Skill Trees**: Drakensang Online uses four distinct progression tabs
  - **Experience Tree**: Base class skills unlocked through leveling
  - **Wisdom Tree**: Passive enhancements (attack speed, crit damage, health)
  - **Group Tree**: Party-focused talents and buffs
  - **Honor/Fame Tree**: PvP-specific abilities and stat boosts

- **Class Specialization**: Four distinct classes with unique playstyles
  - Dragonknight (melee warrior, fury-based)
  - Spellweaver (elemental magic)
  - Ranger (archer with stealth/melee hybrid)
  - Steam Mechanicus (engineer with gadgets)

- **Build Diversity**: Synergy between skills, wisdom passives, and equipment sets enables varied builds

### Crafting and Equipment Systems

- **Rarity-Based Progression**:
  - Item rarities: Common → Improved → Magic → Extraordinary → Legendary → Unique → Set
  - Crafting combines 4 items of same rarity to create 1 item of next tier
  - Common items cannot be crafted (vendor trash)

- **Enchantment Transfer System**:
  - Most powerful crafting mechanic
  - Percentage-based bonuses modifying base stats
  - Number of enchantments scales with rarity (Improved: 1, Magic: 2, Extraordinary: 3, Legendary: 4)
  - Requires unlocking through achievements and quest progression

- **Glyphs of Power (GoP) Upgrade System**:
  - Separate from crafting
  - Earned by melting non-common items at blacksmith
  - Upgrades both base values and enchantments
  - Exponential cost increase with higher levels/rarities

- **Boss-Specific Recipes**:
  - Unique/Set items require specific recipes from boss kills (typically 100+ kills)
  - Requires boss drops, materials, and parallel world rings

### PvP and Multiplayer Content

- **Structured PvP Modes**:
  - Duel (1v1): Best-of-three rounds
  - Death Match (3v3): Small group battles
  - Capture the Flag (5v5): Objective-based team combat
  - Storm the Fortress (6v6): Large-scale strategic combat with turrets

- **PvP Progression**:
  - Badges of Honor currency earned from matches
  - Performance-based rewards (damage, kills, objectives)
  - Seasonal leaderboards with rank-based rewards
  - Separate progression tree (Fame) for PvP

- **Group PvE Content**:
  - Cooperative dungeons and raids
  - Heroic difficulty modes
  - Guild systems with social bonuses
  - Team-based exploration and questing

### Economy and Monetization

- **Dual Currency System**:
  - Gold: Earned through gameplay
  - Andermant: Premium currency (limited free earning, primarily purchased)

- **Monetization Strategy**:
  - No player-to-player trading (intentional design choice)
  - Microtransactions for convenience, cosmetics, and progression acceleration
  - Time-saving options (revives, inventory space, mounts, costumes)
  - Limited-time event items and bundles

- **Design Rationale**:
  - Preventing gold farming and market manipulation
  - Avoiding item inflation
  - Maintaining game balance
  - Protecting monetization integrity

### Single-Player RPG Design (The Dark Eye)

- **Tabletop Adaptation Philosophy**:
  - Accessibility for newcomers while satisfying veteran fans
  - Simplification of complex tabletop rules for PC
  - Retention of core mechanics (attributes, talents, skill checks)

- **Deep Character System**:
  - 20 distinct classes with race restrictions
  - 8 primary attributes
  - 13 combat talents (weapon-specific)
  - 24 regular talents (crafting, social, exploration)
  - Individual spell system for magic users

- **Skill-Based Interactions**:
  - Multi-attribute skill checks (e.g., Cleverness, Courage, Intuition for skinning)
  - Talent-based crafting (alchemy, blacksmithing)
  - Recipe learning through exploration

- **Tactical Combat**:
  - Real-time with pause (similar to Baldur's Gate)
  - Command queuing with keyboard shortcuts
  - Positioning and skill synergy emphasis

## Evidence

### Source 1: Drakensang Online Official

- **Link**: https://www.drakensang.com/en/info
- **Key Points**:
  - Free-to-play action MMORPG with isometric perspective
  - Four unique character classes with distinct abilities
  - PvE and PvP content variety
  - Browser-based origins, now client-based
- **Relevance**: Core game overview and design direction

### Source 2: Drakensang Online Wiki

- **Link**: https://drakensangonline.fandom.com/wiki/Drakensang_Online_Wiki
- **Key Points**:
  - Detailed documentation of skill trees (Experience, Wisdom, Group, Honor)
  - Comprehensive crafting system documentation
  - PvP modes and rewards structure
  - Item progression and rarity tiers
- **Relevance**: Deep dive into game systems and mechanics

### Source 3: Game Design Document (DSA-Museum)

- **Link**: https://dsa-museum.de/drakensang-game-design-document/
- **Key Points**:
  - Design philosophy documentation from original developers
  - The Dark Eye 4.0 rules adaptation approach
  - Lead designers: Fabian Rudzinski and Bernd Beyreuther
  - Balancing accessibility with depth
- **Relevance**: Insight into design philosophy and decision-making process

### Source 4: Dark Legacy Update

- **Link**: https://www.bigpoint.net/press/drakensang_darklegacy/
- **Key Points**:
  - Complete skill tree overhaul
  - Item drop and crafting revamp
  - UI improvements and streamlined hotkey bars
  - Enhanced build diversity
- **Relevance**: Evolution of progression systems based on player feedback

### Source 5: Community Forums

- **Link**: https://board-en.drakensang.com/
- **Key Points**:
  - Player requests for market/trading systems (consistently rejected)
  - Equipment crafting guides and community knowledge
  - Build diversity discussions
  - Feedback on skill tree depth
- **Relevance**: Player perspective on design decisions and pain points

## Implications for Design

### Character Progression

- **Multi-Tree Approach**: BlueMarble could benefit from separating combat, passive, social, and competitive progression into distinct trees
  - Allows players to specialize in different aspects (PvE, PvP, crafting, social)
  - Provides clear progression paths for different playstyles
  - Enables balanced advancement across multiple dimensions

- **Skill-Gear Synergy**: Equipment should enhance or modify skills, not just provide stats
  - Creates meaningful gear choices beyond "higher numbers"
  - Encourages build experimentation
  - Increases replay value through different synergies

### Crafting Systems

- **Rarity Tiers with Meaningful Gates**: Progressive rarity system with achievement/quest unlocks
  - Prevents immediate access to best gear
  - Provides long-term goals
  - Creates sense of mastery and accomplishment
  - Consider for BlueMarble's geological material progression

- **Resource Sink Mechanisms**: Glyphs of Power system demonstrates effective resource management
  - Melting unwanted items for upgrade materials
  - Prevents inventory bloat
  - Creates economic balance
  - Applicable to BlueMarble's mining/resource systems

- **Enchantment/Modification System**: Transferable stat bonuses create depth
  - Players can preserve valuable bonuses while upgrading base items
  - Adds strategic layer to crafting
  - Could apply to BlueMarble's material property system

### PvP Design

- **Structured Competitive Modes**: Variety of formats (1v1, 3v3, 5v5, 6v6) caters to different preferences
  - Consider objective-based modes beyond simple combat
  - Seasonal rankings create ongoing engagement
  - Separate progression trees respect different player interests

- **Separate PvP Balance**: Fame tree and PvP-specific progression
  - Allows balancing PvP without affecting PvE
  - Provides dedicated rewards for competitive players
  - BlueMarble could apply to territorial/resource control

### Economy and Trading

- **No-Trade Model Considerations**:
  - **Pros**: Prevents market manipulation, gold farming, inflation
  - **Cons**: Limits social interaction, player agency, economic gameplay
  - **For BlueMarble**: Consider hybrid approach
    - Allow resource trading (raw materials)
    - Restrict equipment trading (bind on pickup/equip)
    - Enable guild/alliance resource sharing
    - Consider geological resource markets with NPC stabilization

### Monetization (if applicable)

- **Dual Currency with Purpose Separation**:
  - Gameplay currency for normal transactions
  - Premium currency for convenience and cosmetics
  - Avoid pay-to-win perception by limiting power purchases
  - Time-saving vs. power-granting distinction

- **Alternative for BlueMarble**: If avoiding monetization
  - Similar systems can create in-game prestige currencies
  - Achievement-based premium resources
  - Guild contribution systems

## Implications for BlueMarble Specifically

### Geological Simulation Integration

- **Material Rarity System**: Apply Drakensang's rarity tiers to geological materials
  - Common minerals → Improved ores → Magic crystals → Extraordinary gems → Legendary formations
  - Each tier requires specific knowledge/skills to identify and extract
  - Processing chains similar to crafting combination system

- **Knowledge-Based Progression**: Adapt skill check system to geological expertise
  - Multiple attributes for complex tasks (Intelligence, Perception, Dexterity for mineral identification)
  - Talent trees for specializations (mineralogy, metallurgy, gemology, petrology)
  - Recipe system for material processing and refinement techniques

### Multi-Scale World Application

- **Progressive Depth Access**: Similar to rarity unlocks
  - Surface → Shallow mining → Deep mining → Core access
  - Each depth tier requires technological advancement
  - Boss-like challenges (geological hazards, extreme conditions)

### Social Systems

- **Guild Mining Operations**: Adapt group PvE content
  - Cooperative extraction of rare resources
  - Shared guild refineries and workshops
  - Collective knowledge trees for guild specializations

## Open Questions / Next Steps

### Open Questions

- How can Drakensang's structured PvP translate to BlueMarble's geological resource competition?
- What balance between player trading restrictions and economic gameplay works for a mining-focused MMORPG?
- How to adapt enchantment transfer systems to material property modifications?
- Can skill trees accommodate both scientific accuracy and engaging gameplay progression?
- What lessons from tabletop adaptation apply to geological simulation accuracy?

### Next Steps

- [ ] Analyze how rarity tier system could map to geological material classifications
- [ ] Design knowledge/skill tree prototype for geological specializations
- [ ] Evaluate no-trade vs. limited-trade models for resource-based economy
- [ ] Research integration of Drakensang's crafting depth with BlueMarble's material processing
- [ ] Prototype multi-tree progression system (Combat, Knowledge, Social, Resource)
- [ ] Compare with other mining/crafting MMOs (EVE Online, Albion Online) for hybrid design

## Related Documents

- [Advanced Crafting System Research](../game-design/step-2-system-research/step-2.3-crafting-systems/advanced-crafting-system-research.md)
- [Minecraft Technic Mods Dependency Research](../game-design/step-2-system-research/step-2.3-crafting-systems/minecraft-technic-mods-dependency-research.md)
- [Game Design Mechanics Analysis](../literature/game-design-mechanics-analysis.md)
- [Content Design Index](../game-design/step-1-foundation/content-design/README.md)
- [Economy Systems](../../docs/systems/economy-systems.md)
