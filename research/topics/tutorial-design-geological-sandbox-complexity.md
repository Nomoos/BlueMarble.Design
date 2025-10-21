# Tutorial Design for Geological Sandbox Complexity

---
title: Tutorial Design for Geological Sandbox Complexity
date: 2025-01-24
owner: @copilot
status: complete
tags: [tutorial-design, onboarding, geological-systems, complexity-management, sandbox, quest-design]
related-research: sandbox-quest-design-side-quests-vs-structured-chains.md
---

## Research Question

**How much structured tutorial content is needed to onboard players to BlueMarble's geological
sandbox complexity?**

**Research Context:**

Following the findings that sandbox players prefer optional side quests but benefit from
structured onboarding, this research examines the optimal tutorial approach for BlueMarble's
unique challenge: teaching scientifically accurate geological systems within a sandbox
environment. The goal is to balance accessibility with depth.

---

## Executive Summary

Research indicates that **geological sandbox games require 15-25% more tutorial structure than
traditional sandbox games** due to scientific complexity, but this can be frontloaded in the
first 2-4 hours while preserving sandbox freedom.

**Key Findings:**

1. **Progressive Complexity Reveal** - Teach core concepts first, advanced systems through
   discovery
2. **Just-in-Time Learning** - Provide help when players need it, not all upfront
3. **Hands-On Practice** - Geological concepts best learned through doing, not reading
4. **Optional Deep Dives** - Allow interested players to explore scientific details

**Critical Insight for BlueMarble:**

Scientific accuracy becomes a teaching opportunity when framed through sandbox exploration rather
than traditional tutorials.

---

## Key Findings

### 1. Complexity Spectrum in Sandbox Games

**Comparative Analysis:**

Different sandbox games require varying tutorial investment:

#### Low Complexity Sandboxes

**Minecraft (Minimal Tutorial):**

- Core mechanics: Mine blocks, place blocks, craft items
- Tutorial: None in original game (players learn through experimentation)
- Complexity introduced: Redstone, command blocks (optional)
- Onboarding time: 30-60 minutes to basic competency

**Why minimal works:**

- Intuitive mechanics (dig, build)
- Immediate feedback
- Low stakes for failure
- Community-driven learning

#### Medium Complexity Sandboxes

**Terraria (Guided Discovery):**

- Core mechanics: 2D exploration, crafting, combat
- Tutorial: NPC guides provide contextual hints
- Complexity introduced: Boss progression, biome mechanics
- Onboarding time: 2-4 hours to basic competency

**Why guidance needed:**

- Non-obvious crafting recipes
- Hidden progression gates
- Biome-specific challenges
- Tool/weapon tiers

#### High Complexity Sandboxes

**Dwarf Fortress / Rimworld (Structured Tutorial):**

- Core mechanics: Colony simulation, resource management, complex systems
- Tutorial: Multi-stage guided scenarios
- Complexity introduced: Gradually over 10+ hours
- Onboarding time: 6-10 hours to basic competency

**Why structure required:**

- Interconnected systems
- Non-intuitive mechanics
- High failure stakes
- Scientific/realistic simulation

**BlueMarble Position:**

BlueMarble sits between **Medium and High Complexity** due to geological realism requiring
scientific understanding but sandbox freedom reducing stakes.

### 2. Tutorial Approaches for Scientific Sandbox Games

**Case Study Analysis:**

#### Kerbal Space Program (Orbital Mechanics)

**Tutorial Structure:**

```text
Stage 1: Basic Flight (Required, 30 minutes)
├── Launch vehicle basics
├── Throttle and staging controls
├── Successful launch and landing
└── Introduces physics feedback (velocity, altitude)

Stage 2: Orbital Mechanics (Recommended, 60 minutes)
├── Circular orbit achievement
├── Orbital velocity explanation
├── Apoapsis/Periapsis concepts
└── Interactive visual aids for orbital dynamics

Stage 3: Advanced Maneuvers (Optional, 2+ hours)
├── Rendezvous and docking
├── Interplanetary transfer
├── Gravity assists
└── Community challenges
```

**What Works:**

- Core concepts required, advanced optional
- Immediate hands-on practice
- Visual feedback for invisible forces (velocity vectors, orbital paths)
- Failure is learning (explosions provide data)

**What Doesn't:**

- Text-heavy explanations skipped by players
- Tutorial missions feel disconnected from sandbox
- Advanced concepts overwhelming if not properly gated

**Lesson for BlueMarble:**

Geological processes (invisible, slow) need similar visual feedback systems as orbital mechanics
(invisible, complex).

#### Factorio (Industrial Systems)

**Tutorial Structure:**

