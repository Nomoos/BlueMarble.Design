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

## SOURCES Checklist

- [x] **Source 39**: Auction tier fee structures - Processed in batch 1
- [x] **Source 58**: Transport mechanics - Processed in batch 1
- [x] **Source 59**: Auction system scope and design - Processed in batch 1
- [x] **Source 60**: Seasonal effects - Processed in batch 1
- [ ] **Source 61**: Winter preservation
- [ ] **Source 62**: Regional specialization
- [ ] **Source 63**: Commission rates

---

## Source Analysis

### Batch 1: Sources 39, 58, 59, 60

#### Source 39: Auction Tier Fee Structures

**Title/ID:** Source 39 - Auction tier fee structures (lines 149-158, 169-177)

**Key Facts:**

- Defines tiered auction house system with escalating fees based on geographic reach
- Local markets: 1-3% commission rates
- Global markets: 6-8% commission rates
- Fee structure includes both listing fees and commission rates
- Visibility parameter determines buyer reach (citywide vs worldwide)

**Implications/Risks:**

- Fee balance critical to encourage market usage while providing currency sink
- Too high fees may discourage global market usage, limiting intercontinental trade
- Too low fees may fail to control inflation adequately
- Regional market arbitrage may be exploited if fee differentials are too large

**Action Items:**

- ✅ Implemented in design/auction-economy.md with three-tier system (Local 1.5%, Regional 3%, Global 7%)
- ✅ Added fee comparison tables in assets/diagrams/marketplace/auction-tier-system.md
- Economic balancing testing needed during MVP phase
- Monitor player behavior to adjust fee rates based on actual market usage patterns

---

#### Source 58: Transport Mechanics

**Title/ID:** Source 58 - Transport mechanics (lines 563-572, 590-599, 613-620, 621-629, 645-653)

**Key Facts:**

- Multiple transport methods with different capacity, speed, cost, and risk attributes
- Cargo ship example: high capacity, moderate cost (fuel_cost + port_fees), medium risk
- Transport time formula: `time = distance/speed`
- Weather and terrain modify travel time and risk
- Risk formula: `risk ≈ base_risk × (1 + distance/10000) × (1 + terrain_difficulty)`
- Guild/NPC convoy options reduce risk but increase costs
- Insurance available to mitigate loss

**Implications/Risks:**

- Complex system requiring significant server-side calculation for route simulation
- Balance needed between realism and player convenience
- Risk mechanics must be engaging without being punitive
- Weather/terrain systems add computational overhead
- Insurance pricing must balance coverage with premium costs

**Action Items:**

- ✅ Implemented 5 transport methods in design/auction-economy.md (walking, cart, wagon, ship, airship)
- ✅ Transport cost and risk formulas documented
- Need to implement weather/terrain modifier system (Priority 2)
- Need to add insurance system for transport (Priority 2)
- Develop guild convoy mechanics (Priority 3)
- Performance testing required for real-time route calculations

---

#### Source 59: Auction System Scope and Design

**Title/ID:** Source 59 - Auction system scope and design (lines 29-33)

**Key Facts:**

- Hybrid, player-driven market philosophy
- Regional auctions create meaningful trade opportunities
- Physical transport of goods required (no instant teleportation)
- Auctions generate economic sinks via fees
- Supports professions like traders and couriers

**Implications/Risks:**

- Physical transport requirement may frustrate players expecting instant gratification
- Requires robust courier/transport gameplay to support economy
- Player-driven market dependent on active player base in all regions
- Economic sinks must be calibrated to prevent over-draining currency
- Courier profession viability depends on sufficient trade volume

**Action Items:**

- ✅ Core philosophy integrated throughout design/auction-economy.md
- ✅ Player professions documented (trader, courier, market specialist)
- Need player education on hybrid market benefits vs instant-access systems
- Monitor regional market health to ensure all regions have adequate liquidity
- Develop NPC backup systems for low-population regions
- Create tutorial content explaining transport-based economy

---

#### Source 60: Seasonal Effects

**Title/ID:** Source 60 - Seasonal effects (lines 857-861, 877-881, 877-885)

**Key Facts:**

- Four-season cycle affects supply, demand, and trade routes
- Summer harvests: +80% crop supply, grain prices drop to ~60%
- Drought conditions: ×2 grain prices, 50% yield reduction
- Winter: increased fuel demand, natural cold storage
- Seasons can freeze roads/rivers or flood terrain, closing routes
- Cyclical modifiers encourage strategic timing of trading and production

**Implications/Risks:**

- Seasonal price swings may create market instability if not properly dampened
- Route closures during winter could isolate regions economically
- Players may hoard goods to exploit seasonal price changes
- Extreme weather events could disrupt planned trade routes
- Seasonal mechanics add complexity to player decision-making

**Action Items:**

- ✅ Implemented complete four-season system in design/auction-economy.md
- ✅ Supply/demand modifiers documented for all seasons
- ✅ Route availability changes included in design
- Need to implement seasonal route restrictions (Priority 1)
- Develop price smoothing mechanisms to prevent excessive volatility
- Add seasonal event notifications for players
- Monitor for hoarding exploits and implement countermeasures if needed
- Create seasonal trading guides for player education

---

### Batch 1 Summary

Processed sources 39, 58, 59, and 60 covering auction tier structures, transport mechanics, system scope, and seasonal
effects. These four sources form the foundational pillars of the auction economy system:

**Cross-Source Insights:**

- The tiered fee structure (S39) works synergistically with transport costs (S58) to create meaningful trade-offs
  between local convenience and global reach
- Physical transport requirement (S59) gains depth through seasonal route changes (S60), creating strategic timing
  considerations
- Economic sinks from auction fees (S39) combined with transport costs (S58) and spoilage provide robust inflation
  control
- Seasonal effects (S60) interact with preservation methods and transport times (S58) to add complexity without being
  overwhelming

**Key Integration Points:**

- All four sources successfully integrated into design/auction-economy.md
- Visual diagrams created to illustrate tier hierarchy and seasonal route changes
- Implementation roadmap prioritizes core mechanics (fees, transport, seasons) in MVP

**Next Steps:**

- Process remaining sources 61-63 in next batch to complete source analysis
- Begin technical feasibility review with development team
- Develop database schema for auction, transport, and seasonal systems
- Create UI mockups incorporating seasonal indicators and transport options

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
