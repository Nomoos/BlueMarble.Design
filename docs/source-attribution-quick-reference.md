# Source Attribution - Quick Reference

This quick reference provides essential commands and guidelines for tracking and attributing sources properly in BlueMarble research.

## Quick Start

### Run Attribution Check

```bash
cd /home/runner/work/BlueMarble.Design/BlueMarble.Design
python3 scripts/autosources-discovery.py --scan-all --output attribution-report.md
```

### View Report

The report is generated at: `research/literature/attribution-report.md`

Key sections:
- **Source Attribution Report**: Shows compliance rate and missing attributions
- **Discovered Sources**: New sources found in research
- **Processing Queue**: Sources organized by priority

## Required Attribution Format

### Minimum Required (Choose One)

**Option 1: YAML Frontmatter**
```yaml
---
title: Document Title
date: 2025-10-08
source: Source Title by Author Name
---
```

**Option 2: Body Attribution**
```markdown
**Source:** Source Title by Author Name
```

**Option 3: Parent Research**
```yaml
---
parent-research: parent-document.md
---
```

## Common Use Cases

### New Book Analysis

```yaml
---
title: Game Engine Architecture Analysis
date: 2025-10-08
tags: [game-development, architecture, cpp]
status: completed
source: Game Engine Architecture (3rd Edition) by Jason Gregory, CRC Press (2018)
priority: high
---

**Source:** Game Engine Architecture (3rd Edition) by Jason Gregory
**Category:** Game Development
**Priority:** High
```

### Online Resource

```yaml
---
title: Gaffer on Games Networking Analysis
date: 2025-10-08
source: Gaffer on Games - Networked Physics (https://gafferongames.com)
---

**Source:** Gaffer on Games - Networked Physics
**Author:** Glenn Fiedler
**URL:** https://gafferongames.com/post/networked_physics/
```

### Derived Document

```yaml
---
title: ECS Pattern Analysis
date: 2025-10-08
parent-research: game-dev-analysis-game-engine-architecture.md
---
```

## Compliance Standards

- **Target Rate**: 90% or higher
- **Current Rate**: Check latest report
- **Documents Requiring Attribution**: All analysis, extraction, and research documents
- **Documents Exempt**: Summaries, assignment files, meta-documentation

## Fixing Missing Attribution

1. Find document in "Missing Attribution" list
2. Open the document
3. Add `source:` to frontmatter OR `**Source:**` to body
4. Save and re-run check

## Resources

- **Full Guide**: [docs/source-attribution-guide.md](../docs/source-attribution-guide.md)
- **Tool Guide**: [scripts/autosources-discovery-guide.md](../scripts/autosources-discovery-guide.md)
- **Sources README**: [research/sources/README.md](../research/sources/README.md)

## Script Options

```bash
# Scan all documents
python3 scripts/autosources-discovery.py --scan-all

# Scan specific phase
python3 scripts/autosources-discovery.py --phase 2

# Custom output file
python3 scripts/autosources-discovery.py --scan-all --output my-report.md

# JSON format
python3 scripts/autosources-discovery.py --scan-all --format json
```

## Validation Checklist

Before submitting PR:
- [ ] Run attribution checker
- [ ] Review compliance rate (target: >90%)
- [ ] Fix any missing attributions
- [ ] Include attribution report summary in PR

## Getting Help

- Check [docs/source-attribution-guide.md](../docs/source-attribution-guide.md) for detailed examples
- Open an issue if unclear
- See CONTRIBUTING.md for team contact info

---

**Last Updated**: 2025-10-08
**Version**: 1.0
