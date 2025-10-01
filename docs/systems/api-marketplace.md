# Player Trading Marketplace API Specification

**Document Type:** API Specification  
**Version:** 1.0  
**Author:** BlueMarble Design Team  
**Date:** 2024-12-29  
**Status:** Draft  
**Related Specifications:**

- [Feature Specification](../../roadmap/tasks/player-trading-marketplace.md)
- [Economy Systems](economy-systems.md)
- [Database Schema Design](database-schema-design.md)

## Overview

The Player Trading Marketplace API provides programmatic access to the marketplace trading system, enabling players to
list items for sale, browse available listings, complete transactions, and access market analytics. This API is designed
for high performance, scalability, and security to support the player-driven economy in BlueMarble.

### Key Features

- Item listing creation, modification, and deletion
- Advanced search and filtering of marketplace listings
- Secure transaction processing with atomic guarantees
- Historical price data and market analytics
- Seller reputation and rating system
- Cross-island trade logistics integration

### Design Goals

- High performance for real-time marketplace browsing (< 500ms response time)
- Scalability for 10,000 concurrent listings and 1,000 transactions per minute
- Atomic transactions with full rollback capabilities
- Clear and consistent RESTful interface

## Base Configuration

**Base URL:** `https://api.bluemarble.design/v1/marketplace`  
**Authentication:** Required - Bearer token (JWT)  
**Content-Type:** `application/json`  
**Rate Limit:** 100 requests per minute for standard users, 500 for premium

### Required Headers

```http
Authorization: Bearer {token}
Content-Type: application/json
X-Request-ID: {uuid}
```

### Optional Headers

```http
X-API-Version: {minor-version}
Accept-Language: {locale}
```

## Core Endpoints

### 1. Marketplace Listings

#### List All Listings

**Endpoint:** `GET /api/marketplace/listings`

**Description:** Retrieve marketplace listings with optional filtering and pagination.

**Query Parameters:**

| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| category | string | No | Item category filter (e.g., "Resources", "Tools", "Crafted Goods") |
| minPrice | decimal | No | Minimum price filter |
| maxPrice | decimal | No | Maximum price filter |
| island | string | No | Filter by island location |
| sellerId | uuid | No | Filter by specific seller |
| status | string | No | Filter by status (ACTIVE, SOLD, EXPIRED, CANCELLED) |
| sort | string | No | Sort field: "price_asc", "price_desc", "newest", "ending_soon" |
| page | integer | No | Page number (default: 1) |
| limit | integer | No | Items per page (default: 20, max: 100) |

**Response 200 OK:**

```json
{
  "success": true,
  "data": {
    "listings": [
      {
        "id": "550e8400-e29b-41d4-a716-446655440000",
        "sellerId": "123e4567-e89b-12d3-a456-426614174000",
        "sellerName": "PlayerName",
        "itemId": "7c9e6679-7425-40de-944b-e07fc1f90ae7",
        "itemName": "Iron Ore",
        "itemType": "Resource",
        "quantity": 100,
        "pricePerUnit": 5.50,
        "totalPrice": 550.00,
        "listingFee": 5.50,
        "island": "Aethermoor",
        "expiresAt": "2024-12-30T12:00:00Z",
        "createdAt": "2024-12-29T12:00:00Z",
        "status": "ACTIVE",
        "views": 45,
        "favorites": 3
      }
    ],
    "pagination": {
      "page": 1,
      "limit": 20,
      "total": 150,
      "pages": 8
    }
  }
}
```

#### Get Listing Details

**Endpoint:** `GET /api/marketplace/listings/{id}`

**Description:** Get detailed information about a specific listing.

**Path Parameters:**

| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| id | uuid | Yes | Listing unique identifier |

**Response 200 OK:**

