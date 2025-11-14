using System.Numerics;
using BlueMarble.SpatialData;
using BlueMarble.SpatialData.Interfaces;

namespace BlueMarble.SpatialData.Tests.Mocks;

/// <summary>
/// Mock implementation of IGeologicalProcessor for testing
/// Provides controlled, predictable behavior for integration tests
/// </summary>
public class MockGeologicalProcessor : IGeologicalProcessor
{
    // Track method calls for verification
    public int ApplyErosionCallCount { get; private set; }
    public int ApplyDepositionCallCount { get; private set; }
    public int ApplyVolcanicIntrusionCallCount { get; private set; }
    public int ApplyTectonicDeformationCallCount { get; private set; }
    public int QueryRegionCallCount { get; private set; }
    
    // Last parameters received (for verification)
    public IEnumerable<Vector3>? LastErosionPositions { get; private set; }
    public float LastErosionRate { get; private set; }
    public IEnumerable<Vector3>? LastDepositionPositions { get; private set; }
    public MaterialData LastDepositionMaterial { get; private set; }
    
    // Simulated results
    private readonly Dictionary<Vector3, MaterialData> _simulatedRegion;
    
    public MockGeologicalProcessor()
    {
        _simulatedRegion = new Dictionary<Vector3, MaterialData>();
    }

    public void ApplyErosion(IEnumerable<Vector3> positions, float erosionRate)
    {
        ApplyErosionCallCount++;
        LastErosionPositions = positions.ToList();
        LastErosionRate = erosionRate;
    }

    public void ApplyDeposition(IEnumerable<Vector3> positions, MaterialData material)
    {
        ApplyDepositionCallCount++;
        LastDepositionPositions = positions.ToList();
        LastDepositionMaterial = material;
    }

    public void ApplyVolcanicIntrusion(Vector3 center, float radius, MaterialData material)
    {
        ApplyVolcanicIntrusionCallCount++;
    }

    public void ApplyTectonicDeformation(IEnumerable<Vector3> sourceRegion, Vector3 displacement)
    {
        ApplyTectonicDeformationCallCount++;
    }

    public Dictionary<Vector3, MaterialData> QueryRegion(Vector3 min, Vector3 max)
    {
        QueryRegionCallCount++;
        return new Dictionary<Vector3, MaterialData>(_simulatedRegion);
    }

    /// <summary>
    /// Set simulated region data for testing
    /// </summary>
    public void SetSimulatedRegion(Dictionary<Vector3, MaterialData> region)
    {
        _simulatedRegion.Clear();
        foreach (var kvp in region)
        {
            _simulatedRegion[kvp.Key] = kvp.Value;
        }
    }

    /// <summary>
    /// Clear call tracking (for test setup)
    /// </summary>
    public void Clear()
    {
        ApplyErosionCallCount = 0;
        ApplyDepositionCallCount = 0;
        ApplyVolcanicIntrusionCallCount = 0;
        ApplyTectonicDeformationCallCount = 0;
        QueryRegionCallCount = 0;
        LastErosionPositions = null;
        LastDepositionPositions = null;
        _simulatedRegion.Clear();
    }
}
