# Scripts

This directory contains utility scripts for maintaining documentation quality in the BlueMarble.Design repository.

## Available Scripts

### generate-research-issues.py

Generates GitHub issue content for all 20 research assignment groups plus parent and Phase 2 planning issues.

**Usage:**

```bash
python3 scripts/generate-research-issues.py
```

**Output:**

Creates issue files in `/tmp/research-issues/` directory:
- `issue-parent-phase-1.md` - Parent issue for Phase 1
- `issue-group-01.md` through `issue-group-20.md` - Individual group issues
- `issue-phase-2-planning.md` - Phase 2 planning issue
- `README.md` - Instructions for creating issues

**Creating Issues:**

Option 1: Manual (copy/paste into GitHub)
Option 2: GitHub CLI (`gh issue create --body-file ...`)
Option 3: GitHub API (automated script)

See generated README.md for detailed instructions.

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

For Windows users without Git Bash or WSL, Python-based scripts can be run directly with Python.

## Getting Help

If you encounter issues with any script:

1. Check that all requirements are installed
2. Verify you're running from the repository root
3. Check script permissions: `ls -l scripts/`
4. Review the script's comments for specific requirements
5. Create an issue if the problem persists

---

**Last Updated:** 2024-12-29  
**Maintainers:** BlueMarble.Design Documentation Team
