# [Feature Name] API Specification

**Document Type:** API Specification  
**Version:** 1.0  
**Author:** [Your Name/Team]  
**Date:** [YYYY-MM-DD]  
**Status:** [Draft/Review/Approved]  
**Related Specifications:**

- [Link to related system design](../systems/README.md)
- [Link to related features](../gameplay/README.md)

## Overview

[Brief description of the API, its purpose, and what it enables. Explain the business context
and how this API fits into the larger BlueMarble architecture.]

### Key Features

- [Feature 1]
- [Feature 2]
- [Feature 3]

### Design Goals

- [Goal 1: e.g., High performance for real-time operations]
- [Goal 2: e.g., Scalability for concurrent users]
- [Goal 3: e.g., Clear and consistent interface]

## Base Configuration

**Base URL:** `https://api.bluemarble.design/v1/[resource-path]`  
**Authentication:** [Required/Optional] - [Bearer token/API Key/etc.]  
**Content-Type:** `application/json`  
**Rate Limit:** [X requests per minute for standard users]

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

### 1. [Resource Collection Name]

#### List [Resources]

**Endpoint:** `GET /[resources]`

**Description:** [What this endpoint does]

**Query Parameters:**

- `limit` (integer, optional): Number of results per page (default: 50, max: 100)
- `offset` (integer, optional): Number of results to skip (default: 0)
- `sort` (string, optional): Sort field and order (e.g., "created:desc")
- `filter` (string, optional): Filter criteria

**Example Request:**

