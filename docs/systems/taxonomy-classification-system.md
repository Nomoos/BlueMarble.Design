# BlueMarble - Taxonomy Classification System Design

**Version:** 1.0  
**Date:** 2025-01-19  
**Author:** BlueMarble Systems Team

## Overview

This document describes the Taxon system - a general-purpose hierarchical classification framework for BlueMarble. A **taxon** (plural: taxa) represents a hierarchically unspecified unit in a classification system, allowing flexible categorization without predetermined level constraints.

## Concept

The Taxon system provides a flexible, hierarchical classification framework that can be applied to multiple game systems:

- **Factions and Organizations** - hierarchical organizational structures
- **Species and Creatures** - biological classification
- **Geological Features** - terrain and formation hierarchies  
- **Item Categories** - equipment and resource taxonomies
- **Quest Types** - mission classification systems
- **Achievements** - accomplishment categorization

### Key Principles

1. **Level-agnostic**: Taxa don't assume specific hierarchy levels (like Kingdom → Phylum → Class)
2. **Self-describing**: Each taxon defines its own rank/tier within its classification system
3. **Multi-parent capable**: Taxa can belong to multiple classification hierarchies
4. **Domain-specific**: Each classification system operates independently
5. **Extensible**: New classification domains can be added without schema changes

## System Architecture

```
┌─────────────────────────────────────────────────────────────┐
│                  Taxonomy Classification System              │
└─────────────────┬───────────────────────────────────────────┘
                  │
        ┌─────────┴─────────┐
        │                   │
        ↓                   ↓
┌──────────────┐    ┌──────────────┐
│  Taxon Core  │    │ Classification│
│              │←───│   Domains     │
│  - ID        │    │              │
│  - Name      │    │ - Faction    │
│  - Parent    │    │ - Species    │
│  - Rank      │    │ - Geological │
│  - Domain    │    │ - Item       │
└──────┬───────┘    └──────────────┘
       │
       ↓
┌──────────────────────────────┐
│   Taxon Relationships        │
│                              │
│  - Parent-Child (hierarchy)  │
│  - Synonyms (equivalent)     │
│  - Related (cross-reference) │
└──────────────────────────────┘
```

## Data Model

### Taxon Structure

```typescript
interface Taxon {
  // Identity
  taxon_id: string;              // Unique identifier
  domain: TaxonomyDomain;        // Classification system
  name: string;                  // Display name
  scientific_name?: string;      // Formal/technical name
  
  // Hierarchy
  parent_taxon_id?: string;      // Parent in hierarchy (null = root)
  rank: number;                  // Hierarchical level (0 = root, higher = more specific)
  rank_name?: string;            // Optional rank label (e.g., "Phylum", "Division", "Order")
  
  // Metadata
  description?: string;          // Detailed description
  attributes: Record<string, any>; // Domain-specific attributes
  is_deprecated: boolean;        // Taxonomic status
  
  // Relationships
  synonyms: string[];            // Equivalent taxa in other systems
  
  // Tracking
  created_at: Date;
  updated_at: Date;
}

enum TaxonomyDomain {
  FACTION = "faction",           // Organizational hierarchies
  SPECIES = "species",           // Biological classification
  GEOLOGICAL = "geological",     // Terrain/formation types
  ITEM = "item",                 // Equipment and resources
  QUEST = "quest",               // Mission types
  ACHIEVEMENT = "achievement",   // Accomplishment categories
  CUSTOM = "custom"              // User-defined domains
}
```

### Taxon Relationships

```typescript
interface TaxonRelationship {
  source_taxon_id: string;
  target_taxon_id: string;
  relationship_type: RelationshipType;
  strength: number;              // 0.0 to 1.0
  metadata?: Record<string, any>;
}

enum RelationshipType {
  PARENT = "parent",             // Hierarchical parent
  CHILD = "child",               // Hierarchical child
  SYNONYM = "synonym",           // Equivalent taxon
  RELATED = "related",           // Cross-reference
  EXCLUDES = "excludes",         // Mutually exclusive
  REQUIRES = "requires"          // Dependency
}
```

## Database Schema

