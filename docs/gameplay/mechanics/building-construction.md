# Building and Construction Mechanics

**Version:** 1.0  
**Date:** 2025-01-06  
**Status:** Design Specification

## Overview

The Building and Construction System (Stavba) provides comprehensive mechanics for players to design, construct, and maintain structures within the BlueMarble world. This system emphasizes geological suitability assessment, realistic material requirements, and authentic construction processes.

## Core Design Philosophy

### Geological Foundation

All construction is constrained by realistic geological factors:
- Foundation stability depends on subsurface conditions
- Soil bearing capacity limits structure size and weight
- Water table affects foundation depth and type
- Slope stability influences hillside construction

### Material Authenticity

Construction requires appropriate materials:
- Material properties affect structural integrity
- Local material availability influences building costs
- Material quality impacts building performance
- Processing requirements add production chains

### Progressive Complexity

Building difficulty scales with structure complexity:
- Simple structures require basic materials and skills
- Advanced buildings need specialized knowledge
- Infrastructure systems add complexity
- Multi-story buildings require engineering expertise

## System Components

### 1. Site Assessment

#### Geological Suitability Analysis

```csharp
public class SiteSuitability
{
    public Coordinate3D Location { get; set; }
    public BuildingType ProposedBuilding { get; set; }
    
    public SuitabilityReport Assess(GeologicalData geology)
    {
        var report = new SuitabilityReport();
        
        // Check foundation conditions
        report.FoundationQuality = AssessFoundation(geology);
        
        // Check bearing capacity
        report.BearingCapacity = CalculateBearingCapacity(geology);
        
        // Check slope stability
        report.SlopeStability = AssessSlopeStability(geology);
        
        // Check water table
        report.WaterTableDepth = geology.GetWaterTable(Location);
        
        // Check seismic risk
        report.SeismicRisk = AssessSeismicRisk(geology, Location);
        
        // Determine overall suitability
        report.OverallSuitability = CalculateOverallSuitability(report);
        
        return report;
    }
    
    private float CalculateBearingCapacity(GeologicalData geology)
    {
        var soilType = geology.GetSoilType(Location);
        var depth = 2.0f;  // Standard foundation depth
        
        // Terzaghi bearing capacity formula (simplified)
        float c = soilType.Cohesion;  // kPa
        float phi = soilType.FrictionAngle;  // degrees
        float gamma = soilType.UnitWeight;  // kN/m³
        
        // Bearing capacity factors (simplified)
        float Nc = CalculateNc(phi);
        float Nq = CalculateNq(phi);
        float Ngamma = CalculateNgamma(phi);
        
        // Ultimate bearing capacity
        float qult = c * Nc + gamma * depth * Nq + 0.5f * gamma * 1.0f * Ngamma;
        
        // Apply safety factor
        return qult / 3.0f;  // Factor of safety = 3
    }
}
```

#### Environmental Conditions

```csharp
public class EnvironmentalAssessment
{
    public ClimateData Climate { get; set; }
    public TerrainData Terrain { get; set; }
    
    public EnvironmentalFactors Assess(Coordinate3D location)
    {
        return new EnvironmentalFactors
        {
            // Wind exposure
            WindExposure = CalculateWindExposure(location, Terrain),
            
            // Solar exposure
            SolarExposure = CalculateSolarExposure(location, Terrain),
            
            // Flood risk
            FloodRisk = AssessFloodRisk(location, Terrain),
            
            // Temperature extremes
            TemperatureRange = Climate.GetTemperatureRange(location),
            
            // Precipitation
            AnnualRainfall = Climate.GetAnnualRainfall(location)
        };
    }
}
```

### 2. Building Design

#### Structure Types

