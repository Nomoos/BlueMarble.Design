# BlueMarble - Gameplay Systems Design

**Version:** 1.0  
**Date:** 2025-09-29  
**Author:** BlueMarble Systems Team

## Overview

This document details the core gameplay systems that drive player engagement and progression in BlueMarble. Each system is designed to support the overall vision of a community-driven MMORPG experience.

## Character System

### Character Classes

#### Warrior
- **Role:** Melee DPS/Tank
- **Primary Attributes:** Strength, Constitution
- **Core Abilities:**
  - Shield Wall: Defensive stance reducing incoming damage
  - Charge: Gap-closing attack with knockdown effect
  - Berserker Rage: Temporary damage and speed boost
- **Playstyle:** Front-line fighter, high survivability

#### Ranger
- **Role:** Ranged DPS/Support
- **Primary Attributes:** Dexterity, Perception
- **Core Abilities:**
  - Aimed Shot: High-damage precision attack
  - Track: Reveal nearby enemies and resources
  - Animal Companion: Summon AI ally for assistance
- **Playstyle:** Tactical ranged combat, utility support

#### Mage
- **Role:** Magical DPS/Control
- **Primary Attributes:** Intelligence, Wisdom
- **Core Abilities:**
  - Elemental Blast: Area damage with elemental effects
  - Teleport: Instant repositioning ability
  - Magic Shield: Absorb incoming magical damage
- **Playstyle:** High damage output, crowd control

#### Healer
- **Role:** Support/Healing
- **Primary Attributes:** Wisdom, Intelligence
- **Core Abilities:**
  - Group Heal: Restore health to multiple allies
  - Resurrection: Revive fallen party members
  - Blessing: Temporary stat bonuses for allies
- **Playstyle:** Party support, healing specialization

### Attribute System

#### Primary Attributes
- **Strength:** Increases melee damage and carry capacity
- **Dexterity:** Improves ranged accuracy and movement speed
- **Intelligence:** Enhances magical damage and mana pool
- **Wisdom:** Increases healing effectiveness and mana regeneration
- **Constitution:** Boosts health points and physical resistance
- **Perception:** Improves critical hit chance and detection range

#### Secondary Attributes
- **Health Points:** Total damage character can sustain
- **Mana Points:** Resource for magical abilities
- **Stamina:** Resource for physical abilities
- **Armor Rating:** Physical damage reduction
- **Magic Resistance:** Magical damage reduction
- **Movement Speed:** Character locomotion rate

### Skill Trees

#### Skill Categories
- **Combat Skills:** Weapon proficiencies and combat techniques
- **Magic Schools:** Different schools of magical knowledge
- **Crafting Skills:** Creation of items and equipment
- **Social Skills:** Trading, leadership, and communication
- **Survival Skills:** Resource gathering and environmental adaptation

#### Progression Mechanics
- **Skill Points:** Earned through leveling and achievements
- **Practice System:** Skills improve through use
- **Mentorship:** Players can teach others for mutual benefits
- **Skill Caps:** Maximum proficiency levels with soft caps

## Combat System

### Real-time Combat Mechanics

#### Core Combat Loop
1. **Target Selection:** Click or tab-target enemies
2. **Ability Activation:** Use hotkeys or click abilities
3. **Cooldown Management:** Track ability recharge times
4. **Resource Management:** Monitor health, mana, and stamina
5. **Positioning:** Move strategically for advantage

#### Damage Types
- **Physical:** Reduced by armor rating
- **Magical:** Reduced by magic resistance
- **Elemental:** Fire, ice, lightning with special effects
- **True Damage:** Bypasses all defenses (rare)

#### Status Effects
- **Buffs:** Temporary positive effects (strength boost, haste)
- **Debuffs:** Temporary negative effects (poison, slow, silence)
- **Conditions:** Environmental effects (burning, frozen, stunned)

### PvE Combat

#### Enemy Types
- **Standard Mobs:** Basic enemies for regular combat
- **Elite Enemies:** Stronger foes with special abilities
- **Boss Encounters:** Major enemies requiring strategy
- **World Bosses:** Large-scale encounters for multiple players

#### Difficulty Scaling
- **Level-based:** Enemies scale with player level
- **Dynamic Difficulty:** Adjusts based on player performance
- **Group Scaling:** Encounters scale with party size
- **Regional Variation:** Different zones have different difficulty ranges

### PvP Combat

#### PvP Zones
- **Safe Zones:** No PvP allowed (cities, newbie areas)
- **Optional PvP:** Players can flag for PvP combat
- **Open PvP:** Always-on player combat in designated areas
- **Structured PvP:** Battlegrounds and arena matches
  > **Tournament System Research:** See [Anime Tournament and Progression Systems Analysis](../../research/literature/media-analysis-anime-tournament-progression-systems.md)
  > for detailed tournament mechanics, ranking systems, and competitive progression design.

#### PvP Mechanics
- **Honor System:** Reputation based on PvP behavior
- **Kill Rewards:** Experience and items from PvP victories
- **Death Penalties:** Temporary stat reduction or item loss
- **Anti-griefing:** Systems to prevent harassment

## Progression System

### Experience and Leveling

#### Experience Sources
- **Combat:** Defeating enemies provides XP
- **Quests:** Completing objectives grants large XP rewards
- **Discovery:** Finding new locations awards exploration XP
- **Crafting:** Creating items provides crafting XP
- **Social:** Group activities provide bonus XP

#### Level Benefits
- **Attribute Points:** Increase primary attributes
- **Skill Points:** Unlock new abilities and improvements
- **Health/Mana:** Base stats increase with level
- **Equipment Access:** Higher level gear becomes available

### Equipment System

