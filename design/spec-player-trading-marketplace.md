# Player Trading Marketplace - Feature Specification

**Document Type:** Feature Specification  
**Version:** 1.0  
**Author:** BlueMarble Design Team  
**Date:** 2024-12-19  
**Status:** Draft  
**Epic/Theme:** Player Economy & Social Systems  
**Priority:** High

## Executive Summary

The Player Trading Marketplace is a comprehensive trading system that enables players to buy, sell, and exchange goods
in the BlueMarble medieval MMO simulation. This feature addresses the critical need for a robust player-driven economy
by providing intuitive interfaces for listing items, discovering goods, negotiating prices, and completing secure
transactions. The marketplace will serve as the economic backbone of the game, facilitating emergent gameplay through
supply and demand dynamics, price discovery, and specialized trading strategies.

The implementation will integrate with existing systems (inventory, currency, work-auction) while introducing new
mechanics for market analytics, reputation systems, and cross-island trade logistics. Expected impact includes
increased player engagement through economic gameplay loops, enhanced social interaction through trading relationships,
and deeper strategic depth through market speculation and resource management.

## Feature Overview

### Problem Statement

Players currently lack a centralized, efficient system for discovering and trading resources with other players. The
existing work-auction system handles labor but does not support direct item trading at scale. Players need:

- A way to discover what items are available for purchase across the game world
- Transparent price information to make informed trading decisions
- Secure transaction mechanisms to prevent fraud and ensure fair trades
- Tools to manage their own trading business and track market trends
- Cross-island trading capabilities to access specialized regional resources

Without a robust marketplace, players struggle to monetize their specialized production, find needed materials, and
participate in the emergent economy that defines the game's core experience.

### Solution Summary

The Player Trading Marketplace provides a multi-layered trading infrastructure:

1. **Marketplace UI**: Intuitive browsing and search interface with filtering by item type, price, location, and seller
2. **Listing System**: Sellers can create listings with custom prices, quantities, and optional bundling
3. **Transaction Engine**: Secure, atomic transactions that handle currency exchange and item transfer
4. **Price Discovery**: Historical price charts, current market rates, and demand indicators
5. **Reputation System**: Seller ratings, transaction history, and trust indicators
6. **Cross-Island Trade**: Logistics integration for shipping items between islands with transport costs
7. **Market Analytics**: Tools for tracking inventory value, profit margins, and market trends

### User Stories

- As a crafter, I want to sell my crafted goods at competitive prices so that I can profit from my specialized skills
- As a resource gatherer, I want to quickly find buyers for bulk materials so that I can focus on gathering rather
  than seeking individual buyers
- As a merchant player, I want to track price trends across different islands so that I can identify arbitrage
  opportunities
- As a buyer, I want to compare prices and seller reputations so that I can make informed purchasing decisions
- As a new player, I want to browse available items to understand what's valuable so that I can plan my progression
- As a guild leader, I want to bulk purchase supplies for my members so that we can coordinate large-scale projects
- As a specialized producer, I want to accept custom orders with deposits so that I can guarantee sales for
  high-value items

## Detailed Requirements

### Functional Requirements

1. **Item Listing Creation**
   - Description: Sellers can create marketplace listings for items in their inventory
   - Acceptance Criteria:
     - [ ] Interface allows selecting items from player inventory
     - [ ] Sellers can set custom prices per unit and total quantity
     - [ ] Optional listing duration with auto-expiration (1 day, 3 days, 7 days, until sold)
     - [ ] Support for bundling multiple related items in a single listing
     - [ ] Listing fee system (small percentage of list price) to prevent spam
     - [ ] Items are locked in inventory while listed (cannot be used or sold elsewhere)

2. **Marketplace Browsing & Search**
   - Description: Players can discover and filter available listings
   - Acceptance Criteria:
     - [ ] Category-based browsing (Resources, Tools, Crafted Goods, etc.)
     - [ ] Text search with fuzzy matching and suggestions
     - [ ] Advanced filters: price range, location/island, item quality, seller rating
     - [ ] Sort options: price (low/high), newest, ending soon, distance
     - [ ] Pagination with 20-50 items per page
     - [ ] Quick preview of item stats and seller info on hover

