# Latency Compensating Methods in Client/Server Protocol Design - Analysis for BlueMarble MMORPG

---
title: Latency Compensating Methods in Client/Server Protocol Design - GDC 2001 Analysis
date: 2025-01-17
tags: [networking, multiplayer, client-prediction, lag-compensation, mmorpg, performance]
status: complete
priority: high
parent-research: online-game-dev-resources.md
discovered-from: 2D Game Development with Unity
---

**Source:** Latency Compensating Methods in Client/Server In-game Protocol Design and Optimization  
**Author:** Yahn Bernier (Valve Software)  
**Presented:** Game Developers Conference (GDC) 2001  
**URL:** https://developer.valvesoftware.com/wiki/Latency_Compensating_Methods_in_Client/Server_In-game_Protocol_Design_and_Optimization  
**Category:** Networking - Latency Compensation  
**Priority:** High  
**Status:** ✅ Complete  
**Assignment Group:** 37 (Discovered Source #2)  
**Discovered From:** 2D Game Development with Unity analysis (networking patterns)  
**Related Sources:** State Synchronization in Networked Games (Gaffer On Games), Source Engine Networking

---

## Executive Summary

This analysis examines Yahn Bernier's landmark GDC 2001 presentation on latency compensation techniques used in Valve's Source Engine (Counter-Strike, Half-Life). The paper presents practical solutions to the fundamental challenge of networked games: how to create responsive gameplay despite network latency. These techniques are directly applicable to BlueMarble's MMORPG architecture, particularly for entity movement, combat, and interaction systems.

**Key Takeaways for BlueMarble:**
- Client-side prediction eliminates perceived input latency (critical for responsive player movement)
- Server reconciliation ensures authoritative state while maintaining smooth client experience
- Lag compensation enables hit detection that "feels fair" despite 50-150ms network delays
- Entity interpolation provides smooth movement of remote players despite packet loss
- Input command buffering enables reliable command delivery over unreliable networks

**Relevance:** Critical for BlueMarble's networked gameplay. While designed for FPS games, these patterns directly translate to MMORPG entity movement, combat resolution, and resource gathering interactions. Proper implementation is the difference between responsive gameplay and "laggy" frustration.

---

## Part I: The Fundamental Problem

### 1. Network Latency in Online Games

**The Core Challenge:**

In networked games, every player action must traverse the network to the server and back before the player sees the result. This round-trip time (RTT) creates perceived input lag.

```
Player Input → Network (25-75ms) → Server Processing → Network (25-75ms) → Visual Feedback
Total Delay: 50-150ms (noticeable and frustrating)
```

**Impact Without Compensation:**

```csharp
// Naive implementation (unplayable)
public class NaiveNetworkedMovement {
    void Update() {
        if (Input.GetKey(KeyCode.W)) {
            // Send move command to server
            SendCommandToServer(new MoveCommand(Direction.Forward));
            
            // Wait for server response before moving
            // Player feels 100ms+ delay on every input
        }
    }
    
    void OnServerResponse(Vector3 newPosition) {
        // Finally update position after round-trip
        transform.position = newPosition;
    }
}
```

**Player Experience:**
- Every keystroke feels delayed
- Movement is jerky and unresponsive
- Combat timing is impossible
- Game feels "broken" even on good connections

**Bernier's Solution:**
Combine three techniques to hide latency:
1. **Client-side prediction** - Move immediately, verify later
2. **Server reconciliation** - Correct prediction errors smoothly
3. **Lag compensation** - Rewind time for hit detection

---

## Part II: Client-Side Prediction

### 2. Predicting Local Player Movement

**Core Concept:**

The client immediately simulates the player's own movement without waiting for server confirmation. This creates instant visual feedback while the server independently validates the action.

**Implementation Pattern:**

```csharp
public class ClientPrediction {
    // Local player state
    private Vector3 predictedPosition;
    private Queue<PlayerInput> pendingInputs = new Queue<PlayerInput>();
    private int nextInputId = 0;
    
    // Server-confirmed state
    private Vector3 serverPosition;
    private int lastAcknowledgedInputId = -1;
    
    void Update() {
        // 1. Capture player input
        PlayerInput input = CaptureInput();
        input.id = nextInputId++;
        input.timestamp = Time.time;
        
        // 2. Store for reconciliation
        pendingInputs.Enqueue(input);
        
        // 3. Send to server
        SendToServer(input);
        
        // 4. Predict movement locally (instant feedback)
        predictedPosition = SimulateMovement(predictedPosition, input);
        transform.position = predictedPosition;
    }
    
    private PlayerInput CaptureInput() {
        return new PlayerInput {
            forward = Input.GetAxis("Vertical"),
            right = Input.GetAxis("Horizontal"),
            jump = Input.GetButton("Jump"),
            deltaTime = Time.deltaTime
        };
    }
    
    private Vector3 SimulateMovement(Vector3 currentPos, PlayerInput input) {
        // Same movement logic as server
        Vector3 velocity = new Vector3(
            input.right * moveSpeed,
            0,
            input.forward * moveSpeed
        );
        
        // Apply physics (must match server exactly)
        return currentPos + velocity * input.deltaTime;
    }
}
```

**Key Requirements:**

1. **Deterministic Simulation:** Client and server must use identical movement code
2. **Input Recording:** Store all inputs until server confirms them
3. **Immediate Application:** Apply input locally before network round-trip

**BlueMarble Application:**
- Player movement in game world
- Resource gathering actions (immediate visual feedback)
- Skill activation (cast bar starts immediately)
- Interaction with world objects

---

### 3. Server Reconciliation

**The Correction Problem:**

Client predictions can diverge from server reality due to:
- Network packet loss
- Server-side collision resolution
- Other player interactions
- Cheat prevention

**Reconciliation Algorithm:**

```csharp
public class ServerReconciliation {
    void OnServerStateUpdate(ServerState state) {
        // Server confirms position up to input ID X
        serverPosition = state.position;
        lastAcknowledgedInputId = state.lastInputId;
        
        // Remove acknowledged inputs
        while (pendingInputs.Count > 0) {
            var input = pendingInputs.Peek();
            if (input.id <= lastAcknowledgedInputId) {
                pendingInputs.Dequeue();
            } else {
                break;
            }
        }
        
        // Check for prediction error
        float error = Vector3.Distance(predictedPosition, serverPosition);
        
        if (error > reconciliationThreshold) {
            // Significant error - need to correct
            
            // Start from server-confirmed position
            Vector3 correctedPosition = serverPosition;
            
            // Re-simulate all pending inputs
            foreach (var input in pendingInputs) {
                correctedPosition = SimulateMovement(correctedPosition, input);
            }
            
            // Smoothly blend to corrected position
            StartCoroutine(SmoothCorrection(predictedPosition, correctedPosition));
            predictedPosition = correctedPosition;
        }
    }
    
    private IEnumerator SmoothCorrection(Vector3 from, Vector3 to) {
        float duration = 0.1f; // 100ms smooth correction
        float elapsed = 0;
        
        while (elapsed < duration) {
            float t = elapsed / duration;
            transform.position = Vector3.Lerp(from, to, t);
            elapsed += Time.deltaTime;
            yield return null;
        }
        
        transform.position = to;
    }
}
```

**Error Threshold Configuration:**

```csharp
public class ReconciliationConfig {
    // Thresholds for different scenarios
    public float minReconciliationError = 0.01f;  // Ignore tiny errors
    public float softReconciliationError = 0.1f;   // Smooth correction
    public float hardReconciliationError = 1.0f;   // Snap correction (teleport)
    
    public void HandleError(float error) {
        if (error < minReconciliationError) {
            // Ignore - within tolerance
        } else if (error < softReconciliationError) {
            // Smooth interpolation over 100ms
            ApplySmoothCorrection();
        } else if (error < hardReconciliationError) {
            // Faster correction over 50ms
            ApplyQuickCorrection();
        } else {
            // Snap to server position (likely teleport or major desync)
            ApplyHardCorrection();
        }
    }
}
```

**BlueMarble Application:**
- Smooth movement corrections during normal gameplay
- Handle collision with server-validated obstacles
- Prevent client-side cheating while maintaining responsiveness
- Graceful handling of packet loss scenarios

---

## Part III: Entity Interpolation

### 4. Remote Entity Movement

**The Problem:**

Other players' positions arrive in discrete network packets (typically 10-30 Hz). Without interpolation, their movement appears stuttery and teleports.

**Network Update Timeline:**

```
Actual Movement:  →→→→→→→→→→→→→→→→ (smooth)
Network Packets:  A-----B-----C----- (discrete, 100ms apart)
Without Interp:   A→→→→→B→→→→→C→→→→→ (stuttery)
With Interp:      A→→→→→B→→→→→C→→→→→ (smooth)
```

**Interpolation Implementation:**

```csharp
public class EntityInterpolation {
    private struct PositionState {
        public Vector3 position;
        public Quaternion rotation;
        public float timestamp;
    }
    
    // Buffer of received states
    private Queue<PositionState> stateBuffer = new Queue<PositionState>();
    
    // Interpolation delay (buffer time)
    private float interpolationDelay = 0.1f; // 100ms
    
    void OnNetworkUpdate(Vector3 pos, Quaternion rot, float serverTime) {
        // Add to buffer
        stateBuffer.Enqueue(new PositionState {
            position = pos,
            rotation = rot,
            timestamp = serverTime
        });
        
        // Keep buffer reasonable size
        while (stateBuffer.Count > 10) {
            stateBuffer.Dequeue();
        }
    }
    
    void Update() {
        // Calculate interpolation time (slightly in the past)
        float interpolationTime = Time.time - interpolationDelay;
        
        // Find states to interpolate between
        PositionState from = new PositionState();
        PositionState to = new PositionState();
        
        foreach (var state in stateBuffer) {
            if (state.timestamp <= interpolationTime) {
                from = state;
            } else {
                to = state;
                break;
            }
        }
        
        // Interpolate between states
        if (to.timestamp > from.timestamp) {
            float t = (interpolationTime - from.timestamp) / 
                      (to.timestamp - from.timestamp);
            t = Mathf.Clamp01(t);
            
            transform.position = Vector3.Lerp(from.position, to.position, t);
            transform.rotation = Quaternion.Slerp(from.rotation, to.rotation, t);
        }
        
        // Remove old states
        while (stateBuffer.Count > 0 && 
               stateBuffer.Peek().timestamp < interpolationTime - 1.0f) {
            stateBuffer.Dequeue();
        }
    }
}
```

**Adaptive Interpolation:**

```csharp
public class AdaptiveInterpolation {
    private float currentDelay = 0.1f;
    private float minDelay = 0.05f;
    private float maxDelay = 0.2f;
    
    void AdjustInterpolationDelay() {
        // Monitor buffer health
        int bufferSize = stateBuffer.Count;
        
        if (bufferSize < 2) {
            // Buffer starvation - increase delay
            currentDelay = Mathf.Min(currentDelay + 0.01f, maxDelay);
        } else if (bufferSize > 5) {
            // Buffer overflow - decrease delay (more responsive)
            currentDelay = Mathf.Max(currentDelay - 0.01f, minDelay);
        }
    }
}
```

**BlueMarble Application:**
- Smooth movement of other players
- NPC movement in multiplayer zones
- Projectile movement (arrows, spells)
- Resource gathering animations
- Mount/vehicle movement

---

## Part IV: Lag Compensation

### 5. Lag Compensation for Hit Detection

**The Fairness Problem:**

In fast-paced combat, hit detection must account for network latency:

```
Player's View (100ms ago):  Target at position A → Fire!
Server's View (now):        Target at position B (moved)
Without Compensation:       Miss (frustrating for player)
With Compensation:          Rewind to position A → Hit!
```

**Server-Side Rewinding:**

```csharp
public class LagCompensation {
    // History of entity positions
    private Dictionary<int, Queue<HistoricalState>> entityHistory;
    
    private class HistoricalState {
        public Vector3 position;
        public Quaternion rotation;
        public Bounds hitbox;
        public float timestamp;
    }
    
    // Store entity states over time
    void FixedUpdate() {
        float currentTime = Time.time;
        
        foreach (var entity in allEntities) {
            var state = new HistoricalState {
                position = entity.position,
                rotation = entity.rotation,
                hitbox = entity.GetHitbox(),
                timestamp = currentTime
            };
            
            if (!entityHistory.ContainsKey(entity.id)) {
                entityHistory[entity.id] = new Queue<HistoricalState>();
            }
            
            entityHistory[entity.id].Enqueue(state);
            
            // Keep history for max latency window (e.g., 500ms)
            while (entityHistory[entity.id].Count > 0 &&
                   entityHistory[entity.id].Peek().timestamp < currentTime - 0.5f) {
                entityHistory[entity.id].Dequeue();
            }
        }
    }
    
    // Process hit detection with lag compensation
    public bool CheckHit(Player shooter, Vector3 target, float clientTime) {
        // Calculate player's latency
        float latency = Time.time - clientTime;
        float rewindTime = Time.time - latency;
        
        // Find affected entities
        List<Entity> potentialTargets = FindEntitiesNearTarget(target);
        
        foreach (var entity in potentialTargets) {
            // Rewind entity to shooter's view
            HistoricalState rewindState = GetStateAtTime(entity.id, rewindTime);
            
            // Check hit against rewound position
            if (CheckRaycast(shooter.position, target, rewindState)) {
                // Hit confirmed at client's timestamp
                return true;
            }
        }
        
        return false;
    }
    
    private HistoricalState GetStateAtTime(int entityId, float targetTime) {
        var history = entityHistory[entityId];
        
        // Find states to interpolate between
        HistoricalState before = null;
        HistoricalState after = null;
        
        foreach (var state in history) {
            if (state.timestamp <= targetTime) {
                before = state;
            } else {
                after = state;
                break;
            }
        }
        
        // Interpolate to exact time
        if (before != null && after != null) {
            float t = (targetTime - before.timestamp) / 
                      (after.timestamp - before.timestamp);
            
            return new HistoricalState {
                position = Vector3.Lerp(before.position, after.position, t),
                rotation = Quaternion.Slerp(before.rotation, after.rotation, t),
                hitbox = before.hitbox,
                timestamp = targetTime
            };
        }
        
        return before ?? after;
    }
}
```

**BlueMarble Combat Application:**

```csharp
public class BlueMarbleCombat {
    // Combat action with lag compensation
    public void ProcessMeleeAttack(Player attacker, int targetId, float clientTimestamp) {
        // Get attacker's latency
        float latency = GetPlayerLatency(attacker.id);
        
        // Rewind target to attacker's view
        float rewindTime = Time.time - latency;
        Entity target = GetEntity(targetId);
        Vector3 rewindedPosition = GetHistoricalPosition(targetId, rewindTime);
        
        // Check if attack would have hit at client's timestamp
        float distance = Vector3.Distance(attacker.position, rewindedPosition);
        
        if (distance <= attacker.meleeRange) {
            // Hit confirmed - apply damage
            target.TakeDamage(attacker.meleeDamage);
            
            // Send hit confirmation to clients
            BroadcastHitConfirmation(attacker.id, targetId);
        } else {
            // Miss - notify attacker
            SendMissNotification(attacker.id);
        }
    }
    
    // Resource gathering with lag compensation
    public void ProcessGatherAction(Player player, int resourceId, float clientTimestamp) {
        // Verify resource still exists at client's view
        float rewindTime = Time.time - GetPlayerLatency(player.id);
        
        if (ResourceExistedAtTime(resourceId, rewindTime)) {
            // Valid gather - award resources
            player.inventory.Add(GetResourceType(resourceId), amount);
            DestroyResource(resourceId);
        }
    }
}
```

**Limits and Exploits:**

```csharp
public class LagCompensationLimits {
    // Prevent abuse of lag compensation
    public const float MaxCompensationTime = 0.5f; // 500ms max
    public const float MinValidLatency = 0.01f;     // 10ms min
    
    public bool ValidateCompensation(float requestedLatency) {
        if (requestedLatency < MinValidLatency) {
            // Suspiciously low latency
            return false;
        }
        
        if (requestedLatency > MaxCompensationTime) {
            // Excessive latency - cap compensation
            return false;
        }
        
        // Check if consistent with measured latency
        float measuredLatency = GetMeasuredLatency();
        float difference = Mathf.Abs(requestedLatency - measuredLatency);
        
        if (difference > 0.1f) {
            // Client claims significantly different latency
            // Possible lag switch exploit
            return false;
        }
        
        return true;
    }
}
```

---

## Part V: Implementation Architecture

### 6. Complete Network Architecture

**Client Architecture:**

```csharp
public class NetworkedGameClient {
    // Components
    private ClientPrediction prediction;
    private EntityInterpolation interpolation;
    private InputBuffer inputBuffer;
    
    void Start() {
        prediction = new ClientPrediction();
        interpolation = new EntityInterpolation();
        inputBuffer = new InputBuffer();
    }
    
    void Update() {
        // 1. Process player input with prediction
        PlayerInput input = CaptureInput();
        prediction.PredictMovement(input);
        
        // 2. Send input to server
        SendInputToServer(input);
        
        // 3. Interpolate remote entities
        foreach (var entity in remoteEntities) {
            interpolation.UpdateEntity(entity);
        }
    }
    
    void OnServerUpdate(ServerSnapshot snapshot) {
        // 4. Reconcile local prediction
        prediction.Reconcile(snapshot.playerState);
        
        // 5. Update remote entity states
        foreach (var entityState in snapshot.entities) {
            interpolation.AddState(entityState);
        }
    }
}
```

**Server Architecture:**

```csharp
public class NetworkedGameServer {
    // Components
    private LagCompensation lagCompensation;
    private ServerSimulation simulation;
    private SnapshotManager snapshotManager;
    
    void FixedUpdate() {
        // 1. Record state history for lag compensation
        lagCompensation.RecordState();
        
        // 2. Process client inputs
        foreach (var client in clients) {
            ProcessClientInputs(client);
        }
        
        // 3. Simulate game world
        simulation.Tick();
        
        // 4. Generate snapshots for clients
        foreach (var client in clients) {
            ServerSnapshot snapshot = snapshotManager.GenerateSnapshot(client);
            SendToClient(client, snapshot);
        }
    }
    
    void ProcessClientInputs(Client client) {
        // Get buffered inputs
        Queue<PlayerInput> inputs = client.GetPendingInputs();
        
        while (inputs.Count > 0) {
            var input = inputs.Dequeue();
            
            // Validate input
            if (ValidateInput(input)) {
                // Apply to player entity
                ApplyInput(client.playerEntity, input);
                
                // Update last processed input ID
                client.lastProcessedInputId = input.id;
            }
        }
    }
}
```

### 7. Performance Optimization

**Bandwidth Optimization:**

```csharp
public class BandwidthOptimization {
    // Delta compression for state updates
    public byte[] CompressState(EntityState current, EntityState previous) {
        BitWriter writer = new BitWriter();
        
        // Only send changed values
        if (current.position != previous.position) {
            writer.WriteBit(true);
            writer.WriteVector3(current.position);
        } else {
            writer.WriteBit(false);
        }
        
        if (current.rotation != previous.rotation) {
            writer.WriteBit(true);
            writer.WriteQuaternion(current.rotation);
        } else {
            writer.WriteBit(false);
        }
        
        return writer.ToArray();
    }
    
    // Quantization for position/rotation
    public void QuantizePosition(ref Vector3 position) {
        // Reduce precision to 0.01 units
        position.x = Mathf.Round(position.x * 100f) / 100f;
        position.y = Mathf.Round(position.y * 100f) / 100f;
        position.z = Mathf.Round(position.z * 100f) / 100f;
    }
    
    // Priority-based updates
    public List<Entity> PrioritizeUpdates(Player viewer) {
        return allEntities
            .OrderBy(e => Vector3.Distance(e.position, viewer.position))
            .ThenBy(e => e.lastUpdateTime)
            .Take(maxUpdatesPerFrame)
            .ToList();
    }
}
```

**CPU Optimization:**

```csharp
public class PerformanceOptimization {
    // Spatial partitioning for lag compensation lookups
    private SpatialHash spatialHash;
    
    public List<Entity> FindEntitiesNearTarget(Vector3 target, float radius) {
        // O(1) lookup instead of O(n) iteration
        return spatialHash.Query(target, radius);
    }
    
    // Caching historical states
    private ObjectPool<HistoricalState> statePool;
    
    public HistoricalState GetPooledState() {
        return statePool.Get();
    }
    
    public void ReturnState(HistoricalState state) {
        statePool.Return(state);
    }
}
```

---

## Implementation Recommendations

### For BlueMarble MMORPG

**1. Client-Side Prediction:**
   - Apply to player movement, skill casting, resource gathering
   - Use identical simulation code on client and server
   - Implement smooth reconciliation with 100ms blend time
   - Handle edge cases: collision, terrain boundaries, server overrides

**2. Entity Interpolation:**
   - Buffer 100ms of state updates for other players
   - Adaptive buffer size based on packet loss
   - Prioritize nearby entities (update rate based on distance)
   - Extrapolation fallback for packet loss (max 200ms)

**3. Lag Compensation:**
   - Store 500ms of position history server-side
   - Apply to combat (melee, ranged, spells)
   - Apply to resource gathering interactions
   - Validate client timestamps to prevent exploits
   - Cap compensation at 500ms to prevent abuse

**4. Network Protocol:**
   - Reliable ordered channel for critical events (inventory, quests)
   - Unreliable sequenced channel for entity updates (positions)
   - Delta compression for state updates
   - Quantize positions to 0.01m, rotations to 1 degree

**5. Performance Budgets:**
   - Client prediction: <1ms per frame
   - Interpolation: <2ms for 100 entities
   - Server lag compensation: <5ms per combat action
   - Network bandwidth: <10KB/s per player (entity updates)

### Anti-Cheat Considerations

**Server Authority:**
```csharp
public class ServerAuthority {
    // Validate client predictions
    public bool ValidateMovement(Vector3 clientPos, Vector3 serverPos) {
        float maxSpeed = 10f; // m/s
        float deltaTime = Time.deltaTime;
        float maxDistance = maxSpeed * deltaTime * 1.1f; // 10% tolerance
        
        float distance = Vector3.Distance(clientPos, serverPos);
        
        if (distance > maxDistance) {
            // Client moved too far - possible speed hack
            return false;
        }
        
        return true;
    }
    
    // Validate action timing
    public bool ValidateActionTiming(float clientTime, float serverTime) {
        float latency = serverTime - clientTime;
        
        if (latency < 0 || latency > 1.0f) {
            // Invalid timestamp - possible manipulation
            return false;
        }
        
        return true;
    }
}
```

---

## References

### Primary Source
1. Bernier, Y. W. (2001). "Latency Compensating Methods in Client/Server In-game Protocol Design and Optimization". Game Developers Conference (GDC).
   - Available: https://developer.valvesoftware.com/wiki/Latency_Compensating_Methods

### Related Valve/Source Engine Documentation
2. Valve Software. "Source Multiplayer Networking". Valve Developer Community.
3. Valve Software. "Prediction". Valve Developer Community.
4. Valve Software. "Lag Compensation". Valve Developer Community.

### Academic Papers
5. Aggarwal, S., et al. (2004). "Accuracy in Dead-Reckoning Based Distributed Multi-Player Games". NetGames.
6. Claypool, M., & Claypool, K. (2006). "Latency and Player Actions in Online Games". Communications of the ACM.

### Industry Implementation
7. Fiedler, G. "Networked Physics". Gaffer On Games.
8. "Overwatch Gameplay Architecture and Netcode". Blizzard Entertainment GDC Talk.
9. "Rocket League Networking". Psyonix Engineering Blog.

### Related BlueMarble Research
- [game-dev-analysis-2d-game-development-with-unity.md](game-dev-analysis-2d-game-development-with-unity.md) - Network-optimized rendering
- [game-dev-analysis-unity-2d-documentation-best-practices.md](game-dev-analysis-unity-2d-documentation-best-practices.md) - Performance optimization
- [online-game-dev-resources.md](online-game-dev-resources.md) - Source catalog

---

**Document Status:** Complete  
**Assignment Group:** 37 (Discovered Source #2)  
**Discovered From:** 2D Game Development with Unity (networking patterns)  
**Lines:** 612  
**Last Updated:** 2025-01-17  
**Next Steps:** 
- Implement client-side prediction for player movement
- Add lag compensation to combat system
- Profile network bandwidth and optimize state updates
- Integrate with spatial partitioning for efficient lookups

---

**Contribution to Phase 1 Research:**

This analysis provides the fundamental networking architecture for BlueMarble's multiplayer gameplay. Valve's techniques are industry-proven and address the core challenge of responsive gameplay over the internet. The patterns documented here—particularly client-side prediction, server reconciliation, and lag compensation—are essential for creating a satisfying MMORPG experience.

**Key Contributions:**
- ✅ Documented client-side prediction algorithm for responsive input
- ✅ Established server reconciliation pattern for authoritative state
- ✅ Defined entity interpolation for smooth remote player movement
- ✅ Provided lag compensation implementation for fair combat
- ✅ Created performance budgets and anti-cheat considerations

**Integration Points:**
- Player movement and input handling system
- Combat resolution and hit detection
- Entity synchronization and state management
- Network protocol design and bandwidth optimization
- Anti-cheat and server authority validation
