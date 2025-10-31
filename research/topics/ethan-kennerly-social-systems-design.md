# Ethan Kennerly - Social Systems Design: Governments and Churches

## Problem / Context

How can MMORPGs implement meaningful player-driven social systems like governments and churches that provide genuine agency and create emergent community dynamics? This research examines David Ethan Kennerly's pioneering work on social system design in early MMORPGs, particularly his implementation of player governments and religions in Dark Ages (1999).

Understanding Kennerly's design philosophy and implementation approaches is crucial for BlueMarble's social systems, as we aim to create player-driven communities with real political and social structures.

## Key Findings

- **Player-Driven Government System**: Kennerly designed a "synthetic government" in Dark Ages that combined democracy, meritocracy, and role-play merit to create authentic political structures with real in-game power
  - Players held elected offices (Guard, Judge, Burgess) with actual authority to enforce laws and resolve disputes
  - Legislative branch entirely player-controlled - players proposed and voted on laws that shaped town constitutions
  - Political influence based on both popularity (votes) and merit (creative contests in literature, art, philosophy)
  - System balanced bottom-up democracy with top-down role-play elements and developer oversight

- **Player-Driven Religious System**: Eight temples with entirely player-run clergy created authentic religious hierarchies
  - Each temple dedicated to different gods tied to character development paths (Wizards, Warriors, Priests, etc.)
  - Players could join temples, perform rituals, and rise through clergy ranks by recruiting followers
  - Higher-ranking clergy coordinated rituals, judged disputes, and influenced temple policies
  - Physical temple locations served as hubs for religious activities and community gathering

- **Merit and Popularity Mechanics**: Innovative "Clout" system tracked player standing and achievement
  - Meta-currency earned through peer votes and creative contest victories
  - Multiple contest categories: literature, art, philosophy, lore
  - Nobility and high office required success in peer-judged creative contests
  - Blended meritocracy (intellectual achievement) with democracy (popular vote)

- **Synthetic Governance Philosophy**: Kennerly's approach balanced multiple governance models
  - Democracy: bottom-up, player-driven decision making
  - Monarchy: top-down, role-play merit-driven leadership
  - Meritocracy: recognition of skill and creative achievement
  - Administrative control: developer oversight to limit corruption
  - Goal: enrich community interaction while maintaining fairness

- **Player Agency and Self-Governance**: System gave players unprecedented control over virtual society
  - Response to early MMORPGs where admins dominated and players were merely consumers
  - Real authority in hands of players created sense of ownership and civic participation
  - Self-moderated communities with player-created laws and enforcement
  - Encouraged creativity, storytelling, and meaningful social interaction

## Evidence

### Source 1: The Game Archaeologist - Dark Ages Analysis

- **Link**: https://massivelyop.com/2022/10/01/the-game-archaeologist-dark-ages-the-weird-lovechild-of-celtic-lore-and-lovecraftian-legends/
- **Key Points**:
  - Dark Ages (1999) featured player-driven governments where players could hold offices, create laws, and self-organize towns
  - Religious system allowed players to pledge to in-game faiths, advance through religious hierarchy, perform rituals, and recruit followers
  - Nobility and popularity contests had players vote on contributions to art, literature, philosophy, and lore
  - Political structures let players influence rules and governance of virtual spaces
- **Relevance**: Primary source documenting the actual implementation and player experience of Kennerly's social systems

### Source 2: Dark Ages Politics in Theory and Practice

- **Link**: http://tharsis-gate.org/articles/imaginary/DARKAG~1.HTM
- **Key Points**:
  - Describes "synthetic government" approach combining benefits of multiple governance models
  - Detailed explanation of how democracy, monarchy, and meritocracy were balanced
  - Focus on enriching community interaction while limiting corruption
  - Legislative, judicial, and executive functions distributed to players
- **Relevance**: Theoretical foundation for Kennerly's design philosophy and implementation rationale