3. **Transaction Processing**
   - Description: Secure purchase and delivery of marketplace items
   - Acceptance Criteria:
     - [ ] Atomic transactions that verify currency availability and inventory space
     - [ ] Instant transfer for same-island transactions
     - [ ] Transport logistics calculation for cross-island trades
     - [ ] Currency deduction from buyer and deposit to seller minus marketplace fee
     - [ ] Transaction confirmation screen with cost breakdown
     - [ ] Automated rollback on transaction failure
     - [ ] Transaction history log for both parties

4. **Price Discovery & Analytics**
   - Description: Market information tools for informed trading decisions
   - Acceptance Criteria:
     - [ ] Display current lowest/highest/average price for each item type
     - [ ] Historical price chart (7-day, 30-day) showing price trends
     - [ ] Volume indicators showing recent transaction frequency
     - [ ] Price alerts for specific items reaching target thresholds
     - [ ] My listings dashboard showing performance metrics
     - [ ] Inventory valuation tool based on current market prices

5. **Reputation & Trust System**
   - Description: Build trader credibility through transaction history
   - Acceptance Criteria:
     - [ ] Seller rating (1-5 stars) based on completed transactions
     - [ ] Transaction count visible on seller profile
     - [ ] Buyer feedback/review system with text comments
     - [ ] Dispute resolution flag for problematic transactions
     - [ ] Trust badges for high-volume or long-standing traders
     - [ ] Option to filter searches by minimum seller rating

### Non-Functional Requirements

- **Performance:** Search results must return within 500ms, transaction processing within 2 seconds
- **Scalability:** System must handle 10,000 concurrent listings and 1,000 transactions per minute
- **Security:** All transactions must be atomic with rollback capabilities; input validation to prevent exploits
- **Accessibility:** UI must be fully navigable with keyboard, screen reader compatible, colorblind-friendly
- **Compatibility:** Works across web, iOS, Android with responsive design adapting to screen size
- **Data Integrity:** All transactions logged with immutable audit trail for anti-cheat analysis

## User Experience Design

### User Flow

```
Seller Flow:
1. Player opens Inventory → Selects "List on Marketplace" button
2. Marketplace Listing Screen appears
3. Select items to list (multi-select for bundles)
4. Set price per unit, total quantity, duration
5. Preview listing with fee calculation
6. Confirm → Items locked in inventory with "Listed" indicator
7. Listing appears in marketplace, player receives notification when sold

Buyer Flow:
1. Player opens Marketplace from main menu
2. Browse categories OR use search bar
3. Apply filters (price, location, quality)
4. Click listing to view details
5. Check seller reputation, price history
6. Click "Buy Now" → Confirmation screen with cost breakdown
7. Confirm purchase → Currency deducted, items added to inventory
8. Notification confirms successful transaction

Market Analytics Flow:
1. Player opens "My Listings" or "Market Trends" tab
2. View active listings with views/interest indicators
3. Check price charts for items they trade frequently
4. Set price alerts for target buy/sell prices
5. Adjust listings based on market data
```

### Interface Requirements

- **Marketplace Main Screen**: Split view with category sidebar (left), listing grid (center), filters/sort (top right)
- **Listing Detail Modal**: Item stats, seller info, price comparison, buy button, all in overlay dialog
- **Create Listing Form**: Item selector, price inputs, duration picker, fee preview, submit button
- **My Listings Dashboard**: Table view with active/completed tabs, edit/cancel actions, performance metrics
- **Price Charts**: Line graph with adjustable timeframe, hover tooltips showing exact values
- **Transaction History**: Chronological list with item icons, prices, parties involved, timestamps
- **Reputation Display**: Star rating widget, transaction count badge, reviews expandable section
- **Mobile Adaptations**: Bottom sheet for filters, swipeable listing cards, simplified chart views

### Wireframes/Mockups

Design assets will be stored in `/assets/mockups/marketplace/` with the following structure:

```
assets/mockups/marketplace/
├── marketplace-main-view-v1.png          # Full marketplace browser with categories
├── listing-detail-modal-v1.png           # Item detail popup with buy options
├── create-listing-form-v1.png            # Seller's listing creation interface
├── my-listings-dashboard-v1.png          # Seller's management dashboard
├── price-chart-widget-v1.png             # Historical price visualization
├── mobile-marketplace-browse-v1.png      # Mobile-optimized browsing
├── mobile-listing-detail-v1.png          # Mobile item detail view
└── user-flow-diagram-v1.png              # Complete user journey diagram
```

