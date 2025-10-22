# Grid + Vector Hybrid Storage: API Integration Examples

## Executive Summary

This document provides practical API integration examples for the Grid + Vector hybrid storage system within the BlueMarble ecosystem. It demonstrates RESTful endpoints, WebSocket integration, client SDKs, and real-world usage patterns.

## RESTful API Endpoints

### 1. Grid Management API

#### Create Dense Simulation Grid

```http
POST /api/spatial/grids
Content-Type: application/json
Authorization: Bearer {token}

{
  "regionId": "north-america-coastal",
  "bounds": {
    "minX": -125.0,
    "minY": 32.0,
    "maxX": -117.0,
    "maxY": 42.0
  },
  "cellSize": 100.0,
  "initialMaterials": "from-octree"
}
```

**Response:**
```json
{
  "regionId": "north-america-coastal",
  "gridId": "grid_a7b3c9d1",
  "dimensions": {
    "width": 800,
    "height": 1000
  },
  "cellSize": 100.0,
  "totalCells": 800000,
  "memoryUsageMB": 12.5,
  "createdAt": "2025-10-21T17:00:00Z",
  "status": "ready"
}
```

**Implementation:**
```csharp
[ApiController]
[Route("api/spatial/grids")]
public class GridManagementController : ControllerBase
{
    private readonly GridVectorHybridStorage _storage;
    private readonly ILogger<GridManagementController> _logger;
    
    [HttpPost]
    [ProducesResponseType(typeof(GridCreationResponse), 201)]
    [ProducesResponseType(400)]
    [ProducesResponseType(409)]
    public async Task<ActionResult<GridCreationResponse>> CreateGrid(
        [FromBody] CreateGridRequest request)
    {
        try
        {
            // Validate bounds
            if (!IsValidBounds(request.Bounds))
            {
                return BadRequest("Invalid bounds specified");
            }
            
            // Check if grid already exists
            var existing = _storage.GetGrid(request.RegionId);
            if (existing != null)
            {
                return Conflict($"Grid already exists for region: {request.RegionId}");
            }
            
            // Create grid
            var grid = _storage.GetOrCreateGrid(
                request.RegionId,
                ToEnvelope(request.Bounds),
                request.CellSize
            );
            
            // Initialize from octree if requested
            if (request.InitialMaterials == "from-octree")
            {
                await InitializeGridFromOctree(grid);
            }
            
            _logger.LogInformation(
                "Created grid {RegionId} with {Width}x{Height} cells",
                grid.RegionId, grid.Size.Width, grid.Size.Height
            );
            
            return CreatedAtAction(
                nameof(GetGrid),
                new { regionId = grid.RegionId },
                ToResponse(grid)
            );
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating grid");
            return StatusCode(500, "Internal server error");
        }
    }
    
    [HttpGet("{regionId}")]
    [ProducesResponseType(typeof(GridDetailsResponse), 200)]
    [ProducesResponseType(404)]
    public ActionResult<GridDetailsResponse> GetGrid(string regionId)
    {
        var grid = _storage.GetGrid(regionId);
        
        if (grid == null)
        {
            return NotFound($"Grid not found: {regionId}");
        }
        
        return Ok(ToDetailsResponse(grid));
    }
    
    [HttpDelete("{regionId}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public ActionResult DeleteGrid(string regionId)
    {
        var grid = _storage.GetGrid(regionId);
        if (grid == null)
        {
            return NotFound();
        }
        
        _storage.RemoveGrid(regionId);
        
        _logger.LogInformation("Deleted grid {RegionId}", regionId);
        
        return NoContent();
    }
}
```

#### Query Grid Data

```http
POST /api/spatial/grids/{regionId}/query
Content-Type: application/json

{
  "queryType": "region",
  "bounds": {
    "minX": -120.0,
    "minY": 35.0,
    "maxX": -119.0,
    "maxY": 36.0
  },
  "layers": ["material", "elevation", "temperature"],
  "format": "geojson"
}
```

