#!/usr/bin/env python3
"""
Example usage of the Story Viral Ranking System

This script demonstrates how to use the viral ranking system to rank
player-generated content in BlueMarble.
"""

import sys
from pathlib import Path
import time

# Add scripts directory to path
sys.path.insert(0, str(Path(__file__).parent.parent / 'scripts'))

from story_viral_ranking import (
    Story, StoryType, EngagementMetrics, SocialMetrics,
    ContentQuality, StoryRanker
)


def create_sample_stories():
    """Create sample stories for demonstration"""
    current_time = time.time()
    
    return [
        Story(
            story_id="story_001",
            title="Epic Mining Discovery: The Diamond Vein of Northern Peaks",
            story_type=StoryType.DISCOVERY_STORY,
            engagement=EngagementMetrics(
                views=8500,
                shares=420,
                comments=180,
                reactions=950,
                saves=280,
                time_spent_seconds=220,
                completion_rate=0.88
            ),
            social=SocialMetrics(
                author_followers=650,
                author_reputation=78.5,
                guild_size=85,
                alliance_reach=340,
                cross_guild_shares=120
            ),
            quality=ContentQuality(
                word_count=780,
                has_images=True,
                has_video=False,
                uniqueness_score=0.92,
                emotional_impact=0.85,
                narrative_quality=0.88
            ),
            created_timestamp=current_time - 5400  # 1.5 hours ago
        ),
        
        Story(
            story_id="story_002",
            title="My Journey: From Novice to Master Blacksmith",
            story_type=StoryType.PLAYER_EXPERIENCE,
            engagement=EngagementMetrics(
                views=3200,
                shares=95,
                comments=62,
                reactions=280,
                saves=85,
                time_spent_seconds=310,
                completion_rate=0.75
            ),
            social=SocialMetrics(
                author_followers=220,
                author_reputation=65.0,
                guild_size=42,
                alliance_reach=150,
                cross_guild_shares=28
            ),
            quality=ContentQuality(
                word_count=1250,
                has_images=True,
                has_video=True,
                uniqueness_score=0.78,
                emotional_impact=0.92,
                narrative_quality=0.85
            ),
            created_timestamp=current_time - 43200  # 12 hours ago
        ),
        
        Story(
            story_id="story_003",
            title="Settlement Defense: How We Survived the Great Siege",
            story_type=StoryType.SETTLEMENT_STORY,
            engagement=EngagementMetrics(
                views=12000,
                shares=580,
                comments=320,
                reactions=1500,
                saves=450,
                time_spent_seconds=280,
                completion_rate=0.92
            ),
            social=SocialMetrics(
                author_followers=1200,
                author_reputation=88.5,
                guild_size=150,
                alliance_reach=650,
                cross_guild_shares=220
            ),
            quality=ContentQuality(
                word_count=1450,
                has_images=True,
                has_video=True,
                uniqueness_score=0.95,
                emotional_impact=0.96,
                narrative_quality=0.92
            ),
            created_timestamp=current_time - 7200  # 2 hours ago
        ),
        
        Story(
            story_id="story_004",
            title="First to 100 Mining Skill: Tips and Tricks",
            story_type=StoryType.ACHIEVEMENT_STORY,
            engagement=EngagementMetrics(
                views=5500,
                shares=180,
                comments=95,
                reactions=420,
                saves=165,
                time_spent_seconds=190,
                completion_rate=0.82
            ),
            social=SocialMetrics(
                author_followers=480,
                author_reputation=72.0,
                guild_size=65,
                alliance_reach=220,
                cross_guild_shares=58
            ),
            quality=ContentQuality(
                word_count=620,
                has_images=False,
                has_video=False,
                uniqueness_score=0.68,
                emotional_impact=0.65,
                narrative_quality=0.72
            ),
            created_timestamp=current_time - 86400  # 1 day ago
        ),
        
        Story(
            story_id="story_005",
            title="The Cathedral Project: A Community Achievement",
            story_type=StoryType.QUEST_NARRATIVE,
            engagement=EngagementMetrics(
                views=6800,
                shares=280,
                comments=145,
                reactions=720,
                saves=215,
                time_spent_seconds=250,
                completion_rate=0.86
            ),
            social=SocialMetrics(
                author_followers=820,
                author_reputation=81.0,
                guild_size=95,
                alliance_reach=380,
                cross_guild_shares=95
            ),
            quality=ContentQuality(
                word_count=950,
                has_images=True,
                has_video=False,
                uniqueness_score=0.88,
                emotional_impact=0.89,
                narrative_quality=0.90
            ),
            created_timestamp=current_time - 14400  # 4 hours ago
        ),
        
        Story(
            story_id="story_006",
            title="Quick Mining Spot Near Settlement",
            story_type=StoryType.DISCOVERY_STORY,
            engagement=EngagementMetrics(
                views=1200,
                shares=35,
                comments=18,
                reactions=90,
                saves=25,
                time_spent_seconds=85,
                completion_rate=0.65
            ),
            social=SocialMetrics(
                author_followers=80,
                author_reputation=45.0,
                guild_size=25,
                alliance_reach=60,
                cross_guild_shares=8
            ),
            quality=ContentQuality(
                word_count=180,
                has_images=False,
                has_video=False,
                uniqueness_score=0.45,
                emotional_impact=0.35,
                narrative_quality=0.50
            ),
            created_timestamp=current_time - 172800  # 2 days ago
        ),
    ]


