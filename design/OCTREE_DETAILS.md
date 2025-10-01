# Octree Details – Technical Specification

**Version:** 1.0  
**Date:** 2025-10-01  
**Owner:** Engineering Team

## Overview

Tento dokument popisuje systém Octree používaný v BlueMarble.Design pro prostorovou správu a ukládání 3D dat.

---

## Octree Geometry and Dimensions

- **Root node (parent):**
  - Pokrývá globální rozsah v ose X: **0 – 40 075 020 m** (obvod Země)
  - Y, Z: **0 – 20 037 510 m** (od rovníku k pólu, typicky polovina X)
  - Rozměry lze upravit podle potřeby simulačního modelu.

- **Child nodes:**
  - Každé dělení rozdělí prostor na 8 stejných kvádrů („kostek").
  - Child nodes mohou být prázdné (empty material) nebo obsahovat reálná data.
  - Prázdné oblasti strom nealokuje – optimalizace paměti.

- **Level 0 (Root):**
  - Rozměr: X = 40 075 020 m, Y = 20 037 510 m, Z = 20 037 510 m

- **Level 1 (First division):**
  - 8 child nodes, každý s rozměry X = 20 037 510 m, Y = 10 018 755 m, Z = 10 018 755 m

- **Další úrovně:**
  - Každá child node se dělí analogicky, až do požadované granularitiy.

---

## Storage Considerations

- **Optimalizace:**
  - Prázdné oblasti označeny jako „empty material" – strom je komprimovaný.
  - Homogenní oblasti lze automaticky „sloučit" pro úsporu místa.
  - Podpora lazy inheritance: materiál dědí od parent uzlu, pokud není explicitně definován.

- **Výhody:**
  - Rychlé prostorové dotazy, efektivní aktualizace, nízká spotřeba paměti.
  - Vhodné pro simulace planetárních povrchů, geologické vrstvy a MMO světy.

- **Uložení v databázi:**
  - Prostorová data (octree) ukládána v **Cassandra** (optimalizace na objem, rychlost).
  - Metadata a herní data v **PostgreSQL**.

---

## Example Table – Octree Dimensions (per level)

| Level | X (m)       | Y (m)        | Z (m)        | Počet uzlů |
|-------|-------------|--------------|--------------|------------|
| 0     | 40,075,020  | 20,037,510   | 20,037,510   | 1          |
| 1     | 20,037,510  | 10,018,755   | 10,018,755   | 8          |
| 2     | 10,018,755  | 5,009,377.5  | 5,009,377.5  | 64         |
| ...   | ...         | ...          | ...          | ...        |

---

## Design Guidelines

- Vždy používat maximální X pro root node.
- Prázdné child nodes nealokovat – označovat jako „empty".
- Rozměry child nodes musí být vždy polovinou parent node v každé ose.
- Podpora pro homogenní region collapsing a lazy inheritance.

---

## References

- [TECHNICAL_FOUNDATION.md](../docs/TECHNICAL_FOUNDATION.md)
- Database/Octree research documentation
- Cassandra schema design

---

**Status:** Complete  
**Last Updated:** 2025-10-01