```http
GET /v1/[resources]?limit=20&sort=created:desc
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

**Response:** `200 OK`

```json
{
  "success": true,
  "data": {
    "items": [
      {
        "id": "resource-123",
        "name": "Example Resource",
        "status": "active",
        "createdAt": "2024-12-29T12:00:00Z",
        "updatedAt": "2024-12-29T12:00:00Z"
      }
    ],
    "pagination": {
      "limit": 20,
      "offset": 0,
      "total": 100,
      "hasMore": true
    }
  },
  "metadata": {
    "requestId": "req-123e4567-e89b-12d3-a456-426614174000",
    "timestamp": "2024-12-29T12:00:00Z"
  }
}
```

**Error Responses:**

- `401 Unauthorized`: Invalid or missing authentication token
- `403 Forbidden`: User lacks permission to view resources
- `429 Too Many Requests`: Rate limit exceeded

#### Create [Resource]

**Endpoint:** `POST /[resources]`

**Description:** [What this endpoint does]

**Request Body:**

```json
{
  "name": "New Resource",
  "description": "Resource description",
  "properties": {
    "key1": "value1",
    "key2": "value2"
  }
}
```

**Validation Rules:**

- `name`: Required, 3-50 characters, alphanumeric with spaces
- `description`: Optional, max 500 characters
- `properties`: Optional, object with max 10 key-value pairs

**Response:** `201 Created`

```json
{
  "success": true,
  "data": {
    "id": "resource-124",
    "name": "New Resource",
    "description": "Resource description",
    "status": "active",
    "createdAt": "2024-12-29T12:00:00Z",
    "updatedAt": "2024-12-29T12:00:00Z",
    "properties": {
      "key1": "value1",
      "key2": "value2"
    }
  },
  "metadata": {
    "requestId": "req-123e4567-e89b-12d3-a456-426614174000",
    "timestamp": "2024-12-29T12:00:00Z"
  }
}
```

**Error Responses:**

- `400 Bad Request`: Invalid request format
- `422 Unprocessable Entity`: Validation failed
- `409 Conflict`: Resource already exists

#### Get [Resource] Details

**Endpoint:** `GET /[resources]/{resourceId}`

**Description:** [What this endpoint does]

**Path Parameters:**

- `resourceId` (string, required): Unique identifier for the resource

**Response:** `200 OK`

```json
{
  "success": true,
  "data": {
    "id": "resource-123",
    "name": "Example Resource",
    "description": "Detailed description",
    "status": "active",
    "createdAt": "2024-12-29T12:00:00Z",
    "updatedAt": "2024-12-29T12:00:00Z",
    "metadata": {
      "createdBy": "user-456",
      "lastModifiedBy": "user-789"
    }
  },
  "metadata": {
    "requestId": "req-123e4567-e89b-12d3-a456-426614174000",
    "timestamp": "2024-12-29T12:00:00Z"
  }
}
```

**Error Responses:**

- `404 Not Found`: Resource does not exist

#### Update [Resource]

**Endpoint:** `PATCH /[resources]/{resourceId}`

**Description:** [What this endpoint does - partial update]

**Request Body:**

```json
{
  "name": "Updated Name",
  "status": "inactive"
}
```

**Response:** `200 OK`

```json
{
  "success": true,
  "data": {
    "id": "resource-123",
    "name": "Updated Name",
    "status": "inactive",
    "updatedAt": "2024-12-29T13:00:00Z"
  },
  "metadata": {
    "requestId": "req-123e4567-e89b-12d3-a456-426614174000",
    "timestamp": "2024-12-29T13:00:00Z"
  }
}
```

**Error Responses:**

- `404 Not Found`: Resource does not exist
- `409 Conflict`: Update conflicts with current state
- `422 Unprocessable Entity`: Validation failed

#### Delete [Resource]

**Endpoint:** `DELETE /[resources]/{resourceId}`

**Description:** [What this endpoint does]

**Response:** `204 No Content`

**Error Responses:**

- `404 Not Found`: Resource does not exist
- `409 Conflict`: Resource cannot be deleted due to dependencies

### 2. [Nested Resource or Action]

#### [Action Name]

**Endpoint:** `POST /[resources]/{resourceId}/actions/[action-name]`

**Description:** [What this action does]

**Request Body:**

```json
{
  "parameter1": "value1",
  "parameter2": "value2"
}
```

**Response:** `200 OK` or `202 Accepted` (for async operations)

```json
{
  "success": true,
  "data": {
    "actionResult": "success",
    "details": {
      "key": "value"
    }
  }
}
```

## Error Handling

### Standard Error Response Format

All error responses follow this structure:

```json
{
  "success": false,
  "error": {
    "code": "ERROR_CODE",
    "message": "Human-readable error message",
    "details": "Additional context about the error",
    "field": "fieldName",
    "timestamp": "2024-12-29T12:00:00Z"
  },
  "metadata": {
    "requestId": "req-123e4567-e89b-12d3-a456-426614174000",
    "timestamp": "2024-12-29T12:00:00Z"
  }
}
```

### Error Codes

| HTTP Status | Error Code | Description | Retry Recommended |
|------------|------------|-------------|-------------------|
| 400 | BAD_REQUEST | Invalid request format | No |
| 401 | UNAUTHORIZED | Authentication required | No |
| 403 | FORBIDDEN | Insufficient permissions | No |
| 404 | NOT_FOUND | Resource not found | No |
| 409 | CONFLICT | Resource conflict | Maybe |
| 422 | VALIDATION_ERROR | Validation failed | No |
| 429 | RATE_LIMIT_EXCEEDED | Too many requests | Yes (after delay) |
| 500 | INTERNAL_ERROR | Server error | Yes (with backoff) |
| 503 | SERVICE_UNAVAILABLE | Service temporarily down | Yes (after delay) |

### Validation Error Example

```json
{
  "success": false,
  "error": {
    "code": "VALIDATION_ERROR",
    "message": "Request validation failed",
    "errors": [
      {
        "field": "name",
        "message": "Name must be between 3 and 50 characters",
        "constraint": "length",
        "value": "AB"
      }
    ]
  }
}
```

## Rate Limiting

### Rate Limits

| User Type | Requests/Minute | Burst Limit | Concurrent Requests |
|-----------|-----------------|-------------|---------------------|
| Guest | [X] | [Y] | [Z] |
| Standard User | [X] | [Y] | [Z] |
| Premium User | [X] | [Y] | [Z] |
| Service Account | [X] | [Y] | [Z] |

### Rate Limit Headers

```http
X-RateLimit-Limit: 100
X-RateLimit-Remaining: 85
X-RateLimit-Reset: 1735387200
```

### Rate Limit Exceeded Response

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

[Describe the authentication method: JWT Bearer tokens, API keys, OAuth, etc.]

**Example:**

```http
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

