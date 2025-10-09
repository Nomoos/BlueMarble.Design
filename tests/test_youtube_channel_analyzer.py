#!/usr/bin/env python3
"""
Unit tests for YouTube Channel Analyzer
"""

import unittest
import sys
from pathlib import Path
from datetime import datetime
import json
import tempfile
import importlib.util

# Import the module
spec = importlib.util.spec_from_file_location(
    "youtube_channel_analyzer", 
    "scripts/youtube-channel-analyzer.py"
)
youtube_analyzer = importlib.util.module_from_spec(spec)
spec.loader.exec_module(youtube_analyzer)

YouTubeChannelAnalyzer = youtube_analyzer.YouTubeChannelAnalyzer


class TestYouTubeChannelAnalyzer(unittest.TestCase):
    """Test cases for YouTube Channel Analyzer"""
    
    def setUp(self):
        """Set up test fixtures"""
        self.analyzer = YouTubeChannelAnalyzer()
    
    def test_extract_channel_handle_from_handle(self):
        """Test extracting handle when already a handle"""
        handle = self.analyzer.extract_channel_handle("@SebastianLague")
        self.assertEqual(handle, "@SebastianLague")
    
    def test_extract_channel_handle_from_url(self):
        """Test extracting handle from various URL formats"""
        test_cases = [
            ("https://www.youtube.com/@SebastianLague", "@SebastianLague"),
            ("https://youtube.com/@CodeMonkeyUnity/videos", "@CodeMonkeyUnity"),
            ("https://www.youtube.com/c/Brackeys", "@Brackeys"),
            ("https://www.youtube.com/@GameDevTV/about", "@GameDevTV"),
        ]
        
        for url, expected_handle in test_cases:
            with self.subTest(url=url):
                handle = self.analyzer.extract_channel_handle(url)
                self.assertTrue(handle.startswith("@"), f"Handle should start with @: {handle}")
    
    def test_parse_duration_simple(self):
        """Test parsing simple ISO 8601 durations"""
        test_cases = [
            ("PT1M30S", 90),      # 1 minute 30 seconds
            ("PT5M", 300),         # 5 minutes
            ("PT45S", 45),         # 45 seconds
            ("PT1H", 3600),        # 1 hour
            ("PT1H30M", 5400),     # 1 hour 30 minutes
            ("PT2H15M30S", 8130),  # 2 hours 15 minutes 30 seconds
        ]
        
        for duration_str, expected_seconds in test_cases:
            with self.subTest(duration=duration_str):
                seconds = self.analyzer._parse_duration(duration_str)
                self.assertEqual(seconds, expected_seconds)
    
    def test_calculate_video_stats_empty(self):
        """Test stats calculation with empty video list"""
        stats = self.analyzer._calculate_video_stats([])
        self.assertEqual(stats, {})
    
    def test_calculate_video_stats(self):
        """Test stats calculation with sample videos"""
        videos = [
            {
                'video_id': 'vid1',
                'view_count': 1000,
                'like_count': 100,
                'comment_count': 10,
                'duration_seconds': 600
            },
            {
                'video_id': 'vid2',
                'view_count': 2000,
                'like_count': 200,
                'comment_count': 20,
                'duration_seconds': 1200
            }
        ]
        
        stats = self.analyzer._calculate_video_stats(videos)
        
        self.assertEqual(stats['total_story_videos'], 2)
        self.assertEqual(stats['average_views'], 1500)
        self.assertEqual(stats['average_likes'], 150)
        self.assertEqual(stats['average_comments'], 15)
        self.assertEqual(stats['total_views'], 3000)
        self.assertEqual(stats['average_duration_minutes'], 15.0)
    
    def test_extract_channels_from_markdown(self):
        """Test extracting channels from markdown file"""
        # Use the actual research document
        md_file = Path("research/literature/online-game-dev-resources.md")
        
        if not md_file.exists():
            self.skipTest("Research document not found")
        
        channels = self.analyzer.extract_channels_from_markdown(md_file)
        
        # Should find multiple channels
        self.assertGreater(len(channels), 0, "Should extract at least one channel")
        
        # Each channel should have handle and url
        for channel in channels:
            self.assertIn('handle', channel)
            self.assertIn('url', channel)
            self.assertTrue(channel['handle'].startswith('@'))
    
    def test_story_video_filtering_logic(self):
        """Test that story video filtering logic is correct"""
        # Story videos: duration > 60 seconds
        self.assertGreater(
            self.analyzer._parse_duration("PT1M1S"), 
            60,
            "Videos over 60 seconds should be story videos"
        )
        
        # Shorts: duration <= 60 seconds
        self.assertLessEqual(
            self.analyzer._parse_duration("PT59S"),
            60,
            "Videos 60 seconds or less should be shorts"
        )
        
        self.assertLessEqual(
            self.analyzer._parse_duration("PT1M"),
            60,
            "Videos exactly 60 seconds should be shorts"
        )
    
    def test_save_results_json(self):
        """Test saving results as JSON"""
        results = {
            "source_document": "test.md",
            "analyzed_at": datetime.now().isoformat(),
            "channels": [
                {
                    "channel_handle": "@TestChannel",
                    "story_videos_analyzed": 10
                }
            ]
        }
        
        with tempfile.NamedTemporaryFile(mode='w', suffix='.json', delete=False) as f:
            temp_file = Path(f.name)
        
        try:
            self.analyzer._save_results(results, temp_file)
            
            # Verify file was created and is valid JSON
            self.assertTrue(temp_file.exists())
            
            with open(temp_file, 'r') as f:
                loaded = json.load(f)
                self.assertEqual(loaded['source_document'], 'test.md')
        finally:
            if temp_file.exists():
                temp_file.unlink()
    
    def test_save_results_markdown(self):
        """Test saving results as Markdown"""
        results = {
            "source_document": "test.md",
            "analyzed_at": datetime.now().isoformat(),
            "total_channels": 1,
            "channels": [
                {
                    "channel_handle": "@TestChannel",
                    "channel_name": "Test Channel",
                    "story_videos_analyzed": 10,
                    "shorts_excluded": 5,
                    "live_streams_excluded": 2
                }
            ]
        }
        
        with tempfile.NamedTemporaryFile(mode='w', suffix='.md', delete=False) as f:
            temp_file = Path(f.name)
        
        try:
            self.analyzer._save_results(results, temp_file)
            
            # Verify file was created
            self.assertTrue(temp_file.exists())
            
            # Verify content has expected sections
            with open(temp_file, 'r') as f:
                content = f.read()
                self.assertIn('YouTube Channel Analysis Report', content)
                self.assertIn('Story Videos Only', content)
                self.assertIn('Test Channel', content)
        finally:
            if temp_file.exists():
                temp_file.unlink()


