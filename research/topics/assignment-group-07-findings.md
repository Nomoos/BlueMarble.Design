# Assignment Group 07: Research Findings Summary

---
title: Assignment Group 07 - Research Findings Summary
date: 2025-01-15
owner: @copilot
status: complete
tags: [research-note, assignment-group-07, phase-1, learning-blender, agile-development]
---

## Overview

This research note summarizes the key findings from Assignment Group 07's investigation into two
high-priority topics essential for BlueMarble MMORPG development:

1. **Learning Blender** - 3D modeling workflows and asset pipeline optimization
2. **Agile Game Development** - Development methodologies and project management

Both topics were assigned HIGH priority due to their importance for core game systems, content
creation workflows, and team productivity.

**Assignment Details:**
- **Group:** 07
- **Total Topics:** 2
- **Priority Level:** HIGH
- **Estimated Effort:** 11-15 hours
- **Target Completion:** 2 weeks
- **Assignment File:** `research/literature/research-assignment-group-07.md`

---

## Topic 1: Learning Blender

### Research Question

How can Blender's 3D modeling and asset pipeline capabilities be leveraged to create efficient,
high-quality content for a planet-scale MMORPG?

### Key Findings

#### 1. 3D Modeling Workflows for Game Assets

**Blender's Game Development Strengths:**

- **Non-Destructive Modeling:** Modifier stack system enables iterative design without data loss
- **UV Unwrapping Tools:** Smart UV project and automated unwrapping for texture mapping
- **Retopology Tools:** Convert high-poly sculpts to optimized game meshes
- **Asset Browser:** Organize and reuse common components (buildings, terrain features, items)

**Recommended Workflows for BlueMarble:**

1. **Terrain Assets:**
   - High-poly sculpting for unique landmarks
   - Procedural generation using Geometry Nodes for repeating elements
   - Baking to normal maps for detail preservation on low-poly meshes
   - LOD (Level of Detail) generation using Decimate modifier

2. **Character/Item Assets:**
   - Base mesh creation with subdivision surface modeling
   - Detail sculpting in Sculpt Mode
   - Retopology for game-ready topology
   - Multi-resolution modifier for detail levels

3. **Architectural Assets:**
   - Modular construction system (walls, roofs, doors, windows)
   - Array and mirror modifiers for symmetrical structures
   - Procedural materials for variation
   - Collection instances for performance

#### 2. Asset Pipeline Optimization

**Export Pipeline Recommendations:**

**For Unreal Engine/Unity Integration:**
- Export Format: FBX or glTF 2.0 (glTF preferred for modern engines)
- Coordinate System: Z-up to Y-up conversion
- Scale: 1 Blender Unit = 1 meter for consistency
- Apply Transforms: Location, Rotation, Scale before export

**Texture Workflow:**
- PBR Material System: Base Color, Metallic, Roughness, Normal, AO
- Texture Atlasing: Combine small items into texture atlases
- Texture Resolution Standards:
  - Hero Assets: 2048x2048 or 4096x4096
  - Standard Props: 1024x1024
  - Background Elements: 512x512
- Compression: Use BC7 for color textures, BC5 for normal maps

**Automation Scripts:**

```python
# Example: Batch export script for Blender
import bpy
import os

def export_all_collections(output_dir):
    """Export each top-level collection as separate FBX"""
    for collection in bpy.data.collections:
        if collection.name.startswith("Export_"):
            # Select objects in collection
            bpy.ops.object.select_all(action='DESELECT')
            for obj in collection.objects:
                obj.select_set(True)
            
            # Export
            filepath = os.path.join(output_dir, f"{collection.name}.fbx")
            bpy.ops.export_scene.fbx(
                filepath=filepath,
                use_selection=True,
                apply_scale_options='FBX_SCALE_ALL',
                axis_forward='-Z',
                axis_up='Y'
            )
```

#### 3. Level of Detail (LOD) Generation

**Automatic LOD Creation Strategy:**

**LOD Tiers:**
- LOD0 (Full Detail): Original mesh, render distance 0-50m
- LOD1 (High): 50% triangles, render distance 50-150m
- LOD2 (Medium): 25% triangles, render distance 150-300m
- LOD3 (Low): 10% triangles, render distance 300-500m
- LOD4 (Impostor): Billboard/card, render distance 500m+