### Required Permissions

| Endpoint | Method | Required Permission |
|----------|--------|---------------------|
| `/[resources]` | GET | `[resource]:read` |
| `/[resources]` | POST | `[resource]:create` |
| `/[resources]/{id}` | GET | `[resource]:read` |
| `/[resources]/{id}` | PATCH | `[resource]:update` |
| `/[resources]/{id}` | DELETE | `[resource]:delete` |

### Authorization Rules

- [Rule 1: e.g., Users can only modify their own resources]
- [Rule 2: e.g., Admins can access all resources]
- [Rule 3: e.g., Premium users have extended rate limits]

## Data Models

### [Primary Resource] Model

```json
{
  "id": "string (UUID)",
  "name": "string (3-50 chars)",
  "description": "string (optional, max 500 chars)",
  "status": "enum: active|inactive|archived",
  "createdAt": "ISO 8601 timestamp",
  "updatedAt": "ISO 8601 timestamp",
  "createdBy": "string (user ID)",
  "properties": {
    "customField1": "any",
    "customField2": "any"
  }
}
```

### Field Descriptions

| Field | Type | Required | Description | Constraints |
|-------|------|----------|-------------|-------------|
| id | string | Yes | Unique identifier | UUID format |
| name | string | Yes | Resource name | 3-50 characters |
| description | string | No | Detailed description | Max 500 characters |
| status | enum | Yes | Current status | active, inactive, archived |
| createdAt | timestamp | Yes | Creation timestamp | ISO 8601 format |
| updatedAt | timestamp | Yes | Last update timestamp | ISO 8601 format |

## Webhooks (if applicable)

### Webhook Events

| Event | Trigger | Payload |
|-------|---------|---------|
| `[resource].created` | Resource is created | Full resource object |
| `[resource].updated` | Resource is modified | Updated fields + resource ID |
| `[resource].deleted` | Resource is deleted | Resource ID |

### Webhook Payload Format

```json
{
  "event": "[resource].created",
  "timestamp": "2024-12-29T12:00:00Z",
  "data": {
    "resourceId": "resource-123",
    "resource": { /* full resource object */ }
  },
  "signature": "sha256=..."
}
```

### Webhook Configuration

```http
POST /v1/webhooks
Authorization: Bearer {token}

{
  "url": "https://your-service.com/webhook",
  "events": ["[resource].created", "[resource].updated"],
  "secret": "your-webhook-secret"
}
```

## SDK Examples

### JavaScript/TypeScript

```javascript
import { BlueMarbleAPI } from '@bluemarble/api-client';

const client = new BlueMarbleAPI({
  apiKey: 'your-api-key',
  baseUrl: 'https://api.bluemarble.design/v1'
});

// List resources
const resources = await client.[resources].list({
  limit: 20,
  sort: 'created:desc'
});

// Create resource
const newResource = await client.[resources].create({
  name: 'My Resource',
  description: 'Description here'
});

// Get resource
const resource = await client.[resources].get('resource-123');

// Update resource
const updated = await client.[resources].update('resource-123', {
  name: 'Updated Name'
});

// Delete resource
await client.[resources].delete('resource-123');
```

### Python

