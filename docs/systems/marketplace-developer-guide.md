# Player Trading Marketplace - Developer Integration Guide

**Document Type:** Developer Guide  
**Version:** 1.0  
**Author:** BlueMarble Design Team  
**Date:** 2024-12-29  
**Target Audience:** Backend and Frontend Developers

## Overview

This guide provides developers with the technical details needed to integrate the Player Trading Marketplace into the
BlueMarble game. It covers architecture, integration points, implementation guidelines, and best practices.

## Prerequisites

Before integrating marketplace features, ensure you have:

- Working knowledge of BlueMarble's service architecture
- Access to the inventory and currency systems
- Understanding of atomic transactions and database isolation
- Familiarity with WebSocket for real-time updates
- Experience with search indexing (Elasticsearch/Meilisearch)

## Architecture Overview

### System Components

```text
┌─────────────────────────────────────────────────────────────┐
│                     Client Layer                             │
│  - Marketplace UI Component                                  │
│  - Search & Filter Interface                                 │
│  - Transaction Confirmation Dialogs                          │
│  - Real-time Price Updates (WebSocket)                       │
└─────────────────────────────────────────────────────────────┘
                            │
                            ▼
┌─────────────────────────────────────────────────────────────┐
│                    API Gateway                               │
│  - Authentication & Authorization                            │
│  - Rate Limiting                                             │
│  - Request Validation                                        │
│  - Route to Marketplace Service                              │
└─────────────────────────────────────────────────────────────┘
                            │
                            ▼
┌─────────────────────────────────────────────────────────────┐
│              Marketplace Service                             │
│  - Listing CRUD Operations                                   │
│  - Transaction Processing                                    │
│  - Search Query Builder                                      │
│  - Price Analytics Calculator                                │
│  - Reputation Manager                                        │
└─────────────────────────────────────────────────────────────┘
           │              │              │
           ▼              ▼              ▼
┌──────────────┐  ┌──────────────┐  ┌──────────────┐
│  Inventory   │  │   Currency   │  │  Transport   │
│   Service    │  │   Service    │  │   Service    │
└──────────────┘  └──────────────┘  └──────────────┘
```

### Data Flow

**Creating a Listing:**

1. Client sends POST /api/marketplace/listings
2. API Gateway validates authentication and rate limits
3. Marketplace Service validates item ownership
4. Inventory Service locks items
5. Listing created in database
6. Search index updated
7. Response sent to client

**Purchasing an Item:**

1. Client sends POST /api/marketplace/transactions
2. API Gateway validates authentication
3. Marketplace Service starts transaction
4. Currency Service validates buyer funds
5. Inventory Service validates buyer space
6. Atomic transaction executes:
   - Deduct currency from buyer
   - Transfer items from seller to buyer
   - Credit seller (minus fees)
   - Update listing status
   - Create transaction record
7. Search index updated
8. WebSocket notification sent to seller
9. Response sent to buyer

## Database Schema

### Listings Table

```sql
CREATE TABLE marketplace_listings (
    id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    seller_id UUID NOT NULL REFERENCES players(id),
    item_id UUID NOT NULL REFERENCES items(id),
    quantity INTEGER NOT NULL CHECK (quantity > 0),
    price_per_unit DECIMAL(10, 2) NOT NULL CHECK (price_per_unit > 0),
    total_price DECIMAL(10, 2) GENERATED ALWAYS AS (quantity * price_per_unit) STORED,
    listing_fee DECIMAL(10, 2) NOT NULL,
    island VARCHAR(100) NOT NULL,
    expires_at TIMESTAMP WITH TIME ZONE NOT NULL,
    created_at TIMESTAMP WITH TIME ZONE DEFAULT NOW(),
    updated_at TIMESTAMP WITH TIME ZONE DEFAULT NOW(),
    status VARCHAR(20) NOT NULL DEFAULT 'ACTIVE' CHECK (status IN ('ACTIVE', 'SOLD', 'EXPIRED', 'CANCELLED')),
    views INTEGER DEFAULT 0,
    favorites INTEGER DEFAULT 0,
    CONSTRAINT valid_expiration CHECK (expires_at > created_at)
);

CREATE INDEX idx_listings_status ON marketplace_listings(status);
CREATE INDEX idx_listings_island_status ON marketplace_listings(island, status);
CREATE INDEX idx_listings_item_status ON marketplace_listings(item_id, status);
CREATE INDEX idx_listings_seller ON marketplace_listings(seller_id);
CREATE INDEX idx_listings_expires ON marketplace_listings(expires_at) WHERE status = 'ACTIVE';
```

