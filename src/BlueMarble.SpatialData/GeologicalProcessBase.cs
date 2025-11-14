using System.Numerics;

namespace BlueMarble.SpatialData;

/// <summary>
/// Base class implementing Template Method pattern for geological processes
/// Defines the skeleton of geological modification algorithms while allowing
/// subclasses to override specific steps
/// 
/// This follows the separation of concerns principle by:
/// 1. Centralizing common workflow logic in base class
/// 2. Providing extension points for process-specific behavior
/// 3. Enforcing consistent error handling and validation
/// </summary>
public abstract class GeologicalProcessBase
{
    protected readonly DeltaPatchOctree DeltaOctree;
    
    protected GeologicalProcessBase(DeltaPatchOctree deltaOctree)
    {
        DeltaOctree = deltaOctree ?? throw new ArgumentNullException(nameof(deltaOctree));
    }

    /// <summary>
    /// Template method defining the standard workflow for geological processes
    /// This is the invariant algorithm structure that all processes follow
    /// </summary>
    public void Execute(GeologicalProcessContext context)
    {
        // Step 1: Validate input parameters (common across all processes)
        if (!ValidateContext(context))
        {
            throw new ArgumentException($"Invalid context for {GetProcessName()}", nameof(context));
        }

        // Step 2: Prepare affected region (hook method - can be overridden)
        var affectedPositions = DetermineAffectedRegion(context);
        
        // Step 3: Filter positions (hook method - some processes may skip certain areas)
        affectedPositions = FilterPositions(affectedPositions, context);

        // Step 4: Calculate material changes (abstract - must be implemented by subclasses)
        var materialUpdates = CalculateMaterialChanges(affectedPositions, context);

        // Step 5: Validate material changes before applying
        materialUpdates = ValidateMaterialUpdates(materialUpdates);

        // Step 6: Apply changes to octree (common across all processes)
        ApplyChanges(materialUpdates);

        // Step 7: Post-process (hook method - optional cleanup or notifications)
        PostProcess(context, materialUpdates);
    }

    /// <summary>
    /// Validate the geological process context
    /// Override to add process-specific validation
    /// </summary>
    protected virtual bool ValidateContext(GeologicalProcessContext context)
    {
        return context != null && context.IsValid();
    }

    /// <summary>
    /// Determine the region affected by this geological process
    /// This is a hook method that provides default behavior but can be overridden
    /// </summary>
    protected virtual IEnumerable<Vector3> DetermineAffectedRegion(GeologicalProcessContext context)
    {
        // Default implementation: use positions from context
        return context.Positions ?? Enumerable.Empty<Vector3>();
    }

    /// <summary>
    /// Filter positions based on process-specific criteria
    /// Hook method with default pass-through behavior
    /// </summary>
    protected virtual IEnumerable<Vector3> FilterPositions(
        IEnumerable<Vector3> positions, 
        GeologicalProcessContext context)
    {
        // Default: no filtering
        return positions;
    }

    /// <summary>
    /// Calculate material changes for each position
    /// Abstract method - MUST be implemented by each concrete process
    /// This is the core process-specific logic
    /// </summary>
    protected abstract IEnumerable<(Vector3 position, MaterialData material)> CalculateMaterialChanges(
        IEnumerable<Vector3> positions,
        GeologicalProcessContext context);

    /// <summary>
    /// Validate material updates before applying
    /// Hook method for quality checks
    /// </summary>
    protected virtual IEnumerable<(Vector3 position, MaterialData material)> ValidateMaterialUpdates(
        IEnumerable<(Vector3 position, MaterialData material)> updates)
    {
        // Default: ensure no invalid materials
        return updates.Where(u => IsValidMaterial(u.material));
    }

    /// <summary>
    /// Check if a material is valid
    /// Can be overridden for process-specific material constraints
    /// </summary>
    protected virtual bool IsValidMaterial(MaterialData material)
    {
        return material.Density >= 0 && material.Hardness >= 0 && material.Hardness <= 10;
    }

    /// <summary>
    /// Apply material changes to the octree
    /// Common implementation used by all processes
    /// </summary>
    protected virtual void ApplyChanges(IEnumerable<(Vector3 position, MaterialData material)> updates)
    {
        DeltaOctree.WriteMaterialBatch(updates);
    }

    /// <summary>
    /// Post-processing step after changes are applied
    /// Hook method for cleanup or notifications
    /// </summary>
    protected virtual void PostProcess(
        GeologicalProcessContext context,
        IEnumerable<(Vector3 position, MaterialData material)> appliedUpdates)
    {
        // Default: no post-processing
    }

    /// <summary>
    /// Get the name of this geological process (for logging/debugging)
    /// </summary>
    protected abstract string GetProcessName();

    /// <summary>
    /// Get current material at position (utility method for subclasses)
    /// </summary>
    protected MaterialData GetCurrentMaterial(Vector3 position)
    {
        return DeltaOctree.ReadVoxel(position);
    }

    /// <summary>
    /// Get spatial locality metric for this process type
    /// Used for optimization strategies
    /// </summary>
    public abstract double GetSpatialLocalityFactor();
}

/// <summary>
/// Context object containing parameters for geological process execution
/// Follows the Parameter Object pattern to avoid long parameter lists
/// </summary>
public class GeologicalProcessContext
{
    public IEnumerable<Vector3>? Positions { get; set; }
    public Vector3? Center { get; set; }
    public float Radius { get; set; }
    public float Intensity { get; set; }
    public MaterialData? TargetMaterial { get; set; }
    public IEnumerable<(Vector3 from, Vector3 to)>? Displacements { get; set; }
    public Dictionary<string, object>? AdditionalParameters { get; set; }

    public bool IsValid()
    {
        return Intensity >= 0 && Intensity <= 1.0f && Radius >= 0;
    }
}
