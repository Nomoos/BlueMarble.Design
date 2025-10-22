# Int32 Coordinate System - Implementation Specification

**Status**: Draft - Implementation Ready
**Target Repository**: BlueMarble.Core
**Priority**: High
**Estimated Duration**: 2 weeks (10 working days)
**Dependencies**: Database schema, Octree system, Rendering pipeline

---

## Executive Summary

This specification provides detailed implementation guidance for the Int32 (centimeters) coordinate system in BlueMarble.Core, as decided in [ADR-001](adr-001-coordinate-data-type-selection.md). The implementation stores world coordinates as 32-bit signed integers representing centimeters, providing exact 1cm precision (40× better than the 0.25m requirement) for geological and scientific simulations.

**Key Benefits**:
- Exact precision for geological simulations (no floating-point drift)
- 25-35% faster arithmetic than float/double
- 50% memory savings vs double (12 bytes vs 24 bytes per coordinate)
- Deterministic, reproducible results across all platforms

---

## Table of Contents

1. [Core Data Structures](#core-data-structures)
2. [API Design](#api-design)
3. [Database Integration](#database-integration)
4. [Octree Integration](#octree-integration)
5. [Rendering Integration](#rendering-integration)
6. [Network Protocol](#network-protocol)
7. [Testing Strategy](#testing-strategy)
8. [Performance Benchmarks](#performance-benchmarks)
9. [Migration Strategy](#migration-strategy)

---

## Core Data Structures

### WorldCoordinate Struct

```csharp
namespace BlueMarble.Core.Spatial
{
    /// <summary>
    /// Represents a world-space coordinate stored as Int32 centimeters.
    /// Provides exact 1cm precision for geological and scientific simulations.
    /// Range: ±21,474 km (sufficient for 20,000 km world with 7% margin)
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct WorldCoordinate : IEquatable<WorldCoordinate>
    {
        /// <summary>X coordinate in centimeters</summary>
        public int X;
        
        /// <summary>Y coordinate in centimeters (vertical/height)</summary>
        public int Y;
        
        /// <summary>Z coordinate in centimeters</summary>
        public int Z;
        
        // Constants
        public const int CM_PER_METER = 100;
        public const int MIN_VALUE_CM = int.MinValue; // -2,147,483,648 cm = -21,474 km
        public const int MAX_VALUE_CM = int.MaxValue; //  2,147,483,647 cm =  21,474 km
        
        // Constructors
        public WorldCoordinate(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
        }
        
        /// <summary>Create from meters (converts to centimeters)</summary>
        public static WorldCoordinate FromMeters(float x, float y, float z)
        {
            return new WorldCoordinate(
                (int)(x * CM_PER_METER),
                (int)(y * CM_PER_METER),
                (int)(z * CM_PER_METER)
            );
        }
        
        /// <summary>Convert to meters for calculations</summary>
        public Vector3 ToMeters()
        {
            return new Vector3(
                X / (float)CM_PER_METER,
                Y / (float)CM_PER_METER,
                Z / (float)CM_PER_METER
            );
        }
        
        /// <summary>Convert to camera-relative float for rendering</summary>
        public Vector3 ToCameraRelative(WorldCoordinate cameraPosition)
        {
            return new Vector3(
                (X - cameraPosition.X) / (float)CM_PER_METER,
                (Y - cameraPosition.Y) / (float)CM_PER_METER,
                (Z - cameraPosition.Z) / (float)CM_PER_METER
            );
        }
        
        // Arithmetic operations (for simulations)
        public static WorldCoordinate operator +(WorldCoordinate a, WorldCoordinate b)
        {
            return new WorldCoordinate(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        }
        
        public static WorldCoordinate operator -(WorldCoordinate a, WorldCoordinate b)
        {
            return new WorldCoordinate(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
        }
        
        /// <summary>Integer distance squared (avoid sqrt for performance)</summary>
        public long DistanceSquared(WorldCoordinate other)
        {
            long dx = (long)X - other.X;
            long dy = (long)Y - other.Y;
            long dz = (long)Z - other.Z;
            return dx * dx + dy * dy + dz * dz;
        }
        
        /// <summary>Distance in centimeters (returns double for precision)</summary>
        public double Distance(WorldCoordinate other)
        {
            return Math.Sqrt(DistanceSquared(other));
        }
        
        // Equality
        public bool Equals(WorldCoordinate other)
        {
            return X == other.X && Y == other.Y && Z == other.Z;
        }
        
        public override bool Equals(object obj)
        {
            return obj is WorldCoordinate coord && Equals(coord);
        }
        
        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y, Z);
        }
        
        public static bool operator ==(WorldCoordinate a, WorldCoordinate b) => a.Equals(b);
        public static bool operator !=(WorldCoordinate a, WorldCoordinate b) => !a.Equals(b);
        
        public override string ToString()
        {
            return $"WorldCoord({X}cm, {Y}cm, {Z}cm) = ({X/100.0}m, {Y/100.0}m, {Z/100.0}m)";
        }
    }
}
```

### WorldBounds Struct

```csharp
namespace BlueMarble.Core.Spatial
{
    /// <summary>
    /// Axis-aligned bounding box using Int32 coordinates
    /// </summary>
    [Serializable]
    public struct WorldBounds
    {
        public WorldCoordinate Min;
        public WorldCoordinate Max;
        
        public WorldBounds(WorldCoordinate min, WorldCoordinate max)
        {
            Min = min;
            Max = max;
        }
        
        public WorldCoordinate Center => new WorldCoordinate(
            (Min.X + Max.X) / 2,
            (Min.Y + Max.Y) / 2,
            (Min.Z + Max.Z) / 2
        );
        
        public WorldCoordinate Size => new WorldCoordinate(
            Max.X - Min.X,
            Max.Y - Min.Y,
            Max.Z - Min.Z
        );
        
        public bool Contains(WorldCoordinate point)
        {
            return point.X >= Min.X && point.X <= Max.X &&
                   point.Y >= Min.Y && point.Y <= Max.Y &&
                   point.Z >= Min.Z && point.Z <= Max.Z;
        }
        
        public bool Intersects(WorldBounds other)
        {
            return Min.X <= other.Max.X && Max.X >= other.Min.X &&
                   Min.Y <= other.Max.Y && Max.Y >= other.Min.Y &&
                   Min.Z <= other.Max.Z && Max.Z >= other.Min.Z;
        }
    }
}
```

---

## API Design

### Conversion Utilities

```csharp
namespace BlueMarble.Core.Spatial
{
    /// <summary>
    /// Utility class for coordinate conversions
    /// </summary>
    public static class CoordinateConverter
    {
        // Meters to centimeters
        public static int MetersToCentimeters(float meters)
        {
            return (int)(meters * WorldCoordinate.CM_PER_METER);
        }
        
        // Centimeters to meters
        public static float CentimetersToMeters(int centimeters)
        {
            return centimeters / (float)WorldCoordinate.CM_PER_METER;
        }
        
        // Validate coordinate is within world bounds
        public static bool IsValidWorldCoordinate(WorldCoordinate coord)
        {
            const int MAX_WORLD_SIZE_CM = 2_000_000_000; // 20,000 km
            return Math.Abs(coord.X) <= MAX_WORLD_SIZE_CM &&
                   Math.Abs(coord.Y) <= MAX_WORLD_SIZE_CM &&
                   Math.Abs(coord.Z) <= MAX_WORLD_SIZE_CM;
        }
        
        // Clamp to world bounds
        public static WorldCoordinate ClampToWorldBounds(WorldCoordinate coord)
        {
            const int MAX = 2_000_000_000;
            return new WorldCoordinate(
                Math.Clamp(coord.X, -MAX, MAX),
                Math.Clamp(coord.Y, -MAX, MAX),
                Math.Clamp(coord.Z, -MAX, MAX)
            );
        }
    }
}
```

---

## Database Integration

### PostgreSQL Schema

```sql
-- World coordinates stored as INTEGER (centimeters)
CREATE TABLE spatial_entities (
    entity_id BIGINT PRIMARY KEY,
    position_x INTEGER NOT NULL,  -- X coordinate in cm
    position_y INTEGER NOT NULL,  -- Y coordinate (height) in cm
    position_z INTEGER NOT NULL,  -- Z coordinate in cm
    chunk_id BIGINT NOT NULL,     -- For spatial indexing
    
    -- Indexes for spatial queries
    CONSTRAINT valid_world_bounds CHECK (
        position_x BETWEEN -2000000000 AND 2000000000 AND
        position_y BETWEEN -2000000000 AND 2000000000 AND
        position_z BETWEEN -2000000000 AND 2000000000
    )
);

-- Spatial index using chunk-based approach
CREATE INDEX idx_spatial_entities_chunk ON spatial_entities(chunk_id);

-- Individual coordinate indexes for range queries
CREATE INDEX idx_spatial_entities_x ON spatial_entities(position_x);
CREATE INDEX idx_spatial_entities_y ON spatial_entities(position_y);
CREATE INDEX idx_spatial_entities_z ON spatial_entities(position_z);

-- Composite index for bounding box queries
CREATE INDEX idx_spatial_entities_xyz ON spatial_entities(position_x, position_y, position_z);
```

### Query Examples

```sql
-- Find entities within bounding box
SELECT entity_id, position_x, position_y, position_z
FROM spatial_entities
WHERE position_x BETWEEN :min_x AND :max_x
  AND position_y BETWEEN :min_y AND :max_y
  AND position_z BETWEEN :min_z AND :max_z;

-- Find entities in chunk (fast lookup)
SELECT entity_id, position_x, position_y, position_z
FROM spatial_entities
WHERE chunk_id = :chunk_id;

-- Range query with distance filter (uses integer arithmetic)
SELECT entity_id,
       (position_x - :center_x) AS dx,
       (position_y - :center_y) AS dy,
       (position_z - :center_z) AS dz
FROM spatial_entities
WHERE position_x BETWEEN :min_x AND :max_x
  AND position_y BETWEEN :min_y AND :max_y
  AND position_z BETWEEN :min_z AND :max_z
  AND (
      (CAST(position_x - :center_x AS BIGINT) * CAST(position_x - :center_x AS BIGINT) +
       CAST(position_y - :center_y AS BIGINT) * CAST(position_y - :center_y AS BIGINT) +
       CAST(position_z - :center_z AS BIGINT) * CAST(position_z - :center_z AS BIGINT))
      <= :radius_squared_cm
  );
```

---

## Octree Integration

### OctreeNode with Int32 Coordinates

```csharp
namespace BlueMarble.Core.Spatial
{
    /// <summary>
    /// Octree node using Int32 coordinates for efficient spatial indexing
    /// </summary>
    public class OctreeNode
    {
        public WorldBounds Bounds { get; private set; }
        public List<EntityReference> Entities { get; private set; }
        public OctreeNode[] Children { get; private set; }
        public int Depth { get; private set; }
        
        private const int MAX_ENTITIES_PER_NODE = 8;
        private const int MAX_DEPTH = 16;
        
        public OctreeNode(WorldBounds bounds, int depth = 0)
        {
            Bounds = bounds;
            Depth = depth;
            Entities = new List<EntityReference>(MAX_ENTITIES_PER_NODE);
            Children = null;
        }
        
        public void Insert(EntityReference entity, WorldCoordinate position)
        {
            if (!Bounds.Contains(position))
                return;
            
            if (Children == null)
            {
                Entities.Add(entity);
                
                if (Entities.Count > MAX_ENTITIES_PER_NODE && Depth < MAX_DEPTH)
                {
                    Subdivide();
                }
            }
            else
            {
                foreach (var child in Children)
                {
                    if (child.Bounds.Contains(position))
                    {
                        child.Insert(entity, position);
                        break;
                    }
                }
            }
        }
        
        private void Subdivide()
        {
            var center = Bounds.Center;
            var min = Bounds.Min;
            var max = Bounds.Max;
            
            Children = new OctreeNode[8];
            
            // Create 8 child nodes (octants)
            Children[0] = new OctreeNode(new WorldBounds(
                new WorldCoordinate(min.X, min.Y, min.Z),
                new WorldCoordinate(center.X, center.Y, center.Z)
            ), Depth + 1);
            
            Children[1] = new OctreeNode(new WorldBounds(
                new WorldCoordinate(center.X, min.Y, min.Z),
                new WorldCoordinate(max.X, center.Y, center.Z)
            ), Depth + 1);
            
            // ... (remaining 6 octants)
            
            // Redistribute entities to children
            foreach (var entity in Entities)
            {
                // Re-insert into children
            }
            
            Entities.Clear();
        }
        
        public List<EntityReference> Query(WorldBounds queryBounds)
        {
            var results = new List<EntityReference>();
            
            if (!Bounds.Intersects(queryBounds))
                return results;
            
            if (Children == null)
            {
                results.AddRange(Entities);
            }
            else
            {
                foreach (var child in Children)
                {
                    results.AddRange(child.Query(queryBounds));
                }
            }
            
            return results;
        }
    }
}
```

### Morton Encoding (Z-Order Curve)

```csharp
namespace BlueMarble.Core.Spatial
{
    /// <summary>
    /// Morton encoding for Int32 coordinates
    /// Provides efficient spatial indexing using Z-order curve
    /// </summary>
    public static class MortonEncoding
    {
        /// <summary>
        /// Encode 3D Int32 coordinates to Morton code (96-bit)
        /// </summary>
        public static (uint high, uint mid, uint low) Encode(WorldCoordinate coord)
        {
            // Normalize to unsigned (shift by adding INT_MAX + 1)
            uint x = (uint)(coord.X - int.MinValue);
            uint y = (uint)(coord.Y - int.MinValue);
            uint z = (uint)(coord.Z - int.MinValue);
            
            ulong code = 0;
            
            // Interleave bits (first 21 bits of each coordinate)
            for (int i = 0; i < 21; i++)
            {
                code |= ((x & (1UL << i)) << (2 * i));
                code |= ((y & (1UL << i)) << (2 * i + 1));
                code |= ((z & (1UL << i)) << (2 * i + 2));
            }
            
            // Split into 3 parts for 96-bit result
            return (
                (uint)(code >> 64),
                (uint)(code >> 32),
                (uint)code
            );
        }
        
        /// <summary>
        /// Decode Morton code back to WorldCoordinate
        /// </summary>
        public static WorldCoordinate Decode(uint high, uint mid, uint low)
        {
            ulong code = ((ulong)high << 64) | ((ulong)mid << 32) | low;
            
            uint x = 0, y = 0, z = 0;
            
            for (int i = 0; i < 21; i++)
            {
                x |= (uint)((code >> (3 * i)) & 1) << i;
                y |= (uint)((code >> (3 * i + 1)) & 1) << i;
                z |= (uint)((code >> (3 * i + 2)) & 1) << i;
            }
            
            // Denormalize back to signed
            return new WorldCoordinate(
                (int)(x + int.MinValue),
                (int)(y + int.MinValue),
                (int)(z + int.MinValue)
            );
        }
    }
}
```

---

## Rendering Integration

### Camera-Relative Rendering

```csharp
namespace BlueMarble.Rendering
{
    /// <summary>
    /// Converts Int32 world coordinates to camera-relative float for GPU rendering
    /// </summary>
    public class CoordinateRenderingSystem
    {
        private WorldCoordinate _cameraPosition;
        
        public void UpdateCameraPosition(WorldCoordinate newPosition)
        {
            _cameraPosition = newPosition;
        }
        
        /// <summary>
        /// Convert entity positions to camera-relative float for rendering
        /// </summary>
        public Vector3[] ConvertForRendering(WorldCoordinate[] worldPositions)
        {
            var renderPositions = new Vector3[worldPositions.Length];
            
            for (int i = 0; i < worldPositions.Length; i++)
            {
                renderPositions[i] = worldPositions[i].ToCameraRelative(_cameraPosition);
            }
            
            return renderPositions;
        }
        
        /// <summary>
        /// Batch conversion for vertex buffers
        /// </summary>
        public unsafe void ConvertToVertexBuffer(
            WorldCoordinate[] worldPositions,
            float* vertexBuffer,
            int stride)
        {
            for (int i = 0; i < worldPositions.Length; i++)
            {
                var relative = worldPositions[i].ToCameraRelative(_cameraPosition);
                int offset = i * stride;
                vertexBuffer[offset + 0] = relative.X;
                vertexBuffer[offset + 1] = relative.Y;
                vertexBuffer[offset + 2] = relative.Z;
            }
        }
    }
}
```

---

## Network Protocol

### Binary Serialization

```csharp
namespace BlueMarble.Network
{
    /// <summary>
    /// Efficient binary serialization for WorldCoordinate
    /// </summary>
    public static class CoordinateSerializer
    {
        // 12 bytes total (3 × Int32)
        public static void Serialize(WorldCoordinate coord, BinaryWriter writer)
        {
            writer.Write(coord.X);  // 4 bytes
            writer.Write(coord.Y);  // 4 bytes
            writer.Write(coord.Z);  // 4 bytes
        }
        
        public static WorldCoordinate Deserialize(BinaryReader reader)
        {
            return new WorldCoordinate(
                reader.ReadInt32(),
                reader.ReadInt32(),
                reader.ReadInt32()
            );
        }
        
        // Alternative: Delta encoding for reduced bandwidth
        public static void SerializeDelta(
            WorldCoordinate baseCoord,
            WorldCoordinate newCoord,
            BinaryWriter writer)
        {
            // If delta fits in Int16, use 6 bytes instead of 12
            int dx = newCoord.X - baseCoord.X;
            int dy = newCoord.Y - baseCoord.Y;
            int dz = newCoord.Z - baseCoord.Z;
            
            if (dx >= short.MinValue && dx <= short.MaxValue &&
                dy >= short.MinValue && dy <= short.MaxValue &&
                dz >= short.MinValue && dz <= short.MaxValue)
            {
                writer.Write((byte)1); // Delta encoding flag
                writer.Write((short)dx);
                writer.Write((short)dy);
                writer.Write((short)dz);
            }
            else
            {
                writer.Write((byte)0); // Full encoding flag
                Serialize(newCoord, writer);
            }
        }
    }
}
```

---

## Testing Strategy

### Unit Tests

```csharp
namespace BlueMarble.Core.Tests.Spatial
{
    [TestFixture]
    public class WorldCoordinateTests
    {
        [Test]
        public void Constructor_ValidValues_CreatesCoordinate()
        {
            var coord = new WorldCoordinate(100, 200, 300);
            Assert.AreEqual(100, coord.X);
            Assert.AreEqual(200, coord.Y);
            Assert.AreEqual(300, coord.Z);
        }
        
        [Test]
        public void FromMeters_ConvertsCorrectly()
        {
            var coord = WorldCoordinate.FromMeters(1.5f, 2.5f, 3.5f);
            Assert.AreEqual(150, coord.X);
            Assert.AreEqual(250, coord.Y);
            Assert.AreEqual(350, coord.Z);
        }
        
        [Test]
        public void ToMeters_ConvertsCorrectly()
        {
            var coord = new WorldCoordinate(150, 250, 350);
            var meters = coord.ToMeters();
            Assert.AreEqual(1.5f, meters.X, 0.001f);
            Assert.AreEqual(2.5f, meters.Y, 0.001f);
            Assert.AreEqual(3.5f, meters.Z, 0.001f);
        }
        
        [Test]
        public void Distance_CalculatesCorrectly()
        {
            var a = new WorldCoordinate(0, 0, 0);
            var b = new WorldCoordinate(300, 400, 0); // 3m, 4m, 0m = 5m distance
            var distance = a.Distance(b);
            Assert.AreEqual(500.0, distance, 0.001); // 500 cm = 5m
        }
        
        [Test]
        public void Addition_WorksCorrectly()
        {
            var a = new WorldCoordinate(100, 200, 300);
            var b = new WorldCoordinate(50, 75, 25);
            var result = a + b;
            Assert.AreEqual(150, result.X);
            Assert.AreEqual(275, result.Y);
            Assert.AreEqual(325, result.Z);
        }
        
        [Test]
        public void Equality_WorksCorrectly()
        {
            var a = new WorldCoordinate(100, 200, 300);
            var b = new WorldCoordinate(100, 200, 300);
            var c = new WorldCoordinate(101, 200, 300);
            
            Assert.IsTrue(a == b);
            Assert.IsFalse(a == c);
            Assert.IsTrue(a.Equals(b));
        }
        
        [Test]
        public void MaxRange_IsWithinWorldBounds()
        {
            const int MAX_WORLD_CM = 2_000_000_000; // 20,000 km
            var coord = new WorldCoordinate(MAX_WORLD_CM, MAX_WORLD_CM, MAX_WORLD_CM);
            Assert.IsTrue(CoordinateConverter.IsValidWorldCoordinate(coord));
        }
        
        [TestCase(0, 0, 0)]
        [TestCase(1000, 2000, 3000)]
        [TestCase(-1000, -2000, -3000)]
        [TestCase(int.MaxValue / 2, 0, 0)]
        public void MortonEncoding_RoundTrip(int x, int y, int z)
        {
            var original = new WorldCoordinate(x, y, z);
            var (high, mid, low) = MortonEncoding.Encode(original);
            var decoded = MortonEncoding.Decode(high, mid, low);
            Assert.AreEqual(original, decoded);
        }
    }
}
```

### Performance Tests

```csharp
namespace BlueMarble.Core.Tests.Performance
{
    [TestFixture]
    public class CoordinatePerformanceTests
    {
        private const int ITERATIONS = 10_000_000;
        
        [Test]
        public void Benchmark_IntegerArithmetic()
        {
            var coords = new WorldCoordinate[1000];
            var sw = Stopwatch.StartNew();
            
            for (int i = 0; i < ITERATIONS; i++)
            {
                int idx = i % coords.Length;
                coords[idx] = new WorldCoordinate(i, i * 2, i * 3);
            }
            
            sw.Stop();
            Console.WriteLine($"Int32 creation: {sw.ElapsedMilliseconds}ms for {ITERATIONS} ops");
            Assert.Less(sw.ElapsedMilliseconds, 100); // Should be very fast
        }
        
        [Test]
        public void Benchmark_DistanceCalculation()
        {
            var a = new WorldCoordinate(0, 0, 0);
            var b = new WorldCoordinate(1000, 1000, 1000);
            var sw = Stopwatch.StartNew();
            
            long sum = 0;
            for (int i = 0; i < ITERATIONS; i++)
            {
                sum += a.DistanceSquared(b);
            }
            
            sw.Stop();
            Console.WriteLine($"Distance squared: {sw.ElapsedMilliseconds}ms for {ITERATIONS} ops");
            Assert.Less(sw.ElapsedMilliseconds, 50);
        }
    }
}
```

---

## Performance Benchmarks

### Expected Performance Metrics

| Operation | Int32 (cm) | Float | Double | Target |
|-----------|-----------|-------|--------|--------|
| Creation | 0.5 ns | 0.8 ns | 1.2 ns | ✅ Fastest |
| Addition | 0.3 ns | 0.5 ns | 0.8 ns | ✅ Fastest |
| Distance² | 2.1 ns | 3.2 ns | 4.8 ns | ✅ Fastest |
| Memory/coord | 12 bytes | 12 bytes | 24 bytes | ✅ Tied |

### Benchmark Code

```csharp
[MemoryDiagnoser]
public class CoordinateBenchmarks
{
    private WorldCoordinate coordA = new WorldCoordinate(1000, 2000, 3000);
    private WorldCoordinate coordB = new WorldCoordinate(4000, 5000, 6000);
    
    [Benchmark]
    public WorldCoordinate Addition()
    {
        return coordA + coordB;
    }
    
    [Benchmark]
    public long DistanceSquared()
    {
        return coordA.DistanceSquared(coordB);
    }
    
    [Benchmark]
    public Vector3 ToMeters()
    {
        return coordA.ToMeters();
    }
}
```

---

## Migration Strategy

### Phase 1: Core Type (Days 1-3)

**Deliverables**:
- `WorldCoordinate` struct implementation
- `WorldBounds` struct implementation
- `CoordinateConverter` utility class
- Comprehensive unit tests (>95% coverage)
- Performance benchmarks

**Validation**:
- All unit tests pass
- Performance metrics meet targets
- No memory leaks in long-running tests

### Phase 2: Database Integration (Days 4-5)

**Deliverables**:
- PostgreSQL schema updates
- Migration scripts for existing data
- ORM mapping updates (Entity Framework)
- Query performance tests

**Validation**:
- Spatial queries <10ms
- Index performance acceptable
- Data integrity maintained during migration

### Phase 3: Octree Integration (Days 6-8)

**Deliverables**:
- `OctreeNode` updates for Int32
- Morton encoding implementation
- Spatial query functions
- Performance benchmarks

**Validation**:
- Octree operations maintain performance
- Spatial queries accurate
- Memory usage within targets

### Phase 4: Rendering Integration (Days 9-10)

**Deliverables**:
- Camera-relative conversion system
- Vertex buffer generation
- Rendering precision validation

**Validation**:
- No visual artifacts
- Rendering performance maintained
- Frame rate targets met

---

## Related Documents

- [ADR-001: Coordinate Data Type Selection](adr-001-coordinate-data-type-selection.md) - Architectural decision
- [Coordinate Data Type Optimization](coordinate-data-type-optimization.md) - Full research analysis
- [Database Schema Design](../../docs/systems/database-schema-design.md) - Database architecture
- [Spatial Data Storage](../spatial-data-storage/README.md) - Octree implementation

---

## Success Criteria

### Functional Requirements
- ✅ Exact 1cm precision for all coordinates
- ✅ Range supports ±20,000 km world
- ✅ Deterministic calculations across platforms
- ✅ Seamless conversion to/from meters

### Performance Requirements
- ✅ Arithmetic operations 25-35% faster than float/double
- ✅ Memory usage 50% less than double
- ✅ Database queries <10ms
- ✅ Spatial queries >1M per second

### Quality Requirements
- ✅ Unit test coverage >95%
- ✅ Performance benchmarks documented
- ✅ Migration strategy tested
- ✅ No breaking changes to existing systems

---

**Implementation Status**: Ready for development
**Target Start Date**: To be determined
**Assigned Team**: BlueMarble.Core Engineering
