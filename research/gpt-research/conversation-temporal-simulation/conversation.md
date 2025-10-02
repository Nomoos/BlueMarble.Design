# Temporal Simulation & Event Propagation in Planet-Scale Worlds

**Document Type**: Technical Research Paper  
**Date**: 2025-10-02  
**Source**: Provided PDF  
**Related Issue**: #101

## Overview

This document addresses the challenge of modeling a planet-sized game world with multi-scale time management and intelligent event handling. It balances fine-grained simulation (individual NPCs, local weather) with coarse-grained processes (climate systems, geopolitical changes) while remaining performant and responsive.

## Key Concepts

### 1. Hierarchical Simulation

**Multi-Scale Ticks**:
- Fast loop (seconds/minutes) for local interactions
- Slower "macro tick" (hours/days) for world-level processes
- Level-of-Detail (LOD) simulation approach
- Objects in view run full simulation
- Distant objects run simplified simulation periodically

**Spatial Partitioning**:
- Grid or quadtree/octree-based regions
- Each region advances on its own schedule
- Sync with neighbors only when events span regions
- Hierarchical structures group nearby tiles into "sectors"

### 2. Event-Driven Simulation

**Discrete Event Approach**:
- Most of world is idle until something happens
- Entities schedule future actions
- Global dispatcher (priority queue) stores events by timestamp
- When clock reaches time, event fires and updates world
- Idle regions generate no load until triggered

**Event Management**:
- Track scheduled actions per entity
- Can cancel or reschedule if something intervenes
- Example: "sleeping animal wakes instantly when attacked"
- Invalidate sleep timer and trigger immediate action

### 3. Lazy Simulation & Fast-Forwarding

**LOD for Simulation**:
- Full continuous simulation of entire planet is infeasible
- Apply lazy or approximate simulation to regions without players
- Fast-forward distant areas with coarse-grained statistics
- Use analytical models rather than simulating every agent

**Techniques**:
- **Scheduled Updates Only**: Process objects with pending actions only
- **Checkpoint & Replay**: Record state when leaving area, replay/advance when returning
- **Aggregate Models**: Replace detailed agents with statistical models during sleep
- **Deterministic Generation**: Use seeds to derive region details on-demand (like No Man's Sky)

**Causal Consistency**:
- After fast-forward, region state must match what would have happened with continuous simulation
- Critical for player perception and game integrity

### 4. Cascading Events & Geographical Propagation

**Event Chains**:
- Model significant changes as events with location, timestamp, and effect scope
- Example: Volcano eruption → temperature reduction → harvest shortage → migration
- Each event placed on global timeline
- Objects generate events and react to events

**Spatial Propagation**:
- Use spatial partitioning to scope events
- Events have radius of effect
- If range crosses boundaries, propagate to neighboring regions
- Push event up quadtree until finding node covering full area

## Implementation Strategies

### Priority Queue System

```
GlobalEventQueue:
  - Timestamp → Events mapping
  - Process events in chronological order
  - Entities add future events
  - Events can spawn new events
```

### Region-Based Processing

```
For each game tick:
  1. Process high-priority regions (player nearby)
  2. Check scheduled events for other regions
  3. Apply coarse updates to distant regions
  4. Propagate cross-region events
```

### Player Attention System

```
Active Regions (player nearby):
  - Full simulation every frame
  - All entities active
  - Real-time updates

Dormant Regions (no players):
  - Scheduled events only
  - Statistical approximations
  - Fast-forward on demand
```

## Relevance to BlueMarble

### Direct Applications

1. **Multi-Scale Simulation**:
   - Fast updates for player's local voxel area
   - Slower updates for distant planet regions
   - Climate and geological processes on day/week cycles

2. **Event-Driven Architecture**:
   - Terrain modifications as events
   - Resource spawning as scheduled events
   - Player actions trigger event chains
   - Idle regions consume no resources

3. **Spatial Partitioning**:
   - Chunk-based world already supports this
   - Each chunk can have independent update schedule
   - Cross-chunk events propagate through hierarchy

4. **Lazy Loading**:
   - Unvisited regions don't simulate continuously
   - Fast-forward when player enters
   - Deterministic generation for consistency

### Performance Benefits

- **Reduced CPU Load**: Only simulate active areas fully
- **Scalability**: Planet-scale becomes feasible
- **Responsive**: Fast updates where it matters (near players)
- **Efficient**: Background processes run at appropriate intervals

### Design Considerations

- **Event Propagation**: Volcanic eruptions, climate changes
- **Geological Processes**: Erosion, plate tectonics (macro tick)
- **Player Actions**: Building, mining (local high-res tick)
- **Resource Spawning**: Scheduled events per region
- **NPC Activity**: Full sim near players, statistical elsewhere

## Technical Requirements

1. **Priority Queue**: Global event scheduler
2. **Spatial Index**: Quadtree/octree for regions
3. **State Checkpointing**: Save region state when inactive
4. **Deterministic Systems**: Ensure consistent fast-forward
5. **Event Propagation**: Cross-region event handling

## Related Concepts

- **Stendhal MMORPG**: Uses turn counter with scheduled updates
- **No Man's Sky**: Seeded generation plus on-demand simulation
- **LOD Rendering**: Similar concept applied to simulation
- **Discrete Event Simulation**: Core paradigm for efficiency

## Summary

Temporal simulation at planet scale requires:
1. Hierarchical multi-scale updates
2. Event-driven processing
3. Lazy simulation for inactive regions
4. Spatial event propagation
5. Causal consistency guarantees

This approach enables responsive gameplay while simulating an entire planet efficiently.
