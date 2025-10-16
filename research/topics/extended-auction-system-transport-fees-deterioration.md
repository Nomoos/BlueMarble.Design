# Extended Auction System Design - Transport, Fees, and Deterioration

**Version:** 1.1  
**Date:** 2025-01-18  
**Status:** Research & Design Extension  
**Related:** Multi-Step Commodity Swap Routing System

---

## Overview

This document extends the multi-step commodity swap routing system to incorporate realistic auction house mechanics including transport fees, auctioneer fees, guard costs, market tiers (local/regional/global), goods deterioration, and seasonal effects.

## Feedback Integration

Based on user feedback, the system now considers:

1. **Auctioneer Fees** - Always applied for game auctioneers
2. **Transport Fees** - Distance-based costs for moving goods
3. **Transport Guard Fees** - Security costs based on value and distance
4. **Market Tier System** - Local, regional, and global auction houses
5. **Race-based Global Market Fees** - Higher fees for experimental races
6. **Goods Deterioration/Spoilage** - Time-based decay with preservation options
7. **Seasonal Effects** - Supply/demand and preservation needs vary by season

---

## Market Tier System

### 1. Local Auction Houses

**Characteristics:**
- Available in most settlements
- Low fees (1-2% auctioneer fee)
- Limited commodity selection
- Fast transaction times
- No transport required for same-settlement trades

**Fee Structure:**
```javascript
{
    auctioneerFee: 0.015,        // 1.5%
    listingFee: 0.005,           // 0.5% upfront
    transportFee: 0,             // None (local only)
    guardFee: 0                  // None (local only)
}
```

**Use Cases:**
- Quick trades within settlement
- Common goods (food, basic materials)
- Player-to-player local commerce

### 2. Regional Auction Houses

**Characteristics:**
- Located in major cities
- Medium fees (2-4% auctioneer fee)
- Broader commodity selection
- Connects multiple settlements
- Transport required for inter-settlement trades

**Fee Structure:**
```javascript
{
    auctioneerFee: 0.03,         // 3%
    listingFee: 0.01,            // 1% upfront
    transportFee: distance * 0.001,  // Per km
    guardFee: value * 0.005      // 0.5% of value for basic guards
}
```

**Use Cases:**
- Trading between nearby settlements
- Regional specialization (coastal fish, mountain ores)
- Medium-value goods
- Cross-settlement arbitrage

### 3. Global Auction Houses

**Characteristics:**
- Available only at specific nodes (trade capitals)
- High fees (5-10% auctioneer fee)
- Full commodity access across world
- Long-distance transport required
- Premium security options available

**Fee Structure:**
```javascript
{
    auctioneerFee: 0.07,         // 7% (base)
    racePremium: raceMultiplier, // 1.0-2.0x for experimental races
    listingFee: 0.02,            // 2% upfront
    transportFee: distance * 0.002,  // Per km (higher rate)
    guardFee: value * 0.01,      // 1% for standard guards
    insuranceFee: value * 0.015  // Optional insurance
}
```

**Race-based Fee Multipliers:**
```javascript
const raceFeeMultipliers = {
    'native-inhabitants': 1.0,    // No premium
    'established-settlers': 1.2,  // 20% premium
    'experimental-race-1': 1.5,   // 50% premium
    'experimental-race-2': 2.0,   // 100% premium (experimenting with world)
};
```

**Use Cases:**
- Intercontinental trade
- Rare/luxury goods
- High-value transactions
- Strategic resource movement

---

## Transport System

### Transport Fee Calculation

Transport costs depend on:
1. **Distance** - Linear cost per kilometer
2. **Weight** - Heavier goods cost more
3. **Route quality** - Poor roads increase costs
4. **Weather/Season** - Winter travel costs 20-50% more

