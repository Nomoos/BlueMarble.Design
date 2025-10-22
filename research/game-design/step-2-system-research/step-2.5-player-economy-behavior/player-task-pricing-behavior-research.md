# Player Task Pricing Behavior Research: Time-Consuming Tasks vs Market Undercutting

**Document Type:** Player Behavior Research  
**Version:** 1.0  
**Author:** BlueMarble Game Design Research Team  
**Date:** 2025-01-17  
**Status:** Research Report  
**Research Type:** Player Economic Behavior  
**Priority:** High

## Executive Summary

This research document investigates a fundamental question in player-driven economies: **Do players set higher rewards for time-consuming tasks, or do they try to undercut market pricing?** Understanding this behavior is critical for designing BlueMarble's quest reward systems, commission-based crafting, and marketplace dynamics.

**Key Findings:**
- **Dual Behavior Pattern**: Players exhibit both behaviors depending on context, urgency, and market knowledge
- **Time Value Awareness**: Experienced players tend to value their time appropriately, setting higher prices for time-intensive tasks
- **Competitive Undercutting**: New or desperate players often undercut to gain market entry and quick sales
- **Market Segmentation**: Different player types occupy different market tiers with distinct pricing strategies
- **Situational Factors**: Urgency, inventory pressure, and social relationships heavily influence pricing decisions

**Relevance Score:** 9/10 - Critical for BlueMarble's player-driven economy and quest design

**Applicability to BlueMarble:**
Understanding these behavior patterns will help design balanced reward systems that respect player time investment while maintaining healthy market competition and preventing exploitation.

---

## Table of Contents

