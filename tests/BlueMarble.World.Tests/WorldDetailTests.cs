namespace BlueMarble.World.Tests
{
    using BlueMarble.World.Constants;
    using Xunit;
    
    /// <summary>
    /// Tests for WorldDetail constants to ensure backward compatibility.
    /// </summary>
    public class WorldDetailTests
    {
        [Fact]
        public void WorldSizeX_ShouldMatchEarthCircumference()
        {
            // Arrange & Act
            long worldSizeX = WorldDetail.WorldSizeX;
            
            // Assert
            Assert.Equal(40075020L, worldSizeX);
        }
        
        [Fact]
        public void WorldSizeY_ShouldMatchHalfCircumference()
        {
            // Arrange & Act
            long worldSizeY = WorldDetail.WorldSizeY;
            
            // Assert
            Assert.Equal(20037510L, worldSizeY);
        }
        
        [Fact]
        public void WorldDimensions_ShouldBePositive()
        {
            // Assert
            Assert.True(WorldDetail.WorldSizeX > 0);
            Assert.True(WorldDetail.WorldSizeY > 0);
        }
        
        [Fact]
        public void WorldSizeX_ShouldBeWithinLongRange()
        {
            // Assert - verify it's well within long range for safe arithmetic
            Assert.True(WorldDetail.WorldSizeX < long.MaxValue / 1000);
        }
    }
}
