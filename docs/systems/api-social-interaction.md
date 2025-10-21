# BlueMarble - Social Interaction API Specification

**Version:** 1.0  
**Date:** 2025-10-06  
**Author:** BlueMarble API Team  
**Status:** Implemented

## Overview

This document specifies the RESTful API endpoints for the Social Interaction and Settlement Management System. All endpoints follow REST principles, use JSON for data exchange, and require authentication unless otherwise noted.

## Base URL

```
https://api.bluemarble.game/v1
```

## Authentication

All authenticated endpoints require a Bearer token in the Authorization header:

```
Authorization: Bearer {access_token}
```

## Settlement Management Endpoints

### Establish Settlement

Create a new settlement under player/entity control.

**Endpoint:** `POST /settlements`

**Authentication:** Required

**Request Body:**
```json
{
  "name": "Riverside Village",
  "type": "Village",
  "location": {
    "latitude": 45.5231,
    "longitude": -122.6765,
    "region": "Northwest Territory"
  },
  "entityId": "uuid-of-controlling-entity"
}
```

**Response:** `201 Created`
```json
{
  "settlementId": "550e8400-e29b-41d4-a716-446655440000",
  "name": "Riverside Village",
  "type": "Village",
  "controlledBy": "uuid-of-controlling-entity",
  "controlEstablished": "2025-10-06T14:30:00Z",
  "currentPopulation": 120,
  "maxPopulation": 500,
  "controlStability": 50.0,
  "requirements": {
    "political": 10.0,
    "economic": 5.0,
    "military": 5.0
  },
  "location": {
    "latitude": 45.5231,
    "longitude": -122.6765,
    "region": "Northwest Territory",
    "influenceRadius": 5.0
  },
  "economicOutput": 12.0,
  "taxIncome": 2.4
}
```

**Error Responses:**
- `400 Bad Request` - Invalid settlement data or location
- `403 Forbidden` - Insufficient influence to establish settlement
- `409 Conflict` - Location already occupied by another settlement

### Get Settlement Details

Retrieve detailed information about a specific settlement.

**Endpoint:** `GET /settlements/{settlementId}`

**Authentication:** Required

**Response:** `200 OK`
```json
{
  "settlementId": "550e8400-e29b-41d4-a716-446655440000",
  "name": "Riverside Village",
  "type": "Village",
  "controlledBy": "uuid-of-controlling-entity",
  "controlEstablished": "2025-10-06T14:30:00Z",
  "controlStability": 72.5,
  "currentPopulation": 245,
  "maxPopulation": 500,
  "populationGrowthRate": 3.2,
  "populationHappiness": 68.0,
  "requirements": {
    "political": 10.0,
    "economic": 5.0,
    "military": 5.0
  },
  "economicOutput": 24.5,
  "taxIncome": 4.9,
  "resourceProduction": {
    "food": 15.2,
    "wood": 8.5,
    "stone": 3.1
  },
  "location": {
    "latitude": 45.5231,
    "longitude": -122.6765,
    "region": "Northwest Territory",
    "influenceRadius": 5.0
  },
  "buildings": [
    {
      "buildingId": "building-uuid-1",
      "type": "Farm",
      "level": 2,
      "production": 5.0
    },
    {
      "buildingId": "building-uuid-2",
      "type": "Market",
      "level": 1,
      "economicBonus": 1.15
    }
  ],
  "defenses": [
    {
      "defenseId": "defense-uuid-1",
      "type": "Palisade",
      "level": 1,
      "defenseRating": 15.0
    }
  ]
}
```

**Error Responses:**
- `404 Not Found` - Settlement does not exist

### List Player Settlements

Get all settlements controlled by a specific player/entity.

**Endpoint:** `GET /settlements/entity/{entityId}`

**Authentication:** Required

**Query Parameters:**
- `page` (optional): Page number for pagination (default: 1)
- `pageSize` (optional): Number of results per page (default: 20, max: 100)
- `type` (optional): Filter by settlement type

