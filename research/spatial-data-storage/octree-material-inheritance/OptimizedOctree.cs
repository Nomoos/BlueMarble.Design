using System;
using System.Collections.Generic;

namespace BlueMarble.SpatialStorage.Research
{
    /// <summary>
    /// Memory usage statistics for octree
    /// </summary>
    public class OctreeMemoryStats
    {
        public long TotalNodes { get; set; }
        public long NodesWithExplicitMaterial { get; set; }
        public long NodesInheritingMaterial { get; set; }
        public long CollapsedNodes { get; set; }
        public double InheritanceEfficiencyRatio { get; set; }
        public long EstimatedMemoryBytes { get; set; }
        public Dictionary<MaterialId, long> MaterialDistribution { get; set; }
        
        public OctreeMemoryStats()
        {
            MaterialDistribution = new Dictionary<MaterialId, long>();
        }
        
        public override string ToString()
        {
            return $"Memory Stats:\n" +
                   $"  Total Nodes: {TotalNodes}\n" +
                   $"  Explicit Material: {NodesWithExplicitMaterial}\n" +
                   $"  Inheriting: {NodesInheritingMaterial}\n" +
                   $"  Collapsed: {CollapsedNodes}\n" +
                   $"  Inheritance Efficiency: {InheritanceEfficiencyRatio:P2}\n" +
                   $"  Estimated Memory: {EstimatedMemoryBytes:N0} bytes";
        }
    }

    /// <summary>
    /// High-level API for optimized octree with material inheritance
    /// Provides point queries, region operations, and memory optimization
    /// </summary>
    public class OptimizedOctree
    {
        private OptimizedOctreeNode _root;
        private MaterialInheritanceCache _cache;
        private double _homogeneityThreshold = 0.9;
        
        public OptimizedOctree(BoundingBox3D bounds)
        {
            _root = new OptimizedOctreeNode
            {
                Bounds = bounds,
                Level = 0,
                ExplicitMaterial = MaterialId.Air
            };
            _cache = new MaterialInheritanceCache();
        }
        
        /// <summary>
        /// Query material at specific point
        /// </summary>
        public MaterialId QueryMaterial(double x, double y, double z)
        {
            var point = new Vector3(x, y, z);
            return _cache.GetMaterialForPoint(point, _root);
        }
        
        /// <summary>
        /// Query material at specific point with Vector3
        /// </summary>
        public MaterialId QueryMaterial(Vector3 point)
        {
            return _cache.GetMaterialForPoint(point, _root);
        }
        
        /// <summary>
        /// Set material at specific point
        /// </summary>
        public void SetMaterial(Vector3 point, MaterialId material, int maxDepth = 10)
        {
            SetMaterialRecursive(_root, point, material, maxDepth);
            _cache.InvalidatePath("");
        }
        
        private void SetMaterialRecursive(OptimizedOctreeNode node, Vector3 point, 
                                          MaterialId material, int maxDepth)
        {
            if (!node.Bounds.Contains(point))
                return;
            
            if (node.Level >= maxDepth)
            {
                var oldMaterial = node.GetEffectiveMaterial();
                node.ExplicitMaterial = material;
                node.LastModified = DateTime.UtcNow;
                
                if (node.Parent != null)
                {
                    node.Parent.UpdateChildMaterialCount(oldMaterial, material);
                }
                return;
            }
            
            // Expand collapsed node if needed
            if (node.IsCollapsed)
            {
                node.Expand();
            }
            
            // Create children if needed
            if (node.Children == null)
            {
                node.Children = new OptimizedOctreeNode[8];
                for (int i = 0; i < 8; i++)
                {
                    node.Children[i] = new OptimizedOctreeNode
                    {
                        Parent = node,
                        Level = node.Level + 1,
                        Bounds = CalculateChildBounds(node, i)
                    };
                }
            }
            
            // Find and update appropriate child
            for (int i = 0; i < 8; i++)
            {
                if (node.Children[i].Bounds.Contains(point))
                {
                    SetMaterialRecursive(node.Children[i], point, material, maxDepth);
                    break;
                }
            }
        }
        
        /// <summary>
        /// Set material for entire region
        /// </summary>
        public void SetRegion(BoundingBox3D region, MaterialId material, int maxDepth = 10)
        {
            SetRegionRecursive(_root, region, material, maxDepth);
            _cache.InvalidatePath("");
        }
        
