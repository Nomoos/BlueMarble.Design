# Video Generation Pipeline - Technical Implementation Guide

**Document Type:** Technical Implementation Guide  
**Version:** 1.0  
**Author:** Content Systems Team  
**Date:** 2025-01-15  
**Status:** Draft  
**Related Specification:** [Video Generation Pipeline System Specification](spec-video-generation-pipeline.md)

## Overview

This document provides detailed implementation guidance for the Video Generation Pipeline, including code examples, architectural patterns, and integration strategies for implementing the system within your content generation workflow.

## Integration with StoryGenerator

### Insertion Point

The video pipeline integrates immediately after the StoryGenerator completes text generation:

```python
from story_generator import StoryGenerator
from video_pipeline import VideoGenerationPipeline

# Initialize components
story_generator = StoryGenerator(config)
video_pipeline = VideoGenerationPipeline(config)

# Generate story
story = story_generator.generate_story(prompt="Ancient mysteries")

# Feed story directly into video pipeline
video_job = video_pipeline.submit_job(story, priority="normal")

# Track progress
status = video_pipeline.get_job_status(video_job.id)
```

### Pipeline Orchestration

```python
class ContentGenerationOrchestrator:
    """
    Orchestrates the full content generation workflow from story to video.
    """
    
    def __init__(self, story_generator, video_pipeline):
        self.story_generator = story_generator
        self.video_pipeline = video_pipeline
        self.job_tracker = JobTracker()
    
    async def generate_content(self, prompt, config):
        """
        Full workflow: Generate story and create video.
        
        Args:
            prompt: Story generation prompt
            config: Configuration for both story and video
            
        Returns:
            dict: Results containing story and video data
        """
        # Step 1: Generate story
        logger.info(f"Generating story for prompt: {prompt}")
        story = await self.story_generator.generate(prompt, config.story)
        
        # Step 2: Submit to video pipeline
        logger.info(f"Submitting story {story.id} to video pipeline")
        video_job = await self.video_pipeline.submit_job(
            story_data=story,
            config=config.video
        )
        
        # Step 3: Track job
        self.job_tracker.add_job(video_job)
        
        # Step 4: Wait for completion (optional)
        if config.wait_for_completion:
            video_result = await self.video_pipeline.wait_for_job(video_job.id)
            return {
                "story": story,
                "video": video_result
            }
        else:
            return {
                "story": story,
                "video_job_id": video_job.id,
                "status": "processing"
            }
```

## Core Module Implementations

### 1. VideoRenderer Module

The VideoRenderer is responsible for converting composed scenes into final video files.

