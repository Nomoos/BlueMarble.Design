# Realistic Basic Skills Candidates - Research Report

**Document Type:** Research Report  
**Version:** 1.0  
**Author:** Game Design Research Team  
**Date:** 2025-10-01  
**Status:** Final  
**Research Type:** Game Mechanics Research  
**Related Documents:** 
- [Assembly Skills System Research](assembly-skills-system-research.md)
- [Crafting Interface Mockups](assets/crafting-interface-mockups.md)
- [Skill Caps and Decay Research](skill-caps-and-decay-research.md)

## Executive Summary

This research explores and documents realistic basic skills for BlueMarble, focusing on authenticity and 
practical use in gameplay. The analysis covers traditional skill domains including tailoring, blacksmithing, 
alchemy, woodworking, cooking, herbalism, and more. Each skill is assessed for realism, progression systems, 
dependencies, and in-game effects. The document provides actionable recommendations with visual references 
for implementation, building on BlueMarble's geological simulation foundation to create an immersive and 
authentic crafting experience.

**Key Findings:**
- Basic skills should mirror real-world learning curves with apprentice → journeyman → master progression
- Material quality from geological/botanical sources directly impacts crafting outcomes
- Skill progression requires practice-based advancement with diminishing returns
- Dependencies between skills create natural specialization paths
- Visual feedback (UI mockups) critical for player understanding and engagement
- Fiber-based crafting (textiles) provides excellent entry point for new players
- Each skill domain requires 3-5 distinct progression tiers with unlockable techniques

## Table of Contents

