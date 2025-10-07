# Trade and Commerce Systems

**Version:** 1.0  
**Date:** 2025-01-06  
**Status:** Design Specification

## Overview

The Trade and Commerce System (Obchod) provides comprehensive mechanics for economic exchange between players and with NPC entities. This system emphasizes secure transactions, dynamic market pricing, regional trade networks, and authentic economic simulation.

## Core Design Philosophy

### Player-Driven Economy

Market dynamics emerge from player actions:
- Supply and demand determine prices
- Player production drives availability
- Trade routes emerge organically
- Economic specialization creates interdependence

### Security and Trust

All transactions are protected:
- Secure trade windows prevent scams
- Escrow systems for complex deals
- Contract enforcement mechanisms
- Dispute resolution procedures

### Geographic Economics

Location matters for trade:
- Transportation costs affect profitability
- Regional scarcity drives price variations
- Infrastructure quality impacts trade efficiency
- Market access influences economic viability

## System Components

### 1. Direct Player Trading

#### Trade Window System

```csharp
public class TradeSession
{
    public Player Player1 { get; set; }
    public Player Player2 { get; set; }
    public TradeOffer Offer1 { get; set; }
    public TradeOffer Offer2 { get; set; }
    public TradeState State { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ExpiresAt { get; set; }
    
    public void ProposeOffer(Player player, TradeOffer offer)
    {
        if (player == Player1)
        {
            Offer1 = offer;
            State = TradeState.Player1Offered;
        }
        else if (player == Player2)
        {
            Offer2 = offer;
            State = TradeState.Player2Offered;
        }
        
        // Check if both have offered
        if (Offer1 != null && Offer2 != null)
        {
            State = TradeState.BothOffered;
        }
    }
    
    public bool ConfirmTrade(Player player)
    {
        if (State != TradeState.BothOffered)
            return false;
            
        // Mark player as confirmed
        if (player == Player1)
            Offer1.Confirmed = true;
        else if (player == Player2)
            Offer2.Confirmed = true;
        else
            return false;
            
        // Execute if both confirmed
        if (Offer1.Confirmed && Offer2.Confirmed)
        {
            return ExecuteTrade();
        }
        
        return true;
    }
    
    private bool ExecuteTrade()
    {
        // Validate both offers are still valid
        if (!ValidateOffer(Offer1, Player1) || !ValidateOffer(Offer2, Player2))
        {
            State = TradeState.Failed;
            return false;
        }
        
        // Execute atomic transfer
        using (var transaction = BeginTransaction())
        {
            try
            {
                // Transfer items from Player1 to Player2
                TransferItems(Player1, Player2, Offer1.Items);
                TransferCurrency(Player1, Player2, Offer1.Currency);
                
                // Transfer items from Player2 to Player1
                TransferItems(Player2, Player1, Offer2.Items);
                TransferCurrency(Player2, Player1, Offer2.Currency);
                
                // Log transaction
                LogTransaction(Player1, Player2, Offer1, Offer2);
                
                transaction.Commit();
                State = TradeState.Completed;
                return true;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                State = TradeState.Failed;
                LogError(ex);
                return false;
            }
        }
    }
    
    private bool ValidateOffer(TradeOffer offer, Player player)
    {
        // Verify player still has all items
        foreach (var item in offer.Items)
        {
            if (!player.Inventory.Has(item.ItemId, item.Quantity))
                return false;
        }
        
        // Verify player has currency
        if (offer.Currency > 0 && player.Currency < offer.Currency)
            return false;
            
        return true;
    }
}

public class TradeOffer
{
    public List<ItemStack> Items { get; set; }
    public int Currency { get; set; }
    public bool Confirmed { get; set; }
}

public enum TradeState
{
    Proposed,
    Player1Offered,
    Player2Offered,
    BothOffered,
    Completed,
    Cancelled,
    Failed
}
```

### 2. Market System

#### Regional Markets

