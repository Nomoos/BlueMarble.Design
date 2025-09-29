# Spherical Planet Generation - Developer Guide

**Document Type:** Developer Guide  
**Version:** 1.0  
**Author:** Technical Documentation Team  
**Date:** 2024-12-29  
**Status:** Draft  
**Related Documents:** All Spherical Planet Generation specifications

## Quick Start Guide

### Prerequisites

Before implementing the Spherical Planet Generation system, ensure you have:

- **.NET 8.0+** for backend implementation
- **NetTopologySuite 2.5+** for spatial operations
- **GDAL/OGR** for geographic data processing
- **JavaScript ES2020+** for frontend components
- **Understanding of SRID_METER (4087)** coordinate system
- **Familiarity with BlueMarble's GeomorphologicalProcess architecture**

### Installation and Setup

1. **Clone the relevant repositories:**
   ```bash
   git clone https://github.com/Nomoos/BlueMarble.Core
   git clone https://github.com/Nomoos/BlueMarble.Client
   git clone https://github.com/Nomoos/BlueMarble.Server
   ```

2. **Install backend dependencies:**
   ```bash
   dotnet add package NetTopologySuite
   dotnet add package GDAL
   dotnet add package Microsoft.Extensions.DependencyInjection
   ```

3. **Install frontend dependencies:**
   ```bash
   npm install leaflet proj4 d3-geo
   ```

### 5-Minute Implementation Example

Here's a minimal example to get started:

```csharp
// Backend: Generate a simple planet
var config = new PlanetaryConfig
{
    RadiusMeters = 6371000,
    PlateCount = 8,
    OceanCoverage = 0.71,
    Seed = 42
};

var generator = new SphericalPlanetGenerator(config);
var polygons = generator.ExecuteProcess(
    new List<Polygon>(), 
    new List<Polygon>(), 
    new Random(42)
);

// Save to GeoPackage
SavePolygons.WriteToGeoPackage(polygons, "my_planet.gpkg", "surface");
```

```javascript
// Frontend: Display the planet
import { SphericalPlanetViewer } from './spherical-planet-viewer.js';

const viewer = new SphericalPlanetViewer('map-container', 'equirectangular');
await viewer.loadPlanetData('/api/planet/my-planet-id');
viewer.render();
```

## Architecture Overview

### System Components

```
┌─────────────────────────────────────────────────────────┐
│                    Frontend Layer                       │
├─────────────────────────────────────────────────────────┤
│ SphericalPlanetViewer │ ProjectionControls │ BiomeLegend │
│ CoordinateConverter   │ PlanetGenerator    │ DataLoader  │
└─────────────────────────────────────────────────────────┘
                              │
                              ▼
┌─────────────────────────────────────────────────────────┐
│                     API Layer                           │
├─────────────────────────────────────────────────────────┤
│ Planet Generation API │ Projection API │ Biome API      │
│ Data Export API       │ Validation API │ Status API     │
└─────────────────────────────────────────────────────────┘
                              │
                              ▼
┌─────────────────────────────────────────────────────────┐
│                   Backend Services                      │
├─────────────────────────────────────────────────────────┤
│ SphericalPlanetGenerator │ BiomeClassifier │ MapProjections │
│ SeamlessWorldWrapper    │ GeometryProcessor│ DataManager   │
└─────────────────────────────────────────────────────────┘
                              │
                              ▼
┌─────────────────────────────────────────────────────────┐
│                Infrastructure Layer                     │
├─────────────────────────────────────────────────────────┤
│ NetTopologySuite │ GDAL/OGR │ GeoPackage │ Spatial Index │
└─────────────────────────────────────────────────────────┘
```

### Integration Points

1. **GeomorphologicalProcess Integration**: Extends existing geological simulation pipeline
2. **Coordinate System Compatibility**: Uses established SRID_METER (4087) standard
3. **Data Persistence**: Integrates with existing GeoPackage infrastructure
4. **Frontend Compatibility**: Works with existing coordinate conversion utilities

