# Video Generation Pipeline API Specification

**Document Type:** API Specification  
**Version:** 1.0  
**Author:** Content Systems Team  
**Date:** 2025-01-15  
**Status:** Draft  
**Related Documents:**
- [System Specification](spec-video-generation-pipeline.md)
- [Technical Implementation Guide](tech-video-generation-pipeline.md)

## Overview

This document defines the REST API for the Video Generation Pipeline system, providing endpoints for submitting stories, tracking video generation progress, and retrieving completed videos.

**Base URL:** `https://api.bluemarble.design/v1`

**Authentication:** Bearer token required for all endpoints

## Base Configuration

### Authentication

All API requests require authentication using a Bearer token:

```http
Authorization: Bearer <your_api_token>
```

### Request Headers

```http
Content-Type: application/json
Accept: application/json
Authorization: Bearer <token>
```

### Response Format

All responses follow a standard JSON format:

```json
{
  "success": true,
  "data": {},
  "error": null,
  "timestamp": "2025-01-15T10:30:00Z"
}
```

## Core Endpoints

### 1. Video Generation

#### Submit Story for Video Generation

**POST** `/video-pipeline/jobs`

Submit a story for video generation processing.

**Request Body:**

```json
{
  "story_id": "story_12345",
  "story_data": {
    "title": "The Lost Artifact",
    "content": "In a dusty ancient temple, archaeologist Dr. Elena Martinez discovered...",
    "metadata": {
      "genre": "adventure",
      "author": "StoryGenerator_v2"
    }
  },
  "config": {
    "aspect_ratio": "9:16",
    "target_duration": 60,
    "quality": "high",
    "enable_tts": true,
    "enable_background_music": false
  },
  "priority": "normal"
}
```

**Parameters:**

| Field | Type | Required | Description |
|-------|------|----------|-------------|
| story_id | string | Yes | Unique identifier for the story |
| story_data | object | Yes | Story content and metadata |
| story_data.title | string | Yes | Story title |
| story_data.content | string | Yes | Full story text |
| story_data.metadata | object | No | Additional story metadata |
| config | object | No | Video generation configuration |
| config.aspect_ratio | string | No | Video aspect ratio (default: "9:16") |
| config.target_duration | integer | No | Target video duration in seconds (default: 60) |
| config.quality | string | No | Quality preset: "low", "medium", "high" (default: "high") |
| config.enable_tts | boolean | No | Enable text-to-speech (default: true) |
| config.enable_background_music | boolean | No | Add background music (default: false) |
| priority | string | No | Job priority: "high", "normal", "low" (default: "normal") |

**Response (201 Created):**

```json
{
  "success": true,
  "data": {
    "job_id": "job_abc123def456",
    "story_id": "story_12345",
    "status": "queued",
    "created_at": "2025-01-15T10:30:00Z",
    "estimated_completion": "2025-01-15T10:33:00Z",
    "position_in_queue": 3
  },
  "error": null,
  "timestamp": "2025-01-15T10:30:00Z"
}
```

**Error Responses:**

- `400 Bad Request` - Invalid request parameters
- `401 Unauthorized` - Missing or invalid authentication
- `429 Too Many Requests` - Rate limit exceeded
- `500 Internal Server Error` - Server error

#### Get Job Status

**GET** `/video-pipeline/jobs/{job_id}`

Retrieve the current status of a video generation job.

**Path Parameters:**

| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| job_id | string | Yes | Unique job identifier |

**Response (200 OK):**

```json
{
  "success": true,
  "data": {
    "job_id": "job_abc123def456",
    "story_id": "story_12345",
    "status": "processing",
    "progress": {
      "current_stage": "rendering",
      "percentage": 75,
      "stages_completed": ["story_processing", "asset_generation", "scene_composition"],
      "stages_remaining": ["rendering", "post_processing"]
    },
    "created_at": "2025-01-15T10:30:00Z",
    "started_at": "2025-01-15T10:31:00Z",
    "estimated_completion": "2025-01-15T10:33:00Z",
    "attempts": 1,
    "max_attempts": 3
  },
  "error": null,
  "timestamp": "2025-01-15T10:32:30Z"
}
```

**Status Values:**

- `queued` - Job is waiting in queue
- `processing` - Job is actively being processed
- `completed` - Job completed successfully
- `failed` - Job failed after all retry attempts
- `cancelled` - Job was cancelled

