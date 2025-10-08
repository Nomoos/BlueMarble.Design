# Reddit Story Scraper - Example Usage Guide

This guide provides practical examples of using the Reddit story scraper for BlueMarble MMORPG research.

## Getting Started

### Quick Test

First, verify the tool works with test mode (no network required):

```bash
# Basic test
python3 scripts/reddit-story-scraper.py --test-mode --limit 5

# Test with markdown output
python3 scripts/reddit-story-scraper.py --test-mode --limit 5 --format markdown --output /tmp/test.md
```

### First Real Collection

When ready to collect real data from Reddit:

```bash
# Collect top 50 posts from r/MMORPG
python3 scripts/reddit-story-scraper.py --limit 50
```

## Research Scenarios

### Scenario 1: Understanding Player Frustrations

**Goal:** Identify common pain points in modern MMORPGs

```bash
python3 scripts/reddit-story-scraper.py \
  --keywords "frustrating,hate,problem,annoying,broken,terrible" \
  --timeframe year \
  --limit 100 \
  --min-score 20 \
  --output research/reddit-data/player-frustrations-$(date +%Y%m%d).json
```

**What this does:**
- Collects posts mentioning frustrations
- From the past year
- Only posts with 20+ score (quality filter)
- Saves to organized directory

**Follow-up analysis:**
```bash
# Generate readable report
python3 scripts/reddit-story-scraper.py \
  --keywords "frustrating,hate,problem,annoying,broken,terrible" \
  --timeframe year \
  --limit 100 \
  --min-score 20 \
  --format markdown \
  --include-comments \
  --output research/reddit-data/player-frustrations-report.md
```

### Scenario 2: Crafting System Research

**Goal:** Learn what players want in crafting systems

```bash
# Phase 1: Collect posts about crafting
python3 scripts/reddit-story-scraper.py \
  --keywords "crafting,gathering,recipes,materials,smithing" \
  --sort top \
  --timeframe all \
  --limit 200 \
  --min-score 50 \
  --output research/reddit-data/crafting-systems.json

# Phase 2: Get detailed discussions with comments
python3 scripts/reddit-story-scraper.py \
  --keywords "crafting system,crafting mechanics" \
  --sort top \
  --timeframe year \
  --limit 50 \
  --include-comments \
  --format markdown \
  --output research/literature/reddit-crafting-insights.md
```

### Scenario 3: Economy Design Research

**Goal:** Study successful and failed MMORPG economies

```bash
# Collect economy discussions
python3 scripts/reddit-story-scraper.py \
  --subreddit MMORPG \
  --keywords "economy,trading,market,inflation,gold,auction house" \
  --sort top \
  --timeframe year \
  --limit 150 \
  --min-score 25 \
  --output research/reddit-data/economy-discussions.json

# Also check r/gamedesign
python3 scripts/reddit-story-scraper.py \
  --subreddit gamedesign \
  --keywords "economy,virtual economy,game economy" \
  --sort top \
  --timeframe year \
  --limit 100 \
  --output research/reddit-data/economy-gamedesign.json
```

### Scenario 4: Success Case Studies

**Goal:** Learn from successful games (FFXIV, EVE, WoW)

```bash
# FFXIV success stories
python3 scripts/reddit-story-scraper.py \
  --keywords "FFXIV,Final Fantasy XIV,FF14,why FFXIV" \
  --sort top \
  --timeframe year \
  --limit 100 \
  --min-score 50 \
  --include-comments \
  --output research/reddit-data/ffxiv-success.json

# EVE Online economy lessons
python3 scripts/reddit-story-scraper.py \
  --keywords "EVE Online,EVE economy,EVE market" \
  --sort top \
  --timeframe all \
  --limit 100 \
  --min-score 40 \
  --output research/reddit-data/eve-economy.json

# General success factors
python3 scripts/reddit-story-scraper.py \
  --keywords "why successful,what makes,good MMORPG,great MMORPG" \
  --sort top \
  --timeframe year \
  --limit 150 \
  --output research/reddit-data/success-factors.json
```

### Scenario 5: New Player Experience

**Goal:** Understand new player onboarding challenges

```bash
python3 scripts/reddit-story-scraper.py \
  --keywords "new player,newbie,beginner,started playing,first time" \
  --sort top \
  --timeframe month \
  --limit 100 \
  --min-score 15 \
  --include-comments \
  --format markdown \
  --output research/reddit-data/new-player-experience.md
```

### Scenario 6: Social Features and Guilds

**Goal:** Research social mechanics and guild systems

```bash
# Guild management
python3 scripts/reddit-story-scraper.py \
  --keywords "guild,clan,guild management,guild tools,guild system" \
  --timeframe year \
  --limit 100 \
  --min-score 20 \
  --output research/reddit-data/guild-systems.json

# Social features
python3 scripts/reddit-story-scraper.py \
  --keywords "social,friends,community,grouping,party system" \
  --timeframe year \
  --limit 100 \
  --min-score 20 \
  --output research/reddit-data/social-features.json
```

