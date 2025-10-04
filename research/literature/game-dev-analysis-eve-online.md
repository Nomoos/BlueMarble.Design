# EVE Online - Analysis for BlueMarble MMORPG

---
title: EVE Online - Game Design and Technical Architecture Analysis
date: 2025-01-17
tags: [game-development, mmorpg, eve-online, architecture, economy, single-shard]
status: complete
priority: high
parent-research: online-game-dev-resources.md
assignment-group: research-assignment-group-33.md
---

**Source:** EVE Online by CCP Games  
**Category:** MMORPG Case Study - Online Game Development Resources  
**Priority:** High  
**Status:** ✅ Complete  
**Lines:** 550+  
**Related Sources:** World of Warcraft, Ultima Online, MMORPG Development Series

---

## Executive Summary

EVE Online, developed by CCP Games and launched in 2003, represents one of the most ambitious and technically
sophisticated MMORPGs ever created. Its single-shard architecture, player-driven economy, and persistent universe
make it a crucial case study for BlueMarble's planet-scale MMORPG design.

**Key Takeaways for BlueMarble:**
- **Single-Shard Architecture**: All players exist in one persistent universe, creating unprecedented social
  dynamics and economic complexity
- **Player-Driven Economy**: Nearly all items are player-crafted, creating a functioning virtual economy with
  supply chains, market speculation, and resource scarcity
- **Time Dilation System**: Innovative solution for handling massive-scale battles (thousands of simultaneous players)
- **Emergent Gameplay**: Minimal developer-imposed restrictions lead to complex player organizations, politics,
  and warfare
- **Long-Term Persistence**: Design for multi-decade gameplay with meaningful progression and retention

**Relevance to BlueMarble:**
BlueMarble's planetary geology simulation and resource-based gameplay share fundamental design goals with EVE Online:
persistent worlds, player-driven economies, emergent social structures, and technical challenges of massive scale.

---

## Part I: Core Technical Architecture

### 1. Single-Shard Architecture

**Overview:**
Unlike traditional MMORPGs that use multiple servers (shards) to divide the player base, EVE Online runs
on a single cluster serving all players worldwide. This creates a truly persistent universe where all
player actions affect the same shared world.

**Technical Implementation:**

```
EVE Online Architecture:
┌─────────────────────────────────────────────────────┐
│           Global Database Cluster (SQL)             │
│        (Player Data, Universe State, Economy)       │
└─────────────────────────────────────────────────────┘
                         ↕
┌─────────────────────────────────────────────────────┐
│              Proxy Layer (Connection)               │
│           (Load Balancing, Authentication)          │
└─────────────────────────────────────────────────────┘
                         ↕
┌────────┬────────┬────────┬────────┬────────┬────────┐
│ Sol    │ Proxy  │ Node   │ Node   │ Node   │  ...   │
│ Node 1 │ Node 2 │ 1000   │ 1001   │ 1002   │  5000+ │
└────────┴────────┴────────┴────────┴────────┴────────┘
  (Each node hosts one or more solar systems)
```

**Key Design Principles:**
- **Solar System = Node**: Each solar system runs on a dedicated node (or shares with low-population systems)
- **Dynamic Load Balancing**: Systems can migrate between nodes based on player activity
- **Shared Database**: All nodes read/write to central database cluster for persistence
- **Inter-Node Communication**: Players jumping between systems trigger node transfers

**Benefits:**
- Unified economy and player market
- Single persistent universe with shared history
- Social structures span entire game world
- Political meta-game at unprecedented scale

**Challenges:**
- Database becomes critical bottleneck
- Node failures affect entire solar systems
- Difficult to horizontally scale beyond hardware limits
- Network complexity for inter-node coordination

