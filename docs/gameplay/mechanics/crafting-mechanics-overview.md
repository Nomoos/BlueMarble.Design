# Crafting Mechanics Overview

**Document Type:** System Overview  
**Version:** 1.0  
**Author:** Game Design Team  
**Date:** 2025-01-15  
**Status:** Proposed

## Introduction

This document provides a high-level overview of BlueMarble's crafting mechanics system. The system is designed
to be skill-based, rewarding player investment while maintaining accessibility and fun gameplay.

## Core Design Principles

### 1. Smooth Progression

The crafting system avoids hard thresholds and arbitrary cutoffs. Instead, it uses smooth mathematical curves
that provide gradual improvements as players increase their skills.

**Benefits:**

- No frustrating "brick walls" in progression
- Every skill point provides tangible benefit
- Players can attempt challenging recipes early (with higher risk)

### 2. Skill-Based Success

Success in crafting is primarily determined by the relationship between player skill and recipe difficulty.
The system uses a relative skill calculation (player skill / recipe skill) to determine outcomes.

**Key Features:**

- Low skill relative to recipe: High failure rates
- Moderate skill (25%+ of recipe): Very low failure rates
- High skill: Guaranteed success with quality bonuses

### 3. Material Preservation

Failed crafting attempts do NOT consume materials. This encourages:

- Experimentation with new recipes
- Learning through practice
- Reduced frustration for new crafters
- Risk-taking for valuable recipes

### 4. Quality Variation

Successful crafts produce items of varying quality based on:

- Player skill level
- Material quality
- Tool quality
- Specialization bonuses
- Random variation

## System Components

### 1. Success Model

Determines whether a crafting attempt succeeds or fails.

**Key Formula:**

```text
Success Rate = 1 - p_fail(x)

where x = player_skill / recipe_skill
```

**Detailed Documentation:** [Crafting Success Model](./crafting-success-model.md)

### 2. Quality Model

Determines the tier and effectiveness of successfully crafted items.

**Key Formula:**

```text
Quality = base_quality + material_bonus + tool_bonus + spec_bonus + random_variation
```

**Detailed Documentation:** [Crafting Quality Model](./crafting-quality-model.md)

### 3. Skill Progression

Players improve their crafting skills through practice and successful crafts.

**Experience Gain:**

- Successful crafts: Base XP × difficulty multiplier
- Failed attempts: Reduced XP (learning experience)
- Critical successes: Bonus XP

**Detailed Documentation:** [Skill Knowledge System](../../../research/game-design/skill-knowledge-system-research.md)

## Crafting Workflow

### Step 1: Recipe Selection

Player selects a recipe to craft:

```text
╔═══════════════════════════════════════╗
║  Select Recipe                        ║
║  ─────────────────────────────────    ║
║  [⚔] Steel Longsword                 ║
║      Required Skill: 35               ║
║      Your Skill: 42                   ║
║      Success Rate: ~99% 🟢            ║
║      Expected Quality: Fine-Superior  ║
╚═══════════════════════════════════════╝
```text

### Step 2: Material Selection

Player chooses which materials to use:

```text
╔═══════════════════════════════════════╗
║  Materials Required                   ║
║  ─────────────────────────────────    ║
║  Steel Ingot × 4                      ║
║  ☑ Premium (85%) [Selected]           ║
║  ☐ Standard (65%)                     ║
║  ☐ Poor (45%)                         ║
║                                       ║
║  Effect: +17.5% quality bonus         ║
╚═══════════════════════════════════════╝
```text

### Step 3: Crafting Attempt

System rolls for success and calculates quality:

```text
Rolling for success... ✓ SUCCESS
Calculating quality...

Base Quality:       120% (Skill 42/35)
Material Bonus:     +17.5%
Random Variation:   +3%
Specialization:     +15%
Tool Bonus:         +8%
────────────────────────────────────
Final Quality:      163.5% → 100% (capped)

Result: MASTERWORK STEEL LONGSWORD
```text

### Step 4: Result Display

Player receives the crafted item with feedback:

```text
╔═══════════════════════════════════════╗
║  🎉 Crafting Successful!              ║
║  ─────────────────────────────────    ║
║  Created: Masterwork Steel Longsword  ║
║  Quality: 100%                        ║
║  Tier: Legendary                      ║
║                                       ║
║  +150 Blacksmithing XP                ║
║  Level progress: ████░░░ 78%          ║
╚═══════════════════════════════════════╝
```text

## Crafting Professions

### Available Disciplines

1. **Blacksmithing** - Weapons, armor, metal items
2. **Alchemy** - Potions, elixirs, magical components
3. **Tailoring** - Clothing, light armor, bags
4. **Engineering** - Mechanical devices, tools
5. **Cooking** - Food, buffs, consumables
6. **Woodworking** - Wooden items, bows, furniture
7. **Jewelcrafting** - Accessories, gems, enchantments

### Specialization System

Players can specialize within each profession:

**Blacksmithing Specializations:**
- Weaponsmith (+15% weapon quality)
- Armorsmith (+15% armor quality)
- Toolsmith (+15% tool quality)

**Benefits:**
- Quality bonus for specialized items
- Reduced material costs (10-20%)
- Access to unique recipes
- Recognition in community

## Material Quality System

### Gathering Materials

Material quality depends on:
- Gathering skill level
- Resource node richness
- Tools used
- Random variation

### Material Grades

