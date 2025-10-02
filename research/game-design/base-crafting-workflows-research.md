# Base Crafting Workflows and Tool Integration Research

**Document Type:** Research Report  
**Version:** 1.0  
**Author:** BlueMarble Game Design Research Team  
**Date:** 2025-01-15  
**Status:** Final  
**Research Type:** Industry Trends  
**Priority:** High

## Executive Summary

This research analyzes base crafting workflows and tool integration patterns in MMORPGs to inform
improvements to player accessibility and crafting user experience in BlueMarble. The focus is on
making basic item creation straightforward and rewarding while establishing a foundation for more
advanced crafting systems.

**Key Findings:**

- **Streamlined Workflows**: Successful basic crafting systems minimize clicks and present clear
  paths from raw materials to finished items
- **Tool Selection Impact**: Tools affect crafting speed, quality output, and success rates without
  being overly complex for beginners
- **Workstation Proximity**: Physical proximity to appropriate workstations creates spatial gameplay
  while accessibility features prevent frustration
- **Progressive Disclosure**: UI should reveal complexity gradually - basic crafters see simple
  options, experienced crafters see advanced controls
- **Visual Feedback**: Clear indicators for material availability, tool status, and crafting
  progress improve player confidence and satisfaction
- **Resource Management**: Interface should help players understand what they can craft with
  available materials without requiring external calculators

**Applicability to BlueMarble:**

BlueMarble's geological simulation and realistic material properties provide an ideal foundation
for meaningful tool and workstation mechanics. Basic crafting can introduce players to these
systems in an accessible way while preparing them for advanced features.

## Table of Contents

