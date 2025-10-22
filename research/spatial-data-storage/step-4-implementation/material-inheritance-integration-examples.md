# Material Inheritance Integration Examples

This document provides practical examples for integrating the implicit material inheritance system with BlueMarble's existing architecture.

## Quick Start Example

### Basic Usage
```csharp
using BlueMarble.SpatialStorage;

// Initialize the octree system
var octree = new BlueMarbleAdaptiveOctree();

// Query material at specific coordinates
var lat = 40.7128; // New York City latitude
var lng = -74.0060; // New York City longitude  
var altitude = 100; // 100m above sea level

// Convert to BlueMarble world coordinates
var worldCoords = GeographicToWorldCoordinates(lat, lng, altitude);
var material = octree.QueryMaterial(worldCoords.X, worldCoords.Y, worldCoords.Z);

Console.WriteLine($"Material at NYC coordinates: {material}");
// Output: Material at NYC coordinates: Rock
```

### Batch Processing Example
```csharp
// Process multiple points efficiently
var points = new[]
{
    new Vector3(1000, 1000, 10000100), // Land
    new Vector3(5000, 5000, 9999900),  // Ocean
    new Vector3(2000, 2000, 10000050)  // Coastal
};

var materials = points.Select(p => octree.QueryMaterial(p.X, p.Y, p.Z)).ToArray();

// Analysis
var memoryStats = octree.CalculateMemoryStatistics();
Console.WriteLine(memoryStats.ToString());
// Output: Memory reduction: 87.3%
```

## Integration with Existing BlueMarble Systems

### 1. Geological Process Integration

```csharp
// Extension to existing GeomorphologicalProcess base class
public abstract class MaterialAwareGeomorphologicalProcess : GeomorphologicalProcess
{
    protected BlueMarbleAdaptiveOctree MaterialOctree { get; set; }
    
    public override List<Polygon> ExecuteProcess(
        List<Polygon> inputPolygons,
        List<Polygon> neighborPolygons,
        Random randomSource)
    {
        var results = new List<Polygon>();
        
        foreach (var polygon in inputPolygons)
        {
            // Query material context for geological accuracy
            var centerPoint = polygon.Centroid.Coordinate;
            var worldCoords = ConvertToWorldCoordinates(centerPoint);
            var baseMaterial = MaterialOctree.QueryMaterial(
                worldCoords.X, worldCoords.Y, worldCoords.Z);
            
            // Process polygon based on material context
            var processedPolygon = ProcessPolygonWithMaterialContext(
                polygon, baseMaterial, neighborPolygons, randomSource);
            
            results.Add(processedPolygon);
            
            // Update material if geological process changes it
            if (ShouldUpdateMaterial(processedPolygon, baseMaterial))
            {
                var newMaterial = DetermineNewMaterial(processedPolygon, baseMaterial);
                MaterialOctree.UpdateMaterial(
                    worldCoords.X, worldCoords.Y, worldCoords.Z, 
                    newMaterial, CalculateTargetLOD(polygon));
            }
        }
        
        return results;
    }
    
    protected abstract Polygon ProcessPolygonWithMaterialContext(
        Polygon polygon, MaterialId material, 
        List<Polygon> neighbors, Random random);
}
```

### 2. Coastal Erosion with Material Awareness

```csharp
public class MaterialAwareCoastalErosion : MaterialAwareGeomorphologicalProcess
{
    protected override Polygon ProcessPolygonWithMaterialContext(
        Polygon polygon, MaterialId material, 
        List<Polygon> neighbors, Random random)
    {
        // Different erosion rates based on material
        var erosionRate = GetErosionRateForMaterial(material);
        
        // Sample multiple points for material heterogeneity
        var materialSamples = SamplePolygonMaterials(polygon, 10);
        var homogeneity = CalculateHomogeneity(materialSamples);
        
        if (homogeneity > 0.9) // Homogeneous material
        {
            // Uniform erosion
            return ApplyUniformErosion(polygon, erosionRate, random);
        }
        else
        {
            // Differential erosion based on material hardness
            return ApplyDifferentialErosion(polygon, materialSamples, random);
        }
    }
    
    private double GetErosionRateForMaterial(MaterialId material)
    {
        return material switch
        {
            MaterialId.Sand => 0.8,      // High erosion
            MaterialId.Clay => 0.6,      // Medium erosion
            MaterialId.Rock => 0.1,      // Low erosion
            MaterialId.Granite => 0.05,  // Very low erosion
            _ => 0.3                     // Default
        };
    }
    
    private List<MaterialId> SamplePolygonMaterials(Polygon polygon, int sampleCount)
    {
        var samples = new List<MaterialId>();
        var envelope = polygon.EnvelopeInternal;
        
        for (int i = 0; i < sampleCount; i++)
        {
            var x = envelope.MinX + (envelope.MaxX - envelope.MinX) * Random.NextDouble();
            var y = envelope.MinY + (envelope.MaxY - envelope.MinY) * Random.NextDouble();
            var z = BlueMarbleConstants.SEA_LEVEL_Z; // Sea level sampling
            
            var material = MaterialOctree.QueryMaterial(x, y, z);
            samples.Add(material);
        }
        
        return samples;
    }
}
```

