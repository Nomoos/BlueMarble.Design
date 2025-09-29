# BlueMarble Design Templates

This directory contains standardized templates for creating consistent documentation and issues across the BlueMarble project.

## Available Templates

### Design Documentation Templates

#### `game-design-document.md`
Template for high-level game design documents that establish vision, core mechanics, and design principles.

**Usage:** Game design specifications, vision documents, core design principles  
**Output Location:** `docs/core/` or `docs/gameplay/`

#### `feature-specification.md`
Template for detailed feature specifications including requirements, user stories, and acceptance criteria.

**Usage:** Individual feature design, system specifications, implementation requirements  
**Output Location:** `docs/gameplay/`, `docs/systems/`, `docs/ui-ux/`

#### `research-report.md`
Template for comprehensive research reports including methodology, findings, and recommendations.

**Usage:** Market research, technical analysis, player studies, competitive analysis  
**Output Location:** `docs/research/`

### Issue Management Templates

#### `research-roadmap-main-issue.md`
Template for creating the main research roadmap issue that tracks overall progress across multiple research areas.

**Usage:** Primary tracking issue for research initiatives  
**GitHub Usage:** Main research roadmap issue creation  
**Related:** Works with research-question-sub-issue.md

#### `infrastructure-research-roadmap-issue.md`
Template for creating the parent infrastructure & research roadmap issue that tracks BlueMarble's core infrastructure research across 12 major research areas.

**Usage:** Parent tracking issue for infrastructure research initiatives  
**GitHub Usage:** Main infrastructure roadmap parent issue creation  
**Related:** Works with research-question-sub-issue.md for sub-issues

#### `research-question-sub-issue.md`
Template for individual research area issues that provide detailed tracking of specific research questions.

**Usage:** Individual research area tracking  
**GitHub Usage:** Sub-issues linked to main research roadmap  
**Related:** Child issues of research-roadmap-main-issue.md

#### `issue-completion-template.md`
Template for standardized issue completion comments ensuring consistent communication and documentation.

**Usage:** Issue closure, progress reporting, completion documentation  
**GitHub Usage:** Final comments when closing issues

## Template Usage Guide

### Research Issue Templates

For research-related work, use the research issue template system:

1. **Create Main Issue**: Use `research-roadmap-main-issue.md` for overall tracking
2. **Create Sub-Issues**: Use `research-question-sub-issue.md` for individual research areas
3. **Complete Issues**: Use `issue-completion-template.md` for closure

**Detailed Guide:** See `research-issue-templates-guide.md` for complete instructions

### Design Documentation Templates

1. **Choose Appropriate Template**: Select based on document type and scope
2. **Follow Naming Conventions**: Use established file naming patterns
3. **Place in Correct Location**: Follow repository structure guidelines
4. **Cross-Reference**: Link related documents and issues

## Template Customization

### When to Customize
- Project-specific requirements not covered by standard template
- Team workflow differences requiring template adjustments
- New template types needed for emerging documentation needs

### How to Customize
1. Copy existing template closest to your needs
2. Modify sections to match your requirements
3. Update this README to document new template
4. Notify team of template changes and usage

### Template Maintenance
- Review templates quarterly for relevance and completeness
- Update based on team feedback and usage patterns
- Version control template changes for team awareness
- Keep related documentation synchronized

## Quick Reference

| Document Type | Template | Output Location |
|---------------|----------|-----------------|
| Game Design | `game-design-document.md` | `docs/core/`, `docs/gameplay/` |
| Feature Spec | `feature-specification.md` | `docs/gameplay/`, `docs/systems/`, `docs/ui-ux/` |
| Research Report | `research-report.md` | `docs/research/` |
| Main Research Issue | `research-roadmap-main-issue.md` | GitHub Issues |
| Infrastructure Roadmap | `infrastructure-research-roadmap-issue.md` | GitHub Issues |
| Research Sub-Issue | `research-question-sub-issue.md` | GitHub Issues |
| Issue Completion | `issue-completion-template.md` | GitHub Issue Comments |

## Related Documentation

- **Repository Structure**: `../README.md` - Overall repository organization
- **Usage Examples**: `../USAGE_EXAMPLES.md` - Practical application examples  
- **Contributing Guidelines**: `../CONTRIBUTING.md` - Contribution standards and processes
- **Research Issue Guide**: `research-issue-templates-guide.md` - Detailed research template usage

## Support

For questions about template usage:
1. Check the related documentation links above
2. Review existing examples in the `docs/` directory
3. Consult the research issue templates guide for research-specific questions
4. Reach out to project maintainers for template enhancement requests