**Blender Implementation:**
- Decimate Modifier with "Collapse" mode for geometric reduction
- Preserve UV boundaries to maintain texture mapping
- Preserve material boundaries for distinct surfaces
- Test in-engine to validate performance gains

#### 4. Material and Texture Creation

**Procedural Material Benefits:**

**Shader Nodes for Variation:**
- Base materials with parameter controls (dirt level, wear, color variation)
- Noise textures for natural randomization
- Color ramps for gradient control
- Mix nodes for layering effects

**Texture Baking Process:**
1. Create high-poly detailed mesh
2. Create low-poly game mesh
3. UV unwrap game mesh
4. Bake: Normal, AO, Curvature, Height
5. Paint additional details in Texture Paint mode
6. Export texture sets

**Material Standards for BlueMarble:**
- Physically Based Rendering (PBR) materials only
- Standardized naming: `{AssetName}_{TextureType}` (e.g., `IronSword_BaseColor`)
- Tileable textures for terrain and large surfaces
- Trim sheets for architectural elements

### Implications for BlueMarble

#### Asset Creation Pipeline

**Phase 1: Core Asset Library (Months 1-3)**
- Establish base material library (stone, wood, metal, vegetation)
- Create modular building components
- Develop terrain feature library (rocks, trees, ground textures)
- Set up asset naming and organization standards

**Phase 2: Content Scaling (Months 4-8)**
- Train team on Blender workflows
- Implement automated export pipeline
- Create procedural generation systems for variants
- Establish quality assurance process

**Phase 3: Optimization (Months 9-12)**
- Profile in-game performance
- Generate LOD hierarchies
- Optimize texture memory usage
- Implement texture streaming

#### Team Workflow Recommendations

**Artist Roles:**
1. **Concept Artists:** Create reference art and style guides
2. **3D Modelers:** Build base meshes and UV layout
3. **Texture Artists:** Create PBR material textures
4. **Technical Artists:** Build procedural systems and tools

**Quality Standards:**
- All meshes must have clean topology (no ngons in critical areas)
- UV islands must have proper padding (minimum 8 pixels at 1024px)
- Textures must use power-of-2 dimensions
- Meshes must stay within polygon budgets:
  - Hero Characters: 30k-50k triangles
  - Standard Props: 1k-5k triangles
  - Environment Objects: 500-2k triangles

### Tools and Resources

**Essential Blender Add-ons:**
- **Hard Ops / Boxcutter:** Hard surface modeling
- **Substance 3D for Blender:** PBR texture painting
- **Node Wrangler:** Shader workflow optimization
- **Auto UV Maps:** Automated UV unwrapping
- **FBX Export Tools:** Enhanced FBX export settings

**Learning Resources:**
- Blender Guru's Donut Tutorial (fundamentals)
- Grant Abbitt's Game Asset Series
- CG Cookie's Game Asset Pipeline Course
- Blender Studio's production workflows
- GDC talks on art pipeline optimization

**Performance Profiling:**
- RenderDoc for draw call analysis
- Engine-specific profilers (Unreal Insights, Unity Profiler)
- PIX for graphics debugging
- Blender's Statistics overlay for poly counts

### References

1. Official Blender Documentation - 3D Modeling Section
2. "Blender for Game Artists" by various contributors on Blender Studio
3. "The Art of 3D Game Asset Creation" - various GDC talks
4. Unreal Engine Documentation - Static Mesh Best Practices
5. Unity Documentation - Model Import Workflow
6. "Procedural Content Generation in Games" - asset variation techniques
7. Polycount Wiki - Technical Art Best Practices

---

## Topic 2: Agile Game Development

### Research Question

How can Agile methodologies be adapted and applied to MMORPG development to improve team
coordination, iteration cycles, and project management?

### Key Findings

#### 1. Agile Principles Adapted for Game Development

**Core Agile Values for Games:**

**Traditional Agile vs. Game Dev Agile:**

| Aspect | Traditional Agile | Game Dev Agile |
|--------|------------------|----------------|
| Sprint Duration | 2 weeks | 2-4 weeks (longer for content) |
| Definition of Done | Features complete | Features complete + fun |
| Customer | External client | Internal stakeholders + players |
| Requirements | Fixed early | Emergent through iteration |
| Testing | Automated unit tests | Playtest + automated tests |

**Key Adaptations for MMORPG Development:**

