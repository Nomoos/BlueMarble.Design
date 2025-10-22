using System;

namespace BlueMarble.Utils.Spatial.DistributedStorage
{
    /// <summary>
    /// Configuration options for distributed octree storage.
    /// </summary>
    public class DistributedOctreeConfig
    {
        /// <summary>
        /// World spatial bounds for the octree
        /// </summary>
        public SpatialBounds WorldBounds { get; set; }

        /// <summary>
        /// Maximum cache size (number of nodes)
        /// </summary>
        public int MaxCacheSize { get; set; } = 10000;

        /// <summary>
        /// Replication factor for fault tolerance
        /// </summary>
        public int ReplicationFactor { get; set; } = 3;

        /// <summary>
        /// Default consistency level for operations
        /// </summary>
        public ConsistencyLevel DefaultConsistency { get; set; } = ConsistencyLevel.Quorum;

        /// <summary>
        /// Homogeneity threshold for material compression (0.0 to 1.0)
        /// </summary>
        public double HomogeneityThreshold { get; set; } = 0.9;

        /// <summary>
        /// Maximum octree depth level
        /// </summary>
        public int MaxLevel { get; set; } = 20;

        /// <summary>
        /// Enable caching for frequently accessed regions
        /// </summary>
        public bool EnableCaching { get; set; } = true;

        /// <summary>
        /// Create default configuration for BlueMarble world dimensions.
        /// Uses Earth-like coordinate system with NetTopologySuite compatibility.
        /// </summary>
        public static DistributedOctreeConfig CreateDefault()
        {
            return new DistributedOctreeConfig
            {
                // Earth dimensions: longitude [-180, 180], latitude [-90, 90], elevation [-11000m, 9000m]
                WorldBounds = new SpatialBounds(-180, -90, -11000, 180, 90, 9000),
                MaxCacheSize = 10000,
                ReplicationFactor = 3,
                DefaultConsistency = ConsistencyLevel.Quorum,
                HomogeneityThreshold = 0.9,
                MaxLevel = 20,
                EnableCaching = true
            };
        }

        /// <summary>
        /// Create configuration for testing with smaller bounds.
        /// </summary>
        public static DistributedOctreeConfig CreateForTesting()
        {
            return new DistributedOctreeConfig
            {
                WorldBounds = new SpatialBounds(0, 0, 0, 1000, 1000, 1000),
                MaxCacheSize = 100,
                ReplicationFactor = 2,
                DefaultConsistency = ConsistencyLevel.One,
                HomogeneityThreshold = 0.9,
                MaxLevel = 10,
                EnableCaching = true
            };
        }
    }

    /// <summary>
    /// Storage type for factory creation.
    /// </summary>
    public enum StorageType
    {
        /// <summary>
        /// In-memory storage (development and testing)
        /// </summary>
        InMemory,

        /// <summary>
        /// Redis-backed distributed storage (future implementation)
        /// </summary>
        Redis,

        /// <summary>
        /// Cassandra-backed persistent storage (future implementation)
        /// </summary>
        Cassandra
    }

    /// <summary>
    /// Factory for creating distributed octree storage instances with pre-configured settings.
    /// </summary>
    public static class DistributedOctreeFactory
    {
        /// <summary>
        /// Create a storage instance with the specified type and configuration.
        /// </summary>
        public static IDistributedOctreeStorage CreateStorage(
            StorageType storageType,
            DistributedOctreeConfig config = null)
        {
            config ??= DistributedOctreeConfig.CreateDefault();

            return storageType switch
            {
                StorageType.InMemory => CreateInMemoryStorage(config),
                StorageType.Redis => throw new NotImplementedException("Redis storage not yet implemented"),
                StorageType.Cassandra => throw new NotImplementedException("Cassandra storage not yet implemented"),
                _ => throw new ArgumentException($"Unknown storage type: {storageType}")
            };
        }

        /// <summary>
        /// Create an in-memory storage instance suitable for development and testing.
        /// </summary>
        public static IDistributedOctreeStorage CreateInMemoryStorage(DistributedOctreeConfig config = null)
        {
            config ??= DistributedOctreeConfig.CreateDefault();

            if (config.WorldBounds == null)
            {
                throw new ArgumentException("WorldBounds must be specified in configuration");
            }

            return new InMemoryDistributedOctreeStorage(
                config.WorldBounds,
                config.MaxCacheSize,
                config.ReplicationFactor
            );
        }

        /// <summary>
        /// Create a storage instance with default configuration for BlueMarble.
        /// </summary>
        public static IDistributedOctreeStorage CreateDefault()
        {
            return CreateInMemoryStorage(DistributedOctreeConfig.CreateDefault());
        }

        /// <summary>
        /// Create a storage instance optimized for testing.
        /// </summary>
        public static IDistributedOctreeStorage CreateForTesting()
        {
            return CreateInMemoryStorage(DistributedOctreeConfig.CreateForTesting());
        }
    }
}
