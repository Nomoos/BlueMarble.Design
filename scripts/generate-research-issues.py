#!/usr/bin/env python3
"""
Issue Generator for Research Assignment Groups

This script generates GitHub issue content for all 20 research assignment groups
plus parent and Phase 2 planning issues. The generated issues can be:
1. Copied and pasted manually into GitHub
2. Used with GitHub CLI: gh issue create --title "..." --body-file issue.md
3. Used with GitHub API for automated issue creation

Usage:
    python3 generate-research-issues.py
    
Output:
    Creates files in /tmp/research-issues/ directory
"""

import json
import os
import re

# Read the assignment group files to extract information
BASE_DIR = "/home/runner/work/BlueMarble.Design/BlueMarble.Design"
GROUPS_DIR = f"{BASE_DIR}/research/literature"
OUTPUT_DIR = "/tmp/research-issues"

# Create output directory
os.makedirs(OUTPUT_DIR, exist_ok=True)

# Group configurations extracted from assignment files
groups_config = [
    {"num": 1, "topics": 1, "priority": "Critical", "effort": "8-12h", "weeks": 2, "title": "Multiplayer Game Programming"},
    {"num": 2, "topics": 1, "priority": "Critical", "effort": "8-12h", "weeks": 2, "title": "Network Programming for Games"},
    {"num": 3, "topics": 2, "priority": "High", "effort": "10-14h", "weeks": 2, "title": "Energy Systems + Historical Maps"},
    {"num": 4, "topics": 2, "priority": "High", "effort": "12-16h", "weeks": 2, "title": "Algorithms + Systems Design"},
    {"num": 5, "topics": 2, "priority": "High", "effort": "12-16h", "weeks": 2, "title": "Advanced Design + Player Decisions"},
    {"num": 6, "topics": 2, "priority": "High", "effort": "12-16h", "weeks": 2, "title": "Fundamentals + Design Process"},
    {"num": 7, "topics": 2, "priority": "High", "effort": "11-15h", "weeks": 2, "title": "Blender + Agile Development"},
    {"num": 8, "topics": 2, "priority": "High", "effort": "12-16h", "weeks": 2, "title": "Prototyping + Engine Architecture"},
    {"num": 9, "topics": 2, "priority": "High", "effort": "11-15h", "weeks": 2, "title": "Real-Time Rendering + 3D Mathematics"},
    {"num": 10, "topics": 2, "priority": "Medium", "effort": "9-14h", "weeks": 2, "title": "Specialized Collections + Design Vocabulary"},
    {"num": 11, "topics": 1, "priority": "Medium", "effort": "4-6h", "weeks": 1, "title": "VFX and Compositing"},
    {"num": 12, "topics": 1, "priority": "Medium", "effort": "4-6h", "weeks": 1, "title": "Interactive Music"},
    {"num": 13, "topics": 1, "priority": "Medium", "effort": "4-6h", "weeks": 1, "title": "3D User Interfaces"},
    {"num": 14, "topics": 1, "priority": "Medium", "effort": "4-6h", "weeks": 1, "title": "C++ Best Practices"},
    {"num": 15, "topics": 1, "priority": "Medium", "effort": "4-6h", "weeks": 1, "title": "Isometric Projection"},
    {"num": 16, "topics": 1, "priority": "Low", "effort": "2-3h", "weeks": 1, "title": "Unity Game Development"},
    {"num": 17, "topics": 1, "priority": "Low", "effort": "2-3h", "weeks": 1, "title": "Unreal Engine VR"},
    {"num": 18, "topics": 1, "priority": "Low", "effort": "3-4h", "weeks": 1, "title": "Augmented Reality"},
    {"num": 19, "topics": 1, "priority": "Very Low", "effort": "2-3h", "weeks": 1, "title": "Roblox Game Development"},
    {"num": 20, "topics": 0, "priority": "Reserved", "effort": "0h", "weeks": 0, "title": "Reserved for Discovered Sources"},
]

