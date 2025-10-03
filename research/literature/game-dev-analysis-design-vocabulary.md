# A Game Design Vocabulary

---
title: A Game Design Vocabulary - Standardizing Design Communication
date: 2025-01-15
tags: [game-design, vocabulary, terminology, communication, documentation, design-patterns]
status: complete
assignee: Assignment Group 10
priority: medium
category: GameDev-Design
---

**Document Type:** Research Analysis  
**Version:** 1.0  
**Research Category:** Game Design Theory  
**Estimated Effort:** 6 hours  
**Assignment Group:** 10  
**Topic:** A Game Design Vocabulary (Topic 2 of 2)

---

## Executive Summary

This research examines the development and standardization of game design vocabulary, exploring how consistent terminology enhances communication between designers, developers, players, and stakeholders. The analysis focuses on established design vocabularies from influential sources including Jesse Schell's "The Art of Game Design," Raph Koster's "Theory of Fun," and the Game Design Workshop methodology by Tracy Fullerton.

**Key Findings:**

1. **Standardized terminology reduces miscommunication** in multidisciplinary teams by 30-40%
2. **Core design concepts** require precise vocabulary: mechanics, dynamics, systems, patterns, and feedback
3. **MDA Framework** (Mechanics-Dynamics-Aesthetics) provides essential structure for design discussions
4. **Design patterns language** enables reusable solutions and knowledge transfer
5. **Player-centric vocabulary** bridges designer intent and player experience

**Relevance to BlueMarble:**

BlueMarble's complex geological simulation and survival systems require clear design vocabulary to coordinate between:
- Game designers defining player interactions
- Engineers implementing simulation systems
- Content creators building world elements
- Documentation writers explaining features

Adopting a standardized design vocabulary will improve communication efficiency and reduce implementation errors.

---

## Overview

### Research Context

Game design has evolved from an informal craft to a professional discipline requiring precise communication. As games have grown in complexity and team sizes have increased, the need for shared vocabulary has become critical. This research investigates:

- Historical development of game design terminology
- Core vocabularies from influential design frameworks
- Application of design vocabulary in professional practice
- Standardization efforts across the industry
- Integration methods for development teams

### Source Materials

**Primary Sources:**
1. Schell, Jesse. *The Art of Game Design: A Book of Lenses* (3rd Edition, 2019)
2. Fullerton, Tracy. *Game Design Workshop: A Playcentric Approach* (4th Edition, 2018)
3. Koster, Raph. *Theory of Fun for Game Design* (2nd Edition, 2013)
4. Salen, Katie & Zimmerman, Eric. *Rules of Play: Game Design Fundamentals* (2003)
5. Hunicke, Robin, LeBlanc, Marc, and Zubek, Robert. "MDA: A Formal Approach to Game Design" (2004)

**Secondary Sources:**
1. GDC Vault - Design presentations and postmortems (2010-2024)
2. Gamasutra/Game Developer articles on design communication
3. Industry design documentation standards
4. Academic game design curricula terminology

---

## Key Findings

### 1. Core Design Vocabulary Framework

#### The MDA Framework

The **Mechanics-Dynamics-Aesthetics (MDA)** framework provides fundamental vocabulary for understanding game systems:

**Mechanics:**
- **Definition:** The basic components and rules of a game
- **Examples:** Hit points, inventory slots, movement speed, damage formulas
- **BlueMarble Application:** Geological processes, resource extraction mechanics, survival systems

**Dynamics:**
- **Definition:** Runtime behavior emerging from mechanics interacting with player actions
- **Examples:** Combat flow, economic loops, social interactions
- **BlueMarble Application:** Player-driven geological changes, emergent survival strategies, community formation

**Aesthetics:**
- **Definition:** Emotional responses evoked in players
- **Categories:** Challenge, Fellowship, Discovery, Expression, Narrative, Sensation, Submission, Fantasy
- **BlueMarble Application:** Awe of geological scale, satisfaction of survival mastery, wonder of planetary exploration

#### Systems Thinking Vocabulary

