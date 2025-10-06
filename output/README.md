# Output

This directory contains final rendered videos from the StoryGenerator pipeline.

## Purpose

The output stage stores completed videos that have been rendered and are ready for distribution or review.

## Structure

Outputs should be organized by project and version:
- `project-name/` - Directory for each project
  - `v1.0/` - Version directories
    - `final.mp4` - Final rendered video
    - `metadata.json` - Video metadata
    - `review-notes.md` - Review feedback and notes
  - `thumbnails/` - Video thumbnails
  - `subtitles/` - Subtitle files (SRT, VTT)

## Usage

1. Final videos are exported from the render stage
2. Each output should include:
   - The rendered video file
   - Metadata (duration, resolution, codec, etc.)
   - Thumbnails for preview
   - Any associated subtitle or caption files
3. Videos in this folder are ready for distribution

## File Naming Conventions

- Use descriptive names: `project-name-v1.0-final.mp4`
- Include version numbers for iterations
- Use standard video formats (MP4, MOV, AVI)
- Keep thumbnails named consistently: `project-name-thumbnail.jpg`
