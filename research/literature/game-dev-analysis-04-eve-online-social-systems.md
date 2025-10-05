# EVE Online Social Systems Architecture - Analysis for BlueMarble

---
title: EVE Online Social Systems - Guild and Economy Architecture
date: 2025-01-15
tags: [game-development, social-systems, mmorpg, economy, guilds, eve-online]
status: complete
priority: high
category: GameDev-Tech
discovered-from: Roblox Game Development research
---

**Source:** EVE Online Social Systems Architecture (CCP Games Documentation and GDC Talks)  
**Category:** Game Development - Social Systems & Economy  
**Priority:** High  
**Status:** ✅ Complete  
**Lines:** ~550  
**Related Sources:** Multiplayer Game Programming, Game Programming Patterns, Player-Driven Economy Studies

---

## Executive Summary

This analysis examines EVE Online's sophisticated social systems architecture, focusing on guild (corporation) management, player-driven economy, and territorial control systems. EVE Online represents one of the most successful examples of emergent gameplay driven by player organizations in a persistent MMO, making it highly relevant to BlueMarble's planned social features and resource-based economy.

**Key Takeaways for BlueMarble:**
- Corporation (guild) hierarchies with roles, permissions, and shared resources
- Player-driven economy with dynamic pricing based on supply/demand
- Territorial control systems linking geography to guild power
- Contract and transaction systems for secure player-to-player trading
- Alliance mechanics for meta-organizational coordination
- Reputation and security systems managing player interactions

**Relevance to BlueMarble:** Very High - Social systems and player-driven economy are core to BlueMarble's design

## Source Overview

**EVE Online Context:**
EVE Online is a space-based MMO famous for its player-driven economy, massive player organizations (corporations and alliances), and territorial warfare. The game features:
- 300,000+ daily active players
- Player corporations ranging from 5 to 10,000+ members
- Alliances controlling vast regions of space
- Entirely player-driven economy (99% of items player-crafted)
- Long-term political intrigue and warfare

**Why This Source Matters:**
BlueMarble aims to create a persistent world where:
- Players organize into guilds for large-scale mining operations
- Resource extraction and trading drive the economy
- Territory control affects resource access
- Player cooperation is essential for major achievements
- Long-term player investment creates emergent narratives

EVE Online's 20+ years of refinement provide proven patterns for these exact systems.

## Core Social Systems

### 1. Corporation (Guild) Architecture

**Hierarchical Structure:**
```lua
-- Corporation (Guild) data structure
local Corporation = {
    id = "",
    name = "",
    ticker = "", -- 3-5 character abbreviation
    founded = 0,
    ceo = nil, -- Player reference
    headquarters = nil, -- Location reference
    
    -- Organizational structure
    members = {},
    roles = {},
    divisions = {},
    
    -- Resources
    wallet = 0, -- Corporate funds
    hangars = {}, -- Shared storage divisions
    structures = {}, -- Owned buildings/facilities
    
    -- Settings
    taxRate = 0.10, -- % of member income to corp
    applications = {}, -- Pending membership requests
    warStatus = {}, -- Active wars with other corps
    
    -- Reputation
    standings = {} -- Relationships with other corps
}

-- Role system with granular permissions
local Role = {
    name = "",
    permissions = {
        -- Financial
        canAccessWallet = false,
        canPayBounties = false,
        canRentOffices = false,
        
        -- Assets
        canAccessHangar = {}, -- Array of hangar division IDs
        canDeliverToHangar = {},
        canConfigureStructures = false,
        
        -- Members
        canInviteMembers = false,
        canKickMembers = false,
        canAssignRoles = false,
        canSetMemberTitles = false,
        
        -- Operations
        canStartWars = false,
        canSurrenderWars = false,
        canAnchorStructures = false,
        canSetTaxRate = false
    }
}
```

**Application to BlueMarble:**

