# Content Design Workflow

**Document Type:** Research Report - Process  
**Version:** 2.0  
**Author:** BlueMarble Design Team  
**Date:** 2025-01-20  
**Status:** Active  
**Related Files:** [Content Design Index](README.md)

## Executive Summary

Content design operates through a structured 5-phase workflow that transforms game systems and narrative goals into player-facing experiences. This document details each phase, presents proven content design patterns, and explains the tools and techniques content designers use to create engaging player experiences.

---

## How Content Design Works

Content design operates through a structured workflow that transforms game systems and narrative goals into player-facing experiences. Understanding this workflow is essential for effective content creation.

### The Content Design Workflow

#### Phase 1: Requirements Gathering

**Input Sources:**
- Game design documents (systems and mechanics)
- Narrative design briefs (story beats and character arcs)
- Player psychology research (target audience preferences)
- Technical constraints (engine capabilities, UI limitations)

**Activities:**
- Identify teaching moments for new mechanics
- Map story beats to gameplay moments
- Define content goals and success criteria
- Establish content tone and voice guidelines

**Output:**
- Content brief document
- Scope definition
- Success metrics

#### Phase 2: Design and Prototyping

**Brainstorming:**
- Generate multiple quest concepts
- Explore different narrative approaches
- Consider player choice impact
- Map reward structures

**Paper Design:**
- Create quest flowcharts
- Write dialogue trees on paper/whiteboard
- Sketch player journey maps
- Define trigger conditions and states

**Rapid Prototyping:**
- Build simple greybox versions in editor
- Test core loop without polish
- Validate pacing and difficulty
- Identify technical blockers early

**Output:**
- Quest design documents
- Dialogue scripts (draft)
- Flow diagrams
- Prototype builds

#### Phase 3: Implementation

**Content Creation:**
- Write final dialogue with character voice
- Implement quests in game editor
- Create UI text and tooltips
- Write item descriptions and lore

**Technical Implementation:**
- Set up quest triggers and conditions
- Implement branching logic
- Connect to game systems (inventory, skills, etc.)
- Add localization hooks

**Collaboration:**
- Work with level designers on quest spaces
- Coordinate with artists on visual storytelling
- Sync with audio for VO and music cues
- Review with narrative team for consistency

**Output:**
- Implemented content in game build
- Content database entries
- Localization strings
- Implementation notes

#### Phase 4: Testing and Iteration

**Internal Playtesting:**
- Test quest completion paths
- Verify dialogue triggers correctly
- Check reward balance
- Validate tutorial effectiveness

**External Playtesting:**
- Observe new players attempting content
- Gather feedback through surveys
- Track completion rates and drop-off points
- Identify confusion or frustration

**Data Analysis:**
- Review telemetry data (completion rates, time spent, etc.)
- Identify content that's too difficult or too easy
- Find dialogue that players skip
- Locate unclear objectives

**Iteration:**
- Revise dialogue based on feedback
- Adjust difficulty and rewards
- Clarify objectives and guidance
- Polish presentation

**Output:**
- Polished, tested content
- Balance documentation
- Known issues and edge cases
- Player feedback summaries

#### Phase 5: Polish and Launch

**Final Polish:**
- Proofread all text for typos
- Verify localization quality
- Test all edge cases
- Final balance pass

**Documentation:**
- Update design wiki
- Create player-facing guides (if needed)
- Document known limitations
- Prepare patch notes

**Launch Support:**
- Monitor player feedback on forums/social media
- Track telemetry for unexpected behavior
- Prepare hotfixes for critical issues
- Gather data for post-launch iteration

### Content Design Patterns

Content designers rely on proven patterns that work across different game types:

#### The "Training Wheels" Pattern
**Purpose:** Teach mechanics safely before real stakes

**Implementation:**
- Quest requires using new mechanic in controlled environment
- Failure has no permanent consequences
- Success provides clear positive feedback
- Gradually remove safety nets in subsequent content

**Example (BlueMarble Context):**
```
Quest: "The Practice Forge"
- Objective: Craft 3 iron nails using tutorial forge
- Mechanics Taught: Material selection, quality preview, crafting process
- Safety: Tutorial forge has 100% success rate and free materials
- Transition: Next quest uses real forge with actual resource costs
```

#### The "Breadcrumb Trail" Pattern
**Purpose:** Guide exploration and discovery

**Implementation:**
- Initial hook quest in central location
- Follow-up quests lead to new areas
- Each step reveals new information or mechanics
- Final payoff rewards thorough exploration

**Example:**
```
Quest Chain: "The Lost Mine"
1. "Rumors at the Tavern" - NPC mentions abandoned mine
2. "Following the Map" - Leads player to forest edge
3. "The Hidden Entrance" - Teaches environmental puzzle solving
4. "Mine Depths" - Introduces mining mechanics
5. "The Ore Vein" - Rewards with valuable resources
```

#### The "Moral Choice" Pattern
**Purpose:** Create meaningful player agency

**Implementation:**
- Present situation with competing values
- Neither choice is purely good/bad
- Consequences visible in world state
- NPCs react to player's choice

**Example:**
```
Quest: "The Hungry Village"
Choice: Give food to starving villagers OR sell to merchant for profit
Consequence A: Villagers survive, unlock trader, reputation +20
Consequence B: Extra coin now, village abandoned later, reputation -10
Long-term: Choice affects available quests in that region
```