```csharp
public class RegionalMarket
{
    public string MarketId { get; set; }
    public Coordinate3D Location { get; set; }
    public MarketType Type { get; set; }
    public List<MarketOrder> BuyOrders { get; set; }
    public List<MarketOrder> SellOrders { get; set; }
    public PriceHistory History { get; set; }
    
    public MarketOrder PlaceSellOrder(
        Player seller,
        ItemType item,
        int quantity,
        float pricePerUnit,
        TimeSpan duration)
    {
        // Validate seller has items
        if (!seller.Inventory.Has(item, quantity))
            throw new InvalidOperationException("Insufficient items");
            
        // Escrow items
        seller.Inventory.Remove(item, quantity);
        
        var order = new MarketOrder
        {
            OrderId = GenerateOrderId(),
            Type = OrderType.Sell,
            Seller = seller,
            Item = item,
            Quantity = quantity,
            PricePerUnit = pricePerUnit,
            CreatedAt = DateTime.UtcNow,
            ExpiresAt = DateTime.UtcNow + duration,
            Status = OrderStatus.Active
        };
        
        SellOrders.Add(order);
        
        // Try to match with existing buy orders
        TryMatchOrders(order);
        
        return order;
    }
    
    public MarketOrder PlaceBuyOrder(
        Player buyer,
        ItemType item,
        int quantity,
        float pricePerUnit,
        TimeSpan duration)
    {
        float totalCost = quantity * pricePerUnit;
        
        // Validate buyer has currency
        if (buyer.Currency < totalCost)
            throw new InvalidOperationException("Insufficient funds");
            
        // Escrow currency
        buyer.Currency -= totalCost;
        
        var order = new MarketOrder
        {
            OrderId = GenerateOrderId(),
            Type = OrderType.Buy,
            Buyer = buyer,
            Item = item,
            Quantity = quantity,
            PricePerUnit = pricePerUnit,
            EscrowedCurrency = totalCost,
            CreatedAt = DateTime.UtcNow,
            ExpiresAt = DateTime.UtcNow + duration,
            Status = OrderStatus.Active
        };
        
        BuyOrders.Add(order);
        
        // Try to match with existing sell orders
        TryMatchOrders(order);
        
        return order;
    }
    
    private void TryMatchOrders(MarketOrder newOrder)
    {
        if (newOrder.Type == OrderType.Buy)
        {
            // Match buy order with sell orders
            var matchingSells = SellOrders
                .Where(o => o.Item == newOrder.Item && 
                           o.PricePerUnit <= newOrder.PricePerUnit &&
                           o.Status == OrderStatus.Active)
                .OrderBy(o => o.PricePerUnit)
                .ThenBy(o => o.CreatedAt);
                
            foreach (var sellOrder in matchingSells)
            {
                if (newOrder.Quantity == 0)
                    break;
                    
                ExecuteOrderMatch(newOrder, sellOrder);
            }
        }
        else
        {
            // Match sell order with buy orders
            var matchingBuys = BuyOrders
                .Where(o => o.Item == newOrder.Item && 
                           o.PricePerUnit >= newOrder.PricePerUnit &&
                           o.Status == OrderStatus.Active)
                .OrderByDescending(o => o.PricePerUnit)
                .ThenBy(o => o.CreatedAt);
                
            foreach (var buyOrder in matchingBuys)
            {
                if (newOrder.Quantity == 0)
                    break;
                    
                ExecuteOrderMatch(buyOrder, newOrder);
            }
        }
    }
    
    private void ExecuteOrderMatch(MarketOrder buyOrder, MarketOrder sellOrder)
    {
        // Determine quantity to trade
        int tradeQuantity = Math.Min(buyOrder.Quantity, sellOrder.Quantity);
        
        // Use buyer's price (they offered more)
        float tradePrice = buyOrder.PricePerUnit;
        float totalPrice = tradeQuantity * tradePrice;
        
        // Transfer items
        buyOrder.Buyer.Inventory.Add(sellOrder.Item, tradeQuantity);
        
        // Transfer currency
        sellOrder.Seller.Currency += totalPrice;
        
        // Return excess escrow to buyer
        float excessEscrow = (buyOrder.PricePerUnit - tradePrice) * tradeQuantity;
        buyOrder.Buyer.Currency += excessEscrow;
        
        // Update order quantities
        buyOrder.Quantity -= tradeQuantity;
        sellOrder.Quantity -= tradeQuantity;
        buyOrder.EscrowedCurrency -= buyOrder.PricePerUnit * tradeQuantity;
        
        // Update order status
        if (buyOrder.Quantity == 0)
            buyOrder.Status = OrderStatus.Filled;
        if (sellOrder.Quantity == 0)
            sellOrder.Status = OrderStatus.Filled;
            
        // Record transaction
        RecordTransaction(buyOrder, sellOrder, tradeQuantity, tradePrice);
        
        // Update price history
        History.RecordPrice(sellOrder.Item, tradePrice, tradeQuantity, DateTime.UtcNow);
    }
}

public class MarketOrder
{
    public string OrderId { get; set; }
    public OrderType Type { get; set; }
    public Player Seller { get; set; }
    public Player Buyer { get; set; }
    public ItemType Item { get; set; }
    public int Quantity { get; set; }
    public float PricePerUnit { get; set; }
    public float EscrowedCurrency { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime ExpiresAt { get; set; }
    public OrderStatus Status { get; set; }
}

public enum OrderType { Buy, Sell }
public enum OrderStatus { Active, Filled, Cancelled, Expired }
```

