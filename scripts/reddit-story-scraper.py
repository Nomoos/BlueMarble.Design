#!/usr/bin/env python3
"""
Reddit Story Scraper for BlueMarble Research
============================================

Mines stories, player experiences, and design insights from Reddit for BlueMarble MMORPG research.

This tool collects:
- Player experience stories
- Game design discussions
- Community feedback and pain points
- Success/failure case studies
- Technical discussions

Usage:
    python reddit-story-scraper.py [options]

Options:
    --subreddit SUBREDDIT   Target subreddit (default: MMORPG)
    --limit N               Number of posts to fetch (default: 100)
    --timeframe TIME        Timeframe: hour, day, week, month, year, all (default: month)
    --sort SORT             Sort by: hot, new, top, rising (default: top)
    --keywords KEYWORDS     Comma-separated keywords to filter posts
    --min-score N           Minimum score for posts (default: 10)
    --output FILE           Output file (default: reddit-stories-{timestamp}.json)
    --format FORMAT         Output format: json, markdown (default: json)
    --include-comments      Include top comments in output (default: False)
    --test-mode             Use sample data for testing (no network required)
"""

import re
import sys
import json
import time
import argparse
import requests
from datetime import datetime
from pathlib import Path
from typing import List, Dict, Optional
from urllib.parse import quote