```python
import cv2
import numpy as np
from moviepy.editor import ImageClip, AudioFileClip, concatenate_videoclips
from typing import List, Dict, Any
import logging

logger = logging.getLogger(__name__)

class VideoRenderer:
    """
    Renders final video from composed scenes.
    """
    
    def __init__(self, config):
        self.config = config
        self.fps = config.get('fps', 30)
        self.codec = config.get('codec', 'libx264')
        self.audio_codec = config.get('audio_codec', 'aac')
        self.bitrate = config.get('bitrate', '8000k')
    
    def render_video(
        self,
        scenes: List[Dict[str, Any]],
        output_path: str,
        audio_path: str = None
    ) -> Dict[str, Any]:
        """
        Render video from scenes.
        
        Args:
            scenes: List of scene dictionaries with image paths and durations
            output_path: Path for output video file
            audio_path: Optional path to audio file
            
        Returns:
            dict: Rendering results with video information
        """
        try:
            logger.info(f"Starting video rendering with {len(scenes)} scenes")
            
            # Create video clips from scenes
            video_clips = []
            for i, scene in enumerate(scenes):
                logger.debug(f"Processing scene {i+1}/{len(scenes)}")
                
                # Create image clip
                clip = ImageClip(scene['image_path'])
                clip = clip.set_duration(scene['duration'])
                
                # Apply effects
                if scene.get('effects'):
                    clip = self._apply_effects(clip, scene['effects'])
                
                # Add text overlay
                if scene.get('text'):
                    clip = self._add_text_overlay(clip, scene['text'])
                
                video_clips.append(clip)
            
            # Concatenate all clips
            logger.info("Concatenating video clips")
            final_video = concatenate_videoclips(
                video_clips,
                method="compose"
            )
            
            # Add audio if provided
            if audio_path:
                logger.info(f"Adding audio track from {audio_path}")
                audio_clip = AudioFileClip(audio_path)
                final_video = final_video.set_audio(audio_clip)
            
            # Write video file
            logger.info(f"Writing video to {output_path}")
            final_video.write_videofile(
                output_path,
                fps=self.fps,
                codec=self.codec,
                audio_codec=self.audio_codec,
                bitrate=self.bitrate,
                preset='medium',
                threads=4
            )
            
            # Get video info
            video_info = self._get_video_info(output_path)
            
            logger.info(f"Video rendering completed: {output_path}")
            return {
                "status": "success",
                "output_path": output_path,
                "video_info": video_info
            }
            
        except Exception as e:
            logger.error(f"Video rendering failed: {str(e)}")
            raise RenderingError(f"Failed to render video: {str(e)}")
    
    def _apply_effects(self, clip, effects: List[str]):
        """Apply visual effects to clip."""
        for effect in effects:
            if effect == 'fade_in':
                clip = clip.fadein(0.5)
            elif effect == 'fade_out':
                clip = clip.fadeout(0.5)
            elif effect == 'ken_burns':
                # Slow zoom effect
                clip = clip.resize(lambda t: 1 + 0.05 * t)
        return clip
    
    def _add_text_overlay(self, clip, text_config: Dict):
        """Add text overlay to clip."""
        from moviepy.editor import TextClip, CompositeVideoClip
        
        text_clip = TextClip(
            text_config['content'],
            fontsize=text_config.get('size', 48),
            color=text_config.get('color', 'white'),
            font=text_config.get('font', 'Arial-Bold'),
            method='caption',
            size=(clip.w * 0.9, None),
            align='center'
        )
        
        # Position text
        position = text_config.get('position', 'bottom')
        if position == 'bottom':
            text_clip = text_clip.set_position(('center', clip.h * 0.8))
        elif position == 'center':
            text_clip = text_clip.set_position('center')
        
        text_clip = text_clip.set_duration(clip.duration)
        
        # Apply text animation
        if text_config.get('animation') == 'fade_in':
            text_clip = text_clip.fadein(0.5)
        
        # Composite video with text
        return CompositeVideoClip([clip, text_clip])
    
    def _get_video_info(self, video_path: str) -> Dict:
        """Extract video file information."""
        import os
        from moviepy.editor import VideoFileClip
        
        clip = VideoFileClip(video_path)
        
        return {
            "duration": clip.duration,
            "resolution": (clip.w, clip.h),
            "fps": clip.fps,
            "file_size": os.path.getsize(video_path),
            "aspect_ratio": f"{clip.w}:{clip.h}"
        }


class RenderingError(Exception):
    """Custom exception for rendering errors."""
    pass
```

### 2. SceneComposer Module

The SceneComposer organizes and prepares scenes for rendering.

```python
from typing import List, Dict, Any
import logging

logger = logging.getLogger(__name__)

class SceneComposer:
    """
    Composes scenes from story content and generated assets.
    """
    
    def __init__(self, config):
        self.config = config
        self.default_scene_duration = config.get('default_scene_duration', 5.0)
        self.transition_duration = config.get('transition_duration', 0.5)
    
    def compose_scenes(
        self,
        story_scenes: List[Dict],
        generated_assets: Dict[str, Any]
    ) -> List[Dict[str, Any]]:
        """
        Compose scenes from story data and assets.
        
        Args:
            story_scenes: List of scene definitions from story processor
            generated_assets: Dictionary of generated images and audio
            
        Returns:
            List of composed scenes ready for rendering
        """
        composed_scenes = []
        
        for i, scene in enumerate(story_scenes):
            logger.debug(f"Composing scene {i+1}/{len(story_scenes)}")
            
            composed_scene = self._compose_single_scene(
                scene,
                generated_assets,
                scene_number=i
            )
            
            composed_scenes.append(composed_scene)
        
        # Calculate timing
        composed_scenes = self._calculate_timing(composed_scenes)
        
        logger.info(f"Composed {len(composed_scenes)} scenes")
        return composed_scenes
    
    def _compose_single_scene(
        self,
        scene: Dict,
        assets: Dict,
        scene_number: int
    ) -> Dict[str, Any]:
        """Compose a single scene with all elements."""
        
        scene_id = scene['scene_id']
        
        # Get or generate image
        image_path = assets.get('images', {}).get(scene_id)
        if not image_path:
            logger.warning(f"No image found for scene {scene_id}, using fallback")
            image_path = self._create_fallback_image(scene)
        
        # Build composed scene
        composed = {
            "scene_id": scene_id,
            "image_path": image_path,
            "duration": scene.get('duration', self.default_scene_duration),
            "effects": scene.get('effects', ['fade_in', 'fade_out']),
            "text": {
                "content": scene.get('text', ''),
                "position": scene.get('text_position', 'bottom'),
                "size": 48,
                "color": "white",
                "font": "Arial-Bold",
                "animation": "fade_in"
            },
            "transition": scene.get('transition', 'fade')
        }
        
        return composed
    
    def _calculate_timing(self, scenes: List[Dict]) -> List[Dict]:
        """Calculate start and end times for each scene."""
        current_time = 0.0
        
        for scene in scenes:
            scene['start_time'] = current_time
            scene['end_time'] = current_time + scene['duration']
            current_time = scene['end_time']
        
        return scenes
    
    def _create_fallback_image(self, scene: Dict) -> str:
        """Create fallback image if generation failed."""
        from PIL import Image, ImageDraw, ImageFont
        import os
        import tempfile
        
        # Create solid background
        width, height = 1080, 1920
        image = Image.new('RGB', (width, height), color='#1a1a2e')
        draw = ImageDraw.Draw(image)
        
        # Add text
        text = scene.get('text', 'Scene content')
        font_size = 60
        try:
            font = ImageFont.truetype("Arial.ttf", font_size)
        except:
            font = ImageFont.load_default()
        
        # Calculate text position (centered)
        bbox = draw.textbbox((0, 0), text, font=font)
        text_width = bbox[2] - bbox[0]
        text_height = bbox[3] - bbox[1]
        x = (width - text_width) / 2
        y = (height - text_height) / 2
        
        # Draw text
        draw.text((x, y), text, fill='white', font=font)
        
        # Save to temp file
        temp_path = os.path.join(
            tempfile.gettempdir(),
            f"fallback_{scene['scene_id']}.png"
        )
        image.save(temp_path)
        
        return temp_path
```