#### Get Completed Video

**GET** `/video-pipeline/jobs/{job_id}/result`

Retrieve the result of a completed video generation job.

**Path Parameters:**

| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| job_id | string | Yes | Unique job identifier |

**Response (200 OK):**

```json
{
  "success": true,
  "data": {
    "job_id": "job_abc123def456",
    "story_id": "story_12345",
    "status": "completed",
    "video": {
      "url": "https://cdn.bluemarble.design/videos/story_12345.mp4",
      "download_url": "https://cdn.bluemarble.design/videos/story_12345.mp4?download=1",
      "duration": 62.5,
      "resolution": [1080, 1920],
      "aspect_ratio": "9:16",
      "file_size": 5242880,
      "format": "mp4",
      "codec": "h264"
    },
    "thumbnail": {
      "url": "https://cdn.bluemarble.design/thumbnails/story_12345_thumb.jpg",
      "resolution": [1080, 1920]
    },
    "metadata": {
      "title": "The Lost Artifact",
      "description": "In a dusty ancient temple, archaeologist Dr. Elena Martinez discovered a glowing crystal...",
      "tags": ["adventure", "mystery", "artifact", "discovery"],
      "hashtags": ["#adventure", "#mystery", "#storytelling", "#shortfilm"],
      "genre": "adventure"
    },
    "created_at": "2025-01-15T10:30:00Z",
    "completed_at": "2025-01-15T10:32:45Z",
    "processing_time": 165
  },
  "error": null,
  "timestamp": "2025-01-15T10:33:00Z"
}
```

**Error Responses:**

- `404 Not Found` - Job not found
- `409 Conflict` - Job not yet completed

#### Cancel Job

**DELETE** `/video-pipeline/jobs/{job_id}`

Cancel a queued or processing video generation job.

**Path Parameters:**

| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| job_id | string | Yes | Unique job identifier |

**Response (200 OK):**

```json
{
  "success": true,
  "data": {
    "job_id": "job_abc123def456",
    "status": "cancelled",
    "cancelled_at": "2025-01-15T10:31:00Z"
  },
  "error": null,
  "timestamp": "2025-01-15T10:31:00Z"
}
```

**Error Responses:**

- `404 Not Found` - Job not found
- `409 Conflict` - Job already completed or failed

#### List Jobs

**GET** `/video-pipeline/jobs`

List video generation jobs with filtering and pagination.

**Query Parameters:**

| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| status | string | No | Filter by status: "queued", "processing", "completed", "failed" |
| story_id | string | No | Filter by story ID |
| priority | string | No | Filter by priority: "high", "normal", "low" |
| page | integer | No | Page number (default: 1) |
| per_page | integer | No | Items per page (default: 20, max: 100) |
| sort | string | No | Sort by field: "created_at", "completed_at", "priority" |
| order | string | No | Sort order: "asc", "desc" (default: "desc") |

**Response (200 OK):**

```json
{
  "success": true,
  "data": {
    "jobs": [
      {
        "job_id": "job_abc123def456",
        "story_id": "story_12345",
        "status": "completed",
        "priority": "normal",
        "created_at": "2025-01-15T10:30:00Z",
        "completed_at": "2025-01-15T10:32:45Z"
      }
    ],
    "pagination": {
      "page": 1,
      "per_page": 20,
      "total": 45,
      "total_pages": 3,
      "has_next": true,
      "has_prev": false
    }
  },
  "error": null,
  "timestamp": "2025-01-15T10:33:00Z"
}
```

### 2. Configuration Management

#### Get Default Configuration

**GET** `/video-pipeline/config/defaults`

Retrieve default configuration values for video generation.

**Response (200 OK):**

```json
{
  "success": true,
  "data": {
    "aspect_ratios": ["9:16", "16:9", "1:1", "4:5"],
    "qualities": ["low", "medium", "high"],
    "default_config": {
      "aspect_ratio": "9:16",
      "target_duration": 60,
      "quality": "high",
      "enable_tts": true,
      "enable_background_music": false,
      "fps": 30,
      "resolution": [1080, 1920]
    },
    "limits": {
      "max_duration": 180,
      "min_duration": 15,
      "max_file_size": 104857600,
      "max_concurrent_jobs": 5
    }
  },
  "error": null,
  "timestamp": "2025-01-15T10:33:00Z"
}
```

