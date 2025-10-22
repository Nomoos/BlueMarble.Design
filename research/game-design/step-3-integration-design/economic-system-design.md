# Economic System Design: Port Royale-Inspired Trading System

## Overview

This document outlines a comprehensive economic simulation system for BlueMarble, inspired by Port Royale and other classic trading games. The system features 11 commodities across 4 categories, dynamic supply/demand pricing, production chains, and ship-based trading with 4 vessel types.

## Commodity System

### 11 Commodities Across 4 Categories

#### Raw Materials (3 commodities)
1. **Iron Ore**
   - Base Price: 50 credits/ton
   - Weight: 1000 kg/unit
   - Volume: 0.5 m³/unit
   - Production: Mining operations in iron-rich regions
   - Uses: Steel production, tool manufacturing

2. **Coal**
   - Base Price: 30 credits/ton
   - Weight: 800 kg/unit
   - Volume: 0.6 m³/unit
   - Production: Coal mining in sedimentary basins
   - Uses: Fuel for smelting, power generation

3. **Timber**
   - Base Price: 25 credits/unit
   - Weight: 600 kg/unit
   - Volume: 0.8 m³/unit
   - Production: Logging in forested regions
   - Uses: Construction, shipbuilding, furniture

#### Processed Materials (3 commodities)
4. **Steel**
   - Base Price: 120 credits/ton
   - Weight: 1000 kg/unit
   - Volume: 0.3 m³/unit
   - Production: Smelting iron ore with coal (2 Iron Ore + 1 Coal → 1 Steel)
   - Uses: Ship construction, tools, advanced machinery

5. **Planks**
   - Base Price: 45 credits/unit
   - Weight: 400 kg/unit
   - Volume: 0.4 m³/unit
   - Production: Sawmill processing (2 Timber → 3 Planks)
   - Uses: Construction, shipbuilding, furniture

6. **Cloth**
   - Base Price: 60 credits/roll
   - Weight: 50 kg/unit
   - Volume: 0.2 m³/unit
   - Production: Textile mills (requires cotton/flax from agriculture)
   - Uses: Sails, clothing, trade goods

#### Manufactured Goods (3 commodities)
7. **Tools**
   - Base Price: 200 credits/set
   - Weight: 100 kg/unit
   - Volume: 0.15 m³/unit
   - Production: Blacksmith (1 Steel + 1 Timber → 2 Tools)
   - Uses: Mining efficiency, construction, resource extraction

8. **Ships Components**
   - Base Price: 500 credits/set
   - Weight: 500 kg/unit
   - Volume: 0.5 m³/unit
   - Production: Shipyard (2 Steel + 3 Planks + 1 Cloth → 1 Ship Component)
   - Uses: Ship construction and repair

9. **Furniture**
   - Base Price: 150 credits/piece
   - Weight: 200 kg/unit
   - Volume: 0.6 m³/unit
   - Production: Carpenter (2 Planks + 1 Tools → 3 Furniture)
   - Uses: Settlement development, luxury trade

#### Luxury Goods (2 commodities)
10. **Spices**
    - Base Price: 300 credits/crate
    - Weight: 20 kg/unit
    - Volume: 0.1 m³/unit
    - Production: Limited regional availability
    - Uses: High-value trade, no production chain

11. **Fine Art**
    - Base Price: 800 credits/piece
    - Weight: 50 kg/unit
    - Volume: 0.2 m³/unit
    - Production: Artisan guilds in major cities
    - Uses: Luxury trade, settlement prestige

## Dynamic Market System

### Supply and Demand Pricing

The market uses real-time supply and demand tracking to adjust prices dynamically:

