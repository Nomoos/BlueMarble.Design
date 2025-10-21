---
title: Base Building Mechanics and Construction Systems
date: 2025-01-17
tags: [research, survival, base-building, construction, gameplay-mechanics]
status: complete
priority: Medium
phase: 2
group: 05
batch: 3
source_type: analysis
category: survival + gamedev-design
estimated_effort: 6-8h
---

# Base Building Mechanics and Construction Systems

**Document Type:** Research Analysis  
**Research Phase:** Phase 2, Group 05, Batch 3  
**Priority:** Medium  
**Category:** Survival + GameDev-Design  
**Estimated Effort:** 6-8 hours

---

## Executive Summary

Base building is a core pillar of survival gameplay, providing players with shelter, storage, crafting facilities, and a sense of progression and permanence in the game world. This research examines construction mechanics from placement systems and structural integrity to upgrade paths and multiplayer considerations. The analysis focuses on creating intuitive, flexible building systems that balance creativity with gameplay constraints.

Key findings show that successful base building systems must balance **creative freedom** (player expression), **structural realism** (believable constraints), and **gameplay integration** (meaningful progression). For BlueMarble's web-based platform, special attention must be paid to intuitive placement controls, performant networked synchronization, and anti-griefing measures for multiplayer environments.

The recommended approach uses a modular building system with snapping and free-placement modes, structural integrity calculations that guide rather than restrict, and tiered construction materials that enable clear progression paths while maintaining creative flexibility.

---

## Core Concepts and Analysis

### 1. Placement Systems

#### 1.1 Grid-Based vs Free-Form Placement

```csharp
public enum PlacementMode
{
    GridSnap,      // Snap to fixed grid
    FreeForm,      // Place anywhere
    FoundationSnap // Snap to existing structures
}

public class BuildingPlacementSystem
{
    public struct PlacementResult
    {
        public bool CanPlace;
        public Vector3 FinalPosition;
        public Quaternion FinalRotation;
        public List<string> ValidationErrors;
    }
    
    private PlacementMode currentMode = PlacementMode.GridSnap;
    private float gridSize = 1.0f;
    
    public PlacementResult ValidatePlacement(
        BuildingPiece piece,
        Vector3 position,
        Quaternion rotation)
    {
        var result = new PlacementResult
        {
            FinalPosition = position,
            FinalRotation = rotation,
            ValidationErrors = new List<string>()
        };
        
        // Apply placement mode
        switch (currentMode)
        {
            case PlacementMode.GridSnap:
                result.FinalPosition = SnapToGrid(position, gridSize);
                result.FinalRotation = SnapRotation(rotation, 45f);
                break;
                
            case PlacementMode.FoundationSnap:
                var snap = FindNearestSnapPoint(position);
                if (snap.HasValue)
                {
                    result.FinalPosition = snap.Value.Position;
                    result.FinalRotation = snap.Value.Rotation;
                }
                break;
        }
        
        // Validation checks
        if (IsOverlapping(piece, result.FinalPosition))
        {
            result.CanPlace = false;
            result.ValidationErrors.Add("Overlaps existing structure");
        }
        
        if (!HasSupportingFoundation(piece, result.FinalPosition))
        {
            result.CanPlace = false;
            result.ValidationErrors.Add("Requires foundation");
        }
        
        if (!IsOnValidTerrain(piece, result.FinalPosition))
        {
            result.CanPlace = false;
            result.ValidationErrors.Add("Invalid terrain");
        }
        
        result.CanPlace = result.ValidationErrors.Count == 0;
        return result;
    }
    
    private Vector3 SnapToGrid(Vector3 position, float gridSize)
    {
        return new Vector3(
            Mathf.Round(position.x / gridSize) * gridSize,
            Mathf.Round(position.y / gridSize) * gridSize,
            Mathf.Round(position.z / gridSize) * gridSize
        );
    }
    
    private Quaternion SnapRotation(Quaternion rotation, float snapAngle)
    {
        Vector3 euler = rotation.eulerAngles;
        euler.y = Mathf.Round(euler.y / snapAngle) * snapAngle;
        return Quaternion.Euler(euler);
    }
}
```

#### 1.2 Snap Points and Connection System

