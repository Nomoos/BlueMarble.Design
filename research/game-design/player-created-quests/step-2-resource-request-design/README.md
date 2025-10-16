# Step 2: Resource Request Design

## Overview

This step examines how players structure resource gathering quests effectively. We analyze the specific parameters, conditions, and design choices that make resource requests clear, achievable, and economically balanced.

## Material Specification Methods

### 1. Basic Item Type

**Pattern**: Specify only the item type

**Example**: "Need 100 wood"

**Advantages**:
- Simple and clear
- Easy for gatherers to understand
- Low barrier to entry
- Fast quest creation

**Disadvantages**:
- No quality control
- May receive low-quality materials
- Not suitable for crafting requirements
- Economic inefficiency

**Best For**:
- Bulk materials
- Low-value items
- Construction (where any quality works)
- Beginner-friendly quests

### 2. Quality Threshold

**Pattern**: Minimum quality percentage or tier

**Example**: "Need 50 iron ore (Quality 70%+ or higher)"

**Variations**:
- **Percentage-based**: "70%+ quality"
- **Tier-based**: "Fine tier or better"
- **Grade-based**: "B-grade minimum"
- **Star rating**: "3-star or higher"

**Advantages**:
- Ensures usable materials for crafting
- Rewards skilled gatherers
- Creates quality-based economy
- Reflects realistic material needs

**Disadvantages**:
- Excludes beginners
- Higher cost
- Reduces available supply
- Requires quality system implementation

**Best For**:
- Crafting materials
- Advanced production
- Specialized equipment
- Quality-sensitive recipes

### 3. Property-Based Requirements

**Pattern**: Specify material properties rather than just quality

**Example**: "Need iron ore with Durability 80+, Weight <120%"

**Properties Can Include**:
- Durability/strength
- Weight/density
- Flexibility/hardness
- Purity/composition
- Temperature resistance
- Elemental affinities

**Advantages**:
- Precise material matching for recipes
- Encourages geological/material knowledge
- Supports complex crafting systems
- Realistic material science

**Disadvantages**:
- Complex for beginners
- Requires robust material property system
- May be overly specific
- Limits supply availability

**Best For**:
- Advanced crafting
- Specialized items (armor, tools, etc.)
- Geological simulation games (like BlueMarble)
- Educational content about material properties

### 4. Source-Based Requirements

**Pattern**: Specify the geographic origin or extraction method

**Example**: "Need 100 copper ore from Northern Mountains (rich copper deposits)"

**Variations**:
- Geographic region: "From Eastern Hills"
- Geological formation: "From granite deposits"
- Depth requirement: "From below 500m depth"
- Environmental condition: "From underwater deposits"

**Advantages**:
- Drives exploration
- Supports geological accuracy
- Creates geographic specialization
- Educational value (geological knowledge)

**Disadvantages**:
- Highly restrictive
- May require extensive travel
- Limits completer pool
- Difficult to verify automatically

**Best For**:
- Rare materials
- Geological simulation games
- Exploration content
- Region-specific resources

### 5. Mixed Requirements

**Pattern**: Combine multiple specification methods

**Example**: "Need 50 iron ore: Quality 75%+, from volcanic regions, Durability 85+"

**Advantages**:
- Maximum control over material characteristics
- Realistic for specialized crafting
- Rewards expert gatherers
- Educational depth

**Disadvantages**:
- Very restrictive
- High complexity
- Small completer pool
- May be impossible to fulfill

**Best For**:
- Legendary/epic items
- Master crafters
- End-game content
- Research quests

## Quantity Specification

### Fixed Quantity

**Pattern**: Exact amount required

**Example**: "Need exactly 100 copper ore"

**Advantages**:
- Clear and unambiguous
- Predictable rewards
- Simple verification
- Economic clarity

**Disadvantages**:
- No flexibility
- May be difficult to fulfill exactly
- Wastes excess materials
- No partial completion

**Best For**:
- Recipe requirements
- Exact crafting needs
- Small quantities
- Simple quests

### Minimum Quantity

**Pattern**: At least X amount needed

**Example**: "Need at least 100 copper ore (more accepted)"

**Advantages**:
- Flexible for gatherers
- Enables bulk gathering
- Reduces quest spam
- Efficient resource flow

