#!/usr/bin/env python3
"""
Wikipedia Source Processor for BlueMarble Research
===================================================

Processes Wikipedia URLs and generates BibTeX entries for sources.bib

Usage:
    python process-wiki-sources.py <url1> <url2> ...
    
Or use the embedded list of URLs to process.
"""

import re
import sys
import requests
from urllib.parse import unquote, urlparse
from datetime import datetime


def extract_wiki_title(url):
    """Extract the page title from Wikipedia URL"""
    parsed = urlparse(url)
    path = parsed.path
    
    # Extract title from path (e.g., /wiki/Heat -> Heat)
    match = re.search(r'/wiki/(.+)', path)
    if match:
        title = match.group(1)
        # Decode URL encoding
        title = unquote(title)
        # Replace underscores with spaces
        title = title.replace('_', ' ')
        return title
    return None


def generate_bibtex_key(title):
    """Generate a BibTeX key from title"""
    # Remove special characters and convert to lowercase
    key = title.lower()
    # Replace spaces and special chars with underscores
    key = re.sub(r'[^\w\s-]', '', key)
    key = re.sub(r'[-\s]+', '_', key)
    # Remove Wikipedia namespace prefixes
    key = re.sub(r'^wikipedia:', '', key)
    return f"wiki_{key}"


def fetch_wiki_summary(url):
    """Fetch the first paragraph/summary from Wikipedia page"""
    try:
        # Try to fetch the page
        response = requests.get(url, timeout=10)
        response.raise_for_status()
        
        html = response.text
        
        # Extract first paragraph from content
        # Look for the main content area
        match = re.search(r'<p[^>]*>(.*?)</p>', html, re.DOTALL)
        if match:
            text = match.group(1)
            # Remove HTML tags
            text = re.sub(r'<[^>]+>', '', text)
            # Remove reference markers like [1], [2], etc.
            text = re.sub(r'\[\d+\]', '', text)
            # Clean up whitespace
            text = ' '.join(text.split())
            return text[:200] + '...' if len(text) > 200 else text
    except Exception as e:
        print(f"Warning: Could not fetch summary from {url}: {e}")
    
    return None


def generate_bluemarble_note(title, url):
    """Generate BlueMarble-specific note for the source"""
    title_lower = title.lower()
    
    # Map topics to BlueMarble-relevant descriptions
    notes = {
        'hydrological': 'Hydrological modeling for water system simulation and environmental mechanics',
        'fick': 'Diffusion laws for material transport, gas exchange, and chemical processes',
        'heat': 'Heat transfer principles for temperature simulation, smelting, and energy systems',
        'categorization': 'Knowledge organization and taxonomy systems for in-game knowledge bases',
        'biologick': 'Biological classification systems for flora and fauna taxonomy',
        'systematika': 'Systematic classification for organizing game world biology and ecosystems',
    }
    
    # Check for matching keywords
    for keyword, note in notes.items():
        if keyword in title_lower:
            return note
    
    # Default note based on general topic detection
    if 'model' in title_lower or 'simulation' in title_lower:
        return f'{title} for game world simulation and mechanics'
    elif 'law' in title_lower or 'equation' in title_lower:
        return f'{title} for physics simulation and game mechanics'
    else:
        return f'{title} for game design and world building reference'


def create_bibtex_entry(url):
    """Create a BibTeX entry for a Wikipedia URL"""
    title = extract_wiki_title(url)
    if not title:
        print(f"Error: Could not extract title from {url}")
        return None
    
    key = generate_bibtex_key(title)
    note = generate_bluemarble_note(title, url)
    year = datetime.now().year
    
    entry = f"""@misc{{{key},
  title = {{{title}}},
  author = {{{{Wikipedia contributors}}}},
  year = {{{year}}},
  url = {{{url}}},
  note = {{{note}}}
}}"""
    
    return {
        'key': key,
        'title': title,
        'entry': entry,
        'url': url
    }


def main():
    """Main execution function"""
    # URLs to process from the problem statement
    urls = [
        'https://en.wikipedia.org/wiki/Hydrological_model',
        'https://en.wikipedia.org/wiki/Fick%27s_laws_of_diffusion',
        'https://en.wikipedia.org/wiki/Heat',
        'https://en.wikipedia.org/wiki/Wikipedia:Categorization',
        'https://cs.wikipedia.org/wiki/Biologick%C3%A1_systematika',
    ]
    
    # Allow URLs to be passed as command line arguments
    if len(sys.argv) > 1:
        urls = sys.argv[1:]
    
    print("Processing Wikipedia URLs...")
    print("=" * 60)
    
    entries = []
    for url in urls:
        print(f"\nProcessing: {url}")
        entry_data = create_bibtex_entry(url)
        if entry_data:
            entries.append(entry_data)
            print(f"  Title: {entry_data['title']}")
            print(f"  Key: {entry_data['key']}")
    
    print("\n" + "=" * 60)
    print("Generated BibTeX Entries:")
    print("=" * 60)
    
    for entry_data in entries:
        print(f"\n{entry_data['entry']}\n")
    
    return entries


if __name__ == '__main__':
    main()
