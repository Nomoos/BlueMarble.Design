# Game Programming Patterns - Analysis for BlueMarble

---
title: Game Programming Patterns - Analysis for BlueMarble MMORPG
date: 2025-01-15
tags: [game-development, patterns, architecture, design-patterns, mmorpg]
status: complete
priority: high
category: GameDev-Tech
discovered-from: Roblox Game Development research
---

**Source:** Game Programming Patterns by Robert Nystrom  
**Category:** Game Development - Software Architecture  
**Priority:** High  
**Status:** ✅ Complete  
**Lines:** ~600  
**Related Sources:** Game Programming in C++, Design Patterns (Gang of Four), Clean Code, Refactoring

---

## Executive Summary

This analysis extracts essential software design patterns from "Game Programming Patterns" by Robert Nystrom, focusing on patterns directly applicable to BlueMarble's multiplayer geological simulation MMORPG. The book adapts classic software design patterns specifically for game development contexts, emphasizing performance, maintainability, and scalability - critical factors for a persistent world simulation.

**Key Takeaways for BlueMarble:**
- Event Queue pattern enables decoupled communication between game systems
- Component pattern provides flexible entity composition for varied geological features
- Update Method pattern structures the game loop for consistent simulation
- Spatial Partition optimizes collision detection and proximity queries in large worlds
- Object Pool pattern manages memory for frequently created/destroyed entities
- State pattern handles complex entity behaviors (player actions, resource extraction)
- Service Locator provides global access without tight coupling

**Relevance to BlueMarble:** Very High - These patterns directly address multiplayer architecture, world simulation, and performance optimization challenges

## Source Overview

**Book Context:**
"Game Programming Patterns" by Robert Nystrom is a comprehensive guide that takes classic software design patterns and adapts them for game development. Unlike traditional design pattern books, it focuses on real-world game programming challenges: performance constraints, tight coupling between systems, frame-rate consistency, and managing complex state.

**Why This Source Matters:**
BlueMarble's geological simulation requires sophisticated system architecture to handle:
- Thousands of concurrent players interacting with the world
- Complex entity relationships (resources, structures, NPCs)
- Real-time simulation of geological processes
- Efficient spatial queries for proximity-based interactions
- Memory management for long-running server processes

Nystrom's patterns provide proven solutions to these exact challenges.

## Core Patterns Analysis

### 1. Command Pattern

**Pattern Overview:**
Encapsulates actions as objects, enabling undo/redo, queuing, and network replication.

**Game Context:**
In games, the Command pattern turns player inputs into reusable objects that can be:
- Recorded for replays
- Sent over the network
- Queued for execution
- Undone/redone
- Stored for AI behavior

**Implementation Concept:**
```lua
-- Base command interface
local Command = {}
function Command:new()
    local obj = {}
    setmetatable(obj, self)
    self.__index = self
    return obj
end

function Command:execute(actor)
    -- Override in subclasses
end

function Command:undo(actor)
    -- Override in subclasses
end

-- Concrete command: Mining action
local MineCommand = Command:new()
function MineCommand:execute(actor)
    local deposit = actor:findNearestDeposit()
    if deposit and actor:canMine(deposit) then
        local resources = deposit:extract(actor.miningPower)
        actor:addToInventory(resources)
        self.depositId = deposit.id
        self.resources = resources
        return true
    end
    return false
end

function MineCommand:undo(actor)
    -- Return resources to deposit
    local deposit = world:getDeposit(self.depositId)
    if deposit then
        deposit:restore(self.resources)
        actor:removeFromInventory(self.resources)
    end
end

-- Usage in input handler
function InputHandler:handleButton(button, player)
    local command = self:getCommandForButton(button)
    if command then
        if command:execute(player) then
            -- Store for potential undo or network replication
            self.commandHistory:push(command)
            network:sendCommand(command, player.id)
        end
    end
end
```

**Application to BlueMarble:**

**Player Action System:**
Every player action (mining, crafting, building, trading) becomes a Command object:
- **Network Replication**: Send commands to server for validation and broadcast
- **Action History**: Track player actions for analytics and debugging
- **Undo Functionality**: Allow players to undo accidental actions (within constraints)
- **Macro System**: Let players record and replay action sequences

**Geological Event System:**
Natural events (earthquakes, erosion, resource regeneration) as commands:
```lua
local GeologicalEvent = Command:new()
function GeologicalEvent:new(eventType, location, magnitude)
    local obj = Command:new(self)
    obj.eventType = eventType
    obj.location = location
    obj.magnitude = magnitude
    obj.timestamp = os.time()
    return obj
end

function GeologicalEvent:execute(world)
    if self.eventType == "earthquake" then
        world:applySeismicForce(self.location, self.magnitude)
        world:updateTerrainStability(self.location, self.magnitude * 0.5)
    elseif self.eventType == "erosion" then
        world:applyErosion(self.location, self.magnitude)
    end
end
```

