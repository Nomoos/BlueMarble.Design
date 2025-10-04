# Universal Copilot Research Instructions

---
title: Universal Copilot Research Instructions
date: 2025-01-17
tags: [research, copilot, instructions, automation]
status: active
type: instructions
---

**Document Type:** Copilot Worker Instructions  
**Version:** 1.0  
**Purpose:** Universal instructions for Copilot workers processing research assignments  
**Usage:** Copy this block into any research assignment issue or file

---

## 🤖 Copilot Worker Instructions

**File to Process:** `research/literature/research-assignment-group-{NUMBER}.md`

Replace `{NUMBER}` with your assigned group number (01-40).

### Your Task

You are a research assistant working on **Research Assignment Group {NUMBER}**. Your job is to:

1. **Read and analyze** the sources listed in your assignment file
2. **Create comprehensive research documents** based on your analysis
3. **Log any new sources** you discover during your research
4. **Save all work** to the repository in the correct format

### Step-by-Step Workflow

#### Phase 1: Locate Your Assignment

```bash
# Navigate to your assignment file
cd /home/runner/work/BlueMarble.Design/BlueMarble.Design
cat research/literature/research-assignment-group-{NUMBER}.md
```

#### Phase 2: Read Source Material

1. **Identify your topics** in the "Topics" section of your assignment file
2. **Read the source material** for each topic:
   - For books: Read chapters/sections relevant to focus areas
   - For online resources: Review documentation, code examples, tutorials
   - For code repositories: Analyze architecture, patterns, implementations
3. **Take structured notes** as you read:
   - Key concepts and patterns
   - Code examples worth highlighting
   - Implementation recommendations
   - Related sources discovered

#### Phase 3: Create Research Documents

For each topic in your assignment, create a new file:

**File Location:** `research/literature/`  
**File Name:** Use kebab-case based on topic:
- Game development topics: `game-dev-analysis-{topic-name}.md`
- Survival content topics: `survival-content-extraction-{topic-name}.md`
- Other topics: `{category}-{topic-name}.md`

**Document Structure:**

```markdown
---
title: [Topic Title]
date: [YYYY-MM-DD]
tags: [research, analysis, {category}, {priority}]
status: completed
source: [Original source reference]
---

# [Topic Title] - Research Analysis

**Source:** [Source name and reference]  
**Analysis Date:** [YYYY-MM-DD]  
**Priority:** [Critical/High/Medium/Low]  
**Category:** [GameDev-Tech/GameDev-Design/etc.]  
**Analyzed By:** Copilot Research Assistant

## Executive Summary

[2-3 paragraphs summarizing key findings and recommendations]

## Source Overview

**What was analyzed:**
- [Source type: book, documentation, repository, etc.]
- [Sections/chapters covered]
- [Version/date of source material]

**Why this source matters:**
- [Relevance to BlueMarble project]
- [Key problems it solves]

## Core Concepts

### Concept 1: [Name]

[Detailed explanation]

**Key Points:**
- [Point 1]
- [Point 2]
- [Point 3]

**Code Example (if applicable):**
```[language]
// Example code demonstrating the concept
```

### Concept 2: [Name]

[Continue for each major concept...]

## BlueMarble Application

### How to Apply to BlueMarble

1. **[Application Area 1]**
   - Implementation approach
   - Benefits
   - Considerations

2. **[Application Area 2]**
   - Implementation approach
   - Benefits
   - Considerations

### Integration Recommendations

- [Specific recommendation 1]
- [Specific recommendation 2]
- [Specific recommendation 3]

## Implementation Recommendations

### Immediate Actions

1. **[Action 1]**
   - What: [Description]
   - Why: [Rationale]
   - How: [Implementation steps]

2. **[Action 2]**
   - What: [Description]
   - Why: [Rationale]
   - How: [Implementation steps]

### Long-term Considerations

- [Consideration 1]
- [Consideration 2]

## Discovered Sources

During analysis, the following additional sources were identified:

### [Source 1 Name]

**Type:** [Book/Documentation/Repository/Tutorial/etc.]  
**URL/Reference:** [Link or citation]  
**Priority Assessment:** [Critical/High/Medium/Low]  
**Category:** [GameDev-Tech/GameDev-Design/etc.]  
**Why Relevant:** [Brief explanation]  
**Estimated Effort:** [X-Y hours]  
**Discovered From:** [Which concept/section led to this discovery]

### [Source 2 Name]

[Repeat for each discovered source...]

## References

1. [Primary source citation]
2. [Additional references used]
3. [Related documentation links]

## Cross-References

Related research documents:
- [Link to related document 1]
- [Link to related document 2]

---

**Document Status:** Complete  
**Word Count:** [Approximate count]  
**Lines:** [Approximate line count]  
**Quality Check:** ✅ Meets minimum length requirements
```

#### Phase 4: Log Discovered Sources

**Important:** As you research, you'll discover new sources. Log them in TWO places:

1. **In your research document** (as shown above in "Discovered Sources" section)
2. **In your assignment file** under "New Sources Discovery" section

**Format for assignment file:**

