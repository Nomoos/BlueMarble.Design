# UI Wireframes

Interface mockups and user flows for BlueMarble.

<!--
Front matter example:
---
title: UI Wireframes
date: 2025-09-30
owner: @Nomoos
status: draft
tags: [ui, wireframes, ux]
---
-->

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

```text
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

```text
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

**Layout**:

```text
+--------------------------------------------------+
| [Filter: All ▼] [Zoom: + -]     [Waypoint: +]  |
+--------------------------------------------------+
|                                                  |
|   🗺️ World Map Display                          |
|                                                  |
|   • Resource Deposits (color-coded)             |
|   • Player Locations (icons)                    |
|   • Building Locations (structures)             |
|   • Terrain Features (elevation overlay)        |
|                                                  |
|   [Mini-Map in corner]                          |
|                                                  |
+--------------------------------------------------+
| Legend: 🟡 Gold  🔵 Water  🟢 Forest  🏗️ Build  |
+--------------------------------------------------+
```

**Elements**:

- World map with multiple layers
- Zoom controls (+/- buttons)
- Filter toggles (resources, players, buildings, terrain)
- Waypoint system (place markers, create paths)
- Legend for color-coded information
- Coordinate display
- Elevation/depth indicator

**Interactions**:

- Click to place waypoint
- Drag to pan map
- Scroll to zoom
- Right-click for context menu (quick travel, mark location)
- Toggle layers on/off
- Search locations by name

## Building Placement UI

**Layout**:

```text
+--------------------------------------------------+
| Building Mode: [Structure Type ▼]     [Cancel] |
+--------------------------------------------------+
|                                                  |
|   🏗️ Preview Structure (Semi-transparent)       |
|                                                  |
|   ✓ Valid Placement (Green outline)             |
|   ✗ Invalid Placement (Red outline)             |
|                                                  |
+--------------------------------------------------+
| Requirements:                                    |
| • Wood: 50/50 ✓                                  |
| • Stone: 30/30 ✓                                 |
| • Terrain: Flat ✓                                |
| • Proximity: OK ✓                                |
|                                                  |
| [Rotate] [Confirm] [Cancel]                     |
+--------------------------------------------------+
```

**Elements**:

- Structure preview (transparent 3D model)
- Validation indicators (color-coded outline)
- Resource requirement display
- Placement rules checklist
- Rotation controls
- Grid/snap indicators
- Confirm/Cancel buttons

**Interactions**:

- Move cursor to position structure
- Mouse wheel to rotate
- Click to confirm placement
- ESC to cancel
- Real-time validation feedback

**Validation Checks**:

- Terrain slope acceptable
- No overlapping structures
- Sufficient resources available
- Proximity rules satisfied
- Zone/biome restrictions

## Mining/Resource Extraction UI

**Layout**:

```text
+--------------------------------------------------+
| Resource Scanner                                 |
+--------------------------------------------------+
| Scanning radius: [====50m====]  [Scan]          |
+--------------------------------------------------+
|                                                  |
|   Detected Resources:                           |
|                                                  |
|   🟡 Gold Deposit - 200m NE                     |
|      Richness: ████████ 80%                     |
|      Depth: 15m underground                     |
|      [Mark] [Extract]                           |
|                                                  |
|   🔵 Iron Ore - 150m W                          |
|      Richness: ████ 40%                         |
|      Depth: 8m underground                      |
|      [Mark] [Extract]                           |
|                                                  |
+--------------------------------------------------+
| Active Extraction:                              |
| Progress: [=========90%]  Time: 2m 15s          |
| Yield: 45 units (est. 50 units)                 |
+--------------------------------------------------+
```

**Elements**:

- Scanner radius control
- Resource list with details
- Distance and direction indicators
- Resource richness/quality meter
- Depth information
- Extraction progress bar
- Estimated yield display
- Quick action buttons

**Interactions**:

- Adjust scan radius
- Trigger scan
- Mark resources on map
- Initiate extraction
- View detailed resource info
- Cancel extraction

**Extraction Process**:

1. Scan area for resources
2. Select resource to extract
3. Position extraction equipment
4. Monitor progress
5. Collect extracted materials

## Player Interaction UI

**Layout**:

```text
+--------------------------------------------------+
| Player: [PlayerName] Lv.25      Distance: 15m   |
+--------------------------------------------------+
| [Trade] [Invite] [Message] [View Profile]       |
+--------------------------------------------------+
| Status: Online  Guild: [GuildName]              |
| Activity: Mining                                |
+--------------------------------------------------+
```

**Trade Interface**:

```text
+------------------------+------------------------+
|   Your Offer           |   Their Offer          |
|                        |                        |
| [Item Slots]           | [Item Slots]           |
|                        |                        |
| Gold: [     ]          | Gold: [     ]          |
|                        |                        |
| [✗ Not Ready]          | [✗ Not Ready]          |
+------------------------+------------------------+
| [Confirm Trade] [Cancel]                        |
+--------------------------------------------------+
```

