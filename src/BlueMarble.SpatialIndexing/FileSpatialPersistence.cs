using System.Text.Json;

namespace BlueMarble.SpatialIndexing;

/// <summary>
/// File-based spatial persistence with atomic operations for ACID-like properties
/// Stores each chunk as a separate file with atomic write operations
/// </summary>
/// <typeparam name="T">The type of data stored in chunks</typeparam>
public class FileSpatialPersistence<T> : ISpatialPersistence<T>
{
    private readonly string _basePath;
    private readonly JsonSerializerOptions _jsonOptions;
    private readonly SemaphoreSlim _fileLock;

    /// <summary>
    /// Creates a new file-based persistence provider
    /// </summary>
    /// <param name="basePath">Base directory for storing chunk files</param>
    public FileSpatialPersistence(string basePath)
    {
        _basePath = basePath;
        _jsonOptions = new JsonSerializerOptions
        {
            WriteIndented = false,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
        _fileLock = new SemaphoreSlim(1, 1);

        // Ensure base directory exists
        Directory.CreateDirectory(_basePath);
    }

    /// <inheritdoc/>
    public async Task<SpatialChunk<T>?> LoadChunkAsync(int chunkX, int chunkY, CancellationToken cancellationToken = default)
    {
        var filePath = GetChunkFilePath(chunkX, chunkY);

        if (!File.Exists(filePath))
        {
            return null;
        }

        await _fileLock.WaitAsync(cancellationToken);
        try
        {
            var json = await File.ReadAllTextAsync(filePath, cancellationToken);
            var data = JsonSerializer.Deserialize<ChunkFileData>(json, _jsonOptions);

            if (data == null || data.Data == null)
            {
                return null;
            }

            var chunk = new SpatialChunk<T>(chunkX, chunkY, data.ChunkSize);
            chunk.SetRawData(data.Data);
            chunk.IsDirty = false;

            return chunk;
        }
        finally
        {
            _fileLock.Release();
        }
    }

    /// <inheritdoc/>
    public async Task SaveChunkAsync(SpatialChunk<T> chunk, CancellationToken cancellationToken = default)
    {
        var filePath = GetChunkFilePath(chunk.ChunkX, chunk.ChunkY);
        var tempPath = filePath + ".tmp";

        var data = new ChunkFileData
        {
            ChunkX = chunk.ChunkX,
            ChunkY = chunk.ChunkY,
            ChunkSize = chunk.ChunkSize,
            Data = chunk.GetRawData(),
            LastModified = chunk.LastModified
        };

        await _fileLock.WaitAsync(cancellationToken);
        try
        {
            // Ensure directory exists
            var directory = Path.GetDirectoryName(filePath);
            if (directory != null)
            {
                Directory.CreateDirectory(directory);
            }

            // Write to temporary file first (atomic operation)
            var json = JsonSerializer.Serialize(data, _jsonOptions);
            await File.WriteAllTextAsync(tempPath, json, cancellationToken);

            // Atomic move (replaces existing file)
            File.Move(tempPath, filePath, overwrite: true);
        }
        finally
        {
            _fileLock.Release();

            // Clean up temp file if it still exists
            if (File.Exists(tempPath))
            {
                try
                {
                    File.Delete(tempPath);
                }
                catch
                {
                    // Ignore cleanup errors
                }
            }
        }
    }

    /// <inheritdoc/>
    public async Task DeleteChunkAsync(int chunkX, int chunkY, CancellationToken cancellationToken = default)
    {
        var filePath = GetChunkFilePath(chunkX, chunkY);

        if (!File.Exists(filePath))
        {
            return;
        }

        await _fileLock.WaitAsync(cancellationToken);
        try
        {
            File.Delete(filePath);
        }
        finally
        {
            _fileLock.Release();
        }
    }

    /// <inheritdoc/>
    public Task<bool> ChunkExistsAsync(int chunkX, int chunkY, CancellationToken cancellationToken = default)
    {
        var filePath = GetChunkFilePath(chunkX, chunkY);
        return Task.FromResult(File.Exists(filePath));
    }

    private string GetChunkFilePath(int chunkX, int chunkY)
    {
        // Organize chunks into subdirectories for better file system performance
        // Each directory contains 1000x1000 chunks (100,000 chunks per directory)
        int dirX = chunkX / 1000;
        int dirY = chunkY / 1000;

        return Path.Combine(_basePath, $"x{dirX}", $"y{dirY}", $"chunk_{chunkX}_{chunkY}.json");
    }

    private class ChunkFileData
    {
        public int ChunkX { get; set; }
        public int ChunkY { get; set; }
        public int ChunkSize { get; set; }
        public T[] Data { get; set; } = Array.Empty<T>();
        public DateTime LastModified { get; set; }
    }
}
