# Step 3: Incentive Structures

## Overview

This step analyzes reward systems that motivate players to complete resource gathering quests. We examine how games balance rewards to maintain economic health while providing meaningful incentives for both quest creators and completers.

## Core Reward Types

### 1. Currency Rewards

**Pattern**: Direct payment in game currency

**Example**: "100 gold for completing this quest"

**Advantages**:
- Universal value
- Player choice in spending
- Simple to implement
- Clear economic signal

**Disadvantages**:
- Inflation risk
- No intrinsic meaning
- May trivialize rewards
- Requires currency sink balancing

**Best Practices**:
- Base on market prices + premium
- Scale with difficulty and time investment
- Use multiple currency tiers (copper, silver, gold)
- Integrate with economic systems

**Typical Formulas**:
```
Reward = (Market_Price × Quantity × Quality_Multiplier) + Time_Premium + Difficulty_Bonus
```

### 2. Material Exchange

**Pattern**: Trade materials for materials

**Example**: "Give me 100 wood, receive 50 stone and 25 iron"

**Advantages**:
- Direct resource exchange
- No currency inflation
- Useful for players with excess
- Barter economy support

**Disadvantages**:
- Value perception varies
- Storage requirements
- Less flexible
- May not match needs

**Best For**:
- Resource-rich players
- Material conversion needs
- Barter-focused economies
- Storage management

**Exchange Rate Considerations**:
- Rarity ratios
- Processing time value
- Gathering difficulty
- Market demand

### 3. Reputation/Standing

**Pattern**: Points with factions, guilds, or organizations

**Example**: "+50 reputation with Miners Guild"

**Advantages**:
- Long-term progression
- Social standing
- Unlocks special content
- Non-economic incentive

**Disadvantages**:
- Requires reputation system
- Delayed gratification
- May not be valued by all players
- Caps limit usefulness

**Reputation Benefits**:
- Access to faction vendors
- Exclusive quests
- Discounts and bonuses
- Social recognition
- Title unlocks

**Scaling**:
- Early ranks: Fast progression (encourages engagement)
- Mid ranks: Steady progression (sustained effort)
- Late ranks: Slow progression (prestige/dedication)

### 4. Experience/Skill Points

**Pattern**: Progress toward skill or character levels

**Example**: "+500 Mining XP for quest completion"

**Advantages**:
- Direct character progression
- Relevant to quest activity
- Motivates skill development
- No economic impact

**Disadvantages**:
- Value decreases at high levels
- May create grinding incentives
- Limited to relevant skills
- Can be exploited

**Integration Methods**:
- Skill-specific XP (Mining quest → Mining XP)
- General character XP
- Bonus XP for quality completion
- XP multipliers for difficulty

### 5. Knowledge/Recipe Rewards

**Pattern**: Unlock new knowledge, recipes, or discoveries

**Example**: "Complete quest to learn Advanced Smelting recipe"

**Advantages**:
- Permanent value
- Exclusive content access
- Educational benefit
- No economic inflation

**Disadvantages**:
- One-time value only
- May not match player interests
- Requires content creation
- Limited replayability

**Best For**:
- Tutorial quests
- Progression gates
- Educational content
- Exploration incentives

**BlueMarble Applications**:
- Geological knowledge unlocks
- Processing technique discoveries
- Formation identification skills
- Advanced extraction methods

### 6. Items and Equipment

**Pattern**: Receive crafted items or equipment

**Example**: "Receive a quality steel pickaxe for completing quest"

**Advantages**:
- Immediate utility
- Tangible reward
- Showcases crafter skill
- Special or unique items possible

**Disadvantages**:
- May not match player needs
- Item power creep concerns
- Storage requirements
- Economic balance challenges

**Item Reward Types**:
- Tools for gathering (relevant to quest type)
- Consumables (food, potions)
- Crafting materials
- Unique cosmetics
- Special equipment

### 7. Services and Favors

**Pattern**: Future services or discounts

**Example**: "Complete quest to receive 3 free repair services"

**Advantages**:
- Creative flexibility
- Personal relationships
- Long-term value
- Memorable rewards

**Disadvantages**:
- Difficult to enforce
- Trust required
- Value ambiguity
- Tracking overhead

**Service Types**:
- Crafting services (at discount or free)
- Transportation services
- Training and teaching
- Storage access
- Protection or assistance

