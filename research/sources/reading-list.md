# Reading List

Curated reading list for BlueMarble research and development.

**Last Updated:** 2025-01-24  
**Total Sources:** 117 (60 books + 13 survival collections + 23 academic theses + 21 online resources)  
**Completed Analysis:** 12 sources  
**Status:** Active - Tracking all research literature sources

---

## Critical Priority (MMORPG Core Systems)

### Multiplayer Architecture

- [x] **Multiplayer Game Programming: Architecting Networked Games** by Joshua Glazer, Sanjay Madhav - Complete analysis (game-dev-analysis-02-multiplayer-programming.md)
- [ ] **Network Programming for Games: Real-Time Multiplayer Systems** - Critical for MMORPG scalability, real-time protocols, distributed systems
- [ ] **Game Engine Architecture (3rd Edition)** by Jason Gregory - Comprehensive engine architecture, subsystem integration, networking for multiplayer
- [ ] **MMO Architecture: Source Code and Insights** - Direct MMORPG implementation patterns

### Open Source Reference Implementations

- [x] **TrinityCore** - WoW emulator architecture reference (referenced in wow-emulator-architecture-networking.md)
- [x] **CMaNGOS** - MMORPG server architecture reference (referenced in wow-emulator-architecture-networking.md)
- [x] **AzerothCore** - Modern MMORPG server implementation (referenced in wow-emulator-architecture-networking.md)
- [x] **wowdev.wiki** - WoW protocol and architecture documentation (referenced in wow-emulator-architecture-networking.md)

---

## High Priority (Core Game Systems)

### Game Programming

