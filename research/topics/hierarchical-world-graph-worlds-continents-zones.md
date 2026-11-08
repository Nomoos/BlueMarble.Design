---
title: Hierarchical World Graph - Worlds, Continents, Zones
date: 2026-11-08
owner: @Nomoos
status: draft
tags: [world-graph, hierarchy, pathfinding, gateways, sharding, procedural-generation, geology]
---

# Hierarchical World Graph: Worlds, Continents, Zones

**Evidence levels:** **[O]** official or primary source; **[S]** secondary source; **[E]** estimate or
own proposal. Background: [World Graph Representations - Overview](world-graph-representations-overview.md).

## Problem / Context

BlueMarble's world has nested levels: worlds, continents (or islands), zones, locations and places.
Between any two units at the same level there are **only a few routes**: a mountain pass, a strait, a
port, a mine shaft, a portal. How should these graphs be represented, stored and searched so that the
same rules work at every level?

Starting point: `BlueMarble.MVP` has two levels in one graph. Places are nodes, each place belongs
to a location, and edges inside and between locations are ordinary edges. Boats and carts are
services that need a booking, not edges (`docs/plan.md` and `src/engine/graph.ts` in the separate
`BlueMarble.MVP` repository).

## Key Findings

- **Keep one flat graph and put the hierarchy beside it.** The levels are a **containment tree**
  (place → location → zone → continent → world). Movement is **one graph** over places only. A
  "continent-to-continent route" is never authored as its own object: it is an ordinary edge whose two
  ends lie in different continents [E].