```csharp
public class SnapPointSystem
{
    public struct SnapPoint
    {
        public Vector3 LocalPosition;
        public Vector3 LocalNormal;
        public SnapType Type;
        public float SnapRadius;
    }
    
    public enum SnapType
    {
        Wall,
        Floor,
        Ceiling,
        Corner,
        Edge
    }
    
    public List<SnapPoint> GetSnapPoints(BuildingPiece piece)
    {
        var snapPoints = new List<SnapPoint>();
        
        // Foundation piece
        if (piece.Type == PieceType.Foundation)
        {
            // Edges for connecting adjacent foundations
            snapPoints.Add(new SnapPoint
            {
                LocalPosition = new Vector3(piece.Width / 2, 0, 0),
                LocalNormal = Vector3.right,
                Type = SnapType.Edge,
                SnapRadius = 0.5f
            });
            
            // Top surface for building walls
            snapPoints.Add(new SnapPoint
            {
                LocalPosition = new Vector3(0, piece.Height, 0),
                LocalNormal = Vector3.up,
                Type = SnapType.Floor,
                SnapRadius = 0.5f
            });
        }
        
        // Wall piece
        if (piece.Type == PieceType.Wall)
        {
            // Top for ceiling/next floor
            snapPoints.Add(new SnapPoint
            {
                LocalPosition = new Vector3(0, piece.Height, 0),
                LocalNormal = Vector3.up,
                Type = SnapType.Wall,
                SnapRadius = 0.3f
            });
            
            // Sides for adjacent walls
            snapPoints.Add(new SnapPoint
            {
                LocalPosition = new Vector3(piece.Width / 2, piece.Height / 2, 0),
                LocalNormal = Vector3.right,
                Type = SnapType.Edge,
                SnapRadius = 0.3f
            });
        }
        
        return snapPoints;
    }
    
    public SnapPoint? FindNearestSnapPoint(
        Vector3 worldPosition,
        List<BuildingPiece> nearbyPieces)
    {
        float minDistance = float.MaxValue;
        SnapPoint? nearest = null;
        
        foreach (var piece in nearbyPieces)
        {
            var snapPoints = GetSnapPoints(piece);
            
            foreach (var snap in snapPoints)
            {
                Vector3 worldSnapPos = piece.Transform.TransformPoint(snap.LocalPosition);
                float distance = Vector3.Distance(worldPosition, worldSnapPos);
                
                if (distance < snap.SnapRadius && distance < minDistance)
                {
                    minDistance = distance;
                    nearest = new SnapPoint
                    {
                        LocalPosition = worldSnapPos,
                        LocalNormal = piece.Transform.TransformDirection(snap.LocalNormal),
                        Type = snap.Type,
                        SnapRadius = snap.SnapRadius
                    };
                }
            }
        }
        
        return nearest;
    }
}
```

### 2. Structural Integrity

```csharp
public class StructuralIntegritySystem
{
    public struct StructuralNode
    {
        public BuildingPiece Piece;
        public float SupportStrength;  // 0-1
        public List<StructuralNode> SupportedBy;
        public float TotalWeight;
    }
    
    public float CalculateIntegrity(BuildingPiece piece)
    {
        // Ground-connected pieces have 100% integrity
        if (IsGroundConnected(piece))
            return 1.0f;
        
        // Calculate based on support chain
        var supports = FindSupportingPieces(piece);
        
        if (supports.Count == 0)
            return 0f;  // No support = collapse
        
        // Average integrity of supports, with decay
        float avgSupport = 0f;
        foreach (var support in supports)
        {
            float supportIntegrity = CalculateIntegrity(support);
            avgSupport += supportIntegrity;
        }
        avgSupport /= supports.Count;
        
        // Decay based on distance from ground
        float decay = 0.9f;  // 10% loss per level
        return avgSupport * decay;
    }
    
    public bool WillCollapse(BuildingPiece piece)
    {
        float integrity = CalculateIntegrity(piece);
        return integrity < 0.2f;  // Collapse threshold
    }
    
    public void UpdateStructure(BuildingPiece piece)
    {
        // Recalculate integrity for piece and everything it supports
        var integrity = CalculateIntegrity(piece);
        piece.CurrentIntegrity = integrity;
        
        // Check for collapse
        if (WillCollapse(piece))
        {
            CollapsePiece(piece);
            
            // Cascade to supported pieces
            var dependent = FindDependentPieces(piece);
            foreach (var dep in dependent)
            {
                UpdateStructure(dep);
            }
        }
    }
    
    private List<BuildingPiece> FindSupportingPieces(BuildingPiece piece)
    {
        var supports = new List<BuildingPiece>();
        
        // Check pieces below or adjacent that provide support
        var nearby = GetNearbyPieces(piece.Position, 5f);
        
        foreach (var other in nearby)
        {
            if (IsSupporting(other, piece))
            {
                supports.Add(other);
            }
        }
        
        return supports;
    }
    
    private bool IsSupporting(BuildingPiece supporter, BuildingPiece supported)
    {
        // Supporter must be below or adjacent with contact
        Vector3 toSupported = supported.Position - supporter.Position;
        
        // Vertical support (foundations, pillars)
        if (toSupported.y > 0 && toSupported.y < 3f &&
            Vector2.Distance(
                new Vector2(supporter.Position.x, supporter.Position.z),
                new Vector2(supported.Position.x, supported.Position.z)
            ) < 2f)
        {
            return true;
        }
        
        // Horizontal support (beams connecting)
        if (Mathf.Abs(toSupported.y) < 0.5f &&
            Vector3.Distance(supporter.Position, supported.Position) < 3f)
        {
            return true;
        }
        
        return false;
    }
}
```

