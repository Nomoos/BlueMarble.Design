---
title: Player Position at the Lowest Level - Grid or Graph?
date: 2026-11-08
owner: @Nomoos
status: draft
tags: [world-graph, position, coordinates, grid, navmesh, dggs, h3, s2, encounters]
---

# Player Position at the Lowest Level: Grid or Graph?

**Evidence levels:** **[O]** official or primary source; **[S]** secondary source; **[E]** estimate or
own proposal. Related: [World Graph Representations - Overview](world-graph-representations-overview.md),
[Hierarchical World Graph](hierarchical-world-graph-worlds-continents-zones.md),
[ADR-001 Coordinate Data Type](adr-001-coordinate-data-type-selection.md).

## Problem / Context

Above the lowest level the world is a graph of cells and gateways. What is an actor's position at the
bottom: a tile on a grid, a continuous coordinate, or a node in the graph?

Constraints from the current plan:

- The MVP simulates everything in the browser. The planned server is PHP + MariaDB on shared hosting
  with **no WebSockets**, a world tick every 5 game minutes and actions that end exactly at `finishAt`.
- Players play asynchronously on mobile. The main view is a geological cross-section of a location.
- The world data is geological: height rasters and material octrees at 0.25 m precision.

## Key Findings

- **The source of truth should be the graph: a place, or an edge with a time interval** [E]. A
  traveller is stored as `(edge, departAt, arriveAt)`. The current point is interpolated only for
  drawing, so the server needs no per-tick position updates. The MVP already works this way: its
  `Leg` has `edgeId`, `from`, `to`, `startAt` and `finishAt`.
- **Grids belong to fields, not to actors** [E]. Terrain, materials, deposits, weather, claims and
  interest management are naturally rasters or cell indexes. An actor's position is *mapped onto* a
  grid when needed (`cellOf(position)`). It is never stored as a grid cell.