**Response:** `200 OK`
```json
{
  "settlements": [
    {
      "settlementId": "settlement-uuid-1",
      "name": "Riverside Village",
      "type": "Village",
      "population": 245,
      "controlStability": 72.5
    },
    {
      "settlementId": "settlement-uuid-2",
      "name": "Mountain Outpost",
      "type": "Outpost",
      "population": 45,
      "controlStability": 55.0
    }
  ],
  "pagination": {
    "currentPage": 1,
    "totalPages": 1,
    "totalSettlements": 2,
    "pageSize": 20
  }
}
```

### Challenge Settlement Control

Initiate a challenge for control of a settlement.

**Endpoint:** `POST /settlements/{settlementId}/challenge`

**Authentication:** Required

**Request Body:**
```json
{
  "challengerId": "uuid-of-challenging-entity",
  "challengeType": "Political"
}
```

**Response:** `202 Accepted`
```json
{
  "challengeId": "challenge-uuid",
  "settlementId": "settlement-uuid",
  "challengerId": "challenger-uuid",
  "currentControllerId": "controller-uuid",
  "challengeType": "Political",
  "startTime": "2025-10-06T15:00:00Z",
  "endTime": "2025-10-09T15:00:00Z",
  "challengerInfluence": 45.0,
  "defenderInfluence": 38.0,
  "status": "Active"
}
```

**Error Responses:**
- `403 Forbidden` - Insufficient influence or diplomatic restrictions
- `409 Conflict` - Challenge already in progress

### Upgrade Settlement

Upgrade settlement to next tier or improve infrastructure.

**Endpoint:** `PUT /settlements/{settlementId}/upgrade`

**Authentication:** Required

**Request Body:**
```json
{
  "upgradeType": "SettlementType",
  "targetType": "Town"
}
```

**Response:** `200 OK`
```json
{
  "settlementId": "settlement-uuid",
  "name": "Riverside Village",
  "previousType": "Village",
  "newType": "Town",
  "upgradeCost": {
    "gold": 5000,
    "wood": 500,
    "stone": 300
  },
  "newRequirements": {
    "political": 25.0,
    "economic": 20.0,
    "military": 15.0
  },
  "newMaxPopulation": 2000,
  "upgradeCompletionTime": "2025-10-13T15:00:00Z"
}
```

**Error Responses:**
- `400 Bad Request` - Invalid upgrade type or target
- `403 Forbidden` - Insufficient resources or influence

### Transfer Settlement Control

Voluntarily transfer control of a settlement to another entity.

**Endpoint:** `POST /settlements/{settlementId}/transfer`

**Authentication:** Required (must be current controller)

**Request Body:**
```json
{
  "newControllerId": "uuid-of-new-controller",
  "transferType": "Voluntary",
  "terms": {
    "compensationGold": 1000,
    "diplomaticAgreement": true
  }
}
```

**Response:** `200 OK`
```json
{
  "settlementId": "settlement-uuid",
  "previousController": "old-controller-uuid",
  "newController": "new-controller-uuid",
  "transferTime": "2025-10-06T16:00:00Z",
  "transferType": "Voluntary",
  "populationHappinessChange": -5.0
}
```

## Diplomacy Endpoints

### Create Diplomatic Relationship

Establish a diplomatic relationship between two entities.

**Endpoint:** `POST /diplomacy/relationships`

**Authentication:** Required

**Request Body:**
```json
{
  "entityA": "entity-uuid-1",
  "entityB": "entity-uuid-2",
  "initialValue": 0.0,
  "initiatedBy": "entity-uuid-1"
}
```

**Response:** `201 Created`
```json
{
  "relationshipId": "relationship-uuid",
  "entityA": "entity-uuid-1",
  "entityB": "entity-uuid-2",
  "relationshipValue": 0.0,
  "status": "Neutral",
  "establishedDate": "2025-10-06T14:00:00Z",
  "lastStatusChange": "2025-10-06T14:00:00Z",
  "tradeAgreement": false,
  "mutualDefense": false,
  "nonAggressionPact": false
}
```

### Get Diplomatic Relationship