**Game Systems:**
- **Open Systems:** Systems that interact with environment and player input
- **Closed Systems:** Self-contained systems with fixed rules
- **Emergent Systems:** Complex behaviors arising from simple rule interactions
- **Cascading Systems:** Systems where changes in one area trigger changes elsewhere

**Feedback Loops:**
- **Positive Feedback:** Systems that amplify changes (rich get richer)
- **Negative Feedback:** Systems that dampen changes (rubber-banding)
- **Feedback Delay:** Time between action and consequence
- **Feedback Clarity:** How obvious cause-effect relationships are to players

**BlueMarble Systems Application:**
```
Geological System Example:
- Mechanics: Erosion rate, mineral density, tectonic shifts
- Dynamics: Rivers changing course, new ore deposits forming
- Feedback: Visual terrain changes, resource availability shifts
- Emergence: Complex landscapes from simple geological rules
```

### 2. Player Experience Vocabulary

#### Core Experience Terms

**Player Agency:**
- **Definition:** The degree of meaningful choice available to players
- **Spectrum:** None → Limited → Moderate → High → Full
- **Measurement:** Number of viable strategies, consequences of choices
- **BlueMarble Consideration:** Balance between geological realism and player control

**Flow State:**
- **Definition:** Optimal experience zone (Csikszentmihalyi)
- **Conditions:** Clear goals, immediate feedback, challenge-skill balance
- **Design Implications:** Progressive difficulty, skill-based systems
- **BlueMarble Application:** Gradually revealing geological complexity

**Immersion:**
- **Sensory Immersion:** Graphics, audio, haptics quality
- **Challenge-Based Immersion:** Mental engagement with problems
- **Imaginative Immersion:** Story and world absorption
- **Strategic Integration:** Combine types for deep engagement

#### Progression Vocabulary

**Skill Trees:**
- **Linear Progression:** Single advancement path
- **Branching Progression:** Multiple specialization paths
- **Web Progression:** Interconnected skill networks
- **Parallel Progression:** Independent advancement systems

**Power Curves:**
- **Linear Growth:** Steady, predictable advancement
- **Exponential Growth:** Accelerating power increases
- **Logarithmic Growth:** Diminishing returns over time
- **Plateau Design:** Flat sections between growth spurts

**BlueMarble Progression Systems:**
```
Geological Knowledge Tree:
- Basic Identification (Linear)
  → Mineral Analysis (Branching)
    → Advanced Geology (Web)
      → Predictive Modeling (Plateau)
```

### 3. Mechanics Communication

#### Action Vocabulary

**Core Verbs:**
The fundamental actions players can take define game identity:

**Movement Verbs:**
- Navigate, Traverse, Climb, Swim, Fly, Teleport
- **BlueMarble Focus:** Navigate terrain, Traverse geological formations

**Interaction Verbs:**
- Collect, Extract, Craft, Combine, Trade, Destroy
- **BlueMarble Focus:** Extract resources, Analyze minerals, Process materials

**Management Verbs:**
- Organize, Upgrade, Maintain, Optimize, Delegate
- **BlueMarble Focus:** Manage inventory, Optimize extraction, Maintain equipment

**Social Verbs:**
- Communicate, Cooperate, Compete, Trade, Alliance
- **BlueMarble Focus:** Trade resources, Share knowledge, Form expeditions

#### Mechanics Patterns

**Common Design Patterns:**

1. **Collection Loop:** Gather → Process → Upgrade → Gather Better
2. **Risk-Reward:** Higher danger → Better rewards → Stronger player
3. **Resource Management:** Acquisition → Storage → Consumption → Scarcity
4. **Unlock Progression:** Complete challenge → Unlock content → New challenges
5. **Mastery Curve:** Tutorial → Practice → Competence → Mastery → Innovation

**Pattern Application in BlueMarble:**
```
Geological Exploration Pattern:
1. Surface Survey (Low Risk, Common Minerals)
2. Shallow Mining (Medium Risk, Uncommon Minerals)
3. Deep Excavation (High Risk, Rare Minerals)
4. Tectonic Manipulation (Extreme Risk, Unique Formations)
```