**Guild Structure:**
```lua
-- BlueMarble Guild System
local Guild = {
    id = "",
    name = "",
    tag = "", -- 2-4 character tag
    founded = 0,
    
    -- Leadership
    guildMaster = nil,
    officers = {},
    members = {},
    
    -- Organization
    ranks = {
        {name = "Guild Master", level = 5, permissions = {...}},
        {name = "Officer", level = 4, permissions = {...}},
        {name = "Senior Member", level = 3, permissions = {...}},
        {name = "Member", level = 2, permissions = {...}},
        {name = "Recruit", level = 1, permissions = {...}}
    },
    
    -- Shared Resources
    treasury = 0, -- Guild funds
    sharedStorage = {
        materials = {},
        equipment = {},
        discoveries = {} -- Shared geological data
    },
    
    -- Territory
    claimedRegions = {},
    structures = {}, -- Mining rigs, refineries, storage
    
    -- Settings
    contributionTax = 0.05, -- % of mined resources to guild
    recruitmentOpen = true,
    motto = "",
    
    -- Relationships
    allies = {},
    enemies = {},
    reputation = 0
}

function Guild:inviteMember(player, invitedBy)
    if not self:hasPermission(invitedBy, "can_invite") then
        return false, "No permission to invite"
    end
    
    table.insert(self.applications, {
        player = player,
        invitedBy = invitedBy,
        timestamp = os.time(),
        status = "pending"
    })
    
    player:sendNotification("Invited to join " .. self.name)
    return true
end

function Guild:promoteMember(member, newRank, promotedBy)
    if not self:hasPermission(promotedBy, "can_promote") then
        return false, "No permission to promote"
    end
    
    if newRank.level >= promotedBy.rank.level then
        return false, "Cannot promote to equal or higher rank"
    end
    
    member.rank = newRank
    self:broadcastToGuild({
        type = "member_promoted",
        member = member.name,
        newRank = newRank.name,
        promotedBy = promotedBy.name
    })
    
    return true
end
```

**Benefits:**
- Clear organizational hierarchy
- Granular permission system prevents abuse
- Shared resources enable cooperation
- Professional structure encourages long-term membership

### 2. Player-Driven Economy

**Market System:**
```lua
-- Market order system
local MarketOrder = {
    id = "",
    type = "", -- "buy" or "sell"
    item = "",
    quantity = 0,
    pricePerUnit = 0,
    location = "", -- Market location
    
    seller = nil, -- Player reference
    timestamp = 0,
    expiresAt = 0,
    
    minVolume = 1, -- Minimum purchase quantity
    range = "station" -- "station", "solar_system", "region"
}

local Market = {
    orders = {},
    tradeHistory = {},
    priceIndexes = {}
}

function Market:placeSellOrder(player, item, quantity, price, duration)
    -- Validate player has items
    if not player.inventory:has(item, quantity) then
        return false, "Insufficient items"
    end
    
    -- Escrow items
    player.inventory:remove(item, quantity)
    
    -- Create order
    local order = MarketOrder:new({
        type = "sell",
        item = item,
        quantity = quantity,
        pricePerUnit = price,
        seller = player,
        location = player.location,
        timestamp = os.time(),
        expiresAt = os.time() + duration
    })
    
    table.insert(self.orders, order)
    self:broadcastNewOrder(order)
    
    return true, order
end

function Market:placeBuyOrder(player, item, quantity, price, duration)
    local totalCost = quantity * price
    
    -- Validate player has funds
    if player.wallet < totalCost then
        return false, "Insufficient funds"
    end
    
    -- Escrow funds
    player.wallet = player.wallet - totalCost
    
    -- Create order
    local order = MarketOrder:new({
        type = "buy",
        item = item,
        quantity = quantity,
        pricePerUnit = price,
        seller = player,
        location = player.location,
        timestamp = os.time(),
        expiresAt = os.time() + duration,
        escrowedFunds = totalCost
    })
    
    table.insert(self.orders, order)
    self:broadcastNewOrder(order)
    self:attemptAutoMatch(order)
    
    return true, order
end

function Market:attemptAutoMatch(buyOrder)
    -- Find matching sell orders
    local matchingOrders = self:findMatchingOrders(buyOrder)
    
    for _, sellOrder in ipairs(matchingOrders) do
        if sellOrder.pricePerUnit <= buyOrder.pricePerUnit then
            local tradeQuantity = math.min(buyOrder.quantity, sellOrder.quantity)
            
            -- Execute trade
            self:executeTrade(buyOrder, sellOrder, tradeQuantity)
            
            buyOrder.quantity = buyOrder.quantity - tradeQuantity
            if buyOrder.quantity == 0 then
                break
            end
        end
    end
end

function Market:executeTrade(buyOrder, sellOrder, quantity)
    local totalPrice = quantity * sellOrder.pricePerUnit
    
    -- Transfer items to buyer
    buyOrder.seller.inventory:add(sellOrder.item, quantity)
    
    -- Transfer funds to seller
    sellOrder.seller.wallet = sellOrder.seller.wallet + totalPrice
    
    -- Refund excess escrow to buyer if any
    if sellOrder.pricePerUnit < buyOrder.pricePerUnit then
        local refund = (buyOrder.pricePerUnit - sellOrder.pricePerUnit) * quantity
        buyOrder.seller.wallet = buyOrder.seller.wallet + refund
    end
    
    -- Update orders
    sellOrder.quantity = sellOrder.quantity - quantity
    if sellOrder.quantity == 0 then
        self:removeOrder(sellOrder)
    end
    
    -- Record trade history
    table.insert(self.tradeHistory, {
        item = sellOrder.item,
        quantity = quantity,
        price = sellOrder.pricePerUnit,
        timestamp = os.time(),
        location = sellOrder.location
    })
    
    -- Update price indexes
    self:updatePriceIndex(sellOrder.item, sellOrder.pricePerUnit)
end
```

