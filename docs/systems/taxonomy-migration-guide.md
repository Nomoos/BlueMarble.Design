# Taxonomy System Migration Guide

**Document Type:** Implementation Guide  
**Version:** 1.0  
**Date:** 2025-01-19

## Overview

This guide provides step-by-step instructions for migrating existing BlueMarble systems to use the new Taxonomy Classification System. The taxonomy system provides a flexible, hierarchical framework that replaces rigid, hardcoded categorization with dynamic, extensible classifications.

## Migration Scope

### Systems to Migrate

1. **Faction System** - Replace hardcoded faction types with flexible taxa
2. **Achievement Categories** - Migrate to taxonomic organization
3. **Item Categories** - Convert to hierarchical item classification
4. **Quest Types** - Organize quest types as taxa

### Benefits of Migration

- **Flexibility**: Add new categories without schema changes
- **Hierarchy**: Express multi-level organizational structures
- **Attributes**: Store domain-specific data per taxon
- **Relationships**: Express complex associations between entities
- **Consistency**: Unified classification system across domains

## Prerequisites

Before beginning migration:

1. **Backup Database**: Create full backup of production data
2. **Test Environment**: Set up staging environment for testing
3. **Review Documentation**: Read [Taxonomy Classification System](taxonomy-classification-system.md)
4. **Plan Downtime**: Schedule maintenance window if needed
5. **Prepare Rollback**: Have rollback scripts ready

---

## Phase 1: Faction System Migration

### Current Schema

```sql
CREATE TABLE factions (
    faction_id INT PRIMARY KEY,
    faction_name VARCHAR(200) NOT NULL,
    description TEXT,
    icon_url VARCHAR(500),
    faction_type VARCHAR(50) NOT NULL,
    parent_faction_id INT REFERENCES factions(faction_id),
    created_at TIMESTAMP WITH TIME ZONE DEFAULT NOW(),
    CONSTRAINT valid_faction_type CHECK (faction_type IN ('major', 'minor', 'guild', 'organization'))
);
```

### Migration Steps

#### Step 1: Create Taxa Table

```sql
-- Execute taxonomy schema creation
-- (from taxonomy-classification-system.md)
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
    updated_at TIMESTAMP WITH TIME ZONE DEFAULT NOW()
);

-- Create indexes
CREATE INDEX idx_taxa_domain ON taxa(domain);
CREATE INDEX idx_taxa_parent ON taxa(parent_taxon_id);
CREATE INDEX idx_taxa_rank ON taxa(domain, rank);
CREATE INDEX idx_taxa_name ON taxa(name);
CREATE INDEX idx_taxa_attributes ON taxa USING GIN(attributes);
```

#### Step 2: Create Root Taxonomy Structure

```sql
-- Create faction domain root
INSERT INTO taxa (taxon_id, domain, name, rank, description) VALUES
('faction-root', 'faction', 'All Factions and Organizations', 0, 
 'Root of all faction hierarchies');

-- Create main categories based on old faction_type values
INSERT INTO taxa (taxon_id, domain, name, parent_taxon_id, rank, attributes) VALUES
('faction-major', 'faction', 'Major Factions', 'faction-root', 1,
 '{"influence_level": "global", "min_members": 1000}'::jsonb),
 
('faction-minor', 'faction', 'Minor Factions', 'faction-root', 1,
 '{"influence_level": "regional", "min_members": 100}'::jsonb),
 
('faction-guild', 'faction', 'Professional Guilds', 'faction-root', 1,
 '{"focus": "professional_development", "membership_type": "skill-based"}'::jsonb),
 
('faction-organization', 'faction', 'Organizations', 'faction-root', 1,
 '{"structure": "formal", "governance": "structured"}'::jsonb);
```

#### Step 3: Migrate Existing Factions

