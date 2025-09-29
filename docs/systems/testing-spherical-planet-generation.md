# Spherical Planet Generation - Testing Strategy

**Document Type:** Testing Strategy  
**Version:** 1.0  
**Author:** Quality Assurance Team  
**Date:** 2024-12-29  
**Status:** Draft  
**Related Specifications:** 
- [Spherical Planet Generation System](spec-spherical-planet-generation.md)
- [Technical Implementation Guide](tech-spherical-planet-implementation.md)

## Overview

This document outlines the comprehensive testing strategy for the Spherical Planet Generation and Projection System. It covers unit testing, integration testing, performance testing, and validation of mathematical accuracy to ensure the system meets scientific and functional requirements.

## Testing Objectives

### Primary Objectives
1. **Mathematical Accuracy:** Verify projection calculations are mathematically correct within specified tolerances
2. **Scientific Validity:** Ensure biome classifications and climate modeling match real-world patterns
3. **Performance Requirements:** Validate generation completes within specified time and memory constraints
4. **Integration Compatibility:** Confirm seamless integration with existing NetTopologySuite operations
5. **Data Integrity:** Ensure generated data maintains spatial topology and SRID compatibility

### Quality Metrics
- **Unit Test Coverage:** >95% for core algorithms
- **Integration Test Coverage:** 100% of API endpoints
- **Performance Compliance:** 100% of operations meet specified performance targets
- **Mathematical Accuracy:** <1 meter error in coordinate transformations
- **Scientific Accuracy:** >95% accuracy in biome classification against reference data

## Unit Testing Strategy

### 1. Mathematical Functions Testing

#### Spherical Coordinate Conversion Tests

```csharp
[TestClass]
public class SphericalCoordinateTests
{
    [TestMethod]
    [DataRow(0.0, 0.0, ExpectedResult = true)] // Equator/Prime Meridian
    [DataRow(90.0, 0.0, ExpectedResult = true)] // North Pole
    [DataRow(-90.0, 180.0, ExpectedResult = true)] // South Pole/Date Line
    [DataRow(23.5, -45.0, ExpectedResult = true)] // Tropic of Cancer
    public bool ConvertSphericalToCartesian_ValidCoordinates_AccurateConversion(
        double latitude, double longitude)
    {
        // Arrange
        var spherical = new SphericalCoordinate 
        { 
            Latitude = latitude, 
            Longitude = longitude, 
            Radius = 6371000 
        };
        var converter = new CoordinateConverter();

        // Act
        var cartesian = converter.SphericalToCartesian(spherical);
        var backConverted = converter.CartesianToSpherical(cartesian);

        // Assert
        var latError = Math.Abs(spherical.Latitude - backConverted.Latitude);
        var lonError = Math.Abs(spherical.Longitude - backConverted.Longitude);
        
        return latError < 0.001 && lonError < 0.001; // 1 meter tolerance
    }

    [TestMethod]
    public void CalculateSphericalDistance_KnownDistances_AccurateResults()
    {
        // Arrange - Test known distances
        var testCases = new[]
        {
            new { 
                Point1 = new SphericalCoordinate { Lat = 0, Lon = 0, Radius = 6371000 },
                Point2 = new SphericalCoordinate { Lat = 0, Lon = 90, Radius = 6371000 },
                ExpectedDistance = 10018754.171394 // Quarter circumference
            },
            new {
                Point1 = new SphericalCoordinate { Lat = 0, Lon = 0, Radius = 6371000 },
                Point2 = new SphericalCoordinate { Lat = 90, Lon = 0, Radius = 6371000 },
                ExpectedDistance = 10018754.171394 // Quarter meridian
            }
        };

        var calculator = new SphericalDistanceCalculator();

        foreach (var testCase in testCases)
        {
            // Act
            var actualDistance = calculator.CalculateDistance(
                testCase.Point1, testCase.Point2);

            // Assert
            var error = Math.Abs(actualDistance - testCase.ExpectedDistance);
            Assert.IsTrue(error < 1.0, $"Distance error: {error}m");
        }
    }
}
```

