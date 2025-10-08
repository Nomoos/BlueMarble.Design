namespace BlueMarble.World
{
    using BlueMarble.World.Constants;
    
    /// <summary>
    /// Utility methods for working with 3D world coordinates and bounds checking.
    /// Provides validation and zone classification for gameplay mechanics.
    /// </summary>
    public static class Enhanced3DGeometryOps
    {
        /// <summary>
        /// Checks if a coordinate is within the defined gameplay bounds.
        /// </summary>
        /// <param name="x">X coordinate (longitude direction)</param>
        /// <param name="y">Y coordinate (latitude direction)</param>
        /// <param name="z">Z coordinate (altitude/depth)</param>
        /// <returns>True if within gameplay bounds, false otherwise</returns>
        public static bool IsWithinGameplayBounds(long x, long y, long z)
        {
            return z >= Enhanced3DWorldDetail.MaxPlayerDepth && 
                   z <= Enhanced3DWorldDetail.MaxPlayerHeight &&
                   x >= 0 && x < Enhanced3DWorldDetail.WorldSizeX &&
                   y >= 0 && y < Enhanced3DWorldDetail.WorldSizeY;
        }
        
        /// <summary>
        /// Checks if a coordinate is within the total world bounds (including inaccessible areas).
        /// </summary>
        /// <param name="x">X coordinate (longitude direction)</param>
        /// <param name="y">Y coordinate (latitude direction)</param>
        /// <param name="z">Z coordinate (altitude/depth)</param>
        /// <returns>True if within world bounds, false otherwise</returns>
        public static bool IsWithinWorldBounds(long x, long y, long z)
        {
            return x >= 0 && x < Enhanced3DWorldDetail.WorldSizeX &&
                   y >= 0 && y < Enhanced3DWorldDetail.WorldSizeY &&
                   z >= 0 && z < Enhanced3DWorldDetail.WorldSizeZ;
        }
        
        /// <summary>
        /// Determines the accessibility zone for a given altitude/depth.
        /// </summary>
        /// <param name="z">Z coordinate (altitude/depth)</param>
        /// <returns>The accessibility zone classification</returns>
        public static AccessibilityZone DetermineAccessibilityZone(long z)
        {
            return AccessibilityZoneClassifier.DetermineAccessibilityZone(z);
        }
        
        /// <summary>
        /// Calculates the altitude relative to sea level.
        /// </summary>
        /// <param name="z">Z coordinate (absolute altitude)</param>
        /// <returns>Altitude in meters (positive = above sea level, negative = below)</returns>
        public static long GetAltitudeFromSeaLevel(long z)
        {
            return z - Enhanced3DWorldDetail.SeaLevelZ;
        }
        
        /// <summary>
        /// Converts an altitude relative to sea level to absolute Z coordinate.
        /// </summary>
        /// <param name="altitude">Altitude in meters relative to sea level</param>
        /// <returns>Absolute Z coordinate</returns>
        public static long GetZCoordinateFromAltitude(long altitude)
        {
            return Enhanced3DWorldDetail.SeaLevelZ + altitude;
        }
        
        /// <summary>
        /// Checks if a Z coordinate represents a position above sea level.
        /// </summary>
        /// <param name="z">Z coordinate</param>
        /// <returns>True if above sea level, false otherwise</returns>
        public static bool IsAboveSeaLevel(long z)
        {
            return z > Enhanced3DWorldDetail.SeaLevelZ;
        }
        
        /// <summary>
        /// Checks if a Z coordinate represents a position below sea level.
        /// </summary>
        /// <param name="z">Z coordinate</param>
        /// <returns>True if below sea level, false otherwise</returns>
        public static bool IsBelowSeaLevel(long z)
        {
            return z < Enhanced3DWorldDetail.SeaLevelZ;
        }
        
        /// <summary>
        /// Clamps a Z coordinate to gameplay bounds.
        /// </summary>
        /// <param name="z">Z coordinate to clamp</param>
        /// <returns>Clamped Z coordinate within player accessible range</returns>
        public static long ClampToGameplayBounds(long z)
        {
            if (z < Enhanced3DWorldDetail.MaxPlayerDepth)
                return Enhanced3DWorldDetail.MaxPlayerDepth;
            if (z > Enhanced3DWorldDetail.MaxPlayerHeight)
                return Enhanced3DWorldDetail.MaxPlayerHeight;
            return z;
        }
    }
}
