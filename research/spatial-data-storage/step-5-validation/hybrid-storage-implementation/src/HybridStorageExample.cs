using System;
using System.Collections.Generic;

namespace BlueMarble.SpatialStorage
{
    /// <summary>
    /// Practical usage examples demonstrating hybrid storage capabilities
    /// Shows 5 different scenarios: ocean initialization, coastal transition, 
    /// urban development, query performance, and memory management
    /// </summary>
    public class HybridStorageExample
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("BlueMarble Hybrid Storage System - Usage Examples\n");
            Console.WriteLine("=================================================\n");

            // Example 1: Ocean Initialization (demonstrates 90%+ memory savings)
            Example1_OceanInitialization();

            // Example 2: Coastal Transition (demonstrates seamless LOD switching)
            Example2_CoastalTransition();

            // Example 3: Urban Development (demonstrates high-resolution grid performance)
            Example3_UrbanDevelopment();

            // Example 4: Query Performance Comparison
            Example4_QueryPerformance();

            // Example 5: Memory Management
            Example5_MemoryManagement();

            Console.WriteLine("\nAll examples completed successfully!");
        }

        /// <summary>
        /// Example 1: Initialize large ocean region with single material
        /// Demonstrates memory efficiency through material inheritance
        /// </summary>
        public static void Example1_OceanInitialization()
        {
            Console.WriteLine("Example 1: Ocean Initialization");
            Console.WriteLine("--------------------------------");

            var manager = new HybridStorageManager();

            // Define Pacific Ocean region (~10,000 km²)
            var pacificRegion = new Envelope3D(
                -5000000, -2500000, -5000,  // 5000km x 2500km x 5km depth
                5000000, 2500000, 0
            );

            Console.WriteLine($"Initializing ocean region: {pacificRegion.Width / 1000:N0} km x {pacificRegion.Height / 1000:N0} km");

            manager.InitializeHomogeneousRegion(pacificRegion, MaterialId.Ocean);

            // Query a few points to verify
            var testPoint = new Vector3(1000000, 500000, -1000);
            var material = manager.QueryMaterial(testPoint, 5);

            Console.WriteLine($"Material at test point: {material}");

            var stats = manager.GetStatistics();
            Console.WriteLine($"Memory savings: {stats.OctreeMemorySavings:F1}%");
            Console.WriteLine($"Octree nodes: {stats.OctreeNodes}");
            Console.WriteLine();
        }

        /// <summary>
        /// Example 2: Coastal transition zone
        /// Demonstrates seamless switching between octree and grid
        /// </summary>
        public static void Example2_CoastalTransition()
        {
            Console.WriteLine("Example 2: Coastal Transition");
            Console.WriteLine("------------------------------");

            var manager = new HybridStorageManager();

            // Initialize ocean and land regions
            var oceanRegion = new Envelope3D(-10000, -10000, -100, 0, 10000, 0);
            var landRegion = new Envelope3D(0, -10000, 0, 10000, 10000, 100);

            manager.InitializeHomogeneousRegion(oceanRegion, MaterialId.Ocean);
            manager.InitializeHomogeneousRegion(landRegion, MaterialId.Sand);

            // Query across transition at different LODs
            var coastalPoint = new Vector3(-50, 0, 0);

            Console.WriteLine("Querying coastal transition at different LODs:");
            for (int lod = 5; lod <= 15; lod += 5)
            {
                var material = manager.QueryMaterial(coastalPoint, lod);
                string source = lod <= 12 ? "Octree" : "Grid";
                Console.WriteLine($"  LOD {lod,2}: {material,-10} (Source: {source})");
            }

            var stats = manager.GetStatistics();
            Console.WriteLine($"\nQueries: {stats.TotalQueries} (Octree: {stats.OctreeQueryPercent:F1}%, Grid: {stats.GridQueryPercent:F1}%)");
            Console.WriteLine();
        }

        /// <summary>
        /// Example 3: Urban development area with high-resolution details
        /// Demonstrates grid tile performance for dense data
        /// </summary>
        public static void Example3_UrbanDevelopment()
        {
            Console.WriteLine("Example 3: Urban Development");
            Console.WriteLine("----------------------------");

            var manager = new HybridStorageManager(
                transitionLevel: 12,
                maxActiveTiles: 50,
                defaultCellSize: 0.25  // 25cm resolution
            );

            // Urban area: 1km x 1km with detailed buildings
            var urbanRegion = new Envelope3D(
                370000, 5810000, 0,    // UTM coordinates
                371000, 5811000, 200   // 1km x 1km x 200m height
            );

            Console.WriteLine($"Urban region: {urbanRegion.Width}m x {urbanRegion.Height}m");

            // Simulate building materials at high resolution
            Console.WriteLine("Setting building materials...");
            for (double x = urbanRegion.MinX; x < urbanRegion.MaxX; x += 10)
            {
                for (double y = urbanRegion.MinY; y < urbanRegion.MaxY; y += 10)
                {
                    // Simple pattern: buildings every 50m
                    bool isBuilding = ((int)(x / 50) + (int)(y / 50)) % 3 == 0;
                    MaterialId material = isBuilding ? MaterialId.Rock : MaterialId.Vegetation;

                    var position = new Vector3(x, y, 10);
                    manager.UpdateMaterial(position, material, 13);
                }
            }

            Console.WriteLine("Materials set successfully.");

            // Query performance test
            var queryPoint = new Vector3(370500, 5810500, 10);
            var material = manager.QueryMaterial(queryPoint, 13);
            Console.WriteLine($"Material at query point: {material}");

            var stats = manager.GetStatistics();
            Console.WriteLine($"Active tiles: {stats.ActiveTiles}");
            Console.WriteLine($"Total tile memory: {stats.TotalTileMemory / (1024.0 * 1024.0):F2} MB");
            Console.WriteLine($"Grid queries: {stats.GridQueries}");
            Console.WriteLine();
        }

        /// <summary>
        /// Example 4: Query performance comparison
        /// Demonstrates O(log n) vs O(1) performance characteristics
        /// </summary>
        public static void Example4_QueryPerformance()
        {
            Console.WriteLine("Example 4: Query Performance Comparison");
            Console.WriteLine("---------------------------------------");

            var manager = new HybridStorageManager();

            // Initialize test region
            var testRegion = new Envelope3D(-1000, -1000, 0, 1000, 1000, 100);
            manager.InitializeHomogeneousRegion(testRegion, MaterialId.Sand);

            Console.WriteLine("Running performance tests...");

            // Test octree queries (LOD 10)
            int octreeQueries = 10000;
            var random = new Random(42);
            var startTime = DateTime.UtcNow;

            for (int i = 0; i < octreeQueries; i++)
            {
                var pos = new Vector3(
                    random.NextDouble() * 2000 - 1000,
                    random.NextDouble() * 2000 - 1000,
                    random.NextDouble() * 100
                );
                manager.QueryMaterial(pos, 10);
            }

            var octreeTime = (DateTime.UtcNow - startTime).TotalMilliseconds;

            // Test grid queries (LOD 13)
            int gridQueries = 10000;
            startTime = DateTime.UtcNow;

            for (int i = 0; i < gridQueries; i++)
            {
                var pos = new Vector3(
                    random.NextDouble() * 2000 - 1000,
                    random.NextDouble() * 2000 - 1000,
                    random.NextDouble() * 100
                );
                manager.QueryMaterial(pos, 13);
            }

            var gridTime = (DateTime.UtcNow - startTime).TotalMilliseconds;

            Console.WriteLine($"Octree queries: {octreeQueries} in {octreeTime:F2}ms ({octreeTime / octreeQueries:F4}ms per query)");
            Console.WriteLine($"Grid queries: {gridQueries} in {gridTime:F2}ms ({gridTime / gridQueries:F4}ms per query)");

            var stats = manager.GetStatistics();
            Console.WriteLine($"Octree cache hit rate: {stats.OctreeCacheHitRate:P2}");
            Console.WriteLine();
        }

        /// <summary>
        /// Example 5: Memory management and LRU cache
        /// Demonstrates automatic tile eviction under memory pressure
        /// </summary>
        public static void Example5_MemoryManagement()
        {
            Console.WriteLine("Example 5: Memory Management");
            Console.WriteLine("----------------------------");

            // Create manager with limited tile cache
            var manager = new HybridStorageManager(
                transitionLevel: 12,
                maxActiveTiles: 10,  // Small cache to demonstrate eviction
                defaultCellSize: 1.0
            );

            Console.WriteLine("Creating tiles to trigger eviction...");

            // Create 20 tiles (will trigger evictions)
            for (int i = 0; i < 20; i++)
            {
                var position = new Vector3(i * 5000, 0, 0);
                manager.QueryMaterial(position, 13);
            }

            var stats = manager.GetStatistics();
            Console.WriteLine($"Active tiles: {stats.ActiveTiles} (max: 10)");
            Console.WriteLine($"Tile loads: {stats.TileLoads}");
            Console.WriteLine($"Tile evictions: {stats.TileEvictions}");
            Console.WriteLine($"Total tile memory: {stats.TotalTileMemory / (1024.0 * 1024.0):F2} MB");

            // Clear cache
            Console.WriteLine("\nClearing tile cache...");
            manager.ClearTileCache();

            stats = manager.GetStatistics();
            Console.WriteLine($"Active tiles after clear: {stats.ActiveTiles}");
            Console.WriteLine($"Total evictions: {stats.TileEvictions}");
            Console.WriteLine();
        }

        /// <summary>
        /// Comprehensive demonstration of all features
        /// </summary>
        public static void ComprehensiveDemo()
        {
            Console.WriteLine("=== COMPREHENSIVE HYBRID STORAGE DEMO ===\n");

            var manager = new HybridStorageManager(
                transitionLevel: 12,
                maxActiveTiles: 100,
                defaultCellSize: 0.25
            );

            // 1. Initialize global ocean
            Console.WriteLine("1. Initializing global ocean...");
            var globalOcean = new Envelope3D(
                -20000000, -20000000, -11000,
                20000000, 20000000, 0
            );
            manager.InitializeHomogeneousRegion(globalOcean, MaterialId.Ocean);

            // 2. Add continents
            Console.WriteLine("2. Adding continental regions...");
            var northAmerica = new Envelope3D(
                -15000000, 2000000, 0,
                -5000000, 10000000, 5000
            );
            manager.InitializeHomogeneousRegion(northAmerica, MaterialId.Rock);

            // 3. Query different scales
            Console.WriteLine("3. Querying at multiple scales...");
            var testPoints = new List<(Vector3 pos, int lod, string description)>
            {
                (new Vector3(0, 0, -1000), 5, "Deep ocean, global scale"),
                (new Vector3(-10000000, 5000000, 0), 10, "Continental, regional scale"),
                (new Vector3(-10000000, 5000000, 0), 13, "Continental, local scale"),
                (new Vector3(-10000100, 5000100, 10), 15, "Continental, detail scale")
            };

            foreach (var (pos, lod, description) in testPoints)
            {
                var material = manager.QueryMaterial(pos, lod);
                Console.WriteLine($"  {description}: {material}");
            }

            // 4. Display final statistics
            Console.WriteLine("\n4. Final Statistics:");
            var stats = manager.GetStatistics();
            Console.WriteLine(stats);
        }
    }
}