#### Map Projection Tests

```csharp
[TestClass]
public class MapProjectionTests
{
    [TestMethod]
    public void MercatorProjection_StandardTestPoints_MathematicallyAccurate()
    {
        // Arrange
        var projector = new MapProjections();
        var testPoints = new[]
        {
            new { Lat = 0.0, Lon = 0.0, ExpectedX = 0.0, ExpectedY = 0.0 },
            new { Lat = 30.0, Lon = 45.0, ExpectedX = 5009377.085697, ExpectedY = 3503549.843504 },
            new { Lat = -45.0, Lon = -90.0, ExpectedX = -10018754.171394, ExpectedY = -5591295.918644 }
        };

        foreach (var test in testPoints)
        {
            // Act
            var result = projector.ProjectMercator(new SphericalCoordinate 
            { 
                Latitude = test.Lat, 
                Longitude = test.Lon, 
                Radius = 6371000 
            });

            // Assert
            var xError = Math.Abs(result.X - test.ExpectedX);
            var yError = Math.Abs(result.Y - test.ExpectedY);
            
            Assert.IsTrue(xError < 1.0, $"X coordinate error: {xError}m");
            Assert.IsTrue(yError < 1.0, $"Y coordinate error: {yError}m");
        }
    }

    [TestMethod]
    public void RobinsonProjection_InterpolationAccuracy_WithinTolerance()
    {
        // Arrange
        var projector = new MapProjections();
        var robinsonTester = new RobinsonProjectionValidator();

        // Test interpolation at various latitudes
        for (double lat = -90; lat <= 90; lat += 5)
        {
            // Act
            var result = projector.ProjectRobinson(new SphericalCoordinate 
            { 
                Latitude = lat, 
                Longitude = 0, 
                Radius = 6371000 
            });

            var expected = robinsonTester.GetExpectedValue(lat);

            // Assert
            var error = Math.Abs(result.Y - expected.Y);
            Assert.IsTrue(error < 100.0, $"Robinson interpolation error at {lat}°: {error}m");
        }
    }
}
```

### 2. Biome Classification Tests

```csharp
[TestClass]
public class BiomeClassifierTests
{
    [TestMethod]
    public void ClassifyBiome_KnownClimateConditions_CorrectBiomeTypes()
    {
        // Arrange
        var classifier = new BiomeClassifier();
        var testCases = new[]
        {
            new { 
                Temp = 26.0, Precip = 2500.0, Elev = 200.0, Lat = 5.0,
                Expected = BiomeType.TropicalRainforest 
            },
            new { 
                Temp = 25.0, Precip = 150.0, Elev = 500.0, Lat = 25.0,
                Expected = BiomeType.Desert 
            },
            new { 
                Temp = -8.0, Precip = 300.0, Elev = 100.0, Lat = 75.0,
                Expected = BiomeType.Tundra 
            },
            new { 
                Temp = 2.0, Precip = 600.0, Elev = 300.0, Lat = 60.0,
                Expected = BiomeType.BorealForest 
            }
        };

        var climate = new ClimateParameters();

        foreach (var test in testCases)
        {
            // Act
            var biome = classifier.DetermineBiome(
                temperature: test.Temp,
                precipitation: test.Precip,
                elevation: test.Elev,
                latitude: test.Lat,
                climate: climate
            );

            // Assert
            Assert.AreEqual(test.Expected, biome, 
                $"Climate conditions T:{test.Temp}°C, P:{test.Precip}mm, E:{test.Elev}m should be {test.Expected}");
        }
    }

    [TestMethod]
    public void BiomeDistribution_EarthLikeParameters_RealisticProportions()
    {
        // Arrange
        var generator = new SphericalPlanetGenerator(EarthLikeConfig());
        var expectedDistribution = new Dictionary<BiomeType, double>
        {
            [BiomeType.Ocean] = 0.71,
            [BiomeType.Desert] = 0.14,
            [BiomeType.TropicalRainforest] = 0.06,
            [BiomeType.TemperateForest] = 0.09
        };

        // Act
        var polygons = generator.ExecuteProcess(
            new List<Polygon>(), new List<Polygon>(), new Random(42));
        var distribution = CalculateBiomeDistribution(polygons);

        // Assert
        foreach (var expected in expectedDistribution)
        {
            var actual = distribution.GetValueOrDefault(expected.Key, 0.0);
            var error = Math.Abs(actual - expected.Value);
            Assert.IsTrue(error < 0.05, 
                $"Biome {expected.Key} distribution error: {error:P1} (expected {expected.Value:P1}, got {actual:P1})");
        }
    }
}
```

