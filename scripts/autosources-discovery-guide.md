# Automated Source Discovery - Usage Guide

---
title: Automated Source Discovery Usage Guide
date: 2025-01-17
tags: [automation, source-discovery, tools, workflow]
status: active
---

## Overview

The **Automated Source Discovery Tool** (`autosources-discovery.py`) automatically scans existing research documents to discover new research sources from citations, references, and "future research" sections. This tool helps maintain a continuous research pipeline by systematically identifying valuable sources mentioned across the research corpus.

## Features

- **Automatic Scanning**: Scans all research documents for source references
- **Pattern Recognition**: Identifies citations, URLs, ISBNs, and explicit source mentions
- **Priority Classification**: Automatically infers priority levels (critical, high, medium, low)
- **Category Detection**: Classifies sources into categories (gamedev-tech, gamedev-design, etc.)
- **Effort Estimation**: Estimates research effort based on content type
- **Multiple Formats**: Outputs to Markdown, JSON, or YAML
- **Deduplication**: Tracks sources mentioned in multiple documents

## Installation

The tool is located at: `scripts/autosources-discovery.py`

**Requirements:**
- Python 3.7+
- PyYAML (for YAML frontmatter parsing)

**Install dependencies:**
```bash
pip install pyyaml
```

## Usage

### Basic Usage

**Scan all research documents:**
```bash
python scripts/autosources-discovery.py --scan-all
```

**Scan specific phase:**
```bash
python scripts/autosources-discovery.py --phase 3
```

**Generate JSON output:**
```bash
python scripts/autosources-discovery.py --scan-all --format json
```

### Command-Line Options

```
--scan-all              Scan all existing research documents
--phase N               Focus on Phase N documents only  
--priority LEVEL        Filter by priority (critical, high, medium, low)
--category CAT          Filter by category
--output FILE           Output file name (default: auto-discovered-sources.md)
--format FORMAT         Output format: markdown, json, yaml (default: markdown)
```

### Examples

**Example 1: Discover all sources and generate markdown report**
```bash
python scripts/autosources-discovery.py --scan-all --output auto-discovered-sources.md
```

**Example 2: Find critical priority sources from Phase 3**
```bash
python scripts/autosources-discovery.py --phase 3 --priority critical
```

**Example 3: Export to JSON for programmatic processing**
```bash
python scripts/autosources-discovery.py --scan-all --format json --output discovered.json
```

## Output Format

### Markdown Report Structure

The generated markdown report includes:

1. **Executive Summary**
   - Total sources discovered
   - Priority breakdown
   - Category breakdown

2. **Sources by Priority**
   - Critical sources (process first)
   - High priority sources
   - Medium priority sources
   - Low priority sources

3. **For Each Source:**
   - Title and description
   - Priority and category
   - Estimated research effort
   - List of documents that reference this source

4. **Processing Queue**
   - Organized checklist by priority
   - Ready for assignment to research groups

5. **Statistics**
   - Total effort estimates
   - Category distribution
   - Priority distribution

### JSON Output

JSON format provides structured data for programmatic use:
```json
{
  "generated": "2025-01-17T12:00:00",
  "total_sources": 20,
  "sources": [
    {
      "title": "Source Title",
      "description": "Description text",
      "priority": "high",
      "category": "gamedev-tech",
      "estimated_effort": "4-6 hours",
      "references": ["doc1.md", "doc2.md"],
      "discovered_date": "2025-01-17T12:00:00",
      "status": "discovered"
    }
  ],
  "categories": ["gamedev-tech", "gamedev-design"],
  "priorities": ["critical", "high", "medium"]
}
```

## Integration with Research Workflow

### Step 1: Run Discovery Tool

Run the tool periodically (e.g., after completing each research phase):
```bash
python scripts/autosources-discovery.py --scan-all
```

### Step 2: Review Discovered Sources

Review the generated `auto-discovered-sources.md` file:
- Validate source relevance
- Check source accessibility
- Adjust priorities if needed
- Add additional context

### Step 3: Integrate into Phase Planning

Add discovered sources to phase planning documents:
- Create new assignment groups for high-priority sources
- Add to existing groups where appropriate
- Reserve lower-priority sources for future phases

### Step 4: Track Progress

As sources are processed:
- Check off items in the processing queue
- Update source status in the discovery report
- Re-run discovery tool to find new sources

## Pattern Recognition