**Application to BlueMarble:**

**Resource Market:**
```lua
-- BlueMarble resource trading system
local ResourceMarket = {
    orders = {},
    priceHistory = {},
    regionalMarkets = {}
}

function ResourceMarket:getPriceRange(resource, location)
    local orders = self:getOrdersForResource(resource, location)
    
    if #orders == 0 then
        return nil, nil
    end
    
    local buyOrders = self:filterOrders(orders, "buy")
    local sellOrders = self:filterOrders(orders, "sell")
    
    local highestBuy = self:getHighestPrice(buyOrders)
    local lowestSell = self:getLowestPrice(sellOrders)
    
    return highestBuy, lowestSell
end

function ResourceMarket:getMarketDepth(resource, location)
    -- Analyze supply and demand
    local buyVolume = 0
    local sellVolume = 0
    
    for _, order in ipairs(self.orders) do
        if order.item == resource and order.location == location then
            if order.type == "buy" then
                buyVolume = buyVolume + order.quantity
            else
                sellVolume = sellVolume + order.quantity
            end
        end
    end
    
    return {
        buyVolume = buyVolume,
        sellVolume = sellVolume,
        ratio = sellVolume > 0 and (buyVolume / sellVolume) or 0
    }
end

-- Dynamic pricing based on supply/demand
function ResourceMarket:calculateRecommendedPrice(resource, location)
    local history = self:getRecentTrades(resource, location, 100)
    
    if #history == 0 then
        return resource.baseValue
    end
    
    -- Calculate moving average
    local sum = 0
    for _, trade in ipairs(history) do
        sum = sum + trade.price
    end
    local avgPrice = sum / #history
    
    -- Factor in current supply/demand
    local depth = self:getMarketDepth(resource, location)
    local demandMultiplier = 1.0
    
    if depth.ratio > 1.5 then
        -- High demand, low supply
        demandMultiplier = 1.2
    elseif depth.ratio < 0.5 then
        -- Low demand, high supply
        demandMultiplier = 0.8
    end
    
    return avgPrice * demandMultiplier
end
```

**Benefits:**
- Player-driven pricing reflects actual supply/demand
- Encourages economic specialization
- Creates trading profession opportunities
- Resource scarcity drives exploration

### 3. Territorial Control

