namespace BlueMarble.World.Tests
{
    using BlueMarble.World.Constants;
    using Xunit;
    
    /// <summary>
    /// Tests for EconomicPrecision constants.
    /// </summary>
    public class EconomicPrecisionTests
    {
        [Fact]
        public void MinimumCurrencyUnit_ShouldBeOneCent()
        {
            Assert.Equal(0.01m, EconomicPrecision.MinimumCurrencyUnit);
        }
        
        [Fact]
        public void MaximumPlayerWealth_ShouldBeApproximatelyOneTrillion()
        {
            Assert.Equal(999999999999.99m, EconomicPrecision.MaximumPlayerWealth);
        }
        
        [Fact]
        public void StatisticalPrecision_ShouldBeHighPrecision()
        {
            Assert.Equal(1e-15, EconomicPrecision.StatisticalPrecision);
        }
        
        [Fact]
        public void MinimumCurrencyUnit_ShouldBePositive()
        {
            Assert.True(EconomicPrecision.MinimumCurrencyUnit > 0);
        }
        
        [Fact]
        public void MaximumPlayerWealth_ShouldBeGreaterThanMinimum()
        {
            Assert.True(EconomicPrecision.MaximumPlayerWealth > EconomicPrecision.MinimumCurrencyUnit);
        }
        
        [Fact]
        public void DecimalPrecision_ShouldSupportCurrency()
        {
            // Test that decimal can represent currency values accurately
            decimal testValue = 123.45m;
            Assert.Equal(123.45m, testValue);
            
            // Test that we can add minimum currency units
            decimal result = testValue + EconomicPrecision.MinimumCurrencyUnit;
            Assert.Equal(123.46m, result);
        }
        
        [Fact]
        public void MaximumPlayerWealth_ShouldHaveTwoDecimalPlaces()
        {
            // Verify max wealth has exactly 2 decimal places
            decimal wealth = EconomicPrecision.MaximumPlayerWealth;
            decimal truncated = Math.Truncate(wealth * 100) / 100;
            Assert.Equal(wealth, truncated);
        }
    }
}