**Disadvantages**:
- Unpredictable total cost
- Requester may receive too much
- Reward calculation complexity
- Storage concerns

**Best For**:
- Stockpiling
- Ongoing production needs
- Guild requisitions
- Long-term supply

### Range Quantity

**Pattern**: Between X and Y amount

**Example**: "Need 80-120 copper ore"

**Advantages**:
- Flexibility for both parties
- Accommodates gathering variance
- Reasonable completion targets
- Reduced wasted effort

**Disadvantages**:
- Proportional reward calculation needed
- Less precise than fixed quantity
- May complicate verification

**Best For**:
- Variable yield materials
- Flexible crafting batches
- Organic materials with inconsistent gathering
- Reasonable completion windows

### Batch Specification

**Pattern**: Multiples of a batch size

**Example**: "Need copper ore in batches of 50 (up to 200 total)"

**Advantages**:
- Enables partial completion
- Multiple gatherers can contribute
- Flexible gathering schedules
- Reduces all-or-nothing pressure

**Disadvantages**:
- More complex management
- Multiple transactions
- Batch size must be meaningful
- Tracking overhead

**Best For**:
- Large quantity requests
- Guild projects
- Long-term supply contracts
- Community goals

## Delivery Conditions

### Immediate Delivery

**Pattern**: Direct trade upon completion

**Example**: "Meet at Town Square, trade immediately upon gathering"

**Advantages**:
- Instant gratification
- Simple verification
- No escrow needed
- Personal interaction

**Disadvantages**:
- Requires online coordination
- Time zone challenges
- Scheduling overhead
- No offline completion

**Best For**:
- Urgent requests
- Small quantities
- Local players
- High-trust relationships

### Deposit to Storage

**Pattern**: Deliver to player's storage location

**Example**: "Deliver to my chest at coordinates (X, Y)"

**Advantages**:
- Offline completion possible
- No real-time coordination
- Automatic verification
- Time flexibility

**Disadvantages**:
- Requires secure storage
- Access permission management
- Potential for theft
- Location-dependent

**Best For**:
- Large projects
- Guild storage
- Async gameplay
- Long-term contracts

### Mailbox/Delivery System

**Pattern**: Send via in-game mail or courier

**Example**: "Mail materials to PlayerName"

**Advantages**:
- No location requirements
- Offline friendly
- Secure transfer
- Automatic logging

**Disadvantages**:
- May have mail limits (weight, quantity)
- Requires mail system infrastructure
- Potential for mail fees
- Delivery delays

**Best For**:
- Cross-region trades
- Small-medium quantities
- Established systems
- Remote players

### Market/Auction Integration

**Pattern**: Place items in market, quest auto-purchases

**Example**: "List iron ore on market tagged #Quest12345"

**Advantages**:
- Leverages existing market systems
- Price discovery
- Multiple sellers possible
- Economic integration

**Disadvantages**:
- Market fees apply
- May be outbid by others
- Requires market infrastructure
- Less personal

**Best For**:
- Common materials
- Competitive pricing
- Large markets
- Economic gameplay

### Escrow System

**Pattern**: Third-party holds materials and payment until verified

**Example**: "Materials held in escrow, auto-released on quest completion"

**Advantages**:
- High security
- Prevents fraud
- Automatic enforcement
- No trust required

**Disadvantages**:
- Requires escrow infrastructure
- May have fees
- Complexity
- Potential bugs/exploits

**Best For**:
- High-value trades
- Untrusted parties
- Contract systems
- Formal agreements

## Timeframe Specifications

### Open-Ended

**Pattern**: No time limit

**Example**: "Accepting copper ore indefinitely"

**Advantages**:
- No pressure
- Flexible gathering
- Long-term relationships
- Continuous supply

**Disadvantages**:
- May never be fulfilled
- Clutters quest boards
- Lacks urgency
- Difficult to plan around

**Best For**:
- Ongoing needs
- Guild stockpiling
- Non-critical materials
- Stable supply relationships

### Fixed Deadline

**Pattern**: Complete by specific date/time

**Example**: "Need 100 copper ore by Friday 6pm"

**Advantages**:
- Clear expectations
- Creates urgency
- Enables planning
- Auto-cancels if unfulfilled