### 3. Building Progression

```csharp
public class BuildingTierSystem
{
    public enum BuildingTier
    {
        Primitive,    // Sticks and leaves
        Basic,        // Wood and thatch
        Intermediate, // Stone and brick
        Advanced      // Metal and concrete
    }
    
    public struct TierRequirements
    {
        public BuildingTier Tier;
        public List<ItemType> RequiredMaterials;
        public List<ToolType> RequiredTools;
        public float Durability;
        public float StructuralStrength;
    }
    
    public Dictionary<BuildingTier, TierRequirements> TierData = 
        new Dictionary<BuildingTier, TierRequirements>
    {
        [BuildingTier.Primitive] = new TierRequirements
        {
            Tier = BuildingTier.Primitive,
            RequiredMaterials = new List<ItemType> { ItemType.Sticks, ItemType.Leaves },
            RequiredTools = new List<ToolType>(),
            Durability = 100f,
            StructuralStrength = 0.3f
        },
        [BuildingTier.Basic] = new TierRequirements
        {
            Tier = BuildingTier.Basic,
            RequiredMaterials = new List<ItemType> { ItemType.Wood, ItemType.Fiber },
            RequiredTools = new List<ToolType> { ToolType.Axe },
            Durability = 500f,
            StructuralStrength = 0.6f
        },
        [BuildingTier.Intermediate] = new TierRequirements
        {
            Tier = BuildingTier.Intermediate,
            RequiredMaterials = new List<ItemType> { ItemType.Stone, ItemType.Mortar },
            RequiredTools = new List<ToolType> { ToolType.Hammer, ToolType.Chisel },
            Durability = 2000f,
            StructuralStrength = 0.9f
        },
        [BuildingTier.Advanced] = new TierRequirements
        {
            Tier = BuildingTier.Advanced,
            RequiredMaterials = new List<ItemType> { ItemType.Metal, ItemType.Concrete },
            RequiredTools = new List<ToolType> { ToolType.Welding },
            Durability = 5000f,
            StructuralStrength = 1.0f
        }
    };
    
    public bool CanUpgrade(BuildingPiece piece, BuildingTier newTier)
    {
        if (newTier <= piece.CurrentTier)
            return false;
        
        var requirements = TierData[newTier];
        
        // Check player has materials and tools
        return PlayerHasMaterials(requirements.RequiredMaterials) &&
               PlayerHasTools(requirements.RequiredTools);
    }
    
    public void UpgradePiece(BuildingPiece piece, BuildingTier newTier)
    {
        var requirements = TierData[newTier];
        
        // Consume materials
        ConsumeMaterials(requirements.RequiredMaterials);
        
        // Update piece
        piece.CurrentTier = newTier;
        piece.MaxDurability = requirements.Durability;
        piece.CurrentDurability = requirements.Durability;
        piece.StructuralStrength = requirements.StructuralStrength;
        
        // Update visuals
        UpdatePieceAppearance(piece);
        
        // Recalculate structure
        RecalculateStructure(piece);
    }
}
```

### 4. Multiplayer and Permissions