```json
{
  "success": true,
  "data": {
    "id": "550e8400-e29b-41d4-a716-446655440000",
    "sellerId": "123e4567-e89b-12d3-a456-426614174000",
    "seller": {
      "id": "123e4567-e89b-12d3-a456-426614174000",
      "name": "PlayerName",
      "reputation": {
        "rating": 4.7,
        "totalTransactions": 127,
        "completedTransactions": 125
      }
    },
    "item": {
      "id": "7c9e6679-7425-40de-944b-e07fc1f90ae7",
      "name": "Iron Ore",
      "type": "Resource",
      "description": "High-quality iron ore extracted from mountain deposits",
      "quality": "Excellent",
      "attributes": {
        "purity": 95,
        "weight": 1.0
      }
    },
    "quantity": 100,
    "pricePerUnit": 5.50,
    "totalPrice": 550.00,
    "listingFee": 5.50,
    "island": "Aethermoor",
    "expiresAt": "2024-12-30T12:00:00Z",
    "createdAt": "2024-12-29T12:00:00Z",
    "status": "ACTIVE",
    "views": 45,
    "favorites": 3,
    "priceHistory": {
      "average7Day": 5.75,
      "average30Day": 6.20,
      "recentSales": 15
    }
  }
}
```

**Response 404 Not Found:**

```json
{
  "success": false,
  "error": {
    "code": "LISTING_NOT_FOUND",
    "message": "The requested listing does not exist"
  }
}
```

#### Create Listing

**Endpoint:** `POST /api/marketplace/listings`

**Description:** Create a new marketplace listing.

**Request Body:**

```json
{
  "itemId": "7c9e6679-7425-40de-944b-e07fc1f90ae7",
  "quantity": 100,
  "pricePerUnit": 5.50,
  "duration": "7d"
}
```

**Response 201 Created:**

```json
{
  "success": true,
  "data": {
    "id": "550e8400-e29b-41d4-a716-446655440000",
    "itemId": "7c9e6679-7425-40de-944b-e07fc1f90ae7",
    "quantity": 100,
    "pricePerUnit": 5.50,
    "totalPrice": 550.00,
    "listingFee": 5.50,
    "island": "Aethermoor",
    "expiresAt": "2025-01-05T12:00:00Z",
    "createdAt": "2024-12-29T12:00:00Z",
    "status": "ACTIVE"
  }
}
```

**Response 400 Bad Request:**

```json
{
  "success": false,
  "error": {
    "code": "INVALID_REQUEST",
    "message": "Invalid listing parameters",
    "details": {
      "pricePerUnit": "Must be greater than 0",
      "quantity": "Must be greater than 0"
    }
  }
}
```

**Response 422 Unprocessable Entity:**

```json
{
  "success": false,
  "error": {
    "code": "INSUFFICIENT_INVENTORY",
    "message": "Player does not have enough items in inventory"
  }
}
```

#### Update Listing

**Endpoint:** `PATCH /api/marketplace/listings/{id}`

**Description:** Update price or quantity of an existing listing.

**Path Parameters:**

| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| id | uuid | Yes | Listing unique identifier |

**Request Body:**

```json
{
  "pricePerUnit": 5.00,
  "quantity": 80
}
```

**Response 200 OK:**

```json
{
  "success": true,
  "data": {
    "id": "550e8400-e29b-41d4-a716-446655440000",
    "pricePerUnit": 5.00,
    "quantity": 80,
    "totalPrice": 400.00,
    "updatedAt": "2024-12-29T14:00:00Z"
  }
}
```

#### Delete Listing

**Endpoint:** `DELETE /api/marketplace/listings/{id}`

**Description:** Cancel and remove a listing, returning items to seller inventory.

**Path Parameters:**

| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| id | uuid | Yes | Listing unique identifier |

**Response 200 OK:**

```json
{
  "success": true,
  "message": "Listing cancelled and items returned to inventory"
}
```

**Response 403 Forbidden:**

```json
{
  "success": false,
  "error": {
    "code": "UNAUTHORIZED",
    "message": "You can only delete your own listings"
  }
}
```

