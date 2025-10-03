#!/usr/bin/env pwsh
<#
.SYNOPSIS
    Creates GitHub issues for all 40 research assignment groups using GitHub CLI.

.DESCRIPTION
    This PowerShell script automates the creation of GitHub issues for the BlueMarble
    research assignment groups. It creates:
    - 1 parent issue for Phase 1
    - 40 individual group issues
    - 1 Phase 2 planning issue
    
    The script adds a 120-second delay between group issue creation to avoid
    rate limiting and allow for proper issue processing.

.PARAMETER OutputDir
    Directory containing the generated issue files. 
    Default: C:\tmp\research-issues (Windows) or /tmp/research-issues (Unix)

.PARAMETER Assignee
    GitHub username to assign issues to. Optional.

.PARAMETER SkipDelay
    Skip the 120-second delay between issues (not recommended for production use).

.EXAMPLE
    .\create-research-issues.ps1
    
.EXAMPLE
    .\create-research-issues.ps1 -Assignee copilot
    
.EXAMPLE
    .\create-research-issues.ps1 -OutputDir "D:\research-issues"

.NOTES
    Requirements:
    - GitHub CLI (gh) must be installed and authenticated
    - Issue files must be generated first using generate-research-issues.py
    - Run: python3 scripts/generate-research-issues.py
#>

param(
    [string]$OutputDir = $(if ($IsWindows -or $env:OS -match "Windows") { "C:\tmp\research-issues" } else { "/tmp/research-issues" }),
    [string]$Assignee = "",
    [switch]$SkipDelay = $false
)

# Color output functions
function Write-Success {
    param([string]$Message)
    Write-Host "✓ $Message" -ForegroundColor Green
}

function Write-Info {
    param([string]$Message)
    Write-Host "ℹ $Message" -ForegroundColor Cyan
}

function Write-Warning {
    param([string]$Message)
    Write-Host "⚠ $Message" -ForegroundColor Yellow
}

function Write-Error {
    param([string]$Message)
    Write-Host "✗ $Message" -ForegroundColor Red
}

function Write-Header {
    param([string]$Message)
    Write-Host ""
    Write-Host "============================================================" -ForegroundColor Cyan
    Write-Host $Message -ForegroundColor Cyan
    Write-Host "============================================================" -ForegroundColor Cyan
    Write-Host ""
}

# Check prerequisites
function Test-Prerequisites {
    Write-Header "Checking Prerequisites"
    
    # Check if gh is installed
    if (-not (Get-Command gh -ErrorAction SilentlyContinue)) {
        Write-Error "GitHub CLI (gh) is not installed."
        Write-Info "Install from: https://cli.github.com/"
        return $false
    }
    Write-Success "GitHub CLI (gh) is installed"
    
    # Check if authenticated
    $authStatus = gh auth status 2>&1
    if ($LASTEXITCODE -ne 0) {
        Write-Error "GitHub CLI is not authenticated."
        Write-Info "Run: gh auth login"
        return $false
    }
    Write-Success "GitHub CLI is authenticated"
    
    # Check if output directory exists
    if (-not (Test-Path $OutputDir)) {
        Write-Error "Output directory not found: $OutputDir"
        Write-Info "Run: python3 scripts/generate-research-issues.py"
        return $false
    }
    Write-Success "Output directory exists: $OutputDir"
    
    # Check if issue files exist
    $parentIssue = Join-Path $OutputDir "issue-parent-phase-1.md"
    if (-not (Test-Path $parentIssue)) {
        Write-Error "Issue files not found in: $OutputDir"
        Write-Info "Run: python3 scripts/generate-research-issues.py"
        return $false
    }
    Write-Success "Issue files found"
    
    return $true
}

