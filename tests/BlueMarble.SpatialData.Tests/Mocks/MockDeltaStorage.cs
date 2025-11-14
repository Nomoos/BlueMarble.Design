using System.Numerics;
using System.Collections.Concurrent;
using BlueMarble.SpatialData;
using BlueMarble.SpatialData.Interfaces;

namespace BlueMarble.SpatialData.Tests.Mocks;

/// <summary>
/// Mock implementation of IDeltaStorage for testing
/// Provides controlled, predictable behavior for unit tests
/// </summary>
public class MockDeltaStorage : IDeltaStorage
{
    private readonly ConcurrentDictionary<Vector3, MaterialData> _storage;
    private readonly MaterialData _defaultMaterial;
    
    // Track method calls for verification
    public int ReadVoxelCallCount { get; private set; }
    public int WriteVoxelCallCount { get; private set; }
    public int WriteMaterialBatchCallCount { get; private set; }
    public int ConsolidateDeltasCallCount { get; private set; }
    
    // Simulate behavior
    public bool SimulateReadFailure { get; set; }
    public bool SimulateWriteFailure { get; set; }

    public MockDeltaStorage(MaterialData defaultMaterial)
    {
        _storage = new ConcurrentDictionary<Vector3, MaterialData>();
        _defaultMaterial = defaultMaterial;
    }

    public MaterialData ReadVoxel(Vector3 position)
    {
        ReadVoxelCallCount++;
        
        if (SimulateReadFailure)
        {
            throw new InvalidOperationException("Simulated read failure");
        }
        
        return _storage.TryGetValue(position, out var material) ? material : _defaultMaterial;
    }

    public void WriteVoxel(Vector3 position, MaterialData newMaterial)
    {
        WriteVoxelCallCount++;
        
        if (SimulateWriteFailure)
        {
            throw new InvalidOperationException("Simulated write failure");
        }
        
        _storage[position] = newMaterial;
    }

    public void WriteMaterialBatch(IEnumerable<(Vector3 Position, MaterialData Material)> updates)
    {
        WriteMaterialBatchCallCount++;
        
        if (SimulateWriteFailure)
        {
            throw new InvalidOperationException("Simulated batch write failure");
        }
        
        foreach (var (position, material) in updates)
        {
            _storage[position] = material;
        }
    }

    public bool HasDelta(Vector3 position)
    {
        return _storage.ContainsKey(position);
    }

    public int ActiveDeltaCount => _storage.Count;

    public void ConsolidateDeltas(int? threshold = null)
    {
        ConsolidateDeltasCallCount++;
        // Mock implementation - just clear half the deltas for testing
        var toRemove = _storage.Keys.Take(_storage.Count / 2).ToList();
        foreach (var key in toRemove)
        {
            _storage.TryRemove(key, out _);
        }
    }

    /// <summary>
    /// Clear all stored data (for test setup)
    /// </summary>
    public void Clear()
    {
        _storage.Clear();
        ReadVoxelCallCount = 0;
        WriteVoxelCallCount = 0;
        WriteMaterialBatchCallCount = 0;
        ConsolidateDeltasCallCount = 0;
        SimulateReadFailure = false;
        SimulateWriteFailure = false;
    }
}