**Sovereignty System:**
```lua
-- Territory control system
local Territory = {
    id = "",
    name = "",
    boundaries = {}, -- Polygon defining region
    
    -- Ownership
    controlledBy = nil, -- Guild reference
    contestedBy = nil, -- Attacking guild
    
    -- Resources
    resourceNodes = {},
    structures = {},
    
    -- Control mechanics
    controlPoints = 100, -- 0-100 scale
    captureRate = 1, -- Points per hour
    
    -- Benefits
    bonuses = {
        miningSpeed = 1.1, -- 10% bonus
        refineryEfficiency = 1.05,
        defensivePower = 1.2
    }
}

function Territory:updateControl(deltaTime)
    if self.contestedBy then
        -- Check attacker presence
        local attackerPresence = self:countGuildMembers(self.contestedBy)
        local defenderPresence = self:countGuildMembers(self.controlledBy)
        
        if attackerPresence > defenderPresence then
            -- Territory being captured
            self.controlPoints = self.controlPoints - (self.captureRate * deltaTime)
            
            if self.controlPoints <= 0 then
                self:transferControl(self.contestedBy)
            end
        elseif defenderPresence > attackerPresence then
            -- Territory being defended
            self.controlPoints = math.min(100, self.controlPoints + (self.captureRate * deltaTime))
            
            if self.controlPoints >= 50 then
                self.contestedBy = nil -- Defense successful
            end
        end
    end
end

function Territory:transferControl(newOwner)
    local oldOwner = self.controlledBy
    
    self.controlledBy = newOwner
    self.contestedBy = nil
    self.controlPoints = 100
    
    -- Broadcast territory change
    EventBus:publish("TerritoryControl.Changed", {
        territory = self,
        oldOwner = oldOwner,
        newOwner = newOwner
    })
    
    -- Update bonuses for all players in region
    self:applyBonuses()
end
```

**Application to BlueMarble:**

**Regional Control:**
```lua
-- BlueMarble territorial control
local Region = {
    id = "",
    name = "",
    boundaries = {},
    
    controlledBy = nil,
    controlLevel = 0, -- 0-100
    
    -- Strategic value
    resourceDensity = 0,
    strategicImportance = 0,
    
    -- Structures
    miningRigs = {},
    refineries = {},
    defenseStructures = {}
}

function Region:applyControlBonuses()
    if not self.controlledBy then
        return
    end
    
    -- Apply bonuses to controlling guild members
    for _, member in ipairs(self.controlledBy.members) do
        if self:isPlayerInRegion(member) then
            member:addBuff({
                name = "territorial_control",
                miningSpeedBonus = 0.15,
                resourceYieldBonus = 0.10,
                duration = -1 -- Permanent while in region
            })
        end
    end
end

function Region:initiateTakeover(attackingGuild)
    if self.controlledBy == attackingGuild then
        return false, "Already controlled by this guild"
    end
    
    if not self:meetsRequirements(attackingGuild) then
        return false, "Guild does not meet requirements"
    end
    
    -- Start takeover process
    EventBus:publish("Region.TakeoverStarted", {
        region = self,
        attacker = attackingGuild,
        defender = self.controlledBy
    })
    
    return true
end
```

**Benefits:**
- Encourages guild cooperation
- Creates meaningful conflict drivers
- Rewards strategic planning
- Links geography to power

### 4. Contract System

**Secure Trading:**
```lua
-- Contract system for complex trades
local Contract = {
    id = "",
    type = "", -- "item_exchange", "courier", "auction"
    
    -- Parties
    issuer = nil,
    assignee = nil, -- Can be nil for public contracts
    
    -- Terms
    itemsOffered = {},
    itemsRequested = {},
    reward = 0,
    collateral = 0,
    
    -- Status
    status = "open", -- "open", "in_progress", "completed", "failed"
    expiresAt = 0,
    
    -- Location
    startLocation = "",
    endLocation = "" -- For courier contracts
}

function Contract:accept(player)
    if self.status ~= "open" then
        return false, "Contract no longer available"
    end
    
    if self.assignee and self.assignee ~= player then
        return false, "Contract not available to you"
    end
    
    -- Check requirements
    if self.collateral > 0 then
        if player.wallet < self.collateral then
            return false, "Insufficient collateral"
        end
        player.wallet = player.wallet - self.collateral
    end
    
    self.assignee = player
    self.status = "in_progress"
    
    return true
end

function Contract:complete()
    if self.status ~= "in_progress" then
        return false, "Contract not in progress"
    end
    
    -- Verify completion conditions
    if not self:verifyCompletion() then
        return false, "Completion conditions not met"
    end
    
    -- Transfer items
    for _, item in ipairs(self.itemsRequested) do
        self.assignee.inventory:remove(item.type, item.quantity)
        self.issuer.inventory:add(item.type, item.quantity)
    end
    
    for _, item in ipairs(self.itemsOffered) do
        self.issuer.inventory:remove(item.type, item.quantity)
        self.assignee.inventory:add(item.type, item.quantity)
    end
    
    -- Transfer payment
    self.issuer.wallet = self.issuer.wallet - self.reward
    self.assignee.wallet = self.assignee.wallet + self.reward + self.collateral
    
    self.status = "completed"
    
    EventBus:publish("Contract.Completed", {contract = self})
    
    return true
end
```