# Create a single issue
function New-GitHubIssue {
    param(
        [string]$Title,
        [string]$BodyFile,
        [string]$Labels,
        [string]$Assignee
    )
    
    $cmd = "gh issue create --title `"$Title`" --body-file `"$BodyFile`" --label `"$Labels`""
    
    if ($Assignee) {
        $cmd += " --assignee `"$Assignee`""
    }
    
    Write-Info "Creating: $Title"
    
    try {
        $result = Invoke-Expression $cmd 2>&1
        if ($LASTEXITCODE -eq 0) {
            Write-Success "Created: $Title"
            Write-Host "  URL: $result"
            return $true
        } else {
            Write-Error "Failed to create: $Title"
            Write-Host "  Error: $result" -ForegroundColor Red
            return $false
        }
    } catch {
        Write-Error "Exception creating issue: $Title"
        Write-Host "  Error: $_" -ForegroundColor Red
        return $false
    }
}

# Main execution
Write-Header "GitHub Research Issues Creator"
Write-Info "Output directory: $OutputDir"
if ($Assignee) {
    Write-Info "Assignee: $Assignee"
}
Write-Host ""

# Check prerequisites
if (-not (Test-Prerequisites)) {
    Write-Error "Prerequisites check failed. Exiting."
    exit 1
}

Write-Host ""
$response = Read-Host "This will create 42 issues (1 parent + 40 groups + 1 phase 2). Continue? (y/n)"
if ($response -ne 'y' -and $response -ne 'Y') {
    Write-Info "Operation cancelled by user."
    exit 0
}

Write-Header "Creating Issues"

# Step 1: Create parent issue
Write-Info "Step 1: Creating parent Phase 1 issue..."
$parentFile = Join-Path $OutputDir "issue-parent-phase-1.md"
$success = New-GitHubIssue `
    -Title "Research Phase 1: 40 Parallel Assignment Groups" `
    -BodyFile $parentFile `
    -Labels "research,phase-1,epic" `
    -Assignee $Assignee

if (-not $success) {
    Write-Error "Failed to create parent issue. Aborting."
    exit 1
}

Write-Host ""
Write-Info "Parent issue created. Press Enter to continue with group issues..."
Read-Host

# Step 2: Create all 40 group issues
Write-Info "Step 2: Creating 40 group issues..."
Write-Warning "This will take approximately 80 minutes due to rate limiting delays..."
Write-Host ""

$failedIssues = @()
$createdCount = 0

for ($i = 1; $i -le 40; $i++) {
    $num = $i.ToString("00")
    $groupFile = Join-Path $OutputDir "issue-group-$num.md"
    
    if (-not (Test-Path $groupFile)) {
        Write-Warning "Group file not found: $groupFile (skipping)"
        continue
    }
    
    # Read the file to extract title and priority
    $content = Get-Content $groupFile -Raw
    $titleMatch = $content -match "# Research Assignment Group \d+\s*\n\s*\*\*Labels:\*\*.*priority-(\w+)"
    $priority = if ($matches) { $matches[1] } else { "medium" }
    
    $success = New-GitHubIssue `
        -Title "Research Assignment Group $num" `
        -BodyFile $groupFile `
        -Labels "research,assignment-group-$num,priority-$priority,phase-1" `
        -Assignee $Assignee
    
    if ($success) {
        $createdCount++
    } else {
        $failedIssues += $num
    }
    
    # Add delay between issues (except for the last one)
    if ($i -lt 40 -and -not $SkipDelay) {
        Write-Info "Waiting 120 seconds before next issue to avoid rate limiting..."
        Write-Host "  Progress: $createdCount of 40 issues created" -ForegroundColor Cyan
        Start-Sleep -Seconds 120
        Write-Host ""
    }
}

Write-Host ""
Write-Success "Created $createdCount of 40 group issues"

if ($failedIssues.Count -gt 0) {
    Write-Warning "Failed to create issues: $($failedIssues -join ', ')"
}

# Step 3: Create Phase 2 planning issue
Write-Host ""
Write-Info "Step 3: Creating Phase 2 planning issue..."
$phase2File = Join-Path $OutputDir "issue-phase-2-planning.md"
$success = New-GitHubIssue `
    -Title "Research Phase 2: Planning and New Assignment Creation" `
    -BodyFile $phase2File `
    -Labels "research,phase-2,planning" `
    -Assignee $Assignee

# Final summary
Write-Header "Summary"
Write-Success "Issue creation complete!"
Write-Info "Total issues created: $(42 - $failedIssues.Count) of 42"

if ($failedIssues.Count -gt 0) {
    Write-Warning "Some issues failed to create. Please create them manually."
    Write-Info "Failed groups: $($failedIssues -join ', ')"
}

Write-Host ""
Write-Info "Next steps:"
Write-Info "  1. Review issues in GitHub"
Write-Info "  2. Link group issues to parent issue"
Write-Info "  3. Assign issues to team members"
Write-Host ""
