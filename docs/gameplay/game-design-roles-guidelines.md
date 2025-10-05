# BlueMarble - Game Design Roles and Responsibilities Guidelines

**Version:** 1.0  
**Date:** 2025-01-20  
**Author:** BlueMarble Design Team  
**Based on:** [Game Design Roles and Types Research](../../research/topics/game-design-roles-and-types.md)

---

## Purpose

This document provides comprehensive guidelines for structuring game design responsibilities within the BlueMarble project. It defines clear ownership areas, collaboration patterns, tools standards, and workflows for each design specialization to ensure efficient, high-quality game development.

**Target Audience:**
- Design team members understanding their roles and responsibilities
- Project managers structuring design workflows
- Hiring managers defining role requirements
- Cross-functional teams collaborating with designers

---

## Design Philosophy

### Core Principles

#### Specialization with Integration
- **Deep Expertise:** Each designer develops deep expertise in their specialization
- **Collaborative Design:** All designers collaborate on integrated systems
- **Shared Ownership:** Major features require input from multiple specializations
- **Cross-Training:** Designers understand adjacent specializations for better collaboration

#### Data-Informed, Player-Focused
- **Metrics Matter:** Use data to validate design decisions
- **Player Feedback:** Incorporate playtesting and user research
- **Iterative Approach:** Design through rapid prototyping and testing
- **Evidence-Based:** Document rationale for major design decisions

#### Documentation and Knowledge Sharing
- **Design Documents:** All major systems require written documentation
- **Decision Records:** Document why choices were made
- **Knowledge Base:** Maintain searchable design knowledge
- **Onboarding Materials:** Help new team members understand systems

---

## Design Role Structure

### Role Hierarchy and Reporting

```
Lead Game Designer
├── Systems Design Lead
│   ├── Core Systems Designer
│   └── Economy Designer
├── Content Design Lead
│   ├── Level Designer
│   ├── Combat Designer (if applicable)
│   └── Narrative Designer
└── Player Experience Lead
    ├── UX Designer
    ├── Progression Designer
    └── Meta-Game Designer
```

### Role Distribution by Project Phase

#### Pre-Production (Months 1-3)
- **Lead Game Designer:** Vision, core pillars, high-level systems
- **Systems Designer:** Core mechanics prototyping
- **UX Designer:** Interface concepts and user flows
- **Economy Designer:** Economic modeling and simulation

#### Production (Months 4-12)
- **All Roles Active:** Full team implementation
- **Level Designer:** Primary content creation role
- **Narrative Designer:** Quest and story content
- **Progression Designer:** Tuning advancement curves

#### Live Operations (Post-Launch)
- **Meta-Game Designer:** Seasonal content and events
- **Economy Designer:** Monitoring and balancing
- **UX Designer:** Interface improvements
- **All Roles:** Content updates and expansions

---

## Detailed Role Guidelines

### 1. Systems Designer - Core Mechanics and Simulation

#### Primary Responsibilities

**Design Ownership:**
- Core gameplay mechanics and interaction systems
- Mathematical models for game simulation
- Crafting and material processing systems
- Skill progression formulas and curves
- Resource generation and consumption mechanics
- Geological simulation parameters (BlueMarble-specific)

**Documentation Requirements:**
- System design documents with mathematical models
- Balance spreadsheets with formulas and parameters
- Prototyping specifications for engineering
- Integration requirements for other systems

**Key Deliverables:**
- Excel/Google Sheets balance models
- System design specifications
- Prototype implementations in Unity/Unreal
- Balance tuning documentation
- Integration test plans

#### Collaboration Patterns

**Works Closely With:**
- **Economy Designer:** Resource balance and economic impact
- **Progression Designer:** Skill advancement integration
- **Engineering:** Technical implementation and performance
- **Level Designer:** Terrain generation and geological accuracy

**Communication Requirements:**
- Daily: Engineering team on implementation
- Weekly: Design team on system integration
- Bi-weekly: Playtesting sessions and balance reviews
- Monthly: Stakeholder presentations on system health

#### Required Tools and Skills

**Essential Tools:**
- **Excel/Google Sheets:** Mathematical modeling and balance tuning
- **Unity/Unreal Engine:** Prototyping and testing
- **Python/R:** Advanced simulation and statistical analysis
- **Git:** Version control for design parameters
- **Confluence/Notion:** Design documentation

