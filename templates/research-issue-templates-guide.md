# Research Issue Templates Usage Guide

This guide explains how to use the research issue templates to create a structured research roadmap with main and sub-issues.

## Overview

The research issue system consists of:
- **Main Research Roadmap Issue**: Overall tracking and coordination
- **Research Question Sub-Issues**: Individual research areas with detailed progress

## Template Files

- `templates/research-roadmap-main-issue.md` - Main roadmap issue template
- `templates/infrastructure-research-roadmap-issue.md` - Infrastructure & research roadmap parent issue template
- `templates/core-systems-design-roadmap-issue.md` - Core systems design roadmap parent issue template
- `templates/research-question-sub-issue.md` - Individual research question template

## Creating the Research Roadmap

### Option A: Standard Research Roadmap

#### Step 1: Create the Main Research Issue

1. Copy the content from `templates/research-roadmap-main-issue.md`
2. Create a new GitHub issue with title: "Research Roadmap: Technical Questions and Trade-offs for World Material Storage"
3. Replace placeholder values:
   - `#[Issue-X]` - Replace with actual issue numbers as sub-issues are created
   - Update status and progress checkboxes as work progresses

### Option B: Infrastructure & Research Roadmap (Parent Issue)

#### Step 1: Create the Infrastructure Research Roadmap Parent Issue

1. Use the GitHub issue template: "Infrastructure Research Roadmap (Parent Issue)" 
2. Or copy content from `templates/infrastructure-research-roadmap-issue.md`
3. Create a new GitHub issue with title: "BlueMarble Infrastructure & Research Roadmap"
4. Set labels: `infrastructure`, `research`, `epic`, `parent`
5. Set milestone: `Infrastructure` and project: `Infrastructure`

### Option C: Core Systems Design Roadmap (Parent Issue)

#### Step 1: Create the Core Systems Design Roadmap Parent Issue

1. Use the GitHub issue template: "Core Systems Design Roadmap (Parent Issue)"
2. Or copy content from `templates/core-systems-design-roadmap-issue.md`
3. Create a new GitHub issue with title: "BlueMarble Core Systems Design Roadmap"
4. Set labels: `design`, `core-systems`, `epic`, `parent`, `high-priority`
5. Set milestone: `Core Systems Design` and project: `BlueMarble Project Roadmap`

### Step 2: Create Sub-Issues for Each Research Area

For each of the 12 research areas, create a sub-issue:

1. Copy content from `templates/research-question-sub-issue.md`
2. Create GitHub issue with title: `[Research] [Specific Research Area Title]`
3. Fill in the template fields:
   - **Parent Issue**: Reference the main roadmap issue number
   - **Research Area Title**: Use titles from RESEARCH_ISSUES_SUMMARY.md
   - **Priority**: High/Medium/Low based on the research summary
   - **Effort Estimate**: Use estimates from the research summary

### Step 3: Link Issues Together

1. Update main issue with actual sub-issue numbers
2. Ensure sub-issues reference the main issue as parent
3. Add dependency relationships between sub-issues
4. Update project board or milestone tracking

## Research Areas to Create Sub-Issues For

### High Priority
1. **Homogeneous Region Collapsing for Octree Optimization** (3-4 weeks)
2. **Hybrid Compression Strategies for Petabyte-Scale Storage** (6-8 weeks)
3. **Multi-Layer Query Optimization for Read-Dominant Workloads** (5-6 weeks)
4. **Database Architecture for Petabyte-Scale 3D Octree Storage** (12-16 weeks)
5. **3D Octree Storage Architecture Integration** (10-14 weeks)

### Medium Priority
6. **Delta Overlay System for Fine-Grained Octree Updates** (4-5 weeks)
7. **Octree + Grid Hybrid Architecture for Multi-Scale Storage** (8-10 weeks)
8. **Octree + Vector Boundary Integration for Precise Features** (6-7 weeks)
9. **Grid + Vector Combination for Dense Simulation Areas** (8-10 weeks)
10. **Multi-Resolution Blending for Scale-Dependent Geological Processes** (14-18 weeks)

### Low Priority
11. **Distributed Octree Architecture with Spatial Hash Distribution** (10-12 weeks)

**Note**: Issue #1 (Material Inheritance) is already completed and documented.

## Template Customization

### Main Issue Template Customization
- Update research areas list based on current project needs
- Modify success metrics to match project goals
- Adjust implementation phases based on team capacity
- Update related documentation links

### Sub-Issue Template Customization
- Add project-specific technical requirements
- Modify deliverables section for your documentation structure
- Adjust communication plan based on team processes
- Update risk assessment categories

## Workflow Integration

### Progress Tracking
1. **Weekly Updates**: Brief status in main issue comments
2. **Detailed Progress**: Technical findings in sub-issue comments
3. **Cross-Issue Communication**: Reference related issues using #issue-number
4. **Milestone Tracking**: Use GitHub milestones to track phases

### Documentation Integration
- Research reports go in `research/topics/` or relevant research subdirectory
- Technical specs go in `docs/systems/`
- Implementation guides go in appropriate research area (e.g., `research/spatial-data-storage/`)
- Update relevant research index files as work progresses

### Completion Process
1. Complete research work in sub-issue
2. Update deliverables and mark completion criteria
3. Use [Issue Completion Template](issue-completion-template.md) for final comment
4. Update main issue progress checkboxes
5. Link to final documentation and next steps

## Best Practices

### Issue Management
- Use clear, descriptive titles following the naming pattern
- Keep parent-child relationships clearly documented
- Regular status updates prevent issues from going stale
- Use labels to categorize by research type, priority, and status

### Documentation
- Always link to relevant existing documentation
- Create research reports using the research-report.md template
- Keep technical specifications updated as research progresses
- Cross-reference between issues and documentation

### Communication
- Tag relevant team members in progress updates
- Use issue comments for technical discussions
- Reference decisions made in other issues or meetings
- Keep stakeholders informed through regular status updates

## Troubleshooting

### Common Issues
- **Missing Dependencies**: Ensure prerequisite research is identified and tracked
- **Scope Creep**: Keep research focused on the specific questions defined
- **Documentation Drift**: Regularly update related documentation as research progresses
- **Communication Gaps**: Use the communication plan to maintain stakeholder alignment

### Template Updates
- Templates can be modified based on project experience
- Document template changes and notify team members
- Version control template updates for consistency
- Collect feedback from research teams to improve templates

---

This guide should be updated as the research issue process evolves and team feedback is incorporated.