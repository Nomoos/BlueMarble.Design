# Discovery Sources - Game Economy Design References

**Date**: 2025-01-18  
**Status**: Catalogued from external document  
**Purpose**: Track source materials referenced in game economy design

---

## Overview

The external game economy design document references multiple source materials using citation format 【N†LX-Y】, where:
- **N** = Source document number
- **X-Y** = Line range in source document

This document catalogs these references for future investigation and integration.

---

## Identified Sources

### Source 39: Auction Tier Structures
**Referenced Lines**: L149-L158, L169-L177

**Topic**: Auction tier fee structures and commission rates

**References in Document**:
- Fee ranges for different market tiers (1-3% local, 6-8% global)
- Tier parameters and visibility mechanics

**Investigation Priority**: HIGH - Core to auction system design

**Related Implementation**:
- Current system uses Local (1.5%), Regional (3%), Global (7%)
- Closely aligned with described structure

---

### Source 58: Transport Mechanics
**Referenced Lines**: L563-L572, L590-L599, L613-L620, L621-L629, L645-L653

**Topic**: Comprehensive transport system with multiple methods and risk calculation

**References in Document**:
1. **L563-L572**: Transport method attributes (capacity, speed, cost, risk)
2. **L590-L599**: Cargo ship example (high capacity, moderate cost, medium risk)
3. **L613-L620**: Time and cost calculation formulas
4. **L621-L629**: Weather and terrain modifiers
5. **L645-L653**: Loss risk calculation formula

**Investigation Priority**: HIGH - Critical for transport system expansion

**Related Implementation**:
- Basic distance-based transport fees implemented
- Guard tiers (None to Premium) implemented
- Missing: Multiple vehicle types, weather/terrain modifiers

**Key Formulas Extracted**:
```
time = distance / speed
cost = fuel_cost + port_fees
risk = base_risk × (1 + distance/10000) × (1 + terrain_difficulty)
```

---

### Source 59: Auction System Scope
**Referenced Lines**: L29-L33

**Topic**: Overall auction system design philosophy and scope

**References in Document**:
- Hybrid player-driven market concept
- Regional auction structure
- Physical transport requirements

**Investigation Priority**: MEDIUM - High-level design principles

**Related Implementation**:
- Hybrid system implemented (local to global)
- Physical transport concept via fees and time

---

### Source 60: Seasonal Effects
**Referenced Lines**: L857-L861, L877-L881, L877-L885

**Topic**: Seasonal cycles affecting supply, demand, and trade routes

**References in Document**:
1. **L857-L861**: Drought conditions (×2 price, 50% yield cut)
2. **L877-L881**: Summer harvests (+80% supply, 60% price drop)
3. **L877-L885**: Route availability changes (frozen rivers, flooding)

**Investigation Priority**: HIGH - Major system enhancement opportunity

**Related Implementation**:
- Seasonal decay modifiers implemented (summer 1.3x, winter 0.5x)
- Missing: Supply/demand price changes, route restrictions

**Seasonal Data to Implement**:
```javascript
summer: {
    cropSupply: 1.8,        // +80%
    grainPrice: 0.6,        // 60% of base
    fuelDemand: 0.8         // Reduced heating
}
drought: {
    priceMultiplier: 2.0,   // ×2 prices
    yieldReduction: 0.5,    // 50% cuts
    affectedGoods: ['grain', 'vegetables', 'fruits']
}
```

---

### Source 61: Winter Preservation
**Referenced Lines**: L1-L4

**Topic**: Natural cold storage effects in winter

**References in Document**:
- Snow/cold preserves food naturally
- Seasonal preservation mechanics

**Investigation Priority**: MEDIUM - Enhancement to existing system

**Related Implementation**:
- Winter deterioration reduction (0.5x) implemented
- Could expand with cold storage facilities

---

### Source 62: Regional Specialization
**Referenced Lines**: L1172-L1181

**Topic**: Resource-rich areas and regional market differentiation

