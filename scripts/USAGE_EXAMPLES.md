# Research Issues Script Usage Examples

This document provides practical examples for creating research issues using the automation scripts.

## Quick Start

### Windows Users (Recommended)

```powershell
# Step 1: Generate issue files
python3 scripts/generate-research-issues.py

# Step 2: Navigate to scripts directory
cd scripts

# Step 3: Run PowerShell script
.\create-research-issues.ps1

# With optional assignee
.\create-research-issues.ps1 -Assignee copilot
```

### Linux/macOS Users

```bash
# Step 1: Generate issue files
python3 scripts/generate-research-issues.py

# Step 2: Navigate to issue directory
cd /tmp/research-issues

# Step 3: Create issues with bash loop
gh issue create --title "Research Phase 1: 40 Parallel Assignment Groups" \
  --body-file issue-parent-phase-1.md \
  --label "research,phase-1,epic"

for i in {01..40}; do
  gh issue create --title "Research Assignment Group $i" \
    --body-file "issue-group-$i.md" \
    --label "research,assignment-group-$i,phase-1"
  sleep 120
done

gh issue create --title "Research Phase 2: Planning and New Assignment Creation" \
  --body-file issue-phase-2-planning.md \
  --label "research,phase-2,planning"
```

## Platform-Specific Instructions

### Windows PowerShell

**Prerequisites:**
```powershell
# Install GitHub CLI (if not already installed)
winget install GitHub.cli

# Authenticate with GitHub
gh auth login
```

**Usage:**
```powershell
# Basic usage
.\create-research-issues.ps1

# With custom output directory
.\create-research-issues.ps1 -OutputDir "C:\research-issues"

# Skip delays (testing only - not recommended for production)
.\create-research-issues.ps1 -SkipDelay

# Full example with all options
.\create-research-issues.ps1 -OutputDir "C:\tmp\research-issues" -Assignee myusername
```

### Windows Git Bash

```bash
# Run PowerShell script from Git Bash
pwsh scripts/create-research-issues.ps1

# Or use bash commands directly
cd /tmp/research-issues
bash << 'EOF'
gh issue create --title "Research Phase 1: 40 Parallel Assignment Groups" \
  --body-file issue-parent-phase-1.md \
  --label "research,phase-1,epic"

for i in {01..40}; do
  gh issue create --title "Research Assignment Group $i" \
    --body-file "issue-group-$i.md" \
    --label "research,assignment-group-$i,phase-1"
  sleep 120
done
EOF
```

### Windows WSL (Ubuntu)

```bash
# Generate files (from Windows)
python3 scripts/generate-research-issues.py

# Run from WSL
cd /mnt/c/tmp/research-issues
gh issue create --title "Research Phase 1: 40 Parallel Assignment Groups" \
  --body-file issue-parent-phase-1.md \
  --label "research,phase-1,epic"

for i in {01..40}; do
  gh issue create --title "Research Assignment Group $i" \
    --body-file "issue-group-$i.md" \
    --label "research,assignment-group-$i,phase-1"
  sleep 120
done
```

### Linux/macOS with PowerShell Core

```bash
# Install PowerShell Core (if not installed)
# Ubuntu/Debian:
sudo apt-get install -y powershell

# macOS:
brew install --cask powershell

# Run PowerShell script
pwsh scripts/create-research-issues.ps1
```

## Manual Creation (All Platforms)

If automation doesn't work, you can create issues manually:

1. Generate issue files:
   ```bash
   python3 scripts/generate-research-issues.py
   ```

2. Open each file in `/tmp/research-issues/`

3. Copy content from each file

4. Go to: https://github.com/Nomoos/BlueMarble.Design/issues/new

5. Paste content and add appropriate labels

6. Create issue

7. Repeat for all 42 issues

## Troubleshooting

### "gh: command not found"

Install GitHub CLI:
- **Windows**: `winget install GitHub.cli`
- **macOS**: `brew install gh`
- **Linux**: See https://github.com/cli/cli/blob/trunk/docs/install_linux.md

### "GitHub CLI is not authenticated"

```bash
gh auth login
```

Follow the prompts to authenticate.

### "Issue files not found"

Make sure you've run the Python script first:
```bash
python3 scripts/generate-research-issues.py
```

### "PowerShell script won't run" (Windows)

You may need to change execution policy:
```powershell
Set-ExecutionPolicy -ExecutionPolicy RemoteSigned -Scope CurrentUser
```

### Rate Limiting

If you get rate limited:
- The PowerShell script includes 120-second delays by default
- Don't use `-SkipDelay` in production
- If still hitting limits, increase delay time in the script

## Time Estimates

- **PowerShell automated**: ~80 minutes (with delays)
- **Manual creation**: 2-3 hours
- **Bash script**: ~80 minutes (with delays)

## Best Practices

1. **Always generate files first** with Python script
2. **Use delays** between issue creation to avoid rate limits
3. **Test with a few issues** before creating all 40
4. **Assign issues** after creation, not during (faster)
5. **Create parent first**, then groups, then phase 2

## Support

For issues or questions:
1. Check this documentation
2. Review [scripts/README.md](README.md)
3. Create an issue in the repository
4. Contact the maintainers

---

**Last Updated:** 2025-01-03  
**Repository:** [BlueMarble.Design](https://github.com/Nomoos/BlueMarble.Design)
