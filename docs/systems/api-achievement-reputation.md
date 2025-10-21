# BlueMarble Achievement and Reputation API Specification

**Document Type:** API Specification  
**Version:** 1.0  
**Date:** 2025-01-19  
**Status:** Draft  
**Priority:** High  
**Related Documents:**

- [Achievement and Reputation System Design](achievement-reputation-system.md)
- [API Specifications and Protocols](api-specifications.md)
- [Database Schema Design](database-schema-design.md)

## Overview

This document specifies the RESTful API endpoints for the BlueMarble Achievement and Reputation System. These endpoints
enable clients to retrieve achievement data, track progress, manage reputation, and distribute rewards. All endpoints
follow the standard BlueMarble API conventions defined in the [API Specifications](api-specifications.md).

## Base URL

```
https://api.bluemarble.design/v1
```

## Authentication

All endpoints require authentication via Bearer token:

```http
Authorization: Bearer {jwt_token}
```

## Achievement Endpoints

### List Achievements

Retrieve a paginated list of available achievements.

**Endpoint:** `GET /achievements`

**Query Parameters:**

| Parameter | Type | Required | Default | Description |
|-----------|------|----------|---------|-------------|
| category | string | No | - | Filter by category (combat, exploration, social, crafting, collection, economic, event) |
| tier | integer | No | - | Filter by tier (1-5) |
| rarity | string | No | - | Filter by rarity (COMMON, UNCOMMON, RARE, EPIC, LEGENDARY) |
| includeHidden | boolean | No | false | Include hidden/secret achievements |
| page | integer | No | 1 | Page number |
| pageSize | integer | No | 50 | Results per page (max 100) |
| sort | string | No | tier | Sort by: tier, rarity, points, name |
| order | string | No | asc | Sort order: asc, desc |

**Example Request:**

```http
GET /v1/achievements?category=combat&tier=3&page=1&pageSize=20
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
Content-Type: application/json
```

**Response: 200 OK**

```json
{
  "success": true,
  "data": {
    "achievements": [
      {
        "id": "dragon_slayer_gold",
        "name": "Dragon Slayer",
        "description": "Defeat 100 dragons",
        "category": "combat",
        "tier": 3,
        "rarity": "EPIC",
        "points": 100,
        "iconUrl": "https://cdn.bluemarble.design/icons/achievements/dragon_slayer.png",
        "isHidden": false,
        "isRepeatable": false,
        "targetProgress": 100,
        "prerequisites": ["dragon_slayer_silver"],
        "rewards": [
          {
            "type": "currency",
            "amount": 10000
          },
          {
            "type": "title",
            "titleId": "dragonbane"
          },
          {
            "type": "reputation",
            "factionId": "warriors_guild",
            "amount": 500
          }
        ],
        "completionRate": 12.5,
        "createdAt": "2024-01-15T10:00:00Z"
      }
    ],
    "pagination": {
      "page": 1,
      "pageSize": 20,
      "totalResults": 127,
      "totalPages": 7
    }
  },
  "metadata": {
    "requestId": "req-achievement-list-001",
    "timestamp": "2025-01-19T12:00:00Z",
    "version": "1.0"
  }
}
```

**Error Responses:**

- `400 Bad Request`: Invalid query parameters
- `401 Unauthorized`: Missing or invalid authentication
- `429 Too Many Requests`: Rate limit exceeded

---

### Get Achievement Details

Retrieve detailed information about a specific achievement.

**Endpoint:** `GET /achievements/{achievementId}`

**Path Parameters:**

| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| achievementId | string | Yes | Unique achievement identifier |

**Example Request:**

