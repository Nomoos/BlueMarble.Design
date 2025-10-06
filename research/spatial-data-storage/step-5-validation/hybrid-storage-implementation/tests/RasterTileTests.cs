using System;
using Xunit;

namespace BlueMarble.SpatialStorage.Tests
{
    /// <summary>
    /// Test suite for RasterTile (11 tests)
    /// Covers grid tile functionality, O(1) queries, and LRU cache tracking
    /// </summary>
    public class RasterTileTests
    {
        [Fact]
        public void Test01_Constructor_InitializesTile()
        {
            // Arrange & Act
            var bounds = new Envelope3D(0, 0, 0, 1000, 1000, 100);
            var tile = new RasterTile("test_tile", bounds, 1.0, 1000, 1000);

            // Assert
            Assert.Equal("test_tile", tile.TileId);
            Assert.Equal(1000, tile.Width);
            Assert.Equal(1000, tile.Height);
            Assert.NotNull(tile.MaterialGrid);
        }

        [Fact]
        public void Test02_QueryMaterial_ReturnsCorrectMaterial()
        {
            // Arrange
            var bounds = new Envelope3D(0, 0, 0, 1000, 1000, 100);
            var tile = new RasterTile("test_tile", bounds, 1.0, 1000, 1000);
            tile.MaterialGrid[500, 500] = MaterialId.Sand;

            // Act
            var material = tile.QueryMaterial(new Vector3(500, 500, 50));

            // Assert
            Assert.Equal(MaterialId.Sand, material);
        }

        [Fact]
        public void Test03_UpdateMaterial_SetsMaterial()
        {
            // Arrange
            var bounds = new Envelope3D(0, 0, 0, 1000, 1000, 100);
            var tile = new RasterTile("test_tile", bounds, 1.0, 1000, 1000);
            var position = new Vector3(500, 500, 50);

            // Act
            tile.UpdateMaterial(position, MaterialId.Rock);
            var material = tile.QueryMaterial(position);

            // Assert
            Assert.Equal(MaterialId.Rock, material);
        }

        [Fact]
        public void Test04_FillUniform_FillsEntireTile()
        {
            // Arrange
            var bounds = new Envelope3D(0, 0, 0, 1000, 1000, 100);
            var tile = new RasterTile("test_tile", bounds, 1.0, 100, 100);

            // Act
            tile.FillUniform(MaterialId.Ocean);

            // Assert
            for (int y = 0; y < 100; y++)
            {
                for (int x = 0; x < 100; x++)
                {
                    Assert.Equal(MaterialId.Ocean, tile.MaterialGrid[y, x]);
                }
            }
        }

        [Fact]
        public void Test05_CalculateHomogeneity_UniformTile()
        {
            // Arrange
            var bounds = new Envelope3D(0, 0, 0, 1000, 1000, 100);
            var tile = new RasterTile("test_tile", bounds, 1.0, 100, 100);
            tile.FillUniform(MaterialId.Ocean);

            // Act
            double homogeneity = tile.CalculateHomogeneity();

            // Assert
            Assert.Equal(1.0, homogeneity, 0.01);
        }

        [Fact]
        public void Test06_CalculateHomogeneity_MixedTile()
        {
            // Arrange
            var bounds = new Envelope3D(0, 0, 0, 1000, 1000, 100);
            var tile = new RasterTile("test_tile", bounds, 1.0, 100, 100);
            
            // Fill with 50% ocean, 50% sand
            for (int y = 0; y < 100; y++)
            {
                for (int x = 0; x < 100; x++)
                {
                    tile.MaterialGrid[y, x] = (x < 50) ? MaterialId.Ocean : MaterialId.Sand;
                }
            }

            // Act
            double homogeneity = tile.CalculateHomogeneity();

            // Assert
            Assert.True(homogeneity >= 0.5 && homogeneity <= 0.6);
        }

        [Fact]
        public void Test07_GetMemorySize_ReturnsReasonableSize()
        {
            // Arrange
            var bounds = new Envelope3D(0, 0, 0, 1000, 1000, 100);
            var tile = new RasterTile("test_tile", bounds, 1.0, 1024, 1024);

            // Act
            long memorySize = tile.GetMemorySize();

            // Assert
            // 1024 * 1024 * 1 byte = 1,048,576 bytes + overhead
            Assert.True(memorySize > 1000000);
            Assert.True(memorySize < 2000000);
        }

        [Fact]
        public void Test08_LastAccessed_UpdatesOnQuery()
        {
            // Arrange
            var bounds = new Envelope3D(0, 0, 0, 1000, 1000, 100);
            var tile = new RasterTile("test_tile", bounds, 1.0, 100, 100);
            var initialTime = tile.LastAccessed;

            // Act
            System.Threading.Thread.Sleep(10); // Small delay
            tile.QueryMaterial(new Vector3(50, 50, 50));

            // Assert
            Assert.True(tile.LastAccessed > initialTime);
        }

        [Fact]
        public void Test09_AccessCount_IncrementsOnQuery()
        {
            // Arrange
            var bounds = new Envelope3D(0, 0, 0, 1000, 1000, 100);
            var tile = new RasterTile("test_tile", bounds, 1.0, 100, 100);

            // Act
            for (int i = 0; i < 5; i++)
            {
                tile.QueryMaterial(new Vector3(50, 50, 50));
            }

            // Assert
            Assert.Equal(5, tile.AccessCount);
        }

        [Fact]
        public void Test10_FromOctree_GeneratesTileFromOctree()
        {
            // Arrange
            var octree = new GlobalOctree();
            octree.UpdateMaterial(new Vector3(500, 500, 0), MaterialId.Sand);
            
            var bounds = new Envelope3D(0, 0, 0, 1000, 1000, 100);

            // Act
            var tile = RasterTile.FromOctree("test_tile", bounds, octree, 10.0);

            // Assert
            Assert.NotNull(tile);
            Assert.Equal(100, tile.Width);
            Assert.Equal(100, tile.Height);
        }

        [Fact]
        public void Test11_GetStatistics_ReturnsCompleteStats()
        {
            // Arrange
            var bounds = new Envelope3D(0, 0, 0, 1000, 1000, 100);
            var tile = new RasterTile("test_tile", bounds, 1.0, 100, 100);
            tile.FillUniform(MaterialId.Ocean);
            tile.QueryMaterial(new Vector3(50, 50, 50));

            // Act
            var stats = tile.GetStatistics();

            // Assert
            Assert.Equal("test_tile", stats.TileId);
            Assert.Equal(100, stats.Width);
            Assert.Equal(100, stats.Height);
            Assert.Equal(10000, stats.TotalCells);
            Assert.Equal(1, stats.UniqueMaterials);
            Assert.Equal(1.0, stats.Homogeneity, 0.01);
            Assert.Equal(1, stats.AccessCount);
        }
    }
}
