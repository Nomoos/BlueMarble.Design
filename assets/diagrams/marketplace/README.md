# Marketplace System Diagrams

This directory contains technical architecture and flow diagrams for the Player Trading Marketplace.

## Contents

Planned diagrams (to be created):

- `marketplace-system-architecture-v1.png` - High-level system components and layers
- `transaction-flow-diagram-v1.png` - Transaction processing sequence diagram
- `database-schema-marketplace-v1.png` - Data model and relationships

## Diagram Descriptions

### System Architecture

```
┌─────────────────────────────────────────────────────────────┐
│                     Client Layer                             │
│  ┌──────────────┐ ┌──────────────┐ ┌────────────────────┐  │
│  │ Marketplace  │ │   Listing    │ │    Transaction     │  │
│  │  UI Component│ │   Manager    │ │     Handler        │  │
│  └──────────────┘ └──────────────┘ └────────────────────┘  │
└─────────────────────────────────────────────────────────────┘
                            │
                            ▼
┌─────────────────────────────────────────────────────────────┐
│                    API Gateway Layer                         │
│  ┌──────────────┐ ┌──────────────┐ ┌────────────────────┐  │
│  │ Marketplace  │ │ Transaction  │ │   Search & Filter  │  │
│  │  Service API │ │ Processing API│ │        API         │  │
│  └──────────────┘ └──────────────┘ └────────────────────┘  │
└─────────────────────────────────────────────────────────────┘
                            │
                            ▼
┌─────────────────────────────────────────────────────────────┐
│                  Backend Services Layer                      │
│  ┌──────────────┐ ┌──────────────┐ ┌────────────────────┐  │
│  │   Listing    │ │ Transaction  │ │   Search Service   │  │
│  │   Service    │ │   Service    │ │  (Elasticsearch)   │  │
│  ├──────────────┤ ├──────────────┤ ├────────────────────┤  │
│  │  Analytics   │ │  Reputation  │ │   Transport        │  │
│  │   Service    │ │   Service    │ │    Service         │  │
│  └──────────────┘ └──────────────┘ └────────────────────┘  │
└─────────────────────────────────────────────────────────────┘
                            │
                            ▼
┌─────────────────────────────────────────────────────────────┐
│                       Data Layer                             │
│  ┌──────────────┐ ┌──────────────┐ ┌────────────────────┐  │
│  │   Listings   │ │ Transaction  │ │   Search Index     │  │
│  │   Database   │ │     Log      │ │ (Elasticsearch)    │  │
│  │ (PostgreSQL) │ │ (Time-series)│ ├────────────────────┤  │
│  ├──────────────┤ ├──────────────┤ │   Cache Layer      │  │
│  │  Analytics   │ │              │ │     (Redis)        │  │
│  │  Warehouse   │ │              │ │                    │  │
│  └──────────────┘ └──────────────┘ └────────────────────┘  │
└─────────────────────────────────────────────────────────────┘
```

### Transaction Flow

```
Buyer                Transaction Service        Database          Seller
  │                         │                      │               │
  │──Purchase Request──────▶│                      │               │
  │                         │──Validate Buyer──────▶│               │
  │                         │◀─────Currency OK──────│               │
  │                         │──Lock Listing─────────▶│               │
  │                         │◀────Listing Locked────│               │
  │                         │──Deduct Currency──────▶│               │
  │                         │──Transfer Items───────▶│               │
  │                         │──Credit Seller────────▶│               │
  │◀──Transaction Success───│                      │               │
  │                         │──Notify Seller────────┼──────────────▶│
  │                         │──Update Analytics─────▶│               │
```

## Format

- ASCII diagrams for inline documentation (shown above)
- PNG exports for presentations and external documentation
- Source files in draw.io/diagrams.net format for editing

## Status

- [x] ASCII architecture diagrams documented
- [ ] Visual diagrams pending creation
- [ ] Database schema diagram in progress
- [ ] Sequence diagrams for edge cases needed

## Tools

- Draw.io / Diagrams.net for system architecture
- PlantUML for sequence diagrams
- DBDiagram.io for database schema

## Related Documentation

- [Feature Specification](../../design/spec-player-trading-marketplace.md)
- [Technical Architecture Docs](../../docs/systems/)
