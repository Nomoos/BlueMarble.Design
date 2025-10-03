# Content Extraction: Appropriate Technology Library for Crafting Systems

---
title: Appropriate Technology Library Content Extraction for BlueMarble Crafting
date: 2025-01-15
tags: [appropriate-technology, crafting, recipes, sustainable, implementation]
status: active
priority: 2 - Immediate Implementation
source: Appropriate Technology Library (1,050 ebooks)
parent-research: survival-guides-knowledge-domains-research.md
---

**Document Type:** Content Extraction Guide  
**Version:** 1.0  
**Author:** Game Design Research Team  
**Date:** 2025-01-15  
**Priority:** Critical - Phase 1 Implementation  
**Implementation Status:** Ready for Content Team

## Overview

This document provides a systematic approach to extracting crafting recipes, material processing chains, and 
sustainable technology knowledge from the Appropriate Technology Library (1,050 ebooks) for implementation in 
BlueMarble's crafting and skill systems. The library focuses on sustainable, low-tech solutions ideal for 
early-to-mid game content.

## Source Information

### Appropriate Technology Library

**Source:** Magnet link from awesome-survival repository
- **Collection Size:** 1,050 ebooks
- **Focus:** Sustainable living, low-technology solutions, self-sufficiency
- **Content Types:** Manuals, guides, technical documentation, how-to books
- **Technology Level:** Primitive to early industrial (perfect for game progression)
- **License:** Various (verify individual book licenses)

**Key Topic Areas:**
- Agriculture and food production
- Water management and purification
- Shelter construction techniques
- Tool making and metalworking
- Energy systems (solar, wind, water)
- Food preservation methods
- Textile production
- Basic chemistry and materials
- Waste management
- Health and sanitation

## Content Extraction Strategy

### Phase 1: Recipe Database Creation (Week 1-3)

**Objective:** Extract 500+ actionable crafting recipes from the library

**Priority Categories:**

#### Category 1: Basic Survival Tools (100 recipes)
**Target Books:**
- "Village Technology Handbook"
- "Where There Is No Blacksmith"
- "Basic Tool Making"

**Recipes to Extract:**
- Stone tools (axes, knives, scrapers)
- Wooden tools (handles, mallets, spears)
- Rope and cordage making
- Basic containers (bark, woven, clay)
- Fire starting tools
- Simple traps and snares

**Recipe Format:**
```json
{
    "recipe_id": "primitive_stone_axe_001",
    "recipe_name": "Stone Hand Axe",
    "category": "tools",
    "subcategory": "primitive",
    "tier": 1,
    "skill_required": "primitive_tools",
    "skill_level": 1,
    "crafting_time": 30,
    "materials": [
        {"item": "stone", "quantity": 1, "quality": "hard"},
        {"item": "wood_branch", "quantity": 1, "quality": "sturdy"},
        {"item": "plant_fiber", "quantity": 5}
    ],
    "tools_required": [
        {"tool": "hands", "durability_cost": 0}
    ],
    "output": [
        {"item": "stone_axe", "quantity": 1, "quality_range": [0.3, 0.7]}
    ],
    "failure_chance": 0.2,
    "xp_gain": 10,
    "description": "A simple stone axe created by lashing a sharp stone to a wooden handle with plant fibers.",
    "source_book": "Village Technology Handbook",
    "source_page": 47,
    "real_world_authentic": true
}
```

#### Category 2: Agriculture and Food Production (120 recipes)
**Target Books:**
- "Small Farm Handbook"
- "Gardening When It Counts"
- "Food from Dryland Gardens"
- "Low External Input Sustainable Agriculture"

**Recipes to Extract:**
- Soil preparation techniques
- Seed saving and storage
- Crop planting methods
- Irrigation systems
- Composting processes
- Pest control methods
- Harvest techniques
- Food preservation (drying, smoking, fermenting)

