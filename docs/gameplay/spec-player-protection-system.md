# Player Protection System Feature Specification

**Document Type:** Feature Specification  
**Version:** 1.0  
**Author:** BlueMarble Design Team  
**Date:** 2025-10-06  
**Status:** Approved  
**Epic/Theme:** Player Security & Asset Protection  
**Priority:** High

## Executive Summary

The Player Protection System introduces comprehensive asset and resource protection mechanisms that
allow players to secure their possessions through active patrolling, hired guards, and intelligent
threat detection. This system emphasizes player agency through time investment rather than currency
expenditure, offering the highest protection effectiveness (90%) in the game while maintaining
economic balance and preventing exploitation.

## Feature Overview

### Problem Statement

Players in BlueMarble need reliable methods to protect their assets (resources, buildings,
territories, mining operations) from theft, raids, and hostile actions. Current systems lack:

- Active protection mechanisms that don't require constant player presence
- Economic balance between protection costs and asset value
- Intelligence-based NPC behavior for guard selection
- Flexible patrol patterns for various asset configurations
- Real-time threat detection and response

### Solution Summary

The Player Protection System provides multiple layers of defense through:

1. **Self-Guard System:** Players can patrol their own assets with configurable routines
2. **Hired Guard System:** NPCs or other players can be employed as guards with smart job selection
3. **Zone Patrol:** Circular/rectangular area protection with multiple patterns
4. **Path Patrol:** Custom waypoint routes connecting multiple assets
5. **Threat Detection:** Real-time alerts within configurable alert radius
6. **Economic Measures:** Payment-based protection to prevent exploitation

### User Stories

- As a resource miner, I want to patrol my mining operation so that I can prevent theft while
  actively working
- As a territory owner, I want to hire NPC guards so that my assets are protected when I'm offline
- As an NPC, I want to evaluate guard job offers so that I can select the most profitable
  contracts within traveling distance
- As a player, I want to create custom patrol routes so that I can efficiently protect multiple
  scattered assets
- As a builder, I want to protect my construction sites so that resources aren't stolen during
  development
- As a trader, I want protection during resource transport so that I can safely move valuable goods

## Detailed Requirements

### Functional Requirements

1. **Self-Guard System**
   - Description: Players can configure and execute protection routines for their own assets
   - Acceptance Criteria:
     - [ ] Player can enter patrol mode while remaining in-game
     - [ ] Protection effectiveness is 90% for active patrols
     - [ ] No currency cost for self-guard operations
     - [ ] Player character physically moves through patrol area
     - [ ] Real-time position updates visible to player and nearby entities
     - [ ] Patrol can be configured with multiple patterns and parameters

2. **NPC Guard Hiring System**
   - Description: Players can hire NPCs as guards with intelligent job selection mechanics
   - Acceptance Criteria:
     - [ ] NPCs evaluate job offers based on payment/distance ratio
     - [ ] NPCs only accept jobs within 24h walking distance (~50km)
     - [ ] Job selection algorithm prioritizes best-paid nearby positions
     - [ ] Guards automatically travel to assigned patrol location
     - [ ] Guard contracts specify duration, payment, and patrol parameters
     - [ ] Guards maintain 90% protection effectiveness when active

3. **Zone Patrol Mechanics**
   - Description: Protect circular or rectangular areas with configurable patterns
   - Acceptance Criteria:
     - [ ] Support circular zone definition with center point and radius
     - [ ] Support rectangular zone definition with corner coordinates
     - [ ] Perimeter patrol pattern follows zone boundary
     - [ ] Random patrol pattern covers zone with unpredictable movement
     - [ ] Spiral patrol pattern moves from center outward or inward
     - [ ] Real-time position tracking updates every game tick
     - [ ] Zone visualization for patrol configuration

4. **Path Patrol Mechanics**
   - Description: Follow custom waypoint routes connecting multiple assets
   - Acceptance Criteria:
     - [ ] Players can define unlimited waypoints
     - [ ] Waypoints can be connected in specific order
     - [ ] Loop mode returns to start after completing route
     - [ ] One-way mode stops at final waypoint
     - [ ] Patrol speed is configurable per route
     - [ ] Multiple assets can be included in single patrol path
     - [ ] Path visualization during configuration

5. **Threat Detection System**
   - Description: Real-time monitoring and alerting for hostile actions
   - Acceptance Criteria:
     - [ ] Configurable alert radius around patrol position
     - [ ] Detection of theft attempts within alert radius
     - [ ] Detection of hostile player actions
     - [ ] Detection of enemy NPC presence
     - [ ] Alert notifications sent to player/guard
     - [ ] Threat level assessment (low, medium, high, critical)
     - [ ] Automatic response protocols based on threat level

