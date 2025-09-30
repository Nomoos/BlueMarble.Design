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
├── design/                  # Game design documentation and vision
├── docs/                    # Main documentation directory
│   ├── core/               # Core design documents (GDD, TDD)
│   ├── gameplay/           # Gameplay mechanics and systems
│   ├── world/              # World building, lore, and narrative
│   ├── systems/            # Core systems and technical design
│   ├── ui-ux/              # User interface and experience design
│   ├── audio/              # Audio design and music concepts
│   └── research/           # Market research and analysis
├── templates/              # Document templates for consistency
├── assets/                 # Design assets, mockups, and references
├── roadmap/                # Project roadmap and feature tracking
└── archive/                # Archived or deprecated documents
```

## Getting Started

### For Design Contributors

1. Review the [Contributing Guidelines](CONTRIBUTING.md) and [Documentation Best Practices](DOCUMENTATION_BEST_PRACTICES.md)
2. Check existing documentation in the relevant category
3. Use the appropriate template from the `templates/` directory
4. Follow the established naming conventions and structure
5. When completing issues, use the [Issue Completion Template](templates/issue-completion-template.md) to ensure proper documentation and communication

### For Research Contributors

1. Use the [Research Issue Templates](templates/research-issue-templates-guide.md) to create structured research roadmaps
2. Create main research issues using the [Research Roadmap Template](templates/research-roadmap-main-issue.md)
3. Create individual research questions using the [Research Sub-Issue Template](templates/research-question-sub-issue.md)
4. Follow the research documentation guidelines in `research/RESEARCH_ISSUES_SUMMARY.md`

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

### Core Design Documents
- **[Game Design Document](./docs/core/game-design-document.md)** - Complete game vision and design
- **[Technical Design Document](./docs/core/technical-design-document.md)** - Technical architecture and requirements
- **[Documentation Index](./docs/README.md)** - Complete documentation structure

### System Specifications
- **[Gameplay Systems](./docs/systems/gameplay-systems.md)** - Combat, progression, and core mechanics
- **[Economy Systems](./docs/systems/economy-systems.md)** - Trading, crafting, and economic balance
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

1. **Research Phase**: Gather requirements and conduct necessary research
2. **Design Phase**: Create structured design documents using templates
3. **Review Phase**: Collaborate with stakeholders for feedback and iteration
4. **Approval Phase**: Finalize documents for development implementation
5. **Maintenance Phase**: Keep documents updated as implementation progresses

### Quality Checks

Before committing documentation changes, run the quality check script:

```bash
./scripts/check-documentation-quality.sh
```

This script validates:

- Required files and directory structure
- Markdown formatting and linting
- Duplicate content detection
- Broken internal links

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