        private void SetRegionRecursive(OptimizedOctreeNode node, BoundingBox3D region, MaterialId material, int maxDepth)
        {
            // Check if node bounds intersect with region
            if (!BoundsIntersect(node.Bounds, region))
                return;
            
            // Stop at max depth to prevent stack overflow
            if (node.Level >= maxDepth)
            {
                var oldMaterial = node.GetEffectiveMaterial();
                node.ExplicitMaterial = material;
                node.Children = null;
                node.IsCollapsed = true;
                node.LastModified = DateTime.UtcNow;
                
                if (node.Parent != null)
                {
                    node.Parent.UpdateChildMaterialCount(oldMaterial, material);
                }
                return;
            }
            
            // If node is completely within region, set material directly
            if (BoundsContains(region, node.Bounds))
            {
                var oldMaterial = node.GetEffectiveMaterial();
                node.ExplicitMaterial = material;
                node.Children = null;
                node.IsCollapsed = true;
                node.LastModified = DateTime.UtcNow;
                
                if (node.Parent != null)
                {
                    node.Parent.UpdateChildMaterialCount(oldMaterial, material);
                }
                return;
            }
            
            // Partial overlap - recurse to children
            if (node.Children == null)
            {
                node.Children = new OptimizedOctreeNode[8];
                for (int i = 0; i < 8; i++)
                {
                    node.Children[i] = new OptimizedOctreeNode
                    {
                        Parent = node,
                        Level = node.Level + 1,
                        Bounds = CalculateChildBounds(node, i)
                    };
                }
            }
            
            foreach (var child in node.Children)
            {
                if (child != null)
                {
                    SetRegionRecursive(child, region, material, maxDepth);
                }
            }
        }
        
        /// <summary>
        /// Optimize octree by collapsing homogeneous regions
        /// </summary>
        public void OptimizeMemory()
        {
            OptimizeMemoryRecursive(_root);
        }
        
        private void OptimizeMemoryRecursive(OptimizedOctreeNode node)
        {
            if (node.Children == null)
                return;
            
            // First optimize children
            foreach (var child in node.Children)
            {
                if (child != null)
                {
                    OptimizeMemoryRecursive(child);
                }
            }
            
            // Try to collapse this node
            node.TryCollapse(_homogeneityThreshold);
        }
        
        /// <summary>
        /// Calculate memory statistics for the octree
        /// </summary>
        public OctreeMemoryStats CalculateMemoryStatistics()
        {
            var stats = new OctreeMemoryStats();
            CalculateStatsRecursive(_root, stats);
            
            if (stats.TotalNodes > 0)
            {
                stats.InheritanceEfficiencyRatio = 
                    (double)stats.NodesInheritingMaterial / stats.TotalNodes;
            }
            
            // Estimate memory: 64 bytes per node with explicit material, 32 bytes for inheriting
            stats.EstimatedMemoryBytes = 
                (stats.NodesWithExplicitMaterial * 64) + 
                (stats.NodesInheritingMaterial * 32);
            
            return stats;
        }
        
        private void CalculateStatsRecursive(OptimizedOctreeNode node, OctreeMemoryStats stats)
        {
            if (node == null)
                return;
            
            stats.TotalNodes++;
            
            if (node.ExplicitMaterial.HasValue)
            {
                stats.NodesWithExplicitMaterial++;
                var material = node.ExplicitMaterial.Value;
                if (!stats.MaterialDistribution.ContainsKey(material))
                    stats.MaterialDistribution[material] = 0;
                stats.MaterialDistribution[material]++;
            }
            else
            {
                stats.NodesInheritingMaterial++;
            }
            
            if (node.IsCollapsed)
            {
                stats.CollapsedNodes++;
            }
            
            if (node.Children != null)
            {
                foreach (var child in node.Children)
                {
                    if (child != null)
                    {
                        CalculateStatsRecursive(child, stats);
                    }
                }
            }
        }
        
        private BoundingBox3D CalculateChildBounds(OptimizedOctreeNode parent, int childIndex)
        {
            var center = parent.Bounds.Center;
            var size = parent.Bounds.Size;
            var childSize = new Vector3(size.X * 0.5, size.Y * 0.5, size.Z * 0.5);
            
            var childMin = new Vector3(
                (childIndex & 1) == 0 ? parent.Bounds.Min.X : center.X,
                (childIndex & 2) == 0 ? parent.Bounds.Min.Y : center.Y,
                (childIndex & 4) == 0 ? parent.Bounds.Min.Z : center.Z
            );
            
            return new BoundingBox3D(
                childMin,
                new Vector3(childMin.X + childSize.X, childMin.Y + childSize.Y, childMin.Z + childSize.Z)
            );
        }
        
        private bool BoundsIntersect(BoundingBox3D a, BoundingBox3D b)
        {
            return a.Min.X <= b.Max.X && a.Max.X >= b.Min.X &&
                   a.Min.Y <= b.Max.Y && a.Max.Y >= b.Min.Y &&
                   a.Min.Z <= b.Max.Z && a.Max.Z >= b.Min.Z;
        }
        
        private bool BoundsContains(BoundingBox3D outer, BoundingBox3D inner)
        {
            return outer.Min.X <= inner.Min.X && outer.Max.X >= inner.Max.X &&
                   outer.Min.Y <= inner.Min.Y && outer.Max.Y >= inner.Max.Y &&
                   outer.Min.Z <= inner.Min.Z && outer.Max.Z >= inner.Max.Z;
        }
        
        public CacheStatistics GetCacheStatistics()
        {
            return _cache.GetStatistics();
        }
    }
}