Retrieve diplomatic relationship details between two entities.

**Endpoint:** `GET /diplomacy/relationships/{relationshipId}`

**Alternative:** `GET /diplomacy/relationships?entityA={uuid}&entityB={uuid}`

**Authentication:** Required

**Response:** `200 OK`
```json
{
  "relationshipId": "relationship-uuid",
  "entityA": "entity-uuid-1",
  "entityB": "entity-uuid-2",
  "relationshipValue": 35.0,
  "status": "Friendly",
  "establishedDate": "2025-09-15T10:00:00Z",
  "lastStatusChange": "2025-10-01T14:30:00Z",
  "agreements": [
    {
      "agreementId": "agreement-uuid-1",
      "type": "TradeAgreement",
      "signedDate": "2025-09-20T12:00:00Z",
      "expirationDate": "2026-09-20T12:00:00Z",
      "terms": {
        "transactionCostReduction": 15.0
      }
    }
  ],
  "history": [
    {
      "incidentId": "incident-uuid-1",
      "type": "TradeVolumeIncrease",
      "date": "2025-10-01T14:30:00Z",
      "valueChange": 5.0,
      "description": "Trade volume increased significantly"
    }
  ],
  "tradeAgreement": true,
  "mutualDefense": false,
  "nonAggressionPact": true
}
```

### Update Diplomatic Relationship

Modify the diplomatic relationship value based on actions.

**Endpoint:** `PUT /diplomacy/relationships/{relationshipId}`

**Authentication:** Required

**Request Body:**
```json
{
  "valueChange": 10.0,
  "reason": "Successful joint defense operation",
  "incidentType": "MilitaryCooperation"
}
```

**Response:** `200 OK`
```json
{
  "relationshipId": "relationship-uuid",
  "previousValue": 35.0,
  "newValue": 45.0,
  "previousStatus": "Friendly",
  "newStatus": "Friendly",
  "valueChange": 10.0,
  "timestamp": "2025-10-06T16:00:00Z"
}
```

### List Entity Relationships

Get all diplomatic relationships for an entity.

**Endpoint:** `GET /diplomacy/relationships/entity/{entityId}`

**Authentication:** Required

**Query Parameters:**
- `status` (optional): Filter by diplomatic status
- `page` (optional): Page number
- `pageSize` (optional): Results per page

**Response:** `200 OK`
```json
{
  "entityId": "entity-uuid",
  "relationships": [
    {
      "relationshipId": "relationship-uuid-1",
      "otherEntityId": "other-entity-uuid-1",
      "otherEntityName": "Northern Guild",
      "relationshipValue": 45.0,
      "status": "Friendly"
    },
    {
      "relationshipId": "relationship-uuid-2",
      "otherEntityId": "other-entity-uuid-2",
      "otherEntityName": "Southern Alliance",
      "relationshipValue": -30.0,
      "status": "Rival"
    }
  ],
  "pagination": {
    "currentPage": 1,
    "totalPages": 1,
    "totalRelationships": 2
  }
}
```

### Propose Alliance

Submit an alliance proposal to another entity.

**Endpoint:** `POST /diplomacy/alliances/propose`

**Authentication:** Required

**Request Body:**
```json
{
  "proposerId": "proposer-entity-uuid",
  "targetId": "target-entity-uuid",
  "terms": {
    "mutualDefense": true,
    "tradeBonus": 25.0,
    "sharedIntelligence": true,
    "duration": "6 months"
  },
  "message": "Let us join forces for mutual prosperity"
}
```

**Response:** `201 Created`
```json
{
  "proposalId": "proposal-uuid",
  "proposerId": "proposer-entity-uuid",
  "targetId": "target-entity-uuid",
  "proposedAt": "2025-10-06T14:00:00Z",
  "expiresAt": "2025-10-13T14:00:00Z",
  "status": "Pending",
  "terms": {
    "mutualDefense": true,
    "tradeBonus": 25.0,
    "sharedIntelligence": true,
    "duration": "6 months"
  }
}
```

