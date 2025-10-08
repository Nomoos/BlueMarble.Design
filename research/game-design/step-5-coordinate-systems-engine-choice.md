# Step 5: Coordinate System & Engine Choice Guide

This guide provides detailed technical guidance on coordinate systems and engine selection for planet-scale MMORPG development, expanding on the principles outlined in the [Planet-Scale MMORPG GIS Architecture and Simulation Plan](../../ArchitectureAndSimulationPlan.md).

## Overview

Selecting the right coordinate system and game engine is foundational to building a planet-scale world. These decisions affect precision, performance, and every aspect of gameplay from rendering to physics simulation.

---

## 1. Double/64-bit Coordinate Systems

### The Precision Problem at Planetary Scale

At Earth scale (40,075,020m × 20,037,510m), traditional 32-bit floating-point coordinates fail catastrophically:

```
32-bit Float Precision Analysis:
- Mantissa: 23 bits
- Precision at origin (0,0,0): ~0.0001m (0.1mm)
- Precision at 1,000m: ~0.001m (1mm)
- Precision at 10,000m: ~0.01m (1cm)
- Precision at 100,000m: ~0.1m (10cm) ❌ UNACCEPTABLE
- Precision at 1,000,000m: ~1m ❌ CATASTROPHIC
- Precision at 40,000,000m: ~5-10m ❌ GAME-BREAKING
```

**Visual Consequences**:
- Object jittering and vibration
- Z-fighting (flickering surfaces)
- Physics instability
- Collision detection failures
- Animation glitches

### Solution 1: 64-bit Integer Coordinates

**BlueMarble Design Approach**:

```csharp
/// <summary>
/// World position using 64-bit integers for meter-level precision
/// across the entire planet.
/// </summary>
public struct WorldPosition : IEquatable<WorldPosition>
{
    /// <summary>X coordinate in meters from world origin</summary>
    public long X { get; }
    
    /// <summary>Y coordinate in meters from world origin</summary>
    public long Y { get; }
    
    /// <summary>Z coordinate in meters from sea level (±10,000 km range)</summary>
    public long Z { get; }
    
    public WorldPosition(long x, long y, long z)
    {
        X = x;
        Y = y;
        Z = z;
    }
    
    /// <summary>
    /// Convert to local float coordinates for rendering.
    /// Origin should be camera or player position.
    /// </summary>
    public Vector3 ToLocalFloat(WorldPosition origin)
    {
        return new Vector3(
            (float)(X - origin.X),
            (float)(Y - origin.Y),
            (float)(Z - origin.Z)
        );
    }
    
    /// <summary>
    /// Calculate distance between two world positions.
    /// Returns double for precision at large distances.
    /// </summary>
    public double DistanceTo(WorldPosition other)
    {
        long dx = X - other.X;
        long dy = Y - other.Y;
        long dz = Z - other.Z;
        
        return Math.Sqrt(dx * dx + dy * dy + dz * dz);
    }
    
    public bool Equals(WorldPosition other)
    {
        return X == other.X && Y == other.Y && Z == other.Z;
    }
    
    public override int GetHashCode()
    {
        return HashCode.Combine(X, Y, Z);
    }
}
```

**Advantages**:
- ✅ Exact meter precision across entire world
- ✅ No floating-point drift
- ✅ Deterministic calculations
- ✅ Simple arithmetic
- ✅ Network-friendly (fixed size)

**Disadvantages**:
- ❌ Sub-meter precision requires separate handling
- ❌ Requires conversion for rendering
- ❌ Physics engines need adaptation

### Solution 2: 64-bit Double Coordinates

**Unreal Engine 5 LWC (Large World Coordinates) Approach**:

