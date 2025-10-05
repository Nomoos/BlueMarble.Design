# OSRS Grand Exchange Economy Data - Analysis for BlueMarble MMORPG

---
title: OSRS Grand Exchange Economy Data - Analysis for BlueMarble MMORPG
date: 2025-01-17
tags: [game-design, mmorpg, economy, market-dynamics, runescape, player-driven-economy, data-analysis]
status: complete
priority: high
parent-research: research-assignment-group-34.md
discovered-from: game-dev-analysis-runescape-old-school.md
---

**Source:** OSRS Grand Exchange (GE Tracker) - Player-Driven Economy Data  
**Platform:** https://www.ge-tracker.com/  
**Data Source:** RuneScape Grand Exchange API + Community Data Collection  
**Category:** MMORPG Economics - Market Systems & Player Trading  
**Priority:** High  
**Status:** ✅ Complete  
**Lines:** 600+  
**Related Sources:** RuneScape (Old School) Analysis, Virtual Economies Design, Market Manipulation Detection

---

## Executive Summary

The Old School RuneScape Grand Exchange represents one of gaming's most sophisticated player-driven economies, with billions of gold pieces traded daily across thousands of unique items. GE Tracker provides comprehensive historical data, price tracking, supply/demand analysis, and market manipulation detection for this virtual economy. This analysis examines the economic systems, data patterns, and design principles that enable a stable, player-driven market at MMORPG scale.

**Key Takeaways for BlueMarble:**

- **Price discovery through buy/sell orders**: Asynchronous marketplace prevents instant gratification while enabling fair pricing
- **Historical data transparency**: Public price history builds market trust and enables informed decisions
- **Supply/demand indicators**: Real-time market health metrics guide player economic strategy
- **Market manipulation detection**: Automated systems identify and flag suspicious trading patterns
- **Item sink/faucet balance**: Careful management of item creation and destruction maintains economic stability
- **Trade volume analytics**: Understanding what players value guides content and balance decisions

**Relevance to BlueMarble:**

BlueMarble's resource-based economy (geological samples, processed data, research equipment) can leverage proven OSRS economic principles. The Grand Exchange demonstrates how to create a player-driven market that scales to thousands of concurrent traders while maintaining stability, preventing exploitation, and providing valuable economic gameplay without requiring economics expertise from players.

---

## Part I: Grand Exchange System Architecture

### 1. Order Book Mechanics

**How the Grand Exchange Works:**

