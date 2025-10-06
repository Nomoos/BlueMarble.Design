# Reading List

Curated reading list for BlueMarble research and development.

**Last Updated:** 2025-01-28  
**Total Sources:** 103 (75 books + 13 survival collections + 15 online resources)  
**Completed Analysis:** 17 sources  
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

### Spatial Data Structures and Compression

- [x] **Foundations of Multidimensional and Metric Data Structures** by Hanan Samet (2006) - Complete analysis (spatial-data-analysis-morton-code-octree-pointerless-storage.md, spatial-data-analysis-homogeneous-region-collapsing.md)
- [x] **Space-Filling Curves: An Introduction with Applications in Scientific Computing** by Michael Bader (2013) - Morton code theory (spatial-data-analysis-morton-code-octree-pointerless-storage.md)
- [x] **Data Compression: The Complete Reference (4th Edition)** by David Salomon, Giovanni Motta (2007) - RLE and compression techniques (spatial-data-analysis-hybrid-compression-strategies.md)
- [x] **Texturing and Modeling: A Procedural Approach (3rd Edition)** by David S. Ebert et al. (2003) - Procedural generation for compression (spatial-data-analysis-hybrid-compression-strategies.md)
- [x] **Database Internals** by Alex Petrov (2019) - LSM trees, Cassandra, compression (spatial-data-analysis-database-architecture.md, spatial-data-analysis-multi-layer-query-optimization.md)
- [x] **Designing Data-Intensive Applications** by Martin Kleppmann (2017) - Distributed systems and consistency (spatial-data-analysis-database-architecture.md, spatial-data-analysis-multi-layer-query-optimization.md)
- [x] **Redis in Action** by Josiah Carlson (2013) - Caching strategies (spatial-data-analysis-multi-layer-query-optimization.md)
- [x] **NoSQL Distilled** by Pramod J. Sadalage, Martin Fowler (2012) - NoSQL patterns (spatial-data-analysis-database-architecture.md)
- [x] **Adaptive Mesh Refinement: Theory and Applications** by Tomasz Plewa et al. (2005) - AMR theory (spatial-data-analysis-homogeneous-region-collapsing.md)
- [x] **Efficient Sparse Voxel Octrees** by Samuli Laine, Tero Karras (2010) - GPU octrees (spatial-data-analysis-morton-code-octree-pointerless-storage.md, spatial-data-analysis-homogeneous-region-collapsing.md)
- [x] **GigaVoxels** by Cyril Crassin et al. (2009) - Large-scale voxel rendering (spatial-data-analysis-hybrid-compression-strategies.md, spatial-data-analysis-homogeneous-region-collapsing.md)

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

### Spatial Data Storage Research (5 completed)

- [x] **Hybrid Compression Strategies for Petabyte-Scale Octree Storage** - Comprehensive analysis (spatial-data-analysis-hybrid-compression-strategies.md, 900+ lines)
- [x] **Morton Code Octree and Pointerless Storage** - Spatial indexing and memory optimization (spatial-data-analysis-morton-code-octree-pointerless-storage.md, 950+ lines)
- [x] **Multi-Layer Query Optimization for Read-Dominant Workloads** - Caching and performance (spatial-data-analysis-multi-layer-query-optimization.md, 950+ lines)
- [x] **Database Architecture for Petabyte-Scale 3D Octree Storage** - Cassandra + Redis architecture (spatial-data-analysis-database-architecture.md, 1,000+ lines)
- [x] **Homogeneous Region Collapsing for Octree Optimization** - Memory reduction strategies (spatial-data-analysis-homogeneous-region-collapsing.md, 1,000+ lines)

### Survival Content Extraction (10 completed)

All survival extraction guides completed with detailed methodology, JSON schemas, and implementation plans.

---

## Online Resources and Libraries

### Spatial Data and Compression

- [x] **libmorton** - Production-ready Morton code library (https://github.com/Forceflow/libmorton) - Referenced in spatial-data-analysis-morton-code-octree-pointerless-storage.md
- [x] **LZ4 Compression Library** - Fast compression for Cassandra (https://lz4.github.io/lz4/) - Referenced in spatial-data-analysis-hybrid-compression-strategies.md, spatial-data-analysis-database-architecture.md
- [x] **Zstandard (Zstd)** - Tunable compression algorithm (https://facebook.github.io/zstd/) - Referenced in spatial-data-analysis-hybrid-compression-strategies.md, spatial-data-analysis-database-architecture.md
- [x] **OpenVDB** - Sparse volume data structure (https://www.openvdb.org/) - Referenced in spatial-data-analysis-morton-code-octree-pointerless-storage.md, spatial-data-analysis-homogeneous-region-collapsing.md
- [x] **GPU Gems 2: Octree Textures on the GPU** - NVIDIA GPU techniques (https://developer.nvidia.com/gpugems/gpugems2) - Referenced in spatial-data-analysis-hybrid-compression-strategies.md

### Database and Caching

- [x] **Apache Cassandra Documentation** - Distributed database (https://cassandra.apache.org/doc/latest/) - Referenced in spatial-data-analysis-database-architecture.md, spatial-data-analysis-multi-layer-query-optimization.md
- [x] **Redis Documentation** - In-memory caching (https://redis.io/docs/) - Referenced in spatial-data-analysis-multi-layer-query-optimization.md, spatial-data-analysis-database-architecture.md
- [x] **DataStax Academy** - Free Cassandra training (https://www.datastax.com/dev/academy) - Referenced in spatial-data-analysis-database-architecture.md
- [x] **The Last Pickle Blog** - Cassandra expertise (https://thelastpickle.com/blog/) - Referenced in spatial-data-analysis-database-architecture.md, spatial-data-analysis-multi-layer-query-optimization.md
- [x] **Netflix Technology Blog** - Cassandra at scale case studies (https://netflixtechblog.com/) - Referenced in spatial-data-analysis-database-architecture.md, spatial-data-analysis-multi-layer-query-optimization.md
- [x] **Discord Engineering Blog** - Scaling to trillions of messages (https://discord.com/blog/) - Referenced in spatial-data-analysis-database-architecture.md, spatial-data-analysis-multi-layer-query-optimization.md

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
