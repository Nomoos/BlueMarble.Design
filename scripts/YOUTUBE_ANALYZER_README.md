# YouTube Channel Analyzer

A tool for analyzing YouTube channels with a focus on **story videos** (regular video content), excluding YouTube Shorts, live streams, and other content types.

## Overview

This tool helps the BlueMarble research team analyze YouTube channels mentioned in research documents to understand:
- Channel content quality and quantity
- Video frequency and engagement
- Focus on educational/story content vs. shorts and live streams

## Key Features

### 1. Story Video Focus
The analyzer specifically filters for **story videos** (traditional long-form content):
- ✅ Includes: Regular uploaded videos (> 60 seconds)
- ❌ Excludes: YouTube Shorts (≤ 60 seconds)
- ❌ Excludes: Live streams
- ❌ Excludes: Premieres (optional)

### 2. Dual Operation Modes

**Basic Mode (No API Key Required):**
- Web scraping approach
- Basic channel information
- Limited data availability
- Good for quick checks

**API Mode (YouTube Data API v3 Key Required):**
- Detailed channel statistics
- Video-level analysis
- Accurate filtering of shorts vs. story videos
- Engagement metrics (views, likes, comments)
- Duration analysis

### 3. Batch Analysis
- Extract and analyze all channels from markdown documents
- Generate comprehensive reports
- Export results in JSON or Markdown format

## Installation

### Basic Requirements
```bash
pip install requests beautifulsoup4
```

### For Full API-Based Analysis
```bash
pip install requests beautifulsoup4 google-api-python-client
```