**Required Skills:**
- Strong mathematical and analytical thinking
- Programming knowledge (scripting level minimum)
- Systems thinking and interconnected design
- Statistical analysis and data interpretation
- Clear technical writing

**Nice to Have:**
- Game engine programming experience
- Database and SQL knowledge
- Machine learning for simulation
- Physics and scientific simulation background

#### Success Metrics

**Quantitative:**
- System balance scores (variance in player strategies)
- Player engagement with core mechanics (usage rates)
- Bug rates related to system edge cases
- Performance metrics (simulation efficiency)

**Qualitative:**
- Player feedback on system depth and fairness
- Designer satisfaction with system extensibility
- Engineering feedback on implementation clarity
- Playtester understanding of systems

#### Decision Authority

**Full Authority:**
- Mathematical formulas and balance parameters
- System rules and core mechanics design
- Prototype implementation approaches

**Shared Authority:**
- UI/UX impact of systems (with UX Designer)
- Economic balance (with Economy Designer)
- Narrative integration (with Narrative Designer)

**Requires Approval:**
- Major system redesigns affecting multiple areas
- Changes to core gameplay pillars
- Performance-impacting system changes

---

### 2. Economy Designer - Resource Balance and Player Markets

#### Primary Responsibilities

**Design Ownership:**
- In-game economy design and balance
- Currency systems and exchange rates
- Crafting material costs and values
- Resource sink and faucet design
- Player trading and marketplace systems
- Guild economics and territory taxation (BlueMarble-specific)

**Documentation Requirements:**
- Economic models with supply/demand curves
- Resource flow diagrams (sources, sinks, conversions)
- Market simulation results
- Inflation prevention strategies
- Monetization documentation (if applicable)

**Key Deliverables:**
- Economic balance spreadsheets
- Resource flow visualizations
- Market simulation reports
- Economy health dashboards
- Price adjustment recommendations

#### Collaboration Patterns

**Works Closely With:**
- **Systems Designer:** Resource generation mechanics
- **Progression Designer:** Reward pacing and value
- **Meta-Game Designer:** Seasonal economy events
- **Analytics Team:** Economic data analysis

**Communication Requirements:**
- Daily: Data analysis and monitoring
- Weekly: Cross-team balance reviews
- Bi-weekly: Economic health reports
- Monthly: Long-term economy strategy sessions

#### Required Tools and Skills

**Essential Tools:**
- **Excel/Google Sheets:** Economic modeling and analysis
- **SQL/Database Tools:** Player economy data queries
- **Tableau/Power BI:** Economic trend visualization
- **Python/R:** Monte Carlo simulations and modeling
- **Game Analytics:** Transaction tracking and wealth distribution

**Required Skills:**
- Economic theory and market dynamics
- Data analysis and statistical modeling
- Understanding of player psychology and spending
- Long-term strategic thinking
- Clear data visualization

**Nice to Have:**
- Background in economics or finance
- Game monetization experience
- Behavioral economics knowledge
- Machine learning for prediction

#### Success Metrics

**Quantitative:**
- Inflation/deflation rates within target ranges
- Wealth distribution (Gini coefficient)
- Market liquidity and transaction volumes
- Resource sink/faucet balance ratios
- Currency velocity and circulation

**Qualitative:**
- Player satisfaction with economy fairness
- Market activity and trading engagement
- Perception of value and worth
- Economic exploit reports

#### Decision Authority

**Full Authority:**
- Resource costs and values
- Currency exchange rates
- Economic sink design
- Market system rules

**Shared Authority:**
- Reward values (with Progression Designer)
- Crafting recipes (with Systems Designer)
- Monetization strategy (with Business Team)

**Requires Approval:**
- Major economic rebalancing
- New currency introduction
- Marketplace system changes

---

### 3. Level Designer - World Creation and Spatial Design

#### Primary Responsibilities

**Design Ownership:**
- Planetary terrain generation and biome placement
- Settlement layouts and urban planning
- Mining sites and underground structures
- Points of interest and landmark placement
- Navigation challenges and geographic obstacles
- Environmental storytelling through world design

**Documentation Requirements:**
- Level design specifications with maps
- Biome distribution guidelines
- Landmark placement rationale
- Navigation flow documentation
- Environmental storytelling notes

**Key Deliverables:**
- Terrain generation parameters
- Biome and region specifications
- Settlement and city layouts
- Point of interest placements
- Navigation path analysis
- Visual reference materials