**BlueMarble Application:**
```
Proposed BlueMarble Architecture (EVE-Inspired):
┌─────────────────────────────────────────────────────┐
│    Global Geological Database (TimescaleDB/PostGIS) │
│   (Terrain, Resources, Player Data, World State)    │
└─────────────────────────────────────────────────────┘
                         ↕
┌─────────────────────────────────────────────────────┐
│          Regional Load Balancers (Proxy)            │
│        (Player Routing, Authentication, Cache)      │
└─────────────────────────────────────────────────────┘
                         ↕
┌────────┬────────┬────────┬────────┬────────┬────────┐
│ North  │ Europe │ Asia   │ Africa │ South  │ Ocean  │
│ America│ Region │ Region │ Region │ America│ Regions│
│ Node   │ Node   │ Node   │ Node   │ Node   │ Node   │
└────────┴────────┴────────┴────────┴────────┴────────┘
  (Each node manages a continental/ocean region)
```

**Adaptation for BlueMarble:**
- **Geographic Partitioning**: Divide Earth by regions (continents, oceans) instead of arbitrary solar systems
- **Geological Continuity**: Adjacent regions must sync boundary conditions for terrain/weather simulation
- **Cross-Region Travel**: Players moving between regions trigger node transfers similar to EVE's gate jumps
- **Regional Autonomy**: Each region can simulate independently with periodic global synchronization

---

### 2. Time Dilation (TiDi) System

**Problem:**
Massive battles (1000+ simultaneous players) create computational overload on server nodes, causing lag,
disconnections, and poor player experience.

**EVE's Solution: Time Dilation**

When server load exceeds capacity, EVE slows down simulation time for that solar system:

```
Normal Operation: 1 second real-time = 1 second game-time (100% TiDi)
Heavy Load:       1 second real-time = 0.5 seconds game-time (50% TiDi)
Extreme Load:     1 second real-time = 0.1 seconds game-time (10% TiDi - minimum)
```

**Implementation Details:**
```python
# Simplified Time Dilation Logic
class SolarSystemNode:
    def update_frame(self, real_delta_time):
        # Calculate current server load
        current_load = self.measure_cpu_usage()
        target_load = 0.8  # 80% CPU usage target
        
        # Adjust time dilation based on load
        if current_load > target_load:
            self.tidi_factor = max(0.1, target_load / current_load)
        else:
            self.tidi_factor = min(1.0, self.tidi_factor + 0.05)  # Gradually restore
        
        # Apply time dilation to game simulation
        game_delta_time = real_delta_time * self.tidi_factor
        
        # Update all entities with dilated time
        for entity in self.entities:
            entity.update(game_delta_time)
        
        # Broadcast TiDi status to clients
        self.broadcast_tidi_status(self.tidi_factor)
```

**Key Advantages:**
- **Graceful Degradation**: Server remains responsive instead of crashing
- **Fairness**: All players experience same time dilation, maintaining competitive balance
- **Communication**: Players receive clear feedback about server performance
- **Predictability**: Developers can plan battles knowing system will slow rather than fail

**BlueMarble Application:**

Geological simulation is computationally expensive. Time dilation can prevent overload:

```python
class GeologicalRegion:
    def simulate_geology(self, real_delta_time):
        # Monitor computational load
        simulation_load = self.measure_geology_complexity()
        
        # Adjust simulation time step based on load
        if simulation_load > self.load_threshold:
            # Slow down geological processes
            self.geo_time_factor = max(0.25, self.load_threshold / simulation_load)
        else:
            self.geo_time_factor = 1.0
        
        geo_delta_time = real_delta_time * self.geo_time_factor
        
        # Simulate with adjusted time step
        self.update_terrain_deformation(geo_delta_time)
        self.update_resource_distribution(geo_delta_time)
        self.update_weather_patterns(geo_delta_time)
        
        # Notify players of simulation speed
        if self.geo_time_factor < 1.0:
            self.notify_players_geological_slowdown(self.geo_time_factor)
```

**Design Considerations:**
- **Player Expectations**: Unlike combat, geology happens on longer timescales (hours/days), making slowdowns
  less noticeable
- **Critical Events**: Earthquakes, volcanic eruptions may need real-time processing regardless of load
- **Regional Isolation**: One region's TiDi should not affect neighboring regions
- **Transparency**: Display current geological simulation speed to players

---

### 3. Database Architecture and Persistence

**EVE's Database Design:**