### 8. Social Recognition

**Pattern**: Public acknowledgment and prestige

**Example**: "Your name on the town monument" or "Master Gatherer title"

**Advantages**:
- No economic cost
- Social motivation
- Community building
- Permanent recognition

**Disadvantages**:
- Valued differently by players
- Requires social systems
- May not motivate everyone
- Can become devalued

**Recognition Methods**:
- Titles and achievements
- Leaderboards
- Public monuments
- In-game credits
- Special badges/icons
- Hall of fame entries

### 9. Access and Privileges

**Pattern**: Special access to locations, content, or systems

**Example**: "Complete quest to access Elite Mining Area"

**Advantages**:
- Exclusive content
- Progression gate
- Status symbol
- Long-term value

**Disadvantages**:
- Requires privileged content
- May create inequality
- Difficult to balance
- Can exclude players

**Access Types**:
- Restricted areas
- Special vendors
- Advanced tools/systems
- Guild facilities
- Research data
- Teaching opportunities

## Reward Structure Patterns

### Flat Rate

**Pattern**: Fixed reward regardless of quality or speed

**Example**: "100 gold for 100 copper ore, any quality"

**Advantages**:
- Simple and predictable
- Easy to understand
- No calculation needed
- Fair baseline

**Disadvantages**:
- No incentive for excellence
- May receive low quality
- Doesn't reward efficiency
- Less engaging

**Best For**:
- Beginner quests
- Bulk materials
- Any-quality acceptable
- Simple economies

### Tiered Rewards

**Pattern**: Better rewards for better performance

**Example**:
- Basic quality: 100 gold
- Fine quality: 150 gold
- Superior quality: 200 gold

**Advantages**:
- Rewards excellence
- Player choice (effort vs reward)
- Quality stratification
- More engaging

**Disadvantages**:
- More complex
- Calculation required
- May exclude beginners
- Wider economic spread

**Best For**:
- Quality-sensitive materials
- Advanced content
- Skilled players
- Economic depth

### Bonus Multipliers

**Pattern**: Base reward with conditional multipliers

**Example**: "100 gold base, ×1.5 if completed within 24h, ×1.2 if quality 80%+"

**Advantages**:
- Flexible incentives
- Optional challenges
- Scalable rewards
- Engaging mechanics

**Disadvantages**:
- Complex calculations
- Potentially confusing
- Balance challenges
- May feel grindy

**Multiplier Types**:
- Speed bonus (time-based)
- Quality bonus (performance-based)
- Quantity bonus (overdelivery)
- Combo bonuses (multiple conditions met)

### Progressive Rewards

**Pattern**: Increasing rewards for continued participation

**Example**: "1st delivery: 100g, 2nd: 120g, 3rd: 140g, etc."

**Advantages**:
- Encourages loyalty
- Rewards repeat customers
- Long-term relationships
- Increasing engagement

**Disadvantages**:
- Inflation concerns
- Complexity in tracking
- May be exploited
- Caps needed

**Best For**:
- Continuous supply contracts
- Regular partnerships
- Guild systems
- Long-term gameplay

### Achievement-Based Rewards

**Pattern**: Special rewards for milestones

**Example**: "Complete 10 mining quests → Receive Master Miner title + special pickaxe"

**Advantages**:
- Long-term goals
- Prestige and recognition
- Collection motivation
- Doesn't inflate economy

**Disadvantages**:
- Delayed gratification
- May encourage quest spam
- Requires tracking system
- Balance across types

**Achievement Examples**:
- Quest completion count
- Total materials gathered
- Rare material discoveries
- Perfect quality completions
- Speed records
- Community contribution

## Economic Balance Mechanisms

### Market Price Integration

**Pattern**: Suggested rewards based on current market prices

**Example**: "Average market price: 50g. Suggested quest reward: 55-75g (10-50% premium)"

**Benefits**:
- Reflects real economy
- Prevents exploitation
- Fair value discovery
- Economic health

**Implementation**:
```
Suggested_Reward = Average_Market_Price × (1 + Premium_Percentage)
Premium_Percentage = 0.10 to 0.50 (10-50%)

Factors Affecting Premium:
- Urgency (higher = more premium)
- Quality requirements (higher = more premium)
- Quantity (bulk = less premium per unit)
- Location difficulty (harder = more premium)
- Requester reputation (good = can offer less)
```

