# Content Extraction Guide 05: Military Manuals for Large-Scale Operations

---
title: Military Manuals Content Extraction
date: 2025-01-15
tags: [military, logistics, warfare, tactics, tier3-5]
status: completed
priority: medium
source: Military Manuals Collection (22,000+ documents)
---

## Executive Summary

The Military Manuals collection (22,000+ documents) provides authentic logistics, tactics, and organizational structures for BlueMarble's warfare systems.

**Target Content:**
- Logistics and supply chain mechanics
- Unit organization and command structures
- Tactical formations and doctrine
- Fortification and engineering

**Implementation Priority:** MEDIUM - Large-scale PvP and faction gameplay

## Source Overview

### Collection Scope

**Size:** 300+ GB of military field manuals including:
- US Army Field Manuals (FM series)
- Technical Manuals (TM series)
- Training circulars
- Historical doctrine documents

**Key Documents:**
- FM 4-0: Sustainment Operations
- FM 3-0: Operations
- FM 5-34: Engineer Field Data
- FM 21-76: Survival
- TM series: Equipment maintenance

## Content Extraction Strategy

### Category 1: Logistics Systems (Tier 3-5)

**Supply Chain Mechanics:**
```python
class SupplySystem:
    def __init__(self):
        self.supply_classes = {
            1: "Subsistence (food)",
            2: "Clothing and equipment",
            3: "Fuel and energy",
            4: "Construction materials",
            5: "Ammunition",
            6: "Personal items",
            9: "Repair parts"
        }
    
    def calculate_consumption(self, unit_size, days):
        """Calculate supply needs based on FM 4-0"""
        return {
            "food": unit_size * 3 * days,  # 3 meals/day
            "water": unit_size * 15 * days,  # 15L/person/day
            "fuel": unit_size * 0.5 * days,  # 0.5L/person/day
            "ammo": unit_size * 10 * days  # 10 rounds/day combat
        }
```

**Transport Capacity:**
- Horse cart: 500 kg
- Wagon: 1,500 kg
- Truck (Tier 5): 5,000 kg
- Supply convoy rules

### Category 2: Unit Organization (Tier 3-5)

**Command Structure:**
- Squad: 8-12 soldiers
- Platoon: 30-50 soldiers (3-4 squads)
- Company: 100-200 soldiers (3-4 platoons)
- Battalion: 400-1000 soldiers (4-6 companies)

**Command Span of Control:**
```csharp
public class CommandSystem
{
    public int GetMaxSubordinates(CommandLevel level, SituationComplexity complexity)
    {
        int baseSpan = level switch
        {
            CommandLevel.Squad => 3,         // Squad leader: 3 fireteams
# Military Manuals Collection: Logistics, Tactics, and Large-Scale Operations Extraction Guide

---
title: Military Manuals Content Extraction for Planet-Scale MMORPG Warfare Systems
date: 2025-01-15
tags: [content-extraction, military, logistics, warfare, tactics, supply-chain]
priority: high
status: ready-for-implementation
---

**Document Type:** Content Extraction Guide  
**Source:** awesome-survival Military Manuals Collection (22,000+ documents)  
**Version:** 1.0  
**Author:** Game Design Research Team  
**Date:** 2025-01-15  
**Target Audience:** Content designers, system architects, gameplay programmers

## Executive Summary

The Military Manuals Collection contains 22,000+ field manuals, tactical guides, and logistics documentation from military organizations worldwide. This extraction guide focuses on large-scale warfare systems, supply chain logistics, strategic infrastructure, and fortification mechanics that scale from small skirmishes (10-50 players) to planetary warfare (1000+ players coordinating over weeks).

**Key Deliverables:**
- 200+ warfare mechanics (tactics, logistics, command structures)
- Supply chain systems for military operations
- Fortification and siege mechanics
- Strategic infrastructure (communications, supply lines, intelligence)
- 12-week extraction timeline with phase-based deliverables

**BlueMarble Integration:**
- Tier 3-5 military systems (organized warfare → planetary conflict)
- Player-led faction warfare and territory control
- Resource logistics and supply chain gameplay
- Strategic decision-making and command hierarchy

---

## Table of Contents

1. [Source Analysis](#source-analysis)
2. [Extraction Strategy](#extraction-strategy)
3. [Content Categories](#content-categories)
4. [Timeline and Phases](#timeline-and-phases)
5. [Tools and Setup](#tools-and-setup)
6. [Extraction Methodology](#extraction-methodology)
7. [Game Integration](#game-integration)
8. [Quality Assurance](#quality-assurance)
9. [Success Metrics](#success-metrics)
10. [Appendices](#appendices)

---

## Source Analysis

### Collection Overview

**Size:** 22,000+ documents  
**Format:** PDF manuals, field guides, tactical handbooks  
**Content Types:**
- Field manuals (doctrine, procedures, standards)
- Tactical guides (combat operations, unit coordination)
- Logistics manuals (supply chain, transportation, maintenance)
- Engineering guides (fortification, demolition, construction)
- Intelligence manuals (reconnaissance, information warfare)
- Command and control documentation

### Priority Manual Categories

#### 1. Logistics and Supply Chain (Priority: Critical)
- **FM 4-0**: Sustainment Operations
- **FM 4-30**: Ordnance Operations
- **FM 10-27**: General Supply in Theaters of Operations
- **ATP 4-42**: Movement Control
- Supply chain planning, distribution, and maintenance systems

#### 2. Tactical Operations (Priority: High)
- **FM 3-90**: Offense and Defense
- **FM 3-21**: Infantry Battalion Operations
- **FM 6-0**: Commander and Staff Organization
- Small unit tactics scaling to combined arms operations

#### 3. Engineering and Fortification (Priority: High)
- **FM 3-34**: Engineer Operations
- **TC 3-34.40**: General Engineering
- **FM 5-103**: Survivability
- Defensive construction, obstacle creation, fortification systems

#### 4. Command and Control (Priority: Medium)
- **FM 6-0**: Mission Command
- **ATP 6-0.5**: Command Post Organization
- Command hierarchy, decision-making processes, coordination

#### 5. Intelligence and Reconnaissance (Priority: Medium)
- **FM 2-0**: Intelligence
- **ATP 2-01**: Intelligence Support
- Information gathering, analysis, and dissemination systems

### Relevance to Planet-Scale MMORPG

Military manuals provide frameworks for:
- **Large-scale coordination**: Command structures for 100-1000+ player operations
- **Resource logistics**: Supply chains, transportation networks, stockpile management
- **Strategic infrastructure**: Communication systems, supply depots, forward bases
- **Tactical gameplay**: Unit composition, combined arms tactics, terrain utilization
- **Siege warfare**: Fortification construction, siege equipment, defensive systems
- **Territory control**: Occupation, defense, and expansion mechanics

---

## Extraction Strategy

### Phase-Based Approach

#### Phase 1: Logistics Systems (Weeks 1-4)
Focus on supply chain, transportation, and resource management systems that form the foundation of large-scale military operations.

**Target Documents:**
- FM 4-0: Sustainment Operations
- FM 4-30: Ordnance Operations
- FM 10-27: General Supply in Theaters of Operations
- ATP 4-42: Movement Control

**Deliverables:**
- Supply chain mechanics (50+ systems)
- Transportation and logistics rules
- Stockpile and depot management
- Maintenance and repair systems

#### Phase 2: Tactical Operations (Weeks 5-7)
Extract small-unit tactics and combined arms operations that scale from squad (5-10 players) to battalion (100-200 players).

**Target Documents:**
- FM 3-90: Offense and Defense
- FM 3-21: Infantry Battalion Operations
- FM 3-0: Operations

**Deliverables:**
- Tactical doctrines (40+ tactics)
- Unit composition guidelines
- Terrain utilization mechanics
- Combat coordination systems

#### Phase 3: Engineering and Fortification (Weeks 8-10)
Extract construction, fortification, and siege mechanics for defensive and offensive infrastructure.

**Target Documents:**
- FM 3-34: Engineer Operations
- TC 3-34.40: General Engineering
- FM 5-103: Survivability

**Deliverables:**
- Fortification construction (30+ structures)
- Siege equipment and tactics
- Defensive systems and obstacles
- Engineering support mechanics

#### Phase 4: Command and Intelligence (Weeks 11-12)
Extract command hierarchy, decision-making frameworks, and intelligence systems for strategic gameplay.

**Target Documents:**
- FM 6-0: Mission Command
- ATP 6-0.5: Command Post Organization
- FM 2-0: Intelligence

**Deliverables:**
- Command structure mechanics (20+ systems)
- Decision-making frameworks
- Intelligence gathering and analysis
- Communication protocols

---

## Content Categories

### Category 1: Supply Chain and Logistics

#### Core Systems

**Supply Classes (Military Standard):**
```json
{
  "supply_classes": {
    "class_i": {
      "name": "Subsistence",
      "description": "Food, water, and survival rations",
      "consumption_rate": "per_player_per_day",
      "storage_requirements": "temperature_controlled",
      "transport_priority": "medium",
      "game_integration": "Player hunger/thirst systems, expedition supplies"
    },
    "class_ii": {
      "name": "Clothing and Equipment",
      "description": "Individual equipment, tools, administrative materials",
      "consumption_rate": "as_needed",
      "storage_requirements": "protected_from_elements",
      "transport_priority": "low",
      "game_integration": "Player equipment, uniform maintenance"
    },
    "class_iii": {
      "name": "Petroleum and Lubricants",
      "description": "Fuel, oils, lubricants for vehicles and machines",
      "consumption_rate": "per_vehicle_per_hour",
      "storage_requirements": "fireproof_containers",
      "transport_priority": "high",
      "game_integration": "Vehicle fuel systems, generator power, machinery operation"
    },
    "class_iv": {
      "name": "Construction Materials",
      "description": "Fortification materials, barrier materials, construction equipment",
      "consumption_rate": "per_project",
      "storage_requirements": "bulk_storage",
      "transport_priority": "medium",
      "game_integration": "Base construction, fortification building, siege preparations"
    },
    "class_v": {
      "name": "Ammunition",
      "description": "Projectiles, explosives, chemical weapons",
      "consumption_rate": "per_engagement",
      "storage_requirements": "secure_magazines",
      "transport_priority": "critical",
      "game_integration": "Combat supplies, ammunition expenditure, explosive crafting"
    },
    "class_vi": {
      "name": "Personal Demand Items",
      "description": "Comfort items, morale boosters, recreational supplies",
      "consumption_rate": "as_desired",
      "storage_requirements": "general",
      "transport_priority": "low",
      "game_integration": "Morale systems, player comfort, faction cohesion"
    },
    "class_vii": {
      "name": "Major End Items",
      "description": "Vehicles, weapons systems, major equipment",
      "consumption_rate": "replacement_only",
      "storage_requirements": "maintenance_facilities",
      "transport_priority": "special_handling",
      "game_integration": "Vehicle construction, heavy equipment deployment"
    },
    "class_viii": {
      "name": "Medical Materials",
      "description": "Medical supplies, pharmaceuticals, blood products",
      "consumption_rate": "per_casualty",
      "storage_requirements": "climate_controlled",
      "transport_priority": "high",
      "game_integration": "Medical systems, healing items, pharmaceutical crafting"
    },
    "class_ix": {
      "name": "Repair Parts",
      "description": "Spare parts, maintenance materials, repair supplies",
      "consumption_rate": "per_maintenance_cycle",
      "storage_requirements": "organized_warehouse",
      "transport_priority": "medium",
      "game_integration": "Item repair, vehicle maintenance, equipment durability"
    },
    "class_x": {
      "name": "Civil-Military Support",
      "description": "Agriculture, economic development, civic aid",
      "consumption_rate": "project_based",
      "storage_requirements": "varies",
      "transport_priority": "low",
      "game_integration": "Territory development, civilian relations, resource extraction"
    }
  }
}
```

**Supply Chain Mechanics:**

1. **Requisition System**
```csharp
public class MilitaryRequisition
{
    public string ItemClass { get; set; }
    public string ItemName { get; set; }
    public int Quantity { get; set; }
    public string Justification { get; set; }
    public Priority RequestPriority { get; set; }
    public string SupplyPoint { get; set; }
    