#### Collaboration Patterns

**Works Closely With:**
- **Systems Designer:** Geological simulation integration
- **Narrative Designer:** Environmental storytelling
- **Art Team:** Visual asset requirements
- **Engineering:** Terrain generation implementation

**Communication Requirements:**
- Daily: Art and engineering coordination
- Weekly: Design reviews and feedback
- Bi-weekly: Playtesting spatial navigation
- Monthly: World expansion planning

#### Required Tools and Skills

**Essential Tools:**
- **Unity/Unreal Level Editors:** Primary design environment
- **World Creator/Gaea:** Terrain generation and heightmaps
- **Blender (basic):** Whiteboxing and layout visualization
- **Tiled/ProBuilder:** Rapid prototyping
- **Paper/Whiteboard:** Initial layout sketching

**Required Skills:**
- 3D spatial reasoning
- Understanding of player movement and flow
- Basic 3D modeling for whiteboxing
- Color theory and composition
- Environmental storytelling techniques

**Nice to Have:**
- Architecture or urban planning background
- Geology or geography knowledge (BlueMarble-specific)
- Photography and composition skills
- Traditional art training

#### Success Metrics

**Quantitative:**
- Player navigation efficiency (time to objectives)
- Area exploration rates
- Death rates in navigational hazards
- Player density heatmaps

**Qualitative:**
- Player feedback on world beauty and interest
- Discovery moments and memorable locations
- Navigation clarity and wayfinding
- Environmental storytelling effectiveness

#### Decision Authority

**Full Authority:**
- Terrain layout and topography
- Landmark placement
- Navigation path design

**Shared Authority:**
- Biome resource distribution (with Systems/Economy)
- Quest location placement (with Narrative)
- Visual atmosphere (with Art Director)

**Requires Approval:**
- Major geographical changes
- New biome introductions
- Performance-heavy terrain features

---

### 4. UX Designer - Interface and Player Experience

#### Primary Responsibilities

**Design Ownership:**
- User interface design and information architecture
- Complex data visualization (geological surveys, materials)
- Inventory and crafting interface systems
- Map and navigation UI
- Tutorial design and onboarding flows
- Accessibility features and options

**Documentation Requirements:**
- UI specifications with wireframes
- User flow diagrams
- Usability test reports
- Accessibility compliance documentation
- Interface style guide

**Key Deliverables:**
- UI mockups and prototypes
- User flow diagrams
- Usability test plans and reports
- Interface implementation specifications
- Accessibility audit reports

#### Collaboration Patterns

**Works Closely With:**
- **All Designers:** Translating systems into interfaces
- **Art Team:** Visual design implementation
- **Engineering:** Technical UI implementation
- **QA Team:** Usability testing and feedback

**Communication Requirements:**
- Daily: Design and engineering coordination
- Weekly: Cross-functional design reviews
- Bi-weekly: Usability testing sessions
- Monthly: Accessibility reviews

#### Required Tools and Skills

**Essential Tools:**
- **Figma/Adobe XD:** UI mockups and prototypes
- **InVision/Axure:** Interactive prototypes
- **Unity/Unreal UI Systems:** Implementation testing
- **Analytics Tools:** User behavior tracking
- **User Testing Platforms:** Remote usability testing

**Required Skills:**
- Information architecture and hierarchy
- Visual design and typography
- User research and testing methodologies
- Accessibility standards (WCAG)
- Interaction design principles

**Nice to Have:**
- Front-end development (HTML/CSS)
- Psychology or HCI background
- Graphic design experience
- Animation and motion design

#### Success Metrics

**Quantitative:**
- Task completion rates
- Time to complete common actions
- Error rates and misclicks
- Accessibility compliance scores
- User satisfaction scores (SUS)

**Qualitative:**
- Player feedback on usability
- Clarity of information presentation
- Aesthetic satisfaction
- Accessibility for diverse players

#### Decision Authority

**Full Authority:**
- Interface layouts and visual design
- Information architecture
- Usability testing protocols

**Shared Authority:**
- Tutorial content (with all designers)
- Visual aesthetics (with Art Director)
- Accessibility requirements (with Product)

**Requires Approval:**
- Major interface redesigns
- Changes affecting core navigation
- Accessibility features with cost implications

---

### 5. Progression Designer - Player Advancement and Retention

