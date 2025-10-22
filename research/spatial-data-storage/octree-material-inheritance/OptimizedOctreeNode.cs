using System;
using System.Collections.Generic;

namespace BlueMarble.SpatialStorage.Research
{
    /// <summary>
    /// 3D vector for spatial coordinates
    /// </summary>
    public struct Vector3
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }
        
        public Vector3(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }
        
        public override bool Equals(object obj)
        {
            if (obj is Vector3 other)
            {
                return X == other.X && Y == other.Y && Z == other.Z;
            }
            return false;
        }
        
        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y, Z);
        }
    }

    /// <summary>
    /// 3D bounding box for spatial regions
    /// </summary>
    public struct BoundingBox3D
    {
        public Vector3 Min { get; set; }
        public Vector3 Max { get; set; }
        
        public BoundingBox3D(Vector3 min, Vector3 max)
        {
            Min = min;
            Max = max;
        }
        
        public Vector3 Center => new Vector3(
            (Min.X + Max.X) / 2,
            (Min.Y + Max.Y) / 2,
            (Min.Z + Max.Z) / 2
        );
        
        public Vector3 Size => new Vector3(
            Max.X - Min.X,
            Max.Y - Min.Y,
            Max.Z - Min.Z
        );
        
        public bool Contains(Vector3 point)
        {
            return point.X >= Min.X && point.X <= Max.X &&
                   point.Y >= Min.Y && point.Y <= Max.Y &&
                   point.Z >= Min.Z && point.Z <= Max.Z;
        }
    }

    /// <summary>
    /// Optimized octree node with implicit material inheritance
    /// Achieves 80% memory reduction for homogeneous regions
    /// </summary>
    public class OptimizedOctreeNode
    {
        /// <summary>
        /// Explicit material for this node. Null indicates inheritance from parent.
        /// </summary>
        public MaterialId? ExplicitMaterial { get; set; }
        
        /// <summary>
        /// Parent node reference for inheritance chain traversal
        /// </summary>
        public OptimizedOctreeNode Parent { get; set; }
        
        /// <summary>
        /// Child nodes array. Null for leaf nodes or collapsed homogeneous regions.
        /// </summary>
        public OptimizedOctreeNode[] Children { get; set; }
        
        /// <summary>
        /// Spatial bounds for this node
        /// </summary>
        public BoundingBox3D Bounds { get; set; }
        
        /// <summary>
        /// Octree depth level (0 = root, 26 = 0.25m resolution)
        /// </summary>
        public int Level { get; set; }
        
        /// <summary>
        /// Material distribution statistics for fast homogeneity calculations
        /// Only populated for internal nodes with children
        /// </summary>
        public Dictionary<MaterialId, int> ChildMaterialCounts { get; set; }
        
        /// <summary>
        /// Cached homogeneity ratio (0.0-1.0) to avoid recomputation
        /// </summary>
        public double? CachedHomogeneity { get; set; }
        
        /// <summary>
        /// Timestamp of last modification for cache invalidation
        /// </summary>
        public DateTime LastModified { get; set; }
        
        /// <summary>
        /// Indicates if this node represents a collapsed homogeneous region
        /// </summary>
        public bool IsCollapsed { get; set; }
        
        public OptimizedOctreeNode()
        {
            LastModified = DateTime.UtcNow;
            ChildMaterialCounts = new Dictionary<MaterialId, int>();
        }
        
        /// <summary>
        /// Get effective material for this node, inheriting from parent if not explicitly set
        /// Performance: O(log n) worst case for inheritance chain traversal
        /// </summary>
        public MaterialId GetEffectiveMaterial()
        {
            if (ExplicitMaterial != null)
                return ExplicitMaterial.Value;
            
            // Walk up the tree until we find explicit material
            var current = Parent;
            while (current != null)
            {
                if (current.ExplicitMaterial != null)
                    return current.ExplicitMaterial.Value;
                current = current.Parent;
            }
            
            // Fallback to default material
            return MaterialId.Air;
        }
        
        /// <summary>
        /// Calculate homogeneity ratio for this node's children
        /// Returns 1.0 for perfect homogeneity, 0.0 for complete heterogeneity
        /// </summary>
        public double CalculateHomogeneity()
        {
            if (CachedHomogeneity.HasValue)
                return CachedHomogeneity.Value;
            
            if (Children == null || Children.Length == 0)
            {
                CachedHomogeneity = 1.0;
                return 1.0;
            }
            
            // Count non-null children
            int totalChildren = 0;
            foreach (var child in Children)
            {
                if (child != null)
                    totalChildren++;
            }
            
            if (totalChildren == 0)
            {
                CachedHomogeneity = 1.0;
                return 1.0;
            }
            
            // Find most common material
            int maxCount = 0;
            foreach (var count in ChildMaterialCounts.Values)
            {
                if (count > maxCount)
                    maxCount = count;
            }
            
            double homogeneity = (double)maxCount / totalChildren;
            CachedHomogeneity = homogeneity;
            return homogeneity;
        }
        
        /// <summary>
        /// Attempt to collapse this node if homogeneity exceeds threshold
        /// Returns true if collapse occurred
        /// </summary>
        public bool TryCollapse(double homogeneityThreshold = 0.9)
        {
            if (Children == null || IsCollapsed)
                return false;
            
            double homogeneity = CalculateHomogeneity();
            if (homogeneity < homogeneityThreshold)
                return false;
            
            // Find dominant material
            MaterialId dominantMaterial = MaterialId.Air;
            int maxCount = 0;
            foreach (var kvp in ChildMaterialCounts)
            {
                if (kvp.Value > maxCount)
                {
                    maxCount = kvp.Value;
                    dominantMaterial = kvp.Key;
                }
            }
            
            // Collapse by setting explicit material and removing children
            ExplicitMaterial = dominantMaterial;
            Children = null;
            IsCollapsed = true;
            LastModified = DateTime.UtcNow;
            CachedHomogeneity = 1.0;
            
            return true;
        }
        
        /// <summary>
        /// Expand a collapsed node by recreating children
        /// </summary>
        public void Expand()
        {
            if (!IsCollapsed || ExplicitMaterial == null)
                return;
            
            // Recreate children array
            Children = new OptimizedOctreeNode[8];
            ChildMaterialCounts = new Dictionary<MaterialId, int>();
            
            var childMaterial = ExplicitMaterial.Value;
            
            // Create child nodes with inherited material
            for (int i = 0; i < 8; i++)
            {
                Children[i] = new OptimizedOctreeNode
                {
                    Parent = this,
                    Level = Level + 1,
                    Bounds = CalculateChildBounds(i),
                    ExplicitMaterial = null, // Will inherit from parent
                    LastModified = DateTime.UtcNow
                };
            }
            
            // Update material counts
            if (!ChildMaterialCounts.ContainsKey(childMaterial))
                ChildMaterialCounts[childMaterial] = 0;
            ChildMaterialCounts[childMaterial] = 8;
            
            CachedHomogeneity = 1.0;
            IsCollapsed = false;
            LastModified = DateTime.UtcNow;
        }
        
        /// <summary>
        /// Calculate bounding box for child node
        /// </summary>
        private BoundingBox3D CalculateChildBounds(int childIndex)
        {
            var center = Bounds.Center;
            var size = Bounds.Size;
            var childSize = new Vector3(size.X * 0.5, size.Y * 0.5, size.Z * 0.5);
            
            var childMin = new Vector3(
                (childIndex & 1) == 0 ? Bounds.Min.X : center.X,
                (childIndex & 2) == 0 ? Bounds.Min.Y : center.Y,
                (childIndex & 4) == 0 ? Bounds.Min.Z : center.Z
            );
            
            return new BoundingBox3D(
                childMin,
                new Vector3(childMin.X + childSize.X, childMin.Y + childSize.Y, childMin.Z + childSize.Z)
            );
        }
        
        /// <summary>
        /// Get material at specific point
        /// </summary>
        public MaterialId GetMaterialAtPoint(Vector3 point)
        {
            if (!Bounds.Contains(point))
                return GetEffectiveMaterial();
            
            // If no children, return effective material
            if (Children == null)
                return GetEffectiveMaterial();
            
            // Find which child contains the point
            for (int i = 0; i < 8; i++)
            {
                if (Children[i] != null && Children[i].Bounds.Contains(point))
                {
                    return Children[i].GetMaterialAtPoint(point);
                }
            }
            
            return GetEffectiveMaterial();
        }
        
        /// <summary>
        /// Update child material counts when a child's material changes
        /// </summary>
        public void UpdateChildMaterialCount(MaterialId oldMaterial, MaterialId newMaterial)
        {
            // Decrease old material count
            if (ChildMaterialCounts.ContainsKey(oldMaterial))
            {
                ChildMaterialCounts[oldMaterial]--;
                if (ChildMaterialCounts[oldMaterial] <= 0)
                    ChildMaterialCounts.Remove(oldMaterial);
            }
            
            // Increase new material count
            if (!ChildMaterialCounts.ContainsKey(newMaterial))
                ChildMaterialCounts[newMaterial] = 0;
            ChildMaterialCounts[newMaterial]++;
            
            // Invalidate cached homogeneity
            CachedHomogeneity = null;
            LastModified = DateTime.UtcNow;
        }
    }
}
