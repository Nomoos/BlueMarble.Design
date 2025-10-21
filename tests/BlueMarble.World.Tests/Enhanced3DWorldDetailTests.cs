namespace BlueMarble.World.Tests
{
    using BlueMarble.World.Constants;
    using Xunit;
    
    /// <summary>
    /// Tests for Enhanced3DWorldDetail constants.
    /// </summary>
    public class Enhanced3DWorldDetailTests
    {
        [Fact]
        public void WorldSizeX_ShouldMatchEarthCircumference()
        {
            Assert.Equal(40075020L, Enhanced3DWorldDetail.WorldSizeX);
        }
        
        [Fact]
        public void WorldSizeY_ShouldMatchHalfCircumference()
        {
            Assert.Equal(20037510L, Enhanced3DWorldDetail.WorldSizeY);
        }
        
        [Fact]
        public void WorldSizeZ_ShouldBe20MillionMeters()
        {
            Assert.Equal(20000000L, Enhanced3DWorldDetail.WorldSizeZ);
        }
        
        [Fact]
        public void SeaLevelZ_ShouldBeHalfOfWorldSizeZ()
        {
            Assert.Equal(10000000L, Enhanced3DWorldDetail.SeaLevelZ);
            Assert.Equal(Enhanced3DWorldDetail.WorldSizeZ / 2, Enhanced3DWorldDetail.SeaLevelZ);
        }
        
        [Fact]
        public void MaxOctreeDepth_ShouldBe26()
        {
            // For 0.25m resolution: log₂(40,075,020 / 0.25) ≈ 26
            Assert.Equal(26, Enhanced3DWorldDetail.MaxOctreeDepth);
        }
        
        [Fact]
        public void AtmosphereTop_ShouldBe100KmAboveSeaLevel()
        {
            long expectedAltitude = Enhanced3DWorldDetail.SeaLevelZ + 100000;
            Assert.Equal(Enhanced3DWorldDetail.AtmosphereTop, expectedAltitude);
            Assert.Equal(Enhanced3DWorldDetail.AtmosphereTop, 10100000L);
        }
        
        [Fact]
        public void CrustBottom_ShouldBe100KmBelowSeaLevel()
        {
            long expectedDepth = Enhanced3DWorldDetail.SeaLevelZ - 100000;
            Assert.Equal(Enhanced3DWorldDetail.CrustBottom, expectedDepth);
            Assert.Equal(Enhanced3DWorldDetail.CrustBottom, 9900000L);
        }
        
        [Fact]
        public void MantleBottom_ShouldBe2900KmBelowSeaLevel()
        {
            long expectedDepth = Enhanced3DWorldDetail.SeaLevelZ - 2900000;
            Assert.Equal(Enhanced3DWorldDetail.MantleBottom, expectedDepth);
            Assert.Equal(Enhanced3DWorldDetail.MantleBottom, 7100000L);
        }
        
        [Fact]
        public void CoreBoundary_ShouldBe5150KmBelowSeaLevel()
        {
            long expectedDepth = Enhanced3DWorldDetail.SeaLevelZ - 5150000;
            Assert.Equal(Enhanced3DWorldDetail.CoreBoundary, expectedDepth);
            Assert.Equal(Enhanced3DWorldDetail.CoreBoundary, 4850000L);
        }
        
        [Fact]
        public void CoreCenter_ShouldBe6371KmBelowSeaLevel()
        {
            long expectedDepth = Enhanced3DWorldDetail.SeaLevelZ - 6371000;
            Assert.Equal(Enhanced3DWorldDetail.CoreCenter, expectedDepth);
            Assert.Equal(Enhanced3DWorldDetail.CoreCenter, 3629000L);
        }
        
        [Fact]
        public void MaxTerrainHeight_ShouldMatchMountEverest()
        {
            long expectedHeight = Enhanced3DWorldDetail.SeaLevelZ + 8849;
            Assert.Equal(Enhanced3DWorldDetail.MaxTerrainHeight, expectedHeight);
            Assert.Equal(Enhanced3DWorldDetail.MaxTerrainHeight, 10008849L);
        }
        
        [Fact]
        public void DeepestOcean_ShouldMatchMarianaTrench()
        {
            long expectedDepth = Enhanced3DWorldDetail.SeaLevelZ - 11034;
            Assert.Equal(Enhanced3DWorldDetail.DeepestOcean, expectedDepth);
            Assert.Equal(Enhanced3DWorldDetail.DeepestOcean, 9988966L);
        }
        
        [Fact]
        public void MaxPlayerDepth_ShouldBe50KmBelowSeaLevel()
        {
            long expectedDepth = Enhanced3DWorldDetail.SeaLevelZ - 50000;
            Assert.Equal(Enhanced3DWorldDetail.MaxPlayerDepth, expectedDepth);
            Assert.Equal(Enhanced3DWorldDetail.MaxPlayerDepth, 9950000L);
        }
        
        [Fact]
        public void MaxPlayerHeight_ShouldBe50KmAboveSeaLevel()
        {
            long expectedHeight = Enhanced3DWorldDetail.SeaLevelZ + 50000;
            Assert.Equal(Enhanced3DWorldDetail.MaxPlayerHeight, expectedHeight);
            Assert.Equal(Enhanced3DWorldDetail.MaxPlayerHeight, 10050000L);
        }
        
        [Fact]
        public void GeologicalLayers_ShouldBeInCorrectOrder()
        {
            // From bottom to top
            Assert.True(Enhanced3DWorldDetail.CoreCenter < Enhanced3DWorldDetail.CoreBoundary);
            Assert.True(Enhanced3DWorldDetail.CoreBoundary < Enhanced3DWorldDetail.MantleBottom);
            Assert.True(Enhanced3DWorldDetail.MantleBottom < Enhanced3DWorldDetail.CrustBottom);
            Assert.True(Enhanced3DWorldDetail.CrustBottom < Enhanced3DWorldDetail.SeaLevelZ);
            Assert.True(Enhanced3DWorldDetail.SeaLevelZ < Enhanced3DWorldDetail.AtmosphereTop);
        }
        
        [Fact]
        public void PlayerAccessibleRange_ShouldBeWithinWorldBounds()
        {
            Assert.True(Enhanced3DWorldDetail.MaxPlayerDepth >= 0);
            Assert.True(Enhanced3DWorldDetail.MaxPlayerHeight <= Enhanced3DWorldDetail.WorldSizeZ);
            Assert.True(Enhanced3DWorldDetail.MaxPlayerDepth < Enhanced3DWorldDetail.MaxPlayerHeight);
        }
        
        [Fact]
        public void TotalWorldVolume_ShouldBeCalculatedCorrectly()
        {
            decimal expectedVolume = (decimal)Enhanced3DWorldDetail.WorldSizeX * 
                                    Enhanced3DWorldDetail.WorldSizeY * 
                                    Enhanced3DWorldDetail.WorldSizeZ;
            Assert.Equal(Enhanced3DWorldDetail.TotalWorldVolume, expectedVolume);
        }
        
        [Fact]
        public void AllDimensions_ShouldBePositive()
        {
            Assert.True(Enhanced3DWorldDetail.WorldSizeX > 0);
            Assert.True(Enhanced3DWorldDetail.WorldSizeY > 0);
            Assert.True(Enhanced3DWorldDetail.WorldSizeZ > 0);
            Assert.True(Enhanced3DWorldDetail.SeaLevelZ > 0);
        }
        
        [Fact]
        public void BackwardCompatibility_WithWorldDetail()
        {
            // Enhanced3DWorldDetail should have same X and Y values as WorldDetail
            Assert.Equal(WorldDetail.WorldSizeX, Enhanced3DWorldDetail.WorldSizeX);
            Assert.Equal(WorldDetail.WorldSizeY, Enhanced3DWorldDetail.WorldSizeY);
        }
    }
}
