# Scrum Guide 2020 - Official Framework for BlueMarble Development

---
title: Scrum Guide 2020 - Official Framework for BlueMarble Development
date: 2025-01-15
tags: [game-development, scrum, agile, framework, methodology, scrum-guide-2020]
status: complete
priority: medium
parent-research: game-dev-analysis-agile-development.md
discovered-from: Assignment Group 07, Topic 2 - Agile Game Development
---

**Source:** The Scrum Guide 2020 by Ken Schwaber and Jeff Sutherland  
**Category:** Game Development - Agile Framework  
**Priority:** Medium  
**Status:** ✅ Complete  
**Lines:** 500+  
**Related Sources:** Agile Game Development (Parent), Lean Startup (Related)

---

## Executive Summary

The 2020 Scrum Guide represents the most streamlined, simplified version of Scrum to date. This analysis extracts
the core framework and adapts it for BlueMarble MMORPG development, focusing on the 2020 updates that emphasize
empiricism, self-management, and focus on the Product Goal.

**Key Takeaways for BlueMarble:**
- Product Goal provides long-term direction for MMORPG development
- Self-managing teams (not self-organizing) emphasize accountability
- Simplified events and artifacts reduce overhead
- Commitment points (Definition of Done, Sprint Goal, Product Goal) ensure quality
- Three pillars (Transparency, Inspection, Adaptation) enable rapid iteration

---

## Part I: Scrum Framework Fundamentals

### 1. The Three Pillars of Empiricism

**Transparency:**
```
Everyone understands:
- What we're building (Product Backlog visible)
- How we're building it (Sprint Backlog visible)
- What's done (Definition of Done clear)
- Current progress (Sprint Burndown visible)

BlueMarble Application:
- Public roadmap for players
- Development blog updates
- GitHub/Trello for backlogs
- Weekly progress reports
```

**Inspection:**
```
Regular checkpoints:
- Daily: Stand-up (15 min)
- Sprint: Review and Retrospective
- Ongoing: Backlog refinement

BlueMarble Application:
- Daily team sync
- Weekly playtest Friday
- End-of-sprint demo to stakeholders
- Monthly metrics review
```

**Adaptation:**
```
Act on inspection results:
- Sprint not going well? Adjust scope
- Feature not fun? Pivot approach
- Team velocity wrong? Replan

BlueMarble Application:
- Cut features mid-sprint if needed
- Pivot game mechanics based on playtests
- Adjust team size based on velocity
```

### 2. Scrum Values

**Commitment:** Team commits to Sprint Goal  
**Focus:** Work on Sprint Goal items only  
**Openness:** Share progress and blockers honestly  
**Respect:** Trust team members' expertise  
**Courage:** Tackle difficult problems, admit mistakes

**Game Development Adaptation:**

| Value | Challenge in Games | Solution |
|-------|-------------------|----------|
| **Commitment** | "Fun" is subjective | Commit to testing hypothesis, not feature |
| **Focus** | Distractions (cool ideas) | Strict Sprint scope protection |
| **Openness** | Fear of criticism | Psychological safety workshops |
| **Respect** | Specialist silos | Cross-training sessions |
| **Courage** | Sunk cost fallacy | Regular pivot reviews |

---

## Part II: Scrum Team

### 1. Product Owner

**Responsibilities:**
- Develop and communicate Product Goal
- Create and order Product Backlog
- Ensure backlog transparency
- Accept or reject work

**BlueMarble Product Owner:**
```
Role: Lead Game Designer

Responsibilities:
1. Define BlueMarble vision (Product Goal)
2. Prioritize features by player value
3. Maintain feature backlog
4. Decide what ships and when
5. Accept completed features (via playtesting)

Time Allocation:
- 40% Backlog management
- 30% Stakeholder communication
- 20% Sprint planning/review
- 10% Player feedback analysis
```

**Anti-Pattern:** PO as requirements gatherer  
**Better:** PO as value maximizer and decision maker

### 2. Scrum Master

**Responsibilities (2020 Update):**
- Coach team in self-management
- Help team focus on Product Goal
- Remove impediments
- Ensure Scrum events are productive

**BlueMarble Scrum Master:**
```
Role: Technical Lead or Senior Developer

Responsibilities:
1. Facilitate all Scrum events
2. Remove blockers (tools, access, decisions)
3. Shield team from interruptions
4. Coach on Agile practices
5. Drive continuous improvement

Time Allocation:
- 40% Facilitating meetings
- 30% Removing impediments
- 20% Coaching team
- 10% Process improvement
```