class RedditStoryScraper:
    """Scraper for collecting stories and insights from Reddit"""
    
    def __init__(self, user_agent="BlueMarble Research Bot 1.0", test_mode=False):
        self.user_agent = user_agent
        self.base_url = "https://www.reddit.com"
        self.session = requests.Session()
        self.session.headers.update({'User-Agent': self.user_agent})
        self.test_mode = test_mode
        
    def fetch_posts(self, subreddit: str, sort: str = "top", 
                   timeframe: str = "month", limit: int = 100) -> List[Dict]:
        """
        Fetch posts from a subreddit using Reddit's JSON API
        
        Args:
            subreddit: Subreddit name (without /r/)
            sort: Sort method (hot, new, top, rising)
            timeframe: Time filter for top/controversial (hour, day, week, month, year, all)
            limit: Maximum number of posts to fetch
            
        Returns:
            List of post dictionaries
        """
        # Test mode: return sample data
        if self.test_mode:
            return self._get_sample_posts(subreddit, limit)
        
        posts = []
        after = None
        
        # Reddit API limits to 100 per request
        requests_needed = (limit + 99) // 100
        
        print(f"Fetching posts from r/{subreddit} (sort={sort}, timeframe={timeframe})...")
        
        for request_num in range(requests_needed):
            # Build URL
            url = f"{self.base_url}/r/{subreddit}/{sort}.json"
            
            params = {
                'limit': min(100, limit - len(posts)),
                't': timeframe,
            }
            
            if after:
                params['after'] = after
            
            try:
                response = self.session.get(url, params=params, timeout=10)
                response.raise_for_status()
                
                data = response.json()
                
                if 'data' not in data or 'children' not in data['data']:
                    print(f"Warning: Unexpected response format")
                    break
                
                children = data['data']['children']
                
                if not children:
                    print(f"No more posts available")
                    break
                
                for child in children:
                    if child['kind'] == 't3':  # t3 = link/post
                        post_data = child['data']
                        posts.append(self._extract_post_data(post_data))
                
                after = data['data'].get('after')
                
                print(f"  Fetched {len(posts)} posts...")
                
                if not after or len(posts) >= limit:
                    break
                
                # Be nice to Reddit's API
                time.sleep(1)
                
            except requests.exceptions.RequestException as e:
                print(f"Error fetching posts: {e}")
                break
        
        print(f"Fetched {len(posts)} total posts")
        return posts[:limit]
    
    def _get_sample_posts(self, subreddit: str, limit: int) -> List[Dict]:
        """Generate sample posts for testing"""
        print(f"TEST MODE: Generating {limit} sample posts for r/{subreddit}...")
        
        sample_posts = [
            {
                'id': 'sample1',
                'title': 'What MMORPG has the best crafting system?',
                'author': 'test_user_1',
                'score': 245,
                'num_comments': 67,
                'created_utc': 1705420800,
                'created_date': '2025-01-15T14:20:00',
                'url': f'{self.base_url}/r/{subreddit}/comments/sample1',
                'selftext': 'Looking for an MMORPG with a deep, engaging crafting system. What are your favorites?',
                'link_flair_text': 'Discussion',
                'upvote_ratio': 0.95,
                'subreddit': subreddit,
            },
            {
                'id': 'sample2',
                'title': 'Economy design: Why EVE Online works and others fail',
                'author': 'test_user_2',
                'score': 580,
                'num_comments': 123,
                'created_utc': 1705334400,
                'created_date': '2025-01-14T10:20:00',
                'url': f'{self.base_url}/r/{subreddit}/comments/sample2',
                'selftext': 'EVE Online has maintained a player-driven economy for 20 years. What makes it work? Player agency, meaningful destruction, complex supply chains...',
                'link_flair_text': 'Analysis',
                'upvote_ratio': 0.97,
                'subreddit': subreddit,
            },
            {
                'id': 'sample3',
                'title': 'My experience with FFXIV housing: Good and bad',
                'author': 'test_user_3',
                'score': 156,
                'num_comments': 45,
                'created_utc': 1705248000,
                'created_date': '2025-01-13T18:00:00',
                'url': f'{self.base_url}/r/{subreddit}/comments/sample3',
                'selftext': 'Housing in FFXIV is amazing when you can get it, but the scarcity creates frustration...',
                'link_flair_text': 'Discussion',
                'upvote_ratio': 0.89,
                'subreddit': subreddit,
            },
            {
                'id': 'sample4',
                'title': 'Why do modern MMORPGs fail? A developer perspective',
                'author': 'test_developer',
                'score': 892,
                'num_comments': 234,
                'created_utc': 1705161600,
                'created_date': '2025-01-12T14:00:00',
                'url': f'{self.base_url}/r/{subreddit}/comments/sample4',
                'selftext': 'As a game developer, I see recurring patterns in failed MMORPGs: lack of endgame, poor community management, pay-to-win...',
                'link_flair_text': 'Industry',
                'upvote_ratio': 0.96,
                'subreddit': subreddit,
            },
            {
                'id': 'sample5',
                'title': 'Guild management tools: What we need',
                'author': 'guild_leader',
                'score': 67,
                'num_comments': 23,
                'created_utc': 1705075200,
                'created_date': '2025-01-11T10:00:00',
                'url': f'{self.base_url}/r/{subreddit}/comments/sample5',
                'selftext': 'After managing a 200-player guild for 5 years, here are the tools every MMORPG needs...',
                'link_flair_text': 'Suggestion',
                'upvote_ratio': 0.92,
                'subreddit': subreddit,
            },
        ]
        
        # Return requested number of posts (cycling if needed)
        result = []
        for i in range(limit):
            post = sample_posts[i % len(sample_posts)].copy()
            post['id'] = f"sample{i+1}"
            result.append(post)
        
        print(f"Generated {len(result)} sample posts")
        return result
    
    def _extract_post_data(self, post: Dict) -> Dict:
        """Extract relevant data from a Reddit post"""
        return {
            'id': post.get('id'),
            'title': post.get('title'),
            'author': post.get('author'),
            'score': post.get('score', 0),
            'num_comments': post.get('num_comments', 0),
            'created_utc': post.get('created_utc'),
            'created_date': datetime.fromtimestamp(post.get('created_utc', 0)).isoformat(),
            'url': f"{self.base_url}{post.get('permalink')}",
            'selftext': post.get('selftext', ''),
            'link_flair_text': post.get('link_flair_text'),
            'upvote_ratio': post.get('upvote_ratio', 0),
            'subreddit': post.get('subreddit'),
        }
    
    def fetch_comments(self, post_id: str, subreddit: str, limit: int = 20) -> List[Dict]:
        """
        Fetch top comments for a post
        
        Args:
            post_id: Reddit post ID
            subreddit: Subreddit name
            limit: Number of top comments to fetch
            
        Returns:
            List of comment dictionaries
        """
        # Test mode: return sample comments
        if self.test_mode:
            return [
                {
                    'id': f'comment_{post_id}_1',
                    'author': 'commenter_1',
                    'score': 45,
                    'body': 'Great points! I completely agree with your analysis.',
                    'created_utc': 1705420900,
                },
                {
                    'id': f'comment_{post_id}_2',
                    'author': 'commenter_2',
                    'score': 32,
                    'body': 'This is exactly what the industry needs to understand.',
                    'created_utc': 1705421000,
                },
            ]
        
        url = f"{self.base_url}/r/{subreddit}/comments/{post_id}.json"
        
        try:
            response = self.session.get(url, params={'limit': limit}, timeout=10)
            response.raise_for_status()
            
            data = response.json()
            
            if len(data) < 2:
                return []
            
            comments_data = data[1]['data']['children']
            comments = []
            
            for child in comments_data:
                if child['kind'] == 't1':  # t1 = comment
                    comment_data = child['data']
                    comments.append({
                        'id': comment_data.get('id'),
                        'author': comment_data.get('author'),
                        'score': comment_data.get('score', 0),
                        'body': comment_data.get('body', ''),
                        'created_utc': comment_data.get('created_utc'),
                    })
            
            return comments
            
        except requests.exceptions.RequestException as e:
            print(f"Error fetching comments for post {post_id}: {e}")
            return []
    
    def filter_posts(self, posts: List[Dict], keywords: List[str] = None,
                    min_score: int = 0) -> List[Dict]:
        """
        Filter posts by keywords and minimum score
        
        Args:
            posts: List of post dictionaries
            keywords: List of keywords to search for (case-insensitive)
            min_score: Minimum score threshold
            
        Returns:
            Filtered list of posts
        """
        filtered = []
        
        for post in posts:
            # Check minimum score
            if post['score'] < min_score:
                continue
            
            # Check keywords if provided
            if keywords:
                text = (post['title'] + ' ' + post['selftext']).lower()
                if not any(keyword.lower() in text for keyword in keywords):
                    continue
            
            filtered.append(post)
        
        return filtered
    
    def categorize_post(self, post: Dict) -> str:
        """
        Categorize a post based on its content
        
        Returns category string
        """
        title = post['title'].lower()
        text = post['selftext'].lower()
        combined = title + ' ' + text
        
        # Category keywords
        categories = {
            'player_experience': ['experience', 'story', 'remember', 'nostalgia', 'played'],
            'game_design': ['design', 'mechanic', 'feature', 'system', 'balance'],
            'feedback': ['problem', 'issue', 'frustrated', 'hate', 'love', 'wish'],
            'recommendation': ['recommend', 'looking for', 'should i play', 'worth'],
            'technical': ['lag', 'performance', 'fps', 'optimization', 'server'],
            'community': ['guild', 'clan', 'community', 'player', 'toxic'],
            'economy': ['economy', 'trading', 'auction', 'gold', 'market'],
        }
        
        # Score each category
        scores = {}
        for category, keywords in categories.items():
            scores[category] = sum(1 for keyword in keywords if keyword in combined)
        
        # Return category with highest score, or 'general' if no matches
        if not any(scores.values()):
            return 'general'
        
        return max(scores, key=scores.get)


