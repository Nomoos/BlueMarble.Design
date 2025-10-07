# Game Development Repositories Research

## Overview

This document catalogs open-source game development repositories that provide valuable insights into **RPG, MMORPG,
and top-down game development**. The focus is on projects with strong documentation, clear design practices, and
research-oriented content that can inform BlueMarble's development.

**Research Date:** January 2025  
**Focus Areas:** RPGs, MMORPGs, Top-Down Games, Game Engines/Frameworks  
**Selection Criteria:**

- Quality documentation (design docs, wikis, technical notes)
- Game design artifacts (mechanics, balance, narrative structure)
- Research-related content (AI, networking, procedural systems, databases)
- Active maintenance or significant community following

---

## RPG Frameworks & Engines

### 1. Godot Engine

**Repository:** <https://github.com/godotengine/godot>  
**Maintainer:** Godot Engine Community  
**Tech Stack:**

- Language: C++, GDScript, C#
- Rendering: Custom 2D/3D engine
- Networking: Built-in high-level multiplayer API
- Database: Integrates with various backends

**Key Documentation:**

- Comprehensive official documentation at docs.godotengine.org
- 2D and 3D game tutorials with RPG examples
- Networking and multiplayer architecture guides
- Node-based scene system documentation

**Why Useful for BlueMarble:**  
Godot is a proven open-source game engine with excellent 2D top-down support. Its documentation demonstrates best
practices for scene management, multiplayer networking, and modular game architecture. The high-level multiplayer
API and RPC system provide insights into client-server communication patterns suitable for MMORPGs.

---

### 2. Flare Engine (Flare RPG)

**Repository:** <https://github.com/flareteam/flare-engine>  
**Maintainer:** Flare Team  
**Tech Stack:**

- Language: C++
- Rendering: SDL2
- Data Format: JSON/XML for content
- Audio: SDL_mixer

**Key Documentation:**

- Game design philosophy in documentation
- Content creation guides (maps, items, NPCs)
- Action RPG mechanics implementation
- Modding and content pipeline documentation

**Why Useful for BlueMarble:**  
Flare demonstrates a clean separation between engine and game content, with all game data defined in human-readable
files. The documentation covers isometric/top-down rendering techniques, inventory systems, skill trees, and enemy
AI—all relevant for RPG development. The content pipeline shows how to structure game data effectively.

---

### 3. OpenMW (Morrowind Engine Recreation)

**Repository:** <https://github.com/OpenMW/openmw>  
**Maintainer:** OpenMW Team  
**Tech Stack:**

- Language: C++
- Rendering: OpenSceneGraph
- Scripting: Lua
- Physics: Bullet Physics
- Networking: In development for multiplayer (TES3MP fork)

**Key Documentation:**

- Comprehensive wiki covering engine architecture
- Game mechanics reverse-engineering documentation
- Quest system and scripting documentation
- World format and data structure documentation

**Why Useful for BlueMarble:**  
OpenMW provides deep insights into recreating complex RPG systems. The documentation details world persistence,
save game systems, NPC AI, quest logic, and dialogue systems. The TES3MP multiplayer fork demonstrates approaches
to adding networking to single-player RPG engines—highly relevant for understanding MMORPG architecture challenges.

---

## MMORPG Servers & Networking

### 4. TrinityCore

**Repository:** <https://github.com/TrinityCore/TrinityCore>  
**Maintainer:** TrinityCore Community  
**Tech Stack:**

- Language: C++
- Database: MySQL/MariaDB
- Networking: Custom TCP protocol with authentication server
- Scripting: C++ scripts for game content

**Key Documentation:**

- Server architecture documentation
- Database schema documentation (auth, characters, world)
- Networking protocol analysis via community wiki
- Opcode documentation and packet handlers
- Installation and configuration guides

**Why Useful for BlueMarble:**  
TrinityCore is the gold standard for MMORPG server emulation. Its dual-server architecture (auth + world servers),
database design, and opcode-based networking protocol provide battle-tested patterns for massive multiplayer games.
The separation of concerns between authentication, world simulation, and data persistence offers valuable
architectural insights. Documentation covers player movement synchronization, instance management, and guild systems.

