namespace BlueMarble.World.Tests
{
    using BlueMarble.World;
    using BlueMarble.World.Constants;
    using Xunit;
    
    /// <summary>
    /// Tests for AltitudeConverter.
    /// Verifies altitude conversion and sea level comparison logic.
    /// </summary>
    public class AltitudeConverterTests
    {
        [Fact]
        public void GetAltitudeFromSeaLevel_AtSeaLevel_ReturnsZero()
        {
            // Arrange
            long z = Enhanced3DWorldDetail.SeaLevelZ;
            
            // Act
            long altitude = AltitudeConverter.GetAltitudeFromSeaLevel(z);
            
            // Assert
            Assert.Equal(0L, altitude);
        }
        
        [Fact]
        public void GetAltitudeFromSeaLevel_MountEverest_ReturnsPositive8849()
        {
            // Arrange - Mt. Everest equivalent
            long z = Enhanced3DWorldDetail.MaxTerrainHeight;
            
            // Act
            long altitude = AltitudeConverter.GetAltitudeFromSeaLevel(z);
            
            // Assert
            Assert.Equal(8849L, altitude);
        }
        
        [Fact]
        public void GetAltitudeFromSeaLevel_MarianaTrench_ReturnsNegative11034()
        {
            // Arrange - Mariana Trench equivalent
            long z = Enhanced3DWorldDetail.DeepestOcean;
            
            // Act
            long altitude = AltitudeConverter.GetAltitudeFromSeaLevel(z);
            
            // Assert
            Assert.Equal(-11034L, altitude);
        }
        
        [Fact]
        public void GetZCoordinateFromAltitude_Zero_ReturnsSeaLevel()
        {
            // Arrange
            long altitude = 0;
            
            // Act
            long z = AltitudeConverter.GetZCoordinateFromAltitude(altitude);
            
            // Assert
            Assert.Equal(Enhanced3DWorldDetail.SeaLevelZ, z);
        }
        
        [Fact]
        public void GetZCoordinateFromAltitude_Positive8849_ReturnsMountEverest()
        {
            // Arrange
            long altitude = 8849;
            
            // Act
            long z = AltitudeConverter.GetZCoordinateFromAltitude(altitude);
            
            // Assert
            Assert.Equal(Enhanced3DWorldDetail.MaxTerrainHeight, z);
        }
        
        [Fact]
        public void GetZCoordinateFromAltitude_Negative11034_ReturnsMarianaTrench()
        {
            // Arrange
            long altitude = -11034;
            
            // Act
            long z = AltitudeConverter.GetZCoordinateFromAltitude(altitude);
            
            // Assert
            Assert.Equal(Enhanced3DWorldDetail.DeepestOcean, z);
        }
        
        [Fact]
        public void IsAboveSeaLevel_AboveSeaLevel_ReturnsTrue()
        {
            // Arrange
            long z = Enhanced3DWorldDetail.SeaLevelZ + 1000;
            
            // Act & Assert
            Assert.True(AltitudeConverter.IsAboveSeaLevel(z));
        }
        
        [Fact]
        public void IsAboveSeaLevel_AtSeaLevel_ReturnsFalse()
        {
            // Arrange
            long z = Enhanced3DWorldDetail.SeaLevelZ;
            
            // Act & Assert
            Assert.False(AltitudeConverter.IsAboveSeaLevel(z));
        }
        
        [Fact]
        public void IsBelowSeaLevel_BelowSeaLevel_ReturnsTrue()
        {
            // Arrange
            long z = Enhanced3DWorldDetail.SeaLevelZ - 1000;
            
            // Act & Assert
            Assert.True(AltitudeConverter.IsBelowSeaLevel(z));
        }
        
        [Fact]
        public void IsBelowSeaLevel_AtSeaLevel_ReturnsFalse()
        {
            // Arrange
            long z = Enhanced3DWorldDetail.SeaLevelZ;
            
            // Act & Assert
            Assert.False(AltitudeConverter.IsBelowSeaLevel(z));
        }
        
        [Fact]
        public void ClampToGameplayBounds_WithinBounds_ReturnsUnchanged()
        {
            // Arrange
            long z = Enhanced3DWorldDetail.SeaLevelZ;
            
            // Act
            long clamped = AltitudeConverter.ClampToGameplayBounds(z);
            
            // Assert
            Assert.Equal(z, clamped);
        }
        
        [Fact]
        public void ClampToGameplayBounds_TooHigh_ReturnsMaxHeight()
        {
            // Arrange
            long z = Enhanced3DWorldDetail.MaxPlayerHeight + 1000;
            
            // Act
            long clamped = AltitudeConverter.ClampToGameplayBounds(z);
            
            // Assert
            Assert.Equal(Enhanced3DWorldDetail.MaxPlayerHeight, clamped);
        }
        
        [Fact]
        public void ClampToGameplayBounds_TooDeep_ReturnsMaxDepth()
        {
            // Arrange
            long z = Enhanced3DWorldDetail.MaxPlayerDepth - 1000;
            
            // Act
            long clamped = AltitudeConverter.ClampToGameplayBounds(z);
            
            // Assert
            Assert.Equal(Enhanced3DWorldDetail.MaxPlayerDepth, clamped);
        }
    }
}