```javascript
calculatePrice(commodity, region) {
  const basePrice = commodity.basePrice;
  const supply = region.market.getSupply(commodity);
  const demand = region.market.getDemand(commodity);
  
  // Supply/demand ratio with dampening
  const ratio = demand / Math.max(supply, 1);
  const priceMultiplier = Math.pow(ratio, 0.5); // Square root dampening
  
  // Regional specialization bonus
  const specialization = region.specialization[commodity.type] || 1.0;
  
  // Calculate final price with caps
  let finalPrice = basePrice * priceMultiplier / specialization;
  finalPrice = Math.max(finalPrice, basePrice * 0.5);  // Floor: 50% of base
  finalPrice = Math.min(finalPrice, basePrice * 3.0);  // Ceiling: 300% of base
  
  return Math.round(finalPrice);
}
```

### Regional Specialization

Each region has natural advantages for certain commodities:

- **North America East**: Iron Ore (80% price), Coal (85% price), Timber (90% price)
- **Europe**: Steel (85% price), Tools (80% price), Ships Components (85% price)
- **Asia**: Cloth (75% price), Spices (70% price), Fine Art (80% price)
- **South America**: Timber (75% price), Furniture (85% price)
- **Africa**: Luxury Goods specialization (all luxury 85% price)
- **Oceania**: Balanced market (100% average prices)

### Market Events

Random events affect supply and demand:

- **Natural Disasters**: -30% supply for affected commodities
- **Technological Breakthroughs**: +20% production efficiency
- **Trade Agreements**: +15% demand in partner regions
- **Resource Depletion**: Gradual supply reduction over time
- **Seasonal Variations**: ±10% demand based on time of year

## Production Chains

### Chain Dependencies

```
Raw Materials → Processed Materials → Manufactured Goods → Trade

Iron Ore + Coal → Steel → Tools → Enhanced Mining
Timber → Planks → Furniture → Settlement Development
Steel + Planks + Cloth → Ship Components → New Vessels
```

### Production Time and Efficiency

Each production step requires time and facilities:

- **Smelting** (Iron Ore → Steel): 6 game hours, requires Smelter
- **Sawmill** (Timber → Planks): 3 game hours, requires Sawmill
- **Manufacturing** (Steel → Tools): 8 game hours, requires Blacksmith
- **Shipyard** (Components → Ships): 24 game hours, requires Shipyard
- **Crafting** (Planks → Furniture): 5 game hours, requires Carpenter

## Ship-Based Trading System

### 4 Vessel Types

#### 1. Coastal Trader (Sloop)
- **Capacity**: 50 tons (50 units)
- **Speed**: 15 knots (fast)
- **Range**: 500 km
- **Cost**: 2,000 credits
- **Crew**: 5 sailors
- **Best for**: Short-distance, high-value cargo (Spices, Fine Art)
- **Fuel Consumption**: 10 credits/100km

#### 2. Merchant Brig
- **Capacity**: 150 tons (150 units)
- **Speed**: 12 knots (medium)
- **Range**: 1,500 km
- **Cost**: 8,000 credits
- **Crew**: 15 sailors
- **Best for**: Regional trade routes (Steel, Cloth, Tools)
- **Fuel Consumption**: 25 credits/100km

#### 3. Heavy Cargo Ship (Galleon)
- **Capacity**: 400 tons (400 units)
- **Speed**: 8 knots (slow)
- **Range**: 3,000 km
- **Cost**: 25,000 credits
- **Crew**: 40 sailors
- **Best for**: Bulk materials (Iron Ore, Coal, Timber)
- **Fuel Consumption**: 50 credits/100km

#### 4. Fast Clipper
- **Capacity**: 100 tons (100 units)
- **Speed**: 20 knots (very fast)
- **Range**: 2,000 km
- **Cost**: 15,000 credits
- **Crew**: 20 sailors
- **Best for**: Time-sensitive luxury goods
- **Fuel Consumption**: 40 credits/100km

### Automated Route Management

Trade routes can be automated with the following parameters:

```javascript
class TradeRoute {
  constructor(ship, origin, destination) {
    this.ship = ship;
    this.origin = origin;
    this.destination = destination;
    this.buyOrders = [];    // [{commodity, maxPrice, quantity}]
    this.sellOrders = [];   // [{commodity, minPrice, quantity}]
    this.repeatRoute = true;
    this.status = 'active';
  }
  
  calculateProfitability() {
    const distance = calculateDistance(this.origin, this.destination);
    const travelCost = this.ship.calculateTravelCost(distance);
    
    let revenue = 0;
    let costs = travelCost;
    
    for (const order of this.sellOrders) {
      const sellPrice = this.destination.market.getPrice(order.commodity);
      revenue += sellPrice * order.quantity;
    }
    
    for (const order of this.buyOrders) {
      const buyPrice = this.origin.market.getPrice(order.commodity);
      costs += buyPrice * order.quantity;
    }
    
    return revenue - costs;
  }
}
```

## Integration with BlueMarble

### Geographic Integration

The system leverages BlueMarble's existing coordinate and mapping systems:

- **Market Regions**: Mapped to geographic zones (continents, major cities)
- **Trade Routes**: Use real geographic distances for travel time calculation
- **Resource Distribution**: Tied to geological data (ore deposits, forests, coal seams)
- **Weather Effects**: Integrated with BlueMarble's weather system affecting ship speeds

### Modular Architecture (ES6)

```javascript
// markets/Market.js
export class Market {
  constructor(region) {
    this.region = region;
    this.commodities = new Map();
    this.priceHistory = [];
  }
  
  updatePrices() { /* ... */ }
  recordTransaction(commodity, quantity, type) { /* ... */ }
}

// ships/Ship.js
export class Ship {
  constructor(type, owner) {
    this.type = type;
    this.owner = owner;
    this.cargo = new Map();
    this.currentRoute = null;
  }
  
  loadCargo(commodity, quantity) { /* ... */ }
  travel(destination) { /* ... */ }
}

// economy/EconomicSimulation.js
export class EconomicSimulation {
  constructor() {
    this.markets = [];
    this.ships = [];
    this.gameTime = 0;
  }
  
  tick(deltaTime) { /* ... */ }
}
```

### Performance Optimization

- **Market Updates**: Calculate prices every 10 minutes (game time)
- **Spatial Indexing**: Use quadtree for efficient regional market lookups
- **Cached Calculations**: Store commonly accessed values (distances, base prices)
- **Event-Driven**: Only recalculate when transactions occur or events trigger
- **Lazy Loading**: Load market data for visible regions only

### Multiplayer Scalability

- **Regional Sharding**: Each geographic region runs on separate server instance
- **Market Synchronization**: Periodic sync of cross-region price data (every 5 minutes)
- **Transaction Queues**: Buffer market transactions to handle high concurrent load
- **Player Ownership**: Clear ship and cargo ownership with conflict resolution
- **Anti-Cheating**: Server-side validation of all trades and movements

## Prototype Validation

The working prototype demonstrates:

1. **Interactive Economic Simulation**
   - Real-time commodity price updates
   - Supply/demand visualization
   - Production chain animations

2. **Real-time Market Dynamics**
   - Price discovery through trading activity
   - Regional price differences
   - Market event impacts

3. **Trade Route Optimization**
   - Profitability calculator
   - Route planning interface
   - Automated trading logic

4. **Visual Integration**
   - Map-based market display
   - Ship movement visualization
   - Trade flow arrows
   - Price heat maps

## Research Impact

This design provides:

- **Comprehensive Foundation**: Complete economic system architecture
- **Compatibility**: Maintains BlueMarble's ES6 structure and geographic systems
- **Feasibility Validation**: Working prototype proves technical viability
- **Scalability**: Architecture supports multiplayer competitive commerce
- **Educational Value**: Demonstrates real economic principles (supply/demand, specialization, trade)

## Next Steps

1. Implement full prototype with all 11 commodities
2. Create detailed UI mockups for trading interface
3. Develop backend API specifications for multiplayer support
4. Design database schema for persistent market data
5. Create tutorial system for introducing economic mechanics
6. Balance testing with various player scenarios
7. Integration with existing BlueMarble geological systems