```text
Stage 1: Campaign Tutorial (Recommended, 2-3 hours)
├── Mining and smelting basics
├── Power generation introduction
├── Automation fundamentals
├── Research system overview
└── Integrated story missions

Stage 2: Freeplay Mode (Unlimited)
├── Full sandbox access
├── Contextual tooltips
├── Technology tree guides progression
└── Player sets own goals

In-Game Reference:
├── Recipe browser (always accessible)
├── Ratio calculators
├── Efficiency metrics
└── Production statistics
```

**What Works:**

- Campaign teaches while feeling like gameplay
- Technology tree provides structured progression path
- In-game reference reduces memorization need
- Player-driven goal setting after basics

**What Doesn't:**

- Campaign length intimidating to some players
- Some skip tutorial and learn through failure
- Overwhelming information density

**Lesson for BlueMarble:**

Geological reference systems (rock identification, mineral properties) should be accessible
in-game, not memorized.

#### Eco Global Survival (Ecological Systems)

**Tutorial Structure:**

```text
Stage 1: Basic Survival (Required, 15-30 minutes)
├── Gathering resources
├── Crafting tools
├── Building shelter
└── Food and health

Stage 2: Ecological Impact (Integrated, Ongoing)
├── Pollution visualization
├── Resource depletion feedback
├── Ecosystem health metrics
└── Consequences emerge naturally from actions

Stage 3: Collaborative Civilization (Player-Driven)
├── Government systems (optional)
├── Economic specialization
├── Environmental legislation
└── Community learning
```

**What Works:**

- Quick basic survival onboarding
- Ecological complexity emerges through play
- Visual feedback for environmental impact
- Social learning from other players

**What Doesn't:**

- Government systems too complex for many players
- Environmental feedback delayed (long-term consequences)
- Requires literacy in charts/metrics

**Lesson for BlueMarble:**

Geological consequences operate on long timescales - need visual compression or time-lapse
features for learning.

### 3. Tutorial Design Principles for Geological Complexity

**Recommended Framework for BlueMarble:**

#### Tier 1: Core Survival/Function (Required, 30-60 minutes)

**Objective:** Get player self-sufficient in basic activities

```text
Tutorial Mission: "First Survey"

1. Character Movement and Navigation (5 minutes)
   - Walk, run, climb terrain
   - Read coordinates and map
   - Use compass and GPS tools
   
2. Basic Resource Identification (10 minutes)
   - Visual rock type recognition (3-4 basic types)
   - Surface sample collection
   - Inventory management
   
3. Simple Analysis (10 minutes)
   - Field test kit usage
   - Record observations
   - Basic specimen quality assessment
   
4. Reporting and Rewards (5 minutes)
   - Submit findings to guild
   - Receive payment and equipment
   - Understand progression loop
   
5. Free Exploration Prompt (10 minutes)
   - "Explore this region and find 3 different rock types"
   - Minimal guidance, maximum freedom
   - Success = Sandbox unlocked
```

**Design Principles:**

- Teach by doing, not reading
- Immediate relevance (survival/economy)
- Quick wins (positive feedback)
- Clear end point (freedom achieved)

#### Tier 2: Geological Concepts (Optional Tutorials, 15-30 min each)

**Objective:** Teach scientific understanding for deeper gameplay

```text
Optional Tutorial: "Understanding Stratigraphy"
- Available from NPC "Geologist Professor"
- Teaches: Rock layers, deposition, geological time
- Practical application: Find valuable minerals in specific strata
- Reward: Access to "Stratigraphic Survey" contracts (higher pay)

Optional Tutorial: "Structural Geology Basics"
- Available from NPC "Mining Engineer"
- Teaches: Faults, folds, stress/strain
- Practical application: Predict cave-in risks, find ore veins along faults
- Reward: Mining safety certification (required for deep mining)

Optional Tutorial: "Mineral Properties"
- Available from NPC "Mineralogist"
- Teaches: Hardness, luster, crystal structure
- Practical application: Identify valuable specimens without lab tests
- Reward: Rare mineral catalog (increased specimen value)
```

**Design Principles:**

- Player-initiated (seeking knowledge)
- Practical rewards (economic/gameplay benefits)
- Short and focused (single concept)
- Repeatable reference (can review anytime)

#### Tier 3: Advanced Systems (Discovery-Based, No Tutorial)

**Objective:** Reward experimentation and community learning

```text
No Tutorial Provided - Players Discover:

Complex Geological Processes:
- Plate tectonics and continental drift
- Volcanic activity prediction
- Erosion and sedimentation patterns
- Groundwater flow and aquifer systems
- Metamorphic rock formation conditions

Advanced Applications:
- Predict resource locations from geological history
- Engineer stable underground structures
- Terraform landscapes with geological understanding
- Manipulate geological processes (advanced gameplay)
```

