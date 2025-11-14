using System.Numerics;

namespace BlueMarble.SpatialData;

/// <summary>
/// Tectonic deformation process implementation using Template Method pattern
/// Handles material displacement with medium locality (65%)
/// 
/// Layer responsibility: Tectonic movement-specific logic only
/// </summary>
public class TectonicDeformationProcess : GeologicalProcessBase
{
    public TectonicDeformationProcess(DeltaPatchOctree deltaOctree) : base(deltaOctree)
    {
    }

    protected override string GetProcessName() => "Tectonic Deformation";

    public override double GetSpatialLocalityFactor() => 0.65; // Medium locality

    protected override bool ValidateContext(GeologicalProcessContext context)
    {
        return base.ValidateContext(context) &&
               context.Displacements != null &&
               context.Displacements.Any();
    }

    /// <summary>
    /// Determine affected positions from displacement data
    /// </summary>
    protected override IEnumerable<Vector3> DetermineAffectedRegion(GeologicalProcessContext context)
    {
        // Both source and destination positions are affected
        var positions = new HashSet<Vector3>();
        
        foreach (var (from, to) in context.Displacements!)
        {
            positions.Add(from);
            positions.Add(to);
        }

        return positions;
    }

    /// <summary>
    /// Calculate tectonic displacement effects
    /// Move material from source to destination positions
    /// </summary>
    protected override IEnumerable<(Vector3 position, MaterialData material)> CalculateMaterialChanges(
        IEnumerable<Vector3> positions,
        GeologicalProcessContext context)
    {
        var updates = new List<(Vector3, MaterialData)>();

        foreach (var (from, to) in context.Displacements!)
        {
            var material = GetCurrentMaterial(from);

            // Clear source position
            updates.Add((from, MaterialData.DefaultAir));
            
            // Place at destination
            updates.Add((to, material));
        }

        return updates;
    }

    /// <summary>
    /// Post-process to handle collision detection
    /// If material already exists at destination, could implement stacking logic
    /// </summary>
    protected override void PostProcess(
        GeologicalProcessContext context,
        IEnumerable<(Vector3 position, MaterialData material)> appliedUpdates)
    {
        // Could add collision/stacking logic here
        // For now, simple replacement behavior from CalculateMaterialChanges is sufficient
    }
}
