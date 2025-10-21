# Spherical Planet Generation - Troubleshooting and Performance Guide

**Document Type:** Troubleshooting Guide  
**Version:** 1.0  
**Author:** Technical Support Team  
**Date:** 2024-12-29  
**Status:** Complete  
**Related Documents:** 
- [Developer Guide](developer-guide-spherical-planet-generation.md)
- [Quick Reference](quick-reference-spherical-planet.md)
- [API Specification](api-spherical-planet-generation.md)

## Overview

This guide provides comprehensive troubleshooting steps and performance optimization techniques for the Spherical Planet Generation system. Use this guide to diagnose and resolve common issues, and to optimize planet generation performance.

## Common Issues and Solutions

### Issue 1: Generation Takes Extremely Long Time

**Symptoms:**
- Planet generation exceeds 20 minutes
- Progress appears stuck at specific stages
- System becomes unresponsive

**Possible Causes:**
1. Too many plates configured
2. Insufficient system resources
3. Complex climate model calculations
4. Memory swapping due to insufficient RAM

**Diagnosis Steps:**

```csharp
// Check configuration
public void DiagnosePerformanceIssues(PlanetaryConfig config)
{
    Console.WriteLine("Performance Diagnostics:");
    Console.WriteLine($"  Plate Count: {config.PlateCount}");
    
    if (config.PlateCount > 20)
    {
        Console.WriteLine("  ⚠ WARNING: High plate count may cause slow generation");
    }
    
    // Estimate memory requirements
    var estimatedMemoryMB = config.PlateCount * 150; // Rough estimate
    Console.WriteLine($"  Estimated Memory: ~{estimatedMemoryMB} MB");
    
    var availableMemory = GetAvailableMemory();
    if (estimatedMemoryMB > availableMemory * 0.8)
    {
        Console.WriteLine("  ⚠ WARNING: May exceed available memory");
    }
}
```

**Solutions:**

1. **Reduce Plate Count:**
```csharp
// Instead of:
var config = new PlanetaryConfig { PlateCount = 30 };

// Use:
var config = new PlanetaryConfig { PlateCount = 12 }; // Sweet spot for Earth-like
```

2. **Enable Batch Processing:**
```csharp
public class OptimizedPlanetGenerator : SphericalPlanetGenerator
{
    private const int BATCH_SIZE = 1000;
    
    public override List<Polygon> ExecuteProcess(
        List<Polygon> inputPolygons,
        List<Polygon> neighborPolygons,
        Random randomSource)
    {
        var results = new List<Polygon>();
        
        for (int batch = 0; batch < _config.PlateCount; batch += BATCH_SIZE)
        {
            var batchSize = Math.Min(BATCH_SIZE, _config.PlateCount - batch);
            var batchResults = ProcessBatch(batch, batchSize, randomSource);
            results.AddRange(batchResults);
            
            // Force GC between batches
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
        
        return results;
    }
}
```

3. **Use Progress Monitoring:**
```csharp
public class ProgressMonitoredGenerator
{
    public async Task<List<Polygon>> GenerateWithMonitoringAsync(
        PlanetaryConfig config,
        IProgress<GenerationProgress> progress)
    {
        var generator = new SphericalPlanetGenerator(config);
        
        return await Task.Run(() =>
        {
            var stopwatch = Stopwatch.StartNew();
            
            progress.Report(new GenerationProgress 
            { 
                Stage = "Initialization",
                Percentage = 0 
            });
            
            var result = generator.ExecuteProcess(
                new List<Polygon>(),
                new List<Polygon>(),
                new Random(config.Seed)
            );
            
            stopwatch.Stop();
            
            progress.Report(new GenerationProgress 
            { 
                Stage = "Completed",
                Percentage = 100,
                Duration = stopwatch.Elapsed
            });
            
            return result;
        });
    }
}
```

### Issue 2: Invalid Polygon Topology Errors

**Symptoms:**
- Polygons with `IsValid = false`
- Self-intersecting geometries
- Rendering artifacts in visualization
- Spatial query failures

**Diagnosis:**