```javascript
calculateTransportFee(commodity, from, to, season) {
    const distance = calculateDistance(from, to);
    const weight = commodity.weight;
    const route = getRoute(from, to);
    
    // Base cost per kg per km
    let costPerKgKm = 0.001;
    
    // Route quality modifier
    const qualityMultiplier = {
        'paved-road': 1.0,
        'dirt-road': 1.3,
        'trail': 1.6,
        'wilderness': 2.0
    }[route.quality];
    
    // Seasonal modifier
    const seasonMultiplier = {
        'spring': 1.0,
        'summer': 0.9,
        'autumn': 1.1,
        'winter': 1.5
    }[season];
    
    return distance * weight * costPerKgKm * qualityMultiplier * seasonMultiplier;
}
```

### Transport Guard Fees

Security costs based on cargo value and risk:

```javascript
calculateGuardFee(commodity, amount, distance, guardTier) {
    const cargoValue = commodity.basePrice * amount;
    const risk = calculateRouteRisk(distance);
    
    const guardTiers = {
        'none': {
            baseCost: 0,
            valueFee: 0,
            riskReduction: 0,
            ambushChance: 0.15  // 15% chance
        },
        'basic': {
            baseCost: 50,
            valueFee: 0.005,    // 0.5% of value
            riskReduction: 0.6,
            ambushChance: 0.06  // 6% chance
        },
        'standard': {
            baseCost: 150,
            valueFee: 0.01,     // 1% of value
            riskReduction: 0.8,
            ambushChance: 0.02  // 2% chance
        },
        'premium': {
            baseCost: 500,
            valueFee: 0.02,     // 2% of value
            riskReduction: 0.95,
            ambushChance: 0.005 // 0.5% chance
        }
    };
    
    const tier = guardTiers[guardTier];
    const distanceMultiplier = Math.sqrt(distance / 100); // Scales with sqrt of distance
    
    return tier.baseCost + (cargoValue * tier.valueFee) * distanceMultiplier;
}
```

**Guard Options:**
- **None**: Cheapest but high risk (15% ambush chance)
- **Basic**: Militia guards (6% ambush chance)
- **Standard**: Professional guards (2% ambush chance)
- **Premium**: Military escort (0.5% ambush chance)

---

## Goods Deterioration System

### Commodity Decay Rates

Different commodities deteriorate at different rates:

```javascript
const commodityDecayRates = {
    // Food - High decay
    'fresh-fruit': {
        baseDecayRate: 0.05,        // 5% per day
        seasonalMultiplier: {
            'spring': 0.8,
            'summer': 1.3,          // Faster decay in heat
            'autumn': 0.9,
            'winter': 0.5           // Slower in cold
        },
        preservationMethods: {
            'none': 1.0,
            'salting': 0.3,
            'drying': 0.2,
            'smoking': 0.25,
            'canning': 0.05
        }
    },
    
    // Perishable goods
    'meat': {
        baseDecayRate: 0.08,
        seasonalMultiplier: {
            'spring': 0.9,
            'summer': 1.5,
            'autumn': 1.0,
            'winter': 0.3           // Very slow in winter
        },
        preservationMethods: {
            'none': 1.0,
            'salting': 0.2,
            'drying': 0.15,
            'smoking': 0.1,
            'freezing': 0.02        // Best for meat
        }
    },
    
    // Semi-perishable
    'vegetables': {
        baseDecayRate: 0.03,
        seasonalMultiplier: {
            'spring': 0.8,
            'summer': 1.2,
            'autumn': 0.9,
            'winter': 0.6
        },
        preservationMethods: {
            'none': 1.0,
            'drying': 0.3,
            'pickling': 0.1,
            'canning': 0.05
        }
    },
    
    // Stable goods
    'grains': {
        baseDecayRate: 0.005,       // 0.5% per day
        seasonalMultiplier: {
            'spring': 1.0,
            'summer': 1.0,
            'autumn': 1.0,
            'winter': 1.0
        },
        preservationMethods: {
            'none': 1.0,
            'drying': 0.5,
            'silo-storage': 0.2
        }
    },
    
    // Non-perishable
    'iron-ore': {
        baseDecayRate: 0,
        seasonalMultiplier: {},
        preservationMethods: {}
    },
    
    'wood': {
        baseDecayRate: 0.001,       // Minimal decay (rot)
        seasonalMultiplier: {
            'spring': 1.0,
            'summer': 1.2,          // Humidity causes rot
            'autumn': 0.9,
            'winter': 0.5
        },
        preservationMethods: {
            'none': 1.0,
            'treatment': 0.3        // Wood treatment
        }
    }
};
```

