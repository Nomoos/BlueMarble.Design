# Content Extraction: OpenStreetMap for Planet-Scale World Generation

---
title: OpenStreetMap Data Extraction for BlueMarble World Building
date: 2025-01-15
tags: [openstreetmap, world-generation, terrain, resources, implementation]
status: active
priority: 1 - Immediate Implementation
source: https://planet.openstreetmap.org/
parent-research: survival-guides-knowledge-domains-research.md
---

**Document Type:** Content Extraction Guide  
**Version:** 1.0  
**Author:** Game Design Research Team  
**Date:** 2025-01-15  
**Priority:** Critical - Phase 1 Implementation  
**Implementation Status:** Ready for Development Team

## Overview

This document provides a practical guide for extracting and utilizing OpenStreetMap (OSM) data to create 
authentic, planet-scale terrain and resource distribution for BlueMarble. OSM provides real-world geographical 
data that can inform procedural generation systems, biome placement, and resource clustering patterns.

## Source Information

### OpenStreetMap Database

**Primary Source:** https://planet.openstreetmap.org/

**Data Format:**
- **Planet Files:** Complete OSM database dumps (50-100 GB compressed)
- **Regional Extracts:** Continent/country-specific data (smaller, more manageable)
- **Format:** XML (.osm) or Protocol Buffer Binary (.pbf)
- **License:** Open Database License (ODbL) - Free to use with attribution

**Key Data Categories:**
- Terrain elevation and topology
- Water bodies (rivers, lakes, coastlines)
- Biome indicators (forest, grassland, desert classifications)
- Infrastructure patterns (road networks, settlements)
- Resource location patterns (mining areas, agricultural zones)

### Download Options

**1. Full Planet Data:**
- URL: https://planet.openstreetmap.org/pbf/planet-latest.osm.pbf
- Size: ~70 GB compressed
- Update Frequency: Weekly
- Use Case: Complete planetary reference data

**2. Regional Extracts:**
- Source: http://download.geofabrik.de/
- Available regions: Continents, countries, states
- Size: 50 MB - 5 GB per region
- Use Case: Testing and prototyping

**3. Thematic Extracts:**
- Terrain elevation data
- Water network data
- Land use classification
- Settlement patterns

## Data Extraction Strategy for BlueMarble

### Phase 1: Terrain Generation Reference (Week 1-2)

**Objective:** Extract terrain topology patterns to inform procedural generation algorithms

**Data Points to Extract:**

1. **Elevation Patterns**
   - Mountain range formations
   - Valley configurations
   - Plateau structures
   - Coastal elevation profiles

2. **Water Distribution**
   - River network density and patterns
   - Lake formation clustering
   - Coastline complexity
   - Watershed boundaries

3. **Terrain Slope Analysis**
   - Steep terrain (mountains, cliffs)
   - Moderate slopes (hills, gradual transitions)
   - Flat terrain (plains, plateaus)
   - Distribution percentages

**Extraction Method:**
```python
# Pseudocode for terrain pattern extraction
import osmium
import numpy as np

class TerrainAnalyzer(osmium.SimpleHandler):
    def __init__(self):
        osmium.SimpleHandler.__init__(self)
        self.elevation_data = []
        self.water_features = []
        self.terrain_types = {}
    
    def node(self, n):
        # Extract elevation tags
        if 'ele' in n.tags:
            elevation = float(n.tags['ele'])
            lat = n.location.lat
            lon = n.location.lon
            self.elevation_data.append({
                'lat': lat, 
                'lon': lon, 
                'elevation': elevation
            })
    
    def way(self, w):
        # Extract water features
        if 'waterway' in w.tags:
            self.water_features.append({
                'type': w.tags['waterway'],
                'nodes': [n.ref for n in w.nodes]
            })
        
        # Extract terrain classifications
        if 'natural' in w.tags:
            terrain_type = w.tags['natural']
            self.terrain_types[terrain_type] = \
                self.terrain_types.get(terrain_type, 0) + 1

# Usage
handler = TerrainAnalyzer()
handler.apply_file('region-extract.osm.pbf')

# Generate statistical patterns
terrain_stats = analyze_patterns(handler.elevation_data)
```

