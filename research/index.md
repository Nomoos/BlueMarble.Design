# Research Index

Master index of all research notes, experiments, and sources for the BlueMarble project.

## Topics

Research topics are small, focused notes on specific areas of interest. Each topic is self-contained
and typically 200-400 lines.

- [Topic files](topics/) - Individual research topics

### Available Topics

#### Coordinate System Research (Int32 Centimeters)

- **[Data Type Optimization - Executive Summary](topics/data-type-optimization-executive-summary.md)** - 
  Quick reference guide comparing float, double, and Int32 for 20,000 km world. Includes decision matrix, 
  performance benchmarks, and implementation timeline. **Start here for overview.**

- **[ADR-001: Coordinate Data Type Selection](topics/adr-001-coordinate-data-type-selection.md)** - 
  Architectural Decision Record proposing Int32 (centimeters) for world coordinates optimized for geological simulations. 
  Documents decision rationale, consequences, 2-week implementation timeline, and review criteria

- [Coordinate Data Type Optimization](topics/coordinate-data-type-optimization.md) - 
  Comprehensive analysis of float, double, and Int32 data types for storing world dimensions and height up to 
  20,000 km. Includes precision calculations, performance benchmarks, memory implications, and recommendations for 
  BlueMarble engine implementation with emphasis on geological simulation accuracy

- **[Int32 Coordinate Implementation Specification](topics/int32-coordinate-implementation-specification.md)** - 
  Detailed implementation guide for BlueMarble.Core with complete C# code examples, data structures, API design, 
  octree integration, rendering pipeline, and 2-week phased implementation plan

- [Int32 Coordinate Database Schema](topics/int32-coordinate-database-schema.md) - 
  Complete database schema design for PostgreSQL and Cassandra with Int32 coordinates, including indexing strategies, 
  query patterns, partitioning, Morton encoding, and performance optimization techniques

- [Int32 Coordinate Network Protocol](topics/int32-coordinate-network-protocol.md) - 
  Binary network protocol with delta encoding for efficient transmission of Int32 coordinates. Includes bandwidth 
  analysis, compression strategies, adaptive update rates, and client-side interpolation

#### Game Design Topics

- [Game Design Roles and Types](topics/game-design-roles-and-types.md) - 
  Comprehensive overview of game design specializations (Systems, Combat, Economy, Level, Narrative, UX, Progression, 
  Meta-Game designers), their responsibilities, tools, and methodological approaches for modern game development

- [World of Warcraft Emulators – Architecture & Communication](topics/wow-emulator-architecture-networking.md) -
  MMO server architecture, authentication systems, and networking protocols from open-source WoW emulators
- [Gender Influence on Macro vs. Micro-Level Aiming Preferences](topics/gender-macro-micro-aiming-preferences.md) -
  Research examining whether gender influences preference for strategic planning (macro) vs. tactical control (micro) in gameplay
  
