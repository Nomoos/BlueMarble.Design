# Crafting Quality Model - Mathematical Specification

**Document Type:** Mechanics Specification  
**Version:** 1.0  
**Author:** Game Design Team  
**Date:** 2025-01-15  
**Status:** Proposed

## Overview

This document extends the [Crafting Success Model](./crafting-success-model.md) with detailed quality
calculation mechanics. While the success model determines whether a craft succeeds or fails, the quality
model determines the tier and attributes of successful crafts.

## Design Goals

- **Skill Matters:** Higher skill produces better quality items consistently
- **Variation:** Some randomness prevents perfect predictability
- **Material Impact:** Better materials improve quality outcomes
- **Specialization Bonus:** Mastery in specific crafting areas provides benefits

## Quality Calculation Formula

### Base Quality

The base quality is primarily determined by relative skill:

```text
q_base = x · 100%

where x = player_skill / recipe_skill
```text

### Material Quality Modifier

When materials have variable quality, they affect the outcome:

```text
q_material = average(material_quality_1, material_quality_2, ..., material_quality_n)

Material bonus/penalty:
Δq_material = (q_material - 50%) · 0.5
```text

**Interpretation:**
- Average material quality of 50% has no effect (±0%)
- Average material quality of 100% adds +25% to final quality
- Average material quality of 0% subtracts -25% from final quality

### Random Variation

Natural variation in crafting outcomes:

```text
Δq_random = U(-10%, +10%)

where U is uniform random distribution
```text

### Specialization Bonus

Crafters can specialize in specific item categories:

```text
Δq_spec = {
    +15%,  if crafter has specialization in this category
    0%,    otherwise
}
```text

### Tools and Workshop Quality

Quality of tools and workspace affects outcomes:

```text
Δq_tools = (tool_quality / 100%) · 10%

Interpretation:
- Basic tools (50% quality):  +5% bonus
- Good tools (75% quality):   +7.5% bonus
- Master tools (100% quality): +10% bonus
```text

### Final Quality Calculation

```text
q_final = clamp(q_base + Δq_material + Δq_random + Δq_spec + Δq_tools, 1%, 100%)
```text

## Quality Tiers and Effects

### Tier Definitions

| Tier | Quality Range | Color | Rarity |
|------|--------------|-------|--------|
| Poor | 1% - 25% | Gray | Common |
| Common | 26% - 50% | White | Common |
| Good | 51% - 70% | Green | Uncommon |
| Fine | 71% - 85% | Blue | Rare |
| Superior | 86% - 95% | Purple | Epic |
| Masterwork | 96% - 100% | Orange | Legendary |

### Tier Effects on Items

Quality affects item attributes differently based on item type:

#### Weapons

```text
damage = base_damage · (1 + quality/100)
durability = base_durability · (1 + quality/200)
```text

**Example (Base Damage = 10):**
- Poor (25%): 12.5 damage
- Common (50%): 15 damage
- Fine (80%): 18 damage
- Masterwork (100%): 20 damage

#### Armor

```text
defense = base_defense · (1 + quality/100)
durability = base_durability · (1 + quality/150)
```text

#### Consumables (Potions, Food)

```text
effect_potency = base_effect · (0.8 + quality/500)
duration = base_duration · (0.9 + quality/1000)
```text

**Example (Base Healing = 50 HP):**
- Poor (25%): 42.5 HP healed
- Common (50%): 45 HP healed
- Fine (80%): 58 HP healed
- Masterwork (100%): 70 HP healed

## Advanced Quality Mechanics

### Critical Success

On rare occasions, a craft can produce an exceptional result:

```text
Critical Success Chance = (x - 1) · 5%, capped at 10%

Effect: Automatically achieve Masterwork tier (100% quality)
```text

**Example:**
- x = 1.0: 0% critical chance
- x = 1.5: 2.5% critical chance
- x = 2.0: 5% critical chance
- x = 3.0+: 10% critical chance (cap)

### Quality Decay Over Time

Items degrade with use:

```text
current_quality = original_quality · (current_durability / max_durability)^0.5
```text

**Interpretation:**
- At 100% durability: 100% original quality
- At 50% durability: 71% of original quality
- At 25% durability: 50% of original quality
- At 0% durability: Item breaks

## Material Quality System

### Harvested Material Quality

When gathering materials, quality is determined by:

```text
material_quality = base_quality · (1 + gathering_skill/200) · node_richness

where:
- base_quality = 50% (standard)
- gathering_skill = player's gathering skill level
- node_richness = 0.5 to 1.5 (varies by resource node)
```text

### Material Quality Grades

| Grade | Quality Range | Gathering Requirement |
|-------|--------------|----------------------|
| Poor | 1% - 35% | Any skill |
| Standard | 36% - 65% | Skill 25+ |
| Premium | 66% - 85% | Skill 50+ |
| Exceptional | 86% - 100% | Skill 75+ |

### Material Selection Strategy

Players can select which materials to use:

**Conservative Approach:**
- Use standard materials for practice
- Save premium materials for important crafts

