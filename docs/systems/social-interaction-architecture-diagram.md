# Social Interaction System Architecture Diagram

```
┌─────────────────────────────────────────────────────────────────────────────┐
│                    SOCIAL INTERACTION & SETTLEMENT SYSTEM                    │
└─────────────────────────────────────────────────────────────────────────────┘

┌─────────────────────────────────────────────────────────────────────────────┐
│                           CORE INFLUENCE ENGINE                              │
├─────────────────────────────────────────────────────────────────────────────┤
│                                                                              │
│  ┌─────────────────┐      ┌──────────────────┐      ┌───────────────────┐ │
│  │   POLITICAL     │      │    ECONOMIC      │      │     MILITARY      │ │
│  │   INFLUENCE     │      │    INFLUENCE     │      │    INFLUENCE      │ │
│  ├─────────────────┤      ├──────────────────┤      ├───────────────────┤ │
│  │ • Diplomacy     │      │ • Trade Volume   │      │ • PvP Victories   │ │
│  │ • Alliances     │      │ • Settlement Tax │      │ • Defense Success │ │
│  │ • Reputation    │      │ • Market Control │      │ • Army Strength   │ │
│  │ • Governance    │      │ • Resource Nodes │      │ • Fortifications  │ │
│  └─────────────────┘      └──────────────────┘      └───────────────────┘ │
│         ▲                          ▲                          ▲             │
│         │                          │                          │             │
│         └──────────────────────────┴──────────────────────────┘             │
│                                    │                                        │
│                          ┌─────────▼─────────┐                              │
│                          │ InfluenceProfile  │                              │
│                          │   Aggregator      │                              │
│                          └─────────┬─────────┘                              │
└────────────────────────────────────┼─────────────────────────────────────────┘
                                     │
┌────────────────────────────────────▼─────────────────────────────────────────┐
│                            SETTLEMENT SYSTEM                                  │
├───────────────────────────────────────────────────────────────────────────────┤
│                                                                               │
│  ┌──────────┐  ┌──────────┐  ┌──────────┐  ┌──────────┐  ┌──────────────┐ │
│  │ VILLAGE  │  │   TOWN   │  │   CITY   │  │ OUTPOST  │  │ TRADING POST │ │
│  ├──────────┤  ├──────────┤  ├──────────┤  ├──────────┤  ├──────────────┤ │
│  │ Req:     │  │ Req:     │  │ Req:     │  │ Req:     │  │ Req:         │ │
│  │ P: 10    │  │ P: 25    │  │ P: 50    │  │ P: 5     │  │ P: 8         │ │
│  │ E: 5     │  │ E: 20    │  │ E: 40    │  │ E: 3     │  │ E: 25        │ │
│  │ M: 5     │  │ M: 15    │  │ M: 30    │  │ M: 10    │  │ M: 5         │ │
│  │          │  │          │  │          │  │          │  │              │ │
│  │ Pop:     │  │ Pop:     │  │ Pop:     │  │ Pop:     │  │ Pop:         │ │
│  │ 100-500  │  │ 500-2K   │  │ 2K-10K   │  │ 10-100   │  │ 50-300       │ │
│  └──────────┘  └──────────┘  └──────────┘  └──────────┘  └──────────────┘ │
│                                                                               │
│  ┌─────────────────────────────────────────────────────────────────────┐   │
│  │                     SETTLEMENT MANAGER                               │   │
│  ├─────────────────────────────────────────────────────────────────────┤   │
│  │ • Establish Settlement    • Validate Control    • Transfer Control  │   │
│  │ • Population Growth       • Economic Output     • Control Stability │   │
│  │ • Infrastructure Mgmt     • Defense Systems     • Event Handling    │   │
│  └─────────────────────────────────────────────────────────────────────┘   │
└───────────────────────────────────────────────────────────────────────────────┘
                                     │
┌────────────────────────────────────▼─────────────────────────────────────────┐
│                          DIPLOMACY SYSTEM                                     │
├───────────────────────────────────────────────────────────────────────────────┤
│                                                                               │
│  ┌────────────────────────────────────────────────────────────────────────┐ │
│  │              RELATIONSHIP PROGRESSION CONTINUUM                         │ │
│  ├────────────────────────────────────────────────────────────────────────┤ │
│  │                                                                         │ │
│  │  WAR         HOSTILE       RIVAL       NEUTRAL      FRIENDLY    ALLIED │ │
│  │  (-100)      (-75)         (-50)       (0)          (+25)       (+50)  │ │
│  │   ●────────────●────────────●────────────●────────────●──────────●    │ │
│  │   │            │            │            │            │          │     │ │
│  │   │            │            │            │            │          │     │ │
│  │ Full PvP   Hostile     Competition   Standard    Cooperation  Alliance │ │
│  │ Enabled    Actions     Allowed       Relations    Benefits   & Trade  │ │
│  │                                                              Bonuses    │ │
│  └────────────────────────────────────────────────────────────────────────┘ │
│                                                                               │
│  ┌─────────────────────────────────────────────────────────────────────┐   │
│  │                      DIPLOMACY MANAGER                               │   │
│  ├─────────────────────────────────────────────────────────────────────┤   │
│  │ • Create Relationships    • Alliance Proposals   • War Declarations │   │
│  │ • Update Values           • Peace Treaties       • Trade Agreements │   │
│  │ • Track History           • Event Broadcasting   • Dispute Detection│   │
│  └─────────────────────────────────────────────────────────────────────┘   │
└───────────────────────────────────────────────────────────────────────────────┘
                                     │
┌────────────────────────────────────▼─────────────────────────────────────────┐
│                         FEDERATION SYSTEM                                     │
├───────────────────────────────────────────────────────────────────────────────┤
│                                                                               │
│  ┌────────────────┐      ┌────────────────┐      ┌────────────────┐        │
│  │  DEMOCRATIC    │      │   OLIGARCHIC   │      │   AUTOCRATIC   │        │
│  │  GOVERNANCE    │      │   GOVERNANCE   │      │   GOVERNANCE   │        │
│  ├────────────────┤      ├────────────────┤      ├────────────────┤        │
│  │ • All Vote     │      │ • Council Rule │      │ • Single Leader│        │
│  │ • Equal Power  │      │ • Merit-Based  │      │ • Fast Decisions│       │
│  │ • Rotation     │      │ • Contribution │      │ • Centralized  │        │
│  │ • Fairness     │      │ • Balanced     │      │ • Efficient    │        │
│  └────────────────┘      └────────────────┘      └────────────────┘        │
│                                                                               │
│  ┌─────────────────────────────────────────────────────────────────────┐   │
│  │                    FEDERATION BENEFITS                               │   │
│  ├─────────────────────────────────────────────────────────────────────┤   │
│  │ ┌─────────────────┐  ┌─────────────────┐  ┌─────────────────┐     │   │
│  │ │   COLLECTIVE    │  │    ECONOMIC     │  │    STRATEGIC    │     │   │
│  │ │   INFLUENCE     │  │   INTEGRATION   │  │   ADVANTAGES    │     │   │
│  │ ├─────────────────┤  ├─────────────────┤  ├─────────────────┤     │   │
│  │ │ • Pooled Power  │  │ • Free Trade    │  │ • Joint Defense │     │   │
│  │ │ • Higher Tiers  │  │ • Shared        │  │ • Intelligence  │     │   │
│  │ │ • Combined Mil  │  │   Treasury      │  │ • Coordination  │     │   │
│  │ │ • Territory     │  │ • Resources     │  │ • Projects      │     │   │
│  │ └─────────────────┘  └─────────────────┘  └─────────────────┘     │   │
│  └─────────────────────────────────────────────────────────────────────┘   │
└───────────────────────────────────────────────────────────────────────────────┘
                                     │
┌────────────────────────────────────▼─────────────────────────────────────────┐
│                    TERRITORIAL DISPUTE SYSTEM                                 │
├───────────────────────────────────────────────────────────────────────────────┤
│                                                                               │
│  ┌─────────────┐         ┌─────────────┐         ┌─────────────┐           │
│  │ DETECTION   │────────▶│ RESOLUTION  │────────▶│  OUTCOME    │           │
│  ├─────────────┤         ├─────────────┤         ├─────────────┤           │
│  │             │         │             │         │             │           │
│  │ • Overlap   │         │ DIPLOMATIC  │         │ • Territory │           │
│  │   Influence │         │ • Negotiation│        │   Division  │           │
│  │   Areas     │         │ • 48-168hrs │         │ • Exclusive │           │
│  │             │         │ • Treaties  │         │   Control   │           │
│  │ • Competing │         │             │         │ • Joint     │           │
│  │   Claims    │         │ MILITARY    │         │   Control   │           │
│  │             │         │ • Siege     │         │ • Neutral   │           │
│  │ • Threshold │         │ • Battle    │         │   Zone      │           │
│  │   Exceeded  │         │ • Victory   │         │             │           │
│  │             │         │             │         │ • Winner    │           │
│  │             │         │ ECONOMIC    │         │   Takes All │           │
│  │             │         │ • Competition│        │ • Economic  │           │
│  │             │         │ • 1-4 weeks │         │   Winner    │           │
│  │             │         │ • Highest $│          │             │           │
│  └─────────────┘         └─────────────┘         └─────────────┘           │
│                                                                               │
└───────────────────────────────────────────────────────────────────────────────┘

┌───────────────────────────────────────────────────────────────────────────────┐
│                          INTEGRATION LAYER                                    │
├───────────────────────────────────────────────────────────────────────────────┤
│                                                                               │
│  ┌─────────────┐  ┌─────────────┐  ┌─────────────┐  ┌─────────────┐       │
│  │  ECONOMY    │  │   COMBAT    │  │ PROGRESSION │  │   QUESTS    │       │
│  │  SYSTEM     │  │   SYSTEM    │  │   SYSTEM    │  │   SYSTEM    │       │
│  ├─────────────┤  ├─────────────┤  ├─────────────┤  ├─────────────┤       │
│  │ • Tax Income│  │ • Defenses  │  │ • Influence │  │ • Diplomatic│       │
│  │ • Trade     │  │ • Territorial│  │   Rewards   │  │   Missions  │       │
│  │   Bonuses   │  │   PvP       │  │ • Reputation│  │ • Settlement│       │
│  │ • Market    │  │ • Sieges    │  │ • Achievements  │  Tasks     │       │
│  │   Access    │  │             │  │             │  │             │       │
│  └─────────────┘  └─────────────┘  └─────────────┘  └─────────────┘       │
└───────────────────────────────────────────────────────────────────────────────┘

┌───────────────────────────────────────────────────────────────────────────────┐
│                              DATA LAYER                                       │
├───────────────────────────────────────────────────────────────────────────────┤
│                                                                               │
│  ┌──────────────┐  ┌──────────────┐  ┌──────────────┐  ┌──────────────┐   │
│  │ SETTLEMENTS  │  │  INFLUENCE   │  │  DIPLOMATIC  │  │ FEDERATIONS  │   │
│  │   TABLE      │  │   PROFILES   │  │ RELATIONSHIPS│  │    TABLE     │   │
│  └──────────────┘  └──────────────┘  └──────────────┘  └──────────────┘   │
│                                                                               │
│  ┌──────────────┐  ┌──────────────┐  ┌──────────────┐  ┌──────────────┐   │
│  │ TERRITORIAL  │  │   HISTORY    │  │    EVENTS    │  │    CACHE     │   │
│  │  DISPUTES    │  │     LOG      │  │    QUEUE     │  │    LAYER     │   │
│  └──────────────┘  └──────────────┘  └──────────────┘  └──────────────┘   │
└───────────────────────────────────────────────────────────────────────────────┘

┌───────────────────────────────────────────────────────────────────────────────┐
│                              API LAYER                                        │
├───────────────────────────────────────────────────────────────────────────────┤
│                                                                               │
│  REST API                              WebSocket Events                      │
│  ├─ /settlements                       ├─ SettlementChallenged              │
│  ├─ /diplomacy                         ├─ DiplomaticStatusChanged           │
│  ├─ /federations                       ├─ TerritorialDisputeDetected        │
│  ├─ /influence                         ├─ FederationMemberJoined            │
│  └─ /disputes                          └─ ControlTransferred                │
│                                                                               │
└───────────────────────────────────────────────────────────────────────────────┘
```

