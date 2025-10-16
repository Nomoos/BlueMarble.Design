---
title: Unity NetCode for DOTS - Multiplayer Architecture Analysis
date: 2025-01-17
tags: [research, phase-3, group-45, netcode, multiplayer, networking, dots, ecs, high-priority]
status: complete
priority: High
discovered_from: Unity ECS/DOTS Documentation
phase: 3
group: 45
batch: 4
source_type: discovered
estimated_effort: 10-12h
---

# Unity NetCode for DOTS - Multiplayer Architecture Analysis

**Source Type:** Official Unity Package Documentation + Community Resources  
**Category:** GameDev-Tech (Networking, DOTS, Multiplayer)  
**Priority:** High  
**Estimated Effort:** 10-12 hours  
**Batch:** 4 (Discovered Sources)

---

## Executive Summary

Unity NetCode for DOTS provides a complete client-server multiplayer networking solution built on top of the Entity Component System architecture. It enables authoritative server gameplay with client-side prediction, lag compensation, and efficient state synchronization through a "ghost" snapshot system. For BlueMarble's planetary-scale MMORPG, NetCode offers the foundation for supporting 1000+ players per server with deterministic simulation, area-of-interest management, and low-latency responsive gameplay.

### Key Takeaways

1. **Client-Server Architecture**: Authoritative server prevents cheating and ensures consistent simulation
2. **Ghost Snapshots**: Efficient delta-compressed state synchronization (< 10 KB/s per player)
3. **Client-Side Prediction**: Responsive input handling with server reconciliation
4. **Lag Compensation**: Server-side rewinding for hit detection and fairness
5. **Deterministic Simulation**: Burst-compiled systems ensure identical results across clients/server
6. **Performance**: 20-30 players per area, 60 tick rate server, < 100ms latency targets
7. **Area-of-Interest**: Relevancy system for large world streaming (1000+ players per server)

### BlueMarble Applications

- **Authoritative Geological Simulation**: Server controls rock erosion, landslides, resource spawning
- **Player Research Synchronization**: Shared discoveries, collaborative projects
- **Economic Market Replication**: Real-time trading with authoritative transaction validation
- **Vehicle/Equipment Networking**: Rovers, drills, aircraft with prediction and reconciliation
- **Deterministic Physics**: Rock physics, landslides synchronized across all clients
- **Scalable Architecture**: Area-based relevancy for 1000+ concurrent players per shard

---

## 1. NetCode Architecture Overview

### 1.1 Client-Server Model

NetCode implements a traditional authoritative server architecture:

```
┌─────────────────────────────────────────────────────────────┐
│                    Authoritative Server                      │
│  - Runs full simulation (AI, physics, economy)               │
│  - Validates all client inputs                               │
│  - Broadcasts state snapshots to clients                     │
│  - Handles 20-60 clients per server instance                 │
└─────────────────────────────────────────────────────────────┘
           │                      │                      │
           ▼                      ▼                      ▼
    ┌──────────┐          ┌──────────┐          ┌──────────┐
    │ Client 1 │          │ Client 2 │          │ Client N │
    │ Predict  │          │ Predict  │          │ Predict  │
    │ Render   │          │ Render   │          │ Render   │
    │ Reconcile│          │ Reconcile│          │ Reconcile│
    └──────────┘          └──────────┘          └──────────┘
```

**Benefits for BlueMarble:**
- Prevents cheating (resource gathering, research data, trading)
- Ensures geological simulation consistency
- Enables authoritative economy (server validates trades)
- Supports large player counts through sharding

### 1.2 Ghost System

"Ghosts" are networked entities replicated from server to clients:

```csharp
[GhostComponent(PrefabType = GhostPrefabType.All)]
public struct PlayerCharacter : IComponentData
{
    [GhostField] public float3 Position;
    [GhostField] public quaternion Rotation;
    [GhostField] public float Health;
    [GhostField] public int CurrentResearch;
}

[GhostComponent(PrefabType = GhostPrefabType.InterpolatedClient)]
public struct OtherPlayerTag : IComponentData { }
```

**Ghost Types:**
1. **Owner Predicted**: Player's own character (client predicts, server corrects)
2. **Interpolated**: Other players/NPCs (smooth interpolation between snapshots)
3. **Server Only**: Economy state, geological data (never sent to clients)

**Bandwidth Optimization:**
- Delta compression (only changed fields)
- Quantization (position: 1cm precision = 16 bits)
- Priority/relevancy filtering
- Snapshot rate: 20-60 Hz (adjustable per ghost)

Typical bandwidth: **5-15 KB/s per player** (20-player area)

### 1.3 Command System

Players send inputs as "commands" to server:

