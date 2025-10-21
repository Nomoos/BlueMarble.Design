# Player Protection Systems Specification

**Version:** 1.0  
**Date:** 2025-01-06  
**Status:** Draft  
**Owner:** BlueMarble Gameplay Team

## Overview

The Player Protection Systems provide comprehensive security mechanisms for players to guard their assets, territory, and resources in BlueMarble. The system includes personal patrol options, NPC guard hiring, automated defenses, and diplomatic protection agreements.

## System Architecture

### Protection Effectiveness Hierarchy

| Method | Effectiveness | Cost | Time Investment |
|--------|--------------|------|-----------------|
| Personal Patrol | 90% | None | High (active presence) |
| Hired Guards (Players) | 85% | High | None |
| Hired Guards (NPCs) | 75% | Medium | None |
| Automated Defense | 60% | High (initial) | None |
| Diplomatic Protection | 70% | None/Variable | Relationship building |

## Personal Patrol System

### Zone Patrol

Players can personally patrol designated areas around their assets by physically moving through zones.

#### Zone Types

**Circular Zones:**
- Defined by center point and radius
- Radius range: 10m - 500m
- Suitable for point defense (single buildings, mines)

**Rectangular Zones:**
- Defined by corner coordinates
- Maximum area: 1km²
- Suitable for area defense (multiple buildings, farms)

#### Patrol Patterns

**1. Perimeter Patrol**
```javascript
{
  pattern: "perimeter",
  behavior: "walk_edges",
  stopPoints: ["corners", "mid-points"],
  pauseDuration: 5, // seconds at each stop
  effectiveRadius: 50 // meters from patrol path
}
```

**2. Random Patrol**
```javascript
{
  pattern: "random",
  behavior: "random_points_in_zone",
  pointCount: 5, // waypoints per cycle
  minDistance: 20, // meters between points
  effectiveRadius: 40 // meters from current position
}
```

**3. Spiral Patrol**
```javascript
{
  pattern: "spiral",
  behavior: "spiral_from_center",
  loops: 3, // number of spiral loops
  spacing: 15, // meters between loops
  effectiveRadius: 45 // meters from patrol path
}
```

#### Configuration Options

```javascript
{
  zoneType: "circular" | "rectangular",
  patrolPattern: "perimeter" | "random" | "spiral",
  
  // Circular zone specific
  centerPoint: { x: number, y: number },
  radius: number, // 10-500 meters
  
  // Rectangular zone specific
  bounds: {
    topLeft: { x: number, y: number },
    bottomRight: { x: number, y: number }
  },
  
  // Common settings
  walkingSpeed: number, // 1.0-2.0 m/s (default 1.4)
  alertRadius: number, // 50-200 meters
  protectionEffectiveness: 0.90 // 90%
}
```

### Path Patrol

Players can define custom waypoint routes connecting multiple assets.

#### Path Configuration

```javascript
{
  pathId: string,
  name: string,
  waypoints: [
    {
      position: { x: number, y: number },
      assetId: string | null, // asset at this waypoint
      pauseDuration: number, // seconds
      inspectionRequired: boolean
    }
  ],
  
  // Path behavior
  pathType: "loop" | "one-way",
  walkingSpeed: number, // 1.0-2.0 m/s
  
  // Detection settings
  alertRadius: number, // 50-200 meters
  threatDetection: {
    enabled: boolean,
    scanInterval: number, // seconds
    alertOnDetection: boolean
  },
  
  // Statistics
  totalDistance: number, // meters
  estimatedDuration: number, // seconds
  completedCycles: number
}
```

#### Path Mechanics

**Loop Paths:**
- Continuous patrol returning to start
- Ideal for regular inspection routes
- No "direction" preference

**One-Way Paths:**
- Start to end, then teleport/walk back
- Useful for supply routes
- Can include return path as separate waypoints

### Patrol Statistics

Real-time tracking of patrol activities:

```javascript
{
  patrolId: string,
  playerId: string,
  
  // Time metrics
  startTime: timestamp,
  currentDuration: number, // seconds
  totalPatrolTime: number, // seconds (all-time)
  
  // Distance metrics
  distanceCovered: number, // meters (current session)
  totalDistance: number, // meters (all-time)
  
  // Position tracking
  currentPosition: { x: number, y: number },
  lastUpdateTime: timestamp,
  
  // Effectiveness
  threatsDetected: number,
  threatsNeutralized: number,
  assetsSaved: number,
  
  // Status
  active: boolean,
  pattern: string,
  zone: object
}
```

## Hired Guard System

### NPC Guard Behavior

NPCs evaluate guard job opportunities using intelligent decision-making.

#### Job Evaluation Algorithm