```csharp
public enum BuildingType
{
    // Residential
    SimpleHut,           // Basic shelter
    Cottage,             // Small dwelling
    House,              // Standard residence
    Manor,              // Large residence
    Apartment,          // Multi-family dwelling
    
    // Industrial
    Workshop,           // Crafting facility
    Forge,             // Metalworking
    Mill,              // Processing facility
    Smelter,           // Ore processing
    Factory,           // Large-scale production
    
    // Mining
    MineHead,          // Mine entrance facility
    OreSeparation,     // Processing plant
    Concentration,     // Ore concentration
    Warehouse,         // Storage facility
    
    // Agricultural
    Barn,              // Livestock shelter
    Granary,           // Grain storage
    Greenhouse,        // Protected cultivation
    
    // Commercial
    Shop,              // Retail store
    Market,            // Trading post
    Tavern,            // Social gathering
    Inn,               // Accommodation
    Bank,              // Financial services
    
    // Infrastructure
    WaterTower,        // Water supply
    PowerStation,      // Energy generation
    Bridge,            // Transportation
    Dock,              // Water transport
    
    // Defensive
    Watchtower,        // Observation post
    Palisade,          // Defensive wall
    Fortress,          // Military stronghold
    Bunker             // Hardened shelter
}
```

#### Building Specifications

```csharp
public class BuildingSpecification
{
    public BuildingType Type { get; set; }
    public Dimensions Size { get; set; }
    public int Stories { get; set; }
    public FoundationType RequiredFoundation { get; set; }
    public MaterialRequirements Materials { get; set; }
    public SkillRequirements Skills { get; set; }
    public TimeRequirement ConstructionTime { get; set; }
    
    public static BuildingSpecification GetSpecification(BuildingType type)
    {
        switch (type)
        {
            case BuildingType.SimpleHut:
                return new BuildingSpecification
                {
                    Type = type,
                    Size = new Dimensions(4, 4, 3),  // 4x4m, 3m high
                    Stories = 1,
                    RequiredFoundation = FoundationType.SurfaceLevel,
                    Materials = new MaterialRequirements
                    {
                        Wood = 500,      // kg
                        Thatch = 200,    // kg
                        Rope = 50,       // m
                        Stone = 0
                    },
                    Skills = new SkillRequirements
                    {
                        Construction = 10,
                        Carpentry = 5
                    },
                    ConstructionTime = TimeSpan.FromHours(24)
                };
                
            case BuildingType.House:
                return new BuildingSpecification
                {
                    Type = type,
                    Size = new Dimensions(8, 8, 4),
                    Stories = 1,
                    RequiredFoundation = FoundationType.ShallowFooting,
                    Materials = new MaterialRequirements
                    {
                        Wood = 2000,
                        Stone = 3000,
                        Lime = 500,      // Mortar
                        Clay = 1000,     // Bricks or tiles
                        Glass = 50,
                        IronNails = 20
                    },
                    Skills = new SkillRequirements
                    {
                        Construction = 40,
                        Carpentry = 30,
                        Masonry = 30
                    },
                    ConstructionTime = TimeSpan.FromDays(30)
                };
                
            case BuildingType.Forge:
                return new BuildingSpecification
                {
                    Type = type,
                    Size = new Dimensions(10, 10, 5),
                    Stories = 1,
                    RequiredFoundation = FoundationType.ReinforcedFooting,
                    Materials = new MaterialRequirements
                    {
                        Stone = 5000,
                        Brick = 3000,    // Firebrick for furnace
                        Steel = 500,     // Structural and equipment
                        Wood = 1000,     // Roof structure
                        Clay = 2000      // Chimney and furnace lining
                    },
                    Skills = new SkillRequirements
                    {
                        Construction = 60,
                        Masonry = 50,
                        Engineering = 40
                    },
                    ConstructionTime = TimeSpan.FromDays(60)
                };
                
            // Additional building types...
            default:
                throw new ArgumentException($"Unknown building type: {type}");
        }
    }
}
```

### 3. Foundation Systems

#### Foundation Types

