# Resource Gathering and Assembly Skills - Research Report

**Document Type:** Research Report  
**Version:** 1.2  
**Author:** Game Design Research Team  
**Date:** 2025-01-08  
**Last Updated:** 2025-01-08  
**Status:** Final  
**Research Type:** Game Mechanics Research

## Executive Summary

This research report proposes a comprehensive skill system for BlueMarble covering both resource gathering 
and assembly/crafting professions. The system emphasizes realism, geological authenticity, and meaningful 
player progression. It integrates traditional gathering skills (mining, herbalism, logging, hunting, fishing) 
and crafting professions (blacksmithing, tailoring, alchemy, woodworking) with BlueMarble's unique geological 
simulation foundation. The proposed skill system uses a practice-based progression model with a three-tiered 
experience system for gathering (general skill + material group + specific material familiarity) and 
single-track progression for assembly, where material quality, tool quality, environmental conditions, and 
practitioner skill level all influence outcomes including success rate, item quality, and special bonuses.

**Key Findings:**
- Realistic skill systems should reflect real-world learning curves with diminishing returns at higher levels
- Material quality and availability should be tied to BlueMarble's geological simulation
- Gathering skills use three-tiered experience tracking: general proficiency + material group + specific material familiarity
- Material familiarity creates depth (e.g., experienced iron miner is more efficient with iron than copper)
- Experience affects efficiency (speed), quality preservation, and yield (quantity) of gathered materials
- Material group experience unlocks special abilities (ore sense, herb detection, grain reading)
- Rare materials provide higher XP rewards and faster familiarity gains to balance their scarcity
- Multiple diminishing returns systems prevent exploitation while maintaining long-term engagement
- Multi-stage crafting processes create engaging gameplay and respect real-world manufacturing complexity
- Success rates and quality bonuses should scale with skill level, creating meaningful progression
- Specialization within professions allows for player differentiation and economic niches
- Gathering and assembly skills form an integrated chain from raw materials to finished products

## Table of Contents