```cpp
/// <summary>
/// World position using 64-bit doubles.
/// Provides sub-millimeter precision at planetary scale.
/// </summary>
struct FVector3d
{
    double X;
    double Y;
    double Z;
    
    FVector3d(double InX, double InY, double InZ)
        : X(InX), Y(InY), Z(InZ) {}
    
    /// <summary>
    /// Convert to camera-relative single precision for rendering.
    /// UE5 does this automatically via LWC system.
    /// </summary>
    FVector ToFloat(const FVector3d& CameraPosition) const
    {
        return FVector(
            static_cast<float>(X - CameraPosition.X),
            static_cast<float>(Y - CameraPosition.Y),
            static_cast<float>(Z - CameraPosition.Z)
        );
    }
    
    /// <summary>
    /// Distance calculation maintains precision.
    /// </summary>
    double Distance(const FVector3d& Other) const
    {
        double dx = X - Other.X;
        double dy = Y - Other.Y;
        double dz = Z - Other.Z;
        
        return sqrt(dx * dx + dy * dy + dz * dz);
    }
};
```

**Advantages**:
- ✅ Sub-millimeter precision at 40,000km
- ✅ Smooth floating-point operations
- ✅ Natural for physics calculations
- ✅ Industry standard (UE5, Unity DOTS)

**Disadvantages**:
- ❌ Slightly less deterministic than integers
- ❌ Larger network bandwidth (if not compressed)
- ❌ Potential for precision loss with naive operations

### Precision Comparison Table

| Distance from Origin | 32-bit Float | 64-bit Integer | 64-bit Double |
|---------------------|--------------|----------------|---------------|
| 0 m | 0.1 mm | 1 m | 0.0001 mm |
| 100 m | 1 mm | 1 m | 0.001 mm |
| 1,000 m | 1 mm | 1 m | 0.01 mm |
| 10,000 m | 1 cm | 1 m | 0.1 mm |
| 100,000 m | 10 cm ❌ | 1 m | 1 mm |
| 1,000,000 m | 1 m ❌ | 1 m | 1 cm |
| 40,000,000 m | 5-10 m ❌ | 1 m | 5 cm |

### Hybrid Approach: Best of Both Worlds

```csharp
/// <summary>
/// Hybrid coordinate system combining integer world coordinates
/// with local floating-point for sub-meter precision.
/// </summary>
public struct PreciseWorldPosition
{
    // Meter-level precision using integers
    public long MeterX { get; }
    public long MeterY { get; }
    public long MeterZ { get; }
    
    // Sub-meter precision using floats (0.0 to 1.0)
    public float SubMeterX { get; }
    public float SubMeterY { get; }
    public float SubMeterZ { get; }
    
    public PreciseWorldPosition(long meterX, long meterY, long meterZ,
                                float subX = 0f, float subY = 0f, float subZ = 0f)
    {
        MeterX = meterX;
        MeterY = meterY;
        MeterZ = meterZ;
        SubMeterX = Math.Clamp(subX, 0f, 1f);
        SubMeterY = Math.Clamp(subY, 0f, 1f);
        SubMeterZ = Math.Clamp(subZ, 0f, 1f);
    }
    
    /// <summary>
    /// Convert to pure double representation (for calculations).
    /// </summary>
    public (double x, double y, double z) ToDouble()
    {
        return (
            MeterX + SubMeterX,
            MeterY + SubMeterY,
            MeterZ + SubMeterZ
        );
    }
    
    /// <summary>
    /// Create from continuous double coordinates.
    /// </summary>
    public static PreciseWorldPosition FromDouble(double x, double y, double z)
    {
        return new PreciseWorldPosition(
            (long)Math.Floor(x),
            (long)Math.Floor(y),
            (long)Math.Floor(z),
            (float)(x - Math.Floor(x)),
            (float)(y - Math.Floor(y)),
            (float)(z - Math.Floor(z))
        );
    }
}
```

---

## 2. Floating Origin Shifting

### Concept

Floating origin continuously relocates the coordinate system origin to keep the active player/camera near (0,0,0), maintaining maximum floating-point precision for rendering and physics.

### When to Shift Origin

