using System.Numerics;

namespace BlueMarble.SpatialData;

/// <summary>
/// Volcanic intrusion process implementation using Template Method pattern
/// Handles magma/lava injection with high spatial clustering (90%)
/// 
/// Layer responsibility: Volcanic activity-specific logic only
/// </summary>
public class VolcanicIntrusionProcess : GeologicalProcessBase
{
    public VolcanicIntrusionProcess(DeltaPatchOctree deltaOctree) : base(deltaOctree)
    {
    }

    protected override string GetProcessName() => "Volcanic Intrusion";

    public override double GetSpatialLocalityFactor() => 0.90; // High clustering

    protected override bool ValidateContext(GeologicalProcessContext context)
    {
        return base.ValidateContext(context) &&
               context.Center.HasValue &&
               context.Radius > 0 &&
               context.TargetMaterial.HasValue;
    }

    /// <summary>
    /// Determine affected region as sphere around center point
    /// Overrides base implementation to use spherical region
    /// </summary>
    protected override IEnumerable<Vector3> DetermineAffectedRegion(GeologicalProcessContext context)
    {
        var center = context.Center!.Value;
        var radius = context.Radius;
        var positions = new List<Vector3>();
        
        var radiusSquared = radius * radius;
        var minBound = new Vector3(center.X - radius, center.Y - radius, center.Z - radius);
        var maxBound = new Vector3(center.X + radius, center.Y + radius, center.Z + radius);

        for (float x = MathF.Floor(minBound.X); x <= MathF.Ceiling(maxBound.X); x++)
        {
            for (float y = MathF.Floor(minBound.Y); y <= MathF.Ceiling(maxBound.Y); y++)
            {
                for (float z = MathF.Floor(minBound.Z); z <= MathF.Ceiling(maxBound.Z); z++)
                {
                    var position = new Vector3(x, y, z);
                    var distanceSquared = Vector3.DistanceSquared(position, center);

                    if (distanceSquared <= radiusSquared)
                    {
                        positions.Add(position);
                    }
                }
            }
        }

        return positions;
    }

    /// <summary>
    /// Calculate volcanic material placement
    /// All positions within sphere receive volcanic material
    /// </summary>
    protected override IEnumerable<(Vector3 position, MaterialData material)> CalculateMaterialChanges(
        IEnumerable<Vector3> positions,
        GeologicalProcessContext context)
    {
        var volcanicMaterial = context.TargetMaterial!.Value;
        return positions.Select(pos => (pos, volcanicMaterial));
    }

    /// <summary>
    /// Validate that volcanic materials are appropriate
    /// </summary>
    protected override bool IsValidMaterial(MaterialData material)
    {
        return base.IsValidMaterial(material) &&
               (material.MaterialType == MaterialId.Lava ||
                material.MaterialType == MaterialId.Magma ||
                material.MaterialType == MaterialId.Basalt);
    }
}