```http
GET /v1/achievements/dragon_slayer_gold
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

**Response: 200 OK**

```json
{
  "success": true,
  "data": {
    "achievement": {
      "id": "dragon_slayer_gold",
      "name": "Dragon Slayer",
      "description": "Defeat 100 dragons to prove your mastery over these legendary creatures",
      "category": "combat",
      "tier": 3,
      "rarity": "EPIC",
      "points": 100,
      "iconUrl": "https://cdn.bluemarble.design/icons/achievements/dragon_slayer.png",
      "isHidden": false,
      "isRepeatable": false,
      "cooldownDays": null,
      "targetProgress": 100,
      "criteria": {
        "type": "kill_enemy",
        "enemyType": "dragon",
        "count": 100
      },
      "prerequisites": ["dragon_slayer_silver"],
      "rewards": [
        {
          "type": "currency",
          "currencyType": "gold",
          "amount": 10000
        },
        {
          "type": "title",
          "titleId": "dragonbane",
          "titleName": "Dragonbane"
        },
        {
          "type": "reputation",
          "factionId": "warriors_guild",
          "factionName": "Warriors Guild",
          "amount": 500
        },
        {
          "type": "stat_bonus",
          "stat": "dragon_damage",
          "value": 5,
          "unit": "percent"
        }
      ],
      "createdAt": "2024-01-15T10:00:00Z"
    },
    "statistics": {
      "globalCompletionRate": 12.5,
      "totalCompletions": 15238,
      "averageCompletionTime": 2592000,
      "recentCompletions": [
        {
          "characterId": "char-456",
          "characterName": "SkyWarrior",
          "completedAt": "2025-01-19T11:45:00Z"
        }
      ]
    }
  },
  "metadata": {
    "requestId": "req-achievement-detail-002",
    "timestamp": "2025-01-19T12:00:00Z",
    "version": "1.0"
  }
}
```

**Error Responses:**

- `404 Not Found`: Achievement doesn't exist
- `401 Unauthorized`: Missing or invalid authentication

---

### Get Player Achievements

Retrieve all achievements for a specific player, including progress.

**Endpoint:** `GET /players/{playerId}/achievements`

**Path Parameters:**

| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| playerId | string | Yes | Player/Character identifier |

**Query Parameters:**

| Parameter | Type | Required | Default | Description |
|-----------|------|----------|---------|-------------|
| status | string | No | all | Filter by status: completed, in_progress, locked, all |
| category | string | No | - | Filter by achievement category |
| sort | string | No | progress | Sort by: progress, points, completed_date, name |

**Example Request:**

```http
GET /v1/players/char-123/achievements?status=in_progress
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

**Response: 200 OK**

```json
{
  "success": true,
  "data": {
    "playerId": "char-123",
    "summary": {
      "totalAchievements": 487,
      "completedCount": 123,
      "inProgressCount": 45,
      "lockedCount": 319,
      "achievementPoints": 2350,
      "completionPercentage": 25.3,
      "globalRank": 1428
    },
    "achievements": {
      "completed": [
        {
          "id": "first_kill",
          "name": "First Blood",
          "category": "combat",
          "tier": 1,
          "points": 10,
          "completedAt": "2024-12-15T08:30:00Z"
        }
      ],
      "inProgress": [
        {
          "id": "dragon_slayer_gold",
          "name": "Dragon Slayer",
          "category": "combat",
          "tier": 3,
          "points": 100,
          "currentProgress": 67,
          "targetProgress": 100,
          "progressPercentage": 67.0,
          "estimatedCompletion": "2025-02-15T00:00:00Z",
          "startedAt": "2024-11-01T10:00:00Z",
          "lastUpdate": "2025-01-19T10:30:00Z"
        }
      ],
      "locked": [
        {
          "id": "dragon_slayer_platinum",
          "name": "Dragon Master",
          "category": "combat",
          "tier": 4,
          "points": 250,
          "isHidden": false,
          "prerequisites": ["dragon_slayer_gold"]
        }
      ]
    }
  },
  "metadata": {
    "requestId": "req-player-achievements-003",
    "timestamp": "2025-01-19T12:00:00Z",
    "version": "1.0"
  }
}
```

**Error Responses:**

- `403 Forbidden`: Not authorized to view this player's achievements
- `404 Not Found`: Player doesn't exist

---

### Get Achievement Progress

Get detailed progress for a specific achievement.