```javascript
function evaluateGuardJob(npc, job) {
  const distance = calculateDistance(npc.position, job.location);
  const travelTime = distance / npc.walkingSpeed; // walking speed ~1.4 m/s
  
  // Maximum consideration radius: 24h walking distance
  const MAX_DISTANCE = 24 * 3600 * 1.4; // ~120,960 meters (~121 km)
  
  if (distance > MAX_DISTANCE) {
    return null; // Too far, not considered
  }
  
  // Payment/distance ratio optimization
  const paymentPerHour = job.payment / job.duration;
  const travelHours = travelTime / 3600;
  const totalHours = travelHours + job.duration;
  
  const effectivePayRate = job.payment / totalHours;
  
  return {
    jobId: job.id,
    score: effectivePayRate,
    distance: distance,
    travelTime: travelTime,
    effectivePayRate: effectivePayRate
  };
}
```

#### NPC Behavioral Types

```javascript
const NPC_BEHAVIORS = {
  GUARD: {
    name: "Guard",
    jobPreference: "protection",
    skillset: ["combat", "patrol", "alertness"],
    basePayRequirement: 10, // currency per hour
    loyaltyFactor: 0.8
  },
  
  TRADER: {
    name: "Trader",
    jobPreference: "commerce",
    skillset: ["negotiation", "inventory", "transport"],
    basePayRequirement: 15,
    loyaltyFactor: 0.6
  },
  
  BUILDER: {
    name: "Builder",
    jobPreference: "construction",
    skillset: ["building", "repair", "crafting"],
    basePayRequirement: 12,
    loyaltyFactor: 0.7
  },
  
  EXPLORER: {
    name: "Explorer",
    jobPreference: "discovery",
    skillset: ["navigation", "survival", "mapping"],
    basePayRequirement: 14,
    loyaltyFactor: 0.5
  },
  
  MINER: {
    name: "Miner",
    jobPreference: "extraction",
    skillset: ["mining", "geology", "stamina"],
    basePayRequirement: 11,
    loyaltyFactor: 0.75
  },
  
  TRANSPORTER: {
    name: "Transporter",
    jobPreference: "logistics",
    skillset: ["transport", "inventory", "efficiency"],
    basePayRequirement: 13,
    loyaltyFactor: 0.65
  }
};
```

### Player Guard Hiring

Players can hire other players as guards with negotiated terms.

```javascript
{
  contractId: string,
  employer: playerId,
  guard: playerId,
  
  terms: {
    payment: number, // currency per hour
    duration: number, // hours
    location: { x: number, y: number },
    responsibilities: string[],
    bonuses: {
      threatNeutralized: number,
      perfectShift: number
    }
  },
  
  performance: {
    hoursWorked: number,
    threatsDetected: number,
    threatsNeutralized: number,
    rating: number // 1-5 stars
  }
}
```

## Automated Defense Systems

### Defense Structures

**Watch Towers:**
- Range: 100m
- Effectiveness: 60%
- Auto-attack hostile entities
- Requires ammunition/power

**Defense Turrets:**
- Range: 150m  
- Effectiveness: 65%
- Higher damage, slower fire rate
- Expensive maintenance

**Alarm Systems:**
- Range: 200m
- Alert owner of intrusions
- No direct defense
- Low cost

### Configuration

```javascript
{
  structureId: string,
  type: "watch_tower" | "defense_turret" | "alarm_system",
  position: { x: number, y: number },
  
  settings: {
    enabled: boolean,
    targetingMode: "closest" | "strongest" | "weakest",
    alertOwner: boolean,
    autoRepair: boolean
  },
  
  status: {
    health: number,
    ammunition: number,
    power: number,
    lastMaintenance: timestamp
  }
}
```

## Diplomatic Protection

Alliance-based mutual protection agreements.

### Alliance System

```javascript
{
  allianceId: string,
  name: string,
  members: [
    {
      playerId: string,
      role: "leader" | "officer" | "member",
      joinDate: timestamp
    }
  ],
  
  protectionAgreement: {
    mutualDefense: boolean,
    responseTime: number, // minutes
    minimumGuards: number,
    territoryProtection: boolean
  },
  
  statistics: {
    defensesProvided: number,
    defensesReceived: number,
    successRate: number
  }
}
```

### Protection Effectiveness

- Base: 70%
- +5% per active alliance member within 10km
- +10% if alliance has defensive treaty
- Maximum: 85% (with multiple factors)

## Player Freedom Mechanics

### Mining System