---

### 5. AzerothCore

**Repository:** <https://github.com/azerothcore/azerothcore-wotlk>  
**Maintainer:** AzerothCore Community  
**Tech Stack:**

- Language: C++
- Database: MySQL
- Module System: Modular architecture with plugin support
- Networking: Based on TrinityCore protocol

**Key Documentation:**

- Modular architecture documentation
- Module development guides
- Database migration system
- Community-contributed guides for various systems

**Why Useful for BlueMarble:**  
AzerothCore's modular plugin architecture demonstrates how to build extensible MMORPG servers. The module system
allows clean separation of core functionality from custom features—essential for maintainable large-scale projects.
Documentation includes best practices for extending server functionality without modifying core code, relevant for
BlueMarble's potential plugin ecosystem.

---

### 6. Veloren

**Repository:** <https://github.com/veloren/veloren>  
**Maintainer:** Veloren Community  
**Tech Stack:**

- Language: Rust
- Rendering: wgpu (Vulkan/DX12/Metal)
- Networking: Custom UDP-based protocol
- Database: PostgreSQL
- Voxel Engine: Custom implementation

**Key Documentation:**

- Developer documentation covering architecture
- Network protocol documentation
- Entity Component System (ECS) design
- Procedural generation algorithms
- Physics and collision systems

**Why Useful for BlueMarble:**  
Veloren showcases modern MMORPG development in Rust with excellent documentation of networking, procedural world
generation, and ECS architecture. The project's focus on performance and concurrency provides insights into
handling many simultaneous players. Documentation covers interest management (only sending relevant updates to
players), chunk streaming, and server-client prediction—critical for any MMORPG.

---

### 7. Ryzom Core

**Repository:** <https://github.com/ryzom/ryzomcore>  
**Maintainer:** Ryzom Team / Winch Gate Property Limited  
**Tech Stack:**

- Language: C++
- Database: MySQL
- Scripting: Custom scripting system
- Networking: Custom protocol

**Key Documentation:**

- Server architecture overview
- Client-server communication patterns
- AI system documentation (NeL AI)
- World management and sharding documentation

**Why Useful for BlueMarble:**  
Ryzom is a complete commercial MMORPG released as open source, providing rare insights into production MMO
architecture. Documentation covers dynamic event systems, advanced AI behaviors, and world continuity across
server shards. The skill-based progression system (no classes) and the documentation of its implementation offers
alternative approaches to traditional level-based systems.

---

## Top-Down Gameplay Systems

### 8. Godot RPG Tutorial Projects

**Repository:** <https://github.com/GDQuest/godot-demos>  
**Maintainer:** GDQuest  
**Tech Stack:**

- Language: GDScript
- Engine: Godot
- Patterns: State machines, signals, scene composition

**Key Documentation:**

- Best practices for Godot game architecture
- RPG combat system examples
- Inventory and equipment system demos
- State machine implementations for AI

**Why Useful for BlueMarble:**  
GDQuest provides production-quality examples of top-down game mechanics with detailed explanations. The demos
show practical implementation of common RPG systems: inventory, dialogue, combat, exploration. Code is well-
commented and follows Godot best practices, making it an excellent reference for design patterns applicable
to top-down MMORPGs.

---

### 9. Solarus Engine

**Repository:** <https://github.com/solarus-games/solarus>  
**Maintainer:** Solarus Team  
**Tech Stack:**

- Language: C++ core, Lua scripting
- Rendering: SDL2
- Audio: OpenAL
- Map Format: Custom tile-based format

**Key Documentation:**

- Complete Lua API documentation
- Map editor documentation
- Game design tutorial (creating a Zelda-like)
- Entity and movement system documentation

**Why Useful for BlueMarble:**  
Solarus specializes in 2D action-adventure games with top-down perspective (Zelda-like). Documentation covers
tile-based map systems, entity collision, pathfinding, and scripted events. The clear separation between engine
(C++) and game logic (Lua) provides insights into designing scriptable game systems—valuable for allowing
content creators to extend BlueMarble without C# knowledge.

