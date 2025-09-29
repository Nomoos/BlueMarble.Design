#!/bin/bash

# Linux/macOS Compatibility Test Script for Self-Hosted Runners
# This script tests the key functionality that the workflows depend on

echo "Testing Linux/macOS Compatibility for BlueMarble.Design Self-Hosted Runners"
echo "========================================================================"

# Test 1: Check required software availability
echo ""
echo "1. Testing Required Software:"

required_commands=("git" "python3" "node" "npm" "find" "grep" "wc")
for cmd in "${required_commands[@]}"; do
    if command -v "$cmd" >/dev/null 2>&1; then
        echo "  ✓ $cmd is available"
    else
        echo "  ✗ $cmd is NOT available"
    fi
done

# Test 2: Check Python functionality
echo ""
echo "2. Testing Python Functionality:"

if python3 --version >/dev/null 2>&1; then
    echo "  ✓ Python version: $(python3 --version)"
    
    # Test Python modules used in workflows
    if python3 -c "import yaml" 2>/dev/null; then
        echo "  ✓ PyYAML module available"
    else
        echo "  ✗ PyYAML module NOT available (install with: pip install PyYAML)"
    fi
    
    if python3 -c "import os, glob, re" 2>/dev/null; then
        echo "  ✓ Standard Python modules available"
    else
        echo "  ✗ Standard Python modules NOT available"
    fi
else
    echo "  ✗ Python not working properly"
fi

# Test 3: Check Node.js and npm
echo ""
echo "3. Testing Node.js and npm:"

if node --version >/dev/null 2>&1 && npm --version >/dev/null 2>&1; then
    echo "  ✓ Node.js version: $(node --version)"
    echo "  ✓ npm version: $(npm --version)"
else
    echo "  ✗ Node.js or npm not working properly"
fi

# Test 4: Test cross-platform file operations (simulating workflow commands)
echo ""
echo "4. Testing Cross-Platform File Operations:"

# Test markdown file discovery
if command -v find >/dev/null 2>&1; then
    md_count=$(find . -name "*.md" -not -path "./node_modules/*" -not -path "./.git/*" | wc -l)
    echo "  ✓ Found $md_count markdown files using find command"
else
    echo "  ✗ find command not available"
fi

# Test YAML file discovery
yaml_count=$(find . \( -name "*.yml" -o -name "*.yaml" \) | grep -v ".git" | wc -l)
echo "  ✓ Found $yaml_count YAML files"

# Test grep functionality
if echo "test" | grep "test" >/dev/null 2>&1; then
    echo "  ✓ grep command working"
else
    echo "  ✗ grep command not working"
fi

# Test 5: Test shell compatibility
echo ""
echo "5. Testing Shell Compatibility:"

if [ -n "$BASH_VERSION" ]; then
    echo "  ✓ Running in Bash: $BASH_VERSION"
elif [ -n "$ZSH_VERSION" ]; then
    echo "  ✓ Running in Zsh: $ZSH_VERSION"
else
    echo "  ⚠ Running in unknown shell: $0"
fi

# Test basic shell operations
if [ -d "docs" ]; then
    echo "  ✓ Directory existence check working"
else
    echo "  ⚠ docs directory not found (this may be expected)"
fi

if [ -f "README.md" ]; then
    echo "  ✓ File existence check working"
else
    echo "  ✗ README.md not found"
fi

# Test 6: Test archive creation capabilities
echo ""
echo "6. Testing Archive Creation:"

if command -v tar >/dev/null 2>&1; then
    echo "  ✓ tar command available"
else
    echo "  ✗ tar command not available"
fi

if command -v zip >/dev/null 2>&1; then
    echo "  ✓ zip command available"
else
    echo "  ⚠ zip command not available (not required for basic workflows)"
fi

# Summary
echo ""
echo "========================================================================"
echo "Linux/macOS Compatibility Test Complete!"
echo ""
echo "Next Steps:"
echo "1. Address any ✗ items shown above"
echo "2. Install missing dependencies using your package manager"
echo "3. Follow the setup instructions in .github/RUNNER_SETUP.md"
echo "4. Register the runner with your GitHub repository"
echo "5. Test the workflows by creating a test commit or PR"