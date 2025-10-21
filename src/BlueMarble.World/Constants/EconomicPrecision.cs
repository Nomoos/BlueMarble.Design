namespace BlueMarble.World.Constants
{
    /// <summary>
    /// Economic precision constants for high-precision financial calculations.
    /// Uses decimal type for monetary values and double for statistical analysis.
    /// </summary>
    public static class EconomicPrecision
    {
        /// <summary>
        /// Minimum currency unit (0.01 - one cent/penny)
        /// Smallest tradeable monetary amount in the game economy
        /// </summary>
        public const decimal MinimumCurrencyUnit = 0.01m;
        
        /// <summary>
        /// Maximum player wealth limit (999,999,999,999.99 - approximately 1 trillion)
        /// Upper bound for individual player wealth accumulation
        /// </summary>
        public const decimal MaximumPlayerWealth = 999999999999.99m;
        
        /// <summary>
        /// Statistical precision for market analysis (1×10⁻¹⁵)
        /// Used for AI calculations and market trend analysis
        /// </summary>
        public const double StatisticalPrecision = 1e-15;
    }
}