**Endpoint:** `GET /players/{playerId}/achievements/{achievementId}/progress`

**Example Request:**

```http
GET /v1/players/char-123/achievements/dragon_slayer_gold/progress
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

**Response: 200 OK**

```json
{
  "success": true,
  "data": {
    "achievement": {
      "id": "dragon_slayer_gold",
      "name": "Dragon Slayer",
      "targetProgress": 100
    },
    "progress": {
      "currentProgress": 67,
      "progressPercentage": 67.0,
      "completed": false,
      "startedAt": "2024-11-01T10:00:00Z",
      "lastUpdate": "2025-01-19T10:30:00Z",
      "estimatedCompletion": "2025-02-15T00:00:00Z",
      "breakdown": {
        "red_dragons": 25,
        "blue_dragons": 18,
        "green_dragons": 15,
        "black_dragons": 9
      }
    },
    "recentActivity": [
      {
        "timestamp": "2025-01-19T10:30:00Z",
        "progressIncrement": 1,
        "details": "Defeated Ancient Red Dragon"
      },
      {
        "timestamp": "2025-01-18T15:20:00Z",
        "progressIncrement": 2,
        "details": "Defeated 2 Blue Dragons"
      }
    ]
  },
  "metadata": {
    "requestId": "req-achievement-progress-004",
    "timestamp": "2025-01-19T12:00:00Z",
    "version": "1.0"
  }
}
```

---

## Reputation Endpoints

### Get Player Reputation Summary

Retrieve all reputation information for a player.

**Endpoint:** `GET /players/{playerId}/reputation`

**Example Request:**

```http
GET /v1/players/char-123/reputation
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

**Response: 200 OK**

```json
{
  "success": true,
  "data": {
    "playerId": "char-123",
    "trustScore": {
      "overallScore": 687,
      "tier": "TRUSTED",
      "tierProgress": 87,
      "nextTier": "EXEMPLARY",
      "nextTierThreshold": 800,
      "components": {
        "trading": {
          "score": 175,
          "max": 200,
          "percentage": 87.5
        },
        "questing": {
          "score": 145,
          "max": 200,
          "percentage": 72.5
        },
        "guild": {
          "score": 156,
          "max": 200,
          "percentage": 78.0
        },
        "reports": {
          "score": 200,
          "max": 200,
          "percentage": 100.0
        },
        "helpfulness": {
          "score": 111,
          "max": 200,
          "percentage": 55.5
        }
      },
      "benefits": [
        "Premium market access",
        "25 listing slots",
        "10% fee reduction",
        "Can create auctions",
        "Priority support"
      ],
      "accountAgeDays": 234,
      "accountAgeMultiplier": 1.064,
      "lastCalculated": "2025-01-19T11:00:00Z"
    },
    "factionReputation": [
      {
        "factionId": "warriors_guild",
        "factionName": "Warriors Guild",
        "reputation": 7850,
        "reputationLevel": "REVERED",
        "nextLevel": "EXALTED",
        "nextLevelThreshold": 10000,
        "progressToNextLevel": 78.5
      },
      {
        "factionId": "merchants_consortium",
        "factionName": "Merchants Consortium",
        "reputation": 2340,
        "reputationLevel": "FRIENDLY",
        "nextLevel": "HONORED",
        "nextLevelThreshold": 3000,
        "progressToNextLevel": 51.6
      }
    ],
    "summary": {
      "totalFactions": 12,
      "exaltedCount": 0,
      "reveredCount": 1,
      "honoredCount": 2,
      "friendlyCount": 5,
      "neutralCount": 3,
      "unfriendlyCount": 1,
      "hostileCount": 0,
      "hatedCount": 0,
      "highestReputation": {
        "faction": "Warriors Guild",
        "reputation": 7850
      },
      "lowestReputation": {
        "faction": "Thieves Guild",
        "reputation": -1200
      }
    }
  },
  "metadata": {
    "requestId": "req-reputation-summary-005",
    "timestamp": "2025-01-19T12:00:00Z",
    "version": "1.0"
  }
}
```