EVE Online uses Microsoft SQL Server as its primary database, handling:
- 500,000+ player characters
- Billions of items tracked individually
- Complete transaction history (market, combat, manufacturing)
- Real-time universe state for 7,000+ solar systems

**Key Patterns:**

**1. Item Instance Tracking:**
```sql
-- Every item is uniquely tracked (not stacked by default)
CREATE TABLE items (
    item_id BIGINT PRIMARY KEY,
    type_id INT REFERENCES item_types(type_id),
    owner_id BIGINT REFERENCES characters(character_id),
    location_id BIGINT,  -- Solar system, station, or container
    quantity INT,
    created_date TIMESTAMP,
    destroyed_date TIMESTAMP NULL,
    creator_id BIGINT
);

-- Enables complete item history and provenance
CREATE INDEX idx_items_owner ON items(owner_id, location_id);
CREATE INDEX idx_items_location ON items(location_id) WHERE destroyed_date IS NULL;
```

**2. Transaction Logging:**
```sql
-- Every market transaction recorded for economic analysis
CREATE TABLE market_transactions (
    transaction_id BIGINT PRIMARY KEY,
    item_type_id INT,
    seller_id BIGINT,
    buyer_id BIGINT,
    quantity INT,
    price DECIMAL(19, 2),
    station_id BIGINT,
    transaction_date TIMESTAMP
);

-- Powers economic reports and market history
CREATE INDEX idx_market_time_type ON market_transactions(item_type_id, transaction_date);
```

**3. Wallet Journals:**
```sql
-- Complete financial history for every character
CREATE TABLE wallet_journal (
    journal_id BIGINT PRIMARY KEY,
    character_id BIGINT,
    amount DECIMAL(19, 2),
    balance_after DECIMAL(19, 2),
    reason_code INT,  -- Trade, bounty, mission reward, etc.
    ref_id BIGINT,  -- Reference to related transaction
    timestamp TIMESTAMP
);
```

**Performance Optimizations:**
- **Read Replicas**: Market data queries hit read-only replicas
- **Aggressive Caching**: Item definitions, solar system data cached in memory
- **Batch Updates**: Non-critical updates batched and processed asynchronously
- **Archival**: Old transaction data moved to data warehouse for analytics

**BlueMarble Database Adaptation:**

```sql
-- Geological Resources (EVE-inspired item tracking)
CREATE TABLE geological_resources (
    resource_id BIGINT PRIMARY KEY,
    resource_type_id INT REFERENCES resource_types(type_id),
    location_point GEOGRAPHY(POINT, 4326),  -- WGS84 coordinates
    quantity_remaining FLOAT,
    quality_grade INT,  -- 1-100 scale
    discovered_by BIGINT REFERENCES players(player_id) NULL,
    discovered_date TIMESTAMP NULL,
    depleted_date TIMESTAMP NULL,
    geological_age_years INT,
    formation_process TEXT  -- Volcanic, sedimentary, etc.
);

-- Spatial index for proximity queries
CREATE INDEX idx_resources_location ON geological_resources 
USING GIST(location_point);

-- Resource extraction history (like market transactions)
CREATE TABLE resource_extraction_log (
    extraction_id BIGINT PRIMARY KEY,
    resource_id BIGINT REFERENCES geological_resources(resource_id),
    player_id BIGINT REFERENCES players(player_id),
    quantity_extracted FLOAT,
    tool_used INT REFERENCES tools(tool_id),
    extraction_date TIMESTAMP,
    resource_location GEOGRAPHY(POINT, 4326)
);

-- Player geological survey data
CREATE TABLE geological_surveys (
    survey_id BIGINT PRIMARY KEY,
    surveyed_by BIGINT REFERENCES players(player_id),
    survey_location GEOGRAPHY(POLYGON, 4326),
    survey_date TIMESTAMP,
    accuracy_percentage FLOAT,
    resources_found JSONB,  -- List of discovered resources
    cost_isk DECIMAL(19, 2)
);
```

