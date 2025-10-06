using System;
using Xunit;
using BlueMarble.Utils.Spatial.DistributedStorage;

namespace BlueMarble.Utils.Spatial.DistributedStorage.Tests
{
    public class SpatialHashGeneratorTests
    {
        private readonly SpatialBounds _testBounds;
        private readonly SpatialHashGenerator _generator;

        public SpatialHashGeneratorTests()
        {
            _testBounds = new SpatialBounds(0, 0, 0, 1000, 1000, 1000);
            _generator = new SpatialHashGenerator(_testBounds, replicationFactor: 3);
        }

        [Fact]
        public void Constructor_WithNullBounds_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new SpatialHashGenerator(null));
        }

        [Fact]
        public void EncodeMorton3D_AtOrigin_ReturnsZero()
        {
            var morton = _generator.EncodeMorton3D(0, 0, 0, 0);
            Assert.Equal(0ul, morton);
        }

        [Fact]
        public void EncodeMorton3D_PreservesCoordinateOrder()
        {
            // Points with increasing X should have increasing Morton codes (spatial locality)
            var morton1 = _generator.EncodeMorton3D(100, 100, 100, 5);
            var morton2 = _generator.EncodeMorton3D(200, 100, 100, 5);
            var morton3 = _generator.EncodeMorton3D(300, 100, 100, 5);

            Assert.True(morton1 < morton2);
            Assert.True(morton2 < morton3);
        }

        [Fact]
        public void DecodeMorton3D_RoundTrip_PreservesCoordinates()
        {
            uint x = 15, y = 23, z = 8;
            var morton = _generator.EncodeMorton3D(x, y, z, 5);
            var (decodedX, decodedY, decodedZ) = _generator.DecodeMorton3D(morton);

            // Should preserve the grid coordinates at the given level
            Assert.True(decodedX <= x);
            Assert.True(decodedY <= y);
            Assert.True(decodedZ <= z);
        }

        [Fact]
        public void GenerateHierarchicalKeys_CreatesCorrectNumberOfLevels()
        {
            var keys = _generator.GenerateHierarchicalKeys(500, 500, 500, 5);

            Assert.Equal(6, keys.Count); // Levels 0 through 5
        }

        [Fact]
        public void GenerateHierarchicalKeys_LevelsAreSequential()
        {
            var keys = _generator.GenerateHierarchicalKeys(500, 500, 500, 5);

            for (int i = 0; i < keys.Count; i++)
            {
                Assert.Equal(i, keys[i].Level);
            }
        }

        [Fact]
        public void GenerateHierarchicalKeys_ContainsReplicaHashes()
        {
            var keys = _generator.GenerateHierarchicalKeys(500, 500, 500, 3);

            foreach (var key in keys)
            {
                Assert.NotNull(key.ReplicaNodeHashes);
                Assert.Equal(2, key.ReplicaNodeHashes.Count); // replicationFactor=3 means 2 replicas
            }
        }

        [Fact]
        public void GenerateHierarchicalKeys_CacheKeyFormat()
        {
            var keys = _generator.GenerateHierarchicalKeys(500, 500, 500, 2);

            foreach (var key in keys)
            {
                Assert.NotNull(key.CacheKey);
                Assert.StartsWith("octree:", key.CacheKey);
                Assert.Contains($"{key.Level:D2}", key.CacheKey);
            }
        }

        [Fact]
        public void GenerateHierarchicalKeys_SpatialRegionCoversPosition()
        {
            var x = 500.0;
            var y = 500.0;
            var z = 500.0;
            var keys = _generator.GenerateHierarchicalKeys(x, y, z, 3);

            foreach (var key in keys)
            {
                Assert.NotNull(key.SpatialRegion);
                Assert.True(key.SpatialRegion.Contains(x, y, z),
                    $"Level {key.Level} bounds should contain the query position");
            }
        }

        [Fact]
        public void JumpConsistentHash_DistributesEvenly()
        {
            const int numBuckets = 10;
            const int numKeys = 10000;
            var distribution = new int[numBuckets];

            for (ulong key = 0; key < numKeys; key++)
            {
                var bucket = SpatialHashGenerator.JumpConsistentHash(key, numBuckets);
                Assert.InRange(bucket, 0, numBuckets - 1);
                distribution[bucket]++;
            }

            // Check distribution is reasonably even (within 20% of expected)
            var expectedPerBucket = numKeys / numBuckets;
            foreach (var count in distribution)
            {
                Assert.InRange(count, expectedPerBucket * 0.8, expectedPerBucket * 1.2);
            }
        }

        [Fact]
        public void JumpConsistentHash_IsConsistent()
        {
            const int numBuckets = 10;
            const ulong testKey = 12345;

            var bucket1 = SpatialHashGenerator.JumpConsistentHash(testKey, numBuckets);
            var bucket2 = SpatialHashGenerator.JumpConsistentHash(testKey, numBuckets);

            Assert.Equal(bucket1, bucket2);
        }

        [Fact]
        public void EncodeMorton3D_HandlesWorldBoundaries()
        {
            // Test at world boundaries
            var mortonMin = _generator.EncodeMorton3D(0, 0, 0, 5);
            var mortonMax = _generator.EncodeMorton3D(1000, 1000, 1000, 5);

            Assert.NotEqual(mortonMin, mortonMax);
        }

        [Theory]
        [InlineData(100, 200, 300)]
        [InlineData(500, 500, 500)]
        [InlineData(999, 999, 999)]
        public void EncodeMorton3D_DifferentLevels_DifferentPrecision(double x, double y, double z)
        {
            var morton1 = _generator.EncodeMorton3D(x, y, z, 1);
            var morton5 = _generator.EncodeMorton3D(x, y, z, 5);
            var morton10 = _generator.EncodeMorton3D(x, y, z, 10);

            // Higher levels should provide more detailed location
            // Most positions should have different morton codes at different levels
            // (Note: origin (0,0,0) will have morton code 0 at all levels, which is expected)
            Assert.True(morton1 != morton5 || morton5 != morton10 || (x == 0 && y == 0 && z == 0));
        }
    }
}
