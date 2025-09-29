# Spherical Planet Visualization - Frontend Design Specification

**Document Type:** UI/UX Design Specification  
**Version:** 1.0  
**Author:** UI/UX Design Team  
**Date:** 2024-12-29  
**Status:** Draft  
**Related Specification:** [Spherical Planet Generation System](../systems/spec-spherical-planet-generation.md)

## Overview

This document specifies the frontend visualization components for the Spherical Planet Generation system, including interactive map viewers, projection controls, and biome visualization interfaces. The design integrates with BlueMarble's existing frontend architecture while providing new capabilities for spherical planet exploration.

## User Interface Requirements

### 1. Primary Map Viewer Component

**Component Name:** `SphericalPlanetViewer`

**Purpose:** Interactive visualization of generated spherical planets with multiple projection support

**Key Features:**
- Multiple map projection display (Mercator, Robinson, Mollweide, etc.)
- Real-time projection switching
- Biome color coding and legends
- Zoom and pan functionality
- Coordinate display and conversion
- Seamless world wrapping navigation

**Interface Layout:**
```
┌─────────────────────────────────────────────────────────┐
│ ☰ Menu    🌍 Planet Viewer              📊 ⚙️ 📥      │
├─────────────────────────────────────────────────────────┤
│ Projection: [Equirectangular ▼] Biomes: [Enabled ☑️]   │
├─────────────────────────────────────────────────────────┤
│                                                         │
│              Interactive Map Display                    │
│                                                         │
│  ┌─────────────────────────────────────────────────┐   │
│  │                                                 │   │
│  │         [Generated Planet Surface]              │   │
│  │                                                 │   │
│  │  [Mouse: 125.345°E, 23.567°N]                 │   │
│  └─────────────────────────────────────────────────┘   │
│                                                         │
│ Zoom: [━━━━━●━━━━] Biome: TropicalRainforest              │
└─────────────────────────────────────────────────────────┘
```

### 2. Projection Control Panel

**Component Name:** `ProjectionControlPanel`

**Purpose:** Allow users to switch between different map projections and configure projection parameters

**Features:**
- Dropdown selection for projection types
- Parameter controls for customizable projections
- Distortion visualization overlay
- Projection property information

**Interface Elements:**
```
┌─────────────────────────────┐
│ Map Projection Controls     │
├─────────────────────────────┤
│ Type: [Robinson        ▼]   │
│ Central Meridian: [0°  ]    │
│ Standard Parallels:         │
│   Parallel 1: [30°    ]     │
│   Parallel 2: [60°    ]     │
├─────────────────────────────┤
│ ☑️ Show distortion grid     │
│ ☑️ Preserve area            │
│ ☐ Preserve angles           │
├─────────────────────────────┤
│ [Apply] [Reset to Default]  │
└─────────────────────────────┘
```

### 3. Biome Legend and Information Panel

**Component Name:** `BiomeLegendPanel`

**Purpose:** Display biome classifications, colors, and detailed information

**Features:**
- Interactive biome legend with color coding
- Biome filtering and highlighting
- Climate information display
- Statistical summaries

**Layout:**
```
┌─────────────────────────────┐
│ Biome Legend                │
├─────────────────────────────┤
│ 🟦 Ocean (47.2%)            │
│ 🟩 Tropical Rainforest (8%) │
│ 🟫 Desert (14.3%)           │
│ 🟨 Grassland (12.1%)        │
│ ⬜ Tundra (7.4%)            │
│ 🟪 Boreal Forest (11.0%)    │
├─────────────────────────────┤
│ Selected: Tropical Rainfor. │
│ Temperature: 20-30°C        │
│ Precipitation: >2000mm      │
│ Elevation: 0-1000m          │
│ Coverage: 8.0% (3.2M km²)   │
├─────────────────────────────┤
│ [Show All] [Hide Selected]  │
└─────────────────────────────┘
```

### 4. Planet Generation Controls

**Component Name:** `PlanetGenerationControls`

**Purpose:** Interface for configuring and generating new planets