- **An edge's level is derived from its endpoints.** EVE Online names stargates by exactly this rule:
  a system gate stays inside one constellation, a constellation gate links constellations of one
  region, a region gate links regions [O]
  ([EVE University](https://wiki.eveuniversity.org/Stargates)).
- **The few routes are an asset, not a limitation.** Few cut edges make the abstract graph tiny. This
  is the basis of HPA\* in games [O] and of Customizable Route Planning (CRP) for continental road
  networks: overlay graphs built from boundary vertices plus shortcuts inside each cell [O]
  ([Microsoft Research](https://www.microsoft.com/en-us/research/publication/customizable-route-planning/)).
- **The abstract node is a connected component, not a cell.** Factorio splits each 32×32 chunk into
  components, where any tile can reach any other tile in the same component, and makes each component
  an abstract node [O] ([FFF #317](https://factorio.com/blog/post/fff-317)). A zone that holds an
  island reachable only by boat is two nodes, not one.
- **Check reachability before searching.** RimWorld labels every walkable square with a region index
  by flood fill and skips A\* when start and goal labels differ, which removed the hitches from
  failed searches [O] ([Ludeon](https://ludeon.com/blog/2013/07/reachability-at-last)).
- **The same model gives sharding for free.** If a zone owns its simulation, an actor changes owner
  only at a gateway, at a known moment. That is simpler than a handover on a seamless boundary [E].

## 1. How Other Systems Partition Their Worlds

| System | Levels | Links between units | Lesson for BlueMarble |
|---|---|---|---|
| **EVE Online** | region → constellation → solar system | stargates. The gate type follows the membership of its endpoints. Wormholes are transient links; J-space systems have no gates [O] | derive the edge level; temporary edges are first-class |
| **EVE servers** | solar system → server node | each system runs on a node; nodes are allocated from a cluster by load [S] ([repo note](../literature/game-dev-analysis-eve-online-large-scale-combat.md)) | the cell is the unit of load |
| **World of Warcraft** | map (continent or instance) → 64×64 ADT tiles of 533.33 yd → 16×16 chunks [S] ([warcraft-rs](https://warcraft-rs.readthedocs.io/en/latest/formats/world-data/adt.html)) | continents and instances are separate maps; boats, zeppelins and portals link them. Zones label areas of one continuous map [E] | a grid is a storage and streaming partition, not the travel graph |
| **Albion Online** | Royal Continent, Outlands, Roads of Avalon | each Royal City portal leads to its own cluster of black zones; Roads of Avalon have entrances and exits that change regularly [S] ([guides](https://www.albionmarket.gg/guides/zones-and-flagging)) | a gateway can belong to one owner (a city), and routes can expire |
| **MUDs / Evennia** | rooms | exits are one-way objects with a destination; no coordinate system by default [O] ([Evennia](https://www.evennia.com/docs/1.x/Components/Objects.html)) | a place graph with no coordinates is a proven model |
| **RimWorld** | map → regions → rooms | region links; a reachability index from flood fill [O] | label components, test before searching |
| **Factorio** | 32×32 chunks → components | an abstract graph of components feeds a heuristic to the tile-level A\* [O] | the abstraction guides the search; the base graph stays exact |
| **Dragon Age: Origins** | sectors and regions | an extra abstraction level for memory-limited consoles [S] ([Sturtevant](https://webdocs.cs.ualberta.ca/~nathanst/aaai_tut2.pdf)) | abstraction is cheap. Sturtevant's memory-efficient abstraction adds about 3 % storage and finds complete paths up to 100× faster than A\* [O] ([AIIDE](https://ojs.aaai.org/index.php/AIIDE/article/view/18778)) |
| **Road routing (CRP)** | a multi-level partition of a continent | boundary vertices plus per-cell shortcuts. Preprocessing ignores costs; a new cost metric takes "a few seconds" [O] | separate topology from costs; recompute one cell at a time |
| **Star Citizen** | nested zones ("Russian doll") | each zone container has its own frame of reference, so whole ships and planets move without re-sorting a global structure [S] ([GamersNexus](https://gamersnexus.net/gg/2119-chris-roberts-star-citizen-network-and-render-pipelines)) | a ship can be a moving cell with places inside |

## 2. Model: Containment Tree + One Connection Graph [E]

```text
world      w.terra
└─ continent  c.northreach          (landmass or archipelago)
   └─ zone       z.harbor-bay       (unit of simulation, ~drainage basin)
      └─ location   l.harbor        (MVP Location)
         └─ place      harbor.dock  (MVP Place = graph node)
```

- **Levels are data, not schema.** One `cell` type with `level` and `parent`. Adding a level, such as
  a "region" between continent and zone, is a content change, and every algorithm below runs the
  same way at every level.
- **Only places are graph nodes.** Cells have no edges of their own. A cell's links follow from the
  edges of the places it contains.
- **Crossing level of an edge** = the level just below the lowest common ancestor of its two places.
  An edge whose ends share a zone but not a location is a location gate. Ends in different continents
  of one world make it a continent gate.
- **Gateway** = a place that is the endpoint of an edge or service stop that leaves its cell at some
  level. A port is a gateway of its zone, its continent and maybe its world. The gateway lists are
  derived and never edited by hand.
- **IDs** are opaque and stable (`harbor.dock`). The tree is stored with `parent`, plus a derived
  materialised path (`/w.terra/c.northreach/z.harbor-bay/`) for cheap "everything in this continent"
  queries. Rebuild the path on load and never edit it.

```json
{
  "cells": [
    { "id": "w.terra",        "level": "world",     "parent": null },
    { "id": "c.northreach",   "level": "continent", "parent": "w.terra" },
    { "id": "z.harbor-bay",   "level": "zone",      "parent": "c.northreach" },
    { "id": "l.harbor",       "level": "location",  "parent": "z.harbor-bay" }
  ],
  "places": [
    { "id": "harbor.gate", "cell": "l.harbor", "layer": "surface" },
    { "id": "harbor.dock", "cell": "l.harbor", "layer": "shore" }
  ],
  "edges": [
    { "id": "e.iron-col", "a": "harbor.gate", "b": "mine.gate", "kind": "climb",
      "minutes": 90, "requires": { "prof": "climbing", "min": 10 } }
  ],
  "services": [
    { "id": "s.ferry-isle", "carrier": "rowboat", "stops": ["harbor.dock", "isle.pier"] }
  ]
}
```

## 3. Overlay Graphs: How the Few Routes Become a Small Graph

For each level L, the **overlay graph** has these parts [O for the structure, from CRP and HPA\*]:

- **nodes:** the gateways of all level-L cells;
- **real edges:** the crossing edges between cells;
- **shortcuts:** inside each cell, between every pair of its gateways, with cost = the shortest path
  that stays inside the cell. They are stored as a k×k matrix per cell, with ∞ when the gateways lie
  in different components.

Any shortest path splits into in-cell segments between gateways plus crossing edges. Distances in
the overlay are therefore **exact**, not approximations [E, follows from the CRP construction]. A
higher level is built the same way on top of the lower overlay: continent shortcuts are computed on
the zone overlay of that continent.

**Size example** [E]: 1 world, 8 continents, 400 zones and 5 gateways per zone give about 2,000
overlay nodes, about 1,000 crossing edges and 400 × 25 = 10,000 shortcut entries. Dijkstra on that
takes milliseconds, even in PHP.

## 4. Query: From a Place to a Place

```ts
// One overlay level is enough until zones number in the thousands [E]
function route(from: PlaceId, to: PlaceId, cost: StepCost): PathResult {
  const zf = zoneOf(from), zt = zoneOf(to);
  const local = zf === zt ? findPath(zoneGraph(zf), from, to, cost) : NONE;
  const out = costsToGateways(zoneGraph(zf), from, cost);    // from → each gateway of zf
  const inn = costsFromGateways(zoneGraph(zt), to, cost);    // each gateway of zt → to
  const via = overlaySearch(out, inn, cost);                 // multi-source, multi-target Dijkstra
  return cheaper(local, via);   // leaving the zone and coming back can be shorter
}
```

- **Expand lazily.** The overlay route is a list of gateways. Turn a shortcut into real steps only when
  the actor enters that zone. The MVP freezes leg times at departure. Extend that rule so the route
  is frozen per zone, with a re-plan at each gateway, because a pass can close during a long journey.
- **Reachability first.** Give each component a label per capability class (no requirements, Swim
  25, …) as RimWorld does [O]. When the labels differ, answer "no route" or "blocked" without
  searching. The MVP's two passes in `planWalk` ("blocked" vs "no-route") keep working on top of
  this.
- **Property test:** the overlay route must cost the same as a flat Dijkstra on random pairs. This one
  test guards the whole hierarchy [E].

## 5. Costs That Differ per Player

In the MVP an edge costs `base minutes × modifier(proficiency for walk|climb|swim)`, and an edge with
an unmet requirement cannot be crossed. Shortcuts computed once cannot serve every player. Three
options:

| Option | How | Exact? | Cost |
|---|---|---|---|
| **A. No precomputation** | flat Dijkstra over all places per command | yes | fine up to roughly 10⁴–10⁵ places in the searched area [E]; ngraph.path runs Dijkstra on 264k nodes in 264 ms in JS [O] |
| **B. Overlay as a lower bound** | shortcuts with the best possible modifier and every requirement met. The overlay distance becomes an admissible heuristic for A\* on the real graph | yes (A\* with an admissible heuristic) | one metric for everybody. Factorio uses the same split: an abstract pathfinder, searching backwards from the goal, supplies the heuristic for the base pathfinder [O] |
| **C. Pareto shortcuts** | per gateway pair, keep the paths that are Pareto-optimal in (walk min, climb min, swim min, required levels). A player's cost is Σ modifier(kind) × minutes(kind) over the entries they may use | yes | the cost is linear in the minutes vector, so the sets stay small. Without requirements, the lower convex hull is enough [E]. Analogue of CRP customization [O] |

**Recommendation** [E]: keep A until a measurement shows it is too slow, then B. C is needed only if
long journeys stay slow with B.

## 6. Edges That Are Not Roads

- **Services (ships, caravans) are timetable connections.** Journey planning across continents is a
  public-transit routing problem. RAPTOR works in rounds, one per transfer, scans each route at most
  once per round and needs no preprocessing, so it suits networks that change. It returns
  Pareto-optimal journeys by arrival time and number of transfers [O]
  ([Transportation Science 2015](https://pubsonline.informs.org/doi/fpi/10.1287/trsc.2014.0534)).
  BlueMarble carriers leave only when a trip pays off, so there is no fixed timetable. The planner
  uses an expected wait (current bookings, history) and shows a range. The booking escrow decides the
  real departure [E].
- **Several criteria.** Show the player a short Pareto list, for example "fast and expensive" vs "slow
  and cheap" vs "swim the strait yourself", instead of one "best" route [E].
- **Temporary edges.** Examples: EVE wormholes [O]; Albion's Roads of Avalon [S]; in BlueMarble,
  tides, snowed-in passes, frozen straits and landslides from the geology simulation. Store either a
  validity interval `[openFrom, openUntil)` or a state flag owned by the simulation. When it changes,
  recompute only the shortcut matrix of the cell that contains the edge [E].
- **Vertical gateways** (shafts, caves, wells) use the same mechanism. A place's layer (ridge,
  surface, shore, underground) is an attribute, not a hierarchy level. Depth bands from
  [game-world-summary](../game-design/step-1-foundation/game-world-summary.md) become cells only if
  deep mining needs its own simulation unit [E].
- **Between worlds.** A world is an independent simulation: its own clock, economy and database
  partition, for example an island shard ("shard-by-island" in the one-page design) or another
  planet. Crossing is a **transfer**: an atomic handoff of the actor and the cargo, written through
  an outbox in the source world and applied once in the target world. It has no partial progress.
  The planner treats it as a service with a fixed time and fee [E].
- **Moving cells.** A ship can be a cell whose places (deck, hold) are its children, while the ship
  itself is "on an edge" of the sea graph. Passengers are at a place inside the ship. This is the
  Star Citizen idea of nested frames [S], and it gives cargo and passengers one model [E].

## 7. Where Zones and Gateways Come From: Geology [E]

In BlueMarble, the few routes should come from the terrain, not from hand-drawn lines:

- **Zones = drainage basins.** Basin delineation runs on a flow-direction raster, and basins nest
  (sub-basins inside major basins) [S] ([hydrology tools note](spatial-analysis-hydrology-tools.md)).
  Merge sub-basins to a target size, giving a natural hierarchy of zone → region → continent.
- **Continents = connected landmasses** above sea level; ocean basins become sea zones.
- **Land gateways = passes**: the lowest saddle on the divide between two basins. Rivers get fords
  where shallow, and players can build bridges later.
- **Sea gateways = ports** on sheltered coast, plus **straits** where two landmasses come close. A
  strait is a swim edge whose requirement scales with width and current.
- **Roads inside a zone** between settlements: Delaunay → minimum spanning tree → about 15 % of the
  remaining edges added back for loops, as in TinyKeep [S]
  ([GameDev.net](https://gamedev.net/forums/topic/642621-my-procedural-dungeon-generation-algorithm-explained-for-my-game-tinykeep/)).
- **Hand-authored overrides** (a tunnel, a quest bridge) are ordinary edges in content.
- **Quality checks:** 2–6 gateways per zone; a zone with one gateway is a dead-end valley and must
  be tagged as intended; every zone reachable with no requirements, or tagged as gated.

## 8. Sharding and Ownership [E]

- **A zone (or a group of zones) owns** its simulation, its persistence partition and its interest
  management. EVE does the same per solar system [S].
- **Actors change owner only at gateways**, at an arrival event. The geo-sharding research needs
  lock-and-transfer at arbitrary points of a seamless boundary
  ([geo-sharding](../gpt-research/system-architecture-geo-sharding/README.md)). Here the handoff is a
  discrete event that already exists in the event queue.
- **An edge never spans more than two zones.** Put gateway places on the boundary. An edge belongs to
  its origin zone until arrival, and the arrival event is passed to the destination zone.

## 9. Storage

The static topology is content (a JSON file in the repo, built into a PHP array in OPcache; see the
overview). Derived tables, if they go to MariaDB:

```sql
CREATE TABLE wg_cell (
  id        VARCHAR(64)      NOT NULL PRIMARY KEY,
  level     TINYINT UNSIGNED NOT NULL,   -- 0 world, 1 continent, 2 zone, 3 location
  parent_id VARCHAR(64)      NULL,
  path      VARCHAR(255)     NOT NULL,   -- '/w.terra/c.northreach/z.harbor-bay/', derived
  KEY ix_path (path)                     -- WHERE path LIKE '/w.terra/c.northreach/%'
) ENGINE=InnoDB;

CREATE TABLE wg_shortcut (               -- per-cell gateway matrix, derived
  cell_id   VARCHAR(64)  NOT NULL,
  metric    VARCHAR(32)  NOT NULL,       -- 'base', 'lower-bound', …
  from_node VARCHAR(64)  NOT NULL,
  to_node   VARCHAR(64)  NOT NULL,
  minutes   INT UNSIGNED NULL,           -- NULL = unreachable inside the cell
  PRIMARY KEY (cell_id, metric, from_node, to_node)
) ENGINE=InnoDB;
```

## 10. Invariants to Check in CI [E]

1. Every place belongs to exactly one cell, and the cells form a tree with one root per world.
2. Every edge and service stop references existing places. Crossing levels and gateway lists are
   computed, never stored as truth.
3. Warn on zones with 0 gateways (unreachable) or 1 (dead end) unless tagged.
4. Every shortcut equals a Dijkstra run inside its cell.
5. Overlay route = flat route on random pairs (property test, §4).
6. Shared TS/PHP determinism vectors, as in the overview.

## Implications and Next Steps for the MVP [E]

1. Add the `cell` tree to the MVP content: world → archipelago (continent) → zone → location. Leave
   `Place.location` as the link to the leaf cell.
2. Derive gateways and crossing levels. The sea chart then shows the continent overlay, and the
   location cross-section shows the place graph.
3. Keep flat Dijkstra (option A). Write the overlay property test together with the overlay itself.
4. Add services to the travel planner as a Pareto list (time, money, requirement).
5. Experiment in `BlueMarble/Generator`: zones from drainage basins and passes from saddles on a
   generated height raster. Count the gateways per zone.

## Gaps

- WoW details come from community format documentation; the claim that zones are area labels on one
  continuous map was not checked against a primary source.
- EVE's constellation count and the server-node allocation are from wikis and an existing repo note.
- The CRP details come from the abstract and related literature, not from a full reading of the
  paper.
- The Dragon Age sector abstraction comes from tutorial slides and the AIIDE abstract.
- No sizes or timings were measured. The figures in §3 and §5 are estimates.

## Sources

- EVE University: [Stargates](https://wiki.eveuniversity.org/Stargates)
- Delling, Goldberg, Pajor, Werneck: [Customizable Route Planning](https://www.microsoft.com/en-us/research/publication/customizable-route-planning/) (SEA 2011)
- Delling, Pajor, Werneck: [Round-Based Public Transit Routing](https://pubsonline.informs.org/doi/fpi/10.1287/trsc.2014.0534), Transportation Science 49(3), 2015
- Botea, Müller, Schaeffer: [Near Optimal Hierarchical Path-Finding](https://webdocs.cs.ualberta.ca/~mmueller/ps/hpastar.pdf) (2004)
- Factorio: [Friday Facts #317 - New pathfinding algorithm](https://factorio.com/blog/post/fff-317) (2019)
- Ludeon (Tynan Sylvester): [Reachability at last](https://ludeon.com/blog/2013/07/reachability-at-last) (2013)
- Sturtevant: [Memory-Efficient Abstractions for Pathfinding](https://ojs.aaai.org/index.php/AIIDE/article/view/18778) (AIIDE), [tutorial slides](https://webdocs.cs.ualberta.ca/~nathanst/aaai_tut2.pdf)
- warcraft-rs: [ADT format](https://warcraft-rs.readthedocs.io/en/latest/formats/world-data/adt.html), [coordinates](https://warcraft-rs.readthedocs.io/en/latest/resources/coordinates.html)
- Albion Online: [Zones and Flagging](https://www.albionmarket.gg/guides/zones-and-flagging)
- Evennia: [Objects](https://www.evennia.com/docs/1.x/Components/Objects.html)
- GamersNexus: [Chris Roberts on Star Citizen Engine Architecture & Zoning](https://gamersnexus.net/gg/2119-chris-roberts-star-citizen-network-and-render-pipelines)
- TinyKeep: [Procedural dungeon generation explained](https://gamedev.net/forums/topic/642621-my-procedural-dungeon-generation-algorithm-explained-for-my-game-tinykeep/)
- [ngraph.path](https://github.com/anvaka/ngraph.path)
