# Settlement Control System Design

**Version:** 1.0  
**Date:** 2024-01-08  
**Author:** BlueMarble Systems Team

## Overview

The Settlement Control System defines how territories and settlements are controlled in BlueMarble.
A settlement is controlled by an entity (player, guild, dynasty, or alliance) that possesses sufficient
resources and influence - specifically political, economic, or military power. Control is dynamic and
can change hands when a more influential entity challenges the current controller.

This document describes the core mechanics, classes, and systems that implement influence-based settlement
control, diplomatic relationships, territorial disputes, and federative management.

## Core Concept

**Settlement Control Definition:**

> A settlement is controlled by someone who has sufficient resources (influence) to control it -
> through political, economic, or military means.

Control is not permanent or arbitrary; it's based on measurable influence that can grow, diminish,
or be contested.

## Design Influences from Successful Games

The Settlement Control System draws inspiration from proven mechanics in successful multiplayer games,
adapting their best practices to BlueMarble's geological and dynasty-focused gameplay:

### Travian - Cultural Expansion and Loyalty

**Adopted Mechanics:**

- **Cultural Influence Points**: Players accumulate influence over time through cultural development
- **Loyalty System**: Conquered settlements have loyalty ratings that affect productivity and control stability
- **Capital Designation**: One settlement serves as capital with enhanced bonuses
- **Distance Penalties**: Remote settlements are harder to defend and supply
- **Population-Based Production**: Settlement population directly affects resource generation

**BlueMarble Adaptation:**

```csharp
public class SettlementLoyalty
{
    public Guid SettlementId { get; set; }
    public float CurrentLoyalty { get; set; }  // 0-100
    public Guid OriginalFounderId { get; set; }
    public Guid CurrentControllerId { get; set; }
    
    // Loyalty affects control stability
    public float ControlStabilityModifier => CurrentLoyalty / 100f;
    
    // Loyalty changes over time
    public void UpdateLoyalty(float deltaTime)
    {
        // Loyalty trends toward controller's influence
        if (CurrentControllerId == OriginalFounderId)
        {
            // Original controller regains loyalty faster
            CurrentLoyalty = Math.Min(100f, CurrentLoyalty + 0.5f * deltaTime);
        }
        else
        {
            // New controller gains loyalty slowly
            CurrentLoyalty = Math.Min(100f, CurrentLoyalty + 0.1f * deltaTime);
        }
    }
    
    // Low loyalty increases vulnerability
    public float GetVulnerabilityMultiplier()
    {
        return 2.0f - (CurrentLoyalty / 100f);  // 1.0x at 100 loyalty, 2.0x at 0 loyalty
    }
}

public class CulturalInfluencePoints
{
    public Guid EntityId { get; set; }
    public float AvailablePoints { get; set; }
    public float PointGenerationRate { get; set; }
    
    // Culture points enable expansion
    public float CalculatePointGeneration(List<Settlement> controlledSettlements)
    {
        float baseRate = 10f;
        
        // More settlements generate more culture
        float settlementBonus = controlledSettlements.Count * 5f;
        
        // Cultural buildings boost generation
        float culturalBonus = controlledSettlements
            .Sum(s => s.Infrastructure
                .Where(b => b.Type == BuildingType.Cultural)
                .Sum(b => b.CultureBonus));
        
        return baseRate + settlementBonus + culturalBonus;
    }
    
    // Founding new settlements costs culture points
    public bool CanAffordSettlement(SettlementSize size)
    {
        float cost = GetSettlementCost(size);
        return AvailablePoints >= cost;
    }
    
    private float GetSettlementCost(SettlementSize size)
    {
        return size switch
        {
            SettlementSize.Outpost => 500f,
            SettlementSize.Village => 1000f,
            SettlementSize.Town => 2500f,
            SettlementSize.City => 5000f,
            SettlementSize.Metropolis => 10000f,
            _ => 500f
        };
    }
}
```

### Rust - Maintenance and Decay Systems

**Adopted Mechanics:**

- **Upkeep Costs**: Regular resource consumption to maintain structures
- **Decay Timers**: Unmaintained structures gradually deteriorate
- **Authorization Levels**: Tiered access control (owner, moderator, guest)
- **Tool Cupboard Radius**: Physical control radius from structures
- **Exponential Scaling**: Larger bases cost disproportionately more to maintain

**BlueMarble Adaptation:**

```csharp
public class SettlementUpkeep
{
    public Guid SettlementId { get; set; }
    public Dictionary<Resource, float> DailyUpkeepCosts { get; set; }
    public DateTime LastUpkeepPaid { get; set; }
    public float DecayAccumulator { get; set; }
    
    public void CalculateUpkeepCosts(Settlement settlement)
    {
        DailyUpkeepCosts = new Dictionary<Resource, float>();
        
        // Base cost scales with settlement size
        float sizeMultiplier = settlement.Size switch
        {
            SettlementSize.Outpost => 1.0f,
            SettlementSize.Village => 2.5f,
            SettlementSize.Town => 6.0f,
            SettlementSize.City => 15.0f,
            SettlementSize.Metropolis => 40.0f,
            _ => 1.0f
        };
        
        // Infrastructure increases upkeep
        float infrastructureCost = settlement.Infrastructure.Count * 10f;
        
        // Distance from capital increases costs
        float distancePenalty = CalculateDistancePenalty(settlement);
        
        float totalCost = (100f * sizeMultiplier + infrastructureCost) * distancePenalty;
        
        // Convert to resource requirements
        DailyUpkeepCosts[Resource.Food] = totalCost * 0.4f;
        DailyUpkeepCosts[Resource.Materials] = totalCost * 0.3f;
        DailyUpkeepCosts[Resource.Currency] = totalCost * 0.3f;
    }
    
    public void ProcessUpkeep(Dictionary<Resource, float> availableResources)
    {
        bool canAfford = true;
        
        foreach (var cost in DailyUpkeepCosts)
        {
            if (!availableResources.ContainsKey(cost.Key) || 
                availableResources[cost.Key] < cost.Value)
            {
                canAfford = false;
                break;
            }
        }
        
        if (canAfford)
        {
            // Deduct resources
            foreach (var cost in DailyUpkeepCosts)
            {
                availableResources[cost.Key] -= cost.Value;
            }
            
            LastUpkeepPaid = DateTime.UtcNow;
            DecayAccumulator = 0f;
        }
        else
        {
            // Start decay
            float daysSinceUpkeep = (DateTime.UtcNow - LastUpkeepPaid).Days;
            DecayAccumulator += daysSinceUpkeep;
        }
    }
    
    public float GetDecayLevel()
    {
        // 0 = no decay, 1.0 = completely decayed
        return Math.Min(1.0f, DecayAccumulator / 30f);  // Full decay after 30 days
    }
    
    private float CalculateDistancePenalty(Settlement settlement)
    {
        // Would integrate with actual distance calculations
        return 1.0f;  // Placeholder
    }
}

public class SettlementAuthorization
{
    public Guid SettlementId { get; set; }
    public Dictionary<Guid, AuthorizationLevel> Authorizations { get; set; }
    
    public enum AuthorizationLevel
    {
        Owner,      // Full control
        Governor,   // Manage, build, modify
        Builder,    // Can build and modify
        Resident,   // Can use facilities
        Visitor     // Limited access
    }
    
    public bool CanPerformAction(Guid entityId, SettlementAction action)
    {
        if (!Authorizations.TryGetValue(entityId, out var level))
            return false;
        
        return action switch
        {
            SettlementAction.TransferControl => level == AuthorizationLevel.Owner,
            SettlementAction.ChangeGovernance => level <= AuthorizationLevel.Governor,
            SettlementAction.BuildInfrastructure => level <= AuthorizationLevel.Builder,
            SettlementAction.UseFacilities => level <= AuthorizationLevel.Resident,
            SettlementAction.Visit => true,
            _ => false
        };
    }
}

public enum SettlementAction
{
    TransferControl,
    ChangeGovernance,
    BuildInfrastructure,
    UseFacilities,
    Visit
}
```

### Minecraft - Granular Permissions and Visual Boundaries

**Adopted Mechanics:**

- **Claim Blocks**: Limited resource for claiming territory
- **Subclaims**: Subdivide areas with different permissions
- **Permission Flags**: Fine-grained control (build, break, interact, access)
- **Visual Boundaries**: Clear indication of claimed territory
- **Trust System**: Assign permissions to specific players

**BlueMarble Adaptation:**

