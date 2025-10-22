namespace BlueMarble.World
{
    using BlueMarble.World.Constants;
    
    /// <summary>
    /// Validates coordinates against world and gameplay bounds.
    /// Single responsibility: Coordinate validation logic.
    /// </summary>
    public static class CoordinateValidator
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
            return IsWithinPlayableDepth(z) && 
                   IsWithinHorizontalBounds(x, y);
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
            return IsWithinHorizontalBounds(x, y) &&
                   IsWithinVerticalBounds(z);
        }
        
        private static bool IsWithinHorizontalBounds(long x, long y)
        {
            return x >= 0 && x < Enhanced3DWorldDetail.WorldSizeX &&
                   y >= 0 && y < Enhanced3DWorldDetail.WorldSizeY;
        }
        
        private static bool IsWithinVerticalBounds(long z)
        {
            return z >= 0 && z < Enhanced3DWorldDetail.WorldSizeZ;
        }
        
        private static bool IsWithinPlayableDepth(long z)
        {
            return z >= Enhanced3DWorldDetail.MaxPlayerDepth && 
                   z <= Enhanced3DWorldDetail.MaxPlayerHeight;
        }
    }
}
