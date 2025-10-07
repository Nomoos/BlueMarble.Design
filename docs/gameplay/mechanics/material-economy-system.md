# Material Economy System Design

**Document Type:** System Specification  
**Version:** 1.0  
**Author:** BlueMarble Economy Team  
**Date:** 2025-01-22  
**Status:** Proposed

## Purpose & Scope

This document defines a **functional material economy** where resources flow through realistic pipelines.
The goal is a *player-driven* economy that balances supply and demand while encouraging strategic choices.
We model resources as part of a **supply chain/value chain**: raw inputs are gathered (sources/faucets),
processed through crafting, and ultimately consumed or destroyed at end-use sinks.

The economy should feel sustainable and engaging: players' activities determine prices, multiple gathering
methods create healthy competition, and meaningful sinks prevent inflation while maintaining item value.

## Material Categories

Define broad resource **categories** and archetypal examples (each category spans many specific items):

### Metals

Common ores (iron, copper) and refined metals (steel, bronze, silver) used for tools and weapons.

**Examples:**

- **Iron Ore** → **Steel Ingots** → **Weapons, Armor, Tools**
- **Copper Ore** → **Copper Bars** → **Electrical Components, Coins**
- **Silver Ore** → **Silver Bars** → **Jewelry, High-Value Currency**
- **Gold Ore** → **Gold Bars** → **Luxury Items, Currency**
- **Rare Ores** (Mithril, Adamantite) → **Legendary Equipment**

**Economic Characteristics:**

- High value-to-weight ratio for refined metals
- Processing adds significant value (ore → ingot → component)
- Used in virtually all equipment crafting
- Rare metals drive end-game economy

### Timber & Wood

Logs and planks from trees (oak, pine, mahogany) for building and fuel.

**Examples:**

- **Oak Logs** → **Oak Planks** → **Furniture, Buildings**
- **Pine Logs** → **Pine Planks** → **Construction, Scaffolding**
- **Mahogany Logs** → **Mahogany Planks** → **Luxury Furniture, Ships**
- **Bamboo** → **Bamboo Poles** → **Scaffolding, Crafts**
- **Rare Woods** (Ebony, Ironwood) → **High-End Crafting**

**Economic Characteristics:**

- Bulky and low value-to-weight ratio
- Essential for construction and early-game crafting
- Different tree types from different biomes
- Processing doubles value (logs → planks)

### Fibers & Cloth

Plant or animal fibers (flax, cotton, wool, silk) spun into cloth or rope.

**Examples:**

- **Flax** → **Linen Thread** → **Linen Cloth** → **Basic Clothing**
- **Cotton** → **Cotton Thread** → **Cotton Fabric** → **Medium-Quality Clothing**
- **Wool** (from sheep) → **Woolen Cloth** → **Warm Clothing, Blankets**
- **Silk** (from silkworms) → **Silk Fabric** → **Luxury Clothing**
- **Hemp** → **Rope, Canvas** → **Sails, Bags**

**Economic Characteristics:**

- Multi-stage processing adds value at each step
- Essential for clothing and light armor
- Quality variations affect end product significantly
- Renewable through farming

### Food & Agriculture

Crops (wheat, vegetables), livestock (meat, eggs, milk) and fish for eating and cooking.

**Examples:**

- **Wheat** → **Flour** → **Bread, Pastries**
- **Vegetables** (carrots, potatoes) → **Cooked Meals, Soups**
- **Livestock** (cattle, pigs) → **Meat, Leather** → **Food, Armor**
- **Poultry** → **Eggs, Feathers, Meat**
- **Fish** → **Raw Fish** → **Cooked Fish, Fish Oil**
- **Milk** → **Cheese, Butter**

**Economic Characteristics:**

- High turnover due to consumption
- Perishable - creates time pressure
- Essential for survival mechanics
- Farming creates stable supply

### Stone & Minerals

Building stone (granite, marble), sand, clay and gems used in construction and decoration.

**Examples:**

- **Granite** → **Stone Blocks** → **Foundations, Walls**
- **Marble** → **Marble Blocks** → **Decorative Buildings, Statues**
- **Limestone** → **Building Blocks, Cement**
- **Sandstone** → **Desert Construction**
- **Clay** → **Bricks, Pottery, Ceramics**
- **Sand** → **Glass** → **Windows, Bottles**
- **Gems** (Ruby, Sapphire, Diamond) → **Jewelry, Enchanting**

**Economic Characteristics:**

- Very heavy, expensive to transport
- Essential for large-scale construction
- Gems are high-value, low-weight
- Regional variations in quality and availability

### Alchemy/Reagents

Herbal and magical ingredients (herbs, monster drops, ores like sulfur) used in potions, alchemy, enchantments.