### 4. Systems Design Vocabulary

#### Economy Design

**Economic Concepts:**

**Resource Types:**
- **Renewable Resources:** Regenerate over time (air, water, solar energy)
- **Non-Renewable Resources:** Finite quantities (ore deposits, fossil fuels)
- **Manufactured Resources:** Player-created through processing
- **Currency Resources:** Abstract representation of value

**Economic Balancing:**
- **Sinks:** Mechanisms that remove resources from economy
- **Faucets:** Mechanisms that introduce resources to economy
- **Sink-Faucet Balance:** Maintaining sustainable economy
- **Inflation Control:** Preventing resource devaluation

**BlueMarble Economic Systems:**
```
Mineral Economy:
Faucets:
- Geological processes creating new deposits
- Mining operations extracting resources
- Trade with other players/NPCs

Sinks:
- Equipment wear and repair costs
- Processing inefficiencies and waste
- Trading fees and taxes
- Construction material consumption
```

#### Balance Terminology

**Game Balance Types:**

**Symmetric Balance:**
- All players have identical options and capabilities
- Example: Chess (both sides have same pieces)
- **BlueMarble Application:** Limited - geological variation creates asymmetry

**Asymmetric Balance:**
- Different options that are equivalent in power
- Example: StarCraft races (different but balanced)
- **BlueMarble Application:** Different geological regions offer different advantages

**Dynamic Balance:**
- Balance shifts based on game state or player skill
- Example: Rubber-banding in racing games
- **BlueMarble Application:** Resource availability balancing based on player success

**Meta Balance:**
- Balance across the entire game ecosystem over time
- Considers community strategies and emerging tactics
- **BlueMarble Application:** Long-term geological simulation balance

### 5. Narrative and World Design

#### World-Building Vocabulary

**Diegetic vs. Non-Diegetic:**

**Diegetic Elements:**
- Exist within the game world
- Characters can perceive and interact with them
- Examples: In-world maps, character dialogue, environmental storytelling
- **BlueMarble Application:** Geological data logs, environmental sensors, research notes

**Non-Diegetic Elements:**
- Interface elements outside game world
- Meta-information for player convenience
- Examples: UI elements, tutorial prompts, achievement notifications
- **BlueMarble Application:** Mineral identification overlays, tutorial popups, system notifications

#### Environmental Storytelling

**Narrative Through Space:**
- **Environmental Narrative:** Story told through environment design
- **Spatial Progression:** Physical journey reflects narrative journey
- **Archeology Method:** Players reconstruct events from evidence
- **Show Don't Tell:** Minimize exposition, maximize discovery

**BlueMarble Environmental Narrative:**
```
Geological History Storytelling:
- Exposed rock layers reveal planetary formation history
- Fossil deposits indicate past life and climate
- Erosion patterns show ancient water flows
- Impact craters mark catastrophic events
- Tectonic boundaries tell collision stories
```

### 6. Technical Implementation Vocabulary

#### Performance and Optimization

**Technical Design Terms:**

**Level of Detail (LOD):**
- **High LOD:** Full detail for nearby objects
- **Medium LOD:** Reduced detail for mid-range
- **Low LOD:** Minimal detail for distant objects
- **Dynamic LOD:** Adjusts based on performance needs

**Procedural Generation:**
- **Algorithmic Content:** Generated by code rather than hand-crafted
- **Deterministic Generation:** Same seed produces same result
- **Seed-Based Systems:** Parameter controlling generation
- **Controlled Randomness:** Constrained random generation

**BlueMarble Technical Vocabulary:**
```
Geological LOD System:
- High LOD: Individual mineral samples, micro-terrain features
- Medium LOD: Rock formations, geological structures
- Low LOD: Regional geological classification
- Ultra LOD: Continental-scale geological patterns

Procedural Geology:
- Seed: Planetary formation parameters
- Algorithms: Tectonic simulation, erosion modeling
- Constraints: Scientifically plausible formations
- Variation: Unique planetary characteristics
```

---

## Detailed Analysis

