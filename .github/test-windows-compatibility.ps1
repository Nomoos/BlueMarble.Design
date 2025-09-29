# Windows Compatibility Test Script for Self-Hosted Runners
# This script tests the key functionality that the workflows depend on

Write-Host "Testing Windows Compatibility for BlueMarble.Design Self-Hosted Runners" -ForegroundColor Green
Write-Host "================================================================" -ForegroundColor Green

# Test 1: Check required software availability
Write-Host "`n1. Testing Required Software:" -ForegroundColor Yellow

$requiredCommands = @('git', 'python', 'python3', 'py', 'node', 'npm')
$workingPython = $null

foreach ($cmd in $requiredCommands) {
    if (Get-Command $cmd -ErrorAction SilentlyContinue) {
        Write-Host "  ✓ $cmd is available" -ForegroundColor Green
        
        # Test if this is a working Python command
        if ($cmd -in @('python', 'python3', 'py') -and -not $workingPython) {
            try {
                $version = & $cmd --version 2>&1
                if ($LASTEXITCODE -eq 0) {
                    $workingPython = $cmd
                }
            } catch {
                # Continue checking other commands
            }
        }
    } else {
        Write-Host "  ✗ $cmd is NOT available" -ForegroundColor Red
    }
}

# Test 2: Check Python functionality
Write-Host "`n2. Testing Python Functionality:" -ForegroundColor Yellow

# Try different Python commands that might be available on Windows
$pythonCommands = @('python', 'python3', 'py')
$workingPython = $null

foreach ($cmd in $pythonCommands) {
    if (Get-Command $cmd -ErrorAction SilentlyContinue) {
        try {
            $version = & $cmd --version 2>&1
            if ($LASTEXITCODE -eq 0) {
                $workingPython = $cmd
                Write-Host "  ✓ $cmd version: $version" -ForegroundColor Green
                break
            }
        } catch {
            # Continue to next command
        }
    }
}