**Collaboration Panel**:

```text
+--------------------------------------------------+
| Nearby Players: 5                                |
+--------------------------------------------------+
| • PlayerA (25m) - Building    [Invite]          |
| • PlayerB (40m) - Mining      [Invite]          |
| • PlayerC (50m) - Exploring   [Invite]          |
+--------------------------------------------------+
| Active Collaborations:                          |
| • Building Project - 3 players                  |
|   Progress: [====60%]   [Leave] [Details]      |
+--------------------------------------------------+
```

**Elements**:

- Player info display
- Quick action buttons
- Trade interface with item/gold exchange
- Ready status indicators
- Collaboration invitations
- Active collaboration tracking
- Distance indicators

**Interactions**:

- Initiate trade
- Send messages
- Invite to party/collaboration
- View player profiles
- Accept/reject invitations
- Leave collaborations

## Data Visualization

**Resource Distribution Map**:

```text
+--------------------------------------------------+
| Resource Heatmap                                 |
+--------------------------------------------------+
|                                                  |
|   🟥🟥🟧🟧🟨🟩🟩     Density Legend:             |
|   🟥🟧🟧🟨🟨🟩🟩     🟥 Very High                |
|   🟧🟧🟨🟨🟨🟩🟩     🟧 High                     |
|   🟨🟨🟨🟨🟩🟩🟩     🟨 Medium                   |
|   🟩🟩🟩🟩🟩🟩🟩     🟩 Low                      |
|                                                  |
+--------------------------------------------------+
```

**World State Dashboard**:

```text
+--------------------------------------------------+
| World Statistics                                 |
+--------------------------------------------------+
| Active Players: 1,234      Peak: 1,500          |
| Resources Extracted (24h): 45,678 units         |
| Structures Built (24h): 234                     |
| Active Collaborations: 89                       |
+--------------------------------------------------+
| Regional Activity:                              |
| • North Region: ████████ 80% activity           |
| • South Region: ████ 40% activity               |
| • East Region: ██████ 60% activity              |
| • West Region: ██ 20% activity                  |
+--------------------------------------------------+
```

**Player Activity Timeline**:

```text
+--------------------------------------------------+
| Recent Activity                                  |
+--------------------------------------------------+
| 2m ago - PlayerX completed building              |
| 5m ago - PlayerY extracted 50 iron ore          |
| 8m ago - PlayerZ joined collaboration           |
| 10m ago - Guild built structure                 |
+--------------------------------------------------+
```

**Elements**:

- Heatmaps for resource density
- Activity graphs and charts
- Real-time statistics
- Regional breakdowns
- Timeline/feed of events
- Color-coded visualization
- Filterable data views

**Interactions**:

- Filter by resource type
- Filter by time period
- Zoom to regions
- Export data
- Share visualizations
- Set alerts/notifications

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

### Flow 3: Building Placement

1. Enter building mode (B key)
2. Select structure type from menu
3. Position structure preview in world
4. Rotate to desired orientation
5. Validate placement (automatic)
6. Confirm placement if valid
7. Structure construction begins
8. Exit building mode

### Flow 4: Resource Extraction

1. Open resource scanner (G key)
2. Adjust scan radius
3. Trigger scan
4. Review detected resources
5. Select resource to extract
6. Move to resource location
7. Position extraction equipment
8. Initiate extraction
9. Monitor progress
10. Collect extracted materials

### Flow 5: Player Trade

1. Approach another player
2. Initiate trade request
3. Wait for acceptance
4. Add items/gold to offer
5. Mark as ready
6. Wait for other player ready
7. Confirm trade
8. Transaction completed
9. Receive traded items

## Screen States

### State 1: Loading

- Loading spinner
- Progress bar
- Tips display

### State 2: Error

- Error message
- Retry button
- Return to menu option

### State 3: Building Mode

- Structure preview visible
- Placement validation active
- Resource requirements displayed
- Grid/snap indicators shown
- Build controls available

### State 4: Scanning

- Scanner animation active
- Scan radius visualization
- Progress indicator
- Cancel scan option
- Awaiting results

### State 5: Trading

- Trade window open
- Item slots interactive
- Ready status tracking
- Both players must confirm
- Trade locked when ready

## Accessibility Considerations

- Font size options
- Colorblind modes
- High contrast mode
- Screen reader support

## Wireframe Assets

Link to Figma/design assets when available.

## Related Documents

- [Controls](../ux/controls.md) - Input for UI navigation
- [UI Guidelines](../../docs/ui-ux/ui-guidelines.md) - Technical specs
- [Mechanics](../mechanics.md) - Features requiring UI