```csharp
public class BuildingPermissionSystem
{
    public enum PermissionLevel
    {
        None,
        View,
        Use,
        Build,
        Admin
    }
    
    public struct BasePermissions
    {
        public Player Owner;
        public Dictionary<Player, PermissionLevel> PlayerPermissions;
        public Dictionary<string, PermissionLevel> ClanPermissions;
        public PermissionLevel DefaultPermission;
    }
    
    private Dictionary<BuildingBase, BasePermissions> basePermissions;
    
    public bool CanInteract(Player player, BuildingPiece piece, ActionType action)
    {
        var baseObj = FindParentBase(piece);
        if (baseObj == null)
            return true;  // No owner = public
        
        var permissions = basePermissions[baseObj];
        
        // Owner can do anything
        if (permissions.Owner == player)
            return true;
        
        // Check player-specific permission
        if (permissions.PlayerPermissions.TryGetValue(player, out var level))
        {
            return HasPermission(level, action);
        }
        
        // Check clan permission
        if (player.Clan != null &&
            permissions.ClanPermissions.TryGetValue(player.Clan.Id, out var clanLevel))
        {
            return HasPermission(clanLevel, action);
        }
        
        // Check default permission
        return HasPermission(permissions.DefaultPermission, action);
    }
    
    private bool HasPermission(PermissionLevel level, ActionType action)
    {
        return action switch
        {
            ActionType.View => level >= PermissionLevel.View,
            ActionType.OpenDoor => level >= PermissionLevel.Use,
            ActionType.OpenStorage => level >= PermissionLevel.Use,
            ActionType.Build => level >= PermissionLevel.Build,
            ActionType.Demolish => level >= PermissionLevel.Admin,
            ActionType.ChangePermissions => level >= PermissionLevel.Admin,
            _ => false
        };
    }
    
    public void SetPermission(
        BuildingBase baseObj,
        Player target,
        PermissionLevel level)
    {
        if (!basePermissions.TryGetValue(baseObj, out var perms))
            return;
        
        perms.PlayerPermissions[target] = level;
        
        // Notify player
        NotifyPermissionChange(target, baseObj, level);
    }
}
```

### 5. Decay and Maintenance

```csharp
public class BuildingDecaySystem
{
    public struct DecaySettings
    {
        public float BaseDecayRate;        // Per day
        public float WeatherMultiplier;
        public float AbandonedMultiplier;  // Faster decay if no owner nearby
        public float MaintenanceCost;      // Materials to repair
    }
    
    public void UpdateDecay(BuildingPiece piece, float deltaTime)
    {
        var settings = GetDecaySettings(piece.CurrentTier);
        
        // Calculate decay rate
        float decayRate = settings.BaseDecayRate;
        
        // Weather affects decay
        float weatherMult = GetWeatherMultiplier(piece.Position);
        decayRate *= weatherMult;
        
        // Abandoned bases decay faster
        if (IsAbandoned(piece))
        {
            decayRate *= settings.AbandonedMultiplier;
        }
        
        // Apply decay
        float decayAmount = decayRate * deltaTime;
        piece.CurrentDurability -= decayAmount;
        
        // Check for collapse
        if (piece.CurrentDurability <= 0)
        {
            CollapsePiece(piece);
        }
        else if (piece.CurrentDurability < piece.MaxDurability * 0.3f)
        {
            // Visual indication of poor condition
            piece.ShowDamageEffects = true;
        }
    }
    
    public bool Repair(BuildingPiece piece, Player player)
    {
        var settings = GetDecaySettings(piece.CurrentTier);
        
        // Check player has materials
        if (!player.Inventory.HasMaterials(settings.MaintenanceCost))
            return false;
        
        // Consume materials
        player.Inventory.ConsumeMaterials(settings.MaintenanceCost);
        
        // Restore durability
        piece.CurrentDurability = piece.MaxDurability;
        piece.ShowDamageEffects = false;
        
        return true;
    }
    
    private bool IsAbandoned(BuildingPiece piece)
    {
        var owner = FindOwner(piece);
        if (owner == null)
            return true;
        
        // Check if owner has been nearby recently
        return owner.LastSeenInArea(piece.Position, 100f) > TimeSpan.FromDays(7);
    }
}
```

---

## BlueMarble-Specific Recommendations

### 1. Web-Based Building Interface