```csharp
public class TopologyValidator
{
    public TopologyDiagnostics DiagnoseTopology(List<Polygon> polygons)
    {
        var invalidPolygons = new List<(int Index, Polygon Polygon, string Reason)>();
        
        for (int i = 0; i < polygons.Count; i++)
        {
            var polygon = polygons[i];
            
            if (!polygon.IsValid)
            {
                var reason = DetermineInvalidityReason(polygon);
                invalidPolygons.Add((i, polygon, reason));
            }
        }
        
        return new TopologyDiagnostics
        {
            TotalPolygons = polygons.Count,
            InvalidPolygons = invalidPolygons,
            ValidationRate = 1.0 - ((double)invalidPolygons.Count / polygons.Count)
        };
    }
    
    private string DetermineInvalidityReason(Polygon polygon)
    {
        if (polygon.IsEmpty)
            return "Empty polygon";
        
        var coords = polygon.ExteriorRing.Coordinates;
        
        if (coords.Length < 4)
            return "Insufficient coordinates";
        
        if (!coords[0].Equals2D(coords[coords.Length - 1]))
            return "Ring not closed";
        
        // Check for self-intersection
        var ring = polygon.ExteriorRing;
        for (int i = 0; i < ring.Coordinates.Length - 1; i++)
        {
            var segment1 = new LineSegment(
                ring.Coordinates[i],
                ring.Coordinates[i + 1]
            );
            
            for (int j = i + 2; j < ring.Coordinates.Length - 1; j++)
            {
                var segment2 = new LineSegment(
                    ring.Coordinates[j],
                    ring.Coordinates[j + 1]
                );
                
                if (segment1.Intersection(segment2) != null)
                    return "Self-intersecting ring";
            }
        }
        
        return "Unknown topology issue";
    }
}
```

**Solutions:**

1. **Automatic Topology Repair:**
```csharp
public static class TopologyRepair
{
    public static List<Polygon> RepairAll(List<Polygon> polygons)
    {
        var repaired = new List<Polygon>();
        
        foreach (var polygon in polygons)
        {
            if (!polygon.IsValid)
            {
                var fixed = RepairPolygon(polygon);
                if (fixed != null && fixed.IsValid)
                {
                    repaired.Add(fixed);
                }
                else
                {
                    Console.WriteLine($"Failed to repair polygon, skipping");
                }
            }
            else
            {
                repaired.Add(polygon);
            }
        }
        
        return repaired;
    }
    
    public static Polygon RepairPolygon(Polygon polygon)
    {
        try
        {
            // Method 1: Buffer with zero distance
            var buffered = polygon.Buffer(0.0);
            if (buffered is Polygon validPolygon && validPolygon.IsValid)
            {
                GeometryUtils.CopyMetadata(polygon, validPolygon);
                return validPolygon;
            }
            
            // Method 2: Simplify with small tolerance
            var simplified = polygon.Buffer(1.0).Buffer(-1.0);
            if (simplified is Polygon simplifiedPolygon && simplifiedPolygon.IsValid)
            {
                GeometryUtils.CopyMetadata(polygon, simplifiedPolygon);
                return simplifiedPolygon;
            }
            
            // Method 3: Rebuild from envelope
            var envelope = polygon.EnvelopeInternal;
            var factory = new GeometryFactory(
                new PrecisionModel(),
                polygon.SRID
            );
            
            var envelopePolygon = factory.ToGeometry(envelope) as Polygon;
            if (envelopePolygon != null && envelopePolygon.IsValid)
            {
                GeometryUtils.CopyMetadata(polygon, envelopePolygon);
                return envelopePolygon;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Repair failed: {ex.Message}");
        }
        
        return null;
    }
}
```

### Issue 3: Date Line Crossing Problems

**Symptoms:**
- Polygons appearing to span entire world width
- Rendering gaps at 180°/-180° longitude
- Incorrect spatial queries near date line

**Diagnosis:**

```csharp
public class DateLineDiagnostics
{
    public void DiagnoseDateLineCrossings(List<Polygon> polygons)
    {
        Console.WriteLine("Date Line Crossing Analysis:");
        
        var crossingPolygons = polygons
            .Select((p, i) => new { Index = i, Polygon = p })
            .Where(x => CrossesDateLine(x.Polygon))
            .ToList();
        
        Console.WriteLine($"  Total Polygons: {polygons.Count}");
        Console.WriteLine($"  Date Line Crossings: {crossingPolygons.Count}");
        
        if (crossingPolygons.Count > 0)
        {
            Console.WriteLine("\n  Crossing Polygons:");
            foreach (var item in crossingPolygons.Take(5))
            {
                var coords = item.Polygon.ExteriorRing.Coordinates;
                var minX = coords.Min(c => c.X);
                var maxX = coords.Max(c => c.X);
                Console.WriteLine($"    Index {item.Index}: " +
                                $"X range {minX:F2} to {maxX:F2} " +
                                $"(span: {maxX - minX:F2})");
            }
        }
    }
    
    private bool CrossesDateLine(Polygon polygon)
    {
        var coords = polygon.ExteriorRing.Coordinates;
        var minX = coords.Min(c => c.X);
        var maxX = coords.Max(c => c.X);
        return (maxX - minX) > WorldDetail.WorldSizeX / 2.0;
    }
}
```

