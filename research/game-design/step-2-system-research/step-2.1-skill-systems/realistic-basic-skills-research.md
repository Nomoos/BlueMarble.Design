# Realistic Basic Skills Candidates - Research Report

**Document Type:** Research Report  
**Version:** 2.4  
**Author:** Game Design Research Team  
**Date:** 2025-10-01  
**Status:** Final  
**Research Type:** Game Mechanics Research  
**Related Documents:** 
- [Assembly Skills System Research](assembly-skills-system-research.md)
- [Crafting Interface Mockups](assets/crafting-interface-mockups.md)
- [Skill Caps and Decay Research](skill-caps-and-decay-research.md)
- [Cursor Precision vs. Action Queueing Research](cursor-precision-vs-action-queueing-research.md)

## Executive Summary

This research explores and documents realistic basic skills for BlueMarble, focusing on authenticity and 
practical use in gameplay. The analysis covers traditional skill domains including tailoring, blacksmithing, 
alchemy, woodworking, cooking, herbalism, mining, fishing, combat, farming, forestry, animal husbandry, 
first aid, masonry, and milling. Additionally, it provides frameworks for player-created systems including 
religion/faith and economic/governmental structures. Each skill is assessed for realism, progression systems, 
dependencies, and in-game effects. The document provides actionable recommendations with visual references 
for implementation, building on BlueMarble's geological simulation foundation to create an immersive and 
authentic experience where players shape their own societies.

**Key Findings:**
- Basic skills should mirror real-world learning curves with apprentice → journeyman → master progression
- Material quality from geological/botanical sources directly impacts crafting outcomes
- Skill progression requires practice-based advancement with diminishing returns
- Dependencies between skills create natural specialization paths
- Visual feedback (UI mockups) critical for player understanding and engagement
- Fiber-based crafting (textiles) provides excellent entry point for new players
- Each skill domain features 1024 levels across 4 distinct progression tiers (256 levels each)
- Extended progression allows for fine-grained skill development and long-term mastery goals
- Routine-based progression system where characters operate via routines online and offline (players as programmers)
- Historical professions (masonry, milling) provide authentic medieval gameplay depth
- Player-created religion and governance systems enable emergent social structures

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
- Comprehensive documentation of 15+ realistic basic skills
- Frameworks for player-created religion and governance systems
- Clear progression mechanics for each skill (levels 1-1024)
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

#### Tier 1: Novice Textile Worker (Levels 1-256)
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

#### Tier 2: Journeyman Tailor (Levels 257-512)
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

#### Tier 3: Expert Tailor (Levels 513-768)
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

#### Tier 4: Master Clothier (Levels 769-1024)
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

#### Tier 1: Apprentice Smith (Levels 1-256)
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

#### Tier 2: Journeyman Blacksmith (Levels 257-512)
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

#### Tier 3: Expert Weaponsmith (Levels 513-768)
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

#### Tier 4: Master Bladesmith (Levels 769-1024)
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

#### Tier 1: Hedge Alchemist (Levels 1-256)
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

#### Tier 2: Apprentice Alchemist (Levels 257-512)
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

#### Tier 3: Journeyman Alchemist (Levels 513-768)
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

#### Tier 4: Master Alchemist (Levels 769-1024)
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

#### Tier 1: Novice Carpenter (Levels 1-256)
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

#### Tier 2: Journeyman Carpenter (Levels 257-512)
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

#### Tier 3: Expert Woodworker (Levels 513-768)
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

#### Tier 4: Master Craftsman (Levels 769-1024)
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

#### Tier 1: Camp Cook (Levels 1-256)
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

#### Tier 2: Skilled Cook (Levels 257-512)
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

#### Tier 3: Master Chef (Levels 513-768)
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

#### Tier 4: Legendary Chef (Levels 769-1024)
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

#### Tier 1: Plant Gatherer (Levels 1-256)
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

#### Tier 2: Skilled Herbalist (Levels 257-512)
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

#### Tier 3: Master Herbalist (Levels 513-768)
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

#### Tier 4: Legendary Herbalist (Levels 769-1024)
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

#### Tier 1: Prospector (Levels 1-256)
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

#### Tier 2: Miner (Levels 257-512)
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

#### Tier 3: Expert Miner (Levels 513-768)
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

#### Tier 4: Master Prospector (Levels 769-1024)
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

#### Tier 1: Novice Angler (Levels 1-256)
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

#### Tier 2: Skilled Fisher (Levels 257-512)
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

#### Tier 3: Master Angler (Levels 513-768)
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

#### Tier 4: Legendary Fisher (Levels 769-1024)
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

### 9. Combat - Armed and Unarmed Fighting

**Real-World Foundation:**
Combat skills encompass martial training, weapon proficiency, and tactical awareness. Historical warriors 
trained for years in specific weapon schools, from medieval European swordsmanship to Eastern martial arts. 
Combat effectiveness combines physical conditioning, technique mastery, and combat experience.

**Skill Progression Tiers:**

#### Tier 1: Recruit (Levels 1-256)
- **Unlocked Techniques:**
  - Basic weapon handling (swords, axes, maces)
  - Simple blocking and parrying
  - Basic footwork and positioning
  - Shield use fundamentals
  
- **Combat Capabilities:**
  - Simple attack combos (2-3 moves)
  - Basic defensive stance
  - Weapon maintenance knowledge
  
- **Success Rates:** 50-65% hit chance vs equal level
- **Damage Output:** 70-85% of weapon base damage
- **XP Gains:** Fast (75-125 XP per combat encounter)

#### Tier 2: Soldier (Levels 257-512)
- **Unlocked Techniques:**
  - Weapon specialization options
  - Advanced blocking and counter-attacks
  - Combat awareness (detecting flanking)
  - Dual-wielding basics
  
- **Combat Capabilities:**
  - Extended combos (4-6 moves)
  - Active defense (riposte, deflect)
  - Tactical positioning bonuses
  
- **Success Rates:** 65-80% hit chance vs equal level
- **Damage Output:** 85-100% of weapon base damage
- **XP Gains:** Moderate (40-90 XP per encounter)

#### Tier 3: Veteran Warrior (Levels 513-768)
- **Unlocked Techniques:**
  - Master weapon techniques
  - Advanced combat maneuvers (feints, disarms)
  - Armor weak point targeting
  - Combat meditation (stamina management)
  
- **Combat Capabilities:**
  - Complex combos (7-10 moves)
  - Predictive defense
  - Environmental awareness
  
- **Success Rates:** 80-90% hit chance vs equal level
- **Damage Output:** 100-115% of weapon base damage
- **XP Gains:** Slow (20-60 XP per encounter)

#### Tier 4: Master Combatant (Levels 769-1024)
- **Unlocked Techniques:**
  - Signature fighting styles
  - Perfect timing (critical hit boost)
  - Battle trance (enhanced reflexes)
  - Teaching combat techniques
  
- **Combat Capabilities:**
  - Flawless execution
  - Adaptive fighting style
  - Leadership bonuses (group combat)
  