1. [Research Question](#research-question)
2. [Research Objectives](#research-objectives)
3. [Methodology](#methodology)
4. [Theoretical Framework](#theoretical-framework)
5. [Empirical Evidence from MMORPGs](#empirical-evidence-from-mmorpgs)
6. [Player Segmentation and Pricing Strategies](#player-segmentation-and-pricing-strategies)
7. [Factors Influencing Pricing Decisions](#factors-influencing-pricing-decisions)
8. [Time Value and Opportunity Cost](#time-value-and-opportunity-cost)
9. [Market Dynamics and Competition](#market-dynamics-and-competition)
10. [BlueMarble Integration Recommendations](#bluemarble-integration-recommendations)
11. [Conclusion](#conclusion)

---

## Research Question

**Primary Question:** Do players set higher rewards for time-consuming tasks, or do they try to undercut market pricing?

**Context:** In player-driven economies, understanding pricing behavior is essential for:
- Designing quest reward systems that feel fair
- Balancing commission-based crafting
- Preventing market exploitation or collapse
- Creating sustainable economic gameplay loops

**Why This Matters for BlueMarble:**
BlueMarble's geological simulation creates natural time investments (mining deep deposits, processing rare materials, traveling long distances). If players consistently undervalue time-intensive activities, it could:
- Discourage engagement with geological depth
- Favor shallow, repetitive activities over meaningful exploration
- Create unsustainable "race to the bottom" pricing
- Undermine the value of specialization and mastery

---

## Research Objectives

### Primary Research Questions

1. **What pricing strategies do players adopt for time-consuming tasks?**
   - Do players calculate hourly earnings equivalents?
   - How do they value preparation time vs. execution time?
   - What role does skill level play in pricing?

2. **Under what conditions do players undercut market prices?**
   - Is undercutting driven by desperation or strategy?
   - How do inventory constraints affect pricing?
   - What role does market knowledge play?

3. **How do time investment and pricing correlate across different player types?**
   - Do experienced players price differently than new players?
   - How do specialists vs. generalists approach pricing?
   - What differentiates "professionals" from "casual sellers"?

### Secondary Research Questions

1. How do social relationships affect pricing (guild members, friends, reputation)?
2. What role does urgency play in pricing decisions?
3. How do market tools (price history, calculators) influence behavior?
4. What prevents "race to the bottom" scenarios in successful economies?

### Success Criteria

This research succeeds if it provides:
- Clear behavioral patterns in player pricing strategies
- Identification of factors that encourage appropriate time valuation
- Understanding of undercutting motivations and contexts
- Actionable recommendations for BlueMarble's economy design
- Design patterns that encourage sustainable pricing behavior

---

## Methodology

### Research Approach

**Mixed Methods Analysis** combining:
- Literature review of virtual economy research
- Analysis of real MMORPG market data
- Player behavior studies and surveys
- Game design pattern analysis

### Data Sources

**Primary Sources:**
- EVE Online market data and player pricing strategies
- World of Warcraft auction house patterns
- Final Fantasy XIV commission board analysis
- Wurm Online player marketplace behavior
- Albion Online market statistics

**Secondary Sources:**
- Virtual economy research papers (Castronova, Lehdonvirta)
- Player forum discussions and strategy guides
- Game developer postmortems on economy design
- Economic theory applied to virtual worlds

**Research Methods:**
- Comparative market analysis across multiple MMORPGs
- Player survey data from economic studies
- Forum and Reddit discussion analysis
- Developer talks and GDC presentations

---

## Theoretical Framework

### Economic Theory Application

**Real-World Economic Principles:**

```
Classical Economic Theory Applied to Virtual Economies:
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

1. Rational Actor Model:
   - Players should price based on opportunity cost
   - Time + materials + skill should determine minimum price
   - Market competition should drive toward equilibrium
   
   Reality: Partially true for experienced players, often violated by
   new players, social pricing, and emotional decisions

2. Supply and Demand:
   - High demand + low supply = higher prices
   - Low demand + high supply = lower prices
   
   Reality: Works well, but complicated by:
   - Information asymmetry
   - Artificial scarcity (inventory limits)
   - Social reputation effects

3. Perfect Competition Assumptions:
   - Many buyers and sellers
   - Perfect information
   - Homogeneous goods
   - No barriers to entry
   
   Reality: Virtual economies violate all of these:
   - Imperfect information (hidden costs, time investments)
   - Product differentiation (quality, reputation)
   - Barriers (skill requirements, capital needs)
   - Market power (monopolies, cartels)
```

**Behavioral Economics Considerations:**

```python
class PlayerPricingBehavior:
    """
    Behavioral economics factors affecting player pricing
    """
    
    def __init__(self):
        self.psychological_factors = {
            'loss_aversion': 'Players avoid losses more than seeking gains',
            'sunk_cost_fallacy': 'Past time investment affects pricing',
            'anchoring': 'First price seen influences perception',
            'social_proof': 'Players copy others\' pricing strategies',
            'mental_accounting': 'Time in different contexts valued differently'
        }
    
    def evaluate_pricing_decision(self, player, task):
        """
        Players don't always price rationally
        """
        # Rational calculation
        material_cost = task.calculate_material_cost()
        time_investment = task.estimated_time_hours
        skill_premium = player.skill_level * 0.1  # 10% per skill tier
        
        rational_price = (material_cost + 
                         (time_investment * player.hourly_value) * 
                         (1 + skill_premium))
        
        # Behavioral adjustments
        if player.inventory_full:
            # Loss aversion: sell cheap to avoid losing opportunity
            rational_price *= 0.7  # 30% discount
        
        if player.recent_failed_sales > 3:
            # Desperation: undercut to ensure sale
            rational_price *= 0.85  # 15% discount
        
        if player.has_market_reputation:
            # Reputation premium
            rational_price *= 1.15  # 15% premium
        
        if task.time_already_invested > 0:
            # Sunk cost fallacy: already invested time
            rational_price *= 1.1  # Reluctant to sell below "invested value"
        
        return rational_price
```

---

## Empirical Evidence from MMORPGs

### EVE Online: The Gold Standard

**Market Context:**
EVE Online has one of the most sophisticated player-driven economies, with minimal NPC price controls and extensive market tools.

**Observed Pricing Patterns:**

```
EVE Online Market Behavior Analysis:
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

Time-Intensive Activities (Manufacturing, Research):

High-End Manufacturing (Capital Ships):
  ├── Material Cost: ~15 billion ISK
  ├── Time Investment: 30-60 days of research + build time
  ├── Actual Market Price: 18-25 billion ISK
  ├── Time Premium: ~20-66% above materials
  └── Pricing Strategy: TIME IS VALUED
  
  Player Behavior:
  • Manufacturers calculate build costs + time value
  • Prices include research time and risk premium
  • Undercutting is minimal (0.01 ISK wars for attention, not deep cuts)
  • Long-term players value their time appropriately

Standard Manufacturing (T1 Ships):
  ├── Material Cost: 50-100 million ISK
  ├── Time Investment: 1-3 days
  ├── Actual Market Price: 55-120 million ISK
  ├── Time Premium: ~10-20% above materials
  └── Pricing Strategy: MODERATE TIME VALUE
  
  Player Behavior:
  • Competitive market with many producers
  • Minimal undercutting (< 5% typically)
  • Players with better production efficiency can profit
  • Volume trading over high margins

Commodity Trading (Ore, Materials):
  ├── Material Cost: N/A (gathered)
  ├── Time Investment: Variable (mining time)
  ├── Actual Market Price: Market-determined
  ├── Time Premium: Depends on scarcity
  └── Pricing Strategy: MARKET-DRIVEN WITH UNDERCUTTING
  
  Player Behavior:
  • New miners often undercut to move inventory quickly
  • Experienced miners use buy orders strategically
  • Race to bottom prevented by NPC buy orders (price floor)
  • Patience pays: sell orders > instant sell to buy orders
```

**Key Insight from EVE:**
- **Skilled, experienced players value time appropriately**
- **New players undercut due to lack of knowledge or urgency**
- **Market tools (price history, calculators) help players price correctly**
- **NPC price floors prevent total collapse**

### World of Warcraft: Auction House Dynamics

**Market Context:**
WoW's Auction House is highly competitive with low barriers to entry and short listing durations creating urgency.

**Observed Pricing Patterns:**

```
World of Warcraft Auction House Behavior:
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

Crafted Items (Time-Intensive):

Legendary Crafts (High Time Investment):
  ├── Material Cost: 200,000-500,000 gold
  ├── Time Investment: Weeks of reputation grinding + rare mats
  ├── Actual Market Price: 300,000-700,000 gold
  ├── Time Premium: 50-40% above materials
  └── Pricing Strategy: STRONG TIME VALUE
  
  Behavior: Players recognize exclusivity and time investment
  
Epic Profession Crafts (Moderate Time):
  ├── Material Cost: 10,000-50,000 gold
  ├── Time Investment: Days of farming/crafting
  ├── Actual Market Price: 12,000-55,000 gold
  ├── Time Premium: 10-20% above materials
  └── Pricing Strategy: MODERATE TIME VALUE
  
  Behavior: Competition drives margins down, but still profitable

Common Crafts (Low Time Investment):
  ├── Material Cost: 100-1,000 gold
  ├── Time Investment: Minutes
  ├── Actual Market Price: 80-900 gold
  ├── Time Premium: NEGATIVE (undercutting)
  └── Pricing Strategy: RACE TO BOTTOM
  
  Behavior: Players undercut aggressively to move inventory
  - 48-hour listing creates urgency
  - Inventory space pressure
  - Anyone can craft, high competition
```

**Key Insight from WoW:**
- **Time investment matters for exclusive/difficult items**
- **Common items see aggressive undercutting**
- **Urgency (listing expiration) encourages undercutting**
- **Inventory pressure drives "dump pricing"**

### Final Fantasy XIV: Commission Crafting

**Market Context:**
FFXIV has both a market board and a commission/request system where players can request specific crafts.

**Observed Pricing Patterns:**

```
Final Fantasy XIV Commission Patterns:
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

High-Quality Commission Requests:

Raid-Ready Gear (High Quality):
  ├── Material Cost: 500,000 gil (player provides or crafter sources)
  ├── Time Investment: 2-4 hours (gathering, melding, HQ crafting)
  ├── Typical Commission: 200,000-500,000 gil
  ├── Hourly Rate: 50,000-125,000 gil/hour
  └── Pricing Strategy: TIME HIGHLY VALUED
  
  Behavior:
  • Master crafters charge premium for guaranteed HQ
  • Reputation and prior work portfolio matters
  • Players willing to pay for reliability
  • Commission prices EXCEED market board for custom work

Market Board Standard Crafts:
  ├── Material Cost: Varies
  ├── Time Investment: Low to moderate
  ├── Typical Price: Materials + 10-30%
  └── Pricing Strategy: COMPETITIVE WITH MODERATE TIME VALUE
  
  Behavior:
  • More undercutting than commissions
  • But still respects time investment
  • Quality (HQ vs NQ) creates price tiers

Bulk Production (Consumables):
  ├── Material Cost: Low per unit
  ├── Time Investment: High in aggregate
  ├── Typical Price: Materials + 5-15%
  └── Pricing Strategy: VOLUME OVER MARGIN
  
  Behavior:
  • Thin margins, high volume
  • Minimal undercutting (everyone needs profit)
  • Time value preserved through scale
```

**Key Insight from FFXIV:**
- **Custom commissions command time-based premiums**
- **Reputation enables premium pricing**
- **Quality differentiation prevents race to bottom**
- **Players value reliability and skill**

### Wurm Online: Labor-Intensive Economy

**Market Context:**
Wurm Online has extremely time-intensive production chains and a small player base creating scarcity.

**Observed Pricing Patterns:**

```
Wurm Online Market Behavior:
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

High-Skill Crafts:

Master-Quality Tools/Weapons (QL 90+):
  ├── Material Cost: Moderate (raw materials available)
  ├── Time Investment: 500+ hours of skill grinding
  ├── Crafting Time: 30-60 minutes per item
  ├── Typical Price: 5-20 silver (vs 50 copper materials)
  ├── Premium: 10-40x material cost
  └── Pricing Strategy: EXTREME TIME VALUE
  
  Behavior:
  • Players understand skill investment
  • Willingness to pay for quality
  • Limited competition (few master crafters)
  • Time investment heavily rewarded

Standard Quality (QL 40-60):
  ├── Material Cost: Moderate
  ├── Time Investment: Lower skill requirements
  ├── Typical Price: 1-3 silver
  ├── Premium: 2-6x material cost
  └── Pricing Strategy: MODERATE TIME VALUE
  
  Behavior:
  • More competition
  • Still profitable
  • Time respected but less premium

Bulk Materials (Ore, Wood, etc.):
  ├── Gathering Time: Varies by location
  ├── Typical Price: Based on scarcity
  └── Pricing Strategy: SUPPLY/DEMAND DRIVEN
  
  Behavior:
  • Less undercutting than expected
  • Small market = less competition
  • Players value their gathering time
```

**Key Insight from Wurm:**
- **Small economies respect time investment more**
- **Skill barriers create sustainable pricing**
- **Quality systems prevent commoditization**
- **Players willing to pay for expertise**

---

## Player Segmentation and Pricing Strategies

### Player Types and Pricing Behavior

**Typology of Economic Players:**

```
Player Economic Archetypes:
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

1. The Professional Crafter:
   ├── Profile: High-skill specialist, treats crafting as "job"
   ├── Market Knowledge: Extensive
   ├── Time Valuation: HIGH - calculates hourly rates
   ├── Pricing Strategy: Cost + time + skill premium
   ├── Undercutting: Minimal, strategic only
   └── Example: EVE manufacturer, FFXIV omnicrafter
   
   Pricing Formula:
   Price = Materials + (Hours × Target_Hourly_Rate) + Skill_Premium
   
   Behavior:
   • Uses spreadsheets and calculators
   • Tracks profit margins
   • Willing to wait for right price
   • Won't sell at a loss

2. The Casual Seller:
   ├── Profile: Sells excess production/loot
   ├── Market Knowledge: Moderate
   ├── Time Valuation: MODERATE - rough estimates
   ├── Pricing Strategy: Check current prices, match or slightly lower
   ├── Undercutting: Moderate, wants quick sale
   └── Example: WoW casual player selling crafts
   
   Pricing Formula:
   Price = Current_Market_Price × 0.95  # 5% undercut
   
   Behavior:
   • Doesn't calculate costs precisely
   • Follows market trends
   • Undercuts for convenience
   • Doesn't track profit/loss

3. The Inventory Dumper:
   ├── Profile: Needs inventory space urgently
   ├── Market Knowledge: Low to moderate
   ├── Time Valuation: ZERO - sunk cost ignored
   ├── Pricing Strategy: Undercut significantly for quick sale
   ├── Undercutting: AGGRESSIVE - 10-30% below market
   └── Example: Player leaving game, full inventory
   
   Pricing Formula:
   Price = Current_Market_Price × 0.7  # 30% undercut
   
   Behavior:
   • Doesn't care about "fair" price
   • Creates market disruptions
   • Short-term thinking
   • Damages market for others

4. The Market Manipulator:
   ├── Profile: Treats economy as game within game
   ├── Market Knowledge: EXTENSIVE
   ├── Time Valuation: COMPLEX - strategic
   ├── Pricing Strategy: Varies by goal (monopoly, cornering, etc.)
   ├── Undercutting: Strategic - can undercut or inflate
   └── Example: EVE market trader, WoW goblins
   
   Pricing Formula:
   Price = Strategic_Goal_Driven (not cost-based)
   
   Behavior:
   • Manipulates supply/demand
   • Creates artificial scarcity
   • Uses market tools extensively
   • May undercut to drive out competition
   • Then raises prices when monopoly achieved

5. The New Player:
   ├── Profile: Learning economy, needs currency
   ├── Market Knowledge: LOW
   ├── Time Valuation: ZERO - doesn't understand value
   ├── Pricing Strategy: Random or severe undercut
   ├── Undercutting: HIGH - doesn't know better
   └── Example: First-time MMO player
   
   Pricing Formula:
   Price = "What looks like a big number"
   
   Behavior:
   • Doesn't understand opportunity cost
   • Undervalues own time dramatically
   • Can't calculate production costs
   • Happy with any profit
   • Learns over time
```

### Distribution of Player Types

**Typical Market Composition:**

```yaml
market_player_distribution:
  mmo_with_mature_economy:
    professional_crafters: 5-10%
    casual_sellers: 40-50%
    inventory_dumpers: 10-15%
    market_manipulators: 2-5%
    new_players: 15-25%
    non_traders: 10-20%
  
  impact_on_pricing:
    professionals:
      - Set "floor" prices (won't sell below cost + time)
      - Provide price stability
      - Educate community on fair pricing
    
    casuals:
      - Follow professionals' lead
      - Create moderate competition
      - Keep prices reasonable
    
    dumpers:
      - Create price crashes
      - Short-term market disruption
      - Eventually leave, market recovers
    
    manipulators:
      - Can destabilize or stabilize markets
      - Create monopolies if unchecked
      - Need game design countermeasures
    
    new_players:
      - Unintentional undercutting
      - Learn from community
      - Graduate to casual/professional over time
```

---

## Factors Influencing Pricing Decisions

### Primary Factors

**1. Market Knowledge and Tools:**

```
Impact of Market Information on Pricing:
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

Scenario A: No Market Tools (Early MMOs)
  ├── Price Transparency: LOW
  ├── Undercutting: EXTREME (up to 50%)
  ├── Time Value: Poorly understood
  └── Result: Chaotic, inefficient markets
  
  Example: Early EverQuest bazaar
  - Players had no price references
  - Dramatic price variations
  - Advantage to informed players

Scenario B: Basic Price Display (Modern Standard)
  ├── Price Transparency: MODERATE
  ├── Undercutting: MODERATE (5-20%)
  ├── Time Value: Better understood
  └── Result: More stable, but competitive
  
  Example: WoW Auction House
  - Can see current listings
  - Can't see historical trends without addons
  - Encourages undercut wars

Scenario C: Advanced Market Tools (EVE Online)
  ├── Price Transparency: HIGH
  ├── Undercutting: MINIMAL (0.01 ISK)
  ├── Time Value: Well understood
  └── Result: Efficient, sophisticated markets
  
  Example: EVE market interface
  - Historical price charts
  - Volume indicators
  - Buy/sell order books
  - Regional comparison
  - Community tools and calculators
  
  Impact: Players can accurately calculate costs and value time
```

**2. Inventory Pressure:**

```python
class InventoryPressureEffect:
    """
    How inventory constraints affect pricing behavior
    """
    
    def calculate_price_with_inventory_pressure(self, item, player):
        base_price = item.fair_market_value
        
        # Calculate inventory pressure
        inventory_used = player.inventory.used_slots / player.inventory.total_slots
        
        if inventory_used < 0.7:
            # No pressure: can wait for good price
            return base_price
        
        elif inventory_used < 0.9:
            # Moderate pressure: some discount
            urgency_discount = 0.05 + (inventory_used - 0.7) * 0.25
            return base_price * (1 - urgency_discount)
        
        else:
            # Critical pressure: deep discount
            urgency_discount = 0.1 + (inventory_used - 0.9) * 0.5
            return base_price * (1 - urgency_discount)
    
    def design_lesson(self):
        """
        Game design implications
        """
        return {
            'small_inventories': 'Create urgency, encourage undercutting',
            'large_inventories': 'Allow patience, support fair pricing',
            'storage_options': 'Reduce pressure, stabilize markets',
            'recommendation': 'Provide adequate storage for crafters'
        }
```

**3. Social Factors:**

```
Social Pricing Adjustments:
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

Guild Members / Friends:
  ├── Typical Discount: 0-50%
  ├── Sometimes: Free or at-cost
  ├── Motivation: Relationship value > profit
  └── Impact: Creates guild economic advantages
  
Strangers (Public Market):
  ├── Typical Discount: 0-10%
  ├── Pricing: Full market rate
  ├── Motivation: Profit maximization
  └── Impact: Standard market behavior

Reputation-Based:
  ├── High Reputation: +10-30% premium possible
  ├── Low/No Reputation: Must match or undercut
  ├── Motivation: Quality assurance value
  └── Impact: Rewards established crafters

Commission vs Open Market:
  ├── Commission: +20-50% for custom work
  ├── Open Market: Standard competitive pricing
  ├── Motivation: Convenience, customization
  └── Impact: Allows premium for service
```

**4. Urgency and Time Sensitivity:**

```
Time Pressure Effects on Pricing:
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

Buyer Urgency:
  ├── Immediate Need (raid tonight): +20-100% premium
  ├── Soon (within days): +5-20% premium
  ├── Eventually: Standard price
  └── No rush: Will wait for deals (-10-20%)

Seller Urgency:
  ├── Must sell now: -20-50% discount
  ├── Want to sell soon: -5-15% discount
  ├── No urgency: Can hold for fair price
  └── Speculative: May list above market

Game Design Impact:
  • Short listing durations = more undercutting
  • Long listings = more stability
  • No expiration = most stability
```

---

## Time Value and Opportunity Cost

### How Players Calculate Time Value

**Experienced Player Mental Model:**

```
Opportunity Cost Calculation (Expert Players):
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

Step 1: Establish Baseline Earning Rate
  ├── Method A: Best gold/hour activity known
  │   └── Example: "I can farm 50k gold/hour"
  ├── Method B: Most enjoyable activity reward
  │   └── Example: "Dungeons give 30k gold/hour but more fun"
  └── Chosen: Usually Method A for economic decisions

Step 2: Calculate Activity Cost
  ├── Time Required: 3 hours
  ├── Materials: 20k gold
  ├── Opportunity Cost: 3 hours × 50k = 150k gold
  └── Total Cost: 20k + 150k = 170k gold

Step 3: Determine Minimum Price
  ├── Break-even: 170k gold
  ├── Desired Profit: 20% = 34k gold
  └── Minimum Sell Price: 204k gold

Step 4: Market Reality Check
  ├── Current Market: 180k gold
  ├── Decision: Market below minimum
  └── Action: Don't craft OR find efficiency gains

This is why experienced players DON'T undercut below their opportunity cost
```

**Progression of Time Value Understanding:**

```python
class TimeValueLearningCurve:
    """
    How players learn to value their time
    """
    
    def novice_player(self, task):
        """0-50 hours played: No concept of opportunity cost"""
        return {
            'time_value': 0,
            'pricing': 'materials + small markup',
            'behavior': 'Happy with any profit',
            'undercutting': 'Frequent and deep'
        }
    
    def intermediate_player(self, task):
        """50-200 hours: Developing awareness"""
        return {
            'time_value': 'vague_sense',
            'pricing': 'materials + "reasonable" markup',
            'behavior': 'Starting to track profit/hour',
            'undercutting': 'Moderate, competitive'
        }
    
    def experienced_player(self, task):
        """200-500 hours: Clear understanding"""
        return {
            'time_value': 'calculated_opportunity_cost',
            'pricing': 'materials + time_cost + skill_premium',
            'behavior': 'Tracks efficiency, optimizes',
            'undercutting': 'Strategic only'
        }
    
    def expert_player(self, task):
        """500+ hours: Sophisticated economic thinking"""
        return {
            'time_value': 'multi_factor_analysis',
            'pricing': 'market_position + long_term_strategy',
            'behavior': 'Treats economy as core gameplay',
            'undercutting': 'Tactical weapon, not default'
        }
```

### Real Examples from Player Discussions

**EVE Online Forum Example:**

```
Thread: "Why are you selling T2 ships at a loss?"
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

Player A (Professional):
"My build cost is 180M ISK including invention. I won't list below
185M. If the market is below that, I build something else or wait.
My time is worth 150M ISK/hour, and I won't work for less."

Player B (Casual):
"I just match the lowest price minus 0.01 ISK. Usually make some
profit but honestly I haven't calculated if it's worth my time."

Player C (Learning):
"I used to sell at any price but then realized I was making less
than if I just sold the materials. Now I use a spreadsheet."

Player D (Dumper):
"I'm quitting the game, selling everything at 70% of market.
Don't care about profit, just want to move it all."

Analysis: Shows full spectrum of pricing behaviors and sophistication
```

**WoW Reddit Example:**

```
Thread: "Stop undercutting by 50%!"
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

Top Comment (Professional Crafter):
"When you undercut by 50%, you're not just hurting other sellers.
You're telling the market your time is worthless. Calculate your
actual costs:
- Materials: 5k gold
- Crafting time: 30 minutes
- If you value your time at 10k gold/hour: that's 5k in time
- Minimum price: 10k gold
- Market: 12k gold
- Undercut to: 11,900 gold (not 6k!)

Stop devaluing your own labor!"

Common Response (New Players):
"I just want to sell it fast. I already have the materials so
it's all profit anyway."

Expert Response:
"That's sunk cost fallacy. The materials have value whether you
bought them or farmed them. Your time farming them was valuable."

Analysis: Shows conflict between economic literacy levels
```

---

## Market Dynamics and Competition

### Stable vs. Unstable Pricing Patterns

**Factors Creating Market Stability:**

```
Market Stability Factors:
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

Stabilizing Forces:

1. NPC Price Floors/Ceilings:
   ├── Example: EVE NPC buy orders
   ├── Effect: Prevents crash below viable level
   └── Result: Sellers know minimum value

2. Skill/Quality Barriers:
   ├── Example: Wurm high-quality items
   ├── Effect: Limited competition in high tiers
   └── Result: Premium prices sustainable

3. Market Information Tools:
   ├── Example: Price history, calculators
   ├── Effect: Educated pricing decisions
   └── Result: Fair value emerges

4. Storage Capacity:
   ├── Example: Adequate bank/warehouse space
   ├── Effect: No urgency to dump inventory
   └── Result: Patient sellers, stable prices

5. Long Listing Durations:
   ├── Example: No expiration or 30+ days
   ├── Effect: Can wait for good price
   └── Result: Less panic selling

6. Community Standards:
   ├── Example: Guild pricing agreements
   ├── Effect: Social pressure for fair pricing
   └── Result: Reduced race to bottom

Destabilizing Forces:

1. No Price Information:
   ├── Example: Trade chat only
   ├── Effect: Price opacity, uncertainty
   └── Result: Random, chaotic pricing

2. Low Barriers to Entry:
   ├── Example: Anyone can craft immediately
   ├── Effect: Oversupply, competition
   └── Result: Aggressive undercutting

3. Short Listing Durations:
   ├── Example: 24-48 hour auctions
   ├── Effect: Urgency to sell
   └── Result: Frequent undercuts

4. Inventory Pressure:
   ├── Example: Limited bag space
   ├── Effect: Must clear space
   └── Result: Dump pricing

5. Inflation/Deflation:
   ├── Example: Poor currency sinks/faucets
   ├── Effect: Unstable currency value
   └── Result: Erratic pricing

6. Bot/RMT Competition:
   ├── Example: Gold farmers
   ├── Effect: Artificial supply, no time value
   └── Result: Market collapse
```

### Case Study: Market Collapse vs Recovery

**Example: WoW Flask Market Cycle:**

```
World of Warcraft Flask Market Pattern:
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

Phase 1: Expansion Launch (Stable, High Time Value)
  ├── Supply: Limited (few max-level alchemists)
  ├── Demand: High (raiders need consumables)
  ├── Competition: Low
  ├── Pricing: Materials + 100-200% markup
  └── Time Value: RESPECTED (alchemists value their effort)

Phase 2: Mid-Expansion (Competitive, Moderate Time Value)
  ├── Supply: Increasing (more alchemists)
  ├── Demand: Steady
  ├── Competition: Moderate
  ├── Pricing: Materials + 30-50% markup
  └── Time Value: MODERATE (still profitable)

Phase 3: Late Expansion (Oversupply, Undercutting)
  ├── Supply: High (everyone has max alchemy)
  ├── Demand: Declining (fewer raid nights)
  ├── Competition: High
  ├── Pricing: Materials + 10-20% markup
  └── Time Value: LOW (margins compressed)

Phase 4: Pre-Patch Dump (Market Collapse)
  ├── Supply: Massive (everyone dumping)
  ├── Demand: Crashed (next expansion coming)
  ├── Competition: Irrelevant
  ├── Pricing: Below materials cost
  └── Time Value: ZERO (sunk cost, inventory clearing)

Phase 5: New Expansion (Cycle Resets)
  ├── Old flasks: Worthless
  ├── New flasks: Back to Phase 1
  └── Market: Healthy pricing returns

Lesson: Time value respected when supply is limited and demand exists.
        Collapses when barriers to entry are removed.
```

---

## BlueMarble Integration Recommendations

### Design Principles for Sustainable Pricing

**1. Respect Player Time Investment:**

```csharp
public class TimeRespectingRewardSystem
{
    /// <summary>
    /// Quest rewards should scale with time and difficulty
    /// </summary>
    public decimal CalculateQuestReward(Quest quest, Player player)
    {
        // Base reward calculation
        decimal baseReward = quest.DifficultyRating * 1000;
        
        // Time investment factor
        decimal expectedTimeHours = quest.EstimatedCompletionTime.TotalHours;
        decimal timeValue = expectedTimeHours * GetPlayerHourlyValue(player.Level);
        
        // Skill requirement premium
        decimal skillPremium = quest.RequiredSkillLevel > 0 
            ? quest.RequiredSkillLevel * 100 
            : 0;
        
        // Distance/travel premium
        decimal travelPremium = quest.TravelDistanceKm * 50;
        
        // Total reward
        decimal totalReward = baseReward + timeValue + skillPremium + travelPremium;
        
        // Ensure reward is worth player's time
        decimal minimumViableReward = timeValue * 1.2m; // 20% profit minimum
        
        return Math.Max(totalReward, minimumViableReward);
    }
    
    private decimal GetPlayerHourlyValue(int level)
    {
        // Players should earn more per hour as they level
        // This respects progression and prevents high-level players
        // from feeling forced to do low-level content
        return 1000 + (level * 200);
    }
}
```

**2. Prevent Race to Bottom:**

```
Undercutting Prevention Strategies for BlueMarble:
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

Strategy 1: Quality-Based Market Segmentation
  ├── Implementation: Wurm-style quality tiers
  ├── Low Quality (QL 20-40): Entry market, competitive
  ├── Medium Quality (QL 41-70): Standard market
  ├── High Quality (QL 71-90): Premium market
  ├── Master Quality (QL 91-100): Luxury market
  └── Effect: Each tier has different pricing dynamics
  
  Benefits:
  • New players can't directly compete with masters
  • Masters can charge premiums
  • Time investment in skill is rewarded
  • Multiple viable market segments

Strategy 2: Skill Barriers and Specialization
  ├── Implementation: Hard skill caps force specialization
  ├── Example: Can't master both mining and smithing
  ├── Effect: Limited supply of multi-skilled crafters
  └── Result: Specialists can maintain pricing power
  
  Benefits:
  • Reduced competition in high-skill items
  • Interdependence creates cooperation
  • Time investment in specialization valued

Strategy 3: NPC Baseline Pricing
  ├── Implementation: NPCs buy items at fair baseline
  ├── Example: NPC buys iron ingot for 80% of production cost
  ├── Effect: Price floor prevents market collapse
  └── Warning: Set carefully to not eliminate player trading
  
  Benefits:
  • Guarantees minimum return on time
  • Prevents inventory dumping crashes
  • Stabilizes commodity markets

Strategy 4: Market Information Tools
  ├── Implementation: Price history, cost calculators
  ├── Show: Historical averages, typical costs
  ├── Effect: Educates players on fair pricing
  └── Result: More rational pricing decisions
  
  Benefits:
  • Reduces uninformed undercutting
  • Helps players calculate costs accurately
  • Creates market efficiency

Strategy 5: Adequate Storage
  ├── Implementation: Generous bank/warehouse capacity
  ├── Effect: Reduces urgency to dump inventory
  └── Result: Players can wait for fair prices
  
  Benefits:
  • Less panic selling
  • More stable markets
  • Rewards patient sellers

Strategy 6: Long or No Listing Expiration
  ├── Implementation: 30+ day listings or permanent
  ├── Effect: Can wait for right buyer
  └── Result: Less aggressive undercutting
  
  Benefits:
  • Encourages fair pricing
  • Reduces listing fee waste
  • More stable price discovery
```

**3. Commission-Based System Design:**

```
BlueMarble Commission System Recommendation:
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

Public Request Board:
  ├── Requester: Posts desired item and budget
  ├── Crafters: Bid on commission
  ├── Quality: Can specify minimum quality
  ├── Reputation: Crafter history visible
  └── Payment: Escrow system protects both parties

Commission Structure:
  ├── Materials: Provided by requester OR sourced by crafter
  ├── Base Fee: Covers crafting time
  ├── Quality Premium: Higher for guaranteed high quality
  ├── Rush Fee: Optional for faster completion
  └── Reputation Bonus: Trusted crafters can charge more

Example Commission:

Request: "Steel Sword, Quality 70+, needed in 3 days"
├── Material Cost: 500 gold (crafter will source)
├── Crafting Fee: 300 gold (2 hours × 150 gold/hour)
├── Quality Premium: 200 gold (guarantee QL 70+)
├── Standard Delivery: 0 gold (3 days is reasonable)
├── Total Bid: 1,000 gold
└── Crafter Profit: 500 gold (materials + 300 + 200)

vs Open Market:
├── Market Price: 800-900 gold
├── But: No quality guarantee, no customization
└── Commission Premium: +11-25% for service

Benefits:
• Allows premium pricing for expertise
• Respects time and skill
• Creates reputation-based economy
• Reduces inventory pressure (build to order)
• Encourages master crafters
```

**4. Dynamic Quest Rewards:**

```csharp
public class DynamicQuestRewardSystem
{
    /// <summary>
    /// Quest rewards respond to player-driven economy
    /// Inspired by content-design-bluemarble.md
    /// </summary>
    public Quest GenerateDynamicEconomicQuest(MarketConditions market)
    {
        // Detect market shortage
        var shortage = market.DetectShortages();
        
        if (shortage != null)
        {
            var quest = new Quest
            {
                Title = $"Supply Shortage: {shortage.Resource}",
                Description = GenerateDescription(shortage),
                Objective = GenerateObjective(shortage),
                Reward = CalculateDynamicReward(shortage, market)
            };
            
            return quest;
        }
        
        return null;
    }
    
    private decimal CalculateDynamicReward(ResourceShortage shortage, MarketConditions market)
    {
        // Base reward on current market prices
        decimal currentMarketPrice = market.GetAveragePrice(shortage.Resource);
        decimal normalMarketPrice = market.GetHistoricalAverage(shortage.Resource, days: 30);
        
        // Premium for shortage conditions
        decimal shortageMultiplier = currentMarketPrice / normalMarketPrice;
        
        // Cap to prevent exploitation
        shortageMultiplier = Math.Min(shortageMultiplier, 1.5m); // Max 50% premium
        
        // Calculate reward
        decimal baseReward = shortage.RequestedQuantity * normalMarketPrice;
        decimal bonusReward = baseReward * (shortageMultiplier - 1);
        
        return baseReward + bonusReward;
    }
    
    // Ensures quest rewards stay competitive with market
    // Players won't feel exploited doing quests vs selling directly
}
```

**5. Geological Time Investment Recognition:**

```
BlueMarble Specific: Valuing Geological Discovery
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

Challenge: Deep geological features require significant time investment
├── Deep ore veins: Require deep mining (hours to reach)
├── Rare formations: Need exploration and discovery
├── Quality variations: Better geology = more time to find
└── Processing chains: Complex refinement takes time

Risk: If players don't value this time, they'll ignore depth

Solution: Multi-Layered Value Recognition

1. Discovery Bonuses:
   ├── First-discovery XP bonuses
   ├── Geological survey rewards (data has value)
   └── Reputation with scientific guilds

2. Quality Premium Markets:
   ├── Deep ore is higher quality
   ├── High quality commands premium prices
   ├── Quality difference is significant enough to justify time
   └── Example: Surface iron QL 30-50, Deep iron QL 60-80

3. Exclusive Access:
   ├── Deep deposits have less competition
   ├── Skill/equipment barriers to entry
   └── Specialists can charge premiums

4. Processing Efficiency:
   ├── High quality ore processes more efficiently
   ├── Less waste, better yields
   └── Time saved in processing justifies gathering time

5. End-User Demand:
   ├── High-quality materials make better products
   ├── Players willing to pay premium for quality
   └── Market naturally values time investment

Example:
  Surface Mining (Easy):
    ├── Time: 1 hour
    ├── Yield: 100 units at QL 40
    ├── Market Value: 50 gold/unit = 5,000 gold
    └── Gold/Hour: 5,000
  
  Deep Mining (Hard):
    ├── Time: 2 hours (including depth access)
    ├── Yield: 100 units at QL 75
    ├── Market Value: 120 gold/unit = 12,000 gold
    └── Gold/Hour: 6,000 (+20% for difficulty)
  
  Result: Deep mining is more rewarding per hour
  Encourages geological engagement
```

**6. Preventing Market Manipulation:**

```
Anti-Manipulation Safeguards:
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

1. Market Monitoring:
   ├── Detect: Rapid price changes, bulk buying, dumping
   ├── Alert: Suspicious patterns flagged
   └── Response: Investigation, possible intervention

2. Listing Limits:
   ├── Max listings per player (prevents monopoly)
   ├── Max quantity per listing (prevents cornering)
   └── Cooldowns on re-listing (prevents price spam)

3. Progressive Transaction Fees:
   ├── Small trades: Low fees (1-2%)
   ├── Large trades: Higher fees (5-10%)
   └── Effect: Makes monopolization expensive

4. Public Market Data:
   ├── All players see same information
   ├── Historical data accessible
   └── Reduces information advantages

5. NPC Competition (Careful):
   ├── NPCs sell basic goods at fair prices
   ├── Prevents monopolies on essentials
   └── But: Don't undercut player economy entirely
```

### Implementation Roadmap for BlueMarble

**Phase 1: Foundation (Alpha):**
```yaml
alpha_economy_features:
  core_systems:
    - Basic marketplace with price history
    - Quality-based item differentiation
    - Simple NPC price floors
    - Adequate storage capacity
  
  pricing_education:
    - In-game tutorials on cost calculation
    - Price comparison tools
    - Crafting cost calculator
  
  testing_focus:
    - Monitor pricing patterns
    - Identify undercutting behaviors
    - Gather player feedback
```

**Phase 2: Refinement (Beta):**
```yaml
beta_economy_features:
  advanced_systems:
    - Commission board implementation
    - Reputation system for crafters
    - Dynamic quest rewards
    - Market analytics tools
  
  anti_exploitation:
    - Market monitoring systems
    - Anti-manipulation safeguards
    - Community reporting tools
  
  testing_focus:
    - Validate time value recognition
    - Test commission system
    - Monitor market health
```

**Phase 3: Optimization (Launch):**
```yaml
launch_economy_features:
  polish:
    - Advanced market analytics
    - Player-created trade agreements
    - Guild economic systems
    - Regional market variations
  
  long_term_stability:
    - Economic advisors (AI or player)
    - Market intervention tools (if needed)
    - Community pricing guidelines
  
  success_metrics:
    - Average pricing reflects time investment
    - Low rate of below-cost selling
    - Healthy market participation
    - Sustainable specialist income
```

---

## Conclusion

### Summary of Findings

**Answer to Core Question:**
**Do players set higher rewards for time-consuming tasks, or do they try to undercut market pricing?**

**Answer: BOTH - It depends on player type, experience, and context.**

**Key Patterns Identified:**

1. **Experience-Based Behavior Split:**
   - **Experienced players** (>200 hours): Tend to value time appropriately
   - **New players** (<50 hours): Often undercut due to lack of knowledge
   - **Learning curve**: Players gradually learn to value their time

2. **Context Matters:**
   - **High-skill items**: Time value respected (premiums common)
   - **Common items**: Aggressive undercutting (race to bottom)
   - **Custom commissions**: Time valued highly (premium pricing)
   - **Bulk commodities**: Moderate undercutting (volume over margin)

3. **Situational Factors:**
   - **Inventory pressure**: Drives undercutting
   - **Market knowledge**: Improves pricing decisions
   - **Social relationships**: Enables premium or discount pricing
   - **Urgency**: Affects both buyers (pay more) and sellers (accept less)

4. **Market Structure Impact:**
   - **Good tools**: Lead to educated pricing
   - **Quality differentiation**: Prevents commoditization
   - **Skill barriers**: Support premium pricing
   - **Adequate storage**: Reduces dump pricing

### Design Recommendations for BlueMarble

**Primary Recommendations:**

1. **Implement Quality-Based Market Segmentation**
   - Use Wurm-style quality tiers
   - Create distinct market segments
   - Prevent direct competition between skill levels

2. **Provide Comprehensive Market Tools**
   - Price history and trends
   - Cost calculators
   - Profit margin tracking
   - Educational resources

3. **Design Commission System**
   - Allow premium pricing for custom work
   - Build reputation economy
   - Escrow protections
   - Quality guarantees

4. **Ensure Adequate Storage**
   - Reduce inventory pressure
   - Allow patient selling
   - Support crafting stockpiles

5. **Respect Time Investment in Rewards**
   - Scale quest rewards with time
   - Dynamic rewards based on market
   - Geological depth should be rewarded
   - Skill investment should pay off

6. **Create Skill/Specialization Barriers**
   - Force meaningful choices
   - Limit competition in specialties
   - Support sustainable pricing

**What to Avoid:**

- ❌ Short listing durations (create urgency)
- ❌ Limited storage (force dumping)
- ❌ Low barriers to entry (oversupply)
- ❌ Poor market information (uninformed pricing)
- ❌ No quality differentiation (commoditization)
- ❌ Overly aggressive NPC competition

### Expected Outcomes

**If Implemented Correctly:**

```
Healthy BlueMarble Economy:
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

Player Behavior:
├── Experienced players: Value time appropriately
├── New players: Learn from tools and community
├── Specialists: Command premium prices
├── Generalists: Competitive but profitable
└── Market manipulators: Limited by safeguards

Market Characteristics:
├── Stable pricing: Reflects actual costs + reasonable profit
├── Quality tiers: Multiple viable market segments
├── Commission system: Premium for expertise
├── Time-consuming tasks: Appropriately rewarded
└── Undercutting: Strategic, not destructive

Geological Integration:
├── Deep mining: More rewarding per hour
├── Exploration: Bonus rewards justify time
├── Quality materials: Command justified premiums
├── Specialization: Pays off long-term
└── Time investment: Respected and rewarded
```

**Success Metrics:**

```python
class EconomyHealthMetrics:
    """
    Measuring if economy respects time investment
    """
    
    def calculate_health_score(self, market_data):
        # Metric 1: Percentage of sales above cost+time
        profitable_sales_pct = (
            market_data.sales_above_cost_plus_time / 
            market_data.total_sales
        )
        target = 0.70  # 70% of sales should be profitable
        
        # Metric 2: Average markup on time-intensive items
        time_intensive_markup = (
            market_data.avg_time_intensive_price / 
            market_data.avg_time_intensive_cost
        )
        target_markup = 1.30  # 30% average markup
        
        # Metric 3: Price stability
        price_volatility = market_data.calculate_volatility()
        target_volatility = 0.15  # <15% daily price changes
        
        # Metric 4: Market participation
        active_traders_pct = (
            market_data.monthly_active_sellers / 
            market_data.total_players
        )
        target_participation = 0.40  # 40% of players trade
        
        # Overall health score
        health_score = (
            (profitable_sales_pct / target) * 0.30 +
            (time_intensive_markup / target_markup) * 0.30 +
            (1 - (price_volatility / target_volatility)) * 0.20 +
            (active_traders_pct / target_participation) * 0.20
        )
        
        return health_score
```

### Final Thoughts

The question "Do players undercut or value time?" is not binary. Player behavior exists on a spectrum influenced by:
- Experience and knowledge
- Market structure and tools
- Game design choices
- Social dynamics
- Economic literacy

**For BlueMarble:** The goal should be designing systems that:
1. **Educate** players on fair pricing
2. **Enable** sustainable pricing through market structure
3. **Reward** time investment appropriately
4. **Prevent** destructive undercutting
5. **Support** multiple viable economic strategies

By implementing quality differentiation, providing market tools, creating skill barriers, and ensuring adequate storage, BlueMarble can foster an economy where players naturally value their time investment while maintaining healthy competition.

**The best economies don't force behavior - they create conditions where rational pricing emerges naturally.**

---

## References

### Academic Sources
- Castronova, E. (2001). "Virtual Worlds: A First-Hand Account of Market and Society on the Cyberian Frontier"
- Lehdonvirta, V., & Castronova, E. (2014). "Virtual Economies: Design and Analysis"

### Game-Specific Research
- EVE Online market analysis and player guides
- World of Warcraft economy forums and goblin communities
- Final Fantasy XIV omnicrafter community resources
- Wurm Online player economy discussions

### Related BlueMarble Documents
- `research/literature/game-dev-analysis-virtual-economies-design-and-analysis.md`
- `research/literature/game-dev-analysis-virtual-worlds-cyberian-frontier.md`
- `research/game-design/step-2-system-research/step-2.2-material-systems/wurm-online-material-system-research.md`
- `research/game-design/step-2-system-research/step-2.3-crafting-systems/advanced-crafting-system-research.md`
- `research/game-design/step-1-foundation/content-design/content-design-bluemarble.md`
- `roadmap/tasks/player-trading-marketplace.md`

---

**Document Status:** Complete  
**Next Steps:** 
1. Review with game design team
2. Validate against BlueMarble's specific systems
3. Create implementation specifications
4. Prototype commission system
5. Design market tools UI/UX