def main():
    """Main demonstration function"""
    print("=" * 80)
    print("Story Viral Ranking System - Usage Example")
    print("=" * 80)
    print()
    
    # Create sample stories
    stories = create_sample_stories()
    print(f"Created {len(stories)} sample stories")
    print()
    
    # Initialize ranker
    ranker = StoryRanker()
    current_time = time.time()
    
    # Example 1: Rank all stories
    print("1. RANKING ALL STORIES")
    print("-" * 80)
    ranked = ranker.rank_stories(stories, current_time)
    
    for story in ranked:
        print(f"#{story.viral_rank}: {story.title}")
        print(f"   Type: {story.story_type.value}")
        print(f"   Viral Score: {story.viral_score:.2f}/100")
        print(f"   Engagement: {story.engagement.views:,} views, "
              f"{story.engagement.shares} shares, "
              f"{story.engagement.comments} comments")
        print(f"   Social: {story.social.cross_guild_shares} cross-guild shares, "
              f"{story.social.author_followers} followers")
        print()
    
    # Example 2: Get top 3 stories
    print("\n2. TOP 3 STORIES")
    print("-" * 80)
    top_3 = ranker.get_top_stories(stories, current_time, top_n=3)
    
    for story in top_3:
        print(f"• {story.title}")
        print(f"  Score: {story.viral_score:.2f}, "
              f"Shares: {story.engagement.shares}, "
              f"Cross-Guild: {story.social.cross_guild_shares}")
    print()
    
    # Example 3: Get trending stories (last 24 hours)
    print("\n3. TRENDING STORIES (Last 24 Hours)")
    print("-" * 80)
    trending = ranker.get_trending_stories(stories, current_time, 
                                           recency_hours=24, top_n=5)
    
    if trending:
        for story in trending:
            age_hours = (current_time - story.created_timestamp) / 3600
            print(f"• {story.title}")
            print(f"  Score: {story.viral_score:.2f}, "
                  f"Age: {age_hours:.1f} hours, "
                  f"Views: {story.engagement.views:,}")
    else:
        print("No trending stories found")
    print()
    
    # Example 4: Filter by story type
    print("\n4. DISCOVERY STORIES ONLY")
    print("-" * 80)
    discovery = ranker.get_stories_by_type(
        stories, current_time, 
        StoryType.DISCOVERY_STORY, top_n=5
    )
    
    for story in discovery:
        print(f"• {story.title}")
        print(f"  Score: {story.viral_score:.2f}, "
              f"Rank: #{story.viral_rank}")
    print()
    
    # Example 5: Analyze viral factors
    print("\n5. VIRAL FACTOR ANALYSIS (Top Story)")
    print("-" * 80)
    top_story = ranked[0]
    calculator = ranker.calculator
    
    engagement_score = calculator.calculate_engagement_score(top_story.engagement)
    social_score = calculator.calculate_social_score(top_story.social)
    quality_score = calculator.calculate_quality_score(top_story.quality)
    recency_score = calculator.calculate_recency_score(
        top_story.created_timestamp, current_time
    )
    
    print(f"Story: {top_story.title}")
    print(f"Overall Viral Score: {top_story.viral_score:.2f}/100")
    print()
    print("Component Breakdown:")
    print(f"  Engagement: {engagement_score:.2f}/100 (40% weight)")
    print(f"  Social:     {social_score:.2f}/100 (30% weight)")
    print(f"  Quality:    {quality_score:.2f}/100 (20% weight)")
    print(f"  Recency:    {recency_score:.2f}/100 (10% weight)")
    print()
    print("Key Success Factors:")
    if top_story.social.cross_guild_shares > 100:
        print("  ✓ Excellent cross-guild sharing (high viral potential)")
    if top_story.engagement.completion_rate > 0.85:
        print("  ✓ High completion rate (engaging content)")
    if top_story.quality.has_video:
        print("  ✓ Includes video (quality bonus)")
    if top_story.quality.emotional_impact > 0.85:
        print("  ✓ Strong emotional impact")
    if top_story.engagement.shares > 400:
        print("  ✓ High share count (viral indicator)")
    
    print()
    print("=" * 80)
    print("Example completed successfully!")
    print("=" * 80)


if __name__ == "__main__":
    main()
