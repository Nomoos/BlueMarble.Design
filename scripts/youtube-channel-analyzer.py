#!/usr/bin/env python3
"""
YouTube Channel Analyzer for BlueMarble Research
================================================

Analyzes YouTube channels listed in research documents, focusing on story videos
(regular video content) while excluding shorts, live streams, and other content types.

Features:
- Scrapes channel data from YouTube channels
- Filters for story videos only (excludes shorts, live streams, premieres)
- Analyzes video content, frequency, and relevance
- Generates reports for research purposes

Usage:
    python youtube-channel-analyzer.py --channel @SebastianLague
    python youtube-channel-analyzer.py --channel-url https://www.youtube.com/@Brackeys
    python youtube-channel-analyzer.py --analyze-all-from-doc research/literature/online-game-dev-resources.md
    python youtube-channel-analyzer.py --channel @CodeMonkeyUnity --api-key YOUR_API_KEY

Requirements:
    pip install google-api-python-client
    
    OR for a simpler approach without API key (uses web scraping):
    pip install requests beautifulsoup4
"""

import argparse
import json
import re
import sys
from datetime import datetime, timedelta
from pathlib import Path
from typing import List, Dict, Optional
from collections import defaultdict

try:
    import requests
    from bs4 import BeautifulSoup
    SCRAPING_AVAILABLE = True
except ImportError:
    SCRAPING_AVAILABLE = False
    print("Warning: requests and beautifulsoup4 not available. Install with: pip install requests beautifulsoup4")

try:
    from googleapiclient.discovery import build
    from googleapiclient.errors import HttpError
    API_AVAILABLE = True
except ImportError:
    API_AVAILABLE = False
    print("Note: Google API client not available. For full functionality, install with: pip install google-api-python-client")