### Transactions Table

```sql
CREATE TABLE marketplace_transactions (
    id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    listing_id UUID NOT NULL REFERENCES marketplace_listings(id),
    buyer_id UUID NOT NULL REFERENCES players(id),
    seller_id UUID NOT NULL REFERENCES players(id),
    item_id UUID NOT NULL REFERENCES items(id),
    quantity INTEGER NOT NULL,
    price_per_unit DECIMAL(10, 2) NOT NULL,
    total_price DECIMAL(10, 2) NOT NULL,
    marketplace_fee DECIMAL(10, 2) NOT NULL,
    transport_cost DECIMAL(10, 2) DEFAULT 0,
    status VARCHAR(20) NOT NULL DEFAULT 'PENDING' CHECK (status IN ('PENDING', 'COMPLETED', 'FAILED', 'REFUNDED')),
    completed_at TIMESTAMP WITH TIME ZONE,
    created_at TIMESTAMP WITH TIME ZONE DEFAULT NOW(),
    CONSTRAINT buyer_not_seller CHECK (buyer_id != seller_id)
);

CREATE INDEX idx_transactions_buyer ON marketplace_transactions(buyer_id, created_at);
CREATE INDEX idx_transactions_seller ON marketplace_transactions(seller_id, created_at);
CREATE INDEX idx_transactions_status ON marketplace_transactions(status, completed_at);
```

### Price History Table

```sql
CREATE TABLE marketplace_price_history (
    id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
    item_id UUID NOT NULL REFERENCES items(id),
    island VARCHAR(100) NOT NULL,
    price DECIMAL(10, 2) NOT NULL,
    quantity INTEGER NOT NULL,
    timestamp TIMESTAMP WITH TIME ZONE DEFAULT NOW(),
    source VARCHAR(20) NOT NULL CHECK (source IN ('SALE', 'LISTING'))
);

CREATE INDEX idx_price_history_item_time ON marketplace_price_history(item_id, timestamp DESC);
CREATE INDEX idx_price_history_island_item ON marketplace_price_history(island, item_id, timestamp DESC);
```

### Reputation Table

```sql
CREATE TABLE marketplace_reputation (
    seller_id UUID PRIMARY KEY REFERENCES players(id),
    total_transactions INTEGER DEFAULT 0,
    completed_transactions INTEGER DEFAULT 0,
    failed_transactions INTEGER DEFAULT 0,
    total_ratings INTEGER DEFAULT 0,
    sum_ratings INTEGER DEFAULT 0,
    average_rating DECIMAL(3, 2) GENERATED ALWAYS AS (
        CASE WHEN total_ratings > 0 THEN sum_ratings::DECIMAL / total_ratings ELSE 0 END
    ) STORED,
    trust_score DECIMAL(5, 2) DEFAULT 0,
    last_transaction_at TIMESTAMP WITH TIME ZONE,
    created_at TIMESTAMP WITH TIME ZONE DEFAULT NOW(),
    updated_at TIMESTAMP WITH TIME ZONE DEFAULT NOW()
);
```

## API Integration

### Service Dependencies

#### Inventory Service

**Required Methods:**

- `lockItems(playerId, itemId, quantity)` - Lock items for listing
- `unlockItems(playerId, itemId, quantity)` - Unlock cancelled listing items
- `transferItems(fromPlayerId, toPlayerId, itemId, quantity)` - Transfer purchased items
- `validateInventorySpace(playerId, itemId, quantity)` - Check if buyer has space

**Integration Example:**

```typescript
// Lock items when creating listing
async function createListing(sellerId: string, itemId: string, quantity: number): Promise<Listing> {
  const lockResult = await inventoryService.lockItems(sellerId, itemId, quantity);
  if (!lockResult.success) {
    throw new InsufficientInventoryError('Not enough items to list');
  }
  
  try {
    const listing = await db.listings.create({
      sellerId,
      itemId,
      quantity,
      // ... other fields
    });
    return listing;
  } catch (error) {
    // Rollback lock on failure
    await inventoryService.unlockItems(sellerId, itemId, quantity);
    throw error;
  }
}
```

#### Currency Service

**Required Methods:**