- [ ] **Real-Time Rendering (4th Edition)** by Tomas Akenine-Möller, Eric Haines, Naty Hoffman - Graphics pipeline optimization, LOD systems for planetary scale
- [ ] **AI for Games (3rd Edition)** by Ian Millington, John Funge - NPC AI, pathfinding, decision-making architectures
- [ ] **Game Programming Patterns** by Robert Nystrom - Design patterns for game development (online: https://gameprogrammingpatterns.com/)

### Game Design Theory

- [ ] **The Art of Game Design: A Book of Lenses (3rd Edition)** by Jesse Schell - Core design philosophy, player psychology, design frameworks
- [ ] **Level Up! The Guide to Great Video Game Design (2nd Edition)** by Scott Rogers - Level design, RPG mechanics, combat design
- [ ] **Introduction to Game Systems Design** - Core loops, system interaction, progression systems
- [ ] **Advanced Game Design** - Emergence, complexity, asymmetric balance
- [ ] **Players Making Decisions** - Player psychology, meaningful choices, risk/reward
- [ ] **Fundamentals of Game Design** - Genre conventions, player types, core mechanics
- [ ] **Games, Design and Play** - Iterative design, playtesting, prototyping

### Survival Knowledge Collections

- [x] **awesome-survival Repository Overview** - Comprehensive analysis completed (survival-guides-knowledge-domains-research.md)
- [x] **OpenStreetMap Data** - Geographic data for world generation (survival-content-extraction-01-openstreetmap.md)
- [x] **Appropriate Technology Library** - 1,050+ ebooks on crafting and technology (survival-content-extraction-02-appropriate-technology.md)
- [x] **Survivor Library Collection** - Historical technology documentation (survival-content-extraction-03-survivor-library.md)
- [x] **Great Science Textbooks Collection** - 88.9 GB of engineering textbooks (survival-content-extraction-04-great-science-textbooks.md)
- [x] **Military Manuals Collection** - 22,000+ tactical and technical manuals (survival-content-extraction-05-military-manuals.md)
- [x] **Medical Textbooks Collection** - Healthcare systems (survival-content-extraction-06-medical-textbooks.md)
- [x] **Encyclopedia Collections** - Britannica, Americana cross-reference (survival-content-extraction-07-encyclopedia-collections.md)
- [x] **Communication Systems Documentation** - Radio, encryption, mesh networks (survival-content-extraction-08-communication-systems.md)
- [x] **CD3WD Collection** - Community development resources (survival-content-extraction-09-cd3wd-collection.md)
- [ ] **Energy Systems Collection** - Solar, wind, hydroelectric, biofuel systems
- [ ] **Historical Maps and Navigation Resources** - Cartography, celestial navigation, surveying
- [ ] **Specialized Collections (Deep Web Sources)** - Niche technical resources

### Development Process

- [ ] **Agile Game Development** - Sprint planning, iteration process
- [ ] **Introduction to Game Design, Prototyping and Development** - Full pipeline, concept to completion

---

## Medium Priority (Supporting Systems)

### Performance Optimization (C# vs C++)

- [ ] **Writing High-Performance .NET Code (2nd Edition)** by Ben Watson - Comprehensive guide to .NET/C# performance with comparisons to native code, GC optimization, benchmarking
- [ ] **Pro .NET Performance: Optimize Your C# Applications** by Sasha Goldshtein, Dima Zurbalev, Ido Flatow - Deep dive into .NET performance profiling with native code comparisons
- [ ] **C++ High Performance (2nd Edition)** by Bjorn Andrist, Viktor Sehr - Modern C++ performance optimization with benchmark comparisons to managed languages including C#
- [ ] **Optimized C++: Proven Techniques for Heightened Performance** by Kurt Guntheroth - C++ optimization strategies with comparisons to managed languages
- [ ] **CLR via C# (4th Edition)** by Jeffrey Richter - Essential for understanding C# performance vs native code, CLR internals
- [ ] **Pro .NET Memory Management** by Konrad Kokosa - Deep analysis of managed vs unmanaged memory performance
- [ ] **C# Performance Best Practices** by Nick Harrison - Practical C# performance with native interop discussions
- [ ] **High-Performance C# Programming** by Jason Alls - Modern C# performance techniques with .NET 6/7
- [ ] **Effective C++ (3rd Edition)** by Scott Meyers - Industry standard C++ optimization reference
- [ ] **More Effective C++** by Scott Meyers - Advanced C++ optimization patterns

### RPG and Top-Down Development

- [ ] **Game Programming Algorithms and Techniques** - Pathfinding, procedural generation, spatial partitioning
- [ ] **A Game Design Vocabulary** - Design terminology, communication frameworks
- [ ] **Procedural Generation in Game Design** - World generation techniques
- [ ] **Database Design for MMORPGs** - Data architecture for persistent worlds
- [ ] **Virtual Economies: Design and Analysis** by Vili Lehdonvirta, Edward Castronova - Economic system design

### Content Creation

- [ ] **Learning Blender** - 3D modeling, asset pipeline, optimization
- [ ] **[digital]Visual Effects and Compositing** - VFX systems, particle effects
- [ ] **Writing Interactive Music for Video Games** - Dynamic music systems, adaptive audio
- [ ] **3D User Interfaces** - Spatial UI, diegetic interfaces

### Engine-Specific (Reference)

- [ ] **2D Game Development with Unity** by Jared Halpern - Unity 2D techniques
- [ ] **Unity 2D Game Development Cookbook** by Claudio Scolastici - Unity recipes
- [x] **Godot Engine Documentation** (docs.godotengine.org) - Referenced in game_dev_repos.md
- [ ] **Unity Learn - RPG Development** - Official Unity RPG tutorials
- [ ] **Unreal Engine Documentation** - Unreal reference

### Open Source Engines and Tools

- [x] **Godot Engine** - Open source game engine (referenced in game_dev_repos.md)
- [x] **Flare Engine** - Open source ARPG for top-down games (referenced in game_dev_repos.md)
- [x] **OpenMW** - Open source Morrowind engine (referenced in game_dev_repos.md)
- [x] **Recast & Detour** - Navigation mesh toolset (referenced in game_dev_repos.md)
- [ ] **ENet** - Lightweight networking library
- [ ] **RakNet** - Open source networking middleware

---

## High Priority (Academic Theses - Geoinformatics and GIS)

### Highly Relevant to BlueMarble

These master's theses from Palacký University Olomouc (2022) contain relevant research for game development, GIS integration, and spatial visualization.

- [ ] **Digital twins in the context of disaster preparedness: fusion of GIS and game engines** by Deligant Anatole - **Critical**: Directly relevant to GIS-game engine integration for planetary simulation
- [ ] **Portál pro simulační hru Spationomy** by Roller Jan - **High**: Portal for simulation game, geospatial game design patterns
- [ ] **GeoVisualization of Football Players Movement** by Liu Nianhua - Player movement tracking and visualization techniques applicable to MMORPG player systems

### Supporting Research (Medium Priority)

- [ ] **Aplikace metody geodesignu v prostředí ArcGIS GeoPlanner** by Bittner Oldřich - Geodesign method application, spatial planning
- [ ] **VYUŽITÍ ARCGIS CITYENGINE PRO BEZPEČNOSTNÍ PLÁNOVÁNÍ** by Čáp Daniel - 3D city modeling for security planning
- [ ] **Hodnocení míry abstrakce u 3D modelů pro osoby se zrakovým postižením** by Forgáč Jakub - 3D model abstraction and accessibility design
- [ ] **Pokročilá analýza a vizualizace dat z dotazníkového šetření** by Fryčák Filip - Advanced data analysis and visualization
- [ ] **Webové řešení pro vizualizaci nejistoty dat z monitoringu zimování včelstev** by Kuchejdová Magdalena - Web-based data uncertainty visualization
- [ ] **IMPROVING SEN2CUBE.AT WEB APPLICATION VISUALIZATION CAPABILITIES** by Nurul Fatma - Web application visualization improvements
- [ ] **User Evaluation of COVID-19 Dashboards** by Porti Suarez Anna - Dashboard design and user evaluation
- [ ] **Podobnost evropských měst a jejich funkčních území** by Urbančík Filip - City similarity analysis and functional areas
- [ ] **Využití metody think-aloud v kartografickém výzkumu** by Vaníček Tomáš - Think-aloud methodology for UX research
- [ ] **Testování možností záznamu pohybu očí pomocí web kamery** by Jílková Monika - Eye tracking using web camera

### Reference Materials (Low Priority)

- [ ] **Aplikace vyhledávání kolokačních vzorů na prostorová data** by Bučková Simona - Collocation pattern search on spatial data
- [ ] **Termální mapování vybraných ploch města Olomouce** by Ďuriančíková Petra - Thermal mapping of urban areas
- [ ] **Hodnocení změn v intenzitách dopravy ve vybraných regionech Česka** by Hubáček Ondřej - Traffic intensity analysis
- [ ] **ANALYSIS AND GEOVISUALISATION OF BIODIVERSITY MONITORING DATA** by Christie Ella - Biodiversity data geovisualization
- [ ] **Důsledky pandemie COVID-19 na mezinárodní mobilitu v Evropě** by Kačírková Tereza - COVID-19 impact on mobility
- [ ] **Stanovení metrik sněhové pokrývky pomocí metod fotogrammetrie** by Pajdová Marie - Snow cover metrics using remote sensing
- [ ] **Klasifikace a časová analýza osobní a nákladní vlakové dopravy** by Pospíšil Lukáš - Rail transport temporal analysis
- [ ] **VZTAH KVALITY ŽIVOTA K PŘÍSLUŠNOSTI K RURÁLNÍMU A URBÁNNÍMU PROSTORU** by Rypl Oldřich - Quality of life in rural vs urban spaces
- [ ] **PORTRAYAL OF LIKELY CLIMATE CHANGE IMPACTS ON BEEKEEPING** by Wang Yuan - Climate change visualization
- [ ] **Tyfloprůvodce po vybraných památkách Česka** by Žejdlík Jakub - Tactile guide design for accessibility

**Note:** All theses available online at https://www.geoinformatics.upol.cz/dprace/magisterske/

---

## Low Priority (Specialized Topics)

### Platform-Specific

- [ ] **Unity Game Development in 24 Hours** - Unity-specific quick reference
- [ ] **Unreal Engine VR Cookbook** - VR development patterns
- [ ] **Roblox Game Development in 24 Hours** - Platform-specific (likely not applicable)
- [ ] **Augmented Reality / Practical Augmented Reality** - AR concepts for potential mobile companion

### Case Studies and Research

- [ ] **World of Warcraft Programming** - WoW addon development insights
- [ ] **EVE Online: Large Scale Combat** - Large-scale battle systems
- [ ] **Procedural World Generation** - Advanced generation techniques
- [ ] **Developing Online Games: An Insider's Guide** by Jessica Mulligan, Bridgette Patrovsky - Industry case studies
- [ ] **Massively Multiplayer Game Development (Series)** - Historical MMO development

---

## Online Resources (Ongoing Reference)

### Documentation and Tutorials

- [ ] **Game Programming Patterns (Online Edition)** by Robert Nystrom - Free online with code examples
- [ ] **Gaffer on Games** by Glenn Fiedler - Excellent practical networking articles (https://gafferongames.com/)
- [ ] **Gamasutra/Game Developer Articles** - Industry articles and postmortems
- [ ] **GameDev.net** - Community tutorials and resources
- [ ] **GDQuest - Godot Tutorials** - Video tutorials for Godot
- [ ] **Brackeys (Historical Archive)** - Classic Unity tutorials
- [ ] **Sebastian Lague** - Procedural generation tutorials
- [ ] **Code Monkey** - Unity game development
- [ ] **GameDev.tv** - Course platform
- [ ] **GDC (Game Developers Conference)** - Industry talks and presentations
- [ ] **"1500 Archers on a 28.8"** - Age of Empires networking GDC talk
- [ ] **Tribes Networking Model** - Mark Frohnmayer's pioneering FPS networking
- [ ] **Quake 3 Source Code** - Reference implementation (https://github.com/id-Software/Quake-III-Arena)

### Communities and Forums

- [ ] **Reddit - r/gamedev** - General game development community
- [ ] **Reddit - r/MMORPG** - MMORPG-specific discussions
- [ ] **GameDev Stack Exchange** - Q&A for developers
- [ ] **Unity Forums** - Unity-specific help
- [ ] **Unreal Engine Forums** - Unreal-specific help

### Case Study Games

- [ ] **EVE Online** - Large-scale space MMORPG systems (GDC talks)
- [ ] **World of Warcraft** - Traditional MMORPG architecture
- [ ] **Ultima Online** - Historical MMORPG design
- [ ] **RuneScape (Old School)** - Browser-based MMORPG patterns

---

## Completed Analysis Documents

### Research Literature

- [x] **Game Programming in C++** - Detailed analysis (game-dev-analysis-01-game-programming-cpp.md, 1,150 lines)
- [x] **Multiplayer Game Programming** - Comprehensive networking analysis (game-dev-analysis-02-multiplayer-programming.md, 1,000 lines)
- [x] **Tabletop RPG Mechanics Overview** - 9 RPGs analyzed (game-design-mechanics-analysis.md, 1,263 lines)
- [x] **Game Development Resources Overview** - 20+ books organized (game-development-resources-analysis.md, 702 lines)
- [x] **Online Game Development Resources Catalog** - Active auto-growing list (online-game-dev-resources.md, 880+ lines)

### Survival Content Extraction (10 completed)

All survival extraction guides completed with detailed methodology, JSON schemas, and implementation plans.

---

## Priority Legend

- **Critical**: Essential for core MMORPG functionality (networking, architecture)
- **High**: Core game systems (AI, rendering, design theory, survival content)
- **Medium**: Supporting features and tools (content creation, specialized systems)
- **Low**: Platform-specific or specialized topics

## Status Legend

- **[ ]** - Not yet analyzed
- **[x]** - Analysis completed with documentation
- **🔍** - Currently in progress
- **⏳** - Queued for analysis

---

## Notes

### Reading Strategy

1. **Phase 1 (Current)**: Focus on critical multiplayer architecture and networking books
2. **Phase 2**: Deep dive into high-priority game design and AI systems
3. **Phase 3**: Content creation and specialized systems
4. **Ongoing**: Reference documentation, community resources, and case studies

### Cross-References

- All sources tracked in `master-research-queue.md`
- BibTeX entries available in `sources.bib`
- Research assignment groups (1-40) contain detailed analysis tasks
- Completed analyses stored in `research/literature/` directory

### Next Actions

1. Begin detailed analysis of critical multiplayer programming books
2. Continue extraction from survival collections (3 pending: Energy Systems, Historical Maps, Specialized Collections)
3. Document case studies from successful MMORPGs
4. Build code examples and prototypes based on book recommendations
