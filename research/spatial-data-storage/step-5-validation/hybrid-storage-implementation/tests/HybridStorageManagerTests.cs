using System;
using System.Collections.Generic;
using Xunit;

namespace BlueMarble.SpatialStorage.Tests
{
    /// <summary>
    /// Test suite for HybridStorageManager (14 tests)
    /// Covers hybrid coordination, LOD transitions, and cache management
    /// </summary>
    public class HybridStorageManagerTests
    {
        [Fact]
        public void Test01_Constructor_InitializesCorrectly()
        {
            // Arrange & Act
            var manager = new HybridStorageManager();

            // Assert
            var stats = manager.GetStatistics();
            Assert.Equal(12, stats.TransitionLevel);
            Assert.Equal(0, stats.TotalQueries);
        }

        [Fact]
        public void Test02_QueryMaterial_RoutesToOctreeForCoarseLOD()
        {
            // Arrange
            var manager = new HybridStorageManager();
            var position = new Vector3(1000, 1000, 0);
            manager.UpdateMaterial(position, MaterialId.Sand, 10);

            // Act
            var material = manager.QueryMaterial(position, 10);

            // Assert
            Assert.Equal(MaterialId.Sand, material);
            var stats = manager.GetStatistics();
            Assert.True(stats.OctreeQueries > 0);
            Assert.Equal(0, stats.GridQueries);
        }

        [Fact]
        public void Test03_QueryMaterial_RoutesToGridForFineLOD()
        {
            // Arrange
            var manager = new HybridStorageManager();
            var position = new Vector3(1000, 1000, 0);
            manager.UpdateMaterial(position, MaterialId.Rock, 10);

            // Act
            var material = manager.QueryMaterial(position, 13);

            // Assert
            var stats = manager.GetStatistics();
            Assert.True(stats.GridQueries > 0);
            Assert.True(stats.ActiveTiles > 0);
        }

        [Fact]
        public void Test04_TransitionLevel_CorrectlyRoutesQueries()
        {
            // Arrange
            var manager = new HybridStorageManager(transitionLevel: 12);
            var position = new Vector3(1000, 1000, 0);

            // Act
            manager.QueryMaterial(position, 11); // Octree
            manager.QueryMaterial(position, 12); // Octree
            manager.QueryMaterial(position, 13); // Grid

            // Assert
            var stats = manager.GetStatistics();
            Assert.Equal(2, stats.OctreeQueries);
            Assert.Equal(1, stats.GridQueries);
        }

        [Fact]
        public void Test05_UpdateMaterial_UpdatesCorrectStorage()
        {
            // Arrange
            var manager = new HybridStorageManager();
            var position = new Vector3(1000, 1000, 0);

            // Act
            manager.UpdateMaterial(position, MaterialId.Sand, 10);
            var octreeMaterial = manager.QueryMaterial(position, 10);

            manager.UpdateMaterial(position, MaterialId.Rock, 13);
            var gridMaterial = manager.QueryMaterial(position, 13);

            // Assert
            Assert.Equal(MaterialId.Sand, octreeMaterial);
            Assert.Equal(MaterialId.Rock, gridMaterial);
        }

        [Fact]
        public void Test06_QueryRegion_ReturnsMultipleResults()
        {
            // Arrange
            var manager = new HybridStorageManager();
            var region = new Envelope3D(0, 0, 0, 1000, 1000, 100);
            manager.UpdateMaterial(new Vector3(500, 500, 50), MaterialId.Sand, 10);

            // Act
            var results = manager.QueryRegion(region, 10, 100);

            // Assert
            Assert.NotEmpty(results);
        }

        [Fact]
        public void Test07_InitializeHomogeneousRegion_SetsLargeRegion()
        {
            // Arrange
            var manager = new HybridStorageManager();
            var oceanRegion = new Envelope3D(-10000, -10000, -1000, 10000, 10000, 0);

            // Act
            manager.InitializeHomogeneousRegion(oceanRegion, MaterialId.Ocean);

            // Assert
            var material = manager.QueryMaterial(new Vector3(5000, 5000, -500), 5);
            Assert.Equal(MaterialId.Ocean, material);
        }