- `validateBalance(playerId, amount)` - Check if player has enough currency
- `deductCurrency(playerId, amount, reason)` - Remove currency from player
- `addCurrency(playerId, amount, reason)` - Add currency to player

**Integration Example:**

```typescript
// Process transaction payment
async function processPayment(
  buyerId: string,
  sellerId: string,
  totalAmount: number,
  fee: number
): Promise<void> {
  const hasBalance = await currencyService.validateBalance(buyerId, totalAmount);
  if (!hasBalance) {
    throw new InsufficientFundsError('Buyer does not have enough currency');
  }
  
  await db.transaction(async (trx) => {
    // Deduct from buyer
    await currencyService.deductCurrency(buyerId, totalAmount, 'Marketplace purchase', trx);
    
    // Credit seller (minus fee)
    const sellerAmount = totalAmount - fee;
    await currencyService.addCurrency(sellerId, sellerAmount, 'Marketplace sale', trx);
  });
}
```

#### Transport Service

**Required Methods:**

- `calculateTransportCost(fromIsland, toIsland, itemId, quantity)` - Calculate shipping cost
- `estimateDeliveryTime(fromIsland, toIsland)` - Estimate delivery duration
- `scheduleDelivery(buyerId, itemId, quantity, fromIsland, toIsland)` - Schedule item delivery

**Integration Example:**

```typescript
// Handle cross-island purchase
async function purchaseCrossIsland(
  buyerId: string,
  sellerId: string,
  listing: Listing
): Promise<Transaction> {
  const buyerIsland = await getPlayerIsland(buyerId);
  const transportCost = await transportService.calculateTransportCost(
    listing.island,
    buyerIsland,
    listing.itemId,
    listing.quantity
  );
  
  const totalCost = listing.totalPrice + transportCost;
  
  // Process payment
  await processPayment(buyerId, sellerId, totalCost, listing.totalPrice * 0.05);
  
  // Schedule delivery instead of instant transfer
  await transportService.scheduleDelivery(
    buyerId,
    listing.itemId,
    listing.quantity,
    listing.island,
    buyerIsland
  );
  
  return createTransaction(/* ... */);
}
```

### Search Integration

#### Elasticsearch Configuration

```json
{
  "mappings": {
    "properties": {
      "id": { "type": "keyword" },
      "sellerId": { "type": "keyword" },
      "sellerName": { "type": "text" },
      "itemId": { "type": "keyword" },
      "itemName": { "type": "text", "analyzer": "standard" },
      "itemType": { "type": "keyword" },
      "category": { "type": "keyword" },
      "quantity": { "type": "integer" },
      "pricePerUnit": { "type": "scaled_float", "scaling_factor": 100 },
      "totalPrice": { "type": "scaled_float", "scaling_factor": 100 },
      "island": { "type": "keyword" },
      "status": { "type": "keyword" },
      "expiresAt": { "type": "date" },
      "createdAt": { "type": "date" },
      "sellerRating": { "type": "scaled_float", "scaling_factor": 10 }
    }
  }
}
```

#### Search Query Builder

```typescript
// Build Elasticsearch query from filters
function buildSearchQuery(filters: MarketplaceFilters): object {
  const must = [
    { term: { status: 'ACTIVE' } }
  ];
  
  if (filters.category) {
    must.push({ term: { category: filters.category } });
  }
  
  if (filters.minPrice || filters.maxPrice) {
    must.push({
      range: {
        pricePerUnit: {
          gte: filters.minPrice || 0,
          lte: filters.maxPrice || Number.MAX_SAFE_INTEGER
        }
      }
    });
  }
  
  if (filters.island) {
    must.push({ term: { island: filters.island } });
  }
  
  if (filters.minRating) {
    must.push({
      range: {
        sellerRating: { gte: filters.minRating }
      }
    });
  }
  
  return {
    query: { bool: { must } },
    sort: buildSortClause(filters.sort),
    from: (filters.page - 1) * filters.limit,
    size: filters.limit
  };
}
```

## Transaction Processing

### Atomic Transaction Pattern

All marketplace transactions must be atomic to prevent:

- Duplicate purchases
- Currency loss without item transfer
- Item duplication
- Race conditions

**Implementation:**

