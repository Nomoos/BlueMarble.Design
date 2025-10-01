# BlueMarble.Design

BlueMarble.Design is the research and design hub for BlueMarble, a top-down MMORPG. This repository gathers issues, documents, and research from across the BlueMarble ecosystem to generate structured design outputs. By combining exploration, documentation, and system concepts, it provides a solid foundation for features, mechanics, and world-building that will guide downstream development.

## Overview

This repository serves as the central hub for all design-related activities in the BlueMarble ecosystem, including:

- **Game Design Vision**: Core design documents and iterative design development in `/design`
- **Research & Exploration**: Market analysis, player behavior studies, and technical research
- **Design Documentation**: Comprehensive design documents covering all aspects of the game
- **System Concepts**: Core mechanics, progression systems, and gameplay loops
- **World Building**: Lore, narrative, and environmental design
- **Feature Specifications**: Detailed specifications that guide development teams

## Repository Structure

```
├── research/                # Research hub
│   ├── README.md           # Research guidelines
│   ├── index.md            # Master index of research
│   ├── topics/             # Small focused research notes (200-400 lines)
│   ├── notes/              # Informal research notes and ideas
│   ├── experiments/        # Structured experiment logs (YYYY-MM-DD-title.md)
│   ├── literature/         # Formal references and literature reviews
│   ├── sources/            # Bibliography, reading list, quotes
│   ├── game-design/        # Game design research area
│   ├── spatial-data-storage/ # Spatial data storage research
│   └── gpt-research/       # AI-assisted research
├── design/                  # Design documentation hub
│   ├── README.md           # Design guidelines
│   ├── index.md            # Design document index
│   ├── pillars.md          # High-level vision and pillars
│   ├── mechanics.md        # Core loops and systems
│   ├── economy.md          # Progression, currencies, balance
│   ├── narrative.md        # Story, world, characters
│   ├── styleguide.md       # Visual design system and style guide
│   ├── wireframes/         # UI/UX wireframes and screen mockups
│   │   └── ui-wireframes.md # Interface mockups (general)
│   ├── architecture/       # System architecture diagrams
│   ├── ux/                 # User experience design
│   │   └── controls.md     # Input schemes
│   ├── island_start_game_design.md # Existing design
│   └── one-page-design.md  # Existing design
├── roadmap/                 # Strategic planning and roadmap
│   ├── README.md           # Planning guidelines
│   ├── roadmap.md          # Big picture by quarter/phase
│   ├── milestones.md       # Dated deliverables
│   ├── milestones/         # Individual milestone definitions
│   ├── tasks/              # Detailed task breakdowns
│   ├── backlog.md          # Groomed research/design tasks
│   ├── project-roadmap.md  # Detailed project roadmap with tracking
│   ├── milestone-guide.md  # Milestone structure and tracking
│   ├── roadmap-management-guide.md # How to manage roadmap
│   └── feature-prioritization-guide.md # Feature prioritization
├── docs/                    # Comprehensive documentation
│   ├── core/               # Core design documents (GDD, TDD)
│   ├── gameplay/           # Gameplay mechanics and systems
│   ├── world/              # World building, lore, narrative
│   ├── systems/            # Core systems and technical design
│   ├── ui-ux/              # User interface and experience design
│   ├── audio/              # Audio design and music concepts
│   └── research/           # Market research and analysis
├── templates/               # Document templates
│   ├── research-note.md    # Small focused research (200-400 lines)
│   ├── design-doc.md       # Design document template
│   ├── experiment-report.md # Experiment logs
│   ├── playtest-report.md  # Playtest documentation
│   ├── decision-record.md  # ADR-style decisions
│   └── [existing templates] # Game design, feature specs, etc.
├── assets/                  # Design assets, mockups, references
├── scripts/                 # Utility scripts
└── .github/                 # GitHub configuration
    ├── ISSUE_TEMPLATE/     # Issue templates
    │   ├── bug_report.yml
    │   ├── chore.yml
    │   ├── design_task.yml
    │   └── research_note.yml
    ├── workflows/          # CI/CD workflows
    │   ├── quality.yml     # Markdown lint, link check, spellcheck
    │   ├── config-lint.yml # YAML/JSON validation
    │   └── autolabel.yml   # Auto-label and size PRs
    ├── CODEOWNERS          # Review assignments
    ├── PULL_REQUEST_TEMPLATE.md
    └── dependabot.yml      # Dependency updates
```

## Key Principles

### Small, Focused Files

- **Target size**: 200-400 lines (~500-800 words)
- **One topic per file**: Each file covers a single concern
- **Easy to review**: Smaller files = clearer diffs, fewer merge conflicts
- **Use kebab-case**: `enemy-ai-overview.md`, `2025-09-30-combat-playtest.md`

### Front Matter

Include metadata at the top of research and design documents:

```markdown
---
title: Document Title
date: YYYY-MM-DD
owner: @username
status: draft | in-progress | complete
tags: [tag1, tag2, tag3]
---
```

### Cross-Linking

Link liberally between related documents. Each major directory has an `index.md` linking to its contents.

## Getting Started

### For Design Contributors

1. Review the [Contributing Guidelines](CONTRIBUTING.md) and [Documentation Best Practices](DOCUMENTATION_BEST_PRACTICES.md)
2. Check existing documentation in the relevant category
3. Use the appropriate template from the `templates/` directory
4. Follow the established naming conventions and structure
5. When completing issues, use the [Issue Completion Template](templates/issue-completion-template.md) to ensure proper documentation and communication

### For Research Contributors

