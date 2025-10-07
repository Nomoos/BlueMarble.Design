# Auction Economy Design

**Version:** 1.0  
**Date:** 2025-01-18  
**Owner:** @Nomoos  
**Status:** Draft  
**Related:** [Economy](economy.md), [Core Mechanics](mechanics.md)

---

## Executive Summary

This document defines the auction-based trading and economic mechanics for BlueMarble, including a tiered market system,
physical transport logistics, dynamic fees, item spoilage, and seasonal effects. The goal is a **hybrid, player-driven
market** where regional auctions create meaningful trade opportunities while requiring physical transport of goods.

**Key Design Principles:**

- **Regional Specialization**: Different nodes produce different goods, creating natural trade opportunities
- **Physical Transport**: Goods must be physically moved between markets, not teleported
- **Economic Sinks**: Fees and spoilage remove currency and items from the economy
- **Strategic Depth**: Seasonal cycles and preservation mechanics add timing considerations
- **Player Professions**: Support for traders, couriers, and market specialists

---

## Core Systems

### Auction Tier System

The auction system is organized into three tiers, each with distinct characteristics, fees, and reach:

#### Local Auction Houses

**Characteristics:**

- Available in most settlements
- Low fees (1.5% commission)
- Limited to local buyers
- Fast transactions
- No transport required for same-settlement trades

**Fee Structure:**

```javascript
{
    commissionRate: 0.015,       // 1.5% of sale price
    listingFee: 0.005,           // 0.5% upfront cost
    transportFee: 0,             // None for local trades
    guardFee: 0                  // None for local trades
}
```

**Use Cases:**

- Quick trades within settlement
- Common goods (food, basic materials)
- Player-to-player local commerce
- Items unsuitable for long transport (high spoilage)

#### Regional Auction Houses

**Characteristics:**

- Located in major cities
- Medium fees (3% commission)
- Connects multiple settlements within a region
- Broader commodity selection
- Transport required for inter-settlement trades

**Fee Structure:**

```javascript
{
    commissionRate: 0.03,        // 3% of sale price
    listingFee: 0.01,            // 1% upfront cost
    transportFee: distance * 0.001,  // Per km
    guardFee: value * 0.005      // 0.5% of item value
}
```

**Use Cases:**

- Trading between nearby settlements
- Regional specialization goods (coastal fish, mountain ores)
- Medium-value items
- Cross-settlement arbitrage opportunities

#### Global Auction Houses

**Characteristics:**

- Available only at major trade capitals
- High fees (7% base commission)
- Access to worldwide market
- Long-distance transport required
- Premium security options available

**Fee Structure:**

```javascript
{
    commissionRate: 0.07,        // 7% base rate
    racePremium: 1.0-2.0,        // Multiplier for experimental races
    listingFee: 0.02,            // 2% upfront cost
    transportFee: distance * 0.002,  // Per km (higher rate)
    guardFee: value * 0.01,      // 1% of item value
    insuranceFee: value * 0.015  // Optional insurance
}
```

**Race-based Fee Multipliers:**

```javascript
{
    'native-inhabitants': 1.0,    // No premium
    'established-settlers': 1.2,  // 20% premium
    'experimental-race-1': 1.5,   // 50% premium
    'experimental-race-2': 2.0    // 100% premium
}
```

**Use Cases:**

- Intercontinental trade
- Rare and luxury goods
- High-value transactions
- Strategic resource movement

---

### Auction Nodes (Market Regions)

Each geographic **node** represents a distinct market region operating as its own auction house.

**Node Characteristics:**

- Towns, cities, and settlements host separate market nodes
- Items listed in one node must be physically transported to reach buyers elsewhere
- Resource-rich areas supply local goods cheaper
- Scarce resources fetch premium prices
- Major cities become natural trading hubs

**Regional Specialization Examples:**

