# BlueMarble - Economy Systems Design

**Version:** 1.0  
**Date:** 2025-09-29  
**Author:** BlueMarble Economy Team

## Economic Philosophy

BlueMarble's economy is designed to be player-driven, sustainable, and engaging. The system encourages meaningful
player interactions while maintaining balanced progression and preventing economic exploitation.

## Core Economic Principles

### Player-Driven Market

- **Supply and Demand:** Prices fluctuate based on actual player activity
- **Player Production:** Most valuable items created by player crafting
- **Market Competition:** Multiple sources for most goods encourage healthy competition
- **Economic Roles:** Specialized economic professions with unique value

### Balanced Circulation

- **Currency Sinks:** Mechanisms to remove money from the economy
- **Resource Renewal:** Sustainable resource generation systems
- **Inflation Control:** Policies to maintain stable purchasing power
- **Wealth Distribution:** Systems to prevent excessive wealth concentration

### Meaningful Choice

- **Trade-offs:** Economic decisions with real consequences
- **Specialization Benefits:** Advantages for players who focus on specific areas
- **Risk and Reward:** Higher risks offer proportionally higher rewards
- **Long-term Planning:** Benefits for players who plan economic strategies

## Currency Systems

### Primary Currencies

#### Gold Coins

- **Value:** Primary high-value currency
- **Sources:** Quest rewards, trading, high-level activities
- **Uses:** Major purchases, housing, expensive equipment
- **Inflation Control:** Limited generation, significant sinks

#### Silver Coins

- **Value:** Medium-value everyday currency
- **Sources:** Regular gameplay, moderate trading, mid-level content
- **Uses:** Equipment repairs, consumables, common items
- **Exchange Rate:** 100 silver = 1 gold (stable)

#### Copper Coins

- **Value:** Low-value currency for small transactions
- **Sources:** Basic activities, vendor sales, small trades
- **Uses:** Basic consumables, vendor items, tips
- **Exchange Rate:** 100 copper = 1 silver (stable)

### Special Currencies

#### Honor Points

- **Purpose:** PvP reward currency
- **Sources:** Player vs Player combat victories, tournaments
- **Uses:** PvP equipment, cosmetics, special abilities
- **Transfer:** Non-tradeable to prevent exploitation

#### Guild Tokens

- **Purpose:** Guild contribution tracking
- **Sources:** Guild activities, donations, shared achievements
- **Uses:** Guild improvements, member benefits, special events
- **Management:** Guild leadership controls distribution

#### Faction Credits

- **Purpose:** Reputation with specific organizations
- **Sources:** Faction-specific quests and activities
- **Uses:** Unique items, access to restricted areas, special training
- **Conversion:** Limited conversion between some faction currencies

#### Event Currency

- **Purpose:** Seasonal and special event participation
- **Sources:** Event-specific activities and achievements
- **Uses:** Exclusive cosmetics, temporary bonuses, collectibles
- **Expiration:** Limited-time availability to maintain exclusivity

## Trading Systems

### Player-to-Player Trading

#### Direct Trade Interface

- **Security:** Secure trading window with confirmation steps
- **Item Display:** Clear preview of items being exchanged
- **Currency Exchange:** Support for mixed currency and item trades
- **Trade History:** Log of recent trades for reference and dispute resolution

#### Trade Chat System

- **Communication:** Dedicated channel for trade negotiations
- **Moderation:** Anti-spam measures and community moderation
- **Search Function:** Ability to search for specific items or services
- **Reputation Integration:** Display trader reputation scores

### Player Trading Marketplace

> **Comprehensive Documentation:** See [Marketplace Feature Specification](../../roadmap/tasks/player-trading-marketplace.md)
> for complete feature details, [Marketplace API](api-marketplace.md) for technical specifications, and
> [Marketplace Usage Guide](../gameplay/marketplace-usage-guide.md) for player instructions.

The Player Trading Marketplace is the primary hub for player-driven commerce in BlueMarble, offering a secure,
feature-rich platform for buying, selling, and trading items across the game world.

#### Core Features

- **Item Listings:** Players list items for sale at custom prices with flexible duration options
- **Advanced Search:** Powerful filtering by category, price, location, quality, and seller reputation
- **Secure Transactions:** Atomic transaction processing with full rollback capabilities
- **Price Discovery:** Historical price charts, market trends, and analytics tools
- **Reputation System:** Seller ratings, trust badges, and transaction history
- **Cross-Island Trade:** Logistics integration for shipping items between islands with transport costs
- **Market Analytics:** Real-time data on prices, trends, and trading opportunities

