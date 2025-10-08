# BlueMarble Taxonomy System API

**Version:** 1.0  
**Base URL:** `/v1/taxonomy`  
**Authentication:** Required (Bearer Token)

## Overview

The Taxonomy API provides endpoints for managing and querying hierarchical classification systems (taxa) across various game domains including factions, species, geological features, items, and achievements.

## Endpoints

### Taxa Management

#### Get Taxon by ID

Retrieve a specific taxon by its identifier.

```http
GET /v1/taxonomy/taxa/{taxon_id}
```

**Path Parameters:**

| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| taxon_id | string | Yes | Unique taxon identifier |

**Response:** `200 OK`

```json
{
  "taxon_id": "faction-geologist-guild",
  "domain": "faction",
  "name": "International Geological Survey",
  "scientific_name": "IGS",
  "parent_taxon_id": "faction-major",
  "rank": 2,
  "rank_name": null,
  "description": "A major international organization dedicated to geological research",
  "attributes": {
    "specialization": "geology",
    "founded": "2045",
    "headquarters": "Geneva"
  },
  "is_deprecated": false,
  "created_at": "2025-01-19T10:00:00Z",
  "updated_at": "2025-01-19T10:00:00Z"
}
```

**Error Responses:**

- `404 Not Found` - Taxon does not exist
- `401 Unauthorized` - Invalid or missing authentication token

---

#### List Taxa

List taxa with optional filtering.

```http
GET /v1/taxonomy/taxa
```

**Query Parameters:**

| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| domain | string | Yes | Taxonomy domain (faction, species, geological, item, quest, achievement) |
| parent_id | string | No | Filter by parent taxon ID |
| rank | integer | No | Filter by hierarchical rank |
| include_deprecated | boolean | No | Include deprecated taxa (default: false) |
| page | integer | No | Page number for pagination (default: 1) |
| limit | integer | No | Results per page (default: 50, max: 100) |

**Example Request:**

```http
GET /v1/taxonomy/taxa?domain=faction&parent_id=faction-root&limit=20
```

**Response:** `200 OK`

```json
{
  "data": [
    {
      "taxon_id": "faction-major",
      "domain": "faction",
      "name": "Major Factions",
      "rank": 1,
      "parent_taxon_id": "faction-root",
      "child_count": 5
    },
    {
      "taxon_id": "faction-minor",
      "domain": "faction",
      "name": "Minor Factions",
      "rank": 1,
      "parent_taxon_id": "faction-root",
      "child_count": 12
    }
  ],
  "pagination": {
    "page": 1,
    "limit": 20,
    "total": 2,
    "total_pages": 1
  }
}
```

---

#### Create Taxon

Create a new taxon.

```http
POST /v1/taxonomy/taxa
```

**Request Body:**

```json
{
  "taxon_id": "faction-arctic-surveyors",
  "domain": "faction",
  "name": "Arctic Survey Team",
  "scientific_name": "AST",
  "parent_taxon_id": "faction-igs-field",
  "rank": 4,
  "rank_name": "Survey Team",
  "description": "Specialized team for arctic geological surveys",
  "attributes": {
    "region": "arctic",
    "specialization": "ice_core_sampling",
    "team_size": 15
  }
}
```

**Response:** `201 Created`

```json
{
  "taxon_id": "faction-arctic-surveyors",
  "domain": "faction",
  "name": "Arctic Survey Team",
  "parent_taxon_id": "faction-igs-field",
  "rank": 4,
  "created_at": "2025-01-19T12:00:00Z"
}
```

**Error Responses:**

- `400 Bad Request` - Invalid taxon data
- `409 Conflict` - Taxon ID already exists
- `403 Forbidden` - Insufficient permissions

---

#### Update Taxon

Update an existing taxon.

```http
PATCH /v1/taxonomy/taxa/{taxon_id}
```

**Request Body:** (all fields optional)

```json
{
  "name": "Arctic & Antarctic Survey Team",
  "description": "Expanded to cover both polar regions",
  "attributes": {
    "region": "polar",
    "team_size": 25
  }
}
```

**Response:** `200 OK`

```json
{
  "taxon_id": "faction-arctic-surveyors",
  "name": "Arctic & Antarctic Survey Team",
  "updated_at": "2025-01-19T14:00:00Z"
}
```

---

#### Delete Taxon

Mark a taxon as deprecated (soft delete).

```http
DELETE /v1/taxonomy/taxa/{taxon_id}
```

**Query Parameters:**

| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| cascade | boolean | No | Also deprecate all descendants (default: false) |

**Response:** `204 No Content`

**Error Responses:**

- `404 Not Found` - Taxon does not exist
- `409 Conflict` - Taxon has active references

---

### Hierarchy Navigation

#### Get Taxon Hierarchy

Retrieve the hierarchical path for a taxon.

```http
GET /v1/taxonomy/taxa/{taxon_id}/hierarchy
```

**Query Parameters:**

| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| direction | string | No | "up" for ancestors, "down" for descendants (default: "up") |
| max_depth | integer | No | Maximum levels to traverse (default: unlimited) |
| include_self | boolean | No | Include the queried taxon (default: true) |

**Example Request:**

```http
GET /v1/taxonomy/taxa/faction-igs-field/hierarchy?direction=up
```

**Response:** `200 OK`

```json
{
  "taxon_id": "faction-igs-field",
  "hierarchy": [
    {
      "taxon_id": "faction-root",
      "name": "All Factions",
      "rank": 0,
      "depth": 3
    },
    {
      "taxon_id": "faction-major",
      "name": "Major Factions",
      "rank": 1,
      "depth": 2
    },
    {
      "taxon_id": "faction-geologist-guild",
      "name": "International Geological Survey",
      "rank": 2,
      "depth": 1
    },
    {
      "taxon_id": "faction-igs-field",
      "name": "Field Survey Division",
      "rank": 3,
      "depth": 0
    }
  ]
}
```

---

#### Get Children

Get direct children of a taxon.

```http
GET /v1/taxonomy/taxa/{taxon_id}/children
```

**Query Parameters:**

| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| include_deprecated | boolean | No | Include deprecated children (default: false) |
| sort | string | No | Sort field: name, rank, created_at (default: rank) |
| order | string | No | Sort order: asc, desc (default: asc) |

**Response:** `200 OK`

```json
{
  "parent_taxon_id": "faction-geologist-guild",
  "children": [
    {
      "taxon_id": "faction-igs-field",
      "name": "Field Survey Division",
      "rank": 3,
      "child_count": 8
    },
    {
      "taxon_id": "faction-igs-lab",
      "name": "Laboratory Division",
      "rank": 3,
      "child_count": 12
    },
    {
      "taxon_id": "faction-igs-analysis",
      "name": "Data Analysis Division",
      "rank": 3,
      "child_count": 15
    }
  ]
}
```

---

#### Get Siblings

Get sibling taxa (sharing the same parent).

```http
GET /v1/taxonomy/taxa/{taxon_id}/siblings
```

**Response:** `200 OK`

```json
{
  "taxon_id": "faction-igs-field",
  "parent_taxon_id": "faction-geologist-guild",
  "siblings": [
    {
      "taxon_id": "faction-igs-lab",
      "name": "Laboratory Division",
      "rank": 3
    },
    {
      "taxon_id": "faction-igs-analysis",
      "name": "Data Analysis Division",
      "rank": 3
    }
  ]
}
```

---

### Search and Discovery

#### Search Taxa

Full-text search across taxa.

```http
GET /v1/taxonomy/taxa/search
```

**Query Parameters:**

| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| q | string | Yes | Search query |
| domain | string | No | Limit to specific domain |
| rank | integer | No | Limit to specific rank |
| limit | integer | No | Results limit (default: 20, max: 100) |

**Example Request:**

```http
GET /v1/taxonomy/taxa/search?q=geological&domain=faction
```

**Response:** `200 OK`

```json
{
  "query": "geological",
  "results": [
    {
      "taxon_id": "faction-geologist-guild",
      "domain": "faction",
      "name": "International Geological Survey",
      "rank": 2,
      "relevance_score": 0.95,
      "match_type": "name"
    },
    {
      "taxon_id": "faction-geo-research",
      "domain": "faction",
      "name": "Geological Research Institute",
      "rank": 2,
      "relevance_score": 0.87,
      "match_type": "description"
    }
  ],
  "total": 2
}
```

---

### Relationships

#### Get Taxon Relationships

Get relationships for a specific taxon.

```http
GET /v1/taxonomy/taxa/{taxon_id}/relationships
```

**Query Parameters:**

| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| type | string | No | Filter by relationship type |
| direction | string | No | "outgoing", "incoming", "both" (default: "both") |

**Response:** `200 OK`

```json
{
  "taxon_id": "faction-geologist-guild",
  "relationships": [
    {
      "relationship_type": "synonym",
      "target_taxon_id": "faction-igs",
      "target_name": "IGS",
      "strength": 1.0
    },
    {
      "relationship_type": "related",
      "target_taxon_id": "achievement-geological-expert",
      "target_name": "Geological Expert Achievement",
      "strength": 0.8,
      "metadata": {
        "reason": "faction_reputation_requirement"
      }
    }
  ]
}
```

---

#### Create Relationship

Create a relationship between two taxa.

```http
POST /v1/taxonomy/relationships
```

**Request Body:**

```json
{
  "source_taxon_id": "faction-geologist-guild",
  "target_taxon_id": "faction-mining-corp",
  "relationship_type": "related",
  "strength": 0.6,
  "metadata": {
    "reason": "shared_mineral_interests"
  }
}
```

**Response:** `201 Created`

---

### Domain Management

#### List Domains

Get all available taxonomy domains.

```http
GET /v1/taxonomy/domains
```

