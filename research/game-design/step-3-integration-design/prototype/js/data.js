// Commodity definitions with base properties
export const commodities = {
    'iron-ore': {
        id: 'iron-ore',
        name: 'Iron Ore',
        category: 'Raw Materials',
        basePrice: 50,
        weight: 1000,
        volume: 0.5
    },
    'coal': {
        id: 'coal',
        name: 'Coal',
        category: 'Raw Materials',
        basePrice: 30,
        weight: 800,
        volume: 0.6
    },
    'timber': {
        id: 'timber',
        name: 'Timber',
        category: 'Raw Materials',
        basePrice: 25,
        weight: 600,
        volume: 0.8
    },
    'steel': {
        id: 'steel',
        name: 'Steel',
        category: 'Processed Materials',
        basePrice: 120,
        weight: 1000,
        volume: 0.3,
        recipe: { 'iron-ore': 2, 'coal': 1 }
    },
    'planks': {
        id: 'planks',
        name: 'Planks',
        category: 'Processed Materials',
        basePrice: 45,
        weight: 400,
        volume: 0.4,
        recipe: { 'timber': 2 }
    },
    'cloth': {
        id: 'cloth',
        name: 'Cloth',
        category: 'Processed Materials',
        basePrice: 60,
        weight: 50,
        volume: 0.2
    },
    'tools': {
        id: 'tools',
        name: 'Tools',
        category: 'Manufactured Goods',
        basePrice: 200,
        weight: 100,
        volume: 0.15,
        recipe: { 'steel': 1, 'timber': 1 }
    },
    'ship-components': {
        id: 'ship-components',
        name: 'Ship Components',
        category: 'Manufactured Goods',
        basePrice: 500,
        weight: 500,
        volume: 0.5,
        recipe: { 'steel': 2, 'planks': 3, 'cloth': 1 }
    },
    'furniture': {
        id: 'furniture',
        name: 'Furniture',
        category: 'Manufactured Goods',
        basePrice: 150,
        weight: 200,
        volume: 0.6,
        recipe: { 'planks': 2, 'tools': 1 }
    },
    'spices': {
        id: 'spices',
        name: 'Spices',
        category: 'Luxury Goods',
        basePrice: 300,
        weight: 20,
        volume: 0.1
    },
    'fine-art': {
        id: 'fine-art',
        name: 'Fine Art',
        category: 'Luxury Goods',
        basePrice: 800,
        weight: 50,
        volume: 0.2
    }
};

// Ship type definitions
export const shipTypes = {
    'coastal': {
        id: 'coastal',
        name: 'Coastal Trader',
        capacity: 50,
        speed: 15,
        range: 500,
        cost: 2000,
        fuelCostPer100km: 10
    },
    'brig': {
        id: 'brig',
        name: 'Merchant Brig',
        capacity: 150,
        speed: 12,
        range: 1500,
        cost: 8000,
        fuelCostPer100km: 25
    },
    'galleon': {
        id: 'galleon',
        name: 'Heavy Cargo Ship',
        capacity: 400,
        speed: 8,
        range: 3000,
        cost: 25000,
        fuelCostPer100km: 50
    },
    'clipper': {
        id: 'clipper',
        name: 'Fast Clipper',
        capacity: 100,
        speed: 20,
        range: 2000,
        cost: 15000,
        fuelCostPer100km: 40
    }
};

// Regional specialization multipliers
export const regionalSpecialization = {
    'north-america': {
        name: 'North America',
        position: { x: 150, y: 150 },
        specialization: {
            'iron-ore': 0.8,
            'coal': 0.85,
            'timber': 0.9
        }
    },
    'europe': {
        name: 'Europe',
        position: { x: 350, y: 120 },
        specialization: {
            'steel': 0.85,
            'tools': 0.8,
            'ship-components': 0.85
        }
    },
    'asia': {
        name: 'Asia',
        position: { x: 550, y: 180 },
        specialization: {
            'cloth': 0.75,
            'spices': 0.7,
            'fine-art': 0.8
        }
    },
    'south-america': {
        name: 'South America',
        position: { x: 200, y: 300 },
        specialization: {
            'timber': 0.75,
            'furniture': 0.85
        }
    },
    'africa': {
        name: 'Africa',
        position: { x: 400, y: 280 },
        specialization: {
            'spices': 0.85,
            'fine-art': 0.85
        }
    },
    'oceania': {
        name: 'Oceania',
        position: { x: 650, y: 320 },
        specialization: {}  // Balanced, no special bonuses
    }
};

// Calculate distance between two regions
export function calculateDistance(region1Id, region2Id) {
    const r1 = regionalSpecialization[region1Id];
    const r2 = regionalSpecialization[region2Id];
    
    const dx = r2.position.x - r1.position.x;
    const dy = r2.position.y - r1.position.y;
    
    // Scale factor to convert SVG units to km (roughly)
    const scaleFactor = 10;
    return Math.sqrt(dx * dx + dy * dy) * scaleFactor;
}