### 3. Geometric Operations Tests

```csharp
[TestClass]
public class GeometricOperationsTests
{
    [TestMethod]
    public void WorldWrapping_DateLineCrossing_ValidTopology()
    {
        // Arrange
        var wrapper = new SeamlessWorldWrapper();
        var dateLineCrossingPolygon = CreateDateLineCrossingPolygon();

        // Act
        var wrappedPolygons = wrapper.EnsureWorldWrapping(
            new List<Polygon> { dateLineCrossingPolygon });

        // Assert
        Assert.IsTrue(wrappedPolygons.All(p => p.IsValid), 
            "All wrapped polygons must have valid topology");
        
        var totalArea = wrappedPolygons.Sum(p => p.Area);
        var originalArea = dateLineCrossingPolygon.Area;
        var areaError = Math.Abs(totalArea - originalArea) / originalArea;
        
        Assert.IsTrue(areaError < 0.001, 
            $"Area preservation error: {areaError:P3}");
    }

    [TestMethod]
    public void PolygonSimplification_ComplexGeometry_PreservesShape()
    {
        // Arrange
        var simplifier = new GeometrySimplifier();
        var complexPolygon = CreateComplexCoastlinePolygon();
        var tolerance = 100.0; // 100 meter tolerance

        // Act
        var simplified = simplifier.Simplify(complexPolygon, tolerance);

        // Assert
        var areaRatio = simplified.Area / complexPolygon.Area;
        Assert.IsTrue(areaRatio > 0.95 && areaRatio < 1.05, 
            $"Area change too large: {areaRatio:P1}");
        
        Assert.IsTrue(simplified.Coordinates.Length < complexPolygon.Coordinates.Length,
            "Simplified polygon should have fewer coordinates");
        
        Assert.IsTrue(simplified.IsValid, "Simplified polygon must be valid");
    }
}
```

## Integration Testing Strategy

### 1. End-to-End Planet Generation Tests

```csharp
[TestClass]
public class PlanetGenerationIntegrationTests
{
    [TestMethod]
    public async Task FullPlanetGeneration_EarthLikeParameters_CompletesSuccessfully()
    {
        // Arrange
        var config = new PlanetaryConfig
        {
            RadiusMeters = 6371000,
            PlateCount = 12,
            OceanCoverage = 0.71,
            Seed = 42
        };
        
        var manager = new GeomorphologicalProcessManager();
        var stopwatch = Stopwatch.StartNew();

        // Act
        var result = await Task.Run(() => 
            manager.ExecuteProcessingPipeline(null, config));
        stopwatch.Stop();

        // Assert
        Assert.IsNotNull(result);
        Assert.IsTrue(result.Count > 0, "Must generate polygons");
        Assert.IsTrue(result.All(p => p.IsValid), "All polygons must be valid");
        Assert.IsTrue(result.All(p => p.SRID == 4087), "Must use SRID_METER");
        Assert.IsTrue(stopwatch.Elapsed < TimeSpan.FromMinutes(10), 
            $"Generation took too long: {stopwatch.Elapsed}");
        
        // Validate biome distribution
        var biomeStats = AnalyzeBiomeDistribution(result);
        Assert.IsTrue(biomeStats.OceanCoverage > 0.6 && biomeStats.OceanCoverage < 0.8,
            $"Ocean coverage should be ~71%, got {biomeStats.OceanCoverage:P1}");
    }

    [TestMethod]
    public async Task ProjectionPipeline_AllProjectionTypes_MaintainDataIntegrity()
    {
        // Arrange
        var basePlanet = await GenerateTestPlanet();
        var projections = new[] { "mercator", "robinson", "mollweide", "stereographic" };
        var projector = new MapProjections();

        foreach (var projection in projections)
        {
            // Act
            var projected = projector.ProjectToSpecification(basePlanet, projection);
            
            // Assert
            Assert.IsTrue(projected.All(p => p.IsValid), 
                $"Invalid polygons in {projection} projection");
            
            var totalArea = projected.Sum(p => p.Area);
            var originalArea = basePlanet.Sum(p => p.Area);
            
            // Different projections have different area preservation properties
            var expectedAreaRatio = GetExpectedAreaRatio(projection);
            var actualRatio = totalArea / originalArea;
            var tolerance = 0.1; // 10% tolerance
            
            Assert.IsTrue(Math.Abs(actualRatio - expectedAreaRatio) < tolerance,
                $"{projection} projection area ratio {actualRatio:F2} outside expected range");
        }
    }
}
```

