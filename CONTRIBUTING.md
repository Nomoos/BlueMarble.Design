# Contributing to BlueMarble.Design

Welcome to the BlueMarble.Design repository! This guide will help you contribute effectively to our design documentation and research efforts.

## Getting Started

1. **Familiarize yourself** with the repository structure and existing documentation
2. **Review the [Documentation Best Practices](DOCUMENTATION_BEST_PRACTICES.md)** for quality guidelines
3. **Choose a category** that aligns with your contribution (gameplay, world, systems, ui-ux, audio, research)
4. **Use the appropriate template** from the `templates/` directory
5. **Follow naming conventions** and maintain consistency with existing documents

## Document Guidelines

For detailed guidelines on creating quality documentation, see [Documentation Best Practices](DOCUMENTATION_BEST_PRACTICES.md).

### File Naming Conventions

**Standard Naming**:

- Use **kebab-case**: lowercase with hyphens: `player-progression-system.md`, `enemy-ai-overview.md`
- Be descriptive and specific: `pathfinding-comparison-a-star-vs-navmesh.md` not `pathfinding.md`
- Include version numbers for major revisions: `combat-mechanics-v2.md`

**Prefixes for Document Types**:

- `gdd-` for Game Design Documents
- `tdd-` for Technical Design Documents
- `spec-` for Feature Specifications
- `research-` for Research Reports (optional, prefer descriptive names)

**Date Prefixes for Time-Sensitive Documents**:

- Experiments: `YYYY-MM-DD-short-title.md` (e.g., `2025-09-30-combat-playtest.md`)
- Playtests: `YYYY-MM-DD-short-title.md` (e.g., `2025-10-15-player-retention-test.md`)
- Decision records: `decision-record-YYYY-MM-DD-topic.md` or `YYYY-MM-DD-topic-adr.md`

### File Size Guidelines

Keep files **small and focused** to improve readability, collaboration, and reduce merge conflicts:

**Target Size**:

- **200-400 lines** of Markdown (~500-800 words)
- Long enough to hold one clear idea, short enough to scan quickly

**When to Split Files**:

- File covers more than one research question → split into multiple files
- Scrolling feels longer than 2-3 screens → consider splitting
- Multiple unrelated sections (like "Section 2.3.1...") → break into separate files

**Good Examples** (small, focused files):

- ✅ `procedural-tree-generation.md` - One specific technique
- ✅ `player-motivation-self-determination.md` - One concept
- ✅ `pathfinding-comparison-a-star-vs-navmesh.md` - One comparison

**Too Large** (should be split):

- ❌ `ai-research.md` with 20 subsections
- ❌ `combat-systems.md` mixing mechanics, UX, psychology, playtests
- ❌ `game-design.md` covering everything

**When Files Can Be Larger**:

- Comprehensive technical specifications (API docs, system architecture)
- Master index files (linking to other focused documents)
- Templates and guides

### Document Structure

All documents should include:

1. **Front matter** (for research and design documents):

   ```markdown
   ---
   title: Document Title
   date: YYYY-MM-DD
   owner: @username
   status: draft | in-progress | complete | approved
   tags: [tag1, tag2, tag3]
   ---
   ```

2. **Executive summary** or overview (2-3 sentences)
3. **Detailed content** organized with clear headings
4. **References and dependencies** to other documents
5. **Revision history** for major changes (use Git for detailed history)

### Writing Style

- Write clearly and concisely
- Use bullet points and numbered lists for readability
- Include diagrams, mockups, or visual aids when helpful
- Define technical terms and acronyms
- Maintain consistency with established terminology
- **Cross-link liberally**: Link to related research, design docs, and issues
- Keep each document focused on **one topic** or **one concern**

### Templates

Use the appropriate template for your document type:

- [research-note.md](templates/research-note.md) - Small research notes
- [design-doc.md](templates/design-doc.md) - Design documents
- [experiment-report.md](templates/experiment-report.md) - Experiment logs
- [playtest-report.md](templates/playtest-report.md) - Playtest reports
- [decision-record.md](templates/decision-record.md) - ADR-style decisions

See the [templates directory](templates/) for all available templates.

## Review Process

1. **Create a pull request** with your new or updated documentation
2. **Assign reviewers** from relevant teams (design, development, product)
3. **Address feedback** and iterate on the document
4. **Obtain approval** from designated stakeholders
5. **Merge** after all requirements are met

### Automated PR Updates

To keep pull requests current with the main branch, we have automated PR management:

- **Automatic Updates**: Your PR will be automatically updated when main branch changes
- **Conflict Notifications**: You'll be notified if manual conflict resolution is needed
- **Copilot Assistance**: Use GitHub Copilot to help resolve merge conflicts
- **See Details**: Check the [PR Auto-Merge Guide](.github/PR_AUTO_MERGE_GUIDE.md) for more information

## Issue Completion Policy

When completing any issue, team members should follow this process to ensure proper documentation and communication:

### Required Steps for Issue Completion

1. **Write a completion comment** on the issue before closing it
2. **Tag the issue creator** (using @username) in your completion comment
3. **Provide a brief summary** that includes both:
   - **Changes List**: Clear, quick overview of what was modified/added/completed
   - **Reasoning**: Context explaining why changes were made and any key decisions

### Issue Completion Comment Template

Use this template for consistency when completing issues:

```markdown
## Issue Completion Summary

@[issue-creator-username] 

### Changes Made
- [List specific changes, additions, or completions]
- [Use bullet points for clear overview]
- [Include file names, sections, or features affected]

### Reasoning & Context
- [Explain the approach taken and why]
- [Note any important decisions or considerations]
- [Reference related issues, discussions, or dependencies]

### Additional Notes
- [Any follow-up items or related work needed]
- [Links to related documentation or resources]

Issue completed and ready for review.
```

### Example Completion Comment

```markdown
## Issue Completion Summary

@Nomoos 

### Changes Made
- Updated CONTRIBUTING.md to include new issue completion policy
- Added template for standardized completion comments
- Created guidelines for tagging creators and providing summaries

### Reasoning & Context
- Established this policy to improve team communication and documentation
- Template ensures consistency across all team members
- Combines quick overview (changes list) with context (reasoning) as requested

### Additional Notes
- Policy applies to all issue types (design, research, etc.)
- Consider adding this to onboarding documentation for new team members

Issue completed and ready for review.
```

## Categories and Ownership

### Gameplay
- Combat mechanics
- Player progression
- Game modes and activities
- Balancing and economy

### World
- Lore and narrative
- Character design
- Environmental storytelling
- Quest and mission design

### Systems
- Technical architecture
- Database design
- API specifications
- Performance requirements

### UI/UX
- Interface design
- User experience flows
- Accessibility guidelines
- Platform-specific considerations

### Audio
- Sound design principles
- Music direction
- Voice acting guidelines
- Audio implementation specs

### Research
- Market analysis
- Player behavior studies
- Competitive analysis
- Technical research

## Tools and Resources

- **Markdown**: Primary format for documentation
- **Mermaid**: For diagrams and flowcharts
- **Figma/Sketch**: For UI/UX mockups (link to assets)
- **GitHub Issues**: For tracking design tasks and decisions

## Quality Standards

- Ensure all links and references are valid
- Proofread for grammar and spelling
- Verify technical accuracy with relevant teams
- Include appropriate metadata and tags
- Update related documents when necessary

## Getting Help

- Join our design discussions in the project channels
- Review existing documentation for examples and patterns
- Ask questions in pull request comments
- Consult with team leads for guidance on complex decisions

Thank you for contributing to BlueMarble.Design!