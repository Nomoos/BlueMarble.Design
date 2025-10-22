# Step 1: Quest System Patterns

## Overview

This step analyzes the fundamental mechanisms by which players create, publish, and manage quests for other players across various MMORPGs. We examine the core patterns that enable resource-gathering players to outsource grind tasks effectively.

## Core Quest Creation Patterns

### 1. Bounty Board System

**Definition**: Centralized location where players post and browse available quests.

**Examples**:
- **Wurm Online**: Village bounty boards for community tasks
- **EVE Online**: Corporation bulletin boards for member tasks
- **Life is Feudal**: Guild task boards for coordinated production

**Key Features**:
- Physical or virtual location for quest visibility
- Filtering by quest type, reward, location
- Quest expiration and automatic removal
- Priority flagging for urgent requests

**Advantages**:
- Simple, intuitive interface
- Central gathering point encourages social interaction
- Easy to browse and compare available quests
- Clear organizational structure

**Disadvantages**:
- Limited discoverability for remote players
- Can become cluttered with inactive quests
- Requires physical travel to access (in some implementations)

### 2. Work Order System

**Definition**: Automated quest generation from crafter needs, often integrated with crafting interfaces.

**Examples**:
- **World of Warcraft**: Crafting order system for custom items
- **Final Fantasy XIV**: Collectables and custom delivery missions
- **Albion Online**: Automated island worker tasks

**Key Features**:
- Integrated with crafting UI
- Automatic material specification from recipes
- Batch processing for multiple orders
- Quality tier specification
- Time-limited completion windows

**Advantages**:
- Seamless integration with crafting workflow
- Precise material specifications
- Reduced manual quest creation overhead
- Automatic validation of completion criteria

**Disadvantages**:
- Less flexible than manual quest creation
- May not support complex multi-stage requests
- Limited narrative or context options

### 3. Contract System

**Definition**: Formal agreements between players with defined terms, rewards, and failure conditions.

**Examples**:
- **EVE Online**: Comprehensive contract system (courier, item exchange, auction)
- **Star Wars Galaxies**: Crafter work orders and bounty contracts
- **Albion Online**: Transport contracts and trade orders

**Key Features**:
- Legal framework with binding terms
- Collateral and security deposits
- Automatic enforcement of completion
- Contract types: delivery, courier, exchange, auction
- Multi-party contracts (requester, completer, guarantor)

**Advantages**:
- High trust through automatic enforcement
- Supports complex multi-party transactions
- Clear success/failure criteria
- Economic security through collateral

**Disadvantages**:
- Complex interface can be overwhelming
- Requires robust anti-exploitation measures
- May be too formal for simple resource requests

### 4. Request Board / Notice Board

**Definition**: Community-driven quest posting with reputation-based trust.

**Examples**:
- **Wurm Online**: Player-created missions
- **Mortal Online 2**: Guild notice boards
- **Eco Global Survival**: Government work projects

**Key Features**:
- Player-written quest descriptions
- Flexible reward structures
- Reputation tracking for quest creators
- Community moderation and rating
- Quest categories and tags

**Advantages**:
- Highly flexible and creative
- Supports narrative and context
- Community-driven quality control
- Simple to implement

**Disadvantages**:
- Relies on player honesty
- Manual verification of completion
- Potential for disputes
- Limited automation

### 5. Guild Requisition System

**Definition**: Internal guild system for coordinating resource gathering among members.

**Examples**:
- **Life is Feudal**: Guild work orders for construction projects
- **Eco Global Survival**: Collaborative research and building projects
- **EVE Online**: Corporation fleet logistics and preparation

**Key Features**:
- Limited to guild/organization members
- Contribution tracking for individual members
- Collective rewards and benefits
- Long-term project coordination
- Resource pooling and distribution

**Advantages**:
- Strong social bonds and trust
- Supports large-scale collaborative projects
- Clear organizational hierarchy
- Built-in reputation from guild membership

**Disadvantages**:
- Limited to guild members
- Requires guild management overhead
- Less economically flexible
- May exclude solo players

## Quest Discovery Mechanisms

### Geographic-Based Discovery

**Bounty Boards at Towns/Cities**:
- Players visit specific locations to view quests
- Creates natural gathering points
- Encourages exploration
- Example: Wurm Online village boards

**Local Quest Scope**:
- Quests visible only in specific regions
- Encourages local economies
- Reduces spam and clutter
- Example: Life is Feudal regional requests

### Reputation-Based Discovery

**Faction Standing Requirements**:
- High-reputation quests offer better rewards
- Encourages building relationships
- Creates progression path
- Example: EVE Online loyalty point stores

**Skill-Gated Visibility**:
- Advanced quests require minimum skill levels
- Prevents inappropriate quest matching
- Ensures quest completer capability
- Example: FFXIV collectables by crafter level