```csharp
public class TerritoryClaimSystem
{
    public Guid EntityId { get; set; }
    public float AvailableClaimBlocks { get; set; }
    public List<TerritorialClaim> Claims { get; set; }
    
    // Earn claim blocks through playtime and achievements
    public void EarnClaimBlocks(float amount)
    {
        AvailableClaimBlocks += amount;
    }
    
    public bool CanClaimTerritory(float areaSize)
    {
        return AvailableClaimBlocks >= areaSize;
    }
    
    public TerritorialClaim CreateClaim(Coordinate3D center, float radius)
    {
        float area = CalculateArea(radius);
        
        if (!CanClaimTerritory(area))
            return null;
        
        var claim = new TerritorialClaim
        {
            ClaimId = Guid.NewGuid(),
            OwnerId = EntityId,
            Center = center,
            Radius = radius,
            Area = area,
            Permissions = new TerritoryPermissions()
        };
        
        AvailableClaimBlocks -= area;
        Claims.Add(claim);
        
        return claim;
    }
    
    private float CalculateArea(float radius)
    {
        return (float)(Math.PI * radius * radius);
    }
}

public class TerritorialClaim
{
    public Guid ClaimId { get; set; }
    public Guid OwnerId { get; set; }
    public Coordinate3D Center { get; set; }
    public float Radius { get; set; }
    public float Area { get; set; }
    public TerritoryPermissions Permissions { get; set; }
    public List<SubClaim> SubClaims { get; set; }
    
    // Visual boundary for players
    public List<Coordinate3D> GetBoundaryPoints()
    {
        var points = new List<Coordinate3D>();
        int segments = 32;
        
        for (int i = 0; i < segments; i++)
        {
            float angle = (float)(2 * Math.PI * i / segments);
            points.Add(new Coordinate3D(
                Center.X + Radius * (float)Math.Cos(angle),
                Center.Y + Radius * (float)Math.Sin(angle),
                Center.Z
            ));
        }
        
        return points;
    }
}

public class TerritoryPermissions
{
    public Dictionary<Guid, PermissionFlags> EntityPermissions { get; set; }
    public PermissionFlags PublicPermissions { get; set; }
    
    public TerritoryPermissions()
    {
        EntityPermissions = new Dictionary<Guid, PermissionFlags>();
        PublicPermissions = PermissionFlags.None;
    }
    
    public void GrantPermission(Guid entityId, PermissionFlags flags)
    {
        EntityPermissions[entityId] = flags;
    }
    
    public bool HasPermission(Guid entityId, PermissionFlags requiredFlag)
    {
        if (EntityPermissions.TryGetValue(entityId, out var flags))
        {
            return flags.HasFlag(requiredFlag);
        }
        
        return PublicPermissions.HasFlag(requiredFlag);
    }
}

[Flags]
public enum PermissionFlags
{
    None = 0,
    Build = 1,
    Destroy = 2,
    Interact = 4,
    AccessStorage = 8,
    AccessFacilities = 16,
    Harvest = 32,
    All = Build | Destroy | Interact | AccessStorage | AccessFacilities | Harvest
}

public class SubClaim
{
    public Guid SubClaimId { get; set; }
    public Guid ParentClaimId { get; set; }
    public Coordinate3D Center { get; set; }
    public float Radius { get; set; }
    public TerritoryPermissions Permissions { get; set; }
    public string Name { get; set; }  // e.g., "Mining Zone", "Public Market"
}
```

### EVE Online - Sovereignty and Vulnerability Windows

**Adopted Mechanics:**

- **Sovereignty Structures**: Physical anchors for territorial control
- **Vulnerability Timers**: Defenders set when structures can be attacked
- **Influence Projection**: Control radiates from strategic structures
- **Strategic Value**: Different systems have different values
- **Contested State**: Multi-phase territorial conquest

**BlueMarble Adaptation:**

```csharp
public class SovereigntyStructure
{
    public Guid StructureId { get; set; }
    public Guid OwnerId { get; set; }
    public StructureType Type { get; set; }
    public Coordinate3D Location { get; set; }
    public float HealthPoints { get; set; }
    public float MaxHealthPoints { get; set; }
    public SovereigntyState State { get; set; }
    
    // Vulnerability window
    public TimeSpan VulnerabilityStart { get; set; }  // e.g., 18:00 local time
    public TimeSpan VulnerabilityDuration { get; set; }  // e.g., 3 hours
    
    // Control radius
    public float InfluenceRadius { get; set; }
    
    public bool IsVulnerable(DateTime currentTime)
    {
        var localTime = currentTime.TimeOfDay;
        var vulnerabilityEnd = VulnerabilityStart.Add(VulnerabilityDuration);
        
        if (vulnerabilityEnd.TotalHours > 24)
        {
            // Window crosses midnight
            return localTime >= VulnerabilityStart || 
                   localTime <= TimeSpan.FromHours(vulnerabilityEnd.TotalHours - 24);
        }
        
        return localTime >= VulnerabilityStart && localTime <= vulnerabilityEnd;
    }
    
    public float GetInfluenceStrength(Coordinate3D point)
    {
        float distance = CalculateDistance(Location, point);
        
        if (distance > InfluenceRadius)
            return 0f;
        
        // Influence falls off with distance
        return (1.0f - distance / InfluenceRadius) * (HealthPoints / MaxHealthPoints);
    }
    
    private float CalculateDistance(Coordinate3D a, Coordinate3D b)
    {
        return (float)Math.Sqrt(
            Math.Pow(a.X - b.X, 2) + 
            Math.Pow(a.Y - b.Y, 2) + 
            Math.Pow(a.Z - b.Z, 2));
    }
}

public enum StructureType
{
    Citadel,        // Major control structure
    Fortress,       // Defensive structure
    IndustryHub,    // Economic center
    Observatory     // Strategic monitoring
}

public enum SovereigntyState
{
    Stable,         // Normal state
    Reinforced,     // Under attack, waiting for vulnerability
    Contested,      // Active battle for control
    Captured        // Recently changed hands
}

public class ContestedTerritory
{
    public Guid TerritoryId { get; set; }
    public Guid AttackerId { get; set; }
    public Guid DefenderId { get; set; }
    public ConflictPhase CurrentPhase { get; set; }
    public DateTime PhaseStartTime { get; set; }
    public float AttackerProgress { get; set; }  // 0-100
    
    // Multi-phase conquest system
    public void AdvancePhase()
    {
        CurrentPhase = CurrentPhase switch
        {
            ConflictPhase.Initial => ConflictPhase.Entosis,
            ConflictPhase.Entosis => ConflictPhase.Reinforced,
            ConflictPhase.Reinforced => ConflictPhase.FinalBattle,
            ConflictPhase.FinalBattle => ConflictPhase.Captured,
            _ => ConflictPhase.Initial
        };
        
        PhaseStartTime = DateTime.UtcNow;
    }
    
    public TimeSpan GetPhaseTimeRemaining()
    {
        var phaseDuration = CurrentPhase switch
        {
            ConflictPhase.Initial => TimeSpan.FromHours(1),
            ConflictPhase.Entosis => TimeSpan.FromHours(6),
            ConflictPhase.Reinforced => TimeSpan.FromDays(1),
            ConflictPhase.FinalBattle => TimeSpan.FromHours(3),
            _ => TimeSpan.Zero
        };
        
        var elapsed = DateTime.UtcNow - PhaseStartTime;
        return phaseDuration - elapsed;
    }
}

public enum ConflictPhase
{
    Initial,        // Conflict declared
    Entosis,        // Initial attack phase
    Reinforced,     // Waiting for vulnerability window
    FinalBattle,    // Decisive battle
    Captured        // Territory changes hands
}

public class InfluenceMap
{
    private Dictionary<Coordinate3D, Dictionary<Guid, float>> _influenceGrid;
    
    public void CalculateInfluence(List<SovereigntyStructure> structures)
    {
        _influenceGrid = new Dictionary<Coordinate3D, Dictionary<Guid, float>>();
        
        // Calculate influence from each structure
        foreach (var structure in structures)
        {
            ProjectInfluence(structure);
        }
    }
    
    private void ProjectInfluence(SovereigntyStructure structure)
    {
        // Project influence in radius around structure
        int gridSize = (int)structure.InfluenceRadius;
        
        for (int x = -gridSize; x <= gridSize; x++)
        {
            for (int y = -gridSize; y <= gridSize; y++)
            {
                for (int z = -gridSize; z <= gridSize; z++)
                {
                    var point = new Coordinate3D(
                        structure.Location.X + x,
                        structure.Location.Y + y,
                        structure.Location.Z + z
                    );
                    
                    float influence = structure.GetInfluenceStrength(point);
                    
                    if (influence > 0)
                    {
                        if (!_influenceGrid.ContainsKey(point))
                            _influenceGrid[point] = new Dictionary<Guid, float>();
                        
                        _influenceGrid[point][structure.OwnerId] = 
                            Math.Max(_influenceGrid[point].GetValueOrDefault(structure.OwnerId, 0f), influence);
                    }
                }
            }
        }
    }
    
    public Guid? GetDominantController(Coordinate3D point)
    {
        if (!_influenceGrid.TryGetValue(point, out var influences))
            return null;
        
        return influences.OrderByDescending(kvp => kvp.Value).First().Key;
    }
    
    public bool IsContested(Coordinate3D point, float threshold = 0.2f)
    {
        if (!_influenceGrid.TryGetValue(point, out var influences))
            return false;
        
        if (influences.Count < 2)
            return false;
        
        var sorted = influences.OrderByDescending(kvp => kvp.Value).ToList();
        float topInfluence = sorted[0].Value;
        float secondInfluence = sorted[1].Value;
        
        // Contested if second place is within threshold of first
        return (topInfluence - secondInfluence) / topInfluence < threshold;
    }
}
```

### Integration Summary

These proven mechanics enhance the Settlement Control System with:

1. **Active Maintenance** (Rust): Regular upkeep prevents passive expansion
2. **Cultural Expansion** (Travian): Gradual influence growth enables strategic planning
3. **Loyalty Dynamics** (Travian): Conquered territories require ongoing management
4. **Permission Granularity** (Minecraft): Detailed control delegation
5. **Visual Clarity** (Minecraft): Players can see territory boundaries
6. **Strategic Timing** (EVE): Vulnerability windows add tactical depth
7. **Influence Projection** (EVE): Control radiates from key structures
8. **Contested States** (EVE): Clear phases during territorial conflicts

## System Architecture

### High-Level Components

```text
┌─────────────────────────────────────────────────────────────┐
│          Social Interaction System (Orchestrator)           │
└─────────────────────────────────────────────────────────────┘
                          │
          ┌───────────────┼───────────────┐
          │               │               │
          ▼               ▼               ▼
┌──────────────────┐ ┌────────────┐ ┌──────────────────┐
│ Settlement       │ │ Diplomacy  │ │ Influence        │
│ Manager          │ │ Manager    │ │ Profile System   │
└──────────────────┘ └────────────┘ └──────────────────┘
          │               │               │
          └───────────────┴───────────────┘
                          │
                          ▼
                  ┌──────────────┐
                  │  Settlement  │
                  │   Entities   │
                  └──────────────┘
```

## Core Classes and Components

### 1. InfluenceProfile Class

The `InfluenceProfile` class tracks the three types of influence an entity can possess:

```csharp
/// <summary>
/// Represents the influence profile of an entity, measuring their capacity
/// to control settlements through different means.
/// </summary>
public class InfluenceProfile
{
    public Guid EntityId { get; set; }
    public string EntityName { get; set; }
    public EntityType EntityType { get; set; }  // Player, Guild, Dynasty, Alliance
    
    // Core influence types
    public float PoliticalInfluence { get; private set; }
    public float EconomicInfluence { get; private set; }
    public float MilitaryInfluence { get; private set; }
    
    // Calculated total influence
    public float TotalInfluence => PoliticalInfluence + EconomicInfluence + MilitaryInfluence;
    
    // Influence components breakdown
    public Dictionary<InfluenceSource, float> PoliticalSources { get; set; }
    public Dictionary<InfluenceSource, float> EconomicSources { get; set; }
    public Dictionary<InfluenceSource, float> MilitarySources { get; set; }
    
    /// <summary>
    /// Calculate political influence based on various factors
    /// </summary>
    public void CalculatePoliticalInfluence()
    {
        PoliticalInfluence = 0f;
        
        // Diplomatic relationships
        PoliticalInfluence += PoliticalSources.GetValueOrDefault(
            InfluenceSource.DiplomaticTies, 0f);
        
        // Population support
        PoliticalInfluence += PoliticalSources.GetValueOrDefault(
            InfluenceSource.PopulationSupport, 0f);
        
        // Governance structures
        PoliticalInfluence += PoliticalSources.GetValueOrDefault(
            InfluenceSource.GovernanceInstitutions, 0f);
        
        // Cultural influence
        PoliticalInfluence += PoliticalSources.GetValueOrDefault(
            InfluenceSource.CulturalDominance, 0f);
        
        // Historical claims
        PoliticalInfluence += PoliticalSources.GetValueOrDefault(
            InfluenceSource.HistoricalClaims, 0f);
    }
    
    /// <summary>
    /// Calculate economic influence based on various factors
    /// </summary>
    public void CalculateEconomicInfluence()
    {
        EconomicInfluence = 0f;
        
        // Wealth and resources
        EconomicInfluence += EconomicSources.GetValueOrDefault(
            InfluenceSource.AccumulatedWealth, 0f);
        
        // Trade control
        EconomicInfluence += EconomicSources.GetValueOrDefault(
            InfluenceSource.TradeRouteControl, 0f);
        
        // Infrastructure ownership
        EconomicInfluence += EconomicSources.GetValueOrDefault(
            InfluenceSource.InfrastructureOwnership, 0f);
        
        // Resource production
        EconomicInfluence += EconomicSources.GetValueOrDefault(
            InfluenceSource.ResourceProduction, 0f);
        
        // Market dominance
        EconomicInfluence += EconomicSources.GetValueOrDefault(
            InfluenceSource.MarketShare, 0f);
    }
    
    /// <summary>
    /// Calculate military influence based on various factors
    /// </summary>
    public void CalculateMilitaryInfluence()
    {
        MilitaryInfluence = 0f;
        
        // Military forces
        MilitaryInfluence += MilitarySources.GetValueOrDefault(
            InfluenceSource.MilitaryStrength, 0f);
        
        // Defensive fortifications
        MilitaryInfluence += MilitarySources.GetValueOrDefault(
            InfluenceSource.Fortifications, 0f);
        
        // Strategic positioning
        MilitaryInfluence += MilitarySources.GetValueOrDefault(
            InfluenceSource.StrategicLocation, 0f);
        
        // Alliance support
        MilitaryInfluence += MilitarySources.GetValueOrDefault(
            InfluenceSource.AlliedMilitarySupport, 0f);
        
        // Supply lines
        MilitaryInfluence += MilitarySources.GetValueOrDefault(
            InfluenceSource.SupplyChainSecurity, 0f);
    }
    
    /// <summary>
    /// Update all influence calculations
    /// </summary>
    public void RecalculateInfluence()
    {
        CalculatePoliticalInfluence();
        CalculateEconomicInfluence();
        CalculateMilitaryInfluence();
    }
    
    /// <summary>
    /// Check if this profile has sufficient influence to control a settlement
    /// </summary>
    public bool CanControlSettlement(Settlement settlement)
    {
        return TotalInfluence >= settlement.RequiredInfluenceThreshold;
    }
    
    /// <summary>
    /// Compare influence with another entity
    /// </summary>
    public InfluenceComparison CompareWith(InfluenceProfile other)
    {
        return new InfluenceComparison
        {
            TotalInfluenceDelta = this.TotalInfluence - other.TotalInfluence,
            PoliticalDelta = this.PoliticalInfluence - other.PoliticalInfluence,
            EconomicDelta = this.EconomicInfluence - other.EconomicInfluence,
            MilitaryDelta = this.MilitaryInfluence - other.MilitaryInfluence,
            StrongerEntity = this.TotalInfluence > other.TotalInfluence ? this : other
        };
    }
}

/// <summary>
/// Sources of influence for tracking purposes
/// </summary>
public enum InfluenceSource
{
    // Political sources
    DiplomaticTies,
    PopulationSupport,
    GovernanceInstitutions,
    CulturalDominance,
    HistoricalClaims,
    
    // Economic sources
    AccumulatedWealth,
    TradeRouteControl,
    InfrastructureOwnership,
    ResourceProduction,
    MarketShare,
    
    // Military sources
    MilitaryStrength,
    Fortifications,
    StrategicLocation,
    AlliedMilitarySupport,
    SupplyChainSecurity
}

/// <summary>
/// Result of comparing two influence profiles
/// </summary>
public class InfluenceComparison
{
    public float TotalInfluenceDelta { get; set; }
    public float PoliticalDelta { get; set; }
    public float EconomicDelta { get; set; }
    public float MilitaryDelta { get; set; }
    public InfluenceProfile StrongerEntity { get; set; }
}

public enum EntityType
{
    Player,
    Guild,
    Dynasty,
    Alliance,
    NpcFaction
}
```

### 2. Settlement Class

The `Settlement` class represents a controlled territory with influence requirements:

```csharp
/// <summary>
/// Represents a settlement that can be controlled by entities with sufficient influence
/// </summary>
public class Settlement
{
    public Guid SettlementId { get; set; }
    public string Name { get; set; }
    public Coordinate3D Location { get; set; }
    public SettlementSize Size { get; set; }
    public SettlementType Type { get; set; }
    
    // Control mechanics
    public Guid? CurrentControllerId { get; set; }
    public InfluenceProfile CurrentControllerInfluence { get; private set; }
    public float RequiredInfluenceThreshold { get; set; }
    public DateTime LastControlChange { get; set; }
    public List<ControlHistory> ControlHistory { get; set; }
    
    // Settlement properties that affect control
    public int Population { get; set; }
    public float StrategicValue { get; set; }
    public List<Resource> AvailableResources { get; set; }
    public List<Building> Infrastructure { get; set; }
    public Dictionary<Guid, float> InfluenceChallenges { get; set; }  // Entity ID -> Challenge strength
    
    // Game-inspired enhancements
    public SettlementLoyalty Loyalty { get; set; }
    public SettlementUpkeep Upkeep { get; set; }
    public SettlementAuthorization Authorization { get; set; }
    public List<SovereigntyStructure> SovereigntyStructures { get; set; }
    public TerritorialClaim TerritorialClaim { get; set; }
    public ContestedTerritory ContestedStatus { get; set; }
    
    // Governance
    public GovernanceType GovernanceType { get; set; }
    public List<Policy> ActivePolicies { get; set; }
    public TaxationSystem TaxationSystem { get; set; }
    
    /// <summary>
    /// Calculate the required influence threshold based on settlement properties
    /// </summary>
    public float CalculateInfluenceThreshold()
    {
        float baseThreshold = 100f;
        
        // Size multiplier
        baseThreshold *= GetSizeMultiplier(Size);
        
        // Strategic value adds to threshold
        baseThreshold += StrategicValue * 10f;
        
        // Population support (more people = harder to control)
        baseThreshold += Population * 0.1f;
        
        // Resource richness increases required control
        float resourceValue = AvailableResources.Sum(r => r.Value);
        baseThreshold += resourceValue * 0.05f;
        
        // Infrastructure makes it easier to control (reduces threshold)
        float infrastructureBonus = Infrastructure.Count * 5f;
        baseThreshold = Math.Max(baseThreshold - infrastructureBonus, baseThreshold * 0.7f);
        
        // Loyalty affects control difficulty
        if (Loyalty != null)
        {
            baseThreshold *= Loyalty.GetVulnerabilityMultiplier();
        }
        
        // Decay increases vulnerability
        if (Upkeep != null)
        {
            float decayLevel = Upkeep.GetDecayLevel();
            baseThreshold *= (1.0f - (decayLevel * 0.5f));  // Up to 50% reduction at full decay
        }
        
        RequiredInfluenceThreshold = baseThreshold;
        return baseThreshold;
    }
    
    private float GetSizeMultiplier(SettlementSize size)
    {
        return size switch
        {
            SettlementSize.Outpost => 1.0f,
            SettlementSize.Village => 1.5f,
            SettlementSize.Town => 2.5f,
            SettlementSize.City => 4.0f,
            SettlementSize.Metropolis => 6.0f,
            _ => 1.0f
        };
    }
    
    /// <summary>
    /// Check if an entity can establish control over this settlement
    /// </summary>
    public bool CanBeControlledBy(InfluenceProfile challenger)
    {
        if (challenger == null) return false;
        
        // Must meet minimum threshold
        if (challenger.TotalInfluence < RequiredInfluenceThreshold)
            return false;
        
        // If uncontrolled, anyone with sufficient influence can control
        if (CurrentControllerId == null)
            return true;
        
        // If controlled, challenger must have significantly more influence
        float requiredAdvantage = CurrentControllerInfluence.TotalInfluence * 1.25f;
        return challenger.TotalInfluence >= requiredAdvantage;
    }
    
    /// <summary>
    /// Transfer control to a new entity
    /// </summary>
    public ControlTransferResult TransferControl(InfluenceProfile newController)
    {
        var previousController = CurrentControllerId;
        var previousInfluence = CurrentControllerInfluence;
        
        CurrentControllerId = newController.EntityId;
        CurrentControllerInfluence = newController;
        LastControlChange = DateTime.UtcNow;
        
        ControlHistory.Add(new ControlHistory
        {
            Timestamp = DateTime.UtcNow,
            PreviousControllerId = previousController,
            NewControllerId = newController.EntityId,
            TransferReason = DetermineTransferReason(previousInfluence, newController)
        });
        
        return new ControlTransferResult
        {
            Success = true,
            PreviousController = previousController,
            NewController = newController.EntityId,
            Settlement = this
        };
    }
    
    private TransferReason DetermineTransferReason(
        InfluenceProfile previous, 
        InfluenceProfile newController)
    {
        if (previous == null)
            return TransferReason.InitialClaim;
        
        var comparison = newController.CompareWith(previous);
        
        if (comparison.PoliticalDelta > comparison.EconomicDelta && 
            comparison.PoliticalDelta > comparison.MilitaryDelta)
            return TransferReason.PoliticalSupremacy;
        
        if (comparison.EconomicDelta > comparison.PoliticalDelta && 
            comparison.EconomicDelta > comparison.MilitaryDelta)
            return TransferReason.EconomicDominance;
        
        if (comparison.MilitaryDelta > comparison.PoliticalDelta && 
            comparison.MilitaryDelta > comparison.EconomicDelta)
            return TransferReason.MilitaryConquest;
        
        return TransferReason.GeneralSuperiority;
    }
    
    /// <summary>
    /// Register a challenge for control of this settlement
    /// </summary>
    public void RegisterControlChallenge(Guid challengerId, float challengeStrength)
    {
        InfluenceChallenges[challengerId] = challengeStrength;
    }
    
    /// <summary>
    /// Get the strongest challenger to current control
    /// </summary>
    public Guid? GetStrongestChallenger()
    {
        if (!InfluenceChallenges.Any())
            return null;
        
        return InfluenceChallenges
            .OrderByDescending(kvp => kvp.Value)
            .First()
            .Key;
    }
}

public enum SettlementSize
{
    Outpost,
    Village,
    Town,
    City,
    Metropolis
}

public enum SettlementType
{
    Mining,
    Agricultural,
    Trading,
    Military,
    Scientific,
    Mixed
}

public enum TransferReason
{
    InitialClaim,
    PoliticalSupremacy,
    EconomicDominance,
    MilitaryConquest,
    GeneralSuperiority,
    Diplomatic,
    Abandonment
}

public class ControlHistory
{
    public DateTime Timestamp { get; set; }
    public Guid? PreviousControllerId { get; set; }
    public Guid NewControllerId { get; set; }
    public TransferReason TransferReason { get; set; }
}

public class ControlTransferResult
{
    public bool Success { get; set; }
    public Guid? PreviousController { get; set; }
    public Guid NewController { get; set; }
    public Settlement Settlement { get; set; }
    public string Message { get; set; }
}
```