| Grade | Quality | Gathering Skill | Market Value |
|-------|---------|----------------|--------------|
| Poor | 1-35% | Any | Low |
| Standard | 36-65% | 25+ | Medium |
| Premium | 66-85% | 50+ | High |
| Exceptional | 86-100% | 75+ | Very High |

### Material Impact

Better materials provide:
- Higher quality outcomes
- More consistent results
- Better item attributes
- Higher market value

## Progression Path

### Novice Crafter (Skill 1-25)

**Characteristics:**
- Learning basic recipes
- High failure rates on medium recipes
- Low quality outputs
- Building foundation

**Strategy:**
- Focus on easy recipes (skill 10-15)
- Use standard materials
- Practice frequently
- Learn from failures

### Apprentice Crafter (Skill 26-50)

**Characteristics:**
- Moderate success rates
- Access to intermediate recipes
- Good quality outputs
- Developing specialization

**Strategy:**
- Attempt medium recipes (skill 30-40)
- Invest in premium materials for important crafts
- Consider specialization
- Build reputation

### Journeyman Crafter (Skill 51-75)

**Characteristics:**
- High success rates
- Advanced recipes available
- Fine to Superior quality typical
- Established specialization

**Strategy:**
- Craft advanced items (skill 50-70)
- Use premium materials regularly
- Take custom orders
- Mentor new crafters

### Master Crafter (Skill 76-100)

**Characteristics:**
- Nearly guaranteed success
- All recipes accessible
- Consistently high quality
- Renowned expertise

**Strategy:**
- Create masterwork items
- Use exceptional materials
- Charge premium prices
- Pursue legendary recipes

## Economic Integration

### Market Dynamics

Crafted item value depends on:

```text
Market Price = base_value · quality_multiplier · rarity_multiplier · demand_factor

where:
quality_multiplier = (1 + quality/100)^2
rarity_multiplier = 1.0 (Common) to 10.0 (Legendary)
demand_factor = 0.5 (oversaturated) to 2.0 (high demand)
```text

### Crafting as Profession

**Income Sources:**
- Direct sales of crafted items
- Custom commissions
- Repair services
- Teaching/mentoring

**Costs:**
- Material purchases
- Tool maintenance
- Workshop rental
- Recipe acquisition

## Social Systems

### Crafter Reputation

Players build reputation through:
- Quality of crafted items
- Reliability in fulfilling orders
- Innovation in techniques
- Community contributions

### Guilds and Collectives

Crafters can join:
- Profession-specific guilds
- Trading companies
- Crafting collectives
- Master-apprentice relationships

## User Interface Considerations

### Information Display

Players need clear information about:
- Current skill level
- Recipe requirements
- Success rate estimates
- Expected quality range
- Material impact
- Cost breakdown

### Feedback Systems

Provide immediate feedback on:
- Successful crafts (celebratory)
- Failed attempts (encouraging)
- Skill improvements (motivating)
- Quality achievements (rewarding)

## Balance Considerations

### Success Rate Targets

**Design Goals:**
```text
At 25% of recipe skill:   ~0% failure rate
At 50% of recipe skill:   ~0.6% failure rate
At 100% of recipe skill:  ~0.1% failure rate
At 200% of recipe skill:  ~0% failure rate
```text

### Quality Distribution Targets

**At Recipe Skill Level:**
- 20% Good tier
- 50% Fine tier
- 25% Superior tier
- 5% Masterwork tier

### Experience Gain Rates

**Target Progression Time:**
- Novice to Apprentice: 10-15 hours
- Apprentice to Journeyman: 30-40 hours
- Journeyman to Master: 80-100 hours

## Testing Requirements

### Functional Testing

1. Success rate calculations match mathematical model
2. Quality calculations produce expected distributions
3. Material quality correctly affects outcomes
4. Specialization bonuses apply properly
5. UI displays accurate information

### Balance Testing

1. Progression feels appropriately paced
2. Material costs are balanced with outcomes
3. Failure rates feel fair
4. Quality variation provides excitement without frustration
5. Economic value matches effort invested

### User Experience Testing

1. Players understand the system
2. Feedback is clear and helpful
3. Interface is intuitive
4. Progression feels rewarding
5. System encourages desired behaviors

## Related Documentation

### Core System Documentation

- **[Crafting Success Model](./crafting-success-model.md)** - Mathematical model for success rates
- **[Crafting Quality Model](./crafting-quality-model.md)** - Quality calculation system
- **[Economy Systems](../../systems/economy-systems.md)** - Economic integration
- **[Skill Knowledge System](../../../research/game-design/skill-knowledge-system-research.md)** - Skill progression

### Interface Documentation

- **[Crafting Interface Mockups](../../../research/game-design/assets/crafting-interface-mockups.md)** - UI designs

### Related Systems

- **[Gameplay Systems](../../systems/gameplay-systems.md)** - Overall game systems
- **[Player Progression](../spec-player-progression-system.md)** - Character advancement

## Version History

- **1.0** (2025-01-15): Initial overview document consolidating crafting mechanics

## Future Enhancements

### Potential Additions

1. **Recipe Discovery System** - Finding and learning new recipes
2. **Experimentation Mode** - Creating custom variations
3. **Masterwork Projects** - Long-term crafting challenges
4. **Crafting Events** - Community crafting competitions
5. **Historical Records** - Tracking legendary crafts

### Under Consideration

1. **Multi-stage Crafting** - Complex items requiring multiple steps
2. **Collaborative Crafting** - Multiple players working together
3. **Dynamic Recipes** - Recipes that adapt to materials used
4. **Crafting Minigames** - Interactive crafting experiences
