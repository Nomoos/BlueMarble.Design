# Scripts

This directory contains utility scripts for maintaining documentation quality in the BlueMarble.Design repository.

**Quick Links:**
- [Usage Examples](USAGE_EXAMPLES.md) - Platform-specific examples for all scripts
- [Platform Compatibility](#platform-compatibility) - OS-specific requirements

## Available Scripts

### reddit-story-scraper.py

**Reddit Story Mining and Collection Tool** - Mines player experiences, design insights, and community feedback from Reddit to support BlueMarble MMORPG research.

**Quick Start:**
```bash
# Basic collection from r/MMORPG
python3 scripts/reddit-story-scraper.py

# Test mode (no network required)
python3 scripts/reddit-story-scraper.py --test-mode --limit 10
```

**Features:**
- Collect stories and discussions from any public subreddit
- Filter by keywords, score, and timeframe
- Multiple sorting options (hot, new, top, rising)
- Automatic post categorization (game design, player experience, feedback, etc.)
- Optional comment collection for deeper insights
- Output in JSON or Markdown format
- Test mode for offline development and testing

**Usage Examples:**
```bash
# Collect top posts with specific keywords
python3 scripts/reddit-story-scraper.py --keywords "economy,crafting,trading"

# Get recent discussions with comments
python3 scripts/reddit-story-scraper.py --sort hot --timeframe day --include-comments

# Markdown report for manual analysis
python3 scripts/reddit-story-scraper.py --format markdown --output research/reddit-insights.md

# Multiple subreddits (run separately)
python3 scripts/reddit-story-scraper.py --subreddit gamedesign --limit 50
python3 scripts/reddit-story-scraper.py --subreddit truegaming --limit 50
```

**Output:** Auto-generated filename or custom path

**Requirements:**
- Python 3.7+
- requests: `pip install requests` (included)

**Documentation:** See `REDDIT_SCRAPER_README.md` for complete usage guide

**Use Case:** Automate collection of player experiences and design insights from Reddit for research analysis

---

### autosources-discovery.py

**Automated Source Discovery Tool** - Automatically discovers research sources from existing research documents by scanning for citations, references, and "future research" sections.

**Quick Start:**
```bash
python3 scripts/autosources-discovery.py --scan-all
```

**Features:**
- Scans all research documents for source references
- Automatically classifies by priority and category
- Generates processing queues
- Outputs to Markdown or JSON
- Tracks sources across multiple documents
- Deduplicates sources mentioned in multiple places

**Usage Examples:**
```bash
# Scan all documents
python3 scripts/autosources-discovery.py --scan-all

# Scan specific phase
python3 scripts/autosources-discovery.py --phase 3

# Generate JSON output
python3 scripts/autosources-discovery.py --scan-all --format json

# Custom output file
python3 scripts/autosources-discovery.py --scan-all --output my-discoveries.md
```

**Output:** `research/literature/auto-discovered-sources.md`

**Requirements:**
- Python 3.7+
- PyYAML: `pip install pyyaml`

**Documentation:** See `autosources-discovery-guide.md` for complete usage guide

**Use Case:** Run at the end of each research phase to discover new sources for the next phase

---

### generate-research-issues.py

Generates GitHub issue content for all 40 research assignment groups plus parent and Phase 2 planning issues.

**Usage:**

```bash
python3 scripts/generate-research-issues.py
```

**Output:**

Creates issue files in `/tmp/research-issues/` directory:
- `issue-parent-phase-1.md` - Parent issue for Phase 1
- `issue-group-01.md` through `issue-group-40.md` - Individual group issues (40 total)
- `issue-phase-2-planning.md` - Phase 2 planning issue
- `README.md` - Instructions for creating issues

**Creating Issues:**

Option 1: PowerShell script (Windows - recommended for automation)
Option 2: Bash script (Linux/macOS/Git Bash)
Option 3: Manual (copy/paste into GitHub)
Option 4: GitHub CLI (manual command for each issue)

See generated README.md for detailed instructions.

---

### create-research-issues.ps1

**PowerShell script for Windows users** to automate creation of all 42 research issues (1 parent + 40 groups + 1 phase 2).

**Usage:**

```powershell
# Generate issue files first
python3 scripts/generate-research-issues.py

# Run PowerShell script (from scripts directory)
cd scripts
.\create-research-issues.ps1

# With assignee
.\create-research-issues.ps1 -Assignee copilot

# With custom output directory  
.\create-research-issues.ps1 -OutputDir "D:\research-issues"

# Skip delays (for testing only)
.\create-research-issues.ps1 -SkipDelay
```

**Features:**

- Creates all 42 issues automatically with proper labels
- Includes 120-second delays between issues to avoid rate limiting
- Color-coded output with progress tracking
- Error handling and validation
- Optional assignee assignment
- Confirmation prompts before execution

**Requirements:**

- PowerShell 5.1+ (Windows) or PowerShell Core (cross-platform)
- GitHub CLI (`gh`) installed and authenticated (`gh auth login`)
- Issue files generated by `generate-research-issues.py`

**Platform support:**

- Windows (PowerShell 5.1+)
- Linux/macOS (PowerShell Core)
- Windows Git Bash (use `pwsh` command)

**Time estimate:** ~80 minutes for all 40 group issues (due to rate limiting delays)

---

### check-documentation-quality.sh

A comprehensive script that validates documentation quality before committing.

**Usage:**

```bash
./scripts/check-documentation-quality.sh
```

**What it checks:**

1. **Required files** - Ensures essential files like README.md exist
2. **Directory structure** - Validates expected directories are present
3. **Markdown linting** - Runs markdownlint to check formatting
4. **Duplicate content** - Detects duplicate headings and duplicate file content across repository
5. **Broken links** - Identifies broken internal links in documentation
6. **Small/stub files** - Detects files that are too small or lack sufficient content
7. **File organization** - Validates that design documents are properly organized and all directories have README files

**Exit codes:**

- `0` - All checks passed or only warnings found
- `1` - Errors found that should be fixed before committing

**Requirements:**

- Bash (available on Linux, macOS, and Windows with Git Bash or WSL)
- Python 3 (for duplicate and link checking)
- markdownlint-cli (optional, install with `npm install -g markdownlint-cli`)

**Example output:**

```
=========================================
Documentation Quality Check
=========================================

1. Checking required files...
✓ README.md exists
✓ CONTRIBUTING.md exists
✓ DOCUMENTATION_BEST_PRACTICES.md exists

2. Checking directory structure...
✓ Found directory: docs
✓ Found directory: templates
✓ Found directory: design

3. Running markdown linting...
✓ All markdown files pass linting

4. Checking for duplicate content...
✓ No duplicate headings or file content found

5. Checking for broken internal links...
✓ No broken internal links found

6. Checking for small or stub files...
✓ No small or stub files detected

7. Checking file organization...
✓ File organization looks good

=========================================
Summary
=========================================
✓ All checks passed!

Your documentation is ready to commit! 🎉
```

## Running Scripts in CI

These scripts are integrated into the GitHub Actions workflows:

- `.github/workflows/ci.yml` - Main CI workflow
- `.github/workflows/content-quality.yml` - Content quality checks for PRs
- `.github/workflows/design-validation.yml` - Design documentation validation

## Contributing New Scripts

When adding new utility scripts:

1. Make scripts executable: `chmod +x scripts/your-script.sh`
2. Add clear comments explaining what the script does
3. Include usage examples in this README
4. Test on multiple platforms if possible (Linux, macOS, Windows)
5. Handle errors gracefully with appropriate exit codes

## Platform Compatibility

All scripts are designed to work on:

- **Linux** - Native bash and Python support
- **macOS** - Native bash and Python support  
- **Windows** - Using Git Bash, WSL, or PowerShell (where applicable)

**Windows users** have multiple options:
- PowerShell scripts (`.ps1`) work natively on Windows PowerShell 5.1+
- Bash scripts (`.sh`) work in Git Bash or WSL
- Python scripts (`.py`) work with Python 3 installed

**Cross-platform PowerShell**: PowerShell Core is available on Linux and macOS, so `.ps1` scripts can run on all platforms with `pwsh` installed.

## Getting Help

If you encounter issues with any script:

1. Check that all requirements are installed
2. Verify you're running from the repository root
3. Check script permissions: `ls -l scripts/`
4. Review the script's comments for specific requirements
5. See [USAGE_EXAMPLES.md](USAGE_EXAMPLES.md) for detailed platform-specific examples
6. Create an issue if the problem persists

---

## Research Automation Workflow

The scripts support an automated research workflow:

1. **Discovery Phase** (`autosources-discovery.py`)
   - Scan completed research for new sources
   - Generate prioritized discovery report
   - Create processing queue

2. **Planning Phase** (`generate-research-issues.py`)
   - Convert discoveries into assignment groups
   - Generate GitHub issues for tracking

3. **Issue Creation** (`create-research-issues.ps1`)
   - Automate GitHub issue creation
   - Track research progress

4. **Quality Assurance** (`check-documentation-quality.sh`)
   - Validate documentation quality
   - Ensure consistency

---

**Last Updated:** 2025-01-17  
**Maintainers:** BlueMarble.Design Documentation Team