### 2. API Integration Tests

```csharp
[TestClass]
public class PlanetGenerationAPITests
{
    private readonly HttpClient _client;
    
    public PlanetGenerationAPITests()
    {
        _client = new HttpClient { BaseAddress = new Uri("https://api.bluemarble.design/v1/") };
        _client.DefaultRequestHeaders.Authorization = 
            new AuthenticationHeaderValue("Bearer", TestConfiguration.ApiKey);
    }

    [TestMethod]
    public async Task GeneratePlanet_ValidConfiguration_ReturnsSuccess()
    {
        // Arrange
        var request = new
        {
            config = new
            {
                name = "Integration Test Planet",
                radiusMeters = 6371000,
                plateCount = 8,
                oceanCoverage = 0.7,
                seed = 12345
            },
            options = new
            {
                generateBiomes = true,
                applyProjection = "equirectangular"
            }
        };

        // Act
        var response = await _client.PostAsJsonAsync("planet/generate", request);
        
        // Assert
        Assert.AreEqual(HttpStatusCode.Accepted, response.StatusCode);
        
        var result = await response.Content.ReadFromJsonAsync<GenerationResponse>();
        Assert.IsNotNull(result.TaskId);
        Assert.IsNotNull(result.StatusUrl);
        
        // Wait for completion
        var completedPlanet = await WaitForGeneration(result.TaskId);
        Assert.AreEqual("completed", completedPlanet.Status);
        Assert.IsNotNull(completedPlanet.Result.PlanetId);
    }

    [TestMethod]
    public async Task GetPlanetData_ExistingPlanet_ReturnsCorrectFormat()
    {
        // Arrange
        var planetId = await CreateTestPlanet();

        // Act
        var response = await _client.GetAsync($"planet/{planetId}/polygons?format=geojson&limit=100");
        
        // Assert
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        
        var geoJson = await response.Content.ReadFromJsonAsync<FeatureCollection>();
        Assert.AreEqual("FeatureCollection", geoJson.Type);
        Assert.IsTrue(geoJson.Features.Count > 0);
        
        // Validate GeoJSON structure
        foreach (var feature in geoJson.Features)
        {
            Assert.AreEqual("Feature", feature.Type);
            Assert.IsNotNull(feature.Geometry);
            Assert.IsNotNull(feature.Properties);
            Assert.IsTrue(feature.Properties.ContainsKey("biomeType"));
        }
    }
}
```

## Performance Testing Strategy

### 1. Load Testing