#### Price Discovery

```csharp
public class PriceCalculator
{
    public float CalculateMarketPrice(
        ItemType item,
        RegionalMarket market,
        SupplyDemandData supplyDemand)
    {
        // Get historical average
        float historicalAverage = market.History.GetAveragePrice(item, TimeSpan.FromDays(30));
        
        // Get current supply and demand
        int supply = market.SellOrders
            .Where(o => o.Item == item && o.Status == OrderStatus.Active)
            .Sum(o => o.Quantity);
            
        int demand = market.BuyOrders
            .Where(o => o.Item == item && o.Status == OrderStatus.Active)
            .Sum(o => o.Quantity);
            
        // Calculate supply/demand ratio
        float ratio = demand / Math.Max(1f, supply);
        
        // Apply elasticity
        float elasticity = GetPriceElasticity(item);
        float priceMultiplier = 1.0f + (ratio - 1.0f) * elasticity;
        
        // Calculate current market price
        float marketPrice = historicalAverage * priceMultiplier;
        
        // Apply regional modifiers
        marketPrice *= GetRegionalModifier(item, market.Location);
        
        return marketPrice;
    }
    
    private float GetRegionalModifier(ItemType item, Coordinate3D location)
    {
        // Scarcity modifier
        float scarcity = CalculateRegionalScarcity(item, location);
        
        // Transportation cost from nearest production
        float transportCost = CalculateTransportationCost(item, location);
        
        return (1.0f + scarcity * 0.5f) * (1.0f + transportCost);
    }
}
```

### 3. Contract System

#### Trade Contracts

```csharp
public class TradeContract
{
    public string ContractId { get; set; }
    public ContractType Type { get; set; }
    public Player Issuer { get; set; }
    public Player Contractor { get; set; }
    public ContractTerms Terms { get; set; }
    public ContractStatus Status { get; set; }
    public float Collateral { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime ExpiresAt { get; set; }
    
    public static TradeContract CreateItemExchange(
        Player issuer,
        List<ItemStack> offeredItems,
        List<ItemStack> requestedItems,
        TimeSpan duration)
    {
        var contract = new TradeContract
        {
            ContractId = GenerateContractId(),
            Type = ContractType.ItemExchange,
            Issuer = issuer,
            Terms = new ContractTerms
            {
                OfferedItems = offeredItems,
                RequestedItems = requestedItems
            },
            Status = ContractStatus.Open,
            CreatedAt = DateTime.UtcNow,
            ExpiresAt = DateTime.UtcNow + duration
        };
        
        // Escrow offered items
        foreach (var item in offeredItems)
        {
            issuer.Inventory.Remove(item.ItemType, item.Quantity);
        }
        
        return contract;
    }
    
    public static TradeContract CreateCourierContract(
        Player issuer,
        List<ItemStack> items,
        Coordinate3D destination,
        float reward,
        float collateral,
        TimeSpan duration)
    {
        var contract = new TradeContract
        {
            ContractId = GenerateContractId(),
            Type = ContractType.Courier,
            Issuer = issuer,
            Terms = new ContractTerms
            {
                DeliveryItems = items,
                Destination = destination,
                Reward = reward,
                RequiredCollateral = collateral
            },
            Status = ContractStatus.Open,
            CreatedAt = DateTime.UtcNow,
            ExpiresAt = DateTime.UtcNow + duration
        };
        
        // Escrow items and reward
        foreach (var item in items)
        {
            issuer.Inventory.Remove(item.ItemType, item.Quantity);
        }
        issuer.Currency -= reward;
        
        return contract;
    }
    
    public bool AcceptContract(Player contractor)
    {
        if (Status != ContractStatus.Open)
            return false;
            
        // Validate collateral if required
        if (Terms.RequiredCollateral > 0)
        {
            if (contractor.Currency < Terms.RequiredCollateral)
                return false;
                
            // Escrow collateral
            contractor.Currency -= Terms.RequiredCollateral;
            Collateral = Terms.RequiredCollateral;
        }
        
        Contractor = contractor;
        Status = ContractStatus.InProgress;
        
        // Transfer items for courier contracts
        if (Type == ContractType.Courier)
        {
            foreach (var item in Terms.DeliveryItems)
            {
                contractor.Inventory.Add(item.ItemType, item.Quantity);
            }
        }
        
        return true;
    }
    
    public bool CompleteContract()
    {
        if (Status != ContractStatus.InProgress)
            return false;
            
        bool success = false;
        
        switch (Type)
        {
            case ContractType.ItemExchange:
                success = CompleteItemExchange();
                break;
                
            case ContractType.Courier:
                success = CompleteCourier();
                break;
                
            case ContractType.Auction:
                success = CompleteAuction();
                break;
        }
        
        if (success)
        {
            Status = ContractStatus.Completed;
            
            // Return collateral
            if (Collateral > 0)
            {
                Contractor.Currency += Collateral;
                Collateral = 0;
            }
        }
        
        return success;
    }
    
    private bool CompleteCourier()
    {
        // Verify contractor is at destination
        float distance = CalculateDistance(Contractor.Location, Terms.Destination);
        if (distance > 100)  // 100m tolerance
            return false;
            
        // Verify contractor still has items
        foreach (var item in Terms.DeliveryItems)
        {
            if (!Contractor.Inventory.Has(item.ItemType, item.Quantity))
                return false;
        }
        
        // Transfer items to issuer
        foreach (var item in Terms.DeliveryItems)
        {
            Contractor.Inventory.Remove(item.ItemType, item.Quantity);
            Issuer.Inventory.Add(item.ItemType, item.Quantity);
        }
        
        // Pay reward
        Contractor.Currency += Terms.Reward;
        
        return true;
    }
}

public enum ContractType
{
    ItemExchange,
    Courier,
    Auction,
    Service
}

public enum ContractStatus
{
    Open,
    InProgress,
    Completed,
    Cancelled,
    Failed,
    Expired
}
```

