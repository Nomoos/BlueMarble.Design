# Folder Structure Guide

This guide clarifies the purpose and organization of the BlueMarble.Design repository structure.

## Overview

The repository is organized into three primary content directories, each serving a distinct purpose in the design and development workflow:

```
BlueMarble.Design/
├── research/         # All research activities
├── design/          # High-level design vision
├── docs/            # Technical implementation specs
├── roadmap/         # Strategic planning
├── templates/       # Document templates
├── assets/          # Design assets
├── config/          # StoryGenerator pipeline configuration
├── idea/            # Story ideas (pipeline stage 1)
├── script/          # Generated scripts (pipeline stage 2)
├── storyboard/      # Visual storyboards (pipeline stage 3)
├── pipeline-assets/ # Video production assets (pipeline stage 4)
├── render/          # Video rendering (pipeline stage 5)
└── output/          # Final videos (pipeline stage 6)
```

### Information Flow

```
┌─────────────┐
│  Research   │  ← Exploration, investigation, analysis
└──────┬──────┘
       │ Informs
       ↓
┌─────────────┐
│   Design    │  ← Vision, concepts, high-level direction
└──────┬──────┘
       │ Guides
       ↓
┌─────────────┐
│    Docs     │  ← Technical specs, implementation details
└──────┬──────┘
       │ Implements
       ↓
┌─────────────┐
│  Roadmap    │  ← Planning, prioritization, tracking
└─────────────┘
```

## Primary Directories

### `/research` - Research Hub

**Purpose**: Central location for all research activities, from exploratory notes to comprehensive analyses.

**What goes here**:
- Market research and competitive analysis
- Game design research and mechanics analysis
- Technical research (algorithms, data structures, etc.)
- Experiment logs and findings
- Literature reviews
- Bibliography and sources
- Informal research notes

**Structure**:
```
research/
├── topics/             # Small focused research notes (200-400 lines)
├── notes/              # Informal research notes and ideas
├── experiments/        # Structured experiment logs
├── literature/         # Formal references and reviews
├── sources/            # Bibliography, reading lists
├── market-research/    # Market analysis and competitive research
├── game-design/        # Game design research
├── spatial-data-storage/ # Technical research area
└── gpt-research/       # AI-assisted research
```

**Key characteristics**:
- Exploratory and investigative content
- Research questions and findings
- Analysis before decisions
- References to external sources

### `/design` - Design Vision Hub

**Purpose**: High-level game design concepts, vision, and foundational design thinking.

**What goes here**:
- Design pillars and core vision
- High-level mechanics concepts
- Conceptual economy and progression ideas
- Narrative concepts and world vision
- Style guides and visual direction
- Early-stage wireframes and mockups

**Structure**:
```
design/
├── pillars.md          # Core design pillars
├── mechanics.md        # Core gameplay concepts
├── economy.md          # Economic vision
├── narrative.md        # Story concepts
├── styleguide.md       # Visual design system
├── wireframes/         # UI/UX concept mockups
├── architecture/       # High-level architecture
└── ux/                 # User experience concepts
```

**Key characteristics**:
- Vision and direction (not detailed specs)
- "What we want to build" not "how we build it"
- Conceptual and aspirational
- Foundation for technical specs

### `/docs` - Technical Documentation

**Purpose**: Detailed technical implementation specifications and comprehensive documentation.

**What goes here**:
- Game Design Documents (GDD) with detailed mechanics
- Technical Design Documents (TDD)
- API specifications
- System architecture documentation
- Detailed feature specifications
- Implementation requirements
- QA documentation

**Structure**:
```
docs/
├── core/              # GDD, TDD, core documents
├── gameplay/          # Detailed gameplay mechanics
├── systems/           # Technical system specs
├── world/             # Detailed world building
├── ui-ux/             # Interface specifications
└── audio/             # Audio implementation specs
```

**Key characteristics**:
- Implementation-ready specifications
- Detailed technical requirements
- "How we build it" not just "what we want"
- Developer-facing documentation

## Decision Framework

### "Where should this document go?"

Ask yourself these questions:

1. **Is this exploratory or investigative?**
   - Yes → `/research`
   - Example: "Analysis of crafting systems in similar games"