- **Success Rates:** 90-96% hit chance vs equal level
- **Damage Output:** 115-135% of weapon base damage
- **XP Gains:** Very Slow (10-35 XP per encounter)

**In-Game Effects:**
- **Damage Output:** Higher skill = more consistent and higher damage
- **Defense:** Better blocking, parrying, and dodge chance
- **Weapon Versatility:** Unlock multiple weapon types and styles
- **Stamina Efficiency:** Reduced stamina cost for attacks at higher levels
- **PvP/PvE Balance:** Skill matters more than gear at high levels

**Skill Dependencies:**
- **Blacksmithing:** For weapon maintenance and understanding
- **First Aid:** For self-healing in combat situations
- **Hunting:** Complementary for tracking and positioning

**Specialization Paths:**
- **Swordmaster:** Focus on blade weapons, precision strikes
- **Berserker:** Two-handed weapons, high damage output
- **Guardian:** Shield and defense, group protection

---

### 10. Farming - Agricultural Cultivation

**Real-World Foundation:**
Farming is the backbone of civilization, involving crop cultivation, soil management, and seasonal planning. 
Traditional farmers learned through generations, understanding weather patterns, crop rotation, and optimal 
harvest timing. Success depends on knowledge, patience, and hard work.

**Skill Progression Tiers:**

#### Tier 1: Farmhand (Levels 1-256)
- **Unlocked Techniques:**
  - Basic crop planting (wheat, barley, vegetables)
  - Simple irrigation systems
  - Soil preparation and tilling
  - Weed identification and removal
  
- **Material Sources:**
  - Common seeds (wheat, carrots, potatoes)
  - Basic farming tools (hoe, rake, watering can)
  - Compost and manure
  
- **Success Rates:** 60-75% crop survival rate
- **Yield:** 1-2 units per plant
- **Quality Range:** Poor to Standard (30-60%)
- **XP Gains:** Fast (45-95 XP per harvest)

#### Tier 2: Farmer (Levels 257-512)
- **Unlocked Techniques:**
  - Crop rotation strategies
  - Advanced irrigation (channels, wells)
  - Pest management
  - Multiple crop types (grains, legumes, vegetables)
  
- **Material Sources:**
  - Quality seeds (selective breeding)
  - Improved tools
  - Fertilizers and soil amendments
  
- **Success Rates:** 75-85% crop survival rate
- **Yield:** 2-3 units per plant
- **Quality Range:** Standard to Fine (55-75%)
- **XP Gains:** Moderate (30-70 XP per harvest)

#### Tier 3: Master Farmer (Levels 513-768)
- **Unlocked Techniques:**
  - Greenhouse construction
  - Exotic crop cultivation
  - Selective breeding programs
  - Climate adaptation techniques
  
- **Material Sources:**
  - Rare seeds (imported varieties)
  - Specialized fertilizers
  - Advanced irrigation systems
  
- **Success Rates:** 85-92% crop survival rate
- **Yield:** 3-4 units per plant
- **Quality Range:** Fine to Superior (72-88%)
- **XP Gains:** Slow (18-55 XP per harvest)

#### Tier 4: Agricultural Savant (Levels 769-1024)
- **Unlocked Techniques:**
  - Magical crop enhancement
  - Year-round cultivation
  - Hybrid crop creation
  - Large-scale farm management
  
- **Material Sources:**
  - Legendary seeds (magical properties)
  - Alchemical growth enhancers
  - Automated farming systems
  
- **Success Rates:** 92-98% crop survival rate
- **Yield:** 4-5 units per plant
- **Quality Range:** Superior to Legendary (85-99%)
- **XP Gains:** Very Slow (10-40 XP per harvest)

**In-Game Effects:**
- **Food Supply:** Primary source of cooking ingredients
- **Economic Stability:** Reliable income through crop sales
- **Self-Sufficiency:** Reduced dependence on markets
- **Community Support:** Feed guilds and settlements
- **Land Development:** Transform wilderness into productive farms

**Skill Dependencies:**
- **Herbalism:** For understanding plant properties
- **Alchemy:** For fertilizers and growth enhancers
- **Cooking:** Main consumer of farmed crops
- **Animal Husbandry:** Complementary for mixed farming

---

### 11. Forestry - Tree Harvesting and Woodcutting

**Real-World Foundation:**
Forestry combines physical labor with ecological knowledge. Traditional lumberjacks learned tree identification, 
safe felling techniques, and sustainable harvesting. The skill requires strength, precision, and understanding 
of wood properties for selecting the right trees.

**Skill Progression Tiers:**

#### Tier 1: Woodcutter (Levels 1-256)
- **Unlocked Techniques:**
  - Basic tree felling (small trees)
  - Log processing to lumber
  - Tree species identification
  - Safe cutting practices
  
- **Material Sources:**
  - Common trees (pine, birch, oak)
  - Basic axes and saws
  - Accessible forests
  
- **Success Rates:** 70-80% clean cut
- **Quality Preservation:** 50-65% of tree quality retained
- **Cutting Speed:** Slow (45-60 seconds per tree)
- **XP Gains:** Fast (55-105 XP per tree)

#### Tier 2: Lumberjack (Levels 257-512)
- **Unlocked Techniques:**
  - Large tree felling
  - Directional felling (controlled fall)
  - Branch processing
  - Log transport optimization
  
- **Material Sources:**
  - Hardwoods (maple, ash, walnut)
  - Quality cutting tools
  - Dense forests
  
- **Success Rates:** 80-88% clean cut
- **Quality Preservation:** 65-80% of tree quality retained
- **Cutting Speed:** Moderate (30-40 seconds per tree)
- **XP Gains:** Moderate (30-75 XP per tree)

#### Tier 3: Master Forester (Levels 513-768)
- **Unlocked Techniques:**
  - Rare tree identification and harvesting
  - Sustainable forestry practices
  - Ancient tree felling (very large/old trees)
  - Wood quality assessment
  
- **Material Sources:**
  - Exotic hardwoods (ebony, mahogany, teak)
  - Master-grade axes
  - Remote old-growth forests
  
- **Success Rates:** 88-94% clean cut
- **Quality Preservation:** 80-90% of tree quality retained
- **Cutting Speed:** Fast (20-30 seconds per tree)
- **XP Gains:** Slow (18-55 XP per tree)

#### Tier 4: Legendary Forester (Levels 769-1024)
- **Unlocked Techniques:**
  - Magical tree detection
  - Perfect lumber extraction (zero waste)
  - Living wood harvesting (regrowth)
  - Forest cultivation and management
  
- **Material Sources:**
  - Legendary trees (ancient/magical)
  - Enchanted cutting tools
  - Sacred groves
  
- **Success Rates:** 94-99% clean cut
- **Quality Preservation:** 90-100% of tree quality retained
- **Cutting Speed:** Very Fast (15-20 seconds per tree)
- **XP Gains:** Very Slow (10-35 XP per tree)