### 3. Batch Processing Implementation

Implement a job queue system for handling multiple video generation requests:

```python
import asyncio
from typing import Dict, List, Optional
from dataclasses import dataclass
from enum import Enum
import logging

logger = logging.getLogger(__name__)

class JobStatus(Enum):
    """Job status enumeration."""
    QUEUED = "queued"
    PROCESSING = "processing"
    COMPLETED = "completed"
    FAILED = "failed"
    CANCELLED = "cancelled"

@dataclass
class VideoJob:
    """Video generation job."""
    job_id: str
    story_id: str
    story_data: Dict
    config: Dict
    priority: str = "normal"
    status: JobStatus = JobStatus.QUEUED
    attempts: int = 0
    max_attempts: int = 3
    error_message: Optional[str] = None
    result: Optional[Dict] = None

class VideoGenerationQueue:
    """
    Async queue for video generation jobs.
    """
    
    def __init__(self, max_workers: int = 4):
        self.max_workers = max_workers
        self.queue = asyncio.PriorityQueue()
        self.jobs: Dict[str, VideoJob] = {}
        self.workers: List[asyncio.Task] = []
        self.active = False
    
    def submit_job(self, job: VideoJob) -> str:
        """Submit a job to the queue."""
        self.jobs[job.job_id] = job
        
        # Priority: high=0, normal=1, low=2
        priority = {"high": 0, "normal": 1, "low": 2}.get(job.priority, 1)
        
        self.queue.put_nowait((priority, job.job_id))
        logger.info(f"Job {job.job_id} submitted with priority {job.priority}")
        
        return job.job_id
    
    async def start_workers(self):
        """Start worker pool."""
        self.active = True
        self.workers = [
            asyncio.create_task(self._worker(i))
            for i in range(self.max_workers)
        ]
        logger.info(f"Started {self.max_workers} workers")
    
    async def stop_workers(self):
        """Stop all workers."""
        self.active = False
        for worker in self.workers:
            worker.cancel()
        await asyncio.gather(*self.workers, return_exceptions=True)
        logger.info("All workers stopped")
    
    async def _worker(self, worker_id: int):
        """Worker process for handling jobs."""
        logger.info(f"Worker {worker_id} started")
        
        while self.active:
            try:
                # Get next job from queue (with timeout)
                priority, job_id = await asyncio.wait_for(
                    self.queue.get(),
                    timeout=1.0
                )
                
                job = self.jobs[job_id]
                logger.info(f"Worker {worker_id} processing job {job_id}")
                
                # Update job status
                job.status = JobStatus.PROCESSING
                
                # Process job
                try:
                    result = await self._process_job(job)
                    job.status = JobStatus.COMPLETED
                    job.result = result
                    logger.info(f"Job {job_id} completed successfully")
                    
                except Exception as e:
                    job.attempts += 1
                    logger.error(f"Job {job_id} failed (attempt {job.attempts}): {e}")
                    
                    if job.attempts >= job.max_attempts:
                        job.status = JobStatus.FAILED
                        job.error_message = str(e)
                        logger.error(f"Job {job_id} failed permanently after {job.attempts} attempts")
                    else:
                        # Requeue for retry
                        job.status = JobStatus.QUEUED
                        priority = {"high": 0, "normal": 1, "low": 2}.get(job.priority, 1)
                        await asyncio.sleep(2 ** job.attempts)  # Exponential backoff
                        self.queue.put_nowait((priority, job_id))
                        logger.info(f"Job {job_id} requeued for retry")
                
            except asyncio.TimeoutError:
                continue  # No jobs available, continue waiting
            except asyncio.CancelledError:
                break
            except Exception as e:
                logger.error(f"Worker {worker_id} error: {e}")
    
    async def _process_job(self, job: VideoJob) -> Dict:
        """Process a single video generation job."""
        from .story_processor import StoryProcessor
        from .asset_generator import AssetGenerator
        from .scene_composer import SceneComposer
        from .video_renderer import VideoRenderer
        from .metadata_generator import MetadataGenerator
        
        # Initialize components
        story_processor = StoryProcessor(job.config)
        asset_generator = AssetGenerator(job.config)
        scene_composer = SceneComposer(job.config)
        video_renderer = VideoRenderer(job.config)
        metadata_generator = MetadataGenerator(job.config)
        
        # Process story
        scenes = story_processor.process(job.story_data)
        
        # Generate assets
        assets = await asset_generator.generate_assets(scenes)
        
        # Compose scenes
        composed_scenes = scene_composer.compose_scenes(scenes, assets)
        
        # Render video
        output_path = f"/var/videos/generated/{job.story_id}.mp4"
        video_result = video_renderer.render_video(
            composed_scenes,
            output_path,
            audio_path=assets.get('audio')
        )
        
        # Generate metadata and thumbnail
        metadata = metadata_generator.generate_metadata(
            job.story_data,
            video_result['video_info']
        )
        thumbnail_path = metadata_generator.generate_thumbnail(output_path)
        
        return {
            "video_path": video_result['output_path'],
            "thumbnail_path": thumbnail_path,
            "metadata": metadata,
            "video_info": video_result['video_info']
        }
    
    def get_job_status(self, job_id: str) -> Optional[Dict]:
        """Get status of a job."""
        job = self.jobs.get(job_id)
        if not job:
            return None
        
        return {
            "job_id": job.job_id,
            "status": job.status.value,
            "attempts": job.attempts,
            "error_message": job.error_message,
            "result": job.result
        }
```