**Example Recipe:**
```json
{
    "recipe_id": "agriculture_compost_pile_001",
    "recipe_name": "Basic Compost Pile",
    "category": "agriculture",
    "subcategory": "soil_improvement",
    "tier": 2,
    "skill_required": "farming",
    "skill_level": 2,
    "crafting_time": 300,
    "time_to_complete": 2592000,
    "materials": [
        {"item": "plant_waste", "quantity": 50},
        {"item": "manure", "quantity": 10},
        {"item": "water", "quantity": 20},
        {"item": "soil", "quantity": 5}
    ],
    "tools_required": [
        {"tool": "shovel", "durability_cost": 5}
    ],
    "structure_required": "compost_bin",
    "output": [
        {"item": "compost", "quantity": 40, "quality_range": [0.5, 0.9]}
    ],
    "failure_chance": 0.1,
    "xp_gain": 25,
    "description": "Layered organic materials decompose over time to create nutrient-rich compost.",
    "source_book": "Small Farm Handbook",
    "source_page": 112
}
```

#### Category 3: Shelter Construction (80 recipes)
**Target Books:**
- "Shelter" by Lloyd Kahn
- "Building with Earth"
- "Thatching Guide"
- "Small Building Construction"

**Recipes to Extract:**
- Foundation techniques
- Wall construction (mud, wood, stone)
- Roof structures
- Door and window frames
- Waterproofing methods
- Insulation techniques
- Flooring options

#### Category 4: Metalworking and Smithing (90 recipes)
**Target Books:**
- "Where There Is No Blacksmith"
- "Small-Scale Iron Smelting"
- "Metalworking for the Home Shop"

**Recipes to Extract:**
- Ore smelting processes
- Basic forging techniques
- Tool sharpening and maintenance
- Metal joining (riveting, welding)
- Casting methods
- Heat treatment processes

**Example Recipe:**
```json
{
    "recipe_id": "metalwork_iron_smelting_001",
    "recipe_name": "Bloomery Iron Smelting",
    "category": "metalworking",
    "subcategory": "smelting",
    "tier": 3,
    "skill_required": "metallurgy",
    "skill_level": 3,
    "crafting_time": 600,
    "materials": [
        {"item": "iron_ore", "quantity": 10},
        {"item": "charcoal", "quantity": 20},
        {"item": "limestone", "quantity": 2}
    ],
    "tools_required": [
        {"tool": "bloomery_furnace", "durability_cost": 10},
        {"tool": "bellows", "durability_cost": 5}
    ],
    "temperature_required": 1200,
    "output": [
        {"item": "iron_bloom", "quantity": 1, "quality_range": [0.4, 0.8]},
        {"item": "slag", "quantity": 3}
    ],
    "failure_chance": 0.15,
    "xp_gain": 50,
    "description": "Traditional bloomery process extracts iron from ore using charcoal fuel.",
    "source_book": "Small-Scale Iron Smelting",
    "source_page": 23,
    "notes": "Multiple heating cycles may improve iron quality"
}
```

#### Category 5: Water Systems (40 recipes)
**Target Books:**
- "Water for Every Farm"
- "Rainwater Harvesting"
- "Well Drilling Handbook"
- "Water Purification Methods"

**Recipes to Extract:**
- Well construction
- Rainwater collection systems
- Water filtration methods
- Purification techniques
- Irrigation systems
- Water storage solutions

#### Category 6: Energy Systems (50 recipes)
**Target Books:**
- "Solar Cooker Plans"
- "Micro-Hydro Design Manual"
- "Windmill Building Guide"
- "Biogas Technology"

**Recipes to Extract:**
- Solar cooker construction
- Water wheel designs
- Windmill plans
- Biogas digester systems
- Simple battery construction

#### Category 7: Food Preservation (70 recipes)
**Target Books:**
- "Keeping Food Fresh"
- "Root Cellaring"
- "Drying and Smoking"
- "Fermentation Handbook"