### Get YouTube Data API Key
1. Go to [Google Cloud Console](https://console.cloud.google.com/)
2. Create a new project or select existing one
3. Enable "YouTube Data API v3"
4. Create credentials (API Key)
5. Copy the API key for use with the tool

## Usage

### Analyze Single Channel (Basic)
```bash
python scripts/youtube-channel-analyzer.py --channel @SebastianLague
```

### Analyze Single Channel (With API)
```bash
python scripts/youtube-channel-analyzer.py --channel @SebastianLague --api-key YOUR_API_KEY
```

### Analyze Channel from URL
```bash
python scripts/youtube-channel-analyzer.py --channel-url https://www.youtube.com/@CodeMonkeyUnity --api-key YOUR_API_KEY
```

### Analyze All Channels from Document
```bash
python scripts/youtube-channel-analyzer.py \
  --analyze-all-from-doc research/literature/online-game-dev-resources.md \
  --output youtube-analysis-report.md
```

### Export Results to JSON
```bash
python scripts/youtube-channel-analyzer.py \
  --channel @Brackeys \
  --api-key YOUR_API_KEY \
  --output analysis-brackeys.json
```

## Output Format

### Markdown Report Example
```markdown
# YouTube Channel Analysis Report

**Source Document:** research/literature/online-game-dev-resources.md
**Analyzed At:** 2025-01-20T10:30:00
**Total Channels:** 5

---

## Analysis Focus: Story Videos Only

This analysis focuses on **story videos** (regular uploaded content) and excludes:
- YouTube Shorts (videos ≤ 60 seconds)
- Live streams
- Premieres

---

## 1. Sebastian Lague

**Channel Handle:** @SebastianLague
**Channel URL:** https://www.youtube.com/@SebastianLague
**Subscribers:** 1,240,000
**Total Videos:** 156
**Story Videos Analyzed:** 45
**Shorts Excluded:** 10
**Live Streams Excluded:** 2

### Story Video Statistics

- **Total Story Videos:** 45
- **Average Views:** 450,000
- **Average Duration:** 18.5 minutes
- **Total Views:** 20,250,000

**Analysis Method:** youtube_data_api_v3
```

### JSON Output Structure
```json
{
  "source_document": "research/literature/online-game-dev-resources.md",
  "analyzed_at": "2025-01-20T10:30:00",
  "total_channels": 5,
  "channels": [
    {
      "channel_handle": "@SebastianLague",
      "channel_id": "UCmtyQOKKmrMVaKuRXz02jbQ",
      "channel_name": "Sebastian Lague",
      "subscriber_count": 1240000,
      "total_video_count": 156,
      "story_videos_analyzed": 45,
      "shorts_excluded": 10,
      "live_streams_excluded": 2,
      "story_videos_stats": {
        "total_story_videos": 45,
        "average_views": 450000,
        "average_duration_minutes": 18.5,
        "total_views": 20250000
      },
      "filters_applied": [
        "Duration > 60 seconds (excludes Shorts)",
        "Excludes live broadcasts",
        "Focuses on uploaded video content"
      ]
    }
  ]
}
```

## Why Focus on Story Videos?

For research purposes, **story videos** (traditional long-form content) are more valuable because:

1. **Educational Depth**: Story videos typically contain in-depth tutorials and explanations
2. **Technical Content**: Game development concepts require longer format to explain properly
3. **Reference Value**: Long-form content is easier to reference and learn from
4. **Quality Signal**: Channels focusing on story videos show commitment to educational content

YouTube Shorts, while popular, are generally:
- Too brief for technical explanations
- Entertainment-focused rather than educational
- Harder to use as learning resources
- Often promotional or teaser content

## Technical Details

### Video Classification Logic

```python
# A video is classified as a "story video" if:
1. Duration > 60 seconds (excludes Shorts)
2. Not a live stream (liveBroadcastContent != 'live')
3. Not a premiere in progress
4. Regular uploaded video content
```

### API Quota Considerations

The YouTube Data API v3 has quota limits:
- Default: 10,000 units per day
- Channel details: ~5 units
- Video details: 1 unit per video
- Analyzing 50 videos per channel: ~55 units

**Tip**: For large-scale analysis, consider spreading across multiple days or requesting quota increase.

## Integration with Research Workflow

### Step 1: Extract Channels
The tool automatically extracts channel references from markdown documents:
```markdown
#### 22. **Sebastian Lague**
- **URL:** https://www.youtube.com/@SebastianLague
- **Status:** ⏳ Pending Review
```

### Step 2: Analyze Channels
Run batch analysis on all channels in the document.

### Step 3: Review Results
Generated reports include:
- Channel statistics
- Story video focus metrics
- Content quality indicators

### Step 4: Update Research Documents
Use analysis results to:
- Prioritize channels for manual review
- Identify most relevant content creators
- Track channel activity and content output

## Examples

### Example 1: Quick Channel Check
```bash
# Check if a channel is still active and posting story videos
python scripts/youtube-channel-analyzer.py --channel @Brackeys
```

### Example 2: Detailed Analysis with Export
```bash
# Full analysis with API, export to JSON for processing
python scripts/youtube-channel-analyzer.py \
  --channel @CodeMonkeyUnity \
  --api-key $YOUTUBE_API_KEY \
  --output data/code-monkey-analysis.json
```

### Example 3: Batch Analysis for Research Document
```bash
# Analyze all channels mentioned in research
python scripts/youtube-channel-analyzer.py \
  --analyze-all-from-doc research/literature/online-game-dev-resources.md \
  --api-key $YOUTUBE_API_KEY \
  --output research/literature/youtube-channels-analysis.md
```

## Troubleshooting

### Issue: "Scraping libraries not available"
**Solution**: Install required packages
```bash
pip install requests beautifulsoup4
```

### Issue: "YouTube API not initialized"
**Solution**: Provide API key with `--api-key` argument

### Issue: "Channel not found"
**Solution**: Verify the channel handle is correct (e.g., @ChannelName)

### Issue: API Quota Exceeded
**Solution**: 
- Wait for quota reset (daily)
- Use basic mode without API key
- Request quota increase from Google Cloud Console

## Future Enhancements

Potential improvements for future versions:
- [ ] Video content categorization (tutorial vs. devlog vs. showcase)
- [ ] Playlist analysis
- [ ] Upload frequency trends
- [ ] Topic extraction from video titles/descriptions
- [ ] Relevance scoring for BlueMarble research topics
- [ ] Integration with research issue tracking
- [ ] Automated periodic re-analysis

## Related Tools

- `autosources-discovery.py`: Discovers sources from research documents
- `process-wiki-sources.py`: Processes Wikipedia references
- `generate-research-issues.py`: Creates research tracking issues

## References

- [YouTube Data API v3 Documentation](https://developers.google.com/youtube/v3)
- [YouTube Content Classification](https://support.google.com/youtube/answer/9315446)
- Research Document: `research/literature/online-game-dev-resources.md`

---

**Last Updated:** 2025-01-20
**Maintainer:** BlueMarble Research Team
