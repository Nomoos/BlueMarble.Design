# Player Freedom and Protection Mechanics

**Version:** 1.0  
**Date:** 2025-01-06  
**Status:** Draft  
**Owner:** BlueMarble Gameplay Team

## Overview

This document describes the comprehensive player freedom mechanics in BlueMarble, focusing on resource extraction, building, terraforming, trading, and economic systems that enable meaningful player-driven gameplay.

## Core Philosophy

**Player Freedom Principles:**
1. **Meaningful Choices**: Every decision has consequences
2. **Economic Balance**: Natural constraints prevent exploitation
3. **Sandbox Elements**: Freedom within structured systems
4. **Emergent Gameplay**: Player interactions create unique stories
5. **Risk vs Reward**: Greater freedom requires greater protection

## Mining System

### Resource Extraction

Players can claim territory and extract valuable resources from the earth.

#### Mining Operations

```javascript
{
  mineId: string,
  owner: playerId,
  location: { x: number, y: number },
  
  resources: {
    type: "iron" | "gold" | "crystal" | "coal" | "copper" | "silver",
    totalDeposit: number, // total units available
    extracted: number, // units already extracted
    quality: number, // 1-100 (affects processing yield)
    exhaustion: number // 0-1 (1 = depleted)
  },
  
  operations: {
    active: boolean,
    workers: number, // NPCs or players working
    extractionRate: number, // units per hour
    efficiency: number, // 0-1 (tool quality, worker skill)
    maintenanceCost: number // currency per hour
  },
  
  equipment: {
    tools: ["pickaxe", "drill", "explosives"],
    toolCondition: number, // 0-100
    upgrades: ["automation_level_1", "safety_systems"]
  }
}
```

#### Territory Claiming

- **Claim Size**: 100m x 100m to 1km x 1km
- **Claim Cost**: Based on resource richness and location
- **Claim Duration**: Permanent (with maintenance)
- **Claim Defense**: Requires active protection

#### Resource Types

| Resource | Rarity | Base Value | Processing Required |
|----------|--------|------------|---------------------|
| Iron | Common | 10 | Yes - Smelting |
| Copper | Common | 12 | Yes - Smelting |
| Coal | Common | 8 | No |
| Silver | Uncommon | 50 | Yes - Refining |
| Gold | Rare | 100 | Yes - Refining |
| Crystal | Very Rare | 500 | Yes - Cutting |

### Mining Economics

**Initial Investment:**
- Territory Claim: 1,000 - 10,000 currency
- Basic Tools: 500 currency
- Initial Workers: 300 currency per worker

**Operating Costs:**
- Tool Maintenance: 50-100 currency per day
- Worker Wages: 10-15 currency per hour per worker
- Protection Systems: Variable (see protection systems)

**Revenue:**
- Extraction Rate: 10-100 units per hour (based on efficiency)
- Market Price: Varies with supply/demand
- Processing Bonus: +20-50% value for processed resources

## Building System

### Structure Types

Players can construct various buildings to support their operations.

#### Production Buildings

**Mines**
- Purpose: Resource extraction
- Build Cost: 2,000 currency + materials
- Build Time: 4 hours
- Maintenance: 100 currency per day

**Factories**
- Purpose: Resource processing and crafting
- Build Cost: 5,000 currency + materials
- Build Time: 8 hours
- Maintenance: 200 currency per day
- Processing Rate: 50-200 units per hour

**Refineries**
- Purpose: Advanced material processing
- Build Cost: 10,000 currency + materials
- Build Time: 12 hours
- Maintenance: 300 currency per day
- Processing Bonus: +50% output value

#### Storage Buildings

**Warehouses**
- Purpose: Inventory expansion
- Build Cost: 3,000 currency + materials
- Storage Capacity: 10,000 units
- Maintenance: 50 currency per day

**Vaults**
- Purpose: Secure currency storage
- Build Cost: 8,000 currency + materials
- Security: Built-in defense (40% effectiveness)
- Maintenance: 150 currency per day

#### Defense Buildings

**Watch Towers**
- Purpose: Area surveillance and defense
- Build Cost: 4,000 currency + materials
- Range: 100m
- Effectiveness: 60%
- Maintenance: 100 currency per day

**Defense Turrets**
- Purpose: Automated threat elimination
- Build Cost: 8,000 currency + materials
- Range: 150m
- Effectiveness: 65%
- Ammunition Cost: 50 currency per day

**Guard Houses**
- Purpose: Housing for hired guards
- Build Cost: 3,000 currency + materials
- Capacity: 5 guards
- Morale Bonus: +10% guard effectiveness

#### Support Buildings

**Housing**
- Purpose: Worker accommodation
- Build Cost: 2,000 currency + materials
- Capacity: 10 workers
- Efficiency Bonus: +15% worker productivity

**Market Stalls**
- Purpose: Player-to-player trading
- Build Cost: 1,500 currency + materials
- Features: Automated sales while offline
- Commission: 5% of sales