### Deterioration Calculation

```javascript
calculateDeteriorationLoss(commodity, quantity, travelTime, season, preservation) {
    const decayInfo = commodityDecayRates[commodity.type];
    
    if (!decayInfo || decayInfo.baseDecayRate === 0) {
        return 0; // Non-perishable
    }
    
    // Calculate effective decay rate
    const seasonMultiplier = decayInfo.seasonalMultiplier[season] || 1.0;
    const preservationMultiplier = decayInfo.preservationMethods[preservation] || 1.0;
    
    const effectiveDecayRate = decayInfo.baseDecayRate * 
                               seasonMultiplier * 
                               preservationMultiplier;
    
    // Calculate loss over travel time (in days)
    const daysOfTravel = travelTime / 24; // Convert hours to days
    const decayFactor = Math.pow(1 - effectiveDecayRate, daysOfTravel);
    
    const remainingQuantity = quantity * decayFactor;
    const loss = quantity - remainingQuantity;
    
    return {
        originalQuantity: quantity,
        remainingQuantity: Math.floor(remainingQuantity),
        loss: Math.ceil(loss),
        lossPercentage: (loss / quantity * 100).toFixed(2)
    };
}
```

### Preservation Costs

```javascript
const preservationCosts = {
    'salting': {
        costPerUnit: 2,
        timeRequired: 0.5,      // Days
        shelfLifeExtension: 30   // Days
    },
    'drying': {
        costPerUnit: 1,
        timeRequired: 2,
        shelfLifeExtension: 60
    },
    'smoking': {
        costPerUnit: 3,
        timeRequired: 1,
        shelfLifeExtension: 45
    },
    'canning': {
        costPerUnit: 5,
        timeRequired: 0.25,
        shelfLifeExtension: 180
    },
    'freezing': {
        costPerUnit: 4,
        timeRequired: 0,
        shelfLifeExtension: 90,
        ongoingCost: 0.1        // Per day
    }
};
```

---

## Seasonal Effects

### Supply and Demand Fluctuations

```javascript
const seasonalSupplyDemand = {
    'spring': {
        abundant: ['vegetables', 'herbs', 'flowers'],
        scarce: ['preserved-foods', 'winter-grains'],
        priceModifiers: {
            'vegetables': 0.7,      // 30% cheaper (abundant)
            'preserved-foods': 1.3  // 30% more expensive (scarce)
        }
    },
    'summer': {
        abundant: ['fruits', 'fish', 'vegetables'],
        scarce: ['winter-clothes', 'heating-fuel'],
        priceModifiers: {
            'fruits': 0.6,
            'fish': 0.8,
            'heating-fuel': 1.5
        }
    },
    'autumn': {
        abundant: ['grains', 'harvest-vegetables', 'game-meat'],
        scarce: ['summer-fruits', 'flowers'],
        priceModifiers: {
            'grains': 0.7,
            'harvest-vegetables': 0.8,
            'summer-fruits': 1.4
        }
    },
    'winter': {
        abundant: ['preserved-foods', 'winter-clothing', 'heating-fuel'],
        scarce: ['fresh-produce', 'fish'],
        priceModifiers: {
            'fresh-produce': 2.0,   // 100% more expensive
            'heating-fuel': 0.8,    // 20% cheaper (abundant)
            'preserved-foods': 0.9
        }
    }
};
```

### Travel Time Modifiers

```javascript
const seasonalTravelModifiers = {
    'spring': {
        roadCondition: 0.9,     // Muddy roads
        speed: 0.95,
        danger: 1.0
    },
    'summer': {
        roadCondition: 1.0,     // Best conditions
        speed: 1.0,
        danger: 0.9             // Less dangerous
    },
    'autumn': {
        roadCondition: 0.95,
        speed: 1.0,
        danger: 1.0
    },
    'winter': {
        roadCondition: 0.7,     // Snow, ice
        speed: 0.7,             // Slower travel
        danger: 1.3             // More dangerous
    }
};
```

