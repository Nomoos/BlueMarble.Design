using System;
using System.Collections.Generic;

namespace BlueMarble.Utils.Spatial.DistributedStorage
{
    /// <summary>
    /// Represents a 3D octree node with material data, spatial bounds, and distributed hash keys.
    /// Supports material inheritance and homogeneity-based compression.
    /// </summary>
    public class SpatialOctreeNode
    {
        /// <summary>
        /// Unique identifier for the node
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Octree level (0 = root, higher = finer resolution)
        /// </summary>
        public int Level { get; set; }

        /// <summary>
        /// Morton code for spatial hash distribution
        /// </summary>
        public ulong MortonCode { get; set; }

        /// <summary>
        /// Spatial bounds of the node (min and max coordinates)
        /// </summary>
        public SpatialBounds Bounds { get; set; }

        /// <summary>
        /// Material identifier for this node
        /// </summary>
        public int MaterialId { get; set; }

        /// <summary>
        /// Material homogeneity (0.0 to 1.0, 1.0 = uniform material)
        /// </summary>
        public double Homogeneity { get; set; }

        /// <summary>
        /// Whether this is a leaf node
        /// </summary>
        public bool IsLeaf { get; set; }

        /// <summary>
        /// Child node IDs (8 children for 3D octree)
        /// </summary>
        public string[] ChildNodeIds { get; set; }

        /// <summary>
        /// Parent node ID
        /// </summary>
        public string ParentNodeId { get; set; }

        /// <summary>
        /// Version for optimistic concurrency control
        /// </summary>
        public long Version { get; set; }

        /// <summary>
        /// Last modified timestamp
        /// </summary>
        public DateTime LastModified { get; set; }

        /// <summary>
        /// Distributed hash key for cluster routing
        /// </summary>
        public string DistributedHashKey { get; set; }

        /// <summary>
        /// Replica node hashes for fault tolerance
        /// </summary>
        public List<ulong> ReplicaHashes { get; set; }

        public SpatialOctreeNode()
        {
            ChildNodeIds = new string[8];
            ReplicaHashes = new List<ulong>();
            LastModified = DateTime.UtcNow;
            Version = 0;
        }

        /// <summary>
        /// Check if node should be subdivided based on homogeneity threshold
        /// </summary>
        public bool ShouldSubdivide(double homogeneityThreshold = 0.9)
        {
            return !IsLeaf && Homogeneity < homogeneityThreshold;
        }

        /// <summary>
        /// Check if node can be compressed (inherited from parent)
        /// </summary>
        public bool CanCompress(double homogeneityThreshold = 0.9)
        {
            return Homogeneity >= homogeneityThreshold;
        }
    }

    /// <summary>
    /// Represents 3D spatial bounds with min/max coordinates
    /// </summary>
    public class SpatialBounds
    {
        public double MinX { get; set; }
        public double MinY { get; set; }
        public double MinZ { get; set; }
        public double MaxX { get; set; }
        public double MaxY { get; set; }
        public double MaxZ { get; set; }

        public SpatialBounds()
        {
        }

        public SpatialBounds(double minX, double minY, double minZ, double maxX, double maxY, double maxZ)
        {
            MinX = minX;
            MinY = minY;
            MinZ = minZ;
            MaxX = maxX;
            MaxY = maxY;
            MaxZ = maxZ;
        }

        /// <summary>
        /// Get the size of the bounds in each dimension
        /// </summary>
        public (double X, double Y, double Z) GetSize()
        {
            return (MaxX - MinX, MaxY - MinY, MaxZ - MinZ);
        }

        /// <summary>
        /// Get the center point of the bounds
        /// </summary>
        public (double X, double Y, double Z) GetCenter()
        {
            return ((MinX + MaxX) / 2, (MinY + MaxY) / 2, (MinZ + MaxZ) / 2);
        }

        /// <summary>
        /// Check if a point is within the bounds
        /// </summary>
        public bool Contains(double x, double y, double z)
        {
            return x >= MinX && x <= MaxX &&
                   y >= MinY && y <= MaxY &&
                   z >= MinZ && z <= MaxZ;
        }

        /// <summary>
        /// Check if this bounds intersects with another
        /// </summary>
        public bool Intersects(SpatialBounds other)
        {
            return !(MaxX < other.MinX || MinX > other.MaxX ||
                     MaxY < other.MinY || MinY > other.MaxY ||
                     MaxZ < other.MinZ || MinZ > other.MaxZ);
        }
    }
}
