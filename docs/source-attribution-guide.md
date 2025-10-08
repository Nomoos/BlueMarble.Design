# Source Attribution Guide

---
title: Source Attribution Guide for BlueMarble Research
date: 2025-10-08
tags: [documentation, sources, attribution, best-practices]
status: active
---

**Document Type:** Research Best Practices Guide  
**Version:** 1.0  
**Author:** Game Design Research Team  
**Last Updated:** 2025-10-08

---

## Overview

This guide establishes standards for properly attributing sources in BlueMarble research documents to ensure academic integrity, legal compliance, and effective knowledge tracking throughout the research process.

**Why Source Attribution Matters:**
- **Academic Integrity**: Proper credit to original authors and researchers
- **Legal Compliance**: Respect for intellectual property and licensing
- **Knowledge Tracking**: Ability to trace ideas back to their sources
- **Quality Assurance**: Verification and validation of research claims
- **Team Collaboration**: Clear understanding of what information came from where

---

## Attribution Requirements

### 1. YAML Frontmatter (Required)

Every research document MUST include a YAML frontmatter block at the top with source information:

```yaml
---
title: Document Title
date: YYYY-MM-DD
tags: [relevant, tags, here]
status: completed|in-progress|draft
source: Primary Source Name and Reference
parent-research: parent-document.md (if derived from another research doc)
---
```

**Required Fields:**
- `title`: Document title
- `date`: Document creation or last update date
- `tags`: Relevant categorization tags
- `status`: Current status of the document
- `source`: Primary source being analyzed (see formats below)

**Optional but Recommended:**
- `parent-research`: Link to parent research document if this is a derived analysis
- `priority`: Priority level (critical, high, medium, low)
- `phase`: Research phase number if applicable

### 2. Source Body Attribution (Recommended)

Include a **Source:** line near the top of the document body for clarity:

```markdown
**Source:** Full Title by Author Name  
**Category:** Category Name  
**Priority:** Priority Level  
**Status:** Status  
```

---

## Source Attribution Formats

### For Books

**Frontmatter:**
```yaml
source: Game Engine Architecture (3rd Edition) by Jason Gregory, CRC Press (2018)
```

**Body:**
```markdown
**Source:** Game Engine Architecture (3rd Edition) by Jason Gregory  
**Publisher:** CRC Press  
**Year:** 2018  
**ISBN:** 978-1-138-03545-4  
```

### For Online Resources

**Frontmatter:**
```yaml
source: Gaffer on Games - Networked Physics (https://gafferongames.com)
```

**Body:**
```markdown
**Source:** Gaffer on Games - Networked Physics  
**Author:** Glenn Fiedler  
**URL:** https://gafferongames.com/post/networked_physics/  
**Accessed:** 2025-01-15  
```

### For Open Source Projects

**Frontmatter:**
```yaml
source: TrinityCore - World of Warcraft Server Emulator (GitHub)
```

**Body:**
```markdown
**Source:** TrinityCore - World of Warcraft Server Emulator  
**Repository:** https://github.com/TrinityCore/TrinityCore  
**License:** GPL-2.0  
**Last Reviewed:** 2025-01-15  
```

### For Academic Papers

**Frontmatter:**
```yaml
source: "Application of Isotope Hydrology Techniques" by T. R. Holmes (PhD Thesis, 2015)
```

**Body:**
```markdown
**Source:** Application of Isotope Hydrology Techniques to Groundwater Systems  
**Author:** T. R. Holmes  
**Type:** PhD Thesis  
**Institution:** University Example  
**Year:** 2015  
**URL:** https://repository.example.edu/thesis/12345  
```

### For Survival/Technical Collections

**Frontmatter:**
```yaml
source: Appropriate Technology Library (1,050 ebooks collection)
```

**Body:**
```markdown
**Source:** Appropriate Technology Library  
**Collection Size:** 1,050 ebooks  
**Focus:** Sustainable living, low-technology solutions  
**Magnet Link:** magnet:?xt=urn:btih:...  
**License:** Various (verify individual book licenses)  
```

### For Derived Research

When creating analysis based on another research document:

**Frontmatter:**
```yaml
source: Multiple sources (see parent research)
parent-research: game-dev-analysis-multiplayer-programming.md
```

**Body:**
```markdown
**Source:** Derived from parent research analysis  
**Parent Research:** game-dev-analysis-multiplayer-programming.md  
**Focus:** Specific subsystem extracted from parent analysis  
```

---

## Citation Best Practices

### 1. In-Text Citations

When referencing specific ideas or quotes:

```markdown
According to Gregory's *Game Engine Architecture* [1], the ECS pattern provides...

> "Entity-Component-System architecture separates data from behavior"
> — Jason Gregory, Game Engine Architecture (3rd Ed.), p. 1042
```

