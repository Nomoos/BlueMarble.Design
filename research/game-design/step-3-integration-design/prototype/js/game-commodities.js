// Game-specific commodity definitions for testing multi-step swaps
// Based on the problem statement: Wood/Cherry has big market, Wood/Iron not, 
// but there are markets for Iron/Cherry, Iron/Strawberry, Strawberry/Cherry
// Also paths through game currency or Carrot/Iron, Carrot/Wood, Carrot/Blueberry, Blueberry/Strawberry

export const gameCommodities = {
    'wood': {
        id: 'wood',
        name: 'Wood',
        category: 'Resources',
        basePrice: 20,
        description: 'Basic building material'
    },
    'cherry': {
        id: 'cherry',
        name: 'Cherry',
        category: 'Food',
        basePrice: 45,
        description: 'Valuable fruit with large market'
    },
    'iron': {
        id: 'iron',
        name: 'Iron',
        category: 'Resources',
        basePrice: 60,
        description: 'Metal ore for crafting'
    },
    'strawberry': {
        id: 'strawberry',
        name: 'Strawberry',
        category: 'Food',
        basePrice: 40,
        description: 'Popular fruit'
    },
    'carrot': {
        id: 'carrot',
        name: 'Carrot',
        category: 'Food',
        basePrice: 25,
        description: 'Common vegetable, good intermediate commodity'
    },
    'blueberry': {
        id: 'blueberry',
        name: 'Blueberry',
        category: 'Food',
        basePrice: 50,
        description: 'Premium berry'
    },
    'game-currency': {
        id: 'game-currency',
        name: 'Gold Coins',
        category: 'Currency',
        basePrice: 1,
        description: 'Universal game currency'
    }
};

// Market configuration showing which commodity pairs have active markets
// Represents market depth/liquidity for each trading pair
export const marketPairs = {
    // Large, liquid markets
    'wood-cherry': { volume: 1000, spreadPercent: 2 },
    
    // Medium markets
    'iron-cherry': { volume: 500, spreadPercent: 3 },
    'iron-strawberry': { volume: 450, spreadPercent: 3.5 },
    'strawberry-cherry': { volume: 600, spreadPercent: 2.5 },
    
    // Markets through carrots
    'carrot-iron': { volume: 400, spreadPercent: 4 },
    'carrot-wood': { volume: 550, spreadPercent: 3 },
    'carrot-blueberry': { volume: 350, spreadPercent: 4.5 },
    'blueberry-strawberry': { volume: 400, spreadPercent: 3.5 },
    
    // Currency pairs (most liquid)
    'wood-game-currency': { volume: 1200, spreadPercent: 1.5 },
    'cherry-game-currency': { volume: 1100, spreadPercent: 1.5 },
    'iron-game-currency': { volume: 900, spreadPercent: 2 },
    'strawberry-game-currency': { volume: 850, spreadPercent: 2 },
    'carrot-game-currency': { volume: 700, spreadPercent: 2.5 },
    'blueberry-game-currency': { volume: 600, spreadPercent: 3 },
    
    // Small/illiquid markets
    'wood-iron': { volume: 50, spreadPercent: 15 }, // Very small market!
};

/**
 * Test scenarios demonstrating when multi-step routing is beneficial
 */
export const testScenarios = [
    {
        name: 'Wood to Iron - Small direct market',
        from: 'wood',
        to: 'iron',
        amount: 100,
        expectedResult: 'Multi-step through carrot should be better',
        expectedPath: ['wood', 'carrot', 'iron']
    },
    {
        name: 'Wood to Cherry - Large direct market',
        from: 'wood',
        to: 'cherry',
        amount: 100,
        expectedResult: 'Direct route should be best',
        expectedPath: ['wood', 'cherry']
    },
    {
        name: 'Iron to Strawberry - Medium market',
        from: 'iron',
        to: 'strawberry',
        amount: 50,
        expectedResult: 'Direct might be good, or through cherry',
        expectedPath: ['iron', 'strawberry'] // or ['iron', 'cherry', 'strawberry']
    },
    {
        name: 'Wood to Blueberry - No direct market',
        from: 'wood',
        to: 'blueberry',
        amount: 100,
        expectedResult: 'Must use multi-step, probably through carrot',
        expectedPath: ['wood', 'carrot', 'blueberry']
    },
    {
        name: 'Blueberry to Iron - No direct market',
        from: 'blueberry',
        to: 'iron',
        amount: 50,
        expectedResult: 'Multi-step through strawberry and cherry, or through carrot',
        expectedPath: ['blueberry', 'carrot', 'iron'] // or longer paths
    },
    {
        name: 'Complex multi-hop scenario',
        from: 'blueberry',
        to: 'wood',
        amount: 75,
        expectedResult: 'Multiple possible paths through intermediaries',
        expectedPath: null // Multiple valid paths possible
    }
];