## Implementation Walkthrough

### Phase 1: Core Mathematical Foundations

#### Step 1: Implement Spherical Coordinate System

```csharp
public class SphericalCoordinate
{
    public double Latitude { get; set; }  // Degrees: -90 to +90
    public double Longitude { get; set; } // Degrees: -180 to +180
    public double Radius { get; set; }    // Meters: default 6371000
    
    public CartesianCoordinate ToCartesian()
    {
        var latRad = Latitude * Math.PI / 180.0;
        var lonRad = Longitude * Math.PI / 180.0;
        
        return new CartesianCoordinate
        {
            X = Radius * Math.Cos(latRad) * Math.Cos(lonRad),
            Y = Radius * Math.Cos(latRad) * Math.Sin(lonRad),
            Z = Radius * Math.Sin(latRad)
        };
    }
}
```

#### Step 2: Implement Basic Projections

```csharp
public class MapProjections
{
    public Point2D ProjectEquirectangular(SphericalCoordinate coord)
    {
        // Simple cylindrical projection
        var x = coord.Longitude * Math.PI / 180.0 * WorldDetail.EarthRadiusMeters;
        var y = coord.Latitude * Math.PI / 180.0 * WorldDetail.EarthRadiusMeters;
        return new Point2D { X = x, Y = y };
    }
    
    public Point2D ProjectMercator(SphericalCoordinate coord)
    {
        // Mercator projection with pole handling
        var lonRad = coord.Longitude * Math.PI / 180.0;
        var latRad = Math.Max(-85.0, Math.Min(85.0, coord.Latitude)) * Math.PI / 180.0;
        
        var x = WorldDetail.EarthRadiusMeters * lonRad;
        var y = WorldDetail.EarthRadiusMeters * Math.Log(Math.Tan(Math.PI / 4.0 + latRad / 2.0));
        
        return new Point2D { X = x, Y = y };
    }
}
```

#### Step 3: Test Mathematical Accuracy

```csharp
[TestMethod]
public void RoundTripConversion_KnownCoordinates_MaintainsAccuracy()
{
    var original = new SphericalCoordinate { Lat = 45.0, Lon = -90.0, Radius = 6371000 };
    var cartesian = original.ToCartesian();
    var converted = cartesian.ToSpherical();
    
    Assert.AreEqual(original.Latitude, converted.Latitude, 0.001);
    Assert.AreEqual(original.Longitude, converted.Longitude, 0.001);
}
```

### Phase 2: Planet Generation Engine

#### Step 1: Implement Voronoi Distribution

```csharp
public class SphericalVoronoiGenerator
{
    public List<SphericalCoordinate> GenerateUniformDistribution(int pointCount, Random random)
    {
        var points = new List<SphericalCoordinate>();
        
        for (int i = 0; i < pointCount; i++)
        {
            // Use rejection sampling for uniform distribution on sphere
            double u = random.NextDouble();
            double v = random.NextDouble();
            
            double theta = 2.0 * Math.PI * u; // Longitude
            double phi = Math.Acos(2.0 * v - 1.0); // Latitude from pole
            
            points.Add(new SphericalCoordinate
            {
                Longitude = theta * 180.0 / Math.PI - 180.0,
                Latitude = 90.0 - phi * 180.0 / Math.PI,
                Radius = 6371000
            });
        }
        
        return points;
    }
}
```

#### Step 2: Implement Tectonic Simulation

