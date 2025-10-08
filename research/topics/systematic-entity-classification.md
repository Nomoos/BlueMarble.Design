# Systematic Entity Classification: A Taxonomic Framework for Game Entities

**Date**: 2025-01-28  
**Purpose**: Design a comprehensive hierarchical classification system for all game entities, services, and resources  
**Inspiration**: Biological taxonomy (Domain → Kingdom → Phylum → Class → Order → Family → Genus → Species)  
**Related**: Biome classification, fruit categorization, material systems, commodity databases

---

## Executive Summary

This document establishes a comprehensive taxonomic framework for classifying all entities in BlueMarble, inspired by biological systematic classification. Just as biology uses Domain → Kingdom → Phylum → Class → Order → Family → Genus → Species, this system provides a hierarchical structure for organizing game objects, resources, services, NPCs, structures, and abstract concepts.

### Key Benefits

- **Consistent Organization**: Uniform classification across all game systems
- **Scalability**: Easy addition of new entities without restructuring
- **Discoverability**: Players and developers can navigate entity relationships
- **Cross-referencing**: Related entities naturally grouped together
- **Query Optimization**: Database queries benefit from hierarchical structure
- **Modding Support**: Clear extension points for custom content

### Classification Levels (7-tier Hierarchy)

1. **Domain** - Broadest category (Physical, Abstract, Service)
2. **Realm** - Major subdivisions (Natural, Crafted, Living, etc.)
3. **Kingdom** - Primary entity type (Flora, Fauna, Materials, Structures)
4. **Class** - Functional category (Food, Tool, Building, Commodity)
5. **Order** - Behavioral group (Perishable, Durable, Renewable)
6. **Family** - Related entities (Pome fruits, Root vegetables, Metal ores)
7. **Species** - Individual entity (Apple, Carrot, Iron ore)

---

## Part I: Domain Classification System

### Domain 1: Physical Entities

**Definition**: Tangible objects that occupy space and can be interacted with directly

#### Realm 1.1: Natural Resources
Entities found in the environment without crafting

**Kingdom 1.1.1: Flora (Plant Life)**
- Class: Food Plants
  - Order: Fruits
    - Family: Pome Fruits → Species: Apple, Pear, Quince
    - Family: Stone Fruits → Species: Cherry, Plum, Apricot
    - Family: Berries → Species: Serviceberry, Elderberry, Currant
  - Order: Vegetables
    - Family: Root Vegetables → Species: Carrot, Turnip, Parsnip
    - Family: Leafy Greens → Species: Lettuce, Cabbage, Kale
  - Order: Grains
    - Family: Cereals → Species: Wheat, Barley, Oats
    - Family: Pseudocereals → Species: Quinoa, Amaranth

- Class: Material Plants
  - Order: Timber
    - Family: Hardwoods → Species: Oak, Maple, Walnut
    - Family: Softwoods → Species: Pine, Spruce, Fir
  - Order: Fiber Plants
    - Family: Bast Fibers → Species: Flax, Hemp, Jute
    - Family: Leaf Fibers → Species: Sisal, Abaca

- Class: Medicinal Plants
  - Order: Herbs
    - Family: Aromatic Herbs → Species: Mint, Basil, Thyme
    - Family: Medicinal Roots → Species: Ginseng, Ginger

**Kingdom 1.1.2: Fauna (Animal Life)**
- Class: Livestock
  - Order: Large Mammals
    - Family: Bovines → Species: Cattle, Bison, Water Buffalo
    - Family: Equines → Species: Horse, Donkey, Mule
  - Order: Small Mammals
    - Family: Swine → Species: Pig, Wild Boar
    - Family: Ovines → Species: Sheep, Goat

- Class: Wildlife
  - Order: Game Animals
    - Family: Deer → Species: White-tailed Deer, Elk, Moose
    - Family: Birds → Species: Duck, Goose, Pheasant
  - Order: Fish
    - Family: Freshwater → Species: Trout, Bass, Pike
    - Family: Saltwater → Species: Cod, Salmon, Tuna

- Class: Working Animals
  - Order: Beasts of Burden
    - Family: Draft Animals → Species: Ox, Draft Horse
  - Order: Guard Animals
    - Family: Canines → Species: Guard Dog, War Dog

**Kingdom 1.1.3: Minerals**
- Class: Metal Ores
  - Order: Base Metals
    - Family: Iron Group → Species: Iron Ore, Hematite, Magnetite
    - Family: Copper Group → Species: Copper Ore, Malachite, Azurite
  - Order: Precious Metals
    - Family: Noble Metals → Species: Gold Ore, Silver Ore, Platinum
  - Order: Industrial Metals
    - Family: Light Metals → Species: Aluminum Ore, Titanium Ore
    - Family: Specialty Metals → Species: Tin Ore, Zinc Ore, Lead Ore

