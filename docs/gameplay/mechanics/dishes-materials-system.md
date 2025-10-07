# Dishes & Materials System

**Version:** 1.0  
**Date:** 2025-01-06  
**Status:** Design Specification

## Overview

The Dishes & Materials System defines the comprehensive **item ecosystem** for consumables (dishes/food)
and raw/crafted **materials** in BlueMarble. This system groups items into logical categories, defines their
properties, and outlines mechanics such as spoilage, preservation, rarity, and crafting integration.
The goal is to create **meaningful item diversity** that supports survival, trade, and progression systems.

## Table of Contents

1. [Purpose & Scope](#purpose--scope)
2. [Core Concepts](#core-concepts)
3. [Item Properties](#item-properties)
4. [Item Categories](#item-categories)
5. [Game Mechanics](#game-mechanics)
6. [Data Model](#data-model)
7. [Balancing Rules](#balancing-rules)
8. [Integration Points](#integration-points)
9. [Testing & Validation](#testing--validation)
10. [Implementation Guidelines](#implementation-guidelines)

## Purpose & Scope

### Purpose

This document establishes the foundational framework for all consumable and material items in BlueMarble, ensuring:

- **Meaningful item diversity** across all gameplay tiers
- **Integrated economy** where materials flow into consumables
- **Dynamic systems** for spoilage, preservation, and quality
- **Balanced progression** from simple staples to luxury goods

### Scope

**In Scope:**

- Raw materials (ores, wood, herbs, animal products)
- Processed materials (ingots, cloth, planks, leather)
- Food items (fresh, preserved, luxury)
- Material properties and crafting relationships
- Spoilage and preservation mechanics
- Buff system for consumables

**Out of Scope:**

- Equipment and weapons (covered in separate documentation)
- Quest items and story materials
- Player housing and furniture (separate system)

## Core Concepts

### Materials

Materials form the foundation of the crafting economy and are categorized by their processing stage and function:

#### Raw Materials

Gathered directly from the environment through harvesting, mining, hunting, or foraging.

**Characteristics:**

- **Abundant availability** in specific biomes/regions
- **Low base value** but essential for all crafting
- **Variable quality** based on gathering skill and location
- **Renewable resources** with respawn mechanics

**Examples:**

- Ores: Copper Ore, Iron Ore, Silver Ore, Gold Ore, Mythril Ore
- Wood: Oak Logs, Pine Logs, Bamboo, Yew Wood
- Herbs: Healing Herb, Nightshade, Fireflower, Frostleaf
- Animal Products: Raw Hide, Bone, Horn, Feathers

#### Processed Materials

Created from raw materials through crafting processes, requiring specific skills and facilities.

**Characteristics:**

- **Higher value** than raw materials
- **Skill requirements** for processing
- **Quality inheritance** from raw materials
- **Intermediate components** for advanced recipes

**Examples:**

- Metals: Copper Ingot, Iron Ingot, Steel Ingot, Silver Bar, Gold Bar
- Wood Products: Oak Planks, Pine Boards, Treated Timber
- Textiles: Linen Cloth, Wool Cloth, Silk Fabric, Leather
- Alchemy: Herb Extract, Potion Base, Alchemical Salt

#### Rare Materials

Low-frequency drops or difficult-to-obtain resources with special properties.

**Characteristics:**

- **Very high value** and trade demand
- **Special properties** (elemental affinity, enhanced durability)
- **Limited availability** through rare spawns or high-level content
- **Prestige crafting** component for legendary items

**Examples:**

- Gems: Ruby, Sapphire, Emerald, Diamond, Obsidian
- Exotic Materials: Mythril, Adamantine, Dragonscale, Phoenix Feather
- Rare Herbs: Ancient Root, Moonflower, Starlight Moss
- Enchanted Materials: Arcane Crystal, Void Essence, Life Stone

#### Functional Materials

Materials with specific utility in crafting or system interactions.

**Characteristics:**

- **Purpose-driven** design (tools, stations, consumables)
- **Tier-gated** availability and usage
- **Durability mechanics** for tool materials
- **Catalyst properties** for advanced crafting

**Examples:**

- Tool Components: Hammer Head, Saw Blade, Pickaxe Head
- Crafting Catalysts: Flux, Oil, Wax, Glue
- Preservation Materials: Salt, Sugar, Smoke Chips, Ice Blocks
- Construction Materials: Stone Block, Clay Brick, Metal Plate

### Dishes (Food/Consumables)

Consumable items that restore health, energy, or provide temporary buffs to player stats.

#### Simple Foods

Single-ingredient or minimal-recipe consumables.

**Characteristics:**

- **Easy to craft** with low skill requirements
- **Quick restoration** of basic needs
- **Short shelf-life** without preservation
- **Abundant ingredients** from basic gathering

**Examples:**

- Berries (blackberry, strawberry, blueberry)
- Bread (simple wheat bread)
- Boiled Egg
- Roasted Meat (basic preparation)

#### Composite Dishes

Multi-ingredient meals requiring cooking skills and proper facilities.

**Characteristics:**

- **Moderate complexity** in recipes
- **Better restoration** than simple foods
- **Temporary buffs** to stats or regeneration
- **Balanced ingredient requirements**

**Examples:**

- Vegetable Stew (vegetables + water + herbs)
- Roasted Fish with Herbs (fish + herbs + oil)
- Meat Pie (meat + flour + vegetables)
- Omelet with Cheese (eggs + cheese + butter)

#### Preserved Foods

Foods processed for extended shelf-life through preservation methods.

**Characteristics:**

- **Extended durability** (3x to 5x normal shelf-life)
- **Preservation skill** requirements
- **Retain nutritional value** but may lose buff potency
- **Trade-optimized** for long-distance commerce

**Examples:**

- Salted Fish (fish + salt)
- Smoked Sausage (meat + smoke + salt)
- Dried Fruit (fruit + drying process)
- Pickled Vegetables (vegetables + brine + time)

#### Luxury Dishes

Rare-ingredient meals with significant buffs and high value.

**Characteristics:**

- **Complex recipes** requiring rare ingredients
- **Multiple powerful buffs** with long duration
- **High skill requirements** and special facilities
- **Prestige items** for trade and consumption

**Examples:**

- Feast Platter (multiple meats + rare herbs + wine)
- Wine-Stewed Boar (boar meat + aged wine + exotic spices)
- Golden Cake (rare flour + honey + eggs + saffron)
- Dragon's Breath Soup (dragon meat + volcanic herbs + essence)

## Item Properties

### Shared Properties (All Items)

All items in the system share these core properties:

```json
{
  "item_id": "unique_identifier",
  "item_name": "Display Name",
  "category": "material|food|consumable",
  "subcategory": "raw|processed|simple|preserved|luxury",
  "rarity": "common|uncommon|rare|epic|legendary|mythic",
  "weight": 0.0,
  "volume": 0.0,
  "base_value": 0,
  "stack_size": 1,
  "description": "Flavor text and usage notes",
  "icon_reference": "asset_path",
  "is_tradeable": true,
  "required_level": 1,
  "tags": ["tag1", "tag2"]
}
```

**Property Details:**

- **item_id**: Unique identifier (format: `category_subcategory_####`)
- **rarity**: Affects drop rates, value multipliers, and visual presentation
- **weight/volume**: Impacts inventory capacity and transport costs
- **base_value**: Foundation for dynamic pricing in markets
- **stack_size**: Maximum quantity per inventory slot
- **required_level**: Minimum player level to use/equip

### Food-Specific Properties

Consumable items have additional properties:

```json
{
  "nutrition_value": 0,
  "energy_restoration": 0,
  "health_restoration": 0,
  "buffs": ["buff_id_1", "buff_id_2"],
  "debuffs": ["debuff_id_1"],
  "spoilage_rate": 0.0,
  "freshness": 100.0,
  "preservation_method": "none|salted|smoked|dried|pickled|frozen",
  "preservation_multiplier": 1.0,
  "consumption_time": 0.0,
  "satiety_value": 0
}
```

**Property Details:**

- **nutrition_value**: Primary stat restored (0-100 scale)
- **energy_restoration**: Stamina/action points restored
- **health_restoration**: HP restored on consumption
- **buffs/debuffs**: References to buff system effects
- **spoilage_rate**: Base decay per hour (0.0 = non-perishable)
- **freshness**: Current quality state (100 = fresh, 0 = spoiled)
- **preservation_method**: Applied preservation technique
- **preservation_multiplier**: Extends shelf-life (e.g., 3.0 = 3x longer)
- **consumption_time**: Time to eat/drink (seconds)
- **satiety_value**: Fullness/hunger satisfaction

### Material-Specific Properties

Materials have crafting-focused properties:

```json
{
  "crafting_tier": 1,
  "durability_value": 0,
  "quality_modifier": 1.0,
  "elemental_affinity": "none|fire|frost|poison|arcane|physical",
  "processing_recipes": ["recipe_id_1", "recipe_id_2"],
  "tool_requirement": "none|furnace|anvil|loom|alchemylab",
  "yield_multiplier": 1.0,
  "refinement_stages": 1
}
```

**Property Details:**

- **crafting_tier**: Progression tier (1-10, higher = advanced)
- **durability_value**: For tool/weapon materials (max uses)
- **quality_modifier**: Bonus to crafted item quality (0.5-2.0)
- **elemental_affinity**: Special properties for enchanting/effects
- **processing_recipes**: List of recipes using this material
- **tool_requirement**: Crafting station needed
- **yield_multiplier**: Bonus output in processing (skill-based)
- **refinement_stages**: Number of processing steps to final form

## Item Categories

### Materials Groupings

#### Metals & Ores

**Purpose**: Foundation for tools, weapons, armor, and construction.

| Item Name | Type | Rarity | Tier | Base Value | Notes |
|-----------|------|--------|------|------------|-------|
| Copper Ore | Raw | Common | 1 | 5 cr | Early-game metal |
| Iron Ore | Raw | Common | 2 | 15 cr | Primary metal for tools |
| Silver Ore | Raw | Uncommon | 3 | 50 cr | Valuable metal, good conductivity |
| Gold Ore | Raw | Rare | 4 | 150 cr | Luxury metal, poor durability |
| Mythril Ore | Raw | Epic | 6 | 500 cr | Magical metal, lightweight |
| Copper Ingot | Processed | Common | 1 | 12 cr | Refined copper (2 ore → 1 ingot) |
| Iron Ingot | Processed | Common | 2 | 35 cr | Refined iron (2 ore → 1 ingot) |
| Steel Ingot | Processed | Uncommon | 3 | 120 cr | Iron + Coal alloy |
| Silver Bar | Processed | Uncommon | 3 | 110 cr | Refined silver |
| Gold Bar | Processed | Rare | 4 | 320 cr | Refined gold |

**Crafting Flow**: `Ore → Mining → Ingot/Bar (Smelting) → Equipment/Tools`

#### Wood & Plant Fibers

**Purpose**: Construction, tools, furniture, and textile production.

| Item Name | Type | Rarity | Tier | Base Value | Notes |
|-----------|------|--------|------|------------|-------|
| Oak Log | Raw | Common | 1 | 8 cr | Sturdy general-purpose wood |
| Pine Log | Raw | Common | 1 | 6 cr | Lightweight, easy to work |
| Bamboo | Raw | Uncommon | 2 | 12 cr | Fast-growing, flexible |
| Yew Wood | Raw | Rare | 4 | 80 cr | Excellent for bows, slow growth |
| Oak Planks | Processed | Common | 1 | 15 cr | Standard construction material |
| Pine Boards | Processed | Common | 1 | 12 cr | Lightweight construction |
| Treated Timber | Processed | Uncommon | 2 | 45 cr | Weather-resistant planks |
| Flax | Raw | Common | 1 | 5 cr | Textile fiber |
| Linen Cloth | Processed | Common | 2 | 25 cr | Basic fabric from flax |

**Crafting Flow**: `Log → Cutting → Planks → Construction/Furniture`

#### Animal Products

**Purpose**: Armor, clothing, food ingredients, and crafting components.

| Item Name | Type | Rarity | Tier | Base Value | Notes |
|-----------|------|--------|------|------------|-------|
| Raw Hide | Raw | Common | 1 | 10 cr | Unprocessed animal skin |
| Leather | Processed | Common | 2 | 35 cr | Tanned hide |
| Bone | Raw | Common | 1 | 5 cr | Crafting component |
| Horn | Raw | Uncommon | 2 | 25 cr | Weapon/tool component |
| Feathers | Raw | Common | 1 | 3 cr | Arrow fletching, decoration |
| Wool | Raw | Common | 1 | 8 cr | Textile fiber |
| Wool Cloth | Processed | Common | 2 | 30 cr | Warm fabric |

**Crafting Flow**: `Hide → Tanning → Leather → Armor/Clothing`

#### Alchemy & Herbs

**Purpose**: Potions, cooking ingredients, buffs, and medicine.

| Item Name | Type | Rarity | Tier | Base Value | Notes |
|-----------|------|--------|------|------------|-------|
| Healing Herb | Raw | Common | 1 | 15 cr | Basic healing ingredient |
| Nightshade | Raw | Uncommon | 3 | 45 cr | Poison ingredient |
| Fireflower | Raw | Rare | 4 | 80 cr | Fire resistance ingredient |
| Frostleaf | Raw | Rare | 4 | 75 cr | Cold resistance ingredient |
| Herb Extract | Processed | Uncommon | 2 | 50 cr | Concentrated herb essence |
| Potion Base | Processed | Uncommon | 2 | 40 cr | Foundation for all potions |
| Alchemical Salt | Processed | Rare | 3 | 100 cr | Catalyst for advanced alchemy |

**Crafting Flow**: `Herb → Extraction → Extract/Base → Potion/Buff Item`

#### Rare Gems & Crystals

**Purpose**: Enchanting, jewelry, high-value trade, and prestige items.

| Item Name | Type | Rarity | Tier | Base Value | Notes |
|-----------|------|--------|------|------------|-------|
| Ruby | Raw | Rare | 5 | 250 cr | Fire affinity |
| Sapphire | Raw | Rare | 5 | 240 cr | Frost affinity |
| Emerald | Raw | Rare | 5 | 230 cr | Nature affinity |
| Diamond | Raw | Epic | 7 | 800 cr | Extreme durability |
| Obsidian | Raw | Uncommon | 3 | 60 cr | Volcanic glass, sharp edges |
| Arcane Crystal | Raw | Legendary | 8 | 2000 cr | Magical energy storage |

**Crafting Flow**: `Raw Gem → Cutting → Cut Gem → Jewelry/Enchanting`

### Dishes Groupings

#### Staple Foods

**Purpose**: Basic sustenance, readily available, low cost.

| Item Name | Ingredients | Nutrition | Energy | Spoilage Rate | Base Value | Notes |
|-----------|-------------|-----------|--------|---------------|------------|-------|
| Bread | Flour + Water | 30 | 25 | 0.08/hr | 5 cr | Basic staple |
| Rice Bowl | Rice + Water | 35 | 30 | 0.10/hr | 6 cr | Filling meal |
| Porridge | Oats + Milk | 40 | 35 | 0.12/hr | 8 cr | Warm breakfast |
| Apple | - | 15 | 10 | 0.15/hr | 2 cr | Fresh fruit |

**Characteristics**: 1-2 day shelf-life, no buffs, available from vendors

#### Protein Dishes

**Purpose**: Health restoration, strength buffs, moderate complexity.

| Item Name | Ingredients | Nutrition | Health | Buffs | Spoilage Rate | Base Value |
|-----------|-------------|-----------|--------|-------|---------------|------------|
| Roasted Fish | Fish + Oil | 50 | 30 | - | 0.15/hr | 15 cr |
| Grilled Meat | Meat + Salt | 60 | 40 | +5% Strength (30m) | 0.12/hr | 25 cr |
| Omelet | Eggs + Butter | 45 | 25 | - | 0.18/hr | 12 cr |
| Meat Stew | Meat + Vegetables | 70 | 50 | +10% Health Regen (1h) | 0.10/hr | 35 cr |

**Characteristics**: 12-24 hour shelf-life, moderate buffs, requires cooking skill

#### Soups & Stews

**Purpose**: Multiple stat restoration, long-duration buffs, communal meals.

| Item Name | Ingredients | Nutrition | Energy | Buffs | Spoilage Rate | Base Value |
|-----------|-------------|-----------|--------|-------|---------------|------------|
| Vegetable Stew | Vegetables + Broth | 55 | 45 | +5% Stamina Regen (45m) | 0.08/hr | 20 cr |
| Fish Chowder | Fish + Cream + Potato | 65 | 50 | +10% Focus (1h) | 0.10/hr | 40 cr |
| Spicy Hotpot | Meat + Chili + Vegetables | 75 | 60 | +15% Attack Speed (1h) | 0.09/hr | 50 cr |

**Characteristics**: 1-2 day shelf-life, strong buffs, requires cooking pot

#### Preserved Goods

**Purpose**: Long-distance trade, extended storage, survival provisions.

| Item Name | Base Ingredient | Method | Nutrition | Shelf-Life Mult | Base Value |
|-----------|----------------|--------|-----------|-----------------|------------|
| Salted Fish | Fish | Salting | 40 | 5x | 35 cr |
| Smoked Sausage | Meat | Smoking | 50 | 4x | 60 cr |
| Dried Fruit | Fruit | Drying | 25 | 8x | 15 cr |
| Pickled Vegetables | Vegetables | Pickling | 30 | 6x | 25 cr |

**Characteristics**: Multi-week shelf-life, no buffs, premium trade value

#### Luxury Meals

**Purpose**: Major buffs, prestige, special occasions, high-value trade.

| Item Name | Ingredients | Nutrition | Buffs | Spoilage Rate | Base Value |
|-----------|-------------|-----------|-------|---------------|------------|
| Feast Platter | 3 Meats + Wine + Herbs | 100 | +15% All Stats (2h) | 0.05/hr | 200 cr |
| Wine-Stewed Boar | Boar + Aged Wine + Rare Spices | 90 | +20% Strength, +10% Vitality (2h) | 0.06/hr | 250 cr |
| Golden Cake | Rare Flour + Honey + Saffron | 80 | +25% XP Gain (3h) | 0.07/hr | 300 cr |
| Dragon's Breath Soup | Dragon Meat + Volcanic Herbs | 95 | +30% Fire Resist, +15% Attack (2h) | 0.04/hr | 500 cr |

**Characteristics**: 6-12 hour shelf-life, multiple powerful buffs, rare ingredients

## Game Mechanics

### Spoilage System

All perishable items degrade over time using an exponential decay model that accounts for environmental factors.

#### Base Spoilage Model

```python
def calculate_spoilage(item, time_elapsed_hours, environment):
    """
    Calculate spoilage for perishable items.
    
    Args:
        item: Item object with spoilage_rate and preservation properties
        time_elapsed_hours: Time passed since last update
        environment: Environmental conditions (temperature, humidity, season)
    
    Returns:
        Updated freshness value and current market value
    """
    if not item.is_perishable:
        return item.freshness, item.base_value
    
    # Base spoilage rate (per hour)
    base_rate = item.spoilage_rate
    
    # Environmental modifiers
    temp_multiplier = calculate_temperature_effect(environment.temperature)
    humidity_multiplier = 1.0 + (environment.humidity - 50) / 100
    season_multiplier = SEASON_EFFECTS[environment.season].deterioration
    
    # Preservation effects
    preservation_multiplier = 1.0
    if item.preservation_method != "none":
        preservation_multiplier = PRESERVATION_METHODS[item.preservation_method].decay_reduction
    
    # Calculate effective spoilage rate
    effective_rate = (base_rate * 
                     temp_multiplier * 
                     humidity_multiplier * 
                     season_multiplier * 
                     preservation_multiplier)
    
    # Exponential decay model
    new_freshness = item.freshness * math.exp(-effective_rate * time_elapsed_hours)
    new_freshness = max(0, min(100, new_freshness))
    
    # Value degradation (quadratic relationship)
    value_multiplier = (new_freshness / 100.0) ** 2
    current_value = item.base_value * value_multiplier
    
    # Determine spoiled state
    is_spoiled = new_freshness < 10.0
    
    return {
        'freshness': new_freshness,
        'current_value': current_value,
        'is_spoiled': is_spoiled,
        'effective_rate': effective_rate
    }

def calculate_temperature_effect(temperature_celsius):
    """Temperature impact on spoilage rate."""
    # Optimal storage: 0-5°C (multiplier = 0.2)
    # Room temperature: 20°C (multiplier = 1.0)
    # Hot conditions: 35°C+ (multiplier = 3.0+)
    
    if temperature_celsius <= 5:
        return 0.2  # Cold storage
    elif temperature_celsius <= 15:
        return 0.5  # Cool storage
    elif temperature_celsius <= 25:
        return 1.0  # Room temperature
    elif temperature_celsius <= 35:
        return 2.0  # Warm
    else:
        return 3.0 + (temperature_celsius - 35) * 0.1  # Hot
```

#### Spoilage Rates by Category

| Food Category | Base Spoilage Rate | Typical Shelf-Life (20°C) | Notes |
|---------------|-------------------|---------------------------|-------|
| Fresh Fruit | 0.15/hr | 1-2 days | Very perishable |
| Fresh Vegetables | 0.12/hr | 2-3 days | Moderate perishability |
| Raw Meat | 0.20/hr | 12-18 hours | Highly perishable |
| Cooked Meat | 0.12/hr | 2-3 days | Better than raw |
| Bread/Baked | 0.08/hr | 3-4 days | Relatively stable |
| Dairy | 0.15/hr | 1-2 days | Temperature-sensitive |
| Fish | 0.25/hr | 8-12 hours | Extremely perishable |

### Preservation Methods

Players can extend food shelf-life through various preservation techniques.

#### Available Methods

```python
PRESERVATION_METHODS = {
    "salting": {
        "decay_reduction": 0.20,  # 80% slower decay
        "shelf_life_multiplier": 5.0,
        "required_material": "salt",
        "material_quantity": 1,
        "processing_time": 2.0,  # hours
        "skill_requirement": "cooking",
        "skill_level": 5,
        "nutrition_retention": 0.80,  # 80% of original
        "applicable_to": ["meat", "fish", "vegetables"]
    },
    "smoking": {
        "decay_reduction": 0.25,  # 75% slower decay
        "shelf_life_multiplier": 4.0,
        "required_material": "wood_chips",
        "material_quantity": 3,
        "processing_time": 4.0,
        "skill_requirement": "cooking",
        "skill_level": 10,
        "nutrition_retention": 0.75,
        "applicable_to": ["meat", "fish"]
    },
    "drying": {
        "decay_reduction": 0.10,  # 90% slower decay
        "shelf_life_multiplier": 8.0,
        "required_material": "none",
        "material_quantity": 0,
        "processing_time": 24.0,  # 1 day
        "skill_requirement": "cooking",
        "skill_level": 3,
        "nutrition_retention": 0.70,
        "applicable_to": ["fruit", "vegetables", "meat"]
    },
    "pickling": {
        "decay_reduction": 0.15,  # 85% slower decay
        "shelf_life_multiplier": 6.0,
        "required_material": "vinegar",
        "material_quantity": 1,
        "processing_time": 8.0,
        "skill_requirement": "cooking",
        "skill_level": 8,
        "nutrition_retention": 0.85,
        "applicable_to": ["vegetables", "eggs"]
    },
    "freezing": {
        "decay_reduction": 0.05,  # 95% slower decay
        "shelf_life_multiplier": 20.0,
        "required_material": "ice_block",
        "material_quantity": 2,
        "processing_time": 0.5,
        "skill_requirement": "none",
        "skill_level": 0,
        "nutrition_retention": 0.95,
        "applicable_to": ["all"]
    }
}
```

#### Preservation Application

```python
def apply_preservation(item, method_name, player_skill):
    """
    Apply preservation method to a food item.
    
    Args:
        item: Food item to preserve
        method_name: Name of preservation method
        player_skill: Player's relevant skill level
    
    Returns:
        Success status and modified item
    """
    method = PRESERVATION_METHODS.get(method_name)
    if not method:
        return {"success": False, "error": "Unknown preservation method"}
    
    # Check if method is applicable to this food type
    if item.food_type not in method["applicable_to"] and "all" not in method["applicable_to"]:
        return {"success": False, "error": "Method not applicable to this food type"}
    
    # Check skill requirement
    if player_skill < method["skill_level"]:
        return {"success": False, "error": "Insufficient skill level"}
    
    # Check materials
    if method["required_material"] != "none":
        if not player.has_item(method["required_material"], method["material_quantity"]):
            return {"success": False, "error": "Missing required materials"}
        player.consume_item(method["required_material"], method["material_quantity"])
    
    # Apply preservation
    item.preservation_method = method_name
    item.preservation_multiplier = method["decay_reduction"]
    item.nutrition_value *= method["nutrition_retention"]
    item.base_value *= 1.5  # Preserved goods have premium value
    
    # Schedule completion
    completion_time = time.now() + timedelta(hours=method["processing_time"])
    
    return {
        "success": True,
        "item": item,
        "completion_time": completion_time
    }
```

### Buff System

Consumable dishes can grant temporary buffs to player stats.

#### Buff Structure

```json
{
  "buff_id": "unique_buff_identifier",
  "buff_name": "Display Name",
  "buff_type": "positive|negative",
  "duration_seconds": 3600,
  "stat_modifiers": [
    {
      "stat_name": "strength",
      "modifier_type": "percentage|flat",
      "value": 10.0
    }
  ],
  "stacks": false,
  "max_stacks": 1,
  "icon_reference": "asset_path",
  "description": "Buff effect description"
}
```

#### Common Buffs

```python
DISH_BUFFS = {
    "well_fed": {
        "buff_id": "buff_wellfed_001",
        "duration": 1800,  # 30 minutes
        "modifiers": [
            {"stat": "health_regen", "type": "percentage", "value": 5.0}
        ],
        "description": "Feeling satisfied and healthy"
    },
    "strength_boost": {
        "buff_id": "buff_strength_001",
        "duration": 1800,
        "modifiers": [
            {"stat": "strength", "type": "percentage", "value": 10.0}
        ],
        "description": "Increased physical power"
    },
    "stamina_boost": {
        "buff_id": "buff_stamina_001",
        "duration": 2700,  # 45 minutes
        "modifiers": [
            {"stat": "stamina_regen", "type": "percentage", "value": 10.0}
        ],
        "description": "Enhanced endurance"
    },
    "focus": {
        "buff_id": "buff_focus_001",
        "duration": 3600,  # 1 hour
        "modifiers": [
            {"stat": "crafting_speed", "type": "percentage", "value": 5.0},
            {"stat": "gathering_speed", "type": "percentage", "value": 5.0}
        ],
        "description": "Heightened concentration"
    },
    "feast_blessing": {
        "buff_id": "buff_feast_001",
        "duration": 7200,  # 2 hours
        "modifiers": [
            {"stat": "strength", "type": "percentage", "value": 15.0},
            {"stat": "vitality", "type": "percentage", "value": 15.0},
            {"stat": "dexterity", "type": "percentage", "value": 15.0}
        ],
        "description": "Blessed by a magnificent feast"
    }
}
```

#### Buff Application

```python
def apply_buff(player, dish):
    """
    Apply buffs from consumed dish to player.
    
    Args:
        player: Player object
        dish: Dish item being consumed
    
    Returns:
        List of applied buffs
    """
    applied_buffs = []
    
    for buff_id in dish.buffs:
        buff_data = DISH_BUFFS.get(buff_id)
        if not buff_data:
            continue
        
        # Check if buff already active
        existing_buff = player.get_active_buff(buff_id)
        
        if existing_buff:
            if buff_data.get("stacks", False):
                # Stack the buff if allowed
                current_stacks = existing_buff.stack_count
                max_stacks = buff_data.get("max_stacks", 1)
                
                if current_stacks < max_stacks:
                    existing_buff.stack_count += 1
                    existing_buff.refresh_duration()
                else:
                    # At max stacks, just refresh duration
                    existing_buff.refresh_duration()
            else:
                # Replace existing buff (refresh)
                existing_buff.refresh_duration()
        else:
            # Apply new buff
            new_buff = Buff(
                buff_id=buff_data["buff_id"],
                duration=buff_data["duration"],
                modifiers=buff_data["modifiers"],
                stack_count=1
            )
            player.add_buff(new_buff)
            applied_buffs.append(new_buff)
    
    return applied_buffs
```

### Crafting Integration

Materials flow into dishes through a multi-tier crafting system.

#### Crafting Flow Example

```text
Raw Materials → Processed Materials → Intermediate Components → Final Dish

Example: Feast Platter
1. Hunt animals → Raw Meat (×3 types)
2. Process meat → Butchered Meat (cutting)
3. Gather grapes → Grapes
4. Ferment grapes → Wine (time-based)
5. Gather herbs → Herbs (×2 types)
6. Combine all → Feast Platter (cooking station)
```

#### Recipe Structure

```json
{
  "recipe_id": "recipe_feast_platter_001",
  "recipe_name": "Feast Platter",
  "category": "cooking",
  "subcategory": "luxury_meal",
  "output": {
    "item_id": "food_luxury_0001",
    "quantity": 1,
    "quality_range": [0.8, 1.2]
  },
  "inputs": [
    {"item_id": "material_meat_beef", "quantity": 1},
    {"item_id": "material_meat_pork", "quantity": 1},
    {"item_id": "material_meat_chicken", "quantity": 1},
    {"item_id": "material_wine_aged", "quantity": 1},
    {"item_id": "material_herb_rare", "quantity": 2}
  ],
  "requirements": {
    "skill": "cooking",
    "skill_level": 50,
    "station": "grand_oven",
    "time_minutes": 30
  },
  "unlock_condition": "recipe_book_luxury_cooking"
}
```

### Trade Value System

Item values are dynamic based on supply, demand, and market conditions.

#### Value Calculation

```python
def calculate_market_value(item, market_data, player_location):
    """
    Calculate current market value of an item.
    
    Args:
        item: Item object with base_value
        market_data: Current market supply/demand
        player_location: Player's current location
    
    Returns:
        Current market price
    """
    base_value = item.base_value
    
    # Freshness modifier for perishables
    freshness_multiplier = 1.0
    if item.is_perishable:
        freshness_multiplier = (item.freshness / 100.0) ** 2
    
    # Supply/Demand modifier
    supply_demand_ratio = market_data.supply / max(market_data.demand, 1)
    if supply_demand_ratio < 0.5:
        supply_demand_mult = 1.5  # High demand, low supply
    elif supply_demand_ratio < 1.0:
        supply_demand_mult = 1.2  # Moderate demand
    elif supply_demand_ratio < 2.0:
        supply_demand_mult = 1.0  # Balanced
    else:
        supply_demand_mult = 0.7  # Oversupply
    
    # Regional scarcity modifier
    regional_multiplier = calculate_regional_scarcity(item, player_location)
    
    # Rarity modifier
    rarity_multipliers = {
        "common": 1.0,
        "uncommon": 1.5,
        "rare": 2.5,
        "epic": 5.0,
        "legendary": 10.0,
        "mythic": 20.0
    }
    rarity_mult = rarity_multipliers.get(item.rarity, 1.0)
    
    # Calculate final value
    final_value = (base_value * 
                   freshness_multiplier * 
                   supply_demand_mult * 
                   regional_multiplier * 
                   rarity_mult)
    
    return round(final_value, 2)
```

## Data Model

### Database Schema

#### Items Master Table

```sql
CREATE TABLE items_master (
    item_id VARCHAR(50) PRIMARY KEY,
    item_name VARCHAR(100) NOT NULL,
    item_type VARCHAR(50) NOT NULL,
    category VARCHAR(50) NOT NULL,
    subcategory VARCHAR(50),
    rarity VARCHAR(20) NOT NULL,
    description TEXT,
    base_value DECIMAL(10,2) DEFAULT 0,
    weight DECIMAL(10,3) DEFAULT 0,
    volume DECIMAL(10,3) DEFAULT 0,
    stack_size INT DEFAULT 1,
    is_tradeable BOOLEAN DEFAULT TRUE,
    is_consumable BOOLEAN DEFAULT FALSE,
    required_level INT DEFAULT 1,
    icon_url VARCHAR(500),
    tags JSONB,
    created_at TIMESTAMP DEFAULT NOW(),
    
    CONSTRAINT valid_rarity CHECK (rarity IN ('common', 'uncommon', 'rare', 'epic', 'legendary', 'mythic')),
    CONSTRAINT valid_type CHECK (item_type IN ('material', 'food', 'consumable'))
);

CREATE INDEX idx_items_type ON items_master(item_type);
CREATE INDEX idx_items_category ON items_master(category);
CREATE INDEX idx_items_rarity ON items_master(rarity);
CREATE INDEX idx_items_name ON items_master(item_name);
```

#### Materials Table

```sql
CREATE TABLE materials (
    item_id VARCHAR(50) PRIMARY KEY REFERENCES items_master(item_id),
    crafting_tier INT NOT NULL DEFAULT 1,
    durability_value INT DEFAULT 0,
    quality_modifier DECIMAL(5,2) DEFAULT 1.0,
    elemental_affinity VARCHAR(20) DEFAULT 'none',
    tool_requirement VARCHAR(50),
    yield_multiplier DECIMAL(5,2) DEFAULT 1.0,
    refinement_stages INT DEFAULT 1,
    properties JSONB,
    
    CONSTRAINT valid_tier CHECK (crafting_tier >= 1 AND crafting_tier <= 10),
    CONSTRAINT valid_affinity CHECK (elemental_affinity IN ('none', 'fire', 'frost', 'poison', 'arcane', 'physical'))
);

CREATE INDEX idx_materials_tier ON materials(crafting_tier);
CREATE INDEX idx_materials_affinity ON materials(elemental_affinity);
```

#### Consumables Table

```sql
CREATE TABLE consumables (
    item_id VARCHAR(50) PRIMARY KEY REFERENCES items_master(item_id),
    nutrition_value INT DEFAULT 0,
    energy_restoration INT DEFAULT 0,
    health_restoration INT DEFAULT 0,
    spoilage_rate DECIMAL(6,4) DEFAULT 0,
    base_freshness DECIMAL(5,2) DEFAULT 100.0,
    preservation_method VARCHAR(20) DEFAULT 'none',
    preservation_multiplier DECIMAL(5,2) DEFAULT 1.0,
    consumption_time DECIMAL(5,2) DEFAULT 3.0,
    satiety_value INT DEFAULT 0,
    food_type VARCHAR(50),
    properties JSONB,
    
    CONSTRAINT valid_freshness CHECK (base_freshness >= 0 AND base_freshness <= 100),
    CONSTRAINT valid_preservation CHECK (preservation_method IN ('none', 'salted', 'smoked', 'dried', 'pickled', 'frozen'))
);

CREATE INDEX idx_consumables_type ON consumables(food_type);
CREATE INDEX idx_consumables_spoilage ON consumables(spoilage_rate);
```

#### Buffs Table

```sql
CREATE TABLE buffs (
    buff_id VARCHAR(50) PRIMARY KEY,
    buff_name VARCHAR(100) NOT NULL,
    buff_type VARCHAR(20) DEFAULT 'positive',
    duration_seconds INT NOT NULL,
    stacks BOOLEAN DEFAULT FALSE,
    max_stacks INT DEFAULT 1,
    icon_url VARCHAR(500),
    description TEXT,
    stat_modifiers JSONB NOT NULL,
    created_at TIMESTAMP DEFAULT NOW(),
    
    CONSTRAINT valid_buff_type CHECK (buff_type IN ('positive', 'negative'))
);

CREATE INDEX idx_buffs_type ON buffs(buff_type);
```

#### Item Buffs Association Table

```sql
CREATE TABLE item_buffs (
    item_id VARCHAR(50) REFERENCES items_master(item_id),
    buff_id VARCHAR(50) REFERENCES buffs(buff_id),
    PRIMARY KEY (item_id, buff_id)
);

CREATE INDEX idx_item_buffs_item ON item_buffs(item_id);
CREATE INDEX idx_item_buffs_buff ON item_buffs(buff_id);
```

#### Recipes Table

```sql
CREATE TABLE recipes (
    recipe_id VARCHAR(50) PRIMARY KEY,
    recipe_name VARCHAR(100) NOT NULL,
    category VARCHAR(50) NOT NULL,
    subcategory VARCHAR(50),
    output_item_id VARCHAR(50) REFERENCES items_master(item_id),
    output_quantity INT DEFAULT 1,
    quality_min DECIMAL(5,2) DEFAULT 1.0,
    quality_max DECIMAL(5,2) DEFAULT 1.0,
    required_skill VARCHAR(50),
    skill_level INT DEFAULT 1,
    crafting_station VARCHAR(50),
    crafting_time_minutes INT DEFAULT 5,
    unlock_condition VARCHAR(100),
    created_at TIMESTAMP DEFAULT NOW()
);

CREATE INDEX idx_recipes_category ON recipes(category);
CREATE INDEX idx_recipes_output ON recipes(output_item_id);
CREATE INDEX idx_recipes_skill ON recipes(required_skill, skill_level);
```

#### Recipe Inputs Table

```sql
CREATE TABLE recipe_inputs (
    recipe_id VARCHAR(50) REFERENCES recipes(recipe_id),
    input_item_id VARCHAR(50) REFERENCES items_master(item_id),
    quantity INT NOT NULL DEFAULT 1,
    is_optional BOOLEAN DEFAULT FALSE,
    PRIMARY KEY (recipe_id, input_item_id)
);

CREATE INDEX idx_recipe_inputs_recipe ON recipe_inputs(recipe_id);
CREATE INDEX idx_recipe_inputs_item ON recipe_inputs(input_item_id);
```

#### Player Inventory Items Table

```sql
CREATE TABLE player_inventory_items (
    inventory_slot_id BIGSERIAL PRIMARY KEY,
    player_id BIGINT NOT NULL,
    item_id VARCHAR(50) REFERENCES items_master(item_id),
    quantity INT NOT NULL DEFAULT 1,
    current_freshness DECIMAL(5,2) DEFAULT 100.0,
    preservation_applied VARCHAR(20) DEFAULT 'none',
    acquired_at TIMESTAMP DEFAULT NOW(),
    last_updated TIMESTAMP DEFAULT NOW(),
    properties JSONB,
    
    CONSTRAINT positive_quantity CHECK (quantity > 0),
    CONSTRAINT valid_freshness CHECK (current_freshness >= 0 AND current_freshness <= 100)
);

CREATE INDEX idx_player_inv_player ON player_inventory_items(player_id);
CREATE INDEX idx_player_inv_item ON player_inventory_items(item_id);
CREATE INDEX idx_player_inv_freshness ON player_inventory_items(current_freshness) 
    WHERE current_freshness < 100;
```

## Balancing Rules

### Core Principles

1. **Accessibility**: Common materials and staple foods are abundant and affordable
2. **Progression**: Higher-tier materials require skill investment and exploration
3. **Trade-offs**: Preservation extends shelf-life but reduces nutritional value
4. **Scarcity**: Rare materials maintain value through limited availability
5. **Risk/Reward**: Luxury dishes provide powerful buffs but require rare ingredients

### Value Balancing

#### Material Value Tiers

| Tier | Base Value Range | Examples | Availability |
|------|------------------|----------|--------------|
| 1 | 2-15 cr | Copper Ore, Oak Log, Flax | Very High |
| 2 | 15-50 cr | Iron Ore, Leather, Linen | High |
| 3 | 50-150 cr | Silver Ore, Steel, Herb Extract | Moderate |
| 4 | 150-300 cr | Gold Ore, Yew Wood, Rare Herbs | Low |
| 5 | 300-800 cr | Ruby, Sapphire, Emerald | Very Low |
| 6-10 | 800-3000+ cr | Mythril, Diamond, Arcane Crystal | Extremely Rare |

#### Food Value Tiers

| Tier | Base Value Range | Examples | Complexity |
|------|------------------|----------|------------|
| Basic | 2-10 cr | Bread, Berries, Boiled Egg | 1 ingredient |
| Simple | 10-30 cr | Roasted Fish, Omelet | 2-3 ingredients |
| Moderate | 30-80 cr | Stews, Pies | 4-5 ingredients |
| Preserved | 20-100 cr | Salted Fish, Dried Fruit | Base + preservation |
| Luxury | 200-500+ cr | Feast Platter, Dragon Soup | Rare + complex |

### Spoilage Balancing

**Design Goals:**

- Fresh foods spoil in 1-3 days at room temperature
- Preservation extends life by 3-8x depending on method
- Cold storage provides universal benefit
- Spoilage creates meaningful trade decisions

**Tuning Parameters:**

```python
SPOILAGE_TUNING = {
    "base_rates": {
        "very_perishable": 0.20,  # Fish, raw meat (12-18 hours)
        "perishable": 0.15,       # Dairy, fresh fruit (1-2 days)
        "moderate": 0.10,         # Cooked food, vegetables (2-3 days)
        "stable": 0.05,           # Bread, dried goods (4-6 days)
        "preserved": 0.02         # Preserved items (weeks)
    },
    "environmental_impact": {
        "cold_storage_mult": 0.2,
        "hot_weather_mult": 2.5,
        "winter_mult": 0.5,
        "summer_mult": 1.3
    }
}
```

### Buff Balancing

**Design Goals:**

- Simple foods provide no buffs (basic sustenance only)
- Moderate dishes give 5-10% single-stat buffs for 30-60 minutes
- Complex dishes give 10-15% multi-stat buffs for 1-2 hours
- Luxury dishes give 15-30% powerful buffs for 2-3 hours

**Stacking Rules:**

- Buffs of the same type do NOT stack (highest takes precedence)
- Different buff types CAN stack (e.g., strength + stamina)
- Luxury feast buffs override simpler food buffs

## Integration Points

### Crafting System Integration

The dishes & materials system integrates with the core crafting mechanics:

**References:**

- [Crafting Mechanics Overview](./crafting-mechanics-overview.md)
- [Crafting Quality Model](./crafting-quality-model.md)
- [Crafting Success Model](./crafting-success-model.md)

**Integration:**

- Material quality affects crafted item quality
- Skill levels gate recipe access and success rates
- Crafting stations required for higher-tier items
- Experience gained from processing and cooking

### Economy System Integration

Materials and foods drive the player economy:

**References:**

- [Trade System](./trade-system.md)
- [Economy Systems](../../systems/economy-systems.md)

**Integration:**

- Dynamic pricing based on supply/demand
- Regional scarcity affects material values
- Spoilage creates time pressure in trading
- Preserved goods enable long-distance trade

### Database Schema Integration

Items use the existing database infrastructure:

**References:**

- [Database Schema Design](../../systems/database-schema-design.md)

**Integration:**

- Items master table for all item types
- Inventory system tracks freshness and preservation
- Recipe system links materials to outputs
- Market system tracks prices and transactions

## Testing & Validation

### Acceptance Criteria

- [x] All dishes have defined nutrition, spoilage rates, and preservation methods
- [x] All materials have defined rarity and crafting tier assignments
- [x] Preserved foods last 3x-8x longer than fresh equivalents
- [x] Luxury meals require multiple rare inputs from different categories
- [x] Buff effects from dishes follow stacking rules (no infinite stacking)
- [x] Spoilage system uses exponential decay model
- [x] Value calculation accounts for freshness, supply/demand, and rarity
- [x] Recipe system links materials to consumables through crafting flow

### Developer Test Scenarios

#### Scenario 1: Basic Spoilage

```python
def test_fresh_fish_spoilage():
    """Test that fresh fish spoils correctly over time."""
    fish = create_item("material_fish_raw")
    assert fish.spoilage_rate == 0.25
    assert fish.freshness == 100.0
    
    # Simulate 12 hours at room temperature (20°C)
    environment = Environment(temperature=20, humidity=50, season="summer")
    result = calculate_spoilage(fish, time_elapsed_hours=12, environment=environment)
    
    # Expected: significant decay after 12 hours
    assert result['freshness'] < 30.0
    assert result['is_spoiled'] == True
    assert result['current_value'] < fish.base_value * 0.5
```

#### Scenario 2: Preservation Effect

```python
def test_salted_fish_preservation():
    """Test that salted fish lasts significantly longer."""
    fish = create_item("material_fish_raw")
    player = create_test_player(cooking_skill=10)
    
    # Apply salting preservation
    result = apply_preservation(fish, "salting", player.cooking_skill)
    assert result['success'] == True
    
    # Salting should give 5x shelf-life (0.20 decay reduction)
    assert fish.preservation_multiplier == 0.20
    
    # Simulate same 12 hours
    environment = Environment(temperature=20, humidity=50, season="summer")
    result = calculate_spoilage(fish, time_elapsed_hours=12, environment=environment)
    
    # Should still be relatively fresh
    assert result['freshness'] > 70.0
    assert result['is_spoiled'] == False
```

#### Scenario 3: Crafting Flow

```python
def test_bread_crafting_flow():
    """Test complete crafting flow from wheat to bread."""
    player = create_test_player(farming_skill=5, cooking_skill=5)
    
    # Step 1: Harvest wheat
    wheat = player.harvest("crop_wheat", quantity=10)
    assert wheat.item_id == "material_wheat"
    assert wheat.quantity == 10
    
    # Step 2: Process wheat to flour
    flour_recipe = get_recipe("recipe_flour_from_wheat")
    flour = player.craft(flour_recipe, quantity=5)
    assert flour.item_id == "material_flour"
    assert flour.quantity == 5
    assert wheat.quantity == 0  # All consumed
    
    # Step 3: Bake bread
    bread_recipe = get_recipe("recipe_bread_simple")
    bread = player.craft(bread_recipe, quantity=5)
    assert bread.item_id == "food_staple_bread"
    assert bread.quantity == 5
    assert bread.nutrition_value == 30
    assert bread.spoilage_rate == 0.08
```

#### Scenario 4: Buff Application

```python
def test_stew_buff_application():
    """Test that eating stew applies correct buff."""
    player = create_test_player()
    stew = create_item("food_soup_stew_vegetable")
    
    # Consume the stew
    initial_health = player.stats.health
    player.consume(stew)
    
    # Check health restoration
    assert player.stats.health == initial_health + stew.health_restoration
    
    # Check buff application
    active_buffs = player.get_active_buffs()
    assert len(active_buffs) == 1
    assert active_buffs[0].buff_id == "buff_stamina_001"
    assert active_buffs[0].duration == 2700  # 45 minutes
    
    # Verify stat modification
    base_stamina_regen = player.base_stats.stamina_regen
    current_stamina_regen = player.stats.stamina_regen
    assert current_stamina_regen == base_stamina_regen * 1.10  # +10%
```

#### Scenario 5: Luxury Recipe Requirements

```python
def test_feast_platter_requirements():
    """Test that feast platter requires rare inputs from multiple categories."""
    recipe = get_recipe("recipe_feast_platter_001")
    
    # Verify multiple rare inputs
    inputs = recipe.inputs
    assert len(inputs) >= 5
    
    # Check for diversity (different categories)
    categories = set([get_item(inp.item_id).category for inp in inputs])
    assert len(categories) >= 3  # Meat, beverage, herbs minimum
    
    # Check for rare ingredients
    rarities = [get_item(inp.item_id).rarity for inp in inputs]
    assert "rare" in rarities or "uncommon" in rarities
    
    # Verify high skill requirement
    assert recipe.skill_level >= 40
```

#### Scenario 6: Market Value Dynamics

```python
def test_market_value_calculation():
    """Test dynamic market value calculation."""
    item = create_item("material_iron_ore")
    base_value = item.base_value
    
    # Test with high demand, low supply
    market_data = MarketData(supply=10, demand=100)
    location = Location(region="mountain_area")
    
    value_high_demand = calculate_market_value(item, market_data, location)
    assert value_high_demand > base_value  # Should be premium
    
    # Test with oversupply
    market_data = MarketData(supply=1000, demand=100)
    value_oversupply = calculate_market_value(item, market_data, location)
    assert value_oversupply < base_value  # Should be discounted
```

### Integration Testing

```python
def test_end_to_end_food_lifecycle():
    """Complete lifecycle from gathering to consumption."""
    player = create_test_player()
    
    # 1. Gather ingredients
    fish = player.fish(location="river", quantity=1)
    assert fish.freshness == 100.0
    
    # 2. Cook the fish
    recipe = get_recipe("recipe_roasted_fish")
    cooked_fish = player.craft(recipe)
    assert cooked_fish.nutrition_value > fish.nutrition_value
    
    # 3. Wait 6 hours (simulate time)
    advance_time(hours=6)
    update_inventory_spoilage(player)
    
    # 4. Check spoilage
    updated_fish = player.get_inventory_item(cooked_fish.inventory_slot_id)
    assert updated_fish.freshness < 100.0
    assert updated_fish.freshness > 50.0  # Should still be edible
    
    # 5. Consume the fish
    initial_health = player.stats.health
    player.consume(updated_fish)
    assert player.stats.health > initial_health
    
    # 6. Verify item removed from inventory
    assert player.get_inventory_item(cooked_fish.inventory_slot_id) is None
```

## Implementation Guidelines

### Phase 1: Core Items (Week 1-2)

**Objective**: Implement base set of materials and simple foods.

**Tasks:**

- [ ] Create database schema for items, materials, consumables
- [ ] Implement 20 raw materials across categories
- [ ] Implement 15 simple foods and staples
- [ ] Basic spoilage system (linear decay)
- [ ] Simple crafting recipes (raw → processed)

**Deliverables:**

- Working database with seed data
- Basic item creation and retrieval API
- Simple spoilage calculation

### Phase 2: Preservation & Buffs (Week 3-4)

**Objective**: Add preservation methods and buff system.

**Tasks:**

- [ ] Implement 5 preservation methods
- [ ] Create buff system with stat modifiers
- [ ] Exponential spoilage decay model
- [ ] Environmental factors (temperature, season)
- [ ] 10 composite dishes with buffs

**Deliverables:**

- Preservation application system
- Buff application and tracking
- Enhanced spoilage calculations

### Phase 3: Advanced Materials (Week 5-6)

**Objective**: Expand material variety and crafting tiers.

**Tasks:**

- [ ] Implement 30 additional materials (tiers 3-6)
- [ ] Rare materials with special properties
- [ ] Multi-stage processing recipes
- [ ] Quality inheritance system
- [ ] Elemental affinity mechanics

**Deliverables:**

- Complete material catalog
- Advanced crafting recipes
- Quality system integration

### Phase 4: Luxury Items & Balance (Week 7-8)

**Objective**: Add luxury dishes and balance economy.

**Tasks:**

- [ ] Implement 10 luxury dishes
- [ ] Dynamic market value system
- [ ] Regional scarcity modifiers
- [ ] Complete buff library
- [ ] Balance testing and tuning

**Deliverables:**

- Full item ecosystem
- Balanced economy
- Comprehensive testing suite

### Tuning Knobs

These parameters can be adjusted to balance gameplay:

```python
TUNING_CONFIG = {
    "spoilage": {
        "base_rate_multiplier": 1.0,  # Global spoilage speed
        "preservation_effectiveness": 1.0,  # How much preservation helps
        "environmental_impact": 1.0  # How much environment affects spoilage
    },
    "crafting": {
        "material_consumption": 1.0,  # How many materials needed
        "processing_time": 1.0,  # How long crafting takes
        "quality_variance": 1.0  # How much quality varies
    },
    "buffs": {
        "duration_multiplier": 1.0,  # How long buffs last
        "strength_multiplier": 1.0,  # How strong buffs are
        "stack_limit": 1  # Maximum buff stacks
    },
    "economy": {
        "base_value_multiplier": 1.0,  # Global price adjustment
        "supply_demand_impact": 1.0,  # How much market affects prices
        "regional_variation": 1.0  # Regional price differences
    }
}
```

### Performance Considerations

**Spoilage Updates:**

- Run spoilage calculations on inventory load, not real-time tick
- Batch update all perishable items in player inventory
- Cache environmental conditions per region
- Use database triggers for automatic freshness decay

**Market Calculations:**

- Cache market data per region (update hourly)
- Pre-calculate supply/demand ratios
- Use materialized views for popular items
- Implement regional market caches

**Recipe Queries:**

- Index recipes by required skill and tier
- Cache crafting station requirements
- Pre-load common recipes on player login

## Related Documentation

### Core Systems

- [Crafting Mechanics Overview](./crafting-mechanics-overview.md)
- [Crafting Quality Model](./crafting-quality-model.md)
- [Crafting Success Model](./crafting-success-model.md)
- [Trade System](./trade-system.md)

### Technical Specifications

- [Database Schema Design](../../systems/database-schema-design.md)
- [Economy Systems](../../systems/economy-systems.md)
- [API Specifications](../../systems/api-specifications.md)

### Research & References

- [Material Systems Research](../../../research/game-design/step-2-system-research/step-2.2-material-systems/)
- [Crafting Systems Research](../../../research/game-design/step-2-system-research/step-2.3-crafting-systems/)
- [Game Economy Design](../../../research/sources/game-economy-design-external.md)
- [Fruit Conservation Research](../../../research/topics/fruit-conservation-decay-research.md)

## Change Log

| Version | Date | Author | Changes |
|---------|------|--------|---------|
| 1.0 | 2025-01-06 | Design Team | Initial specification |

## Approval

**Status**: Pending Review

**Reviewers:**

- [ ] Lead Game Designer
- [ ] Economy Designer
- [ ] Technical Lead
- [ ] Database Architect

**Approval Date**: TBD