1. Browse [research/](research/) to understand existing research areas
2. Check the [research index](research/index.md) for context
3. Use the [research-note template](templates/research-note.md) for focused topics
4. Use the [experiment-report template](templates/experiment-report.md) for experiments
5. Keep notes small (200-400 lines) and focused on one question
6. Name files with kebab-case: `topic-name.md` or `YYYY-MM-DD-experiment.md`
7. Update relevant index files when adding research

### For Developers

1. Browse the `docs/` directory for relevant design specifications
2. Check the `roadmap/` for current priorities and status
3. Reference design documents when implementing features
4. Provide feedback on design feasibility during the review process

## Documentation Standards

- **[Documentation Best Practices](DOCUMENTATION_BEST_PRACTICES.md)** - Guidelines for creating high-quality documentation
- **[Contributing Guidelines](CONTRIBUTING.md)** - How to contribute to the repository
- **[Usage Examples](USAGE_EXAMPLES.md)** - Practical examples of using the repository structure

## Key Documentation

### Technical Foundation
- **[Technical Foundation Overview](./docs/TECHNICAL_FOUNDATION.md)** - Comprehensive core system architecture documentation
- **[System Architecture Design](./docs/systems/system-architecture-design.md)** - Service-oriented architecture
- **[Database Schema Design](./docs/systems/database-schema-design.md)** - Hybrid database architecture
- **[API Specifications](./docs/systems/api-specifications.md)** - RESTful API design and protocols
- **[Security Framework Design](./docs/systems/security-framework-design.md)** - Security model and compliance

### Core Design Documents
- **[Game Design Document](./docs/core/game-design-document.md)** - Complete game vision and design
- **[Technical Design Document](./docs/core/technical-design-document.md)** - Technical architecture and requirements
- **[Documentation Index](./docs/README.md)** - Complete documentation structure

### System Specifications
- **[Gameplay Systems](./docs/systems/gameplay-systems.md)** - Combat, progression, and core mechanics
- **[Economy Systems](./docs/systems/economy-systems.md)** - Trading, crafting, and economic balance
- **[Marketplace API](./docs/systems/api-marketplace.md)** - Player trading marketplace API specification
- **[UI/UX Guidelines](./docs/ui-ux/ui-guidelines.md)** - Interface design and user experience standards

### World Design
- **[World Lore](./docs/world/world-lore.md)** - Setting, history, and cultural elements
- **[Audio Guidelines](./docs/audio/audio-guidelines.md)** - Music and sound design principles

### Research Foundation
- **[Market Research](./docs/research/market-research.md)** - Competitive analysis and market positioning

## Document Types

- **Game Design Documents (GDD)**: High-level game design and vision
- **Technical Design Documents (TDD)**: System architecture and implementation details
- **Feature Specifications**: Detailed feature requirements and user stories
- **Research Reports**: Market analysis, player studies, and technical research
- **Art Direction**: Visual style guides and artistic vision
- **Audio Design**: Sound design principles and music direction

## Workflow

### General Workflow

1. **Research Phase**: Gather requirements and conduct necessary research
2. **Design Phase**: Create structured design documents using templates
3. **Review Phase**: Collaborate with stakeholders for feedback and iteration
4. **Approval Phase**: Finalize documents for development implementation
5. **Maintenance Phase**: Keep documents updated as implementation progresses

### Detailed Workflow Guidelines

#### Research Workflow

1. Add research notes in `research/notes/` as single-topic files
2. Place formal references and summaries in `research/literature/`
3. Record experiment results in `research/experiments/`
4. Keep files **small and focused on one topic** (200-400 lines)
5. Use descriptive names (e.g., `database-architecture.md`, `mmo-retention-study.md`)
6. Add cross-links between related files where useful
7. Update `research/index.md` when adding significant research

#### Design Workflow

1. Document design artifacts (wireframes, diagrams, styleguides) in `design/`
2. Store wireframes and mockups in `design/wireframes/`
3. Place architecture diagrams in `design/architecture/`
4. Keep the style guide updated in `design/styleguide.md`
5. Use descriptive names (e.g., `login-wireframe.md`, `api-architecture.md`)
6. Update `design/index.md` when adding new design documents

#### Roadmap Workflow

1. Break down the roadmap into milestones and tasks in `roadmap/`
2. Create milestone definitions in `roadmap/milestones/`
3. Define detailed tasks in `roadmap/tasks/`
4. Update milestone status as work progresses
5. Link tasks to relevant design and research documents

### Quality Checks

Before committing documentation changes, run the quality check script:

```bash
./scripts/check-documentation-quality.sh
```

This script validates:

- Required files and directory structure
- Markdown formatting and linting
- Duplicate content detection (headings and full file comparison)
- Broken internal links
- Small or stub files that need more content
- File organization and proper README placement

See [scripts/README.md](scripts/README.md) for more information.

### Automated PR Management

This repository includes automated PR management to keep pull requests up-to-date:

- **Auto-Merge from Main**: Outdated PRs are automatically updated with the latest changes from main
- **Conflict Detection**: Automatic detection of merge conflicts with notifications
- **Copilot Integration**: Guidance for using GitHub Copilot to resolve conflicts
- **Team Notifications**: Authors and reviewers are notified of updates and conflicts

Learn more in the [PR Auto-Merge Guide](.github/PR_AUTO_MERGE_GUIDE.md).

## Links to Related Repositories

- [BlueMarble.Core](https://github.com/Nomoos/BlueMarble.Core) - Core game engine and systems
- [BlueMarble.Client](https://github.com/Nomoos/BlueMarble.Client) - Client application
- [BlueMarble.Server](https://github.com/Nomoos/BlueMarble.Server) - Server infrastructure
- [BlueMarble.Assets](https://github.com/Nomoos/BlueMarble.Assets) - Game assets and resources

## License

This project is proprietary and all rights are reserved. See the [LICENSE](LICENSE) file for details.