### 3. SettlementManager Class

The `SettlementManager` handles establishment and management of settlements:

```csharp
/// <summary>
/// Manages settlement establishment, control transfers, and territorial claims
/// </summary>
public class SettlementManager
{
    private Dictionary<Guid, Settlement> _settlements;
    private Dictionary<Guid, InfluenceProfile> _influenceProfiles;
    private DiplomacyManager _diplomacyManager;
    
    public SettlementManager(DiplomacyManager diplomacyManager)
    {
        _settlements = new Dictionary<Guid, Settlement>();
        _influenceProfiles = new Dictionary<Guid, InfluenceProfile>();
        _diplomacyManager = diplomacyManager;
    }
    
    /// <summary>
    /// Establish a new settlement
    /// </summary>
    public SettlementEstablishmentResult EstablishSettlement(
        InfluenceProfile founder,
        Coordinate3D location,
        string name,
        SettlementType type)
    {
        // Create new settlement
        var settlement = new Settlement
        {
            SettlementId = Guid.NewGuid(),
            Name = name,
            Location = location,
            Type = type,
            Size = SettlementSize.Outpost,
            Population = 0,
            StrategicValue = CalculateStrategicValue(location),
            AvailableResources = DiscoverLocalResources(location),
            Infrastructure = new List<Building>(),
            InfluenceChallenges = new Dictionary<Guid, float>(),
            ControlHistory = new List<ControlHistory>()
        };
        
        // Calculate influence requirements
        settlement.CalculateInfluenceThreshold();
        
        // Check if founder has sufficient influence
        if (!founder.CanControlSettlement(settlement))
        {
            return new SettlementEstablishmentResult
            {
                Success = false,
                Message = $"Insufficient influence to establish settlement. " +
                         $"Required: {settlement.RequiredInfluenceThreshold}, " +
                         $"Available: {founder.TotalInfluence}"
            };
        }
        
        // Establish control
        settlement.CurrentControllerId = founder.EntityId;
        settlement.CurrentControllerInfluence = founder;
        settlement.LastControlChange = DateTime.UtcNow;
        
        settlement.ControlHistory.Add(new ControlHistory
        {
            Timestamp = DateTime.UtcNow,
            PreviousControllerId = null,
            NewControllerId = founder.EntityId,
            TransferReason = TransferReason.InitialClaim
        });
        
        // Register settlement
        _settlements[settlement.SettlementId] = settlement;
        _influenceProfiles[founder.EntityId] = founder;
        
        return new SettlementEstablishmentResult
        {
            Success = true,
            Settlement = settlement,
            Message = $"Settlement '{name}' established successfully at {location}"
        };
    }
    
    /// <summary>
    /// Attempt to take control of an existing settlement
    /// </summary>
    public ControlTransferResult ChallengeSettlementControl(
        Guid settlementId,
        InfluenceProfile challenger)
    {
        if (!_settlements.TryGetValue(settlementId, out var settlement))
        {
            return new ControlTransferResult
            {
                Success = false,
                Message = "Settlement not found"
            };
        }
        
        // Check if challenger has sufficient influence
        if (!settlement.CanBeControlledBy(challenger))
        {
            return new ControlTransferResult
            {
                Success = false,
                Message = $"Insufficient influence to control settlement. " +
                         $"Required: {settlement.RequiredInfluenceThreshold * 1.25f}, " +
                         $"Available: {challenger.TotalInfluence}"
            };
        }
        
        // Check diplomatic relationships
        if (settlement.CurrentControllerId.HasValue)
        {
            var currentController = _influenceProfiles[settlement.CurrentControllerId.Value];
            var relationship = _diplomacyManager.GetRelationship(
                challenger.EntityId, 
                currentController.EntityId);
            
            // Cannot challenge allies without breaking alliance
            if (relationship?.RelationType == RelationType.Alliance)
            {
                return new ControlTransferResult
                {
                    Success = false,
                    Message = "Cannot challenge settlement controlled by ally. " +
                             "Break alliance first."
                };
            }
        }
        
        // Transfer control
        return settlement.TransferControl(challenger);
    }
    
    /// <summary>
    /// Update influence profiles and check for control changes
    /// </summary>
    public void UpdateSettlementControl(float deltaTime)
    {
        foreach (var settlement in _settlements.Values)
        {
            // Recalculate influence threshold based on settlement growth
            settlement.CalculateInfluenceThreshold();
            
            // Check if current controller still has sufficient influence
            if (settlement.CurrentControllerId.HasValue)
            {
                var controller = _influenceProfiles[settlement.CurrentControllerId.Value];
                controller.RecalculateInfluence();
                
                // Loss of control if influence drops below threshold
                if (controller.TotalInfluence < settlement.RequiredInfluenceThreshold)
                {
                    HandleLossOfControl(settlement, controller);
                }
            }
            
            // Process pending challenges
            ProcessControlChallenges(settlement);
        }
    }
    
    private void HandleLossOfControl(Settlement settlement, InfluenceProfile controller)
    {
        // Check if there's a challenger with sufficient influence
        var strongestChallenger = settlement.GetStrongestChallenger();
        
        if (strongestChallenger.HasValue && 
            _influenceProfiles.TryGetValue(strongestChallenger.Value, out var challenger))
        {
            if (challenger.CanControlSettlement(settlement))
            {
                settlement.TransferControl(challenger);
                return;
            }
        }
        
        // No valid challenger - settlement becomes independent
        settlement.CurrentControllerId = null;
        settlement.CurrentControllerInfluence = null;
        settlement.LastControlChange = DateTime.UtcNow;
    }
    
    private void ProcessControlChallenges(Settlement settlement)
    {
        // Remove expired challenges
        var expiredChallenges = new List<Guid>();
        foreach (var challenge in settlement.InfluenceChallenges)
        {
            // Challenges expire after certain time or if influence drops
            if (!_influenceProfiles.TryGetValue(challenge.Key, out var challenger) ||
                challenger.TotalInfluence < settlement.RequiredInfluenceThreshold)
            {
                expiredChallenges.Add(challenge.Key);
            }
        }
        
        foreach (var expired in expiredChallenges)
        {
            settlement.InfluenceChallenges.Remove(expired);
        }
    }
    
    private float CalculateStrategicValue(Coordinate3D location)
    {
        // Strategic value based on location properties
        // This would integrate with terrain, resource, and trade route systems
        return 50f; // Placeholder
    }
    
    private List<Resource> DiscoverLocalResources(Coordinate3D location)
    {
        // Discover resources available at this location
        // This would integrate with the geological and resource systems
        return new List<Resource>(); // Placeholder
    }
    
    /// <summary>
    /// Get all settlements controlled by an entity
    /// </summary>
    public List<Settlement> GetSettlementsControlledBy(Guid entityId)
    {
        return _settlements.Values
            .Where(s => s.CurrentControllerId == entityId)
            .ToList();
    }
    
    /// <summary>
    /// Get settlement by ID
    /// </summary>
    public Settlement GetSettlement(Guid settlementId)
    {
        return _settlements.GetValueOrDefault(settlementId);
    }
}

public class SettlementEstablishmentResult
{
    public bool Success { get; set; }
    public Settlement Settlement { get; set; }
    public string Message { get; set; }
}
```