Architecture diagram will be stored as:

```
assets/diagrams/marketplace/
├── marketplace-system-architecture-v1.png    # High-level system components
├── transaction-flow-diagram-v1.png          # Transaction processing sequence
└── database-schema-marketplace-v1.png       # Data model relationships
```

## Technical Considerations

### Architecture Overview

**Client-Server Architecture:**

```
Client Layer (WebGL/Mobile)
├── Marketplace UI Component
├── Listing Manager
├── Transaction Handler
└── Market Data Visualizer

API Gateway Layer
├── Marketplace Service API
├── Transaction Processing API
├── Search & Filter API
└── Analytics API

Backend Services Layer
├── Listing Service (Create, Update, Delete, Query)
├── Transaction Service (Validate, Execute, Rollback)
├── Search Service (Elasticsearch/Meilisearch for fast queries)
├── Analytics Service (Price aggregation, trend calculation)
├── Reputation Service (Rating calculation, review management)
└── Transport Service (Cross-island logistics)

Data Layer
├── Listings Database (PostgreSQL)
├── Transaction Log (Time-series DB)
├── Search Index (Elasticsearch)
├── Cache Layer (Redis for hot data)
└── Analytics Warehouse (Clickhouse for aggregations)
```

**Key Technical Decisions:**

1. **Database Choice**: PostgreSQL for transactional data with ACID guarantees, Elasticsearch for search performance
2. **Caching Strategy**: Redis cache for popular item prices (TTL 5 min) and active listings (TTL 1 min)
3. **Transaction Isolation**: Serializable isolation level to prevent race conditions in concurrent purchases
4. **Real-time Updates**: WebSocket connections for instant notification of listing changes and completed sales
5. **Scalability**: Horizontal scaling of read-heavy search service, write-optimized listing service
6. **Data Partitioning**: Listings partitioned by island/region for geographic query optimization

### API Endpoints (if applicable)

| Method | Endpoint | Description | Request | Response |
|--------|----------|-------------|---------|----------|
| GET | /api/marketplace/listings | Fetch marketplace listings with filters | Query params: category, minPrice, maxPrice, island, sort, page | Paginated listing array |
| POST | /api/marketplace/listings | Create new marketplace listing | Body: {itemId, quantity, pricePerUnit, duration} | Created listing object |
| GET | /api/marketplace/listings/{id} | Get specific listing details | Path param: listing ID | Full listing with seller info |
| PUT | /api/marketplace/listings/{id} | Update existing listing price/quantity | Body: {pricePerUnit, quantity} | Updated listing object |
| DELETE | /api/marketplace/listings/{id} | Cancel and remove listing | Path param: listing ID | Success confirmation |
| POST | /api/marketplace/transactions | Purchase item from marketplace | Body: {listingId, quantity} | Transaction receipt |
| GET | /api/marketplace/transactions | Get user transaction history | Query params: type (buy/sell), page | Transaction list |
| GET | /api/marketplace/analytics/prices/{itemId} | Get price history for item | Path param: itemId, Query: timeframe | Price data points array |
| GET | /api/marketplace/analytics/trends | Get market trend data | Query params: category, timeframe | Trend statistics |
| POST | /api/marketplace/reputation/rate | Submit seller rating | Body: {sellerId, rating, comment} | Rating confirmation |

### Data Model

**Listing Entity:**

```
Listing {
  id: UUID
  sellerId: UUID (foreign key to Player)
  itemId: UUID (foreign key to Item)
  quantity: Integer
  pricePerUnit: Decimal
  totalPrice: Decimal (calculated)
  listingFee: Decimal
  island: String
  expiresAt: Timestamp
  createdAt: Timestamp
  status: Enum (ACTIVE, SOLD, EXPIRED, CANCELLED)
  views: Integer (incremented on view)
  favorites: Integer (saved by buyers)
}
```

**Transaction Entity:**

```
Transaction {
  id: UUID
  listingId: UUID (foreign key to Listing)
  buyerId: UUID (foreign key to Player)
  sellerId: UUID (foreign key to Player)
  itemId: UUID
  quantity: Integer
  pricePerUnit: Decimal
  totalPrice: Decimal
  marketplaceFee: Decimal
  transportCost: Decimal (if cross-island)
  status: Enum (PENDING, COMPLETED, FAILED, REFUNDED)
  completedAt: Timestamp
  createdAt: Timestamp
}
```

