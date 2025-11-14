using System.Numerics;

namespace BlueMarble.SpatialData;

/// <summary>
/// Adapter for integrating geological processes with delta overlay system
/// Refactored to use layered architecture with Template Method pattern
/// 
/// This class now acts as the orchestration layer, delegating to specialized
/// process implementations rather than containing all logic directly.
/// Follows "No Layer Skipping" principle - communicates with process layer,
/// which in turn communicates with storage layer.
/// </summary>
public class GeomorphologicalOctreeAdapter
{
    private readonly DeltaPatchOctree _deltaOctree;
    
    // Process layer instances - using composition pattern
    private readonly ErosionProcess _erosionProcess;
    private readonly DepositionProcess _depositionProcess;
    private readonly VolcanicIntrusionProcess _volcanicProcess;
    private readonly TectonicDeformationProcess _tectonicProcess;
    private readonly WeatheringProcess _weatheringProcess;

    public GeomorphologicalOctreeAdapter(DeltaPatchOctree deltaOctree)
    {
        _deltaOctree = deltaOctree ?? throw new ArgumentNullException(nameof(deltaOctree));
        
        // Initialize process layer components
        _erosionProcess = new ErosionProcess(deltaOctree);
        _depositionProcess = new DepositionProcess(deltaOctree);
        _volcanicProcess = new VolcanicIntrusionProcess(deltaOctree);
        _tectonicProcess = new TectonicDeformationProcess(deltaOctree);
        _weatheringProcess = new WeatheringProcess(deltaOctree);
    }

    /// <summary>
    /// Apply erosion process - removes material from surface
    /// Erosion typically has high spatial locality (85%)
    /// 
    /// Refactored to delegate to specialized ErosionProcess layer
    /// Maintains backward compatibility with existing API
    /// </summary>
    public void ApplyErosion(IEnumerable<Vector3> positions, float erosionRate)
    {
        var context = new GeologicalProcessContext
        {
            Positions = positions,
            Intensity = erosionRate
        };

        _erosionProcess.Execute(context);
    }

    /// <summary>
    /// Apply deposition process - adds material to surface
    /// Deposition has medium spatial locality
    /// 
    /// Refactored to delegate to specialized DepositionProcess layer
    /// </summary>
    public void ApplyDeposition(IEnumerable<(Vector3 position, MaterialData depositMaterial)> depositions)
    {
        // Extract positions and use first material as target
        var depositionList = depositions.ToList();
        if (!depositionList.Any())
            return;

        var positions = depositionList.Select(d => d.position);
        var targetMaterial = depositionList.First().depositMaterial;

        var context = new GeologicalProcessContext
        {
            Positions = positions,
            TargetMaterial = targetMaterial
        };

        _depositionProcess.Execute(context);
    }

    /// <summary>
    /// Apply volcanic intrusion - magma/lava injection
    /// Volcanic activity has high spatial clustering
    /// 
    /// Refactored to delegate to specialized VolcanicIntrusionProcess layer
    /// </summary>
    public void ApplyVolcanicIntrusion(Vector3 center, float radius, MaterialData volcanicMaterial)
    {
        var context = new GeologicalProcessContext
        {
            Center = center,
            Radius = radius,
            TargetMaterial = volcanicMaterial
        };

        _volcanicProcess.Execute(context);
    }

    /// <summary>
    /// Apply tectonic deformation - material displacement
    /// Tectonic activity has medium locality
    /// 
    /// Refactored to delegate to specialized TectonicDeformationProcess layer
    /// </summary>
    public void ApplyTectonicDeformation(IEnumerable<(Vector3 from, Vector3 to)> displacements)
    {
        var context = new GeologicalProcessContext
        {
            Displacements = displacements
        };

        _tectonicProcess.Execute(context);
    }

    /// <summary>
    /// Apply weathering process - gradual material transformation
    /// Weathering has high spatial locality along surfaces
    /// 
    /// Refactored to delegate to specialized WeatheringProcess layer
    /// </summary>
    public void ApplyWeathering(IEnumerable<Vector3> surfacePositions, MaterialData weatheredMaterial)
    {
        var context = new GeologicalProcessContext
        {
            Positions = surfacePositions,
            TargetMaterial = weatheredMaterial,
            Intensity = 1.0f // Full weathering by default
        };

        _weatheringProcess.Execute(context);
    }

    /// <summary>
    /// Get material at position (pass-through to delta octree)
    /// </summary>
    public MaterialData GetMaterial(Vector3 position)
    {
        return _deltaOctree.ReadVoxel(position);
    }

    /// <summary>
    /// Set material at position (pass-through to delta octree)
    /// </summary>
    public void SetMaterial(Vector3 position, MaterialData material)
    {
        _deltaOctree.WriteVoxel(position, material);
    }

    /// <summary>
    /// Query region for geological analysis
    /// </summary>
    public IEnumerable<(Vector3 position, MaterialData material)> QueryRegion(Vector3 minBounds, Vector3 maxBounds)
    {
        var results = new List<(Vector3, MaterialData)>();

        for (float x = minBounds.X; x <= maxBounds.X; x++)
        {
            for (float y = minBounds.Y; y <= maxBounds.Y; y++)
            {
                for (float z = minBounds.Z; z <= maxBounds.Z; z++)
                {
                    var position = new Vector3(x, y, z);
                    var material = _deltaOctree.ReadVoxel(position);
                    results.Add((position, material));
                }
            }
        }

        return results;
    }

    /// <summary>
    /// Consolidate pending deltas (expose for process control)
    /// </summary>
    public void ConsolidateChanges()
    {
        _deltaOctree.ConsolidateDeltas();
    }

    /// <summary>
    /// Get count of pending delta changes
    /// </summary>
    public int GetPendingChangeCount()
    {
        return _deltaOctree.ActiveDeltaCount;
    }
}
