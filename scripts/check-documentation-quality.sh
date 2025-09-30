#!/bin/bash
#
# Documentation Quality Check Script
# Run this before committing to ensure documentation quality
#

set -e

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
REPO_ROOT="$(cd "$SCRIPT_DIR/.." && pwd)"

cd "$REPO_ROOT"

echo "========================================="
echo "Documentation Quality Check"
echo "========================================="
echo ""

# Colors for output
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
NC='\033[0m' # No Color

# Track overall status
WARNINGS=0
ERRORS=0

# Function to print status messages
print_status() {
    local status=$1
    local message=$2
    
    case $status in
        "ok")
            echo -e "${GREEN}✓${NC} $message"
            ;;
        "warn")
            echo -e "${YELLOW}⚠${NC} $message"
            WARNINGS=$((WARNINGS + 1))
            ;;
        "error")
            echo -e "${RED}✗${NC} $message"
            ERRORS=$((ERRORS + 1))
            ;;
        "info")
            echo -e "ℹ $message"
            ;;
    esac
}

# Check 1: Required files exist
echo "1. Checking required files..."
if [ -f "README.md" ]; then
    print_status "ok" "README.md exists"
else
    print_status "error" "README.md not found"
fi

if [ -f "CONTRIBUTING.md" ]; then
    print_status "ok" "CONTRIBUTING.md exists"
else
    print_status "warn" "CONTRIBUTING.md not found (recommended)"
fi

if [ -f "DOCUMENTATION_BEST_PRACTICES.md" ]; then
    print_status "ok" "DOCUMENTATION_BEST_PRACTICES.md exists"
else
    print_status "warn" "DOCUMENTATION_BEST_PRACTICES.md not found (recommended)"
fi

echo ""

# Check 2: Required directories exist
echo "2. Checking directory structure..."
for dir in docs templates design; do
    if [ -d "$dir" ]; then
        print_status "ok" "Found directory: $dir"
    else
        print_status "warn" "Directory not found: $dir (may be expected)"
    fi
done

echo ""

# Check 3: Markdown linting
echo "3. Running markdown linting..."
if command -v markdownlint >/dev/null 2>&1; then
    if markdownlint --config .markdownlint.json **/*.md 2>&1 | tee /tmp/lint-output.txt; then
        print_status "ok" "All markdown files pass linting"
    else
        lint_issues=$(wc -l < /tmp/lint-output.txt)
        print_status "warn" "Found $lint_issues linting issues (see above)"
        print_status "info" "Run 'markdownlint --fix --config .markdownlint.json **/*.md' to auto-fix some issues"
    fi
else
    print_status "warn" "markdownlint not installed - install with: npm install -g markdownlint-cli"
fi

echo ""

# Check 4: Check for duplicate headings
echo "4. Checking for duplicate content..."
python3 << 'EOF' || python << 'EOF'
import os
import re
import glob

duplicates_found = False

# Check main documentation files
main_files = ['README.md', 'USAGE_EXAMPLES.md', 'CONTRIBUTING.md']
for file in main_files:
    if not os.path.exists(file):
        continue
    
    with open(file, 'r') as f:
        content = f.read()
        
    # Find all H2 headings
    headings = re.findall(r'^## (.+)$', content, re.MULTILINE)
    seen = {}
    for heading in headings:
        if heading in seen:
            print(f"⚠ Duplicate heading in {file}: '{heading}'")
            duplicates_found = True
        else:
            seen[heading] = 1

if not duplicates_found:
    print("✓ No duplicate headings found in main files")
EOF

echo ""

# Check 5: Check for broken internal links
echo "5. Checking for broken internal links..."
python3 << 'EOF' || python << 'EOF'
import os
import re
import glob

link_pattern = re.compile(r'\[.*?\]\(([^)]*\.md)\)')
md_files = glob.glob('**/*.md', recursive=True)
md_files = [f for f in md_files if '.git' not in f]

broken_links = []
for md_file in md_files:
    if not os.path.isfile(md_file):
        continue
    
    try:
        with open(md_file, 'r', encoding='utf-8', errors='ignore') as f:
            content = f.read()
    except:
        continue
        
    for match in link_pattern.finditer(content):
        link = match.group(1)
        
        # Skip external links
        if link.startswith('http'):
            continue
        
        # Resolve relative path
        link_path = os.path.normpath(os.path.join(os.path.dirname(md_file), link.lstrip('./')))
        
        if not os.path.exists(link_path):
            broken_links.append((md_file, link))

if broken_links:
    print(f"⚠ Found {len(broken_links)} broken link(s):")
    for file, link in broken_links[:5]:
        print(f"  {file} -> {link}")
    if len(broken_links) > 5:
        print(f"  ... and {len(broken_links) - 5} more")
else:
    print("✓ No broken internal links found")
EOF

echo ""

# Summary
echo "========================================="
echo "Summary"
echo "========================================="

if [ $ERRORS -gt 0 ]; then
    print_status "error" "Found $ERRORS error(s)"
fi

if [ $WARNINGS -gt 0 ]; then
    print_status "warn" "Found $WARNINGS warning(s)"
fi

if [ $ERRORS -eq 0 ] && [ $WARNINGS -eq 0 ]; then
    print_status "ok" "All checks passed!"
    echo ""
    echo "Your documentation is ready to commit! 🎉"
    exit 0
elif [ $ERRORS -eq 0 ]; then
    echo ""
    echo "Documentation has warnings but is acceptable to commit."
    echo "Consider addressing warnings to improve quality."
    exit 0
else
    echo ""
    echo "Please fix errors before committing."
    exit 1
fi
