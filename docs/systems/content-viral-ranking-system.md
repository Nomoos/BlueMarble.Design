# Content Viral Ranking System

**Version:** 1.0  
**Date:** 2025-01-20  
**Author:** BlueMarble Systems Team  
**Status:** Active

## Executive Summary

This document describes the Content Viral Ranking System for BlueMarble, which ranks player-generated stories, quest narratives, and content by their viral potential based on engagement metrics, social factors, and content quality.

## Overview

The viral ranking system helps surface the most engaging and shareable content to players, encouraging community participation and content creation. It ranks stories based on multiple factors including:

- **Engagement metrics** (views, shares, reactions)
- **Social network effects** (reach, cross-guild sharing)
- **Content quality** (uniqueness, emotional impact)
- **Temporal recency** (recent content gets boosted)

## Story Types

The system supports ranking for different types of content:

| Story Type | Description | Example |
|------------|-------------|---------|
| `PLAYER_EXPERIENCE` | Personal journey narratives | "My Journey from Novice to Master Smith" |
| `QUEST_NARRATIVE` | Quest completion stories | "The Great Cathedral Quest" |
| `SETTLEMENT_STORY` | Settlement-related events | "Settlement Defense: How We Survived" |
| `ACHIEVEMENT_STORY` | Achievement/milestone stories | "First Player to Reach Level 50 Mining" |
| `DISCOVERY_STORY` | Exploration and discovery | "Epic Mining Discovery in Northern Peaks" |

## Ranking Algorithm

### Components

The viral score is calculated using four weighted components:

```
Viral Score = 
    (Engagement Score × 40%) +
    (Social Score × 30%) +
    (Quality Score × 20%) +
    (Recency Score × 10%)
```

### 1. Engagement Score (40% weight)

Measures how players interact with the content:

**Metrics:**
- **Shares** (10× multiplier) - Most valuable metric
- **Saves** (5× multiplier) - Strong engagement signal
- **Comments** (3× multiplier) - Active participation
- **Reactions** (2× multiplier) - Passive engagement
- **Views** (1× multiplier) - Base reach

**Bonuses:**
- **Completion Rate**: +50% if readers complete the story
- **Time Spent**: Up to 2× multiplier for longer engagement (300s baseline)

**Formula:**
```python
engagement_points = (
    shares × 10 +
    saves × 5 +
    comments × 3 +
    reactions × 2 +
    views × 1
)

engagement_rate = engagement_points / views
completion_bonus = 1.0 + (completion_rate × 0.5)
time_bonus = min(1.0 + (time_spent / 300), 2.0)

score = engagement_rate × completion_bonus × time_bonus
normalized = min(100, log(score + 1) × 10)
```

### 2. Social Score (30% weight)

Measures network reach and author influence:

**Metrics:**
- **Author Followers**: log(followers) × 2.0
- **Author Reputation**: reputation × 0.1
- **Guild Size**: log(guild_size) × 1.5
- **Alliance Reach**: log(alliance_reach) × 2.0
- **Cross-Guild Shares**: shares × 15.0 (key viral indicator)

**Why Cross-Guild Shares Matter:**
Content that spreads across guilds demonstrates broader appeal and higher viral potential than content that stays within a single community.

**Formula:**
```python
author_score = log(followers + 1) × 2.0
reputation_bonus = reputation × 0.1
guild_reach = log(guild_size + 1) × 1.5
alliance_reach = log(alliance_reach + 1) × 2.0
cross_guild_bonus = cross_guild_shares × 15.0

score = author_score + reputation_bonus + guild_reach + 
        alliance_reach + cross_guild_bonus
normalized = min(100, score)
```

### 3. Quality Score (20% weight)

Assesses content production quality:

**Metrics:**
- **Uniqueness Score** (30 points) - How original the content is
- **Emotional Impact** (30 points) - Emotional resonance
- **Narrative Quality** (30 points) - Story structure and writing

**Bonuses:**
- **Word Count**:
  - 100-299 words: +3 points
  - 300-999 words: +5 points
  - 1000+ words: +7 points
- **Has Images**: +2 points
- **Has Video**: +3 points

**Formula:**
```python
base_score = (
    uniqueness_score × 30 +
    emotional_impact × 30 +
    narrative_quality × 30
)

word_bonus = calculate_word_bonus(word_count)
media_bonus = (2 if has_images else 0) + (3 if has_video else 0)

score = base_score + word_bonus + media_bonus
normalized = min(100, score)
```

### 4. Recency Score (10% weight)

Applies temporal decay to promote fresh content:

**Parameters:**
- **Half-Life**: 3 days (72 hours)
- **Decay Function**: Exponential decay

