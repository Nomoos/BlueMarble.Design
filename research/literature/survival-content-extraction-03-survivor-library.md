# Content Extraction: Survivor Library for Historical Technology

---
title: Survivor Library Content Extraction for Pre-Industrial Technology
date: 2025-01-15
tags: [survivor-library, historical, pre-industrial, technology, implementation]
status: active
priority: 3 - Immediate Implementation
source: survivorlibrary.com (4 parts collection)
parent-research: survival-guides-knowledge-domains-research.md
---

**Document Type:** Content Extraction Guide  
**Version:** 1.0  
**Author:** Game Design Research Team  
**Date:** 2025-01-15  
**Priority:** Critical - Phase 1 Implementation  
**Implementation Status:** Ready for Content Team

## Overview

This document outlines the extraction process for historical technology and pre-industrial crafting knowledge from 
the Survivor Library collection. This library focuses on "how to survive when technology doesn't work," making it 
ideal for authentic primitive-to-medieval game content.

## Source Information

### Survivor Library Collection

**Source:** http://www.survivorlibrary.com/ (4-part torrent collection)
- **Collection Size:** ~150 GB total (4 parts)
- **Focus:** Pre-industrial technology, historical manufacturing, homesteading
- **Content Types:** Historical texts, trade manuals, military field guides, agricultural guides
- **Time Period Coverage:** 1700s-1950s (pre-electrical era)
- **Organization:** By topic (agriculture, construction, manufacturing, etc.)

**Key Characteristics:**
- Authentic historical documents and manuals
- Proven techniques from pre-industrial era
- Detailed technical illustrations
- Step-by-step procedures from craftsmen
- No modern technology dependencies

**Priority Topics for BlueMarble:**
1. Historical manufacturing processes (1800s-1900s)
2. Traditional agriculture and animal husbandry
3. Pre-industrial construction techniques
4. Historical metalworking and smithing
5. Traditional medicine and remedies
6. Historical food preservation
7. Early mechanical engineering
8. Textile production methods
9. Woodworking and carpentry
10. Chemical processes (soap, dyes, acids)

## Extraction Strategy

### Phase 1: Historical Manufacturing (Week 1-2)

**Objective:** Extract authentic 18th-19th century manufacturing processes

**Target Documents:**
- "Knight's American Mechanical Dictionary" (1876)
- "Cyclopedia of Practical Receipts" (1872)
- "Workshop Receipts" series
- "Henley's Twentieth Century Formulas"

**Content to Extract:**

#### 1. Metalworking Processes
**From historical texts:**
- Iron ore smelting (blast furnace operations)
- Steel making (crucible, Bessemer processes)
- Brass and bronze casting
- Wire drawing techniques
- Sheet metal working
- Tool hardening and tempering
- Electroplating methods

**Example Recipe Format:**
```json
{
    "recipe_id": "historical_steel_cementation_001",
    "recipe_name": "Cementation Steel Production",
    "category": "metalworking",
    "subcategory": "steel_making",
    "tier": 4,
    "historical_period": "1750-1850",
    "skill_required": "advanced_metallurgy",
    "skill_level": 8,
    "crafting_time": 86400,
    "materials": [
        {"item": "wrought_iron_bars", "quantity": 10},
        {"item": "charcoal_powder", "quantity": 50},
        {"item": "bone_ash", "quantity": 5}
    ],
    "structure_required": "cementation_furnace",
    "tools_required": [
        {"tool": "fire_poker", "durability_cost": 10}
    ],
    "temperature_required": 1100,
    "output": [
        {"item": "blister_steel", "quantity": 9, "quality_range": [0.6, 0.9]}
    ],
    "process_notes": [
        "Pack iron bars with charcoal in sealed chest",
        "Heat for 7-10 days at cherry-red temperature",
        "Carbon diffuses into iron surface",
        "Results in hard, brittle blister steel"
    ],
    "xp_gain": 100,
    "source_document": "Knight's Mechanical Dictionary",
    "source_year": 1876,
    "historical_note": "Used for edge tools before crucible steel became common"
}
```