**Benefits:**
- Consistent interface for all player and world actions
- Easy network serialization
- Replay capability for debugging
- Action validation on server before execution

### 2. Flyweight Pattern

**Pattern Overview:**
Shares common data between multiple objects to minimize memory usage when dealing with large numbers of similar entities.

**Game Context:**
In a game with thousands of entities, most share common properties (texture, behavior, stats). The Flyweight pattern separates:
- **Intrinsic state**: Shared across all instances (texture, base stats, behavior scripts)
- **Extrinsic state**: Unique to each instance (position, health, owner)

**Implementation Concept:**
```lua
-- Shared resource data (intrinsic state)
local ResourceType = {}
ResourceType.registry = {}

function ResourceType:new(name, density, value, texture, extractionDifficulty)
    local obj = {
        name = name,
        density = density,
        baseValue = value,
        texture = texture,
        extractionDifficulty = extractionDifficulty,
        particleEffect = "rock_dust",
        soundEffect = "mining_" .. name
    }
    setmetatable(obj, self)
    self.__index = self
    ResourceType.registry[name] = obj
    return obj
end

-- Initialize resource types once
ResourceType:new("iron_ore", 7.87, 10, "textures/iron_ore.png", 5)
ResourceType:new("copper_ore", 8.96, 8, "textures/copper_ore.png", 4)
ResourceType:new("gold_ore", 19.32, 50, "textures/gold_ore.png", 8)

-- Individual resource deposit (extrinsic state)
local ResourceDeposit = {}
function ResourceDeposit:new(typeName, position, quantity, quality)
    return {
        type = ResourceType.registry[typeName], -- Shared reference
        position = position,
        quantity = quantity,
        quality = quality,
        discoveredBy = nil,
        lastMined = 0
    }
end

-- Usage: 10,000 iron deposits share the same ResourceType object
for i = 1, 10000 do
    local deposit = ResourceDeposit:new("iron_ore", randomPosition(), 
                                         randomQuantity(), randomQuality())
    world:addDeposit(deposit)
end
```

**Application to BlueMarble:**

**Resource System:**
- **Shared Data**: Resource type definitions (texture, density, value, extraction rules)
- **Instance Data**: Position, quantity, quality, discovery state
- Memory savings: 10,000 deposits × 200 bytes shared data = 2MB saved vs. 0.2KB overhead

**Entity Templates:**
```lua
-- Shared template data
local EntityTemplate = {
    registry = {}
}

function EntityTemplate:register(name, data)
    self.registry[name] = {
        model = data.model,
        defaultStats = data.stats,
        behaviors = data.behaviors,
        animations = data.animations,
        collisionMesh = data.collision
    }
end

-- Register templates
EntityTemplate:register("mining_rig", {
    model = "models/mining_rig.obj",
    stats = {health = 1000, powerConsumption = 500},
    behaviors = {"AutoMine", "ResourceStorage"},
    animations = {"idle", "active", "damaged"},
    collision = "boxes/mining_rig_collision.dat"
})

-- Lightweight entity instances
local Entity = {}
function Entity:new(templateName, position, owner)
    return {
        template = EntityTemplate.registry[templateName], -- Shared
        position = position,
        owner = owner,
        currentHealth = nil, -- Will use template.defaultStats.health
        state = "idle"
    }
end
```

**Benefits:**
- Reduced memory footprint for massive worlds
- Faster entity instantiation
- Easier balancing (change template affects all instances)
- Network efficiency (send template reference, not full data)

### 3. Observer Pattern (Event System)

**Pattern Overview:**
Defines one-to-many dependency between objects so that when one object changes state, all dependents are notified automatically.

**Game Context:**
Game systems need to react to events without tight coupling. Observer pattern enables:
- Achievement system reacting to player actions
- UI updating when game state changes
- Sound effects triggering on events
- Multiple systems responding to single event

**Implementation Concept:**
```lua
-- Event system using observer pattern
local EventBus = {
    listeners = {}
}

function EventBus:subscribe(eventType, callback, priority)
    if not self.listeners[eventType] then
        self.listeners[eventType] = {}
    end
    table.insert(self.listeners[eventType], {
        callback = callback,
        priority = priority or 0
    })
    -- Sort by priority
    table.sort(self.listeners[eventType], function(a, b)
        return a.priority > b.priority
    end)
end

function EventBus:publish(eventType, eventData)
    if self.listeners[eventType] then
        for _, listener in ipairs(self.listeners[eventType]) do
            listener.callback(eventData)
        end
    end
end

function EventBus:unsubscribe(eventType, callback)
    if self.listeners[eventType] then
        for i, listener in ipairs(self.listeners[eventType]) do
            if listener.callback == callback then
                table.remove(self.listeners[eventType], i)
                break
            end
        end
    end
end
```