### Construction Mechanics

```javascript
{
  buildingId: string,
  type: string,
  owner: playerId,
  location: { x: number, y: number },
  
  construction: {
    blueprint: string,
    progress: number, // 0-100%
    requiredResources: {
      wood: 100,
      stone: 200,
      iron: 50
    },
    providedResources: {
      wood: 80,
      stone: 200,
      iron: 30
    },
    workersAssigned: number,
    estimatedCompletion: timestamp
  },
  
  status: {
    operational: boolean,
    health: number, // 0-100
    efficiency: number, // 0-1
    lastMaintenance: timestamp
  },
  
  upgrades: {
    available: ["automation", "expansion", "reinforcement"],
    active: ["basic_tools"],
    upgradeProgress: number
  }
}
```

## Terraforming System

### Landscape Modification

Players can modify terrain for strategic and aesthetic purposes.

#### Terraforming Operations

**Excavation**
- Remove earth to create pits, moats, trenches
- Cost: 10 currency per cubic meter
- Speed: 100m³ per hour with basic equipment
- Uses: Defensive positions, foundations, mining access

**Fill/Elevation**
- Add earth to raise terrain, create walls, flatten areas
- Cost: 15 currency per cubic meter
- Speed: 80m³ per hour with basic equipment
- Uses: Defensive walls, platforms, landscaping

**Water Management**
- Create canals, ponds, drainage systems
- Cost: 50 currency per meter (complex)
- Requires: Surveying skill
- Uses: Transportation, defense, aesthetics

#### Environmental Impact

```javascript
{
  operationId: string,
  type: "excavation" | "fill" | "water_management",
  location: { x: number, y: number },
  area: number, // square meters affected
  
  effects: {
    drainageImpact: number, // -1 to 1
    erosionRisk: number, // 0-1
    vegetationImpact: number, // -1 to 1
    aestheticScore: number // 1-100
  },
  
  approvals: {
    nearbyPlayers: [playerId], // affected neighbors
    allianceApproval: boolean,
    environmentalCheck: boolean
  }
}
```

### Terraforming Economics

**Equipment Costs:**
- Basic Shovel: 50 currency
- Excavator: 5,000 currency
- Advanced Machinery: 20,000 currency

**Operation Costs:**
- Manual Labor: 5 currency per m³
- Machine Operation: 10 currency per m³ (faster)
- Surveying: 100 currency per operation

## Trading System

### Player-to-Player Trading

Direct trading between players with optional NPC intermediaries.

#### Trade Structure

```javascript
{
  tradeId: string,
  seller: playerId,
  buyer: playerId | "npc" | null,
  
  items: [
    {
      itemId: string,
      itemName: string,
      quantity: number,
      pricePerUnit: number,
      totalPrice: number
    }
  ],
  
  terms: {
    totalPrice: number,
    currency: "gold" | "silver",
    paymentMethod: "immediate" | "escrow",
    
    delivery: {
      method: "pickup" | "delivery" | "market_stall",
      location: { x: number, y: number },
      deliveryTime: timestamp,
      deliveryFee: number
    }
  },
  
  status: "pending" | "accepted" | "in_transit" | "completed" | "cancelled",
  
  escrow: {
    enabled: boolean,
    depositAmount: number,
    releasedToSeller: boolean
  }
}
```

#### Market System

**Public Marketplace:**
- Players list items for sale
- Automated sales when offline
- 5% commission fee
- Price discovery through supply/demand

**Private Trades:**
- Direct player-to-player negotiation
- No commission fees
- Requires both parties online (or escrow)
- Higher risk, higher freedom

### NPC Trading Automation

NPCs can participate in trades autonomously.

```javascript
{
  npcTraderId: string,
  npcType: "trader",
  
  tradingStrategy: {
    buyOrders: [
      {
        item: "iron_ore",
        maxPrice: 15,
        quantity: 100,
        autoAccept: true
      }
    ],
    
    sellOrders: [
      {
        item: "processed_iron",
        minPrice: 25,
        quantity: 50,
        autoAccept: true
      }
    ],
    
    priceFlexibility: number, // 0-1 (negotiation)
    preferredPlayers: [playerId], // reputation-based
    blacklistedPlayers: [playerId]
  }
}
```

## Economic Balance Systems

### Anti-Exploitation Measures

#### Resource Scarcity

**Finite Deposits:**
- All resource nodes have limited supply
- Exhaustion creates need for exploration
- Overexploitation damages ecosystem

**Regeneration Mechanics:**
- Resources slowly regenerate over time
- Sustainable harvesting encouraged
- Penalties for over-extraction

#### Cost Structures

**Scaling Costs:**
```javascript
function calculateOperatingCost(operation) {
  const baseCost = operation.baseMaintenanceCost;
  const scaleFactor = 1 + (operation.size / 1000);
  const distanceFactor = 1 + (operation.distanceFromCapital / 10000);
  
  return baseCost * scaleFactor * distanceFactor;
}
```