---

## Extended Cost Calculation

### Total Transaction Cost

Combining all factors:

```javascript
calculateTotalTransactionCost(swap) {
    const {
        commodity,
        amount,
        fromMarket,
        toMarket,
        marketTier,
        playerRace,
        season,
        preservation,
        guardTier
    } = swap;
    
    // 1. Auctioneer fees (always applied)
    const auctioneerFee = calculateAuctioneerFee(
        commodity,
        amount,
        marketTier,
        playerRace
    );
    
    // 2. Transport fees (if inter-market)
    const transportFee = fromMarket !== toMarket
        ? calculateTransportFee(commodity, fromMarket, toMarket, season)
        : 0;
    
    // 3. Guard fees (if inter-market)
    const guardFee = fromMarket !== toMarket && guardTier !== 'none'
        ? calculateGuardFee(commodity, amount, calculateDistance(fromMarket, toMarket), guardTier)
        : 0;
    
    // 4. Preservation costs (optional)
    const preservationCost = preservation !== 'none'
        ? preservationCosts[preservation].costPerUnit * amount
        : 0;
    
    // 5. Deterioration loss
    const travelTime = calculateTravelTime(fromMarket, toMarket, season);
    const deterioration = calculateDeteriorationLoss(
        commodity,
        amount,
        travelTime,
        season,
        preservation
    );
    
    // 6. Insurance (optional)
    const insuranceFee = marketTier === 'global' && guardTier === 'premium'
        ? commodity.basePrice * amount * 0.015
        : 0;
    
    return {
        auctioneerFee,
        transportFee,
        guardFee,
        preservationCost,
        deteriorationLoss: deterioration.loss,
        deteriorationValue: deterioration.loss * commodity.basePrice,
        insuranceFee,
        totalMonetaryCost: auctioneerFee + transportFee + guardFee + preservationCost + insuranceFee,
        totalLoss: auctioneerFee + transportFee + guardFee + preservationCost + insuranceFee + deterioration.loss * commodity.basePrice,
        effectiveAmount: amount - deterioration.loss
    };
}
```

### Example Calculation

**Scenario**: Trading 100 Fresh Cherries from Local Market A to Regional Market B in Summer

```javascript
const swap = {
    commodity: { type: 'fresh-fruit', name: 'Cherry', basePrice: 45, weight: 0.5 },
    amount: 100,
    fromMarket: 'market-a',
    toMarket: 'market-b',
    marketTier: 'regional',
    playerRace: 'native-inhabitants',
    season: 'summer',
    preservation: 'none',
    guardTier: 'basic'
};

const costs = calculateTotalTransactionCost(swap);

// Results:
{
    auctioneerFee: 135,              // 3% of 4500
    transportFee: 25,                // Distance-based
    guardFee: 22.5,                  // 0.5% of value + base
    preservationCost: 0,             // No preservation
    deteriorationLoss: 6.5,          // 6.5 cherries lost
    deteriorationValue: 292.5,       // 6.5 * 45
    insuranceFee: 0,                 // Not global
    totalMonetaryCost: 182.5,        // Out of pocket
    totalLoss: 475,                  // Including deterioration value
    effectiveAmount: 93.5            // 93-94 cherries arrive
}
```

**With Preservation (Drying)**:
```javascript
const swapWithPreservation = { ...swap, preservation: 'drying' };
const costsPreserved = calculateTotalTransactionCost(swapWithPreservation);

// Results:
{
    auctioneerFee: 135,
    transportFee: 25,
    guardFee: 22.5,
    preservationCost: 100,           // 1 per unit * 100
    deteriorationLoss: 1.3,          // Much less loss (80% reduction)
    deteriorationValue: 58.5,
    insuranceFee: 0,
    totalMonetaryCost: 282.5,        // Higher upfront
    totalLoss: 341,                  // But lower total loss!
    effectiveAmount: 98.7            // 98-99 cherries arrive
}
```