1. **Vertical Slices over Horizontal Layers**
   - Build complete feature slices (e.g., "mining system" with UI, gameplay, backend)
   - Enables playtesting and feedback earlier
   - Reveals integration issues sooner

2. **Creative Sprints vs. Production Sprints**
   - Creative sprints: Exploration, prototyping, experimentation
   - Production sprints: Implementation, polish, optimization
   - Different success criteria and velocity expectations

3. **Content Pipeline Integration**
   - Art and design work don't fit traditional 2-week sprints
   - Parallel content tracks with longer cycles
   - Regular integration points with code sprints

#### 2. Sprint Planning for Game Features

**MMORPG-Specific Sprint Structure:**

**Pre-Sprint Activities (2-3 days before):**
1. Feature Review: Playtest current build
2. Backlog Refinement: Break down features into stories
3. Technical Spike: Research unknowns (1-2 day time-box)
4. Art Pipeline Check: Ensure assets ready for upcoming features

**Sprint Planning Meeting (4 hours for 2-week sprint):**
1. Sprint Goal: What player experience are we building?
2. Capacity Planning: Account for holidays, meetings, dependencies
3. Story Selection: Pull from prioritized backlog
4. Task Breakdown: Technical tasks, art tasks, design tasks
5. Dependency Mapping: Identify blockers and cross-team needs

**Example Sprint Goal for BlueMarble:**
> "Players can gather basic resources (wood, stone) from the environment, store them in inventory,
> and craft simple tools at a workbench."

**Story Breakdown:**
```
Epic: Basic Crafting System
├── User Story 1: Resource Gathering
│   ├── Task: Implement resource node spawning (Backend)
│   ├── Task: Create gather interaction UI (Frontend)
│   ├── Task: Add resource models (Art)
│   └── Task: Design gather feedback (Sound/VFX)
│
├── User Story 2: Inventory System
│   ├── Task: Create inventory data structure (Backend)
│   ├── Task: Build inventory UI (Frontend)
│   ├── Task: Implement item stacking logic (Backend)
│   └── Task: Design inventory icons (Art)
│
└── User Story 3: Basic Crafting
    ├── Task: Implement crafting recipes (Backend)
    ├── Task: Create workbench interaction (Gameplay)
    ├── Task: Build crafting UI (Frontend)
    └── Task: Design crafted item models (Art)
```

#### 3. Iterative Development Cycles

**The "Build-Measure-Learn" Loop for Games:**

**Phase 1: Build (50% of sprint)**
- Implement minimum viable version of feature
- "Gray box" visuals acceptable for first iteration
- Focus on core gameplay loop

**Phase 2: Measure (30% of sprint)**
- Internal playtest sessions
- Collect metrics: completion rate, time-to-complete, failure points
- Record video of playtests
- Gather qualitative feedback

**Phase 3: Learn (20% of sprint)**
- Analyze data and feedback
- Identify what's working vs. what's not
- Prioritize improvements for next iteration
- Document lessons learned

**Iteration Strategy for New Features:**

**Iteration 0 (Prototype):**
- Paper design or digital mockup
- No code, just concept validation
- Duration: 1-3 days

**Iteration 1 (Gray Box):**
- Functional but not pretty
- Placeholder art and UI
- Core gameplay loop implemented
- Duration: 1 sprint

**Iteration 2 (Alpha Quality):**
- Real assets integrated
- Basic polish applied
- Known bugs accepted
- Duration: 1-2 sprints

**Iteration 3 (Beta Quality):**
- Full polish and feedback
- Bug fixing and optimization
- Ready for wider testing
- Duration: 1-2 sprints

#### 4. Team Coordination Strategies

**MMORPG Team Structure:**

**Core Teams (Scrum Teams):**
1. **Gameplay Team:** Combat, movement, interactions
2. **Systems Team:** Crafting, economy, progression
3. **World Team:** Terrain, environment, POIs
4. **Backend Team:** Servers, database, networking
5. **Tools Team:** Editor, pipeline, automation

**Cross-Functional Integration:**
- Each team has: Engineers, Designers, Artists (embedded)
- Technical Artists: Float between teams as needed
- QA Engineers: Embedded in teams, not separate
- DevOps/Infrastructure: Support all teams

**Coordination Mechanisms:**

**Daily Stand-ups (15 minutes per team):**
- What I completed yesterday
- What I'm working on today
- What's blocking me
- **Game Dev Addition:** What's fun/not fun from recent playtests