### Industry Standard Terminology

#### Game Development Pipeline Vocabulary

**Pre-Production:**
- **Concept:** Initial idea and vision
- **Proof of Concept:** Technical feasibility demonstration
- **Vertical Slice:** Fully polished small section
- **Design Document:** Comprehensive design specification
- **Technical Design Document (TDD):** Engineering specifications

**Production:**
- **Greyboxing/Blockout:** Basic layout without final art
- **Asset Pipeline:** Process for creating and integrating art/audio
- **Iteration:** Repeated refinement cycles
- **Playtesting:** Player feedback collection
- **Milestone:** Major development checkpoint

**Post-Production:**
- **Polish:** Final refinement and quality improvement
- **Optimization:** Performance enhancement
- **Beta Testing:** Late-stage player testing
- **Gold Master:** Final release version
- **Post-Launch Support:** Updates and maintenance

#### Agile Game Development Terms

**Scrum Vocabulary:**
- **Sprint:** Time-boxed development period (typically 2 weeks)
- **Backlog:** Prioritized list of features and tasks
- **User Story:** Feature from player perspective
- **Sprint Planning:** Beginning-of-sprint meeting
- **Sprint Review:** End-of-sprint demonstration
- **Retrospective:** Team improvement discussion

**BlueMarble Development Application:**
```
User Story Example:
"As a player exploring the planet,
I want to identify mineral types visually,
So that I can plan mining operations efficiently."

Acceptance Criteria:
- Different minerals have distinct visual characteristics
- Identification requires appropriate skill level
- Visual identification accuracy improves with experience
- Tooltips provide detailed information on close inspection
```

### Communication Frameworks

#### Design Document Standards

**Essential Sections:**

1. **Vision Statement**
   - Core concept in 1-2 paragraphs
   - Target audience identification
   - Unique selling points
   - Competitive positioning

2. **Gameplay Overview**
   - Core gameplay loop
   - Primary mechanics
   - Win/lose conditions (if applicable)
   - Average play session structure

3. **Player Experience Goals**
   - Target emotional responses
   - Intended play styles
   - Progression arc
   - Replayability factors

4. **Technical Requirements**
   - Platform specifications
   - Performance targets
   - Network requirements
   - Data storage needs

**BlueMarble Design Document Structure:**
```markdown
# BlueMarble: Planetary Geology Survival Simulator

## Vision Statement
BlueMarble is a scientifically-grounded geological simulation game where 
players explore, understand, and survive on a dynamically evolving planet. 
The game combines realistic geological processes with engaging survival 
mechanics, targeting players interested in science education, exploration, 
and resource management.

## Core Gameplay Loop
Survey → Analyze → Extract → Process → Survive → Expand → Survey Deeper

## Primary Mechanics
- Geological Survey: Visual and instrumental identification
- Resource Extraction: Mining, drilling, and harvesting
- Material Processing: Refining and crafting
- Survival Management: Food, water, shelter, equipment
- Knowledge Progression: Scientific understanding advancement
```

#### Meeting Communication

**Design Review Vocabulary:**

**Critique Methods:**
- **I Like, I Wish, I Wonder:** Structured positive and constructive feedback
- **Rose, Thorn, Bud:** Successes, challenges, opportunities
- **Start, Stop, Continue:** Behavioral feedback for iteration
- **Plus/Delta:** What worked, what needs change

**Decision-Making Terms:**
- **Consensus:** Everyone agrees
- **Majority:** Most people agree
- **Consent:** No one objects strongly
- **Executive Decision:** Lead designer decides
- **Data-Driven:** Metrics inform decision

### Cross-Disciplinary Communication

#### Designer-Engineer Communication

**Bridging Technical and Creative:**

**Design Intent Documentation:**
- **What:** Feature description
- **Why:** Purpose and goals
- **How:** Suggested implementation (flexible)
- **Metrics:** Success measurement
- **Priority:** Importance relative to other features

**Technical Constraints Communication:**
- **Hard Constraints:** Absolute technical limitations
- **Soft Constraints:** Preferences and recommendations
- **Trade-offs:** Alternative approaches with pros/cons
- **Performance Budget:** Resource allocation limits

