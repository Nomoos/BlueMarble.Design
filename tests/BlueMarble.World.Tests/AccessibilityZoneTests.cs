namespace BlueMarble.World.Tests
{
    using BlueMarble.World.Constants;
    using Xunit;
    
    /// <summary>
    /// Tests for AccessibilityZone enum.
    /// </summary>
    public class AccessibilityZoneTests
    {
        [Fact]
        public void Surface_ShouldHaveValue0()
        {
            Assert.Equal(0, (int)AccessibilityZone.Surface);
        }
        
        [Fact]
        public void DeepMining_ShouldHaveValue1()
        {
            Assert.Equal(1, (int)AccessibilityZone.DeepMining);
        }
        
        [Fact]
        public void HighAltitude_ShouldHaveValue2()
        {
            Assert.Equal(2, (int)AccessibilityZone.HighAltitude);
        }
        
        [Fact]
        public void ExtremeDepth_ShouldHaveValue3()
        {
            Assert.Equal(3, (int)AccessibilityZone.ExtremeDepth);
        }
        
        [Fact]
        public void AtmosphericHigh_ShouldHaveValue4()
        {
            Assert.Equal(4, (int)AccessibilityZone.AtmosphericHigh);
        }
        
        [Fact]
        public void Inaccessible_ShouldHaveValue5()
        {
            Assert.Equal(5, (int)AccessibilityZone.Inaccessible);
        }
        
        [Fact]
        public void AllZones_ShouldBeDefined()
        {
            // Verify all expected zones exist
            var zones = Enum.GetValues<AccessibilityZone>();
            Assert.Equal(6, zones.Length);
            Assert.Contains(AccessibilityZone.Surface, zones);
            Assert.Contains(AccessibilityZone.DeepMining, zones);
            Assert.Contains(AccessibilityZone.HighAltitude, zones);
            Assert.Contains(AccessibilityZone.ExtremeDepth, zones);
            Assert.Contains(AccessibilityZone.AtmosphericHigh, zones);
            Assert.Contains(AccessibilityZone.Inaccessible, zones);
        }
    }
}