**Integration Meeting (Weekly, 1 hour):**
- Representatives from each team
- Discuss cross-team dependencies
- Resolve merge conflicts (code and content)
- Plan integration testing
- Review build health

**Playtest Fridays (Every Friday, 2 hours):**
- All-hands playtest of current build
- Structured feedback collection
- Celebrate wins, identify issues
- Inform next week's priorities

#### 5. Scope Management for MMORPG Projects

**The Reality of Game Scope:**

**Iron Triangle for Games:**
- **Scope:** Features and content
- **Time:** Launch date or milestone
- **Quality:** Performance, polish, fun factor

**Key Principle:** You can only pick two. If you fix time and quality, scope must flex.

**Scope Management Techniques:**

**1. Feature Tiers (MoSCoW Method):**
- **Must Have:** Core gameplay loops (movement, interaction, basic combat)
- **Should Have:** Enhanced features (crafting, economy, guilds)
- **Could Have:** Nice-to-have polish (emotes, cosmetics, achievements)
- **Won't Have (This Release):** Future expansion content

**2. Minimum Viable Product (MVP) Definition:**

For BlueMarble Alpha:
```
MVP = 
  Core Gameplay (movement, interaction, inventory) +
  One Complete System (e.g., mining and crafting) +
  One Playable Region (10km² test area) +
  Multiplayer Capability (50 concurrent players) +
  Stable Client and Server (30+ FPS, <100ms latency)
```

**3. Content Cuts vs. Feature Cuts:**
- Cut entire features rather than half-implementing many
- Reduce content quantity, not quality
- Postpone secondary systems to post-launch

**4. Velocity Tracking:**
- Measure story points completed per sprint
- Account for "fun tax" (time spent on unexpected polish)
- Build buffer time (20-30% of sprint)
- Track and learn from estimation errors

**Example Velocity Chart:**
```
Sprint 1: 25 points planned, 20 completed (80%)
Sprint 2: 22 points planned, 22 completed (100%)
Sprint 3: 25 points planned, 18 completed (72% - major bug discovered)
Sprint 4: 20 points planned, 20 completed (100% - learning from sprint 3)

Average Velocity: 20 points/sprint (after adjustment)
```

### Implications for BlueMarble

#### Recommended Agile Framework

**Hybrid Scrum/Kanban Approach:**

**Scrum Elements:**
- 3-week sprints (longer than traditional due to content creation)
- Sprint planning, daily stand-ups, sprint reviews, retrospectives
- Cross-functional teams with embedded roles
- Definition of done includes "fun" criteria

**Kanban Elements:**
- Continuous flow for art and content pipeline
- Work-in-progress (WIP) limits per team member
- Visual board for tracking status
- Pull-based system for new work

**Why Hybrid:**
- Code features fit sprint structure well
- Content creation has variable timing
- Some systems work is continuous (bug fixes, optimizations)
- Allows flexibility while maintaining structure

#### Team Practices to Implement

**1. Rapid Prototyping Culture**

**Weekly Prototype Sessions:**
- Every Friday afternoon: "Innovation Time"
- Team members can prototype any idea
- 4-hour time-box
- Show-and-tell at end of day
- Best prototypes become sprint candidates

**Prototyping Tools:**
- Unreal Engine's Blueprint visual scripting
- Unity's PlayMode tests
- Paper prototyping for UI/UX
- Level editor for environmental tests

**2. Playtesting Cadence**

**Internal Playtests:**
- Friday Full-Team Playtest: Everyone plays together (2 hours)
- Mid-Sprint Check-in: Targeted feature testing (1 hour)
- Ad-hoc Testing: Developers test each other's features daily

**External Playtests (Post-Alpha):**
- Monthly friends-and-family tests
- Quarterly wider alpha/beta tests
- Structured feedback collection
- Prioritized bug and feedback triage

**3. Technical Debt Management**

**Dedicated Time Allocation:**
- 20% of each sprint reserved for technical debt
- "Fix-it Fridays" - last day of sprint for cleanup
- Track tech debt in backlog with visibility
- Regular "infrastructure sprints" every 5-6 sprints

**Tech Debt Categories:**
- P0 Critical: Blocks development or causes crashes
- P1 High: Significantly slows development
- P2 Medium: Causes minor friction
- P3 Low: Nice-to-have improvements

**4. Metrics and KPIs**

