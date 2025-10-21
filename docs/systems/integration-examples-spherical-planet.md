# Spherical Planet Generation - Integration Examples

**Document Type:** Integration Guide  
**Version:** 1.0  
**Author:** Technical Documentation Team  
**Date:** 2024-12-29  
**Status:** Complete  
**Related Documents:** 
- [API Specification](api-spherical-planet-generation.md)
- [Developer Guide](developer-guide-spherical-planet-generation.md)
- [Quick Reference](quick-reference-spherical-planet.md)

## Overview

This document provides practical, real-world integration examples showing how to use the Spherical Planet Generation system in various scenarios. Each example includes complete, working code that demonstrates the full workflow from generation to visualization.

## Example 1: Complete Planet Generation Workflow

This example demonstrates the full workflow: configuration, generation, monitoring, and data retrieval.

### Backend Implementation (C#)

```csharp
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BlueMarble.Spatial;
using NetTopologySuite.Geometries;

public class PlanetGenerationWorkflow
{
    private readonly PlanetaryConfig _config;
    private readonly SphericalPlanetGenerator _generator;
    
    public PlanetGenerationWorkflow()
    {
        // Step 1: Configure planet parameters
        _config = new PlanetaryConfig
        {
            RadiusMeters = 6371000,  // Earth-like
            PlateCount = 12,
            OceanCoverage = 0.71,
            Seed = 42,
            Climate = new ClimateParameters
            {
                GlobalTemperature = 15.0,
                TemperatureVariation = 40.0,
                PrecipitationBase = 1000.0,
                SeasonalVariation = 0.2
            }
        };
        
        _generator = new SphericalPlanetGenerator(_config);
    }
    
    public async Task<PlanetGenerationResult> GenerateCompleteAsync()
    {
        var random = new Random(_config.Seed);
        
        // Step 2: Execute generation
        Console.WriteLine("Starting planet generation...");
        var startTime = DateTime.UtcNow;
        
        var polygons = _generator.ExecuteProcess(
            new List<Polygon>(),
            new List<Polygon>(),
            random
        );
        
        var duration = DateTime.UtcNow - startTime;
        Console.WriteLine($"Generation completed in {duration:mm\\:ss}");
        
        // Step 3: Validate results
        var validationResult = ValidateGeneration(polygons);
        if (!validationResult.IsValid)
        {
            throw new InvalidOperationException(
                $"Generation validation failed: {validationResult.ErrorMessage}");
        }
        
        // Step 4: Save to GeoPackage
        var outputPath = "planet_output.gpkg";
        SavePolygons.WriteToGeoPackage(polygons, outputPath, "planet_surface");
        Console.WriteLine($"Saved to {outputPath}");
        
        // Step 5: Generate statistics
        var stats = GenerateStatistics(polygons);
        
        return new PlanetGenerationResult
        {
            Polygons = polygons,
            Statistics = stats,
            ValidationResult = validationResult,
            OutputPath = outputPath,
            Duration = duration
        };
    }
    
    private ValidationResult ValidateGeneration(List<Polygon> polygons)
    {
        var errors = new List<string>();
        
        // Check polygon count
        if (polygons.Count == 0)
        {
            errors.Add("No polygons generated");
        }
        
        // Check SRID consistency
        if (!polygons.All(p => p.SRID == WorldDetail.SRID_METER))
        {
            errors.Add("Inconsistent SRID values");
        }
        
        // Check topology validity
        var invalidCount = polygons.Count(p => !p.IsValid);
        if (invalidCount > 0)
        {
            errors.Add($"{invalidCount} polygons have invalid topology");
        }
        
        // Check biome distribution
        var oceanCoverage = CalculateOceanCoverage(polygons);
        if (Math.Abs(oceanCoverage - _config.OceanCoverage) > 0.1)
        {
            errors.Add($"Ocean coverage {oceanCoverage:P1} differs from " +
                      $"target {_config.OceanCoverage:P1}");
        }
        
        return new ValidationResult
        {
            IsValid = errors.Count == 0,
            ErrorMessage = string.Join("; ", errors)
        };
    }
    
    private PlanetStatistics GenerateStatistics(List<Polygon> polygons)
    {
        var biomeDistribution = polygons
            .GroupBy(p => GetBiomeType(p))
            .ToDictionary(
                g => g.Key,
                g => new BiomeStats
                {
                    Count = g.Count(),
                    TotalArea = g.Sum(p => p.Area),
                    Coverage = g.Sum(p => p.Area) / polygons.Sum(p => p.Area)
                }
            );
        
        return new PlanetStatistics
        {
            TotalPolygons = polygons.Count,
            TotalArea = polygons.Sum(p => p.Area),
            BiomeDistribution = biomeDistribution,
            AveragePolygonArea = polygons.Average(p => p.Area),
            MinPolygonArea = polygons.Min(p => p.Area),
            MaxPolygonArea = polygons.Max(p => p.Area)
        };
    }
    
    private double CalculateOceanCoverage(List<Polygon> polygons)
    {
        var totalArea = polygons.Sum(p => p.Area);
        var oceanArea = polygons
            .Where(p => GetBiomeType(p) == BiomeType.Ocean)
            .Sum(p => p.Area);
        return oceanArea / totalArea;
    }
    
    private BiomeType GetBiomeType(Polygon polygon)
    {
        // Extract biome from metadata
        if (polygon.UserData is Dictionary<string, object> metadata &&
            metadata.TryGetValue("BiomeType", out var biomeValue))
        {
            return (BiomeType)biomeValue;
        }
        return BiomeType.Ocean;
    }
}

// Usage
public class Program
{
    public static async Task Main(string[] args)
    {
        var workflow = new PlanetGenerationWorkflow();
        var result = await workflow.GenerateCompleteAsync();
        
        Console.WriteLine($"\nGeneration Summary:");
        Console.WriteLine($"  Total Polygons: {result.Statistics.TotalPolygons:N0}");
        Console.WriteLine($"  Total Area: {result.Statistics.TotalArea:E2} m²");
        Console.WriteLine($"  Duration: {result.Duration:mm\\:ss}");
        Console.WriteLine($"\nBiome Distribution:");
        
        foreach (var (biome, stats) in result.Statistics.BiomeDistribution)
        {
            Console.WriteLine($"  {biome}: {stats.Coverage:P1} " +
                            $"({stats.Count:N0} polygons)");
        }
    }
}
```

