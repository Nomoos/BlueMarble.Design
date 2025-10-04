# Validation Guide: Sub-Issues and Pull Requests Compliance

---
title: Validation Guide for Research Assignment Compliance
date: 2025-01-17
tags: [research, validation, compliance, quality-assurance]
status: active
type: validation-guide
---

**Document Type:** Validation Guide  
**Version:** 1.0  
**Purpose:** Ensure sub-issues and pull requests respect the requirement to process sources from `research-assignment-group-{NUMBER}.md` files  
**Audience:** Coordinators, reviewers, and quality assurance

---

## Overview

This guide provides validation criteria and checklists to verify that research sub-issues and pull requests properly follow the Phase 1 research organization requirements.

**Key Requirement:** Each child issue (Groups 01-40) must process sources from their respective assignment file: `research/literature/research-assignment-group-{NUMBER}.md`

---

## Validation Checklist for Sub-Issues

### Issue Structure Validation

Use this checklist when reviewing or creating sub-issues for research assignments:

#### ✅ Issue Title
- [ ] Follows format: `Research Assignment Group {NUMBER}: {TOPIC_SUMMARY}`
- [ ] Group number is between 01-40
- [ ] Topic summary matches assignment file content

#### ✅ Issue Labels
- [ ] Has `research` label
- [ ] Has `assignment-group-{NUMBER}` label (e.g., `assignment-group-01`)
- [ ] Has appropriate priority label (`priority-critical`, `priority-high`, `priority-medium`, or `priority-low`)
- [ ] Has `phase-1` label

#### ✅ Assignment File Reference
- [ ] Issue description explicitly mentions: `research/literature/research-assignment-group-{NUMBER}.md`
- [ ] Assignment file path is correct and matches group number
- [ ] Issue links to the correct assignment file in "Support Resources" section

#### ✅ Topics Alignment
- [ ] Topics listed in issue match topics in the assignment file
- [ ] Topic count matches the assignment file
- [ ] Priority levels match the assignment file

#### ✅ Instructions Reference
- [ ] Issue references or includes instructions on processing sources from assignment file
- [ ] Links to `copilot-research-instructions.md` or `phase-1-research-note.md` provided
- [ ] Clear directive to read and process sources from the group-specific assignment file

#### ✅ Deliverables Section
- [ ] Specifies output location: `research/literature/`
- [ ] Mentions file naming conventions (kebab-case)
- [ ] Lists required document sections
- [ ] References quality standards

#### ✅ Progress Tracking
- [ ] Has checklist for each topic in the assignment
- [ ] Includes discovery logging checkbox
- [ ] Includes submission checklist items

---

## Validation Checklist for Pull Requests

### PR Content Validation

Use this checklist when reviewing pull requests for research assignments:

#### ✅ PR Title and Description
- [ ] PR title indicates which group(s) are addressed (e.g., "Research Group 01: Multiplayer Programming Analysis")
- [ ] PR description references the assignment file processed: `research-assignment-group-{NUMBER}.md`
- [ ] PR links to the parent issue (#183) and specific sub-issue

#### ✅ Files Changed
- [ ] New research documents are in `research/literature/` directory
- [ ] File names use kebab-case (e.g., `game-dev-analysis-topic-name.md`)
- [ ] No files created in wrong directories
- [ ] Assignment file (`research-assignment-group-{NUMBER}.md`) is updated with:
  - Progress tracking checkboxes marked complete
  - Discovered sources logged (if any)

#### ✅ Research Document Quality
- [ ] Each document has proper YAML front matter
- [ ] Documents include all required sections:
  - Executive Summary
  - Source Overview
  - Core Concepts
  - BlueMarble Application
  - Implementation Recommendations
  - References
- [ ] Documents meet minimum length requirements (check assignment file for specifics)
- [ ] Code examples included where relevant
- [ ] Cross-references to related documents added

#### ✅ Source Processing Verification
- [ ] PR demonstrates that sources were read from the assignment file
- [ ] Topics covered match the assignment file topics
- [ ] Analysis depth is appropriate for priority level (Critical > High > Medium > Low)
- [ ] Focus areas from assignment file are addressed

#### ✅ Discovery Logging
- [ ] If new sources discovered, they are logged in the assignment file
- [ ] Discovered sources use the proper template format:
  - Source Name
  - Discovered From
  - Priority
  - Category
  - Rationale
  - Estimated Effort
- [ ] Discovered sources also mentioned in research documents

#### ✅ Quality Standards
- [ ] YAML front matter is complete and correct
- [ ] Minimum length requirements met
- [ ] Citations and references properly formatted
- [ ] No spelling or formatting errors (run linters if available)

---

## Automated Validation Commands

### Check Issue Compliance

```bash
# List all research assignment group issues
gh issue list --label "phase-1" --label "research" --json number,title,labels,body

# Check if a specific issue references the assignment file
gh issue view {ISSUE_NUMBER} --json body | grep -o "research-assignment-group-[0-9]\{2\}\.md"

# Verify issue has correct labels
gh issue view {ISSUE_NUMBER} --json labels | jq '.labels[].name'
```

### Check PR Compliance

```bash
# List all research PRs
gh pr list --label "research" --json number,title,files

# Check files changed in a PR
gh pr view {PR_NUMBER} --json files | jq '.files[].path'

# Verify PR references assignment file
gh pr view {PR_NUMBER} --json body | grep -o "research-assignment-group-[0-9]\{2\}\.md"

# Check if research documents are in correct directory
gh pr view {PR_NUMBER} --json files | jq '.files[].path' | grep "research/literature/"
```

### Validate File Naming

```bash
# Check for proper kebab-case naming in research documents
cd /home/runner/work/BlueMarble.Design/BlueMarble.Design/research/literature
ls -1 *.md | grep -v "^[a-z0-9-]*\.md$" && echo "Found files not in kebab-case" || echo "All files properly named"

# Check for YAML front matter in all research documents
for file in research/literature/game-dev-analysis-*.md research/literature/survival-content-extraction-*.md; do
  if [ -f "$file" ]; then
    head -1 "$file" | grep -q "^---$" || echo "Missing front matter: $file"
  fi
done
```

### Validate Assignment File Updates

```bash
# Check if assignment file has been updated with progress
GROUP_NUM="01"  # Change as needed
grep "\[x\]" research/literature/research-assignment-group-${GROUP_NUM}.md || echo "No completed tasks in Group ${GROUP_NUM}"

# Check if discovered sources were logged
grep -A5 "Discovered Sources Log" research/literature/research-assignment-group-${GROUP_NUM}.md | grep "Source Name:" || echo "No discoveries logged in Group ${GROUP_NUM}"
```

---

## Common Compliance Issues

### Issue: Sub-issue doesn't reference assignment file

**Problem:** Issue description is generic and doesn't mention the specific assignment file.

**Solution:** Update issue description to explicitly state:
```markdown
**Assignment File:** `research/literature/research-assignment-group-{NUMBER}.md`
```

**Validation:** Run `gh issue view {ISSUE_NUMBER} --json body | grep "research-assignment-group"`

---

### Issue: PR creates files in wrong directory

**Problem:** Research documents are created outside `research/literature/` directory.

**Solution:** Move files to correct location:
```bash
git mv wrong/location/file.md research/literature/file.md
```

**Validation:** All research documents should be in `research/literature/` directory only.

---

### Issue: File naming doesn't follow kebab-case

**Problem:** Files use title-case, snake_case, or spaces in names.

**Examples:**
- ❌ `Game_Dev_Analysis.md`
- ❌ `game dev analysis.md`
- ❌ `GameDevAnalysis.md`
- ✅ `game-dev-analysis.md`

**Solution:** Rename files to kebab-case:
```bash
git mv Game_Dev_Analysis.md game-dev-analysis.md
```

**Validation:** Run the kebab-case checker script above.

---

### Issue: Topics don't match assignment file

**Problem:** PR analyzes different topics than listed in the assignment file.

**Solution:** 
1. Verify the correct assignment file was read
2. Ensure topics analyzed match the "Topics" section of the assignment file
3. If topics are correct but assignment file is wrong, update assignment file

**Validation:** Compare issue/PR topics with assignment file:
```bash
GROUP_NUM="01"
grep "^### [0-9]" research/literature/research-assignment-group-${GROUP_NUM}.md
```

---

### Issue: Missing YAML front matter

**Problem:** Research documents don't have proper YAML front matter at the top.

**Solution:** Add front matter to documents:
```yaml
---
title: [Topic Title]
date: [YYYY-MM-DD]
tags: [research, analysis, category, priority]
status: completed
source: [Source reference]
---
```

**Validation:** Check first 10 lines of each document:
```bash
head -10 research/literature/your-file.md
```

---

### Issue: Discovered sources not logged

**Problem:** New sources were found during research but not logged in assignment file.

**Solution:** Update assignment file's "Discovered Sources Log" section:
```markdown
### Discovered Sources Log

**Source Name:** [Title]
**Discovered From:** [Topic that led to discovery]
**Priority:** [Critical/High/Medium/Low]
**Category:** [Category]
**Rationale:** [Why relevant]
**Estimated Effort:** [X-Y hours]
**Reference:** [URL or citation]

---
```

**Validation:** Check assignment file for logged discoveries:
```bash
grep -A10 "Discovered Sources Log" research/literature/research-assignment-group-01.md
```

---

## Manual Review Checklist

For comprehensive validation, manually review:

### Sub-Issue Review
1. **Read the issue description**
   - Does it clearly state the assignment file to process?
   - Are the topics listed correctly?
   - Is the group number correct?

2. **Check issue metadata**
   - Correct labels applied?
   - Assigned to appropriate person?
   - Linked to parent issue #183?

3. **Verify instructions**
   - Does the issue include or reference processing instructions?
   - Is `copilot-research-instructions.md` linked?
   - Are the deliverable requirements clear?

### Pull Request Review
1. **Read the PR description**
   - Does it reference the assignment file processed?
   - Does it link to the parent issue and sub-issue?
   - Are changes summarized clearly?

2. **Review files changed**
   - All files in `research/literature/` directory?
   - File names use kebab-case?
   - Assignment file updated with progress?

3. **Check document quality**
   - Open each research document and verify:
     - YAML front matter present and complete
     - All required sections included
     - Appropriate length and depth
     - Code examples where relevant
     - Cross-references added

4. **Verify source processing**
   - Compare document content with assignment file topics
   - Ensure focus areas from assignment are addressed
   - Check that priority level is respected in analysis depth

5. **Validate discoveries**
   - If new sources mentioned in documents, are they logged?
   - Is the discovery logging format correct?
   - Are discoveries in both research doc and assignment file?

---

## Compliance Report Template

Use this template to report validation results:

```markdown
## Compliance Validation Report

**Date:** [YYYY-MM-DD]
**Reviewer:** [Your name]
**Scope:** [Issue numbers or PR numbers reviewed]

### Summary
- Total items reviewed: [X]
- Fully compliant: [X]
- Compliant with minor issues: [X]
- Non-compliant: [X]

### Detailed Findings

#### Issue #{NUMBER} / PR #{NUMBER}
- **Status:** [Compliant / Minor Issues / Non-Compliant]
- **Group Number:** [01-40]
- **Assignment File Referenced:** [Yes/No]
- **Issues Found:**
  - [Issue 1]
  - [Issue 2]
- **Recommendations:**
  - [Recommendation 1]
  - [Recommendation 2]

[Repeat for each item reviewed]

### Common Issues Identified
1. [Most common issue]
2. [Second most common issue]
3. [Third most common issue]

### Recommendations for Improvement
- [Improvement 1]
- [Improvement 2]
- [Improvement 3]
```

---

## Quick Compliance Check Script

Save this as a shell script for quick validation:

```bash
#!/bin/bash
# validate-research-compliance.sh

GROUP_NUM=$1

if [ -z "$GROUP_NUM" ]; then
  echo "Usage: $0 {GROUP_NUMBER}"
  echo "Example: $0 01"
  exit 1
fi

echo "=== Validating Research Assignment Group ${GROUP_NUM} ==="
echo ""

# Check if assignment file exists
ASSIGNMENT_FILE="research/literature/research-assignment-group-${GROUP_NUM}.md"
if [ ! -f "$ASSIGNMENT_FILE" ]; then
  echo "❌ Assignment file not found: $ASSIGNMENT_FILE"
  exit 1
else
  echo "✅ Assignment file exists"
fi

# Check for progress updates
if grep -q "\[x\]" "$ASSIGNMENT_FILE"; then
  echo "✅ Progress tracked in assignment file"
  COMPLETED=$(grep -c "\[x\]" "$ASSIGNMENT_FILE")
  echo "   Completed items: $COMPLETED"
else
  echo "⚠️  No completed progress items found"
fi

# Check for discovered sources
if grep -A5 "Discovered Sources Log" "$ASSIGNMENT_FILE" | grep -q "Source Name:"; then
  echo "✅ Discovered sources logged"
  DISCOVERIES=$(grep -A20 "Discovered Sources Log" "$ASSIGNMENT_FILE" | grep -c "Source Name:")
  echo "   Discoveries: $DISCOVERIES"
else
  echo "ℹ️  No discovered sources logged (may be none found)"
fi

# Check for research documents matching this group
echo ""
echo "=== Research Documents ==="
DOCS=$(find research/literature -name "*.md" -type f -exec grep -l "group.*${GROUP_NUM}\|Group ${GROUP_NUM}" {} \; 2>/dev/null)
if [ -n "$DOCS" ]; then
  echo "$DOCS" | while read doc; do
    echo "✅ Found: $doc"
    # Check for YAML front matter
    if head -1 "$doc" | grep -q "^---$"; then
      echo "   ✅ Has YAML front matter"
    else
      echo "   ❌ Missing YAML front matter"
    fi
  done
else
  echo "⚠️  No research documents found for Group ${GROUP_NUM}"
fi

echo ""
echo "=== Validation Complete ==="
```

---

## Integration with CI/CD

To automate compliance checking in CI/CD pipelines, add these checks:

```yaml
# .github/workflows/research-compliance.yml
name: Research Compliance Check

on:
  pull_request:
    paths:
      - 'research/literature/**'

jobs:
  validate:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v2
      
      - name: Check file naming
        run: |
          cd research/literature
          for file in *.md; do
            if [[ ! $file =~ ^[a-z0-9-]+\.md$ ]]; then
              echo "::error::File $file does not follow kebab-case naming"
              exit 1
            fi
          done
      
      - name: Check YAML front matter
        run: |
          cd research/literature
          for file in game-dev-analysis-*.md survival-content-extraction-*.md; do
            if [ -f "$file" ]; then
              if ! head -1 "$file" | grep -q "^---$"; then
                echo "::error::File $file missing YAML front matter"
                exit 1
              fi
            fi
          done
      
      - name: Validate assignment file updates
        run: |
          # Check if any assignment files were modified
          git diff --name-only origin/${{ github.base_ref }}...HEAD | grep "research-assignment-group-" || echo "No assignment files updated"
```

---

## Summary

**Key Compliance Requirements:**

1. ✅ Sub-issues must reference `research-assignment-group-{NUMBER}.md`
2. ✅ PRs must process sources from the correct assignment file
3. ✅ Research documents must be in `research/literature/` directory
4. ✅ File names must use kebab-case
5. ✅ Documents must have YAML front matter
6. ✅ Topics must match assignment file
7. ✅ Discovered sources must be logged
8. ✅ Progress tracking must be updated

**Validation Tools:**
- Manual checklists (above)
- Automated scripts (provided)
- CI/CD integration (optional)
- Regular compliance audits

**Non-Compliance Actions:**
1. Comment on issue/PR with specific violations
2. Request corrections before merge/closure
3. Provide links to this validation guide
4. Update issue/PR to meet requirements

---

**Guide Version:** 1.0  
**Last Updated:** 2025-01-17  
**Maintainer:** Research Coordination Team  
**Related Documents:**
- [phase-1-research-note.md](phase-1-research-note.md)
- [copilot-research-instructions.md](copilot-research-instructions.md)
- [research-assignment-groups-overview.md](research-assignment-groups-overview.md)

---

*This validation guide ensures all sub-issues and pull requests properly respect the requirement to process sources from their respective `research-assignment-group-{NUMBER}.md` files. Regular use of these validation tools maintains consistency and quality across Phase 1 research.*