```csharp
public class FloatingOriginManager
{
    // Shift origin when player moves this far from current origin
    private const double SHIFT_THRESHOLD = 5000.0; // 5 km
    
    private WorldPosition currentOrigin;
    private WorldPosition playerWorldPos;
    
    public void Update(WorldPosition newPlayerPosition)
    {
        playerWorldPos = newPlayerPosition;
        
        // Calculate distance from current origin
        double distance = playerWorldPos.DistanceTo(currentOrigin);
        
        if (distance > SHIFT_THRESHOLD)
        {
            ShiftOrigin(playerWorldPos);
        }
    }
    
    private void ShiftOrigin(WorldPosition newOrigin)
    {
        WorldPosition shift = new WorldPosition(
            newOrigin.X - currentOrigin.X,
            newOrigin.Y - currentOrigin.Y,
            newOrigin.Z - currentOrigin.Z
        );
        
        // Update all subsystems
        UpdateEntities(shift);
        UpdatePhysics(shift);
        UpdateRendering(shift);
        UpdateNetworking(newOrigin);
        
        currentOrigin = newOrigin;
    }
}
```

### Entity Position Management

```csharp
public class Entity
{
    // World position (absolute, 64-bit)
    public WorldPosition WorldPos { get; set; }
    
    // Local position (relative to origin, 32-bit)
    // Used for rendering and physics
    private Vector3 localPosition;
    
    public void UpdateLocalPosition(WorldPosition origin)
    {
        localPosition = WorldPos.ToLocalFloat(origin);
    }
    
    public Vector3 GetRenderPosition()
    {
        return localPosition;
    }
}
```

### Multi-Player Origin Shifting

For multiplayer scenarios where players are far apart:

```csharp
public class RegionalOriginManager
{
    // Each region has its own local origin
    private Dictionary<RegionId, WorldPosition> regionOrigins;
    
    // Players in different regions see the world relative to their region
    public Vector3 GetRenderPositionForPlayer(
        Entity entity,
        Player observer)
    {
        // Get observer's region origin
        WorldPosition observerOrigin = regionOrigins[observer.RegionId];
        
        // Return entity position relative to observer's origin
        return entity.WorldPos.ToLocalFloat(observerOrigin);
    }
}
```

---

## 3. Coordinate Transform: EPSG:4087

### Equidistant Cylindrical Projection

BlueMarble uses EPSG:4087 (World Equidistant Cylindrical) for internal representation:

```csharp
public static class CoordinateTransforms
{
    // EPSG:4087 World Equidistant Cylindrical
    public const double WORLD_WIDTH_METERS = 40075020.0;  // Circumference at equator
    public const double WORLD_HEIGHT_METERS = 20037510.0; // Half circumference (pole to pole)
    
    public const double MIN_LONGITUDE = -180.0;
    public const double MAX_LONGITUDE = 180.0;
    public const double MIN_LATITUDE = -90.0;
    public const double MAX_LATITUDE = 90.0;
    
    /// <summary>
    /// Convert geodetic coordinates (lat/lon/altitude) to world coordinates.
    /// </summary>
    public static WorldPosition GeodeticToWorld(
        double latitude,  // degrees, -90 to 90
        double longitude, // degrees, -180 to 180
        double altitude)  // meters above sea level
    {
        // Equidistant cylindrical: simple linear mapping
        double x = (longitude - MIN_LONGITUDE) / (MAX_LONGITUDE - MIN_LONGITUDE) * WORLD_WIDTH_METERS;
        double y = (latitude - MIN_LATITUDE) / (MAX_LATITUDE - MIN_LATITUDE) * WORLD_HEIGHT_METERS;
        
        return new WorldPosition(
            (long)Math.Round(x),
            (long)Math.Round(y),
            (long)Math.Round(altitude)
        );
    }
    
    /// <summary>
    /// Convert world coordinates to geodetic (lat/lon/altitude).
    /// </summary>
    public static (double lat, double lon, double alt) WorldToGeodetic(
        WorldPosition worldPos)
    {
        double longitude = (double)worldPos.X / WORLD_WIDTH_METERS * 
                          (MAX_LONGITUDE - MIN_LONGITUDE) + MIN_LONGITUDE;
        double latitude = (double)worldPos.Y / WORLD_HEIGHT_METERS * 
                         (MAX_LATITUDE - MIN_LATITUDE) + MIN_LATITUDE;
        double altitude = worldPos.Z;
        
        return (latitude, longitude, altitude);
    }
    
    /// <summary>
    /// Calculate great circle distance between two geodetic positions.
    /// Uses Haversine formula for accuracy.
    /// </summary>
    public static double GreatCircleDistance(
        double lat1, double lon1,
        double lat2, double lon2)
    {
        const double EARTH_RADIUS = 6371000.0; // meters
        
        double dLat = (lat2 - lat1) * Math.PI / 180.0;
        double dLon = (lon2 - lon1) * Math.PI / 180.0;
        
        double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                   Math.Cos(lat1 * Math.PI / 180.0) * 
                   Math.Cos(lat2 * Math.PI / 180.0) *
                   Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
        
        double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
        
        return EARTH_RADIUS * c;
    }
}
```