**Not a Project Manager:**  
Scrum Master doesn't assign tasks or manage timeline

### 3. Developers

**2020 Simplified Definition:**
"People committed to creating any aspect of a usable Increment each Sprint"

**BlueMarble Development Team:**
```
Cross-functional team (5-7 people):
- 2-3 Engineers (gameplay, backend, tools)
- 1-2 Designers (systems, level)
- 1-2 Artists (3D, UI)
- 1 QA (embedded tester)

Self-managing means:
- Team decides who does what
- Team decides how to do it
- Team commits to Sprint Goal together
- No sub-teams or hierarchies
```

**Key Change from 2017:**  
"Self-organizing" → "Self-managing" (stronger ownership)

---

## Part III: Scrum Events

### 1. The Sprint

**2020 Definition:**
"Fixed-length event (one month or less) that wraps all other events"

**BlueMarble Sprint Length:** 3 weeks (15 working days)

**Why 3 Weeks:**
- 1 week: Too short for game content
- 2 weeks: Still rushed for art/design
- 3 weeks: Sweet spot for iteration
- 4+ weeks: Feedback loop too slow

**Sprint Goal:**
```
Example Sprint Goal (Good):
"Players can craft basic tools and weapons from gathered resources,
and the experience teaches the core gathering/crafting loop"

Example Sprint Goal (Bad):
"Complete 15 backlog items"
(Not focused, not measurable, no value statement)
```

### 2. Sprint Planning

**2020 Structure:**

**Topic 1: Why is this Sprint valuable?**
```
Product Owner proposes Sprint Goal
Team discusses and refines
Team commits to Sprint Goal

Example:
PO: "We need to validate that crafting is fun"
Team: "Let's focus on 3 basic recipes and measure engagement"
Sprint Goal: "Test crafting fun factor with 3 recipes"
```

**Topic 2: What can be Done this Sprint?**
```
Team pulls items from Product Backlog
Team forecasts what can be completed
Creates Sprint Backlog

Example:
- Implement crafting UI
- Add 3 recipes (sword, axe, armor)
- Create crafting tutorial
- Add crafting metrics
```

**Topic 3: How will the work get done?**
```
Team breaks down items into tasks
Team self-assigns work
Team identifies dependencies

Example for "Implement crafting UI":
- Design mockups (Designer, 4h)
- Implement UI framework (Engineer, 8h)
- Create UI art assets (Artist, 6h)
- Wire up functionality (Engineer, 4h)
- Test and polish (QA, 4h)
```

### 3. Daily Scrum

**2020 Simplified Format:**
"Whatever structure works for the team"

**BlueMarble Daily Scrum (15 minutes):**
```
Format Options:

Option A (Traditional):
- What I did yesterday
- What I'm doing today
- Any blockers

Option B (Sprint Goal Focused):
- How did yesterday move us toward Sprint Goal?
- What will today accomplish?
- What's blocking Sprint Goal?

Option C (Kanban Style):
Walk the board right-to-left:
- What's blocked?
- What's in review?
- What's in progress?
- What's next?
```

### 4. Sprint Review

**Purpose:** Inspect Increment and adapt Product Backlog

**BlueMarble Sprint Review (2 hours):**
```
1. Demo (60 min)
   - Show working game features
   - Let stakeholders play
   - Gather feedback

2. Metrics Review (30 min)
   - Show Sprint Goal achievement
   - Present playtest data
   - Review analytics

3. Backlog Adaptation (30 min)
   - Update backlog based on feedback
   - Re-prioritize if needed
   - Discuss next Sprint
```

### 5. Sprint Retrospective

**Purpose:** Improve team effectiveness

**BlueMarble Retrospective (90 minutes):**
```
Format: "Start, Stop, Continue"

1. Individual Reflection (10 min)
   - Silent writing, post-its

2. Group Discussion (40 min)
   - What should we START doing?
   - What should we STOP doing?
   - What should we CONTINUE doing?

3. Action Items (30 min)
   - Pick top 3 improvements
   - Assign owners
   - Set deadlines

4. Review Previous Actions (10 min)
   - Did we do what we said?
   - Why or why not?
```

---

## Part IV: Scrum Artifacts

### 1. Product Backlog (+ Product Goal)

**2020 Addition: Product Goal**

**BlueMarble Product Goal:**
```
"Create an MMORPG where players experience meaningful progression
through crafting, gathering, and exploration in a persistent world
with 1000+ concurrent players per server"

This provides direction for all backlog items
```

