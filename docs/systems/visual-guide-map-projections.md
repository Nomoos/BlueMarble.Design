# Map Projection Visual Guide

**Document Type:** Visual Reference  
**Version:** 1.0  
**Author:** Technical Documentation Team  
**Date:** 2024-12-29  
**Status:** Complete  
**Related Documents:** 
- [Feature Specification](spec-spherical-planet-generation.md)
- [Quick Reference](quick-reference-spherical-planet.md)

## Overview

This guide provides visual representations and comparisons of different map projections used in the Spherical Planet Generation system. Each projection has specific characteristics, advantages, and ideal use cases.

## Projection Types Comparison

### Visual Characteristics

```
┌─────────────────────────────────────────────────────────────────────┐
│                    EQUIRECTANGULAR PROJECTION                       │
├─────────────────────────────────────────────────────────────────────┤
│  +180°                                                      -180°   │
│    ┌──────────────────────────────────────────────────────┐        │
│    │                    North Pole                        │ +90°   │
│    │ ▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓ │        │
│    │ ░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░ │        │
│    │ ▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒ │  0°    │
│    │ ░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░ │        │
│    │ ▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓ │        │
│    │                    South Pole                        │ -90°   │
│    └──────────────────────────────────────────────────────┘        │
│  Characteristics:                                                   │
│  • Simple rectangular grid                                          │
│  • Meridians and parallels are straight lines                      │
│  • Extreme distortion at poles (areas stretched horizontally)      │
│  • Easy coordinate conversion: x = λR, y = φR                      │
└─────────────────────────────────────────────────────────────────────┘

┌─────────────────────────────────────────────────────────────────────┐
│                      MERCATOR PROJECTION                            │
├─────────────────────────────────────────────────────────────────────┤
│    ┌──────────────────────────────────────────────────────┐        │
│    │ ▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓ │ +85°   │
│    │ ▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓ │        │
│    │ ░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░ │        │
│    │ ▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒ │  0°    │
│    │ ░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░ │        │
│    │ ▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓ │        │
│    │ ▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓ │ -85°   │
│    └──────────────────────────────────────────────────────┘        │
│    (Poles cannot be shown - infinite extent)                       │
│  Characteristics:                                                   │
│  • Conformal (preserves angles)                                    │
│  • Straight lines are rhumb lines (constant bearing)               │
│  • Extreme area distortion at high latitudes                       │
│  • Ideal for navigation                                            │
└─────────────────────────────────────────────────────────────────────┘

┌─────────────────────────────────────────────────────────────────────┐
│                      ROBINSON PROJECTION                            │
├─────────────────────────────────────────────────────────────────────┤
│        ╭────────────────────────────────────────────╮              │
│       ╱ ▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓ ╲             │
│      │  ░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░  │            │
│     │   ░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░   │           │
│     │   ▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒   │  0°       │
│     │   ░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░   │           │
│      │  ░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░░  │            │
│       ╲ ▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓ ╱             │
│        ╰────────────────────────────────────────────╯              │
│  Characteristics:                                                   │
│  • Pseudocylindrical with curved meridians                         │
│  • Balanced compromise between distortions                         │
│  • Aesthetically pleasing for world maps                           │
│  • Not conformal, not equal-area                                   │
└─────────────────────────────────────────────────────────────────────┘

┌─────────────────────────────────────────────────────────────────────┐
│                    MOLLWEIDE PROJECTION                             │
├─────────────────────────────────────────────────────────────────────┤
│              ╭───────────────────────╮                              │
│            ╱  ▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓  ╲                            │
│          ╱   ░░░░░░░░░░░░░░░░░░░░░░░   ╲                           │
│         │    ░░░░░░░░░░░░░░░░░░░░░░░    │                          │
│        │     ▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒     │  0°                     │
│         │    ░░░░░░░░░░░░░░░░░░░░░░░    │                          │
│          ╲   ░░░░░░░░░░░░░░░░░░░░░░░   ╱                           │
│            ╲  ▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓  ╱                            │
│              ╰───────────────────────╯                              │
│  Characteristics:                                                   │
│  • Equal-area (preserves relative sizes)                           │
│  • Elliptical shape                                                │
│  • Curved meridians (except central)                               │
│  • Good for thematic world maps                                    │
└─────────────────────────────────────────────────────────────────────┘
```

## Distortion Patterns

### Area Distortion Visualization