**Disadvantages**:
- Time zone issues
- Inflexible
- Pressure on gatherers
- May go unfulfilled

**Best For**:
- Time-sensitive needs
- Event preparation
- Scheduled production
- Coordinated activities

### Duration-Based

**Pattern**: Complete within X time from acceptance

**Example**: "Complete within 48 hours of accepting"

**Advantages**:
- Fair to all time zones
- Flexible start time
- Personal deadline
- Prevents quest hoarding

**Disadvantages**:
- Tracking complexity
- Multiple overlapping deadlines
- May pressure slow gatherers
- Real-life interference

**Best For**:
- Individual completers
- Moderate urgency
- Flexible scheduling
- Active player focus

### Rush Bonuses

**Pattern**: Extra reward for fast completion

**Example**: "100 gold normally, +50 gold if completed within 2 hours"

**Advantages**:
- Optional urgency
- Rewards fast gatherers
- Emergency flexibility
- Market-driven speed

**Disadvantages**:
- May encourage careless gathering
- Benefits online/active players only
- Complexity in reward calculation
- Potential for exploitation

**Best For**:
- Emergency needs
- Premium rewards
- Competitive gameplay
- Critical path materials

## Partial Completion Options

### All-or-Nothing

**Pattern**: Must deliver full quantity or get nothing

**Example**: "Need exactly 100 copper ore - no partial completion"

**Advantages**:
- Clear requirements
- Predictable for requester
- Simple verification
- Binary success state

**Disadvantages**:
- High risk for gatherers
- May go unfulfilled
- Wasteful of partial efforts
- Discourages attempts

**Best For**:
- Small quantities
- Critical exact needs
- Recipe requirements
- High-reward quests

### Proportional Payment

**Pattern**: Payment scales with amount delivered

**Example**: "Need 100 copper ore - 1 gold per ore delivered"

**Advantages**:
- Rewards partial effort
- More likely to be fulfilled
- Flexible for gatherers
- Reduces wasted work

**Disadvantages**:
- Requester may not get full amount
- Multiple deliveries complexity
- May take longer to fulfill
- Tracking overhead

**Best For**:
- Large quantities
- Non-critical materials
- Community goals
- Flexible needs

### Milestone Payments

**Pattern**: Bonus payments at specific thresholds

**Example**: "50 gold at 50 ore, 100 gold at 100 ore, 50 gold bonus if completed"

**Advantages**:
- Incentivizes completion
- Rewards progress
- Gamified experience
- Clear milestone goals

**Disadvantages**:
- Complex reward structure
- Requires careful balancing
- Tracking overhead
- May encourage gaming the system

**Best For**:
- Large projects
- Long-term contracts
- Guild goals
- Engaging progression

### First-Come-First-Served vs. Best-Offer

**First-Come-First-Served**:
- First player to deliver gets the quest
- Rewards speed and availability
- Simple and fair
- Creates competition

**Best-Offer**:
- Multiple players can bid/submit
- Requester chooses best quality/price
- Price discovery
- May take longer

## Quality Control and Acceptance Criteria

### Automatic Acceptance

**Pattern**: System validates materials automatically

**Example**: "All copper ore 70%+ automatically accepted on delivery"

**Advantages**:
- Instant fulfillment
- No verification needed
- Offline friendly
- Reduces disputes

**Disadvantages**:
- Requires robust item system
- May miss edge cases
- Less personal
- Limited flexibility

**Best For**:
- Common materials
- Clear specifications
- High-volume quests
- Scalable systems

### Manual Inspection

**Pattern**: Requester reviews and accepts/rejects

**Example**: "Will inspect materials within 24h and accept/reject"

**Advantages**:
- Full control for requester
- Catches edge cases
- Personal interaction
- Subjective quality assessment

**Disadvantages**:
- Requires online time
- Potential for disputes
- Slow fulfillment
- Trust issues

**Best For**:
- High-value materials
- Subjective quality
- Special requests
- Personal relationships

### Third-Party Verification

**Pattern**: Independent party validates completion

**Example**: "Guild master will verify material quality"

**Advantages**:
- Neutral judgment
- Reduces disputes
- Professional assessment
- Builds trust

**Disadvantages**:
- Requires verifier availability
- May have fees
- Slower process
- Limited verifier pool

