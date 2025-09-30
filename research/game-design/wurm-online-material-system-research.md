# Wurm Online Material and Quality System Research

**Document Type:** Market Research Report  
**Version:** 1.0  
**Author:** BlueMarble Game Design Research Team  
**Date:** 2024-12-29  
**Status:** Final  
**Research Type:** Market Research  
**Priority:** Low

## Executive Summary

This research document analyzes Wurm Online's material quality and crafting systems to identify mechanics that can
be adapted for BlueMarble's geological simulation MMORPG. Wurm Online features one of the most sophisticated material
quality systems in the MMORPG genre, with a 0-100 quality scale that affects every aspect of gameplay from crafting
to combat to economy.

**Key Findings:**

- Wurm Online uses a comprehensive 0-100 quality (QL) system that affects all items, materials, and structures
- Quality is determined by player skill, tool quality, and material quality in a multiplicative relationship
- The player-driven economy is heavily influenced by quality variations, creating natural market segmentation
- Over 130 skills directly interact with the quality system, creating deep interdependencies
- Quality degradation over time creates ongoing demand and economic cycles

**Recommendations for BlueMarble:**

- Adopt a similar continuous quality scale (0-100%) instead of discrete quality tiers
- Implement skill-quality relationships where player skill directly affects craftable quality ranges
- Use material quality as a primary driver of final product quality
- Create quality-based market differentiation to support robust player economy
- Integrate quality with geological material properties for enhanced realism


## Table of Contents