### Scenario 7: PvP vs PvE Preferences

**Goal:** Understand player preferences for combat systems

```bash
# PvP discussions
python3 scripts/reddit-story-scraper.py \
  --keywords "PvP,player vs player,pvp system,open world pvp" \
  --timeframe year \
  --limit 100 \
  --min-score 25 \
  --output research/reddit-data/pvp-preferences.json

# PvE discussions
python3 scripts/reddit-story-scraper.py \
  --keywords "PvE,dungeons,raids,endgame content" \
  --timeframe year \
  --limit 100 \
  --min-score 25 \
  --output research/reddit-data/pve-preferences.json
```

### Scenario 8: Housing and Player Creativity

**Goal:** Research player housing systems

```bash
python3 scripts/reddit-story-scraper.py \
  --keywords "housing,player housing,home,decoration,customization" \
  --timeframe all \
  --limit 150 \
  --min-score 30 \
  --include-comments \
  --format markdown \
  --output research/reddit-data/housing-systems.md
```

### Scenario 9: Monetization and Fair Play

**Goal:** Understand player attitudes toward monetization

```bash
python3 scripts/reddit-story-scraper.py \
  --keywords "pay to win,p2w,monetization,cash shop,fair,cosmetic" \
  --timeframe year \
  --limit 150 \
  --min-score 30 \
  --output research/reddit-data/monetization-attitudes.json
```

### Scenario 10: Technical Performance

**Goal:** Learn about technical issues and expectations

```bash
python3 scripts/reddit-story-scraper.py \
  --keywords "performance,optimization,lag,fps,server,latency" \
  --timeframe month \
  --limit 100 \
  --min-score 15 \
  --output research/reddit-data/technical-performance.json
```

## Multi-Subreddit Research

### Comprehensive Game Design Research

Collect from multiple relevant subreddits:

```bash
# Create directory
mkdir -p research/reddit-data/multi-subreddit-$(date +%Y%m%d)

# r/MMORPG - Player perspective
python3 scripts/reddit-story-scraper.py \
  --subreddit MMORPG \
  --limit 100 \
  --output research/reddit-data/multi-subreddit-$(date +%Y%m%d)/mmorpg.json

# r/gamedesign - Design perspective
python3 scripts/reddit-story-scraper.py \
  --subreddit gamedesign \
  --limit 100 \
  --output research/reddit-data/multi-subreddit-$(date +%Y%m%d)/gamedesign.json

# r/truegaming - Deep analysis
python3 scripts/reddit-story-scraper.py \
  --subreddit truegaming \
  --limit 100 \
  --output research/reddit-data/multi-subreddit-$(date +%Y%m%d)/truegaming.json

# r/gamedev - Developer insights
python3 scripts/reddit-story-scraper.py \
  --subreddit gamedev \
  --keywords "MMORPG,multiplayer,online game" \
  --limit 50 \
  --output research/reddit-data/multi-subreddit-$(date +%Y%m%d)/gamedev.json
```

## Data Analysis Workflow

### Step 1: Collection

```bash
# Broad collection
python3 scripts/reddit-story-scraper.py \
  --limit 200 \
  --min-score 20 \
  --output research/reddit-data/broad-collection.json
```

### Step 2: Focused Deep Dive

```bash
# Analyze broad collection, then dive deeper into interesting topics
python3 scripts/reddit-story-scraper.py \
  --keywords "specific,topic,from,analysis" \
  --limit 100 \
  --include-comments \
  --format markdown \
  --output research/reddit-data/focused-deep-dive.md
```

### Step 3: Integration with Research Documents

Reference collected data in research documents:

```markdown
## Player Feedback on Crafting Systems

**Data Source:** Reddit r/MMORPG Analysis (Collected 2025-01-17)  
**Collection File:** `research/reddit-data/crafting-systems.json`  
**Sample Size:** 200 posts with 50+ score

**Key Findings:**

1. **Complexity vs Accessibility** (58 posts mentioned)
   - Players want depth but not tedious grinding
   - Example: "EVE Online's manufacturing is too complex for casuals" (Score: 234)

2. **Economic Impact** (43 posts mentioned)
   - Crafting must matter to the economy
   - Example: "WoW Classic crafting actually matters" (Score: 456)

[Reference: reddit-stories-MMORPG-20250117.json, posts #23, #45, #67]
```

## Automated Collection Scripts

Create bash scripts for regular collection:

### weekly-collection.sh

```bash
#!/bin/bash
# Weekly collection of hot topics

DATE=$(date +%Y%m%d)
DIR="research/reddit-data/weekly-$DATE"
mkdir -p "$DIR"

echo "Collecting weekly Reddit insights..."

# Hot topics
python3 scripts/reddit-story-scraper.py \
  --sort hot \
  --timeframe week \
  --limit 50 \
  --min-score 20 \
  --output "$DIR/hot-topics.json"

# Top discussions
python3 scripts/reddit-story-scraper.py \
  --sort top \
  --timeframe week \
  --limit 50 \
  --min-score 50 \
  --output "$DIR/top-discussions.json"

echo "Collection complete! Check $DIR"
```