```csharp
public struct PlayerCommand : ICommandData
{
    public uint Tick;
    public float3 MoveInput;
    public quaternion ViewRotation;
    public InputButtons Buttons; // Jump, interact, drill, etc.
}

[UpdateInGroup(typeof(GhostInputSystemGroup))]
public partial class PlayerCommandSystem : SystemBase
{
    protected override void OnUpdate()
    {
        var tick = World.GetExistingSystem<NetworkTimeSystem>().ServerTick;
        
        Entities
            .WithAll<PlayerCommandInput>()
            .ForEach((ref DynamicBuffer<PlayerCommand> commandBuffer) =>
            {
                var cmd = new PlayerCommand
                {
                    Tick = tick,
                    MoveInput = GetPlayerInput(),
                    ViewRotation = GetViewRotation(),
                    Buttons = GetButtonStates()
                };
                commandBuffer.AddCommandData(cmd);
            }).ScheduleParallel();
    }
}
```

**Command Buffer:**
- Client stores last 128 commands (for reconciliation)
- Server processes commands in tick order
- Duplicate detection prevents replay attacks

---

## 2. Client-Side Prediction and Reconciliation

### 2.1 Prediction Loop

Client predicts own movement immediately (no latency):

```csharp
[UpdateInGroup(typeof(PredictedSimulationSystemGroup))]
public partial class PlayerMovementSystem : SystemBase
{
    protected override void OnUpdate()
    {
        var deltaTime = Time.DeltaTime;
        
        Entities
            .WithAll<Simulate, PredictedGhostComponent>()
            .ForEach((ref Translation pos, ref Rotation rot, 
                     in PlayerCommand cmd, in PlayerVelocity vel) =>
            {
                // Predict movement using command input
                var moveDir = math.mul(rot.Value, new float3(cmd.MoveInput.x, 0, cmd.MoveInput.z));
                pos.Value += moveDir * vel.Speed * deltaTime;
                rot.Value = cmd.ViewRotation;
            }).ScheduleParallel();
    }
}
```

**Prediction Steps:**
1. Client receives input (frame N)
2. Client immediately applies movement (no wait)
3. Client stores command in buffer
4. Client sends command to server
5. Server processes command (100ms later)
6. Server sends corrected state back
7. Client reconciles prediction with server state

### 2.2 Server Reconciliation

When server state differs from prediction, client rewinds and replays:

```csharp
[UpdateInGroup(typeof(GhostSimulationSystemGroup))]
public partial class PredictionReconciliationSystem : SystemBase
{
    protected override void OnUpdate()
    {
        var serverTick = GetSingleton<NetworkSnapshotAck>().LastReceivedSnapshotByLocal;
        
        Entities
            .WithAll<PredictedGhostComponent>()
            .ForEach((Entity entity, ref Translation pos, 
                     in DynamicBuffer<PlayerCommand> commands) =>
            {
                // If server position differs from prediction...
                if (math.distance(pos.Value, serverPosition) > 0.5f)
                {
                    // Rewind to server state
                    pos.Value = serverPosition;
                    
                    // Re-apply all commands after server tick
                    for (uint tick = serverTick + 1; tick <= currentTick; tick++)
                    {
                        var cmd = commands.GetDataAtTick(tick);
                        ApplyMovement(ref pos, cmd);
                    }
                }
            }).ScheduleParallel();
    }
}
```

**Reconciliation Scenarios:**

| Scenario | Client Prediction | Server Result | Action |
|----------|-------------------|---------------|--------|
| Normal | Move forward 5m | Move forward 5m | ✅ No correction |
| Lag Spike | Move forward 5m | Move forward 3m | ⚠️ Snap back 2m, replay |
| Collision | Walk through rock | Blocked by rock | ⚠️ Correct position |
| Teleport | Continuous movement | Server teleport | ⚠️ Accept teleport |

**Smoothing:** Clients can interpolate corrections over 100-200ms to hide snapping.

---

## 3. Lag Compensation

### 3.1 Server-Side Rewinding

For instant actions (shooting, mining), server rewinds world to client's view:

```csharp
[UpdateInGroup(typeof(GhostPredictionSystemGroup))]
public partial class LagCompensationSystem : SystemBase
{
    private NativeArray<HistoryEntry> positionHistory;
    
    protected override void OnUpdate()
    {
        var serverTick = GetSingleton<NetworkTimeSystem>().ServerTick;
        
        // Store current positions in history
        Entities
            .ForEach((Entity entity, in Translation pos) =>
            {
                positionHistory[entity] = new HistoryEntry
                {
                    Tick = serverTick,
                    Position = pos.Value
                };
            }).ScheduleParallel();
        
        // Process player actions (drilling, mining)
        Entities
            .WithAll<PlayerCommand>()
            .ForEach((in PlayerCommand cmd) =>
            {
                // Rewind world to client's view time
                var clientTick = cmd.Tick;
                var rewindDelay = serverTick - clientTick; // ~100ms at 60Hz
                
                // Restore rock positions to client's perspective
                RestoreWorldState(clientTick);
                
                // Process action (did client hit rock?)
                if (RaycastHit(cmd.ViewRotation, out Entity hitEntity))
                {
                    // Award resources, damage rock, etc.
                    ProcessMiningAction(hitEntity, cmd);
                }
                
                // Restore current state
                RestoreWorldState(serverTick);
            }).ScheduleParallel();
    }
}
```