### Source 3: Wikipedia - Dark Ages Game Design

- **Link**: https://en.wikipedia.org/wiki/Dark_Ages_(1999_video_game)
- **Key Points**:
  - Player governments emerged in Mileth, Rucesion, and later Medenia continent
  - Political offices included Guard, Judge, and Burgess with actual in-game power
  - Game emphasized player-managed content and storyline progression
  - Celtic mythology inspiration combined with Lovecraftian elements
- **Relevance**: Verification of game systems and historical context

### Source 4: Ethan Kennerly Resume and Biography

- **Link**: https://studylib.net/doc/8560416/ethan-kennerly---designer-scripter-resume
- **Link**: https://en.wikipedia.org/wiki/David_Ethan_Kennerly
- **Key Points**:
  - Designed social systems for Nexus: The Kingdom of the Winds and Dark Ages during tenure with Nexon
  - Background in algorithms, animation, and narrative design from San Francisco State University
  - Expertise in scripting systems (Python, ActionScript), quest frameworks, and equipment systems
  - Published academic work on game world abstraction and behavioral rules
- **Relevance**: Professional credentials and verification of design contributions

### Source 5: Dark Ages Religious System Details

- **Link**: https://gamicus.fandom.com/wiki/Dark_Ages/Walkthrough
- **Key Points**:
  - Eight different temples, five representing dominant attributes tied to character classes
  - Clergy entirely composed of players who could perform rituals and rise through ranks
  - Temple hierarchies managed by player communities with unique lore and practices
  - Advancement required active participation: coordinating rituals, judging disputes, influencing policies
- **Relevance**: Specific mechanics of the religious system implementation

### Design Observations

- **Community-Driven Dynamics**: Religious and political systems created genuine social structures with factions, conflicts, and politics mirroring real-world organizations
- **Emergent Gameplay**: Player agency led to unpredictable but meaningful community events and storylines
- **Creative Recognition**: Contest system encouraged intellectual and artistic contributions beyond combat
- **Role-Play Integration**: Systems complemented Celtic-mythology-inspired world and encouraged community storytelling
- **Scalability**: System worked for town-level governance but also expanded to continent-level (Medenia)

## Implications for BlueMarble Design

- **Player Government Framework**: Implement tiered political system for settlements
  - Start with small settlement councils (3-5 elected officials)
  - Scale to city governments with multiple branches (legislative, judicial, executive)
  - Include both elected positions (popular vote) and appointed positions (merit-based)
  - Give real authority: tax rates, building permissions, resource allocation, law enforcement
  - Implement conflict resolution and dispute arbitration systems

- **Religious/Ideological Systems**: Create player-run organizations with hierarchical structures
  - Multiple competing philosophies/religions tied to game lore (geological focus, corporate ideologies, survival philosophies)
  - Player clergy/leadership with recruitment and advancement mechanics
  - Physical locations (temples, corporate headquarters, guild halls) as social hubs
  - Rituals and ceremonies that provide gameplay benefits and community bonding
  - Inter-organization politics and competition for followers

- **Merit and Recognition Systems**: Beyond combat, recognize diverse player contributions
  - Engineering/building contests (best mine design, most efficient processing plant)
  - Economic achievement (successful trade routes, market manipulation detection/rewards)
  - Scientific discovery (new ore deposits, geological phenomena documentation)
  - Community service (teaching new players, creating guides, organizing events)
  - Track "influence" or "reputation" as meta-currency for political advancement

- **Synthetic Governance Approach**: Balance multiple power structures
  - Popular democracy for broad decisions (settlement locations, major projects)
  - Meritocracy for specialized roles (chief engineer, head of mining operations)
  - Economic power (wealthy players influence through investment and funding)
  - Developer oversight for balance and anti-corruption measures
  - Term limits and recall mechanisms to prevent stagnation

