namespace BlueMarble.World
{
    using BlueMarble.World.Constants;
    
    /// <summary>
    /// Facade for 3D world coordinate operations.
    /// Provides a unified interface to coordinate validation, altitude conversion, and zone classification.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This class follows the Facade pattern, delegating to specialized classes:
    /// - <see cref="CoordinateValidator"/> for bounds checking
    /// - <see cref="AltitudeConverter"/> for altitude calculations
    /// - <see cref="AccessibilityZoneClassifier"/> for zone determination
    /// </para>
    /// <para>
    /// Maintains backward compatibility while following Single Responsibility Principle.
    /// </para>
    /// </remarks>
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
            return CoordinateValidator.IsWithinGameplayBounds(x, y, z);
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
            return CoordinateValidator.IsWithinWorldBounds(x, y, z);
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
            return AltitudeConverter.GetAltitudeFromSeaLevel(z);
        }
        
        /// <summary>
        /// Converts an altitude relative to sea level to absolute Z coordinate.
        /// </summary>
        /// <param name="altitude">Altitude in meters relative to sea level</param>
        /// <returns>Absolute Z coordinate</returns>
        public static long GetZCoordinateFromAltitude(long altitude)
        {
            return AltitudeConverter.GetZCoordinateFromAltitude(altitude);
        }
        
        /// <summary>
        /// Checks if a Z coordinate represents a position above sea level.
        /// </summary>
        /// <param name="z">Z coordinate</param>
        /// <returns>True if above sea level, false otherwise</returns>
        public static bool IsAboveSeaLevel(long z)
        {
            return AltitudeConverter.IsAboveSeaLevel(z);
        }
        
        /// <summary>
        /// Checks if a Z coordinate represents a position below sea level.
        /// </summary>
        /// <param name="z">Z coordinate</param>
        /// <returns>True if below sea level, false otherwise</returns>
        public static bool IsBelowSeaLevel(long z)
        {
            return AltitudeConverter.IsBelowSeaLevel(z);
        }
        
        /// <summary>
        /// Clamps a Z coordinate to gameplay bounds.
        /// </summary>
        /// <param name="z">Z coordinate to clamp</param>
        /// <returns>Clamped Z coordinate within player accessible range</returns>
        public static long ClampToGameplayBounds(long z)
        {
            return AltitudeConverter.ClampToGameplayBounds(z);
        }
    }
}