```
EQUIRECTANGULAR - Area Distortion by Latitude
┌──────────────────────────────────────────────────────────────────┐
│ +90° ▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓ (400% area inflation)                  │
│ +75° ▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒ (150% area inflation)                      │
│ +60° ░░░░░░░░░░░░░ (75% area inflation)                          │
│ +45° ▒▒▒▒▒▒▒▒▒▒ (40% area inflation)                            │
│ +30° ░░░░░░░░ (15% area inflation)                               │
│ +15° ▓▓▓▓▓▓ (3% area inflation)                                  │
│   0° ████ (No distortion)                                         │
│ -15° ▓▓▓▓▓▓ (3% area inflation)                                  │
│ -30° ░░░░░░░░ (15% area inflation)                               │
│ -45° ▒▒▒▒▒▒▒▒▒▒ (40% area inflation)                            │
│ -60° ░░░░░░░░░░░░░ (75% area inflation)                          │
│ -75° ▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒ (150% area inflation)                      │
│ -90° ▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓ (400% area inflation)                  │
└──────────────────────────────────────────────────────────────────┘

MERCATOR - Area Distortion by Latitude
┌──────────────────────────────────────────────────────────────────┐
│ +85° ▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓ (1000%+ area inflation!)     │
│ +70° ▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒ (300% area inflation)                   │
│ +60° ░░░░░░░░░░░░░░ (150% area inflation)                        │
│ +45° ▒▒▒▒▒▒▒▒▒▒ (75% area inflation)                            │
│ +30° ░░░░░░░░ (30% area inflation)                               │
│ +15° ▓▓▓▓▓▓ (7% area inflation)                                  │
│   0° ████ (No distortion)                                         │
│ -15° ▓▓▓▓▓▓ (7% area inflation)                                  │
│ -30° ░░░░░░░░ (30% area inflation)                               │
│ -45° ▒▒▒▒▒▒▒▒▒▒ (75% area inflation)                            │
│ -60° ░░░░░░░░░░░░░░ (150% area inflation)                        │
│ -70° ▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒ (300% area inflation)                   │
│ -85° ▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓ (1000%+ area inflation!)     │
└──────────────────────────────────────────────────────────────────┘
```

## Date Line Handling

### Before Splitting
```
┌────────────────────────────────────────────────────────────┐
│                  INVALID POLYGON                            │
│                                                              │
│     Start (170°E)                    End (170°W)            │
│          ▼                               ▼                   │
│          ●───────────────────────────────●                   │
│          │                               │                   │
│          │    Polygon appears to span    │                   │
│          │    entire world width         │                   │
│          │    (340° longitude range!)    │                   │
│          │                               │                   │
│          ●───────────────────────────────●                   │
│                                                              │
│  X-coordinate range: 0 to 40,000,000m (spans entire world) │
│  This creates invalid topology!                             │
└────────────────────────────────────────────────────────────┘
```

### After Splitting
```
┌────────────────────────────────────────────────────────────┐
│                  VALID POLYGONS                             │
│                                                              │
│  Polygon 1 (Western side)      Polygon 2 (Eastern side)    │
│                                                              │
│  ●───────────●                  ●───────────●               │
│  │   180°W   │                  │   180°E   │               │
│  │           │                  │           │               │
│  │  Valid    │                  │  Valid    │               │
│  │ topology  │                  │ topology  │               │
│  │           │                  │           │               │
│  ●───────────●                  ●───────────●               │
│                                                              │
│  Both polygons now have valid topology and can be           │
│  properly rendered and queried                              │
└────────────────────────────────────────────────────────────┘
```

## Coordinate System Transformations

### Geographic (WGS84) to SRID_METER (4087)

```
INPUT: Geographic Coordinates (SRID 4326)
┌─────────────────────────────────────────┐
│ Latitude:  45.0°  (range: -90° to 90°) │
│ Longitude: -90.0° (range: -180° to 180°)│
└─────────────────────────────────────────┘
              │
              │ Transformation
              ▼
┌─────────────────────────────────────────────────────────────┐
│ Step 1: Normalize to 0-based range                          │
│   lon_normalized = -90.0 + 180.0 = 90.0°                   │
│   lat_normalized = 45.0 + 90.0 = 135.0°                    │
└─────────────────────────────────────────────────────────────┘
              │
              ▼
┌─────────────────────────────────────────────────────────────┐
│ Step 2: Convert to proportion of world size                 │
│   x_prop = 90.0 / 360.0 = 0.25                             │
│   y_prop = 135.0 / 180.0 = 0.75                            │
└─────────────────────────────────────────────────────────────┘
              │
              ▼
┌─────────────────────────────────────────────────────────────┐
│ Step 3: Scale to SRID_METER coordinates                     │
│   x = 0.25 × 40,075,020m = 10,018,755m                     │
│   y = 0.75 × 20,037,510m = 15,028,132m                     │
└─────────────────────────────────────────────────────────────┘
              │
              ▼
OUTPUT: SRID_METER Coordinates (SRID 4087)
┌─────────────────────────────────────────┐
│ X: 10,018,755m (range: 0 to 40,075,020m)│
│ Y: 15,028,132m (range: 0 to 20,037,510m)│
└─────────────────────────────────────────┘
```