```python
from bluemarble_api import BlueMarbleAPI

client = BlueMarbleAPI(
    api_key='your-api-key',
    base_url='https://api.bluemarble.design/v1'
)

# List resources
resources = client.[resources].list(limit=20, sort='created:desc')

# Create resource
new_resource = client.[resources].create(
    name='My Resource',
    description='Description here'
)

# Get resource
resource = client.[resources].get('resource-123')

# Update resource
updated = client.[resources].update(
    'resource-123',
    name='Updated Name'
)

# Delete resource
client.[resources].delete('resource-123')
```

### CSharp

```csharp
using BlueMarble.ApiClient;

var client = new BlueMarbleApiClient(new ApiClientOptions
{
    ApiKey = "your-api-key",
    BaseUrl = "https://api.bluemarble.design/v1"
});

// List resources
var resources = await client.[Resources].ListAsync(new ListOptions
{
    Limit = 20,
    Sort = "created:desc"
});

// Create resource
var newResource = await client.[Resources].CreateAsync(new [Resource]
{
    Name = "My Resource",
    Description = "Description here"
});

// Get resource
var resource = await client.[Resources].GetAsync("resource-123");

// Update resource
var updated = await client.[Resources].UpdateAsync("resource-123", new [Resource]Update
{
    Name = "Updated Name"
});

// Delete resource
await client.[Resources].DeleteAsync("resource-123");
```

## Performance Considerations

### Response Time Targets

| Endpoint Type | Target (p95) | Target (p99) |
|--------------|--------------|--------------|
| Read operations | < 100ms | < 200ms |
| Write operations | < 300ms | < 500ms |
| Complex queries | < 500ms | < 1000ms |
| Async operations | < 50ms (initiation) | < 100ms |

### Caching Strategy

**Cacheable Endpoints:**

- `GET /[resources]` - Cache for [X] seconds
- `GET /[resources]/{id}` - Cache for [Y] seconds

**Cache Headers:**

```http
Cache-Control: public, max-age=300
ETag: "33a64df551425fcc55e4d42a148795d9f25f89d4"
```

### Pagination Best Practices

- Use cursor-based pagination for large datasets
- Default page size: 50 items
- Maximum page size: 100 items
- Include `hasMore` indicator in responses

## Testing

### Test Coverage Requirements

- [ ] Unit tests for all business logic
- [ ] Integration tests for all endpoints
- [ ] Load tests for performance validation
- [ ] Security tests for vulnerability scanning

### Example Test Cases

#### Test Case 1: Create Resource

```gherkin
Given: Valid authentication token
When: POST /[resources] with valid data
Then: Returns 201 Created with resource object
```

#### Test Case 2: Invalid Input

```gherkin
Given: Valid authentication token
When: POST /[resources] with invalid data
Then: Returns 422 Validation Error with details
```

#### Test Case 3: Unauthorized Access

```gherkin
Given: No authentication token
When: GET /[resources]
Then: Returns 401 Unauthorized
```

## Migration Guide (if applicable)

### From Previous Version

[If this is an update to an existing API, provide migration guidance]

**Breaking Changes:**

- [Change 1 and how to adapt]
- [Change 2 and how to adapt]

**Deprecated Features:**

- [Feature 1] - Use [alternative] instead
- [Feature 2] - Use [alternative] instead

## References

**Related Documentation:**

- [General API Specifications](./api-specifications.md)
- [Authentication Guide](./README.md)
- [Error Handling Guide](./README.md)

**Standards:**

- [RESTful API Best Practices](https://restfulapi.net/)
- [OpenAPI Specification](https://spec.openapis.org/oas/v3.1.0)

## Version History

| Version | Date | Author | Changes |
|---------|------|--------|---------|
| 1.0 | [YYYY-MM-DD] | [Author] | Initial specification |

## Approval Checklist

- [ ] API endpoints reviewed and approved
- [ ] Request/response formats documented
- [ ] Authentication and authorization specified
- [ ] Error handling documented
- [ ] Rate limiting configured
- [ ] Performance targets defined
- [ ] Security review completed
- [ ] SDK examples provided
- [ ] Test cases defined
- [ ] Documentation complete
- [ ] Stakeholder approval received