#### Primary Responsibilities

**Design Ownership:**
- Skill mastery and specialization progression
- Technology tree design and unlocks
- Achievement and milestone systems
- Long-term player retention mechanics
- Reward pacing and value curves
- Social reputation and guild advancement

**Documentation Requirements:**
- Progression curve models
- Unlock schedule spreadsheets
- Achievement specifications
- Retention strategy documents
- Reward value analysis

**Key Deliverables:**
- XP curve and leveling models
- Skill tree specifications
- Achievement system design
- Reward schedule documentation
- Retention analysis reports

#### Collaboration Patterns

**Works Closely With:**
- **Systems Designer:** Skill system integration
- **Economy Designer:** Reward value balance
- **Meta-Game Designer:** Long-term engagement
- **Analytics Team:** Retention data analysis

**Communication Requirements:**
- Daily: Analytics review
- Weekly: Progression balance reviews
- Bi-weekly: Player journey mapping
- Monthly: Retention strategy sessions

#### Required Tools and Skills

**Essential Tools:**
- **Excel/Google Sheets:** Progression modeling
- **Analytics Platforms:** Player behavior tracking
- **Python/R:** Statistical analysis
- **Visualization Tools:** Progression path diagrams
- **Playtesting Tools:** Progress tracking

**Required Skills:**
- Psychology and motivation theory
- Statistical analysis
- Long-term strategic thinking
- Player behavior understanding
- Clear data presentation

**Nice to Have:**
- Behavioral psychology background
- Live service experience
- Product management skills
- A/B testing expertise

#### Success Metrics

**Quantitative:**
- Player retention rates (D1, D7, D30)
- Progression velocity (time to milestones)
- Achievement completion rates
- Player churn at key progression points
- Engagement duration trends

**Qualitative:**
- Player satisfaction with progression pace
- Perceived sense of advancement
- Goal clarity and motivation
- Reward satisfaction

#### Decision Authority

**Full Authority:**
- XP curves and progression rates
- Achievement design
- Unlock schedules

**Shared Authority:**
- Skill system design (with Systems Designer)
- Reward values (with Economy Designer)
- Retention features (with Meta-Game Designer)

**Requires Approval:**
- Major progression rebalancing
- Changes affecting player investment
- New progression systems

---

### 6. Narrative Designer - Story and World Building

#### Primary Responsibilities

**Design Ownership:**
- Historical event narratives (geological epochs)
- Guild and faction lore
- Discovery-based environmental storytelling
- Player achievement documentation and history
- Quest design and branching narratives
- Character backstories and relationships

**Documentation Requirements:**
- Lore bible and world building documents
- Quest design specifications
- Dialogue scripts and branching trees
- Character profiles and relationships
- Timeline of historical events

**Key Deliverables:**
- Lore documentation
- Quest specifications
- Dialogue scripts
- Environmental storytelling guides
- Character design documents

#### Collaboration Patterns

**Works Closely With:**
- **Level Designer:** Environmental storytelling
- **Systems Designer:** Narrative system integration
- **Art Team:** Visual storytelling elements
- **Localization:** Translation and cultural adaptation

**Communication Requirements:**
- Daily: Writing and content creation
- Weekly: Story reviews and feedback
- Bi-weekly: Lore consistency checks
- Monthly: Narrative roadmap updates

#### Required Tools and Skills

**Essential Tools:**
- **Writing Software:** Scrivener, Google Docs
- **Dialogue Systems:** Twine, Ink, Yarn Spinner, Articy:Draft
- **Story Mapping:** Miro, Milanote
- **Version Control:** Git for narrative content
- **Localization Tools:** Translation management

**Required Skills:**
- Creative writing and storytelling
- Character development
- Dialogue writing
- Branching narrative design
- Cultural research and sensitivity

**Nice to Have:**
- Literature or screenwriting background
- Game writing experience
- Multiple language proficiency
- Historical knowledge (for BlueMarble)

#### Success Metrics

**Quantitative:**
- Quest completion rates
- Dialogue skip rates
- Story content engagement time
- Branching path exploration

**Qualitative:**
- Player emotional engagement
- Story coherence and satisfaction
- Character believability
- Lore consistency

#### Decision Authority

**Full Authority:**
- Story content and dialogue
- Character personalities
- Lore and world building details

**Shared Authority:**
- Quest mechanics (with Systems Designer)
- Environmental storytelling (with Level Designer)
- Cutscene presentation (with UX Designer)