**Best For**:
- High-value transactions
- Untrusted parties
- Guild coordination
- Formal contracts

### Reputation-Based Trust

**Pattern**: Established relationships bypass verification

**Example**: "Trusted gatherers auto-accepted, others require inspection"

**Advantages**:
- Rewards good behavior
- Faster for regulars
- Builds community
- Efficient for repeat business

**Disadvantages**:
- Barriers for newcomers
- Potential for abuse
- Reputation system required
- May create cliques

**Best For**:
- Established economies
- Repeat relationships
- Guild systems
- Long-term gameplay

## Quest Description Best Practices

### Clear Material Identification

**Good**: "Need 100 copper ore (the reddish-brown rocks from mining)"
**Bad**: "Need copper" (could be ore, ingots, or items)

### Specific Quality Requirements

**Good**: "Need iron ore at 75%+ quality for steel production"
**Bad**: "Need good iron ore"

### Location Information

**Good**: "Deliver to my workshop at Town Square (coordinates: 1234, 5678)"
**Bad**: "Bring to me somewhere"

### Reward Clarity

**Good**: "Reward: 100 gold + 10 reputation with Miners Guild"
**Bad**: "Good payment"

### Purpose Context (Optional)

**Good**: "Gathering materials for town wall construction - help our community!"
**Bad**: "Just need stuff"

**Benefits of Context**:
- Creates emotional investment
- Builds community
- Educational value
- Narrative engagement

### Timeframe Communication

**Good**: "Need by Friday 6pm EST (48 hours from now)"
**Bad**: "Need soon"

### Contact Information

**Good**: "Message me in-game or mail @PlayerName if questions"
**Bad**: No contact info

## Common Pitfalls and Solutions

### Pitfall: Over-Specific Requirements

**Problem**: "Need copper ore from Northern Mountains, 90%+ quality, exactly 73 units, delivered by Tuesday"

**Solution**: Balance specificity with feasibility. Consider:
- Is each requirement necessary?
- Is fulfillment realistic?
- Are there enough qualified gatherers?

### Pitfall: Unclear Rewards

**Problem**: "Will pay well" or "Negotiable"

**Solution**: State specific rewards upfront:
- Fixed currency amount
- Material exchange rates
- Reputation points
- Special bonuses

### Pitfall: Geographic Impossibility

**Problem**: "Need materials from three different continents delivered in 1 hour"

**Solution**: Consider:
- Travel time to gathering locations
- Reasonable completion windows
- Multiple gatherers for large areas
- Break into separate quests

### Pitfall: Economic Imbalance

**Problem**: "1000 gold for 1 copper ore" or "1 copper for 1000 ore"

**Solution**: Use market integration:
- Display average market price
- Suggest reward range
- Show price history
- Warn about extreme values

### Pitfall: Quality Verification Disputes

**Problem**: "That ore isn't really 75% quality!"

**Solution**: Implement clear systems:
- Objective quality metrics
- Screenshots/timestamps
- Third-party verification
- Escrow with quality check

## Examples of Well-Designed Resource Quests

### Example 1: Beginner-Friendly Quest

**Title**: "Need Wood for Construction"

**Description**: 
"Hi! I'm building a new workshop and need basic building materials. Any quality wood is fine - this is practice work, so don't worry about being perfect!"

**Requirements**:
- 100 wood (any quality)
- Deliver to Town Square chest (coords: 1234, 5678)
- Complete within 7 days

**Reward**:
- 50 gold
- 5 reputation with Builders Guild
- Bonus: +10 gold if delivered within 24 hours

**Why It Works**:
- Simple requirements
- No quality pressure
- Clear location
- Generous timeframe
- Friendly tone
- Optional rush bonus

### Example 2: Expert-Level Quest

**Title**: "Premium Iron Ore for Master Blacksmithing"

**Description**:
"Seeking high-quality iron ore for crafting legendary weapons. Must be from volcanic regions for best tempering properties. This is for an important guild commission, so quality matters!"

**Requirements**:
- 50 iron ore
- Quality: 85%+ (Superior tier)
- Source: Volcanic deposits (Southern Volcano region)
- Durability property: 90+
- Deliver via in-game mail

