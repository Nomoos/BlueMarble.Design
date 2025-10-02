# Analysis: Activity-Based Skill and Attribute System

## Executive Summary

This analysis examines a "learn by doing" attribute system with four main categories (Might, Dexterity, Mind, Presence), each containing three sub-aspects. The system promotes natural character development through gameplay actions rather than manual point allocation. This design philosophy aligns perfectly with BlueMarble's sandbox gameplay, where character progression should emerge organically from player activities like mining, building, exploring, and trading.

## Key Insights

### 1. **Natural Progression Through Actions**

**Core Principle**: Skills improve by performing related activities

**Benefits**:
- Character development reflects actual gameplay
- No artificial leveling mechanics
- Encourages experimentation
- Rewards diverse playstyles

**Relevance to BlueMarble**: Mining improves strength, building develops coordination, exploration enhances perception - all natural gameplay loops.

### 2. **Four-Attribute Model with Sub-Aspects**

**Structure**:
- **Might**: Strength, Stamina, Robustness
- **Dexterity**: Speed, Fine Motor Skills, Coordination  
- **Mind**: Intelligence, Perception, Foresight
- **Presence**: Leadership, Persuasion, Mental Resilience

**Advantages**:
- Covers physical, mental, and social abilities
- Sub-aspects provide specialization depth
- Main attributes act as resource pools
- Flexible character builds

**Relevance to BlueMarble**: Supports combat, crafting, exploration, and social gameplay styles.

### 3. **No Class Restrictions**

**Design Choice**: Players develop based on what they do, not chosen class

**Impact**:
- Hybrid builds possible
- Players can change focus over time
- Encourages exploration of game systems
- Reduces barriers to trying new activities

**Relevance to BlueMarble**: Perfect for sandbox world - players shouldn't be locked into roles.

## Recommendations

### Implementation Priority

1. **Activity Tracking System** (Critical)
   - Track player actions server-side
   - Map actions to attribute improvements
   - Gradual, continuous progression

2. **Attribute Database Schema** (Critical)
   - Four main attributes with sub-aspects
   - Current values and progression rates
   - Action contribution formulas

3. **Balance Mechanisms** (High Priority)
   - Diminishing returns prevent grinding
   - Multiple activities contribute to each attribute
   - Encourage diverse gameplay

### Design Guidelines

- **Transparency**: Show players how actions improve attributes
- **Feedback**: Visual progression indicators
- **Balance**: No single optimal path
- **Flexibility**: Easy to respec or try new activities

## Related Research

- [MMORPG Automated Mechanics](../conversation-dr_68dd00b5/) - Offline progression systems
- [Game Design Research](../../game-design/) - Core gameplay mechanics
