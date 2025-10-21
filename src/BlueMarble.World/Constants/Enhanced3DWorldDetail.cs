namespace BlueMarble.World.Constants
{
    /// <summary>
    /// Enhanced 3D world parameters for full spherical planet simulation.
    /// Extends WorldDetail with Z-axis (altitude/depth) dimension while maintaining
    /// full backward compatibility with existing 2D operations.
    /// 
    /// World Dimensions: 40,075,020 × 20,037,510 × 20,000,000 meters
    /// Coordinate System: 64-bit signed integers (long) for meter-level precision
    /// </summary>
    public static class Enhanced3DWorldDetail
    {
        // ============================================================================
        // Existing 2D World Parameters (Maintained for Compatibility)
        // ============================================================================
        
        /// <summary>
        /// World size in X dimension - Earth's circumference in meters
        /// </summary>
        public const long WorldSizeX = 40075020L;
        
        /// <summary>
        /// World size in Y dimension - Half circumference in meters (0 to π)
        /// </summary>
        public const long WorldSizeY = 20037510L;
        
        // ============================================================================
        // New Z Dimension for Full 3D Octree Implementation
        // ============================================================================
        
        /// <summary>
        /// World size in Z dimension - Total vertical range (±10,000 km from sea level)
        /// Spans from deep core to exosphere
        /// </summary>
        public const long WorldSizeZ = 20000000L;
        
        /// <summary>
        /// Sea level reference point (center of Z-axis at 10,000 km)
        /// All altitudes are measured relative to this reference
        /// </summary>
        public const long SeaLevelZ = WorldSizeZ / 2; // 10,000,000 meters
        
        // ============================================================================
        // Octree Configuration for Spatial Indexing
        // ============================================================================
        
        /// <summary>
        /// Maximum octree depth for 0.25m resolution
        /// Calculation: log₂(40,075,020 / 0.25) ≈ 26 levels
        /// </summary>
        public const int MaxOctreeDepth = 26;
        
        // ============================================================================
        // Geological Reference Levels (Depth/Altitude from Sea Level)
        // ============================================================================
        
        /// <summary>
        /// Atmosphere top boundary - Kármán line (+100 km above sea level)
        /// Marks transition to space environment
        /// </summary>
        public const long AtmosphereTop = SeaLevelZ + 100000;
        
        /// <summary>
        /// Bottom of Earth's crust (-100 km below sea level)
        /// Marks transition to upper mantle
        /// </summary>
        public const long CrustBottom = SeaLevelZ - 100000;
        
        /// <summary>
        /// Bottom of Earth's mantle (-2,900 km below sea level)
        /// Marks transition to outer core
        /// </summary>
        public const long MantleBottom = SeaLevelZ - 2900000;
        
        /// <summary>
        /// Outer core to inner core boundary (-5,150 km below sea level)
        /// Transition from liquid to solid iron-nickel
        /// </summary>
        public const long CoreBoundary = SeaLevelZ - 5150000;
        
        /// <summary>
        /// Earth's geometric center (-6,371 km below sea level)
        /// Center of the planet
        /// </summary>
        public const long CoreCenter = SeaLevelZ - 6371000;
        
        // ============================================================================
        // Gameplay-Relevant Terrain Features
        // ============================================================================
        
        /// <summary>
        /// Maximum natural terrain height (+8,849 m - Mount Everest equivalent)
        /// Highest point on planetary surface
        /// </summary>
        public const long MaxTerrainHeight = SeaLevelZ + 8849;
        
        /// <summary>
        /// Deepest ocean trench (-11,034 m - Mariana Trench equivalent)
        /// Lowest point on planetary surface
        /// </summary>
        public const long DeepestOcean = SeaLevelZ - 11034;
        
        // ============================================================================
        // Player Accessibility Limits
        // ============================================================================
        
        /// <summary>
        /// Maximum depth players can reach (-50 km below sea level)
        /// Deep mining operations limit for gameplay balance
        /// </summary>
        public const long MaxPlayerDepth = SeaLevelZ - 50000;
        
        /// <summary>
        /// Maximum altitude players can reach (+50 km above sea level)
        /// High-altitude exploration limit for gameplay balance
        /// </summary>
        public const long MaxPlayerHeight = SeaLevelZ + 50000;
        
        // ============================================================================
        // Total World Volume Calculation
        // ============================================================================
        
        /// <summary>
        /// Total world volume in cubic meters
        /// 40,075,020 × 20,037,510 × 20,000,000 = 1.604 × 10²² m³
        /// </summary>
        public const decimal TotalWorldVolume = (decimal)WorldSizeX * WorldSizeY * WorldSizeZ;
    }
}