**PriceHistory Entity (for analytics):**

```
PriceHistory {
  id: UUID
  itemId: UUID
  island: String
  price: Decimal
  quantity: Integer
  timestamp: Timestamp
  source: Enum (SALE, LISTING)
}
```

**SellerReputation Entity:**

```
SellerReputation {
  sellerId: UUID (primary key, foreign key to Player)
  totalTransactions: Integer
  completedTransactions: Integer
  averageRating: Decimal
  totalRatings: Integer
  lastTransactionAt: Timestamp
  trustScore: Decimal (calculated)
}
```

**Database Indexes:**

- Listings: (island, status, createdAt), (itemId, status), (sellerId, status)
- Transactions: (buyerId, createdAt), (sellerId, createdAt), (status, completedAt)
- PriceHistory: (itemId, timestamp), (island, itemId, timestamp)

### Third-Party Integrations

- **Elasticsearch/Meilisearch**: Fast full-text search for item names and descriptions with typo tolerance
- **Redis**: Caching layer for frequently accessed data (hot items, price aggregations)
- **WebSocket Service**: Real-time notifications for marketplace events (new listings, sold items, price changes)
- **Analytics Platform**: Integration with game analytics system for tracking marketplace usage and economic health
- **Anti-Cheat System**: Hooks for detecting market manipulation, price fixing, and automated trading bots

## Testing Strategy

### Test Cases

1. **Create Listing - Happy Path**
   - Preconditions: Player has items in inventory, sufficient currency for listing fee
   - Steps:
     1. Open inventory and select item
     2. Click "List on Marketplace"
     3. Enter price and quantity
     4. Confirm listing
   - Expected Result: Listing created, items locked in inventory, listing appears in marketplace searches

2. **Purchase Item - Same Island**
   - Preconditions: Active listing exists, buyer has sufficient currency and inventory space
   - Steps:
     1. Search for item in marketplace
     2. Click listing to view details
     3. Click "Buy Now"
     4. Confirm transaction
   - Expected Result: Currency deducted from buyer, items added to buyer inventory, listing removed, seller receives
     payment minus fees

3. **Price Chart Display**
   - Preconditions: Historical price data exists for item
   - Steps:
     1. Open listing detail
     2. Navigate to "Price History" tab
     3. Select 7-day timeframe
   - Expected Result: Line chart displays with date on X-axis, price on Y-axis, tooltips show exact values on hover

4. **Concurrent Purchase Conflict**
   - Preconditions: Listing with quantity=1, two buyers attempt simultaneous purchase
   - Steps:
     1. Buyer A initiates purchase
     2. Buyer B initiates purchase 100ms later
     3. Both confirm within transaction window
   - Expected Result: First transaction succeeds, second receives "item no longer available" error, no currency lost

5. **Cross-Island Trade**
   - Preconditions: Listing on Island A, buyer on Island B
   - Steps:
     1. Buyer searches marketplace, finds item on different island
     2. Views listing detail showing transport cost
     3. Confirms purchase with total cost breakdown
   - Expected Result: Transaction includes transport cost, item added to buyer inventory after simulated transport delay,
     notification sent when delivery complete

### Edge Cases

- **Listing Expiration**: Listings automatically marked EXPIRED when duration elapses, items returned to seller
  inventory with notification
- **Insufficient Inventory Space**: Purchase fails gracefully with clear error if buyer lacks space, currency not
  deducted
- **Seller Deletes Listed Item**: System prevents item deletion while listed; must cancel listing first
- **Market Manipulation**: Monitor for price patterns indicating collusion, implement cooldowns on rapid price changes
- **Network Interruption During Transaction**: Transaction marked PENDING with 5-minute timeout, auto-rollback if not
  completed
- **Seller Goes Offline**: Listings remain active; transactions complete automatically with items/currency in escrow
- **Invalid Pricing**: Frontend validation prevents negative prices, backend validation prevents prices exceeding
  reasonable bounds (e.g., 1000x normal value)

### Performance Testing

**Load Testing Requirements:**

- Simulate 1,000 concurrent users browsing marketplace
- 100 transactions per second purchase rate
- Search query response time < 500ms at 95th percentile
- Transaction completion time < 2 seconds at 95th percentile
- Database connection pool handles burst traffic without timeouts

**Stress Testing Scenarios:**