**Error Responses:**
- `400 Bad Request` - Invalid proposal terms
- `403 Forbidden` - Insufficient relationship level (must be Friendly)

### Accept Alliance Proposal

Accept a pending alliance proposal.

**Endpoint:** `POST /diplomacy/alliances/{proposalId}/accept`

**Authentication:** Required (must be target entity)

**Response:** `200 OK`
```json
{
  "proposalId": "proposal-uuid",
  "status": "Accepted",
  "acceptedAt": "2025-10-07T10:00:00Z",
  "relationshipUpdate": {
    "relationshipId": "relationship-uuid",
    "newValue": 55.0,
    "newStatus": "Allied"
  },
  "agreements": [
    {
      "agreementId": "agreement-uuid",
      "type": "MutualDefense",
      "effectiveDate": "2025-10-07T10:00:00Z",
      "expirationDate": "2026-04-07T10:00:00Z"
    }
  ]
}
```

### Declare War

Declare war on another entity.

**Endpoint:** `POST /diplomacy/war/declare`

**Authentication:** Required

**Request Body:**
```json
{
  "declaringEntityId": "entity-uuid-1",
  "targetEntityId": "entity-uuid-2",
  "reason": "Territorial dispute over Harbor Town",
  "objectives": [
    "Capture Harbor Town",
    "Reduce military influence"
  ]
}
```

**Response:** `201 Created`
```json
{
  "warId": "war-uuid",
  "declaringEntity": "entity-uuid-1",
  "targetEntity": "entity-uuid-2",
  "declaredAt": "2025-10-06T14:00:00Z",
  "status": "Active",
  "relationshipUpdate": {
    "relationshipId": "relationship-uuid",
    "newValue": -85.0,
    "newStatus": "War"
  },
  "affectedSettlements": [
    "settlement-uuid-1",
    "settlement-uuid-2"
  ]
}
```

## Federation Endpoints

### Create Federation

Form a new federation from allied entities.

**Endpoint:** `POST /federations`

**Authentication:** Required

**Request Body:**
```json
{
  "name": "Northern Alliance",
  "founderIds": [
    "entity-uuid-1",
    "entity-uuid-2"
  ],
  "charter": {
    "governanceModel": "Democratic",
    "memberContributionPercent": 20.0,
    "votingRules": "Majority",
    "expulsionRules": "TwoThirds"
  }
}
```

**Response:** `201 Created`
```json
{
  "federationId": "federation-uuid",
  "name": "Northern Alliance",
  "createdAt": "2025-10-06T14:00:00Z",
  "memberIds": [
    "entity-uuid-1",
    "entity-uuid-2"
  ],
  "leaderId": "entity-uuid-1",
  "governanceModel": "Democratic",
  "collectiveInfluence": {
    "political": 85.0,
    "economic": 90.0,
    "military": 65.0
  },
  "treasury": {
    "gold": 0,
    "resources": {}
  }
}
```

**Error Responses:**
- `400 Bad Request` - Invalid charter or insufficient members
- `403 Forbidden` - Members not all Allied with each other

### Get Federation Details

Retrieve detailed federation information.

**Endpoint:** `GET /federations/{federationId}`

**Authentication:** Required

**Response:** `200 OK`
```json
{
  "federationId": "federation-uuid",
  "name": "Northern Alliance",
  "createdAt": "2025-10-06T14:00:00Z",
  "memberIds": [
    "entity-uuid-1",
    "entity-uuid-2",
    "entity-uuid-3"
  ],
  "leaderId": "entity-uuid-1",
  "governanceModel": "Democratic",
  "charter": {
    "governanceModel": "Democratic",
    "memberContributionPercent": 20.0,
    "votingRules": "Majority",
    "expulsionRules": "TwoThirds"
  },
  "collectiveInfluence": {
    "political": 125.0,
    "economic": 140.0,
    "military": 95.0
  },
  "treasury": {
    "gold": 15000,
    "resources": {
      "wood": 5000,
      "stone": 3000
    }
  },
  "controlledSettlements": [
    "settlement-uuid-1",
    "settlement-uuid-2"
  ],
  "activeProjects": [
    {
      "projectId": "project-uuid-1",
      "name": "Grand Trade Hub",
      "type": "Infrastructure",
      "progress": 45.0,
      "completionDate": "2025-11-15T00:00:00Z"
    }
  ]
}
```

