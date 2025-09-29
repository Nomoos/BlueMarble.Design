# Spherical Planet Generation API Specification

**Document Type:** API Specification  
**Version:** 1.0  
**Author:** Backend Architecture Team  
**Date:** 2024-12-29  
**Status:** Draft  
**Related Specifications:** 
- [Spherical Planet Generation System](../systems/spec-spherical-planet-generation.md)
- [Technical Implementation Guide](../systems/tech-spherical-planet-implementation.md)

## Overview

This document defines the RESTful API for the Spherical Planet Generation system, providing endpoints for planet generation, biome classification, map projections, and data retrieval. The API maintains compatibility with BlueMarble's existing spatial data infrastructure while adding new capabilities for spherical planet operations.

## Base Configuration

**Base URL:** `https://api.bluemarble.design/v1`  
**Authentication:** Bearer token required for generation operations  
**Content-Type:** `application/json`  
**Coordinate System:** SRID_METER (4087) for all spatial data  

## Core Endpoints

### 1. Planet Generation

#### Generate New Planet

**Endpoint:** `POST /planet/generate`

**Description:** Generates a new spherical planet with specified parameters

**Request Body:**
```json
{
  "config": {
    "name": "Earth-like Test Planet",
    "radiusMeters": 6371000,
    "plateCount": 12,
    "oceanCoverage": 0.71,
    "seed": 12345,
    "climate": {
      "globalTemperature": 15.0,
      "temperatureVariation": 40.0,
      "precipitationBase": 1000.0,
      "seasonalVariation": 0.2
    }
  },
  "options": {
    "generateBiomes": true,
    "applyProjection": "equirectangular",
    "outputFormat": "geojson",
    "compressionLevel": "medium"
  }
}
```

**Response:** `202 Accepted`
```json
{
  "taskId": "planet-gen-8f4a2c1d-9e5b-4c3a-8f2e-1a3b4c5d6e7f",
  "status": "queued",
  "estimatedCompletionTime": "2024-12-29T15:30:00Z",
  "statusUrl": "/planet/generate/status/planet-gen-8f4a2c1d-9e5b-4c3a-8f2e-1a3b4c5d6e7f"
}
```

#### Check Generation Status

**Endpoint:** `GET /planet/generate/status/{taskId}`

**Response:** `200 OK`
```json
{
  "taskId": "planet-gen-8f4a2c1d-9e5b-4c3a-8f2e-1a3b4c5d6e7f",
  "status": "in_progress",
  "progress": {
    "percentage": 75,
    "currentStage": "classifying_biomes",
    "stages": [
      {"name": "generating_plates", "status": "completed", "duration": "00:02:15"},
      {"name": "applying_tectonics", "status": "completed", "duration": "00:01:30"},
      {"name": "classifying_biomes", "status": "in_progress", "progress": 75},
      {"name": "applying_projection", "status": "pending"},
      {"name": "finalizing", "status": "pending"}
    ]
  },
  "result": null,
  "error": null,
  "estimatedTimeRemaining": "00:01:45"
}
```

**Response (Completed):** `200 OK`
```json
{
  "taskId": "planet-gen-8f4a2c1d-9e5b-4c3a-8f2e-1a3b4c5d6e7f",
  "status": "completed",
  "progress": {
    "percentage": 100,
    "currentStage": "completed",
    "totalDuration": "00:08:23"
  },
  "result": {
    "planetId": "planet-12345-abcdef",
    "downloadUrl": "/planet/planet-12345-abcdef/download",
    "previewUrl": "/planet/planet-12345-abcdef/preview",
    "statistics": {
      "totalPolygons": 15847,
      "totalArea": 510072000000000,
      "biomeDistribution": {
        "Ocean": 0.712,
        "TropicalRainforest": 0.083,
        "Desert": 0.143,
        "Grassland": 0.062
      }
    }
  },
  "error": null
}
```

### 2. Planet Data Retrieval

#### Get Planet Information

**Endpoint:** `GET /planet/{planetId}`

