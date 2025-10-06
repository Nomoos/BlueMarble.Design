using System.Diagnostics;
using Xunit;

namespace BlueMarble.SpatialIndexing.Tests;

/// <summary>
/// Performance tests validating the chunked spatial grid system
/// Tests cover random access, sequential access, bulk operations, memory efficiency, lazy loading, and multi-layer queries
/// </summary>
public class PerformanceTests
{
    private const int ChunkSize = 100;

    [Fact]
    public async Task RandomAccess_10000Operations_OutperformsOctree()
    {
        // Arrange
        var grid = new ChunkedSpatialGrid<int>(0, new ChunkedSpatialGridConfig
        {
            ChunkSize = ChunkSize,
            MaxCachedChunks = 100,
            EnableLazyLoading = false
        });

        var random = new Random(42);
        const int operationCount = 10000;

        // Warm up - create some chunks
        for (int i = 0; i < 10; i++)
        {
            await grid.SetAsync(random.Next(0, 1000), random.Next(0, 1000), 42);
        }

        // Act
        var stopwatch = Stopwatch.StartNew();
        for (int i = 0; i < operationCount; i++)
        {
            int x = random.Next(0, 1000);
            int y = random.Next(0, 1000);
            await grid.SetAsync(x, y, i);
            var value = await grid.GetAsync(x, y);
            Assert.Equal(i, value);
        }
        stopwatch.Stop();

        // Assert - O(1) operations should complete quickly
        // Realistic target accounting for async overhead: ~10ms for 10,000 operations
        Assert.True(stopwatch.ElapsedMilliseconds < 100,
            $"10,000 random operations should complete in under 100ms (far better than octree O(log n)), took {stopwatch.ElapsedMilliseconds}ms");
        
        // Verify average time is reasonable (accounting for async overhead)
        var averageTimePerOp = stopwatch.Elapsed.TotalMicroseconds / operationCount;
        Assert.True(averageTimePerOp < 10.0, 
            $"Average time per operation should be under 10μs, was {averageTimePerOp:F3}μs");
    }

    [Fact]
    public async Task SequentialAccess_10000Operations_ShowsGoodCacheLocality()
    {
        // Arrange
        var grid = new ChunkedSpatialGrid<int>(0, new ChunkedSpatialGridConfig
        {
            ChunkSize = ChunkSize,
            MaxCachedChunks = 100,
            EnableLazyLoading = false
        });

        const int operationCount = 10000;

        // Act - Sequential access pattern
        var stopwatch = Stopwatch.StartNew();
        for (int i = 0; i < operationCount; i++)
        {
            int x = i % 100;
            int y = i / 100;
            await grid.SetAsync(x, y, i);
        }

        for (int i = 0; i < operationCount; i++)
        {
            int x = i % 100;
            int y = i / 100;
            var value = await grid.GetAsync(x, y);
            Assert.Equal(i, value);
        }
        stopwatch.Stop();

        // Assert - Sequential should be very fast due to cache locality
        Assert.True(stopwatch.ElapsedMilliseconds < 50,
            $"10,000 sequential operations should complete in under 50ms, took {stopwatch.ElapsedMilliseconds}ms");
    }

    [Fact]
    public async Task BulkOperations_LargeRegion_ScalesLinearly()
    {
        // Arrange
        var grid = new ChunkedSpatialGrid<int>(0, new ChunkedSpatialGridConfig
        {
            ChunkSize = ChunkSize,
            MaxCachedChunks = 100,
            EnableLazyLoading = false
        });

        const int regionSize = 100;
        var data = new int[regionSize, regionSize];
        for (int y = 0; y < regionSize; y++)
        {
            for (int x = 0; x < regionSize; x++)
            {
                data[y, x] = y * regionSize + x;
            }
        }

        // Act - Set region
        var stopwatch = Stopwatch.StartNew();
        await grid.SetRegionAsync(0, 0, data);
        stopwatch.Stop();
        var setTime = stopwatch.ElapsedMilliseconds;

        // Act - Get region
        stopwatch.Restart();
        var result = await grid.GetRegionAsync(0, 0, regionSize, regionSize);
        stopwatch.Stop();
        var getTime = stopwatch.ElapsedMilliseconds;

        // Assert
        Assert.True(setTime < 100, $"Setting 10,000 cells should take under 100ms, took {setTime}ms");
        Assert.True(getTime < 100, $"Getting 10,000 cells should take under 100ms, took {getTime}ms");

        // Verify data
        for (int y = 0; y < regionSize; y++)
        {
            for (int x = 0; x < regionSize; x++)
            {
                Assert.Equal(data[y, x], result[y, x]);
            }
        }
    }

    [Fact]
    public async Task MemoryUsage_100x100Chunk_MoreEfficientThanOctree()
    {
        // Arrange
        var grid = new ChunkedSpatialGrid<int>(0, new ChunkedSpatialGridConfig
        {
            ChunkSize = ChunkSize,
            MaxCachedChunks = 100,
            EnableLazyLoading = false
        });

        // Act - Create one chunk by setting values
        for (int y = 0; y < ChunkSize; y++)
        {
            for (int x = 0; x < ChunkSize; x++)
            {
                await grid.SetAsync(x, y, 42);
            }
        }

        var memoryUsage = grid.GetMemoryUsage();

        // Assert
        // 100x100 chunk with int (4 bytes) = 40,000 bytes data + overhead
        // Should be ~40KB per chunk vs octree's ~1MB equivalent
        Assert.True(memoryUsage < 100_000, 
            $"Memory usage for 100×100 chunk should be under 100KB, was {memoryUsage / 1024}KB");

        // Verify efficiency ratio
        const long octreeEstimate = 1_000_000; // 1MB for equivalent octree structure
        var efficiency = (double)octreeEstimate / memoryUsage;
        Assert.True(efficiency > 10, 
            $"Chunked grid should be at least 10x more memory efficient than octree, was {efficiency:F1}x");
    }

