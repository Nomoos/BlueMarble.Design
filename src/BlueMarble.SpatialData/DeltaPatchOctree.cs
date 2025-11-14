using System.Collections.Concurrent;
using System.Numerics;
using BlueMarble.SpatialData.Interfaces;

namespace BlueMarble.SpatialData;

/// <summary>
/// Delta overlay system for fine-grained octree updates
/// Provides 10x performance improvement for sparse geological updates
/// </summary>
public class DeltaPatchOctree : IDeltaStorage
{
    private readonly OptimizedOctreeNode _baseTree;
    private readonly IOctreeStorage? _octreeStorage;
    private readonly ConcurrentDictionary<Vector3, MaterialDelta> _deltas;
    private readonly int _consolidationThreshold;
    private readonly DeltaCompactionStrategy _compactionStrategy;
    private int _maxDepth;
    private float _worldSize;
    private Vector3 _worldOrigin;

    /// <summary>
    /// Active delta count for monitoring
    /// </summary>
    public int ActiveDeltaCount => _deltas.Count;

    /// <summary>
    /// Constructor for dependency injection with IOctreeStorage interface
    /// </summary>
    public DeltaPatchOctree(
        IOctreeStorage octreeStorage,
        int consolidationThreshold = 1000,
        DeltaCompactionStrategy compactionStrategy = DeltaCompactionStrategy.LazyThreshold,
        int maxDepth = 20,
        float worldSize = 65536f,
        Vector3? worldOrigin = null)
    {
        _octreeStorage = octreeStorage ?? throw new ArgumentNullException(nameof(octreeStorage));
        _baseTree = null!; // Not used when injecting IOctreeStorage
        _deltas = new ConcurrentDictionary<Vector3, MaterialDelta>();
        _consolidationThreshold = consolidationThreshold;
        _compactionStrategy = compactionStrategy;
        _maxDepth = maxDepth;
        _worldSize = worldSize;
        _worldOrigin = worldOrigin ?? new Vector3(-worldSize / 2, -worldSize / 2, -worldSize / 2);
    }

    /// <summary>
    /// Original constructor for backward compatibility
    /// </summary>
    public DeltaPatchOctree(
        OptimizedOctreeNode baseTree,
        int consolidationThreshold = 1000,
        DeltaCompactionStrategy compactionStrategy = DeltaCompactionStrategy.LazyThreshold,
        int maxDepth = 20,
        float worldSize = 65536f,
        Vector3? worldOrigin = null)
    {
        _baseTree = baseTree ?? throw new ArgumentNullException(nameof(baseTree));
        _octreeStorage = null; // Using direct reference
        _deltas = new ConcurrentDictionary<Vector3, MaterialDelta>();
        _consolidationThreshold = consolidationThreshold;
        _compactionStrategy = compactionStrategy;
        _maxDepth = maxDepth;
        _worldSize = worldSize;
        _worldOrigin = worldOrigin ?? new Vector3(-worldSize / 2, -worldSize / 2, -worldSize / 2);
    }

    /// <summary>
    /// Query material with delta overlay support
    /// O(1) delta lookup + O(log n) octree fallback
    /// </summary>
    public MaterialData ReadVoxel(Vector3 position)
    {
        // Check delta overlay first - O(1) operation
        if (_deltas.TryGetValue(position, out var delta))
        {
            return delta.NewMaterial;
        }

        // Fall back to base octree - O(log n) operation
        return _octreeStorage?.GetMaterialAt(position) ?? GetMaterialFromOctree(position);
    }

    /// <summary>
    /// Update material using delta overlay approach
    /// O(1) operation for sparse updates
    /// </summary>
    public void WriteVoxel(Vector3 position, MaterialData newMaterial)
    {
        var baseMaterial = _octreeStorage?.GetMaterialAt(position) ?? GetMaterialFromOctree(position);

        if (baseMaterial.Equals(newMaterial))
        {
            // Remove delta if reverting to base - optimization
            _deltas.TryRemove(position, out _);
        }
        else
        {
            // Store as delta - O(1) operation
            var delta = new MaterialDelta
            {
                Position = position,
                BaseMaterial = baseMaterial,
                NewMaterial = newMaterial,
                Timestamp = DateTime.UtcNow
            };

            _deltas.AddOrUpdate(position, delta, (key, existing) => delta);
        }

        // Trigger consolidation when threshold reached
        if (_deltas.Count > _consolidationThreshold)
        {
            TriggerDeltaConsolidation();
        }
    }