**Conclusion**: Paying 100 for drying saves 134 in total loss (475 - 341) and delivers 5+ more cherries.

---

## Integration with Swap Router

### Updated Cost Function

The swap router's cost calculation now includes all these factors:

```javascript
calculateExchangeCost(fromCommodity, toCommodity, market, options = {}) {
    const {
        season = 'summer',
        marketTier = 'regional',
        playerRace = 'native-inhabitants',
        preservation = 'none',
        guardTier = 'basic'
    } = options;
    
    // Original exchange rate
    const buyPrice = market.getPrice(fromCommodity);
    const sellPrice = market.getPrice(toCommodity);
    const exchangeRate = buyPrice / sellPrice;
    
    // Base transaction fee
    const baseFeeRate = 0.025;
    
    // Auctioneer fee based on market tier and race
    const auctioneerFeeRate = getAuctioneerFeeRate(marketTier, playerRace);
    
    // Slippage based on liquidity
    const fromData = market.commodityData.get(fromCommodity);
    const toData = market.commodityData.get(toCommodity);
    const avgLiquidity = (fromData.supply + toData.supply) / 2;
    const slippage = Math.max(0.01, Math.min(0.05, 100 / avgLiquidity));
    
    // Deterioration impact
    const deteriorationRate = calculateDeteriorationRate(
        fromCommodity,
        season,
        preservation
    );
    
    // Total effective rate
    const effectiveRate = exchangeRate * 
                         (1 - baseFeeRate) * 
                         (1 - auctioneerFeeRate) * 
                         (1 - slippage) * 
                         (1 - deteriorationRate);
    
    return 1 / effectiveRate;
}
```

### Route Optimization with Extensions

The router now considers:
1. Market tier selection (local vs regional vs global)
2. Preservation method selection
3. Guard tier selection
4. Seasonal timing
5. Race-based fee impacts

```javascript
findOptimalRouteExtended(fromCommodity, toCommodity, amount, options) {
    // Find base optimal route
    const baseRoute = this.findOptimalRoute(fromCommodity, toCommodity, amount);
    
    // Optimize for market tiers
    const tierOptions = ['local', 'regional', 'global'];
    const tierRoutes = tierOptions.map(tier => 
        this.findOptimalRoute(fromCommodity, toCommodity, amount, { 
            ...options, 
            marketTier: tier 
        })
    );
    
    // Optimize for preservation (for perishables)
    let preservationRoutes = [];
    if (isPerishable(fromCommodity)) {
        const preservationMethods = ['none', 'drying', 'salting', 'smoking'];
        preservationRoutes = preservationMethods.map(method =>
            this.findOptimalRoute(fromCommodity, toCommodity, amount, {
                ...options,
                preservation: method
            })
        );
    }
    
    // Optimize for guard tiers
    const guardTiers = ['none', 'basic', 'standard', 'premium'];
    const guardRoutes = guardTiers.map(tier =>
        this.findOptimalRoute(fromCommodity, toCommodity, amount, {
            ...options,
            guardTier: tier
        })
    );
    
    // Find absolute optimal considering all factors
    const allRoutes = [
        baseRoute,
        ...tierRoutes,
        ...preservationRoutes,
        ...guardRoutes
    ].filter(r => r.success);
    
    // Sort by total cost (including all fees and losses)
    allRoutes.sort((a, b) => a.totalCost - b.totalCost);
    
    return {
        optimal: allRoutes[0],
        alternatives: allRoutes.slice(1, 5),
        recommendations: generateRecommendations(allRoutes, options)
    };
}
```

---

## Implementation Priorities

### Phase 1: Core Extensions (Immediate)
1. ✅ Auctioneer fee system
2. ✅ Market tier structure (local/regional/global)
3. ✅ Basic transport fees

### Phase 2: Security and Risk (Near-term)
1. ✅ Guard tier system
2. ✅ Risk calculation
3. ✅ Insurance options

### Phase 3: Perishables (Medium-term)
1. ✅ Deterioration system
2. ✅ Preservation methods
3. ✅ Seasonal effects on decay