```sql
-- Core taxonomy table
CREATE TABLE taxa (
    taxon_id VARCHAR(64) PRIMARY KEY,
    domain VARCHAR(32) NOT NULL,
    name VARCHAR(255) NOT NULL,
    scientific_name VARCHAR(255),
    parent_taxon_id VARCHAR(64) REFERENCES taxa(taxon_id),
    rank INT NOT NULL DEFAULT 0,
    rank_name VARCHAR(64),
    description TEXT,
    attributes JSONB DEFAULT '{}',
    is_deprecated BOOLEAN DEFAULT FALSE,
    created_at TIMESTAMP WITH TIME ZONE DEFAULT NOW(),
    updated_at TIMESTAMP WITH TIME ZONE DEFAULT NOW(),
    
    CONSTRAINT valid_rank CHECK (rank >= 0),
    CONSTRAINT valid_domain CHECK (domain IN (
        'faction', 'species', 'geological', 'item', 
        'quest', 'achievement', 'custom'
    )),
    CONSTRAINT no_self_parent CHECK (taxon_id != parent_taxon_id)
);

-- Indexes for efficient queries
CREATE INDEX idx_taxa_domain ON taxa(domain);
CREATE INDEX idx_taxa_parent ON taxa(parent_taxon_id);
CREATE INDEX idx_taxa_rank ON taxa(domain, rank);
CREATE INDEX idx_taxa_name ON taxa(name);
CREATE INDEX idx_taxa_attributes ON taxa USING GIN(attributes);

-- Taxon relationships
CREATE TABLE taxon_relationships (
    source_taxon_id VARCHAR(64) NOT NULL REFERENCES taxa(taxon_id) ON DELETE CASCADE,
    target_taxon_id VARCHAR(64) NOT NULL REFERENCES taxa(taxon_id) ON DELETE CASCADE,
    relationship_type VARCHAR(32) NOT NULL,
    strength DECIMAL(3,2) DEFAULT 1.0,
    metadata JSONB DEFAULT '{}',
    created_at TIMESTAMP WITH TIME ZONE DEFAULT NOW(),
    
    PRIMARY KEY (source_taxon_id, target_taxon_id, relationship_type),
    CONSTRAINT valid_relationship_type CHECK (relationship_type IN (
        'parent', 'child', 'synonym', 'related', 'excludes', 'requires'
    )),
    CONSTRAINT valid_strength CHECK (strength BETWEEN 0 AND 1),
    CONSTRAINT no_self_relationship CHECK (source_taxon_id != target_taxon_id)
);

CREATE INDEX idx_taxon_relationships_source ON taxon_relationships(source_taxon_id);
CREATE INDEX idx_taxon_relationships_target ON taxon_relationships(target_taxon_id);
CREATE INDEX idx_taxon_relationships_type ON taxon_relationships(relationship_type);

-- Taxon synonyms (for quick lookup)
CREATE TABLE taxon_synonyms (
    taxon_id VARCHAR(64) NOT NULL REFERENCES taxa(taxon_id) ON DELETE CASCADE,
    synonym VARCHAR(255) NOT NULL,
    language VARCHAR(8) DEFAULT 'en',
    is_preferred BOOLEAN DEFAULT FALSE,
    
    PRIMARY KEY (taxon_id, synonym, language)
);

CREATE INDEX idx_taxon_synonyms_lookup ON taxon_synonyms(synonym);
```

## Usage Examples

### Example 1: Faction Hierarchy

```typescript
// Root: All Factions
const allFactions = {
  taxon_id: "faction-root",
  domain: TaxonomyDomain.FACTION,
  name: "All Factions",
  rank: 0,
  parent_taxon_id: null,
  attributes: {}
};

// Major Faction Category
const majorFactions = {
  taxon_id: "faction-major",
  domain: TaxonomyDomain.FACTION,
  name: "Major Factions",
  rank: 1,
  parent_taxon_id: "faction-root",
  attributes: {
    influence_level: "continental",
    min_members: 1000
  }
};

// Specific Faction
const geologistGuild = {
  taxon_id: "faction-geologist-guild",
  domain: TaxonomyDomain.FACTION,
  name: "International Geological Survey",
  scientific_name: "IGS",
  rank: 2,
  parent_taxon_id: "faction-major",
  attributes: {
    specialization: "geology",
    founded: "2045",
    headquarters: "Geneva"
  }
};

// Sub-faction
const fieldSurveyors = {
  taxon_id: "faction-igs-field",
  domain: TaxonomyDomain.FACTION,
  name: "Field Survey Division",
  rank: 3,
  parent_taxon_id: "faction-geologist-guild",
  attributes: {
    role: "field_work",
    size: 250
  }
};
```

