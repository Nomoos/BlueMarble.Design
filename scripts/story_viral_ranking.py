#!/usr/bin/env python3
"""
Story Viral Potential Ranking System for BlueMarble

This module provides functionality to rank player-generated stories, quest narratives,
and content by their viral potential based on engagement metrics and social factors.
"""

from typing import Dict, List, Optional, Tuple
from dataclasses import dataclass
from enum import Enum
import math


class StoryType(Enum):
    """Types of stories that can be ranked"""
    PLAYER_EXPERIENCE = "player_experience"
    QUEST_NARRATIVE = "quest_narrative"
    SETTLEMENT_STORY = "settlement_story"
    ACHIEVEMENT_STORY = "achievement_story"
    DISCOVERY_STORY = "discovery_story"


@dataclass
class EngagementMetrics:
    """Metrics used to calculate viral potential"""
    views: int = 0
    shares: int = 0
    comments: int = 0
    reactions: int = 0
    saves: int = 0
    time_spent_seconds: float = 0.0
    completion_rate: float = 0.0  # 0.0 to 1.0


@dataclass
class SocialMetrics:
    """Social network factors affecting viral potential"""
    author_followers: int = 0
    author_reputation: float = 0.0  # 0.0 to 100.0
    guild_size: int = 0
    alliance_reach: int = 0
    cross_guild_shares: int = 0


@dataclass
class ContentQuality:
    """Quality indicators of the content"""
    word_count: int = 0
    has_images: bool = False
    has_video: bool = False
    uniqueness_score: float = 0.0  # 0.0 to 1.0
    emotional_impact: float = 0.0  # 0.0 to 1.0
    narrative_quality: float = 0.0  # 0.0 to 1.0


@dataclass
class Story:
    """Represents a story to be ranked"""
    story_id: str
    title: str
    story_type: StoryType
    engagement: EngagementMetrics
    social: SocialMetrics
    quality: ContentQuality
    created_timestamp: float
    
    def __post_init__(self):
        self.viral_score: Optional[float] = None
        self.viral_rank: Optional[int] = None