**Output for BlueMarble:**
- Terrain height distribution curves
- Water density maps
- Slope angle histograms
- Biome transition probabilities

### Phase 2: Biome Distribution Patterns (Week 3-4)

**Objective:** Extract real-world biome placement patterns based on geography

**OSM Tags for Biome Identification:**

| OSM Tag | Value | BlueMarble Biome |
|---------|-------|------------------|
| natural | forest | Temperate Forest |
| natural | grassland | Grassland/Savanna |
| natural | scrub | Mediterranean Scrubland |
| natural | sand | Desert |
| natural | wetland | Swamp/Marsh |
| natural | tundra | Tundra |
| natural | glacier | Ice/Snow |

**Data to Extract:**

1. **Biome Clustering Patterns**
   - How biomes cluster geographically
   - Transition zones between biomes
   - Size distribution of biome patches
   - Elevation correlation with biome types

2. **Climate Indicators**
   - Latitude correlation with biome
   - Distance from water influence
   - Elevation effects on biome type
   - Continental vs coastal patterns

3. **Biome Diversity Metrics**
   - Number of distinct biomes per region
   - Biome edge density
   - Fragmentation patterns
   - Connectivity of similar biomes

**Implementation for BlueMarble:**
```
BiomeGenerationRules {
    - LatitudinalZones: Equatorial, Tropical, Temperate, Polar
    - ElevationModifiers: Sea level, Low hills, Mountains, High peaks
    - MoistureGradients: Coast → Inland drying pattern
    - ContinentalityEffects: Size of landmass affects climate
}

BiomePlacementAlgorithm {
    1. Generate base climate zones by latitude
    2. Apply elevation modifiers (mountains = colder/wetter)
    3. Calculate moisture gradients from water sources
    4. Add transition zones between biomes (gradual changes)
    5. Apply noise for natural variation
    6. Validate against OSM statistical patterns
}
```

### Phase 3: Resource Distribution Logic (Week 5-6)

**Objective:** Use real-world geological and land-use patterns to inform resource placement

**OSM Data for Resource Indicators:**

1. **Mining and Quarry Locations**
   - OSM Tag: `landuse=quarry`
   - OSM Tag: `man_made=mineshaft`
   - Indicates: Geological formations with extractable minerals
   - BlueMarble Use: Weight ore deposit probability

2. **Agricultural Suitability**
   - OSM Tag: `landuse=farmland`
   - OSM Tag: `landuse=orchard`
   - Indicates: Fertile soil, adequate water, favorable climate
   - BlueMarble Use: Arable land distribution

3. **Forest Resources**
   - OSM Tag: `natural=wood`
   - OSM Tag: `landuse=forest`
   - Density variation by region
   - BlueMarble Use: Timber resource availability

4. **Water Sources**
   - OSM Tag: `natural=spring`
   - OSM Tag: `natural=water` (wells)
   - Distribution patterns
   - BlueMarble Use: Fresh water access points

**Resource Distribution Algorithm:**
```
ResourcePlacementSystem {
    // Mineral Resources
    OreDeposits {
        - MountainousTerrain: Higher probability of metallic ores
        - VolcanicRegions: Rare minerals, sulfur deposits
        - SedimentaryBasins: Coal, limestone, clay
        - RiverBeds: Placer deposits (gold, gems)
    }
    
    // Biological Resources
    BiologicalResources {
        - ForestDensity: Based on biome and rainfall patterns
        - GameAnimalPopulations: By biome type and food availability
        - FishPopulations: In water bodies, by water type
        - MedicinalPlants: Specific biome distributions
    }
    
    // Agricultural Potential
    AgriculturalZones {
        - SoilQuality: Derived from terrain and biome
        - WaterAccess: Distance to water sources
        - ClimateZone: Growing season length
        - TerrainSlope: Flat land preferred
    }
}
```

