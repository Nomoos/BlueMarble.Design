# Taxonomy System - Usage Examples

**Document Type:** Implementation Guide  
**Version:** 1.0  
**Date:** 2025-01-19

## Overview

This document provides practical examples of using the Taxonomy Classification System across different game domains in BlueMarble. Each example demonstrates real-world usage patterns and best practices.

## Table of Contents

1. [Faction Hierarchies](#faction-hierarchies)
2. [Species Classification](#species-classification)
3. [Geological Features](#geological-features)
4. [Item Categorization](#item-categorization)
5. [Achievement Organization](#achievement-organization)
6. [Quest Type Classification](#quest-type-classification)

---

## Faction Hierarchies

### Example: Complete Faction Taxonomy

This example shows how to organize factions and organizations in a hierarchical structure.

```typescript
// Define the taxonomy for factions
const factionTaxonomy = [
  {
    taxon_id: "faction-root",
    domain: "faction",
    name: "All Organizations",
    rank: 0,
    parent_taxon_id: null,
    description: "Root of all faction and organization hierarchies"
  },
  
  // Level 1: Major Categories
  {
    taxon_id: "faction-major",
    domain: "faction",
    name: "Major Factions",
    rank: 1,
    parent_taxon_id: "faction-root",
    attributes: {
      influence_scope: "global",
      min_members: 1000,
      political_power: "high"
    }
  },
  {
    taxon_id: "faction-minor",
    domain: "faction",
    name: "Minor Factions",
    rank: 1,
    parent_taxon_id: "faction-root",
    attributes: {
      influence_scope: "regional",
      min_members: 100,
      political_power: "medium"
    }
  },
  {
    taxon_id: "faction-professional",
    domain: "faction",
    name: "Professional Guilds",
    rank: 1,
    parent_taxon_id: "faction-root",
    attributes: {
      focus: "professional_development",
      membership_type: "skill-based"
    }
  },
  
  // Level 2: Specific Organizations
  {
    taxon_id: "faction-igs",
    domain: "faction",
    name: "International Geological Survey",
    scientific_name: "IGS",
    rank: 2,
    parent_taxon_id: "faction-professional",
    description: "Premier organization for geological research and education",
    attributes: {
      specialization: "geology",
      founded: "2045",
      headquarters: "Geneva, Switzerland",
      member_count: 2500,
      reputation_requirement: 0
    }
  },
  {
    taxon_id: "faction-ims",
    domain: "faction",
    name: "International Mining Syndicate",
    scientific_name: "IMS",
    rank: 2,
    parent_taxon_id: "faction-professional",
    attributes: {
      specialization: "mining",
      founded: "2041",
      headquarters: "Perth, Australia",
      member_count: 3200
    }
  },
  
  // Level 3: Divisions/Departments
  {
    taxon_id: "faction-igs-field",
    domain: "faction",
    name: "Field Survey Division",
    rank: 3,
    parent_taxon_id: "faction-igs",
    description: "On-site geological survey teams operating worldwide",
    attributes: {
      role: "field_operations",
      team_count: 45,
      active_projects: 127
    }
  },
  {
    taxon_id: "faction-igs-research",
    domain: "faction",
    name: "Research & Analysis Division",
    rank: 3,
    parent_taxon_id: "faction-igs",
    attributes: {
      role: "research",
      facilities: ["Geneva Lab", "Tokyo Analysis Center", "Cape Town Institute"],
      publications_per_year: 350
    }
  },
  {
    taxon_id: "faction-igs-education",
    domain: "faction",
    name: "Education & Outreach Division",
    rank: 3,
    parent_taxon_id: "faction-igs",
    attributes: {
      role: "education",
      programs: ["Certification", "Workshops", "Field Training"],
      students_per_year: 800
    }
  },
  
  // Level 4: Regional Teams
  {
    taxon_id: "faction-igs-field-arctic",
    domain: "faction",
    name: "Arctic Survey Team",
    rank: 4,
    parent_taxon_id: "faction-igs-field",
    attributes: {
      region: "arctic",
      specialization: ["ice_cores", "permafrost", "glaciology"],
      team_size: 12,
      base_location: "Svalbard Research Station"
    }
  },
  {
    taxon_id: "faction-igs-field-volcanic",
    domain: "faction",
    name: "Volcanic Activity Monitoring Team",
    rank: 4,
    parent_taxon_id: "faction-igs-field",
    attributes: {
      region: "global",
      specialization: ["volcanology", "seismology"],
      team_size: 18,
      active_monitoring_sites: 47
    }
  }
];
```

### Usage: Query Faction Hierarchy

```typescript
// Get all professional guilds
async function getProfessionalGuilds() {
  const response = await fetch(
    '/v1/taxonomy/taxa?domain=faction&parent_id=faction-professional'
  );
  return response.json();
}

// Get complete hierarchy for Arctic team
async function getArcticTeamHierarchy() {
  const response = await fetch(
    '/v1/taxonomy/taxa/faction-igs-field-arctic/hierarchy?direction=up'
  );
  return response.json();
}

// Get all divisions of IGS
async function getIGSDivisions() {
  const response = await fetch(
    '/v1/taxonomy/taxa/faction-igs/children'
  );
  return response.json();
}
```

---

## Species Classification

### Example: Biological Taxonomy for Game Creatures

```typescript
const speciesTaxonomy = [
  // Kingdom level
  {
    taxon_id: "species-animalia",
    domain: "species",
    name: "Animalia",
    rank: 0,
    rank_name: "Kingdom",
    parent_taxon_id: null,
    description: "All animal life forms"
  },
  
  // Phylum level
  {
    taxon_id: "species-chordata",
    domain: "species",
    name: "Chordata",
    rank: 1,
    rank_name: "Phylum",
    parent_taxon_id: "species-animalia",
    description: "Animals with a notochord"
  },
  {
    taxon_id: "species-arthropoda",
    domain: "species",
    name: "Arthropoda",
    rank: 1,
    rank_name: "Phylum",
    parent_taxon_id: "species-animalia",
    description: "Invertebrates with exoskeletons"
  },
  
  // Class level
  {
    taxon_id: "species-mammalia",
    domain: "species",
    name: "Mammalia",
    rank: 2,
    rank_name: "Class",
    parent_taxon_id: "species-chordata",
    attributes: {
      characteristics: ["warm-blooded", "fur", "mammary_glands"],
      habitat: "terrestrial_primary"
    }
  },
  {
    taxon_id: "species-aves",
    domain: "species",
    name: "Aves",
    rank: 2,
    rank_name: "Class",
    parent_taxon_id: "species-chordata",
    attributes: {
      characteristics: ["feathers", "wings", "beaks"],
      mobility: "flight_capable"
    }
  },
  
  // Order level
  {
    taxon_id: "species-carnivora",
    domain: "species",
    name: "Carnivora",
    rank: 3,
    rank_name: "Order",
    parent_taxon_id: "species-mammalia",
    attributes: {
      diet: "carnivorous",
      hunting_behavior: true,
      pack_behavior: ["solitary", "pack", "pride"]
    }
  },
  {
    taxon_id: "species-rodentia",
    domain: "species",
    name: "Rodentia",
    rank: 3,
    rank_name: "Order",
    parent_taxon_id: "species-mammalia",
    attributes: {
      diet: "herbivorous",
      characteristics: ["continuously_growing_incisors"],
      population_density: "high"
    }
  },
  
  // Family level
  {
    taxon_id: "species-felidae",
    domain: "species",
    name: "Felidae",
    rank: 4,
    rank_name: "Family",
    parent_taxon_id: "species-carnivora",
    description: "Cats - all sizes from house cats to lions",
    attributes: {
      characteristics: ["retractable_claws", "night_vision"],
      hunting_style: "ambush_predator"
    }
  },
  
  // Genus level
  {
    taxon_id: "species-panthera",
    domain: "species",
    name: "Panthera",
    rank: 5,
    rank_name: "Genus",
    parent_taxon_id: "species-felidae",
    description: "Big cats capable of roaring",
    attributes: {
      size: "large",
      roar_capable: true,
      social_structure: "varies_by_species"
    }
  },
  
  // Species level
  {
    taxon_id: "species-panthera-leo",
    domain: "species",
    name: "Panthera leo",
    rank: 6,
    rank_name: "Species",
    parent_taxon_id: "species-panthera",
    description: "Lion - social big cat",
    attributes: {
      common_name: "Lion",
      habitat: "savanna",
      social_structure: "pride",
      conservation_status: "vulnerable",
      game_attributes: {
        threat_level: "high",
        loot_table: "big_cat_tier3",
        experience_value: 500,
        tameable: false
      }
    }
  }
];
```

### Usage: Species-based Game Mechanics

```typescript
// Get all carnivores for spawning in a region
async function getCarnivoresForRegion(biome: string) {
  const response = await fetch(
    '/v1/taxonomy/taxa/species-carnivora/hierarchy?direction=down&max_depth=5'
  );
  const hierarchy = await response.json();
  
  // Filter species suitable for the biome
  return hierarchy.hierarchy.filter(taxon => 
    taxon.attributes?.habitat === biome ||
    taxon.attributes?.habitat === 'any'
  );
}

// Check if a creature is a big cat for special hunting achievements
async function isBigCat(speciesTaxonId: string): Promise<boolean> {
  const response = await fetch(
    `/v1/taxonomy/taxa/${speciesTaxonId}/hierarchy?direction=up`
  );
  const ancestry = await response.json();
  
  return ancestry.hierarchy.some(taxon => 
    taxon.taxon_id === 'species-panthera'
  );
}
```

---

## Geological Features

### Example: Terrain Classification System

```typescript
const geologicalTaxonomy = [
  {
    taxon_id: "geo-root",
    domain: "geological",
    name: "Geological Features",
    rank: 0,
    parent_taxon_id: null
  },
  
  // Major feature types
  {
    taxon_id: "geo-mountains",
    domain: "geological",
    name: "Mountain Formations",
    rank: 1,
    parent_taxon_id: "geo-root",
    attributes: {
      min_elevation: 1000,
      formation_timescale: "millions_of_years",
      erosion_rate: "slow"
    }
  },
  {
    taxon_id: "geo-plains",
    domain: "geological",
    name: "Plains and Flatlands",
    rank: 1,
    parent_taxon_id: "geo-root",
    attributes: {
      elevation_range: [0, 500],
      terrain_difficulty: "easy",
      agriculture_suitability: "high"
    }
  },
  {
    taxon_id: "geo-water-features",
    domain: "geological",
    name: "Water Features",
    rank: 1,
    parent_taxon_id: "geo-root"
  },
  
  // Mountain subtypes
  {
    taxon_id: "geo-volcanic",
    domain: "geological",
    name: "Volcanic Mountains",
    rank: 2,
    parent_taxon_id: "geo-mountains",
    attributes: {
      formation_type: "volcanic",
      hazard_level: ["low", "medium", "high"],
      mineral_richness: "very_high",
      resource_types: ["obsidian", "pumice", "sulfur", "rare_earth_metals"]
    }
  },
  {
    taxon_id: "geo-fold",
    domain: "geological",
    name: "Fold Mountains",
    rank: 2,
    parent_taxon_id: "geo-mountains",
    attributes: {
      formation_type: "tectonic_collision",
      mineral_richness: "high",
      resource_types: ["coal", "iron", "copper", "gold"]
    }
  },
  
  // Volcanic subtypes
  {
    taxon_id: "geo-volcanic-active",
    domain: "geological",
    name: "Active Volcanoes",
    rank: 3,
    parent_taxon_id: "geo-volcanic",
    attributes: {
      activity_level: "active",
      hazard_level: "high",
      exploration_difficulty: "extreme",
      unique_resources: ["volcanic_glass", "heat_crystals"],
      game_mechanics: {
        periodic_events: true,
        damage_zones: true,
        special_quests: true
      }
    }
  },
  {
    taxon_id: "geo-volcanic-dormant",
    domain: "geological",
    name: "Dormant Volcanoes",
    rank: 3,
    parent_taxon_id: "geo-volcanic",
    attributes: {
      activity_level: "dormant",
      hazard_level: "low",
      cave_systems: "extensive",
      unique_resources: ["cooled_magma_veins", "geothermal_vents"]
    }
  },
  
  // Water features
  {
    taxon_id: "geo-rivers",
    domain: "geological",
    name: "Rivers and Streams",
    rank: 2,
    parent_taxon_id: "geo-water-features",
    attributes: {
      resource_types: ["freshwater", "river_sediment", "fish"],
      transportation: "boat_capable"
    }
  },
  {
    taxon_id: "geo-lakes",
    domain: "geological",
    name: "Lakes",
    rank: 2,
    parent_taxon_id: "geo-water-features",
    attributes: {
      resource_types: ["freshwater", "lake_minerals", "fish"],
      settlement_suitability: "high"
    }
  }
];
```

### Usage: Procedural Generation with Taxonomy

```typescript
// Generate appropriate resources based on geological feature
async function generateResources(location: Location) {
  // Identify geological feature at location
  const geologicalType = identifyGeologicalFeature(location);
  
  // Get taxon information
  const response = await fetch(
    `/v1/taxonomy/taxa/${geologicalType}`
  );
  const taxon = await response.json();
  
  // Generate resources based on taxon attributes
  const resources = taxon.attributes.resource_types.map(type => ({
    type: type,
    abundance: calculateAbundance(taxon, type),
    quality: calculateQuality(taxon, location)
  }));
  
  return resources;
}

// Check if location is hazardous
async function isHazardousLocation(geologicalTaxonId: string): Promise<boolean> {
  const response = await fetch(`/v1/taxonomy/taxa/${geologicalTaxonId}`);
  const taxon = await response.json();
  
  return taxon.attributes?.hazard_level === 'high' ||
         taxon.attributes?.hazard_level === 'extreme';
}
```

---

## Item Categorization

### Example: Equipment and Resource Classification

```typescript
const itemTaxonomy = [
  {
    taxon_id: "item-root",
    domain: "item",
    name: "All Items",
    rank: 0,
    parent_taxon_id: null
  },
  
  // Top-level categories
  {
    taxon_id: "item-equipment",
    domain: "item",
    name: "Equipment",
    rank: 1,
    parent_taxon_id: "item-root"
  },
  {
    taxon_id: "item-resources",
    domain: "item",
    name: "Resources",
    rank: 1,
    parent_taxon_id: "item-root"
  },
  {
    taxon_id: "item-consumables",
    domain: "item",
    name: "Consumables",
    rank: 1,
    parent_taxon_id: "item-root"
  },
  
  // Equipment subtypes
  {
    taxon_id: "item-equipment-tools",
    domain: "item",
    name: "Tools",
    rank: 2,
    parent_taxon_id: "item-equipment",
    attributes: {
      durability_system: true,
      skill_requirements: true
    }
  },
  {
    taxon_id: "item-equipment-armor",
    domain: "item",
    name: "Armor",
    rank: 2,
    parent_taxon_id: "item-equipment"
  },
  
  // Tool types
  {
    taxon_id: "item-tools-mining",
    domain: "item",
    name: "Mining Tools",
    rank: 3,
    parent_taxon_id: "item-equipment-tools",
    attributes: {
      primary_skill: "mining",
      wear_rate: "medium",
      repair_materials: ["iron_ingot", "wood"]
    }
  },
  {
    taxon_id: "item-tools-geological",
    domain: "item",
    name: "Geological Survey Tools",
    rank: 3,
    parent_taxon_id: "item-equipment-tools",
    attributes: {
      primary_skill: "geology",
      wear_rate: "low",
      specialized: true
    }
  },
  
  // Specific tools
  {
    taxon_id: "item-pickaxe",
    domain: "item",
    name: "Pickaxes",
    rank: 4,
    parent_taxon_id: "item-tools-mining",
    attributes: {
      tool_type: "pickaxe",
      tiers: ["stone", "iron", "steel", "mithril"],
      effectiveness: {
        stone: 1.0,
        ore: 2.0,
        crystal: 0.5
      }
    }
  },
  {
    taxon_id: "item-seismometer",
    domain: "item",
    name: "Seismometer",
    rank: 4,
    parent_taxon_id: "item-tools-geological",
    attributes: {
      tool_type: "measurement",
      specialization: "seismic_activity",
      rarity: "rare",
      required_skill_level: 50,
      faction_requirement: "faction-igs"
    }
  },
  
  // Resource types
  {
    taxon_id: "item-resources-ores",
    domain: "item",
    name: "Ores and Minerals",
    rank: 2,
    parent_taxon_id: "item-resources",
    attributes: {
      stackable: true,
      max_stack: 1000,
      market_category: "raw_materials"
    }
  },
  {
    taxon_id: "item-resources-gems",
    domain: "item",
    name: "Gems and Crystals",
    rank: 2,
    parent_taxon_id: "item-resources",
    attributes: {
      stackable: true,
      max_stack: 100,
      market_category: "precious_materials",
      value_multiplier: 10
    }
  }
];
```

---

## Achievement Organization

### Example: Multi-tier Achievement System

```typescript
const achievementTaxonomy = [
  {
    taxon_id: "achievement-root",
    domain: "achievement",
    name: "All Achievements",
    rank: 0,
    parent_taxon_id: null
  },
  
  {
    taxon_id: "achievement-geological",
    domain: "achievement",
    name: "Geological Achievements",
    rank: 1,
    parent_taxon_id: "achievement-root",
    attributes: {
      icon: "mountain",
      category_color: "#8B4513"
    }
  },
  
  {
    taxon_id: "achievement-geo-discovery",
    domain: "achievement",
    name: "Discovery Achievements",
    rank: 2,
    parent_taxon_id: "achievement-geological",
    attributes: {
      type: "discovery",
      rewards_type: ["experience", "reputation"]
    }
  },
  
  {
    taxon_id: "achievement-geo-mastery",
    domain: "achievement",
    name: "Geological Mastery",
    rank: 2,
    parent_taxon_id: "achievement-geological",
    attributes: {
      type: "skill_mastery",
      rewards_type: ["skill_points", "unique_tools"]
    }
  },
  
  // Progressive achievements
  {
    taxon_id: "achievement-sample-collector",
    domain: "achievement",
    name: "Sample Collector Series",
    rank: 3,
    parent_taxon_id: "achievement-geo-discovery",
    attributes: {
      tiers: [
        { name: "Novice Collector", threshold: 100, reward: "basic_storage" },
        { name: "Experienced Collector", threshold: 500, reward: "advanced_storage" },
        { name: "Master Collector", threshold: 2000, reward: "specialized_storage" }
      ]
    }
  }
];
```

---

## Quest Type Classification

### Example: Quest Taxonomy

```typescript
const questTaxonomy = [
  {
    taxon_id: "quest-root",
    domain: "quest",
    name: "All Quests",
    rank: 0,
    parent_taxon_id: null
  },
  
  {
    taxon_id: "quest-main",
    domain: "quest",
    name: "Main Story Quests",
    rank: 1,
    parent_taxon_id: "quest-root",
    attributes: {
      importance: "critical",
      repeatable: false,
      prerequisites_required: true
    }
  },
  
  {
    taxon_id: "quest-faction",
    domain: "quest",
    name: "Faction Quests",
    rank: 1,
    parent_taxon_id: "quest-root",
    attributes: {
      reputation_reward: true,
      faction_locked: true
    }
  },
  
  {
    taxon_id: "quest-faction-igs",
    domain: "quest",
    name: "IGS Quests",
    rank: 2,
    parent_taxon_id: "quest-faction",
    attributes: {
      associated_faction: "faction-igs",
      skill_focus: "geology",
      reputation_faction: "faction-igs"
    }
  },
  
  {
    taxon_id: "quest-igs-survey",
    domain: "quest",
    name: "Survey Missions",
    rank: 3,
    parent_taxon_id: "quest-faction-igs",
    attributes: {
      quest_type: "survey",
      rewards: ["geological_samples", "reputation", "currency"],
      difficulty_scaling: true
    }
  }
];
```

---

## Best Practices Summary

1. **Consistent Naming**: Use descriptive, hierarchical IDs (e.g., `faction-igs-field-arctic`)
2. **Appropriate Depth**: Keep hierarchies 3-7 levels deep for manageable navigation
3. **Rich Attributes**: Store domain-specific data in the `attributes` field
4. **Clear Rank Names**: Use meaningful rank_name values when hierarchical level matters
5. **Documentation**: Include descriptions for non-obvious taxa
6. **Relationships**: Use taxon relationships for cross-domain connections
7. **Query Optimization**: Cache frequently accessed taxonomy branches

## Performance Tips

- Cache entire domain taxonomies for frequently accessed systems (factions, items)
- Use hierarchy queries sparingly; cache results when possible
- Index custom attributes for domain-specific queries
- Consider materialized paths for very deep hierarchies
- Batch create operations when initializing new taxonomies

## Related Documentation

- [Taxonomy Classification System](taxonomy-classification-system.md) - System design
- [Taxonomy API](api-taxonomy-system.md) - API reference
- [Database Schema Design](database-schema-design.md) - Data persistence