**In-Game Effects:**
- **Resource Supply:** Primary wood source for construction and crafting
- **Economic Value:** High-quality lumber commands premium prices
- **Environmental Impact:** Sustainable practices vs. clear-cutting choices
- **Exploration:** Finding rare trees leads to new areas
- **Building Materials:** Essential for structures and ships

**Skill Dependencies:**
- **Woodworking:** Main consumer of harvested lumber
- **Farming:** Understanding of plant growth and ecology
- **Survival:** Navigation and wilderness knowledge

---

### 12. Animal Husbandry - Livestock Care and Breeding

**Real-World Foundation:**
Animal husbandry involves raising, breeding, and caring for domestic animals. Traditional herders and 
farmers learned animal behavior, breeding selection, health care, and optimal feeding. Success requires 
patience, observation skills, and deep understanding of animal needs.

**Skill Progression Tiers:**

#### Tier 1: Stable Hand (Levels 1-256)
- **Unlocked Techniques:**
  - Basic animal care (feeding, watering)
  - Simple health checks
  - Common animal behavior recognition
  - Basic training (horses, cattle)
  
- **Animal Types:**
  - Common livestock (chickens, pigs, cows)
  - Basic working animals (horses, oxen)
  - Simple housing and pens
  
- **Success Rates:** 65-75% animal survival rate
- **Breeding Success:** 40-55% successful births
- **Quality Range:** Standard animals (40-60% quality)
- **XP Gains:** Fast (50-100 XP per animal raised)

#### Tier 2: Herder (Levels 257-512)
- **Unlocked Techniques:**
  - Selective breeding basics
  - Disease identification and treatment
  - Advanced training (multiple commands)
  - Herd management
  
- **Animal Types:**
  - Quality livestock (breeding stock)
  - Specialized animals (war horses, dairy cattle)
  - Improved facilities (barns, stables)
  
- **Success Rates:** 75-85% animal survival rate
- **Breeding Success:** 55-70% successful births
- **Quality Range:** Fine animals (60-78% quality)
- **XP Gains:** Moderate (30-75 XP per animal)

#### Tier 3: Master Breeder (Levels 513-768)
- **Unlocked Techniques:**
  - Advanced breeding programs (traits)
  - Exotic animal handling
  - Animal psychology and bonding
  - Veterinary medicine
  
- **Animal Types:**
  - Rare breeds (special properties)
  - Exotic creatures (llamas, camels)
  - Premium facilities (heated stables)
  
- **Success Rates:** 85-92% animal survival rate
- **Breeding Success:** 70-85% successful births
- **Quality Range:** Superior animals (75-90% quality)
- **XP Gains:** Slow (18-55 XP per animal)

#### Tier 4: Legendary Rancher (Levels 769-1024)
- **Unlocked Techniques:**
  - Magical animal bonding
  - Mythical creature care (griffins, pegasi)
  - Perfect trait breeding
  - Large-scale ranch operations
  
- **Animal Types:**
  - Legendary animals (unique properties)
  - Magical creatures
  - State-of-the-art facilities
  
- **Success Rates:** 92-98% animal survival rate
- **Breeding Success:** 85-95% successful births
- **Quality Range:** Legendary animals (88-99% quality)
- **XP Gains:** Very Slow (10-40 XP per animal)

**In-Game Effects:**
- **Mount Quality:** Better animals = faster travel, combat bonuses
- **Food Production:** Meat, milk, eggs for cooking
- **Material Production:** Leather, wool, feathers for crafting
- **Economic Income:** Selling breeding stock and products
- **Companionship:** Bonded animals as permanent companions

**Skill Dependencies:**
- **Farming:** For animal feed production
- **Cooking:** For processing animal products
- **First Aid:** Transferable healing knowledge
- **Hunting:** Understanding animal behavior

**Specialization Paths:**
- **Horse Trainer:** War mounts, racing horses, premium transport
- **Livestock Farmer:** Food production, dairy, meat
- **Exotic Handler:** Rare and magical creatures

---

### 13. First Aid - Emergency Medical Treatment

**Real-World Foundation:**
First aid encompasses emergency medical care, wound treatment, and stabilization. Historical field medics 
and healers learned through practical experience, understanding anatomy, herbal remedies, and surgical 
techniques. Quick thinking and steady hands save lives.

**Skill Progression Tiers:**

#### Tier 1: Field Medic (Levels 1-256)
- **Unlocked Techniques:**
  - Basic wound cleaning and bandaging
  - Bleeding control
  - Simple splinting for fractures
  - Common illness identification
  
- **Material Sources:**
  - Bandages and cloth strips
  - Clean water
  - Basic herbal poultices
  - Simple medical tools
  
- **Success Rates:** 60-75% treatment success
- **Healing Amount:** 15-30% of max health restored
- **Treatment Speed:** 20-30 seconds
- **XP Gains:** Fast (60-110 XP per treatment)

#### Tier 2: Combat Medic (Levels 257-512)
- **Unlocked Techniques:**
  - Advanced wound suturing
  - Poison/antidote basics
  - Battlefield triage
  - Multiple patient management
  
- **Material Sources:**
  - Quality bandages and thread
  - Antiseptic solutions
  - Basic antidotes
  - Surgical tools
  
- **Success Rates:** 75-85% treatment success
- **Healing Amount:** 30-50% of max health restored
- **Treatment Speed:** 15-25 seconds
- **XP Gains:** Moderate (35-80 XP per treatment)

#### Tier 3: Master Healer (Levels 513-768)
- **Unlocked Techniques:**
  - Complex surgery
  - Disease curing
  - Limb reattachment
  - Magical healing augmentation
  
- **Material Sources:**
  - Premium medical supplies
  - Alchemical healing salves
  - Advanced surgical equipment
  - Rare medicinal herbs
  
- **Success Rates:** 85-92% treatment success
- **Healing Amount:** 50-75% of max health restored
- **Treatment Speed:** 10-20 seconds
- **XP Gains:** Slow (20-60 XP per treatment)

#### Tier 4: Legendary Physician (Levels 769-1024)
- **Unlocked Techniques:**
  - Resurrection techniques (near-death recovery)
  - Curse/hex removal
  - Instant stabilization
  - Group healing capabilities
  
- **Material Sources:**
  - Legendary medical supplies
  - Life-saving elixirs
  - Enchanted medical tools
  - Divine blessing components
  
- **Success Rates:** 92-98% treatment success
- **Healing Amount:** 75-95% of max health restored
- **Treatment Speed:** 5-15 seconds
- **XP Gains:** Very Slow (10-40 XP per treatment)

**In-Game Effects:**
- **Survival:** Critical for dungeon/raid groups
- **Self-Sufficiency:** Reduce potion dependence
- **Economic Service:** Healing services for pay
- **Support Role:** Essential in group content
- **Death Prevention:** Revive incapacitated players

**Skill Dependencies:**
- **Herbalism:** For medicinal plant gathering
- **Alchemy:** For potion and antidote creation
- **Combat:** Understanding injuries and trauma
- **Animal Husbandry:** Transferable medical knowledge

**Specialization Paths:**
- **Battle Surgeon:** Fast emergency treatment in combat
- **Plague Doctor:** Disease and poison specialist
- **Chirurgeon:** Complex surgical procedures