- Peak load: 5,000 concurrent users during major game events
- Database failure recovery: Verify transaction rollback on DB disconnect
- Cache invalidation: Ensure price updates propagate within 30 seconds
- Memory leak testing: 24-hour sustained load test with monitoring

## Risks and Mitigation

| Risk | Probability | Impact | Mitigation Strategy |
|------|-------------|--------|---------------------|
| Market manipulation (price fixing) | Medium | High | Implement rate limiting, anomaly detection, manual review flags for suspicious patterns |
| Transaction exploits (duplication) | Low | Critical | Extensive security testing, atomic transactions with serializable isolation, audit logging |
| Performance degradation at scale | High | High | Horizontal scaling architecture, caching strategy, load testing, database optimization |
| User confusion with complex UI | Medium | Medium | User testing sessions, progressive disclosure, tooltips, tutorial integration |
| Cross-island trade complexity | Medium | Medium | Clear cost breakdown, estimated delivery time, option to disable for simplicity |
| Inflation from over-listing | Low | Medium | Listing fees to prevent spam, market analytics to monitor health, adjust fees dynamically |
| Real-money trading (RMT) | Medium | High | Reputation system, transaction monitoring, ToS enforcement, report mechanisms |

## Dependencies

### Internal Dependencies

- **Inventory System**: Must expose API for locking/unlocking items, transferring items between players
- **Currency System**: Integration for deducting/adding currency with transaction logging
- **Work-Auction System**: Share reputation data, potentially unified transaction history
- **Player Profile System**: Access to player names, trust scores, transaction history
- **Transport/Logistics System**: Calculate shipping costs and delivery times for cross-island trades
- **Notification System**: Push notifications for sales, purchases, listing expirations
- **Anti-Cheat System**: Integration for flagging suspicious trading patterns

### External Dependencies

- **Search Infrastructure**: Elasticsearch or Meilisearch deployment and configuration
- **WebSocket Server**: Real-time communication infrastructure for live updates
- **Analytics Pipeline**: Data warehouse for market trend analysis
- **Payment Processing**: If marketplace includes premium listing features (future consideration)

### Blocking Dependencies

- **Inventory System API**: Must be completed and stable before marketplace can lock items
- **Transaction Framework**: Core atomic transaction system must exist before marketplace transactions
- **Database Schema Migration**: Requires database infrastructure supporting new marketplace tables

## Success Metrics

### Key Performance Indicators (KPIs)

- **Marketplace Adoption**: 60% of active players create at least one listing within first month
- **Transaction Volume**: Average 10,000 transactions per day within 3 months of launch
- **User Engagement**: Players spend average 15 minutes per session browsing marketplace
- **Listing Success Rate**: 70% of listings result in completed sale within expiration period
- **Price Discovery Efficiency**: Price variance for common items decreases 30% after 2 months
- **Cross-Island Trade**: 25% of transactions involve cross-island shipping
- **Reputation System Usage**: 40% of buyers leave ratings after transactions

### Analytics Requirements

**Track the following metrics:**

- Listing creation rate (per day, per item category)
- Transaction completion rate and failure reasons
- Average listing duration before sale
- Price trends over time per item type
- Search query patterns and popular filters
- User session time in marketplace UI
- Conversion rate from listing view to purchase
- Seller repeat rate (returning to list more items)
- Buyer repeat rate (frequency of purchases)
- Revenue from listing fees and transaction fees
- Geographic distribution of trades (heatmap of island connections)

**Dashboard Requirements:**

- Real-time marketplace health dashboard for game admins
- Economic balance monitoring (inflation indicators, supply/demand ratios)
- Anti-cheat alerts for suspicious patterns
- Player-facing analytics for their own trading performance

## Timeline and Phases

### Phase 1: Core Marketplace (8 weeks)

- **Duration:** Weeks 1-8
- **Deliverables:**
  - Basic listing creation and browsing UI
  - Search and filter functionality
  - Transaction processing system
  - Database schema and API implementation
  - Unit and integration tests
- **Success Criteria:** Players can list items, search marketplace, and complete same-island transactions

### Phase 2: Analytics & Reputation (4 weeks)

- **Duration:** Weeks 9-12
- **Deliverables:**
  - Price history charts and trend analysis
  - Seller reputation system with ratings
  - My Listings dashboard with performance metrics
  - Transaction history viewer