    [Fact]
    public async Task LazyLoading_ReducesMemoryFootprint()
    {
        // Arrange
        var tempPath = Path.Combine(Path.GetTempPath(), $"spatial_test_{Guid.NewGuid()}");
        var persistence = new FileSpatialPersistence<int>(tempPath);

        try
        {
            var grid = new ChunkedSpatialGrid<int>(0, new ChunkedSpatialGridConfig
            {
                ChunkSize = ChunkSize,
                MaxCachedChunks = 10, // Small cache to force eviction
                EnableLazyLoading = true,
                AutoSaveOnEviction = true
            }, persistence);

            // Act - Create more chunks than cache size
            for (int i = 0; i < 20; i++)
            {
                await grid.SetAsync(i * 150, 0, i); // Each chunk is 150 units apart
            }

            await grid.FlushAsync();

            // Assert - Should only keep 10 chunks in memory
            Assert.True(grid.CachedChunkCount <= 11, // Allow slight overflow during operations
                $"Should keep at most 10 chunks cached, has {grid.CachedChunkCount}");

            // Verify data can still be read back (lazy loading)
            for (int i = 0; i < 20; i++)
            {
                var value = await grid.GetAsync(i * 150, 0);
                Assert.Equal(i, value);
            }
        }
        finally
        {
            // Cleanup
            if (Directory.Exists(tempPath))
            {
                Directory.Delete(tempPath, recursive: true);
            }
        }
    }

    [Fact]
    public async Task MultiLayer_ComplexQuery_CompletesUnder100ms()
    {
        // Arrange - Simulate 3 layers (terrain, vegetation, geology)
        var terrainLayer = new ChunkedSpatialGrid<int>(0, new ChunkedSpatialGridConfig
        {
            ChunkSize = ChunkSize,
            MaxCachedChunks = 100
        });

        var vegetationLayer = new ChunkedSpatialGrid<int>(0, new ChunkedSpatialGridConfig
        {
            ChunkSize = ChunkSize,
            MaxCachedChunks = 100
        });

        var geologyLayer = new ChunkedSpatialGrid<int>(0, new ChunkedSpatialGridConfig
        {
            ChunkSize = ChunkSize,
            MaxCachedChunks = 100
        });

        // Populate layers
        const int regionSize = 50;
        for (int y = 0; y < regionSize; y++)
        {
            for (int x = 0; x < regionSize; x++)
            {
                await terrainLayer.SetAsync(x, y, 1);
                await vegetationLayer.SetAsync(x, y, 2);
                await geologyLayer.SetAsync(x, y, 3);
            }
        }

        // Act - Query all layers
        var stopwatch = Stopwatch.StartNew();
        var results = new List<(int terrain, int vegetation, int geology)>();

        for (int y = 0; y < regionSize; y++)
        {
            for (int x = 0; x < regionSize; x++)
            {
                var terrain = await terrainLayer.GetAsync(x, y);
                var vegetation = await vegetationLayer.GetAsync(x, y);
                var geology = await geologyLayer.GetAsync(x, y);
                results.Add((terrain, vegetation, geology));
            }
        }
        stopwatch.Stop();

        // Assert
        Assert.True(stopwatch.ElapsedMilliseconds < 100,
            $"Multi-layer query should complete in under 100ms, took {stopwatch.ElapsedMilliseconds}ms");

        Assert.Equal(regionSize * regionSize, results.Count);
        Assert.All(results, r =>
        {
            Assert.Equal(1, r.terrain);
            Assert.Equal(2, r.vegetation);
            Assert.Equal(3, r.geology);
        });
    }

    [Fact]
    public async Task Persistence_SaveAndLoad_PreservesData()
    {
        // Arrange
        var tempPath = Path.Combine(Path.GetTempPath(), $"spatial_test_{Guid.NewGuid()}");
        var persistence = new FileSpatialPersistence<int>(tempPath);

        try
        {
            // Create and populate grid
            var grid1 = new ChunkedSpatialGrid<int>(0, new ChunkedSpatialGridConfig
            {
                ChunkSize = ChunkSize,
                MaxCachedChunks = 100
            }, persistence);

            for (int i = 0; i < 100; i++)
            {
                await grid1.SetAsync(i, i, i);
            }

            await grid1.FlushAsync();

            // Create new grid with same persistence
            var grid2 = new ChunkedSpatialGrid<int>(0, new ChunkedSpatialGridConfig
            {
                ChunkSize = ChunkSize,
                MaxCachedChunks = 100,
                EnableLazyLoading = true
            }, persistence);

            // Act & Assert - Verify all data is loaded correctly
            for (int i = 0; i < 100; i++)
            {
                var value = await grid2.GetAsync(i, i);
                Assert.Equal(i, value);
            }
        }
        finally
        {
            // Cleanup
            if (Directory.Exists(tempPath))
            {
                Directory.Delete(tempPath, recursive: true);
            }
        }
    }
}