**Response:** `200 OK`
```json
{
  "planetId": "planet-12345-abcdef",
  "metadata": {
    "name": "Earth-like Test Planet",
    "createdAt": "2024-12-29T14:22:00Z",
    "version": "1.0",
    "config": {
      "radiusMeters": 6371000,
      "plateCount": 12,
      "oceanCoverage": 0.71,
      "seed": 12345
    }
  },
  "statistics": {
    "totalPolygons": 15847,
    "totalArea": 510072000000000,
    "averagePolygonArea": 32145678,
    "biomeCount": 8,
    "biomeDistribution": {
      "Ocean": {"coverage": 0.712, "area": 363171264000000},
      "TropicalRainforest": {"coverage": 0.083, "area": 42355976000000},
      "Desert": {"coverage": 0.143, "area": 72940296000000},
      "TemperateForest": {"coverage": 0.045, "area": 22953240000000},
      "Grassland": {"coverage": 0.017, "area": 8671224000000}
    }
  },
  "availableProjections": [
    "equirectangular",
    "mercator", 
    "robinson",
    "mollweide",
    "stereographic"
  ],
  "availableFormats": ["geojson", "geopackage", "shapefile"]
}
```

#### Get Planet Polygons

**Endpoint:** `GET /planet/{planetId}/polygons`

**Query Parameters:**
- `projection` (string): Projection type (default: "equirectangular")
- `format` (string): Output format (default: "geojson")
- `biome` (string): Filter by biome type (optional)
- `bbox` (string): Bounding box filter "minx,miny,maxx,maxy" (optional)
- `simplification` (number): Geometry simplification tolerance (optional)
- `limit` (integer): Maximum number of polygons (default: 1000)
- `offset` (integer): Pagination offset (default: 0)

**Example:** `GET /planet/planet-12345-abcdef/polygons?projection=mercator&biome=TropicalRainforest&limit=500`

**Response:** `200 OK`
```json
{
  "type": "FeatureCollection",
  "metadata": {
    "planetId": "planet-12345-abcdef",
    "projection": "mercator",
    "coordinateSystem": "SRID:4087",
    "totalFeatures": 1247,
    "returnedFeatures": 500,
    "hasMore": true,
    "nextUrl": "/planet/planet-12345-abcdef/polygons?offset=500&projection=mercator&biome=TropicalRainforest&limit=500"
  },
  "features": [
    {
      "type": "Feature",
      "id": "polygon-1",
      "geometry": {
        "type": "Polygon",
        "coordinates": [[[180.0, -85.0], [180.5, -85.0], [180.5, -84.5], [180.0, -84.5], [180.0, -85.0]]]
      },
      "properties": {
        "biomeType": "TropicalRainforest",
        "temperature": 26.3,
        "precipitation": 2450.7,
        "elevation": 234.5,
        "area": 1256789.34,
        "plateId": "plate-7",
        "color": "#228B22",
        "generated": "2024-12-29T14:22:15Z"
      }
    }
  ]
}
```

### 3. Biome Operations

#### Get Biome Classifications

**Endpoint:** `GET /planet/{planetId}/biomes`

**Response:** `200 OK`
```json
{
  "planetId": "planet-12345-abcdef",
  "biomes": [
    {
      "type": "Ocean",
      "coverage": 0.712,
      "totalArea": 363171264000000,
      "polygonCount": 8423,
      "averageTemperature": 15.2,
      "temperatureRange": {"min": -1.8, "max": 30.1},
      "precipitation": 0,
      "color": "#4169E1",
      "description": "Deep ocean waters covering the majority of the planet surface"
    },
    {
      "type": "TropicalRainforest", 
      "coverage": 0.083,
      "totalArea": 42355976000000,
      "polygonCount": 1247,
      "averageTemperature": 26.8,
      "temperatureRange": {"min": 20.2, "max": 32.1},
      "precipitation": 2847.3,
      "color": "#228B22",
      "description": "Dense tropical forests with high biodiversity and precipitation"
    }
  ],
  "climateZones": [
    {
      "zone": "Tropical",
      "latitudeRange": {"min": -23.5, "max": 23.5},
      "coverage": 0.234,
      "biomes": ["TropicalRainforest", "TropicalSavanna", "Desert"]
    },
    {
      "zone": "Temperate",
      "latitudeRange": {"min": -66.5, "max": -23.5},
      "coverage": 0.412,
      "biomes": ["TemperateForest", "Grassland", "Desert"]
    }
  ]
}
```

