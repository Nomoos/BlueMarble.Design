// Economic simulation controller
import { Market } from './market.js';
import { regionalSpecialization, commodities, shipTypes, calculateDistance } from './data.js';

export class EconomicSimulation {
    constructor() {
        this.markets = new Map();
        this.gameTime = 0; // Minutes
        this.isPaused = false;
        this.speed = 1; // 1x or 2x
        this.statistics = {
            totalTrades: 0,
            totalVolume: 0,
            activeShips: 3,
            avgPriceChange: 0
        };
        
        // Initialize markets for each region
        Object.keys(regionalSpecialization).forEach(regionId => {
            this.markets.set(regionId, new Market(regionId));
        });
    }

    tick() {
        if (this.isPaused) return;
        
        // Advance game time (1 minute per tick at 1x speed)
        this.gameTime += this.speed;
        
        // Update markets every 10 game minutes
        if (this.gameTime % 10 === 0) {
            this.markets.forEach(market => market.updatePrices());
            this.updateStatistics();
        }
    }

    updateStatistics() {
        let totalChange = 0;
        let count = 0;
        
        this.markets.forEach(market => {
            market.commodityData.forEach((data, commodityId) => {
                const change = parseFloat(market.getPriceChange(commodityId));
                if (!isNaN(change)) {
                    totalChange += Math.abs(change);
                    count++;
                }
            });
        });
        
        this.statistics.avgPriceChange = count > 0 ? (totalChange / count).toFixed(2) : 0;
        
        // Simulate trades happening
        if (Math.random() < 0.3) {
            this.statistics.totalTrades++;
            this.statistics.totalVolume += Math.floor(Math.random() * 10000 + 1000);
        }
    }

    getTimeString() {
        const day = Math.floor(this.gameTime / (24 * 60)) + 1;
        const hours = Math.floor((this.gameTime % (24 * 60)) / 60);
        const minutes = this.gameTime % 60;
        return `Day ${day}, ${String(hours).padStart(2, '0')}:${String(minutes).padStart(2, '0')}`;
    }

    togglePause() {
        this.isPaused = !this.isPaused;
        return this.isPaused;
    }

    toggleSpeed() {
        this.speed = this.speed === 1 ? 2 : 1;
        return this.speed;
    }

    calculateTradeProfit(shipTypeId, originId, destId, commodityId) {
        const ship = shipTypes[shipTypeId];
        const commodity = commodities[commodityId];
        const originMarket = this.markets.get(originId);
        const destMarket = this.markets.get(destId);
        
        // Get prices
        const buyPrice = originMarket.getPrice(commodityId);
        const sellPrice = destMarket.getPrice(commodityId);
        
        // Calculate distance and travel cost
        const distance = calculateDistance(originId, destId);
        const travelCost = (distance / 100) * ship.fuelCostPer100km;
        
        // Maximum cargo we can carry
        const maxCargo = Math.floor(ship.capacity / (commodity.weight / 1000));
        
        // Calculate profit
        const revenue = sellPrice * maxCargo;
        const costs = (buyPrice * maxCargo) + travelCost;
        const profit = revenue - costs;
        const profitMargin = ((profit / costs) * 100).toFixed(1);
        
        // Travel time in hours
        const travelTime = (distance / ship.speed).toFixed(1);
        
        return {
            ship: ship.name,
            commodity: commodity.name,
            origin: regionalSpecialization[originId].name,
            destination: regionalSpecialization[destId].name,
            distance: Math.round(distance),
            buyPrice,
            sellPrice,
            maxCargo,
            revenue,
            costs,
            profit,
            profitMargin,
            travelTime,
            travelCost: Math.round(travelCost),
            isProfitable: profit > 0
        };
    }

    getAllCommodityPrices(regionId) {
        const market = this.markets.get(regionId);
        const prices = {};
        
        Object.values(commodities).forEach(commodity => {
            const price = market.getPrice(commodity.id);
            const change = parseFloat(market.getPriceChange(commodity.id));
            prices[commodity.id] = {
                name: commodity.name,
                category: commodity.category,
                price,
                change,
                basePrice: commodity.basePrice
            };
        });
        
        return prices;
    }
}