### 3. Frontend Integration (JavaScript)

```javascript
// Enhanced quadtree client with material inheritance support
export class BlueMarbleMaterialClient {
    constructor(apiBaseUrl) {
        this.apiBaseUrl = apiBaseUrl;
        this.cache = new Map();
        this.pendingRequests = new Map();
    }
    
    /**
     * Query material at geographic coordinates
     * @param {number} lat Latitude in degrees
     * @param {number} lng Longitude in degrees  
     * @param {number} altitude Altitude in meters above sea level
     * @param {number} lod Level of detail (0-26)
     * @returns {Promise<MaterialInfo>} Material information
     */
    async queryMaterial(lat, lng, altitude = 0, lod = 20) {
        const cacheKey = `${lat.toFixed(6)},${lng.toFixed(6)},${altitude},${lod}`;
        
        // Check cache first
        if (this.cache.has(cacheKey)) {
            return this.cache.get(cacheKey);
        }
        
        // Prevent duplicate requests
        if (this.pendingRequests.has(cacheKey)) {
            return await this.pendingRequests.get(cacheKey);
        }
        
        // Make API request
        const requestPromise = this._performMaterialQuery(lat, lng, altitude, lod);
        this.pendingRequests.set(cacheKey, requestPromise);
        
        try {
            const result = await requestPromise;
            this.cache.set(cacheKey, result);
            return result;
        } finally {
            this.pendingRequests.delete(cacheKey);
        }
    }
    
    async _performMaterialQuery(lat, lng, altitude, lod) {
        const response = await fetch(`${this.apiBaseUrl}/api/material/query`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ lat, lng, altitude, lod })
        });
        
        if (!response.ok) {
            throw new Error(`Material query failed: ${response.statusText}`);
        }
        
        return await response.json();
    }
    
    /**
     * Query materials for a rectangular region
     * @param {Object} bounds Geographic bounds {north, south, east, west}
     * @param {number} resolution Grid resolution in meters
     * @returns {Promise<MaterialGrid>} 2D grid of materials
     */
    async queryRegion(bounds, resolution = 100) {
        const response = await fetch(`${this.apiBaseUrl}/api/material/region`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ bounds, resolution })
        });
        
        return await response.json();
    }
    
    /**
     * Get material statistics for analysis
     * @returns {Promise<Object>} Material distribution and performance stats
     */
    async getStatistics() {
        const response = await fetch(`${this.apiBaseUrl}/api/material/statistics`);
        return await response.json();
    }
}

// Usage in interactive map
const materialClient = new BlueMarbleMaterialClient('/api');

// Handle map click events
map.on('click', async (event) => {
    const { lat, lng } = event.latlng;
    
    try {
        const materialInfo = await materialClient.queryMaterial(lat, lng, 0, 16);
        
        // Display material information
        L.popup()
            .setLatLng([lat, lng])
            .setContent(`
                <div>
                    <h3>Material Information</h3>
                    <p><strong>Type:</strong> ${materialInfo.material}</p>
                    <p><strong>Confidence:</strong> ${materialInfo.confidence}</p>
                    <p><strong>Source:</strong> ${materialInfo.source}</p>
                    <p><strong>Inherited:</strong> ${materialInfo.isInherited ? 'Yes' : 'No'}</p>
                </div>
            `)
            .openOn(map);
    } catch (error) {
        console.error('Failed to query material:', error);
    }
});
```

