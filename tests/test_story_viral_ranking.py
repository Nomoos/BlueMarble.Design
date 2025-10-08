#!/usr/bin/env python3
"""
Test suite for Story Viral Ranking System
"""

import sys
import time
from pathlib import Path

# Add scripts directory to path
sys.path.insert(0, str(Path(__file__).parent.parent / 'scripts'))

from story_viral_ranking import (
    Story, StoryType, EngagementMetrics, SocialMetrics,
    ContentQuality, StoryRanker, ViralPotentialCalculator
)


def test_engagement_score():
    """Test engagement score calculation"""
    calculator = ViralPotentialCalculator()
    
    # High engagement
    high_engagement = EngagementMetrics(
        views=10000,
        shares=500,
        comments=200,
        reactions=1000,
        saves=300,
        time_spent_seconds=300,
        completion_rate=0.9
    )
    high_score = calculator.calculate_engagement_score(high_engagement)
    
    # Low engagement
    low_engagement = EngagementMetrics(
        views=1000,
        shares=10,
        comments=5,
        reactions=20,
        saves=5,
        time_spent_seconds=60,
        completion_rate=0.5
    )
    low_score = calculator.calculate_engagement_score(low_engagement)
    
    assert high_score > low_score, "High engagement should score higher"
    assert 0 <= high_score <= 100, "Score should be in 0-100 range"
    assert 0 <= low_score <= 100, "Score should be in 0-100 range"
    
    print(f"✓ Engagement score test passed (high: {high_score:.2f}, low: {low_score:.2f})")


def test_social_score():
    """Test social network score calculation"""
    calculator = ViralPotentialCalculator()
    
    # High social reach
    high_social = SocialMetrics(
        author_followers=1000,
        author_reputation=90.0,
        guild_size=200,
        alliance_reach=1000,
        cross_guild_shares=100
    )
    high_score = calculator.calculate_social_score(high_social)
    
    # Low social reach
    low_social = SocialMetrics(
        author_followers=10,
        author_reputation=20.0,
        guild_size=5,
        alliance_reach=10,
        cross_guild_shares=1
    )
    low_score = calculator.calculate_social_score(low_social)
    
    assert high_score > low_score, "High social reach should score higher"
    assert 0 <= high_score <= 100, "Score should be in 0-100 range"
    assert 0 <= low_score <= 100, "Score should be in 0-100 range"
    
    print(f"✓ Social score test passed (high: {high_score:.2f}, low: {low_score:.2f})")


def test_quality_score():
    """Test content quality score calculation"""
    calculator = ViralPotentialCalculator()
    
    # High quality
    high_quality = ContentQuality(
        word_count=1000,
        has_images=True,
        has_video=True,
        uniqueness_score=0.95,
        emotional_impact=0.9,
        narrative_quality=0.95
    )
    high_score = calculator.calculate_quality_score(high_quality)
    
    # Low quality
    low_quality = ContentQuality(
        word_count=50,
        has_images=False,
        has_video=False,
        uniqueness_score=0.3,
        emotional_impact=0.2,
        narrative_quality=0.4
    )
    low_score = calculator.calculate_quality_score(low_quality)
    
    assert high_score > low_score, "High quality should score higher"
    assert 0 <= high_score <= 100, "Score should be in 0-100 range"
    assert 0 <= low_score <= 100, "Score should be in 0-100 range"
    
    print(f"✓ Quality score test passed (high: {high_score:.2f}, low: {low_score:.2f})")


def test_recency_score():
    """Test recency score calculation"""
    calculator = ViralPotentialCalculator()
    
    current_time = time.time()
    
    # Recent content (1 hour ago)
    recent_score = calculator.calculate_recency_score(
        current_time - 3600, current_time
    )
    
    # Old content (1 week ago)
    old_score = calculator.calculate_recency_score(
        current_time - (7 * 86400), current_time
    )
    
    assert recent_score > old_score, "Recent content should score higher"
    assert 0 <= recent_score <= 100, "Score should be in 0-100 range"
    assert 0 <= old_score <= 100, "Score should be in 0-100 range"
    assert recent_score > 95, "Recent content should score very high"
    
    print(f"✓ Recency score test passed (recent: {recent_score:.2f}, old: {old_score:.2f})")


