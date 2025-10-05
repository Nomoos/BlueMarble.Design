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

## Appendix C: Recommended Reading and Resources

### Essential Books (All Designers)

1. **"The Art of Game Design: A Book of Lenses" (3rd Edition, 2019)** - Jesse Schell
   - ISBN: 978-1138632059
   - The definitive game design textbook with 100+ design lenses
   - Covers psychology, technology, business, and creativity
   - Interactive companion app available

2. **"Level Up! The Guide to Great Video Game Design" (2nd Edition, 2014)** - Scott Rogers
   - ISBN: 978-1118877166
   - Practical, entertaining guide for all aspects of design
   - Excellent illustrations and examples
   - Great for communicating with non-designers

3. **"Game Design Workshop: A Playcentric Approach to Creating Innovative Games" (4th Edition, 2018)** - Tracy Fullerton
   - ISBN: 978-1138098770
   - Used in top game design programs worldwide
   - Includes exercises and worksheets
   - Emphasis on iterative design and playtesting

4. **"Rules of Play: Game Design Fundamentals" (2003)** - Katie Salen & Eric Zimmerman
   - ISBN: 978-0262240451
   - Comprehensive theoretical foundation
   - Academic but essential for understanding game systems
   - Reference book for serious designers

5. **"A Theory of Fun for Game Design" (2nd Edition, 2013)** - Raph Koster
   - ISBN: 978-1449363215
   - Quick read (4-6 hours) with profound insights
   - Illustrated exploration of why games are fun
   - Essential for understanding player motivation

---

### Systems and Combat Design

6. **"Designing Games: A Guide to Engineering Experiences" (2013)** - Tynan Sylvester
   - ISBN: 978-1449337933
   - Systems thinking and emergent gameplay
   - Written by RimWorld creator
   - Essential for understanding complex system interactions

7. **"Game Mechanics: Advanced Game Design" (2012)** - Ernest Adams & Joris Dormans
   - ISBN: 978-0321820273
   - Mathematical approach to game systems
   - Introduces Machinations framework
   - Includes system modeling exercises

8. **"Game Balance" (2021)** - Ian Schreiber & Brenda Romero
   - ISBN: 978-1032023304
   - Comprehensive guide to balancing mechanics
   - Mathematical models and practical techniques
   - Industry-standard reference

9. **"Characteristics of Games" (2011)** - George Skaff Elias, Richard Garfield, K. Robert Gutschera
   - ISBN: 978-0262017138
   - Co-authored by Magic: The Gathering creator
   - Deep analysis across all game types
   - Excellent for combat designers

10. **"The Ultimate Guide to Video Game Writing and Design" (2006)** - Flint Dille & John Zuur Platten
    - ISBN: 978-1580650663
    - Industry veterans' practical techniques
    - Combat scenario design
    - Quest and mission structure

11. **"21st Century Game Design" (2005)** - Chris Bateman & Richard Boon
    - ISBN: 978-1584503712
    - Audience-focused design approach
    - Demographic game design model
    - Understanding different player types

---

### Economy and Progression Design

12. **"Virtual Economies: Design and Analysis" (2014)** - Vili Lehdonvirta & Edward Castronova
    - ISBN: 978-0262027250
    - Academic analysis of game economies
    - Real-world economic theory applied to games
    - Essential for economy designers

13. **"Free-to-Play: Making Money From Games You Give Away" (2013)** - Will Luton
    - ISBN: 978-0321957962
    - F2P monetization and economy design
    - Data-driven balance approach
    - Practical metrics and KPIs

14. **"Game Analytics: Maximizing the Value of Player Data" (2013)** - Magy Seif El-Nasr, Anders Drachen, Alessandro Canossa
    - ISBN: 978-1447152811
    - Essential for data-driven design
    - Metrics for progression and economy
    - Analytics implementation guide

15. **"Behavioral Game Design" (2013)** - John Hopson
    - Online resource from Xbox Live Arcade pioneer
    - Psychological principles for progression
    - Variable reward schedules
    - Player retention strategies