6. **Protection Types**
   - Description: Multiple protection methodologies for different scenarios
   - Acceptance Criteria:
     - [ ] Self-guard: Player actively patrols their assets
     - [ ] Hired guard: NPC or player performs patrol duty
     - [ ] Automated defense: Stationary defense mechanisms (turrets, traps)
     - [ ] Diplomatic: Alliance-based protection agreements
     - [ ] All types achieve 90% effectiveness when properly configured

7. **Resource Management Integration**
   - Description: Protection applies to all resource-based activities
   - Acceptance Criteria:
     - [ ] Mining operation protection
     - [ ] Building site protection
     - [ ] Terraforming area protection
     - [ ] Trade route protection
     - [ ] Resource storage protection
     - [ ] Agricultural plot protection

8. **Economic Anti-Exploitation Measures**
   - Description: Payment-based systems prevent abuse and maintain balance
   - Acceptance Criteria:
     - [ ] NPC guards require competitive payment rates
     - [ ] Payment scales with distance and contract duration
     - [ ] Minimum payment thresholds prevent spam contracts
     - [ ] Maximum payment caps prevent economic manipulation
     - [ ] Contract disputes resolved through arbitration system
     - [ ] Payment held in escrow until contract completion

### Non-Functional Requirements

- **Performance:** Patrol position updates must process within 100ms per entity
- **Scalability:** System must support 1000+ simultaneous patrol routes per server
- **Security:** Protection calculations performed server-side to prevent cheating
- **Accessibility:** Patrol configuration UI supports keyboard and mouse input
- **Compatibility:** Works across all terrain types and weather conditions

## User Experience Design

### User Flow

```text
Player Asset Protection Flow:
1. Player identifies asset requiring protection
2. Opens protection configuration interface
3. Selects protection type (self-guard, hired guard, automated, diplomatic)
4. Configures patrol parameters:
   - Zone type (circular, rectangular, path)
   - Patrol pattern (perimeter, random, spiral, waypoint)
   - Alert radius
   - Duration
5. For hired guards: sets payment and posts job offer
6. For self-guard: begins patrol immediately
7. Receives real-time threat alerts during patrol
8. Reviews protection effectiveness reports

NPC Guard Job Selection Flow:
1. NPC receives notification of available guard jobs
2. Evaluates each job offer within 50km radius
3. Calculates payment/distance ratio for each offer
4. Filters jobs by contract duration and requirements
5. Selects highest-value offer
6. Travels to patrol location
7. Begins patrol duties
8. Receives payment upon contract completion
```

### Interface Requirements

- **Protection Dashboard:** Displays all active protections and their status
- **Patrol Configuration Panel:** Interactive map for defining zones and paths
- **Threat Alert HUD:** Real-time notifications of detected threats
- **Guard Contract Manager:** Interface for posting, accepting, and managing guard jobs
- **Effectiveness Monitor:** Visual representation of protection coverage
- **Guard Position Tracker:** Real-time display of guard locations on map

### Wireframes/Mockups

Wireframes to be created in `/design/wireframes/player-protection-ui.md`

## Technical Considerations

### Architecture Overview

The protection system uses a distributed architecture with:

- **Server-side patrol engine:** Handles all movement calculations and threat detection
- **Client-side visualization:** Renders patrol paths and guard positions
- **Database storage:** Persistent storage of contracts, routes, and effectiveness data
- **Event system:** Real-time threat detection and alert distribution
- **NPC AI integration:** Guard behavior controlled by NPC decision systems

### API Endpoints

| Method | Endpoint | Description | Request | Response |
|--------|----------|-------------|---------|----------|
| POST | `/api/v1/protection/patrol/create` | Create new patrol | `{type, zone}` | `{patrolId}` |
| GET | `/api/v1/protection/patrol/{id}` | Get patrol details | N/A | `{details}` |
| PUT | `/api/v1/protection/patrol/{id}/update` | Update patrol | `{updates}` | `{patrolId}` |
| DELETE | `/api/v1/protection/patrol/{id}` | Cancel patrol | N/A | `{status}` |
| POST | `/api/v1/protection/guard/hire` | Post guard job | `{location}` | `{jobId}` |
| GET | `/api/v1/protection/guard/available` | List guard jobs | `{location}` | `{jobs[]}` |
| POST | `/api/v1/protection/guard/accept` | Accept guard job | `{jobId}` | `{contractId}` |
| GET | `/api/v1/protection/threats` | Get detected threats | `{patrolId}` | `{threats[]}` |
| GET | `/api/v1/protection/effectiveness` | Get metrics | `{assetId}` | `{effectiveness}` |

### Data Model

