using System;
using Xunit;
using BlueMarble.Utils.Spatial.DistributedStorage;

namespace BlueMarble.Utils.Spatial.DistributedStorage.Tests
{
    public class DistributedOctreeFactoryTests
    {
        [Fact]
        public void CreateStorage_WithInMemoryType_ReturnsInMemoryStorage()
        {
            var config = DistributedOctreeConfig.CreateForTesting();
            var storage = DistributedOctreeFactory.CreateStorage(StorageType.InMemory, config);

            Assert.NotNull(storage);
            Assert.IsAssignableFrom<IDistributedOctreeStorage>(storage);
        }

        [Fact]
        public void CreateStorage_WithRedisType_ThrowsNotImplementedException()
        {
            var config = DistributedOctreeConfig.CreateForTesting();
            
            Assert.Throws<NotImplementedException>(
                () => DistributedOctreeFactory.CreateStorage(StorageType.Redis, config));
        }

        [Fact]
        public void CreateStorage_WithCassandraType_ThrowsNotImplementedException()
        {
            var config = DistributedOctreeConfig.CreateForTesting();
            
            Assert.Throws<NotImplementedException>(
                () => DistributedOctreeFactory.CreateStorage(StorageType.Cassandra, config));
        }

        [Fact]
        public void CreateInMemoryStorage_WithValidConfig_Succeeds()
        {
            var config = DistributedOctreeConfig.CreateForTesting();
            var storage = DistributedOctreeFactory.CreateInMemoryStorage(config);

            Assert.NotNull(storage);
        }

        [Fact]
        public void CreateInMemoryStorage_WithNullConfig_UsesDefaultConfig()
        {
            var storage = DistributedOctreeFactory.CreateInMemoryStorage(null);

            Assert.NotNull(storage);
        }

        [Fact]
        public void CreateInMemoryStorage_WithNullWorldBounds_ThrowsException()
        {
            var config = new DistributedOctreeConfig
            {
                WorldBounds = null
            };

            Assert.Throws<ArgumentException>(
                () => DistributedOctreeFactory.CreateInMemoryStorage(config));
        }

        [Fact]
        public void CreateDefault_ReturnsInMemoryStorage()
        {
            var storage = DistributedOctreeFactory.CreateDefault();

            Assert.NotNull(storage);
            Assert.IsAssignableFrom<IDistributedOctreeStorage>(storage);
        }

        [Fact]
        public void CreateForTesting_ReturnsInMemoryStorage()
        {
            var storage = DistributedOctreeFactory.CreateForTesting();

            Assert.NotNull(storage);
            Assert.IsAssignableFrom<IDistributedOctreeStorage>(storage);
        }
    }

    public class DistributedOctreeConfigTests
    {
        [Fact]
        public void CreateDefault_HasCorrectWorldBounds()
        {
            var config = DistributedOctreeConfig.CreateDefault();

            Assert.NotNull(config.WorldBounds);
            Assert.Equal(-180, config.WorldBounds.MinX);
            Assert.Equal(-90, config.WorldBounds.MinY);
            Assert.Equal(-11000, config.WorldBounds.MinZ);
            Assert.Equal(180, config.WorldBounds.MaxX);
            Assert.Equal(90, config.WorldBounds.MaxY);
            Assert.Equal(9000, config.WorldBounds.MaxZ);
        }

        [Fact]
        public void CreateDefault_HasReasonableDefaults()
        {
            var config = DistributedOctreeConfig.CreateDefault();

            Assert.Equal(10000, config.MaxCacheSize);
            Assert.Equal(3, config.ReplicationFactor);
            Assert.Equal(ConsistencyLevel.Quorum, config.DefaultConsistency);
            Assert.Equal(0.9, config.HomogeneityThreshold);
            Assert.Equal(20, config.MaxLevel);
            Assert.True(config.EnableCaching);
        }

        [Fact]
        public void CreateForTesting_HasSmallerBounds()
        {
            var config = DistributedOctreeConfig.CreateForTesting();

            Assert.NotNull(config.WorldBounds);
            Assert.Equal(0, config.WorldBounds.MinX);
            Assert.Equal(1000, config.WorldBounds.MaxX);
        }

        [Fact]
        public void CreateForTesting_HasTestOptimizedSettings()
        {
            var config = DistributedOctreeConfig.CreateForTesting();

            Assert.Equal(100, config.MaxCacheSize);
            Assert.Equal(2, config.ReplicationFactor);
            Assert.Equal(ConsistencyLevel.One, config.DefaultConsistency);
            Assert.Equal(10, config.MaxLevel);
        }

        [Fact]
        public void Config_CanBeCustomized()
        {
            var config = new DistributedOctreeConfig
            {
                WorldBounds = new SpatialBounds(0, 0, 0, 500, 500, 500),
                MaxCacheSize = 5000,
                ReplicationFactor = 5,
                DefaultConsistency = ConsistencyLevel.All,
                HomogeneityThreshold = 0.8,
                MaxLevel = 15,
                EnableCaching = false
            };

            Assert.Equal(5000, config.MaxCacheSize);
            Assert.Equal(5, config.ReplicationFactor);
            Assert.Equal(ConsistencyLevel.All, config.DefaultConsistency);
            Assert.Equal(0.8, config.HomogeneityThreshold);
            Assert.Equal(15, config.MaxLevel);
            Assert.False(config.EnableCaching);
        }
    }
}