```csharp
[TestClass]
public class PerformanceTests
{
    [TestMethod]
    public async Task PlanetGeneration_LargeScale_MeetsPerformanceTargets()
    {
        var testCases = new[]
        {
            new { PlateCount = 8, ExpectedTime = TimeSpan.FromMinutes(5), ExpectedMemory = 2000 },
            new { PlateCount = 12, ExpectedTime = TimeSpan.FromMinutes(8), ExpectedMemory = 3000 },
            new { PlateCount = 20, ExpectedTime = TimeSpan.FromMinutes(15), ExpectedMemory = 4000 }
        };

        foreach (var testCase in testCases)
        {
            // Arrange
            var config = new PlanetaryConfig { PlateCount = testCase.PlateCount };
            var memoryBefore = GC.GetTotalMemory(true);
            var stopwatch = Stopwatch.StartNew();

            // Act
            var result = await GeneratePlanet(config);
            stopwatch.Stop();
            
            var memoryAfter = GC.GetTotalMemory(true);
            var memoryUsed = (memoryAfter - memoryBefore) / 1024 / 1024; // MB

            // Assert
            Assert.IsTrue(stopwatch.Elapsed <= testCase.ExpectedTime,
                $"Generation took {stopwatch.Elapsed}, expected <= {testCase.ExpectedTime}");
            
            Assert.IsTrue(memoryUsed <= testCase.ExpectedMemory,
                $"Used {memoryUsed}MB, expected <= {testCase.ExpectedMemory}MB");
        }
    }

    [TestMethod]
    public async Task ConcurrentGeneration_MultipleRequests_HandlesLoad()
    {
        // Arrange
        var concurrentRequests = 5;
        var tasks = new List<Task<List<Polygon>>>();

        // Act
        for (int i = 0; i < concurrentRequests; i++)
        {
            var config = new PlanetaryConfig { Seed = i, PlateCount = 8 };
            tasks.Add(GeneratePlanet(config));
        }

        var results = await Task.WhenAll(tasks);

        // Assert
        Assert.AreEqual(concurrentRequests, results.Length);
        Assert.IsTrue(results.All(r => r.Count > 0), "All generations should produce results");
        
        // Verify unique results (different seeds should produce different planets)
        var areaCounts = results.Select(r => r.Sum(p => p.Area)).ToArray();
        Assert.IsTrue(areaCounts.Distinct().Count() == concurrentRequests,
            "Different seeds should produce different results");
    }
}
```

### 2. Memory Usage Testing

```csharp
[TestMethod]
public void MemoryUsageProfile_VariousPlanetSizes_WithinLimits()
{
    var testSizes = new[] { 1000, 5000, 10000, 20000 }; // Planet radius in km
    var memoryResults = new List<(int Size, long Memory)>();

    foreach (var radius in testSizes)
    {
        // Force garbage collection before test
        GC.Collect();
        GC.WaitForPendingFinalizers();
        GC.Collect();
        
        var memoryBefore = GC.GetTotalMemory(true);
        
        // Generate planet
        var config = new PlanetaryConfig { RadiusMeters = radius * 1000 };
        var planet = new SphericalPlanetGenerator(config);
        var result = planet.ExecuteProcess(new List<Polygon>(), new List<Polygon>(), new Random());
        
        var memoryAfter = GC.GetTotalMemory(false);
        var memoryUsed = memoryAfter - memoryBefore;
        
        memoryResults.Add((radius, memoryUsed));
        
        // Memory should scale reasonably with planet size
        var memoryMB = memoryUsed / 1024 / 1024;
        Assert.IsTrue(memoryMB < 4000, $"Memory usage {memoryMB}MB exceeds 4GB limit for radius {radius}km");
    }

    // Verify memory scaling is reasonable (not exponential)
    for (int i = 1; i < memoryResults.Count; i++)
    {
        var prev = memoryResults[i - 1];
        var curr = memoryResults[i];
        
        var sizeRatio = (double)curr.Size / prev.Size;
        var memoryRatio = (double)curr.Memory / prev.Memory;
        
        // Memory should not grow faster than size squared
        Assert.IsTrue(memoryRatio <= sizeRatio * sizeRatio * 1.5,
            $"Memory scaling too aggressive: size ratio {sizeRatio:F2}, memory ratio {memoryRatio:F2}");
    }
}
```

## Scientific Validation Testing

### 1. Biome Distribution Validation

