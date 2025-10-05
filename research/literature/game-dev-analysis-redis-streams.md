---
title: "Redis Streams for Game Events - Event Sourcing and Real-Time Processing for MMORPGs"
date: 2025-01-16
tags: [redis, streams, event-sourcing, messaging, real-time, mmorpg, architecture]
category: GameDev-Tech
priority: High
status: completed
related_documents:
  - game-dev-analysis-database-design-for-mmorpgs.md
  - game-dev-analysis-multiplayer-programming.md
  - game-dev-analysis-world-of-warcraft.md
estimated_effort: 3-4 hours
actual_effort: 3.5 hours
discovered_from: "Assignment Group 24 & 01 Research"
---

# Redis Streams for Game Events
## Event Sourcing and Real-Time Processing for MMORPGs

### Executive Summary

Redis Streams provides a powerful, lightweight alternative to traditional message queues (RabbitMQ, Kafka) for real-time event processing in MMORPGs. This analysis examines Redis Streams' architecture, performance characteristics, and application patterns for BlueMarble's geological gameplay, player actions, and world events.

**Key Findings:**
- **Throughput**: 100,000+ messages/sec on single Redis instance
- **Latency**: Sub-millisecond event delivery for local consumers
- **Reliability**: At-least-once delivery with consumer groups and ACKs
- **Memory efficiency**: ~1KB per event with automatic trimming
- **Use case**: Perfect for real-time analytics, audit trails, and event fanout

**BlueMarble Applications:**
- Player action audit trail (mining, surveying, trading)
- Real-time leaderboards and achievements
- Economic analytics and cheat detection
- Geological event processing (earthquakes, resource spawns)
- Multi-service event fanout (notifications, analytics, logging)

---

## 1. Redis Streams Fundamentals

### 1.1 Stream Data Structure

Redis Streams is an append-only log data structure introduced in Redis 5.0. Each stream consists of time-ordered entries with auto-generated IDs.

**Stream Entry Structure:**
```
Stream: "player:actions:12345"
├─ 1734567890123-0: {action: "mine", resource: "iron", qty: 5}
├─ 1734567890456-0: {action: "move", x: 123.4, y: 567.8, z: 90.1}
├─ 1734567891789-0: {action: "survey", skill_check: 18, success: true}
└─ 1734567892012-0: {action: "trade", item: "pickaxe", gold: 150}
```

**Entry ID Format:** `timestamp-sequence`
- Timestamp: Milliseconds since epoch
- Sequence: Counter for events in same millisecond
- Example: `1734567890123-0` = Jan 18, 2025 12:31:30.123 UTC, first event

### 1.2 Core Commands

#### XADD - Add Events to Stream

```redis
# Add mining event to player action stream
XADD player:actions:12345 * action mine resource iron quantity 5 x 123.4 y 567.8 z 90.1

# Returns: "1734567890123-0" (auto-generated ID)
```

**Auto-ID vs Custom ID:**
- `*`: Redis generates timestamp-sequence ID (recommended)
- `1234567890-0`: Custom ID (use for replaying external events)

**Maxlen Limiting (Memory Management):**
```redis
# Keep only last 10,000 events (approximate trimming)
XADD player:actions:12345 MAXLEN ~ 10000 * action survey ...

# Exact trimming (more expensive)
XADD player:actions:12345 MAXLEN 10000 * action trade ...
```

#### XREAD - Read Events from Stream

```redis
# Read last 10 events from multiple streams
XREAD COUNT 10 STREAMS player:actions:12345 world:events:earthquakes 0 0

# Block for up to 5 seconds waiting for new events
XREAD BLOCK 5000 STREAMS player:actions:12345 $

# Read only new events ($ = start from end)
```

#### XRANGE - Query Historical Events

```redis
# Get all events in time range
XRANGE player:actions:12345 1734567890000 1734567900000

# Get first 100 events
XRANGE player:actions:12345 - + COUNT 100

# Get events in reverse order
XREVRANGE player:actions:12345 + - COUNT 50
```

### 1.3 Consumer Groups

Consumer groups enable distributed processing with load balancing and reliability.

**Architecture:**
```
Stream: player:actions:*
│
├─ Consumer Group: "analytics"
│  ├─ Consumer: "analytics-worker-1" (processes 50% of events)
│  └─ Consumer: "analytics-worker-2" (processes 50% of events)
│
├─ Consumer Group: "anti-cheat"
│  └─ Consumer: "cheat-detector-1" (processes 100% of events)
│
└─ Consumer Group: "achievements"
   ├─ Consumer: "achievement-worker-1"
   └─ Consumer: "achievement-worker-2"
```

**Each consumer group:**
- Maintains own read position in stream
- Distributes events among group consumers (competing consumers)
- Tracks pending/unacknowledged events
- Enables independent processing speeds

**Creating Consumer Group:**
```redis
# Create group starting from beginning of stream
XGROUP CREATE player:actions:12345 analytics 0

# Create group starting from end (only new events)
XGROUP CREATE player:actions:12345 anti-cheat $
```

**Reading as Consumer:**
```redis
# Read up to 10 new events for this consumer
XREADGROUP GROUP analytics worker-1 COUNT 10 STREAMS player:actions:12345 >

# Returns events and their IDs
```

**Acknowledging Events:**
```redis
# Mark event as successfully processed
XACK player:actions:12345 analytics 1734567890123-0

# Acknowledge multiple events
XACK player:actions:12345 analytics 1734567890123-0 1734567890456-0 1734567890789-0
```

