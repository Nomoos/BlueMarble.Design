using System.Numerics;

namespace BlueMarble.SpatialData.Interfaces;

/// <summary>
/// Interface for octree storage operations
/// Enables testability by allowing mock implementations
/// </summary>
public interface IOctreeStorage
{
    /// <summary>
    /// Query material at a specific position
    /// </summary>
    /// <param name="position">3D position to query</param>
    /// <returns>Material at the specified position</returns>
    MaterialData GetMaterialAt(Vector3 position);

    /// <summary>
    /// Set material at a specific position
    /// </summary>
    /// <param name="position">3D position to update</param>
    /// <param name="material">New material to set</param>
    void SetMaterialAt(Vector3 position, MaterialData material);

    /// <summary>
    /// Query materials in a region
    /// </summary>
    /// <param name="min">Minimum bounds of region</param>
    /// <param name="max">Maximum bounds of region</param>
    /// <returns>Dictionary of positions and their materials</returns>
    Dictionary<Vector3, MaterialData> QueryRegion(Vector3 min, Vector3 max);

    /// <summary>
    /// Get the effective material for a node (with inheritance)
    /// </summary>
    /// <returns>The effective material</returns>
    MaterialData GetEffectiveMaterial();
}
