// Market class for managing regional markets
import { commodities, regionalSpecialization } from './data.js';

export class Market {
    constructor(regionId) {
        this.regionId = regionId;
        this.region = regionalSpecialization[regionId];
        this.commodityData = new Map();
        
        // Initialize all commodities with base data
        Object.values(commodities).forEach(commodity => {
            this.commodityData.set(commodity.id, {
                supply: 100 + Math.random() * 50,
                demand: 100 + Math.random() * 50,
                currentPrice: commodity.basePrice,
                priceHistory: [commodity.basePrice]
            });
        });
    }

    calculatePrice(commodityId) {
        const commodity = commodities[commodityId];
        const data = this.commodityData.get(commodityId);
        
        // Supply/demand ratio with dampening
        const ratio = data.demand / Math.max(data.supply, 1);
        const priceMultiplier = Math.pow(ratio, 0.5);
        
        // Regional specialization
        const specialization = this.region.specialization[commodityId] || 1.0;
        
        // Calculate final price with caps
        let finalPrice = commodity.basePrice * priceMultiplier / specialization;
        finalPrice = Math.max(finalPrice, commodity.basePrice * 0.5);
        finalPrice = Math.min(finalPrice, commodity.basePrice * 3.0);
        
        return Math.round(finalPrice);
    }

    updatePrices() {
        this.commodityData.forEach((data, commodityId) => {
            // Random supply/demand fluctuation
            data.supply += (Math.random() - 0.5) * 5;
            data.demand += (Math.random() - 0.5) * 5;
            
            // Keep values in reasonable range
            data.supply = Math.max(50, Math.min(200, data.supply));
            data.demand = Math.max(50, Math.min(200, data.demand));
            
            // Update price
            const newPrice = this.calculatePrice(commodityId);
            data.currentPrice = newPrice;
            data.priceHistory.push(newPrice);
            
            // Keep history limited
            if (data.priceHistory.length > 100) {
                data.priceHistory.shift();
            }
        });
    }

    getPrice(commodityId) {
        const data = this.commodityData.get(commodityId);
        return data ? data.currentPrice : 0;
    }

    getPriceChange(commodityId) {
        const data = this.commodityData.get(commodityId);
        if (!data || data.priceHistory.length < 2) return 0;
        
        const current = data.priceHistory[data.priceHistory.length - 1];
        const previous = data.priceHistory[data.priceHistory.length - 2];
        return ((current - previous) / previous * 100).toFixed(1);
    }

    recordTransaction(commodityId, quantity, type) {
        const data = this.commodityData.get(commodityId);
        if (!data) return;
        
        if (type === 'buy') {
            data.demand += quantity * 0.1;
            data.supply -= quantity * 0.05;
        } else if (type === 'sell') {
            data.supply += quantity * 0.1;
            data.demand -= quantity * 0.05;
        }
    }
}