### 1.4 Pending Entry List (PEL)

Tracks events delivered to consumers but not yet acknowledged (at-least-once delivery).

**Check Pending Events:**
```redis
# See pending events for consumer group
XPENDING player:actions:12345 analytics

# Response:
# 1) (integer) 3                    # 3 pending events
# 2) "1734567890123-0"              # Oldest pending ID
# 3) "1734567891234-0"              # Newest pending ID
# 4) 1) 1) "worker-1"               # Consumer stats
#       2) "2"                       # 2 pending events
#    2) 1) "worker-2"
#       2) "1"
```

**Claim Abandoned Events:**
```redis
# Take ownership of events idle for 60 seconds (handle worker failure)
XCLAIM player:actions:12345 analytics worker-3 60000 1734567890123-0

# Auto-claim with timeout check
XAUTOCLAIM player:actions:12345 analytics worker-3 60000 0 COUNT 10
```

**Dead Letter Queue Pattern:**
```c#
// After max retries, move to DLQ stream
if (retryCount >= MAX_RETRIES)
{
    await redis.XAddAsync("player:actions:dlq", new[]
    {
        new NameValueEntry("original_stream", "player:actions:12345"),
        new NameValueEntry("event_id", eventId),
        new NameValueEntry("error", error.Message),
        new NameValueEntry("retry_count", retryCount.ToString())
    });
    
    // ACK original to remove from pending
    await redis.XAckAsync("player:actions:12345", "analytics", eventId);
}
```

---

## 2. Event Sourcing Architecture

### 2.1 Event Sourcing Fundamentals

**Definition:** Store all state changes as a sequence of immutable events rather than mutable current state.

**Traditional State Storage:**
```sql
-- Current state only
UPDATE player_inventory SET iron_ore = 125 WHERE player_id = 12345;
```

**Event Sourcing:**
```redis
-- Append event to log
XADD player:12345:inventory:events * event_type mined resource iron_ore quantity 5 timestamp 1734567890

-- Current state derived by replaying events
```

### 2.2 Benefits for MMORPGs

**1. Complete Audit Trail**
- Every player action recorded permanently
- Investigate exploits after the fact
- Rollback player progress if bug found
- Legal compliance (GDPR right to access data)

**2. Temporal Queries**
- "What was player inventory on Jan 10, 2025?"
- "How much iron was mined this week?"
- "What actions did player take before ban?"

**3. Event Replay**
- Rebuild state from events after database corruption
- Test new features on historical data
- Train ML models on player behavior

**4. Microservices Integration**
- Multiple services consume same event stream
- Add new services without modifying producers
- Each service maintains own materialized view

### 2.3 Event Schema Design

**Event Structure:**
```json
{
  "event_id": "1734567890123-0",
  "event_type": "resource_extracted",
  "aggregate_id": "player:12345",
  "timestamp": 1734567890123,
  "version": 1,
  "data": {
    "resource_type": "iron_ore",
    "quantity": 5,
    "location": {"x": 123.4, "y": 567.8, "z": 90.1},
    "tool": "steel_pickaxe",
    "skill_level": 18
  },
  "metadata": {
    "session_id": "abc123",
    "ip_address": "10.0.1.5",
    "client_version": "1.2.3"
  }
}
```

**Event Types for BlueMarble:**
```
Player Events:
- player_registered
- player_logged_in
- player_logged_out
- player_moved
- player_died
- player_respawned

Geological Events:
- resource_extracted
- resource_surveyed
- terrain_modified
- geological_event_occurred (earthquake, landslide)

Economic Events:
- resource_sold
- resource_bought
- trade_completed
- company_formed
- company_dissolved

Social Events:
- chat_message_sent
- party_formed
- party_disbanded
- expedition_started
- expedition_completed
```

### 2.4 Snapshot Pattern

**Problem:** Replaying millions of events to rebuild state is slow.

**Solution:** Periodic snapshots + incremental events.

```
Player State Timeline:
├─ Snapshot @ 1734000000 (Jan 1, 2025): {level: 10, iron: 100, gold: 500}
├─ Events 1734000000-1734432000:
│  ├─ mined iron +5
│  ├─ sold iron 10 for 50 gold
│  ├─ leveled up to 11
│  └─ ...50,000 more events...
├─ Snapshot @ 1734432000 (Jan 6, 2025): {level: 13, iron: 247, gold: 1250}
└─ Events since last snapshot (only 1,000 events to replay)
```

**Implementation:**
```c#
public async Task<PlayerState> RebuildPlayerState(string playerId, long? asOfTimestamp = null)
{
    // Find most recent snapshot before target timestamp
    var snapshot = await GetLatestSnapshot(playerId, asOfTimestamp);
    var state = snapshot?.State ?? new PlayerState();
    
    // Replay events since snapshot
    var startId = snapshot?.LastEventId ?? "0";
    var endId = asOfTimestamp.HasValue ? $"{asOfTimestamp}-0" : "+";
    
    var events = await redis.XRangeAsync(
        $"player:{playerId}:events",
        startId,
        endId,
        count: 100000
    );
    
    foreach (var evt in events)
    {
        state = ApplyEvent(state, evt);
    }
    
    return state;
}

// Create snapshot every 10,000 events or daily
public async Task CreateSnapshot(string playerId)
{
    var state = await RebuildPlayerState(playerId);
    var lastEventId = await redis.XRevRangeAsync(
        $"player:{playerId}:events",
        "+", "-", 1
    ).First().Id;
    
    await redis.SetAsync(
        $"player:{playerId}:snapshot:{DateTimeOffset.UtcNow.ToUnixTimeSeconds()}",
        JsonSerializer.Serialize(new { State = state, LastEventId = lastEventId }),
        expiry: TimeSpan.FromDays(90)
    );
}
```