### Add Federation Member

Add a new member to an existing federation.

**Endpoint:** `POST /federations/{federationId}/members`

**Authentication:** Required (must have federation permission)

**Request Body:**
```json
{
  "newMemberId": "entity-uuid-4",
  "approvedBy": "entity-uuid-1",
  "votingResults": {
    "for": 2,
    "against": 0,
    "abstain": 1
  }
}
```

**Response:** `200 OK`
```json
{
  "federationId": "federation-uuid",
  "newMemberId": "entity-uuid-4",
  "addedAt": "2025-10-07T10:00:00Z",
  "memberCount": 4,
  "updatedInfluence": {
    "political": 155.0,
    "economic": 175.0,
    "military": 115.0
  }
}
```

### Remove Federation Member

Remove a member from the federation.

**Endpoint:** `DELETE /federations/{federationId}/members/{memberId}`

**Authentication:** Required (must have federation permission)

**Query Parameters:**
- `reason`: Reason for removal (voluntary, expelled, inactive)

**Response:** `200 OK`
```json
{
  "federationId": "federation-uuid",
  "removedMemberId": "entity-uuid-4",
  "removedAt": "2025-10-08T12:00:00Z",
  "reason": "voluntary",
  "memberCount": 3,
  "updatedInfluence": {
    "political": 125.0,
    "economic": 140.0,
    "military": 95.0
  }
}
```

### Get Federation Influence

Retrieve the collective influence of a federation.

**Endpoint:** `GET /federations/{federationId}/influence`

**Authentication:** Required

**Response:** `200 OK`
```json
{
  "federationId": "federation-uuid",
  "collectiveInfluence": {
    "political": 125.0,
    "economic": 140.0,
    "military": 95.0,
    "total": 360.0
  },
  "memberContributions": [
    {
      "memberId": "entity-uuid-1",
      "contribution": {
        "political": 45.0,
        "economic": 50.0,
        "military": 35.0
      }
    },
    {
      "memberId": "entity-uuid-2",
      "contribution": {
        "political": 40.0,
        "economic": 45.0,
        "military": 30.0
      }
    },
    {
      "memberId": "entity-uuid-3",
      "contribution": {
        "political": 40.0,
        "economic": 45.0,
        "military": 30.0
      }
    }
  ],
  "lastUpdated": "2025-10-06T16:00:00Z"
}
```

## Influence Endpoints

### Get Influence Profile

Retrieve entity's current influence values.

**Endpoint:** `GET /influence/{entityId}`

**Authentication:** Required

**Response:** `200 OK`
```json
{
  "entityId": "entity-uuid",
  "political": 45.0,
  "economic": 50.0,
  "military": 35.0,
  "total": 130.0,
  "lastUpdated": "2025-10-06T16:00:00Z",
  "modifiers": [
    {
      "name": "Guild Bonus",
      "type": "Political",
      "value": 5.0,
      "expiresAt": "2025-11-06T16:00:00Z"
    },
    {
      "name": "Trade Hub Control",
      "type": "Economic",
      "value": 10.0,
      "permanent": true
    }
  ]
}
```

### Modify Influence

Update entity influence values (typically called by game systems).

**Endpoint:** `POST /influence/{entityId}/modify`

**Authentication:** Required (system token)

**Request Body:**
```json
{
  "influenceType": "Political",
  "amount": 5.0,
  "reason": "Successful diplomatic mission",
  "source": "QuestSystem"
}
```

**Response:** `200 OK`
```json
{
  "entityId": "entity-uuid",
  "influenceType": "Political",
  "previousValue": 45.0,
  "newValue": 50.0,
  "change": 5.0,
  "reason": "Successful diplomatic mission",
  "timestamp": "2025-10-06T16:30:00Z"
}
```