**Features:**
- Planet parameter configuration
- Real-time generation preview
- Save/load planet configurations
- Generation progress indicator

**Interface:**
```
┌─────────────────────────────┐
│ Planet Generation           │
├─────────────────────────────┤
│ Planet Size:                │
│ Radius: [6371] km           │
│ Plates: [━━━●━━━━] 12        │
│                             │
│ Climate:                    │
│ Temperature: [━━━●━━━] 15°C  │
│ Ocean Coverage: [━━━━●━━] 71%│
│                             │
│ Seed: [12345    ] [🎲]      │
├─────────────────────────────┤
│ [Generate Planet] [Preview] │
│                             │
│ ████████░░ 80% Complete     │
│ Generating biomes...        │
└─────────────────────────────┘
```

## User Experience Flows

### 1. Planet Exploration Flow

```
Start → Load Planet → Choose Projection → Explore Map → 
Examine Biomes → Change Parameters → Generate New Planet
```

**Detailed Steps:**
1. **Initial Load:** User opens planet viewer, default planet loads with Equirectangular projection
2. **Projection Selection:** User experiments with different projections to see distortion effects
3. **Navigation:** User pans and zooms to explore different regions
4. **Biome Investigation:** User clicks on regions to see biome information
5. **Customization:** User adjusts planet parameters and generates new variants

### 2. Comparative Analysis Flow

```
Load Planet A → Split View → Load Planet B → 
Compare Projections → Analyze Differences → Export Comparison
```

**Features:**
- Side-by-side planet comparison
- Synchronized navigation between views
- Difference highlighting overlay
- Statistical comparison panels

### 3. Educational Exploration Flow

```
Tutorial Mode → Learn Projections → Understand Biomes → 
Practice Generation → Quiz Mode → Achievement Unlock
```

**Educational Components:**
- Interactive tutorials for map projections
- Biome classification learning modules
- Planet generation parameter education
- Gamified learning achievements

## Technical Integration

### JavaScript Module Structure

```javascript
// Main module organization
src/
├── modules/
│   ├── spherical-planet/
│   │   ├── SphericalPlanetViewer.js
│   │   ├── ProjectionManager.js
│   │   ├── BiomeClassifier.js
│   │   ├── CoordinateConverter.js
│   │   └── PlanetDataLoader.js
│   ├── projections/
│   │   ├── MercatorProjection.js
│   │   ├── RobinsonProjection.js
│   │   ├── MollweideProjection.js
│   │   └── ProjectionUtils.js
│   └── ui/
│       ├── PlanetControls.js
│       ├── BiomeLegend.js
│       └── ProjectionSelector.js
```

### API Integration Points

```javascript
// API communication patterns
export class PlanetDataService {
    constructor(baseUrl) {
        this.baseUrl = baseUrl;
        this.cache = new Map();
    }

    async getPlanetData(planetId, projection = 'equirectangular') {
        const cacheKey = `${planetId}-${projection}`;
        
        if (this.cache.has(cacheKey)) {
            return this.cache.get(cacheKey);
        }

        const response = await fetch(
            `${this.baseUrl}/api/planet/${planetId}/projection/${projection}`
        );
        
        const data = await response.json();
        this.cache.set(cacheKey, data);
        
        return data;
    }

    async generatePlanet(config) {
        const response = await fetch(`${this.baseUrl}/api/planet/generate`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(config)
        });
        
        return response.json();
    }

    async getBiomeStatistics(planetId) {
        const response = await fetch(
            `${this.baseUrl}/api/planet/${planetId}/biomes/statistics`
        );
        
        return response.json();
    }
}
```

### Coordinate System Integration