#### Reclassify Biomes

**Endpoint:** `POST /planet/{planetId}/biomes/reclassify`

**Request Body:**
```json
{
  "climateParameters": {
    "globalTemperature": 17.0,
    "temperatureVariation": 35.0,
    "precipitationBase": 1200.0,
    "seasonalVariation": 0.3
  },
  "biomeRules": [
    {
      "biomeType": "TropicalRainforest",
      "temperatureMin": 22.0,
      "temperatureMax": 35.0,
      "precipitationMin": 2500.0,
      "elevationMax": 1500.0
    }
  ]
}
```

**Response:** `202 Accepted`
```json
{
  "taskId": "biome-reclassify-4d2a1b3c-7e8f-4a5b-9c6d-2e1f3a4b5c6d",
  "status": "queued",
  "statusUrl": "/planet/planet-12345-abcdef/biomes/reclassify/status/biome-reclassify-4d2a1b3c-7e8f-4a5b-9c6d-2e1f3a4b5c6d"
}
```

### 4. Map Projections

#### Get Available Projections

**Endpoint:** `GET /projections`

**Response:** `200 OK`
```json
{
  "projections": [
    {
      "name": "equirectangular",
      "displayName": "Equirectangular (Plate Carrée)",
      "description": "Simple cylindrical projection with equally spaced meridians and parallels",
      "properties": {
        "preservesArea": false,
        "preservesAngles": false,
        "preservesDistance": false,
        "distortionPattern": "increases_toward_poles"
      },
      "parameters": {
        "centralMeridian": {"type": "number", "default": 0.0, "range": [-180, 180]},
        "standardParallel": {"type": "number", "default": 0.0, "range": [-90, 90]}
      },
      "useCases": ["web_mapping", "simple_visualization", "global_overview"]
    },
    {
      "name": "mercator",
      "displayName": "Mercator",
      "description": "Cylindrical conformal projection preserving angles",
      "properties": {
        "preservesArea": false,
        "preservesAngles": true,
        "preservesDistance": false,
        "distortionPattern": "extreme_polar_enlargement"
      },
      "parameters": {
        "centralMeridian": {"type": "number", "default": 0.0, "range": [-180, 180]}
      },
      "useCases": ["navigation", "web_maps", "local_detail"]
    },
    {
      "name": "robinson",
      "displayName": "Robinson",
      "description": "Pseudocylindrical projection balancing area and angle distortion",
      "properties": {
        "preservesArea": false,
        "preservesAngles": false,
        "preservesDistance": false,
        "distortionPattern": "minimal_overall_distortion"
      },
      "parameters": {
        "centralMeridian": {"type": "number", "default": 0.0, "range": [-180, 180]}
      },
      "useCases": ["world_maps", "educational_materials", "atlases"]
    }
  ]
}
```

#### Apply Projection to Planet

**Endpoint:** `POST /planet/{planetId}/projection/{projectionName}`

**Request Body:**
```json
{
  "parameters": {
    "centralMeridian": 0.0,
    "standardParallel": 45.0
  },
  "options": {
    "outputFormat": "geojson",
    "simplificationTolerance": 100.0,
    "includeDistortionData": true
  }
}
```

**Response:** `200 OK`
```json
{
  "projectionName": "robinson",
  "parameters": {
    "centralMeridian": 0.0
  },
  "downloadUrl": "/planet/planet-12345-abcdef/projection/robinson/download",
  "distortionAnalysis": {
    "maxAreaDistortion": 1.67,
    "maxAngleDistortion": 23.4,
    "averageDistortion": 0.34,
    "distortionMap": "/planet/planet-12345-abcdef/projection/robinson/distortion"
  },
  "metadata": {
    "totalFeatures": 15847,
    "projectedArea": 510072000000000,
    "bounds": {
      "minX": -17005833.33,
      "minY": -8625154.47,
      "maxX": 17005833.33,
      "maxY": 8625154.47
    }
  }
}
```

