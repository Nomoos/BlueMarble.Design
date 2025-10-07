#!/usr/bin/env python3
"""
Automated Source Discovery Tool for BlueMarble Research
========================================================

This tool automatically discovers and catalogs research sources from:
1. Existing research documents (citations and references)
2. Related source recommendations
3. Cross-references within completed research
4. (Planned) Academic and industry databases (future enhancement; not yet implemented)

Usage:
    python autosources-discovery.py [options]

Options:
    --scan-all          Scan all existing research documents
    --phase N           Focus on Phase N documents only
    --priority LEVEL    Filter by priority (critical, high, medium, low)
    --category CAT      Filter by category (gamedev-tech, gamedev-design, etc.)
    --output FILE       Output file for discovered sources (default: auto-discovered-sources.md)
    --format FORMAT     Output format: markdown, json, yaml (default: markdown)
"""

import os
import re
import json
import yaml
from pathlib import Path
from datetime import datetime
from typing import List, Dict, Set, Optional
from collections import defaultdict

class SourceDiscovery:
    """Automated source discovery engine"""
    
    def __init__(self, research_dir: str = "research/literature"):
        self.research_dir = Path(research_dir)
        self.discovered_sources = []
        self.source_references = defaultdict(list)
        self.categories = set()
        self.priorities = set()
        
    def scan_research_documents(self, phase_filter: Optional[int] = None) -> List[Dict]:
        """Scan all research documents for source references"""
        print(f"Scanning research documents in {self.research_dir}...")
        
        patterns = [
            # Citation patterns
            r'\*\*(?:Title|Source):\*\*\s+(.+)',
            r'\*\*Author:\*\*\s+(.+)',
            r'\*\*Publisher:\*\*\s+(.+)',
            r'ISBN:\s*(\d{3}-\d{10}|\d{13})',
            r'URL:\s*(https?://[^\s\)]+)',
            
            # Discovered from patterns
            r'Discovered From:\s*(.+)',
            r'Referenced in:\s*(.+)',
            
            # Next steps / future research patterns
            r'Future research:\s*(.+)',
            r'Additional sources:\s*(.+)',
            r'Recommended reading:\s*(.+)',
            r'See also:\s*(.+)',
        ]
        
        for doc_file in self.research_dir.glob("*.md"):
            # If phase_filter is set, check both filename and frontmatter for phase info
            if phase_filter:
                phase_in_name = f"phase-{phase_filter}" in doc_file.name
                try:
                    with open(doc_file, 'r', encoding='utf-8') as f:
                        content = f.read()
                    frontmatter = self._extract_frontmatter(content)
                    phase_in_frontmatter = False
                    # Accept both int and str for phase in frontmatter
                    if frontmatter and "phase" in frontmatter:
                        phase_value = frontmatter["phase"]
                        # Try to normalize to int for comparison
                        try:
                            phase_in_frontmatter = int(phase_value) == int(phase_filter)
                        except Exception:
                            phase_in_frontmatter = str(phase_value) == str(phase_filter)
                    if not (phase_in_name or phase_in_frontmatter):
                        continue
                except Exception as e:
                    print(f"Error reading {doc_file} for phase filtering: {e}")
                    continue
                
            self._scan_document(doc_file, patterns)
        
        return self.discovered_sources
    
    def _scan_document(self, doc_path: Path, patterns: List[str]):
        """Scan a single document for source references"""
        try:
            with open(doc_path, 'r', encoding='utf-8') as f:
                content = f.read()
                
            # Extract YAML frontmatter
            frontmatter = self._extract_frontmatter(content)
            
            # Scan for source patterns
            for pattern in patterns:
                matches = re.finditer(pattern, content, re.MULTILINE | re.IGNORECASE)
                for match in matches:
                    self._add_source_reference(doc_path.name, match.group(1), pattern)
            
            # Look for "Discovered Sources" sections
            self._extract_discovered_sections(doc_path.name, content)
            
        except Exception as e:
            print(f"Error scanning {doc_path}: {e}")
    
    def _extract_frontmatter(self, content: str) -> Dict:
        """Extract YAML frontmatter from markdown document"""
        match = re.match(r'^---\s*\n(.*?)\n---\s*\n', content, re.DOTALL)
        if match:
            try:
                return yaml.safe_load(match.group(1))
            except:
                return {}
        return {}
    
    def _extract_discovered_sections(self, doc_name: str, content: str):
        """Extract sources from 'Discovered Sources' sections"""
        # Pattern for discovered sources sections
        section_pattern = r'##\s+(?:Discovered|Next|Future|Additional)\s+Sources.*?\n(.*?)(?=\n##|\Z)'
        
        for match in re.finditer(section_pattern, content, re.DOTALL | re.IGNORECASE):
            section_content = match.group(1)
            
            # Extract source entries
            source_pattern = r'[-\*]\s+\*\*(.+?)\*\*\s*[:\-]?\s*(.+?)(?=\n[-\*]|\n\n|\Z)'
            for source_match in re.finditer(source_pattern, section_content, re.DOTALL):
                title = source_match.group(1).strip()
                description = source_match.group(2).strip()
                
                self._add_discovered_source(
                    title=title,
                    description=description,
                    source_document=doc_name,
                    priority=self._infer_priority(description),
                    category=self._infer_category(description)
                )
    
    def _add_source_reference(self, doc_name: str, reference: str, pattern_type: str):
        """Add a source reference to the tracking system"""
        self.source_references[reference].append({
            'document': doc_name,
            'pattern': pattern_type
        })
    
    def _add_discovered_source(self, title: str, description: str, source_document: str,
                                priority: str = 'medium', category: str = 'general'):
        """Add a discovered source to the collection"""
        # Check if source already exists
        for source in self.discovered_sources:
            if source['title'].lower() == title.lower():
                source['references'].append(source_document)
                return
        
        # Add new source
        self.discovered_sources.append({
            'title': title,
            'description': description,
            'priority': priority,
            'category': category,
            'references': [source_document],
            'discovered_date': datetime.now().isoformat(),
            'status': 'discovered',
            'estimated_effort': self._estimate_effort(description)
        })
        
        self.priorities.add(priority)
        self.categories.add(category)
    
    def _infer_priority(self, text: str) -> str:
        """Infer priority from description text"""
        text_lower = text.lower()
        
        if any(word in text_lower for word in ['critical', 'essential', 'must-read', 'fundamental']):
            return 'critical'
        elif any(word in text_lower for word in ['important', 'high', 'recommended']):
            return 'high'
        elif any(word in text_lower for word in ['useful', 'helpful', 'medium']):
            return 'medium'
        else:
            return 'low'
    
    def _infer_category(self, text: str) -> str:
        """Infer category from description text"""
        text_lower = text.lower()
        
        categories = {
            'gamedev-tech': ['technical', 'architecture', 'engine', 'performance', 'optimization'],
            'gamedev-design': ['design', 'mechanics', 'gameplay', 'balance', 'economy'],
            'gamedev-art': ['art', 'graphics', 'rendering', 'visual', 'shader'],
            'survival': ['survival', 'crafting', 'resource', 'gathering'],
            'architecture': ['distributed', 'scalable', 'infrastructure', 'backend'],
            'networking': ['network', 'multiplayer', 'synchronization', 'latency'],
        }
        
        for category, keywords in categories.items():
            if any(keyword in text_lower for keyword in keywords):
                return category
        
        return 'general'
    
    def _estimate_effort(self, description: str) -> str:
        """Estimate research effort based on description"""
        text_lower = description.lower()
        
        # Check for explicit hour estimates
        hour_match = re.search(r'(\d+)-?(\d+)?\s*hours?', text_lower)
        if hour_match:
            return hour_match.group(0)
        
        # Infer from content type
        if any(word in text_lower for word in ['book', 'comprehensive', 'extensive']):
            return '8-12 hours'
        elif any(word in text_lower for word in ['talk', 'presentation', 'video']):
            return '2-4 hours'
        elif any(word in text_lower for word in ['article', 'blog', 'post']):
            return '1-3 hours'
        else:
            return '4-6 hours'
    
    def generate_markdown_report(self, output_file: str = "auto-discovered-sources.md"):
        """Generate a markdown report of discovered sources"""
        output_path = self.research_dir / output_file
        
        # Group sources by priority
        by_priority = defaultdict(list)
        for source in self.discovered_sources:
            by_priority[source['priority']].append(source)
        
        # Generate markdown content
        content = [
            "# Auto-Discovered Research Sources",
            "",
            "---",
            f"title: Auto-Discovered Research Sources",
            f"date: {datetime.now().strftime('%Y-%m-%d')}",
            "tags: [research, auto-discovered, sources]",
            "status: discovered",
            "generated: automatic",
            "---",
            "",
            "**Document Type:** Auto-Generated Source Discovery Report",
            f"**Generation Date:** {datetime.now().strftime('%Y-%m-%d %H:%M:%S')}",
            f"**Total Sources Discovered:** {len(self.discovered_sources)}",
            f"**Source Documents Scanned:** {len(list(self.research_dir.glob('*.md')))}",
            "",
            "---",
            "",
            "## Executive Summary",
            "",
            f"This document contains {len(self.discovered_sources)} research sources automatically discovered from existing research documents. Sources were extracted from citations, references, 'future research' sections, and cross-references.",
            "",
            "**Discovery Breakdown:**",
        ]
        
        # Add priority breakdown
        for priority in ['critical', 'high', 'medium', 'low']:
            if priority in by_priority:
                content.append(f"- **{priority.capitalize()}:** {len(by_priority[priority])} sources")
        
        content.extend([
            "",
            "**Categories:**",
        ])
        
        # Add category breakdown
        by_category = defaultdict(list)
        for source in self.discovered_sources:
            by_category[source['category']].append(source)
        
        for category in sorted(by_category.keys()):
            content.append(f"- **{category}:** {len(by_category[category])} sources")
        
        content.extend([
            "",
            "---",
            "",
        ])
        
        # Add sources by priority
        for priority in ['critical', 'high', 'medium', 'low']:
            if priority not in by_priority:
                continue
            
            content.extend([
                f"## {priority.capitalize()} Priority Sources ({len(by_priority[priority])} sources)",
                "",
            ])
            
            for idx, source in enumerate(sorted(by_priority[priority], 
                                               key=lambda x: x['title']), 1):
                content.extend([
                    f"### {idx}. {source['title']}",
                    "",
                    f"**Priority:** {source['priority'].capitalize()}",
                    f"**Category:** {source['category']}",
                    f"**Estimated Effort:** {source['estimated_effort']}",
                    "",
                    f"**Description:**",
                    source['description'],
                    "",
                    f"**Discovered From:**",
                ])
                
                for ref in source['references']:
                    content.append(f"- {ref}")
                
                content.extend([
                    "",
                    "---",
                    "",
                ])
        
        # Add processing queue section
        content.extend([
            "## Processing Queue",
            "",
            "Sources are organized by priority for systematic processing:",
            "",
            "### Critical Priority (Process First)",
        ])
        
        if 'critical' in by_priority:
            for source in by_priority['critical']:
                content.append(f"- [ ] {source['title']} ({source['estimated_effort']})")
        
        content.append("")
        content.append("### High Priority (Process Second)")
        
        if 'high' in by_priority:
            for source in by_priority['high']:
                content.append(f"- [ ] {source['title']} ({source['estimated_effort']})")
        
        content.extend([
            "",
            "### Medium Priority (Process Third)",
        ])
        
        if 'medium' in by_priority:
            for source in by_priority['medium']:
                content.append(f"- [ ] {source['title']} ({source['estimated_effort']})")
        
        content.extend([
            "",
            "---",
            "",
            "## Statistics",
            "",
            f"**Total Sources:** {len(self.discovered_sources)}",
            f"**Total Estimated Effort:** {self._calculate_total_effort()}",
            f"**Unique Categories:** {len(self.categories)}",
            f"**Unique Priorities:** {len(self.priorities)}",
            "",
            "---",
            "",
            "## Next Steps",
            "",
            "1. Review discovered sources for relevance",
            "2. Validate source availability and accessibility",
            "3. Assign sources to appropriate research phases",
            "4. Create assignment groups for critical and high-priority sources",
            "5. Begin systematic processing following batch workflow",
            "",
            "---",
            "",
            f"**Generated:** {datetime.now().strftime('%Y-%m-%d %H:%M:%S')}",
            f"**Tool:** autosources-discovery.py",
            f"**Status:** Ready for Review",
        ])
        
        # Write to file
        with open(output_path, 'w', encoding='utf-8') as f:
            f.write('\n'.join(content))
        
        print(f"✅ Markdown report generated: {output_path}")
        return output_path
    
    def _calculate_total_effort(self) -> str:
        """Calculate total estimated effort across all sources"""
        total_min = 0
        total_max = 0
        
        for source in self.discovered_sources:
            effort = source['estimated_effort']
            match = re.search(r'(\d+)-(\d+)', effort)
            if match:
                total_min += int(match.group(1))
                total_max += int(match.group(2))
        
        return f"{total_min}-{total_max} hours"
    
    def generate_json_report(self, output_file: str = "auto-discovered-sources.json"):
        """Generate a JSON report of discovered sources"""
        output_path = self.research_dir / output_file
        
        report = {
            'generated': datetime.now().isoformat(),
            'total_sources': len(self.discovered_sources),
            'sources': self.discovered_sources,
            'categories': list(self.categories),
            'priorities': list(self.priorities),
        }
        
        with open(output_path, 'w', encoding='utf-8') as f:
            json.dump(report, f, indent=2)
        
        print(f"✅ JSON report generated: {output_path}")
        return output_path