    public RequisitionStatus ProcessRequisition()
    {
        // Check authorization
        if (!IsAuthorizedRequester())
            return RequisitionStatus.Denied;
        
        // Check availability at supply point
        if (!CheckStockAvailability(SupplyPoint, ItemClass, ItemName, Quantity))
            return RequisitionStatus.Backordered;
        
        // Process based on priority
        int processingTime = CalculateProcessingTime(RequestPriority, Quantity);
        
        // Schedule transportation
        TransportOrder order = CreateTransportOrder(this);
        
        return RequisitionStatus.Approved;
    }
}

public enum Priority
{
    Routine = 1,      // 2-7 days delivery
    Priority = 2,     // 1-2 days delivery
    Immediate = 3,    // 12-24 hours delivery
    Flash = 4         // 6-12 hours delivery
}
```

2. **Transportation Network**
```csharp
public class MilitaryTransportation
{
    public List<TransportRoute> Routes { get; set; }
    public List<TransportAsset> AvailableAssets { get; set; }
    
    public TransportPlan CreateLogisticsChain(SupplyPoint origin, SupplyPoint destination, List<Supply> cargo)
    {
        // Calculate optimal route considering:
        // - Distance and terrain
        // - Security/threat level
        // - Road/infrastructure quality
        // - Asset availability
        
        TransportRoute route = FindOptimalRoute(origin, destination);
        
        // Determine asset requirements
        TransportAsset asset = SelectAppropriateAsset(cargo, route);
        
        // Calculate time and fuel
        TimeSpan transitTime = CalculateTransitTime(route, asset, cargo);
        float fuelRequired = CalculateFuelConsumption(route, asset, cargo);
        
        return new TransportPlan
        {
            Route = route,
            Asset = asset,
            TransitTime = transitTime,
            FuelRequired = fuelRequired,
            SecurityEscort = DetermineEscortNeeds(route.ThreatLevel)
        };
    }
}
```

3. **Supply Point Management**
```csharp
public class SupplyPoint
{
    public string PointId { get; set; }
    public Vector3 Location { get; set; }
    public SupplyPointType Type { get; set; }
    public Dictionary<string, int> Inventory { get; set; }
    public int StorageCapacity { get; set; }
    public List<string> ServicingUnits { get; set; }
    