```csharp
public enum FoundationType
{
    SurfaceLevel,      // No excavation, suitable for light structures
    ShallowFooting,    // 0.5-1.5m depth, standard buildings
    DeepFooting,       // 1.5-3m depth, heavy buildings
    PileFoundation,    // Deep piles for poor soil or heavy loads
    RaftFoundation,    // Continuous slab for uniform load distribution
    RockAnchored       // Anchored into bedrock for extreme stability
}

public class Foundation
{
    public FoundationType Type { get; set; }
    public float Depth { get; set; }
    public Polygon Footprint { get; set; }
    public List<FoundationElement> Elements { get; set; }
    
    public static Foundation Design(
        BuildingSpecification building,
        SiteSuitability site,
        GeologicalData geology)
    {
        // Determine required foundation type
        var type = DetermineFoundationType(building, site, geology);
        
        // Calculate depth
        var depth = CalculateFoundationDepth(type, site.WaterTableDepth, geology);
        
        // Design foundation elements
        var elements = DesignFoundationElements(type, building, site, geology);
        
        return new Foundation
        {
            Type = type,
            Depth = depth,
            Footprint = CalculateFootprint(building.Size),
            Elements = elements
        };
    }
    
    private static float CalculateFoundationDepth(
        FoundationType type,
        float waterTable,
        GeologicalData geology)
    {
        float minDepth = 0;
        
        switch (type)
        {
            case FoundationType.SurfaceLevel:
                minDepth = 0.1f;
                break;
            case FoundationType.ShallowFooting:
                minDepth = 1.0f;
                break;
            case FoundationType.DeepFooting:
                minDepth = 2.0f;
                break;
            case FoundationType.PileFoundation:
                minDepth = 5.0f;
                break;
        }
        
        // Ensure foundation is above water table (or use waterproofing)
        if (minDepth > waterTable - 0.5f)
        {
            // Need waterproofing or drainage
            return minDepth;
        }
        
        return minDepth;
    }
}
```

### 4. Construction Process

#### Build Phases

