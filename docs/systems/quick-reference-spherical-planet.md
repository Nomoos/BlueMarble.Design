# Spherical Planet Generation - Quick Reference Guide

**Document Type:** Quick Reference  
**Version:** 1.0  
**Author:** Technical Documentation Team  
**Date:** 2024-12-29  
**Status:** Complete  
**Related Documents:** 
- [Feature Specification](spec-spherical-planet-generation.md)
- [Technical Implementation](tech-spherical-planet-implementation.md)
- [Developer Guide](developer-guide-spherical-planet-generation.md)

## Quick Start

### Generate a Basic Planet

```csharp
// Minimal configuration
var config = new PlanetaryConfig
{
    RadiusMeters = 6371000,  // Earth-like
    PlateCount = 12,
    OceanCoverage = 0.71,
    Seed = 42
};

var generator = new SphericalPlanetGenerator(config);
var polygons = generator.ExecuteProcess(
    new List<Polygon>(), 
    new List<Polygon>(), 
    new Random(42)
);

// Save result
SavePolygons.WriteToGeoPackage(polygons, "planet.gpkg", "surface");
```

### Apply Different Projections

```csharp
var projections = new MapProjections();

// Equirectangular (simple, no distortion)
var equirect = projections.ProjectEquirectangular(sphericalCoord);

// Mercator (preserves angles, good for navigation)
var mercator = projections.ProjectMercator(sphericalCoord);

// Robinson (balanced, good for world maps)
var robinson = projections.ProjectRobinson(sphericalCoord);
```

## Common Coordinate Systems

| System | SRID | Range X | Range Y | Use Case |
|--------|------|---------|---------|----------|
| Geographic | 4326 | -180° to 180° | -90° to 90° | Lat/Lon storage |
| SRID_METER | 4087 | 0 to 40,075,020m | 0 to 20,037,510m | BlueMarble standard |
| Web Mercator | 3857 | -20,037,508m to 20,037,508m | -20,048,966m to 20,048,966m | Web mapping |

## Projection Comparison

### Mercator
**Pros:** Preserves angles, straight rhumb lines  
**Cons:** Extreme polar distortion  
**Best for:** Navigation, local detail, web maps  

```csharp
var mercator = projections.ProjectMercator(coord);
// Automatically clamps latitude to ±85.0511°
```

### Equirectangular
**Pros:** Simple, reversible, equal spacing  
**Cons:** Area distortion at poles  
**Best for:** Simple visualization, data storage  

```csharp
var equirect = projections.ProjectEquirectangular(coord);
// Direct linear conversion: x = lon × R, y = lat × R
```

### Robinson
**Pros:** Balanced distortion, aesthetically pleasing  
**Cons:** Not equal-area or conformal  
**Best for:** World maps, atlases, presentations  

```csharp
var robinson = projections.ProjectRobinson(coord);
// Uses lookup tables for smooth curves
```

## Biome Classification Cheat Sheet

| Biome | Temperature (°C) | Precipitation (mm/yr) | Typical Latitude |
|-------|------------------|----------------------|------------------|
| Ocean | -2 to 30 | Any | Any |
| TropicalRainforest | 20-30 | >2000 | 0-15° |
| Desert | Any | <250 | 15-30° |
| Savanna | 15-30 | 500-1000 | 10-25° |
| TemperateForest | 5-20 | 750-1500 | 30-50° |
| BorealForest | -5 to 5 | 400-850 | 50-65° |
| Tundra | <0 | <400 | 60-75° |
| IceSheet | <-10 | Any | >75° |

## Common Issues and Solutions

### Issue: "Invalid polygon topology"
```csharp
// Solution: Use buffer(0) to fix
var fixed = invalidPolygon.Buffer(0.0);
if (fixed is Polygon validPoly)
{
    // Use validPoly
}
```

### Issue: "Polygon crosses date line"
```csharp
// Solution: Use DateLineSplitter
var splitter = new DateLineSplitter();
var splitPolygons = splitter.SplitPolygonAtDateLine(polygon);
```

### Issue: "Mercator projection at poles fails"
```csharp
// Solution: Clamp latitude
const double MAX_LAT = 85.0511287798;
var safeLat = Math.Max(-MAX_LAT, Math.Min(MAX_LAT, latitude));
```