**Application to BlueMarble:**

**Resource Discovery System:**
```lua
-- Multiple systems subscribe to discovery event
EventBus:subscribe("ResourceDiscovered", function(event)
    -- Achievement system
    if event.resourceType == "rare_earth" then
        AchievementSystem:unlock(event.player, "first_rare_earth")
    end
end, 100)

EventBus:subscribe("ResourceDiscovered", function(event)
    -- Market system
    MarketSystem:updatePrices(event.resourceType, -0.01) -- Supply increase
end, 50)

EventBus:subscribe("ResourceDiscovered", function(event)
    -- Analytics
    Analytics:recordEvent("discovery", {
        player = event.player.id,
        resource = event.resourceType,
        location = event.location
    })
end, 10)

EventBus:subscribe("ResourceDiscovered", function(event)
    -- Guild system
    if event.player.guild then
        GuildSystem:addContribution(event.player.guild, "discoveries", 1)
    end
end, 50)

-- Publish event from mining system
function MiningSystem:onDepositFound(player, deposit)
    EventBus:publish("ResourceDiscovered", {
        player = player,
        resourceType = deposit.type.name,
        location = deposit.position,
        quantity = deposit.quantity,
        timestamp = os.time()
    })
end
```

**Geological Event Broadcasting:**
```lua
-- Earthquake affects multiple systems
EventBus:subscribe("GeologicalEvent.Earthquake", function(event)
    -- Terrain system
    TerrainSystem:applyDeformation(event.epicenter, event.magnitude)
end, 100)

EventBus:subscribe("GeologicalEvent.Earthquake", function(event)
    -- Structure system
    StructureSystem:checkStabilityInRadius(event.epicenter, event.radius)
end, 90)

EventBus:subscribe("GeologicalEvent.Earthquake", function(event)
    -- Player system
    PlayerSystem:notifyPlayersInRadius(event.epicenter, event.radius, 
                                       "Earthquake detected!")
end, 50)
```

**Benefits:**
- Decoupled systems (no direct dependencies)
- Easy to add new reactions to events
- Priority-based execution order
- Clean separation of concerns

### 4. Component Pattern

**Pattern Overview:**
Allows entities to be composed of reusable components rather than using deep inheritance hierarchies.

**Game Context:**
Instead of creating classes like `FlyingEnemyThatShootsFireballs`, components let you compose behavior:
- Position Component
- Renderable Component
- Physics Component
- AI Component
- Weapon Component

**Implementation Concept:**
```lua
-- Component system
local Component = {}
function Component:new()
    local obj = {}
    setmetatable(obj, self)
    self.__index = self
    return obj
end

-- Position component
local PositionComponent = Component:new()
function PositionComponent:new(x, y, z)
    local obj = Component.new(self)
    obj.x = x or 0
    obj.y = y or 0
    obj.z = z or 0
    return obj
end

-- Resource storage component
local StorageComponent = Component:new()
function StorageComponent:new(capacity)
    local obj = Component.new(self)
    obj.capacity = capacity
    obj.contents = {}
    return obj
end

function StorageComponent:add(resourceType, amount)
    if self:getFreeSpace() >= amount then
        self.contents[resourceType] = (self.contents[resourceType] or 0) + amount
        return true
    end
    return false
end

function StorageComponent:getFreeSpace()
    local used = 0
    for _, amount in pairs(self.contents) do
        used = used + amount
    end
    return self.capacity - used
end

-- Entity composed of components
local Entity = {}
function Entity:new(id)
    return {
        id = id,
        components = {}
    }
end

function Entity:addComponent(componentType, component)
    self.components[componentType] = component
end

function Entity:getComponent(componentType)
    return self.components[componentType]
end

function Entity:hasComponent(componentType)
    return self.components[componentType] ~= nil
end
```

**Application to BlueMarble:**

**Flexible Entity System:**
```lua
-- Create a mining rig with storage and power consumption
local miningRig = Entity:new("mining_rig_001")
miningRig:addComponent("Position", PositionComponent:new(100, 50, 200))
miningRig:addComponent("Storage", StorageComponent:new(1000))
miningRig:addComponent("Power", PowerComponent:new(500, 100)) -- capacity, consumption
miningRig:addComponent("Mining", MiningComponent:new(10)) -- mining power
miningRig:addComponent("Ownership", OwnershipComponent:new(playerId))

-- Create a player with different components
local player = Entity:new("player_001")
player:addComponent("Position", PositionComponent:new(95, 50, 195))
player:addComponent("Inventory", StorageComponent:new(100))
player:addComponent("Skills", SkillsComponent:new({mining = 5, crafting = 3}))
player:addComponent("Health", HealthComponent:new(100, 100))
player:addComponent("Network", NetworkComponent:new(sessionId))

-- Systems operate on entities with specific components
local MiningSystem = {}
function MiningSystem:update(entities, deltaTime)
    for _, entity in ipairs(entities) do
        -- Only process entities with required components
        if entity:hasComponent("Mining") and 
           entity:hasComponent("Power") and
           entity:hasComponent("Storage") then
            
            local mining = entity:getComponent("Mining")
            local power = entity:getComponent("Power")
            local storage = entity:getComponent("Storage")
            
            if power:hasEnergy(power.consumption * deltaTime) then
                local resources = self:extractNearbyResources(entity, mining.power)
                if storage:add(resources.type, resources.amount) then
                    power:consume(power.consumption * deltaTime)
                end
            end
        end
    end
end
```

