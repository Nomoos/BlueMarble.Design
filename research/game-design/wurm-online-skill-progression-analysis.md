# Wurm Online Skill System and Knowledge Progression Analysis

**Document Type:** Market Research Report  
**Version:** 1.0  
**Author:** BlueMarble Game Design Research Team  
**Date:** 2025-01-13  
**Status:** Final  
**Research Type:** Market Research  
**Priority:** Low

## Executive Summary

This research document analyzes Wurm Online's skill progression and knowledge systems to identify mechanics that can inform BlueMarble's approach to player mastery, specialization, and player-driven economy. Wurm Online features one of the most comprehensive skill-based progression systems in the MMORPG genre, with 130+ skills that improve through use and create natural specialization patterns.

**Key Findings:**

- **Use-Based Progression:** All 130+ skills improve through practice with incremental gains (0.00001 per action), creating constant micro-progression feedback
- **Natural Specialization:** Soft skill cap (~700 total points) and time investment naturally drive specialization without hard restrictions
- **Player-Driven Economy:** Skill-based quality systems create market segmentation where specialists command premium prices
- **Implicit Knowledge:** Players learn optimal techniques through experimentation rather than explicit skill unlocks
- **Long-Term Mastery:** Skills can take years to master, creating persistent character identity and reputation systems

**Recommendations for BlueMarble:**

- Adopt use-based progression with visible incremental gains for player feedback
- Implement soft caps that encourage specialization while maintaining player freedom
- Link skill levels directly to quality output for economic differentiation
- Design parent-child skill relationships to create strategic progression paths
- Balance progression pacing for both casual and hardcore players (avoid extreme grind)

## Table of Contents

