# Research Assignment Template for Phase 2+ (Discovered Sources)

---
title: Research Assignment Template for Newly Discovered Sources
date: 2025-01-15
tags: [research-queue, assignment, template, source-discovery]
status: template
---

**Document Type:** Assignment Template  
**Version:** 1.0  
**Purpose:** Template for organizing newly discovered research sources into balanced assignment groups

## Overview

This template is used to create new assignment group files when sources are discovered during research. It ensures discovered sources are tracked with priority and distributed for future research phases.

## Usage Instructions

### When to Use This Template

Use this template after completing a research phase when:
1. Multiple new sources have been discovered across assignment groups
2. Discovered sources need to be organized for next research phase
3. New balanced assignment groups need to be created

### Steps to Create New Assignment Groups

1. **Collect Discoveries**
   - Review all 4 assignment group files
   - Extract sources from "New Sources Discovery" sections
   - Compile into single list with all metadata

2. **Validate and Prioritize**
   - Review each discovered source
   - Validate priority assessments
   - Add missing information
   - Remove duplicates

3. **Balance Distribution**
   - Count sources by priority (Critical/High/Medium/Low)
   - Calculate total estimated effort
   - Determine number of groups needed (typically 4 for parallel work)
   - Distribute using round-robin by priority

4. **Create New Assignment Files**
   - Copy this template for each new group
   - Name files: `research-assignment-phase-2-group-[N].md`
   - Fill in discovered sources
   - Update overview document

5. **Update Master Queue**
   - Add all discovered sources to master research queue
   - Update total source count
   - Mark as pending research

---

## Template Structure for Phase 2+ Assignment Groups

```markdown
# Research Assignment Phase [N] - Group [N]

---
title: Research Assignment Phase [N] Group [N]
date: [YYYY-MM-DD]
tags: [research-queue, assignment, parallel-work, phase-[N]]
status: pending
assignee: TBD
---

**Document Type:** Research Assignment  
**Version:** 1.0  
**Phase:** [N]  
**Total Topics:** [X]  
**Priority Mix:** [X] Critical, [X] High, [X] Medium, [X] Low  
**Status:** Ready for Assignment

## Overview

This assignment group contains discovered sources from Phase [N-1] research. Topics have been validated and balanced for parallel execution.

## Assignment Summary

- **Critical Priority:** [X] topics
- **High Priority:** [X] topics
- **Medium Priority:** [X] topics
- **Low Priority:** [X] topics

**Estimated Total Effort:** [X-Y] hours  
**Target Completion:** [N] weeks

---

## Topics

### 1. [Source Name] ([PRIORITY])

**Priority:** [Critical/High/Medium/Low]  
**Category:** [Category]  
**Source:** Discovered from [Original Topic]  
**Estimated Effort:** [X-Y] hours  
**Document Target:** [X-Y] lines

**Focus Areas:**
- [Focus area 1]
- [Focus area 2]
- [Focus area 3]

**Deliverables:**
- Analysis document: `[filename].md`
- [Specific deliverable 1]
- [Specific deliverable 2]

**Discovery Rationale:**
[Why this source was discovered and why it's relevant]

**Why [Priority]:**
[Justification for priority level]

---

[Repeat for each topic in group]

---

## Work Guidelines

### Research Process

1. **Source Review** (30% of time)
   - Read/review source material thoroughly
   - Take structured notes
   - Identify key concepts relevant to BlueMarble

2. **Analysis** (40% of time)
   - Compare with existing BlueMarble systems
   - Identify integration opportunities
   - Evaluate technical feasibility
   - Consider scalability implications

3. **Documentation** (30% of time)
   - Write comprehensive analysis document
   - Include code examples where appropriate
   - Add cross-references to related research
   - Provide clear recommendations

### Document Structure

Each analysis document should include:

1. **Executive Summary** - Key findings and recommendations
2. **Source Overview** - What was analyzed
3. **Core Concepts** - Main ideas and patterns
4. **BlueMarble Application** - How to apply to project
5. **Implementation Recommendations** - Specific action items
6. **References** - Citations and further reading

### Quality Standards

- **Minimum Length:** As specified per topic (varies by priority)
- **Code Examples:** Include where relevant
- **Citations:** Proper attribution of sources
- **Cross-References:** Link to related research documents
- **Front Matter:** Include YAML front matter with metadata

---

## Progress Tracking

Track progress using this checklist:

- [ ] [Topic 1] ([Priority])
- [ ] [Topic 2] ([Priority])
- [ ] [Topic 3] ([Priority])
[Continue for all topics]

---

## New Sources Discovery

During research, you may discover additional sources referenced in materials you're analyzing. Track them here for future research phases.

### Discovery Template

For each newly discovered source, add an entry:

```markdown
**Source Name:** [Title of discovered source]
**Discovered From:** [Which topic led to this discovery]
**Priority:** [Critical/High/Medium/Low - your assessment]
**Category:** [Category type]
**Rationale:** [Why this source is relevant to BlueMarble]
**Estimated Effort:** [Hours needed for analysis]
```

### Discovered Sources Log

Add discovered sources below this line:

---

<!-- Example entries go here -->

---

## Submission Guidelines

1. Create documents in `research/literature/` directory
2. Use kebab-case naming convention
3. Include proper YAML front matter
4. Update master research queue upon completion
5. Cross-link with related documents
6. Log any newly discovered sources

---

## Support and Questions

- Review existing completed documents for format examples
- Reference `research/literature/README.md` for guidelines
- Check `research/literature/example-topic.md` for template
- Consult master research queue for context

---

**Created:** [Date]  
**Last Updated:** [Date]  
**Status:** Ready for Assignment  
**Next Action:** Assign to team member
```