```typescript
async function executePurchase(buyerId: string, listingId: string, quantity: number): Promise<Transaction> {
  return await db.transaction(async (trx) => {
    // 1. Lock listing for update (prevents concurrent purchases)
    const listing = await trx.listings
      .where({ id: listingId, status: 'ACTIVE' })
      .forUpdate()
      .first();
    
    if (!listing) {
      throw new ListingNotFoundError('Listing not available');
    }
    
    if (listing.quantity < quantity) {
      throw new InsufficientQuantityError('Not enough items available');
    }
    
    // 2. Validate buyer has funds
    const totalCost = listing.pricePerUnit * quantity;
    const hasFunds = await currencyService.validateBalance(buyerId, totalCost, trx);
    if (!hasFunds) {
      throw new InsufficientFundsError('Not enough currency');
    }
    
    // 3. Validate buyer has inventory space
    const hasSpace = await inventoryService.validateSpace(buyerId, listing.itemId, quantity, trx);
    if (!hasSpace) {
      throw new InsufficientSpaceError('Not enough inventory space');
    }
    
    // 4. Process payment
    const marketplaceFee = totalCost * 0.05;
    await currencyService.deductCurrency(buyerId, totalCost, 'Marketplace purchase', trx);
    await currencyService.addCurrency(listing.sellerId, totalCost - marketplaceFee, 'Marketplace sale', trx);
    
    // 5. Transfer items
    await inventoryService.unlockItems(listing.sellerId, listing.itemId, quantity, trx);
    await inventoryService.transferItems(listing.sellerId, buyerId, listing.itemId, quantity, trx);
    
    // 6. Update listing
    const remainingQuantity = listing.quantity - quantity;
    if (remainingQuantity === 0) {
      await trx.listings.where({ id: listingId }).update({ status: 'SOLD' });
    } else {
      await trx.listings.where({ id: listingId }).update({ quantity: remainingQuantity });
    }
    
    // 7. Create transaction record
    const transaction = await trx.transactions.insert({
      listingId,
      buyerId,
      sellerId: listing.sellerId,
      itemId: listing.itemId,
      quantity,
      pricePerUnit: listing.pricePerUnit,
      totalPrice: totalCost,
      marketplaceFee,
      status: 'COMPLETED',
      completedAt: new Date()
    });
    
    // 8. Update price history
    await trx.priceHistory.insert({
      itemId: listing.itemId,
      island: listing.island,
      price: listing.pricePerUnit,
      quantity,
      source: 'SALE'
    });
    
    // 9. Update seller reputation
    await trx.reputation
      .where({ sellerId: listing.sellerId })
      .increment('completed_transactions', 1)
      .increment('total_transactions', 1);
    
    return transaction;
  });
}
```

### Error Handling

Implement proper rollback on failures:

```typescript
try {
  const transaction = await executePurchase(buyerId, listingId, quantity);
  
  // Post-transaction actions (outside atomic transaction)
  await searchIndex.updateListing(listingId);
  await websocket.notifySeller(transaction.sellerId, transaction);
  
  return transaction;
} catch (error) {
  if (error instanceof InsufficientFundsError) {
    return { success: false, error: 'INSUFFICIENT_FUNDS', message: error.message };
  } else if (error instanceof ListingNotFoundError) {
    return { success: false, error: 'LISTING_NOT_FOUND', message: error.message };
  } else {
    logger.error('Transaction failed', { error, buyerId, listingId });
    return { success: false, error: 'INTERNAL_ERROR', message: 'Transaction failed, please try again' };
  }
}
```

## Real-Time Updates

### WebSocket Integration

Implement WebSocket connections for real-time marketplace updates:

**Server-Side:**

```typescript
// Marketplace event emitter
class MarketplaceEvents {
  async emitListingCreated(listing: Listing) {
    await websocket.broadcast(`marketplace:${listing.island}`, {
      type: 'LISTING_CREATED',
      data: listing
    });
  }
  
  async emitListingSold(transaction: Transaction) {
    // Notify seller
    await websocket.sendToUser(transaction.sellerId, {
      type: 'LISTING_SOLD',
      data: transaction
    });
  }
  
  async emitPriceChange(itemId: string, island: string, newPrice: number) {
    await websocket.broadcast(`marketplace:prices:${itemId}`, {
      type: 'PRICE_UPDATE',
      data: { itemId, island, price: newPrice }
    });
  }
}
```

**Client-Side:**

```typescript
// Subscribe to marketplace updates
const ws = new WebSocket('wss://api.bluemarble.design/ws');

ws.on('message', (event) => {
  const message = JSON.parse(event.data);
  
  switch (message.type) {
    case 'LISTING_CREATED':
      updateMarketplaceUI(message.data);
      break;
    case 'LISTING_SOLD':
      showNotification('Your item sold!', message.data);
      break;
    case 'PRICE_UPDATE':
      updatePriceDisplay(message.data);
      break;
  }
});
```