**Solution:**

```csharp
// Use the DateLineSplitter from tech documentation
var splitter = new DateLineSplitter();
var repairedPolygons = new List<Polygon>();

foreach (var polygon in polygons)
{
    var splitPolygons = splitter.SplitPolygonAtDateLine(polygon);
    repairedPolygons.AddRange(splitPolygons);
}
```

### Issue 4: Biome Distribution Unrealistic

**Symptoms:**
- Ocean coverage differs significantly from configured value
- Unexpected biome concentrations
- Missing expected biomes

**Diagnosis:**

```csharp
public class BiomeDistributionAnalyzer
{
    public void AnalyzeBiomeDistribution(
        List<Polygon> polygons,
        PlanetaryConfig config)
    {
        var totalArea = polygons.Sum(p => p.Area);
        var biomeAreas = polygons
            .GroupBy(p => GetBiomeType(p))
            .ToDictionary(
                g => g.Key,
                g => g.Sum(p => p.Area)
            );
        
        Console.WriteLine("Biome Distribution Analysis:");
        Console.WriteLine($"  Target Ocean Coverage: {config.OceanCoverage:P1}");
        
        var actualOcean = biomeAreas.GetValueOrDefault(BiomeType.Ocean, 0) / totalArea;
        Console.WriteLine($"  Actual Ocean Coverage: {actualOcean:P1}");
        
        var difference = Math.Abs(actualOcean - config.OceanCoverage);
        if (difference > 0.1)
        {
            Console.WriteLine($"  ⚠ WARNING: {difference:P1} deviation from target");
        }
        
        Console.WriteLine("\n  All Biomes:");
        foreach (var (biome, area) in biomeAreas.OrderByDescending(x => x.Value))
        {
            var coverage = area / totalArea;
            Console.WriteLine($"    {biome}: {coverage:P1}");
        }
    }
}
```

**Solutions:**

1. **Adjust Climate Parameters:**
```csharp
public static ClimateParameters GetEarthLikeClimate()
{
    return new ClimateParameters
    {
        GlobalTemperature = 15.0,      // 10-20°C recommended
        TemperatureVariation = 40.0,   // 30-50°C recommended
        PrecipitationBase = 1000.0,    // 800-1200mm recommended
        SeasonalVariation = 0.2        // 0.1-0.3 recommended
    };
}
```

2. **Increase Plate Count:**
```csharp
// More plates = more realistic distribution
var config = new PlanetaryConfig
{
    PlateCount = 12, // Instead of 5-7
    OceanCoverage = 0.71
};
```

3. **Try Different Seeds:**
```csharp
// Some seeds produce better distributions
for (int seed = 1; seed <= 100; seed++)
{
    var config = new PlanetaryConfig { Seed = seed, PlateCount = 12 };
    var generator = new SphericalPlanetGenerator(config);
    var polygons = generator.ExecuteProcess(/* ... */);
    
    var oceanCoverage = CalculateOceanCoverage(polygons);
    if (Math.Abs(oceanCoverage - 0.71) < 0.05)
    {
        Console.WriteLine($"Seed {seed} produces good distribution: {oceanCoverage:P1}");
    }
}
```

### Issue 5: Memory Exhaustion

**Symptoms:**
- `OutOfMemoryException` during generation
- System becomes unresponsive
- Generation fails at biome classification stage

**Diagnosis:**

```csharp
public class MemoryMonitor
{
    public void MonitorMemoryUsage(Action generationAction)
    {
        var before = GC.GetTotalMemory(true);
        var stopwatch = Stopwatch.StartNew();
        
        try
        {
            generationAction();
        }
        finally
        {
            stopwatch.Stop();
            var after = GC.GetTotalMemory(true);
            var used = (after - before) / (1024.0 * 1024.0);
            
            Console.WriteLine($"Memory Usage:");
            Console.WriteLine($"  Before: {before / (1024.0 * 1024.0):F2} MB");
            Console.WriteLine($"  After: {after / (1024.0 * 1024.0):F2} MB");
            Console.WriteLine($"  Used: {used:F2} MB");
            Console.WriteLine($"  Duration: {stopwatch.Elapsed:mm\\:ss}");
        }
    }
}
```

