using System.Numerics;

namespace BlueMarble.SpatialData;

/// <summary>
/// Memory-efficient octree node with implicit material inheritance
/// Achieves 80-95% memory reduction for homogeneous regions
/// </summary>
public class OptimizedOctreeNode
{
    private MaterialData? _explicitMaterial;
    private OptimizedOctreeNode?[]? _children;
    private Dictionary<MaterialId, int>? _childMaterialCounts;
    private double? _cachedHomogeneity;

    /// <summary>
    /// Parent node for inheritance chain traversal
    /// </summary>
    public OptimizedOctreeNode? Parent { get; set; }

    /// <summary>
    /// Explicitly stored material (null means inherit from parent)
    /// </summary>
    public MaterialData? ExplicitMaterial
    {
        get => _explicitMaterial;
        set
        {
            _explicitMaterial = value;
            _cachedHomogeneity = null; // Invalidate cache
        }
    }

    /// <summary>
    /// Child nodes (null if leaf node)
    /// </summary>
    public OptimizedOctreeNode?[]? Children
    {
        get => _children;
        set
        {
            _children = value;
            _cachedHomogeneity = null; // Invalidate cache
        }
    }

    /// <summary>
    /// Material distribution counts for homogeneity calculation
    /// </summary>
    public Dictionary<MaterialId, int> ChildMaterialCounts
    {
        get => _childMaterialCounts ??= new Dictionary<MaterialId, int>();
        set
        {
            _childMaterialCounts = value;
            _cachedHomogeneity = null; // Invalidate cache
        }
    }

    /// <summary>
    /// Cached homogeneity ratio to avoid repeated calculations
    /// </summary>
    public double? CachedHomogeneity
    {
        get => _cachedHomogeneity;
        set => _cachedHomogeneity = value;
    }

    /// <summary>
    /// Get effective material for this node, inheriting from parent if not explicitly set
    /// Performance: O(log n) worst case for inheritance chain traversal
    /// </summary>
    public MaterialData GetEffectiveMaterial()
    {
        if (_explicitMaterial.HasValue)
            return _explicitMaterial.Value;

        // Walk up the tree until we find explicit material
        var current = Parent;
        while (current != null)
        {
            if (current._explicitMaterial.HasValue)
                return current._explicitMaterial.Value;
            current = current.Parent;
        }

        // Fallback to default material (ocean)
        return MaterialData.DefaultOcean;
    }

    /// <summary>
    /// Check if this node needs explicit material storage
    /// Memory optimization: only store materials that differ from parent
    /// </summary>
    public bool RequiresExplicitMaterial()
    {
        var parentMaterial = Parent?.GetEffectiveMaterial();
        return _explicitMaterial.HasValue &&
               (parentMaterial == null || !_explicitMaterial.Value.Equals(parentMaterial.Value));
    }

    /// <summary>
    /// Calculate homogeneity for BlueMarble's 90% threshold rule
    /// "if there is air in 90% 16×16m material this cell will be air"
    /// </summary>
    public double CalculateHomogeneity()
    {
        if (_cachedHomogeneity.HasValue)
            return _cachedHomogeneity.Value;

        if (_childMaterialCounts == null || _childMaterialCounts.Count <= 1)
        {
            _cachedHomogeneity = 1.0;
            return 1.0;
        }

        var totalCount = _childMaterialCounts.Values.Sum();
        if (totalCount == 0)
        {
            _cachedHomogeneity = 1.0;
            return 1.0;
        }

        var maxCount = _childMaterialCounts.Values.Max();
        _cachedHomogeneity = (double)maxCount / totalCount;
        return _cachedHomogeneity.Value;
    }

    /// <summary>
    /// Check if node can be collapsed based on homogeneity threshold
    /// </summary>
    public bool CanCollapse(double threshold = 0.9)
    {
        return CalculateHomogeneity() >= threshold;
    }

    /// <summary>
    /// Get or create child node at index
    /// </summary>
    public OptimizedOctreeNode GetOrCreateChild(int index)
    {
        if (index < 0 || index >= 8)
            throw new ArgumentOutOfRangeException(nameof(index), "Child index must be 0-7");

        _children ??= new OptimizedOctreeNode?[8];

        if (_children[index] == null)
        {
            _children[index] = new OptimizedOctreeNode { Parent = this };
        }

        return _children[index]!;
    }

    /// <summary>
    /// Check if this is a leaf node
    /// </summary>
    public bool IsLeaf => _children == null || _children.All(c => c == null);

    /// <summary>
    /// Update child material count for homogeneity tracking
    /// </summary>
    public void UpdateChildMaterialCount(MaterialId materialId, int delta)
    {
        if (_childMaterialCounts == null)
            _childMaterialCounts = new Dictionary<MaterialId, int>();

        if (!_childMaterialCounts.ContainsKey(materialId))
            _childMaterialCounts[materialId] = 0;

        _childMaterialCounts[materialId] += delta;

        if (_childMaterialCounts[materialId] <= 0)
            _childMaterialCounts.Remove(materialId);

        _cachedHomogeneity = null; // Invalidate cache
    }
}
