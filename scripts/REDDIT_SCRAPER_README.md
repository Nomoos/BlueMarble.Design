# Reddit Story Scraper

## Overview

The Reddit Story Scraper is a tool for mining player experiences, design insights, and community feedback from Reddit to support BlueMarble MMORPG research. It collects and organizes stories, discussions, and feedback from gaming subreddits, particularly r/MMORPG.

## Purpose

This tool automates the collection of valuable research data that previously had to be manually gathered. It helps researchers:

- Discover player pain points and expectations
- Identify successful and failed game features
- Track market trends and community sentiment
- Collect real-world design case studies
- Build a knowledge base of player experiences

## Features

- **Flexible Subreddit Targeting**: Collect from any public subreddit
- **Multiple Sorting Options**: Hot, new, top, or rising posts
- **Time-based Filtering**: Hour, day, week, month, year, or all time
- **Keyword Filtering**: Filter posts by relevant keywords
- **Score Thresholding**: Only collect high-quality, upvoted content
- **Comment Collection**: Optionally include top comments for deeper insights
- **Automatic Categorization**: Posts are categorized (player experience, game design, feedback, etc.)
- **Multiple Output Formats**: JSON (machine-readable) or Markdown (human-readable)

## Installation

The script requires Python 3.7+ and the `requests` library (already included in this repository's environment).

```bash
# Verify installation
python3 scripts/reddit-story-scraper.py --help
```

## Usage

### Basic Usage

Collect top 100 posts from r/MMORPG:

```bash
python3 scripts/reddit-story-scraper.py
```

### Collect from Specific Subreddit

```bash
python3 scripts/reddit-story-scraper.py --subreddit gamedesign --limit 50
```

### Filter by Keywords

Collect posts related to economy and trading:

```bash
python3 scripts/reddit-story-scraper.py --keywords "economy,trading,crafting,marketplace"
```

### Include Comments

Get posts with their top comments for deeper insights:

```bash
python3 scripts/reddit-story-scraper.py --include-comments --limit 50
```

### Get Recent Hot Topics

```bash
python3 scripts/reddit-story-scraper.py --sort hot --timeframe day
```

### Output as Markdown

Generate a human-readable report:

```bash
python3 scripts/reddit-story-scraper.py --format markdown --output research/reddit-insights.md
```

## Command-Line Options

| Option | Description | Default |
|--------|-------------|---------|
| `--subreddit` | Target subreddit name | `MMORPG` |
| `--limit` | Number of posts to fetch | `100` |
| `--timeframe` | Time filter (hour, day, week, month, year, all) | `month` |
| `--sort` | Sort method (hot, new, top, rising) | `top` |
| `--keywords` | Comma-separated keywords to filter | None |
| `--min-score` | Minimum score threshold | `10` |
| `--output` | Output file path | Auto-generated |
| `--format` | Output format (json, markdown) | `json` |
| `--include-comments` | Include top comments | `False` |

## Output Format

### JSON Output

The JSON output includes:
- Metadata (collection timestamp, tool version)
- Array of posts with:
  - Post ID, title, author
  - Score, comment count, upvote ratio
  - Created date
  - URL, text content
  - Flair information
  - Automatic category assignment
  - Top comments (if requested)

Example structure:
```json
{
  "metadata": {
    "collected_at": "2025-01-17T10:30:00",
    "total_posts": 50,
    "tool": "reddit-story-scraper.py",
    "version": "1.0"
  },
  "posts": [
    {
      "id": "abc123",
      "title": "What makes a good MMORPG economy?",
      "author": "username",
      "score": 245,
      "num_comments": 67,
      "created_date": "2025-01-15T14:20:00",
      "url": "https://www.reddit.com/r/MMORPG/comments/...",
      "selftext": "Post content...",
      "category": "economy"
    }
  ]
}
```

### Markdown Output

The Markdown output is organized by category with:
- Document header and metadata
- Posts grouped by category
- For each post:
  - Title, author, score, date
  - Direct link to Reddit
  - Post content (truncated if long)
  - Top comments (if included)

## Post Categories

Posts are automatically categorized based on content:

- **player_experience**: Personal stories and experiences
- **game_design**: Mechanics, features, and design discussions
- **feedback**: Problems, frustrations, and suggestions
- **recommendation**: Game recommendations and searches
- **technical**: Performance, lag, and technical issues
- **community**: Guild, clan, and social dynamics
- **economy**: Trading, markets, and in-game economies
- **general**: Uncategorized posts

## Use Cases

### 1. Market Research

Understand what players want in MMORPGs:

```bash
python3 scripts/reddit-story-scraper.py \
  --keywords "looking for,recommend,worth playing" \
  --min-score 20 \
  --limit 200
```

### 2. Feature Validation

Check community sentiment on specific features:

```bash
python3 scripts/reddit-story-scraper.py \
  --keywords "housing,crafting,economy" \
  --sort top \
  --timeframe year
```

### 3. Pain Point Discovery

Find common player frustrations:

```bash
python3 scripts/reddit-story-scraper.py \
  --keywords "frustrating,hate,problem,issue" \
  --include-comments
```

### 4. Success Case Studies

Learn from successful games:

```bash
python3 scripts/reddit-story-scraper.py \
  --keywords "FFXIV,EVE Online,WoW,success" \
  --min-score 50
```

### 5. Multiple Subreddit Collection

Collect from related subreddits:

```bash
# r/MMORPG
python3 scripts/reddit-story-scraper.py --subreddit MMORPG --limit 100

# r/gamedesign
python3 scripts/reddit-story-scraper.py --subreddit gamedesign --limit 100

# r/truegaming
python3 scripts/reddit-story-scraper.py --subreddit truegaming --limit 100
```

## Best Practices

### Rate Limiting

The script automatically includes delays between requests to respect Reddit's API guidelines:
- 1 second between post page requests
- 1 second between comment requests

### API Usage

This tool uses Reddit's public JSON API, which:
- Does not require authentication
- Is available for public posts
- Has rate limits (handled automatically)
- Should be used responsibly

### Data Quality

For best results:
- Use `--min-score` to filter low-quality posts
- Target specific subreddits relevant to your research
- Use keywords to focus on specific topics
- Collect comments only when needed (slower but more insights)

### Storage and Organization

Recommended file organization:
```
research/
  reddit-data/
    MMORPG/
      reddit-stories-MMORPG-20250117.json
      reddit-stories-MMORPG-economy-20250117.json
    gamedesign/
      reddit-stories-gamedesign-20250117.json
```

## Integration with Research

### Manual Analysis

Use Markdown output for quick reading and manual analysis:

```bash
python3 scripts/reddit-story-scraper.py \
  --format markdown \
  --output research/literature/reddit-stories-mmorpg-$(date +%Y%m%d).md
```

### Automated Processing

Use JSON output for further processing with other tools:

```python
import json

with open('reddit-stories.json') as f:
    data = json.load(f)
    
# Analyze trends
categories = {}
for post in data['posts']:
    cat = post['category']
    categories[cat] = categories.get(cat, 0) + 1

print(categories)
```

### Cross-referencing

Reference collected stories in research documents:

```markdown
**Source:** Reddit r/MMORPG (collected 2025-01-17)
**Post:** "What makes EVE Online's economy work?"
**Score:** 450+ upvotes
**Key Insight:** Player-driven markets require meaningful crafting and destruction
```

## Troubleshooting

### Connection Errors

If you encounter connection errors:
- Check your internet connection
- Reddit may be temporarily unavailable
- Rate limiting may be in effect (wait a few minutes)

### No Posts Returned

If no posts are returned:
- Check that the subreddit name is correct
- Try lowering `--min-score`
- Try a different `--timeframe`
- Remove or adjust `--keywords`

### Incomplete Data

If some fields are missing:
- This is normal; not all posts have all fields
- The script handles missing data gracefully

## Limitations

- Only accesses public posts (no private subreddits)
- Cannot access deleted or removed content
- Limited to Reddit's public API capabilities
- No real-time streaming (use for historical data)

## Related Tools

- `process-wiki-sources.py`: Process Wikipedia sources for research
- `autosources-discovery.py`: Discover new research sources
- `generate-research-issues.py`: Generate research task issues

## Future Enhancements

Potential improvements for future versions:
- Sentiment analysis on collected posts
- Trend detection over time
- Multi-subreddit collection in single run
- Export to other formats (CSV, database)
- Integration with research document generation

## Examples

### Comprehensive MMORPG Research Collection

```bash
# Top posts of the month
python3 scripts/reddit-story-scraper.py \
  --subreddit MMORPG \
  --sort top \
  --timeframe month \
  --limit 100 \
  --min-score 50 \
  --output research/reddit-data/mmorpg-top-month.json

# Recent hot discussions
python3 scripts/reddit-story-scraper.py \
  --subreddit MMORPG \
  --sort hot \
  --limit 50 \
  --include-comments \
  --output research/reddit-data/mmorpg-hot-discussions.json

# Economy-focused posts
python3 scripts/reddit-story-scraper.py \
  --subreddit MMORPG \
  --keywords "economy,trading,market,gold,crafting" \
  --timeframe year \
  --limit 200 \
  --format markdown \
  --output research/reddit-data/mmorpg-economy-insights.md
```

## Support

For issues or questions:
1. Check this README for common solutions
2. Review the script's help: `python3 scripts/reddit-story-scraper.py --help`
3. Check related documentation in `scripts/README.md`

## Version History

- **1.0** (2025-01-17): Initial release
  - Basic post collection
  - Keyword filtering
  - Comment collection
  - Automatic categorization
  - JSON and Markdown output