---

### 14. Masonry - Stone Construction and Building

**Real-World Foundation:**
Masonry is the ancient craft of building with stone, brick, and mortar. Historical masons were essential 
craftsmen responsible for constructing everything from simple homes to grand cathedrals. The craft requires 
understanding of materials, structural principles, and precise craftsmanship passed down through generations.

**Skill Progression Tiers:**

#### Tier 1: Apprentice Mason (Levels 1-256)
- **Unlocked Techniques:**
  - Basic stone cutting and shaping
  - Simple mortar mixing
  - Foundation laying
  - Basic wall construction
  
- **Material Sources:**
  - Common stone (limestone, sandstone)
  - Basic mortar (sand, lime)
  - Simple tools (hammer, chisel)
  
- **Success Rates:** 60-75%
- **Quality Range:** Poor to Standard (30-65%)
- **XP Gains:** Fast (50-100 XP per structure)

#### Tier 2: Journeyman Mason (Levels 257-512)
- **Unlocked Techniques:**
  - Advanced stone cutting (precise angles)
  - Arch construction
  - Column building
  - Decorative stonework
  
- **Material Sources:**
  - Quality stone (marble, granite)
  - Specialized mortars
  - Improved tools
  
- **Success Rates:** 75-85%
- **Quality Range:** Standard to Fine (60-78%)
- **XP Gains:** Moderate (30-75 XP per structure)

#### Tier 3: Master Mason (Levels 513-768)
- **Unlocked Techniques:**
  - Complex structural design
  - Vault and dome construction
  - Stone carving and sculpture
  - Load-bearing calculations
  
- **Material Sources:**
  - Rare stone types
  - Premium mortars
  - Master-grade tools
  
- **Success Rates:** 85-92%
- **Quality Range:** Fine to Superior (75-90%)
- **XP Gains:** Slow (18-55 XP per structure)

#### Tier 4: Grand Master Mason (Levels 769-1024)
- **Unlocked Techniques:**
  - Cathedral-scale construction
  - Perfect stone fitting (no mortar needed)
  - Structural innovation
  - Apprentice training and guild leadership
  
- **Material Sources:**
  - Legendary stone materials
  - Specialized binding agents
  - Enchanted construction tools
  
- **Success Rates:** 92-98%
- **Quality Range:** Superior to Legendary (88-99%)
- **XP Gains:** Very Slow (10-40 XP per structure)

**In-Game Effects:**
- **Building Construction:** Essential for permanent structures
- **Defensive Structures:** Walls, towers, fortifications
- **Infrastructure:** Bridges, roads, aqueducts
- **Economic Value:** High-quality buildings increase property value
- **Prestige:** Grand structures bring fame and influence

**Skill Dependencies:**
- **Mining:** For stone material sourcing
- **Engineering:** For structural design (advanced)
- **Blacksmithing:** For tool creation and maintenance

**Specialization Paths:**
- **Fortification Specialist:** Military structures and defenses
- **Cathedral Builder:** Grand religious and civic buildings
- **Stone Carver:** Decorative and artistic stonework

---

### 15. Milling - Grain Processing and Production

**Real-World Foundation:**
Millers operated water or wind-powered mills to grind grain into flour, an essential service in medieval 
communities. The miller was a crucial figure in the food production chain, requiring knowledge of mechanics, 
grain properties, and timing to produce quality flour.

**Skill Progression Tiers:**

#### Tier 1: Mill Hand (Levels 1-256)
- **Unlocked Techniques:**
  - Basic grain grinding
  - Mill stone maintenance
  - Simple sifting
  - Grain storage
  
- **Material Sources:**
  - Common grains (wheat, rye, barley)
  - Basic millstones
  - Simple sieves
  
- **Success Rates:** 65-75%
- **Quality Range:** Poor to Standard (35-60%)
- **Processing Speed:** Slow (10-15 units/hour)
- **XP Gains:** Fast (55-105 XP per batch)

#### Tier 2: Journeyman Miller (Levels 257-512)
- **Unlocked Techniques:**
  - Multi-grain processing
  - Quality grading
  - Mill optimization
  - By-product utilization (bran separation)
  
- **Material Sources:**
  - Quality grains
  - Improved millstones
  - Fine sieves and bolters
  
- **Success Rates:** 75-85%
- **Quality Range:** Standard to Fine (55-75%)
- **Processing Speed:** Moderate (15-25 units/hour)
- **XP Gains:** Moderate (30-70 XP per batch)

#### Tier 3: Master Miller (Levels 513-768)
- **Unlocked Techniques:**
  - Specialized flour production
  - Mill power optimization
  - Weather-based timing
  - Multiple stone management
  
- **Material Sources:**
  - Premium grains
  - Precision-cut millstones
  - Advanced sifting systems
  
- **Success Rates:** 85-92%
- **Quality Range:** Fine to Superior (72-88%)
- **Processing Speed:** Fast (25-40 units/hour)
- **XP Gains:** Slow (18-55 XP per batch)

#### Tier 4: Grand Master Miller (Levels 769-1024)
- **Unlocked Techniques:**
  - Perfect flour production
  - Multi-mill operations
  - Innovative grinding techniques
  - Miller's guild leadership
  
- **Material Sources:**
  - Legendary grain varieties
  - Master-crafted millstones
  - State-of-the-art equipment
  
- **Success Rates:** 92-98%
- **Quality Range:** Superior to Legendary (85-99%)
- **Processing Speed:** Very Fast (40-60 units/hour)
- **XP Gains:** Very Slow (10-35 XP per batch)

**In-Game Effects:**
- **Food Production:** Essential for bread and baking
- **Economic Control:** Mills often held monopolies
- **Community Service:** Central to settlement survival
- **By-Products:** Bran for animal feed, whole grain products
- **Social Standing:** Millers held important economic positions

**Skill Dependencies:**
- **Farming:** For grain sourcing
- **Engineering:** For mill mechanism understanding
- **Cooking:** Primary consumer of flour products

**Specialization Paths:**
- **Grain Master:** Expert in grain selection and grading
- **Mill Engineer:** Mechanical optimization and innovation
- **Baker's Supplier:** Specialized flour for specific baking needs

---

## Player-Created Systems

### Religion and Faith Framework

**Design Philosophy:**
BlueMarble provides a flexible framework for religion and faith systems, allowing players to create, 
develop, and maintain their own belief systems. The game does not prescribe specific religions but 
offers mechanical support for faith-based gameplay.

**Core Mechanics:**

**1. Faith System Creation:**

Players can establish faith systems with the following components:

```
Faith Creation Elements:
- Founding Principles: Core beliefs and tenets (player-defined)
- Sacred Symbols: Visual representations and iconography
- Ritual Practices: Ceremonies, observances, calendar events
- Moral Code: Ethical guidelines for followers
- Hierarchical Structure: Leadership and organizational roles
- Sacred Sites: Locations of religious significance
```

**2. Faith-Related Skills:**

