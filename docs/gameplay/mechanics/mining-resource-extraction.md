# Mining and Resource Extraction System

**Version:** 1.0  
**Date:** 2025-01-06  
**Status:** Design Specification

## Overview

The Mining and Resource Extraction System (Volná těžba / Free Mining) provides players with comprehensive tools to discover, extract, and manage geological resources throughout the BlueMarble world. This system emphasizes realistic geological constraints, three-dimensional underground networks, and strategic resource management.

## Core Design Philosophy

### Geological Authenticity

All extraction mechanics are grounded in realistic geological processes:
- Resource distribution follows authentic geological formations
- Extraction difficulty varies by rock type and mineral hardness
- Environmental factors (water table, structural stability) affect operations
- Material quality varies based on geological conditions

### Player Freedom

Players have complete freedom in exploration and extraction:
- No arbitrary extraction zones or restricted areas (beyond property rights)
- Discovery-based resource location
- Player-designed mining networks
- Strategic choice in extraction methods

## System Components

### 1. Resource Discovery

#### Geological Survey

Players can conduct surveys to locate resource deposits:

```csharp
public class GeologicalSurvey
{
    public SurveyType Type { get; set; }  // Surface, Subsurface, Deep
    public Coordinate3D Location { get; set; }
    public float Radius { get; set; }
    public float Accuracy { get; set; }  // Skill-dependent
    
    public SurveyResult Conduct(PlayerSkills skills, SurveyEquipment equipment)
    {
        var deposits = ScanForDeposits(Location, Radius);
        var accuracy = CalculateAccuracy(skills.Geology, equipment.Quality);
        
        return new SurveyResult
        {
            DetectedDeposits = FilterByAccuracy(deposits, accuracy),
            DepthEstimates = CalculateDepthEstimates(deposits, accuracy),
            QualityIndicators = EstimateQuality(deposits, accuracy),
            GeologicalConditions = AssessConditions(Location)
        };
    }
}
```

#### Discovery Methods

- **Visual Prospecting**: Surface indicators of subsurface deposits
- **Geophysical Survey**: Advanced equipment reveals deeper deposits
- **Test Drilling**: Sampling to confirm deposit quality
- **Geological Mapping**: Systematic region analysis

### 2. Three-Dimensional Mining Networks

#### Network Architecture

```csharp
public class MiningNetwork
{
    public Graph3D TunnelSystem { get; set; }
    public List<Shaft> VerticalAccess { get; set; }
    public List<ExtractionNode> ActiveMines { get; set; }
    public InfrastructureSystem Infrastructure { get; set; }
    
    public MiningPlan PlanExpansion(Coordinate3D target, GeologicalData geology)
    {
        // Calculate optimal path through geology
        var path = PathfindThroughGeology(CurrentExtent, target, geology);
        
        // Determine structural requirements
        var supports = CalculateStructuralSupport(path, geology.RockStrength);
        
        // Plan life support systems
        var ventilation = DesignVentilationSystem(path, CurrentNetwork);
        var drainage = DesignDrainageSystem(path, geology.WaterTable);
        
        return new MiningPlan
        {
            TunnelRoute = path,
            SupportStructures = supports,
            VentilationSystem = ventilation,
            DrainageSystem = drainage,
            EstimatedCost = CalculateCost(path, supports, ventilation, drainage),
            EstimatedTime = CalculateConstructionTime(path, geology)
        };
    }
}
```

#### Tunnel Construction

Players construct tunnels following realistic engineering:

**Tunnel Types**:
- **Access Tunnels**: Main horizontal passages (2.5m x 2.5m minimum)
- **Transport Tunnels**: Wider passages for cart systems (3m x 3m)
- **Extraction Drifts**: Narrow passages to ore bodies (2m x 2m)
- **Ventilation Shafts**: Vertical air circulation (1m x 1m minimum)

**Support Requirements**:

