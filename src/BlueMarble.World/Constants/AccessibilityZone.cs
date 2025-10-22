namespace BlueMarble.World.Constants
{
    /// <summary>
    /// Defines gameplay accessibility zones based on altitude/depth.
    /// Determines what areas players can access with different equipment/technology levels.
    /// </summary>
    public enum AccessibilityZone
    {
        /// <summary>
        /// Surface zone: -1km to +10km above sea level
        /// Normal gameplay area accessible with basic equipment
        /// </summary>
        Surface = 0,
        
        /// <summary>
        /// Deep mining zone: -1km to -50km below sea level
        /// Requires specialized mining equipment and support infrastructure
        /// </summary>
        DeepMining = 1,
        
        /// <summary>
        /// High altitude zone: +10km to +50km above sea level
        /// Accessible via aircraft or space elevator systems
        /// </summary>
        HighAltitude = 2,
        
        /// <summary>
        /// Extreme depth zone: -50km to -100km below sea level
        /// Advanced civilization projects, requires cutting-edge technology
        /// </summary>
        ExtremeDepth = 3,
        
        /// <summary>
        /// Atmospheric high zone: +50km to +100km above sea level
        /// Space program territory, edge of atmosphere
        /// </summary>
        AtmosphericHigh = 4,
        
        /// <summary>
        /// Inaccessible zone: Beyond ±100km from sea level
        /// Geological simulation only, not accessible to players
        /// Includes deep mantle, core, and outer space
        /// </summary>
        Inaccessible = 5
    }
}
