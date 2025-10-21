# Wikipedia Source Processing

## Overview

This document describes how to process Wikipedia pages and add them as sources to the BlueMarble research bibliography.

## Script: process-wiki-sources.py

### Purpose
Automatically processes Wikipedia URLs and generates BibTeX entries formatted for inclusion in `research/sources/sources.bib`.

### Features
- Extracts page titles from Wikipedia URLs (supports all language versions)
- Generates standardized BibTeX keys (e.g., `wiki_heat`, `wiki_hydrological_model`)
- Creates BlueMarble-specific descriptions for each source
- Handles URL encoding and special characters
- Supports both English and non-English Wikipedia pages

### Usage

#### Process Default URLs
```bash
python3 scripts/process-wiki-sources.py
```

#### Process Custom URLs
```bash
python3 scripts/process-wiki-sources.py "https://en.wikipedia.org/wiki/Page1" "https://en.wikipedia.org/wiki/Page2"
```

### Output Format

The script generates BibTeX entries in the following format:

```bibtex
@misc{wiki_<key>,
  title = {<Title>},
  author = {{Wikipedia contributors}},
  year = {<current_year>},
  url = {<url>},
  note = {<BlueMarble-specific description>}
}
```

### Example

**Input:**
```
https://en.wikipedia.org/wiki/Hydrological_model
```

**Output:**
```bibtex
@misc{wiki_hydrological_model,
  title = {Hydrological model},
  author = {{Wikipedia contributors}},
  year = {2025},
  url = {https://en.wikipedia.org/wiki/Hydrological_model},
  note = {Hydrological modeling for water system simulation and environmental mechanics}
}
```

## Recent Additions

### 2025-01-17 - Assignment Group Processing

Processed 5 Wikipedia pages as requested:

1. **Hydrological model** - Water system simulation and environmental mechanics
2. **Fick's laws of diffusion** - Material transport and gas exchange
3. **Heat** - Temperature simulation and energy systems
4. **Wikipedia:Categorization** - Knowledge organization systems
5. **Biologická systematika** (Czech) - Biological classification systems

**Total Wikipedia sources:** 26 entries  
**Location:** `research/sources/sources.bib` lines 719-759

## Adding New Sources

### Manual Process

1. Run the script with your URLs
2. Review the generated BibTeX entries
3. Copy the entries to `research/sources/sources.bib`
4. Place entries in appropriate category section
5. Ensure proper formatting and indentation
6. Commit changes with descriptive message

### Automatic Integration (Future Enhancement)

Consider extending the script to:
- Automatically append to sources.bib
- Check for duplicate entries
- Validate BibTeX syntax
- Organize entries by category

## Maintenance Notes

- Keep descriptions relevant to BlueMarble game development context
- Use consistent formatting across all wiki entries
- Update year field when adding new sources
- Group related entries with comment headers (e.g., `% Environmental Systems`)

## Related Files

- `research/sources/sources.bib` - Main bibliography file
- `scripts/autosources-discovery.py` - Automatic source discovery from research documents
- `research/sources/SOURCE-SUMMARY.md` - Overview of research sources
