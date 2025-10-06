# StoryGenerator Pipeline - Repository Structure

This document describes the folder structure for the StoryGenerator pipeline, which transforms ideas into videos.

## Overview

The StoryGenerator pipeline follows a sequential workflow from idea conceptualization to final video output. Each stage has its own directory for organizing files and intermediate artifacts.

## Pipeline Stages

### 1. `/idea` - Idea Generation
Initial concept and story ideas before processing.
- **Input**: Raw concepts, themes, story ideas
- **Output**: Structured idea documents ready for script generation

### 2. `/script` - Script Generation
Transform ideas into structured narratives with dialogue and scene descriptions.
- **Input**: Idea documents from `/idea`
- **Output**: Formatted scripts with scenes, dialogue, and timing

### 3. `/storyboard` - Storyboard Creation
Visual planning and shot-by-shot breakdown of scripts.
- **Input**: Scripts from `/script`
- **Output**: Storyboards with frame descriptions and visual sequences

### 4. `/pipeline-assets` - Asset Collection
Media files required for video production.
- **Input**: Asset requirements from storyboards
- **Output**: Organized images, audio, video clips, and fonts
- **Note**: Separate from `/assets` which contains design documentation assets

### 5. `/render` - Video Rendering
Composition, effects, and rendering of final videos.
- **Input**: Storyboards and assets
- **Output**: Rendered video files and intermediate renders

### 6. `/output` - Final Output
Completed videos ready for distribution.
- **Input**: Rendered videos from `/render`
- **Output**: Final videos with metadata, thumbnails, and subtitles

## Configuration

### `/config` - Pipeline Configuration

Configuration files for the StoryGenerator pipeline:

- **`pipeline.yaml`**: Pipeline structure, stages, and global settings
- **`scoring.yaml`**: Quality evaluation criteria and thresholds

## Pipeline Flow

```
Idea → Script → Storyboard → Assets → Render → Output
  ↓       ↓         ↓          ↓        ↓        ↓
/idea  /script  /storyboard  /pipeline-  /render  /output
                              assets
```

## Configuration-Driven Behavior

The pipeline behavior is controlled by configuration files in `/config`:
- Stage enablement and ordering
- Quality thresholds and scoring
- Output specifications
- Format and resolution settings

## Integration with Existing Structure

The StoryGenerator pipeline directories complement the existing repository structure:
- **`/research`**: Research supporting pipeline development
- **`/design`**: Design vision for the pipeline
- **`/docs`**: Technical documentation for the pipeline
- **`/assets`**: Design assets (mockups, diagrams) - separate from pipeline assets
- **`/templates`**: Document templates for pipeline stages
- **`/scripts`**: Utility scripts for pipeline automation

## Getting Started

1. Review the configuration files in `/config`
2. Place initial ideas in `/idea`
3. Follow the pipeline stages sequentially
4. Use README files in each directory for stage-specific guidance

## Future Enhancements

- Automated pipeline orchestration
- Quality scoring integration
- Asset generation tools
- Template-based content creation
- CI/CD integration for automated processing