**Examples:**

- **Herbs** (Lavender, Sage, Wolfsbane) → **Basic Potions**
- **Mushrooms** (various types) → **Poisons, Antidotes**
- **Monster Drops** (scales, fangs, eyes) → **Powerful Potions**
- **Magical Crystals** → **Enchantments, Spells**
- **Sulfur, Saltpeter** → **Explosives, Advanced Alchemy**
- **Blood, Essences** → **Dark Magic, Powerful Enchantments**

**Economic Characteristics:**

- Biome-specific availability
- Many single-use consumables
- High value for rare ingredients
- Complex recipes require multiple components

### Crafted Goods

Manufactured items (tools, weapons, armor, clothing, furniture, potions) that combine multiple raw inputs.

**Examples:**

- **Iron Tools** (pickaxe, axe, hammer) → **Resource Gathering**
- **Iron Weapons** (sword, spear, bow) → **Combat**
- **Iron Armor** (helmet, chestplate, greaves) → **Protection**
- **Furniture** (tables, chairs, beds) → **Housing, Comfort**
- **Potions** (healing, mana, buffs) → **Consumable Bonuses**
- **Enchanted Items** → **Magical Equipment**

**Economic Characteristics:**

- Combines multiple resource types
- Value exceeds sum of components (crafter profit margin)
- Quality variations create market tiers
- Degradation creates ongoing demand

## Sources

Identify how each category enters the economy:

### Mining & Quarrying

Extract ores, gems, stone from nodes. High-quality nodes yield rarer or higher-grade materials.
Higher skill/tools boost yield.

**Mechanics:**

- **Resource Nodes:** Fixed spawn points with respawn timers
- **Node Quality:** Varies by location and randomness
  - Common nodes: Iron, Copper, Tin (60% spawn rate)
  - Uncommon nodes: Silver, Coal (30% spawn rate)
  - Rare nodes: Gold, Gems (10% spawn rate)
- **Skill Impact:** Higher skill = more yield per node
  - Base yield: 1-3 units
  - +1% yield per 10 skill levels
  - Critical success chance (5%) yields double
- **Tool Quality:** Better tools = faster extraction, higher yield
- **Geographic Distribution:** Mountain regions have higher metal density

**Economic Balance:**

- Node respawn: 30-60 minutes depending on rarity
- Daily gathering cap per player: Prevents farming exploitation
- Depleted areas require exploration for new nodes

### Logging

Cut down trees for wood. Different forests yield different tree types; lumber mills or saws
can further process logs. Geographic diversity (mountains vs plains) yields different wood qualities.

**Mechanics:**

- **Tree Types by Biome:**
  - Plains: Oak, Birch (common building woods)
  - Forest: Pine, Maple (abundant lumber)
  - Jungle: Mahogany, Teak (high-value woods)
  - Mountain: Spruce (cold-resistant wood)
- **Tree Maturity:** Older trees yield more logs (3-5 years growth)
- **Processing:** Sawmills convert logs to planks at 2:3 ratio
- **Replanting:** Players can plant saplings for renewable forestry
- **Skill Impact:** Higher skill reduces waste during processing

**Economic Balance:**

- Tree regrowth: 2-7 real-time days depending on type
- Clear-cutting consequences: Reduced spawns in over-harvested areas
- Sustainable forestry bonuses for replanting

### Farming & Animal Husbandry

Grow crops (grain, vegetables) and raise livestock. Provides food and raw fibers (flax, cotton, wool).
Farming skill and land fertility affect output.

**Mechanics:**

- **Crop Cycles:**
  - Fast crops (wheat, vegetables): 2-3 real-time days
  - Medium crops (cotton, flax): 4-6 real-time days
  - Slow crops (fruit trees): 14-30 real-time days
- **Soil Fertility:** Degrades with repeated planting, requires rotation or fertilizer
- **Livestock Management:**
  - Feeding requirements: Daily food consumption
  - Breeding: Produces offspring after gestation period
  - Products: Milk (daily), wool (weekly), eggs (daily), meat (slaughter)
- **Skill Impact:** Higher skill = better yields, healthier animals
- **Weather Impact:** Drought, frost can damage crops

**Economic Balance:**

- Land ownership required: Prevents unlimited farming
- Upkeep costs: Animal feed, seeds, tools
- Seasonal bonuses: Spring/summer yields +20%

### Foraging/Gathering

Collect wild herbs, mushrooms, berries, and wood pulp. Reagents for alchemy often come from specific biomes.

**Mechanics:**

- **Wild Spawns:**
  - Herbs spawn in specific biomes (forest herbs, mountain herbs, swamp herbs)
  - Mushrooms in dark/damp areas
  - Berries seasonal (summer/autumn)