**Optimal Approach:**
- Match material quality to target item tier
- Use premium materials when near quality breakpoints

**Luxury Approach:**
- Always use best materials
- Guarantees high-tier outcomes (but expensive)

## Quality Visibility to Players

### Pre-Craft Information

Before attempting a craft, players see:

```text
Expected Quality Range: [q_min, q_max]

where:
q_min = q_base + worst_case_modifiers - 10%
q_max = q_base + best_case_modifiers + 10%
```text

### Post-Craft Feedback

After successful craft:

```text
Base Quality:        x · 100%        [70%]
Material Bonus:      +Δq_material    [+15%]
Specialization:      +Δq_spec        [+15%]
Tool Quality:        +Δq_tools       [+8%]
Random Variation:    +Δq_random      [+2%]
────────────────────────────────────────────
Final Quality:       q_final         [110% → 100%]

Result: MASTERWORK STEEL SWORD
```text

## Implementation Example

```csharp
public class QualityCalculator
{
    public float CalculateQuality(CraftingContext context)
    {
        float baseQuality = context.RelativeSkill * 100f;
        
        // Material modifier
        float materialBonus = CalculateMaterialBonus(context.Materials);
        
        // Random variation
        float randomVariation = Random.Range(-10f, 10f);
        
        // Specialization bonus
        float specializationBonus = context.HasSpecialization ? 15f : 0f;
        
        // Tool quality bonus
        float toolBonus = (context.ToolQuality / 100f) * 10f;
        
        // Check for critical success
        if (RollCriticalSuccess(context.RelativeSkill))
        {
            return 100f; // Masterwork guaranteed
        }
        
        // Calculate final quality
        float finalQuality = baseQuality + materialBonus + randomVariation + 
                            specializationBonus + toolBonus;
        
        return Mathf.Clamp(finalQuality, 1f, 100f);
    }
    
    private float CalculateMaterialBonus(List<Material> materials)
    {
        if (materials.Count == 0) return 0f;
        
        float averageQuality = materials.Average(m => m.Quality);
        return (averageQuality - 50f) * 0.5f;
    }
    
    private bool RollCriticalSuccess(float relativeSkill)
    {
        float critChance = Mathf.Min((relativeSkill - 1f) * 0.05f, 0.1f);
        return Random.Range(0f, 1f) < critChance;
    }
}

public class CraftingContext
{
    public float PlayerSkill { get; set; }
    public float RecipeSkill { get; set; }
    public float RelativeSkill => PlayerSkill / RecipeSkill;
    public List<Material> Materials { get; set; }
    public bool HasSpecialization { get; set; }
    public float ToolQuality { get; set; }
}

public class Material
{
    public string Name { get; set; }
    public float Quality { get; set; } // 0-100
    public MaterialGrade Grade { get; set; }
}

public enum MaterialGrade
{
    Poor,
    Standard,
    Premium,
    Exceptional
}
```text

## Balancing Considerations

### Quality Distribution Goals

At different skill levels, target quality distributions:

**Below Recipe Skill (x = 0.5):**
- 60% Common tier
- 30% Good tier
- 10% Fine tier

**At Recipe Skill (x = 1.0):**
- 20% Good tier
- 50% Fine tier
- 25% Superior tier
- 5% Masterwork tier

**Above Recipe Skill (x = 1.5):**
- 40% Superior tier
- 50% Masterwork tier
- 10% Critical Success bonus

### Economic Impact

Quality tiers should reflect value:

```text
Vendor Price = base_price · (1 + quality/100)^2

Example (Base Price = 100 gold):
- Common (50%): 225 gold
- Fine (80%): 324 gold
- Masterwork (100%): 400 gold
```text

## Testing Requirements

### Quality Distribution Tests

1. **Skill Level Tests:** Verify quality distributions at x = 0.5, 1.0, 1.5, 2.0
2. **Material Impact Tests:** Confirm material quality affects outcomes as expected
3. **Specialization Tests:** Verify bonuses apply correctly
4. **Critical Success Tests:** Confirm rates match formula

### Player Experience Tests

1. **Clarity:** Players understand quality predictions
2. **Fairness:** Quality outcomes feel appropriate for skill investment
3. **Progression:** Quality improves noticeably as skills increase
4. **Excitement:** High-quality crafts feel rewarding

## Related Documentation

- [Crafting Success Model](./crafting-success-model.md) - Success/failure mechanics
- [Crafting Mechanics Overview](./crafting-mechanics-overview.md) - System overview
- [Economy Systems](../../systems/economy-systems.md) - Economic balance
- [Skill Knowledge System](../../../research/game-design/skill-knowledge-system-research.md) - Skill progression
- [Life is Feudal Material System Research](../../../research/game-design/life-is-feudal-material-system-research.md) - Material quality and crafting analysis from proven MMORPG systems
- [Vintage Story Material System Research](../../../research/game-design/vintage-story-material-system-research.md) - Material quality and geological integration analysis

## Version History

- **1.0** (2025-01-15): Initial specification for quality calculation system