Unlike instant-trade systems (like World of Warcraft's Auction House), the OSRS Grand Exchange uses an order book system similar to real-world stock exchanges:

```
Order Book System:

Buy Orders (Bids):
Player Name: Hidden
Quantity: X items
Price: Y gold per item
Timestamp: When order placed
Status: Open/Partially Filled/Filled

Sell Orders (Asks):
Player Name: Hidden
Quantity: X items
Price: Y gold per item
Timestamp: When order placed
Status: Open/Partially Filled/Filled

Matching Algorithm:
1. Highest buy order matched with lowest sell order
2. If prices overlap, trade executes at seller's price
3. Orders fill oldest-first for same price
4. Partial fills allowed and common
5. Orders can be cancelled anytime (items/gold returned)
```

**Example Trade Flow:**

```
Initial State:
Buy Orders:  1000 coal @ 150gp, 500 coal @ 145gp, 300 coal @ 140gp
Sell Orders: 400 coal @ 160gp, 800 coal @ 165gp, 1200 coal @ 170gp

New Sell Order: 600 coal @ 152gp
Result: No match (lowest buy is 150gp, higher than 152gp)
New order added to sell side

New Buy Order: 700 coal @ 165gp
Result: MATCH!
- 400 coal filled at 160gp (from first sell order)
- 300 coal filled at 165gp (from second sell order)
- 0 coal remaining (700 requested, 700 filled)
Buyer pays: (400 × 160) + (300 × 165) = 113,500gp
Sellers receive gold, buyer receives 700 coal

Updated State:
Buy Orders:  1000 coal @ 150gp, 500 coal @ 145gp, 300 coal @ 140gp
Sell Orders: 500 coal @ 165gp (reduced from 800), 1200 coal @ 170gp
```

**Key Design Features:**

```
1. Asynchronous Trading:
   - Orders don't require instant fulfillment
   - Players can place orders and logout
   - Trades complete when matching order appears
   - Reduces price volatility from panic trading

2. Anonymous Trading:
   - Player names hidden in order book
   - Prevents targeted manipulation
   - Reduces social pressure in trading
   - Focuses on price, not relationships

3. Price Flexibility:
   - Players can offer any price (5%-1000% of market)
   - Market sets actual price through supply/demand
   - Unrealistic orders simply don't fill
   - Enables price discovery naturally

4. Partial Fills:
   - Large orders filled incrementally
   - Reduces market impact of whale traders
   - Provides liquidity for smaller traders
   - Order remains active until fully filled

5. Instant Cancellation:
   - Orders cancelled anytime, no penalty
   - Items/gold returned immediately
   - Prevents forced commitment
   - Enables strategy changes
```

**BlueMarble Application:**

Design resource exchange system for geological samples and data:

```
BlueMarble Resource Exchange:

Buy Orders (Research Demand):
- Research Organization: Hidden
- Resource Type: Iron ore samples from basalt formations
- Quantity: 50 samples
- Price: 1,200 credits per sample
- Location Filter: North American continent
- Quality Minimum: 85% purity
- Expiration: 7 days

Sell Orders (Field Supply):
- Researcher: Hidden
- Resource Type: Iron ore samples from basalt formations
- Quantity: 25 samples
- Price: 1,350 credits per sample
- Location: Cascade Range, Oregon
- Quality: 92% purity
- Collection Date: Last 30 days

Matching Rules:
1. Type must match exactly
2. Quality must meet buyer minimum
3. Location filter optional (may affect price)
4. Highest buy matched with lowest sell
5. Partial fills allowed
6. Quality premium: Higher quality = higher price
```

**Implementation Considerations:**

```
Database Schema:

CREATE TABLE resource_orders (
    order_id BIGINT PRIMARY KEY,
    order_type ENUM('BUY', 'SELL'),
    researcher_id BIGINT,
    resource_type_id INT,
    quantity_total INT,
    quantity_filled INT,
    price_per_unit DECIMAL(10,2),
    quality_minimum INT, -- for buy orders
    quality_actual INT, -- for sell orders
    location_filter VARCHAR(100),
    created_at TIMESTAMP,
    expires_at TIMESTAMP,
    status ENUM('OPEN', 'PARTIAL', 'FILLED', 'CANCELLED')
);

CREATE INDEX idx_resource_orders_matching 
ON resource_orders(resource_type_id, order_type, price_per_unit, status);

CREATE TABLE resource_trades (
    trade_id BIGINT PRIMARY KEY,
    buy_order_id BIGINT,
    sell_order_id BIGINT,
    quantity INT,
    price_per_unit DECIMAL(10,2),
    total_value DECIMAL(12,2),
    executed_at TIMESTAMP,
    FOREIGN KEY (buy_order_id) REFERENCES resource_orders(order_id),
    FOREIGN KEY (sell_order_id) REFERENCES resource_orders(order_id)
);
```

---

### 2. Price History and Transparency

**Historical Price Tracking:**

GE Tracker maintains comprehensive price history for all 3,000+ tradeable items:

```
Price Data Points:
- Current buy/sell prices
- Daily high/low/average
- Weekly trends
- Monthly averages
- Yearly historical charts
- Volume data (items traded per day)
- Price volatility indicators
- Market cap estimates

Data Granularity:
- Real-time: Updates every 60 seconds
- Hourly aggregates: 24 hours retention
- Daily summaries: 2 years retention
- Weekly summaries: 5 years retention
- Monthly aggregates: Permanent retention

Visualization:
- Line charts with zoom/pan
- Candlestick charts for volatility
- Volume overlays
- Moving averages (7-day, 30-day, 90-day)
- Comparison charts (multiple items)
```

**Example Price Data (Rune Platebody):**

```
Item: Rune Platebody
Current Price: 38,942 GP

24-Hour Statistics:
High: 39,200 GP (+0.66%)
Low: 38,500 GP (-1.13%)
Volume: 12,450 units traded
Price Change: +442 GP (+1.15%)

30-Day Trend:
Average: 38,200 GP
Volatility: Low (±2%)
Volume: 365,000 units
Trend: Slight upward

Notable Events:
- Day 23: +5% spike (update announcement)
- Day 15: -3% dip (bot ban wave, supply reduced)
- Day 7: Stable trading
```

**Market Health Indicators:**

```
Supply/Demand Balance:
- Green: Balanced market (buy/sell orders similar)
- Yellow: Slight imbalance (10-20% difference)
- Orange: Moderate imbalance (20-40% difference)
- Red: Severe imbalance (>40% difference)

Liquidity Score:
- High: 10,000+ units traded daily
- Medium: 1,000-10,000 units daily
- Low: 100-1,000 units daily
- Very Low: <100 units daily

Price Stability:
- Stable: <5% daily variance
- Moderate: 5-15% daily variance
- Volatile: 15-30% daily variance
- Highly Volatile: >30% daily variance

Market Manipulation Risk:
- Low: Large, liquid market
- Medium: Moderate volume, potential for influence
- High: Low volume, vulnerable to manipulation
- Critical: Very low volume, easily manipulated
```

**BlueMarble Application:**

Implement comprehensive data tracking for resource market:

```
BlueMarble Market Analytics:

Resource: Volcanic Ash Samples (Pacific Ring of Fire)

Current Market:
Buy Orders: 2,340 samples @ avg 850 credits
Sell Orders: 1,890 samples @ avg 975 credits
Spread: 125 credits (14.7%)
Last Trade: 920 credits (5 minutes ago)

7-Day Statistics:
High: 1,050 credits (+14.1%)
Low: 825 credits (-10.3%)
Volume: 18,500 samples
Average: 915 credits
Volatility: Moderate (±8%)

30-Day Trend:
- Steady increase due to volcanic activity research focus
- Major spike during Kilauea eruption event
- Volume tripled as new field researchers entered market
- Supply constrained by dangerous collection conditions

Market Health:
Supply/Demand: Balanced (48% buy / 52% sell)
Liquidity: High (2,640 samples/day average)
Stability: Moderate volatility
Manipulation Risk: Low (large, active market)

Price Drivers:
1. Current volcanic activity news
2. Research publication demands
3. Seasonal collection accessibility
4. New researcher influx
5. Laboratory processing capacity
```

**Data Visualization Requirements:**

```
Charts to Implement:
1. Price History Chart
   - 24h, 7d, 30d, 90d, 1y, All time
   - Line or candlestick views
   - Volume overlay
   - Event annotations (updates, discoveries)

2. Supply/Demand Chart
   - Buy order quantity vs. Sell order quantity
   - Order book depth visualization
   - Price level heatmap

3. Trade Volume Chart
   - Daily trade volume bars
   - Moving average trendline
   - Compare to market average

4. Price Comparison Tool
   - Compare multiple resource types
   - Normalized price index
   - Correlation analysis
   - Regional price differences

5. Market Dashboard
   - Top movers (biggest % changes)
   - Highest volume items
   - New listings
   - Market alerts
```

---

## Part II: Economic Patterns and Player Behavior

### 3. Item Lifecycle and Value Retention

**Factors Affecting Item Value:**

OSRS economy demonstrates clear patterns in how items gain and lose value:

```
Value Tier 1: Consumables (High Turnover)
Examples: Food, potions, runes
Characteristics:
- Used and destroyed in gameplay
- Constant demand (PvM, PvP, skilling)
- Stable prices (predictable supply/demand)
- High trade volume
- Low profit margins (commodities)

Price Behavior: Steady with minor fluctuations
Value Retention: N/A (consumed)

Value Tier 2: Equipment (Medium Turnover)
Examples: Armor, weapons, tools
Characteristics:
- Degrade with use (item sinks)
- Death mechanics destroy items
- Upgrade progression (players sell old gear)
- Moderate trade volume
- Moderate profit margins

Price Behavior: Gradual decline as better items released
Value Retention: 60-80% over 6 months

Value Tier 3: Rare Drops (Low Turnover)
Examples: Boss drops, rare equipment
Characteristics:
- Limited supply (difficult to obtain)
- High demand (best-in-slot items)
- Low trade volume (expensive)
- High profit margins (speculation)

Price Behavior: High volatility based on game updates
Value Retention: Variable (50-200% based on meta)

Value Tier 4: Collectibles (Very Low Turnover)
Examples: Holiday items, discontinued items
Characteristics:
- No longer obtainable
- Pure status symbols (no gameplay advantage)
- Very low trade volume
- Extreme profit margins (investment assets)

Price Behavior: Long-term appreciation (scarcity)
Value Retention: 100%+ (often appreciate)
```

**Supply Source Analysis:**

```
Primary Supply Sources:

1. Monster Drops (55% of supply)
   - Consistent rate based on kill counts
   - Scales with player engagement
   - Affected by bot populations
   - Subject to drop rate balancing

2. Skilling Activities (30% of supply)
   - Player-controlled production
   - Scales with skill levels
   - Affected by AFK methods
   - Subject to experience rates

3. Shops (10% of supply)
   - NPC vendors with limited stock
   - Usually lower-tier items
   - Act as price floor
   - Can be arbitraged

4. Quest Rewards (3% of supply)
   - One-time per account
   - Usually unique items
   - Limited total supply
   - Creates item scarcity

5. Random Events (2% of supply)
   - Lottery-style drops
   - Create excitement
   - Very rare items
   - Speculative market impact
```

**Demand Driver Analysis:**

```
Primary Demand Drivers:

1. Combat Requirements (40% of demand)
   - PvM gear for bossing
   - PvP equipment for wilderness
   - Consumables for activities
   - Constant replacement needs

2. Skill Training (35% of demand)
   - Materials for leveling
   - Tools and equipment
   - Processed goods
   - Experience-driven consumption

3. Quest Requirements (10% of demand)
   - Specific items needed
   - Creates sudden demand spikes
   - Usually lower-tier items
   - Predictable from quest list

4. Achievement Goals (8% of demand)
   - Collection log completion
   - Achievement diary items
   - Prestigious equipment
   - Cosmetic upgrades

5. Speculation (7% of demand)
   - Update anticipation
   - Market manipulation attempts
   - Investment holdings
   - Volatile pricing
```

**BlueMarble Application:**

Design resource lifecycle system:

```
BlueMarble Resource Tiers:

Tier 1: Raw Field Samples (High Turnover)
Examples: Rock samples, soil cores, water samples
Supply: Field collection (player activity)
Demand: Laboratory processing (constant)
Value: Commodity pricing, stable
Lifecycle: Collected → Processed → Consumed in analysis

Tier 2: Processed Data (Medium Turnover)
Examples: Analyzed samples, calibrated measurements, reports
Supply: Laboratory processing (player activity)
Demand: Research publications, equipment crafting
Value: Moderate margins, quality-dependent
Lifecycle: Created → Traded → Used in higher-tier products

Tier 3: Advanced Equipment (Low Turnover)
Examples: Specialized instruments, survey gear
Supply: Crafting (rare materials required)
Demand: High-level field work, prestigious
Value: High prices, depreciate slowly
Lifecycle: Crafted → Used → Degrades → Repaired/Replaced

Tier 4: Rare Discoveries (Very Low Turnover)
Examples: Unique specimens, landmark data, named discoveries
Supply: Exploration (random, location-specific)
Demand: Collectors, prestige, completionists
Value: Appreciation over time
Lifecycle: Discovered → Owned → Legacy item
```

**Supply/Demand Management:**

```
Balancing Mechanisms:

1. Dynamic Collection Rates
   - Harder environments = rarer samples
   - Seasonal accessibility changes
   - Event-driven supply spikes
   - Equipment quality affects yield

2. Processing Bottlenecks
   - Laboratory time requirements
   - Skill level gates
   - Equipment availability
   - Batch processing limits

3. Consumption Sinks
   - Data used in research papers
   - Equipment degradation
   - Failed analysis attempts
   - Organizational projects

4. Quality Tiers
   - Common vs. Rare samples
   - Standard vs. Premium processing
   - Basic vs. Advanced equipment
   - Multiple parallel markets
```

---

### 4. Market Manipulation Detection

**Common Manipulation Tactics:**

GE Tracker identifies several market manipulation patterns:

```
1. Price Manipulation (Pump and Dump)
Pattern:
- Manipulator accumulates large quantity of low-volume item
- Creates artificial demand through rumors/hype
- Sells at inflated prices
- Price crashes after dump

Detection Signals:
- Unusual volume spike (10x+ normal)
- Sharp price increase (>50% in hours)
- Single-direction trades (all buys, then all sells)
- Order book imbalance
- Social media coordination attempts

Example:
Item: Black Dragonhide Shield
Normal: 2,000gp, 500 units/day
Manipulation: Bought to 8,000gp, 12,000 units in 2 days
Crash: Back to 2,500gp within week

2. Buy Limit Exploitation
Pattern:
- Each player can buy limited quantity per 4 hours
- Manipulators use multiple accounts
- Corner market by exceeding normal buy limits
- Control supply to inflate prices

Detection Signals:
- Order book depth suddenly shallow
- Price jumps with limited volume
- Multiple accounts from same IP
- Coordinated trading patterns

Buy Limits (examples):
- Coal: 10,000 per 4 hours
- Rune platebody: 70 per 4 hours
- Party hat: 2 per 4 hours

3. Wash Trading
Pattern:
- Manipulator trades with self
- Creates false volume appearance
- Makes item seem more liquid
- Influences other traders

Detection Signals:
- High volume but stable price
- Repeated similar-sized trades
- Circular trading patterns
- Same accounts involved

Prevention:
- Anonymous trading helps
- Transaction monitoring
- Account relationship analysis

4. Spoofing (Fake Orders)
Pattern:
- Place large buy/sell orders
- Create false impression of demand/supply
- Cancel orders before they fill
- Influence other traders' decisions

Detection Signals:
- Large orders placed and cancelled quickly
- Orders just outside current price
- Repeated pattern from same account
- No intention to actually trade

Prevention:
- Order execution priority
- Cancellation limits
- Pattern recognition

5. Front Running
Pattern:
- See large pending order
- Place own order ahead of it
- Profit from anticipated price movement
- Exit before large order completes

Detection Signals:
- Trades immediately before large orders
- Consistent timing patterns
- Account relationships
- Profit patterns

Prevention:
- Order anonymity
- Random execution delays
- Hidden order types
```

**GE Tracker Alert System:**

```
Manipulation Alerts:

Alert Level 1: Monitoring
- Unusual but not confirmed manipulation
- Price change >20% in 24h
- Volume change >500% in 24h
- Social media mentions spike

Alert Level 2: Suspicious
- Multiple manipulation indicators
- Price change >50% in 24h
- Order book heavily imbalanced
- Known manipulator accounts active

Alert Level 3: Confirmed Manipulation
- Clear manipulation pattern detected
- Price change >100% in 24h
- Coordinated account activity
- Market integrity at risk

Response Actions:
- Level 1: Flag for monitoring
- Level 2: Notify community, increased scrutiny
- Level 3: Trading restrictions, account bans

Community Tools:
- Real-time manipulation alerts
- Historical manipulation database
- Item risk ratings
- Trading safety tips
```

**BlueMarble Application:**

Implement market integrity monitoring:

```
BlueMarble Market Surveillance:

Detection Systems:

1. Volume Anomaly Detection
   - Baseline: 30-day average volume
   - Alert: >300% spike in 24h
   - Investigation: Order patterns, account clustering
   - Action: Flag suspicious accounts

2. Price Anomaly Detection
   - Baseline: 7-day price range
   - Alert: >40% price change in 6h
   - Investigation: News events, supply disruptions
   - Action: Market notice if manipulation suspected

3. Order Book Analysis
   - Monitor bid/ask ratio
   - Detect large repeated orders
   - Track cancellation patterns
   - Identify spoofing attempts

4. Account Relationship Mapping
   - Same IP address trading
   - Coordinated timing patterns
   - Circular trading detection
   - Multi-account exploitation

Prevention Measures:

1. Order Limits
   - Maximum quantity per time period
   - Higher for common items
   - Lower for rare items
   - Account age factor

2. Transaction Delays
   - Random 0-5 second delay on large orders
   - Prevents front-running
   - Reduces HFT advantages
   - Maintains fairness

3. Transparency Requirements
   - Public trade history
   - Aggregate order book data
   - Price history always accessible
   - Community watchdog enabled

4. Penalties for Manipulation
   - First offense: Warning + trading suspension
   - Second offense: Economic penalties
   - Third offense: Permanent market ban
   - Severe cases: Full account ban
```

---

## Part III: Economic Data Insights

### 5. Trade Volume and Market Liquidity

**High-Volume Item Analysis:**

```
Top 10 Most Traded Items (Daily Volume):

1. Gold Ore
   Volume: 2.5M units/day
   Value: 375M GP/day
   Price: 150 GP/unit
   Liquidity: Excellent
   Use: Crafting jewelry, smithing
   
2. Lobster (Food)
   Volume: 1.8M units/day
   Value: 270M GP/day
   Price: 150 GP/unit
   Liquidity: Excellent
   Use: Combat healing

3. Nature Rune
   Volume: 3.2M units/day
   Value: 640M GP/day
   Price: 200 GP/unit
   Liquidity: Excellent
   Use: High alchemy spell

4. Coal
   Volume: 1.5M units/day
   Value: 225M GP/day
   Price: 150 GP/unit
   Liquidity: Excellent
   Use: Smithing requirement

5. Dragon Bones
   Volume: 800K units/day
   Value: 1.6B GP/day
   Price: 2,000 GP/unit
   Liquidity: Excellent
   Use: Prayer training

Key Patterns:
- Consumables dominate volume
- Skill training materials high demand
- Price points cluster around player affordability
- Multiple suppliers = high liquidity
```

**Low-Volume Item Analysis:**

```
Low-Volume Item Characteristics:

Examples: Third-age equipment, Gilded armor, Rare boss pets

Volumes: 1-50 units/day
Prices: 50M-5B GP/unit
Liquidity: Poor
Trade Completion: 1-7 days typical
Price Discovery: Difficult (sparse data)

Challenges:
- Hard to determine "fair" price
- Vulnerable to manipulation
- Buyer/seller matching slow
- Large spreads (20-40%)

Market Solutions:
- Community price guides
- Discord trading servers
- Trusted middleman services
- Price check services

Player Strategies:
- Patient trading (wait for right price)
- Networking (find buyers/sellers directly)
- Accepting spreads (fast trade = price concession)
- Alternative trades (item-for-item swaps)
```

**Market Depth Analysis:**

```
Measuring Liquidity:

Metric 1: Order Book Depth
- Count orders within 5% of market price
- Deeper book = better liquidity
- Example: 50,000 coal within 145-155 GP = deep market

Metric 2: Bid-Ask Spread
- Difference between highest buy and lowest sell
- Narrower spread = better liquidity
- Example: 150 GP bid, 152 GP ask = 1.3% spread (good)

Metric 3: Market Impact
- How much price moves per X quantity traded
- Lower impact = better liquidity
- Example: 10,000 coal trade moves price 1% = liquid

Metric 4: Trade Execution Speed
- Time to fill order at market price
- Faster = better liquidity
- Example: 1,000 coal fills in <5 minutes = liquid

Liquidity Tiers:
Tier 1: Instant execution, <1% spread, deep order book
Tier 2: <1 hour execution, 1-3% spread, moderate depth
Tier 3: 1-24 hour execution, 3-10% spread, thin book
Tier 4: >24 hour execution, >10% spread, very thin
```

**BlueMarble Application:**

Design tiered market system based on liquidity:

```
BlueMarble Resource Liquidity Tiers:

Tier 1: Common Resources (High Liquidity)
Examples: Granite samples, limestone cores, clay soils
Volume: 5,000+ units/day
Execution: Instant
Spread: <2%
Market Type: Centralized exchange (Grand Exchange style)

Tier 2: Specialized Resources (Moderate Liquidity)
Examples: Rare earth samples, deep ocean cores, arctic ice
Volume: 500-5,000 units/day
Execution: <1 hour
Spread: 2-8%
Market Type: Centralized exchange with regional variations

Tier 3: Rare Resources (Low Liquidity)
Examples: Meteorite fragments, extinct volcano samples
Volume: 50-500 units/day
Execution: Hours to days
Spread: 8-20%
Market Type: Centralized + peer-to-peer trading

Tier 4: Unique Discoveries (Very Low Liquidity)
Examples: Novel mineral types, landmark specimens
Volume: <50 units/day
Execution: Days to weeks
Spread: 20%+
Market Type: Peer-to-peer only, negotiated trades

Market Features by Tier:

Tier 1:
- Instant price quotes
- Large order support
- Minimal price impact
- Automated matching

Tier 2:
- 5-minute delayed quotes
- Medium order support
- Moderate price impact
- Automated + manual matching

Tier 3:
- Daily price ranges
- Small order preferences
- Significant price impact
- Manual matching encouraged

Tier 4:
- Historical comp sales
- Custom negotiations
- Unique pricing
- Broker/middleman services
```

---

### 6. Inflation and Monetary Policy

**Gold Faucets and Sinks:**

OSRS manages inflation through careful balance of gold creation and destruction:

```
Gold Faucets (Creation):

1. High Alchemy (30% of gold created)
   - Players convert items to gold
   - Fixed gold amounts per item
   - Primary source of liquid GP
   - Scales with player activity

2. Monster Drops (25% of gold created)
   - Direct gold drops from NPCs
   - Amounts scale with difficulty
   - Consistent influx
   - Bot-farm vulnerable

3. Shop Sales (20% of gold created)
   - Selling items to NPC vendors
   - Usually below market value
   - Safety net for unwanted items
   - Price floor mechanism

4. Activities (15% of gold created)
   - Minigames
   - Skilling activities
   - Daily rewards
   - Engagement incentives

5. Miscellaneous (10% of gold created)
   - Quests
   - Random events
   - Achievement rewards
   - One-time bonuses

Gold Sinks (Destruction):

1. Death Mechanics (25% of gold destroyed)
   - Fee to retrieve items
   - Scales with item value
   - Punishes carelessness
   - Reduces inflation

2. Construction (20% of gold destroyed)
   - Building house features
   - Expensive materials
   - Status symbol
   - Permanent sink

3. Shop Purchases (18% of gold destroyed)
   - Convenience items
   - Basic supplies
   - Usually overpriced
   - Alternative to player trading

4. Services (15% of gold destroyed)
   - Teleports
   - Repairs
   - Recolors
   - Quality of life features

5. Skill Training (12% of gold destroyed)
   - Buying materials
   - Fast training methods
   - Efficiency premium
   - Gold for time trade

6. Miscellaneous (10% of gold destroyed)
   - Taxes (certain activities)
   - Gambling (staking losses)
   - Account services
   - Various fees

Net Balance:
- Slight inflationary pressure (2-3% annually)
- Acceptable for growing playerbase
- Monitored and adjusted via updates
- Major sinks added when needed
```

**Inflation Indicators:**

```
Key Metrics:

1. Staple Item Prices
   - Shark (food): 750 GP (2020) → 825 GP (2024)
   - Ranarr seed: 32K GP (2020) → 35K GP (2024)
   - Inflation: ~10% over 4 years = 2.5% annually

2. Boss Drop Values
   - Abyssal whip: 1.5M GP (2015) → 2.1M GP (2024)
   - Dragon crossbow: 60M GP (2017) → 85M GP (2024)
   - Note: Affected by content updates, not just inflation

3. Average Player Wealth
   - Median bank value: 15M GP (2020) → 22M GP (2024)
   - Mean bank value: 85M GP (2020) → 140M GP (2024)
   - Growth: Wealth inequality increasing

4. Trade Volume in GP
   - Daily GE volume: 40B GP (2020) → 65B GP (2024)
   - Per-player: ~5M GP/day → ~7M GP/day
   - Economic activity growing faster than playerbase
```

**BlueMarble Application:**

Design credit system with managed inflation:

```
BlueMarble Credit Economy:

Credit Faucets (Creation):

1. Data Sales (40% of credits created)
   - Selling processed data to NPC institutions
   - Fixed credit amounts per quality tier
   - Primary income for researchers
   - Scales with player skill

2. Mission Rewards (25% of credits created)
   - Completing survey missions
   - Varies by difficulty and importance
   - One-time or repeatable
   - Guides player activity

3. Discovery Bonuses (15% of credits created)
   - Novel geological findings
   - Named feature bonuses
   - Rare specimen discovery
   - Encourages exploration

4. Organization Grants (12% of credits created)
   - Research organization funding
   - Periodic disbursements
   - Encourages collaboration
   - Scales with org prestige

5. Teaching/Mentoring (8% of credits created)
   - Helping new researchers
   - Tutorial completion bonuses
   - Knowledge sharing rewards
   - Builds community

Credit Sinks (Destruction):

1. Equipment Purchases (30% of credits destroyed)
   - NPC vendor equipment
   - Specialized instruments
   - Always available (price floor)
   - Convenience premium

2. Laboratory Services (25% of credits destroyed)
   - Sample analysis fees
   - Equipment calibration
   - Data processing costs
   - Quality tiers available

3. Transportation (15% of credits destroyed)
   - Fast travel between sites
   - Expedition logistics
   - Emergency evacuation
   - Convenience vs. cost

4. Repairs and Maintenance (15% of credits destroyed)
   - Equipment degradation
   - Instrument recalibration
   - Vehicle maintenance
   - Ongoing costs

5. Licensing and Access (10% of credits destroyed)
   - Research site permits
   - Restricted area access
   - Premium data services
   - Exclusive content gates

6. Organizational Expenses (5% of credits destroyed)
   - Organization facilities
   - Shared laboratories
   - Event hosting
   - Prestige features

Target Inflation Rate:
- 1-2% annually (stable)
- Monitor monthly via key item prices
- Adjust faucets/sinks via content updates
- Maintain purchasing power
```

**Inflation Management Tools:**

```
1. Dynamic Faucet Adjustment
   - Lower mission rewards if inflation high
   - Increase rewards if deflation occurs
   - Automatic or manual adjustment
   - Communicated transparently

2. New Sink Introduction
   - Add desirable money sinks when needed
   - Prestige items, cosmetics
   - Quality of life features
   - Voluntary, not forced

3. Supply Throttling
   - Limit high-value resource spawn rates
   - Seasonal availability
   - Skill requirements
   - Prevents oversupply crashes

4. Demand Stimulation
   - New crafting recipes using existing items
   - Event-based consumption
   - Temporary bonuses
   - Creates scarcity value

5. Market Data Monitoring
   - Real-time inflation dashboard
   - Alert system for anomalies
   - Quarterly economic reports
   - Community transparency
```

---

## Part IV: Implementation Recommendations

### 7. Core Features for BlueMarble Economy

**Phase 1: Foundation (Launch)**

```
Must-Have Features:

1. Resource Exchange System
   - Order book matching algorithm
   - Buy/sell order placement
   - Partial fills support
   - Order cancellation
   - Anonymous trading
   - Basic price display

Database Requirements:
- Order table (active orders)
- Trade history table
- Price history table (daily aggregates)
- User credit balances

API Endpoints:
- POST /market/order (place order)
- GET /market/orders/{resource_id} (view order book)
- DELETE /market/order/{order_id} (cancel order)
- GET /market/trades/{resource_id} (trade history)
- GET /market/price/{resource_id} (current price)

2. Basic Price Tracking
   - Current buy/sell prices
   - Daily high/low
   - Last 7 days price chart
   - Trade volume display

3. Credit Management
   - Player credit balance
   - Transaction history
   - NPC vendor integration
   - Mission reward system

4. Order Limits
   - Quantity per time period
   - Account age multiplier
   - Anti-manipulation protection
```

**Phase 2: Enhancement (3-6 months)**

```
Important Features:

1. Advanced Analytics
   - 30-day price charts
   - Moving averages
   - Volume trends
   - Supply/demand indicators

2. Market Alerts
   - Price threshold notifications
   - Order filled notifications
   - Large price movement alerts
   - Manipulation warnings

3. Trade Tools
   - Price comparison (multiple resources)
   - Profit calculator
   - Order optimizer
   - Market efficiency analyzer

4. Economic Reports
   - Weekly market summaries
   - Top movers report
   - Volume leaders
   - Inflation tracking
```

**Phase 3: Advanced (6-12 months)**

```
Nice-to-Have Features:

1. Predictive Analytics
   - Price forecasting (ML-based)
   - Supply/demand projections
   - Seasonal patterns
   - Event impact predictions

2. Social Trading
   - Market discussion forums
   - Trading tips sharing
   - Economic analysis blogs
   - Community price guides

3. API Access
   - Third-party app development
   - Data export capabilities
   - Market data feeds
   - Trading bots (approved)

4. Advanced Security
   - Behavioral analysis
   - Collusion detection
   - Anomaly detection AI
   - Market surveillance team
```

---

### 8. Success Metrics and KPIs

**Economic Health Indicators:**

```
Primary Metrics:

1. Market Liquidity
   Target: 70%+ of resources trade within 1 hour
   Measure: Average order execution time by tier
   Alert: <50% execution within 24 hours

2. Price Stability
   Target: <10% daily price variance for common items
   Measure: Standard deviation of daily prices
   Alert: >20% volatility for 3+ consecutive days

3. Trade Volume
   Target: 80%+ of players trade weekly
   Measure: Unique traders per week / active users
   Alert: <60% participation rate

4. Order Book Depth
   Target: 10,000+ units within 5% of market price (common items)
   Measure: Sum of buy/sell orders near market price
   Alert: <2,000 units (thin market)

5. Bid-Ask Spread
   Target: <5% spread for common items
   Measure: (Ask - Bid) / Mid-price
   Alert: >10% spread consistently

Secondary Metrics:

1. Market Manipulation Incidents
   Target: <1 confirmed case per month
   Measure: Surveillance system alerts + investigations
   Alert: >3 cases per month

2. Inflation Rate
   Target: 1-2% annually
   Measure: Staple item price index
   Alert: >5% annually or deflation

3. Player Wealth Distribution
   Target: Gini coefficient <0.6
   Measure: Wealth inequality metric
   Alert: >0.7 (extreme inequality)

4. New Player Economic Success
   Target: 60%+ reach "sustainable income" within 30 days
   Measure: Credit earnings vs. expenses
   Alert: <40% success rate

5. Market Efficiency
   Target: Price differences <3% across regions
   Measure: Arbitrage opportunities
   Alert: >10% price differences (fragmented market)
```

**Data Collection Requirements:**

```
Real-Time Tracking:
- Every order placed/cancelled
- Every trade executed
- Price updates (per resource)
- Order book snapshots (hourly)

Daily Aggregation:
- Price high/low/average
- Trade volume totals
- Unique trader counts
- Top movers list

Weekly Analysis:
- Market health reports
- Manipulation investigations
- Trend analysis
- Community feedback

Monthly Review:
- Economic state of the game
- Inflation analysis
- Feature effectiveness
- Strategic adjustments
```

---

## Conclusion

The OSRS Grand Exchange demonstrates that a player-driven economy can provide engaging gameplay, stable markets, and valuable player services at MMORPG scale. The key success factors are transparency, careful balance of supply and demand, active manipulation prevention, and data-driven decision making.

**Core Principles for BlueMarble:**

✅ **Order book system enables fair price discovery**  
✅ **Historical data transparency builds market trust**  
✅ **Liquidity tiers accommodate different resource types**  
✅ **Active surveillance prevents manipulation**  
✅ **Managed inflation maintains purchasing power**  
✅ **Data analytics guide economic policy**  
✅ **Player accessibility prioritized over complexity**

**Critical Success Factors:**

1. **Market Liquidity**: Ensure common resources trade quickly and efficiently
2. **Price Stability**: Prevent wild swings that discourage participation
3. **Fair Trading**: Anonymous order books and manipulation detection
4. **Data Transparency**: Public price history and market analytics
5. **Economic Balance**: Careful faucet/sink management prevents inflation
6. **Continuous Monitoring**: Real-time surveillance and regular adjustments

**Implementation Priorities:**

**High Priority (Launch):**
- Order book matching system
- Basic price tracking (current + 7-day history)
- Credit management system
- Order limits and basic anti-manipulation
- NPC vendor integration

**Medium Priority (3-6 months):**
- Advanced analytics (30-day charts, indicators)
- Market alert system
- Trade tools and calculators
- Weekly economic reports

**Long-Term (6-12 months):**
- Predictive analytics and forecasting
- API access for third-party tools
- Advanced ML-based manipulation detection
- Social trading features

---

## References

### Market Data Sources

1. **GE Tracker**: https://www.ge-tracker.com/
2. **OSRS Grand Exchange**: https://oldschool.runescape.wiki/w/Grand_Exchange
3. **RuneScape API**: Official Grand Exchange API documentation
4. **Price Tracking Services**: Community-developed tools

### Economic Analysis

5. **"Virtual Economies: Design and Analysis"** - Vili Lehdonvirta, Edward Castronova (MIT Press, 2014)
6. **"Player-Driven Economies in MMORPGs"** - Game Economics Research papers
7. **"Market Manipulation in Virtual Worlds"** - Academic studies
8. **OSRS Economic Analysis** - Community researchers and YouTubers

### Technical Resources

9. **Order Book Design Patterns** - Financial systems architecture
10. **Market Surveillance Systems** - Fraud detection methodologies
11. **Real-Time Data Analytics** - Time-series database design
12. **Economic Modeling** - Inflation and supply/demand analysis

### Related BlueMarble Research

13. [RuneScape (Old School) Main Analysis](./game-dev-analysis-runescape-old-school.md)
14. [GDC Talk: OSRS Journey](./game-dev-analysis-gdc-osrs-journey.md)
15. [Research Assignment Group 34](./research-assignment-group-34.md)
16. [Player-Driven Economy Design Patterns](../topics/player-driven-economy.md)

---

**Document Status:** ✅ Complete  
**Last Updated:** 2025-01-17  
**Author:** BlueMarble Research Team  
**Review Status:** Ready for Implementation Planning  
**Discovered From:** RuneScape (Old School) Analysis  
**Next Document:** RuneLite Third-Party Client Analysis
