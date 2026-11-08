---
title: World Graph Representations - Overview for BlueMarble
date: 2026-11-08
owner: @Nomoos
status: draft
tags: [world-graph, pathfinding, grids, navmesh, procedural-generation, determinism, storage]
---

# World Graph Representations - Overview for BlueMarble

Adapted from the WebGames research note `Weby/WebGames/docs/12-grafove-reprezentace-sveta.md`
(Czech, 2026-11-07) and narrowed to BlueMarble. Two follow-up notes answer the BlueMarble-specific
questions:

- [Hierarchical World Graph: Worlds, Continents, Zones](hierarchical-world-graph-worlds-continents-zones.md)
- [Player Position at the Lowest Level: Grid or Graph?](player-position-lowest-level-grid-vs-graph.md)

**Evidence levels:** **[O]** official or primary source; **[S]** secondary source; **[E]** estimate or
own proposal.

## Problem / Context

BlueMarble.MVP (`BlueMarble.MVP/src/engine/graph.ts`) already runs on a graph: places are nodes,
walk/climb/swim edges carry base minutes and optional proficiency requirements, and carriers (boats,
carts) are scheduled services rather than edges. The question is which representations exist, which
algorithms they need, and which of them BlueMarble should use as the world grows.

## Key Findings

- **A game world is a graph.** Nodes are places where something can stand (a tile, a district, a
  room, a harbour); edges are allowed moves or adjacency and carry a weight (time, money, risk). The
  representation decides what is cheap to compute: distance, reach, visibility, fair placement [O]
  ([Red Blob Games](https://www.redblobgames.com/pathfinding/grids/graphs.html)).
- **Grids are a special case of graphs.** Dijkstra and A\* work on any weighted directed graph [O]
  (same source).
- **Edges are stored in one of three ways:** per-node adjacency lists (most common), a global edge
  list, or an adjacency matrix [O] (same source). A small graph that is queried often is worth
  precomputing into a distance matrix; a large sparse graph belongs in adjacency lists [E].
- **The fastest speed-up is a smaller graph.** Tune the algorithm only after that [O]
  ([A\* introduction](https://www.redblobgames.com/pathfinding/a-star/introduction.html)).
- **Asynchronous browser strategies often skip pathfinding entirely.** Travian derives arrival time
  from distance and unit speed [O]; OGame clones compute distance from the galaxy:system:position
  address [S]. A graph pays off where the route itself is a decision: bridge or ferry, blockade,
  terrain, a strait that needs Swimming 25 [E].

## 1. Representations

| Representation | Nodes and edges | Typical use | Strengths | Weaknesses | BlueMarble |
|---|---|---|---|---|---|
| **square grid** | tile; 4 or 8 neighbours | turn-based and browser strategy, roguelikes, OSRS | trivial storage, address = coordinate | many nodes on open ground, ugly ties | terrain and material rasters, not actor position |
| **hex grid** | hex; 6 equidistant neighbours | wargames, 4X | uniform neighbour distance | harder coordinates | territory and claims layer (H3 on a sphere) |
| **place graph** (waypoints) | authored places and routes | board games (Diplomacy), MUDs, world maps | few nodes, edges carry story (bridge, ferry) | movement only along given routes | **MVP today**: places inside locations |
| **polygon map** (Voronoi/Delaunay) | cell; adjacent when sharing an edge | procedural islands, provinces | natural-looking regions | generator takes days | zone partition of generated terrain |
| **navmesh** | convex polygon | 3D and real-time action | few polygons cover large areas | generation, complexity | only if free movement ever arrives |
| **visibility graph** | obstacle corners | RTS, robotics | exact paths around obstacles | rebuild when obstacles change | no |
| **hierarchical graph** | clusters plus transitions | large maps (HPA\*), road routing (CRP) | speed, natural partition | preprocessing | **yes**, for worlds, continents, zones |
| **distance field / flow field** | value per node | tower defence, RTS crowds, roguelike AI | many units to one goal for one pass | one field per goal | NPC targets, market catchment |
| **knowledge graph** | entities and relations | LLM agents, narrative games | facts for generated text | LLMs are not deterministic | later (rumours, news) |

Notes:

- **Hexes.** Cube coordinates (q + r + s = 0) are simplest for algorithms; axial or doubled
  coordinates for storage. Neighbour offsets are uniform in axial and cube coordinates, not in
  offset coordinates [O] ([Red Blob Games, Hexagonal Grids](https://www.redblobgames.com/grids/hexagons/)).
- **Diplomacy as a place graph.** 56 land and 19 sea regions, 34 supply centres [O]
  ([Wikipedia](https://en.wikipedia.org/wiki/Diplomacy_(game))). Rule engines keep adjacency lists and
  record multi-coast provinces separately [S]
  ([diplomacy, Python](https://diplomacy.readthedocs.io/en/latest/_modules/diplomacy/engine/map.html)).
  One node may need several entrances for different edge kinds; the MVP does the same (a gate for
  roads, a dock for boats) [E].
- **Polygon maps.** Amit Patel (2010) uses two linked graphs: Delaunay centres for adjacency and
  paths, Voronoi corners for shapes, borders and rivers; Lloyd relaxation makes cells regular [S]
  ([simblob, part 1](https://simblob.blogspot.com/2010/09/polygon-map-generation-part-1.html)).
- **Navmesh.** Recast (generation via voxelisation) and Detour (queries, crowds), C++, zlib licence
  [S] ([recastnav.com](https://recastnav.com/dir_97c6bac6df0065e0578eb2a70aa0bb50.html)).
- **A scene graph is not the world graph.** Engine node trees drive rendering; in Godot nodes are an
  interface to data processed by separate servers [O]
  ([Godot](https://godotengine.org/article/why-isnt-godot-ecs-based-game-engine/)). The world graph
  lives on the server; the client renders a projection [E].
- **Knowledge graphs.** AriGraph (IJCAI 2025) builds semantic and episodic memory while exploring
  text games [O] ([IJCAI](https://www.ijcai.org/proceedings/2025/0002)). If rumours or news are ever
  generated by an LLM, feed it facts from the graph and validate the output; the LLM never changes
  world state [E].

## 2. Algorithms

| Task | Algorithm [O] |
|---|---|
| paths from or to all places, equal costs | Breadth First Search |
| the same with different costs | Dijkstra |
| a path to one goal | A\*; the heuristic must not overestimate |
| fast but not necessarily shortest | Greedy Best First, or A\* with an inadmissible heuristic |

Source: [Red Blob Games](https://www.redblobgames.com/pathfinding/a-star/introduction.html). To reach
the nearest of several goals, add a virtual goal node linked to all of them [O]
([tower defense](https://www.redblobgames.com/pathfinding/tower-defense/)).

- **Dijkstra maps (Brogue).** Goals get 0, every other node is repeatedly lowered to "lowest
  neighbour + 1". Fields can be weighted and summed (food, player, flight) to get behaviour that looks
  intelligent [O] ([RogueBasin](https://roguebasin.com/index.php?title=The_Incredible_Power_of_Dijkstra_Maps)).
- **Jump Point Search** (Harabor and Grastien, AAAI 2011): optimal, no preprocessing, an order of
  magnitude faster than A\* on uniform-cost grids [O]
  ([AAAI](https://ojs.aaai.org/index.php/AAAI/article/view/7994)). See also
  [algorithm-analysis-jump-point-search](../literature/algorithm-analysis-jump-point-search.md).
- **HPA\*** (Botea, Müller, Schaeffer 2004): clusters with precomputed transitions; up to 10× faster
  than optimised A\*, paths within 1 % of optimal [O]
  ([University of Alberta](https://webdocs.cs.ualberta.ca/~mmueller/ps/hpastar.pdf)).
- **Scale check.** ngraph.path finds one path on the New York road graph (264,346 nodes, 733,844
  edges) in 55 ms with A\*, 44 ms with NBA\*, 264 ms with Dijkstra [O]
  ([GitHub](https://github.com/anvaka/ngraph.path)). The MVP sample has tens of places; even a
  planet of authored places stays far smaller than a road network [E].

## 3. Determinism

- Pathfinding produces many ties; the result then depends on neighbour order [O]
  ([Implementation of A\*](https://www.redblobgames.com/pathfinding/a-star/implementation.html)).
- PHP `SplPriorityQueue` returns equal-priority elements "in no particular order" [O]
  ([php.net](https://www.php.net/manual/en/splpriorityqueue.compare.php)), so a PHP server will not
  match the TS client without extra rules.
- **Rules** [E]: integer weights ≥ 1; neighbours in a fixed order (by id); the queue is ordered by
  `[cost, id]`; on equal cost the predecessor with the smaller id wins; shared test vectors (graph,
  start, goal → expected route) run in both `vitest` and PHPUnit.
- **Status:** `BlueMarble.MVP/src/engine/graph.ts` already follows these rules (sorted adjacency,
  `[cost, id]` heap, smaller-id predecessor). A PHP port must reproduce them:

```php
// Dijkstra with deterministic tie-breaking [E]
final class MinQueue extends SplPriorityQueue
{
    // priority = [cost, id]; the smaller pair leaves first
    public function compare(mixed $a, mixed $b): int { return $b <=> $a; }
}

/** @param array<int, array<int,int>> $adj node => [neighbour => weight >= 1], sorted by id */
function dijkstra(array $adj, int $start): array
{
    $dist = [$start => 0];
    $prev = [];
    $q = new MinQueue();
    $q->setExtractFlags(SplPriorityQueue::EXTR_BOTH);
    $q->insert($start, [0, $start]);
    while (!$q->isEmpty()) {
        ['data' => $u, 'priority' => [$d]] = $q->extract();
        if ($d > $dist[$u]) continue;                       // stale entry
        foreach ($adj[$u] ?? [] as $v => $w) {
            $nd = $d + $w;
            if (!isset($dist[$v]) || $nd < $dist[$v]) {
                $dist[$v] = $nd;
                $prev[$v] = $u;
                $q->insert($v, [$nd, $v]);
            } elseif ($nd === $dist[$v] && $u < $prev[$v]) {
                $prev[$v] = $u;                             // tie: smaller predecessor id
            }
        }
    }
    return [$dist, $prev];
}
```

## 4. Procedural Generation Through Graphs

- **Mission and space** (Dormans 2010): first a directed mission graph (what unlocks what) from a
  graph grammar, then its projection into space [O]
  ([PCG Workshop 2010](https://www.pcgworkshop.com/archive/dormans2010adventures.pdf)).
- **Cyclic generation** (Unexplored): a large cycle with entrance and goal split into two arcs; the
  cycle type decides their use (alternative paths, lock on one arc and key on the other); about 50
  grammar modules on a 5×5 node grid, tiles only at the end [O]
  ([Boris the Brave](https://www.boristhebrave.com/2021/04/10/dungeon-generation-in-unexplored/)).
- **Locks and keys** can be anything that blocks progress; the dependency graph shows how much
  choice the player really has [O]
  ([Lock and Key Dungeons](https://www.boristhebrave.com/2021/02/27/lock-and-key-dungeons/)). In
  BlueMarble the locks are proficiency requirements (Swimming 25 for the strait) [E].
- **TinyKeep:** Delaunay triangulation of room centres → minimum spanning tree → about 15 % of the
  remaining edges added back for loops [S]
  ([GameDev.net](https://gamedev.net/forums/topic/642621-my-procedural-dungeon-generation-algorithm-explained-for-my-game-tinykeep/)).
  The same recipe fits a road network between generated settlements [E].
- **WFC on graphs:** DeBroglie treats square, hex and irregular grids as graphs [S]
  ([boristhebrave.com](https://boristhebrave.com/tag/wfc/page/2)).

## 5. Graphs Beyond the Map

| Graph | Nodes → edges | Use in BlueMarble |
|---|---|---|
| proficiency relations | activities and materials → `related` | spill-over of gains (MVP: one hop, 0.15) |
| recipes (DAG) | materials and products → "produces" | cycle check at content load; longest path = steps to the most valuable good |
| economy (Machinations) | pools, gates, converters → resource flows | find feedback loops before balancing [S] ([AIIDE](https://ojs.aaai.org/index.php/AIIDE/article/view/12477)) |
| trade and transport | ports → carrier lines | where goods flow, which lines pay off |
| social graph | players → meetings, trades, hires | journal, reputation, multi-account detection |
| ledger flows | accounts → transfers over 7 days | a star of new accounts feeding one receiver is suspicious [E] |

## 6. Storage on the Planned Server (PHP + MariaDB)

The MVP plan moves the server to PHP 8.3 and MariaDB on VEDOS later. The WebGames rules apply [E]:

| What | Where | Why |
|---|---|---|
| places, edges, base weights, requirements, cell tree | content file in the repo, built into a PHP array (`return [...]`) kept in OPcache | versioned and reviewable; the TS client reads the same file |
| closures, owners, actors on the road, bookings | MariaDB | changes go through the command pipeline |
| derived data (gateway matrices, distance fields) | computed at deploy or by cron | cheap to rebuild, never a source of truth |

- VEDOS web hosting uses OPcache; Memcache and Xcache are not available [O]
  ([kb.vedos.cz](https://kb.vedos.cz/webhosting-php/)).
- MariaDB supports recursive CTEs with `max_recursive_iterations` and `CYCLE … RESTRICT` [O]
  ([MariaDB](https://mariadb.com/kb/en/with/)). Use them only for bounded reach (fog of war,
  "within 3 hops"); compute weighted paths in PHP [E].
- OQGRAPH runs Dijkstra in SQL but must be installed with `INSTALL SONAME 'ha_oqgraph'` [O]
  ([MariaDB](https://mariadb.com/docs/server/server-usage/storage-engines/oqgraph-storage-engine/installing-oqgraph)),
  which a shared-hosting customer cannot do [E].

## 7. Server Authority Over the Graph [E]

- **A command carries the goal, not the result:** `move {actor, to}` or `move {actor, via: [...]}`.
  The server checks each step is an existing, open edge the actor may take, computes the time and
  stores `finishAt`. Client times are never accepted.
- **The route is frozen at departure** (MVP `beginWalk`). A bridge closed later affects only new
  commands; queued events never need recomputation.
- **Fog of war as a subgraph:** the view endpoint returns only nodes within k hops of what the player
  knows, so the client has nothing to reveal.

## Implications

- Keep the place graph as the movement model; add hierarchy (zones, continents, worlds) on top of it
  instead of replacing it. Details: [hierarchical note](hierarchical-world-graph-worlds-continents-zones.md).
- Keep grids for fields (terrain, materials, claims), not for actor positions. Details:
  [position note](player-position-lowest-level-grid-vs-graph.md).

## Gaps

- Travian's exact distance formula is not in its official help; the OGame formula comes from the
  open-source clone SuperNova only.
- Dijkstra performance in PHP on VEDOS has not been measured.

## Sources

- Red Blob Games: [Grids and Graphs](https://www.redblobgames.com/pathfinding/grids/graphs.html),
  [Introduction to A\*](https://www.redblobgames.com/pathfinding/a-star/introduction.html),
  [Implementation of A\*](https://www.redblobgames.com/pathfinding/a-star/implementation.html),
  [Flow Field Pathfinding](https://www.redblobgames.com/pathfinding/tower-defense/),
  [Hexagonal Grids](https://www.redblobgames.com/grids/hexagons/)
- Botea, Müller, Schaeffer: [Near Optimal Hierarchical Path-Finding](https://webdocs.cs.ualberta.ca/~mmueller/ps/hpastar.pdf) (2004)
- Harabor, Grastien: [Online Graph Pruning for Pathfinding on Grid Maps](https://ojs.aaai.org/index.php/AAAI/article/view/7994) (AAAI 2011)
- Brian Walker: [The Incredible Power of Dijkstra Maps](https://roguebasin.com/index.php?title=The_Incredible_Power_of_Dijkstra_Maps)
- [Recast Navigation](https://recastnav.com/dir_97c6bac6df0065e0578eb2a70aa0bb50.html), [ngraph.path](https://github.com/anvaka/ngraph.path)
- Amit Patel: [Polygon map generation](https://simblob.blogspot.com/2010/09/polygon-map-generation-part-1.html) (2010)
- Joris Dormans: [Adventures in Level Design](https://www.pcgworkshop.com/archive/dormans2010adventures.pdf) (2010)
- Boris the Brave: [Dungeon Generation in Unexplored](https://www.boristhebrave.com/2021/04/10/dungeon-generation-in-unexplored/), [Lock and Key Dungeons](https://www.boristhebrave.com/2021/02/27/lock-and-key-dungeons/)
- [Travian: Coordinates, Distance and Navigation](https://support.travian.com/en/articles/215-guide-the-map-coordinates-distance-and-navigation), [SuperNova `Universe::distance()`](https://scrutinizer-ci.com/g/supernova-ws/SuperNova/code-structure/trunk/operation/Universe::distance)
- [Diplomacy (Wikipedia)](https://en.wikipedia.org/wiki/Diplomacy_(game)), [Machinations (AIIDE)](https://ojs.aaai.org/index.php/AIIDE/article/view/12477), [AriGraph (IJCAI 2025)](https://www.ijcai.org/proceedings/2025/0002)
- MariaDB [WITH](https://mariadb.com/kb/en/with/), [Installing OQGRAPH](https://mariadb.com/docs/server/server-usage/storage-engines/oqgraph-storage-engine/installing-oqgraph); PHP [SplPriorityQueue::compare](https://www.php.net/manual/en/splpriorityqueue.compare.php); [VEDOS PHP](https://kb.vedos.cz/webhosting-php/)