```csharp
[TestMethod]
public void BiomeDistribution_EarthReference_ScientificallyAccurate()
{
    // Arrange - Earth reference data
    var earthBiomeDistribution = new Dictionary<BiomeType, double>
    {
        [BiomeType.Ocean] = 0.712,
        [BiomeType.Desert] = 0.143,
        [BiomeType.TropicalRainforest] = 0.062,
        [BiomeType.TemperateForest] = 0.091,
        [BiomeType.BorealForest] = 0.084,
        [BiomeType.Grassland] = 0.081,
        [BiomeType.Tundra] = 0.054
    };

    // Act - Generate Earth-like planet
    var earthConfig = new PlanetaryConfig
    {
        RadiusMeters = 6371000,
        PlateCount = 7, // Major tectonic plates
        OceanCoverage = 0.712,
        Climate = EarthLikeClimate()
    };
    
    var generator = new SphericalPlanetGenerator(earthConfig);
    var result = generator.ExecuteProcess(new List<Polygon>(), new List<Polygon>(), new Random(42));
    var actualDistribution = CalculateBiomeDistribution(result);

    // Assert - Compare to Earth reference within tolerance
    foreach (var expectedBiome in earthBiomeDistribution)
    {
        var actual = actualDistribution.GetValueOrDefault(expectedBiome.Key, 0.0);
        var error = Math.Abs(actual - expectedBiome.Value);
        var tolerance = 0.03; // 3% tolerance
        
        Assert.IsTrue(error <= tolerance,
            $"Biome {expectedBiome.Key}: expected {expectedBiome.Value:P1}, got {actual:P1}, error {error:P1}");
    }
}

[TestMethod]
public void ClimateZones_LatitudinalDistribution_FollowsEarthPattern()
{
    // Arrange
    var generator = new SphericalPlanetGenerator(EarthLikeConfig());
    var result = generator.ExecuteProcess(new List<Polygon>(), new List<Polygon>(), new Random(42));

    // Act - Analyze biome distribution by latitude
    var latitudeBands = new[]
    {
        new { Name = "Tropical", MinLat = -23.5, MaxLat = 23.5, ExpectedBiomes = new[] { BiomeType.TropicalRainforest, BiomeType.TropicalSavanna } },
        new { Name = "Temperate", MinLat = 23.5, MaxLat = 66.5, ExpectedBiomes = new[] { BiomeType.TemperateForest, BiomeType.Grassland } },
        new { Name = "Arctic", MinLat = 66.5, MaxLat = 90.0, ExpectedBiomes = new[] { BiomeType.Tundra, BiomeType.IceSheet } }
    };

    foreach (var band in latitudeBands)
    {
        var bandPolygons = result.Where(p => 
        {
            var lat = ConvertYToLatitude(p.Centroid.Y);
            return lat >= band.MinLat && lat <= band.MaxLat;
        });

        var bandBiomes = bandPolygons
            .Where(p => p.UserData is Dictionary<string, object> metadata && 
                       metadata.ContainsKey("BiomeType"))
            .Select(p => (BiomeType)((Dictionary<string, object>)p.UserData)["BiomeType"])
            .GroupBy(b => b)
            .ToDictionary(g => g.Key, g => g.Count());

        // Assert - Expected biomes should be predominant in their zones
        foreach (var expectedBiome in band.ExpectedBiomes)
        {
            var count = bandBiomes.GetValueOrDefault(expectedBiome, 0);
            var totalCount = bandBiomes.Values.Sum();
            var proportion = (double)count / totalCount;
            
            Assert.IsTrue(proportion > 0.1, // At least 10% presence
                $"{expectedBiome} should be present in {band.Name} zone (got {proportion:P1})");
        }
    }
}
```

### 2. Mathematical Accuracy Validation