### 5. World Wrapping and Topology

#### Validate World Topology

**Endpoint:** `POST /planet/{planetId}/validate`

**Response:** `200 OK`
```json
{
  "isValid": true,
  "validationResults": {
    "topologyErrors": [],
    "wrappingIssues": [],
    "geometryErrors": [],
    "biomeConsistency": true,
    "coordinateSystemValid": true
  },
  "statistics": {
    "validPolygons": 15847,
    "invalidPolygons": 0,
    "fixedPolygons": 3,
    "wrappedPolygons": 127
  },
  "recommendations": [
    "Consider simplifying complex geometries for better performance",
    "Some biome transitions are very sharp - consider smoothing"
  ]
}
```

#### Apply World Wrapping

**Endpoint:** `POST /planet/{planetId}/wrap`

**Request Body:**
```json
{
  "wrapOptions": {
    "longitudeWrapping": true,
    "latitudeClipping": true,
    "topologyValidation": true,
    "seamlessEdges": true
  }
}
```

**Response:** `200 OK`
```json
{
  "wrappingApplied": true,
  "modifications": {
    "wrappedPolygons": 127,
    "clippedPolygons": 34,
    "repairedTopology": 8
  },
  "downloadUrl": "/planet/planet-12345-abcdef/wrapped/download"
}
```

### 6. Export and Download

#### Download Planet Data

**Endpoint:** `GET /planet/{planetId}/download`

**Query Parameters:**
- `format` (string): Export format (geojson, geopackage, shapefile) (default: "geojson")
- `projection` (string): Projection type (default: "equirectangular")
- `compression` (string): Compression level (none, low, medium, high) (default: "medium")

**Response:** `200 OK`
- Content-Type: `application/octet-stream` or `application/json`
- Content-Disposition: `attachment; filename="planet-12345-abcdef.gpkg"`

#### Export Planet Configuration

**Endpoint:** `GET /planet/{planetId}/config/export`

**Response:** `200 OK`
```json
{
  "exportedAt": "2024-12-29T16:45:00Z",
  "planetConfig": {
    "name": "Earth-like Test Planet",
    "radiusMeters": 6371000,
    "plateCount": 12,
    "oceanCoverage": 0.71,
    "seed": 12345,
    "climate": {
      "globalTemperature": 15.0,
      "temperatureVariation": 40.0,
      "precipitationBase": 1000.0,
      "seasonalVariation": 0.2
    }
  },
  "generationOptions": {
    "generateBiomes": true,
    "applyProjection": "equirectangular",
    "outputFormat": "geojson"
  },
  "reproductionInstructions": {
    "endpoint": "/planet/generate",
    "exactConfig": "Use the planetConfig object as the 'config' field in the request body"
  }
}
```

## Error Handling

### Standard Error Response Format

```json
{
  "error": {
    "code": "PLANET_GENERATION_FAILED",
    "message": "Planet generation failed due to invalid parameters",
    "details": {
      "field": "plateCount",
      "value": -5,
      "reason": "Plate count must be positive integer between 3 and 50"
    },
    "timestamp": "2024-12-29T14:22:00Z",
    "requestId": "req-8f4a2c1d-9e5b-4c3a-8f2e-1a3b4c5d6e7f"
  }
}
```

### Error Codes

| Code | HTTP Status | Description |
|------|-------------|-------------|
| `INVALID_PLANET_CONFIG` | 400 | Invalid planet configuration parameters |
| `PLANET_NOT_FOUND` | 404 | Requested planet does not exist |
| `GENERATION_IN_PROGRESS` | 409 | Planet generation already in progress |
| `GENERATION_FAILED` | 500 | Planet generation process failed |
| `PROJECTION_NOT_SUPPORTED` | 400 | Requested projection type not available |
| `INSUFFICIENT_RESOURCES` | 503 | Server resources insufficient for generation |
| `RATE_LIMIT_EXCEEDED` | 429 | Too many generation requests |
| `AUTH_REQUIRED` | 401 | Authentication required for this operation |
| `PERMISSION_DENIED` | 403 | Insufficient permissions for operation |