**Formula:**
```python
age_seconds = current_time - created_time
decay_factor = exp(-age_seconds × ln(2) / half_life)
score = decay_factor × 100
```

**Effect:**
- Fresh content (< 6 hours): ~98-100 points
- 1 day old: ~79 points
- 3 days old: ~50 points
- 1 week old: ~18 points

## API Usage

### Basic Ranking

```python
from scripts.story_viral_ranking import (
    Story, StoryType, EngagementMetrics, 
    SocialMetrics, ContentQuality, StoryRanker
)
import time

# Create ranker
ranker = StoryRanker()

# Prepare stories
stories = [
    Story(
        story_id="story123",
        title="Epic Mining Discovery",
        story_type=StoryType.DISCOVERY_STORY,
        engagement=EngagementMetrics(
            views=5000,
            shares=250,
            comments=120,
            reactions=800,
            saves=150,
            time_spent_seconds=180,
            completion_rate=0.85
        ),
        social=SocialMetrics(
            author_followers=500,
            author_reputation=75.0,
            guild_size=50,
            alliance_reach=200,
            cross_guild_shares=80
        ),
        quality=ContentQuality(
            word_count=450,
            has_images=True,
            has_video=False,
            uniqueness_score=0.9,
            emotional_impact=0.8,
            narrative_quality=0.85
        ),
        created_timestamp=time.time() - 3600
    ),
    # ... more stories
]

# Rank all stories
ranked = ranker.rank_stories(stories, time.time())

# Get top 10
top_stories = ranker.get_top_stories(stories, time.time(), top_n=10)

# Get trending (last 24 hours)
trending = ranker.get_trending_stories(stories, time.time(), 
                                       recency_hours=24, top_n=10)

# Filter by type
discovery_stories = ranker.get_stories_by_type(
    stories, time.time(), 
    StoryType.DISCOVERY_STORY, top_n=10
)
```

### Analyzing Results

```python
for story in ranked:
    print(f"Rank #{story.viral_rank}: {story.title}")
    print(f"  Viral Score: {story.viral_score:.2f}")
    print(f"  Type: {story.story_type.value}")
    print(f"  Engagement: {story.engagement.views} views, "
          f"{story.engagement.shares} shares")
```

## Integration Points

### 1. Social Feed

Display trending stories in player social feeds:

```typescript
// Frontend integration
async function loadTrendingStories() {
    const response = await api.get('/content/trending', {
        params: { hours: 24, limit: 10 }
    });
    
    displayStories(response.data.stories);
}
```

### 2. Content Discovery UI

Create a "Popular Stories" section in the game:

```typescript
interface StoryCard {
    storyId: string;
    title: string;
    author: string;
    viralScore: number;
    thumbnailUrl: string;
    engagement: {
        views: number;
        shares: number;
        reactions: number;
    };
}

function PopularStoriesWidget() {
    const [stories, setStories] = useState<StoryCard[]>([]);
    
    useEffect(() => {
        fetchTopStories().then(setStories);
    }, []);
    
    return (
        <div className="popular-stories">
            <h2>Trending Stories</h2>
            {stories.map(story => (
                <StoryCard key={story.storyId} {...story} />
            ))}
        </div>
    );
}
```

### 3. Guild Content Feeds

Show top stories within guild:

```python
# Backend service
class GuildContentService:
    def get_guild_top_stories(self, guild_id: str, 
                             days: int = 7, 
                             limit: int = 20) -> List[Story]:
        # Get stories from guild members
        guild_stories = self.story_repository.get_by_guild(
            guild_id, 
            since=datetime.now() - timedelta(days=days)
        )
        
        # Rank by viral potential
        ranker = StoryRanker()
        ranked = ranker.rank_stories(guild_stories, time.time())
        
        return ranked[:limit]
```

### 4. Notification System

Notify authors when their content goes viral:

```python
class ViralNotificationService:
    VIRAL_THRESHOLD = 70.0  # Score threshold for "viral"
    
    def check_and_notify(self, story: Story):
        if story.viral_score >= self.VIRAL_THRESHOLD:
            self.send_notification(
                user_id=story.author_id,
                title="Your Story is Going Viral!",
                message=f'"{story.title}" has reached {story.viral_score:.0f}/100 viral score!'
            )
```

## Performance Considerations

### Caching

```python
from functools import lru_cache
import time

class CachedStoryRanker:
    def __init__(self, cache_ttl_seconds=300):  # 5 minutes
        self.ranker = StoryRanker()
        self.cache_ttl = cache_ttl_seconds
        self._cache = {}
    
    def get_top_stories(self, cache_key: str, 
                       stories: List[Story],
                       top_n: int = 10) -> List[Story]:
        current_time = time.time()
        
        # Check cache
        if cache_key in self._cache:
            cached_time, cached_result = self._cache[cache_key]
            if current_time - cached_time < self.cache_ttl:
                return cached_result
        
        # Calculate fresh rankings
        result = self.ranker.get_top_stories(stories, current_time, top_n)
        
        # Update cache
        self._cache[cache_key] = (current_time, result)
        
        return result
```

