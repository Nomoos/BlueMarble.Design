# Taxonomy System Architecture Diagram

**Document Type:** Architecture Visualization  
**Version:** 1.0  
**Date:** 2025-01-19

## System Architecture Overview

This document provides visual representations of the Taxonomy Classification System architecture.

## High-Level Architecture

```mermaid
graph TB
    subgraph "Application Layer"
        API[Taxonomy API]
        GameSystems[Game Systems]
    end
    
    subgraph "Taxonomy Core"
        TaxonService[Taxon Service]
        HierarchyEngine[Hierarchy Engine]
        SearchEngine[Search Engine]
    end
    
    subgraph "Data Layer"
        TaxaTable[(Taxa Table)]
        RelationshipsTable[(Taxon Relationships)]
        SynonymsTable[(Taxon Synonyms)]
        Cache[(Redis Cache)]
    end
    
    API --> TaxonService
    GameSystems --> TaxonService
    TaxonService --> HierarchyEngine
    TaxonService --> SearchEngine
    HierarchyEngine --> TaxaTable
    SearchEngine --> TaxaTable
    TaxonService --> RelationshipsTable
    TaxonService --> SynonymsTable
    TaxonService --> Cache
```

## Taxonomy Domains

```mermaid
graph LR
    Root[Taxonomy System]
    
    Root --> Faction[Faction Domain]
    Root --> Species[Species Domain]
    Root --> Geological[Geological Domain]
    Root --> Item[Item Domain]
    Root --> Quest[Quest Domain]
    Root --> Achievement[Achievement Domain]
    
    Faction --> F1[Major Factions]
    Faction --> F2[Minor Factions]
    Faction --> F3[Guilds]
    
    Species --> S1[Animalia]
    Species --> S2[Plantae]
    
    Geological --> G1[Mountains]
    Geological --> G2[Plains]
    Geological --> G3[Water Features]
    
    Item --> I1[Equipment]
    Item --> I2[Resources]
    Item --> I3[Consumables]
    
    Quest --> Q1[Main Quests]
    Quest --> Q2[Side Quests]
    Quest --> Q3[Faction Quests]
    
    Achievement --> A1[Combat]
    Achievement --> A2[Exploration]
    Achievement --> A3[Social]
```

## Faction Hierarchy Example

```mermaid
graph TD
    Root[faction-root: All Factions<br/>rank: 0]
    
    Root --> Major[faction-major: Major Factions<br/>rank: 1]
    Root --> Minor[faction-minor: Minor Factions<br/>rank: 1]
    Root --> Guild[faction-guild: Professional Guilds<br/>rank: 1]
    
    Guild --> IGS[faction-igs: International Geological Survey<br/>rank: 2]
    Guild --> IMS[faction-ims: International Mining Syndicate<br/>rank: 2]
    
    IGS --> Field[faction-igs-field: Field Survey Division<br/>rank: 3]
    IGS --> Research[faction-igs-research: Research Division<br/>rank: 3]
    IGS --> Education[faction-igs-education: Education Division<br/>rank: 3]
    
    Field --> Arctic[faction-igs-field-arctic: Arctic Survey Team<br/>rank: 4]
    Field --> Volcanic[faction-igs-field-volcanic: Volcanic Monitoring Team<br/>rank: 4]
    Field --> Coastal[faction-igs-field-coastal: Coastal Survey Team<br/>rank: 4]
    
    style Root fill:#e1f5ff
    style Guild fill:#fff4e1
    style IGS fill:#ffe1e1
    style Field fill:#e1ffe1
    style Arctic fill:#ffe1ff
```

## Species Taxonomy Example

```mermaid
graph TD
    Kingdom[species-animalia: Animalia<br/>rank: 0, rank_name: Kingdom]
    
    Kingdom --> Chordata[species-chordata: Chordata<br/>rank: 1, rank_name: Phylum]
    Kingdom --> Arthropoda[species-arthropoda: Arthropoda<br/>rank: 1, rank_name: Phylum]
    
    Chordata --> Mammalia[species-mammalia: Mammalia<br/>rank: 2, rank_name: Class]
    Chordata --> Aves[species-aves: Aves<br/>rank: 2, rank_name: Class]
    
    Mammalia --> Carnivora[species-carnivora: Carnivora<br/>rank: 3, rank_name: Order]
    Mammalia --> Rodentia[species-rodentia: Rodentia<br/>rank: 3, rank_name: Order]
    
    Carnivora --> Felidae[species-felidae: Felidae<br/>rank: 4, rank_name: Family]
    Carnivora --> Canidae[species-canidae: Canidae<br/>rank: 4, rank_name: Family]
    
    Felidae --> Panthera[species-panthera: Panthera<br/>rank: 5, rank_name: Genus]
    
    Panthera --> Leo[species-panthera-leo: Panthera leo<br/>rank: 6, rank_name: Species<br/>common_name: Lion]
    Panthera --> Tigris[species-panthera-tigris: Panthera tigris<br/>rank: 6, rank_name: Species<br/>common_name: Tiger]
    
    style Kingdom fill:#e1f5ff
    style Chordata fill:#fff4e1
    style Mammalia fill:#ffe1e1
    style Carnivora fill:#e1ffe1
    style Felidae fill:#ffe1ff
    style Panthera fill:#f5e1ff
    style Leo fill:#ffffe1
```