**Requires Approval:**
- Major lore changes
- Character deaths or major changes
- Controversial or sensitive content

---

### 7. Meta-Game Designer - Social Systems and Live Service

#### Primary Responsibilities

**Design Ownership:**
- Guild system and territory control
- Player trading and marketplace dynamics
- Seasonal geological events
- Player rankings and leaderboards
- Social features and community building
- Live service content calendar

**Documentation Requirements:**
- Guild system specifications
- Seasonal event plans
- Social feature requirements
- Community engagement strategies
- Live service roadmap

**Key Deliverables:**
- Guild and social system designs
- Seasonal event specifications
- Community feature requirements
- Live ops content calendar
- Player retention initiatives

#### Collaboration Patterns

**Works Closely With:**
- **Economy Designer:** Guild economics
- **Progression Designer:** Seasonal progression
- **Community Team:** Player feedback and engagement
- **Analytics Team:** Social engagement metrics

**Communication Requirements:**
- Daily: Community feedback monitoring
- Weekly: Live ops planning
- Bi-weekly: Social feature reviews
- Monthly: Seasonal content planning

#### Required Tools and Skills

**Essential Tools:**
- **Analytics Suites:** Player behavior tracking
- **Live Service Platforms:** Event management
- **A/B Testing Tools:** Feature experimentation
- **Community Tools:** Discord, forums
- **Spreadsheets:** Reward and progression modeling

**Required Skills:**
- Community management
- Live service operations
- Social dynamics understanding
- Event planning and execution
- Data-driven iteration

**Nice to Have:**
- Sociology or community studies background
- Product management experience
- Marketing and growth experience
- Competitive gaming background

#### Success Metrics

**Quantitative:**
- Guild formation and activity rates
- Social feature engagement
- Seasonal event participation
- Player retention during live events
- Community growth metrics

**Qualitative:**
- Community health and toxicity levels
- Player satisfaction with social features
- Guild member satisfaction
- Event enjoyment and excitement

#### Decision Authority

**Full Authority:**
- Social feature design
- Seasonal event content
- Community feature priorities

**Shared Authority:**
- Guild economy (with Economy Designer)
- Social progression (with Progression Designer)
- Event rewards (with Economy/Progression)

**Requires Approval:**
- Major social system changes
- Controversial community features
- Changes affecting existing guilds

---

## Cross-Functional Workflows

### Feature Design Process

#### 1. Concept Phase (Week 1)
**Lead:** Relevant Design Lead  
**Participants:** All affected designers

**Activities:**
- Initial concept brainstorming
- Preliminary requirements gathering
- Identify cross-functional dependencies
- Create rough scope estimate

**Deliverables:**
- One-page concept document
- Initial dependency map
- Resource requirement estimate

#### 2. Detailed Design Phase (Weeks 2-4)
**Lead:** Feature owner designer  
**Participants:** Cross-functional design team

**Activities:**
- Create detailed design documentation
- Build mathematical models and prototypes
- Conduct internal design reviews
- Identify technical risks

**Deliverables:**
- Complete design specification
- Balance models and prototypes
- Technical requirements document
- Risk assessment

#### 3. Prototyping Phase (Weeks 5-7)
**Lead:** Systems/UX Designer  
**Participants:** Engineering, Art, Design

**Activities:**
- Build playable prototype
- Conduct internal playtests
- Iterate based on feedback
- Finalize specifications

**Deliverables:**
- Working prototype
- Playtest reports
- Finalized specifications
- Implementation plan

#### 4. Implementation Phase (Weeks 8-12)
**Lead:** Engineering Lead  
**Participants:** All relevant teams

**Activities:**
- Feature implementation
- Regular design reviews
- Address implementation issues
- Prepare for QA testing

**Deliverables:**
- Implemented feature
- Implementation notes
- QA test plans
- Documentation updates

#### 5. Tuning Phase (Weeks 13-14)
**Lead:** Feature owner designer  
**Participants:** Design, QA, Analytics

**Activities:**
- Balance tuning
- User testing
- Polish and refinement
- Prepare launch materials

**Deliverables:**
- Final balance parameters
- User testing reports
- Polish list completion
- Launch documentation

---

### Design Review Cadence

#### Daily Standups (15 minutes)
**Participants:** Design team  
**Format:** Quick updates and blockers