### 4. Error Handling Implementation

Comprehensive error handling with fallback mechanisms:

```python
import logging
from typing import Optional, Dict, Any

logger = logging.getLogger(__name__)

class PipelineError(Exception):
    """Base exception for pipeline errors."""
    pass

class ImageGenerationError(PipelineError):
    """Error in image generation."""
    pass

class TTSError(PipelineError):
    """Error in TTS generation."""
    pass

class RenderingError(PipelineError):
    """Error in video rendering."""
    pass

class ErrorHandler:
    """
    Centralized error handling with fallback strategies.
    """
    
    def __init__(self, config):
        self.config = config
        self.fallback_enabled = config.get('fallback_enabled', True)
    
    def handle_image_generation_error(
        self,
        error: Exception,
        scene_data: Dict
    ) -> str:
        """
        Handle image generation failure.
        
        Returns path to fallback image if enabled, otherwise raises.
        """
        logger.warning(
            f"Image generation failed for scene {scene_data['scene_id']}: {error}"
        )
        
        if not self.fallback_enabled:
            raise ImageGenerationError(f"Image generation failed: {error}")
        
        # Create fallback image
        logger.info("Creating fallback image with text overlay")
        return self._create_text_on_background(
            scene_data.get('text', 'Scene content'),
            scene_data['scene_id']
        )
    
    def handle_tts_error(
        self,
        error: Exception,
        scenes: List[Dict]
    ) -> Optional[str]:
        """
        Handle TTS generation failure.
        
        Returns None (silent video) if fallback enabled, otherwise raises.
        """
        logger.warning(f"TTS generation failed: {error}")
        
        if not self.fallback_enabled:
            raise TTSError(f"TTS generation failed: {error}")
        
        # Adjust scene durations for reading time
        logger.info("TTS failed, creating silent video with extended scene durations")
        for scene in scenes:
            # Increase duration by 50% to allow reading
            scene['duration'] *= 1.5
        
        return None  # No audio
    
    def handle_rendering_error(
        self,
        error: Exception,
        job_data: Dict
    ) -> None:
        """
        Handle video rendering failure.
        
        Rendering errors are critical and cannot be recovered.
        """
        logger.error(f"Video rendering failed: {error}")
        
        # Log detailed information for debugging
        logger.error(f"Job data: {job_data}")
        
        # Raise error to be handled by retry mechanism
        raise RenderingError(f"Video rendering failed: {error}")
    
    def _create_text_on_background(
        self,
        text: str,
        scene_id: str,
        background_color: str = "#1a1a2e"
    ) -> str:
        """Create fallback image with text on solid background."""
        from PIL import Image, ImageDraw, ImageFont
        import os
        import tempfile
        
        width, height = 1080, 1920
        image = Image.new('RGB', (width, height), color=background_color)
        draw = ImageDraw.Draw(image)
        
        # Word wrap text
        words = text.split()
        lines = []
        current_line = []
        max_width = 900
        
        try:
            font = ImageFont.truetype("Arial.ttf", 60)
        except:
            font = ImageFont.load_default()
        
        for word in words:
            current_line.append(word)
            line_text = ' '.join(current_line)
            bbox = draw.textbbox((0, 0), line_text, font=font)
            line_width = bbox[2] - bbox[0]
            
            if line_width > max_width:
                current_line.pop()
                lines.append(' '.join(current_line))
                current_line = [word]
        
        if current_line:
            lines.append(' '.join(current_line))
        
        # Calculate total text height
        line_height = 80
        total_height = len(lines) * line_height
        start_y = (height - total_height) / 2
        
        # Draw each line
        for i, line in enumerate(lines):
            bbox = draw.textbbox((0, 0), line, font=font)
            line_width = bbox[2] - bbox[0]
            x = (width - line_width) / 2
            y = start_y + i * line_height
            draw.text((x, y), line, fill='white', font=font)
        
        # Save to temp file
        temp_path = os.path.join(
            tempfile.gettempdir(),
            f"fallback_{scene_id}.png"
        )
        image.save(temp_path)
        
        return temp_path
```