### Example 2: Species Classification

```typescript
// Biological taxonomy for game creatures
const animalia = {
  taxon_id: "species-animalia",
  domain: TaxonomyDomain.SPECIES,
  name: "Animalia",
  rank: 0,
  rank_name: "Kingdom",
  parent_taxon_id: null
};

const chordata = {
  taxon_id: "species-chordata",
  domain: TaxonomyDomain.SPECIES,
  name: "Chordata",
  rank: 1,
  rank_name: "Phylum",
  parent_taxon_id: "species-animalia"
};

const mammalia = {
  taxon_id: "species-mammalia",
  domain: TaxonomyDomain.SPECIES,
  name: "Mammalia",
  rank: 2,
  rank_name: "Class",
  parent_taxon_id: "species-chordata"
};

const carnivora = {
  taxon_id: "species-carnivora",
  domain: TaxonomyDomain.SPECIES,
  name: "Carnivora",
  rank: 3,
  rank_name: "Order",
  parent_taxon_id: "species-mammalia",
  attributes: {
    diet: "carnivorous",
    hunting_behavior: true
  }
};
```

### Example 3: Geological Features

```typescript
const geologicalRoot = {
  taxon_id: "geo-root",
  domain: TaxonomyDomain.GEOLOGICAL,
  name: "Geological Features",
  rank: 0,
  parent_taxon_id: null
};

const mountains = {
  taxon_id: "geo-mountains",
  domain: TaxonomyDomain.GEOLOGICAL,
  name: "Mountain Formations",
  rank: 1,
  parent_taxon_id: "geo-root",
  attributes: {
    formation_type: "tectonic",
    min_elevation: 1000
  }
};

const volcanicMountains = {
  taxon_id: "geo-volcanic-mountains",
  domain: TaxonomyDomain.GEOLOGICAL,
  name: "Volcanic Mountains",
  rank: 2,
  parent_taxon_id: "geo-mountains",
  attributes: {
    formation_type: "volcanic",
    activity_level: ["active", "dormant", "extinct"]
  }
};
```

## API Design

### Get Taxon by ID

```http
GET /v1/taxonomy/taxa/{taxon_id}
```

**Response:**
```json
{
  "taxon_id": "faction-geologist-guild",
  "domain": "faction",
  "name": "International Geological Survey",
  "scientific_name": "IGS",
  "parent_taxon_id": "faction-major",
  "rank": 2,
  "rank_name": null,
  "description": "A major international organization dedicated to geological research",
  "attributes": {
    "specialization": "geology",
    "founded": "2045"
  },
  "is_deprecated": false,
  "created_at": "2025-01-19T10:00:00Z",
  "updated_at": "2025-01-19T10:00:00Z"
}
```

### List Taxa by Domain

```http
GET /v1/taxonomy/taxa?domain={domain}&parent_id={parent_id}&rank={rank}
```

**Query Parameters:**
- `domain` (required): taxonomy domain
- `parent_id` (optional): filter by parent taxon
- `rank` (optional): filter by hierarchical rank
- `include_deprecated` (optional): include deprecated taxa

### Get Taxon Hierarchy

```http
GET /v1/taxonomy/taxa/{taxon_id}/hierarchy?direction={up|down}&max_depth={n}
```

**Parameters:**
- `direction`: "up" for ancestors, "down" for descendants
- `max_depth`: maximum levels to traverse

**Response:**
```json
{
  "taxon_id": "faction-igs-field",
  "hierarchy": [
    {
      "taxon_id": "faction-root",
      "name": "All Factions",
      "rank": 0,
      "depth": 3
    },
    {
      "taxon_id": "faction-major",
      "name": "Major Factions",
      "rank": 1,
      "depth": 2
    },
    {
      "taxon_id": "faction-geologist-guild",
      "name": "International Geological Survey",
      "rank": 2,
      "depth": 1
    },
    {
      "taxon_id": "faction-igs-field",
      "name": "Field Survey Division",
      "rank": 3,
      "depth": 0
    }
  ]
}
```

### Search Taxa

```http
GET /v1/taxonomy/taxa/search?q={query}&domain={domain}
```

Supports full-text search across names, synonyms, and descriptions.