- **Success Criteria:** Players can view price trends, rate sellers, and track their trading performance

### Phase 3: Advanced Features (6 weeks)

- **Duration:** Weeks 13-18
- **Deliverables:**
  - Cross-island trading with logistics integration
  - Price alerts and watchlists
  - Bulk listing tools for high-volume sellers
  - Advanced search filters and saved searches
  - Mobile UI optimization
- **Success Criteria:** Cross-island trades functional, mobile experience polished, power-user tools available

### Phase 4: Polish & Optimization (2 weeks)

- **Duration:** Weeks 19-20
- **Deliverables:**
  - Performance optimization based on load testing
  - UI/UX refinements from user feedback
  - Tutorial and onboarding flow
  - Anti-cheat system integration
  - Documentation and admin tools
- **Success Criteria:** System handles target load, tutorials complete, ready for release

## Out of Scope

**Explicitly NOT included in this initial release:**

- Auction system with bidding (separate feature for future)
- Direct player-to-player trading UI (separate from marketplace listings)
- Guild/alliance bulk purchasing agreements
- Crafting commission system (custom order marketplace)
- Marketplace API for third-party tools
- In-game advertising system for listings
- Premium listing features (highlighted listings, featured placement)
- Currency exchange between different currency types
- Automated trading bots or API access
- Physical marketplace locations (in-game buildings)

## Future Considerations

**Potential enhancements for future releases:**

- **Auction House Mode**: Time-limited bidding on rare items with automatic bid increments
- **Commission System**: Accept custom orders with deposit requirements for specialized crafting
- **Guild Marketplaces**: Private trading systems for guild members with discounted fees
- **Market Forecasting**: AI-driven price prediction tools for strategic traders
- **Trading Caravans**: Visual representation of cross-island trades with interception risk/reward
- **Marketplace Buildings**: Physical storefronts in settlements with customization options
- **Trading Achievements**: Badges and titles for marketplace milestones (100 sales, 1M currency traded)
- **Seasonal Events**: Limited-time marketplace events with special items or bonuses
- **API Access**: Authenticated API for player-created market analysis tools
- **Mobile Notifications**: Push notifications for price alerts, sold items, new listings matching criteria

## Appendices

### Appendix A: Research and References

- **Game Economics Research**: `research/topics/player-driven-economies.md`
- **UI/UX Patterns**: `research/topics/marketplace-ui-best-practices.md`
- **Competitive Analysis**: `research/game-design/mmo-marketplace-analysis.md`
- **Player Surveys**: `research/experiments/2024-06-marketplace-player-needs.md`
- **Economic Modeling**: `docs/gameplay/economy-simulation-parameters.md`

### Appendix B: Revision History

| Version | Date | Author | Changes |
|---------|------|--------|---------|
| 1.0 | 2024-12-19 | BlueMarble Design Team | Initial comprehensive specification document |

### Appendix C: Related Documents

- [Core Economy Design](economy.md) - Overall economic system design
- [Work-Auction System](../docs/gameplay/spec-work-auction.md) - Labor marketplace integration
- [Inventory System Spec](../docs/systems/inventory-system.md) - Item management technical details
- [Island Transport Design](../docs/gameplay/transport-logistics.md) - Cross-island shipping mechanics
- [Player Reputation System](../docs/systems/reputation-framework.md) - Trust and rating infrastructure
- [UI/UX Guidelines](../docs/ui-ux/ui-guidelines.md) - Interface design standards
- [Anti-Cheat Framework](../docs/systems/anti-cheat-architecture.md) - Security and fraud prevention

### Appendix D: Design Assets Reference

All visual design assets for this feature will be organized in:

- **Wireframes**: `/assets/mockups/marketplace/`
- **System Diagrams**: `/assets/diagrams/marketplace/`
- **UI Components**: `/assets/concepts/marketplace-ui-components/`
- **User Flow Diagrams**: `/assets/diagrams/user-flows/marketplace-flows/`

Asset naming convention: `marketplace-[component]-[purpose]-v[version].[extension]`

---

**Document Status:** This specification is in DRAFT status and requires review from:

- Game Design Lead (gameplay balance and progression integration)
- Technical Lead (architecture feasibility and scalability)
- UX Designer (interface usability and accessibility)
- Economy Designer (pricing models and market health)
- Product Manager (scope and timeline validation)