## Example 2: Web API Integration

This example shows how to create a REST API endpoint for planet generation with async processing.

### ASP.NET Core Web API

```csharp
using Microsoft.AspNetCore.Mvc;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class PlanetController : ControllerBase
{
    private static ConcurrentDictionary<string, GenerationTask> _activeTasks = new();
    private readonly IBackgroundTaskQueue _taskQueue;
    private readonly ILogger<PlanetController> _logger;
    
    public PlanetController(
        IBackgroundTaskQueue taskQueue,
        ILogger<PlanetController> logger)
    {
        _taskQueue = taskQueue;
        _logger = logger;
    }
    
    [HttpPost("generate")]
    public async Task<IActionResult> GeneratePlanet(
        [FromBody] PlanetGenerationRequest request)
    {
        // Validate request
        if (request.Config.PlateCount < 3 || request.Config.PlateCount > 50)
        {
            return BadRequest(new { error = "Plate count must be between 3 and 50" });
        }
        
        // Create task ID
        var taskId = Guid.NewGuid().ToString();
        var task = new GenerationTask
        {
            TaskId = taskId,
            Status = GenerationStatus.Queued,
            CreatedAt = DateTime.UtcNow,
            Config = request.Config
        };
        
        _activeTasks[taskId] = task;
        
        // Queue background task
        await _taskQueue.QueueBackgroundWorkItemAsync(async token =>
        {
            await ProcessPlanetGenerationAsync(taskId, request.Config, token);
        });
        
        return Accepted(new
        {
            taskId = taskId,
            status = "queued",
            statusUrl = $"/api/planet/status/{taskId}"
        });
    }
    
    [HttpGet("status/{taskId}")]
    public IActionResult GetGenerationStatus(string taskId)
    {
        if (!_activeTasks.TryGetValue(taskId, out var task))
        {
            return NotFound(new { error = "Task not found" });
        }
        
        return Ok(new
        {
            taskId = task.TaskId,
            status = task.Status.ToString().ToLower(),
            progress = new
            {
                percentage = task.Progress,
                currentStage = task.CurrentStage,
                estimatedTimeRemaining = task.EstimatedTimeRemaining
            },
            result = task.Result,
            error = task.Error
        });
    }
    
    [HttpGet("{planetId}/polygons")]
    public async Task<IActionResult> GetPlanetData(
        string planetId,
        [FromQuery] string projection = "equirectangular",
        [FromQuery] string format = "geojson",
        [FromQuery] string biome = null)
    {
        try
        {
            // Load planet data
            var polygons = LoadPolygons.ReadPolygonsFromGeoPackage(
                $"planets/{planetId}.gpkg", 
                "planet_surface"
            );
            
            if (polygons == null || polygons.Count == 0)
            {
                return NotFound(new { error = "Planet not found" });
            }
            
            // Filter by biome if specified
            if (!string.IsNullOrEmpty(biome))
            {
                polygons = polygons
                    .Where(p => GetBiomeType(p).ToString() == biome)
                    .ToList();
            }
            
            // Apply projection if different from current
            if (projection != "equirectangular")
            {
                var projector = new MapProjections();
                polygons = ApplyProjection(polygons, projection, projector);
            }
            
            // Format output
            if (format == "geojson")
            {
                var geoJson = ConvertToGeoJson(polygons);
                return Ok(geoJson);
            }
            
            return Ok(new { polygons = polygons.Count, data = polygons });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving planet data");
            return StatusCode(500, new { error = "Internal server error" });
        }
    }
    
    private async Task ProcessPlanetGenerationAsync(
        string taskId,
        PlanetaryConfig config,
        CancellationToken cancellationToken)
    {
        var task = _activeTasks[taskId];
        
        try
        {
            task.Status = GenerationStatus.InProgress;
            task.CurrentStage = "generating_plates";
            
            var generator = new SphericalPlanetGenerator(config);
            var random = new Random(config.Seed);
            
            // Generate with progress tracking
            var polygons = await Task.Run(() =>
            {
                task.Progress = 25;
                var result = generator.ExecuteProcess(
                    new List<Polygon>(),
                    new List<Polygon>(),
                    random
                );
                task.Progress = 100;
                return result;
            }, cancellationToken);
            
            // Save planet
            var planetId = Guid.NewGuid().ToString();
            var outputPath = $"planets/{planetId}.gpkg";
            SavePolygons.WriteToGeoPackage(polygons, outputPath, "planet_surface");
            
            task.Status = GenerationStatus.Completed;
            task.Result = new
            {
                planetId = planetId,
                downloadUrl = $"/api/planet/{planetId}/download",
                statistics = GenerateStats(polygons)
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Planet generation failed");
            task.Status = GenerationStatus.Failed;
            task.Error = new { message = ex.Message };
        }
    }
}
```