```
Clergy/Religious Leadership (New Skill):
- Ritual Performance: Conducting ceremonies and services
- Scripture Knowledge: Understanding and teaching doctrines
- Community Leadership: Guiding followers
- Sacred Architecture: Temple and shrine design
- Faith Healing: Spiritual/psychological treatment

Levels 1-256: Local clergy
Levels 257-512: Regional religious leaders
Levels 513-768: High priests/priestesses
Levels 769-1024: Prophets and faith founders
```

**3. Faith-Based Progression:**

```
Individual Faith Stat:
- Devotion Level: Personal commitment to faith (0-1024)
- Ritual Participation: Regular observance increases devotion
- Sacred Quests: Faith-specific objectives
- Moral Alignment: Actions consistent with faith principles
- Community Standing: Respect within religious community

Faith Benefits:
- Comfort Bonus: Reduced stress, improved well-being
- Community Support: Aid from fellow believers
- Sacred Sites Access: Special locations and resources
- Divine Inspiration: Creativity and problem-solving boosts
- Social Capital: Influence within faith community
```

**4. Player-Defined Doctrines:**

The system supports diverse belief structures:

```
Possible Faith Types (Player-Created):
- Nature-Based: Reverence for natural world, seasons, elements
- Ancestor Worship: Honoring and seeking guidance from ancestors
- Philosophical: Ethical systems and life principles
- Polytheistic: Multiple deities with different domains
- Monotheistic: Single supreme deity
- Dualistic: Balance between opposing forces
- Agnostic/Secular: Community values without supernatural elements
```

**5. Religious Structures and Buildings:**

```
Sacred Architecture (Masonry + Clergy):
- Small Shrines: Personal or family worship (Levels 1-100)
- Community Temples: Local congregation spaces (Levels 100-300)
- Grand Cathedrals: Regional religious centers (Levels 300-600)
- Sacred Complexes: Multi-building religious compounds (Levels 600-1024)

Building Benefits:
- Ritual efficiency bonuses
- Community gathering spaces
- Storage for religious artifacts
- Training facilities for clergy
- Pilgrimage destinations
```

**6. Faith-Based Economy:**

```
Religious Economics:
- Tithes and Offerings: Voluntary contributions
- Temple Markets: Religious goods and services
- Pilgrimage Trade: Travel and tourism
- Sacred Crafts: Religious artwork and items
- Charity Systems: Community support networks
```

**7. Interfaith Relations:**

```
Faith Interaction Systems:
- Tolerance Levels: How faiths view each other
- Shared Values: Common principles between faiths
- Theological Debate: Peaceful discussion and exchange
- Cooperation: Joint projects and mutual aid
- Conflict Resolution: Mediation and compromise
```

**Implementation Notes:**
- All faith content is player-created and moderated
- No prescribed "correct" religion or belief system
- Mechanical benefits balanced across all faith types
- Encourages roleplay and community building
- Respects diverse real-world beliefs by keeping content fictional

---

### Economic and Governance Frameworks

**Design Philosophy:**
BlueMarble provides foundational systems for economics and governance, allowing players to establish 
and maintain their own economic models and governmental structures. The game offers tools and mechanics 
rather than prescribed systems.

**Economic System Framework:**

**1. Economic Models (Player-Determined):**

```
Possible Economic Systems:
- Free Market: Supply and demand, minimal regulation
- Controlled Economy: State-managed production and prices
- Guild System: Craft-based economic control
- Cooperative: Shared ownership and resources
- Mixed Models: Combination of approaches
- Barter-Based: Non-monetary exchange systems
- Command Economy: Centralized planning
```

**2. Economic Skill: Trade and Commerce**

```
Trade Skill Progression (New Skill):
Levels 1-256: Local Merchant
- Basic buying and selling
- Simple negotiations
- Market awareness
- Inventory management

Levels 257-512: Regional Trader
- Trade route establishment
- Contract negotiation
- Market analysis
- Credit and loans

Levels 513-768: Commerce Master
- International trade
- Economic policy influence
- Trade organization leadership
- Market manipulation

Levels 769-1024: Economic Architect
- System-wide economic planning
- Currency management
- Trade empire building
- Economic theory development
```

**3. Currency and Exchange:**

```
Monetary Systems (Player-Configurable):
- Commodity Money: Gold, silver, goods as currency
- Fiat Currency: Player-issued money tokens
- Multiple Currencies: Region-specific systems
- Credit Systems: Promissory notes and banking
- Cryptocurrency-Style: Digital ledger systems
- Resource-Backed: Currency tied to specific goods
```

**4. Market Mechanics:**

```
Market Systems:
- Price Discovery: Supply/demand equilibrium
- Auction Houses: Competitive bidding
- Guild Markets: Craft-specific exchanges
- Black Markets: Underground trading
- Futures Markets: Advance contracts
- Regulatory Bodies: Player-managed oversight
```

**Governance System Framework:**

**1. Government Types (Player-Established):**

```
Governance Models:
- Democracy: Elected representatives
- Monarchy: Hereditary or appointed rulers
- Oligarchy: Rule by elite group
- Theocracy: Religious leadership
- Meritocracy: Achievement-based leadership
- Anarchism: Self-governing collectives
- Council System: Representative assemblies
- Tribal: Clan-based leadership
```

**2. Governance Skill: Leadership and Administration**

```
Leadership Skill (New Skill):
Levels 1-256: Community Organizer
- Basic group coordination
- Simple rule enforcement
- Dispute resolution
- Resource allocation

Levels 257-512: Regional Administrator
- Policy development
- Multi-community coordination
- Diplomatic relations
- Budget management

Levels 513-768: State Leader
- Large-scale governance
- Legal system development
- International diplomacy
- Economic policy

Levels 769-1024: Nation Builder
- Constitutional development
- Multi-state coordination
- Historical legacy
- System innovation
```

**3. Legal and Justice Systems:**

```
Law Framework (Player-Defined):
- Criminal Code: What constitutes crimes
- Civil Law: Property and contracts
- Court Systems: Dispute resolution
- Enforcement: Guards, militia, police
- Punishment: Fines, imprisonment, banishment
- Appeals Process: Review and revision
```

**4. Civic Infrastructure:**

```
Governance Buildings (Masonry + Leadership):
- Town Halls: Local governance centers
- Courts: Legal proceedings
- Guard Posts: Law enforcement
- Administrative Offices: Bureaucracy
- Diplomatic Embassies: Foreign relations
- Public Works: Infrastructure management
```

**5. Taxation and Public Finance:**

```
Fiscal Systems (Player-Determined):
- Tax Types: Income, property, sales, tariffs
- Tax Rates: Player-set levels
- Public Services: What government provides
- Budget Allocation: Spending priorities
- Debt Management: Loans and obligations
- Transparency: Public vs. private finances
```

**6. Diplomatic Relations:**

```
Inter-Community Diplomacy:
- Treaties and Agreements
- Trade Pacts
- Military Alliances
- Cultural Exchanges
- Conflict Resolution
- Ambassador Systems
```

**Integration with Existing Systems:**