#### Validate Configuration

**POST** `/video-pipeline/config/validate`

Validate a video generation configuration before submitting a job.

**Request Body:**

```json
{
  "config": {
    "aspect_ratio": "9:16",
    "target_duration": 60,
    "quality": "high"
  }
}
```

**Response (200 OK):**

```json
{
  "success": true,
  "data": {
    "valid": true,
    "warnings": [],
    "errors": []
  },
  "error": null,
  "timestamp": "2025-01-15T10:33:00Z"
}
```

**Response (400 Bad Request) - Invalid Configuration:**

```json
{
  "success": false,
  "data": {
    "valid": false,
    "warnings": [
      "Target duration exceeds recommended 60 seconds for social media"
    ],
    "errors": [
      "Invalid aspect_ratio: '9:18'. Must be one of: 9:16, 16:9, 1:1, 4:5"
    ]
  },
  "error": "Configuration validation failed",
  "timestamp": "2025-01-15T10:33:00Z"
}
```

### 3. Asset Management

#### Get Generated Assets

**GET** `/video-pipeline/jobs/{job_id}/assets`

Retrieve individual assets generated during video creation.

**Path Parameters:**

| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| job_id | string | Yes | Unique job identifier |

**Response (200 OK):**

```json
{
  "success": true,
  "data": {
    "job_id": "job_abc123def456",
    "assets": {
      "images": [
        {
          "scene_id": "scene_001",
          "url": "https://cdn.bluemarble.design/assets/job_abc123/scene_001.png",
          "prompt": "Ancient temple interior with dust and light shafts",
          "resolution": [1080, 1920]
        },
        {
          "scene_id": "scene_002",
          "url": "https://cdn.bluemarble.design/assets/job_abc123/scene_002.png",
          "prompt": "Close-up of glowing crystal in archaeologist's hands",
          "resolution": [1080, 1920]
        }
      ],
      "audio": {
        "narration_url": "https://cdn.bluemarble.design/assets/job_abc123/narration.mp3",
        "duration": 62.5,
        "voice": "en-US-Neural2-C"
      }
    }
  },
  "error": null,
  "timestamp": "2025-01-15T10:33:00Z"
}
```

### 4. Publishing

#### Publish Video

**POST** `/video-pipeline/jobs/{job_id}/publish`

Publish a completed video to specified platforms.

**Path Parameters:**

| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| job_id | string | Yes | Unique job identifier |

**Request Body:**

```json
{
  "platforms": ["instagram", "youtube"],
  "schedule": {
    "publish_at": "2025-01-15T15:00:00Z"
  },
  "platform_specific": {
    "instagram": {
      "type": "reel",
      "caption": "Check out this amazing story! #adventure #storytelling"
    },
    "youtube": {
      "title": "The Lost Artifact - Short Story",
      "description": "An exciting adventure story...",
      "visibility": "public",
      "category": "Entertainment"
    }
  }
}
```

**Response (202 Accepted):**

```json
{
  "success": true,
  "data": {
    "publish_id": "pub_xyz789",
    "job_id": "job_abc123def456",
    "platforms": {
      "instagram": {
        "status": "queued_manual",
        "message": "Queued for manual posting due to platform restrictions",
        "scheduled_at": "2025-01-15T15:00:00Z"
      },
      "youtube": {
        "status": "processing",
        "message": "Upload in progress",
        "upload_id": "yt_upload_123"
      }
    }
  },
  "error": null,
  "timestamp": "2025-01-15T10:33:00Z"
}
```

#### Get Publishing Status

**GET** `/video-pipeline/publishing/{publish_id}`

Get the status of a publishing operation.

**Path Parameters:**

| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| publish_id | string | Yes | Unique publishing operation identifier |

**Response (200 OK):**

```json
{
  "success": true,
  "data": {
    "publish_id": "pub_xyz789",
    "job_id": "job_abc123def456",
    "platforms": {
      "instagram": {
        "status": "queued_manual",
        "queued_at": "2025-01-15T10:33:00Z"
      },
      "youtube": {
        "status": "published",
        "url": "https://youtube.com/shorts/abc123",
        "published_at": "2025-01-15T15:05:00Z"
      }
    }
  },
  "error": null,
  "timestamp": "2025-01-15T15:06:00Z"
}
```