#### 2. Chemical Processes
**Historical formulas to extract:**
- Soap making (various types)
- Dye production (natural sources)
- Ink manufacturing
- Glue and adhesive making
- Tanning solutions
- Cleaning compounds
- Preservatives
- Medicines and remedies

#### 3. Textile Production
**Traditional methods:**
- Fiber preparation (flax, wool, cotton)
- Spinning techniques
- Dyeing procedures
- Weaving patterns
- Finishing processes
- Waterproofing treatments

### Phase 2: Historical Agriculture (Week 3-4)

**Target Documents:**
- "American Agriculturist" archives
- "Farm and Garden Rule Book"
- "Five Acres and Independence"
- Historical agricultural bulletins

**Content to Extract:**

#### 1. Traditional Farming Methods
- Crop rotation systems
- Seed selection and saving
- Soil improvement techniques
- Hand tool usage
- Draft animal management
- Harvest timing and methods
- Storage techniques

#### 2. Animal Husbandry
- Breed selection criteria
- Feeding and care schedules
- Disease recognition and treatment
- Breeding practices
- Meat processing
- Dairy operations
- Veterinary procedures

**Example Recipe:**
```json
{
    "recipe_id": "agriculture_crop_rotation_001",
    "recipe_name": "Four-Field Rotation System",
    "category": "agriculture",
    "subcategory": "crop_management",
    "tier": 3,
    "historical_period": "1700-1900",
    "skill_required": "farming",
    "skill_level": 5,
    "time_scale": "seasonal",
    "implementation": {
        "year_1": {
            "field_1": "wheat",
            "field_2": "turnips",
            "field_3": "barley_with_clover",
            "field_4": "clover_fallow"
        },
        "year_2": {
            "field_1": "turnips",
            "field_2": "barley_with_clover",
            "field_3": "clover_fallow",
            "field_4": "wheat"
        }
    },
    "benefits": {
        "soil_fertility": "+20% over single crop",
        "pest_reduction": "50% fewer crop-specific pests",
        "yield_improvement": "+15% average yield"
    },
    "xp_gain": 50,
    "source_document": "Farm and Garden Rule Book",
    "historical_note": "Norfolk four-course rotation, widely adopted in 1700s"
}
```

### Phase 3: Historical Construction (Week 5-6)

**Target Documents:**
- "Builder's Guide" series (1800s)
- "Carpentry and Building" magazines
- "Masonry and Bricklaying" manuals
- Historical architecture books

**Content to Extract:**

#### 1. Foundation Techniques
- Stone foundation laying
- Rubble trench systems
- Pier and post foundations
- Moisture barriers (historical methods)
- Underpinning techniques

#### 2. Structural Systems
- Timber framing (mortise and tenon)
- Log construction methods
- Stone masonry techniques
- Brick laying patterns
- Roof truss designs

#### 3. Finishing Work
- Plaster preparation and application
- Whitewash and paint formulas
- Floor laying techniques
- Window and door installation
- Hardware installation

### Phase 4: Historical Woodworking (Week 7-8)

**Target Documents:**
- "Workshop Companion" series
- "Hand Tool Woodworking"
- Historical furniture making guides
- Cooperage and barrel making

**Content to Extract:**

#### 1. Hand Tool Techniques
- Saw selection and use
- Plane types and applications
- Chisel techniques
- Joinery methods
- Wood finishing

#### 2. Specialized Crafts
- Barrel making (cooperage)
- Wheelwright techniques
- Boat building
- Furniture construction
- Timber seasoning

## Historical Authenticity Framework

### Time Period Classification

**Medieval Period Recipes (500-1500 CE):**
- Very basic tools
- Limited material processing
- Community-scale production
- Tier 2-3 game content

**Early Modern Period (1500-1750):**
- Improved hand tools
- Guild-based specialization
- Regional trade networks
- Tier 3-4 game content