**Response:**
```json
{
  "regionId": "north-america-coastal",
  "queryBounds": {
    "minX": -120.0,
    "minY": 35.0,
    "maxX": -119.0,
    "maxY": 36.0
  },
  "dimensions": {
    "width": 100,
    "height": 100
  },
  "layers": {
    "material": {
      "encoding": "base64-rle",
      "data": "AAEBAgME..."
    },
    "elevation": {
      "encoding": "float32-array",
      "min": 0.0,
      "max": 1250.5,
      "data": "3.14,15.9,26.5..."
    },
    "temperature": {
      "encoding": "float32-array",
      "min": -5.0,
      "max": 35.0,
      "data": "20.5,21.3,19.8..."
    }
  },
  "queriedAt": "2025-10-21T17:05:00Z",
  "queryTimeMs": 45
}
```

### 2. Vector Boundary API

#### Add or Update Boundary

```http
POST /api/spatial/boundaries
Content-Type: application/json

{
  "id": "san-andreas-fault",
  "type": "fault",
  "geometry": {
    "type": "LineString",
    "coordinates": [
      [-121.0, 36.0],
      [-120.5, 36.5],
      [-120.0, 37.0]
    ]
  },
  "properties": {
    "name": "San Andreas Fault",
    "faultType": "strike-slip",
    "priority": 0.95
  },
  "materialTransition": {
    "leftMaterial": 15,
    "rightMaterial": 18,
    "transitionType": "sharp",
    "blendWidth": 50.0
  }
}
```

**Response:**
```json
{
  "id": "san-andreas-fault",
  "status": "created",
  "affectedGrids": ["north-america-coastal", "california-central"],
  "synchronizedCells": 1523,
  "processingTimeMs": 125
}
```

**Implementation:**
```csharp
[ApiController]
[Route("api/spatial/boundaries")]
public class BoundaryManagementController : ControllerBase
{
    private readonly GridVectorHybridStorage _storage;
    private readonly ILogger<BoundaryManagementController> _logger;
    
    [HttpPost]
    [ProducesResponseType(typeof(BoundaryOperationResponse), 200)]
    [ProducesResponseType(typeof(BoundaryOperationResponse), 201)]
    [ProducesResponseType(400)]
    public async Task<ActionResult<BoundaryOperationResponse>> AddOrUpdateBoundary(
        [FromBody] BoundaryRequest request)
    {
        try
        {
            var stopwatch = Stopwatch.StartNew();
            
            // Convert from GeoJSON to NTS geometry
            var geometry = GeoJsonToGeometry(request.Geometry);
            
            // Create boundary feature
            var boundary = new BoundaryFeature
            {
                Id = request.Id,
                Geometry = geometry,
                Type = ParseBoundaryType(request.Type),
                Priority = request.Properties.GetValueOrDefault("priority", 0.5),
                MaterialTransition = new MaterialTransition
                {
                    LeftMaterial = new MaterialId(request.MaterialTransition.LeftMaterial),
                    RightMaterial = new MaterialId(request.MaterialTransition.RightMaterial),
                    Type = ParseTransitionType(request.MaterialTransition.TransitionType),
                    BlendWidth = request.MaterialTransition.BlendWidth
                },
                Properties = request.Properties
            };
            
            // Check if boundary exists
            var existing = _storage._precisionBoundaries.GetBoundary(request.Id);
            var isUpdate = existing != null;
            
            // Add or update boundary (this triggers grid synchronization)
            _storage.AddOrUpdateBoundary(boundary);
            
            // Find affected grids
            var affectedGrids = FindAffectedGrids(geometry.EnvelopeInternal);
            var synchronizedCells = CountSynchronizedCells(boundary, affectedGrids);
            
            stopwatch.Stop();
            
            _logger.LogInformation(
                "{Action} boundary {Id}, affected {GridCount} grids, {CellCount} cells",
                isUpdate ? "Updated" : "Created",
                request.Id,
                affectedGrids.Count,
                synchronizedCells
            );
            
            var response = new BoundaryOperationResponse
            {
                Id = request.Id,
                Status = isUpdate ? "updated" : "created",
                AffectedGrids = affectedGrids.Select(g => g.RegionId).ToList(),
                SynchronizedCells = synchronizedCells,
                ProcessingTimeMs = stopwatch.ElapsedMilliseconds
            };
            
            return isUpdate ? Ok(response) : Created($"/api/spatial/boundaries/{request.Id}", response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing boundary");
            return StatusCode(500, "Internal server error");
        }
    }
    
    [HttpGet("{boundaryId}")]
    [ProducesResponseType(typeof(BoundaryDetailsResponse), 200)]
    [ProducesResponseType(404)]
    public ActionResult<BoundaryDetailsResponse> GetBoundary(string boundaryId)
    {
        var boundary = _storage._precisionBoundaries.GetBoundary(boundaryId);
        
        if (boundary == null)
        {
            return NotFound($"Boundary not found: {boundaryId}");
        }
        
        return Ok(ToDetailsResponse(boundary));
    }
    
    [HttpGet]
    [ProducesResponseType(typeof(BoundaryListResponse), 200)]
    public ActionResult<BoundaryListResponse> QueryBoundaries(
        [FromQuery] double? minX,
        [FromQuery] double? minY,
        [FromQuery] double? maxX,
        [FromQuery] double? maxY,
        [FromQuery] string type = null)
    {
        List<BoundaryFeature> boundaries;
        
        if (minX.HasValue && minY.HasValue && maxX.HasValue && maxY.HasValue)
        {
            // Query by bounding box
            var envelope = new Envelope(minX.Value, maxX.Value, minY.Value, maxY.Value);
            boundaries = _storage.GetVectorBoundaries(envelope);
        }
        else
        {
            // Get all boundaries
            boundaries = _storage._precisionBoundaries.GetAllBoundaries().ToList();
        }
        
        // Filter by type if specified
        if (!string.IsNullOrEmpty(type))
        {
            var boundaryType = ParseBoundaryType(type);
            boundaries = boundaries.Where(b => b.Type == boundaryType).ToList();
        }
        
        return Ok(new BoundaryListResponse
        {
            Count = boundaries.Count,
            Boundaries = boundaries.Select(ToSummaryResponse).ToList()
        });
    }
}
```