**Maintenance Requirements:**
- All buildings require regular maintenance
- Neglect leads to efficiency loss
- Complete neglect leads to decay and collapse

#### Market Regulation

**Dynamic Pricing:**
```javascript
function calculateMarketPrice(item, supply, demand) {
  const basePrice = item.baseValue;
  const supplyFactor = 1 / (supply / 1000);
  const demandFactor = demand / 1000;
  
  return basePrice * supplyFactor * demandFactor;
}
```

**Price Floors/Ceilings:**
- Minimum prices prevent dumping
- Maximum prices prevent price gouging
- NPC traders enforce reasonable ranges

### Emergent Economy

#### Supply Chain Gameplay

**Vertical Integration:**
- Mine extraction → Factory processing → Market sales
- Each stage adds value
- Specialization creates interdependence

**Trade Routes:**
- Transportation between locations
- Guard escort requirements
- Piracy risk creates job opportunities

**Economic Niches:**
- Resource extractors
- Processors/crafters
- Traders/merchants
- Guards/protectors
- Builders/constructors

## Player Interaction Systems

### Cooperative Mechanics

**Shared Operations:**
- Co-owned mines or factories
- Profit sharing agreements
- Shared defense costs

**Contracts:**
- Formal agreements between players
- Enforced by game systems
- Breach penalties

**Guilds/Alliances:**
- Collective resource pooling
- Shared facilities
- Economic cooperation

### Competitive Mechanics

**Resource Competition:**
- Limited resource nodes
- Territory disputes
- Market competition

**Economic Warfare:**
- Price undercutting
- Supply monopolies
- Trade embargoes

**Raiding:**
- Attack rival operations
- Steal resources
- Requires overcoming defenses

## Integration with Protection Systems

### Protection Requirements by Activity

| Activity | Recommended Protection | Risk Level |
|----------|----------------------|------------|
| Mining | Guards + Towers | High |
| Factory Operation | Guards + Alarms | Medium |
| Trading (Marketplace) | Market Security | Low |
| Terraforming | Optional | Low |
| Resource Transport | Guard Escort | Very High |

### Economic Impact of Protection

**Cost-Benefit Analysis:**
```javascript
function calculateProtectionROI(operation, protection) {
  const operationValue = operation.revenue - operation.costs;
  const protectionCost = protection.hourlyRate * 24;
  const lossRiskReduction = operation.riskValue * protection.effectiveness;
  
  const netBenefit = lossRiskReduction - protectionCost;
  const roi = netBenefit / protectionCost;
  
  return {
    worth: netBenefit > 0,
    roi: roi,
    netBenefit: netBenefit
  };
}
```

## Progression Systems

### Skill Advancement

**Mining Skills:**
- Extraction Efficiency: Faster resource gathering
- Ore Identification: Better quality assessment
- Safety: Reduced accident risk

**Building Skills:**
- Construction Speed: Faster building
- Material Efficiency: Less waste
- Structural Integrity: More durable buildings

**Trading Skills:**
- Negotiation: Better prices
- Market Analysis: Price prediction
- Logistics: Cheaper transport

### Unlockable Features

**Tier 1 (Beginner):**
- Basic mining (iron, coal)
- Simple buildings (storage, housing)
- Local trading

**Tier 2 (Intermediate):**
- Advanced mining (gold, silver)
- Production buildings (factories)
- Regional trading

**Tier 3 (Advanced):**
- Rare resources (crystals)
- Defense structures
- Automated trading systems

**Tier 4 (Expert):**
- Territory expansion
- Guild operations
- Economic empire management

## Implementation Requirements

### Server Systems

- Resource node generation and tracking
- Building construction and status management
- Trading and escrow systems
- Economic simulation and price calculations
- Player interaction validation

### Client Interface

- Interactive building placement
- Resource visualization
- Trade interface
- Economic dashboards
- Market analytics

### Database Schema

```sql
-- Resource nodes
CREATE TABLE resource_nodes (
  id UUID PRIMARY KEY,
  type VARCHAR(50),
  location POINT,
  total_deposit INTEGER,
  extracted INTEGER,
  quality INTEGER,
  owner_id UUID
);

-- Buildings
CREATE TABLE buildings (
  id UUID PRIMARY KEY,
  type VARCHAR(50),
  owner_id UUID,
  location POINT,
  health INTEGER,
  operational BOOLEAN,
  construction_progress INTEGER
);

-- Trade orders
CREATE TABLE trade_orders (
  id UUID PRIMARY KEY,
  seller_id UUID,
  buyer_id UUID,
  items JSONB,
  total_price DECIMAL,
  status VARCHAR(20),
  created_at TIMESTAMP
);
```

## References

- [Player Protection Systems](./spec-player-protection-systems.md)
- [Economy Systems](../systems/economy-systems.md)
- [Gameplay Systems](../systems/gameplay-systems.md)