### 4. NPC Trading

#### NPC Merchants

```csharp
public class NPCMerchant
{
    public string MerchantId { get; set; }
    public string Name { get; set; }
    public MerchantType Type { get; set; }
    public Coordinate3D Location { get; set; }
    public Inventory Stock { get; set; }
    public float CurrencyReserve { get; set; }
    public PricingStrategy Pricing { get; set; }
    
    public TradeOffer GetBuyPrice(ItemType item, int quantity)
    {
        // Check if merchant buys this type
        if (!Pricing.WillBuy(item))
            return null;
            
        // Calculate base price
        float basePrice = Pricing.GetBaseBuyPrice(item);
        
        // Apply quantity discount (merchants pay less for bulk)
        float quantityModifier = 1.0f - (quantity / 1000f) * 0.2f;  // Up to 20% less
        quantityModifier = Math.Max(0.5f, quantityModifier);
        
        // Apply stock level modifier (pay more if low stock)
        int currentStock = Stock.GetQuantity(item);
        float stockModifier = currentStock < 10 ? 1.2f : 1.0f;
        
        float pricePerUnit = basePrice * quantityModifier * stockModifier;
        
        return new TradeOffer
        {
            Items = new List<ItemStack> { new ItemStack(item, quantity) },
            Currency = (int)(pricePerUnit * quantity)
        };
    }
    
    public TradeOffer GetSellPrice(ItemType item, int quantity)
    {
        // Check if merchant has stock
        if (Stock.GetQuantity(item) < quantity)
            return null;
            
        // Calculate base price
        float basePrice = Pricing.GetBaseSellPrice(item);
        
        // Apply markup
        float markup = Pricing.GetMarkup(item);
        
        // Apply scarcity modifier
        int currentStock = Stock.GetQuantity(item);
        float scarcityModifier = currentStock < 10 ? 1.5f : 1.0f;
        
        float pricePerUnit = basePrice * markup * scarcityModifier;
        
        return new TradeOffer
        {
            Items = new List<ItemStack> { new ItemStack(item, quantity) },
            Currency = (int)(pricePerUnit * quantity)
        };
    }
    
    public void RestockInventory(TimeSpan timePassed)
    {
        // Merchants gradually restock based on type
        foreach (var itemType in Pricing.StockedItems)
        {
            int currentStock = Stock.GetQuantity(itemType);
            int targetStock = Pricing.GetTargetStock(itemType);
            
            if (currentStock < targetStock)
            {
                // Restock rate depends on merchant type
                int restockAmount = (int)(timePassed.TotalHours * Pricing.RestockRate);
                int toAdd = Math.Min(restockAmount, targetStock - currentStock);
                
                Stock.Add(itemType, toAdd);
            }
        }
    }
}

public enum MerchantType
{
    GeneralGoods,     // Wide variety, low markup
    Specialist,       // Specific category, high quality
    Blacksmith,       // Weapons and armor
    Alchemist,        // Potions and reagents
    Fletcher,         // Ranged weapons and ammunition
    Jeweler,          // Gems and jewelry
    Provisioner       // Food and supplies
}
```