### 2. Transactions

#### Purchase Item

**Endpoint:** `POST /api/marketplace/transactions`

**Description:** Purchase an item from the marketplace.

**Request Body:**

```json
{
  "listingId": "550e8400-e29b-41d4-a716-446655440000",
  "quantity": 50
}
```

**Response 201 Created:**

```json
{
  "success": true,
  "data": {
    "transactionId": "a1b2c3d4-e5f6-4789-0123-456789abcdef",
    "listingId": "550e8400-e29b-41d4-a716-446655440000",
    "buyerId": "789e0123-e89b-12d3-a456-426614174000",
    "sellerId": "123e4567-e89b-12d3-a456-426614174000",
    "itemId": "7c9e6679-7425-40de-944b-e07fc1f90ae7",
    "quantity": 50,
    "pricePerUnit": 5.50,
    "totalPrice": 275.00,
    "marketplaceFee": 13.75,
    "transportCost": 0.00,
    "finalCost": 288.75,
    "status": "COMPLETED",
    "completedAt": "2024-12-29T15:30:00Z"
  }
}
```

**Response 402 Payment Required:**

```json
{
  "success": false,
  "error": {
    "code": "INSUFFICIENT_FUNDS",
    "message": "Player does not have enough currency",
    "details": {
      "required": 288.75,
      "available": 200.00
    }
  }
}
```

**Response 409 Conflict:**

```json
{
  "success": false,
  "error": {
    "code": "LISTING_UNAVAILABLE",
    "message": "Item is no longer available for purchase"
  }
}
```

#### Get Transaction History

**Endpoint:** `GET /api/marketplace/transactions`

**Description:** Get player's transaction history.

**Query Parameters:**

| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| type | string | No | Filter by type: "buy", "sell", "all" (default: "all") |
| status | string | No | Filter by status: "PENDING", "COMPLETED", "FAILED", "REFUNDED" |
| page | integer | No | Page number (default: 1) |
| limit | integer | No | Items per page (default: 20, max: 100) |

**Response 200 OK:**

```json
{
  "success": true,
  "data": {
    "transactions": [
      {
        "id": "a1b2c3d4-e5f6-4789-0123-456789abcdef",
        "type": "buy",
        "itemName": "Iron Ore",
        "quantity": 50,
        "totalPrice": 275.00,
        "otherParty": "SellerName",
        "status": "COMPLETED",
        "completedAt": "2024-12-29T15:30:00Z"
      }
    ],
    "pagination": {
      "page": 1,
      "limit": 20,
      "total": 45,
      "pages": 3
    }
  }
}
```

### 3. Market Analytics

#### Get Price History

**Endpoint:** `GET /api/marketplace/analytics/prices/{itemId}`

**Description:** Get historical price data for a specific item.

**Path Parameters:**

| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| itemId | uuid | Yes | Item unique identifier |

**Query Parameters:**

| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| timeframe | string | No | "7d", "30d", "90d" (default: "7d") |
| island | string | No | Filter by specific island |

**Response 200 OK:**

```json
{
  "success": true,
  "data": {
    "itemId": "7c9e6679-7425-40de-944b-e07fc1f90ae7",
    "itemName": "Iron Ore",
    "timeframe": "7d",
    "statistics": {
      "averagePrice": 5.75,
      "minPrice": 4.50,
      "maxPrice": 7.00,
      "medianPrice": 5.80,
      "totalVolume": 2500
    },
    "pricePoints": [
      {
        "timestamp": "2024-12-22T00:00:00Z",
        "price": 5.50,
        "volume": 350
      },
      {
        "timestamp": "2024-12-23T00:00:00Z",
        "price": 5.75,
        "volume": 420
      }
    ]
  }
}
```

#### Get Market Trends

**Endpoint:** `GET /api/marketplace/analytics/trends`

**Description:** Get overall market trend data and statistics.

**Query Parameters:**

| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| category | string | No | Filter by item category |
| timeframe | string | No | "7d", "30d", "90d" (default: "7d") |

**Response 200 OK:**

```json
{
  "success": true,
  "data": {
    "timeframe": "7d",
    "overview": {
      "totalListings": 8543,
      "totalTransactions": 3421,
      "totalVolume": 1250000.00,
      "averageTransactionValue": 365.43
    },
    "topItems": [
      {
        "itemId": "7c9e6679-7425-40de-944b-e07fc1f90ae7",
        "itemName": "Iron Ore",
        "category": "Resource",
        "averagePrice": 5.75,
        "volume": 2500,
        "priceChange": 5.2
      }
    ]
  }
}
```

### 4. Reputation System

#### Submit Seller Rating

**Endpoint:** `POST /api/marketplace/reputation/rate`

**Description:** Submit a rating and review for a seller after a transaction.

**Request Body:**

```json
{
  "transactionId": "a1b2c3d4-e5f6-4789-0123-456789abcdef",
  "rating": 5,
  "comment": "Great seller, fast delivery!"
}
```

**Response 201 Created:**

```json
{
  "success": true,
  "message": "Rating submitted successfully"
}
```

**Response 400 Bad Request:**

```json
{
  "success": false,
  "error": {
    "code": "INVALID_RATING",
    "message": "Rating must be between 1 and 5"
  }
}
```

#### Get Seller Reputation

**Endpoint:** `GET /api/marketplace/reputation/{sellerId}`

**Description:** Get reputation details for a specific seller.

**Path Parameters:**

| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| sellerId | uuid | Yes | Seller player unique identifier |

**Response 200 OK:**

```json
{
  "success": true,
  "data": {
    "sellerId": "123e4567-e89b-12d3-a456-426614174000",
    "reputation": {
      "rating": 4.7,
      "totalRatings": 125,
      "totalTransactions": 127,
      "completedTransactions": 125,
      "failedTransactions": 2,
      "trustScore": 92.5,
      "memberSince": "2024-06-15T00:00:00Z",
      "badges": ["Trusted Seller", "Fast Shipper"]
    },
    "recentReviews": [
      {
        "rating": 5,
        "comment": "Great seller, fast delivery!",
        "createdAt": "2024-12-29T15:45:00Z"
      }
    ]
  }
}
```

## Error Handling

### Standard Error Response Format

All errors follow a consistent JSON format:

```json
{
  "success": false,
  "error": {
    "code": "ERROR_CODE",
    "message": "Human-readable error message",
    "details": {
      "field": "Additional context or validation errors"
    }
  }
}
```

### Common Error Codes

| HTTP Status | Error Code | Description |
|-------------|------------|-------------|
| 400 | INVALID_REQUEST | Malformed request or invalid parameters |
| 401 | UNAUTHORIZED | Missing or invalid authentication token |
| 403 | FORBIDDEN | User lacks permission for this action |
| 404 | LISTING_NOT_FOUND | Requested listing does not exist |
| 404 | ITEM_NOT_FOUND | Requested item does not exist |
| 409 | LISTING_UNAVAILABLE | Item no longer available for purchase |
| 409 | DUPLICATE_LISTING | Item already listed on marketplace |
| 422 | INSUFFICIENT_INVENTORY | Not enough items in player inventory |
| 422 | INSUFFICIENT_FUNDS | Not enough currency for transaction |
| 422 | INSUFFICIENT_SPACE | Not enough inventory space for purchase |
| 429 | RATE_LIMIT_EXCEEDED | Too many requests, please slow down |
| 500 | INTERNAL_ERROR | Server error, please try again later |
| 503 | SERVICE_UNAVAILABLE | Marketplace temporarily unavailable |

## Rate Limiting

### Rate Limits