**Recipes to Extract:**
- Drying methods (sun, smoke, air)
- Salting and curing
- Fermentation processes
- Pickling techniques
- Cold storage construction
- Canning procedures

## Extraction Methodology

### Step 1: Digital Processing (Week 1)

**Tools Needed:**
- PDF text extraction tools (pdftotext, Adobe Acrobat)
- OCR software for scanned documents
- Text processing scripts
- Database system for storage

**Process:**
1. Download and organize 1,050 ebooks
2. Extract text content from PDFs
3. Apply OCR to scanned documents
4. Organize by topic category
5. Create searchable database

### Step 2: Recipe Identification (Week 2)

**Manual Review Process:**
- 2-3 content team members
- Each reviews specific topic categories
- Identify step-by-step processes
- Flag recipes with clear instructions
- Note material requirements
- Document tool needs

**Selection Criteria:**
- Clear, actionable steps
- Defined material inputs and outputs
- Appropriate for game mechanics
- Fits within technology tier system
- Authentic real-world process

### Step 3: Recipe Standardization (Week 3)

**Convert to Game Format:**

**Template Fields:**
- Recipe ID (unique identifier)
- Recipe Name (player-facing)
- Category and Subcategory
- Technology Tier (1-5)
- Skill Required
- Skill Level
- Crafting Time (in-game seconds)
- Materials (with quantities and quality requirements)
- Tools Required (with durability costs)
- Output Items (with quantity and quality ranges)
- Failure Chance
- XP Gain
- Description (lore-friendly)
- Source Attribution
- Real-World Authentication

**Quality Scaling:**
```
Recipe Quality Factors:
- Skill Level: Higher skill = better base quality
- Material Quality: Better inputs = better outputs
- Tool Quality: Better tools = quality bonus
- Environmental Factors: Right conditions help
- Random Variation: Adds unpredictability

Quality Formula:
BaseQuality = 0.3 + (SkillLevel * 0.1)
MaterialBonus = Average(InputQualities) * 0.2
ToolBonus = Average(ToolQualities) * 0.15
EnvironmentBonus = (ConditionsMet ? 0.1 : 0)
RandomFactor = Random(-0.1, +0.1)

FinalQuality = Clamp(BaseQuality + MaterialBonus + ToolBonus + EnvironmentBonus + RandomFactor, 0.0, 1.0)
```

### Step 4: Skill Tree Integration (Week 4)

**Map Recipes to Skill System:**

**Skill Categories:**
1. **Primitive Crafting** (Tier 1)
   - Stone knapping
   - Cordage making
   - Fire starting
   - Basket weaving

2. **Basic Agriculture** (Tier 2)
   - Soil preparation
   - Planting techniques
   - Harvesting methods
   - Food preservation

3. **Construction** (Tier 2-3)
   - Carpentry
   - Masonry
   - Roofing
   - Foundation work

4. **Metalworking** (Tier 3-4)
   - Ore processing
   - Smelting
   - Forging
   - Casting

5. **Advanced Engineering** (Tier 4-5)
   - Mechanical systems
   - Water engineering
   - Energy systems
   - Infrastructure

**Skill Dependencies:**
```
SkillTree {
    PrimitiveCrafting [Tier 1] {
        Prerequisites: None
        UnlocksRecipes: 100 basic recipes
        UnlocksSkills: [BasicAgriculture, Construction]
    }
    
    BasicAgriculture [Tier 2] {
        Prerequisites: [PrimitiveCrafting >= 5]
        UnlocksRecipes: 120 farming recipes
        UnlocksSkills: [AdvancedAgriculture, FoodPreservation]
    }
    
    Construction [Tier 2] {
        Prerequisites: [PrimitiveCrafting >= 5]
        UnlocksRecipes: 80 building recipes
        UnlocksSkills: [AdvancedConstruction, Metalworking]
    }
    
    Metalworking [Tier 3] {
        Prerequisites: [Construction >= 10]
        UnlocksRecipes: 90 metalwork recipes
        UnlocksSkills: [AdvancedMetallurgy, Engineering]
    }
}
```