### Phase 4: Advanced Features (Long-term)
1. ⏳ Dynamic weather effects
2. ⏳ Player-run auction houses
3. ⏳ Convoy system for bulk transport
4. ⏳ Trade insurance marketplace

---

## Database Schema Extensions

### Additional Tables

```sql
-- Market tiers and fees
CREATE TABLE market_tiers (
    market_id UUID PRIMARY KEY,
    tier VARCHAR(20) NOT NULL, -- 'local', 'regional', 'global'
    auctioneer_fee_base DECIMAL(5,4),
    listing_fee DECIMAL(5,4),
    location_id UUID,
    active BOOLEAN DEFAULT true
);

-- Race-based fee multipliers
CREATE TABLE race_market_fees (
    race_id VARCHAR(50) PRIMARY KEY,
    global_market_multiplier DECIMAL(3,2),
    description TEXT
);

-- Commodity decay rates
CREATE TABLE commodity_decay (
    commodity_id VARCHAR(50) PRIMARY KEY,
    base_decay_rate DECIMAL(5,4),
    spring_multiplier DECIMAL(3,2),
    summer_multiplier DECIMAL(3,2),
    autumn_multiplier DECIMAL(3,2),
    winter_multiplier DECIMAL(3,2)
);

-- Preservation methods
CREATE TABLE preservation_methods (
    method_id VARCHAR(50) PRIMARY KEY,
    cost_per_unit DECIMAL(10,2),
    time_required_hours DECIMAL(6,2),
    decay_reduction_percent DECIMAL(5,2),
    shelf_life_extension_days INTEGER
);

-- Transport routes with guard options
CREATE TABLE transport_routes (
    route_id UUID PRIMARY KEY,
    from_market_id UUID,
    to_market_id UUID,
    distance_km DECIMAL(10,2),
    quality VARCHAR(20), -- 'paved-road', 'dirt-road', 'trail', 'wilderness'
    base_risk_factor DECIMAL(3,2),
    seasonal_risk JSONB
);

-- Guard tiers
CREATE TABLE guard_tiers (
    tier_id VARCHAR(20) PRIMARY KEY,
    base_cost DECIMAL(10,2),
    value_fee_percent DECIMAL(5,4),
    risk_reduction DECIMAL(3,2),
    ambush_chance DECIMAL(5,4)
);
```

---

## API Endpoints

### New Endpoints

```
POST /api/markets/swap/calculate-extended
Body: {
    from: 'wood',
    to: 'iron',
    amount: 100,
    fromMarket: 'market-a',
    toMarket: 'market-b',
    options: {
        marketTier: 'regional',
        season: 'summer',
        preservation: 'drying',
        guardTier: 'basic',
        playerRace: 'native-inhabitants'
    }
}
Response: Complete cost breakdown including all fees and losses

GET /api/markets/preservation-methods
Response: List of available preservation methods with costs

GET /api/markets/guard-tiers
Response: List of guard tiers with costs and risk reduction

GET /api/markets/seasonal-effects?season=summer
Response: Current seasonal supply/demand/price modifiers

POST /api/markets/optimize-route
Body: Same as calculate-extended
Response: Optimal route considering all factors plus top alternatives
```

---

## Conclusion

This extended system creates a rich, realistic trading economy that:

1. **Rewards Strategic Planning** - Players must consider multiple factors
2. **Creates Economic Depth** - Many variables affect profitability
3. **Encourages Specialization** - Different races/regions have advantages
4. **Adds Risk/Reward Gameplay** - Security vs cost tradeoffs
5. **Reflects Real-World Trade** - Preservation, transport, security all matter
6. **Supports Emergent Behavior** - Players optimize for their situation

The system maintains the core pathfinding algorithm while adding layers of strategic decision-making that make trade more engaging and realistic.

---

## References

- Original swap router implementation
- Auction house systems research document
- EVE Online market mechanics
- Historical trade route economics
- Medieval preservation techniques
- Modern supply chain management

## License

Part of the BlueMarble.Design research repository.  
All rights reserved.