class ViralPotentialCalculator:
    """
    Calculates viral potential scores for stories based on multiple factors.
    
    The algorithm considers:
    - Engagement metrics (views, shares, reactions)
    - Social network effects (reach, cross-guild sharing)
    - Content quality (uniqueness, emotional impact)
    - Temporal decay (recency)
    """
    
    # Weight factors for different components
    ENGAGEMENT_WEIGHT = 0.40
    SOCIAL_WEIGHT = 0.30
    QUALITY_WEIGHT = 0.20
    RECENCY_WEIGHT = 0.10
    
    # Engagement metric weights
    SHARE_MULTIPLIER = 10.0  # Shares are most valuable
    REACTION_MULTIPLIER = 2.0
    COMMENT_MULTIPLIER = 3.0
    SAVE_MULTIPLIER = 5.0
    VIEW_MULTIPLIER = 1.0
    
    # Time decay parameters (in seconds)
    HALF_LIFE = 86400 * 3  # 3 days
    
    def calculate_engagement_score(self, metrics: EngagementMetrics) -> float:
        """
        Calculate engagement score from metrics.
        
        Formula emphasizes shares and saves over passive views.
        """
        if metrics.views == 0:
            return 0.0
        
        # Weighted engagement points
        engagement_points = (
            metrics.shares * self.SHARE_MULTIPLIER +
            metrics.reactions * self.REACTION_MULTIPLIER +
            metrics.comments * self.COMMENT_MULTIPLIER +
            metrics.saves * self.SAVE_MULTIPLIER +
            metrics.views * self.VIEW_MULTIPLIER
        )
        
        # Normalize by views (engagement rate)
        engagement_rate = engagement_points / metrics.views
        
        # Apply completion rate multiplier
        completion_bonus = 1.0 + (metrics.completion_rate * 0.5)
        
        # Apply time spent multiplier (longer engagement = more interesting)
        time_bonus = min(1.0 + (metrics.time_spent_seconds / 300.0), 2.0)  # Cap at 2x
        
        score = engagement_rate * completion_bonus * time_bonus
        
        # Normalize to 0-100 scale (logarithmic to handle outliers)
        return min(100.0, math.log1p(score) * 10.0)
    
    def calculate_social_score(self, social: SocialMetrics) -> float:
        """
        Calculate social network reach score.
        
        Emphasizes cross-guild sharing (broader reach) over single-guild virality.
        """
        # Author influence base
        author_score = math.log1p(social.author_followers) * 2.0
        reputation_bonus = social.author_reputation * 0.1
        
        # Network effects
        guild_reach = math.log1p(social.guild_size) * 1.5
        alliance_reach = math.log1p(social.alliance_reach) * 2.0
        
        # Cross-guild sharing is key indicator of viral potential
        cross_guild_bonus = social.cross_guild_shares * 15.0
        
        total_score = (
            author_score +
            reputation_bonus +
            guild_reach +
            alliance_reach +
            cross_guild_bonus
        )
        
        # Normalize to 0-100 scale
        return min(100.0, total_score)
    
    def calculate_quality_score(self, quality: ContentQuality) -> float:
        """
        Calculate content quality score.
        
        High-quality content has better long-term viral potential.
        """
        # Base quality from metrics
        base_score = (
            quality.uniqueness_score * 30.0 +
            quality.emotional_impact * 30.0 +
            quality.narrative_quality * 30.0
        )
        
        # Word count bonus (substantial content)
        word_bonus = 0.0
        if 100 <= quality.word_count < 300:
            word_bonus = 3.0
        elif 300 <= quality.word_count < 1000:
            word_bonus = 5.0
        elif quality.word_count >= 1000:
            word_bonus = 7.0
        
        # Media bonuses
        media_bonus = 0.0
        if quality.has_images:
            media_bonus += 2.0
        if quality.has_video:
            media_bonus += 3.0
        
        total_score = base_score + word_bonus + media_bonus
        
        return min(100.0, total_score)
    
    def calculate_recency_score(self, created_timestamp: float, 
                                current_timestamp: float) -> float:
        """
        Calculate recency score using exponential decay.
        
        Recent content gets higher scores, with half-life of 3 days.
        """
        age_seconds = current_timestamp - created_timestamp
        
        if age_seconds < 0:
            return 100.0  # Future content (edge case)
        
        # Exponential decay with half-life
        decay_factor = math.exp(-age_seconds * math.log(2) / self.HALF_LIFE)
        
        return decay_factor * 100.0
    
    def calculate_viral_score(self, story: Story, 
                             current_timestamp: float) -> float:
        """
        Calculate overall viral potential score for a story.
        
        Returns a score from 0-100 indicating viral potential.
        Higher scores indicate content more likely to go viral.
        """
        engagement_score = self.calculate_engagement_score(story.engagement)
        social_score = self.calculate_social_score(story.social)
        quality_score = self.calculate_quality_score(story.quality)
        recency_score = self.calculate_recency_score(
            story.created_timestamp, current_timestamp
        )
        
        # Weighted combination
        viral_score = (
            engagement_score * self.ENGAGEMENT_WEIGHT +
            social_score * self.SOCIAL_WEIGHT +
            quality_score * self.QUALITY_WEIGHT +
            recency_score * self.RECENCY_WEIGHT
        )
        
        return viral_score


