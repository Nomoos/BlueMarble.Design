# Render

This directory contains rendering configurations and intermediate files for the StoryGenerator pipeline.

## Purpose

The render stage takes storyboards and assets to produce the final video content through composition, effects, and rendering.

## Structure

Rendering should be organized by project:
- `project-name/` - Directory for each render project
  - `config.yaml` - Render configuration
  - `intermediate/` - Intermediate render files
  - `logs/` - Render logs and progress tracking
  - `preview/` - Preview renders for review

## Usage

1. Render projects are created based on storyboards and assets
2. Each render should include:
   - Composition settings
   - Effects and transitions
   - Timing and synchronization
   - Output specifications
3. Final rendered videos are moved to the `/output` directory

## Configuration

Render configurations should specify:
- Resolution and frame rate
- Codec and quality settings
- Effects and transitions
- Audio mixing settings
