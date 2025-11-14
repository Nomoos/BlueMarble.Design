using System.Numerics;

namespace BlueMarble.SpatialData;

/// <summary>
/// Deposition process implementation using Template Method pattern
/// Handles material accumulation with medium spatial locality
/// 
/// Layer responsibility: Deposition-specific logic only
/// </summary>
public class DepositionProcess : GeologicalProcessBase
{
    public DepositionProcess(DeltaPatchOctree deltaOctree) : base(deltaOctree)
    {
    }

    protected override string GetProcessName() => "Deposition";

    public override double GetSpatialLocalityFactor() => 0.65; // Medium locality

    protected override bool ValidateContext(GeologicalProcessContext context)
    {
        return base.ValidateContext(context) &&
               context.Positions != null &&
               context.TargetMaterial.HasValue;
    }

    /// <summary>
    /// Calculate deposition effects - layering algorithm
    /// </summary>
    protected override IEnumerable<(Vector3 position, MaterialData material)> CalculateMaterialChanges(
        IEnumerable<Vector3> positions,
        GeologicalProcessContext context)
    {
        var updates = new List<(Vector3, MaterialData)>();
        var depositMaterial = context.TargetMaterial!.Value;

        foreach (var position in positions)
        {
            var currentMaterial = GetCurrentMaterial(position);

            // If air, directly place deposit
            if (currentMaterial.MaterialType == MaterialId.Air)
            {
                updates.Add((position, depositMaterial));
            }
            else
            {
                // Layer on top - could implement mixing logic here
                // For now, replace with deposit material
                updates.Add((position, depositMaterial));
            }
        }

        return updates;
    }
}