---

### 10. NetHack

**Repository:** <https://github.com/NetHack/NetHack>  
**Maintainer:** NetHack DevTeam  
**Tech Stack:**

- Language: C
- Display: Terminal/ASCII with tile support
- Networking: Remote play via telnet
- World: Procedurally generated dungeons

**Key Documentation:**

- Extensive in-code documentation
- Design philosophy discussions (Guidebook)
- Procedural generation algorithms
- Complex game mechanics documentation

**Why Useful for BlueMarble:**  
NetHack is one of the oldest continuously developed games (1987-present) with sophisticated mechanics. While not
graphical, its documentation reveals deep RPG system design: complex item interactions, emergent gameplay,
procedural generation, and permadeath mechanics. The decades of refinement provide insights into balanced,
engaging roguelike RPG design that could inform BlueMarble's systems.

---

## General Reusable Libraries & Tools

### 11. ENet (Networking Library)

**Repository:** <https://github.com/lsalzman/enet>  
**Maintainer:** Lee Salzman  
**Tech Stack:**

- Language: C
- Protocol: Custom UDP-based reliable transport
- Platform: Cross-platform

**Key Documentation:**

- Protocol specification
- API documentation
- Reliability and flow control algorithms
- Comparison with TCP and raw UDP

**Why Useful for BlueMarble:**  
ENet provides a battle-tested networking layer specifically designed for games. Documentation explains the
tradeoffs between TCP and UDP, and how to implement reliability selectively. Many games (including some open-source
MMOs) use ENet, making its design patterns industry-proven. Understanding its approach to packet reliability,
ordering, and fragmentation informs network protocol decisions.

---

### 12. Recast & Detour (Navigation Mesh)

**Repository:** <https://github.com/recastnavigation/recastnavigation>  
**Maintainer:** Recast Navigation Community  
**Tech Stack:**

- Language: C++
- Purpose: Navmesh generation and pathfinding
- Integration: Used in Unity, Unreal, Godot, custom engines

**Key Documentation:**

- Navmesh generation algorithms
- Dynamic obstacle handling
- Crowd simulation documentation
- A* pathfinding implementation details

**Why Useful for BlueMarble:**  
Recast/Detour is the industry standard for navigation meshes, used by major game engines. Documentation covers
spatial reasoning, pathfinding optimization, and handling dynamic worlds—essential for NPC AI in MMORPGs. The
algorithms for turning 3D geometry into navigable meshes and efficiently finding paths through complex environments
are directly applicable to BlueMarble's world simulation.

---

### 13. Tiled Map Editor & TMX Format

**Repository:** <https://github.com/mapeditor/tiled>  
**Maintainer:** Tiled Contributors  
**Tech Stack:**

- Language: C++ (Qt)
- Data Format: TMX (XML), JSON variants
- Purpose: 2D tile-based map editing

**Key Documentation:**

- TMX format specification
- Layer types and properties documentation
- Object and collision documentation
- Integration guides for various engines

**Why Useful for BlueMarble:**  
Tiled is the de facto standard for 2D top-down map editing. Documentation of the TMX format reveals best practices
for storing tile-based world data, layer composition, and custom properties. The format's flexibility and
widespread adoption make it a reference for designing world data structures. Understanding how Tiled handles
large maps, infinite maps, and tile properties informs BlueMarble's spatial data architecture.

---

### 14. Entity Component System (EnTT)

**Repository:** <https://github.com/skypjack/entt>  
**Maintainer:** Michele Caini  
**Tech Stack:**

- Language: C++ (header-only)
- Paradigm: Entity Component System
- Performance: Cache-friendly, compile-time optimization

**Key Documentation:**

- ECS architecture and philosophy
- Performance analysis and benchmarks
- Component storage strategies
- System iteration patterns