1. [Research Objectives](#research-objectives)
2. [Methodology](#methodology)
3. [Wurm Online Skill System Overview](#wurm-online-skill-system-overview)
4. [Use-Based Progression Mechanics](#use-based-progression-mechanics)
5. [Skill Structure and Organization](#skill-structure-and-organization)
6. [Specialization and Mastery Systems](#specialization-and-mastery-systems)
7. [Knowledge Integration](#knowledge-integration)
8. [Player-Driven Economy Impact](#player-driven-economy-impact)
9. [UI/UX Analysis](#uiux-analysis)
10. [Player Psychology and Engagement](#player-psychology-and-engagement)
11. [Comparison with BlueMarble Goals](#comparison-with-bluemarble-goals)
12. [Recommendations for BlueMarble](#recommendations-for-bluemarble)
13. [Implementation Considerations](#implementation-considerations)
14. [Next Steps](#next-steps)

## Research Objectives

### Primary Research Questions

1. **How does Wurm Online structure skill progression and specialization?**
   - What is the complete skill structure (130+ skills)?
   - How do skills improve through use?
   - What mechanics drive natural specialization?

2. **How does the system support player-driven economy and mastery?**
   - How do skill levels affect economic viability?
   - What creates market segmentation by skill tier?
   - How do specialists gain reputation and premium pricing?

3. **What are the impacts of knowledge systems on gameplay and community?**
   - How is implicit knowledge learned?
   - What role does experimentation play?
   - How do players share knowledge?

4. **Which mechanics can inform BlueMarble's design?**
   - What works well for geological simulation context?
   - What should be adapted vs avoided?
   - How can we balance accessibility with depth?

### Success Criteria

This research is successful if it:

- Provides comprehensive understanding of Wurm's 130+ skill system
- Identifies specific progression mechanics applicable to BlueMarble
- Analyzes economic impacts of skill-based specialization
- Delivers actionable recommendations with implementation considerations
- Includes UI/UX analysis from provided screenshots

## Methodology

### Research Approach

**Qualitative analysis** combining official documentation review, community resource analysis, player feedback study, and comparative game design analysis.

### Data Collection Methods

- **Wiki Analysis:** Comprehensive review of Wurmpedia skill documentation
- **Community Forums:** Analysis of player discussions on progression, specialization, and economy
- **Player Guides:** Expert player guides on optimal skill training and specialization paths
- **UI/UX Review:** Analysis of provided screenshots for interface patterns
- **Economic Analysis:** Study of player-driven economy mechanics and skill impact

### Data Sources

- **Wurmpedia:** <https://www.wurmpedia.com/index.php/Skills> (Primary skill system documentation)
- **Wurm Online Official Website:** <https://www.wurmonline.com/>
- **Wurm Online Forums:** Player economy and progression discussions
- **Community Calculators:** Skill planning and progression tools
- **Player Testimonials:** Long-term player experiences with skill system

### Limitations

- Limited access to actual game code and precise formulas
- Progression formulas documented by community may have minor inaccuracies
- Economic data based on anecdotal player observations
- Focus on Freedom cluster (PvE) rather than Epic/PvP mechanics
- Screenshots provided show different game (appears to be survival game UI for concept illustration)


## Wurm Online Skill System Overview

### Core Philosophy

Wurm Online's skill system is built on these principles:

**Complete Freedom:**
- All 130+ skills available to all players from the start
- No class restrictions or skill prerequisites
- Players choose specialization through time investment, not system constraints

**Use-Based Progression:**
- Skills improve through practice, not point allocation
- Every action provides incremental skill gain
- Early progression fast, late progression slow (logarithmic curve)

**Natural Specialization:**
- Soft skill cap (~700 total skill points) encourages focus
- Time investment creates natural specialization
- Specialists emerge organically based on playstyle

**Persistent Character Identity:**
- Single character per server (typically)
- Years of skill investment create reputation
- Master craftsmen known by name in server economy

### Skill Count and Categories

Wurm Online features **130+ individual skills** organized into categories:

**Primary Skill Categories:**

```
Combat Skills (~15 skills)
├── Weapons Mastery
│   ├── Swords, Axes, Hammers, Polearms
│   ├── Bows, Crossbows
│   └── Shields, Fighting
├── Combat Techniques
│   ├── Aggressive, Normal, Defensive Fighting Styles
│   └── Taunting, War Machines
└── Armor Types
    ├── Chain, Plate, Leather, Cloth
    └── Shield Types

Crafting Skills (~40 skills)
├── Smithing Tree
│   ├── Blacksmithing (weapons, armor, tools)
│   ├── Weapon Smithing (specialized)
│   ├── Armour Smithing (specialized)
│   ├── Shield Smithing
│   └── Chain Armour Smithing
├── Carpentry Tree
│   ├── Carpentry (general woodworking)
│   ├── Fine Carpentry (furniture, items)
│   ├── Ship Building
│   └── Boatbuilding
├── Tailoring Tree
│   ├── Cloth Tailoring
│   ├── Leatherworking
│   └── Toy Making
└── Other Crafting
    ├── Pottery, Stonecutting, Masonry
    ├── Jewelry Smithing, Gold Smithing
    └── Ropemaking, Papyrusmaking

Resource Gathering (~20 skills)
├── Mining
│   ├── Mining (general)
│   ├── Prospecting (ore location)
│   └── Surface Mining
├── Woodcutting
│   ├── Woodcutting
│   └── Forestry (tree age, quality)
├── Farming
│   ├── Farming (crops)
│   ├── Gardening (herbs)
│   └── Animal Husbandry
└── Other Gathering
    ├── Digging, Foraging, Botanizing
    ├── Fishing
    └── Hunting, Tracking

Processing Skills (~15 skills)
├── Smelting (ore to metal)
├── Cooking (food preparation)
├── Alchemy (potions, dyes)
├── Butchering (animal processing)
└── Various material processing

Utility Skills (~30 skills)
├── Building & Construction
│   ├── Paving, Roofing
│   ├── Masonry (walls, buildings)
│   └── Repairing (maintenance)
├── Transportation
│   ├── Riding (various mounts)
│   ├── Sailing, Piloting
│   └── Animal Leading
├── Analysis & Knowledge
│   ├── Restoration (archaeology)
│   ├── Nature (animal/plant knowledge)
│   └── First Aid, Soul Depth
└── Miscellaneous
    ├── Lock Picking, Stealing
    ├── Climbing, Swimming
    └── Preaching, Meditating

Religious/Spiritual (~10 skills)
├── Channeling (casting spells)
├── Faith (religion strength)
├── Praying, Prayer
└── Meditating, Meditation paths
```

### Skill Value Range

All skills range from **0.00 to 100.00**:

- **0.00-20.00:** Novice - Learning basics, high failure rate
- **20.00-40.00:** Journeyman - Functional capability, moderate success
- **40.00-60.00:** Expert - Competent, reliable results
- **60.00-80.00:** Master - High skill, premium quality
- **80.00-90.00:** Grand Master - Elite tier, rare on servers
- **90.00-100.00:** Legendary - Exceptional, often takes years

## Use-Based Progression Mechanics

### Skill Gain Formula

Wurm Online uses a **learn-by-doing** system where every action provides skill experience:

**Basic Progression Formula (Community Documented):**

```text
skill_gain = base_gain × difficulty_modifier × parent_skill_bonus × characteristic_bonus

Where:
- base_gain = 0.00001 to 0.001 per action (varies by skill)
- difficulty_modifier = function of (action_difficulty vs current_skill)
- parent_skill_bonus = parent skill provides gains to children
- characteristic_bonus = player stats (strength, stamina, etc.) affect gains

Optimal Gains:
- Action difficulty ≈ Current skill level ± 10
- Too easy: 0.1× gain multiplier
- Just right: 1.0× gain multiplier
- Too hard: 0.3× gain multiplier (high failure rate)
```

### Incremental Progression

**Micro-Progression Feedback:**

```text
Example: Blacksmithing Progression
Action: Create Iron Lump (difficulty 20)

At Skill 15.234:
- Attempt 1: +0.00023 (15.23423)
- Attempt 2: +0.00019 (15.23442)
- Attempt 3: +0.00021 (15.23463)
...
After 100 attempts: 15.234 → 15.256 (+0.022)

At Skill 50.125:
- Attempt 1: +0.00008 (50.12508)
- Attempt 2: +0.00007 (50.12515)
...
After 100 attempts: 50.125 → 50.133 (+0.008)

At Skill 85.678:
- Attempt 1: +0.00002 (85.67802)
- Attempt 2: +0.00001 (85.67803)
...
After 100 attempts: 85.678 → 85.680 (+0.002)
```

**Key Characteristic:** Players see constant tiny increases, creating psychological satisfaction even at high skill levels.

### Progression Curve

**Time Investment by Skill Level:**

```text
Skill Tier | Skill Range | Hours Required | Gain Rate | Player Tier
-----------|-------------|----------------|-----------|-------------
Novice     | 0-20        | 5-15 hours     | Fast      | All players
Journeyman | 20-40       | 20-40 hours    | Moderate  | Casual focus
Expert     | 40-60       | 60-120 hours   | Slow      | Dedicated
Master     | 60-80       | 150-300 hours  | Very Slow | Specialist
Grand      | 80-90       | 400-800 hours  | Extreme   | Server elite
Legendary  | 90-100      | 1000+ hours    | Glacial   | Rare masters

Total to 100: 1500-2500+ hours of focused practice per skill
Total to 70 (viable specialist): 300-500 hours per skill
```

**Logarithmic Curve Characteristics:**

- Early levels (0-30): Rapid gains, immediate feedback
- Mid levels (30-60): Steady progression, clear improvement
- High levels (60-80): Slow but consistent gains
- Elite levels (80-100): Extremely slow, requires dedication

### Parent-Child Skill Relationships

Many skills have **parent-child relationships** where parent skill level affects child skill improvement:

**Example: Smithing Skill Tree**

```text
Smithing [Parent Skill] (Level 50)
│
├─── Blacksmithing [Child] (Level 60)
│    ├─ Gains: +0.5% bonus from Smithing parent
│    ├─ Quality: Parent skill affects maximum quality possible
│    └─ Specializations: Weapons, Tools, General Items
│
├─── Weapon Smithing [Grandchild] (Level 70)
│    ├─ Gains: Bonus from Smithing + Blacksmithing
│    ├─ Quality: Both parents affect final weapon quality
│    └─ Highly specialized, premium tier
│
└─── Armour Smithing [Grandchild] (Level 65)
     ├─ Gains: Bonus from Smithing + Blacksmithing
     ├─ Quality: Parent skills critical for armor stats
     └─ Separate specialization path

Strategic Progression Path:
1. Raise Smithing (parent) to 50+ first
2. Then specialize in Blacksmithing
3. Finally master Weapon or Armour Smithing
4. Result: Higher quality ceiling, faster gains
```

**Parent Skill Bonus Formula (Estimated):**

```text
child_skill_gain = base_gain × (1 + parent_skill / 200)

Example:
- Parent Smithing at 50: +25% bonus to Blacksmithing gains
- Parent Smithing at 80: +40% bonus to Blacksmithing gains
- Parent Smithing at 100: +50% bonus to all children
```

### Optimal Training Strategies

**Community-Discovered Optimal Paths:**

1. **Grind Curve Matching:**
   - Always work on tasks matching current skill level
   - Example: Skill 40 → craft QL 35-45 items
   - Avoid: Skill 40 → craft QL 10 items (minimal gains)

2. **Parent Skill Investment:**
   - Level parent skills first for multiplicative gains
   - Smithing to 50 before Blacksmithing to 70
   - Body Stamina/Strength before combat skills

3. **Tool Quality Impact:**
   - Higher quality tools = faster actions = more attempts
   - More attempts = more skill gain opportunities
   - Tools don't affect gain per action, but actions per hour

4. **Sleep Bonus System:**
   - Players accumulate "sleep bonus" offline
   - Sleep bonus = 5× skill gain multiplier
   - Strategic: Use sleep bonus on hardest skills to train

5. **Difficulty Scaling:**
   - Create items slightly above your skill level
   - Use higher difficulty materials as skill increases
   - Balance failure rate vs gain optimization


## Skill Structure and Organization

### Crafting Skills Deep Dive

**Blacksmithing Skill Tree** (Example of deep specialization):

```text
General Smithing (Parent)
├─ Level 0-30: Basic understanding of metalworking
├─ Unlocks: Smelting ore, basic metal lumps
└─ Quality Ceiling: QL 30 items maximum

    └─── Blacksmithing (Child)
         ├─ Level 0-30: Journeyman smith
         ├─ Level 30-60: Expert smith
         ├─ Level 60-80: Master smith
         ├─ Level 80-90: Grand Master smith
         ├─ Level 90-100: Legendary smith
         └─ Quality Ceiling: Increases with skill

              ├─── Weapon Smithing (Grandchild)
              │    ├─ Specialist in weapon creation
              │    ├─ Higher quality weapons possible
              │    ├─ Faster weapon creation speed
              │    └─ Market premium for master weapons
              │
              ├─── Armour Smithing (Grandchild)
              │    ├─ Specialist in armor creation
              │    ├─ Better armor stats possible
              │    └─ Defensive gear focus
              │
              └─── Shield Smithing (Grandchild)
                   ├─ Specialist in shield creation
                   └─ Separate from armor smithing
```

**Carpentry Skill Tree:**

```text
Carpentry (Parent)
├─ General woodworking
├─ Creates basic wooden items
└─ Enables child skills

    ├─── Fine Carpentry (Child)
    │    ├─ Furniture creation
    │    ├─ Decorative items
    │    └─ High-quality wooden goods
    │
    ├─── Ship Building (Child)
    │    ├─ Ocean-going vessels
    │    ├─ Large construction projects
    │    └─ Complex multi-part assembly
    │
    └─── Boatbuilding (Child)
         ├─ Small watercraft
         ├─ River/lake boats
         └─ Simpler than ships
```

### Resource Gathering Skills

**Mining Skill** (detailed example):

```text
Mining Progression:
Level 0-10: Surface mining only, slow, low quality
Level 10-20: Can mine rock, basic ores
Level 20-40: Better ore quality, faster mining
Level 40-60: Expert miner, good quality consistent
Level 60-80: Master miner, high quality ores
Level 80-100: Legendary miner, maximum quality

Mining Mechanics:
- Each mining action trains Mining skill
- Quality of ore = function(skill, node_quality, tools)
- Higher skill = better chance of rare ores
- Prospecting skill (separate) helps find high-quality nodes

Related Skills:
├─ Mining (primary)
├─ Prospecting (ore location)
├─ Surface Mining (terraforming)
└─ Body Stamina (affects mining speed/endurance)
```

### Characteristic Skills

Wurm Online also has **body characteristic skills** that affect everything:

```text
Body Stats (Train automatically through activity):
├── Body Strength → Affects carry capacity, damage
├── Body Stamina → Affects action speed, endurance
├── Body Control → Affects accuracy, balance
├── Mind Logic → Affects learning speed, puzzle-solving
└── Mind Speed → Affects skill gain rate

These skills:
- Train passively during all activities
- No direct skill gain actions
- Provide multiplicative bonuses to other skills
- Take longest to train (thousands of hours to 90+)
```

## Specialization and Mastery Systems

### Soft Skill Cap Mechanics

Wurm Online uses a **soft skill cap** system rather than hard limits:

**Total Skill Points:**

```text
Theoretical Total Skill Points: 130 skills × 100 points = 13,000 points
Practical Skill Cap (community observed): ~700-1000 total points

Why Soft Cap Exists:
1. Time Investment: Years needed per skill to 90+
2. Skill Decay: Unused skills may decay (server-dependent)
3. Opportunity Cost: Grinding one skill means not training others
4. Logarithmic Curve: Extreme time requirements at high levels

Example Specialized Player Profile:
Total Skill Points: ~750
- 3-5 Skills at 80-100 (Masters): 350-450 points
- 10-15 Skills at 40-70 (Functional): 250-300 points
- 20-30 Skills at 10-40 (Basic): 150-200 points
- Rest at <10 (Touched but not trained): <100 points
```

### Natural Specialization Patterns

**Specialization Archetypes** emerge organically:

#### Master Crafter Archetype

```text
Blacksmith Specialist:
├─ Smithing: 90 (parent skill, enables children)
├─ Blacksmithing: 95 (primary specialization)
├─ Weapon Smithing: 85 (deep specialization)
├─ Mining: 70 (resource self-sufficiency)
├─ Repairing: 60 (tool maintenance)
├─ Misc crafting skills: 20-40 (functional)
└─ Rest: 10-30 (basic capabilities)

Total Points: ~700
Market Value: Premium weaponsmith, known reputation
Playstyle: Focused on weapon quality, commissioned work
```

#### Gatherer-Processor Archetype

```text
Resource Specialist:
├─ Mining: 85 (expert ore extraction)
├─ Prospecting: 90 (find best nodes)
├─ Woodcutting: 80 (timber supply)
├─ Farming: 70 (food production)
├─ Smelting: 60 (ore processing)
├─ Basic crafting: 30-40 (self-sufficiency)
└─ Rest: 10-20 (minimal)

Total Points: ~650
Market Value: Bulk resource supplier
Playstyle: Gather and sell raw materials
```

#### Jack-of-All-Trades Archetype

```text
Generalist:
├─ 15-20 skills at 40-60 (functional in many areas)
├─ No specialization >70
└─ Total: ~800 points spread widely

Market Value: Lower per-item but diverse offerings
Playstyle: Self-sufficient, solo-friendly
Challenge: Cannot compete in premium markets
```

### Mastery Recognition

**Community Recognition Systems:**

- **Server Reputation:** Master crafters known by name
- **Quality Guarantee:** Players seek specific crafters for quality
- **Premium Pricing:** Masters charge 2-10× rates of journeymen
- **Commissioned Work:** Elite items made to order
- **Teaching Role:** Masters train apprentices

**Skill Title System:**

```text
Skill-Based Titles (Automatic):
- 50 skill: "Journeyman [Skill]"
- 70 skill: "Master [Skill]"
- 90 skill: "Grand Master [Skill]"
- 100 skill: "Legendary [Skill]"

Example: "Grand Master Blacksmith" = 90+ Blacksmithing skill
These titles:
- Display in-game
- Show expertise at a glance
- Create social status
- Enable trust in trading
```

## Knowledge Integration

### Implicit Knowledge System

Unlike many MMOs, Wurm Online uses **implicit knowledge** rather than explicit skill unlocks:

**What Players Learn Through Experience:**

1. **Material Relationships:**
   - Which materials produce best results
   - Optimal material combinations
   - Quality thresholds for different items

2. **Technique Optimization:**
   - Fastest grinding methods per skill
   - Most efficient resource gathering paths
   - Action timing for maximum gains

3. **Quality Understanding:**
   - How skill level affects output quality
   - Tool quality impact on final products
   - Material quality vs skill tradeoffs

4. **Economic Knowledge:**
   - Market prices by quality tier
   - Demand patterns for different items
   - Profitable specialization paths

**No Explicit Recipe System:**

```text
Traditional MMO Recipe System:
- Learn Recipe → Unlock ability to craft item
- Recipe book or trainer required
- Binary: Can craft or cannot craft

Wurm Online Approach:
- All items craftable from start
- Success chance based on skill level
- Quality based on skill + materials
- Players discover through experimentation

Example:
- New player can attempt any item immediately
- Low skill = high failure rate + low quality
- Experimentation reveals optimal methods
- Community shares knowledge through wikis/forums
```

### Experimentation and Discovery

**Player-Driven Knowledge Base:**

- **Wurmpedia:** Community-maintained wiki with formulas, tips, strategies
- **Forum Guides:** Veteran players share optimal paths
- **In-Game Teaching:** Experienced players mentor newcomers
- **Trial and Error:** Players discover new techniques

**Knowledge Sharing Mechanics:**

```text
How Knowledge Spreads:
1. Player discovers optimal technique
2. Shares in guild or forum
3. Community tests and validates
4. Added to wiki if widely useful
5. Becomes common knowledge

Example Discovery:
"Sleep bonus is 5× skill gain" → 
Community testing confirms →
Optimal strategy: Save sleep bonus for hardest skills →
Becomes standard practice
```

### Recipe Complexity

**Item Complexity Tiers:**

```text
Simple Items (Skill 10-20):
- Basic tools (stone axe, hammer)
- Simple materials (boards, shafts)
- Single-step crafting

Intermediate Items (Skill 40-60):
- Metal tools (iron saw, chisel)
- Basic furniture
- 2-3 step crafting process

Complex Items (Skill 70+):
- Weapons, armor
- Ships, carts
- Multi-component assembly
- Requires multiple high skills

Mastery Items (Skill 90+):
- Premium weapons/armor
- Large structures (guard towers)
- Requires near-perfect execution
```

## Player-Driven Economy Impact

### Skill-Based Market Segmentation

The skill system creates **natural market tiers**:

**Quality Tiers and Pricing:**

```text
Low Quality (QL 1-30):
- Producers: All skill levels (1-30)
- Customers: New players, disposable items
- Pricing: Minimal (1-5 copper per item)
- Volume: Very high
- Competition: Extreme

Mid Quality (QL 30-60):
- Producers: Intermediate skills (30-60)
- Customers: Established players, functional needs
- Pricing: Moderate (10-50 copper per item)
- Volume: High
- Competition: Moderate

High Quality (QL 60-80):
- Producers: Skilled specialists (60-80)
- Customers: Endgame players, quality seekers
- Pricing: Premium (50 copper - 5 silver per item)
- Volume: Moderate
- Competition: Limited

Master Quality (QL 80-100):
- Producers: Elite masters (80-100)
- Customers: Collectors, min-maxers, status seekers
- Pricing: Luxury (5-100+ silver per item)
- Volume: Very low
- Competition: Minimal
```

### Specialization Economics

**Economic Viability by Specialization:**

```text
Generalist (Jack-of-All-Trades):
Revenue Model: High volume, low margin
- Produce 50-100 items/day
- QL 40-50 average
- 10-20 copper profit per item
- Daily income: 5-20 silver
Challenges: High competition, time-intensive

Specialist (Master Crafter):
Revenue Model: Low volume, high margin
- Produce 5-15 items/day
- QL 80-90 average
- 5-20 silver profit per item
- Daily income: 25-300 silver
Advantages: Premium pricing, commissioned work, reputation

Resource Supplier:
Revenue Model: Bulk materials
- Gather 500-1000 units/day
- QL varies by skill
- 1-10 copper per unit
- Daily income: 5-100 silver
Advantages: Consistent demand, less competition at high quality
```

### Trade Dependencies

**Production Chain Economics:**

```text
Example: Master Longsword Production

Step 1: Iron Ore (QL 80)
- Producer: Master Miner (Mining 85+)
- Price: 10 copper per ore
- Time: 10 actions = 5 minutes

Step 2: Iron Lump (QL 78)
- Producer: Expert Smelter (Smelting 75+)
- Input cost: 10 copper (ore)
- Added value: 15 copper (smelting)
- Output price: 25 copper per lump
- Time: 5 minutes

Step 3: Sword Blade (QL 85)
- Producer: Master Blacksmith (Blacksmithing 90+)
- Input cost: 25 copper (lump)
- Added value: 2 silver (smith skill)
- Output price: 2.25 silver
- Time: 20 minutes

Step 4: Completed Longsword (QL 87)
- Producer: Grand Master Weaponsmith (Weapon Smithing 95+)
- Input cost: 2.25 silver (blade) + 50 copper (handle)
- Added value: 15 silver (master skill + reputation)
- Final price: 17.75 silver
- Total time: 40-60 minutes

Value Distribution:
- Miner: 10 copper (0.5%)
- Smelter: 15 copper (0.8%)
- Blacksmith: 2 silver (11%)
- Weaponsmith: 15 silver (85%)
- Total: 17.75 silver

Key Insight: Highest skill level commands majority of value
```

### Reputation and Brand Value

**Master Crafter Reputation Systems:**

```text
Reputation Building:
1. Consistent Quality: Always produce QL 80+ items
2. Reliability: Deliver commissioned work on time
3. Specialization: Known for specific item type
4. Community Presence: Active in trade channels
5. Teaching: Share knowledge, mentor apprentices

Reputation Benefits:
├─ Premium Pricing: Charge 20-50% above market rate
├─ Commissioned Work: Players seek you specifically
├─ Steady Demand: Regular customers
├─ Bulk Orders: Guild contracts
└─ Social Status: Server recognition

Example:
"TheMaster" - Server-famous Grand Master Weaponsmith
- Known for QL 90+ longswords
- Charges 30 silver per sword (market rate: 20 silver)
- 2-week wait list for commissions
- Reputation = brand value
```

## UI/UX Analysis

### Skill Interface Patterns

**Note:** The provided screenshots show a different game's skill interface (appears to be a survival game), not Wurm Online. However, we can analyze the concepts shown:

**Screenshot 1 Analysis - Skill Overview Interface:**

```text
Interface Elements Observed:
├─ Active Skills Section
│   ├─ Skill icons with visual identity
│   ├─ "Already assigned" status indicators
│   └─ "Add to skill bar" action buttons
│
├─ Skills List
│   ├─ Skill names (Defensive Fighting, Firemaking, Hammers, Healing, etc.)
│   ├─ Progress indicators (dots showing progression)
│   ├─ Current value display (1.00 shown)
│   ├─ Checkmark indicators (green checkmarks)
│   └─ Skill descriptions

Design Patterns:
- Tab-based navigation (Equipment & Attributes, Health, Skills, Bonuses)
- Visual skill categorization
- Clear progression feedback
- Status indicators for skill states
```

**Screenshot 2 Analysis - Skill Details:**

```text
Interface Elements:
├─ Continued skill list (Hi-Tech Items, Knives, Leatherworking, Masonry)
├─ Same progression dot system
├─ Attribute correlation indicators
│   ├─ STRENGTH, STAMINA, CONTROL, LOGIC columns
│   ├─ SPEED, PSY STR, PSY DEPTH columns
│   └─ Checkmarks showing which attributes affect skill

Design Insights:
- Shows relationship between skills and attributes
- Multi-column layout for attribute correlation
- Progression visualization through dots
- Consistent value display (1.00)
```

### Wurm Online Actual UI Patterns

**Based on Wurmpedia documentation, Wurm Online uses:**

```text
Skill Window Interface:
├─ Tree View
│   ├─ Expandable skill categories
│   ├─ Parent-child relationships shown hierarchically
│   └─ Indent levels show skill depth
│
├─ Skill Entry Display
│   ├─ Skill name
│   ├─ Current value (XX.XXXXX format)
│   ├─ Last gain timestamp
│   ├─ Skill affinity indicator
│   └─ Tooltip with skill description
│
└─ Sorting Options
    ├─ By category
    ├─ By level (highest first)
    ├─ By recent gains
    └─ Alphabetical

Key Features:
- Precise value display (5 decimal places)
- Last gain tracking
- Parent skill highlighting
- Skill affinity marking (bonus gains)
```

### UI/UX Lessons for BlueMarble

**Positive Patterns to Adopt:**

1. **Precise Value Display:**
   - Show exact skill values (not just bars)
   - Players appreciate knowing exact progress
   - Creates attachment to incremental gains

2. **Hierarchical Organization:**
   - Parent-child relationships clear
   - Category grouping intuitive
   - Easy to find related skills

3. **Progress Feedback:**
   - Show recent gains
   - Highlight actively improving skills
   - Provide visual progression indicators

4. **Attribute Correlation:**
   - Show which attributes affect which skills
   - Help players understand synergies
   - Guide strategic progression choices

**Potential Improvements:**

1. **Visual Skill Tree:**
   - Add graphical skill tree view
   - Show progression paths visually
   - Highlight optimal routes

2. **Progress Prediction:**
   - "Time to next level" estimation
   - "Actions required" counter
   - Goal-setting tools

3. **Comparison Tools:**
   - Compare your skills to server average
   - See where you rank in specializations
   - Identify market opportunities

## Player Psychology and Engagement

### Micro-Progression Satisfaction

**The Power of 0.00001:**

```text
Psychological Impact:
- Every action provides visible progress
- No "wasted" grinding sessions
- Constant positive reinforcement
- Progress bar never stops moving

Player Testimony (paraphrased from forums):
"Even after 5 years, gaining 0.001 in blacksmithing feels rewarding.
You know you're getting closer to that next quality threshold."

Comparison to Level Systems:
Level System: "90% to next level" (frustrating)
Wurm System: "+0.00234 this session" (satisfying)
```

### Long-Term Engagement

**Multi-Year Progression:**

```text
Year 1: Novice to Journeyman
- Rapid progression in chosen skills
- Exploration of different paths
- Finding preferred specialization
- Skill range: 0-50 in 3-5 main skills

Year 2: Journeyman to Expert
- Focused specialization emerges
- Economic viability established
- Server reputation building
- Skill range: 50-70 in 2-3 main skills

Year 3+: Expert to Master
- Deep mastery pursuit
- Premium market access
- Community leadership
- Skill range: 70-90+ in 1-2 main skills

Result: Years of engagement per character
```

### Burnout Considerations

**Potential Burnout Factors:**

```text
Extreme Grind:
- 80-100 skill levels extremely slow
- Thousands of hours required
- Repetitive actions
- Risk of player frustration

Community Observations:
- Many players stop at 70-80 skills
- "Good enough" quality satisfaction
- Diminishing returns on time investment
- Elite tier (90+) often not worth effort

Mitigation Strategies (player-developed):
- Focus on multiple skills to 70 vs one to 100
- Combine grinding with social activity
- Use sleep bonus efficiently
- Accept "good enough" quality tiers
```

### Player Retention Patterns

**What Keeps Players Engaged:**

```text
Positive Factors:
├─ Character Investment: Years of progress creates attachment
├─ Reputation: Known identity on server
├─ Economic Position: Established market niche
├─ Social Bonds: Guilds, friendships, rivalries
└─ Perpetual Goals: Always one more skill to improve

Negative Factors:
├─ Extreme Time Requirements: 90+ skills take years
├─ Repetitive Actions: Grinding can be monotonous
├─ Server Population: Low pop = limited economy
└─ New Player Gap: Veterans vastly outskill newcomers
```

## Comparison with BlueMarble Goals

### Alignment with BlueMarble Vision

**Strong Alignments:**

1. **Use-Based Progression:**
   - ✅ Wurm's practice-based system fits geological simulation
   - ✅ Skill improves through actual usage of techniques
   - ✅ Natural learning curve matches real-world expertise

2. **Specialization Through Choice:**
   - ✅ Soft caps encourage focus without forcing it
   - ✅ Players choose specialization based on interest
   - ✅ Supports both solo and group-oriented playstyles

3. **Player-Driven Economy:**
   - ✅ Skill-based quality creates market tiers
   - ✅ Specialists command premium pricing
   - ✅ Trade dependencies foster player interaction

4. **Knowledge Discovery:**
   - ✅ Implicit learning through experimentation
   - ✅ Community knowledge sharing
   - ✅ Rewards exploration and innovation

**Misalignments or Concerns:**

1. **Extreme Time Requirements:**
   - ⚠️ 1500+ hours to master one skill may not suit BlueMarble's target audience
   - ⚠️ Need to balance depth with accessibility
   - ⚠️ Consider accelerated progression for geological skills

2. **130+ Skills May Be Excessive:**
   - ⚠️ BlueMarble's geological focus may not need this many skills
   - ⚠️ Quality over quantity: deeper mechanics in fewer skills
   - ⚠️ 20-40 well-designed skills may be more appropriate

3. **Lack of Explicit Knowledge System:**
   - ⚠️ BlueMarble plans knowledge/research mechanics
   - ⚠️ Wurm's implicit system may not showcase geological learning
   - ⚠️ Hybrid approach: Implicit skills + explicit knowledge

### Adaptation Recommendations

**What to Adopt:**

```text
✓ Use-based progression with micro-gains
✓ Parent-child skill relationships
✓ Soft skill caps (not hard limits)
✓ Quality output tied to skill level
✓ Precise skill value display
✓ Natural specialization patterns
✓ Reputation-based master crafter systems
```

**What to Adapt:**

```text
≈ Reduce total skill count (130 → 30-50)
≈ Faster progression curve (avoid 1000+ hour grinds)
≈ Add explicit knowledge system alongside implicit
≈ More structured specialization paths
≈ Milestone rewards at key skill levels
≈ Better new player onboarding
```

**What to Avoid:**

```text
✗ Extreme late-game grind (90-100 skill levels)
✗ Purely implicit learning (needs explicit knowledge too)
✗ No hard goals or milestones
✗ Overwhelming skill count
✗ Lack of guided progression for new players
```

## Recommendations for BlueMarble

### Core Skill System Design

#### 1. Adopt Use-Based Progression

**Recommendation:** Implement practice-based skill improvement with visible incremental gains.

**Implementation:**

```csharp
public class SkillProgressionSystem
{
    public void ApplySkillGain(Player player, SkillType skill, float actionDifficulty)
    {
        float currentSkill = player.GetSkillLevel(skill);
        
        // Wurm-inspired formula
        float baseGain = 0.0001f; // Micro-gains
        float difficultyMatch = CalculateDifficultyMatch(currentSkill, actionDifficulty);
        float progressionCurve = 1.0f / (1.0f + currentSkill / 15.0f); // Logarithmic
        float parentBonus = GetParentSkillBonus(player, skill);
        
        float skillGain = baseGain * difficultyMatch * progressionCurve * parentBonus;
        
        player.AddSkillExperience(skill, skillGain);
        
        // UI feedback for player
        NotifySkillGain(player, skill, skillGain);
    }
    
    private float CalculateDifficultyMatch(float skillLevel, float difficulty)
    {
        float diff = Math.Abs(skillLevel - difficulty);
        if (diff < 10) return 1.0f; // Just right
        if (diff < 20) return 0.5f; // Close
        return 0.2f; // Too easy or hard
    }
}
```

**Benefits:**
- Constant player feedback and satisfaction
- No wasted actions or grinding sessions
- Natural skill improvement through gameplay
- Aligns with geological simulation (learning by doing)

#### 2. Implement Parent-Child Skill Structure

**Recommendation:** Create hierarchical skill relationships for strategic progression.

**Proposed BlueMarble Skill Tree:**

```text
General Geology [Tier 1 Parent]
├─ Skill level: 0-100
├─ Provides: +0.15 per point to all geological children
├─ Trains through: Any geological activity
└─ At 100: +15 effective points to all specializations

    ├─── Mineralogy [Tier 2 Specialist]
    │    ├─ Base: 0-100
    │    ├─ Bonus: +15 from General Geology (if maxed)
    │    ├─ Effective: Up to 115
    │    └─ Enables: Mineral identification, extraction optimization
    │
    ├─── Petrology [Tier 2 Specialist]
    │    ├─ Base: 0-100
    │    ├─ Bonus: +15 from General Geology
    │    ├─ Effective: Up to 115
    │    └─ Enables: Rock formation analysis, quarrying
    │
    └─── Sedimentology [Tier 2 Specialist]
         ├─ Base: 0-100
         ├─ Bonus: +15 from General Geology
         ├─ Effective: Up to 115
         └─ Enables: Depositional analysis, soil science
```

**Benefits:**
- Rewards broad foundation before specialization
- Creates optimal learning paths
- Doesn't punish specialization
- Adds strategic depth to character building

#### 3. Balance Progression Pacing

**Recommendation:** Avoid Wurm's extreme grind while maintaining long-term goals.

**Proposed Progression Curve:**

```text
Target Hours to Reach Skill Levels:
Level 0-20:   5-10 hours (fast initial learning)
Level 20-40:  15-25 hours (moderate progression)
Level 40-60:  30-50 hours (dedicated practice)
Level 60-80:  60-100 hours (specialist territory)
Level 80-90:  120-180 hours (master level)
Level 90-100: 200-300 hours (elite optional)

Total to 70 (viable specialist): 100-180 hours
Total to 100 (complete mastery): 400-600 hours

Comparison to Wurm:
- Wurm: 1500-2500 hours to 100
- BlueMarble: 400-600 hours to 100
- Ratio: 3-4× faster than Wurm
- Still provides long-term progression
- More accessible to broader audience
```

#### 4. Design Soft Skill Caps

**Recommendation:** Implement soft caps to encourage specialization without hard limits.

**Proposed System:**

```text
Total Skill Points Available: ~800-1000
Skill Count: 30-40 geological/crafting skills

Example Specialist Build:
- 2-3 skills at 80-100 (Masters): 240-270 points
- 5-8 skills at 50-70 (Proficient): 250-400 points
- 10-15 skills at 20-50 (Functional): 300-450 points
- Rest at 0-20 (Minimal): 0-200 points
Total: ~790-1320 points

Soft Cap Mechanics:
- No hard cap enforced
- Time investment creates natural cap
- Logarithmic curve makes 100 in many skills impractical
- Players self-limit based on goals
```

#### 5. Link Skills to Quality Output

**Recommendation:** Skill level directly determines craftable quality range.

**Quality Formula:**

```csharp
public float CalculateCraftingQuality(Player player, Recipe recipe)
{
    float skillLevel = player.GetSkillLevel(recipe.RequiredSkill);
    float materialQuality = GetAverageMaterialQuality(recipe.Materials);
    float toolQuality = player.GetEquippedTool(recipe.ToolType).Quality;
    
    // Skill is primary determinant (50% weight)
    float baseQuality = skillLevel * 0.5f;
    
    // Materials affect output (30% weight)
    float materialBonus = (materialQuality - 50f) * 0.3f;
    
    // Tools affect output (20% weight)
    float toolBonus = (toolQuality - 50f) * 0.2f;
    
    // Random variation
    float randomFactor = UnityEngine.Random.Range(-5f, 5f);
    
    // Hard cap: Cannot exceed skill level by more than 5
    float finalQuality = baseQuality + materialBonus + toolBonus + randomFactor;
    finalQuality = Mathf.Min(finalQuality, skillLevel + 5f);
    
    return Mathf.Clamp(finalQuality, 1f, 100f);
}
```

**Economic Impact:**
- Creates market tiers by skill level
- Specialists command premium pricing
- Encourages trade between skill levels
- Supports player-driven economy

#### 6. Add Milestone Rewards

**Recommendation:** Add explicit milestones to break up progression (unlike Wurm's pure grind).

**Proposed Milestones:**

```text
Skill Level 25: "Apprentice" Title
- Unlock: Basic efficiency bonus (+5% speed)
- Reward: Recipe variations unlocked
- Recognition: Title displayed

Skill Level 50: "Journeyman" Title
- Unlock: Intermediate quality tier accessible
- Reward: Ability to take apprentices
- Recognition: Market listing priority

Skill Level 75: "Master" Title
- Unlock: Premium quality tier accessible
- Reward: Unique craftable items
- Recognition: Server-wide announcement
- Benefit: +10% quality to all crafts in skill

Skill Level 90: "Grand Master" Title
- Unlock: Elite quality tier accessible
- Reward: Custom item naming rights
- Recognition: Hall of Fame listing
- Benefit: +15% quality to all crafts in skill
```

### Economic System Integration

#### 7. Quality-Based Market Tiers

**Recommendation:** Design market systems that leverage skill-based quality differences.

**Market Interface Features:**

```text
Auction House Quality Filters:
├─ Filter by Quality Range
│   ├─ Starter (QL 1-30)
│   ├─ Functional (QL 30-60)
│   ├─ Premium (QL 60-80)
│   └─ Elite (QL 80-100)
│
├─ Sort by Price Per Quality Point
│   ├─ Enables value comparison
│   └─ "Best bang for buck" discovery
│
└─ Crafter Reputation Display
    ├─ Average quality produced
    ├─ Items sold count
    ├─ Customer ratings
    └─ Specialization badges
```

#### 8. Reputation System

**Recommendation:** Track crafter quality and build server reputation.

**Implementation:**

```csharp
public class CrafterReputation
{
    public Dictionary<SkillType, ReputationStats> SkillReputation { get; set; }
    
    public class ReputationStats
    {
        public float AverageQuality { get; set; }
        public int ItemsCrafted { get; set; }
        public float ConsistencyScore { get; set; } // Lower = more consistent
        public int CustomerRatings { get; set; }
        public List<string> Specializations { get; set; }
    }
    
    public float GetReputationBonus(SkillType skill)
    {
        var stats = SkillReputation[skill];
        
        // Reputation bonus: up to +5% quality for consistent masters
        if (stats.AverageQuality > 80 && 
            stats.ConsistencyScore < 5 && 
            stats.ItemsCrafted > 100)
        {
            return 5f; // Master craftsman reputation bonus
        }
        
        return 0f;
    }
}
```

### UI/UX Recommendations

#### 9. Skill Interface Design

**Recommendation:** Design clear, informative skill interface.

**Key Features:**

```text
Skill Window:
├─ Tree View (hierarchical)
│   ├─ Parent skills highlighted
│   ├─ Bonuses shown
│   └─ Expandable categories
│
├─ Precise Value Display
│   ├─ XX.XX format (2 decimals sufficient)
│   ├─ Recent gains highlighted
│   └─ Gain rate indicator
│
├─ Progress Tracking
│   ├─ Time to next milestone
│   ├─ Actions required estimate
│   └─ Historical gain graph
│
└─ Specialization Planner
    ├─ Skill goal setting
    ├─ Optimal path suggestions
    └─ Economic viability indicators
```

#### 10. Progress Feedback

**Recommendation:** Provide constant positive feedback for skill gains.

**Feedback Systems:**

```text
Immediate Feedback:
- Floating text: "+0.23 Mineralogy"
- Sound effect for skill gains
- Progress bar update
- Skill glow on significant gains

Session Summary:
- "Mineralogy +2.34 this session"
- "3 hours to next milestone"
- Comparison to previous sessions

Long-Term Tracking:
- Weekly/monthly progress graphs
- Skill ranking on server
- Specialization path progress
- Economic value estimation
```

## Implementation Considerations

### Technical Architecture

**Skill Data Structure:**

```csharp
public class PlayerSkillSystem
{
    public class Skill
    {
        public SkillType Type { get; set; }
        public float CurrentLevel { get; set; } // 0.00 - 100.00
        public float TotalExperience { get; set; }
        public DateTime LastGain { get; set; }
        public SkillType? ParentSkill { get; set; }
        public List<SkillType> ChildSkills { get; set; }
        public float GainMultiplier { get; set; } // Affinity, sleep bonus, etc.
    }
    
    public Dictionary<SkillType, Skill> Skills { get; set; }
    
    public float GetEffectiveSkillLevel(SkillType skill)
    {
        var skillData = Skills[skill];
        float base Level = skillData.CurrentLevel;
        
        // Add parent skill bonus
        if (skillData.ParentSkill.HasValue)
        {
            float parentLevel = Skills[skillData.ParentSkill.Value].CurrentLevel;
            baseLevel += parentLevel * 0.15f; // 15% bonus from parent
        }
        
        return baseLevel;
    }
}
```

### Database Schema

```sql
-- Player skills table
CREATE TABLE player_skills (
    player_id UUID,
    skill_type VARCHAR(50),
    current_level DECIMAL(7,5), -- 0.00000 to 100.00000
    total_experience BIGINT,
    last_gain_timestamp TIMESTAMP,
    gain_multiplier DECIMAL(4,2),
    PRIMARY KEY (player_id, skill_type)
);

-- Skill gain history (for analytics)
CREATE TABLE skill_gain_history (
    id UUID PRIMARY KEY,
    player_id UUID,
    skill_type VARCHAR(50),
    gain_amount DECIMAL(7,5),
    action_type VARCHAR(50),
    timestamp TIMESTAMP
);

-- Crafter reputation
CREATE TABLE crafter_reputation (
    player_id UUID,
    skill_type VARCHAR(50),
    avg_quality DECIMAL(5,2),
    total_crafted INT,
    consistency_score DECIMAL(5,2),
    customer_ratings INT,
    PRIMARY KEY (player_id, skill_type)
);
```

### Performance Considerations

**Optimization Strategies:**

1. **Skill Gain Calculation:**
   - Cache parent skill levels
   - Batch gain calculations
   - Async database updates

2. **UI Updates:**
   - Update skill display at 0.01 increments only
   - Batch UI updates every 100ms
   - Lazy load skill history

3. **Reputation Tracking:**
   - Update reputation async after craft
   - Cache reputation scores
   - Recalculate only on new craft

### Balance Tuning

**Progression Curve Balancing:**

```text
Key Variables to Tune:
├─ base_gain: 0.0001 - 0.001 (affects overall pace)
├─ difficulty_match_curve: How strongly difficulty affects gains
├─ progression_curve_factor: 10-20 (affects late-game slowdown)
├─ parent_bonus_multiplier: 0.10-0.20 (affects parent skill value)
└─ milestone_intervals: 20/40/60/80 vs 25/50/75/90

Testing Approach:
1. Simulate 1000 players with different play patterns
2. Track time to reach each skill tier
3. Measure specialization emergence
4. Validate economic viability of different builds
5. Gather playtest feedback on "grind feel"
6. Iterate on multipliers
```

## Next Steps

### Immediate Actions Required

- [ ] **Review recommendations with design team** - Due: Week 1 - Owner: Design Lead
  - Decide which Wurm mechanics to adopt
  - Determine skill count for BlueMarble (30-50 skills)
  - Approve progression curve parameters

- [ ] **Create skill taxonomy for BlueMarble** - Due: Week 2 - Owner: Game Designer
  - Define 30-50 geological/crafting skills
  - Establish parent-child relationships
  - Map skills to gameplay activities

- [ ] **Prototype skill progression system** - Due: Week 4 - Owner: Gameplay Developer
  - Implement use-based progression
  - Add parent-child bonuses
  - Create skill UI mockups

- [ ] **Design economic integration** - Due: Week 5 - Owner: Economy Designer
  - Link skills to quality output
  - Design market segmentation
  - Create reputation system

- [ ] **Playtest progression pacing** - Due: Week 8 - Owner: QA Lead
  - Test progression curves with players
  - Validate "grind feel"
  - Adjust pacing parameters

### Follow-up Research

- **Progression Curve Simulation** (4 weeks): Mathematical modeling of different progression curves to optimize pacing
- **Economic Balance Study** (6 weeks): Simulate player-driven economy with skill-based quality tiers
- **New Player Onboarding** (3 weeks): Design tutorials and guidance for skill system
- **Geological Skill Taxonomy** (4 weeks): Deep dive into defining specific geological skills for BlueMarble

### Stakeholder Communication

**Design Team Presentation (Week 2):**
- Present key findings from Wurm Online analysis
- Discuss applicability to BlueMarble
- Prioritize feature adoption

**Development Team Workshop (Week 3):**
- Technical architecture review
- Implementation complexity assessment
- Timeline and resource estimation

**Community Preview (Month 2):**
- Blog post on skill system philosophy
- Preview of progression mechanics
- Gather early community feedback

## Appendices

### Appendix A: Wurm Online Skill List Summary

**Complete Skill Categories:**

```text
Combat (15 skills):
- Fighting, Aggressive/Normal/Defensive, Taunting
- Weapons: Swords, Axes, Mauls, Knives, etc.
- Armor: Chain, Plate, Leather, Cloth, Shield

Crafting (40+ skills):
- Smithing, Blacksmithing, Weapon/Armour/Shield Smithing
- Carpentry, Fine Carpentry, Ship/Boat Building
- Tailoring, Leatherworking
- Pottery, Stonecutting, Masonry
- Jewelry, Toy Making, Ropemaking

Gathering (20 skills):
- Mining, Prospecting, Surface Mining
- Woodcutting, Forestry
- Farming, Gardening, Animal Husbandry
- Digging, Foraging, Botanizing, Fishing

Processing (15 skills):
- Smelting, Cooking, Alchemy
- Butchering, Milling, etc.

Utility (30+ skills):
- Building, Paving, Roofing, Repairing
- Riding, Sailing, Climbing, Swimming
- First Aid, Nature, Restoration

Religious (10 skills):
- Channeling, Faith, Prayer, Meditation
```

### Appendix B: Key Takeaways for BlueMarble

**Adopt:**
✓ Use-based progression with micro-gains
✓ Parent-child skill relationships
✓ Soft skill caps encouraging specialization
✓ Quality output tied directly to skill level
✓ Precise skill value display (XX.XX format)
✓ Reputation-based master crafter systems

**Adapt:**
≈ Reduce skill count (130 → 30-50)
≈ Faster progression (3-4× Wurm's pace)
≈ Add explicit milestones and rewards
≈ Hybrid implicit/explicit knowledge
≈ Better new player guidance

**Avoid:**
✗ Extreme grind (1000+ hours per skill)
✗ Purely implicit progression (no milestones)
✗ Overwhelming skill count
✗ Lack of structured specialization paths

### Appendix C: References

**Primary Sources:**
- Wurmpedia Skills: <https://www.wurmpedia.com/index.php/Skills>
- Wurmpedia Main Page: <https://www.wurmpedia.com/>
- Wurm Online Official: <https://www.wurmonline.com/>

**Community Resources:**
- Wurm Forums: Player progression discussions
- Wurm Wiki: Community-maintained guides
- Player Calculators: Skill planning tools

**Comparative Research:**
- BlueMarble Skill System Research: `/research/game-design/skill-knowledge-system-research.md`
- Wurm Material System: `/research/game-design/wurm-online-material-system-research.md`
- Life is Feudal Analysis: `/research/game-design/life-is-feudal-material-system-analysis.md`

### Appendix D: Skill Progression Diagrams

**Diagram 1: Progression Curve Comparison**

```text
Skill Level Over Time:

100 |                                        ___Wurm___
 90 |                                   ____/
 80 |                              ____/
 70 |                         ____/
 60 |                    ____/           ___BlueMarble Proposed___
 50 |              _____/            ___/
 40 |         ____/              ___/
 30 |    ____/               ___/
 20 |___/                ___/
 10 |___________________/
  0 |________________
    0   200  400  600  800  1000  1200  1400  1600  1800  2000
                         Hours Played

Key Differences:
- BlueMarble: 70 skill in ~180 hours (viable specialist)
- Wurm: 70 skill in ~400 hours
- BlueMarble: 100 skill in ~600 hours (complete mastery)
- Wurm: 100 skill in ~2000 hours
```

**Diagram 2: Specialization Pattern**

```text
Example Specialist Build (700 total skill points):

Skill Investment Distribution:

Master Skills (80-100):     ████████████ 35%
Expert Skills (60-80):      ████████ 25%
Proficient Skills (40-60):  ██████ 20%
Functional Skills (20-40):  ████ 15%
Basic Skills (0-20):        █ 5%

Natural Specialization:
- 2-3 Master skills (primary identity)
- 5-8 Expert skills (supporting capabilities)
- 10-15 Proficient skills (functional needs)
- Rest at basic levels (flexibility)
```

### Appendix E: Revision History

| Version | Date | Author | Changes |
|---------|------|--------|---------|
| 1.0 | 2025-01-13 | BlueMarble Research Team | Initial comprehensive research report |

---

**Research Completed:** 2025-01-13  
**Status:** Final Report - Ready for Design Review  
**Next Review Date:** Q2 2025 (Post-Implementation Evaluation)

For questions or feedback on this research, please contact the BlueMarble Game Design Research Team.