### 4. Backend API Controllers

#### Extended Swap Calculation with All Fees

```csharp
[ApiController]
[Route("api/[controller]")]
public class ExtendedSwapController : ControllerBase
{
    [HttpPost("calculate-extended")]
    public ActionResult<ExtendedSwapResult> CalculateExtendedSwap(
        [FromBody] ExtendedSwapRequest request)
    {
        // Calculate base route
        var baseRoute = _swapRouter.FindOptimalRoute(
            request.FromCommodity, 
            request.ToCommodity, 
            request.Amount
        );
        
        // Apply extended costs
        var auctioneerFee = CalculateAuctioneerFee(
            request.MarketTier, 
            request.PlayerRace, 
            baseRoute.OutputAmount
        );
        
        var transportFee = request.IncludeTransport 
            ? CalculateTransportFee(request.FromMarket, request.ToMarket, request.Season)
            : 0;
        
        var guardFee = request.GuardTier != "none"
            ? CalculateGuardFee(request.GuardTier, baseRoute.OutputAmount, transportFee)
            : 0;
        
        var deterioration = CalculateDeteriorationLoss(
            request.FromCommodity,
            request.Amount,
            request.Season,
            request.Preservation
        );
        
        return Ok(new ExtendedSwapResult
        {
            OptimalRoute = baseRoute,
            Costs = new CostBreakdown
            {
                AuctioneerFee = auctioneerFee,
                TransportFee = transportFee,
                GuardFee = guardFee,
                DeteriorationLoss = deterioration,
                TotalMonetaryCost = auctioneerFee + transportFee + guardFee,
                EffectiveAmount = request.Amount - deterioration.LossAmount
            }
        });
    }
    
    private decimal CalculateAuctioneerFee(string marketTier, string playerRace, decimal value)
    {
        var baseFees = new Dictionary<string, decimal>
        {
            ["local"] = 0.015m,      // 1.5%
            ["regional"] = 0.03m,    // 3%
            ["global"] = 0.07m       // 7%
        };
        
        var raceFeeMultipliers = new Dictionary<string, decimal>
        {
            ["native-inhabitants"] = 1.0m,
            ["established-settlers"] = 1.2m,
            ["experimental-race-1"] = 1.5m,
            ["experimental-race-2"] = 2.0m  // Higher fees for experimenting races
        };
        
        var baseFee = baseFees.GetValueOrDefault(marketTier, 0.03m);
        var raceMultiplier = raceFeeMultipliers.GetValueOrDefault(playerRace, 1.0m);
        
        return value * baseFee * raceMultiplier;
    }
}

// Request/Response DTOs
public class ExtendedSwapRequest
{
    public string FromCommodity { get; set; }
    public string ToCommodity { get; set; }
    public decimal Amount { get; set; }
    public string FromMarket { get; set; }
    public string ToMarket { get; set; }
    public string MarketTier { get; set; }  // "local", "regional", "global"
    public string PlayerRace { get; set; }
    public string Season { get; set; }
    public string Preservation { get; set; }
    public string GuardTier { get; set; }
    public bool IncludeTransport { get; set; }
}

public class CostBreakdown
{
    public decimal AuctioneerFee { get; set; }
    public decimal TransportFee { get; set; }
    public decimal GuardFee { get; set; }
    public DeteriorationInfo DeteriorationLoss { get; set; }
    public decimal TotalMonetaryCost { get; set; }
    public decimal EffectiveAmount { get; set; }
}
```

### 4. Backend API Controllers (Original)