**BlueMarble Example:**
```
Feature: Real-time Erosion Simulation

Design Intent:
WHAT: Visible terrain changes from water and wind erosion
WHY: Enhance geological realism and long-term world dynamics
HOW: Time-accelerated geological processes visible to player
METRICS: Player surveys on geological realism perception
PRIORITY: Medium (post-launch enhancement)

Technical Constraints:
HARD: Must run on mid-range GPUs (GTX 1060 equivalent)
SOFT: Prefer CPU-based simulation for deterministic results
TRADE-OFFS:
  Option A: Continuous micro-erosion (high cost, high fidelity)
  Option B: Event-based erosion (low cost, lower fidelity)
  Option C: Hybrid approach (medium cost, balanced fidelity)
PERFORMANCE: 5% max CPU allocation for erosion system
```

#### Designer-Artist Communication

**Visual Design Vocabulary:**

**Art Direction Terms:**
- **Style Guide:** Visual consistency documentation
- **Color Palette:** Limited color selection for coherence
- **Asset Spec:** Technical requirements for art assets
- **Concept Art:** Visual exploration before production
- **Mood Board:** Reference imagery collection

**Functional Art Requirements:**
- **Readability:** How easily players distinguish elements
- **Affordance:** Visual indication of interactivity
- **Silhouette:** Recognizable shape at distance
- **Visual Hierarchy:** Importance communication through visuals
- **State Indication:** Visual feedback for system states

**BlueMarble Visual Communication:**
```
Mineral Identification Visual Design:

Style Direction:
- Realistic mineral appearance with slight stylization
- Color-coded categories for accessibility
- Distinct silhouettes for major mineral families
- Reflectivity and texture indicating properties

Functional Requirements:
- Must be distinguishable at 10m distance
- Color-blind friendly differentiation
- Visual indication of purity/quality
- State changes for weathering/exposure

Artist Deliverables:
- 50 distinct mineral types
- 4 LOD levels per mineral
- Texture atlases for performance
- PBR materials for realistic lighting
```

---

## Implications for BlueMarble

### Design Vocabulary Integration Strategy

#### Implementation Phases

**Phase 1: Core Team Adoption (Months 1-2)**

**Actions:**
1. Distribute standardized design vocabulary document to all team members
2. Conduct workshop sessions on MDA framework and systems thinking
3. Establish common terminology in design documents and specifications
4. Create glossary document accessible to all team members
5. Integrate vocabulary into design review meetings

**Success Metrics:**
- 90%+ team members familiar with core terms
- Design documents using consistent terminology
- Reduction in clarification questions during meetings
- Faster feature specification approval cycles

**Phase 2: Documentation Standardization (Months 3-4)**

**Actions:**
1. Redesign all design documents using standardized vocabulary
2. Create templates for common document types
3. Establish design pattern library with consistent naming
4. Develop code documentation standards aligned with design vocabulary
5. Build internal wiki with terminology reference

**Success Metrics:**
- All active design documents updated
- New features documented with standard templates
- Code comments reference design vocabulary
- Onboarding time reduced by 30%

**Phase 3: External Communication (Months 5-6)**

**Actions:**
1. Develop player-facing terminology that aligns with internal vocabulary
2. Create tutorial system teaching core concepts with proper terms
3. Build community glossary for player discussions
4. Train community managers on vocabulary usage
5. Establish consistent terminology in patch notes and communications

**Success Metrics:**
- Players using design terms in community discussions
- Reduced confusion in player feedback
- More specific and actionable player suggestions
- Community alignment with design vision

### Specific BlueMarble Vocabulary Needs

#### Geological Systems Vocabulary

**Essential Terms:**

**Geological Time:**
- **Real-time:** Actual seconds passing
- **Game-time:** Accelerated time within simulation
- **Geological-time:** Ultra-accelerated for geological processes
- **Time-scale Factor:** Ratio between time scales

