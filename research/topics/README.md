# Research Topics

This directory contains small, focused research notes on specific topics. Each file should be
self-contained and cover one research question or area.

## Guidelines

- **One topic per file**: Each file covers a single, focused research question
- **File size**: Target 200-400 lines (~500-800 words)
- **Naming**: Use kebab-case: `enemy-ai-overview.md`, `pathfinding-comparison-a-star-vs-navmesh.md`
- **Front matter**: Include metadata at the top of each file

## Front Matter Template

```markdown
---
title: Topic Title
date: YYYY-MM-DD
owner: @username
status: draft | in-progress | complete
tags: [tag1, tag2, tag3]
---
```

## Structure

Each topic note should include:

1. **Problem/Context**: What question are we answering?
2. **Key Findings**: Main discoveries (bullets)
3. **Evidence**: Links, quotes, data
4. **Implications**: How does this affect design?
5. **Next Steps**: Open questions or follow-up research

## Current Topics

### AI and Automation

- **[AI Issue Template Research](ai-issue-template-research.md)** - AI-enhanced issue templates with automated research suggestions, categorization, and context analysis

### Technical Systems

- **[Multi-Resolution Blending for Geological Processes](multi-resolution-blending-geological-processes.md)** - Scale-dependent geological process simulation and data blending techniques
- **[WoW Emulator Architecture and Networking](wow-emulator-architecture-networking.md)** - MMO networking architecture insights

### Game Design Systems

- **[Auction House Systems](auction-house-systems-local-global-transport.md)** - Local vs global auction systems with transport mechanics
- **[Competitive Quest Control](competitive-quest-control-mechanisms.md)** - Quest competition and control mechanisms
- **[Completionist Quest Engagement](completionist-quest-engagement-patterns.md)** - Quest patterns for completionist players
- **[Explorer Quest Preferences](explorer-quest-preferences-discovery-vs-combat.md)** - Quest design for explorer player types
- **[Quest Board Systems](quest-board-transparency-vs-private-contracts.md)** - Quest visibility and contract systems
- **[Quest Design Approaches](quest-design-narrative-vs-transactional.md)** - Narrative vs transactional quest design
- **[Trust in Player-Created Quests](trust-player-created-quests-reputation-systems.md)** - Reputation systems for player content

### Player Demographics

- **[Game Mechanics that Attract Women Players](game-mechanics-that-attract-women-players.md)** - Design considerations for diverse audiences
- **[Why Don't Women Play Games](why-dont-women-play-games.md)** - Barriers to entry analysis

### Roles and Types

- **[Game Design Roles and Types](game-design-roles-and-types.md)** - Professional roles in game design

## Examples

Good topic files:

- `ai-issue-template-research.md` - Specific AI application
- `multi-resolution-blending-geological-processes.md` - Focused technical system
- `quest-design-narrative-vs-transactional.md` - Single system analysis

Too broad (should be split):

- `game-systems.md` - Multiple unrelated topics
- `research-notes.md` - Vague, catch-all file

## Cross-Linking

Link related topics and reference them in the [research index](../index.md).