### Phase 4: Settlement and Infrastructure Patterns (Week 7-8)

**Objective:** Use real-world settlement patterns to guide player city placement and NPC settlements

**OSM Settlement Data:**

1. **Settlement Size Distribution**
   - Villages: population < 5,000
   - Towns: population 5,000-50,000
   - Cities: population > 50,000
   - Density by region type

2. **Settlement Location Factors**
   - Water access (rivers, lakes, coasts)
   - Flat or gently sloping terrain
   - Proximity to resources
   - Trade route intersections

3. **Infrastructure Networks**
   - Road density patterns
   - River navigation routes
   - Mountain pass locations
   - Coastal port configurations

**BlueMarble NPC Settlement System:**
```
NPCSettlementGeneration {
    // Initial settlement placement
    SettlementSeedPoints {
        - CoastalBias: 60% near coastlines (real-world pattern)
        - RiverBias: 30% along major rivers
        - InlandResources: 10% at valuable resource locations
    }
    
    // Settlement growth factors
    GrowthFactors {
        - WaterAccess: Critical for early settlements
        - ArableLand: Supports larger populations
        - ResourceProximity: Mining towns, lumber camps
        - TradeRouteAccess: Crossroads become cities
    }
    
    // Settlement density
    SettlementDistribution {
        - MinimumSpacing: 5-10 km between villages
        - RegionalClusters: Cities spawn towns/villages nearby
        - WildernessZones: Large areas with no settlements
    }
}
```

## Practical Implementation Steps

### Step 1: Download Reference Data (Day 1)

**Action Items:**
1. Download regional extracts for diverse geographical areas:
   - Europe (temperate, varied terrain)
   - North Africa (desert and Mediterranean)
   - Southeast Asia (tropical, monsoon)
   - North America (continental climate diversity)

2. Tools needed:
   - osmium-tool (command-line OSM processing)
   - QGIS (visualization and analysis)
   - Python with pyosmium library

**Commands:**
```bash
# Download regional extracts
wget http://download.geofabrik.de/europe-latest.osm.pbf
wget http://download.geofabrik.de/africa-latest.osm.pbf
wget http://download.geofabrik.de/asia-latest.osm.pbf
wget http://download.geofabrik.de/north-america-latest.osm.pbf

# Extract specific data categories
osmium tags-filter europe-latest.osm.pbf \
    natural=forest,grassland,water \
    landuse=farmland,forest \
    -o biome-data.osm.pbf

osmium tags-filter europe-latest.osm.pbf \
    natural=water,spring \
    waterway=river,stream \
    -o water-features.osm.pbf
```

### Step 2: Statistical Analysis (Days 2-5)

**Metrics to Calculate:**

1. **Terrain Statistics:**
   - Elevation distribution (histogram)
   - Slope angle distribution
   - Aspect distribution
   - Roughness metrics

2. **Biome Statistics:**
   - Biome type percentages by latitude band
   - Average biome patch size
   - Biome transition frequencies
   - Elevation ranges per biome

3. **Water Network Statistics:**
   - River density by terrain type
   - Lake size distribution
   - Watershed sizes
   - Coastline fractal dimension

4. **Settlement Statistics:**
   - Settlement size distribution
   - Inter-settlement distances
   - Settlement elevation preferences
   - Coastal vs inland ratios