- **Gathering Skill:** Improves identification and yield
- **Rare Spawns:** Legendary reagents in dangerous areas (5% spawn rate)
- **Knowledge System:** Higher skill unlocks identification of rare plants
- **Tool Impact:** Herbalist tools improve quality of gathered herbs

**Economic Balance:**

- Respawn timers: 15-30 minutes for common, 1-24 hours for rare
- Biome-specific: Creates regional specialization
- Diminishing returns: Over-gathering reduces spawn rates

### Hunting & Fishing

Kill animals or fish for meat, leather hides, feathers, and bones. These yield food and crafting inputs
(leather, bones).

**Mechanics:**

- **Wildlife Population:** Dynamic spawning based on ecosystem health
- **Hunting Yields:**
  - Small game (rabbits, birds): Meat, feathers, bones
  - Medium game (deer, boar): Meat, leather, bones
  - Large game (bear, elk): High-quality leather, trophy items
- **Fishing:**
  - Common fish (trout, bass): Basic food
  - Uncommon fish (salmon, tuna): Better food/oil
  - Rare fish (marlin, exotic species): Luxury trade items
- **Skill Impact:** Better tracking, higher yield, rare loot chances
- **Overhunting Penalty:** Reduced spawns in over-hunted areas

**Economic Balance:**

- Population regeneration: Slower for larger animals
- Hunting limits: Daily caps prevent exploitation
- Protected species: Penalties for hunting endangered animals

### Crafting/Processing

Smelting ore into ingots, sawing logs into planks, spinning fiber into cloth. Processing transforms
basic materials into refined resources.

**Mechanics:**

- **Processing Stations:**
  - Furnace/Smelter: Ore + Fuel → Ingots
  - Sawmill: Logs → Planks (2:3 ratio)
  - Spinning Wheel: Fiber → Thread
  - Loom: Thread → Cloth
- **Time Requirements:** Processing takes real-time (5 seconds to 2 minutes)
- **Batch Processing:** Process multiple items simultaneously with advanced facilities
- **Fuel Costs:** Smelting requires coal or charcoal
- **Skill Impact:** Reduces waste, increases output quality

**Economic Balance:**

- Equipment costs: Building and maintaining processing stations
- Time investment: Encourages specialization
- Energy costs: Fuel consumption prevents infinite processing

### Loot & Drops

Monster drops or expedition finds can source rare materials (gems, magical cores). Full-loot PvP
also redistributes gear as a high-end sink.

**Mechanics:**

- **Monster Loot Tables:**
  - Common drops: Basic materials, coins (80%)
  - Uncommon drops: Quality materials, gems (15%)
  - Rare drops: Legendary components, unique items (5%)
- **Boss Encounters:** Guaranteed high-value drops
- **Dungeon Rewards:** Chests with rare materials
- **PvP Loot:**
  - Full-loot zones: Winner takes all equipped items
  - Partial-loot zones: Winner takes 30% of inventory
  - Safe zones: No looting
- **Salvaging:** Break down equipment for raw materials

**Economic Balance:**

- Drop rates carefully tuned to prevent flooding
- Rare drop protection: Bad luck prevention after X attempts
- PvP incentivizes risk vs reward decisions

## Sinks

List how resources leave circulation or are consumed:

### Crafting Consumption

Most raw resources are sunk into recipes (weapons, armor, tools, buildings). Higher-tier crafts
consume more inputs.

**Consumption Rates:**

- **Basic Crafts:**
  - Iron Sword: 5 Iron Ingots, 2 Wood
  - Leather Armor: 8 Leather Pieces, 4 Thread
- **Intermediate Crafts:**
  - Steel Armor: 15 Steel Ingots, 5 Cloth, 10 Leather
  - Enchanted Weapon: 10 Rare Metal, 5 Gems, 3 Magical Essences
- **Advanced Crafts:**
  - Legendary Equipment: 50+ rare materials, unique components
  - Buildings: 100+ stone, 50+ wood, 25+ metal

**Economic Impact:**

- Constant demand for raw materials
- Higher tiers create exponential demand
- Failed crafts do NOT consume materials (encourages experimentation)

### Construction

Building structures (homes, workshops, roads) consumes large amounts of wood, stone, metal, and cloth.

**Material Requirements:**

- **Small House:** 200 Wood, 150 Stone, 20 Metal, 10 Cloth
- **Workshop:** 300 Wood, 250 Stone, 100 Metal, 50 Cloth
- **Guild Hall:** 2000 Wood, 3000 Stone, 500 Metal, 200 Cloth
- **Castle Wall (per section):** 1000 Stone, 200 Metal
- **Roads (per tile):** 10 Stone, 5 Gravel

**Economic Impact:**

- Massive one-time material sinks
- Drives bulk trading and resource stockpiling
- Creates demand for transport services