def generate_group_issue(group_config):
    """Generate issue content for a specific group"""
    num = group_config["num"]
    
    # Read the actual assignment file to extract topic details
    filename = f"{GROUPS_DIR}/research-assignment-group-{num:02d}.md"
    
    try:
        with open(filename, 'r') as f:
            content = f.read()
            
        # Extract topics from the file (look for ### headers)
        topics = re.findall(r'### \d+\. (.+?) \(', content)
        topic_list = "\n".join([f"{i+1}. {topic}" for i, topic in enumerate(topics)])
        
        if not topics:
            topic_list = f"See assignment file for details: `research/literature/research-assignment-group-{num:02d}.md`"
            
    except FileNotFoundError:
        topic_list = f"See assignment file for details: `research/literature/research-assignment-group-{num:02d}.md`"
    
    # Priority mix
    if group_config["topics"] == 0:
        priority_mix = "Reserved (0 topics initially)"
    elif group_config["topics"] == 1:
        priority_mix = f"1 {group_config['priority']}"
    else:
        priority_mix = f"{group_config['topics']} {group_config['priority']}"
    
    # Generate checkboxes for topics
    topic_checkboxes = "\n".join([f"- [ ] {topic}" for topic in topics]) if topics else "- [ ] See assignment file"
    
    issue_content = f"""# Research Assignment Group {num:02d}

**Labels:** `research`, `assignment-group-{num:02d}`, `priority-{group_config['priority'].lower()}`, `phase-1`

## Assignment Details

**Assignment File:** `research/literature/research-assignment-group-{num:02d}.md`  
**Total Topics:** {group_config['topics']}  
**Priority:** {group_config['priority']}  
**Estimated Effort:** {group_config['effort']}  
**Target Completion:** {group_config['weeks']} week(s)

## Topics to Research

{topic_list}

## Deliverables

For each topic, create a comprehensive analysis document in `research/literature/`:

- Proper YAML front matter
- Minimum length requirements met (see assignment file)
- Code examples where relevant
- Cross-references to related research
- Clear recommendations for BlueMarble

## Progress Checklist

{topic_checkboxes}
- [ ] Discovery logging completed
- [ ] All documents submitted to `research/literature/`
- [ ] Master research queue updated

## Quality Standards

- ✅ Proper YAML front matter
- ✅ Minimum length requirements met
- ✅ Code examples where relevant
- ✅ Cross-references to related research
- ✅ BlueMarble-specific recommendations

## Support Resources

- Assignment file: `/research/literature/research-assignment-group-{num:02d}.md`
- Overview: `/research/literature/research-assignment-groups-overview.md`
- Example: `/research/literature/example-topic.md`
- Guidelines: `/research/literature/README.md`

---

**Related to:** Parent Phase 1 Research Issue  
**Phase:** 1  
**Status:** Ready for Assignment
"""
    
    # Write to file
    output_file = f"{OUTPUT_DIR}/issue-group-{num:02d}.md"
    with open(output_file, 'w') as f:
        f.write(issue_content)
    
    print(f"✓ Generated: {output_file}")
    
    return output_file

def generate_parent_issue():
    """Generate parent issue for Phase 1"""
    issue_content = """# Phase 1 Research: Complete 28 Topics Across 20 Parallel Groups

**Labels:** `research`, `phase-1`, `parent-issue`, `epic`

## Overview

Track Phase 1 research execution across 20 parallel assignment groups.

**Total Topics:** 28 (27 assigned + 1 reserve)  
**Timeline:** 1-2 weeks  
**Total Effort:** 180-250 hours  
**Per Person:** 9-12.5 hours average

## Sub-Issues (20 Assignment Groups)

### Critical Priority (Start Immediately)
- [ ] Group 01: Multiplayer Game Programming (Critical, 8-12h)
- [ ] Group 02: Network Programming for Games (Critical, 8-12h)

### High Priority (Core Systems)
- [ ] Group 03: Energy Systems + Historical Maps (High, 10-14h)
- [ ] Group 04: Algorithms + Systems Design (High, 12-16h)
- [ ] Group 05: Advanced Design + Player Decisions (High, 12-16h)
- [ ] Group 06: Fundamentals + Design Process (High, 12-16h)
- [ ] Group 07: Blender + Agile Development (High, 11-15h)
- [ ] Group 08: Prototyping + Engine Architecture (High, 12-16h)
- [ ] Group 09: Real-Time Rendering + 3D Mathematics (High, 11-15h)

### Medium Priority (Enhancement)
- [ ] Group 10: Specialized Collections + Design Vocabulary (Medium, 9-14h)
- [ ] Group 11: VFX and Compositing (Medium, 4-6h)
- [ ] Group 12: Interactive Music (Medium, 4-6h)
- [ ] Group 13: 3D User Interfaces (Medium, 4-6h)
- [ ] Group 14: C++ Best Practices (Medium, 4-6h)
- [ ] Group 15: Isometric Projection (Medium, 4-6h)

### Low Priority (Specialized)
- [ ] Group 16: Unity Game Development (Low, 2-3h)
- [ ] Group 17: Unreal Engine VR (Low, 2-3h)
- [ ] Group 18: Augmented Reality (Low, 3-4h)

### Very Low Priority
- [ ] Group 19: Roblox Game Development (Very Low, 2-3h)

### Reserve
- [ ] Group 20: Reserved for Discovered Sources

## Success Metrics

- ✅ Critical items complete within 3 days
- ✅ 50% High priority complete within Week 1
- ✅ 80% total completion within Week 2
- ✅ All documents meet quality standards
- ✅ Zero merge conflicts

## Resources

- Overview: `/research/literature/research-assignment-groups-overview.md`
- Assignment Files: `/research/literature/research-assignment-group-01.md` through `group-20.md`
- Master Queue: `/research/literature/master-research-queue.md`

---

**Phase:** 1  
**Status:** Ready to Start
"""
    
    output_file = f"{OUTPUT_DIR}/issue-parent-phase-1.md"
    with open(output_file, 'w') as f:
        f.write(issue_content)
    
    print(f"✓ Generated: {output_file}")
    return output_file

