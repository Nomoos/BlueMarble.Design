# Player Stats and Attribute Systems Research

**Document Type:** Market Research Report  
**Version:** 1.0  
**Author:** BlueMarble Game Design Research Team  
**Date:** 2025-01-15  
**Status:** In Progress  
**Research Type:** Market Research  
**Priority:** Medium

## Executive Summary

This research document analyzes player character attribute and stat systems from leading MMORPGs to identify design patterns applicable to BlueMarble's geological simulation game. The document examines how different games structure base attributes, derived statistics, and character progression mechanics.

**Key Findings:**

- Most MMORPGs use 4-8 core attributes that influence derived stats
- Attributes typically fall into physical, mental, and social categories
- Derived stats (health, stamina, carry weight) scale from base attributes
- Modern games trend toward simplified systems with clear player impact
- Geological simulation requires specialized stats for environmental interaction

**Recommendations for BlueMarble:**

- Implement 6 core attributes: Strength, Endurance, Agility, Intelligence, Perception, Willpower
- Tie attributes to geological skills and environmental survival
- Use derived stats for practical gameplay mechanics (carry capacity, stamina, tool efficiency)
- Balance attribute importance across different playstyles (gathering, crafting, exploration)

## Table of Contents

1. [Research Objectives](#research-objectives)
2. [Methodology](#methodology)
3. [Core Attribute Systems](#core-attribute-systems)
4. [Derived Statistics](#derived-statistics)
5. [Attribute Progression Models](#attribute-progression-models)
6. [Comparative Analysis](#comparative-analysis)
7. [BlueMarble Application](#bluemarble-application)
8. [Implementation Recommendations](#implementation-recommendations)
9. [References](#references)

## Research Objectives

### Primary Research Questions

1. What core attribute systems do successful MMORPGs use?
2. How do attributes influence gameplay mechanics and player experience?
3. What models exist for attribute progression and character development?
4. Which attribute patterns best suit geological simulation gameplay?

### Secondary Research Questions

1. How do attributes relate to skill systems in successful games?
2. What derived statistics provide meaningful player feedback?
3. How do games balance attribute importance across playstyles?
4. What UI/UX patterns make attribute systems accessible?

### Success Criteria

This research is successful if it:

- Documents attribute systems from 5+ leading MMORPGs
- Identifies common patterns and successful innovations
- Provides specific recommendations for BlueMarble's geological context
- Delivers actionable implementation guidance

## Methodology

### Research Approach

Qualitative analysis of attribute systems from diverse MMORPG genres, focusing on mechanics that support skill-based gameplay and environmental interaction.

### Data Collection Methods

- **Documentation Review:** Official game guides and wikis
- **System Analysis:** Attribute calculations and progression curves
- **Comparative Study:** Cross-game pattern identification
- **Design Analysis:** UI/UX for attribute presentation

### Games Analyzed

- World of Warcraft (traditional theme park MMORPG)
- EVE Online (skill-based sandbox)
- RuneScape (hybrid progression system)
- Elder Scrolls Online (action-oriented attributes)
- Life is Feudal (realistic survival simulation)
- Wurm Online (sandbox crafting focus)

### Limitations

- Focus on attribute mechanics relevant to geological simulation
- PvP-specific attributes excluded
- Analysis current as of 2025

## Core Attribute Systems

### World of Warcraft

**Core Attributes (Modern):**
- Strength (melee physical damage, carry capacity)
- Agility (ranged physical damage, dodge, crit)
- Intellect (spell power, mana pool)
- Stamina (health points)

**Evolution:** Classic WoW had Spirit and additional stats (removed for simplification)

**Key Insights:**
- Simplified from 5 to 4 core attributes over time
- Each attribute clearly tied to specific playstyles
- Stamina universal across all classes
- Attribute scaling straightforward and predictable

### EVE Online

**Core Attributes:**
- Intelligence (science, engineering skills)
- Memory (spaceship command, navigation)
- Perception (missile, gunnery systems)
- Willpower (shield, armor systems)
- Charisma (social, corporation skills)

**Key Insights:**
- Attributes affect skill training speed, not direct combat stats
- Long-term investment system (remap cooldowns)
- Encourages specialization through attribute allocation
- Attributes persist across character lifetime

### RuneScape

**Core Attributes (Skills as Stats):**
- Attack (melee accuracy)
- Strength (melee damage)
- Defence (damage reduction, armor)
- Constitution (health points)
- Ranged (ranged combat)
- Magic (spell casting)
- Prayer (temporary buffs)

**Derived Attributes:**
- Combat Level (calculated from combat skills)
- Total Level (sum of all skill levels)

**Key Insights:**
- No separation between skills and attributes
- Each "attribute" is a trainable skill
- Straightforward progression without complex builds
- Skills directly impact relevant mechanics

### Elder Scrolls Online

**Core Attributes:**
- Health (survivability)
- Magicka (spell casting resource)
- Stamina (physical abilities resource)

**Key Insights:**
- Minimalist approach (only 3 attributes)
- Player distributes points freely on level up
- Resources double as attributes and power pools
- Extremely accessible system

### Life is Feudal

**Core Attributes:**
- Strength (melee damage, carry capacity)
- Agility (attack speed, movement)
- Constitution (health, stamina)
- Willpower (faith-based abilities, mental resistance)
- Intelligence (faster skill gain in crafting/gathering)

**Key Insights:**
- Attributes start at 10, raised through gameplay
- Training expensive and time-consuming
- Soft-capped at 100 (hard cap 120 with bonuses)
- Attributes support skill-based gameplay focus

### Wurm Online

**Core Attributes:**
- Body Strength (carry capacity, combat)
- Mind Logic (faster skill gain, spellcasting)
- Mind Speed (action timer reduction)
- Body Stamina (endurance, action points)
- Soul Strength (prayer power, channeling)
- Soul Depth (prayer power, faith)
- Body Control (climbing, balance, combat stance)

**Key Insights:**
- 7 attributes organized by Body, Mind, Soul
- Attributes improve through use (like skills)
- Low starting values, slow progression
- Heavy integration with skill system

## Derived Statistics

### Common Derived Stats

**Health/Hit Points:**
- Primary derived stat across all games
- Typically scales from Constitution/Stamina/Strength
- Linear or polynomial scaling curves
- Critical for survival gameplay

**Stamina/Endurance:**
- Action point pool for activities
- Scales from Endurance/Constitution attributes
- Regenerates over time
- Gates continuous action performance

**Carry Capacity:**
- Weight limit for inventory
- Usually derived from Strength
- Affects exploration and gathering gameplay
- Important for resource-based games

**Movement Speed:**
- Base speed + modifiers from Agility/Dexterity
- Often affected by carried weight
- Critical for world traversal
- May have mount/vehicle alternatives

**Regeneration Rates:**
- Health recovery out of combat
- Stamina recovery rate
- Mana/resource regeneration
- Scales from Willpower/Constitution

### BlueMarble-Specific Derived Stats

**Tool Efficiency:**
- How quickly tools degrade during use
- Based on Intelligence and Perception
- Encourages careful resource usage

**Environmental Resistance:**
- Tolerance to temperature extremes
- Protection from hazards
- Scales from Endurance and Willpower

**Geological Perception:**
- Ability to identify resources visually
- Range for detecting ore deposits
- Based on Perception and Intelligence

## Attribute Progression Models

### Level-Based Allocation

**Model:** Players receive attribute points per level to distribute freely

**Examples:**
- Elder Scrolls Online (1 point per level)
- Traditional D&D-style RPGs

**Pros:**
- Player agency and build variety
- Clear progression milestones
- Easy to understand

**Cons:**
- Min-maxing and "wrong" builds possible
- Can feel arbitrary if not tied to narrative
- Requires careful balance

### Use-Based Improvement

**Model:** Attributes improve through relevant actions

**Examples:**
- Wurm Online (attributes gain through use)
- Early Elder Scrolls games (Morrowind/Oblivion)

**Pros:**
- Natural progression tied to gameplay
- No separate attribute training needed
- Encourages diverse activities

**Cons:**
- Grind-prone gameplay
- Can be gamified/exploited
- Slower progression feel

### Training-Based Improvement

**Model:** Players spend resources (time/money) to train attributes

**Examples:**
- Life is Feudal (expensive attribute training)
- EVE Online (skill training time)

**Pros:**
- Deliberate character development
- Economic resource sink
- Supports specialization

**Cons:**
- Can feel like waiting/grinding
- May favor pay-to-skip mechanics
- Less connection to active gameplay

### Hybrid Models

**Model:** Combine multiple progression methods

**Examples:**
- World of Warcraft (gear stats + base attributes)
- RuneScape (skills as stats + equipment bonuses)

**Pros:**
- Multiple progression paths
- Can balance different player preferences
- Rich character development system

**Cons:**
- More complex to balance
- Steeper learning curve
- Requires more extensive documentation

## Comparative Analysis

### Attribute Count vs Complexity

| Game | Core Attributes | Derived Stats | Complexity Rating |
|------|----------------|---------------|-------------------|
| World of Warcraft | 4 | ~8 | Low |
| Elder Scrolls Online | 3 | ~5 | Very Low |
| EVE Online | 5 | 0 (indirect) | Medium |
| RuneScape | ~10 (skills) | ~5 | Medium |
| Life is Feudal | 5 | ~10 | Medium-High |
| Wurm Online | 7 | ~15 | High |

**Observations:**
- Modern games trend toward fewer, clearer attributes
- Sandbox games maintain higher complexity for depth
- Derived stats provide gameplay depth without attribute bloat
- Player accessibility improves with 3-5 core attributes

### Attribute Impact on Gameplay

**High Impact Systems:**
- Attributes directly determine combat effectiveness
- Clear "right" and "wrong" attribute choices per playstyle
- Examples: WoW (class-specific attributes), ESO (resource pools)

**Medium Impact Systems:**
- Attributes support skills but don't dominate
- Gradual improvement rather than dramatic shifts
- Examples: Life is Feudal, Wurm Online

**Low Impact Systems:**
- Attributes affect secondary mechanics only
- Skill training speed rather than direct power
- Examples: EVE Online

**Best for BlueMarble:** Medium impact supporting skill-based gameplay

## BlueMarble Application

### Recommended Core Attributes

**Strength (STR)**
- **Primary:** Carry capacity, melee tool efficiency
- **Secondary:** Physical stamina pool, lifting heavy objects
- **Geological Use:** Moving large rock samples, hauling ore
- **Skill Synergy:** Mining, Construction, Resource Gathering

**Endurance (END)**
- **Primary:** Health points, stamina pool
- **Secondary:** Environmental resistance, stamina recovery
- **Geological Use:** Long exploration journeys, harsh conditions
- **Skill Synergy:** Exploration, Survival, Physical Labor

**Agility (AGI)**
- **Primary:** Movement speed, dodge/reaction time
- **Secondary:** Climbing, balancing on terrain
- **Geological Use:** Navigating difficult terrain, cave exploration
- **Skill Synergy:** Climbing, Navigation, Acrobatics

**Intelligence (INT)**
- **Primary:** Skill learning rate, tool crafting quality
- **Secondary:** Pattern recognition, geological analysis
- **Geological Use:** Identifying rock types, understanding strata
- **Skill Synergy:** Geology, Engineering, Research

**Perception (PER)**
- **Primary:** Resource detection range, sample quality assessment
- **Secondary:** Spotting hazards, tracking changes
- **Geological Use:** Finding ore deposits, noticing geological events
- **Skill Synergy:** Prospecting, Surveying, Observation

**Willpower (WIL)**
- **Primary:** Mental stamina, focus during complex tasks
- **Secondary:** Resistance to fear/panic, tool precision
- **Geological Use:** Steady hands for delicate work, danger composure
- **Skill Synergy:** Precision Crafting, Scientific Method, Leadership

### Derived Statistics for BlueMarble

**Health Points (HP)**
- Formula: `HP = 100 + (END * 10) + (STR * 2)`
- Purpose: Survival capacity, injury resistance
- Gameplay: Death/injury from hazards, falls, environmental damage

**Stamina Points (SP)**
- Formula: `SP = 100 + (END * 5) + (STR * 3)`
- Purpose: Physical action resource
- Gameplay: Mining, crafting, climbing, sprinting consume stamina
- Recovery: Rests, food, sleep restore stamina

**Carry Capacity (CC)**
- Formula: `CC = 50 + (STR * 5) + (END * 2)` kg
- Purpose: Inventory weight limit
- Gameplay: Overweight = movement penalty, can't run
- Modification: Backpacks, mounts increase capacity

**Tool Durability Multiplier (TDM)**
- Formula: `TDM = 1.0 + (INT * 0.01) + (PER * 0.005)`
- Purpose: Tools last longer with higher attributes
- Gameplay: Skilled users maintain tools better

**Resource Detection Range (RDR)**
- Formula: `RDR = 10 + (PER * 0.5) + (INT * 0.2)` meters
- Purpose: How far player can spot resources
- Gameplay: Higher perception spots ore deposits from farther away

**Action Speed Modifier (ASM)**
- Formula: `ASM = 1.0 + (AGI * 0.005) + (INT * 0.003)`
- Purpose: Speed multiplier for crafting/gathering actions
- Gameplay: Faster completion of tasks

### Progression Model for BlueMarble

**Recommended Model:** Hybrid Use-Based + Milestone Training

**Passive Improvement (Use-Based):**
- Attributes gain small increments through relevant gameplay
- STR increases when carrying heavy loads
- END increases through extended physical activity
- INT increases through successful crafting/research
- PER increases through resource gathering
- AGI increases through movement activities
- WIL increases through focused, complex tasks

**Active Training (Milestone-Based):**
- Major attribute gains at skill tier milestones
- +2 attribute points when reaching Journeyman in related skill
- +3 attribute points when reaching Expert in related skill
- +5 attribute points when reaching Master in related skill
- Player chooses which attribute(s) to improve

**Example Progression:**
```
Player starts: All attributes at 10 (total 60)
After 100 hours gameplay (use-based): ~15-20 per attribute (90-120 total)
With 5 Journeyman skills: +10 additional points (100-130 total)
With 2 Expert skills: +6 additional points (106-136 total)
With 1 Master skill: +5 additional points (111-141 total)
Maximum realistic: ~150-180 total (25-30 per attribute)
```

**Soft Cap:** 30 per attribute (achievable through dedicated play)  
**Hard Cap:** 40 per attribute (requires extraordinary effort)

### Balancing Across Playstyles

**Gatherer/Miner Focus:**
- Primary: STR, END, PER
- Tools: Pickaxes, hammers, collection tools
- Gameplay: High carry capacity, long stamina, spot resources

**Crafter/Engineer Focus:**
- Primary: INT, PER, WIL
- Tools: Precision tools, workbenches, blueprints
- Gameplay: Efficient tool use, quality output, pattern recognition

**Explorer/Surveyor Focus:**
- Primary: END, AGI, PER
- Tools: Climbing gear, mapping tools, survival equipment
- Gameplay: Terrain navigation, hazard avoidance, wide exploration

**Researcher/Geologist Focus:**
- Primary: INT, PER, WIL
- Tools: Analysis equipment, documentation tools, samples
- Gameplay: Understanding systems, identifying patterns, knowledge discovery

**Balanced Generalist:**
- Moderate investment across all attributes
- Flexibility to engage with multiple systems
- Slower progression in any single specialization

## Implementation Recommendations

### Phase 1: Core System

1. **Implement 6 base attributes** with starting value of 10 each
2. **Create 6 derived statistics** using formulas above
3. **Build attribute display UI** showing current values and effects
4. **Implement basic progression** (use-based gains only)

### Phase 2: Progression Depth

1. **Add milestone training** at skill tier advancements
2. **Implement soft/hard caps** to encourage specialization
3. **Create attribute respec system** (limited uses, resource cost)
4. **Balance progression rates** through playtesting

### Phase 3: Polish and Integration

1. **Integrate with skill system** for synergy bonuses
2. **Add gear/equipment bonuses** that modify attributes
3. **Create temporary buffs/debuffs** (food, weather, status effects)
4. **Implement attribute-based dialogue/quest options** for RPG depth

### Technical Considerations

**Data Storage:**
```json
{
  "attributes": {
    "strength": 15,
    "endurance": 12,
    "agility": 10,
    "intelligence": 18,
    "perception": 16,
    "willpower": 14
  },
  "derived": {
    "health": 270,
    "stamina": 206,
    "carryCapacity": 99,
    "toolDurabilityMultiplier": 1.26,
    "resourceDetectionRange": 20.6,
    "actionSpeedModifier": 1.104
  }
}
```

**Performance:**
- Calculate derived stats on attribute change only (not every frame)
- Cache derived stat values
- Use event system to notify UI of changes

**Balancing:**
- Log attribute progression rates during alpha testing
- Monitor which attributes players prioritize
- Adjust progression curves based on data
- Ensure all attributes have clear value across playstyles

## References

### Primary Sources

1. World of Warcraft Official Game Guide - Attributes and Stats
2. EVE Online Documentation - Character Attributes
3. RuneScape Wiki - Skills and Stats System
4. Elder Scrolls Online - Character Creation Guide
5. Life is Feudal Wiki - Attributes Guide
6. Wurm Online Documentation - Characteristics System

### Secondary Sources

1. "Game Balance" by Ian Schreiber - Attribute System Design
2. "Theory of Fun for Game Design" by Raph Koster - Progression Systems
3. "Designing Virtual Worlds" by Richard Bartle - Character Attributes in MMOs
4. GDC Talks on Character Progression Systems

### Related BlueMarble Research

- [Skill and Knowledge System Research](skill-knowledge-system-research.md)
- [Realistic Basic Skills Research](realistic-basic-skills-research.md)
- [Skill Relationships and Compatibility Research](skill-relationships-compatibility-research.md)
- [Life is Feudal Skill Specialization Research](life-is-feudal-skill-specialization-system-research.md)

---

**Document Status:** In Progress - Ready for Review  
**Last Updated:** 2025-01-15  
**Next Steps:** Validation through prototyping and playtesting