### 3. Material Query API

#### Point Query

```http
POST /api/spatial/query/material
Content-Type: application/json

{
  "position": {
    "x": -120.5,
    "y": 36.5,
    "z": 0.0
  },
  "context": {
    "targetResolution": 100.0,
    "requiresPrecision": true,
    "lod": 0
  }
}
```

**Response:**
```json
{
  "material": {
    "id": 15,
    "name": "Granite",
    "properties": {
      "hardness": 7.0,
      "density": 2.75
    }
  },
  "confidence": 1.0,
  "source": "vector_precision",
  "boundaryId": "san-andreas-fault",
  "queryTimeMs": 0.8
}
```

#### Batch Query

```http
POST /api/spatial/query/material/batch
Content-Type: application/json

{
  "positions": [
    {"x": -120.5, "y": 36.5, "z": 0},
    {"x": -120.6, "y": 36.6, "z": 0},
    {"x": -120.7, "y": 36.7, "z": 0}
  ],
  "context": {
    "targetResolution": 100.0
  }
}
```

**Response:**
```json
{
  "results": [
    {
      "position": {"x": -120.5, "y": 36.5, "z": 0},
      "material": {"id": 15},
      "confidence": 1.0,
      "source": "vector_precision"
    },
    {
      "position": {"x": -120.6, "y": 36.6, "z": 0},
      "material": {"id": 15},
      "confidence": 0.95,
      "source": "grid_interior"
    },
    {
      "position": {"x": -120.7, "y": 36.7, "z": 0},
      "material": {"id": 18},
      "confidence": 1.0,
      "source": "vector_precision"
    }
  ],
  "totalQueryTimeMs": 2.1,
  "averageQueryTimeMs": 0.7
}
```