---

### Get Faction Reputation Details

Get detailed reputation information for a specific faction.

**Endpoint:** `GET /players/{playerId}/reputation/faction/{factionId}`

**Example Request:**

```http
GET /v1/players/char-123/reputation/faction/warriors_guild
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

**Response: 200 OK**

```json
{
  "success": true,
  "data": {
    "playerId": "char-123",
    "faction": {
      "id": "warriors_guild",
      "name": "Warriors Guild",
      "description": "Elite organization of warrior champions",
      "iconUrl": "https://cdn.bluemarble.design/icons/factions/warriors_guild.png",
      "type": "major"
    },
    "reputation": {
      "current": 7850,
      "level": "REVERED",
      "levelRange": {
        "min": 6000,
        "max": 9999
      },
      "nextLevel": "EXALTED",
      "nextLevelThreshold": 10000,
      "progressToNextLevel": 78.5,
      "progressNeeded": 2150
    },
    "benefits": {
      "current": [
        "15% vendor discount",
        "Elite faction quests available",
        "Rare mount: War Horse",
        "Faction ability: Battle Cry",
        "Access to Guild Armory",
        "Special title: Revered Champion"
      ],
      "nextLevel": [
        "20% vendor discount",
        "Legendary equipment access",
        "Unique mount: Legendary War Stallion",
        "Champion title and insignia",
        "Vote in Guild decisions",
        "Access to Champions Hall"
      ]
    },
    "history": [
      {
        "timestamp": "2025-01-19T08:00:00Z",
        "changeAmount": 100,
        "newReputation": 7850,
        "reason": "Completed elite quest: Dragon's Bane",
        "eventType": "quest"
      },
      {
        "timestamp": "2025-01-18T15:30:00Z",
        "changeAmount": 50,
        "newReputation": 7750,
        "reason": "Participated in faction defense event",
        "eventType": "event"
      }
    ],
    "relatedFactions": [
      {
        "factionId": "royal_guard",
        "factionName": "Royal Guard",
        "relationship": "ALLIED",
        "currentReputation": 2800,
        "reputationLevel": "FRIENDLY"
      },
      {
        "factionId": "shadow_clan",
        "factionName": "Shadow Clan",
        "relationship": "HOSTILE",
        "currentReputation": -800,
        "reputationLevel": "UNFRIENDLY"
      }
    ]
  },
  "metadata": {
    "requestId": "req-faction-reputation-006",
    "timestamp": "2025-01-19T12:00:00Z",
    "version": "1.0"
  }
}
```

---

### Get Trust Score Details

Get detailed breakdown of player trust score.

**Endpoint:** `GET /players/{playerId}/trust-score`

**Example Request:**

```http
GET /v1/players/char-123/trust-score
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

**Response: 200 OK**