```markdown
### Discovered Sources Log

**Source Name:** [Title of discovered source]  
**Discovered From:** [Which topic/analysis led to this]  
**Priority:** [Critical/High/Medium/Low - your assessment]  
**Category:** [GameDev-Tech/GameDev-Design/GameDev-Content/Survival/etc.]  
**Rationale:** [Why this source is relevant to BlueMarble]  
**Estimated Effort:** [X-Y hours needed for analysis]  
**Reference:** [URL or citation]

---
```

#### Phase 5: Update Progress Tracking

In your assignment file (`research-assignment-group-{NUMBER}.md`), update the "Progress Tracking" section:

```markdown
## Progress Tracking

- [x] [Topic 1 name] - Completed [YYYY-MM-DD]
- [ ] [Topic 2 name] - In Progress
- [x] All documents created and placed in `research/literature/`
- [x] All documents have proper front matter
- [x] Discovered sources logged
```

#### Phase 6: Save and Submit

1. **Verify all files are in correct location:**
   ```bash
   ls -la research/literature/game-dev-analysis-*.md
   ls -la research/literature/survival-content-extraction-*.md
   ```

2. **Check file naming (kebab-case):**
   - ✅ Good: `game-dev-analysis-multiplayer-programming.md`
   - ❌ Bad: `Game_Dev_Analysis_Multiplayer_Programming.md`

3. **Verify front matter in each document:**
   ```bash
   head -10 research/literature/[your-file].md
   ```

4. **Commit your work:**
   - All research documents should be in `research/literature/`
   - Assignment file should be updated with progress and discoveries
   - Use descriptive commit messages

### Quality Checklist

Before marking your work complete, verify:

- [ ] ✅ All topics from assignment file researched
- [ ] ✅ Each topic has a dedicated analysis document
- [ ] ✅ All documents in `research/literature/` directory
- [ ] ✅ All documents use kebab-case naming
- [ ] ✅ All documents have YAML front matter
- [ ] ✅ Documents meet minimum length requirements (see assignment file)
- [ ] ✅ Code examples included where relevant
- [ ] ✅ Citations and references properly formatted
- [ ] ✅ Cross-references added to related documents
- [ ] ✅ Discovered sources logged in both places
- [ ] ✅ Progress tracking updated in assignment file
- [ ] ✅ BlueMarble-specific recommendations included

### Common Pitfalls to Avoid

1. **Don't skip source discovery logging** - This is critical for Phase 2 planning
2. **Don't use title case in filenames** - Always use kebab-case
3. **Don't forget YAML front matter** - It's required for all documents
4. **Don't create generic summaries** - Include specific, actionable recommendations
5. **Don't ignore minimum length requirements** - They vary by priority level
6. **Don't forget code examples** - Include them for technical topics
7. **Don't work in isolation** - Add cross-references to related research

### File Naming Examples

**Game Development Topics:**
- `game-dev-analysis-multiplayer-programming.md`
- `game-dev-analysis-network-architecture.md`
- `game-dev-analysis-rendering-pipelines.md`

**Survival Content Topics:**
- `survival-content-extraction-medical-systems.md`
- `survival-content-extraction-crafting-recipes.md`
- `survival-content-extraction-technology-trees.md`

**Other Topics:**
- `design-patterns-mmorpg-architecture.md`
- `algorithm-analysis-pathfinding-systems.md`
- `tools-analysis-development-workflows.md`

### Getting Help

If you encounter issues:

1. **Check examples:**
   - `research/literature/example-topic.md` - Format example
   - `research/literature/game-dev-analysis-01-game-programming-cpp.md` - Complete example

2. **Review guidelines:**
   - `research/literature/README.md` - General guidelines
   - `research/literature/research-assignment-groups-overview.md` - Overview
   - `research/literature/phase-1-research-note.md` - Phase 1 coordination

3. **Check templates:**
   - `templates/research-note.md` - Research note template
   - `templates/research-report.md` - Research report template

### Automation Tips for Copilot Workers

**Quick commands to get started:**

```bash
# 1. Navigate to repository
cd /home/runner/work/BlueMarble.Design/BlueMarble.Design

# 2. Read your assignment
export GROUP_NUM="01"  # Change to your group number
cat research/literature/research-assignment-group-${GROUP_NUM}.md

# 3. List topics in your assignment
grep "^### " research/literature/research-assignment-group-${GROUP_NUM}.md

# 4. Create your first research document
touch research/literature/game-dev-analysis-[topic-name].md

# 5. Check progress
grep "^- \[" research/literature/research-assignment-group-${GROUP_NUM}.md
```

---

## Summary for Copilot Workers

**Your mission:** Research sources → Create analysis documents → Log discoveries → Save everything

**Three key actions:**
1. **READ** `research-assignment-group-{NUMBER}.md` for your topics
2. **CREATE** analysis documents in `research/literature/` for each topic
3. **LOG** discovered sources in your assignment file

**Output format:**
- Comprehensive markdown documents
- Proper YAML front matter
- Kebab-case filenames
- Minimum length requirements met
- Discovered sources logged

**Success criteria:**
- All topics researched and documented
- All discoveries logged for Phase 2
- All quality standards met
- Progress tracking updated

---

**Instructions Version:** 1.0  
**Last Updated:** 2025-01-17  
**Applies To:** All Phase 1 research assignment groups (01-40)  
**Next Review:** Phase 2 planning

---

*These instructions are designed to be copy-pasted into any research assignment issue or file to guide Copilot workers through the research process. They are self-contained and require only the group number to be customized.*