#### The "Chain Reaction" Pattern
**Purpose:** Show player impact on world systems

**Implementation:**
- Initial action seems small/local
- Consequences ripple through connected systems
- Player discovers impact through environmental storytelling
- Creates sense of living world

**Example (BlueMarble Context):**
```
Quest: "Clear the Forest"
Action: Player clears trees for farmland
Consequence Chain:
- Erosion increases in region (geological system)
- Nearby stream becomes muddy (water quality system)
- Fish population declines (ecosystem)
- Fishing village NPCs comment on poor catches (social system)
- New quest unlocked: "Restoring the Stream"
```

#### The "Mastery Showcase" Pattern
**Purpose:** Let players demonstrate learned skills

**Implementation:**
- Later content requires synthesis of multiple mechanics
- No explicit tutorial or guidance
- Success proves player competency
- Significant reward acknowledges achievement

**Example:**
```
Quest: "The Master Smith's Challenge"
Requirements:
- Mine high-quality ore (mining mastery)
- Identify optimal material properties (geology knowledge)
- Craft superior weapon (smithing expertise)
- Complete within time limit (efficiency)
No hand-holding - player must apply all learned skills
```

### Content Design Tools and Techniques

#### Dialogue Trees

**Structure Types:**

**Hub-and-Spoke:**
```
         [Topic 1]
              ↓
    ┌─────────────────┐
    │   Central Hub   │
    └─────────────────┘
         ↓         ↓
   [Topic 2]   [Topic 3]
```
- Player returns to main menu after each topic
- Good for information gathering
- Low branching complexity

**Linear with Branches:**
```
Start → Choice A → Result A
    ↓
    Choice B → Result B
```
- Forward momentum with occasional choices
- Good for narrative-driven content
- Medium complexity

**State-Based:**
```
Initial State → Action → New State → Different Dialogue
```
- Dialogue changes based on world state
- Good for reactive NPCs
- High complexity, high immersion

#### Quest Gating Mechanisms

**Soft Gates** (Recommended but not required):
- Level recommendation
- Difficulty warning
- NPC advice against attempting

**Hard Gates** (Cannot proceed without):
- Quest completion requirement
- Item possession check
- Skill level threshold
- Reputation threshold

**Optimal Balance for BlueMarble:**
- Use soft gates for difficulty management
- Use hard gates sparingly for story-critical moments
- Allow player creativity to bypass some gates
- Geological knowledge as natural gate (can't mine deep without structural knowledge)

#### Reward Structures

**Tangible Rewards:**
- Items (tools, materials, equipment)
- Currency (gold, reputation points)
- Experience points / skill increases
- Unlocked recipes or blueprints

**Intangible Rewards:**
- Story progression and reveals
- Character development and relationships
- World state changes (reputation, access)
- Personal satisfaction (mastery, discovery)

**Optimal Mix:**
- 60% tangible (player needs progression)
- 40% intangible (player wants meaning)
- Align rewards with player motivation
- Scale rewards with effort and risk

### Content Design Challenges

#### Challenge 1: Teaching Without Telling

**Problem:** Players skip tutorial text, need to learn through doing

**Solutions:**
- Contextual, just-in-time tutorials (only when needed)
- Design quest objectives that require using mechanic
- Use visual/audio feedback instead of text
- Make failure informative rather than punishing
- Progressive complexity (learn one thing at a time)

#### Challenge 2: Branching Content Scalability

**Problem:** Branching choices multiply content creation cost

**Solutions:**
- "Branching river" structure (branches rejoin)
- Cosmetic variation (same outcome, different dialogue)
- Delayed consequences (impact later content)
- Procedural variation (same structure, different details)
- Focus branching on key moments only

#### Challenge 3: Content Pacing

**Problem:** Different players consume content at different rates

**Solutions:**
- Optional side content for completionists
- Multiple difficulty settings
- Adaptive difficulty based on performance
- Clear critical path with optional branches
- Content that scales with player level

#### Challenge 4: Replayability

**Problem:** Content feels stale on second playthrough

**Solutions:**
- Hidden content discovered on replay
- Multiple solution paths to same goal
- Divergent choice consequences
- Procedural variation in details
- New Game+ exclusive content

#### Challenge 5: Localization-Friendly Content

**Problem:** Text written for one language doesn't work in others

**Solutions:**
- Avoid idioms and cultural references
- Use simple, clear sentence structures
- Minimize text-in-images
- Design for text expansion (some languages 30% longer)
- Use gender-neutral language where possible
- Include context notes for translators

---


## Next Steps

- **Study Examples**: [Video Game RPGs](content-design-video-game-rpgs.md) | [Tabletop RPGs](content-design-tabletop-rpgs.md)
- **Understand the Role**: [Content Designer Role](content-designer-role.md)
- **Learn Skills**: [Essential Skills](content-designer-skills.md)

## Related Research

- [Content Design Index](README.md) - Full topic navigation
- [Content Design Overview](content-design-overview.md) - What is content design
- [BlueMarble Applications](content-design-bluemarble.md) - Implementation recommendations

---

*This document details the practical workflow and patterns content designers use. See the [Content Design Index](README.md) for complete research coverage.*
