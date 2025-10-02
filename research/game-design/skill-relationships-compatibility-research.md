# Skill Relationships and Compatibility - Research Report

**Document Type:** Research Report  
**Version:** 1.0  
**Author:** Game Design Research Team  
**Date:** 2025-10-02  
**Status:** Final  
**Research Type:** Game Mechanics Research  
**Related Documents:** 
- [Realistic Basic Skills Research](realistic-basic-skills-research.md)
- [Assembly Skills System Research](assembly-skills-system-research.md)
- [Life is Feudal Material System Analysis](life-is-feudal-material-system-analysis.md)
- [Skill Knowledge System Research](skill-knowledge-system-research.md)
- [Crafting Interface Mockups](assets/crafting-interface-mockups.md)

## Executive Summary

This research investigates the relationships, compatibility, and substitutability between basic skills in 
BlueMarble's skill system. Through detailed analysis of skill overlaps, dependencies, synergies, and conflicts, 
this document provides frameworks for representing complex skill interactions in a realistic game system. 
The research addresses key questions about how skills interact (e.g., does tailoring overlap with weaving?), 
whether skills can substitute for each other (e.g., can blacksmithing replace basic metalworking?), and how 
to model these relationships effectively.

**Key Findings:**
- Skills exhibit three primary relationship types: hierarchical (parent-child), complementary (synergistic), 
  and competitive (overlapping)
- Substitutability exists on a spectrum rather than binary yes/no
- Cross-skill bonuses create natural progression incentives
- Skill overlap analysis reveals 15 major skill clusters with varying degrees of interaction
- Network models and compatibility matrices provide robust frameworks for implementation
- The Fiber (Leaves) crafting system exemplifies effective skill dependency visualization


## Table of Contents