1. [Research Objectives](#research-objectives)
2. [Methodology](#methodology)
3. [Core Assembly Skills Overview](#core-assembly-skills-overview)
4. [Resource Gathering Skills](#resource-gathering-skills)
5. [Gathering Experience System Design Considerations](#gathering-experience-system-design-considerations)
6. [Skill Progression Mechanics](#skill-progression-mechanics)
7. [Crafting Interface Design](#crafting-interface-design)
8. [Material Quality and Geological Integration](#material-quality-and-geological-integration)
9. [Real-World Skill Analysis](#real-world-skill-analysis)
10. [Recommendations](#recommendations)
11. [Implementation Roadmap](#implementation-roadmap)
12. [Appendices](#appendices)

## Research Objectives

### Primary Research Questions
1. How can assembly skills realistically represent real-world crafting professions within BlueMarble's 
   geological simulation framework?
2. What progression mechanics create meaningful skill advancement while maintaining engagement?
3. How should material quality, tool quality, and environmental factors influence crafting outcomes?
4. What specialization paths within each profession provide distinct gameplay experiences?

### Secondary Research Questions
1. How can the crafting interface clearly communicate success rates, bonuses, and quality factors?
2. What balance between skill-based and material-based quality creates the best player experience?
3. How can assembly skills integrate with BlueMarble's existing economic and geological systems?

### Success Criteria
This research succeeds if it provides:
- Clear, implementable specifications for core assembly skills
- Realistic progression mechanics that reward practice and specialization
- Integration pathways with existing BlueMarble geological systems
- Actionable UI/UX recommendations for crafting interfaces

## Methodology

### Research Approach
Mixed methods approach combining:
- Analysis of real-world crafting professions and skill acquisition
- Review of successful MMORPG crafting systems
- Integration study with BlueMarble's existing geological and economic systems
- Design of progression curves and success rate formulas

### Data Collection Methods
- **Historical Research:** Study of traditional craftsmanship and skill development in blacksmithing, 
  tailoring, alchemy, and woodworking
- **Game System Analysis:** Review of crafting mechanics in EVE Online, Final Fantasy XIV, 
  Elder Scrolls Online, and similar MMORPGs
- **Mathematical Modeling:** Development of progression curves and quality calculation formulas
- **Documentation Review:** Analysis of existing BlueMarble design documents

### Data Sources
- Historical craftsmanship literature and apprenticeship models
- MMORPG design documentation and player feedback
- BlueMarble game design documents (gameplay-systems.md, economy-systems.md)
- Real-world manufacturing processes and quality control standards

## Core Assembly Skills Overview

### Blacksmithing

**Definition:** The art of shaping metal through heating, hammering, and tempering to create weapons, 
armor, tools, and hardware.

**Real-World Basis:**
- Requires understanding of metal properties, crystal structures, and thermal dynamics
- Involves multiple stages: material preparation, heating, shaping, cooling, finishing
- Quality depends on temperature control, hammer technique, and material purity
- Different metals require different techniques (iron vs. steel vs. bronze)

**BlueMarble Integration:**
- Uses ores and metals extracted from BlueMarble's geological simulation
- Metal quality varies based on deposit purity and extraction method
- Forge temperature and fuel quality affect outcomes
- Tool wear reflects realistic degradation

**Skill Progression Areas:**
1. **Material Preparation** (Levels 1-20)
   - Basic ore smelting and purification
   - Understanding metal properties
   - Simple alloy creation
   
2. **Shaping Techniques** (Levels 21-50)
   - Advanced forging methods
   - Precision tool creation
   - Complex armor crafting
   
3. **Master Techniques** (Levels 51-100)
   - Damascus steel and pattern welding
   - Enchantment-ready equipment
   - Custom masterwork creation

**Specialization Paths:**
- **Weaponsmith:** Focus on weapon balance, edge geometry, and combat effectiveness
- **Armorsmith:** Expertise in protective equipment, weight distribution, and mobility
- **Toolmaker:** Specialization in precision tools for other crafts, mining, and engineering

### Tailoring

**Definition:** The craft of designing, cutting, and assembling fabric and leather into clothing, 
light armor, bags, and decorative items.

**Real-World Basis:**
- Requires pattern design, material selection, and precise cutting
- Different materials (cotton, linen, leather, silk) have unique properties
- Quality depends on stitch precision, pattern efficiency, and material handling
- Multiple stages: design, pattern creation, cutting, assembly, finishing

**BlueMarble Integration:**
- Natural fibers from plants (cotton, flax, hemp) grown in appropriate climates
- Leather from hunting, quality varies by animal and tanning process
- Dyes from minerals and plants based on geological/botanical availability
- Thread strength and durability based on source materials

**Skill Progression Areas:**
1. **Basic Textile Work** (Levels 1-20)
   - Simple clothing construction
   - Basic pattern use
   - Material identification
   
2. **Advanced Tailoring** (Levels 21-50)
   - Custom pattern design
   - Light armor construction
   - Specialty item creation
   
3. **Master Craftsmanship** (Levels 51-100)
   - Magical fabric integration
   - Lightweight protective gear
   - Prestigious fashion items

**Specialization Paths:**
- **Clothier:** Focus on mage robes, comfort wear, and fashion items
- **Leatherworker:** Expertise in leather armor, bags, and functional gear
- **Outfitter:** Specialization in expedition gear, storage solutions, and utility items

### Alchemy

**Definition:** The science and art of extracting, combining, and transforming natural substances 
to create potions, elixirs, and magical components.

**Real-World Basis:**
- Based on historical alchemy, herbalism, and modern chemistry
- Requires knowledge of plant properties, mineral composition, and chemical reactions
- Multiple processing methods: distillation, extraction, infusion, calcination
- Quality depends on ingredient purity, processing temperature, and timing

**BlueMarble Integration:**
- Herbs and plants from diverse biomes and climates
- Minerals and crystals from geological formations
- Pure water sources (quality varies by location)
- Creature components from hunting (magical properties)

**Skill Progression Areas:**
1. **Basic Preparation** (Levels 1-20)
   - Simple potion brewing
   - Ingredient identification
   - Basic extraction techniques
   
2. **Advanced Formulation** (Levels 21-50)
   - Complex multi-ingredient potions
   - Timed reaction control
   - Quality enhancement techniques
   
3. **Master Alchemy** (Levels 51-100)
   - Experimental formulations
   - Magical augmentation
   - Philosopher's stone research

**Specialization Paths:**
- **Healer:** Focus on restorative potions, antidotes, and medical preparations
- **Enhancer:** Expertise in buff potions, attribute boosters, and performance enhancers
- **Transmuter:** Specialization in material transformation and magical component creation

### Woodworking

**Definition:** The craft of shaping and assembling wood into weapons, tools, furniture, 
and structural components.

**Real-World Basis:**
- Requires understanding of wood grain, moisture content, and species properties
- Different woods have distinct characteristics (hardwood vs. softwood)
- Multiple techniques: sawing, carving, joinery, finishing
- Quality depends on wood selection, tool sharpness, and craftsmanship precision

**BlueMarble Integration:**
- Wood types based on BlueMarble's botanical and climate simulation
- Wood quality varies by tree age, growing conditions, and harvesting method
- Seasoning and drying affect wood properties realistically
- Joint strength and furniture durability reflect real-world engineering

**Skill Progression Areas:**
1. **Basic Carpentry** (Levels 1-20)
   - Simple furniture construction
   - Basic tool crafting
   - Wood identification
   
2. **Advanced Woodworking** (Levels 21-50)
   - Complex joinery techniques
   - Weapon crafting (bows, staffs, hafts)
   - Precision instrument creation
   
3. **Master Woodworking** (Levels 51-100)
   - Artistic furniture pieces
   - Magical wood integration
   - Ship and structure components

**Specialization Paths:**
- **Fletcher:** Focus on bows, arrows, and ranged weapon components
- **Carpenter:** Expertise in furniture, structures, and architectural components
- **Instrument Maker:** Specialization in musical instruments, magical focuses, and precision items

### Resource Gathering Skills

**Definition:** Resource gathering skills represent the player's ability to efficiently and effectively extract 
raw materials from the environment. Unlike assembly skills which transform materials into finished products, 
gathering skills focus on harvesting, extracting, and collecting resources while maintaining quality and 
maximizing yield.

**Dual Experience System:**

Gathering skills use a dual-experience model that tracks both:
1. **General Gathering Skill Experience:** Overall proficiency in the gathering profession
2. **Material-Specific Experience:** Familiarity with individual materials or material groups

This creates depth where an experienced miner who has primarily worked with iron ore will be more efficient 
with iron extraction than with copper, even though both are metal ores.

#### Mining

**Definition:** The extraction of ores, gems, and minerals from geological formations using specialized tools 
and techniques.

**Real-World Basis:**
- Requires understanding of geology, rock identification, and extraction methods
- Different materials require different tools (pickaxe for hard rock, shovel for soft deposits)
- Extraction efficiency depends on technique, tool quality, and geological conditions
- Safety considerations in real mining (structural integrity, ventilation)

**BlueMarble Integration:**
- Directly tied to geological simulation - ore locations based on realistic formations
- Material quality preservation depends on extraction technique
- Tool durability affected by rock hardness
- Deposit depletion modeled realistically

**Material Groups:**
- **Metal Ores:** Iron, copper, gold, silver, tin (similar extraction techniques)
- **Precious Gems:** Diamonds, rubies, emeralds (require careful extraction)
- **Stone & Rock:** Granite, marble, limestone (building materials)
- **Minerals:** Salt, sulfur, coal (varied extraction methods)

**Experience Progression:**
```
Mining_XP = Base_XP × Material_Hardness × Extraction_Quality × Tool_Efficiency
Material_Familiarity_XP = Mining_XP × 0.5 (for specific material)
Group_Familiarity_XP = Mining_XP × 0.25 (for material group)
```

**Skill Benefits:**
- **Levels 1-20:** Basic ore identification, standard extraction
  - Success Rate: 70-85%
  - Material Loss: 20-30%
  - Extraction Speed: Slow
  
- **Levels 21-50:** Advanced techniques, vein following, quality preservation
  - Success Rate: 85-92%
  - Material Loss: 10-20%
  - Extraction Speed: Moderate
  - Bonus: Occasional extra yield (+5-10%)
  
- **Levels 51-100:** Master extraction, minimal waste, quality enhancement
  - Success Rate: 92-98%
  - Material Loss: 5-10%
  - Extraction Speed: Fast
  - Bonus: Regular extra yield (+10-20%), rare quality upgrade

**Material Familiarity Bonuses:**
```
Familiarity_Bonus = (Material_Experience / 1000) × 5% (capped at +25%)

Example:
- 0 Iron Ore extractions: +0% efficiency
- 200 Iron Ore extractions: +10% efficiency with iron
- 500+ Iron Ore extractions: +25% efficiency with iron (capped)
```

**Specialization Paths:**
- **Prospector:** Focus on finding deposits, geological surveying, vein prediction
- **Quarryman:** Expertise in building stone extraction, large-scale operations
- **Gem Cutter:** Specialization in precious gem extraction and initial processing

#### Herbalism

**Definition:** The collection of plants, flowers, roots, and fungi for use in alchemy, cooking, and medicine.

**Real-World Basis:**
- Requires plant identification knowledge and seasonal awareness
- Different harvesting methods for different plant parts (leaves vs. roots vs. flowers)
- Timing affects potency (season, time of day, lunar cycles)
- Sustainable harvesting practices ensure continued availability

**BlueMarble Integration:**
- Plants tied to climate zones and biomes in BlueMarble's simulation
- Seasonal availability affects what can be gathered
- Harvesting technique affects plant regrowth time
- Weather and soil quality influence plant potency

**Material Groups:**
- **Flowers:** Decorative and magical properties
- **Herbs:** Medicinal and alchemical uses
- **Roots:** Strong medicinal properties, requires digging
- **Fungi:** Special properties, requires careful identification
- **Tree Products:** Bark, resin, sap (requires tree-specific knowledge)

**Experience Progression:**
```
Herbalism_XP = Base_XP × Plant_Rarity × Harvest_Quality × Season_Modifier
Plant_Familiarity_XP = Herbalism_XP × 0.5 (for specific plant)
Group_Familiarity_XP = Herbalism_XP × 0.25 (for plant group)
```

**Skill Benefits:**
- **Levels 1-20:** Basic plant identification, standard gathering
  - Yield: 1-2 per plant
  - Quality Preservation: 70%
  - Damage to Plant: High (longer regrowth)
  
- **Levels 21-50:** Seasonal knowledge, sustainable harvesting
  - Yield: 2-3 per plant
  - Quality Preservation: 85%
  - Damage to Plant: Moderate (standard regrowth)
  - Bonus: Rare material drops (+5% chance)
  
- **Levels 51-100:** Master harvesting, potency enhancement
  - Yield: 3-4 per plant
  - Quality Preservation: 95%
  - Damage to Plant: Minimal (faster regrowth)
  - Bonus: Rare drops (+15%), quality upgrades

**Specialization Paths:**
- **Botanist:** Focus on rare plants, cross-breeding, cultivation
- **Forager:** Expertise in wild harvesting, tracking seasonal changes
- **Mycologist:** Specialization in fungi identification and cultivation

#### Logging

**Definition:** The harvesting of wood and forest products from trees for use in woodworking, construction, 
and fuel.

**Real-World Basis:**
- Requires tree species identification and growth assessment
- Proper felling techniques ensure safety and wood quality
- Different tools for different tree sizes (axe, saw, specialized equipment)
- Sustainable forestry practices maintain forest health

**BlueMarble Integration:**
- Tree types based on climate and terrain simulation
- Tree age affects wood quality and yield
- Proper felling preserves wood grain integrity
- Forest regeneration modeled realistically

**Material Groups:**
- **Hardwoods:** Oak, maple, mahogany (furniture, tools)
- **Softwoods:** Pine, spruce, cedar (construction, general use)
- **Exotic Woods:** Rare species with special properties
- **Tree Products:** Bark, resin, sap (secondary resources)

**Experience Progression:**
```
Logging_XP = Base_XP × Tree_Size × Wood_Quality × Technique_Efficiency
Wood_Familiarity_XP = Logging_XP × 0.5 (for specific wood type)
Group_Familiarity_XP = Logging_XP × 0.25 (for wood group)
```

**Skill Benefits:**
- **Levels 1-20:** Basic tree felling, identification
  - Wood Loss: 20-30% (poor cuts, splitting)
  - Quality Preservation: 60%
  - Felling Time: Slow
  
- **Levels 21-50:** Proper techniques, grain preservation
  - Wood Loss: 10-20%
  - Quality Preservation: 80%
  - Felling Time: Moderate
  - Bonus: Occasional rare wood finds
  
- **Levels 51-100:** Master lumberjack, optimal harvesting
  - Wood Loss: 5-10%
  - Quality Preservation: 95%
  - Felling Time: Fast
  - Bonus: Maximum yield, quality upgrades

**Specialization Paths:**
- **Timber Scout:** Focus on finding prime trees, forest surveying
- **Lumberjack:** Expertise in efficient felling, large-scale operations
- **Tree Tender:** Specialization in sustainable harvesting, forest management

#### Hunting

**Definition:** The tracking and harvesting of animals for leather, bones, meat, and other animal products.

**Real-World Basis:**
- Requires animal tracking, behavior knowledge, and ethical harvesting
- Different animals require different approaches
- Proper field dressing preserves material quality
- Sustainable hunting maintains population balance

**BlueMarble Integration:**
- Animal populations based on ecosystem simulation
- Tracking difficulty varies by terrain and animal
- Clean kills preserve hide quality
- Over-hunting affects local populations

**Material Groups:**
- **Leathers:** Light (rabbit), medium (deer), heavy (bear)
- **Bones & Horns:** Crafting materials, tools
- **Meats:** Cooking ingredients, varying quality
- **Exotic Materials:** Rare creature components (magical properties)

**Experience Progression:**
```
Hunting_XP = Base_XP × Animal_Difficulty × Harvest_Quality × Stealth_Bonus
Animal_Familiarity_XP = Hunting_XP × 0.5 (for specific animal)
Group_Familiarity_XP = Hunting_XP × 0.25 (for animal group)
```

**Skill Benefits:**
- **Levels 1-20:** Basic tracking, standard harvesting
  - Tracking Success: 60-70%
  - Material Quality: 60-70%
  - Harvest Time: Slow
  
- **Levels 21-50:** Advanced tracking, efficient harvesting
  - Tracking Success: 80-85%
  - Material Quality: 80-85%
  - Harvest Time: Moderate
  - Bonus: Occasional rare materials
  
- **Levels 51-100:** Master hunter, premium harvesting
  - Tracking Success: 90-95%
  - Material Quality: 90-95%
  - Harvest Time: Fast
  - Bonus: Rare materials, quality upgrades

**Specialization Paths:**
- **Tracker:** Focus on finding rare animals, behavior prediction
- **Trapper:** Expertise in live capture, sustainable harvesting
- **Butcher:** Specialization in field dressing, material yield maximization

#### Fishing

**Definition:** The catching of fish and aquatic resources from rivers, lakes, and oceans.

**Real-World Basis:**
- Requires knowledge of fish species, behavior, and seasonal patterns
- Different techniques for different environments (river vs. ocean)
- Proper handling preserves fish quality
- Weather and time of day affect success

**BlueMarble Integration:**
- Fish populations based on water quality and ecosystem health
- Seasonal spawning patterns affect availability
- Proper technique preserves fish quality for cooking
- Sustainable fishing maintains aquatic ecosystems

**Material Groups:**
- **Freshwater Fish:** River and lake species
- **Saltwater Fish:** Ocean species
- **Shellfish:** Crustaceans and mollusks
- **Aquatic Plants:** Kelp, seaweed (secondary resources)

**Experience Progression:**
```
Fishing_XP = Base_XP × Fish_Rarity × Catch_Quality × Location_Modifier
Fish_Familiarity_XP = Fishing_XP × 0.5 (for specific fish)
Group_Familiarity_XP = Fishing_XP × 0.25 (for fish group)
```

**Skill Benefits:**
- **Levels 1-20:** Basic fishing, common catches
  - Catch Rate: 60-70%
  - Quality Preservation: 65%
  - Rare Catches: Very low
  
- **Levels 21-50:** Advanced techniques, better yields
  - Catch Rate: 75-85%
  - Quality Preservation: 80%
  - Rare Catches: Moderate
  
- **Levels 51-100:** Master angler, premium catches
  - Catch Rate: 85-95%
  - Quality Preservation: 95%
  - Rare Catches: High

**Specialization Paths:**
- **Deep Sea Fisher:** Focus on ocean fishing, large catches
- **Fly Fisher:** Expertise in rivers and streams, technique-focused
- **Aquaculturist:** Specialization in fish farming, sustainable practices

### Gathering Experience System Design Considerations

**Overview:**

The three-tiered gathering experience system creates depth and rewards both broad exploration and focused 
specialization. This section addresses key design questions about how the system functions and balances 
different gameplay styles.

#### Experience Impact on Gathering Performance

**What does higher experience improve?**

The gathering experience system affects three key performance metrics:

1. **Efficiency (Speed):**
   - General Gathering Skill: Reduces time per harvest action
   - Material-Specific Experience: Further reduces time for familiar materials
   - Combined Effect: Expert gatherers working with familiar materials are significantly faster
   
   ```
   Harvest_Time = Base_Time × (1 - General_Skill_Modifier) × (1 - Familiarity_Modifier)
   
   Example - Mining Iron Ore:
   - Novice (Level 5, 0 Iron XP): 10s × 0.9 × 1.0 = 9.0s per ore
   - Intermediate (Level 30, 100 Iron XP): 10s × 0.7 × 0.9 = 6.3s per ore
   - Expert (Level 70, 500 Iron XP): 10s × 0.5 × 0.75 = 3.75s per ore
   ```

2. **Quality Preservation:**
   - Determines how well the material's base quality is maintained during extraction
   - Lower skill may damage materials, reducing their quality for assembly use
   - Higher skill preserves or even slightly enhances base quality
   
   ```
   Final_Material_Quality = Base_Quality × Quality_Preservation_Rate
   
   Quality Preservation by Skill Level:
   - Levels 1-20: 60-75% preservation (significant quality loss)
   - Levels 21-50: 75-90% preservation (moderate loss)
   - Levels 51-100: 90-100% preservation (minimal to no loss)
   - Material Familiarity Bonus: +5% to +15% additional preservation
   ```

3. **Yield (Quantity):**
   - Base yield increases slightly with general skill level
   - Material familiarity provides chance for bonus yields
   - Rare "critical success" chances increase with experience
   
   ```
   Base_Yield = Standard_Yield × (1 + General_Skill_Bonus)
   Bonus_Yield_Chance = 5% + (Material_Familiarity / 20)% (capped at 25%)
   Critical_Success_Chance = 1% + (General_Skill_Level / 20)% (capped at 6%)
   
   Example - Herbalism (Collecting Healing Herbs):
   - Novice (Level 5): 1-2 herbs, 7% bonus chance, 1.25% critical
   - Intermediate (Level 30): 1-3 herbs, 15% bonus chance, 2.5% critical
   - Expert (Level 70, 400 Herb XP): 2-4 herbs, 25% bonus chance, 4.5% critical
   ```

**Impact Summary:**
- Efficiency improvements are most noticeable (up to 60% faster)
- Quality preservation is crucial for high-end crafting (prevents waste)
- Yield improvements are modest but meaningful over time

#### Experience Growth Rates and Balance

**How fast should different experience types grow?**

The current system uses a 2:1:0.5 ratio for General:Group:Specific experience:

```
General_Gathering_XP = Base_XP × Difficulty_Multiplier × Quality_Multiplier
Material_Group_XP = General_XP × 0.5
Specific_Material_XP = General_XP × 0.25
```

**Rationale:**

1. **General Gathering (100% of base XP):**
   - Fastest progression for broad applicability
   - Encourages exploration and trying different materials
   - Levels 1-30: Rapid advancement (learning fundamentals)
   - Levels 31-70: Steady progression (mastering techniques)
   - Levels 71-100: Slow progression (perfecting mastery)

2. **Material Group (50% of base XP):**
   - Medium-speed progression for category expertise
   - Example: Mining all metal ores improves metal ore group experience
   - Takes approximately twice as long to "master" a group vs general skill
   - Provides meaningful bonuses without requiring extreme specialization

3. **Specific Material (25% of base XP):**
   - Slowest progression for deep specialization
   - Rewards players who focus on specific materials
   - Takes approximately 4x longer to max out than general skill
   - Creates expert niches (e.g., "the best iron miner on the server")

**Progression Timeline Examples:**

```
To reach Level 50 in Mining (General):
- ~200 hours of diverse mining activities
- Harvesting many different ore types
- Natural progression through normal gameplay

To max out Iron Ore Familiarity (500 extractions):
- ~50 hours focused on iron ore only
- OR ~200 hours if iron is 25% of mining activities
- Creates specialization without requiring exclusive focus

To max out Metal Ores Group:
- ~100 hours mining various metal ores
- Spread across iron, copper, gold, silver, tin
- Encourages variety within a category
```

**Diminishing Returns:**

Yes, the system includes diminishing returns to prevent exploitation:

```
Familiarity_Bonus_Per_Extraction = (Current_Extractions < 100) ? 0.05% : 
                                   (Current_Extractions < 300) ? 0.03% :
                                   (Current_Extractions < 500) ? 0.01% : 0%

Example:
- Extractions 1-100: Gain 5% total bonus (0.05% per extraction)
- Extractions 101-300: Gain 6% total bonus (0.03% per extraction)
- Extractions 301-500: Gain 2% total bonus (0.01% per extraction)
- Extractions 501+: Gain 12% from other sources (group, skill level)
- Maximum familiarity bonus: 25% at 500 extractions
```

This prevents "grinding" a single material to quickly max out while still rewarding dedication.

#### Material Group Experience and Special Abilities

**Should material group experience unlock special abilities?**

Yes, material group experience unlocks passive abilities and active techniques at key milestones:

**Milestone System:**

```
Group_Familiarity_Milestones:
- Level 1 (0 XP): Basic group recognition
- Level 2 (500 Group XP): Enhanced detection (locate nearby materials)
- Level 3 (1500 Group XP): Quality assessment (see material quality before extraction)
- Level 4 (3000 Group XP): Efficient extraction (reduced tool wear for group)
- Level 5 (5000 Group XP): Master techniques (group-specific special abilities)
```

**Examples by Gathering Skill:**

**Mining - Metal Ores Group:**
- Level 2: "Ore Sense" - Detect metal ore deposits within 20m radius
- Level 3: "Vein Reading" - See ore quality and vein size before mining
- Level 4: "Efficient Mining" - 20% less tool durability loss on metal ores
- Level 5: "Vein Following" - Automatically trace ore veins to richest deposits

**Herbalism - Medicinal Herbs Group:**
- Level 2: "Herb Scent" - Detect medicinal herbs within 15m, even if hidden
- Level 3: "Potency Assessment" - See herb medicinal strength before harvesting
- Level 4: "Gentle Harvest" - Plants regrow 25% faster after harvesting
- Level 5: "Perfect Timing" - Know optimal harvest time for maximum potency

**Logging - Hardwood Group:**
- Level 2: "Grain Reading" - See wood grain quality through bark
- Level 3: "Age Assessment" - Know tree age and wood quality before felling
- Level 4: "Clean Felling" - 15% less wood waste from precision cuts
- Level 5: "Master Lumberjack" - Fell trees in chosen direction, no damage to wood

**Design Philosophy:**
- Abilities enhance gameplay without being mandatory
- Each ability saves time or improves outcomes
- Encourages specialization in material groups
- Creates distinct playstyles (generalist vs. specialist)

#### Rare Material Handling

**How should the system handle rare materials?**

Rare materials use a balanced approach that makes them valuable without making them impossible to master:

**Rare Material Characteristics:**

1. **Higher Base XP Rewards:**
   ```
   Rare_Material_XP_Multiplier = 2.0 to 5.0 (vs 1.0 for common materials)
   
   Example:
   - Common Iron Ore: 10 base XP per extraction
   - Uncommon Mithril Ore: 25 base XP per extraction (2.5× multiplier)
   - Rare Adamantite Ore: 50 base XP per extraction (5× multiplier)
   ```

2. **Faster Familiarity Gains:**
   ```
   Rare materials count as multiple extractions for familiarity:
   - Common materials: 1 extraction = 1 familiarity point
   - Uncommon materials: 1 extraction = 2 familiarity points
   - Rare materials: 1 extraction = 3 familiarity points
   - Legendary materials: 1 extraction = 5 familiarity points
   ```

3. **Lower Familiarity Requirements:**
   ```
   Familiarity caps adjusted for rarity:
   - Common materials: 500 extractions for max bonus
   - Uncommon materials: 300 extractions for max bonus (600 effective)
   - Rare materials: 200 extractions for max bonus (600 effective)
   - Legendary materials: 100 extractions for max bonus (500 effective)
   ```

4. **Economic Incentives:**
   - Rare materials always worth gathering, even at low skill
   - Market value reflects both rarity and quality
   - Even "failed" rare extractions yield some value
   - Encourages risk-taking by skilled gatherers

**Example: Legendary Moonstone Crystal (Rare Mining Material)**

```
First Extraction (Zero Moonstone Experience):
- Success Rate: 40% (vs 85% for common iron)
- Quality Preservation: 50% (vs 70% for iron)
- Time Required: 60 seconds (vs 10s for iron)
- XP Gained: 250 (vs 10 for iron)
- Familiarity Gained: 5 points (vs 1 for iron)
- Market Value: 1000 gold even at 50% quality

100th Extraction (500 Moonstone Familiarity Points = Max):
- Success Rate: 65% (+25% from familiarity)
- Quality Preservation: 75% (+25% from familiarity)
- Time Required: 45 seconds (25% faster)
- XP Gained: 250 (same base)
- Familiarity Gained: 0 (capped)
- Market Value: 2000 gold at 75% quality

Result: Rare material mastery is achievable (100 extractions vs 500 for common)
        but requires dedication and provides significant rewards
```

**Design Goals:**
- Rare materials remain challenging but not impossible to master
- First-time gatherers can still succeed (just less efficiently)
- Experts have clear advantages without monopolizing access
- Economic value justifies the difficulty and time investment

#### Diminishing Returns and Anti-Exploitation

**Should there be diminishing returns?**

Yes, multiple diminishing returns systems prevent exploitation while maintaining engagement:

**1. Material Familiarity Diminishing Returns:**

```
Extraction_Progress = (Current_Extractions / Target_Extractions)

XP_Per_Extraction = Base_XP × (1.0 - (Progress × 0.5))

Example - Iron Ore Familiarity (Target: 500 extractions):
- Extraction 1-100 (0-20%): 100% XP (fast gains)
- Extraction 101-200 (20-40%): 90% XP (slight slowdown)
- Extraction 201-300 (40-60%): 80% XP (noticeable slowdown)
- Extraction 301-400 (60-80%): 70% XP (diminishing returns)
- Extraction 401-500 (80-100%): 60% XP (slow final gains)
```

**2. Repetitive Action Penalties:**

```
Same_Material_Spam_Penalty:
- Gathering same material >50 times in 1 hour: -10% XP
- Gathering same material >100 times in 1 hour: -25% XP
- Gathering same material >200 times in 1 hour: -50% XP
- Penalty resets after 30 minutes of diverse activity

Purpose: Prevents mindless grinding, encourages variety
Exception: Does not apply to rare materials
```

**3. Skill Level Diminishing Returns:**

```
General_Skill_XP_Per_Level:
- Levels 1-20: 100-200 XP per level (fast early progression)
- Levels 21-40: 200-400 XP per level (steady progression)
- Levels 41-60: 400-800 XP per level (slower progression)
- Levels 61-80: 800-1600 XP per level (slow progression)
- Levels 81-100: 1600-3200 XP per level (very slow mastery)

Total to Level 100: ~64,000 XP
Time Investment: ~400-600 hours of focused gathering
```

**4. Material Depletion Mechanic:**

```
Deposit_Quality_Degradation:
- First 10 extractions: 100% quality
- Extractions 11-30: 95% quality (slight degradation)
- Extractions 31-60: 85% quality (noticeable degradation)
- Extractions 61-100: 70% quality (significant degradation)
- Extractions 101+: 50% quality (depleted deposit)

Result: Players naturally move between deposits
        Prevents camping one location
        Encourages exploration
```

**5. Efficiency Caps:**

```
Maximum_Combined_Bonuses:
- General Skill: Up to +50% efficiency (at Level 100)
- Material Familiarity: Up to +25% efficiency (at 500 extractions)
- Material Group: Up to +15% efficiency (at max group level)
- Equipment Bonuses: Up to +20% efficiency (best tools)
- Total Maximum: +110% efficiency (vs +0% for novice)

Practical Limit: +75-85% for most expert players
                Prevents exponential scaling
                Maintains challenge at high levels
```

**Anti-Exploitation Summary:**

The system includes five layers of diminishing returns:
1. Material familiarity gains slow as you approach cap
2. Repetitive actions are penalized within same play session
3. General skill levels require exponentially more XP
4. Material deposits degrade with overuse
5. Efficiency bonuses have hard caps

**Result:** Progression feels rewarding throughout the journey while preventing exploitation and maintaining 
long-term engagement. Players are naturally encouraged to diversify their activities while still being able 
to specialize meaningfully.

### Gathering and Assembly Integration

**Material Quality Chain:**
```
Geological Formation → Gathering Skill → Material Quality → Assembly Skill → Final Product
```

**Example: Iron Sword Production**
1. **Geological Formation:** Hematite deposit (85% purity)
2. **Mining (Level 40, Iron Familiarity 300):** 
   - Base extraction: 82% quality
   - Familiarity bonus: +15% efficiency → 94% quality iron ore
3. **Blacksmithing - Smelting (Level 25):**
   - Processing: 94% → 88% quality iron ingot
4. **Blacksmithing - Forging (Level 35):**
   - Crafting: 88% material + skill + tools → 85% quality Fine Iron Sword

**Gathering-Assembly Synergy:**
- High-quality materials partially compensate for lower assembly skills
- Master gatherers can supply premium materials that increase crafting success rates
- Material familiarity unlocks special processing techniques in assembly
- Economic specialization: gather-focused vs. craft-focused vs. vertically integrated players

## Skill Progression Mechanics

### Experience Gain Model

**Practice-Based System:**
```
Experience Gained = Base_XP × Material_Difficulty × Item_Complexity × Success_Modifier

Where:
- Base_XP = 10 (constant)
- Material_Difficulty = 0.5 to 3.0 (harder materials give more XP)
- Item_Complexity = 0.5 to 5.0 (complex items give more XP)
- Success_Modifier = 1.0 (success), 0.3 (failure but learned), 1.5 (critical success)
```

**Level Requirements:**
```
XP_Required(Level) = 100 × Level^1.5

Example progression:
Level 1 → 2: 100 XP
Level 10 → 11: 3,162 XP
Level 50 → 51: 35,355 XP
Level 99 → 100: 98,995 XP
```

This creates a natural learning curve where:
- Early levels advance quickly (learning basics)
- Mid levels require sustained practice (developing skill)
- High levels demand mastery (perfecting technique)

### Success Rate Formula

**Base Success Rate:**
```
Success_Rate = Base_Rate + Skill_Bonus - Material_Penalty - Complexity_Penalty + Tool_Bonus + 
               Environment_Bonus

Where:
- Base_Rate = 50% (minimum)
- Skill_Bonus = (Current_Skill / Recommended_Skill) × 30%
- Material_Penalty = Material_Quality_Variance × 10%
- Complexity_Penalty = Item_Complexity × 5%
- Tool_Bonus = Tool_Quality × 15%
- Environment_Bonus = Workshop_Quality × 10%

Capped between 10% (minimum) and 95% (maximum)
```

**Example Calculations:**

*Novice Blacksmith (Level 5) crafting Iron Sword (Recommended Level 8):*
```
Base_Rate = 50%
Skill_Bonus = (5/8) × 30% = 18.75%
Material_Penalty = 0% (standard iron)
Complexity_Penalty = 2 × 5% = 10%
Tool_Bonus = 10% (basic tools)
Environment_Bonus = 5% (basic forge)

Success_Rate = 50% + 18.75% - 10% + 10% + 5% = 73.75%
```

*Master Blacksmith (Level 80) crafting Iron Sword:*
```
Base_Rate = 50%
Skill_Bonus = (80/8) × 30% = 30% (capped at recommended skill)
Material_Penalty = 0%
Complexity_Penalty = 10%
Tool_Bonus = 15% (masterwork tools)
Environment_Bonus = 10% (advanced forge)

Success_Rate = 50% + 30% - 10% + 15% + 10% = 95% (maximum)
```

### Quality Tiers and Bonuses

**Item Quality System:**

1. **Crude (0-20% Quality):**
   - Functional but flawed
   - -20% to base item stats
   - Rapid durability loss
   - Occurs when skill is much lower than recommended
   
2. **Standard (21-60% Quality):**
   - Meets basic requirements
   - Base item stats (no modifier)
   - Normal durability
   - Common outcome for appropriate skill level
   
3. **Fine (61-85% Quality):**
   - Above-average craftsmanship
   - +10% to base item stats
   - Improved durability (+25%)
   - Occurs when skill exceeds recommended level
   
4. **Superior (86-95% Quality):**
   - Excellent craftsmanship
   - +25% to base item stats
   - Significantly improved durability (+50%)
   - Rare, requires high skill and good materials
   
5. **Masterwork (96-100% Quality):**
   - Peak craftsmanship
   - +50% to base item stats
   - Maximum durability (+100%)
   - Additional special property slot
   - Very rare, requires master skill, perfect materials, ideal conditions

**Quality Calculation:**
```
Item_Quality = (Skill_Level / Max_Skill) × 40% +
               (Material_Quality / 100) × 30% +
               (Tool_Quality / 100) × 20% +
               (Environment_Quality / 100) × 10% +
               Random_Variance(-5% to +5%)
```

### Specialization Benefits

**Specialization Unlocks:**
- Available at Level 25 in base profession
- Choose one specialization path
- Grants unique recipes and techniques
- Provides efficiency bonuses in specialization area

**Specialization Bonuses:**
```
Within Specialization:
- +15% success rate for specialized items
- +10% quality bonus
- 20% reduced material requirements
- Access to exclusive recipes
- Faster crafting time (-25%)

Outside Specialization:
- No penalties
- Standard progression continues
- Can still craft all base profession items
```

### Critical Success and Failure

**Critical Success (5% base chance + 1% per 10 skill levels):**
- Automatically produces Superior or Masterwork quality
- Grants bonus experience (×1.5)
- May discover new recipe variants
- No material waste

**Critical Failure (Base 10% - 1% per 10 skill levels, minimum 1%):**
- Item creation fails completely
- Materials partially lost (50% recovery)
- Grants minimal experience (×0.1)
- Potential tool damage (rare)

## Crafting Interface Design

### Interface Components

**1. Recipe Selection Panel**
```
┌─────────────────────────────────────┐
│ AVAILABLE RECIPES                   │
│ ─────────────────────────────────── │
│ ☐ Filter: Weapons | Armor | Tools  │
│ ☐ Show: Learnable | Known | All    │
│                                     │
│ [Icon] Iron Sword                   │
│        Recommended: Level 8         │
│        Your Level: 5                │
│        Success: ~74%                │
│                                     │
│ [Icon] Steel Dagger                 │
│        Recommended: Level 12        │
│        Your Level: 5                │
│        Success: ~45%                │
└─────────────────────────────────────┘
```

**2. Material Selection Interface**
```
┌─────────────────────────────────────┐
│ CRAFTING: Iron Sword                │
│ ─────────────────────────────────── │
│ Required Materials:                 │
│                                     │
│ [Icon] Iron Ingot × 3               │
│        Quality: 75% ████████░░      │
│        Source: Hematite Deposit     │
│        Effect: +5% success          │
│                                     │
│ [Icon] Oak Handle × 1               │
│        Quality: 60% ██████░░░░      │
│        Source: Oak Tree (aged 40y)  │
│        Effect: Standard             │
│                                     │
│ Optional Materials:                 │
│ [Icon] Quenching Oil                │
│        Effect: +10% quality         │
└─────────────────────────────────────┘
```

**3. Crafting Progress Display**
```
┌─────────────────────────────────────┐
│ CRAFTING IN PROGRESS                │
│ ─────────────────────────────────── │
│ Iron Sword                          │
│                                     │
│ Stage 1: Heating Metal              │
│ [████████████░░░░] 75%              │
│                                     │
│ Temperature: 1200°C (Optimal)       │
│ Timing: Good ✓                      │
│                                     │
│ Quality Projection: Fine (72%)      │
│ Success Probability: 74%            │
│                                     │
│ [Cancel] [Continue]                 │
└─────────────────────────────────────┘
```

**4. Result Display**
```
┌─────────────────────────────────────┐
│ CRAFTING COMPLETE                   │
│ ─────────────────────────────────── │
│            [Icon]                   │
│         FINE IRON SWORD             │
│                                     │
│ Quality: 72% (Fine)                 │
│ Durability: 125/125 (+25%)          │
│ Damage: 33 (+10% from quality)      │
│                                     │
│ Bonuses:                            │
│ ✓ +10% Attack Speed                 │
│ ✓ Improved Durability               │
│                                     │
│ Experience Gained: 67 XP            │
│ Blacksmithing: Level 5 → 6          │
│                                     │
│ [Take Item] [Craft Another]         │
└─────────────────────────────────────┘
```

### Information Display Priorities

**Always Visible:**
- Current success rate (updated dynamically as materials selected)
- Recommended skill level vs. current skill level
- Material quality indicators
- Expected item quality range

**Shown on Hover/Detail View:**
- Detailed breakdown of success rate calculation
- Material properties and effects
- Potential quality outcomes and probabilities
- Tool and environment bonuses

**Visual Indicators:**
- Color coding for skill level appropriateness:
  - Green: Below recommended (easy, low XP)
  - Yellow: At recommended (appropriate challenge)
  - Orange: Above recommended (challenging, high XP)
  - Red: Far above recommended (very difficult)

## Material Quality and Geological Integration

### Material Quality Sources

**1. Geological Formation Quality**
```
Material_Base_Quality = Formation_Purity × Deposit_Concentration × Extraction_Method

Where:
- Formation_Purity: 0.5-1.0 (geological formation quality)
- Deposit_Concentration: 0.6-1.0 (ore concentration in deposit)
- Extraction_Method: 0.7-1.0 (mining technique efficiency)
```

**Example: Iron Ore Quality**
- **Poor Quality (30-50%):** Surface deposits, heavily oxidized, mixed with impurities
  - Found in: Weathered surface exposures, ancient placer deposits
  - Effect: Brittle metal, requires extensive purification
  
- **Standard Quality (51-75%):** Common ore bodies, moderate purity
  - Found in: Sedimentary iron formations, common deposits
  - Effect: Reliable material for standard items
  
- **High Quality (76-90%):** Pure ore veins, minimal impurities
  - Found in: Magmatic segregations, metamorphic enrichments
  - Effect: Superior metal quality, easier to work with
  
- **Exceptional Quality (91-100%):** Nearly pure deposits, rare formations
  - Found in: Pristine magmatic concentrations, ancient meteor deposits
  - Effect: Premium material for masterwork items

### Material Processing and Refinement

**Processing Stages:**

1. **Raw Material Extraction**
   - Quality affected by mining skill and method
   - Careful extraction preserves material integrity
   - Rushed extraction may damage material structure

2. **Primary Processing**
   - Ore smelting, leather tanning, fiber spinning
   - Processing skill affects material quality retention
   - Process quality: (Input_Quality × Processing_Skill × Equipment_Quality)

3. **Refinement**
   - Purification, tempering, finishing
   - Can improve material quality by up to 20%
   - Requires advanced skill and specialized equipment

**Processing Example: Iron Ore → Iron Ingot**
```
Raw Ore Quality: 70%
Smelting Skill: Level 15 (75% efficiency)
Furnace Quality: 80%

Ingot_Quality = 70% × 0.75 × 0.80 = 42%

Refinement (Level 25 skill):
Final_Quality = 42% + (42% × 0.20) = 50.4%
```

### Environmental Factor Integration

**Workshop/Forge Quality Factors:**

1. **Equipment Quality** (0-100%)
   - Tool sharpness and condition
   - Forge temperature control
   - Workspace organization

2. **Environmental Conditions** (±20%)
   - Temperature and humidity (affects leather, wood)
   - Ventilation (affects metal working heat)
   - Lighting (affects precision work)

3. **Location Benefits** (±15%)
   - Near material sources (fresh materials)
   - In specialized districts (inspiration bonus)
   - Guild workshop access (master guidance)

## Real-World Skill Analysis

### Blacksmithing - Historical Context

**Apprenticeship Model:**
- 7-10 years traditional apprenticeship
- Early years: basic techniques, tool maintenance
- Middle years: independent work under supervision
- Final years: complex projects, style development

**Skill Stages:**
1. **Novice (Years 1-2):** Basic hammer control, fire management, simple shapes
2. **Journeyman (Years 3-5):** Independent work, complex projects, quality consistency
3. **Master (Years 6-10):** Innovation, teaching, masterwork creation

**Game Translation:**
- Levels 1-20: Novice period (rapid learning)
- Levels 21-60: Journeyman period (skill refinement)
- Levels 61-100: Master period (perfection, innovation)

### Tailoring - Skill Development

**Real-World Progression:**
- Pattern reading: 6 months - 1 year
- Basic garment construction: 1-2 years
- Custom pattern design: 3-5 years
- Master tailoring: 7-10 years

**Critical Skills:**
1. **Fabric Selection:** Understanding material properties
2. **Pattern Making:** Translating designs to patterns
3. **Cutting Precision:** Minimizing waste, maximizing quality
4. **Assembly Technique:** Stitch quality, seam strength

**Game Translation:**
- Material handling improves with skill
- Pattern efficiency reduces material waste at higher levels
- Stitch quality affects item durability
- Advanced techniques unlock at milestone levels

### Alchemy - Learning Curve

**Historical Alchemical Training:**
- 5-7 years traditional training
- Extensive ingredient knowledge required
- Process timing critical for success
- Experimentation leads to discovery

**Knowledge Areas:**
1. **Materia Medica:** Ingredient identification and properties
2. **Processing Techniques:** Distillation, calcination, extraction
3. **Formulation Theory:** Component interactions and synergies
4. **Timing and Temperature:** Critical process parameters

**Game Translation:**
- Recipe discovery through experimentation
- Ingredient substitution possible at high skill
- Process timing affects potion potency
- Failed experiments provide learning experience

### Woodworking - Craft Mastery

**Traditional Training:**
- 4-6 years apprenticeship
- Wood species knowledge critical
- Joint techniques define quality
- Tool maintenance essential

**Skill Progression:**
1. **Basic Carpentry:** Straight cuts, simple joints, basic projects
2. **Intermediate Work:** Complex joinery, curved work, finishing
3. **Advanced Mastery:** Artistic work, innovation, wood movement prediction

**Game Translation:**
- Wood selection affects final item properties
- Joint quality determines furniture durability
- Tool maintenance becomes important mechanic
- Weather and moisture affect work quality

## Recommendations

### Immediate Actions

1. **Implement Core Skill Framework**
   - Description: Create the four basic professions with practice-based progression
   - Rationale: Establishes foundation for entire crafting system
   - Priority: High
   - Timeline: Phase 1 (Months 1-2)
   - Dependencies: None
   - Deliverables:
     - Skill experience tracking system
     - Level progression database
     - Base success rate calculations

2. **Develop Material Quality System**
   - Description: Integrate material quality with geological simulation
   - Rationale: Connects crafting to BlueMarble's core geological mechanics
   - Priority: High
   - Timeline: Phase 1 (Months 2-3)
   - Dependencies: Geological simulation, material database
   - Deliverables:
     - Material quality calculation algorithms
     - Geological formation → material quality mapping
     - Processing quality retention formulas

3. **Create Basic Crafting Interface**
   - Description: Implement recipe selection and crafting progress UI
   - Rationale: Players need clear feedback on crafting mechanics
   - Priority: High
   - Timeline: Phase 2 (Months 3-4)
   - Dependencies: Core skill framework
   - Deliverables:
     - Recipe browser interface
     - Material selection panel
     - Crafting progress display
     - Result feedback screen

4. **Implement Specialization System**
   - Description: Add specialization paths for each profession
   - Rationale: Provides long-term progression goals and player differentiation
   - Priority: Medium
   - Timeline: Phase 2 (Months 4-5)
   - Dependencies: Core skill framework at Level 25+
   - Deliverables:
     - Specialization selection interface
     - Bonus calculation system
     - Exclusive recipe database

5. **Balance Testing and Tuning**
   - Description: Extensive playtesting of progression curves and success rates
   - Rationale: Ensures engaging progression without frustration
   - Priority: High
   - Timeline: Phase 3 (Months 5-6)
   - Dependencies: All core systems implemented
   - Deliverables:
     - Balance report
     - Adjusted formulas
     - Player feedback integration

### Long-term Considerations

**1. Advanced Material Science**
- Implement alloy creation systems (bronze, steel, Damascus)
- Material aging and seasoning mechanics (wood, leather)
- Material combination synergies
- Experimental material discovery

**2. Master Crafting Systems**
- Player-designed recipes
- Signature techniques (personalized bonuses)
- Teaching system (masters training apprentices)
- Crafting competitions and rankings

**3. Economic Integration**
- Market demand affects recipe profitability
- Supply chain management for materials
- Crafting guilds and cooperatives
- Commission system for custom orders

**4. Cross-Profession Collaboration**
- Combined crafting projects (blacksmith + woodworker for weapons)
- Material trading and specialization
- Shared workshops and equipment
- Collaborative masterwork creation

### Areas for Further Research

1. **Enchanting Integration**
   - How do magical properties integrate with physical crafting?
   - Should enchanting be a separate skill or part of each profession?
   - What are the limits of magical enhancement?

2. **Tool Degradation and Maintenance**
   - How quickly should tools wear out?
   - What's the right balance for maintenance gameplay?
   - Should tool quality significantly impact success rates?

3. **Recipe Discovery Systems**
   - How should players learn new recipes?
   - What balance between taught recipes and discovered recipes?
   - Should experimentation be rewarded even if it fails?

4. **Crafting Time vs. Engagement**
   - How long should crafting take for different items?
   - Should complex items have interactive stages?
   - What's the right balance between realism and gameplay flow?

## Implementation Roadmap

### Phase 1: Foundation (Months 1-3)

**Month 1: Core Framework**
- Skill tracking system
- Experience gain calculations
- Level progression database
- Basic recipe database

**Month 2: Material Integration**
- Material quality system
- Geological formation → material quality
- Processing quality calculations
- Material database expansion

**Month 3: Success Rate System**
- Success rate formula implementation
- Quality tier calculations
- Tool and environment bonuses
- Critical success/failure mechanics

**Deliverables:**
- Functional skill progression for all four professions
- Material quality integrated with geological simulation
- Basic crafting success rates operational

### Phase 2: Interface and Specialization (Months 4-5)

**Month 4: Crafting Interface**
- Recipe selection UI
- Material selection interface
- Crafting progress display
- Result feedback screens

**Month 5: Specialization System**
- Specialization selection interface
- Bonus calculations
- Exclusive recipes per specialization
- Specialization progression tracking

**Deliverables:**
- Complete crafting interface
- All specialization paths available
- Visual feedback for quality and success rates

### Phase 3: Balance and Polish (Months 6-7)

**Month 6: Playtesting**
- Internal balance testing
- Formula adjustments
- Progression curve refinement
- Player feedback collection

**Month 7: Polish and Documentation**
- UI polish and clarity improvements
- Tooltip and help system
- Tutorial creation
- Player documentation

**Deliverables:**
- Balanced progression system
- Polished user experience
- Complete player documentation

### Phase 4: Advanced Features (Months 8-10)

**Month 8: Advanced Materials**
- Alloy systems
- Material aging
- Exotic materials

**Month 9: Master Systems**
- Player recipe design
- Signature techniques
- Teaching mechanics

**Month 10: Economic Integration**
- Commission system
- Crafting guilds
- Market integration

**Deliverables:**
- Advanced crafting features
- Economic gameplay integration
- Social crafting systems

## Appendices

### Appendix A: Success Rate Examples

**Example 1: Novice Tailor Crafting Linen Shirt**
```
Skill Level: 3
Recommended Level: 2
Material Quality: 60% (standard linen)
Tool Quality: 50% (basic needle and thread)
Environment: 40% (no workshop)

Success_Rate = 50% + (3/2 × 30%) + 0 - (1 × 5%) + (0.5 × 15%) + (0.4 × 10%)
            = 50% + 45% - 5% + 7.5% + 4%
            = 101.5% → 95% (capped)

Result: Very high success chance, appropriate for learning
```

**Example 2: Intermediate Alchemist Crafting Healing Potion**
```
Skill Level: 35
Recommended Level: 30
Material Quality: 80% (high-quality herbs)
Tool Quality: 70% (good alembic)
Environment: 60% (basic laboratory)

Success_Rate = 50% + (35/30 × 30%) + 0 - (2 × 5%) + (0.7 × 15%) + (0.6 × 10%)
            = 50% + 35% - 10% + 10.5% + 6%
            = 91.5%

Quality_Expected = (35/100 × 40%) + (80/100 × 30%) + (70/100 × 20%) + (60/100 × 10%)
                 = 14% + 24% + 14% + 6%
                 = 58% (Standard Quality, approaching Fine)
```

**Example 3: Master Blacksmith Crafting Legendary Sword**
```
Skill Level: 95
Recommended Level: 90
Material Quality: 98% (meteoritic steel)
Tool Quality: 95% (masterwork tools)
Environment: 90% (legendary forge)

Success_Rate = 50% + (95/90 × 30%) + 0 - (5 × 5%) + (0.95 × 15%) + (0.9 × 10%)
            = 50% + 31.7% - 25% + 14.25% + 9%
            = 79.95%

Quality_Expected = (95/100 × 40%) + (98/100 × 30%) + (95/100 × 20%) + (90/100 × 10%)
                 = 38% + 29.4% + 19% + 9%
                 = 95.4% (Masterwork Quality)

Note: High complexity reduces success rate but doesn't affect quality calculation
```

### Appendix B: Material Property Tables

**Metal Quality by Source:**

| Metal | Poor (30-50%) | Standard (51-75%) | High (76-90%) | Exceptional (91-100%) |
|-------|---------------|-------------------|---------------|----------------------|
| Iron | Surface rust | Sedimentary | Magmatic | Meteoritic |
| Copper | Oxidized ore | Common deposits | Pure veins | Native copper |
| Gold | Placer dust | Alluvial | Lode deposits | Pure nuggets |
| Silver | Tarnished ore | Standard veins | Rich deposits | Native silver |

**Wood Quality by Species and Age:**

| Wood Type | Young (<20y) | Mature (20-50y) | Old (50-100y) | Ancient (>100y) |
|-----------|--------------|-----------------|---------------|-----------------|
| Oak | 40-50% | 60-75% | 80-90% | 92-100% |
| Pine | 50-60% | 65-75% | 75-85% | 85-95% |
| Maple | 45-55% | 65-80% | 85-95% | 95-100% |
| Yew | 35-45% | 70-85% | 90-98% | 98-100% |

**Fiber Quality by Processing:**

| Fiber | Raw | Cleaned | Spun | Fine Spun |
|-------|-----|---------|------|-----------|
| Cotton | 40-50% | 60-70% | 75-85% | 90-100% |
| Flax | 45-55% | 65-75% | 80-88% | 92-100% |
| Wool | 35-45% | 55-70% | 70-85% | 85-95% |
| Silk | 70-80% | 85-90% | 92-96% | 97-100% |

### Appendix C: Progression Curves

**Experience Requirements by Level:**

| Level Range | XP per Level (avg) | Total XP | Time Investment (estimated) |
|-------------|-------------------|----------|-----------------------------|
| 1-10 | 200 | 2,000 | 10-15 hours |
| 11-25 | 1,500 | 22,500 | 40-60 hours |
| 26-50 | 8,000 | 200,000 | 120-180 hours |
| 51-75 | 25,000 | 625,000 | 300-400 hours |
| 76-100 | 70,000 | 1,750,000 | 600-800 hours |

**Success Rate Progression:**

| Skill vs Recommended | Success Rate (avg) | Experience Gain | Risk/Reward |
|---------------------|-------------------|-----------------|-------------|
| -20 levels | 95% | Low (0.5×) | Safe, slow |
| -10 levels | 90% | Medium (0.8×) | Comfortable |
| Equal | 75-80% | Standard (1.0×) | Balanced |
| +10 levels | 55-65% | High (1.5×) | Challenging |
| +20 levels | 30-40% | Very High (2.0×) | Risky |

### Appendix D: Visual Interface Mockups

**Crafting Interface Color Coding:**

```
Success Rate Colors:
┌──────────────────────────────────────┐
│ 90-100%: ████████ Bright Green      │ Very Safe
│ 75-89%:  ████████ Green              │ Safe
│ 60-74%:  ████████ Yellow             │ Moderate
│ 40-59%:  ████████ Orange             │ Risky
│ 0-39%:   ████████ Red                │ Very Risky
└──────────────────────────────────────┘

Quality Colors:
┌──────────────────────────────────────┐
│ Crude:      ████████ Dark Gray       │
│ Standard:   ████████ White           │
│ Fine:       ████████ Light Blue      │
│ Superior:   ████████ Purple          │
│ Masterwork: ████████ Gold            │
└──────────────────────────────────────┘
```

**Material Quality Visual Indicator:**
```
Poor Quality:     [████░░░░░░] 40%
Standard Quality: [███████░░░] 70%
High Quality:     [█████████░] 90%
Perfect Quality:  [██████████] 100%
```

### Appendix E: Recipe Database Sample

**Blacksmithing Recipes:**

| Item | Level | Materials | Time | Complexity |
|------|-------|-----------|------|------------|
| Iron Dagger | 1 | Iron Ingot ×1, Wood Handle ×1 | 5 min | 1 |
| Iron Sword | 8 | Iron Ingot ×3, Wood Handle ×1 | 15 min | 2 |
| Steel Sword | 25 | Steel Ingot ×3, Oak Handle ×1 | 30 min | 3 |
| Plate Armor | 45 | Steel Ingot ×15, Leather Straps ×5 | 90 min | 5 |
| Masterwork Sword | 80 | Damascus Steel ×5, Rare Wood ×1 | 180 min | 5 |

**Tailoring Recipes:**

| Item | Level | Materials | Time | Complexity |
|------|-------|-----------|------|------------|
| Cloth Bandage | 1 | Linen ×1 | 1 min | 0.5 |
| Linen Shirt | 3 | Linen ×3, Thread ×1 | 10 min | 1 |
| Leather Armor | 20 | Leather ×8, Thread ×3 | 45 min | 3 |
| Silk Robe | 40 | Silk ×10, Gold Thread ×2 | 90 min | 4 |
| Masterwork Cloak | 75 | Fine Silk ×8, Enchanted Thread ×3 | 120 min | 5 |

**Alchemy Recipes:**

| Item | Level | Materials | Time | Complexity |
|------|-------|-----------|------|------------|
| Minor Health Potion | 1 | Red Herb ×2, Water ×1 | 3 min | 1 |
| Health Potion | 15 | Healing Herb ×3, Pure Water ×1 | 10 min | 2 |
| Strength Elixir | 30 | Giant's Toe ×1, Iron Ore ×2, Alcohol ×1 | 25 min | 3 |
| Greater Mana Potion | 50 | Mana Crystal ×1, Blue Herb ×5 | 40 min | 4 |
| Elixir of Mastery | 85 | Rare Components ×8, Dragon Blood ×1 | 120 min | 5 |

**Woodworking Recipes:**

| Item | Level | Materials | Time | Complexity |
|------|-------|-----------|------|------------|
| Wooden Club | 1 | Oak Log ×1 | 5 min | 1 |
| Bow | 12 | Yew Wood ×2, Bowstring ×1 | 30 min | 2 |
| Wooden Shield | 18 | Oak Planks ×5, Iron Rivets ×8 | 45 min | 3 |
| Furniture Set | 40 | Various Woods ×20, Glue ×5 | 120 min | 4 |
| Masterwork Longbow | 70 | Ancient Yew ×3, Enchanted String ×1 | 150 min | 5 |

### Appendix F: Integration with Existing Systems

**Connection to BlueMarble Geological Simulation:**

```
Geological Layer → Material Quality → Crafting Input
───────────────────────────────────────────────────
Hematite Deposit (85% purity) → High Quality Iron Ore (82%) → Superior Iron Ingot (78%)
Oak Forest (mature trees) → High Quality Oak (75%) → Fine Oak Planks (72%)
Flax Fields (optimal climate) → Standard Flax (65%) → Standard Linen (63%)
Crystal Caves (pure formation) → Exceptional Crystals (95%) → Masterwork Mana Gems (92%)
```

**Connection to Economy Systems:**

```
Skill Level → Item Quality → Market Value
─────────────────────────────────────────
Low Skill → Crude/Standard Items → Low Market Value (base price ×0.5-1.0)
Medium Skill → Standard/Fine Items → Standard Market Value (base price ×1.0-1.5)
High Skill → Fine/Superior Items → High Market Value (base price ×1.5-2.5)
Master Skill → Superior/Masterwork Items → Premium Market Value (base price ×2.5-5.0)
```

**Connection to Player Progression:**

```
Character Level → Skill Access → Equipment Tier
────────────────────────────────────────────────
Levels 1-20 → Basic Skills → Crude/Standard Equipment
Levels 21-40 → Intermediate Skills → Standard/Fine Equipment
Levels 41-60 → Advanced Skills → Fine/Superior Equipment
Levels 61-80 → Expert Skills → Superior Equipment
Levels 81-100 → Master Skills → Superior/Masterwork Equipment
```

### Appendix G: Revision History

| Version | Date | Author | Changes |
|---------|------|--------|---------|
| 1.0 | 2025-01-08 | Game Design Research Team | Initial comprehensive research report |

---

## Related Research

**Material and Quality Systems:**
- [Mortal Online 2 Material System Research](mortal-online-2-material-system-research.md) - Detailed material quality mechanics and open-ended crafting analysis
- [Crafting Quality Model](../../docs/gameplay/mechanics/crafting-quality-model.md) - Mathematical specifications for quality calculations
- [Crafting Interface Mockups](assets/crafting-interface-mockups.md) - Visual design for crafting interfaces

**Skill Progression Systems:**
- [Skill and Knowledge System Research](skill-knowledge-system-research.md) - Comparative analysis of MMORPG skill systems
- [Skill Caps and Decay Research](skill-caps-and-decay-research.md) - Skill cap mechanics and decay systems

**Game Design Foundation:**
- [Mechanics Research](mechanics-research.md) - Economic systems and production chains
- [Player Freedom Analysis](player-freedom-analysis.md) - Framework for player agency
- [Implementation Plan](implementation-plan.md) - Development roadmap

---

*This research report provides a complete foundation for implementing realistic assembly skills in BlueMarble. 
The proposed system balances realism with engaging gameplay, integrates seamlessly with BlueMarble's geological 
simulation, and provides clear progression paths for players. Implementation should proceed in phases, with 
continuous playtesting and balance adjustments based on player feedback.*