### 2. Reference Sections

Include a **References** section at the end of detailed analyses:

```markdown
## References

1. Gregory, Jason. *Game Engine Architecture* (3rd Edition). CRC Press, 2018.
2. Glazer, Joshua & Madhav, Sanjay. *Multiplayer Game Programming*. Addison-Wesley, 2015.
3. Fiedler, Glenn. "Networked Physics." *Gaffer on Games*. https://gafferongames.com/post/networked_physics/
```

### 3. Cross-References

Link to related research documents:

```markdown
**Related Research:**
- [Game Engine Architecture Analysis](game-dev-analysis-02-game-engine-architecture.md)
- [Multiplayer Networking Patterns](game-dev-analysis-03-multiplayer-programming.md)
- [ECS Implementation Guide](../topics/entity-component-system.md)
```

---

## Discovered Sources

When discovering new sources during research, document them properly:

### In Discovered Sources Section

```markdown
## Discovered Sources

### High Priority

- **Reliable UDP Networking Libraries**: ENet, LiteNetLib, and Lidgren offer production-ready 
  solutions for game networking. Discovered during multiplayer architecture research.
  - **Priority:** High
  - **Category:** Networking
  - **Estimated Effort:** 6-8 hours
  - **References:** ENet documentation, LiteNetLib GitHub, Lidgren repository

### Medium Priority

- **GPU Gems 3 - Procedural Terrain Generation**: Chapter on real-time terrain synthesis.
  - **Priority:** Medium
  - **Category:** Graphics Programming
  - **Estimated Effort:** 4-6 hours
```

### Logging Discoveries

The `autosources-discovery.py` script automatically scans for discovered sources in research documents. Use the standard format above to ensure proper detection.

---

## Attribution Compliance Checking

### Using the Automated Tool

Run the source discovery and attribution checker:

```bash
cd /home/runner/work/BlueMarble.Design/BlueMarble.Design

# Scan all documents and generate report
python3 scripts/autosources-discovery.py --scan-all --output attribution-report.md

# Scan specific phase
python3 scripts/autosources-discovery.py --phase 2 --output phase-2-attribution.md
```

### Understanding the Report

The generated report includes:

1. **Discovered Sources**: New sources found in research documents
2. **Source Attribution Report**: 
   - Documents scanned count
   - Properly attributed documents
   - Missing attribution list
   - Compliance rate
3. **Action Items**: Documents requiring attribution fixes

### Compliance Standards

**Minimum Compliance Rate:** 90%

Documents requiring attribution:
- All `game-dev-analysis-*.md` files
- All `survival-content-extraction-*.md` files
- All `discovered-source-*.md` files
- Individual research analyses

Documents that may skip attribution:
- Summary documents (`*-summary.md`)
- Assignment group files (`research-assignment-group-*.md`)
- Meta-documentation (`README.md`, guides)

---

## Common Attribution Mistakes

### ❌ Mistake 1: No Frontmatter

```markdown
# My Research Document

This document analyzes...
```

**Fix:** Add YAML frontmatter with source field.

### ❌ Mistake 2: Incomplete Source Information

```yaml
---
source: Game Engine Book
---
```

**Fix:** Include full title, author, and edition:
```yaml
source: Game Engine Architecture (3rd Edition) by Jason Gregory
```

### ❌ Mistake 3: Missing Date

```yaml
---
title: My Analysis
source: Some Book
---
```

**Fix:** Always include the date:
```yaml
---
title: My Analysis
date: 2025-10-08
source: Some Book by Author Name
---
```

### ❌ Mistake 4: Uncited Quotes or Ideas

```markdown
The best approach is to use ECS architecture because it's cache-friendly.
```

**Fix:** Add citation:
```markdown
The best approach is to use ECS architecture because it's cache-friendly, improving 
iteration performance by 2-10x compared to traditional OOP [Gregory, GEA 3rd Ed., p. 1045].
```

---

## License and Copyright Considerations

### Open Source Projects

When referencing open source projects:
- Include license type (GPL, MIT, Apache, etc.)
- Link to project repository
- Note any usage restrictions
- Attribute authors/maintainers

### Commercial Books

When analyzing commercial books:
- Fair use applies to analysis and commentary
- Don't reproduce large sections of copyrighted text
- Use brief quotes with proper attribution
- Link to purchase information when appropriate

### Academic Resources

When using academic papers:
- Check if open access or behind paywall
- Include DOI or repository link
- Follow academic citation standards
- Note institutional affiliation if relevant

### Survival Collections

Many survival collections have mixed licenses:
- Verify license for each individual source
- Note if public domain, creative commons, or other
- Include attribution as required by license
- Document any usage restrictions