**Lag Compensation Benefits:**
- Fair gameplay (high-ping players not disadvantaged)
- Responsive actions (drilling feels instant)
- Prevents "leading targets" requirement

**BlueMarble Use Cases:**
- Rock drilling (raycast from client's view)
- Resource gathering (hit detection)
- Equipment placement (collision checks)
- Vehicle collision (rewound physics)

### 3.2 Interpolation Delay

Other players are interpolated 100-200ms behind:

```csharp
[UpdateInGroup(typeof(GhostUpdateSystemGroup))]
public partial class GhostInterpolationSystem : SystemBase
{
    protected override void OnUpdate()
    {
        var interpolationTick = GetSingleton<NetworkTimeSystem>().InterpolationTick;
        var interpolationTickFraction = GetSingleton<NetworkTimeSystem>().InterpolationTickFraction;
        
        Entities
            .WithAll<InterpolatedGhostComponent>()
            .ForEach((ref Translation pos, in DynamicBuffer<GhostSnapshot> snapshots) =>
            {
                // Get two snapshots to interpolate between
                var snapshot1 = snapshots.GetDataAtTick(interpolationTick);
                var snapshot2 = snapshots.GetDataAtTick(interpolationTick + 1);
                
                // Smooth interpolation
                pos.Value = math.lerp(snapshot1.Position, snapshot2.Position, interpolationTickFraction);
            }).ScheduleParallel();
    }
}
```

**Tradeoff:** Smooth motion vs. 100-200ms delay (acceptable for other players)

---

## 4. Deterministic Simulation

### 4.1 Fixed-Point Math

NetCode uses deterministic fixed-point math for critical gameplay:

```csharp
// Floating-point (non-deterministic across platforms)
public float CalculateDamage(float attackPower, float defense)
{
    return attackPower * 1.5f - defense * 0.8f; // ❌ May differ on ARM vs x86
}

// Fixed-point (deterministic)
public int CalculateDamage(int attackPower, int defense)
{
    // All values in 1/1000th units (e.g., 1500 = 1.5)
    return (attackPower * 1500 - defense * 800) / 1000; // ✅ Identical everywhere
}
```

**Deterministic Systems:**
- Physics simulation (rock movement, landslides)
- Economic calculations (price updates, trades)
- AI behavior (predictable NPC actions)
- Random number generation (shared seed per tick)

### 4.2 Deterministic Random

Synchronized RNG for consistent results:

```csharp
public struct DeterministicRandom : IComponentData
{
    public Random Value;
}

[UpdateInGroup(typeof(GhostPredictionSystemGroup))]
public partial class RandomEventSystem : SystemBase
{
    protected override void OnUpdate()
    {
        var serverTick = GetSingleton<NetworkTimeSystem>().ServerTick;
        
        Entities
            .ForEach((ref DeterministicRandom rng) =>
            {
                // Initialize with server tick as seed
                rng.Value = new Random((uint)serverTick);
                
                // Generate deterministic events
                if (rng.Value.NextFloat() < 0.01f) // 1% chance
                {
                    SpawnLandslide(rng.Value);
                }
            }).ScheduleParallel();
    }
}
```

**BlueMarble Applications:**
- Resource spawn locations (deterministic per sector)
- Weather events (synchronized across clients)
- NPC spawning (same creatures everywhere)
- Geological events (landslides, erosion)

---

## 5. Area-of-Interest and Relevancy

### 5.1 Relevancy Sets

Server only sends ghosts relevant to each client:

```csharp
[UpdateInGroup(typeof(GhostSendSystemGroup))]
public partial class RelevancySystem : SystemBase
{
    protected override void OnUpdate()
    {
        Entities
            .WithAll<NetworkIdComponent>()
            .ForEach((Entity clientEntity, in NetworkIdComponent netId) =>
            {
                var clientPos = GetPlayerPosition(clientEntity);
                
                // Build relevancy set for this client
                var relevantGhosts = new NativeList<Entity>(Allocator.Temp);
                
                Entities
                    .WithAll<GhostComponent>()
                    .ForEach((Entity ghostEntity, in Translation ghostPos) =>
                    {
                        var distance = math.distance(clientPos, ghostPos.Value);
                        
                        // Include if within area-of-interest (500m radius)
                        if (distance < 500f)
                        {
                            relevantGhosts.Add(ghostEntity);
                        }
                    }).Run();
                
                // Update client's relevancy set
                SetRelevancySet(netId, relevantGhosts);
            }).Run();
    }
}
```

**Relevancy Rules for BlueMarble:**

| Entity Type | Relevancy Range | Priority |
|-------------|-----------------|----------|
| Players | 500m radius | High |
| NPCs (active) | 200m radius | Medium |
| NPCs (idle) | 100m radius | Low |
| Vehicles | 1000m radius | High |
| Resources | 50m radius | Low |
| Geological events | Global (if major) | Critical |
| Economy data | Global | Medium |

**Benefits:**
- 1000+ players per server (20-30 per area)
- Reduced bandwidth (5-15 KB/s per client)
- Lower CPU (only simulate relevant entities)

### 5.2 Dynamic Interest Management

Adjust relevancy based on context:

```csharp
public partial class DynamicInterestSystem : SystemBase
{
    protected override void OnUpdate()
    {
        Entities
            .WithAll<NetworkIdComponent>()
            .ForEach((in PlayerState state) =>
            {
                float aoiRadius = 500f;
                
                // Expand AoI when in vehicle (faster movement)
                if (HasComponent<InVehicle>(state.Entity))
                    aoiRadius = 1000f;
                
                // Expand AoI when viewing market (see all traders)
                if (HasComponent<ViewingMarket>(state.Entity))
                    aoiRadius = 10000f; // Entire sector
                
                // Expand AoI for major events (everyone sees landslide)
                if (HasComponent<LandslideEvent>(state.Entity))
                    aoiRadius = 5000f;
                
                UpdateAreaOfInterest(state.Entity, aoiRadius);
            }).Run();
    }
}
```

---

## 6. Connection Management

### 6.1 Connection Flow

```
Client                                Server
  │                                     │
  ├──── Connect Request ──────────────►│
  │                                     │
  │◄──── Accept + Server Info ─────────┤
  │                                     │
  ├──── Join Request ─────────────────►│
  │      (player name, version)         │
  │                                     │
  │◄──── Spawn Player Entity ───────────┤
  │      (entity ID, initial state)     │
  │                                     │
  ├──── Ready ────────────────────────►│
  │                                     │
  │◄──── Game State Snapshot ───────────┤
  │      (all relevant ghosts)          │
  │                                     │
  ├──── Player Commands ──────────────►│
  │      (60 Hz input stream)           │
  │                                     │
  │◄──── State Snapshots ───────────────┤
  │      (20-60 Hz ghost updates)       │
  │                                     │
```

### 6.2 Disconnection Handling

```csharp
[UpdateInGroup(typeof(ServerSimulationSystemGroup))]
public partial class DisconnectionSystem : SystemBase
{
    protected override void OnUpdate()
    {
        Entities
            .WithAll<NetworkIdComponent>()
            .WithNone<NetworkStreamConnection>() // Disconnected
            .ForEach((Entity entity, in PlayerCharacter player) =>
            {
                // Player disconnected - handle gracefully
                
                // 1. Mark player as disconnected (keep for 5 minutes)
                EntityManager.AddComponent<DisconnectedPlayer>(entity);
                EntityManager.SetComponentData(entity, new DisconnectTimer { Remaining = 300f });
                
                // 2. Stop simulating AI for this player
                EntityManager.RemoveComponent<SimulateTag>(entity);
                
                // 3. Save player state to database
                SavePlayerState(player);
                
                // 4. Notify other players
                BroadcastPlayerDisconnect(player.Name);
                
                // 5. If in vehicle, safely stop it
                if (HasComponent<InVehicle>(entity))
                {
                    StopVehicle(GetComponent<InVehicle>(entity).VehicleEntity);
                }
            }).Run();
        
        // Clean up after 5 minute timeout
        Entities
            .WithAll<DisconnectedPlayer>()
            .ForEach((Entity entity, ref DisconnectTimer timer) =>
            {
                timer.Remaining -= Time.DeltaTime;
                
                if (timer.Remaining <= 0)
                {
                    // Player didn't reconnect - fully remove
                    EntityManager.DestroyEntity(entity);
                }
            }).Run();
    }
}
```

**Reconnection Flow:**
- Client sends reconnect with saved session token
- Server restores player entity (if within 5min window)
- Client receives full state snapshot
- Resume gameplay seamlessly

---

## 7. Performance Optimization

### 7.1 Tick Rate Configuration

```csharp
public struct NetworkTickRateSettings
{
    public uint SimulationTickRate;      // Server simulation (60 Hz recommended)
    public uint NetworkTickRate;         // Ghost snapshot send rate (20-60 Hz)
    public uint ClientTickRate;          // Client prediction rate (60 Hz)
    public float InterpolationDelay;     // Interpolation buffer (100-200ms)
    public uint MaxCommandAge;           // Drop commands older than N ticks
}

// High-performance setup
var settings = new NetworkTickRateSettings
{
    SimulationTickRate = 60,    // Smooth server simulation
    NetworkTickRate = 30,        // Balance bandwidth/smoothness
    ClientTickRate = 60,         // Responsive input
    InterpolationDelay = 0.1f,   // 100ms buffer
    MaxCommandAge = 128          // ~2 seconds at 60Hz
};
```

**Tradeoffs:**

| Tick Rate | Bandwidth | Latency Feel | CPU Usage |
|-----------|-----------|--------------|-----------|
| 20 Hz | Low (3-5 KB/s) | Acceptable | Low |
| 30 Hz | Medium (5-10 KB/s) | Good | Medium |
| 60 Hz | High (10-20 KB/s) | Excellent | High |

**BlueMarble Recommendation:** 30-60 Hz for gameplay, 20 Hz for economy updates

### 7.2 Bandwidth Budgets

```csharp
[GhostComponent]
public struct OptimizedGhost : IComponentData
{
    [GhostField(Quantization = 100)]  // 1cm precision
    public float3 Position;
    
    [GhostField(Quantization = 1000)] // 0.001 precision
    public quaternion Rotation;
    
    [GhostField(Quantization = 1)]    // Integer only
    public int Health;
    
    [GhostField(SendDataForChildEntity = false)] // Don't sync child transforms
    public Entity EquipmentEntity;
}
```

**Bandwidth Breakdown (per player):**

| Data Type | Size | Rate | Bandwidth |
|-----------|------|------|-----------|
| Position | 12 bytes | 30 Hz | 360 B/s |
| Rotation | 8 bytes | 30 Hz | 240 B/s |
| Velocity | 12 bytes | 30 Hz | 360 B/s |
| Health/State | 4 bytes | 30 Hz | 120 B/s |
| Commands | 20 bytes | 60 Hz | 1200 B/s |
| Overhead | Variable | - | 500 B/s |
| **Total** | | | **~3 KB/s** |

**20-player area:** 60 KB/s per client (0.5 Mbps) - achievable on most connections

### 7.3 Burst Compilation

All networking systems use Burst for maximum performance:

```csharp
[BurstCompile]
[UpdateInGroup(typeof(GhostPredictionSystemGroup))]
public partial struct PlayerMovementSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var deltaTime = SystemAPI.Time.DeltaTime;
        
        foreach (var (transform, velocity, cmd) in 
                 SystemAPI.Query<RefRW<LocalTransform>, 
                                 RefRO<PlayerVelocity>, 
                                 RefRO<PlayerCommand>>()
                         .WithAll<Simulate, PredictedGhostComponent>())
        {
            var moveDir = math.mul(transform.ValueRO.Rotation, 
                                   new float3(cmd.ValueRO.MoveInput.x, 0, cmd.ValueRO.MoveInput.z));
            transform.ValueRW.Position += moveDir * velocity.ValueRO.Speed * deltaTime;
        }
    }
}
```

**Performance:** 10,000+ ghost updates in < 2ms with Burst compilation

---

## 8. BlueMarble Implementation Guide

### 8.1 Networking Architecture

```
┌──────────────────────────────────────────────────────────┐
│                    BlueMarble Server                      │
│                                                           │
│  ┌─────────────┐  ┌──────────────┐  ┌────────────────┐  │
│  │ Player Sim  │  │ Geological   │  │ Economic       │  │
│  │ - Movement  │  │ - Rock Erosion│  │ - Markets     │  │
│  │ - Actions   │  │ - Landslides │  │ - Trading     │  │
│  │ - Research  │  │ - Resources  │  │ - Prices      │  │
│  └─────────────┘  └──────────────┘  └────────────────┘  │
│         │                │                  │            │
│         └────────────────┴──────────────────┘            │
│                          │                               │
│                    ┌─────▼──────┐                        │
│                    │ NetCode    │                        │
│                    │ Ghost Sync │                        │
│                    └─────┬──────┘                        │
└──────────────────────────┼───────────────────────────────┘
                           │
            ┌──────────────┼──────────────┐
            │              │              │
     ┌──────▼────┐  ┌──────▼────┐  ┌─────▼─────┐
     │ Client A  │  │ Client B  │  │ Client C  │
     │ - Predict │  │ - Predict │  │ - Predict │
     │ - Render  │  │ - Render  │  │ - Render  │
     │ - UI      │  │ - UI      │  │ - UI      │
     └───────────┘  └───────────┘  └───────────┘
```

### 8.2 Ghost Components

```csharp
// Player character (owner-predicted)
[GhostComponent(PrefabType = GhostPrefabType.AllPredicted)]
public struct PlayerCharacter : IComponentData
{
    [GhostField] public float3 Position;
    [GhostField] public quaternion Rotation;
    [GhostField] public float Health;
    [GhostField] public int CurrentResearch;
    [GhostField] public int InventoryHash; // Changed when items added/removed
}

// NPC researcher (interpolated)
[GhostComponent(PrefabType = GhostPrefabType.InterpolatedClient)]
public struct NPCResearcher : IComponentData
{
    [GhostField(Quantization = 100)] public float3 Position;
    [GhostField] public int CurrentBehavior; // Idle, gathering, analyzing
    [GhostField] public Entity TargetRock;
}

// Rock resource (interpolated, low priority)
[GhostComponent(PrefabType = GhostPrefabType.InterpolatedClient, 
                SendTypeOptimization = GhostSendType.OnlyPredictedClients)]
public struct RockResource : IComponentData
{
    [GhostField(Quantization = 100)] public float3 Position;
    [GhostField] public MaterialType Type;
    [GhostField] public float Health; // For destruction
}

// Economic market (server-only, replicated as events)
public struct MarketData : IComponentData
{
    public float GranitePrice;
    public int GraniteSupply;
    public int GraniteDemand;
    // Not ghosted - sent via RPC when market UI opened
}
```

### 8.3 Input Commands

```csharp
public struct PlayerCommand : ICommandData
{
    public uint Tick;
    
    // Movement
    public float2 MoveInput;      // WASD normalized
    public float2 ViewRotation;   // Pitch/yaw
    public byte Buttons;          // Jump, interact, drill (bit flags)
    
    // Actions
    public ActionType CurrentAction; // Drill, mine, scan, trade
    public Entity InteractTarget;    // Entity being interacted with
    
    // Research
    public int SelectedResearch;     // Currently selected research project
}

public enum ActionType : byte
{
    None = 0,
    Drill = 1,
    Mine = 2,
    Scan = 3,
    PlaceEquipment = 4,
    Trade = 5,
    FastTravel = 6
}
```

### 8.4 Prediction Systems

```csharp
// Player movement prediction
[BurstCompile]
[UpdateInGroup(typeof(PredictedSimulationSystemGroup))]
public partial struct PlayerMovementPredictionSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var deltaTime = SystemAPI.Time.DeltaTime;
        
        foreach (var (transform, velocity, cmd) in 
                 SystemAPI.Query<RefRW<LocalTransform>, 
                                 RefRO<PlayerVelocity>, 
                                 RefRO<PlayerCommand>>()
                         .WithAll<Simulate, PredictedGhostComponent>())
        {
            // Apply movement command
            var forward = math.mul(transform.ValueRO.Rotation, new float3(0, 0, 1));
            var right = math.mul(transform.ValueRO.Rotation, new float3(1, 0, 0));
            
            var moveDir = forward * cmd.ValueRO.MoveInput.y + 
                          right * cmd.ValueRO.MoveInput.x;
            
            if (math.lengthsq(moveDir) > 0.01f)
            {
                moveDir = math.normalize(moveDir);
                transform.ValueRW.Position += moveDir * velocity.ValueRO.Speed * deltaTime;
            }
            
            // Apply rotation
            var yaw = quaternion.RotateY(cmd.ValueRO.ViewRotation.y);
            transform.ValueRW.Rotation = yaw;
        }
    }
}

// Drilling action prediction
[BurstCompile]
[UpdateInGroup(typeof(PredictedSimulationSystemGroup))]
public partial struct DrillingPredictionSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        foreach (var (transform, cmd, drilling) in 
                 SystemAPI.Query<RefRO<LocalTransform>, 
                                 RefRO<PlayerCommand>, 
                                 RefRW<DrillingState>>()
                         .WithAll<Simulate, PredictedGhostComponent>())
        {
            if (cmd.ValueRO.CurrentAction == ActionType.Drill)
            {
                // Predict drilling progress
                drilling.ValueRW.Progress += 10f * SystemAPI.Time.DeltaTime;
                
                // Client-side VFX (sparks, particles)
                if (drilling.ValueRO.Progress > 100f)
                {
                    // Predicted completion (will be validated by server)
                    drilling.ValueRW.Progress = 0;
                    SpawnDrillingCompletionVFX(transform.ValueRO.Position);
                }
            }
        }
    }
}
```

### 8.5 Server Authority Systems

```csharp
// Server validates drilling and awards resources
[BurstCompile]
[UpdateInGroup(typeof(ServerSimulationSystemGroup))]
public partial struct ServerDrillingSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        foreach (var (drilling, inventory, cmd) in 
                 SystemAPI.Query<RefRW<DrillingState>, 
                                 RefRW<PlayerInventory>, 
                                 RefRO<PlayerCommand>>()
                         .WithAll<Simulate>())
        {
            if (cmd.ValueRO.CurrentAction == ActionType.Drill && 
                drilling.ValueRO.Progress >= 100f)
            {
                // Server authority: Award resources
                var rockEntity = cmd.ValueRO.InteractTarget;
                
                if (SystemAPI.HasComponent<RockResource>(rockEntity))
                {
                    var rock = SystemAPI.GetComponent<RockResource>(rockEntity);
                    
                    // Determine resource yield (server RNG)
                    var random = SystemAPI.GetSingleton<ServerRandom>();
                    var yield = CalculateResourceYield(rock.Type, random);
                    
                    // Add to inventory
                    AddToInventory(ref inventory.ValueRW, rock.Type, yield);
                    
                    // Damage or destroy rock
                    rock.Health -= 20f;
                    if (rock.Health <= 0)
                    {
                        state.EntityManager.DestroyEntity(rockEntity);
                        SpawnRockDebris(rock.Position);
                    }
                    else
                    {
                        SystemAPI.SetComponent(rockEntity, rock);
                    }
                    
                    // Reset drilling
                    drilling.ValueRW.Progress = 0;
                }
            }
        }
    }
}
```

### 8.6 Area Management

```csharp
// Divide world into sectors for area-of-interest
public struct SectorComponent : IComponentData
{
    public int2 SectorCoords; // (0-9, 0-9) for 100 sectors
    public int PlayerCount;
    public bool IsActive;
}

[UpdateInGroup(typeof(ServerSimulationSystemGroup))]
public partial struct SectorManagementSystem : SystemBase
{
    protected override void OnUpdate()
    {
        // Update player counts per sector
        var sectorCounts = new NativeArray<int>(100, Allocator.Temp);
        
        Entities
            .WithAll<PlayerCharacter>()
            .ForEach((in Translation pos) =>
            {
                var sectorCoords = WorldToSector(pos.Value);
                var sectorIndex = sectorCoords.x + sectorCoords.y * 10;
                sectorCounts[sectorIndex]++;
            }).Run();
        
        // Activate/deactivate sectors based on player presence
        Entities
            .WithAll<SectorComponent>()
            .ForEach((Entity entity, ref SectorComponent sector) =>
            {
                var sectorIndex = sector.SectorCoords.x + sector.SectorCoords.y * 10;
                sector.PlayerCount = sectorCounts[sectorIndex];
                
                // Activate if players present
                sector.IsActive = sector.PlayerCount > 0;
                
                if (!sector.IsActive)
                {
                    // Deactivate NPCs, reduce simulation
                    DeactivateSectorEntities(sector.SectorCoords);
                }
                else if (sector.PlayerCount > 30)
                {
                    // Too many players - spawn new server instance
                    RequestSectorSharding(sector.SectorCoords);
                }
            }).Run();
    }
}
```

---

## 9. Discovered Sources for Phase 4

During analysis of Unity NetCode for DOTS, identified the following high-value sources:

### 9.1 Multiplayer Architecture

1. **Unity Transport Package**
   - **Priority:** High
   - **Category:** Networking
   - **Effort:** 4-6 hours
   - **Rationale:** Low-level networking layer, UDP reliability, connection management

2. **NetCode for GameObjects (Legacy)**
   - **Priority:** Medium
   - **Category:** Networking
   - **Effort:** 3-4 hours
   - **Rationale:** Compare with DOTS NetCode, migration strategies

3. **Photon Quantum (ECS Networking)**
   - **Priority:** Medium
   - **Category:** Networking, Deterministic
   - **Effort:** 6-8 hours
   - **Rationale:** Alternative deterministic networking solution, lockstep architecture

### 9.2 Optimization Patterns

4. **Network LOD and Interest Management**
   - **Priority:** High
   - **Category:** Optimization, Networking
   - **Effort:** 4-5 hours
   - **Rationale:** Advanced relevancy systems for MMO-scale games

---

## 10. Implementation Roadmap

### Phase 1: Foundation (Weeks 1-2)

**Deliverables:**
- Basic client-server architecture
- Player connection/disconnection
- Simple ghost replication (position only)
- Command input system

**Validation:**
- 2-4 players can connect
- Players see each other move
- Disconnection doesn't crash server

### Phase 2: Prediction and Reconciliation (Weeks 3-4)

**Deliverables:**
- Client-side prediction for player
- Server reconciliation
- Interpolation for other players
- Basic lag compensation

**Validation:**
- Responsive movement (no perceived latency)
- Smooth reconciliation (no visible snapping)
- Works with 100-200ms artificial lag

### Phase 3: Gameplay Systems (Weeks 5-6)

**Deliverables:**
- Drilling/mining actions
- Resource collection (authoritative)
- Research progress synchronization
- Vehicle networking

**Validation:**
- Drilling feels responsive
- Resources awarded correctly (no duplication)
- Research syncs across sessions
- Vehicles predicted smoothly

### Phase 4: Scaling (Weeks 7-8)

**Deliverables:**
- Area-of-interest relevancy
- Sector-based sharding
- Bandwidth optimization
- 20-30 player stress test

**Validation:**
- < 10 KB/s per player bandwidth
- Smooth gameplay with 30 players
- CPU usage < 50% on server
- No crashes or desyncs

### Phase 5: Advanced Features (Weeks 9-10)

**Deliverables:**
- Economic market replication
- Geological event synchronization
- NPC networking (interpolated)
- Reconnection handling

**Validation:**
- Market prices sync correctly
- Landslides visible to all players
- NPCs don't teleport
- Reconnection restores state

### Phase 6: Production Hardening (Weeks 11-12)

**Deliverables:**
- Cheat detection (speedhack, teleport)
- Connection quality monitoring
- Graceful degradation (packet loss)
- Performance profiling

**Validation:**
- Cheating mitigated
- Players notified of poor connection
- Gameplay stable at 20% packet loss
- Server logs useful diagnostics

---

## 11. Key Insights and Best Practices

### 11.1 Architecture Decisions

| Decision | Rationale | Tradeoff |
|----------|-----------|----------|
| Client-server (not P2P) | Prevent cheating, authoritative economy | Higher server costs |
| 30 Hz snapshot rate | Balance bandwidth/smoothness | ~33ms visual latency |
| 100ms interpolation delay | Smooth other players | Slight disconnect feeling |
| Sector sharding | Scale to 1000+ players | World not truly seamless |
| Deterministic physics | Identical sim everywhere | More complex code |

### 11.2 Common Pitfalls

1. **Over-replicating Data**
   - ❌ Sending every entity to every client
   - ✅ Use area-of-interest filtering

2. **Non-Deterministic Logic**
   - ❌ Using `Time.deltaTime` directly in gameplay
   - ✅ Use fixed timestep with deterministic math

3. **Poor Prediction Design**
   - ❌ Predicting server-validated actions (resource spawning)
   - ✅ Only predict client inputs (movement, aiming)

4. **Bandwidth Waste**
   - ❌ Sending full state every frame
   - ✅ Delta compression + quantization

5. **CPU Bottlenecks**
   - ❌ Running full AI for all NPCs
   - ✅ LOD-based simulation (only active NPCs)

### 11.3 Performance Targets

**Server (60 Hz simulation):**
- 20-30 players per area: 50% CPU usage
- Ghost updates: < 5ms per frame
- Physics simulation: < 5ms per frame
- Pathfinding: < 10ms per frame (async)
- Memory: < 2 GB per server instance

**Client (60 FPS rendering):**
- Prediction: < 2ms per frame
- Interpolation: < 1ms per frame
- Command buffer: < 1ms per frame
- Network I/O: < 1ms per frame
- Memory: < 500 MB for networking

**Network (per client):**
- Bandwidth: 5-15 KB/s (0.12 Mbps)
- Latency tolerance: < 200ms
- Packet loss tolerance: < 10%
- Reconnection time: < 5 seconds

---

## 12. Conclusion

Unity NetCode for DOTS provides a robust, performant networking foundation for BlueMarble's planetary-scale MMORPG. Key advantages include:

1. **Authoritative Server**: Prevents cheating, ensures consistent geological simulation
2. **Client Prediction**: Responsive gameplay despite network latency
3. **Efficient Replication**: Bandwidth-optimized for 20-30 players per area
4. **Deterministic Simulation**: Identical results across all clients and server
5. **Scalable Architecture**: Area-of-interest enables 1000+ concurrent players per shard
6. **ECS Integration**: Leverages Burst compilation for maximum performance

**Implementation Recommendation:** Adopt NetCode for DOTS as the primary networking solution. Start with Phase 1 (foundation) and incrementally add features. Target 30 players per area with 60 Hz server simulation and 30 Hz snapshot rate for optimal balance of performance, bandwidth, and gameplay smoothness.

**Next Steps:**
1. Prototype basic client-server architecture (Weeks 1-2)
2. Implement prediction and reconciliation (Weeks 3-4)
3. Build out gameplay systems (drilling, resources, research) (Weeks 5-6)
4. Scale to 20-30 players with area-of-interest (Weeks 7-8)
5. Add economic and geological event replication (Weeks 9-10)
6. Harden for production (cheat detection, monitoring) (Weeks 11-12)

**Total Estimated Effort:** 12 weeks for production-ready multiplayer architecture

---

**Document Status:** Complete  
**Last Updated:** 2025-01-17  
**Cross-References:**
- Unity DOTS Physics (deterministic simulation)
- Game Engine Architecture - Subsystems (client-server patterns)
- Unity ECS/DOTS Documentation (ECB, system ordering)

**Phase 4 Integration:** Add discovered sources to networking research queue.

