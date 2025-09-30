# Skill Caps, Experience, and Skill Decay in Progression Systems

**Document Type:** Design Research & Analysis  
**Version:** 1.0  
**Author:** BlueMarble Game Design Research Team  
**Date:** 2025-01-07  
**Status:** Research Report  
**Research Type:** Design Research  
**Priority:** Medium  
**Parent Research:** [Skill and Knowledge System Research](skill-knowledge-system-research.md)

## Executive Summary

This research document analyzes skill caps, experience-based progression, and skill decay mechanics in RPG
progression systems. The study examines how level-based caps, use-based skill improvement, and decay mechanics
interact to create natural specialization without arbitrary hard limits. The research concludes that combining
level-based category caps with skill decay provides sufficient specialization pressure, making additional global
caps unnecessary.

**Key Findings:**
- Level-based skill category caps effectively limit early-game power creep
- Skill decay mechanics create natural specialization through maintenance costs
- Combined systems prevent "master-of-all-trades" without feeling overly restrictive
- Decay floors prevent complete skill loss while allowing for rustiness
- Additional global caps beyond category limits are likely redundant
- Alternative hard cap systems with minimum competence thresholds offer different trade-offs
- Hierarchical skill trees (category → group → specific) enable granular specialization while reducing
  micromanagement through shared group points
- Tag-based systems (domain + discipline + material tags) offer greater flexibility than hierarchies for
  simulation-heavy games, allowing natural skill transfer and emergent specialization
- Weighted tag calculations with quality scores provide concrete, repeatable crafting mechanics
- With 23+ skill categories, decay becomes essential for forcing meaningful specialization choices

**Recommendations:**
- Focus on tuning existing decay rates rather than adding new cap systems
- Implement grace periods or slower decay for high-level skills
- Set decay floors to maintain baseline competence
- Balance decay severity with player enjoyment and accessibility
- Consider hard cap alternative: 2 master skills, 500 flexible points, minimum competence (15) for all trained
  skills with higher failure rates
- For hierarchical systems: use shared group points for material-based skills (e.g., gathering/metal/iron)
- For tag-based systems: use weighted calculations (domain 40%, discipline 30%, material 20%, exact 10%)
- Implement quality tiers based on Quality Score vs Recipe Complexity ratios (0.5×, 0.8×, 1.2×, 1.5×)
- Tag-based approach recommended for BlueMarble's geological simulation due to flexibility and natural skill
  transfer across similar contexts
- With 23+ skills, organize into meta-categories to guide specialization and prevent maintenance overload

## Table of Contents

