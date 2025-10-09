# YouTube Channel Analysis Report - Sample

**Source Document:** research/literature/online-game-dev-resources.md  
**Analyzed At:** 2025-01-20T14:30:00  
**Total Channels:** 5

---

## Analysis Focus: Story Videos Only

This analysis focuses on **story videos** (regular uploaded content) and excludes:
- YouTube Shorts (videos ≤ 60 seconds)
- Live streams
- Premieres

**Note:** This is a sample report demonstrating the expected output format when the analyzer is run with YouTube Data API access. The actual analysis would be performed by running:

```bash
python scripts/youtube-channel-analyzer.py \
  --analyze-all-from-doc research/literature/online-game-dev-resources.md \
  --api-key YOUR_YOUTUBE_API_KEY \
  --output youtube-channels-analysis.md
```

---

## 1. Sebastian Lague

**Channel Handle:** @SebastianLague  
**Channel URL:** https://www.youtube.com/@SebastianLague  
**Subscribers:** 1,650,000  
**Total Videos:** 168  
**Story Videos Analyzed:** 50  
**Shorts Excluded:** 8  
**Live Streams Excluded:** 3

### Story Video Statistics

- **Total Story Videos:** 50
- **Average Views:** 520,000
- **Average Duration:** 16.8 minutes
- **Total Views:** 26,000,000

### Content Quality Assessment

**Strengths:**
- High-quality technical tutorials
- In-depth procedural generation content
- Excellent production value
- Strong engagement metrics

**Relevance to BlueMarble:**
- ✅ Procedural terrain generation
- ✅ Pathfinding algorithms (A*)
- ✅ Marching cubes terrain
- ✅ Technical game development concepts

**Priority:** HIGH - Essential resource for technical implementation

**Analysis Method:** youtube_data_api_v3

---

## 2. Code Monkey

**Channel Handle:** @CodeMonkeyUnity  
**Channel URL:** https://www.youtube.com/@CodeMonkeyUnity  
**Subscribers:** 820,000  
**Total Videos:** 486  
**Story Videos Analyzed:** 50  
**Shorts Excluded:** 124  
**Live Streams Excluded:** 15

### Story Video Statistics

- **Total Story Videos:** 347
- **Average Views:** 145,000
- **Average Duration:** 12.3 minutes
- **Total Views:** 50,315,000

### Content Quality Assessment

**Strengths:**
- Prolific content creator
- Unity-specific tutorials
- Multiplayer networking focus
- Grid-based game systems

**Relevance to BlueMarble:**
- ✅ Turn-based strategy systems
- ✅ Grid systems implementation
- ✅ Multiplayer networking basics
- ✅ RPG systems design

**Note:** High shorts count (124) indicates recent shift to shorter content format. Story videos remain primary resource.

**Priority:** HIGH - Strong Unity and networking content

**Analysis Method:** youtube_data_api_v3

---

## 3. Brackeys (Historical Archive)

**Channel Handle:** @Brackeys  
**Channel URL:** https://www.youtube.com/@Brackeys  
**Subscribers:** 1,640,000  
**Total Videos:** 318  
**Story Videos Analyzed:** 50  
**Shorts Excluded:** 0  
**Live Streams Excluded:** 0

### Story Video Statistics

- **Total Story Videos:** 318
- **Average Views:** 380,000
- **Average Duration:** 14.5 minutes
- **Total Views:** 120,840,000

### Content Quality Assessment

**Strengths:**
- Complete tutorial series
- Beginner-friendly content
- Wide coverage of Unity topics
- Strong community following

**Relevance to BlueMarble:**
- ✅ Unity 2D/3D tutorials
- ✅ RPG mechanics basics
- ✅ Game design principles
- ⚠️ Content no longer being updated

**Note:** Channel inactive since 2020 but archive remains highly valuable educational resource.

**Priority:** MEDIUM-HIGH - Excellent archive despite no new content

**Analysis Method:** youtube_data_api_v3

---

## 4. GameDev.tv

**Channel Handle:** @GameDevTV  
**Channel URL:** https://www.youtube.com/@GameDevTV  
**Subscribers:** 305,000  
**Total Videos:** 842  
**Story Videos Analyzed:** 50  
**Shorts Excluded:** 45  
**Live Streams Excluded:** 67

### Story Video Statistics

- **Total Story Videos:** 730
- **Average Views:** 35,000
- **Average Duration:** 22.7 minutes
- **Total Views:** 25,550,000

### Content Quality Assessment

**Strengths:**
- Long-form tutorial content
- Structured course materials
- Regular live streams for community
- Multi-platform coverage (Unity, Unreal, Godot)

**Relevance to BlueMarble:**
- ✅ Complete RPG development courses
- ✅ Multiplayer networking tutorials
- ✅ C# programming fundamentals
- ⚠️ Some content promotional for paid courses

**Note:** Free YouTube content complements paid courses. Good mix of beginner to intermediate topics.

**Priority:** MEDIUM - Good supplementary resource

**Analysis Method:** youtube_data_api_v3

