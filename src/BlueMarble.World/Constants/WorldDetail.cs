namespace BlueMarble.World.Constants
{
    /// <summary>
    /// Core world dimensions for BlueMarble simulation.
    /// Maintains backward compatibility with existing 2D operations.
    /// </summary>
    public static class WorldDetail
    {
        /// <summary>
        /// World size in X dimension (Earth's circumference in meters)
        /// </summary>
        public const long WorldSizeX = 40075020L;
        
        /// <summary>
        /// World size in Y dimension (half circumference in meters, 0 to π)
        /// </summary>
        public const long WorldSizeY = 20037510L;
    }
}
