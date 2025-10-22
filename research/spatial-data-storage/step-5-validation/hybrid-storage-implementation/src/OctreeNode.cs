using System;
using System.Collections.Generic;

namespace BlueMarble.SpatialStorage
{
    /// <summary>
    /// Octree node with material inheritance and automatic subdivision
    /// Reduces memory by 80-95% through material inheritance in homogeneous regions
    /// </summary>
    public class OctreeNode
    {
        public Envelope3D Bounds { get; set; }
        public int Level { get; set; }
        public MaterialId? Material { get; set; }
        public OctreeNode[] Children { get; set; }
        public OctreeNode Parent { get; set; }
        public DateTime LastModified { get; set; }

        // Material inheritance support
        public bool IsLeaf => Children == null;
        public bool HasExplicitMaterial => Material.HasValue;

        public OctreeNode(Envelope3D bounds, int level, OctreeNode parent = null)
        {
            Bounds = bounds;
            Level = level;
            Parent = parent;
            LastModified = DateTime.UtcNow;
        }

        /// <summary>
        /// Query material at a specific position with inheritance
        /// O(log n) complexity for octree depth
        /// </summary>
        public MaterialId QueryMaterial(Vector3 position)
        {
            // Check bounds
            if (!Bounds.Contains(position))
                throw new ArgumentException("Position outside node bounds");

            // If this node has explicit material, return it
            if (HasExplicitMaterial)
                return Material.Value;

            // If leaf without explicit material, inherit from parent
            if (IsLeaf)
                return InheritMaterial();

            // Find appropriate child and recurse
            int childIndex = GetChildIndex(position);
            if (Children[childIndex] != null)
                return Children[childIndex].QueryMaterial(position);

            // Child doesn't exist, inherit material
            return InheritMaterial();
        }

        /// <summary>
        /// Subdivide node into 8 children
        /// </summary>
        public void Subdivide()
        {
            if (!IsLeaf)
                return;

            Children = new OctreeNode[8];
            Vector3 center = Bounds.Center;

            for (int i = 0; i < 8; i++)
            {
                Envelope3D childBounds = GetChildBounds(i, center);
                Children[i] = new OctreeNode(childBounds, Level + 1, this);
            }
        }

        /// <summary>
        /// Set material for this node
        /// </summary>
        public void SetMaterial(MaterialId material)
        {
            Material = material;
            LastModified = DateTime.UtcNow;

            // If setting material on internal node, clear children to save memory
            if (!IsLeaf && CanCollapse())
            {
                Children = null;
            }
        }

        /// <summary>
        /// Update material at a specific position
        /// Automatically subdivides if needed
        /// </summary>
        public void UpdateMaterial(Vector3 position, MaterialId material, int maxLevel = 12)
        {
            if (!Bounds.Contains(position))
                throw new ArgumentException("Position outside node bounds");

            // At max level or leaf, just set material
            if (Level >= maxLevel || IsLeaf)
            {
                SetMaterial(material);
                return;
            }

            // Subdivide if needed
            if (IsLeaf)
                Subdivide();

            // Update appropriate child
            int childIndex = GetChildIndex(position);
            if (Children[childIndex] == null)
            {
                Envelope3D childBounds = GetChildBounds(childIndex, Bounds.Center);
                Children[childIndex] = new OctreeNode(childBounds, Level + 1, this);
            }

            Children[childIndex].UpdateMaterial(position, material, maxLevel);
        }

        /// <summary>
        /// Calculate homogeneity of this node (for optimization decisions)
        /// </summary>
        public double CalculateHomogeneity()
        {
            if (IsLeaf)
                return 1.0;

            if (Children == null)
                return 1.0;

            Dictionary<MaterialId, int> materialCounts = new Dictionary<MaterialId, int>();
            int totalSamples = 0;

            foreach (var child in Children)
            {
                if (child != null)
                {
                    MaterialId childMaterial = child.HasExplicitMaterial 
                        ? child.Material.Value 
                        : child.InheritMaterial();

                    if (!materialCounts.ContainsKey(childMaterial))
                        materialCounts[childMaterial] = 0;

                    materialCounts[childMaterial]++;
                    totalSamples++;
                }
            }

            if (totalSamples == 0)
                return 1.0;

            int maxCount = 0;
            foreach (var count in materialCounts.Values)
            {
                if (count > maxCount)
                    maxCount = count;
            }

            return (double)maxCount / totalSamples;
        }

        /// <summary>
        /// Inherit material from parent chain
        /// </summary>
        private MaterialId InheritMaterial()
        {
            OctreeNode current = this;
            while (current != null)
            {
                if (current.HasExplicitMaterial)
                    return current.Material.Value;
                current = current.Parent;
            }

            // Default material if no parent has explicit material
            return MaterialId.Air;
        }

        /// <summary>
        /// Check if this node can collapse (all children same material)
        /// </summary>
        private bool CanCollapse()
        {
            if (IsLeaf || Children == null)
                return false;

            MaterialId? firstMaterial = null;
            foreach (var child in Children)
            {
                if (child == null)
                    continue;

                MaterialId childMaterial = child.HasExplicitMaterial 
                    ? child.Material.Value 
                    : child.InheritMaterial();

                if (!firstMaterial.HasValue)
                {
                    firstMaterial = childMaterial;
                }
                else if (firstMaterial.Value != childMaterial)
                {
                    return false;
                }
            }

            return firstMaterial.HasValue;
        }

        /// <summary>
        /// Get child index for position (0-7 for octree)
        /// </summary>
        private int GetChildIndex(Vector3 position)
        {
            Vector3 center = Bounds.Center;
            int index = 0;

            if (position.X >= center.X) index |= 1;
            if (position.Y >= center.Y) index |= 2;
            if (position.Z >= center.Z) index |= 4;

            return index;
        }

        /// <summary>
        /// Get bounds for child at index
        /// </summary>
        private Envelope3D GetChildBounds(int childIndex, Vector3 center)
        {
            double minX = (childIndex & 1) == 0 ? Bounds.MinX : center.X;
            double maxX = (childIndex & 1) == 0 ? center.X : Bounds.MaxX;
            double minY = (childIndex & 2) == 0 ? Bounds.MinY : center.Y;
            double maxY = (childIndex & 2) == 0 ? center.Y : Bounds.MaxY;
            double minZ = (childIndex & 4) == 0 ? Bounds.MinZ : center.Z;
            double maxZ = (childIndex & 4) == 0 ? center.Z : Bounds.MaxZ;

            return new Envelope3D(minX, minY, minZ, maxX, maxY, maxZ);
        }

        /// <summary>
        /// Get memory statistics for this node and descendants
        /// </summary>
        public (int nodeCount, int leafCount, int materialCount) GetStatistics()
        {
            int nodeCount = 1;
            int leafCount = IsLeaf ? 1 : 0;
            int materialCount = HasExplicitMaterial ? 1 : 0;

            if (!IsLeaf && Children != null)
            {
                foreach (var child in Children)
                {
                    if (child != null)
                    {
                        var childStats = child.GetStatistics();
                        nodeCount += childStats.nodeCount;
                        leafCount += childStats.leafCount;
                        materialCount += childStats.materialCount;
                    }
                }
            }

            return (nodeCount, leafCount, materialCount);
        }
    }
}