```javascript
{
  mineId: string,
  owner: playerId,
  location: { x: number, y: number },
  
  resources: {
    type: "iron" | "gold" | "crystal" | "coal",
    totalDeposit: number,
    extracted: number,
    quality: number // 1-100
  },
  
  operations: {
    active: boolean,
    workers: number,
    extractionRate: number, // per hour
    efficiency: number // 0-1
  }
}
```

### Building System

Players can construct various structures:

**Building Types:**
- Mines (resource extraction)
- Factories (crafting/processing)
- Storage Facilities (inventory expansion)
- Defense Towers (automated protection)
- Housing (worker accommodation)

```javascript
{
  buildingId: string,
  type: string,
  owner: playerId,
  location: { x: number, y: number },
  
  construction: {
    blueprint: string,
    progress: number, // 0-100%
    requiredResources: object,
    estimatedCompletion: timestamp
  },
  
  functionality: {
    active: boolean,
    capacity: number,
    efficiency: number,
    maintenanceCost: number
  }
}
```

### Trade System

```javascript
{
  tradeId: string,
  seller: playerId,
  buyer: playerId | "npc",
  
  items: [
    {
      itemId: string,
      quantity: number,
      pricePerUnit: number
    }
  ],
  
  terms: {
    totalPrice: number,
    currency: string,
    deliveryLocation: { x: number, y: number },
    deliveryTime: timestamp
  },
  
  status: "pending" | "accepted" | "completed" | "cancelled",
  
  npcParticipation: {
    enabled: boolean,
    autoAcceptPrice: number,
    priceRange: { min: number, max: number }
  }
}
```

## Economic Balance

### Cost Structures

**Personal Patrol:**
- No currency cost
- Time investment only
- Highest effectiveness (90%)

**NPC Guards:**
- 10-15 currency per hour
- Based on distance and job type
- Medium effectiveness (75%)

**Player Guards:**
- Negotiated rates (typically 15-25 per hour)
- Higher reliability
- High effectiveness (85%)

**Automated Defense:**
- Initial cost: 1000-5000 currency
- Maintenance: 50-200 per day
- Lower effectiveness (60-65%)

### Anti-Exploitation Measures

1. **Distance Limits:** NPCs only consider jobs within 24h walking distance
2. **Payment Requirements:** Minimum viable rates prevent exploitation
3. **Performance Tracking:** Guard ratings affect future hire rates
4. **Resource Scarcity:** Limited resources prevent infinite growth
5. **Maintenance Costs:** All systems require upkeep

## Implementation Requirements

### Server-Side

- Position tracking system (player location updates)
- Patrol path validation
- Threat detection algorithms
- NPC AI decision-making engine
- Contract management system
- Real-time statistics collection

### Client-Side

- Interactive zone/path editor
- Real-time patrol visualization
- Threat alert notifications
- Guard hiring interface
- Performance dashboards

### Database Schema

```sql
-- Patrol zones
CREATE TABLE patrol_zones (
  id UUID PRIMARY KEY,
  player_id UUID NOT NULL,
  zone_type VARCHAR(20),
  pattern VARCHAR(20),
  config JSONB,
  created_at TIMESTAMP,
  active BOOLEAN
);

-- Patrol statistics
CREATE TABLE patrol_stats (
  id UUID PRIMARY KEY,
  player_id UUID NOT NULL,
  patrol_id UUID,
  duration INTEGER,
  distance FLOAT,
  threats_detected INTEGER,
  threats_neutralized INTEGER,
  session_start TIMESTAMP
);

-- Guard contracts
CREATE TABLE guard_contracts (
  id UUID PRIMARY KEY,
  employer_id UUID NOT NULL,
  guard_id UUID NOT NULL,
  guard_type VARCHAR(20),
  payment FLOAT,
  duration INTEGER,
  location POINT,
  status VARCHAR(20),
  created_at TIMESTAMP
);
```

## Testing Requirements

### Functional Tests

- Zone patrol pattern execution
- Path waypoint navigation
- NPC job evaluation accuracy
- Guard contract fulfillment
- Threat detection reliability

### Performance Tests

- Position tracking at scale (1000+ concurrent patrols)
- NPC AI decision speed (< 100ms per evaluation)
- Real-time statistics updates (< 1s latency)

### Balance Tests

- Cost/effectiveness ratios
- Exploitation prevention
- Economic sustainability

## Future Enhancements

- Advanced patrol AI (adaptive patterns)
- Squad-based guard operations
- Reputation system for guards
- Enhanced diplomatic mechanics
- Territory control systems

## References

- [Gameplay Systems](./gameplay-systems.md)
- [Economy Systems](../systems/economy-systems.md)
- [NPC AI Research](../../research/literature/game-dev-analysis-ai-for-games-3rd-edition.md)
- [Network Programming](../../research/literature/game-dev-analysis-network-programming-games.md)
