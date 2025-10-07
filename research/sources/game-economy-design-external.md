# Game Economy Design: Auctions, Transport, Fees, Spoilage, Seasonality

**Date**: 2025-01-18  
**Source**: External research document provided by @Nomoos  
**Status**: Processed and integrated into research  
**Related**: Multi-step commodity swap routing, extended auction system

---

## Purpose & Scope

This document defines the auction-based trading and related economic mechanics for BlueMarble, including listing tiers, transport logistics, dynamic fees, item spoilage, and seasonal effects. The goal is a **hybrid, player-driven market** where regional auctions (from local to global) create meaningful trade opportunities while requiring physical transport of goods. Auctions generate economic sinks via fees, and spoilage prevents infinite item hoarding. Seasonal cycles modulate supply and demand to add strategic depth. This design balances player convenience against geographic realism, supporting professions like traders and couriers.

## Core Concepts

### Auction Tiers

Auctions are organized by **tier/zones** (e.g. local, city, regional, national, global) with increasing listing fees and commission rates for broader reach. Local markets have low fees (e.g. 1-3%) while global markets carry higher fees (e.g. up to 6-8%). Each tier has parameters:

- **Listing Fee:** Fixed charge for creating an auction (scaled by tier).  
- **Commission Rate:** Percentage of sale price taken by system as a fee.  
- **Visibility:** Determines buyer reach (e.g. citywide vs worldwide).  

Higher tiers cost more to list but expose items to more buyers.

**Alignment with Current Implementation:**
- ✅ Our system implements Local (1.5%), Regional (3%), Global (7%) tiers
- ✅ Listing fees and commission rates are included
- ✅ Visibility concept aligns with market tier reach

### Auction Nodes (Market Regions)

Each geographic **node** (market region) operates as its own auction house. For example, towns or cities may host separate market nodes. Items listed in one node must be **physically transported** to other nodes (via player or NPC couriers) to reach buyers elsewhere. Nodes embody regional specialization: resource-rich areas supply local goods cheaper, scarce resources fetch premiums. Major cities become trading hubs. Connecting nodes (e.g. via roads or shipping lanes) requires investment (like contribution points) and enables trade routes.

**Alignment with Current Implementation:**
- ✅ Multi-step routing supports inter-node trading
- ✅ Regional specialization concept is present in our market pairs
- 📋 TODO: Implement contribution points for trade route establishment
- 📋 TODO: Add node connectivity system

### Transport Mechanics

Transport moves sold goods between nodes. Multiple methods exist (e.g. walking, horse-cart, truck, ship, plane), each with capacity, speed, cost, and risk attributes. For example, a cargo ship has high capacity and moderate cost (`fuel_cost + port_fees`) with medium risk. Transport time and cost depend on distance and speed (`time = distance/speed`). Weather and terrain modify travel: harsh terrain or storms increase time and risk. Longer or more valuable shipments carry more **loss risk**: 

```
risk ≈ base_risk × (1 + distance/10000) × (1 + terrain_difficulty)
```

Guild or NPC convoy options (with guards) reduce risk but raise costs. Insurance can be purchased to mitigate loot loss.

**Alignment with Current Implementation:**
- ✅ Transport fees implemented with distance-based calculation
- ✅ Multiple guard tiers (None, Basic, Standard, Premium)
- ✅ Risk/cost tradeoffs present (15% to 0.5% ambush chance)
- 📋 TODO: Add multiple transport methods (walking, cart, ship, plane)
- 📋 TODO: Implement weather/terrain modifiers
- 📋 TODO: Add insurance system for transport

**Enhanced Transport Formula:**
```javascript
transportCost = (distance / speed) × (
    baseCostPerKm × 
    vehicleCapacityMultiplier × 
    terrainMultiplier × 
    weatherMultiplier
)

transportRisk = baseRisk × 
    (1 + distance/10000) × 
    (1 + terrainDifficulty) × 
    weatherRiskFactor × 
    (1 - guardReduction)
```

