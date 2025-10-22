using BlueMarble.SpatialIndexing;
using System.Diagnostics;

namespace BlueMarble.Examples;

/// <summary>
/// Comprehensive examples demonstrating all major use cases of the Spatial Indexing system
/// </summary>
public class SpatialIndexingExample
{
    /// <summary>
    /// Example 1: Basic Usage
    /// Demonstrates simple get/set operations with default configuration
    /// </summary>
    public static async Task BasicUsageExample()
    {
        Console.WriteLine("=== Example 1: Basic Usage ===\n");

        // Create a grid with default configuration
        var grid = new ChunkedSpatialGrid<int>(defaultValue: 0);

        // Set some values
        Console.WriteLine("Setting values...");
        await grid.SetAsync(100, 200, 42);
        await grid.SetAsync(101, 200, 43);
        await grid.SetAsync(100, 201, 44);

        // Read values back
        Console.WriteLine("Reading values back...");
        int value1 = await grid.GetAsync(100, 200);
        int value2 = await grid.GetAsync(101, 200);
        int value3 = await grid.GetAsync(100, 201);

        Console.WriteLine($"Value at (100, 200): {value1}");
        Console.WriteLine($"Value at (101, 200): {value2}");
        Console.WriteLine($"Value at (100, 201): {value3}");

        // Get memory usage
        long memoryUsage = grid.GetMemoryUsage();
        Console.WriteLine($"Memory usage: {memoryUsage / 1024}KB");
        Console.WriteLine($"Cached chunks: {grid.CachedChunkCount}");

        await grid.DisposeAsync();
        Console.WriteLine();
    }

    /// <summary>
    /// Example 2: Multi-Layer Data
    /// Demonstrates managing multiple data layers (terrain, vegetation, geology)
    /// </summary>
    public static async Task MultiLayerExample()
    {
        Console.WriteLine("=== Example 2: Multi-Layer Data ===\n");

        // Create separate layers for different data types
        var terrainLayer = new ChunkedSpatialGrid<byte>(defaultValue: 0);
        var vegetationLayer = new ChunkedSpatialGrid<byte>(defaultValue: 0);
        var geologyLayer = new ChunkedSpatialGrid<ushort>(defaultValue: 0);

        // Define some test data
        const int regionSize = 50;
        Console.WriteLine($"Populating {regionSize}×{regionSize} region across 3 layers...");

        var stopwatch = Stopwatch.StartNew();

        // Populate all layers
        for (int y = 0; y < regionSize; y++)
        {
            for (int x = 0; x < regionSize; x++)
            {
                await terrainLayer.SetAsync(x, y, (byte)(x % 10));      // Terrain type 0-9
                await vegetationLayer.SetAsync(x, y, (byte)(y % 5));    // Vegetation type 0-4
                await geologyLayer.SetAsync(x, y, (ushort)(x * y));     // Geology value
            }
        }

        stopwatch.Stop();
        Console.WriteLine($"Population completed in {stopwatch.ElapsedMilliseconds}ms");

        // Query all layers at specific positions
        Console.WriteLine("\nQuerying layers at position (25, 25):");
        var terrain = await terrainLayer.GetAsync(25, 25);
        var vegetation = await vegetationLayer.GetAsync(25, 25);
        var geology = await geologyLayer.GetAsync(25, 25);

        Console.WriteLine($"  Terrain: {terrain}");
        Console.WriteLine($"  Vegetation: {vegetation}");
        Console.WriteLine($"  Geology: {geology}");

        // Memory usage per layer
        Console.WriteLine("\nMemory usage per layer:");
        Console.WriteLine($"  Terrain: {terrainLayer.GetMemoryUsage() / 1024}KB");
        Console.WriteLine($"  Vegetation: {vegetationLayer.GetMemoryUsage() / 1024}KB");
        Console.WriteLine($"  Geology: {geologyLayer.GetMemoryUsage() / 1024}KB");

        await terrainLayer.DisposeAsync();
        await vegetationLayer.DisposeAsync();
        await geologyLayer.DisposeAsync();
        Console.WriteLine();
    }