### Background Task Queue Service

```csharp
public interface IBackgroundTaskQueue
{
    ValueTask QueueBackgroundWorkItemAsync(
        Func<CancellationToken, ValueTask> workItem);
    
    ValueTask<Func<CancellationToken, ValueTask>> DequeueAsync(
        CancellationToken cancellationToken);
}

public class BackgroundTaskQueue : IBackgroundTaskQueue
{
    private readonly Channel<Func<CancellationToken, ValueTask>> _queue;
    
    public BackgroundTaskQueue(int capacity = 100)
    {
        _queue = Channel.CreateBounded<Func<CancellationToken, ValueTask>>(
            new BoundedChannelOptions(capacity)
            {
                FullMode = BoundedChannelFullMode.Wait
            });
    }
    
    public async ValueTask QueueBackgroundWorkItemAsync(
        Func<CancellationToken, ValueTask> workItem)
    {
        await _queue.Writer.WriteAsync(workItem);
    }
    
    public async ValueTask<Func<CancellationToken, ValueTask>> DequeueAsync(
        CancellationToken cancellationToken)
    {
        return await _queue.Reader.ReadAsync(cancellationToken);
    }
}
```

## Example 3: Interactive Web Visualization

Frontend implementation using JavaScript to visualize generated planets.

### HTML Structure

```html
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <title>Planet Viewer</title>
    <link rel="stylesheet" href="https://unpkg.com/leaflet@1.9.4/dist/leaflet.css" />
    <style>
        #map { height: 600px; width: 100%; }
        #controls { padding: 20px; background: #f0f0f0; }
        .biome-legend { 
            position: absolute; 
            bottom: 20px; 
            right: 20px; 
            background: white; 
            padding: 10px;
            border-radius: 5px;
            box-shadow: 0 2px 5px rgba(0,0,0,0.2);
        }
    </style>
</head>
<body>
    <div id="controls">
        <h2>Spherical Planet Viewer</h2>
        <label>
            Projection:
            <select id="projection">
                <option value="equirectangular">Equirectangular</option>
                <option value="mercator">Mercator</option>
                <option value="robinson">Robinson</option>
            </select>
        </label>
        <button id="loadPlanet">Load Planet</button>
        <span id="status"></span>
    </div>
    
    <div id="map"></div>
    
    <div class="biome-legend">
        <h4>Biomes</h4>
        <div id="legend-content"></div>
    </div>
    
    <script src="https://unpkg.com/leaflet@1.9.4/dist/leaflet.js"></script>
    <script src="planet-viewer.js"></script>
</body>
</html>
```