- **A tile grid as the truth is a design choice for real-time tile movement**, as in RuneScape: tiles
  addressed by absolute X, Y and a plane, grouped into 8×8 chunks and 64×64 regions, with the client
  loading a 104×104-tile area around the player [S]
  ([RSWiki](https://rswiki.moparisthebest.com/Map-Region-System.html)). It needs a fast server tick
  and a live connection, which the planned stack does not have.
- **Continuous coordinates are a choice for free real-time movement**, as in WoW: a map id plus x, y, z.
  Emulator servers validate movement against a Recast/Detour navmesh built per map tile [S]
  ([TrinityCore source](https://trinitycore.net/df/da9/mapbuilder_8cpp_source)). If BlueMarble ever
  gets free movement, use zone-local frames (§5), not one global grid.
- **Global cell systems are indexes, not positions.** S2 leaf cells (level 30) are about 1 cm across,
  and every cm² of Earth has a 64-bit id [O] ([S2](https://s2geometry.io/devguide/s2cell_hierarchy)).
  H3 resolution 15 hexagons average 0.895 m², and every resolution has exactly 12 pentagons [O]
  ([H3](https://h3geo.org/docs/core-library/restable/)). They suit claims, territory and lookups. As
  movement truth they are awkward: cell shapes vary and pentagons exist.

## 1. Options

| Option | Position is | Used by | Fits an async PHP server | Fits geology data | Verdict [E] |
|---|---|---|---|---|---|
| **A. Place** | `placeId` (+ optional slot) | MUDs (Evennia rooms have no coordinates [O]), the MVP | yes: one row, changes only on events | via the place footprint | **truth while resting** |
| **B. Edge + interval** | `edge, from, departAt, arriveAt` | Travian and OGame fleets, MVP legs | yes: two timestamps, no ticks | via the edge polyline | **truth while travelling** |
| **C. Square tile grid** | `x, y, plane` | RuneScape [S], roguelikes | no: needs tick-by-tick moves and a live connection | good: tile = raster cell | only with real-time tile gameplay |
| **D. Hex grid / DGGS cell** | H3 or S2 cell id | GIS, territory maps | yes, as an index | good, but cell shapes vary | **claims and indexes, not actors** |
| **E. Continuous, zone-local** | `zone, x, y, z` (int cm) | WoW (map + xyz [S]), Star Citizen (nested frames [S]) | no: free movement needs streaming | good via local raster | the future path if free movement arrives |
| **F. Continuous, global** | `x, y, z` in one world frame | ADR-001 (int32 cm) | as E | as E | avoid as truth for actors (§5) |

## 2. Recommended Type [E]

```ts
/** Where an actor is. The only source of truth; grids are derived indexes. */
export type Position =
  | { kind: 'place'; place: PlaceId; slot?: SlotId }                  // resting (MVP Actor.place)
  | { kind: 'edge'; edge: EdgeId; from: PlaceId; to: PlaceId;          // travelling (MVP Leg)
      departAt: number; arriveAt: number }
  | { kind: 'aboard'; carrier: CarrierId; place?: PlaceId }            // on a ship: a moving cell
  | { kind: 'transfer'; toWorld: WorldId; arriveAt: number };          // between worlds
// later, only together with real-time transport:
//  | { kind: 'local'; zone: ZoneId; x: number; y: number; z: number } // int cm in the zone frame

/** Derived, never stored. */
declare function pointAt(pos: Position, t: number): GeoPoint;          // for drawing
declare function cellOf(pos: Position, grid: GridSpec): CellId;        // terrain, AOI, claims
```

- **Drawing a traveller:** `f = (t − departAt) / (arriveAt − departAt)` and the point is at fraction f of
  the edge polyline. Times are frozen at departure, so speed is constant along the leg by construction.
  The polyline is only for display; the rules use the edge's time.
- **No stopping in the middle of an edge.** The MVP rule "nobody waits in the middle of a road"
  (`stopWalk` ends at the end of the current leg) keeps edges atomic. Stopping mid-edge would need a
  temporary node and a split edge. Make that an explicit feature (camp on the road) if it is ever
  wanted.
- **Aboard a carrier:** the passenger's position does not change while the ship moves. Only the ship
  has an edge position. The ship is a moving cell (see the hierarchical note, §6).

## 3. Inside a Place: Slots, Not Coordinates [E]

When it matters where someone is inside a place, the question is almost always about *capacity*:
a smelter has two furnaces, a dock has three berths, a mine face has room for four miners. Model
these as **slots**, countable resources of the place, rather than as coordinates:

- a slot is taken by an action and freed at its `finishAt`; queues and waiting follow from that;
- the cross-section UI keeps its horizontal `x ∈ [0, 1]` per place for layout only (MVP `Place.x`);
- learning by observation ("someone better is working here") stays a per-place query.

## 4. Encounters on Edges Without Ticks [E]

Two actors on the same edge have linear positions in time, so meetings can be solved exactly.
Measure position as a fraction of the edge, 0 at `a` and 1 at `b`:

- **Opposite directions.** A goes a → b during [a0, a1] and B goes b → a during [b0, b1]:
  `pA(t) = (t − a0) / (a1 − a0)` and `pB(t) = 1 − (t − b0) / (b1 − b0)`.
  Solve `pA(t*) = pB(t*)`. They meet if `t*` lies in `[max(a0, b0), min(a1, b1)]`.
- **Same direction.** One overtakes the other if their order at the start of the shared interval
  differs from the order at its end.
- Integer milliseconds and a fixed order of evaluation keep the result deterministic. Road events
  (company on the road, bandits, learning by observation while travelling) then cost one calculation
  per pair of actors on the edge, not one per tick.

## 5. If Free Movement Ever Comes [E]

The place graph stays as the macro layer. Places become polygons inside a zone, long travel stays on
graph edges, and continuous movement happens only inside a zone. RimWorld (regions over tiles [O])
and Factorio (components over tiles [O]) use the same two layers.

- **Use a frame per zone, not one global frame.** Each zone stores its origin (lat, lon, height) and a
  local east-north-up tangent plane. Positions are int32 centimetres relative to that origin.
  - A zone spans at most hundreds of kilometres, far within the int32 range of ±21,474 km. ADR-001's
    integer centimetres therefore work without range worries.
  - Local frames also avoid the pole distortion of the equirectangular EPSG:4087 projection, which
    [step-5](../game-design/step-5-coordinate-systems-engine-choice.md) lists as a drawback.
- **Global int32 cm is tight.** EPSG:4087 is 40,075,020 m wide. In centimetres that fits int32
  (±21,474,836 m) only when the origin sits at the centre (±20,037,510 m). Local frames make the
  question disappear.
- **Collision and validation:** a navmesh per zone tile (Recast/Detour, as on WoW emulators [S]), or a
  walkability grid derived from the terrain raster. Either is derived from geology and rebuilt when
  the terrain changes.
- **Streaming and interest:** a grid of cells per zone. UE5 World Partition, for example, splits the
  world into grid cells loaded by distance from streaming sources such as the player [O]
  ([Epic](https://dev.epicgames.com/documentation/en-us/unreal-engine/world-partition-in-unreal-engine)).
  Rendering uses floats relative to the camera (floating origin), as described in step-5.
- **Precondition:** a real-time transport (WebSocket or similar) and a server with a tick. This is a
  different architecture from the planned shared hosting.

## 6. Where Grids Do Belong [E]

| Data | Grid or index | Why |
|---|---|---|
| terrain height, materials | height raster plus octree at 0.25 m ([spatial-data-storage](../spatial-data-storage/)) | geology is a field |
| resource deposits, ground water | raster cells referenced by place footprints | "what is under the mine" = footprint ∩ raster |
| place and edge footprints | polygon or cell set plus depth range | links the graph to the fields; landslides close edges whose footprint becomes impassable |
| claims, territory, settlement control | H3 or S2 cells (hierarchical 64-bit ids) | global, multi-resolution, cheap set operations |
| interest management, streaming | spatial hash or grid per zone | who sees whom, what to load |
| weather, seasons | coarse raster | slow fields that change edge states (snowed-in pass) |

## Implications

1. Keep the MVP's `Actor.place` and `Leg` as the position model. Name it explicitly as the `Position`
   union above when the server port starts.
2. Add `footprint` to places and `polyline` to edges in content. These are display data and the link
   to geology.
3. Add slots to places where capacity matters (smelter, berths) before adding any coordinates.
4. Add edge encounters (§4) when road events are designed.
5. Treat free movement as a separate architecture decision (an ADR) with zone-local frames, not as an
   extension of the async server.

## Gaps

- RuneScape's tick length (commonly cited as 0.6 s) was not verified; the region numbers come from a
  community wiki.
- The TrinityCore navmesh details come from source-browser pages and may be dated.
- The H3 edge length at resolution 15 (about 0.5 m) is from the older 3.x table; the area figure is
  from the current table.
- Interpolation and encounter maths were not prototyped.

## Sources

- RSWiki: [Map region system](https://rswiki.moparisthebest.com/Map-Region-System.html)
- S2 Geometry: [S2Cell hierarchy](https://s2geometry.io/devguide/s2cell_hierarchy), [S2 cell statistics](https://s2geometry.io/resources/s2cell_statistics)
- H3: [Tables of cell statistics across resolutions](https://h3geo.org/docs/core-library/restable/)
- TrinityCore: [mmaps generator, MapBuilder](https://trinitycore.net/df/da9/mapbuilder_8cpp_source), [TileBuilder](https://trinitycore.net/de/d30/tilebuilder_8cpp_source)
- Epic Games: [World Partition](https://dev.epicgames.com/documentation/en-us/unreal-engine/world-partition-in-unreal-engine), [Large World Coordinates](https://dev.epicgames.com/documentation/en-us/unreal-engine/large-world-coordinates-in-unreal-engine-5)
- Evennia: [Objects](https://www.evennia.com/docs/1.x/Components/Objects.html)
- Ludeon: [Reachability at last](https://ludeon.com/blog/2013/07/reachability-at-last); Factorio: [FFF #317](https://factorio.com/blog/post/fff-317)
- GamersNexus: [Star Citizen zone system](https://gamersnexus.net/gg/2119-chris-roberts-star-citizen-network-and-render-pipelines)