- Class: Stone & Rock
  - Order: Building Stone
    - Family: Igneous → Species: Granite, Basalt, Obsidian
    - Family: Sedimentary → Species: Limestone, Sandstone, Shale
    - Family: Metamorphic → Species: Marble, Slate, Quartzite
  - Order: Aggregate Materials
    - Family: Loose Stone → Species: Gravel, Sand, Clay

- Class: Gemstones
  - Order: Precious Gems
    - Family: Transparent → Species: Diamond, Sapphire, Ruby
  - Order: Semi-Precious
    - Family: Quartz → Species: Amethyst, Citrine, Jade

**Kingdom 1.1.4: Environmental Elements**
- Class: Water Sources
  - Order: Fresh Water → Species: River Water, Lake Water, Well Water
  - Order: Salt Water → Species: Sea Water, Brine
- Class: Energy Sources
  - Order: Fossil Fuels → Species: Coal, Peat, Oil Shale
  - Order: Renewable → Species: Wind, Hydro, Solar

#### Realm 1.2: Crafted Items
Entities created through player or NPC actions

**Kingdom 1.2.1: Materials**
- Class: Processed Materials
  - Order: Metals
    - Family: Base Metal Ingots → Species: Iron Ingot, Copper Ingot, Steel Ingot
    - Family: Alloys → Species: Bronze, Brass, Pewter
  - Order: Textiles
    - Family: Natural Fabrics → Species: Linen, Wool, Cotton
    - Family: Processed Fabrics → Species: Felt, Canvas, Silk
  - Order: Refined Materials
    - Family: Leather → Species: Tanned Leather, Cured Hide
    - Family: Processed Wood → Species: Planks, Beams, Shingles

**Kingdom 1.2.2: Tools & Equipment**
- Class: Hand Tools
  - Order: Cutting Tools
    - Family: Axes → Species: Hand Axe, Felling Axe, Battle Axe
    - Family: Knives → Species: Utility Knife, Skinning Knife
  - Order: Shaping Tools
    - Family: Hammers → Species: Claw Hammer, Sledgehammer, Smith Hammer
    - Family: Saws → Species: Hand Saw, Cross-cut Saw
  - Order: Agricultural Tools
    - Family: Cultivation → Species: Hoe, Rake, Plow
    - Family: Harvesting → Species: Sickle, Scythe, Shears

- Class: Weapons
  - Order: Melee Weapons
    - Family: Swords → Species: Short Sword, Long Sword, Claymore
    - Family: Clubs → Species: Mace, Flail, War Hammer
    - Family: Pole Arms → Species: Spear, Pike, Halberd
  - Order: Ranged Weapons
    - Family: Bows → Species: Short Bow, Long Bow, Crossbow
    - Family: Thrown → Species: Javelin, Throwing Axe

- Class: Armor & Protection
  - Order: Body Armor
    - Family: Light Armor → Species: Leather Armor, Padded Armor
    - Family: Medium Armor → Species: Chain Mail, Scale Mail
    - Family: Heavy Armor → Species: Plate Armor, Full Plate
  - Order: Shields
    - Family: Small Shields → Species: Buckler, Target Shield
    - Family: Large Shields → Species: Kite Shield, Tower Shield

**Kingdom 1.2.3: Consumables**
- Class: Food Items
  - Order: Prepared Foods
    - Family: Baked Goods → Species: Bread, Pie, Cake
    - Family: Cooked Meals → Species: Stew, Roast, Soup
  - Order: Preserved Foods
    - Family: Dried → Species: Dried Fruit, Jerky, Hardtack
    - Family: Pickled → Species: Pickles, Sauerkraut, Kimchi
    - Family: Fermented → Species: Cheese, Wine, Beer

- Class: Potions & Medicines
  - Order: Health Restoration
    - Family: Minor Healing → Species: Bandage, Health Potion (Minor)
    - Family: Major Healing → Species: Health Potion (Major), Healing Salve
  - Order: Buffs & Enhancements
    - Family: Strength → Species: Strength Potion, Fortifying Elixir
    - Family: Speed → Species: Speed Potion, Haste Draught

**Kingdom 1.2.4: Structures**
- Class: Buildings
  - Order: Residential
    - Family: Housing → Species: Cottage, House, Manor
    - Family: Lodging → Species: Inn, Tavern, Boarding House
  - Order: Commercial
    - Family: Crafting → Species: Forge, Tannery, Sawmill
    - Family: Trading → Species: Market Stall, Shop, Warehouse
  - Order: Agricultural
    - Family: Production → Species: Farm, Orchard, Vineyard
    - Family: Storage → Species: Barn, Silo, Granary

