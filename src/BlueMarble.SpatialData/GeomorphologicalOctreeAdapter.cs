using System.Numerics;

namespace BlueMarble.SpatialData;

/// <summary>
/// Adapter for integrating geological processes with delta overlay system
/// Provides optimized interfaces for erosion, deposition, and volcanic processes
/// </summary>
public class GeomorphologicalOctreeAdapter
{
    private readonly DeltaPatchOctree _deltaOctree;

    public GeomorphologicalOctreeAdapter(DeltaPatchOctree deltaOctree)
    {
        _deltaOctree = deltaOctree ?? throw new ArgumentNullException(nameof(deltaOctree));
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
            var currentMaterial = _deltaOctree.ReadVoxel(position);

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

        _deltaOctree.WriteMaterialBatch(updates);
    }

    /// <summary>
    /// Apply deposition process - adds material to surface
    /// Deposition has medium spatial locality
    /// </summary>
    public void ApplyDeposition(IEnumerable<(Vector3 position, MaterialData depositMaterial)> depositions)
    {
        var updates = new List<(Vector3, MaterialData)>();

        foreach (var (position, depositMaterial) in depositions)
        {
            var currentMaterial = _deltaOctree.ReadVoxel(position);

            // If air, replace with deposit
            if (currentMaterial.MaterialType == MaterialId.Air)
            {
                updates.Add((position, depositMaterial));
            }
            else
            {
                // Layer on top - for now just replace, could be more sophisticated
                updates.Add((position, depositMaterial));
            }
        }

        _deltaOctree.WriteMaterialBatch(updates);
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

        _deltaOctree.WriteMaterialBatch(updates);
    }

    /// <summary>
    /// Apply tectonic deformation - material displacement
    /// Tectonic activity has medium locality
    /// </summary>
    public void ApplyTectonicDeformation(IEnumerable<(Vector3 from, Vector3 to)> displacements)
    {
        var updates = new List<(Vector3, MaterialData)>();

        foreach (var (from, to) in displacements)
        {
            var material = _deltaOctree.ReadVoxel(from);

            // Move material from 'from' to 'to'
            updates.Add((from, MaterialData.DefaultAir)); // Clear source
            updates.Add((to, material)); // Place at destination
        }

        _deltaOctree.WriteMaterialBatch(updates);
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

        _deltaOctree.WriteMaterialBatch(updates);
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