### Get Effective Influence

Calculate effective influence for an entity considering diplomatic modifiers.

**Endpoint:** `GET /influence/{entityId}/effective`

**Authentication:** Required

**Query Parameters:**
- `targetEntityId` (optional): Calculate influence relative to specific entity
- `settlementId` (optional): Calculate influence for specific settlement

**Response:** `200 OK`
```json
{
  "entityId": "entity-uuid",
  "baseInfluence": {
    "political": 50.0,
    "economic": 55.0,
    "military": 35.0
  },
  "effectiveInfluence": {
    "political": 55.0,
    "economic": 60.5,
    "military": 35.0
  },
  "appliedModifiers": [
    {
      "source": "Allied with Target",
      "modifier": 1.1,
      "affectedTypes": ["Political", "Economic"]
    }
  ]
}
```

## Territorial Dispute Endpoints

### List Active Disputes

Get all active territorial disputes.

**Endpoint:** `GET /disputes`

**Authentication:** Required

**Query Parameters:**
- `entityId` (optional): Filter disputes involving specific entity
- `settlementId` (optional): Filter disputes for specific settlement
- `status` (optional): Filter by dispute status

**Response:** `200 OK`
```json
{
  "disputes": [
    {
      "disputeId": "dispute-uuid-1",
      "settlementId": "settlement-uuid",
      "settlementName": "Harbor Town",
      "disputeType": "ControlDispute",
      "status": "Active",
      "startedAt": "2025-10-05T10:00:00Z",
      "entities": [
        {
          "entityId": "entity-uuid-1",
          "entityName": "Guild C",
          "influence": 65.0
        },
        {
          "entityId": "entity-uuid-2",
          "entityName": "Guild D",
          "influence": 58.0
        }
      ],
      "resolutionDeadline": "2025-10-08T10:00:00Z"
    }
  ],
  "totalDisputes": 1
}
```

### Get Dispute Details

Retrieve detailed information about a specific dispute.

**Endpoint:** `GET /disputes/{disputeId}`

**Authentication:** Required

**Response:** `200 OK`
```json
{
  "disputeId": "dispute-uuid",
  "settlementId": "settlement-uuid",
  "settlementName": "Harbor Town",
  "disputeType": "ControlDispute",
  "status": "NegotiationPhase",
  "startedAt": "2025-10-05T10:00:00Z",
  "resolutionDeadline": "2025-10-08T10:00:00Z",
  "entities": [
    {
      "entityId": "entity-uuid-1",
      "entityName": "Guild C",
      "currentInfluence": 65.0,
      "diplomaticStatus": "Neutral",
      "proposedResolution": "Maintain current control with trade agreement"
    },
    {
      "entityId": "entity-uuid-2",
      "entityName": "Guild D",
      "currentInfluence": 58.0,
      "diplomaticStatus": "Neutral",
      "proposedResolution": "Joint control arrangement"
    }
  ],
  "negotiationHistory": [
    {
      "timestamp": "2025-10-06T12:00:00Z",
      "entityId": "entity-uuid-1",
      "action": "ProposedTradeAgreement",
      "details": "Offered 15% trade bonus in exchange for recognition"
    }
  ],
  "availableResolutions": [
    "Diplomatic",
    "Military",
    "Economic"
  ]
}
```

### Propose Dispute Resolution

Submit a resolution proposal for a territorial dispute.

**Endpoint:** `POST /disputes/{disputeId}/resolve`

**Authentication:** Required (must be involved entity)

**Request Body:**
```json
{
  "entityId": "entity-uuid-1",
  "resolutionType": "Diplomatic",
  "proposal": {
    "outcome": "TradeAgreement",
    "terms": {
      "recognizeControl": true,
      "tradeBonus": 15.0,
      "duration": "1 year"
    }
  }
}
```

