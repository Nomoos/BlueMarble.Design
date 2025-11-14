using System.Numerics;

namespace BlueMarble.SpatialData;

/// <summary>
/// Erosion process implementation using Template Method pattern
/// Handles surface material removal with high spatial locality (85%)
/// 
/// Layer responsibility: Implements erosion-specific logic only
/// Does not directly manipulate low-level octree - uses base class methods
/// </summary>
public class ErosionProcess : GeologicalProcessBase
{
    public ErosionProcess(DeltaPatchOctree deltaOctree) : base(deltaOctree)
    {
    }

    protected override string GetProcessName() => "Erosion";

    public override double GetSpatialLocalityFactor() => 0.85; // 85% spatial locality

    /// <summary>
    /// Validate that erosion context has required data
    /// </summary>
    protected override bool ValidateContext(GeologicalProcessContext context)
    {
        return base.ValidateContext(context) && 
               context.Positions != null && 
               context.Intensity >= 0;
    }

    /// <summary>
    /// Filter out positions that are already air (cannot erode air)
    /// This demonstrates layer-specific filtering logic
    /// </summary>
    protected override IEnumerable<Vector3> FilterPositions(
        IEnumerable<Vector3> positions,
        GeologicalProcessContext context)
    {
        return positions.Where(pos =>
        {
            var material = GetCurrentMaterial(pos);
            return material.MaterialType != MaterialId.Air;
        });
    }

    /// <summary>
    /// Calculate erosion effects - core erosion algorithm
    /// Encapsulates erosion-specific transformation logic
    /// </summary>
    protected override IEnumerable<(Vector3 position, MaterialData material)> CalculateMaterialChanges(
        IEnumerable<Vector3> positions,
        GeologicalProcessContext context)
    {
        var updates = new List<(Vector3, MaterialData)>();
        var erosionRate = context.Intensity;

        foreach (var position in positions)
        {
            var currentMaterial = GetCurrentMaterial(position);

            if (erosionRate >= 1.0f)
            {
                // Complete erosion - convert to air
                updates.Add((position, MaterialData.DefaultAir));
            }
            else
            {
                // Partial erosion - reduce density
                var erodedMaterial = currentMaterial;
                erodedMaterial.Density *= (1.0f - erosionRate);
                
                // If density becomes too low, convert to air
                if (erodedMaterial.Density < 10f)
                {
                    updates.Add((position, MaterialData.DefaultAir));
                }
                else
                {
                    updates.Add((position, erodedMaterial));
                }
            }
        }

        return updates;
    }
}
