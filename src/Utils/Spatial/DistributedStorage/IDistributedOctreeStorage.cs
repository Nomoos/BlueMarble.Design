using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BlueMarble.Utils.Spatial.DistributedStorage
{
    /// <summary>
    /// Consistency level for distributed operations.
    /// </summary>
    public enum ConsistencyLevel
    {
        /// <summary>
        /// Return after 1 replica acknowledges (fastest, less durable)
        /// </summary>
        One,

        /// <summary>
        /// Return after majority of replicas acknowledge (balanced)
        /// </summary>
        Quorum,

        /// <summary>
        /// Return after all replicas acknowledge (slowest, most durable)
        /// </summary>
        All
    }

    /// <summary>
    /// Result of a query operation.
    /// </summary>
    public class QueryResult
    {
        public SpatialOctreeNode Node { get; set; }
        public bool Success { get; set; }
        public string Source { get; set; }
        public TimeSpan Latency { get; set; }
        public int MaterialId { get; set; }
    }

    /// <summary>
    /// Result of a write operation.
    /// </summary>
    public class WriteResult
    {
        public bool Success { get; set; }
        public long NewVersion { get; set; }
        public string ErrorMessage { get; set; }
        public int AcknowledgedReplicas { get; set; }
    }

    /// <summary>
    /// Interface defining storage operations for distributed octree with fault tolerance
    /// and consistency guarantees.
    /// </summary>
    public interface IDistributedOctreeStorage
    {
        /// <summary>
        /// Query material at a specific 3D position and level of detail.
        /// </summary>
        Task<QueryResult> QueryMaterialAsync(
            double x, 
            double y, 
            double z, 
            int lod, 
            ConsistencyLevel consistency = ConsistencyLevel.Quorum,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Get an octree node by its ID.
        /// </summary>
        Task<SpatialOctreeNode> GetNodeAsync(
            string nodeId, 
            ConsistencyLevel consistency = ConsistencyLevel.Quorum,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Get a node by Morton code and level.
        /// </summary>
        Task<SpatialOctreeNode> GetNodeByMortonAsync(
            ulong mortonCode, 
            int level,
            ConsistencyLevel consistency = ConsistencyLevel.Quorum,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Write or update an octree node with optimistic concurrency control.
        /// </summary>
        Task<WriteResult> WriteNodeAsync(
            SpatialOctreeNode node, 
            ConsistencyLevel consistency = ConsistencyLevel.Quorum,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Delete an octree node.
        /// </summary>
        Task<bool> DeleteNodeAsync(
            string nodeId, 
            ConsistencyLevel consistency = ConsistencyLevel.Quorum,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Query nodes within a spatial region.
        /// </summary>
        Task<List<SpatialOctreeNode>> QueryRegionAsync(
            SpatialBounds bounds, 
            int maxLevel,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Get storage statistics.
        /// </summary>
        Task<StorageStatistics> GetStatisticsAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Clear all cached data (for testing purposes).
        /// </summary>
        Task ClearCacheAsync(CancellationToken cancellationToken = default);
    }

    /// <summary>
    /// Storage statistics for monitoring and diagnostics.
    /// </summary>
    public class StorageStatistics
    {
        public long TotalNodes { get; set; }
        public long CacheHits { get; set; }
        public long CacheMisses { get; set; }
        public long TotalQueries { get; set; }
        public long TotalWrites { get; set; }
        public double AverageQueryLatencyMs { get; set; }
        public double CacheHitRate => TotalQueries > 0 ? (double)CacheHits / TotalQueries : 0;
        public Dictionary<string, long> NodesByLevel { get; set; }

        public StorageStatistics()
        {
            NodesByLevel = new Dictionary<string, long>();
        }
    }
}