### 5. Metadata and Thumbnail Generation

```python
from typing import Dict, Any
from datetime import datetime
import os
import logging

logger = logging.getLogger(__name__)

class MetadataGenerator:
    """
    Generates metadata and thumbnails for videos.
    """
    
    def __init__(self, config):
        self.config = config
    
    def generate_metadata(
        self,
        story_data: Dict,
        video_info: Dict
    ) -> Dict[str, Any]:
        """Generate comprehensive metadata for video."""
        
        # Extract story information
        title = story_data.get('title', 'Untitled Story')
        content = story_data.get('content', '')
        genre = story_data.get('metadata', {}).get('genre', 'general')
        
        # Generate description
        description = self._generate_description(content)
        
        # Generate tags and hashtags
        tags = self._extract_tags(content, genre)
        hashtags = self._generate_hashtags(genre, tags)
        
        metadata = {
            "title": title,
            "description": description,
            "tags": tags,
            "hashtags": hashtags,
            "duration": video_info['duration'],
            "created_at": datetime.now().isoformat(),
            "format": {
                "resolution": video_info['resolution'],
                "aspect_ratio": video_info['aspect_ratio'],
                "codec": "h264",
                "file_size": video_info['file_size']
            },
            "genre": genre,
            "source": "story_generator"
        }
        
        logger.info(f"Generated metadata for '{title}'")
        return metadata
    
    def generate_thumbnail(
        self,
        video_path: str,
        timestamp: float = 2.0,
        size: tuple = (1080, 1920)
    ) -> str:
        """Generate thumbnail from video frame."""
        from moviepy.editor import VideoFileClip
        from PIL import Image, ImageDraw, ImageFont
        import tempfile
        
        # Extract frame from video
        clip = VideoFileClip(video_path)
        frame = clip.get_frame(timestamp)
        
        # Convert to PIL Image
        image = Image.fromarray(frame)
        
        # Resize if needed
        if image.size != size:
            image = image.resize(size, Image.Resampling.LANCZOS)
        
        # Optional: Add title overlay
        if self.config.get('add_title_to_thumbnail', True):
            image = self._add_title_overlay(image)
        
        # Save thumbnail
        thumbnail_dir = os.path.dirname(video_path)
        video_name = os.path.splitext(os.path.basename(video_path))[0]
        thumbnail_path = os.path.join(thumbnail_dir, f"{video_name}_thumb.jpg")
        
        image.save(thumbnail_path, quality=90)
        
        logger.info(f"Generated thumbnail: {thumbnail_path}")
        return thumbnail_path
    
    def _generate_description(self, content: str, max_length: int = 200) -> str:
        """Generate concise description from story content."""
        # Take first few sentences
        sentences = content.split('. ')
        description = '. '.join(sentences[:2])
        
        if len(description) > max_length:
            description = description[:max_length-3] + "..."
        
        return description
    
    def _extract_tags(self, content: str, genre: str) -> list:
        """Extract relevant tags from content."""
        # Simple keyword extraction (in production, use NLP)
        tags = [genre]
        
        # Add common keywords
        keywords = [
            'story', 'adventure', 'mystery', 'discovery',
            'journey', 'exploration', 'ancient', 'artifact'
        ]
        
        content_lower = content.lower()
        for keyword in keywords:
            if keyword in content_lower:
                tags.append(keyword)
        
        return list(set(tags))[:10]  # Limit to 10 tags
    
    def _generate_hashtags(self, genre: str, tags: list) -> list:
        """Generate hashtags for social media."""
        hashtags = [f"#{genre}"]
        
        # Add tag-based hashtags
        for tag in tags[:5]:
            hashtags.append(f"#{tag.replace(' ', '')}")
        
        # Add general hashtags
        hashtags.extend([
            "#storytelling",
            "#shortfilm",
            "#aiart",
            "#creative"
        ])
        
        return hashtags
    
    def _add_title_overlay(self, image: Image.Image) -> Image.Image:
        """Add title text overlay to thumbnail."""
        from PIL import ImageDraw, ImageFont
        
        draw = ImageDraw.Draw(image)
        
        # Semi-transparent overlay bar
        overlay_height = 200
        overlay = Image.new('RGBA', (image.width, overlay_height), (0, 0, 0, 180))
        image.paste(overlay, (0, image.height - overlay_height), overlay)
        
        # Add title text (simplified - would need actual title)
        try:
            font = ImageFont.truetype("Arial-Bold.ttf", 48)
        except:
            font = ImageFont.load_default()
        
        text = "Story Video"
        bbox = draw.textbbox((0, 0), text, font=font)
        text_width = bbox[2] - bbox[0]
        x = (image.width - text_width) / 2
        y = image.height - overlay_height / 2 - 24
        
        draw.text((x, y), text, fill='white', font=font)
        
        return image
```