**Why Useful for BlueMarble:**  
EnTT demonstrates modern ECS architecture with excellent performance characteristics. Documentation explains why
ECS is superior for game development (compared to inheritance hierarchies), how to structure components for
cache efficiency, and patterns for system interactions. While BlueMarble uses C#, the architectural principles
translate directly and inform decisions about entity management in a large-scale multiplayer environment.

---

### 15. Phaser (2D Game Framework)

**Repository:** <https://github.com/photonstorm/phaser>  
**Maintainer:** Photon Storm  
**Tech Stack:**

- Language: JavaScript/TypeScript
- Rendering: WebGL, Canvas
- Physics: Arcade, Matter.js
- Audio: Web Audio API

**Key Documentation:**

- Extensive API documentation
- Game design patterns and examples
- State management and scene handling
- Multiplayer examples with Socket.IO

**Why Useful for BlueMarble:**  
While Phaser is browser-based, its documentation provides excellent examples of 2D game architecture and design
patterns. The scene management system, state machines, and event-driven architecture are applicable to any game.
Multiplayer examples demonstrate client-side prediction and server reconciliation techniques essential for smooth
online gameplay. TypeScript examples show strong typing patterns beneficial for large codebases.

---

## Summary of Key Learnings

### Architecture Patterns

1. **Dual-server architecture** (auth + world) from TrinityCore/AzerothCore
2. **Modular plugin systems** for extensibility (AzerothCore)
3. **ECS architecture** for entity management (Veloren, EnTT)
4. **Data-driven design** with external content files (Flare, Solarus, Tiled)

### Networking Insights

1. **Opcode-based protocols** for efficient binary communication (TrinityCore, AzerothCore)
2. **Interest management** to reduce bandwidth (Veloren)
3. **Client-server prediction** for responsive gameplay (Phaser multiplayer examples)
4. **UDP with selective reliability** (ENet, Veloren)

### Game Systems Design

1. **Skill-based progression** alternatives to level-based (Ryzom)
2. **Procedural generation** for content variety (NetHack, Veloren)
3. **Navigation and AI** patterns (Recast/Detour, OpenMW)
4. **Inventory and equipment** systems (Flare, GDQuest examples)

### Documentation Approaches

1. **Architecture overviews** before diving into implementation
2. **Code examples** alongside conceptual explanations
3. **Performance analysis** and optimization guides
4. **Integration guides** for extensibility

---

## Recommended Next Steps

### For BlueMarble Development

1. **Study TrinityCore's dual-server architecture** - Applicable to BlueMarble's gateway/world server design
2. **Review Veloren's ECS and networking** - Modern approach to MMORPG architecture in a systems language
3. **Examine Tiled's data format** - Inform BlueMarble's world data structure and tooling decisions
4. **Analyze ENet's protocol design** - Understand reliability/performance tradeoffs for networking
5. **Explore Recast/Detour for NPC AI** - Essential for sophisticated NPC behaviors and pathfinding

### Documentation Strategies

1. Adopt **architecture-first documentation** pattern seen in Veloren and OpenMW
2. Include **code examples** with every major system (like GDQuest demos)
3. Document **performance considerations** upfront (like EnTT)
4. Create **integration guides** for extensibility (like AzerothCore modules)

### Technical Considerations

1. Evaluate **ECS vs. traditional OOP** for entity management based on EnTT and Veloren insights
2. Consider **Lua or similar scripting** for content creators (Solarus model)
3. Plan **modular architecture** early to allow community extensions (AzerothCore approach)
4. Design **data formats** for human readability and version control (Flare, Tiled approach)

---

## Related BlueMarble Documentation

- [Technical Design Document](../core/technical-design-document.md) - Server architecture overview
- [WoW Emulator Architecture Research](../../research/topics/wow-emulator-architecture-networking.md) - Deep dive
  into MMORPG networking
- [System Architecture Design](../systems/system-architecture-design.md) - BlueMarble's current architecture
- [Game Design Research](../../research/game-design/) - Game mechanics and design philosophy

---

**Research Compiled By:** BlueMarble Research Team  
**Last Updated:** January 2025  
**Status:** Complete  
**Next Review:** Q2 2025 (check for new relevant projects)
