using System.Numerics;

namespace BlueMarble.SpatialData.Interfaces;

/// <summary>
/// Interface for geological process operations
/// Enables testability by allowing mock implementations
/// </summary>
public interface IGeologicalProcessor
{
    /// <summary>
    /// Apply erosion process to remove material from surface
    /// </summary>
    /// <param name="positions">Positions to erode</param>
    /// <param name="erosionRate">Rate of erosion (0.0 to 1.0)</param>
    void ApplyErosion(IEnumerable<Vector3> positions, float erosionRate);

    /// <summary>
    /// Apply deposition process to add material
    /// </summary>
    /// <param name="positions">Positions for deposition</param>
    /// <param name="material">Material to deposit</param>
    void ApplyDeposition(IEnumerable<Vector3> positions, MaterialData material);

    /// <summary>
    /// Apply volcanic intrusion to create magma/lava volumes
    /// </summary>
    /// <param name="center">Center of volcanic activity</param>
    /// <param name="radius">Radius of affected area</param>
    /// <param name="material">Volcanic material (magma/lava)</param>
    void ApplyVolcanicIntrusion(Vector3 center, float radius, MaterialData material);

    /// <summary>
    /// Apply tectonic deformation to displace material
    /// </summary>
    /// <param name="sourceRegion">Source region to move material from</param>
    /// <param name="displacement">Displacement vector</param>
    void ApplyTectonicDeformation(IEnumerable<Vector3> sourceRegion, Vector3 displacement);

    /// <summary>
    /// Query materials in a region
    /// </summary>
    /// <param name="min">Minimum bounds</param>
    /// <param name="max">Maximum bounds</param>
    /// <returns>Dictionary of position-material pairs</returns>
    Dictionary<Vector3, MaterialData> QueryRegion(Vector3 min, Vector3 max);
}