    /// <summary>
    /// Example 3: Bulk Operations
    /// Demonstrates efficient bulk set/get operations for large regions
    /// </summary>
    public static async Task BulkOperationsExample()
    {
        Console.WriteLine("=== Example 3: Bulk Operations ===\n");

        var grid = new ChunkedSpatialGrid<int>(defaultValue: 0);

        // Create a 100×100 region of data
        const int size = 100;
        var data = new int[size, size];

        Console.WriteLine($"Creating {size}×{size} data array...");
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                data[y, x] = y * size + x;
            }
        }

        // Bulk set operation
        Console.WriteLine("Performing bulk set operation...");
        var stopwatch = Stopwatch.StartNew();
        await grid.SetRegionAsync(worldX: 0, worldY: 0, values: data);
        stopwatch.Stop();
        Console.WriteLine($"Bulk set completed in {stopwatch.ElapsedMilliseconds}ms");

        // Bulk get operation
        Console.WriteLine("Performing bulk get operation...");
        stopwatch.Restart();
        var retrieved = await grid.GetRegionAsync(worldX: 0, worldY: 0, width: size, height: size);
        stopwatch.Stop();
        Console.WriteLine($"Bulk get completed in {stopwatch.ElapsedMilliseconds}ms");

        // Verify some values
        Console.WriteLine("\nVerifying values:");
        Console.WriteLine($"  (0, 0): {retrieved[0, 0]} (expected: 0)");
        Console.WriteLine($"  (50, 50): {retrieved[50, 50]} (expected: {50 * size + 50})");
        Console.WriteLine($"  (99, 99): {retrieved[99, 99]} (expected: {99 * size + 99})");

        await grid.DisposeAsync();
        Console.WriteLine();
    }

    /// <summary>
    /// Example 4: Persistence with File Storage
    /// Demonstrates saving and loading data with file-based persistence
    /// </summary>
    public static async Task PersistenceExample()
    {
        Console.WriteLine("=== Example 4: Persistence with File Storage ===\n");

        var tempPath = Path.Combine(Path.GetTempPath(), $"spatial_example_{Guid.NewGuid()}");
        Console.WriteLine($"Using temporary storage: {tempPath}");

        try
        {
            // Create persistence provider
            var persistence = new FileSpatialPersistence<int>(tempPath);

            // Create grid with persistence enabled
            var grid = new ChunkedSpatialGrid<int>(
                defaultValue: 0,
                config: new ChunkedSpatialGridConfig
                {
                    ChunkSize = 100,
                    MaxCachedChunks = 50,
                    EnableLazyLoading = true,
                    AutoSaveOnEviction = true,
                    AutoFlushInterval = TimeSpan.FromSeconds(5)
                },
                persistence: persistence
            );

            // Write data
            Console.WriteLine("Writing data...");
            for (int i = 0; i < 200; i++)
            {
                await grid.SetAsync(i, i, i * 100);
            }

            // Force save
            Console.WriteLine("Flushing to disk...");
            await grid.FlushAsync();
            await grid.DisposeAsync();

            Console.WriteLine("Grid disposed. Creating new grid with same persistence...");

            // Create new grid with same persistence
            var grid2 = new ChunkedSpatialGrid<int>(
                defaultValue: 0,
                config: new ChunkedSpatialGridConfig
                {
                    ChunkSize = 100,
                    MaxCachedChunks = 50,
                    EnableLazyLoading = true
                },
                persistence: persistence
            );

            // Read data back (lazy loading from disk)
            Console.WriteLine("Reading data back (lazy loading)...");
            var value0 = await grid2.GetAsync(0, 0);
            var value50 = await grid2.GetAsync(50, 50);
            var value100 = await grid2.GetAsync(100, 100);
            var value199 = await grid2.GetAsync(199, 199);

            Console.WriteLine($"Value at (0, 0): {value0} (expected: 0)");
            Console.WriteLine($"Value at (50, 50): {value50} (expected: 5000)");
            Console.WriteLine($"Value at (100, 100): {value100} (expected: 10000)");
            Console.WriteLine($"Value at (199, 199): {value199} (expected: 19900)");

            await grid2.DisposeAsync();
        }
        finally
        {
            // Cleanup
            if (Directory.Exists(tempPath))
            {
                Console.WriteLine("Cleaning up temporary files...");
                Directory.Delete(tempPath, recursive: true);
            }
        }

        Console.WriteLine();
    }

    /// <summary>
    /// Example 5: Memory Management and LRU Eviction
    /// Demonstrates automatic cache management with LRU eviction
    /// </summary>
    public static async Task MemoryManagementExample()
    {
        Console.WriteLine("=== Example 5: Memory Management and LRU Eviction ===\n");

        var grid = new ChunkedSpatialGrid<int>(
            defaultValue: 0,
            config: new ChunkedSpatialGridConfig
            {
                ChunkSize = 100,
                MaxCachedChunks = 10, // Small cache to demonstrate eviction
                EnableLazyLoading = false
            }
        );

        Console.WriteLine($"Max cached chunks: {10}");
        Console.WriteLine("Creating 20 chunks (will trigger eviction)...\n");

        // Create more chunks than the cache can hold
        for (int i = 0; i < 20; i++)
        {
            // Each chunk is 150 units apart (in different chunk coordinates)
            await grid.SetAsync(i * 150, 0, i);
            
            Console.WriteLine($"Created chunk {i}, cached chunks: {grid.CachedChunkCount}, " +
                            $"memory: {grid.GetMemoryUsage() / 1024}KB");
        }

        Console.WriteLine($"\nFinal cached chunks: {grid.CachedChunkCount} (LRU eviction kept it at ~10)");

        // Access old chunks (should still work via recreation)
        Console.WriteLine("\nAccessing previously evicted chunks:");
        var value0 = await grid.GetAsync(0, 0);
        var value5 = await grid.GetAsync(5 * 150, 0);
        var value10 = await grid.GetAsync(10 * 150, 0);

        Console.WriteLine($"Value at chunk 0: {value0}");
        Console.WriteLine($"Value at chunk 5: {value5}");
        Console.WriteLine($"Value at chunk 10: {value10}");

        await grid.DisposeAsync();
        Console.WriteLine();
    }

    /// <summary>
    /// Example 6: Performance Benchmarking
    /// Demonstrates performance characteristics with timing measurements
    /// </summary>
    public static async Task PerformanceBenchmarkExample()
    {
        Console.WriteLine("=== Example 6: Performance Benchmarking ===\n");

        var grid = new ChunkedSpatialGrid<int>(
            defaultValue: 0,
            config: new ChunkedSpatialGridConfig
            {
                ChunkSize = 100,
                MaxCachedChunks = 100,
                EnableLazyLoading = false
            }
        );

        var random = new Random(42);
        const int operationCount = 10000;

        // Benchmark random access
        Console.WriteLine($"Benchmarking {operationCount} random access operations...");
        var stopwatch = Stopwatch.StartNew();

        for (int i = 0; i < operationCount; i++)
        {
            int x = random.Next(0, 1000);
            int y = random.Next(0, 1000);
            await grid.SetAsync(x, y, i);
        }

        stopwatch.Stop();
        var avgWrite = stopwatch.Elapsed.TotalMicroseconds / operationCount;
        Console.WriteLine($"Random write: {stopwatch.ElapsedMilliseconds}ms total, {avgWrite:F3}μs per operation");

        // Benchmark random reads
        stopwatch.Restart();

        for (int i = 0; i < operationCount; i++)
        {
            int x = random.Next(0, 1000);
            int y = random.Next(0, 1000);
            _ = await grid.GetAsync(x, y);
        }

        stopwatch.Stop();
        var avgRead = stopwatch.Elapsed.TotalMicroseconds / operationCount;
        Console.WriteLine($"Random read: {stopwatch.ElapsedMilliseconds}ms total, {avgRead:F3}μs per operation");

        // Benchmark sequential access
        Console.WriteLine($"\nBenchmarking {operationCount} sequential access operations...");
        stopwatch.Restart();

        for (int i = 0; i < operationCount; i++)
        {
            int x = i % 100;
            int y = i / 100;
            await grid.SetAsync(x, y, i);
        }

        stopwatch.Stop();
        Console.WriteLine($"Sequential write: {stopwatch.ElapsedMilliseconds}ms (excellent cache locality)");

        stopwatch.Restart();

        for (int i = 0; i < operationCount; i++)
        {
            int x = i % 100;
            int y = i / 100;
            _ = await grid.GetAsync(x, y);
        }

        stopwatch.Stop();
        Console.WriteLine($"Sequential read: {stopwatch.ElapsedMilliseconds}ms (excellent cache locality)");

        // Memory efficiency
        Console.WriteLine($"\nMemory usage: {grid.GetMemoryUsage() / 1024}KB for {grid.CachedChunkCount} cached chunks");
        Console.WriteLine($"Average per chunk: {grid.GetMemoryUsage() / grid.CachedChunkCount / 1024}KB");

        await grid.DisposeAsync();
        Console.WriteLine();
    }

    /// <summary>
    /// Main entry point to run all examples
    /// </summary>
    public static async Task Main(string[] args)
    {
        Console.WriteLine("BlueMarble Spatial Indexing System - Examples\n");
        Console.WriteLine("=".PadRight(50, '='));
        Console.WriteLine();

        try
        {
            await BasicUsageExample();
            await MultiLayerExample();
            await BulkOperationsExample();
            await PersistenceExample();
            await MemoryManagementExample();
            await PerformanceBenchmarkExample();

            Console.WriteLine("=".PadRight(50, '='));
            Console.WriteLine("\nAll examples completed successfully!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\nError running examples: {ex.Message}");
            Console.WriteLine(ex.StackTrace);
        }
    }
}