/**
 * Creates a simple market simulation for the game commodities
 * This allows testing the swap router with realistic market conditions
 */
export class GameMarket {
    constructor(marketName = 'central-market') {
        this.marketName = marketName;
        this.commodityData = new Map();
        
        // Initialize commodity data with market pair information
        Object.values(gameCommodities).forEach(commodity => {
            // Base supply and demand
            let supply = 100 + Math.random() * 50;
            let demand = 100 + Math.random() * 50;
            
            // Adjust based on category
            if (commodity.category === 'Currency') {
                supply = 1000; // Very liquid
                demand = 1000;
            } else if (commodity.category === 'Food') {
                supply *= 1.2; // More food available
            }
            
            this.commodityData.set(commodity.id, {
                supply,
                demand,
                currentPrice: commodity.basePrice,
                priceHistory: [commodity.basePrice],
                marketPairs: this.getMarketPairsFor(commodity.id)
            });
        });
    }

    /**
     * Get all trading pairs available for a commodity
     */
    getMarketPairsFor(commodityId) {
        const pairs = [];
        Object.keys(marketPairs).forEach(pairKey => {
            const [first, second] = pairKey.split('-');
            if (first === commodityId || second === commodityId) {
                pairs.push({
                    pairKey,
                    partner: first === commodityId ? second : first,
                    volume: marketPairs[pairKey].volume,
                    spread: marketPairs[pairKey].spreadPercent
                });
            }
        });
        return pairs;
    }

    /**
     * Calculate price for a commodity
     */
    calculatePrice(commodityId) {
        const commodity = gameCommodities[commodityId];
        const data = this.commodityData.get(commodityId);
        
        if (!commodity || !data) return 0;
        
        // Supply/demand ratio
        const ratio = data.demand / Math.max(data.supply, 1);
        const priceMultiplier = Math.pow(ratio, 0.5);
        
        // Calculate price with caps
        let finalPrice = commodity.basePrice * priceMultiplier;
        finalPrice = Math.max(finalPrice, commodity.basePrice * 0.5);
        finalPrice = Math.min(finalPrice, commodity.basePrice * 3.0);
        
        return Math.round(finalPrice * 100) / 100;
    }

    /**
     * Get current price
     */
    getPrice(commodityId) {
        const data = this.commodityData.get(commodityId);
        return data ? data.currentPrice : 0;
    }

    /**
     * Update market prices
     */
    updatePrices() {
        this.commodityData.forEach((data, commodityId) => {
            // Random fluctuation
            data.supply += (Math.random() - 0.5) * 10;
            data.demand += (Math.random() - 0.5) * 10;
            
            // Keep in range
            data.supply = Math.max(50, Math.min(300, data.supply));
            data.demand = Math.max(50, Math.min(300, data.demand));
            
            // Update price
            const newPrice = this.calculatePrice(commodityId);
            data.currentPrice = newPrice;
            data.priceHistory.push(newPrice);
            
            if (data.priceHistory.length > 50) {
                data.priceHistory.shift();
            }
        });
    }

    /**
     * Check if a direct market exists between two commodities
     */
    hasDirectMarket(commodityA, commodityB) {
        const pairKey1 = `${commodityA}-${commodityB}`;
        const pairKey2 = `${commodityB}-${commodityA}`;
        return marketPairs.hasOwnProperty(pairKey1) || marketPairs.hasOwnProperty(pairKey2);
    }

    /**
     * Get market depth for a trading pair
     */
    getMarketDepth(commodityA, commodityB) {
        const pairKey1 = `${commodityA}-${commodityB}`;
        const pairKey2 = `${commodityB}-${commodityA}`;
        
        const pair = marketPairs[pairKey1] || marketPairs[pairKey2];
        if (!pair) return null;
        
        return {
            volume: pair.volume,
            spread: pair.spreadPercent,
            exists: true
        };
    }
}
