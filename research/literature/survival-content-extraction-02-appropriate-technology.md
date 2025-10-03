# Content Extraction: Appropriate Technology Library for Crafting Systems

---
title: Appropriate Technology Library Content Extraction
date: 2025-01-15
tags: [crafting, recipes, appropriate-technology, sustainability]
status: completed
priority: high
source: Appropriate Technology Library (1050+ ebooks)
---

## Executive Summary

The Appropriate Technology Library contains 1,050+ ebooks focused on sustainable, low-tech solutions perfect for BlueMarble's crafting system. This guide outlines extraction of 500+ recipes across Tiers 1-4.

**Target Content:**
- 500+ extractable crafting recipes
- Focus on practical, step-by-step processes
- Technology tiers 1-4 (primitive to industrial)
- Authentic real-world techniques

**Implementation Priority:** HIGH - Core crafting content

## Source Overview

### Appropriate Technology Library

**Collection Size:** 1,050+ ebooks covering:
- Agriculture and food production
- Water systems and sanitation
- Building and construction
- Energy generation (small-scale)
- Metalworking and tool making
- Textile production
- Food preservation

**Magnet Link:**
```
magnet:?xt=urn:btih:927CEF33C1E320C669ED7913CC1A63736DA530B9&dn=Appropriate+Technology+Library+-1050+eBooks
```

**Organization:**
- Organized by topic folders
- Mix of scanned PDFs and text-searchable documents
- Quality varies - some require OCR
- Most books 50-300 pages

## Content Extraction Strategy

### Phase 1: Recipe Database Creation (Week 1-3)

#### Category 1: Food Production (120 recipes)
**Target Books:**
- "Small-Scale Food Processing"
- "Food Preservation for the Self-Sufficient"
- "Root Cellars"
- "Fermentation Guide"

**Recipes to Extract:**
- Bread baking (10 variations)
- Cheese making (8 types)
- Pickling and preserving (20 methods)
- Drying and smoking (15 techniques)
- Fermentation (15 products)
- Beer/wine making (12 processes)
- Rendering fats (5 methods)
- Sugar processing (10 techniques)

**Example Recipe Format:**
```json
{
  "recipe_id": "food_0001",
  "name": "Sourdough Bread",
  "tier": 2,
  "category": "cooking",
  "inputs": [
    {"item": "flour", "quantity": 500, "unit": "g"},
    {"item": "water", "quantity": 350, "unit": "ml"},
    {"item": "salt", "quantity": 10, "unit": "g"},
    {"item": "sourdough_starter", "quantity": 100, "unit": "g"}
  ],
  "outputs": [
    {"item": "sourdough_loaf", "quantity": 1, "quality_range": [0.6, 1.0]}
  ],
  "requirements": {
    "skill": "cooking",
    "skill_level": 10,
    "workshop": "bakery",
    "time_minutes": 180,
    "temperature": "200C"
  },
  "source": {
    "book": "Traditional Bread Making",
    "page": 34,
    "library": "Appropriate Technology Library"
  },
  "steps": [
    "Mix flour and water, let rest 30 minutes",
    "Add starter and salt, knead 10 minutes",
    "First rise 4 hours at room temperature",
    "Shape loaf, second rise 2 hours",
    "Score top and bake 40 minutes at 200C"
  ]
}
```

#### Category 2: Agriculture (100 recipes)
**Target Books:**
- "Organic Gardening Methods"
- "Composting Guide"
- "Seed Saving Handbook"
- "Small-Scale Livestock"

**Recipes to Extract:**
- Composting methods (15 variations)
- Crop planting (30 crops)
- Seed saving (20 crops)
- Animal breeding (10 species)
- Pest control (15 organic methods)
- Soil improvement (10 techniques)

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

#### Category 4: Metalworking (90 recipes)
**Target Books:**
- "The Backyard Blacksmith"
- "Small-Scale Smelting"
- "Tool Making Manual"
- "Forge Construction"

**Recipes to Extract:**
- Bloomery iron smelting
- Charcoal production (5 methods)
- Basic forging (20 tools)
- Casting techniques (10 molds)
- Heat treatment (15 processes)
- Tool sharpening (10 methods)

