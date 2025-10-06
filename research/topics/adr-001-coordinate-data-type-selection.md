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
- Target performance: >1M spatial queries per second
- Storage requirement: Support 10,000-50,000 concurrent users
- Precision requirement: Millimeter-scale detail at planetary scale

### Problem Statement

**What data type should BlueMarble use for world coordinates to balance precision, performance, and scalability for a 20,000 km scale world?**

### Constraints

- Must support at least 20,000 km range in all three dimensions
- Must provide millimeter or better precision across entire world
- Must integrate with existing octree compression strategies
- Must not significantly degrade query performance (<50ms target latency)
- Must be feasible to implement in C# with Unity/Godot compatibility

### Requirements

1. **Precision**: Millimeter-scale precision at 20,000 km distance
2. **Performance**: Support >1M queries/second across distributed system
3. **Memory**: Minimize memory footprint for petabyte-scale storage
4. **Determinism**: Consistent behavior across all platforms and scenarios
5. **Integration**: Compatible with octree, Morton codes, and spatial hashing

## Options Considered

### Option 1: Float (32-bit IEEE 754)

**Description**: Use standard single-precision floating-point (float) for all coordinates.

**Pros**:
- Half the memory of double (4 bytes vs 8 bytes per component)
- Fast on all modern CPUs and GPUs
- Universal hardware support
- Standard library compatibility

**Cons**:
- **Insufficient precision**: Only ~1.19 meter precision at 20,000 km scale
- Cannot represent millimeter details at planetary scale
- Precision loss accumulates in operations
- Not suitable for primary world coordinates

**Trade-offs**:
- Memory savings don't outweigh precision loss
- Usable only for local/camera-relative coordinates

**Estimated Effort**: Low (already available)

**Verdict**: ❌ Not suitable for world coordinates

### Option 2: Double (64-bit IEEE 754)

**Description**: Use standard double-precision floating-point (double) for all coordinates.

**Pros**:
- Excellent precision: ~2.2 micrometer at 20,000 km scale
- Standard IEEE 754 format, well-understood
- Excellent tooling and debugging support
- Only 3-5% slower than fixed-point in modern CPUs
- Low implementation risk

**Cons**:
- Slightly slower than fixed-point integer operations
- 8 bytes per component (same as fixed-point)
- Floating-point edge cases (NaN, infinity, denormals)
- Less compression-friendly than integers

**Trade-offs**:
- Safety and standards vs marginal performance loss
- Lower implementation risk vs slightly worse performance

**Estimated Effort**: Low (standard library available)

**Verdict**: ✅ Acceptable fallback option

### Option 3: Fixed-Point 64-bit (40-bit integer / 24-bit fractional)

**Description**: Custom 64-bit fixed-point representation with 40 bits for integer part and 24 bits for fractional part.

**Pros**:
- **Excellent precision**: 59.6 nanometer at 20,000 km scale
- **Better performance**: 5-10% faster than double in typical operations
- **Better compression**: 8% better compression in octree storage
- **Deterministic**: No floating-point edge cases
- **Optimal for octree**: Bit-level operations for Morton encoding
- **Same memory**: 8 bytes per component (same as double)

**Cons**:
- Moderate implementation complexity
- Requires custom SIMD optimization for best performance
- Less tooling support than standard types
- Need conversion utilities for external APIs

**Trade-offs**:
- Implementation effort vs performance gains
- Custom code vs standard libraries

**Estimated Effort**: Medium (2-3 weeks for initial implementation, 1-2 weeks for SIMD optimization)

**Precision Analysis**:
```
Range: 40 bits integer = ±549,755,813 meters (±549,755 km)
  → 27× larger than required 20,000 km world
  
Precision: 24 bits fractional = 1/(2^24) ≈ 0.0000000596 meters
  → 59.6 nanometer precision (far exceeds requirements)
```

**Verdict**: ✅ **Recommended primary option**

### Option 4: Hybrid Approach (Fixed-Point World + Float Local)

**Description**: Use fixed-point 64-bit for world coordinates, float for local/chunk-relative coordinates.

**Pros**:
- Combines best of both worlds
- 25-45% memory savings for large datasets
- Excellent precision where needed
- Better cache efficiency
- Optimal GPU rendering (float for vertices)

**Cons**:
- Increased API complexity
- Need clear boundaries between coordinate spaces
- Conversion overhead at boundaries
- More code to maintain

**Trade-offs**:
- Complexity vs memory efficiency
- Performance optimization vs API simplicity

**Estimated Effort**: Medium-High (3-4 weeks for complete implementation)

**Verdict**: ✅ **Recommended enhanced strategy**

## Decision

**Chosen Option**: Option 3 + Option 4 Hybrid - **Fixed-Point 64-bit (40/24) for World Coordinates with Float for Local Offsets**

### Rationale