**Response:** `200 OK`

```json
{
  "domains": [
    {
      "domain": "faction",
      "name": "Factions & Organizations",
      "description": "Organizational hierarchies",
      "taxon_count": 127,
      "max_rank": 5
    },
    {
      "domain": "species",
      "name": "Species Classification",
      "description": "Biological taxonomy",
      "taxon_count": 423,
      "max_rank": 7
    },
    {
      "domain": "geological",
      "name": "Geological Features",
      "description": "Terrain and formation types",
      "taxon_count": 89,
      "max_rank": 4
    }
  ]
}
```

---

#### Get Domain Statistics

Get statistics for a specific domain.

```http
GET /v1/taxonomy/domains/{domain}/stats
```

**Response:** `200 OK`

```json
{
  "domain": "faction",
  "total_taxa": 127,
  "deprecated_taxa": 3,
  "root_taxa": 1,
  "max_depth": 5,
  "avg_depth": 3.2,
  "rank_distribution": {
    "0": 1,
    "1": 4,
    "2": 12,
    "3": 45,
    "4": 52,
    "5": 13
  },
  "last_updated": "2025-01-19T15:30:00Z"
}
```

---

## Rate Limits

- **Authenticated requests:** 1000 requests per hour
- **Search endpoints:** 100 requests per hour
- **Write operations:** 100 requests per hour

Rate limit headers:
```
X-RateLimit-Limit: 1000
X-RateLimit-Remaining: 847
X-RateLimit-Reset: 1642600800
```

## Error Responses

All errors follow a consistent format:

```json
{
  "error": {
    "code": "TAXON_NOT_FOUND",
    "message": "The requested taxon does not exist",
    "details": {
      "taxon_id": "faction-invalid"
    }
  }
}
```

### Common Error Codes

| Code | HTTP Status | Description |
|------|-------------|-------------|
| `TAXON_NOT_FOUND` | 404 | Taxon does not exist |
| `INVALID_DOMAIN` | 400 | Invalid taxonomy domain |
| `INVALID_RANK` | 400 | Invalid rank value |
| `CIRCULAR_REFERENCE` | 400 | Parent creates circular hierarchy |
| `DUPLICATE_TAXON_ID` | 409 | Taxon ID already exists |
| `INSUFFICIENT_PERMISSIONS` | 403 | User lacks required permissions |
| `RATE_LIMIT_EXCEEDED` | 429 | Too many requests |

## Authentication

All API requests require a valid Bearer token:

```http
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

## Permissions

Required permissions for taxonomy operations:

- `taxonomy:read` - View taxa
- `taxonomy:write` - Create/update taxa
- `taxonomy:delete` - Deprecate taxa
- `taxonomy:admin` - All operations including domain management

## Best Practices

1. **Cache Frequently Used Hierarchies**: Use `/hierarchy` endpoint results for client-side caching
2. **Pagination**: Always use pagination for list endpoints to avoid large responses
3. **Batch Operations**: Group multiple operations when possible to reduce API calls
4. **Include Filters**: Use domain and parent_id filters to narrow search scope
5. **Handle Deprecation**: Check `is_deprecated` flag in your application logic

## Examples

### Example 1: Browse Faction Hierarchy

```javascript
// Get root factions
const rootResponse = await fetch('/v1/taxonomy/taxa?domain=faction&rank=0');
const roots = await rootResponse.json();

// Get children of root
const childrenResponse = await fetch(
  `/v1/taxonomy/taxa/${roots.data[0].taxon_id}/children`
);
const children = await childrenResponse.json();

// Get full hierarchy for a specific faction
const hierarchyResponse = await fetch(
  `/v1/taxonomy/taxa/faction-igs-field/hierarchy?direction=up`
);
const hierarchy = await hierarchyResponse.json();
```

### Example 2: Create New Faction Division

```javascript
const newTaxon = {
  taxon_id: "faction-igs-coastal",
  domain: "faction",
  name: "Coastal Survey Division",
  parent_taxon_id: "faction-igs-field",
  rank: 4,
  description: "Specializes in coastal and marine geology",
  attributes: {
    region: "coastal",
    specialization: "marine_geology"
  }
};

const response = await fetch('/v1/taxonomy/taxa', {
  method: 'POST',
  headers: {
    'Authorization': 'Bearer <token>',
    'Content-Type': 'application/json'
  },
  body: JSON.stringify(newTaxon)
});
```

### Example 3: Search for Geological Taxa

```javascript
const searchResponse = await fetch(
  '/v1/taxonomy/taxa/search?q=volcanic&domain=geological'
);
const results = await searchResponse.json();

results.results.forEach(taxon => {
  console.log(`${taxon.name} (rank ${taxon.rank})`);
});
```

## Changelog

### Version 1.0 (2025-01-19)

- Initial API specification
- Core CRUD operations for taxa
- Hierarchy navigation endpoints
- Search functionality
- Relationship management