- [Why Don't Women Play Games? - Gender Barriers in Gaming](topics/why-dont-women-play-games.md) -
  Analysis of historical and cultural barriers preventing women's full participation in gaming, with actionable insights for inclusive game design

- [Board Games in Video Games and Regulatory Considerations](topics/board-games-in-video-games-and-regulatory-considerations.md) -
  Research on implementing traditional board games (Chess, Shogi, Go, Mancala) as mini-games in MMORPGs, with comprehensive analysis of cryptocurrency integration and gambling regulations

- [Spire: The City Must Fall - Universe Knowledge Base](topics/spire-the-city-must-fall-universe-knowledge-base.md) -
  Comprehensive reference document covering Spire's dark fantasy setting, resistance mechanics, stress/fallout system, character classes, factions, themes, and design lessons applicable to BlueMarble

- [Spatial Analysis: Hydrology Tools](topics/spatial-analysis-hydrology-tools.md) -
  Research on flow direction, basin delineation, and drainage analysis from ArcGIS Spatial Analyst for realistic water flow simulation and terrain watershed modeling in BlueMarble's geological MMORPG

- [Spatial Analysis: Groundwater Tools](topics/spatial-analysis-groundwater-tools.md) -
  Analysis of Darcy flow and subsurface water movement for implementing realistic aquifer systems, mining water management, and underground fluid dynamics in BlueMarble's deep mining mechanics

- [Spatial Analysis: Interpolation Tools](topics/spatial-analysis-interpolation-tools.md) -
  Examination of IDW, Kriging, Spline, and Natural Neighbor interpolation methods for procedural terrain generation, multi-scale detail, and smooth transitions in BlueMarble's planetary-scale world

- [Spatial Analysis: Generalization Tools](topics/spatial-analysis-generalization-tools.md) -
  Study of aggregation, boundary cleaning, and filtering techniques for level-of-detail systems, data simplification, and multi-resolution terrain representation in BlueMarble

- [Spatial Analysis: Map Algebra](topics/spatial-analysis-map-algebra.md) -
  Research on local, focal, zonal, and global raster operations for terrain analysis, resource distribution, and spatial calculations in BlueMarble's simulation systems

#### UI/UX and Display Systems

- [Dark Ages Display Systems Research](topics/dark-ages-display-systems-research.md) -
  Comprehensive analysis of medieval/dark ages UI design patterns from games like Graveyard Keeper, including pixel art techniques, panel-based layouts, medieval iconography, crafting/inventory interfaces, and AAA game examples (Medieval Dynasty, Kingdom Come, Crusader Kings). Documents design implications for BlueMarble's geological MMORPG

## Notes

Informal research notes and brainstorming for ideas in development.

- [Notes directory](notes/) - Quick observations, ideas, and work-in-progress research

## Experiments

Structured experiment logs documenting hypotheses, methods, results, and decisions.

- [Experiment logs](experiments/) - Dated experiment reports

## Literature

Formal references, summaries, and literature reviews from external sources.

- [Literature directory](literature/) - Summaries and reviews of academic papers and technical documentation

### Available Literature Reviews

- [Survival Guides Knowledge Domains Research](literature/survival-guides-knowledge-domains-research.md) - 
  Analysis of survival knowledge systems from the awesome-survival repository, identifying skill hierarchies, 
  material processing chains, and knowledge preservation mechanics applicable to BlueMarble game design

- [Daily Routine Frustration and Cursor Accuracy Research](literature/game-dev-research-cursor-accuracy-gender-accessibility.md) - 
  Research on gender differences in motor skills, cursor accuracy requirements in games, and accessibility design 
  for routine-based systems. Analysis of The Sims' success and recommendations for BlueMarble's routine-based 
  progression system to attract underserved demographics (women, older players, disabled players)

### Content Extraction Guides (Priority Order)

- [Content Extraction 01: OpenStreetMap](literature/survival-content-extraction-01-openstreetmap.md) - 
  Practical guide for extracting terrain topology, biome distribution, resource placement, and settlement patterns 
  from OpenStreetMap data for planet-scale world generation
  
- [Content Extraction 02: Appropriate Technology Library](literature/survival-content-extraction-02-appropriate-technology.md) - 
  Systematic extraction of 500+ crafting recipes from 1,050 ebooks covering sustainable technology, agriculture, 
  metalworking, and construction for Tier 1-3 game content
  
- [Content Extraction 03: Survivor Library](literature/survival-content-extraction-03-survivor-library.md) - 
  Historical technology extraction from pre-industrial manuals (1700s-1950s) for 340+ authentic recipes covering 
  manufacturing, agriculture, construction, and specialized crafts

- [Content Extraction 04: Great Science Textbooks](literature/survival-content-extraction-04-great-science-textbooks.md) - 
  Advanced engineering and science extraction from 88.9 GB collection for 300+ Tier 4-5 recipes covering metallurgy, 
  chemical engineering, mechanical systems, electrical power, and energy systems

- [Content Extraction 05: Military Manuals](literature/survival-content-extraction-05-military-manuals.md) - 
  Large-scale warfare systems extraction from 22,000+ military field manuals covering logistics, tactics, fortification, 
  command structures, and intelligence for planet-scale MMORPG combat operations

- [Content Extraction 06: Medical Textbooks](literature/survival-content-extraction-06-medical-textbooks.md) - 
  Healthcare systems and pharmaceutical production extraction covering 250+ medical mechanics, disease management, 
  surgical procedures, and epidemic events for immersive medical gameplay

### Research Management

- [Master Research Queue](literature/master-research-queue.md) - 
  Comprehensive tracking document for all 31 identified research sources with status, priorities, and processing strategy

### Game Design Analysis Documents

- [Game Design Mechanics Analysis](literature/game-design-mechanics-analysis.md) - 
  Comprehensive analysis of innovative mechanics from 9 tabletop RPGs (Masks, Mazes, Outgunned, Spire, Warhammer, 
  Wildsea, Cyberpunk RED, Call of Cthulhu, Apocalypse World) with implementation patterns for BlueMarble
  
- [Game Development Resources Analysis](literature/game-development-resources-analysis.md) - 
  Organized guide to 20+ game development books covering programming, design theory, multiplayer systems, 
  content creation, and development process with prioritized reading list and application strategies

- [The Sims and Gaming Women Phenomenon](literature/game-dev-analysis-the-sims-and-gaming-women-phenomenon.md) - 
  Analysis of The Sims franchise's revolutionary impact on gender demographics in gaming, examining why the game 
  attracted 60-70% women players and lessons for inclusive game design

- [Gender Differences in Spatial Planning vs Combat Preferences](literature/game-dev-research-gender-spatial-planning-vs-combat-preferences.md) - 
  Comprehensive research examining whether women show stronger preferences for spatial planning mechanics (settlement 
  building, city layout, resource arrangement) versus combat aiming, with academic research synthesis, cognitive 
  studies analysis, and actionable design recommendations for BlueMarble MMORPG

## Sources

Bibliography, reading lists, and raw notes from external sources.

- [sources.bib](sources/sources.bib) - Bibliography
- [Reading List](sources/reading-list.md) - Curated reading list
- [Quotes](sources/quotes.md) - Notable quotes and excerpts
- [Content Design Research](game-design/step-1-foundation/content-design/) - Comprehensive research on content 
  design organized into focused topics: definition, workflow, video game RPG analysis, tabletop RPG analysis, 
  comparative analysis, professional practice, and BlueMarble applications. See the 
  [Content Design Index](game-design/step-1-foundation/content-design/README.md) for navigation.
- [Game Design Sources](game-design/game-sources.md) - Curated collection of game design, research, theory,
  and gamification sources
- [From Inspiration to Design Document](game-design/step-1-foundation/from-inspiration-to-design-document.md) - 
  Comprehensive guide on the game design process from inspiration to formal documentation, covering concept development,
  building game building blocks, and creating effective design documents
- [Narrative Inspiration: Sci-Fi Mining World](game-design/step-1-foundation/narrative-inspiration-sci-fi-mining-world.md) - 
  Science fiction narrative framework without magic, featuring multi-species mining colony, clone generation, controlled
  reproduction, and resource extraction under superior race oversight

## Market Research

Market analysis, competitive research, and industry trends.

- [Market Research](market-research/) - Market analysis and competitive landscape
- [Market Research Overview](market-research/market-research.md) - Competitive analysis and market positioning
- [Game Development Repositories](market-research/game_dev_repos.md) - Analysis of game development repositories and resources
- [Voxel Games Sources](market-research/voxel_games_sources.md) - Research on voxel-based games and technologies

## Existing Research Areas

For comprehensive research areas, see:

- [Spatial Data Storage](spatial-data-storage/) - Spatial data storage strategies
- [Game Design](game-design/) - Game design research and analysis
- [GPT Research](gpt-research/) - Research from AI conversations
- [Market Research](market-research/) - Market analysis and competitive landscape

## Contributing

When adding research:

1. Keep notes small and focused (one topic per file)
2. Use kebab-case filenames
3. Include front matter with metadata
4. Cross-link related research
5. Update this index

See [CONTRIBUTING.md](../CONTRIBUTING.md) for detailed guidelines.