### Why EPSG:4087?

**Advantages**:
- ✅ Simple linear mapping (lat/lon → x/y)
- ✅ Uniform grid cells
- ✅ Easy to understand and debug
- ✅ Fast coordinate conversions
- ✅ Suitable for game world chunking

**Disadvantages**:
- ❌ Distortion at poles (not uniform area)
- ❌ Not suitable for large-scale navigation
- ❌ Requires Haversine for accurate distances

**Alternative**: For scientific accuracy, consider Web Mercator (EPSG:3857) or UTM zones.

---

## 4. Game Engine Selection

### Unreal Engine 5 - Best for AAA Quality

**Large World Coordinates (LWC) System**:
- Native 64-bit double precision
- Automatic origin rebasing
- Supports worlds up to 88 million km
- Transparent conversion to 32-bit for rendering

```cpp
// UE5 Example: Using FVector3d (double precision)
void AMyActor::SetWorldPosition(const FVector3d& NewPosition)
{
    // UE5 LWC automatically handles precision
    SetActorLocation(NewPosition);
}

// Origin rebasing happens automatically
void AMyWorld::Tick(float DeltaTime)
{
    // LWC shifts origin when needed
    // Completely transparent to game code
}
```

**Pros**:
- ✅ Industry-leading graphics (Nanite, Lumen)
- ✅ Native 64-bit coordinate support
- ✅ Automatic origin shifting
- ✅ Excellent documentation
- ✅ Large asset marketplace

**Cons**:
- ❌ High learning curve
- ❌ Large engine size (100+ GB)
- ❌ C++ required for best performance
- ❌ Licensing costs for large teams

**Best For**: High-fidelity AAA MMORPGs with large budgets

### Flax Engine - Lightweight Alternative

**Double Precision Mode**:
- Configurable 64-bit coordinate support
- Solar-system scale capability
- Lighter weight than UE5

```csharp
// Flax Example: C# API with double precision
public class MyActor : Actor
{
    public void SetPosition(Vector3Double worldPos)
    {
        Position = worldPos;
    }
}
```

**Pros**:
- ✅ C# scripting (easier than C++)
- ✅ Lighter weight than UE5
- ✅ Modern rendering features
- ✅ Free for projects under $250k revenue

**Cons**:
- ❌ Smaller community
- ❌ Fewer marketplace assets
- ❌ Less mature than UE5

**Best For**: Indie MMORPGs with smaller teams

### Godot 4 - Open Source Option

**Experimental Double Precision**:
- Community-maintained 64-bit builds
- Free and open source
- Python-like scripting (GDScript)

```gdscript
# Godot Example: Using double precision build
extends Node3D

var world_position: Vector3d

func _ready():
    world_position = Vector3d(1000000, 2000000, 0)
    global_position = to_local_coords(world_position)
```

**Pros**:
- ✅ Completely free and open source
- ✅ Easy to learn (GDScript)
- ✅ Lightweight
- ✅ Active community

**Cons**:
- ❌ Double precision is experimental
- ❌ Graphics not as advanced as UE5
- ❌ Limited MMO networking built-in
- ❌ Smaller asset ecosystem