2. **Is this a high-level concept or vision?**
   - Yes → `/design`
   - Example: "Core design pillars for economy system"

3. **Is this a detailed technical specification?**
   - Yes → `/docs`
   - Example: "API specification for crafting system"

4. **Is this planning or prioritization?**
   - Yes → `/roadmap`
   - Example: "Q2 2024 feature roadmap"

### Examples by Type

#### Market Research
- **Research Phase**: `research/market-research/mmorpg-analysis-2024.md`
- **Design Impact**: `design/economy.md` (updated based on research)
- **Technical Spec**: `docs/systems/economy-systems.md` (implementation)

#### New Feature Flow
1. **Research**: `research/topics/player-trading-analysis.md`
2. **Design Vision**: `design/marketplace-concept.md`
3. **Technical Spec**: `docs/gameplay/spec-player-marketplace.md`
4. **API Spec**: `docs/systems/api-marketplace.md`

#### Technical Investigation
1. **Research**: `research/spatial-data-storage/compression-analysis.md`
2. **Design Decision**: `design/architecture/data-storage-approach.md`
3. **Technical Spec**: `docs/systems/database-schema-design.md`

## Supporting Directories

### `/roadmap` - Strategic Planning

Project planning, milestones, and feature prioritization.

### `/templates` - Document Templates

Standardized templates for creating consistent documentation across all directories.

### `/assets` - Design Assets

Visual assets, mockups, diagrams, and reference materials.

### `.github` - Repository Configuration

GitHub-specific configuration including issue templates, workflows, and automation.

### StoryGenerator Pipeline Directories

The repository includes a complete video generation pipeline (see [STORYGENERATOR_PIPELINE.md](STORYGENERATOR_PIPELINE.md) for details):

- **`/config`**: Pipeline configuration (pipeline.yaml, scoring.yaml)
- **`/idea`**: Initial story concepts and ideas
- **`/script`**: Generated scripts from ideas
- **`/storyboard`**: Visual planning and shot breakdowns
- **`/pipeline-assets`**: Media files for video production (separate from `/assets`)
- **`/render`**: Video rendering and composition
- **`/output`**: Final rendered videos

The pipeline follows a sequential workflow: Idea → Script → Storyboard → Assets → Render → Output

## Migration from Old Structure

### What Changed

**Before**: Research was split between `/research` and `/docs/research`
- General research: `/research`
- Market research: `/docs/research`

**After**: All research consolidated in `/research`
- All research types: `/research` with subdirectories
- Market research: `/research/market-research`

### Updated Paths

| Old Path | New Path |
|----------|----------|
| `docs/research/market-research.md` | `research/market-research/market-research.md` |
| `docs/research/game_dev_repos.md` | `research/market-research/game_dev_repos.md` |
| `docs/research/voxel_games_sources.md` | `research/market-research/voxel_games_sources.md` |

## Best Practices

### File Naming

- Use **kebab-case**: `player-trading-system.md`
- Be **descriptive**: `crafting-system-mechanics-v2.md` not `crafting.md`
- Use **prefixes** for document types:
  - `spec-` for specifications
  - `gdd-` for game design documents
  - `tdd-` for technical design documents
  - `api-` for API specifications
  - `YYYY-MM-DD-` for dated documents (experiments, playtests)

### File Size

- Target **200-400 lines** for most documents
- Keep files **focused on one topic**
- Split large documents into multiple files
- Use index files to link related content

### Cross-Linking

Link liberally between related documents:
- Research → Design: Link to research that informed design decisions
- Design → Docs: Link to technical specs that implement the design
- Docs → Research: Link back to research for context

### Documentation Flow

1. **Research** first: Investigate and analyze
2. **Design** second: Define vision and concepts based on research
3. **Docs** third: Create detailed technical specifications
4. **Implementation** last: Build according to specs

## Related Documentation

- [README.md](README.md) - Repository overview
- [CONTRIBUTING.md](CONTRIBUTING.md) - Contribution guidelines
- [DOCUMENTATION_BEST_PRACTICES.md](DOCUMENTATION_BEST_PRACTICES.md) - Writing guidelines
- [research/README.md](research/README.md) - Research directory details
- [design/README.md](design/README.md) - Design directory details
- [docs/README.md](docs/README.md) - Technical documentation details