16. **"Monetization Strategy in Social Games" (Multiple Online Resources)**
    - Facebook, King, Supercell design philosophies
    - Retention and engagement metrics
    - Live service best practices

---

### Level and World Design

17. **"An Architectural Approach to Level Design" (2nd Edition, 2019)** - Christopher W. Totten
    - ISBN: 978-1138828056
    - Architectural principles for level design
    - Spatial design and player flow
    - Practical exercises and case studies

18. **"Level Design: Processes and Experiences" (2017)** - Christopher W. Totten (Editor)
    - ISBN: 978-1498755641
    - Essays from professional level designers
    - Real-world workflows and case studies
    - Multiple genre perspectives

19. **"Environmental Storytelling: Creating Immersive 3D Worlds Using Lessons from the Theme Park Industry" (2018)** - Don Carson
    - ISBN: 978-1138234109
    - Theme park design applied to games
    - Creating memorable spaces
    - Narrative through environment

20. **"100 Principles of Game Design" (2019)** - DESPAIN Wendy
    - ISBN: 978-0321902498
    - Includes strong section on level design
    - Quick reference principles
    - Industry wisdom compiled

---

### UX and Interface Design

21. **"Don't Make Me Think, Revisited: A Common Sense Approach to Web Usability" (3rd Edition, 2014)** - Steve Krug
    - ISBN: 978-0321965516
    - Classic usability guide applicable to game UI
    - Clear, actionable principles
    - Quick read with immediate value

22. **"The Design of Everyday Things" (Revised Edition, 2013)** - Don Norman
    - ISBN: 978-0465050659
    - Fundamental design principles
    - Understanding user mental models
    - Essential for all designers

23. **"Game UI Discoveries: What Players Want" (2019)** - Masamichi Harada
    - ISBN: 978-4862465344
    - Game-specific UI/UX research
    - Player expectations from Japanese market
    - Data-driven insights

24. **"The Gamer's Brain: How Neuroscience and UX Can Impact Video Game Design" (2017)** - Celia Hodent
    - ISBN: 978-1498775502
    - Cognitive science for game design
    - Perception, memory, and attention
    - Essential for UX designers

25. **"Interface Design for Learning: Design Strategies for Learning Experiences" (2015)** - Dorian Peters
    - ISBN: 978-0321903044
    - Onboarding and tutorial design
    - Learning psychology
    - Progressive disclosure techniques

---

### Narrative Design

26. **"Hamlet on the Holodeck: The Future of Narrative in Cyberspace" (Updated Edition, 2017)** - Janet Murray
    - ISBN: 978-0262533485
    - Interactive storytelling theory
    - Digital narrative foundations
    - Academic but essential

27. **"Story: Substance, Structure, Style and the Principles of Screenwriting" (1997)** - Robert McKee
    - ISBN: 978-0060391683
    - Classic storytelling structure
    - Used in film and game writing
    - Deep dive into narrative craft

28. **"The Writer's Journey: Mythic Structure for Writers" (3rd Edition, 2007)** - Christopher Vogler
    - ISBN: 978-1932907360
    - Hero's journey framework
    - Character arc development
    - Popular in game narrative

29. **"Creating Character Arcs: The Masterful Author's Guide" (2016)** - K.M. Weiland
    - ISBN: 978-1505255133
    - Character development techniques
    - Applicable to NPCs and player characters
    - Practical exercises

30. **"Procedural Storytelling in Game Design" (2015)** - Tanya X. Short & Tarn Adams (Editors)
    - ISBN: 978-1498729765
    - Emergent narrative systems
    - Essays from industry leaders
    - Essential for systemic narrative

31. **"Interactive Storytelling for Video Games" (2011)** - Josiah Lebowitz & Chris Klug
    - ISBN: 978-0240817170
    - Video game narrative techniques
    - Branching dialogue systems
    - Quest design principles

---

### Meta-Game and Live Operations