    public bool CanSupport(string unitId, int playerCount, int daysSupply)
    {
        // Calculate consumption rates
        Dictionary<string, int> required = CalculateConsumption(unitId, playerCount, daysSupply);
        
        // Check inventory against requirements
        foreach (var item in required)
        {
            if (!Inventory.ContainsKey(item.Key) || Inventory[item.Key] < item.Value)
                return false;
        }
        
        return true;
    }
    
    public void ProcessResupply(List<Supply> incomingSupplies)
    {
        int totalVolume = incomingSupplies.Sum(s => s.Volume);
        
        if (GetCurrentStorageUsed() + totalVolume > StorageCapacity)
        {
            // Trigger overflow procedures
            HandleStorageOverflow(incomingSupplies);
        }
        
        // Add to inventory
        foreach (var supply in incomingSupplies)
        {
            if (Inventory.ContainsKey(supply.ItemId))
                Inventory[supply.ItemId] += supply.Quantity;
            else
                Inventory[supply.ItemId] = supply.Quantity;
        }
    }
}

public enum SupplyPointType
{
    FieldDepot,           // Forward supply point near combat
    IntermediateDepot,    // Regional distribution center
    BaseDepot,            // Major logistics hub
    StrategicStockpile    // Long-term reserves
}
```

### Category 2: Tactical Operations

#### Unit Composition and Organization

**Standard Unit Structures:**
```json
{
  "military_units": {
    "fireteam": {
      "size": "4-5 players",
      "composition": {
        "team_leader": 1,
        "rifleman": 2,
        "automatic_rifleman": 1,
        "grenadier": 1
      },
      "capabilities": ["patrol", "reconnaissance", "point_defense"],
      "command_span": "direct_voice",
      "effective_range": "100-200m"
    },
    "squad": {
      "size": "8-13 players",
      "composition": {
        "squad_leader": 1,
        "fireteams": 2
      },
      "capabilities": ["assault", "defense", "patrol", "ambush"],
      "command_span": "squad_radio",
      "effective_range": "500m"
    },
    "platoon": {
      "size": "25-40 players",
      "composition": {
        "platoon_leader": 1,
        "platoon_sergeant": 1,
        "squads": 3-4,
        "weapon_squad": 1
      },
      "capabilities": ["sustained_combat", "area_defense", "coordinated_assault"],
      "command_span": "tactical_radio_net",
      "effective_range": "2km"
    },
    "company": {
      "size": "100-200 players",
      "composition": {
        "company_commander": 1,
        "executive_officer": 1,
        "platoons": 3-4,
        "headquarters_section": 1,
        "support_platoon": 1
      },
      "capabilities": ["independent_operations", "base_defense", "offensive_operations"],
      "command_span": "command_post",
      "effective_range": "10km"
    },
    "battalion": {
      "size": "300-1000 players",
      "composition": {
        "battalion_commander": 1,
        "staff_officers": 5-10,
        "companies": 3-5,
        "headquarters_company": 1
      },
      "capabilities": ["major_operations", "strategic_objectives", "sustained_campaigns"],
      "command_span": "battalion_command_post",
      "effective_range": "50km"
    }
  }
}
```

**Tactical Doctrines:**

1. **Offensive Operations**
```csharp
public class OffensiveTactics
{
    public static TacticalPlan PlanMovementToContact(Unit unit, Vector3 objective)
    {
        // Movement formation based on terrain and threat
        Formation formation = DetermineFormation(unit, terrain, threatLevel);
        
        // Security elements
        List<Unit> securityElements = new List<Unit>
        {
            AssignAdvanceGuard(unit, 500m),    // 500m ahead
            AssignFlankSecurity(unit),          // On both flanks
            AssignRearGuard(unit, 200m)         // 200m behind
        };
        
        // Movement techniques
        MovementTechnique technique = SelectMovementTechnique(threatLevel);
        // - Traveling: Low threat, fast movement
        // - Traveling Overwatch: Moderate threat, one element covering
        // - Bounding Overwatch: High threat, alternating movement
        
        return new TacticalPlan
        {
            Formation = formation,
            SecurityElements = securityElements,
            MovementTechnique = technique,
            ObjectiveRallyPoint = CalculateORP(objective, 200m)
        };
    }
    
    public static AttackPlan PlanDeliberateAttack(Unit unit, DefensivePosition enemy)
    {
        // Reconnaissance and intelligence
        IntelligenceReport intel = ConductReconnaissance(enemy);
        
        // Determine form of maneuver
        ManeuverType maneuver = SelectManeuver(intel);
        // - Frontal: Direct assault
        // - Envelopment: Flank attack
        // - Turning Movement: Deep flank/rear attack
        // - Infiltration: Covert penetration
        // - Penetration: Breakthrough at weak point
        
        // Task organization
        AttackForce assault = unit.GetSubunit(0.6f);     // 60% for assault
        AttackForce support = unit.GetSubunit(0.3f);     // 30% for support by fire
        AttackForce reserve = unit.GetSubunit(0.1f);     // 10% for reserve
        
        // Fire support plan
        FireSupportPlan fires = CoordinateFireSupport(assault, support, enemy);
        
        return new AttackPlan
        {
            Maneuver = maneuver,
            AssaultForce = assault,
            SupportForce = support,
            Reserve = reserve,
            FireSupport = fires,
            PhaseLines = EstablishPhaseLines(objective)
        };
    }
}

public enum ManeuverType
{
    Frontal,
    Envelopment,
    TurningMovement,
    Infiltration,
    Penetration
}
```

2. **Defensive Operations**
```csharp
public class DefensiveTactics
{
    public static DefensivePlan PlanAreaDefense(Unit unit, Territory area)
    {
        // Terrain analysis
        TerrainAnalysis terrain = AnalyzeDefensiveTerrain(area);
        
        // Defensive position selection
        List<DefensivePosition> positions = SelectDefensivePositions(
            terrain.KeyTerrain,
            terrain.AvenuesOfApproach,
            terrain.ObservationFields
        );
        
        // Obstacle plan
        ObstaclePlan obstacles = PlanObstacles(
            terrain.AvenuesOfApproach,
            positions,
            unit.EngineerAssets
        );
        
        // Task organization
        DefenseForce main = unit.GetSubunit(0.5f);       // 50% main defense
        DefenseForce security = unit.GetSubunit(0.2f);   // 20% security zone
        DefenseForce reserve = unit.GetSubunit(0.2f);    // 20% reserve
        DefenseForce support = unit.GetSubunit(0.1f);    // 10% indirect fire
        
        return new DefensivePlan
        {
            DefensivePositions = positions,
            Obstacles = obstacles,
            MainDefense = main,
            SecurityZone = security,
            Reserve = reserve,
            FireSupport = support,
            FallbackPositions = PlanFallbackPositions(area)
        };
    }
    