### Dynamic Reward Adjustment

**Pattern**: Rewards change based on supply and demand

**Example**: "Copper ore quests: 10% bonus this week due to shortage"

**Advantages**:
- Self-regulating economy
- Responds to market conditions
- Prevents bottlenecks
- Educational about economics

**Disadvantages**:
- Can be confusing
- Potential for manipulation
- Requires robust tracking
- May feel unfair

**Adjustment Factors**:
- Current supply levels
- Quest completion rates
- Market demand signals
- Seasonal variations
- Event impacts

### Reward Caps and Minimums

**Pattern**: Limits on reward amounts to prevent exploitation

**Example**: "Minimum reward: 50g. Maximum reward: 500g."

**Purpose**:
- Prevent lowball offers
- Prevent exploitative high rewards
- Maintain economic balance
- Protect new players

**Cap Calculation**:
```
Minimum_Reward = (Market_Price × Quantity × 1.05) + Time_Cost
Maximum_Reward = (Market_Price × Quantity × 2.0) + Reasonable_Premium

Where:
- 1.05 = Minimum 5% premium over market
- 2.0 = Maximum 100% premium over market
- Time_Cost = Estimated gathering time × hourly rate
```

### Escrow and Deposit Requirements

**Pattern**: Requester must pay reward upfront or deposit it

**Example**: "Quest creator must deposit 100g in escrow before posting"

**Benefits**:
- Guarantees payment
- Prevents fake quests
- Trust building
- Protects completers

**Implementation**:
- Full reward in escrow (safest)
- Partial deposit (e.g., 20-50%)
- Reputation-based waivers for trusted players
- Auto-release on completion

### Anti-Exploitation Measures

**1. Rate Limiting**:
- Maximum quests per player per day
- Cooldown between identical quests
- Maximum active quests at once

**2. Reward Reasonability Checks**:
- Flag suspiciously high/low rewards
- Require admin approval for extremes
- Display warning messages

**3. Completion Verification**:
- Screenshot requirements
- GPS coordinates
- Third-party verification
- Timestamp validation

**4. Reputation Integration**:
- Good reputation enables higher rewards
- Bad reputation restricts quest creation
- Dispute resolution tracking
- Community ratings

## Reward Psychology and Motivation

### Intrinsic vs Extrinsic Motivation

**Intrinsic Rewards** (internal satisfaction):
- Learning and discovery
- Mastery and skill improvement
- Social connection and reputation
- Creative expression
- Meaningful contribution

**Extrinsic Rewards** (external incentives):
- Currency and items
- Experience points
- Achievements and titles
- Leaderboard rankings
- Access to content

**Best Practice**: Combine both types
- Base reward (extrinsic): Currency, XP
- Bonus reward (intrinsic): Recognition, knowledge, community benefit

### Variable Ratio Rewards

**Pattern**: Unpredictable bonus rewards

**Example**: "Complete quest for 100g, 10% chance for bonus 50g"

**Psychology**:
- Gambling mechanics (use cautiously)
- Engagement boost
- Excitement and surprise
- Can be addictive

**Ethical Considerations**:
- Don't exploit gambling psychology
- Keep bonus modest
- Transparent odds
- Optional, not core mechanic

### Loss Aversion

**Pattern**: Fear of losing potential rewards

**Example**: "Quest expires in 2 hours! Don't miss out on 200g reward!"

**Psychology**:
- FOMO (Fear of Missing Out)
- Creates urgency
- Drives action
- Can be stressful

**Ethical Use**:
- Don't create artificial scarcity
- Reasonable timeframes
- Clear communication
- Avoid pressure tactics

### Progress and Achievement

**Pattern**: Visible progress toward goals

**Example**: "5/10 mining quests completed toward Master Miner achievement"

**Psychology**:
- Goal gradient effect (acceleration near goal)
- Sense of accomplishment
- Clear progression path
- Collection motivation

**Best Practices**:
- Show progress bars
- Celebrate milestones
- Multiple achievement tiers
- Diverse achievement types

## Reward Balancing Guidelines

### Effort-to-Reward Ratio

