using System.Numerics;
using System.Collections.Concurrent;
using BlueMarble.SpatialData;
using BlueMarble.SpatialData.Interfaces;

namespace BlueMarble.SpatialData.Tests.Mocks;

/// <summary>
/// Mock implementation of IOctreeStorage for testing
/// Provides controlled, predictable behavior for unit tests
/// </summary>
public class MockOctreeStorage : IOctreeStorage
{
    private readonly ConcurrentDictionary<Vector3, MaterialData> _storage;
    private readonly MaterialData _defaultMaterial;
    
    // Track method calls for verification
    public int GetMaterialAtCallCount { get; private set; }
    public int SetMaterialAtCallCount { get; private set; }
    public int QueryRegionCallCount { get; private set; }
    
    public MockOctreeStorage(MaterialData defaultMaterial)
    {
        _storage = new ConcurrentDictionary<Vector3, MaterialData>();
        _defaultMaterial = defaultMaterial;
    }

    public MaterialData GetMaterialAt(Vector3 position)
    {
        GetMaterialAtCallCount++;
        return _storage.TryGetValue(position, out var material) ? material : _defaultMaterial;
    }

    public void SetMaterialAt(Vector3 position, MaterialData material)
    {
        SetMaterialAtCallCount++;
        _storage[position] = material;
    }

    public Dictionary<Vector3, MaterialData> QueryRegion(Vector3 min, Vector3 max)
    {
        QueryRegionCallCount++;
        var results = new Dictionary<Vector3, MaterialData>();
        
        foreach (var kvp in _storage)
        {
            if (kvp.Key.X >= min.X && kvp.Key.X <= max.X &&
                kvp.Key.Y >= min.Y && kvp.Key.Y <= max.Y &&
                kvp.Key.Z >= min.Z && kvp.Key.Z <= max.Z)
            {
                results[kvp.Key] = kvp.Value;
            }
        }
        
        return results;
    }

    public MaterialData GetEffectiveMaterial()
    {
        return _defaultMaterial;
    }

    /// <summary>
    /// Clear all stored data (for test setup)
    /// </summary>
    public void Clear()
    {
        _storage.Clear();
        GetMaterialAtCallCount = 0;
        SetMaterialAtCallCount = 0;
        QueryRegionCallCount = 0;
    }

    /// <summary>
    /// Get count of stored materials (for test assertions)
    /// </summary>
    public int StoredMaterialCount => _storage.Count;
}