    public static CounterAttackPlan PlanCounterAttack(Unit reserve, Vector3 penetration)
    {
        // Determine counterattack type
        CounterAttackType type = DetermineCounterAttackType(penetration);
        // - Spoiling: Disrupt enemy preparation
        // - Counterattack: Restore original position
        // - Counteroffensive: Major offensive to destroy enemy
        
        // Assembly area
        Vector3 assemblyArea = SelectAssemblyArea(reserve, penetration);
        
        // Attack corridor
        AttackCorridor corridor = DetermineAttackCorridor(
            assemblyArea,
            penetration,
            friendlyLines
        );
        
        return new CounterAttackPlan
        {
            Type = type,
            AssemblyArea = assemblyArea,
            AttackCorridor = corridor,
            SupportingFires = CoordinateCounterAttackFires(corridor),
            ExecutionTime = CalculateOptimalTiming(penetration)
        };
    }
}
```

### Category 3: Engineering and Fortification

#### Fortification Construction

**Defensive Structure Types:**
```json
{
  "fortifications": {
    "fighting_position": {
      "name": "Individual Fighting Position",
      "construction_time": "2-4 hours",
      "materials": ["shovel", "sandbags", "camouflage"],
      "protection_level": "small_arms",
      "capacity": "1-2 players",
      "features": ["overhead_cover", "grenade_sump", "camouflage_net"]
    },
    "bunker": {
      "name": "Reinforced Bunker",
      "construction_time": "2-3 days",
      "materials": ["timber", "concrete", "steel_reinforcement", "sandbags"],
      "protection_level": "heavy_weapons",
      "capacity": "4-8 players",
      "features": ["firing_ports", "overhead_protection", "blast_doors", "ventilation"]
    },
    "trench_line": {
      "name": "Trench System",
      "construction_time": "1-2 weeks",
      "materials": ["engineering_equipment", "revetment_materials", "drainage_pipe"],
      "protection_level": "artillery",
      "capacity": "20-50 players per 100m",
      "features": ["communication_trenches", "dugouts", "traverses", "drainage"]
    },
    "fortified_base": {
      "name": "Forward Operating Base",
      "construction_time": "2-4 weeks",
      "materials": ["hesco_barriers", "concrete", "steel", "wire_obstacles"],
      "protection_level": "sustained_assault",
      "capacity": "100-500 players",
      "features": ["perimeter_defenses", "command_post", "supply_depot", "barracks", "medical_facility"]
    },
    "fortress": {
      "name": "Strategic Fortress",
      "construction_time": "3-6 months",
      "materials": ["concrete", "steel", "advanced_materials", "heavy_equipment"],
      "protection_level": "siege_weapons",
      "capacity": "1000+ players",
      "features": ["curtain_walls", "towers", "gates", "underground_bunkers", "supply_stockpiles"]
    }
  }
}
```

**Obstacle Systems:**

1. **Wire Obstacles**
```csharp
public class WireObstacle
{
    public WireObstacleType Type { get; set; }
    public float Length { get; set; }
    public int RequiredMaterials { get; set; }
    public TimeSpan ConstructionTime { get; set; }
    
    public static WireObstacle CreateConcertina(float length)
    {
        // Concertina wire: Quick deployment, low protection
        return new WireObstacle
        {
            Type = WireObstacleType.Concertina,
            Length = length,
            RequiredMaterials = (int)(length / 15),  // 1 roll per 15m
            ConstructionTime = TimeSpan.FromMinutes(length * 2),
            MovementDelay = TimeSpan.FromSeconds(30),
            CanopyHeight = 1.0f
        };
    }
    
    public static WireObstacle CreateDoubleApron(float length)
    {
        // Double apron fence: More secure, slower to construct
        return new WireObstacle
        {
            Type = WireObstacleType.DoubleApron,
            Length = length,
            RequiredMaterials = (int)(length / 10),  // More material intensive
            ConstructionTime = TimeSpan.FromMinutes(length * 5),
            MovementDelay = TimeSpan.FromMinutes(2),
            CanopyHeight = 1.5f
        };
    }
}

public enum WireObstacleType
{
    Concertina,       // Quick deployment
    SingleApron,      // Standard fence
    DoubleApron,      // High security
    LowWire,          // Anti-personnel
    TangleWire        // Area denial
}
```

2. **Minefield Design**
```csharp
public class Minefield
{
    public MinefieldType Type { get; set; }
    public Polygon Area { get; set; }
    public int MineCount { get; set; }
    public float MineDensity { get; set; }  // Mines per square meter
    
    public static Minefield CreateProtectiveMinefield(DefensivePosition position, float depth)
    {
        // Protective: Directly in front of defensive position
        Polygon area = CreateMinefieldArea(position.Frontage, depth);
        
        return new Minefield
        {
            Type = MinefieldType.Protective,
            Area = area,
            MineDensity = 0.05f,  // 1 mine per 20 m²
            MineCount = (int)(area.AreaSquareMeters * 0.05f),
            ConstructionTime = CalculateMineLayingTime(area.AreaSquareMeters),
            Marking = MinefieldMarking.Recorded,
            Coverage = "Integrated with wire obstacles"
        };
    }
    
    public static Minefield CreateInterdictionMinefield(Vector3 location, float radius)
    {
        // Interdiction: Deny terrain or routes to enemy
        Circle area = new Circle(location, radius);
        
        return new Minefield
        {
            Type = MinefieldType.Interdiction,
            Area = area.ToPolygon(),
            MineDensity = 0.02f,  // 1 mine per 50 m²
            MineCount = (int)(area.AreaSquareMeters * 0.02f),
            ConstructionTime = CalculateMineLayingTime(area.AreaSquareMeters),
            Marking = MinefieldMarking.Phony,  // Fake markers to deceive
            Coverage = "Scattered pattern"
        };
    }
}

public enum MinefieldType
{
    Protective,       // Defend position
    Interdiction,     // Deny area
    Harassing,        // Slow enemy
    Phony,            // Deception
    Nuisance          // Psychological
}
```

3. **Anti-Vehicle Obstacles**
```csharp
public class AntiVehicleObstacle
{
    public static Obstacle CreateTankDitch(float length, float depth)
    {
        // Tank ditch: Impassable to vehicles
        return new Obstacle
        {
            Type = ObstacleType.TankDitch,
            Dimensions = new Vector3(length, 3.0f, depth),  // 3m wide standard
            ConstructionTime = TimeSpan.FromHours(length / 10),  // 10m per hour with excavator
            RequiredEquipment = "Excavator or bulldozer",
            EffectivenessVehicles = 1.0f,  // 100% effective
            EffectivenessInfantry = 0.1f   // 10% delay
        };
    }
    
    public static Obstacle CreateDragonTeeth(int rows, int count)
    {
        // Dragon's teeth: Concrete pyramids
        return new Obstacle
        {
            Type = ObstacleType.DragonTeeth,
            Count = count,
            Rows = rows,
            ConstructionTime = TimeSpan.FromDays(count / 50),  // 50 per day
            RequiredMaterials = $"{count * 0.5f} m³ concrete",
            EffectivenessVehicles = 0.8f,  // 80% effective (can be bypassed with effort)
            EffectivenessInfantry = 0.0f   // No effect on infantry
        };
    }
    
    public static Obstacle CreateAbatis(float length, string vegetation)
    {
        // Abatis: Felled trees
        return new Obstacle
        {
            Type = ObstacleType.Abatis,
            Length = length,
            ConstructionTime = TimeSpan.FromHours(length / 5),  // 5m per hour
            RequiredEquipment = "Chainsaw or axe",
            EffectivenessVehicles = 0.6f,  // 60% effective
            EffectivenessInfantry = 0.3f,  // 30% delay
            VegetationRequired = vegetation
        };
    }
}
```

### Category 4: Command and Control

#### Command Structure

**Command Hierarchy:**
```csharp
public class CommandHierarchy
{
    public CommandLevel Level { get; set; }
    public Player Commander { get; set; }
    public List<Unit> SubordinateUnits { get; set; }
    public int SpanOfControl { get; set; }  // Number of direct subordinates
    
