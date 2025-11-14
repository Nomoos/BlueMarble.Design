using System.Numerics;
using BlueMarble.SpatialData.Interfaces;

namespace BlueMarble.SpatialData;

/// <summary>
/// Adapter for integrating geological processes with delta overlay system
/// Provides optimized interfaces for erosion, deposition, and volcanic processes
/// </summary>
public class GeomorphologicalOctreeAdapter : IGeologicalProcessor
{
    private readonly IDeltaStorage _deltaStorage;

    /// <summary>
    /// Constructor for dependency injection with IDeltaStorage interface
    /// </summary>
    public GeomorphologicalOctreeAdapter(IDeltaStorage deltaStorage)
    {
        _deltaStorage = deltaStorage ?? throw new ArgumentNullException(nameof(deltaStorage));
    }

    /// <summary>
    /// Original constructor for backward compatibility
    /// </summary>
    public GeomorphologicalOctreeAdapter(DeltaPatchOctree deltaOctree)
    {
        _deltaStorage = deltaOctree ?? throw new ArgumentNullException(nameof(deltaOctree));
    }

    /// <summary>
    /// Apply erosion process - removes material from surface
    /// Erosion typically has high spatial locality (85%)
    /// </summary>
    public void ApplyErosion(IEnumerable<Vector3> positions, float erosionRate)
    {
        var updates = new List<(Vector3, MaterialData)>();

        foreach (var position in positions)
        {
            var currentMaterial = _deltaStorage.ReadVoxel(position);

            // Skip if already air
            if (currentMaterial.MaterialType == MaterialId.Air)
                continue;

            // Apply erosion effect - for simplicity, convert to air or less dense material
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
                updates.Add((position, erodedMaterial));
            }
        }

        _deltaStorage.WriteMaterialBatch(updates);
    }

    /// <summary>
    /// Apply deposition process - adds material to surface
    /// Deposition has medium spatial locality
    /// </summary>
    public void ApplyDeposition(IEnumerable<Vector3> positions, MaterialData material)
    {
        var updates = new List<(Vector3, MaterialData)>();

        foreach (var position in positions)
        {
            var currentMaterial = _deltaStorage.ReadVoxel(position);

            // If air, replace with deposit
            if (currentMaterial.MaterialType == MaterialId.Air)
            {
                updates.Add((position, material));
            }
            else
            {
                // Layer on top - for now just replace, could be more sophisticated
                updates.Add((position, material));
            }
        }

        _deltaStorage.WriteMaterialBatch(updates);
    }

    /// <summary>
    /// Apply volcanic intrusion - magma/lava injection
    /// Volcanic activity has high spatial clustering
    /// </summary>
    public void ApplyVolcanicIntrusion(Vector3 center, float radius, MaterialData volcanicMaterial)
    {
        var updates = new List<(Vector3, MaterialData)>();

        // Generate sphere of affected positions
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
                        updates.Add((position, volcanicMaterial));
                    }
                }
            }
        }

        _deltaStorage.WriteMaterialBatch(updates);
    }

    /// <summary>
    /// Apply tectonic deformation - material displacement
    /// Tectonic activity has medium locality
    /// </summary>
    public void ApplyTectonicDeformation(IEnumerable<Vector3> sourceRegion, Vector3 displacement)
    {
        var updates = new List<(Vector3, MaterialData)>();

        foreach (var from in sourceRegion)
        {
            var material = _deltaStorage.ReadVoxel(from);
            var to = from + displacement;

            // Move material from 'from' to 'to'
            updates.Add((from, MaterialData.DefaultAir)); // Clear source
            updates.Add((to, material)); // Place at destination
        }

        _deltaStorage.WriteMaterialBatch(updates);
    }

    /// <summary>
    /// Apply weathering process - gradual material transformation
    /// Weathering has high spatial locality along surfaces
    /// </summary>
    public void ApplyWeathering(IEnumerable<Vector3> surfacePositions, MaterialData weatheredMaterial)
    {
        var updates = surfacePositions
            .Select(pos => (pos, weatheredMaterial))
            .ToList();

        _deltaStorage.WriteMaterialBatch(updates);
    }

    /// <summary>
    /// Get material at position (pass-through to delta storage)
    /// </summary>
    public MaterialData GetMaterial(Vector3 position)
    {
        return _deltaStorage.ReadVoxel(position);
    }

    /// <summary>
    /// Set material at position (pass-through to delta storage)
    /// </summary>
    public void SetMaterial(Vector3 position, MaterialData material)
    {
        _deltaStorage.WriteVoxel(position, material);
    }

    /// <summary>
    /// Query region for geological analysis
    /// </summary>
    public Dictionary<Vector3, MaterialData> QueryRegion(Vector3 minBounds, Vector3 maxBounds)
    {
        var results = new Dictionary<Vector3, MaterialData>();

        for (float x = minBounds.X; x <= maxBounds.X; x++)
        {
            for (float y = minBounds.Y; y <= maxBounds.Y; y++)
            {
                for (float z = minBounds.Z; z <= maxBounds.Z; z++)
                {
                    var position = new Vector3(x, y, z);
                    var material = _deltaStorage.ReadVoxel(position);
                    results[position] = material;
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
        _deltaStorage.ConsolidateDeltas();
    }

    /// <summary>
    /// Get count of pending delta changes
    /// </summary>
    public int GetPendingChangeCount()
    {
        return _deltaStorage.ActiveDeltaCount;
    }
}
