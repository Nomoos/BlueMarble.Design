# BlueMarble.Design Usage Examples

This document demonstrates how to effectively use the BlueMarble.Design repository structure with practical examples.

## Scenario 1: Creating a Research Roadmap

### Step 1: Setup Research Issue Structure

1. Create main research roadmap issue using `/templates/research-roadmap-main-issue.md`
2. Title: "Research Roadmap: Technical Questions and Trade-offs for World Material Storage"
3. Fill in research areas from `research/RESEARCH_ISSUES_SUMMARY.md`

### Step 2: Create Individual Research Issues

1. For each research area, create sub-issue using `/templates/research-question-sub-issue.md`
2. Example: "[Research] Homogeneous Region Collapsing for Octree Optimization"
3. Link each sub-issue to the main roadmap issue as parent
4. Set priorities and effort estimates from research summary

### Step 3: Track Progress

1. Update main issue weekly with overall progress
2. Use sub-issues for detailed technical findings
3. Update `research/RESEARCH_ISSUES_SUMMARY.md` as work progresses

## Scenario 2: Adding a New Gameplay Feature

### Step 1: Requirements Gathering Phase

1. Create requirements gathering document using `/templates/requirements-gathering.md`
2. Identify and document all stakeholders (product, design, engineering, users)
3. Define requirement sources (user research, market analysis, technical constraints)
4. Example: `docs/gameplay/requirements-crafting-system.md`
5. Collect data through interviews, surveys, and competitive analysis
6. Document all functional and non-functional requirements

### Step 2: Research Phase

1. Check existing documentation in `/research/` for relevant studies
2. If needed, create new research document using `/templates/research-note.md` or `/templates/research-report.md`
3. Example: `research/topics/player-crafting-preferences.md`
4. Link research findings back to requirements document

### Step 3: Design Phase

1. Create feature specification using `/templates/feature-specification.md`
2. Place in appropriate category: `docs/gameplay/spec-crafting-system.md`
3. Link to related research, requirements gathering document, and dependencies
4. Ensure all requirements from gathering phase are addressed

### Step 4: Review and Integration

1. Update roadmap in `/roadmap/project-roadmap.md`
2. Cross-reference with related systems in other documentation categories
3. Add to appropriate tracking systems

## Scenario 3: World Building Development

### Step 1: Foundation

1. Review existing world documentation in `/docs/world/`
2. Create new lore document: `docs/world/lore-northern-kingdoms.md`
3. Use narrative design guidelines from category README

### Step 2: Integration

1. Update related gameplay documentation to reflect world elements
2. Consider UI/UX implications for presenting lore to players
3. Document any technical requirements in `/docs/systems/`

### Step 3: Asset Planning

1. Create asset requirements in `/assets/` directory
2. Link visual design needs to UI/UX documentation
3. Plan audio requirements in `/docs/audio/`

## Scenario 4: Technical System Design

### Step 1: Research Requirements

1. Review gameplay and world requirements that drive technical needs
2. Research technical approaches in `/research/`
3. Document findings: `research/topics/networking-solutions.md`

### Step 2: System Design

1. Create technical design document: `docs/systems/tdd-multiplayer-networking.md`
2. Specify API requirements: `docs/systems/api-player-sync.md`
3. Plan performance testing approach

### Step 3: Implementation Planning

1. Update project roadmap with technical milestones
2. Identify dependencies on other system components
3. Plan integration testing with gameplay systems

## Scenario 5: Cross-Functional Feature Development

### Example: Social Guild System

This feature touches multiple categories and demonstrates the interconnected nature of the documentation system.

#### Gameplay Design (`docs/gameplay/`)

- `spec-guild-system.md` - Core mechanics and player experience
- `balance-guild-progression.md` - Progression and reward balancing

#### World Integration (`docs/world/`)

- `lore-guild-organizations.md` - How guilds fit into world lore
- `narrative-guild-storylines.md` - Guild-specific quest content

#### Technical Implementation (`docs/systems/`)

- `tdd-guild-infrastructure.md` - Server architecture for guild data
- `api-guild-management.md` - API specifications for guild operations

#### User Interface (`docs/ui-ux/`)

- `ui-guild-management-interface.md` - Guild management UI design
- `ux-guild-discovery-flow.md` - Player journey for finding/joining guilds

#### Audio Design (`docs/audio/`)

- `audio-guild-feedback.md` - Audio cues for guild activities
- `music-guild-halls.md` - Music design for guild spaces

#### Supporting Research (`research/`)

- `research/market-research/mmorpg-monetization-2024.md` - Market analysis informing monetization design
- `research/topics/player-guild-behavior.md` - Player research informing design
- `research/topics/competitive-guild-systems.md` - Analysis of existing solutions

## Document Linking and Cross-References

### Internal Linking Patterns

