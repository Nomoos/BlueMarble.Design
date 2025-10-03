# Content Extraction Guide 01: OpenStreetMap Data for World Generation

---
title: OpenStreetMap Data Extraction for World Generation
date: 2025-01-15
tags: [world-generation, openstreetmap, terrain, geography]
status: completed
priority: critical
source: https://www.openstreetmap.org/
---

## Executive Summary

OpenStreetMap (OSM) provides comprehensive geographical data that can be leveraged for realistic world generation in BlueMarble. This guide outlines how to extract, process, and integrate OSM data for terrain generation, resource distribution, and location-based content.

**Key Applications:**
- Terrain elevation and topology
- Biome distribution (forests, grasslands, deserts)
- Water features (rivers, lakes, coastlines)
- Real-world location references
- Resource node placement logic

**Implementation Priority:** CRITICAL - Foundation for all spatial gameplay

## Source Overview

### OpenStreetMap Data Structure

**What is OpenStreetMap:**
- Collaborative open-source map of the world
- Contains detailed geographical and infrastructure data
- Free to use under Open Database License (ODbL)
- Continuously updated by community contributors

**Data Categories Relevant to BlueMarble:**
1. **Natural Features**
   - Terrain elevation
   - Forests and vegetation
   - Water bodies
   - Geological features

2. **Infrastructure** (Historical reference)
   - Settlement patterns
   - Road networks
   - Agricultural areas
   - Resource extraction sites

3. **Metadata**
   - Naming conventions
   - Cultural regions
   - Climate zones

**Data Format:**
- XML-based (.osm files)
- PBF (Protocol Buffer Binary Format) for large datasets
- GeoJSON for web integration
- Shapefiles for GIS applications

**Data Size:**
- Full planet export: ~100 GB compressed
- Regional extracts: 100 MB - 10 GB
- City-level data: 1-100 MB

## Content Extraction Strategy

### Phase 1: Data Acquisition (Week 1)

**Step 1: Download Regional Extracts**

Use Geofabrik for pre-processed regional data:
```bash
# Download example - North America extract
wget https://download.geofabrik.de/north-america-latest.osm.pbf

# Or specific country
wget https://download.geofabrik.de/europe/germany-latest.osm.pbf
```

**Alternative Sources:**
- Planet.osm (full planet export)
- BBBike extracts (city-specific)
- Overpass API (custom queries)

**Step 2: Install Processing Tools**

```bash
# Install osmosis for data processing
sudo apt-get install osmosis

# Install GDAL for format conversion
sudo apt-get install gdal-bin

# Install PostgreSQL with PostGIS extension
sudo apt-get install postgresql postgis

# Python libraries
pip install osmium pyosmium shapely rasterio
```

### Phase 2: Data Processing (Week 2)

**Extract Terrain Elevation Data**

```python
import osmium
import json

class ElevationHandler(osmium.SimpleHandler):
    def __init__(self):
        osmium.SimpleHandler.__init__(self)
        self.elevation_points = []
    
    def node(self, n):
        if 'ele' in n.tags:
            self.elevation_points.append({
                'lat': n.location.lat,
                'lon': n.location.lon,
                'elevation': float(n.tags['ele'])
            })
    
    def export_to_json(self, filename):
        with open(filename, 'w') as f:
            json.dump(self.elevation_points, f)

# Usage
handler = ElevationHandler()
handler.apply_file('region.osm.pbf')
handler.export_to_json('elevation_data.json')
```

**Extract Natural Features**

```python
class NaturalFeaturesHandler(osmium.SimpleHandler):
    def __init__(self):
        osmium.SimpleHandler.__init__(self)
        self.forests = []
        self.water = []
        self.mountains = []
    
    def area(self, a):
        if 'natural' in a.tags:
            feature_type = a.tags['natural']
            
            # Extract forest areas
            if feature_type in ['wood', 'forest']:
                self.forests.append({
                    'type': 'forest',
                    'geometry': self.extract_geometry(a),
                    'name': a.tags.get('name', 'Unnamed Forest')
                })
            
            # Extract water bodies
            elif feature_type in ['water', 'wetland']:
                self.water.append({
                    'type': feature_type,
                    'geometry': self.extract_geometry(a),
                    'water_type': a.tags.get('water', 'unknown')
                })
            
            # Extract mountain features
            elif feature_type == 'peak':
                self.mountains.append({
                    'type': 'mountain_peak',
                    'lat': a.location.lat,
                    'lon': a.location.lon,
                    'elevation': a.tags.get('ele', 0),
                    'name': a.tags.get('name', 'Unnamed Peak')
                })
    
    def extract_geometry(self, area):
        """Extract polygon geometry from area"""
        # Implementation depends on pyosmium version
        # Returns list of (lat, lon) coordinates
        pass
```