class StoryRanker:
    """
    Ranks a collection of stories by their viral potential.
    """
    
    def __init__(self):
        self.calculator = ViralPotentialCalculator()
    
    def rank_stories(self, stories: List[Story], 
                    current_timestamp: float) -> List[Story]:
        """
        Rank stories by viral potential.
        
        Args:
            stories: List of stories to rank
            current_timestamp: Current time for recency calculation
            
        Returns:
            List of stories sorted by viral score (highest first)
        """
        # Calculate viral scores
        for story in stories:
            story.viral_score = self.calculator.calculate_viral_score(
                story, current_timestamp
            )
        
        # Sort by viral score (descending)
        ranked_stories = sorted(
            stories,
            key=lambda s: s.viral_score if s.viral_score is not None else 0.0,
            reverse=True
        )
        
        # Assign ranks
        for rank, story in enumerate(ranked_stories, start=1):
            story.viral_rank = rank
        
        return ranked_stories
    
    def get_top_stories(self, stories: List[Story], 
                       current_timestamp: float,
                       top_n: int = 10) -> List[Story]:
        """
        Get top N stories by viral potential.
        
        Args:
            stories: List of stories to rank
            current_timestamp: Current time for recency calculation
            top_n: Number of top stories to return
            
        Returns:
            List of top N stories by viral score
        """
        ranked = self.rank_stories(stories, current_timestamp)
        return ranked[:top_n]
    
    def get_stories_by_type(self, stories: List[Story],
                           current_timestamp: float,
                           story_type: StoryType,
                           top_n: int = 10) -> List[Story]:
        """
        Get top stories of a specific type.
        
        Args:
            stories: List of stories to rank
            current_timestamp: Current time for recency calculation
            story_type: Type of story to filter for
            top_n: Number of top stories to return
            
        Returns:
            List of top N stories of the specified type
        """
        filtered = [s for s in stories if s.story_type == story_type]
        ranked = self.rank_stories(filtered, current_timestamp)
        return ranked[:top_n]
    
    def get_trending_stories(self, stories: List[Story],
                            current_timestamp: float,
                            recency_hours: int = 24,
                            top_n: int = 10) -> List[Story]:
        """
        Get trending stories (recent + high engagement).
        
        Args:
            stories: List of stories to rank
            current_timestamp: Current time for recency calculation
            recency_hours: Only consider stories from last N hours
            top_n: Number of top stories to return
            
        Returns:
            List of trending stories
        """
        cutoff_time = current_timestamp - (recency_hours * 3600)
        recent = [s for s in stories if s.created_timestamp >= cutoff_time]
        
        if not recent:
            return []
        
        ranked = self.rank_stories(recent, current_timestamp)
        return ranked[:top_n]


# Example usage and testing
if __name__ == "__main__":
    import time
    
    # Create sample stories
    current_time = time.time()
    
    stories = [
        Story(
            story_id="story1",
            title="Epic Mining Discovery in Northern Peaks",
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
            created_timestamp=current_time - 3600  # 1 hour ago
        ),
        Story(
            story_id="story2",
            title="My Journey from Novice to Master Smith",
            story_type=StoryType.PLAYER_EXPERIENCE,
            engagement=EngagementMetrics(
                views=2000,
                shares=80,
                comments=45,
                reactions=200,
                saves=60,
                time_spent_seconds=240,
                completion_rate=0.70
            ),
            social=SocialMetrics(
                author_followers=150,
                author_reputation=55.0,
                guild_size=30,
                alliance_reach=80,
                cross_guild_shares=20
            ),
            quality=ContentQuality(
                word_count=800,
                has_images=True,
                has_video=True,
                uniqueness_score=0.7,
                emotional_impact=0.9,
                narrative_quality=0.8
            ),
            created_timestamp=current_time - 86400  # 1 day ago
        ),
        Story(
            story_id="story3",
            title="Settlement Defense: How We Survived the Siege",
            story_type=StoryType.SETTLEMENT_STORY,
            engagement=EngagementMetrics(
                views=8000,
                shares=400,
                comments=200,
                reactions=1200,
                saves=300,
                time_spent_seconds=200,
                completion_rate=0.90
            ),
            social=SocialMetrics(
                author_followers=800,
                author_reputation=85.0,
                guild_size=100,
                alliance_reach=500,
                cross_guild_shares=150
            ),
            quality=ContentQuality(
                word_count=1200,
                has_images=True,
                has_video=True,
                uniqueness_score=0.95,
                emotional_impact=0.95,
                narrative_quality=0.90
            ),
            created_timestamp=current_time - 7200  # 2 hours ago
        ),
    ]
    
    # Rank stories
    ranker = StoryRanker()
    ranked = ranker.rank_stories(stories, current_time)
    
    print("Story Viral Potential Rankings:")
    print("-" * 80)
    for story in ranked:
        print(f"Rank #{story.viral_rank}: {story.title}")
        print(f"  Type: {story.story_type.value}")
        print(f"  Viral Score: {story.viral_score:.2f}")
        print(f"  Engagement: {story.engagement.views} views, "
              f"{story.engagement.shares} shares")
        print()