```markdown
<!-- Link to related documents -->
See also: [Player Progression System](../gameplay/spec-player-progression-system.md)

<!-- Link to research that informed this design -->
Based on: [MMORPG Market Analysis 2024](../research/market-research/mmorpg-analysis-2024.md)

<!-- Link to technical requirements -->
Technical requirements: [Guild Infrastructure](../systems/tdd-guild-infrastructure.md)
```

### Dependency Tracking

Each document should clearly identify:

- **Design Dependencies**: Other design documents that must be completed first
- **Technical Dependencies**: Technical systems required for implementation
- **Content Dependencies**: Art, audio, or written content needed

## Version Control Best Practices

### Branching Strategy

- Create feature branches for major document additions: `feature/guild-system-design`
- Use descriptive commit messages: "Add initial guild system specification"
- Tag major milestone completions: `design-phase-1-complete`

### Review Process

1. Create pull request for new documents
2. Assign reviewers from relevant teams
3. Address feedback and iterate
4. Merge after approval from designated stakeholders
5. **Complete the issue** using the [Issue Completion Template](templates/issue-completion-template.md) to ensure
   proper documentation and communication with the issue creator

### Change Management

- Update revision history in documents for major changes
- Notify stakeholders of significant updates
- Maintain backward compatibility references for deprecated approaches

## Repository Maintenance

### Regular Activities

- **Weekly**: Update project roadmap with current progress
- **Monthly**: Review and update README files for accuracy
- **Quarterly**: Archive completed or deprecated documents
- **As needed**: Update templates based on usage feedback

### Issue Completion Workflow

When completing any issue, follow this process to ensure proper communication:

#### Example: Completing a Design Issue

1. **Complete the work** as specified in the issue
2. **Write completion comment** using the template from `templates/issue-completion-template.md`
3. **Tag the issue creator** (e.g., @Nomoos) in your comment
4. **Provide summary** with both changes list and reasoning

#### Sample Completion Comment

```markdown
## Issue Completion Summary

@Nomoos 

### Changes Made
- Created new combat mechanics specification in `docs/gameplay/spec-combat-mechanics-v2.md`
- Updated main gameplay README to reference new combat system
- Added combat balance considerations to economy systems document

### Reasoning & Context
- Chose action-point based system for more strategic gameplay
- Integrated with existing progression system to maintain consistency
- Balanced for both solo and group play scenarios

### Additional Notes
- Combat specification ready for technical review by development team
- May need playtesting to validate balance assumptions

Issue completed and ready for review.
```

#### Example: Completing a Research Issue

```markdown
## Issue Completion Summary

@ProjectManager 

### Changes Made
- Completed comprehensive market analysis in `research/market-research/mmorpg-monetization-2024.md`
- Added competitive pricing analysis section
- Updated research summary with key recommendations

### Reasoning & Context
- Focused on subscription-based models due to BlueMarble's target audience
- Analyzed both Eastern and Western market approaches for global perspective
- Recommendations align with established game design principles

### Additional Notes
- Research findings should inform upcoming business model discussions
- Consider quarterly updates to track market changes

Issue completed and ready for review.
```

### Quality Assurance

- Verify all internal links are functional
- Ensure consistent formatting across documents
- Check that all documents follow established naming conventions
- Validate that templates are being used appropriately

### Working with Pull Requests

#### Automated PR Updates

This repository has automated PR management to keep your work current:

**What Happens Automatically:**

- Your PR is checked regularly for updates needed from main
- If outdated, main is automatically merged into your PR branch
- You receive notifications about updates and any conflicts

**What You Need to Do:**

- Review automated merge commits in your PR
- Resolve conflicts if automatic merging fails
- Use GitHub Copilot for conflict resolution assistance
- Ensure tests pass after updates

**See the [PR Auto-Merge Guide](.github/PR_AUTO_MERGE_GUIDE.md) for detailed information**

#### Manual PR Management

When you need to manually update your PR:

```bash
# Fetch latest changes
git fetch origin

# Merge main into your branch
git checkout your-branch-name
git merge origin/main

# Push updates
git push
```

## Getting Started Checklist

For new contributors:

- [ ] Read the main [README.md](../README.md) and [CONTRIBUTING.md](../CONTRIBUTING.md)
- [ ] Review existing documents in your area of focus
- [ ] Identify appropriate templates for your contribution type
- [ ] Check the project roadmap for current priorities
- [ ] Reach out to team leads with any questions

For project leads:

- [ ] Ensure team members understand the repository structure
- [ ] Establish review processes for your domain
- [ ] Set up notifications for changes in your area

- [ ] Plan regular documentation maintenance activities

- [ ] Monitor usage patterns and suggest improvements

This structure provides a solid foundation for organizing and maintaining design documentation throughout the
BlueMarble project lifecycle.