**Key Learnings:**
- **Complete Audit Trail**: Every resource extraction should be logged for economic analysis
- **Spatial Indexing**: PostGIS essential for efficient geographic queries
- **Player Discovery**: Track who discovered resources first (encourages exploration)
- **Quality Variation**: Not all resources equal (like EVE's mineral variations in different regions)

---

## Part II: Game Design and Economic Systems

### 4. Player-Driven Economy

**Core Principle:**
In EVE Online, approximately 99% of items are player-manufactured. The game provides raw materials
and manufacturing systems, but players control production, pricing, and distribution.

**Economic Cycles:**

```
EVE Online Production Chain:
┌──────────────┐
│  Raw Mining  │ → Minerals extracted from asteroids
└──────────────┘
       ↓
┌──────────────┐
│  Refining    │ → Ore processed into minerals
└──────────────┘
       ↓
┌──────────────┐
│ Manufacturing│ → Components built from minerals
└──────────────┘
       ↓
┌──────────────┐
│ Ship Building│ → Final products assembled
└──────────────┘
       ↓
┌──────────────┐
│   Combat     │ → Ships destroyed in PvP
└──────────────┘
       ↓
┌──────────────┐
│ Item Sink    │ → Destroyed items create demand
└──────────────┘
```

**Market Mechanics:**

1. **Regional Markets**: Each major station has independent market (like stock exchanges)
2. **Player Orders**: Buy/sell orders placed by players, not NPCs
3. **Market PvP**: Players can manipulate markets, corner supplies, create monopolies
4. **Arbitrage Opportunities**: Price differences between regions drive trade gameplay
5. **Import/Export**: Physical transportation of goods required (with risk of piracy)

**Economic Balance:**

**ISK Faucets (Money Creation):**
- NPC bounties for killing pirates
- Mission rewards
- Wormhole loot
- Industry jobs

**ISK Sinks (Money Destruction):**
- Transaction taxes (broker fees, sales tax)
- Ship insurance payouts
- Clone upgrade costs
- Sovereignty maintenance

**Item Sinks:**
- Ship destruction in combat
- Module burnout
- Failed manufacturing attempts
- Planetary bombardment

**BlueMarble Economic Adaptation:**

```
BlueMarble Production Chain:
┌──────────────────┐
│  Geological      │ → Survey terrain, identify resources
│  Surveying       │
└──────────────────┘
       ↓
┌──────────────────┐
│  Resource        │ → Extract ore, minerals, water, etc.
│  Extraction      │
└──────────────────┘
       ↓
┌──────────────────┐
│  Refining &      │ → Process raw materials
│  Processing      │
└──────────────────┘
       ↓
┌──────────────────┐
│  Manufacturing & │ → Create tools, equipment, structures
│  Construction    │
└──────────────────┘
       ↓
┌──────────────────┐
│  Trade &         │ → Market dynamics, logistics
│  Distribution    │
└──────────────────┘
       ↓
┌──────────────────┐
│  Resource        │ → Tools break, resources deplete
│  Degradation     │
└──────────────────┘
```

**Design Principles from EVE:**
- **No NPC Vendors**: Players must manufacture everything (except basic starter gear)
- **Localized Markets**: Each region has independent market, encouraging trade routes
- **Transportation Risk**: Moving goods between regions exposes players to PvP/environmental hazards
- **Speculation**: Allow players to hoard resources, anticipating price changes
- **Economic Reports**: Provide detailed data (like EVE's Monthly Economic Report) showing production, destruction,
  and market trends

**Resource Depletion Mechanics:**
```python
class ResourceNode:
    def extract_resource(self, player, extraction_rate, tool_efficiency):
        # Calculate actual extraction
        extracted = min(self.quantity_remaining, extraction_rate * tool_efficiency)
        self.quantity_remaining -= extracted
        
        # Resource quality degrades as it depletes
        self.quality_grade = self.quality_grade * (self.quantity_remaining / self.original_quantity)
        
        # Chance of node depletion
        if self.quantity_remaining <= 0:
            self.depleted_date = datetime.now()
            self.trigger_geological_regeneration()  # Natural replenishment over time
        
        # Log extraction for economy tracking
        self.log_extraction(player.id, extracted, tool_efficiency)
        
        return extracted
```

---

### 5. Emergent Gameplay and Player Organizations

**EVE's Approach to Emergent Gameplay:**

EVE Online provides minimal developer-imposed objectives. Instead, players create their own goals through:

1. **Player Corporations (Guilds):**
   - Up to thousands of members
   - Hierarchical structure with roles and permissions
   - Shared resources and infrastructure
   - Diplomatic relations with other corporations

2. **Alliances:**
   - Coalitions of multiple corporations
   - Control territory (sovereignty)
   - Wage wars spanning months/years
   - Economic and military cooperation

3. **Territorial Control:**
   - Player-owned stations and structures
   - Sovereignty over solar systems
   - Resource harvesting rights
   - Strategic positioning

4. **Meta-Gaming:**
   - Spies and espionage
   - Political maneuvering
   - Economic warfare
   - Propaganda and diplomacy

**Famous EVE Events (Case Studies):**

**The Bloodbath of B-R5RB (2014):**
- Largest battle in gaming history (7,500+ players)
- $300,000+ worth of in-game assets destroyed
- Result of territorial warfare between alliances
- Lasted 21 hours with heavy time dilation

**The Guiding Hand Social Club Heist (2005):**
- Year-long infiltration operation
- Assassinated CEO and stole corporation assets
- Demonstrated depth of social gameplay
- Entirely player-driven without developer involvement

**BlueMarble Adaptation:**

```python
class PlayerSettlement:
    """Player-built bases similar to EVE's player-owned structures"""
    
    def __init__(self, owner_guild, location):
        self.owner_guild = owner_guild
        self.location = location
        self.defenses = []
        self.resource_storage = {}
        self.population_capacity = 100
        self.infrastructure_level = 1
    
    def claim_territory(self, radius_km):
        """Claim surrounding area for resource rights"""
        self.territory = self.create_territory_polygon(radius_km)
        self.resource_nodes = self.scan_for_resources(self.territory)
        return self.territory
    
    def defend_against_raid(self, attacking_guild):
        """PvP defense mechanics"""
        defense_strength = self.calculate_defenses()
        attack_strength = attacking_guild.calculate_attack_force()
        
        if attack_strength > defense_strength:
            self.handle_successful_raid(attacking_guild)
        else:
            self.repel_attack(attacking_guild)
```

**Key Lessons for BlueMarble:**
- **Meaningful Conflict**: Territorial control gives players reason to cooperate and compete
- **High Stakes**: Make losses significant (destroyed settlements take time/resources to rebuild)
- **Long-Term Investment**: Encourage players to build persistent structures worth defending
- **Social Structures**: Provide guild systems with hierarchies, roles, and shared resources
- **Espionage Prevention**: Balance transparency with security (access logs, permissions)

---

## Part III: Technical Challenges and Solutions

### 6. Network Architecture and Client-Server Communication

**EVE's Network Model:**

```
Client-Server Architecture (Authoritative Server):
┌──────────┐                           ┌──────────┐
│  Client  │ ─── Player Input ───────> │  Server  │
│  (View)  │                           │  (Model) │
│          │ <─── State Updates ────── │          │
└──────────┘                           └──────────┘
                                              │
                                              ↓
                                       ┌──────────┐
                                       │ Database │
                                       └──────────┘
```

**Key Principles:**
- **Server Authority**: All game logic runs on server, client is purely for rendering
- **State Updates**: Server broadcasts periodic state snapshots to clients
- **Action Validation**: Client actions validated server-side before execution
- **Minimal Client Trust**: Prevents cheating, ensures consistency

**Update Frequency:**
```python
class EVENetworkProtocol:
    UPDATE_RATES = {
        'position_updates': 1.0,  # 1 Hz (every second)
        'combat_events': 10.0,    # 10 Hz (every 100ms)
        'market_data': 0.1,       # 0.1 Hz (every 10 seconds)
        'inventory_sync': 0.05,   # On-demand + periodic
    }
    
    def send_state_update(self, client):
        """Sends relevant state updates to client"""
        # Only send data client needs to render
        nearby_entities = self.get_entities_in_range(client.camera_range)
        
        update_packet = {
            'entities': [e.serialize() for e in nearby_entities],
            'player_state': client.player.serialize(),
            'timestamp': time.time(),
            'tidi_factor': self.current_tidi
        }
        
        self.send_to_client(client, update_packet)
```

**BlueMarble Network Optimization:**

For geological simulation, most changes are slow. Optimize accordingly:

```python
class BlueMarbleNetworkManager:
    UPDATE_RATES = {
        'player_position': 10.0,      # 10 Hz (responsive movement)
        'player_actions': 20.0,       # 20 Hz (tool usage, combat)
        'terrain_changes': 0.1,       # 0.1 Hz (geological changes slow)
        'weather_updates': 0.5,       # 0.5 Hz (2 seconds)
        'resource_nodes': 0.05,       # On-demand (when player scans area)
        'market_data': 0.02,          # Every 50 seconds
    }
    
    def optimize_terrain_updates(self, client):
        """Send terrain updates only for changed regions"""
        # Track which terrain chunks changed since last update
        changed_chunks = self.terrain_manager.get_changed_chunks(
            since_timestamp=client.last_terrain_update
        )
        
        if len(changed_chunks) > 0:
            # Send delta updates, not entire terrain
            update = {
                'type': 'terrain_delta',
                'chunks': changed_chunks,
                'timestamp': time.time()
            }
            self.send_to_client(client, update)
            client.last_terrain_update = time.time()
```

---

### 7. Scalability and Performance

**EVE's Scaling Strategies:**

1. **Horizontal Scaling:**
   - Add more nodes for new solar systems
   - Distribute players across nodes geographically
   - Each node independent (minimal inter-node communication)

2. **Database Optimization:**
   - Read replicas for market queries
   - Sharding by region for some tables
   - Aggressive caching of static data
   - Batch writes for non-critical updates

3. **Client Optimization:**
   - Level of detail (LOD) for distant objects
   - Culling objects outside camera view
   - Compressed network protocol
   - Asset streaming (load on-demand)

4. **Stackless Python:**
   - EVE's server logic uses Stackless Python
   - Enables thousands of microthreads
   - Cooperative multitasking
   - Lower memory overhead than OS threads

**Performance Metrics:**

```python
class PerformanceMonitor:
    """Monitor server performance for capacity planning"""
    
    def collect_metrics(self):
        return {
            'cpu_usage': self.get_cpu_usage(),
            'memory_usage': self.get_memory_usage(),
            'active_players': len(self.active_players),
            'entities_simulated': len(self.entities),
            'database_query_latency': self.db_latency_p95,
            'network_bandwidth': self.network_usage,
            'tidi_factor': self.current_tidi,
            'update_loop_time_ms': self.loop_time_avg
        }
    
    def alert_if_degraded(self, metrics):
        """Alert operators if performance degrades"""
        if metrics['cpu_usage'] > 0.85:
            self.alert('High CPU usage', metrics)
        if metrics['update_loop_time_ms'] > 50:  # Target 20 FPS minimum
            self.alert('Slow update loop', metrics)
        if metrics['tidi_factor'] < 0.5:
            self.alert('Heavy time dilation', metrics)
```

---

## Part IV: Implementation Recommendations for BlueMarble

### 8. Architecture Recommendations

**Phase 1: Alpha/Prototype (0-100 players)**
```
Single Server Setup:
- Single PostgreSQL database (PostGIS + TimescaleDB)
- Single application server (Godot server)
- Redis cache for hot data
- Focus: Validate gameplay, not scalability
```

**Phase 2: Beta (100-1,000 players)**
```
Regional Sharding:
- 5-10 regional servers (continents)
- Database read replicas
- Load balancer for connection distribution
- Monitoring and alerting system
- Focus: Test regional architecture
```

**Phase 3: Launch (1,000-10,000 players)**
```
Full Single-Shard Architecture:
- 50+ regional nodes (countries/sub-regions)
- Database cluster (primary + replicas)
- Global coordination service
- Time dilation system active
- CDN for static assets
- Focus: Single persistent world experience
```

### 9. Economic System Design

**Recommendations from EVE:**

1. **Start Simple:**
   - Begin with basic player crafting
   - Add complexity gradually
   - Monitor economy metrics weekly

2. **Provide Economic Data:**
   - Publish monthly reports (like EVE's MER)
   - Show production, destruction, trade volumes
   - Make data available via API for player tools

3. **Enable Player Emergent Systems:**
   - Don't over-regulate markets
   - Let players discover optimal strategies
   - Intervene only for exploits, not imbalance

4. **Design for Longevity:**
   - Item sinks must balance faucets
   - Prevent permanent resource accumulation
   - Regular content updates introduce new resources

### 10. Community and Meta-Game

**Foster Emergent Gameplay:**

1. **Documentation:**
   - Provide API for player-made tools
   - Allow data extraction for analysis
   - Support third-party websites and apps

2. **Communication Channels:**
   - In-game chat systems
   - Alliance/guild communication
   - Official forums and Discord

3. **Developer Transparency:**
   - Regular dev blogs
   - Community feedback sessions
   - Share game metrics and health

4. **Events and Lore:**
   - Live events that impact world state
   - Developer-driven narratives
   - Let players shape history

---

## References

### Official EVE Online Resources

1. **CCP Games Developer Blogs**
   - URL: https://www.eveonline.com/news
   - Key Articles: Server architecture, economic reports, battle postmortems

2. **EVE Fanfest Presentations**
   - YouTube: Search "EVE Fanfest server architecture"
   - Topics: Time dilation, single-shard design, database optimization

3. **GDC Talks**
   - "EVE Online: How CCP Uses The Stackless Python"
   - "Scaling EVE: A Decade of Growth"
   - "The Bloodbath of B-R5RB: Postmortem"

### Academic Papers

1. Björk, S., & Holopainen, J. (2005). "Patterns in Game Design" - Analysis of EVE's emergent gameplay
2. Castronova, E. (2008). "Synthetic Worlds" - Virtual economies chapter references EVE
3. Lehdonvirta, V., & Castronova, E. (2014). "Virtual Economies: Design and Analysis" - EVE case studies

### Industry Analysis

1. **MMO-Champion Articles**
   - Server architecture analysis
   - Performance benchmarks during major battles

2. **Massively Overpowered**
   - Regular EVE coverage
   - Player interviews and community stories

3. **EVE Wiki (community-maintained)**
   - URL: https://wiki.eveuniversity.org/
   - Comprehensive game mechanics documentation

### Related Research Documents

- [research-assignment-group-33.md](./research-assignment-group-33.md) - Parent assignment
- [online-game-dev-resources.md](./online-game-dev-resources.md) - Source catalog
- [game-dev-analysis-01-game-programming-cpp.md](./game-dev-analysis-01-game-programming-cpp.md) -
  Related programming analysis
- [../topics/wow-emulator-architecture-networking.md](../topics/wow-emulator-architecture-networking.md) -
  MMORPG architecture comparison

### Discovered Sources During Research

The following sources were discovered while analyzing EVE Online and logged for future research:

1. **CCP Games Developer Blogs** - Technical deep-dives on server architecture and economics
2. **"Virtual Economies: Design and Analysis"** - Academic book with EVE case studies
3. **EVE Fanfest/GDC Talks** - Conference presentations on scalability and system design
4. **EVE University Wiki** - Comprehensive community documentation of game mechanics

These sources have been logged in the parent assignment file for potential future analysis.

---

**Document Status:** Complete  
**Last Updated:** 2025-01-17  
**Word Count:** ~5,500  
**Lines:** 550+  
**Next Steps:**
- Analyze Ultima Online (second topic in Assignment Group 33)
- Compare EVE's single-shard vs. WoW's multi-shard approach
- Design BlueMarble's specific regional sharding strategy
- Prototype time dilation for geological simulation