**Dynamic Feature Addition:**
```lua
-- Add component at runtime
function upgradeToAutomated(miningRig)
    if not miningRig:hasComponent("AI") then
        miningRig:addComponent("AI", AIComponent:new("auto_mine"))
    end
end

-- Remove component when feature no longer needed
function disablePowerSystem(entity)
    entity.components["Power"] = nil
end
```

**Benefits:**
- Flexible entity composition
- Reusable components across entity types
- Easy to add/remove features at runtime
- Systems operate on component types, not entity classes

### 5. Update Method Pattern

**Pattern Overview:**
Structures game loop to update each entity once per frame in a consistent manner.

**Game Context:**
Every game needs a game loop that:
- Updates all entities
- Maintains consistent frame timing
- Processes input, physics, AI, rendering in order
- Handles variable frame rates

**Implementation Concept:**
```lua
-- Update method for game entities
local GameObject = {}
function GameObject:new()
    local obj = {}
    setmetatable(obj, self)
    self.__index = self
    return obj
end

function GameObject:update(deltaTime)
    -- Override in subclasses
end

-- Game loop
local GameLoop = {
    entities = {},
    running = false,
    lastFrameTime = 0
}

function GameLoop:run()
    self.running = true
    self.lastFrameTime = os.clock()
    
    while self.running do
        local currentTime = os.clock()
        local deltaTime = currentTime - self.lastFrameTime
        self.lastFrameTime = currentTime
        
        -- Cap delta time to prevent spiral of death
        if deltaTime > 0.25 then
            deltaTime = 0.25
        end
        
        self:update(deltaTime)
        self:render()
        
        -- Maintain target frame rate
        self:sleep(0.016) -- ~60 FPS
    end
end

function GameLoop:update(deltaTime)
    -- Process input
    InputSystem:update(deltaTime)
    
    -- Update all entities
    for _, entity in ipairs(self.entities) do
        entity:update(deltaTime)
    end
    
    -- Update systems
    PhysicsSystem:update(deltaTime)
    CollisionSystem:update(deltaTime)
    NetworkSystem:update(deltaTime)
end
```

**Application to BlueMarble:**

**Geological Simulation Loop:**
```lua
local WorldSimulation = {
    systems = {},
    simulationRate = 60, -- ticks per second
    accumulator = 0,
    fixedDeltaTime = 1/60
}

function WorldSimulation:update(realDeltaTime)
    self.accumulator = self.accumulator + realDeltaTime
    
    -- Fixed timestep for deterministic simulation
    while self.accumulator >= self.fixedDeltaTime do
        self:fixedUpdate(self.fixedDeltaTime)
        self.accumulator = self.accumulator - self.fixedDeltaTime
    end
    
    -- Variable timestep for rendering/interpolation
    self:variableUpdate(realDeltaTime)
end

function WorldSimulation:fixedUpdate(deltaTime)
    -- Deterministic geological processes
    TerrainSystem:simulateTectonics(deltaTime)
    ErosionSystem:simulateWeathering(deltaTime)
    ResourceSystem:simulateRegeneration(deltaTime)
    
    -- Player actions
    for _, player in ipairs(self:getActivePlayers()) do
        player:update(deltaTime)
    end
    
    -- Physics
    PhysicsSystem:step(deltaTime)
end

function WorldSimulation:variableUpdate(deltaTime)
    -- Non-critical systems
    ParticleSystem:update(deltaTime)
    AnimationSystem:update(deltaTime)
    SoundSystem:update(deltaTime)
end
```

**System Priority Ordering:**
```lua
-- Update systems in specific order
function GameLoop:update(deltaTime)
    -- 1. Input (highest priority)
    InputSystem:update(deltaTime)
    
    -- 2. Game logic
    PlayerSystem:update(deltaTime)
    AISystem:update(deltaTime)
    
    -- 3. Physics
    PhysicsSystem:update(deltaTime)
    CollisionSystem:update(deltaTime)
    
    -- 4. World simulation
    GeologicalSystem:update(deltaTime)
    WeatherSystem:update(deltaTime)
    
    -- 5. Networking
    NetworkSystem:sendUpdates(deltaTime)
    
    -- 6. Cleanup
    EntityManager:removeDestroyedEntities()
end
```