**Best For**: Open-source projects, prototypes, smaller-scale games

### Unity - Requires Custom Implementation

**Manual 64-bit Coordinate System**:
- No native large world support
- Requires custom origin shifting
- DOTS ECS can handle 64-bit positions

```csharp
// Unity Example: Manual origin shifting required
public class WorldPositionComponent : IComponentData
{
    public long X;
    public long Y;
    public long Z;
}

public class OriginShiftSystem : SystemBase
{
    protected override void OnUpdate()
    {
        // Manual origin shifting logic
        Entities.ForEach((ref Translation trans, in WorldPositionComponent worldPos) =>
        {
            trans.Value = ConvertToLocal(worldPos);
        }).Schedule();
    }
}
```

**Pros**:
- ✅ Largest asset store
- ✅ Huge community
- ✅ C# scripting
- ✅ DOTS ECS for performance

**Cons**:
- ❌ No native large world support
- ❌ Requires significant custom implementation
- ❌ Physics engines limited to 32-bit
- ❌ Origin shifting must be manual

**Best For**: Teams with Unity expertise willing to implement custom solutions

### Custom Engine (C++/Rust)

**Maximum Control**:
- Build exactly what you need
- Optimize for your specific use case
- No licensing costs

```rust
// Rust Example: Custom ECS with 64-bit coordinates
#[derive(Component)]
struct WorldPosition {
    x: i64,
    y: i64,
    z: i64,
}

#[derive(Component)]
struct LocalPosition {
    x: f32,
    y: f32,
    z: f32,
}

fn update_local_positions(
    query: Query<(&WorldPosition, &mut LocalPosition)>,
    origin: Res<WorldOrigin>,
) {
    for (world_pos, mut local_pos) in query.iter_mut() {
        local_pos.x = (world_pos.x - origin.x) as f32;
        local_pos.y = (world_pos.y - origin.y) as f32;
        local_pos.z = (world_pos.z - origin.z) as f32;
    }
}
```

**Pros**:
- ✅ Complete control
- ✅ Optimal performance
- ✅ No licensing costs
- ✅ Perfect for your needs

**Cons**:
- ❌ Massive development effort
- ❌ Years of development time
- ❌ Requires expert team
- ❌ No existing asset ecosystem

**Best For**: Very large studios with specific requirements and resources

---

## 5. Physics Engine Considerations

Most physics engines (PhysX, Bullet, Jolt) use 32-bit floats internally. Solutions:

### Option 1: Manual Origin Shifting

```cpp
class PhysicsOriginShifter
{
public:
    void ShiftPhysicsWorld(const Vector3& shift)
    {
        physicsWorld->SetSimulationPaused(true);
        
        // Shift all rigid bodies
        for (RigidBody* body : physicsWorld->GetBodies())
        {
            Vector3 currentPos = body->GetPosition();
            body->SetPosition(currentPos - shift);
            // Velocities remain unchanged (relative motion)
        }
        
        // Shift collision shapes
        for (Collider* collider : physicsWorld->GetColliders())
        {
            Vector3 currentPos = collider->GetPosition();
            collider->SetPosition(currentPos - shift);
        }
        
        physicsWorld->SetSimulationPaused(false);
    }
};
```

### Option 2: Multiple Physics Worlds

```csharp
public class RegionalPhysicsManager
{
    // Each region has its own physics world
    private Dictionary<RegionId, PhysicsWorld> regionPhysics;
    
    public void Simulate(float deltaTime)
    {
        // Simulate each region independently
        foreach (var region in regionPhysics.Values)
        {
            region.Step(deltaTime);
        }
    }
    
    public void TransferEntity(Entity entity, RegionId fromRegion, RegionId toRegion)
    {
        // Remove from old physics world
        regionPhysics[fromRegion].RemoveBody(entity.RigidBody);
        
        // Add to new physics world (with position adjustment)
        Vector3 localPos = CalculateLocalPosition(entity, toRegion);
        entity.RigidBody.SetPosition(localPos);
        regionPhysics[toRegion].AddBody(entity.RigidBody);
    }
}
```