## Data Model Relationships

```mermaid
erDiagram
    TAXA ||--o{ TAXA : "parent_taxon_id"
    TAXA ||--o{ TAXON_RELATIONSHIPS : "source_taxon_id"
    TAXA ||--o{ TAXON_RELATIONSHIPS : "target_taxon_id"
    TAXA ||--o{ TAXON_SYNONYMS : "taxon_id"
    TAXA ||--o{ CHARACTER_FACTION_REPUTATION : "taxon_id"
    TAXA ||--o{ ACHIEVEMENTS_MASTER : "category_taxon_id"
    
    TAXA {
        string taxon_id PK
        string domain
        string name
        string scientific_name
        string parent_taxon_id FK
        int rank
        string rank_name
        text description
        jsonb attributes
        boolean is_deprecated
        timestamp created_at
        timestamp updated_at
    }
    
    TAXON_RELATIONSHIPS {
        string source_taxon_id PK,FK
        string target_taxon_id PK,FK
        string relationship_type PK
        decimal strength
        jsonb metadata
        timestamp created_at
    }
    
    TAXON_SYNONYMS {
        string taxon_id PK,FK
        string synonym PK
        string language PK
        boolean is_preferred
    }
```

## API Request Flow

```mermaid
sequenceDiagram
    participant Client
    participant API as Taxonomy API
    participant Service as Taxon Service
    participant Cache as Redis Cache
    participant DB as PostgreSQL
    
    Client->>API: GET /v1/taxonomy/taxa/{id}/hierarchy
    API->>Service: getHierarchy(taxonId)
    Service->>Cache: check cache
    
    alt Cache Hit
        Cache-->>Service: return cached hierarchy
    else Cache Miss
        Service->>DB: SELECT with recursive CTE
        DB-->>Service: hierarchy data
        Service->>Cache: store in cache
    end
    
    Service-->>API: hierarchy response
    API-->>Client: JSON response
```

## Query Patterns

### Ancestor Query Flow

```mermaid
graph TD
    Start[Start: taxon_id = 'faction-igs-field-arctic']
    Query[Recursive CTE Query]
    Step1[Step 1: Find taxon_id]
    Step2[Step 2: Find parent]
    Step3[Step 3: Find parent's parent]
    Step4[Step 4: Continue until root]
    Result[Result: Complete ancestry path]
    
    Start --> Query
    Query --> Step1
    Step1 --> Step2
    Step2 --> Step3
    Step3 --> Step4
    Step4 --> Result
    
    Result --> Display[Display:<br/>faction-root rank 0<br/>faction-guild rank 1<br/>faction-igs rank 2<br/>faction-igs-field rank 3<br/>faction-igs-field-arctic rank 4]
```

### Descendant Query Flow

```mermaid
graph TD
    Start[Start: taxon_id = 'faction-igs']
    Query[Recursive CTE Query]
    Level1[Level 1: Direct children]
    Level2[Level 2: Grandchildren]
    Level3[Level 3: Great-grandchildren]
    Filter[Filter by max_depth if specified]
    Result[Result: Complete subtree]
    
    Start --> Query
    Query --> Level1
    Level1 --> Level2
    Level2 --> Level3
    Level3 --> Filter
    Filter --> Result
    
    Result --> Display[Display:<br/>faction-igs-field rank 3<br/>faction-igs-research rank 3<br/>faction-igs-education rank 3<br/>faction-igs-field-arctic rank 4<br/>faction-igs-field-volcanic rank 4<br/>...]
```

## Integration with Game Systems

```mermaid
graph LR
    subgraph "Game Systems"
        FactionSys[Faction System]
        AchieveSys[Achievement System]
        ItemSys[Item System]
        QuestSys[Quest System]
    end
    
    subgraph "Taxonomy System"
        TaxonAPI[Taxon API]
        TaxonDB[(Taxa Database)]
    end
    
    subgraph "Player Data"
        PlayerFaction[Player Faction Rep]
        PlayerAchieve[Player Achievements]
        PlayerItems[Player Inventory]
        PlayerQuests[Player Quests]
    end
    
    FactionSys --> TaxonAPI
    AchieveSys --> TaxonAPI
    ItemSys --> TaxonAPI
    QuestSys --> TaxonAPI
    
    TaxonAPI --> TaxonDB
    
    TaxonDB --> PlayerFaction
    TaxonDB --> PlayerAchieve
    TaxonDB --> PlayerItems
    TaxonDB --> PlayerQuests
```