### Seasonality

A four-season cycle affects supply, demand, and trade routes. For example:

- **Summer harvests** boost crop supply (e.g. +80%) and drop grain prices (to ~60%)
- **Drought conditions** drastically raise prices (×2 for grain) and cut yields in half
- **Winter** spikes fuel demand and offers natural cold storage (snow "preserves food")
- Seasons can freeze roads/rivers (halting certain routes) and flood terrain (closing others)

These cyclical modifiers encourage timing trading and production (e.g. storing food or mining in appropriate seasons).

**Alignment with Current Implementation:**
- ✅ Seasonal effects on deterioration (summer 1.3x, winter 0.5x)
- ✅ Seasonal travel modifiers implemented
- 📋 TODO: Implement seasonal supply/demand price modifiers
- 📋 TODO: Add route availability changes (frozen rivers, flooded paths)
- 📋 TODO: Implement harvest cycles and yields

**Enhanced Seasonal System:**
```javascript
const seasonalEffects = {
    spring: {
        supplyModifiers: {
            vegetables: 1.3,      // Spring planting
            flowers: 1.5
        },
        demandModifiers: {
            seeds: 1.4,
            tools: 1.2
        },
        routeAvailability: {
            'river-trade': true,  // Rivers thawed
            'mountain-pass': false // Still snowed in
        }
    },
    summer: {
        supplyModifiers: {
            grain: 1.8,           // +80% from harvest
            fruits: 1.6,
            vegetables: 1.4
        },
        priceModifiers: {
            grain: 0.6,           // Prices drop to 60%
            fuel: 0.8             // Lower heating demand
        },
        routeAvailability: {
            'all-routes': true
        }
    },
    autumn: {
        supplyModifiers: {
            preservedFood: 1.3,
            harvestGoods: 1.5
        },
        demandModifiers: {
            preservationSupplies: 1.4,
            winterPrep: 1.3
        }
    },
    winter: {
        supplyModifiers: {
            fuel: 1.2,
            winterClothing: 1.1
        },
        demandModifiers: {
            fuel: 1.5,            // Heating demand up
            food: 1.2             // Scarcity
        },
        priceModifiers: {
            freshProduce: 2.0     // ×2 due to scarcity
        },
        routeAvailability: {
            'river-trade': false, // Frozen
            'sea-route': false,   // Ice
            'mountain-pass': false
        },
        preservation: {
            naturalColdStorage: true, // Snow preserves food
            deteriorationMultiplier: 0.5
        }
    }
};
```

### Spoilage

Perishable goods (food, alchemical ingredients, etc.) degrade over time if not sold or consumed. Each perishable item has a **shelf-life** or freshness meter. A simple model:

```
freshness_new = freshness_old × e^(–spoilage_rate × time_elapsed)
```

Spoilage rate can be increased by temperature/humidity or decreased by preservation methods (cold storage, salting). Snowy/colder seasons naturally preserve food longer. If freshness reaches zero, the item is spoiled (unsellable or greatly devalued). Spoilage penalizes listing long durations of perishable goods.

**Alignment with Current Implementation:**
- ✅ Deterioration system for perishables (fruits 5%/day, meat 8%/day)
- ✅ Preservation methods (drying, salting, smoking, canning)
- ✅ Seasonal effects on decay (summer 1.3x, winter 0.5x)
- ✅ Freshness-based value calculation
- 📋 TODO: Implement exponential decay model (currently linear)
- 📋 TODO: Add temperature/humidity factors
- 📋 TODO: Implement cold storage facilities