---

## Attribution Workflow

### When Starting New Research

1. **Create Document**: Start with proper YAML frontmatter
2. **Document Source**: Fill in source field immediately
3. **Add Body Attribution**: Include **Source:** line at top
4. **Research**: Conduct analysis with citations
5. **Discovered Sources**: Note any new sources found
6. **References**: Add references section if needed
7. **Cross-link**: Link to related documents

### When Updating Existing Research

1. **Check Attribution**: Verify source information is complete
2. **Add Missing Info**: Fill in any missing attribution fields
3. **Update Date**: Change date field to reflect update
4. **Verify Citations**: Ensure all quotes/ideas are cited
5. **Run Checker**: Use `autosources-discovery.py` to validate

### Before Submitting PR

1. Run attribution compliance check:
   ```bash
   python3 scripts/autosources-discovery.py --scan-all
   ```
2. Review attribution report
3. Fix any missing attributions
4. Achieve >90% compliance rate
5. Include attribution report in PR description

---

## Integration with Existing Systems

### research/sources/sources.bib

When analyzing a source:
1. Add entry to `sources.bib` in BibTeX format
2. Reference the BibTeX key in your analysis
3. Keep sources.bib synchronized with analyses

### research/sources/reading-list.md

When completing analysis:
1. Mark source as `[x]` completed in reading list
2. Add link to analysis document
3. Update status and completion date

### Master Research Queue

Update tracking in `research/literature/master-research-queue.md`:
1. Mark source as completed
2. Link to analysis document
3. Note any discovered sources
4. Update phase/group completion status

---

## Examples

### Example 1: Game Development Book Analysis

```markdown
---
title: Game Programming Patterns Analysis for BlueMarble
date: 2025-01-15
tags: [game-development, design-patterns, architecture]
status: completed
source: Game Programming Patterns by Robert Nystrom (2014)
priority: high
phase: 1
---

**Source:** Game Programming Patterns by Robert Nystrom  
**Category:** Game Development - Design Patterns  
**Priority:** High  
**Status:** ✅ Complete  
**Related Sources:** Game Engine Architecture, Clean Code, Design Patterns (GoF)

## Executive Summary

This analysis extracts design pattern applications from Nystrom's *Game Programming 
Patterns* [1] specifically for BlueMarble's MMORPG architecture...

[Rest of analysis with proper citations]

## References

1. Nystrom, Robert. *Game Programming Patterns*. Genever Benning, 2014. 
   Available at: https://gameprogrammingpatterns.com/
```

### Example 2: Open Source Project Analysis

```markdown
---
title: TrinityCore Server Architecture Analysis
date: 2025-01-18
tags: [open-source, server-architecture, mmorpg, wow]
status: completed
source: TrinityCore - World of Warcraft Server Emulator (GitHub)
priority: high
---

**Source:** TrinityCore - World of Warcraft Server Emulator  
**Repository:** https://github.com/TrinityCore/TrinityCore  
**License:** GPL-2.0  
**Category:** Open Source MMORPG Server  
**Last Reviewed:** 2025-01-18

## Executive Summary

TrinityCore is a GPL-2.0 licensed open source implementation of a World of Warcraft 
server emulator. This analysis examines architecture patterns applicable to BlueMarble...

[Analysis continues]

## License Compliance

TrinityCore is licensed under GPL-2.0. This analysis is for educational and research 
purposes only. BlueMarble will implement similar patterns using original code, not 
derived works from TrinityCore.
```

---

## Troubleshooting

### Issue: Attribution Report Shows Low Compliance

**Solution:**
1. Run: `python3 scripts/autosources-discovery.py --scan-all`
2. Review "Documents Missing Source Attribution" section
3. Add source field to frontmatter of listed documents
4. Re-run to verify improvement

### Issue: Can't Find Original Source

**Solution:**
1. Check parent-research field for derived documents
2. Review research/sources/sources.bib
3. Check research/sources/reading-list.md
4. Ask in team chat or GitHub issue

### Issue: Multiple Sources in Single Document

**Solution:**
1. Use primary source in `source:` field
2. List additional sources in References section
3. Consider splitting into multiple documents if appropriate

---

## Additional Resources

- **Bibliography Guide**: `research/sources/README.md`
- **Research Compliance**: `research/literature/research-compliance-validation-guide.md`
- **Source Discovery Tool**: `scripts/autosources-discovery.py`
- **Source Discovery Guide**: `scripts/autosources-discovery-guide.md`

---

**Version:** 1.0  
**Last Updated:** 2025-10-08  
**Maintained By:** Game Design Research Team  
**Questions?** Open an issue or check CONTRIBUTING.md