### 6. Publishing Integration

```python
from typing import Dict, List, Any
import logging
from abc import ABC, abstractmethod

logger = logging.getLogger(__name__)

class Publisher(ABC):
    """Abstract base class for platform publishers."""
    
    @abstractmethod
    async def upload(self, video_data: Dict) -> Dict:
        """Upload video to platform."""
        pass
    
    @abstractmethod
    def validate(self, video_data: Dict) -> bool:
        """Validate video meets platform requirements."""
        pass

class InstagramPublisher(Publisher):
    """Publisher for Instagram (Stories, Reels, Posts)."""
    
    def __init__(self, credentials: Dict):
        self.credentials = credentials
        self.max_file_size = 100 * 1024 * 1024  # 100MB
        self.max_duration = 90  # seconds for Reels
    
    def validate(self, video_data: Dict) -> bool:
        """Validate video for Instagram."""
        video_info = video_data['video_info']
        
        # Check file size
        if video_info['file_size'] > self.max_file_size:
            logger.warning("Video exceeds Instagram file size limit")
            return False
        
        # Check duration
        if video_info['duration'] > self.max_duration:
            logger.warning("Video exceeds Instagram duration limit")
            return False
        
        # Check aspect ratio (9:16 for Reels/Stories)
        if video_info['aspect_ratio'] not in ['9:16', '1:1', '4:5']:
            logger.warning("Video aspect ratio not ideal for Instagram")
            return False
        
        return True
    
    async def upload(self, video_data: Dict) -> Dict:
        """
        Upload video to Instagram.
        
        Note: Instagram API has strict limitations. This is a placeholder
        for actual implementation which may require manual posting or
        approved business accounts.
        """
        logger.warning(
            "Instagram automation is restricted. "
            "Consider manual posting or approved scheduling tools."
        )
        
        # In production, would use Instagram Graph API
        # For now, store in publishing queue for manual handling
        return {
            "status": "queued_manual",
            "message": "Video queued for manual Instagram posting",
            "video_path": video_data['video_path']
        }

class PublishingManager:
    """
    Manages video publishing to multiple platforms.
    """
    
    def __init__(self, config: Dict):
        self.config = config
        self.publishers = {}
        
        # Initialize publishers based on config
        if config.get('instagram_enabled'):
            self.publishers['instagram'] = InstagramPublisher(
                config['instagram_credentials']
            )
        
        # Add other publishers as needed
    
    async def publish_video(
        self,
        video_data: Dict,
        target_platforms: List[str]
    ) -> Dict[str, Any]:
        """
        Publish video to specified platforms.
        
        Args:
            video_data: Video file and metadata
            target_platforms: List of platform names
            
        Returns:
            dict: Publishing results per platform
        """
        results = {}
        
        for platform in target_platforms:
            publisher = self.publishers.get(platform)
            
            if not publisher:
                results[platform] = {
                    "status": "error",
                    "message": f"Publisher not configured for {platform}"
                }
                continue
            
            try:
                # Validate video for platform
                if not publisher.validate(video_data):
                    results[platform] = {
                        "status": "failed",
                        "message": "Video does not meet platform requirements"
                    }
                    continue
                
                # Upload to platform
                result = await publisher.upload(video_data)
                results[platform] = result
                
                logger.info(f"Published to {platform}: {result}")
                
            except Exception as e:
                logger.error(f"Publishing failed for {platform}: {e}")
                results[platform] = {
                    "status": "error",
                    "message": str(e)
                }
        
        return results
```