#### Category 5: Textile Production (60 recipes)
**Target Books:**
- "Spinning and Weaving"
- "Natural Dyes"
- "Fiber Processing"
- "Basketry Techniques"

**Recipes to Extract:**
- Fiber processing (flax, wool, cotton)
- Spinning techniques
- Weaving patterns (20 types)
- Natural dyeing (25 colors)
- Basketry (10 styles)

#### Category 6: Energy and Water (50 recipes)
**Target Books:**
- "Water Pumps and Hydraulic Rams"
- "Small Wind Power"
- "Solar Cooking"
- "Biogas Production"

**Recipes to Extract:**
- Water wheel construction
- Wind pump design
- Solar cooker building
- Biogas digester setup
- Water purification systems

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

### Step 3: Recipe Formatting (Week 3)

**Standardization Process:**
- Convert to JSON format
- Assign technology tier (1-5)
- Map to skill system
- Define time requirements
- Set quality ranges
- Add source attribution

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
  Cooking: {
    prerequisites: [Fire_Making],
    unlocks: [Bread_Baking, Cheese_Making, Preserving]
  },
  Agriculture: {
    prerequisites: [Tool_Use],
    unlocks: [Planting, Composting, Animal_Husbandry]
  },
  Carpentry: {
    prerequisites: [Tool_Making, Wood_Working],
    unlocks: [House_Building, Furniture, Advanced_Structures]
  }
}
```

## Implementation Priorities

### Week 1-2: Survival Essentials (Phase 1A)
**Priority: Critical**

Extract and implement 70 recipes:
- 20 fire starting and cooking
- 20 water purification and storage
- 15 basic shelter construction
- 15 primitive tool making

**Deliverable:** JSON file with 70 survival recipes + basic skill tree

### Week 3-4: Food Systems (Phase 1B)
**Priority: High**

Extract and implement 120 recipes:
- 40 cooking and food preparation
- 40 food preservation
- 40 basic agriculture

**Deliverable:** JSON file with 120 food recipes + agriculture skill tree

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

### Week 9-10: Advanced Systems (Phase 2B)
**Priority: Medium**

Extract and implement 140 recipes:
- 60 textile production
- 50 energy systems
- 30 water engineering

**Deliverable:** JSON files for advanced systems + integrated skill trees

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

**Usage Example:**
```python
extractor = RecipeExtractor()

# Process all PDF files in directory
for pdf_file in pdf_directory:
    text = extract_pdf_text(pdf_file)
    extractor.extract_from_text(text, pdf_file, determine_category(pdf_file))

extractor.export_to_json('extracted_recipes.json')
```

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

**Time Balance:**
- Tier 1 recipes: 1-10 minutes
- Tier 2 recipes: 10-60 minutes
- Tier 3 recipes: 1-4 hours
- Tier 4 recipes: 4-12 hours
- Tier 5 recipes: 12+ hours

**Resource Balance:**
- Common materials abundant
- Rare materials limited
- Output value proportional to input + time
- Quality variation adds replayability

## Deliverables

### Short-term Deliverables (Week 1-4)
1. **recipes_survival_tier1.json** - 70 basic survival recipes
2. **recipes_food_tier2.json** - 120 food production recipes
3. **recipes_construction_tier2-3.json** - 80 building recipes
4. **skill_tree_basics.json** - Tier 1-2 skill definitions

### Mid-term Deliverables (Week 5-8)
5. **recipes_metalworking_tier3-4.json** - 90 metallurgy recipes
6. **recipes_textiles_tier2-4.json** - 60 textile recipes
7. **skill_tree_advanced.json** - Tier 3-4 skill definitions

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

## References

- **Source:** Appropriate Technology Library
- **Download:** Via magnet link in awesome-survival repository
- **Related:** `survival-guides-knowledge-domains-research.md`
- **Integration:** BlueMarble crafting system documentation

---

**Document Status:** Complete
**Last Updated:** 2025-01-15
**Estimated Extraction Time:** 10 weeks (2-3 people)
**Priority:** HIGH - Core game content