**Enhanced Spoilage Model:**
```javascript
calculateSpoilage(item, timeElapsed, environment) {
    const baseSpoilageRate = item.spoilageRate; // per hour
    
    // Environmental factors
    const tempMultiplier = calculateTempMultiplier(environment.temperature);
    const humidityMultiplier = 1 + (environment.humidity - 50) / 100;
    
    // Preservation effects
    const preservationMultiplier = item.preservation 
        ? preservationMethods[item.preservation].decayReduction
        : 1.0;
    
    // Season effects
    const seasonMultiplier = seasons[environment.season].deteriorationMultiplier;
    
    // Exponential decay model
    const effectiveRate = baseSpoilageRate × 
        tempMultiplier × 
        humidityMultiplier × 
        seasonMultiplier × 
        preservationMultiplier;
    
    const newFreshness = item.freshness × Math.exp(-effectiveRate × timeElapsed);
    
    // Value degradation based on freshness
    const valueMultiplier = Math.pow(newFreshness, 2); // Quadratic value loss
    
    return {
        freshness: Math.max(0, newFreshness),
        currentValue: item.baseValue × valueMultiplier,
        isSpoiled: newFreshness < 0.1 // Below 10% freshness
    };
}
```

---

## Discovery Sources Referenced

The original document contains references to source materials (formatted as 【59†L29-L33】, etc.). These appear to reference:

1. **Source 39**: Auction tier fee structures (lines 149-158, 169-177)
2. **Source 58**: Transport mechanics (lines 563-572, 590-599, 613-620, 621-629, 645-653)
3. **Source 59**: Auction system scope and design (lines 29-33)
4. **Source 60**: Seasonal effects (lines 857-861, 877-881, 877-885)
5. **Source 61**: Winter preservation (lines 1-4)
6. **Source 62**: Regional specialization (lines 1172-1181)
7. **Source 63**: Commission rates (lines 128-132)

These sources should be investigated and catalogued for comprehensive research integration.

---

## Integration Recommendations

### Immediate Enhancements (Priority 1)
1. **Exponential Spoilage Model** - Replace linear decay with exponential for realism
2. **Seasonal Supply/Demand** - Implement price modifiers based on season
3. **Route Availability** - Add seasonal route restrictions (frozen rivers, etc.)
4. **Temperature/Humidity** - Environmental factors for spoilage

### Medium-Term Features (Priority 2)
1. **Multiple Transport Methods** - Walking, cart, ship, plane with different attributes
2. **Insurance System** - Optional insurance for transport with premium calculation
3. **Cold Storage Facilities** - Player-owned preservation infrastructure
4. **Contribution Points** - Investment system for trade route establishment

### Long-Term Features (Priority 3)
1. **Dynamic Node System** - Player-founded market nodes
2. **Guild Convoys** - Organized group transport with shared costs/risks
3. **Weather Events** - Storms, droughts affecting specific routes/seasons
4. **Harvest Cycles** - Time-gated production based on seasons

---

## Comparison with Current Implementation

| Feature | Described | Implemented | Status |
|---------|-----------|-------------|--------|
| Auction Tiers | Local, City, Regional, National, Global | Local, Regional, Global | ✅ Partial |
| Tier Fees | 1-8% range | 1.5-7% range | ✅ Complete |
| Transport Fees | Distance-based | Distance-based | ✅ Complete |
| Guard System | Multiple tiers | 4 tiers (None-Premium) | ✅ Complete |
| Spoilage | Exponential decay | Linear decay | ⚠️ Different model |
| Preservation | Multiple methods | 4 methods | ✅ Complete |
| Seasonality | Supply/demand/routes | Decay modifiers only | ⚠️ Partial |
| Transport Methods | Multiple vehicles | Single abstraction | ❌ Not implemented |
| Insurance | Optional | Not implemented | ❌ Not implemented |
| Cold Storage | Facility-based | Seasonal effect only | ⚠️ Simplified |

---

## References

- Original document: `game_economy_design.md` (provided by @Nomoos)
- Related: `extended-auction-system-transport-fees-deterioration.md`
- Related: `multi-step-commodity-swap-routing.md`
- Related: `auction-house-systems-local-global-transport.md`

## License

Part of the BlueMarble.Design research repository.  
All rights reserved.