**Solutions:**

1. **Streaming Processing:**
```csharp
public IEnumerable<Polygon> GeneratePolygonsStreaming(PlanetaryConfig config)
{
    var generator = new SphericalPlanetGenerator(config);
    
    const int batchSize = 500;
    for (int i = 0; i < config.PlateCount; i += batchSize)
    {
        var batch = GenerateBatch(i, batchSize, config);
        
        foreach (var polygon in batch)
        {
            yield return polygon;
        }
        
        // Allow GC between batches
        GC.Collect();
        GC.WaitForPendingFinalizers();
    }
}

// Usage:
await foreach (var polygon in GeneratePolygonsStreaming(config))
{
    ProcessPolygon(polygon);
    SaveToDatabase(polygon);
}
```

2. **Reduce Precision:**
```csharp
// Use single precision for intermediate calculations
public class LowPrecisionGenerator : SphericalPlanetGenerator
{
    protected override List<Polygon> GenerateSphericalSurface()
    {
        // Use simplified geometry
        var factory = new GeometryFactory(
            new PrecisionModel(PrecisionModels.FloatingSingle),
            WorldDetail.SRID_METER
        );
        
        // Generate with reduced precision
        // ...
    }
}
```

## Performance Optimization Guidelines

### 1. Spatial Indexing

**Problem:** Neighbor queries take O(n²) time

**Solution:** Use STRtree spatial index

```csharp
public class SpatialIndexOptimizer
{
    public List<Polygon> FindNeighborsOptimized(
        Polygon targetPolygon,
        List<Polygon> allPolygons)
    {
        // Build spatial index
        var index = new STRtree<Polygon>();
        foreach (var polygon in allPolygons)
        {
            index.Insert(polygon.EnvelopeInternal, polygon);
        }
        
        // Query with expanded envelope
        var searchEnvelope = targetPolygon.EnvelopeInternal.Copy();
        searchEnvelope.ExpandBy(1000); // 1km buffer
        
        var candidates = index.Query(searchEnvelope)
            .Cast<Polygon>()
            .Where(p => p.Intersects(targetPolygon))
            .ToList();
        
        return candidates;
    }
}

// Performance comparison:
// Without index: ~10,000ms for 10,000 polygons
// With index:    ~50ms for 10,000 polygons (200x faster!)
```

### 2. Geometry Simplification

**Problem:** High-detail polygons slow down processing

**Solution:** Use Douglas-Peucker simplification

```csharp
public class GeometrySimplifier
{
    public List<Polygon> SimplifyForRendering(
        List<Polygon> polygons,
        double tolerance)
    {
        var simplifier = new DouglasPeuckerSimplifier(tolerance);
        
        return polygons
            .Select(p =>
            {
                var simplified = simplifier.Simplify(p) as Polygon;
                if (simplified != null && simplified.IsValid)
                {
                    GeometryUtils.CopyMetadata(p, simplified);
                    return simplified;
                }
                return p;
            })
            .ToList();
    }
}

// Example tolerances:
// High detail (zoom in): 10m
// Medium detail: 100m
// Low detail (zoom out): 1000m
```

### 3. Parallel Processing

**Problem:** Single-threaded processing is slow

**Solution:** Use parallel processing for independent operations

```csharp
public class ParallelPlanetGenerator : SphericalPlanetGenerator
{
    public override List<Polygon> ExecuteProcess(
        List<Polygon> inputPolygons,
        List<Polygon> neighborPolygons,
        Random randomSource)
    {
        // Generate seeds (sequential for deterministic results)
        var seeds = GenerateSeeds(randomSource);
        
        // Process plates in parallel
        var polygons = seeds
            .AsParallel()
            .WithDegreeOfParallelism(Environment.ProcessorCount)
            .Select(seed => GeneratePlatePolygon(seed))
            .ToList();
        
        // Apply biome classification in parallel
        var biomePolygons = polygons
            .AsParallel()
            .Select(p => _biomeClassifier.ClassifyPolygon(p, _config.Climate))
            .ToList();
        
        return biomePolygons;
    }
}
```

### 4. Caching and Memoization

**Problem:** Repeated calculations waste time

**Solution:** Cache expensive operations