**Extract Settlement Patterns**

```python
class SettlementHandler(osmium.SimpleHandler):
    def __init__(self):
        osmium.SimpleHandler.__init__(self)
        self.settlements = []
    
    def node(self, n):
        if 'place' in n.tags:
            place_type = n.tags['place']
            if place_type in ['city', 'town', 'village', 'hamlet']:
                self.settlements.append({
                    'type': place_type,
                    'name': n.tags.get('name', 'Unnamed'),
                    'lat': n.location.lat,
                    'lon': n.location.lon,
                    'population': n.tags.get('population', 0)
                })
    
    def get_settlement_density_map(self, grid_size=100):
        """Create heat map of settlement density"""
        # Used for determining resource spawn probabilities
        # Areas near historical settlements = higher resource density
        density_map = {}
        # Implementation: bin settlements into grid squares
        return density_map
```

### Phase 3: Game Integration (Week 3-4)

**Terrain Generation Algorithm**

```python
class TerrainGenerator:
    def __init__(self, osm_data):
        self.elevation_data = osm_data['elevation']
        self.water_bodies = osm_data['water']
        self.forest_areas = osm_data['forests']
    
    def generate_heightmap(self, region_bounds, resolution=1024):
        """
        Generate heightmap for game terrain
        
        Args:
            region_bounds: (min_lat, min_lon, max_lat, max_lon)
            resolution: Heightmap resolution (1024x1024, 2048x2048, etc.)
        
        Returns:
            2D array of elevation values normalized to [0, 1]
        """
        import numpy as np
        from scipy.interpolate import griddata
        
        # Extract elevation points within bounds
        points = []
        values = []
        for point in self.elevation_data:
            if self.in_bounds(point, region_bounds):
                points.append([point['lat'], point['lon']])
                values.append(point['elevation'])
        
        # Create grid
        lat_range = np.linspace(region_bounds[0], region_bounds[2], resolution)
        lon_range = np.linspace(region_bounds[1], region_bounds[3], resolution)
        grid_lat, grid_lon = np.meshgrid(lat_range, lon_range)
        
        # Interpolate elevation data
        heightmap = griddata(points, values, (grid_lat, grid_lon), method='cubic')
        
        # Normalize to [0, 1]
        heightmap = (heightmap - heightmap.min()) / (heightmap.max() - heightmap.min())
        
        return heightmap
    
    def generate_biome_map(self, heightmap):
        """
        Assign biomes based on elevation and moisture
        
        Biome Rules (simplified):
        - Elevation < 0.3: Lowlands (grassland/forest)
        - Elevation 0.3-0.6: Hills (mixed forest/grassland)
        - Elevation 0.6-0.8: Mountains (sparse vegetation)
        - Elevation > 0.8: High mountains (snow/ice)
        """
        biome_map = np.zeros_like(heightmap, dtype=int)
        
        # Assign based on elevation
        biome_map[heightmap < 0.3] = 1  # Lowlands
        biome_map[(heightmap >= 0.3) & (heightmap < 0.6)] = 2  # Hills
        biome_map[(heightmap >= 0.6) & (heightmap < 0.8)] = 3  # Mountains
        biome_map[heightmap >= 0.8] = 4  # High mountains
        
        # Adjust for water bodies
        for water in self.water_bodies:
            # Mark water areas
            pass
        
        # Adjust for forests
        for forest in self.forest_areas:
            # Increase forest biome in these areas
            pass
        
        return biome_map
    
    def in_bounds(self, point, bounds):
        """Check if point is within region bounds"""
        return (bounds[0] <= point['lat'] <= bounds[2] and
                bounds[1] <= point['lon'] <= bounds[3])
```

**Resource Distribution Logic**