    public static int GetOptimalSpanOfControl(CommandLevel level, SituationComplexity complexity)
    {
        // Military doctrine: 3-5 subordinate units optimal
        int baseSpan = level switch
        {
            CommandLevel.Team => 4,          // Team leader: 4 soldiers
            CommandLevel.Squad => 2,         // Squad leader: 2 fire teams
            CommandLevel.Platoon => 4,       // Platoon leader: 4 squads
            CommandLevel.Company => 4,       // Company commander: 4 platoons
            CommandLevel.Battalion => 5,     // Battalion commander: 5 companies
            _ => 3
        };
        
        // Adjust for complexity
        return complexity switch
        {
            SituationComplexity.Low => baseSpan + 2,
            SituationComplexity.Moderate => baseSpan,
            SituationComplexity.High => baseSpan - 1,
            SituationComplexity.Extreme => baseSpan - 2,
            _ => baseSpan
        };
    }
}

public enum CommandLevel
{
    Team,
    Squad,
    Platoon,
    Company,
    Battalion,
    Regiment,
    Division
}

public enum SituationComplexity
{
    Low,        // Routine operations
    Moderate,   // Normal combat
    High,       // Complex environment
    Extreme     // Multi-threat, chaotic
}
```

### Category 3: Tactical Formations (Tier 3-4)

**Infantry Formations:**
- Line formation (maximum firepower)
- Column formation (rapid movement)
- Wedge formation (assault)
- Defensive positions

**Movement Techniques:**
- Traveling (low threat)
- Traveling overwatch (moderate threat)
- Bounding overwatch (high threat)

### Category 4: Fortification (Tier 2-5)

**Field Fortifications:**
- Fighting positions (1-2 person)
- Trench systems
- Bunkers and strongpoints
- Anti-vehicle obstacles
- Wire obstacles

## Game Integration

### Warfare Game Mechanics

**Player Roles:**
- Private: Individual combat
- Squad Leader: 8-12 NPCs/players
- Platoon Leader: 30-50 troops
- Company Commander: 100-200 troops

**Supply Management:**
```json
{
  "company_logistics": {
    "personnel": 120,
    "daily_consumption": {
      "food": 360,
      "water": 1800,
      "ammunition": 1200
    },
    "supply_points_needed": 3,
    "transport_capacity": {
      "wagons": 5,
      "carrying_capacity_kg": 7500
    }
  }
}
```

## Extraction Methodology

### Step 1: Document Selection

Priority documents:
1. FM 4-0 (Sustainment) - Logistics mechanics
2. FM 3-0 (Operations) - Tactical doctrine
3. FM 5-34 (Engineer Field Data) - Fortifications
4. FM 7-8 (Infantry Platoon and Squad) - Unit tactics

### Step 2: Text Extraction

    High,       // Complex operations
    Extreme     // Multi-domain operations
}
```

**Mission Command:**
```csharp
public class MissionOrder
{
    // Standard 5-paragraph military order format
    
    // 1. SITUATION
    public SituationParagraph Situation { get; set; }
    
    // 2. MISSION
    public string Mission { get; set; }  // Who, what, when, where, why
    
    // 3. EXECUTION
    public ExecutionParagraph Execution { get; set; }
    
    // 4. SUSTAINMENT
    public SustainmentParagraph Sustainment { get; set; }
    
    // 5. COMMAND AND SIGNAL
    public CommandSignalParagraph CommandSignal { get; set; }
    
    public static MissionOrder CreateFragmentaryOrder(MissionOrder baseOrder, string changes)
    {
        // FRAGO: Quick update to existing order
        return new MissionOrder
        {
            OrderType = OrderType.Fragmentary,
            BaseOrder = baseOrder,
            Changes = changes,
            TimeOfChange = DateTime.UtcNow
        };
    }
}

public class SituationParagraph
{
    public EnemyForces Enemy { get; set; }
    public FriendlyForces Friendly { get; set; }
    public AttachmentsParagraph Attachments { get; set; }
    public DetachmentsParagraph Detachments { get; set; }
}

public class ExecutionParagraph
{
    public string CommanderIntent { get; set; }  // Commander's vision of end state
    public ConceptOfOperations Concept { get; set; }
    public List<TaskAssignment> Tasks { get; set; }
    public CoordinatingInstructions Coordinating { get; set; }
}
```

**Decision-Making Process:**
```csharp
public class MilitaryDecisionMakingProcess
{
    // MDMP: Structured approach for planning
    
    public static OperationPlan ExecuteMDMP(Mission mission)
    {
        // Step 1: Receipt of Mission
        MissionAnalysis analysis = AnalyzeMission(mission);
        
        // Step 2: Mission Analysis
        RestatedMission restated = RestatemeMission(mission, analysis);
        
        // Step 3: Course of Action Development
        List<CourseOfAction> coas = DevelopCoursesOfAction(restated);
        
        // Step 4: Course of Action Analysis (Wargaming)
        List<WargameResult> wargamed = WargameCoursesOfAction(coas);
        
        // Step 5: Course of Action Comparison
        CourseOfAction selected = CompareCOAs(wargamed);
        
        // Step 6: Course of Action Approval
        if (CommanderApproval(selected))
        {
            // Step 7: Orders Production
            return ProduceOperationOrder(selected);
        }
        
        return null;
    }
    
    public static List<CourseOfAction> DevelopCoursesOfAction(RestatedMission mission)
    {
        List<CourseOfAction> coas = new List<CourseOfAction>();
        
        // Develop multiple options (typically 2-3)
        for (int i = 0; i < 3; i++)
        {
            CourseOfAction coa = new CourseOfAction
            {
                Name = $"COA {i + 1}",
                PurposeOrTask = mission.Task,
                MethodOrConcept = GenerateMethod(mission, i),
                EndState = mission.DesiredEndState
            };
            
            coas.Add(coa);
        }
        
        return coas;
    }
}
```

### Category 5: Intelligence and Reconnaissance

#### Intelligence Collection

**Collection Methods:**
```csharp
public class IntelligenceCollection
{
    public static ReconnaissancePlan PlanReconnaissance(Territory area, Priority priority)
    {
        // Determine reconnaissance type
        ReconType type = DetermineReconType(area, priority);
        
        // Assign collection assets
        List<ReconAsset> assets = AssignReconAssets(type, area);
        
        // Named Areas of Interest (NAI)
        List<NAI> namedAreas = IdentifyNAI(area);
        
        // Intelligence Requirements
        List<IntelRequirement> requirements = DefineIntelRequirements(priority);
        
        return new ReconnaissancePlan
        {
            Type = type,
            Assets = assets,
            NamedAreas = namedAreas,
            Requirements = requirements,
            ReportingSchedule = DetermineReportingSchedule(priority)
        };
    }
}

public enum ReconType
{
    RouteReconnaissance,      // Specific route
    ZoneReconnaissance,       // Designated area
    AreaReconnaissance,       // Specific location
    ReconnaissanceInForce,    // Large-scale probing
    SpecialReconnaissance     // Covert intelligence gathering
}

public class IntelRequirement
{
    public string Requirement { get; set; }
    public Priority Priority { get; set; }
    public DateTime LatestAcceptableTime { get; set; }  // LTIOV
    
    // Priority Intelligence Requirements (PIR)
    public static List<IntelRequirement> GetCommandersPIR()
    {
        return new List<IntelRequirement>
        {
            new IntelRequirement 
            { 
                Requirement = "Where are enemy main defensive positions?",
                Priority = Priority.Flash
            },
            new IntelRequirement 
            { 
                Requirement = "What is enemy reserve strength and location?",
                Priority = Priority.Immediate
            },
            new IntelRequirement 
            { 
                Requirement = "What are enemy supply routes and stockpile locations?",
                Priority = Priority.Priority
            }
        };
    }
}
```