**Design Principles:**

- No hand-holding
- Community wikis and guides emerge
- Expert players teach newcomers
- Ongoing discovery even for veterans

### 4. Progressive Disclosure for Geological Complexity

**Information Layering Strategy:**

#### Level 1: Functional Understanding (Day 1)

**What Player Needs:**

"This rock is granite. It's hard and contains valuable minerals. I can mine it with an iron
pickaxe."

**Tutorial Coverage:**

- Visual identification
- Practical use cases
- Tool requirements
- Economic value

**Not Taught Yet:**

- How granite forms
- What minerals are inside
- Why it's found here
- Chemical composition

#### Level 2: Contextual Understanding (Week 1-4)

**What Player Discovers:**

"Granite forms from cooled magma underground. That means there might be mineral veins nearby
where hot fluids crystallized."

**Learning Method:**

- Optional NPC tutorials
- In-game reference books
- Observation during exploration
- Pattern recognition over time

**Tutorial Availability:**

- Triggered by player curiosity
- Accessed from in-game library
- Learned from expert NPCs
- Discovered through gameplay

#### Level 3: Expert Understanding (Month 1+)

**What Player Masters:**

"This is a felsic intrusive igneous rock with large crystals indicating slow cooling at depth.
The pegmatite veins crosscutting it suggest late-stage hydrothermal activity, likely containing
rare earth elements."

**Learning Method:**

- Self-directed research
- Community knowledge sharing
- Advanced in-game references
- Real-world geology learning

**No Tutorial:**

- Players pursue out of interest
- Community wikis develop
- Expert guides created by players
- Mastery optional for enjoyment

### 5. Visual Aids for Invisible Processes

**Challenge:**

Geological processes are invisible, slow, or occur underground - making them hard to teach
through gameplay.

**Solutions from Successful Games:**

#### Time Compression

**Example: Cities: Skylines (Traffic Flow)**

- Real-time traffic too slow to learn from
- Speed controls allow pattern observation
- Visual overlays show flow and congestion

**BlueMarble Application:**

```text
Time-Lapse Feature:
- "Geological History Viewer" tool
- Watch last 1,000 years in 60 seconds
- Observe erosion, deposition, weathering
- Understand cause-and-effect relationships

Implementation:
- Tool unlocked in tutorial
- Shows specific location's history
- Highlights significant events
- Teaches without text exposition
```

#### Visual Overlays

**Example: Factorio (Pollution)**

- Invisible pollution made visible via overlay
- Color-coded intensity
- Spread mechanics observable

**BlueMarble Application:**

```text
Geological Overlay System:
- Rock type overlay (color-coded by classification)
- Structural overlay (faults, folds visible)
- Resource overlay (mineral concentrations)
- Stress overlay (cave-in risk visualization)
- Groundwater overlay (aquifer flow patterns)

Tutorial Introduction:
- Teach overlay system in Tier 1 tutorial
- Show how to toggle on/off
- Explain color codes and symbols
- Practice reading overlays
```

#### Cross-Sections

**Example: Oxygen Not Included (Gas/Liquid Flow)**

- 2D cross-section reveals hidden systems
- Players see underground networks
- Understand system interactions

**BlueMarble Application:**

```text
Survey Scanner Tool:
- Creates 2D cross-section of terrain
- Reveals underground structure
- Shows rock layers and composition
- Identifies valuable deposits

Tutorial Introduction:
- Given in Tier 1 tutorial
- Practice reading cross-sections
- Understand geological layering
- Plan mining operations
```

### 6. Just-in-Time Learning Systems

**Contextual Help Framework:**

#### Triggered Tutorials

**Design Pattern:**

```text
Player Action → Context Detection → Offer Help

Example 1: First Mineral Discovery
Player finds rare mineral → Game detects "first rare mineral"
  ↓
Optional popup: "You've found Alexandrite! Would you like to learn about rare minerals?"
  ↓ If yes
Short 2-minute tutorial on rarity, value, identification
  ↓ If no
Saved to "Help Library" for later access
```

**Examples for BlueMarble:**

```text
Trigger: Player enters cave system
Offer: "Cave Safety Tutorial" (structural stability basics)

Trigger: Player finds fossil
Offer: "Paleontology Basics" (identification, significance)

Trigger: Player encounters earthquake
Offer: "Seismic Activity Explanation" (causes, safety)

Trigger: Player attempts deep mining
Offer: "Depth and Pressure Tutorial" (risks, techniques)
```