## Biome Classification Decision Tree

```
                         Start
                           │
                           ▼
                    ┌──────────────┐
                    │ Elevation < 0?│
                    └──────┬────┬──┘
                           │    │
                       Yes │    │ No
                           │    │
                           ▼    ▼
                      ┌──────┐  ┌────────────────┐
                      │OCEAN │  │ Temperature?   │
                      └──────┘  └───┬─────┬──────┘
                                    │     │
                             < -10°C│     │> -10°C
                                    │     │
                                    ▼     ▼
                            ┌─────────┐  ┌──────────────┐
                            │ICE SHEET│  │Precipitation?│
                            └─────────┘  └──┬────┬──────┘
                                            │    │
                                     < 250mm│    │> 250mm
                                            │    │
                                            ▼    ▼
                                      ┌────────┐  ┌──────────────┐
                                      │ DESERT │  │ Temperature? │
                                      └────────┘  └──┬────┬──────┘
                                                     │    │
                                              < 0°C  │    │> 0°C
                                                     │    │
                                                     ▼    ▼
                                              ┌──────┐  ┌─────────────┐
                                              │TUNDRA│  │Precipitation?│
                                              └──────┘  └──┬───┬──────┘
                                                           │   │
                                                   >1000mm │   │<1000mm
                                                           │   │
                                                           ▼   ▼
                                                    ┌────────┐ ┌──────────┐
                                                    │ FOREST │ │GRASSLAND │
                                                    │(varies)│ │/SAVANNA  │
                                                    └────────┘ └──────────┘
```

## Spherical Voronoi Distribution

### Uniform Point Distribution on Sphere

```
                     North Pole
                         ●
                        ╱│╲
                       ╱ │ ╲
                      ╱  │  ╲
                     ●───●───●     Golden Spiral Distribution
                    ╱│╲ │ ╱│╲      ensures uniform spacing
                   ╱ │ ╲│╱ │ ╲     across entire sphere
                  ●──●──●──●──●
                 ╱│╲ │╱ │╲│╱│╲    Algorithm:
                ╱ │ ●───●───● ╲   θ = 2π × i / φ
               ●──●─────●─────●   φ = arccos(1 - 2(i+0.5)/n)
              │  │      │      │
              ●──●──────●──────●  Where:
               ╲│╱      │     ╱   • i = point index
                ●───────●────●    • n = total points
                 ╲      │   ╱     • φ = golden ratio
                  ╲     │  ╱
                   ●────●──●
                    ╲   │ ╱
                     ╲  │╱
                      ╲ ●
                       ╲│
                        ●
                    South Pole
```

## Performance Optimization Strategies

### Batch Processing Flow

```
┌──────────────────────────────────────────────────────────────┐
│                    PLANET GENERATION                          │
├──────────────────────────────────────────────────────────────┤
│                                                               │
│  Total Plates: 10,000                                        │
│  Batch Size: 1,000                                           │
│                                                               │
│  ┌────────────┐  ┌────────────┐  ┌────────────┐            │
│  │  Batch 1   │  │  Batch 2   │  │  Batch 3   │  ...       │
│  │ Plates     │  │ Plates     │  │ Plates     │            │
│  │ 0-999      │  │ 1000-1999  │  │ 2000-2999  │            │
│  └──────┬─────┘  └──────┬─────┘  └──────┬─────┘            │
│         │                │                │                   │
│         ▼                ▼                ▼                   │
│  ┌────────────┐  ┌────────────┐  ┌────────────┐            │
│  │ Process    │  │ Process    │  │ Process    │            │
│  │ & Save     │  │ & Save     │  │ & Save     │            │
│  └──────┬─────┘  └──────┬─────┘  └──────┬─────┘            │
│         │                │                │                   │
│         ▼                ▼                ▼                   │
│  ┌────────────┐  ┌────────────┐  ┌────────────┐            │
│  │ GC         │  │ GC         │  │ GC         │            │
│  │ Collect    │  │ Collect    │  │ Collect    │            │
│  └────────────┘  └────────────┘  └────────────┘            │
│                                                               │
│  Benefits:                                                    │
│  • Controlled memory usage                                   │
│  • Progressive results                                       │
│  • Resumable on failure                                      │
└──────────────────────────────────────────────────────────────┘
```