```csharp
public class TectonicSimulator
{
    public List<Polygon> ApplyTectonicForces(List<Polygon> plates, TectonicParameters parameters)
    {
        var processedPlates = new List<Polygon>();
        
        foreach (var plate in plates)
        {
            // Determine neighboring plates
            var neighbors = FindNeighboringPlates(plate, plates);
            
            // Apply compression/extension based on relative motion
            var forces = CalculateTectonicForces(plate, neighbors, parameters);
            
            // Modify plate boundaries
            var modifiedPlate = ApplyForces(plate, forces);
            
            // Generate mountain ranges at convergent boundaries
            if (forces.Any(f => f.Type == ForceType.Compression))
            {
                modifiedPlate = GenerateMountainRanges(modifiedPlate, forces);
            }
            
            processedPlates.Add(modifiedPlate);
        }
        
        return processedPlates;
    }
}
```

#### Step 3: Integrate with GeomorphologicalProcess

```csharp
public class SphericalPlanetGenerator : GeomorphologicalProcess
{
    public override List<Polygon> ExecuteProcess(
        List<Polygon> inputPolygons,
        List<Polygon> neighborPolygons,
        Random randomSource)
    {
        // 1. Generate initial spherical distribution
        var voronoiSeeds = _voronoiGenerator.GenerateUniformDistribution(_config.PlateCount, randomSource);
        
        // 2. Create continental plates
        var initialPlates = CreateContinentalPlates(voronoiSeeds);
        
        // 3. Apply tectonic processes
        var tectonicPlates = _tectonicSimulator.ApplyTectonicForces(initialPlates, _config.TectonicParameters);
        
        // 4. Classify biomes
        var biomePolygons = _biomeClassifier.ClassifyBiomes(tectonicPlates, _config.Climate);
        
        // 5. Project to planar coordinates (SRID_METER = 4087)
        var projectedPolygons = _mapProjections.ProjectToSRID4087(biomePolygons);
        
        // 6. Apply world wrapping
        var wrappedPolygons = _worldWrapper.EnsureWorldWrapping(projectedPolygons);
        
        return wrappedPolygons;
    }
}
```

### Phase 3: Biome Classification System

#### Step 1: Implement Climate Model

```csharp
public class ClimateModel
{
    public double CalculateTemperature(SphericalCoordinate location, ClimateParameters climate)
    {
        var latitude = Math.Abs(location.Latitude);
        var elevation = GetElevation(location);
        
        // Base temperature decreases with latitude
        var latitudeEffect = climate.GlobalTemperature - (latitude / 90.0) * climate.TemperatureVariation;
        
        // Temperature decreases with elevation (lapse rate ~6.5°C/km)
        var elevationEffect = -6.5 * (elevation / 1000.0);
        
        // Seasonal variation (simplified)
        var seasonalEffect = climate.SeasonalVariation * Math.Sin(2 * Math.PI * GetDayOfYear() / 365.0);
        
        return latitudeEffect + elevationEffect + seasonalEffect;
    }
    
    public double CalculatePrecipitation(SphericalCoordinate location, ClimateParameters climate)
    {
        var latitude = Math.Abs(location.Latitude);
        var temperature = CalculateTemperature(location, climate);
        
        // Precipitation patterns based on latitude zones
        double precipitationFactor;
        if (latitude < 30) // Tropical zones
        {
            precipitationFactor = 1.5 + 0.5 * Math.Sin(latitude * Math.PI / 30.0);
        }
        else if (latitude < 60) // Temperate zones
        {
            precipitationFactor = 1.0;
        }
        else // Arctic zones
        {
            precipitationFactor = 0.3;
        }
        
        // Temperature affects precipitation capacity
        var temperatureEffect = Math.Max(0.1, temperature / 30.0);
        
        return climate.PrecipitationBase * precipitationFactor * temperatureEffect;
    }
}
```

#### Step 2: Implement Biome Decision Tree