**Intelligence Analysis:**
```csharp
public class IntelligenceAnalysis
{
    public static ThreatAssessment AnalyzeEnemyCapabilities(List<IntelReport> reports)
    {
        // IPB: Intelligence Preparation of the Battlefield
        
        // Step 1: Define battlefield environment
        BattlefieldEnvironment environment = DefineBattlefield(reports);
        
        // Step 2: Describe battlefield effects
        TerrainAnalysis terrain = AnalyzeTerrain(environment);
        WeatherAnalysis weather = AnalyzeWeather(environment);
        
        // Step 3: Evaluate the threat
        EnemyAnalysis enemy = AnalyzeEnemy(reports);
        
        // Step 4: Determine threat courses of action
        List<EnemyCOA> enemyCOAs = DetermineEnemyCOAs(enemy, terrain);
        
        return new ThreatAssessment
        {
            Environment = environment,
            Terrain = terrain,
            Weather = weather,
            EnemyAnalysis = enemy,
            EnemyCOAs = enemyCOAs,
            MostLikelyCOA = enemyCOAs.OrderByDescending(c => c.Probability).First(),
            MostDangerousCOA = enemyCOAs.OrderByDescending(c => c.ThreatLevel).First()
        };
    }
}
```

---

## Timeline and Phases

### 12-Week Extraction Plan

#### Week 1-2: Logistics Foundation
**Primary Documents:** FM 4-0, FM 10-27, ATP 4-42

**Deliverables:**
- Supply classification system (10 classes)
- Requisition and distribution mechanics
- Transportation network rules
- Storage and depot management systems

**Estimated Output:** 150-200 lines

#### Week 3-4: Logistics Implementation
**Primary Documents:** FM 4-30, Logistics handbooks

**Deliverables:**
- Consumption rate calculations
- Maintenance systems
- Fuel and ammunition management
- Medical supply chains

**Estimated Output:** 100-150 lines

#### Week 5-6: Tactical Operations
**Primary Documents:** FM 3-90, FM 3-21, FM 3-0

**Deliverables:**
- Unit composition guidelines (fireteam → battalion)
- Offensive tactics (movement, assault, exploitation)
- Defensive tactics (position, obstacles, counterattack)
- Tactical coordination mechanics

**Estimated Output:** 150-200 lines

#### Week 7: Tactical Integration
**Primary Documents:** Combined arms manuals

**Deliverables:**
- Combined arms coordination
- Fire support integration
- Air-ground coordination
- Urban operations

**Estimated Output:** 80-100 lines

#### Week 8-9: Engineering and Fortification
**Primary Documents:** FM 3-34, TC 3-34.40, FM 5-103

**Deliverables:**
- Fortification construction (5 tiers)
- Obstacle systems (wire, mines, barriers)
- Siege equipment and tactics
- Demolition and breach operations

**Estimated Output:** 120-150 lines

#### Week 10: Engineering Support
**Primary Documents:** Engineering manuals

**Deliverables:**
- Engineer support to operations
- Route clearance
- Infrastructure repair
- Field fortifications

**Estimated Output:** 60-80 lines

#### Week 11: Command and Control
**Primary Documents:** FM 6-0, ATP 6-0.5

**Deliverables:**
- Command hierarchy and span of control
- Mission orders format
- Decision-making processes (MDMP)
- Coordination procedures

**Estimated Output:** 80-100 lines

#### Week 12: Intelligence Systems
**Primary Documents:** FM 2-0, ATP 2-01

**Deliverables:**
- Reconnaissance planning
- Intelligence collection
- Analysis frameworks (IPB)
- Reporting systems

**Estimated Output:** 60-80 lines

**Total Estimated Output:** 800-1,060 lines

---

## Tools and Setup

### Required Tools

1. **PDF Readers and Extractors**
   - Adobe Acrobat Reader DC
   - PDFtk (PDF toolkit for command-line operations)
   - Tabula (for extracting tables from PDFs)

2. **Text Processing**
   - Python with PyPDF2 library
   - Regular expressions for pattern matching
   - Natural Language Processing (spaCy or NLTK)

3. **Data Organization**
   - Notion or Obsidian for knowledge management
   - Spreadsheet software for data organization
   - JSON/YAML editors for game data formats

4. **Version Control**
   - Git for tracking extraction progress
   - GitHub/GitLab for collaboration

### Setup Instructions

```bash
# Install PDF processing tools
pip install PyPDF2 pdfplumber tabula-py

# Install NLP tools
pip install spacy nltk

# Download spaCy language model
python -m spacy download en_core_web_sm

# Create project structure
mkdir -p military_manuals_extraction/{pdfs,json,docs,scripts}
cd military_manuals_extraction

# Initialize git repository
git init
```

### Access to Military Manuals

**Public Sources:**
- Army Publishing Directorate (APD): https://armypubs.army.mil/
- Marine Corps Publications: https://www.marines.mil/News/Publications/
- Field Manual Archives: Various open-source repositories

**Note:** Use only publicly available, unclassified materials.

---

## Extraction Methodology

### Step-by-Step Process

#### Step 1: Document Selection and Download
```python
import requests
import os

# List of priority field manuals
priority_manuals = [
    {"id": "FM 4-0", "title": "Sustainment Operations", "url": "..."},
    {"id": "FM 3-90", "title": "Offense and Defense", "url": "..."},
    {"id": "FM 3-34", "title": "Engineer Operations", "url": "..."},
    # ... more manuals
]

def download_manual(manual):
    """Download military manual from public source"""
    response = requests.get(manual['url'])
    filename = f"pdfs/{manual['id'].replace(' ', '_')}.pdf"
    
    with open(filename, 'wb') as f:
        f.write(response.content)
    
    print(f"Downloaded: {manual['title']}")

# Download all priority manuals
for manual in priority_manuals:
    download_manual(manual)
```

#### Step 2: Text Extraction
```python
import PyPDF2
import pdfplumber

def extract_text_from_pdf(pdf_path):
    """Extract text content from PDF"""
    with open(pdf_path, 'rb') as file:
        reader = PyPDF2.PdfReader(file)
        text = ""
        
        for page in reader.pages:
            text += page.extract_text()
    
    return text

def extract_tables_from_pdf(pdf_path):
    """Extract tables from PDF"""
    tables = []
    
    with pdfplumber.open(pdf_path) as pdf:
        for page in pdf.pages:
            page_tables = page.extract_tables()
            tables.extend(page_tables)
    
    return tables

# Example usage
fm_4_0_text = extract_text_from_pdf("pdfs/FM_4-0.pdf")
supply_tables = extract_tables_from_pdf("pdfs/FM_4-0.pdf")
```

### Step 3: Content Analysis

