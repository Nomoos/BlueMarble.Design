# Life is Feudal Material System Analysis

**Document Type:** Market Research & System Analysis  
**Version:** 1.0  
**Author:** BlueMarble Game Design Research Team  
**Date:** 2025-01-20  
**Status:** Research Report  
**Research Type:** Market Research  
**Priority:** Low

## Executive Summary

This research document provides an in-depth analysis of Life is Feudal's material quality and crafting systems, focusing on how these mechanics create interdependence, encourage specialization, and provide meaningful progression. The analysis examines the game's use-based skill system with hard caps, quality scaling mechanics, and specialization tiers to extract actionable lessons for BlueMarble's geological crafting systems.

**Key Findings:**
- Hard skill cap (600 points) forces meaningful specialization and player interdependence
- Quality scaling provides continuous progression beyond binary success/failure
- Skill tier unlocks at 30/60/90 create clear progression milestones
- Alignment system (crafting vs combat) creates distinct character paths
- Material quality directly impacts final product quality, creating economic depth
- Parent-child skill relationships encourage logical skill combinations

## Table of Contents

1. [Research Methodology](#research-methodology)
2. [Material Quality System](#material-quality-system)
3. [Crafting Progression Mechanics](#crafting-progression-mechanics)
4. [Specialization System](#specialization-system)
5. [Skill Cap and Alignment System](#skill-cap-and-alignment-system)
6. [Economic Integration](#economic-integration)
7. [Comparison with BlueMarble](#comparison-with-bluemarble)
8. [Recommendations for BlueMarble](#recommendations-for-bluemarble)
9. [Implementation Considerations](#implementation-considerations)

## Research Methodology

### Research Sources

**Primary Sources:**
- Official Life is Feudal Wiki (https://lifeisfeudal.fandom.com/wiki/Life_Is_Feudal_Wiki)
- Official website (https://lifeisfeudal.com/)
- Developer blog posts and patch notes
- Community forums and player guides

**Analysis Methods:**
- System documentation review
- Player feedback analysis
- Comparison with similar sandbox MMORPGs
- Focus on mechanics applicable to geological simulation

**Research Scope:**
- Material quality and crafting systems (primary focus)
- Skill progression and specialization mechanics
- Economic interdependence systems
- Exclusions: Combat systems, PvP mechanics, territorial warfare

## Material Quality System

### Quality Scale Overview

Life is Feudal uses a continuous quality scale from 0-100 for all items and materials:

```
Quality Scale:
0-9:   Trash tier (rarely seen, severe failures)
10-29: Poor quality (beginner crafters)
30-49: Below average (developing crafters)
50-69: Average quality (journeyman level)
70-89: Good quality (expert crafters)
90-99: Exceptional quality (master crafters)
100:   Perfect quality (extremely rare)
```

### Material Quality Inheritance

**Core Principle:** Output quality is influenced by input material quality

```
Quality Calculation:
Base Quality = (Skill Level / Recipe Requirement) × 100
Material Modifier = Average(Input Material Quality) × Weight
Tool Modifier = Tool Quality × Weight
Random Variation = ±5-10%

Final Quality = Base + Material Mod + Tool Mod + Random
Clamped to: [0, 100]
```

**Example: Iron Ingot Smelting**
```
Player Smelting Skill: 60
Recipe Requirement: 50
Iron Ore Quality: 75

Base Quality = (60/50) × 100 = 120% → 100 (capped)
Material Bonus = 75 × 0.3 = +22.5
Tool Bonus (Basic Furnace) = 50 × 0.1 = +5
Random Roll = +3

Final Quality = 100 + 22.5 + 5 + 3 = 130.5 → 100 (capped)
Result: Quality 100 iron ingot (maximum quality)
```

### Material Quality Effects

**Crafting Impact:**
- Higher quality materials produce higher quality products
- Quality loss at each processing step (typically 5-15%)
- Multi-stage recipes accumulate quality bonuses/penalties
- Some recipes require minimum material quality thresholds

**Economic Impact:**
- Quality affects item durability (higher quality = longer lasting)
- Quality affects tool efficiency (better tools work faster)
- Quality affects weapon/armor effectiveness
- Quality creates distinct market tiers and pricing

**Example: Multi-Stage Processing Chain**
```
Stage 1: Mining Iron Ore
└─ Player Mining Skill: 70 → Quality 75 ore extracted

Stage 2: Smelting to Iron Ingot
├─ Ore Quality: 75
├─ Smelting Skill: 60
└─ Output: Quality 68 ingot (quality degradation)

Stage 3: Forging Iron Sword
├─ Ingot Quality: 68
├─ Blacksmithing Skill: 80
└─ Output: Quality 72 sword (skill compensates)

Final Product Quality: 72/100
- Durability: 720 (instead of base 500)
- Damage: 18-22 (instead of base 15-20)
- Value: 3.5x base price
```

## Crafting Progression Mechanics

### Use-Based Skill Gain

**Core Mechanic:** Skills improve through repeated use, not point allocation

```
Skill Gain Formula:
gain = base_gain × difficulty_modifier × success_modifier × parent_bonus

Where:
- base_gain: Decreases as skill increases (diminishing returns)
- difficulty_modifier: Actions matching skill level give best gains
- success_modifier: Failures still grant small gains ("pain tolerance")
- parent_bonus: Parent skill level provides bonus to child skills
```

**Optimal Leveling Strategy:**
```
Blacksmithing Progression:

Level 0-30: Craft nails (trivial, fast gains)
├─ Gain per success: 0.5-1.0 points
├─ Time per craft: ~10 seconds
└─ Materials: Minimal iron

Level 30-60: Craft horseshoes (moderate difficulty)
├─ Gain per success: 0.3-0.6 points
├─ Time per craft: ~30 seconds
├─ Materials: Moderate iron
└─ Unlocks: Tier 2 recipes

Level 60-90: Craft weapons (challenging)
├─ Gain per success: 0.1-0.3 points
├─ Time per craft: ~2-5 minutes
├─ Materials: Significant iron + other materials
└─ Unlocks: Tier 3 recipes

Level 90-100: Master-level crafting
├─ Gain per success: 0.01-0.05 points
├─ Time per craft: ~10+ minutes
├─ Materials: Rare/expensive materials
└─ Quality: Consistently high output
```

### Skill Tiers and Unlocks

**Three-Tier System:** Skills unlock abilities at specific thresholds

```
Skill Level Milestones:

0-29: Novice Tier
├─ Basic recipes available
├─ High failure rate
├─ Low quality output (10-40)
└─ Limited efficiency

30-59: Journeyman Tier
├─ Tier 2 recipes unlocked
├─ Moderate success rate
├─ Average quality output (40-70)
├─ Improved efficiency
└─ Can use advanced tools

60-89: Expert Tier
├─ Tier 3 recipes unlocked
├─ High success rate
├─ Good quality output (60-90)
├─ High efficiency
└─ Can craft quality tools

90-100: Master Tier
├─ Master recipes unlocked
├─ Very high success rate
├─ Exceptional quality output (80-100)
├─ Maximum efficiency
└─ Can mentor others
```

### Difficulty Curve and Time Investment

**Exponential Progression:** Later skill levels require significantly more time

```
Time to Reach Skill Level (Approximate):

Level 30: ~10-15 hours of focused crafting
Level 60: ~40-60 hours of focused crafting
Level 90: ~200-300 hours of focused crafting
Level 100: ~500-800 hours of focused crafting

Total to Master: ~800-1000 hours per skill
```

**Pain Tolerance Mechanic:**
- Failed crafting attempts still grant skill gains
- Failure gains = ~20-30% of success gains
- Encourages attempting challenging recipes
- Reduces frustration from failure streaks

## Specialization System

### Hard Skill Cap Mechanics

**The 600 Point System:**
```
Total Available Skill Points: 600
Maximum per Skill: 100
Minimum Skills at Cap: 6 skills at 100

Typical Specialization Patterns:

Pattern A: Deep Specialist
├─ 2 skills at 100 (200 points)
├─ 4 skills at 60-80 (280 points)
└─ 4 skills at 30 (120 points)
Total: 600 points across 10 skills

Pattern B: Versatile Specialist
├─ 1 skill at 100 (100 points)
├─ 5 skills at 90 (450 points)
└─ 5 skills at 10 (50 points)
Total: 600 points across 11 skills

Pattern C: Focused Master
├─ 6 skills at 100 (600 points)
└─ All other skills at 0
Total: 600 points across 6 skills
```

### Parent-Child Skill Relationships

**Hierarchical Skill Structure:** Parent skills boost related child skills

```
Example: Metalworking Skill Tree

Artisan [Parent Skill]
├─ Affects: All crafting skills
├─ Bonus: +0.1 per point to all children
└─ At 100: +10 effective points to all crafting

    └─── Smelting [Tier 2 Parent]
         ├─ Requires: Artisan knowledge
         ├─ Enables: Metal ingot production
         └─ Bonus: +0.15 per point to metalworking

              └─── Blacksmithing [Tier 3 Child]
                   ├─ Requires: Smelting + Artisan
                   ├─ Effective Skill = Base + (Artisan×0.1) + (Smelting×0.15)
                   └─ Example: 60 base + (80×0.1) + (70×0.15) = 78.5 effective

Strategic Skill Investment:
- Raise parent skills first for broader benefits
- Deep child specialization with parent support
- Synergy between related skill families
```

### Specialization Archetypes

**Common Player Builds:**

```
1. Master Blacksmith
├─ Primary: Blacksmithing 100
├─ Supporting: Smelting 90, Mining 60, Artisan 80
├─ Total: 330 points in metalworking chain
├─ Role: Weapon/armor/tool production
└─ Trade: High-quality metal goods

2. Combat Specialist
├─ Primary: Melee Combat 100, Heavy Armor 90
├─ Supporting: Various weapon skills 60-80
├─ Total: ~500 points in combat
├─ Role: PvP, defense, hunting
└─ Trade: Protection services, rare materials

3. Farmer/Provisioner
├─ Primary: Farming 100, Cooking 90
├─ Supporting: Herbalism 70, Animal Lore 60
├─ Total: 320 points in food production
├─ Role: Food supply, brewing
└─ Trade: Food, potions, animal products

4. Builder/Architect
├─ Primary: Construction 100, Carpentry 90
├─ Supporting: Masonry 70, Architecture 60
├─ Total: 320 points in building
├─ Role: Fortifications, buildings, furniture
└─ Trade: Construction services, buildings

5. Resource Gatherer
├─ Primary: Mining 100, Logging 80
├─ Supporting: Prospecting 70, Gathering 60
├─ Total: 310 points in gathering
├─ Role: Raw material supply
└─ Trade: Ores, wood, raw resources
```

## Skill Cap and Alignment System

### Alignment Impact on Skills

**Two Distinct Paths:** Crafting vs Combat alignment affects available skills

```
Alignment System:

Crafting Alignment (+100 to -100)
├─ +100: Pure crafter, combat penalties
├─ +50 to +100: Efficient crafting, reduced combat
├─ -50 to +50: Neutral, balanced capabilities
├─ -50 to -100: Efficient combat, reduced crafting
└─ -100: Pure warrior, crafting penalties

Alignment Effects:
- Alignment shifts based on actions performed
- Extreme alignment provides specialization bonuses
- Neutral alignment maintains flexibility
- Changing alignment requires significant effort

Crafting Alignment Benefits (+100):
├─ +15% crafting speed
├─ +10% quality bonus to crafted items
├─ -20% material consumption
└─ Access to master crafter recipes

Combat Alignment Benefits (-100):
├─ +15% damage output
├─ +10% armor effectiveness
├─ +20% stamina efficiency
└─ Access to master combat abilities
```

### Forced Interdependence

**Economic Necessity:** Skill caps create player dependencies

```
Interdependence Examples:

Single Player Cannot:
├─ Master all crafting skills (600 point limit)
├─ Be self-sufficient in all areas
├─ Craft all items at maximum quality
└─ Fill multiple specialized roles

Economic Results:
├─ Trading becomes essential
├─ Guild structures form naturally
├─ Market for specialized services
├─ Social bonds through need
└─ Value in skill diversity

Guild Synergy:
├─ 5-10 specialized members
├─ Complementary skill sets
├─ Resource sharing systems
├─ Knowledge transfer
└─ Collective self-sufficiency
```

### Multi-Account Dynamics

**Player Response:** Some players run multiple accounts

```
Multi-Account Strategies:

Account 1: Combat Specialist
├─ Primary: Combat skills maxed
├─ Purpose: PvP, defense, hunting
└─ Alignment: -100 (combat focused)

Account 2: Crafter Specialist
├─ Primary: Crafting skills maxed
├─ Purpose: Production, trading
└─ Alignment: +100 (crafting focused)

Account 3: Resource Gatherer
├─ Primary: Gathering skills maxed
├─ Purpose: Material supply
└─ Alignment: +50 (crafting leaning)

Trade-offs:
├─ Time investment: 3x the effort
├─ Subscription cost: 3x the price
├─ Complexity: Managing multiple characters
├─ Benefits: Near-total self-sufficiency
└─ Community view: Mixed (some see as gaming system)
```

## Economic Integration

### Quality-Based Market Tiers

**Price Scaling by Quality:**

```
Market Price Formula:
price = base_price × quality_multiplier

Quality Tiers:
├─ 0-29: 0.5-0.8x base price (budget tier)
├─ 30-49: 0.8-1.2x base price (standard tier)
├─ 50-69: 1.2-2.0x base price (quality tier)
├─ 70-89: 2.0-4.0x base price (premium tier)
└─ 90-100: 4.0-10x base price (masterwork tier)

Example: Iron Sword Pricing
├─ Base sword (Q50): 100 gold
├─ Poor sword (Q30): 80 gold
├─ Quality sword (Q70): 250 gold
├─ Premium sword (Q85): 350 gold
└─ Masterwork sword (Q95): 800 gold
```

### Supply Chain Integration

**Multi-Stage Production Economy:**

```
Example: Steel Sword Production Chain

Stage 1: Raw Material Extraction
├─ Miner: Extracts iron ore (Q75)
├─ Skill: Mining 80
├─ Time: 30 minutes
├─ Value: 20 gold (ore)
└─ Sells to: Smelter

Stage 2: Material Processing
├─ Smelter: Creates iron ingots (Q70)
├─ Skill: Smelting 70
├─ Time: 20 minutes
├─ Value: 45 gold (ingot)
└─ Sells to: Blacksmith

Stage 3: Component Creation
├─ Blacksmith: Forges sword blade (Q75)
├─ Skill: Blacksmithing 85
├─ Time: 45 minutes
├─ Value: 200 gold (finished sword)
└─ Sells to: Warrior or Market

Value Chain Analysis:
├─ Total input cost: 65 gold (ore + ingot)
├─ Final product value: 200 gold
├─ Value added: 135 gold
├─ Profit margin: ~67%
└─ Specialist advantage: Quality bonus increases value
```

### Specialization Value

**Economic Benefits of Mastery:**

```
Master vs Journeyman Comparison:

Item: Iron Breastplate

Journeyman Smith (Skill 60):
├─ Success Rate: 70%
├─ Quality Output: 50-65 average
├─ Durability: 600-750
├─ Time per Craft: 8 minutes
├─ Market Value: 150-200 gold
└─ Effective Hourly Rate: 900-1400 gold/hour (with failures)

Master Smith (Skill 95):
├─ Success Rate: 98%
├─ Quality Output: 85-95 average
├─ Durability: 1000-1200
├─ Time per Craft: 6 minutes
├─ Market Value: 450-600 gold
└─ Effective Hourly Rate: 4400-5900 gold/hour

Master Advantage:
├─ 4-5x higher hourly income
├─ Superior reputation
├─ Access to premium clientele
├─ Guild leadership opportunities
└─ Economic moat from specialization
```

## Comparison with BlueMarble

### Similarities

**Systems BlueMarble Already Implements:**

```
1. Quality Scaling Model
├─ BlueMarble: Quality 0-100 for crafted items
├─ Life is Feudal: Quality 0-100 scale
└─ Match: Very similar approach

2. Material Quality Inheritance
├─ BlueMarble: Material quality affects output
├─ Life is Feudal: Input quality affects output
└─ Match: Core mechanic aligned

3. Skill-Based Progression
├─ BlueMarble: Use-based skill improvement
├─ Life is Feudal: Use-based with diminishing returns
└─ Match: Similar philosophy

4. Specialization Bonuses
├─ BlueMarble: +15% bonus for specialization
├─ Life is Feudal: Tier unlocks + quality bonuses
└─ Match: Both reward focus
```

### Key Differences

**Areas Where Systems Diverge:**

```
1. Skill Cap System
├─ BlueMarble: Soft caps, level-based categories
├─ Life is Feudal: Hard cap at 600 total points
├─ Impact: LiF creates stronger forced specialization
└─ Consideration: Hard cap increases interdependence

2. Skill Tier Unlocks
├─ BlueMarble: Continuous progression curve
├─ Life is Feudal: Discrete tiers at 30/60/90
├─ Impact: LiF provides clear milestone goals
└─ Consideration: Tiers create structured objectives

3. Alignment System
├─ BlueMarble: No alignment system
├─ Life is Feudal: Crafting vs Combat alignment
├─ Impact: LiF enforces broader specialization
└─ Consideration: Could adapt to geological vs other focus

4. Parent-Child Skills
├─ BlueMarble: Tag-based skill relationships
├─ Life is Feudal: Explicit parent bonuses
├─ Impact: Both create synergies differently
└─ Consideration: Tag system more flexible

5. Failure Rewards
├─ BlueMarble: Not explicitly documented
├─ Life is Feudal: "Pain tolerance" grants gains
├─ Impact: Reduces frustration from failures
└─ Consideration: Could improve player experience
```

### System Integration Comparison

```
Quality Calculation Comparison:

Life is Feudal:
q_final = (skill/recipe_req × 100) + (material_q × 0.3) + (tool_q × 0.1) + random(±10)

BlueMarble (Current):
q_final = (skill/recipe_skill × 100) + material_bonus + specialization(15) + tool_bonus(10) + random(±10)

Key Differences:
├─ LiF: Simpler formula, fewer modifiers
├─ BM: More granular control, explicit specialization
├─ LiF: Direct material quality weight (30%)
├─ BM: Material bonus calculated from average
└─ Both: Clamped to [0, 100] range

Recommendation: BlueMarble's model is more sophisticated
```

## Recommendations for BlueMarble

### 1. Adopt Skill Tier Milestone System

**Recommendation:** Implement discrete ability unlocks at skill levels

```
Proposed BlueMarble Tier System:

Geology Skill Example:

Level 0-24: Novice Geologist
├─ Can identify: Common rocks (granite, sandstone, limestone)
├─ Can extract: Surface samples only
├─ Analysis: Basic visual inspection
└─ Quality: 10-40 identification accuracy

Level 25-49: Journeyman Geologist
├─ Unlocks: Advanced rock identification (25+ types)
├─ Unlocks: Core sampling techniques
├─ Unlocks: Basic chemical testing
├─ Analysis: Microscopic examination
├─ Quality: 40-70 identification accuracy
└─ Specialization: Choose focus (Mineralogy/Petrology/Sedimentology)

Level 50-74: Expert Geologist
├─ Unlocks: Rare mineral identification
├─ Unlocks: Deep drilling techniques
├─ Unlocks: Advanced chemical analysis
├─ Analysis: Spectroscopic methods
├─ Quality: 70-90 identification accuracy
└─ Specialization: Master techniques in chosen focus

Level 75-99: Master Geologist
├─ Unlocks: All geological analysis techniques
├─ Unlocks: Predictive modeling capabilities
├─ Unlocks: Can teach/mentor others
├─ Analysis: Complete analytical suite
├─ Quality: 90-100 identification accuracy
└─ Specialization: Can author research papers, unlock unique abilities

Level 100: Legendary Geologist
├─ Unlocks: Unique discovery abilities
├─ Unlocks: Can identify unknown materials
├─ Research: Can propose new theories
└─ Legacy: Name attached to discoveries
```

**Benefits:**
- Clear progression goals for players
- Natural tutorial pacing (new abilities = new learning)
- Milestone celebration opportunities
- Justification for continued skill grinding

### 2. Consider Hard Skill Cap Option

**Recommendation:** Implement optional hard cap mode for servers

```
Proposed BlueMarble Hard Cap System:

Standard Mode (No Cap):
├─ Current system preserved
├─ Players can learn all skills
├─ Solo play viable
└─ Casual-friendly

Specialist Mode (Hard Cap):
├─ Total skill points: 800-1000
├─ Maximum per skill: 100
├─ Minimum skills maxable: 8-10
├─ Forces specialization
├─ Increases player interdependence
└─ Server option (like PvP toggle)

Benefits of Optional System:
├─ Serves both casual and hardcore audiences
├─ Creates distinct gameplay modes
├─ Allows server communities to choose
├─ Tests specialist economy without forcing it
└─ Can gather data on which mode players prefer
```

### 3. Implement Parent-Child Skill Bonuses

**Recommendation:** Add bonus system for related skills

```
Proposed BlueMarble Parent Skill System:

General Geology [Tier 1 Parent]
├─ Skill level: 0-100
├─ Provides: +0.1 per point to all geological children
├─ At 100: +10 effective points to all specializations
└─ Encourages: Broad foundation before specialization

    ├─── Mineralogy [Tier 2 Specialist]
    │    ├─ Base: 0-100
    │    ├─ Bonus: +10 from General Geology (if maxed)
    │    ├─ Effective: Up to 110
    │    └─ Enables: Mineral identification, extraction techniques
    │
    ├─── Petrology [Tier 2 Specialist]
    │    ├─ Base: 0-100
    │    ├─ Bonus: +10 from General Geology
    │    ├─ Effective: Up to 110
    │    └─ Enables: Rock formation analysis, quarrying
    │
    └─── Sedimentology [Tier 2 Specialist]
         ├─ Base: 0-100
         ├─ Bonus: +10 from General Geology
         ├─ Effective: Up to 110
         └─ Enables: Depositional analysis, soil science

Benefits:
├─ Rewards logical skill progression
├─ Creates optimal learning paths
├─ Doesn't punish specialization
├─ Maintains flexibility
└─ Adds strategic depth to character building
```

### 4. Add Failure Reward System

**Recommendation:** Grant partial experience for failed crafting attempts

```
Proposed "Learning from Mistakes" System:

Success vs Failure Gains:

Successful Craft:
├─ Base experience: 100%
├─ Quality bonus: 0-50% (based on output quality)
├─ First-time bonus: +25% (recipe mastery)
└─ Total: 100-175% experience

Failed Craft:
├─ Base experience: 25% of success
├─ Difficulty bonus: 0-15% (challenging recipes)
├─ Materials consumed: 50-100%
└─ Total: 25-40% experience

Critical Failure:
├─ Base experience: 10% of success
├─ Materials consumed: 100%
├─ Possible equipment damage
└─ Total: 10-15% experience

Benefits:
├─ Reduces frustration from failure streaks
├─ Encourages attempting challenging recipes
├─ Maintains progression even when RNG is poor
├─ Balances risk/reward for difficult crafts
└─ Player remains engaged during learning phase
```

### 5. Enhance Alignment/Focus System

**Recommendation:** Create geological specialization paths

```
Proposed BlueMarble Specialization Alignments:

Research Focus (-100 to +100):

Pure Researcher (+100):
├─ Benefits:
│   ├─ +20% research speed
│   ├─ +15% discovery chance
│   ├─ +10% analysis quality
│   └─ Access to theoretical research
├─ Penalties:
│   ├─ -15% extraction speed
│   ├─ -10% industrial efficiency
│   └─ Reduced practical skills
└─ Archetype: Academic, theorist, scientist

Balanced (0):
├─ Benefits:
│   ├─ No penalties to any path
│   ├─ Flexibility in all activities
│   └─ Can shift focus as needed
├─ Penalties:
│   ├─ No specialization bonuses
│   └─ No unique unlocks
└─ Archetype: Generalist, explorer

Pure Industrialist (-100):
├─ Benefits:
│   ├─ +20% extraction speed
│   ├─ +15% resource efficiency
│   ├─ +10% crafting speed
│   └─ Access to industrial equipment
├─ Penalties:
│   ├─ -15% research speed
│   ├─ -10% theoretical understanding
│   └─ Reduced analysis capabilities
└─ Archetype: Miner, producer, engineer

Alignment Shift:
├─ Changes based on actions performed
├─ Research activities: +1 per action
├─ Industrial activities: -1 per action
├─ Shift rate: ~100 actions to fully change
└─ Can deliberately shift alignment
```

### 6. Implement Mastery Recognition

**Recommendation:** Add social recognition for skill mastery

```
Proposed Master Recognition System:

Skill Milestones:

Level 25: Journeyman Certificate
├─ Awarded automatically
├─ Display title: "Journeyman [Skill]"
├─ Can teach basics to others
└─ Slight trading bonus

Level 50: Expert Certification
├─ Awarded automatically
├─ Display title: "Expert [Skill]"
├─ Can write guides/tutorials
├─ Market reputation bonus
└─ Can mentor apprentices

Level 75: Master Accreditation
├─ Awarded automatically
├─ Display title: "Master [Skill]"
├─ Can create custom specializations
├─ Significant trading reputation
├─ Can propose new research
└─ Guild leadership eligible

Level 90: Grand Master Status
├─ Awarded automatically
├─ Display title: "Grand Master [Skill]"
├─ Unique cosmetic rewards
├─ Legacy features (name on discoveries)
├─ Can propose game content
└─ Community leadership role

Level 100: Legendary Master
├─ Extremely rare achievement
├─ Display title: "Legendary [Skill] Master"
├─ Unique abilities/perks
├─ Permanent legacy features
├─ Hall of Fame recognition
└─ Can mentor cross-server
```

## Implementation Considerations

### Technical Requirements

```
Database Schema Additions:

-- Skill tier tracking
CREATE TABLE player_skill_tiers (
    player_id INT,
    skill_id INT,
    current_level INT,
    current_tier INT,
    tier_1_unlocked TIMESTAMP,
    tier_2_unlocked TIMESTAMP,
    tier_3_unlocked TIMESTAMP,
    tier_4_unlocked TIMESTAMP,
    specialization_chosen VARCHAR(50),
    PRIMARY KEY (player_id, skill_id)
);

-- Parent-child skill bonuses
CREATE TABLE skill_relationships (
    parent_skill_id INT,
    child_skill_id INT,
    bonus_multiplier DECIMAL(4,2),
    PRIMARY KEY (parent_skill_id, child_skill_id)
);

-- Alignment tracking
CREATE TABLE player_alignment (
    player_id INT PRIMARY KEY,
    research_industrial_alignment INT DEFAULT 0,
    alignment_history JSON,
    last_shift_timestamp TIMESTAMP
);

-- Failure experience tracking
CREATE TABLE crafting_attempts (
    attempt_id INT PRIMARY KEY AUTO_INCREMENT,
    player_id INT,
    recipe_id INT,
    success BOOLEAN,
    quality_output INT,
    experience_gained DECIMAL(10,2),
    timestamp TIMESTAMP
);
```

### Balance Considerations

**Key Tuning Variables:**

```
1. Skill Tier Thresholds
├─ Current: 25/50/75/90/100
├─ Alternative: 30/60/90/100 (Life is Feudal style)
├─ Consideration: More tiers = more milestones
└─ Recommendation: Start with 25/50/75, add more if needed

2. Hard Cap Amount
├─ Life is Feudal: 600 points
├─ BlueMarble suggestion: 800-1000 points
├─ Consideration: More skills in BlueMarble than LiF
└─ Recommendation: Make server-configurable

3. Parent Skill Bonus
├─ Life is Feudal: 0.1-0.15 per point
├─ BlueMarble suggestion: 0.1 per point
├─ Consideration: Higher = more incentive for generalist foundation
└─ Recommendation: 0.1 starting, tune based on playtesting

4. Failure Experience Rate
├─ Life is Feudal: 20-30% of success
├─ BlueMarble suggestion: 25-40% (more forgiving)
├─ Consideration: Higher = less grinding frustration
└─ Recommendation: 30% baseline, increase for very difficult recipes

5. Alignment Shift Speed
├─ New mechanic for BlueMarble
├─ Suggestion: 1 point per significant action
├─ Consideration: Too fast = no commitment, too slow = feels locked
└─ Recommendation: ~100-200 actions for full shift
```

### Phased Rollout Plan

```
Phase 1: Foundation (Month 1-2)
├─ Implement skill tier data structures
├─ Add tier unlock detection
├─ Create milestone notification system
└─ Basic UI for tier display

Phase 2: Parent-Child Skills (Month 2-3)
├─ Define skill relationships
├─ Implement bonus calculation
├─ Add to effective skill calculation
└─ UI display of bonuses

Phase 3: Failure Rewards (Month 3-4)
├─ Implement failure experience system
├─ Balance experience rates
├─ Track and display learning progress
└─ Analytics for tuning

Phase 4: Alignment System (Month 4-6)
├─ Design alignment mechanics
├─ Implement tracking and shift logic
├─ Create alignment-specific benefits
├─ UI for alignment display and management

Phase 5: Optional Hard Cap (Month 6-7)
├─ Implement server-configurable caps
├─ Create specialized game modes
├─ Balance for specialist economy
└─ Community testing and feedback

Phase 6: Mastery Recognition (Month 7-8)
├─ Title system implementation
├─ Cosmetic rewards
├─ Legacy features
└─ Hall of Fame/leaderboards
```

### Testing Requirements

```
1. Skill Tier Progression
├─ Test: Tier unlocks at correct levels
├─ Test: Abilities properly gated
├─ Test: UI displays correctly
└─ Test: Milestone notifications work

2. Parent-Child Bonuses
├─ Test: Bonuses calculate correctly
├─ Test: Multiple parents stack properly
├─ Test: UI shows effective skill levels
└─ Test: Balance feels appropriate

3. Failure Experience
├─ Test: Experience granted on failure
├─ Test: Rates feel fair to players
├─ Test: Materials consumed appropriately
└─ Test: Analytics tracking works

4. Alignment System
├─ Test: Alignment shifts as expected
├─ Test: Benefits/penalties apply correctly
├─ Test: Can shift alignment deliberately
└─ Test: UI feedback is clear

5. Hard Cap Mode
├─ Test: Cap enforces correctly
├─ Test: UI shows remaining points
├─ Test: Server configuration works
└─ Test: Economy functions in specialist mode
```

## Conclusion

Life is Feudal's material and quality system provides valuable lessons for BlueMarble's geological crafting systems. The key insights are:

**Primary Takeaways:**

1. **Hard Caps Create Interdependence:** The 600-point skill cap successfully forces specialization and creates a thriving player economy based on skill diversity.

2. **Tier Unlocks Provide Structure:** Discrete ability unlocks at 30/60/90 create clear progression milestones that give players concrete goals.

3. **Parent-Child Skills Add Depth:** Bonus systems for related skills encourage logical progression paths without forcing them.

4. **Failure Rewards Reduce Frustration:** Granting partial experience for failed attempts maintains engagement during the learning curve.

5. **Quality Scaling Creates Economy:** Continuous quality variation (0-100) generates natural market tiers and values specialization.

6. **Alignment Systems Foster Identity:** The crafting vs combat alignment helps players develop distinct character identities and roles.

**Application to BlueMarble:**

BlueMarble can adopt Life is Feudal's successful mechanics while adapting them to the geological context:

- Implement skill tier milestones for structured progression
- Add parent-child bonuses for geological skill trees
- Create research vs industrial alignment system
- Implement failure experience for gentler learning curves
- Consider optional hard cap modes for different server types
- Add mastery recognition systems for social status

These implementations should be phased gradually, with extensive playtesting and community feedback at each stage. The goal is to create meaningful specialization and player interdependence while maintaining BlueMarble's scientific accuracy and educational mission.

**Next Steps:**

1. Present recommendations to design team
2. Prototype skill tier system with one skill tree
3. Playtest with community feedback
4. Iterate and refine based on data
5. Roll out successful mechanics gradually
6. Monitor economic impact and player engagement

## References

- Life is Feudal Official Wiki: https://lifeisfeudal.fandom.com/wiki/Life_Is_Feudal_Wiki
- Official Website: https://lifeisfeudal.com/
- BlueMarble Crafting Quality Model: `/docs/gameplay/mechanics/crafting-quality-model.md`
- BlueMarble Skill Knowledge System Research: `/research/game-design/skill-knowledge-system-research.md`
- BlueMarble Skill Caps Research: `/research/game-design/skill-caps-and-decay-research.md`

---

**Document Version History:**
- **1.0** (2025-01-20): Initial research document completed