32. **"Hooked: How to Build Habit-Forming Products" (2014)** - Nir Eyal
    - ISBN: 978-1591847786
    - Psychology of engagement
    - Hook model for retention
    - Ethical considerations

33. **"The Lean Startup" (2011)** - Eric Ries
    - ISBN: 978-0307887894
    - Iterative development
    - Minimum viable product
    - Data-driven iteration

34. **"Engagement Game: Why Your Workplace Culture Should Look More Like a Video Game" (2019)** - Mike Hyzy
    - ISBN: 978-1948122641
    - Gamification principles
    - Engagement systems
    - Community building

35. **"Building Successful Online Communities" (2012)** - Robert E. Kraut & Paul Resnick
    - ISBN: 978-0262016575
    - Online community design
    - Social dynamics
    - Moderation and governance

---

### Psychology and Player Behavior

36. **"The Psychology of Video Games" (2019)** - Celia Hodent
    - ISBN: 978-1138090712
    - Comprehensive psychology overview
    - Motivation, emotion, engagement
    - Research-backed insights

37. **"Reality Is Broken: Why Games Make Us Better and How They Can Change the World" (2011)** - Jane McGonigal
    - ISBN: 978-0143120612
    - Positive psychology and games
    - Player motivation understanding
    - Inspirational perspective

38. **"Drive: The Surprising Truth About What Motivates Us" (2011)** - Daniel H. Pink
    - ISBN: 978-1594484803
    - Autonomy, mastery, purpose framework
    - Intrinsic motivation
    - Applicable to progression design

39. **"Flow: The Psychology of Optimal Experience" (1990)** - Mihaly Csikszentmihalyi
    - ISBN: 978-0061339202
    - Foundation of flow theory
    - Challenge-skill balance
    - Essential for all designers

40. **"Addiction by Design: Machine Gambling in Las Vegas" (2014)** - Natasha Dow Schüll
    - ISBN: 978-0691160887
    - Understanding addictive design
    - Ethical considerations
    - Important cautionary perspective

---

### MMO and Multiplayer Design (BlueMarble-specific)

41. **"Designing Virtual Worlds" (2003)** - Richard Bartle
    - ISBN: 978-0131018167
    - Classic MMO design text
    - Player types (Bartle taxonomy)
    - Social dynamics in virtual worlds

42. **"MMO Design Essentials" (2012)** - Damion Schubert
    - ISBN: 978-1118698396
    - Practical MMO design patterns
    - Social systems and economy
    - Live service considerations

43. **"Multiplayer Game Programming" (2015)** - Joshua Glazer & Sanjay Madhav
    - ISBN: 978-0134034300
    - Technical foundations for designers
    - Network architecture understanding
    - Latency and synchronization

44. **"Communities of Play: Emergent Cultures in Multiplayer Games and Virtual Worlds" (2011)** - Celia Pearce & Artemesia
    - ISBN: 978-0262516518
    - Social dynamics research
    - Player culture formation
    - Long-term community building

---

### Simulation Game Design (BlueMarble-specific)

45. **"A Casual Revolution: Reinventing Video Games and Their Players" (2009)** - Jesper Juul
    - ISBN: 978-0262013376
    - Accessibility in complex games
    - Difficulty and engagement
    - Broader audience reach

46. **"Half-Real: Video Games between Real Rules and Fictional Worlds" (2005)** - Jesper Juul
    - ISBN: 978-0262101106
    - Game rules and fiction interaction
    - Relevant for simulation design
    - Academic but accessible

47. **"Simulations and Serious Games for Education" (2017)** - Dirk Ifenthaler, et al.
    - ISBN: 978-3319517414
    - Educational simulation design
    - Learning through systems
    - Applicable to complex simulations

---

### Business and Production

48. **"Blood, Sweat, and Pixels: The Triumphant, Turbulent Stories Behind How Video Games Are Made" (2017)** - Jason Schreier
    - ISBN: 978-0062651235
    - Real development stories
    - Production challenges
    - Context for design decisions

