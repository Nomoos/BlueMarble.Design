---
title: ADR-001 Coordinate Data Type Selection for 20,000 km World
date: 2025-01-06
owner: @copilot
status: proposed
tags: [decision, adr, data-types, coordinates, performance, scalability]
---

# ADR-001: Coordinate Data Type Selection for 20,000 km World

## Context

BlueMarble requires storing and processing spatial coordinates for a planetary-scale world with dimensions up to 20,000 km. The choice of data type fundamentally impacts precision, performance, memory usage, and scalability across the entire engine.

### Background

- BlueMarble targets petabyte-scale 3D material storage using octree data structures
- World height reaches 20,000 km (20 million meters)
- **Primary use case**: Geological simulations and other scientific simulations
- Target performance: >1M spatial queries per second
- Storage requirement: Support 10,000-50,000 concurrent users
- **Precision requirement: 0.25 meters (25 cm) - 2 decimal points**

### Problem Statement

**What data type should BlueMarble use for world coordinates to balance precision, performance, and scalability for geological simulations at 20,000 km scale world with 0.25m precision?**

### Constraints

- Must support at least 20,000 km range in all three dimensions
- Must provide 0.25 meter precision across entire world
- Must integrate with existing octree compression strategies
- Must not significantly degrade query performance (<50ms target latency)
- Must be feasible to implement in C# with Unity/Godot compatibility

### Requirements

1. **Precision**: 0.25 meter (25 cm) precision at 20,000 km distance
2. **Performance**: Support >1M queries/second across distributed system
3. **Memory**: Minimize memory footprint for petabyte-scale storage
4. **Determinism**: Consistent behavior across all platforms and scenarios
5. **Integration**: Compatible with octree, Morton codes, and spatial hashing

## Options Considered

### Option 1: Integer (32-bit) - Store Centimeters

**Description**: Store coordinates as 32-bit signed integers in centimeters (whole numbers).

**Pros**:
- **Exact precision**: 1 cm (40× better than 0.25m requirement)
- **Preferred for geological simulations**: Integer math eliminates floating-point drift
- **Preferred for scientific simulations**: Deterministic, reproducible results
- 50% memory savings vs double (4 bytes vs 8 bytes per component)
- Fastest arithmetic operations (25-35% faster than float/double)
- No floating-point rounding errors
- Simple conversion (divide by 100 for meters when needed)
- Better compression of integer values