```sql
-- Migration script to convert factions to taxa
INSERT INTO taxa (
    taxon_id,
    domain,
    name,
    scientific_name,
    parent_taxon_id,
    rank,
    description,
    attributes,
    created_at
)
SELECT 
    'faction-' || faction_id::text,
    'faction',
    faction_name,
    NULL, -- Add scientific_name if you have abbreviations
    CASE 
        -- Map to parent category based on faction_type
        WHEN parent_faction_id IS NULL THEN
            CASE faction_type
                WHEN 'major' THEN 'faction-major'
                WHEN 'minor' THEN 'faction-minor'
                WHEN 'guild' THEN 'faction-guild'
                WHEN 'organization' THEN 'faction-organization'
            END
        -- If has parent, reference parent taxon
        ELSE 'faction-' || parent_faction_id::text
    END,
    CASE 
        WHEN parent_faction_id IS NULL THEN 2  -- Direct child of category
        ELSE (
            -- Calculate rank based on parent's rank
            SELECT rank + 1 FROM taxa 
            WHERE taxon_id = 'faction-' || parent_faction_id::text
        )
    END,
    description,
    jsonb_build_object(
        'legacy_faction_id', faction_id,
        'faction_type', faction_type,
        'icon_url', icon_url
    ),
    created_at
FROM factions;
```

#### Step 4: Migrate Faction Relationships

```sql
-- Create taxon relationships from faction relationships
INSERT INTO taxon_relationships (
    source_taxon_id,
    target_taxon_id,
    relationship_type,
    strength,
    metadata
)
SELECT 
    'faction-' || faction_id::text,
    'faction-' || related_faction_id::text,
    CASE relationship_type
        WHEN 'ALLIED' THEN 'related'
        WHEN 'HOSTILE' THEN 'excludes'
        WHEN 'NEUTRAL' THEN 'related'
    END,
    influence_factor,
    jsonb_build_object(
        'legacy_relationship_type', relationship_type
    )
FROM faction_relationships;
```

#### Step 5: Update Character Reputation References

```sql
-- Add new column for taxon reference
ALTER TABLE character_faction_reputation 
ADD COLUMN taxon_id VARCHAR(64) REFERENCES taxa(taxon_id);

-- Populate taxon_id from faction_id
UPDATE character_faction_reputation
SET taxon_id = 'faction-' || faction_id::text;

-- After verification, make taxon_id NOT NULL and create index
ALTER TABLE character_faction_reputation 
ALTER COLUMN taxon_id SET NOT NULL;

CREATE INDEX idx_character_faction_rep_taxon 
ON character_faction_reputation(taxon_id);
```

#### Step 6: Update Application Code

```typescript
// OLD CODE
interface Faction {
  faction_id: number;
  faction_name: string;
  faction_type: 'major' | 'minor' | 'guild' | 'organization';
  parent_faction_id?: number;
}

async function getFactionsByType(type: string): Promise<Faction[]> {
  return db.query(
    'SELECT * FROM factions WHERE faction_type = $1',
    [type]
  );
}

// NEW CODE
interface FactionTaxon extends Taxon {
  domain: 'faction';
}

async function getFactionsByCategory(categoryTaxonId: string): Promise<FactionTaxon[]> {
  const response = await fetch(
    `/v1/taxonomy/taxa?domain=faction&parent_id=${categoryTaxonId}`
  );
  return response.json();
}

// Get all major factions
const majorFactions = await getFactionsByCategory('faction-major');

// Get faction hierarchy
async function getFactionHierarchy(factionTaxonId: string) {
  const response = await fetch(
    `/v1/taxonomy/taxa/${factionTaxonId}/hierarchy?direction=up`
  );
  return response.json();
}
```

#### Step 7: Deprecate Old Tables (Optional)

```sql
-- After successful migration and verification period
-- Rename old tables instead of dropping (for safety)
ALTER TABLE factions RENAME TO factions_deprecated;
ALTER TABLE faction_relationships RENAME TO faction_relationships_deprecated;

-- Or keep them as views for backward compatibility
CREATE VIEW factions AS
SELECT 
    CAST(SUBSTRING(taxon_id FROM 9) AS INT) as faction_id,
    name as faction_name,
    description,
    attributes->>'icon_url' as icon_url,
    attributes->>'faction_type' as faction_type,
    CAST(SUBSTRING(parent_taxon_id FROM 9) AS INT) as parent_faction_id,
    created_at
FROM taxa
WHERE domain = 'faction' AND rank >= 2;
```