### Spatial Index Query Optimization

```
WITHOUT SPATIAL INDEX:
┌──────────────────────────────────────────────────────────────┐
│  Query: Find neighbors of polygon P                          │
│                                                               │
│  for each polygon in all_polygons:  ← O(n) complexity       │
│      if polygon.intersects(P):                               │
│          add to neighbors                                    │
│                                                               │
│  Time: ~1000ms for 10,000 polygons                          │
└──────────────────────────────────────────────────────────────┘

WITH SPATIAL INDEX (STRtree):
┌──────────────────────────────────────────────────────────────┐
│  Query: Find neighbors of polygon P                          │
│                                                               │
│  candidates = index.Query(P.Envelope)  ← O(log n) complexity│
│  for each polygon in candidates:                             │
│      if polygon.intersects(P):                               │
│          add to neighbors                                    │
│                                                               │
│  Time: ~10ms for 10,000 polygons (100x faster!)            │
└──────────────────────────────────────────────────────────────┘
```

## Integration Workflow

### Complete Generation Pipeline

```
┌─────────────────────────────────────────────────────────────────┐
│                     PLANET GENERATION PIPELINE                   │
└─────────────────────────────────────────────────────────────────┘
                              │
                              ▼
                    ┌──────────────────┐
                    │ Input Config     │
                    │ • Radius         │
                    │ • Plate Count    │
                    │ • Ocean Coverage │
                    │ • Seed           │
                    └────────┬─────────┘
                             │
                             ▼
                    ┌──────────────────┐
                    │ 1. Generate      │
                    │ Spherical Points │
                    │ (Voronoi Seeds)  │
                    └────────┬─────────┘
                             │
                             ▼
                    ┌──────────────────┐
                    │ 2. Create        │
                    │ Voronoi Cells    │
                    │ (Plates)         │
                    └────────┬─────────┘
                             │
                             ▼
                    ┌──────────────────┐
                    │ 3. Apply         │
                    │ Tectonic Forces  │
                    │ • Compression    │
                    │ • Extension      │
                    └────────┬─────────┘
                             │
                             ▼
                    ┌──────────────────┐
                    │ 4. Classify      │
                    │ Biomes           │
                    │ • Climate Model  │
                    │ • Elevation      │
                    └────────┬─────────┘
                             │
                             ▼
                    ┌──────────────────┐
                    │ 5. Project to    │
                    │ SRID_METER       │
                    │ (Equirectangular)│
                    └────────┬─────────┘
                             │
                             ▼
                    ┌──────────────────┐
                    │ 6. Apply World   │
                    │ Wrapping         │
                    │ • Date Line      │
                    │ • Polar Clamp    │
                    └────────┬─────────┘
                             │
                             ▼
                    ┌──────────────────┐
                    │ 7. Validate      │
                    │ Topology         │
                    │ • Fix Invalid    │
                    │ • Buffer(0)      │
                    └────────┬─────────┘
                             │
                             ▼
                    ┌──────────────────┐
                    │ Output:          │
                    │ List<Polygon>    │
                    │ with metadata    │
                    └──────────────────┘
```

## Related Documentation

- [Feature Specification](spec-spherical-planet-generation.md) - Complete requirements
- [Technical Implementation](tech-spherical-planet-implementation.md) - Detailed code examples
- [Quick Reference](quick-reference-spherical-planet.md) - Fast lookup guide
- [Developer Guide](developer-guide-spherical-planet-generation.md) - Step-by-step tutorials
- [API Specification](api-spherical-planet-generation.md) - REST API documentation

---

**Note:** The ASCII diagrams in this document are simplified representations. Actual map projections involve complex mathematical transformations that preserve different properties (area, angle, distance, or direction) based on the projection type selected.

**Last Updated:** 2024-12-29  
**Maintained By:** Technical Documentation Team