| Region Type | Abundant Resources | Scarce Resources |
|-------------|-------------------|------------------|
| Coastal Settlement | Fish, salt, shells | Ore, timber, grain |
| Mountain Town | Ore, gems, stone | Fish, crops, cloth |
| Farming Village | Grain, vegetables, leather | Metal tools, gems |
| Forest Settlement | Timber, herbs, furs | Stone, fish, metal |

**Node Connectivity:**

- Connecting nodes requires investment (contribution points)
- Established trade routes reduce transport costs
- New routes can be opened through player or guild investment
- Route quality affects transport time and risk

---

### Transport Mechanics

Transport moves goods between auction nodes with multiple methods, costs, and risks.

#### Transport Methods

| Method | Capacity | Speed | Base Cost | Risk Level |
|--------|----------|-------|-----------|-----------|
| Walking | Low (50kg) | 5 km/h | None | Medium |
| Horse Cart | Medium (500kg) | 15 km/h | Low | Medium |
| Wagon | High (2000kg) | 10 km/h | Medium | High |
| Cargo Ship | Very High (10000kg) | 20 km/h | High | Medium |
| Airship | Medium (1000kg) | 50 km/h | Very High | Low |

#### Transport Cost Formula

```javascript
transportCost = (distance / speed) * (
    baseCostPerKm * 
    vehicleCapacityMultiplier * 
    terrainMultiplier * 
    weatherMultiplier
)
```

**Factors:**

- **Distance**: Kilometers between origin and destination
- **Speed**: Vehicle speed affects time and fuel costs
- **Terrain**: Mountains, forests, water add difficulty multipliers
- **Weather**: Storms, snow, heat affect travel time and cost

#### Transport Risk Formula

```javascript
transportRisk = baseRisk * 
    (1 + distance/10000) * 
    (1 + terrainDifficulty) * 
    weatherRiskFactor * 
    (1 - guardReduction)
```

**Risk Mitigation:**

- **Guards**: Basic (50% risk reduction), Standard (75%), Premium (90%)
- **Convoys**: Group transport shares costs and improves security
- **Insurance**: Optional coverage for item loss (1-3% of value)
- **Route Selection**: Safer routes cost more but reduce risk

---

### Item Spoilage System

Perishable goods degrade over time if not sold or consumed, creating pressure to trade quickly.

#### Spoilage Model

**Base Formula (Exponential Decay):**

```javascript
freshness_new = freshness_old * Math.exp(-spoilageRate * timeElapsed)
```

**Enhanced Model with Environmental Factors:**

```javascript
function calculateSpoilage(item, timeElapsed, environment) {
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
    
    // Calculate effective rate
    const effectiveRate = baseSpoilageRate * 
        tempMultiplier * 
        humidityMultiplier * 
        seasonMultiplier * 
        preservationMultiplier;
    
    const newFreshness = item.freshness * Math.exp(-effectiveRate * timeElapsed);
    
    // Value degradation based on freshness (quadratic)
    const valueMultiplier = Math.pow(newFreshness, 2);
    
    return {
        freshness: Math.max(0, newFreshness),
        currentValue: item.baseValue * valueMultiplier,
        isSpoiled: newFreshness < 0.1 // Below 10% = spoiled
    };
}
```

#### Perishable Item Categories

| Category | Base Decay Rate | Preservation Options |
|----------|----------------|---------------------|
| Fresh Produce | 5% per day | Cold storage, canning |
| Raw Meat | 8% per day | Salting, smoking, drying |
| Dairy | 6% per day | Cold storage, cheese-making |
| Baked Goods | 4% per day | Drying |
| Alchemical Ingredients | 3% per day | Sealed containers, refrigeration |

#### Preservation Methods

| Method | Decay Reduction | Cost | Requirements |
|--------|----------------|------|-------------|
| None | 1.0x (100%) | Free | None |
| Drying | 0.5x (50%) | Low | Time, sunlight |
| Salting | 0.4x (40%) | Low | Salt |
| Smoking | 0.3x (30%) | Medium | Smokehouse |
| Canning | 0.2x (20%) | Medium | Cans, equipment |
| Cold Storage | 0.15x (15%) | High | Ice house, cellar |
| Refrigeration | 0.1x (10%) | Very High | Magic/tech cooling |