---

## 5. GDC (Game Developers Conference)

**Channel Handle:** @Gdconf  
**Channel URL:** https://www.youtube.com/@Gdconf  
**Subscribers:** 590,000  
**Total Videos:** 2,147  
**Story Videos Analyzed:** 50  
**Shorts Excluded:** 12  
**Live Streams Excluded:** 8

### Story Video Statistics

- **Total Story Videos:** 2,127
- **Average Views:** 28,000
- **Average Duration:** 47.5 minutes
- **Total Views:** 59,556,000

### Content Quality Assessment

**Strengths:**
- Professional industry talks
- MMORPG architecture deep-dives
- Postmortems from major titles
- Technical implementation details

**Relevance to BlueMarble:**
- ✅ CRITICAL - MMO architecture talks
- ✅ CRITICAL - EVE Online single-shard system
- ✅ CRITICAL - WoW programming insights
- ✅ Networking at scale
- ✅ Server architecture patterns

**Specific Videos of Interest:**
- "EVE Online: Single Shard Architecture"
- "World of Warcraft: Server Architecture"
- "Pathfinding in Large-Scale MMOs"
- "Client-Server Networking Best Practices"

**Priority:** CRITICAL - Industry-standard professional knowledge

**Analysis Method:** youtube_data_api_v3

---

## Summary Statistics

### Overall Content Analysis

| Metric | Total | Average per Channel |
|--------|-------|---------------------|
| Total Subscribers | 5,005,000 | 1,001,000 |
| Total Videos | 3,961 | 792 |
| Story Videos | 3,572 | 714 |
| Shorts (Excluded) | 189 | 38 |
| Live Streams (Excluded) | 93 | 19 |

### Story Video Focus Metrics

- **Story Video Ratio:** 90.2% (3,572 / 3,961)
- **Shorts Ratio:** 4.8% (189 / 3,961)
- **Live Stream Ratio:** 2.3% (93 / 3,961)

**Analysis:** High story video ratio (90.2%) indicates these channels focus primarily on long-form educational content, which is ideal for research and learning purposes.

### Engagement Quality

All channels show strong engagement metrics:
- High subscriber counts (305K - 1.65M)
- Good view-to-subscriber ratios
- Active or valuable archived content
- Technical depth appropriate for game development

### Priority Assessment

1. **CRITICAL Priority:**
   - GDC (@Gdconf) - Professional industry talks, MMORPG architecture

2. **HIGH Priority:**
   - Sebastian Lague (@SebastianLague) - Technical implementation
   - Code Monkey (@CodeMonkeyUnity) - Unity multiplayer systems

3. **MEDIUM-HIGH Priority:**
   - Brackeys (@Brackeys) - Comprehensive archive
   - GameDev.tv (@GameDevTV) - Structured tutorials

### Recommendations

1. **Immediate Action:**
   - Watch GDC talks on MMORPG architecture
   - Review Sebastian Lague's procedural generation series
   - Study Code Monkey's multiplayer networking tutorials

2. **Regular Monitoring:**
   - Track new content from active channels (all except Brackeys)
   - Check for shorts vs. story video ratio changes
   - Monitor for new MMORPG-related content

3. **Content Integration:**
   - Create dedicated analysis documents for key video series
   - Extract implementation patterns from high-priority content
   - Build reference library of relevant tutorials

---

## Methodology Notes

### Story Video Classification

Videos classified as "story videos" meet these criteria:
1. Duration > 60 seconds (excludes YouTube Shorts)
2. Not live broadcasts
3. Regular uploaded video content
4. Educational or tutorial format

### Why Focus on Story Videos?

**Educational Value:**
- Story videos provide in-depth explanations
- Technical concepts require longer format
- Better reference material for implementation
- Higher production value and planning

**Quality Signal:**
- Channels with high story video ratios prioritize education
- Long-form content shows commitment to thorough teaching
- More suitable for technical game development learning

**Practical Benefits:**
- Easier to reference specific timestamps
- Can be used as learning modules
- Better for team training and onboarding
- More detailed code examples and explanations

### Excluded Content Types

**YouTube Shorts (≤ 60 seconds):**
- Too brief for technical content
- Often promotional or teaser content
- Limited educational value
- Difficult to use as reference

**Live Streams:**
- Less structured than edited content
- Variable audio/video quality
- Harder to find specific information
- Time-consuming to review

**Premieres:**
- Not yet available for analysis
- May be live or story video when released

---

**Analysis Tool:** `scripts/youtube-channel-analyzer.py`  
**Report Generated:** Sample report for demonstration  
**Last Updated:** 2025-01-20

**To generate your own analysis:**
```bash
# Get YouTube Data API key from Google Cloud Console
# Enable YouTube Data API v3
# Install dependencies: pip install google-api-python-client

python scripts/youtube-channel-analyzer.py \
  --analyze-all-from-doc research/literature/online-game-dev-resources.md \
  --api-key YOUR_API_KEY \
  --output research/literature/youtube-channels-analysis.md
```
