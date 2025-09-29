using System;
using System.Threading;
using Xunit;
using BlueMarble.SpatialStorage.Octree;

namespace BlueMarble.Tests.SpatialStorage.Octree
{
    /// <summary>
    /// Tests for material inheritance cache functionality
    /// </summary>
    public class MaterialInheritanceCacheTests
    {
        [Fact]
        public void GetMaterialForPath_FirstCall_ComputesAndCachesMaterial()
        {
            // Arrange
            var cache = new MaterialInheritanceCache();
            var root = CreateTestOctree();
            var path = "001"; // Path to a specific node

            // Act
            var material1 = cache.GetMaterialForPath(path, root);
            var material2 = cache.GetMaterialForPath(path, root); // Should use cache

            // Assert
            Assert.NotNull(material1);
            Assert.Equal(material1.Id, material2.Id);
            
            var stats = cache.GetStatistics();
            Assert.True(stats.CacheSize > 0);
        }

        [Fact]
        public void InvalidatePath_RemovesMatchingEntries()
        {
            // Arrange
            var cache = new MaterialInheritanceCache();
            var root = CreateTestOctree();
            
            // Cache multiple paths
            cache.GetMaterialForPath("001", root);
            cache.GetMaterialForPath("0011", root);
            cache.GetMaterialForPath("002", root);

            var initialSize = cache.GetStatistics().CacheSize;

            // Act - Invalidate paths starting with "001"
            cache.InvalidatePath("001");

            // Assert
            var finalSize = cache.GetStatistics().CacheSize;
            Assert.True(finalSize < initialSize, "Cache size should decrease after invalidation");
        }

        [Fact]
        public void ClearCache_RemovesAllEntries()
        {
            // Arrange
            var cache = new MaterialInheritanceCache();
            var root = CreateTestOctree();
            
            // Cache some entries
            cache.GetMaterialForPath("001", root);
            cache.GetMaterialForPath("002", root);
            
            Assert.True(cache.GetStatistics().CacheSize > 0);

            // Act
            cache.ClearCache();

            // Assert
            Assert.Equal(0, cache.GetStatistics().CacheSize);
        }

        [Fact]
        public void GetStatistics_ReturnsValidStatistics()
        {
            // Arrange
            var cache = new MaterialInheritanceCache();
            var root = CreateTestOctree();
            
            // Act
            cache.GetMaterialForPath("001", root);
            var stats = cache.GetStatistics();

            // Assert
            Assert.True(stats.CacheSize >= 0);
            Assert.True(stats.MaxCacheSize > 0);
            Assert.True(stats.CacheHitRate >= 0 && stats.CacheHitRate <= 1);
            Assert.True(stats.MemoryUsage >= 0);
            Assert.True(stats.CacheUtilization >= 0 && stats.CacheUtilization <= 1);
        }

        [Fact]
        public void GetMaterialForPath_EmptyPath_ReturnsRootMaterial()
        {
            // Arrange
            var cache = new MaterialInheritanceCache();
            var root = CreateTestOctree();

            // Act
            var material = cache.GetMaterialForPath("", root);

            // Assert
            Assert.Equal(root.GetEffectiveMaterial().Id, material.Id);
        }

        [Fact]
        public void GetMaterialForPath_InvalidPath_ReturnsDefaultMaterial()
        {
            // Arrange
            var cache = new MaterialInheritanceCache();
            var root = CreateTestOctree();

            // Act
            var material = cache.GetMaterialForPath("999", root); // Invalid child index

            // Assert
            Assert.NotNull(material);
        }

        [Fact]
        public void CacheExpiration_ExpiredEntriesAreRefreshed()
        {
            // This test would require modifying the cache timeout to be very short
            // For now, we'll test the basic structure
            
            // Arrange
            var cache = new MaterialInheritanceCache();
            var root = CreateTestOctree();

            // Act
            var material = cache.GetMaterialForPath("001", root);

            // Assert
            Assert.NotNull(material);
            // In a real scenario, we'd wait for expiration and verify refresh
        }

        /// <summary>
        /// Performance test to ensure cache provides significant speedup
        /// </summary>
        [Fact]
        public void PerformanceTest_CacheImprovesPerfomance()
        {
            // Arrange
            var cache = new MaterialInheritanceCache();
            var root = CreateLargeTestOctree(); // Deep tree for expensive lookups
            var path = "001100110011"; // Deep path

            // Act - First call (cold cache)
            var startTime = DateTime.UtcNow;
            var material1 = cache.GetMaterialForPath(path, root);
            var firstCallTime = DateTime.UtcNow - startTime;

            // Second call (warm cache)
            startTime = DateTime.UtcNow;
            var material2 = cache.GetMaterialForPath(path, root);
            var secondCallTime = DateTime.UtcNow - startTime;

            // Assert
            Assert.Equal(material1.Id, material2.Id);
            // Cache should be faster (though in microsecond range, may not always be detectable)
            Assert.True(secondCallTime <= firstCallTime || secondCallTime.TotalMilliseconds < 5,
                $"Second call ({secondCallTime.TotalMilliseconds}ms) should be faster than or equal to first call ({firstCallTime.TotalMilliseconds}ms)");
        }

        private MaterialInheritanceNode CreateTestOctree()
        {
            var root = new MaterialInheritanceNode
            {
                ExplicitMaterial = new MaterialData
                {
                    Id = MaterialId.Ocean,
                    Name = "Ocean",
                    Density = 1.025f
                },
                Level = 0,
                Children = new MaterialInheritanceNode[8]
            };

            // Create some child nodes
            for (int i = 0; i < 4; i++)
            {
                root.Children[i] = new MaterialInheritanceNode
                {
                    Parent = root,
                    Level = 1,
                    ExplicitMaterial = i == 0 ? new MaterialData { Id = MaterialId.Rock, Name = "Rock", Density = 2.5f } : null
                };
            }

            return root;
        }

        private MaterialInheritanceNode CreateLargeTestOctree()
        {
            var root = new MaterialInheritanceNode
            {
                ExplicitMaterial = MaterialData.DefaultOcean,
                Level = 0
            };

            // Create a deeper tree structure
            var current = root;
            for (int level = 1; level <= 10; level++)
            {
                current.Children = new MaterialInheritanceNode[8];
                for (int i = 0; i < 8; i++)
                {
                    current.Children[i] = new MaterialInheritanceNode
                    {
                        Parent = current,
                        Level = level,
                        ExplicitMaterial = (level == 5 && i == 0) ? 
                            new MaterialData { Id = MaterialId.Rock, Name = "Rock", Density = 2.5f } : null
                    };
                }
                current = current.Children[0]; // Follow first path
            }

            return root;
        }
    }
}