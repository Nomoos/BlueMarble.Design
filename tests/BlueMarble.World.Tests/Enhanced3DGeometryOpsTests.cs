namespace BlueMarble.World.Tests
{
    using BlueMarble.World;
    using BlueMarble.World.Constants;
    using Xunit;
    
    /// <summary>
    /// Tests for Enhanced3DGeometryOps utility methods.
    /// </summary>
    public class Enhanced3DGeometryOpsTests
    {
        #region Bounds Checking Tests
        
        [Fact]
        public void IsWithinGameplayBounds_ValidCoordinates_ReturnsTrue()
        {
            // Arrange - coordinates at sea level, center of world
            long x = Enhanced3DWorldDetail.WorldSizeX / 2;
            long y = Enhanced3DWorldDetail.WorldSizeY / 2;
            long z = Enhanced3DWorldDetail.SeaLevelZ;
            
            // Act & Assert
            Assert.True(Enhanced3DGeometryOps.IsWithinGameplayBounds(x, y, z));
        }
        
        [Fact]
        public void IsWithinGameplayBounds_MaxPlayerHeight_ReturnsTrue()
        {
            // Arrange
            long x = 1000;
            long y = 1000;
            long z = Enhanced3DWorldDetail.MaxPlayerHeight;
            
            // Act & Assert
            Assert.True(Enhanced3DGeometryOps.IsWithinGameplayBounds(x, y, z));
        }
        
        [Fact]
        public void IsWithinGameplayBounds_MaxPlayerDepth_ReturnsTrue()
        {
            // Arrange
            long x = 1000;
            long y = 1000;
            long z = Enhanced3DWorldDetail.MaxPlayerDepth;
            
            // Act & Assert
            Assert.True(Enhanced3DGeometryOps.IsWithinGameplayBounds(x, y, z));
        }
        
        [Fact]
        public void IsWithinGameplayBounds_TooHigh_ReturnsFalse()
        {
            // Arrange - above max player height
            long x = 1000;
            long y = 1000;
            long z = Enhanced3DWorldDetail.MaxPlayerHeight + 1;
            
            // Act & Assert
            Assert.False(Enhanced3DGeometryOps.IsWithinGameplayBounds(x, y, z));
        }
        
        [Fact]
        public void IsWithinGameplayBounds_TooDeep_ReturnsFalse()
        {
            // Arrange - below max player depth
            long x = 1000;
            long y = 1000;
            long z = Enhanced3DWorldDetail.MaxPlayerDepth - 1;
            
            // Act & Assert
            Assert.False(Enhanced3DGeometryOps.IsWithinGameplayBounds(x, y, z));
        }
        
        [Fact]
        public void IsWithinGameplayBounds_NegativeX_ReturnsFalse()
        {
            // Arrange
            long x = -1;
            long y = 1000;
            long z = Enhanced3DWorldDetail.SeaLevelZ;
            
            // Act & Assert
            Assert.False(Enhanced3DGeometryOps.IsWithinGameplayBounds(x, y, z));
        }
        
        [Fact]
        public void IsWithinWorldBounds_ValidCoordinates_ReturnsTrue()
        {
            // Arrange
            long x = Enhanced3DWorldDetail.WorldSizeX / 2;
            long y = Enhanced3DWorldDetail.WorldSizeY / 2;
            long z = Enhanced3DWorldDetail.WorldSizeZ / 2;
            
            // Act & Assert
            Assert.True(Enhanced3DGeometryOps.IsWithinWorldBounds(x, y, z));
        }
        
        [Fact]
        public void IsWithinWorldBounds_CoreCenter_ReturnsTrue()
        {
            // Arrange - even core center is within world bounds
            long x = 1000;
            long y = 1000;
            long z = Enhanced3DWorldDetail.CoreCenter;
            
            // Act & Assert
            Assert.True(Enhanced3DGeometryOps.IsWithinWorldBounds(x, y, z));
        }
        
        [Fact]
        public void IsWithinWorldBounds_OutOfBounds_ReturnsFalse()
        {
            // Arrange - beyond world size
            long x = Enhanced3DWorldDetail.WorldSizeX + 1;
            long y = 1000;
            long z = 1000;
            
            // Act & Assert
            Assert.False(Enhanced3DGeometryOps.IsWithinWorldBounds(x, y, z));
        }
        
        #endregion
        
        #region Accessibility Zone Tests
        
        [Fact]
        public void DetermineAccessibilityZone_SeaLevel_ReturnsSurface()
        {
            // Arrange
            long z = Enhanced3DWorldDetail.SeaLevelZ;
            
            // Act
            var zone = Enhanced3DGeometryOps.DetermineAccessibilityZone(z);
            
            // Assert
            Assert.Equal(AccessibilityZone.Surface, zone);
        }
        
        [Fact]
        public void DetermineAccessibilityZone_5KmUp_ReturnsSurface()
        {
            // Arrange - 5km above sea level
            long z = Enhanced3DWorldDetail.SeaLevelZ + 5000;
            
            // Act
            var zone = Enhanced3DGeometryOps.DetermineAccessibilityZone(z);
            
            // Assert
            Assert.Equal(AccessibilityZone.Surface, zone);
        }
        
        [Fact]
        public void DetermineAccessibilityZone_15KmUp_ReturnsHighAltitude()
        {
            // Arrange - 15km above sea level
            long z = Enhanced3DWorldDetail.SeaLevelZ + 15000;
            
            // Act
            var zone = Enhanced3DGeometryOps.DetermineAccessibilityZone(z);
            
            // Assert
            Assert.Equal(AccessibilityZone.HighAltitude, zone);
        }
        
        [Fact]
        public void DetermineAccessibilityZone_60KmUp_ReturnsAtmosphericHigh()
        {
            // Arrange - 60km above sea level
            long z = Enhanced3DWorldDetail.SeaLevelZ + 60000;
            
            // Act
            var zone = Enhanced3DGeometryOps.DetermineAccessibilityZone(z);
            
            // Assert
            Assert.Equal(AccessibilityZone.AtmosphericHigh, zone);
        }
        
        [Fact]
        public void DetermineAccessibilityZone_5KmDown_ReturnsDeepMining()
        {
            // Arrange - 5km below sea level
            long z = Enhanced3DWorldDetail.SeaLevelZ - 5000;
            
            // Act
            var zone = Enhanced3DGeometryOps.DetermineAccessibilityZone(z);
            
            // Assert
            Assert.Equal(AccessibilityZone.DeepMining, zone);
        }
        
        [Fact]
        public void DetermineAccessibilityZone_60KmDown_ReturnsExtremeDepth()
        {
            // Arrange - 60km below sea level
            long z = Enhanced3DWorldDetail.SeaLevelZ - 60000;
            
            // Act
            var zone = Enhanced3DGeometryOps.DetermineAccessibilityZone(z);
            
            // Assert
            Assert.Equal(AccessibilityZone.ExtremeDepth, zone);
        }
        
        [Fact]
        public void DetermineAccessibilityZone_150KmUp_ReturnsInaccessible()
        {
            // Arrange - 150km above sea level (space)
            long z = Enhanced3DWorldDetail.SeaLevelZ + 150000;
            
            // Act
            var zone = Enhanced3DGeometryOps.DetermineAccessibilityZone(z);
            
            // Assert
            Assert.Equal(AccessibilityZone.Inaccessible, zone);
        }
        
        [Fact]
        public void DetermineAccessibilityZone_CoreCenter_ReturnsInaccessible()
        {
            // Arrange
            long z = Enhanced3DWorldDetail.CoreCenter;
            
            // Act
            var zone = Enhanced3DGeometryOps.DetermineAccessibilityZone(z);
            
            // Assert
            Assert.Equal(AccessibilityZone.Inaccessible, zone);
        }
        
        #endregion
        
        #region Altitude Conversion Tests
        
        [Fact]
        public void GetAltitudeFromSeaLevel_SeaLevel_ReturnsZero()
        {
            // Arrange
            long z = Enhanced3DWorldDetail.SeaLevelZ;
            
            // Act
            long altitude = Enhanced3DGeometryOps.GetAltitudeFromSeaLevel(z);
            
            // Assert
            Assert.Equal(0, altitude);
        }
        
        [Fact]
        public void GetAltitudeFromSeaLevel_AboveSeaLevel_ReturnsPositive()
        {
            // Arrange - 1000m above sea level
            long z = Enhanced3DWorldDetail.SeaLevelZ + 1000;
            
            // Act
            long altitude = Enhanced3DGeometryOps.GetAltitudeFromSeaLevel(z);
            
            // Assert
            Assert.Equal(1000, altitude);
        }
        
        [Fact]
        public void GetAltitudeFromSeaLevel_BelowSeaLevel_ReturnsNegative()
        {
            // Arrange - 500m below sea level
            long z = Enhanced3DWorldDetail.SeaLevelZ - 500;
            
            // Act
            long altitude = Enhanced3DGeometryOps.GetAltitudeFromSeaLevel(z);
            
            // Assert
            Assert.Equal(-500, altitude);
        }
        
        [Fact]
        public void GetZCoordinateFromAltitude_ZeroAltitude_ReturnsSeaLevel()
        {
            // Arrange
            long altitude = 0;
            
            // Act
            long z = Enhanced3DGeometryOps.GetZCoordinateFromAltitude(altitude);
            
            // Assert
            Assert.Equal(Enhanced3DWorldDetail.SeaLevelZ, z);
        }
        
        [Fact]
        public void GetZCoordinateFromAltitude_PositiveAltitude_ReturnsCorrectZ()
        {
            // Arrange
            long altitude = 8849; // Mount Everest
            
            // Act
            long z = Enhanced3DGeometryOps.GetZCoordinateFromAltitude(altitude);
            
            // Assert
            Assert.Equal(Enhanced3DWorldDetail.MaxTerrainHeight, z);
        }
        
        [Fact]
        public void GetZCoordinateFromAltitude_NegativeAltitude_ReturnsCorrectZ()
        {
            // Arrange
            long altitude = -11034; // Mariana Trench
            
            // Act
            long z = Enhanced3DGeometryOps.GetZCoordinateFromAltitude(altitude);
            
            // Assert
            Assert.Equal(Enhanced3DWorldDetail.DeepestOcean, z);
        }
        
        [Fact]
        public void AltitudeConversion_RoundTrip_PreservesValue()
        {
            // Arrange
            long originalAltitude = 5000;
            
            // Act - convert altitude to Z and back
            long z = Enhanced3DGeometryOps.GetZCoordinateFromAltitude(originalAltitude);
            long roundTripAltitude = Enhanced3DGeometryOps.GetAltitudeFromSeaLevel(z);
            
            // Assert
            Assert.Equal(originalAltitude, roundTripAltitude);
        }
        
        #endregion
        
        #region Sea Level Tests
        
        [Fact]
        public void IsAboveSeaLevel_AboveSeaLevel_ReturnsTrue()
        {
            // Arrange
            long z = Enhanced3DWorldDetail.SeaLevelZ + 1;
            
            // Act & Assert
            Assert.True(Enhanced3DGeometryOps.IsAboveSeaLevel(z));
        }
        
        [Fact]
        public void IsAboveSeaLevel_AtSeaLevel_ReturnsFalse()
        {
            // Arrange
            long z = Enhanced3DWorldDetail.SeaLevelZ;
            
            // Act & Assert
            Assert.False(Enhanced3DGeometryOps.IsAboveSeaLevel(z));
        }
        
        [Fact]
        public void IsAboveSeaLevel_BelowSeaLevel_ReturnsFalse()
        {
            // Arrange
            long z = Enhanced3DWorldDetail.SeaLevelZ - 1;
            
            // Act & Assert
            Assert.False(Enhanced3DGeometryOps.IsAboveSeaLevel(z));
        }
        
        [Fact]
        public void IsBelowSeaLevel_BelowSeaLevel_ReturnsTrue()
        {
            // Arrange
            long z = Enhanced3DWorldDetail.SeaLevelZ - 1;
            
            // Act & Assert
            Assert.True(Enhanced3DGeometryOps.IsBelowSeaLevel(z));
        }
        
        [Fact]
        public void IsBelowSeaLevel_AtSeaLevel_ReturnsFalse()
        {
            // Arrange
            long z = Enhanced3DWorldDetail.SeaLevelZ;
            
            // Act & Assert
            Assert.False(Enhanced3DGeometryOps.IsBelowSeaLevel(z));
        }
        
        [Fact]
        public void IsBelowSeaLevel_AboveSeaLevel_ReturnsFalse()
        {
            // Arrange
            long z = Enhanced3DWorldDetail.SeaLevelZ + 1;
            
            // Act & Assert
            Assert.False(Enhanced3DGeometryOps.IsBelowSeaLevel(z));
        }
        
        #endregion
        
        #region Clamping Tests
        
        [Fact]
        public void ClampToGameplayBounds_WithinBounds_ReturnsUnchanged()
        {
            // Arrange
            long z = Enhanced3DWorldDetail.SeaLevelZ;
            
            // Act
            long clamped = Enhanced3DGeometryOps.ClampToGameplayBounds(z);
            
            // Assert
            Assert.Equal(z, clamped);
        }
        
        [Fact]
        public void ClampToGameplayBounds_TooHigh_ClampsToMaxHeight()
        {
            // Arrange - way above max height
            long z = Enhanced3DWorldDetail.MaxPlayerHeight + 10000;
            
            // Act
            long clamped = Enhanced3DGeometryOps.ClampToGameplayBounds(z);
            
            // Assert
            Assert.Equal(Enhanced3DWorldDetail.MaxPlayerHeight, clamped);
        }
        
        [Fact]
        public void ClampToGameplayBounds_TooDeep_ClampsToMaxDepth()
        {
            // Arrange - way below max depth
            long z = Enhanced3DWorldDetail.MaxPlayerDepth - 10000;
            
            // Act
            long clamped = Enhanced3DGeometryOps.ClampToGameplayBounds(z);
            
            // Assert
            Assert.Equal(Enhanced3DWorldDetail.MaxPlayerDepth, clamped);
        }
        
        [Fact]
        public void ClampToGameplayBounds_AtMaxHeight_ReturnsUnchanged()
        {
            // Arrange
            long z = Enhanced3DWorldDetail.MaxPlayerHeight;
            
            // Act
            long clamped = Enhanced3DGeometryOps.ClampToGameplayBounds(z);
            
            // Assert
            Assert.Equal(z, clamped);
        }
        
        [Fact]
        public void ClampToGameplayBounds_AtMaxDepth_ReturnsUnchanged()
        {
            // Arrange
            long z = Enhanced3DWorldDetail.MaxPlayerDepth;
            
            // Act
            long clamped = Enhanced3DGeometryOps.ClampToGameplayBounds(z);
            
            // Assert
            Assert.Equal(z, clamped);
        }
        
        #endregion
    }
}