#### Listing Mechanics

- **Listing Fee:** 1-5% of list price to prevent spam (non-refundable)
- **Duration Options:** 1 day, 3 days, 7 days, or until sold (premium feature)
- **Fixed Pricing:** Set price per unit with instant purchase option
- **Quantity Control:** List partial stacks or entire inventory quantities
- **Item Locking:** Listed items locked in inventory until sold or cancelled

#### Transaction Processing

- **Atomic Transactions:** Verify currency and inventory space, then execute instantly
- **Marketplace Fee:** 5% transaction fee deducted from seller proceeds
- **Transport Costs:** Distance-based shipping fees for cross-island trades
- **Currency Support:** Accepts gold, silver, and copper with automatic conversion
- **Instant Fulfillment:** Same-island trades complete immediately
- **Scheduled Delivery:** Cross-island items delivered after transport time (2-24 hours)

#### Search and Filtering

- **Category Filters:** Browse by Resources, Tools, Crafted Goods, Consumables, Rare Items
- **Price Range:** Set minimum and maximum price filters
- **Location Filters:** Filter by island to avoid or target transport costs
- **Quality Filters:** Filter by item quality tiers
- **Seller Filters:** Search by minimum reputation rating
- **Sort Options:** Price (low/high), newest, ending soon, distance

#### Market Analytics

- **Price History:** 7-day and 30-day historical charts with trend visualization
- **Current Market Rate:** Display average, lowest, and highest current prices
- **Volume Indicators:** Show recent transaction frequency and total volume
- **Price Alerts:** Set notifications for items reaching target prices
- **Market Trends Dashboard:** Category-wide analytics and arbitrage opportunities
- **Inventory Valuation:** Calculate total inventory value based on market prices

#### Reputation and Trust

- **Seller Ratings:** 1-5 star system based on buyer feedback
- **Transaction Count:** Public display of completed transactions
- **Trust Score:** Calculated metric (0-100) based on performance
- **Trust Badges:** Earned through consistent quality (Trusted Seller, Fast Shipper, etc.)
- **Review System:** Buyers can leave text reviews and rate transactions
- **Dispute Resolution:** Flagging system for problematic transactions with moderator review

#### Cross-Island Trading

- **Distance Calculation:** Transport costs based on island distance
- **Weight/Volume Pricing:** Heavier or bulkier items cost more to ship
- **Delivery Tracking:** Real-time updates on shipment status
- **Express Delivery:** Optional 2x cost for 50% faster delivery
- **Regional Pricing:** Take advantage of price differences between islands for arbitrage

### Auction House System (Future Enhancement)

The auction house system is planned as a future addition to complement the marketplace with time-based bidding:

#### Planned Features

- **Timed Bidding:** Competitive bidding with automatic auction end times
- **Bid Increments:** Minimum bid increase requirements
- **Proxy Bidding:** Automatic bidding up to maximum amount
- **Last-Minute Extensions:** Prevent sniping with extended time for last-minute bids
- **Reserve Prices:** Minimum sale price set by seller

### Merchant NPCs

#### Standard Vendors

- **Basic Goods:** Common consumables, tools, and materials
- **Consistent Pricing:** Stable prices for essential items
- **Limited Inventory:** Restricted quantities to encourage player trading
- **Reputation Discounts:** Price reductions for high-reputation players

#### Specialty Merchants

- **Unique Items:** Rare materials and specialized equipment
- **Dynamic Inventory:** Stock changes based on world events and player actions
- **Reputation Requirements:** Access restricted by faction standing
- **Limited Quantities:** Scarce items with restocking timers

## Crafting Economy

> **Detailed Mechanics:** See [Crafting Mechanics Documentation](../gameplay/mechanics/crafting-mechanics-overview.md)
> for formal mathematical models, success rates, and quality calculations.

### Production Systems

#### Resource Gathering

- **Mining:** Extract ores and gems from designated areas
- **Herbalism:** Collect plants and magical components
- **Hunting:** Obtain leather, bones, and other animal products
- **Logging:** Harvest different types of wood and tree materials

#### Processing Chains

- **Raw to Refined:** Multi-step processing for valuable materials
- **Specialization Benefits:** Efficiency bonuses for focused crafters
- **Quality Variations:** Different processing methods yield different quality results
- **Batch Processing:** Efficiency improvements for bulk processing

### Crafting Professions

#### Blacksmithing

- **Primary Products:** Weapons, armor, tools, hardware
- **Key Materials:** Ores, metals, gems, crafting components
- **Specializations:** Weapons, armor, or decorative items
- **Economic Role:** High-value item production, equipment customization