**Product Backlog Structure:**

The Product Backlog contains ordered items (typically User Stories) that deliver value toward the Product Goal.

For detailed examples of Product Backlog structure including Epics, Features, User Stories, and Tasks for BlueMarble's crafting system, see the [Backlog Creation section](game-dev-analysis-agile-development.md#1-backlog-creation-and-prioritization) in the parent Agile Game Development document.

Each backlog item includes:
- Description
- Acceptance criteria
- Estimate (story points)
- Priority (MoSCoW)

### 2. Sprint Backlog (+ Sprint Goal)

**Sprint Goal Example:**
"By end of sprint, players can gather and craft in a way that's fun and teaches the core loop"

**Sprint Backlog:**
```
Sprint Goal: Validate crafting fun factor

Sprint Backlog Items:
1. [8 points] Gathering system
   - Chop trees for wood
   - Mine rocks for stone
   - Visual feedback

2. [5 points] Crafting UI
   - Recipe list
   - Material display
   - Craft button

3. [5 points] 3 Basic Recipes
   - Wooden sword
   - Stone axe
   - Leather armor

4. [3 points] Tutorial
   - Teach gathering
   - Teach crafting
   - Test comprehension

Total: 21 points (team velocity: 20-25)
```

### 3. Increment (+ Definition of Done)

**2020 Emphasis: Must be usable (releasable quality)**

**BlueMarble Definition of Done:**
```
Feature is Done when:
✓ Code written and reviewed
✓ Unit tests pass (>80% coverage)
✓ Integration tests pass
✓ Playtested with 10+ people
✓ Performance acceptable (60 FPS)
✓ No P0/P1 bugs
✓ Documentation updated
✓ Deployed to staging environment

If any checklist item fails, feature is NOT Done
```

---

## Part V: Implementation for BlueMarble

### 1. Team Setup

**Team Alpha: Core Gameplay**
- Product Owner: Lead Designer
- Scrum Master: Senior Engineer
- Developers: 5 people (2 eng, 2 design, 1 art)
- Sprint Length: 3 weeks

**Team Bravo: World Systems**
- Product Owner: World Design Lead
- Scrum Master: Tech Lead
- Developers: 5 people (2 eng, 2 design, 1 art)
- Sprint Length: 3 weeks

### 2. Sprint Calendar

```
Week 1:
- Monday: Sprint Planning (4h)
- Tuesday-Friday: Development + Daily Scrums
- Friday: Playtest

Week 2:
- Monday-Friday: Development + Daily Scrums
- Wednesday: Backlog Refinement (2h)
- Friday: Playtest

Week 3:
- Monday-Thursday: Development + Daily Scrums
- Friday AM: Sprint Review (2h)
- Friday PM: Sprint Retrospective (90min)
- Weekend: Buffer

Monday Week 4: Next Sprint Planning
```

### 3. Success Metrics

**Process Metrics:**
- Sprint Goal Achievement: >80%
- Velocity Stability: ±20% variance
- Definition of Done Compliance: 100%

**Outcome Metrics:**
- Player Retention: Improving trend
- Bug Escape Rate: <5%
- Team Satisfaction: >4/5

---

## Conclusions

### Key 2020 Changes

1. **Product Goal:** Long-term direction
2. **Self-Managing:** Stronger team ownership
3. **Simplified:** Less prescriptive
4. **Commitments:** Clear accountability points

### BlueMarble Recommendations

1. Start with 3-week Sprints
2. Co-locate teams if possible
3. Invest in playtest infrastructure
4. Clear Definition of Done from day 1
5. Monthly Product Goal review

### Next Steps

1. Form first Scrum team
2. Define initial Product Goal
3. Create Product Backlog (top 50 items)
4. Run Sprint 0 (setup)
5. Begin regular Sprint cadence

### References

1. The Scrum Guide 2020 - scrumguides.org
2. Scrum.org resources
3. "Scrum: The Art of Doing Twice the Work in Half the Time" - Jeff Sutherland

---

**Document Status:** Complete  
**Total Research Time:** 3 hours  
**Completion Date:** 2025-01-15  
**Author:** Research Team, Phase 2 Discovered Source

**Related Documents:**
- `research/literature/game-dev-analysis-agile-development.md` (Parent)
- `research/literature/game-dev-analysis-lean-startup-principles.md` (Related)

**Tags:** #scrum #scrum-guide-2020 #agile #framework #methodology #phase-2
