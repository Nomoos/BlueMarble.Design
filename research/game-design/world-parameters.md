# World Parameters: Technical Specifications for 3D Spherical World

**Document Type:** Technical Research  
**Version:** 1.0  
**Author:** BlueMarble Game Design Research Team  
**Date:** 2024  
**Status:** Draft

## Executive Summary

This document defines the technical world parameters for transforming BlueMarble into an interactive simulation game with a fully 3D spherical planet. The specifications maintain full backward compatibility with existing BlueMarble architecture while enabling unprecedented geological gameplay mechanics at planetary scale.

## Table of Contents

1. [World Dimensions](#world-dimensions)
2. [Coordinate System](#coordinate-system)
3. [Data Types and Precision](#data-types-and-precision)
4. [Performance Requirements](#performance-requirements)
5. [Integration with BlueMarble Architecture](#integration-with-bluemarble-architecture)
6. [Geological Reference Levels](#geological-reference-levels)
7. [Spatial Indexing Strategy](#spatial-indexing-strategy)

## World Dimensions

### Core Specifications

Based on BlueMarble's existing constants and geological requirements:

- **X Dimension**: 40,075,020 meters (Earth's circumference)
- **Y Dimension**: 20,037,510 meters (half circumference, 0 to π)
- **Z Dimension**: 20,000,000 meters (±10,000 km from sea level)

**Total World Volume**: 40,075,020 × 20,037,510 × 20,000,000 meters

### Enhanced WorldDetail Constants

```csharp
public static class Enhanced3DWorldDetail
{
    // Existing 2D world parameters from BlueMarble (maintained for compatibility)
    public const long WorldSizeX = 40075020L; // Earth circumference
    public const long WorldSizeY = 20037510L; // Half circumference
    
    // New Z dimension for full 3D octree implementation
    public const long WorldSizeZ = 20000000L; // ±10,000 km from sea level
    public const long SeaLevelZ = WorldSizeZ / 2; // 10,000 km (center reference)
    
    // Octree depth calculations for 0.25m resolution
    public const int MaxOctreeDepth = 26; // log₂(40,075,020 / 0.25) ≈ 26 levels
    
    // Key reference levels for geological processes
    public const long AtmosphereTop = SeaLevelZ + 100000;     // +100 km
    public const long CrustBottom = SeaLevelZ - 100000;       // -100 km  
    public const long MantleBottom = SeaLevelZ - 2900000;     // -2,900 km
    public const long CoreBoundary = SeaLevelZ - 5150000;     // -5,150 km
    public const long CoreCenter = SeaLevelZ - 6371000;       // -6,371 km (Earth's center)
    
    // Gameplay-relevant altitudes
    public const long MaxTerrainHeight = SeaLevelZ + 8849;    // Mount Everest equivalent
    public const long DeepestOcean = SeaLevelZ - 11034;       // Mariana Trench equivalent
    public const long MaxPlayerDepth = SeaLevelZ - 50000;     // 50km underground limit for gameplay
    public const long MaxPlayerHeight = SeaLevelZ + 50000;    // 50km altitude limit for gameplay
}
```

## Coordinate System

### Precision Requirements

**Primary Data Type**: `long` (64-bit signed integer)
- **Range**: -9,223,372,036,854,775,808 to 9,223,372,036,854,775,807
- **Precision**: Meter-level accuracy across the entire world
- **Maximum World Coordinate**: 40,075,020 meters (well within long range)

### Coordinate Space Layout

```
Z-Axis (Vertical):
├── +10,000,000m │ Space/Exosphere Boundary
├──    +100,000m │ Atmosphere Top (Kármán Line)
├──     +50,000m │ Maximum Player Altitude
├──      +8,849m │ Highest Terrestrial Peak
├──           0m │ Sea Level Reference
├──     -11,034m │ Deepest Ocean Trench
├──     -50,000m │ Maximum Player Depth
├──    -100,000m │ Crust Bottom
├──  -2,900,000m │ Mantle Bottom
├──  -5,150,000m │ Outer Core Boundary
├──  -6,371,000m │ Earth's Center
└── -10,000,000m │ Conceptual World Bottom
```

### Economic Data Types

For economic calculations requiring high precision:

```csharp
public static class EconomicPrecision
{
    // Use decimal for all monetary calculations
    public const decimal MinimumCurrencyUnit = 0.01m; // Smallest tradeable amount
    public const decimal MaximumPlayerWealth = 999999999999.99m; // 1 trillion limit
    
    // Use double for statistical calculations
    public const double StatisticalPrecision = 1e-15; // Market analysis precision
}
```

## Data Types and Precision

### Spatial Coordinates

| Purpose | Data Type | Range | Precision | Usage |
|---------|-----------|-------|-----------|-------|
| World Position | `long` | ±9.2×10¹⁸ | 1 meter | Player/object coordinates |
| Sub-meter Details | `float` | ±3.4×10³⁸ | 0.1 meter | Visual effects, particles |
| Economic Values | `decimal` | ±7.9×10²⁸ | 0.01 currency | All monetary calculations |
| Statistical Data | `double` | ±1.7×10³⁰⁸ | 1×10⁻¹⁵ | Market analysis, AI calculations |

### Backward Compatibility Guarantees

```csharp
// All existing WorldDetail constants remain unchanged
public static class WorldDetail
{
    public const long WorldSizeX = 40075020L; // Unchanged
    public const long WorldSizeY = 20037510L; // Unchanged
    // Z dimension added without affecting existing 2D operations
}
```

## Performance Requirements

### Real-time Response Targets

| Player Activity | Maximum Response Time | Complexity Level |
|----------------|----------------------|------------------|
| Movement | 16ms (60 FPS) | Low |
| Environmental Interaction | 33ms (30 FPS) | Medium |
| Terrain Modification | 100ms | High |
| Mining Operations | 250ms | Very High |
| Geological Process Triggers | 1000ms | Extreme |

### Memory and Storage Optimization

**Octree Compression Strategy**:
- **Homogeneous Regions**: 90% material uniformity triggers automatic node collapse
- **Active Regions**: Full resolution maintained in 10km radius around players
- **Deep Ocean/Space**: Aggressive compression with procedural detail generation
- **Historical Data**: Compressed deltas for geological process history

## Integration with BlueMarble Architecture

### Frontend Extensions

```javascript
// Extension to existing quadtree system
export class Enhanced3DQuadTree extends AdaptiveQuadTree {
    constructor(bounds, maxDepth = 26) {
        super(bounds, maxDepth);
        this.zBounds = { min: 0, max: Enhanced3DWorldDetail.WorldSizeZ };
    }
    
    query3D(x, y, z, radius) {
        // 3D spatial queries for game mechanics
        const bounds2D = this.calculateProjectedBounds(x, y, z, radius);
        const candidates = this.query(bounds2D);
        return this.filter3DDistance(candidates, x, y, z, radius);
    }
}
```

### Backend Integration

```csharp
// Extension to existing GeometryOps class
public static class Enhanced3DGeometryOps
{
    public static MaterialOctree CreateGameWorldOctree()
    {
        return new MaterialOctree
        {
            WorldBounds = new Envelope3D(
                0, Enhanced3DWorldDetail.WorldSizeX,
                0, Enhanced3DWorldDetail.WorldSizeY, 
                0, Enhanced3DWorldDetail.WorldSizeZ
            ),
            MaxDepth = Enhanced3DWorldDetail.MaxOctreeDepth,
            SeaLevelReference = Enhanced3DWorldDetail.SeaLevelZ,
            DefaultMaterial = MaterialId.Ocean,
            CompressionThreshold = 0.9 // 90% homogeneity for compression
        };
    }
    
    public static bool IsWithinGameplayBounds(long x, long y, long z)
    {
        return z >= Enhanced3DWorldDetail.MaxPlayerDepth && 
               z <= Enhanced3DWorldDetail.MaxPlayerHeight &&
               x >= 0 && x < Enhanced3DWorldDetail.WorldSizeX &&
               y >= 0 && y < Enhanced3DWorldDetail.WorldSizeY;
    }
}
```

## Geological Reference Levels

### Atmospheric Layers (Above Sea Level)

| Layer | Altitude Range | Game Mechanics | Material Properties |
|-------|---------------|----------------|-------------------|
| Troposphere | 0 - 12km | Weather, flying vehicles | Air density affects movement |
| Stratosphere | 12 - 50km | High-altitude exploration | Reduced oxygen, temperature effects |
| Space Transition | 50 - 100km | Space elevator endpoints | Vacuum transition zone |
| Exosphere | 100km+ | Orbital mechanics | Space environment |

### Terrestrial Layers (Below Sea Level)

| Layer | Depth Range | Game Mechanics | Material Properties |
|-------|------------|----------------|-------------------|
| Continental Crust | 0 - 50km | Deep mining, geothermal | Granite, sedimentary rocks |
| Oceanic Crust | 0 - 10km | Underwater exploration | Basalt, marine sediments |
| Upper Mantle | 50 - 400km | Volcanic activity sources | Olivine, pyroxene |
| Lower Mantle | 400 - 2900km | Tectonic process drivers | High-pressure minerals |
| Outer Core | 2900 - 5150km | Magnetic field generation | Liquid iron-nickel |
| Inner Core | 5150 - 6371km | Planetary center | Solid iron-nickel |

### Gameplay Accessibility Zones

```csharp
public enum AccessibilityZone
{
    Surface,          // -1km to +10km (normal gameplay)
    DeepMining,       // -1km to -50km (specialized equipment required)
    HighAltitude,     // +10km to +50km (aircraft/space elevator access)
    ExtremeDepth,     // -50km to -100km (advanced civilization projects)
    AtmosphericHigh,  // +50km to +100km (space program territory)
    Inaccessible      // Beyond ±100km (geological simulation only)
}
```

## Spatial Indexing Strategy

### Octree Depth Allocation

```
Depth 0-10:  Continental/Ocean-scale regions (10,000km to 40km resolution)
Depth 11-15: Regional geology (40km to 1.25km resolution)
Depth 16-20: Local terrain (1.25km to 40m resolution)
Depth 21-25: Detailed features (40m to 1.25m resolution)
Depth 26:    Maximum detail (0.25m resolution for critical areas)
```

### Adaptive Resolution Strategy

**Hot Zones**: Areas with high player activity maintain maximum resolution
**Warm Zones**: Moderate detail for areas visible to players
**Cold Zones**: Compressed representation for distant/unused areas
**Frozen Zones**: Minimal representation for deep space/core regions

### Memory Allocation Guidelines

| Zone Type | Memory Budget | Update Frequency | Compression Ratio |
|-----------|---------------|------------------|-------------------|
| Hot | 512MB per zone | Real-time (16ms) | 1:1 (no compression) |
| Warm | 64MB per zone | Low latency (100ms) | 4:1 compression |
| Cold | 8MB per zone | Background (1s) | 16:1 compression |
| Frozen | 1MB per zone | Rare (60s) | 64:1 compression |

## Performance Validation

### Benchmark Requirements

All specifications must meet the following performance criteria on target hardware:

- **Minimum Spec**: 4GB RAM, integrated graphics, mobile processors
- **Query Response**: 95th percentile under target response times
- **Memory Usage**: Peak usage under 2GB for client, 8GB for server
- **Network Bandwidth**: Under 10KB/s steady state, 100KB/s peak per player

### Stress Testing Scenarios

1. **1000 simultaneous players** in adjacent 1km² areas
2. **Large-scale mining operation** affecting 10km³ volume
3. **Continental terraforming project** with 100km² surface changes
4. **Volcanic eruption simulation** with real-time geological cascades

## Conclusion

These world parameters provide a comprehensive foundation for BlueMarble's transformation into an interactive geological simulation game. The specifications maintain scientific accuracy while enabling unprecedented scale gameplay mechanics, from individual mining operations to continental terraforming projects.

The use of 64-bit integers for spatial coordinates ensures meter-level precision across the entire planetary surface, while the adaptive octree strategy provides optimal performance for both intimate player interactions and massive geological processes.

All specifications maintain full backward compatibility with existing BlueMarble architecture, ensuring a smooth transition from scientific simulation to interactive gaming platform.