1. [Research Context](#research-context)
2. [Experience and Automatic Skill Improvement](#experience-and-automatic-skill-improvement)
3. [Skill Caps and Level-Based Limits](#skill-caps-and-level-based-limits)
4. [Skill Decay and Use-It-Or-Lose-It Mechanics](#skill-decay-and-use-it-or-lose-it-mechanics)
5. [Specialization vs Generalization in Practice](#specialization-vs-generalization-in-practice)
6. [Design Analysis and Recommendations](#design-analysis-and-recommendations)
7. [Integration with BlueMarble Systems](#integration-with-bluemarble-systems)
8. [References and Sources](#references-and-sources)

## Research Context

This research addresses the design challenge of preventing "master-of-all-trades" characters in skill-based
progression systems while maintaining player freedom and enjoyment. The analysis focuses on systems that use:

- **Experience-based leveling** that unlocks skill point capacity
- **Automatic skill improvement** through usage ("learn by doing")
- **Category-based skill caps** tied to character level
- **Skill decay mechanics** that reduce unused skills over time

The goal is to determine whether additional caps are needed beyond level-based category limits and skill decay,
or if these mechanics alone provide sufficient specialization pressure.

## Experience and Automatic Skill Improvement

Many RPGs allow characters to improve skills by using them rather than assigning points manually. This "learn by
doing" approach mirrors real-life practice and creates intuitive progression. 

### Use-Based Progression Examples

**Darkfall: Rise of Agon** (MMORPG):
- Skills level up simply by using them
- Swing a sword to raise sword skill
- Shoot arrows to train archery
- No manual point allocation required

**System Design Pattern:**
```
Character gains XP → Level increases → Skill point capacity increases
Skills improve automatically through use → XP contributes to next level
```

### The Challenge: Unlimited Progression

Purely use-based systems can lead to unintended consequences if unlimited:
- Players attempt to train every single skill
- "Jack-of-all-trades" characters become optimal
- Character specialization and identity diminish
- Long-term progression loses meaning

This necessitates some form of limitation to encourage specialization and maintain character diversity.

## Skill Caps and Level-Based Limits

A **skill cap** is a limit on how many skill points a character can have, either in total or within certain
categories. The goal is to force **specialization**: players must choose which skills to excel in rather than
maxing out all skills.

### Level-Based Category Caps

**Design Pattern:**
```
Character Level → Determines maximum skill points per category
Category: Combat → Max points = Level × multiplier
Category: Crafting → Max points = Level × multiplier
Category: Social → Max points = Level × multiplier
```

**Example Implementation:**
- Level 10 character: 30 Combat points, 30 Crafting points, 30 Social points
- Level 50 character: 150 Combat points, 150 Crafting points, 150 Social points

### Historical Context: D&D 3rd Edition

D&D 3rd Edition limited skill ranks based on character level:
- **Max Rank in Class Skills:** Character Level + 3
- **Max Rank in Cross-Class Skills:** (Character Level + 3) / 2

This system:
- Prevented low-level characters from having absurdly high expertise
- Ensured progression felt earned as players leveled up
- Created natural skill specialization through opportunity costs

### Benefits of Level-Based Caps

1. **Early Game Balance:** Prevents low-level power spikes
2. **Progressive Scaling:** Character capacity grows with advancement
3. **Natural Pacing:** Skills scale with overall character development
4. **Multiple Categories:** Allows diverse builds without unlimited power

### Question: Are Additional Caps Needed?

Given that character level already controls skill capacity per category, the question becomes whether an
**additional global cap** is necessary. Examples of global caps include:

**Ultima Online:**
- Total skill points capped at 700
- Enforces 7 skills at Grandmaster (100 points each)
- Hard limit regardless of other factors

**Analysis:** If category-based caps already exist and scale with level, a global cap might be:
- **Redundant:** Category limits already force choices
- **Overly restrictive:** May feel unnecessarily constraining
- **Potentially unnecessary:** Especially when combined with decay mechanics

## Skill Decay and Use-It-Or-Lose-It Mechanics

The concept of **skill decay** reflects that skills diminish over time if unused, mirroring real-life ability
deterioration with neglect.

### Design Principles

**Core Mechanic:**
- Characters "forget" skills if they don't use them for extended periods
- Skills gradually decrease in effective level over time
- Decay rate may vary by skill type or current level
- XP unlocks capacity but doesn't permanently lock in skills

**Real-World Analogy:**
- Language skills fade without practice
- Muscle memory deteriorates with disuse
- Complex procedures require regular rehearsal
- Some skills never completely disappear (like riding a bike)

### Purpose of Skill Decay

**Game Balance:**
- Prevents "master-of-all-trades" characters without active maintenance
- Creates opportunity costs for skill diversity
- Encourages ongoing engagement with chosen specializations
- Naturally enforces character identity through time constraints

**Realism:**
- Specialists exist because maintaining expertise requires dedication
- Time spent on one craft is time not spent on another
- True mastery demands consistent practice and application
- Neglected skills naturally atrophy

### Decay Floors: Preventing Total Loss

**Design Pattern:**
```
Skill Level 0-10: No decay (permanent baseline)
Skill Level 11-50: Slow decay to level 10
Skill Level 51-100: Moderate decay to level 25
Skill Level 100+: Fast decay to level 50
```

**Benefits:**
- Characters retain baseline competence permanently
- Prevents frustration of complete skill loss
- Mirrors real-world retention of fundamental skills
- Allows temporary skill diversification without permanent penalty

**RimWorld Example:**
- Skills decay only above certain levels
- Represents being "rusty but not forgetting entirely"
- Analogous to bike riding: core ability remains

### Combining Decay with Level-Based Caps

**Dynamic Equilibrium:**

1. **Character levels up** → Skill capacity increases
2. **Player uses skills** → Skills improve toward new capacity
3. **Player spreads too thin** → Some skills unused
4. **Unused skills decay** → Fall to maintenance level
5. **Result:** Natural specialization through time management

**Emergent Specialization:**
- Characters can theoretically learn any skill
- Practical maintenance requires focused effort
- Breadth vs. depth becomes player choice
- Time investment naturally enforces specialization

**Player Psychology:**
- More engaging than hard arbitrary limits
- Feels fair: decay is consequence of choice
- Provides agency: players control which skills to maintain
- Allows experimentation without permanent commitment

## Specialization vs Generalization in Practice

### The Balance Question

With level-based category caps and skill decay mechanics in place, is an additional global cap needed?

**Analysis: Probably Not**

The combination of mechanics already serves to prevent "max everything" builds:

1. **Level-Based Caps:** Limit total potential per category
2. **Skill Decay:** Create maintenance costs for breadth
3. **Time Investment:** Natural resource constraint
4. **Opportunity Costs:** Practice time is finite

### Risk of Over-Restriction

**Potential Issues with Additional Caps:**

**Player Experience:**
- Too many limitations feel constraining
- Reduces player agency and experimentation
- May frustrate rather than challenge
- Limits creative problem-solving

**System Complexity:**
- More rules to understand and track
- Increased cognitive load for players
- Additional balancing complexity
- Potential for confusing interactions

**Natural vs. Artificial Constraints:**
- Decay feels like natural consequence
- Hard caps feel arbitrary and punitive
- Players prefer logical limitations
- Emergent specialization more satisfying than forced

### Appeal of Skill-Based Systems

**Player Freedom:**
- Build characters as desired
- Experiment with different combinations
- Adapt to changing gameplay needs
- Create unique character identities

**Decay as Gentle Nudge:**
- Encourages specialization naturally
- Doesn't block experimentation
- Provides consequences without hard stops
- Balances freedom with meaningful choices

## Design Analysis and Recommendations

### System Summary

The analyzed system is a **hybrid approach** combining:

1. **Experience levels** expand skill capacity per category
2. **Automatic skill growth** through usage (learn by doing)
3. **Skill decay** through disuse (use it or lose it)
4. **Level-based category caps** scale with character advancement

### Core Recommendation: No Additional Caps Needed

**Reasoning:**

The combination of existing mechanics functions as a **self-balancing ecosystem**:

- **Level caps** prevent early-game imbalance
- **Decay mechanics** prevent late-game universality
- **Usage-based progression** rewards active engagement
- **Natural constraints** feel fair and logical

**Additional global caps would be:**
- Redundant given category-based limits
- Potentially frustrating on top of decay
- Solving a problem already addressed
- Adding complexity without proportional benefit

### Alternative Approach: Hard Cap with Minimum Competence

While the previous recommendation suggests avoiding additional caps, an alternative design pattern exists that
uses a **total skill point cap** combined with **minimum competence thresholds**:

**System Design:**
```
Total Skill Points Available at Max Level:
- All skills at minimum (15 each): e.g., 10 skills × 15 = 150 points
- Maximum 2 skills at maximum (100 each): 200 points
- Extra points for distribution: 500 points
- Total Available: 850 points

Point Distribution Example:
- 2 Master Skills (100 each): 200 points
- 3 Expert Skills (75 each): 225 points
- 5 Proficient Skills (50 each): 250 points
- 5 Basic Skills (15 each): 75 points
- Remaining skills: unused (0 points)
Total: 750 points used, 100 points flexible
```

**Key Mechanics:**

**Minimum Competence Thresholds:**
- **Skill Level 15:** Minimum to attempt task, high failure rate (60-70%)
- **Skill Level 25:** Basic competence, moderate failure rate (40-50%)
- **Skill Level 50:** Proficient, low failure rate (15-25%)
- **Skill Level 75:** Expert, very low failure rate (5-10%)
- **Skill Level 100:** Master, minimal failure rate (1-2%)

**Skill Decay to Minimums:**
- Skills decay toward their maintained minimum level
- If a skill has been trained to 15+, it decays to 15 (not to 0)
- Players maintain permanent capability at minimum competence
- Skills above maintenance threshold decay faster

**Build Archetype Examples:**

**Specialist Build (Deep Expertise):**
```
Mining: 100 (Master)
Geology: 100 (Master)
Surveying: 75 (Expert)
Processing: 50 (Proficient)
Construction: 25 (Basic)
All other skills: 15 (Minimal) if trained, 0 if never used
Total: ~530 points + 15 per additional trained skill
```

**Generalist Build (Broad Capability):**
```
8 skills at 75 (Expert): 600 points
5 skills at 50 (Proficient): 250 points
All skills accessible at minimum (15) after any training
Total: 850 points = all allocation used
```

**Advantages of This System:**

1. **Clear Specialization Pressure:** Hard cap forces meaningful choices
2. **Minimum Competence Safety Net:** Can attempt any trained task, just with higher failure
3. **Flexible Build Diversity:** Supports both specialists and generalists
4. **Predictable Progression:** Players can plan builds knowing total points available
5. **Failure as Gameplay:** Minimum skill attempts create tension and challenge

**Disadvantages:**

1. **More Restrictive:** Less freedom than pure decay-based systems
2. **Requires Careful Balancing:** Point costs must be tuned precisely
3. **May Discourage Experimentation:** Fear of "wasting" points on wrong skills
4. **Build Lock-in:** Harder to change specialization mid-game

**Design Considerations:**

**Skill Reset Mechanics:**
- Allow periodic skill reallocation (e.g., once per level bracket)
- Decay naturally reallocates points by reducing unused skills
- Consider skill "respec" items or services

**Minimum Failure Rates:**
- Ensure minimum-skill attempts are viable but risky
- Create interesting gameplay through skill checks
- Reward specialization without hard-blocking generalists

**Point Economy Tuning:**
- Balance total points to allow 2 masters + meaningful breadth
- Ensure marginal points in many skills vs. deep specialization both viable
- Consider diminishing returns at high skill levels (80-100 costs more)

**Integration with Decay:**
- Decay primarily affects skills above maintenance threshold
- Minimum competence (15) acts as automatic decay floor for trained skills
- Players maintain basic capability in all attempted skills permanently

### Hierarchical Skill Tree Structures

Modern skill systems often implement **hierarchical skill trees** with multiple levels of granularity, allowing for
both broad competence and specialized expertise within domains.

**Hierarchical Structure Pattern:**
```
Skill Tree (Category) → Skill Group/Material Group → Specific Skill/Material/Item

Examples:
Combat → One-Hand → Axe
Combat → One-Hand → Dagger
Combat → One-Hand → Sword
Combat → Two-Hand → Greatsword
Combat → Ranged → Bow

Gathering → Metal → Iron
Gathering → Metal → Copper
Gathering → Metal → Gold
Gathering → Herbs → Chamomile
Gathering → Herbs → Lavender
Gathering → Mushrooms → Pholiota Squarrosa
Gathering → Mushrooms → Chanterelle

Crafting → Woodworking → Building → House
Crafting → Woodworking → Building → Fence
Crafting → Woodworking → Furniture → Chair
Crafting → Blacksmithing → Weapons → Sword
Crafting → Blacksmithing → Armor → Breastplate
```

**Skill Point Allocation in Hierarchical Systems:**

**Method 1: Aggregate Points (Total Category)**
```
Total points in category = sum of all sub-skills
Example:
- Combat Category: 200 points total
  - One-Hand Group: 120 points
    - Axe: 40 points
    - Dagger: 30 points
    - Sword: 50 points
  - Two-Hand Group: 50 points
    - Greatsword: 50 points
  - Ranged Group: 30 points
    - Bow: 30 points
```

**Method 2: Shared Group Points (Efficiency Bonus)**
```
Group level provides base competence, specific skills add bonuses
Example:
- One-Hand Weapons: 60 points (applies to all one-hand weapons)
  - Axe Specialization: +25 points (total 85 for axes)
  - Sword Specialization: +40 points (total 100 for swords)
  - Dagger: 60 (no specialization, uses group level only)

Benefits:
- Encourages exploration within groups
- Specialization still rewarded
- Reduces point inflation
- More intuitive progression
```

**Method 3: Independent Tracks (Maximum Granularity)**
```
Each specific skill tracked separately, no aggregation
Example:
- Iron Mining: 75 points
- Copper Mining: 45 points
- Gold Mining: 30 points
- Chamomile Gathering: 60 points
- Lavender Gathering: 40 points

Considerations:
- Highest detail and control
- Can lead to many skills to manage
- Best for material-focused games
- Works well with minimum competence (15) system
```

**Application to BlueMarble Geological Skills:**

**Hierarchical Geological Skill Tree:**
```
Geological Sciences (Category)
├── Petrology (Skill Group)
│   ├── Igneous Rock Analysis (Specific)
│   ├── Sedimentary Rock Analysis (Specific)
│   └── Metamorphic Rock Analysis (Specific)
├── Mineralogy (Skill Group)
│   ├── Silicate Identification (Specific)
│   ├── Carbonate Identification (Specific)
│   └── Ore Mineral Identification (Specific)
└── Structural Geology (Skill Group)
    ├── Fault Analysis (Specific)
    ├── Fold Analysis (Specific)
    └── Stratigraphic Analysis (Specific)

Extraction Skills (Category)
├── Mining (Skill Group)
│   ├── Metal Ore Mining (Specific)
│   │   ├── Iron Ore: individual material skill
│   │   ├── Copper Ore: individual material skill
│   │   └── Gold Ore: individual material skill
│   ├── Gemstone Mining (Specific)
│   └── Coal Mining (Specific)
├── Surveying (Skill Group)
│   ├── Surface Surveying (Specific)
│   ├── Subsurface Surveying (Specific)
│   └── Geophysical Surveying (Specific)
└── Prospecting (Skill Group)
    ├── Visual Prospecting (Specific)
    ├── Sample Analysis (Specific)
    └── Deposit Estimation (Specific)

Crafting (Category)
├── Woodworking (Skill Group)
│   ├── Building (Specific)
│   │   ├── House: individual item skill
│   │   ├── Fence: individual item skill
│   │   └── Bridge: individual item skill
│   └── Furniture (Specific)
│       ├── Chair: individual item skill
│       └── Table: individual item skill
└── Metalworking (Skill Group)
    ├── Tool Crafting (Specific)
    └── Structural Components (Specific)
```

**Point Distribution in Hierarchical System:**

**Option A: Shared Group Points (Recommended)**
```
Player invests 80 points in "Mining" group:
- Can mine any ore at 80 skill level
- Additional 20 points in "Iron Ore" specialization
- Iron Ore effective skill: 100 (Master)
- Copper Ore effective skill: 80 (Expert from group)
- Gold Ore effective skill: 80 (Expert from group)

Advantages:
- Encourages trying different materials within group
- Specialization still meaningful
- Reduces micromanagement
- Natural learning curve (general → specific)
```

**Option B: Material Familiarity (Hybrid)**
```
Mining skill: 80 points (general technique)
Material Familiarity (separate track):
- Iron Ore: 100 uses (familiar: +10 bonus) = effective 90
- Copper Ore: 20 uses (learning: +2 bonus) = effective 82
- Gold Ore: 5 uses (novice: +0 bonus) = effective 80

Total effective skills:
- Iron: 90 (specialized through practice)
- Copper: 82 (some experience)
- Gold: 80 (baseline from Mining skill)

Advantages:
- Realistic learning model
- Practice matters beyond points
- Encourages diverse experience
- Compatible with existing systems
```

**Integration with Hard Cap System:**

With 925 total points and hierarchical skills:

**Scenario 1: Deep Specialization**
```
Mining Group: 100 points (master general mining)
├── Iron Ore Specialization: +20 → Iron at 120 (capped at 100, so 100)
├── Copper Ore: 100 (from group)
└── Gold Ore: 100 (from group)

Petrology Group: 100 points (master rock analysis)
├── Igneous Analysis: 100 (from group)
├── Sedimentary Analysis: 100 (from group)  
└── Metamorphic Analysis: 100 (from group)

Other skills: Distributed among remaining ~725 points

Benefits:
- Master level in all materials within mastered groups
- High efficiency across entire group
- Natural expertise spread within domain
```

**Scenario 2: Selective Specialization**
```
Mining Group: 50 points (proficient general mining)
├── Iron Ore Specialization: +50 → Iron at 100 (Master)
├── Copper Ore: 50 (Proficient from group)
└── Gold Ore: 50 (Proficient from group)

Petrology Group: 50 points (proficient analysis)
├── Igneous Specialization: +50 → Igneous at 100 (Master)
├── Sedimentary: 50 (Proficient from group)
└── Metamorphic: 50 (Proficient from group)

Benefits:
- Two true masteries (Iron, Igneous)
- Proficient in related materials
- More points for breadth in other categories
```

**Decay Considerations for Hierarchical Skills:**

**Group-Level Decay:**
- Group skill decays like any other skill
- Specific specializations decay independently
- Using any material in group slows group decay
- Specialization bonuses decay faster (specific practice needed)

**Example:**
```
Player has Mining: 80, Iron Specialization: +20
After 3 months without mining:
- Mining group decays: 80 → 70 (general skill rusty)
- Iron specialization decays: +20 → +10 (specific practice lost)
- Iron effective: 80 (still proficient, lost master edge)
- Other ores: 70 (baseline from group decay)

To recover:
- Mine any ore: restores Mining group skill
- Mine specifically iron: restores Iron specialization bonus
```

**Design Recommendations for Hierarchical Systems:**

1. **Use Shared Group Points** for categories with many similar items (ores, herbs, wood types)
2. **Use Independent Skills** for distinctly different techniques (different weapon types, different crafts)
3. **Implement Material Familiarity** as bonus system on top of group skills for realistic progression
4. **Cap Specialization Bonuses** to avoid exponential growth (e.g., +25 max bonus)
5. **Show Clear Hierarchies** in UI so players understand point allocation
6. **Allow Group Practice** to maintain baseline competence across all group members

### Tag-Based Skill System (Alternative to Hierarchical)

An alternative to hierarchical skill trees is a **tag-based system** where skills are defined by multiple
independent tags rather than a parent-child hierarchy. Each activity combines tags from different dimensions to
determine skill application.

**Tag-Based System Design:**

Instead of: `Gathering → Metal Ores → Iron` (hierarchical)  
Use: `[gathering] + [metal] + [iron ore]` (tag-based)

**Tag Category Structure:**

**1. Domain Tags (Area of Use)**
```
combat, gathering, crafting, building, survival, trading, magic, social
```
Defines the broad gameplay domain the skill belongs to.

**2. Discipline / Activity Tags (Skill Group)**
```
one-hand, two-hand, ranged, unarmed
woodworking, mining, farming, tailoring, fishing, cooking
blacksmithing, alchemy, engineering, construction
navigation, speechcraft, thievery, healing
```
Defines the specific technique or craft being performed.

**3. Action Tags (Type of Activity)**
```
attack, defend, parry, dodge
harvest, extract, collect, gather
construct, build, carve, shape
weave, brew, cook, forge, smelt
hunt, tame, fish, farm
```
Defines the specific action being performed.

**4. Material Group Tags**

**Organic Materials:**
```
wood, herb, mushroom, fiber, leather, pelt, food, supplement, flora, fauna
```

**Anorganic Materials:**
```
metal, stone, clay, mineral, gem, salt, liquid, fuel, ore, crystal
```

**5. Exact Material Tags (Specific Resources)**

**Organic Examples:**
```
pine-wood, oak-wood, birch-wood
chamomile, mint, lavender, rosemary
chanterelle, porcini, morel
hemp-fiber, cotton, wool
deer-pelt, bear-pelt
```

**Anorganic Examples:**
```
iron-ore, copper-ore, silver-ore, gold-ore, tin-ore
granite, marble, limestone, basalt
quartz, feldspar, mica
ruby, sapphire, emerald, diamond
clear-water, salt-water, oil
charcoal, coal, peat
```

**Skill Point Allocation in Tag-Based Systems:**

**Method 1: Skill Points Per Tag**
```
Each tag has its own skill level:
[gathering]: 75 points
[metal]: 60 points
[iron-ore]: 40 points

Combined Effective Skill:
Iron ore gathering = avg([gathering: 75], [metal: 60], [iron-ore: 40]) = 58.3
Weighted: gathering(50%) + metal(30%) + iron-ore(20%) = 64.5

Benefits:
- Skills transfer across contexts (gathering skill helps with all gathering)
- Specialization in specific materials still rewarded
- Natural breadth with specialization
```

**Method 2: Tag Combination Points**
```
Allocate points to specific tag combinations:
[gathering + metal + iron-ore]: 80 points
[gathering + metal + copper-ore]: 50 points
[gathering + herb + chamomile]: 30 points

Benefits:
- Direct control over specific activities
- Clear understanding of capabilities
- Similar to independent hierarchical tracks
```

**Method 3: Weighted Tag System**
```
Domain weight: 40%
Discipline weight: 30%
Material group weight: 20%
Exact material weight: 10%

Example:
[gathering]: 80 points
[mining]: 70 points
[metal]: 60 points
[iron-ore]: 90 points

Iron ore mining skill = (80×0.4) + (70×0.3) + (60×0.2) + (90×0.1) = 74 effective
Gold ore mining skill = (80×0.4) + (70×0.3) + (60×0.2) + (20×0.1) = 67 effective

Benefits:
- High granularity with manageable skill counts
- Specialization emerges from material-specific practice
- Domain expertise provides solid baseline
```

**BlueMarble Tag-Based Implementation:**

**Example Activities with Tags:**

```
Activity: Mining iron ore
Tags: [gathering] + [mining] + [metal] + [iron-ore]
Skill calculation: Weighted average or combination

Activity: Harvesting chamomile
Tags: [gathering] + [herb] + [chamomile]
Skill calculation: Based on relevant tag levels

Activity: Carving oak chair
Tags: [crafting] + [woodworking] + [carve] + [wood] + [oak-wood]
Skill calculation: Multi-tag combination

Activity: Attacking with iron sword
Tags: [combat] + [one-hand] + [attack] + [metal] + [iron]
Skill calculation: Combat + weapon + material factors

Activity: Cooking deer meat stew
Tags: [survival] + [cooking] + [cook] + [food] + [deer-meat]
Skill calculation: Cooking skill + ingredient familiarity
```

**Skill Budget with Tag System:**

With 23 domain-level skills and tag-based sub-skills:

```
Domain Skills (23): 345 points minimum (15 each)
Discipline Tags (~30): Points distributed as needed
Material Groups (~15): Points distributed as needed
Exact Materials (~100+): Minimal points, gained through practice

Total System:
- 2 Master domain skills: 200 points
- 5 Expert disciplines: 375 points (75 each)
- 10 Proficient material groups: 500 points (50 each)
- Exact materials: Familiarity through use (no point cost)

Total: 1,075 points for explicit allocation
Exact materials gained automatically through practice
```

**Advantages of Tag-Based System:**

1. **Flexible Organization**: Skills aren't locked into single hierarchies
2. **Natural Skill Transfer**: Domain skills apply broadly (gathering helps all gathering)
3. **Emergent Specialization**: Material familiarity emerges from repeated use
4. **Reduced Micromanagement**: Don't need to track every possible combination
5. **Realistic Learning**: Using oak teaches you about wood in general
6. **Cross-Domain Skills**: Tags can combine across domains (combat + magic + fire)
7. **Easy Expansion**: Add new materials without restructuring skill trees

**Disadvantages:**

1. **Complex Calculation**: Need algorithm to combine multiple tag values
2. **UI Challenge**: Showing how tags affect different activities
3. **Balance Difficulty**: Many tag interactions to tune
4. **Player Understanding**: Less intuitive than simple trees initially
5. **Database Complexity**: More tags to store and query

**Decay in Tag-Based Systems:**

**Tag-Level Decay:**
```
Domain tags decay slowly (broad skills)
Discipline tags decay moderately (specific techniques)
Material group tags decay moderately (categorical knowledge)
Exact material tags decay quickly (specific familiarity)

Example after 2 months without mining:
[gathering]: 80 → 75 (domain decays slowly)
[mining]: 70 → 60 (discipline decays moderately)
[metal]: 60 → 55 (material group decays moderately)
[iron-ore]: 90 → 40 (exact material decays quickly)

Result: Iron mining skill drops from 74 to 62
Can still mine iron competently (62) but lost mastery edge
Other metal ores still at decent level due to [metal] retention
```

**Design Recommendations for Tag-Based Systems:**

1. **Use Domain + Discipline as Primary**: These provide the skill baseline
2. **Material Groups as Modifiers**: Boost effectiveness within material categories
3. **Exact Materials as Familiarity**: Automatic tracking, small bonuses
4. **Clear Tag Weights**: Document how tags combine for each activity type
5. **Practice-Based Material Learning**: Don't require point investment in specific materials
6. **UI Feedback**: Show which tags are improving during activities
7. **Tag Caps**: Limit how much exact material specialization can exceed domain baseline

**Comparison: Hierarchical vs. Tag-Based**

| Aspect | Hierarchical | Tag-Based |
|--------|-------------|-----------|
| Organization | Parent → Child tree | Independent tag dimensions |
| Skill Transfer | Within branch only | Across all matching tags |
| Specialization | Clear path down tree | Emergent from tag combinations |
| UI Complexity | Moderate (tree view) | High (multi-dimensional) |
| Calculation | Simple (sum/average branch) | Complex (weighted combination) |
| Flexibility | Fixed relationships | Dynamic combinations |
| Expansion | Add to existing branches | Add new tags freely |
| Player Understanding | Intuitive | Requires learning |
| Best For | Traditional RPGs | Simulation-heavy games |

**Recommendation for BlueMarble:**

Given BlueMarble's simulation focus on realistic geological processes, a **tag-based system** may be more
appropriate than hierarchical trees. The flexibility allows materials, tools, and techniques to interact in
realistic ways without artificial hierarchical constraints.

Suggested implementation:
- **Domain tags (23)**: Explicit point allocation (combat, gathering, mining, etc.)
- **Discipline tags (~30)**: Explicit point allocation (one-hand, woodworking, etc.)
- **Material groups (~15)**: Moderate point allocation or practice-based
- **Exact materials (100+)**: Automatic familiarity through use, no point cost
- **Action tags**: Derived from domain/discipline, no separate tracking

This provides depth without overwhelming micromanagement, and supports BlueMarble's goal of realistic geological
interaction where knowledge and skills transfer naturally across similar contexts.

### Practical Tag-Based Crafting Calculations

To make the tag-based system concrete and repeatable, here's a detailed calculation model using rope-making as an
example. This provides a template for implementing crafting systems with tags.

**1. Tag Categories for Crafting**

For crafting an item (e.g., hemp rope), these tag categories apply:

```
domain: crafting (general craftsmanship)
discipline: fiberwork / weaving / rope-making (primary skill)
action: construct / weave (action type)
material_group: organic/fiber (material category)
material: hemp-fiber (specific material quality)
tool: ropewalk / spindle (tool proficiency and bonuses)
```

**2. Calculation Model**

**Core Formula:**
```
Quality Score = Σ(skill_level × weight) + material_quality × material_weight + tool_bonus

Success Chance = clamp((Quality Score / Recipe Complexity) × 100%, 0%, 100%)

Quality Tier = determined by Quality Score vs Recipe Complexity thresholds
```

**Quality Tiers (relative to Recipe Complexity C):**
```
Masterwork:  Quality Score ≥ 1.5 × C
Excellent:   Quality Score ≥ 1.2 × C
Good:        Quality Score ≥ 0.8 × C
Common:      Quality Score ≥ 0.5 × C
Poor:        Quality Score < 0.5 × C
```

**3. Recommended Weights for Rope-Making**

These weights can be adjusted based on game balance:

```
Primary discipline (rope-making/weaving):        0.40
Fiber processing (fiberwork/material handling):  0.25
General crafting skill (crafting):               0.10
Tool proficiency (tool):                         0.15
Material quality (material):                     0.10
Total:                                           1.00
```

**4. Concrete Example: Hemp Rope Crafting**

**Recipe Parameters:**
```
Item: Basic Hemp Rope
Recipe Complexity (C): 60 (medium difficulty)
```

**Character Skills/States:**
```
rope-making: 60
fiberwork: 40
crafting: 50
tool proficiency (ropewalk): 30
material quality (hemp fiber): 80 (high quality fiber, 0-100 scale)
```

**Weighted Calculation:**
```
rope-making contribution:    60 × 0.40 = 24.0
fiberwork contribution:      40 × 0.25 = 10.0
crafting contribution:       50 × 0.10 =  5.0
tool proficiency:            30 × 0.15 =  4.5
material quality:            80 × 0.10 =  8.0
                             _______________
Quality Score:                           51.5
```

**Success Determination:**
```
Success Chance = (51.5 / 60) × 100% = 85.83%
```
High chance of success! Player has 85.83% chance to craft the rope.

**Quality Tier Determination:**

Compare Quality Score (51.5) to thresholds for C=60:
```
Masterwork threshold: 1.5 × 60 = 90  → NO (51.5 < 90)
Excellent threshold:  1.2 × 60 = 72  → NO (51.5 < 72)
Good threshold:       0.8 × 60 = 48  → YES (51.5 ≥ 48)
Common threshold:     0.5 × 60 = 30  → YES (51.5 ≥ 30)

Result: Good Quality Hemp Rope (if success roll passes)
```

**Failure Handling:**

If the 14.17% failure chance occurs:
- **Partial Success**: Produce Poor quality rope (reduced durability/strength)
- **Material Loss**: Consume materials with partial return (e.g., 25% recovered)
- **Critical Failure** (optional 5% of failures): Tool damage, complete material loss

**5. Critical Success/Failure Mechanics**

**Critical Success** (optional, 5% above success threshold or Quality Score ≫ C):
- Enhanced product: +10% durability, +5% strength
- Bonus: Extra quantity (e.g., 10% more rope length)
- Experience: Double skill gain from crafting

**Critical Failure** (when Quality Score < 0.2 × C or critical roll):
- Broken rope during crafting
- 100% material loss
- Possible tool damage (10% durability loss)
- Negative experience: Small skill loss to relevant tags

**6. Factors Affecting Results**

**Material Quality Impact:**
```
Poor hemp fiber (quality 20):  -6.0 points to Quality Score
Average fiber (quality 50):    -3.0 points to Quality Score
High quality fiber (quality 80): +8.0 points to Quality Score (as shown)
Exceptional fiber (quality 95): +9.5 points to Quality Score
```

**Tool Bonuses:**
```
No tool (hands only):          -4.5 points (tool proficiency × weight)
Basic spindle (quality 30):     +4.5 points (as shown)
Quality ropewalk (quality 60):  +9.0 points
Master ropewalk (quality 90):  +13.5 points
```

**Skill Synergies & Perks:**
```
"Experienced Ropemaker" perk:  +10% to primary discipline (rope-making +6 effective)
"Material Expert" perk:        +15 to material quality effectiveness
"Tool Master" perk:            +20% to tool proficiency contribution
```

**Diminishing Returns** (optional balance mechanism):
```
Skills above 90: Effective value = 90 + (actual - 90) × 0.5
Example: rope-making 95 → effective 92.5 for calculation
Prevents extreme min-maxing while rewarding mastery
```

**7. Recipe Data Structure**

**JSON Schema Example:**
```json
{
  "name": "Basic Hemp Rope",
  "complexity": 60,
  "required_tags": {
    "domain": ["crafting"],
    "discipline": ["fiberwork", "weaving", "rope-making"],
    "material_group": ["organic", "fiber"],
    "material": ["hemp-fiber", "flax-fiber"]
  },
  "material_requirements": [
    {"tag": "material:hemp-fiber", "amount": 5, "unit": "bundles"}
  ],
  "weights": {
    "primary_discipline": 0.40,
    "secondary_discipline": 0.25,
    "domain": 0.10,
    "tool": 0.15,
    "material": 0.10
  },
  "output": {
    "item": "hemp_rope",
    "base_quantity": 10,
    "unit": "meters"
  },
  "time": {
    "base_seconds": 300,
    "skill_reduction": "0.5% per point above 50"
  },
  "failure_mechanics": {
    "on_fail": "produce_poor_quality_or_consume_materials",
    "material_recovery": 0.25,
    "crit_fail_chance": 0.05,
    "crit_success_chance": 0.05
  },
  "experience_gain": {
    "rope-making": 15,
    "fiberwork": 10,
    "crafting": 5
  }
}
```

**8. Multi-Material Recipes**

For recipes requiring multiple materials:

**Example: Reinforced Rope (hemp + leather strips)**
```
Material Quality Calculation:
hemp_fiber (quality 80): 80 × 0.07 = 5.6
leather_strips (quality 60): 60 × 0.03 = 1.8
Combined material contribution: 5.6 + 1.8 = 7.4

Weights adjusted:
primary discipline: 0.40
fiberwork: 0.20
leatherworking: 0.10
crafting: 0.10
tool: 0.10
materials: 0.10 (split between materials)
```

**9. Scaling Complexity**

**Easy Recipes** (C = 20-40):
- Simple twine, basic cordage
- Low material waste on failure
- Common/Good quality achievable by novices

**Medium Recipes** (C = 50-70):
- Standard rope, nets, basic textiles
- Moderate skill requirement
- Good/Excellent quality requires competence

**Hard Recipes** (C = 80-100):
- Specialized rigging, heavy-duty cables
- High skill requirement
- Excellent/Masterwork only for masters

**Expert Recipes** (C = 120+):
- Ceremonial textiles, precision instruments
- Multiple rare materials
- Masterwork requires near-perfect conditions

**10. Implementation Recommendations**

**Balance Guidelines:**
1. **Weight Distribution**: Primary discipline 35-45%, avoid any tag > 50%
2. **Success Rate**: Target 70-90% for medium recipes at appropriate skill levels
3. **Critical Rates**: Keep crit success/fail at 5% base, modified by extreme Quality Scores
4. **Material Recovery**: 20-30% on failure prevents excessive frustration
5. **Complexity Scaling**: Roughly C = (skill_requirement + 10) for balanced challenge

**Performance Optimization:**
- Cache tag lookups for recipes
- Pre-calculate common weight combinations
- Use lookup tables for quality tier thresholds
- Batch calculate for production queues

**Player Feedback:**
- Show which tags contribute to success
- Display quality tier probabilities before crafting
- Highlight material quality impact
- Suggest skill improvements for better results

**Extensibility:**
- Add environmental factors (weather, location bonuses)
- Include character states (hunger, fatigue reducing effectiveness)
- Support recipe variations (same output, different materials/methods)
- Enable mastery bonuses for frequently crafted items

**Example UI Flow:**
```
Player selects recipe: "Basic Hemp Rope"
System displays:
├─ Success Chance: 85.8%
├─ Expected Quality: Good (51.5/60)
├─ Time Required: 4m 15s
├─ Contributing Skills:
│  ├─ Rope-making (60): +24.0 [Primary]
│  ├─ Fiberwork (40): +10.0
│  ├─ Crafting (50): +5.0
│  └─ Tool (Ropewalk 30): +4.5
├─ Material Impact:
│  └─ Hemp Fiber (Quality 80): +8.0 [High Quality]
└─ Possible Outcomes:
   ├─ Good Quality: 85.8% (current result)
   ├─ Excellent: Impossible (need Quality Score 72+)
   └─ Masterwork: Impossible (need Quality Score 90+)

Suggestions for improvement:
- Increase rope-making to 75 for Excellent chance
- Use Exceptional hemp fiber (95) for +1.5 bonus
- Upgrade to Quality Ropewalk for +4.5 bonus
```

This practical implementation provides a complete, repeatable system for calculating crafting outcomes using the
tag-based skill system. The weighted approach ensures all relevant skills contribute proportionally while
maintaining clear success/failure mechanics and quality tiers.

### Focus on Tuning, Not New Restrictions

Instead of adding new cap systems, optimize existing mechanics:

#### Decay Rate Tuning

**Considerations:**
- Decay shouldn't feel punitive for normal gameplay patterns
- Taking breaks for story content shouldn't severely penalize
- High-level skills should decay slower (harder to forget mastery)
- Low-level skills can decay faster (easier to forget basics)

**Suggested Approach:**
```
Decay Rate Factors:
- Skill Level: Higher = slower decay
- Skill Type: Some skills more "sticky" than others
- Grace Period: Short breaks don't trigger decay
- Decay Curve: Exponential or logarithmic, not linear
```

#### Grace Period Implementation

**Design Pattern:**
```
Last Used Time → Grace Period → Decay Begins
Grace Period Duration:
- Novice Skills (1-25): 1 week real-time
- Journeyman Skills (26-50): 2 weeks real-time
- Expert Skills (51-75): 3 weeks real-time
- Master Skills (76-100): 4 weeks real-time
```

**Benefits:**
- Casual players not excessively punished
- Encourages regular engagement without anxiety
- Allows vacation breaks without major consequences
- Differentiates commitment by skill tier

#### Decay Floor Calibration

**Suggested Thresholds:**
```
Skill Tier → Decay Floor
Novice (1-25) → Decays to 0 (can forget basics)
Journeyman (26-50) → Decays to 10 (retains fundamentals)
Expert (51-75) → Decays to 25 (maintains proficiency)
Master (76-100) → Decays to 50 (expert baseline)
```

**Philosophy:**
- Once mastered, never completely forgotten
- Investment in high skills has lasting value
- Encourages reaching higher tiers for permanence
- Balances accessibility with meaningful progression

### Player Experience Priorities

**Design Goals:**

1. **Feel Fair:** Consequences should feel earned, not arbitrary
2. **Encourage Engagement:** Reward regular play without anxiety
3. **Allow Flexibility:** Permit experimentation and adaptation
4. **Promote Identity:** Foster specialization through natural selection
5. **Maintain Fun:** Balance realism with enjoyment

**Warning Signs of Poor Tuning:**
- Players feel punished for taking breaks
- Anxiety about skill loss overshadows enjoyment
- Optimal strategy is tedious maintenance grinding
- System discourages trying new content

## Integration with BlueMarble Systems

### Alignment with Geological Progression

BlueMarble's skill system should reflect geological expertise development:

**Skill Categories for BlueMarble:**
1. **Geological Sciences:** Field identification, analysis, theory
2. **Extraction Skills:** Mining, surveying, prospecting
3. **Processing Skills:** Refining, material preparation, analysis
4. **Construction Skills:** Engineering, building, infrastructure
5. **Social Skills:** Trading, teaching, collaboration

**Application of Cap and Decay Mechanics:**

**Level-Based Caps:**
```
Character Level → Category Capacity
Level 1-10: Foundation phase (30 points per category)
Level 11-25: Specialization emerges (75 points per category)
Level 26-50: Expert development (150 points per category)
Level 51-100: Master refinement (250 points per category)
```

**Decay Implementation:**
```
Geological Sciences:
- Theory skills decay slower (knowledge-based)
- Practical skills decay faster (hands-on)
- Decay floor at 25% of peak (never forget fundamentals)

Extraction Skills:
- Equipment operation decays moderately
- Technique identification decays slowly
- Efficiency decays faster (muscle memory)

Processing Skills:
- Process knowledge decays slowly
- Manual dexterity decays moderately
- Quality assessment decays slowly

Construction Skills:
- Design principles decay slowly
- Construction techniques decay moderately
- Tool proficiency decays faster

Social Skills:
- Relationship building doesn't decay (permanent)
- Trading expertise decays slowly
- Teaching ability decays slowly
```

### Knowledge System Integration

**Knowledge vs. Skills:**
- **Knowledge:** Unlocks capabilities, doesn't decay
- **Skills:** Execution efficiency, can decay
- **Synergy:** Knowledge preserves decay floor

**Example:**
```
Geological Knowledge: "Sedimentary Rock Formation"
- Once discovered: Permanent
- Enables: Sedimentary rock identification skill
- Skill Execution: Decays if unused
- Decay Floor: Higher with more related knowledge
```

**Benefits:**
- Knowledge investment has permanent value
- Skills require maintenance but build on knowledge
- Encourages both learning and practice
- Creates natural progression: learn → practice → master → maintain

### Player Specialization Paths

**Emergent Roles:**

**The Geologist Specialist:**
- Focuses on Geological Sciences category
- Maintains 3-4 core geology skills at mastery
- Other skills maintained at journeyman for utility
- Knowledge-focused, slow skill decay

**The Master Miner:**
- Focuses on Extraction Skills category
- Maintains mining and surveying at peak
- Lets processing skills decay to expert floor
- Action-focused, requires regular engagement

**The Civil Engineer:**
- Focuses on Construction Skills category
- Maintains structural and civil engineering
- Keeps geological and extraction at proficient level
- Balanced maintenance requirements

**The Trading Master:**
- Focuses on Social Skills category
- Maintains trading and collaboration at peak
- Geological knowledge without peak skills
- Low maintenance (social skills decay slowly)

### BlueMarble Comprehensive Skill List

Based on the planned game design, BlueMarble will feature **23+ skill categories** covering diverse gameplay
activities. This comprehensive skill list demonstrates the scale and scope of the progression system:

**Complete Skill Categories:**
1. **Combat** - Fighting and combat techniques
2. **Gathering** - Resource collection and harvesting
3. **Woodworking** - Wood processing and carpentry
4. **Blacksmithing** - Metal working and weapon/tool crafting
5. **Alchemy** - Potion brewing and chemical processing
6. **Cooking** - Food preparation and recipes
7. **Fishing** - Catching fish and aquatic resources
8. **Hunting** - Tracking and harvesting game
9. **Farming** - Agriculture and crop cultivation
10. **Mining** - Ore extraction and tunneling
11. **Smithing** - General metalworking
12. **Tailoring** - Fabric working and clothing
13. **Leatherworking** - Leather processing and goods
14. **Trading** - Commerce and negotiation
15. **Navigation** - Wayfinding and travel
16. **Survival** - Wilderness skills and endurance
17. **Magic** - Arcane abilities and spells
18. **Healing** - Medical treatment and restoration
19. **Thievery** - Stealth and lockpicking
20. **Speechcraft** - Persuasion and social interaction
21. **Engineering** - Mechanical design and construction
22. **Building** - Structure construction and architecture
23. **Exploration** - Discovery and mapping

**Skill Budget Calculations with 23 Skills:**

With the hard cap system (2 masters + 500 flexible points + minimum 15):

```
Total Available Points at Max Level:
- All 23 skills at minimum (15 each): 23 × 15 = 345 points
- Maximum 2 skills at mastery (100 each): 200 points
- Flexible distribution pool: 500 points
- Total skill points available: 1,045 points

Point Distribution Examples:

Specialist Build (Deep Focus):
- 2 Master skills (100 each): 200 points
- 5 Expert skills (75 each): 375 points
- 8 Proficient skills (50 each): 400 points
- 8 Basic skills (15 each): 120 points
- Total: 1,095 points (adjust by reducing some proficient to basic)

Balanced Build (Moderate Breadth):
- 2 Master skills (100 each): 200 points
- 10 Proficient skills (50 each): 500 points
- 11 Basic skills (15 each): 165 points
- Total: 865 points (180 points for fine-tuning)

Generalist Build (Maximum Versatility):
- 0 Master skills
- 15 Expert skills (75 each): 1,125 points (too many, need to reduce)
- Adjusted: 13 Expert skills (75 each): 975 points
- 10 Basic skills (15 each): 150 points
- Total: 1,125 points (adjust by reducing 1-2 experts)
```

**Practical Build Example: Geological Specialist**
```
Master Level (100 points each):
- Mining: 100
- Engineering: 100

Expert Level (75 points each):
- Blacksmithing: 75 (tool making)
- Building: 75 (infrastructure)
- Exploration: 75 (finding resources)

Proficient Level (50 points each):
- Gathering: 50 (general resources)
- Survival: 50 (field operations)
- Trading: 50 (selling minerals)
- Navigation: 50 (travel to sites)

Basic Level (15 each):
- All remaining 15 skills: 225 points

Total: 200 + 225 + 200 + 225 = 850 points
Remaining: 195 points for adjustments
```

**Hierarchical Organization of 23 Skills:**

Many of these top-level skills would contain hierarchical sub-skills:

```
Combat
├── Melee
│   ├── One-Hand
│   │   ├── Axe
│   │   ├── Sword
│   │   └── Dagger
│   └── Two-Hand
│       ├── Greatsword
│       └── Polearm
└── Ranged
    ├── Bow
    └── Crossbow

Gathering
├── Metal Ores
│   ├── Iron
│   ├── Copper
│   └── Gold
├── Herbs
│   ├── Chamomile
│   └── Lavender
└── Mushrooms
    ├── Pholiota Squarrosa
    └── Chanterelle

Woodworking
├── Building
│   ├── House
│   ├── Fence
│   └── Bridge
└── Furniture
    ├── Chair
    └── Table

Mining
├── Surface Mining
│   ├── Open Pit
│   └── Quarrying
├── Underground Mining
│   ├── Shaft Mining
│   └── Tunnel Mining
└── Specialized Extraction
    ├── Gem Mining
    └── Coal Mining

Engineering
├── Structural
│   ├── Foundations
│   └── Load Bearing
├── Mechanical
│   ├── Pulley Systems
│   └── Gears
└── Geological
    ├── Tunnel Support
    └── Drainage Systems
```

**Managing 23+ Skills with Decay:**

With this many skills, the decay system becomes crucial for forcing specialization:

**Maintenance Requirements:**
- **2 Master skills**: Practice 2-3 times per week to maintain peak
- **5 Expert skills**: Practice once per week minimum
- **8 Proficient skills**: Practice twice per month
- **8 Basic skills**: Practice once per month or accept decay to minimum (15)

**Time Investment (example gameplay hours per week):**
- Masters (2 skills): 6-8 hours/week
- Experts (5 skills): 5 hours/week
- Proficient (8 skills): 4 hours/week
- Basic (8 skills): 2 hours/week
- Total: ~17-19 hours/week active skill use

This creates natural specialization - players cannot maintain expertise in all 23 skills simultaneously.

**Skill Category Grouping for Hard Cap:**

To simplify tracking, the 23 skills might be organized into meta-categories:

**Production Skills (9):**
- Woodworking, Blacksmithing, Alchemy, Cooking, Smithing, Tailoring, Leatherworking, Engineering, Building

**Resource Skills (5):**
- Gathering, Fishing, Hunting, Farming, Mining

**Combat & Survival (4):**
- Combat, Survival, Thievery, Healing

**Social & Utility (5):**
- Trading, Speechcraft, Navigation, Exploration, Magic

With meta-categories, players might be limited to mastery in 2 individual skills but required to choose from
different meta-categories (e.g., can't master both Mining and Gathering, encouraging diverse builds).

### Applying Hard Cap Approach to BlueMarble

If BlueMarble adopts the **hard cap with minimum competence** system, the implementation would be:

**Total Skill Budget at Max Level:**
```
Geological Skill Categories (assume 15 total skills across 5 categories):
- Minimum competence (15 each): 15 × 15 = 225 points (all skills accessible)
- Maximum 2 skills at mastery (100 each): 200 points
- Flexible distribution pool: 500 points
- Total skill points available: 925 points

Point Distribution Strategy:
- 2 Master skills: 200 points
- 4 Expert skills (75 each): 300 points  
- 6 Proficient skills (50 each): 300 points
- 3 Basic skills (25 each): 75 points
- Total allocation: 875 points
```

**BlueMarble-Specific Skill Point Costs:**
```
Geological Sciences (3 skills):
- Petrology: 0-100 points
- Mineralogy: 0-100 points
- Structural Geology: 0-100 points

Extraction Skills (3 skills):
- Mining: 0-100 points
- Surveying: 0-100 points
- Prospecting: 0-100 points

Processing Skills (3 skills):
- Ore Refining: 0-100 points
- Material Analysis: 0-100 points
- Quality Assessment: 0-100 points

Construction Skills (3 skills):
- Geological Engineering: 0-100 points
- Mining Engineering: 0-100 points
- Infrastructure Development: 0-100 points

Social Skills (3 skills):
- Trading: 0-100 points
- Teaching: 0-100 points
- Collaboration: 0-100 points
```

**Minimum Competence Thresholds for Geological Tasks:**

**Mining at Different Skill Levels:**
- **15 (Minimum):** Can mine, 65% chance of breakage, 30% resource loss, slow speed
- **25 (Basic):** Improved technique, 45% breakage, 20% loss, moderate speed
- **50 (Proficient):** Competent mining, 20% breakage, 10% loss, good speed
- **75 (Expert):** Skilled mining, 8% breakage, 3% loss, fast speed
- **100 (Master):** Perfect technique, 1% breakage, 0% loss, maximum speed

**Geological Identification at Different Skill Levels:**
- **15 (Minimum):** Identify rock type category (sedimentary/igneous/metamorphic), 50% wrong subcategory
- **25 (Basic):** Identify common formations, 30% error on specific types
- **50 (Proficient):** Accurate identification of standard formations, 10% error on rare types
- **75 (Expert):** Expert identification, 3% error only on exotic formations
- **100 (Master):** Perfect identification, instant recognition, no errors

**Example Build: Specialized Geologist-Miner**
```
Mastery (100 points each):
- Petrology: 100 (Master geological analysis)
- Mining: 100 (Master extraction)

Expert (75 points each):
- Mineralogy: 75 (Expert identification)
- Surveying: 75 (Expert site analysis)
- Structural Geology: 75 (Expert formation understanding)

Proficient (50 points each):
- Mining Engineering: 50 (Proficient construction)
- Ore Refining: 50 (Proficient processing)

Basic Competence (15-25 points each):
- All remaining skills at 15-25 (can attempt, higher failure)

Total: ~725 points, 200 points for flexibility
```

**Gameplay Implications:**

**Risk vs. Reward:**
- Players can attempt any trained skill at minimum level
- Higher failure rates create tension and resource costs
- Specialization reduces waste and increases efficiency
- Generalists more versatile but less efficient

**Economic Impact:**
- Master craftsmen produce higher quality with less waste
- Minimum-skill attempts create demand for specialists
- Failed attempts create scarcity and value
- Trade economy benefits from specialization

**Team Dynamics:**
- Specialists highly valued for critical tasks
- Generalists useful for support and flexibility
- Knowledge sharing enables team capability growth
- Collaborative projects leverage diverse expertise

### Balancing for Geological Context

**Considerations:**

**Geological Timescales:**
- Real geology operates on long timescales
- Game mechanics need faster feedback
- Decay rates should feel geological but playable

**Scientific Knowledge:**
- Geological understanding shouldn't fully decay
- Theoretical knowledge more permanent
- Practical application requires maintenance

**Collaborative Gameplay:**
- Specialization enhances group play
- Knowledge sharing remains valuable
- Skill maintenance creates interdependence

**Educational Value:**
- System should encourage learning geology
- Knowledge investment rewarded permanently
- Skills represent applied understanding

## References and Sources

### Games Analyzed

**Skill Systems with Caps:**
- **Ultima Online:** 700-point total skill cap, enforcing 7 Grandmaster skills
- **Darkfall: Rise of Agon:** Use-based progression with soft caps
- **Wurm Online:** 130+ skills with soft caps and decay considerations
- **RimWorld:** Skill decay above threshold levels

**Design Documentation:**
- **D&D 3.5e Skills Summary** (d20 SRD): Skill rank maximum tied to character level (level +3 rule)
- **Darkfall: Rise of Agon** official documentation: Skills improve by using them

### Discussion Sources

**Community Analysis:**
- MMORPG skill system discussions: Necessity of limits to avoid universal masters
- RimWorld community: Skill decay purpose for realism and preventing omni-experts
- Wurm Online forums: Skill decay vs. skill caps and player perceptions

**Design Philosophy:**
- INTP Forum: Custom RPG with skill decay to minimum level, reflecting real-world learning/forgetting
- Game design discussions: Balancing player freedom with meaningful specialization

### Design Principles

**Core Concepts:**
- **Use-it-or-lose-it:** Skills require maintenance through practice
- **Specialization through scarcity:** Time as limiting resource
- **Natural consequences:** Decay as logical outcome, not arbitrary punishment
- **Baseline retention:** Never forget completely, reflecting real learning
- **Fair progression:** Earned advancement feels meaningful

---

**Related Documents:**
- [Skill and Knowledge System Research](skill-knowledge-system-research.md) - Parent research document
- [Skill System Child Research Issues](skill-system-child-research-issues.md) - Related research questions
- [Implementation Plan](implementation-plan.md) - Development timeline
- [Player Freedom Analysis](player-freedom-analysis.md) - Constraint frameworks