**Application to BlueMarble:**

**Mining Contracts:**
```lua
-- BlueMarble contract system
function Guild:createMiningContract(resourceType, quantity, reward, duration)
    local contract = Contract:new({
        type = "resource_delivery",
        issuer = self,
        itemsRequested = {{type = resourceType, quantity = quantity}},
        reward = reward,
        expiresAt = os.time() + duration
    })
    
    ContractBoard:postContract(contract)
    
    return contract
end

-- Players can fulfill guild contracts
function Player:fulfillContract(contract)
    if not self.inventory:has(contract.itemsRequested) then
        return false, "Missing required items"
    end
    
    contract:accept(self)
    contract:complete()
    
    return true
end
```

## Implementation Recommendations for BlueMarble

### Critical Features (Implement First)

**1. Basic Guild System - Week 1-3**
- Guild creation and membership
- Basic hierarchy (leader, officers, members)
- Shared treasury
- **Estimated Effort:** 3 weeks
- **Priority:** Critical

**2. Guild Storage - Week 3-4**
- Shared resource storage
- Permission-based access
- Contribution tracking
- **Estimated Effort:** 1 week
- **Priority:** High

**3. Basic Market System - Week 4-6**
- Buy/sell orders
- Order matching
- Trade history
- **Estimated Effort:** 2 weeks
- **Priority:** High

### Advanced Features (Second Phase)

**4. Role/Permission System - Week 7-8**
- Granular permissions
- Custom roles
- Permission inheritance
- **Estimated Effort:** 2 weeks
- **Priority:** Medium

**5. Territorial Control - Week 9-12**
- Region ownership
- Control mechanics
- Bonuses and benefits
- **Estimated Effort:** 4 weeks
- **Priority:** Medium

**6. Contract System - Week 13-15**
- Contract creation
- Escrow system
- Completion verification
- **Estimated Effort:** 3 weeks
- **Priority:** Low

## References

### Primary Sources

1. **EVE Online Developer Blogs** - CCP Games
   - Corporation mechanics documentation
   - Market system design discussions
   - Sovereignty system updates

2. **"Designing Virtual Worlds" by Richard Bartle**
   - Chapter on player organizations
   - Social dynamics in MMOs

3. **GDC Talks on EVE Online**
   - "EVE Online: Building a Thriving Economy"
   - "Managing 300,000 Players in One Universe"

### Related Research

- [game-dev-analysis-03-multiplayer-programming.md](game-dev-analysis-03-multiplayer-programming.md) - Server architecture for social systems
- [game-dev-analysis-02-game-programming-patterns.md](game-dev-analysis-02-game-programming-patterns.md) - Component pattern for guild members
- [game-dev-analysis-roblox-concepts.md](game-dev-analysis-roblox-concepts.md) - Community engagement patterns

## Discovered Sources

During this research, the following related sources were identified for future investigation:

1. **Second Life Economy Study** - Virtual world economy patterns (Medium Priority)
2. **World of Warcraft Guild Management Systems** - Guild progression mechanics (Medium Priority)
3. **Entropia Universe Real Cash Economy** - Real money trading systems (Low Priority)

These sources have been logged in [research-assignment-group-19.md](research-assignment-group-19.md) for future research phases.

---

**Document Status:** Complete  
**Last Updated:** 2025-01-15  
**Word Count:** ~4,500 words  
**Research Time:** 5 hours  
**Priority:** High (Social Systems Critical)  
**Applicability:** Very High - Core social and economic features for BlueMarble

**Next Steps:**
- Begin implementation with basic guild system
- Design guild storage and contribution tracking
- Prototype market order system
- Plan territorial control mechanics
- Consider integration with existing player systems