1. **Precision Requirement Met**: 59.6 nanometer precision far exceeds millimeter requirement
2. **Performance Optimized**: 5-10% better than double, meets >1M QPS target
3. **Storage Efficient**: Same 8 bytes as double, but better compression (+8%)
4. **Deterministic Arithmetic**: Avoids floating-point edge cases in spatial operations
5. **Octree Optimized**: Enables efficient Morton encoding and bit-level operations
6. **Scalable**: Supports hybrid strategy for 25-45% memory savings
7. **Practical Range**: ±549,755 km range (27× required) with room for expansion

**Decision Makers**:
- @copilot (Technical Analysis & Recommendation)
- Awaiting approval from BlueMarble Engineering Team

**Date Decided**: 2025-01-06 (Proposed)

## Consequences

### Positive Consequences

1. **Superior Precision**: Sub-micrometer precision ensures no quality loss at any scale
2. **Better Performance**: 5-10% improvement in spatial queries supports scalability goals
3. **Improved Compression**: 8% better octree compression reduces storage costs
4. **Deterministic Behavior**: Eliminates floating-point edge case bugs
5. **Memory Efficient**: Hybrid approach saves 25-45% memory in large datasets
6. **Future-Proof**: 27× headroom allows for larger worlds without changes

### Negative Consequences

1. **Implementation Effort**: Requires 2-3 weeks for core implementation
   - *Mitigation*: Can phase in gradually, starting with core types
2. **Testing Burden**: Need comprehensive testing of custom arithmetic
   - *Mitigation*: Develop robust test suite with edge cases
3. **API Complexity**: Hybrid approach adds coordinate space conversions
   - *Mitigation*: Provide clear API boundaries and automatic conversions
4. **External Integration**: Need conversion for third-party libraries
   - *Mitigation*: Implement efficient conversion utilities

### Neutral Consequences

1. **Learning Curve**: Team needs to understand fixed-point arithmetic
2. **Documentation**: Requires comprehensive documentation of coordinate spaces
3. **SIMD Optimization**: Optional but beneficial for maximum performance

### Impact Areas

- **Spatial Data Storage**: Direct impact on octree node storage format and compression
- **Physics System**: Need local coordinate space with float for physics simulation
- **Rendering Pipeline**: GPU uses float for vertices (camera-relative conversion)
- **Network Protocol**: May affect client-server synchronization precision
- **Database Schema**: Storage format for coordinate columns
- **API Surface**: All coordinate-handling APIs need updates

## Implementation

### Action Items

- [ ] **Phase 1: Core Type Implementation** (Week 1-2)
  - [ ] Implement FixedPoint64 struct with basic operations
  - [ ] Implement arithmetic operations (+, -, *, /)
  - [ ] Implement comparison operations (==, <, >, etc)
  - [ ] Create conversion utilities (to/from double, float)
  - [ ] Write comprehensive unit tests

- [ ] **Phase 2: Integration** (Week 3-4)
  - [ ] Update OctreeNode to use FixedPoint64
  - [ ] Implement Morton encoding with fixed-point
  - [ ] Update spatial query functions
  - [ ] Implement coordinate space conversion API
  - [ ] Integration tests with existing systems

- [ ] **Phase 3: Optimization** (Week 5-6)
  - [ ] SIMD optimizations (AVX2/AVX-512)
  - [ ] Benchmark vs double baseline
  - [ ] Profiling and performance tuning
  - [ ] Documentation and examples

- [ ] **Phase 4: Hybrid Strategy** (Week 7-8)
  - [ ] Define coordinate space hierarchy
  - [ ] Implement local float coordinate system
  - [ ] Camera-relative GPU rendering
  - [ ] Complete API documentation

### Timeline

- **Phase 1** (Week 1-2): Core fixed-point type and operations
- **Phase 2** (Week 3-4): Octree integration and spatial functions
- **Phase 3** (Week 5-6): SIMD optimization and performance validation
- **Phase 4** (Week 7-8): Hybrid coordinate system and GPU integration

**Total Duration**: 8 weeks for complete implementation

### Dependencies

- Existing octree implementation (spatial-data-storage research)
- Database schema updates for coordinate storage
- Physics engine integration for local coordinate conversion
- Rendering pipeline updates for GPU vertex data

## Review Date

**Next Review**: 2025-04-06 (3 months after implementation start)

**Review Criteria**: What conditions would trigger a review?

1. **Performance Metrics Not Met**: If <1M QPS not achieved after optimization
2. **Implementation Complexity**: If development exceeds 10 weeks total effort
3. **Precision Issues**: If any precision-related bugs discovered in production
4. **Better Alternative Found**: If industry adopts superior approach
5. **Team Feedback**: If engineering team identifies significant issues

## Related Documents

- [Coordinate Data Type Optimization Research](coordinate-data-type-optimization.md) - Full technical analysis
- [Spatial Data Storage Research](../spatial-data-storage/README.md) - Octree compression strategies
- [Hybrid Array + Octree Storage](../spatial-data-storage/hybrid-array-octree-storage-strategy.md) - Storage architecture
- [Database Schema Design](../../docs/systems/database-schema-design.md) - Database integration
- [System Architecture Design](../../docs/systems/system-architecture-design.md) - Overall system design