**Implementation:**
```csharp
[ApiController]
[Route("api/spatial/query")]
public class SpatialQueryController : ControllerBase
{
    private readonly GridVectorHybridStorage _storage;
    private readonly HybridQueryProcessor _queryProcessor;
    
    [HttpPost("material")]
    [ProducesResponseType(typeof(MaterialQueryResponse), 200)]
    public ActionResult<MaterialQueryResponse> QueryMaterial(
        [FromBody] MaterialQueryRequest request)
    {
        var stopwatch = Stopwatch.StartNew();
        
        var position = new Vector3(
            request.Position.X,
            request.Position.Y,
            request.Position.Z
        );
        
        var context = new QueryContext
        {
            TargetResolution = request.Context?.TargetResolution ?? 1.0,
            RequiresPrecision = request.Context?.RequiresPrecision ?? false,
            LOD = request.Context?.Lod ?? 0
        };
        
        var result = _queryProcessor.QueryMaterial(position, context);
        
        stopwatch.Stop();
        result.QueryTimeMs = stopwatch.Elapsed.TotalMilliseconds;
        
        return Ok(ToResponse(result));
    }
    
    [HttpPost("material/batch")]
    [ProducesResponseType(typeof(BatchMaterialQueryResponse), 200)]
    public ActionResult<BatchMaterialQueryResponse> QueryMaterialBatch(
        [FromBody] BatchMaterialQueryRequest request)
    {
        var stopwatch = Stopwatch.StartNew();
        
        var context = new QueryContext
        {
            TargetResolution = request.Context?.TargetResolution ?? 1.0,
            RequiresPrecision = request.Context?.RequiresPrecision ?? false,
            LOD = request.Context?.Lod ?? 0
        };
        
        var results = new List<MaterialQueryResult>();
        
        foreach (var pos in request.Positions)
        {
            var position = new Vector3(pos.X, pos.Y, pos.Z);
            var result = _queryProcessor.QueryMaterial(position, context);
            results.Add(result);
        }
        
        stopwatch.Stop();
        
        return Ok(new BatchMaterialQueryResponse
        {
            Results = results.Select(ToResponse).ToList(),
            TotalQueryTimeMs = stopwatch.Elapsed.TotalMilliseconds,
            AverageQueryTimeMs = stopwatch.Elapsed.TotalMilliseconds / results.Count
        });
    }
}
```

### 4. Simulation API

#### Run Geological Simulation

```http
POST /api/simulation/run
Content-Type: application/json

{
  "simulationType": "erosion",
  "regionIds": ["north-america-coastal"],
  "parameters": {
    "erosionCoefficient": 0.01,
    "maxErosionPerStep": 0.5,
    "depositionRate": 0.5
  },
  "deltaTime": "PT1H",
  "async": true
}
```

**Response:**
```json
{
  "simulationId": "sim_x9y8z7",
  "status": "running",
  "estimatedCompletionTime": "2025-10-21T18:00:00Z",
  "progressUrl": "/api/simulation/sim_x9y8z7/progress"
}
```

**Implementation:**
```csharp
[ApiController]
[Route("api/simulation")]
public class SimulationController : ControllerBase
{
    private readonly GeomorphologySimulationManager _simManager;
    private readonly IBackgroundJobClient _jobClient;
    
    [HttpPost("run")]
    [ProducesResponseType(typeof(SimulationStartResponse), 202)]
    [ProducesResponseType(typeof(SimulationStepResult), 200)]
    public async Task<ActionResult> RunSimulation(
        [FromBody] SimulationRunRequest request)
    {
        var config = BuildSimulationConfig(request);
        var deltaTime = XmlConvert.ToTimeSpan(request.DeltaTime);
        
        if (request.Async)
        {
            // Queue as background job
            var jobId = _jobClient.Enqueue(() => 
                _simManager.RunSimulationStep(config, deltaTime)
            );
            
            return Accepted(new SimulationStartResponse
            {
                SimulationId = jobId,
                Status = "running",
                EstimatedCompletionTime = DateTime.UtcNow.Add(
                    EstimateCompletionTime(config, deltaTime)
                ),
                ProgressUrl = $"/api/simulation/{jobId}/progress"
            });
        }
        else
        {
            // Run synchronously
            var result = await _simManager.RunSimulationStep(config, deltaTime);
            return Ok(result);
        }
    }
    
    [HttpGet("{simulationId}/progress")]
    [ProducesResponseType(typeof(SimulationProgressResponse), 200)]
    [ProducesResponseType(404)]
    public ActionResult<SimulationProgressResponse> GetProgress(string simulationId)
    {
        // Implementation would query job status
        // This is simplified for demonstration
        return Ok(new SimulationProgressResponse
        {
            SimulationId = simulationId,
            Status = "running",
            PercentComplete = 45.0,
            ProcessedCells = 450000,
            TotalCells = 1000000,
            ElapsedTime = "PT15M",
            EstimatedTimeRemaining = "PT18M"
        });
    }
}
```

## WebSocket Integration

### Real-time Simulation Updates