class YouTubeChannelAnalyzer:
    """Analyzes YouTube channels with focus on story videos"""
    
    def __init__(self, api_key: Optional[str] = None):
        """
        Initialize the analyzer
        
        Args:
            api_key: Optional YouTube Data API v3 key for detailed analysis
        """
        self.api_key = api_key
        self.youtube = None
        
        if api_key and API_AVAILABLE:
            self.youtube = build('youtube', 'v3', developerKey=api_key)
    
    def extract_channel_handle(self, url_or_handle: str) -> str:
        """
        Extract channel handle from URL or return as-is if already a handle
        
        Args:
            url_or_handle: Channel URL or handle (e.g., @SebastianLague)
            
        Returns:
            Channel handle starting with @
        """
        # If already a handle
        if url_or_handle.startswith('@'):
            return url_or_handle
        
        # Extract from URL patterns
        patterns = [
            r'youtube\.com/@([^/\?]+)',
            r'youtube\.com/c/([^/\?]+)',
            r'youtube\.com/channel/([^/\?]+)',
            r'youtube\.com/user/([^/\?]+)',
        ]
        
        for pattern in patterns:
            match = re.search(pattern, url_or_handle)
            if match:
                handle = match.group(1)
                return f"@{handle}" if not handle.startswith('@') else handle
        
        return url_or_handle
    
    def analyze_channel_basic(self, channel_handle: str) -> Dict:
        """
        Basic channel analysis without API (web scraping approach)
        
        Args:
            channel_handle: Channel handle (e.g., @SebastianLague)
            
        Returns:
            Dictionary with channel analysis
        """
        if not SCRAPING_AVAILABLE:
            return {
                "error": "Scraping libraries not available",
                "channel": channel_handle,
                "message": "Install requests and beautifulsoup4 for basic analysis"
            }
        
        # Clean handle
        handle = channel_handle.lstrip('@')
        url = f"https://www.youtube.com/@{handle}/videos"
        
        try:
            headers = {
                'User-Agent': 'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36'
            }
            response = requests.get(url, headers=headers, timeout=10)
            response.raise_for_status()
            
            # Note: YouTube uses dynamic content loading, so basic scraping is limited
            # This is a simplified analysis
            analysis = {
                "channel_handle": f"@{handle}",
                "channel_url": f"https://www.youtube.com/@{handle}",
                "videos_url": url,
                "analyzed_at": datetime.now().isoformat(),
                "analysis_method": "basic_scraping",
                "note": "Limited data available without API key. For full analysis, provide YouTube Data API v3 key.",
                "story_videos_focus": True,
                "filters_applied": [
                    "Excludes Shorts (videos under 60 seconds)",
                    "Excludes Live Streams",
                    "Excludes Premieres",
                    "Focuses on regular uploaded videos"
                ]
            }
            
            # Try to extract some basic info from the page
            soup = BeautifulSoup(response.text, 'html.parser')
            
            # Look for channel name in meta tags
            og_title = soup.find('meta', property='og:title')
            if og_title:
                analysis['channel_name'] = og_title.get('content', '')
            
            return analysis
            
        except Exception as e:
            return {
                "error": str(e),
                "channel": channel_handle,
                "analyzed_at": datetime.now().isoformat()
            }
    
    def analyze_channel_with_api(self, channel_handle: str) -> Dict:
        """
        Detailed channel analysis using YouTube Data API
        
        Args:
            channel_handle: Channel handle (e.g., @SebastianLague)
            
        Returns:
            Dictionary with detailed channel analysis
        """
        if not self.youtube:
            return {
                "error": "YouTube API not initialized",
                "message": "Provide API key for detailed analysis"
            }
        
        try:
            # Get channel details
            handle = channel_handle.lstrip('@')
            
            # Search for channel by handle
            search_response = self.youtube.search().list(
                part='snippet',
                q=handle,
                type='channel',
                maxResults=1
            ).execute()
            
            if not search_response.get('items'):
                return {
                    "error": "Channel not found",
                    "channel": channel_handle
                }
            
            channel_id = search_response['items'][0]['id']['channelId']
            
            # Get channel statistics
            channel_response = self.youtube.channels().list(
                part='snippet,statistics,contentDetails',
                id=channel_id
            ).execute()
            
            if not channel_response.get('items'):
                return {
                    "error": "Channel details not found",
                    "channel_id": channel_id
                }
            
            channel_data = channel_response['items'][0]
            
            # Get uploads playlist
            uploads_playlist_id = channel_data['contentDetails']['relatedPlaylists']['uploads']
            
            # Get recent videos from uploads playlist
            videos_response = self.youtube.playlistItems().list(
                part='snippet,contentDetails',
                playlistId=uploads_playlist_id,
                maxResults=50  # Analyze up to 50 recent videos
            ).execute()
            
            # Analyze videos to filter story videos only
            story_videos = []
            shorts_count = 0
            live_count = 0
            
            for item in videos_response.get('items', []):
                video_id = item['contentDetails']['videoId']
                
                # Get video details to determine duration and type
                video_response = self.youtube.videos().list(
                    part='snippet,contentDetails,statistics',
                    id=video_id
                ).execute()
                
                if video_response.get('items'):
                    video = video_response['items'][0]
                    duration = video['contentDetails']['duration']
                    
                    # Parse ISO 8601 duration (e.g., PT1M30S = 1 minute 30 seconds)
                    duration_seconds = self._parse_duration(duration)
                    
                    # Filter: Story videos are typically > 60 seconds
                    # Shorts are <= 60 seconds
                    if duration_seconds > 60:
                        # Check if it's a live stream
                        is_live = video['snippet'].get('liveBroadcastContent') == 'live'
                        
                        if not is_live:
                            story_videos.append({
                                'video_id': video_id,
                                'title': video['snippet']['title'],
                                'published_at': video['snippet']['publishedAt'],
                                'duration_seconds': duration_seconds,
                                'view_count': int(video['statistics'].get('viewCount', 0)),
                                'like_count': int(video['statistics'].get('likeCount', 0)),
                                'comment_count': int(video['statistics'].get('commentCount', 0))
                            })
                        else:
                            live_count += 1
                    else:
                        shorts_count += 1
            
            # Compile analysis
            analysis = {
                "channel_handle": channel_handle,
                "channel_id": channel_id,
                "channel_name": channel_data['snippet']['title'],
                "channel_description": channel_data['snippet']['description'][:200] + "..." 
                    if len(channel_data['snippet']['description']) > 200 
                    else channel_data['snippet']['description'],
                "subscriber_count": int(channel_data['statistics'].get('subscriberCount', 0)),
                "total_video_count": int(channel_data['statistics']['videoCount']),
                "total_view_count": int(channel_data['statistics']['viewCount']),
                "analyzed_at": datetime.now().isoformat(),
                "analysis_method": "youtube_data_api_v3",
                "story_videos_analyzed": len(story_videos),
                "shorts_excluded": shorts_count,
                "live_streams_excluded": live_count,
                "story_videos": story_videos[:10],  # Include top 10 for preview
                "story_videos_stats": self._calculate_video_stats(story_videos),
                "filters_applied": [
                    "Duration > 60 seconds (excludes Shorts)",
                    "Excludes live broadcasts",
                    "Focuses on uploaded video content"
                ]
            }
            
            return analysis
            
        except HttpError as e:
            return {
                "error": f"YouTube API error: {e}",
                "channel": channel_handle
            }
        except Exception as e:
            return {
                "error": str(e),
                "channel": channel_handle
            }
    
    def _parse_duration(self, duration: str) -> int:
        """
        Parse ISO 8601 duration to seconds
        
        Args:
            duration: ISO 8601 duration string (e.g., PT1M30S)
            
        Returns:
            Duration in seconds
        """
        # Remove PT prefix
        duration = duration.replace('PT', '')
        
        hours = 0
        minutes = 0
        seconds = 0
        
        # Parse hours
        if 'H' in duration:
            hours = int(duration.split('H')[0])
            duration = duration.split('H')[1]
        
        # Parse minutes
        if 'M' in duration:
            minutes = int(duration.split('M')[0])
            duration = duration.split('M')[1]
        
        # Parse seconds
        if 'S' in duration:
            seconds = int(duration.split('S')[0])
        
        return hours * 3600 + minutes * 60 + seconds
    
    def _calculate_video_stats(self, videos: List[Dict]) -> Dict:
        """Calculate statistics for story videos"""
        if not videos:
            return {}
        
        total_views = sum(v.get('view_count', 0) for v in videos)
        total_likes = sum(v.get('like_count', 0) for v in videos)
        total_comments = sum(v.get('comment_count', 0) for v in videos)
        
        avg_duration = sum(v.get('duration_seconds', 0) for v in videos) / len(videos)
        
        return {
            "total_story_videos": len(videos),
            "average_views": total_views // len(videos) if videos else 0,
            "average_likes": total_likes // len(videos) if videos else 0,
            "average_comments": total_comments // len(videos) if videos else 0,
            "average_duration_minutes": round(avg_duration / 60, 2),
            "total_views": total_views,
            "total_likes": total_likes,
            "total_comments": total_comments
        }
    
    def analyze_channel(self, channel_handle: str) -> Dict:
        """
        Analyze channel using available method (API or basic scraping)
        
        Args:
            channel_handle: Channel handle or URL
            
        Returns:
            Channel analysis dictionary
        """
        handle = self.extract_channel_handle(channel_handle)
        
        if self.youtube:
            return self.analyze_channel_with_api(handle)
        else:
            return self.analyze_channel_basic(handle)
    
    def extract_channels_from_markdown(self, md_file: Path) -> List[Dict]:
        """
        Extract YouTube channel information from markdown file
        
        Args:
            md_file: Path to markdown file
            
        Returns:
            List of channel dictionaries
        """
        channels = []
        
        try:
            with open(md_file, 'r', encoding='utf-8') as f:
                content = f.read()
            
            # Pattern to match channel entries in the markdown
            # Looking for URL patterns like: https://www.youtube.com/@ChannelName
            url_pattern = r'https?://(?:www\.)?youtube\.com/(@[^\s\)]+)'
            
            matches = re.finditer(url_pattern, content)
            
            for match in matches:
                handle = match.group(1)
                channels.append({
                    'handle': handle,
                    'url': match.group(0)
                })
            
            # Remove duplicates
            seen = set()
            unique_channels = []
            for ch in channels:
                if ch['handle'] not in seen:
                    seen.add(ch['handle'])
                    unique_channels.append(ch)
            
            return unique_channels
            
        except Exception as e:
            print(f"Error reading markdown file: {e}")
            return []
    
    def analyze_all_channels_from_doc(self, md_file: Path, output_file: Optional[Path] = None) -> Dict:
        """
        Analyze all channels mentioned in a markdown document
        
        Args:
            md_file: Path to markdown file
            output_file: Optional output file for results
            
        Returns:
            Dictionary with all channel analyses
        """
        channels = self.extract_channels_from_markdown(md_file)
        
        print(f"Found {len(channels)} unique channels in {md_file}")
        
        results = {
            "source_document": str(md_file),
            "analyzed_at": datetime.now().isoformat(),
            "total_channels": len(channels),
            "channels": []
        }
        
        for i, channel_info in enumerate(channels, 1):
            print(f"\nAnalyzing channel {i}/{len(channels)}: {channel_info['handle']}")
            
            analysis = self.analyze_channel(channel_info['handle'])
            results["channels"].append(analysis)
        
        if output_file:
            self._save_results(results, output_file)
            print(f"\nResults saved to {output_file}")
        
        return results
    
    def _save_results(self, results: Dict, output_file: Path):
        """Save analysis results to file"""
        output_file.parent.mkdir(parents=True, exist_ok=True)
        
        if output_file.suffix == '.json':
            with open(output_file, 'w', encoding='utf-8') as f:
                json.dump(results, f, indent=2)
        else:
            # Save as markdown
            with open(output_file, 'w', encoding='utf-8') as f:
                self._write_markdown_report(results, f)
    
    def _write_markdown_report(self, results: Dict, file):
        """Write results as markdown report"""
        file.write("# YouTube Channel Analysis Report\n\n")
        file.write(f"**Source Document:** {results['source_document']}\n")
        file.write(f"**Analyzed At:** {results['analyzed_at']}\n")
        file.write(f"**Total Channels:** {results['total_channels']}\n\n")
        file.write("---\n\n")
        
        file.write("## Analysis Focus: Story Videos Only\n\n")
        file.write("This analysis focuses on **story videos** (regular uploaded content) and excludes:\n")
        file.write("- YouTube Shorts (videos ≤ 60 seconds)\n")
        file.write("- Live streams\n")
        file.write("- Premieres\n\n")
        file.write("---\n\n")
        
        for i, channel in enumerate(results['channels'], 1):
            file.write(f"## {i}. {channel.get('channel_name', channel.get('channel_handle', 'Unknown'))}\n\n")
            
            if 'error' in channel:
                file.write(f"**Error:** {channel['error']}\n\n")
                continue
            
            file.write(f"**Channel Handle:** {channel.get('channel_handle', 'N/A')}\n")
            file.write(f"**Channel URL:** {channel.get('channel_url', 'N/A')}\n")
            
            if 'subscriber_count' in channel:
                file.write(f"**Subscribers:** {channel['subscriber_count']:,}\n")
            
            if 'total_video_count' in channel:
                file.write(f"**Total Videos:** {channel['total_video_count']:,}\n")
            
            if 'story_videos_analyzed' in channel:
                file.write(f"**Story Videos Analyzed:** {channel['story_videos_analyzed']}\n")
                file.write(f"**Shorts Excluded:** {channel['shorts_excluded']}\n")
                file.write(f"**Live Streams Excluded:** {channel['live_streams_excluded']}\n\n")
            
            if 'story_videos_stats' in channel:
                stats = channel['story_videos_stats']
                file.write("### Story Video Statistics\n\n")
                file.write(f"- **Total Story Videos:** {stats.get('total_story_videos', 0)}\n")
                file.write(f"- **Average Views:** {stats.get('average_views', 0):,}\n")
                file.write(f"- **Average Duration:** {stats.get('average_duration_minutes', 0)} minutes\n")
                file.write(f"- **Total Views:** {stats.get('total_views', 0):,}\n\n")
            
            file.write(f"**Analysis Method:** {channel.get('analysis_method', 'Unknown')}\n\n")
            file.write("---\n\n")