- Class: Infrastructure
  - Order: Transportation
    - Family: Roads → Species: Dirt Road, Cobblestone Road, Paved Road
    - Family: Bridges → Species: Wooden Bridge, Stone Bridge
  - Order: Defense
    - Family: Fortifications → Species: Palisade, Wall, Tower
    - Family: Military → Species: Barracks, Armory, Watchtower

---

## Part II: Abstract Domain

### Domain 2: Abstract Entities

**Definition**: Non-physical concepts that affect gameplay

#### Realm 2.1: Player Systems

**Kingdom 2.1.1: Achievements**
- Class: Milestones
  - Order: Exploration → Species: Distance Traveled, Locations Discovered
  - Order: Combat → Species: Enemies Defeated, Bosses Killed
  - Order: Crafting → Species: Items Crafted, Mastery Achieved

**Kingdom 2.1.2: Reputation**
- Class: Faction Standing
  - Order: Positive → Species: Friendly, Honored, Exalted
  - Order: Negative → Species: Unfriendly, Hostile, Hated
  - Order: Neutral → Species: Indifferent, Unknown

**Kingdom 2.1.3: Skills & Abilities**
- Class: Production Skills
  - Order: Crafting → Species: Blacksmithing, Carpentry, Tailoring
  - Order: Gathering → Species: Mining, Herbalism, Fishing
- Class: Combat Skills
  - Order: Weapon Skills → Species: Swordsmanship, Archery, Unarmed
  - Order: Magic Skills → Species: Fire Magic, Ice Magic, Healing

#### Realm 2.2: Economic Systems

**Kingdom 2.2.1: Currencies**
- Class: Primary Currency
  - Order: Standard → Species: Gold Coin, Trade Coin (TC)
- Class: Special Currencies
  - Order: Regional → Species: City Scrip, Guild Token
  - Order: Event → Species: Festival Token, Season Pass

**Kingdom 2.2.2: Contracts**
- Class: Trade Contracts
  - Order: Buy Orders → Species: Resource Request, Bulk Purchase
  - Order: Sell Orders → Species: Resource Sale, Commodity Listing
- Class: Service Contracts
  - Order: Quests → Species: Delivery Quest, Crafting Quest, Combat Quest
  - Order: Employment → Species: Guard Hire, Crafting Service, Transport

#### Realm 2.3: Social Systems

**Kingdom 2.3.1: Organizations**
- Class: Guilds
  - Order: Crafting Guilds → Species: Smithing Guild, Merchant Guild
  - Order: Combat Guilds → Species: Mercenary Company, Knights Order
- Class: Settlements
  - Order: Player Settlements → Species: Hamlet, Village, Town, City
  - Order: NPC Settlements → Species: Trading Post, Fortress, Capital

**Kingdom 2.3.2: Relationships**
- Class: Player Relations
  - Order: Friendship → Species: Acquaintance, Friend, Close Friend
  - Order: Alliance → Species: Trade Partner, Military Ally
  - Order: Rivalry → Species: Competitor, Enemy

---

## Part III: Service Domain

### Domain 3: Services & Actions

**Definition**: Activities, processes, and functions provided to players

#### Realm 3.1: Player Services

**Kingdom 3.1.1: Crafting Services**
- Class: Material Processing
  - Order: Smelting → Species: Ore Smelting, Metal Refining
  - Order: Tanning → Species: Hide Tanning, Leather Treating
  - Order: Milling → Species: Grain Milling, Wood Sawing

- Class: Item Creation
  - Order: Equipment Crafting → Species: Weapon Smithing, Armor Forging
  - Order: Tool Making → Species: Tool Smithing, Implement Crafting
  - Order: Construction → Species: Building, Structure Assembly

**Kingdom 3.1.2: Trading Services**
- Class: Marketplace Operations
  - Order: Direct Trade → Species: Player-to-Player Trade, NPC Trade
  - Order: Auction House → Species: Bidding, Buy Now, Timed Auction
  - Order: Commodity Exchange → Species: Bulk Trading, Futures Trading

- Class: Transportation Services
  - Order: Logistics → Species: Item Transport, Bulk Shipping
  - Order: Personal Travel → Species: Fast Travel, Carriage Service

**Kingdom 3.1.3: Information Services**
- Class: Knowledge Systems
  - Order: Exploration → Species: Map Making, Surveying
  - Order: Research → Species: Lore Discovery, Technical Research
  - Order: Education → Species: Skill Training, Apprenticeship

#### Realm 3.2: System Services

**Kingdom 3.2.1: World Services**
- Class: Environmental Processes
  - Order: Time Systems → Species: Day/Night Cycle, Seasons, Weather
  - Order: Resource Regeneration → Species: Tree Regrowth, Ore Respawn
  - Order: Decay Systems → Species: Food Spoilage, Structure Decay

