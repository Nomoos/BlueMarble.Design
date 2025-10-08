# Sample Research Document - Proper Attribution Example

---
title: Sample Research Document - Proper Attribution Example
date: 2025-10-08
tags: [example, template, attribution, documentation]
status: example
source: "This is an example document demonstrating proper source attribution"
parent-research: N/A
priority: medium
---

**Source:** Example Source: Proper Attribution in Research Documents  
**Category:** Documentation - Best Practices  
**Priority:** Medium  
**Status:** ✅ Example  
**Purpose:** Demonstrate correct source attribution format

---

## Overview

This is a **template example** showing the correct way to attribute sources in BlueMarble research documents. Use this as a reference when creating new research analyses.

---

## Why This Document Has Proper Attribution

This document demonstrates proper attribution in THREE ways:

### 1. YAML Frontmatter (Line 3-10)

```yaml
---
title: Sample Research Document - Proper Attribution Example
date: 2025-10-08
tags: [example, template, attribution, documentation]
status: example
source: "This is an example document demonstrating proper source attribution"
parent-research: N/A
priority: medium
---
```

**Key fields:**
- `source:` - Primary source being analyzed (REQUIRED)
- `parent-research:` - Parent document if derived (OPTIONAL)
- `date:` - Document creation/update date (REQUIRED)
- `title:` - Document title (REQUIRED)

### 2. Body Attribution (Line 12-16)

```markdown
**Source:** Example Source: Proper Attribution in Research Documents  
**Category:** Documentation - Best Practices  
**Priority:** Medium  
**Status:** ✅ Example  
```

This provides quick reference for readers and helps with source tracking.

### 3. References Section (see below)

Detailed citations of all sources referenced in the document.

---

## Example Citations

When referencing ideas from sources, always cite them:

### Direct Quote

> "Proper source attribution is essential for academic integrity and knowledge tracking."
> — Source Attribution Guide, docs/source-attribution-guide.md, Section 2.1

### Paraphrased Idea

According to the Source Attribution Guide [1], research documents should include attribution in both frontmatter and body for maximum clarity.

### Technical Reference

The autosources-discovery.py script [2] automatically validates source attribution across all research documents, tracking three types of attribution: frontmatter source, body source, and parent research links.

---

## Related Research

Cross-reference related documents:

- [Source Attribution Guide](../docs/source-attribution-guide.md) - Complete attribution standards
- [Source Attribution Quick Reference](../docs/source-attribution-quick-reference.md) - Quick commands
- [Autosources Discovery Guide](../scripts/autosources-discovery-guide.md) - Tool documentation

---

## Discovered Sources

When discovering new sources during research, document them:

### High Priority

- **Game Engine Architecture Patterns**: Advanced ECS implementation patterns discovered in Chapter 15. 
  Recommended for immediate review.
  - **Priority:** High
  - **Category:** GameDev-Tech
  - **Estimated Effort:** 8-10 hours
  - **Reference:** Game Engine Architecture (3rd Ed.), Chapter 15

### Medium Priority

- **Multiplayer Synchronization Strategies**: State replication patterns for large-scale games.
  - **Priority:** Medium
  - **Category:** Networking
  - **Estimated Effort:** 6-8 hours

---

## References

1. BlueMarble Design Team. "Source Attribution Guide." docs/source-attribution-guide.md. 
   BlueMarble.Design repository, 2025.

2. BlueMarble Design Team. "Automated Source Discovery Tool." scripts/autosources-discovery.py. 
   BlueMarble.Design repository, 2025.

3. (Add your actual source references here following standard citation format)

---

## Implementation Notes

**For Developers Using This Template:**

1. Replace all example content with your actual research
2. Update the `source:` field in frontmatter with your actual source
3. Update the **Source:** line in the body
4. Add your actual citations in the References section
5. Keep cross-references to related BlueMarble documents
6. Document any new sources you discover
7. Run `python3 scripts/autosources-discovery.py --scan-all` to verify

**Quick Check:**
- [ ] YAML frontmatter with `source:` field
- [ ] **Source:** line in document body
- [ ] All quotes and ideas properly cited
- [ ] References section complete
- [ ] Related research cross-referenced
- [ ] Discovered sources documented

---

**Document Type:** Example Template  
**Version:** 1.0  
**Created:** 2025-10-08  
**Usage:** Copy this structure for new research documents  
**Validation:** Run `python3 scripts/autosources-discovery.py --scan-all` to verify attribution

---

**✅ This document demonstrates proper attribution and will pass compliance checks.**