---

## 3. Real-Time Game Event Processing

### 3.1 Multi-Service Event Fanout

**Architecture Pattern:**

```
                      ┌──────────────────────┐
                      │  Game Server         │
                      │  (Event Producer)    │
                      └──────────┬───────────┘
                                 │
                                 │ XADD
                                 ▼
                      ┌──────────────────────┐
                      │  Redis Stream:       │
                      │  world:events        │
                      └──────────┬───────────┘
                                 │
                ┌────────────────┼────────────────┐
                │                │                │
                │ XREADGROUP     │ XREADGROUP     │ XREADGROUP
                ▼                ▼                ▼
    ┌───────────────────┐ ┌──────────────┐ ┌────────────────┐
    │ Analytics Service │ │ Anti-Cheat   │ │ Achievement    │
    │ Consumer Group    │ │ Consumer Grp │ │ Consumer Group │
    └───────────────────┘ └──────────────┘ └────────────────┘
```

**Producer Code (Game Server):**
```c#
public class GameEventPublisher
{
    private readonly IDatabase _redis;
    
    public async Task PublishPlayerAction(string playerId, PlayerAction action)
    {
        var streamKey = "world:events";
        
        await _redis.XAddAsync(streamKey, new[]
        {
            new NameValueEntry("event_type", "player_action"),
            new NameValueEntry("player_id", playerId),
            new NameValueEntry("action_type", action.Type),
            new NameValueEntry("action_data", JsonSerializer.Serialize(action.Data)),
            new NameValueEntry("timestamp", DateTimeOffset.UtcNow.ToUnixTimeMilliseconds())
        }, maxLength: 1000000, useApproximateMaxLength: true);
    }
    
    public async Task PublishGeologicalEvent(GeologicalEvent evt)
    {
        await _redis.XAddAsync("world:events", new[]
        {
            new NameValueEntry("event_type", "geological"),
            new NameValueEntry("geo_event_type", evt.Type), // earthquake, landslide, etc
            new NameValueEntry("location", $"{evt.X},{evt.Y},{evt.Z}"),
            new NameValueEntry("magnitude", evt.Magnitude),
            new NameValueEntry("radius", evt.Radius)
        }, maxLength: 100000, useApproximateMaxLength: true);
    }
}
```

**Consumer Code (Analytics Service):**
```c#
public class AnalyticsConsumer : BackgroundService
{
    private readonly IDatabase _redis;
    private const string StreamKey = "world:events";
    private const string GroupName = "analytics";
    private readonly string _consumerName;
    
    public AnalyticsConsumer(IDatabase redis, string hostname)
    {
        _redis = redis;
        _consumerName = $"analytics-{hostname}-{Guid.NewGuid():N}";
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // Create consumer group if doesn't exist
        try
        {
            await _redis.XGroupCreateAsync(StreamKey, GroupName, "0");
        }
        catch (RedisServerException ex) when (ex.Message.Contains("BUSYGROUP"))
        {
            // Group already exists, continue
        }
        
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                // Read up to 100 events, block for 5 seconds if none available
                var results = await _redis.XReadGroupAsync(
                    GroupName,
                    _consumerName,
                    new StreamPosition(StreamKey, ">"),
                    count: 100,
                    block: 5000
                );
                
                foreach (var streamEntry in results.SelectMany(r => r.Entries))
                {
                    await ProcessEvent(streamEntry);
                    
                    // Acknowledge successful processing
                    await _redis.XAckAsync(StreamKey, GroupName, streamEntry.Id);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing events");
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
    
    private async Task ProcessEvent(StreamEntry entry)
    {
        var eventType = entry.Values.FirstOrDefault(v => v.Name == "event_type").Value;
        
        switch (eventType)
        {
            case "player_action":
                await ProcessPlayerAction(entry);
                break;
            case "geological":
                await ProcessGeologicalEvent(entry);
                break;
            default:
                _logger.LogWarning("Unknown event type: {EventType}", eventType);
                break;
        }
    }
}
```

### 3.2 Player Leaderboard Updates

**Real-time leaderboard using sorted sets + streams:**

```c#
public class LeaderboardConsumer : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await _redis.XGroupCreateAsync("world:events", "leaderboard", "$"); // Only new events
        
        while (!stoppingToken.IsCancellationRequested)
        {
            var results = await _redis.XReadGroupAsync(
                "leaderboard",
                _consumerName,
                new StreamPosition("world:events", ">"),
                count: 50,
                block: 2000
            );
            
            foreach (var entry in results.SelectMany(r => r.Entries))
            {
                var eventType = entry["event_type"];
                
                if (eventType == "player_action")
                {
                    var playerId = entry["player_id"];
                    var actionType = entry["action_type"];
                    
                    if (actionType == "resource_extracted")
                    {
                        var resource = entry["resource"];
                        var quantity = int.Parse(entry["quantity"]);
                        
                        // Update leaderboard atomically
                        await _redis.SortedSetIncrementAsync(
                            $"leaderboard:mining:{resource}",
                            playerId,
                            quantity
                        );
                        
                        // Update player stats cache
                        await _redis.HashIncrementAsync(
                            $"player:{playerId}:stats",
                            $"mined_{resource}",
                            quantity
                        );
                    }
                }
                
                await _redis.XAckAsync("world:events", "leaderboard", entry.Id);
            }
        }
    }
}

// Query leaderboard
public async Task<List<LeaderboardEntry>> GetTop100Miners(string resource)
{
    var entries = await _redis.SortedSetRangeByRankWithScoresAsync(
        $"leaderboard:mining:{resource}",
        0, 99,
        order: Order.Descending
    );
    
    return entries.Select((e, index) => new LeaderboardEntry
    {
        Rank = index + 1,
        PlayerId = e.Element,
        Score = (long)e.Score
    }).ToList();
}
```

