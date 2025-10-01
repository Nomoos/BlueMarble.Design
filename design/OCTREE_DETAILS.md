# Octree Details – Technical Specification

**Version:** 1.0  
**Date:** 2025-10-01  
**Owner:** Engineering Team

## Overview

This document describes the Octree system used in BlueMarble.Design for spatial management and 3D data storage.

---

## Octree Geometry and Dimensions

- **Root node (parent):**
  - Covers global range in X axis: **0 – 40 075 020 m** (Earth's circumference)
  - Y, Z: **0 – 20 037 510 m** (from equator to pole, typically half of X)
  - Dimensions can be adjusted according to simulation model needs.

- **Child nodes:**
  - Each division splits the space into 8 equal cuboids ("cubes").
  - Child nodes can be empty (empty material) or contain real data.
  - Empty regions are not allocated by the tree – memory optimization.

- **Level 0 (Root):**
  - Dimensions: X = 40 075 020 m, Y = 20 037 510 m, Z = 20 037 510 m

- **Level 1 (First division):**
  - 8 child nodes, each with dimensions X = 20 037 510 m, Y = 10 018 755 m, Z = 10 018 755 m

- **Additional levels:**
  - Each child node divides analogously, up to the required granularity.

---

## Storage Considerations

- **Optimization:**
  - Empty regions marked as "empty material" – tree is compressed.
  - Homogeneous regions can be automatically "collapsed" to save space.
  - Lazy inheritance support: material inherits from parent node if not explicitly defined.

- **Advantages:**
  - Fast spatial queries, efficient updates, low memory consumption.
  - Suitable for planetary surface simulations, geological layers, and MMO worlds.

- **Database storage:**
  - Spatial data (octree) stored in **Cassandra** (optimized for volume and speed).
  - Metadata and game data in **PostgreSQL**.

---

## Example Table – Octree Dimensions (per level)

| Level | X (m)       | Y (m)        | Z (m)        | Node Count |
|-------|-------------|--------------|--------------|------------|
| 0     | 40,075,020  | 20,037,510   | 20,037,510   | 1          |
| 1     | 20,037,510  | 10,018,755   | 10,018,755   | 8          |
| 2     | 10,018,755  | 5,009,377.5  | 5,009,377.5  | 64         |
| ...   | ...         | ...          | ...          | ...        |

---

## Design Guidelines

- Always use maximum X for root node.
- Do not allocate empty child nodes – mark as "empty".
- Child node dimensions must always be half of parent node in each axis.
- Support for homogeneous region collapsing and lazy inheritance.

---

## References

- [TECHNICAL_FOUNDATION.md](../docs/TECHNICAL_FOUNDATION.md)
- Database/Octree research documentation
- Cassandra schema design

---

**Status:** Complete  
**Last Updated:** 2025-10-01