```javascript
// Client-side WebSocket connection
const ws = new WebSocket('wss://api.bluemarble.com/simulation/stream');

ws.onopen = () => {
  // Subscribe to simulation updates
  ws.send(JSON.stringify({
    action: 'subscribe',
    simulationId: 'sim_x9y8z7',
    updateInterval: 5000  // ms
  }));
};

ws.onmessage = (event) => {
  const update = JSON.parse(event.data);
  
  switch (update.type) {
    case 'progress':
      updateProgressBar(update.percentComplete);
      break;
      
    case 'cellUpdate':
      // Update specific grid cells
      updateGridCells(update.cells);
      break;
      
    case 'boundaryUpdate':
      // Boundary geometry changed
      updateBoundary(update.boundaryId, update.geometry);
      break;
      
    case 'complete':
      console.log('Simulation complete:', update.results);
      break;
      
    case 'error':
      console.error('Simulation error:', update.message);
      break;
  }
};
```

**Server Implementation:**
```csharp
public class SimulationWebSocketHandler : WebSocketHandler
{
    private readonly GeomorphologySimulationManager _simManager;
    
    public override async Task OnConnected(WebSocket socket)
    {
        await base.OnConnected(socket);
        
        var socketId = WebSocketConnectionManager.GetId(socket);
        _logger.LogInformation("Client connected: {SocketId}", socketId);
    }
    
    public override async Task ReceiveAsync(
        WebSocket socket,
        WebSocketReceiveResult result,
        byte[] buffer)
    {
        var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
        var request = JsonSerializer.Deserialize<WebSocketRequest>(message);
        
        switch (request.Action)
        {
            case "subscribe":
                await SubscribeToSimulation(socket, request.SimulationId);
                break;
                
            case "unsubscribe":
                await UnsubscribeFromSimulation(socket, request.SimulationId);
                break;
        }
    }
    
    private async Task SubscribeToSimulation(WebSocket socket, string simulationId)
    {
        // Register for updates
        _subscriptions.Add(simulationId, socket);
        
        // Send confirmation
        await SendMessageAsync(socket, new
        {
            type = "subscribed",
            simulationId = simulationId
        });
    }
    
    public async Task BroadcastSimulationUpdate(
        string simulationId,
        SimulationUpdate update)
    {
        if (_subscriptions.TryGetValue(simulationId, out var sockets))
        {
            foreach (var socket in sockets)
            {
                await SendMessageAsync(socket, update);
            }
        }
    }
}
```

## Client SDK Examples

### C# Client SDK

```csharp
public class BlueMarbleGridVectorClient
{
    private readonly HttpClient _httpClient;
    private readonly string _baseUrl;
    
    public BlueMarbleGridVectorClient(string baseUrl, string apiKey)
    {
        _baseUrl = baseUrl;
        _httpClient = new HttpClient();
        _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");
    }
    
    // Grid Operations
    public async Task<GridCreationResponse> CreateGridAsync(CreateGridRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync(
            $"{_baseUrl}/api/spatial/grids",
            request
        );
        
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<GridCreationResponse>();
    }
    
    public async Task<GridDetailsResponse> GetGridAsync(string regionId)
    {
        var response = await _httpClient.GetAsync(
            $"{_baseUrl}/api/spatial/grids/{regionId}"
        );
        
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<GridDetailsResponse>();
    }
    
    // Boundary Operations
    public async Task<BoundaryOperationResponse> AddBoundaryAsync(BoundaryRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync(
            $"{_baseUrl}/api/spatial/boundaries",
            request
        );
        
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<BoundaryOperationResponse>();
    }
    
    // Query Operations
    public async Task<MaterialQueryResponse> QueryMaterialAsync(
        double x, double y, double z,
        QueryContext context = null)
    {
        var request = new MaterialQueryRequest
        {
            Position = new { X = x, Y = y, Z = z },
            Context = context
        };
        
        var response = await _httpClient.PostAsJsonAsync(
            $"{_baseUrl}/api/spatial/query/material",
            request
        );
        
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<MaterialQueryResponse>();
    }
    
    // Simulation Operations
    public async Task<SimulationStartResponse> RunErosionSimulationAsync(
        string[] regionIds,
        ErosionParameters parameters,
        TimeSpan deltaTime,
        bool async = true)
    {
        var request = new SimulationRunRequest
        {
            SimulationType = "erosion",
            RegionIds = regionIds,
            Parameters = parameters,
            DeltaTime = XmlConvert.ToString(deltaTime),
            Async = async
        };
        
        var response = await _httpClient.PostAsJsonAsync(
            $"{_baseUrl}/api/simulation/run",
            request
        );
        
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<SimulationStartResponse>();
    }
}
```