```csharp
[TestMethod]
public void ProjectionAccuracy_ReferenceCoordinates_WithinTolerance()
{
    // Arrange - Use known geographic survey points
    var referencePoints = LoadNationalGeodeticSurveyPoints();
    var projections = new[] { "mercator", "robinson", "mollweide" };
    var projector = new MapProjections();

    foreach (var projection in projections)
    {
        var errors = new List<double>();
        
        foreach (var point in referencePoints)
        {
            // Act
            var projected = projector.Project(point.Spherical, projection);
            var backProjected = projector.Unproject(projected, projection);
            
            // Calculate error
            var distance = CalculateSphericalDistance(point.Spherical, backProjected);
            errors.Add(distance);
        }

        // Assert
        var maxError = errors.Max();
        var averageError = errors.Average();
        var tolerance = GetProjectionTolerance(projection);
        
        Assert.IsTrue(maxError < tolerance,
            $"{projection} projection max error {maxError:F2}m exceeds tolerance {tolerance}m");
        
        Assert.IsTrue(averageError < tolerance / 2,
            $"{projection} projection average error {averageError:F2}m too high");
    }
}
```

## Automated Testing Infrastructure

### 1. Continuous Integration Pipeline

```yaml
# azure-pipelines.yml
trigger:
  branches:
    include:
      - main
      - develop
  paths:
    include:
      - src/SphericalPlanet/*
      - tests/SphericalPlanet/*

pool:
  vmImage: 'ubuntu-latest'

variables:
  buildConfiguration: 'Release'
  testResultsDirectory: '$(Agent.TempDirectory)/TestResults'

stages:
- stage: Build
  jobs:
  - job: BuildAndTest
    steps:
    - task: UseDotNet@2
      inputs:
        packageType: 'sdk'
        version: '8.0.x'
    
    - task: DotNetCoreCLI@2
      displayName: 'Restore packages'
      inputs:
        command: 'restore'
        projects: '**/*.csproj'
    
    - task: DotNetCoreCLI@2
      displayName: 'Build solution'
      inputs:
        command: 'build'
        projects: '**/*.csproj'
        arguments: '--configuration $(buildConfiguration) --no-restore'
    
    - task: DotNetCoreCLI@2
      displayName: 'Run unit tests'
      inputs:
        command: 'test'
        projects: '**/Tests.Unit.csproj'
        arguments: '--configuration $(buildConfiguration) --no-build --collect:"XPlat Code Coverage" --results-directory $(testResultsDirectory)'
    
    - task: DotNetCoreCLI@2
      displayName: 'Run integration tests'
      inputs:
        command: 'test'
        projects: '**/Tests.Integration.csproj'
        arguments: '--configuration $(buildConfiguration) --no-build --results-directory $(testResultsDirectory)'
      condition: and(succeeded(), eq(variables['Build.SourceBranch'], 'refs/heads/main'))
    
    - task: PublishCodeCoverageResults@1
      displayName: 'Publish code coverage'
      inputs:
        codeCoverageTool: 'Cobertura'
        summaryFileLocation: '$(testResultsDirectory)/**/coverage.cobertura.xml'

- stage: PerformanceTest
  dependsOn: Build
  condition: and(succeeded(), eq(variables['Build.SourceBranch'], 'refs/heads/main'))
  jobs:
  - job: PerformanceValidation
    pool:
      vmImage: 'windows-latest' # Use Windows for performance consistency
    steps:
    - task: DotNetCoreCLI@2
      displayName: 'Run performance tests'
      inputs:
        command: 'test'
        projects: '**/Tests.Performance.csproj'
        arguments: '--configuration Release --logger "trx;LogFileName=performance-results.trx"'
    
    - task: PublishTestResults@2
      displayName: 'Publish performance results'
      inputs:
        testResultsFormat: 'VSTest'
        testResultsFiles: '**/performance-results.trx'
        mergeTestResults: true
```

### 2. Test Data Management