### JavaScript Implementation

```javascript
// planet-viewer.js
class PlanetViewer {
    constructor() {
        this.map = null;
        this.currentLayer = null;
        this.planetId = null;
        this.biomeColors = this.initializeBiomeColors();
        
        this.initializeMap();
        this.setupEventListeners();
    }
    
    initializeMap() {
        this.map = L.map('map', {
            center: [0, 0],
            zoom: 2,
            maxZoom: 10,
            minZoom: 1
        });
        
        // No base map - we'll show our generated planet
        L.tileLayer('data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAYAAAAfFcSJAAAADUlEQVR42mNkYPhfDwAChwGA60e6kgAAAABJRU5ErkJggg==', {
            attribution: 'BlueMarble Planet Generator'
        }).addTo(this.map);
    }
    
    setupEventListeners() {
        document.getElementById('loadPlanet').addEventListener('click', () => {
            this.generateAndLoadPlanet();
        });
        
        document.getElementById('projection').addEventListener('change', (e) => {
            if (this.planetId) {
                this.loadPlanetWithProjection(this.planetId, e.target.value);
            }
        });
    }
    
    async generateAndLoadPlanet() {
        const statusEl = document.getElementById('status');
        statusEl.textContent = 'Generating planet...';
        
        try {
            // Start generation
            const response = await fetch('/api/planet/generate', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({
                    config: {
                        radiusMeters: 6371000,
                        plateCount: 12,
                        oceanCoverage: 0.71,
                        seed: Math.floor(Math.random() * 10000)
                    }
                })
            });
            
            const task = await response.json();
            
            // Poll for completion
            const planetData = await this.pollForCompletion(task.taskId);
            this.planetId = planetData.planetId;
            
            // Load planet data
            const projection = document.getElementById('projection').value;
            await this.loadPlanetWithProjection(this.planetId, projection);
            
            statusEl.textContent = 'Planet loaded successfully!';
        } catch (error) {
            statusEl.textContent = `Error: ${error.message}`;
            console.error('Generation error:', error);
        }
    }
    
    async pollForCompletion(taskId, maxAttempts = 60) {
        for (let i = 0; i < maxAttempts; i++) {
            await this.sleep(5000); // Wait 5 seconds
            
            const response = await fetch(`/api/planet/status/${taskId}`);
            const status = await response.json();
            
            document.getElementById('status').textContent = 
                `${status.progress.currentStage}: ${status.progress.percentage}%`;
            
            if (status.status === 'completed') {
                return status.result;
            } else if (status.status === 'failed') {
                throw new Error(status.error.message);
            }
        }
        
        throw new Error('Generation timeout');
    }
    
    async loadPlanetWithProjection(planetId, projection) {
        const response = await fetch(
            `/api/planet/${planetId}/polygons?projection=${projection}&format=geojson`
        );
        const geoJson = await response.json();
        
        // Remove existing layer
        if (this.currentLayer) {
            this.map.removeLayer(this.currentLayer);
        }
        
        // Add new layer
        this.currentLayer = L.geoJSON(geoJson, {
            style: (feature) => this.getFeatureStyle(feature),
            onEachFeature: (feature, layer) => {
                layer.bindPopup(this.createPopupContent(feature));
            }
        }).addTo(this.map);
        
        // Fit bounds
        this.map.fitBounds(this.currentLayer.getBounds());
        
        // Update legend
        this.updateLegend(geoJson);
    }
    
    getFeatureStyle(feature) {
        const biome = feature.properties.biomeType || 'Ocean';
        return {
            fillColor: this.biomeColors[biome] || '#4169E1',
            fillOpacity: 0.7,
            color: '#333',
            weight: 0.5
        };
    }
    
    createPopupContent(feature) {
        const props = feature.properties;
        return `
            <strong>Biome:</strong> ${props.biomeType}<br>
            <strong>Temperature:</strong> ${props.temperature}°C<br>
            <strong>Precipitation:</strong> ${props.precipitation}mm/year<br>
            <strong>Elevation:</strong> ${props.elevation}m
        `;
    }
    
    updateLegend(geoJson) {
        const biomes = {};
        geoJson.features.forEach(f => {
            const biome = f.properties.biomeType || 'Ocean';
            biomes[biome] = (biomes[biome] || 0) + 1;
        });
        
        const legendHtml = Object.entries(biomes)
            .sort((a, b) => b[1] - a[1])
            .map(([biome, count]) => `
                <div style="margin: 5px 0;">
                    <span style="display: inline-block; width: 20px; height: 20px; 
                                 background: ${this.biomeColors[biome]}; 
                                 margin-right: 5px; border: 1px solid #333;">
                    </span>
                    ${biome} (${count})
                </div>
            `).join('');
        
        document.getElementById('legend-content').innerHTML = legendHtml;
    }
    
    initializeBiomeColors() {
        return {
            'Ocean': '#4169E1',
            'TropicalRainforest': '#228B22',
            'TemperateRainforest': '#2E8B57',
            'BorealForest': '#006400',
            'Tundra': '#B0E0E6',
            'Desert': '#EDC9AF',
            'Grassland': '#9ACD32',
            'Savanna': '#F4A460',
            'TemperateForest': '#3CB371',
            'TropicalSavanna': '#DAA520',
            'Alpine': '#DCDCDC',
            'Wetland': '#4682B4',
            'IceSheet': '#F0F8FF',
            'BarrenLand': '#A9A9A9'
        };
    }
    
    sleep(ms) {
        return new Promise(resolve => setTimeout(resolve, ms));
    }
}

// Initialize when DOM is ready
document.addEventListener('DOMContentLoaded', () => {
    new PlanetViewer();
});
```