```
Cross-System Interactions:
- Economic Impact on Skills: Price affects material availability
- Governance Impact on Trade: Regulations and tariffs
- Faith Impact on Law: Moral codes influencing legal systems
- Skill Impact on Economy: Craftsmen drive production
- Community Impact on Governance: Population needs shape policy
```

**Implementation Philosophy:**

1. **Framework, Not Prescription:** Game provides tools, players create content
2. **Emergent Systems:** Complex behaviors arise from simple mechanics
3. **Player Agency:** Communities decide their own structures
4. **Dynamic Evolution:** Systems can change over time
5. **Conflict Resolution:** Built-in mechanics for disputes
6. **Historical Authenticity:** Inspired by real systems but not bound by them
7. **Scalability:** Works for small groups and large nations

**Example Player-Created Scenarios:**

```
Scenario 1: Guild Republic
- Economy: Guild-controlled craft production
- Government: Council of guild masters
- Faith: Craft-deity polytheism
- Result: Specialized, high-quality production with oligarchic control

Scenario 2: Theocratic Farming Community
- Economy: Cooperative agriculture
- Government: Religious council leadership
- Faith: Nature-based spirituality
- Result: Sustainable, community-focused settlement

Scenario 3: Free Market Democracy
- Economy: Laissez-faire capitalism
- Government: Representative democracy
- Faith: Secular ethics
- Result: Dynamic, competitive economic growth
```

These frameworks allow players to experiment with different social, economic, and governmental structures, 
creating unique gameplay experiences while maintaining balanced mechanics and avoiding prescriptive designs.

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
XP_Required(level) = 8 × (level^1.08)

Examples:
Level 1 → 2:     10 XP
Level 10 → 11:   96 XP
Level 50 → 51:   546 XP
Level 100 → 101: 1,156 XP
Level 256 → 257: 3,191 XP
Level 512 → 513: 6,746 XP
Level 1024:      Total ~7.0 million XP
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
- **Novice (1-256):** 35-45 hours of focused practice
- **Journeyman (257-512):** 120-140 hours additional
- **Expert (513-768):** 210-240 hours additional
- **Master (769-1024):** 320-360 hours additional

**Total to Master:** 685-785 hours per skill (realistic specialization with deep mastery)

### Cross-Skill Synergies

Skills provide minor bonuses to related skills:

```
Related_Skill_Bonus = (Related_Skill_Level × 0.1) capped at +10 levels

Examples:
- Herbalism Level 50 provides +5 effective levels to Alchemy
- Blacksmithing Level 40 provides +4 effective levels to Mining
- Cooking Level 30 provides +3 effective levels to Alchemy
```

### Routine-Based Progression System

**Concept Overview:**
BlueMarble features a comprehensive routine-based progression system where characters are always controlled by 
routines, whether players are online or offline. This system emphasizes strategic planning and automation over 
direct player control, creating a unique gameplay experience where success comes from designing effective routines 
rather than moment-to-moment input.

**Core Philosophy:**
- Characters operate via routines at all times (online and offline)
- Players design, modify, and optimize routines rather than directly controlling actions
- Progression comes from routine mastery, not time logged in
- Strategic depth through routine programming and optimization

**Core Mechanics:**

**1. Routine System:**

Players interact with their characters primarily through routine management:

```
Routine Types:
- Cyclic Routines: Repeat continuously (craft 10 items, rest, repeat)
- Event-Driven Routines: Trigger on conditions (when fiber < 0.000025 ETH, buy 100)
- Sequential Routines: Execute steps in order, then stop
- Conditional Routines: Branch based on inventory, prices, time, etc.
- Hybrid Routines: Combine multiple types

Routine Management:
- Create Routine: Design new routine from scratch
- Save Routine: Store routine for future use
- Load Routine: Apply saved routine to character
- Modify Routine: Edit currently running or saved routine
- Share Routine: Export to other players or guild members
```

**2. Routine Execution:**

```
Execution Rules:
- Character must complete current routine block before switching
- Blocks are atomic: finish or fail, no partial completion
- Player can queue routine changes (applied after current block)
- Travel automatically handled if routine requires specific location
- Resource availability checked before block execution

Block Completion States:
- Success: Block completed as designed
- Failure: Block failed (insufficient resources, skill check failed)
- Interrupted: External event (attacked, building destroyed)
- Queued Change: New routine waiting to activate
```

**3. Routine Building Blocks:**

```
Basic Actions:
- Craft [item] × [quantity]
- Gather [resource] from [location]
- Travel to [location]
- Rest for [duration]
- Trade [item] at [market]
- Train [skill] for [duration]
- Process [material] into [product]

Conditional Logic:
- If [condition] then [action] else [action]
- While [condition] do [action]
- Wait until [condition]
- Repeat [action] [times or until condition]

Conditions:
- Inventory level (if fiber > 100)
- Market price (if fiber < 0.000025 ETH per unit)
- Time of day/season
- Skill level reached
- Location status (if workshop available)
- Resource availability
- Character state (health, energy, hunger)

Event Triggers:
- Price threshold crossed
- Resource depleted
- Skill level up
- Market opportunity detected
- Guild notification received
- Time-based (daily reset, seasonal change)
```

**4. Example Routines:**

```
Example 1: Simple Crafting Loop (Cyclic)
1. Check inventory for 50 iron ore
2. If insufficient, travel to mine and gather 100 ore
3. Travel to blacksmith workshop
4. Craft 10 iron daggers
5. Rest for 1 hour
6. Repeat from step 1

Example 2: Market Arbitrage (Event-Driven)
1. Monitor global fiber auction
2. When price < 0.000025 ETH per unit:
   - Buy 1000 units
   - Travel to processing facility
   - Process fiber into thread
   - List thread on market at 150% markup
3. When thread sells, return to step 1
4. If no trigger for 6 hours, craft items from inventory

Example 3: Farming Operations (Hybrid)
Morning Block (6 AM - 12 PM):
- Check crop readiness
- Harvest ready crops
- Replant empty fields
- Water young plants
- Feed animals

Afternoon Block (12 PM - 6 PM):
- If market open: sell harvested goods
- If fiber price < threshold: buy fiber for weaving
- Repair tools if durability < 50%
- Craft fertilizer from by-products

Evening Block (6 PM - 12 AM):
- Process grains at mill
- Craft textiles from fiber
- Train tailoring skill
- Rest

Night Block (12 AM - 6 AM):
- Sleep/Rest (passive skill gain)
- Automated security routine
```

**5. Routine Change Management:**

```
While Character is Online:
- View current routine execution status
- See which block is running (e.g., "Crafting iron daggers: 3/10 complete")
- Queue routine change (applies after current block finishes)
- Emergency stop (abandon current block, incur failure penalties)
- Real-time monitoring of routine performance

Block Completion Requirements:
- Current block must complete successfully OR fail before switching
- Cannot interrupt mid-block (prevents exploitation)
- Partial progress lost if emergency stop used
- Queued changes execute immediately after block completion

Routine Change Workflow:
1. Player selects "Change Routine"
2. System shows: "Current block will complete first (est. 15 minutes)"
3. Player designs/loads new routine
4. New routine queued with notification
5. Current block finishes → New routine activates
6. Player receives confirmation notification
```

