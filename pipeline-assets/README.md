# Pipeline Assets

This directory contains media assets for the StoryGenerator pipeline.

## Purpose

The pipeline-assets stage collects, generates, or stores all media files needed for video production, including images, audio, video clips, and other resources.

## Structure

Assets should be organized by type and project:
- `images/` - Image files (backgrounds, characters, objects)
- `audio/` - Audio files (music, sound effects, voiceovers)
- `video/` - Video clips and animations
- `fonts/` - Font files
- `project-name/` - Project-specific assets

## Usage

1. Assets are collected or generated based on storyboards
2. Supported formats:
   - Images: PNG, JPG, SVG
   - Audio: MP3, WAV, OGG
   - Video: MP4, AVI, MOV
3. Assets from this folder are used during the rendering stage

## Notes

- Keep assets organized by project to avoid confusion
- Use descriptive filenames
- Consider file size and optimization
- This directory is separate from `/assets` which contains design mockups and documentation assets
