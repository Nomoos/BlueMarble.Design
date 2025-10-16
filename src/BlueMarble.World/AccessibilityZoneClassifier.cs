namespace BlueMarble.World
{
    using BlueMarble.World.Constants;
    
    /// <summary>
    /// Determines accessibility zones based on altitude/depth from sea level.
    /// Single responsibility: Zone classification logic.
    /// </summary>
    public static class AccessibilityZoneClassifier
    {
        /// <summary>
        /// Determines the accessibility zone for a given altitude/depth.
        /// </summary>
        /// <param name="z">Z coordinate (altitude/depth)</param>
        /// <returns>The accessibility zone classification</returns>
        public static AccessibilityZone DetermineAccessibilityZone(long z)
        {
            long altitude = z - Enhanced3DWorldDetail.SeaLevelZ;
            
            // Beyond ±100km from sea level
            if (IsInaccessible(altitude))
                return AccessibilityZone.Inaccessible;
            
            // High atmospheric zone: +50km to +100km
            if (IsAtmosphericHigh(altitude))
                return AccessibilityZone.AtmosphericHigh;
            
            // Extreme depth: -50km to -100km
            if (IsExtremeDepth(altitude))
                return AccessibilityZone.ExtremeDepth;
            
            // High altitude: +10km to +50km
            if (IsHighAltitude(altitude))
                return AccessibilityZone.HighAltitude;
            
            // Deep mining: -1km to -50km
            if (IsDeepMining(altitude))
                return AccessibilityZone.DeepMining;
            
            // Surface: -1km to +10km
            return AccessibilityZone.Surface;
        }
        
        private static bool IsInaccessible(long altitude)
        {
            return altitude > 100000 || altitude < -100000;
        }
        
        private static bool IsAtmosphericHigh(long altitude)
        {
            return altitude >= 50000 && altitude <= 100000;
        }
        
        private static bool IsExtremeDepth(long altitude)
        {
            return altitude <= -50000 && altitude >= -100000;
        }
        
        private static bool IsHighAltitude(long altitude)
        {
            return altitude >= 10000 && altitude < 50000;
        }
        
        private static bool IsDeepMining(long altitude)
        {
            return altitude <= -1000 && altitude > -50000;
        }
    }
}