def generate_phase2_planning_issue():
    """Generate Phase 2 planning issue"""
    issue_content = """# Phase 2 Planning: Organize Discovered Sources

**Labels:** `research`, `phase-2`, `planning`, `source-discovery`

## Description

Plan and organize newly discovered research sources from Phase 1 into Phase 2 assignment groups.

## Prerequisites

- ✅ All Phase 1 groups complete
- ✅ Discovered sources logged in assignment files
- ✅ Master research queue updated

## Planning Steps

- [ ] Collect discoveries from all 20 Phase 1 groups
- [ ] Validate and prioritize sources
- [ ] Create discovery statistics
- [ ] Balance and distribute into Phase 2 groups
- [ ] Create Phase 2 assignment files
- [ ] Update master research queue
- [ ] Create Phase 2 sub-issues

## Template

Use: `/research/literature/research-assignment-template-phase-2.md`

## Expected Outcomes

- **Estimated Discoveries:** 20-40 new sources
- **Phase 2 Groups:** 10-20 groups
- **Phase 2 Timeline:** Similar 1-2 week execution

---

**Status:** Blocked (waiting for Phase 1)  
**Phase:** Planning for Phase 2
"""
    
    output_file = f"{OUTPUT_DIR}/issue-phase-2-planning.md"
    with open(output_file, 'w') as f:
        f.write(issue_content)
    
    print(f"✓ Generated: {output_file}")
    return output_file

def generate_readme():
    """Generate README for using the issues"""
    readme_content = """# Research Assignment Issues

This directory contains pre-generated GitHub issue content for the BlueMarble research assignment groups.

## Files

- `issue-parent-phase-1.md` - Parent issue tracking all Phase 1 work
- `issue-group-01.md` through `issue-group-20.md` - Individual group issues
- `issue-phase-2-planning.md` - Phase 2 planning issue

## Usage

### Option 1: Manual Creation (Copy/Paste)

1. Go to GitHub Issues: https://github.com/Nomoos/BlueMarble.Design/issues/new
2. Copy content from `issue-parent-phase-1.md`
3. Paste into issue body
4. Add labels as indicated
5. Create issue
6. Note the issue number
7. Repeat for each group issue, linking to parent

### Option 2: GitHub CLI

```bash
# Create parent issue
gh issue create --title "Phase 1 Research: Complete 28 Topics Across 20 Parallel Groups" \\
  --body-file issue-parent-phase-1.md \\
  --label "research,phase-1,parent-issue,epic"

# Create group issues (repeat for 01-20)
gh issue create --title "Research Assignment Group 01: Multiplayer Game Programming" \\
  --body-file issue-group-01.md \\
  --label "research,assignment-group-01,priority-critical,phase-1"

# Create Phase 2 planning issue
gh issue create --title "Phase 2 Planning: Organize Discovered Sources" \\
  --body-file issue-phase-2-planning.md \\
  --label "research,phase-2,planning,source-discovery"
```

### Option 3: GitHub API (Automated)

See `create-issues-api.sh` script for automated creation using GitHub API.

## Issue Hierarchy

```
Phase 1 Research (Parent) #XXX
├── Group 01 #XXX
├── Group 02 #XXX
├── ...
├── Group 19 #XXX
└── Group 20 #XXX

Phase 2 Planning #XXX (created after Phase 1 completes)
```

## Workflow

1. Create parent Phase 1 issue first
2. Create all 20 group issues, referencing parent issue number
3. Assign each group issue to a team member
4. Track progress as groups complete
5. After Phase 1 complete, create Phase 2 planning issue
6. After Phase 2 planned, create Phase 2 parent and group issues

## Labels to Create

Make sure these labels exist in your repository:
- `research`
- `phase-1`
- `phase-2`
- `parent-issue`
- `epic`
- `planning`
- `source-discovery`
- `priority-critical`
- `priority-high`
- `priority-medium`
- `priority-low`
- `assignment-group-01` through `assignment-group-20`

## Assignees

Update each issue with appropriate assignee after creation.
"""
    
    output_file = f"{OUTPUT_DIR}/README.md"
    with open(output_file, 'w') as f:
        f.write(readme_content)
    
    print(f"✓ Generated: {output_file}")
    return output_file

# Main execution
if __name__ == "__main__":
    print("=" * 60)
    print("Generating Research Assignment Issues")
    print("=" * 60)
    print()
    
    # Generate all issues
    print("Generating parent issue...")
    generate_parent_issue()
    print()
    
    print("Generating 20 group issues...")
    for group in groups_config:
        generate_group_issue(group)
    print()
    
    print("Generating Phase 2 planning issue...")
    generate_phase2_planning_issue()
    print()
    
    print("Generating README...")
    generate_readme()
    print()
    
    print("=" * 60)
    print(f"✓ All issues generated in: {OUTPUT_DIR}")
    print("=" * 60)
    print()
    print("Next steps:")
    print(f"  1. Review files in {OUTPUT_DIR}")
    print("  2. Use manual copy/paste or GitHub CLI to create issues")
    print("  3. See README.md in output directory for detailed instructions")
