# BlueMarble Core Systems Design Roadmap Usage

This document explains how to use the BlueMarble Core Systems Design Roadmap parent issue for project
management and traceability.

## Purpose

The Core Systems Design Roadmap serves as a **parent issue** that:

- Tracks progress across all 13 major core systems design areas
- Links related sub-issues for traceability and project management
- Provides a central coordination point for design work
- Enables milestone tracking for the Core Systems Design project
- Facilitates stakeholder communication and progress reporting

## Creating the Parent Issue

### Using the Template

1. Navigate to GitHub Issues in the BlueMarble.Design repository
2. Create a new issue with title: **"BlueMarble Core Systems Design Roadmap"**
3. Copy content from `templates/core-systems-design-roadmap-issue.md`
4. Set the following issue metadata:
   - **Labels**: `design`, `core-systems`, `epic`, `parent`, `high-priority`
   - **Milestone**: `Core Systems Design`
   - **Project**: `BlueMarble Project Roadmap`
   - **Assignees**: Design team leads

### Initial Configuration

Replace placeholder values in the template:
- `#[PARENT-ISSUE-NUMBER]` → Actual parent issue number (e.g., `#75`)
- Update timeline dates based on actual project schedule
- Adjust phase durations if needed based on team capacity

## Linking Sub-Issues

### Creating Sub-Issues

For each of the 13 design areas:

1. Use `templates/research-question-sub-issue.md` as template
2. Create GitHub issue with title: `[Design] [Specific Design Area Title]`
3. Set **Parent Issue** field to reference the main roadmap issue number
4. Set priority based on the design area classification:
   - High Priority: Issues 1-5
   - Medium Priority: Issues 6-10
   - Low Priority: Issues 11-13

### Design Areas to Create Sub-Issues For

#### High Priority (Critical for Core Functionality)
- **Issue #1**: Game Architecture Specification
- **Issue #2**: Core Gameplay Mechanics Design
- **Issue #3**: Player Progression Framework
- **Issue #4**: Database Schema Design
- **Issue #5**: API Specifications and Protocols

#### Medium Priority (Feature Enhancements)
- **Issue #6**: Combat System Design
- **Issue #7**: Character Progression System
- **Issue #8**: Social and Guild Systems
- **Issue #9**: Economic System Design
- **Issue #10**: Security Framework Design

#### Low Priority (Future Enhancements)
- **Issue #11**: PvP Systems and Balance
- **Issue #12**: End-game Content Design
- **Issue #13**: Cross-platform Compatibility Design

### Updating Parent Issue References

As you create sub-issues:

1. Update the parent issue checkboxes with actual issue numbers
2. Replace `#[Issue-X]` placeholders with real issue numbers (e.g., `#[Issue-1]` → `#76`)
3. Ensure each sub-issue references the parent issue number
4. Use GitHub's linking syntax to create automatic relationships

## Project Management Integration

### GitHub Projects

Add the parent issue and all sub-issues to the "BlueMarble Project Roadmap" project:

1. Add parent issue to project board
2. Place in "Core Systems Design" column/swim lane
3. Add all sub-issues as they are created
4. Use project views to track overall progress

### Milestone Tracking

1. Create or use existing "Core Systems Design" milestone
2. Add parent issue and all sub-issues to this milestone
3. Set milestone due date to Q4 2025 (adjust as needed)
4. Track completion percentage in milestone view

### Labels and Organization

Use consistent labels across all related issues:
- `core-systems` - All issues in this roadmap
- `design` - Design work
- `high-priority`, `medium-priority`, `low-priority` - Priority levels
- `phase-1`, `phase-2`, `phase-3`, `phase-4` - Implementation phases
- `blocked` - Issues waiting on dependencies

## Progress Tracking

### Weekly Updates

Post weekly status updates in parent issue comments:
```markdown
## Week of [Date]

### Completed
- [x] #[Issue-X] - [Brief description]

### In Progress
- [ ] #[Issue-Y] - [Current status and % complete]

### Blocked
- [ ] #[Issue-Z] - [Blocker description and resolution plan]

### Next Week
- Plan to start #[Issue-A]
- Complete #[Issue-Y]
```

### Design Reviews

Schedule and document design reviews:
1. Create calendar events for major design reviews
2. Document review outcomes in issue comments
3. Update issue status based on review feedback
4. Link to review recordings or notes

### Status Dashboard

Use GitHub's built-in features to create status views:
- Filter by milestone to see all Core Systems Design issues
- Use project boards for kanban-style tracking
- Generate burndown charts using milestone progress
- Track velocity using completed issues per sprint

## Success Metrics

Track the following metrics weekly:

### Completion Metrics
- Number of completed design areas
- Percentage of milestone complete
- Number of approved designs vs. in-review

### Quality Metrics
- Number of design revisions required
- Stakeholder approval rate
- Cross-functional review participation

### Timeline Metrics
- Actual vs. planned completion dates
- Average time per design area
- Critical path status

## Related Documentation

- **Template**: `templates/core-systems-design-roadmap-issue.md`
- **Sub-Issue Template**: `templates/research-question-sub-issue.md`
- **Template Guide**: `templates/research-issue-templates-guide.md`
- **Project Roadmap**: `roadmap-guides/project-roadmap.md`
- **Game Design Document**: `docs/gameplay/gdd-core-game-design.md`

## Example Workflow

### Scenario: Starting Game Architecture Design

1. **Create sub-issue** from template for "Game Architecture Specification"
2. **Set metadata**: 
   - Title: `[Design] Game Architecture Specification`
   - Parent: #75 (Core Systems Design Roadmap)
   - Priority: High
   - Effort: 4-5 weeks
3. **Fill in template sections**:
   - Design question and objectives
   - Technical scope and dependencies
   - Expected deliverables
4. **Link to parent**: Add `Parent Issue: #75` in sub-issue
5. **Update parent**: Change `#[Issue-1]` to actual issue number in parent
6. **Begin work**: Assign team members and start design work
7. **Weekly updates**: Post progress in sub-issue comments
8. **Review and approval**: Schedule design review, document outcome
9. **Mark complete**: Check off item in parent issue when approved

## Best Practices

### Communication
- Post major updates in parent issue for visibility
- Use sub-issue comments for detailed discussions
- Tag stakeholders when input is needed
- Link related PRs and documentation

### Documentation
- Keep design documents in `docs/` directory
- Link to documents from issues
- Update documents based on feedback
- Maintain document version history

### Dependency Management
- Clearly identify dependencies in sub-issues
- Update parent issue if critical path changes
- Communicate blockers immediately
- Adjust timeline if dependencies slip

### Team Coordination
- Hold weekly sync meetings
- Review parent issue together
- Identify and resolve conflicts early
- Celebrate milestones and completions

## Troubleshooting

### Common Issues

**Problem**: Sub-issue numbering doesn't match planned sequence  
**Solution**: Issue numbers are auto-assigned by GitHub. Update parent issue with actual numbers.

**Problem**: Dependencies blocking progress  
**Solution**: Update blocked issue with `blocked` label, communicate in parent issue, adjust timeline.

**Problem**: Design revisions causing delays  
**Solution**: Update effort estimates, communicate timeline impact, adjust downstream schedules.

**Problem**: Stakeholder feedback conflicts  
**Solution**: Schedule alignment meeting, document decision rationale, update designs accordingly.

## Support

For questions about Core Systems Design Roadmap usage:
1. Review this documentation
2. Check existing examples in GitHub issues
3. Consult with design team leads
4. Reach out to project managers for process questions
