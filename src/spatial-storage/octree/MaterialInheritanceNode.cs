using System;
using System.Collections.Generic;
using System.Linq;

namespace BlueMarble.SpatialStorage.Octree
{
    /// <summary>
    /// Octree node with implicit material inheritance for memory optimization.
    /// Implements lazy material inheritance to reduce storage from petabytes to gigabytes
    /// for homogeneous regions like oceans.
    /// </summary>
    public class MaterialInheritanceNode
    {
        /// <summary>
        /// Explicit material for this node. If null, inherits from parent.
        /// </summary>
        public MaterialData? ExplicitMaterial { get; set; }

        /// <summary>
        /// Child material counts for fast homogeneity checks
        /// </summary>
        public Dictionary<MaterialId, int> ChildMaterialCounts { get; set; } = new();

        /// <summary>
        /// Child nodes (8 for 3D octree, 4 for 2D quadtree)
        /// </summary>
        public MaterialInheritanceNode[]? Children { get; set; }

        /// <summary>
        /// Parent node reference for inheritance chain
        /// </summary>
        public MaterialInheritanceNode? Parent { get; set; }

        /// <summary>
        /// Node bounds in 3D space
        /// </summary>
        public BoundingBox Bounds { get; set; } = new();

        /// <summary>
        /// Octree level (0 = root)
        /// </summary>
        public int Level { get; set; }

        /// <summary>
        /// Get effective material for this node, inheriting from parent if not explicitly set.
        /// Implements the core inheritance algorithm with O(log n) worst-case lookup.
        /// </summary>
        public MaterialData GetEffectiveMaterial()
        {
            if (ExplicitMaterial != null)
                return ExplicitMaterial;

            // Walk up the tree until we find explicit material
            var current = Parent;
            while (current != null)
            {
                if (current.ExplicitMaterial != null)
                    return current.ExplicitMaterial;
                current = current.Parent;
            }

            // Fallback to default material (e.g., ocean)
            return MaterialData.DefaultOcean;
        }

        /// <summary>
        /// Check if this node needs explicit material storage.
        /// Returns true only if the material differs from what would be inherited.
        /// </summary>
        public bool RequiresExplicitMaterial()
        {
            var parentMaterial = Parent?.GetEffectiveMaterial();
            return ExplicitMaterial != null && !ExplicitMaterial.Equals(parentMaterial);
        }

        /// <summary>
        /// Calculate homogeneity of child materials.
        /// Returns 1.0 for completely homogeneous, 0.0 for completely heterogeneous.
        /// </summary>
        public double CalculateHomogeneity()
        {
            if (ChildMaterialCounts.Count <= 1) 
                return 1.0;

            var dominant = ChildMaterialCounts.Values.Max();
            var total = ChildMaterialCounts.Values.Sum();
            return (double)dominant / total;
        }

        /// <summary>
        /// Get material at specific point within this node's bounds.
        /// Uses inheritance and homogeneity checks for efficient querying.
        /// </summary>
        public MaterialData GetMaterialAtPoint(Vector3 point)
        {
            // Verify point is within bounds
            if (!Bounds.Contains(point))
                throw new ArgumentException("Point is outside node bounds");

            // If homogeneous or leaf, return primary material
            if (Children == null || CalculateHomogeneity() >= 0.9) // 90% homogeneity threshold
                return GetEffectiveMaterial();

            // Find appropriate child and recurse
            var childIndex = CalculateChildIndex(point);
            return Children[childIndex]?.GetMaterialAtPoint(point) ?? GetEffectiveMaterial();
        }

        /// <summary>
        /// Set material for this node, optimizing storage by checking if inheritance can be used
        /// </summary>
        public void SetMaterial(MaterialData material)
        {
            var inheritedMaterial = Parent?.GetEffectiveMaterial();
            
            if (material.Equals(inheritedMaterial))
            {
                // Can inherit, clear explicit material
                ExplicitMaterial = null;
            }
            else
            {
                // Must store explicitly
                ExplicitMaterial = material;
            }
        }

        /// <summary>
        /// Calculate memory footprint of this subtree
        /// </summary>
        public long CalculateMemoryFootprint()
        {
            long memory = sizeof(int) * 2; // Level + basic overhead
            
            if (ExplicitMaterial != null)
                memory += EstimateMaterialSize(ExplicitMaterial);

            memory += ChildMaterialCounts.Count * (sizeof(int) * 2); // Dictionary overhead

            if (Children != null)
            {
                foreach (var child in Children)
                {
                    if (child != null)
                        memory += child.CalculateMemoryFootprint();
                }
            }

            return memory;
        }

        /// <summary>
        /// Optimize subtree by removing unnecessary explicit materials
        /// </summary>
        public void OptimizeInheritance()
        {
            // If this node's explicit material matches inherited material, remove it
            if (ExplicitMaterial != null && ExplicitMaterial.Equals(Parent?.GetEffectiveMaterial()))
            {
                ExplicitMaterial = null;
            }

            // Recursively optimize children
            if (Children != null)
            {
                foreach (var child in Children)
                {
                    child?.OptimizeInheritance();
                }
            }
        }

        private int CalculateChildIndex(Vector3 point)
        {
            var center = Bounds.Center;
            int index = 0;

            if (point.X >= center.X) index |= 1;
            if (point.Y >= center.Y) index |= 2;
            if (point.Z >= center.Z) index |= 4;

            return index;
        }

        private static long EstimateMaterialSize(MaterialData material)
        {
            // Rough estimation: enum + string + float + properties
            return sizeof(int) + (material.Name?.Length ?? 0) * sizeof(char) + sizeof(float) + 64; // Properties overhead
        }
    }

    /// <summary>
    /// Simple 3D bounding box for spatial queries
    /// </summary>
    public class BoundingBox
    {
        public Vector3 Min { get; set; }
        public Vector3 Max { get; set; }
        
        public Vector3 Center => new Vector3(
            (Min.X + Max.X) / 2,
            (Min.Y + Max.Y) / 2,
            (Min.Z + Max.Z) / 2
        );

        public bool Contains(Vector3 point)
        {
            return point.X >= Min.X && point.X <= Max.X &&
                   point.Y >= Min.Y && point.Y <= Max.Y &&
                   point.Z >= Min.Z && point.Z <= Max.Z;
        }
    }

    /// <summary>
    /// Simple 3D vector for spatial coordinates
    /// </summary>
    public struct Vector3
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }

        public Vector3(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }
    }
}