```csharp
public class BiomeClassifier
{
    public BiomeType DetermineBiome(double temperature, double precipitation, double elevation, double latitude)
    {
        // Ice sheet conditions
        if (temperature < -10 || elevation > 5000)
            return BiomeType.IceSheet;
        
        // Ocean conditions
        if (elevation < 0)
            return BiomeType.Ocean;
        
        // Desert conditions (low precipitation)
        if (precipitation < 250)
            return BiomeType.Desert;
        
        // Forest classifications
        if (precipitation > 1000)
        {
            if (temperature > 20)
                return BiomeType.TropicalRainforest;
            else if (temperature > 10)
                return BiomeType.TemperateForest;
            else if (temperature > 0)
                return BiomeType.BorealForest;
        }
        
        // Grassland and savanna
        if (precipitation > 500)
        {
            if (Math.Abs(latitude) < 30 && temperature > 15)
                return BiomeType.TropicalSavanna;
            else
                return BiomeType.Grassland;
        }
        
        // Tundra
        if (temperature < 5)
            return BiomeType.Tundra;
        
        // Default temperate conditions
        return BiomeType.TemperateForest;
    }
}
```

### Phase 4: Frontend Integration

#### Step 1: Create Planet Viewer Component

```javascript
export class SphericalPlanetViewer {
    constructor(containerId, projectionType = 'equirectangular') {
        this.container = document.getElementById(containerId);
        this.projection = projectionType;
        this.canvas = this.createCanvas();
        this.ctx = this.canvas.getContext('2d');
        this.data = null;
        
        this.setupEventListeners();
    }
    
    async loadPlanetData(planetId) {
        try {
            const response = await fetch(`/api/planet/${planetId}/polygons?projection=${this.projection}`);
            this.data = await response.json();
            this.render();
        } catch (error) {
            console.error('Failed to load planet data:', error);
        }
    }
    
    render() {
        if (!this.data) return;
        
        this.ctx.clearRect(0, 0, this.canvas.width, this.canvas.height);
        
        for (const feature of this.data.features) {
            this.drawPolygon(feature);
        }
    }
    
    drawPolygon(feature) {
        const coords = feature.geometry.coordinates[0];
        const biomeColor = feature.properties.color;
        
        this.ctx.fillStyle = biomeColor;
        this.ctx.strokeStyle = '#333';
        this.ctx.lineWidth = 0.5;
        
        this.ctx.beginPath();
        const [firstX, firstY] = this.screenCoordinates(coords[0]);
        this.ctx.moveTo(firstX, firstY);
        
        for (let i = 1; i < coords.length; i++) {
            const [x, y] = this.screenCoordinates(coords[i]);
            this.ctx.lineTo(x, y);
        }
        
        this.ctx.closePath();
        this.ctx.fill();
        this.ctx.stroke();
    }
    
    screenCoordinates([lon, lat]) {
        // Convert projected coordinates to screen coordinates
        const x = (lon - this.bounds.minX) / (this.bounds.maxX - this.bounds.minX) * this.canvas.width;
        const y = (this.bounds.maxY - lat) / (this.bounds.maxY - this.bounds.minY) * this.canvas.height;
        return [x, y];
    }
}
```

#### Step 2: Add Projection Controls

```javascript
export class ProjectionControlPanel {
    constructor(viewerId) {
        this.viewer = document.getElementById(viewerId);
        this.createControls();
    }
    
    createControls() {
        const panel = document.createElement('div');
        panel.className = 'projection-controls';
        
        const select = document.createElement('select');
        select.innerHTML = `
            <option value="equirectangular">Equirectangular</option>
            <option value="mercator">Mercator</option>
            <option value="robinson">Robinson</option>
            <option value="mollweide">Mollweide</option>
        `;
        
        select.addEventListener('change', (e) => {
            this.changeProjection(e.target.value);
        });
        
        panel.appendChild(select);
        this.viewer.appendChild(panel);
    }
    
    async changeProjection(projectionType) {
        // Reload data with new projection
        const planetId = this.viewer.dataset.planetId;
        const response = await fetch(`/api/planet/${planetId}/projection/${projectionType}`, {
            method: 'POST'
        });
        
        if (response.ok) {
            await this.viewer.loadPlanetData(planetId);
        }
    }
}
```