## System Flow Example: Establishing a Settlement

```
1. PLAYER ACTION
   ▼
2. CHECK INFLUENCE REQUIREMENTS
   ├─ Political ≥ 10? ✓
   ├─ Economic ≥ 5?  ✓
   └─ Military ≥ 5?  ✓
   ▼
3. VALIDATE LOCATION
   ├─ Not occupied? ✓
   ├─ Suitable terrain? ✓
   └─ No conflicts? ✓
   ▼
4. DEDUCT RESOURCES
   ├─ 500 gold
   ├─ 100 wood
   └─ 50 stone
   ▼
5. CREATE SETTLEMENT
   ├─ Assign control
   ├─ Initial population: 120
   ├─ Stability: 50%
   └─ Register in database
   ▼
6. UPDATE INFLUENCE
   ├─ +2 Political for establishment
   └─ Begin daily calculations
   ▼
7. NOTIFY SYSTEMS
   ├─ Economic system: New tax source
   ├─ Diplomacy system: Check nearby entities
   ├─ Event system: Broadcast creation
   └─ Quest system: Update objectives
   ▼
8. SETTLEMENT ACTIVE
   Population grows, economy develops, control maintained
```

## Diplomatic Progression Example

```
NEUTRAL (0)
    │
    ├─ Regular Trading (+1/week)
    ├─ Joint Quests (+3 each)
    └─ Gift Exchange (+2 each)
    │
    ▼
FRIENDLY (+25)
    │
    ├─ Propose Alliance
    ├─ Accept Proposal
    └─ Sign Mutual Defense Pact
    │
    ▼
ALLIED (+50)
    │
    ├─ Form Federation
    ├─ Pool Influence
    └─ Shared Benefits Active
    │
    ▼
FEDERATION MEMBER
    │
    • Free internal trade
    • Collective influence
    • Joint operations
    • Shared territory defense
```
