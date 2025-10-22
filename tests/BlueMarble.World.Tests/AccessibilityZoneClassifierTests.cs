namespace BlueMarble.World.Tests
{
    using BlueMarble.World;
    using BlueMarble.World.Constants;
    using Xunit;
    
    /// <summary>
    /// Tests for AccessibilityZoneClassifier.
    /// Verifies zone classification logic is correct.
    /// </summary>
    public class AccessibilityZoneClassifierTests
    {
        [Fact]
        public void DetermineAccessibilityZone_SeaLevel_ReturnsSurface()
        {
            // Arrange
            long z = Enhanced3DWorldDetail.SeaLevelZ;
            
            // Act
            AccessibilityZone zone = AccessibilityZoneClassifier.DetermineAccessibilityZone(z);
            
            // Assert
            Assert.Equal(AccessibilityZone.Surface, zone);
        }
        
        [Fact]
        public void DetermineAccessibilityZone_1000MBelowSeaLevel_ReturnsDeepMining()
        {
            // Arrange
            long z = Enhanced3DWorldDetail.SeaLevelZ - 1000;
            
            // Act
            AccessibilityZone zone = AccessibilityZoneClassifier.DetermineAccessibilityZone(z);
            
            // Assert
            Assert.Equal(AccessibilityZone.DeepMining, zone);
        }
        
        [Fact]
        public void DetermineAccessibilityZone_10000MAboveSeaLevel_ReturnsHighAltitude()
        {
            // Arrange
            long z = Enhanced3DWorldDetail.SeaLevelZ + 10000;
            
            // Act
            AccessibilityZone zone = AccessibilityZoneClassifier.DetermineAccessibilityZone(z);
            
            // Assert
            Assert.Equal(AccessibilityZone.HighAltitude, zone);
        }
        
        [Fact]
        public void DetermineAccessibilityZone_50000MBelowSeaLevel_ReturnsExtremeDepth()
        {
            // Arrange
            long z = Enhanced3DWorldDetail.SeaLevelZ - 50000;
            
            // Act
            AccessibilityZone zone = AccessibilityZoneClassifier.DetermineAccessibilityZone(z);
            
            // Assert
            Assert.Equal(AccessibilityZone.ExtremeDepth, zone);
        }
        
        [Fact]
        public void DetermineAccessibilityZone_50000MAboveSeaLevel_ReturnsAtmosphericHigh()
        {
            // Arrange
            long z = Enhanced3DWorldDetail.SeaLevelZ + 50000;
            
            // Act
            AccessibilityZone zone = AccessibilityZoneClassifier.DetermineAccessibilityZone(z);
            
            // Assert
            Assert.Equal(AccessibilityZone.AtmosphericHigh, zone);
        }
        
        [Fact]
        public void DetermineAccessibilityZone_BeyondMaxAltitude_ReturnsInaccessible()
        {
            // Arrange
            long z = Enhanced3DWorldDetail.SeaLevelZ + 150000;
            
            // Act
            AccessibilityZone zone = AccessibilityZoneClassifier.DetermineAccessibilityZone(z);
            
            // Assert
            Assert.Equal(AccessibilityZone.Inaccessible, zone);
        }
        
        [Fact]
        public void DetermineAccessibilityZone_BeyondMaxDepth_ReturnsInaccessible()
        {
            // Arrange
            long z = Enhanced3DWorldDetail.SeaLevelZ - 150000;
            
            // Act
            AccessibilityZone zone = AccessibilityZoneClassifier.DetermineAccessibilityZone(z);
            
            // Assert
            Assert.Equal(AccessibilityZone.Inaccessible, zone);
        }
    }
}