## Common Pitfalls and Solutions

### 1. Coordinate System Confusion

**Problem:** Mixing up different coordinate systems leads to incorrect spatial operations.

**Solution:**
```csharp
// Always validate coordinate system
public static void ValidateCoordinateSystem(Geometry geometry)
{
    if (geometry.SRID != WorldDetail.SRID_METER)
    {
        throw new InvalidOperationException(
            $"Expected SRID {WorldDetail.SRID_METER}, got {geometry.SRID}");
    }
}

// Use consistent conversion utilities
public static class CoordinateUtils
{
    public static Point ConvertToSRID4087(SphericalCoordinate spherical)
    {
        var projected = ProjectToEquidistantCylindrical(spherical);
        var factory = new GeometryFactory(new PrecisionModel(), WorldDetail.SRID_METER);
        return factory.CreatePoint(new Coordinate(projected.X, projected.Y));
    }
}
```

### 2. Pole Handling in Projections

**Problem:** Singularities at poles can cause infinite values or projection failures.

**Solution:**
```csharp
public Point2D ProjectMercatorSafe(SphericalCoordinate coord)
{
    // Clamp latitude to avoid pole singularities
    var clampedLat = Math.Max(-85.0511, Math.Min(85.0511, coord.Latitude));
    
    var lonRad = coord.Longitude * Math.PI / 180.0;
    var latRad = clampedLat * Math.PI / 180.0;
    
    var x = WorldDetail.EarthRadiusMeters * lonRad;
    var y = WorldDetail.EarthRadiusMeters * Math.Log(Math.Tan(Math.PI / 4.0 + latRad / 2.0));
    
    return new Point2D { X = x, Y = y };
}
```

### 3. Date Line Crossing

**Problem:** Polygons crossing the international date line can have invalid topology.

**Solution:**
```csharp
public List<Polygon> HandleDateLineCrossing(Polygon polygon)
{
    var coords = polygon.ExteriorRing.Coordinates;
    var lonitudes = coords.Select(c => c.X).ToArray();
    
    // Check for date line crossing (large longitude jumps)
    var maxLonDiff = lonitudes.Zip(lonitudes.Skip(1), (a, b) => Math.Abs(b - a)).Max();
    
    if (maxLonDiff > 180 * Math.PI / 180 * WorldDetail.EarthRadiusMeters)
    {
        return SplitAtDateLine(polygon);
    }
    
    return new List<Polygon> { polygon };
}
```

### 4. Performance with Large Datasets

**Problem:** Generating planets with many polygons can exceed memory limits.

**Solution:**
```csharp
public class StreamingPlanetGenerator : SphericalPlanetGenerator
{
    public IEnumerable<Polygon> GeneratePolygonsStreaming(PlanetaryConfig config)
    {
        const int batchSize = 1000;
        var totalPlates = config.PlateCount;
        
        for (int offset = 0; offset < totalPlates; offset += batchSize)
        {
            var batchSize = Math.Min(batchSize, totalPlates - offset);
            var batch = GeneratePlateBatch(offset, batchSize, config);
            
            foreach (var polygon in batch)
            {
                yield return polygon;
            }
            
            // Allow garbage collection between batches
            GC.Collect();
        }
    }
}
```

## Performance Optimization Tips

### Backend Optimizations

1. **Use Spatial Indexing:**
```csharp
var spatialIndex = new STRtree<Polygon>();
foreach (var polygon in polygons)
{
    spatialIndex.Insert(polygon.EnvelopeInternal, polygon);
}

// Fast neighbor queries
var neighbors = spatialIndex.Query(targetPolygon.EnvelopeInternal.ExpandedBy(1000));
```

2. **Optimize Geometry Operations:**
```csharp
// Use buffering instead of complex geometric operations
var simplified = complexPolygon.Buffer(0.0); // Fixes topology issues
var generalized = complexPolygon.Buffer(tolerance).Buffer(-tolerance); // Simplifies shape
```

