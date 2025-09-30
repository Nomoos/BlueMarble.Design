# BlueMarble Design Templates

This directory contains standardized templates for creating consistent documentation and issues across the BlueMarble project.

## Available Templates

### Small, Focused Templates (New)

These templates support the new repository structure with small, focused documents.

#### `research-note.md`

Template for small, focused research notes on specific topics (200-400 lines).

**Usage:** Individual research topics, focused investigations, concept analysis  
**Output Location:** `research/topics/`  
**Naming:** Use kebab-case: `topic-name.md`

#### `design-doc.md`

Template for focused design documents covering a single system or feature (200-400 lines).

**Usage:** System design, feature mechanics, design specifications  
**Output Location:** `design/`  
**Naming:** Use kebab-case: `system-name.md`

#### `experiment-report.md`

Template for structured experiment logs with hypothesis, method, results, and decisions.

**Usage:** Experiments, A/B tests, technical prototypes  
**Output Location:** `research/experiments/`  
**Naming:** Date-prefixed: `YYYY-MM-DD-experiment-name.md`

#### `playtest-report.md`

Template for documenting playtest sessions with setup, observations, and findings.

**Usage:** Playtest sessions, user testing, feedback collection  
**Output Location:** `research/experiments/` or project-specific folder  
**Naming:** Date-prefixed: `YYYY-MM-DD-playtest-name.md`

#### `decision-record.md`

ADR-style template for documenting important design decisions with context and rationale.

**Usage:** Architecture decisions, design trade-offs, significant choices  
**Output Location:** `design/` or `docs/systems/`  
**Naming:** Date-prefixed: `YYYY-MM-DD-decision-topic.md` or `decision-record-topic.md`

### Comprehensive Templates (Existing)

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

#### `core-systems-design-roadmap-issue.md`
Template for creating the parent core systems design roadmap issue that tracks BlueMarble's core systems
design across 13 major design areas covering architecture, gameplay, and foundational research.

**Usage:** Parent tracking issue for core systems design initiatives  
**GitHub Usage:** Main core systems design roadmap parent issue creation  
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

### Small, Focused Documents (New Structure)

For small, focused research and design work:

1. **Research Notes**: Use `research-note.md` for focused topics (200-400 lines)
   - Place in `research/topics/`
   - Keep notes atomic and focused on one question
   - Use kebab-case naming

2. **Design Documents**: Use `design-doc.md` for system/feature design (200-400 lines)
   - Place in `design/` or appropriate subdirectory
   - Focus on one system or feature
   - Include front matter metadata

3. **Experiments**: Use `experiment-report.md` for experiments
   - Place in `research/experiments/`
   - Use date-prefix: `YYYY-MM-DD-name.md`
   - Document hypothesis, method, results, decision

4. **Playtests**: Use `playtest-report.md` for playtest sessions
   - Place in `research/experiments/` or project folder
   - Use date-prefix: `YYYY-MM-DD-name.md`
   - Capture observations, pain points, delights

5. **Decisions**: Use `decision-record.md` for important decisions
   - Place in `design/` or `docs/systems/`
   - Use ADR format: context, options, decision, consequences
   - Document trade-offs and rationale

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
| Research Note | `research-note.md` | `research/topics/` |
| Design Doc | `design-doc.md` | `design/` |
| Experiment | `experiment-report.md` | `research/experiments/` |
| Playtest | `playtest-report.md` | `research/experiments/` |
| Decision Record | `decision-record.md` | `design/`, `docs/systems/` |
| Game Design | `game-design-document.md` | `docs/core/`, `docs/gameplay/` |
| Feature Spec | `feature-specification.md` | `docs/gameplay/`, `docs/systems/`, `docs/ui-ux/` |
| Research Report | `research-report.md` | `docs/research/` |
| Main Research Issue | `research-roadmap-main-issue.md` | GitHub Issues |
| Infrastructure Roadmap | `infrastructure-research-roadmap-issue.md` | GitHub Issues |
| Core Systems Roadmap | `core-systems-design-roadmap-issue.md` | GitHub Issues |
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