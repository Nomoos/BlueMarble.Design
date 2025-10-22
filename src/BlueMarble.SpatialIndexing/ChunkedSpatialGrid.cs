using System.Collections.Concurrent;

namespace BlueMarble.SpatialIndexing;

/// <summary>
/// Configuration options for the chunked spatial grid
/// </summary>
public class ChunkedSpatialGridConfig
{
    /// <summary>
    /// Size of each chunk (default 100x100)
    /// </summary>
    public int ChunkSize { get; set; } = 100;

    /// <summary>
    /// Maximum number of chunks to keep in memory (LRU eviction)
    /// </summary>
    public int MaxCachedChunks { get; set; } = 1000;

    /// <summary>
    /// Whether to enable lazy loading from persistence
    /// </summary>
    public bool EnableLazyLoading { get; set; } = true;

    /// <summary>
    /// Whether to automatically save dirty chunks on eviction
    /// </summary>
    public bool AutoSaveOnEviction { get; set; } = true;

    /// <summary>
    /// Interval for automatic dirty chunk flushing (null = disabled)
    /// </summary>
    public TimeSpan? AutoFlushInterval { get; set; } = TimeSpan.FromSeconds(30);
}

/// <summary>
/// Main grid manager with lazy loading, LRU eviction, and persistence support
/// Optimized for dense worlds with billions of cells
/// </summary>
/// <typeparam name="T">The type of data stored in the grid</typeparam>
public class ChunkedSpatialGrid<T>
{
    private readonly ConcurrentDictionary<(int x, int y), SpatialChunk<T>> _chunks;
    private readonly LinkedList<(int x, int y)> _lruList;
    private readonly object _lruLock = new();
    private readonly ChunkedSpatialGridConfig _config;
    private readonly ISpatialPersistence<T>? _persistence;
    private readonly Timer? _autoFlushTimer;

    /// <summary>
    /// Default value for uninitialized cells
    /// </summary>
    public T DefaultValue { get; }

    /// <summary>
    /// Gets the chunk size
    /// </summary>
    public int ChunkSize => _config.ChunkSize;

    /// <summary>
    /// Gets the current number of cached chunks
    /// </summary>
    public int CachedChunkCount => _chunks.Count;

    /// <summary>
    /// Creates a new chunked spatial grid
    /// </summary>
    /// <param name="defaultValue">Default value for uninitialized cells</param>
    /// <param name="config">Configuration options</param>
    /// <param name="persistence">Optional persistence provider</param>
    public ChunkedSpatialGrid(T defaultValue, ChunkedSpatialGridConfig? config = null, ISpatialPersistence<T>? persistence = null)
    {
        DefaultValue = defaultValue;
        _config = config ?? new ChunkedSpatialGridConfig();
        _persistence = persistence;
        _chunks = new ConcurrentDictionary<(int x, int y), SpatialChunk<T>>();
        _lruList = new LinkedList<(int x, int y)>();

        if (_config.AutoFlushInterval.HasValue)
        {
            _autoFlushTimer = new Timer(AutoFlushCallback, null, _config.AutoFlushInterval.Value, _config.AutoFlushInterval.Value);
        }
    }

    /// <summary>
    /// Gets the value at the specified world coordinates
    /// O(1) operation with potential chunk loading
    /// </summary>
    /// <param name="worldX">World X coordinate</param>
    /// <param name="worldY">World Y coordinate</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The value at the specified coordinates</returns>
    public async Task<T> GetAsync(int worldX, int worldY, CancellationToken cancellationToken = default)
    {
        var (chunkX, chunkY, localX, localY) = WorldToChunkCoords(worldX, worldY);
        var chunk = await GetOrLoadChunkAsync(chunkX, chunkY, cancellationToken);
        return chunk.Get(localX, localY);
    }

    /// <summary>
    /// Sets the value at the specified world coordinates
    /// O(1) operation with potential chunk loading
    /// </summary>
    /// <param name="worldX">World X coordinate</param>
    /// <param name="worldY">World Y coordinate</param>
    /// <param name="value">The value to set</param>
    /// <param name="cancellationToken">Cancellation token</param>
    public async Task SetAsync(int worldX, int worldY, T value, CancellationToken cancellationToken = default)
    {
        var (chunkX, chunkY, localX, localY) = WorldToChunkCoords(worldX, worldY);
        var chunk = await GetOrLoadChunkAsync(chunkX, chunkY, cancellationToken);
        chunk.Set(localX, localY, value);
    }