**Output Format:**
```json
{
    "terrain_analysis": {
        "elevation": {
            "min": 0,
            "max": 8848,
            "mean": 237,
            "median": 145,
            "percentiles": {
                "25": 50,
                "50": 145,
                "75": 380,
                "90": 750,
                "95": 1200
            }
        },
        "slope_angles": {
            "flat_0_5": 0.42,
            "gentle_5_15": 0.31,
            "moderate_15_30": 0.18,
            "steep_30_plus": 0.09
        }
    },
    "biome_distribution": {
        "by_latitude": {
            "equatorial_0_10": {
                "tropical_rainforest": 0.70,
                "tropical_savanna": 0.25,
                "mangrove": 0.05
            },
            "tropical_10_23": {
                "tropical_forest": 0.40,
                "savanna": 0.35,
                "desert": 0.15,
                "grassland": 0.10
            }
        }
    }
}
```

### Step 3: Create Procedural Generation Rules (Days 6-10)

**Deliverables:**

1. **Terrain Generation Ruleset**
   - Noise function parameters for realistic terrain
   - Elevation distribution matching OSM stats
   - Water placement probability maps
   - Erosion simulation parameters

2. **Biome Placement Ruleset**
   - Latitude-based base climate
   - Elevation adjustments
   - Moisture gradient calculations
   - Transition zone widths

3. **Resource Distribution Ruleset**
   - Ore deposit probability by terrain type
   - Forest density by biome
   - Water source placement
   - Agricultural potential scoring

4. **Settlement Generation Ruleset**
   - Initial placement algorithm
   - Growth simulation parameters
   - Infrastructure generation
   - Naming conventions (from OSM name patterns)

### Step 4: Validation and Iteration (Days 11-14)

**Validation Process:**

1. **Generate Test Worlds**
   - Create 5-10 test continents using new rules
   - Vary parameters (continental size, latitude range)
   - Generate at multiple scales (regional to planetary)

2. **Compare Against OSM Statistics**
   - Calculate same metrics for generated worlds
   - Compare distributions to real-world data
   - Identify discrepancies

3. **Visual Validation**
   - Generate maps of test worlds
   - Compare visually to real-world maps
   - Check for obvious artifacts or unrealistic patterns

4. **Gameplay Validation**
   - Does resource distribution create interesting trade opportunities?
   - Are settlement locations logical and accessible?
   - Is terrain navigable but varied?
   - Do biomes support diverse gameplay?

## Integration with BlueMarble Systems

### Connection to Existing Research

**Geological Simulation:**
- OSM terrain patterns inform base topology
- Mineral distribution from geological formations
- Erosion patterns from real-world examples

**Material Systems:**
- Resource clustering creates regional specialization
- Trade routes emerge from resource distribution
- Quality variations by geological origin

**Settlement Systems:**
- NPC city placement follows historical patterns
- Player cities have optimal locations
- Infrastructure development mirrors real progression

### Technical Integration Points

**World Generation Pipeline:**
```
1. Generate Base Terrain
   ├─ Use OSM-derived noise parameters
   ├─ Apply elevation distribution statistics
   └─ Create realistic mountain/valley formations

2. Place Water Features
   ├─ River network generation (follows terrain)
   ├─ Lake placement (terrain depressions)
   └─ Coastline generation (fractal patterns)

3. Assign Biomes
   ├─ Latitude-based climate zones
   ├─ Elevation adjustments
   ├─ Moisture gradients from water
   └─ Transition zones between biomes

4. Distribute Resources
   ├─ Geological ore deposits
   ├─ Forest/vegetation density
   ├─ Water sources
   └─ Wildlife spawning zones

5. Generate Settlements
   ├─ Initial NPC city placement
   ├─ Town and village distribution
   ├─ Road network generation
   └─ Points of interest (ruins, camps, etc.)
```

## Data Files and Deliverables

### Immediate Deliverables (Week 1-2)

1. **terrain_statistics.json**
   - Comprehensive statistical analysis of OSM terrain data
   - Elevation, slope, aspect distributions
   - Regional variations

2. **biome_distribution_rules.json**
   - Biome placement probabilities by conditions
   - Transition zone specifications
   - Climate factor weights