**Formula**:
```
Reward Value = (Time_Investment × Base_Hourly_Rate) + 
               (Difficulty_Factor × Risk_Premium) + 
               (Skill_Requirement × Expertise_Bonus) +
               (Travel_Distance × Travel_Cost)

Where:
Base_Hourly_Rate = Average currency earned per hour in game
Difficulty_Factor = 1.0 (easy) to 3.0 (very hard)
Skill_Requirement = 0.0 (no skill) to 2.0 (master level)
Travel_Distance = Time to reach gathering location
```

**Example Calculation**:
- Gather 100 copper ore
- Time investment: 30 minutes
- Base hourly rate: 200g/hour
- Difficulty: 1.2 (slightly challenging)
- Skill requirement: 0.5 (beginner friendly)
- Travel: 5 minutes

```
Reward = (0.5 hours × 200g) + (1.2 × 50g) + (0.5 × 30g) + (5 min × 2g/min)
       = 100g + 60g + 15g + 10g
       = 185g suggested reward
```

### Quality-to-Reward Scaling

**Tiered Approach**:
```
Quality Tier    | Multiplier | Example (Base: 100g)
----------------|------------|---------------------
Any quality     | 1.0×       | 100g
Basic (50%+)    | 1.1×       | 110g
Fine (65%+)     | 1.3×       | 130g
Superior (80%+) | 1.6×       | 160g
Masterwork (95%+)| 2.0×      | 200g
```

**Rationale**:
- Linear scaling insufficient (effort increases exponentially)
- Exponential curve rewards mastery appropriately
- Creates quality-stratified economy
- Maintains economic balance

### Bulk Discounting

**Pattern**: Larger quantities get proportionally lower per-unit rewards

**Example**:
```
Quantity        | Per-Unit Price | Total Reward
----------------|----------------|-------------
1-50 units      | 2.0g           | 100g (50 units)
51-200 units    | 1.8g           | 270g (150 units)
201-1000 units  | 1.6g           | 1280g (800 units)
1000+ units     | 1.5g           | 1500g (1000 units)
```

**Rationale**:
- Bulk gathering more efficient (less downtime)
- Economies of scale
- Prevents inflation from large orders
- Still provides total value increase

### Reputation Impact on Rewards

**Pattern**: Established reputation enables trust-based variations

**Trusted Requester**:
- Can offer slightly lower rewards (trust premium reduction)
- Faster quest filling
- Better quality materials received
- Preferred by gatherers

**Untrusted/New Requester**:
- Must offer premium rewards (trust premium)
- Slower quest filling
- May receive minimum acceptable quality
- Must build reputation

**Formula**:
```
Trust_Adjustment = Base_Reward × (1 - (Reputation_Score / Max_Reputation) × 0.15)

Example:
- Base reward: 200g
- Reputation: 800/1000
- Trust adjustment: 200g × (1 - 0.8 × 0.15) = 200g × 0.88 = 176g

High reputation saves 24g per quest while still attracting completers
```

## Multi-Currency and Complex Reward Systems

### Multiple Currency Types

**Example Currencies**:
- **Gold**: General currency for purchasing
- **Reputation Points**: Faction standing
- **Knowledge Points**: Educational progression
- **Guild Merits**: Internal guild currency
- **Premium Currency**: Optional purchasable (if F2P model)

**Quest Reward Example**:
"Complete quest for 100 Gold + 25 Reputation + 50 Knowledge Points"

**Benefits**:
- Addresses multiple player goals
- Provides choice (if optional)
- Reduces single-currency inflation
- Enables diverse progression paths

**Challenges**:
- Complexity for players
- Balance across currencies
- Value perception varies
- Exchange rate management

### Reward Packages

**Pattern**: Bundled rewards of multiple types

**Example**: "Complete quest for Care Package: 100g + Quality Pickaxe + Mining Buff (1 hour)"

**Benefits**:
- Perceived higher value (bundle effect)
- Multiple motivations addressed
- Creative flexibility
- Memorable rewards

**Package Types**:
- Starter packages (tools + resources)
- Professional packages (specialized for skill)
- Prestige packages (cosmetics + recognition)
- Practical packages (consumables + currency)

### Choice-Based Rewards

**Pattern**: Player chooses from reward options

**Example**: "Choose your reward: A) 200 gold, B) Quality pickaxe, C) 100 reputation"