```csharp
public class TunnelSupport
{
    public static SupportRequirement Calculate(RockType rock, float depth, float span)
    {
        // Calculate rock load
        float rockLoad = CalculateRockLoad(rock.Density, depth, span);
        
        // Determine support type based on rock quality
        if (rock.CompressiveStrength > 100 && rock.IsStable)
        {
            return new SupportRequirement
            {
                Type = SupportType.Unsupported,
                Spacing = 0,
                Materials = null
            };
        }
        else if (rock.CompressiveStrength > 50)
        {
            return new SupportRequirement
            {
                Type = SupportType.RockBolts,
                Spacing = CalculateBoltSpacing(rock, rockLoad),
                Materials = new[] { MaterialType.SteelBolts, MaterialType.Resin }
            };
        }
        else
        {
            return new SupportRequirement
            {
                Type = SupportType.TimberSets,
                Spacing = 1.0f,  // Every meter
                Materials = new[] { MaterialType.Timber, MaterialType.LaggingPlanks }
            };
        }
    }
}
```

### 3. Resource Extraction

#### Extraction Methods

Different methods suit different geological conditions:

```csharp
public enum ExtractionMethod
{
    HandMining,        // Basic pickaxe work
    DrillAndBlast,     // Explosive excavation
    ContinuousMining,  // Mechanical cutting
    RoomAndPillar,     // Systematic extraction with support pillars
    LongwallMining     // Advanced continuous extraction
}

public class ExtractionProcess
{
    public ExtractionResult Extract(
        ExtractionNode node,
        ExtractionMethod method,
        ToolQuality tools,
        PlayerSkills skills)
    {
        var geology = GetLocalGeology(node.Location);
        var baseYield = CalculateBaseYield(geology, node.Extent);
        
        // Apply method efficiency
        var methodEfficiency = GetMethodEfficiency(method, geology);
        
        // Apply tool quality modifier
        var toolModifier = GetToolModifier(tools, geology.Hardness);
        
        // Apply skill modifier
        var skillModifier = GetSkillModifier(skills.Mining, method);
        
        // Calculate actual yield
        var actualYield = baseYield * methodEfficiency * toolModifier * skillModifier;
        
        // Determine quality
        var quality = DetermineQuality(geology.Purity, skills.Mining);
        
        // Calculate byproducts
        var byproducts = CalculateByproducts(geology, actualYield);
        
        // Environmental impact
        var impact = CalculateEnvironmentalImpact(method, actualYield);
        
        return new ExtractionResult
        {
            PrimaryMaterial = actualYield,
            MaterialQuality = quality,
            Byproducts = byproducts,
            EnvironmentalImpact = impact,
            ToolDegradation = CalculateToolWear(tools, geology.Hardness)
        };
    }
}
```

#### Yield Calculations

Material yield depends on multiple factors:

**Formula**:
```
Yield = BaseYield × MethodEfficiency × ToolQuality × SkillModifier × QualityFactor

Where:
- BaseYield: Geological deposit quantity (kg or m³)
- MethodEfficiency: 0.4-0.95 based on method and geology
- ToolQuality: 0.5-1.2 based on tool grade
- SkillModifier: 0.7-1.3 based on mining skill level
- QualityFactor: 0.8-1.0 based on deposit purity
```

### 4. Infrastructure Systems

#### Ventilation System

```csharp
public class VentilationSystem
{
    public List<AirShaft> Shafts { get; set; }
    public List<VentilationFan> Fans { get; set; }
    public float AirFlowRate { get; set; }  // m³/s
    
    public bool IsAdequate(MiningNetwork network)
    {
        // Calculate required airflow
        var requiredFlow = CalculateRequiredAirflow(
            network.ActiveWorkers,
            network.TotalVolume,
            network.DustGeneration
        );
        
        // Calculate actual airflow
        var actualFlow = CalculateActualAirflow(Shafts, Fans, network.Geometry);
        
        return actualFlow >= requiredFlow;
    }
    
    private float CalculateRequiredAirflow(int workers, float volume, float dust)
    {
        // Minimum: 6 m³/min per worker
        float workerRequirement = workers * 6.0f;
        
        // Volume requirement: Complete air change every 30 minutes
        float volumeRequirement = volume / 30.0f;
        
        // Dust dilution requirement
        float dustRequirement = dust * 100.0f;
        
        return Math.Max(workerRequirement, Math.Max(volumeRequirement, dustRequirement));
    }
}
```

#### Drainage System

