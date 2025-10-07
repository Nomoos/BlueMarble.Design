// Multi-Step Swap Router - Inspired by 1inch routing algorithm
// Finds optimal paths for commodity exchanges through intermediate markets

export class SwapRouter {
    constructor(markets) {
        this.markets = markets;
        this.exchangeGraph = new Map();
        this.buildExchangeGraph();
    }

    /**
     * Build a graph representing possible commodity exchanges
     * Each edge represents a potential trade with its cost (fees + slippage)
     * Only creates edges for valid market pairs (respects market structure)
     */
    buildExchangeGraph() {
        // For each market region
        Object.keys(this.markets).forEach(regionId => {
            const market = this.markets[regionId];
            
            // For each commodity in the market
            market.commodityData.forEach((data, commodityId) => {
                if (!this.exchangeGraph.has(commodityId)) {
                    this.exchangeGraph.set(commodityId, new Map());
                }
                
                // Check if market has getMarketPairsFor method (respects market pairs)
                if (typeof market.getMarketPairsFor === 'function') {
                    const marketPairs = data.marketPairs || market.getMarketPairsFor(commodityId);
                    
                    // Only create edges for valid market pairs
                    marketPairs.forEach(pair => {
                        const targetCommodityId = pair.partner;
                        const targetData = market.commodityData.get(targetCommodityId);
                        
                        if (targetData) {
                            const cost = this.calculateExchangeCost(
                                commodityId, 
                                targetCommodityId, 
                                market
                            );
                            
                            // Store the edge with market context
                            const edges = this.exchangeGraph.get(commodityId);
                            if (!edges.has(targetCommodityId) || cost < edges.get(targetCommodityId).cost) {
                                edges.set(targetCommodityId, {
                                    cost,
                                    market: regionId,
                                    buyPrice: market.getPrice(commodityId),
                                    sellPrice: market.getPrice(targetCommodityId),
                                    volume: pair.volume,
                                    spread: pair.spread
                                });
                            }
                        }
                    });
                } else {
                    // Fallback: can exchange to any other commodity in the same market
                    // (for markets that don't specify pairs)
                    market.commodityData.forEach((targetData, targetCommodityId) => {
                        if (commodityId !== targetCommodityId) {
                            const cost = this.calculateExchangeCost(
                                commodityId, 
                                targetCommodityId, 
                                market
                            );
                            
                            const edges = this.exchangeGraph.get(commodityId);
                            if (!edges.has(targetCommodityId) || cost < edges.get(targetCommodityId).cost) {
                                edges.set(targetCommodityId, {
                                    cost,
                                    market: regionId,
                                    buyPrice: market.getPrice(commodityId),
                                    sellPrice: market.getPrice(targetCommodityId)
                                });
                            }
                        }
                    });
                }
            });
        });
    }

    /**
     * Calculate the cost of exchanging one commodity for another
     * Includes transaction fees, slippage, market inefficiency, auctioneer fees,
     * transport costs, guard fees, and deterioration
     */
    calculateExchangeCost(fromCommodity, toCommodity, market, options = {}) {
        const buyPrice = market.getPrice(fromCommodity);
        const sellPrice = market.getPrice(toCommodity);
        
        // Exchange rate (how much of 'to' you get per unit of 'from')
        const exchangeRate = buyPrice / sellPrice;
        
        // Base transaction fee (2.5% of transaction value)
        const baseFeeRate = 0.025;
        
        // Auctioneer fee (ALWAYS applied) - varies by market tier
        const marketTier = options.marketTier || 'regional';
        const playerRace = options.playerRace || 'native-inhabitants';
        const auctioneerFeeRate = this.getAuctioneerFeeRate(marketTier, playerRace);
        
        // Slippage based on market liquidity (simulated)
        const fromData = market.commodityData.get(fromCommodity);
        const toData = market.commodityData.get(toCommodity);
        const avgLiquidity = (fromData.supply + toData.supply) / 2;
        const slippage = Math.max(0.01, Math.min(0.05, 100 / avgLiquidity)); // 1-5%
        
        // Transport fee (for inter-market trades)
        const transportFeeRate = options.includeTransport ? 0.01 : 0;
        
        // Guard fee (for security during transport)
        const guardTier = options.guardTier || 'none';
        const guardFeeRate = this.getGuardFeeRate(guardTier);
        
        // Deterioration/spoilage factor (for perishable goods)
        const season = options.season || 'summer';
        const preservation = options.preservation || 'none';
        const deteriorationRate = this.getDeteriorationRate(fromCommodity, season, preservation);
        
        // Total cost factor (lower is better)
        // Cost = 1 / (rate after all fees, slippage, and deterioration)
        const effectiveRate = exchangeRate * 
                             (1 - baseFeeRate) * 
                             (1 - auctioneerFeeRate) * 
                             (1 - slippage) * 
                             (1 - transportFeeRate) *
                             (1 - guardFeeRate) *
                             (1 - deteriorationRate);
        
        return 1 / effectiveRate;
    }
    