- Class: Simulation Services
  - Order: Economy Simulation → Species: Price Fluctuation, Supply/Demand
  - Order: Population Dynamics → Species: NPC Migration, Wildlife Spawning
  - Order: Territory Control → Species: Influence Calculation, Border Management

**Kingdom 3.2.2: Management Services**
- Class: Settlement Management
  - Order: Administration → Species: Tax Collection, Law Enforcement
  - Order: Development → Species: Infrastructure Building, Expansion
  - Order: Defense → Species: Guard Patrol, Territory Protection

---

## Part IV: Implementation Framework

### Database Schema Integration

```typescript
interface EntityTaxonomy {
  domain: Domain;           // Level 1: Physical, Abstract, Service
  realm: Realm;            // Level 2: Natural, Crafted, Player Systems
  kingdom: Kingdom;        // Level 3: Flora, Fauna, Minerals, etc.
  class: EntityClass;      // Level 4: Food Plants, Tools, Buildings
  order: Order;            // Level 5: Fruits, Melee Weapons, etc.
  family: Family;          // Level 6: Pome Fruits, Swords, etc.
  species: Species;        // Level 7: Apple, Long Sword, etc.
  
  // Additional metadata
  taxonomyPath: string;    // "Physical.Natural.Flora.Food.Fruits.Pome.Apple"
  taxonomyId: string;      // Unique identifier
  parentId?: string;       // Reference to parent classification
  alternateNames: string[]; // Synonyms, regional names
}

// Example: Apple classification
const appleEntity: EntityTaxonomy = {
  domain: "Physical",
  realm: "Natural Resources",
  kingdom: "Flora",
  class: "Food Plants",
  order: "Fruits",
  family: "Pome Fruits",
  species: "Apple",
  taxonomyPath: "Physical.Natural.Flora.Food.Fruits.Pome.Apple",
  taxonomyId: "PHY.NAT.FLO.FOO.FRU.POM.APP",
  parentId: "PHY.NAT.FLO.FOO.FRU.POM",
  alternateNames: ["Malus domestica", "Common Apple"]
};

// Example: Iron Ingot (crafted from Iron Ore)
const ironIngotEntity: EntityTaxonomy = {
  domain: "Physical",
  realm: "Crafted Items",
  kingdom: "Materials",
  class: "Processed Materials",
  order: "Metals",
  family: "Base Metal Ingots",
  species: "Iron Ingot",
  taxonomyPath: "Physical.Crafted.Materials.Processed.Metals.BaseIngots.IronIngot",
  taxonomyId: "PHY.CRA.MAT.PRO.MET.BAS.IRN",
  parentId: "PHY.CRA.MAT.PRO.MET.BAS",
  alternateNames: ["Refined Iron", "Iron Bar"]
};
```

### Query Patterns

```typescript
// Find all entities in a family
function getEntityFamily(familyId: string): Entity[] {
  return db.query(`
    SELECT * FROM entities 
    WHERE taxonomy_path LIKE '${familyId}%'
  `);
}

// Find related entities (same family, different species)
function getRelatedEntities(entity: Entity): Entity[] {
  const familyPath = entity.taxonomyPath.split('.').slice(0, -1).join('.');
  return db.query(`
    SELECT * FROM entities 
    WHERE taxonomy_path LIKE '${familyPath}%'
    AND entity_id != '${entity.id}'
  `);
}

// Navigate taxonomy hierarchy
function getTaxonomyLevel(entity: Entity, level: number): string {
  return entity.taxonomyPath.split('.')[level];
}

// Example: Get all pome fruits
const pomeFruits = getEntityFamily("Physical.Natural.Flora.Food.Fruits.Pome");
// Returns: Apple, Pear, Quince, Serviceberry, etc.

// Example: Find alternatives to apple
const appleAlternatives = getRelatedEntities(appleEntity);
// Returns: Pear, Quince, Medlar (same family, different species)
```

### Classification Rules

1. **Uniqueness**: Each species has exactly one primary classification path
2. **Inheritance**: Species inherit properties from all parent levels
3. **Cross-referencing**: Entities can have secondary classifications
4. **Extensibility**: New levels can be added between existing ones
5. **Consistency**: Similar entities grouped together by shared characteristics

### Property Inheritance