### 3.3 Achievement Processing

**Complex achievement tracking with event aggregation:**

```c#
public class AchievementProcessor
{
    // Achievement: Mine 1,000 iron ore
    public async Task ProcessMiningEvent(StreamEntry entry)
    {
        var playerId = entry["player_id"];
        var resource = entry["resource"];
        var quantity = int.Parse(entry["quantity"]);
        
        if (resource == "iron_ore")
        {
            // Increment player's total mined count
            var totalMined = await _redis.HashIncrementAsync(
                $"player:{playerId}:mining_totals",
                "iron_ore",
                quantity
            );
            
            // Check achievement thresholds
            await CheckMiningAchievements(playerId, "iron_ore", totalMined);
        }
    }
    
    private async Task CheckMiningAchievements(string playerId, string resource, long totalMined)
    {
        var thresholds = new[] { 100, 500, 1000, 5000, 10000 };
        
        foreach (var threshold in thresholds)
        {
            if (totalMined >= threshold)
            {
                var achievementKey = $"achievement:mine_{resource}_{threshold}";
                
                // Check if player already has this achievement
                var hasAchievement = await _redis.SetContainsAsync(
                    $"player:{playerId}:achievements",
                    achievementKey
                );
                
                if (!hasAchievement)
                {
                    // Award achievement
                    await _redis.SetAddAsync(
                        $"player:{playerId}:achievements",
                        achievementKey
                    );
                    
                    // Publish achievement event
                    await _redis.XAddAsync("world:events", new[]
                    {
                        new NameValueEntry("event_type", "achievement_unlocked"),
                        new NameValueEntry("player_id", playerId),
                        new NameValueEntry("achievement_id", achievementKey),
                        new NameValueEntry("timestamp", DateTimeOffset.UtcNow.ToUnixTimeMilliseconds())
                    });
                }
            }
        }
    }
}
```

---

## 4. Performance Characteristics

### 4.1 Throughput Benchmarks

**Hardware:** Standard Redis instance (4 vCPU, 16GB RAM)

| Operation | Throughput | Latency (p99) |
|-----------|-----------|---------------|
| XADD (no trimming) | 150,000 ops/sec | 0.5ms |
| XADD (with MAXLEN ~) | 120,000 ops/sec | 0.8ms |
| XREAD (10 entries) | 200,000 ops/sec | 0.3ms |
| XREADGROUP (10 entries) | 180,000 ops/sec | 0.4ms |
| XACK (single) | 250,000 ops/sec | 0.2ms |
| XACK (batch 100) | 500,000 acks/sec | 0.5ms |

**BlueMarble Load Estimate:**
- 10,000 concurrent players
- Average 5 events/sec per player = 50,000 events/sec
- Peak load (50% of players active simultaneously) = 75,000 events/sec
- **Conclusion:** Single Redis instance can handle load with 2x headroom

### 4.2 Memory Management

**Memory per Event:**
- Overhead: ~50 bytes (ID, metadata)
- Average payload: 200-300 bytes (serialized JSON)
- **Total:** ~350 bytes per event

**Memory Calculation:**
```
Events/sec: 50,000
Retention: 24 hours
Total events: 50,000 * 86,400 = 4.32 billion events
Memory: 4.32B * 350 bytes = 1.51 TB
```

**Problem:** Cannot store 24 hours in memory at full retention.

**Solution 1: Automatic Trimming**
```redis
# Keep only last 10 million events (~3.5GB, ~55 minutes at 50k/sec)
XADD world:events MAXLEN ~ 10000000 * ...

# Approximate trimming (~) is faster (O(1) vs O(N))
# Actual length may be 10M - 100k due to implementation
```

**Solution 2: Time-Based Trimming with MINID**
```redis
# Remove events older than 1 hour
XTRIM world:events MINID <one_hour_ago_timestamp>-0
```

**Solution 3: Tiered Storage**
```
Hot (Redis Streams):     Last 1 hour     = 3.5GB
Warm (TimescaleDB):      Last 7 days     = 100GB (compressed)
Cold (S3/Glacier):       Long-term       = Unlimited (cheap)
```

**Implementation:**
```c#
public class EventArchiver : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
            
            // Archive events older than 1 hour
            var oneHourAgo = DateTimeOffset.UtcNow.AddHours(-1).ToUnixTimeMilliseconds();
            var endId = $"{oneHourAgo}-0";
            
            // Read events to archive
            var events = await _redis.XRangeAsync("world:events", "-", endId, count: 10000);
            
            if (events.Length > 0)
            {
                // Write to TimescaleDB
                await _timescaleDb.BulkInsertEventsAsync(events);
                
                // Remove from Redis
                await _redis.XTrimAsync("world:events", minId: events.Last().Id);
                
                _logger.LogInformation("Archived {Count} events to TimescaleDB", events.Length);
            }
        }
    }
}
```