#### Dynamic NPC Pricing

```csharp
public class NPCPricingStrategy
{
    public float BaseMarkup { get; set; }  // Default: 1.5 (50% markup)
    public Dictionary<ItemType, float> CategoryMarkups { get; set; }
    public float RestockRate { get; set; }
    
    public float GetBaseBuyPrice(ItemType item)
    {
        // Base value from item definition
        float baseValue = item.BaseValue;
        
        // NPCs buy at 40-60% of base value
        return baseValue * 0.5f;
    }
    
    public float GetBaseSellPrice(ItemType item)
    {
        float baseValue = item.BaseValue;
        float markup = GetMarkup(item);
        
        return baseValue * markup;
    }
    
    public float GetMarkup(ItemType item)
    {
        if (CategoryMarkups.TryGetValue(item, out float specificMarkup))
            return specificMarkup;
            
        return BaseMarkup;
    }
}
```

### 5. Trade Routes

#### Transportation System

```csharp
public class TradeRoute
{
    public string RouteId { get; set; }
    public Coordinate3D Origin { get; set; }
    public Coordinate3D Destination { get; set; }
    public List<Waypoint> Waypoints { get; set; }
    public float TotalDistance { get; set; }
    public TransportMethod Method { get; set; }
    public RouteQuality Quality { get; set; }
    
    public float CalculateTransportCost(float weight)
    {
        // Base cost per kg per km
        float baseCostPerKgKm = Method.CostRate;
        
        // Distance cost
        float distanceCost = weight * TotalDistance * baseCostPerKgKm;
        
        // Quality modifier (better routes are cheaper)
        float qualityModifier = Quality switch
        {
            RouteQuality.Poor => 1.5f,
            RouteQuality.Adequate => 1.2f,
            RouteQuality.Good => 1.0f,
            RouteQuality.Excellent => 0.8f,
            _ => 1.0f
        };
        
        // Terrain difficulty
        float terrainModifier = CalculateTerrainDifficulty();
        
        return distanceCost * qualityModifier * terrainModifier;
    }
    
    public TimeSpan CalculateTransportTime(float weight)
    {
        // Base speed depends on method
        float baseSpeed = Method.BaseSpeed;  // km/h
        
        // Weight penalty
        float weightPenalty = weight > Method.OptimalLoad 
            ? 1.0f - ((weight - Method.OptimalLoad) / weight) * 0.3f
            : 1.0f;
            
        // Route quality affects speed
        float qualityModifier = Quality switch
        {
            RouteQuality.Poor => 0.6f,
            RouteQuality.Adequate => 0.8f,
            RouteQuality.Good => 1.0f,
            RouteQuality.Excellent => 1.2f,
            _ => 1.0f
        };
        
        float effectiveSpeed = baseSpeed * weightPenalty * qualityModifier;
        float hours = TotalDistance / effectiveSpeed;
        
        return TimeSpan.FromHours(hours);
    }
}

public class TransportMethod
{
    public string Name { get; set; }
    public float BaseSpeed { get; set; }      // km/h
    public float CostRate { get; set; }       // Currency per kg per km
    public float OptimalLoad { get; set; }    // kg
    public float MaxLoad { get; set; }        // kg
}

public enum RouteQuality
{
    Poor,        // Rough terrain, no infrastructure
    Adequate,    // Basic path
    Good,        // Established road
    Excellent    // Paved road with facilities
}
```

### 6. Trading Companies

#### Player Organizations