---

## 6. Engine Comparison Matrix

| Feature | UE5 | Flax | Godot 4 | Unity | Custom |
|---------|-----|------|---------|-------|--------|
| **64-bit Coordinates** | ✅ Native | ✅ Built-in | ⚠️ Experimental | ❌ Manual | ✅ Full Control |
| **Origin Shifting** | ✅ Automatic | ⚠️ Manual | ⚠️ Manual | ❌ Manual | ✅ Full Control |
| **Graphics Quality** | ⭐⭐⭐⭐⭐ | ⭐⭐⭐⭐ | ⭐⭐⭐ | ⭐⭐⭐⭐ | ⭐⭐⭐⭐⭐ |
| **Learning Curve** | High | Medium | Low | Medium | Very High |
| **Performance** | Excellent | Very Good | Good | Very Good | Optimal |
| **Cost** | 5% royalty | Free <$250k | Free | Free | Development Time |
| **Community** | Huge | Small | Large | Huge | N/A |
| **MMO Features** | Limited | Limited | Limited | Limited | Custom |
| **Development Time** | 6-12 months | 8-14 months | 10-16 months | 8-14 months | 24-48 months |

---

## 7. Recommendations

### For High-Fidelity AAA MMORPG
**Choose**: Unreal Engine 5
- Reason: Best graphics, native 64-bit support, automatic origin shifting
- Trade-off: High complexity, C++ required

### For Indie MMORPG
**Choose**: Flax Engine
- Reason: Good balance of features and ease of use, C# scripting
- Trade-off: Smaller community, fewer assets

### For Prototype/Small-Scale
**Choose**: Godot 4
- Reason: Free, easy to learn, rapid prototyping
- Trade-off: Limited graphics quality, experimental 64-bit

### For Maximum Control
**Choose**: Custom Engine (C++/Rust)
- Reason: Perfect optimization, no licensing
- Trade-off: Years of development, expert team required

---

## 8. Implementation Checklist

### Phase 1: Coordinate System (Week 1-2)
- [ ] Define WorldPosition struct (64-bit integers or doubles)
- [ ] Implement coordinate conversion functions (geodetic ↔ world)
- [ ] Create precision test suite
- [ ] Validate precision at various distances

### Phase 2: Origin Shifting (Week 3-4)
- [ ] Implement FloatingOriginManager
- [ ] Add entity position tracking
- [ ] Create origin shift event system
- [ ] Test with moving camera/player

### Phase 3: Engine Integration (Week 5-8)
- [ ] Set up chosen engine with 64-bit support
- [ ] Implement rendering coordinate conversion
- [ ] Integrate physics engine with origin shifting
- [ ] Create test scenes at various world locations

### Phase 4: Validation (Week 9-10)
- [ ] Test precision at 0m, 100km, 1000km, 10000km, 40000km
- [ ] Validate physics stability
- [ ] Check for visual artifacts (jitter, z-fighting)
- [ ] Profile performance impact

---

## Further Reading

### Internal Documentation
- [Step 8: MMORPG GIS Key Takeaways](step-8-mmorpg-gis-key-takeaways.md) - Architecture principles
- [World Parameters](step-1-foundation/world-parameters.md) - Technical specifications
- [3D Mathematics for Game Programming](../literature/game-dev-analysis-3d-mathematics.md) - Numerical stability

### Engine Documentation
- [Unreal Engine 5 Large World Coordinates](https://docs.unrealengine.com/5.0/en-US/large-world-coordinates-in-unreal-engine-5/)
- [Flax Engine Documentation](https://docs.flaxengine.com/)
- [Godot Double Precision](https://github.com/godotengine/godot/pull/54249)

### External Resources
- [Floating Origin Techniques](https://docs.unity3d.com/Manual/CamerasOverview.html)
- [Precision in Game Engines](https://blog.selfshadow.com/2008/01/13/precision-improvements/)

---

**Document Version**: 1.0  
**Last Updated**: 2024-01-15  
**Status**: Complete