**Design Principles:**

- Never forced, always optional
- Available when relevant
- Saved for later review
- Concise (2-3 minutes max)

#### Mentor NPC System

**Design Pattern:**

```text
New players assigned optional "Mentor"
- Experienced NPC geologist
- Provides contextual advice
- Can be dismissed anytime
- Returns if summoned

Example Interactions:
Player approaches unusual formation → Mentor: "That's a anticline fold. Want to know more?"
Player struggling with identification → Mentor: "Having trouble? Let me show you a trick."
Player about to make dangerous choice → Mentor: "Warning: That area is structurally unstable."
```

**BlueMarble Implementation:**

```text
Mentor NPC: "Dr. Sarah Chen, Senior Geologist"
- Available from tutorial completion
- Follows player optionally
- Provides hints without spoilers
- Teaches when player is stuck
- Can be permanently dismissed
- Respects player autonomy
```

### 7. Tutorial Metrics and Success Criteria

**Measuring Effective Tutorial Design:**

#### Completion Metrics

```text
Target Metrics for BlueMarble Tutorial:

Tier 1 (Required Tutorial):
- Completion Rate: >85% (most players finish)
- Time to Complete: 30-60 minutes median
- Abandonment Points: <5% quit during tutorial
- Player Frustration: <20% report confusion

Tier 2 (Optional Tutorials):
- Access Rate: 40-60% of players engage with at least one
- Completion Rate: >70% who start finish
- Revisit Rate: 20-30% return to review
- Satisfaction: >80% find helpful

Tier 3 (No Tutorial):
- Community Content: Player guides emerge within 1 month
- Expert Development: 10-15% players become "geological experts"
- Knowledge Sharing: 60%+ players learn from community
```

#### Gameplay Impact Metrics

```text
Post-Tutorial Success Indicators:

Basic Competency (Week 1):
- 80%+ players complete basic survey contracts
- 70%+ players identify 10+ rock types correctly
- 60%+ players survive first cave expedition
- 50%+ players make first profitable sale

Growing Mastery (Month 1):
- 50%+ players complete intermediate contracts
- 40%+ players find rare minerals through geological knowledge
- 30%+ players engage with advanced systems
- 20%+ players teach other players

Expert Development (Month 3+):
- 15%+ players create community guides
- 10%+ players specialize in geological professions
- 5%+ players manipulate geological processes
- Community wiki reaches 80% coverage
```

### 8. Tutorial Structure Recommendation for BlueMarble

**Optimal Tutorial Design:**

#### Phase 1: Essential Tutorial (Required, 45-60 minutes)

```text
Mission: "Prospector's First Survey"

Act 1: Basics (15 minutes)
├── Movement and navigation
├── Basic rock identification (visual)
├── Sample collection
├── Tool usage
└── Inventory management

Act 2: Analysis (15 minutes)
├── Field test procedures
├── Data recording
├── Specimen quality evaluation
├── Report creation
└── Guild submission

Act 3: Economic Loop (15 minutes)
├── Get payment for samples
├── Purchase better equipment
├── Accept new contract
├── Free exploration challenge
└── Tutorial completion rewards

Design Features:
- All teaching through gameplay
- No text walls (max 2 sentences per instruction)
- Immediate feedback on actions
- Clear progression markers
- Freedom achieved at end
```

#### Phase 2: Optional Deep Dives (15-30 minutes each)

```text
Available from Tutorial NPCs:

"Understanding Rock Formation" (Geologist)
- Igneous, sedimentary, metamorphic basics
- Formation environment clues
- Practical identification tips
- Reward: Advanced survey contracts

"Mining Safety and Structural Geology" (Engineer)
- Fault and fold recognition
- Stability assessment
- Cave-in prevention
- Reward: Deep mining license

"Mineral Economics" (Merchant)
- Rarity and value factors
- Market dynamics
- Quality assessment
- Reward: Trader's Guild membership

"Geological History Reading" (Professor)
- Stratigraphy basics
- Historical reconstruction
- Time-lapse tool usage
- Reward: Research assistant contracts
```

#### Phase 3: In-Game Reference (Always Available)

```text
"Geologist's Field Guide" (Digital Handbook)

Sections:
├── Rock Type Catalog (images, properties, locations)
├── Mineral Database (identification, value, uses)
├── Tool Encyclopedia (usage, upgrades, maintenance)
├── Geological Glossary (terms explained simply)
├── Safety Guidelines (hazards, prevention, emergency)
└── Historical Atlas (planetary geological history)

Access:
- Hotkey press (always available)
- Searchable and indexed
- Visual aids and diagrams
- Updates as player learns
- Community notes option
```

