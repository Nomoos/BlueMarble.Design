using System.Numerics;

namespace BlueMarble.SpatialData.Interfaces;

/// <summary>
/// Interface for delta overlay storage operations
/// Enables testability by allowing mock implementations
/// </summary>
public interface IDeltaStorage
{
    /// <summary>
    /// Read voxel material with delta overlay support
    /// </summary>
    /// <param name="position">Position to query</param>
    /// <returns>Material at position (delta or base)</returns>
    MaterialData ReadVoxel(Vector3 position);

    /// <summary>
    /// Write voxel material using delta overlay
    /// </summary>
    /// <param name="position">Position to update</param>
    /// <param name="newMaterial">New material to set</param>
    void WriteVoxel(Vector3 position, MaterialData newMaterial);

    /// <summary>
    /// Write multiple voxels in a batch operation
    /// </summary>
    /// <param name="updates">Collection of position-material pairs</param>
    void WriteMaterialBatch(IEnumerable<(Vector3 Position, MaterialData Material)> updates);

    /// <summary>
    /// Check if a delta exists at the given position
    /// </summary>
    /// <param name="position">Position to check</param>
    /// <returns>True if delta exists, false otherwise</returns>
    bool HasDelta(Vector3 position);

    /// <summary>
    /// Get the count of active deltas
    /// </summary>
    int ActiveDeltaCount { get; }

    /// <summary>
    /// Consolidate deltas into the base octree
    /// </summary>
    /// <param name="threshold">Optional threshold for selective consolidation</param>
    void ConsolidateDeltas(int? threshold = null);
}
