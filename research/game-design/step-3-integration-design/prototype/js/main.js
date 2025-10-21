// Main application entry point
import { EconomicSimulation } from './simulation.js';
import { commodities, regionalSpecialization } from './data.js';

class App {
    constructor() {
        this.simulation = new EconomicSimulation();
        this.currentRegion = 'north-america';
        this.tickInterval = null;
        
        this.initializeUI();
        this.startSimulation();
    }

    initializeUI() {
        // Time controls
        document.getElementById('pauseBtn').addEventListener('click', () => {
            const isPaused = this.simulation.togglePause();
            document.getElementById('pauseBtn').innerHTML = isPaused ? '▶ Resume' : '⏸ Pause';
        });

        document.getElementById('speedBtn').addEventListener('click', () => {
            const speed = this.simulation.toggleSpeed();
            document.getElementById('speedBtn').innerHTML = speed === 1 ? '⏩ 2x Speed' : '⏩ 1x Speed';
        });

        // Region selector
        document.querySelectorAll('.region-btn').forEach(btn => {
            btn.addEventListener('click', (e) => {
                document.querySelectorAll('.region-btn').forEach(b => b.classList.remove('active'));
                e.target.classList.add('active');
                this.currentRegion = e.target.dataset.region;
                this.updatePriceComparison();
            });
        });

        // Trade calculator
        document.getElementById('calculateBtn').addEventListener('click', () => {
            this.calculateRoute();
        });

        // Initialize displays
        this.updateMarketGrid();
        this.updatePriceComparison();
    }

    startSimulation() {
        // Update every second (represents 1 game minute at 1x speed)
        this.tickInterval = setInterval(() => {
            this.simulation.tick();
            this.updateUI();
        }, 1000);
    }

    updateUI() {
        // Update time display
        document.getElementById('gameTime').textContent = this.simulation.getTimeString();
        
        // Update statistics
        document.getElementById('totalTrades').textContent = this.simulation.statistics.totalTrades;
        document.getElementById('totalVolume').textContent = 
            this.simulation.statistics.totalVolume.toLocaleString();
        document.getElementById('activeShips').textContent = this.simulation.statistics.activeShips;
        document.getElementById('avgPrice').textContent = 
            this.simulation.statistics.avgPriceChange + '%';
        
        // Update market displays every 10 seconds
        if (Math.floor(this.simulation.gameTime) % 10 === 0) {
            this.updateMarketGrid();
            this.updatePriceComparison();
        }
    }

    updateMarketGrid() {
        const gridElement = document.getElementById('marketGrid');
        const prices = this.simulation.getAllCommodityPrices('north-america');
        
        let html = '';
        Object.values(prices).forEach(item => {
            const changeClass = item.change > 0 ? 'up' : (item.change < 0 ? 'down' : '');
            const changeSymbol = item.change > 0 ? '↑' : (item.change < 0 ? '↓' : '');
            
            html += `
                <div class="commodity-card">
                    <div class="name">${item.name}</div>
                    <div class="category">${item.category}</div>
                    <div class="price">${item.price} cr</div>
                    <div class="change ${changeClass}">
                        ${changeSymbol} ${Math.abs(item.change)}%
                    </div>
                </div>
            `;
        });
        
        gridElement.innerHTML = html;
    }

    updatePriceComparison() {
        const comparisonElement = document.getElementById('priceComparison');
        const regionalPrices = this.simulation.getAllCommodityPrices(this.currentRegion);
        const globalPrices = this.simulation.getAllCommodityPrices('north-america');
        
        let html = '';
        Object.entries(regionalPrices).forEach(([id, item]) => {
            const globalPrice = globalPrices[id].price;
            const diff = ((item.price - globalPrice) / globalPrice * 100).toFixed(1);
            const vsGlobal = diff > 0 ? 
                `<span style="color: #f56565">+${diff}%</span>` : 
                `<span style="color: #48bb78">${diff}%</span>`;
            
            html += `
                <div class="price-item">
                    <div class="commodity-name">${item.name}</div>
                    <div class="price-value">${item.price} cr</div>
                    <div class="vs-global">vs Global: ${vsGlobal}</div>
                </div>
            `;
        });
        
        comparisonElement.innerHTML = html;
    }

    calculateRoute() {
        const shipType = document.getElementById('shipType').value;
        const origin = document.getElementById('originRegion').value;
        const destination = document.getElementById('destRegion').value;
        const commodity = document.getElementById('commodity').value;
        
        const result = this.simulation.calculateTradeProfit(
            shipType, origin, destination, commodity
        );
        
        const resultsElement = document.getElementById('routeResults');
        resultsElement.classList.add('show');
        
        const profitClass = result.isProfitable ? 'profit-positive' : 'profit-negative';
        
        resultsElement.innerHTML = `
            <h3>Trade Route Analysis</h3>
            <div class="result-row">
                <span class="result-label">Ship:</span>
                <span class="result-value">${result.ship}</span>
            </div>
            <div class="result-row">
                <span class="result-label">Commodity:</span>
                <span class="result-value">${result.commodity}</span>
            </div>
            <div class="result-row">
                <span class="result-label">Route:</span>
                <span class="result-value">${result.origin} → ${result.destination}</span>
            </div>
            <div class="result-row">
                <span class="result-label">Distance:</span>
                <span class="result-value">${result.distance} km</span>
            </div>
            <div class="result-row">
                <span class="result-label">Travel Time:</span>
                <span class="result-value">${result.travelTime} hours</span>
            </div>
            <div class="result-row">
                <span class="result-label">Buy Price:</span>
                <span class="result-value">${result.buyPrice} cr/unit</span>
            </div>
            <div class="result-row">
                <span class="result-label">Sell Price:</span>
                <span class="result-value">${result.sellPrice} cr/unit</span>
            </div>
            <div class="result-row">
                <span class="result-label">Max Cargo:</span>
                <span class="result-value">${result.maxCargo} units</span>
            </div>
            <div class="result-row">
                <span class="result-label">Revenue:</span>
                <span class="result-value">${result.revenue.toLocaleString()} cr</span>
            </div>
            <div class="result-row">
                <span class="result-label">Costs (goods + fuel):</span>
                <span class="result-value">${result.costs.toLocaleString()} cr</span>
            </div>
            <div class="result-row">
                <span class="result-label">Fuel Cost:</span>
                <span class="result-value">${result.travelCost} cr</span>
            </div>
            <hr style="margin: 15px 0; border: none; border-top: 2px solid #e2e8f0;">
            <div class="result-row">
                <span class="result-label">Net Profit:</span>
                <span class="result-value ${profitClass}">
                    ${result.profit.toLocaleString()} cr (${result.profitMargin}%)
                </span>
            </div>
        `;
        
        // Draw trade route on map
        this.drawTradeRoute(origin, destination, result.isProfitable);
    }

    drawTradeRoute(originId, destId, isProfitable) {
        const origin = regionalSpecialization[originId];
        const dest = regionalSpecialization[destId];
        
        const routesGroup = document.getElementById('tradeRoutes');
        routesGroup.innerHTML = ''; // Clear previous routes
        
        const path = document.createElementNS('http://www.w3.org/2000/svg', 'path');
        const d = `M ${origin.position.x} ${origin.position.y} L ${dest.position.x} ${dest.position.y}`;
        path.setAttribute('d', d);
        path.setAttribute('class', isProfitable ? 'trade-route' : 'trade-route unprofitable');
        
        routesGroup.appendChild(path);
    }
}

// Start the application when DOM is ready
document.addEventListener('DOMContentLoaded', () => {
    new App();
});
