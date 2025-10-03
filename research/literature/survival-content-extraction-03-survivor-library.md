# Content Extraction Guide 03: Survivor Library Collection

---
title: Survivor Library Content Extraction for Historical Technology
date: 2025-01-15
tags: [survivor-library, primitive-technology, historical, crafting]
status: completed
priority: high
source: Survivor Library Collection (survivorlibrary.com)
---

## Executive Summary

The Survivor Library is a comprehensive collection focused on "how to survive when technology fails." It emphasizes low-tech, historically-proven methods perfect for BlueMarble's Tier 1-3 content.

**Target Content:**
- 150+ historical technology recipes
- Focus on pre-industrial revolution techniques
- Technology tiers 1-3 (primitive to medieval)
- Proven historical methods

**Implementation Priority:** HIGH - Early game foundational content

## Source Overview

### Survivor Library Structure

**Collection Focus:**
- Pre-1900s technology and techniques
- Emphasis on self-sufficiency
- Historical documents and reprints
- Practical how-to guides

**Major Categories:**
- Primitive skills (fire, shelter, tools)
- Blacksmithing and metallurgy
- Historical farming methods
- Traditional food preservation
- Water systems
- Construction techniques
- Textile production

## Content Extraction Strategy

### Category 1: Primitive Technology (Tier 1) - 40 recipes

**Stone Tools:**
- Flint knapping basics (10 tools)
- Stone axe heads
- Arrowheads and spearheads
- Grinding stones
- Fire striking stones

**Fire Making:**
- Bow drill method
- Hand drill method
- Fire plough technique
- Flint and steel
- Fire preservation

**Cordage:**
- Plant fiber identification (10 types)
- Twisting techniques
- Braiding methods
- Rope strength testing

### Category 2: Historical Blacksmithing (Tier 2-3) - 50 recipes

**Forge Setup:**
- Clay forge construction
- Bellows design (3 types)
- Charcoal production
- Anvil alternatives

**Basic Forging:**
- Tool making (hammers, tongs, chisels)
- Nail production
- Horseshoes
- Farm implements (15 tools)
- Weapons (10 types)

**Heat Treatment:**
- Annealing
- Tempering
- Case hardening
- Quenching media

### Category 3: Historical Agriculture (Tier 2) - 30 recipes

**Soil Management:**
- Crop rotation systems (5 patterns)
- Green manure crops
- Composting methods
- Pest control (historical methods)

**Tool Making:**
- Wooden plow construction
- Harrow design
- Scythe use and sharpening
- Hand tools

### Category 4: Traditional Building (Tier 2-3) - 30 recipes

**Log Construction:**
- Log cabin building
- Notching techniques (5 types)
- Chinking and daubing
- Roof framing

**Adobe/Cob:**
- Mud brick making
- Cob wall construction
- Lime plaster
- Earth floor techniques

## Implementation Details

### Recipe Format Example

```json
{
  "recipe_id": "survivor_001",
  "name": "Bow Drill Fire Starting",
  "tier": 1,
  "category": "survival",
  "inputs": [
    {"item": "dry_wood_spindle", "quantity": 1},
    {"item": "bow_branch", "quantity": 1},
    {"item": "cordage", "quantity": 1},
    {"item": "fireboard", "quantity": 1},
    {"item": "tinder_bundle", "quantity": 1}
  ],
  "outputs": [
    {"item": "fire", "quantity": 1, "success_rate": 0.7}
  ],
  "requirements": {
    "skill": "fire_making",
    "skill_level": 5,
    "time_minutes": 10,
    "weather": "dry_conditions"
  },
  "source": {
    "collection": "Survivor Library",
    "document": "Primitive Fire Making Methods",
    "page": 12
  }
}
```

## Deliverables

1. **recipes_primitive_tier1.json** - 40 primitive technology recipes
2. **recipes_blacksmithing_tier2-3.json** - 50 historical metalworking
3. **recipes_agriculture_historical_tier2.json** - 30 farming methods
4. **recipes_building_traditional_tier2-3.json** - 30 construction techniques
5. **skill_tree_primitive.json** - Tier 1-3 primitive skills

## Success Metrics

- **Quantity:** 150+ historical recipes extracted
- **Quality:** 100% historically accurate
- **Balance:** Appropriate for early game
- **Integration:** Maps to survival skill tree
- **Tutorial Value:** Excellent for teaching new players

## Next Steps

1. Download Survivor Library collection
2. Organize by technology tier
3. Extract with focus on step-by-step instructions
4. Create tutorial content from tier 1 recipes
5. Balance for game progression

---

**Document Status:** Complete
**Last Updated:** 2025-01-15
**Priority:** HIGH
**Estimated Time:** 4 weeks