#### Equipment Slots
- **Weapon:** Primary combat tool
- **Off-hand:** Secondary weapon or shield
- **Armor:** Head, chest, legs, feet protection
- **Accessories:** Rings, amulets, trinkets
- **Tools:** Crafting and gathering implements

#### Item Quality Tiers
- **Common (Gray):** Basic equipment with standard stats
- **Uncommon (Green):** Improved stats with minor bonuses
- **Rare (Blue):** Significant bonuses and special properties
- **Epic (Purple):** Powerful items with unique effects
- **Legendary (Orange):** Extremely rare items with game-changing abilities

#### Enhancement System
- **Upgrading:** Improve item stats using materials
- **Enchanting:** Add magical properties to equipment
- **Socketing:** Insert gems for additional bonuses
- **Repairing:** Maintain equipment condition

### Achievement System

#### Achievement Categories
- **Combat:** Defeat specific enemies or reach milestones
- **Exploration:** Discover locations and complete areas
- **Social:** Participate in guilds and community events
- **Crafting:** Master skills and create rare items
- **Collection:** Gather sets of items or resources

#### Achievement Rewards
- **Titles:** Cosmetic recognition for accomplishments
- **Cosmetics:** Unique visual items and effects
- **Stats:** Permanent character improvements
- **Access:** Unlock new areas or features

## Economy System

### Currency Types

#### Primary Currency
- **Gold Coins:** Standard transaction medium
- **Silver Coins:** Medium-value transactions
- **Copper Coins:** Small transactions and change

#### Special Currencies
- **Honor Points:** Earned through PvP activities
- **Faction Tokens:** Reputation with specific groups
- **Event Currency:** Limited-time seasonal events
- **Premium Currency:** Optional purchase for convenience items

### Trading System

#### Player-to-Player Trading
- **Direct Trade:** Face-to-face item exchange
- **Secure Trading:** System-mediated safe trading
- **Trade Chat:** Communication channel for commerce
- **Reputation System:** Track reliable trading partners

#### Auction House
- **Listing System:** Players post items for sale
- **Bidding:** Competitive pricing through auctions
- **Buyout Prices:** Instant purchase options
- **Search Functions:** Filter by category, level, stats

### Crafting System

> **Detailed Mechanics:** See [Crafting Mechanics Documentation](../gameplay/mechanics/crafting-mechanics-overview.md)
> for formal mathematical models, success rates, and quality calculations.

#### Crafting Professions
- **Blacksmithing:** Create weapons and armor
- **Alchemy:** Brew potions and magical items
- **Tailoring:** Craft clothing and light armor
- **Engineering:** Build mechanical devices and tools
- **Cooking:** Prepare food buffs and consumables

#### Resource Gathering
- **Mining:** Extract ore and precious metals
- **Herbalism:** Collect plants and magical components
- **Hunting:** Obtain leather and animal products
- **Fishing:** Catch fish and aquatic resources
- **Logging:** Harvest wood and forest materials

## Social Systems

### Guild System

#### Guild Features
- **Guild Halls:** Shared spaces for guild members
- **Guild Bank:** Shared storage for items and currency
- **Guild Ranks:** Hierarchical permission system
- **Guild Events:** Organized activities and competitions
- **Guild Wars:** Competitive guild vs guild content

#### Guild Benefits
- **Experience Bonuses:** Increased XP when playing with guildmates
- **Skill Sharing:** Members can teach skills to each other
- **Group Content:** Access to guild-only dungeons and raids
- **Social Features:** Private chat channels and calendars

### Communication Systems

#### Chat Channels
- **Say:** Local area communication
- **Whisper:** Private messages between players
- **Guild:** Guild-only communication channel
- **Trade:** Server-wide trading channel
- **General:** Open server communication

#### Social Features
- **Friends List:** Track online status of favorite players
- **Ignore List:** Block communication from unwanted players
- **Mentorship:** Experienced players guide newcomers
- **Player Reporting:** System for reporting inappropriate behavior

## Event System

### Dynamic Events

#### World Events
- **Invasions:** Large-scale enemy attacks on settlements
- **Resource Booms:** Temporary increased gathering yields
- **Weather Events:** Environmental effects on gameplay
- **Seasonal Changes:** Regular world state changes

#### Player-Driven Events
- **Tournaments:** Player-organized competitions
  > **Detailed Research:** See [Anime Tournament and Progression Systems Analysis](../../research/literature/media-analysis-anime-tournament-progression-systems.md)
  > for comprehensive tournament system design inspired by Hunter x Hunter and Tower of God.
- **Market Events:** Economic opportunities and challenges
- **Exploration Expeditions:** Group discovery missions
- **Cultural Events:** Role-playing and social gatherings

### Scheduled Events

#### Daily Events
- **Daily Quests:** New objectives each day
- **Happy Hour:** Increased experience or rewards
- **Boss Spawns:** Predictable world boss appearances

#### Weekly Events
- **Guild Competitions:** Inter-guild challenges
- **Themed Events:** Special gameplay modes
- **Developer Q&A:** Community interaction sessions

## Balancing Considerations

### Power Progression
- **Horizontal Growth:** New abilities without power inflation
- **Vertical Limits:** Cap on maximum character power
- **Skill Diversity:** Multiple viable character builds
- **Equipment Variety:** Different gear for different playstyles

### Player Retention
- **Daily Goals:** Short-term objectives to maintain engagement
- **Long-term Progression:** Extended character development paths
- **Social Bonds:** Systems that encourage lasting relationships
- **Content Variety:** Multiple activities to prevent repetition

---

*This systems design document provides the foundation for implementing engaging gameplay mechanics. Regular playtesting and iteration will be essential for balancing and refinement.*