**Benefits:**
- Consistent update timing across all entities
- Clear system execution order
- Fixed timestep for deterministic simulation
- Easy to add/remove systems

### 6. Spatial Partition (Quadtree/Octree)

**Pattern Overview:**
Organizes game objects in space to enable efficient spatial queries (what's near me?).

**Game Context:**
In large game worlds, checking every entity against every other is O(n²). Spatial partitioning reduces this to O(log n) by organizing entities hierarchically based on position.

**Implementation Concept:**
```lua
-- Quadtree for 2D spatial partitioning
local Quadtree = {}
function Quadtree:new(boundary, capacity)
    return {
        boundary = boundary, -- {x, y, width, height}
        capacity = capacity or 4,
        entities = {},
        divided = false,
        northwest = nil,
        northeast = nil,
        southwest = nil,
        southeast = nil
    }
end

function Quadtree:subdivide()
    local x, y = self.boundary.x, self.boundary.y
    local w, h = self.boundary.width / 2, self.boundary.height / 2
    
    self.northwest = Quadtree:new({x=x, y=y, width=w, height=h}, self.capacity)
    self.northeast = Quadtree:new({x=x+w, y=y, width=w, height=h}, self.capacity)
    self.southwest = Quadtree:new({x=x, y=y+h, width=w, height=h}, self.capacity)
    self.southeast = Quadtree:new({x=x+w, y=y+h, width=w, height=h}, self.capacity)
    self.divided = true
end

function Quadtree:insert(entity)
    if not self:contains(entity.position) then
        return false
    end
    
    if #self.entities < self.capacity then
        table.insert(self.entities, entity)
        return true
    end
    
    if not self.divided then
        self:subdivide()
    end
    
    return self.northwest:insert(entity) or
           self.northeast:insert(entity) or
           self.southwest:insert(entity) or
           self.southeast:insert(entity)
end

function Quadtree:query(range, found)
    found = found or {}
    
    if not self:intersects(range) then
        return found
    end
    
    for _, entity in ipairs(self.entities) do
        if range:contains(entity.position) then
            table.insert(found, entity)
        end
    end
    
    if self.divided then
        self.northwest:query(range, found)
        self.northeast:query(range, found)
        self.southwest:query(range, found)
        self.southeast:query(range, found)
    end
    
    return found
end
```

**Application to BlueMarble:**

**Proximity-Based Resource Discovery:**
```lua
-- Find all resource deposits within 100m of player
local player = getPlayer(playerId)
local range = {
    x = player.position.x - 100,
    y = player.position.y - 100,
    width = 200,
    height = 200
}

local nearbyDeposits = worldQuadtree:query(range)
for _, deposit in ipairs(nearbyDeposits) do
    if not deposit.discovered then
        deposit.discovered = true
        EventBus:publish("ResourceDiscovered", {
            player = player,
            deposit = deposit
        })
    end
end
```

**Efficient Collision Detection:**
```lua
-- Only check collisions between nearby entities
function CollisionSystem:update(deltaTime)
    for _, entity in ipairs(movingEntities) do
        local range = {
            x = entity.position.x - entity.collisionRadius,
            y = entity.position.y - entity.collisionRadius,
            width = entity.collisionRadius * 2,
            height = entity.collisionRadius * 2
        }
        
        local nearby = worldQuadtree:query(range)
        for _, other in ipairs(nearby) do
            if entity ~= other then
                self:checkCollision(entity, other)
            end
        end
    end
end
```

**Network Relevancy (Area of Interest):**
```lua
-- Only send updates about nearby entities to players
function NetworkSystem:sendWorldUpdates()
    for _, player in ipairs(connectedPlayers) do
        local range = {
            x = player.position.x - 500, -- 500m view distance
            y = player.position.y - 500,
            width = 1000,
            height = 1000
        }
        
        local relevantEntities = worldQuadtree:query(range)
        local updatePacket = self:serializeEntities(relevantEntities)
        self:sendToPlayer(player, updatePacket)
    end
end
```

**Benefits:**
- O(log n) spatial queries vs O(n) brute force
- Essential for large worlds with thousands of entities
- Reduces network bandwidth (only send relevant updates)
- Enables efficient collision detection

### 7. Object Pool Pattern

**Pattern Overview:**
Reuses objects instead of creating and destroying them frequently, reducing garbage collection pressure.

**Game Context:**
Games frequently create/destroy objects (bullets, particles, network messages). Object pooling pre-allocates objects and reuses them, avoiding:
- Allocation overhead
- Garbage collection pauses
- Memory fragmentation

**Implementation Concept:**
```lua
-- Generic object pool
local ObjectPool = {}
function ObjectPool:new(createFunc, resetFunc, initialSize)
    local obj = {
        createFunc = createFunc,
        resetFunc = resetFunc,
        available = {},
        inUse = {}
    }
    setmetatable(obj, self)
    self.__index = self
    
    -- Pre-allocate initial objects
    for i = 1, initialSize do
        table.insert(obj.available, createFunc())
    end
    
    return obj
end

function ObjectPool:acquire()
    local obj
    if #self.available > 0 then
        obj = table.remove(self.available)
    else
        obj = self.createFunc()
    end
    self.inUse[obj] = true
    return obj
end

function ObjectPool:release(obj)
    if self.inUse[obj] then
        self.inUse[obj] = nil
        self.resetFunc(obj)
        table.insert(self.available, obj)
    end
end

function ObjectPool:releaseAll()
    for obj, _ in pairs(self.inUse) do
        self:release(obj)
    end
end
```

**Application to BlueMarble:**

**Particle System:**
```lua
-- Pool for rock particles during mining
local particlePool = ObjectPool:new(
    -- Create function
    function()
        return {
            position = {x=0, y=0, z=0},
            velocity = {x=0, y=0, z=0},
            lifetime = 0,
            color = {r=1, g=1, b=1, a=1}
        }
    end,
    -- Reset function
    function(particle)
        particle.lifetime = 0
        particle.color.a = 1
    end,
    1000 -- Initial pool size
)

-- Emit particles when mining
function MiningSystem:onMineHit(position)
    for i = 1, 20 do
        local particle = particlePool:acquire()
        particle.position = {x=position.x, y=position.y, z=position.z}
        particle.velocity = randomVelocity()
        particle.lifetime = 2.0
        particleSystem:addParticle(particle)
    end
end

-- Update and return to pool when expired
function ParticleSystem:update(deltaTime)
    for i = #self.particles, 1, -1 do
        local particle = self.particles[i]
        particle.lifetime = particle.lifetime - deltaTime
        
        if particle.lifetime <= 0 then
            particlePool:release(particle)
            table.remove(self.particles, i)
        else
            particle.position.x = particle.position.x + particle.velocity.x * deltaTime
            particle.position.y = particle.position.y + particle.velocity.y * deltaTime
            particle.position.z = particle.position.z + particle.velocity.z * deltaTime
        end
    end
end
```

**Network Message Pool:**
```lua
-- Pool for network messages
local messagePool = ObjectPool:new(
    function()
        return {
            type = "",
            data = {},
            timestamp = 0
        }
    end,
    function(msg)
        msg.type = ""
        msg.data = {}
    end,
    500
)

-- Send message
function NetworkSystem:sendMessage(type, data, recipient)
    local msg = messagePool:acquire()
    msg.type = type
    msg.data = data
    msg.timestamp = os.time()
    self:transmit(msg, recipient)
    -- Message returned to pool after transmission
    messagePool:release(msg)
end
```

**Entity Pool (for frequently spawned/removed entities):**
```lua
-- Pool for temporary entities like mining drones
local dronePool = ObjectPool:new(
    function()
        return Entity:new("mining_drone")
    end,
    function(drone)
        drone:getComponent("Position"):reset(0, 0, 0)
        drone:getComponent("Task"):clear()
    end,
    50
)

-- Spawn drone
function PlayerSystem:deployMiningDrone(player, target)
    local drone = dronePool:acquire()
    drone:getComponent("Position"):set(player.position.x, player.position.y, player.position.z)
    drone:getComponent("Task"):setTarget(target)
    drone:getComponent("Ownership"):setOwner(player.id)
    world:addEntity(drone)
end

-- Return drone when task complete
function DroneSystem:onTaskComplete(drone)
    world:removeEntity(drone)
    dronePool:release(drone)
end
```

**Benefits:**
- Reduced garbage collection pressure
- Predictable performance (no allocation spikes)
- Lower memory fragmentation
- Essential for high-frequency object creation

### 8. State Pattern

**Pattern Overview:**
Encapsulates state-dependent behavior in separate state objects, avoiding complex if/switch statements.

**Game Context:**
Game entities have different behaviors in different states (idle, walking, attacking, dead). State pattern makes these transitions explicit and maintainable.

**Implementation Concept:**
```lua
-- State base class
local State = {}
function State:new()
    local obj = {}
    setmetatable(obj, self)
    self.__index = self
    return obj
end

function State:enter(entity)
    -- Called when entering state
end

function State:update(entity, deltaTime)
    -- Called each frame
end

function State:exit(entity)
    -- Called when leaving state
end

-- Concrete states
local IdleState = State:new()
function IdleState:enter(entity)
    entity:playAnimation("idle")
end

function IdleState:update(entity, deltaTime)
    if entity:hasTarget() then
        entity:changeState(entity.states.moving)
    end
end

local MovingState = State:new()
function MovingState:enter(entity)
    entity:playAnimation("walking")
end

function MovingState:update(entity, deltaTime)
    entity:moveTowards(entity.target, deltaTime)
    if entity:reachedTarget() then
        entity:changeState(entity.states.idle)
    end
end

-- Entity with state machine
local Entity = {}
function Entity:new()
    local obj = {
        currentState = nil,
        states = {
            idle = IdleState,
            moving = MovingState
        }
    }
    setmetatable(obj, self)
    self.__index = self
    obj:changeState(obj.states.idle)
    return obj
end

function Entity:changeState(newState)
    if self.currentState then
        self.currentState:exit(self)
    end
    self.currentState = newState
    self.currentState:enter(self)
end

function Entity:update(deltaTime)
    if self.currentState then
        self.currentState:update(self, deltaTime)
    end
end
```

**Application to BlueMarble:**

**Player Action States:**
```lua
-- Player mining states
local PlayerIdleState = State:new()
local PlayerMiningState = State:new()
local PlayerCraftingState = State:new()
local PlayerTradingState = State:new()

function PlayerMiningState:enter(player)
    player.currentAction = "mining"
    player:playAnimation("mining_start")
    player:equipTool("pickaxe")
end

function PlayerMiningState:update(player, deltaTime)
    local deposit = player.targetDeposit
    if not deposit or deposit.quantity <= 0 then
        player:changeState(player.states.idle)
        return
    end
    
    player.miningProgress = player.miningProgress + player.miningSpeed * deltaTime
    
    if player.miningProgress >= deposit.extractionTime then
        player:extractResources(deposit)
        player.miningProgress = 0
        
        if player.inventory:isFull() then
            player:changeState(player.states.idle)
            player:showMessage("Inventory full!")
        end
    end
end

function PlayerMiningState:exit(player)
    player:playAnimation("mining_end")
    player:unequipTool()
end
```

**Resource Deposit States:**
```lua
-- Resource deposit lifecycle states
local DepositUndiscovered = State:new()
local DepositActive = State:new()
local DepositDepleted = State:new()
local DepositRegenerating = State:new()

function DepositActive:update(deposit, deltaTime)
    if deposit.quantity <= 0 then
        deposit:changeState(deposit.states.depleted)
    end
end

function DepositDepleted:enter(deposit)
    deposit.regenerationTimer = 0
    EventBus:publish("DepositDepleted", {deposit = deposit})
end

function DepositDepleted:update(deposit, deltaTime)
    deposit.regenerationTimer = deposit.regenerationTimer + deltaTime
    if deposit.regenerationTimer >= deposit.regenerationTime then
        deposit:changeState(deposit.states.regenerating)
    end
end

function DepositRegenerating:update(deposit, deltaTime)
    deposit.quantity = deposit.quantity + deposit.regenerationRate * deltaTime
    if deposit.quantity >= deposit.maxQuantity then
        deposit.quantity = deposit.maxQuantity
        deposit:changeState(deposit.states.active)
    end
end
```

**Benefits:**
- Clean separation of state-specific behavior
- Easy to add new states
- Clear state transitions
- Eliminates complex if/else chains

## Implementation Recommendations for BlueMarble

### High Priority Patterns (Implement First)

**1. Event System (Observer Pattern) - Week 1-2**
```lua
-- Core event bus for system communication
-- Enables decoupled architecture from the start
-- Estimated effort: 2-3 days
```

**Why First:**
- Foundation for all other systems
- Prevents tight coupling early
- Easy to add new features later

**2. Component Pattern - Week 2-3**
```lua
-- Entity-component system for flexible entities
-- Estimated effort: 1 week
```

**Why Second:**
- Depends on event system
- Core to game architecture
- Needed before adding many entity types

**3. Object Pool - Week 3-4**
```lua
-- Pools for particles, network messages, temporary entities
-- Estimated effort: 3-4 days
```

**Why Third:**
- Performance critical
- Easy to retrofit existing systems
- Immediate measurable impact

### Medium Priority Patterns (Implement Second)

**4. Spatial Partition (Quadtree) - Week 5-6**
```lua
-- 2D quadtree for world partitioning
-- 3D octree if needed for underground
-- Estimated effort: 1 week
```

**5. Command Pattern - Week 6-7**
```lua
-- Player actions and geological events
-- Estimated effort: 4-5 days
```

**6. State Pattern - Week 7-8**
```lua
-- Entity state machines
-- Estimated effort: 3-4 days
```

### Low Priority Patterns (Polish)

**7. Flyweight Pattern - Week 9+**
```lua
-- Optimization once entity count is high
-- Estimated effort: 2-3 days
```

**8. Update Method - Ongoing**
```lua
-- Refine game loop structure
-- Estimated effort: continuous refinement
```

## Integration Guidelines

### Pattern Combination Example

A complete mining operation using multiple patterns:

```lua
-- 1. Component Pattern: Flexible entity composition
local player = Entity:new("player_001")
player:addComponent("Position", PositionComponent:new(100, 50, 200))
player:addComponent("Inventory", StorageComponent:new(100))
player:addComponent("Skills", SkillsComponent:new({mining = 5}))
player:addComponent("StateMachine", StateMachineComponent:new())

-- 2. State Pattern: Player action states
player:getComponent("StateMachine"):addState("idle", IdleState)
player:getComponent("StateMachine"):addState("mining", MiningState)
player:getComponent("StateMachine"):changeState("idle")

-- 3. Command Pattern: Player initiates mining
local mineCommand = MineCommand:new(targetDeposit)
if mineCommand:execute(player) then
    -- 4. Observer Pattern: Broadcast event
    EventBus:publish("MiningStarted", {
        player = player,
        deposit = targetDeposit
    })
    
    -- 5. State Pattern: Transition to mining state
    player:getComponent("StateMachine"):changeState("mining")
end

-- 6. Update Method: Game loop processes mining
function GameLoop:update(deltaTime)
    -- 7. Spatial Partition: Find nearby entities efficiently
    local range = {x = player.position.x - 100, y = player.position.y - 100, 
                   width = 200, height = 200}
    local nearby = worldQuadtree:query(range)
    
    -- Update player state
    player:getComponent("StateMachine"):update(deltaTime)
    
    -- 8. Object Pool: Spawn particles
    if player.isMining then
        for i = 1, 5 do
            local particle = particlePool:acquire()
            particle.position = player.position
            particleSystem:addParticle(particle)
        end
    end
end

-- 9. Flyweight Pattern: Shared resource data
-- All iron deposits share same ResourceType object
local ironType = ResourceType.registry["iron_ore"]
-- Each deposit has unique state
local deposit = ResourceDeposit:new("iron_ore", position, 1000, 0.8)
```

## References

### Primary Source

1. **Game Programming Patterns** by Robert Nystrom
   - Website: https://gameprogrammingpatterns.com/
   - Print: ISBN 978-0990582908
   - Online: Free HTML version available
   - All patterns discussed in detail with game-specific examples

### Related Books

2. **Design Patterns: Elements of Reusable Object-Oriented Software** by Gang of Four
   - Classic software patterns (basis for Nystrom's book)

3. **Game Programming in C++** by Sanjay Madhav
   - Complementary implementation details
   - Related analysis: game-dev-analysis-01-game-programming-cpp.md

4. **Game Engine Architecture** by Jason Gregory
   - Broader architectural context
   - How patterns fit into full engine

### Online Resources

5. **Robert Nystrom's Blog**: http://journal.stuffwithstuff.com/
   - Additional pattern discussions
   - Game development insights

6. **GDC Talks on Architecture**
   - Various presentations on game architecture patterns
   - Case studies from shipping games

## Related Research

### Within BlueMarble Repository

- [game-dev-analysis-01-game-programming-cpp.md](game-dev-analysis-01-game-programming-cpp.md) - C++ implementation details
- [game-dev-analysis-roblox-concepts.md](game-dev-analysis-roblox-concepts.md) - Event-driven architecture examples
- [online-game-dev-resources.md](online-game-dev-resources.md) - Additional development resources
- [master-research-queue.md](master-research-queue.md) - Research tracking

### External Resources

- Game Programming Patterns website (free online version)
- Component pattern discussions in ECS frameworks
- Object pooling benchmarks and case studies
- State machine implementations in popular engines

## Discovered Sources

During this research, the following related sources were identified for future investigation:

1. **Data-Oriented Design** by Richard Fabian - Performance-focused approach complementing component pattern (High Priority)
2. **Refactoring: Improving the Design of Existing Code** by Martin Fowler - Techniques for migrating to patterns (Medium Priority)
3. **Game Engine Gems** series - Pattern implementations in production engines (Medium Priority)
4. **"Evolve Your Hierarchy" (GDC Talk)** by Mick West - Component systems in practice (Medium Priority)

These sources have been logged in [research-assignment-group-19.md](research-assignment-group-19.md) for future research phases.

---

**Document Status:** Complete  
**Last Updated:** 2025-01-15  
**Word Count:** ~5,000 words  
**Research Time:** 4 hours  
**Priority:** High (Core Architecture)  
**Applicability:** Very High - Fundamental patterns for BlueMarble's architecture

**Next Steps:**
- Begin implementation with Event System (Observer Pattern)
- Prototype Component Pattern for player entities
- Create object pools for high-frequency entities
- Review pattern combinations for complex features
- Train team on pattern usage and benefits
