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
     * Includes transaction fees, slippage, and market inefficiency
     */
    calculateExchangeCost(fromCommodity, toCommodity, market) {
        const buyPrice = market.getPrice(fromCommodity);
        const sellPrice = market.getPrice(toCommodity);
        
        // Exchange rate (how much of 'to' you get per unit of 'from')
        const exchangeRate = buyPrice / sellPrice;
        
        // Transaction fee (2.5% of transaction value)
        const feeRate = 0.025;
        
        // Slippage based on market liquidity (simulated)
        const fromData = market.commodityData.get(fromCommodity);
        const toData = market.commodityData.get(toCommodity);
        const avgLiquidity = (fromData.supply + toData.supply) / 2;
        const slippage = Math.max(0.01, Math.min(0.05, 100 / avgLiquidity)); // 1-5%
        
        // Total cost factor (lower is better)
        // Cost = 1 / (rate after fees and slippage)
        const effectiveRate = exchangeRate * (1 - feeRate) * (1 - slippage);
        
        return 1 / effectiveRate;
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