## Rate Limiting

### Generation Operations
- **Planet Generation:** 5 requests per hour per user
- **Biome Reclassification:** 10 requests per hour per user
- **Projection Operations:** 20 requests per hour per user

### Data Retrieval
- **Planet Data:** 1000 requests per hour per user
- **Polygon Data:** 500 requests per hour per user
- **Downloads:** 50 requests per hour per user

### Rate Limit Headers
```
X-RateLimit-Limit: 5
X-RateLimit-Remaining: 3
X-RateLimit-Reset: 1640995200
X-RateLimit-Resource: planet-generation
```

## Authentication and Authorization

### Authentication
```
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

### Permission Levels
- **Viewer:** Read-only access to existing planets
- **Generator:** Can generate new planets and modify existing ones
- **Admin:** Full access including system configuration

### Rate Limits by Permission Level
| Operation | Viewer | Generator | Admin |
|-----------|--------|-----------|-------|
| Planet Generation | 0/hour | 5/hour | Unlimited |
| Data Retrieval | 100/hour | 1000/hour | Unlimited |
| Downloads | 10/hour | 50/hour | Unlimited |

## Webhooks

### Generation Complete Notification

**Webhook URL:** User-configured endpoint  
**Method:** POST  
**Content-Type:** application/json

**Payload:**
```json
{
  "event": "planet.generation.completed",
  "timestamp": "2024-12-29T15:30:00Z",
  "data": {
    "taskId": "planet-gen-8f4a2c1d-9e5b-4c3a-8f2e-1a3b4c5d6e7f",
    "planetId": "planet-12345-abcdef",
    "status": "completed",
    "duration": "00:08:23",
    "downloadUrl": "/planet/planet-12345-abcdef/download"
  },
  "user": {
    "id": "user-123",
    "email": "scientist@example.com"
  }
}
```

## SDK Examples

### JavaScript SDK Usage

```javascript
import { BlueMarblePlanetAPI } from '@bluemarble/planet-sdk';

const api = new BlueMarblePlanetAPI({
    baseUrl: 'https://api.bluemarble.design/v1',
    apiKey: 'your-api-key'
});

// Generate a new planet
const generationTask = await api.planet.generate({
    config: {
        radiusMeters: 6371000,
        plateCount: 12,
        oceanCoverage: 0.71,
        seed: 12345
    },
    options: {
        generateBiomes: true,
        applyProjection: 'robinson'
    }
});

// Wait for completion
const planet = await generationTask.waitForCompletion();

// Get planet data with specific projection
const data = await api.planet.getPolygons(planet.id, {
    projection: 'mercator',
    format: 'geojson',
    biome: 'TropicalRainforest'
});

// Apply different projection
const robinsonData = await api.planet.applyProjection(planet.id, 'robinson', {
    centralMeridian: -90.0
});
```

### Python SDK Usage

```python
from bluemarble_planet import PlanetAPI

api = PlanetAPI(
    base_url='https://api.bluemarble.design/v1',
    api_key='your-api-key'
)

# Generate planet
task = api.planet.generate(
    config={
        'radius_meters': 6371000,
        'plate_count': 12,
        'ocean_coverage': 0.71,
        'seed': 12345
    },
    options={
        'generate_biomes': True,
        'apply_projection': 'equirectangular'
    }
)

# Monitor progress
while not task.is_complete():
    print(f"Progress: {task.progress.percentage}%")
    time.sleep(30)

planet = task.result()

# Export to GeoPackage
planet.download(format='geopackage', filename='my_planet.gpkg')

# Analyze biome distribution
biomes = planet.biomes.get_statistics()
print(f"Ocean coverage: {biomes['Ocean']['coverage']:.1%}")
```

This API specification provides comprehensive coverage of all spherical planet generation capabilities while maintaining RESTful design principles and integration with BlueMarble's existing infrastructure.