#### Alchemy

- **Primary Products:** Potions, elixirs, magical components
- **Key Materials:** Herbs, crystals, creature parts, pure water
- **Specializations:** Healing, enhancement, or utility potions
- **Economic Role:** Consumable production, magical augmentation

#### Enchanting

- **Primary Products:** Magical enhancements for equipment
- **Key Materials:** Magical crystals, rare components, enchanted items
- **Specializations:** Weapon, armor, or utility enchantments
- **Economic Role:** Item improvement, magical services

#### Tailoring

- **Primary Products:** Clothing, light armor, bags, decorative items
- **Key Materials:** Cloth, leather, thread, dyes, decorative elements
- **Specializations:** Functional, cosmetic, or magical clothing
- **Economic Role:** Equipment production, fashion items, storage solutions

### Market Integration

#### Crafter Reputation

- **Quality Ratings:** Customer feedback on crafted items
- **Specialization Recognition:** Known expertise in specific areas
- **Commission System:** Custom orders with reputation bonuses
- **Master Crafter Status:** Elite recognition for exceptional crafters

#### Supply Chain Management

- **Material Sourcing:** Relationships with gatherers and suppliers
- **Production Planning:** Anticipating market demands
- **Inventory Management:** Balancing stock levels and storage costs
- **Distribution Networks:** Efficient delivery of finished products

## Economic Balance Mechanisms

### Currency Sinks

#### Equipment Maintenance

- **Repair Costs:** Regular expenses for equipment upkeep
- **Upgrade Fees:** Costs for item improvements and modifications
- **Transportation:** Fast travel and teleportation fees
- **Housing Costs:** Property taxes and maintenance expenses

#### Guild Operations

- **Guild Hall Maintenance:** Ongoing costs for guild facilities
- **Event Organization:** Expenses for guild tournaments and activities
- **Territory Control:** Costs associated with controlling areas
- **Member Benefits:** Guild-funded improvements and bonuses

### Inflation Controls

#### Dynamic Pricing

- **Vendor Adjustment:** NPC prices adjust based on server economy
- **Tax Systems:** Progressive taxation on high-value transactions
- **Market Fees:** Transaction costs that scale with item value
- **Currency Exchange:** Controlled exchange rates between currencies

#### Supply Management

- **Resource Scarcity:** Limited availability of high-value materials
- **Seasonal Variations:** Changing availability based on world events
- **Quality Degradation:** Items that lose value over time
- **Consumption Requirements:** Ongoing costs for optimal performance

### Wealth Distribution

#### Progressive Systems

- **Diminishing Returns:** Reduced efficiency for excessive wealth accumulation
- **Charity Incentives:** Benefits for players who help newcomers
- **Community Projects:** Server-wide goals that benefit everyone
- **Taxation on Luxury:** Higher costs for non-essential premium items

#### Social Economy

- **Mentorship Rewards:** Benefits for teaching other players
- **Guild Sharing:** Systems that encourage resource sharing
- **Community Events:** Activities that promote economic cooperation
- **Reputation Benefits:** Economic advantages for positive community members

## Regional Economics

### Economic Zones

#### Commercial Centers

- **Aethermere Capital:** Primary trading hub with all facilities
- **High Volume:** Major auction house and merchant concentration
- **Price Stability:** Competitive pricing due to market efficiency
- **International Trade:** Gateway for cross-server economics

#### Resource Regions

- **Specialized Production:** Areas known for specific resources
- **Local Markets:** Regional trading with unique opportunities
- **Transportation Costs:** Distance affects delivery prices
- **Seasonal Availability:** Weather and events affect resource production

#### Frontier Markets

- **High Risk/Reward:** Dangerous areas with valuable opportunities
- **Limited Infrastructure:** Reduced trading facilities and security
- **Speculation:** High volatility and investment opportunities
- **Pioneer Benefits:** Early adopter advantages in new territories

### Cross-Regional Trade

#### Transportation Systems

- **Merchant Caravans:** Bulk goods transportation with scheduling
- **Portal Networks:** Expensive but instant item transfer
- **Player Couriers:** Players can offer delivery services
- **Risk Management:** Insurance options for valuable shipments

#### Arbitrage Opportunities

- **Price Differences:** Regional variations create trading opportunities
- **Information Networks:** Players share market intelligence
- **Timing Strategies:** Seasonal and event-based trading
- **Investment Risks:** Potential losses from market changes

## Player Economic Roles

### Economic Archetypes

#### The Merchant