```typescript
interface PatrolConfiguration {
  id: string;
  playerId: string;
  assetId: string;
  type: 'self-guard' | 'hired-guard' | 'automated' | 'diplomatic';
  zone: CircularZone | RectangularZone | PathZone;
  pattern: 'perimeter' | 'random' | 'spiral' | 'waypoint';
  alertRadius: number; // meters
  duration: number; // seconds
  startTime: timestamp;
  effectiveness: number; // 0-100%
  status: 'active' | 'paused' | 'completed' | 'cancelled';
}

interface CircularZone {
  center: Coordinate;
  radius: number; // meters
}

interface RectangularZone {
  topLeft: Coordinate;
  bottomRight: Coordinate;
}

interface PathZone {
  waypoints: Coordinate[];
  loopMode: boolean;
  speed: number; // meters/second
}

interface GuardContract {
  id: string;
  jobPosterId: string;
  guardId: string | null; // null if unfilled
  patrolId: string;
  payment: number;
  escrowStatus: 'pending' | 'held' | 'released' | 'disputed';
  duration: number; // seconds
  distance: number; // km from guard location
  paymentDistanceRatio: number;
  status: 'open' | 'accepted' | 'active' | 'completed' | 'cancelled';
}

interface ThreatDetection {
  id: string;
  patrolId: string;
  timestamp: timestamp;
  location: Coordinate;
  threatType: 'theft' | 'hostile_player' | 'enemy_npc' | 'raid';
  threatLevel: 'low' | 'medium' | 'high' | 'critical';
  targetId: string;
  resolved: boolean;
}

interface ProtectionEffectiveness {
  assetId: string;
  timeRange: {start: timestamp, end: timestamp};
  overallEffectiveness: number; // 0-100%
  threatsDetected: number;
  threatsRepelled: number;
  incidentReports: ThreatDetection[];
}
```

### Third-Party Integrations

- **Pathfinding Library:** Integration with A* pathfinding for optimal patrol routes
- **Spatial Index:** QuadTree implementation for efficient proximity queries

## Testing Strategy

### Test Cases

1. **Self-Guard Patrol Activation**
   - Preconditions: Player logged in, has asset to protect
   - Steps:
     1. Navigate to asset location
     2. Open protection interface
     3. Configure circular zone with 50m radius
     4. Select perimeter patrol pattern
     5. Activate self-guard mode
   - Expected Result: Player begins patrolling perimeter, position updates in real-time

2. **NPC Guard Job Selection**
   - Preconditions: NPC available for guard work, multiple jobs posted
   - Steps:
     1. Post 3 guard jobs at varying distances and payments
     2. NPC evaluates all jobs within 50km
     3. NPC calculates payment/distance ratios
     4. NPC selects highest ratio job
   - Expected Result: NPC accepts most profitable nearby job, travels to location

3. **Threat Detection and Alert**
   - Preconditions: Active patrol, hostile player approaches
   - Steps:
     1. Establish active patrol with 30m alert radius
     2. Hostile player enters alert radius
     3. Hostile player attempts theft action
   - Expected Result: Threat detected, alert sent to guard, threat level assessed

4. **Zone Patrol Pattern Coverage**
   - Preconditions: Rectangular zone configured with random pattern
   - Steps:
     1. Configure 100m x 100m rectangular zone
     2. Activate random patrol pattern
     3. Track coverage over 5 minute period
   - Expected Result: All areas of zone visited, unpredictable movement pattern

5. **Path Patrol Waypoint Navigation**
   - Preconditions: Multiple assets requiring protection
   - Steps:
     1. Define 5 waypoints connecting scattered assets
     2. Enable loop mode
     3. Set patrol speed to 5 m/s
     4. Activate patrol
   - Expected Result: Guard visits all waypoints in order, returns to start, maintains speed

### Edge Cases

- **Terrain Obstacles:** Patrol path blocked by impassable terrain - system reroutes around obstacles
- **Guard Contract Disputes:** Payment not released after completion - arbitration system resolves
- **Simultaneous Threats:** Multiple threats detected at once - prioritizes by threat level
- **Zone Boundary Changes:** Protected asset relocated - patrol zone automatically updates
- **NPC Guard Offline:** Hired guard disconnects mid-patrol - backup guard assigned or refund issued
- **Maximum Patrol Duration:** Contract exceeds maximum allowed time - system auto-renews with confirmation

### Performance Testing

- **Concurrent Patrols:** Test 1000 simultaneous patrol routes on single server
- **Position Update Rate:** Verify 100ms update cycle maintained under load
- **Threat Detection Latency:** Measure time from threat action to alert delivery
- **Pathfinding Performance:** Test route calculation time for complex paths
- **Database Query Efficiency:** Monitor contract and threat query response times

## Risks and Mitigation

| Risk | Probability | Impact | Mitigation Strategy |
|------|-------------|--------|-------------------|
| NPC pathfinding failures | Medium | High | Fallback routing, path validation |
| Protection exploits | Medium | Critical | Server verification, audit logs |
| Economic imbalance | High | High | Dynamic pricing, balance reviews |
| Performance degradation | Medium | High | Optimize queries, patrol pooling |
| Contract gaming | High | Medium | Rate limiting, min durations |
| Detection false positives | Medium | Low | Tunable sensitivity, ML refinement |