**Pattern Matching for Mechanics:**
```python
import re

def extract_supply_rates(text):
    """Extract supply consumption rates"""
    # Look for patterns like "X units per Y time"
    pattern = r"(\d+\.?\d*)\s*(\w+)\s+per\s+(\w+)"
    matches = re.findall(pattern, text)
    return matches

def extract_organization(text):
    """Extract unit organization structures"""
    # Look for tables of organization
    pattern = r"(Squad|Platoon|Company|Battalion)\s*:\s*(\d+)\s*(personnel|soldiers)"
    matches = re.findall(pattern, text, re.IGNORECASE)
    return matches
```

### Step 4: JSON Conversion

#### Step 3: Content Analysis and Categorization
```python
import spacy
import re

nlp = spacy.load("en_core_web_sm")

def identify_game_mechanics(text):
    """Identify content relevant to game mechanics"""
    doc = nlp(text)
    
    mechanics = {
        "systems": [],
        "procedures": [],
        "calculations": [],
        "requirements": []
    }
    
    # Look for procedural language
    for sent in doc.sents:
        if re.search(r'\b(shall|must|will|required)\b', sent.text, re.IGNORECASE):
            mechanics["procedures"].append(sent.text)
        
        if re.search(r'\b(calculate|determine|estimate)\b', sent.text, re.IGNORECASE):
            mechanics["calculations"].append(sent.text)
        
        if re.search(r'\b(system|process|method)\b', sent.text, re.IGNORECASE):
            mechanics["systems"].append(sent.text)
    
    return mechanics

# Analyze logistics manual
logistics_mechanics = identify_game_mechanics(fm_4_0_text)
```

#### Step 4: JSON Conversion
```python
import json

def convert_to_game_data(mechanics, category):
    """Convert extracted mechanics to game data format"""
    game_data = {
        "category": category,
        "mechanics": [],
        "metadata": {
            "source": "FM 4-0",
            "extraction_date": "2025-01-15",
            "game_tier": "3-5"
        }
    }
    
    for mechanic in mechanics:
        game_data["mechanics"].append({
            "name": extract_mechanic_name(mechanic),
            "description": mechanic,
            "requirements": extract_requirements(mechanic),
            "effects": extract_effects(mechanic)
        })
    
    return game_data

# Convert and save
logistics_data = convert_to_game_data(logistics_mechanics["systems"], "logistics")
with open("game_data_logistics.json", "w") as f:
    json.dump(logistics_data, f, indent=2)
```

## Deliverables

1. **mechanics_logistics.json** - Supply chain systems
2. **mechanics_organization.json** - Unit structures and command
3. **mechanics_tactics.json** - Formation and movement
4. **mechanics_fortification.json** - Defensive structures
5. **warfare_system_documentation.md** - Complete integration guide

## Success Metrics

- **Authenticity:** 100% based on real military doctrine
- **Balance:** Scaled appropriately for game
- **Complexity:** Manageable for players
- **Integration:** Works with existing systems

## Tools and Setup

### Required Tools

1. **PDF Readers and Extractors**
   - Adobe Acrobat Reader DC
   - PDFtk (PDF toolkit for command-line operations)
   - Tabula (for extracting tables from PDFs)

2. **Text Processing**
   - Python with PyPDF2 library
   - Regular expressions for pattern matching
   - Natural Language Processing (spaCy or NLTK)

3. **Data Organization**
   - Notion or Obsidian for knowledge management
   - Spreadsheet software for data organization
   - JSON/YAML editors for game data formats

4. **Version Control**
   - Git for tracking extraction progress
   - GitHub/GitLab for collaboration

---

**Document Status:** Complete
**Last Updated:** 2025-01-15
**Priority:** MEDIUM
**Estimated Time:** 8 weeks

with open("json/logistics_systems.json", 'w') as f:
    json.dump(logistics_data, f, indent=2)
```

#### Step 5: Game Integration Specification
```python
def create_integration_spec(game_data, existing_systems):
    """Create specifications for integrating with existing game systems"""
    spec = {
        "new_mechanics": game_data["mechanics"],
        "system_dependencies": [],
        "integration_points": []
    }
    
    # Identify dependencies on existing systems
    for mechanic in game_data["mechanics"]:
        if requires_crafting_system(mechanic):
            spec["system_dependencies"].append("crafting_system")
        
        if requires_skill_system(mechanic):
            spec["system_dependencies"].append("skill_system")
    
    # Define integration points
    spec["integration_points"] = identify_integration_points(
        game_data,
        existing_systems
    )
    
    return spec
```

---

## Game Integration

### BlueMarble System Mapping

#### Integration with Existing Systems

**1. Crafting System Integration**
```json
{
  "integration": "military_logistics_to_crafting",
  "mapping": {
    "supply_class_iv": {
      "game_system": "construction_materials",
      "recipes": [
        "fortification_bunker",
        "trench_system",
        "wire_obstacles"
      ]
    },
    "supply_class_v": {
      "game_system": "ammunition_crafting",
      "recipes": [
        "rifle_ammunition",
        "artillery_shells",
        "explosives"
      ]
    },
    "supply_class_ix": {
      "game_system": "repair_materials",
      "recipes": [
        "vehicle_repair_kit",
        "weapon_maintenance_kit",
        "equipment_spare_parts"
      ]
    }
  }
}
```

**2. Skill System Integration**
```json
{
  "integration": "military_roles_to_skills",
  "skill_trees": {
    "logistics_specialist": {
      "tier": 4,
      "prerequisite_skills": ["basic_inventory_management", "transportation"],
      "unlocks": [
        "supply_requisition",
        "convoy_planning",
        "depot_management",
        "consumption_forecasting"
      ]
    },
    "combat_engineer": {
      "tier": 4,
      "prerequisite_skills": ["construction", "demolitions_basics"],
      "unlocks": [
        "fortification_construction",
        "obstacle_placement",
        "minefield_design",
        "breach_operations"
      ]
    },
    "tactical_commander": {
      "tier": 5,
      "prerequisite_skills": ["small_unit_tactics", "communication"],
      "unlocks": [
        "operation_planning",
        "mission_orders",
        "coordination_management",
        "strategic_decision_making"
      ]
    }
  }
}
```

**3. Territory Control Integration**
```json
{
  "integration": "military_operations_to_territory",
  "mechanics": {
    "area_defense": {
      "enables": "territory_holding",
      "requirements": [
        "defensive_positions",
        "supply_lines",
        "communication_network"
      ],
      "effects": [
        "control_point_capture_resistance",
        "enemy_movement_slowdown",
        "resource_extraction_protection"
      ]
    },
    "offensive_operation": {
      "enables": "territory_capture",
      "requirements": [
        "assault_force",
        "logistics_support",
        "intelligence_on_target"
      ],
      "effects": [
        "control_point_capture",
        "enemy_displacement",
        "resource_access"
      ]
    }
  }
}
```

### Player Progression Path

**Military Career Progression:**
```
Tier 1: Individual Soldier (Hours 0-50)
├─ Basic Combat Skills
├─ Equipment Maintenance
└─ Small Unit Tactics

Tier 2: Team Leader (Hours 50-150)
├─ Fire Team Leadership
├─ Tactical Communication
└─ Basic Logistics

Tier 3: Squad/Platoon Leader (Hours 150-400)
├─ Multi-Unit Coordination
├─ Supply Chain Management
└─ Defensive Planning