```csharp
public class ConstructionProject
{
    public BuildingSpecification Specification { get; set; }
    public Foundation Foundation { get; set; }
    public List<ConstructionPhase> Phases { get; set; }
    public float Progress { get; set; }  // 0.0 - 1.0
    public ConstructionState State { get; set; }
    
    public void InitializePhases()
    {
        Phases = new List<ConstructionPhase>
        {
            // Phase 1: Site Preparation
            new ConstructionPhase
            {
                Name = "Site Preparation",
                Tasks = new List<ConstructionTask>
                {
                    new ConstructionTask("Clear vegetation", TimeSpan.FromHours(4)),
                    new ConstructionTask("Level ground", TimeSpan.FromHours(8)),
                    new ConstructionTask("Mark foundation", TimeSpan.FromHours(2))
                },
                RequiredMaterials = new MaterialSet { /* Stakes, string */ }
            },
            
            // Phase 2: Foundation
            new ConstructionPhase
            {
                Name = "Foundation Construction",
                Tasks = new List<ConstructionTask>
                {
                    new ConstructionTask("Excavate foundation", TimeSpan.FromHours(16)),
                    new ConstructionTask("Install drainage", TimeSpan.FromHours(4)),
                    new ConstructionTask("Pour footings", TimeSpan.FromHours(8)),
                    new ConstructionTask("Cure concrete", TimeSpan.FromDays(3))
                },
                RequiredMaterials = Foundation.Materials
            },
            
            // Phase 3: Structural Frame
            new ConstructionPhase
            {
                Name = "Structural Frame",
                Tasks = new List<ConstructionTask>
                {
                    new ConstructionTask("Erect posts", TimeSpan.FromHours(12)),
                    new ConstructionTask("Install beams", TimeSpan.FromHours(16)),
                    new ConstructionTask("Build walls", TimeSpan.FromHours(40)),
                    new ConstructionTask("Install floor joists", TimeSpan.FromHours(12))
                },
                RequiredMaterials = Specification.Materials.Structural
            },
            
            // Phase 4: Roof
            new ConstructionPhase
            {
                Name = "Roof Construction",
                Tasks = new List<ConstructionTask>
                {
                    new ConstructionTask("Install rafters", TimeSpan.FromHours(16)),
                    new ConstructionTask("Lay roofing", TimeSpan.FromHours(24)),
                    new ConstructionTask("Install gutters", TimeSpan.FromHours(4))
                },
                RequiredMaterials = Specification.Materials.Roofing
            },
            
            // Phase 5: Finishing
            new ConstructionPhase
            {
                Name = "Finishing Work",
                Tasks = new List<ConstructionTask>
                {
                    new ConstructionTask("Install windows", TimeSpan.FromHours(8)),
                    new ConstructionTask("Install doors", TimeSpan.FromHours(4)),
                    new ConstructionTask("Interior walls", TimeSpan.FromHours(20)),
                    new ConstructionTask("Flooring", TimeSpan.FromHours(16)),
                    new ConstructionTask("Painting", TimeSpan.FromHours(12))
                },
                RequiredMaterials = Specification.Materials.Finishing
            }
        };
    }
    
    public void ProgressConstruction(float workerHours, WorkerSkills skills)
    {
        var currentPhase = GetCurrentPhase();
        
        if (currentPhase == null)
        {
            State = ConstructionState.Complete;
            return;
        }
        
        // Apply skill efficiency
        float efficiency = CalculateWorkEfficiency(skills, currentPhase);
        float effectiveHours = workerHours * efficiency;
        
        // Progress current task
        var currentTask = currentPhase.GetCurrentTask();
        currentTask.Progress += effectiveHours / currentTask.Duration.TotalHours;
        
        if (currentTask.Progress >= 1.0f)
        {
            currentTask.Complete = true;
            
            // Check if phase is complete
            if (currentPhase.IsComplete())
            {
                currentPhase.Complete = true;
            }
        }
        
        // Update overall progress
        UpdateOverallProgress();
    }
}
```

#### Material Delivery

```csharp
public class MaterialDelivery
{
    public MaterialRequirements Required { get; set; }
    public MaterialInventory OnSite { get; set; }
    public List<Delivery> PendingDeliveries { get; set; }
    
    public bool HasSufficientMaterials(ConstructionPhase phase)
    {
        foreach (var requirement in phase.RequiredMaterials)
        {
            if (OnSite.GetQuantity(requirement.Material) < requirement.Quantity)
                return false;
        }
        return true;
    }
    
    public void ConsumePhase(ConstructionPhase phase)
    {
        foreach (var requirement in phase.RequiredMaterials)
        {
            OnSite.Remove(requirement.Material, requirement.Quantity);
        }
    }
}
```

### 5. Building Quality

#### Quality Factors

Building quality affects performance and durability:

```csharp
public class BuildingQuality
{
    public float StructuralIntegrity { get; set; }  // 0.0 - 1.0
    public float WeatherResistance { get; set; }
    public float Insulation { get; set; }
    public float Aesthetics { get; set; }
    public float Functionality { get; set; }
    
    public static BuildingQuality Calculate(
        MaterialSet materials,
        WorkerSkills skills,
        ConstructionMethods methods)
    {
        return new BuildingQuality
        {
            StructuralIntegrity = CalculateStructural(materials, skills),
            WeatherResistance = CalculateWeather(materials, methods),
            Insulation = CalculateInsulation(materials, methods),
            Aesthetics = CalculateAesthetics(materials, skills),
            Functionality = CalculateFunctionality(methods, skills)
        };
    }
    
    private static float CalculateStructural(MaterialSet materials, WorkerSkills skills)
    {
        // Material quality contribution
        float materialQuality = materials.AverageQuality * 0.4f;
        
        // Skill contribution
        float skillQuality = (
            skills.Construction * 0.3f +
            skills.Engineering * 0.3f
        ) / 100f;
        
        // Structural soundness
        float structural = materials.StructuralSoundness * 0.3f;
        
        return materialQuality + skillQuality + structural;
    }
    
    public float GetOverallQuality()
    {
        return (
            StructuralIntegrity * 0.4f +
            WeatherResistance * 0.2f +
            Insulation * 0.15f +
            Aesthetics * 0.10f +
            Functionality * 0.15f
        );
    }
}
```

