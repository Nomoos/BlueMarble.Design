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

**Recommendations:**
- Focus on tuning existing decay rates rather than adding new cap systems
- Implement grace periods or slower decay for high-level skills
- Set decay floors to maintain baseline competence
- Balance decay severity with player enjoyment and accessibility
- Consider hard cap alternative: 2 master skills, 500 flexible points, minimum competence (15) for all trained
  skills with higher failure rates

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