```csharp
public class TradingCompany
{
    public string CompanyId { get; set; }
    public string Name { get; set; }
    public Player Owner { get; set; }
    public List<Player> Members { get; set; }
    public List<TradeRoute> EstablishedRoutes { get; set; }
    public Dictionary<Coordinate3D, Warehouse> Warehouses { get; set; }
    public float CompanyFunds { get; set; }
    
    public void EstablishTradeRoute(
        Coordinate3D origin,
        Coordinate3D destination,
        List<ItemType> tradedGoods)
    {
        var route = new TradeRoute
        {
            RouteId = GenerateRouteId(),
            Origin = origin,
            Destination = destination,
            Method = SelectOptimalTransportMethod(origin, destination),
            Waypoints = PlanWaypoints(origin, destination)
        };
        
        route.TotalDistance = CalculateTotalDistance(route.Waypoints);
        route.Quality = AssessRouteQuality(route);
        
        EstablishedRoutes.Add(route);
    }
    
    public void ExecuteTradeRun(TradeRoute route, List<ItemStack> cargo)
    {
        // Calculate costs
        float weight = cargo.Sum(c => c.Weight);
        float cost = route.CalculateTransportCost(weight);
        
        if (CompanyFunds < cost)
            throw new InvalidOperationException("Insufficient company funds");
            
        // Deduct cost
        CompanyFunds -= cost;
        
        // Calculate delivery time
        var deliveryTime = route.CalculateTransportTime(weight);
        
        // Schedule delivery
        ScheduleDelivery(route, cargo, deliveryTime);
    }
}
```

## Economic Balancing

### Market Equilibrium

```csharp
public class MarketBalancing
{
    public void UpdateMarketEquilibrium(RegionalMarket market)
    {
        foreach (var itemType in market.GetTradedItems())
        {
            var supply = CalculateTotalSupply(market, itemType);
            var demand = CalculateTotalDemand(market, itemType);
            
            // Calculate equilibrium price
            float equilibriumPrice = CalculateEquilibriumPrice(
                itemType,
                supply,
                demand,
                market.History
            );
            
            // Update suggested prices
            market.SetSuggestedPrice(itemType, equilibriumPrice);
            
            // Adjust NPC merchant pricing
            AdjustNPCPricing(market, itemType, equilibriumPrice);
        }
    }
    
    private float CalculateEquilibriumPrice(
        ItemType item,
        float supply,
        float demand,
        PriceHistory history)
    {
        float basePrice = item.BaseValue;
        
        // Supply/demand ratio
        float ratio = demand / Math.Max(1f, supply);
        
        // Elasticity
        float elasticity = GetPriceElasticity(item);
        
        // Calculate new price
        float targetPrice = basePrice * (1.0f + (ratio - 1.0f) * elasticity);
        
        // Smooth transition from current price
        float currentPrice = history.GetAveragePrice(item, TimeSpan.FromDays(7));
        float smoothedPrice = currentPrice * 0.8f + targetPrice * 0.2f;
        
        return smoothedPrice;
    }
}
```

## Testing Requirements

### Unit Tests

1. **Trade Validation**: Verify trade security and atomicity
2. **Price Calculations**: Test market price discovery
3. **Contract Execution**: Validate contract fulfillment
4. **Transport Costs**: Verify cost calculations

### Integration Tests

1. **Complete Trade**: Full trade workflow from negotiation to completion
2. **Market Operations**: Order placement and matching
3. **Contract Lifecycle**: Contract creation through completion
4. **NPC Trading**: NPC merchant interactions

### Balance Tests

1. **Market Stability**: Verify price convergence
2. **Trade Profitability**: Validate viable trade opportunities
3. **Transportation Economics**: Test route viability
4. **NPC Pricing**: Ensure competitive NPC prices

## Related Documentation

- [Mining and Resource Extraction](./mining-resource-extraction.md)
- [Building and Construction](./building-construction.md)
- [Terraforming Systems](./terraforming.md)
- [Protection Systems](./anti-exploitation.md)
- [Economy Systems](../../systems/economy-systems.md)
- [Game Mechanics Design](../../GAME_MECHANICS_DESIGN.md)

## Implementation Notes

### Security Considerations

- Atomic transactions prevent item duplication
- Escrow systems prevent scams
- Trade logs enable fraud investigation
- Rate limiting prevents market manipulation

### Performance Considerations

- Cache market prices for frequently traded items
- Optimize order matching algorithms
- Index contracts by various criteria
- Efficient spatial queries for nearby markets

### User Experience

- Clear trade window with item preview
- Visual price history charts
- Trade route profitability calculator
- Contract search and filtering tools