### 6. Building Maintenance

#### Degradation System

```csharp
public class BuildingCondition
{
    public float CurrentCondition { get; set; }  // 0.0 - 1.0
    public float DegradationRate { get; set; }   // Per day
    public List<Damage> DamageEvents { get; set; }
    
    public void UpdateCondition(float deltaTime, EnvironmentalFactors environment)
    {
        // Base degradation from age
        float ageDegradation = DegradationRate * deltaTime;
        
        // Environmental degradation
        float weatherDegradation = CalculateWeatherDegradation(environment);
        
        // Apply degradation
        CurrentCondition -= (ageDegradation + weatherDegradation) * deltaTime;
        
        // Clamp to valid range
        CurrentCondition = Math.Max(0, Math.Min(1, CurrentCondition));
    }
    
    private float CalculateWeatherDegradation(EnvironmentalFactors environment)
    {
        float degradation = 0;
        
        // Moisture damage
        if (environment.AnnualRainfall > 1000)  // mm/year
            degradation += 0.0001f;
            
        // Temperature cycling
        float tempRange = environment.TemperatureRange;
        degradation += tempRange * 0.00001f;
        
        // Wind damage
        degradation += environment.WindExposure * 0.00005f;
        
        return degradation;
    }
}
```

#### Repair System

```csharp
public class BuildingRepair
{
    public RepairType Type { get; set; }
    public MaterialRequirements Materials { get; set; }
    public TimeSpan Duration { get; set; }
    public float ConditionRestoration { get; set; }
    
    public static List<RepairType> GetRequiredRepairs(BuildingCondition condition)
    {
        var repairs = new List<RepairType>();
        
        if (condition.CurrentCondition < 0.3f)
        {
            repairs.Add(RepairType.MajorStructural);
            repairs.Add(RepairType.RoofReplacement);
            repairs.Add(RepairType.FoundationRepair);
        }
        else if (condition.CurrentCondition < 0.6f)
        {
            repairs.Add(RepairType.MinorStructural);
            repairs.Add(RepairType.RoofPatch);
            repairs.Add(RepairType.SurfaceRepair);
        }
        else if (condition.CurrentCondition < 0.9f)
        {
            repairs.Add(RepairType.Maintenance);
            repairs.Add(RepairType.Painting);
        }
        
        return repairs;
    }
}
```

### 7. Infrastructure Integration

#### Utility Connections

```csharp
public class UtilityConnections
{
    public WaterConnection Water { get; set; }
    public PowerConnection Power { get; set; }
    public SewerConnection Sewer { get; set; }
    public RoadAccess Road { get; set; }
    
    public bool IsFullyConnected()
    {
        return Water != null && 
               Power != null && 
               Sewer != null && 
               Road != null;
    }
    
    public float GetConnectionCost()
    {
        float cost = 0;
        
        if (Water == null) cost += CalculateWaterConnectionCost();
        if (Power == null) cost += CalculatePowerConnectionCost();
        if (Sewer == null) cost += CalculateSewerConnectionCost();
        if (Road == null) cost += CalculateRoadConnectionCost();
        
        return cost;
    }
}
```

## Economic Integration

### Construction Costs