## Configuration

### Complete Configuration Example

```yaml
# config/video_pipeline.yaml

video_pipeline:
  # Processing settings
  processing:
    max_concurrent_jobs: 4
    job_timeout: 600  # 10 minutes
    retry_attempts: 3
    retry_backoff_multiplier: 2
  
  # Video settings
  video:
    fps: 30
    codec: "libx264"
    audio_codec: "aac"
    bitrate: "8000k"
    preset: "medium"  # faster, fast, medium, slow, slower
    default_aspect_ratio: "9:16"
    default_duration: 60
    resolutions:
      portrait: [1080, 1920]
      square: [1080, 1080]
      landscape: [1920, 1080]
  
  # Image generation
  images:
    service: "stable_diffusion"  # or "dalle", "midjourney"
    api_endpoint: "https://api.stability.ai/v1"
    max_retries: 3
    timeout: 30
    fallback_enabled: true
    cache_enabled: true
    cache_ttl: 604800  # 7 days
  
  # Text-to-speech
  tts:
    service: "google_tts"  # or "amazon_polly", "azure_speech"
    voice: "en-US-Neural2-C"
    speech_rate: 1.0
    pitch: 0
    fallback_enabled: true
    cache_enabled: true
    cache_ttl: 2592000  # 30 days
  
  # Scene composition
  scenes:
    default_duration: 5.0
    transition_duration: 0.5
    text_position: "bottom"
    text_size: 48
    text_color: "#FFFFFF"
    text_font: "Arial-Bold"
  
  # Publishing
  publishing:
    auto_publish: false
    platforms:
      - instagram
      - youtube
    scheduling_enabled: true
  
  # Storage
  storage:
    output_path: "/var/videos/generated"
    cache_path: "/var/cache/video_pipeline"
    temp_path: "/tmp/video_pipeline"
    max_storage_gb: 500
  
  # Error handling
  error_handling:
    fallback_enabled: true
    log_level: "INFO"
    alert_on_failure: true
```

## Testing

### Unit Tests

```python
import unittest
from unittest.mock import Mock, patch
from video_pipeline import VideoRenderer, SceneComposer

class TestVideoRenderer(unittest.TestCase):
    
    def setUp(self):
        self.config = {
            'fps': 30,
            'codec': 'libx264',
            'bitrate': '8000k'
        }
        self.renderer = VideoRenderer(self.config)
    
    def test_render_video_success(self):
        """Test successful video rendering."""
        scenes = [
            {
                'image_path': '/tmp/scene1.png',
                'duration': 5.0,
                'text': {'content': 'Scene 1'}
            }
        ]
        
        result = self.renderer.render_video(
            scenes,
            '/tmp/output.mp4'
        )
        
        self.assertEqual(result['status'], 'success')
        self.assertIn('output_path', result)

class TestSceneComposer(unittest.TestCase):
    
    def setUp(self):
        self.config = {
            'default_scene_duration': 5.0
        }
        self.composer = SceneComposer(self.config)
    
    def test_compose_scenes(self):
        """Test scene composition."""
        story_scenes = [
            {
                'scene_id': 'scene_001',
                'text': 'Scene 1 text',
                'duration': 5.0
            }
        ]
        assets = {
            'images': {'scene_001': '/tmp/scene1.png'}
        }
        
        composed = self.composer.compose_scenes(story_scenes, assets)
        
        self.assertEqual(len(composed), 1)
        self.assertIn('start_time', composed[0])
        self.assertIn('end_time', composed[0])

if __name__ == '__main__':
    unittest.main()
```