## Material Chain Mapping

### Tier 1: Natural Resources
**From Appropriate Technology Library:**

```
RawMaterials {
    Stone: "gathered from ground, river beds"
    Wood: "fallen branches, small trees"
    PlantFiber: "from various plants (flax, hemp, bark)"
    Clay: "found near water sources"
    Sand: "riverbeds, beaches, deserts"
}

BasicProcessing {
    Stone → [knapping] → SharpStone
    Wood + SharpStone → [cutting] → WoodPlanks
    PlantFiber → [twisting] → Cordage
    Clay → [shaping + drying] → UnfiredPot
    UnfiredPot + Fire → [firing] → CeramicPot
}
```

### Tier 2: Processed Materials
```
IntermediateMaterials {
    Charcoal: Wood + (low oxygen fire) → Charcoal
    Leather: Hide + (tanning process) → Leather
    Flour: Grain + (grinding) → Flour
    Cloth: Fiber + (spinning + weaving) → Cloth
    Lime: Limestone + (burning) → Lime
}
```

### Tier 3: Advanced Materials
```
AdvancedProcessing {
    IronBloom: IronOre + Charcoal + (bloomery) → IronBloom
    WroughtIron: IronBloom + (repeated heating + hammering) → WroughtIron
    Steel: WroughtIron + Carbon + (crucible smelting) → Steel
    Glass: Sand + Lime + Soda + (high heat) → Glass
    Concrete: Lime + Sand + Gravel + Water → Concrete
}
```

## Implementation Priorities

### Week 1-2: Core Survival Recipes (Phase 1A)
**Priority: Critical**

Extract and implement 100 recipes:
- 25 primitive tools
- 25 basic shelter components
- 25 food gathering/preparation
- 25 fire and water management

**Deliverable:** JSON file with 100 fully specified recipes

### Week 3-4: Agriculture System (Phase 1B)
**Priority: High**

Extract and implement 120 recipes:
- 30 soil preparation and planting
- 30 crop growing techniques
- 30 harvest and storage
- 30 food preservation methods

**Deliverable:** JSON file with 120 farming recipes + agriculture skill tree

### Week 5-6: Construction System (Phase 1C)
**Priority: High**

Extract and implement 80 recipes:
- 20 foundations and flooring
- 20 wall construction
- 20 roof structures
- 20 doors, windows, finishing

**Deliverable:** JSON file with 80 building recipes + construction skill tree

### Week 7-8: Metalworking System (Phase 2A)
**Priority: Medium**

Extract and implement 90 recipes:
- 20 ore processing
- 25 smelting techniques
- 25 forging and shaping
- 20 finishing and treatment

**Deliverable:** JSON file with 90 metalwork recipes + metallurgy skill tree

### Week 9-10: Specialized Systems (Phase 2B)
**Priority: Medium**

Extract and implement 110 recipes:
- 40 water systems
- 50 energy systems
- 20 waste management

**Deliverable:** JSON files for specialized craft systems

## Quality Assurance Process

### Recipe Validation Checklist

For each extracted recipe:
- [ ] Clear step-by-step process documented
- [ ] All materials identified and quantified
- [ ] Tool requirements specified
- [ ] Time estimates realistic for game balance
- [ ] Output quality ranges defined
- [ ] Skill requirements appropriate for tier
- [ ] XP rewards balanced
- [ ] Source attribution complete
- [ ] Real-world authenticity verified
- [ ] Game mechanics integration considered

### Balance Review

**Crafting Time Balance:**
- Tier 1 recipes: 10-60 seconds
- Tier 2 recipes: 60-300 seconds
- Tier 3 recipes: 300-600 seconds
- Tier 4 recipes: 600-1800 seconds
- Tier 5 recipes: 1800+ seconds

**Material Cost Balance:**
- Common materials: Abundant, low cost
- Uncommon materials: Regional availability
- Rare materials: Specific locations only
- Exotic materials: Endgame content

