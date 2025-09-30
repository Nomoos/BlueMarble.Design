# Crafting Success Model - Formal Mathematical Specification

**Document Type:** Mechanics Specification  
**Version:** 1.0  
**Author:** Game Design Team  
**Date:** 2025-01-15  
**Status:** Proposed

## Overview

This document defines the formal mathematical model for crafting success in BlueMarble. The model is designed
to provide smooth, predictable progression without arbitrary hard thresholds, while maintaining meaningful
challenge and skill-based gameplay.

## Design Philosophy

The crafting system is built on these core principles:

- **No Hard Thresholds:** Success rates change smoothly based on skill levels
- **Skill-Based Progression:** Players with higher skills have better outcomes
- **Material Preservation:** Low failure rates mean materials aren't frequently wasted
- **Smooth Learning Curve:** Gradual difficulty scaling encourages experimentation

## Notation and Variables

### Core Variables

- **s** — Current player skill level (in the specific discipline)
- **r** — Recommended/required skill level for the recipe (recipe difficulty)
- **x = s / r** — Relative skill (dimensionless ratio)
- **U(0,1)** — Uniform random number on interval [0,1]
- **clamp(z, a, b)** — Clamp value z to interval [a, b]

### Constants

- **γ (gamma)** — Curve steepness parameter for low-skill failure rate (suggested: γ = 2)
- **ε (epsilon)** — Base failure rate at skill threshold (suggested: ε = 0.01, or 1%)
- **k** — Decay rate for failure probability above threshold (suggested: k = 2)

## Crafting Attempt Rules

### 1. Minimum Skill Requirement

**Rule:** "Without any skill, you cannot craft the recipe."

**Condition:**

```text
Crafting attempt allowed if and only if: s > 0
```text


This means the player must have at least some level of the skill (even if very low) to attempt the recipe.

### 2. Failure Probability Model

**Rule:** "From quarter skill level onwards, the chance of failure is minimal (nearly zero), and upon
failure you don't lose materials. At very low skill levels, failures are frequent."

The failure probability function uses a smooth transition at x = 0.25 (25% of recipe skill):

```text
p_fail(x) = {
    1 - (x / 0.25)^γ,                    if 0 ≤ x < 0.25
    ε · e^(-k(x - 0.25)),                if x ≥ 0.25
}
```text

#### Low Skill Region (0 ≤ x < 0.25)

When player skill is less than 25% of recipe requirement:
- At x = 0: p_fail = 1.0 (100% failure rate)
- As x approaches 0.25: p_fail approaches 0
- The curve is smooth and controlled by parameter γ

**Example values (with γ = 2):**
```text
x = 0.10  →  p_fail = 0.84  (84% failure rate)
x = 0.15  →  p_fail = 0.64  (64% failure rate)
x = 0.20  →  p_fail = 0.36  (36% failure rate)
x = 0.24  →  p_fail = 0.08  (8% failure rate)
```text

#### High Skill Region (x ≥ 0.25)

When player skill reaches or exceeds 25% of recipe requirement:
- Failure rate drops exponentially
- At x = 0.25: p_fail = ε (e.g., 1%)
- As x increases: p_fail approaches 0 asymptotically

**Example values (with ε = 0.01, k = 2):**
```text
x = 0.25  →  p_fail = 0.010  (1.0% failure rate)
x = 0.50  →  p_fail = 0.006  (0.6% failure rate)
x = 1.00  →  p_fail = 0.001  (0.1% failure rate)
x = 2.00  →  p_fail ≈ 0      (nearly 0%)
```text

### 3. Material Loss on Failure

**Rule:** When a crafting attempt fails, materials are NOT lost.

This encourages experimentation and reduces frustration, especially for newer players learning the system.

**Implementation Note:**
```text
if (Random(0,1) < p_fail(x)):
    // Attempt failed
    return CraftingResult.Failed
    // Materials remain in inventory
else:
    // Attempt succeeded, proceed to quality calculation
    // Materials are consumed
```text

## Quality Outcome Model

### Quality Range Calculation

When a crafting attempt succeeds, the quality of the resulting item is determined by:

```text
quality_base = x · 100%
quality_variation = Random(-10%, +10%)
quality_final = clamp(quality_base + quality_variation, 1%, 100%)
```text

**Interpretation:**
- **Below Recipe Skill (x < 1):** Items tend to have lower quality
- **At Recipe Skill (x = 1):** Items average around 100% quality with variation
- **Above Recipe Skill (x > 1):** Consistently high-quality items

### Quality Tiers

Quality values map to item tiers:

```text
Poor:      1% - 25%    (Gray)
Common:    26% - 50%   (White)
Good:      51% - 70%   (Green)
Fine:      71% - 85%   (Blue)
Superior:  86% - 95%   (Purple)
Masterwork: 96% - 100% (Orange)
```text

## Parameter Tuning Guidelines

### Adjusting Failure Curve Steepness (γ)