49. **"The Game Production Handbook" (3rd Edition, 2014)** - Heather Maxwell Chandler
    - ISBN: 978-1449688097
    - Production workflows
    - Design in development pipeline
    - Practical project management

50. **"Game Development Essentials: Game Project Management" (2009)** - John Hight & Jeannie Novak
    - ISBN: 978-1418052829
    - Managing game projects
    - Designer-producer relationship
    - Workflow optimization

---

## Online Resources and Communities

### Professional Publications

51. **Game Developer (formerly Gamasutra)** - https://www.gamedeveloper.com/
    - Industry-leading publication
    - Design postmortems and case studies
    - Daily articles and news
    - Free extensive archives

52. **Polygon** - https://www.polygon.com/
    - Game journalism with design focus
    - Developer interviews
    - Industry analysis

53. **GameDiscoverCo** - https://gamediscover.co/
    - Market analysis and player behavior
    - Data-driven insights
    - Weekly newsletter with trends
    - Plus subscription for deep data

54. **80.lv** - https://80.lv/
    - Developer interviews and breakdowns
    - Technical and design deep dives
    - Portfolio showcases

55. **Deconstructor of Fun** - https://www.deconstructoroffun.com/
    - Mobile game design analysis
    - Monetization and retention
    - F2P best practices

---

### Video Content and Online Courses

56. **GDC Vault** - https://gdcvault.com/
    - 9,000+ conference presentations
    - Design, art, programming tracks
    - Free and premium content
    - Essential for staying current

57. **Extra Credits** - https://www.youtube.com/@extracredits
    - 500+ educational videos
    - Animated design concept explanations
    - History and theory coverage
    - Beginner-friendly

58. **Game Maker's Toolkit (Mark Brown)** - https://www.youtube.com/@GMTK
    - In-depth design analysis videos
    - Pattern and mechanic breakdowns
    - Annual game jam
    - High production quality

59. **GDC YouTube Channel** - https://www.youtube.com/@Gdconf
    - Free GDC talk library
    - All development disciplines
    - Updated regularly
    - Searchable by topic

60. **Coursera - Game Design and Development Specialization (Michigan State)** - https://www.coursera.org/specializations/game-development
    - University-level courses
    - Full specialization or individual courses
    - Certificate available
    - Includes capstone project

61. **Coursera - Game Design: Art and Concepts Specialization (CalArts)** - https://www.coursera.org/specializations/game-design
    - Artistic game design approach
    - World and character design
    - Story and development
    - Certificate program

62. **Udemy - Complete Game Design Course** - https://www.udemy.com/
    - Search "game design" for 500+ courses
    - Practical, project-based
    - Affordable with sales
    - Lifetime access

63. **LinkedIn Learning - Game Design Paths** - https://www.linkedin.com/learning/
    - Professional development courses
    - Industry expert instructors
    - Includes software tutorials
    - Often free through libraries/universities

64. **YouTube - Design Doc** - https://www.youtube.com/@DesignDoc
    - Game design analysis channel
    - Specific mechanic deep dives
    - Historical design evolution

65. **YouTube - Noclip** - https://www.youtube.com/@NoclipVideo
    - Game development documentaries
    - Behind-the-scenes design
    - High-quality production

---

### Podcasts

66. **Designer Notes (Soren Johnson)** - https://www.designer-notes.com/
    - Interviews with legendary designers
    - Sid Meier, Will Wright, Bruce Shelley, etc.
    - Deep design philosophy
    - 200+ episodes

67. **The Game Design Round Table** - https://thegamedesignroundtable.com/
    - Weekly design topic discussions
    - Industry professionals
    - Practical design conversations
    - 300+ episodes

68. **Eggplant: The Secret Lives of Games** - https://eggplant.show/
    - Thoughtful game analysis
    - Cultural and artistic perspectives
    - Design philosophy exploration

69. **Game Dev Unchained** - Multiple platforms
    - Indie and AAA interviews
    - Design process insights
    - Career advice