```json
{
  "success": true,
  "data": {
    "playerId": "char-123",
    "trustScore": {
      "overall": 687,
      "tier": "TRUSTED",
      "tierThresholds": {
        "current": { "min": 600, "max": 799 },
        "next": { "min": 800, "max": 1000 }
      },
      "progressToNextTier": 87
    },
    "components": {
      "trading": {
        "score": 175,
        "maxScore": 200,
        "percentage": 87.5,
        "stats": {
          "totalTrades": 347,
          "successfulTrades": 342,
          "successRate": 98.6,
          "totalTradeValue": 456789,
          "averageTradeValue": 1316,
          "positiveFeedback": 312,
          "negativeFeedback": 3
        },
        "calculation": "Based on success rate (98.6%) and volume bonus"
      },
      "questing": {
        "score": 145,
        "maxScore": 200,
        "percentage": 72.5,
        "stats": {
          "questsAccepted": 234,
          "questsCompleted": 208,
          "completionRate": 88.9,
          "groupQuestsHelped": 67,
          "questsAbandoned": 12
        },
        "calculation": "Based on 88.9% completion rate"
      },
      "guild": {
        "score": 156,
        "maxScore": 200,
        "percentage": 78.0,
        "stats": {
          "currentGuild": "Dragon Slayers",
          "guildRank": "Officer",
          "contributionScore": 12450,
          "guildAverageContribution": 8900,
          "relativeContribution": 139.9,
          "eventsParticipated": 45
        },
        "calculation": "139.9% of guild average contribution"
      },
      "communityReports": {
        "score": 200,
        "maxScore": 200,
        "percentage": 100.0,
        "stats": {
          "totalReports": 0,
          "verifiedReports": 0,
          "unverifiedReports": 0,
          "appealedReports": 0
        },
        "calculation": "No verified reports (max score)"
      },
      "helpfulness": {
        "score": 111,
        "maxScore": 200,
        "percentage": 55.5,
        "stats": {
          "mentoredPlayers": 8,
          "questsHelped": 67,
          "resourcesShared": 2340,
          "positiveFeedback": 45,
          "tutorialsCompleted": 3
        },
        "calculation": "Mentoring (80) + Quest help (67) + Resources (23) + Feedback (45) capped at 200"
      }
    },
    "modifiers": {
      "accountAge": {
        "days": 234,
        "years": 0.64,
        "multiplier": 1.064,
        "bonus": 44
      }
    },
    "benefits": [
      {
        "benefit": "Premium market access",
        "description": "Access to advanced marketplace features"
      },
      {
        "benefit": "25 listing slots",
        "description": "Increased marketplace listing capacity"
      },
      {
        "benefit": "10% fee reduction",
        "description": "Reduced transaction and listing fees"
      },
      {
        "benefit": "Premium listings",
        "description": "Can create featured marketplace listings"
      },
      {
        "benefit": "Priority support",
        "description": "Higher priority in customer support queue"
      }
    ],
    "penalties": [],
    "history": [
      {
        "timestamp": "2025-01-19T10:00:00Z",
        "scoreChange": 2,
        "newScore": 687,
        "reason": "Successful trade completed",
        "component": "trading"
      }
    ],
    "lastCalculated": "2025-01-19T11:00:00Z"
  },
  "metadata": {
    "requestId": "req-trust-score-007",
    "timestamp": "2025-01-19T12:00:00Z",
    "version": "1.0"
  }
}
```

---

### List Factions

Get a list of all factions in the game.

**Endpoint:** `GET /factions`

**Query Parameters:**

| Parameter | Type | Required | Default | Description |
|-----------|------|----------|---------|-------------|
| type | string | No | - | Filter by faction type (major, minor, guild, organization) |
| playerId | string | No | - | Include player's reputation with each faction |
| page | integer | No | 1 | Page number |
| pageSize | integer | No | 50 | Results per page |

**Example Request:**

```http
GET /v1/factions?type=major&playerId=char-123
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

**Response: 200 OK**

```json
{
  "success": true,
  "data": {
    "factions": [
      {
        "id": "warriors_guild",
        "name": "Warriors Guild",
        "description": "Elite organization of warrior champions",
        "type": "major",
        "iconUrl": "https://cdn.bluemarble.design/icons/factions/warriors_guild.png",
        "playerReputation": 7850,
        "playerReputationLevel": "REVERED",
        "relationships": [
          {
            "factionId": "royal_guard",
            "factionName": "Royal Guard",
            "relationship": "ALLIED"
          },
          {
            "factionId": "shadow_clan",
            "factionName": "Shadow Clan",
            "relationship": "HOSTILE"
          }
        ]
      }
    ],
    "pagination": {
      "page": 1,
      "pageSize": 50,
      "totalResults": 12,
      "totalPages": 1
    }
  },
  "metadata": {
    "requestId": "req-factions-list-008",
    "timestamp": "2025-01-19T12:00:00Z",
    "version": "1.0"
  }
}
```

---

## Reward Endpoints

### Get Available Rewards

Get rewards available for claiming based on achievements and reputation.

**Endpoint:** `GET /players/{playerId}/rewards/available`

**Example Request:**

```http
GET /v1/players/char-123/rewards/available
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