### 5. Analytics

#### Get Job Statistics

**GET** `/video-pipeline/stats`

Get statistics about video generation jobs.

**Query Parameters:**

| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| start_date | string | No | Start date (ISO 8601 format) |
| end_date | string | No | End date (ISO 8601 format) |
| group_by | string | No | Group by: "day", "week", "month" |

**Response (200 OK):**

```json
{
  "success": true,
  "data": {
    "period": {
      "start": "2025-01-01T00:00:00Z",
      "end": "2025-01-15T23:59:59Z"
    },
    "totals": {
      "jobs_submitted": 245,
      "jobs_completed": 234,
      "jobs_failed": 8,
      "jobs_cancelled": 3,
      "success_rate": 0.954
    },
    "performance": {
      "avg_processing_time": 165.3,
      "avg_queue_time": 23.7,
      "total_video_duration": 14650,
      "total_storage_used": 1288490188
    },
    "by_priority": {
      "high": 45,
      "normal": 185,
      "low": 15
    }
  },
  "error": null,
  "timestamp": "2025-01-15T10:33:00Z"
}
```

## Webhooks

### Webhook Configuration

Configure webhooks to receive notifications about job status changes.

#### Register Webhook

**POST** `/video-pipeline/webhooks`

Register a webhook URL for event notifications.

**Request Body:**

```json
{
  "url": "https://your-app.com/webhooks/video-pipeline",
  "events": ["job.completed", "job.failed", "job.cancelled"],
  "secret": "your_webhook_secret"
}
```

**Response (201 Created):**

```json
{
  "success": true,
  "data": {
    "webhook_id": "wh_abc123",
    "url": "https://your-app.com/webhooks/video-pipeline",
    "events": ["job.completed", "job.failed", "job.cancelled"],
    "active": true,
    "created_at": "2025-01-15T10:33:00Z"
  },
  "error": null,
  "timestamp": "2025-01-15T10:33:00Z"
}
```

### Webhook Events

#### Job Completed Event

```json
{
  "event": "job.completed",
  "timestamp": "2025-01-15T10:32:45Z",
  "data": {
    "job_id": "job_abc123def456",
    "story_id": "story_12345",
    "status": "completed",
    "video_url": "https://cdn.bluemarble.design/videos/story_12345.mp4",
    "thumbnail_url": "https://cdn.bluemarble.design/thumbnails/story_12345_thumb.jpg",
    "processing_time": 165
  }
}
```

#### Job Failed Event

```json
{
  "event": "job.failed",
  "timestamp": "2025-01-15T10:32:45Z",
  "data": {
    "job_id": "job_abc123def456",
    "story_id": "story_12345",
    "status": "failed",
    "error": "Image generation failed after 3 attempts",
    "attempts": 3
  }
}
```

## Error Handling

### Error Response Format

```json
{
  "success": false,
  "data": null,
  "error": {
    "code": "INVALID_CONFIGURATION",
    "message": "Invalid aspect ratio specified",
    "details": {
      "field": "aspect_ratio",
      "value": "9:18",
      "allowed_values": ["9:16", "16:9", "1:1", "4:5"]
    }
  },
  "timestamp": "2025-01-15T10:33:00Z"
}
```

### Error Codes

| Code | HTTP Status | Description |
|------|-------------|-------------|
| `INVALID_REQUEST` | 400 | Invalid request parameters |
| `INVALID_CONFIGURATION` | 400 | Invalid video generation configuration |
| `UNAUTHORIZED` | 401 | Missing or invalid authentication |
| `FORBIDDEN` | 403 | Insufficient permissions |
| `JOB_NOT_FOUND` | 404 | Requested job does not exist |
| `STORY_NOT_FOUND` | 404 | Story data not found |
| `CONFLICT` | 409 | Job in conflicting state |
| `RATE_LIMIT_EXCEEDED` | 429 | Too many requests |
| `PROCESSING_ERROR` | 500 | Internal processing error |
| `RENDERING_FAILED` | 500 | Video rendering failed |
| `STORAGE_ERROR` | 503 | Storage system unavailable |

## Rate Limiting

API requests are rate-limited per API key:

- **Free tier:** 10 requests/minute, 100 requests/hour
- **Standard tier:** 60 requests/minute, 1000 requests/hour
- **Premium tier:** 120 requests/minute, 5000 requests/hour