### 4.3 Persistence Options

**RDB (Snapshotting):**
- Periodic point-in-time snapshots
- Fast restarts, less disk I/O
- Potential data loss (up to last snapshot interval)
- Recommended: Save every 5-15 minutes

**AOF (Append-Only File):**
- Logs every write operation
- Minimal data loss (fsync every second)
- Slower restarts (replay all operations)
- Recommended: Use AOF with `everysec` fsync

**Best Practice for Streams:**
```
# redis.conf
appendonly yes
appendfsync everysec
save 900 1           # Save after 15 min if 1 key changed
save 300 10          # Save after 5 min if 10 keys changed
save 60 10000        # Save after 1 min if 10k keys changed
```

**Alternative: Master-Slave Replication**
- Run 2-3 Redis instances
- Slaves replicate master asynchronously
- Promotes slave if master fails
- Zero data loss with synchronous replication (slower)

---

## 5. Redis Streams vs Alternatives

### 5.1 Redis Streams vs Pub/Sub

| Feature | Redis Streams | Redis Pub/Sub |
|---------|---------------|---------------|
| Message persistence | Yes (configurable retention) | No (fire-and-forget) |
| Consumer groups | Yes (load balancing) | No (all subscribers get all messages) |
| At-least-once delivery | Yes (ACK + PEL) | No (if subscriber offline, message lost) |
| Backpressure | Yes (consumer reads at own pace) | No (fast publishers overwhelm slow subscribers) |
| Historical queries | Yes (XRANGE, XREVRANGE) | No (only live messages) |
| Use case | Event sourcing, audit trail, task queue | Real-time notifications, chat |