### Equipment Repair and Maintenance

Tools and armor degrade with use. Repairing them consumes resources or currency.

**Degradation System:**

- **Durability Loss:**
  - Combat: 1% per hit taken or dealt
  - Tool use: 0.5% per resource gathered
  - Movement: 0.01% per distance traveled (armor only)
- **Repair Costs:**
  - Same-material repair: 20% of crafting cost per 25% durability restored
  - Blacksmith service: 30% of crafting cost + labor fee
  - Field repair: 50% of crafting cost (less efficient)
- **Max Durability Decay:** Each repair reduces max durability by 1%

**Economic Impact:**

- Ongoing demand for materials
- Creates service economy for repair specialists
- Eventually items must be replaced (long-term sink)

### Decay & Spoilage

Perishable goods (food, alchemy reagents) spoil over time. Organic materials may rot.

**Decay Rates:**

- **Food:**
  - Raw meat: 1 real-time day
  - Cooked food: 3 real-time days
  - Preserved food: 7-14 real-time days
  - Dried/Canned: 30+ real-time days
- **Alchemy Reagents:**
  - Fresh herbs: 3 real-time days
  - Dried herbs: 14 real-time days
  - Rare components: Do not decay (magical preservation)
- **Organic Materials:**
  - Leather: Very slow decay (90+ days)
  - Wood: Minimal decay unless exposed to water

**Preservation Methods:**

- Cold storage: Doubles shelf life
- Preservation magic: Triples shelf life (costs mana)
- Canning/Smoking: Converts to long-term storage

**Economic Impact:**

- Time pressure for food trading
- Value of preservation facilities
- Natural sink for excess production

### Consumable Use

Potion drinking, food consumption, arrow/bolt usage all destroy items on use.

**Consumption Patterns:**

- **Combat Consumables:**
  - Healing potions: 2-5 per difficult fight
  - Buff potions: 1-2 per boss encounter
  - Arrows/Bolts: 20-50 per combat session
- **Survival Consumables:**
  - Food: 3 meals per real-time day
  - Water: Minimal need with wells/fountains
- **Utility Consumables:**
  - Torches: Burn out after 10 minutes
  - Repair kits: Single use
  - Teleport scrolls: Single use

**Economic Impact:**

- Constant, predictable demand
- Scales with player activity
- Creates stable markets

### Taxes & Fees

NPC-controlled sinks such as market taxes, tolls, or tribute remove currency.

**Tax Systems:**

- **Market Fees:**
  - Listing fee: 1-5% of list price (prevents spam)
  - Transaction fee: 5% of sale price (removed from economy)
- **Property Taxes:**
  - Housing: 1% of value per month
  - Land plots: 0.5% of value per month
- **Travel Tolls:**
  - Fast travel: 50-500 gold depending on distance
  - Bridge/road tolls: 5-20 gold per crossing
- **Guild Fees:**
  - Formation cost: 10,000 gold
  - Maintenance: 1,000 gold per week

**Economic Impact:**

- Major currency sink
- Discourages hoarding
- Funds NPC infrastructure (roleplay justification)

### Rituals & Sacrifices

(Optional) Rituals might consume rare components.

**Ritual Costs:**

- **Summoning Rituals:**
  - Materials: 10-50 rare reagents
  - Offerings: Gems, magical items
- **Enchanting Rituals:**
  - Sacrifice: Lower-tier enchanted items
  - Components: Magical essences, rare crystals
- **Divine Rituals:**
  - Offerings: Food, valuables, crafted goods
  - Rewards: Divine blessings, faction reputation

**Economic Impact:**

- Sink for rare materials
- Creates demand for specific items
- Optional - not required for progression

### Trade & Market Fees

Listing fees or broker fees consume money.

**Fee Structure:**

- **Auction House:**
  - Listing: 1-5% of starting bid (non-refundable if unsold)
  - Success fee: 5% of final price
  - Relist fee: 50% of original listing fee
- **Broker Services:**
  - Commission: 10-15% of trade value
  - Escrow service: 5% fee for high-value trades
  - Cross-region trade: +2% per region distance

**Economic Impact:**

- Prevents market spam
- Removes currency from circulation
- Creates value in direct player-to-player trades

## Economic Roles

Explain each category's place in the broader economy:

### Local Staple vs. Export

Basic resources (stone, wood, fiber, food) are locally abundant; refined metals and rare reagents
become valuable exports.

**Regional Specialization:**

- **Mountain Regions:** Metal ore abundance (iron, copper, silver)
- **Forest Regions:** Timber abundance (oak, pine, hardwoods)
- **Plains Regions:** Agricultural abundance (wheat, cotton, livestock)
- **Coastal Regions:** Fish abundance, salt production
- **Desert Regions:** Sand, rare minerals, exotic herbs
- **Swamp Regions:** Rare alchemy ingredients, unique woods