| User Type | Requests/Minute | Burst Limit | Concurrent Requests |
|-----------|-----------------|-------------|---------------------|
| Guest | 30 | 10 | 5 |
| Standard User | 100 | 30 | 10 |
| Premium User | 500 | 100 | 20 |
| Service Account | 1000 | 200 | 50 |

### Rate Limit Headers

Every API response includes rate limit information:

```http
X-RateLimit-Limit: 100
X-RateLimit-Remaining: 85
X-RateLimit-Reset: 1735387200
```

### Rate Limit Exceeded Response

**Response 429 Too Many Requests:**

```json
{
  "success": false,
  "error": {
    "code": "RATE_LIMIT_EXCEEDED",
    "message": "Rate limit exceeded",
    "retryAfter": 45,
    "limit": 100,
    "resetAt": "2024-12-29T12:01:00Z"
  }
}
```

## Authentication and Authorization

### Authentication Method

The Marketplace API uses JWT Bearer tokens for authentication. Tokens are obtained through the main authentication
service and must be included in all API requests.

**Example:**

```http
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

### Required Permissions

| Endpoint | Method | Required Permission |
|----------|--------|---------------------|
| `/listings` | GET | `marketplace:read` |
| `/listings` | POST | `marketplace:create` |
| `/listings/{id}` | GET | `marketplace:read` |
| `/listings/{id}` | PATCH | `marketplace:update` (own listings only) |
| `/listings/{id}` | DELETE | `marketplace:delete` (own listings only) |
| `/transactions` | POST | `marketplace:transact` |
| `/transactions` | GET | `marketplace:read` |
| `/analytics/*` | GET | `marketplace:read` |
| `/reputation/rate` | POST | `marketplace:rate` |

### Authorization Rules

- Users can only modify or delete their own listings
- Sellers cannot purchase from their own listings
- Rating requires completed transaction between buyer and seller
- Premium users have extended rate limits and additional features

## Data Models

### Listing Entity

```typescript
interface Listing {
  id: UUID;
  sellerId: UUID;
  itemId: UUID;
  quantity: number;
  pricePerUnit: number;
  totalPrice: number;
  listingFee: number;
  island: string;
  expiresAt: string; // ISO 8601 timestamp
  createdAt: string; // ISO 8601 timestamp
  updatedAt?: string; // ISO 8601 timestamp
  status: 'ACTIVE' | 'SOLD' | 'EXPIRED' | 'CANCELLED';
  views: number;
  favorites: number;
}
```

### Transaction Entity

```typescript
interface Transaction {
  id: UUID;
  listingId: UUID;
  buyerId: UUID;
  sellerId: UUID;
  itemId: UUID;
  quantity: number;
  pricePerUnit: number;
  totalPrice: number;
  marketplaceFee: number;
  transportCost: number;
  status: 'PENDING' | 'COMPLETED' | 'FAILED' | 'REFUNDED';
  completedAt?: string; // ISO 8601 timestamp
  createdAt: string; // ISO 8601 timestamp
}
```

### Reputation Entity

```typescript
interface SellerReputation {
  sellerId: UUID;
  rating: number; // 0-5, decimal
  totalRatings: number;
  totalTransactions: number;
  completedTransactions: number;
  failedTransactions: number;
  trustScore: number; // 0-100
  memberSince: string; // ISO 8601 timestamp
  badges: string[];
}
```

## Performance Considerations

### Response Time Targets

| Endpoint Type | Target Response Time | 95th Percentile |
|--------------|---------------------|----------------|
| Listing Search | < 500ms | < 750ms |
| Single Listing | < 200ms | < 300ms |
| Transaction | < 2000ms | < 3000ms |
| Analytics | < 1000ms | < 1500ms |

### Caching Strategy

- **Listing Search Results**: 30-second cache for popular queries
- **Item Price Data**: 5-minute cache for price history
- **Seller Reputation**: 10-minute cache for reputation scores
- **Market Trends**: 1-hour cache for overall statistics

### Pagination Best Practices

- Use cursor-based pagination for large datasets
- Default page size: 20 items
- Maximum page size: 100 items
- Include `hasMore` indicator in responses

## SDK Examples

### JavaScript/TypeScript

```typescript
import { MarketplaceClient } from '@bluemarble/marketplace-sdk';

const client = new MarketplaceClient({
  apiKey: 'your-api-key',
  baseUrl: 'https://api.bluemarble.design/v1'
});

// List items
const listings = await client.listings.list({
  category: 'Resources',
  minPrice: 1.0,
  maxPrice: 10.0,
  page: 1
});

// Create listing
const newListing = await client.listings.create({
  itemId: 'item-uuid',
  quantity: 100,
  pricePerUnit: 5.50,
  duration: '7d'
});

// Purchase item
const transaction = await client.transactions.create({
  listingId: 'listing-uuid',
  quantity: 50
});
```

### Python

```python
from bluemarble_marketplace import MarketplaceClient

client = MarketplaceClient(
    api_key='your-api-key',
    base_url='https://api.bluemarble.design/v1'
)

# List items
listings = client.listings.list(
    category='Resources',
    min_price=1.0,
    max_price=10.0,
    page=1
)

# Create listing
new_listing = client.listings.create(
    item_id='item-uuid',
    quantity=100,
    price_per_unit=5.50,
    duration='7d'
)

# Purchase item
transaction = client.transactions.create(
    listing_id='listing-uuid',
    quantity=50
)
```

## Testing

### Test Scenarios

1. **Create and List Item**
   - Verify item is locked in inventory
   - Confirm listing appears in search results
   - Validate listing fee calculation

2. **Purchase Item - Same Island**
   - Verify currency deduction
   - Confirm item transfer to buyer
   - Check seller receives payment minus fees

3. **Purchase Item - Cross Island**
   - Verify transport cost calculation
   - Check delivery time estimation
   - Confirm delayed item delivery

4. **Concurrent Purchase Conflict**
   - Two buyers attempt to purchase last item
   - First succeeds, second receives error
   - No currency lost from failed purchase

5. **Rate Limiting**
   - Exceed rate limit threshold
   - Verify 429 response with retry information
   - Confirm reset timer works correctly

### Performance Testing

- Load test with 1,000 concurrent users browsing
- Stress test transaction processing at 100 TPS
- Verify search query performance under load
- Test cache invalidation and propagation

## Migration Guide

### From Direct Trading to Marketplace

**Breaking Changes:**

- Direct trade API endpoints are deprecated
- Currency transfer now includes marketplace fees
- Item locking mechanism changed to support listings

**Deprecated Features:**

- Direct P2P trade UI - Use marketplace listings instead
- Trade chat system - Integrated into marketplace interface

**Migration Steps:**

1. Update client applications to use marketplace endpoints
2. Migrate existing trade data to marketplace format
3. Update currency calculations to include fees
4. Test transaction flow with new API

## References

**Related Documentation:**

- [Feature Specification](../../roadmap/tasks/player-trading-marketplace.md)
- [Economy Systems](economy-systems.md)
- [Database Schema Design](database-schema-design.md)
- [Authentication Guide](api-specifications.md)

**Standards:**

- [RESTful API Best Practices](https://restfulapi.net/)
- [OpenAPI Specification](https://spec.openapis.org/oas/v3.1.0)
- [JSON:API Specification](https://jsonapi.org/)

## Version History

| Version | Date | Author | Changes |
|---------|------|--------|---------|
| 1.0 | 2024-12-29 | BlueMarble Design Team | Initial API specification document |

## Approval Checklist

- [x] API endpoints reviewed and approved
- [x] Request/response formats documented
- [x] Authentication and authorization specified
- [x] Error handling documented
- [x] Rate limiting configured
- [x] Performance targets defined
- [ ] Security review completed
- [ ] SDK examples provided
- [ ] Test cases defined
- [x] Documentation complete
- [ ] Stakeholder approval received
