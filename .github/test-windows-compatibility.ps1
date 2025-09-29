# Windows Compatibility Test Script for Self-Hosted Runners
# This script tests the key functionality that the workflows depend on

Write-Host "Testing Windows Compatibility for BlueMarble.Design Self-Hosted Runners" -ForegroundColor Green
Write-Host "================================================================" -ForegroundColor Green

# Test 1: Check required software availability
Write-Host "`n1. Testing Required Software:" -ForegroundColor Yellow

$requiredCommands = @('git', 'python', 'python3', 'node', 'npm')
foreach ($cmd in $requiredCommands) {
    if (Get-Command $cmd -ErrorAction SilentlyContinue) {
        Write-Host "  ✓ $cmd is available" -ForegroundColor Green
    } else {
        Write-Host "  ✗ $cmd is NOT available" -ForegroundColor Red
    }
}

# Test 2: Check Python functionality
Write-Host "`n2. Testing Python Functionality:" -ForegroundColor Yellow

try {
    $pythonVersion = python --version 2>&1
    Write-Host "  ✓ Python version: $pythonVersion" -ForegroundColor Green
    
    # Test Python modules used in workflows
    python -c "import yaml; print('✓ PyYAML available')" 2>$null
    if ($LASTEXITCODE -eq 0) {
        Write-Host "  ✓ PyYAML module available" -ForegroundColor Green
    } else {
        Write-Host "  ✗ PyYAML module NOT available (install with: pip install PyYAML)" -ForegroundColor Red
    }
    
    python -c "import os, glob, re; print('✓ Standard modules available')"
    Write-Host "  ✓ Standard Python modules available" -ForegroundColor Green
    
} catch {
    Write-Host "  ✗ Python not working properly" -ForegroundColor Red
}

# Test 3: Check Node.js and npm
Write-Host "`n3. Testing Node.js and npm:" -ForegroundColor Yellow

try {
    $nodeVersion = node --version
    $npmVersion = npm --version
    Write-Host "  ✓ Node.js version: $nodeVersion" -ForegroundColor Green
    Write-Host "  ✓ npm version: $npmVersion" -ForegroundColor Green
} catch {
    Write-Host "  ✗ Node.js or npm not working properly" -ForegroundColor Red
}

# Test 4: Test cross-platform file operations (simulating workflow commands)
Write-Host "`n4. Testing Cross-Platform File Operations:" -ForegroundColor Yellow

# Test markdown file discovery
try {
    $mdFiles = Get-ChildItem -Path . -Filter "*.md" -Recurse | Where-Object { $_.FullName -notlike "*\.git*" -and $_.FullName -notlike "*node_modules*" } | Select-Object -First 3
    Write-Host "  ✓ Found $($mdFiles.Count) markdown files:" -ForegroundColor Green
    foreach ($file in $mdFiles) {
        Write-Host "    - $($file.Name)" -ForegroundColor Gray
    }
} catch {
    Write-Host "  ✗ Failed to discover markdown files" -ForegroundColor Red
}

# Test YAML file discovery
try {
    $yamlFiles = Get-ChildItem -Path . -Include "*.yml", "*.yaml" -Recurse | Where-Object { $_.FullName -notlike "*\.git*" } | Select-Object -First 3
    Write-Host "  ✓ Found $($yamlFiles.Count) YAML files:" -ForegroundColor Green
    foreach ($file in $yamlFiles) {
        Write-Host "    - $($file.Name)" -ForegroundColor Gray
    }
} catch {
    Write-Host "  ✗ Failed to discover YAML files" -ForegroundColor Red
}

# Test 5: Test Git Bash availability (required for workflows)
Write-Host "`n5. Testing Git Bash Availability:" -ForegroundColor Yellow

$gitBashPath = Get-Command bash -ErrorAction SilentlyContinue
if ($gitBashPath) {
    Write-Host "  ✓ Git Bash is available at: $($gitBashPath.Source)" -ForegroundColor Green
    
    # Test a simple bash command
    $bashTest = bash -c "echo 'Bash is working'"
    if ($bashTest -eq "Bash is working") {
        Write-Host "  ✓ Bash commands execute correctly" -ForegroundColor Green
    } else {
        Write-Host "  ✗ Bash commands not executing correctly" -ForegroundColor Red
    }
} else {
    Write-Host "  ✗ Git Bash is NOT available" -ForegroundColor Red
    Write-Host "    Install Git for Windows to get Git Bash" -ForegroundColor Yellow
}

# Test 6: Test PowerShell execution policy
Write-Host "`n6. Testing PowerShell Execution Policy:" -ForegroundColor Yellow

$executionPolicy = Get-ExecutionPolicy
Write-Host "  Current execution policy: $executionPolicy" -ForegroundColor Gray

if ($executionPolicy -in @('RemoteSigned', 'Unrestricted', 'Bypass')) {
    Write-Host "  ✓ Execution policy allows script execution" -ForegroundColor Green
} else {
    Write-Host "  ⚠ Execution policy may block scripts" -ForegroundColor Yellow
    Write-Host "    Consider running: Set-ExecutionPolicy -ExecutionPolicy RemoteSigned -Scope CurrentUser" -ForegroundColor Yellow
}

# Summary
Write-Host "`n================================================================" -ForegroundColor Green
Write-Host "Windows Compatibility Test Complete!" -ForegroundColor Green
Write-Host "`nNext Steps:" -ForegroundColor Yellow
Write-Host "1. Address any ✗ items shown above" -ForegroundColor Gray
Write-Host "2. Install missing dependencies using Chocolatey or manual installation" -ForegroundColor Gray
Write-Host "3. Follow the setup instructions in .github/RUNNER_SETUP.md" -ForegroundColor Gray
Write-Host "4. Register the runner with your GitHub repository" -ForegroundColor Gray
Write-Host "5. Test the workflows by creating a test commit or PR" -ForegroundColor Gray