3. **Batch Database Operations:**
```csharp
public async Task SavePolygonsBatch(IEnumerable<Polygon> polygons, string tableName)
{
    const int batchSize = 1000;
    var batches = polygons.Batch(batchSize);
    
    foreach (var batch in batches)
    {
        await SavePolygonBatchInternal(batch, tableName);
    }
}
```

### Frontend Optimizations

1. **Use Canvas for Large Datasets:**
```javascript
// More efficient than SVG for many polygons
const canvas = document.createElement('canvas');
const ctx = canvas.getContext('2d');

// Implement viewport culling
function renderVisiblePolygons(viewport) {
    const visible = polygons.filter(p => intersects(p.bounds, viewport));
    visible.forEach(p => drawPolygon(ctx, p));
}
```

2. **Implement Level of Detail:**
```javascript
function getSimplificationLevel(zoomLevel) {
    if (zoomLevel < 3) return 1000; // 1km tolerance
    if (zoomLevel < 6) return 100;  // 100m tolerance
    return 10; // 10m tolerance
}
```

3. **Use Web Workers for Processing:**
```javascript
// projection-worker.js
self.onmessage = function(e) {
    const { coordinates, projectionType } = e.data;
    const projected = applyProjection(coordinates, projectionType);
    self.postMessage(projected);
};

// Main thread
const worker = new Worker('projection-worker.js');
worker.postMessage({ coordinates, projectionType: 'robinson' });
```

## Testing and Validation

### Required Test Coverage

1. **Unit Tests** (>95% coverage):
   - Mathematical functions (projections, distance calculations)
   - Biome classification logic
   - Coordinate conversions
   - Geometry operations

2. **Integration Tests** (100% API coverage):
   - Full planet generation pipeline
   - API endpoint validation
   - Database persistence
   - Frontend-backend communication

3. **Performance Tests**:
   - Memory usage validation
   - Generation time benchmarks
   - Load testing for concurrent requests

### Example Test Implementation

```csharp
[TestMethod]
public void SphericalPlanetGeneration_EarthLikeParameters_RealisticResults()
{
    // Arrange
    var config = new PlanetaryConfig
    {
        RadiusMeters = 6371000,
        PlateCount = 7,
        OceanCoverage = 0.712,
        Seed = 42
    };
    
    var generator = new SphericalPlanetGenerator(config);
    var stopwatch = Stopwatch.StartNew();
    
    // Act
    var result = generator.ExecuteProcess(new List<Polygon>(), new List<Polygon>(), new Random(42));
    stopwatch.Stop();
    
    // Assert
    Assert.IsTrue(result.Count > 0, "Should generate polygons");
    Assert.IsTrue(result.All(p => p.IsValid), "All polygons should be valid");
    Assert.IsTrue(stopwatch.Elapsed < TimeSpan.FromMinutes(10), "Should complete within 10 minutes");
    
    // Validate biome distribution
    var biomes = result.GroupBy(p => GetBiomeType(p)).ToDictionary(g => g.Key, g => g.Sum(p => p.Area));
    var totalArea = biomes.Values.Sum();
    var oceanRatio = biomes.GetValueOrDefault(BiomeType.Ocean, 0) / totalArea;
    
    Assert.IsTrue(oceanRatio > 0.6 && oceanRatio < 0.8, $"Ocean coverage should be ~71%, got {oceanRatio:P1}");
}
```

## Deployment Considerations

### Configuration Management

```json
{
  "SphericalPlanetGeneration": {
    "DefaultConfig": {
      "RadiusMeters": 6371000,
      "MaxPlateCount": 50,
      "MinPlateCount": 3,
      "DefaultProjection": "equirectangular"
    },
    "Performance": {
      "MaxConcurrentGenerations": 5,
      "BatchSize": 1000,
      "TimeoutMinutes": 15,
      "MaxMemoryMB": 4096
    },
    "Caching": {
      "EnableResultCaching": true,
      "CacheExpiryHours": 24,
      "MaxCacheSize": "10GB"
    }
  }
}
```