**Trade Dynamics:**

- Local abundance = low prices, oversupply
- Regional scarcity = high prices, import demand
- Transportation costs make bulk basics less profitable
- Lightweight valuables (gems, potions) better for long-distance trade

### Crafting Progression

Early-game crafts use common resources, while end-game items consume rare materials.

**Progression Tiers:**

- **Tier 1 (Beginner):**
  - Materials: Iron, Copper, Oak, Cotton, Basic Herbs
  - Items: Basic tools, leather armor, simple weapons
  - Cost: 10-100 gold worth of materials
- **Tier 2 (Intermediate):**
  - Materials: Steel, Bronze, Hardwood, Quality Leather, Uncommon Herbs
  - Items: Quality armor, enchanted weapons, potions
  - Cost: 500-2,000 gold worth of materials
- **Tier 3 (Advanced):**
  - Materials: Mithril, Gold, Exotic Woods, Silk, Rare Gems
  - Items: Master-crafted armor, powerful enchantments
  - Cost: 5,000-20,000 gold worth of materials
- **Tier 4 (Legendary):**
  - Materials: Adamantite, Ancient Woods, Legendary Gems, Dragon Scales
  - Items: Legendary weapons, artifact armor
  - Cost: 50,000+ gold worth of materials

**Economic Impact:**

- New players can afford early crafting
- End-game players drive demand for rare materials
- Progression creates natural material tiers

### Labor Specialization

Gatherers provide raw inputs, crafters convert them into goods, creating interdependence.

**Specialization Roles:**

- **Primary Gatherers:**
  - Miners: Focus on ore/gem extraction
  - Loggers: Focus on timber harvesting
  - Farmers: Focus on crops/livestock
  - Hunters: Focus on meat/leather
- **Processors:**
  - Smelters: Convert ore to metal
  - Sawmill Operators: Convert logs to planks
  - Tanners: Process hides to leather
  - Millers: Process grain to flour
- **Crafters:**
  - Blacksmiths: Create metal equipment
  - Tailors: Create cloth armor/clothing
  - Alchemists: Create potions
  - Enchanters: Enhance equipment
- **Merchants:**
  - Bulk traders: Buy and resell materials
  - Arbitrage traders: Cross-region trading
  - Shopkeepers: Sell to local consumers

**Economic Impact:**

- Creates player interdependence
- Specialization bonuses encourage focus
- Complex supply chains add depth

### Value Chains

Base resources → intermediate goods → final outputs.

**Example Value Chains:**

**Metal Chain:**

```text
Iron Ore (Mining) →
  Iron Ingots (Smelting) →
    Steel Ingots (Advanced Smelting + Coal) →
      Steel Weapon (Blacksmithing + Wood for Handle) →
        Enchanted Steel Weapon (Enchanting + Magical Components)
```

**Textile Chain:**

```text
Cotton (Farming) →
  Cotton Thread (Spinning) →
    Cotton Cloth (Weaving) →
      Cloth Armor (Tailoring + Leather Reinforcement) →
        Enchanted Cloth Armor (Enchanting + Gems)
```

**Food Chain:**

```text
Wheat (Farming) →
  Flour (Milling) →
    Dough (Mixing + Water) →
      Bread (Baking) →
        Fortified Bread (Alchemy + Herbs)
```

**Economic Impact:**

- Each step adds value (profit opportunity)
- Specialization at each stage
- Longer chains = more player interactions

### Trade & Currency

Certain materials may act as currency or commodity anchors.

**Currency Materials:**

- **Gold/Silver:** Direct value as coinage
- **Gems:** High-value portable wealth
- **Rare Ingots:** Barter in high-end trades

**Commodity Anchors:**

- **Iron Ingots:** Universal building material (stable price)
- **Wheat/Grain:** Food staple (price indicator of economy health)
- **Coal:** Energy source (drives processing costs)

**Economic Impact:**

- Stable commodities resist inflation
- Allows for material-based trading
- Creates natural price relationships

### Economic Anchors

Some items drive demand through gameplay goals (e.g. castles, legendary weapons).

**Major Anchors:**

- **Guild Halls:** Massive material sink (5,000+ materials)
- **Castles:** Ultra-massive sink (50,000+ materials)
- **Legendary Crafting:** Requires rare, hard-to-obtain components
- **Ship Building:** Large multi-material projects
- **Territory Control:** Ongoing maintenance requirements

**Economic Impact:**

- Long-term goals drive sustained demand
- Guild-level projects create coordinated effort
- Prestige items justify high prices

## Industry Terms and Simulation Concepts