**Response: 200 OK**

```json
{
  "success": true,
  "data": {
    "playerId": "char-123",
    "rewards": [
      {
        "rewardId": "reward-001",
        "source": "achievement",
        "sourceId": "dragon_slayer_gold",
        "sourceName": "Dragon Slayer",
        "availableAt": "2025-01-19T10:30:00Z",
        "expiresAt": "2025-02-19T10:30:00Z",
        "rewards": [
          {
            "type": "currency",
            "currencyType": "gold",
            "amount": 10000
          },
          {
            "type": "title",
            "titleId": "dragonbane",
            "titleName": "Dragonbane"
          }
        ]
      }
    ],
    "totalRewards": 3
  },
  "metadata": {
    "requestId": "req-rewards-available-009",
    "timestamp": "2025-01-19T12:00:00Z",
    "version": "1.0"
  }
}
```

---

### Claim Reward

Claim a pending reward.

**Endpoint:** `POST /players/{playerId}/rewards/{rewardId}/claim`

**Example Request:**

```http
POST /v1/players/char-123/rewards/reward-001/claim
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
Content-Type: application/json
```

**Response: 200 OK**

```json
{
  "success": true,
  "data": {
    "rewardId": "reward-001",
    "claimedAt": "2025-01-19T12:00:00Z",
    "rewards": [
      {
        "type": "currency",
        "currencyType": "gold",
        "amount": 10000,
        "newBalance": 56789
      },
      {
        "type": "title",
        "titleId": "dragonbane",
        "titleName": "Dragonbane",
        "unlocked": true
      }
    ],
    "message": "Rewards successfully claimed!"
  },
  "metadata": {
    "requestId": "req-reward-claim-010",
    "timestamp": "2025-01-19T12:00:00Z",
    "version": "1.0"
  }
}
```

**Error Responses:**

- `404 Not Found`: Reward doesn't exist or already claimed
- `410 Gone`: Reward has expired
- `400 Bad Request`: Reward claim requirements not met

---

## Leaderboard Endpoints

### Achievement Leaderboard

Get top players by achievement points.

**Endpoint:** `GET /leaderboard/achievements`

**Query Parameters:**

| Parameter | Type | Required | Default | Description |
|-----------|------|----------|---------|-------------|
| timeframe | string | No | all-time | Timeframe: daily, weekly, monthly, all-time |
| category | string | No | - | Filter by achievement category |
| limit | integer | No | 100 | Number of results (max 100) |

**Example Request:**

