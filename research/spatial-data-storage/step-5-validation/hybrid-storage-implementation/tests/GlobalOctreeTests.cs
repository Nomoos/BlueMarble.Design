using System;
using System.Collections.Generic;
using Xunit;

namespace BlueMarble.SpatialStorage.Tests
{
    /// <summary>
    /// Test suite for GlobalOctree (15 tests)
    /// Covers octree operations, material inheritance, and performance
    /// </summary>
    public class GlobalOctreeTests
    {
        [Fact]
        public void Test01_Constructor_InitializesCorrectly()
        {
            // Arrange & Act
            var octree = new GlobalOctree();

            // Assert
            var stats = octree.GetStatistics();
            Assert.Equal(1, stats.TotalNodes); // Root node only
            Assert.Equal(1, stats.LeafNodes);
            Assert.Equal(0, stats.QueryCount);
        }

        [Fact]
        public void Test02_QueryMaterial_ReturnsDefaultForUninitializedRegion()
        {
            // Arrange
            var octree = new GlobalOctree();
            var position = new Vector3(1000, 1000, 0);

            // Act
            var material = octree.QueryMaterial(position);

            // Assert
            Assert.Equal(MaterialId.Air, material); // Default material
        }

        [Fact]
        public void Test03_UpdateMaterial_SetsAndRetrieves()
        {
            // Arrange
            var octree = new GlobalOctree();
            var position = new Vector3(1000, 1000, 0);

            // Act
            octree.UpdateMaterial(position, MaterialId.Sand);
            var material = octree.QueryMaterial(position);

            // Assert
            Assert.Equal(MaterialId.Sand, material);
        }

        [Fact]
        public void Test04_MaterialInheritance_InheritsFromParent()
        {
            // Arrange
            var octree = new GlobalOctree();
            var parentPosition = new Vector3(0, 0, 0);
            var childPosition = new Vector3(100, 100, 0);

            // Act
            octree.UpdateMaterial(parentPosition, MaterialId.Ocean);
            var childMaterial = octree.QueryMaterial(childPosition);

            // Assert - Child inherits from parent
            Assert.Equal(MaterialId.Ocean, childMaterial);
        }

        [Fact]
        public void Test05_Subdivision_CreatesChildNodes()
        {
            // Arrange
            var octree = new GlobalOctree();
            var position1 = new Vector3(1000, 1000, 0);
            var position2 = new Vector3(2000, 2000, 0);

            // Act - Force subdivision by updating different positions
            octree.UpdateMaterial(position1, MaterialId.Sand);
            octree.UpdateMaterial(position2, MaterialId.Rock);

            // Assert
            var stats = octree.GetStatistics();
            Assert.True(stats.TotalNodes > 1);
        }

        [Fact]
        public void Test06_MultipleUpdates_MaintainsCorrectMaterials()
        {
            // Arrange
            var octree = new GlobalOctree();
            var positions = new List<(Vector3, MaterialId)>
            {
                (new Vector3(1000, 1000, 0), MaterialId.Sand),
                (new Vector3(2000, 2000, 0), MaterialId.Rock),
                (new Vector3(3000, 3000, 0), MaterialId.Clay)
            };

            // Act
            foreach (var (pos, mat) in positions)
            {
                octree.UpdateMaterial(pos, mat);
            }

            // Assert
            foreach (var (pos, expectedMat) in positions)
            {
                var actualMat = octree.QueryMaterial(pos);
                Assert.Equal(expectedMat, actualMat);
            }
        }

        [Fact]
        public void Test07_BatchUpdate_UpdatesMultiplePositions()
        {
            // Arrange
            var octree = new GlobalOctree();
            var updates = new List<(Vector3, MaterialId)>
            {
                (new Vector3(1000, 1000, 0), MaterialId.Sand),
                (new Vector3(2000, 2000, 0), MaterialId.Rock),
                (new Vector3(3000, 3000, 0), MaterialId.Clay)
            };

            // Act
            octree.UpdateMaterialBatch(updates);

            // Assert
            foreach (var (pos, expectedMat) in updates)
            {
                Assert.Equal(expectedMat, octree.QueryMaterial(pos));
            }
        }