def test_viral_score_calculation():
    """Test overall viral score calculation"""
    calculator = ViralPotentialCalculator()
    current_time = time.time()
    
    # Create a high-performing story
    high_story = Story(
        story_id="test1",
        title="Test Story 1",
        story_type=StoryType.DISCOVERY_STORY,
        engagement=EngagementMetrics(
            views=10000, shares=500, comments=200,
            reactions=1000, saves=300,
            time_spent_seconds=300, completion_rate=0.9
        ),
        social=SocialMetrics(
            author_followers=1000, author_reputation=90.0,
            guild_size=200, alliance_reach=1000,
            cross_guild_shares=100
        ),
        quality=ContentQuality(
            word_count=1000, has_images=True, has_video=True,
            uniqueness_score=0.95, emotional_impact=0.9,
            narrative_quality=0.95
        ),
        created_timestamp=current_time - 3600
    )
    
    high_score = calculator.calculate_viral_score(high_story, current_time)
    
    # Create a low-performing story
    low_story = Story(
        story_id="test2",
        title="Test Story 2",
        story_type=StoryType.PLAYER_EXPERIENCE,
        engagement=EngagementMetrics(
            views=100, shares=1, comments=0,
            reactions=5, saves=0,
            time_spent_seconds=30, completion_rate=0.3
        ),
        social=SocialMetrics(
            author_followers=5, author_reputation=10.0,
            guild_size=3, alliance_reach=5,
            cross_guild_shares=0
        ),
        quality=ContentQuality(
            word_count=50, has_images=False, has_video=False,
            uniqueness_score=0.2, emotional_impact=0.1,
            narrative_quality=0.3
        ),
        created_timestamp=current_time - (7 * 86400)
    )
    
    low_score = calculator.calculate_viral_score(low_story, current_time)
    
    assert high_score > low_score, "High-performing story should score higher"
    assert 0 <= high_score <= 100, "Score should be in 0-100 range"
    assert 0 <= low_score <= 100, "Score should be in 0-100 range"
    
    print(f"✓ Viral score calculation test passed (high: {high_score:.2f}, low: {low_score:.2f})")


def test_story_ranking():
    """Test story ranking functionality"""
    ranker = StoryRanker()
    current_time = time.time()
    
    # Create stories with varying quality
    stories = [
        Story(
            story_id=f"story{i}",
            title=f"Story {i}",
            story_type=StoryType.DISCOVERY_STORY,
            engagement=EngagementMetrics(
                views=1000 * i, shares=50 * i, comments=20 * i,
                reactions=100 * i, saves=30 * i,
                time_spent_seconds=180, completion_rate=0.8
            ),
            social=SocialMetrics(
                author_followers=100 * i, author_reputation=50.0 + i * 5,
                guild_size=20 * i, alliance_reach=100 * i,
                cross_guild_shares=10 * i
            ),
            quality=ContentQuality(
                word_count=500, has_images=True, has_video=False,
                uniqueness_score=0.7, emotional_impact=0.6,
                narrative_quality=0.7
            ),
            created_timestamp=current_time - 3600
        )
        for i in range(1, 6)
    ]
    
    ranked = ranker.rank_stories(stories, current_time)
    
    # Check that stories are properly ranked
    assert len(ranked) == 5, "Should have 5 stories"
    assert all(s.viral_rank is not None for s in ranked), "All stories should have ranks"
    assert all(s.viral_score is not None for s in ranked), "All stories should have scores"
    
    # Check descending order
    for i in range(len(ranked) - 1):
        assert ranked[i].viral_score >= ranked[i + 1].viral_score, \
            "Stories should be in descending score order"
        assert ranked[i].viral_rank < ranked[i + 1].viral_rank, \
            "Ranks should be ascending"
    
    print(f"✓ Story ranking test passed ({len(ranked)} stories ranked)")


def test_top_stories():
    """Test getting top N stories"""
    ranker = StoryRanker()
    current_time = time.time()
    
    # Create 20 stories
    stories = [
        Story(
            story_id=f"story{i}",
            title=f"Story {i}",
            story_type=StoryType.DISCOVERY_STORY,
            engagement=EngagementMetrics(
                views=100 * i, shares=5 * i, comments=2 * i,
                reactions=10 * i, saves=3 * i,
                time_spent_seconds=120, completion_rate=0.7
            ),
            social=SocialMetrics(
                author_followers=50, author_reputation=50.0,
                guild_size=20, alliance_reach=100,
                cross_guild_shares=5
            ),
            quality=ContentQuality(
                word_count=400, has_images=True, has_video=False,
                uniqueness_score=0.6, emotional_impact=0.5,
                narrative_quality=0.6
            ),
            created_timestamp=current_time - 3600
        )
        for i in range(1, 21)
    ]
    
    top_10 = ranker.get_top_stories(stories, current_time, top_n=10)
    
    assert len(top_10) == 10, "Should return exactly 10 stories"
    assert all(s.viral_rank <= 10 for s in top_10), "All should be in top 10"
    
    print(f"✓ Top stories test passed (got {len(top_10)} stories)")