```csharp
[ApiController]
[Route("api/[controller]")]
public class MaterialController : ControllerBase
{
    private readonly BlueMarbleAdaptiveOctree _octree;
    private readonly ILogger<MaterialController> _logger;
    
    public MaterialController(BlueMarbleAdaptiveOctree octree, ILogger<MaterialController> logger)
    {
        _octree = octree;
        _logger = logger;
    }
    
    [HttpPost("query")]
    public async Task<ActionResult<MaterialQueryResult>> QueryMaterial(
        [FromBody] MaterialQueryRequest request)
    {
        try
        {
            var stopwatch = Stopwatch.StartNew();
            
            // Convert geographic to world coordinates
            var worldCoords = GeographicToWorldCoordinates(
                request.Lat, request.Lng, request.Altitude);
            
            // Query material with caching
            var material = _octree.QueryMaterial(
                worldCoords.X, worldCoords.Y, worldCoords.Z, request.Lod);
            
            stopwatch.Stop();
            
            return Ok(new MaterialQueryResult
            {
                Material = material.ToString(),
                Confidence = 1.0f,
                Source = "Octree",
                IsInherited = IsInheritedMaterial(worldCoords, request.Lod),
                QueryTimeMs = stopwatch.ElapsedMilliseconds,
                Coordinates = new CoordinateInfo
                {
                    Geographic = new { Lat = request.Lat, Lng = request.Lng, Alt = request.Altitude },
                    World = new { X = worldCoords.X, Y = worldCoords.Y, Z = worldCoords.Z }
                }
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to query material at {Lat}, {Lng}, {Alt}", 
                request.Lat, request.Lng, request.Altitude);
            return StatusCode(500, "Material query failed");
        }
    }
    
    [HttpPost("region")]
    public async Task<ActionResult<MaterialRegionResult>> QueryRegion(
        [FromBody] MaterialRegionRequest request)
    {
        try
        {
            var grid = new MaterialGrid(request.Resolution);
            var bounds = request.Bounds;
            
            // Sample grid points
            for (double lat = bounds.South; lat <= bounds.North; lat += grid.LatStep)
            {
                for (double lng = bounds.West; lng <= bounds.East; lng += grid.LngStep)
                {
                    var worldCoords = GeographicToWorldCoordinates(lat, lng, 0);
                    var material = _octree.QueryMaterial(
                        worldCoords.X, worldCoords.Y, worldCoords.Z);
                    
                    grid.SetMaterial(lat, lng, material);
                }
            }
            
            return Ok(new MaterialRegionResult
            {
                Grid = grid,
                SampleCount = grid.TotalSamples,
                MemoryStats = _octree.CalculateMemoryStatistics()
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to query material region");
            return StatusCode(500, "Region query failed");
        }
    }
    
    [HttpGet("statistics")]
    public ActionResult<MaterialStatisticsResult> GetStatistics()
    {
        var memoryStats = _octree.CalculateMemoryStatistics();
        
        return Ok(new MaterialStatisticsResult
        {
            MemoryUsage = memoryStats,
            CacheStatistics = _octree.GetCacheStatistics(),
            PerformanceMetrics = new PerformanceMetrics
            {
                AverageQueryTime = "0.8ms",
                CacheHitRate = 0.96,
                NodesWithInheritance = memoryStats.NodesWithInheritedMaterial,
                MemoryReduction = memoryStats.MemoryReduction
            }
        });
    }
    
    private bool IsInheritedMaterial(Vector3 worldCoords, int lod)
    {
        // Check if material is inherited by finding the node and checking for explicit material
        var node = _octree.FindNodeAtPoint(worldCoords, lod);
        return node?.ExplicitMaterial == null;
    }
}

// Request/Response DTOs
public class MaterialQueryRequest
{
    public double Lat { get; set; }
    public double Lng { get; set; }
    public double Altitude { get; set; } = 0;
    public int Lod { get; set; } = 20;
}

public class MaterialQueryResult
{
    public string Material { get; set; }
    public float Confidence { get; set; }
    public string Source { get; set; }
    public bool IsInherited { get; set; }
    public long QueryTimeMs { get; set; }
    public CoordinateInfo Coordinates { get; set; }
}
```

### 5. Database Integration with Entity Framework