```typescript
interface EntityProperties {
  // Inherited from Kingdom (Flora)
  isLiving: boolean;
  requiresEnvironment: string;
  
  // Inherited from Class (Food Plants)
  isConsumable: boolean;
  nutritionalValue: number;
  
  // Inherited from Order (Fruits)
  harvestSeason: Season;
  growthTime: number;
  
  // Inherited from Family (Pome Fruits)
  storageMethod: string;
  decayRate: number;
  
  // Specific to Species (Apple)
  varieties: string[];
  specificTraits: object;
}

// Example property inheritance
const appleProperties: EntityProperties = {
  // From Flora
  isLiving: true,
  requiresEnvironment: "temperate",
  
  // From Food Plants
  isConsumable: true,
  nutritionalValue: 50,
  
  // From Fruits
  harvestSeason: "autumn",
  growthTime: 120, // days
  
  // From Pome Fruits
  storageMethod: "cold_storage",
  decayRate: 0.01, // 1% per day
  
  // Specific to Apple
  varieties: ["Granny Smith", "Red Delicious", "Fuji"],
  specificTraits: {
    coldStorageMultiplier: 10,
    dryingEffectiveness: 0.95
  }
};
```

---

## Part V: Integration with Existing Systems

### Fruit Categorization Integration

The existing fruit research already follows taxonomic principles:

```javascript
// Current system
fruitCategories = {
    excellentKeepers: ['Apple', 'Quince', 'Some pears'],
    goodKeepers: ['Asian pear', 'European pear'],
    moderateKeepers: ['Loquat', 'Some crabapples']
}

// Enhanced with taxonomy
fruitDatabase = new Map([
    ['apple', {
        // Taxonomy
        domain: 'Physical',
        kingdom: 'Flora',
        family: 'Pome',
        species: 'Apple',
        scientificName: 'Malus domestica',
        
        // Game properties (existing)
        category: 'excellentKeeper',
        baseDecayRate: 0.01,
        coldStorageMultiplier: 0.1,
        marketValue: { fresh: 20, dried: 35 }
    }]
]);
```

### Biome Classification Integration

```typescript
// Current biome classification
enum BiomeType {
    TropicalRainforest,
    TemperateForest,
    BorealForest,
    Desert,
    Tundra
}

// Enhanced with taxonomy
interface BiomeTaxonomy {
    domain: "Environmental",
    realm: "Terrestrial",
    kingdom: "Climate Zone",
    class: "Temperature-Precipitation Matrix",
    order: string, // "Tropical", "Temperate", "Arctic"
    family: string, // "Rainforest", "Deciduous", "Coniferous"
    species: BiomeType
}
```

### Material System Integration

```typescript
// Current material categories (scattered)
// Enhanced with unified taxonomy

const materialTaxonomy = {
    // Natural materials
    "wood": {
        domain: "Physical",
        realm: "Natural",
        kingdom: "Flora",
        class: "Material Plants",
        order: "Timber",
        family: "Hardwoods",
        species: "Oak"
    },
    
    // Processed materials
    "iron_ingot": {
        domain: "Physical",
        realm: "Crafted",
        kingdom: "Materials",
        class: "Processed Materials",
        order: "Metals",
        family: "Base Metal Ingots",
        species: "Iron Ingot",
        sourceEntity: "iron_ore" // Link to natural resource
    }
};
```

---

## Part VI: Practical Applications

### 1. Crafting Recipe Organization

```typescript
interface CraftingRecipe {
  output: {
    entityId: string;
    taxonomy: EntityTaxonomy;
  };
  inputs: Array<{
    entityId: string;
    taxonomy: EntityTaxonomy;
    quantity: number;
  }>;
}

// Example: Sword crafting uses taxonomy to find valid materials
function getCraftingRecipe(output: string): CraftingRecipe {
  return {
    output: {
      entityId: "longsword",
      taxonomy: {
        domain: "Physical",
        realm: "Crafted",
        kingdom: "Tools & Equipment",
        class: "Weapons",
        order: "Melee Weapons",
        family: "Swords",
        species: "Long Sword"
      }
    },
    inputs: [
      {
        entityId: "steel_ingot",
        taxonomy: {
          family: "Base Metal Ingots", // Any metal ingot could work
          species: "Steel Ingot"
        },
        quantity: 3
      },
      {
        entityId: "leather",
        taxonomy: {
          family: "Leather", // Any leather type
          species: "Tanned Leather"
        },
        quantity: 1
      }
    ]
  };
}

// Flexible recipe matching: Allow similar materials
function canSubstitute(required: EntityTaxonomy, available: EntityTaxonomy): boolean {
  // Materials from same family can often substitute
  return required.family === available.family;
}
```

### 2. Market Price Relationships