**6. Location-Based Routines:**

```
Travel Requirements:
- If routine requires specific location, character auto-travels
- Travel time calculated based on distance and terrain
- Character follows safest/fastest route based on settings
- Can be interrupted by events (combat, weather, roadblocks)

Location Types:
- Workshop: Fixed building (blacksmith, tailor shop)
- Resource Node: Mine, forest, fishing spot
- Market: Trading hub, auction house
- Guild Hall: Social and crafting facilities
- Home: Personal rest and storage location
- Mobile: Traveling merchant, nomadic activities

Travel Integration:
"Travel to [Location]" automatically added as first block if needed
Example: Character at home, routine requires mine
- Block 1: Travel to mine (auto-inserted)
- Block 2: Gather 100 ore (routine starts here)
- Block 3: Travel to workshop (auto-inserted)
- Block 4: Craft items
```

**7. XP and Progression Mechanics:**

```
XP_Per_Hour = Base_XP_Rate × Routine_Efficiency × Facility_Quality × Skill_Match

Base_XP_Rate: Equal for online and offline (routine-based design)
Routine_Efficiency: 0.5-1.0 based on routine optimization quality
Facility_Quality: 0.8-1.2 based on workshop/facility upgrades  
Skill_Match: 0.7-1.3 based on routine vs. character skill levels

Routine Effectiveness Factors:
High Effectiveness (90-100% efficiency):
- Routine perfectly matched to character skills
- Optimal resource flow (no bottlenecks)
- Quality tools and facilities available
- Conditional logic handles edge cases well
- Event triggers properly configured

Medium Effectiveness (70-90% efficiency):
- Routine generally appropriate for character
- Some resource inefficiencies
- Standard tools and facilities
- Basic conditional logic
- Limited event handling

Low Effectiveness (50-70% efficiency):
- Routine mismatched to character skills
- Frequent resource shortages
- Poor facilities or missing tools
- No conditional logic (rigid routine)
- No event-driven optimization
```

**8. Market Integration and Event-Driven Trading:**

```
Market Condition Triggers:
- Price thresholds (buy when price < X, sell when price > Y)
- Volume triggers (when daily volume > Z)
- Spread opportunities (buy location A, sell location B when spread > threshold)
- Supply/demand signals
- Seasonal price patterns

Example Trading Routine:
Monitor:
- Global fiber auction price every 30 minutes
- Local thread demand every hour

Conditions:
- If fiber price < 0.000025 ETH per unit on global auction:
  → Buy 1000 units
  → Queue processing routine
  
- If thread price > 0.00005 ETH per unit on local market:
  → Sell processed thread inventory
  → Calculate profit
  → Adjust future price thresholds

Actions:
- Automatic bid placement
- Transaction execution
- Inventory management
- Profit tracking
- Price history analysis for optimization

Integration with Skills:
- Trade skill level affects transaction fees
- Higher skill = better price discovery
- Market knowledge improves trigger accuracy
- Reputation affects available markets
```

**9. Routine Failure Handling:

Offline routines consume and produce materials realistically:

```
Crafting Routines:
- Consume raw materials from character storage
- Produce items based on skill level and success rates
- Quality follows normal distribution (slightly lower average than active play)
- Failed attempts consume materials but provide learning XP

Gathering Routines:
- Generate resources over time
- Quality and quantity based on skill level
- Location resource availability matters
- Tool durability decreases normally

Agricultural Routines:
- Crops grow according to real-time cycles
- Animals require feeding from storage
- Harvests occur automatically at maturity
- Quality varies with care level
```

**5. Skill Point Accumulation:**

Characters can earn skill points through offline progression, but at reduced rates:

```
Offline Skill Point Rate:
- Active Play: 1 skill point per level
- Offline Play: 0.5 skill points per level (capped)
- Bonus: +0.5 skill points if routine perfectly matches leveled skill

Maximum Offline Skill Points: 10 per week (prevents pure offline progression)
Encourages: Regular login to spend points and adjust routines
```

**6. Risk and Reward Balance:**

Offline progression includes realistic risks:

```
Potential Events (Random, Low Probability):
- Workshop accidents (minor tool damage, small XP loss)
- Resource spoilage (food, organic materials decay)
- Weather impacts (farming, gathering affected)
- Facility wear (requires maintenance investment)
- Breakthrough moments (rare +10% XP bonus for session)

Mitigation:
- Higher facilities quality reduces risk
- Appropriate routine selection minimizes problems
**9. Routine Failure Handling:**

```
Failure Types:
- Resource Shortage: Insufficient materials mid-routine
- Skill Check Failure: Action too difficult for current skill
- Location Unavailable: Required building destroyed/occupied
- Market Condition Failed: Price condition never met
- Travel Interrupted: Cannot reach required location
- Tool Breakage: Equipment durability reached zero

Failure Responses (Routine Logic):
- Fallback Actions: "If craft fails, gather more materials"
- Alternative Paths: "If workshop occupied, travel to backup"
- Emergency Stops: "If gold < 100, pause trading routine"
- Notification Triggers: "Alert me if routine fails 3 times"
- Auto-Recovery: "Repair tools when durability < 20%"

Failure Penalties:
- Partial material consumption (25-50% lost)
- Time wasted (travel, setup time)
- Tool durability loss
- Reputation impact for failed contracts
- XP loss minimized (learning experience)

Mitigation Strategies:
- Buffer resources (e.g., keep 20% extra materials)
- Condition checks before expensive actions
- Fallback routines for common failure modes
- Insurance systems (guild or NPC)
- Redundant pathways in routine design
```

**10. Social Integration:**

Routine-based gameplay enhances multiplayer systems:

```
Guild Benefits:
- Shared routine libraries (master crafters share optimized routines)
- Guild workshops provide higher facility quality
- Shared material pools support continuous routines
- Guild members can maintain others' facilities
- Coordinated routines for group activities

Cooperative Routines:
- Farm collective: Members contribute to shared farm
- Trade networks: Automated arbitrage between regions
- Production chains: One player's output feeds another's input
- Defense coordination: Alert members when routine interrupted by attack

Player Economy Impact:
- Continuous production maintains market liquidity
- Event-driven trading stabilizes prices
- Specialization enabled (full-time merchants, crafters)
- Creates arbitrage opportunities for traders
- Routine efficiency becomes tradeable knowledge
```

**11. Implementation Considerations:**

**Balance Goals:**
- Online and offline XP rates equal when routines properly configured
- Encourages strategic routine design and optimization
- Success depends on routine quality, not playtime
- Respects all player schedules (casual to hardcore)
- Deep strategic gameplay through routine programming

**Technical Requirements:**
- Server-side routine execution engine
- Real-time event monitoring (prices, conditions)
- Material inventory tracking and validation
- Travel pathfinding and time calculation
- Market integration for automated trading
- Routine validation and syntax checking
- Failure detection and recovery systems

**Player Interface:**
```
Routine Builder UI:
- Drag-and-drop block creation
- Conditional logic flow visualization
- Real-time validation and warnings
- Simulation mode (test without executing)
- Performance analytics (efficiency metrics)
- Market data integration
- Community routine sharing