def test_trending_stories():
    """Test getting trending stories"""
    ranker = StoryRanker()
    current_time = time.time()
    
    # Create stories with different ages
    stories = []
    
    # Recent stories (last 12 hours)
    for i in range(1, 6):
        stories.append(Story(
            story_id=f"recent{i}",
            title=f"Recent Story {i}",
            story_type=StoryType.DISCOVERY_STORY,
            engagement=EngagementMetrics(
                views=1000, shares=50, comments=20,
                reactions=100, saves=30,
                time_spent_seconds=180, completion_rate=0.8
            ),
            social=SocialMetrics(
                author_followers=100, author_reputation=60.0,
                guild_size=30, alliance_reach=150,
                cross_guild_shares=20
            ),
            quality=ContentQuality(
                word_count=500, has_images=True, has_video=False,
                uniqueness_score=0.7, emotional_impact=0.7,
                narrative_quality=0.7
            ),
            created_timestamp=current_time - (i * 3600)  # 1-5 hours ago
        ))
    
    # Old stories (2 days ago)
    for i in range(1, 6):
        stories.append(Story(
            story_id=f"old{i}",
            title=f"Old Story {i}",
            story_type=StoryType.PLAYER_EXPERIENCE,
            engagement=EngagementMetrics(
                views=5000, shares=200, comments=100,
                reactions=500, saves=150,
                time_spent_seconds=240, completion_rate=0.9
            ),
            social=SocialMetrics(
                author_followers=500, author_reputation=80.0,
                guild_size=100, alliance_reach=500,
                cross_guild_shares=80
            ),
            quality=ContentQuality(
                word_count=800, has_images=True, has_video=True,
                uniqueness_score=0.9, emotional_impact=0.9,
                narrative_quality=0.9
            ),
            created_timestamp=current_time - (2 * 86400)  # 2 days ago
        ))
    
    trending = ranker.get_trending_stories(stories, current_time, 
                                           recency_hours=24, top_n=10)
    
    assert len(trending) <= 10, "Should return at most 10 stories"
    assert len(trending) > 0, "Should find some trending stories"
    
    # All trending stories should be recent
    for story in trending:
        age_hours = (current_time - story.created_timestamp) / 3600
        assert age_hours < 24, f"Story {story.story_id} is too old ({age_hours:.1f} hours)"
    
    print(f"✓ Trending stories test passed (found {len(trending)} trending)")


def test_stories_by_type():
    """Test filtering stories by type"""
    ranker = StoryRanker()
    current_time = time.time()
    
    # Create stories of different types
    story_types = [
        StoryType.DISCOVERY_STORY,
        StoryType.PLAYER_EXPERIENCE,
        StoryType.SETTLEMENT_STORY,
        StoryType.ACHIEVEMENT_STORY,
        StoryType.QUEST_NARRATIVE
    ]
    
    stories = []
    for i, story_type in enumerate(story_types * 3):  # 3 of each type
        stories.append(Story(
            story_id=f"story{i}",
            title=f"Story {i}",
            story_type=story_type,
            engagement=EngagementMetrics(
                views=1000, shares=50, comments=20,
                reactions=100, saves=30,
                time_spent_seconds=180, completion_rate=0.8
            ),
            social=SocialMetrics(
                author_followers=100, author_reputation=60.0,
                guild_size=30, alliance_reach=150,
                cross_guild_shares=20
            ),
            quality=ContentQuality(
                word_count=500, has_images=True, has_video=False,
                uniqueness_score=0.7, emotional_impact=0.7,
                narrative_quality=0.7
            ),
            created_timestamp=current_time - 3600
        ))
    
    # Get discovery stories only
    discovery = ranker.get_stories_by_type(
        stories, current_time, StoryType.DISCOVERY_STORY, top_n=10
    )
    
    assert len(discovery) == 3, "Should find exactly 3 discovery stories"
    assert all(s.story_type == StoryType.DISCOVERY_STORY for s in discovery), \
        "All should be discovery stories"
    
    print(f"✓ Stories by type test passed (found {len(discovery)} discovery stories)")


def run_all_tests():
    """Run all test functions"""
    tests = [
        test_engagement_score,
        test_social_score,
        test_quality_score,
        test_recency_score,
        test_viral_score_calculation,
        test_story_ranking,
        test_top_stories,
        test_trending_stories,
        test_stories_by_type
    ]
    
    print("Running Story Viral Ranking System Tests")
    print("=" * 60)
    
    passed = 0
    failed = 0
    
    for test in tests:
        try:
            test()
            passed += 1
        except AssertionError as e:
            print(f"✗ {test.__name__} failed: {e}")
            failed += 1
        except Exception as e:
            print(f"✗ {test.__name__} error: {e}")
            failed += 1
    
    print("=" * 60)
    print(f"Tests: {passed} passed, {failed} failed, {passed + failed} total")
    
    return failed == 0


if __name__ == "__main__":
    success = run_all_tests()
    sys.exit(0 if success else 1)
