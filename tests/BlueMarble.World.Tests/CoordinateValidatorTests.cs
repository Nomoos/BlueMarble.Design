namespace BlueMarble.World.Tests
{
    using BlueMarble.World;
    using BlueMarble.World.Constants;
    using Xunit;
    
    /// <summary>
    /// Tests for CoordinateValidator.
    /// Verifies coordinate validation logic is correct.
    /// </summary>
    public class CoordinateValidatorTests
    {
        [Fact]
        public void IsWithinGameplayBounds_ValidCoordinates_ReturnsTrue()
        {
            // Arrange - coordinates at sea level, center of world
            long x = Enhanced3DWorldDetail.WorldSizeX / 2;
            long y = Enhanced3DWorldDetail.WorldSizeY / 2;
            long z = Enhanced3DWorldDetail.SeaLevelZ;
            
            // Act & Assert
            Assert.True(CoordinateValidator.IsWithinGameplayBounds(x, y, z));
        }
        
        [Fact]
        public void IsWithinGameplayBounds_MaxPlayerHeight_ReturnsTrue()
        {
            // Arrange
            long x = 1000;
            long y = 1000;
            long z = Enhanced3DWorldDetail.MaxPlayerHeight;
            
            // Act & Assert
            Assert.True(CoordinateValidator.IsWithinGameplayBounds(x, y, z));
        }
        
        [Fact]
        public void IsWithinGameplayBounds_MaxPlayerDepth_ReturnsTrue()
        {
            // Arrange
            long x = 1000;
            long y = 1000;
            long z = Enhanced3DWorldDetail.MaxPlayerDepth;
            
            // Act & Assert
            Assert.True(CoordinateValidator.IsWithinGameplayBounds(x, y, z));
        }
        
        [Fact]
        public void IsWithinGameplayBounds_AboveMaxHeight_ReturnsFalse()
        {
            // Arrange
            long x = 1000;
            long y = 1000;
            long z = Enhanced3DWorldDetail.MaxPlayerHeight + 1;
            
            // Act & Assert
            Assert.False(CoordinateValidator.IsWithinGameplayBounds(x, y, z));
        }
        
        [Fact]
        public void IsWithinGameplayBounds_BelowMaxDepth_ReturnsFalse()
        {
            // Arrange
            long x = 1000;
            long y = 1000;
            long z = Enhanced3DWorldDetail.MaxPlayerDepth - 1;
            
            // Act & Assert
            Assert.False(CoordinateValidator.IsWithinGameplayBounds(x, y, z));
        }
        
        [Fact]
        public void IsWithinWorldBounds_ValidCoordinates_ReturnsTrue()
        {
            // Arrange
            long x = 1000;
            long y = 1000;
            long z = 1000;
            
            // Act & Assert
            Assert.True(CoordinateValidator.IsWithinWorldBounds(x, y, z));
        }
        
        [Fact]
        public void IsWithinWorldBounds_AtWorldEdge_ReturnsTrue()
        {
            // Arrange - at max valid coordinates (one less than size)
            long x = Enhanced3DWorldDetail.WorldSizeX - 1;
            long y = Enhanced3DWorldDetail.WorldSizeY - 1;
            long z = Enhanced3DWorldDetail.WorldSizeZ - 1;
            
            // Act & Assert
            Assert.True(CoordinateValidator.IsWithinWorldBounds(x, y, z));
        }
        
        [Fact]
        public void IsWithinWorldBounds_BeyondXBoundary_ReturnsFalse()
        {
            // Arrange
            long x = Enhanced3DWorldDetail.WorldSizeX;
            long y = 1000;
            long z = 1000;
            
            // Act & Assert
            Assert.False(CoordinateValidator.IsWithinWorldBounds(x, y, z));
        }
        
        [Fact]
        public void IsWithinWorldBounds_NegativeX_ReturnsFalse()
        {
            // Arrange
            long x = -1;
            long y = 1000;
            long z = 1000;
            
            // Act & Assert
            Assert.False(CoordinateValidator.IsWithinWorldBounds(x, y, z));
        }
    }
}