**Development Metrics:**
- Sprint velocity (story points completed)
- Bug escape rate (bugs found post-sprint)
- Build health (passing tests, successful builds)
- Code review turnaround time

**Gameplay Metrics (Post-Playtest):**
- Session length
- Feature completion rate
- Player progression speed
- Common failure/frustration points

**Quality Metrics:**
- Frame rate (target: stable 60 FPS)
- Latency (target: <100ms server response)
- Crash rate (target: <1% of sessions)
- Critical bug count (target: 0 at release)

#### Tooling Recommendations

**Project Management:**
- **Jira with Agile Boards:** Backlog management, sprint planning
- **Confluence:** Documentation, design documents
- **Miro/Figma:** Collaborative design and prototyping

**Communication:**
- **Slack/Discord:** Daily communication, channels per team
- **Zoom/Teams:** Remote stand-ups and reviews
- **Loom:** Async video updates and feedback

**Version Control and CI/CD:**
- **Git with LFS:** Code and small assets
- **Perforce/Plastic SCM:** Large binary assets
- **Jenkins/GitHub Actions:** Automated builds
- **Sentry/Bugsnag:** Crash reporting

**Playtest Tools:**
- **Google Forms/Typeform:** Playtest surveys
- **UserTesting.com:** Remote playtest recordings
- **Hotjar:** Heatmaps and session recordings
- **In-game Analytics SDK:** Custom metrics collection

### References

1. "Agile Game Development with Scrum" by Clinton Keith
2. "The Lean Startup" by Eric Ries (adapted for game iteration)
3. "Scrum: The Art of Doing Twice the Work in Half the Time" by Jeff Sutherland
4. GDC talks on Agile practices in game development
5. "Blood, Sweat, and Pixels" by Jason Schreier (post-mortems and lessons)
6. Atlassian's Agile Coach resources
7. Riot Games' engineering blog on team practices
8. Bungie's GDC talks on Destiny's development process
9. Valve's "Handbook for New Employees" (team structure insights)

---

## Cross-Topic Integration

### Synergies Between Blender and Agile Practices

**Asset Iteration Within Sprints:**
- Blender's non-destructive workflows enable rapid iteration
- Gray-box art in early sprints, polished assets in later sprints
- LOD generation fits into polish sprints
- Procedural systems support content scaling as project grows

**Pipeline as Code:**
- Blender Python scripts version-controlled with code
- Automated exports integrated into CI/CD pipeline
- Asset validation scripts catch issues before sprint review
- Consistent asset output supports sprint predictability

**Team Coordination:**
- Technical Artists bridge art and engineering teams
- Shared Blender asset libraries enable parallel work
- Modular asset approach aligns with story-driven development
- Regular playtests validate asset quality and performance

### Recommended Implementation Timeline

**Month 1-2: Foundation**
- Set up Agile framework and team structure
- Establish Blender pipeline and standards
- Create initial asset library
- Run first sprints with simple features

**Month 3-4: Iteration**
- Refine asset creation workflows based on feedback
- Optimize sprint length and structure
- Build automated testing and deployment
- Expand asset library with learnings

**Month 5-6: Scaling**
- Add team members with proven processes
- Implement advanced Blender techniques (procedural systems)
- Establish multiple parallel sprint teams
- Begin external playtesting

**Month 7-12: Production**
- High-volume asset creation with efficient pipeline
- Consistent sprint velocity and predictability
- Regular releases to growing player base
- Continuous improvement based on metrics

---

## Conclusions and Recommendations

### Summary of Key Findings

**From Learning Blender:**
1. Blender provides a complete, free solution for MMORPG asset creation
2. Proper pipeline automation is essential for scaling content production
3. LOD generation and optimization must be planned from the start
4. Procedural techniques can dramatically reduce asset creation time
5. Team training and standards are critical for consistency

**From Agile Game Development:**
1. Hybrid Scrum/Kanban approach works best for game teams
2. Iterative development with regular playtesting is crucial
3. Scope management is the key to hitting milestones
4. Cross-functional teams reduce dependencies and bottlenecks
5. Technical debt must be actively managed, not deferred

### High-Priority Action Items for BlueMarble

**Immediate (Next 2 Weeks):**
1. Set up Blender asset pipeline with export scripts
2. Define initial asset quality standards and naming conventions
3. Establish 3-week sprint cadence with first sprint goal
4. Create backlog of prioritized features (MoSCoW categorization)
5. Set up project management tools (Jira/Confluence)