def main():
    """Main execution function"""
    import argparse
    
    parser = argparse.ArgumentParser(
        description='Automated Source Discovery Tool for BlueMarble Research'
    )
    parser.add_argument('--scan-all', action='store_true',
                       help='Scan all existing research documents')
    parser.add_argument('--phase', type=int,
                       help='Focus on Phase N documents only')
    parser.add_argument('--priority', choices=['critical', 'high', 'medium', 'low'],
                       help='Filter by priority level')
    parser.add_argument('--category',
                       help='Filter by category')
    parser.add_argument('--output', default='auto-discovered-sources.md',
                       help='Output file for discovered sources')
    parser.add_argument('--format', choices=['markdown', 'json', 'yaml'],
                       default='markdown',
                       help='Output format')
    
    args = parser.parse_args()
    
    # Initialize discovery engine
    discovery = SourceDiscovery()
    
    # Scan documents
    print("🔍 Starting automated source discovery...")
    discovery.scan_research_documents(phase_filter=args.phase)
    
    print(f"✅ Discovered {len(discovery.discovered_sources)} sources")
    
    # Generate report
    if args.format == 'markdown':
        discovery.generate_markdown_report(args.output)
    elif args.format == 'json':
        discovery.generate_json_report(args.output.replace('.md', '.json'))
    
    print("\n📊 Discovery Summary:")
    print(f"   Total Sources: {len(discovery.discovered_sources)}")
    print(f"   Categories: {', '.join(sorted(discovery.categories))}")
    print(f"   Priorities: {', '.join(sorted(discovery.priorities))}")
    print(f"\n✅ Automated source discovery complete!")


if __name__ == '__main__':
    main()