**When to use Pub/Sub:**
- Real-time notifications (player enters zone)
- Chat messages (ephemeral, don't need history)
- Presence updates (player online/offline)

**When to use Streams:**
- Player actions (need audit trail)
- Economic transactions (need history for analysis)
- Geological events (need replay capability)
- Achievement progress (need durability)

### 5.2 Redis Streams vs Apache Kafka

| Feature | Redis Streams | Apache Kafka |
|---------|---------------|---------------|
| Throughput | 100-150k msgs/sec | 1M+ msgs/sec |
| Latency | Sub-millisecond | 5-10ms |
| Data retention | Limited by RAM | Unlimited (disk-based) |
| Operational complexity | Low (single binary) | High (ZooKeeper, brokers, etc) |
| Horizontal scaling | Limited (sharding complex) | Excellent (partition-based) |
| Ecosystem | Basic | Rich (Kafka Streams, Connect, etc) |
| Cost | Low (single instance) | High (cluster + ops overhead) |

**When to use Redis Streams:**
- Event volumes < 100k/sec
- Low-latency requirements (< 1ms)
- Short retention (hours to days)
- Simple deployment requirements
- Budget constraints

**When to use Kafka:**
- Event volumes > 500k/sec
- Long retention (weeks to months)
- Complex stream processing (Kafka Streams)
- Multi-datacenter replication
- Need exactly-once semantics

**Hybrid Approach for BlueMarble:**
```
Redis Streams (hot path):
- Real-time player actions
- Achievement processing
- Leaderboard updates
- Short-term analytics (last hour)

Kafka (analytical path):
- Long-term event storage
- Complex stream processing (fraud detection)
- Cross-datacenter replication
- Historical analytics
```

**Data Flow:**
```
Game Server → Redis Streams → [Redis consumers process] → Archive to Kafka
                    ↓
              Real-time features
              (leaderboards, achievements)
```

### 5.3 Redis Streams vs RabbitMQ

| Feature | Redis Streams | RabbitMQ |
|---------|---------------|-----------|
| Routing | Stream-based | Exchange-based (flexible) |
| Priority queues | Manual implementation | Native support |
| Dead letter queues | Manual implementation | Native support |
| Message TTL | Manual trimming | Native per-message TTL |
| Transactional pub | Limited | Full AMQP transactions |
| Throughput | Higher | Lower |
| Protocol | Redis protocol (simple) | AMQP (complex but feature-rich) |

**When to use RabbitMQ:**
- Need complex routing (topic exchanges, fanout)
- Priority queues required
- Transactional guarantees critical
- Existing AMQP ecosystem

**When to use Redis Streams:**
- Higher throughput needs
- Simpler deployment
- Event sourcing patterns
- Time-series event data

---

## 6. BlueMarble Implementation Patterns

### 6.1 Geological Event Streams

**Stream Organization:**
```
geological:events:earthquakes
geological:events:landslides
geological:events:resource_spawns
geological:events:terrain_modifications
```

**Event Producer (Geological Simulation Service):**
```c#
public class GeologicalSimulator
{
    public async Task SimulateEarthquake(Vector3 epicenter, float magnitude)
    {
        // Calculate affected area
        var radius = magnitude * 100; // meters
        var affectedChunks = CalculateAffectedChunks(epicenter, radius);
        
        // Publish earthquake event
        var eventId = await _redis.XAddAsync("geological:events:earthquakes", new[]
        {
            new NameValueEntry("epicenter_x", epicenter.X),
            new NameValueEntry("epicenter_y", epicenter.Y),
            new NameValueEntry("epicenter_z", epicenter.Z),
            new NameValueEntry("magnitude", magnitude),
            new NameValueEntry("radius", radius),
            new NameValueEntry("affected_chunks", JsonSerializer.Serialize(affectedChunks)),
            new NameValueEntry("timestamp", DateTimeOffset.UtcNow.ToUnixTimeMilliseconds())
        });
        
        _logger.LogInformation("Earthquake event {EventId} published: magnitude {Magnitude} at {Epicenter}",
            eventId, magnitude, epicenter);
    }
}
```

**Event Consumer (Terrain Service):**
```c#
public class TerrainModificationConsumer : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await _redis.XGroupCreateAsync("geological:events:earthquakes", "terrain_modifier", "$");
        
        while (!stoppingToken.IsCancellationRequested)
        {
            var results = await _redis.XReadGroupAsync(
                "terrain_modifier",
                _consumerName,
                new StreamPosition("geological:events:earthquakes", ">"),
                count: 10,
                block: 5000
            );
            
            foreach (var entry in results.SelectMany(r => r.Entries))
            {
                var epicenter = new Vector3(
                    float.Parse(entry["epicenter_x"]),
                    float.Parse(entry["epicenter_y"]),
                    float.Parse(entry["epicenter_z"])
                );
                var magnitude = float.Parse(entry["magnitude"]);
                var affectedChunks = JsonSerializer.Deserialize<List<ChunkId>>(entry["affected_chunks"]);
                
                // Apply terrain modifications
                foreach (var chunkId in affectedChunks)
                {
                    await ApplyEarthquakeDamage(chunkId, epicenter, magnitude);
                }
                
                await _redis.XAckAsync("geological:events:earthquakes", "terrain_modifier", entry.Id);
            }
        }
    }
}
```

### 6.2 Economic Analytics Pipeline

**Real-time market price aggregation:**

```c#
public class EconomicAnalyticsConsumer : BackgroundService
{
    private readonly Dictionary<string, List<decimal>> _recentPrices = new();
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await _redis.XGroupCreateAsync("world:events", "economics", "$");
        
        // Flush aggregated data every 10 seconds
        var flushTimer = new Timer(FlushAggregatedData, null, TimeSpan.FromSeconds(10), TimeSpan.FromSeconds(10));
        
        while (!stoppingToken.IsCancellationRequested)
        {
            var results = await _redis.XReadGroupAsync(
                "economics",
                _consumerName,
                new StreamPosition("world:events", ">"),
                count: 100,
                block: 2000
            );
            
            foreach (var entry in results.SelectMany(r => r.Entries))
            {
                if (entry["event_type"] == "trade_completed")
                {
                    var resource = entry["resource"].ToString();
                    var pricePerUnit = decimal.Parse(entry["price_per_unit"]);
                    
                    lock (_recentPrices)
                    {
                        if (!_recentPrices.ContainsKey(resource))
                            _recentPrices[resource] = new List<decimal>();
                        
                        _recentPrices[resource].Add(pricePerUnit);
                    }
                }
                
                await _redis.XAckAsync("world:events", "economics", entry.Id);
            }
        }
        
        flushTimer.Dispose();
    }
    
    private async void FlushAggregatedData(object state)
    {
        Dictionary<string, List<decimal>> snapshot;
        lock (_recentPrices)
        {
            snapshot = new Dictionary<string, List<decimal>>(_recentPrices);
            _recentPrices.Clear();
        }
        
        // Calculate and store market statistics
        foreach (var (resource, prices) in snapshot)
        {
            if (prices.Count == 0) continue;
            
            var stats = new MarketStats
            {
                Resource = resource,
                AvgPrice = prices.Average(),
                MinPrice = prices.Min(),
                MaxPrice = prices.Max(),
                Volume = prices.Count,
                Timestamp = DateTimeOffset.UtcNow
            };
            
            // Store in TimescaleDB for historical analysis
            await _timescaleDb.InsertMarketStatsAsync(stats);
            
            // Update Redis cache for quick access
            await _redis.StringSetAsync(
                $"market:stats:{resource}:latest",
                JsonSerializer.Serialize(stats),
                expiry: TimeSpan.FromMinutes(1)
            );
        }
    }
}
```

### 6.3 Anti-Cheat Detection Pipeline

**Anomaly detection using event patterns:**

```c#
public class AntiCheatConsumer : BackgroundService
{
    private readonly Dictionary<string, PlayerActivityTracker> _trackers = new();
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await _redis.XGroupCreateAsync("world:events", "anti_cheat", "$");
        
        while (!stoppingToken.IsCancellationRequested)
        {
            var results = await _redis.XReadGroupAsync(
                "anti_cheat",
                _consumerName,
                new StreamPosition("world:events", ">"),
                count: 100,
                block: 1000
            );
            
            foreach (var entry in results.SelectMany(r => r.Entries))
            {
                if (entry["event_type"] == "player_action")
                {
                    var playerId = entry["player_id"].ToString();
                    var actionType = entry["action_type"].ToString();
                    
                    // Get or create tracker
                    if (!_trackers.ContainsKey(playerId))
                        _trackers[playerId] = new PlayerActivityTracker(playerId);
                    
                    var tracker = _trackers[playerId];
                    tracker.RecordAction(actionType, entry);
                    
                    // Check for suspicious patterns
                    var violations = tracker.DetectViolations();
                    if (violations.Any())
                    {
                        await ReportViolations(playerId, violations);
                    }
                }
                
                await _redis.XAckAsync("world:events", "anti_cheat", entry.Id);
            }
        }
    }
}

public class PlayerActivityTracker
{
    private readonly Queue<DateTime> _miningEvents = new();
    private readonly Queue<(Vector3 pos, DateTime time)> _movementEvents = new();
    
    public void RecordAction(string actionType, StreamEntry entry)
    {
        var now = DateTime.UtcNow;
        
        switch (actionType)
        {
            case "mine":
                _miningEvents.Enqueue(now);
                // Keep only last 60 seconds
                while (_miningEvents.Count > 0 && (now - _miningEvents.Peek()).TotalSeconds > 60)
                    _miningEvents.Dequeue();
                break;
                
            case "move":
                var pos = new Vector3(
                    float.Parse(entry["x"]),
                    float.Parse(entry["y"]),
                    float.Parse(entry["z"])
                );
                _movementEvents.Enqueue((pos, now));
                while (_movementEvents.Count > 0 && (now - _movementEvents.Peek().time).TotalSeconds > 10)
                    _movementEvents.Dequeue();
                break;
        }
    }
    
    public List<CheatViolation> DetectViolations()
    {
        var violations = new List<CheatViolation>();
        
        // Check mining rate
        if (_miningEvents.Count > 120) // > 2 per second for 60 seconds
        {
            violations.Add(new CheatViolation
            {
                Type = "excessive_mining_rate",
                Severity = "high",
                Details = $"Mining rate: {_miningEvents.Count / 60.0:F1} per second"
            });
        }
        
        // Check movement speed
        if (_movementEvents.Count >= 2)
        {
            var recent = _movementEvents.TakeLast(2).ToList();
            var distance = Vector3.Distance(recent[0].pos, recent[1].pos);
            var timeDelta = (recent[1].time - recent[0].time).TotalSeconds;
            var speed = distance / timeDelta;
            
            if (speed > MAX_PLAYER_SPEED * 1.5) // 50% tolerance
            {
                violations.Add(new CheatViolation
                {
                    Type = "speed_hack",
                    Severity = "critical",
                    Details = $"Speed: {speed:F1} m/s (max: {MAX_PLAYER_SPEED} m/s)"
                });
            }
        }
        
        return violations;
    }
}
```

---

## 7. Operational Best Practices

### 7.1 Monitoring and Alerting

**Key Metrics to Track:**

```c#
public class StreamMonitor : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
            
            // Check stream length
            var streamLength = await _redis.XLengthAsync("world:events");
            _metrics.Gauge("redis.stream.length", streamLength, new[] { "stream:world:events" });
            
            // Check pending entries per consumer group
            var groups = new[] { "analytics", "anti_cheat", "achievements", "leaderboard" };
            foreach (var group in groups)
            {
                var pending = await _redis.XPendingAsync("world:events", group);
                _metrics.Gauge("redis.stream.pending", pending.Count, new[] { $"group:{group}" });
                
                // Alert if pending count too high
                if (pending.Count > 10000)
                {
                    _logger.LogWarning("High pending count for group {Group}: {Count}", group, pending.Count);
                    await _alerting.SendAlert($"Redis consumer group '{group}' has {pending.Count} pending entries");
                }
            }
            
            // Check memory usage
            var info = await _redis.ExecuteAsync("INFO", "memory");
            var usedMemory = ParseMemoryUsage(info);
            _metrics.Gauge("redis.memory.used_bytes", usedMemory);
            
            if (usedMemory > MAX_MEMORY * 0.9)
            {
                _logger.LogError("Redis memory usage critical: {UsedMB} MB", usedMemory / 1024 / 1024);
                await _alerting.SendAlert($"Redis memory usage at {usedMemory / 1024 / 1024} MB (90% of max)");
            }
        }
    }
}
```

### 7.2 Consumer Lag Detection

**Track how far behind consumers are:**

```c#
public async Task<ConsumerLagReport> GetConsumerLag(string streamKey, string groupName)
{
    // Get last entry ID in stream
    var lastEntry = await _redis.XRevRangeAsync(streamKey, "+", "-", 1);
    var lastId = lastEntry.FirstOrDefault().Id;
    
    // Get pending entries for group
    var pending = await _redis.XPendingAsync(streamKey, groupName);
    
    // Get group's last delivered ID
    var groupInfo = await _redis.XInfoGroupsAsync(streamKey);
    var group = groupInfo.FirstOrDefault(g => g.Name == groupName);
    
    return new ConsumerLagReport
    {
        StreamKey = streamKey,
        GroupName = groupName,
        LastStreamId = lastId,
        LastDeliveredId = group?.LastDeliveredId ?? "0-0",
        PendingCount = pending.Count,
        EstimatedLagSeconds = EstimateLag(lastId, group?.LastDeliveredId)
    };
}

private double EstimateLag(string lastId, string deliveredId)
{
    // Extract timestamps from IDs (format: timestamp-sequence)
    var lastTimestamp = long.Parse(lastId.Split('-')[0]);
    var deliveredTimestamp = long.Parse(deliveredId?.Split('-')[0] ?? "0");
    
    return (lastTimestamp - deliveredTimestamp) / 1000.0; // Convert ms to seconds
}
```

### 7.3 Dead Letter Queue Handling

**Monitor and process failed events:**

```c#
public class DeadLetterQueueMonitor : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
            
            // Check DLQ size
            var dlqLength = await _redis.XLengthAsync("world:events:dlq");
            
            if (dlqLength > 100)
            {
                _logger.LogWarning("Dead letter queue has {Count} entries, investigating...", dlqLength);
                
                // Sample recent DLQ entries
                var samples = await _redis.XRevRangeAsync("world:events:dlq", "+", "-", 10);
                
                foreach (var entry in samples)
                {
                    var originalStream = entry["original_stream"];
                    var eventId = entry["event_id"];
                    var error = entry["error"];
                    var retryCount = entry["retry_count"];
                    
                    _logger.LogError("DLQ entry: stream={Stream}, eventId={EventId}, error={Error}, retries={Retries}",
                        originalStream, eventId, error, retryCount);
                }
                
                // Alert ops team
                await _alerting.SendAlert($"Dead letter queue has {dlqLength} entries requiring investigation");
            }
        }
    }
}
```

---

## 8. Discovered Sources

During this research, the following sources were identified for future analysis:

**1. Redis Time Series for Gaming Metrics**
- **Priority:** Medium
- **Category:** GameDev-Tech
- **Estimated Effort:** 2-3 hours
- **Rationale:** Specialized module for time-series data (player counts, resource prices over time)

**2. Redis Gears for Stream Processing**
- **Priority:** Medium
- **Category:** GameDev-Tech
- **Estimated Effort:** 3-4 hours
- **Rationale:** Serverless event processing within Redis (could simplify consumer logic)

**3. EventStoreDB vs Redis Streams Comparison**
- **Priority:** Low
- **Category:** GameDev-Tech
- **Estimated Effort:** 2-3 hours
- **Rationale:** Purpose-built event sourcing database comparison

---

## 9. Recommendations for BlueMarble

### 9.1 Adoption Strategy

**Phase 1: MVP (Month 1-2)**
- Implement Redis Streams for player action logging
- Single consumer group for analytics
- 1-hour retention in Redis
- Manual archival to PostgreSQL

**Phase 2: Event Sourcing (Month 3-4)**
- Full event sourcing for player state
- Snapshot pattern implementation
- Multiple consumer groups (analytics, anti-cheat, achievements)
- Automated archival pipeline to TimescaleDB

**Phase 3: Real-Time Features (Month 5-6)**
- Real-time leaderboards
- Achievement processing
- Economic analytics dashboard
- Anti-cheat detection system

**Phase 4: Scale (Month 7+)**
- Redis Cluster for horizontal scaling
- Kafka integration for long-term storage
- Advanced stream processing (Redis Gears or custom)
- Cross-region replication

### 9.2 Technology Stack

**Recommended:**
- **Redis 7.0+**: Latest streams features and performance
- **Redis Cluster**: Horizontal scaling when > 100k events/sec
- **TimescaleDB**: Long-term event storage and analytics
- **Grafana**: Monitoring dashboards
- **C#/.NET**: Application-level consumers

**Alternative for Higher Scale:**
- **Kafka**: If event volumes exceed 500k/sec or need long retention in stream
- **Redis Streams → Kafka**: Hybrid approach (hot path in Redis, archive to Kafka)

### 9.3 Cost Estimation

**AWS Pricing (us-east-1):**

| Component | Specification | Monthly Cost |
|-----------|---------------|--------------|
| Redis (ElastiCache) | cache.r6g.xlarge (4 vCPU, 32GB) | $240 |
| Redis Replica | cache.r6g.xlarge (failover) | $240 |
| TimescaleDB (RDS) | db.r6g.2xlarge (8 vCPU, 64GB) | $600 |
| Data Transfer | 100GB/month outbound | $9 |
| **Total** | | **$1,089/month** |

**Scaling Costs:**
- 50k events/sec: $1,089/month (above config)
- 200k events/sec: $2,500/month (Redis Cluster 3 nodes + larger TimescaleDB)
- 1M events/sec: $8,000/month (Redis Cluster 6 nodes + Kafka + large TimescaleDB)

---

## 10. Conclusion

Redis Streams provides an excellent foundation for event-driven architecture in BlueMarble:

**Strengths:**
- ✅ High throughput (100k+ events/sec per instance)
- ✅ Low latency (sub-millisecond)
- ✅ Simple deployment (single binary)
- ✅ Built-in consumer groups and reliability
- ✅ Cost-effective for moderate loads

**Limitations:**
- ❌ RAM-limited retention (hours, not weeks)
- ❌ Complex horizontal scaling
- ❌ Limited stream processing features vs Kafka Streams

**Best Use Cases:**
- ✅ Real-time player action logging
- ✅ Achievement and leaderboard processing
- ✅ Anti-cheat detection pipelines
- ✅ Economic analytics
- ✅ Geological event distribution

**Not Recommended For:**
- ❌ Long-term event storage (use TimescaleDB/Kafka)
- ❌ Complex event processing (use Kafka Streams/Flink)
- ❌ Multi-datacenter replication (use Kafka)

**Recommended Architecture:**
```
Game Servers → Redis Streams → [Real-time consumers] → TimescaleDB
                    ↓
            (Leaderboards, Achievements, Anti-Cheat)
                    ↓
              Optional: Archive to Kafka for advanced analytics
```

This approach provides the responsiveness of Redis for hot-path features while leveraging specialized databases (TimescaleDB) and message queues (Kafka) for long-term storage and complex processing.

---

**Document Version:** 1.0  
**Last Updated:** 2025-01-16  
**Author:** Research Team  
**Review Status:** Completed