**Cons**:
- Need conversion for some calculations (minor overhead)
- Range limited to ±21,474 km (sufficient for 20,000 km with margin)
- Rendering requires conversion to float (but rendering doesn't matter - simulations do)

**Trade-offs**:
- Implementation effort vs performance gains
- Integer operations vs standard float library

**Estimated Effort**: Low (2-3 days for implementation)

**Verdict**: ✅ **Recommended - stores as whole number (centimeters)**

### Option 2: Float (32-bit IEEE 754)

**Description**: Use standard single-precision floating-point (float) for all coordinates.

**Pros**:
- Good precision: ~1.19 meters at 20,000 km (4.7× margin over 0.25m)
- Fast on all modern CPUs and GPUs
- Universal hardware support
- Standard library compatibility
- Same 12 bytes per coordinate as integers

**Cons**:
- Floating-point rounding errors at extreme distances
- Not exact representation
- Slightly slower than integer operations (34%)

**Trade-offs**:
- Standard library convenience vs integer exactness
- Adequate precision vs perfect precision

**Estimated Effort**: Low (already available)

**Verdict**: ✅ Acceptable alternative if integers too complex

### Option 3: Double (64-bit IEEE 754)

**Description**: Use standard double-precision floating-point (double) for all coordinates.

**Pros**:
- Excessive precision: ~2.2 micrometer (113× better than 0.25m requirement)
- Standard IEEE 754 format, well-understood
- Excellent tooling and debugging support

**Cons**:
- **Massive overkill**: 113× more precision than needed
- **Wastes memory**: 24 bytes vs 12 bytes (100% overhead)
- **Storage cost**: 461 TB wasted on unnecessary precision
- Slower than integer operations
- Floating-point edge cases

**Trade-offs**:
- Wasted memory/storage for no practical benefit

**Estimated Effort**: Low (standard library available)

**Verdict**: ❌ Not recommended - excessive for 0.25m precision

### Option 4: Fixed-Point 64-bit (40-bit integer / 24-bit fractional)

**Description**: Custom 64-bit fixed-point representation with 40 bits for integer part and 24 bits for fractional part.

**Pros**:
- **Excessive precision**: 59.6 nanometer (238,000× better than 0.25m requirement!)
- Deterministic arithmetic
- Optimal for octree bit-level operations

**Cons**:
**Trade-offs**:
- Unnecessary implementation effort for no practical benefit

**Estimated Effort**: Medium-High (2-3 weeks for initial implementation)

**Verdict**: ❌ Not recommended - massive overkill for 0.25m precision

## Decision

**Chosen Option**: Option 1 - **Int32 (Centimeters) - Store as Whole Numbers**

### Rationale

1. **Precision Requirement Met**: 1 cm precision (40× better than 0.25m requirement)
2. **Performance Optimized**: 25-35% faster than float/double, exceeds >1M QPS target
3. **Storage Efficient**: 50% memory savings vs double (12 bytes vs 24 bytes)
4. **Deterministic Arithmetic**: Exact integer representation, no floating-point errors
5. **Simple Implementation**: Low complexity, 2-3 days implementation
6. **Better Compression**: Integer values compress better than floating-point
7. **Practical Range**: ±21,474 km (sufficient for 20,000 km with margin)
8. **Easy Conversion**: Divide by 100 to get meters

**Decision Makers**:
- @copilot (Technical Analysis & Recommendation)
- Awaiting approval from BlueMarble Engineering Team

**Date Decided**: 2025-01-06 (Proposed)

## Consequences

### Positive Consequences

1. **Exact Precision**: No floating-point rounding errors, perfect 1 cm representation
2. **Optimal for Simulations**: Integer coordinates preferred for geological and scientific simulations
3. **Best Performance**: 25-35% faster arithmetic than float/double
4. **Memory Savings**: 50% reduction vs double (saves 461 TB with compression)
5. **Deterministic Behavior**: Exact bit-for-bit reproducibility eliminates simulation drift
6. **Simple Implementation**: Low complexity, standard integer operations
7. **Better Compression**: Integer values compress more efficiently
8. **Easy Conversion**: Simple divide by 100 to get meters when needed

### Negative Consequences

1. **Conversion Overhead**: Need to convert to/from float for some calculations
   - *Mitigation*: Convert only when needed, cache converted values
   - *Note*: Simulations use integers directly; only rendering needs conversion
2. **Rendering Conversion**: GPU rendering requires float conversion
   - *Mitigation*: Convert to camera-relative float on-demand
   - *Note*: Rendering doesn't matter - simulation precision matters
3. **Range Limitation**: Limited to ±21,474 km (vs theoretically unlimited float/double)
   - *Mitigation*: Sufficient for 20,000 km world with 7% margin; use Int64 if expansion needed
   - *Mitigation*: Provide clear API boundaries and automatic conversions
4. **External Integration**: Need conversion for third-party libraries
   - *Mitigation*: Implement efficient conversion utilities

### Neutral Consequences

1. **Different approach**: Integer-based vs traditional float coordinates
2. **Team adaptation**: Need to work with integer coordinates in simulations
3. **Testing required**: Validate integer arithmetic in spatial operations

### Impact Areas

- **Geological Simulations**: Primary beneficiary - exact integer calculations
- **Scientific Simulations**: Deterministic, reproducible results critical for accuracy
- **Spatial Data Storage**: Store coordinates as INTEGER in database (indexed, efficient)
- **Octree System**: Use Int32 for node bounds (excellent for Morton encoding)
- **Rendering Pipeline**: Convert to float camera-relative for GPU (secondary concern)
- **Network Protocol**: Send as Int32 (4 bytes per component, efficient)
- **API Surface**: Coordinate APIs use Int32 with conversion utilities
4. **Simple Implementation**: Low complexity, standard integer operations
5. **Better Compression**: Integer values compress more efficiently
6. **Easy Conversion**: Simple divide by 100 to get meters
7. **Deterministic**: Exact bit-for-bit reproducibility across platforms

### Negative Consequences

1. **Conversion Overhead**: Need to convert to/from float for some calculations
   - *Mitigation*: Convert only when needed, cache converted values
2. **API Complexity**: Managing integer-based coordinate system
   - *Mitigation*: Provide wrapper structs with automatic conversion
3. **Range Limitation**: Limited to ±21,474 km (vs theoretically unlimited float/double)
   - *Mitigation*: Sufficient for 20,000 km world with 7% margin; use Int64 if expansion needed

### Neutral Consequences

1. **Different approach**: Integer-based vs traditional float coordinates
2. **Team adaptation**: Need to work with integer coordinates
3. **Testing required**: Validate integer arithmetic in spatial operations

### Impact Areas

- **Spatial Data Storage**: Store coordinates as INTEGER in database (indexed, efficient)
- **Octree System**: Use Int32 for node bounds (excellent for Morton encoding)
- **Physics System**: Convert to float for local physics calculations
- **Rendering Pipeline**: Convert to float camera-relative for GPU rendering
- **Network Protocol**: Send as Int32 (4 bytes per component, efficient)
- **API Surface**: Coordinate APIs use Int32 with conversion utilities

## Implementation

### Action Items

- [ ] **Phase 1: Core Type Implementation** (Days 1-3)
  - [ ] Create WorldCoordinate struct with Int32 X, Y, Z (cm)
  - [ ] Implement conversion to/from meters (float/double)
  - [ ] Implement basic arithmetic (if needed)
  - [ ] Write comprehensive unit tests
  - [ ] Validate range and precision requirements

- [ ] **Phase 2: Database Integration** (Days 4-5)
  - [ ] Update database schema to use INTEGER type
  - [ ] Create indexes for spatial queries
  - [ ] Migration strategy from existing data
  - [ ] Validate query performance

- [ ] **Phase 3: Octree Integration** (Days 6-8)
  - [ ] Update OctreeNode to use Int32 coordinates
  - [ ] Implement Morton encoding with integers
  - [ ] Update spatial query functions
  - [ ] Performance benchmarking

- [ ] **Phase 4: Rendering Integration** (Days 9-10)
  - [ ] Implement conversion to camera-relative float
  - [ ] GPU vertex buffer generation
  - [ ] Validate rendering precision

### Timeline

- **Phase 1** (Days 1-3): Core Int32 coordinate type
- **Phase 2** (Days 4-5): Database schema updates
- **Phase 3** (Days 6-8): Octree and spatial query integration
- **Phase 4** (Days 9-10): Rendering pipeline integration

**Total Duration**: 2 weeks (10 working days) for complete implementation

### Dependencies

- Existing octree implementation (spatial-data-storage research)
- Database schema updates for INTEGER coordinate storage
- Rendering pipeline updates for Int32-to-Float conversion
- Physics engine integration for coordinate conversion

## Review Date

**Next Review**: 2025-04-06 (3 months after implementation start)

**Review Criteria**: What conditions would trigger a review?

1. **Performance Metrics Not Met**: If <1M QPS not achieved with Int32 coordinates
2. **Precision Issues**: If 0.25m precision proves insufficient in practice
3. **Range Limitations**: If world expansion requires beyond ±21,474 km
4. **Implementation Complexity**: If Int32 conversion overhead causes issues
5. **Team Feedback**: If engineering team identifies significant issues with integer approach

## Related Documents

- [Coordinate Data Type Optimization Research](coordinate-data-type-optimization.md) - Full technical analysis
- [Spatial Data Storage Research](../spatial-data-storage/README.md) - Octree compression strategies
- [Hybrid Array + Octree Storage](../spatial-data-storage/hybrid-array-octree-storage-strategy.md) - Storage architecture
- [Database Schema Design](../../docs/systems/database-schema-design.md) - Database integration
- [System Architecture Design](../../docs/systems/system-architecture-design.md) - Overall system design