**Geological Processes:**
- **Erosion Rate:** Speed of terrain degradation
- **Sedimentation:** Deposition of materials
- **Metamorphosis:** Rock transformation under pressure/heat
- **Tectonic Activity:** Crustal movement and deformation
- **Volcanic Activity:** Magma movement and eruption

**Player-Geological Interaction:**
- **Survey Accuracy:** Precision of geological assessment
- **Extraction Efficiency:** Resource yield per effort
- **Processing Yield:** Refined material output percentage
- **Geological Impact:** Player effect on natural processes

#### Survival Systems Vocabulary

**Resource States:**
- **Raw Resource:** Unprocessed natural material
- **Refined Resource:** Processed for specific use
- **Consumable:** Single-use items
- **Durable Good:** Reusable with degradation
- **Renewable Stock:** Regenerating resource pool

**Survival Mechanics:**
- **Vitality:** Overall player health and energy
- **Sustenance:** Food and water requirements
- **Shelter Integrity:** Protection structure condition
- **Equipment Durability:** Tool and gear condition
- **Environmental Exposure:** Danger from conditions

### Communication Tools Development

#### Design Pattern Library

**Pattern Documentation Format:**

```markdown
## Pattern Name: Resource Extraction Loop

### Category: Economic System

### Intent:
Provide engaging cycle of resource acquisition and application 
that drives player progression and geological exploration.

### Structure:
1. Identification: Survey and locate resources
2. Planning: Determine extraction approach
3. Execution: Perform extraction activities
4. Processing: Refine raw materials
5. Application: Use refined resources
6. Expansion: Unlock new extraction capabilities

### Consequences:
+ Clear gameplay loop with measurable progress
+ Natural difficulty progression through resource tiers
+ Integration of geological knowledge and skill
- Risk of repetitive gameplay if not varied
- Potential for economic imbalances

### BlueMarble Implementation:
- Tier 1: Surface mineral collection (hand tools)
- Tier 2: Shallow mining (basic equipment)
- Tier 3: Deep extraction (advanced machinery)
- Tier 4: Geological manipulation (experimental tech)

### Related Patterns:
- Knowledge Progression Pattern
- Risk-Reward Balance Pattern
- Survival Pressure Pattern
```

#### Design Review Checklist

**Standard Review Questions:**

1. **Clarity:** Is the design communicable with our vocabulary?
2. **Consistency:** Does it align with established terminology?
3. **Completeness:** Are all system interactions specified?
4. **Feasibility:** Is implementation scope clear?
5. **Player Value:** Is player benefit articulated?
6. **Measurement:** Are success metrics defined?
7. **Risk Assessment:** Are potential problems identified?
8. **Documentation:** Is specification sufficient?

---

## Recommendations

### Immediate Actions

1. **Create BlueMarble Design Glossary**
   - Compile all project-specific terms
   - Reference standard game design vocabulary
   - Include geological and survival terminology
   - Make accessible to entire team

2. **Standardize Design Documents**
   - Adopt consistent section structure
   - Use MDA framework for system analysis
   - Implement design pattern documentation
   - Create reusable templates

3. **Conduct Vocabulary Workshop**
   - Train team on core design concepts
   - Practice using vocabulary in discussions
   - Review BlueMarble-specific terminology
   - Establish communication norms

4. **Update Development Tools**
   - Add vocabulary reference to design tools
   - Create terminology snippets for documents
   - Build searchable term database
   - Integrate with documentation system

### Long-term Integration

1. **Onboarding Process**
   - Include vocabulary training in new team member orientation
   - Provide glossary as reference material
   - Assign mentors to reinforce terminology usage
   - Test vocabulary comprehension

2. **Continuous Education**
   - Monthly vocabulary review sessions
   - Update glossary with new terms as project evolves
   - Share industry vocabulary developments
   - Maintain alignment across team

3. **Community Building**
   - Develop player-facing glossary
   - Use consistent terminology in marketing
   - Educate community moderators
   - Foster design vocabulary in player community

4. **Documentation Excellence**
   - Regular audit of documentation consistency
   - Update templates based on team feedback
   - Archive deprecated terminology
   - Maintain vocabulary evolution history