### 4. DiplomacyManager Class

The `DiplomacyManager` handles relationships, rivalries, and alliances:

```csharp
/// <summary>
/// Manages diplomatic relationships between entities
/// </summary>
public class DiplomacyManager
{
    private Dictionary<Guid, Dictionary<Guid, DiplomaticRelationship>> _relationships;
    private List<Alliance> _alliances;
    private List<TerritorialDispute> _disputes;
    
    public DiplomacyManager()
    {
        _relationships = new Dictionary<Guid, Dictionary<Guid, DiplomaticRelationship>>();
        _alliances = new List<Alliance>();
        _disputes = new List<TerritorialDispute>();
    }
    
    /// <summary>
    /// Establish or update a diplomatic relationship
    /// </summary>
    public void EstablishRelationship(
        Guid entity1Id,
        Guid entity2Id,
        RelationType relationType,
        float relationshipStrength = 0f)
    {
        var relationship = new DiplomaticRelationship
        {
            Entity1Id = entity1Id,
            Entity2Id = entity2Id,
            RelationType = relationType,
            RelationshipStrength = relationshipStrength,
            EstablishedDate = DateTime.UtcNow,
            LastModified = DateTime.UtcNow
        };
        
        // Store bidirectional relationship
        if (!_relationships.ContainsKey(entity1Id))
            _relationships[entity1Id] = new Dictionary<Guid, DiplomaticRelationship>();
        
        if (!_relationships.ContainsKey(entity2Id))
            _relationships[entity2Id] = new Dictionary<Guid, DiplomaticRelationship>();
        
        _relationships[entity1Id][entity2Id] = relationship;
        _relationships[entity2Id][entity1Id] = relationship;
    }
    
    /// <summary>
    /// Get relationship between two entities
    /// </summary>
    public DiplomaticRelationship GetRelationship(Guid entity1Id, Guid entity2Id)
    {
        if (_relationships.TryGetValue(entity1Id, out var entity1Relations))
        {
            return entity1Relations.GetValueOrDefault(entity2Id);
        }
        
        return null;
    }
    
    /// <summary>
    /// Form an alliance between multiple entities
    /// </summary>
    public AllianceResult FormAlliance(
        List<Guid> memberIds,
        string allianceName,
        AllianceType allianceType)
    {
        if (memberIds.Count < 2)
        {
            return new AllianceResult
            {
                Success = false,
                Message = "Alliance requires at least 2 members"
            };
        }
        
        var alliance = new Alliance
        {
            AllianceId = Guid.NewGuid(),
            Name = allianceName,
            Type = allianceType,
            MemberIds = memberIds,
            FormedDate = DateTime.UtcNow,
            SharedResources = new Dictionary<Resource, float>(),
            SharedSettlements = new List<Guid>(),
            AlliancePolicies = new List<AlliancePolicy>()
        };
        
        _alliances.Add(alliance);
        
        // Establish alliance relationships between all members
        for (int i = 0; i < memberIds.Count; i++)
        {
            for (int j = i + 1; j < memberIds.Count; j++)
            {
                EstablishRelationship(
                    memberIds[i],
                    memberIds[j],
                    RelationType.Alliance,
                    100f);
            }
        }
        
        return new AllianceResult
        {
            Success = true,
            Alliance = alliance,
            Message = $"Alliance '{allianceName}' formed successfully"
        };
    }
    
    /// <summary>
    /// Create a territorial dispute
    /// </summary>
    public void CreateTerritorialDispute(
        Guid settlementId,
        Guid claimant1Id,
        Guid claimant2Id,
        string reason)
    {
        var dispute = new TerritorialDispute
        {
            DisputeId = Guid.NewGuid(),
            SettlementId = settlementId,
            Claimant1Id = claimant1Id,
            Claimant2Id = claimant2Id,
            Reason = reason,
            CreatedDate = DateTime.UtcNow,
            Status = DisputeStatus.Active
        };
        
        _disputes.Add(dispute);
        
        // Deteriorate relationship between claimants
        var existingRelation = GetRelationship(claimant1Id, claimant2Id);
        if (existingRelation != null)
        {
            existingRelation.RelationshipStrength -= 20f;
            if (existingRelation.RelationshipStrength < -50f)
            {
                existingRelation.RelationType = RelationType.Rivalry;
            }
        }
        else
        {
            EstablishRelationship(claimant1Id, claimant2Id, RelationType.Neutral, -20f);
        }
    }
    
    /// <summary>
    /// Resolve a territorial dispute
    /// </summary>
    public DisputeResolutionResult ResolveTerritorialDispute(
        Guid disputeId,
        Guid winnerId,
        ResolutionMethod method)
    {
        var dispute = _disputes.FirstOrDefault(d => d.DisputeId == disputeId);
        if (dispute == null)
        {
            return new DisputeResolutionResult
            {
                Success = false,
                Message = "Dispute not found"
            };
        }
        
        dispute.Status = DisputeStatus.Resolved;
        dispute.ResolutionDate = DateTime.UtcNow;
        dispute.WinnerId = winnerId;
        dispute.ResolutionMethod = method;
        
        // Update relationships based on resolution method
        if (method == ResolutionMethod.Diplomatic)
        {
            // Diplomatic resolution improves relations slightly
            var relation = GetRelationship(dispute.Claimant1Id, dispute.Claimant2Id);
            if (relation != null)
            {
                relation.RelationshipStrength += 10f;
            }
        }
        else if (method == ResolutionMethod.Military)
        {
            // Military resolution damages relations significantly
            var relation = GetRelationship(dispute.Claimant1Id, dispute.Claimant2Id);
            if (relation != null)
            {
                relation.RelationshipStrength -= 30f;
                relation.RelationType = RelationType.Rivalry;
            }
        }
        
        return new DisputeResolutionResult
        {
            Success = true,
            Dispute = dispute,
            Message = $"Dispute resolved via {method}"
        };
    }
    
    /// <summary>
    /// Get all active disputes involving an entity
    /// </summary>
    public List<TerritorialDispute> GetActiveDisputes(Guid entityId)
    {
        return _disputes
            .Where(d => d.Status == DisputeStatus.Active &&
                       (d.Claimant1Id == entityId || d.Claimant2Id == entityId))
            .ToList();
    }
    
    /// <summary>
    /// Get all alliances an entity is part of
    /// </summary>
    public List<Alliance> GetAlliances(Guid entityId)
    {
        return _alliances
            .Where(a => a.MemberIds.Contains(entityId))
            .ToList();
    }
    
    /// <summary>
    /// Update diplomatic relationships over time
    /// </summary>
    public void UpdateDiplomacy(float deltaTime)
    {
        // Decay relationships toward neutral over time
        foreach (var entity1Relations in _relationships.Values)
        {
            foreach (var relationship in entity1Relations.Values)
            {
                // Rivalries and alliances don't decay naturally
                if (relationship.RelationType == RelationType.Neutral)
                {
                    // Neutral relationships slowly fade
                    relationship.RelationshipStrength *= 0.999f;
                }
            }
        }
    }
}

public class DiplomaticRelationship
{
    public Guid Entity1Id { get; set; }
    public Guid Entity2Id { get; set; }
    public RelationType RelationType { get; set; }
    public float RelationshipStrength { get; set; }  // -100 to +100
    public DateTime EstablishedDate { get; set; }
    public DateTime LastModified { get; set; }
    public List<DiplomaticEvent> History { get; set; }
}

public enum RelationType
{
    Alliance,
    Friendly,
    Neutral,
    Unfriendly,
    Rivalry,
    War
}

public class Alliance
{
    public Guid AllianceId { get; set; }
    public string Name { get; set; }
    public AllianceType Type { get; set; }
    public List<Guid> MemberIds { get; set; }
    public DateTime FormedDate { get; set; }
    public Dictionary<Resource, float> SharedResources { get; set; }
    public List<Guid> SharedSettlements { get; set; }
    public List<AlliancePolicy> AlliancePolicies { get; set; }
}

public enum AllianceType
{
    Trade,           // Economic cooperation
    Military,        // Mutual defense
    Scientific,      // Knowledge sharing
    Federation       // Full political union
}

public class TerritorialDispute
{
    public Guid DisputeId { get; set; }
    public Guid SettlementId { get; set; }
    public Guid Claimant1Id { get; set; }
    public Guid Claimant2Id { get; set; }
    public string Reason { get; set; }
    public DateTime CreatedDate { get; set; }
    public DisputeStatus Status { get; set; }
    public DateTime? ResolutionDate { get; set; }
    public Guid? WinnerId { get; set; }
    public ResolutionMethod? ResolutionMethod { get; set; }
}

public enum DisputeStatus
{
    Active,
    UnderNegotiation,
    Resolved,
    Escalated
}

public enum ResolutionMethod
{
    Diplomatic,
    Economic,
    Military,
    Arbitration
}

public class AllianceResult
{
    public bool Success { get; set; }
    public Alliance Alliance { get; set; }
    public string Message { get; set; }
}

public class DisputeResolutionResult
{
    public bool Success { get; set; }
    public TerritorialDispute Dispute { get; set; }
    public string Message { get; set; }
}
```

### 5. SocialInteractionSystem Class

The `SocialInteractionSystem` ties everything together:

```csharp
/// <summary>
/// Central system that orchestrates settlement control, influence, and diplomacy
/// </summary>
public class SocialInteractionSystem
{
    private SettlementManager _settlementManager;
    private DiplomacyManager _diplomacyManager;
    private Dictionary<Guid, InfluenceProfile> _influenceProfiles;
    
    public SocialInteractionSystem()
    {
        _diplomacyManager = new DiplomacyManager();
        _settlementManager = new SettlementManager(_diplomacyManager);
        _influenceProfiles = new Dictionary<Guid, InfluenceProfile>();
    }
    
    /// <summary>
    /// Register a new entity (player, guild, dynasty, etc.)
    /// </summary>
    public void RegisterEntity(InfluenceProfile profile)
    {
        _influenceProfiles[profile.EntityId] = profile;
    }
    
    /// <summary>
    /// Update an entity's influence profile
    /// </summary>
    public void UpdateInfluenceProfile(
        Guid entityId,
        InfluenceSource source,
        float value,
        InfluenceType type)
    {
        if (!_influenceProfiles.TryGetValue(entityId, out var profile))
            return;
        
        switch (type)
        {
            case InfluenceType.Political:
                profile.PoliticalSources[source] = value;
                break;
            case InfluenceType.Economic:
                profile.EconomicSources[source] = value;
                break;
            case InfluenceType.Military:
                profile.MilitarySources[source] = value;
                break;
        }
        
        profile.RecalculateInfluence();
    }
    
    /// <summary>
    /// Main update loop for the social interaction system
    /// </summary>
    public void Update(float deltaTime)
    {
        // Update all influence profiles
        foreach (var profile in _influenceProfiles.Values)
        {
            profile.RecalculateInfluence();
        }
        
        // Update settlement control based on influence changes
        _settlementManager.UpdateSettlementControl(deltaTime);
        
        // Update diplomatic relationships
        _diplomacyManager.UpdateDiplomacy(deltaTime);
        
        // Process any pending events
        ProcessEvents();
    }
    
    /// <summary>
    /// Attempt to establish a new settlement
    /// </summary>
    public SettlementEstablishmentResult EstablishSettlement(
        Guid founderId,
        Coordinate3D location,
        string name,
        SettlementType type)
    {
        if (!_influenceProfiles.TryGetValue(founderId, out var founder))
        {
            return new SettlementEstablishmentResult
            {
                Success = false,
                Message = "Entity not found"
            };
        }
        
        return _settlementManager.EstablishSettlement(founder, location, name, type);
    }
    
    /// <summary>
    /// Challenge control of an existing settlement
    /// </summary>
    public ControlTransferResult ChallengeSettlement(
        Guid challengerId,
        Guid settlementId)
    {
        if (!_influenceProfiles.TryGetValue(challengerId, out var challenger))
        {
            return new ControlTransferResult
            {
                Success = false,
                Message = "Challenger entity not found"
            };
        }
        
        return _settlementManager.ChallengeSettlementControl(settlementId, challenger);
    }
    
    /// <summary>
    /// Form an alliance
    /// </summary>
    public AllianceResult FormAlliance(
        List<Guid> memberIds,
        string allianceName,
        AllianceType allianceType)
    {
        // Verify all members exist
        foreach (var memberId in memberIds)
        {
            if (!_influenceProfiles.ContainsKey(memberId))
            {
                return new AllianceResult
                {
                    Success = false,
                    Message = $"Entity {memberId} not found"
                };
            }
        }
        
        return _diplomacyManager.FormAlliance(memberIds, allianceName, allianceType);
    }
    
    /// <summary>
    /// Create a territorial dispute
    /// </summary>
    public void CreateTerritorialDispute(
        Guid settlementId,
        Guid claimant1Id,
        Guid claimant2Id,
        string reason)
    {
        _diplomacyManager.CreateTerritorialDispute(
            settlementId,
            claimant1Id,
            claimant2Id,
            reason);
    }
    
    /// <summary>
    /// Get entity's influence profile
    /// </summary>
    public InfluenceProfile GetInfluenceProfile(Guid entityId)
    {
        return _influenceProfiles.GetValueOrDefault(entityId);
    }
    
    /// <summary>
    /// Get all settlements controlled by an entity
    /// </summary>
    public List<Settlement> GetControlledSettlements(Guid entityId)
    {
        return _settlementManager.GetSettlementsControlledBy(entityId);
    }
    
    /// <summary>
    /// Get diplomatic relationships for an entity
    /// </summary>
    public List<Alliance> GetAlliances(Guid entityId)
    {
        return _diplomacyManager.GetAlliances(entityId);
    }
    
    /// <summary>
    /// Get active disputes involving an entity
    /// </summary>
    public List<TerritorialDispute> GetActiveDisputes(Guid entityId)
    {
        return _diplomacyManager.GetActiveDisputes(entityId);
    }
    
    private void ProcessEvents()
    {
        // Process system events like automatic control transfers,
        // dispute escalations, alliance breakdowns, etc.
    }
}

public enum InfluenceType
{
    Political,
    Economic,
    Military
}
```

## Game Mechanics

### Control Establishment

1. **Initial Settlement Founding:**
   - Entity must have minimum influence threshold
   - Calculate threshold based on location, resources, strategic value
   - Establish control and add to control history

2. **Control Challenge:**
   - Challenger must have 125% of current controller's influence
   - Cannot challenge allies without breaking alliance
   - Successful challenge transfers control
   - Records transfer reason (political, economic, or military supremacy)

3. **Control Loss:**
   - If controller's influence drops below threshold
   - Settlement becomes independent or transfers to strongest challenger
   - Historical control changes tracked

### Influence Calculation

**Political Influence Sources:**

- Diplomatic ties and treaties
- Population support and approval
- Governance institutions
- Cultural dominance
- Historical claims to territory

**Economic Influence Sources:**

- Accumulated wealth
- Trade route control
- Infrastructure ownership
- Resource production capacity
- Market share and dominance

**Military Influence Sources:**

- Military force strength
- Defensive fortifications
- Strategic positioning
- Allied military support
- Supply chain security

### Diplomatic Relationships

**Relationship Types:**

- **Alliance:** Mutual cooperation, cannot challenge settlements
- **Friendly:** Positive relations, bonuses to trade/cooperation
- **Neutral:** No special relationship
- **Unfriendly:** Negative relations, penalties
- **Rivalry:** Active competition, frequent disputes
- **War:** Open conflict

**Alliance Types:**

- **Trade Alliance:** Economic cooperation and shared markets
- **Military Alliance:** Mutual defense pact
- **Scientific Alliance:** Knowledge and research sharing
- **Federation:** Full political union with shared governance

### Territorial Disputes

**Dispute Creation:**

- Two entities claim same settlement
- Automatically creates rivalry or worsens relationship
- Can be resolved through multiple methods

**Resolution Methods:**

- **Diplomatic:** Negotiation and compromise
- **Economic:** Economic pressure or buyout
- **Military:** Armed conflict
- **Arbitration:** Third-party mediation

## Integration with Other Systems

### Geological and Resource Systems

Settlement control is tied to:

- Local geological resources (minerals, water, fertile land)
- Strategic locations (trade routes, defensible positions)
- Infrastructure built on geological understanding

### Dynasty System

Dynasties accumulate influence through:

- Generational knowledge transfer
- Territorial holdings
- Political standing in region
- Specialized expertise

### Guild System

Guilds contribute to influence through:

- Economic power from trade networks
- Specialized knowledge and expertise
- Political power through membership
- Infrastructure ownership

### Economy System

Economic influence derived from:

- Wealth accumulation
- Trade route control
- Market manipulation
- Resource monopolization

## Example Usage Scenarios

### Scenario 1: Founding a New Mining Settlement

```csharp
var socialSystem = new SocialInteractionSystem();

// Create player influence profile
var playerProfile = new InfluenceProfile
{
    EntityId = Guid.NewGuid(),
    EntityName = "GeologicalExperts Guild",
    EntityType = EntityType.Guild
};

// Add influence sources
playerProfile.EconomicSources[InfluenceSource.AccumulatedWealth] = 150f;
playerProfile.PoliticalSources[InfluenceSource.PopulationSupport] = 80f;
playerProfile.EconomicSources[InfluenceSource.InfrastructureOwnership] = 120f;
playerProfile.RecalculateInfluence();

// Register entity
socialSystem.RegisterEntity(playerProfile);

// Establish settlement
var result = socialSystem.EstablishSettlement(
    playerProfile.EntityId,
    new Coordinate3D(100, 200, 50),
    "Rockshire Mine",
    SettlementType.Mining);

if (result.Success)
{
    Console.WriteLine($"Settlement established: {result.Settlement.Name}");
    Console.WriteLine($"Required influence: {result.Settlement.RequiredInfluenceThreshold}");
    Console.WriteLine($"Your influence: {playerProfile.TotalInfluence}");
}
```

### Scenario 2: Challenging Settlement Control

```csharp
// Rival guild builds influence
var rivalProfile = new InfluenceProfile
{
    EntityId = Guid.NewGuid(),
    EntityName = "Mineral Traders Consortium",
    EntityType = EntityType.Guild
};

rivalProfile.EconomicSources[InfluenceSource.AccumulatedWealth] = 200f;
rivalProfile.EconomicSources[InfluenceSource.TradeRouteControl] = 180f;
rivalProfile.MilitarySources[InfluenceSource.MilitaryStrength] = 100f;
rivalProfile.RecalculateInfluence();

socialSystem.RegisterEntity(rivalProfile);

// Challenge control
var challengeResult = socialSystem.ChallengeSettlement(
    rivalProfile.EntityId,
    settlement.SettlementId);

if (challengeResult.Success)
{
    Console.WriteLine("Control transferred!");
    Console.WriteLine($"Reason: {settlement.ControlHistory.Last().TransferReason}");
}
```

### Scenario 3: Forming a Trade Alliance