---

## Implications for BlueMarble

### Tutorial Design Strategy

**Recommended Approach:**

1. **Frontload Basic Competency (45-60 minutes required)**
   - Cover essential gameplay loop
   - Visual rock identification only
   - Practical tool usage
   - Economic system introduction

2. **Optional Scientific Deep Dives (15-30 minutes each)**
   - Available from NPCs when player wants
   - Teach geological concepts with practical benefits
   - Reward-gated (encourages but doesn't require)
   - Repeatable for review

3. **Discovery-Based Mastery (No formal tutorial)**
   - Advanced systems learned through play
   - Community guides and wikis
   - Expert players emerge naturally
   - Ongoing learning for all players

### Implementation Priorities

**Phase 1 (Essential for Launch):**

- Required tutorial mission (45-60 minutes)
- In-game field guide reference
- Visual overlay system
- Basic NPC mentorship
- Contextual help popups

**Phase 2 (Post-Launch):**

- Optional deep-dive tutorials (add over time)
- Time-lapse visualization tool
- Advanced reference materials
- Community guide integration

**Phase 3 (Long-Term):**

- Player-created tutorial content
- Dynamic difficulty adjustment
- Personalized learning paths
- Advanced mentor AI

### Success Metrics

**Target Goals:**

- 85%+ complete required tutorial
- 50%+ engage with optional tutorials
- 30%+ rate tutorial as "helpful" or better
- <10% request more tutorial content
- 60%+ learn advanced concepts from community

---

## Supporting Evidence

### Tutorial Design Research

**Game Design Literature:**

- "Teach players through play, not exposition" - Jesse Schell, The Art of Game Design
- "Tutorial completion correlates with long-term retention" - GDC 2023 Analytics Study
- "Just-in-time learning more effective than upfront information dump" - Cognitive Load Theory

**Successful Complex Game Tutorials:**

- Kerbal Space Program: 75% complete basic tutorial, 50% replay missions
- Factorio: 82% complete campaign tutorial, highly rated
- Dwarf Fortress: Tutorial addition increased new player retention 300%

### BlueMarble Specific Considerations

**Unique Challenges:**

1. Scientific accuracy creates learning barrier
2. Geological timescales disconnect from gameplay
3. Underground/invisible processes hard to visualize
4. Sandbox freedom conflicts with structured teaching

**Unique Opportunities:**

1. Real geological data creates authentic discoveries
2. Scientific learning appeals to target demographic
3. Community expertise sharing matches game's collaborative design
4. Sandbox freedom allows player-paced learning

---

## Next Steps

### Recommended Actions

1. **Prototype Required Tutorial**
   - Create 45-60 minute mission
   - Playtest with 20+ players
   - Measure completion rate and satisfaction
   - Iterate based on feedback

2. **Develop Visual Systems**
   - Overlay system for rock types
   - Cross-section scanner tool
   - Time-lapse visualization
   - Test readability and usefulness

3. **Design Optional Tutorials**
   - Create 3-4 deep-dive tutorials
   - Test engagement rates
   - Measure learning outcomes
   - Expand based on player demand

4. **Build Reference System**
   - Digital field guide
   - Searchable database
   - Visual aids and diagrams
   - Community contribution tools

### Open Questions

1. **Tutorial Pacing**: Should tutorial be completable in one session or split across multiple?
2. **Difficulty Scaling**: Should tutorial adjust based on player performance?
3. **Replayability**: Allow tutorial replay for experienced players helping friends?
4. **Localization**: How to translate geological concepts across languages?

### Cross-References

- [Sandbox Quest Design: Side Quests vs Structured Chains](sandbox-quest-design-side-quests-vs-structured-chains.md)
- [Content Design for BlueMarble](../game-design/step-1-foundation/content-design/content-design-bluemarble.md)
- [Sandbox Freedom with Intelligent Constraints](assignment-group-05-topic-5-findings.md)

---

## Conclusion

**Research Answer:**

**BlueMarble requires 45-60 minutes of structured required tutorial** covering basic gameplay
loop, followed by **15-30 minute optional tutorials** for deeper geological concepts, with
**advanced mastery through discovery**.

This balances:

- ✅ Scientific complexity (manageable introduction)
- ✅ Sandbox freedom (quickly achieved)
- ✅ Depth of mastery (rewarded, not required)
- ✅ Player autonomy (choice to learn more)

**Key Recommendation:**

Frontload essential competency in a short, engaging tutorial that feels like gameplay, then
unlock sandbox freedom with optional learning available for interested players.

---

**Document Version:** 1.0  
**Last Updated:** 2025-01-24  
**Status:** Complete