### Value Chain

Resources flow through sources, transforms, pools, and sinks.

**Flow Diagram:**

```text
SOURCES (Faucets) → TRANSFORMS (Processing) → POOLS (Storage) → SINKS (Consumption)
     ↓                      ↓                       ↓                    ↓
  Mining              Smelting              Player Inventory        Crafting
  Logging             Sawmilling            Guild Banks             Repair
  Farming             Spinning              Market Listings         Decay
  Hunting             Cooking               Warehouses              Consumption
```

### Supply Chain

Real-world analogy: extraction → processing → distribution → use.

**BlueMarble Supply Chain:**

1. **Extraction:** Gathering raw materials from the world
2. **Processing:** Converting raw materials to refined goods
3. **Distribution:** Moving materials to craftspeople or markets
4. **Manufacturing:** Crafting finished goods
5. **Retail:** Selling to end consumers
6. **Use:** Consumption or equipment until degradation

### Throughput

Rate of resource flow. Bottlenecks can cause backlogs.

**Throughput Mechanics:**

- **Gathering Rate:** Materials/hour per gatherer
- **Processing Rate:** Refined goods/hour per processor
- **Crafting Rate:** Items/hour per crafter
- **Bottleneck Identification:** If processing < gathering, raw materials accumulate

**Balance Targets:**

- Gathering: 50-100 units/hour
- Processing: 40-80 units/hour (90% efficiency)
- Crafting: 20-40 items/hour
- Consumption: 30-50 items/hour

### Inventory Turnover

How quickly stock is consumed and replenished.

**Turnover Metrics:**

- **High Turnover (1-3 days):** Consumables (food, potions, arrows)
- **Medium Turnover (1-2 weeks):** Common materials (iron, wood, leather)
- **Low Turnover (1+ months):** Rare materials (gems, legendary components)

**Economic Impact:**

- High turnover items need constant restocking
- Low turnover items can be stockpiled
- Turnover affects market liquidity

### Degradation

Items/resources lose durability or quality with use/time.

**Degradation Types:**

- **Equipment Durability:** Loses effectiveness with use
- **Food Spoilage:** Becomes inedible over time
- **Material Quality:** Exposure to elements reduces quality
- **Max Durability Loss:** Permanent reduction from repairs

**Economic Impact:**

- Ensures ongoing demand (replacement cycle)
- Prevents indefinite item accumulation
- Creates urgency for perishables

## Best Practices & Examples

### RuneScape

Uses many sinks (repairs, consumables, quests) to control inflation.

**Key Features:**

- Item degradation on death
- Construction skill (massive material sink)
- Alchemy system (converts items to gold at controlled rates)
- Untradeable quest rewards (prevents farming)

**Lessons for BlueMarble:**

- Multiple diverse sinks prevent inflation
- Death penalties create item demand
- Untradeable items control rare item supply
- Player-owned houses sink massive materials

### Albion Online

All gear is player-crafted, full-loot PvP acts as sink.

**Key Features:**

- 100% player-driven economy
- Full-loot PvP zones (gear destroyed/looted)
- Localized resources (encourages trade)
- Black market for anonymity

**Lessons for BlueMarble:**

- Full loot creates strong sink in PvP zones
- Regional resources drive trade
- Player crafting ensures all items have material value
- Risk zones balance risk vs reward

### Eco

Player-driven stores and contracts, currency backed by resources.

**Key Features:**

- Player-owned shops with custom prices
- Contract system for labor/materials
- Government and taxation player-controlled
- Environmental impact from resource extraction

**Lessons for BlueMarble:**

- Player shops create diverse economy
- Contracts formalize trade agreements
- Player governance adds political layer
- Resource limits force sustainable practices

### Life is Feudal

Material quality affects final outputs, rewarding specialization.

**Key Features:**

- Quality system (0-100) for all materials
- Quality affects crafting success and output quality
- Skill training through repetition
- Terraforming and massive construction projects

**Lessons for BlueMarble:**

- Quality variation creates market tiers
- Specialization rewarded with better outputs
- Massive construction projects sink materials
- Skill progression through practice

## Balancing Strategy

### Balance Sources and Sinks

Keep inflows ≈ outflows.

**Monitoring Approach:**

- Track total resources entering economy per day
- Track total resources leaving economy per day
- Target: Outflow = 90-110% of inflow (slight deflation preferred)

**Adjustment Levers:**

- Increase sinks: Higher degradation rates, more repairs needed
- Decrease sources: Lower drop rates, longer respawn timers
- Add new sinks: New crafting recipes, construction projects
- Remove sources: Limit gathering in over-farmed areas

### Adjust Drop/Harvest Rates

Calibrate node respawn and yield.

**Tuning Variables:**

