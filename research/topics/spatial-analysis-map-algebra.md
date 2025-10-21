# Spatial Analysis: Map Algebra Toolset

## Problem / Context

BlueMarble's terrain and simulation systems require complex mathematical operations on spatial data. Map algebra provides a framework for expressing spatial analysis as mathematical operations on rasters, enabling powerful terrain processing, resource calculations, and environmental modeling.

This research examines map algebra concepts and operations to inform BlueMarble's spatial computation architecture.

## Key Findings

- **Local Operations**: Cell-by-cell calculations
  - Arithmetic: addition, subtraction, multiplication, division
  - Logical: AND, OR, NOT, XOR
  - Relational: greater than, less than, equal to
  - Functional: trigonometric, exponential, logarithmic
  - Each output cell depends only on corresponding input cell(s)

- **Focal Operations**: Neighborhood analysis
  - Moving window calculations (3x3, 5x5, circular, etc.)
  - Statistics: mean, median, standard deviation, range
  - Filters: smoothing, edge detection, slope calculation
  - Output cell depends on surrounding cells
  - Essential for terrain derivatives (slope, aspect, curvature)

- **Zonal Operations**: Statistics within regions
  - Calculates statistics for each zone (mean elevation per basin)
  - Zones defined by categorical raster
  - Statistics: sum, mean, median, min, max, variety, area
  - Useful for summarizing data by geographic unit

- **Global Operations**: Whole-raster calculations
  - Distance operations: Euclidean distance, cost distance
  - Statistics: histogram, global mean/std
  - Frequency analysis: value counts
  - Used for connectivity and accessibility analysis

## Evidence

### Source 1: Map Algebra Toolset Overview

- **Link**: https://pro.arcgis.com/en/pro-app/3.4/tool-reference/spatial-analyst/an-overview-of-the-map-algebra-toolset.htm
- **Key Points**:
  - Unified framework for spatial calculations
  - Expression-based interface: output = input1 + input2 * 0.5
  - Tools include: Raster Calculator, Cell Statistics, Combinatorial operators
  - Supports complex multi-step analysis workflows
- **Relevance**: Architectural pattern for BlueMarble's spatial computation engine

### Source 2: Local Operations

- **Link**: (ArcGIS Map Algebra documentation)
- **Key Points**:
  - Simple per-cell operations: elevation_diff = dem2 - dem1
  - Conditional operations: output = Con(slope > 30, 1, 0)
  - Combine multiple rasters: suitability = 0.5*proximity + 0.3*slope + 0.2*aspect
  - Highly parallelizable (GPU-friendly)
- **Relevance**: Foundation for resource calculations, environmental modeling

### Source 3: Focal Operations

- **Link**: (ArcGIS Focal Statistics documentation)
- **Key Points**:
  - Neighborhood shapes: rectangle, circle, annulus, wedge
  - Statistics: Mean (smoothing), Std (roughness), Range (local variation)
  - Edge handling: ignore no-data, replicate edges, reflect
  - Separable filters can optimize performance
- **Relevance**: Terrain analysis, smooth transitions, feature detection

### Data/Observations

- Map algebra expressions are compositional:
  - Simple operations combine into complex analysis
  - Example: habitat suitability = (water_proximity < 1000) & (slope < 15) & (elevation > 500)
  - Readable, maintainable, and debuggable

- Performance characteristics:
  - Local operations: embarrassingly parallel, O(n) where n=cells
  - Focal operations: O(n*k) where k=window size, but separable filters reduce to O(n)
  - Zonal operations: O(n + z*log(z)) where z=zones
  - Global operations: varies by algorithm (distance transforms O(n), histograms O(n))

- Data type considerations:
  - Integer rasters: exact values, support modulo/bitwise operations
  - Float rasters: continuous values, require careful equality testing
  - Boolean rasters: efficient storage, fast logical operations

## Implications for Design

- **Terrain Analysis Layer**: Map algebra engine as core service
  - Expression evaluator for spatial calculations
  - Cached intermediate results for complex queries
  - GPU acceleration for large-scale operations
  - Impact: Flexible, powerful spatial analysis without hardcoding every operation

- **Resource Distribution**: Use map algebra for procedural placement
  - Combine elevation, slope, climate, proximity to define suitability
  - Example: ore_deposits = (depth > 1000) * (temperature < 500) * noise()
  - Probabilistic placement based on calculated suitability
  - Impact: Realistic, controllable resource generation

- **Gameplay Calculations**: Real-time map algebra for game mechanics
  - Agricultural yield = fertility * water_availability * temperature_suitability
  - Accessibility = cost_distance(roads) + terrain_difficulty
  - Visibility = viewshed(observer_position, terrain)
  - Impact: Deep simulation emerging from simple spatial rules

- **Modding Support**: Expose map algebra to modders
  - Custom resource placement rules
  - New environmental effects
  - Player-created analysis tools
  - Impact: Extensible simulation without source code access

## Open Questions / Next Steps

### Open Questions

- Should we implement a full scripting language or just predefined operations?
- How do we handle temporal map algebra (raster time series)?
- What's the balance between pre-computed vs runtime-computed spatial data?
- Should focal operations be limited to prevent performance abuse?

### Next Steps

- [ ] Design map algebra expression syntax for BlueMarble
- [ ] Implement basic local operations (arithmetic, logical, relational)
- [ ] Prototype focal operations with GPU acceleration
- [ ] Create library of common spatial analysis expressions
- [ ] Benchmark performance on representative terrain sizes
- [ ] Document map algebra API for content creators

## Related Documents

- [Spatial Analysis: Hydrology Tools](spatial-analysis-hydrology-tools.md)
- [Spatial Analysis: Interpolation Tools](spatial-analysis-interpolation-tools.md)
- [Spatial Analysis: Generalization Tools](spatial-analysis-generalization-tools.md)
- [Enhanced 3D Geometry Operations](../../src/BlueMarble.World/Enhanced3DGeometryOps.cs)
- [Material Economy System](../../docs/gameplay/mechanics/material-economy-system.md)
