# Configuration

This directory contains configuration files for the StoryGenerator pipeline.

## Purpose

The config directory stores YAML configuration files that define pipeline behavior, scoring criteria, and other settings.

## Files

### `pipeline.yaml`

Defines the structure and behavior of the video generation pipeline:
- Pipeline stages and their order
- Stage-specific configurations
- Global pipeline settings
- Output format specifications

### `scoring.yaml`

Defines scoring criteria for evaluating generated content:
- Quality metrics and weights
- Scoring thresholds
- Evaluation criteria
- Auto-approval settings

## Usage

1. Edit configuration files to customize pipeline behavior
2. Validate YAML syntax before committing changes
3. Document any configuration changes in commit messages
4. Test configuration changes in a development environment first

## Configuration Guidelines

- Use clear, descriptive keys
- Include comments for complex settings
- Keep default values reasonable
- Document units and ranges for numeric values
- Version configuration files when making breaking changes