```python
class ResourceDistributor:
    def __init__(self, terrain_data, osm_data):
        self.terrain = terrain_data
        self.geological_features = osm_data['geological']
        self.historical_sites = osm_data['settlements']
    
    def place_mineral_nodes(self, mineral_type):
        """
        Place mineral resource nodes based on terrain and geology
        
        Rules:
        - Iron ore: Near historical mining sites, mountain regions
        - Copper: Hills and mountains
        - Coal: Sedimentary regions, historical mining areas
        - Stone: Rocky areas, mountainous regions
        """
        nodes = []
        
        if mineral_type == 'iron':
            # Prefer mountain regions (elevation 0.6-0.8)
            candidates = self.find_terrain_type(elevation_range=(0.6, 0.8))
            # Weight by proximity to historical mining sites
            weighted_candidates = self.weight_by_historical_sites(candidates)
            nodes = self.sample_locations(weighted_candidates, count=100)
        
        elif mineral_type == 'stone':
            # Abundant in rocky/mountain areas
            candidates = self.find_terrain_type(elevation_range=(0.5, 1.0))
            nodes = self.sample_locations(candidates, count=500)
        
        return nodes
    
    def place_vegetation(self, vegetation_type):
        """
        Place vegetation resources
        
        Rules:
        - Berries: Forest edges, lowlands
        - Medicinal herbs: Specific biomes based on OSM forest types
        - Wood: Dense forests
        """
        if vegetation_type == 'berries':
            # Find forest edges (forest areas adjacent to grassland)
            forest_edges = self.find_forest_edges()
            return self.sample_locations(forest_edges, count=200)
        
        elif vegetation_type == 'hardwood':
            # Dense old-growth forests
            dense_forests = self.find_dense_forests()
            return self.sample_locations(dense_forests, count=150)
        
        return []
    
    def find_terrain_type(self, elevation_range):
        """Find all terrain cells matching elevation criteria"""
        mask = ((self.terrain.heightmap >= elevation_range[0]) &
                (self.terrain.heightmap <= elevation_range[1]))
        return np.argwhere(mask)
    
    def weight_by_historical_sites(self, candidates):
        """Weight candidate locations by proximity to historical sites"""
        # Locations near historical mining/settlement = higher probability
        pass
    
    def sample_locations(self, candidates, count):
        """Randomly sample locations from candidates"""
        if len(candidates) <= count:
            return candidates
        indices = np.random.choice(len(candidates), count, replace=False)
        return candidates[indices]
```

### Phase 4: Data Export (Week 4)

**Export for Game Engine**

```python
class GameDataExporter:
    def __init__(self, processed_data):
        self.data = processed_data
    
    def export_terrain_package(self, output_dir):
        """
        Export complete terrain package for game engine
        
        Includes:
        - Heightmap (PNG or binary format)
        - Biome map (indexed image or JSON)
        - Resource spawn data (JSON)
        - Water features (vector data)
        - Named locations (JSON)
        """
        import os
        import json
        from PIL import Image
        
        os.makedirs(output_dir, exist_ok=True)
        
        # Export heightmap as 16-bit grayscale PNG
        heightmap_img = (self.data['heightmap'] * 65535).astype(np.uint16)
        Image.fromarray(heightmap_img).save(f"{output_dir}/heightmap.png")
        
        # Export biome map as indexed PNG
        biome_img = self.data['biome_map'].astype(np.uint8)
        Image.fromarray(biome_img).save(f"{output_dir}/biomes.png")
        
        # Export resource locations as JSON
        resource_data = {
            'minerals': self.data['mineral_nodes'],
            'vegetation': self.data['vegetation_nodes'],
            'water_sources': self.data['water_features']
        }
        with open(f"{output_dir}/resources.json", 'w') as f:
            json.dump(resource_data, f, indent=2)
        
        # Export named locations
        locations = {
            'mountains': self.data['mountain_peaks'],
            'settlements': self.data['settlement_sites'],
            'forests': self.data['named_forests']
        }
        with open(f"{output_dir}/locations.json", 'w') as f:
            json.dump(locations, f, indent=2)
        
        # Export metadata
        metadata = {
            'region': self.data['region_name'],
            'bounds': self.data['bounds'],
            'resolution': self.data['heightmap'].shape,
            'scale': '1 pixel = 100 meters',  # Configurable
            'generation_date': datetime.now().isoformat()
        }
        with open(f"{output_dir}/metadata.json", 'w') as f:
            json.dump(metadata, f, indent=2)

# Usage
exporter = GameDataExporter(processed_data)
exporter.export_terrain_package('output/region_01')
```

## Game Integration Details

### Unity Integration Example