def save_as_json(posts: List[Dict], output_file: str):
    """Save posts as JSON file"""
    output_path = Path(output_file)
    
    # Add metadata
    data = {
        'metadata': {
            'collected_at': datetime.now().isoformat(),
            'total_posts': len(posts),
            'tool': 'reddit-story-scraper.py',
            'version': '1.0',
        },
        'posts': posts
    }
    
    with output_path.open('w', encoding='utf-8') as f:
        json.dump(data, f, indent=2, ensure_ascii=False)
    
    print(f"\nSaved {len(posts)} posts to {output_file}")


def save_as_markdown(posts: List[Dict], output_file: str, subreddit: str):
    """Save posts as Markdown file"""
    output_path = Path(output_file)
    
    with output_path.open('w', encoding='utf-8') as f:
        # Write header
        f.write(f"# Reddit Stories Collection - r/{subreddit}\n\n")
        f.write(f"**Collected:** {datetime.now().strftime('%Y-%m-%d %H:%M:%S')}\n")
        f.write(f"**Total Posts:** {len(posts)}\n\n")
        f.write("---\n\n")
        
        # Group by category
        categorized = {}
        for post in posts:
            category = post.get('category', 'general')
            if category not in categorized:
                categorized[category] = []
            categorized[category].append(post)
        
        # Write each category
        for category, category_posts in sorted(categorized.items()):
            f.write(f"## {category.replace('_', ' ').title()}\n\n")
            
            for post in category_posts:
                f.write(f"### {post['title']}\n\n")
                f.write(f"**Author:** u/{post['author']} | ")
                f.write(f"**Score:** {post['score']} | ")
                f.write(f"**Comments:** {post['num_comments']} | ")
                f.write(f"**Date:** {post['created_date'][:10]}\n\n")
                f.write(f"**URL:** {post['url']}\n\n")
                
                if post['selftext']:
                    # Truncate long posts
                    text = post['selftext']
                    if len(text) > 500:
                        text = text[:500] + "..."
                    f.write(f"{text}\n\n")
                
                if 'comments' in post and post['comments']:
                    f.write(f"**Top Comments:**\n\n")
                    for comment in post['comments'][:3]:
                        body = comment['body']
                        if len(body) > 200:
                            body = body[:200] + "..."
                        f.write(f"- [{comment['score']}] {body}\n")
                    f.write("\n")
                
                f.write("---\n\n")
    
    print(f"\nSaved {len(posts)} posts to {output_file}")