## Integration with Existing Systems

### Faction System Migration

The existing faction system can be migrated to use taxa:

```sql
-- Migration: Convert existing factions to taxa
INSERT INTO taxa (taxon_id, domain, name, parent_taxon_id, rank, attributes)
SELECT 
    'faction-' || faction_id::text,
    'faction',
    faction_name,
    CASE 
        WHEN parent_faction_id IS NOT NULL 
        THEN 'faction-' || parent_faction_id::text 
        ELSE NULL 
    END,
    CASE faction_type
        WHEN 'major' THEN 1
        WHEN 'minor' THEN 2
        WHEN 'guild' THEN 2
        WHEN 'organization' THEN 3
        ELSE 0
    END,
    jsonb_build_object(
        'faction_type', faction_type,
        'icon_url', icon_url
    )
FROM factions;
```

### Achievement Categories

Achievement categories can be represented as taxa:

```typescript
const achievementRoot = {
  taxon_id: "achievement-root",
  domain: TaxonomyDomain.ACHIEVEMENT,
  name: "All Achievements",
  rank: 0
};

const combatAchievements = {
  taxon_id: "achievement-combat",
  domain: TaxonomyDomain.ACHIEVEMENT,
  name: "Combat Achievements",
  rank: 1,
  parent_taxon_id: "achievement-root",
  attributes: {
    category: "combat",
    icon: "sword"
  }
};
```

## Query Patterns

### Find All Children of a Taxon

```sql
-- Direct children
SELECT * FROM taxa 
WHERE parent_taxon_id = 'faction-geologist-guild';

-- All descendants (recursive)
WITH RECURSIVE descendants AS (
    SELECT * FROM taxa WHERE taxon_id = 'faction-geologist-guild'
    UNION ALL
    SELECT t.* FROM taxa t
    INNER JOIN descendants d ON t.parent_taxon_id = d.taxon_id
)
SELECT * FROM descendants WHERE taxon_id != 'faction-geologist-guild';
```

### Find Ancestor Path

```sql
WITH RECURSIVE ancestors AS (
    SELECT * FROM taxa WHERE taxon_id = 'faction-igs-field'
    UNION ALL
    SELECT t.* FROM taxa t
    INNER JOIN ancestors a ON t.taxon_id = a.parent_taxon_id
)
SELECT * FROM ancestors ORDER BY rank ASC;
```

### Find Siblings

```sql
SELECT * FROM taxa 
WHERE parent_taxon_id = (
    SELECT parent_taxon_id FROM taxa WHERE taxon_id = 'faction-igs-field'
)
AND taxon_id != 'faction-igs-field';
```

## Best Practices

1. **Consistent Naming**: Use clear, descriptive names for taxa
2. **Appropriate Ranks**: Assign ranks consistently within each domain
3. **Meaningful Attributes**: Store domain-specific data in attributes field
4. **Avoid Deep Nesting**: Keep hierarchies manageable (typically 3-7 levels)
5. **Document Rank Names**: Use rank_name to clarify hierarchy levels
6. **Handle Deprecation**: Mark obsolete taxa instead of deleting
7. **Use Relationships**: Leverage relationships for complex associations

## Performance Considerations

- **Materialized Paths**: For frequently queried hierarchies, consider storing full paths
- **Caching**: Cache commonly accessed taxonomies (factions, achievement categories)
- **Index Strategy**: Ensure proper indexing on parent_taxon_id and domain
- **Limit Depth**: Avoid recursive queries on very deep hierarchies

## Future Extensions

1. **Versioning**: Track taxonomic changes over time
2. **Multi-language Support**: Expand synonym system for localization
3. **Access Control**: Add permission system for taxonomy management
4. **Validation Rules**: Domain-specific validation for taxon creation
5. **Import/Export**: Tools for bulk taxonomy management
6. **Visualization**: Tree/graph views of taxonomic hierarchies

## References

- Biological Taxonomy: [Wikipedia - Taxonomy](https://en.wikipedia.org/wiki/Taxonomy)
- Nested Set Model: [Managing Hierarchical Data in MySQL](http://mikehillyer.com/articles/managing-hierarchical-data-in-mysql/)
- Closure Tables: [Bill Karwin - SQL Antipatterns](https://pragprog.com/titles/bksqla/sql-antipatterns/)
