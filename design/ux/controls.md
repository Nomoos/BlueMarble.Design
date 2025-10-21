# Controls

Input schemes and control layouts for BlueMarble.

<!--
Front matter example:
---
title: Controls
date: 2025-09-30
owner: @Nomoos
status: draft
tags: [controls, input, ux]
---
-->

## Overview

This document defines the control schemes for BlueMarble across different platforms.

## Primary Controls

### Keyboard & Mouse

**Movement**:

- W/A/S/D: Character movement
- Mouse: Camera control

**Actions**:

- Left Click: Primary action
- Right Click: Secondary action
- E: Interact

**Interface**:

- I: Inventory
- M: Map
- B: Building mode
- G: Resource scanner
- T: Trade (when near player)
- ESC: Menu

**Building Mode**:

- Mouse Move: Position structure
- Mouse Wheel: Rotate structure
- Left Click: Confirm placement
- Right Click: Cancel/Exit building mode
- R: Rotate structure (alternative)
- Tab: Cycle structure types

**Resource Extraction**:

- G: Open scanner
- +/-: Adjust scan radius
- Space: Trigger scan
- Left Click: Select resource
- E: Begin extraction
- ESC: Cancel extraction

### Controller

**Movement**:

- Left Stick: Character movement
- Right Stick: Camera control

**Actions**:

- A/X: Primary action
- B/Circle: Secondary action
- Y/Triangle: Interact

**Interface**:

- Start: Menu
- Select: Map
- X/Square: Building mode
- Y/Triangle: Resource scanner
- LB: Previous tool
- RB: Next tool

**Building Mode**:

- Left Stick: Position structure
- Right Stick: Rotate structure
- A/X: Confirm placement
- B/Circle: Cancel/Exit building mode
- D-pad Left/Right: Cycle structure types

**Resource Extraction**:

- Y/Triangle: Open scanner
- D-pad Up/Down: Adjust scan radius
- A/X: Trigger scan or select resource
- B/Circle: Cancel
- RT: Begin extraction

## Control Contexts

### Context 1: Exploration

Active controls when exploring the world

### Context 2: Combat

Active controls during combat

### Context 3: Menu/UI

Active controls in menus

### Context 4: Building Mode

Active controls when placing structures:

- Movement controls remain active
- Building-specific actions enabled
- Rotation controls active
- Placement confirmation/cancellation
- Structure type selection

### Context 5: Resource Extraction

Active controls during scanning and extraction:

- Scanner interface controls
- Scan radius adjustment
- Resource selection
- Extraction initiation
- Extraction monitoring
- Movement restricted during active extraction

## Accessibility

- Remappable controls
- Controller support
- Alternative input methods

## Edge Cases

- Conflicting inputs: Resolution
- Context switching: How transitions work

## Related Documents

- [UI Wireframes](../wireframes/ui-wireframes.md) - Visual interface
- [Mechanics](../mechanics.md) - Actions requiring controls