def main():
    """Main execution function"""
    parser = argparse.ArgumentParser(
        description='Reddit Story Scraper for BlueMarble Research',
        formatter_class=argparse.RawDescriptionHelpFormatter,
        epilog="""
Examples:
  # Fetch top 100 posts from r/MMORPG
  python reddit-story-scraper.py
  
  # Fetch posts with specific keywords
  python reddit-story-scraper.py --keywords "economy,trading,crafting"
  
  # Fetch from multiple subreddits (run separately)
  python reddit-story-scraper.py --subreddit gamedesign --limit 50
  
  # Get recent hot posts with comments
  python reddit-story-scraper.py --sort hot --timeframe day --include-comments
        """
    )
    
    parser.add_argument('--subreddit', default='MMORPG',
                       help='Target subreddit (default: MMORPG)')
    parser.add_argument('--limit', type=int, default=100,
                       help='Number of posts to fetch (default: 100)')
    parser.add_argument('--timeframe', default='month',
                       choices=['hour', 'day', 'week', 'month', 'year', 'all'],
                       help='Timeframe for top posts (default: month)')
    parser.add_argument('--sort', default='top',
                       choices=['hot', 'new', 'top', 'rising'],
                       help='Sort method (default: top)')
    parser.add_argument('--keywords', type=str,
                       help='Comma-separated keywords to filter posts')
    parser.add_argument('--min-score', type=int, default=10,
                       help='Minimum score for posts (default: 10)')
    parser.add_argument('--output', type=str,
                       help='Output file (default: auto-generated)')
    parser.add_argument('--format', default='json',
                       choices=['json', 'markdown'],
                       help='Output format (default: json)')
    parser.add_argument('--include-comments', action='store_true',
                       help='Include top comments in output')
    parser.add_argument('--test-mode', action='store_true',
                       help='Use sample data for testing (no network required)')
    
    args = parser.parse_args()
    
    # Initialize scraper
    scraper = RedditStoryScraper(test_mode=args.test_mode)
    
    # Fetch posts
    posts = scraper.fetch_posts(
        subreddit=args.subreddit,
        sort=args.sort,
        timeframe=args.timeframe,
        limit=args.limit
    )
    
    if not posts:
        print("No posts fetched. Exiting.")
        return 1
    
    # Filter posts
    keywords = args.keywords.split(',') if args.keywords else None
    filtered_posts = scraper.filter_posts(posts, keywords=keywords, min_score=args.min_score)
    
    print(f"Filtered to {len(filtered_posts)} posts (min_score={args.min_score})")
    
    if not filtered_posts:
        print("No posts match the filter criteria. Exiting.")
        return 1
    
    # Categorize posts
    for post in filtered_posts:
        post['category'] = scraper.categorize_post(post)
    
    # Fetch comments if requested
    if args.include_comments:
        print("\nFetching top comments...")
        for i, post in enumerate(filtered_posts):
            comments = scraper.fetch_comments(post['id'], args.subreddit, limit=5)
            post['comments'] = comments
            if (i + 1) % 10 == 0:
                print(f"  Processed {i + 1}/{len(filtered_posts)} posts...")
            time.sleep(1)  # Be nice to Reddit's API
    
    # Generate output filename if not provided
    if not args.output:
        timestamp = datetime.now().strftime('%Y%m%d_%H%M%S')
        ext = 'md' if args.format == 'markdown' else 'json'
        args.output = f"reddit-stories-{args.subreddit}-{timestamp}.{ext}"
    
    # Save output
    if args.format == 'json':
        save_as_json(filtered_posts, args.output)
    else:
        save_as_markdown(filtered_posts, args.output, args.subreddit)
    
    # Print summary
    print("\n" + "=" * 60)
    print("Collection Summary:")
    print("=" * 60)
    print(f"Subreddit: r/{args.subreddit}")
    print(f"Posts collected: {len(filtered_posts)}")
    print(f"Output file: {args.output}")
    
    # Category breakdown
    categories = {}
    for post in filtered_posts:
        cat = post.get('category', 'general')
        categories[cat] = categories.get(cat, 0) + 1
    
    print("\nCategory Breakdown:")
    for category, count in sorted(categories.items(), key=lambda x: x[1], reverse=True):
        print(f"  {category.replace('_', ' ').title()}: {count}")
    
    return 0


if __name__ == '__main__':
    sys.exit(main())