---

### Seasonal Effects

A four-season cycle modulates supply, demand, trade routes, and spoilage rates.

#### Spring

**Supply Modifiers:**

- Vegetables: +30% (spring planting)
- Flowers: +50%
- Seeds: -20% (planted, not sold)

**Demand Modifiers:**

- Seeds: +40%
- Tools: +20%
- Building materials: +30%

**Route Effects:**

- Rivers thawed (water routes open)
- Mountain passes still snowed in
- Mud from thawing affects cart travel (-20% speed)

**Spoilage:**

- Moderate deterioration (1.0x multiplier)
- Warming temperatures increase decay

#### Summer

**Supply Modifiers:**

- Grain: +80% (harvest season)
- Fruits: +60%
- Vegetables: +40%

**Price Modifiers:**

- Grain: -40% (to 60% of base price)
- Fuel: -20% (lower heating demand)

**Route Effects:**

- All routes available
- Hot weather affects travel comfort
- Dry roads improve speed (+10%)

**Spoilage:**

- High deterioration (1.3x multiplier)
- Heat accelerates decay
- Cold storage more valuable

#### Autumn

**Supply Modifiers:**

- Preserved foods: +30%
- Harvest goods: +50%
- Hunting game: +20%

**Demand Modifiers:**

- Preservation supplies: +40%
- Winter preparation items: +30%
- Fuel/firewood: +20%

**Route Effects:**

- Rain and mud slow travel (-15% speed)
- Harvest season creates transport demand

**Spoilage:**

- Moderate deterioration (0.9x multiplier)
- Cooling temperatures reduce decay

#### Winter

**Supply Modifiers:**

- Fuel/firewood: +20%
- Winter clothing: +10%
- Fresh produce: -50%

**Demand Modifiers:**

- Fuel: +50%
- Food: +20%
- Warm clothing: +40%

**Price Modifiers:**

- Fresh produce: +100% (×2 due to scarcity)
- Preserved foods: +50%

**Route Effects:**

- River trade: CLOSED (frozen)
- Sea routes: CLOSED (ice)
- Mountain passes: CLOSED (snow)
- Cart speed: -30% (snow, ice)

**Spoilage:**

- Low deterioration (0.5x multiplier)
- Natural cold storage
- Snow preserves food

---

## Economic Balance Mechanisms

### Price Floors

**NPC Vendor Buyback:**

- NPCs buy common goods at 40% of average market price
- Prevents total market collapse
- Prevents grief dumping
- Provides emergency liquidity

### Price Ceilings

**NPC Vendor Sales:**

- NPCs sell common goods at 160% of average market price
- Prevents price manipulation
- Limits monopoly power
- Provides supply backstop

### Transaction Fees as Currency Sink

Fees remove currency from the economy to control inflation:

- Local markets: 1.5-2% total fees
- Regional markets: 3-4% total fees
- Global markets: 7-10% total fees

**Annual Currency Removal Estimate:**

- Assumes 100,000 daily transactions
- Average transaction value: 100 gold
- Average fee rate: 5%
- Daily sink: 500,000 gold
- Annual sink: 182,500,000 gold

---

## Player Professions and Gameplay

### Trader Profession

**Activities:**

- Buy low in one region, sell high in another
- Monitor seasonal price fluctuations
- Invest in bulk goods during harvest
- Specialize in regional arbitrage

**Skills:**

- Price analysis and market research
- Route optimization
- Risk assessment
- Negotiation with transport providers

### Courier/Transport Profession

**Activities:**

- Transport goods for other players
- Specialize in secure high-value deliveries
- Organize convoys for shared security
- Operate transport businesses

**Specializations:**

- **Speed Courier**: Light loads, urgent deliveries, premium fees
- **Bulk Hauler**: Large capacity, efficient routes, volume discounts
- **Security Specialist**: Armed guards, high-value items, low loss rate
- **Stealth Operative**: Concealed transport, avoid main roads

