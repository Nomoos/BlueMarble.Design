# Phase 2: Next Steps and Issue Setup Guide

---
title: Phase 2 Next Steps and Issue Setup Guide
date: 2025-01-17
tags: [research, phase-2, planning, issues, github]
status: ready
---

**Document Type:** Implementation Guide  
**Purpose:** Provide step-by-step instructions for Phase 2 planning and GitHub issue creation  
**Target Audience:** Research coordinators, project managers, team leads  
**Prerequisites:** Phase 1 complete (100%)

---

## Executive Summary

Phase 1 research is complete (40/40 groups, 200+ documents, 250+ hours). This guide provides:
1. **Immediate next steps** for Phase 2 planning (2-3 days)
2. **Issue creation workflow** for Phase 2 assignment groups
3. **Automation scripts** and manual procedures
4. **Quality checklists** and validation steps

**Estimated Timeline:** 2-3 days for planning, then immediate Phase 2 execution start

---

## Table of Contents

1. [Phase 2 Planning Workflow](#phase-2-planning-workflow)
2. [Step-by-Step Issue Creation](#step-by-step-issue-creation)
3. [Automation Scripts](#automation-scripts)
4. [Manual Issue Creation](#manual-issue-creation)
5. [Phase 2 Assignment Files](#phase-2-assignment-files)
6. [Quality Validation](#quality-validation)
7. [Timeline and Milestones](#timeline-and-milestones)
8. [Troubleshooting](#troubleshooting)

---

## Phase 2 Planning Workflow

### Overview

Phase 2 planning converts 50-60 discovered sources from Phase 1 into organized assignment groups for parallel execution.

**Total Time:** 14-21 hours (2-3 days)

### Planning Steps

#### Step 1: Aggregate Discovered Sources (2-3 hours)

**Objective:** Collect all discovered sources from Phase 1 into a master list

**Inputs:**
- `discovered-sources-queue.md` (Group 36)
- `research-assignment-group-22-discovered-sources-queue.md` (Group 22)
- `assignment-group-35-discovered-sources-queue.md` (Group 35)
- Individual group assignment files (check "Discovered Sources Log" sections)

**Process:**
```bash
# 1. Create working directory
mkdir -p /tmp/phase2-planning
cd /tmp/phase2-planning

# 2. Extract all discovered sources
grep -r "Source Name:" research/literature/research-assignment-group-*.md > discovered-all.txt
grep -r "Source Name:" research/literature/discovered-sources-queue.md >> discovered-all.txt
grep -r "Source Name:" research/literature/assignment-group-*-discovered-sources-queue.md >> discovered-all.txt

# 3. Count sources
wc -l discovered-all.txt
```

**Output:** `phase2-discovered-sources-master-list.md`

**Template:**
```markdown
# Phase 2 Discovered Sources - Master List

**Date:** 2025-01-17
**Total Sources:** [COUNT]
**Source:** Aggregated from Phase 1 (Groups 1-40)

## Discovered Sources by Category

### GameDev-Tech (Networking, Architecture, Systems)
1. Source Name - Priority - Estimated Hours - Origin Group
2. ...

### GameDev-Design (Mechanics, Balance, Psychology)
1. ...

### GameDev-Art (Graphics, Audio, VFX)
1. ...

### Survival (Energy, Resources, Crafting)
1. ...

### Architecture (Distributed Systems, Databases)
1. ...
```

---

#### Step 2: Prioritize and Categorize (2-3 hours)

**Objective:** Assign priority and category to each source

**Criteria:**

**Priority Levels:**
- **Critical:** Blocks development, immediate business value (10%)
- **High:** Core features, significant impact (45%)
- **Medium:** Nice-to-have, improves quality (35%)
- **Low:** Future consideration, exploratory (10%)

**Categories:**
- GameDev-Tech (50%)
- GameDev-Design (24%)
- Survival (17%)
- GameDev-Art (8%)
- Architecture (3%)

**Process:**
1. Review each discovered source
2. Assign priority based on:
   - Implementation urgency
   - Business value
   - Technical dependencies
   - Risk reduction
3. Assign category based on content type
4. Estimate research hours (4-12h each)

**Output:** `phase2-sources-prioritized.md`

---

#### Step 3: Create Distribution Plan (3-4 hours)

**Objective:** Distribute sources across 40 Phase 2 groups

**Algorithm:**

```python
# Balancing Algorithm
total_sources = 50-60
target_groups = 40
sources_per_group = 1-2  # Average 1.25-1.5

# Distribution rules:
1. Critical sources → Groups 1-5 (immediate work)
2. High priority → Groups 6-25 (core work)
3. Medium priority → Groups 26-38 (quality work)
4. Low priority → Groups 39-40 (future work)

# Balance by category:
- Distribute each category evenly across groups
- Avoid grouping similar sources (parallel work)
- Match sources to group expertise if known
- Maintain 4-12 hour effort per group
```

**Constraints:**
- Minimum 4 hours per group (avoid too small)
- Maximum 12 hours per group (avoid too large)
- Isolate sources to prevent merge conflicts
- Balance skill requirements across groups

**Output:** `phase2-distribution-plan.md`

**Template:**
```markdown
# Phase 2 Distribution Plan

**Total Sources:** [COUNT]
**Total Groups:** 40
**Average per Group:** 1.25-1.5 sources

## Group Assignments

### Critical Priority Groups (1-5)
- Group 01: [Source A] (Critical, 8h) + [Source B] (High, 4h) = 12h
- Group 02: [Source C] (Critical, 10h) = 10h
- ...

### High Priority Groups (6-25)
- Group 06: [Source D] (High, 6h) + [Source E] (High, 6h) = 12h
- ...

### Medium Priority Groups (26-38)
- Group 26: [Source F] (Medium, 8h) = 8h
- ...

### Low Priority Groups (39-40)
- Group 39: [Source G] (Low, 4h) = 4h
- Group 40: [Source H] (Low, 4h) = 4h

## Statistics
- Critical: 5 sources (10%)
- High: 25 sources (45%)
- Medium: 20 sources (35%)
- Low: 5 sources (10%)
```

---

#### Step 4: Generate Phase 2 Assignment Files (4-6 hours)

**Objective:** Create 40 assignment files with proper structure

**Template Location:** `research/literature/research-assignment-template-phase-2.md`

**Process:**
```bash
# 1. Copy template for each group
for i in {01..40}; do
  cp research/literature/research-assignment-template-phase-2.md \
     research/literature/research-assignment-group-phase2-${i}.md
done

# 2. Customize each file with:
# - Group number
# - Assigned sources
# - Priority
# - Estimated effort
# - Deliverables
# - Quality standards
```

**Required Sections:**
- YAML front matter
- Overview
- Assigned Sources (1-2 sources)
- Expected Deliverables
- Quality Standards
- Progress Tracking
- Discovered Sources Log (for Phase 3)

**Example Assignment File:**
```markdown
# Research Assignment Group Phase 2-01

---
title: Research Assignment Phase 2 Group 01
date: 2025-01-20
tags: [research-queue, assignment, phase-2, critical]
status: ready
assignee: TBD
---

**Document Type:** Research Assignment (Phase 2)
**Priority:** Critical
**Estimated Effort:** 12 hours
**Total Sources:** 2

## Overview

This Phase 2 group focuses on [CATEGORY] research discovered during Phase 1.

## Assigned Sources

### Source 1: [Source Name] (CRITICAL, 8h)

**Origin:** Discovered in Phase 1 Group XX
**Priority:** Critical
**Estimated Effort:** 8 hours

**Research Focus:**
- [Topic 1]
- [Topic 2]
- [Topic 3]

**Expected Deliverable:** `[filename].md` (600-800 lines minimum)

**Why Critical:**
- [Business value]
- [Technical importance]
- [Implementation urgency]

### Source 2: [Source Name] (HIGH, 4h)

**Origin:** Discovered in Phase 1 Group YY
**Priority:** High
**Estimated Effort:** 4 hours

**Research Focus:**
- [Topic 1]
- [Topic 2]

**Expected Deliverable:** `[filename].md` (400-600 lines minimum)

## Progress Tracking

- [ ] Source 1: [Source Name]
- [ ] Source 2: [Source Name]
- [ ] Documents created with YAML front matter
- [ ] Quality standards met
- [ ] Discovered sources logged

## Discovered Sources Log

Log any new sources discovered during this research:

**Source Name:** [Name]
**URL/Reference:** [Link]
**Priority:** [Critical/High/Medium/Low]
**Estimated Effort:** [hours]
**Why Relevant:** [Explanation]

---

**Created:** 2025-01-20
**Status:** Ready for Assignment
**Phase:** 2
```

---

#### Step 5: Update Master Research Queue (1-2 hours)

**Objective:** Update tracking document with Phase 2 sources

**File:** `research/literature/master-research-queue.md`

**Updates:**
1. Add Phase 2 section
2. Update completion statistics
3. Add Phase 2 sources to queue
4. Update timeline and milestones

**Process:**
```bash
# Edit master-research-queue.md
# Add new section:

## Phase 2: Discovered Sources Research

### Sources from Phase 1 Discovery
**Total:** 50-60 sources
**Groups:** 40 parallel assignment groups
**Timeline:** 1-2 months
**Status:** Planning Complete, Ready to Start

### Phase 2 Categories
- GameDev-Tech: 25 sources
- GameDev-Design: 12 sources
- Survival: 9 sources
- GameDev-Art: 4 sources
- Architecture: 2 sources
```

---

#### Step 6: Create Phase 2 Issues (2-3 hours)

**See:** [Step-by-Step Issue Creation](#step-by-step-issue-creation) below

---

## Step-by-Step Issue Creation

### Prerequisites

**Required:**
- ✅ Phase 1 complete (100%)
- ✅ Phase 2 assignment files created (40 files)
- ✅ GitHub CLI installed (`gh`) OR manual access to GitHub
- ✅ Repository access with issue creation permissions

**Optional:**
- GitHub CLI authenticated: `gh auth login`
- PowerShell 5.1+ (Windows automation)
- Python 3.6+ (issue generation script)

---

### Option 1: Automated Issue Creation (Recommended)

#### Using Python Script + PowerShell (Windows)

**Step 1: Generate Issue Content**

```bash
# Generate all issue markdown files
cd /home/runner/work/BlueMarble.Design/BlueMarble.Design
python3 scripts/generate-research-issues.py

# Output: Creates 42 files in /tmp/research-issues/
# - issue-parent-phase-2.md
# - issue-group-phase2-01.md through issue-group-phase2-40.md
# - issue-phase-3-planning.md
```

**Step 2: Review Generated Issues**

```bash
# Check generated content
ls -la /tmp/research-issues/
head -50 /tmp/research-issues/issue-parent-phase-2.md
```

**Step 3: Create Issues with PowerShell**

```powershell
# Windows PowerShell
cd scripts
.\create-research-issues.ps1 -Phase 2

# With assignee
.\create-research-issues.ps1 -Phase 2 -Assignee copilot

# Dry run (preview without creating)
.\create-research-issues.ps1 -Phase 2 -DryRun
```

**What it does:**
- Creates parent Phase 2 issue
- Creates all 40 group issues
- Adds proper labels (`research`, `phase-2`, `assignment-group-XX`)
- Links groups to parent issue
- Adds 120-second delays to avoid rate limiting
- Shows progress and error handling

---

#### Using Bash Script (Linux/macOS/Git Bash)

**Step 1: Generate Issue Content**

```bash
python3 scripts/generate-research-issues.py
```

**Step 2: Create Parent Issue**

```bash
cd /tmp/research-issues

# Create parent issue and save number
PARENT_NUM=$(gh issue create \
  --title "Research Phase 2: Process 50-60 Discovered Sources Across 40 Groups" \
  --body-file issue-parent-phase-2.md \
  --label "research,phase-2,epic" \
  --repo Nomoos/BlueMarble.Design \
  | grep -oP '\d+$')

echo "Parent issue created: #${PARENT_NUM}"
```

**Step 3: Create Group Issues in Batch**

```bash
# Create all 40 group issues
for i in {01..40}; do
  echo "Creating issue for Group Phase2-${i}..."
  
  gh issue create \
    --title "Research Assignment Phase 2 Group ${i}" \
    --body-file "issue-group-phase2-${i}.md" \
    --label "research,phase-2,assignment-group-${i}" \
    --repo Nomoos/BlueMarble.Design
  
  # Add reference to parent in body
  # Note: Edit issue body to add "Part of #${PARENT_NUM}"
  
  # Rate limiting: wait 2 minutes between issues
  sleep 120
done
```

---

### Option 2: Manual Issue Creation

If automation isn't available, create issues manually through GitHub UI:

#### Step 1: Create Parent Issue

**Navigate to:** https://github.com/Nomoos/BlueMarble.Design/issues/new

**Title:**
```
Research Phase 2: Process 50-60 Discovered Sources Across 40 Groups
```

**Body:**
```markdown
# Phase 2 Research: 50-60 Discovered Sources

**Labels:** `research`, `phase-2`, `epic`, `parent-issue`

## Overview

Execute Phase 2 research on sources discovered during Phase 1.

**Total Groups:** 40 parallel assignment groups  
**Total Sources:** 50-60  
**Timeline:** 1-2 months  
**Average Effort:** 5-10 hours per group

## Prerequisites

- ✅ Phase 1 complete (40/40 groups)
- ✅ Discovered sources aggregated
- ✅ Distribution plan created
- ✅ Assignment files generated

## Sub-Issues (40 Assignment Groups)

### Critical Priority (Groups 1-5)
- [ ] #XXX Group Phase2-01: [Source A + Source B] (Critical, 12h)
- [ ] #XXX Group Phase2-02: [Source C] (Critical, 10h)
- [ ] #XXX Group Phase2-03: [Source D + Source E] (Critical, 12h)
- [ ] #XXX Group Phase2-04: [Source F] (Critical, 10h)
- [ ] #XXX Group Phase2-05: [Source G + Source H] (Critical, 12h)

### High Priority (Groups 6-25)
- [ ] #XXX Group Phase2-06: [Sources] (High, 10h)
- ... (list all 20 groups)

### Medium Priority (Groups 26-38)
- [ ] #XXX Group Phase2-26: [Sources] (Medium, 8h)
- ... (list all 13 groups)

### Low Priority (Groups 39-40)
- [ ] #XXX Group Phase2-39: [Source] (Low, 4h)
- [ ] #XXX Group Phase2-40: [Source] (Low, 4h)

## Progress Tracking

**Completed:** 0 of 40 groups (0%)  
**In Progress:** 0  
**Not Started:** 40

## Success Criteria

- All 40 groups complete with quality documentation
- All sources analyzed and documented
- New discoveries logged for Phase 3
- Implementation recommendations provided

## Resources

- Phase 2 Assignment Files: `/research/literature/research-assignment-group-phase2-[01-40].md`
- Phase 2 Planning Guide: `/research/literature/phase-2-next-steps-and-issue-setup.md`
- Master Research Queue: `/research/literature/master-research-queue.md`

---

**Phase:** 2  
**Status:** Ready to Start  
**Expected Completion:** [DATE + 2 months]
```

**Labels:** `research`, `phase-2`, `epic`, `parent-issue`

---

#### Step 2: Create Individual Group Issues

For each of the 40 groups, create an issue:

**Title Format:**
```
Research Assignment Phase 2 Group [01-40]
```

**Body Format:**
```markdown
# Research Assignment Phase 2 Group [XX]

**Labels:** `research`, `phase-2`, `assignment-group-[XX]`

**Part of:** #[PARENT_ISSUE_NUMBER]

## Assignment

**Priority:** [Critical/High/Medium/Low]  
**Estimated Effort:** [X-Y hours]  
**Total Sources:** [1-2]

### Sources Assigned

1. **[Source Name]** ([Priority], [Xh])
   - Focus: [Topics]
   - Deliverable: `[filename].md`
   - Why Important: [Explanation]

2. **[Source Name]** ([Priority], [Xh]) [if applicable]
   - Focus: [Topics]
   - Deliverable: `[filename].md`
   - Why Important: [Explanation]

## Deliverables

- [ ] Research document(s) with YAML front matter
- [ ] Minimum [400-800] lines per document
- [ ] Code examples and implementation guidance
- [ ] Cross-references to related research
- [ ] Discovered sources logged

## Quality Standards

- Comprehensive analysis of source material
- BlueMarble-specific implementation recommendations
- Production-ready code examples
- Clear documentation structure
- Proper citations and references

## Resources

- Assignment File: `/research/literature/research-assignment-group-phase2-[XX].md`
- Phase 1 Summary: `/research/literature/phase-1-research-summary.md`
- Quality Standards: `/research/literature/research-compliance-validation-guide.md`

---

**Phase:** 2  
**Status:** Ready for Assignment  
**Assignee:** TBD
```

**Labels:** `research`, `phase-2`, `assignment-group-[XX]`

**Priority Labels:**
- Critical: Add `priority-critical`
- High: Add `priority-high`
- Medium: Add `priority-medium`
- Low: Add `priority-low`

---

#### Step 3: Link Issues

After creating all issues:

1. Edit parent issue to add actual issue numbers in checklist
2. Add parent issue number to each group issue body
3. Update assignment files with issue numbers

---

## Phase 2 Assignment Files

### File Naming Convention

```
research-assignment-group-phase2-[01-40].md
```

### Required Structure

All Phase 2 assignment files must include:

1. **YAML Front Matter**
```yaml
---
title: Research Assignment Phase 2 Group [XX]
date: 2025-01-20
tags: [research-queue, assignment, phase-2, [priority]]
status: ready
assignee: TBD
---
```

2. **Overview Section**
- Document type
- Priority level
- Estimated effort
- Total sources

3. **Assigned Sources**
- For each source (1-2 per group):
  - Source name and priority
  - Origin (which Phase 1 group discovered it)
  - Estimated effort
  - Research focus areas
  - Expected deliverable filename
  - Business value / importance

4. **Expected Deliverables**
- List of documents to create
- Minimum line counts
- Quality requirements
- Cross-reference expectations

5. **Quality Standards**
- YAML front matter requirements
- Analysis depth
- Code example requirements
- Documentation structure
- Citation standards

6. **Progress Tracking**
- Checkbox list of all sources
- Checkbox for quality checks
- Checkbox for discovered sources logged

7. **Discovered Sources Log**
- Template for logging new sources
- Fields: Name, URL, Priority, Effort, Relevance

---

### Assignment File Template

See complete template at:
```
research/literature/research-assignment-template-phase-2.md
```

---

## Automation Scripts

### Python Issue Generator

**File:** `scripts/generate-research-issues.py`

**Usage:**
```bash
# Generate all Phase 2 issue files
python3 scripts/generate-research-issues.py --phase 2

# Output directory
ls -la /tmp/research-issues/

# Files created:
# - issue-parent-phase-2.md
# - issue-group-phase2-01.md through issue-group-phase2-40.md
# - README.md (usage instructions)
```

**Customization:**
Edit the script to modify:
- Issue templates
- Group configurations
- Label assignments
- Priority distributions

---

### PowerShell Automation Script

**File:** `scripts/create-research-issues.ps1`

**Usage:**
```powershell
# Create all Phase 2 issues
.\create-research-issues.ps1 -Phase 2

# With assignee
.\create-research-issues.ps1 -Phase 2 -Assignee copilot

# Dry run (preview without creating)
.\create-research-issues.ps1 -Phase 2 -DryRun

# Custom output directory
.\create-research-issues.ps1 -Phase 2 -OutputDir "C:\phase2-issues"
```

**Features:**
- Automatic rate limiting (120s between issues)
- Error handling and retry logic
- Progress reporting
- Dry run mode for testing
- Issue number capture for linking

---

### Bash Automation Script

Create `scripts/create-phase2-issues.sh`:

```bash
#!/bin/bash
# create-phase2-issues.sh
# Creates all Phase 2 research issues

set -e

REPO="Nomoos/BlueMarble.Design"
ISSUES_DIR="/tmp/research-issues"

echo "Creating Phase 2 research issues..."

# Check gh CLI
if ! command -v gh &> /dev/null; then
  echo "Error: GitHub CLI (gh) not installed"
  exit 1
fi

# Generate issue files
python3 scripts/generate-research-issues.py --phase 2

# Create parent issue
echo "Creating parent issue..."
PARENT_NUM=$(gh issue create \
  --title "Research Phase 2: Process 50-60 Discovered Sources" \
  --body-file "${ISSUES_DIR}/issue-parent-phase-2.md" \
  --label "research,phase-2,epic" \
  --repo "$REPO" \
  | grep -oP '\d+$')

echo "Parent issue created: #${PARENT_NUM}"
echo ""

# Create group issues
for i in {01..40}; do
  echo "Creating Group Phase2-${i} issue..."
  
  # Add parent reference to body
  BODY_FILE="${ISSUES_DIR}/issue-group-phase2-${i}.md"
  echo "" >> "$BODY_FILE"
  echo "**Part of:** #${PARENT_NUM}" >> "$BODY_FILE"
  
  gh issue create \
    --title "Research Assignment Phase 2 Group ${i}" \
    --body-file "$BODY_FILE" \
    --label "research,phase-2,assignment-group-${i}" \
    --repo "$REPO"
  
  echo "✅ Group ${i} created"
  echo ""
  
  # Rate limiting
  if [ "$i" != "40" ]; then
    echo "Waiting 120 seconds to avoid rate limiting..."
    sleep 120
  fi
done

echo ""
echo "✅ All Phase 2 issues created!"
echo "Parent issue: #${PARENT_NUM}"
echo "Group issues: 40 created"
```

**Make executable:**
```bash
chmod +x scripts/create-phase2-issues.sh
```

---

## Quality Validation

### Pre-Launch Checklist

Before starting Phase 2, validate:

- [ ] All Phase 1 groups marked complete (100%)
- [ ] All discovered sources aggregated
- [ ] Distribution plan reviewed and approved
- [ ] All 40 assignment files created
- [ ] All assignment files follow template structure
- [ ] YAML front matter present on all files
- [ ] Master research queue updated
- [ ] Issues created and linked
- [ ] Team members assigned to groups

---

### Phase 2 Assignment File Validation

**Script:** `scripts/validate-phase2-assignments.sh`

```bash
#!/bin/bash
# Validate Phase 2 assignment files

ERRORS=0

echo "Validating Phase 2 assignment files..."

for i in {01..40}; do
  FILE="research/literature/research-assignment-group-phase2-${i}.md"
  
  if [ ! -f "$FILE" ]; then
    echo "❌ Missing: $FILE"
    ((ERRORS++))
    continue
  fi
  
  # Check YAML front matter
  if ! head -1 "$FILE" | grep -q "^---$"; then
    echo "❌ $FILE: Missing YAML front matter"
    ((ERRORS++))
  fi
  
  # Check required sections
  if ! grep -q "## Overview" "$FILE"; then
    echo "❌ $FILE: Missing Overview section"
    ((ERRORS++))
  fi
  
  if ! grep -q "## Assigned Sources" "$FILE"; then
    echo "❌ $FILE: Missing Assigned Sources section"
    ((ERRORS++))
  fi
  
  if ! grep -q "## Progress Tracking" "$FILE"; then
    echo "❌ $FILE: Missing Progress Tracking section"
    ((ERRORS++))
  fi
  
  echo "✅ Group Phase2-${i} validated"
done

if [ $ERRORS -eq 0 ]; then
  echo ""
  echo "✅ All Phase 2 assignment files valid!"
  exit 0
else
  echo ""
  echo "❌ Found $ERRORS validation errors"
  exit 1
fi
```

---

### Issue Validation

After creating issues:

```bash
# Check parent issue exists
gh issue view [PARENT_NUM] --repo Nomoos/BlueMarble.Design

# Check all group issues exist
for i in {01..40}; do
  gh issue list --label "assignment-group-${i}" --repo Nomoos/BlueMarble.Design
done

# Check label distribution
gh issue list --label "phase-2" --repo Nomoos/BlueMarble.Design --state open --limit 50
```

---

## Timeline and Milestones

### Week 1: Planning Complete ✅

- ✅ Phase 1 validation (100% complete)
- ✅ Discovered sources aggregation
- ✅ Prioritization and categorization
- ✅ Distribution plan
- ✅ Assignment files created
- ✅ Issues created

**Milestone:** Planning Complete, Ready to Execute

---

### Week 2-4: Critical & High Priority Groups (Groups 1-25)

**Objective:** Complete critical and high-priority research

**Groups:** 1-25 (25 groups)  
**Effort:** 200-250 hours  
**Timeline:** 2-3 weeks with parallel execution

**Milestones:**
- Week 2: Groups 1-10 complete
- Week 3: Groups 11-20 complete
- Week 4: Groups 21-25 complete

---

### Week 5-7: Medium Priority Groups (Groups 26-38)

**Objective:** Complete medium-priority research

**Groups:** 26-38 (13 groups)  
**Effort:** 80-120 hours  
**Timeline:** 2-3 weeks

**Milestone:** Week 7 - All medium priority complete

---

### Week 8: Low Priority & Wrap-Up (Groups 39-40)

**Objective:** Complete remaining research and prepare Phase 3

**Groups:** 39-40 (2 groups)  
**Effort:** 8-16 hours  

**Activities:**
- Complete final groups
- Aggregate Phase 2 discoveries
- Create Phase 2 summary
- Plan Phase 3 (if needed)
- Update master research queue

**Milestone:** Phase 2 Complete

---

## Troubleshooting

### Common Issues

#### Issue: "gh: command not found"

**Solution:**
```bash
# Install GitHub CLI
# Ubuntu/Debian
sudo apt install gh

# macOS
brew install gh

# Windows
winget install GitHub.cli

# Then authenticate
gh auth login
```

---

#### Issue: "Rate limit exceeded"

**Solution:**
- Add delays between issue creation (120 seconds minimum)
- Use authenticated requests (gh CLI logged in)
- Create issues in smaller batches
- Wait 1 hour if limit hit, then resume

---

#### Issue: "Permission denied when creating issues"

**Solution:**
- Verify repository access: `gh repo view Nomoos/BlueMarble.Design`
- Check permissions: Must have write access
- Re-authenticate: `gh auth login`
- Verify organization membership if private repo

---

#### Issue: "Assignment files not found"

**Solution:**
```bash
# Check if files exist
ls research/literature/research-assignment-group-phase2-*.md

# If missing, create from template
for i in {01..40}; do
  cp research/literature/research-assignment-template-phase-2.md \
     research/literature/research-assignment-group-phase2-${i}.md
done

# Then customize each file
```

---

#### Issue: "Python script fails"

**Solution:**
```bash
# Check Python version (need 3.6+)
python3 --version

# Check script exists
ls scripts/generate-research-issues.py

# Run with verbose output
python3 -v scripts/generate-research-issues.py

# Check output directory
mkdir -p /tmp/research-issues
```

---

#### Issue: "Cannot link parent and group issues"

**Solution:**
1. Create parent issue first, note the issue number
2. Edit group issue templates to add parent reference
3. Create group issues
4. Manually edit parent issue to add group issue numbers in checklist

**Or:**
Use GitHub API or CLI to update issues programmatically:
```bash
# Update parent issue body
gh issue edit [PARENT_NUM] --body-file updated-parent.md
```

---

## Quick Reference Commands

### Validation
```bash
# Validate Phase 1 complete
./scripts/validate-phase1-completion.sh

# Validate Phase 2 assignments
./scripts/validate-phase2-assignments.sh
```

### Issue Creation
```bash
# Generate issues (Python)
python3 scripts/generate-research-issues.py --phase 2

# Create issues (PowerShell - Windows)
.\scripts\create-research-issues.ps1 -Phase 2

# Create issues (Bash - Linux/Mac)
./scripts/create-phase2-issues.sh
```

### Status Checking
```bash
# Check Phase 2 issues
gh issue list --label "phase-2" --repo Nomoos/BlueMarble.Design

# Check specific group
gh issue list --label "assignment-group-01" --repo Nomoos/BlueMarble.Design

# Check parent issue
gh issue view [PARENT_NUM] --repo Nomoos/BlueMarble.Design
```

---

## Summary

### Immediate Actions

1. **Aggregate discovered sources** (2-3 hours)
   - Collect from all Phase 1 groups
   - Create master list
   - Count and categorize

2. **Prioritize and categorize** (2-3 hours)
   - Assign priority to each source
   - Assign category
   - Estimate effort

3. **Create distribution plan** (3-4 hours)
   - Balance across 40 groups
   - Match priority to group numbers
   - Ensure 4-12 hour effort per group

4. **Generate assignment files** (4-6 hours)
   - Use template for each group
   - Customize with source details
   - Validate structure

5. **Update master queue** (1-2 hours)
   - Add Phase 2 section
   - Update statistics
   - Update timeline

6. **Create GitHub issues** (2-3 hours)
   - Generate issue content
   - Create parent issue
   - Create 40 group issues
   - Link all issues

**Total Time:** 14-21 hours (2-3 days)

---

### Success Criteria

✅ All discovered sources aggregated  
✅ Distribution plan approved  
✅ 40 assignment files created  
✅ All files validated  
✅ Master queue updated  
✅ GitHub issues created  
✅ Team ready to start Phase 2

---

### Expected Outcomes

**By Week 1:**
- Phase 2 planning complete
- Issues assigned to team members
- First groups start research

**By Month 2:**
- Phase 2 execution complete
- 50-60 sources researched and documented
- Phase 3 discoveries logged
- Implementation priorities updated

---

## Related Documents

- [Phase 1 Research Summary](phase-1-research-summary.md)
- [Phase 1 Completion Status](phase-1-completion-status.md)
- [Phase 2 Planning Quick Start](phase-2-planning-quick-start.md)
- [Master Research Queue](master-research-queue.md)
- [Research Assignment Groups Overview](research-assignment-groups-overview.md)
- [Research Compliance Validation Guide](research-compliance-validation-guide.md)

---

**Created:** 2025-01-17  
**Status:** Ready for Use  
**Next Update:** After Phase 2 planning complete  
**Contact:** Research coordinators for questions

---

🚀 **Ready to Launch Phase 2!** 🚀