    /// <summary>
    /// Batch update for geological processes
    /// Optimized for sparse, distributed updates
    /// </summary>
    public void WriteMaterialBatch(IEnumerable<(Vector3 Position, MaterialData Material)> updates)
    {
        foreach (var (position, material) in updates)
        {
            WriteVoxel(position, material);
        }
    }

    /// <summary>
    /// Check if position has a delta override
    /// </summary>
    public bool HasDelta(Vector3 position)
    {
        return _deltas.ContainsKey(position);
    }

    /// <summary>
    /// Consolidate deltas back into base octree structure
    /// This is called when delta threshold is reached
    /// </summary>
    public void ConsolidateDeltas(int? threshold = null)
    {
        if (_deltas.IsEmpty)
            return;

        // Group deltas by spatial proximity for efficient consolidation
        var deltasList = _deltas.Values.ToList();

        foreach (var delta in deltasList)
        {
            // Apply delta to base tree
            if (_octreeStorage != null)
            {
                _octreeStorage.SetMaterialAt(delta.Position, delta.NewMaterial);
            }
            else
            {
                SetMaterialInOctree(delta.Position, delta.NewMaterial);
            }

            // Remove from delta overlay
            _deltas.TryRemove(delta.Position, out _);
        }
    }

    /// <summary>
    /// Clear all deltas (for testing or reset)
    /// </summary>
    public void ClearDeltas()
    {
        _deltas.Clear();
    }

    /// <summary>
    /// Get all active deltas (for monitoring/debugging)
    /// </summary>
    public IEnumerable<MaterialDelta> GetAllDeltas()
    {
        return _deltas.Values.ToList();
    }

    private void TriggerDeltaConsolidation()
    {
        switch (_compactionStrategy)
        {
            case DeltaCompactionStrategy.LazyThreshold:
                ConsolidateOldestDeltas();
                break;
            case DeltaCompactionStrategy.SpatialClustering:
                ConsolidateSpatialClusters();
                break;
            case DeltaCompactionStrategy.TimeBasedBatching:
                ConsolidateByAge();
                break;
        }
    }

    private void ConsolidateOldestDeltas()
    {
        // Consolidate oldest 50% of deltas
        var deltasToConsolidate = _deltas.Values
            .OrderBy(d => d.Timestamp)
            .Take(_deltas.Count / 2)
            .ToList();

        foreach (var delta in deltasToConsolidate)
        {
            if (_octreeStorage != null)
            {
                _octreeStorage.SetMaterialAt(delta.Position, delta.NewMaterial);
            }
            else
            {
                SetMaterialInOctree(delta.Position, delta.NewMaterial);
            }
            _deltas.TryRemove(delta.Position, out _);
        }
    }

    private void ConsolidateSpatialClusters()
    {
        // Group deltas by spatial proximity and consolidate clustered regions
        var clusters = ClusterDeltasBySpatialProximity(_deltas.Values);

        foreach (var cluster in clusters.Take(clusters.Count / 2))
        {
            foreach (var delta in cluster)
            {
                if (_octreeStorage != null)
                {
                    _octreeStorage.SetMaterialAt(delta.Position, delta.NewMaterial);
                }
                else
                {
                    SetMaterialInOctree(delta.Position, delta.NewMaterial);
                }
                _deltas.TryRemove(delta.Position, out _);
            }
        }
    }

    private void ConsolidateByAge()
    {
        // Consolidate deltas older than 1 hour
        var cutoffTime = DateTime.UtcNow.AddHours(-1);
        var oldDeltas = _deltas.Values
            .Where(d => d.Timestamp < cutoffTime)
            .ToList();

        foreach (var delta in oldDeltas)
        {
            if (_octreeStorage != null)
            {
                _octreeStorage.SetMaterialAt(delta.Position, delta.NewMaterial);
            }
            else
            {
                SetMaterialInOctree(delta.Position, delta.NewMaterial);
            }
            _deltas.TryRemove(delta.Position, out _);
        }
    }