### Issue: "Biome distribution unrealistic"
```csharp
// Check climate parameters
var climate = new ClimateParameters
{
    GlobalTemperature = 15.0,  // Should be 10-20°C
    TemperatureVariation = 40.0,  // Should be 30-50°C
    PrecipitationBase = 1000.0,  // Should be 800-1200mm
    SeasonalVariation = 0.2  // Should be 0.1-0.3
};
```

## Performance Tips

### Memory Management
```csharp
// Process in batches for large planets
const int BATCH_SIZE = 1000;
for (int i = 0; i < totalPlates; i += BATCH_SIZE)
{
    var batch = ProcessBatch(i, BATCH_SIZE);
    SaveBatch(batch);
    
    // Force GC for very large operations
    if (totalPlates > 10000)
    {
        GC.Collect();
        GC.WaitForPendingFinalizers();
    }
}
```

### Spatial Indexing
```csharp
// Use STRtree for neighbor queries
var index = new STRtree<Polygon>();
foreach (var poly in polygons)
{
    index.Insert(poly.EnvelopeInternal, poly);
}

// Fast spatial query
var neighbors = index.Query(targetEnvelope);
```

### Simplification
```csharp
// Reduce polygon complexity while preserving shape
var simplified = polygon.Buffer(tolerance).Buffer(-tolerance);
```

## Coordinate Conversion Formulas

### Spherical to SRID_METER
```csharp
double x = (longitude + 180.0) / 360.0 * WorldDetail.WorldSizeX;
double y = (latitude + 90.0) / 180.0 * WorldDetail.WorldSizeY;
```

### SRID_METER to Spherical
```csharp
double longitude = (x / WorldDetail.WorldSizeX) * 360.0 - 180.0;
double latitude = (y / WorldDetail.WorldSizeY) * 180.0 - 90.0;
```

### Haversine Distance
```csharp
double HaversineDistance(double lat1, double lon1, double lat2, double lon2)
{
    double R = 6371000; // Earth radius in meters
    double dLat = (lat2 - lat1) * Math.PI / 180.0;
    double dLon = (lon2 - lon1) * Math.PI / 180.0;
    
    double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
               Math.Cos(lat1 * Math.PI / 180.0) * Math.Cos(lat2 * Math.PI / 180.0) *
               Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
    
    double c = 2 * Math.Atan2(Math.Math.Sqrt(a), Math.Sqrt(1 - a));
    return R * c;
}
```

## API Endpoints Quick Reference

### Generate Planet
```http
POST /api/planet/generate
Content-Type: application/json

{
  "config": {
    "radiusMeters": 6371000,
    "plateCount": 12,
    "oceanCoverage": 0.71,
    "seed": 42
  },
  "options": {
    "generateBiomes": true,
    "applyProjection": "equirectangular"
  }
}
```

### Get Planet Data
```http
GET /api/planet/{planetId}/polygons?projection=robinson
```

### Apply Projection
```http
POST /api/planet/{planetId}/projection/{projectionType}
```

### List Projections
```http
GET /api/projections
```

## Testing Checklist

- [ ] Mathematical accuracy: coordinate conversions within 1m tolerance
- [ ] Biome distribution: matches Earth-like patterns (71% ocean)
- [ ] Performance: Earth-sized planet completes in <10 minutes
- [ ] Memory: Peak usage <4GB
- [ ] Date line handling: polygons split correctly
- [ ] Polar regions: no projection singularities
- [ ] Topology: all polygons valid (IsValid = true)
- [ ] SRID consistency: all geometries use SRID_METER (4087)

## Configuration Recommendations

### Small Test Planet
```csharp
new PlanetaryConfig
{
    RadiusMeters = 1000000,   // 1000 km
    PlateCount = 5,
    OceanCoverage = 0.50,
    Seed = 42
}
// ~30 seconds generation time
```

### Earth-like Planet
```csharp
new PlanetaryConfig
{
    RadiusMeters = 6371000,   // Earth radius
    PlateCount = 12,
    OceanCoverage = 0.71,
    Seed = 12345
}
// ~5-8 minutes generation time
```

### Large Detailed Planet
```csharp
new PlanetaryConfig
{
    RadiusMeters = 10000000,  // 10,000 km
    PlateCount = 30,
    OceanCoverage = 0.60,
    Seed = 99999
}
// ~15-20 minutes generation time
```

## Constants Reference