3. **water_network_patterns.json**
   - River density maps
   - Lake size distributions
   - Watershed characteristics

### Medium-Term Deliverables (Week 3-4)

4. **resource_placement_rules.json**
   - Ore deposit probabilities
   - Forest density maps
   - Agricultural suitability scoring

5. **settlement_generation_config.json**
   - Settlement placement algorithm parameters
   - Size distribution rules
   - Infrastructure generation specs

### Long-Term Deliverables (Week 5-8)

6. **procedural_world_generator.py**
   - Complete world generation script
   - Uses all extracted rulesets
   - Generates test worlds for validation

7. **validation_report.md**
   - Comparison of generated vs real-world statistics
   - Visual examples
   - Recommendations for tuning

## Success Metrics

### Quantitative Metrics

- **Terrain Realism:** Generated elevation distributions within 10% of OSM statistics
- **Biome Diversity:** 8-12 distinct biomes per continent
- **Resource Distribution:** No single region contains >30% of any rare resource
- **Settlement Density:** 1 city per 10,000 km², 1 village per 500 km²

### Qualitative Metrics

- **Visual Realism:** Generated maps resemble real geography
- **Gameplay Balance:** Resource distribution creates interesting economics
- **Exploration Value:** Varied terrain encourages exploration
- **Logical Settlement:** Cities located where players would expect them

## Tools and Resources

### Required Software

1. **osmium-tool** - Fast OSM data processing
   - Installation: `apt-get install osmium-tool`
   - Documentation: https://osmcode.org/osmium-tool/

2. **QGIS** - Visualization and spatial analysis
   - Installation: https://qgis.org/en/site/forusers/download.html
   - OSM plugin available

3. **Python Libraries:**
   - pyosmium - OSM data parsing
   - numpy/scipy - Statistical analysis
   - matplotlib - Visualization
   - noise - Perlin/Simplex noise for terrain generation

### Useful References

- **OSM Wiki - Map Features:** https://wiki.openstreetmap.org/wiki/Map_Features
- **OSM Wiki - Tagging:** https://wiki.openstreetmap.org/wiki/Tags
- **Geofabrik Downloads:** http://download.geofabrik.de/
- **OSM Data Processing Tutorial:** https://osmcode.org/pyosmium/

## Timeline and Resource Allocation

### Week 1-2: Data Acquisition and Initial Analysis
- **Effort:** 1 developer full-time
- **Output:** Statistical datasets, initial rulesets
- **Blockers:** None (data is publicly available)

### Week 3-4: Biome and Resource Rules
- **Effort:** 1 developer full-time
- **Output:** Complete ruleset files
- **Blockers:** May need design input on balance

### Week 5-6: Settlement Generation
- **Effort:** 1 developer + 1 designer (part-time)
- **Output:** Settlement placement system
- **Blockers:** Requires terrain/biome completion

### Week 7-8: Integration and Validation
- **Effort:** 1 developer + 1 QA tester
- **Output:** Working prototype, validation report
- **Blockers:** Requires world generation system architecture

## Next Steps

1. **Assign Development Resources:** Allocate 1 developer to this extraction project
2. **Set Up Development Environment:** Install required tools and libraries
3. **Begin Phase 1:** Download and analyze initial OSM datasets
4. **Weekly Progress Reviews:** Track against timeline
5. **Iterate Based on Results:** Adjust approach as patterns emerge

## Related Documents

- [Survival Guides Knowledge Domains Research](survival-guides-knowledge-domains-research.md) - Parent research document
- [Content Extraction 02: Appropriate Technology Library](survival-content-extraction-02-appropriate-technology.md) - Next priority
- [Content Extraction 03: Survivor Library](survival-content-extraction-03-survivor-library.md) - Following priority

---

**Document Status:** Ready for implementation  
**Last Updated:** 2025-01-15  
**Next Review:** After Phase 1 completion (Week 2)