```csharp
public static class TestDataManager
{
    private static readonly string TestDataPath = Path.Combine(
        AppDomain.CurrentDomain.BaseDirectory, "TestData");

    public static PlanetaryConfig EarthLikeConfig() => new PlanetaryConfig
    {
        RadiusMeters = 6371000,
        PlateCount = 7,
        OceanCoverage = 0.712,
        Seed = 42,
        Climate = new ClimateParameters
        {
            GlobalTemperature = 15.0,
            TemperatureVariation = 40.0,
            PrecipitationBase = 1000.0,
            SeasonalVariation = 0.2
        }
    };

    public static List<ReferenceCoordinate> LoadReferenceCoordinates()
    {
        var filePath = Path.Combine(TestDataPath, "reference-coordinates.json");
        var json = File.ReadAllText(filePath);
        return JsonSerializer.Deserialize<List<ReferenceCoordinate>>(json);
    }

    public static BiomeDistribution LoadEarthBiomeReference()
    {
        var filePath = Path.Combine(TestDataPath, "earth-biome-distribution.json");
        var json = File.ReadAllText(filePath);
        return JsonSerializer.Deserialize<BiomeDistribution>(json);
    }

    public static void SaveTestResults(string testName, object results)
    {
        var resultsPath = Path.Combine(TestDataPath, "Results");
        Directory.CreateDirectory(resultsPath);
        
        var fileName = $"{testName}-{DateTime.UtcNow:yyyyMMdd-HHmmss}.json";
        var filePath = Path.Combine(resultsPath, fileName);
        
        var json = JsonSerializer.Serialize(results, new JsonSerializerOptions 
        { 
            WriteIndented = true 
        });
        File.WriteAllText(filePath, json);
    }
}
```

### 3. Test Reporting and Metrics

```csharp
public class TestMetricsCollector
{
    private readonly List<TestResult> _results = new();

    public void RecordTest(string testName, TimeSpan duration, bool passed, 
        Dictionary<string, object> metrics = null)
    {
        _results.Add(new TestResult
        {
            TestName = testName,
            Duration = duration,
            Passed = passed,
            Timestamp = DateTime.UtcNow,
            Metrics = metrics ?? new Dictionary<string, object>()
        });
    }

    public TestSummary GenerateSummary()
    {
        return new TestSummary
        {
            TotalTests = _results.Count,
            PassedTests = _results.Count(r => r.Passed),
            FailedTests = _results.Count(r => !r.Passed),
            AverageDuration = TimeSpan.FromMilliseconds(
                _results.Average(r => r.Duration.TotalMilliseconds)),
            TotalDuration = TimeSpan.FromMilliseconds(
                _results.Sum(r => r.Duration.TotalMilliseconds)),
            PerformanceMetrics = ExtractPerformanceMetrics(),
            CoverageMetrics = ExtractCoverageMetrics()
        };
    }

    private Dictionary<string, double> ExtractPerformanceMetrics()
    {
        var performanceTests = _results.Where(r => 
            r.Metrics.ContainsKey("MemoryUsageMB") || 
            r.Metrics.ContainsKey("ThroughputOpsPerSec"));

        return new Dictionary<string, double>
        {
            ["AverageMemoryUsageMB"] = performanceTests
                .Where(r => r.Metrics.ContainsKey("MemoryUsageMB"))
                .Average(r => (double)r.Metrics["MemoryUsageMB"]),
            ["AverageThroughput"] = performanceTests
                .Where(r => r.Metrics.ContainsKey("ThroughputOpsPerSec"))
                .Average(r => (double)r.Metrics["ThroughputOpsPerSec"])
        };
    }
}
```

## Quality Gates and Success Criteria

### Definition of Done
- [ ] All unit tests pass with >95% code coverage
- [ ] All integration tests pass
- [ ] Performance tests meet specified benchmarks
- [ ] Mathematical accuracy tests pass with <1m tolerance
- [ ] Scientific validation shows >95% accuracy
- [ ] Memory usage tests pass within 4GB limit
- [ ] API tests validate all endpoints
- [ ] Documentation updated and reviewed

### Release Criteria
- [ ] Zero critical bugs
- [ ] <5 high-priority bugs
- [ ] Performance regression <5% from baseline
- [ ] All automated tests passing for 7 consecutive days
- [ ] Manual testing completed by QA team
- [ ] Security review completed
- [ ] Load testing validates production capacity

This comprehensive testing strategy ensures the Spherical Planet Generation system meets both functional and non-functional requirements while maintaining scientific accuracy and integration compatibility.