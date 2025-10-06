namespace BlueMarble.SpatialIndexing;

/// <summary>
/// Interface for pluggable persistence providers (file-based, SQL, NoSQL, etc.)
/// Enables transactional operations and ACID-like properties
/// </summary>
/// <typeparam name="T">The type of data stored in spatial chunks</typeparam>
public interface ISpatialPersistence<T>
{
    /// <summary>
    /// Loads a chunk from persistent storage
    /// </summary>
    /// <param name="chunkX">X coordinate of the chunk</param>
    /// <param name="chunkY">Y coordinate of the chunk</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The loaded chunk, or null if not found</returns>
    Task<SpatialChunk<T>?> LoadChunkAsync(int chunkX, int chunkY, CancellationToken cancellationToken = default);

    /// <summary>
    /// Saves a chunk to persistent storage with atomic operations
    /// </summary>
    /// <param name="chunk">The chunk to save</param>
    /// <param name="cancellationToken">Cancellation token</param>
    Task SaveChunkAsync(SpatialChunk<T> chunk, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a chunk from persistent storage
    /// </summary>
    /// <param name="chunkX">X coordinate of the chunk</param>
    /// <param name="chunkY">Y coordinate of the chunk</param>
    /// <param name="cancellationToken">Cancellation token</param>
    Task DeleteChunkAsync(int chunkX, int chunkY, CancellationToken cancellationToken = default);

    /// <summary>
    /// Checks if a chunk exists in persistent storage
    /// </summary>
    /// <param name="chunkX">X coordinate of the chunk</param>
    /// <param name="chunkY">Y coordinate of the chunk</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if the chunk exists, false otherwise</returns>
    Task<bool> ChunkExistsAsync(int chunkX, int chunkY, CancellationToken cancellationToken = default);
}