In-Game Notifications:
- Routine status updates
- Block completion alerts
- Failure notifications with cause
- Market trigger activations
- Resource level warnings
- Performance analytics summaries
```

**Player Communication:**
```
Session Summary (Login After Offline):
"While you were away for 2 days, 4 hours:

Routine Performance:
- Blacksmithing Routine: 85% efficiency
- Executed 47 blocks successfully, 3 failures
- XP Gained: +2,450 Blacksmithing (Levels 245 → 247)

Production:
- Crafted: 15 Iron Daggers (3 Fine, 10 Standard, 2 Poor quality)
- Materials Consumed: 45 Iron Ingots, 15 Oak Handles
- Materials Remaining: 12 Iron Ingots, 4 Oak Handles

Trading:
- Fiber price trigger activated 2 times
- Bought: 2000 fiber units at avg 0.000023 ETH
- Sold: 150 thread units at avg 0.000048 ETH  
- Trading Profit: +0.0375 ETH

Travel:
- Traveled 45 km total (Workshop → Mine → Market → Home)
- Travel time: 3.2 hours

Failures:
- 2× Resource shortage (iron ingots depleted)
- 1× Workshop occupied (waited 20 minutes)

Recommendations:
- Increase iron ingot buffer from 50 to 75 units
- Add conditional check for workshop availability
- Current routine efficiency could improve to 92% with adjustments
- [View Detailed Log] [Optimize Routine]"
```

**12. Design Philosophy:**

The routine-based progression system reflects BlueMarble's core gameplay philosophy where characters operate 
via routines at all times, whether players are online or offline:

1. **Always-On Routines:** Characters controlled by routines 24/7, not direct player input
2. **Strategic Gameplay:** Success from routine design and optimization, not reflexes
3. **Equal Time Value:** Online and offline progression identical when routines properly configured
4. **Programming Depth:** Complex conditional logic and event-driven behaviors
5. **Maintains Authenticity:** Characters "live" continuously through intelligent routines
6. **Enables All Playstyles:** Supports casual to hardcore, traders to crafters
7. **Supports Dynamic Economy:** Continuous market activity via event-driven trading
8. **Rewards Mastery:** Best players master routine programming, not clickspeed

**Key Differentiators:**
- Players are routine programmers, not direct controllers
- Online time used for routine design, testing, and optimization
- Offline time executes routines with equal efficiency
- Market integration enables automated trading and arbitrage
- Social systems reward sharing optimized routines
- Deep strategic gameplay through routine complexity

This system creates unique gameplay where mastering routine programming and understanding game systems 
matters more than time logged in, making BlueMarble accessible to all schedules while providing 
extraordinary depth for those who engage with the routine system.

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

**Combat & Survival Skills:**
12. Combat - Armed and unarmed fighting
13. First Aid - Emergency medical treatment
14. Survival - Wilderness navigation and shelter

**Agricultural Skills:**
15. Farming - Crop cultivation and management
16. Animal Husbandry - Livestock care and breeding

**Advanced Skills (Specialized Crafts):**
17. Engineering - Mechanisms, devices
18. Enchanting - Magical augmentation
19. Jewelry - Precious metal/gem work
20. Masonry - Stone construction

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
| 1     | 10          | 10            | 0.0              |
| 10    | 96          | 508           | 0.1              |
| 50    | 546         | 13,401        | 1.3              |
| 100   | 1,156       | 56,128        | 5.6              |
| 256   | 3,191       | 394,271       | 39               |
| 512   | 6,746       | 1,663,886     | 166              |
| 768   | 10,453      | 3,864,760     | 386              |
| 1024  | 14,263      | 7,028,455     | 703              |

*Estimated hours assume average crafting activity, not continuous grinding

**Tier Boundaries:**
- Tier 1 completion (Level 256): ~39 hours
- Tier 2 completion (Level 512): ~166 hours cumulative
- Tier 3 completion (Level 768): ~386 hours cumulative  
- Tier 4 completion (Level 1024): ~703 hours cumulative

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
1. **Fifteen Core Skills** provide diverse gameplay opportunities across crafting, combat, agriculture, survival, and historical professions
2. **Four-Tier Progression** (256 levels per tier, 1024 total) creates clear advancement with fine-grained mastery
3. **Material Quality** from geological/botanical systems drives crafting depth
4. **Skill Dependencies** encourage specialization and player cooperation
5. **Visual UI Elements** communicate complex systems clearly to players
6. **Combat & Survival Skills** expand beyond crafting to complete gameplay experience
7. **Agricultural Systems** (Farming, Animal Husbandry) support player-driven economy
8. **Historical Professions** (Masonry, Milling) add authentic medieval depth
9. **Extended Level System** (1024 levels) provides long-term progression goals and meaningful incremental improvements
10. **Routine-Based Progression** where characters operate via routines 24/7 (online and offline), with players as routine programmers
11. **Player-Created Systems** for religion, economics, and governance enable emergent social structures and diverse community experiences

**Next Steps:**
1. Implement Phase 1 core skills (Tailoring, Cooking, Herbalism, Mining, Farming)
2. Add Phase 1b survival and historical skills (Combat, First Aid, Forestry, Masonry, Milling)
3. Develop player-created system frameworks (Religion, Trade, Leadership skills)
4. Develop material quality algorithms integrated with geological simulation
5. Create crafting UI based on fiber crafting example
6. Design combat, healing, and governance interfaces
7. Implement offline progression system with routine assignment
8. Test progression curves with player feedback
9. Iterate on balance and timing based on real gameplay data
10. Create community tools for player-defined religions and governance

**Related Research:**
- [Cursor Accuracy and Gender Accessibility Research](../../../literature/game-dev-research-cursor-accuracy-gender-accessibility.md) - Analysis of routine-based systems and accessibility design for underserved demographics
- [The Sims and Gaming Woman Phenomenon](../../../literature/game-dev-analysis-the-sims-and-gaming-women-phenomenon.md) - Case study on accessible simulation design

---

**Document Version History:**
| Version | Date | Changes |
|---------|------|---------|
| 1.0 | 2025-10-01 | Initial comprehensive research report |
| 1.1 | 2025-10-01 | Added Combat, Farming, Forestry, Animal Husbandry, and First Aid skills |
| 2.0 | 2025-10-01 | Expanded to 1024-level system (256 levels per tier) with adjusted XP progression |
| 2.1 | 2025-10-01 | Added offline progression system with routine-based skill development |
| 2.2 | 2025-10-01 | Updated offline progression to treat offline and online time equally (routine-based design) |
| 2.3 | 2025-10-01 | Added historical professions (Masonry, Milling) and player-created systems (Religion, Economics, Governance) |
| 2.4 | 2025-10-01 | Expanded routine system: characters always run routines (online/offline), cyclic/event-driven, market integration |