**Topics:**
- What each designer is working on
- Blockers or dependencies
- Urgent cross-team needs
- Quick questions and clarifications

#### Weekly Design Reviews (2 hours)
**Participants:** Full design team  
**Format:** Structured presentations and feedback

**Topics:**
- Feature design presentations
- Balance review and discussion
- Cross-functional dependency resolution
- Design documentation reviews

#### Bi-Weekly Playtests (3 hours)
**Participants:** Design team, stakeholders  
**Format:** Hands-on gameplay testing

**Topics:**
- Feature testing and feedback
- System integration validation
- User experience assessment
- Balance and tuning needs

#### Monthly Strategy Sessions (4 hours)
**Participants:** Design leads, directors  
**Format:** Strategic planning and roadmap

**Topics:**
- Roadmap review and updates
- Long-term design vision
- Team structure and resource needs
- Major design decisions

---

## Design Documentation Standards

### Required Documentation by Phase

#### Concept Phase
- **One-pager:** Executive summary of feature
- **Requirements:** Initial scope and goals
- **Dependencies:** Cross-functional needs

#### Design Phase
- **Design Specification:** Complete feature design
- **Balance Models:** Mathematical models and formulas
- **Wireframes/Mockups:** Visual representations (if applicable)
- **Technical Requirements:** Engineering needs

#### Implementation Phase
- **Implementation Notes:** Real-world implementation details
- **Edge Cases:** Documented handling of edge cases
- **Test Plans:** QA testing specifications

#### Post-Launch
- **Performance Reports:** Metrics and analytics
- **Post-Mortem:** Lessons learned
- **Iteration Plan:** Future improvements

### Documentation Tools and Storage

**Primary Tools:**
- **Confluence/Notion:** Living documentation
- **Google Drive:** Shared documents and spreadsheets
- **GitHub/GitLab:** Version-controlled design parameters
- **Figma:** UI/UX documentation and mockups

**Organization Structure:**
```
Design Documentation/
├── Vision & Pillars/
│   ├── Core Design Vision
│   └── Design Pillars
├── Systems/
│   ├── Core Mechanics
│   ├── Crafting Systems
│   └── Geological Simulation
├── Economy/
│   ├── Economic Models
│   ├── Resource Balance
│   └── Market Systems
├── Progression/
│   ├── XP Curves
│   ├── Skill Trees
│   └── Achievements
├── UX/
│   ├── Interface Specs
│   ├── User Flows
│   └── Usability Reports
└── Templates/
    ├── Feature Spec Template
    ├── Balance Model Template
    └── Post-Mortem Template
```

---

## Tool Standards and Requirements

### Mandatory Tools (All Designers)

**Documentation:**
- **Confluence or Notion:** Team wiki and documentation
- **Google Workspace:** Shared documents and spreadsheets
- **Git:** Version control for design files

**Communication:**
- **Slack/Discord:** Team communication
- **Zoom/Meet:** Video conferencing
- **Miro/Milanote:** Collaborative brainstorming

**Project Management:**
- **Jira/Asana:** Task tracking
- **GitHub Issues:** Design bug tracking

### Role-Specific Tools

**Systems & Economy Designers:**
- Excel/Google Sheets (advanced proficiency required)
- Python or R (recommended for simulations)
- Machinations (system visualization)

**Level & Combat Designers:**
- Unity or Unreal Engine (proficiency required)
- Blender (basic whiteboxing)
- World Creator or Gaea (terrain generation)

**UX Designer:**
- Figma or Adobe XD (proficiency required)
- InVision or Axure (prototyping)
- Analytics platform access

**Narrative Designer:**
- Writing software (Scrivener recommended)
- Dialogue tool (Articy:Draft, Ink, or Twine)
- Story mapping tool

**Progression & Meta-Game:**
- Advanced Excel/Sheets (pivot tables, complex formulas)
- Analytics platform (Mixpanel, Amplitude, or similar)
- Python for data analysis (recommended)

---

## Onboarding and Training

### New Designer Onboarding (First 30 Days)

#### Week 1: Foundation
- Repository and documentation orientation
- Team introductions and role clarifications
- Tool setup and access provisioning
- Review core design documents

#### Week 2: Domain Learning
- Deep dive into assigned specialization
- Review existing features in area
- Shadow experienced designers
- First small documentation task