70. **The AIAS Game Maker's Notebook** - https://www.interactive.org/Podcasts/
    - Academy of Interactive Arts & Sciences
    - Hall of Fame designer interviews
    - Industry history and evolution

71. **Game Dev Field Guide** - https://www.gamedevfieldguide.com/
    - Industry veterans sharing knowledge
    - Career development focus
    - Practical advice

---

### Communities and Forums

72. **Reddit - r/gamedesign** - https://www.reddit.com/r/gamedesign/
    - 280,000+ members
    - Design discussions and critique
    - Resource sharing
    - Active daily

73. **Reddit - r/truegamedev** - https://www.reddit.com/r/truegamedev/
    - Professional developers only
    - Higher quality discussions
    - Industry veterans
    - Verified members

74. **Reddit - r/ludology** - https://www.reddit.com/r/ludology/
    - Game studies and theory
    - Academic perspective
    - Research discussions

75. **IGDA (International Game Developers Association)** - https://igda.org/
    - Professional organization
    - Local chapters worldwide
    - Special Interest Groups (SIGs)
    - Game Design SIG specifically
    - Career resources and networking

76. **Game Design Discord Servers**
    - Game Designers Hangout (20,000+ members)
    - IGDA Discord
    - GDC Community Discord
    - Real-time discussions and feedback

77. **Designer League** - Online community
    - Professional game designers
    - Mentorship programs
    - Portfolio reviews
    - Job opportunities

78. **Twitter/X Game Design Community**
    - Follow: @jesseschell, @ibablu, @Soren_Johnson, @JeffVogel, @gamedevundrgnd
    - Design discourse and insights
    - Portfolio sharing
    - Industry networking
    - Job postings

---

### Tools and Resource Databases

79. **Game UI Database** - https://www.gameuidatabase.com/
    - 40,000+ UI screenshots
    - Searchable by game/platform/element
    - Essential UX reference
    - Pattern library

80. **Machinations.io** - https://machinations.io/
    - Visual system design tool
    - Simulate game economies
    - Balance testing
    - Educational resources

81. **mobygames** - https://www.mobygames.com/
    - Comprehensive game database
    - Full credit listings
    - Career tracking
    - Research tool

82. **HowLongToBeat** - https://howlongtobeat.com/
    - Game completion times
    - Pacing research
    - Player engagement data
    - Progression design reference

83. **Game Design Tools** - https://www.gamedesigntools.com/
    - Curated tool directory
    - Prototyping resources
    - Design software reviews

---

### Design Pattern and Analysis Resources

84. **Game Programming Patterns** - https://gameprogrammingpatterns.com/
    - Free online book
    - Technical patterns for designers
    - Understanding implementation
    - Code structure insights

85. **Gamasutra's Design Patterns Collection** - https://www.gamedeveloper.com/
    - Common design patterns
    - Cross-game analysis
    - Historical perspectives

86. **MDA Framework** - "Mechanics, Dynamics, Aesthetics"
    - Foundational design framework
    - Academic paper (free online)
    - Essential design theory

87. **Temporal Dynamics of Design** - Various resources
    - Richard Lemarchand's work
    - Design cadence and pacing
    - GDC talks available

---

### Analytics and Market Research

88. **SteamSpy** - https://steamspy.com/
    - Steam game statistics
    - Player count estimates
    - Market research
    - Genre analysis

89. **Steam Charts** - https://steamcharts.com/
    - Live player tracking
    - Retention visualization
    - Engagement patterns
    - Competitive analysis

90. **GameRefinery** - https://www.gamerefinery.com/
    - Mobile game intelligence
    - Feature analysis
    - Market trends
    - Free reports and webinars

91. **Newzoo** - https://newzoo.com/
    - Game market intelligence
    - Player behavior research
    - Free reports and insights
    - Industry forecasts

92. **Sensor Tower** - https://sensortower.com/
    - Mobile game analytics
    - Download and revenue estimates
    - Category rankings
    - Competitive intelligence