```typescript
// React component for building mode
interface BuildingModeProps {
    selectedPiece: BuildingPieceType;
    placementMode: PlacementMode;
    onPlace: (position: Vector3, rotation: Quaternion) => void;
}

const BuildingMode: React.FC<BuildingModeProps> = ({
    selectedPiece,
    placementMode,
    onPlace
}) => {
    const [previewPosition, setPreviewPosition] = useState<Vector3>(null);
    const [canPlace, setCanPlace] = useState(false);
    const [errors, setErrors] = useState<string[]>([]);
    
    useEffect(() => {
        // Update preview position on mouse move
        const handleMouseMove = (e: MouseEvent) => {
            const ray = camera.screenToWorldRay(e.clientX, e.clientY);
            const hit = raycast(ray);
            
            if (hit) {
                const result = validatePlacement(
                    selectedPiece,
                    hit.point,
                    currentRotation
                );
                
                setPreviewPosition(result.finalPosition);
                setCanPlace(result.canPlace);
                setErrors(result.errors);
            }
        };
        
        window.addEventListener('mousemove', handleMouseMove);
        return () => window.removeEventListener('mousemove', handleMouseMove);
    }, [selectedPiece]);
    
    const handleClick = () => {
        if (canPlace && previewPosition) {
            onPlace(previewPosition, currentRotation);
        }
    };
    
    return (
        <div className="building-mode">
            <BuildingPreview
                piece={selectedPiece}
                position={previewPosition}
                canPlace={canPlace}
            />
            {!canPlace && (
                <div className="placement-errors">
                    {errors.map(err => <div key={err}>{err}</div>)}
                </div>
            )}
            <BuildingControls
                onRotate={() => rotate(45)}
                onModeChange={setPlacementMode}
            />
        </div>
    );
};
```

### 2. Server-Side Validation

```csharp
public class ServerBuildingValidator
{
    public BuildingValidationResult ValidateServerSide(
        Player player,
        BuildingPiece piece,
        Vector3 position)
    {
        var result = new BuildingValidationResult();
        
        // Anti-cheat: verify player is close enough
        if (Vector3.Distance(player.Position, position) > 10f)
        {
            result.IsValid = false;
            result.Reason = "Too far from building location";
            return result;
        }
        
        // Verify player has materials
        if (!player.Inventory.HasMaterials(piece.RequiredMaterials))
        {
            result.IsValid = false;
            result.Reason = "Insufficient materials";
            return result;
        }
        
        // Verify placement is valid (server-side check)
        var placementResult = ValidatePlacement(piece, position);
        if (!placementResult.CanPlace)
        {
            result.IsValid = false;
            result.Reason = string.Join(", ", placementResult.ValidationErrors);
            return result;
        }
        
        // Anti-grief: check building limits
        if (ExceedsBuildingLimits(player))
        {
            result.IsValid = false;
            result.Reason = "Building limit reached";
            return result;
        }
        
        result.IsValid = true;
        return result;
    }
}
```

---

## Implementation Roadmap

### Phase 1: Core System (Week 1-2)
1. Basic placement with grid snapping
2. Simple building pieces (walls, floors, roofs)
3. Material requirements
4. Visual previews

### Phase 2: Structural System (Week 3)
1. Integrity calculations
2. Support chain validation
3. Collapse mechanics
4. Foundation requirements

### Phase 3: Progression (Week 4)
1. Building tiers
2. Upgrade system
3. Decay and maintenance
4. Repair mechanics

### Phase 4: Multiplayer (Week 5)
1. Permission system
2. Server validation
3. Anti-griefing measures
4. Clan building

### Phase 5: Polish (Week 6)
1. Advanced placement modes
2. Blueprints and templates
3. Building challenges/achievements
4. Tutorial system

---

## References and Cross-Links

### Related Research
- `survival-analysis-historical-building-techniques.md` - Construction methods
- `survival-analysis-primitive-tools-technology.md` - Tool requirements
- `survival-analysis-inventory-management-ui-ux.md` - Material management

### External Resources
- Rust, Ark, Valheim - Building system examples
- Minecraft - Creative building inspiration
- Conan Exiles - Structural integrity system
- Building physics and engineering basics

---

**Document Status:** Complete  
**Last Updated:** 2025-01-17  
**Next Steps:** Implement core placement and basic pieces  
**Related Issues:** Phase 2 Group 05 research assignment