if ($workingPython) {
    # Test Python modules used in workflows
    try {
        & $workingPython -c "import yaml; print('✓ PyYAML available')" 2>$null
        if ($LASTEXITCODE -eq 0) {
            Write-Host "  ✓ PyYAML module available" -ForegroundColor Green
        } else {
            Write-Host "  ✗ PyYAML module NOT available (install with: $workingPython -m pip install PyYAML)" -ForegroundColor Red
        }
    } catch {
        Write-Host "  ✗ PyYAML module NOT available (install with: $workingPython -m pip install PyYAML)" -ForegroundColor Red
    }
    
    try {
        & $workingPython -c "import os, glob, re; print('✓ Standard modules available')"
        if ($LASTEXITCODE -eq 0) {
            Write-Host "  ✓ Standard Python modules available" -ForegroundColor Green
        }
    } catch {
        Write-Host "  ✗ Standard Python modules NOT available" -ForegroundColor Red
    }
} else {
    Write-Host "  ✗ No working Python installation found" -ForegroundColor Red
    Write-Host "    Try installing Python from python.org or using 'winget install Python.Python.3'" -ForegroundColor Yellow
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

# Test markdown file discovery using PowerShell
try {
    $mdFiles = Get-ChildItem -Path . -Filter "*.md" -Recurse | Where-Object { 
        $_.FullName -notlike "*\.git*" -and 
        $_.FullName -notlike "*node_modules*" 
    } | Select-Object -First 5
    
    Write-Host "  ✓ Found $($mdFiles.Count) markdown files (showing first 5):" -ForegroundColor Green
    foreach ($file in $mdFiles) {
        Write-Host "    - $($file.Name)" -ForegroundColor Gray
    }
} catch {
    Write-Host "  ✗ Failed to discover markdown files" -ForegroundColor Red
}

# Test YAML file discovery using PowerShell
try {
    $yamlFiles = Get-ChildItem -Path . -Include "*.yml", "*.yaml" -Recurse | Where-Object { 
        $_.FullName -notlike "*\.git*" 
    } | Select-Object -First 5
    
    Write-Host "  ✓ Found $($yamlFiles.Count) YAML files (showing first 5):" -ForegroundColor Green
    foreach ($file in $yamlFiles) {
        Write-Host "    - $($file.Name)" -ForegroundColor Gray
    }
} catch {
    Write-Host "  ✗ Failed to discover YAML files" -ForegroundColor Red
}

# Test file content analysis (simulating what workflows do)
if ($mdFiles -and $mdFiles.Count -gt 0) {
    try {
        $testFile = $mdFiles[0]
        $content = Get-Content $testFile.FullName -ErrorAction Stop
        $size = (Get-Item $testFile.FullName).Length
        Write-Host "  ✓ Can read file content: $($testFile.Name) ($size bytes)" -ForegroundColor Green
    } catch {
        Write-Host "  ✗ Failed to read file content" -ForegroundColor Red
    }
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

# Test 7: Test Workflow Compatibility
Write-Host "`n7. Testing Workflow Compatibility:" -ForegroundColor Yellow

if ($workingPython) {
    # Test the Python-based file operations that workflows now use
    Write-Host "  Testing Python-based file operations used in workflows..." -ForegroundColor Gray
    
    try {
        # Test markdown file discovery (like the workflows do)
        $mdTestResult = & $workingPython -c @"
import glob
import os
md_files = []
for pattern in ['**/*.md', './*.md']:
    md_files.extend(glob.glob(pattern, recursive=True))
md_files = [f for f in md_files if '.git' not in f and 'node_modules' not in f]
print(f'Found {len(md_files)} markdown files')
for f in md_files[:3]:
    print(f'  {f}')
"@
        if ($LASTEXITCODE -eq 0) {
            Write-Host "  ✓ Python-based markdown discovery works" -ForegroundColor Green
            Write-Host "    $mdTestResult" -ForegroundColor Gray
        }
    } catch {
        Write-Host "  ✗ Python-based markdown discovery failed" -ForegroundColor Red
    }
    
    try {
        # Test YAML validation (like the workflows do)
        $yamlTestResult = & $workingPython -c @"
import os
import yaml

# Better YAML file discovery that includes hidden directories
yaml_files = []
for root, dirs, files in os.walk('.'):
    for file in files:
        if file.endswith(('.yml', '.yaml')):
            full_path = os.path.join(root, file)
            if '/.git/' not in full_path.replace(os.sep, '/'):
                yaml_files.append(full_path)

print(f'Found {len(yaml_files)} YAML files')
valid_count = 0
for file in yaml_files:
    try:
        with open(file, 'r', encoding='utf-8') as f:
            yaml.safe_load(f)
        valid_count += 1
    except Exception as e:
        print(f'Invalid YAML: {file} - {e}')
print(f'{valid_count}/{len(yaml_files)} YAML files are valid')
"@
        if ($LASTEXITCODE -eq 0) {
            Write-Host "  ✓ Python-based YAML validation works" -ForegroundColor Green
            Write-Host "    $yamlTestResult" -ForegroundColor Gray
        }
    } catch {
        Write-Host "  ✗ Python-based YAML validation failed" -ForegroundColor Red
    }
} else {
    Write-Host "  ⚠ Cannot test workflow compatibility without working Python" -ForegroundColor Yellow
}

Write-Host "`nNext Steps:" -ForegroundColor Yellow
Write-Host "1. Address any ✗ items shown above" -ForegroundColor Gray
Write-Host "2. Install missing dependencies using Chocolatey or manual installation" -ForegroundColor Gray
Write-Host "3. Follow the setup instructions in .github/RUNNER_SETUP.md" -ForegroundColor Gray
Write-Host "4. Register the runner with your GitHub repository" -ForegroundColor Gray
Write-Host "5. Test the workflows by creating a test commit or PR" -ForegroundColor Gray