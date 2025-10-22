# Spatial Analysis: Groundwater Tools

## Problem / Context

BlueMarble's geological simulation requires subsurface water modeling to support realistic mining, resource extraction, and underground gameplay. Understanding professional groundwater analysis tools helps design accurate aquifer systems and subsurface fluid dynamics.

This research examines ArcGIS Spatial Analyst groundwater tools (Darcy Flow, Darcy Velocity) to understand subsurface water movement and application to BlueMarble's deep mining mechanics.

## Key Findings

- **Darcy's Law**: Fundamental equation governing groundwater flow through porous media
  - Flow rate proportional to hydraulic gradient and permeability
  - Q = -K * A * (dh/dl) where K=hydraulic conductivity, A=cross-sectional area, dh/dl=hydraulic gradient
  - Applies to steady-state flow in saturated conditions

- **Darcy Flow Tool**: Calculates groundwater volume flow rate per unit width
  - Requires input rasters: head surface, porosity, thickness, transmissivity
  - Outputs volumetric flow rate (volume/time/width)
  - Used for estimating water yield from aquifers

- **Darcy Velocity Tool**: Calculates seepage velocity (speed of water through pores)
  - Outputs both magnitude and direction of flow
  - Seepage velocity = flux / effective porosity
  - Critical for contaminant transport modeling

- **Subsurface Modeling**: Three-dimensional analysis of underground water movement
  - Requires geological layering (aquitards vs aquifers)
  - Permeability varies by material type (sand > clay)
  - Confined vs unconfined aquifer behavior differs significantly

## Evidence

### Source 1: Darcy Flow Tool

- **Link**: https://pro.arcgis.com/en/pro-app/3.4/tool-reference/spatial-analyst/darcy-flow.htm
- **Key Points**:
  - Calculates volumetric flow rate of groundwater through aquifer
  - Formula: Q = T * W * dh/dl (T=transmissivity, W=width, dh/dl=gradient)
  - Input: head raster (potentiometric surface)
  - Output: flow magnitude raster
- **Relevance**: Determines how fast water moves through underground materials

### Source 2: Darcy Velocity Tool

- **Link**: https://pro.arcgis.com/en/pro-app/3.4/tool-reference/spatial-analyst/darcy-velocity.htm
- **Key Points**:
  - Calculates seepage velocity (actual water particle speed)
  - Outputs magnitude and direction (vector field)
  - Requires porosity values for each material type
  - Formula: v = K * (dh/dl) / n where n=effective porosity
- **Relevance**: Predicts contaminant spread, useful for pollution gameplay mechanics

### Source 3: Groundwater Toolset Overview

- **Link**: https://pro.arcgis.com/en/pro-app/3.4/tool-reference/spatial-analyst/an-overview-of-the-groundwater-tools.htm
- **Key Points**:
  - Complete toolkit for subsurface water analysis
  - Includes: Darcy Flow, Darcy Velocity, Porous Puff, Particle Track
  - Requires detailed geological parameters (K, porosity, thickness)
  - Supports steady-state and transient analysis
- **Relevance**: Framework for implementing realistic underground water systems

### Data/Observations

- Hydraulic conductivity varies by orders of magnitude:
  - Gravel: 10⁻² to 10² m/day
  - Sand: 10⁻³ to 10⁰ m/day
  - Clay: 10⁻⁷ to 10⁻³ m/day
  - Granite: 10⁻¹⁰ to 10⁻⁶ m/day
- Groundwater moves much slower than surface water (cm/day vs m/s)
- Confined aquifers can be pressurized (artesian conditions)
- Water tables follow surface topography at large scale but lag behind

## Implications for Design

- **Mining Mechanics**: Groundwater inflow affects mining operations
  - Deeper mines encounter more water
  - High-permeability layers flood faster
  - Pumping systems required for deep excavations
  - Impact: Realistic challenge for underground expansion

- **Material Properties**: Each geological layer needs permeability value
  - Sand/gravel: high permeability (easy drainage, fast flooding)
  - Clay/shale: low permeability (natural barrier, slow seepage)
  - Fractured rock: variable permeability (unpredictable behavior)
  - Impact: Material choice matters for underground construction

- **Aquifer Gameplay**: Water table mechanics create strategic depth
  - Wells must reach aquifer depth
  - Over-pumping lowers water table
  - Contamination spreads along flow paths
  - Impact: Resource management and environmental consequences

- **Performance Considerations**: Simplified Darcy simulation for game scale
  - Pre-compute steady-state aquifer geometry
  - Approximate dynamic changes with regional water balance
  - Full 3D flow only in critical areas (player mining sites)
  - Impact: Realistic behavior without excessive computation

## Open Questions / Next Steps

### Open Questions

- What spatial resolution is needed for groundwater simulation (km vs m)?
- Should aquifer depletion be permanent or regenerate over time?
- How do we handle the transition from saturated to unsaturated zones?
- Should subsurface flow affect surface features (springs, wetlands)?

### Next Steps

- [ ] Define hydraulic conductivity values for BlueMarble's material types
- [ ] Design simplified Darcy flow algorithm for voxel grid
- [ ] Prototype water table calculation for test region
- [ ] Implement well drilling mechanics with aquifer interaction
- [ ] Balance pumping rates and recharge for gameplay pacing

## Related Documents

- [Spatial Analysis: Hydrology Tools](spatial-analysis-hydrology-tools.md)
- [Mining and Resource Extraction](../../docs/gameplay/mechanics/mining-resource-extraction.md)
- [Material Economy System](../../docs/gameplay/mechanics/material-economy-system.md)
- [Terraforming Mechanics](../../docs/gameplay/mechanics/terraforming.md)
