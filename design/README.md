# Design Directory

This directory contains **high-level game design vision and concepts** for BlueMarble, focusing on iterative design
development and foundational design direction for the medieval MMO simulation game.

> **Note**: This directory focuses on **design vision and concepts**. For detailed technical implementation specifications,
> see the [/docs](../docs/) directory. For research and exploratory work, see the [/research](../research/) directory.

## Purpose

The `/design` folder serves as the central location for:

- **Design Vision**: High-level game design direction and principles
- **Core Design Concepts**: Foundational mechanics and systems concepts
- **Iterative Design Process**: Version-controlled design evolution and refinement
- **Design Pillars**: Core values and design philosophy
- **Conceptual Documentation**: Early-stage design thinking and vision

## Folder Structure

```text
design/
├── README.md                    # This file - explains folder purpose and workflow
├── index.md                     # Design document index and navigation
├── pillars.md                   # Core design pillars and philosophy
├── mechanics.md                 # Core gameplay loops and systems
├── economy.md                   # Progression, currencies, resource management
├── auction-economy.md           # Auction-based trading, transport, fees, spoilage
├── narrative.md                 # Story, world-building, characters
├── styleguide.md                # Visual design system and brand
├── island_start_game_design.md  # Core game design for island start scope
├── one-page-design.md           # Concise design overview
├── OCTREE_DETAILS.md            # Octree spatial data system specification
├── expanse-rpg-the-whispering-signal.md  # Example tabletop RPG scenario
├── architecture/                # System architecture diagrams
├── ux/                          # User experience design
└── wireframes/                  # UI/UX wireframes and mockups
```

## Document Types

### Game Design Documents

- **File Pattern**: `[scope]_game_design.md`
- **Purpose**: Core design vision, mechanics, and technical implementation notes
- **Content**: One-page design format with gameplay loops, mechanics, technical architecture
- **Examples**: `island_start_game_design.md`, `one-page-design.md`

### System Design Documents

- **File Pattern**: `[system-name].md`
- **Purpose**: Detailed system design including mechanics, formulas, and implementation guidance
- **Content**: Comprehensive system specifications with formulas, examples, and integration points
- **Examples**: `auction-economy.md` (auction trading system with transport, fees, spoilage)

### Design Specifications

- **File Pattern**: `spec-[feature-name].md`
- **Purpose**: Conceptual feature design and high-level requirements
- **Content**: Design concepts, user flows, and high-level considerations
- **Note**: For detailed technical specifications, use `/docs` directory

### Tabletop RPG Scenarios

- **File Pattern**: `[rpg-system]-[scenario-name].md`
- **Purpose**: Document tabletop RPG scenarios as design research and reference material
- **Content**: Complete scenario with hooks, NPCs, locations, design analysis for BlueMarble adaptation
- **Examples**: `expanse-rpg-the-whispering-signal.md`
- **Template**: See `/templates/tabletop-rpg-scenario.md`

## Workflow and Update Process

### Adding New Design Documents

1. **Research Phase**: Review existing documentation and gather requirements
2. **Draft Creation**: Create initial design document using established format
3. **Review Process**: Collaborate with stakeholders for feedback and iteration
4. **Version Control**: Use Git commits to track design evolution and decisions
5. **Integration**: Link to related documents in `/docs`, `/templates`, and `/assets`

### Design Document Format

All design documents should include:

- **Header**: Title, version, author, date, status
- **Executive Summary**: Core concept and vision
- **Detailed Design**: Mechanics, systems, and implementation notes
- **Technical Considerations**: Architecture suggestions and constraints
- **Open Questions**: Unresolved design decisions requiring input

### Versioning Strategy

- Use Git commit history for tracking design iterations
- Include meaningful commit messages describing design changes
- Tag major design milestones for easy reference
- Maintain revision history within documents for significant changes

## Integration with Project Structure

### Relationship to Other Directories

- **`/docs`**: Technical implementation details and system specifications
- **`/templates`**: Document templates and formatting standards
- **`/assets`**: Visual design assets, mockups, and reference materials
- **`/roadmap`**: Project timeline and feature prioritization

### Cross-References

Design documents should reference:

- Related technical specifications in `/docs`
- Implementation templates from `/templates`
- Visual assets and mockups in `/assets`
- Timeline and priorities from `/roadmap`

## Quality Standards

### Content Requirements

- Clear, concise language appropriate for development teams
- Well-structured sections with logical flow
- Visual aids (diagrams, mockups) when helpful
- Technical accuracy and feasibility considerations

### Documentation Standards

- Follow project markdown formatting conventions
- Use consistent naming patterns and file organization
- Include proper internal linking to related documents
- Maintain up-to-date revision information

## Getting Started

### For Design Contributors

1. Review existing design documents in this folder
2. Check related documentation in `/docs` for technical context
3. Use appropriate templates from `/templates` directory
4. Follow established naming conventions and format patterns
5. Coordinate with development teams for technical feasibility

### For Developers

1. Reference design documents for feature implementation guidance
2. Provide feedback on design feasibility during review process
3. Update design documents when implementation deviates from initial design
4. Use design specifications to guide development priorities

## Related Documentation

- [Main Project README](../README.md) - Overall project structure and goals
- [Contributing Guidelines](../CONTRIBUTING.md) - Contribution process and standards
- [Documentation Standards](../docs/README.md) - Technical documentation guidelines
- [Templates](../templates/) - Document templates and formatting guides

---

This directory establishes a foundation for systematic design documentation and iterative development workflow,
ensuring design decisions are properly captured, versioned, and accessible to the development team.