- **Respawn Timer:** Time between node availability
- **Yield per Node:** Units gathered per interaction
- **Node Density:** Spawn rate in regions
- **Skill Scaling:** Yield increase per skill level

**Testing Process:**

1. Establish baseline (e.g., 50 iron/hour per player)
2. Monitor actual rates in live environment
3. Compare to consumption rates
4. Adjust respawn/yield to match consumption
5. Re-test and iterate

### Control Decay and Depreciation

Tune spoilage/durability rates.

**Tuning Variables:**

- **Equipment Durability Loss Rate:** % per use
- **Food Spoilage Time:** Real-time hours until decay
- **Repair Costs:** % of crafting cost to repair
- **Max Durability Decay:** % lost per repair

**Target Metrics:**

- Equipment lifespan: 40-80 hours of active use
- Food should spoil before next harvest cycle
- Repair costs: 15-25% of crafting cost
- Item replacement cycle: Every 2-4 weeks of active play

### Price Feedback Loops

NPC traders adjust buy/sell prices.

**Dynamic Pricing System:**

- **Oversupply:** NPC buy price decreases, sell price stable
- **Undersupply:** NPC buy price increases, sell price increases
- **Price Ranges:** NPC prices within 50-150% of base price
- **Adjustment Rate:** 5-10% per day based on supply

**Implementation:**

```javascript
function adjustNPCPrice(item, currentSupply, targetSupply) {
    const supplyRatio = currentSupply / targetSupply;
    const basePrice = item.basePrice;
    
    if (supplyRatio > 1.5) {
        // Oversupply - NPC buys less
        return basePrice * Math.max(0.5, 1 / supplyRatio);
    } else if (supplyRatio < 0.5) {
        // Undersupply - NPC pays more
        return basePrice * Math.min(1.5, 1 / supplyRatio);
    }
    
    return basePrice;
}
```

### Monitor Metrics

Track inventories, trade volumes, market prices.

**Key Metrics:**

- **Material Inventory:** Total units in player possession
- **Trade Volume:** Units traded per day
- **Average Prices:** Market price trends over time
- **Price Volatility:** Price fluctuation magnitude
- **Wealth Distribution:** Gini coefficient of player wealth
- **Material Velocity:** Times material changes hands

**Reporting Dashboard:**

- Daily snapshot of key metrics
- Weekly trend analysis
- Monthly comprehensive review
- Alert triggers for anomalies (>50% price change)

### Iterate Gradually

Make small parameter changes and monitor effects.

**Best Practices:**

- **Single Variable Changes:** Change one thing at a time
- **Small Adjustments:** 5-10% changes, not 50-100%
- **Observation Period:** Wait 1-2 weeks before next change
- **A/B Testing:** Test changes on subset of servers
- **Rollback Plan:** Be ready to revert changes
- **Community Communication:** Announce changes transparently

## Developer Checklists

### Phase 1: Core Design

- [x] List materials & categories
- [x] Design resource nodes (spawn, density, respawn)
- [ ] Define material properties (weight, value, stack size)
- [ ] Create material hierarchy (raw → refined → crafted)
- [ ] Document quality system for materials
- [ ] Design resource distribution by biome/region

### Phase 2: Source Implementation

- [ ] Implement gathering systems (mining, logging, foraging)
- [ ] Create resource nodes with respawn mechanics
- [ ] Implement skill-based gathering bonuses
- [ ] Add tool quality impact on gathering
- [ ] Create fishing/hunting wildlife systems
- [ ] Implement processing stations (furnaces, sawmills)
- [ ] Add crafting recipes for material conversion

### Phase 3: Sink Implementation

- [ ] Build crafting recipes for all items
- [ ] Implement equipment durability system
- [ ] Create repair mechanics and costs
- [ ] Add food/potion consumption mechanics
- [ ] Implement spoilage/decay for perishables
- [ ] Set up construction material requirements
- [ ] Add market fees and taxes
- [ ] Implement ritual/sacrifice sinks (optional)

### Phase 4: Economic Tools

- [ ] Create player marketplace system
- [ ] Implement auction house (future)
- [ ] Add NPC vendors with dynamic pricing
- [ ] Create trade chat/interface
- [ ] Implement escrow for high-value trades
- [ ] Add price history tracking
- [ ] Create market analytics dashboard

### Phase 5: Quality & Balance

- [ ] Implement quality/degradation mechanics
- [ ] Create material quality tiers
- [ ] Add crafting quality calculations
- [ ] Implement max durability decay
- [ ] Add preservation mechanics for food

### Phase 6: Monitoring

- [ ] Track economy metrics (inventory, prices, volume)
- [ ] Create admin dashboard for economy monitoring
- [ ] Implement automated alerts for anomalies
- [ ] Set up logging for all transactions
- [ ] Create weekly/monthly economic reports