### JavaScript/TypeScript Client SDK

```typescript
export class BlueMarbleGridVectorClient {
  private baseUrl: string;
  private apiKey: string;
  
  constructor(baseUrl: string, apiKey: string) {
    this.baseUrl = baseUrl;
    this.apiKey = apiKey;
  }
  
  // Grid Operations
  async createGrid(request: CreateGridRequest): Promise<GridCreationResponse> {
    const response = await fetch(`${this.baseUrl}/api/spatial/grids`, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
        'Authorization': `Bearer ${this.apiKey}`
      },
      body: JSON.stringify(request)
    });
    
    if (!response.ok) {
      throw new Error(`HTTP error! status: ${response.status}`);
    }
    
    return await response.json();
  }
  
  async getGrid(regionId: string): Promise<GridDetailsResponse> {
    const response = await fetch(
      `${this.baseUrl}/api/spatial/grids/${regionId}`,
      {
        headers: {
          'Authorization': `Bearer ${this.apiKey}`
        }
      }
    );
    
    if (!response.ok) {
      throw new Error(`HTTP error! status: ${response.status}`);
    }
    
    return await response.json();
  }
  
  // Query Operations
  async queryMaterial(
    x: number,
    y: number,
    z: number,
    context?: QueryContext
  ): Promise<MaterialQueryResponse> {
    const response = await fetch(
      `${this.baseUrl}/api/spatial/query/material`,
      {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
          'Authorization': `Bearer ${this.apiKey}`
        },
        body: JSON.stringify({
          position: { x, y, z },
          context: context || {}
        })
      }
    );
    
    if (!response.ok) {
      throw new Error(`HTTP error! status: ${response.status}`);
    }
    
    return await response.json();
  }
  
  // Batch query with retry logic
  async queryMaterialBatch(
    positions: Position[],
    context?: QueryContext,
    retries: number = 3
  ): Promise<BatchMaterialQueryResponse> {
    let lastError: Error;
    
    for (let i = 0; i < retries; i++) {
      try {
        const response = await fetch(
          `${this.baseUrl}/api/spatial/query/material/batch`,
          {
            method: 'POST',
            headers: {
              'Content-Type': 'application/json',
              'Authorization': `Bearer ${this.apiKey}`
            },
            body: JSON.stringify({
              positions,
              context: context || {}
            })
          }
        );
        
        if (!response.ok) {
          throw new Error(`HTTP error! status: ${response.status}`);
        }
        
        return await response.json();
      } catch (error) {
        lastError = error;
        if (i < retries - 1) {
          await this.sleep(Math.pow(2, i) * 1000); // Exponential backoff
        }
      }
    }
    
    throw lastError;
  }
  
  private sleep(ms: number): Promise<void> {
    return new Promise(resolve => setTimeout(resolve, ms));
  }
}
```

## Usage Examples

### Example 1: Creating a Coastal Erosion Simulation