class TestStoryVideoFiltering(unittest.TestCase):
    """Test cases specifically for story video filtering logic"""
    
    def test_duration_thresholds(self):
        """Test duration-based filtering thresholds"""
        analyzer = YouTubeChannelAnalyzer()
        
        # These should be classified as shorts (excluded)
        shorts_durations = ["PT30S", "PT45S", "PT1M", "PT59S"]
        for duration in shorts_durations:
            seconds = analyzer._parse_duration(duration)
            self.assertLessEqual(
                seconds, 60, 
                f"{duration} should be <= 60 seconds (short)"
            )
        
        # These should be classified as story videos (included)
        story_durations = ["PT1M1S", "PT2M", "PT5M30S", "PT15M", "PT1H"]
        for duration in story_durations:
            seconds = analyzer._parse_duration(duration)
            self.assertGreater(
                seconds, 60,
                f"{duration} should be > 60 seconds (story video)"
            )


def run_tests():
    """Run all tests"""
    # Create test suite
    loader = unittest.TestLoader()
    suite = unittest.TestSuite()
    
    # Add all test cases
    suite.addTests(loader.loadTestsFromTestCase(TestYouTubeChannelAnalyzer))
    suite.addTests(loader.loadTestsFromTestCase(TestStoryVideoFiltering))
    
    # Run tests
    runner = unittest.TextTestRunner(verbosity=2)
    result = runner.run(suite)
    
    # Return exit code
    return 0 if result.wasSuccessful() else 1


if __name__ == '__main__':
    sys.exit(run_tests())