```typescript
// Prices influenced by taxonomy relationships
function calculateMarketPrice(entity: Entity, market: Market): number {
  const baseprice = entity.basePrice;
  
  // Supply/demand for entire family affects individual species
  const familySupply = market.getSupply(entity.taxonomy.family);
  const familyDemand = market.getDemand(entity.taxonomy.family);
  
  // Related species compete for same market
  const relatedEntities = getEntityFamily(entity.taxonomy.family);
  const competition = relatedEntities.length;
  
  // Calculate dynamic price
  const supplyDemandRatio = familyDemand / familySupply;
  const competitionFactor = 1 / Math.sqrt(competition);
  
  return baseprice * supplyDemandRatio * competitionFactor;
}
```

### 3. Quest Generation

```typescript
// Generate quests based on entity relationships
function generateGatheringQuest(): Quest {
  // Pick a random family
  const family = randomFamily();
  
  // Request multiple species from that family
  const speciesInFamily = getEntityFamily(family);
  const requestedSpecies = randomSample(speciesInFamily, 3);
  
  return {
    type: "gathering",
    title: `Collect ${family} specimens`,
    description: `Gather various ${family} for research`,
    requirements: requestedSpecies.map(s => ({
      entity: s,
      quantity: randomInt(1, 10)
    })),
    reward: calculateReward(family)
  };
}

// Example generated quest:
// "Collect Pome Fruit specimens"
// - 5x Apple
// - 3x Pear  
// - 2x Quince
```

### 4. Skill System Integration

```typescript
interface Skill {
  name: string;
  affectedTaxonomy: {
    kingdom?: string;
    class?: string;
    order?: string;
    family?: string;
  };
  bonuses: SkillBonus[];
}

// Example: Smithing skill affects all metal crafting
const smithingSkill: Skill = {
  name: "Smithing",
  affectedTaxonomy: {
    kingdom: "Materials",
    class: "Processed Materials",
    order: "Metals"
  },
  bonuses: [
    { type: "quality", value: 0.1 }, // +10% quality per level
    { type: "speed", value: 0.05 }   // +5% crafting speed
  ]
};

// Check if skill applies to entity
function skillApplies(skill: Skill, entity: Entity): boolean {
  const tax = entity.taxonomy;
  return (!skill.affectedTaxonomy.kingdom || tax.kingdom === skill.affectedTaxonomy.kingdom) &&
         (!skill.affectedTaxonomy.class || tax.class === skill.affectedTaxonomy.class) &&
         (!skill.affectedTaxonomy.order || tax.order === skill.affectedTaxonomy.order) &&
         (!skill.affectedTaxonomy.family || tax.family === skill.affectedTaxonomy.family);
}
```

### 5. Discovery & Unlocking System

```typescript
interface DiscoverySystem {
  // Players discover entities by family
  discoveredFamilies: Set<string>;
  discoveredSpecies: Map<string, Entity[]>;
}

// Discovering one species reveals family information
function discoverEntity(entity: Entity, player: Player): void {
  const family = entity.taxonomy.family;
  
  if (!player.discovery.discoveredFamilies.has(family)) {
    // First discovery in this family!
    player.discovery.discoveredFamilies.add(family);
    
    // Reveal silhouettes of related species
    const relatedSpecies = getEntityFamily(family);
    player.discovery.discoveredSpecies.set(family, 
      relatedSpecies.map(s => ({ ...s, revealed: false }))
    );
    
    // Grant discovery bonus
    player.grantReward({
      type: "first_discovery",
      family: family,
      xp: 100
    });
  }
  
  // Mark this specific species as discovered
  const species = player.discovery.discoveredSpecies.get(family);
  const speciesEntry = species.find(s => s.id === entity.id);
  if (speciesEntry) {
    speciesEntry.revealed = true;
  }
}
```

---

## Part VII: Extensibility & Modding

### Adding New Entities

```typescript
// Modders can extend taxonomy by adding new species to existing families
const modEntity: EntityTaxonomy = {
  domain: "Physical",
  realm: "Natural Resources",
  kingdom: "Flora",
  class: "Food Plants",
  order: "Fruits",
  family: "Pome Fruits",
  species: "Medlar", // New fruit not in base game
  taxonomyPath: "Physical.Natural.Flora.Food.Fruits.Pome.Medlar",
  taxonomyId: "PHY.NAT.FLO.FOO.FRU.POM.MED",
  parentId: "PHY.NAT.FLO.FOO.FRU.POM",
  alternateNames: ["Mespilus germanica"]
};

// Or add entire new families
const newFamily: EntityTaxonomy = {
  domain: "Physical",
  realm: "Natural Resources",
  kingdom: "Flora",
  class: "Food Plants",
  order: "Fruits",
  family: "Tropical Fruits", // New family
  species: "Mango",
  taxonomyPath: "Physical.Natural.Flora.Food.Fruits.Tropical.Mango",
  taxonomyId: "PHY.NAT.FLO.FOO.FRU.TRO.MAN",
  parentId: "PHY.NAT.FLO.FOO.FRU.TRO",
  alternateNames: ["Mangifera indica"]
};
```