### Database Indexing

Recommended indexes for efficient querying:

```sql
-- Index for recency queries
CREATE INDEX idx_stories_created_timestamp 
ON stories(created_timestamp DESC);

-- Index for story type filtering
CREATE INDEX idx_stories_type 
ON stories(story_type, created_timestamp DESC);

-- Index for author queries
CREATE INDEX idx_stories_author 
ON stories(author_id, created_timestamp DESC);

-- Composite index for trending queries
CREATE INDEX idx_stories_trending 
ON stories(created_timestamp DESC, views DESC, shares DESC);
```

## Monitoring and Analytics

### Key Metrics to Track

1. **Distribution of Viral Scores**
   - Monitor score distribution across all content
   - Identify if scoring is too lenient or strict

2. **Engagement Correlation**
   - Track correlation between viral score and actual engagement
   - Validate algorithm accuracy

3. **Content Type Performance**
   - Which story types achieve highest viral scores
   - Inform content creation recommendations

4. **Temporal Patterns**
   - Peak posting times for viral content
   - Optimal content lifecycle length

### Analytics Query Examples

```sql
-- Top performing content this week
SELECT 
    story_id,
    title,
    story_type,
    viral_score,
    views,
    shares,
    created_at
FROM ranked_stories
WHERE created_at >= NOW() - INTERVAL '7 days'
ORDER BY viral_score DESC
LIMIT 100;

-- Average viral score by story type
SELECT 
    story_type,
    AVG(viral_score) as avg_score,
    COUNT(*) as story_count,
    AVG(shares) as avg_shares
FROM ranked_stories
WHERE created_at >= NOW() - INTERVAL '30 days'
GROUP BY story_type
ORDER BY avg_score DESC;

-- Cross-guild sharing leaders
SELECT 
    author_id,
    COUNT(*) as viral_stories,
    AVG(viral_score) as avg_score,
    SUM(cross_guild_shares) as total_cross_guild_shares
FROM ranked_stories
WHERE viral_score >= 70
GROUP BY author_id
ORDER BY total_cross_guild_shares DESC
LIMIT 50;
```

## Best Practices

### For Content Creators

**To Maximize Viral Potential:**

1. **Create Unique Content**: Original perspectives score higher
2. **Add Visuals**: Images and videos boost quality score significantly
3. **Tell Emotional Stories**: Emotional impact is heavily weighted
4. **Share Across Communities**: Cross-guild shares are the #1 viral indicator
5. **Engage with Readers**: Respond to comments to boost engagement
6. **Post at Peak Times**: Leverage recency bonus when players are active

### For Game Designers

**System Tuning:**

1. **Monitor Score Distribution**: Adjust weights if scores cluster too tightly
2. **A/B Test Display**: Test different ways of surfacing ranked content
3. **Incentivize Quality**: Reward high-scoring content creators
4. **Prevent Gaming**: Watch for artificial inflation tactics
5. **Balance Recency**: Adjust half-life if recent content dominates too much

### For Community Managers

**Content Moderation:**

1. **Review High-Scoring Content**: Ensure quality standards
2. **Feature Top Stories**: Manually highlight exceptional content
3. **Guide Creators**: Share tips for creating engaging content
4. **Address Gaming**: Detect and address score manipulation

## Future Enhancements

### Planned Features

1. **Machine Learning Integration**
   - Train models on historical viral content
   - Predict viral potential at post time
   - Personalized viral predictions per player

2. **A/B Testing Framework**
   - Test weight adjustments
   - Measure impact on engagement
   - Optimize algorithm iteratively

3. **Content Recommendations**
   - "Stories you might like" based on viral patterns
   - Collaborative filtering
   - Personalized ranking

4. **Author Reputation System**
   - Build reputation based on viral content history
   - Bonus for consistently high-quality creators
   - Creator tiers/badges

5. **Viral Prediction API**
   - Pre-publication viral potential estimate
   - Suggestions to improve viral potential
   - Real-time viral trajectory tracking

## Related Documents

- [Social Interaction System](social-interaction-settlement-system.md)
- [Achievement and Reputation System](achievement-reputation-system.md)
- [Content Design Guidelines](../gameplay/content-rating-guidelines.md)
- [Engagement Metrics Research](../../research/topics/completionist-quest-engagement-patterns.md)

---

**Document Status:** ✅ Active  
**Review Schedule:** Quarterly  
**Next Review:** 2025-04-20  
**Owner:** Systems Team Lead