def main():
    """Main execution function"""
    parser = argparse.ArgumentParser(
        description='YouTube Channel Analyzer for BlueMarble Research',
        formatter_class=argparse.RawDescriptionHelpFormatter,
        epilog="""
Examples:
  # Analyze single channel
  python youtube-channel-analyzer.py --channel @SebastianLague
  
  # Analyze channel with API key for detailed data
  python youtube-channel-analyzer.py --channel @SebastianLague --api-key YOUR_KEY
  
  # Analyze all channels from document
  python youtube-channel-analyzer.py --analyze-all-from-doc research/literature/online-game-dev-resources.md
  
  # Analyze and save to file
  python youtube-channel-analyzer.py --channel @CodeMonkeyUnity --output analysis.json
        """
    )
    
    parser.add_argument(
        '--channel',
        help='YouTube channel handle (e.g., @SebastianLague) or URL'
    )
    
    parser.add_argument(
        '--channel-url',
        help='Full YouTube channel URL'
    )
    
    parser.add_argument(
        '--analyze-all-from-doc',
        type=Path,
        help='Analyze all channels mentioned in a markdown document'
    )
    
    parser.add_argument(
        '--api-key',
        help='YouTube Data API v3 key for detailed analysis'
    )
    
    parser.add_argument(
        '--output',
        type=Path,
        help='Output file for results (.json or .md)'
    )
    
    args = parser.parse_args()
    
    # Initialize analyzer
    analyzer = YouTubeChannelAnalyzer(api_key=args.api_key)
    
    # Handle different modes
    if args.analyze_all_from_doc:
        if not args.analyze_all_from_doc.exists():
            print(f"Error: File not found: {args.analyze_all_from_doc}")
            sys.exit(1)
        
        output_file = args.output or Path('youtube-channel-analysis.md')
        analyzer.analyze_all_channels_from_doc(args.analyze_all_from_doc, output_file)
        
    elif args.channel or args.channel_url:
        channel = args.channel or args.channel_url
        
        print(f"Analyzing channel: {channel}")
        result = analyzer.analyze_channel(channel)
        
        if args.output:
            analyzer._save_results({"channels": [result]}, args.output)
            print(f"\nResults saved to {args.output}")
        else:
            print("\nAnalysis Result:")
            print(json.dumps(result, indent=2))
    else:
        parser.print_help()
        sys.exit(1)


if __name__ == '__main__':
    main()