- **Higher γ** (e.g., 3-4): More forgiving at low skills, steeper ramp at threshold
- **Lower γ** (e.g., 1.5): Harsher penalty for low skills, gentler transition

### Adjusting Base Failure Rate (ε)

- **Higher ε** (e.g., 0.05): More failures even at reasonable skill levels
- **Lower ε** (e.g., 0.001): Nearly perfect success once threshold is reached

### Adjusting Decay Rate (k)

- **Higher k** (e.g., 4): Faster drop in failure rate as skill increases
- **Lower k** (e.g., 1): Slower improvement with skill gains

## Implementation Pseudo-code

```csharp
public class CraftingSystem
{
    // Configuration constants
    private const float SKILL_THRESHOLD = 0.25f;
    private const float GAMMA = 2.0f;
    private const float EPSILON = 0.01f;
    private const float DECAY_RATE = 2.0f;

    public CraftingResult AttemptCraft(float playerSkill, float recipeSkill)
    {
        // Rule 1: Check minimum skill requirement
        if (playerSkill <= 0)
        {
            return CraftingResult.InsufficientSkill;
        }

        // Calculate relative skill
        float x = playerSkill / recipeSkill;

        // Calculate failure probability
        float failureChance = CalculateFailureChance(x);

        // Roll for success/failure
        float roll = Random.Range(0f, 1f);
        if (roll < failureChance)
        {
            // Failed attempt - materials preserved
            return CraftingResult.Failed;
        }

        // Success! Calculate quality
        float quality = CalculateQuality(x);
        return new CraftingResult.Success(quality);
    }

    private float CalculateFailureChance(float x)
    {
        if (x < SKILL_THRESHOLD)
        {
            // Low skill region: polynomial curve
            float ratio = x / SKILL_THRESHOLD;
            return 1.0f - Mathf.Pow(ratio, GAMMA);
        }
        else
        {
            // High skill region: exponential decay
            float exponent = -DECAY_RATE * (x - SKILL_THRESHOLD);
            return EPSILON * Mathf.Exp(exponent);
        }
    }

    private float CalculateQuality(float x)
    {
        float baseQuality = x * 100f;
        float variation = Random.Range(-10f, 10f);
        return Mathf.Clamp(baseQuality + variation, 1f, 100f);
    }
}
```text

## Gameplay Examples

### Example 1: Novice Crafter

- Player skill: s = 5
- Recipe difficulty: r = 50
- Relative skill: x = 5/50 = 0.10

**Failure rate:** ~84%
**Strategy:** Practice on easier recipes first, or accept high failure rate for valuable recipes

### Example 2: Apprentice Crafter

- Player skill: s = 15
- Recipe difficulty: r = 50
- Relative skill: x = 15/50 = 0.30

**Failure rate:** ~0.4%
**Expected quality:** 30% ± 10% → Common/Good tier items
**Strategy:** Reasonable success rate, but quality is limited

### Example 3: Journeyman Crafter

- Player skill: s = 50
- Recipe difficulty: r = 50
- Relative skill: x = 50/50 = 1.00

**Failure rate:** ~0.1%
**Expected quality:** 100% ± 10% → Superior/Masterwork tier items
**Strategy:** Reliable crafting at recommended skill level

### Example 4: Master Crafter

- Player skill: s = 100
- Recipe difficulty: r = 50
- Relative skill: x = 100/50 = 2.00

**Failure rate:** ≈0%
**Expected quality:** 100% (capped) → Consistently Masterwork items
**Strategy:** Trivial success, guaranteed high quality

## Testing and Validation

### Unit Test Coverage

The following scenarios should be tested:

1. **Boundary Conditions:**
   - x = 0 (no skill)
   - x = 0.24 (just below threshold)
   - x = 0.25 (at threshold)
   - x = 0.26 (just above threshold)
   - x = 1.0 (exact match)
   - x > 2.0 (high mastery)

2. **Failure Rate Distribution:**
   - Run 10,000 attempts at various skill levels
   - Verify failure rates match mathematical expectations
   - Ensure smooth transitions at threshold

3. **Quality Distribution:**
   - Verify quality centers around expected values
   - Check quality caps at 100%
   - Confirm variation range is correct

### Balance Testing

Monitor these metrics during playtesting:

- Average attempts needed per successful craft at different skill levels
- Material consumption rates
- Player progression speed through crafting tiers
- Player satisfaction with failure/success rates

## Related Documentation

- [Crafting Quality Model](./crafting-quality-model.md) - Detailed quality calculation
- [Crafting Mechanics Overview](./crafting-mechanics-overview.md) - High-level system design
- [Economy Systems](../../systems/economy-systems.md) - Economic integration
- [Skill Knowledge System](../../../research/game-design/skill-knowledge-system-research.md) - Skill progression

## Version History

- **1.0** (2025-01-15): Initial formal specification, translated from Czech original