- **Creative Contribution Systems**: Encourage non-combat engagement
  - In-game writing system for lore, histories, technical manuals
  - Engineering blueprint sharing and voting
  - Art/design contests for settlement layouts and building aesthetics
  - Scientific paper system for documenting geological discoveries
  - Peer review and recognition mechanics

- **Self-Governance Tools**: Provide infrastructure for player-created rules
  - Law/policy creation interface with voting mechanisms
  - Enforcement tools for elected officials (temporary bans from settlement, resource penalties)
  - Public record system displaying all laws and policies
  - Appeal mechanisms for disputed decisions
  - Transparency in voting and decision-making processes

- **Social Hub Design**: Create meaningful physical spaces for community
  - Town halls with public meeting spaces and voting stations
  - Temples/ideology centers with ritual spaces
  - Contest halls for displaying creative works
  - Public forums for debates and announcements
  - Private offices for elected officials

- **Player Agency Focus**: Trust players with real power
  - Avoid purely cosmetic political positions
  - Give player governments control over meaningful game mechanics
  - Allow player-created content to influence world state
  - Enable genuine conflict and competition between player organizations
  - Developer intervention only for technical issues or severe abuse

## Open Questions / Next Steps

### Open Questions

- How do we prevent dominant groups from monopolizing all positions of power?
- What term limits and rotation systems work best for maintaining fresh leadership?
- How can we encourage cross-settlement cooperation while maintaining competitive dynamics?
- What happens to player-created laws when officials are offline or quit the game?
- How do we handle succession when religious/political leaders become inactive?
- What role should AI/NPCs play in supporting player-run systems during low-population periods?
- How do we balance realism with fun in political/legal systems?
- What metrics should we use to measure success of social systems?

### Next Steps

- [ ] Design settlement government structure with specific roles and powers
- [ ] Create merit/influence tracking system for political advancement
- [ ] Develop religious/ideological framework tied to BlueMarble lore (mining corporations, geological cults, survival philosophies)
- [ ] Prototype law creation and voting mechanics
- [ ] Design contest system for engineering, building, and economic achievements
- [ ] Research conflict resolution mechanisms from other games (EVE Online, ArcheAge)
- [ ] Define developer oversight boundaries and anti-corruption measures
- [ ] Create technical specification for political/religious system implementation
- [ ] Design UI/UX for governance interfaces (voting, law display, contest submission)
- [ ] Prototype physical spaces (town halls, temples) in game world

### Additional Research Needed

- [ ] Research EVE Online's CSM (Council of Stellar Management) for meta-governance insights
- [ ] Analyze Ultima Online's town governance system
- [ ] Study ArcheAge's jury system for player justice
- [ ] Examine real-world civic tech platforms (Decidim, Your Priorities) for voting mechanics
- [ ] Research medieval guild structures for organizational hierarchy inspiration
- [ ] Study Burning Man's organizational structure for temporary community governance

## Related Documents

- [Social Interaction and Settlement System](../../docs/systems/social-interaction-settlement-system.md) - Current BlueMarble social system design
- [Game Design Document - Social Systems](../../docs/core/game-design-document.md) - Core GDD social mechanics section
- [Guild and Social System Designs](../../docs/gameplay/game-design-roles-guidelines.md) - Guild system guidelines
- [Economic System Design](../game-design/step-3-integration-design/economic-system-design.md) - Economic systems that interface with political systems
- [Player Created Quests vs Contracts Research](./player-created-quests-vs-contracts-research.md) - Player-driven content creation related to governance

## Tags

`social-systems` `player-governance` `religion-design` `mmorpg-politics` `player-agency` `community-design` `merit-systems` `dark-ages` `ethan-kennerly` `settlement-systems`

## Metadata

- **Author**: Research Agent
- **Date**: 2025-10-31
- **Status**: Complete
- **Research Method**: Web search and analysis of primary and secondary sources
- **Word Count**: ~1,800 words
- **Target Audience**: BlueMarble design team, social systems designers