```http
GET /v1/leaderboard/achievements?timeframe=monthly&limit=10
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

**Response: 200 OK**

```json
{
  "success": true,
  "data": {
    "leaderboard": [
      {
        "rank": 1,
        "playerId": "char-456",
        "playerName": "AchievementHunter",
        "achievementPoints": 8950,
        "achievementsCompleted": 423,
        "recentAchievement": {
          "id": "legendary_collector",
          "name": "Legendary Collector",
          "completedAt": "2025-01-18T14:30:00Z"
        }
      }
    ],
    "timeframe": "monthly",
    "generatedAt": "2025-01-19T12:00:00Z"
  },
  "metadata": {
    "requestId": "req-leaderboard-achievements-011",
    "timestamp": "2025-01-19T12:00:00Z",
    "version": "1.0"
  }
}
```

---

## WebSocket Events

For real-time achievement and reputation updates, clients can subscribe to WebSocket events.

### Achievement Progress Update

**Event:** `achievement.progress`

```json
{
  "event": "achievement.progress",
  "timestamp": "2025-01-19T12:00:00Z",
  "data": {
    "playerId": "char-123",
    "achievementId": "dragon_slayer_gold",
    "currentProgress": 68,
    "targetProgress": 100,
    "progressIncrement": 1,
    "message": "Defeated Ancient Red Dragon"
  }
}
```

### Achievement Completed

**Event:** `achievement.completed`

```json
{
  "event": "achievement.completed",
  "timestamp": "2025-01-19T12:00:00Z",
  "data": {
    "playerId": "char-123",
    "achievement": {
      "id": "dragon_slayer_gold",
      "name": "Dragon Slayer",
      "tier": 3,
      "points": 100
    },
    "rewards": [
      {
        "type": "currency",
        "amount": 10000
      }
    ],
    "globalMessage": "AchievementHunter has earned the achievement: Dragon Slayer!"
  }
}
```

### Reputation Changed

**Event:** `reputation.changed`

```json
{
  "event": "reputation.changed",
  "timestamp": "2025-01-19T12:00:00Z",
  "data": {
    "playerId": "char-123",
    "factionId": "warriors_guild",
    "factionName": "Warriors Guild",
    "oldReputation": 7750,
    "newReputation": 7850,
    "change": 100,
    "reason": "Completed elite quest: Dragon's Bane",
    "levelChanged": false,
    "currentLevel": "REVERED"
  }
}
```

### Trust Score Updated

**Event:** `trust.updated`

```json
{
  "event": "trust.updated",
  "timestamp": "2025-01-19T12:00:00Z",
  "data": {
    "playerId": "char-123",
    "oldScore": 685,
    "newScore": 687,
    "change": 2,
    "tier": "TRUSTED",
    "tierChanged": false,
    "reason": "Successful trade completed"
  }
}
```

---

## Rate Limiting

All endpoints are subject to rate limiting:

| Endpoint Type | Rate Limit | Window |
|---------------|------------|--------|
| GET endpoints | 100 requests | 1 minute |
| POST endpoints | 20 requests | 1 minute |
| Leaderboards | 10 requests | 1 minute |

Rate limit headers are included in responses:

```http
X-RateLimit-Limit: 100
X-RateLimit-Remaining: 95
X-RateLimit-Reset: 1642593600
```

---

## Error Codes

| Code | HTTP Status | Description |
|------|-------------|-------------|
| ACHIEVEMENT_NOT_FOUND | 404 | Achievement doesn't exist |
| ACHIEVEMENT_LOCKED | 403 | Prerequisites not met |
| ACHIEVEMENT_ALREADY_COMPLETED | 400 | Achievement already claimed |
| FACTION_NOT_FOUND | 404 | Faction doesn't exist |
| REPUTATION_LIMIT_REACHED | 429 | Daily reputation gain limit reached |
| REWARD_EXPIRED | 410 | Reward claim period has passed |
| REWARD_NOT_AVAILABLE | 400 | Reward not ready to claim |
| TRUST_SCORE_RECALCULATION_PENDING | 503 | Trust score update in progress |
| INVALID_FACTION_RELATIONSHIP | 400 | Invalid faction relationship data |

---

## Security Considerations

### Authorization

- Players can only view their own detailed achievement progress
- Admin roles required for:
  - Modifying achievements
  - Manual reputation adjustments
  - Trust score overrides
  - Viewing sensitive player data

### Data Privacy

- Achievement progress is private by default
- Players can opt-in to public leaderboards
- Trust scores visible only to authorized systems
- Reputation history sanitized in public APIs

### Anti-Exploit Measures

- Rate limiting on reputation-gaining actions
- Automated detection of suspicious patterns
- Cooldowns on repeatable achievements
- Network analysis for collusion detection

---

## Testing

### Test Endpoints

Test endpoints available in development and staging environments:

```
POST /v1/test/achievements/{playerId}/complete/{achievementId}
POST /v1/test/reputation/{playerId}/set/{factionId}
POST /v1/test/trust-score/{playerId}/recalculate
```

### Mock Data

Mock achievement and reputation data available for integration testing.

---

## Version History

| Version | Date | Author | Changes |
|---------|------|--------|---------|
| 1.0 | 2025-01-19 | Systems Team | Initial API specification |

---

## References

- [Achievement and Reputation System Design](achievement-reputation-system.md)
- [API Specifications and Protocols](api-specifications.md)
- [Database Schema Design](database-schema-design.md)
- [WebSocket Protocol Documentation](websocket-protocol.md)