**References in Document**:
- Local goods cheaper in production regions
- Scarce resources fetch premiums elsewhere
- Major cities as trading hubs

**Investigation Priority**: MEDIUM - Aligns with current market pairs

**Related Implementation**:
- Market pairs with different volumes/spreads
- Regional specialization concept present
- Could enhance with explicit resource/scarcity mapping

---

### Source 63: Commission Rates
**Referenced Lines**: L128-L132

**Topic**: Percentage fees taken by system from sales

**References in Document**:
- Commission rate mechanics
- Fee structure details

**Investigation Priority**: LOW - Already implemented

**Related Implementation**:
- Auctioneer fees implemented across all tiers
- Commission concept integrated into cost calculation

---

## Source Document Types

Based on line ranges and content, these sources appear to be:

1. **Game Design Documents** (Sources 39, 59, 63)
   - Design specifications
   - System parameters
   - Economic balance data

2. **Technical Documentation** (Source 58)
   - Implementation details
   - Mathematical formulas
   - System mechanics

3. **Feature Specifications** (Sources 60, 61, 62)
   - Seasonal systems
   - Regional mechanics
   - Environmental effects

---

## Investigation Priorities

### Priority 1: High-Impact Sources
- **Source 58** (Transport Mechanics) - Lines 563-653
- **Source 60** (Seasonal Effects) - Lines 857-885
- **Source 39** (Auction Tiers) - Lines 149-177

**Rationale**: Core systems with significant gameplay impact

### Priority 2: Enhancement Sources
- **Source 62** (Regional Specialization) - Lines 1172-1181
- **Source 61** (Winter Preservation) - Lines 1-4
- **Source 59** (Auction Scope) - Lines 29-33

**Rationale**: Expand existing systems with richer mechanics

### Priority 3: Implemented Sources
- **Source 63** (Commission Rates) - Lines 128-132

**Rationale**: Already implemented, review for completeness only

---

## Integration Roadmap

### Phase 1: Source Acquisition
1. Request access to source documents from project leads
2. Identify if sources are internal or external references
3. Determine document formats and locations

### Phase 2: Detailed Analysis
1. Extract full content from high-priority sources
2. Map formulas and systems to implementation requirements
3. Identify conflicts or inconsistencies with current design

### Phase 3: Implementation Planning
1. Create feature specifications from source data
2. Prioritize based on impact and effort
3. Design integration approach for new mechanics

### Phase 4: Integration
1. Implement missing transport mechanics (Source 58)
2. Expand seasonal system (Source 60)
3. Enhance regional specialization (Source 62)

---

## Questions for Source Owners

1. **Source Format**: Are these documents markdown, PDF, or other format?
2. **Accessibility**: Where are the source documents stored?
3. **Version Control**: Are these sources versioned? What are the latest versions?
4. **Authorship**: Who authored each source document?
5. **Status**: Which sources are approved vs. draft vs. deprecated?
6. **Dependencies**: Are there other sources not referenced in this document?

---

## Next Steps

1. ✅ Catalog all source references from external document
2. ✅ Prioritize sources by implementation impact
3. ⏳ Request access to high-priority source documents
4. ⏳ Extract detailed specifications from sources
5. ⏳ Create feature specifications for missing mechanics
6. ⏳ Implement priority enhancements

---

## Related Documents

- `game-economy-design-external.md` - Processed external document
- `extended-auction-system-transport-fees-deterioration.md` - Current implementation
- `multi-step-commodity-swap-routing.md` - Routing system
- `auction-house-systems-local-global-transport.md` - Auction research

---

## Notes

The citation format 【N†LX-Y】 suggests these sources may be:
- Academic or research papers
- Internal design documents with specific line numbering
- Previously catalogued materials with reference system

The consistent numbering (39, 58, 59, 60, 61, 62, 63) suggests these are part of a larger collection, possibly with missing numbers (40-57) not referenced in this particular document.

---

## License

Part of the BlueMarble.Design research repository.  
All rights reserved.
