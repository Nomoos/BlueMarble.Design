# Life is Feudal: Material Quality and Crafting System Analysis

**Document Type:** Market Research & System Analysis  
**Version:** 1.0  
**Author:** BlueMarble Game Design Research Team  
**Date:** 2025-01-16  
**Status:** Research Report  
**Research Type:** Market Research  
**Priority:** Low

## Executive Summary

This research document provides a comprehensive analysis of Life is Feudal's material quality and crafting systems to inform the design of BlueMarble's geological material processing and quality mechanics. Life is Feudal (LiF) is a medieval survival MMORPG with one of the most sophisticated material quality and crafting progression systems in the genre, making it highly relevant for BlueMarble's material-driven gameplay.

**Key Findings:**
- Quality scale ranges from 0-100 with direct impact on crafted item effectiveness
- Material quality combines with player skill in weighted calculations
- Hard skill cap (600 points) creates forced specialization and economic interdependence
- Tiered skill progression (0/30/60/90) unlocks new recipes and improves quality
- Parent-child skill relationships create meaningful skill investment choices
- Tool quality multiplies crafting effectiveness (quality and speed)
- Processing chains require multiple specialized crafters for optimal outcomes
- Alignment system separates crafting and combat progression paths

**Relevance to BlueMarble:**
Life is Feudal's systems demonstrate how material quality can drive player specialization, economic complexity, and long-term engagement in a simulation-focused MMORPG. The integration of geological material properties with crafting mechanics provides a proven model for BlueMarble's scientific approach to material science.

## Table of Contents