---

## Phase 2: Achievement Categories Migration

### Current Structure

```sql
-- Achievements with hardcoded categories
CREATE TABLE achievements_master (
    achievement_id VARCHAR(64) PRIMARY KEY,
    category VARCHAR(50) NOT NULL,
    -- other fields...
    CONSTRAINT valid_category CHECK (category IN (
        'combat', 'exploration', 'social', 'crafting', 
        'collection', 'economic', 'event'
    ))
);
```

### Migration Steps

#### Step 1: Create Achievement Taxonomy

```sql
-- Root
INSERT INTO taxa (taxon_id, domain, name, rank) VALUES
('achievement-root', 'achievement', 'All Achievements', 0);

-- Main categories
INSERT INTO taxa (taxon_id, domain, name, parent_taxon_id, rank, attributes) VALUES
('achievement-combat', 'achievement', 'Combat Achievements', 'achievement-root', 1,
 '{"icon": "sword", "color": "#DC143C"}'::jsonb),
 
('achievement-exploration', 'achievement', 'Exploration Achievements', 'achievement-root', 1,
 '{"icon": "compass", "color": "#4169E1"}'::jsonb),
 
('achievement-social', 'achievement', 'Social Achievements', 'achievement-root', 1,
 '{"icon": "users", "color": "#32CD32"}'::jsonb),
 
('achievement-crafting', 'achievement', 'Crafting Achievements', 'achievement-root', 1,
 '{"icon": "hammer", "color": "#DAA520"}'::jsonb),
 
('achievement-collection', 'achievement', 'Collection Achievements', 'achievement-root', 1,
 '{"icon": "treasure", "color": "#9370DB"}'::jsonb),
 
('achievement-economic', 'achievement', 'Economic Achievements', 'achievement-root', 1,
 '{"icon": "coins", "color": "#FFD700"}'::jsonb),
 
('achievement-event', 'achievement', 'Event Achievements', 'achievement-root', 1,
 '{"icon": "star", "color": "#FF69B4"}'::jsonb);

-- Add subcategories (example for combat)
INSERT INTO taxa (taxon_id, domain, name, parent_taxon_id, rank, attributes) VALUES
('achievement-combat-boss', 'achievement', 'Boss Kill Achievements', 'achievement-combat', 2,
 '{"type": "boss_kills", "difficulty": "high"}'::jsonb),
 
('achievement-combat-mastery', 'achievement', 'Combat Mastery', 'achievement-combat', 2,
 '{"type": "skill_mastery", "progressive": true}'::jsonb);
```

#### Step 2: Link Achievements to Taxa

```sql
-- Add taxon reference
ALTER TABLE achievements_master 
ADD COLUMN category_taxon_id VARCHAR(64) REFERENCES taxa(taxon_id);

-- Map existing categories to taxa
UPDATE achievements_master
SET category_taxon_id = 'achievement-' || category;

-- Create index
CREATE INDEX idx_achievements_category_taxon 
ON achievements_master(category_taxon_id);
```

---

## Phase 3: Item Categories Migration

### Migration Steps

#### Step 1: Create Item Taxonomy

```sql
-- Create comprehensive item taxonomy
INSERT INTO taxa (taxon_id, domain, name, rank) VALUES
('item-root', 'item', 'All Items', 0);

INSERT INTO taxa (taxon_id, domain, name, parent_taxon_id, rank) VALUES
('item-equipment', 'item', 'Equipment', 'item-root', 1),
('item-resources', 'item', 'Resources', 'item-root', 1),
('item-consumables', 'item', 'Consumables', 'item-root', 1),
('item-special', 'item', 'Special Items', 'item-root', 1);

-- Equipment subtypes
INSERT INTO taxa (taxon_id, domain, name, parent_taxon_id, rank, attributes) VALUES
('item-equipment-tools', 'item', 'Tools', 'item-equipment', 2,
 '{"durability_system": true, "skill_requirements": true}'::jsonb),
 
('item-equipment-armor', 'item', 'Armor', 'item-equipment', 2,
 '{"defense_bonus": true, "equipment_slot": true}'::jsonb),
 
('item-equipment-weapons', 'item', 'Weapons', 'item-equipment', 2,
 '{"attack_bonus": true, "weapon_type": true}'::jsonb);
```