The tool recognizes several source reference patterns:

### Citation Patterns
- `**Title:** Source Title`
- `**Author:** Author Name`
- `**Publisher:** Publisher Name`
- `ISBN: 978-XXXXXXXXXX`
- `URL: https://example.com`

### Discovery Patterns
- `Discovered From: Original Source`
- `Referenced in: Document Name`

### Future Research Patterns
- `Future research: Source Title`
- `Additional sources: Source Name`
- `Recommended reading: Book Title`
- `See also: Related Work`

### Section Patterns
The tool automatically extracts sources from sections titled:
- "Discovered Sources"
- "Next Sources"
- "Future Research"
- "Additional Sources"

## Customization

### Adding New Patterns

Edit `scripts/autosources-discovery.py` and add patterns to the `patterns` list in the `scan_research_documents` method:

```python
patterns = [
    # Add your custom pattern here
    r'Your pattern:\s*(.+)',
]
```

### Adjusting Priority Inference

Modify the `_infer_priority` method to adjust priority classification:

```python
def _infer_priority(self, text: str) -> str:
    text_lower = text.lower()
    # Add custom priority keywords
    if 'your_keyword' in text_lower:
        return 'critical'
```

### Adding Categories

Extend the `_infer_category` method to add new categories:

```python
categories = {
    'your-category': ['keyword1', 'keyword2'],
}
```

## Best Practices

1. **Run Regularly**: Execute discovery tool after completing each major phase
2. **Review Thoroughly**: Manually review discovered sources before adding to research queue
3. **Maintain Quality**: Not all discovered sources may be relevant - filter appropriately
4. **Track Sources**: Use the generated processing queue to track completion
5. **Update Patterns**: Add new pattern recognitions as you discover common citation formats
6. **Version Control**: Commit both the tool and generated discovery reports

## Workflow Integration

### End of Phase Cycle

When completing a research phase:

1. **Run Discovery**:
   ```bash
   python scripts/autosources-discovery.py --phase 3 --output phase-3-auto-discovered.md
   ```

2. **Review and Prioritize**:
   - Review discovered sources
   - Validate accessibility
   - Adjust priorities

3. **Create Next Phase**:
   - Use high-priority discovered sources for next phase planning
   - Create assignment groups from discovery report
   - Add to phase planning documents

4. **Archive Discovery Report**:
   - Rename with phase number: `phase-3-discovered-sources-final.md`
   - Commit to version control

### Continuous Discovery

For ongoing research:

1. **Weekly/Monthly Runs**: Execute discovery tool on schedule
2. **Aggregate Results**: Combine with manual source curation
3. **Maintain Queue**: Keep processing queue updated
4. **Track Metrics**: Monitor discovery rates and source quality

## Troubleshooting

### No Sources Discovered

**Issue**: Tool finds 0 sources

**Solutions**:
- Check that research documents contain source references
- Verify pattern matching is working
- Add more specific patterns for your citation style
- Check file paths and permissions

### Incorrect Priority/Category

**Issue**: Sources classified incorrectly

**Solutions**:
- Adjust keyword lists in `_infer_priority` and `_infer_category`
- Add more specific keywords
- Manually override in generated report

### Duplicate Sources

**Issue**: Same source appears multiple times

**Solutions**:
- Tool has built-in deduplication by title
- Check for case sensitivity issues
- Verify source titles are consistent across documents

## Future Enhancements

Potential improvements for the tool:

- [ ] Integration with academic databases (Google Scholar, arXiv)
- [ ] Automatic ISBN/DOI validation
- [ ] Link checking and availability validation
- [ ] Machine learning-based classification
- [ ] Integration with citation management tools (BibTeX, Zotero)
- [ ] Automatic assignment group creation
- [ ] Web scraping for additional source metadata
- [ ] Duplicate detection across different naming conventions

## Support

For issues or feature requests:
1. Check this documentation first
2. Review existing discovery reports for examples
3. Examine the script comments and docstrings
4. Test with `--help` flag for command-line options

## Version History

- **v1.0** (2025-01-17): Initial release
  - Pattern-based source extraction
  - Markdown and JSON output formats
  - Priority and category classification
  - Deduplication support

---

**Tool Location:** `scripts/autosources-discovery.py`  
**Documentation:** `scripts/autosources-discovery-guide.md`  
**Example Output:** `research/literature/auto-discovered-sources.md`  
**Status:** Production Ready