```csharp
// Three guilds form trade alliance
var memberIds = new List<Guid>
{
    guild1Profile.EntityId,
    guild2Profile.EntityId,
    guild3Profile.EntityId
};

var allianceResult = socialSystem.FormAlliance(
    memberIds,
    "Northern Trade Federation",
    AllianceType.Trade);

if (allianceResult.Success)
{
    Console.WriteLine($"Alliance '{allianceResult.Alliance.Name}' formed!");
    Console.WriteLine($"Members: {allianceResult.Alliance.MemberIds.Count}");
    
    // Alliance members cannot challenge each other's settlements
    // They can share resources and coordinate economically
}
```

### Scenario 4: Territorial Dispute

```csharp
// Two entities claim the same settlement
socialSystem.CreateTerritorialDispute(
    settlement.SettlementId,
    entity1Id,
    entity2Id,
    "Historical claim vs. current occupation");

// Resolve diplomatically
var disputes = socialSystem.GetActiveDisputes(entity1Id);
foreach (var dispute in disputes)
{
    var resolution = _diplomacyManager.ResolveTerritorialDispute(
        dispute.DisputeId,
        entity1Id,
        ResolutionMethod.Diplomatic);
    
    if (resolution.Success)
    {
        Console.WriteLine("Dispute resolved peacefully");
        // Relationship slightly improved
    }
}
```

## Performance Considerations

### Optimization Strategies

1. **Influence Recalculation:**
   - Only recalculate when sources change
   - Cache total influence values
   - Batch updates during system tick

2. **Relationship Queries:**
   - Index relationships by entity ID
   - Cache frequently accessed relationships
   - Use spatial partitioning for settlement queries

3. **Control Checks:**
   - Only check control validity when influence changes significantly
   - Use event-driven updates rather than polling
   - Batch settlement updates

### Scalability

- System designed to handle thousands of settlements
- O(1) lookups for settlements and relationships
- Linear complexity for alliance operations
- Efficient dispute resolution tracking

## Future Enhancements

### Potential Additions

1. **Influence Maps:**
   - Visual representation of territorial influence
   - Gradual influence falloff from settlements
   - Contested zones between entities

2. **Reputation System:**
   - Entity reputation affects diplomatic options
   - Historical actions influence future relationships
   - Reputation decay over time

3. **Dynamic Alliance Goals:**
   - Alliances can set shared objectives
   - Progress tracking toward alliance goals
   - Rewards for achieving alliance milestones

4. **Economic Sanctions:**
   - Entities can impose trade restrictions
   - Economic warfare as alternative to military conflict
   - Impact on economic influence

5. **Cultural Influence:**
   - Separate cultural influence system
   - Cultural dominance affects settlement loyalty
   - Language, traditions, and customs spread

## Advanced Game Mechanics in Practice

### Scenario 5: Maintaining a Decaying Settlement (Rust-Inspired)

```csharp
var settlementManager = new SettlementManager(diplomacyManager);
var settlement = settlementManager.GetSettlement(settlementId);

// Check upkeep requirements
var upkeep = settlement.Upkeep;
upkeep.CalculateUpkeepCosts(settlement);

Console.WriteLine($"Daily upkeep for {settlement.Name}:");
foreach (var cost in upkeep.DailyUpkeepCosts)
{
    Console.WriteLine($"  {cost.Key}: {cost.Value}");
}

// Player has resources available
var playerResources = new Dictionary<Resource, float>
{
    [Resource.Food] = 1000f,
    [Resource.Materials] = 500f,
    [Resource.Currency] = 750f
};

// Process upkeep payment
upkeep.ProcessUpkeep(playerResources);

if (upkeep.DecayAccumulator > 0)
{
    Console.WriteLine($"Warning: Settlement is decaying! Decay level: {upkeep.GetDecayLevel() * 100}%");
    Console.WriteLine($"Control threshold reduced by {upkeep.GetDecayLevel() * 50}%");
}
else
{
    Console.WriteLine("Upkeep paid successfully. Settlement maintained.");
}
```

### Scenario 6: Cultural Expansion System (Travian-Inspired)

```csharp
var culturalSystem = new CulturalInfluencePoints
{
    EntityId = playerProfile.EntityId,
    AvailablePoints = 0f
};

// Generate culture points over time
var controlledSettlements = settlementManager.GetSettlementsControlledBy(playerProfile.EntityId);
culturalSystem.PointGenerationRate = culturalSystem.CalculatePointGeneration(controlledSettlements);

Console.WriteLine($"Generating {culturalSystem.PointGenerationRate} culture points per day");
Console.WriteLine($"Current culture points: {culturalSystem.AvailablePoints}");

// Accumulate points
culturalSystem.AvailablePoints += culturalSystem.PointGenerationRate;

// Attempt to found new settlement
if (culturalSystem.CanAffordSettlement(SettlementSize.Village))
{
    Console.WriteLine("Sufficient culture points to found a new village!");
    
    var result = settlementManager.EstablishSettlement(
        playerProfile,
        new Coordinate3D(150, 250, 60),
        "New Frontier Village",
        SettlementType.Agricultural);
    
    if (result.Success)
    {
        // Deduct culture points
        float cost = 1000f;  // Village cost
        culturalSystem.AvailablePoints -= cost;
        Console.WriteLine($"Village founded! Remaining culture points: {culturalSystem.AvailablePoints}");
    }
}
```

### Scenario 7: Loyalty and Conquest (Travian-Inspired)

```csharp
// Player conquers a settlement
var conqueredSettlement = settlementManager.GetSettlement(targetSettlementId);
var conquerorProfile = socialSystem.GetInfluenceProfile(conquerorId);

// Transfer control
var transferResult = settlementManager.ChallengeSettlementControl(
    targetSettlementId,
    conquerorProfile);

if (transferResult.Success)
{
    // Initialize loyalty system for conquered settlement
    conqueredSettlement.Loyalty = new SettlementLoyalty
    {
        SettlementId = conqueredSettlement.SettlementId,
        CurrentLoyalty = 20f,  // Start with low loyalty
        OriginalFounderId = transferResult.PreviousController.Value,
        CurrentControllerId = conquerorId
    };
    
    Console.WriteLine($"Settlement conquered! Starting loyalty: {conqueredSettlement.Loyalty.CurrentLoyalty}%");
    Console.WriteLine($"Control stability: {conqueredSettlement.Loyalty.ControlStabilityModifier * 100}%");
    
    // Loyalty affects vulnerability
    float vulnerabilityMultiplier = conqueredSettlement.Loyalty.GetVulnerabilityMultiplier();
    Console.WriteLine($"Settlement is {vulnerabilityMultiplier}x more vulnerable to reconquest");
    
    // Over time, loyalty increases
    for (int days = 0; days < 100; days++)
    {
        conqueredSettlement.Loyalty.UpdateLoyalty(1.0f);  // 1 day
        
        if (days % 10 == 0)
        {
            Console.WriteLine($"Day {days}: Loyalty = {conqueredSettlement.Loyalty.CurrentLoyalty:F1}%");
        }
    }
}
```

### Scenario 8: Territory Claims and Permissions (Minecraft-Inspired)

```csharp
var claimSystem = new TerritoryClaimSystem
{
    EntityId = guildId,
    AvailableClaimBlocks = 5000f,
    Claims = new List<TerritorialClaim>()
};

// Create main territory claim
var mainClaim = claimSystem.CreateClaim(
    new Coordinate3D(200, 300, 50),
    radius: 50f);

if (mainClaim != null)
{
    Console.WriteLine($"Territory claimed! Area: {mainClaim.Area:F0} blocks");
    Console.WriteLine($"Remaining claim blocks: {claimSystem.AvailableClaimBlocks:F0}");
    
    // Grant permissions to guild members
    mainClaim.Permissions.GrantPermission(member1Id, PermissionFlags.Build | PermissionFlags.Harvest);
    mainClaim.Permissions.GrantPermission(member2Id, PermissionFlags.All);
    
    // Set public permissions
    mainClaim.Permissions.PublicPermissions = PermissionFlags.Visit | PermissionFlags.Interact;
    
    // Create subclaim for mining zone
    var miningZone = new SubClaim
    {
        SubClaimId = Guid.NewGuid(),
        ParentClaimId = mainClaim.ClaimId,
        Center = new Coordinate3D(210, 310, 45),
        Radius = 15f,
        Name = "Mining Zone",
        Permissions = new TerritoryPermissions()
    };
    
    // Different permissions in mining zone
    miningZone.Permissions.GrantPermission(miningGuildId, PermissionFlags.All);
    miningZone.Permissions.PublicPermissions = PermissionFlags.None;
    
    mainClaim.SubClaims.Add(miningZone);
    
    Console.WriteLine($"Created subclaim: {miningZone.Name}");
    
    // Check permissions
    bool canBuild = mainClaim.Permissions.HasPermission(member1Id, PermissionFlags.Build);
    Console.WriteLine($"Member 1 can build: {canBuild}");
    
    // Get visual boundaries for rendering
    var boundaryPoints = mainClaim.GetBoundaryPoints();
    Console.WriteLine($"Boundary has {boundaryPoints.Count} points for visualization");
}
```

### Scenario 9: Sovereignty Structures and Vulnerability Windows (EVE-Inspired)