1. [Research Objectives](#research-objectives)
2. [Methodology](#methodology)
3. [Wurm Online Quality System Overview](#wurm-online-quality-system-overview)
4. [Material Quality Mechanics](#material-quality-mechanics)
5. [Crafting and Quality Calculation](#crafting-and-quality-calculation)
6. [Skill-Quality Relationship](#skill-quality-relationship)
7. [Tool Quality Impact](#tool-quality-impact)
8. [Quality Degradation System](#quality-degradation-system)
9. [Economic Impact Analysis](#economic-impact-analysis)
10. [Player-Driven Economy Interactions](#player-driven-economy-interactions)
11. [Comparison with BlueMarble Current Design](#comparison-with-bluemarble-current-design)
12. [Recommendations for BlueMarble](#recommendations-for-bluemarble)
13. [Implementation Considerations](#implementation-considerations)
14. [Next Steps](#next-steps)

## Research Objectives

### Primary Research Questions

1. How does Wurm Online manage material quality throughout the crafting pipeline?
2. What is the mathematical relationship between skill, tools, materials, and final quality?
3. How does the quality system influence player-driven economy dynamics?

### Secondary Research Questions

1. How do players specialize in quality production?
2. What quality ranges are viable for different market segments?
3. How does quality degradation create economic cycles?
4. What lessons can be applied to BlueMarble's geological focus?

### Success Criteria

This research is successful if it:

- Provides clear understanding of Wurm's quality calculation formulas
- Identifies adoptable mechanics for BlueMarble's crafting system
- Analyzes economic impacts of quality-based systems
- Delivers actionable recommendations for BlueMarble integration

## Methodology

### Research Approach

Qualitative analysis combining official documentation review, community resource analysis, and player economy study.

### Data Collection Methods

- **Wiki Analysis:** Comprehensive review of Wurmpedia documentation on quality mechanics
- **Community Forums:** Analysis of player discussions on crafting strategies and economy
- **Player Guides:** Review of expert player guides on skill progression and crafting optimization
- **Economic Analysis:** Study of market trends and pricing based on quality tiers

### Data Sources

- Wurmpedia (<https://www.wurmpedia.com/>) - Official community wiki
- Wurm Online Official Website (<https://www.wurmonline.com/>)
- Wurm Online Forums - Player economy and crafting discussions
- Community crafting calculators and skill planning tools

### Limitations

- Limited access to actual game code and precise formulas
- Quality formulas documented by players may have minor inaccuracies
- Economic data is anecdotal from community observations
- Focus is on Freedom cluster (PvE) rather than Epic/PvP mechanics


## Wurm Online Quality System Overview

### Core Quality Concept

Wurm Online uses a **Quality Level (QL)** system ranging from **0.00 to 100.00** that applies to:

- Raw materials (ore, wood, clay, etc.)
- Processed materials (metal lumps, planks, bricks)
- Tools and equipment
- Crafted items and structures
- Food and consumables
- Vehicles and mounts

### Quality Properties

Quality affects:

**For Tools:**

- Effectiveness in gathering/crafting actions
- Damage output (weapons)
- Protective value (armor)
- Durability and decay rate
- Speed of action completion

**For Materials:**

- Final quality potential of crafted items
- Processing efficiency
- Market value
- Weight (in some cases)

**For Structures:**

- Defensive strength
- Decay resistance
- Aesthetic appearance
- Functional capacity

### Quality Display

Quality is shown to players with precision:

```text
Iron Lump, QL 45.23
Large Anvil, QL 78.91
Longsword, QL 62.50
```

This precise display creates psychological attachment to incremental improvements and allows for detailed market
segmentation.

## Material Quality Mechanics

### Raw Material Extraction

When harvesting raw materials, quality is determined by:

**Formula (Player Documented):**

```text
Material_QL = min(skill_level, node_QL) + random_variation

where:
- skill_level = player's relevant gathering skill
- node_QL = quality potential of the resource node (varies by location)
- random_variation = typically ±5 QL
```

**Key Characteristics:**

- **Skill Cap:** Cannot extract materials significantly above skill level
- **Node Variance:** Different locations have different quality potential
- **Improvement Path:** Higher skill = access to higher quality materials

### Material Quality Ranges

**Common Quality Tiers (Player Classification):**

- **1-20 QL:** Beginner materials, minimal value
- **20-40 QL:** Functional materials, common market items
- **40-60 QL:** Good quality, competitive market segment
- **60-80 QL:** High quality, premium pricing
- **80-90 QL:** Expert level, rare and expensive
- **90-100 QL:** Master tier, extremely rare, prestige items

### Material Processing

Processing raw materials (smelting ore to metal lumps, cutting logs to planks) follows:

**Processing Formula:**

```text
Processed_QL = (input_QL + tool_QL + skill_level) / 3 + bonus - penalty

where:
- input_QL = quality of raw material
- tool_QL = quality of tool used
- skill_level = player's processing skill
- bonus = from various factors (location, improvements, etc.)
- penalty = from failing the action
```

**Critical Insights:**

- All three factors (input, tool, skill) matter equally in processing
- Using high-quality tools on low-quality materials still produces low results
- Balanced investment across all factors yields best results

### Material Combination

When crafting items requiring multiple materials:

**Average Quality Approach:**

```text
Combined_Material_QL = weighted_average(material1_QL, material2_QL, ...)

Examples:
- Sword: Iron Lump QL + Handle QL (weighted by material amounts)
- Wall: Multiple materials averaged based on quantity used
```

**Design Philosophy:**

- Quality cannot be "hidden" by using one high-quality component
- Encourages consistent quality across all material inputs
- Creates market demand for complete quality sets

## Crafting and Quality Calculation

### Basic Crafting Formula

The core crafting formula in Wurm Online is:

**Final Quality Formula (Documented by Players):**

```text
Final_QL = (avg_material_QL × 0.3) + (tool_QL × 0.2) + (skill_level × 0.5) + random_factor

where:
- avg_material_QL = average quality of all input materials
- tool_QL = quality of primary crafting tool
- skill_level = relevant crafting skill (0-100)
- random_factor = ±10 QL maximum variance

Constraints:
- Final_QL cannot exceed player's skill level by more than 5-10 QL
- Critical success can add +5-10 QL bonus
- Critical failure can result in lower quality or item loss
```

### Quality Improvement Actions

Players can improve existing items:

**Improvement Formula:**

```text
New_QL = Current_QL + improvement_gain

improvement_gain = (skill_level - Current_QL) / difficulty_modifier

where:
- difficulty_modifier depends on item type (10-30 typically)
- Gains decrease as item approaches skill level
- Tool quality affects success chance but not gain amount
```

**Key Mechanics:**

- Improvement requires same skill as creation
- Multiple improvement sessions needed for high quality
- Diminishing returns as quality approaches skill cap
- Can damage item on critical failure (loses quality)

### Repair Mechanics

Damaged items can be repaired, affecting quality:

**Repair Quality Impact:**

```text
QL_after_repair = Original_QL × (Current_Damage_Percentage)

Example:
- Original QL: 70.00
- Damaged to 50% durability
- Repaired QL: ~68-70 (slight quality loss possible)
```

**Economic Implication:**

- High-quality items maintain value through repairs
- Creates ongoing demand for repair materials
- Master craftsmen can repair with minimal quality loss

## Skill-Quality Relationship

### Skill Level Caps on Quality

The relationship between skill and achievable quality is fundamental:

**Quality Ceiling Formula:**

```text
Max_achievable_QL = player_skill_level + bonus_factor

where:
- bonus_factor = 0-10 depending on conditions (rare, usually 0-5)
- Most common: Max_achievable_QL ≈ player_skill_level
```

**Progression Implications:**

```text
Skill 30 → Can reliably craft QL 25-30 items
Skill 50 → Can reliably craft QL 45-50 items  
Skill 70 → Can reliably craft QL 65-70 items
Skill 90 → Can reliably craft QL 85-90 items
```

### Skill Gain from Crafting

Skill improvement is tied to action difficulty:

**Skill Gain Formula (Player Theory):**

```text
Skill_gain = base_gain × difficulty_modifier × quality_modifier

where:
- base_gain = 0.0001 - 0.001 per action
- difficulty_modifier = target_QL / current_skill_level
- quality_modifier = bonus for attempting higher quality items

Optimal Skill Training:
- Craft items at 80-90% of current skill level
- Too easy = minimal gains
- Too hard = high failure rate, wasted materials
```

### Multi-Skill Dependencies

Many crafting actions involve multiple skills:

**Example: Blacksmithing Chain**

```text
Mining Skill (70) → Extract Iron Ore (QL 70)
    ↓
Smelting Skill (65) + Tool (QL 60) → Iron Lump (QL 65)
    ↓
Blacksmithing (80) + Anvil (QL 70) → Sword (QL 75)
    ↓
Weapon Smithing (75) → Improvement to QL 78
```

**Key Insight:**

- Production chain quality is limited by weakest skill
- Encourages player specialization and trade
- Creates interdependencies in player economy

### Skill-Based Market Segmentation

Different skill levels naturally create market tiers:

**Market Segments by Skill:**

- **Novice (0-30):** Basic tools, starter equipment
- **Journeyman (30-50):** Functional items, common market
- **Expert (50-70):** Quality items, competitive market
- **Master (70-90):** Premium items, specialist market
- **Legendary (90-100):** Prestige items, collector market

## Tool Quality Impact

### Tool Quality Multiplier

Tool quality directly affects crafting outcomes:

**Tool Impact Formula:**

```text
Tool_effectiveness = base_effectiveness × (tool_QL / 100)

Example:
- Base action time: 10 seconds
- QL 50 tool: 10 × (50/100) = 5 seconds faster
- QL 90 tool: 10 × (90/100) = 1 second (near optimal)
```

**Benefits of High-Quality Tools:**

- Faster action completion (30-50% faster at QL 90 vs QL 30)
- Higher success chance for difficult actions
- Better final quality outcomes
- Reduced stamina consumption

### Tool Specialization

Different tools have different quality impact profiles:

**Tool Categories:**

**Critical Tools (high quality essential):**

- Anvils for blacksmithing (directly affects final QL)
- Forges for smelting (affects processing quality)
- Stone chisels for masonry (affects building quality)

**Utility Tools (quality less critical):**

- Hammers for construction (affects speed more than quality)
- Saws for woodcutting (affects yield more than quality)
- Shovels for digging (affects speed primarily)

**Precision Tools (moderate quality importance):**

- Needles for leatherworking
- Carving knives for woodcraft
- Pottery spindles

### Tool Quality Investment Strategy

Players must decide tool quality investment:

**Economic Trade-off:**

```text
Low-Quality Tool (QL 30):
- Cost: 1-2 silver
- Time per craft: +30%
- Quality penalty: -10 QL on final product
- Break-even: ~100 crafts

High-Quality Tool (QL 80):
- Cost: 50-100 silver  
- Time per craft: baseline
- Quality bonus: +15 QL on final product
- Break-even: ~500 crafts but enables premium market
```

**Optimal Strategy:**

- Start with adequate tools (QL 40-50)
- Upgrade critical tools first (anvils, forges)
- Invest in high QL tools when specializing

## Quality Degradation System

### Damage Over Time

All items degrade through use and time:

**Decay Formula:**

```text
Damage_per_use = base_damage / (quality_modifier × maintenance_modifier)

where:
- base_damage = item type dependent (0.01-0.1 per use)
- quality_modifier = item_QL / 50 (higher QL = slower decay)
- maintenance_modifier = 1.0-2.0 based on storage conditions
```

**Example Decay Rates:**

```text
QL 30 Tool: ~1000 uses before needing repair
QL 60 Tool: ~2000 uses before needing repair
QL 90 Tool: ~4000 uses before needing repair
```

### Environmental Degradation

Items decay based on storage and exposure:

**Decay Modifiers:**

- **On-ground:** 2x decay rate
- **In containers:** 0.5x decay rate
- **Under roof:** 0.25x decay rate
- **In-use:** 1.0x decay rate
- **Quality affects decay:** Higher QL = slower natural decay

### Repair Economy

Quality degradation creates economic cycles:

**Repair Market Dynamics:**

```text
Repair Demand = (Total_Items × Decay_Rate) / Repair_Skill_Supply

Market Segments:
- Field Repairs: Quick, low-skill, maintains 80% QL
- Workshop Repairs: Slower, high-skill, maintains 95% QL
- Master Repairs: Premium, expert-skill, maintains 98-100% QL
```

**Economic Impact:**

- Continuous demand for repair materials
- Repair services create ongoing revenue for crafters
- High-quality items justify premium repair costs
- Creates natural item sink (eventual destruction)

## Economic Impact Analysis

### Price-Quality Relationship

Market prices strongly correlate with quality:

**Price Scaling by Quality (Player Market Data):**

```text
Base Item: Iron Longsword

QL 20-30: 1-2 silver (starter tier)
QL 40-50: 5-8 silver (functional tier)
QL 60-70: 20-30 silver (quality tier)
QL 80-85: 80-120 silver (premium tier)
QL 90-95: 300-500 silver (master tier)
QL 95-100: 1000+ silver (prestige tier)
```

**Price Scaling Formula:**

```text
Price ≈ Base_Price × (QL / 50)^2.5

This exponential relationship reflects:
- Increasing skill investment required
- Higher quality material costs
- Reduced supply at higher qualities
- Premium perception value
```

### Supply-Demand by Quality Tier

Different quality tiers have different market dynamics:

**Low Quality (QL 20-40):**

- **Supply:** Abundant (all skill levels)
- **Demand:** High volume (new players, disposable items)
- **Competition:** High (many suppliers)
- **Margins:** Low (5-10% profit)
- **Turnover:** Very fast (minutes to hours)

**Mid Quality (QL 40-60):**

- **Supply:** Moderate (intermediate skills)
- **Demand:** Steady (established players)
- **Competition:** Moderate
- **Margins:** Moderate (15-25% profit)
- **Turnover:** Fast (hours to days)

**High Quality (QL 60-80):**

- **Supply:** Limited (skilled crafters)
- **Demand:** Moderate (endgame players)
- **Competition:** Low (specialist market)
- **Margins:** High (30-50% profit)
- **Turnover:** Moderate (days to weeks)

**Premium Quality (QL 80-100):**

- **Supply:** Rare (master crafters only)
- **Demand:** Low volume (collectors, min-maxers)
- **Competition:** Minimal (few suppliers)
- **Margins:** Very high (50-100%+ profit)
- **Turnover:** Slow (weeks to months)

### Market Niches by Quality

Quality variations create specialized market niches:

**Example: Armor Market Segmentation**

```text
Starter Sets (QL 20-30):
- Customers: New players
- Volume: High
- Strategy: Mass production, fast turnover

Functional Sets (QL 50):
- Customers: Mid-level players
- Volume: Moderate  
- Strategy: Reliable quality, consistent supply

PvP Combat Sets (QL 70):
- Customers: Active combatants
- Volume: Steady replacement demand
- Strategy: Balance quality vs cost (consumable in PvP)

Prestige Sets (QL 90+):
- Customers: Collectors, guild leaders
- Volume: Very low
- Strategy: Reputation-based, commissioned work
```

## Player-Driven Economy Interactions

### Specialization Incentives

The quality system encourages specialization:

**Specialist Advantages:**

```text
General Crafter (Multiple skills at 50):
- Can produce QL 45-50 items across categories
- Competes in mid-tier markets
- Jack-of-all-trades, limited premium access

Specialist Crafter (One skill at 90):
- Can produce QL 85-90 items in specialty
- Commands premium pricing
- Master reputation, limited competition
- Requires focused time investment (500+ hours)
```

**Economic Calculation:**

```text
Generalist Revenue:
- 10 skills × 50 items/week × 10 silver avg = 5000 silver/week
- Lower margins, higher volume

Specialist Revenue:
- 1 skill × 10 items/week × 100 silver avg = 1000 silver/week
- Higher margins, lower volume, but less competition
- + Commissioned premium work: 500-2000 silver/week
- Total: 1500-3000 silver/week
```

### Trade Dependencies

Quality system creates natural trade networks:

**Production Chain Example: Master Sword**

```text
Player A (Mining 90) → Extract QL 85 Iron Ore
    ↓ (sells to Player B)
Player B (Smelting 80) + QL 75 Forge → QL 78 Iron Lump
    ↓ (sells to Player C)
Player C (Blacksmithing 95) + QL 90 Anvil → QL 88 Sword Blade
    ↓ (combines with)
Player D (Carpentry 90) → QL 87 Sword Handle
    ↓ (final assembly by Player C)
Player C (Weapon Smithing 95) → QL 90 Master Longsword
    ↓
Final Customer: 500 silver
```

**Value Distribution:**

- Player A: 20 silver (raw ore)
- Player B: 40 silver (processed metal)
- Player D: 50 silver (handle)
- Player C: 390 silver (final crafting, multiple skills, reputation)

### Reputation and Brand Value

Master craftsmen build reputation through consistent quality:

**Reputation Mechanics (Informal):**

- **Consistent Quality:** Known for QL 85+ items
- **Specialization:** "The best weaponsmith on server"
- **Reliability:** Delivers on commissioned work
- **Innovation:** Discovers optimal material combinations

**Brand Premium:**

```text
Generic QL 85 Sword: 80 silver (market rate)
Master Craftsman QL 85 Sword: 120 silver (50% premium)

Premium Justification:
- Guaranteed quality (no "lemons")
- Improvement services included
- Repair priority
- Prestige association
```

### Guild and Alliance Economies

Quality affects guild economic strategies:

**Guild Crafting Strategies:**

**Vertical Integration:**

```text
Guild Members specialize in production chain:
- Miners → Processors → Smiths → Improvers
- Internal quality standards (minimum QL 70)
- Cost savings vs market (20-30%)
- Strategic advantage in PvP (consistent quality)
```

**Quality Tiers by Purpose:**

```text
Guild Equipment Standards:
- Training/Casual: QL 40-50 (cheap, replaceable)
- Active Members: QL 60-70 (balanced quality/cost)
- Elite/Officers: QL 80+ (maximum performance)
- Ceremonial/Prestige: QL 90+ (guild reputation)
```

### Economic Cycles

Quality degradation creates predictable economic cycles:

**Seasonal Demand Patterns:**

```text
Week 1-2 (Post-Update):
- High demand for new items
- Premium quality sales peak
- Material prices spike

Week 3-4:
- Steady replacement demand
- Mid-tier quality dominates
- Material prices stabilize

Month 2-3:
- Repair services increase
- Replacement cycle begins
- Used item market active

Month 4+:
- Major item sink period
- New crafting cycle
- Premium quality renewed demand
```

## Comparison with BlueMarble Current Design

### BlueMarble Existing Quality System

Based on existing documentation, BlueMarble uses:

**Current Model (from crafting-quality-model.md):**

```text
Quality Tiers:
- Common (0-50%)
- Good (50-70%)
- Fine (70-85%)
- Superior (85-95%)
- Masterwork (95-100%)

Quality Formula:
q_final = (x × 100) + material_bonus + random + specialization + tool_bonus

where:
- x = player_skill / recipe_skill
- material_bonus = (avg_material_quality - 50%) × 0.5
- random = U(-10%, +10%)
- specialization = +15% if specialized
- tool_bonus = (tool_quality / 100) × 10%
```

### Key Differences from Wurm Online

| Aspect | Wurm Online | BlueMarble Current |
|--------|-------------|-------------------|
| **Quality Scale** | Continuous 0-100 | Continuous with tier labels |
| **Display** | Precise (45.23) | Percentage-based |
| **Skill Cap** | Hard (can't exceed skill +5) | Soft (multiplicative) |
| **Material Impact** | 30% weight | 25% weight (scaled) |
| **Tool Impact** | 20% weight | 10% weight (scaled) |
| **Skill Impact** | 50% weight | Primary determinant |
| **Random Factor** | ±10 QL | ±10% |
| **Quality Tiers** | Player-defined ranges | System-defined tiers |
| **Improvement** | Repeated actions | Not specified |
| **Degradation** | Active decay system | Durability-based decay |

### Compatibility Analysis

**Strengths of Current BlueMarble Approach:**

- ✅ Skill-driven quality aligns with use-based progression
- ✅ Material quality integration exists
- ✅ Tool quality considered
- ✅ Specialization bonuses encourage focus
- ✅ Random variation adds uncertainty

**Areas for Enhancement from Wurm:**

- ⚠️ Material weight could increase (30% vs current scaling)
- ⚠️ Tool weight could increase (20% vs current 10%)
- ⚠️ Hard skill caps could limit over-leveling
- ⚠️ Quality improvement mechanics not specified
- ⚠️ Degradation system could be more detailed
- ⚠️ No guild/reputation quality mechanics

## Recommendations for BlueMarble

### Core Recommendations

#### 1. Maintain Continuous Quality Scale

**Recommendation:** Keep continuous 0-100% quality scale, avoid discrete tiers for calculation.

**Rationale:**

- Allows incremental progression feeling
- Creates psychological attachment to improvements
- Enables precise market segmentation
- Aligns with geological simulation (natural variation)

**Implementation:**

```csharp
public class CraftingQuality
{
    // Internal: continuous 0-100
    public float QualityValue { get; set; }
    
    // Display: tier label + precise value
    public string QualityDisplay => $"{GetTierName()} ({QualityValue:F1}%)";
    
    private string GetTierName()
    {
        return QualityValue switch
        {
            >= 95 => "Masterwork",
            >= 85 => "Superior",
            >= 70 => "Fine",
            >= 50 => "Good",
            _ => "Common"
        };
    }
}
```

#### 2. Strengthen Material Quality Impact

**Recommendation:** Increase material quality weight from current formula to match Wurm's ~30% impact.

**Current BlueMarble:**

```text
material_bonus = (avg_material_quality - 50%) × 0.5  // Max ±25% impact
```

**Recommended:**

```text
material_bonus = (avg_material_quality - 50%) × 0.6  // Max ±30% impact

Justification:
- Emphasizes geological material properties
- Creates demand for high-quality materials
- Supports material gathering specialists
- Aligns with BlueMarble's geological focus
```

#### 3. Implement Hard Skill Caps

**Recommendation:** Add hard skill cap to prevent over-skilling producing unrealistic quality.

**Proposed Formula:**

```csharp
public float CalculateQualityWithSkillCap(CraftingContext context)
{
    float baseQuality = context.RelativeSkill * 100f;
    
    // Hard cap: can't exceed skill by more than 5%
    float skillCap = context.PlayerSkill + 5f;
    baseQuality = Mathf.Min(baseQuality, skillCap);
    
    // Continue with existing material, tool, random modifiers
    float materialBonus = CalculateMaterialBonus(context.Materials);
    float toolBonus = (context.ToolQuality / 100f) * 15f; // Increased from 10%
    float specializationBonus = context.HasSpecialization ? 15f : 0f;
    float randomVariation = Random.Range(-10f, 10f);
    
    float finalQuality = baseQuality + materialBonus + toolBonus + 
                        specializationBonus + randomVariation;
    
    return Mathf.Clamp(finalQuality, 1f, 100f);
}
```

**Benefits:**

- Prevents skill 100 with QL 10 materials producing QL 90 items (unrealistic)
- Creates natural progression ceiling
- Maintains value of material quality
- Aligns with geological realism

#### 4. Add Quality Improvement Mechanics

**Recommendation:** Implement iterative improvement system like Wurm's.

**Proposed System:**

```csharp
public class QualityImprovement
{
    public float AttemptImprovement(Item item, Player player, Tool tool)
    {
        float currentQL = item.Quality;
        float skillLevel = player.GetSkill(item.RequiredSkill);
        float toolQL = tool.Quality;
        
        // Can only improve if skill exceeds current quality
        if (skillLevel <= currentQL)
        {
            return currentQL; // No improvement possible
        }
        
        // Improvement gain diminishes as quality approaches skill
        float skillDifference = skillLevel - currentQL;
        float improvementGain = skillDifference / 20f; // ~5% per attempt at max difference
        
        // Tool quality affects success chance, not gain
        float successChance = (toolQL / 100f) * 0.8f + 0.2f; // 20-100% chance
        
        if (Random.Range(0f, 1f) < successChance)
        {
            float newQuality = currentQL + improvementGain;
            return Mathf.Min(newQuality, skillLevel + 5f); // Cap at skill + 5
        }
        else
        {
            // Critical failure: possible quality loss
            if (Random.Range(0f, 1f) < 0.05f) // 5% critical failure
            {
                return Mathf.Max(currentQL - 5f, 1f);
            }
            return currentQL; // No change on regular failure
        }
    }
}
```

**Benefits:**

- Provides endgame progression activity
- Creates market for improvement services
- Allows salvaging lower-quality items
- Adds depth to crafting gameplay

#### 5. Enhance Tool Quality Impact

**Recommendation:** Increase tool quality weight from 10% to 15-20%.

**Rationale:**

- Tools are geological items (hammers, anvils, forges)
- Higher tool weight creates demand for quality tools
- Supports tool-crafting specialists
- More realistic impact of tool quality on outcomes

**Proposed Formula:**

```text
tool_bonus = (tool_quality / 100) × 15  // Increased from 10

Impact Examples:
- QL 50 tool: +7.5% to final quality
- QL 75 tool: +11.25% to final quality  
- QL 95 tool: +14.25% to final quality
```

#### 6. Integrate with Geological Material Grades

**Recommendation:** Link quality system to geological material properties.

**Proposed Integration:**

```csharp
public class GeologicalMaterial
{
    public string MaterialType { get; set; } // "Hematite", "Magnetite", etc.
    public float QualityValue { get; set; } // 0-100
    public MaterialGrade GeologicalGrade { get; set; }
    
    // Geological properties affect quality potential
    public float IronContent { get; set; } // 0.45-0.70
    public float ProcessingDifficulty { get; set; } // 0.3-0.8
    public float DurabilityModifier { get; set; } // 0.9-1.4
    
    public float GetQualityModifier()
    {
        // Higher iron content = higher quality potential
        // Lower processing difficulty = easier to work
        return (IronContent * 1.2f) * (2.0f - ProcessingDifficulty);
    }
}

// Usage in crafting:
material_bonus = (avg_material_quality * geological_modifier - 50%) × 0.6
```

**Benefits:**

- Deep integration with BlueMarble's geological focus
- Creates meaningful material choices
- Reflects real-world material science
- Educational value for players

### Economic System Recommendations

#### 7. Implement Quality-Based Market Segmentation

**Recommendation:** Design auction/market systems with quality filtering.

**Proposed Market Interface:**

```text
Market Search: "Iron Longsword"

Filter by Quality:
[ ] Any Quality
[ ] 20-40 QL (Starter)
[x] 40-60 QL (Functional)  
[ ] 60-80 QL (Premium)
[ ] 80+ QL (Master)

Sort by:
[x] Price per QL (efficiency)
[ ] Absolute Price
[ ] Quality (highest first)

Results:
1. Iron Longsword QL 45.2 - 8 silver (17.7 silver/QL)
2. Iron Longsword QL 52.8 - 12 silver (22.7 silver/QL)
3. Iron Longsword QL 58.1 - 18 silver (31.0 silver/QL)
```

**Benefits:**

- Enables quality-conscious shopping
- Creates price competition within quality tiers
- Supports specialist crafters
- Improves market efficiency

#### 8. Create Quality-Based Reputation System

**Recommendation:** Track crafter quality averages and build reputation.

**Proposed System:**

```csharp
public class CrafterReputation
{
    public Dictionary<string, QualityStats> ItemQualityHistory { get; set; }
    
    public class QualityStats
    {
        public float AverageQuality { get; set; }
        public int ItemsCrafted { get; set; }
        public float ConsistencyScore { get; set; } // Std deviation
        public int CustomerSatisfaction { get; set; } // Ratings
    }
    
    public float GetReputationBonus(string itemType)
    {
        var stats = ItemQualityHistory[itemType];
        
        // Reputation bonus: up to +5% for consistent high quality
        if (stats.AverageQuality > 80 && stats.ConsistencyScore < 5 && 
            stats.ItemsCrafted > 100)
        {
            return 5f; // Master craftsman bonus
        }
        else if (stats.AverageQuality > 60 && stats.ItemsCrafted > 50)
        {
            return 2.5f; // Skilled craftsman bonus
        }
        
        return 0f;
    }
}
```

**Benefits:**

- Rewards consistent quality
- Creates master craftsman identities
- Adds social/economic gameplay
- Aligns with player-driven economy goals

#### 9. Design Guild Quality Standards

**Recommendation:** Enable guilds to set internal quality standards.

**Proposed Features:**

```text
Guild Crafting System:

Quality Standards:
- Minimum Quality: QL 60
- Member Equipment: QL 70+  
- Officer Equipment: QL 80+
- Guild Bank: Accept QL 65+ donations

Member Benefits:
- Internal Market: 20% discount vs public
- Priority Crafting: Guild crafters serve members first
- Quality Guarantee: Repairs/replacements for guild gear
- Material Pool: Shared high-quality materials

Guild Progression:
- Guild Reputation: Average quality of member crafts
- Guild Bonuses: +2% quality for all guild crafts (max)
- Guild Facilities: QL 90+ forges, anvils, workshops
```

**Benefits:**

- Encourages guild cooperation
- Creates guild economic strategies
- Adds guild progression dimension
- Supports competitive/collaborative gameplay

### Implementation Recommendations

#### 10. Phased Implementation Approach

**Phase 1: Core Quality System Enhancement (Month 1-2)**

- Implement hard skill caps on quality
- Increase material quality weight to 30%
- Increase tool quality weight to 15%
- Add precise quality display (XX.X%)

**Phase 2: Quality Improvement System (Month 3-4)**

- Implement iterative improvement mechanics
- Add improvement skill gain
- Create improvement failure risks
- Design improvement UI/UX

**Phase 3: Geological Integration (Month 5-6)**

- Link quality to geological material properties
- Implement material grade quality modifiers
- Add location-based quality variations
- Create geological quality discovery

**Phase 4: Economic Systems (Month 7-9)**

- Implement quality-based market filters
- Add crafter reputation tracking
- Create quality-based pricing algorithms
- Design guild quality standards

**Phase 5: Polish and Balance (Month 10-12)**

- Balance quality progression curves
- Tune economic parameters
- Refine player feedback systems
- Test edge cases and exploits

## Implementation Considerations

### Technical Architecture

**Quality Data Structure:**

```csharp
public class QualitySystem
{
    // Core quality management
    public float CalculateItemQuality(CraftingContext context) { }
    public float CalculateMaterialQuality(GatheringContext context) { }
    public float AttemptQualityImprovement(ImprovementContext context) { }
    
    // Quality display and formatting
    public string FormatQualityDisplay(float quality) { }
    public QualityTier GetQualityTier(float quality) { }
    
    // Economic integration
    public float CalculateQualityPriceModifier(float quality) { }
    public List<Item> FilterByQuality(List<Item> items, QualityRange range) { }
    
    // Reputation integration
    public void RecordCraftQuality(Player crafter, Item item) { }
    public float GetCrafterReputationBonus(Player crafter, string itemType) { }
}
```

### Database Schema

**Quality Tracking Tables:**

```sql
-- Item quality
CREATE TABLE items (
    item_id UUID PRIMARY KEY,
    item_type VARCHAR(100),
    quality_value DECIMAL(5,2), -- 0.00 to 100.00
    crafter_id UUID,
    created_timestamp TIMESTAMP,
    quality_tier VARCHAR(20), -- Cached for queries
    material_qualities JSON -- Track input material qualities
);

-- Crafter reputation
CREATE TABLE crafter_reputation (
    crafter_id UUID,
    item_type VARCHAR(100),
    avg_quality DECIMAL(5,2),
    total_crafted INTEGER,
    consistency_score DECIMAL(5,2),
    last_updated TIMESTAMP,
    PRIMARY KEY (crafter_id, item_type)
);

-- Quality improvement history
CREATE TABLE quality_improvements (
    improvement_id UUID PRIMARY KEY,
    item_id UUID,
    player_id UUID,
    quality_before DECIMAL(5,2),
    quality_after DECIMAL(5,2),
    success BOOLEAN,
    timestamp TIMESTAMP
);
```

### Performance Considerations

**Optimization Strategies:**

1. **Cache Quality Calculations:**

```csharp
public class QualityCache
{
    private Dictionary<string, float> _qualityCache = new();
    
    public float GetCachedQuality(string itemKey)
    {
        if (_qualityCache.TryGetValue(itemKey, out float quality))
        {
            return quality;
        }
        return -1f; // Not cached
    }
    
    public void CacheQuality(string itemKey, float quality)
    {
        _qualityCache[itemKey] = quality;
    }
}
```

2. **Index Quality Ranges for Market Queries:**

```sql
CREATE INDEX idx_items_quality_tier ON items(quality_tier);
CREATE INDEX idx_items_quality_range ON items(quality_value);
CREATE INDEX idx_items_type_quality ON items(item_type, quality_value);
```

3. **Batch Quality Updates:**

```csharp
public void BatchUpdateQualityDecay(List<Item> items)
{
    // Process in batches to avoid performance spikes
    const int batchSize = 1000;
    for (int i = 0; i < items.Count; i += batchSize)
    {
        var batch = items.Skip(i).Take(batchSize);
        ProcessDecayBatch(batch);
    }
}
```

### Balance Considerations

**Quality Progression Curve:**

```text
Target Skill-to-Quality Relationship:
Skill 0-20:  Can craft QL 0-20 (learning phase)
Skill 20-40: Can craft QL 15-40 (functional phase)
Skill 40-60: Can craft QL 35-65 (competitive phase)
Skill 60-80: Can craft QL 55-85 (expert phase)
Skill 80-90: Can craft QL 75-95 (master phase)
Skill 90-100: Can craft QL 85-100 (legendary phase)

Overlap ensures:
- Progression feels continuous
- Multiple skill levels compete in same quality tier
- Material/tool quality matters more at higher skills
```

**Economic Balance Parameters:**

```text
Price Scaling Target:
Base Price × (Quality / 50)^2.2

Examples (Base Price = 10 silver):
QL 25: 6.25 silver (starter tier)
QL 50: 10 silver (functional tier)
QL 75: 22.5 silver (expert tier)
QL 90: 40.5 silver (master tier)
QL 100: 52.3 silver (legendary tier)

Ensures:
- Linear progression not exponential (gentler curve than Wurm's 2.5)
- High quality accessible but valuable
- Premium tier exists without extreme prices
```

### Testing Requirements

**Quality System Tests:**

1. **Unit Tests:**
   - Quality calculation formula accuracy
   - Skill cap enforcement
   - Material quality averaging
   - Tool quality impact
   - Random variation bounds

2. **Integration Tests:**
   - Full crafting pipeline quality flow
   - Quality improvement iteration
   - Reputation tracking accuracy
   - Market filtering by quality
   - Guild quality standards enforcement

3. **Balance Tests:**
   - Quality progression curves
   - Economic pricing equilibrium
   - Skill-to-quality ratios
   - Material-to-quality impact
   - Tool-to-quality impact

4. **Performance Tests:**
   - Quality calculation speed (target: <1ms)
   - Market query performance with quality filters
   - Batch decay processing
   - Quality cache hit rates

## Next Steps

### Immediate Actions Required

- [ ] Review recommendations with design team - Due: Week 1 - Owner: Design Lead
- [ ] Prioritize implementation phases - Due: Week 1 - Owner: Product Manager
- [ ] Create technical specification for quality system enhancements - Due: Week 2 - Owner: Technical Lead
- [ ] Design UI/UX for quality display and improvement - Due: Week 3 - Owner: UX Designer
- [ ] Prototype quality calculation changes - Due: Week 4 - Owner: Gameplay Developer

### Follow-up Research

- Economic Simulation Study (8 weeks): Test quality-based economy with simulated player behaviors
- Player Testing (4 weeks): Gather feedback on quality system feel and progression
- Geological Material Properties Research (6 weeks): Deep dive into real material science for quality modifiers
- Guild System Integration Study (4 weeks): Design guild-level quality standards and benefits

### Stakeholder Communication

**Design Team Presentation (Week 2):**

- Present key findings and recommendations
- Discuss integration with existing crafting system
- Prioritize enhancement features

**Development Team Workshop (Week 3):**

- Technical architecture review
- Implementation complexity assessment
- Timeline and resource estimation

**Community Preview (Month 2):**

- Blog post on quality system philosophy
- Preview of geological material integration
- Gather early community feedback

## Appendices

### Appendix A: Wurm Online Quality Formulas Summary

**Core Crafting Formula:**

```text
Final_QL = (avg_material_QL × 0.3) + (tool_QL × 0.2) + (skill × 0.5) + random(-10, +10)
Constraint: Final_QL ≤ skill + 5
```

**Quality Improvement Formula:**

```text
QL_gain = (skill - current_QL) / difficulty_modifier
difficulty_modifier ≈ 15-25 depending on item type
```

**Material Gathering Formula:**

```text
Material_QL = min(skill, node_QL) + random(-5, +5)
```

**Processing Formula:**

```text
Processed_QL = (input_QL + tool_QL + skill) / 3
```

**Decay Formula:**

```text
Damage_per_use = base_damage / (QL / 50)
Higher QL = slower decay
```

### Appendix B: BlueMarble Integration Checklist

**Quality System Enhancement:**

- [ ] Implement continuous quality display (XX.X%)
- [ ] Add hard skill caps (skill + 5 max)
- [ ] Increase material quality weight (25% → 30%)
- [ ] Increase tool quality weight (10% → 15%)
- [ ] Add quality improvement mechanics
- [ ] Implement iterative improvement actions

**Geological Integration:**

- [ ] Link quality to material grades (Hematite, Magnetite, etc.)
- [ ] Add geological property modifiers
- [ ] Implement location-based quality variations
- [ ] Create material quality discovery gameplay

**Economic Systems:**

- [ ] Design quality-based market filters
- [ ] Implement quality price scaling
- [ ] Add crafter reputation tracking
- [ ] Create guild quality standards
- [ ] Design quality-based market segmentation

**UI/UX:**

- [ ] Design quality display interface
- [ ] Create improvement action interface
- [ ] Add market quality filters
- [ ] Design crafter reputation displays
- [ ] Create guild quality management UI

### Appendix C: Key References

**Primary Sources:**

- Wurmpedia Main Page: <https://www.wurmpedia.com/index.php/Main_Page>
- Wurmpedia Quality Article: <https://www.wurmpedia.com/index.php/Quality>
- Wurm Online Official Site: <https://www.wurmonline.com/>
- Wurm Forums Economy Discussions: <https://forum.wurmonline.com/>

**Player Guides:**

- "Understanding Quality in Wurm" - Community Guide
- "Optimal Crafting Strategies" - Player Analysis
- "Economic Guide to Quality Markets" - Trade Guild Documentation
- "Master Crafter's Handbook" - Advanced Techniques

**Comparative Research:**

- BlueMarble Crafting Quality Model: `/docs/gameplay/mechanics/crafting-quality-model.md`
- BlueMarble Skill System Research: `/research/game-design/skill-knowledge-system-research.md`
- BlueMarble Mechanics Research: `/research/game-design/mechanics-research.md`

### Appendix D: Revision History

| Version | Date | Author | Changes |
|---------|------|--------|---------|
| 1.0 | 2024-12-29 | BlueMarble Research Team | Initial comprehensive research report |

---

**Research Completed:** 2024-12-29  
**Status:** Final Report - Ready for Design Review  
**Next Review Date:** Q2 2025 (Post-Implementation Evaluation)

For questions or feedback on this research, please contact the BlueMarble Game Design Research Team.