## Deployment

### Docker Container

```dockerfile
# Dockerfile
FROM python:3.10-slim

# Install system dependencies
RUN apt-get update && apt-get install -y \
    ffmpeg \
    imagemagick \
    fonts-liberation \
    && rm -rf /var/lib/apt/lists/*

# Set working directory
WORKDIR /app

# Copy requirements
COPY requirements.txt .
RUN pip install --no-cache-dir -r requirements.txt

# Copy application code
COPY . .

# Create directories
RUN mkdir -p /var/videos/generated /var/cache/video_pipeline

# Set environment variables
ENV PYTHONUNBUFFERED=1
ENV VIDEO_OUTPUT_PATH=/var/videos/generated
ENV CACHE_PATH=/var/cache/video_pipeline

# Run application
CMD ["python", "video_pipeline_worker.py"]
```

### Kubernetes Deployment

```yaml
# k8s/video-pipeline-deployment.yaml
apiVersion: apps/v1
kind: Deployment
metadata:
  name: video-pipeline-worker
spec:
  replicas: 4
  selector:
    matchLabels:
      app: video-pipeline-worker
  template:
    metadata:
      labels:
        app: video-pipeline-worker
    spec:
      containers:
      - name: worker
        image: video-pipeline:latest
        resources:
          requests:
            memory: "4Gi"
            cpu: "2"
          limits:
            memory: "8Gi"
            cpu: "4"
        volumeMounts:
        - name: video-storage
          mountPath: /var/videos
        - name: cache-storage
          mountPath: /var/cache
        env:
        - name: MAX_CONCURRENT_JOBS
          value: "1"
        - name: REDIS_URL
          valueFrom:
            secretKeyRef:
              name: video-pipeline-secrets
              key: redis-url
      volumes:
      - name: video-storage
        persistentVolumeClaim:
          claimName: video-storage-pvc
      - name: cache-storage
        persistentVolumeClaim:
          claimName: cache-storage-pvc
```

## Monitoring and Observability

### Metrics Collection

```python
from prometheus_client import Counter, Histogram, Gauge
import time

# Define metrics
videos_generated = Counter(
    'videos_generated_total',
    'Total number of videos generated'
)

video_generation_duration = Histogram(
    'video_generation_duration_seconds',
    'Time spent generating videos'
)

active_jobs = Gauge(
    'active_video_jobs',
    'Number of currently active video generation jobs'
)

generation_errors = Counter(
    'video_generation_errors_total',
    'Total number of video generation errors',
    ['error_type']
)

# Usage in pipeline
async def process_video_with_metrics(job):
    active_jobs.inc()
    start_time = time.time()
    
    try:
        result = await process_video(job)
        videos_generated.inc()
        return result
    except Exception as e:
        generation_errors.labels(error_type=type(e).__name__).inc()
        raise
    finally:
        duration = time.time() - start_time
        video_generation_duration.observe(duration)
        active_jobs.dec()
```

## Performance Optimization Tips

1. **Use GPU acceleration** for video encoding when available
2. **Implement caching** for generated images and TTS audio
3. **Parallel processing** for independent assets
4. **Optimize image resolution** before rendering
5. **Use efficient video codecs** (H.264 with hardware encoding)
6. **Batch processing** for multiple videos
7. **CDN delivery** for final videos
8. **Monitor resource usage** and scale workers accordingly

## Troubleshooting

### Common Issues

**Problem:** Video rendering is slow
- **Solution:** Enable GPU acceleration, reduce resolution, or increase worker count

**Problem:** Image generation fails frequently
- **Solution:** Check API rate limits, implement better retry logic, ensure fallback is enabled

**Problem:** Audio/video sync issues
- **Solution:** Verify audio duration matches scene timing, use fixed frame rate

**Problem:** High memory usage
- **Solution:** Process scenes individually, clear cache regularly, limit concurrent jobs

## References

- FFmpeg Documentation: https://ffmpeg.org/documentation.html
- MoviePy Documentation: https://zulko.github.io/moviepy/
- OpenCV Documentation: https://docs.opencv.org/
- PIL/Pillow Documentation: https://pillow.readthedocs.io/
- asyncio Documentation: https://docs.python.org/3/library/asyncio.html