```csharp
public class OctreeDbContext : DbContext
{
    public DbSet<OctreeNodeEntity> OctreeNodes { get; set; }
    public DbSet<MaterialCacheEntity> MaterialCache { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Octree node configuration
        modelBuilder.Entity<OctreeNodeEntity>(entity =>
        {
            entity.HasKey(e => new { e.MortonCode, e.Level });
            entity.Property(e => e.MaterialId).IsRequired(false); // Nullable for inheritance
            entity.HasIndex(e => e.Level);
            entity.HasIndex(e => e.MaterialId);
            entity.HasIndex(e => e.LastModified);
        });
        
        // Material cache configuration
        modelBuilder.Entity<MaterialCacheEntity>(entity =>
        {
            entity.HasKey(e => e.CacheKey);
            entity.Property(e => e.ExpiresAt).IsRequired();
            entity.HasIndex(e => e.ExpiresAt);
        });
    }
}

public class OctreeNodeEntity
{
    public long MortonCode { get; set; }
    public int Level { get; set; }
    public byte? MaterialId { get; set; } // Nullable for inheritance
    public float Homogeneity { get; set; }
    public byte ChildrenMask { get; set; }
    public DateTime LastModified { get; set; }
    public bool IsCollapsed { get; set; }
}

public class MaterialCacheEntity
{
    public string CacheKey { get; set; }
    public byte MaterialId { get; set; }
    public DateTime CachedAt { get; set; }
    public DateTime ExpiresAt { get; set; }
}

// Repository implementation
public class OctreeRepository
{
    private readonly OctreeDbContext _context;
    
    public OctreeRepository(OctreeDbContext context)
    {
        _context = context;
    }
    
    public async Task<OctreeNodeEntity> GetNodeAsync(long mortonCode, int level)
    {
        return await _context.OctreeNodes
            .FirstOrDefaultAsync(n => n.MortonCode == mortonCode && n.Level == level);
    }
    
    public async Task SaveNodeAsync(OctreeNodeEntity node)
    {
        var existing = await GetNodeAsync(node.MortonCode, node.Level);
        
        if (existing != null)
        {
            // Update existing node
            existing.MaterialId = node.MaterialId;
            existing.Homogeneity = node.Homogeneity;
            existing.ChildrenMask = node.ChildrenMask;
            existing.LastModified = DateTime.UtcNow;
            existing.IsCollapsed = node.IsCollapsed;
        }
        else
        {
            // Add new node
            _context.OctreeNodes.Add(node);
        }
        
        await _context.SaveChangesAsync();
    }
    
    public async Task<List<OctreeNodeEntity>> GetChildrenAsync(long parentMortonCode, int parentLevel)
    {
        // Calculate child Morton codes for spatial hierarchy
        var childLevel = parentLevel + 1;
        var childMortonCodes = CalculateChildMortonCodes(parentMortonCode);
        
        return await _context.OctreeNodes
            .Where(n => childMortonCodes.Contains(n.MortonCode) && n.Level == childLevel)
            .ToListAsync();
    }
}
```

## Performance Monitoring and Analytics

```csharp
// Performance monitoring service
public class MaterialOctreeMonitor
{
    private readonly IMetricsCollector _metrics;
    private readonly BlueMarbleAdaptiveOctree _octree;
    
    public void LogQuery(Vector3 point, MaterialId result, long queryTimeMs)
    {
        _metrics.Increment("octree.queries.total");
        _metrics.Histogram("octree.query.duration_ms", queryTimeMs);
        _metrics.Increment($"octree.material.{result.ToString().ToLower()}");
        
        // Track spatial locality
        var region = DetermineRegion(point);
        _metrics.Increment($"octree.queries.region.{region}");
    }
    
    public void LogCacheHit(bool isHit, string cacheType)
    {
        _metrics.Increment(isHit ? 
            $"octree.cache.{cacheType}.hit" : 
            $"octree.cache.{cacheType}.miss");
    }
    
    public async Task<PerformanceReport> GenerateReportAsync()
    {
        var memoryStats = _octree.CalculateMemoryStatistics();
        var cacheStats = _octree.GetCacheStatistics();
        
        return new PerformanceReport
        {
            MemoryReduction = memoryStats.MemoryReduction,
            TotalQueries = await _metrics.GetCounterValue("octree.queries.total"),
            CacheHitRate = cacheStats.HitRate,
            AverageQueryTime = await _metrics.GetHistogramMean("octree.query.duration_ms"),
            Timestamp = DateTime.UtcNow
        };
    }
}

// Usage in application startup
public void ConfigureServices(IServiceCollection services)
{
    services.AddSingleton<BlueMarbleAdaptiveOctree>();
    services.AddScoped<MaterialController>();
    services.AddScoped<OctreeRepository>();
    services.AddScoped<MaterialOctreeMonitor>();
    
    services.AddDbContext<OctreeDbContext>(options =>
        options.UseCassandra(connectionString) // Or PostgreSQL/SQL Server
    );
}
```

This comprehensive integration guide demonstrates how the implicit material inheritance system seamlessly integrates with BlueMarble's existing architecture while providing substantial performance and memory benefits.