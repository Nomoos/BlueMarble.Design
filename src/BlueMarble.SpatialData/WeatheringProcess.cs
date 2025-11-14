using System.Numerics;

namespace BlueMarble.SpatialData;

/// <summary>
/// Weathering process implementation using Template Method pattern
/// Handles gradual material transformation with high surface locality
/// 
/// Layer responsibility: Weathering-specific logic only
/// </summary>
public class WeatheringProcess : GeologicalProcessBase
{
    public WeatheringProcess(DeltaPatchOctree deltaOctree) : base(deltaOctree)
    {
    }

    protected override string GetProcessName() => "Weathering";

    public override double GetSpatialLocalityFactor() => 0.85; // High locality along surfaces

    protected override bool ValidateContext(GeologicalProcessContext context)
    {
        return base.ValidateContext(context) &&
               context.Positions != null &&
               context.TargetMaterial.HasValue;
    }

    /// <summary>
    /// Filter to only surface positions (positions with air neighbors)
    /// </summary>
    protected override IEnumerable<Vector3> FilterPositions(
        IEnumerable<Vector3> positions,
        GeologicalProcessContext context)
    {
        return positions.Where(pos => IsSurfacePosition(pos));
    }

    /// <summary>
    /// Calculate weathering transformation
    /// Gradual conversion to weathered material type
    /// </summary>
    protected override IEnumerable<(Vector3 position, MaterialData material)> CalculateMaterialChanges(
        IEnumerable<Vector3> positions,
        GeologicalProcessContext context)
    {
        var weatheredMaterial = context.TargetMaterial!.Value;
        var intensity = context.Intensity;

        var updates = new List<(Vector3, MaterialData)>();

        foreach (var position in positions)
        {
            var currentMaterial = GetCurrentMaterial(position);
            
            // Gradual transformation based on intensity
            if (intensity >= 1.0f)
            {
                // Complete weathering
                updates.Add((position, weatheredMaterial));
            }
            else
            {
                // Partial weathering - blend properties
                var blendedMaterial = BlendMaterials(currentMaterial, weatheredMaterial, intensity);
                updates.Add((position, blendedMaterial));
            }
        }

        return updates;
    }

    /// <summary>
    /// Check if position is at surface (has at least one air neighbor)
    /// </summary>
    private bool IsSurfacePosition(Vector3 position)
    {
        var offsets = new[]
        {
            new Vector3(1, 0, 0), new Vector3(-1, 0, 0),
            new Vector3(0, 1, 0), new Vector3(0, -1, 0),
            new Vector3(0, 0, 1), new Vector3(0, 0, -1)
        };

        foreach (var offset in offsets)
        {
            var neighbor = position + offset;
            var neighborMaterial = GetCurrentMaterial(neighbor);
            if (neighborMaterial.MaterialType == MaterialId.Air)
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Blend two materials based on intensity factor
    /// </summary>
    private MaterialData BlendMaterials(MaterialData from, MaterialData to, float factor)
    {
        return new MaterialData(
            to.MaterialType,
            from.Density * (1 - factor) + to.Density * factor,
            from.Hardness * (1 - factor) + to.Hardness * factor,
            to.Properties
        );
    }
}