1. [Research Objectives](#research-objectives)
2. [Methodology](#methodology)
3. [Skill Relationship Types](#skill-relationship-types)
4. [Skill Overlap Analysis](#skill-overlap-analysis)
5. [Substitutability Framework](#substitutability-framework)
6. [Compatibility Systems](#compatibility-systems)
7. [Dependency Networks](#dependency-networks)
8. [Synergy and Conflict Models](#synergy-and-conflict-models)
9. [Network Representations](#network-representations)
10. [Case Study: Fiber Crafting System](#case-study-fiber-crafting-system)
11. [Implementation Recommendations](#implementation-recommendations)
12. [Appendices](#appendices)

## Research Objectives

### Primary Questions

1. **Skill Overlap:** Do skills like tailoring and weaving overlap? How much functional duplication exists?
2. **Substitutability:** Can one skill replace another (e.g., blacksmithing vs. basic metalworking)?
3. **Compatibility:** Which skill combinations create synergies vs. conflicts?
4. **Dependencies:** What prerequisite relationships exist between skills?
5. **Representation:** How can we model these relationships for implementation?

### Success Criteria

- Comprehensive taxonomy of skill relationship types
- Detailed overlap analysis for all 15+ core skills
- Substitutability scoring system (0-100%)
- Compatibility matrices showing skill pair interactions
- Visual network models for skill dependencies
- Actionable implementation recommendations


## Methodology

### Research Approach

**Multi-Method Analysis:**
1. **Historical Analysis** - Study real-world craft relationships and trade interactions
2. **Game System Review** - Analyze successful skill systems in Wurm Online, Life is Feudal, EVE Online
3. **Graph Theory Application** - Model skills as nodes with weighted edges representing relationships
4. **Matrix Analysis** - Create compatibility matrices for quantitative skill pair assessment
5. **Player Behavior Study** - Review common skill combinations and progression paths

### Data Sources

- Existing BlueMarble research documents (realistic-basic-skills-research.md, assembly-skills-system-research.md)
- Historical craft guild structures and apprenticeship systems
- MMORPG skill system documentation and player wikis
- Graph theory and network analysis literature
- Real-world occupational compatibility studies

### Analytical Framework

Skills are evaluated across five dimensions:
1. **Functional Overlap** - Do skills produce similar outputs?
2. **Resource Sharing** - Do skills use the same materials/tools?
3. **Knowledge Transfer** - Do skills share conceptual foundations?
4. **Temporal Compatibility** - Can skills be practiced simultaneously?
5. **Economic Complementarity** - Do skills create market synergies?


## Skill Relationship Types

### 1. Hierarchical Relationships (Parent-Child)

**Definition:** One skill serves as a foundation for another, with the parent skill providing bonuses 
or unlocking capabilities in the child skill.

**Characteristics:**
- Unidirectional influence (parent → child)
- Parent skill level provides passive bonuses to child
- Child skill may require minimum parent skill level
- Multiplicative progression benefits

**Example: Metalworking Hierarchy**

```
Artisan (Parent Skill)
│
├─→ Smelting (Tier 2 Parent)
│   ├─→ Blacksmithing (Tier 3 Child)
│   │   ├─→ Weaponsmithing (Tier 4 Specialist)
│   │   └─→ Armorsmithing (Tier 4 Specialist)
│   └─→ Jewelry Making (Tier 3 Child)
│
└─→ Woodworking (Tier 2 Parent)
    ├─→ Carpentry (Tier 3 Child)
    └─→ Bowyer (Tier 3 Child)

Bonus Formula:
Effective Child Skill = Base + (Parent₁ × 0.1) + (Parent₂ × 0.15)

Example Calculation:
- Blacksmithing Base: 60
- Artisan Level: 80 (+8 bonus)
- Smelting Level: 70 (+10.5 bonus)
- Effective Blacksmithing: 78.5
```

**BlueMarble Applications:**
- Herbalism → Alchemy (gathering expertise improves crafting)
- Mining → Blacksmithing (ore knowledge enhances metalwork)
- Forestry → Woodworking (wood identification improves carpentry)
- Cooking → Alchemy (understanding mixtures and reactions)

### 2. Complementary Relationships (Synergistic)

**Definition:** Skills that work together to create greater value than either skill alone, 
without strict dependency.

**Characteristics:**
- Bidirectional benefits
- Combined use produces superior outcomes
- No hard prerequisites
- Additive or multiplicative bonuses

**Example: Crafting Synergies**

```
Tailoring + Leatherworking = Enhanced Armor Crafting
├─ Tailoring provides: Cloth padding, comfort linings
├─ Leatherworking provides: Protective layers, durability
└─ Combined: Superior mixed-material armor (+15% quality)

Blacksmithing + Woodworking = Tool Making Mastery
├─ Blacksmithing provides: Metal heads, blades, reinforcements
├─ Woodworking provides: Handles, hafts, grips
└─ Combined: Exceptional tools (+20% efficiency)

Alchemy + Cooking = Advanced Gastronomy
├─ Alchemy provides: Potency understanding, extraction methods
├─ Cooking provides: Flavor balance, preparation techniques
└─ Combined: Gourmet dishes with buff effects (+25% duration)
```

**Synergy Bonus Formula:**

```
Quality Bonus = Base Quality × (1 + (Skill₁ × Skill₂ × Synergy_Coefficient))

Where Synergy_Coefficient ranges from 0.001 to 0.005 depending on compatibility

Example:
- Base Quality: 70%
- Tailoring: 80
- Leatherworking: 75
- Synergy_Coefficient: 0.002
- Bonus: 70 × (1 + (80 × 75 × 0.002)) = 70 × (1 + 12) = 910% (capped at reasonable values)

Realistic Formula:
Quality Bonus = Base Quality + (Skill₁/10 + Skill₂/10) × Synergy_Coefficient
Example: 70 + (8 + 7.5) × 1.5 = 70 + 23.25 = 93.25%
```

### 3. Competitive Relationships (Overlapping)

**Definition:** Skills that produce similar outputs or serve similar functions, creating functional overlap 
and potential substitutability.

**Characteristics:**
- High functional similarity
- Resource competition
- Player choice of specialization
- Diminishing returns when both are pursued

**Example: Textile Production Overlap**

```
Tailoring vs. Weaving vs. Fiber Processing

Tailoring (Assembly Focus):
├─ Cuts and assembles pre-made cloth
├─ Pattern creation and fitting
├─ Garment construction
└─ Output: Clothing, bags, banners

Weaving (Production Focus):
├─ Transforms thread into cloth
├─ Pattern weaving and texture
├─ Cloth quality determination
└─ Output: Cloth bolts, textiles

Fiber Processing (Raw Material Focus):
├─ Prepares raw fibers (flax, hemp, cotton)
├─ Spinning thread from fiber
├─ Dyeing and treatment
└─ Output: Thread, dyed fibers

Overlap Analysis:
- Tailoring ↔ Weaving: 40% overlap (both produce usable textiles)
- Weaving ↔ Fiber Processing: 60% overlap (both handle raw materials)
- Tailoring ↔ Fiber Processing: 20% overlap (minimal direct interaction)
```

**Specialization Pressure:**

When skills overlap, players face specialization decisions:

```
Option A: Vertical Integration (Full Chain)
Fiber Processing (50) → Weaving (50) → Tailoring (70)
- Pros: Complete self-sufficiency, quality control
- Cons: High skill point investment, time intensive

Option B: Specialization (Focus on One)
Tailoring (100) only
- Pros: Master-level output, efficient skill points
- Cons: Dependent on others for materials

Option C: Partnership Focus (Complementary Pair)
Tailoring (80) + Leatherworking (80)
- Pros: Diverse product line, synergies
- Cons: Still dependent on suppliers
```


### 4. Enabling Relationships (Prerequisites)

**Definition:** One skill must be learned before another can be accessed or practiced effectively.

**Characteristics:**
- Hard requirement (cannot proceed without prerequisite)
- Unlock mechanism for advanced content
- Logical skill progression
- Prevents premature specialization

**Example: Prerequisite Chains**

```
Mining (Level 1+) → Required for → Blacksmithing
Herbalism (Level 1+) → Required for → Alchemy
Forestry (Level 1+) → Required for → Woodworking

Advanced Prerequisites:
Blacksmithing (Level 50+) → Required for → Weaponsmithing Specialization
Alchemy (Level 40+) + Herbalism (Level 30+) → Required for → Master Alchemist

Soft Prerequisites (Recommended but not required):
Cooking (Level 20+) → Recommended for → Alchemy (understanding mixtures)
Combat (Level 30+) → Recommended for → Weaponsmithing (understanding weapon use)
```

### 5. Conflicting Relationships (Mutually Exclusive)

**Definition:** Skills that cannot coexist at high levels due to philosophical, mechanical, 
or balance reasons.

**Characteristics:**
- Skill cap systems enforce trade-offs
- Alignment-based restrictions
- Realism constraints
- Economic balance requirements

**Example: Alignment-Based Conflicts**

```
Crafting Alignment vs. Combat Alignment
(From Life is Feudal System Analysis)

Pure Crafter (+100 Alignment):
├─ Bonus: +20% crafting speed, +15% quality
├─ Penalty: -30% combat effectiveness
└─ Cannot excel in: Combat skills, weapon mastery

Pure Warrior (-100 Alignment):
├─ Bonus: +20% combat damage, +15% armor effectiveness
├─ Penalty: -30% crafting speed and quality
└─ Cannot excel in: Production skills, crafting mastery

Balanced Player (0 Alignment):
├─ No bonuses or penalties
└─ Can develop both but excels at neither
```

**Skill Cap Conflicts:**

```
Total Skill Points: 1000 (example cap)
Mastery Requirement: 100 points per skill

Scenario: Player wants to master multiple overlapping skills
- Tailoring (100) + Weaving (100) + Fiber Processing (100) = 300 points
- Remaining: 700 points for other skills
- Trade-off: Deep specialization in textiles vs. broader skill set

Optimal Strategy:
- Master Tailoring (100) 
- Proficient Weaving (60) - bonus from Tailoring synergy
- Basic Fiber Processing (30) - just enough for basic materials
- Total: 190 points, leaving 810 for other skill trees
```


## Skill Overlap Analysis

### Detailed Overlap Assessment

This section examines specific skill pairs to determine functional overlap, substitutability, 
and complementarity.

### Case 1: Tailoring vs. Weaving

**Skill Definitions:**

**Tailoring:**
- Primary Function: Assembling cloth into garments and items
- Key Activities: Pattern creation, cutting, sewing, fitting
- Input Materials: Pre-made cloth, thread, fasteners
- Output Products: Clothing, bags, banners, cloth armor
- Skill Focus: Design and assembly

**Weaving:**
- Primary Function: Creating cloth from thread
- Key Activities: Loom operation, pattern weaving, texture control
- Input Materials: Thread, yarn, dyes
- Output Products: Cloth bolts, tapestries, decorative textiles
- Skill Focus: Material production

**Overlap Analysis:**

```
Functional Overlap: 35%

Shared Aspects:
├─ Understanding of fiber properties: 15%
├─ Pattern design and visualization: 10%
├─ Color theory and dye knowledge: 7%
└─ Textile quality assessment: 3%

Unique to Tailoring:
├─ Garment construction techniques
├─ Body measurement and fitting
├─ Complex assemblies (multi-piece garments)
└─ Fastener integration (buttons, ties)

Unique to Weaving:
├─ Loom operation and maintenance
├─ Thread tension control
├─ Warp and weft manipulation
└─ Fabric structure creation
```

**Substitutability Score: 25%**

Can Weaving replace Tailoring?
- No - Weaving produces materials, not finished garments
- A weaver cannot create fitted clothing without tailoring skill
- Weaver can produce simple uncut cloth items (wraps, shawls)

Can Tailoring replace Weaving?
- Partially - A tailor can work with existing cloth
- Cannot produce cloth from raw materials
- Must purchase or trade for cloth supplies

**Optimal Relationship:** Sequential Dependency (Weaving → Tailoring)

```
Progression Path:
1. Fiber Processing (1-30) - Prepare raw materials
2. Weaving (1-50) - Create cloth
3. Tailoring (1-100) - Master garment creation

Alternative Path (Specialization):
- Pure Tailor: Skill 100 Tailoring, buy cloth from weavers
- Pure Weaver: Skill 100 Weaving, sell cloth to tailors
```

### Case 2: Blacksmithing vs. Basic Metalworking

**Skill Definitions:**

**Blacksmithing:**
- Primary Function: Shaping heated metal into tools, weapons, armor
- Key Activities: Forging, hammering, tempering, heat treatment
- Input Materials: Metal ingots, fuel (coal/charcoal), water
- Output Products: Weapons, armor, tools, hardware
- Skill Focus: Hot working and shaping

**Basic Metalworking (if separate):**
- Primary Function: General metal manipulation and assembly
- Key Activities: Filing, grinding, riveting, cold working
- Input Materials: Metal pieces, fasteners, abrasives
- Output Products: Repairs, assemblies, simple modifications
- Skill Focus: Cold working and assembly

**Overlap Analysis:**

```
Functional Overlap: 70%

Shared Aspects:
├─ Metal property understanding: 20%
├─ Tool usage and maintenance: 15%
├─ Quality assessment: 10%
├─ Safety procedures: 10%
├─ Material waste minimization: 8%
└─ Basic shaping techniques: 7%

Unique to Blacksmithing:
├─ Forge operation and temperature control
├─ Heat treatment and tempering
├─ Complex forging techniques
└─ Alloy creation

Unique to Basic Metalworking:
├─ Precision fitting
├─ Assembly techniques
├─ Repair methodology
└─ Surface finishing
```

**Substitutability Score: 85%**

Can Blacksmithing replace Basic Metalworking?
- Yes - Most blacksmithing skills encompass basic metalworking
- A blacksmith can perform all basic metalworking tasks
- Blacksmithing is essentially advanced metalworking

Can Basic Metalworking replace Blacksmithing?
- Partially - Can handle repairs and modifications
- Cannot create items from raw metal (no forging capability)
- Limited to working with existing metal pieces

**Optimal Relationship:** Hierarchical (Basic Metalworking → Blacksmithing)

**Recommendation for BlueMarble:**

```
Option A: Single Unified Skill
- Call it "Blacksmithing" 
- Include both hot and cold working
- Simpler for players, less skill bloat

Option B: Hierarchical Split
- Basic Metalworking (Levels 1-30): Cold working, repairs, assembly
- Blacksmithing (Levels 31-100): Forge work, creation, tempering
- Allows gradual progression, more realistic

Option C: Parent-Child Structure
- Blacksmithing (Parent Skill)
  ├─ Forging (Child): Hot working
  ├─ Fitting (Child): Cold working  
  └─ Tempering (Child): Heat treatment
- Maximum realism, higher complexity

RECOMMENDED: Option B - Hierarchical Split
- Balances realism with playability
- Natural progression path
- Clear skill differentiation
```


### Case 3: Alchemy vs. Cooking

**Skill Definitions:**

**Alchemy:**
- Primary Function: Creating potions, elixirs, and chemical compounds
- Key Activities: Ingredient extraction, solution mixing, distillation, reaction control
- Input Materials: Herbs, minerals, solvents, catalysts
- Output Products: Potions, poisons, dyes, chemical reagents
- Skill Focus: Chemical reactions and potency

**Cooking:**
- Primary Function: Preparing food and beverages
- Key Activities: Ingredient preparation, cooking, seasoning, preservation
- Input Materials: Meat, vegetables, grains, spices, liquids
- Output Products: Meals, preserved foods, beverages
- Skill Focus: Flavor and nutrition

**Overlap Analysis:**

```
Functional Overlap: 45%

Shared Aspects:
├─ Ingredient identification and sourcing: 12%
├─ Mixture creation and ratios: 10%
├─ Heat control and timing: 8%
├─ Preservation techniques: 7%
├─ Tool usage (mortars, pots, knives): 5%
└─ Quality ingredient selection: 3%

Unique to Alchemy:
├─ Chemical reaction understanding
├─ Potency calculation and concentration
├─ Magical/mystical infusion
├─ Poison and antidote creation
└─ Distillation and purification

Unique to Cooking:
├─ Flavor balancing and seasoning
├─ Nutritional content optimization
├─ Texture and presentation
├─ Various cooking methods (roasting, baking, frying)
└─ Recipe scaling for portions
```

**Substitutability Score: 30%**

**Cross-Skill Benefits:**

```
Cooking Benefits from Alchemy:
├─ +10% efficiency when processing herbs
├─ +15% effectiveness of food buffs
├─ Unlock: Medicinal foods
└─ Unlock: Long-lasting preserves

Alchemy Benefits from Cooking:
├─ +10% success rate for liquid mixtures
├─ +12% potion shelf life
├─ Unlock: Flavored potions (easier consumption)
└─ Unlock: Food-based buff potions

Combined Mastery Unlocks:
├─ Gastronomy (Gourmet food with alchemical effects)
├─ Medicine (Healing foods and drinks)
└─ Fermentation (Alcohols with special properties)
```

**Optimal Relationship:** Complementary with Cross-Bonuses

### Case 4: Carpentry vs. Woodworking vs. Forestry

**Skill Definitions:**

**Forestry:**
- Primary Function: Tree identification, logging, sustainable harvesting
- Key Activities: Tree selection, felling, log processing
- Output: Raw logs, wood identification knowledge

**Woodworking:**
- Primary Function: Basic wood shaping and item creation
- Key Activities: Cutting, planing, joining, finishing
- Output: Furniture, tools, simple structures

**Carpentry:**
- Primary Function: Advanced structural wood construction
- Key Activities: Framing, joinery, architectural assembly
- Output: Buildings, bridges, complex wooden structures

**Overlap Analysis:**

```
Forestry ↔ Woodworking: 30% overlap
├─ Wood identification: 15%
├─ Grain reading: 8%
└─ Quality assessment: 7%

Woodworking ↔ Carpentry: 65% overlap
├─ Joinery techniques: 20%
├─ Tool usage: 18%
├─ Wood preparation: 15%
└─ Finishing methods: 12%

Forestry ↔ Carpentry: 15% overlap
├─ Structural wood selection: 10%
└─ Load-bearing assessment: 5%
```

**Recommended Structure:**

```
Forestry (Gathering Skill)
    ↓ (provides material knowledge)
Woodworking (Parent Craft Skill)
    ↓ (foundation for specialization)
    ├─→ Carpentry (Specialization)
    ├─→ Bowyer (Specialization)
    ├─→ Furniture Making (Specialization)
    └─→ Coopering/Barrel Making (Specialization)
```

### Comprehensive Overlap Matrix

```
                    Tailoring  Weaving  Blacksmithing  Metalwork  Alchemy  Cooking  Forestry  Woodwork
Tailoring              100%      35%        5%           8%        2%       5%        2%        3%
Weaving                35%      100%        3%           5%        4%       3%        8%        5%
Blacksmithing           5%       3%       100%          85%        8%       12%       5%        15%
Metalwork               8%       5%        85%         100%        6%       10%       3%        12%
Alchemy                 2%       4%         8%           6%      100%       45%       35%       8%
Cooking                 5%       3%        12%          10%       45%      100%       10%       12%
Forestry                2%       8%         5%           3%       35%       10%      100%       40%
Woodworking             3%       5%        15%          12%        8%       12%       40%      100%

Legend:
100% = Same skill
75-99% = Very high overlap (consider merging)
50-74% = High overlap (strong substitutability)
25-49% = Moderate overlap (complementary)
10-24% = Low overlap (some synergy)
0-9% = Minimal overlap (independent)
```


## Substitutability Framework

### Substitutability Spectrum

Substitutability exists on a continuum rather than as a binary property:

```
0%          25%         50%         75%         100%
|-----------|-----------|-----------|-----------|
No          Partial     Moderate    High        Complete
Substitution Overlap    Overlap     Overlap     Substitution

Examples by Category:

0-25% (Independent Skills):
- Tailoring ↔ Mining
- Alchemy ↔ Combat
- Forestry ↔ Fishing

25-50% (Complementary Skills):
- Alchemy ↔ Cooking (45%)
- Forestry ↔ Woodworking (40%)
- Tailoring ↔ Weaving (35%)

50-75% (High Overlap Skills):
- Woodworking ↔ Carpentry (65%)
- Hunting ↔ Survival (60%)
- Herbalism ↔ Farming (55%)

75-100% (Near-Identical Skills):
- Blacksmithing ↔ Basic Metalworking (85%)
- Mining ↔ Prospecting (80%)
- Fishing ↔ Angling (90%)
```

### Substitutability Factors

Five factors determine substitutability between skills:

```
1. Output Similarity (40% weight)
   - Do skills produce the same or similar items?
   - Can outputs serve the same function?

2. Process Similarity (25% weight)
   - Are the production methods similar?
   - Do they use similar tools and techniques?

3. Input Material Overlap (15% weight)
   - Do skills use the same raw materials?
   - Is there resource competition?

4. Knowledge Transfer (10% weight)
   - Does mastery of one inform the other?
   - Are underlying principles shared?

5. Market Competition (10% weight)
   - Do practitioners compete for the same customers?
   - Are products directly competing?

Substitutability Score Formula:
S = (Output × 0.40) + (Process × 0.25) + (Materials × 0.15) + 
    (Knowledge × 0.10) + (Market × 0.10)
```

### Substitutability Examples

**High Substitutability: Blacksmithing vs. Basic Metalworking (85%)**

```
Output Similarity: 90%
- Both produce metal items, tools, and equipment
- Blacksmithing adds weapon/armor specialization

Process Similarity: 95%
- Both involve metal shaping and manipulation
- Blacksmithing adds hot-working processes

Material Overlap: 85%
- Identical input materials (metal ingots, ores)
- Similar tool requirements

Knowledge Transfer: 80%
- Shared understanding of metal properties
- Blacksmithing builds on metalworking foundation

Market Competition: 70%
- Significant customer overlap
- Blacksmiths can fulfill basic metalwork orders

Total: (90×0.4) + (95×0.25) + (85×0.15) + (80×0.1) + (70×0.1) = 85.5%
```

**Moderate Substitutability: Alchemy vs. Cooking (45%)**

```
Output Similarity: 30%
- Different primary outputs (potions vs. food)
- Some overlap in buffs/effects

Process Similarity: 60%
- Both involve mixing and heat application
- Different end goals and precision requirements

Material Overlap: 40%
- Some shared ingredients (herbs, water, salt)
- Mostly different material bases

Knowledge Transfer: 55%
- Shared understanding of mixtures and reactions
- Different focus (chemistry vs. cuisine)

Market Competition: 25%
- Different primary markets
- Some overlap in buff/enhancement consumers

Total: (30×0.4) + (60×0.25) + (40×0.15) + (55×0.1) + (25×0.1) = 45.5%
```

**Low Substitutability: Tailoring vs. Weaving (35%)**

```
Output Similarity: 25%
- Different output types (garments vs. cloth)
- Sequential in production chain

Process Similarity: 40%
- Different tools (needle vs. loom)
- Different core techniques

Material Overlap: 35%
- Weaving produces what tailoring consumes
- Indirect material relationship

Knowledge Transfer: 50%
- Shared textile understanding
- Complementary rather than overlapping

Market Competition: 20%
- Different market positions
- Suppliers vs. craftsmen

Total: (25×0.4) + (40×0.25) + (35×0.15) + (50×0.1) + (20×0.1) = 32.25%
```

### Design Implications

**For High Substitutability Skills (75%+):**

```
Recommendation: Merge or Make Hierarchical

Option 1: Merge into Single Skill
- Simplifies skill system
- Reduces player confusion
- Example: "Blacksmithing" encompasses all metalworking

Option 2: Hierarchical Progression
- Basic skill unlocks advanced skill
- Natural progression path
- Example: Basic Metalworking (1-30) → Blacksmithing (31-100)

Option 3: Specialization Branches
- Single parent skill with specializations
- Player choice of focus area
- Example: Metalworking → [Blacksmithing | Jewelry | Weapons]
```

**For Moderate Substitutability Skills (40-75%):**

```
Recommendation: Implement Cross-Skill Bonuses

- Keep skills separate
- Award bonuses for having both
- Create synergistic combinations
- Example: Alchemy + Cooking = Gastronomy specialization
```

**For Low Substitutability Skills (0-40%):**

```
Recommendation: Keep Independent with Dependencies

- Maintain distinct skills
- Create dependency chains where logical
- Focus on complementary relationships
- Example: Weaving → Tailoring (sequential production)
```


## Compatibility Systems

### Compatibility Matrix

A compatibility matrix quantifies how well skill pairs work together:

```
Compatibility Score: -100 (conflict) to +100 (perfect synergy)

                Mining  Blacksmith  Tailoring  Alchemy  Cooking  Herbalism  Forestry  Woodwork
Mining            0        +85         -5        +15      +5        -10        +20       +10
Blacksmith      +85         0         +10        +15     +25        -5        +30       +40
Tailoring        -5       +10          0         +20     +10       +35        +15       +15
Alchemy         +15       +15        +20          0      +65       +80        +45       +20
Cooking          +5       +25        +10        +65       0        +55        +25       +30
Herbalism       -10        -5        +35        +80     +55         0         +50       +25
Forestry        +20       +30        +15        +45     +25       +50          0        +75
Woodworking     +10       +40        +15        +20     +30       +25        +75         0

Legend:
+80 to +100 = Exceptional Synergy (strongly recommended combination)
+60 to +79  = High Synergy (excellent combination)
+40 to +59  = Good Synergy (beneficial combination)
+20 to +39  = Moderate Synergy (helpful combination)
+1 to +19   = Minor Synergy (slight benefits)
0           = Neutral (no interaction)
-1 to -20   = Minor Conflict (slight disadvantages)
-21 to -50  = Moderate Conflict (noticeable trade-offs)
-51 to -100 = Strong Conflict (should avoid combination)
```

### High Synergy Combinations

**1. Herbalism + Alchemy (+80)**

```
Synergy Benefits:
├─ Herbalism provides ingredient knowledge
├─ Alchemy uses herbs more efficiently
├─ Combined: +20% potion potency
├─ Combined: +15% rare ingredient identification
└─ Unlock: Exotic potion recipes

Gameplay Impact:
- Herbalist-Alchemists are most efficient potion creators
- Self-sufficient ingredient gathering and processing
- Natural progression path for new players
- Economic advantage in potion markets
```

**2. Forestry + Woodworking (+75)**

```
Synergy Benefits:
├─ Forestry provides wood quality knowledge
├─ Woodworking uses lumber more efficiently
├─ Combined: +18% final product quality
├─ Combined: +12% material yield
└─ Unlock: Master woodcraft techniques

Gameplay Impact:
- Woodworker-Loggers control entire supply chain
- Better wood selection leads to superior items
- Reduced material waste
- Can identify optimal trees for specific projects
```

**3. Mining + Blacksmithing (+85)**

```
Synergy Benefits:
├─ Mining provides ore quality understanding
├─ Blacksmithing uses metals more effectively
├─ Combined: +20% forge efficiency
├─ Combined: +15% item durability
└─ Unlock: Advanced alloy creation

Gameplay Impact:
- Miner-Blacksmiths are premier metalworkers
- Direct ore-to-item pipeline
- Superior material quality control
- Most economically viable for weapon/armor creation
```

**4. Alchemy + Cooking (+65)**

```
Synergy Benefits:
├─ Shared understanding of mixtures and reactions
├─ Cross-application of techniques
├─ Combined: +15% buff duration on food
├─ Combined: +12% potion palatability
└─ Unlock: Gastronomy specialization

Gameplay Impact:
- Creates unique "Culinary Alchemist" role
- Gourmet foods with magical effects
- Medicinal meals for healing
- Premium market position
```

### Conflict Systems

Some skill combinations create conflicts or diminishing returns:

**Resource Competition Conflicts:**

```
Mining vs. Herbalism (-10)
Reason: Time-based conflict
├─ Both require field time for gathering
├─ Different terrain preferences
├─ Cannot gather both simultaneously
└─ Skill point investment trade-off

Impact:
- Players must choose gathering focus
- Encourages specialization or trading
- Creates natural player roles
```

**Philosophical Conflicts (if Alignment System exists):**

```
Combat vs. Crafting
Alignment-Based Penalty: Up to -30% effectiveness

Pure Crafter (+100 Crafting Alignment):
├─ +20% crafting speed and quality
└─ -30% combat effectiveness

Pure Warrior (-100 Combat Alignment):
├─ +20% combat damage and defense
└─ -30% crafting speed and quality

Design Purpose:
- Forces meaningful character choices
- Creates player interdependence
- Establishes distinct character identities
- Drives player economy and trading
```

**Skill Cap Conflicts:**

```
Total Skill Points: 1000 (example system)

High Overlap Skills Create Natural Conflicts:
- Weaving (100) + Tailoring (100) = 200 points
- Alternative: Tailoring (100) alone = 100 points
- Opportunity Cost: 100 points for marginal gain

Decision Framework:
Should I invest in both Weaving and Tailoring?
├─ Yes, if: Value self-sufficiency, enjoy full chain
├─ No, if: Want to master other skills, prefer trading
└─ Partial: Tailor (100) + Basic Weaving (30) for understanding
```

### Compatibility Scoring Formula

```
Compatibility Score = 
    (Synergy_Bonus × 0.4) + 
    (Resource_Sharing × 0.3) + 
    (Knowledge_Transfer × 0.2) + 
    (Market_Complementarity × 0.1) - 
    (Conflict_Penalty)

Where:
- Synergy_Bonus: 0-100 (combined effectiveness increase)
- Resource_Sharing: 0-100 (material and tool compatibility)
- Knowledge_Transfer: 0-100 (learning efficiency bonus)
- Market_Complementarity: 0-100 (economic synergy)
- Conflict_Penalty: 0-100 (negative interactions)

Example: Herbalism + Alchemy
Synergy_Bonus: 90
Resource_Sharing: 85
Knowledge_Transfer: 75
Market_Complementarity: 70
Conflict_Penalty: 0

Score = (90×0.4) + (85×0.3) + (75×0.2) + (70×0.1) - 0
      = 36 + 25.5 + 15 + 7 - 0
      = 83.5 (+80 Exceptional Synergy)
```


## Dependency Networks

### Visual Dependency Network

```
                        ┌─────────────────────┐
                        │   RESOURCE LAYER    │
                        │  (Gathering Skills) │
                        └─────────────────────┘
                                   │
        ┌──────────────────────────┼──────────────────────────┐
        │                          │                          │
        ▼                          ▼                          ▼
   ┌────────┐                ┌──────────┐              ┌───────────┐
   │ Mining │────────────────│ Forestry │──────────────│ Herbalism │
   └────────┘                └──────────┘              └───────────┘
        │                          │                          │
        │                          │                          │
        ▼                          ▼                          ▼
                        ┌─────────────────────┐
                        │  PROCESSING LAYER   │
                        │ (Production Skills) │
                        └─────────────────────┘
                                   │
        ┌──────────────────────────┼──────────────────────────┐
        │                          │                          │
        ▼                          ▼                          ▼
┌───────────────┐          ┌──────────────┐           ┌─────────┐
│ Blacksmithing │          │ Woodworking  │           │ Alchemy │
└───────────────┘          └──────────────┘           └─────────┘
        │                          │                          │
        │                          │                          │
        ▼                          ▼                          ▼
                        ┌─────────────────────┐
                        │   CRAFTING LAYER    │
                        │ (Advanced Skills)   │
                        └─────────────────────┘
                                   │
        ┌──────────────────────────┼──────────────────────────┐
        │                          │                          │
        ▼                          ▼                          ▼
┌───────────────┐          ┌──────────────┐           ┌─────────────┐
│ Weaponsmithing│          │  Carpentry   │           │  Enchanting │
└───────────────┘          └──────────────┘           └─────────────┘
                                   │
                                   │
                                   ▼
                        ┌─────────────────────┐
                        │   MASTERY LAYER     │
                        │ (Specializations)   │
                        └─────────────────────┘
```

### Detailed Dependency Chains

**Chain 1: Metalworking Pipeline**

```
1. Mining (Level 1-50)
   ├─ Output: Ore, raw metals
   ├─ Time: 50-100 hours to reach 50
   └─ Unlocks: Blacksmithing at Level 1

2. Blacksmithing (Level 1-70)
   ├─ Requires: Mining 1+
   ├─ Output: Basic weapons, tools, armor
   ├─ Time: 100-200 hours to reach 70
   └─ Unlocks: Specializations at Level 50

3. Weaponsmithing (Level 50+)
   ├─ Requires: Blacksmithing 50+, Mining 30+
   ├─ Output: Superior weapons
   └─ Bonus: +15% weapon quality

4. Armorsmithing (Level 50+)
   ├─ Requires: Blacksmithing 50+, Mining 30+
   ├─ Output: Superior armor
   └─ Bonus: +15% armor protection

Alternative Branch:
3. Jewelry Making (Level 40+)
   ├─ Requires: Blacksmithing 40+, Mining 25+
   ├─ Output: Rings, amulets, decorative items
   └─ Bonus: +10% precious metal efficiency
```

**Chain 2: Textile Pipeline**

```
1. Herbalism (Level 1-30)
   ├─ Output: Plant fibers (flax, hemp, cotton)
   ├─ Time: 30-60 hours to reach 30
   └─ Unlocks: Understanding of natural fibers

2. Fiber Processing (Level 1-40)
   ├─ Requires: Herbalism 10+ (soft requirement)
   ├─ Output: Prepared fibers, thread
   ├─ Time: 40-80 hours to reach 40
   └─ Unlocks: Weaving at Level 1

3. Weaving (Level 1-60)
   ├─ Requires: Fiber Processing 1+
   ├─ Output: Cloth bolts, textiles
   ├─ Time: 80-150 hours to reach 60
   └─ Unlocks: Advanced textile work

4. Tailoring (Level 1-100)
   ├─ Requires: Weaving 1+ OR access to cloth
   ├─ Output: Clothing, bags, cloth armor
   ├─ Time: 150-300 hours to reach 100
   └─ Specializations: Armorsmith, Fashion Designer

Parallel Branch:
3. Leatherworking (Level 1-70)
   ├─ Requires: Hunting/Animal Husbandry for materials
   ├─ Output: Leather goods, medium armor
   └─ Can combine with Tailoring for mixed armor

Integration Point:
5. Master Clothier (Level 80+)
   ├─ Requires: Tailoring 80+, Weaving 50+, Leatherworking 40+
   ├─ Output: Masterwork garments, hybrid armor
   └─ Bonus: +25% quality on combined-material items
```

**Chain 3: Botanical/Chemical Pipeline**

```
1. Herbalism (Level 1-50)
   ├─ Output: Medicinal herbs, reagents
   ├─ Time: 50-100 hours to reach 50
   └─ Unlocks: Alchemy at Level 1

2. Alchemy (Level 1-80)
   ├─ Requires: Herbalism 1+
   ├─ Output: Potions, elixirs, poisons
   ├─ Time: 100-200 hours to reach 80
   └─ Unlocks: Advanced specializations

3. Advanced Alchemy (Level 50+)
   ├─ Requires: Alchemy 50+, Herbalism 40+
   ├─ Branches:
   │   ├─ Potioncraft: Buff and healing potions
   │   ├─ Toxicology: Poisons and antidotes
   │   └─ Transmutation: Material conversion
   └─ Bonus: +20% potion potency

Parallel Integration:
2b. Cooking (Level 1-60)
    ├─ Requires: Herbalism 10+ (soft requirement)
    ├─ Output: Food, beverages
    └─ Cross-bonus with Alchemy

4. Gastronomy (Level 60+)
   ├─ Requires: Alchemy 60+, Cooking 60+
   ├─ Output: Gourmet dishes with alchemical effects
   └─ Bonus: +30% food buff duration
```

### Dependency Types

**Hard Dependencies (Required):**

```
Cannot practice Child without Parent:
- Blacksmithing requires Mining 1+
- Weaponsmithing requires Blacksmithing 50+
- Enchanting requires Alchemy 40+

System Implementation:
IF player.skill(Mining) < 1 THEN
    block_skill_gain(Blacksmithing)
    show_message("You need Mining skill to understand metalworking")
END IF
```

**Soft Dependencies (Recommended):**

```
Can practice without prerequisite but less efficient:
- Alchemy recommended Herbalism 10+ (can buy herbs instead)
- Tailoring recommended Weaving 1+ (can buy cloth instead)
- Cooking recommended Herbalism 5+ (can buy ingredients)

System Implementation:
IF player.skill(Herbalism) >= 10 THEN
    alchemy_efficiency_bonus = 1.15
ELSE
    alchemy_efficiency_bonus = 1.0
    show_tooltip("Learning Herbalism would improve your Alchemy efficiency")
END IF
```

**Knowledge Dependencies (Informational):**

```
Skills that inform understanding but don't block:
- Combat knowledge improves Weaponsmithing quality
- Architecture knowledge improves Carpentry structures
- Survival knowledge improves Herbalism gathering

System Implementation:
weapon_quality_bonus = base_quality + (combat_skill × 0.05)
```

### Circular Dependencies

Some skills have bidirectional relationships:

```
Mining ←→ Blacksmithing
├─ Mining provides ore for blacksmithing
├─ Blacksmithing creates better mining tools
└─ Circular benefit: Each skill improves the other

Tool Quality Improvement Loop:
1. Mine with basic tools (100% time)
2. Smith better tools using mined ore
3. Mine with better tools (80% time, +10% yield)
4. Smith even better tools
5. Continue cycle

Formula:
tool_efficiency = base_efficiency × (1 + (tool_quality × 0.001))
tool_quality = crafter_skill × material_quality × 0.01

Max Benefit: ~30% efficiency gain with master tools
```


## Synergy and Conflict Models

### Synergy Calculation System

```
Combined Skill Effectiveness = Individual Effectiveness + Synergy Bonus

Synergy Bonus Formula:
Bonus = (Skill₁ × Skill₂ × Synergy_Coefficient) / 1000

Where Synergy_Coefficient depends on compatibility:
- Exceptional Synergy (80-100): Coefficient = 5.0
- High Synergy (60-79): Coefficient = 3.5
- Good Synergy (40-59): Coefficient = 2.0
- Moderate Synergy (20-39): Coefficient = 1.0
- Minor Synergy (1-19): Coefficient = 0.5

Example: Herbalism (70) + Alchemy (65)
Compatibility: +80 (Exceptional Synergy)
Synergy_Coefficient: 5.0
Bonus = (70 × 65 × 5.0) / 1000 = 22.75

Effective Alchemy Skill when gathering herbs: 65 + 22.75 = 87.75
```

### Synergy Types

**1. Efficiency Synergies**

```
Skills that reduce time/resource costs:

Mining + Blacksmithing:
├─ Mining knowledge reduces ore waste in forging
├─ Formula: waste_reduction = mining_skill × 0.001
├─ At Mining 70: 7% less ore waste
└─ Saves materials and time

Herbalism + Alchemy:
├─ Herb knowledge improves extraction efficiency
├─ Formula: potion_yield = base_yield × (1 + herbalism_skill × 0.002)
├─ At Herbalism 80: +16% more potions per herb
└─ Economic advantage

Forestry + Woodworking:
├─ Wood selection reduces material waste
├─ Formula: lumber_efficiency = 1 + (forestry_skill × 0.0015)
├─ At Forestry 60: +9% more usable lumber
└─ Better project planning
```

**2. Quality Synergies**

```
Skills that improve output quality:

Combat + Weaponsmithing:
├─ Combat experience informs weapon design
├─ Formula: weapon_quality += combat_skill × 0.003
├─ At Combat 80: +24% weapon effectiveness
└─ Weapons feel better to use

Navigation + Cartography:
├─ Travel experience improves map accuracy
├─ Formula: map_quality = base + (navigation_skill × 0.005)
├─ At Navigation 70: +35% map detail
└─ More valuable maps

Animal Husbandry + Leatherworking:
├─ Animal care improves hide quality
├─ Formula: leather_quality = hide_base × (1 + husbandry_skill × 0.002)
├─ At Husbandry 75: +15% leather quality
└─ Premium leather goods
```

**3. Unlock Synergies**

```
Skill combinations that unlock new abilities:

Alchemy (60+) + Cooking (60+) = Gastronomy
├─ Unlocks: Gourmet food with buff effects
├─ Unlocks: Medicinal meals
├─ Unlocks: Long-lasting feast preparations
└─ New market niche

Blacksmithing (70+) + Woodworking (50+) + Leatherworking (40+) = Master Craftsman
├─ Unlocks: Complex multi-material items
├─ Unlocks: Masterwork furniture
├─ Unlocks: Composite weapons (wood+metal+leather)
└─ Premium crafting tier

Mining (80+) + Blacksmithing (80+) + Alchemy (60+) = Advanced Metallurgy
├─ Unlocks: Custom alloy creation
├─ Unlocks: Magical metal infusion
├─ Unlocks: Experimental materials
└─ Cutting-edge innovation
```

### Conflict Models

**1. Time Conflicts**

```
Skills that compete for active practice time:

Gathering Skills Conflict:
├─ Mining vs. Herbalism vs. Forestry vs. Fishing
├─ All require field time
├─ Cannot practice simultaneously
└─ Forces specialization or time management

Penalty Model:
Active_Skill_Gain = base_XP
Inactive_Skill_Decay = active_time × decay_rate

If player spends 80% time mining:
- Mining: Normal progression
- Herbalism: 0.5% weekly decay (from disuse)
- Forestry: 0.5% weekly decay
```

**2. Resource Conflicts**

```
Skills that compete for scarce materials:

Blacksmithing vs. Jewelry Making (compete for precious metals):
├─ Both need gold, silver, platinum
├─ Limited supply in world
├─ Market price competition
└─ Strategic material allocation

Tailoring vs. Alchemy (compete for plant materials):
├─ Both use certain plant fibers/dyes
├─ Flax for linen vs. flax oil for potions
├─ Cotton for cloth vs. cotton in reagents
└─ Harvest decision points
```

**3. Skill Point Conflicts**

```
With limited skill points (e.g., 1000 total cap):

High Overlap = High Opportunity Cost:
- Weaving (100) + Tailoring (100) = 200 points
- Overlapping skills = redundant investment
- Better: Tailoring (100) + Leatherworking (80) + Woodworking (60)
- Result: Diverse product line, better skill point efficiency

Optimal Allocation Model:
Diminishing_Returns = points_invested² × overlap_coefficient

For 75%+ overlap skills:
DR_Coefficient = 0.001
Cost = 100 + (100² × 0.001) = 110 effective points for second skill

For 25-50% overlap skills:
DR_Coefficient = 0.0003
Cost = 100 + (100² × 0.0003) = 103 effective points
```

**4. Alignment Conflicts**

```
If alignment system exists (Crafting vs. Combat):

Crafting Alignment: -100 (Combat) to +100 (Crafting)

Pure Crafter (+100):
├─ Crafting Skills: +25% speed, +20% quality
├─ Combat Skills: -35% effectiveness
└─ Cannot master combat skills above 60

Balanced (0):
├─ No bonuses or penalties
└─ Jack of all trades, master of none

Pure Warrior (-100):
├─ Combat Skills: +30% damage, +25% survivability
├─ Crafting Skills: -35% speed and quality
└─ Cannot master crafting skills above 60

Shift Mechanic:
- Alignment shifts 1 point per hour of skill practice
- Crafting practice: +1 toward Crafting alignment
- Combat practice: -1 toward Combat alignment
- Takes 100 hours to shift from neutral to extreme
```

### Conflict Resolution Strategies

**For Players:**

```
Strategy 1: Specialization
- Focus on one skill branch deeply
- Trade with others for needed goods
- Maximum efficiency in chosen area

Strategy 2: Complementary Skills
- Choose skills with high synergy, low overlap
- Example: Blacksmithing + Woodworking (tools)
- Example: Alchemy + Cooking (buffs)

Strategy 3: Vertical Integration
- Master full production chain
- Example: Mining → Blacksmithing → Weaponsmithing
- Self-sufficient but narrow product range

Strategy 4: Horizontal Diversification
- Multiple unrelated skills
- Example: Tailoring + Cooking + Herbalism
- Broad capability, multiple market positions
```

**For Game Designers:**

```
Balancing Principles:
1. High overlap skills should merge or become hierarchical
2. Complementary skills should have clear synergy bonuses
3. Resource conflicts create meaningful player choices
4. Time conflicts encourage specialization and trading
5. Skill caps force strategic character building
```


## Network Representations

### Graph-Based Skill Network

Skills can be modeled as nodes in a directed graph with weighted edges:

```
Graph Structure:
- Nodes: Individual skills
- Edges: Relationships between skills
- Edge Weights: Strength of relationship (-100 to +100)
- Edge Direction: Dependency flow (if applicable)

Example Graph Notation (DOT format):

digraph SkillNetwork {
    // Gathering Layer
    Mining [layer=1]
    Herbalism [layer=1]
    Forestry [layer=1]
    
    // Processing Layer
    Blacksmithing [layer=2]
    Alchemy [layer=2]
    Woodworking [layer=2]
    
    // Edges with weights
    Mining -> Blacksmithing [weight=85, label="+85"]
    Herbalism -> Alchemy [weight=80, label="+80"]
    Forestry -> Woodworking [weight=75, label="+75"]
    
    // Synergy edges (bidirectional)
    Alchemy -> Cooking [weight=65, label="+65", dir=both]
    Blacksmithing -> Woodworking [weight=40, label="+40", dir=both]
}
```

### Skill Network Visualization

```
                              SKILL NETWORK MAP
                              
    Layer 1: Resource Gathering
    ════════════════════════════
    
    [Mining]────────────[Herbalism]────────────[Forestry]
       │                     │                      │
       │+85                  │+80                   │+75
       │                     │                      │
    Layer 2: Processing
    ═══════════════════
       │                     │                      │
       ▼                     ▼                      ▼
    [Blacksmithing]──+40──[Alchemy]──+65──[Cooking]  [Woodworking]
       │                     │                      │
       │+70                  │+55                   │+65
       │                     │                      │
    Layer 3: Crafting & Specialization
    ══════════════════════════════════
       │                     │                      │
       ▼                     ▼                      ▼
    [Weaponsmithing]    [Gastronomy]          [Carpentry]
    [Armorsmithing]     [Medicine]            [Furniture]
    
    
    Legend:
    ───── Direct Dependency (hierarchical)
    ─+##─ Synergy Relationship (bidirectional with strength)
    [Skill] Independent Skill Node
```

### Adjacency Matrix Representation

```
Skill Adjacency Matrix (showing relationship strengths):

             Mining  Black  Herb  Alch  Cook  Forest  Wood  Tail  Weave
Mining         0     85     -10    15    5     20     10    -5     2
Blacksmith    20      0      5    15    25     30     40    10     5
Herbalism     -5      5      0    80    55     50     25    35    20
Alchemy        8     15     45     0    65     45     20    20    10
Cooking        5     20     40    65     0     25     30    10     5
Forestry      15     25     35    40    20      0     75    15    12
Woodworking   10     35     20    18    25     75      0    15    10
Tailoring      2     10     30    18     8     12     15     0    35
Weaving        2      8     18    10     5     15     12    35     0

Interpretation:
- Diagonal (0): Self-relationship
- Positive values: Synergy strength
- Negative values: Conflict strength
- Asymmetric: Different benefits in each direction
```

### Tree-Based Hierarchy

```
SKILL TREE STRUCTURE

Root: Artisan (All Crafting Skills)
│
├─── Material Gathering
│    ├─── Mining
│    │    └─── Prospecting (sub-skill)
│    ├─── Herbalism
│    │    ├─── Plant Identification
│    │    └─── Harvesting Techniques
│    └─── Forestry
│         ├─── Tree Identification
│         └─── Sustainable Logging
│
├─── Material Processing
│    ├─── Smelting (requires Mining)
│    │    └─── Alloy Creation
│    ├─── Fiber Processing (requires Herbalism)
│    │    ├─── Spinning
│    │    └─── Dyeing
│    └─── Lumber Processing (requires Forestry)
│         ├─── Sawing
│         └─── Curing
│
├─── Crafting
│    ├─── Blacksmithing (requires Smelting)
│    │    ├─── Weaponsmithing
│    │    ├─── Armorsmithing
│    │    └─── Toolmaking
│    ├─── Weaving (requires Fiber Processing)
│    │    └─── Tapestry
│    ├─── Tailoring (requires Weaving)
│    │    ├─── Garment Making
│    │    └─── Armor Padding
│    └─── Woodworking (requires Lumber Processing)
│         ├─── Carpentry
│         ├─── Furniture Making
│         └─── Bowyer
│
└─── Advanced Crafting
     ├─── Engineering (requires multiple crafting skills)
     ├─── Masterwork Creation (requires mastery in any craft)
     └─── Innovation (requires experimentation in multiple skills)
```

### Cluster Analysis

Skills naturally form clusters based on similarity and interaction:

```
Cluster 1: Metalworking Cluster
    Core: Blacksmithing
    Members: Mining, Smelting, Weaponsmithing, Armorsmithing, Jewelry
    Cohesion: 0.85 (very tight cluster)
    
Cluster 2: Textile Cluster
    Core: Tailoring
    Members: Herbalism(partial), Weaving, Fiber Processing, Leatherworking
    Cohesion: 0.72 (strong cluster)
    
Cluster 3: Botanical/Chemical Cluster
    Core: Alchemy
    Members: Herbalism, Cooking, Medicine, Toxicology
    Cohesion: 0.68 (strong cluster)
    
Cluster 4: Woodworking Cluster
    Core: Woodworking
    Members: Forestry, Carpentry, Furniture Making, Bowyer
    Cohesion: 0.75 (strong cluster)
    
Cluster 5: Survival Cluster
    Core: Survival
    Members: Hunting, Fishing, Cooking, Herbalism, First Aid
    Cohesion: 0.60 (moderate cluster)

Inter-Cluster Bridges (skills connecting clusters):
- Cooking: Links Botanical/Chemical ↔ Survival
- Herbalism: Links Botanical/Chemical ↔ Textile
- Leatherworking: Links Survival ↔ Textile
- Toolmaking: Links Metalworking ↔ Woodworking
```

### Pathfinding in Skill Networks

Players can navigate skill networks to find optimal progression paths:

```
Problem: Player wants to become a Master Armorer
Current Skills: None
Goal: Armorsmithing 100

Pathfinding Algorithm (Dijkstra-like):
1. Identify target skill: Armorsmithing
2. Trace back dependencies: Armorsmithing ← Blacksmithing ← Mining
3. Calculate optimal path considering synergies

Path 1 (Minimum Requirements):
Mining (30) → Blacksmithing (50) → Armorsmithing (100)
Total Investment: 180 skill points
Time: ~280 hours

Path 2 (Synergy Optimized):
Mining (60) → Blacksmithing (70) → Armorsmithing (100)
Total Investment: 230 skill points
Time: ~350 hours
Benefits: +15% efficiency from higher parent skills

Path 3 (Multi-Skill Support):
Mining (50) → Blacksmithing (60) → Woodworking (40) → Armorsmithing (100)
Total Investment: 250 skill points
Time: ~380 hours
Benefits: Can craft composite armor, +20% versatility

Recommendation Engine:
def recommend_path(target_skill, player_skills, play_style):
    if play_style == "efficient":
        return minimum_requirement_path()
    elif play_style == "optimal":
        return synergy_optimized_path()
    elif play_style == "versatile":
        return multi_skill_path()
```


## Case Study: Fiber Crafting System

### Context: Fiber (Leaves) Crafting UI

The Fiber (Leaves) crafting system, referenced in the [Crafting Interface Mockups](assets/crafting-interface-mockups.md),
exemplifies effective skill relationship visualization and material quality integration.

### Skill Relationships in Fiber Crafting

```
Complete Fiber-to-Garment Pipeline:

1. Herbalism (Gathering)
   ├─ Harvest: Flax, Hemp, Cotton, Nettle
   ├─ Quality Factors: Plant age, climate, soil
   ├─ Skill Impact: +1% yield per 10 levels
   └─ Output: Raw plant fibers

        ↓ (material flow)

2. Fiber Processing (Material Prep)
   ├─ Activities: Retting, scutching, hackling
   ├─ Quality Factors: Processing technique, time
   ├─ Skill Impact: +2% fiber quality per 10 levels
   └─ Output: Prepared fibers, thread

        ↓ (material flow)

3. Weaving (Production)
   ├─ Activities: Thread-to-cloth conversion
   ├─ Quality Factors: Tension, pattern, consistency
   ├─ Skill Impact: +1.5% cloth quality per 10 levels
   └─ Output: Cloth bolts, specific qualities

        ↓ (material flow)

4. Tailoring (Assembly)
   ├─ Activities: Pattern, cutting, sewing
   ├─ Quality Factors: Fit, stitching, design
   ├─ Skill Impact: +2.5% garment quality per 10 levels
   └─ Output: Fiber Clothing (final product)
```

### UI Integration of Skills

The crafting interface shows skill relationships through:

**1. Material Quality Visualization**

```
From Crafting Interface Mockups:

╔══════════════════════════════════════════════════════════╗
║ FIBER MATERIALS (Select Quality)                        ║
║                                                          ║
║ [🌿] Flax Fiber × 6                                     ║
║                                                          ║
║      ┌─────────────────────────────────────┐            ║
║      │ ⭐ Premium Flax (88%) ██████████░    │            ║
║      │   Source: Aged plants, perfect soil  │            ║
║      │   Effect: +15% quality, +10% durable │            ║
║      │   Requires: Herbalism 40+            │ ← Skill   ║
║      │                                       │   Gate    ║
║      │ ○ Standard Flax (65%) ███████░░░     │            ║
║      │   Source: Common harvest              │            ║
║      │   Effect: Normal quality              │            ║
║      │   Requires: Herbalism 10+            │            ║
║      │                                       │            ║
║      │ ○ Poor Flax (42%) ████░░░░░░         │            ║
║      │   Source: Young/damaged plants       │            ║
║      │   Effect: -8% quality, -12% durable  │            ║
║      │   Requires: Herbalism 1+             │            ║
║      └─────────────────────────────────────┘            ║
╚══════════════════════════════════════════════════════════╝

Key Features:
- Material quality directly linked to gathering skill level
- Visual quality bars show material grade
- Tooltip shows skill requirements
- Clear cause-and-effect relationship
```

**2. Cross-Skill Bonus Display**

```
╔══════════════════════════════════════════════════════════╗
║ CRAFTING SUMMARY                                        ║
║ ────────────────────────────────────────────────        ║
║ Base Success Rate:        70%                           ║
║ + Your Tailoring (35):  +13%                           ║
║ + Your Weaving (25):     +8%  ← Cross-skill bonus     ║
║ + Your Herbalism (18):   +3%  ← Material knowledge    ║
║ + Material Quality:     +15%  ← Premium flax           ║
║ ────────────────────────────────────────────────        ║
║ Final Success Rate:      109% (100% cap, +9% quality) ║
╚══════════════════════════════════════════════════════════╝

Demonstrates:
- Multiple skills contribute to final outcome
- Synergy bonuses are visible and quantified
- Overflow success converts to quality bonus
- Encourages player to develop related skills
```

**3. Skill Progression Feedback**

```
After Crafting:

╔══════════════════════════════════════════════════════════╗
║ ✓ CRAFTING COMPLETE                                     ║
║                                                          ║
║ Created: Fine Fiber Tunic (Quality: 83%)                ║
║                                                          ║
║ Skill Experience Gained:                                ║
║ ├─ Tailoring: +75 XP (Primary skill)                   ║
║ ├─ Weaving: +15 XP (Related skill bonus)               ║
║ └─ Herbalism: +5 XP (Material knowledge)               ║
║                                                          ║
║ Skill Relationship Unlocked!                            ║
║ "Textile Mastery" - Weaving and Tailoring work          ║
║ together more efficiently (+5% quality bonus)           ║
╚══════════════════════════════════════════════════════════╝

Features:
- Primary skill gets full XP
- Related skills get partial XP (cross-training)
- Relationship milestones unlock bonuses
- Encourages skill synergy development
```

### Design Lessons from Fiber Crafting

**1. Transparent Relationships**

```
Players can see how skills interact:
✓ Clear prerequisite chains (Herbalism → Weaving → Tailoring)
✓ Visible bonus calculations
✓ Material quality linked to source skills
✓ Cross-skill XP gains reward related skill development
```

**2. Meaningful Choices**

```
Players face strategic decisions:
- Specialize in Tailoring (100) and buy materials?
- Balance Tailoring (60) + Weaving (50) + Herbalism (40)?
- Master entire chain for quality control?

Each choice has trade-offs:
├─ Specialization: Highest output in one skill, dependent on economy
├─ Balance: Good all-around, moderate quality
└─ Vertical Integration: Best quality, highest time investment
```

**3. Progressive Complexity**

```
Beginner Experience:
- Simple interface, few choices
- Basic materials available
- Clear success/failure
- "You need better materials" feedback

Intermediate Experience:
- More material options appear
- Quality differences become visible
- Cross-skill bonuses show up
- "Herbalism helps your Tailoring" discovery

Advanced Experience:
- Premium materials require skill gates
- Multiple synergy bonuses stack
- Optimization becomes engaging
- "Min-maxing the perfect outfit" gameplay
```

### Implementation Recommendations

Based on Fiber Crafting system success:

**1. Material-Skill Linking**

```sql
-- Database schema for material quality gating
CREATE TABLE materials (
    material_id INT PRIMARY KEY,
    name VARCHAR(100),
    base_quality INT,
    required_gathering_skill VARCHAR(50),
    required_skill_level INT,
    quality_bonus_per_skill_level DECIMAL(4,2)
);

-- Example data
INSERT INTO materials VALUES 
(1, 'Premium Flax', 88, 'Herbalism', 40, 0.5),
(2, 'Standard Flax', 65, 'Herbalism', 10, 0.3),
(3, 'Poor Flax', 42, 'Herbalism', 1, 0.1);

-- Quality calculation
effective_quality = base_quality + 
    (player_skill_level - required_skill_level) * quality_bonus_per_skill_level
```

**2. Cross-Skill Bonus System**

```python
def calculate_crafting_success(player, recipe, materials):
    base_rate = recipe.base_success_rate
    
    # Primary skill bonus
    primary_bonus = player.get_skill(recipe.primary_skill) * recipe.skill_coefficient
    
    # Cross-skill bonuses
    cross_bonuses = 0
    for related_skill in recipe.related_skills:
        skill_level = player.get_skill(related_skill.name)
        cross_bonuses += skill_level * related_skill.bonus_coefficient
    
    # Material bonuses
    material_bonus = sum(m.quality_bonus for m in materials)
    
    # Total calculation
    total_rate = base_rate + primary_bonus + cross_bonuses + material_bonus
    
    # Cap at 100%, overflow becomes quality bonus
    if total_rate > 100:
        quality_bonus = total_rate - 100
        success_rate = 100
    else:
        quality_bonus = 0
        success_rate = total_rate
    
    return success_rate, quality_bonus
```

**3. Skill Relationship Discovery**

```python
class SkillRelationshipSystem:
    def __init__(self):
        self.relationships = {}
        self.unlocked_bonuses = {}
    
    def check_unlock(self, player, skill_a, skill_b):
        # Check if player has used both skills together
        combined_uses = player.get_combined_skill_uses(skill_a, skill_b)
        
        # Unlock thresholds
        if combined_uses >= 10 and not self.is_unlocked(player, skill_a, skill_b):
            self.unlock_synergy(player, skill_a, skill_b, tier=1)
        elif combined_uses >= 50:
            self.unlock_synergy(player, skill_a, skill_b, tier=2)
        elif combined_uses >= 200:
            self.unlock_synergy(player, skill_a, skill_b, tier=3)
    
    def get_synergy_bonus(self, player, skill_a, skill_b):
        tier = self.get_unlock_tier(player, skill_a, skill_b)
        return {
            1: 0.05,  # +5% bonus
            2: 0.10,  # +10% bonus
            3: 0.15,  # +15% bonus
        }.get(tier, 0)
```


## Implementation Recommendations

### For BlueMarble Skill System

Based on the research findings, the following recommendations will create a robust, engaging skill system:

### 1. Skill Structure Recommendations

**Consolidate High-Overlap Skills:**

```
Current Potential Issues:
- Blacksmithing vs. Basic Metalworking (85% overlap)
- Weaving vs. Textile Production (80% overlap)
- Fishing vs. Angling (90% overlap)

Recommendations:
✓ Merge Blacksmithing + Basic Metalworking → "Blacksmithing" (unified skill)
✓ Keep Weaving separate but make it Tier 2 parent of Tailoring
✓ Merge Fishing + Angling → "Fishing" (single skill)
✗ Don't create redundant parallel skills

Result: Cleaner skill list, less player confusion, same depth
```

**Implement Hierarchical Skill Trees:**

```
Recommended Structure for BlueMarble:

Tier 1: Gathering Skills (Foundation)
├─ Mining (1-50)
├─ Herbalism (1-50)
├─ Forestry (1-50)
├─ Hunting (1-50)
└─ Fishing (1-50)

Tier 2: Processing Skills (Intermediate)
├─ Blacksmithing (requires Mining 1+)
├─ Alchemy (requires Herbalism 1+)
├─ Woodworking (requires Forestry 1+)
├─ Cooking (requires Hunting/Fishing 1+)
└─ Weaving (requires Herbalism 10+)

Tier 3: Crafting Skills (Advanced)
├─ Weaponsmithing (requires Blacksmithing 50+)
├─ Armorsmithing (requires Blacksmithing 50+)
├─ Tailoring (requires Weaving 30+)
├─ Leatherworking (requires Hunting 30+)
└─ Carpentry (requires Woodworking 40+)

Tier 4: Specializations (Mastery)
├─ Master Craftsman (requires 3+ Tier 3 skills at 70+)
├─ Gastronomy (requires Alchemy 60+ and Cooking 60+)
└─ Engineering (requires any 2 crafting skills at 80+)
```

### 2. Cross-Skill Bonus Implementation

**Bonus Calculation System:**

```python
# Configuration: Define all skill relationships
SKILL_SYNERGIES = {
    ('Herbalism', 'Alchemy'): {
        'coefficient': 0.002,
        'max_bonus': 20,
        'description': 'Herb knowledge improves potion creation'
    },
    ('Mining', 'Blacksmithing'): {
        'coefficient': 0.0015,
        'max_bonus': 15,
        'description': 'Ore knowledge reduces metal waste'
    },
    ('Forestry', 'Woodworking'): {
        'coefficient': 0.0018,
        'max_bonus': 18,
        'description': 'Wood identification improves quality'
    },
    ('Alchemy', 'Cooking'): {
        'coefficient': 0.0012,
        'max_bonus': 12,
        'description': 'Chemical understanding enhances recipes'
    },
    ('Combat', 'Weaponsmithing'): {
        'coefficient': 0.0008,
        'max_bonus': 8,
        'description': 'Combat experience informs weapon design'
    }
}

def get_synergy_bonus(skill_a_level, skill_b_level, skill_pair):
    if skill_pair not in SKILL_SYNERGIES:
        return 0
    
    config = SKILL_SYNERGIES[skill_pair]
    bonus = skill_a_level * skill_b_level * config['coefficient']
    return min(bonus, config['max_bonus'])

# Usage example
herbalism_level = 70
alchemy_level = 65
bonus = get_synergy_bonus(herbalism_level, alchemy_level, ('Herbalism', 'Alchemy'))
# Returns: min(70 * 65 * 0.002, 20) = min(9.1, 20) = 9.1% bonus
```

### 3. Skill Relationship Visualization

**In-Game Skill Tree Display:**

```
╔════════════════════════════════════════════════════════════════╗
║                     YOUR SKILL TREE                            ║
╠════════════════════════════════════════════════════════════════╣
║                                                                ║
║  Mining (Level 45) ─────────────────────┐                    ║
║  ██████████████████░░░░░░░░░░░ 45/100   │                    ║
║                                          │                    ║
║                     +12% bonus to ───────┤                    ║
║                                          │                    ║
║  Blacksmithing (Level 38) ◄──────────────┘                    ║
║  ██████████████░░░░░░░░░░░░░░ 38/100                          ║
║  Effective: 50 (Base 38 + Mining Bonus 12)                    ║
║                                                                ║
║  ┌─────────────────────────────────────────┐                  ║
║  │ 🔒 Weaponsmithing                       │                  ║
║  │    Requires: Blacksmithing 50           │                  ║
║  │    Progress: 38/50 (76%)                │                  ║
║  │    ETA: ~20 more crafts                 │                  ║
║  └─────────────────────────────────────────┘                  ║
║                                                                ║
║  Synergies Discovered:                                         ║
║  ✓ Mining ↔ Blacksmithing: +12% efficiency                   ║
║  ○ Blacksmithing ↔ Woodworking: Not yet discovered           ║
║    (Craft 5 items using both skills to unlock)                ║
║                                                                ║
╚════════════════════════════════════════════════════════════════╝
```

### 4. Material Quality Integration

**Link Material Quality to Source Skills:**

```sql
-- Material quality affected by gathering skill
CREATE TABLE material_instances (
    id INT PRIMARY KEY AUTO_INCREMENT,
    material_type_id INT,
    quality_percentage INT,
    gathered_by_player_id INT,
    gatherer_skill_level INT,
    gathering_timestamp TIMESTAMP,
    
    -- Quality calculation
    GENERATED ALWAYS AS (
        base_quality + 
        (gatherer_skill_level * quality_per_skill_point) +
        environmental_bonus
    ) STORED
);

-- Material processing affects quality
CREATE TABLE processed_materials (
    id INT PRIMARY KEY AUTO_INCREMENT,
    source_material_id INT,
    processor_skill_level INT,
    processing_quality_retained DECIMAL(4,2),
    
    -- Quality retention based on skill
    -- Low skill: loses 10-20% quality
    -- High skill: loses 0-5% quality
    CONSTRAINT quality_retention CHECK (
        processing_quality_retained = 
        1.0 - (0.20 - (processor_skill_level * 0.002))
    )
);
```

### 5. Skill Cap System

**Implement Total Skill Cap with Smart Allocation:**

```python
class SkillCapSystem:
    TOTAL_SKILL_CAP = 1000  # Total points across all skills
    MAX_SKILL_LEVEL = 100   # Maximum single skill level
    
    def __init__(self, player):
        self.player = player
        self.skills = {}
    
    def get_available_points(self):
        used_points = sum(self.skills.values())
        return self.TOTAL_SKILL_CAP - used_points
    
    def can_increase_skill(self, skill_name):
        current_level = self.skills.get(skill_name, 0)
        
        # Check individual cap
        if current_level >= self.MAX_SKILL_LEVEL:
            return False, "Skill already at maximum"
        
        # Check total cap
        if self.get_available_points() <= 0:
            return False, "Total skill cap reached. Forget a skill to continue."
        
        return True, "Can increase"
    
    def suggest_allocation(self, playstyle):
        """Suggest skill point allocation based on playstyle"""
        suggestions = {
            'armorer': {
                'Mining': 60,
                'Blacksmithing': 100,
                'Weaponsmithing': 80,
                'Armorsmithing': 80,
                'Woodworking': 40,
                # Remaining 640 points for other skills
            },
            'alchemist': {
                'Herbalism': 80,
                'Alchemy': 100,
                'Cooking': 60,
                'Survival': 40,
                # Remaining 720 points
            },
            'trader': {
                'Trading': 100,
                'Speechcraft': 80,
                'Cooking': 50,
                'Tailoring': 50,
                'Blacksmithing': 50,
                # Remaining 670 points - diverse for market
            }
        }
        return suggestions.get(playstyle, {})
```

### 6. Progression Balancing

**Prevent Grinding, Encourage Synergies:**

```python
def calculate_xp_gain(action_difficulty, player_skill_level, related_skills):
    # Base XP from action
    base_xp = action_difficulty * 10
    
    # Diminishing returns at high skill
    skill_multiplier = 1.0 / (1 + (player_skill_level / 100))
    
    # Bonus from related skills
    synergy_bonus = 0
    for related_skill, bonus_rate in related_skills.items():
        synergy_bonus += related_skill.level * bonus_rate
    
    # Challenge bonus (attempting difficult tasks)
    challenge_bonus = max(0, action_difficulty - player_skill_level) * 0.5
    
    total_xp = (base_xp * skill_multiplier) + synergy_bonus + challenge_bonus
    
    return max(1, total_xp)  # Minimum 1 XP

# Example usage
xp = calculate_xp_gain(
    action_difficulty=60,
    player_skill_level=55,
    related_skills={
        'Mining': (70, 0.05),  # Mining 70 gives 3.5 bonus
        'Smelting': (40, 0.03)  # Smelting 40 gives 1.2 bonus
    }
)
```

### 7. Player Guidance System

**Help Players Understand Relationships:**

```python
class SkillAdvisor:
    """Provides contextual advice about skill relationships"""
    
    def suggest_next_skill(self, player):
        current_skills = player.get_skills()
        suggestions = []
        
        # Find skills with good synergy to current skills
        for skill_name in ALL_SKILLS:
            if skill_name in current_skills:
                continue
            
            synergy_score = 0
            for current_skill, level in current_skills.items():
                synergy = get_synergy(current_skill, skill_name)
                synergy_score += synergy * level
            
            if synergy_score > 50:
                suggestions.append({
                    'skill': skill_name,
                    'synergy_score': synergy_score,
                    'reason': f"Works well with your {current_skill} skill"
                })
        
        return sorted(suggestions, key=lambda x: x['synergy_score'], reverse=True)[:5]
    
    def explain_relationship(self, skill_a, skill_b):
        """Explain how two skills relate"""
        relationship = get_relationship(skill_a, skill_b)
        
        if relationship['type'] == 'prerequisite':
            return f"{skill_a} is required to learn {skill_b}"
        elif relationship['type'] == 'synergy':
            return f"{skill_a} and {skill_b} work together for +{relationship['bonus']}% effectiveness"
        elif relationship['type'] == 'overlap':
            return f"{skill_a} and {skill_b} overlap {relationship['percentage']}% - consider specializing"
        else:
            return f"{skill_a} and {skill_b} are independent skills"
```


## Appendices

### Appendix A: Complete Skill Relationship Reference

**Quick Reference Matrix for All Core Skills**

```
Skill Pair                        Overlap%  Substitutability%  Synergy  Type
═══════════════════════════════════════════════════════════════════════════════
Mining ↔ Blacksmithing              15%         10%            +85    Prerequisite
Mining ↔ Jewelry Making             12%          8%            +70    Prerequisite
Herbalism ↔ Alchemy                 20%         15%            +80    Prerequisite
Herbalism ↔ Cooking                 18%         12%            +55    Complementary
Herbalism ↔ Weaving                 25%         20%            +40    Material Supply
Forestry ↔ Woodworking              30%         25%            +75    Prerequisite
Forestry ↔ Carpentry                18%         15%            +60    Support
Alchemy ↔ Cooking                   45%         30%            +65    Synergistic
Alchemy ↔ Medicine                  55%         40%            +75    Hierarchical
Blacksmithing ↔ Weaponsmithing      60%         50%            +70    Hierarchical
Blacksmithing ↔ Armorsmithing       60%         50%            +70    Hierarchical
Blacksmithing ↔ Woodworking         15%         10%            +40    Complementary
Woodworking ↔ Carpentry             65%         55%            +75    Hierarchical
Woodworking ↔ Bowyer                50%         40%            +65    Hierarchical
Tailoring ↔ Weaving                 35%         25%            +50    Sequential
Tailoring ↔ Leatherworking          30%         20%            +45    Parallel Craft
Weaving ↔ Fiber Processing          60%         50%            +70    Sequential
Cooking ↔ Survival                  20%         15%            +40    Complementary
Fishing ↔ Cooking                   10%          5%            +35    Material Supply
Hunting ↔ Leatherworking            12%          8%            +45    Material Supply
Combat ↔ Weaponsmithing              5%          2%            +25    Informational
First Aid ↔ Alchemy                 25%         18%            +50    Parallel
Animal Husbandry ↔ Leatherworking   10%          5%            +35    Material Supply
```

### Appendix B: Skill Cluster Definitions

**Full Cluster Analysis:**

```
CLUSTER 1: METALWORKING
    ├─ Core Skills: Blacksmithing, Mining, Smelting
    ├─ Specializations: Weaponsmithing, Armorsmithing, Jewelry
    ├─ Cohesion Score: 0.87
    ├─ Average Internal Synergy: +78
    └─ External Connections: Woodworking (+40), Engineering (+60)

CLUSTER 2: TEXTILES
    ├─ Core Skills: Tailoring, Weaving, Fiber Processing
    ├─ Specializations: Fashion Design, Armor Padding
    ├─ Cohesion Score: 0.72
    ├─ Average Internal Synergy: +65
    └─ External Connections: Herbalism (+40), Leatherworking (+45)

CLUSTER 3: BOTANICAL/CHEMICAL
    ├─ Core Skills: Herbalism, Alchemy, Cooking
    ├─ Specializations: Gastronomy, Medicine, Toxicology
    ├─ Cohesion Score: 0.71
    ├─ Average Internal Synergy: +70
    └─ External Connections: Farming (+50), Survival (+45)

CLUSTER 4: WOODWORKING
    ├─ Core Skills: Forestry, Woodworking, Carpentry
    ├─ Specializations: Furniture Making, Bowyer, Coopering
    ├─ Cohesion Score: 0.78
    ├─ Average Internal Synergy: +72
    └─ External Connections: Blacksmithing (+40), Engineering (+55)

CLUSTER 5: SURVIVAL
    ├─ Core Skills: Survival, Hunting, Fishing, Foraging
    ├─ Specializations: Tracking, Trapping, Field Medicine
    ├─ Cohesion Score: 0.63
    ├─ Average Internal Synergy: +58
    └─ External Connections: Cooking (+45), First Aid (+50)
```

### Appendix C: Progression Time Estimates

**Expected Time to Reach Skill Milestones:**

```
Skill Level    Active Hours    Total Hours    Passive/Offline
═══════════════════════════════════════════════════════════════
10 (Novice)         5-8 hrs       10-15 hrs    2-3 days casual
30 (Apprentice)    20-30 hrs      40-60 hrs    1-2 weeks
50 (Journeyman)    50-80 hrs     100-150 hrs   3-4 weeks
70 (Expert)       100-150 hrs    180-250 hrs   6-8 weeks
90 (Master)       180-250 hrs    300-400 hrs   10-12 weeks
100 (Grandmaster) 250-350 hrs    400-600 hrs   14-18 weeks

With Synergy Bonuses (related skills at 70+):
Level 50:          40-60 hrs      80-120 hrs   2-3 weeks
Level 70:          75-120 hrs    140-200 hrs   4-6 weeks
Level 90:         140-200 hrs    240-320 hrs   8-10 weeks
Level 100:        200-280 hrs    320-480 hrs   12-15 weeks

Note: Times assume optimal practice (crafting at appropriate difficulty)
Casual play may take 50-100% longer
Routine-based offline progression can significantly reduce active time requirement
```

### Appendix D: Economic Implications

**Market Dynamics Based on Skill Relationships:**

```
VERTICAL INTEGRATION vs. SPECIALIZATION

Vertical Integration (Full Chain):
Example: Mining (60) → Blacksmithing (80) → Weaponsmithing (100)
├─ Total Investment: 240 skill points
├─ Time Investment: ~400 hours
├─ Market Position: Self-sufficient weapon creator
├─ Profit Margin: High (no intermediary costs)
├─ Product Range: Narrow (weapons only)
└─ Market Risk: Low (controls entire supply)

Horizontal Specialization (Broad):
Example: Weaponsmithing (100) + Tailoring (70) + Woodworking (60) + Cooking (50)
├─ Total Investment: 280 skill points
├─ Time Investment: ~450 hours
├─ Market Position: Multi-product trader
├─ Profit Margin: Medium (buys materials)
├─ Product Range: Wide (diverse offerings)
└─ Market Risk: Medium (material price volatility)

Deep Specialization (Single Focus):
Example: Weaponsmithing (100) only, buy all materials
├─ Total Investment: 100 skill points
├─ Time Investment: ~250 hours
├─ Market Position: Premium weapons specialist
├─ Profit Margin: Low-Medium (high material costs)
├─ Product Range: Very Narrow (weapons only)
└─ Market Risk: High (material availability dependent)

Economic Recommendation:
- New players: Start with vertical integration in one cluster
- Mid-game: Maintain specialization with trading partnerships
- End-game: Deep specialization with established supply chains
```

### Appendix E: Balancing Guidelines

**For Game Designers:**

```
OVERLAP THRESHOLDS:

0-25% Overlap: Keep as separate skills
    └─ Example: Mining and Tailoring

25-50% Overlap: Separate with cross-bonuses
    └─ Example: Alchemy and Cooking

50-75% Overlap: Consider hierarchical structure
    └─ Example: Woodworking and Carpentry

75-100% Overlap: Merge or make linear progression
    └─ Example: Blacksmithing and Basic Metalworking

SYNERGY BONUSES:

Small Synergy (+1 to +20):
    └─ Formula: bonus = (skill_a + skill_b) / 200

Medium Synergy (+21 to +50):
    └─ Formula: bonus = (skill_a × skill_b) / 1000

Large Synergy (+51 to +100):
    └─ Formula: bonus = (skill_a × skill_b) / 500
    └─ Cap at reasonable maximum (usually +20%)

CONFLICT PENALTIES:

Time Conflicts:
    └─ Natural (players can only do one thing at a time)
    └─ No mechanical penalty needed

Resource Conflicts:
    └─ Implemented through scarcity and market prices
    └─ No direct skill penalty

Philosophical Conflicts (if alignment system):
    └─ -10% to -30% efficiency penalty
    └─ Only for thematic reasons (Combat vs. Crafting)
```

### Appendix F: Testing Recommendations

**Quality Assurance Focus Areas:**

```
1. SYNERGY BONUS VALIDATION
   ✓ Verify all synergy bonuses calculate correctly
   ✓ Test edge cases (skill at 0, skill at 100)
   ✓ Confirm bonuses display in UI
   ✓ Check that bonuses stack appropriately

2. PREREQUISITE ENFORCEMENT
   ✓ Confirm hard prerequisites block skill use
   ✓ Verify soft prerequisites show warnings
   ✓ Test unlock notifications fire correctly
   ✓ Check prerequisite chains (A→B→C)

3. SKILL CAP SYSTEM
   ✓ Verify total cap enforcement
   ✓ Test individual skill maximum
   ✓ Confirm "forget skill" functionality
   ✓ Check cap-reaching notifications

4. MATERIAL QUALITY PROPAGATION
   ✓ Test gathering skill affects material quality
   ✓ Verify processing skill affects quality retention
   ✓ Confirm final craft quality uses all factors
   ✓ Check quality display in UI

5. CROSS-SKILL XP GAINS
   ✓ Verify related skills gain partial XP
   ✓ Test XP ratios are balanced
   ✓ Confirm XP notifications are clear
   ✓ Check that XP gains don't allow exploits

6. PLAYER GUIDANCE
   ✓ Test skill advisor suggestions are relevant
   ✓ Verify relationship explanations are clear
   ✓ Confirm tooltip information is accurate
   ✓ Check progression path visualization
```

## Conclusion

This research provides a comprehensive framework for understanding and implementing skill relationships, 
compatibility, and substitutability in BlueMarble's skill system. Key takeaways:

**Major Findings:**

1. **Skills exhibit three primary relationship types:** Hierarchical (parent-child), complementary 
   (synergistic), and competitive (overlapping), each requiring different design approaches.

2. **Substitutability exists on a spectrum** (0-100%) rather than binary yes/no, determined by five 
   factors: output similarity, process similarity, material overlap, knowledge transfer, and market competition.

3. **High-overlap skills (75%+) should be merged or made hierarchical** to reduce system complexity while 
   maintaining depth (e.g., Blacksmithing + Basic Metalworking → unified Blacksmithing).

4. **Cross-skill synergy bonuses create natural progression incentives** without forcing specific paths, 
   allowing player choice while rewarding strategic skill combinations.

5. **Network models and compatibility matrices** provide robust frameworks for implementation, enabling 
   systematic balancing and clear player communication.

6. **The Fiber (Leaves) crafting system** exemplifies effective skill relationship visualization through 
   material quality linking, cross-skill bonus display, and progressive complexity.

**Design Principles:**

- **Transparency:** Players should understand how skills interact
- **Meaningful Choices:** Each skill combination should offer distinct benefits
- **Progressive Complexity:** Beginners see simple systems, experts discover depth
- **Natural Specialization:** Skill caps and time constraints encourage focus
- **Economic Integration:** Skill relationships drive player trading and cooperation

**Implementation Priority:**

1. **Phase 1:** Consolidate high-overlap skills, establish hierarchy
2. **Phase 2:** Implement cross-skill bonus system with clear UI feedback
3. **Phase 3:** Add skill cap system with smart allocation tools
4. **Phase 4:** Deploy player guidance and relationship discovery systems

This framework balances realism with playability, creates meaningful player choices, and supports 
BlueMarble's vision of an authentic, player-driven world where skills and specializations naturally 
emerge from gameplay.

---

**Document Status:** Complete  
**Last Updated:** 2025-10-02  
**Review Status:** Ready for Implementation Planning