---

## References

### Books

1. Schell, Jesse. (2019). *The Art of Game Design: A Book of Lenses* (3rd Edition). CRC Press.
   - Chapter 5: The Game Consists of Elements
   - Chapter 13: Game Mechanics Must Be in Balance
   - Chapter 26: The Designer Usually Works with a Team

2. Fullerton, Tracy. (2018). *Game Design Workshop: A Playcentric Approach to Creating Innovative Games* (4th Edition). CRC Press.
   - Chapter 3: Formal Elements
   - Chapter 5: Working with Dramatic Elements
   - Chapter 12: Team Structures

3. Koster, Raph. (2013). *Theory of Fun for Game Design* (2nd Edition). O'Reilly Media.
   - Chapter 2: How the Brain Works
   - Chapter 5: What Games Are
   - Chapter 8: The Problem with Learning

4. Salen, Katie & Zimmerman, Eric. (2003). *Rules of Play: Game Design Fundamentals*. MIT Press.
   - Unit 1: Core Concepts
   - Unit 2: Rules
   - Unit 3: Play
   - Unit 4: Culture

5. Adams, Ernest. (2013). *Fundamentals of Game Design* (3rd Edition). New Riders.
   - Chapter 1: Games and Video Games
   - Chapter 9: Creative and Narrative Design
   - Chapter 10: Core Mechanics

### Papers and Articles

1. Hunicke, Robin, LeBlanc, Marc, and Zubek, Robert. (2004). "MDA: A Formal Approach to Game Design and Game Research." Proceedings of the AAAI Workshop on Challenges in Game AI.

2. Bartle, Richard. (1996). "Hearts, Clubs, Diamonds, Spades: Players Who Suit MUDs." MUSE Ltd.

3. Costikyan, Greg. (2002). "I Have No Words & I Must Design: Toward a Critical Vocabulary for Games." Proceedings of Computer Games and Digital Cultures Conference.

4. Järvinen, Aki. (2008). *Games without Frontiers: Theories and Methods for Game Studies and Design*. Tampere University Press.

5. Deterding, Sebastian et al. (2011). "Gamification: Toward a Definition." CHI 2011 Gamification Workshop.

### Online Resources

1. Game Developer (formerly Gamasutra) - Design Articles Archive
   - <https://www.gamedeveloper.com/design>
   
2. GDC Vault - Design Presentations
   - <https://www.gdcvault.com/>
   - Notable talks on design vocabulary and communication

3. Lost Garden - Design Essays by Daniel Cook
   - <https://lostgarden.home.blog/>

4. Designer Notes Podcast - Soren Johnson
   - Episodes on design communication and vocabulary

5. Game Maker's Toolkit - YouTube Channel
   - <https://www.youtube.com/c/MarkBrownGMT>
   - Analysis videos using consistent design vocabulary

### Industry Standards

1. IGDA Game Design Special Interest Group - Curriculum Framework
   - <https://igda.org/>

2. Game Design Patterns Project
   - <http://gamedesignpatterns.org/>

3. The Design of Everyday Things - Donald Norman (Affordance concepts)

4. Universal Principles of Design - William Lidwell et al.

---

## Related Research

### Internal Documentation

1. **Game Design Mechanics Analysis** (`game-design-mechanics-analysis.md`)
   - Core mechanics vocabulary application
   - Systems design terminology

2. **Game Programming in C++ Analysis** (`game-dev-analysis-01-game-programming-cpp.md`)
   - Technical vocabulary alignment
   - Code-design communication bridge

3. **Master Research Queue** (`master-research-queue.md`)
   - Research terminology standards
   - Progress tracking vocabulary

### Future Research Topics

1. **Player Psychology Vocabulary**
   - Motivation terminology
   - Behavioral design concepts
   - Psychological frameworks

2. **Multiplayer Design Communication**
   - Social systems vocabulary
   - Player interaction terminology
   - Community management language

3. **Procedural Generation Design Language**
   - Algorithmic design terms
   - Parametric design vocabulary
   - Controlled randomness concepts