### Market Specialist

**Activities:**

- Operate as player auctioneer (alternative to NPC)
- Provide market analysis and price predictions
- Manage consignment sales
- Facilitate bulk transactions

**Reputation System:**

- Build trust through successful transactions
- Lower commission rates with high reputation
- Access to exclusive deals and contracts

---

## Implementation Priorities

### Priority 1: Core Systems (MVP)

- [ ] Implement three-tier auction system (local, regional, global)
- [ ] Basic fee structure with commission and listing fees
- [ ] Simple transport cost calculation (distance-based)
- [ ] Basic spoilage for perishable items (linear decay)
- [ ] Seasonal price modifiers (basic)

### Priority 2: Enhanced Mechanics

- [ ] Exponential decay model for spoilage
- [ ] Temperature and humidity environmental factors
- [ ] Multiple transport methods with different attributes
- [ ] Guard tiers and risk mitigation
- [ ] Preservation methods (drying, salting, cold storage)
- [ ] Route availability based on seasons

### Priority 3: Advanced Features

- [ ] Player-run auction houses
- [ ] Insurance system for transport
- [ ] Dynamic node connectivity
- [ ] Contribution points for trade route development
- [ ] Weather events affecting transport
- [ ] Guild convoy systems
- [ ] Cold storage facilities (player-owned)

---

## Integration with Existing Systems

### Connection to Multi-Step Routing

The auction economy integrates with the existing multi-step commodity swap routing:

- Auction purchases can trigger routing calculations
- Transport costs feed into routing optimization
- Regional prices influence swap profitability
- Spoilage affects multi-step trade viability

### Connection to Player Progression

- Trading skills improve with experience
- Transport efficiency increases with levels
- Reputation affects available deals and fees
- Specialization unlocks advanced features

### Connection to World Geography

- Resource distribution affects regional prices
- Terrain influences transport costs and routes
- Climate zones affect spoilage rates
- Settlement placement creates natural trade hubs

---

## Open Questions and Considerations

### Balancing Concerns

1. **Fee Balance**: Are the fee rates appropriate to encourage use while providing sufficient currency sink?
2. **Spoilage Rates**: Do decay rates create sufficient pressure without being punitive?
3. **Transport Costs**: Are costs balanced to make long-distance trade profitable but not trivial?
4. **Risk Levels**: Is the transport risk/reward ratio engaging for traders?

### Design Decisions

1. **Player vs NPC Auctioneers**: Should players be able to fully replace NPC auction houses?
2. **Insurance Coverage**: What percentage of item value should insurance cover (80%? 100%?)?
3. **Contribution Points**: How should players earn and spend these for trade routes?
4. **Monopoly Prevention**: What mechanisms prevent market cornering by wealthy players/guilds?

### Technical Considerations

1. **Database Performance**: How to efficiently store and query distributed auction data?
2. **Real-Time Updates**: How frequently should prices and listings update?
3. **Transport Simulation**: Should actual transport be simulated or abstracted?
4. **Spoilage Calculation**: Server-side continuous calculation or discrete time steps?

---

## References

- [Research: Auction House Systems](../research/topics/auction-house-systems-local-global-transport.md)
- [Research: Extended Auction System](../research/topics/extended-auction-system-transport-fees-deterioration.md)
- [Research: Game Economy Design External](../research/sources/game-economy-design-external.md)
- [Design: Economy](economy.md)
- [Design: Core Mechanics](mechanics.md)

---

**Document Status**: This design document is in draft status and requires:

1. Technical feasibility review
2. Economic balancing analysis
3. Player testing and feedback
4. Integration planning with existing systems

**Next Steps**:

1. Review with development team for technical constraints
2. Create detailed specifications for Priority 1 features
3. Develop prototype for core auction tier system
4. Design database schema for auction and transport data
5. Create UI mockups for auction interface