### Custom Realms & Kingdoms

```typescript
// Mods can add entire new classifications for fantasy elements
const magicalEntity: EntityTaxonomy = {
  domain: "Physical",
  realm: "Magical", // New realm!
  kingdom: "Enchanted Flora",
  class: "Magical Plants",
  order: "Mana-Bearing",
  family: "Mana Fruits",
  species: "Arcane Berry",
  taxonomyPath: "Physical.Magical.EnchantedFlora.MagicalPlants.ManaBearing.ManaFruits.ArcaneBerry",
  taxonomyId: "PHY.MAG.EFL.MAP.MAN.MFR.ARB",
  alternateNames: ["Manaberry", "Spell Fruit"]
};
```

### Validation System

```typescript
// Ensure taxonomy consistency
function validateTaxonomy(entity: EntityTaxonomy): ValidationResult {
  const errors: string[] = [];
  
  // Check path matches individual fields
  const pathParts = entity.taxonomyPath.split('.');
  if (pathParts[0] !== entity.domain) {
    errors.push("Domain mismatch in path");
  }
  
  // Verify parent exists
  if (entity.parentId) {
    const parent = db.getTaxonomy(entity.parentId);
    if (!parent) {
      errors.push("Parent taxonomy not found");
    }
  }
  
  // Check for duplicate IDs
  const existing = db.getTaxonomy(entity.taxonomyId);
  if (existing && existing.species !== entity.species) {
    errors.push("Taxonomy ID already in use");
  }
  
  // Ensure species name is unique within family
  const siblings = getEntityFamily(entity.parentId);
  const duplicate = siblings.find(s => 
    s.species === entity.species && s.taxonomyId !== entity.taxonomyId
  );
  if (duplicate) {
    errors.push("Species name already exists in family");
  }
  
  return {
    valid: errors.length === 0,
    errors: errors
  };
}
```

---

## Part VIII: Migration & Adoption Strategy

### Phase 1: Taxonomy Infrastructure (Week 1-2)

1. **Database Schema**: Add taxonomy fields to entity tables
2. **API Updates**: Add taxonomy endpoints
3. **Documentation**: Create taxonomy reference guide

```sql
-- Add taxonomy columns to entities table
ALTER TABLE entities ADD COLUMN taxonomy_domain VARCHAR(50);
ALTER TABLE entities ADD COLUMN taxonomy_realm VARCHAR(50);
ALTER TABLE entities ADD COLUMN taxonomy_kingdom VARCHAR(50);
ALTER TABLE entities ADD COLUMN taxonomy_class VARCHAR(50);
ALTER TABLE entities ADD COLUMN taxonomy_order VARCHAR(50);
ALTER TABLE entities ADD COLUMN taxonomy_family VARCHAR(50);
ALTER TABLE entities ADD COLUMN taxonomy_species VARCHAR(100);
ALTER TABLE entities ADD COLUMN taxonomy_path VARCHAR(500);
ALTER TABLE entities ADD COLUMN taxonomy_id VARCHAR(100) UNIQUE;
ALTER TABLE entities ADD COLUMN parent_taxonomy_id VARCHAR(100);

-- Create indexes for taxonomy queries
CREATE INDEX idx_taxonomy_path ON entities(taxonomy_path);
CREATE INDEX idx_taxonomy_family ON entities(taxonomy_family);
CREATE INDEX idx_taxonomy_kingdom ON entities(taxonomy_kingdom);
```

### Phase 2: Initial Classification (Week 3-4)

1. **Classify Existing Entities**: Assign taxonomy to all current items
2. **Validation**: Ensure consistency and completeness
3. **Cross-references**: Link related entities

```typescript
// Migration script
async function classifyExistingEntities() {
  const entities = await db.getAllEntities();
  
  for (const entity of entities) {
    // Infer taxonomy from existing category
    const taxonomy = inferTaxonomy(entity);
    
    // Validate and apply
    const validation = validateTaxonomy(taxonomy);
    if (validation.valid) {
      await db.updateEntity(entity.id, { taxonomy });
    } else {
      console.error(`Invalid taxonomy for ${entity.name}:`, validation.errors);
    }
  }
}

function inferTaxonomy(entity: Entity): EntityTaxonomy {
  // Use existing category fields to determine taxonomy
  if (entity.category === 'fruit') {
    return {
      domain: 'Physical',
      realm: 'Natural Resources',
      kingdom: 'Flora',
      class: 'Food Plants',
      order: 'Fruits',
      family: determineFruitFamily(entity),
      species: entity.name,
      taxonomyPath: generatePath(entity),
      taxonomyId: generateId(entity)
    };
  }
  // ... handle other categories
}
```