        [Fact]
        public void Test08_OceanInitialization_SetsLargeRegion()
        {
            // Arrange
            var octree = new GlobalOctree();
            var oceanRegion = new Envelope3D(-10000, -10000, -1000, 10000, 10000, 0);

            // Act
            octree.InitializeOceanRegion(oceanRegion);

            // Assert
            var testPoint = new Vector3(5000, 5000, -500);
            var material = octree.QueryMaterial(testPoint);
            Assert.Equal(MaterialId.Ocean, material);
        }

        [Fact]
        public void Test09_CacheHit_ImprovesPerformance()
        {
            // Arrange
            var octree = new GlobalOctree();
            var position = new Vector3(1000, 1000, 0);
            octree.UpdateMaterial(position, MaterialId.Sand);

            // Act - Query same position multiple times
            for (int i = 0; i < 10; i++)
            {
                octree.QueryMaterial(position);
            }

            // Assert
            var stats = octree.GetStatistics();
            Assert.True(stats.CacheHitRate > 0.5); // At least 50% cache hits
        }

        [Fact]
        public void Test10_QueryRegion_ReturnsAllMaterials()
        {
            // Arrange
            var octree = new GlobalOctree();
            var region = new Envelope3D(0, 0, 0, 2000, 2000, 100);
            
            octree.UpdateMaterial(new Vector3(500, 500, 50), MaterialId.Sand);
            octree.UpdateMaterial(new Vector3(1500, 1500, 50), MaterialId.Rock);

            // Act
            var results = octree.QueryRegion(region, 500);

            // Assert
            Assert.NotEmpty(results);
            Assert.Contains(results, r => r.material == MaterialId.Sand);
        }

        [Fact]
        public void Test11_MemorySavings_DemonstratesInheritance()
        {
            // Arrange
            var octree = new GlobalOctree();
            var largeRegion = new Envelope3D(-100000, -100000, -1000, 100000, 100000, 0);

            // Act - Initialize with single material
            octree.InitializeOceanRegion(largeRegion);
            var stats = octree.GetStatistics();

            // Assert - Should have high memory savings due to inheritance
            Assert.True(stats.MemorySavingsPercent > 50);
        }

        [Fact]
        public void Test12_MaxLevel_LimitsSubdivision()
        {
            // Arrange
            var octree = new GlobalOctree(maxLevel: 5);
            var position = new Vector3(1000, 1000, 0);

            // Act - Try to force deep subdivision
            for (int i = 0; i < 10; i++)
            {
                octree.UpdateMaterial(position, MaterialId.Sand);
            }

            // Assert - Should not exceed max level
            var stats = octree.GetStatistics();
            // With max level 5, maximum nodes = 1 + 8 + 64 + 512 + 4096 + 32768 = 37449
            Assert.True(stats.TotalNodes < 40000);
        }

        [Fact]
        public void Test13_ClearCache_ResetsCache()
        {
            // Arrange
            var octree = new GlobalOctree();
            var position = new Vector3(1000, 1000, 0);
            octree.UpdateMaterial(position, MaterialId.Sand);
            octree.QueryMaterial(position);

            // Act
            octree.ClearCache();
            var statsAfterClear = octree.GetStatistics();

            // Assert
            Assert.Equal(0, statsAfterClear.CacheSize);
        }

        [Fact]
        public void Test14_GetResolutionAtLevel_CalculatesCorrectly()
        {
            // Arrange & Act
            double level0 = GlobalOctree.GetResolutionAtLevel(0);
            double level12 = GlobalOctree.GetResolutionAtLevel(12);

            // Assert
            Assert.Equal(GlobalOctree.WORLD_SIZE, level0);
            Assert.True(level12 < level0);
            Assert.True(level12 > 0);
        }

        [Fact]
        public void Test15_GetLevelForResolution_CalculatesCorrectly()
        {
            // Arrange & Act
            int levelFor1m = GlobalOctree.GetLevelForResolution(1.0);
            int levelFor100m = GlobalOctree.GetLevelForResolution(100.0);

            // Assert
            Assert.True(levelFor1m > levelFor100m);
            Assert.True(levelFor1m >= 12); // ~1m is around level 12
        }
    }
}