        [Fact]
        public void Test08_TileCache_LoadsAndCachesTiles()
        {
            // Arrange
            var manager = new HybridStorageManager(maxActiveTiles: 10);

            // Act - Query multiple positions at high LOD
            for (int i = 0; i < 5; i++)
            {
                var position = new Vector3(i * 1000, 0, 0);
                manager.QueryMaterial(position, 13);
            }

            // Assert
            var stats = manager.GetStatistics();
            Assert.True(stats.ActiveTiles > 0);
            Assert.True(stats.TileLoads > 0);
        }

        [Fact]
        public void Test09_LRUEviction_EvictsOldTiles()
        {
            // Arrange
            var manager = new HybridStorageManager(maxActiveTiles: 5);

            // Act - Create more tiles than cache can hold
            for (int i = 0; i < 10; i++)
            {
                var position = new Vector3(i * 10000, 0, 0);
                manager.QueryMaterial(position, 13);
            }

            // Assert
            var stats = manager.GetStatistics();
            Assert.True(stats.ActiveTiles <= 5);
            Assert.True(stats.TileEvictions > 0);
        }

        [Fact]
        public void Test10_ClearTileCache_RemovesAllTiles()
        {
            // Arrange
            var manager = new HybridStorageManager();
            
            // Create some tiles
            for (int i = 0; i < 5; i++)
            {
                manager.QueryMaterial(new Vector3(i * 1000, 0, 0), 13);
            }

            // Act
            manager.ClearTileCache();

            // Assert
            var stats = manager.GetStatistics();
            Assert.Equal(0, stats.ActiveTiles);
        }

        [Fact]
        public void Test11_Statistics_TracksBothSystems()
        {
            // Arrange
            var manager = new HybridStorageManager();
            var position = new Vector3(1000, 1000, 0);

            // Act
            manager.QueryMaterial(position, 10); // Octree
            manager.QueryMaterial(position, 13); // Grid

            // Assert
            var stats = manager.GetStatistics();
            Assert.True(stats.OctreeQueries > 0);
            Assert.True(stats.GridQueries > 0);
            Assert.Equal(2, stats.TotalQueries);
            Assert.True(stats.OctreeQueryPercent > 0);
            Assert.True(stats.GridQueryPercent > 0);
        }

        [Fact]
        public void Test12_PreloadTiles_LoadsTilesInAdvance()
        {
            // Arrange
            var manager = new HybridStorageManager();
            var region = new Envelope3D(0, 0, 0, 10000, 10000, 100);

            // Act
            manager.PreloadTiles(region, 13);

            // Assert
            var stats = manager.GetStatistics();
            Assert.True(stats.ActiveTiles > 0);
            Assert.True(stats.TileLoads > 0);
        }

        [Fact]
        public void Test13_MemoryEfficiency_DemonstratesSavings()
        {
            // Arrange
            var manager = new HybridStorageManager();
            var largeRegion = new Envelope3D(-100000, -100000, -1000, 100000, 100000, 0);

            // Act
            manager.InitializeHomogeneousRegion(largeRegion, MaterialId.Ocean);
            var stats = manager.GetStatistics();

            // Assert - Octree should show memory savings
            Assert.True(stats.OctreeMemorySavings > 50);
        }

        [Fact]
        public void Test14_SeamlessTransition_MaintainsDataAcrossLODs()
        {
            // Arrange
            var manager = new HybridStorageManager();
            var position = new Vector3(1000, 1000, 0);
            manager.UpdateMaterial(position, MaterialId.Sand, 10);

            // Act - Query at different LODs
            var coarseMaterial = manager.QueryMaterial(position, 10);
            var fineMaterial = manager.QueryMaterial(position, 13);

            // Assert - Both should return same material (inherited from octree)
            Assert.Equal(MaterialId.Sand, coarseMaterial);
            // Grid tile is generated from octree, so should inherit the material
            Assert.Equal(MaterialId.Sand, fineMaterial);
        }
    }
}