    /**
     * Get auctioneer fee rate based on market tier and player race
     */
    getAuctioneerFeeRate(marketTier, playerRace) {
        const baseFees = {
            'local': 0.015,      // 1.5%
            'regional': 0.03,    // 3%
            'global': 0.07       // 7%
        };
        
        const raceFeeMultipliers = {
            'native-inhabitants': 1.0,
            'established-settlers': 1.2,
            'experimental-race-1': 1.5,
            'experimental-race-2': 2.0  // Higher fees for races experimenting with this world
        };
        
        const baseFee = baseFees[marketTier] || 0.03;
        const raceMultiplier = raceFeeMultipliers[playerRace] || 1.0;
        
        return baseFee * raceMultiplier;
    }
    
    /**
     * Get guard fee rate based on security tier
     */
    getGuardFeeRate(guardTier) {
        const guardFees = {
            'none': 0,
            'basic': 0.005,      // 0.5% - basic guards
            'standard': 0.01,    // 1% - professional guards
            'premium': 0.02      // 2% - military escort
        };
        
        return guardFees[guardTier] || 0;
    }
    
    /**
     * Get deterioration rate for commodity based on season and preservation
     */
    getDeteriorationRate(commodityId, season, preservation) {
        // Simplified deterioration model
        const commodityTypes = {
            'wood': { baseRate: 0.001, perishable: false },
            'cherry': { baseRate: 0.05, perishable: true },
            'iron': { baseRate: 0, perishable: false },
            'strawberry': { baseRate: 0.04, perishable: true },
            'carrot': { baseRate: 0.03, perishable: true },
            'blueberry': { baseRate: 0.045, perishable: true },
            'game-currency': { baseRate: 0, perishable: false }
        };
        
        const commodity = commodityTypes[commodityId] || { baseRate: 0, perishable: false };
        
        if (!commodity.perishable) {
            return 0;
        }
        
        // Seasonal multipliers
        const seasonMultipliers = {
            'spring': 0.8,
            'summer': 1.3,  // Faster decay in heat
            'autumn': 0.9,
            'winter': 0.5   // Slower in cold
        };
        
        // Preservation multipliers
        const preservationMultipliers = {
            'none': 1.0,
            'drying': 0.2,
            'salting': 0.3,
            'smoking': 0.25,
            'canning': 0.05
        };
        
        const seasonMult = seasonMultipliers[season] || 1.0;
        const preservationMult = preservationMultipliers[preservation] || 1.0;
        
        return commodity.baseRate * seasonMult * preservationMult;
    }