```csharp
public class DrainageSystem
{
    public List<Sump> Sumps { get; set; }
    public List<Pump> Pumps { get; set; }
    public List<DrainageChannel> Channels { get; set; }
    
    public bool CanManageWater(MiningNetwork network, GeologicalData geology)
    {
        // Calculate water inflow
        var waterInflow = CalculateWaterInflow(network, geology);
        
        // Calculate pumping capacity
        var pumpCapacity = Pumps.Sum(p => p.FlowRate);
        
        // Factor in channel efficiency
        var channelEfficiency = CalculateChannelEfficiency(Channels);
        
        return (pumpCapacity * channelEfficiency) >= waterInflow;
    }
    
    private float CalculateWaterInflow(MiningNetwork network, GeologicalData geology)
    {
        float inflow = 0;
        
        foreach (var tunnel in network.TunnelSystem.Edges)
        {
            var depth = tunnel.AverageDepth;
            var waterTable = geology.GetWaterTable(tunnel.Location);
            
            if (depth > waterTable)
            {
                // Below water table - calculate seepage
                var permeability = geology.GetPermeability(tunnel.Location);
                var pressure = (depth - waterTable) * 9.81f;  // kPa
                inflow += CalculateSeepage(tunnel.SurfaceArea, permeability, pressure);
            }
        }
        
        return inflow;
    }
}
```

### 5. Safety Systems

#### Hazard Monitoring

```csharp
public class SafetyMonitoring
{
    public List<GasDetector> GasDetectors { get; set; }
    public List<StructuralMonitor> StructuralMonitors { get; set; }
    public AlertSystem Alerts { get; set; }
    
    public void MonitorConditions(MiningNetwork network)
    {
        // Check gas levels
        foreach (var detector in GasDetectors)
        {
            var gasLevel = detector.MeasureGasConcentration();
            
            if (gasLevel.Methane > 0.5f)  // 0.5% threshold
            {
                Alerts.TriggerEvacuation(detector.Location, HazardType.ExplosiveGas);
            }
            
            if (gasLevel.CarbonMonoxide > 50)  // 50 ppm threshold
            {
                Alerts.TriggerAlert(detector.Location, HazardType.ToxicGas);
            }
        }
        
        // Check structural stability
        foreach (var monitor in StructuralMonitors)
        {
            var displacement = monitor.MeasureRockDisplacement();
            
            if (displacement > monitor.CriticalThreshold)
            {
                Alerts.TriggerAlert(monitor.Location, HazardType.StructuralFailure);
            }
        }
    }
}
```

## Resource Types

### Metallic Ores

- **Iron Ore**: Common, widespread deposits
- **Copper Ore**: Moderate rarity, higher value
- **Gold Ore**: Rare, highest value
- **Silver Ore**: Rare, precious metal
- **Aluminum Ore**: Bauxite deposits
- **Tin Ore**: Alloy component
- **Lead Ore**: Industrial applications

### Non-Metallic Minerals

- **Coal**: Energy source, widespread
- **Salt**: Chemical and food use
- **Gypsum**: Construction material
- **Limestone**: Cement and flux
- **Marble**: Decorative stone
- **Granite**: Construction stone
- **Quartz**: Industrial applications

### Precious Stones

- **Diamond**: Extremely rare, highest value
- **Ruby**: Rare gemstone
- **Sapphire**: Rare gemstone
- **Emerald**: Rare gemstone
- **Topaz**: Semi-precious
- **Amethyst**: Semi-precious

## Progression Systems

### Mining Skill

```csharp
public class MiningSkill
{
    public float GeneralMining { get; set; }  // 0-100
    public Dictionary<MaterialType, float> MaterialSpecialization { get; set; }
    
    public void GainExperience(ExtractionResult extraction)
    {
        // General mining experience
        float baseXP = CalculateBaseXP(extraction.Difficulty);
        GeneralMining += baseXP * 0.3f;
        
        // Material-specific experience
        var material = extraction.PrimaryMaterial.Type;
        if (!MaterialSpecialization.ContainsKey(material))
            MaterialSpecialization[material] = 0;
            
        MaterialSpecialization[material] += baseXP * 0.7f;
        
        // Apply diminishing returns
        ApplyDiminishingReturns();
    }
    
    public float GetEffectiveSkill(MaterialType material)
    {
        float general = GeneralMining * 0.3f;
        float specialized = MaterialSpecialization.GetValueOrDefault(material, 0) * 0.7f;
        return general + specialized;
    }
}
```

### Equipment Progression

**Tool Tiers**:

1. **Basic Tools**: Wooden handles, iron heads (efficiency: 0.5)
2. **Standard Tools**: Steel construction (efficiency: 0.7)
3. **Quality Tools**: Hardened steel, good balance (efficiency: 0.9)
4. **Master Tools**: Premium alloys, ergonomic (efficiency: 1.1)
5. **Legendary Tools**: Exceptional craftsmanship (efficiency: 1.2)