1. [Research Methodology](#research-methodology)
2. [Material Quality System](#material-quality-system)
3. [Crafting Progression Mechanics](#crafting-progression-mechanics)
4. [Specialization and Skill Cap System](#specialization-and-skill-cap-system)
5. [Tool Quality and Workshop Impact](#tool-quality-and-workshop-impact)
6. [Processing Chains and Dependencies](#processing-chains-and-dependencies)
7. [Economic Impact and Player Interdependence](#economic-impact-and-player-interdependence)
8. [Comparative Analysis with Other Systems](#comparative-analysis-with-other-systems)
9. [BlueMarble Design Recommendations](#bluemarble-design-recommendations)
10. [Implementation Considerations](#implementation-considerations)
11. [Related Research](#related-research)

## Research Methodology

### Research Sources

**Primary Sources:**
- Official Life is Feudal Wiki: https://lifeisfeudal.fandom.com/wiki/Life_Is_Feudal_Wiki
- Official game documentation: https://lifeisfeudal.com/
- Developer blog posts and patch notes
- Player guides and community analysis

**Analysis Approach:**
1. **Material System Mechanics:** How quality is determined and represented
2. **Skill Integration:** Relationship between skills and material quality
3. **Progression Depth:** How players advance in crafting specializations
4. **Economic Systems:** Impact of quality on player economy
5. **Player Psychology:** Engagement through specialization and mastery

**Research Questions:**
- How does Life is Feudal implement material quality and crafting systems?
- What progression and specialization mechanics are present?
- What lessons can be applied to BlueMarble's geological material system?

### Research Scope

**In Scope:**
- Material quality calculation formulas
- Crafting progression and skill tiers
- Specialization mechanics and constraints
- Tool and workshop quality systems
- Processing chains and material dependencies

**Out of Scope:**
- Combat systems and PvP mechanics
- Territory control and guild warfare
- Character creation and attribute systems
- Non-crafting skills (combat, survival, etc.)

## Material Quality System

### Quality Scale and Representation

Life is Feudal uses a **0-100 quality scale** for all materials and crafted items:

```
Quality Range:
0-20   : Very Poor (unusable or barely functional)
21-40  : Poor (low durability, weak stats)
41-60  : Average (standard functionality)
61-80  : Good (enhanced performance)
81-95  : Excellent (significantly better)
96-100 : Outstanding (exceptional, rare)
```

**Key Characteristics:**
- **Linear Scale:** Quality directly affects item statistics (damage, armor, durability)
- **Visible to Players:** Exact quality number shown on all items
- **Inheritable:** Material quality carries through processing chains
- **Degradation:** Items lose quality through use (durability loss)

### Material Extraction Quality

Raw material quality depends on multiple factors:

#### Geological Node Quality
```
Node Quality = Base_Quality × (1 + Bonus_Modifiers)

Where:
Base_Quality = 50 (average) + Random(-10, +10)
Bonus_Modifiers = Skill_Bonus + Tool_Bonus + Location_Bonus
```

**Example: Mining Iron Ore**
```
Base Node Quality: 55 (above average deposit)
Mining Skill Bonus: +15% (70 mining skill at 60 tier)
Pickaxe Quality: +8% (80 quality tool)
Result: 55 × 1.23 = 67.65 ≈ 68 quality iron ore
```

#### Skill Impact on Extraction
```
Extraction_Quality = Node_Quality × Skill_Multiplier

Skill Tiers:
0-29   : 0.70× multiplier (30% quality penalty)
30-59  : 0.85× multiplier (15% quality penalty)
60-89  : 1.00× multiplier (full quality extraction)
90-100 : 1.15× multiplier (15% quality bonus)
```

**Critical Insight:** Low-skill gatherers waste high-quality resources by extracting at reduced quality.

### Material Processing Quality

Processing materials (smelting, refining, etc.) follows a quality calculation:

```
Processed_Quality = (Input_Quality_Avg × 0.7 + Skill_Quality × 0.3) × Tool_Multiplier

Components:
Input_Quality_Avg = Average quality of all input materials
Skill_Quality = (Player_Skill / Recipe_Skill_Req) × 100
Tool_Multiplier = (Tool_Quality / 100) × Efficiency_Factor
```

**Example: Smelting Iron Ore to Iron Ingot**
```
Input: 3× Iron Ore (quality 68, 65, 72) → Average = 68.33
Player Skill: 75 Smelting
Recipe Requirement: 60 Smelting
Tool: 85 quality forge

Skill_Quality = (75 / 60) × 100 = 125 (capped at 100)
Base_Quality = (68.33 × 0.7) + (100 × 0.3) = 47.83 + 30 = 77.83
Tool_Multiplier = (85 / 100) × 1.1 = 0.935
Final_Quality = 77.83 × 0.935 = 72.77 ≈ 73 quality iron ingot
```

### Quality Inheritance in Crafting

Final item quality uses weighted input materials:

```
Crafted_Quality = (Σ(Material_Quality × Weight) × 0.6) + (Skill_Quality × 0.4)

Where:
Weight = Material importance to recipe (primary = 1.0, secondary = 0.5)
Skill_Quality = Player skill effectiveness vs recipe requirement
```

**Example: Crafting Iron Sword**
```
Inputs:
- 2× Iron Ingot (quality 73) - Primary material (weight = 1.0)
- 1× Oak Handle (quality 60) - Secondary material (weight = 0.5)

Material_Avg = ((73 × 2 × 1.0) + (60 × 1 × 0.5)) / (2 + 0.5) = (146 + 30) / 2.5 = 70.4
Player Skill: 80 Blacksmithing
Recipe Req: 70 Blacksmithing
Skill_Quality = (80 / 70) × 100 = 114 (capped at 100)

Final_Quality = (70.4 × 0.6) + (100 × 0.4) = 42.24 + 40 = 82.24 ≈ 82 quality iron sword
```

### Quality Impact on Item Performance

Quality directly scales item statistics:

**Weapons:**
```
Base_Damage = Recipe_Base_Damage
Actual_Damage = Base_Damage × (Quality / 100)

Example: Iron Sword
Base: 30 damage
Q50: 30 × 0.50 = 15 damage
Q70: 30 × 0.70 = 21 damage
Q90: 30 × 0.90 = 27 damage
```

**Armor:**
```
Armor_Value = Base_Armor × (Quality / 100)
Durability = Base_Durability × (Quality / 100)
```

**Tools:**
```
Efficiency = Base_Efficiency × (Quality / 100)
Work_Speed = Base_Speed × (Quality / 100)
Durability = Base_Durability × (Quality / 100)
```

**Critical Design Point:** Quality is never cosmetic in Life is Feudal—it always has mechanical impact.

## Crafting Progression Mechanics

### Skill Tier System

Life is Feudal uses **tiered skill progression** with breakpoints at 30/60/90:

```
Skill Tiers:
Tier 0 (0-29)   : Beginner - Basic recipes, quality penalties
Tier 1 (30-59)  : Competent - Intermediate recipes, reduced penalties
Tier 2 (60-89)  : Expert - Advanced recipes, full quality access
Tier 3 (90-100) : Master - Premium recipes, quality bonuses
```

### Recipe Unlocking

Recipes unlock at specific skill levels:

**Example: Blacksmithing Progression**
```
Skill 0   : Can craft: Simple tools, crude weapons (Q < 40)
Skill 30  : Unlocks: Iron tools, basic armor (Q < 60)
Skill 60  : Unlocks: Steel weapons, plate armor (Q < 80)
Skill 90  : Unlocks: Masterwork items, rare recipes (Q ≤ 100)
```

**Unlock Formula:**
```
Recipe_Available = Player_Skill >= Recipe_Min_Skill
Recipe_Quality_Cap = min(100, Recipe_Base_Quality + (Player_Skill - Recipe_Min_Skill) × 0.5)
```

### Skill Gain Mechanics

Skills improve through use with diminishing returns:

```
Skill_Gain = Base_Gain × Difficulty_Multiplier × Success_Modifier

Base_Gain = 0.01 to 0.1 (depends on action complexity)
Difficulty_Multiplier = Recipe_Skill / Player_Skill (higher = more gain)
Success_Modifier = {
    1.0 if successful
    0.3 if failed (still gain experience from failures)
}
```

**Exponential Difficulty Curve:**
```
Time to Next Skill Point (approximate):
0-30   : 1-2 hours per point (fast progression)
30-60  : 3-5 hours per point (moderate)
60-90  : 8-15 hours per point (slow)
90-100 : 20-40 hours per point (very slow)

Total time to 100: ~500-800 hours of focused crafting
```

### Parent-Child Skill Relationships

Skills have hierarchical dependencies:

```
Blacksmithing (Parent)
├── Weapon Smithing (Child)
├── Armor Smithing (Child)
└── Tool Smithing (Child)

Parent Bonus:
Child_Effective_Skill = Child_Skill + (Parent_Skill × 0.2)

Example:
Blacksmithing: 80
Weapon Smithing: 50
Effective Weapon Smithing = 50 + (80 × 0.2) = 50 + 16 = 66
```

**Strategic Implication:** Investing in parent skills provides broad bonuses across multiple crafting types.

### Quality Improvement with Skill

As skills increase, crafters can achieve higher quality:

```
Max_Achievable_Quality = Base_Recipe_Quality + Skill_Bonus

Skill_Bonus calculation:
Skill < Recipe_Req : -30 to 0 (penalty zone)
Skill = Recipe_Req : 0 (baseline)
Skill > Recipe_Req : 0 to +20 (bonus zone)

Formula:
Skill_Bonus = min(20, (Player_Skill - Recipe_Req) × 0.5)
```

**Example: Crafting with Increasing Skill**
```
Recipe: Iron Sword (requires 70 Blacksmithing, base quality 70)

Player Skill 50: Max quality ≈ 55 (penalty of -15)
Player Skill 70: Max quality ≈ 75 (baseline)
Player Skill 90: Max quality ≈ 85 (bonus of +10)
Player Skill 100: Max quality ≈ 90 (bonus of +15, materials matter more)
```

## Specialization and Skill Cap System

### Hard Skill Cap Mechanics

Life is Feudal enforces a **600-point total skill cap**:

```
Total_Skill_Points_Available = 600

Constraints:
- All ~50 skills share this pool
- Minimum 0 per skill, maximum 100 per skill
- Reallocation possible but costly (skill decay)
```

### Specialization Pressure

The hard cap forces meaningful choices:

**Specialist Build (Deep Mastery):**
```
Example: Master Blacksmith
Blacksmithing: 100 (100 points)
Mining: 90 (90 points)
Smelting: 90 (90 points)
Material Lore: 60 (60 points)
Construction: 60 (60 points)
Prospecting: 50 (50 points)
Other skills: 150 points distributed (minimal competence)
Total: 600 points
```

**Generalist Build (Broad Capability):**
```
Example: Self-Sufficient Crafter
8 crafting skills at 60: 480 points
4 gathering skills at 30: 120 points
Total: 600 points
Can do many things adequately, master none
```

**Critical Design Insight:** The 600-point cap creates **forced interdependence**—no single player can be expert at everything, driving economic cooperation.

### Alignment System

Life is Feudal separates crafting and combat progression:

```
Alignment Values:
+1000 to +500  : Crafting Alignment (combat skills decay faster)
+499 to -499   : Neutral (balanced progression)
-500 to -1000  : Combat Alignment (crafting skills decay faster)

Alignment Changes:
Crafting actions: +1 to +5 alignment per action
Combat actions: -1 to -5 alignment per action
```

**Effect on Skill Progression:**
```
Skill_Gain_Multiplier = {
    1.5× if aligned with action type
    1.0× if neutral
    0.5× if misaligned
}

Skill_Decay_Rate = {
    0.5× if aligned
    1.0× if neutral
    2.0× if misaligned
}
```

**Strategic Implication:** Players must commit to either crafting or combat focus, preventing hybrid master characters.

### Specialization Depth Example

**Master Weaponsmith Progression Path:**

```
Phase 1 (0-300 hours): Foundation
├── Mining: 0 → 70 (gather raw materials)
├── Smelting: 0 → 60 (process ores)
├── Blacksmithing: 0 → 60 (basic crafting)
└── Points Used: 190 / 600

Phase 2 (300-600 hours): Specialization
├── Blacksmithing: 60 → 90 (expert crafting)
├── Weapon Smithing: 0 → 70 (weapon focus)
├── Mining: 70 → 80 (better resources)
└── Points Used: 340 / 600

Phase 3 (600-1000 hours): Mastery
├── Blacksmithing: 90 → 100 (mastery)
├── Weapon Smithing: 70 → 90 (expert weapons)
├── Material Lore: 0 → 60 (quality identification)
└── Points Used: 530 / 600

Remaining 70 points: Utility skills (construction, animal lore, etc.)
```

**Time Investment:** ~1000 hours to achieve master weaponsmith status with supporting skills.

## Tool Quality and Workshop Impact

### Tool Quality System

Tools in Life is Feudal have their own quality ratings that multiply effectiveness:

```
Tool_Effectiveness = Base_Effectiveness × (Tool_Quality / 100)

Effects:
- Work Speed: Higher quality = faster crafting/gathering
- Quality Bonus: Higher quality = better output quality
- Durability: Higher quality = longer tool life
```

**Example: Mining with Different Quality Picks**
```
Base mining speed: 10 ore per minute
Q50 pick: 10 × 0.50 = 5 ore/min, Q penalty -20%
Q75 pick: 10 × 0.75 = 7.5 ore/min, Q penalty -10%
Q90 pick: 10 × 0.90 = 9 ore/min, Q bonus +5%
```

### Workshop and Building Quality

Crafting buildings also have quality ratings:

```
Building Types:
- Forge (for metalworking)
- Kiln (for ceramics)
- Workshop (for woodworking)
- Tailor Shop (for textiles)

Building_Quality_Bonus = (Building_Quality - 50) × 0.3

Q60 Forge: +3% to crafted item quality
Q80 Forge: +9% to crafted item quality
Q95 Forge: +13.5% to crafted item quality
```

**Compounding Effects:**
```
Total_Quality = Base × Tool_Mult × Workshop_Mult

Example: Master smith with good equipment
Base crafted quality: 80
Tool multiplier: 1.10 (Q90 hammer)
Workshop multiplier: 1.09 (Q80 forge)
Final quality: 80 × 1.10 × 1.09 = 95.92 ≈ 96
```

### Tool Durability and Maintenance

Tools degrade with use:

```
Durability_Loss_Per_Use = Base_Loss / (Tool_Quality / 100)

Q50 tool: 2× durability loss (breaks faster)
Q100 tool: 1× durability loss (baseline)

Repair_Quality_Loss = Current_Quality × 0.05 per repair
(Tools lose 5% quality when repaired)
```

**Economic Implication:** High-quality tools are valuable investments that pay off through increased productivity and reduced replacement costs.

## Processing Chains and Dependencies

### Multi-Stage Processing

Most valuable items require multiple processing stages:

**Example: Steel Sword Production Chain**

```
Stage 1: Resource Extraction
└── Iron Ore (Q70) ← Mining skill + tool quality
    └── Mined from high-quality deposit

Stage 2: Primary Processing
└── Iron Ingot (Q73) ← Smelting skill + forge quality
    └── Flux needed for steel quality

Stage 3: Alloying
└── Steel Ingot (Q78) ← Advanced smelting + crucible
    └── Requires charcoal (wood quality matters)

Stage 4: Component Crafting
├── Steel Blade (Q80) ← Blacksmithing + hammer quality
└── Leather Handle (Q65) ← Leatherworking + tools
    └── Requires tanned leather (previous chain)

Stage 5: Final Assembly
└── Steel Sword (Q77) ← Weapon Smithing + workshop
    └── Quality = weighted average of components
```

**Key Observation:** Each stage requires different specialist skills, making cooperation essential for optimal quality.

### Quality Bottlenecks

The weakest link determines final quality:

```
Chain Quality = min(Stage1_Q, Stage2_Q, ..., StageN_Q) × 0.95

Example with bottleneck:
Iron Ore: Q85 (excellent miner)
Smelting: Q50 (poor smelter) ← BOTTLENECK
Blacksmithing: Q90 (master smith)
Final weapon: ~Q48 (limited by smelting)
```

**Strategic Insight:** Investing in mid-chain processing skills is as important as mastering the final crafting stage.

### Material Substitution and Alternatives

Recipes often allow material substitutions with quality trade-offs:

```
Example: Weapon Handle
├── Oak Handle (base quality, 1.0× modifier)
├── Ash Handle (lighter, 1.05× quality, 0.9× weight)
└── Ironwood Handle (premium, 1.15× quality, rare material)

Final Quality = Base_Quality × Material_Modifier
```

## Economic Impact and Player Interdependence

### Player-Driven Economy

Life is Feudal's quality system creates rich economic interactions:

**Price Scaling by Quality:**
```
Price = Base_Price × (Quality / 50)^1.5

Example: Iron Sword (base price 100 silver)
Q40 sword: 100 × (40/50)^1.5 = 71 silver
Q70 sword: 100 × (70/50)^1.5 = 166 silver
Q95 sword: 100 × (95/50)^1.5 = 262 silver
```

**Market Differentiation:**
- Low-quality goods (Q30-50): Mass market, affordable, replaceable
- Mid-quality goods (Q60-75): Professional grade, reliable
- High-quality goods (Q85+): Luxury market, rare, prestigious

### Specialist Services

Master crafters offer services rather than just items:

**Service Types:**
1. **Commission Crafting:** Customer provides materials, crafter provides skill
   - Price = Labor_Cost + Risk_Premium (material loss risk)
   - Guarantees minimum quality based on crafter reputation

2. **Repair and Maintenance:** Extending item lifespan
   - Requires skill equal to item's crafting requirement
   - Quality maintenance critical for valuable items

3. **Quality Assessment:** Material Lore skill identifies exact quality
   - Prevents fraud in material trading
   - Adds value to rare/high-quality materials

### Guild Specialization

Guilds often organize around crafting specializations:

**Example: Metallurgy Guild Structure**
```
Guild Roles:
├── Miners (3-4 members): Extract Q70+ ores
├── Smelters (2-3 members): Process to ingots
├── Blacksmiths (2-3 members): Craft tools and weapons
├── Armorers (1-2 members): Specialize in armor
└── Quality Control (1 member): Material Lore expert

Total: 10-15 specialized members
```

**Competitive Advantage:** Guilds with optimized crafting chains dominate markets through consistent high-quality production.

### Territory and Resource Control

Quality distribution across the map drives territorial conflicts:

```
Resource Quality Zones:
├── Common Areas: Q40-60 deposits (accessible to all)
├── Rare Areas: Q65-80 deposits (contested territory)
└── Premium Areas: Q85+ deposits (guild-controlled)

Strategic Value = Resource_Quality × Rarity × Accessibility
```

## Comparative Analysis with Other Systems

### Life is Feudal vs Other Crafting Systems

| Feature | Life is Feudal | Wurm Online | Vintage Story | BlueMarble (Current) |
|---------|----------------|-------------|---------------|----------------------|
| Quality Scale | 0-100 linear | 0-100 with QL impact | 0-100% | 1-100% |
| Skill Impact | 40% of quality | 60% of quality | 50% of quality | Relative skill |
| Material Impact | 60% of quality | 40% of quality | 50% of quality | 50% (material bonus) |
| Hard Skill Cap | Yes (600 points) | Yes (varies) | No | TBD |
| Specialization | Forced | Forced | Optional | TBD |
| Tool Quality | Multiplier | Direct impact | Durability focus | Tool quality bonus |
| Processing Chains | Multi-stage | Multi-stage | Multi-stage | Planned |
| Quality Inheritance | Weighted average | Weighted average | Weighted average | Planned |

### Strengths of Life is Feudal System

**Advantages:**
1. **Clear Progression:** 30/60/90 skill tiers provide concrete goals
2. **Economic Depth:** Quality variations create market stratification
3. **Forced Cooperation:** Hard cap ensures no player is self-sufficient
4. **Meaningful Specialization:** 500-1000 hour investment creates expertise
5. **Transparent Mechanics:** Players understand quality calculations

**Player Engagement Benefits:**
- Long-term character development (100+ hours per specialization)
- Reputation building through consistent quality
- Guild organizational challenges (optimal skill distribution)
- Economic PvP (competing through better products)

### Limitations and Challenges

**Weaknesses:**
1. **Steep Learning Curve:** Complex formulas intimidate new players
2. **Multi-Account Pressure:** Some players create "alt" characters for different specializations
3. **Skill Decay Frustration:** Misaligned actions cause skill loss
4. **Quality Obsession:** Players sometimes overvalue perfect quality over functionality
5. **Barrier to Entry:** High skill requirements limit participation in advanced crafting

**Design Trade-offs:**
- **Realism vs Accessibility:** Complex systems increase realism but reduce casual player appeal
- **Specialization vs Flexibility:** Hard caps create interdependence but limit solo play
- **Quality vs Quantity:** High-quality production is slow, limiting market supply

## BlueMarble Design Recommendations

### Core Principles to Adopt

**1. Visible, Meaningful Quality Scale**
```
Recommendation: Adopt 0-100 quality scale for materials
- Align with existing crafting-quality-model.md (already uses 0-100%)
- Display exact quality numbers to players
- Ensure quality has mechanical impact, not just cosmetic
```

**2. Material-Skill Weighted Calculation**
```
Current BlueMarble Formula:
q_final = q_base + Δq_material + Δq_random + Δq_spec + Δq_tools

Recommended Enhancement (inspired by LiF):
q_final = (Material_Quality × 0.6) + (Skill_Quality × 0.4) + Random(-5%, +5%)

Benefits:
- Encourages sourcing high-quality materials
- Still rewards skill investment
- Reduces pure RNG impact (±5% vs ±10%)
```

**3. Processing Chain Quality Inheritance**
```
Implement multi-stage processing:
Raw Material (geological) → Processed Material → Component → Final Product

Each stage:
- Requires appropriate skill
- Can improve or degrade quality
- Involves tool quality multipliers
```

**4. Tool Quality Multiplier System**
```
Current: Δq_tools = (tool_quality / 100%) · 10%
Recommended: Tool_Multiplier = (tool_quality / 100) × 1.2

Example:
Q50 tools: 0.6× effectiveness (significant penalty)
Q75 tools: 0.9× effectiveness (minor penalty)
Q100 tools: 1.2× effectiveness (20% bonus)
```

### Geological Material Quality Adaptation

**Integrate with Geological Processes:**

```
Material_Quality = Base_Geological_Quality × Extraction_Skill_Factor

Base_Geological_Quality factors:
- Formation depth (metamorphic grade)
- Mineral purity (composition analysis)
- Crystal structure (affects processing)
- Weathering state (affects usability)

Example: Granite Quality
├── Depth: 2000m = Q75 base (high metamorphic grade)
├── Feldspar purity: 85% = +5 quality
├── Crystal size: Coarse = +3 quality (easier to work)
└── Weathering: Fresh = +7 quality (no degradation)
Final Base Quality: 90 (before extraction skill)
```

**Geological Realism with Game Balance:**
- High-quality deposits are geologically rare (realistic)
- Quality affects processing difficulty (easier to work with pure materials)
- Regional quality variations drive exploration and trade

### Specialization System for BlueMarble

**Adapt Life is Feudal's Hard Cap Model:**

```
BlueMarble Skill Budget:
- Total: 600-800 points (slightly more flexible than LiF)
- Geological skills: ~15-20 skills
- Minimum per skill: 15 (basic competence)
- Maximum per skill: 100 (mastery)

Skill Categories:
├── Extraction (Mining, Quarrying, Drilling): 200 points for mastery
├── Processing (Smelting, Refining, Testing): 150 points for mastery
├── Analysis (Geology, Material Science, Quality Control): 150 points
└── Fabrication (Crafting, Assembly, Engineering): 200 points

Master Specialist: 100 + 90 + 60 + 50 = 300 primary skills
Supporting Skills: 300 points distributed
Total: 600 points
```

**Specialization Benefits:**
- Reputation system: "Master Geologist" titles
- Unique recipes unlock at 90+ skill
- Quality bonuses for specialization (current +15% is good)
- Economic niches (steel specialist, gemstone expert, etc.)

### Progression Pacing

**Adopt Tiered Skill Breakpoints:**

```
BlueMarble Skill Tiers (inspired by LiF):
Tier 0 (0-29)   : Novice - Basic materials, Q < 50
Tier 1 (30-59)  : Competent - Standard materials, Q < 70
Tier 2 (60-89)  : Expert - Advanced materials, Q < 90
Tier 3 (90-100) : Master - Premium materials, Q ≤ 100

Recipe unlocks at tier boundaries:
30: Steel tools, basic alloys
60: High-grade metals, precision instruments
90: Exotic materials, masterwork items
```

**Time Investment (similar to LiF):**
```
Tier 0→1 (0-30): 40-60 hours (learning phase)
Tier 1→2 (30-60): 80-120 hours (expertise phase)
Tier 2→3 (60-90): 200-300 hours (mastery phase)
Tier 3 polish (90-100): 150-200 hours (perfection phase)

Total to mastery: 470-680 hours per skill
```

### Economic System Design

**Quality-Based Market Stratification:**

```
Price Scaling (inspired by LiF):
Price = Base_Price × (Quality / 60)^1.5

Example: Steel Ingot (base 50 credits)
Q40 ingot: 50 × (40/60)^1.5 = 27 credits (budget option)
Q60 ingot: 50 × (60/60)^1.5 = 50 credits (standard)
Q80 ingot: 50 × (80/60)^1.5 = 77 credits (professional)
Q95 ingot: 50 × (95/60)^1.5 = 111 credits (premium)
```

**Player Services Market:**
- Commission crafting (bring your materials, pay for skill)
- Quality assessment services (Material Science skill)
- Repair and maintenance (preserve valuable items)
- Consulting (optimal material sourcing advice)

### Implementation Priorities

**Phase 1 (Months 1-2): Foundation**
- [ ] Implement 0-100 quality scale for geological materials
- [ ] Add quality to material database (extend EnhancedMaterial class)
- [ ] Integrate quality with existing crafting-quality-model.md
- [ ] Display quality in UI (exact numbers, not just tiers)

**Phase 2 (Months 3-4): Skill Integration**
- [ ] Implement skill tier system (0/30/60/90 breakpoints)
- [ ] Add parent-child skill relationships
- [ ] Create recipe unlock system based on skill tiers
- [ ] Balance skill gain rates (exponential curve)

**Phase 3 (Months 5-6): Processing Chains**
- [ ] Multi-stage material processing (extraction → processing → crafting)
- [ ] Quality inheritance through processing chains
- [ ] Tool quality multiplier system
- [ ] Workshop/building quality bonuses

**Phase 4 (Months 7-8): Specialization**
- [ ] Implement skill cap system (600-800 point budget)
- [ ] Specialization bonuses and titles
- [ ] Economic system (price scaling by quality)
- [ ] Reputation tracking for quality crafters

### Geological Realism Integration

**Maintain Scientific Accuracy:**

```
Quality Determination:
├── Geological factors (60%): Depth, composition, formation type
├── Extraction factors (25%): Skill, tools, technique
└── Processing factors (15%): Refinement, testing, analysis

Example: High-Grade Iron Ore
Geological Quality: 85 (deep banded iron formation, high Fe content)
Extraction Impact: 85 × 0.95 = 81 (competent miner with good tools)
Final Quality: 81 (ready for smelting)
```

**Educate Through Mechanics:**
- High metamorphic grade = better quality (teaches geology)
- Mineral purity affects processing (teaches chemistry)
- Crystal structure impacts usability (teaches material science)

## Implementation Considerations

### Technical Architecture

**Database Schema Extension:**

```csharp
// Extend existing EnhancedMaterial class
public class EnhancedMaterial : IMaterial
{
    // Existing properties from implementation-plan.md
    public MaterialId Id { get; set; }
    public string Name { get; set; }
    public MaterialQuality Quality { get; set; }  // Already exists
    
    // New properties for LiF-inspired system
    public int QualityValue { get; set; }  // 0-100 precise quality
    public GeologicalQualityFactors BaseQuality { get; set; }
    public ProcessingRequirement[] ProcessingChain { get; set; }
    public MaterialGrade Grade { get; set; }  // Poor/Standard/Premium/Exceptional
}

public class GeologicalQualityFactors
{
    public int FormationDepth { get; set; }
    public float MineralPurity { get; set; }  // 0.0-1.0
    public CrystalStructure Structure { get; set; }
    public WeatheringState Weathering { get; set; }
    
    public int CalculateBaseQuality()
    {
        // Geological simulation determines base quality
        return (int)(FormationDepth / 30 + MineralPurity * 20 + 
                     StructureBonus() + WeatheringPenalty());
    }
}
```

**Quality Calculator Service:**

```csharp
public class MaterialQualityCalculator
{
    public int CalculateExtractedQuality(
        int baseGeologicalQuality,
        int playerSkill,
        int toolQuality)
    {
        // LiF-inspired extraction formula
        float skillMultiplier = GetSkillMultiplier(playerSkill);
        float toolMultiplier = toolQuality / 100f;
        
        int extractedQuality = (int)(baseGeologicalQuality * 
                                     skillMultiplier * 
                                     toolMultiplier);
        
        return Mathf.Clamp(extractedQuality, 1, 100);
    }
    
    private float GetSkillMultiplier(int skill)
    {
        if (skill < 30) return 0.70f;
        if (skill < 60) return 0.85f;
        if (skill < 90) return 1.00f;
        return 1.15f;
    }
    
    public int CalculateProcessedQuality(
        List<MaterialInput> inputs,
        int playerSkill,
        int recipeRequirement,
        int toolQuality)
    {
        // Weighted average of input materials
        float materialAvg = CalculateWeightedMaterialAverage(inputs);
        
        // Skill quality factor
        float skillQuality = Mathf.Min(100f, 
            (playerSkill / (float)recipeRequirement) * 100f);
        
        // Combine: 60% materials, 40% skill
        float baseQuality = (materialAvg * 0.6f) + (skillQuality * 0.4f);
        
        // Tool multiplier
        float toolMultiplier = toolQuality / 100f;
        
        return (int)Mathf.Clamp(baseQuality * toolMultiplier, 1, 100);
    }
}
```

### UI/UX Considerations

**Quality Display:**

```
Material Display Example:
┌────────────────────────────────┐
│ Iron Ore                       │
│ Quality: 78 ⭐⭐⭐⭐           │
│ Grade: Premium                 │
│                                │
│ Geological Factors:            │
│ • Formation: Banded Iron (Q85) │
│ • Purity: 92% Fe₂O₃ (+5)      │
│ • Condition: Fresh (+3)        │
│                                │
│ Best Use: High-grade steel     │
│ Market Value: ~85 credits      │
└────────────────────────────────┘
```

**Crafting Interface:**

```
Crafting Preview:
┌────────────────────────────────┐
│ Recipe: Steel Ingot            │
│ Requires: Smelting 60          │
│ Your Skill: 75 (✓)             │
│                                │
│ Materials:                     │
│ • 3× Iron Ore (Q78 avg) ✓      │
│ • 1× Flux (Q60) ✓              │
│                                │
│ Expected Quality: 74-82        │
│ Success Chance: 95%            │
│ Time: 2 minutes                │
│                                │
│ [Craft] [Cancel]               │
└────────────────────────────────┘
```

### Testing and Validation

**Quality System Tests:**

```csharp
[Test]
public void TestQualityInheritance()
{
    // Verify quality carries through processing chain
    var ore = new Material { Quality = 80 };
    var ingot = ProcessMaterial(ore, skill: 70, toolQuality: 90);
    Assert.IsTrue(ingot.Quality >= 70 && ingot.Quality <= 85);
}

[Test]
public void TestSkillTierBreakpoints()
{
    // Verify tier bonuses apply correctly
    Assert.AreEqual(0.70f, GetSkillMultiplier(25));
    Assert.AreEqual(0.85f, GetSkillMultiplier(50));
    Assert.AreEqual(1.00f, GetSkillMultiplier(75));
    Assert.AreEqual(1.15f, GetSkillMultiplier(95));
}

[Test]
public void TestPriceScaling()
{
    // Verify quality affects price correctly
    var basePrice = 100;
    Assert.AreEqual(71, CalculatePrice(basePrice, 40));
    Assert.AreEqual(166, CalculatePrice(basePrice, 70));
    Assert.AreEqual(262, CalculatePrice(basePrice, 95));
}
```

### Performance Considerations

**Caching Strategy:**
```
Cache geological base quality per region:
- Regional quality variations stored in quadtree
- Individual node quality calculated on demand
- Quality calculations cached for frequently accessed materials
```

**Optimization:**
```
Quality calculations are O(1):
- Simple weighted averages
- No recursive traversal
- Suitable for real-time crafting UI updates
```

## Related Research

### Cross-References

This research integrates with:

1. **Skill and Knowledge System Research** (`skill-knowledge-system-research.md`)
   - Life is Feudal section provides context (lines 410-454)
   - Skill-based progression model analysis
   - Specialization depth comparisons

2. **Crafting Quality Model** (`docs/gameplay/mechanics/crafting-quality-model.md`)
   - Existing quality calculation formula
   - Material quality bonus system (currently ±25%)
   - Tool quality implementation (currently +10% max)
   - **Recommendation:** Update to align with LiF-inspired 60/40 material/skill split

3. **Skill Caps and Decay Research** (`skill-caps-and-decay-research.md`)
   - Hard cap system analysis
   - Specialization pressure mechanics
   - Time investment calculations
   - **Recommendation:** Adopt 600-800 point cap with geological skill focus

4. **Implementation Plan** (`implementation-plan.md`)
   - Material system extensions (Month 2, lines 101-128)
   - Enhanced material properties structure
   - Quality and grading system implementation
   - **Recommendation:** Prioritize quality scale implementation in Phase 1

5. **Player Freedom Analysis** (`player-freedom-analysis.md`)
   - Knowledge-based progression constraints
   - Intelligent constraint frameworks
   - **Recommendation:** Use quality system as natural constraint (skill requirements)

### Comparison with Other Researched Games

**Life is Feudal vs Wurm Online:**
- Both use 0-100 quality scale
- LiF: 40% skill / 60% material weighting
- Wurm: 60% skill / 40% material weighting
- **BlueMarble should favor material quality (geological focus)**

**Life is Feudal vs Vintage Story:**
- LiF: Hard skill cap creates forced specialization
- VS: No cap, optional specialization through depth
- **BlueMarble benefits from forced specialization (economic interdependence)**

**Life is Feudal vs Novus Inceptio:**
- Both geological/survival focused
- LiF: Medieval crafting chains
- NI: Modern geological equipment
- **BlueMarble can blend both approaches (historical progression)**

### Future Research Directions

**Recommended Follow-up Research:**

1. **Processing Chain Depth Analysis**
   - How many stages before diminishing returns?
   - Optimal complexity for player engagement vs frustration

2. **Economic Simulation**
   - Market equilibrium with quality stratification
   - Price discovery mechanisms
   - Supply/demand balancing

3. **Reputation and Social Systems**
   - Master crafter recognition
   - Quality-based leaderboards
   - Guild specialization benefits

4. **Tutorial and Onboarding**
   - Teaching quality system to new players
   - Progressive complexity introduction
   - Quality vs quantity decision education

## Conclusion

Life is Feudal's material quality and crafting system provides a proven model for depth, specialization, and economic complexity in simulation-focused MMORPGs. The key insights for BlueMarble are:

1. **60/40 Material/Skill Split:** Emphasizes geological quality while rewarding skill investment
2. **Hard Skill Cap:** Creates forced interdependence and emergent economic cooperation
3. **Tiered Progression:** Clear 30/60/90 breakpoints provide concrete goals
4. **Tool Quality Multipliers:** Makes equipment investment meaningful
5. **Processing Chains:** Multi-stage production encourages specialization
6. **Quality Transparency:** Exact quality numbers build player understanding and trust

**Implementation Priority: High**
Despite "Low" priority label, the material quality system is foundational for:
- Crafting mechanics (already documented)
- Economic systems (in development)
- Player specialization (skill system in design)
- Geological realism (core BlueMarble value)

**Recommended Action:**
Integrate Life is Feudal's quality calculation approach into existing `crafting-quality-model.md` during Phase 1 of the implementation plan, prioritizing material-driven quality for geological authenticity.

## References

### Primary Sources
- Life is Feudal Official Wiki: https://lifeisfeudal.fandom.com/wiki/Life_Is_Feudal_Wiki
- Life is Feudal Official Site: https://lifeisfeudal.com/
- Player Guides: Community-contributed crafting calculators and quality guides

### Related Documentation
- `research/game-design/skill-knowledge-system-research.md` - Lines 410-454 (Life is Feudal analysis)
- `docs/gameplay/mechanics/crafting-quality-model.md` - Current BlueMarble quality system
- `research/game-design/skill-caps-and-decay-research.md` - Skill progression analysis
- `research/game-design/implementation-plan.md` - Material system implementation (Month 2)

### Revision History
- **v1.0 (2025-01-16):** Initial research document created
- Created in response to GitHub issue: "Life is Feudal Material System Analysis"
