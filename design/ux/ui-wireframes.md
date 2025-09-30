# UI Wireframes

Interface mockups and user flows for BlueMarble.

---
title: UI Wireframes
date: 2025-09-30
owner: @Nomoos
status: draft
tags: [ui, wireframes, ux]
---

## Overview

This document contains UI wireframes, mockups, and flow diagrams for BlueMarble interfaces.

## Main Menu

**Flow**:

1. Splash Screen
2. Main Menu
3. Game Start / Options / Exit

**Elements**:

- Logo
- Start Game button
- Options button
- Exit button

## HUD (Heads-Up Display)

**Layout**:

```
[Health]              [Mini-Map]
[Resources]           
                      
[Abilities]           [Objectives]
```

**Elements**:

- Health bar
- Resource displays
- Ability cooldowns
- Mini-map
- Objectives tracker

## Inventory Screen

**Layout**:

```
+------------------+------------------+
|  Character       |   Inventory      |
|  Equipment       |   Grid           |
+------------------+------------------+
|  Stats           |   Details        |
+------------------+------------------+
```

**Interactions**:

- Drag & drop items
- Right-click context menu
- Filter/sort options

## Map Screen

**Elements**:

- World map
- Zoom controls
- Filter toggles
- Waypoint system

## Flows

### Flow 1: Item Acquisition

1. Pick up item
2. Item added to inventory
3. Notification displayed

### Flow 2: Crafting

1. Open crafting menu
2. Select recipe
3. Confirm materials
4. Create item

## Screen States

### State 1: Loading

- Loading spinner
- Progress bar
- Tips display

### State 2: Error

- Error message
- Retry button
- Return to menu option

## Accessibility Considerations

- Font size options
- Colorblind modes
- High contrast mode
- Screen reader support

## Wireframe Assets

Link to Figma/design assets when available.

## Related Documents

- [Controls](controls.md) - Input for UI navigation
- [UI Guidelines](../../docs/ui-ux/ui-guidelines.md) - Technical specs
- [Mechanics](../mechanics.md) - Features requiring UI