**Industrial Revolution Era (1750-1850):**
- Early mechanization
- Factory system emerging
- Advanced metallurgy
- Tier 4-5 game content

**Late Industrial Period (1850-1950):**
- Mature mechanical systems
- Chemical processes
- Mass production techniques
- Tier 5+ game content

### Technology Progression Mapping

**Tier 1 → Tier 2 Transition:**
```
PrimitiveTools → TraditionalCrafts
- Stone tools → Iron tools
- Natural materials → Processed materials
- Individual production → Workshop production
- Gathering → Agriculture

Example Historical Source: 
"Primitive Technology" sections from Survivor Library
```

**Tier 2 → Tier 3 Transition:**
```
TraditionalCrafts → SpecializedManufacturing
- Basic forging → Advanced metalworking
- Hand tools → Specialized tools
- Local production → Regional trade
- Simple chemistry → Complex formulations

Example Historical Source:
"Workshop Receipts" series, "Cyclopedia of Practical Receipts"
```

**Tier 3 → Tier 4 Transition:**
```
SpecializedManufacturing → EarlyIndustrial
- Manual processes → Water/wind powered
- Small workshops → Early factories
- Basic alloys → Advanced metallurgy
- Natural materials → Synthesized materials

Example Historical Source:
"Knight's Mechanical Dictionary", Industrial manuals from 1800s
```

## Recipe Extraction Workflow

### Step 1: Document Acquisition (Day 1-2)

**Download and Organize:**
1. Download all 4 parts of Survivor Library
2. Extract archives to organized directory structure
3. Index all documents by:
   - Topic category
   - Historical period
   - Technology level
   - Document type

**Directory Structure:**
```
survivor_library/
├── agriculture/
│   ├── 1700s/
│   ├── 1800s/
│   └── 1900s/
├── metalworking/
│   ├── historical_smithing/
│   ├── iron_steel_making/
│   └── casting_forming/
├── construction/
│   ├── foundations/
│   ├── timber_framing/
│   └── masonry/
├── chemistry/
│   ├── formulas/
│   ├── processes/
│   └── materials/
└── manufacturing/
    ├── textiles/
    ├── woodworking/
    └── mechanical/
```

### Step 2: Priority Content Identification (Day 3-5)

**Selection Criteria:**
- Clear, step-by-step instructions
- Complete material lists
- Appropriate technology level for game
- Historical documentation
- Transferable to game mechanics

**Flag for Extraction:**
- Recipes with diagrams/illustrations
- Multi-step processes
- Material transformation chains
- Tool-making procedures
- Infrastructure construction

### Step 3: Recipe Standardization (Day 6-14)

**Conversion Process:**

**For each identified recipe:**
1. Extract raw text and illustrations
2. Identify all materials and quantities
3. List required tools and equipment
4. Break down into game-actionable steps
5. Determine appropriate tier level
6. Calculate crafting time estimates
7. Define output items and quality ranges
8. Assign skill requirements
9. Add historical context notes
10. Validate against game balance

**Historical Recipe Adaptation:**
```
Historical Source Text:
"To make blister steel: Pack bars of wrought iron with powdered 
charcoal in a chest made of firebrick. Seal with fire clay. Heat 
in furnace at cherry-red heat for 7 to 10 days. The carbon from 
the charcoal penetrates the surface of the iron, creating a hard 
steel suitable for edge tools."

Adapted Game Recipe:
{
    "recipe_name": "Cementation Steel",
    "process_steps": [
        "Place wrought iron bars in cementation chest",
        "Pack with charcoal powder between layers",
        "Seal chest with fire clay",
        "Maintain constant heat for 7 days",
        "Allow to cool slowly",
        "Extract hardened blister steel"
    ],
    "game_time": 600,
    "real_time_equivalent": "7 days",
    "success_factors": [
        "Temperature consistency",
        "Proper sealing",
        "Quality of iron input"
    ]
}
```

## Integration with Game Systems

### Skill Tree Integration