Rate limit headers are included in all responses:

```http
X-RateLimit-Limit: 60
X-RateLimit-Remaining: 45
X-RateLimit-Reset: 1642252800
```

When rate limit is exceeded:

```json
{
  "success": false,
  "data": null,
  "error": {
    "code": "RATE_LIMIT_EXCEEDED",
    "message": "Rate limit exceeded. Please retry after 2025-01-15T10:35:00Z",
    "details": {
      "limit": 60,
      "reset_at": "2025-01-15T10:35:00Z"
    }
  },
  "timestamp": "2025-01-15T10:33:00Z"
}
```

## SDK Examples

### Python SDK

```python
from bluemarble import VideoGenerationClient

# Initialize client
client = VideoGenerationClient(api_key="your_api_key")

# Submit story for video generation
job = client.video_pipeline.create_job(
    story_id="story_12345",
    story_data={
        "title": "The Lost Artifact",
        "content": "In a dusty ancient temple..."
    },
    config={
        "aspect_ratio": "9:16",
        "target_duration": 60,
        "quality": "high"
    },
    priority="normal"
)

print(f"Job created: {job.job_id}")

# Poll for completion
while job.status not in ["completed", "failed"]:
    job = client.video_pipeline.get_job(job.job_id)
    print(f"Status: {job.status}, Progress: {job.progress.percentage}%")
    time.sleep(5)

# Get result
if job.status == "completed":
    result = client.video_pipeline.get_result(job.job_id)
    print(f"Video URL: {result.video.url}")
    print(f"Thumbnail URL: {result.thumbnail.url}")
```

### JavaScript/Node.js SDK

```javascript
const { VideoGenerationClient } = require('@bluemarble/sdk');

// Initialize client
const client = new VideoGenerationClient({
  apiKey: 'your_api_key'
});

// Submit story for video generation
async function generateVideo() {
  const job = await client.videoPipeline.createJob({
    storyId: 'story_12345',
    storyData: {
      title: 'The Lost Artifact',
      content: 'In a dusty ancient temple...'
    },
    config: {
      aspectRatio: '9:16',
      targetDuration: 60,
      quality: 'high'
    },
    priority: 'normal'
  });

  console.log(`Job created: ${job.jobId}`);

  // Wait for completion
  const result = await client.videoPipeline.waitForJob(job.jobId);

  console.log(`Video URL: ${result.video.url}`);
  console.log(`Thumbnail URL: ${result.thumbnail.url}`);
}

generateVideo().catch(console.error);
```

### cURL Examples

#### Submit Job

```bash
curl -X POST https://api.bluemarble.design/v1/video-pipeline/jobs \
  -H "Authorization: Bearer your_api_token" \
  -H "Content-Type: application/json" \
  -d '{
    "story_id": "story_12345",
    "story_data": {
      "title": "The Lost Artifact",
      "content": "In a dusty ancient temple..."
    },
    "config": {
      "aspect_ratio": "9:16",
      "target_duration": 60,
      "quality": "high"
    },
    "priority": "normal"
  }'
```

#### Get Job Status

```bash
curl -X GET https://api.bluemarble.design/v1/video-pipeline/jobs/job_abc123def456 \
  -H "Authorization: Bearer your_api_token"
```

#### Get Job Result

```bash
curl -X GET https://api.bluemarble.design/v1/video-pipeline/jobs/job_abc123def456/result \
  -H "Authorization: Bearer your_api_token"
```

## Versioning

The API uses URL versioning. The current version is `v1`.

Future versions will be accessible via:
- `/v2/video-pipeline/...`
- `/v3/video-pipeline/...`

Version 1 will be supported for at least 12 months after a new version is released.

## Changelog

### v1.0 (2025-01-15)
- Initial API release
- Job submission and tracking
- Configuration management
- Asset retrieval
- Publishing integration
- Webhook support
- Analytics endpoints

## Related Documentation

- [Video Generation Pipeline System Specification](spec-video-generation-pipeline.md)
- [Technical Implementation Guide](tech-video-generation-pipeline.md)
- [API Authentication Guide](api-authentication.md)
- [Rate Limiting Policy](api-rate-limiting.md)

## Support

For API support:
- Documentation: https://docs.bluemarble.design/video-pipeline
- Email: api-support@bluemarble.design
- Status Page: https://status.bluemarble.design