### Phase 3: System Integration (Week 5-6)

1. **Crafting System**: Use taxonomy for recipe matching
2. **Market System**: Implement family-based pricing
3. **Quest System**: Generate taxonomy-based quests
4. **UI Updates**: Display taxonomy information

### Phase 4: Player Features (Week 7-8)

1. **Discovery System**: Implement family-based discovery
2. **Encyclopedia**: Taxonomy browser for discovered entities
3. **Search & Filter**: Taxonomy-based search
4. **Mod Support**: Enable community extensions

---

## Part IX: Reference Tables

### Complete Taxonomy ID Prefixes

| Domain | Realm | Kingdom | Class | Code |
|--------|-------|---------|-------|------|
| Physical | Natural | Flora | Food Plants | PHY.NAT.FLO.FOO |
| Physical | Natural | Flora | Material Plants | PHY.NAT.FLO.MAT |
| Physical | Natural | Fauna | Livestock | PHY.NAT.FAU.LIV |
| Physical | Natural | Fauna | Wildlife | PHY.NAT.FAU.WIL |
| Physical | Natural | Minerals | Metal Ores | PHY.NAT.MIN.ORE |
| Physical | Natural | Minerals | Stone & Rock | PHY.NAT.MIN.STO |
| Physical | Crafted | Materials | Processed | PHY.CRA.MAT.PRO |
| Physical | Crafted | Tools | Hand Tools | PHY.CRA.TOO.HND |
| Physical | Crafted | Tools | Weapons | PHY.CRA.TOO.WEA |
| Abstract | Player | Achievements | Milestones | ABS.PLY.ACH.MIL |
| Abstract | Player | Reputation | Faction | ABS.PLY.REP.FAC |
| Abstract | Economic | Currency | Primary | ABS.ECO.CUR.PRI |
| Service | Player | Crafting | Processing | SRV.PLY.CRA.PRO |
| Service | Player | Trading | Marketplace | SRV.PLY.TRA.MAR |
| Service | System | World | Environmental | SRV.SYS.WOR.ENV |

### Sample Entity Classifications

| Entity | Full Taxonomy Path | ID |
|--------|-------------------|-----|
| Apple | Physical.Natural.Flora.Food.Fruits.Pome.Apple | PHY.NAT.FLO.FOO.FRU.POM.APP |
| Iron Ore | Physical.Natural.Minerals.Ores.Base.Iron.IronOre | PHY.NAT.MIN.ORE.BAS.IRN.ORE |
| Iron Ingot | Physical.Crafted.Materials.Processed.Metals.Base.IronIngot | PHY.CRA.MAT.PRO.MET.BAS.ING |
| Long Sword | Physical.Crafted.Tools.Weapons.Melee.Swords.LongSword | PHY.CRA.TOO.WEA.MEL.SWO.LON |
| Health Potion | Physical.Crafted.Consumables.Potions.Health.Minor | PHY.CRA.CON.POT.HEA.MIN |
| Forge | Physical.Crafted.Structures.Commercial.Crafting.Forge | PHY.CRA.STR.COM.CRA.FOR |
| Trade Coin | Abstract.Economic.Currency.Primary.TradeCoin | ABS.ECO.CUR.PRI.TC |
| Smithing Skill | Abstract.Player.Skills.Production.Crafting.Smithing | ABS.PLY.SKI.PRO.CRA.SMI |

---

## Part X: Conclusion

This systematic entity classification provides BlueMarble with a robust, scalable framework for organizing all game entities. By following biological taxonomy principles, we ensure:

- **Consistency**: All entities follow the same classification logic
- **Discoverability**: Players can explore entity relationships naturally
- **Extensibility**: New entities fit seamlessly into existing structure
- **Performance**: Hierarchical queries optimize database operations
- **Modding**: Clear extension points for community content

The 7-tier hierarchy (Domain → Realm → Kingdom → Class → Order → Family → Species) provides sufficient granularity while remaining intuitive and maintainable. Integration with existing systems (fruits, biomes, materials) demonstrates practical application across diverse game mechanics.

### Next Steps

1. ✅ Complete taxonomy framework design
2. [ ] Implement database schema changes
3. [ ] Classify all existing entities
4. [ ] Update game systems to use taxonomy
5. [ ] Build player-facing discovery features
6. [ ] Create modding documentation
7. [ ] Validate with community feedback

---

**References**:
- Biological Taxonomy: [Domain (Biology) - Wikipedia](https://cs.wikipedia.org/wiki/Dom%C3%A9na_(biologie))
- Existing Systems: `fruit-conservation-decay-research.md`, `pome-fruits-comprehensive-database.md`
- Technical Implementation: `database-schema-design.md`, `system-architecture-design.md`