```csharp
public class CachedProjections
{
    private readonly ConcurrentDictionary<(double, double, ProjectionType), Point2D> _cache = new();
    private readonly MapProjections _projections = new();
    
    public Point2D ProjectCached(
        SphericalCoordinate coord,
        ProjectionType type)
    {
        var key = (coord.Latitude, coord.Longitude, type);
        
        return _cache.GetOrAdd(key, _ =>
        {
            return type switch
            {
                ProjectionType.Mercator => _projections.ProjectMercator(coord),
                ProjectionType.Robinson => _projections.ProjectRobinson(coord),
                _ => _projections.ProjectEquirectangular(coord)
            };
        });
    }
}
```

### 5. Database Optimization

**Problem:** Saving large polygon sets is slow

**Solution:** Batch inserts and indexing

```csharp
public class OptimizedPolygonStorage
{
    public async Task SavePolygonsBatchAsync(
        List<Polygon> polygons,
        string connectionString)
    {
        const int batchSize = 1000;
        
        using var connection = new NpgsqlConnection(connectionString);
        await connection.OpenAsync();
        
        // Disable autocommit for batch insert
        using var transaction = await connection.BeginTransactionAsync();
        
        try
        {
            for (int i = 0; i < polygons.Count; i += batchSize)
            {
                var batch = polygons.Skip(i).Take(batchSize);
                
                using var writer = await connection.BeginBinaryImportAsync(
                    "COPY planet_polygons (geometry, biome, metadata) FROM STDIN BINARY"
                );
                
                foreach (var polygon in batch)
                {
                    await writer.StartRowAsync();
                    await writer.WriteAsync(polygon, NpgsqlDbType.Geometry);
                    await writer.WriteAsync(GetBiomeType(polygon).ToString());
                    await writer.WriteAsync(GetMetadata(polygon), NpgsqlDbType.Jsonb);
                }
                
                await writer.CompleteAsync();
            }
            
            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}
```

## Performance Benchmarks

### Expected Performance Metrics

| Configuration | Plate Count | Generation Time | Peak Memory | Polygons Generated |
|--------------|-------------|-----------------|-------------|-------------------|
| Small | 5 | 30 seconds | 500 MB | ~500 |
| Medium | 12 | 5-8 minutes | 2-3 GB | ~2,000 |
| Large | 20 | 10-15 minutes | 4-5 GB | ~5,000 |
| Extra Large | 30 | 15-25 minutes | 6-8 GB | ~10,000 |

### Hardware Recommendations

**Minimum:**
- CPU: 4 cores, 2.0 GHz
- RAM: 8 GB
- Storage: 10 GB available

**Recommended:**
- CPU: 8+ cores, 3.0+ GHz
- RAM: 16-32 GB
- Storage: 50 GB SSD

**Optimal:**
- CPU: 16+ cores, 3.5+ GHz
- RAM: 64 GB
- Storage: 100 GB NVMe SSD

## Diagnostic Tools

### Performance Profiling

```csharp
public class PerformanceProfiler
{
    private readonly Dictionary<string, Stopwatch> _timers = new();
    
    public void StartTimer(string operation)
    {
        if (!_timers.ContainsKey(operation))
        {
            _timers[operation] = new Stopwatch();
        }
        _timers[operation].Restart();
    }
    
    public void StopTimer(string operation)
    {
        if (_timers.TryGetValue(operation, out var timer))
        {
            timer.Stop();
        }
    }
    
    public void PrintReport()
    {
        Console.WriteLine("\nPerformance Report:");
        Console.WriteLine("══════════════════════════════════");
        
        foreach (var (operation, timer) in _timers.OrderByDescending(x => x.Value.Elapsed))
        {
            Console.WriteLine($"{operation,-30}: {timer.Elapsed:mm\\:ss\\.fff}");
        }
    }
}

// Usage:
var profiler = new PerformanceProfiler();
profiler.StartTimer("Total Generation");

profiler.StartTimer("Voronoi Generation");
var voronoi = GenerateVoronoi();
profiler.StopTimer("Voronoi Generation");

profiler.StartTimer("Biome Classification");
var biomes = ClassifyBiomes(voronoi);
profiler.StopTimer("Biome Classification");

profiler.StopTimer("Total Generation");
profiler.PrintReport();
```

## Related Documentation

- [Developer Guide](developer-guide-spherical-planet-generation.md) - Implementation tutorials
- [Quick Reference](quick-reference-spherical-planet.md) - Fast lookup
- [API Specification](api-spherical-planet-generation.md) - REST API docs
- [Technical Implementation](tech-spherical-planet-implementation.md) - Implementation details

---

**Last Updated:** 2024-12-29  
**Maintained By:** Technical Support Team  
**For Additional Support:** Create an issue in the repository with diagnostic output