---

## Discovery Statistics Template

After collecting discovered sources, create a summary:

```markdown
## Phase [N-1] Discovery Summary

**Research Period:** [Start Date] - [End Date]  
**Groups Analyzed:** 4  
**Primary Topics Completed:** [X]  
**New Sources Discovered:** [Y]

### Discovery Breakdown by Group

**Group 1:**
- Sources discovered: [X]
- Highest priority: [Critical/High]
- Primary categories: [List]

**Group 2:**
- Sources discovered: [X]
- Highest priority: [Critical/High]
- Primary categories: [List]

**Group 3:**
- Sources discovered: [X]
- Highest priority: [Critical/High]
- Primary categories: [List]

**Group 4:**
- Sources discovered: [X]
- Highest priority: [Critical/High]
- Primary categories: [List]

### Priority Distribution of Discovered Sources

| Priority | Count | Percentage |
|----------|-------|------------|
| Critical | [X] | [X]% |
| High | [X] | [X]% |
| Medium | [X] | [X]% |
| Low | [X] | [X]% |
| **Total** | [Y] | 100% |

### Category Distribution

| Category | Count | Priority Mix |
|----------|-------|--------------|
| GameDev-Tech | [X] | [Distribution] |
| GameDev-Design | [X] | [Distribution] |
| GameDev-Content | [X] | [Distribution] |
| Survival | [X] | [Distribution] |
| Other | [X] | [Distribution] |

### Recommended Phase [N] Structure

Based on discoveries, recommend:
- **Number of Groups:** [4 or different]
- **Estimated Timeline:** [N] weeks
- **Total Effort:** [X-Y] hours across all groups
- **Critical Focus:** [Key areas requiring immediate attention]
```

---

## Balancing Algorithm

When creating new groups from discovered sources:

1. **Sort by Priority:** Critical → High → Medium → Low
2. **Calculate Effort:** Sum estimated hours per source
3. **Target per Group:** Total effort / Number of groups
4. **Round-Robin Distribution:**
   - Distribute Critical sources evenly
   - Distribute High sources evenly
   - Fill with Medium and Low to balance effort
5. **Validate Balance:**
   - Each group has similar total effort (±5 hours)
   - Each group has mix of priorities
   - No group is all low priority or all critical

---

## Example: Creating Phase 2 from Phase 1 Discoveries

**Scenario:** Phase 1 complete, 20 new sources discovered

**Step 1: Collection**
- Group 1 discovered: 6 sources (2 High, 3 Medium, 1 Low)
- Group 2 discovered: 5 sources (1 Critical, 2 High, 2 Medium)
- Group 3 discovered: 4 sources (3 High, 1 Low)
- Group 4 discovered: 5 sources (2 High, 2 Medium, 1 Low)
- **Total:** 20 sources (1 Critical, 9 High, 7 Medium, 3 Low)

**Step 2: Prioritization**
- Validate: 1 Critical confirmed
- Review: 9 High priority (2 upgraded to Critical, 7 remain High)
- Adjust: 7 Medium (1 downgraded to Low, 6 remain Medium)
- Result: 2 Critical, 7 High, 6 Medium, 5 Low

**Step 3: Balance**
- Target groups: 4 (for parallel work)
- Sources per group: ~5
- Effort per group: ~25-35 hours

**Step 4: Distribution (Round-Robin)**
- Group 1: 1 Critical, 2 High, 2 Medium, 0 Low = 5 topics
- Group 2: 1 Critical, 2 High, 1 Medium, 2 Low = 6 topics
- Group 3: 0 Critical, 2 High, 2 Medium, 1 Low = 5 topics
- Group 4: 0 Critical, 1 High, 1 Medium, 2 Low = 4 topics

**Step 5: Create Files**
- Copy template 4 times
- Name: `research-assignment-phase-2-group-[1-4].md`
- Fill in topics for each group
- Update overview document

---

## File Naming Convention

- **Phase 1:** `research-assignment-group-[N].md` (already created)
- **Phase 2+:** `research-assignment-phase-[N]-group-[M].md`
- **Overview:** `research-assignment-groups-overview.md` (updated each phase)

---

## Integration with Master Queue

After organizing discovered sources:

1. Update master queue with all new sources
2. Mark originating research as "Source of [N] discoveries"
3. Add new sources to appropriate categories
4. Update total source counts
5. Recalculate completion percentages

---

**Template Version:** 1.0  
**Created:** 2025-01-15  
**Last Updated:** 2025-01-15  
**Status:** Template - Ready for Use  

---

*This template ensures consistent organization of discovered sources across research phases, maintaining quality and balance for parallel execution.*