```csharp
public class ConstructionEconomics
{
    public float CalculateTotalCost(
        BuildingSpecification building,
        MaterialPrices prices,
        LaborRates labor,
        float distance)
    {
        // Material costs
        float materialCost = CalculateMaterialCost(building.Materials, prices);
        
        // Transportation costs
        float transportCost = CalculateTransportCost(
            building.Materials,
            distance,
            prices.TransportRate
        );
        
        // Labor costs
        float laborCost = CalculateLaborCost(
            building.ConstructionTime,
            building.Skills,
            labor
        );
        
        // Equipment rental
        float equipmentCost = CalculateEquipmentCost(building.Type);
        
        // Overhead (10%)
        float overhead = (materialCost + laborCost + equipmentCost) * 0.1f;
        
        return materialCost + transportCost + laborCost + equipmentCost + overhead;
    }
}
```

### Property Value

```csharp
public class PropertyValuation
{
    public float Calculate(
        Building building,
        Location location,
        MarketConditions market)
    {
        // Base value from construction cost
        float baseValue = building.ConstructionCost;
        
        // Quality adjustment
        float qualityMod = building.Quality.GetOverallQuality();
        
        // Condition adjustment
        float conditionMod = building.Condition.CurrentCondition;
        
        // Location value
        float locationMod = CalculateLocationValue(location, market);
        
        // Infrastructure bonus
        float infraBonus = building.Utilities.IsFullyConnected() ? 1.2f : 1.0f;
        
        return baseValue * qualityMod * conditionMod * locationMod * infraBonus;
    }
}
```

## Player Progression

### Construction Skills

```csharp
public class ConstructionSkills
{
    public float GeneralConstruction { get; set; }
    public float Carpentry { get; set; }
    public float Masonry { get; set; }
    public float Engineering { get; set; }
    public float Architecture { get; set; }
    
    public void GainExperience(ConstructionProject project, float contribution)
    {
        // Base XP from project difficulty
        float baseXP = project.Specification.Difficulty * contribution;
        
        // Distribute XP based on project type
        GeneralConstruction += baseXP * 0.3f;
        
        if (project.Specification.RequiresWoodwork())
            Carpentry += baseXP * 0.4f;
            
        if (project.Specification.RequiresMasonry())
            Masonry += baseXP * 0.4f;
            
        if (project.Specification.RequiresEngineering())
            Engineering += baseXP * 0.3f;
    }
}
```

## Testing Requirements

### Unit Tests

1. **Bearing Capacity**: Validate foundation calculations
2. **Material Requirements**: Verify material quantity calculations
3. **Construction Time**: Test time estimation accuracy
4. **Quality Calculations**: Validate quality factor interactions

### Integration Tests

1. **Full Construction**: Complete building construction workflow
2. **Site Assessment**: Geological suitability evaluation
3. **Maintenance Cycle**: Degradation and repair over time
4. **Economic Calculations**: Cost and value calculations

### Balance Tests

1. **Construction Costs**: Verify economic viability
2. **Build Times**: Ensure reasonable progression pace
3. **Material Availability**: Test supply chain adequacy
4. **Quality Impact**: Validate quality benefits

## Related Documentation

- [Mining and Resource Extraction](./mining-resource-extraction.md)
- [Terraforming Systems](./terraforming.md)
- [Trade Systems](./trade-system.md)
- [Protection Systems](./anti-exploitation.md)
- [Economy Systems](../../systems/economy-systems.md)
- [Game Mechanics Design](../../GAME_MECHANICS_DESIGN.md)

## Implementation Notes

### Performance Considerations

- Cache geological data for frequent site assessments
- Optimize building rendering for large settlements
- Efficient collision detection for placement validation
- Stream building data as players approach

### Visual Feedback

- Clear building placement preview with suitability indicators
- Construction progress visualization
- Material requirement display
- Warning systems for unsuitable sites

### Multi-Player Coordination

- Collaborative construction projects
- Shared resource contribution
- Permission systems for building on others' land
- Guild construction capabilities