```csharp
// Deploy sovereignty structure
var citadel = new SovereigntyStructure
{
    StructureId = Guid.NewGuid(),
    OwnerId = allianceId,
    Type = StructureType.Citadel,
    Location = new Coordinate3D(500, 500, 100),
    HealthPoints = 10000f,
    MaxHealthPoints = 10000f,
    State = SovereigntyState.Stable,
    VulnerabilityStart = TimeSpan.FromHours(18),  // 6 PM local time
    VulnerabilityDuration = TimeSpan.FromHours(3), // 3-hour window
    InfluenceRadius = 200f
};

Console.WriteLine($"Citadel deployed at {citadel.Location}");
Console.WriteLine($"Vulnerable: {citadel.VulnerabilityStart} - " +
                  $"{citadel.VulnerabilityStart.Add(citadel.VulnerabilityDuration)}");

// Check if currently vulnerable
var currentTime = DateTime.UtcNow;
bool isVulnerable = citadel.IsVulnerable(currentTime);
Console.WriteLine($"Currently vulnerable: {isVulnerable}");

// Calculate influence at various points
var testPoint1 = new Coordinate3D(550, 550, 100);  // Nearby
var testPoint2 = new Coordinate3D(800, 800, 100);  // Far away

float influence1 = citadel.GetInfluenceStrength(testPoint1);
float influence2 = citadel.GetInfluenceStrength(testPoint2);

Console.WriteLine($"Influence at nearby point: {influence1 * 100:F0}%");
Console.WriteLine($"Influence at distant point: {influence2 * 100:F0}%");

// Attack scenario - structure becomes reinforced
if (isVulnerable)
{
    citadel.HealthPoints -= 5000f;  // Take damage
    
    if (citadel.HealthPoints < citadel.MaxHealthPoints * 0.25f)
    {
        citadel.State = SovereigntyState.Reinforced;
        Console.WriteLine("Citadel reinforced! Waiting for next vulnerability window...");
    }
}
```

### Scenario 10: Contested Territory with Multi-Phase Conquest (EVE-Inspired)

```csharp
// Initiate territorial conflict
var contestedTerritory = new ContestedTerritory
{
    TerritoryId = territoryId,
    AttackerId = attackingGuildId,
    DefenderId = defendingGuildId,
    CurrentPhase = ConflictPhase.Initial,
    PhaseStartTime = DateTime.UtcNow,
    AttackerProgress = 0f
};

Console.WriteLine($"Territorial conflict initiated!");
Console.WriteLine($"Attacker: {attackingGuildId}");
Console.WriteLine($"Defender: {defendingGuildId}");

// Simulate conflict progression
while (contestedTerritory.CurrentPhase != ConflictPhase.Captured)
{
    var timeRemaining = contestedTerritory.GetPhaseTimeRemaining();
    Console.WriteLine($"\nPhase: {contestedTerritory.CurrentPhase}");
    Console.WriteLine($"Time remaining: {timeRemaining.TotalHours:F1} hours");
    Console.WriteLine($"Attacker progress: {contestedTerritory.AttackerProgress:F0}%");
    
    // Simulate attacker making progress
    contestedTerritory.AttackerProgress += 25f;
    
    if (contestedTerritory.AttackerProgress >= 100f)
    {
        contestedTerritory.AttackerProgress = 0f;
        contestedTerritory.AdvancePhase();
        Console.WriteLine($"Phase complete! Advancing to {contestedTerritory.CurrentPhase}");
    }
    
    if (contestedTerritory.CurrentPhase == ConflictPhase.Captured)
    {
        Console.WriteLine("\nTerritory captured!");
        
        // Transfer control to attacker
        var settlement = settlementManager.GetSettlement(territoryId);
        var attackerProfile = socialSystem.GetInfluenceProfile(attackingGuildId);
        settlement.TransferControl(attackerProfile);
        
        break;
    }
    
    // Break after a few iterations for demo
    if (contestedTerritory.CurrentPhase == ConflictPhase.FinalBattle)
        break;
}
```

### Scenario 11: Influence Map Visualization (EVE-Inspired)

```csharp
// Create influence map for a region
var influenceMap = new InfluenceMap();

// Multiple factions have structures in the region
var structures = new List<SovereigntyStructure>
{
    new SovereigntyStructure 
    { 
        OwnerId = faction1Id, 
        Location = new Coordinate3D(100, 100, 50),
        InfluenceRadius = 150f,
        HealthPoints = 10000f,
        MaxHealthPoints = 10000f
    },
    new SovereigntyStructure 
    { 
        OwnerId = faction2Id, 
        Location = new Coordinate3D(250, 100, 50),
        InfluenceRadius = 150f,
        HealthPoints = 8000f,
        MaxHealthPoints = 10000f
    },
    new SovereigntyStructure 
    { 
        OwnerId = faction3Id, 
        Location = new Coordinate3D(175, 200, 50),
        InfluenceRadius = 100f,
        HealthPoints = 6000f,
        MaxHealthPoints = 10000f
    }
};

// Calculate influence across the region
influenceMap.CalculateInfluence(structures);

// Check control at specific points
var checkPoint = new Coordinate3D(175, 150, 50);
var dominantController = influenceMap.GetDominantController(checkPoint);
bool isContested = influenceMap.IsContested(checkPoint);

if (dominantController.HasValue)
{
    Console.WriteLine($"Point {checkPoint} controlled by: {dominantController.Value}");
}

if (isContested)
{
    Console.WriteLine("This area is contested between multiple factions!");
}
else
{
    Console.WriteLine("This area has clear control.");
}

// Visualize influence map (pseudo-code for rendering)
Console.WriteLine("\nInfluence Map (ASCII representation):");
for (int y = 0; y < 30; y++)
{
    for (int x = 0; x < 40; x++)
    {
        var point = new Coordinate3D(x * 10, y * 10, 50);
        var controller = influenceMap.GetDominantController(point);
        var contested = influenceMap.IsContested(point);
        
        if (contested)
            Console.Write("?");
        else if (controller == faction1Id)
            Console.Write("1");
        else if (controller == faction2Id)
            Console.Write("2");
        else if (controller == faction3Id)
            Console.Write("3");
        else
            Console.Write(".");
    }
    Console.WriteLine();
}
```

### Scenario 12: Integrated Settlement with All Systems

```csharp
// Create a fully-featured settlement using all game-inspired mechanics
var advancedSettlement = new Settlement
{
    SettlementId = Guid.NewGuid(),
    Name = "Fortress Prime",
    Location = new Coordinate3D(1000, 1000, 100),
    Size = SettlementSize.City,
    Type = SettlementType.Military,
    Population = 5000,
    StrategicValue = 85f,
    
    // Loyalty system (Travian-inspired)
    Loyalty = new SettlementLoyalty
    {
        CurrentLoyalty = 75f,
        OriginalFounderId = founderId,
        CurrentControllerId = founderId
    },
    
    // Upkeep system (Rust-inspired)
    Upkeep = new SettlementUpkeep
    {
        SettlementId = Guid.NewGuid(),
        LastUpkeepPaid = DateTime.UtcNow,
        DecayAccumulator = 0f
    },
    
    // Authorization system (Rust-inspired)
    Authorization = new SettlementAuthorization
    {
        SettlementId = Guid.NewGuid(),
        Authorizations = new Dictionary<Guid, SettlementAuthorization.AuthorizationLevel>
        {
            [founderId] = SettlementAuthorization.AuthorizationLevel.Owner,
            [governor1Id] = SettlementAuthorization.AuthorizationLevel.Governor,
            [builder1Id] = SettlementAuthorization.AuthorizationLevel.Builder
        }
    },
    
    // Sovereignty structures (EVE-inspired)
    SovereigntyStructures = new List<SovereigntyStructure>
    {
        new SovereigntyStructure
        {
            Type = StructureType.Citadel,
            Location = new Coordinate3D(1000, 1000, 100),
            InfluenceRadius = 250f,
            VulnerabilityStart = TimeSpan.FromHours(20),
            VulnerabilityDuration = TimeSpan.FromHours(2)
        }
    },
    
    // Territory claim (Minecraft-inspired)
    TerritorialClaim = new TerritorialClaim
    {
        ClaimId = Guid.NewGuid(),
        OwnerId = founderId,
        Center = new Coordinate3D(1000, 1000, 100),
        Radius = 300f,
        Permissions = new TerritoryPermissions
        {
            PublicPermissions = PermissionFlags.Visit
        }
    }
};

// Calculate influence requirements with all modifiers
advancedSettlement.Upkeep.CalculateUpkeepCosts(advancedSettlement);
float influenceThreshold = advancedSettlement.CalculateInfluenceThreshold();

Console.WriteLine($"\n=== {advancedSettlement.Name} Status ===");
Console.WriteLine($"Type: {advancedSettlement.Type} {advancedSettlement.Size}");
Console.WriteLine($"Population: {advancedSettlement.Population:N0}");
Console.WriteLine($"Strategic Value: {advancedSettlement.StrategicValue:F0}");
Console.WriteLine($"\nLoyalty: {advancedSettlement.Loyalty.CurrentLoyalty:F0}%");
Console.WriteLine($"Control Stability: {advancedSettlement.Loyalty.ControlStabilityModifier * 100:F0}%");
Console.WriteLine($"\nRequired Influence: {influenceThreshold:F0}");
Console.WriteLine($"Decay Level: {advancedSettlement.Upkeep.GetDecayLevel() * 100:F0}%");
Console.WriteLine($"\nDaily Upkeep:");
foreach (var cost in advancedSettlement.Upkeep.DailyUpkeepCosts)
{
    Console.WriteLine($"  {cost.Key}: {cost.Value:F0}");
}
Console.WriteLine($"\nSovereignty Structures: {advancedSettlement.SovereigntyStructures.Count}");
Console.WriteLine($"Territory Radius: {advancedSettlement.TerritorialClaim.Radius:F0}m");
Console.WriteLine($"Authorized Users: {advancedSettlement.Authorization.Authorizations.Count}");
```

## Conclusion

The Settlement Control System provides a robust, influence-based framework for territorial control in
BlueMarble. By basing control on measurable political, economic, and military influence rather than
arbitrary mechanics, the system creates meaningful strategic depth and realistic power dynamics.

Entities must build and maintain their influence through concrete actions, diplomatic relationships
shape territorial possibilities, and control can shift dynamically as power balances change. This
creates emergent gameplay where players' economic decisions, infrastructure investments, and diplomatic
choices all contribute to their territorial power.

The system integrates seamlessly with BlueMarble's existing geological, dynasty, and guild systems,
creating a cohesive framework for player interaction and competition at all scales - from individual
settlements to continental alliances.