**XP Progression Balance:**
- Tier 1: 5-15 XP per craft
- Tier 2: 15-30 XP per craft
- Tier 3: 30-60 XP per craft
- Tier 4: 60-120 XP per craft
- Tier 5: 120+ XP per craft

## Integration with BlueMarble Systems

### Geological System Integration
- Material quality varies by source geology
- Ore deposits match geological formations
- Clay quality depends on soil composition
- Stone hardness varies by rock type

### Settlement System Integration
- Recipes unlock based on settlement development
- Specialized workshops enable advanced recipes
- Trade brings materials from other regions
- NPC craftsmen teach advanced techniques

### Economic System Integration
- Recipe outputs become trade goods
- Specialized regions produce specific items
- Supply and demand affect crafting choices
- Quality commands premium prices

## Tools and Scripts

### Recipe Extraction Template

**Python Script for Text Processing:**
```python
import json
import re

class RecipeExtractor:
    def __init__(self):
        self.recipes = []
        self.next_id = 1
    
    def extract_from_text(self, text, source_book, category):
        """
        Parse text for recipe-like patterns
        """
        # Look for material lists
        materials_pattern = r"Materials:\s*(.*?)\n\n"
        # Look for step-by-step instructions
        steps_pattern = r"\d+\.\s+(.*?)(?=\n\d+\.|\n\n|$)"
        
        # Extract and structure recipe data
        recipe = {
            "recipe_id": f"{category}_{self.next_id:04d}",
            "source_book": source_book,
            "category": category,
            # ... additional fields
        }
        
        self.recipes.append(recipe)
        self.next_id += 1
        
        return recipe
    
    def export_to_json(self, filename):
        with open(filename, 'w') as f:
            json.dump(self.recipes, f, indent=2)
```

## Deliverables

### Immediate Deliverables (Week 1-4)
1. **recipes_tier1_survival.json** - 100 primitive survival recipes
2. **recipes_tier2_agriculture.json** - 120 farming and food recipes
3. **skill_tree_early_game.json** - Skill dependencies for Tier 1-2
4. **material_chains_basic.json** - Material processing flows

### Medium-Term Deliverables (Week 5-8)
5. **recipes_tier2_construction.json** - 80 building recipes
6. **recipes_tier3_metalwork.json** - 90 metalworking recipes
7. **skill_tree_mid_game.json** - Skill dependencies for Tier 2-3

### Long-Term Deliverables (Week 9-10)
8. **recipes_specialized_systems.json** - Water, energy, waste systems
9. **recipe_database_complete.json** - All 500+ recipes integrated
10. **crafting_system_documentation.md** - Complete implementation guide

## Success Metrics

- **Quantity:** 500+ recipes extracted and formatted
- **Quality:** 95%+ recipes have complete specifications
- **Balance:** 90%+ recipes pass balance review
- **Authenticity:** 100% recipes traceable to source material
- **Coverage:** All major crafting categories represented
- **Integration:** Recipes map to skill tree structure

## Next Steps

1. **Obtain Library:** Download Appropriate Technology Library
2. **Organize Content:** Sort ebooks by category
3. **Assign Team:** 2-3 content extractors for 10 weeks
4. **Begin Extraction:** Start with Tier 1 survival recipes
5. **Weekly Reviews:** Track progress against deliverables
6. **Iterative Refinement:** Adjust based on game balance needs

## Related Documents

- [Survival Guides Knowledge Domains Research](survival-guides-knowledge-domains-research.md) - Parent research
- [Content Extraction 01: OpenStreetMap](survival-content-extraction-01-openstreetmap.md) - Previous priority
- [Content Extraction 03: Survivor Library](survival-content-extraction-03-survivor-library.md) - Next priority

---

**Document Status:** Ready for content extraction  
**Last Updated:** 2025-01-15  
**Next Review:** After Phase 1A completion (Week 2)