```csharp
// Initialize client
var client = new BlueMarbleGridVectorClient(
    "https://api.bluemarble.com",
    Environment.GetEnvironmentVariable("BLUEMARBLE_API_KEY")
);

// Step 1: Create grid for coastal region
var gridResponse = await client.CreateGridAsync(new CreateGridRequest
{
    RegionId = "california-coast",
    Bounds = new BoundsDto
    {
        MinX = -122.5,
        MinY = 37.0,
        MaxX = -122.0,
        MaxY = 37.5
    },
    CellSize = 50.0, // 50m resolution
    InitialMaterials = "from-octree"
});

Console.WriteLine($"Created grid: {gridResponse.GridId}");
Console.WriteLine($"Total cells: {gridResponse.TotalCells}");

// Step 2: Add coastline boundary
var boundaryResponse = await client.AddBoundaryAsync(new BoundaryRequest
{
    Id = "california-coastline-segment",
    Type = "coastline",
    Geometry = new GeoJsonGeometry
    {
        Type = "LineString",
        Coordinates = new[]
        {
            new[] { -122.5, 37.0 },
            new[] { -122.3, 37.2 },
            new[] { -122.0, 37.5 }
        }
    },
    MaterialTransition = new MaterialTransitionDto
    {
        LeftMaterial = 1,  // Water
        RightMaterial = 10, // Sand
        TransitionType = "linear",
        BlendWidth = 100.0
    }
});

Console.WriteLine($"Added boundary, synchronized {boundaryResponse.SynchronizedCells} cells");

// Step 3: Run erosion simulation
var simResponse = await client.RunErosionSimulationAsync(
    regionIds: new[] { "california-coast" },
    parameters: new ErosionParameters
    {
        ErosionCoefficient = 0.01f,
        MaxErosionPerStep = 0.5f
    },
    deltaTime: TimeSpan.FromHours(1),
    async: true
);

Console.WriteLine($"Simulation started: {simResponse.SimulationId}");
Console.WriteLine($"Estimated completion: {simResponse.EstimatedCompletionTime}");

// Step 4: Monitor progress
while (true)
{
    var progress = await client.GetSimulationProgressAsync(simResponse.SimulationId);
    
    Console.WriteLine($"Progress: {progress.PercentComplete:F1}%");
    
    if (progress.Status == "complete")
    {
        Console.WriteLine("Simulation complete!");
        break;
    }
    
    await Task.Delay(5000); // Check every 5 seconds
}
```

### Example 2: Interactive Material Queries

```typescript
// TypeScript/JavaScript example for game client

const client = new BlueMarbleGridVectorClient(
  'https://api.bluemarble.com',
  process.env.BLUEMARBLE_API_KEY
);

// Query material under player position
async function getTerrainAtPosition(playerX: number, playerY: number) {
  const result = await client.queryMaterial(playerX, playerY, 0, {
    targetResolution: 1.0,
    requiresPrecision: true
  });
  
  console.log(`Material: ${result.material.name}`);
  console.log(`Source: ${result.source}`);
  console.log(`Confidence: ${result.confidence}`);
  
  return result.material;
}

// Batch query for visible area
async function getVisibleTerrainMaterials(
  centerX: number,
  centerY: number,
  radius: number
) {
  const positions: Position[] = [];
  
  // Sample in a grid pattern
  for (let dx = -radius; dx <= radius; dx += 10) {
    for (let dy = -radius; dy <= radius; dy += 10) {
      positions.push({
        x: centerX + dx,
        y: centerY + dy,
        z: 0
      });
    }
  }
  
  const results = await client.queryMaterialBatch(positions);
  
  console.log(`Queried ${results.results.length} positions`);
  console.log(`Average query time: ${results.averageQueryTimeMs.toFixed(2)}ms`);
  
  // Group by material type
  const materialCounts = new Map<number, number>();
  results.results.forEach(r => {
    const count = materialCounts.get(r.material.id) || 0;
    materialCounts.set(r.material.id, count + 1);
  });
  
  console.log('Material distribution:', Object.fromEntries(materialCounts));
  
  return results;
}
```

## Error Handling

### Common Error Responses

```json
// 400 Bad Request
{
  "error": "ValidationError",
  "message": "Invalid bounds specified",
  "details": {
    "field": "bounds.minX",
    "reason": "Must be less than maxX"
  }
}

// 404 Not Found
{
  "error": "NotFound",
  "message": "Grid not found: unknown-region"
}

// 409 Conflict
{
  "error": "Conflict",
  "message": "Grid already exists for region: california-coast"
}

// 429 Rate Limit
{
  "error": "RateLimitExceeded",
  "message": "Too many requests",
  "retryAfter": 60
}

// 500 Internal Server Error
{
  "error": "InternalServerError",
  "message": "An unexpected error occurred",
  "requestId": "req_abc123"
}
```

## Conclusion

This API integration guide provides comprehensive examples for integrating the Grid + Vector hybrid storage system with client applications. The RESTful endpoints, WebSocket connections, and client SDKs enable seamless interaction with the spatial storage system for various use cases including simulation, querying, and real-time updates.

**Key Features**:
- RESTful API for CRUD operations on grids and boundaries
- Efficient batch querying for performance
- Real-time WebSocket updates for simulations
- Client SDKs in C# and TypeScript
- Comprehensive error handling
- Practical usage examples
