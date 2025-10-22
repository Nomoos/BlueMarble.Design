namespace BlueMarble.World
{
    using BlueMarble.World.Constants;
    
    /// <summary>
    /// Provides altitude conversion and sea level comparison utilities.
    /// Single responsibility: Altitude calculations and sea level comparisons.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This class handles conversions between:
    /// - Absolute Z coordinates (0 to WorldSizeZ)
    /// - Relative altitude (meters from sea level)
    /// </para>
    /// <para>
    /// Example: Sea level is at Z = 10,000,000m. An altitude of +8,849m (Mt. Everest)
    /// corresponds to Z = 10,008,849m.
    /// </para>
    /// </remarks>
    public static class AltitudeConverter
    {
        /// <summary>
        /// Calculates the altitude relative to sea level.
        /// </summary>
        /// <param name="z">Z coordinate (absolute altitude)</param>
        /// <returns>Altitude in meters (positive = above sea level, negative = below)</returns>
        /// <example>
        /// <code>
        /// // Mt. Everest equivalent
        /// long z = 10008849;
        /// long altitude = AltitudeConverter.GetAltitudeFromSeaLevel(z);
        /// // altitude = 8849 (meters above sea level)
        /// </code>
        /// </example>
        public static long GetAltitudeFromSeaLevel(long z)
        {
            return z - Enhanced3DWorldDetail.SeaLevelZ;
        }
        
        /// <summary>
        /// Converts an altitude relative to sea level to absolute Z coordinate.
        /// </summary>
        /// <param name="altitude">Altitude in meters relative to sea level (positive = above, negative = below)</param>
        /// <returns>Absolute Z coordinate</returns>
        /// <example>
        /// <code>
        /// // Convert -11,034m (Mariana Trench) to Z coordinate
        /// long altitude = -11034;
        /// long z = AltitudeConverter.GetZCoordinateFromAltitude(altitude);
        /// // z = 9988966
        /// </code>
        /// </example>
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
        /// <returns>Clamped Z coordinate within player accessible range (-50km to +50km from sea level)</returns>
        /// <example>
        /// <code>
        /// // Attempting to access extreme depth
        /// long z = 9000000; // -1,000km below sea level
        /// long clamped = AltitudeConverter.ClampToGameplayBounds(z);
        /// // clamped = 9950000 (-50km, max player depth)
        /// </code>
        /// </example>
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