### Phase 7: Testing & Tuning

- [ ] Run balance simulations with various player counts
- [ ] Test material flow rates (gathering → consumption)
- [ ] Validate price stability
- [ ] Test edge cases (hoarding, flooding)
- [ ] Conduct player feedback sessions
- [ ] Adjust parameters based on data

### Phase 8: Polish & Launch

- [ ] Write player-facing documentation
- [ ] Create tutorial systems for economy
- [ ] Implement anti-exploit measures
- [ ] Add logging for suspicious activity
- [ ] Prepare rollout communication
- [ ] Monitor post-launch metrics intensively

## Rollout Plan

### Phase 1 – Core Resources (Weeks 1-4)

**Goal:** Introduce basic gathering and sinks to establish economy foundation.

**Features:**

- Basic gathering (metals, wood, stone, fiber, crops)
- Simple crafting recipes (basic tools, weapons, armor)
- NPC vendors for baseline prices
- Equipment durability and repair
- Food consumption mechanics

**Success Metrics:**

- 60%+ players engage in gathering
- 40%+ players engage in crafting
- Stable prices within ±20% of targets
- Positive player feedback on gathering

### Phase 2 – Intermediate Goods (Weeks 5-8)

**Goal:** Add processing chains and more complex crafting.

**Features:**

- Processing stations (smelting, weaving, milling)
- Multi-stage recipes (ore → ingot → item)
- Quality system for materials
- Advanced crafting (enchanting basics)
- Player-to-player trading interface

**Success Metrics:**

- 30%+ players specialize in processing
- 25%+ players engage in trading
- Material prices reflect processing costs
- Active trade chat

### Phase 3 – Ecosystem Growth (Weeks 9-16)

**Goal:** Add advanced materials and large-scale uses.

**Features:**

- Construction system (houses, workshops)
- Guild structures and requirements
- Ship building (if naval content exists)
- Player marketplace/auction house
- Market fees and taxes
- Cross-region trading

**Success Metrics:**

- 10+ player houses built per day
- Active marketplace with 100+ listings
- Healthy price competition
- Regional price variations (arbitrage opportunities)

### Phase 4 – Specialization & Progression (Weeks 17-24)

**Goal:** Add depth through quality scaling and rare content.

**Features:**

- Quality scaling (material quality affects outputs)
- Rare resource nodes (legendary materials)
- Advanced alchemy (complex potions)
- Item degradation improvements
- Legendary crafting recipes
- Repair specialist profession

**Success Metrics:**

- 20%+ players have specialized profession
- Quality tiers visible in market prices
- Rare materials trade at 10x+ common materials
- Low inflation rate (<5% per month)

### Phase 5 – Stabilization (Weeks 25-32)

**Goal:** Monitor and tune the economy for long-term health.

**Features:**

- Economy monitoring dashboard
- Dynamic NPC pricing based on supply
- Seasonal events (harvest festivals)
- Additional sinks (community projects)
- Tweak spawn/decay rates based on data

**Success Metrics:**

- Material inflow ≈ outflow (±10%)
- Stable average prices (±15% variation)
- Low player complaints about economy
- High engagement in economic activities

### Phase 6 – Continuous Iteration (Ongoing)

**Goal:** Keep economy fresh and balanced.

**Features:**

- New sinks (seasonal events, community projects)
- New sources (content expansions, new areas)
- Adjust faucets as needed (respawn rates)
- Add new crafting recipes
- Introduce limited-time materials
- Balance PvP loot mechanics

**Success Metrics:**

- Sustained player engagement
- Healthy trade volumes
- Low inflation/deflation
- Positive player sentiment
- Active economy at all levels

## Related Documentation

- [Economy Systems](../../systems/economy-systems.md) - Overall economic system design
- [Economy Design](../../../design/economy.md) - High-level economic vision
- [Crafting Mechanics](./crafting-mechanics-overview.md) - Crafting system specifications
- [Mining and Resource Extraction](./mining-resource-extraction.md) - Mining mechanics details
- [Player Resource Dashboard](../spec-player-resource-dashboard.md) - UI for resource management
- [Marketplace Usage Guide](../marketplace-usage-guide.md) - Player trading guide

## Notes

- This document focuses on material economy (physical resources), not currency economy
- Currency sinks and sources covered in [Economy Systems](../../systems/economy-systems.md)
- Quality systems detailed in [Crafting Quality Model](./crafting-quality-model.md)
- PvP loot mechanics to be detailed in separate PvP design document

---

*This material economy design provides the foundation for a sustainable, player-driven economy in BlueMarble.
Regular monitoring and iterative adjustments will be essential to maintain balance as the player base grows and
evolves.*