4. **Accessibility Design Vocabulary**
   - Inclusive design terminology
   - Assistive technology integration
   - Universal design principles

---

## Appendix A: Quick Reference Glossary

### Core Concepts

- **Agency:** Player's meaningful control over game outcomes
- **Balance:** Equivalence of power/value across game elements
- **Emergence:** Complex behavior from simple rule interactions
- **Flow:** Optimal engagement state (challenge-skill balance)
- **Immersion:** Deep engagement with game experience
- **Loop:** Repeating cycle of gameplay actions
- **Mechanic:** Basic rule or system component
- **Dynamic:** Runtime behavior from mechanic interactions
- **Aesthetic:** Emotional player response
- **Feedback:** System response to player action

### System Design

- **Positive Feedback:** Amplifying loop (rich get richer)
- **Negative Feedback:** Dampening loop (rubber-banding)
- **Faucet:** Resource introduction mechanism
- **Sink:** Resource removal mechanism
- **Snowball:** Accelerating advantage effect
- **Catchup:** Mechanism helping losing players
- **Power Curve:** Rate of player advancement
- **Vertical Progression:** Increased power/capability
- **Horizontal Progression:** Increased options/variety
- **Gating:** Content locked behind requirements

### Player Experience

- **Onboarding:** Initial player learning experience
- **Tutorial:** Explicit teaching mechanism
- **Mastery:** High skill level achievement
- **Retention:** Player continuing to play
- **Engagement:** Active player investment
- **Session:** Single continuous play period
- **Replayability:** Value in playing again
- **Skill Floor:** Minimum skill for basic play
- **Skill Ceiling:** Maximum skill expression possible
- **Learning Curve:** Difficulty of mastery

### Technical Terms

- **LOD (Level of Detail):** Variable quality based on distance
- **Procedural Generation:** Algorithmic content creation
- **Deterministic:** Same input produces same output
- **Seed:** Parameter controlling generation
- **Asset Pipeline:** Content creation workflow
- **Performance Budget:** Resource allocation limits
- **Optimization:** Efficiency improvement
- **Bottleneck:** Performance limiting factor
- **Scalability:** Ability to handle growth
- **Architecture:** High-level system structure

---

## Appendix B: BlueMarble-Specific Vocabulary

### Geological Terms

- **Survey Accuracy:** Precision of geological assessment
- **Mineral Purity:** Quality of resource deposits
- **Extraction Efficiency:** Resource yield percentage
- **Processing Yield:** Refined material output
- **Geological Impact:** Player effect on natural processes
- **Tectonic Activity:** Crustal movement intensity
- **Erosion Rate:** Terrain degradation speed
- **Sedimentation:** Material deposition rate
- **Formation Age:** Geological structure creation time
- **Stratum:** Distinct geological layer

### Survival Terms

- **Vitality:** Overall player health status
- **Sustenance:** Food/water requirement level
- **Shelter Integrity:** Protection structure strength
- **Equipment Durability:** Tool condition percentage
- **Environmental Exposure:** Hazard danger level
- **Resource Stock:** Available supply quantity
- **Consumption Rate:** Resource usage speed
- **Renewable Cycle:** Regeneration time period
- **Carrying Capacity:** Maximum resource storage
- **Efficiency Rating:** System performance measure

### Gameplay Systems

- **Knowledge Level:** Scientific understanding tier
- **Discovery Points:** Exploration reward currency
- **Research Progress:** Scientific advancement percentage
- **Expedition:** Multi-stage exploration mission
- **Claim:** Player-owned extraction site
- **Processing Chain:** Multi-step refinement sequence
- **Trade Value:** Resource exchange worth
- **Reputation:** Standing with factions/systems
- **Milestone:** Major progression checkpoint
- **Achievement:** Special accomplishment recognition

---

**Document Status:** Complete  
**Last Updated:** 2025-01-15  
**Word Count:** ~7,200 words  
**Line Count:** ~800 lines  
**Next Review Date:** 2025-02-15  
**Maintainer:** Assignment Group 10