#### Week 3: Contribution
- Take ownership of minor feature or improvement
- Participate in design reviews
- Submit first design proposal
- Begin regular standup participation

#### Week 4: Integration
- Lead first small feature design
- Present to team
- Establish workflow and collaboration patterns
- Set 90-day goals with manager

### Continuous Learning Requirements

**Monthly (4 hours/month):**
- Industry reading and research
- GDC talks and conference videos
- Postmortems and case studies

**Quarterly (1 day/quarter):**
- Team design workshop or training
- External conference or training
- Skill development in adjacent area

**Annually (1 week/year):**
- Attend major conference (GDC, DevCom, etc.)
- Deep skill development course
- Contribute to design community

---

## Success Metrics and Performance

### Team-Level Metrics

**Design Quality:**
- Feature satisfaction scores (player surveys)
- Critical bug rate from design issues
- Design iteration velocity
- Documentation completeness

**Collaboration Efficiency:**
- Cross-functional review turnaround time
- Design dependency blocker frequency
- Cross-team satisfaction scores

**Delivery Performance:**
- Design milestone hit rate
- Feature design-to-implementation time
- Post-launch iteration requirements

### Individual Performance Criteria

**Core Competencies (All Designers):**
- Design quality and player satisfaction
- Documentation thoroughness
- Collaboration and communication
- Innovation and creativity
- Data-driven decision making

**Role-Specific Criteria:**
- See individual role sections for metrics
- Specialized tool proficiency
- Domain expertise depth
- Cross-functional leadership

---

## BlueMarble-Specific Considerations

### Geological Simulation Integration

**Systems Designer Responsibilities:**
- Maintain scientific accuracy in simulation parameters
- Balance realism vs. gameplay fun
- Document geological behaviors
- Coordinate with science advisors (if applicable)

**Level Designer Responsibilities:**
- Realistic terrain generation
- Biome placement based on geological principles
- Resource distribution following natural patterns
- Educational opportunities through environment

### Player-Driven Economy Emphasis

**Economy Designer Priorities:**
- Robust player trading systems
- Anti-exploitation measures
- Guild economic gameplay
- Territory resource management

### Long-Term Simulation Focus

**All Designers:**
- Design for 100+ hour gameplay
- Consider years-long player progression
- Plan for emergent player behaviors
- Support player-created content and stories

---

## Conflict Resolution and Decision Making

### Design Disagreement Process

#### Level 1: Designer Discussion (1-2 days)
- Designers discuss and attempt to reach consensus
- Document different perspectives
- Identify objective criteria for evaluation

#### Level 2: Lead Designer Mediation (1 week)
- Relevant design lead facilitates discussion
- Review data and player feedback
- Prototype competing approaches if feasible
- Make recommendation with documented rationale

#### Level 3: Director Decision (1 week)
- Escalate to Creative Director or Game Director
- Present both perspectives with evidence
- Director makes final call
- Document decision and rationale

### Authority Matrix

| Decision Type | Systems | Economy | Level | UX | Progression | Narrative | Meta-Game |
|--------------|---------|---------|-------|-----|-------------|-----------|-----------|
| Core Mechanics | ✅ Lead | 🤝 Shared | 🤝 Shared | 📋 Informed | 📋 Informed | 📋 Informed | 📋 Informed |
| Resource Balance | 🤝 Shared | ✅ Lead | 📋 Informed | 📋 Informed | 🤝 Shared | 📋 Informed | 📋 Informed |
| World Layout | 📋 Informed | 📋 Informed | ✅ Lead | 📋 Informed | 📋 Informed | 🤝 Shared | 📋 Informed |
| Interface Design | 🤝 Shared | 📋 Informed | 📋 Informed | ✅ Lead | 📋 Informed | 📋 Informed | 📋 Informed |
| Progression Curves | 🤝 Shared | 🤝 Shared | 📋 Informed | 📋 Informed | ✅ Lead | 📋 Informed | 🤝 Shared |
| Story Content | 📋 Informed | 📋 Informed | 🤝 Shared | 📋 Informed | 📋 Informed | ✅ Lead | 📋 Informed |
| Social Features | 📋 Informed | 🤝 Shared | 📋 Informed | 🤝 Shared | 🤝 Shared | 📋 Informed | ✅ Lead |

**Legend:**
- ✅ **Lead:** Final decision authority after consultation
- 🤝 **Shared:** Must reach consensus with other shared roles
- 📋 **Informed:** Must be informed but no veto power