## Economic Integration

### Resource Valuation

```csharp
public class ResourceEconomics
{
    public float CalculateMarketValue(Material material, MarketConditions market)
    {
        // Base value from material type
        float baseValue = material.Type.BaseValue;
        
        // Quality multiplier
        float qualityMod = GetQualityMultiplier(material.Quality);
        
        // Supply and demand
        float supplyDemandMod = CalculateSupplyDemand(material.Type, market);
        
        // Scarcity modifier
        float scarcityMod = CalculateScarcity(material.Type, market.Region);
        
        return baseValue * qualityMod * supplyDemandMod * scarcityMod;
    }
}
```

### Transportation Costs

```csharp
public class TransportationCost
{
    public float Calculate(Material material, Route route, TransportMethod method)
    {
        // Weight-based cost
        float weightCost = material.Weight * route.Distance * method.CostPerKgKm;
        
        // Terrain difficulty modifier
        float terrainMod = route.TerrainDifficulty;
        
        // Infrastructure quality modifier
        float infraMod = route.InfrastructureQuality;
        
        return weightCost * terrainMod / infraMod;
    }
}
```

## Environmental Systems

### Impact Assessment

```csharp
public class EnvironmentalImpact
{
    public float SurfaceDisturbance { get; set; }  // Area affected (m²)
    public float WaterContamination { get; set; }  // Contamination level
    public float AirPollution { get; set; }  // Dust and emissions
    public float NoiseLevel { get; set; }  // Decibels
    
    public void CalculateImpact(ExtractionMethod method, float volume)
    {
        switch (method)
        {
            case ExtractionMethod.HandMining:
                SurfaceDisturbance = 0;  // Underground only
                WaterContamination = 0.1f;
                AirPollution = 0.2f;
                NoiseLevel = 50f;
                break;
                
            case ExtractionMethod.DrillAndBlast:
                SurfaceDisturbance = 0;
                WaterContamination = 0.3f;
                AirPollution = 0.8f;
                NoiseLevel = 120f;
                break;
                
            // Additional methods...
        }
        
        // Scale by volume
        SurfaceDisturbance *= volume;
        WaterContamination *= volume;
        AirPollution *= volume;
    }
}
```

### Restoration Requirements

Mining operations may require environmental restoration:

- **Shaft Sealing**: Proper closure of abandoned shafts
- **Surface Rehabilitation**: Restoring disturbed surface areas
- **Water Treatment**: Treating contaminated mine water
- **Waste Management**: Proper disposal of mining waste

## Testing Requirements

### Unit Tests

1. **Survey Accuracy**: Verify detection ranges and accuracy calculations
2. **Yield Calculations**: Test all modifier interactions
3. **Support Requirements**: Validate structural calculations
4. **Infrastructure Capacity**: Test ventilation and drainage adequacy

### Integration Tests

1. **Network Construction**: Full mining network creation workflow
2. **Extraction Process**: Complete extraction from survey to yield
3. **Safety Systems**: Hazard detection and response
4. **Economic Calculations**: Market value and transportation costs

### Balance Tests

1. **Resource Distribution**: Verify appropriate scarcity levels
2. **Extraction Rates**: Ensure reasonable progression pace
3. **Economic Viability**: Validate profitability ranges
4. **Environmental Balance**: Test impact and restoration costs

## Related Documentation

- [Building and Construction Mechanics](./building-construction.md)
- [Terraforming Systems](./terraforming.md)
- [Trade Systems](./trade-system.md)
- [Protection Systems](./anti-exploitation.md)
- [Economy Systems](../../systems/economy-systems.md)
- [Game Mechanics Design](../../GAME_MECHANICS_DESIGN.md)

## Implementation Notes

### Performance Considerations

- Use spatial indexing (octree) for efficient resource location
- Cache geological data for frequently accessed regions
- Optimize pathfinding for tunnel planning
- Stream underground network data as needed

### Scalability

- Support thousands of concurrent mining operations
- Handle large underground networks (millions of cubic meters)
- Efficient multi-player coordination in shared mines
- Balance server load for geological calculations

### Player Experience

- Clear visual feedback for survey results
- Intuitive tunnel planning interface
- Warning systems for hazards
- Tutorial progression for skill development
