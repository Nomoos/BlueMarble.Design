# Hybrid Octree + Array Index: Documentation and Optimization Guide

## Overview

This comprehensive guide provides setup instructions, configuration best practices, performance optimization techniques, and troubleshooting for the Hybrid Octree + Array Index storage system.

## Table of Contents

1. [Getting Started](#getting-started)
2. [Configuration Guide](#configuration-guide)
3. [Performance Optimization](#performance-optimization)
4. [Monitoring and Diagnostics](#monitoring-and-diagnostics)
5. [Troubleshooting](#troubleshooting)
6. [Best Practices](#best-practices)
7. [Advanced Topics](#advanced-topics)

---

## Getting Started

### Prerequisites

- .NET 6.0 or higher
- Zarr storage backend (or alternative: HDF5, PostgreSQL)
- Minimum 8GB RAM for development
- Recommended 32GB+ RAM for production

### Installation

```bash
# Install dependencies
dotnet add package System.Numerics.Vectors
dotnet add package System.Threading.Tasks.Dataflow

# For Zarr support (Python interop)
pip install zarr numcodecs

# For HDF5 support
dotnet add package HDF.PInvoke
```

### Quick Start Example

```csharp
using System;
using System.Numerics;
using System.Threading.Tasks;
using BlueMarble.Storage.Hybrid;
using BlueMarble.Storage.Hybrid.Zarr;
using BlueMarble.Storage.Hybrid.Delta;
using BlueMarble.Storage.Hybrid.Querying;

public class QuickStart
{
    public static async Task Main()
    {
        // 1. Create storage configuration
        var config = new HybridStorageConfig
        {
            ChunkSize = 128,
            IndexAccelerationThreshold = 12,
            MaxConcurrentRebuilds = 4
        };

        // 2. Initialize storage system
        var storage = await InitializeStorageAsync(config);

        // 3. Write data
        var position = new Vector3(100, 200, 50);
        var material = new MaterialId(42);
        await storage.UpdateMaterialAsync(position, material);
        Console.WriteLine($"Updated material at {position}");

        // 4. Read data
        var retrievedMaterial = await storage.QueryMaterialAsync(position);
        Console.WriteLine($"Material at {position}: {retrievedMaterial.Value}");

        // 5. Query region
        var bounds = new BoundingBox(
            new Vector3(0, 0, 0),
            new Vector3(1000, 1000, 100)
        );
        var regionResult = await storage.QueryRegionAsync(bounds, targetLOD: 10);
        Console.WriteLine($"Found {regionResult.HomogeneousRegions.Count} regions");
    }

    private static async Task<HybridStorageManager> InitializeStorageAsync(
        HybridStorageConfig config)
    {
        // Setup Zarr primary storage
        var zarrConfig = new ZarrStorageConfig
        {
            DatasetPath = "/data/bluemarble/materials",
            ChunkSize = config.ChunkSize,
            CacheSizeBytes = 2L * 1024 * 1024 * 1024 // 2GB
        };

        var zarrClient = new ZarrClient();
        var primaryStorage = new ZarrChunkedArrayStore(zarrConfig, zarrClient);

        // Setup octree index
        var octreeIndex = new MaterialOctreeIndex(primaryStorage, maxDepth: 20);

        // Setup rebuild manager
        var rebuildManager = new IndexRebuildManager(
            octreeIndex,
            primaryStorage,
            config.MaxConcurrentRebuilds
        );

        // Create unified storage manager
        return new HybridStorageManager(
            primaryStorage,
            octreeIndex,
            rebuildManager,
            config
        );
    }
}
```

---

## Configuration Guide

### Storage Configuration

#### Chunk Size Selection

```csharp
public class ChunkSizeGuide
{
    // Recommended chunk sizes based on use case
    public static readonly Dictionary<string, int> RecommendedChunkSizes = new()
    {
        ["HighDetailUrban"] = 64,      // 64³ cells, ~512KB per chunk
        ["Standard"] = 128,             // 128³ cells, ~4MB per chunk (default)
        ["LargeScaleTerrain"] = 256,   // 256³ cells, ~32MB per chunk
        ["LowMemory"] = 32              // 32³ cells, ~64KB per chunk
    };

    public static int CalculateOptimalChunkSize(
        float expectedUpdateDensity,
        long availableMemoryBytes)
    {
        // High update density → smaller chunks (more granular updates)
        if (expectedUpdateDensity > 0.1f)
            return 64;

        // Low memory → smaller chunks
        if (availableMemoryBytes < 4L * 1024 * 1024 * 1024) // < 4GB
            return 64;

        // Default for most cases
        return 128;
    }
}
```

#### Compression Configuration

```csharp
public class CompressionConfig
{
    public CompressionCodec Codec { get; set; }
    public int CompressionLevel { get; set; }

    public static CompressionConfig ForWorkload(WorkloadType workload)
    {
        return workload switch
        {
            WorkloadType.ReadHeavy => new CompressionConfig
            {
                Codec = CompressionCodec.LZ4,  // Fast decompression
                CompressionLevel = 3
            },
            WorkloadType.WriteHeavy => new CompressionConfig
            {
                Codec = CompressionCodec.LZ4,  // Fast compression
                CompressionLevel = 1
            },
            WorkloadType.StorageOptimized => new CompressionConfig
            {
                Codec = CompressionCodec.Zstd, // Best compression ratio
                CompressionLevel = 5
            },
            _ => new CompressionConfig
            {
                Codec = CompressionCodec.Zstd,
                CompressionLevel = 3  // Balanced
            }
        };
    }
}

public enum WorkloadType
{
    ReadHeavy,
    WriteHeavy,
    Balanced,
    StorageOptimized
}

public enum CompressionCodec
{
    None,
    LZ4,
    Zstd,
    Blosc
}
```

#### Cache Configuration

```csharp
public class CacheConfiguration
{
    // Primary storage chunk cache
    public long ChunkCacheSizeBytes { get; set; } = 2L * 1024 * 1024 * 1024; // 2GB

    // Query result cache
    public long QueryCacheSizeBytes { get; set; } = 100 * 1024 * 1024; // 100MB

    // Cache eviction policy
    public CacheEvictionPolicy EvictionPolicy { get; set; } = CacheEvictionPolicy.LRU;

    // Cache TTL
    public TimeSpan DefaultTTL { get; set; } = TimeSpan.FromMinutes(5);

    public static CacheConfiguration ForMemoryConstraint(long totalMemoryBytes)
    {
        // Allocate 30% of available memory to caching
        long cacheMemory = (long)(totalMemoryBytes * 0.3);

        return new CacheConfiguration
        {
            ChunkCacheSizeBytes = (long)(cacheMemory * 0.8),  // 80% for chunks
            QueryCacheSizeBytes = (long)(cacheMemory * 0.2),  // 20% for queries
            EvictionPolicy = CacheEvictionPolicy.LRU,
            DefaultTTL = totalMemoryBytes < 8L * 1024 * 1024 * 1024
                ? TimeSpan.FromMinutes(2)  // Shorter TTL for low memory
                : TimeSpan.FromMinutes(5)
        };
    }
}

public enum CacheEvictionPolicy
{
    LRU,  // Least Recently Used
    LFU,  // Least Frequently Used
    TTL   // Time To Live
}
```

### Delta Overlay Configuration

```csharp
public class DeltaConfiguration
{
    // Consolidation threshold
    public int ConsolidationThreshold { get; set; } = 1000;

    // Compaction strategy
    public DeltaCompactionStrategy Strategy { get; set; } = 
        DeltaCompactionStrategy.SpatialClustering;

    // Auto-consolidation interval
    public TimeSpan AutoConsolidationInterval { get; set; } = TimeSpan.FromMinutes(5);

    public static DeltaConfiguration ForProcessType(GeologicalProcessType processType)
    {
        return processType switch
        {
            GeologicalProcessType.Erosion => new DeltaConfiguration
            {
                ConsolidationThreshold = 5000,  // Many small updates
                Strategy = DeltaCompactionStrategy.SpatialClustering,
                AutoConsolidationInterval = TimeSpan.FromMinutes(10)
            },
            GeologicalProcessType.Tectonics => new DeltaConfiguration
            {
                ConsolidationThreshold = 1000,  // Fewer, larger updates
                Strategy = DeltaCompactionStrategy.LazyThreshold,
                AutoConsolidationInterval = TimeSpan.FromMinutes(5)
            },
            GeologicalProcessType.Sedimentation => new DeltaConfiguration
            {
                ConsolidationThreshold = 3000,
                Strategy = DeltaCompactionStrategy.TimeBasedBatching,
                AutoConsolidationInterval = TimeSpan.FromMinutes(7)
            },
            _ => new DeltaConfiguration()
        };
    }
}

public enum GeologicalProcessType
{
    Erosion,
    Tectonics,
    Sedimentation,
    Climate,
    Volcanic
}
```

---

## Performance Optimization

### Memory Optimization

#### Memory Budget Calculator

```csharp
public class MemoryBudgetCalculator
{
    public static MemoryBudget CalculateBudget(
        long totalSystemMemoryBytes,
        int expectedConcurrentUsers)
    {
        // Reserve memory for OS and other processes
        long availableMemory = (long)(totalSystemMemoryBytes * 0.7);

        // Calculate per-component allocation
        return new MemoryBudget
        {
            ChunkCache = (long)(availableMemory * 0.50),       // 50% for chunk cache
            QueryCache = (long)(availableMemory * 0.15),       // 15% for query cache
            DeltaOverlay = (long)(availableMemory * 0.10),     // 10% for deltas
            IndexStructures = (long)(availableMemory * 0.15),  // 15% for indices
            WorkingSet = (long)(availableMemory * 0.10)        // 10% for operations
        };
    }

    public static void PrintBudget(MemoryBudget budget)
    {
        Console.WriteLine("Memory Budget:");
        Console.WriteLine($"  Chunk Cache:     {budget.ChunkCache / (1024 * 1024):N0} MB");
        Console.WriteLine($"  Query Cache:     {budget.QueryCache / (1024 * 1024):N0} MB");
        Console.WriteLine($"  Delta Overlay:   {budget.DeltaOverlay / (1024 * 1024):N0} MB");
        Console.WriteLine($"  Index Structures:{budget.IndexStructures / (1024 * 1024):N0} MB");
        Console.WriteLine($"  Working Set:     {budget.WorkingSet / (1024 * 1024):N0} MB");
        Console.WriteLine($"  Total:           {budget.Total / (1024 * 1024):N0} MB");
    }
}

public class MemoryBudget
{
    public long ChunkCache { get; set; }
    public long QueryCache { get; set; }
    public long DeltaOverlay { get; set; }
    public long IndexStructures { get; set; }
    public long WorkingSet { get; set; }

    public long Total =>
        ChunkCache + QueryCache + DeltaOverlay + IndexStructures + WorkingSet;
}
```

#### Memory Pressure Handler

```csharp
using System;
using System.Threading;
using System.Threading.Tasks;

public class MemoryPressureHandler
{
    private readonly HybridStorageManager _storage;
    private readonly DeltaAwareStorageManager _deltaStorage;
    private readonly Timer _monitorTimer;
    private long _lastMemoryCheck;

    public MemoryPressureHandler(
        HybridStorageManager storage,
        DeltaAwareStorageManager deltaStorage)
    {
        _storage = storage;
        _deltaStorage = deltaStorage;

        // Monitor memory every 30 seconds
        _monitorTimer = new Timer(
            CheckMemoryPressure,
            null,
            TimeSpan.Zero,
            TimeSpan.FromSeconds(30));
    }

    private void CheckMemoryPressure(object state)
    {
        var currentMemory = GC.GetTotalMemory(false);
        var memoryInfo = GC.GetGCMemoryInfo();

        // Calculate memory pressure
        float pressure = (float)currentMemory / memoryInfo.TotalAvailableMemoryBytes;

        if (pressure > 0.8f) // 80% threshold
        {
            Console.WriteLine($"High memory pressure detected: {pressure:P}");
            _ = Task.Run(HandleHighPressureAsync);
        }
        else if (pressure > 0.6f) // 60% threshold
        {
            Console.WriteLine($"Moderate memory pressure: {pressure:P}");
            _ = Task.Run(HandleModeratePressureAsync);
        }

        _lastMemoryCheck = currentMemory;
    }

    private async Task HandleHighPressureAsync()
    {
        Console.WriteLine("Executing aggressive memory cleanup...");

        // 1. Force delta consolidation
        await _deltaStorage.FlushDeltasAsync();

        // 2. Clear query caches
        // _queryManager.ClearCache();

        // 3. Trigger garbage collection
        GC.Collect(2, GCCollectionMode.Aggressive, true, true);
        GC.WaitForPendingFinalizers();

        Console.WriteLine("Memory cleanup completed");
    }

    private async Task HandleModeratePressureAsync()
    {
        Console.WriteLine("Executing standard memory cleanup...");

        // Consolidate oldest deltas
        await _deltaStorage.FlushDeltasAsync();

        Console.WriteLine("Memory cleanup completed");
    }
}
```

### Query Optimization

#### Query Plan Optimizer

```csharp
public class QueryPlanOptimizer
{
    public QueryPlan OptimizeQuery(QueryRequest request)
    {
        var plan = new QueryPlan();

        // Analyze query characteristics
        var queryVolume = CalculateVolume(request.Bounds);
        var expectedResultSize = EstimateResultSize(request);

        // Choose execution strategy
        if (queryVolume < 1000) // Small region
        {
            plan.Strategy = QueryStrategy.DirectAccess;
            plan.UseCache = true;
            plan.Parallelism = 1;
        }
        else if (queryVolume < 1000000) // Medium region
        {
            plan.Strategy = QueryStrategy.IndexAccelerated;
            plan.UseCache = true;
            plan.Parallelism = 4;
        }
        else // Large region
        {
            plan.Strategy = QueryStrategy.StreamingAccess;
            plan.UseCache = false;
            plan.Parallelism = 8;
            plan.EnableResultStreaming = true;
        }

        // Optimize LOD based on region size
        if (request.TargetLOD == -1) // Auto LOD
        {
            plan.OptimizedLOD = CalculateOptimalLOD(queryVolume);
        }
        else
        {
            plan.OptimizedLOD = request.TargetLOD;
        }

        return plan;
    }

    private float CalculateVolume(BoundingBox bounds)
    {
        var size = bounds.Max - bounds.Min;
        return size.X * size.Y * size.Z;
    }

    private long EstimateResultSize(QueryRequest request)
    {
        var volume = CalculateVolume(request.Bounds);
        var voxelCount = volume / (request.Resolution * request.Resolution * request.Resolution);
        return (long)(voxelCount * 2); // 2 bytes per MaterialId
    }

    private int CalculateOptimalLOD(float volume)
    {
        // Larger regions use lower LOD for performance
        if (volume > 10000000) return 8;   // ~100km scale
        if (volume > 1000000) return 10;   // ~10km scale
        if (volume > 100000) return 12;    // ~1km scale
        if (volume > 10000) return 15;     // ~100m scale
        return 20;                          // Maximum detail
    }
}

public class QueryRequest
{
    public BoundingBox Bounds { get; set; }
    public int TargetLOD { get; set; } = -1; // -1 = auto
    public float Resolution { get; set; } = 1.0f;
}

public class QueryPlan
{
    public QueryStrategy Strategy { get; set; }
    public bool UseCache { get; set; }
    public int Parallelism { get; set; }
    public int OptimizedLOD { get; set; }
    public bool EnableResultStreaming { get; set; }
}

public enum QueryStrategy
{
    DirectAccess,
    IndexAccelerated,
    StreamingAccess
}
```

### Batch Processing Optimization

```csharp
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class BatchProcessor
{
    private readonly DeltaAwareStorageManager _storage;
    private readonly int _optimalBatchSize;

    public BatchProcessor(DeltaAwareStorageManager storage)
    {
        _storage = storage;
        _optimalBatchSize = CalculateOptimalBatchSize();
    }

    /// <summary>
    /// Process updates in optimized batches
    /// </summary>
    public async Task ProcessUpdatesAsync(
        IEnumerable<(Vector3 position, MaterialId material)> updates)
    {
        var updateList = updates.ToList();

        // Group by spatial proximity for better cache locality
        var spatialGroups = GroupBySpatialProximity(updateList);

        // Process each group in optimal batch sizes
        foreach (var group in spatialGroups)
        {
            var batches = group.Chunk(_optimalBatchSize);

            foreach (var batch in batches)
            {
                await _storage.BatchUpdateAsync(batch);
            }
        }
    }

    private List<List<(Vector3, MaterialId)>> GroupBySpatialProximity(
        List<(Vector3 position, MaterialId material)> updates)
    {
        // Group updates by chunk for spatial locality
        var chunkGroups = updates
            .GroupBy(u => CalculateChunkId(u.position))
            .Select(g => g.ToList())
            .ToList();

        return chunkGroups;
    }

    private ChunkId CalculateChunkId(Vector3 position)
    {
        const int chunkSize = 128;
        return new ChunkId
        {
            X = (int)Math.Floor(position.X / chunkSize),
            Y = (int)Math.Floor(position.Y / chunkSize),
            Z = (int)Math.Floor(position.Z / chunkSize)
        };
    }

    private int CalculateOptimalBatchSize()
    {
        // Based on available memory and typical update patterns
        var availableMemory = GC.GetGCMemoryInfo().TotalAvailableMemoryBytes;

        if (availableMemory > 16L * 1024 * 1024 * 1024) // > 16GB
            return 10000;
        else if (availableMemory > 8L * 1024 * 1024 * 1024) // > 8GB
            return 5000;
        else
            return 1000;
    }
}

// Extension method for chunking
public static class EnumerableExtensions
{
    public static IEnumerable<IEnumerable<T>> Chunk<T>(this IEnumerable<T> source, int chunkSize)
    {
        var list = source.ToList();
        for (int i = 0; i < list.Count; i += chunkSize)
        {
            yield return list.Skip(i).Take(chunkSize);
        }
    }
}
```

---

## Monitoring and Diagnostics

### Comprehensive Monitoring Dashboard

```csharp
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

public class MonitoringDashboard
{
    private readonly HybridStorageManager _storage;
    private readonly DeltaAwareStorageManager _deltaStorage;
    private readonly HybridQueryManager _queryManager;
    private readonly Timer _updateTimer;

    public MonitoringDashboard(
        HybridStorageManager storage,
        DeltaAwareStorageManager deltaStorage,
        HybridQueryManager queryManager)
    {
        _storage = storage;
        _deltaStorage = deltaStorage;
        _queryManager = queryManager;

        // Update dashboard every 5 seconds
        _updateTimer = new Timer(
            _ => DisplayDashboard(),
            null,
            TimeSpan.Zero,
            TimeSpan.FromSeconds(5));
    }

    public void DisplayDashboard()
    {
        Console.Clear();
        var sb = new StringBuilder();

        sb.AppendLine("═══════════════════════════════════════════════════════════");
        sb.AppendLine("     BlueMarble Hybrid Storage - Monitoring Dashboard");
        sb.AppendLine("═══════════════════════════════════════════════════════════");
        sb.AppendLine();

        // Storage statistics
        var storageStats = _storage.GetStatistics();
        sb.AppendLine("Storage System:");
        sb.AppendLine($"  Rebuild Queue Depth: {storageStats.RebuildQueueDepth}");
        sb.AppendLine();

        // Delta statistics
        var deltaStats = _deltaStorage.GetStatistics();
        sb.AppendLine("Delta Overlay:");
        sb.AppendLine($"  Active Deltas:       {deltaStats.DeltaCount:N0}");
        sb.AppendLine($"  Threshold:           {deltaStats.ConsolidationThreshold:N0}");
        sb.AppendLine($"  Utilization:         {deltaStats.DeltaUtilization:P}");
        sb.AppendLine();

        // Query statistics
        var queryStats = _queryManager.GetStatistics();
        sb.AppendLine("Query Performance:");
        sb.AppendLine($"  Total Queries:       {queryStats.PerformanceMetrics.TotalQueries:N0}");
        sb.AppendLine($"  Cache Hit Rate:      {queryStats.PerformanceMetrics.CacheHitRate:P}");
        sb.AppendLine($"  Avg Query Time:      {queryStats.PerformanceMetrics.AverageQueryTime.TotalMilliseconds:F2}ms");
        sb.AppendLine($"  Queries/Second:      {queryStats.PerformanceMetrics.QueriesPerSecond:F0}");
        sb.AppendLine();

        // Cache statistics
        sb.AppendLine("Cache Utilization:");
        sb.AppendLine($"  Entries:             {queryStats.CacheStatistics.EntryCount:N0}");
        sb.AppendLine($"  Size:                {queryStats.CacheStatistics.SizeBytes / (1024 * 1024):N0} MB");
        sb.AppendLine($"  Utilization:         {queryStats.CacheStatistics.Utilization:P}");
        sb.AppendLine();

        // Memory statistics
        var memInfo = GC.GetGCMemoryInfo();
        var usedMemory = GC.GetTotalMemory(false);
        sb.AppendLine("Memory Usage:");
        sb.AppendLine($"  Used:                {usedMemory / (1024 * 1024):N0} MB");
        sb.AppendLine($"  Total Available:     {memInfo.TotalAvailableMemoryBytes / (1024 * 1024):N0} MB");
        sb.AppendLine($"  Pressure:            {(float)usedMemory / memInfo.TotalAvailableMemoryBytes:P}");
        sb.AppendLine();

        // Optimization statistics
        sb.AppendLine("Optimizations:");
        sb.AppendLine($"  LOD Optimizations:   {queryStats.OptimizationMetrics.LODOptimizations:N0}");
        sb.AppendLine($"  Ray Optimizations:   {queryStats.OptimizationMetrics.RayTraceOptimizations:N0}");
        sb.AppendLine();

        sb.AppendLine("═══════════════════════════════════════════════════════════");
        sb.AppendLine($"Last Updated: {DateTime.UtcNow:yyyy-MM-dd HH:mm:ss} UTC");

        Console.WriteLine(sb.ToString());
    }

    public void Stop()
    {
        _updateTimer?.Dispose();
    }
}
```

### Performance Profiler

```csharp
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

public class PerformanceProfiler
{
    private readonly Dictionary<string, List<TimeSpan>> _timings;
    private readonly Dictionary<string, Stopwatch> _activeTimers;

    public PerformanceProfiler()
    {
        _timings = new Dictionary<string, List<TimeSpan>>();
        _activeTimers = new Dictionary<string, Stopwatch>();
    }

    public void StartTimer(string operationName)
    {
        if (!_activeTimers.ContainsKey(operationName))
        {
            _activeTimers[operationName] = new Stopwatch();
        }

        _activeTimers[operationName].Restart();
    }

    public void StopTimer(string operationName)
    {
        if (_activeTimers.TryGetValue(operationName, out var timer))
        {
            timer.Stop();

            if (!_timings.ContainsKey(operationName))
            {
                _timings[operationName] = new List<TimeSpan>();
            }

            _timings[operationName].Add(timer.Elapsed);

            // Keep only last 1000 measurements
            if (_timings[operationName].Count > 1000)
            {
                _timings[operationName].RemoveAt(0);
            }
        }
    }

    public ProfileReport GenerateReport()
    {
        var report = new ProfileReport();

        foreach (var (operation, timings) in _timings)
        {
            if (!timings.Any()) continue;

            var avg = TimeSpan.FromTicks((long)timings.Average(t => t.Ticks));
            var min = timings.Min();
            var max = timings.Max();

            report.Operations.Add(new OperationProfile
            {
                Name = operation,
                Count = timings.Count,
                Average = avg,
                Min = min,
                Max = max,
                Total = TimeSpan.FromTicks(timings.Sum(t => t.Ticks))
            });
        }

        return report;
    }

    public void PrintReport()
    {
        var report = GenerateReport();

        Console.WriteLine("\nPerformance Profile Report");
        Console.WriteLine("═════════════════════════════════════════════════════");
        Console.WriteLine($"{"Operation",-30} {"Count",8} {"Avg (ms)",10} {"Min (ms)",10} {"Max (ms)",10}");
        Console.WriteLine("─────────────────────────────────────────────────────");

        foreach (var op in report.Operations.OrderByDescending(o => o.Total))
        {
            Console.WriteLine(
                $"{op.Name,-30} {op.Count,8} {op.Average.TotalMilliseconds,10:F2} " +
                $"{op.Min.TotalMilliseconds,10:F2} {op.Max.TotalMilliseconds,10:F2}");
        }

        Console.WriteLine("═════════════════════════════════════════════════════\n");
    }
}

public class ProfileReport
{
    public List<OperationProfile> Operations { get; set; } = new();
}

public class OperationProfile
{
    public string Name { get; set; }
    public int Count { get; set; }
    public TimeSpan Average { get; set; }
    public TimeSpan Min { get; set; }
    public TimeSpan Max { get; set; }
    public TimeSpan Total { get; set; }
}
```

---

## Troubleshooting

### Common Issues and Solutions

#### Issue 1: High Memory Usage

**Symptoms:**
- System memory usage > 80%
- Frequent garbage collection
- Slow query performance

**Solutions:**

```csharp
public class MemoryTroubleshooting
{
    public static void DiagnoseMemoryIssue(HybridStorageManager storage)
    {
        var memInfo = GC.GetGCMemoryInfo();
        var usedMemory = GC.GetTotalMemory(false);
        var pressure = (float)usedMemory / memInfo.TotalAvailableMemoryBytes;

        Console.WriteLine($"Memory Pressure: {pressure:P}");

        if (pressure > 0.8f)
        {
            Console.WriteLine("CRITICAL: High memory pressure detected");
            Console.WriteLine("\nRecommended Actions:");
            Console.WriteLine("1. Reduce chunk cache size");
            Console.WriteLine("2. Increase delta consolidation frequency");
            Console.WriteLine("3. Reduce query cache TTL");
            Console.WriteLine("4. Enable aggressive GC mode");
        }
    }

    public static async Task ApplyMemoryFix(
        HybridStorageManager storage,
        DeltaAwareStorageManager deltaStorage)
    {
        // 1. Force delta consolidation
        await deltaStorage.FlushDeltasAsync();

        // 2. Trigger aggressive GC
        GC.Collect(2, GCCollectionMode.Aggressive, true, true);
        GC.WaitForPendingFinalizers();

        // 3. Clear caches
        // storage.ClearCaches();

        Console.WriteLine("Memory optimization completed");
    }
}
```

#### Issue 2: Slow Query Performance

**Symptoms:**
- Query latency > 100ms
- Low cache hit rate
- High CPU usage

**Solutions:**

```csharp
public class QueryTroubleshooting
{
    public static void DiagnoseQueryPerformance(HybridQueryManager queryManager)
    {
        var stats = queryManager.GetStatistics();

        Console.WriteLine("Query Performance Diagnostics:");
        Console.WriteLine($"Cache Hit Rate: {stats.PerformanceMetrics.CacheHitRate:P}");
        Console.WriteLine($"Avg Query Time: {stats.PerformanceMetrics.AverageQueryTime.TotalMilliseconds:F2}ms");

        if (stats.PerformanceMetrics.CacheHitRate < 0.5f)
        {
            Console.WriteLine("\nWARNING: Low cache hit rate");
            Console.WriteLine("Recommendations:");
            Console.WriteLine("- Increase query cache size");
            Console.WriteLine("- Increase cache TTL");
            Console.WriteLine("- Review query patterns for optimization");
        }

        if (stats.PerformanceMetrics.AverageQueryTime.TotalMilliseconds > 50)
        {
            Console.WriteLine("\nWARNING: High average query time");
            Console.WriteLine("Recommendations:");
            Console.WriteLine("- Use lower LOD for large regions");
            Console.WriteLine("- Enable query parallelization");
            Console.WriteLine("- Check index rebuild queue depth");
        }
    }
}
```

#### Issue 3: Delta Buildup

**Symptoms:**
- Delta count near threshold
- Frequent consolidations
- Increased memory usage

**Solutions:**

```csharp
public class DeltaTroubleshooting
{
    public static void DiagnoseDeltaIssue(DeltaAwareStorageManager storage)
    {
        var stats = storage.GetStatistics();

        Console.WriteLine("Delta Overlay Diagnostics:");
        Console.WriteLine($"Delta Count: {stats.DeltaCount}");
        Console.WriteLine($"Threshold: {stats.ConsolidationThreshold}");
        Console.WriteLine($"Utilization: {stats.DeltaUtilization:P}");

        if (stats.DeltaUtilization > 0.8f)
        {
            Console.WriteLine("\nWARNING: High delta utilization");
            Console.WriteLine("Recommendations:");
            Console.WriteLine("- Increase consolidation threshold");
            Console.WriteLine("- Use spatial clustering strategy");
            Console.WriteLine("- Force immediate consolidation");
        }
    }

    public static async Task ApplyDeltaFix(DeltaAwareStorageManager storage)
    {
        Console.WriteLine("Forcing delta consolidation...");
        await storage.FlushDeltasAsync();
        Console.WriteLine("Consolidation complete");

        var stats = storage.GetStatistics();
        Console.WriteLine($"New delta count: {stats.DeltaCount}");
    }
}
```

---

## Best Practices

### General Guidelines

1. **Choose appropriate chunk sizes**
   - Urban areas: 64³ cells
   - Natural terrain: 128³ cells
   - Large-scale features: 256³ cells

2. **Configure caching based on workload**
   - Read-heavy: Large query cache, LZ4 compression
   - Write-heavy: Small query cache, fast compression
   - Balanced: Default settings

3. **Monitor and tune regularly**
   - Check memory pressure every 30 seconds
   - Review query performance daily
   - Optimize delta consolidation weekly

4. **Use appropriate LOD for each process**
   - Tectonics: LOD 8 (100km scale)
   - Erosion: LOD 20 (1m scale)
   - Climate: LOD 5 (1000km scale)

5. **Batch updates when possible**
   - 100x better performance than individual updates
   - Group by spatial proximity
   - Use optimal batch sizes (1000-10000)

### Production Deployment Checklist

```markdown
## Pre-Deployment Checklist

Storage Configuration:
- [ ] Chunk size appropriate for workload
- [ ] Compression codec selected
- [ ] Cache sizes configured based on available memory
- [ ] Delta consolidation threshold set

Performance Tuning:
- [ ] Memory budget calculated
- [ ] Query optimizer configured
- [ ] Batch processor enabled
- [ ] Parallelization settings optimized

Monitoring:
- [ ] Monitoring dashboard enabled
- [ ] Performance profiler configured
- [ ] Memory pressure handler active
- [ ] Alert thresholds configured

Testing:
- [ ] Load tested with realistic data
- [ ] Query performance validated
- [ ] Memory usage profiled
- [ ] Failover scenarios tested

Documentation:
- [ ] Configuration documented
- [ ] Monitoring procedures documented
- [ ] Troubleshooting guide available
- [ ] Team trained on operations
```

---

## Advanced Topics

### Distributed Deployment

```csharp
public class DistributedStorageConfiguration
{
    public List<StorageNode> Nodes { get; set; } = new();
    public ShardingStrategy Strategy { get; set; } = ShardingStrategy.SpatialHash;
    public int ReplicationFactor { get; set; } = 3;

    public static DistributedStorageConfiguration ForCluster(int nodeCount)
    {
        var config = new DistributedStorageConfiguration
        {
            Strategy = ShardingStrategy.SpatialHash,
            ReplicationFactor = Math.Min(3, nodeCount)
        };

        for (int i = 0; i < nodeCount; i++)
        {
            config.Nodes.Add(new StorageNode
            {
                Id = i,
                Endpoint = $"node-{i}.bluemarble.local:5000",
                Capacity = 1024L * 1024 * 1024 * 1024 // 1TB
            });
        }

        return config;
    }
}

public class StorageNode
{
    public int Id { get; set; }
    public string Endpoint { get; set; }
    public long Capacity { get; set; }
}

public enum ShardingStrategy
{
    SpatialHash,
    Geographic,
    RoundRobin
}
```

### Custom Process Integration

```csharp
public class CustomProcessAdapter : IGeologicalProcessAdapter
{
    public string ProcessName => "CustomProcess";

    public async Task<ProcessResult> ExecuteAsync(
        ProcessParameters parameters,
        CancellationToken cancellationToken = default)
    {
        // Implement custom geological process
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<Vector3>> GetAffectedRegionsAsync(
        ProcessParameters parameters,
        CancellationToken cancellationToken = default)
    {
        // Return positions affected by process
        throw new NotImplementedException();
    }
}
```

---

## Summary

This guide provides comprehensive coverage of:

- **Setup and Configuration**: Quick start and detailed configuration options
- **Performance Optimization**: Memory management, query optimization, batch processing
- **Monitoring**: Real-time dashboards and performance profiling
- **Troubleshooting**: Common issues and solutions
- **Best Practices**: Production deployment guidelines
- **Advanced Topics**: Distributed deployment and custom integration

For additional support, refer to:
- Core implementation: `hybrid-storage-core-implementation.md`
- Delta overlay: `hybrid-storage-delta-overlay.md`
- Query integration: `hybrid-storage-query-integration.md`