1. [Research Objectives](#research-objectives)
2. [Methodology](#methodology)
3. [Core Basic Skills Analysis](#core-basic-skills-analysis)
4. [Skill Progression Framework](#skill-progression-framework)
5. [Skill Dependencies and Synergies](#skill-dependencies-and-synergies)
6. [Implementation Recommendations](#implementation-recommendations)
7. [Visual References and UI Examples](#visual-references-and-ui-examples)
8. [Appendices](#appendices)

## Research Objectives

### Primary Questions
1. What basic skills provide the most authentic and engaging gameplay experience?
2. How should skill progression balance realism with player enjoyment?
3. What dependencies between skills create meaningful choices?
4. How can UI/UX communicate complex skill interactions clearly?

### Success Criteria
- Comprehensive documentation of 8-12 realistic basic skills
- Clear progression mechanics for each skill (levels 1-100)
- Actionable implementation recommendations
- Visual references for player-facing interfaces
- Integration pathways with BlueMarble's geological systems

## Methodology

### Research Approach
Mixed methods combining:
- Historical analysis of traditional crafts and apprenticeship systems
- Game design analysis of successful skill-based MMORPGs
- Mathematical modeling of progression curves and success rates
- UI/UX mockup creation for player feedback visualization

### Data Sources
- Historical craftsmanship literature (medieval guilds, traditional trades)
- Game systems from Wurm Online, Eco, Vintage Story, Life is Feudal
- BlueMarble design documents (assembly-skills-system-research.md)
- Real-world skill acquisition research (deliberate practice, mastery curves)

## Core Basic Skills Analysis

### 1. Tailoring - Textile and Fiber Work

**Real-World Foundation:**
Tailoring is one of humanity's oldest crafts, involving the design, cutting, and assembly of fabric 
and leather into clothing, bags, and light armor. Historical tailors progressed from simple repairs 
to master pattern-making over 7-10 years of apprenticeship.

**Skill Progression Tiers:**

#### Tier 1: Novice Textile Worker (Levels 1-20)
- **Unlocked Techniques:**
  - Basic thread spinning from plant fibers
  - Simple cloth weaving (linen, cotton)
  - Crude clothing assembly (tunics, pants, hoods)
  - Material identification (flax, hemp, wool)
  
- **Material Sources:**
  - Wild plant fibers (flax, hemp, nettles)
  - Processed thread from fiber stations
  - Basic dyes from common plants
  
- **Success Rates:** 60-75%
- **Quality Range:** Poor to Standard (20-60%)
- **XP Gains:** Fast (50-100 XP per craft)

#### Tier 2: Journeyman Tailor (Levels 21-40)
- **Unlocked Techniques:**
  - Pattern design and modification
  - Multi-piece garment assembly
  - Leather working (basic armor, bags)
  - Decorative stitching and embroidery
  
- **Material Sources:**
  - Cultivated high-quality fibers
  - Tanned leather from hunting
  - Specialized dyes (mineral-based, rare plants)
  - Reinforced thread (waxed, silk)
  
- **Success Rates:** 75-85%
- **Quality Range:** Standard to Fine (50-75%)
- **XP Gains:** Moderate (25-75 XP per craft)

#### Tier 3: Expert Tailor (Levels 41-70)
- **Unlocked Techniques:**
  - Custom fitted garments (armor, formal wear)
  - Advanced leather hardening
  - Light armor with protective inserts
  - Waterproofing and weatherproofing
  
- **Material Sources:**
  - Exotic fibers (silk, high-grade wool)
  - Treated leather (hardened, oiled)
  - Rare dyes (quality affects prestige)
  - Metal studs and reinforcements
  
- **Success Rates:** 85-92%
- **Quality Range:** Fine to Superior (65-85%)
- **XP Gains:** Slow (15-50 XP per craft)

#### Tier 4: Master Clothier (Levels 71-100)
- **Unlocked Techniques:**
  - Masterwork fashion items (prestige clothing)
  - Enchantment-ready garments (magical integration)
  - Lightweight protective gear (rogue armor)
  - Guild-level quality standards
  
- **Material Sources:**
  - Legendary fibers (spider silk, rare imports)
  - Masterwork leather (ancient techniques)
  - Alchemical treatments
  - Precious metal threading (gold, silver)
  
- **Success Rates:** 92-98%
- **Quality Range:** Superior to Masterwork (80-99%)
- **XP Gains:** Very Slow (5-25 XP per craft)

**In-Game Effects:**
- **Clothing Quality:** Affects warmth, durability, and social interactions
- **Armor Value:** Light armor from leather provides protection without weight
- **Bag Capacity:** Better tailoring = more efficient storage items
- **Market Value:** High-quality garments command premium prices
- **Prestige:** Wearing/crafting masterwork items increases reputation

**Skill Dependencies:**
- **Herbalism:** For dye sources and fiber identification
- **Leatherworking:** Complementary for armor and bags
- **Alchemy:** For fabric treatments and dye processing

**UI Reference - Fiber Crafting Example:**
See [Crafting Interface Mockups](assets/crafting-interface-mockups.md) - "Crafting Fiber Clothing" section 
for detailed UI showing material selection, quality impact, and success rate visualization.

---

### 2. Blacksmithing - Metalworking and Forging

**Real-World Foundation:**
Blacksmithing combines metallurgy, physics, and artistry to shape metal through controlled heating 
and hammering. Historical blacksmiths trained 5-7 years as apprentices before becoming journeymen, 
with master status requiring another 5-10 years.

**Skill Progression Tiers:**

#### Tier 1: Apprentice Smith (Levels 1-20)
- **Unlocked Techniques:**
  - Basic ore smelting (iron, copper)
  - Simple tool forging (nails, hinges, basic tools)
  - Bloom refinement
  - Fire management
  
- **Material Sources:**
  - Common ore deposits (iron, copper, tin)
  - Charcoal for fuel
  - Clay for simple molds
  
- **Success Rates:** 55-70%
- **Quality Range:** Poor to Standard (25-55%)
- **XP Gains:** Fast (75-125 XP per craft)
- **Special:** High failure rate teaches importance of temperature control

#### Tier 2: Journeyman Blacksmith (Levels 21-40)
- **Unlocked Techniques:**
  - Steel production and carburization
  - Weapon forging (swords, axes, spearheads)
  - Basic armor plates
  - Quenching and tempering
  
- **Material Sources:**
  - High-quality ore deposits
  - Steel ingots (pre-processed)
  - Specialized fuels (coke, high-grade charcoal)
  - Quenching oils and water
  
- **Success Rates:** 70-82%
- **Quality Range:** Standard to Fine (50-70%)
- **XP Gains:** Moderate (40-90 XP per craft)

#### Tier 3: Expert Weaponsmith (Levels 41-70)
- **Unlocked Techniques:**
  - Advanced alloy creation (steel variants)
  - Precision weapon crafting (balanced blades)
  - Chainmail and scale armor
  - Edge geometry optimization
  
- **Material Sources:**
  - Rare ore deposits (high purity)
  - Exotic metals (nickel, chromium)
  - Precision tools and anvils
  - Alchemical quenchants
  
- **Success Rates:** 82-90%
- **Quality Range:** Fine to Superior (68-88%)
- **XP Gains:** Slow (25-65 XP per craft)

#### Tier 4: Master Bladesmith (Levels 71-100)
- **Unlocked Techniques:**
  - Pattern welding (Damascus steel)
  - Masterwork weapons with perfect balance
  - Enchantment-ready metalwork
  - Custom alloy formulation
  
- **Material Sources:**
  - Legendary ore deposits
  - Meteorite iron (rare)
  - Master-grade tools
  - Magical flux agents
  
- **Success Rates:** 90-97%
- **Quality Range:** Superior to Legendary (85-99%)
- **XP Gains:** Very Slow (10-40 XP per craft)

**In-Game Effects:**
- **Weapon Damage:** Quality directly affects base damage and critical chance
- **Durability:** Better smithing = longer-lasting equipment
- **Tool Efficiency:** High-quality tools work faster and last longer
- **Repair Capability:** Can repair items up to your skill level
- **Market Dominance:** Master smiths control armor/weapon economy

**Skill Dependencies:**
- **Mining:** For ore extraction and quality preservation
- **Alchemy:** For quenching formulas and alloy additives
- **Engineering:** For advanced tool and mechanism creation

**Specialization Paths:**
- **Weaponsmith:** Focus on combat effectiveness and balance
- **Armorsmith:** Expertise in protective equipment and weight distribution
- **Toolmaker:** Specialization in precision tools for other crafts

---

### 3. Alchemy - Potion Brewing and Chemical Processing

**Real-World Foundation:**
Alchemy combines historical mysticism with practical chemistry, herbalism, and mineral processing. 
Medieval alchemists studied for decades, learning to extract essences, create compounds, and 
transform materials through controlled reactions.

**Skill Progression Tiers:**

#### Tier 1: Hedge Alchemist (Levels 1-20)
- **Unlocked Techniques:**
  - Basic herb preparation (drying, grinding)
  - Simple infusions and teas
  - Healing salves and poultices
  - Ingredient identification
  
- **Material Sources:**
  - Common herbs (healing plants, basic reagents)
  - Clean water sources
  - Simple containers (clay pots, glass vials)
  
- **Success Rates:** 65-75%
- **Quality Range:** Weak to Standard (30-60%)
- **Effect Potency:** 20-50% of maximum
- **XP Gains:** Fast (60-110 XP per craft)

#### Tier 2: Apprentice Alchemist (Levels 21-40)
- **Unlocked Techniques:**
  - Basic distillation
  - Multi-ingredient potions
  - Buff potions (stamina, strength)
  - Timed reactions and heating control
  
- **Material Sources:**
  - Cultivated herbs (higher quality)
  - Mineral reagents (salt, sulfur, mercury)
  - Glass apparatus (alembics, retorts)
  - Pure alcohol (solvent)
  
- **Success Rates:** 75-84%
- **Quality Range:** Standard to Fine (55-75%)
- **Effect Potency:** 50-70% of maximum
- **XP Gains:** Moderate (35-80 XP per craft)

#### Tier 3: Journeyman Alchemist (Levels 41-70)
- **Unlocked Techniques:**
  - Advanced distillation (fractional)
  - Antidotes and cures
  - Magical reagent extraction
  - Crystallization and purification
  
- **Material Sources:**
  - Rare herbs (specific biomes)
  - Exotic minerals (gemstone dust, rare earths)
  - Creature components (organs, blood, venom)
  - Alchemical catalysts
  
- **Success Rates:** 84-91%
- **Quality Range:** Fine to Superior (70-88%)
- **Effect Potency:** 70-90% of maximum
- **XP Gains:** Slow (20-60 XP per craft)

#### Tier 4: Master Alchemist (Levels 71-100)
- **Unlocked Techniques:**
  - Philosopher's stone research
  - Transmutation attempts (lead to gold)
  - Elixirs of life (powerful buffs)
  - Experimental formulations
  
- **Material Sources:**
  - Legendary herbs (mythical plants)
  - Crystalline matrices (pure mineral essences)
  - Dragon blood and rare creature parts
  - Celestial reagents (meteorite dust)
  
- **Success Rates:** 91-96%
- **Quality Range:** Superior to Legendary (88-99%)
- **Effect Potency:** 90-100% of maximum
- **XP Gains:** Very Slow (8-35 XP per craft)

**In-Game Effects:**
- **Health/Mana Restoration:** Potions restore resources based on quality
- **Buff Durations:** Better alchemy = longer-lasting effects
- **Potion Toxicity:** Failed brewing can create harmful substances
- **Material Transformation:** High-level alchemists can convert materials
- **Economic Power:** Control over consumable market

**Skill Dependencies:**
- **Herbalism:** For ingredient gathering and identification
- **Mining:** For mineral reagents
- **Cooking:** Complementary heat control and ingredient mixing

---

### 4. Woodworking - Carpentry and Joinery

**Real-World Foundation:**
Woodworking encompasses carpentry, joinery, and fine woodcraft. Traditional apprenticeships 
lasted 4-6 years, with specialization in furniture, structural work, or precision instruments 
requiring additional years of mastery.

**Skill Progression Tiers:**

#### Tier 1: Novice Carpenter (Levels 1-20)
- **Unlocked Techniques:**
  - Basic lumber processing (logs to planks)
  - Simple furniture (tables, chairs, chests)
  - Straight cuts and basic joinery
  - Wood species identification
  
- **Material Sources:**
  - Common trees (pine, oak, birch)
  - Basic hand tools (saw, chisel, plane)
  - Simple fasteners (nails, pegs)
  
- **Success Rates:** 65-78%
- **Quality Range:** Crude to Standard (35-60%)
- **XP Gains:** Fast (55-105 XP per craft)

#### Tier 2: Journeyman Carpenter (Levels 21-40)
- **Unlocked Techniques:**
  - Advanced joinery (dovetails, mortise-tenon)
  - Weapon components (bow staves, hafts, shields)
  - Precision cutting and shaping
  - Wood seasoning and treatment
  
- **Material Sources:**
  - Hardwoods (maple, walnut, ash)
  - Quality tools (sharp saws, precision chisels)
  - Wood glues and finishes
  - Metal reinforcements
  
- **Success Rates:** 78-86%
- **Quality Range:** Standard to Fine (58-76%)
- **XP Gains:** Moderate (30-75 XP per craft)

#### Tier 3: Expert Woodworker (Levels 41-70)
- **Unlocked Techniques:**
  - Complex furniture (wardrobes, beds)
  - Bow crafting (longbows, crossbows)
  - Decorative carving and inlay
  - Lamination and compound construction
  
- **Material Sources:**
  - Exotic hardwoods (ebony, mahogany, teak)
  - Master-grade tools
  - Finishing oils and varnishes
  - Precious wood inlays
  
- **Success Rates:** 86-92%
- **Quality Range:** Fine to Superior (72-87%)
- **XP Gains:** Slow (18-55 XP per craft)

#### Tier 4: Master Craftsman (Levels 71-100)
- **Unlocked Techniques:**
  - Artistic masterpieces (thrones, musical instruments)
  - Ship components (masts, hulls)
  - Magical wood integration (living wood)
  - Structural engineering (buildings, siege weapons)
  
- **Material Sources:**
  - Ancient wood (centuries-old trees)
  - Legendary tools (heirloom quality)
  - Alchemical wood treatments
  - Magical resins and finishes
  
- **Success Rates:** 92-98%
- **Quality Range:** Superior to Masterwork (85-99%)
- **XP Gains:** Very Slow (10-40 XP per craft)

**In-Game Effects:**
- **Furniture Bonuses:** Quality affects rest/storage capacity
- **Weapon Performance:** Bow damage/accuracy depends on woodworking
- **Structural Integrity:** Building quality affects durability
- **Aesthetic Value:** Fine furniture increases prestige
- **Tool Creation:** Wood handles for metal tools

**Skill Dependencies:**
- **Forestry/Logging:** For wood harvesting and quality
- **Blacksmithing:** For metal fasteners and reinforcements
- **Engineering:** For complex mechanisms and structures

**Specialization Paths:**
- **Fletcher:** Bows, arrows, and ranged weapons
- **Furniture Maker:** Decorative and functional furniture
- **Shipwright:** Large-scale construction and vehicles

---

### 5. Cooking - Culinary Arts and Food Preparation

**Real-World Foundation:**
Cooking is essential for survival and enjoyment. Professional cooks historically trained 
3-5 years, learning ingredient properties, heat control, and recipe formulation.

**Skill Progression Tiers:**

#### Tier 1: Camp Cook (Levels 1-20)
- **Unlocked Techniques:**
  - Basic fire cooking (roasting, boiling)
  - Simple recipes (stews, breads, roasted meat)
  - Food preservation basics (salting, drying)
  - Ingredient identification
  
- **Material Sources:**
  - Common ingredients (meat, vegetables, grain)
  - Basic cookware (pot, pan, spit)
  - Simple seasonings (salt, pepper)
  
- **Success Rates:** 70-80%
- **Food Quality:** Basic to Standard (40-65%)
- **Buff Effects:** Small hunger reduction, minor buffs
- **XP Gains:** Fast (45-95 XP per craft)

#### Tier 2: Skilled Cook (Levels 21-40)
- **Unlocked Techniques:**
  - Multi-ingredient recipes
  - Advanced preservation (smoking, pickling)
  - Baking and pastries
  - Heat control and timing
  
- **Material Sources:**
  - Quality ingredients (choice cuts, fresh produce)
  - Better cookware (ovens, specialized pans)
  - Herbs and spices
  - Dairy products
  
- **Success Rates:** 80-88%
- **Food Quality:** Standard to Fine (60-78%)
- **Buff Effects:** Moderate hunger reduction, useful buffs (2-4 hours)
- **XP Gains:** Moderate (25-70 XP per craft)

#### Tier 3: Master Chef (Levels 41-70)
- **Unlocked Techniques:**
  - Complex recipes (multi-course meals)
  - Exotic cuisine
  - Buff-focused cooking
  - Recipe creation and modification
  
- **Material Sources:**
  - Rare ingredients (exotic meats, rare herbs)
  - Professional equipment
  - Expensive seasonings and sauces
  - Wine and spirits
  
- **Success Rates:** 88-94%
- **Food Quality:** Fine to Superior (75-90%)
- **Buff Effects:** Strong buffs (4-8 hours), stacking effects
- **XP Gains:** Slow (15-50 XP per craft)

#### Tier 4: Legendary Chef (Levels 71-100)
- **Unlocked Techniques:**
  - Feast preparation (group buffs)
  - Alchemical cooking (magical effects)
  - Signature dishes (unique recipes)
  - Experimental cuisine
  
- **Material Sources:**
  - Legendary ingredients (dragon meat, golden apples)
  - Masterwork equipment
  - Alchemical additives
  - Ancient recipes and techniques
  
- **Success Rates:** 94-99%
- **Food Quality:** Superior to Legendary (88-99%)
- **Buff Effects:** Powerful buffs (8-24 hours), multiple stacking effects
- **XP Gains:** Very Slow (8-30 XP per craft)

**In-Game Effects:**
- **Hunger Management:** Better food = longer satiation
- **Stat Buffs:** Cooked food provides temporary stat increases
- **Health Regeneration:** Quality meals increase regen rates
- **Morale/Happiness:** Good food improves character mood
- **Group Bonuses:** Feasts provide party-wide buffs

**Skill Dependencies:**
- **Farming:** For ingredient cultivation
- **Hunting/Fishing:** For protein sources
- **Alchemy:** For buff enhancement and preservation

---

### 6. Herbalism - Plant Gathering and Identification

**Real-World Foundation:**
Herbalism is the ancient practice of identifying, gathering, and using plants for medicine, 
cooking, and crafting. Traditional herbalists studied plant properties for years under mentors.

**Skill Progression Tiers:**

#### Tier 1: Plant Gatherer (Levels 1-20)
- **Unlocked Techniques:**
  - Common plant identification
  - Basic gathering (flowers, leaves, roots)
  - Optimal harvest timing (day/night, seasons)
  - Plant quality assessment
  
- **Material Sources:**
  - Common herbs (meadow plants, forest herbs)
  - Basic gathering tools (knife, basket)
  - Accessible locations
  
- **Success Rates:** 75-85%
- **Quality Preservation:** 60-75% of plant quality retained
- **Yield:** 1-2 units per plant
- **XP Gains:** Fast (40-90 XP per gather)

#### Tier 2: Skilled Herbalist (Levels 21-40)
- **Unlocked Techniques:**
  - Rare plant identification
  - Sustainable harvesting (replanting)
  - Plant propagation basics
  - Drying and storage methods
  
- **Material Sources:**
  - Uncommon herbs (specific biomes)
  - Quality gathering tools
  - Cultivation knowledge
  
- **Success Rates:** 85-90%
- **Quality Preservation:** 75-85% of plant quality retained
- **Yield:** 2-3 units per plant
- **XP Gains:** Moderate (25-65 XP per gather)

#### Tier 3: Master Herbalist (Levels 41-70)
- **Unlocked Techniques:**
  - Exotic plant identification
  - Magical plant detection
  - Cross-breeding and cultivation
  - Essence extraction
  
- **Material Sources:**
  - Rare herbs (remote locations)
  - Specialized tools
  - Cultivation facilities (gardens)
  
- **Success Rates:** 90-95%
- **Quality Preservation:** 85-95% of plant quality retained
- **Yield:** 3-4 units per plant
- **XP Gains:** Slow (15-45 XP per gather)

#### Tier 4: Legendary Herbalist (Levels 71-100)
- **Unlocked Techniques:**
  - Mythical plant identification
  - Living plant communion
  - Genetic modification (magical)
  - Complete quality preservation
  
- **Material Sources:**
  - Legendary herbs (unique spawns)
  - Enchanted gathering tools
  - Secret garden locations
  
- **Success Rates:** 95-99%
- **Quality Preservation:** 95-100% of plant quality retained
- **Yield:** 4-5 units per plant
- **XP Gains:** Very Slow (8-25 XP per gather)

**In-Game Effects:**
- **Alchemy Materials:** Primary ingredient source
- **Cooking Enhancement:** Herbs improve recipes
- **Dye Production:** Certain plants provide dyes
- **Medicine Creation:** Direct healing item crafting
- **Location Discovery:** Finding rare plants reveals map areas

**Skill Dependencies:**
- **Alchemy:** Main consumer of gathered herbs
- **Cooking:** For culinary herbs and spices
- **Tailoring:** For dye extraction

---

### 7. Mining - Ore and Mineral Extraction

**Real-World Foundation:**
Mining requires geological knowledge, physical endurance, and technical skill to extract 
valuable materials from rock formations. Traditional miners learned on-site over years 
of dangerous work.

**Skill Progression Tiers:**

#### Tier 1: Prospector (Levels 1-20)
- **Unlocked Techniques:**
  - Basic ore identification (iron, copper, tin)
  - Surface mining
  - Rock breaking fundamentals
  - Vein following
  
- **Material Sources:**
  - Surface deposits
  - Basic pickaxes and hammers
  - Common ore veins
  
- **Success Rates:** 70-80%
- **Quality Preservation:** 50-65% of ore quality retained
- **Extraction Speed:** Slow (30-45 seconds per unit)
- **XP Gains:** Fast (60-100 XP per extraction)

#### Tier 2: Miner (Levels 21-40)
- **Unlocked Techniques:**
  - Rare ore identification (silver, gold)
  - Tunnel mining and shoring
  - Efficient extraction (less waste)
  - Gem recognition
  
- **Material Sources:**
  - Underground deposits
  - Quality mining tools
  - Deeper ore veins
  
- **Success Rates:** 80-87%
- **Quality Preservation:** 65-80% of ore quality retained
- **Extraction Speed:** Moderate (20-30 seconds per unit)
- **XP Gains:** Moderate (35-75 XP per extraction)

#### Tier 3: Expert Miner (Levels 41-70)
- **Unlocked Techniques:**
  - Precious gem extraction
  - Geological formation analysis
  - Advanced tunneling (minimal support)
  - Quality ore sensing
  
- **Material Sources:**
  - Deep deposits
  - Master-grade tools
  - Rich ore veins
  
- **Success Rates:** 87-93%
- **Quality Preservation:** 80-90% of ore quality retained
- **Extraction Speed:** Fast (15-20 seconds per unit)
- **XP Gains:** Slow (20-55 XP per extraction)

#### Tier 4: Master Prospector (Levels 71-100)
- **Unlocked Techniques:**
  - Legendary ore detection (meteorite, mythical metals)
  - Perfect extraction (no waste)
  - Underground mapping
  - Vein prediction
  
- **Material Sources:**
  - Rare geological formations
  - Enchanted mining tools
  - Unique ore deposits
  
- **Success Rates:** 93-98%
- **Quality Preservation:** 90-100% of ore quality retained
- **Extraction Speed:** Very Fast (10-15 seconds per unit)
- **XP Gains:** Very Slow (10-35 XP per extraction)

**In-Game Effects:**
- **Resource Supply:** Primary metal/mineral source
- **Economic Power:** Control over raw material market
- **Geological Discovery:** Finding rare deposits
- **Tool Degradation:** Mining wears tools quickly
- **Building Materials:** Stone and gems for construction

**Skill Dependencies:**
- **Blacksmithing:** Main consumer of ores
- **Alchemy:** For mineral reagents
- **Engineering:** For mining equipment and supports

---

### 8. Fishing - Aquatic Resource Harvesting

**Real-World Foundation:**
Fishing combines patience, knowledge of water ecosystems, and technique mastery. Traditional 
fishing communities passed down knowledge over generations.

**Skill Progression Tiers:**

#### Tier 1: Novice Angler (Levels 1-20)
- **Unlocked Techniques:**
  - Basic rod fishing
  - Common fish identification
  - Bait preparation
  - Casting basics
  
- **Material Sources:**
  - Rivers and ponds
  - Simple rod and line
  - Common bait (worms, bread)
  
- **Success Rates:** 60-75%
- **Catch Quality:** Small to medium fish, low quality
- **Catch Speed:** Slow (2-4 minutes per catch)
- **XP Gains:** Fast (35-85 XP per catch)

#### Tier 2: Skilled Fisher (Levels 21-40)
- **Unlocked Techniques:**
  - Specialized techniques (fly fishing, net fishing)
  - Rare fish identification
  - Water reading (depth, temperature, currents)
  - Efficient hook setting
  
- **Material Sources:**
  - Lakes and coastal waters
  - Quality fishing gear
  - Specialized lures and bait
  
- **Success Rates:** 75-85%
- **Catch Quality:** Medium to large fish, standard quality
- **Catch Speed:** Moderate (1-2 minutes per catch)
- **XP Gains:** Moderate (25-65 XP per catch)

#### Tier 3: Master Angler (Levels 41-70)
- **Unlocked Techniques:**
  - Deep sea fishing
  - Trophy fish targeting
  - Seasonal migration patterns
  - Minimal escape chance
  
- **Material Sources:**
  - Open ocean
  - Professional equipment
  - Rare lures
  
- **Success Rates:** 85-92%
- **Catch Quality:** Large to trophy fish, fine quality
- **Catch Speed:** Fast (30-90 seconds per catch)
- **XP Gains:** Slow (15-45 XP per catch)

#### Tier 4: Legendary Fisher (Levels 71-100)
- **Unlocked Techniques:**
  - Mythical creature fishing (sea monsters)
  - Perfect fish preservation
  - Aquatic resource location
  - Guaranteed trophy catches
  
- **Material Sources:**
  - Legendary fishing spots
  - Enchanted fishing gear
  - Magical bait
  
- **Success Rates:** 92-98%
- **Catch Quality:** Trophy to legendary fish
- **Catch Speed:** Very Fast (20-60 seconds per catch)
- **XP Gains:** Very Slow (8-30 XP per catch)

**In-Game Effects:**
- **Food Source:** Primary protein for cooking
- **Alchemy Ingredients:** Fish parts for potions
- **Economic Opportunity:** Trophy fish are valuable
- **Relaxation Mechanic:** Fishing reduces stress/fatigue
- **Exploration:** Finding fishing spots reveals locations

**Skill Dependencies:**
- **Cooking:** Main consumer of fish
- **Alchemy:** For fish-based ingredients
- **Woodworking:** For rod and boat crafting

---

## Skill Progression Framework

### Universal Progression Mechanics

All basic skills in BlueMarble follow a consistent progression framework:

**Experience Formula:**
```
XP_Gained = Base_XP × Difficulty_Multiplier × Material_Quality × Skill_Gap_Modifier
```

**Skill Gap Modifier:**
- Crafting below skill level: 0.1x XP (minimal learning)
- Crafting at skill level: 1.0x XP (optimal learning)
- Crafting above skill level: 1.5-2.0x XP (accelerated learning, higher failure risk)

**Level Progression Curve:**
```
XP_Required(level) = 100 × (level^1.8)

Examples:
Level 1 → 2:     100 XP
Level 10 → 11:   1,995 XP
Level 20 → 21:   7,241 XP
Level 50 → 51:   69,644 XP
Level 100:       Total ~2.5 million XP
```

**Success Rate Formula:**
```
Success_Rate = Base_Rate + (Skill_Level - Recipe_Level) × 2% + Material_Bonus + Tool_Bonus

Clamped between 10% (minimum) and 98% (maximum)
```

**Quality Outcome Formula:**
```
Item_Quality = (Skill_Level × 0.4 + Material_Quality × 0.35 + Tool_Quality × 0.15 + Random(0-10)) × Success_Modifier

Success_Modifier:
- Critical Success (5%): 1.3x
- Normal Success: 1.0x
- Partial Success: 0.7x
- Critical Failure (5%): Item destroyed
```

### Diminishing Returns

To encourage specialization and prevent power leveling:

**Skill Tiers and Time Investment:**
- **Novice (1-20):** 10-20 hours of focused practice
- **Journeyman (21-40):** 30-50 hours additional
- **Expert (41-70):** 80-120 hours additional
- **Master (71-100):** 150-250 hours additional

**Total to Master:** 270-440 hours per skill (realistic specialization)

### Cross-Skill Synergies

Skills provide minor bonuses to related skills:

```
Related_Skill_Bonus = (Related_Skill_Level × 0.1) capped at +10 levels

Examples:
- Herbalism Level 50 provides +5 effective levels to Alchemy
- Blacksmithing Level 40 provides +4 effective levels to Mining
- Cooking Level 30 provides +3 effective levels to Alchemy
```

---

## Skill Dependencies and Synergies

### Dependency Network

```
                    GATHERING SKILLS
                    ================
                    Mining ←→ Blacksmithing
                    Herbalism ←→ Alchemy
                    Forestry ←→ Woodworking
                    Hunting/Fishing ←→ Cooking
                    
                    PROCESSING SKILLS
                    =================
                    Blacksmithing → Weaponsmithing
                    Tailoring → Leatherworking
                    Alchemy → Medicine/Enchanting
                    Woodworking → Carpentry/Bowyer
                    
                    CRAFTING SKILLS
                    ===============
                    All Processing → Engineering
                    Cooking + Alchemy → Gastronomy
                    All Crafts → Masterwork Creation
```

### Natural Progression Paths

**Path 1: The Armorer**
1. Mining (1-30) - Gather raw materials
2. Blacksmithing (1-50) - Learn metalworking
3. Specialize: Armorsmith (50+) - Master armor creation

**Path 2: The Alchemist**
1. Herbalism (1-40) - Gather ingredients
2. Alchemy (1-70) - Learn brewing
3. Cooking (20-40) - Enhance understanding
4. Advanced Alchemy (70+) - Experimental formulations

**Path 3: The Woodsman**
1. Forestry (1-30) - Understand wood
2. Woodworking (1-50) - Basic carpentry
3. Specialize: Fletcher (50+) OR Furniture Maker (50+)

**Path 4: The Clothier**
1. Herbalism (1-20) - Gather fibers and dyes
2. Tailoring (1-70) - Master textiles
3. Alchemy (20-30) - Fabric treatments
4. Master Tailoring (70+) - Prestige garments

---

## Implementation Recommendations

### Phase 1: Core Skills (Months 1-3)

**Priority 1 - Essential Skills:**
1. **Tailoring** - Entry-level crafting, low barrier to entry
2. **Cooking** - Survival necessity, frequent use
3. **Herbalism** - Resource gathering, supports other skills
4. **Mining** - Core resource extraction

**Implementation:**
- Practice-based XP system
- 4-tier progression (20 levels per tier)
- Material quality integration with geological simulation
- Basic crafting UI (see Visual References section)

**Deliverables:**
- Skill progression database
- Success rate calculation system
- Material quality algorithms
- Basic crafting interface

### Phase 2: Advanced Crafting (Months 4-6)

**Priority 2 - Crafting Skills:**
1. **Blacksmithing** - Weapon and armor creation
2. **Woodworking** - Tools and structures
3. **Alchemy** - Consumables and buffs

**Implementation:**
- Advanced material dependencies
- Specialization system
- Recipe unlocking progression
- Enhanced UI with quality visualization

**Deliverables:**
- Specialization path selection
- Recipe database
- Advanced success calculations
- Quality outcome system

### Phase 3: Specializations (Months 7-9)

**Priority 3 - Mastery Systems:**
1. **Specialization Paths** - Weaponsmith, Armorsmith, Fletcher, etc.
2. **Cross-Skill Synergies** - Related skill bonuses
3. **Master Techniques** - Legendary recipes
4. **Economic Integration** - Player-driven market

**Implementation:**
- Mastery unlocks at level 50+
- Unique recipes per specialization
- Prestige items and reputation system
- Guild/workshop features

### Phase 4: Advanced Systems (Months 10-12)

**Priority 4 - Endgame Content:**
1. **Legendary Crafting** - Rare and unique items
2. **Teaching System** - Master crafters train others
3. **Competitive Crafting** - Quality competitions
4. **Historical Recognition** - Server-famous crafters

---

## Visual References and UI Examples

### Crafting Interface - Fiber/Textile Work

The existing [Crafting Interface Mockups](assets/crafting-interface-mockups.md) document contains 
an excellent example of fiber-based crafting UI (referenced as "image17" or Fiber/Leaves crafting):

**Key UI Elements:**
- **Material Selection Panel** - Shows available fiber qualities with visual quality bars
- **Success Rate Indicator** - Color-coded (green/yellow/red) with percentage
- **Material Impact Comparison** - Side-by-side quality/cost analysis
- **Skill Level Display** - Shows recommended vs. actual skill level
- **Quality Prediction** - Expected output quality range

**Example from Document:**
```
Premium Flax (88%) ██████████░ +15%
Source: Aged plants, perfect climate
Effect: Higher quality, better durability
```

This UI pattern should be replicated across all crafting skills with appropriate material 
variations (ore quality for blacksmithing, wood quality for carpentry, etc.).

### Skill Progression Visualization

**Progress Bar Design:**
```
Current Level: 35         Experience: 28,720 / 35,355 (81%)
[████████████████████████████████████████████░░░░░░░░]

Next Unlock (Level 36):
🔒 Advanced alloy creation
🔒 Decorative inlay techniques
🔒 Improved success rates (+2%)
```

**Skill Tree View:**
```
TAILORING SPECIALIZATIONS (Unlock at Level 50)

┌──────────────┐      ┌──────────────┐      ┌──────────────┐
│  CLOTHIER    │      │LEATHERWORKER │      │  OUTFITTER   │
├──────────────┤      ├──────────────┤      ├──────────────┤
│ Mage Robes   │      │ Leather Armor│      │ Expedition   │
│ Fashion Items│      │ Bags & Packs │      │ Storage Items│
│ +10% Quality │      │ +15% Durabil.│      │ +20% Capacity│
└──────────────┘      └──────────────┘      └──────────────┘
```

### Material Quality Indicators

**Visual Quality System:**
```
Quality Tiers with Color Coding:

Poor (0-40%)      ████░░░░░░  [Red]     -10% to success
Standard (41-60%) ██████░░░░  [White]   No modifier
Fine (61-75%)     ████████░░  [Green]   +5% to success
Superior (76-90%) █████████░  [Blue]    +10% to success
Masterwork (91%+) ██████████  [Purple]  +15% to success, prestige bonus
```

---

## Appendices

### Appendix A: Complete Basic Skills List

**Gathering Skills (Resource Extraction):**
1. Mining - Ores, gems, stone
2. Herbalism - Plants, herbs, flowers
3. Forestry/Logging - Wood, bark, resin
4. Hunting - Meat, leather, bones
5. Fishing - Fish, aquatic materials

**Processing Skills (Material Refinement):**
6. Blacksmithing - Metal working
7. Tailoring - Textile working
8. Alchemy - Potion brewing
9. Woodworking - Wood crafting
10. Cooking - Food preparation
11. Leatherworking - Hide processing

**Advanced Skills (Specialized Crafts):**
12. Engineering - Mechanisms, devices
13. Enchanting - Magical augmentation
14. Jewelry - Precious metal/gem work
15. Masonry - Stone construction

### Appendix B: Material Quality Mapping

**Geological Simulation → Material Quality:**

```
Ore_Quality = Deposit_Purity × Formation_Age × Extraction_Skill × Tool_Quality

Deposit_Purity: Determined by BlueMarble geological simulation (0.3-1.0)
Formation_Age: Older formations = higher purity (0.5-1.2)
Extraction_Skill: Player skill level / deposit difficulty (0.4-1.3)
Tool_Quality: Tool condition and tier (0.6-1.1)
```

**Botanical Simulation → Plant Quality:**

```
Plant_Quality = Biome_Match × Growth_Time × Harvest_Timing × Gathering_Skill

Biome_Match: How well plant matches ideal climate (0.4-1.0)
Growth_Time: Maturity affects potency (0.5-1.1)
Harvest_Timing: Correct season/time of day (0.7-1.0)
Gathering_Skill: Player skill level (0.5-1.2)
```

### Appendix C: XP Tables

**XP Required per Level (Selected Levels):**

| Level | XP Required | Cumulative XP | Real-Time Hours* |
|-------|-------------|---------------|------------------|
| 1     | 100         | 100           | 0.2              |
| 10    | 1,995       | 12,508        | 5                |
| 20    | 7,241       | 80,745        | 20               |
| 30    | 16,424      | 229,968       | 50               |
| 40    | 29,686      | 485,619       | 90               |
| 50    | 47,044      | 867,622       | 150              |
| 60    | 68,511      | 1,398,054     | 220              |
| 70    | 94,093      | 2,099,012     | 300              |
| 80    | 123,791     | 3,093,600     | 390              |
| 90    | 157,606     | 4,404,931     | 500              |
| 100   | 195,536     | 6,056,326     | 650              |

*Estimated hours assume average crafting activity, not continuous grinding

### Appendix D: Success Rate Calculations

**Formula Breakdown:**

```csharp
public class CraftingSystem
{
    public float CalculateSuccessRate(
        int playerSkill,
        int recipeDifficulty,
        float materialQuality,
        float toolQuality,
        bool hasSpecialization)
    {
        // Base success rate
        float baseRate = 0.50f; // 50% base
        
        // Skill vs difficulty
        float skillDifference = (playerSkill - recipeDifficulty) * 0.02f;
        
        // Material quality bonus
        float materialBonus = (materialQuality - 0.50f) * 0.20f;
        
        // Tool quality bonus
        float toolBonus = (toolQuality - 0.50f) * 0.15f;
        
        // Specialization bonus
        float specBonus = hasSpecialization ? 0.15f : 0.0f;
        
        // Combine and clamp
        float totalRate = baseRate + skillDifference + materialBonus + toolBonus + specBonus;
        return Mathf.Clamp(totalRate, 0.10f, 0.98f);
    }
}
```

### Appendix E: Integration with Existing Systems

**Geological Simulation Integration:**
- Mining ore quality directly from geological data
- Stone/gem deposits in realistic formations
- Mineral composition affects alloy creation
- Ore exhaustion and regeneration rates

**Botanical Simulation Integration:**
- Plant growth based on climate and soil
- Seasonal availability for herbs
- Fiber quality from plant maturity
- Dye plants in specific biomes

**Economic System Integration:**
- Player-crafted items in markets
- Supply/demand based on skill distribution
- Quality tiers affect pricing
- Specialization creates economic niches

---

## Conclusion

This research provides a comprehensive framework for implementing realistic basic skills in BlueMarble. 
The documented skills balance authenticity with engaging gameplay, offering clear progression paths, 
meaningful choices, and deep integration with BlueMarble's geological simulation foundation.

**Key Takeaways:**
1. **Eight Core Skills** provide diverse gameplay opportunities
2. **Four-Tier Progression** (20 levels per tier) creates clear advancement
3. **Material Quality** from geological/botanical systems drives crafting depth
4. **Skill Dependencies** encourage specialization and player cooperation
5. **Visual UI Elements** communicate complex systems clearly to players

**Next Steps:**
1. Implement Phase 1 core skills (Tailoring, Cooking, Herbalism, Mining)
2. Develop material quality algorithms integrated with geological simulation
3. Create crafting UI based on fiber crafting example
4. Test progression curves with player feedback
5. Iterate on balance and timing based on real gameplay data

---

**Document Version History:**
| Version | Date | Changes |
|---------|------|---------|
| 1.0 | 2025-10-01 | Initial comprehensive research report |