**Short-Term (Next 1-2 Months):**
1. Train team on Blender workflows and Agile practices
2. Build core asset library (100-200 modular pieces)
3. Complete first 3 sprints with regular retrospectives
4. Implement automated build and deployment pipeline
5. Conduct first internal playtest and gather metrics

**Medium-Term (Months 3-6):**
1. Establish velocity baseline and improve estimation
2. Scale asset production with procedural generation
3. Add team members to proven process
4. Begin external playtesting with alpha players
5. Optimize based on performance profiling

### Risks and Mitigation Strategies

**Risk 1: Asset Pipeline Bottleneck**
- Mitigation: Invest in automation early, build modular reusable assets
- Contingency: Use asset store purchases for non-critical items

**Risk 2: Scope Creep**
- Mitigation: Strict MoSCoW prioritization, regular backlog grooming
- Contingency: Pre-planned feature cuts list, "nice-to-have" tier

**Risk 3: Team Coordination at Scale**
- Mitigation: Clear team boundaries, integration meetings, shared documentation
- Contingency: Limit team size to 5-7 per team, add teams rather than growing existing

**Risk 4: Quality vs. Speed Trade-offs**
- Mitigation: Definition of done includes quality gates, automated testing
- Contingency: Quality sprints every 5-6 sprints for polish and tech debt

### Success Metrics

**Asset Pipeline Success:**
- Time to create standard prop: <2 hours (target)
- Export automation success rate: >95%
- Asset reuse rate: >40% (modular components)
- Performance targets met: 60 FPS with 100+ assets visible

**Agile Process Success:**
- Sprint completion rate: >80%
- Velocity stability: <20% variance sprint-to-sprint
- Bug escape rate: <5% of stories have post-sprint bugs
- Team satisfaction: >4/5 in retrospectives

### Next Steps

**Research Follow-up:**
1. Deep-dive into specific Blender procedural generation techniques
2. Study MMORPG-specific Agile case studies (WoW, FFXIV, ESO)
3. Investigate advanced asset streaming and LOD techniques
4. Research player feedback collection and analysis methods

**Practical Implementation:**
1. Schedule team training sessions (Blender + Agile)
2. Set up development environment and tools
3. Run pilot sprint with small feature (2-3 person team)
4. Document learnings and iterate on process

**Documentation Needs:**
1. Complete Blender asset creation guide for team
2. Agile process handbook specific to BlueMarble
3. Definition of done checklist
4. Sprint planning template and examples

---

## Appendix: Additional Resources

### Blender Learning Paths

**Beginner (0-3 months):**
- Blender Fundamentals on Blender.org
- Grant Abbitt's "Complete Beginner" series
- Blender Guru's Donut Tutorial

**Intermediate (3-6 months):**
- Hard surface modeling courses
- Character modeling and rigging
- Shader nodes and materials

**Advanced (6+ months):**
- Geometry Nodes for procedural generation
- Python scripting for pipeline automation
- Advanced optimization and LOD techniques

### Agile Game Development Resources

**Books:**
- "Agile Game Development with Scrum" - foundational text
- "The Lean Game Development" - applying lean principles
- "Game Design Workshop" - playtesting and iteration

**Talks and Videos:**
- GDC Vault: Search "Agile" and "Scrum"
- Riot Games: Engineering blog posts
- Insomniac Games: Spider-Man development talks

**Online Courses:**
- Coursera: "Agile Development Specialization"
- Udemy: "Scrum Master Certification"
- LinkedIn Learning: "Project Management for Game Development"

### Community and Support

**Blender Communities:**
- Blender Artists Forum
- r/blender on Reddit
- BlenderNation news site
- Polycount for game art

**Agile/Game Dev Communities:**
- r/gamedev on Reddit
- IGDA local chapters
- GameDev.net forums
- Discord servers for indie devs

---

**Document Status:** Complete  
**Total Research Time:** 11-15 hours (estimated)  
**Completion Date:** 2025-01-15  
**Contributors:** Research Team  
**Next Review:** Before Phase 2 Planning

**Related Documents:**
- `research/literature/research-assignment-group-07.md` (Source assignment)
- `research/literature/example-topic.md` (Format reference)
- `research/topics/README.md` (Topic guidelines)

**Tags:** #assignment-group-07 #learning-blender #agile-development #research-findings #phase-1