### Social Network Discovery

**Guild Channels**:
- Quests shared through guild communications
- Leverages existing social bonds
- Private or semi-private quest pools
- Example: Most MMO guild systems

**Friend Lists and Contacts**:
- Preferred crafter/gatherer relationships
- Repeat customer systems
- Personal reputation building
- Example: Star Wars Galaxies vendor favorites

### Matchmaking Systems

**Automatic Quest Recommendations**:
- System suggests quests based on player skills
- Geographic proximity to quest location
- Historical completion patterns
- Example: FFXIV duty finder for crafting leves

**Push Notifications**:
- Alerts when relevant quests become available
- Customizable by material type, reward, location
- Reduces time spent browsing
- Example: Mobile companion apps

## Quest Types for Resource Gathering

### 1. Simple Material Request

**Pattern**: "Bring me X units of Y material"

**Example**: "Need 100 copper ore - 50 silver reward"

**Characteristics**:
- Single material type
- Fixed quantity
- Simple completion verification
- Straightforward reward

**Best For**:
- Common materials
- Large quantity requests
- Low-value items
- New players

### 2. Quality-Specified Request

**Pattern**: "Bring me X units of Y material at Z quality or higher"

**Example**: "Need 50 premium iron ingots (70%+ quality) - 200 gold reward"

**Characteristics**:
- Quality threshold specification
- Higher reward for better quality
- Requires skill to fulfill
- Economic stratification

**Best For**:
- Crafting-critical materials
- Advanced players
- Specialized production chains
- Quality-sensitive recipes

### 3. Multi-Material Bundle

**Pattern**: "Bring me X of A, Y of B, Z of C"

**Example**: "Need 100 wood, 50 stone, 25 iron for construction - 150 gold reward"

**Characteristics**:
- Multiple material types
- Related materials (often for recipes)
- Partial completion options
- Bulk gathering efficiency

**Best For**:
- Construction projects
- Recipe requirements
- Reducing quest spam
- Large-scale operations

### 4. Processed Material Request

**Pattern**: "Bring me X processed materials (requires intermediate crafting)"

**Example**: "Need 20 steel bars (from iron + coal) - 300 gold reward"

**Characteristics**:
- Requires intermediate processing
- Higher reward reflects added value
- Multiple skill requirements
- Processing chain coordination

**Best For**:
- Advanced crafting
- Specialized processors
- Time-saving for requesters
- Economic depth

### 5. Expedition / Survey Quest

**Pattern**: "Gather X from specific location Y"

**Example**: "Need 30 rare herbs from Northern Mountains - 400 gold reward"

**Characteristics**:
- Geographic specificity
- Often rare or difficult to access materials
- May involve danger or challenge
- Exploration incentive

**Best For**:
- Rare materials
- Geographic diversity
- Exploration content
- Adventure gameplay

### 6. Timed Rush Order

**Pattern**: "Bring me X within Y timeframe for bonus reward"

**Example**: "URGENT: Need 200 copper ore within 2 hours - 150 gold + 50 gold time bonus"

**Characteristics**:
- Time pressure
- Higher reward for speed
- Often for critical production needs
- Creates urgency

**Best For**:
- Emergency situations
- Competitive crafting
- Event preparation
- Premium rewards

### 7. Continuous Supply Contract

**Pattern**: "Deliver X amount of Y daily/weekly for ongoing reward"

**Example**: "Supply 100 coal daily - 50 gold per delivery, 500 gold weekly completion bonus"

**Characteristics**:
- Recurring deliveries
- Long-term relationship
- Stable income for gatherers
- Predictable supply for crafters

**Best For**:
- Industrial operations
- Stable supply chains
- Dedicated gatherers
- Efficient economies

### 8. Competitive Bidding

**Pattern**: "Best offer wins - seeking lowest price for X units of Y"

**Example**: "Seeking 1000 iron ore - accepting bids, lowest price wins"

**Characteristics**:
- Multiple players can bid
- Price discovery mechanism
- Market-driven pricing
- Time-limited bidding period

**Best For**:
- Common materials
- Large quantities
- Price-sensitive requesters
- Competitive markets

## Quest Creation Interface Patterns

### Wizard/Stepper Interface

**Flow**:
1. Select quest type (simple/quality/bundle/etc.)
2. Specify materials (type, quantity, quality)
3. Set reward (currency, materials, reputation)
4. Define conditions (timeframe, location, notes)
5. Review and publish

**Advantages**:
- Guides new users
- Reduces errors
- Clear step-by-step process
- Validation at each step

**Disadvantages**:
- More clicks required
- Less flexible
- Slower for experienced users

### Form-Based Interface