## Example 4: Batch Processing and Analysis

Processing multiple planets for comparative analysis.

```python
import asyncio
import pandas as pd
from bluemarble_planet import PlanetAPI
from datetime import datetime

class PlanetBatchAnalyzer:
    def __init__(self, api_key):
        self.api = PlanetAPI(api_key=api_key)
        self.results = []
    
    async def generate_planet_batch(self, configurations):
        """Generate multiple planets in parallel"""
        tasks = []
        
        for i, config in enumerate(configurations):
            print(f"Starting generation {i+1}/{len(configurations)}...")
            task = await self.api.planet.generate(config=config)
            tasks.append((config, task))
        
        # Wait for all to complete
        for config, task in tasks:
            while not task.is_complete():
                progress = task.progress.percentage
                print(f"Planet {config['seed']}: {progress}%")
                await asyncio.sleep(30)
            
            planet = task.result()
            self.results.append({
                'config': config,
                'planet': planet,
                'statistics': await self.analyze_planet(planet)
            })
        
        return self.results
    
    async def analyze_planet(self, planet):
        """Analyze planet biome distribution and characteristics"""
        biomes = planet.biomes.get_statistics()
        
        return {
            'total_polygons': planet.statistics.total_polygons,
            'ocean_coverage': biomes.get('Ocean', {}).get('coverage', 0),
            'land_coverage': 1 - biomes.get('Ocean', {}).get('coverage', 0),
            'biome_diversity': len(biomes),
            'dominant_biome': max(biomes.items(), key=lambda x: x[1]['coverage'])[0],
            'generation_time': planet.metadata.generation_time
        }
    
    def export_comparison(self, filename='planet_comparison.csv'):
        """Export comparison data to CSV"""
        data = []
        for result in self.results:
            row = {
                'seed': result['config']['seed'],
                'plate_count': result['config']['plate_count'],
                'radius': result['config']['radius_meters'],
                **result['statistics']
            }
            data.append(row)
        
        df = pd.DataFrame(data)
        df.to_csv(filename, index=False)
        print(f"Exported to {filename}")
        return df

# Usage example
async def main():
    analyzer = PlanetBatchAnalyzer(api_key='your-api-key')
    
    # Generate 5 planets with different seeds
    configs = [
        {
            'radius_meters': 6371000,
            'plate_count': 12,
            'ocean_coverage': 0.71,
            'seed': seed
        }
        for seed in range(1, 6)
    ]
    
    results = await analyzer.generate_planet_batch(configs)
    
    # Export comparison
    df = analyzer.export_comparison()
    
    # Print summary
    print("\nPlanet Comparison Summary:")
    print(df[['seed', 'ocean_coverage', 'biome_diversity', 'generation_time']])

if __name__ == '__main__':
    asyncio.run(main())
```

## Related Documentation

- [API Specification](api-spherical-planet-generation.md) - Complete API reference
- [Developer Guide](developer-guide-spherical-planet-generation.md) - Step-by-step tutorials
- [Quick Reference](quick-reference-spherical-planet.md) - Fast lookup guide
- [Visual Guide](visual-guide-map-projections.md) - Projection visualizations
- [Technical Implementation](tech-spherical-planet-implementation.md) - Implementation details

---

**Last Updated:** 2024-12-29  
**Maintained By:** Technical Documentation Team