#### Step 2: Migrate Item Categories

```sql
-- Add item taxonomy references
ALTER TABLE items
ADD COLUMN category_taxon_id VARCHAR(64) REFERENCES taxa(taxon_id);

-- Map items to appropriate taxa based on existing category
UPDATE items
SET category_taxon_id = CASE 
    WHEN item_type = 'tool' THEN 'item-equipment-tools'
    WHEN item_type = 'armor' THEN 'item-equipment-armor'
    WHEN item_type = 'weapon' THEN 'item-equipment-weapons'
    WHEN item_type = 'resource' THEN 'item-resources'
    WHEN item_type = 'consumable' THEN 'item-consumables'
    ELSE 'item-special'
END;
```

---

## Phase 4: Quest Types Migration

### Migration Steps

```sql
-- Create quest taxonomy
INSERT INTO taxa (taxon_id, domain, name, rank) VALUES
('quest-root', 'quest', 'All Quests', 0);

INSERT INTO taxa (taxon_id, domain, name, parent_taxon_id, rank, attributes) VALUES
('quest-main', 'quest', 'Main Story', 'quest-root', 1,
 '{"importance": "critical", "repeatable": false}'::jsonb),
 
('quest-side', 'quest', 'Side Quests', 'quest-root', 1,
 '{"importance": "optional", "repeatable": false}'::jsonb),
 
('quest-daily', 'quest', 'Daily Quests', 'quest-root', 1,
 '{"importance": "routine", "repeatable": true, "cooldown": "24h"}'::jsonb),
 
('quest-faction', 'quest', 'Faction Quests', 'quest-root', 1,
 '{"faction_locked": true, "reputation_reward": true}'::jsonb);

-- Add taxon reference to quests
ALTER TABLE quests
ADD COLUMN quest_type_taxon_id VARCHAR(64) REFERENCES taxa(taxon_id);

UPDATE quests
SET quest_type_taxon_id = 'quest-' || quest_type;
```

---

## Validation and Testing

### Validation Checklist

- [ ] All existing factions mapped to taxa
- [ ] Faction hierarchies preserved correctly
- [ ] Character reputation data migrated
- [ ] Achievement categories linked to taxa
- [ ] Item categories converted
- [ ] Quest types migrated
- [ ] API endpoints tested
- [ ] Performance benchmarks met
- [ ] No data loss verified

### Testing Script

```sql
-- Verify faction migration
SELECT 
    COUNT(*) as legacy_count,
    (SELECT COUNT(*) FROM taxa WHERE domain = 'faction') as taxon_count
FROM factions_deprecated;

-- Check for orphaned records
SELECT taxon_id, name
FROM taxa
WHERE domain = 'faction' 
  AND parent_taxon_id IS NOT NULL
  AND parent_taxon_id NOT IN (SELECT taxon_id FROM taxa);

-- Verify reputation links
SELECT COUNT(*) as missing_taxon_refs
FROM character_faction_reputation
WHERE taxon_id IS NULL;
```

### Performance Testing

```sql
-- Compare query performance
EXPLAIN ANALYZE
SELECT * FROM factions_deprecated WHERE faction_type = 'major';

EXPLAIN ANALYZE
SELECT * FROM taxa WHERE domain = 'faction' AND parent_taxon_id = 'faction-major';
```

---

## Rollback Plan

### Rollback Steps

1. **Stop Application**: Prevent new data writes
2. **Restore from Backup**: Use pre-migration backup
3. **Verify Data Integrity**: Run validation checks
4. **Restart Application**: Resume normal operations
5. **Document Issues**: Record problems for analysis

### Rollback Script Template