**Response:** `202 Accepted`
```json
{
  "disputeId": "dispute-uuid",
  "proposalId": "proposal-uuid",
  "proposedBy": "entity-uuid-1",
  "resolutionType": "Diplomatic",
  "proposedAt": "2025-10-06T14:00:00Z",
  "requiresApproval": true,
  "approvalEntities": [
    "entity-uuid-2"
  ],
  "approvalDeadline": "2025-10-07T14:00:00Z"
}
```

## WebSocket Events

The Social Interaction system uses WebSocket connections for real-time notifications.

### Connection

```
wss://api.bluemarble.game/v1/ws/social
```

### Event Types

#### Settlement Control Challenged

```json
{
  "eventType": "SettlementChallenged",
  "timestamp": "2025-10-06T15:00:00Z",
  "data": {
    "settlementId": "settlement-uuid",
    "settlementName": "Riverside Village",
    "challengerId": "challenger-uuid",
    "challengerName": "Empire A",
    "challengeEndTime": "2025-10-09T15:00:00Z"
  }
}
```

#### Diplomatic Status Changed

```json
{
  "eventType": "DiplomaticStatusChanged",
  "timestamp": "2025-10-06T15:00:00Z",
  "data": {
    "relationshipId": "relationship-uuid",
    "entityA": "entity-uuid-1",
    "entityB": "entity-uuid-2",
    "previousStatus": "Friendly",
    "newStatus": "Allied",
    "newValue": 55.0
  }
}
```

#### Territorial Dispute Detected

```json
{
  "eventType": "TerritorialDisputeDetected",
  "timestamp": "2025-10-06T15:00:00Z",
  "data": {
    "disputeId": "dispute-uuid",
    "settlementId": "settlement-uuid",
    "involvedEntities": [
      "entity-uuid-1",
      "entity-uuid-2"
    ],
    "resolutionDeadline": "2025-10-09T15:00:00Z"
  }
}
```

#### Federation Member Joined

```json
{
  "eventType": "FederationMemberJoined",
  "timestamp": "2025-10-06T15:00:00Z",
  "data": {
    "federationId": "federation-uuid",
    "federationName": "Northern Alliance",
    "newMemberId": "entity-uuid-4",
    "newMemberName": "Eastern Guild"
  }
}
```

## Error Responses

### Standard Error Format

All error responses follow this format:

```json
{
  "error": {
    "code": "ERROR_CODE",
    "message": "Human-readable error message",
    "details": {
      "field": "Additional context if applicable"
    },
    "timestamp": "2025-10-06T15:00:00Z"
  }
}
```

### Common Error Codes

- `INSUFFICIENT_INFLUENCE` - Entity lacks required influence
- `DIPLOMATIC_RESTRICTION` - Action blocked by diplomatic status
- `SETTLEMENT_OCCUPIED` - Settlement already controlled
- `INVALID_SETTLEMENT_TYPE` - Invalid settlement type specified
- `CHALLENGE_IN_PROGRESS` - Cannot start new challenge during active challenge
- `INSUFFICIENT_RESOURCES` - Not enough resources for action
- `INVALID_RELATIONSHIP` - Relationship does not meet requirements
- `FEDERATION_REQUIREMENT_NOT_MET` - Federation requirements not satisfied
- `UNAUTHORIZED` - Authentication failed or insufficient permissions
- `NOT_FOUND` - Requested resource does not exist
- `RATE_LIMIT_EXCEEDED` - Too many requests

## Rate Limiting

API requests are rate-limited per user/entity:

- Standard endpoints: 100 requests per minute
- Write operations: 20 requests per minute
- WebSocket messages: 50 per minute

Rate limit headers are included in all responses:

```
X-RateLimit-Limit: 100
X-RateLimit-Remaining: 95
X-RateLimit-Reset: 1696608000
```

## Versioning

The API uses URL-based versioning. The current version is v1. When breaking changes are introduced, a new version will be released (v2, v3, etc.) and maintained in parallel for a transition period.

## Conclusion

This API specification provides comprehensive access to the Social Interaction and Settlement Management System, enabling clients to manage settlements, conduct diplomacy, form federations, and resolve territorial disputes. The API is designed to be RESTful, scalable, and real-time where appropriate using WebSocket connections.