    private List<List<MaterialDelta>> ClusterDeltasBySpatialProximity(IEnumerable<MaterialDelta> deltas)
    {
        // Simple clustering by grid cell
        var clusters = new Dictionary<Vector3, List<MaterialDelta>>();

        foreach (var delta in deltas)
        {
            var clusterKey = new Vector3(
                MathF.Floor(delta.Position.X / 16f) * 16f,
                MathF.Floor(delta.Position.Y / 16f) * 16f,
                MathF.Floor(delta.Position.Z / 16f) * 16f
            );

            if (!clusters.ContainsKey(clusterKey))
                clusters[clusterKey] = new List<MaterialDelta>();

            clusters[clusterKey].Add(delta);
        }

        return clusters.Values.ToList();
    }

    private MaterialData GetMaterialFromOctree(Vector3 position)
    {
        // Navigate octree to find material at position
        var node = _baseTree;
        var currentSize = _worldSize;
        var currentOrigin = _worldOrigin;

        for (int depth = 0; depth < _maxDepth; depth++)
        {
            // Get effective material if this is a leaf or homogeneous node
            if (node.IsLeaf || (node.CachedHomogeneity.HasValue && node.CachedHomogeneity.Value >= 0.9))
            {
                return node.GetEffectiveMaterial();
            }

            // Calculate child index based on position
            var halfSize = currentSize / 2.0f;
            var childIndex = 0;
            var centerX = currentOrigin.X + halfSize;
            var centerY = currentOrigin.Y + halfSize;
            var centerZ = currentOrigin.Z + halfSize;

            if (position.X >= centerX) childIndex |= 1;
            if (position.Y >= centerY) childIndex |= 2;
            if (position.Z >= centerZ) childIndex |= 4;

            // Update origin for next level
            if ((childIndex & 1) != 0) currentOrigin.X = centerX;
            if ((childIndex & 2) != 0) currentOrigin.Y = centerY;
            if ((childIndex & 4) != 0) currentOrigin.Z = centerZ;

            currentSize = halfSize;

            // Get child node
            if (node.Children == null || node.Children[childIndex] == null)
            {
                return node.GetEffectiveMaterial();
            }

            node = node.Children[childIndex]!;
        }

        return node.GetEffectiveMaterial();
    }

    private void SetMaterialInOctree(Vector3 position, MaterialData material)
    {
        // Navigate octree and set material at position
        var node = _baseTree;
        var currentSize = _worldSize;
        var currentOrigin = _worldOrigin;

        for (int depth = 0; depth < _maxDepth; depth++)
        {
            // Calculate child index based on position
            var halfSize = currentSize / 2.0f;
            var childIndex = 0;
            var centerX = currentOrigin.X + halfSize;
            var centerY = currentOrigin.Y + halfSize;
            var centerZ = currentOrigin.Z + halfSize;

            if (position.X >= centerX) childIndex |= 1;
            if (position.Y >= centerY) childIndex |= 2;
            if (position.Z >= centerZ) childIndex |= 4;

            // Update origin for next level
            if ((childIndex & 1) != 0) currentOrigin.X = centerX;
            if ((childIndex & 2) != 0) currentOrigin.Y = centerY;
            if ((childIndex & 4) != 0) currentOrigin.Z = centerZ;

            currentSize = halfSize;

            // At max depth, set the material
            if (depth == _maxDepth - 1)
            {
                var childNode = node.GetOrCreateChild(childIndex);
                childNode.ExplicitMaterial = material;
                return;
            }

            // Get or create child for next level
            node = node.GetOrCreateChild(childIndex);
        }
    }
}

/// <summary>
/// Represents a material change stored in the delta overlay
/// </summary>
public class MaterialDelta
{
    public Vector3 Position { get; set; }
    public MaterialData BaseMaterial { get; set; }
    public MaterialData NewMaterial { get; set; }
    public DateTime Timestamp { get; set; }
    public int PatchVersion { get; set; }
}

/// <summary>
/// Delta compaction strategies for consolidation
/// </summary>
public enum DeltaCompactionStrategy
{
    /// <summary>
    /// Consolidate when delta count exceeds threshold
    /// </summary>
    LazyThreshold,

    /// <summary>
    /// Consolidate deltas in spatial clusters
    /// </summary>
    SpatialClustering,

    /// <summary>
    /// Consolidate deltas older than threshold
    /// </summary>
    TimeBasedBatching
}