**Benefits**:
- Player agency
- Matches individual needs
- Higher satisfaction
- Replayability

**Implementation**:
- 2-4 reward options
- Roughly equivalent value
- Different reward types (currency vs items vs reputation)
- Clear descriptions

## Reward Distribution Methods

### Immediate Distribution

**Pattern**: Reward given instantly on completion

**Advantages**:
- Instant gratification
- Simple implementation
- Clear cause-effect
- High satisfaction

### Delayed Distribution

**Pattern**: Reward after verification period

**Example**: "Reward distributed within 24h of completion"

**Advantages**:
- Allows verification
- Fraud prevention
- Quality checks
- Dispute window

### Milestone Distribution

**Pattern**: Rewards distributed at intervals

**Example**: "50g at 50% completion, 100g at 100% completion, 50g bonus on verification"

**Advantages**:
- Motivates continued effort
- Reduces completion abandonment
- Risk distribution
- Progress reinforcement

### Batch Distribution

**Pattern**: Multiple completions paid together

**Example**: "Complete 10 deliveries, receive 1000g total at end of week"

**Advantages**:
- Administrative efficiency
- Bulk discount application
- Relationship building
- Long-term engagement

## BlueMarble-Specific Reward Integration

### Geological Knowledge Rewards

**Pattern**: Unlock geological discoveries and understanding

**Examples**:
- "Learn: Copper ore formation in sedimentary rock"
- "Discover: Volcanic deposits produce higher quality materials"
- "Unlock: Advanced extraction technique for deep deposits"

**Benefits**:
- Educational mission alignment
- Permanent character progression
- Enables advanced gameplay
- No economic inflation

**Implementation**:
- Knowledge tree system
- Progressive unlocks
- Quiz/verification optional
- Applied in gameplay (better gathering)

### Collaborative Terraforming Rewards

**Pattern**: Contribution to large-scale geological projects

**Example**: "Help build mountain pass road - contribution tracked, rewards based on participation"

**Rewards**:
- Proportional currency (based on contribution)
- Name on project monument
- Access to completed infrastructure
- Reputation with community

**Benefits**:
- Community building
- Meaningful large-scale projects
- Visible world impact
- Educational about engineering

### Research Contribution Rewards

**Pattern**: Geological data collection rewarded

**Example**: "Survey 10 geological formations, receive geological database access"

**Rewards**:
- Scientific data access
- Research credits
- Co-authorship on discoveries
- Special researcher title

**Benefits**:
- Citizen science gameplay
- Educational value
- Real-world relevance
- Unique rewards

### Material Processing Chain Rewards

**Pattern**: Rewards for multi-stage processing

**Example**: "Quest chain: 1) Gather ore (100g), 2) Smelt ingots (50g), 3) Alloy steel (75g) - Total: 225g + bonus recipe"

**Benefits**:
- Teaches geological processes
- Encourages specialization
- Multi-player cooperation
- Educational progression

## Summary

Effective reward systems balance multiple factors:

1. **Fairness**: Appropriate compensation for effort and skill
2. **Motivation**: Appeals to both intrinsic and extrinsic needs
3. **Sustainability**: Maintains economic health long-term
4. **Flexibility**: Accommodates diverse player preferences
5. **Engagement**: Creates meaningful and memorable experiences

Key Principles:
- Base rewards on market prices plus reasonable premium
- Scale rewards with quality, difficulty, and time investment
- Combine multiple reward types for broader appeal
- Integrate reputation and trust systems
- Prevent exploitation through caps and verification
- Balance economic and non-economic incentives

For BlueMarble:
- Emphasize knowledge and discovery rewards (educational)
- Integrate geological progression with quest completion
- Support collaborative large-scale projects
- Reward scientific methodology and data collection
- Maintain economic balance while encouraging specialization
- Create meaningful rewards that advance both player and scientific goals

## Related Steps

- Previous: [Step 2: Resource Request Design](../step-2-resource-request-design/) - Material specifications
- Next: [Step 4: BlueMarble Implementation](../step-4-bluemarble-implementation/) - Integration recommendations
- Related: [Skill Systems](../../step-2-system-research/step-2.1-skill-systems/) - Skill progression integration
- Related: [Economic Systems](../../step-1-foundation/) - Game economy fundamentals