**Layout**:
- Single screen with all fields
- Material selection dropdowns
- Quantity sliders/inputs
- Reward calculator
- Preview pane

**Advantages**:
- Fast for experienced users
- All options visible
- Easy to adjust and fine-tune
- Supports templates

**Disadvantages**:
- Can be overwhelming
- Easy to make mistakes
- Requires more screen space

### Template System

**Approach**:
- Pre-defined quest templates
- Fill in quantities and rewards
- Common patterns readily available
- Custom templates saveable

**Examples**:
- "Gather ore" template
- "Process materials" template
- "Construction supply" template

**Advantages**:
- Very fast quest creation
- Standardized patterns
- Lower barrier to entry
- Reduces errors

**Disadvantages**:
- Less flexible
- May not fit all needs
- Requires template maintenance

### Voice/Text Command

**Pattern**: Natural language quest creation

**Example**: "/quest create: Need 100 copper ore, reward 50 gold, expires 24h"

**Advantages**:
- Very fast
- No UI required
- Scriptable/macroing
- Accessible

**Disadvantages**:
- Parsing complexity
- Error-prone
- Limited expressiveness
- Discoverability issues

## Quest Management Features

### Quest Status Tracking

**Requester View**:
- Quest publish date and expiration
- Number of players working on quest
- Partial completion status
- Expected completion time

**Completer View**:
- Quest acceptance tracking
- Progress towards completion
- Time remaining
- Estimated reward

### Quest Modification

**Allowed Changes**:
- Reward increase (to attract more completers)
- Expiration extension
- Quantity reduction (with proportional reward)
- Priority flagging

**Prohibited Changes**:
- Material type change (would invalidate work in progress)
- Quality reduction after acceptance
- Reward decrease after acceptance

### Quest Cancellation

**Requester Cancellation**:
- Penalty for canceling accepted quests
- Full cancellation before acceptance (small fee)
- Partial payment for work completed

**Completer Abandonment**:
- Reputation impact
- Quest returned to available pool
- Grace period for temporary disconnects

## Anti-Exploitation Measures

### Quest Creation Limits

**Spam Prevention**:
- Maximum number of active quests per player
- Cooldown between quest postings
- Minimum reward thresholds
- Material requirement validation

### Reward Validation

**Economic Safeguards**:
- Minimum reward-to-effort ratios
- Market price integration
- Warning for suspicious rewards
- Admin flagging for review

### Completion Verification

**Anti-Fraud**:
- Automatic item transfer on completion
- Escrow system for rewards
- Screenshot/timestamp verification
- Community reporting

### Reputation Systems

**Trust Metrics**:
- Quest creator reliability score
- Completion rate tracking
- Dispute history
- Community ratings

## Integration with Game Systems

### Skill Progression Integration

**Skill-Based Quest Gating**:
- Minimum skill requirements for quest visibility
- Skill experience rewards for completion
- Skill-specific quest chains
- Progressive difficulty scaling

**Example**: Mining quests require minimum Mining skill level

### Crafting System Integration

**Recipe-Based Quest Generation**:
- Automatic quest creation from recipe requirements
- Material list auto-population
- Quality specification from recipe needs
- Batch processing for multiple recipes

**Example**: Selecting "Craft Steel Sword" offers to create quest for materials

### Economic System Integration

**Market Price Integration**:
- Suggested reward based on market prices
- Dynamic reward scaling with supply/demand
- Price history display
- Profitability calculator for completers

### Guild/Faction Integration

**Organizational Benefits**:
- Guild quest pool with member priorities
- Shared quest rewards to guild coffers
- Faction reputation rewards
- Guild resource allocation systems

## Summary

Player-created quest systems for resource gathering employ diverse patterns optimized for different use cases:

- **Bounty boards** work well for local, community-driven economies
- **Work orders** excel for integrated crafting workflows
- **Contracts** provide security for high-value transactions
- **Guild requisitions** enable large-scale collaborative projects

Successful implementations share common features:
- Clear creation interfaces with appropriate guidance
- Robust discovery mechanisms matching players to relevant quests
- Flexible quest types accommodating various resource needs
- Anti-exploitation measures maintaining economic health
- Integration with existing skill, crafting, and social systems

For BlueMarble, these patterns provide a foundation for designing a player-created quest system that:
- Supports geological resource specialization
- Encourages knowledge-based progression
- Enables collaborative terraforming projects
- Maintains educational value through structured resource gathering
- Creates sustainable player-driven economy

## Related Steps

- Next: [Step 2: Resource Request Design](../step-2-resource-request-design/) - Detailed analysis of how players structure resource requests
- Related: [Content Design](../../step-1-foundation/content-design/) - Quest design principles
- Related: [Crafting Systems](../../step-2-system-research/step-2.3-crafting-systems/) - Integration with crafting workflows
