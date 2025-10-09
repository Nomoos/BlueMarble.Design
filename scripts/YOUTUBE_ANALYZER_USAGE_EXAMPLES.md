# YouTube Channel Analyzer - Usage Examples

This document provides practical examples of using the YouTube Channel Analyzer tool to analyze game development channels for the BlueMarble project.

## Quick Start Examples

### Example 1: Basic Channel Analysis (No API Key)

Analyze a single channel using basic scraping (limited information):

```bash
cd /path/to/BlueMarble.Design
python3 scripts/youtube-channel-analyzer.py --channel @SebastianLague
```

**What You Get:**
- Channel handle and URL
- Note about limited data without API key
- List of filters applied (story videos only)

**Output:**
```json
{
  "channel_handle": "@SebastianLague",
  "channel_url": "https://www.youtube.com/@SebastianLague",
  "analyzed_at": "2025-01-20T10:30:00",
  "analysis_method": "basic_scraping",
  "note": "Limited data available without API key.",
  "story_videos_focus": true,
  "filters_applied": [
    "Excludes Shorts (videos under 60 seconds)",
    "Excludes Live Streams",
    "Excludes Premieres",
    "Focuses on regular uploaded videos"
  ]
}
```

### Example 2: Detailed API-Based Analysis

For comprehensive analysis, use YouTube Data API v3:

**Step 1: Get API Key**
1. Visit [Google Cloud Console](https://console.cloud.google.com/)
2. Create/select project
3. Enable "YouTube Data API v3"
4. Create API credentials
5. Copy the API key

**Step 2: Run Analysis**
```bash
python3 scripts/youtube-channel-analyzer.py \
  --channel @CodeMonkeyUnity \
  --api-key AIzaSyD...your_key_here... \
  --output code-monkey-analysis.json
```

**What You Get:**
- Full channel statistics (subscribers, total videos)
- Story video count and metrics
- Shorts and live streams excluded count
- Average views, likes, comments
- Duration analysis
- Recent video details

**Output:**
```json
{
  "channel_handle": "@CodeMonkeyUnity",
  "channel_id": "UCFK6NCbuCIVzA6Yj1G_ZqCg",
  "channel_name": "Code Monkey",
  "subscriber_count": 820000,
  "total_video_count": 486,
  "story_videos_analyzed": 50,
  "shorts_excluded": 124,
  "live_streams_excluded": 15,
  "story_videos_stats": {
    "total_story_videos": 347,
    "average_views": 145000,
    "average_duration_minutes": 12.3,
    "total_views": 50315000
  }
}
```

### Example 3: Batch Analysis from Document

Analyze all YouTube channels mentioned in a research document:

```bash
python3 scripts/youtube-channel-analyzer.py \
  --analyze-all-from-doc research/literature/online-game-dev-resources.md \
  --api-key YOUR_API_KEY \
  --output research/literature/youtube-channels-analysis.md
```

**What Happens:**
1. Script extracts all YouTube channel URLs from the markdown
2. Analyzes each channel sequentially
3. Generates comprehensive markdown report
4. Saves to specified output file

**Sample Output:**
- Full analysis report for 5 channels
- Story video statistics for each
- Priority assessment
- Recommendations for research use

## Platform-Specific Instructions

### Windows (PowerShell)

```powershell
# Navigate to repository
cd C:\Projects\BlueMarble.Design

# Set API key as environment variable
$env:YOUTUBE_API_KEY = "AIzaSyD...your_key..."

# Run analysis
python scripts/youtube-channel-analyzer.py `
  --channel @SebastianLague `
  --api-key $env:YOUTUBE_API_KEY `
  --output analysis-results.json
```

### Linux / macOS (Bash)

```bash
# Navigate to repository
cd ~/Projects/BlueMarble.Design

# Set API key as environment variable
export YOUTUBE_API_KEY="AIzaSyD...your_key..."

# Run analysis
python3 scripts/youtube-channel-analyzer.py \
  --channel @SebastianLague \
  --api-key "$YOUTUBE_API_KEY" \
  --output analysis-results.json
```

### Windows (Git Bash)

```bash
# Navigate to repository
cd /c/Projects/BlueMarble.Design

# Set API key
export YOUTUBE_API_KEY="AIzaSyD...your_key..."

# Run with python3 or python
python3 scripts/youtube-channel-analyzer.py \
  --channel @SebastianLague \
  --api-key "$YOUTUBE_API_KEY"
```

## Common Use Cases

### Use Case 1: Quick Channel Check

**Scenario:** You found a new channel and want to quickly check if it's worth deeper analysis.

```bash
# Quick check without API key
python3 scripts/youtube-channel-analyzer.py --channel @NewGameDevChannel
```

**Decision Making:**
- If basic info looks good, do detailed analysis with API key
- Add to research document if relevant

### Use Case 2: Periodic Review of Research Channels

**Scenario:** Monthly review of all channels in research document to track activity.

```bash
# Create dated report
TODAY=$(date +%Y-%m-%d)
python3 scripts/youtube-channel-analyzer.py \
  --analyze-all-from-doc research/literature/online-game-dev-resources.md \
  --api-key $YOUTUBE_API_KEY \
  --output "reports/youtube-analysis-$TODAY.md"
```

**Benefits:**
- Track channel activity over time
- Identify inactive channels
- Discover high-value new content
- Update channel priorities

### Use Case 3: Story Video vs Shorts Analysis

**Scenario:** Evaluate if a channel is shifting to Shorts over story videos.

```bash
# Analyze channel with detailed metrics
python3 scripts/youtube-channel-analyzer.py \
  --channel @PopularChannel \
  --api-key $YOUTUBE_API_KEY \
  --output channel-content-mix.json
```

**Analysis Points:**
- Check `shorts_excluded` vs `story_videos_analyzed` ratio
- If shorts > story videos, channel may be less valuable for research
- Story videos indicate deeper, educational content

### Use Case 4: Bulk Channel Discovery

**Scenario:** You have a list of channel handles and want to analyze them all.

**Step 1: Create a temporary markdown file**
```markdown
# Channels to Analyze

- URL: https://www.youtube.com/@Channel1
- URL: https://www.youtube.com/@Channel2
- URL: https://www.youtube.com/@Channel3
```

**Step 2: Run batch analysis**
```bash
python3 scripts/youtube-channel-analyzer.py \
  --analyze-all-from-doc /tmp/channels-to-check.md \
  --api-key $YOUTUBE_API_KEY \
  --output discovery-analysis.md
```

## Output Format Options

### JSON Output (for processing)

```bash
# Generate JSON for programmatic use
python3 scripts/youtube-channel-analyzer.py \
  --channel @SebastianLague \
  --api-key $YOUTUBE_API_KEY \
  --output results.json
```

**Use Cases for JSON:**
- Import into databases
- Process with other scripts
- Generate charts/visualizations
- Automated reporting

### Markdown Output (for documentation)

```bash
# Generate Markdown for human reading
python3 scripts/youtube-channel-analyzer.py \
  --channel @SebastianLague \
  --api-key $YOUTUBE_API_KEY \
  --output results.md
```

**Use Cases for Markdown:**
- Direct inclusion in research documents
- Easy reading and reviewing
- Version control friendly
- GitHub rendering

## Integration with Research Workflow

### Step 1: Extract Channels

The tool automatically extracts channels from your research markdown:

```markdown
#### 22. **Sebastian Lague**
- **URL:** https://www.youtube.com/@SebastianLague
- **Status:** ⏳ Pending Review
```

### Step 2: Run Analysis

```bash
python3 scripts/youtube-channel-analyzer.py \
  --analyze-all-from-doc research/literature/online-game-dev-resources.md \
  --api-key $YOUTUBE_API_KEY \
  --output research/literature/youtube-channels-analysis.md
```

### Step 3: Review Results

Open the generated report and review:
- Story video statistics
- Content quality indicators
- Priority recommendations

### Step 4: Update Research Document

Based on analysis, update channel status:

```markdown
#### 22. **Sebastian Lague**
- **URL:** https://www.youtube.com/@SebastianLague
- **Status:** ✅ Analyzed - High Priority
- **Story Videos:** 50 analyzed, avg 16.8 min
- **Focus:** Technical implementations, procedural generation
```

## Understanding the Output

### Story Video Statistics Explained

```json
"story_videos_stats": {
  "total_story_videos": 50,      // Videos > 60 seconds
  "average_views": 520000,        // Mean views per story video
  "average_duration_minutes": 16.8, // Mean video length
  "total_views": 26000000         // Total views on story videos
}
```

**Quality Indicators:**
- **High average views** (>100K): Popular, engaging content
- **Long average duration** (>10 min): In-depth tutorials
- **Many story videos**: Consistent content creator
- **Low shorts ratio**: Focus on educational content

### Filters Applied

The tool applies these filters automatically:

1. **Duration Filter**: Videos ≤ 60 seconds classified as Shorts (excluded)
2. **Live Stream Filter**: Active or past live streams excluded
3. **Premiere Filter**: Upcoming premieres excluded (optional)

**Why These Filters?**
- Story videos have more educational value
- Shorts lack depth for technical content
- Live streams are less structured
- Focus on curated, edited content

## Troubleshooting

### Issue: "Channel not found"

**Possible Causes:**
- Incorrect channel handle
- Channel URL format not recognized
- Channel deleted or renamed

**Solutions:**
```bash
# Try different URL formats
python3 scripts/youtube-channel-analyzer.py --channel @ChannelName
python3 scripts/youtube-channel-analyzer.py --channel-url https://www.youtube.com/@ChannelName
python3 scripts/youtube-channel-analyzer.py --channel-url https://www.youtube.com/c/ChannelName
```

### Issue: "YouTube API not initialized"

**Cause:** No API key provided or invalid API key

**Solution:**
```bash
# Provide valid API key
python3 scripts/youtube-channel-analyzer.py \
  --channel @ChannelName \
  --api-key YOUR_VALID_API_KEY
```

### Issue: "API quota exceeded"

**Cause:** YouTube API has daily quota limits (10,000 units/day by default)

**Solutions:**
1. Wait for quota reset (daily)
2. Use basic mode without API key (limited info)
3. Request quota increase from Google Cloud Console
4. Spread analysis over multiple days

**Quota Usage:**
- Channel details: ~5 units
- Video details: 1 unit per video
- Analyzing 50 videos: ~55 units
- Batch analyzing 10 channels: ~550 units

### Issue: "Scraping libraries not available"

**Cause:** Required Python packages not installed

**Solution:**
```bash
pip install requests beautifulsoup4
# For API mode:
pip install google-api-python-client
```

## Advanced Usage

### Custom Filtering Logic

If you need to modify filtering logic, edit `youtube-channel-analyzer.py`:

```python
# Current: Videos > 60 seconds are story videos
if duration_seconds > 60:
    # Story video
    
# To change threshold to 90 seconds:
if duration_seconds > 90:
    # Story video
```

### Export to Database

Process JSON output to import into database:

```python
import json

# Load analysis results
with open('results.json', 'r') as f:
    data = json.load(f)

# Extract for database
for channel in data['channels']:
    channel_id = channel['channel_id']
    subscribers = channel['subscriber_count']
    story_videos = channel['story_videos_analyzed']
    # Insert into database...
```

### Automated Monitoring

Set up cron job (Linux/macOS) or Task Scheduler (Windows) for periodic analysis:

```bash
# Cron job - run monthly on 1st day at 9 AM
0 9 1 * * cd ~/BlueMarble.Design && python3 scripts/youtube-channel-analyzer.py --analyze-all-from-doc research/literature/online-game-dev-resources.md --api-key $YOUTUBE_API_KEY --output reports/youtube-$(date +\%Y-\%m).md
```

## Best Practices

1. **API Key Security**
   - Never commit API keys to version control
   - Use environment variables
   - Rotate keys periodically

2. **Quota Management**
   - Monitor API usage in Google Cloud Console
   - Batch analyze during off-hours
   - Use basic mode for quick checks

3. **Result Organization**
   - Use dated output files for tracking over time
   - Keep analysis reports in `reports/` or `research/literature/`
   - Version control analysis results to track changes

4. **Research Integration**
   - Update research documents based on analysis
   - Mark channels as analyzed with ✅
   - Add key statistics to channel entries

## See Also

- [Main Documentation](YOUTUBE_ANALYZER_README.md) - Complete tool guide
- [Sample Report](../docs/youtube-channel-analysis-sample.md) - Example output
- [Scripts README](README.md) - All available tools

---

**Last Updated:** 2025-01-20  
**Author:** BlueMarble Research Team