93. **App Annie (data.ai)** - https://www.data.ai/
    - Mobile market data
    - User acquisition insights
    - Retention metrics

---

### Academic Resources

94. **Game Studies Journal** - http://gamestudies.org/
    - Peer-reviewed academic journal
    - Free access
    - Design theory and research
    - Quarterly publication

95. **DiGRA (Digital Games Research Association)** - https://digra.org/
    - Academic game research
    - Conference proceedings
    - Global network
    - Research papers

96. **ACM Digital Library - Games Section** - https://dl.acm.org/
    - Computer science research
    - Technical and design papers
    - University access often available
    - CHI PLAY conference papers

97. **Journal of Games Criticism** - http://gamescriticism.org/
    - Cultural game analysis
    - Critical perspectives
    - Design implications

98. **Well Played Journal** - https://press.etc.cmu.edu/journals/well-played/
    - Close readings of games
    - Design analysis
    - Educational focus

---

### Specialized Resources by Role

**Systems Designers:**
99. **Machinations Tutorials** - Video series on system modeling
100. **Explorable Explanations** - https://explorabl.es/ - Interactive systems
101. **Systems Design Facebook Group** - Professional community
102. **Gamasutra Systems Design Articles** - Filtered article collection

**Economy Designers:**
103. **EVE Online Economy Reports** - Quarterly MMO economy analysis
104. **Virtual Economy Forum** - Specialized community
105. **Roblox Developer Hub** - User-generated economy insights
106. **Axie Infinity Economy Papers** - Blockchain game economy research

**Level Designers:**
107. **World of Level Design** - https://www.worldofleveldesign.com/
108. **Level Design Lobby Discord** - 15,000+ members
109. **Valve Developer Community** - https://developer.valvesoftware.com/
110. **Unreal Engine Documentation - Level Design** - Official resources

**UX Designers:**
111. **Nielsen Norman Group** - https://www.nngroup.com/ - UX research authority
112. **Laws of UX** - https://lawsofux.com/ - Psychological principles
113. **UX Collective** - https://uxdesign.cc/ - Design articles
114. **Game UX Summit** - Annual conference focused on game UX

**Narrative Designers:**
115. **Narrative Design Explorers Club** - Professional community
116. **IFDB (Interactive Fiction Database)** - https://ifdb.org/
117. **Twine Cookbook** - https://twinery.org/cookbook/ - Interactive narrative
118. **Choice of Games Forum** - Narrative game discussions

**Progression Designers:**
119. **A/B Testing Resources** - Optimizely, Split.io education
120. **Retention Science Blog** - F2P retention strategies
121. **GameRefinery Webinars** - Progression design analysis

**Meta-Game Designers:**
122. **Community Management Resources** - CMX Hub
123. **Live Ops Best Practices** - Various GDC talks
124. **Social Features Design** - Facebook Gaming resources

---

### Conferences and Events

125. **Game Developers Conference (GDC)** - San Francisco, March
    - Premier industry conference
    - Design Summit and tracks
    - 28,000+ attendees
    - Networking paradise

126. **GDC Europe** - Various locations, Summer
    - European focus
    - Similar content to main GDC
    - More accessible for European developers

127. **PAX Dev** - Seattle/Boston, varies
    - Developer-focused
    - Smaller, intimate setting
    - Practical workshops
    - Networking emphasis

128. **Develop Conference** - Brighton, UK, July
    - European game development
    - Strong design track
    - Career development focus

129. **DevCom** - Cologne, Germany, August
    - During Gamescom week
    - International developers
    - Business and design
    - Free for registered developers

130. **IndieCade** - Los Angeles, October
    - Independent games festival
    - Experimental design
    - Innovation showcase
    - Academic connections

131. **GDC China** - Shanghai, varies
    - Asian market focus
    - Mobile and PC design
    - Regional insights

132. **Reboot Develop** - Dubrovnik, Croatia, April
    - Boutique developer conference
    - High-quality talks
    - Beautiful location
    - Strong networking