    /// <summary>
    /// Gets multiple values in a rectangular region (bulk operation)
    /// More efficient than individual calls
    /// </summary>
    /// <param name="worldX">Start X coordinate</param>
    /// <param name="worldY">Start Y coordinate</param>
    /// <param name="width">Width of the region</param>
    /// <param name="height">Height of the region</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>2D array of values</returns>
    public async Task<T[,]> GetRegionAsync(int worldX, int worldY, int width, int height, CancellationToken cancellationToken = default)
    {
        var result = new T[height, width];

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                result[y, x] = await GetAsync(worldX + x, worldY + y, cancellationToken);
            }
        }

        return result;
    }

    /// <summary>
    /// Sets multiple values in a rectangular region (bulk operation)
    /// </summary>
    public async Task SetRegionAsync(int worldX, int worldY, T[,] values, CancellationToken cancellationToken = default)
    {
        int height = values.GetLength(0);
        int width = values.GetLength(1);

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                await SetAsync(worldX + x, worldY + y, values[y, x], cancellationToken);
            }
        }
    }

    /// <summary>
    /// Flushes all dirty chunks to persistent storage
    /// </summary>
    public async Task FlushAsync(CancellationToken cancellationToken = default)
    {
        if (_persistence == null) return;

        var dirtyChunks = _chunks.Values.Where(c => c.IsDirty).ToList();

        foreach (var chunk in dirtyChunks)
        {
            await _persistence.SaveChunkAsync(chunk, cancellationToken);
            chunk.IsDirty = false;
        }
    }

    /// <summary>
    /// Gets or loads a chunk, handling LRU eviction
    /// </summary>
    private async Task<SpatialChunk<T>> GetOrLoadChunkAsync(int chunkX, int chunkY, CancellationToken cancellationToken)
    {
        var key = (chunkX, chunkY);

        // Fast path: chunk already in cache
        if (_chunks.TryGetValue(key, out var chunk))
        {
            UpdateLRU(key);
            return chunk;
        }

        // Slow path: load from persistence or create new
        chunk = await LoadChunkAsync(chunkX, chunkY, cancellationToken);

        // Add to cache
        _chunks[key] = chunk;
        UpdateLRU(key);

        // Check if we need to evict
        await EvictIfNeededAsync(cancellationToken);

        return chunk;
    }

    /// <summary>
    /// Loads a chunk from persistence or creates a new one
    /// </summary>
    private async Task<SpatialChunk<T>> LoadChunkAsync(int chunkX, int chunkY, CancellationToken cancellationToken)
    {
        if (_persistence != null && _config.EnableLazyLoading)
        {
            var loaded = await _persistence.LoadChunkAsync(chunkX, chunkY, cancellationToken);
            if (loaded != null)
            {
                return loaded;
            }
        }

        // Create new chunk with default values
        var chunk = new SpatialChunk<T>(chunkX, chunkY, _config.ChunkSize);
        chunk.Fill(DefaultValue);
        return chunk;
    }

    /// <summary>
    /// Updates LRU list for a chunk
    /// </summary>
    private void UpdateLRU((int x, int y) key)
    {
        lock (_lruLock)
        {
            _lruList.Remove(key);
            _lruList.AddFirst(key);
        }
    }

    /// <summary>
    /// Evicts least recently used chunks if cache is full
    /// </summary>
    private async Task EvictIfNeededAsync(CancellationToken cancellationToken)
    {
        while (_chunks.Count > _config.MaxCachedChunks)
        {
            (int x, int y) toEvict;

            lock (_lruLock)
            {
                if (_lruList.Last == null) break;
                toEvict = _lruList.Last.Value;
                _lruList.RemoveLast();
            }

            if (_chunks.TryRemove(toEvict, out var chunk))
            {
                if (_config.AutoSaveOnEviction && chunk.IsDirty && _persistence != null)
                {
                    await _persistence.SaveChunkAsync(chunk, cancellationToken);
                }
            }
        }
    }

    /// <summary>
    /// Auto-flush timer callback
    /// </summary>
    private async void AutoFlushCallback(object? state)
    {
        try
        {
            await FlushAsync();
        }
        catch
        {
            // Suppress exceptions in timer callback
        }
    }

    /// <summary>
    /// Converts world coordinates to chunk coordinates and local coordinates
    /// </summary>
    private (int chunkX, int chunkY, int localX, int localY) WorldToChunkCoords(int worldX, int worldY)
    {
        int chunkX = worldX >= 0 ? worldX / _config.ChunkSize : (worldX - _config.ChunkSize + 1) / _config.ChunkSize;
        int chunkY = worldY >= 0 ? worldY / _config.ChunkSize : (worldY - _config.ChunkSize + 1) / _config.ChunkSize;

        int localX = worldX - chunkX * _config.ChunkSize;
        int localY = worldY - chunkY * _config.ChunkSize;

        return (chunkX, chunkY, localX, localY);
    }

    /// <summary>
    /// Gets total memory usage estimate in bytes
    /// </summary>
    public long GetMemoryUsage()
    {
        return _chunks.Values.Sum(c => c.GetMemoryUsage());
    }

    /// <summary>
    /// Disposes resources
    /// </summary>
    public async ValueTask DisposeAsync()
    {
        if (_autoFlushTimer != null)
        {
            await _autoFlushTimer.DisposeAsync();
        }

        await FlushAsync();
    }
}