```javascript
// Integration with existing coordinate conversion
import { coordinateConversion } from '../utils/coordinate-conversion.js';

export class PlanetCoordinateManager {
    constructor() {
        this.WORLD_SIZE_X = 40075020; // meters
        this.WORLD_SIZE_Y = 20037510; // meters
    }

    convertSRIDToDisplay(sridCoord, projection = 'equirectangular') {
        // Convert from SRID_METER (4087) to lat/lon
        const longitude = (sridCoord.x / this.WORLD_SIZE_X) * 360 - 180;
        const latitude = (sridCoord.y / this.WORLD_SIZE_Y) * 180 - 90;
        
        // Apply selected projection
        return this.applyProjection(longitude, latitude, projection);
    }

    applyProjection(longitude, latitude, projectionType) {
        switch (projectionType) {
            case 'mercator':
                return this.mercatorProject(longitude, latitude);
            case 'robinson':
                return this.robinsonProject(longitude, latitude);
            case 'mollweide':
                return this.mollweideProject(longitude, latitude);
            default:
                return this.equirectangularProject(longitude, latitude);
        }
    }

    // Integration with existing quadtree system
    getQuadtreePath(displayCoord) {
        const sridCoord = this.convertDisplayToSRID(displayCoord);
        return coordinateConversion.getQuadPath(sridCoord.x, sridCoord.y);
    }
}
```

## Responsive Design Specifications

### Mobile Interface (320px - 768px)

```
┌─────────────────────┐
│ ☰  🌍 Planet Viewer │
├─────────────────────┤
│ Proj: [Equirect. ▼] │
│ Biomes: [On ☑️]     │
├─────────────────────┤
│                     │
│   [Planet Display]  │
│                     │
│   Lat: 23.5°N       │
│   Lon: 125.3°E      │
├─────────────────────┤
│ ████████░░ 80%      │
│ Generating...       │
└─────────────────────┘
```

### Tablet Interface (768px - 1024px)

```
┌─────────────────────────────────────┐
│ ☰ Menu  🌍 Planet Viewer      ⚙️ 📊 │
├─────────────────────────────────────┤
│ Projection: [Equirectangular ▼]    │
│ ☑️ Biomes  ☑️ Grid  ☐ Distortion   │
├─────────────────────────────────────┤
│                                     │
│        [Main Planet Display]        │
│                                     │
│ Coords: 125.345°E, 23.567°N        │
│ Biome: TropicalRainforest           │
├─────────────────────────────────────┤
│ [Generate] [Save] [Export]          │
└─────────────────────────────────────┘
```

### Desktop Interface (1024px+)

Full layout as shown in primary interface specifications above.

## Accessibility Requirements

### Keyboard Navigation
- Tab navigation through all interactive elements
- Arrow keys for map panning
- +/- keys for zoom control
- Enter/Space for button activation
- Escape to close modals and panels

### Screen Reader Support
```javascript
// ARIA attributes for map regions
<div 
    role="application" 
    aria-label="Interactive planet map viewer"
    aria-describedby="map-instructions">
    
    <div 
        role="img" 
        aria-label={`Planet surface showing ${biomeCount} different biomes`}
        aria-describedby="biome-legend">
        
        <canvas ref="mapCanvas" />
    </div>
    
    <div 
        id="map-instructions" 
        className="sr-only">
        Use arrow keys to pan the map, plus and minus to zoom.
        Tab to access projection controls and biome information.
    </div>
</div>
```

### Color Accessibility
- Colorblind-friendly biome color palette
- High contrast mode support
- Pattern/texture alternatives to color coding
- Configurable color schemes

### Visual Indicators
```javascript
// Biome color scheme with accessibility support
export const ACCESSIBLE_BIOME_COLORS = {
    Ocean: { 
        color: '#1E40AF', 
        pattern: 'dots',
        textColor: '#FFFFFF',
        highContrast: '#000080'
    },
    TropicalRainforest: { 
        color: '#15803D', 
        pattern: 'dense-lines',
        textColor: '#FFFFFF',
        highContrast: '#004000'
    },
    Desert: { 
        color: '#D97706', 
        pattern: 'sparse-dots',
        textColor: '#000000',
        highContrast: '#CC6600'
    }
    // ... additional biomes
};
```

## Performance Optimization