```sql
BEGIN;

-- Drop new columns
ALTER TABLE character_faction_reputation DROP COLUMN IF EXISTS taxon_id;
ALTER TABLE achievements_master DROP COLUMN IF EXISTS category_taxon_id;
ALTER TABLE items DROP COLUMN IF EXISTS category_taxon_id;
ALTER TABLE quests DROP COLUMN IF EXISTS quest_type_taxon_id;

-- Restore original tables
DROP TABLE IF EXISTS factions;
DROP TABLE IF EXISTS faction_relationships;
ALTER TABLE factions_deprecated RENAME TO factions;
ALTER TABLE faction_relationships_deprecated RENAME TO faction_relationships;

-- Verify
SELECT COUNT(*) FROM factions;

COMMIT;
```

---

## Post-Migration Tasks

### Immediate Tasks

1. **Monitor Performance**: Track query times and system load
2. **Review Logs**: Check for errors or warnings
3. **User Communication**: Inform users of any changes
4. **Documentation Update**: Update API and user docs

### Long-term Tasks

1. **Expand Taxonomy**: Add more specific taxa as needed
2. **Optimize Queries**: Create materialized views for common queries
3. **Clean Up**: Remove deprecated tables after verification period
4. **Training**: Educate development team on taxonomy system

---

## Troubleshooting

### Common Issues

#### Issue: Circular Parent References

**Problem**: Taxa with circular parent relationships

**Solution**:
```sql
-- Find circular references
WITH RECURSIVE hierarchy AS (
    SELECT taxon_id, parent_taxon_id, 1 as depth
    FROM taxa
    WHERE domain = 'faction'
    UNION ALL
    SELECT t.taxon_id, t.parent_taxon_id, h.depth + 1
    FROM taxa t
    JOIN hierarchy h ON t.taxon_id = h.parent_taxon_id
    WHERE h.depth < 10
)
SELECT * FROM hierarchy WHERE depth > 5;

-- Fix by updating parent
UPDATE taxa SET parent_taxon_id = 'faction-root' 
WHERE taxon_id = '<problematic_taxon>';
```

#### Issue: Missing Parent Taxa

**Problem**: Child taxa reference non-existent parents

**Solution**:
```sql
-- Find orphans
SELECT t.taxon_id, t.name, t.parent_taxon_id
FROM taxa t
LEFT JOIN taxa p ON t.parent_taxon_id = p.taxon_id
WHERE t.parent_taxon_id IS NOT NULL AND p.taxon_id IS NULL;

-- Create missing parent or reassign
INSERT INTO taxa (taxon_id, domain, name, rank) 
VALUES ('<missing_parent_id>', 'faction', 'Unknown Category', 1);
```

#### Issue: Performance Degradation

**Problem**: Slow queries on taxonomy tables

**Solution**:
```sql
-- Add missing indexes
CREATE INDEX CONCURRENTLY idx_taxa_domain_parent 
ON taxa(domain, parent_taxon_id);

-- Analyze tables
ANALYZE taxa;
ANALYZE taxon_relationships;

-- Consider materialized views for complex hierarchies
CREATE MATERIALIZED VIEW faction_hierarchy_cache AS
WITH RECURSIVE hierarchy AS (
    SELECT taxon_id, name, parent_taxon_id, 0 as depth, 
           ARRAY[taxon_id] as path
    FROM taxa WHERE domain = 'faction' AND parent_taxon_id IS NULL
    UNION ALL
    SELECT t.taxon_id, t.name, t.parent_taxon_id, h.depth + 1,
           h.path || t.taxon_id
    FROM taxa t
    JOIN hierarchy h ON t.parent_taxon_id = h.taxon_id
)
SELECT * FROM hierarchy;
```

---

## Support and Resources

### Documentation References

- [Taxonomy Classification System](taxonomy-classification-system.md) - System design
- [Taxonomy API](api-taxonomy-system.md) - API reference
- [Taxonomy Usage Examples](taxonomy-usage-examples.md) - Practical examples
- [Database Schema Design](database-schema-design.md) - Overall schema

### Getting Help

- Technical Questions: Contact Database Architecture Team
- Migration Support: Submit ticket to DevOps team
- Bug Reports: Create issue in repository
- Feature Requests: Discuss with Product team

---

## Changelog

### Version 1.0 (2025-01-19)

- Initial migration guide
- Faction system migration detailed
- Achievement, item, and quest migrations outlined
- Validation and rollback procedures documented