**Reward**:
- 500 gold
- 50 reputation with Blacksmiths Guild
- 1 Legendary Crafting Recipe (your choice from my collection)
- Your name credited on final item

**Why It Works**:
- Clear quality expectations
- Specific but achievable requirements
- High but fair reward
- Special bonus (recipe) for expert work
- Recognition (name credit)
- Explains why requirements matter

### Example 3: Community Project

**Title**: "Town Wall Construction - Stone Gathering"

**Description**:
"Our town is building defensive walls! We need massive amounts of stone. Every contribution helps - even small amounts make a difference. All contributors will be recognized on the town monument!"

**Requirements**:
- Stone blocks (any quality)
- Minimum delivery: 10 blocks
- Target: 10,000 blocks total (community goal)
- Deliver to Town Storage

**Reward** (per 10 blocks):
- 20 gold
- 2 reputation with Town Council
- Your name on contributors monument
- Milestone bonus: 100 gold at 100 blocks contributed

**Why It Works**:
- Community-focused
- Low barrier to entry (10 minimum)
- Scales to any contribution level
- Clear community goal
- Public recognition
- Milestone incentives

### Example 4: Specialized Research Quest

**Title**: "Geological Survey - Rare Mineral Samples"

**Description**:
"I'm researching geological formations and need samples from specific locations. This is scientific work, so precise sourcing is critical. Samples must be pure and properly documented."

**Requirements**:
- 10 samples each from:
  - Deep granite formations (500m+ depth)
  - Coastal limestone deposits
  - Volcanic basalt regions
- Quality: 60%+ (to ensure purity)
- Include location coordinates in delivery notes
- Screenshot of extraction location

**Reward**:
- 800 gold (80 gold per correct sample)
- 100 reputation with Geological Society
- Access to my geological maps (valuable knowledge)
- Co-authorship on research paper (prestige)

**Why It Works**:
- Educational value
- Clear documentation requirements
- Scientific authenticity
- Knowledge as reward
- Prestige recognition
- Fair compensation for effort

## Integration with BlueMarble's Geological System

### Geological Property Specifications

**Example**: "Need iron ore with Fe content 65%+, low sulfur content (<5%), from sedimentary deposits"

- Leverages BlueMarble's geological accuracy
- Educational about material properties
- Rewards geological knowledge
- Creates realistic crafting requirements

### Multi-Stage Processing Quests

**Example**: "Quest chain: 1) Gather raw ore, 2) Process to ingots, 3) Alloy with carbon"

- Teaches geological/metallurgical processes
- Creates interdependence between specialists
- Educational progression
- Realistic production chains

### Exploration-Based Discovery Quests

**Example**: "Find and document a new copper deposit, bring samples (Quality irrelevant - discovery reward)"

- Encourages exploration
- Educational about geological prospecting
- Rewards curiosity
- Scientific methodology

### Terraforming Coordination Quests

**Example**: "Guild project: Need 10,000 stone to build mountain pass road - geological engineering project"

- Large-scale collaboration
- Practical geological manipulation
- Long-term community goals
- Engineering education

## Summary

Effective resource request design requires balancing:

1. **Clarity**: Precise specifications without overwhelming complexity
2. **Feasibility**: Realistic requirements achievable by target audience
3. **Flexibility**: Options for partial completion and delivery methods
4. **Fairness**: Appropriate rewards for effort and skill required
5. **Context**: Helpful information that aids completion and builds community

Key principles:
- Start simple, add complexity as needed
- Consider gatherer perspective (effort, skill, time)
- Provide context and purpose for engagement
- Use clear, unambiguous language
- Balance specificity with accessibility
- Integrate with economic and social systems

For BlueMarble specifically:
- Leverage geological accuracy for educational quests
- Use material properties meaningfully
- Create progression from simple to complex requests
- Reward geological knowledge and expertise
- Support collaborative large-scale projects
- Maintain scientific authenticity in requirements

## Related Steps

- Previous: [Step 1: Quest System Patterns](../step-1-quest-system-patterns/) - Quest creation mechanisms
- Next: [Step 3: Incentive Structures](../step-3-incentive-structures/) - Reward systems and economic balance
- Related: [Material Systems](../../step-2-system-research/step-2.2-material-systems/) - Material quality and properties