---

## Appendix A: Hiring Profiles

### Systems Designer Profile

**Required Experience:**
- 3+ years game design experience
- Strong mathematical and analytical skills
- Experience with balance modeling
- Programming/scripting ability

**Education:**
- Bachelor's degree in relevant field (Computer Science, Mathematics, Engineering)
- Or equivalent experience in game design

**Portfolio Requirements:**
- Mathematical balance models
- System design documents
- Shipped game features
- Balance analysis examples

### Economy Designer Profile

**Required Experience:**
- 3+ years game economy experience
- Data analysis expertise
- Statistical modeling skills
- Live service experience preferred

**Education:**
- Bachelor's degree in Economics, Mathematics, Data Science, or related
- Or equivalent experience

**Portfolio Requirements:**
- Economic models and simulations
- Data analysis reports
- Shipped economy features
- Post-mortem analysis examples

### UX Designer Profile

**Required Experience:**
- 3+ years UX design experience (gaming preferred)
- User research and testing expertise
- Interface design portfolio
- Accessibility knowledge

**Education:**
- Bachelor's degree in HCI, Design, Psychology, or related
- Or equivalent experience

**Portfolio Requirements:**
- UI mockups and specifications
- User research reports
- Shipped interface features
- Usability test documentation

---

## Appendix B: Templates

### Feature Design Template

```markdown
# Feature Name

**Designer:** [Name]
**Date:** YYYY-MM-DD
**Status:** [Concept/Design/Implementation/Live]

## Summary
[One paragraph overview]

## Goals
- Goal 1
- Goal 2
- Goal 3

## Non-Goals
- What this feature will NOT do

## Design Details
[Detailed design specification]

## Balance Parameters
[Mathematical models, formulas, values]

## Dependencies
- System A (Systems Designer)
- Feature B (Economy Designer)

## Success Metrics
- Metric 1: Target value
- Metric 2: Target value

## Open Questions
- Question 1
- Question 2

## References
- Related Document 1
- Related Document 2
```

### Balance Model Template

[See separate Excel template in shared drive]

### Post-Mortem Template

```markdown
# Feature Post-Mortem: [Feature Name]

**Date:** YYYY-MM-DD
**Designer:** [Name]
**Launch Date:** YYYY-MM-DD

## Summary
[Brief overview of feature and outcomes]

## Goals vs. Results

| Goal | Target | Actual | Status |
|------|--------|--------|--------|
| Metric 1 | Value | Value | ✅/⚠️/❌ |

## What Went Well
- Success 1
- Success 2

## What Didn't Go Well
- Challenge 1
- Challenge 2

## Lessons Learned
- Lesson 1
- Lesson 2

## Future Recommendations
- Recommendation 1
- Recommendation 2
```

---

## Appendix C: Recommended Reading

### Essential Books (All Designers)
1. **"The Art of Game Design: A Book of Lenses"** - Jesse Schell
2. **"Level Up! The Guide to Great Video Game Design"** - Scott Rogers
3. **"Game Design Workshop"** - Tracy Fullerton

### Systems Design
4. **"Designing Games"** - Tynan Sylvester
5. **"Game Mechanics: Advanced Game Design"** - Earnest Adams & Joris Dormans

### Economy Design
6. **"Virtual Economies"** - Vili Lehdonvirta & Edward Castronova
7. **"Game Balance"** - Ian Schreiber & Brenda Romero

### UX Design
8. **"Don't Make Me Think"** - Steve Krug
9. **"The Design of Everyday Things"** - Don Norman

### Narrative Design
10. **"Hamlet on the Holodeck"** - Janet Murray
11. **"Story"** - Robert McKee

---

## Document Revision History

| Version | Date | Author | Changes |
|---------|------|--------|---------|
| 1.0 | 2025-01-20 | BlueMarble Design Team | Initial guidelines based on role research |

---

## Related Documents

- [Game Design Roles and Types Research](../../research/topics/game-design-roles-and-types.md) - Foundation research
- [Core Game Design Document](gdd-core-game-design.md) - Overall game vision
- [Contributing Guidelines](../../CONTRIBUTING.md) - Repository contribution standards
- [Design Pillars](../../design/pillars.md) - High-level design principles

---

*These guidelines are living documents and should be updated as the team and project evolve. Feedback and suggestions for improvements are always welcome.*