```csharp
// World dimensions (SRID_METER = 4087)
const double WorldSizeX = 40075020.0;  // Earth circumference at equator
const double WorldSizeY = 20037510.0;  // Half circumference (pole to pole)

// Earth properties
const double EarthRadiusMeters = 6371000.0;
const double EarthCircumference = 40075017.0;

// Projection limits
const double MercatorMaxLatitude = 85.0511287798;
const double StandardParallel = 0.0;  // For equirectangular

// SRID codes
const int SRID_GEOGRAPHIC = 4326;  // WGS84 lat/lon
const int SRID_METER = 4087;       // World Equidistant Cylindrical
const int SRID_WEB_MERCATOR = 3857; // Web Mercator
```

## Debug Utilities

### Dump Polygon Statistics
```csharp
public static void DumpPolygonStats(List<Polygon> polygons)
{
    var stats = new
    {
        Count = polygons.Count,
        TotalArea = polygons.Sum(p => p.Area),
        AverageArea = polygons.Average(p => p.Area),
        MinArea = polygons.Min(p => p.Area),
        MaxArea = polygons.Max(p => p.Area),
        ValidCount = polygons.Count(p => p.IsValid),
        InvalidCount = polygons.Count(p => !p.IsValid),
        BiomeDistribution = polygons
            .GroupBy(p => GetBiomeType(p))
            .ToDictionary(g => g.Key, g => new
            {
                Count = g.Count(),
                AreaPercent = g.Sum(p => p.Area) / polygons.Sum(p => p.Area) * 100
            })
    };
    
    Console.WriteLine(JsonSerializer.Serialize(stats, new JsonSerializerOptions 
    { 
        WriteIndented = true 
    }));
}
```

### Visualize Coordinate Ranges
```csharp
public static void VisualizeCoordinateRanges(List<Polygon> polygons)
{
    var minX = polygons.Min(p => p.EnvelopeInternal.MinX);
    var maxX = polygons.Max(p => p.EnvelopeInternal.MaxX);
    var minY = polygons.Min(p => p.EnvelopeInternal.MinY);
    var maxY = polygons.Max(p => p.EnvelopeInternal.MaxY);
    
    Console.WriteLine($"X Range: {minX:F2} to {maxX:F2} (span: {maxX - minX:F2})");
    Console.WriteLine($"Y Range: {minY:F2} to {maxY:F2} (span: {maxY - minY:F2})");
    Console.WriteLine($"Coverage: {(maxX - minX) / WorldDetail.WorldSizeX:P1} × " +
                     $"{(maxY - minY) / WorldDetail.WorldSizeY:P1}");
}
```

## Related Documentation

- **[Complete Feature Specification](spec-spherical-planet-generation.md)** - Full requirements and design
- **[Technical Implementation Guide](tech-spherical-planet-implementation.md)** - Detailed implementation
- **[Developer Guide](developer-guide-spherical-planet-generation.md)** - Step-by-step tutorials
- **[API Specification](api-spherical-planet-generation.md)** - REST API documentation
- **[Testing Strategy](testing-spherical-planet-generation.md)** - Test plans and coverage
- **[QA Test Plan](qa-test-plan-spherical-planet.md)** - Quality assurance procedures

## Support and Troubleshooting

### Common Error Messages

| Error | Cause | Solution |
|-------|-------|----------|
| `TopologyException` | Self-intersecting polygon | Use `.Buffer(0.0)` to fix |
| `ArgumentOutOfRangeException` | Invalid latitude/longitude | Validate input ranges |
| `OutOfMemoryException` | Too many plates | Reduce plate count or use batching |
| `InvalidOperationException` | SRID mismatch | Ensure all geometries use SRID_METER |

### Performance Benchmarks

| Planet Size | Plate Count | Generation Time | Peak Memory |
|-------------|-------------|-----------------|-------------|
| Small (1000km) | 5 | ~30 seconds | ~500MB |
| Medium (3000km) | 10 | ~2 minutes | ~1.5GB |
| Earth (6371km) | 12 | ~6 minutes | ~3GB |
| Large (10000km) | 20 | ~12 minutes | ~5GB |

### Getting Help

1. Check this quick reference first
2. Review the [Developer Guide](developer-guide-spherical-planet-generation.md)
3. Search existing issues in the repository
4. Create a new issue with detailed error logs and configuration

---

**Last Updated:** 2024-12-29  
**Maintained By:** Technical Documentation Team  
**Feedback:** Submit issues or pull requests to improve this guide
