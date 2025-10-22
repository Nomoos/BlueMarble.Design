# Int32 Coordinate System - Network Protocol Design

**Status**: Draft - Design Ready
**Protocol**: Binary + Delta Encoding
**Priority**: High
**Bandwidth Target**: <100 KB/s per player for position updates

---

## Executive Summary

This document defines the network protocol for transmitting Int32 (centimeter) coordinates in BlueMarble. The design optimizes for:

- **Bandwidth efficiency**: Delta encoding reduces payload by 50-70%
- **Low latency**: Binary protocol minimizes serialization overhead
- **Accuracy**: Integer coordinates maintain exact precision over network
- **Scalability**: Supports 10,000-50,000 concurrent players

**Key Feature**: Delta encoding transmits only position changes, reducing typical payloads from 12 bytes to 6-8 bytes per entity.

---

## Table of Contents

1. [Protocol Overview](#protocol-overview)
2. [Message Formats](#message-formats)
3. [Delta Encoding](#delta-encoding)
4. [Compression Strategy](#compression-strategy)
5. [Update Frequency](#update-frequency)
6. [Interpolation](#interpolation)
7. [Bandwidth Analysis](#bandwidth-analysis)
8. [Implementation](#implementation)

---

## Protocol Overview

### Design Principles

1. **Binary Protocol**: Use binary encoding for efficiency (not JSON/XML)
2. **Delta Encoding**: Transmit position deltas instead of absolute positions
3. **Variable Length**: Use shorter encodings for small movements
4. **Batch Updates**: Group multiple entity updates in single packet
5. **Priority System**: Critical updates (players) have higher frequency

### Message Structure

```
[Header][Payload][Checksum]

Header (8 bytes):
- Message Type (1 byte)
- Message Length (2 bytes)
- Timestamp (4 bytes)
- Flags (1 byte)

Payload (variable):
- Entity updates (variable length per entity)

Checksum (2 bytes):
- CRC16 for error detection
```

---

## Message Formats

### Full Position Update

**Use Case**: Initial position, teleport, respawn

```
Message Type: 0x01 (POSITION_FULL)

Per Entity (17 bytes):
- Entity ID (4 bytes, uint32)
- Position X (4 bytes, int32)
- Position Y (4 bytes, int32)
- Position Z (4 bytes, int32)
- Flags (1 byte)

Total: 17 bytes per entity
```

**C# Structure**:
```csharp
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct FullPositionUpdate
{
    public uint EntityId;        // 4 bytes
    public int PositionX;         // 4 bytes (centimeters)
    public int PositionY;         // 4 bytes (centimeters)
    public int PositionZ;         // 4 bytes (centimeters)
    public byte Flags;            // 1 byte
    
    public const int SIZE = 17;
}
```

### Delta Position Update (Small)

**Use Case**: Normal movement (delta fits in Int16)

```
Message Type: 0x02 (POSITION_DELTA_SMALL)

Per Entity (11 bytes):
- Entity ID (4 bytes, uint32)
- Delta X (2 bytes, int16)
- Delta Y (2 bytes, int16)
- Delta Z (2 bytes, int16)
- Flags (1 byte)

Total: 11 bytes per entity
Range: ±327.67 meters per update
```

**C# Structure**:
```csharp
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct SmallDeltaUpdate
{
    public uint EntityId;        // 4 bytes
    public short DeltaX;         // 2 bytes (centimeters)
    public short DeltaY;         // 2 bytes (centimeters)
    public short DeltaZ;         // 2 bytes (centimeters)
    public byte Flags;           // 1 byte
    
    public const int SIZE = 11;
}
```

### Delta Position Update (Tiny)

**Use Case**: Very small movements (delta fits in Int8)

```
Message Type: 0x03 (POSITION_DELTA_TINY)

Per Entity (8 bytes):
- Entity ID (4 bytes, uint32)
- Delta X (1 byte, int8)
- Delta Y (1 byte, int8)
- Delta Z (1 byte, int8)
- Flags (1 byte)

Total: 8 bytes per entity
Range: ±1.27 meters per update
```

**C# Structure**:
```csharp
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct TinyDeltaUpdate
{
    public uint EntityId;        // 4 bytes
    public sbyte DeltaX;         // 1 byte (centimeters)
    public sbyte DeltaY;         // 1 byte (centimeters)
    public sbyte DeltaZ;         // 1 byte (centimeters)
    public byte Flags;           // 1 byte
    
    public const int SIZE = 8;
}
```

### Batch Update Message

```
Message Type: 0x10 (BATCH_UPDATE)

Header (8 bytes):
- Message Type (1 byte): 0x10
- Entity Count (2 bytes, uint16)
- Timestamp (4 bytes, uint32)
- Flags (1 byte)

Payload (variable):
- Update Type (1 byte per entity)
- Update Data (8-17 bytes per entity)

Total: 8 + (9-18 bytes × entity count)
```

---

## Delta Encoding

### Algorithm

```csharp
public class PositionDeltaEncoder
{
    private Dictionary<uint, WorldCoordinate> _lastPositions = new();
    
    public byte[] EncodeUpdate(uint entityId, WorldCoordinate newPosition)
    {
        if (!_lastPositions.TryGetValue(entityId, out var lastPosition))
        {
            // First update - send full position
            _lastPositions[entityId] = newPosition;
            return EncodeFullPosition(entityId, newPosition);
        }
        
        // Calculate delta
        int dx = newPosition.X - lastPosition.X;
        int dy = newPosition.Y - lastPosition.Y;
        int dz = newPosition.Z - lastPosition.Z;
        
        // Choose appropriate encoding
        if (FitsTiny(dx, dy, dz))
        {
            _lastPositions[entityId] = newPosition;
            return EncodeTinyDelta(entityId, dx, dy, dz);
        }
        else if (FitsSmall(dx, dy, dz))
        {
            _lastPositions[entityId] = newPosition;
            return EncodeSmallDelta(entityId, dx, dy, dz);
        }
        else
        {
            // Delta too large - send full position
            _lastPositions[entityId] = newPosition;
            return EncodeFullPosition(entityId, newPosition);
        }
    }
    
    private bool FitsTiny(int dx, int dy, int dz)
    {
        return dx >= sbyte.MinValue && dx <= sbyte.MaxValue &&
               dy >= sbyte.MinValue && dy <= sbyte.MaxValue &&
               dz >= sbyte.MinValue && dz <= sbyte.MaxValue;
    }
    
    private bool FitsSmall(int dx, int dy, int dz)
    {
        return dx >= short.MinValue && dx <= short.MaxValue &&
               dy >= short.MinValue && dy <= short.MaxValue &&
               dz >= short.MinValue && dz <= short.MaxValue;
    }
    
    private byte[] EncodeFullPosition(uint entityId, WorldCoordinate pos)
    {
        var buffer = new byte[FullPositionUpdate.SIZE];
        using (var stream = new MemoryStream(buffer))
        using (var writer = new BinaryWriter(stream))
        {
            writer.Write(entityId);
            writer.Write(pos.X);
            writer.Write(pos.Y);
            writer.Write(pos.Z);
            writer.Write((byte)0); // Flags
        }
        return buffer;
    }
    
    private byte[] EncodeSmallDelta(uint entityId, int dx, int dy, int dz)
    {
        var buffer = new byte[SmallDeltaUpdate.SIZE];
        using (var stream = new MemoryStream(buffer))
        using (var writer = new BinaryWriter(stream))
        {
            writer.Write(entityId);
            writer.Write((short)dx);
            writer.Write((short)dy);
            writer.Write((short)dz);
            writer.Write((byte)0); // Flags
        }
        return buffer;
    }
    
    private byte[] EncodeTinyDelta(uint entityId, int dx, int dy, int dz)
    {
        var buffer = new byte[TinyDeltaUpdate.SIZE];
        using (var stream = new MemoryStream(buffer))
        using (var writer = new BinaryWriter(stream))
        {
            writer.Write(entityId);
            writer.Write((sbyte)dx);
            writer.Write((sbyte)dy);
            writer.Write((sbyte)dz);
            writer.Write((byte)0); // Flags
        }
        return buffer;
    }
}
```

### Decoding

```csharp
public class PositionDeltaDecoder
{
    private Dictionary<uint, WorldCoordinate> _currentPositions = new();
    
    public WorldCoordinate DecodeUpdate(byte messageType, byte[] data)
    {
        using (var stream = new MemoryStream(data))
        using (var reader = new BinaryReader(stream))
        {
            uint entityId = reader.ReadUInt32();
            
            switch (messageType)
            {
                case 0x01: // Full position
                {
                    var position = new WorldCoordinate(
                        reader.ReadInt32(),
                        reader.ReadInt32(),
                        reader.ReadInt32()
                    );
                    _currentPositions[entityId] = position;
                    return position;
                }
                
                case 0x02: // Small delta
                {
                    if (!_currentPositions.TryGetValue(entityId, out var current))
                        throw new Exception("Missing base position for delta");
                    
                    var delta = new WorldCoordinate(
                        reader.ReadInt16(),
                        reader.ReadInt16(),
                        reader.ReadInt16()
                    );
                    var newPosition = current + delta;
                    _currentPositions[entityId] = newPosition;
                    return newPosition;
                }
                
                case 0x03: // Tiny delta
                {
                    if (!_currentPositions.TryGetValue(entityId, out var current))
                        throw new Exception("Missing base position for delta");
                    
                    var delta = new WorldCoordinate(
                        reader.ReadSByte(),
                        reader.ReadSByte(),
                        reader.ReadSByte()
                    );
                    var newPosition = current + delta;
                    _currentPositions[entityId] = newPosition;
                    return newPosition;
                }
                
                default:
                    throw new Exception($"Unknown message type: {messageType}");
            }
        }
    }
}
```

---

## Compression Strategy

### LZ4 Compression

Apply LZ4 compression to batched updates for additional bandwidth savings.

```csharp
public byte[] CompressBatch(List<byte[]> updates)
{
    // Concatenate all updates
    int totalSize = updates.Sum(u => u.Length);
    var uncompressed = new byte[totalSize];
    int offset = 0;
    
    foreach (var update in updates)
    {
        Buffer.BlockCopy(update, 0, uncompressed, offset, update.Length);
        offset += update.Length;
    }
    
    // Apply LZ4 compression
    var compressed = LZ4.Compress(uncompressed);
    
    // Only use if compression ratio is good (>20% savings)
    if (compressed.Length < uncompressed.Length * 0.8)
    {
        return compressed;
    }
    else
    {
        return uncompressed; // Compression not worth it
    }
}
```

### Expected Compression Ratios

```
Uncompressed batch (100 tiny deltas): 800 bytes
LZ4 compressed:                       ~400-500 bytes
Compression ratio:                    ~40-50%

Uncompressed batch (100 small deltas): 1,100 bytes
LZ4 compressed:                        ~600-700 bytes
Compression ratio:                     ~35-45%
```

---

## Update Frequency

### Priority Tiers

| Entity Type | Update Rate | Bytes/Update | Bandwidth |
|-------------|------------|--------------|-----------|
| Local Player | 20 Hz | 8-11 | 160-220 B/s |
| Nearby Players | 10 Hz | 8-11 | 80-110 B/s |
| Nearby NPCs | 5 Hz | 8 | 40 B/s |
| Distant Entities | 1 Hz | 8 | 8 B/s |
| Static Objects | 0.1 Hz | 17 | 1.7 B/s |

### Adaptive Rate Control

```csharp
public class AdaptiveUpdateScheduler
{
    public float GetUpdateRate(Entity entity, WorldCoordinate playerPosition)
    {
        var distance = entity.Position.Distance(playerPosition);
        var distanceMeters = distance / 100.0; // Convert cm to meters
        
        if (entity.IsPlayer)
        {
            if (distanceMeters < 50)      return 20.0f; // 20 Hz
            else if (distanceMeters < 200) return 10.0f; // 10 Hz
            else if (distanceMeters < 500) return 5.0f;  // 5 Hz
            else                          return 2.0f;  // 2 Hz
        }
        else if (entity.IsNPC)
        {
            if (distanceMeters < 50)      return 10.0f;
            else if (distanceMeters < 200) return 5.0f;
            else if (distanceMeters < 500) return 2.0f;
            else                          return 1.0f;
        }
        else // Static
        {
            return 0.1f; // Very infrequent updates
        }
    }
}
```

---

## Interpolation

### Client-Side Prediction

```csharp
public class PositionInterpolator
{
    private WorldCoordinate _lastPosition;
    private WorldCoordinate _targetPosition;
    private float _interpolationTime;
    private float _totalTime;
    
    public void ReceiveUpdate(WorldCoordinate newPosition, float updateRate)
    {
        _lastPosition = _targetPosition;
        _targetPosition = newPosition;
        _interpolationTime = 0;
        _totalTime = 1.0f / updateRate; // Time until next update
    }
    
    public WorldCoordinate GetInterpolatedPosition(float deltaTime)
    {
        _interpolationTime += deltaTime;
        float t = Math.Min(_interpolationTime / _totalTime, 1.0f);
        
        // Linear interpolation
        return new WorldCoordinate(
            (int)(_lastPosition.X + (_targetPosition.X - _lastPosition.X) * t),
            (int)(_lastPosition.Y + (_targetPosition.Y - _lastPosition.Y) * t),
            (int)(_lastPosition.Z + (_targetPosition.Z - _lastPosition.Z) * t)
        );
    }
}
```

### Extrapolation for High Latency

```csharp
public WorldCoordinate ExtrapolatePosition(
    WorldCoordinate lastPosition,
    WorldCoordinate velocity,
    float latencySeconds)
{
    // Velocity in cm/s, latency in seconds
    int extrapolatedX = lastPosition.X + (int)(velocity.X * latencySeconds);
    int extrapolatedY = lastPosition.Y + (int)(velocity.Y * latencySeconds);
    int extrapolatedZ = lastPosition.Z + (int)(velocity.Z * latencySeconds);
    
    return new WorldCoordinate(extrapolatedX, extrapolatedY, extrapolatedZ);
}
```

---

## Bandwidth Analysis

### Per-Player Bandwidth (Download)

**Scenario**: Player in populated area (100 nearby entities)

```
Entity Updates:
- 10 nearby players @ 10 Hz × 8 bytes     = 800 B/s
- 50 NPCs @ 5 Hz × 8 bytes                = 2,000 B/s
- 40 distant entities @ 1 Hz × 8 bytes    = 320 B/s

Subtotal (entity updates):                  3,120 B/s

Other Game Data:
- Chat messages                             500 B/s
- Combat events                             1,000 B/s
- UI updates                                500 B/s

Total Download Bandwidth:                   ~5.1 KB/s
```

**With compression (40% reduction)**:       ~3.1 KB/s

### Per-Player Bandwidth (Upload)

```
Player Position Updates:
- Self position @ 20 Hz × 8 bytes          = 160 B/s

Player Actions:
- Combat actions                           = 500 B/s
- Interactions                             = 200 B/s

Total Upload Bandwidth:                     ~0.9 KB/s
```

### Server Bandwidth (10,000 Players)

```
Download (Server → Clients):
- 10,000 players × 3.1 KB/s               = 31 MB/s = 248 Mbps

Upload (Clients → Server):
- 10,000 players × 0.9 KB/s               = 9 MB/s = 72 Mbps

Total Server Bandwidth:                     40 MB/s = 320 Mbps
```

**Conclusion**: Server bandwidth requirements are reasonable for modern infrastructure.

---

## Implementation

### Server-Side Broadcast

```csharp
public class PositionBroadcastSystem
{
    private PositionDeltaEncoder _encoder = new();
    private Dictionary<uint, List<uint>> _playerVisibility = new();
    
    public void BroadcastPositionUpdate(
        uint entityId,
        WorldCoordinate newPosition)
    {
        // Encode update
        var updateData = _encoder.EncodeUpdate(entityId, newPosition);
        
        // Find players who can see this entity
        var visiblePlayers = GetVisiblePlayers(entityId, newPosition);
        
        // Send to each player
        foreach (var playerId in visiblePlayers)
        {
            SendToPlayer(playerId, updateData);
        }
    }
    
    private List<uint> GetVisiblePlayers(uint entityId, WorldCoordinate position)
    {
        // Query spatial index for nearby players
        var nearbyPlayers = SpatialIndex.QueryRadius(
            position,
            visibilityRange: 50000 // 500m in centimeters
        );
        
        return nearbyPlayers;
    }
    
    public void BroadcastBatch(List<(uint entityId, WorldCoordinate position)> updates)
    {
        // Group by visible players
        var playerUpdates = new Dictionary<uint, List<byte[]>>();
        
        foreach (var (entityId, position) in updates)
        {
            var encoded = _encoder.EncodeUpdate(entityId, position);
            var visiblePlayers = GetVisiblePlayers(entityId, position);
            
            foreach (var playerId in visiblePlayers)
            {
                if (!playerUpdates.ContainsKey(playerId))
                    playerUpdates[playerId] = new List<byte[]>();
                
                playerUpdates[playerId].Add(encoded);
            }
        }
        
        // Send batched updates
        foreach (var (playerId, updates) in playerUpdates)
        {
            var compressed = CompressBatch(updates);
            SendToPlayer(playerId, compressed);
        }
    }
}
```

### Client-Side Reception

```csharp
public class PositionReceiver
{
    private PositionDeltaDecoder _decoder = new();
    private Dictionary<uint, PositionInterpolator> _interpolators = new();
    
    public void OnReceivePacket(byte[] data)
    {
        using (var stream = new MemoryStream(data))
        using (var reader = new BinaryReader(stream))
        {
            byte messageType = reader.ReadByte();
            ushort entityCount = reader.ReadUInt16();
            uint timestamp = reader.ReadUInt32();
            
            for (int i = 0; i < entityCount; i++)
            {
                byte updateType = reader.ReadByte();
                int updateSize = GetUpdateSize(updateType);
                var updateData = reader.ReadBytes(updateSize);
                
                var position = _decoder.DecodeUpdate(updateType, updateData);
                uint entityId = BitConverter.ToUInt32(updateData, 0);
                
                // Update interpolator
                if (!_interpolators.ContainsKey(entityId))
                    _interpolators[entityId] = new PositionInterpolator();
                
                _interpolators[entityId].ReceiveUpdate(position, GetUpdateRate(updateType));
            }
        }
    }
    
    public WorldCoordinate GetEntityPosition(uint entityId, float deltaTime)
    {
        if (_interpolators.TryGetValue(entityId, out var interpolator))
        {
            return interpolator.GetInterpolatedPosition(deltaTime);
        }
        return default;
    }
}
```

---

## Related Documents

- [Int32 Implementation Specification](int32-coordinate-implementation-specification.md) - Code implementation
- [Int32 Database Schema](int32-coordinate-database-schema.md) - Database design
- [ADR-001: Coordinate Data Type Selection](adr-001-coordinate-data-type-selection.md) - Architecture decision

---

## Success Criteria

### Bandwidth Efficiency
- ✅ <5 KB/s per player (average)
- ✅ Delta encoding reduces payload by 50-70%
- ✅ Compression adds 35-45% savings
- ✅ Total bandwidth <3.1 KB/s per player

### Latency
- ✅ Serialization overhead <0.1ms
- ✅ Network jitter <50ms
- ✅ Interpolation smooth at 10-20 Hz

### Scalability
- ✅ Supports 10,000 concurrent players
- ✅ Server bandwidth <400 Mbps
- ✅ CPU overhead <5% for encoding/decoding

### Accuracy
- ✅ No precision loss over network
- ✅ Deterministic across platforms
- ✅ Error correction for packet loss