### Rendering Performance
- Canvas-based rendering for large datasets
- Level-of-detail (LOD) for polygon simplification
- Viewport culling for off-screen content
- Progressive loading of planet data

### Memory Management
```javascript
export class OptimizedPlanetRenderer {
    constructor(canvas, options = {}) {
        this.canvas = canvas;
        this.ctx = canvas.getContext('2d');
        this.tileCache = new LRUCache(100); // Limit cache size
        this.visiblePolygons = new Set();
        this.loadedTiles = new Map();
    }

    render(viewport) {
        // Only render visible polygons
        this.updateVisiblePolygons(viewport);
        
        // Use simplified geometry for distant objects
        const lodLevel = this.calculateLOD(viewport.zoom);
        
        // Progressive loading based on priority
        this.loadPriorityTiles(viewport);
        
        this.drawVisiblePolygons(lodLevel);
    }

    updateVisiblePolygons(viewport) {
        const bounds = viewport.getBounds();
        this.visiblePolygons.clear();
        
        // Use spatial indexing for efficient visibility testing
        const candidates = this.spatialIndex.query(bounds);
        
        for (const polygon of candidates) {
            if (this.intersects(polygon.bounds, bounds)) {
                this.visiblePolygons.add(polygon);
            }
        }
    }
}
```

### Data Loading Strategy
- Asynchronous loading with progress indicators
- Incremental data loading based on viewport
- Caching of frequently accessed projections
- Background preloading of adjacent regions

## Animation and Transitions

### Projection Transitions
```css
.projection-transition {
    transition: transform 0.8s cubic-bezier(0.4, 0.0, 0.2, 1);
}

@keyframes projection-morph {
    0% { 
        transform: matrix(1, 0, 0, 1, 0, 0); 
        opacity: 1; 
    }
    50% { 
        opacity: 0.7; 
    }
    100% { 
        transform: matrix(var(--target-transform)); 
        opacity: 1; 
    }
}
```

### Loading Animations
```javascript
// Planet generation progress animation
export class PlanetGenerationAnimator {
    constructor(canvas) {
        this.canvas = canvas;
        this.progress = 0;
        this.animationId = null;
    }

    startGenerationAnimation() {
        this.animateGeneration();
    }

    animateGeneration() {
        this.ctx.clearRect(0, 0, this.canvas.width, this.canvas.height);
        
        // Draw planet outline
        this.drawPlanetOutline();
        
        // Draw generation progress
        this.drawGenerationProgress();
        
        // Draw current biome being generated
        this.drawCurrentBiome();
        
        if (this.progress < 1.0) {
            this.animationId = requestAnimationFrame(() => this.animateGeneration());
        }
    }
}
```

## Testing and Quality Assurance

### Visual Testing Requirements
- Cross-browser rendering consistency
- Projection mathematical accuracy verification
- Color accessibility validation
- Performance benchmarking across devices

### User Testing Scenarios
1. **First-time User Journey:** New user exploring planet generation
2. **Educational Use Case:** Student learning about map projections
3. **Research Application:** Scientist analyzing biome distributions
4. **Accessibility Testing:** Users with various accessibility needs

### Automated Testing
```javascript
// Example visual regression test
describe('SphericalPlanetViewer', () => {
    test('renders Mercator projection correctly', async () => {
        const viewer = new SphericalPlanetViewer(mockCanvas, 'mercator');
        await viewer.loadPlanetData(testPlanetData);
        
        const screenshot = await viewer.takeScreenshot();
        expect(screenshot).toMatchSnapshot('mercator-projection.png');
    });

    test('biome colors match accessibility standards', () => {
        const colors = BiomeColorManager.getAccessibleColors();
        
        colors.forEach(color => {
            expect(getContrastRatio(color.foreground, color.background))
                .toBeGreaterThan(4.5); // WCAG AA standard
        });
    });
});
```

This frontend design specification provides comprehensive guidance for implementing the user interface components of the Spherical Planet Generation system, ensuring both functionality and accessibility while maintaining consistency with BlueMarble's existing design patterns.