## Performance Optimization

### Caching Strategy

Implement multi-level caching:

```typescript
// Redis cache for hot data
class MarketplaceCache {
  // Cache popular searches (30 second TTL)
  async cacheSearchResults(query: string, results: Listing[]): Promise<void> {
    await redis.setex(`search:${hashQuery(query)}`, 30, JSON.stringify(results));
  }
  
  // Cache price data (5 minute TTL)
  async cachePriceData(itemId: string, data: PriceData): Promise<void> {
    await redis.setex(`prices:${itemId}`, 300, JSON.stringify(data));
  }
  
  // Cache seller reputation (10 minute TTL)
  async cacheReputation(sellerId: string, reputation: Reputation): Promise<void> {
    await redis.setex(`reputation:${sellerId}`, 600, JSON.stringify(reputation));
  }
}
```

### Database Optimization

Use appropriate indexes and queries:

```typescript
// Efficient listing query with pagination
async function getActiveListings(filters: MarketplaceFilters): Promise<Listing[]> {
  let query = db.listings
    .where({ status: 'ACTIVE' })
    .limit(filters.limit)
    .offset((filters.page - 1) * filters.limit);
  
  if (filters.island) {
    query = query.where({ island: filters.island });
  }
  
  if (filters.category) {
    query = query.where({ category: filters.category });
  }
  
  // Use covering index for sorting
  switch (filters.sort) {
    case 'price_asc':
      query = query.orderBy('price_per_unit', 'asc');
      break;
    case 'price_desc':
      query = query.orderBy('price_per_unit', 'desc');
      break;
    case 'newest':
      query = query.orderBy('created_at', 'desc');
      break;
  }
  
  return await query;
}
```

## Testing

### Unit Tests

```typescript
describe('Marketplace Service', () => {
  describe('createListing', () => {
    it('should create listing and lock items', async () => {
      const listing = await marketplaceService.createListing(
        sellerId,
        itemId,
        100,
        5.50,
        '7d'
      );
      
      expect(listing.status).toBe('ACTIVE');
      expect(inventoryService.lockItems).toHaveBeenCalledWith(sellerId, itemId, 100);
    });
    
    it('should fail if insufficient inventory', async () => {
      inventoryService.lockItems.mockResolvedValue({ success: false });
      
      await expect(
        marketplaceService.createListing(sellerId, itemId, 100, 5.50, '7d')
      ).rejects.toThrow(InsufficientInventoryError);
    });
  });
  
  describe('executePurchase', () => {
    it('should complete atomic transaction', async () => {
      const transaction = await marketplaceService.executePurchase(buyerId, listingId, 50);
      
      expect(transaction.status).toBe('COMPLETED');
      expect(currencyService.deductCurrency).toHaveBeenCalled();
      expect(currencyService.addCurrency).toHaveBeenCalled();
      expect(inventoryService.transferItems).toHaveBeenCalled();
    });
    
    it('should rollback on insufficient funds', async () => {
      currencyService.validateBalance.mockResolvedValue(false);
      
      await expect(
        marketplaceService.executePurchase(buyerId, listingId, 50)
      ).rejects.toThrow(InsufficientFundsError);
      
      // Verify no side effects
      expect(inventoryService.transferItems).not.toHaveBeenCalled();
    });
  });
});
```

### Integration Tests

```typescript
describe('Marketplace Integration', () => {
  it('should handle concurrent purchase attempts', async () => {
    // Create listing with quantity 1
    const listing = await createTestListing({ quantity: 1 });
    
    // Two buyers attempt to purchase simultaneously
    const results = await Promise.allSettled([
      marketplaceService.executePurchase(buyer1Id, listing.id, 1),
      marketplaceService.executePurchase(buyer2Id, listing.id, 1)
    ]);
    
    // One should succeed, one should fail
    const succeeded = results.filter(r => r.status === 'fulfilled');
    const failed = results.filter(r => r.status === 'rejected');
    
    expect(succeeded.length).toBe(1);
    expect(failed.length).toBe(1);
  });
});
```

## Security Considerations

### Authorization Checks

Always verify permissions:

