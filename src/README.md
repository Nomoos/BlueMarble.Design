# BlueMarble.World - World Parameters Implementation

This directory contains the C# implementation of the world parameters specified in the BlueMarble game design research.

## Overview

BlueMarble.World provides the core constants and utilities for a 3D spherical world simulation with:
- **World Dimensions**: 40,075,020 × 20,037,510 × 20,000,000 meters
- **Coordinate System**: 64-bit signed integers (long) for meter-level precision
- **Full backward compatibility** with existing 2D BlueMarble systems

## Architecture

### Constants

#### `WorldDetail.cs`
Base world dimensions maintaining backward compatibility:
- `WorldSizeX`: 40,075,020 meters (Earth's circumference)
- `WorldSizeY`: 20,037,510 meters (half circumference)

#### `Enhanced3DWorldDetail.cs`
Extended 3D world parameters:
- All WorldDetail constants (for compatibility)
- `WorldSizeZ`: 20,000,000 meters (±10,000 km from sea level)
- `SeaLevelZ`: 10,000,000 meters (center reference point)
- Geological reference levels (atmosphere, crust, mantle, core)
- Gameplay boundaries (player depth/height limits)
- Octree configuration constants

#### `EconomicPrecision.cs`
Economic calculation constants:
- `MinimumCurrencyUnit`: 0.01 (decimal precision)
- `MaximumPlayerWealth`: 999,999,999,999.99
- `StatisticalPrecision`: 1×10⁻¹⁵ (for market analysis)

#### `AccessibilityZone.cs`
Enum defining gameplay accessibility zones:
- Surface (-1km to +10km)
- DeepMining (-1km to -50km)
- HighAltitude (+10km to +50km)
- ExtremeDepth (-50km to -100km)
- AtmosphericHigh (+50km to +100km)
- Inaccessible (beyond ±100km)

### Utilities

#### `Enhanced3DGeometryOps.cs`
Utility methods for working with 3D world coordinates:
- Bounds checking (gameplay and world bounds)
- Accessibility zone determination
- Altitude/coordinate conversions
- Sea level checks
- Coordinate clamping

## Usage Examples

### Basic Coordinate Validation

```csharp
using BlueMarble.World;
using BlueMarble.World.Constants;

// Check if coordinates are within gameplay bounds
long x = 20000000L;
long y = 10000000L;
long z = Enhanced3DWorldDetail.SeaLevelZ + 1000; // 1km above sea level

bool inBounds = Enhanced3DGeometryOps.IsWithinGameplayBounds(x, y, z);
// Returns: true
```

### Altitude Conversions

```csharp
// Convert altitude to Z coordinate
long altitude = 8849; // Mount Everest height
long z = Enhanced3DGeometryOps.GetZCoordinateFromAltitude(altitude);
// Returns: 10008849

// Convert Z coordinate back to altitude
long calculatedAltitude = Enhanced3DGeometryOps.GetAltitudeFromSeaLevel(z);
// Returns: 8849
```

### Accessibility Zone Classification

```csharp
// Determine what zone a coordinate is in
long deepUnderground = Enhanced3DWorldDetail.SeaLevelZ - 5000; // 5km underground
var zone = Enhanced3DGeometryOps.DetermineAccessibilityZone(deepUnderground);
// Returns: AccessibilityZone.DeepMining

// Check specific zones
bool needsSpecialEquipment = zone == AccessibilityZone.DeepMining || 
                              zone == AccessibilityZone.ExtremeDepth;
```

### Economic Calculations

```csharp
// Use decimal for all monetary calculations
decimal price = 123.45m;
decimal tax = price * 0.15m;
decimal total = price + tax;

// Verify within valid range
bool isValid = total <= EconomicPrecision.MaximumPlayerWealth;
```

## Key Design Principles

### 1. Backward Compatibility
All existing WorldDetail constants are preserved unchanged. Enhanced3DWorldDetail extends these with new 3D capabilities.

### 2. Meter-Level Precision
Using `long` (64-bit signed integer) provides exact meter-level precision across the entire world (40+ million meters) without floating-point errors.

### 3. Scientific Accuracy
All geological reference levels match real Earth dimensions:
- Atmosphere top: +100 km (Kármán line)
- Crust bottom: -100 km
- Mantle bottom: -2,900 km
- Core center: -6,371 km (Earth's radius)

### 4. Gameplay Balance
Player accessibility limits prevent gameplay imbalance while allowing for future expansion:
- Maximum player height: +50 km (high-altitude exploration)
- Maximum player depth: -50 km (deep mining operations)

## Building and Testing

### Prerequisites
- .NET 8.0 SDK or later

### Build
```bash
dotnet build BlueMarble.World.sln
```

### Run Tests
```bash
dotnet test BlueMarble.World.sln
```

### Test Coverage
72 comprehensive unit tests covering:
- All constant values and relationships
- Bounds checking logic
- Accessibility zone classification
- Altitude conversion accuracy
- Sea level checks
- Coordinate clamping
- Backward compatibility

## Integration with Research Documentation

This implementation directly implements the specifications from:
- `/research/game-design/step-1-foundation/world-parameters.md`
- `/research/spatial-data-storage/step-3-architecture-design/octree-optimization-guide.md`
- `/research/game-design/step-4-implementation-planning/implementation-plan.md`

All constants and formulas match the research documentation exactly.

## Future Enhancements

The current implementation provides the foundation for:
1. **Octree spatial indexing** - MaxOctreeDepth constant supports 0.25m resolution
2. **Material storage systems** - Bounds checking enables efficient spatial queries
3. **Geological process simulation** - Reference levels support realistic process modeling
4. **Economic systems** - High-precision decimal types for complex economies
5. **Multi-player gameplay** - Accessibility zones enable balanced progression systems

## Performance Characteristics

- **Memory footprint**: Minimal - only constants (no runtime state)
- **Computation**: All operations are O(1) arithmetic
- **No allocations**: All methods operate on value types
- **Thread-safe**: Constants are immutable, utilities are pure functions

## Contributing

When adding new constants or utilities:
1. Maintain backward compatibility with WorldDetail
2. Add comprehensive unit tests
3. Document in XML comments
4. Follow existing naming conventions
5. Ensure consistency with research documentation

## License

See repository LICENSE file.