### Monitoring and Alerts

```csharp
public class PlanetGenerationMetrics
{
    private static readonly Counter GenerationRequests = Metrics
        .CreateCounter("planet_generation_requests_total", "Total planet generation requests");
    
    private static readonly Histogram GenerationDuration = Metrics
        .CreateHistogram("planet_generation_duration_seconds", "Planet generation duration");
    
    private static readonly Gauge ActiveGenerations = Metrics
        .CreateGauge("planet_generation_active", "Currently active planet generations");
    
    public static void RecordGenerationStart()
    {
        GenerationRequests.Inc();
        ActiveGenerations.Inc();
    }
    
    public static void RecordGenerationComplete(TimeSpan duration)
    {
        GenerationDuration.Observe(duration.TotalSeconds);
        ActiveGenerations.Dec();
    }
}
```

### Health Checks

```csharp
public class PlanetGenerationHealthCheck : IHealthCheck
{
    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            // Quick generation test
            var testConfig = new PlanetaryConfig { RadiusMeters = 1000000, PlateCount = 3 };
            var generator = new SphericalPlanetGenerator(testConfig);
            
            var stopwatch = Stopwatch.StartNew();
            var result = generator.ExecuteProcess(new List<Polygon>(), new List<Polygon>(), new Random());
            stopwatch.Stop();
            
            if (result.Count > 0 && stopwatch.Elapsed < TimeSpan.FromSeconds(30))
            {
                return HealthCheckResult.Healthy("Planet generation system operational");
            }
            else
            {
                return HealthCheckResult.Degraded("Planet generation system slow or producing no results");
            }
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy("Planet generation system failed", ex);
        }
    }
}
```

## Troubleshooting Guide

### Common Issues and Solutions

1. **"Invalid polygon topology" errors:**
   - Use `polygon.Buffer(0.0)` to fix self-intersections
   - Validate coordinates before creating polygons
   - Check for duplicate consecutive points

2. **"Projection singularity" errors:**
   - Clamp latitude values for Mercator projection
   - Handle pole regions separately
   - Use appropriate projection for the region

3. **"Memory out of bounds" errors:**
   - Implement streaming processing
   - Reduce plate count for testing
   - Use geometry simplification

4. **"Biome distribution unrealistic" warnings:**
   - Review climate parameters
   - Check elevation data
   - Validate biome classification thresholds

### Debug Tools

```csharp
public static class PlanetGenerationDebugger
{
    public static void DumpGenerationStatistics(List<Polygon> polygons, string filename)
    {
        var stats = new
        {
            TotalPolygons = polygons.Count,
            TotalArea = polygons.Sum(p => p.Area),
            AverageArea = polygons.Average(p => p.Area),
            BiomeDistribution = polygons
                .GroupBy(p => GetBiomeType(p))
                .ToDictionary(g => g.Key.ToString(), g => new {
                    Count = g.Count(),
                    Area = g.Sum(p => p.Area),
                    Percentage = g.Sum(p => p.Area) / polygons.Sum(p => p.Area) * 100
                }),
            CoordinateRanges = new {
                MinX = polygons.Min(p => p.EnvelopeInternal.MinX),
                MaxX = polygons.Max(p => p.EnvelopeInternal.MaxX),
                MinY = polygons.Min(p => p.EnvelopeInternal.MinY),
                MaxY = polygons.Max(p => p.EnvelopeInternal.MaxY)
            }
        };
        
        File.WriteAllText(filename, JsonSerializer.Serialize(stats, new JsonSerializerOptions { WriteIndented = true }));
    }
}
```

This developer guide provides practical, actionable guidance for implementing the Spherical Planet Generation system while avoiding common pitfalls and ensuring good performance.