```typescript
async function updateListing(listingId: string, userId: string, updates: Partial<Listing>): Promise<Listing> {
  const listing = await db.listings.findOne({ id: listingId });
  
  if (!listing) {
    throw new NotFoundError('Listing not found');
  }
  
  // Verify user owns the listing
  if (listing.sellerId !== userId) {
    throw new ForbiddenError('You can only update your own listings');
  }
  
  return await db.listings.update(listingId, updates);
}
```

### Input Validation

Validate all inputs:

```typescript
function validateListingInput(input: CreateListingInput): void {
  if (input.quantity <= 0) {
    throw new ValidationError('Quantity must be greater than 0');
  }
  
  if (input.pricePerUnit <= 0) {
    throw new ValidationError('Price must be greater than 0');
  }
  
  if (input.pricePerUnit > 1000000) {
    throw new ValidationError('Price exceeds maximum allowed');
  }
  
  if (!['1d', '3d', '7d'].includes(input.duration)) {
    throw new ValidationError('Invalid duration');
  }
}
```

### Rate Limiting

Implement proper rate limiting:

```typescript
class RateLimiter {
  async checkLimit(userId: string, action: string): Promise<boolean> {
    const key = `ratelimit:${userId}:${action}`;
    const count = await redis.incr(key);
    
    if (count === 1) {
      await redis.expire(key, 60); // 1 minute window
    }
    
    const limit = this.getLimitForAction(action);
    return count <= limit;
  }
  
  private getLimitForAction(action: string): number {
    const limits = {
      'create_listing': 20,
      'search': 100,
      'purchase': 50
    };
    return limits[action] || 10;
  }
}
```

## Monitoring and Logging

### Key Metrics

Track these metrics:

- Listing creation rate
- Transaction success/failure rate
- Average transaction time
- Search query performance
- Cache hit rate
- WebSocket connection count

### Logging

```typescript
// Log important events
logger.info('Listing created', {
  listingId: listing.id,
  sellerId: listing.sellerId,
  itemId: listing.itemId,
  price: listing.pricePerUnit
});

logger.info('Transaction completed', {
  transactionId: transaction.id,
  buyerId: transaction.buyerId,
  sellerId: transaction.sellerId,
  amount: transaction.totalPrice,
  duration: performance.now() - startTime
});

// Log errors with context
logger.error('Transaction failed', {
  error: error.message,
  stack: error.stack,
  buyerId,
  listingId,
  attemptNumber
});
```

## Deployment

### Database Migrations

Run migrations in order:

```bash
# 001 - Create listings table
npm run migrate:up 001_create_listings_table

# 002 - Create transactions table
npm run migrate:up 002_create_transactions_table

# 003 - Create price history table
npm run migrate:up 003_create_price_history_table

# 004 - Create reputation table
npm run migrate:up 004_create_reputation_table
```

### Search Index Setup

```bash
# Create Elasticsearch index
curl -X PUT "localhost:9200/marketplace_listings" -H 'Content-Type: application/json' -d @marketplace_index.json

# Bulk index existing listings
npm run index:rebuild
```

### Environment Configuration

```env
# Marketplace Service
MARKETPLACE_DB_HOST=localhost
MARKETPLACE_DB_PORT=5432
MARKETPLACE_DB_NAME=bluemarble_marketplace
MARKETPLACE_REDIS_HOST=localhost
MARKETPLACE_REDIS_PORT=6379
MARKETPLACE_ES_HOST=localhost:9200
MARKETPLACE_WS_PORT=8080

# Feature Flags
MARKETPLACE_ENABLED=true
MARKETPLACE_CROSS_ISLAND_ENABLED=true
MARKETPLACE_REPUTATION_ENABLED=true

# Rate Limits
MARKETPLACE_RATE_LIMIT_STANDARD=100
MARKETPLACE_RATE_LIMIT_PREMIUM=500
```

## Related Documentation

- [Marketplace Feature Specification](../../roadmap/tasks/player-trading-marketplace.md)
- [Marketplace API](api-marketplace.md)
- [Marketplace Usage Guide](../gameplay/marketplace-usage-guide.md)
- [Economy Systems](economy-systems.md)
- [Database Schema Design](database-schema-design.md)
- [System Architecture](system-architecture-design.md)

## Support

For technical questions or issues:

- Join #marketplace-dev channel in Discord
- Review API documentation at <https://docs.bluemarble.design>
- Submit issues in GitHub repository
- Contact <backend-team@bluemarble.design>