    /**
     * Find the optimal path to exchange fromCommodity to toCommodity
     * Uses Dijkstra's algorithm to find the lowest-cost path
     * 
     * @param {string} fromCommodity - Source commodity ID
     * @param {string} toCommodity - Target commodity ID
     * @param {number} amount - Amount of source commodity to exchange
     * @returns {Object} - Optimal route with path, total cost, and comparison to direct route
     */
    findOptimalRoute(fromCommodity, toCommodity, amount = 1) {
        // Check for self-exchange
        if (fromCommodity === toCommodity) {
            return {
                success: false,
                error: 'Cannot exchange a commodity with itself'
            };
        }
        
        // Check if both commodities exist in the graph
        if (!this.exchangeGraph.has(fromCommodity) || !this.exchangeGraph.has(toCommodity)) {
            return {
                success: false,
                error: 'One or both commodities not found in any market'
            };
        }

        // Dijkstra's algorithm implementation
        const distances = new Map();
        const previous = new Map();
        const unvisited = new Set();
        const edgeDetails = new Map(); // Store edge details for path reconstruction

        // Initialize
        this.exchangeGraph.forEach((edges, nodeId) => {
            distances.set(nodeId, Infinity);
            previous.set(nodeId, null);
            unvisited.add(nodeId);
        });
        distances.set(fromCommodity, 0);

        while (unvisited.size > 0) {
            // Find unvisited node with minimum distance
            let currentNode = null;
            let minDistance = Infinity;
            unvisited.forEach(node => {
                const dist = distances.get(node);
                if (dist < minDistance) {
                    minDistance = dist;
                    currentNode = node;
                }
            });

            if (currentNode === null || minDistance === Infinity) {
                break; // No path exists
            }

            // Found the target
            if (currentNode === toCommodity) {
                break;
            }

            unvisited.delete(currentNode);

            // Check all neighbors
            const edges = this.exchangeGraph.get(currentNode);
            if (edges) {
                edges.forEach((edgeData, neighbor) => {
                    if (unvisited.has(neighbor)) {
                        const altDistance = distances.get(currentNode) + edgeData.cost;
                        if (altDistance < distances.get(neighbor)) {
                            distances.set(neighbor, altDistance);
                            previous.set(neighbor, currentNode);
                            // Store edge details for this path
                            edgeDetails.set(neighbor, edgeData);
                        }
                    }
                });
            }
        }

        // Reconstruct path
        const path = [];
        const pathDetails = [];
        let current = toCommodity;
        
        while (current !== null) {
            path.unshift(current);
            if (previous.get(current) !== null) {
                const edgeData = edgeDetails.get(current);
                pathDetails.unshift({
                    from: previous.get(current),
                    to: current,
                    ...edgeData
                });
            }
            current = previous.get(current);
        }

        // Check if path was found
        if (path.length === 0 || path[0] !== fromCommodity) {
            return {
                success: false,
                error: 'No exchange path found between commodities'
            };
        }

        // Calculate total cost and output amount
        const totalCost = distances.get(toCommodity);
        const outputAmount = amount / totalCost;

        // Calculate direct route for comparison (if it exists)
        let directRoute = null;
        const directEdges = this.exchangeGraph.get(fromCommodity);
        if (directEdges && directEdges.has(toCommodity)) {
            const directEdge = directEdges.get(toCommodity);
            const directOutputAmount = amount / directEdge.cost;
            directRoute = {
                path: [fromCommodity, toCommodity],
                cost: directEdge.cost,
                outputAmount: directOutputAmount,
                steps: [{
                    from: fromCommodity,
                    to: toCommodity,
                    ...directEdge
                }]
            };
        }

        // Determine if multi-step route is better
        const isMultiStepBetter = !directRoute || outputAmount > directRoute.outputAmount;
        const improvement = directRoute 
            ? ((outputAmount - directRoute.outputAmount) / directRoute.outputAmount * 100).toFixed(2)
            : 'N/A';

        return {
            success: true,
            from: fromCommodity,
            to: toCommodity,
            inputAmount: amount,
            optimalRoute: {
                path,
                pathDetails,
                totalCost,
                outputAmount,
                steps: path.length - 1
            },
            directRoute,
            comparison: {
                useMultiStep: isMultiStepBetter,
                improvement: improvement + '%',
                reason: isMultiStepBetter 
                    ? (directRoute ? 'Multi-step route is cheaper' : 'No direct route available')
                    : 'Direct route is better'
            }
        };
    }

    /**
     * Find all viable routes (for comparison and display)
     * Limited to routes with max 3 hops to prevent excessive computation
     */
    findAllRoutes(fromCommodity, toCommodity, maxHops = 3) {
        const routes = [];
        const visited = new Set();

        const dfs = (current, target, path, costSoFar, depth) => {
            if (depth > maxHops) return;
            
            if (current === target && path.length > 1) {
                routes.push({
                    path: [...path],
                    totalCost: costSoFar,
                    outputAmount: 1 / costSoFar,
                    steps: path.length - 1
                });
                return;
            }

            visited.add(current);
            const edges = this.exchangeGraph.get(current);
            
            if (edges) {
                edges.forEach((edgeData, neighbor) => {
                    if (!visited.has(neighbor)) {
                        path.push(neighbor);
                        dfs(neighbor, target, path, costSoFar + edgeData.cost, depth + 1);
                        path.pop();
                    }
                });
            }
            
            visited.delete(current);
        };

        dfs(fromCommodity, toCommodity, [fromCommodity], 0, 0);

        // Sort by output amount (descending - more output is better)
        return routes.sort((a, b) => b.outputAmount - a.outputAmount);
    }

    /**
     * Get market depth information for a commodity pair
     * Useful for determining if a route is actually executable
     */
    getMarketDepth(commodityId, marketId) {
        const market = this.markets[marketId];
        if (!market) return null;

        const data = market.commodityData.get(commodityId);
        if (!data) return null;

        return {
            supply: data.supply,
            demand: data.demand,
            price: data.currentPrice,
            liquidity: (data.supply + data.demand) / 2
        };
    }

    /**
     * Refresh the exchange graph when markets update
     */
    refresh() {
        this.exchangeGraph.clear();
        this.buildExchangeGraph();
    }
}