- **Focus:** Buy low, sell high across different markets
- **Skills:** Market analysis, negotiation, reputation management
- **Tools:** Price tracking, inventory management, transportation
- **Success Factors:** Information, timing, risk management

#### The Crafter

- **Focus:** Transform raw materials into valuable products
- **Skills:** Production efficiency, quality control, innovation
- **Tools:** Crafting equipment, material sourcing, custom orders
- **Success Factors:** Specialization, reputation, supply relationships

#### The Investor

- **Focus:** Long-term wealth accumulation through strategic investments
- **Skills:** Market prediction, portfolio management, risk assessment
- **Tools:** Market analysis, diverse holdings, economic forecasting
- **Success Factors:** Patience, diversification, market knowledge

#### The Service Provider

- **Focus:** Offer specialized services to other players
- **Skills:** Expertise development, customer service, marketing
- **Tools:** Professional equipment, reputation systems, advertising
- **Success Factors:** Skill mastery, reliability, customer satisfaction

### Economic Progression

#### Beginner Phase

- **Basic Trading:** Simple buy/sell transactions with NPCs
- **Gathering Start:** Introduction to resource collection
- **Market Learning:** Understanding basic economic principles
- **Safety Net:** Systems to prevent new player exploitation

#### Intermediate Phase

- **Specialization Choice:** Focusing on specific economic activities
- **Market Participation:** Active trading in player markets
- **Relationship Building:** Developing supplier and customer networks
- **Risk Management:** Learning to handle economic uncertainties

#### Advanced Phase

- **Market Leadership:** Influencing regional or server-wide markets
- **Complex Strategies:** Multi-faceted economic operations
- **Mentorship:** Teaching and guiding newer economic players
- **Innovation:** Developing new economic strategies and relationships

## Economic Events and Disruptions

### Planned Economic Events

#### Seasonal Markets

- **Harvest Festivals:** Increased food and agricultural trading
- **Winter Preparations:** High demand for warm clothing and supplies
- **Spring Expansion:** New resource availability and exploration
- **Summer Celebrations:** Luxury goods and entertainment focus

#### World Events

- **War Economics:** Military supplies in high demand
- **Discovery Events:** New resources create market opportunities
- **Celebration Periods:** Luxury and gift items become valuable
- **Disaster Response:** Emergency supplies and reconstruction materials

### Economic Emergencies

#### Market Crashes

- **Cause Identification:** Understanding what triggered the crash
- **Stabilization Measures:** Administrative tools to restore balance
- **Recovery Support:** Assistance for affected players
- **Prevention Systems:** Safeguards against future crashes

#### Exploitation Response

- **Detection Systems:** Automated monitoring for unusual patterns
- **Investigation Process:** Thorough review of suspected violations
- **Corrective Actions:** Appropriate penalties and market corrections
- **Communication:** Transparent reporting to the community

## Monitoring and Analytics

### Economic Metrics

#### Market Health Indicators

- **Price Stability:** Monitoring for excessive volatility
- **Trading Volume:** Ensuring adequate market activity
- **Wealth Distribution:** Tracking concentration of resources
- **Inflation Rates:** Monitoring currency purchasing power

#### Player Satisfaction

- **Economic Accessibility:** Ensuring fair opportunities for all players
- **Progression Satisfaction:** Players feel economic advancement is meaningful
- **System Fairness:** Perception that economic systems are balanced
- **Engagement Levels:** Active participation in economic activities

### Data Collection

#### Automated Tracking

- **Transaction Logging:** Complete records of all economic activities
- **Price Monitoring:** Real-time tracking of market prices
- **Volume Analysis:** Understanding market activity patterns
- **Player Behavior:** Anonymized tracking of economic choices

#### Regular Reporting

- **Weekly Summaries:** Brief reports on market conditions
- **Monthly Analysis:** Detailed review of economic trends
- **Quarterly Reviews:** Comprehensive evaluation of system health
- **Annual Planning:** Long-term economic strategy development

## Future Economic Considerations

### Scalability Planning

- **Server Growth:** Economic systems that handle increasing player populations
- **Content Expansion:** Economic integration for new game content
- **Cross-Server Trading:** Potential for inter-server economic activities
- **Platform Integration:** Economic systems for future platform releases

### Technology Integration

- **AI Market Analysis:** Automated tools for economic monitoring
- **Blockchain Integration:** Potential for secure, transparent transactions
- **External APIs:** Integration with external economic tracking tools
- **Mobile Connectivity:** Economic features accessible from mobile devices

---

*This economy design document provides the framework for BlueMarble's economic systems. Regular monitoring and
adjustment will be necessary to maintain balance and player satisfaction as the economy evolves.*