```csharp
// C# code for importing terrain into Unity
using UnityEngine;
using System.IO;

public class OSMTerrainImporter : MonoBehaviour
{
    public void ImportTerrain(string dataDirectory)
    {
        // Load heightmap
        Texture2D heightmap = LoadPNG($"{dataDirectory}/heightmap.png");
        
        // Create terrain
        TerrainData terrainData = new TerrainData();
        terrainData.heightmapResolution = heightmap.width;
        terrainData.size = new Vector3(10000, 1000, 10000); // 10km x 10km
        
        // Apply heightmap
        float[,] heights = ConvertHeightmap(heightmap);
        terrainData.SetHeights(0, 0, heights);
        
        // Load and apply biome data
        Texture2D biomeMap = LoadPNG($"{dataDirectory}/biomes.png");
        ApplyBiomes(terrainData, biomeMap);
        
        // Load resource locations
        string resourceJson = File.ReadAllText($"{dataDirectory}/resources.json");
        PlaceResources(resourceJson);
        
        // Create terrain object
        GameObject terrainObj = Terrain.CreateTerrainGameObject(terrainData);
    }
    
    float[,] ConvertHeightmap(Texture2D heightmap)
    {
        int resolution = heightmap.width;
        float[,] heights = new float[resolution, resolution];
        
        for (int y = 0; y < resolution; y++)
        {
            for (int x = 0; x < resolution; x++)
            {
                Color pixel = heightmap.GetPixel(x, y);
                heights[y, x] = pixel.r; // Use red channel (grayscale)
            }
        }
        
        return heights;
    }
    
    void ApplyBiomes(TerrainData terrain, Texture2D biomeMap)
    {
        // Apply different terrain textures based on biome map
        // Spawn appropriate vegetation
        // Set terrain properties (grass color, etc.)
    }
    
    void PlaceResources(string json)
    {
        // Parse JSON and instantiate resource nodes
        // Use Unity's object pooling for performance
    }
}
```

## Deliverables

### Immediate Deliverables (Week 4)
1. **extraction_scripts/** - Python scripts for OSM data processing
2. **terrain_data/** - Processed heightmaps and biome maps for 3-5 starter regions
3. **resource_distribution.json** - Resource spawn logic based on terrain
4. **integration_guide.md** - Step-by-step Unity/Unreal integration

### Long-term Deliverables (Ongoing)
5. **procedural_generator/** - Tool for generating new regions on-demand
6. **regional_variations/** - Different biome distributions by geographic region
7. **seasonal_variations/** - Weather and season system based on geography

## Success Metrics

- **Coverage:** 5+ distinct geographic regions generated
- **Quality:** Terrain feels natural and varied
- **Performance:** <5 seconds to load a 10km x 10km region
- **Authenticity:** Players recognize real-world geographical patterns
- **Replayability:** Each region feels unique

## Integration with Other Systems

### Crafting System
- Regional resources affect available recipes
- Desert regions: Clay abundant, wood scarce
- Mountain regions: Stone and ore abundant, agriculture difficult
- Forest regions: Wood abundant, varied plant materials

### Settlement System
- Terrain affects building placement
- Flat lowlands: Easy construction, large settlements
- Mountains: Difficult construction, defensive advantage
- Rivers/coasts: Trade bonuses, fishing access

### Farming System
- Soil quality based on biome
- Elevation affects crop types
- Water proximity affects irrigation needs

## Next Steps

1. **Download Test Data:** Start with a small region (single state/province)
2. **Set Up Pipeline:** Install tools and run test extraction
3. **Generate Test Terrain:** Create 1-2 test regions for prototype
4. **Integration Testing:** Import into game engine and test performance
5. **Iterate:** Refine based on visual quality and gameplay feel

## Technical Considerations

### Performance Optimization
- Use LOD (Level of Detail) for terrain rendering
- Stream terrain data in chunks
- Cache processed data to avoid re-processing
- Use spatial indexing for resource queries

### Data Storage
- Compressed heightmap storage: 1 MB per 1024x1024 region
- Resource locations: ~10 KB per region
- Biome data: 256 KB per region (if using texture)
- Total: ~2 MB per 10km x 10km region

### Scalability
- On-demand region generation for infinite world
- Player-explored regions are persistent
- Un-explored regions generated procedurally using OSM data as seed

## References

- **OpenStreetMap:** https://www.openstreetmap.org/
- **Geofabrik Downloads:** https://download.geofabrik.de/
- **OSM Wiki - Terrain:** https://wiki.openstreetmap.org/wiki/Terrain
- **Osmium Documentation:** https://osmcode.org/pyosmium/
- **GDAL Documentation:** https://gdal.org/

---

**Document Status:** Complete
**Last Updated:** 2025-01-15
**Priority:** CRITICAL
**Estimated Implementation Time:** 4 weeks (data processing) + ongoing (new regions)
