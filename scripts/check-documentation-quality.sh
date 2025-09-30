#!/bin/bash
#
# Documentation Quality Check Script
# Run this before committing to ensure documentation quality
#

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

# Determine python command
PYTHON_CMD="python3"
if ! command -v python3 >/dev/null 2>&1; then
    if command -v python >/dev/null 2>&1; then
        PYTHON_CMD="python"
    else
        echo "Error: Python not found. Please install Python 3."
        exit 1
    fi
fi

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
$PYTHON_CMD << 'EOF'
import os
import re
import glob
import hashlib

duplicates_found = False

# Check main documentation files for duplicate headings
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

# Check for duplicate file content across repository
md_files = [f for f in glob.glob('**/*.md', recursive=True) if '.git' not in f]
file_hashes = {}
for f in md_files:
    try:
        with open(f, 'rb') as file:
            content = file.read()
            if len(content) > 0:
                content_hash = hashlib.sha256(content).hexdigest()
                if content_hash not in file_hashes:
                    file_hashes[content_hash] = []
                file_hashes[content_hash].append(f)
    except (OSError, UnicodeDecodeError):
        continue

# Report duplicate files
for hash_val, files in file_hashes.items():
    if len(files) > 1:
        print(f"⚠ Duplicate file content detected:")
        for f in files:
            print(f"    {f}")
        duplicates_found = True

if not duplicates_found:
    print("✓ No duplicate headings or file content found")
EOF

echo ""

# Check 5: Check for broken internal links
echo "5. Checking for broken internal links..."
$PYTHON_CMD << 'EOF'
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

# Check 6: Check for small/stub files
echo "6. Checking for small or stub files..."
$PYTHON_CMD << 'EOF'
import os
import glob

small_files = []
md_files = [f for f in glob.glob('**/*.md', recursive=True) if '.git' not in f]

# Define minimum file size threshold (500 bytes)
MIN_FILE_SIZE = 500
# Define minimum content lines (excluding headings and empty lines)
MIN_CONTENT_LINES = 10

for f in md_files:
    file_size = os.path.getsize(f)
    
    # Check file size
    if file_size < MIN_FILE_SIZE:
        try:
            with open(f, 'r', encoding='utf-8', errors='ignore') as file:
                lines = file.readlines()
                # Count non-empty, non-heading lines
                content_lines = [l for l in lines if l.strip() and not l.strip().startswith('#')]
                
                if len(content_lines) < MIN_CONTENT_LINES:
                    small_files.append((f, file_size, len(content_lines)))
        except (OSError, UnicodeDecodeError):
            continue

if small_files:
    print(f"⚠ Found {len(small_files)} small or stub file(s) that may need content:")
    for f, size, lines in sorted(small_files, key=lambda x: x[1])[:10]:
        print(f"    {f} ({size} bytes, {lines} content lines)")
    if len(small_files) > 10:
        print(f"    ... and {len(small_files) - 10} more")
    print("  Consider adding more content or consolidating with related documents")
else:
    print("✓ No small or stub files detected")
EOF

echo ""

# Check 7: Validate file organization
echo "7. Checking file organization..."
$PYTHON_CMD << 'EOF'
import os
import glob

# Check for design documents in root that should be in proper folders
root_md_files = glob.glob('*.md')
expected_root_files = [
    'README.md', 
    'CONTRIBUTING.md', 
    'DOCUMENTATION_BEST_PRACTICES.md',
    'USAGE_EXAMPLES.md',
    'LICENSE.md'
]

misplaced_files = []
for f in root_md_files:
    basename = os.path.basename(f)
    if basename not in expected_root_files:
        # Check if it's a design document that should be in docs/ or design/
        if any(keyword in basename.lower() for keyword in ['design', 'gdd', 'outline', 'spec', 'plan']):
            misplaced_files.append(f)

# Check for proper README files in directories
expected_readme_dirs = [
    'docs', 'docs/core', 'docs/gameplay', 'docs/systems', 'docs/world',
    'docs/ui-ux', 'docs/audio', 'docs/research',
    'templates', 'assets', 'design', 'research', 'roadmap', 'scripts'
]

missing_readmes = []
for dir_path in expected_readme_dirs:
    readme_path = os.path.join(dir_path, 'README.md')
    if os.path.isdir(dir_path) and not os.path.exists(readme_path):
        missing_readmes.append(dir_path)

issues_found = False

if misplaced_files:
    print(f"⚠ Found {len(misplaced_files)} design document(s) in root that should be organized:")
    for f in misplaced_files:
        print(f"    {f} -> Consider moving to docs/, design/, or templates/")
    issues_found = True

if missing_readmes:
    print(f"⚠ Found {len(missing_readmes)} director(ies) without README.md:")
    for d in missing_readmes:
        print(f"    {d}/")
    issues_found = True

if not issues_found:
    print("✓ File organization looks good")
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