133. **Nordic Game Conference** - Malmö, Sweden, May
    - Scandinavian game industry
    - Design-forward
    - Indie and AAA mix

---

### Online Design Challenges and Jams

134. **Global Game Jam** - https://globalgamejam.org/
    - Annual worldwide event
    - 48-hour game creation
    - Theme-based challenges
    - Networking and learning

135. **Ludum Dare** - https://ldjam.com/
    - Online game jam
    - 48/72 hour formats
    - Large community
    - Design experimentation

136. **GMTK Game Jam** - https://itch.io/jam/gmtk-jam
    - Annual jam by Mark Brown
    - 48 hours
    - Design-focused themes
    - Large participation

137. **7 Day FPS** - https://itch.io/jam/7dfps
    - One week FPS challenge
    - Combat design practice
    - Rapid prototyping

138. **One Game a Month** - http://www.onegameamonth.com/
    - Monthly creation challenge
    - Portfolio building
    - Skill development

---

### Recommended Learning Path for BlueMarble Team

**Foundation (Months 1-3):**
1. Read Schell's "Art of Game Design" (all team)
2. Read Sylvester's "Designing Games" (systems focus)
3. Complete Coursera Game Design Specialization
4. Watch 50 GDC design talks
5. Join IGDA and local chapter

**Specialization (Months 4-6):**
6. Read role-specific books (see lists above)
7. Attend GDC or equivalent conference
8. Build design portfolio pieces
9. Participate in game jams
10. Join role-specific communities

**Advanced (Months 7-12):**
11. Read academic papers on specialization
12. Mentor emerging designers
13. Write design articles or give talks
14. Contribute to open-source design tools
15. Develop expertise in BlueMarble-specific areas

**BlueMarble-Specific Focus:**
- **Simulation:** Dwarf Fortress, RimWorld, SimCity design analysis
- **MMO Economy:** Virtual Economies book, EVE Online reports
- **Long-term Engagement:** Hooked, Behavioral Game Design
- **Geological Systems:** Scientific simulation papers
- **Player-Driven Worlds:** Minecraft, Space Engineers analysis

---

### Budget Allocation Recommendations

**For Individual Designers:**
- **Books:** $500-1000/year (build library)
- **Online Courses:** $500-1000/year
- **Conference Attendance:** $2000-3000/year (one major)
- **Tools/Software:** $500-1000/year
- **Total:** $3500-6000/year per designer

**For Design Team:**
- **Shared Library:** $2000 initial, $1000/year maintenance
- **Team Conferences:** $10,000-20,000/year (multiple attendance)
- **Online Resources:** $5000/year (subscriptions)
- **Training Programs:** $10,000/year
- **External Consultants:** $5000-10,000/year
- **Total:** $32,000-46,000/year for team of 5-7

**Free Resources Strategy:**
- Utilize all free GDC talks on YouTube
- Leverage free Coursera course audits
- Access university libraries for academic papers
- Participate in free online communities
- Watch all Extra Credits and GMTK videos
- Read free online design postmortems

---

### Continuous Learning Requirements

**Monthly (4 hours minimum):**
- Read 2-3 design articles
- Watch 2-3 GDC talks
- Participate in community discussions
- Study competitor games

**Quarterly (8 hours minimum):**
- Complete one online course or specialization
- Read one design book
- Attend virtual conference or workshop
- Write design reflection or article

**Annually (40 hours minimum):**
- Attend one major conference (GDC, DevCom, etc.)
- Complete comprehensive course or certification
- Mentor or teach game design
- Contribute to design community (talks, articles)

---

### Key Resource Aggregators

**Save These URLs:**
- Game Design Resources GitHub: https://github.com/ellisonleao/magictools
- Awesome Game Design: https://github.com/Calinou/awesome-game-design
- Game Development Resources: https://game-development.zeef.com/
- Free Game Development: https://github.com/Kavex/GameDev-Resources

These curated lists are maintained by the community and include hundreds of additional resources, tools, and learning materials updated regularly

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