## Dependencies

### Internal Dependencies

- NPC AI behavior system (for guard decision-making)
- Spatial data storage system (for zone queries)
- Player positioning system (for real-time tracking)
- Combat system (for threat assessment)
- Economy system (for guard payments)
- Asset management system (for protection targets)

### External Dependencies

- Pathfinding library for optimal route calculation
- Spatial indexing system for proximity queries

### Blocking Dependencies

- Server-side positioning system must be implemented first
- NPC AI decision framework required for guard behavior

## Success Metrics

### Key Performance Indicators (KPIs)

- **Protection Effectiveness:** Target 90% average across all protection types
- **Guard Contract Fulfillment Rate:** >95% of contracts completed successfully
- **Threat Detection Accuracy:** >80% true positive rate, <5% false positive rate
- **Player Adoption Rate:** 60% of asset-owning players use protection system within 30 days
- **Economic Balance:** Average guard payment remains within 10% of design targets
- **System Performance:** 99.5% uptime, <100ms average response time

### Analytics Requirements

Track the following metrics:

- Number of active patrols per day/week/month
- Distribution of protection types (self-guard vs hired)
- Average contract payment amounts by distance
- Threat detection statistics (types, frequencies, locations)
- Protection effectiveness by zone type and pattern
- Guard NPC job selection patterns
- Player time investment in self-guard activities
- Economic impact of protection system on overall economy

## Timeline and Phases

### Phase 1: Core Patrol Mechanics (Weeks 1-4)

- **Duration:** 4 weeks
- **Deliverables:**
  - Basic patrol engine (zone definition, movement, position tracking)
  - Self-guard system implementation
  - Circular and rectangular zone support
  - Perimeter patrol pattern
- **Success Criteria:** Players can configure and execute basic patrols with real-time tracking

### Phase 2: Advanced Patterns & Threat Detection (Weeks 5-8)

- **Duration:** 4 weeks
- **Deliverables:**
  - Random and spiral patrol patterns
  - Path patrol with waypoint navigation
  - Threat detection system
  - Alert notification system
- **Success Criteria:** Full pattern support, threats detected and reported accurately

### Phase 3: Guard Hiring System (Weeks 9-12)

- **Duration:** 4 weeks
- **Deliverables:**
  - Guard contract system
  - NPC job evaluation algorithm
  - Payment and escrow mechanics
  - Contract management UI
- **Success Criteria:** NPCs successfully evaluate and accept guard contracts

### Phase 4: Polish & Balance (Weeks 13-16)

- **Duration:** 4 weeks
- **Deliverables:**
  - Performance optimization
  - Economic balancing
  - Anti-exploitation measures
  - UI/UX improvements
  - Comprehensive testing
- **Success Criteria:** System achieves all KPI targets, passes all test cases

## Out of Scope

The following features are explicitly NOT included in this initial release:

- Automated turret/trap construction and maintenance
- Diplomatic alliance management interface
- Cross-server protection coordination
- Player-vs-player guard combat system (uses existing combat)
- Guard training and skill progression
- Protection insurance policies
- Territory conquest mechanics
- Siege warfare integration

## Future Considerations

Potential enhancements for future releases:

- **Guard Specializations:** Different guard types (archer, warrior, mage) with unique abilities
- **Protection Tiers:** Bronze/Silver/Gold protection levels with increasing effectiveness
- **Mobile Patrols:** Guards can escort players during resource transport
- **Guard Reputation System:** Performance-based ratings affecting job opportunities
- **Advanced Threat Response:** Automated counter-measures and defensive tactics
- **Protection Analytics Dashboard:** Detailed historical data and trend analysis
- **Multi-Asset Protection Plans:** Bundled protection for multiple assets with discounts
- **Guild Protection Networks:** Shared guard pools for guild members

## Appendices

### Appendix A: Research and References

- [Halo 3 AI: Building a Better Battle]
  (../../research/literature/game-dev-analysis-halo3-building-better-battle.md) -
  Spatial evaluation and cover systems
- [AI for Games (3rd Edition)]
  (../../research/literature/game-dev-analysis-ai-for-games-3rd-edition.md) -
  NPC behavior and state machines
- [MMORPG Development](../../research/literature/game-dev-analysis-mmorpg-development.md) -
  Economy and anti-exploit systems
- [Auction House Systems]
  (../../research/topics/auction-house-systems-local-global-transport.md) -
  Transport security mechanics

### Appendix B: Revision History

| Version | Date | Author | Changes |
|---------|------|--------|---------|
| 1.0 | 2025-10-06 | BlueMarble Design Team | Initial specification |