Tier 4: Company Commander (Hours 400-800)
├─ Large-Scale Operations
├─ Advanced Logistics
└─ Engineering Support

Tier 5: Battalion Commander (Hours 800+)
├─ Strategic Planning
├─ Multi-Domain Operations
└─ Campaign Management
```

---

## Quality Assurance

### Validation Process

#### 1. Historical Accuracy Review
- Cross-reference with multiple military sources
- Verify terminology and procedures
- Ensure doctrinal consistency

#### 2. Game Balance Review
```python
def validate_game_balance(mechanic):
    """Validate that military mechanic is balanced for gameplay"""
    checks = {
        "resource_cost": check_resource_balance(mechanic),
        "time_investment": check_time_balance(mechanic),
        "player_coordination": check_coordination_requirements(mechanic),
        "counter_play": check_counter_mechanics(mechanic)
    }
    
    # All checks must pass
    return all(checks.values())
```

#### 3. Integration Testing
- Test with existing crafting systems
- Validate skill tree progression
- Ensure territory control mechanics work correctly

#### 4. Playability Testing
- Verify mechanics are understandable
- Ensure proper tutorial/documentation
- Test with various player group sizes

### Quality Metrics

**Extraction Quality:**
- Accuracy: 95%+ (verified against source documents)
- Completeness: 90%+ (all major systems extracted)
- Clarity: 85%+ (readable by development team)

**Game Integration Quality:**
- Balance: All mechanics pass balance review
- Consistency: 95%+ consistent with existing systems
- Usability: 80%+ positive playtest feedback

---

## Success Metrics

### Extraction Success Criteria

✅ **200+ warfare mechanics extracted** (tactics, logistics, command)  
✅ **Complete supply chain system** (10 supply classes with consumption rates)  
✅ **Fortification progression** (5 tiers from fighting position to fortress)  
✅ **Command hierarchy framework** (team → battalion with span of control)  
✅ **12-week timeline completed** on schedule

### Integration Success Criteria

✅ **Military careers integrated** with existing skill trees  
✅ **Large-scale warfare enabled** (100+ player operations)  
✅ **Logistics gameplay functional** (supply chains, transport, stockpiles)  
✅ **Territory control enhanced** with military mechanics  
✅ **Player feedback positive** (playtest approval 75%+)

### Impact Metrics

**Gameplay Depth:**
- New strategic layer for faction warfare
- Resource logistics become meaningful gameplay
- Command roles create leadership opportunities

**Player Engagement:**
- Large-scale operations require sustained coordination
- Multiple roles support diverse playstyles
- Long-term campaigns create server narratives

**Educational Value:**
- Players learn actual military logistics
- Real-world tactical concepts applied
- Historical military procedures preserved

---

## Appendices

### Appendix A: Priority Manual List

**Critical Priority (Weeks 1-4):**
1. FM 4-0: Sustainment Operations
2. FM 10-27: General Supply in Theaters of Operations
3. ATP 4-42: Movement Control
4. FM 4-30: Ordnance Operations

**High Priority (Weeks 5-7):**
5. FM 3-90: Offense and Defense
6. FM 3-21: Infantry Battalion Operations
7. FM 3-0: Operations

**Medium Priority (Weeks 8-10):**
8. FM 3-34: Engineer Operations
9. TC 3-34.40: General Engineering
10. FM 5-103: Survivability

**Lower Priority (Weeks 11-12):**
11. FM 6-0: Mission Command
12. ATP 6-0.5: Command Post Organization
13. FM 2-0: Intelligence

### Appendix B: Glossary of Military Terms

**AO:** Area of Operations  
**COA:** Course of Action  
**FRAGO:** Fragmentary Order  
**IPB:** Intelligence Preparation of the Battlefield  
**LOC:** Line of Communication  
**MDMP:** Military Decision-Making Process  
**NAI:** Named Area of Interest  
**OPORD:** Operation Order  
**PIR:** Priority Intelligence Requirement  
**SOP:** Standard Operating Procedure

### Appendix C: Sample Extraction Output

```json
{
  "mechanic_id": "logistics_001",
  "name": "Supply Requisition System",
  "category": "logistics",
  "tier": 4,
  "source": {
    "document": "FM 4-0",
    "page": "3-12",
    "section": "Supply Operations"
  },
  "description": "Players can requisition supplies from supply points using a priority-based system",
  "requirements": {
    "player_rank": "squad_leader",
    "authorization": "company_commander_approval",
    "supply_point_access": true
  },
  "mechanics": {
    "request_format": {
      "supply_class": "string",
      "item_name": "string",
      "quantity": "integer",
      "justification": "string",
      "priority": "routine|priority|immediate|flash"
    },
    "processing_time": {
      "routine": "2-7 days",
      "priority": "1-2 days",
      "immediate": "12-24 hours",
      "flash": "6-12 hours"
    },
    "approval_process": {
      "automatic": ["routine < 1000 units"],
      "commander_review": ["priority > 1000 units", "all immediate/flash"]
    }
  },
  "game_integration": {
    "ui_element": "requisition_terminal",
    "notification_system": "request_status_updates",
    "transportation_trigger": "automatic_convoy_scheduling"
  }
}
```

### Appendix D: Cross-Reference Table

| Military System | BlueMarble Mechanic | Integration Point |
|----------------|---------------------|-------------------|
| Supply Classes | Resource Categories | Inventory System |
| Unit Organization | Group Structure | Party/Guild System |
| Tactical Formations | Combat Positioning | Combat System |
| Fortifications | Building System | Construction Mechanics |
| Command Hierarchy | Leadership Roles | Guild Ranks |
| Intelligence Collection | Scouting System | Exploration Mechanics |
| Logistics Chain | Trade Routes | Economy System |

### Appendix E: Implementation Recommendations

**Phase 1: Logistics Foundation (Month 1-2)**
1. Implement supply classification system
2. Create requisition and distribution mechanics
3. Build transportation network framework
4. Develop depot management UI

**Phase 2: Tactical Systems (Month 3-4)**
5. Implement unit composition guidelines
6. Create tactical coordination mechanics
7. Build command hierarchy system
8. Develop battlefield communication

**Phase 3: Engineering and Fortification (Month 5-6)**
9. Implement fortification construction
10. Create obstacle placement systems
11. Build siege equipment mechanics
12. Develop defensive structure progression

**Phase 4: Command and Intelligence (Month 7-8)**
13. Implement mission planning system
14. Create intelligence gathering mechanics
15. Build command post functionality
16. Develop strategic decision-making tools

**Phase 5: Integration and Balance (Month 9-10)**
17. Integrate with existing systems
18. Balance resource costs and timings
19. Playtest large-scale operations
20. Refine based on feedback

---

**Document Status:** Ready for Implementation  
**Next Steps:** Begin Phase 1 document downloads and setup extraction environment  
**Estimated Completion:** 12 weeks from start date  
**Review Schedule:** Weekly progress reviews, bi-weekly deliverable assessments

---

**References:**
- U.S. Army Publishing Directorate: https://armypubs.army.mil/
- FM 4-0: Sustainment Operations (2019)
- FM 3-90: Offense and Defense (2017)
- FM 3-34: Engineer Operations (2014)
- ATP 6-0.5: Command Post Organization and Operations (2017)

**Document Version:** 1.0  
**Last Updated:** 2025-01-15  
**Next Review:** After Week 4 (Logistics extraction complete)
