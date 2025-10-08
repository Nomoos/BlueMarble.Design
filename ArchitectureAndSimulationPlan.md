# Planet-Scale MMORPG GIS Architecture and Simulation Plan

Designing an Earth-sized MMO world (40,075,020 m × 20,037,510 m in EPSG:4087) at 0.25 m voxel precision requires rethinking traditional game engines and data systems. We must use 64-bit coordinates and hierarchical LOD to handle the enormous scale, combined with cloud-friendly storage and scalable networking.

---

## Coordinate System & Engine Choice

- **Double/64-bit Coordinates**  
  Represent all world positions with double-precision floats or 64-bit integers to retain sub-meter accuracy planet-wide.  
  - Blue Marble design: 64-bit integer coords with ±10,000 km vertical range (meter precision).  
  - Unreal Engine 5 LWC: 64-bit doubles, supports ≈88 million km.  
  - Flax Engine: switch to doubles for Solar-System scale.  

- **Floating Origin Shifting**  
  Continuously relocate the local origin (keep the player near (0,0,0)) to avoid float precision loss.  
  - Store voxel data in integer space (1 = 1 m).  
  - Do physics/raycast math in local [0,1] range, convert to world coords last.  

- **Engine Choice**  
  - Unreal Engine 5 (Large World Coordinates).  
  - Flax Engine with double precision.  
  - Godot 4 with experimental doubles.  
  - Custom ECS engines (Unity DOTS, Rust/C++).  
  - For physics engines stuck at 32-bit (e.g. PhysX), shift origin dynamically.

**📖 For detailed coordinate system design, engine comparisons, and implementation guides, see [Step 5: Coordinate Systems & Engine Choice](research/game-design/step-5-coordinate-systems-engine-choice.md).**

---

## Rendering & LOD Strategy

- **Multi-Resolution Terrain**  
  - Partition globe into tiles, use **octree/quadtree** per tile.  
  - Near: 0.25 m voxels.  
  - Far: merge into coarser cells / meshes.  
  - Example: [Octo voxel engine](https://github.com/OctoEngine/octo-voxel) (an open-source GPU-based voxel renderer) uses GPU ray-marching with LOD.  

- **Rendering Pipeline**  
  - Origin-relative rendering (subtract camera position).  
  - Frustum + occlusion culling.  
  - Multi-threaded chunk generation.  
  - Lighting simplified or precomputed at distance.  

- **Atmosphere & Skydome**  
  - Use skydome/sky-sphere shaders following camera.  
  - Weather/day-night at coarse resolution.

**📖 For detailed LOD algorithms, culling techniques, and rendering optimization, see [Step 7: Rendering & LOD Strategy](research/game-design/step-7-rendering-lod-strategy.md).**

---

## Physics, AI & Simulation

- **Chunk-Based Physics**  
  - Divide into local 1–10 km regions.  
  - Objects collide only within active region.  
  - Seamless transfer at boundaries.  
  - Use voxel raycasting in integer space for collisions.  

- **Physics Origin Shifting**  
  - Tie physics origin to camera/active region.  
  - Prevent 32-bit float drift.  

- **AI & Navigation**  
  - Hierarchical pathfinding: coarse global graph + fine local navmesh.  
  - Flow-field pathfinding for open areas.  
  - Dormant AI outside AOI.  

- **Procedural/Dynamic Terrain**  
  - Apply updates directly in voxel array.  
  - Rebuild octree indices asynchronously.  

---

## Voxel Data Storage & Streaming

- **Hybrid Array-Octree Storage**  
  - Flat voxel arrays (e.g. Zarr/HDF5) segmented into chunks.  
  - Example chunk: `128×128×128` (2,097,152 voxels), ~2 MB uncompressed (assuming 1 byte per voxel, e.g., `uint8` material IDs). For larger data types (e.g., `uint16` or `uint32`), chunk size will increase proportionally.
  - Store material IDs, compressed (Zstd).  

- **Cloud-Optimized Formats**  
  - COG for raster layers.  
  - PMTiles/Zarr for volumetric data.  
  - Pre-generate LOD pyramids (MIP).  

- **Spatial Indexing**  
  - Global: S2 geometry / Morton codes.  
  - Local: quadtrees / quadkeys.  
  - 3D queries: R-trees for irregular features.  

- **Streaming Strategy**  
  - Load high-res voxels around player.  
  - Medium resolution at mid-distance.  
  - Low-res background.  
  - Unload distant chunks, use LRU caches.

**📖 For detailed storage architecture, compression strategies, and streaming implementation, see [Step 6: Voxel Data Storage & Streaming](research/game-design/step-6-voxel-data-storage-streaming.md).**

---

## Networking & Multi-Layer Synchronization

- **Region-Based Sharding**  
  - Split world into continents/regions.  
  - Each region handled by its own server process.  
  - Seamless handoffs across regions/layers.  

- **Interest Management**  
  - AOI filtering for client updates.  
  - Vertical/layered LOD for subsurface/atmosphere.  

- **Cross-Region Effects**  
  - Cell tower handoff model.  
  - Pre-warm destination server with state.  
  - Split effect calculation across servers.  

- **Consistency**  
  - Local authority + eventual global consistency.  
  - Timestamped deltas for weather/day-night.  

---

## Scalable Architecture & Precision

- **Spatial Indexing**  
  - Hierarchical: global S2/Morton → regional quadtree.  
  - Index maps (x,y,z) to chunk IDs.  

- **Coordinate Transforms**  
  - Use EPSG:4087 (equidistant cylindrical) internally.  
  - Flat metric system: ±20,037,510 m extents.  

- **Floating-Point Errors**  
  - Keep players near origin.  
  - Split vertical world into zones with shifting origins.  

- **Parallelization**  
  - Multi-threaded chunk generation and mesh building.  
  - GPU compute for terrain generation or physics.  
  - Async streaming and index rebuild.  

---

## Frameworks & Research Directions

- **Engines**  
  - Unreal Engine 5 (Nanite + LWC).  
  - Flax Engine, Godot 4 (double precision).  
  - Octo voxel engine (Rust).  

- **Geospatial Libraries**  
  - GDAL/PROJ for coordinate transforms.  
  - S2 geometry, H3 indexing.  

- **Physics Engines**  
  - Jolt Physics, PhysX with origin shifting.  
  - Simplified fluid/atmosphere simulation.  

- **Data Storage**  
  - Zarr (cloud-friendly, multi-terabyte).  
  - Tile servers + STAC catalogs.  

- **Networking**  
  - MMO frameworks: Photon, SmartFox. (SpatialOS—discontinued 2022)  
  - Study WoW, EVE, GW2 architectures.  

---

## Key Takeaways

- **Hierarchical decomposition**: split the world by region, resolution, and layer.  
- **64-bit everywhere**: mandatory for precision.  
- **Origin shifting**: critical for both rendering and physics.  
- **Cloud-native storage**: Zarr/COG/PMTiles for planet-scale voxel/terrain data.  
- **Sharding + AOI networking**: only send what matters.  
- **Async + GPU acceleration**: keep performance viable.

**📖 For detailed explanations, code examples, and implementation guides for each principle, see [Step 8: MMORPG GIS Key Takeaways](research/game-design/step-8-mmorpg-gis-key-takeaways.md).**

---