**Historical Technology Skills:**
```
HistoricalCrafts [Parent Skill] {
    - Blacksmithing [Level 1-15]
      ├─ Basic Forging [1-5]
      ├─ Tool Making [5-10]
      └─ Advanced Metallurgy [10-15]
    
    - Carpentry [Level 1-15]
      ├─ Hand Tools [1-5]
      ├─ Joinery [5-10]
      └─ Fine Woodworking [10-15]
    
    - Masonry [Level 1-15]
      ├─ Stone Working [1-5]
      ├─ Brick Laying [5-10]
      └─ Advanced Construction [10-15]
    
    - Chemistry [Level 1-15]
      ├─ Simple Compounds [1-5]
      ├─ Formulations [5-10]
      └─ Complex Synthesis [10-15]
}
```

### Quality System Integration

**Historical Quality Grades:**
```
QualityLevels {
    Apprentice [0.3-0.5]:
        - First attempts at craft
        - Functional but crude
        - May have defects
    
    Journeyman [0.5-0.7]:
        - Competent work
        - Reliable quality
        - Standard functionality
    
    Master [0.7-0.9]:
        - Excellent craftsmanship
        - Superior durability
        - Enhanced properties
    
    Legendary [0.9-1.0]:
        - Perfect execution
        - Maximum durability
        - Special properties
}
```

## Deliverables

### Week 1-2: Historical Manufacturing
- **recipes_historical_metalworking.json** (50+ recipes)
- **recipes_historical_chemistry.json** (40+ recipes)
- **recipes_historical_textiles.json** (30+ recipes)

### Week 3-4: Historical Agriculture
- **recipes_historical_farming.json** (60+ recipes)
- **recipes_animal_husbandry.json** (40+ recipes)

### Week 5-6: Historical Construction
- **recipes_historical_building.json** (50+ recipes)
- **construction_techniques.json** (techniques library)

### Week 7-8: Specialized Crafts
- **recipes_historical_woodworking.json** (40+ recipes)
- **recipes_specialized_crafts.json** (30+ recipes)

**Total Target:** 340+ historically authentic recipes

## Quality Assurance

### Historical Accuracy Review
- [ ] Source document identified and cited
- [ ] Historical period verified
- [ ] Process steps match historical descriptions
- [ ] Materials authentic to time period
- [ ] Tools appropriate for technology level
- [ ] Results consistent with historical outcomes

### Game Balance Review
- [ ] Crafting time appropriate for tier
- [ ] Material costs balanced against output value
- [ ] Skill requirements logical for complexity
- [ ] XP rewards match effort required
- [ ] Output quality ranges reasonable
- [ ] Failure chances balanced

### Documentation Review
- [ ] Complete recipe specification
- [ ] Clear player-facing descriptions
- [ ] Historical context provided
- [ ] Integration notes for developers
- [ ] Source attribution complete

## Success Metrics

- **Quantity:** 340+ recipes extracted
- **Coverage:** All major historical craft categories
- **Authenticity:** 100% historically sourced
- **Completeness:** 95%+ recipes fully specified
- **Integration:** Mapped to skill and tier systems
- **Quality:** Pass all review criteria

## Next Steps

1. **Download Collection:** Acquire Survivor Library (4 parts)
2. **Organize Content:** Sort by category and period
3. **Assign Team:** 2 content extractors, 1 historian consultant
4. **Begin Extraction:** Start with metalworking recipes
5. **Weekly Progress:** Track against 340+ recipe goal
6. **Iterative Review:** Game balance and historical accuracy

## Related Documents

- [Survival Guides Knowledge Domains Research](survival-guides-knowledge-domains-research.md) - Parent research
- [Content Extraction 01: OpenStreetMap](survival-content-extraction-01-openstreetmap.md) - Geographic data
- [Content Extraction 02: Appropriate Technology](survival-content-extraction-02-appropriate-technology.md) - Sustainable tech

---

**Document Status:** Ready for content extraction  
**Last Updated:** 2025-01-15  
**Next Review:** After Phase 1 completion (Week 2)