### monthly-deep-dive.sh

```bash
#!/bin/bash
# Monthly deep dive with comments

DATE=$(date +%Y%m%d)
DIR="research/reddit-data/monthly-$DATE"
mkdir -p "$DIR"

echo "Performing monthly deep dive..."

# Top posts with comments
python3 scripts/reddit-story-scraper.py \
  --sort top \
  --timeframe month \
  --limit 100 \
  --min-score 100 \
  --include-comments \
  --format markdown \
  --output "$DIR/monthly-insights.md"

echo "Deep dive complete! Check $DIR"
```

## Best Practices

### 1. Start Broad, Then Focus

```bash
# First pass: broad collection
python3 scripts/reddit-story-scraper.py --limit 200

# Analyze results, identify themes

# Second pass: focused collection
python3 scripts/reddit-story-scraper.py --keywords "identified,themes" --include-comments
```

### 2. Use Appropriate Score Thresholds

- `--min-score 10`: General exploration
- `--min-score 50`: Quality discussions
- `--min-score 100`: Highly valued content
- `--min-score 500`: Exceptional posts only

### 3. Balance Collection Size

- Small (10-50): Quick insights, testing
- Medium (50-150): Standard research
- Large (150-300): Comprehensive analysis
- Very Large (300+): Long-term tracking

### 4. Organize Your Data

```
research/reddit-data/
├── by-topic/
│   ├── crafting/
│   ├── economy/
│   └── social/
├── by-date/
│   ├── 2025-01/
│   └── 2025-02/
└── reports/
    └── monthly-insights.md
```

### 5. Document Your Collections

Create `research/reddit-data/README.md`:

```markdown
# Reddit Data Collections

## 2025-01-17: Initial MMORPG Research
- Collection: crafting-systems.json
- Posts: 200
- Timeframe: Past year
- Key findings: [summary]

## 2025-01-20: Economy Deep Dive
- Collection: economy-discussions.json
- Posts: 150
- Key findings: [summary]
```

## Troubleshooting

### Rate Limiting

If you get rate limited:
```bash
# Split into smaller batches with delays
for i in {1..5}; do
  python3 scripts/reddit-story-scraper.py --limit 50
  sleep 300  # 5-minute delay
done
```

### Network Issues

Use test mode for development:
```bash
# Develop and test locally
python3 scripts/reddit-story-scraper.py --test-mode --limit 20

# Then run real collection when ready
python3 scripts/reddit-story-scraper.py --limit 20
```

### No Matching Posts

Try broader keywords or lower score threshold:
```bash
# Too specific
python3 scripts/reddit-story-scraper.py --keywords "very,specific,thing" --min-score 100
# Result: 0 posts

# Broader approach
python3 scripts/reddit-story-scraper.py --keywords "broader,terms" --min-score 20
# Result: 45 posts
```

## Integration Examples

### Python Analysis

```python
import json
from collections import Counter

# Load collected data
with open('research/reddit-data/crafting-systems.json') as f:
    data = json.load(f)

# Analyze categories
categories = Counter(post['category'] for post in data['posts'])
print(f"Category distribution: {categories}")

# Find highest-scoring posts
top_posts = sorted(data['posts'], key=lambda x: x['score'], reverse=True)[:10]
for post in top_posts:
    print(f"{post['score']:4d} | {post['title']}")
```

### Generate Research Summary

```python
def generate_summary(json_file, output_md):
    with open(json_file) as f:
        data = json.load(f)
    
    with open(output_md, 'w') as f:
        f.write(f"# Reddit Research Summary\n\n")
        f.write(f"**Collected:** {data['metadata']['collected_at']}\n")
        f.write(f"**Total Posts:** {data['metadata']['total_posts']}\n\n")
        
        # Categorize
        categories = {}
        for post in data['posts']:
            cat = post['category']
            if cat not in categories:
                categories[cat] = []
            categories[cat].append(post)
        
        # Write each category
        for cat, posts in categories.items():
            f.write(f"## {cat.title()}\n\n")
            for post in posts[:5]:  # Top 5 per category
                f.write(f"- [{post['score']}] {post['title']}\n")
            f.write("\n")

generate_summary('broad-collection.json', 'summary-report.md')
```

## Next Steps

After collecting data:

1. **Analyze** - Look for patterns and themes
2. **Categorize** - Organize by topic
3. **Extract** - Pull out key insights
4. **Document** - Add findings to research documents
5. **Iterate** - Collect more focused data based on findings

## Related Documentation

- [REDDIT_SCRAPER_README.md](REDDIT_SCRAPER_README.md) - Complete tool documentation
- [scripts/README.md](README.md) - All available scripts
- [research/literature/game-dev-analysis-reddit---r-mmorpg.md](../research/literature/game-dev-analysis-reddit---r-mmorpg.md) - Manual Reddit analysis example
