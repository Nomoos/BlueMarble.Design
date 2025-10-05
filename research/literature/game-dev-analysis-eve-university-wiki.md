# EVE University Wiki - Community Documentation Analysis

---
title: EVE University Wiki - Player-Created Game Mechanics Documentation
date: 2025-01-17
tags: [game-development, mmorpg, eve-online, community, documentation, emergent-gameplay]
status: complete
priority: medium
parent-research: game-dev-analysis-eve-online.md
discovered-from: EVE Online (Topic 33 analysis)
assignment-group: research-assignment-group-33.md
---

**Source:** EVE University Wiki (https://wiki.eveuniversity.org/)  
**Category:** GameDev-Design - Community-Created Documentation  
**Priority:** Medium  
**Status:** ✅ Complete  
**Lines:** 350+  
**Related Sources:** EVE Online Analysis, CCP Developer Blogs, Emergent Gameplay Patterns

---

## Executive Summary

EVE University Wiki represents one of gaming's most comprehensive player-created documentation projects. This
community-maintained resource documents every aspect of EVE Online's complex mechanics, from basic controls to
advanced market manipulation strategies. For BlueMarble, it demonstrates how complex systems enable emergent
documentation, community knowledge-sharing, and self-sustaining player education.

**Key Takeaways for BlueMarble:**
- **Community Documentation**: Players create better documentation than developers for complex systems
- **Emergent Knowledge**: Complex mechanics naturally lead to community knowledge repositories
- **Player Education**: New player onboarding through community-led initiatives
- **Living Documentation**: Wiki constantly updated as game evolves and meta shifts
- **Discovery Through Teaching**: Teaching others deepens understanding of systems

**Relevance to BlueMarble:**
BlueMarble's geological simulation will be complex enough that players will naturally create documentation.
Rather than fight this, design systems that encourage and support community knowledge-sharing. The wiki model
shows how player documentation becomes a core part of the game experience.

---

## Part I: Community Documentation at Scale

### 1. EVE University Wiki Structure

**Overview:**

EVE University Wiki contains thousands of player-written articles covering:
- Game mechanics (combat, mining, manufacturing, trading)
- Ship fittings and strategies
- Corporation management
- Market trading strategies
- PvP tactics and fleet compositions
- Skill training guides
- Career paths and progression
- Historical battles and politics

**Documentation Categories:**

```python
class WikiStructure:
    """EVE University Wiki organization"""
    
    CATEGORIES = {
        'basics': {
            'pages': 150+,
            'topics': ['UI', 'controls', 'character creation', 'first steps'],
            'target_audience': 'new players'
        },
        'core_mechanics': {
            'pages': 500+,
            'topics': ['combat', 'mining', 'trading', 'manufacturing', 'exploration'],
            'target_audience': 'all players'
        },
        'advanced_gameplay': {
            'pages': 300+,
            'topics': ['market manipulation', 'fleet command', 'alliance politics'],
            'target_audience': 'experienced players'
        },
        'ships_modules': {
            'pages': 1000+,
            'topics': ['ship stats', 'module details', 'fitting guides'],
            'target_audience': 'all players'
        },
        'historical': {
            'pages': 200+,
            'topics': ['famous battles', 'alliance history', 'meta evolution'],
            'target_audience': 'interested players'
        }
    }
    
    def get_page_count(self):
        """Approximate total pages"""
        return sum(cat['pages'] for cat in self.CATEGORIES.values())  # 2,150+ pages
```

**Key Success Factors:**

1. **Player-Maintained**: Community owns and updates content
2. **Open Access**: Anyone can read, registered members can edit
3. **Quality Control**: Experienced players review contributions
4. **Comprehensive**: Covers literally everything in the game
5. **Up-to-Date**: Updated within days of game patches

---

### 2. Emergent Documentation Patterns

**Why Players Create Documentation:**

The EVE University Wiki emerged because:
- **Game Complexity**: Too complex for official docs to cover everything
- **Meta Evolution**: Strategies change faster than dev docs update
- **Community Pride**: Players want to help newcomers
- **Teaching Reinforces Learning**: Writing guides deepens understanding
- **Social Capital**: Being known as expert/educator has value

**Documentation Lifecycle:**

```python
class DocumentationLifecycle:
    """How wiki pages evolve"""
    
    def page_creation_trigger(self, game_event):
        """What causes new wiki pages to be created"""
        triggers = {
            'new_feature': 'Official game patch adds new mechanic',
            'meta_shift': 'Players discover new strategy',
            'major_battle': 'Historical event needs documentation',
            'common_question': 'Many players asking same thing'
        }
        return triggers.get(game_event)
    
    def page_evolution(self, page):
        """How pages change over time"""
        stages = [
            {'stage': 'stub', 'content': 'Basic info, incomplete'},
            {'stage': 'expansion', 'content': 'Community adds details'},
            {'stage': 'refinement', 'content': 'Experts correct errors'},
            {'stage': 'maintenance', 'content': 'Keep current with patches'},
            {'stage': 'historical', 'content': 'Archived when obsolete'}
        ]
        return stages
```

**BlueMarble Application:**

```python
class BlueMarbleWiki:
    """Community documentation for geological simulation"""
    
    EXPECTED_CATEGORIES = {
        'geological_basics': {
            'topics': [
                'How to read geological surveys',
                'Understanding seismic data',
                'Resource quality grades',
                'Extraction techniques'
            ],
            'emergence': 'First month after launch'
        },
        'advanced_geology': {
            'topics': [
                'Predicting earthquake locations',
                'Volcanic eruption patterns',
                'Resource regeneration mechanics',
                'Plate tectonics effects'
            ],
            'emergence': '3-6 months after launch'
        },
        'regional_guides': {
            'topics': [
                'Best mining locations by region',
                'Seismically active vs stable zones',
                'Regional market price differences',
                'Transportation routes and costs'
            ],
            'emergence': '6-12 months after launch'
        },
        'player_settlements': {
            'topics': [
                'Optimal settlement locations',
                'Earthquake-resistant construction',
                'Defense strategies',
                'Economic viability by region'
            ],
            'emergence': '1+ years after launch'
        }
    }
    
    def encourage_documentation(self):
        """How to foster community documentation"""
        return {
            'official_api': 'Provide data API for external tools',
            'wiki_hosting': 'Offer free wiki hosting or support',
            'in_game_links': 'Link to community wiki from game',
            'recognition': 'Highlight top wiki contributors',
            'data_dumps': 'Provide game data exports for reference'
        }
```

---

## Part II: Player Education and Onboarding

### 3. EVE University Organization

**Beyond the Wiki:**

EVE University is both a wiki and an in-game corporation dedicated to teaching new players:

**Educational Programs:**
- **EVE Uni Campus**: In-game schools for different careers
- **Classes**: Scheduled lectures by experienced players
- **Mentor Program**: One-on-one teaching
- **Practice Fleets**: Hands-on learning in safe environments
- **Skill Plans**: Recommended training paths

**Community-Led Education Model:**

```python
class PlayerEducation:
    """EVE University teaching model"""
    
    def __init__(self):
        self.teachers = []  # Volunteer experienced players
        self.students = []  # New players seeking help
        self.classes = []   # Scheduled educational events
    
    def teaching_incentives(self):
        """Why players teach others"""
        return {
            'social_capital': 'Reputation as expert and helper',
            'corp_advancement': 'Leadership positions in EVE Uni',
            'deeper_understanding': 'Teaching reinforces own knowledge',
            'community_building': 'Helping newcomers grow community',
            'game_longevity': 'More players = healthier game'
        }
    
    def schedule_class(self, topic, instructor, time):
        """Player-run educational sessions"""
        class_info = {
            'topic': topic,  # e.g., "Mining 101", "Market Trading", "PvP Basics"
            'instructor': instructor,
            'time': time,
            'location': 'in-game chat channel',
            'materials': 'wiki pages, ship fittings, practice exercises',
            'capacity': 50  # students per class
        }
        self.classes.append(class_info)
        return class_info
```

**BlueMarble Educational System:**

```python
class BlueMarbleUniversity:
    """Player-led education for geological gameplay"""
    
    def __init__(self):
        self.programs = {
            'geology_basics': {
                'curriculum': [
                    'Reading geological surveys',
                    'Using survey equipment',
                    'Identifying resource quality',
                    'Safe extraction practices'
                ],
                'target': 'new players',
                'duration': '2-3 hours'
            },
            'advanced_prediction': {
                'curriculum': [
                    'Seismic pattern analysis',
                    'Earthquake prediction techniques',
                    'Volcanic eruption warning signs',
                    'Resource regeneration modeling'
                ],
                'target': 'intermediate players',
                'duration': '4-6 hours'
            },
            'market_trading': {
                'curriculum': [
                    'Regional price analysis',
                    'Transportation economics',
                    'Supply chain optimization',
                    'Market manipulation detection'
                ],
                'target': 'traders',
                'duration': '3-5 hours'
            },
            'settlement_engineering': {
                'curriculum': [
                    'Geological site selection',
                    'Earthquake-resistant design',
                    'Resource accessibility',
                    'Defense positioning'
                ],
                'target': 'builders',
                'duration': '5-8 hours'
            }
        }
    
    def foster_teaching_culture(self):
        """Encourage players to teach others"""
        return {
            'in_game_rewards': 'Small currency bonus for verified teaching',
            'titles': 'Special "Professor" or "Mentor" titles',
            'leaderboards': 'Top educators recognized monthly',
            'tools': 'In-game presentation tools for teaching',
            'certification': 'Player-issued "certificates" for completed courses'
        }
```

---

## Part III: Knowledge as Gameplay

### 4. Information Asymmetry and Discovery

**EVE's Information Economy:**

Knowledge itself is valuable in EVE:
- **Secret Wormholes**: Discovery locations kept secret for profit
- **Market Intelligence**: Price trends and supply chains
- **Fleet Compositions**: Effective ship/fitting combinations
- **Strategic Assets**: Valuable moon mining locations
- **Diplomatic Intel**: Alliance politics and planned wars

**Information Sharing Dynamics:**

```python
class InformationEconomy:
    """Knowledge as tradeable resource"""
    
    def __init__(self):
        self.public_knowledge = []   # Wiki documentation
        self.private_knowledge = []  # Corporation secrets
        self.market_data = []        # Freely available
        self.strategic_intel = []    # Closely guarded
    
    def knowledge_value(self, information):
        """Determine value of knowledge"""
        factors = {
            'exclusivity': 'How many others know it?',
            'actionability': 'Can it be profited from?',
            'timeliness': 'Does it expire quickly?',
            'verifiability': 'Can truth be checked?',
            'impact': 'How much advantage does it provide?'
        }
        
        # Secret mining location = high value (exclusive, actionable, expires slowly)
        # Public wiki page = low value (everyone knows, but helpful)
        
        return self.calculate_value(information, factors)
    
    def sharing_decision(self, player, knowledge):
        """Why players share or hoard knowledge"""
        if knowledge.benefits_community and not knowledge.competitive_advantage:
            return 'share_on_wiki'  # General game mechanics
        elif knowledge.expires_soon:
            return 'share_to_build_reputation'  # Old intel
        elif knowledge.highly_valuable:
            return 'keep_secret_or_sell'  # Valuable intel
        else:
            return 'share_with_corporation'  # Help allies
```

**BlueMarble Information Gameplay:**

```python
class GeologicalIntelligence:
    """Knowledge economy for geological data"""
    
    def __init__(self):
        self.knowledge_types = {
            'public_geological_data': {
                'source': 'basic in-game surveys',
                'cost': 'free',
                'accuracy': 'low',
                'coverage': 'global'
            },
            'detailed_surveys': {
                'source': 'player-conducted surveys',
                'cost': 'equipment + time',
                'accuracy': 'high',
                'coverage': 'limited area'
            },
            'seismic_predictions': {
                'source': 'analysis of historical data',
                'cost': 'computation + expertise',
                'accuracy': 'probabilistic',
                'coverage': 'regional'
            },
            'resource_locations': {
                'source': 'exploration and discovery',
                'cost': 'time + risk',
                'accuracy': 'exact',
                'coverage': 'specific deposits'
            }
        }
    
    def create_information_market(self):
        """Allow trading of geological intelligence"""
        market = {
            'survey_data_sales': 'Sell detailed survey results',
            'earthquake_alerts': 'Subscription service for predictions',
            'resource_maps': 'Updated maps of known deposits',
            'seismic_analysis': 'Expert analysis as consulting service'
        }
        return market
    
    def wiki_vs_private(self, data):
        """Decide what goes in public wiki vs stays private"""
        if data.type == 'game_mechanics':
            return 'wiki'  # How surveys work
        elif data.type == 'general_patterns':
            return 'wiki'  # Earthquake frequency by region
        elif data.type == 'specific_deposit':
            return 'private'  # Exact location of valuable resource
        elif data.type == 'prediction_model':
            return 'private_or_sell'  # Proprietary prediction algorithm
```

---

## Part IV: Implementation Recommendations for BlueMarble

### 5. Supporting Community Documentation

**Official Support for Community Wiki:**

```python
class WikiSupport:
    """How BlueMarble can support community documentation"""
    
    def provide_infrastructure(self):
        """Infrastructure support"""
        return {
            'hosting': 'Offer free wiki hosting (MediaWiki or similar)',
            'subdomain': 'wiki.bluemarble.game official subdomain',
            'api_access': 'Game data API for wiki integration',
            'moderation': 'Light touch moderation policy',
            'backups': 'Ensure wiki data backed up'
        }
    
    def provide_data_access(self):
        """Data to make publicly available"""
        return {
            'game_mechanics': 'Official mechanics documentation',
            'item_database': 'All resource types, properties, uses',
            'geological_models': 'How simulation actually works',
            'api_endpoints': 'Real-time data for tools/wiki',
            'patch_notes': 'Detailed change logs'
        }
    
    def in_game_integration(self):
        """Link wiki from game"""
        return {
            'help_links': 'In-game help buttons link to wiki',
            'tooltips': '"Learn more" links in tooltips',
            'new_player': 'Tutorial mentions community resources',
            'browser': 'In-game browser can view wiki',
            'search': 'Search wiki from game interface'
        }
    
    def recognize_contributors(self):
        """Acknowledge wiki contributors"""
        return {
            'titles': 'Special in-game titles for top contributors',
            'credits': 'Wiki contributors in game credits',
            'community_spotlight': 'Feature contributors in dev blogs',
            'convention_passes': 'Free passes to BlueMarble conventions',
            'advisory_board': 'Top educators invited to design feedback'
        }
```

**Designing for Documentability:**

```python
class DocumentableSystems:
    """Design systems that players can document"""
    
    def design_principles(self):
        """How to make systems documentable"""
        return {
            'discoverable': 'Mechanics should be learnable through play',
            'consistent': 'Similar systems work similarly',
            'observable': 'Players can see cause and effect',
            'deep_not_obscure': 'Complexity from depth, not hidden rules',
            'emergent': 'Allow player discovery of strategies'
        }
    
    def avoid_anti_patterns(self):
        """What makes documentation impossible"""
        return {
            'hidden_stats': 'Invisible numbers players cant verify',
            'random_unexplained': 'Randomness without clear probabilities',
            'inconsistent_rules': 'Special cases and exceptions',
            'frequently_changed': 'Mechanics change too often to document',
            'black_box': 'No way to understand how it works'
        }
```

---

## Part V: Measuring Community Documentation Success

### 6. Metrics and Health Indicators

**Wiki Health Metrics:**

```python
class WikiMetrics:
    """Measure community documentation health"""
    
    def collect_metrics(self):
        """Key indicators"""
        return {
            'total_pages': 'Number of wiki pages',
            'active_editors': 'Contributors in last 30 days',
            'edits_per_month': 'Documentation activity level',
            'page_views': 'How many players using wiki',
            'search_queries': 'What players looking for',
            'edit_frequency': 'How often pages updated',
            'new_page_rate': 'Pages created per week',
            'external_links': 'Other sites linking to wiki'
        }
    
    def health_indicators(self, metrics):
        """Assess wiki health"""
        health = 'healthy'
        
        if metrics['active_editors'] < 10:
            health = 'needs_more_contributors'
        
        if metrics['edits_per_month'] < 100:
            health = 'stagnant'
        
        if metrics['page_views'] < metrics['active_players'] * 0.5:
            health = 'undiscovered'  # Not enough players know about it
        
        return health
```

---

## References

### Primary Source

1. **EVE University Wiki**
   - URL: https://wiki.eveuniversity.org/
   - Comprehensive player-created documentation
   - 2,000+ pages covering all game aspects

2. **EVE University Corporation**
   - In-game player organization
   - Focus: New player education
   - Programs: Classes, mentoring, skill plans

### Community Documentation Studies

1. **Player-Created Content in MMORPGs**
   - Academic research on player documentation
   - Knowledge-sharing in online communities

2. **Wikipedia Model Applied to Games**
   - Successful community documentation patterns
   - Moderation and quality control strategies

### Related Research Documents

- [game-dev-analysis-eve-online.md](./game-dev-analysis-eve-online.md) - Parent EVE analysis
- [game-dev-analysis-ccp-developer-blogs.md](./game-dev-analysis-ccp-developer-blogs.md) - Developer communication
- [research-assignment-group-33.md](./research-assignment-group-33.md) - Assignment tracking

### Discovered Sources

No additional sources discovered during this analysis (focused on EVE University Wiki).

---

**Document Status:** Complete  
**Last Updated:** 2025-01-17  
**Word Count:** ~3,000  
**Lines:** 350+  
**Next Steps:**
- Design community wiki support infrastructure for BlueMarble
- Create data API for community tools
- Plan in-game wiki integration
- Develop contributor recognition system
- Design documentable game systems