## Migration Strategy

```mermaid
graph LR
    subgraph "Phase 1: Setup"
        Create[Create Taxa Tables]
        Index[Create Indexes]
    end
    
    subgraph "Phase 2: Data Migration"
        Root[Create Root Taxa]
        Categories[Create Categories]
        Migrate[Migrate Existing Data]
    end
    
    subgraph "Phase 3: Application Update"
        UpdateCode[Update Application Code]
        UpdateAPI[Deploy New API]
        UpdateDB[Add Foreign Keys]
    end
    
    subgraph "Phase 4: Validation"
        Test[Test Queries]
        Verify[Verify Data Integrity]
        Monitor[Monitor Performance]
    end
    
    subgraph "Phase 5: Cleanup"
        Deprecate[Deprecate Old Tables]
        Document[Update Documentation]
    end
    
    Create --> Index
    Index --> Root
    Root --> Categories
    Categories --> Migrate
    Migrate --> UpdateCode
    UpdateCode --> UpdateAPI
    UpdateAPI --> UpdateDB
    UpdateDB --> Test
    Test --> Verify
    Verify --> Monitor
    Monitor --> Deprecate
    Deprecate --> Document
```

## Performance Optimization

```mermaid
graph TD
    Request[Client Request]
    
    Request --> Cache{Cache Hit?}
    Cache -->|Yes| Return1[Return Cached Data]
    Cache -->|No| CheckMV{Materialized View?}
    
    CheckMV -->|Yes| QueryMV[Query Materialized View]
    CheckMV -->|No| QueryDB[Query Database with CTE]
    
    QueryMV --> Store1[Store in Cache]
    QueryDB --> Store2[Store in Cache]
    
    Store1 --> Return2[Return Data]
    Store2 --> Return2
    
    style Cache fill:#e1f5ff
    style CheckMV fill:#fff4e1
    style Return1 fill:#e1ffe1
```

## Taxon Attributes Structure

```mermaid
graph TD
    Taxon[Taxon Object]
    
    Taxon --> Identity[Identity]
    Taxon --> Hierarchy[Hierarchy]
    Taxon --> Metadata[Metadata]
    Taxon --> Tracking[Tracking]
    
    Identity --> ID[taxon_id: string]
    Identity --> Domain[domain: enum]
    Identity --> Name[name: string]
    Identity --> Scientific[scientific_name: string]
    
    Hierarchy --> Parent[parent_taxon_id: string]
    Hierarchy --> Rank[rank: number]
    Hierarchy --> RankName[rank_name: string]
    
    Metadata --> Desc[description: text]
    Metadata --> Attrs[attributes: JSONB]
    Metadata --> Deprecated[is_deprecated: boolean]
    
    Tracking --> Created[created_at: timestamp]
    Tracking --> Updated[updated_at: timestamp]
    
    style Identity fill:#e1f5ff
    style Hierarchy fill:#fff4e1
    style Metadata fill:#ffe1e1
    style Tracking fill:#e1ffe1
```

## Use Case: Player Joining Faction

```mermaid
sequenceDiagram
    participant Player
    participant GameClient
    participant FactionSystem
    participant TaxonomyAPI
    participant Database
    
    Player->>GameClient: Request to join faction
    GameClient->>FactionSystem: joinFaction(playerId, factionId)
    
    FactionSystem->>TaxonomyAPI: GET /v1/taxonomy/taxa/{factionId}
    TaxonomyAPI->>Database: SELECT from taxa WHERE taxon_id = ?
    Database-->>TaxonomyAPI: faction taxon data
    TaxonomyAPI-->>FactionSystem: faction details
    
    FactionSystem->>FactionSystem: Check requirements
    
    alt Requirements Met
        FactionSystem->>Database: INSERT into character_faction_reputation
        FactionSystem->>TaxonomyAPI: GET /v1/taxonomy/taxa/{factionId}/hierarchy
        TaxonomyAPI-->>FactionSystem: faction hierarchy
        FactionSystem->>GameClient: Success + faction info
        GameClient->>Player: Welcome to faction!
    else Requirements Not Met
        FactionSystem->>GameClient: Failure + missing requirements
        GameClient->>Player: Cannot join faction
    end
```

## Summary

This architecture provides:

- **Flexibility**: Domain-agnostic taxonomy system
- **Scalability**: Efficient hierarchical queries with caching
- **Extensibility**: New domains and taxa without schema changes
- **Performance**: Optimized with indexes, caching, and materialized views
- **Integration**: Clean API for all game systems

## Related Documentation

- [Taxonomy Classification System](taxonomy-classification-system.md) - Core design
- [Taxonomy API](api-taxonomy-system.md) - API specification
- [Taxonomy Usage Examples](taxonomy-usage-examples.md) - Practical examples
- [Taxonomy Migration Guide](taxonomy-migration-guide.md) - Migration process