1. [Research Objectives](#research-objectives)
2. [Methodology](#methodology)
3. [Base Crafting Workflow Analysis](#base-crafting-workflow-analysis)
4. [Tool Selection Mechanics](#tool-selection-mechanics)
5. [Workstation Integration](#workstation-integration)
6. [UI/UX Design Patterns for Basic Crafting](#uiux-design-patterns-for-basic-crafting)
7. [Comparative Game Analysis](#comparative-game-analysis)
8. [Workflow Diagrams](#workflow-diagrams)
9. [Recommendations for BlueMarble](#recommendations-for-bluemarble)
10. [Implementation Considerations](#implementation-considerations)
11. [Conclusion](#conclusion)
12. [Appendices](#appendices)

## Research Objectives

### Primary Research Questions

1. **How do base crafting workflows enable resource processing and item creation?**
   - What are the essential steps in basic crafting systems?
   - How do games minimize friction in the crafting process?
   - What workflows balance simplicity with meaningful player choice?

2. **What role does tool selection and workstation proximity play in crafting operations?**
   - How do tools affect crafting outcomes?
   - What are the trade-offs between tool quality and accessibility?
   - How does physical location affect crafting availability?

3. **How does the user interface support efficient crafting and resource selection?**
   - What UI patterns reduce cognitive load for basic crafters?
   - How do interfaces communicate material requirements clearly?
   - What feedback mechanisms improve crafting satisfaction?

### Secondary Research Questions

1. How do basic crafting systems introduce players to more complex mechanics?
2. What accessibility features make crafting approachable for new players?
3. How do games balance realism with gameplay convenience in tool/workstation mechanics?
4. What progression paths exist from basic to advanced crafting?

### Success Criteria

This research succeeds if it provides:

- Clear workflow models for basic crafting operations
- Actionable UI/UX recommendations for crafting interfaces
- Tool and workstation integration patterns suitable for BlueMarble
- Annotated examples from successful games
- Implementation guidance for Q1 2026 development

## Methodology

### Research Approach

**Mixed Methods Analysis** combining qualitative gameplay analysis with quantitative feature
comparison across multiple MMORPGs.

### Data Collection Methods

- **Gameplay Analysis**: Direct examination of crafting workflows in reference games
- **UI/UX Review**: Study of crafting interface designs and interaction patterns
- **Documentation Analysis**: Review of existing BlueMarble crafting specifications
- **Community Research**: Analysis of player feedback on crafting systems
- **Workflow Mapping**: Creation of step-by-step process diagrams

### Data Sources

- **Reference Games**: World of Warcraft, Final Fantasy XIV, Wurm Online, Vintage Story,
  Eco Global Survival, Novus Inceptio, Life is Feudal
- **BlueMarble Documentation**:
  - [Crafting Mechanics Overview](../../docs/gameplay/mechanics/crafting-mechanics-overview.md)
  - [Crafting Interface Mockups](./assets/crafting-interface-mockups.md)
  - [Assembly Skills System Research](./assembly-skills-system-research.md)
  - [Advanced Crafting System Research](./advanced-crafting-system-research.md)
- **Community Resources**: Forums, wikis, tutorial videos, player guides

### Research Scope

**Included:**

- Basic recipe processing (simple 1-3 material recipes)
- Tool selection and quality effects
- Workstation requirements and proximity
- UI integration for basic crafting
- Material availability indication
- Crafting progress feedback

**Excluded:**

- Advanced multi-stage crafting
- Combat mechanics
- Farming/agriculture systems
- Guild/collaborative crafting
- Auction house integration

**Time Constraint:** Research completed for Q1 2026 implementation planning

## Base Crafting Workflow Analysis

### Essential Workflow Steps

Successful basic crafting systems follow a consistent pattern that players can internalize:

#### 1. Discovery Phase

```text
Player Discovers Recipe
        ↓
Recipe Requirements Displayed
        ↓
Player Checks Material/Tool Availability
```text

**Key Observations:**

- Recipes can be discovered through exploration, NPCs, or skill progression
- Clear requirement display prevents player frustration
- Visual indicators show what's available vs. needed

#### 2. Preparation Phase

```text
Player Gathers Required Materials
        ↓
Player Acquires/Equips Appropriate Tools
        ↓
Player Travels to Required Workstation
```text

**Key Observations:**

- Games vary in how strictly they enforce workstation requirements
- Tool accessibility affects crafting engagement
- Fast travel or recall features reduce preparation tedium

#### 3. Execution Phase

```text
Player Opens Crafting Interface
        ↓
Player Selects Recipe
        ↓
Player Confirms Material Selection
        ↓
Crafting Process Executes
        ↓
Result Displayed and Items Created
```text

**Key Observations:**

- Streamlined interfaces allow experienced players to craft quickly
- Progress bars with time estimates set expectations
- Success/failure feedback is immediate and clear

### Workflow Efficiency Patterns

#### Pattern 1: Instant Crafting (WoW-style)

```text
Open Interface → Select Recipe → Click Craft → Item Created
Time: 2-5 seconds per item
```text

**Advantages:**

- Very fast for bulk crafting
- Low friction for new players
- Focus on material gathering, not interface navigation

**Disadvantages:**

- Less engaging for single items
- Minimal skill expression
- Can feel like clicking menus

#### Pattern 2: Timed Crafting (FFXIV-style)

```text
Open Interface → Select Recipe → Confirm → Wait 10-30s → Item Created
```text

**Advantages:**

- Creates sense of accomplishment
- Natural pacing prevents spam crafting
- Time can scale with item complexity

**Disadvantages:**

- Can be tedious for bulk operations
- Requires AFK time or other activities
- May frustrate impatient players

#### Pattern 3: Interactive Crafting (Eco-style)

```text
Open Interface → Select Recipe → Place Materials → Monitor Progress → 
Adjust Parameters → Wait for Completion → Retrieve Item
```text

**Advantages:**

- Engaging and skill-based
- Feels more realistic
- Creates opportunities for quality variance

**Disadvantages:**

- Higher learning curve
- Can slow down production
- More complex UI requirements

### Recommended Hybrid Approach for BlueMarble

Based on BlueMarble's existing design philosophy emphasizing geological realism and skill-based
progression:

```text
╔═══════════════════════════════════════════════════════════════╗
║ BASIC CRAFTING WORKFLOW - BLUEMARBLE                          ║
╠═══════════════════════════════════════════════════════════════╣
║                                                                ║
║ Step 1: Open Crafting Interface (at workstation)              ║
║         ↓                                                      ║
║ Step 2: View Available Recipes (filtered by station/tools)    ║
║         ↓                                                      ║
║ Step 3: Select Recipe                                         ║
║         - Visual material check (green=have, red=need)        ║
║         - Tool compatibility indicator                        ║
║         - Expected quality/time estimate                      ║
║         ↓                                                      ║
║ Step 4: Confirm Crafting                                      ║
║         - Auto-selects materials (best quality by default)    ║
║         - Shows tool being used                               ║
║         - Displays success probability                        ║
║         ↓                                                      ║
║ Step 5: Crafting Executes (5-45 seconds)                     ║
║         - Progress bar with time remaining                    ║
║         - Quality indicators update in real-time              ║
║         - Can queue multiple items                            ║
║         ↓                                                      ║
║ Step 6: Result Displayed                                      ║
║         - Item quality shown                                  ║
║         - XP gained                                           ║
║         - Option to craft again                               ║
║                                                                ║
╚═══════════════════════════════════════════════════════════════╝
```text

**Time Investment:**

- Simple items (nails, planks): 5-10 seconds
- Standard items (tools, basic weapons): 15-30 seconds
- Complex items (armor pieces, advanced tools): 30-60 seconds

**Key Features:**

- Quick enough for basic gameplay
- Long enough to feel meaningful
- Scales with item complexity
- Allows queuing to reduce repetition

## Tool Selection Mechanics

### Tool Role in Basic Crafting

Tools serve multiple gameplay functions beyond pure realism:

#### Function 1: Quality Modifier

```text
Basic Tools (30-50% quality):
  - Readily available
  - Affordable for new players
  - Suitable for practice crafting
  - Produces functional but basic items
  
Standard Tools (50-75% quality):
  - Craftable with moderate skill
  - Reasonable durability
  - Good for regular crafting
  - Balance of cost and performance

Quality Tools (75-90% quality):
  - Requires skilled craftsperson
  - Higher material costs
  - Enhanced durability
  - Noticeable quality improvement

Master Tools (90-100% quality):
  - Rare or very expensive
  - Exceptional durability
  - Maximum quality output
  - Status symbol for crafters
```text

#### Function 2: Efficiency Modifier

```text
Tool Quality → Crafting Speed Relationship

Basic Tools:     100% base time (no bonus)
Standard Tools:  -10% crafting time
Quality Tools:   -20% crafting time  
Master Tools:    -30% crafting time

Example: Crafting Iron Sword
- Base time: 30 seconds
- With basic hammer: 30 seconds
- With quality hammer: 24 seconds
- With master hammer: 21 seconds
```text

#### Function 3: Recipe Enablement

```text
Some advanced recipes require minimum tool quality:

Recipe: Steel Longsword
├─ Requires: Blacksmithing 25+
├─ Requires: Quality Forge or better
└─ Requires: Standard Hammer or better

Recipe: Iron Nails
├─ Requires: Blacksmithing 1+
├─ Requires: Any Forge
└─ Requires: Any Hammer
```text

### Tool Degradation vs. Durability

**Model A: Consumable Tools (Life is Feudal style)**

- Tools degrade with each use
- Must be repaired or replaced
- Creates ongoing demand for tool crafting
- Can be frustrating for new players

**Model B: Permanent Tools (WoW style)**

- Tools never break
- Once acquired, used indefinitely
- Simpler for players
- Less economic depth

**Model C: Repairable Tools (RECOMMENDED for BlueMarble)**

```text
Tool Durability System:
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
Each tool has durability points:
  Basic Tools:    100 uses
  Standard Tools: 300 uses
  Quality Tools:  600 uses
  Master Tools:   1,000 uses

Degradation:
  - Each craft reduces durability by 1
  - Tools at 0 durability become "Damaged"
  - Damaged tools give penalties: -50% quality, +50% time
  - Can still be used but strongly incentivize repair

Repair:
  - Costs materials (fraction of crafting cost)
  - Can repair at workstation
  - Restores 100% durability
  - Self-repair with appropriate skill
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
```text

**Benefits of Repairable Model:**

- Creates material sink without being punishing
- Encourages maintenance gameplay
- Rewards planning (repairing before expeditions)
- Doesn't interrupt crafting sessions suddenly

### Tool Selection Interface

#### UI Pattern: Tool Indicator Display

```text
┌────────────────────────────────────────────────────┐
│ CRAFTING: Iron Sword                               │
│ ──────────────────────────────────────────────     │
│                                                     │
│ Required Tool: Hammer                              │
│                                                     │
│ Your Tools:                                        │
│ ◉ Standard Hammer        [Durability: 245/300]    │
│   Effect: Base quality, -10% crafting time         │
│                                                     │
│ ○ Basic Hammer           [Durability: 78/100]     │
│   Effect: -5% quality, no time bonus               │
│                                                     │
│ ○ Quality Hammer         [Durability: 580/600]    │
│   Effect: +10% quality, -20% crafting time         │
│   ⚠ This tool is expensive to repair               │
│                                                     │
│ Selected: Standard Hammer ✓                        │
│ [Change Selection]                                 │
└────────────────────────────────────────────────────┘
```text

**Design Principles:**

- Default selection uses "best available" for recipe
- Players can manually select lower-quality tools if desired
- Durability warnings prevent accidental tool breaking
- Clear indication of tool effects
- One-click to change if needed

## Workstation Integration

### Workstation Types and Requirements

#### Basic Workstation Categories

```text
╔════════════════════════════════════════════════════╗
║ BLACKSMITHING WORKSTATIONS                         ║
╠════════════════════════════════════════════════════╣
║                                                     ║
║ Campfire Forge (Portable)                          ║
║ ├─ Enables: Basic repairs, simple items           ║
║ ├─ Quality Modifier: -20%                         ║
║ ├─ Speed Modifier: +50% time                      ║
║ └─ Setup: Anywhere, requires fuel                 ║
║                                                     ║
║ Basic Forge (Small Structure)                      ║
║ ├─ Enables: All basic recipes                     ║
║ ├─ Quality Modifier: 0% (baseline)                ║
║ ├─ Speed Modifier: Base time                      ║
║ └─ Setup: Requires foundation, chimney            ║
║                                                     ║
║ Workshop Forge (Medium Structure)                  ║
║ ├─ Enables: Basic + intermediate recipes          ║
║ ├─ Quality Modifier: +10%                         ║
║ ├─ Speed Modifier: -15% time                      ║
║ └─ Setup: Dedicated building, anvil, tools        ║
║                                                     ║
║ Master Forge (Large Structure)                     ║
║ ├─ Enables: All recipes                           ║
║ ├─ Quality Modifier: +20%                         ║
║ ├─ Speed Modifier: -25% time                      ║
║ └─ Setup: Major investment, team construction     ║
║                                                     ║
╚════════════════════════════════════════════════════╝
```text

### Proximity Mechanics

#### Model 1: Strict Proximity (Wurm Online style)

- Must be standing at workstation
- No interaction beyond immediate range
- Creates crowded workstation areas
- Very realistic but can be limiting

#### Model 2: Building Proximity (Vintage Story style)

- Must be in same building/area as workstation
- Can move around while crafting
- More flexible but still location-dependent
- Good for social crafting

#### Model 3: Inventory Access (WoW style)

- Can craft anywhere with portable tools
- Workstations only for advanced recipes
- Very convenient but less immersive
- Reduces world interaction

#### RECOMMENDED: Hybrid Proximity System

```text
╔════════════════════════════════════════════════════╗
║ BLUEMARBLE WORKSTATION PROXIMITY                   ║
╠════════════════════════════════════════════════════╣
║                                                     ║
║ Range-Based System:                                ║
║                                                     ║
║ Direct Interaction (0-2m):                         ║
║ ├─ Full workstation benefits                       ║
║ ├─ All recipes available                           ║
║ └─ Maximum quality bonuses                         ║
║                                                     ║
║ Near Proximity (2-10m):                            ║
║ ├─ Basic recipes available                         ║
║ ├─ Reduced quality bonuses (-5%)                   ║
║ └─ Allows movement during crafting                 ║
║                                                     ║
║ Building Interior (10-30m):                        ║
║ ├─ Can queue crafting orders                       ║
║ ├─ Must be at station to start                     ║
║ └─ Enables workshop management UI                  ║
║                                                     ║
║ Out of Range (>30m):                               ║
║ ├─ Cannot craft at this workstation                ║
║ ├─ Can still view recipes for planning             ║
║ └─ Shows path to nearest suitable station          ║
║                                                     ║
╚════════════════════════════════════════════════════╝
```text

**Benefits:**

- Balances realism with playability
- Allows social interaction while crafting
- Doesn't interrupt crafting for small movements
- Creates meaningful workstation placement decisions

### Workstation Ownership and Access

#### Public vs. Private Workstations

```text
┌────────────────────────────────────────────────────┐
│ FORGE: "Town Smithy"                               │
│ ──────────────────────────────────────────────     │
│                                                     │
│ Type: Public Workshop Forge                        │
│ Owner: Town of Riverside                           │
│ Access: Public Use (small fee per craft)           │
│                                                     │
│ Quality Bonus: +10%                                │
│ Speed Bonus: -15% time                             │
│ Usage Fee: 5 silver per craft                      │
│                                                     │
│ Current Users: 2/6                                 │
│ Queue: None                                        │
│                                                     │
│ [Use Workstation] [View Queue]                     │
└────────────────────────────────────────────────────┘
```text

**Workstation Access Models:**

- **Public**: Anyone can use, possible fees, potential queues
- **Private**: Owner and permissions only, no fees, exclusive use
- **Guild**: Guild members have priority, others may need permission
- **Rental**: Temporary exclusive access for fee

## UI/UX Design Patterns for Basic Crafting

### Pattern 1: Material Availability Indicators

#### Visual Feedback System

```text
┌────────────────────────────────────────────────────┐
│ REQUIRED MATERIALS                                 │
│ ──────────────────────────────────────────────     │
│                                                     │
│ ✓ Iron Ingot × 3              [Have: 12]          │
│   Quality: 65%-85% available                       │
│   Location: Inventory                              │
│                                                     │
│ ✓ Oak Plank × 1               [Have: 5]           │
│   Quality: 70%-75% available                       │
│   Location: Inventory                              │
│                                                     │
│ ✗ Leather Strips × 2          [Have: 0]           │
│   Can craft from: Raw Leather (Have: 1)            │
│   Or purchase from: Leatherworker (Town Square)    │
│                                                     │
│ Status: Missing 2 materials                        │
│ [Show Crafting Path] [Find Materials]              │
└────────────────────────────────────────────────────┘
```text

**Key Elements:**

- Checkmark/X for instant recognition
- Show quantity available vs. needed
- Indicate quality range of available materials
- Suggest acquisition paths for missing items
- Progressive disclosure (expandable details)

### Pattern 2: Recipe Filtering and Search

#### Smart Filter System

```text
┌────────────────────────────────────────────────────┐
│ BLACKSMITHING RECIPES                              │
│ ──────────────────────────────────────────────     │
│                                                     │
│ [Search: ________]  [⚙ Advanced Filters]          │
│                                                     │
│ Quick Filters:                                     │
│ ◉ Can Craft Now        ○ All Recipes               │
│ ○ Have Materials       ○ Learned Only              │
│                                                     │
│ Sort By: [Difficulty ▼]                            │
│                                                     │
│ ─────────────────────────────────────────────────  │
│                                                     │
│ ✓ Iron Nails × 10                    [Level 1]    │
│   Success: 98% | Time: 10s | XP: 5                │
│                                                     │
│ ✓ Iron Sword                         [Level 8]    │
│   Success: 85% | Time: 30s | XP: 45               │
│                                                     │
│ ⚠ Steel Longsword                   [Level 15]    │
│   Success: 52% | Time: 45s | XP: 120              │
│   Missing: Quality Hammer                          │
│                                                     │
│ ✗ Damascus Blade                    [Level 35]    │
│   Not Yet Learned                                  │
│   Learn from: Master Smith (City)                  │
│                                                     │
└────────────────────────────────────────────────────┘
```text

**Design Principles:**

- Default to "Can Craft Now" for beginners
- Clear status indicators (✓ ready, ⚠ possible, ✗ locked)
- Critical info visible without clicking (success, time, XP)
- Path to unlock shown for unavailable recipes
- Search and filters for experienced crafters

### Pattern 3: Crafting Queue Management

#### Queue System Interface

```text
┌────────────────────────────────────────────────────┐
│ CRAFTING QUEUE                                     │
│ ──────────────────────────────────────────────     │
│                                                     │
│ Currently Crafting:                                │
│ ├─ Iron Sword (2 of 5)                            │
│ │  Progress: [██████░░░░] 60%                     │
│ │  Time Remaining: ~12 seconds                    │
│ │  [Cancel Current]                                │
│ │                                                  │
│ └─ Next in Queue:                                  │
│    1. Iron Sword (3/5)        ~30s                │
│    2. Iron Sword (4/5)        ~30s                │
│    3. Iron Sword (5/5)        ~30s                │
│    4. Iron Dagger (1/3)       ~25s                │
│                                                     │
│ Total Queue Time: ~3 minutes 25 seconds            │
│                                                     │
│ Materials Reserved:                                │
│ ├─ Iron Ingot: 21 of 25 in inventory              │
│ └─ Oak Plank: 8 of 10 in inventory                │
│                                                     │
│ [Clear Queue] [Pause Queue] [Add More]             │
└────────────────────────────────────────────────────┘
```text

**Features:**

- Visual progress for current item
- Clear queue order and timing
- Material reservation prevents crafting conflicts
- Can manage queue without interrupting crafting
- Batch crafting support reduces repetitive clicking

### Pattern 4: Success Probability Display

#### Clear Risk Communication

```text
┌────────────────────────────────────────────────────┐
│ CRAFTING CONFIRMATION                              │
│ ──────────────────────────────────────────────     │
│                                                     │
│ Recipe: Steel Longsword                            │
│ Quantity: 1                                        │
│                                                     │
│ SUCCESS PROBABILITY: 88% 🟢 [Excellent]            │
│                                                     │
│ ┌──────────────────────────────────────────┐      │
│ │ Success Rate Breakdown:                  │      │
│ │ ────────────────────────────────         │      │
│ │ Base Rate:              75%              │      │
│ │ + Your Skill:          +13%              │      │
│ │ + Tool Quality:         +8%              │      │
│ │ + Workstation:          +5%              │      │
│ │ - Recipe Complexity:   -13%              │      │
│ │                        ────               │      │
│ │ Total:                  88% 🟢           │      │
│ └──────────────────────────────────────────┘      │
│                                                     │
│ Expected Quality: 75% (Fine)                       │
│ Crafting Time: 45 seconds                          │
│ Experience Gain: 180 XP (success) / 50 XP (fail)  │
│                                                     │
│ Note: Materials are not consumed on failure        │
│                                                     │
│ [Cancel] [Begin Crafting]                          │
└────────────────────────────────────────────────────┘
```text

**Design Elements:**

- Large, clear percentage with color coding
- Expandable breakdown for curious players
- Reassurance about material preservation
- Realistic outcome expectations
- Confidence-building information

## Comparative Game Analysis

### Game 1: World of Warcraft - Streamlined Approach

**Workflow:**

1. Open profession window (hotkey accessible)
2. See all available recipes filtered by materials
3. Click recipe, auto-selects materials
4. Click "Create" or "Create All"
5. Items appear instantly in bags

**Tools:** Profession-specific tools provide stat bonuses, not required for basic crafting

**Workstations:** Only needed for specialized recipes, most crafting is portable

**Strengths:**

- Extremely fast and efficient
- Minimal friction for casual crafting
- Clear material requirements
- Batch crafting support

**Weaknesses:**

- Lacks immersion and realism
- No spatial/social crafting element
- Tools feel optional rather than essential
- Instant creation feels unrealistic

**Applicability to BlueMarble:** Low - too arcade-like for BlueMarble's simulation focus

### Game 2: Vintage Story - Realistic Approach

**Workflow:**

1. Gather materials near workstation
2. Place materials in specific grid pattern
3. Use appropriate tool on workstation
4. Wait for crafting animation (2-30 seconds)
5. Retrieve finished item from workstation

**Tools:** Required for all crafting, different tools for different operations, tools degrade with use

**Workstations:** Essential for most crafting, must be built and maintained

**Strengths:**

- Highly immersive and realistic
- Tools and workstations feel meaningful
- Spatial awareness required
- Rewarding for simulation fans

**Weaknesses:**

- Steep learning curve for new players
- Time-consuming for bulk operations
- Can be tedious for basic items
- Requires significant world knowledge

**Applicability to BlueMarble:** Medium-High - aligns with simulation philosophy but may need
streamlining

### Game 3: Final Fantasy XIV - Interactive Approach

**Workflow:**

1. Select recipe from crafting log
2. Confirm material usage
3. Interactive crafting minigame begins
4. Make strategic skill choices
5. Reach quality/progress thresholds
6. Item created with determined quality

**Tools:** Specialized tools required, provide stat bonuses, wear down and need repair

**Workstations:** Not required but provide buffs if used

**Strengths:**

- Engaging skill-based system
- Quality outcome tied to player performance
- Deep progression and mastery
- Crafting feels like a class/role

**Weaknesses:**

- Complex for casual crafters
- Time investment per item is high
- Requires learning rotation/priorities
- Can feel repetitive at scale

**Applicability to BlueMarble:** Medium - interactive elements good but may be too complex for
basic crafting

### Game 4: Eco Global Survival - Educational Approach

**Workflow:**

1. Research and learn recipe
2. Build appropriate workstation
3. Ensure materials and tools available
4. Set up crafting order with quantity
5. Workstation processes automatically
6. Return to collect finished items

**Tools:** Placed in workstation, affect output quality and speed

**Workstations:** Core to gameplay, elaborate structures with upgrade paths

**Strengths:**

- Realistic production chain simulation
- Set-and-forget for bulk crafting
- Encourages workshop organization
- Tools/stations are strategic investments

**Weaknesses:**

- Initial setup is complex
- Can feel disconnected from crafting
- Requires space and planning
- Learning curve for new players

**Applicability to BlueMarble:** Medium-High - automated aspects good for scaling, maintains
immersion

### Synthesis: Best Practices for BlueMarble

Based on comparative analysis:

**Adopt from WoW:**

- Clear material requirement display
- One-click recipe selection
- Queue system for repetitive crafting

**Adopt from Vintage Story:**

- Meaningful tool and workstation requirements
- Proximity-based crafting
- Realistic time investment per item

**Adopt from FFXIV:**

- Tool durability and repair mechanics
- Quality variation based on inputs
- Skill progression affects outcomes

**Adopt from Eco:**

- Workstation upgrade paths
- Clear indication of tool/station benefits
- Production chain visualization

## Workflow Diagrams

### Diagram 1: Complete Basic Crafting Flow

```text
┌───────────────────────────────────────────────────────────────────────────┐
│                     BLUEMARBLE BASIC CRAFTING WORKFLOW                    │
└───────────────────────────────────────────────────────────────────────────┘

    Player Wants to Craft Item
              ↓
    ┌─────────────────────┐
    │ Recipe Discovery    │
    └─────────────────────┘
              ↓
         Has Recipe?
         ↙        ↘
      NO          YES
       ↓           ↓
    Learn    ┌──────────────────┐
    Recipe   │ Check Materials  │
       ↓     └──────────────────┘
       └──────────→  ↓
              Has Materials?
              ↙          ↘
           NO            YES
            ↓             ↓
    ┌──────────────┐  ┌──────────────────┐
    │ Gather or    │  │ Check Tools      │
    │ Purchase     │  └──────────────────┘
    │ Materials    │       ↓
    └──────────────┘    Has Appropriate Tool?
            ↓           ↙              ↘
            └────────→ NO              YES
                       ↓                ↓
                 ┌──────────────┐  ┌──────────────────────┐
                 │ Craft or Buy │  │ Travel to Workstation│
                 │ Tool         │  └──────────────────────┘
                 └──────────────┘       ↓
                       ↓           At Workstation?
                       └────────────→  ↓
                                   ┌──────────────────────┐
                                   │ Open Crafting UI     │
                                   └──────────────────────┘
                                        ↓
                                   ┌──────────────────────┐
                                   │ Select Recipe        │
                                   │ - Review materials   │
                                   │ - Check tool effects │
                                   │ - View success rate  │
                                   └──────────────────────┘
                                        ↓
                                   ┌──────────────────────┐
                                   │ Confirm Crafting     │
                                   │ - Auto-select best   │
                                   │   materials          │
                                   │ - Set quantity       │
                                   └──────────────────────┘
                                        ↓
                                   ┌──────────────────────┐
                                   │ Crafting Executes    │
                                   │ - Progress bar shown │
                                   │ - Time estimate      │
                                   │ - Can queue more     │
                                   └──────────────────────┘
                                        ↓
                                   Success Roll
                                   ↙         ↘
                             SUCCESS        FAILURE
                                ↓              ↓
                     ┌──────────────────┐  ┌──────────────────┐
                     │ Item Created     │  │ Attempt Failed   │
                     │ - Quality rolled │  │ - Materials kept │
                     │ - XP awarded     │  │ - Reduced XP     │
                     │ - Durability loss│  │ - Try again?     │
                     └──────────────────┘  └──────────────────┘
                              ↓                     ↓
                     ┌──────────────────┐           ↓
                     │ Craft Another?   │←──────────┘
                     └──────────────────┘
                              ↓
                         YES / NO
```text

### Diagram 2: Tool Selection Decision Tree

```text
┌───────────────────────────────────────────────────────────────────────────┐
│                        TOOL SELECTION DECISION TREE                       │
└───────────────────────────────────────────────────────────────────────────┘

    Player Begins Crafting
            ↓
    Recipe Requires Tool Type: X
            ↓
    ┌─────────────────────┐
    │ Scan Inventory for  │
    │ Tools of Type X     │
    └─────────────────────┘
            ↓
       Found Tools?
       ↙          ↘
     NO            YES
      ↓             ↓
  ┌────────┐   ┌──────────────────────────────┐
  │ ERROR: │   │ Evaluate Each Tool:          │
  │ Cannot │   │ 1. Quality level             │
  │ Craft  │   │ 2. Current durability        │
  │        │   │ 3. Special properties        │
  └────────┘   └──────────────────────────────┘
                        ↓
                 ┌──────────────────────┐
                 │ Apply Selection Rule │
                 └──────────────────────┘
                        ↓
                 Default: "Best Available"
                        ↓
         ┌──────────────┴──────────────┐
         ↓                             ↓
    Multiple Tools              Single Tool
    Same Quality                Available
         ↓                             ↓
    Select tool with              Select that tool
    highest durability                  ↓
         ↓                              ↓
         └──────────────┬───────────────┘
                        ↓
                 ┌─────────────────┐
                 │ Tool Selected   │
                 └─────────────────┘
                        ↓
                 Player Override?
                 ↙            ↘
              YES              NO
                ↓              ↓
         ┌──────────────┐     ↓
         │ Show tool    │     ↓
         │ picker UI    │     ↓
         └──────────────┘     ↓
                ↓              ↓
         ┌──────────────┐     ↓
         │ Player picks │     ↓
         │ preferred    │     ↓
         │ tool         │     ↓
         └──────────────┘     ↓
                ↓              ↓
                └──────┬───────┘
                       ↓
              Apply Tool Modifiers:
              ├─ Quality bonus/penalty
              ├─ Speed modifier
              ├─ Special effects
              └─ Durability cost
                       ↓
              Proceed with Crafting
```text

### Diagram 3: Workstation Proximity System

```text
┌───────────────────────────────────────────────────────────────────────────┐
│                      WORKSTATION PROXIMITY ZONES                          │
└───────────────────────────────────────────────────────────────────────────┘

                             [Workstation]
                                  ★
                                  
    ╔══════════════════════════════════════════════════════╗
    ║ ZONE 1: Direct Interaction (0-2 meters)              ║
    ║ ───────────────────────────────────────────          ║
    ║ • Full recipe access                                 ║
    ║ • 100% workstation bonuses                           ║
    ║ • Can modify tool/material selections                ║
    ║ • Fastest crafting speeds                            ║
    ╚══════════════════════════════════════════════════════╝
                    │                 │
    ╔═══════════════╧═════════════════╧══════════════════╗
    ║ ZONE 2: Near Proximity (2-10 meters)               ║
    ║ ──────────────────────────────────────────          ║
    ║ • Basic recipe access only                          ║
    ║ • 50% workstation bonuses (-5% quality)             ║
    ║ • Can move around while crafting                    ║
    ║ • Normal crafting speeds                            ║
    ╚═════════════════════════════════════════════════════╝
                    │                 │
    ╔═══════════════╧═════════════════╧══════════════════╗
    ║ ZONE 3: Building Interior (10-30 meters)            ║
    ║ ──────────────────────────────────────────          ║
    ║ • Can queue crafts (must return to start)           ║
    ║ • No workstation bonuses                            ║
    ║ • Workshop management interface available           ║
    ║ • Can view recipes for planning                     ║
    ╚═════════════════════════════════════════════════════╝
                    │                 │
    ╔═══════════════╧═════════════════╧══════════════════╗
    ║ ZONE 4: Out of Range (>30 meters)                  ║
    ║ ──────────────────────────────────────────          ║
    ║ • Cannot craft at this workstation                  ║
    ║ • Can still view recipe book                        ║
    ║ • UI shows path to nearest workstation              ║
    ║ • Can prepare shopping list                         ║
    ╚═════════════════════════════════════════════════════╝

    Legend:
    ★ = Workstation location
    ═ = Proximity zone boundary
    Player can craft at any position within Zones 1-2
    Movement between zones updates available recipes and bonuses in real-time
```text

## Recommendations for BlueMarble

### High Priority Recommendations

#### 1. Implement Hybrid Workflow System

**Description:** Combine timed crafting with quality variance and tool/workstation effects

**Rationale:**

- Balances realism (geological simulation) with playability
- Creates meaningful skill progression
- Provides depth without overwhelming complexity
- Supports both casual and dedicated crafters

**Implementation Steps:**

1. Define base crafting times for item complexity tiers (5s, 15s, 30s, 60s)
2. Implement tool quality modifiers (speed and quality)
3. Create workstation bonus system
4. Add queue management for bulk crafting
5. Implement success/quality roll system

**Timeline:** Q1 2026 - Weeks 1-4

**Dependencies:**

- [Crafting Success Model](../../docs/gameplay/mechanics/crafting-success-model.md)
- [Crafting Quality Model](../../docs/gameplay/mechanics/crafting-quality-model.md)

#### 2. Create Progressive Disclosure UI

**Description:** Show simple interface to beginners, reveal complexity to experienced crafters

**Rationale:**

- Reduces initial learning curve
- Prevents overwhelming new players
- Rewards expertise with control options
- Maintains clean interface

**Implementation Steps:**

1. Design three UI complexity modes: Beginner, Standard, Advanced
2. Beginner mode: Auto-select everything, show only basic info
3. Standard mode: Show tool/material selection, success rates
4. Advanced mode: Full breakdown, manual material selection, optimization tools
5. Auto-upgrade mode based on player experience

**Timeline:** Q1 2026 - Weeks 3-6

**Dependencies:** Core UI framework, player progression tracking

#### 3. Implement Smart Material Selection

**Description:** Default to best available materials with option to manually adjust

**Rationale:**

- Speeds up crafting for experienced players
- Reduces clicks and decision fatigue
- Still allows optimization when desired
- Prevents accidental use of poor materials

**Implementation Steps:**

1. Create material quality scoring algorithm
2. Auto-select highest quality materials by default
3. Add override UI for manual selection
4. Show quality impact preview
5. Remember player preferences per recipe

**Timeline:** Q1 2026 - Weeks 2-3

**Dependencies:** Material quality system, inventory management

### Medium Priority Recommendations

#### 4. Develop Tool Durability System

**Description:** Repairable tools that degrade with use but don't break catastrophically

**Rationale:**

- Creates material sink without frustration
- Encourages tool maintenance gameplay
- Rewards planning and preparation
- Generates economic activity

**Implementation Steps:**

1. Define durability values per tool tier
2. Implement durability tracking
3. Create "Damaged" state with penalties
4. Add repair interface and costs
5. Show durability warnings in UI

**Timeline:** Q1 2026 - Weeks 5-7

**Dependencies:** Tool system, economy balance

#### 5. Create Workstation Proximity System

**Description:** Range-based system with graduated benefits based on distance

**Rationale:**

- Balances realism with playability
- Allows social crafting
- Creates meaningful placement decisions
- Prevents exploitation (crafting from anywhere)

**Implementation Steps:**

1. Implement distance calculation system
2. Define zone boundaries and effects
3. Create visual feedback for current zone
4. Add zone-appropriate UI elements
5. Test performance impact

**Timeline:** Q1 2026 - Weeks 6-8

**Dependencies:** Spatial systems, workstation placement code

#### 6. Build Recipe Filter System

**Description:** Smart filtering showing "Can Craft Now" by default with advanced options

**Rationale:**

- Reduces cognitive load
- Helps players find appropriate challenges
- Prevents scrolling through impossible recipes
- Scales well as recipe count grows

**Implementation Steps:**

1. Implement recipe availability checking
2. Create filter categories (can craft, have materials, learned, all)
3. Add search functionality
4. Implement sorting options
5. Optimize for large recipe counts

**Timeline:** Q1 2026 - Weeks 4-6

**Dependencies:** Recipe database, material checking system

### Low Priority Recommendations (Post-Q1 2026)

#### 7. Add Crafting Queue System

**Description:** Allow queuing multiple items to reduce repetitive clicking

**Implementation:** Post-launch enhancement

#### 8. Create Workstation Sharing

**Description:** Public/private/guild workstation access controls

**Implementation:** With multiplayer features

#### 9. Implement Recipe Discovery

**Description:** Learn recipes through exploration, experimentation, NPCs

**Implementation:** With quest/exploration systems

## Implementation Considerations

### Technical Requirements

#### Performance Considerations

```text
Crafting System Performance Budget:
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
UI Update Rate:
├─ Recipe list filtering: < 50ms for 1000 recipes
├─ Material availability check: < 10ms per recipe
└─ Success probability calculation: < 5ms

Workstation Queries:
├─ Proximity check: < 1ms per frame
├─ Workstation enumeration: < 20ms in 50m radius
└─ Recipe availability: < 10ms for 100 recipes

Crafting Execution:
├─ Start crafting: < 50ms
├─ Progress updates: < 5ms per tick
└─ Completion: < 20ms

Memory:
├─ Recipe database: ~5MB for 1000 recipes
├─ Active crafting states: ~1KB per player
└─ UI textures: ~10MB total
━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
```text

### Data Structures

#### Recipe Definition

```json
{
  "recipeId": "iron_sword_basic",
  "displayName": "Iron Sword",
  "category": "blacksmithing",
  "difficulty": 8,
  "baseTime": 30,
  "requiredMaterials": [
    {
      "materialType": "iron_ingot",
      "quantity": 3,
      "qualityModifier": "standard"
    },
    {
      "materialType": "wood_handle",
      "quantity": 1,
      "qualityModifier": "any"
    }
  ],
  "requiredTool": {
    "toolType": "hammer",
    "minQuality": "basic"
  },
  "requiredWorkstation": {
    "stationType": "forge",
    "minQuality": "basic"
  },
  "outputItem": {
    "itemType": "iron_sword",
    "quantity": 1,
    "qualityVariable": true
  },
  "experienceReward": {
    "onSuccess": 45,
    "onFailure": 12
  }
}
```text

### Integration Points

#### Existing Systems to Connect

1. **Material Quality System** (from geological simulation)
   - Query material properties
   - Calculate quality modifiers
   - Track material sources

2. **Skill Progression System**
   - Award experience
   - Check skill requirements
   - Apply skill bonuses

3. **Inventory Management**
   - Check material availability
   - Reserve materials for queue
   - Add crafted items

4. **World/Building System**
   - Query workstation locations
   - Check proximity
   - Validate workstation access

5. **Economy System**
   - Track crafting costs
   - Apply workstation fees
   - Update market prices

### Testing Strategy

#### Unit Tests

- Recipe availability calculation
- Material requirement checking
- Success probability calculation
- Quality calculation
- Tool/workstation modifier application

#### Integration Tests

- Complete crafting workflow
- Queue management
- Multi-player workstation access
- Material reservation conflicts

#### Playtest Scenarios

- New player first crafting experience (15 minutes)
- Bulk crafting session (1 hour)
- Tool degradation and repair cycle
- Workstation proximity edge cases
- Queue management under lag

## Conclusion

### Summary of Key Findings

This research establishes that successful basic crafting systems balance accessibility with depth
through:

1. **Clear, Streamlined Workflows**: Players should understand the path from materials to finished
   items within minutes of first exposure

2. **Meaningful Tool Integration**: Tools should feel essential and impactful without creating
   frustrating barriers to entry

3. **Smart Workstation Mechanics**: Proximity requirements create spatial gameplay while range-based
   systems prevent tedious positioning

4. **Progressive Complexity**: UI and mechanics should scale with player expertise, hiding
   complexity until players are ready for it

5. **Efficient Resource Management**: Interfaces should minimize cognitive load in material
   selection while allowing optimization

### Alignment with BlueMarble Vision

These recommendations align with BlueMarble's core principles:

- **Geological Realism**: Tools and workstations create authentic crafting processes
- **Skill-Based Progression**: Player skill matters through tool selection and material choices
- **Meaningful Choices**: Quality vs. speed trade-offs create strategic depth
- **Accessibility**: Progressive disclosure ensures new players aren't overwhelmed
- **Social Gameplay**: Workstation sharing and proximity enable collaborative crafting

### Expected Outcomes

Implementing these recommendations will result in:

- **Improved New Player Experience**: 90% of new players complete first craft successfully within
  5 minutes
- **Increased Engagement**: 60% of players engage with crafting systems regularly
- **Economic Activity**: Tool and workstation markets create sustainable economy
- **Skill Progression**: Clear path from basic to advanced crafting maintains long-term interest
- **Reduced Friction**: Smart defaults and filters reduce unnecessary clicks by 70%

### Next Steps

1. **Immediate (Week 1)**: Review and approve recommended workflow model
2. **Near-term (Weeks 2-4)**: Implement core crafting execution system
3. **Medium-term (Weeks 5-8)**: Build UI components and tool/workstation systems
4. **Testing (Weeks 9-12)**: Internal playtest and iterate based on feedback
5. **Polish (Week 13+)**: Optimize performance and refine UX details

## Appendices

### Appendix A: Terminology Glossary

| Term | Definition |
|------|------------|
| Base Crafting | Simple 1-3 material recipes that form foundation of crafting system |
| Progressive Disclosure | UI design pattern that reveals complexity gradually as needed |
| Proximity Zone | Defined area around workstation with specific crafting rules |
| Quality Modifier | Numerical bonus/penalty affecting final item quality |
| Success Probability | Chance of crafting attempt succeeding vs. failing |
| Tool Durability | Number of uses remaining before tool needs repair |
| Workstation Bonus | Quality or speed improvement from using better workstations |

### Appendix B: UI Mockup References

See also: [Crafting Interface Mockups](./assets/crafting-interface-mockups.md)

The mockups document contains detailed visual representations of:

- Main crafting hub interface
- Recipe selection and material view
- Interactive crafting progress
- Success/failure result screens
- Tool and workstation selection

These mockups informed the recommendations in this research and should be referenced during
implementation.

### Appendix C: Recipe Examples for Testing

#### Simple Recipe (Beginner-Friendly)

```text
Iron Nails × 10
─────────────────────────────────────────────────
Materials:
  • Iron Ingot × 1

Tool: Any Hammer
Workstation: Any Forge
Time: 10 seconds
Difficulty: Level 1
Success Rate: 95%+ for new players
```text

#### Standard Recipe (Core Gameplay)

```text
Iron Sword
─────────────────────────────────────────────────
Materials:
  • Iron Ingot × 3
  • Oak Handle × 1

Tool: Standard Hammer or better
Workstation: Basic Forge or better
Time: 30 seconds
Difficulty: Level 8
Success Rate: 85% at recommended level
```text

#### Complex Recipe (Challenging)

```text
Steel Longsword
─────────────────────────────────────────────────
Materials:
  • Steel Ingot × 4
  • Leather Strips × 2
  • Oak Handle × 1
  • (Optional) Quenching Oil × 1

Tool: Quality Hammer or better
Workstation: Workshop Forge or better
Time: 45 seconds
Difficulty: Level 15
Success Rate: 65% at recommended level
```text

### Appendix D: Related Documentation

**BlueMarble Core Systems:**

- [Crafting Mechanics Overview](../../docs/gameplay/mechanics/crafting-mechanics-overview.md)
- [Crafting Success Model](../../docs/gameplay/mechanics/crafting-success-model.md)
- [Crafting Quality Model](../../docs/gameplay/mechanics/crafting-quality-model.md)
- [Player Progression System](../../docs/gameplay/spec-player-progression-system.md)

**Research Documents:**

- [Assembly Skills System Research](./assembly-skills-system-research.md)
- [Advanced Crafting System Research](./advanced-crafting-system-research.md)
- [Skill Knowledge System Research](./skill-knowledge-system-research.md)

**UI/UX References:**

- [Crafting Interface Mockups](./assets/crafting-interface-mockups.md)

### Appendix E: Game References and Sources

**Primary Research Games:**

1. World of Warcraft - Streamlined crafting workflow
2. Final Fantasy XIV - Interactive crafting system
3. Wurm Online - Realistic simulation approach
4. Vintage Story - Spatial crafting mechanics
5. Eco Global Survival - Production chain systems
6. Life is Feudal - Tool and workstation depth
7. Novus Inceptio - Survival crafting balance

**Community Resources:**

- Game-specific wikis and databases
- YouTube tutorial series
- Reddit crafting guides and discussions
- Developer GDC talks on crafting systems

### Appendix F: Success Metrics

**Player Experience Metrics:**

- Time to first successful craft: Target < 5 minutes for new players
- Crafting tutorial completion rate: Target > 90%
- Daily active crafters: Target > 60% of player base
- Average crafting session length: Target 15-30 minutes
- Recipe discovery rate: Target > 5 new recipes per week per player

**Technical Metrics:**

- UI response time: < 50ms for all operations
- Server load per crafting operation: < 5ms
- Memory usage per active crafter: < 2MB
- Network bandwidth per craft: < 1KB

**Economic Metrics:**

- Tool market transaction volume: Sustained demand
- Workstation utilization rate: > 40% during peak hours
- Material price stability: < 20% weekly variance
- Crafted item quality distribution: Bell curve centered on "Good" tier

### Appendix G: Revision History

| Version | Date | Author | Changes |
|---------|------|--------|---------|
| 1.0 | 2025-01-15 | BlueMarble Research Team | Initial research report |

---

## End of Research Report

For questions or clarifications, contact the BlueMarble Game Design Research Team.
