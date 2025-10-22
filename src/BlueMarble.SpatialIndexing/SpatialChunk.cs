namespace BlueMarble.SpatialIndexing;

/// <summary>
/// Individual spatial chunk with flat array storage providing O(1) coordinate lookup
/// Optimized for dense worlds with high-resolution data
/// </summary>
/// <typeparam name="T">The type of data stored in the chunk</typeparam>
public class SpatialChunk<T>
{
    private readonly T[] _data;
    private readonly int _chunkSize;

    /// <summary>
    /// X coordinate of this chunk in chunk space
    /// </summary>
    public int ChunkX { get; }

    /// <summary>
    /// Y coordinate of this chunk in chunk space
    /// </summary>
    public int ChunkY { get; }

    /// <summary>
    /// Size of the chunk (width and height)
    /// </summary>
    public int ChunkSize => _chunkSize;

    /// <summary>
    /// Timestamp of last modification
    /// </summary>
    public DateTime LastModified { get; set; }

    /// <summary>
    /// Timestamp of last access
    /// </summary>
    public DateTime LastAccessed { get; set; }

    /// <summary>
    /// Whether the chunk has been modified since last save
    /// </summary>
    public bool IsDirty { get; set; }

    /// <summary>
    /// Creates a new spatial chunk
    /// </summary>
    /// <param name="chunkX">X coordinate in chunk space</param>
    /// <param name="chunkY">Y coordinate in chunk space</param>
    /// <param name="chunkSize">Size of the chunk (typically 100-1000)</param>
    public SpatialChunk(int chunkX, int chunkY, int chunkSize = 100)
    {
        ChunkX = chunkX;
        ChunkY = chunkY;
        _chunkSize = chunkSize;
        _data = new T[chunkSize * chunkSize];
        LastModified = DateTime.UtcNow;
        LastAccessed = DateTime.UtcNow;
        IsDirty = false;
    }

    /// <summary>
    /// Gets the value at the specified local coordinates within the chunk
    /// O(1) lookup
    /// </summary>
    /// <param name="localX">Local X coordinate (0 to ChunkSize-1)</param>
    /// <param name="localY">Local Y coordinate (0 to ChunkSize-1)</param>
    /// <returns>The value at the specified coordinates</returns>
    public T Get(int localX, int localY)
    {
        ValidateLocalCoordinates(localX, localY);
        LastAccessed = DateTime.UtcNow;
        return _data[localY * _chunkSize + localX];
    }

    /// <summary>
    /// Sets the value at the specified local coordinates within the chunk
    /// O(1) operation
    /// </summary>
    /// <param name="localX">Local X coordinate (0 to ChunkSize-1)</param>
    /// <param name="localY">Local Y coordinate (0 to ChunkSize-1)</param>
    /// <param name="value">The value to set</param>
    public void Set(int localX, int localY, T value)
    {
        ValidateLocalCoordinates(localX, localY);
        _data[localY * _chunkSize + localX] = value;
        LastModified = DateTime.UtcNow;
        LastAccessed = DateTime.UtcNow;
        IsDirty = true;
    }

    /// <summary>
    /// Gets the raw data array (for bulk operations and serialization)
    /// </summary>
    public T[] GetRawData() => _data;

    /// <summary>
    /// Sets data from a raw array (for deserialization)
    /// </summary>
    /// <param name="data">The data array to copy from</param>
    public void SetRawData(T[] data)
    {
        if (data.Length != _data.Length)
        {
            throw new ArgumentException($"Data array must have length {_data.Length}", nameof(data));
        }

        Array.Copy(data, _data, _data.Length);
        LastModified = DateTime.UtcNow;
        LastAccessed = DateTime.UtcNow;
        IsDirty = true;
    }

    /// <summary>
    /// Fills the entire chunk with a value
    /// </summary>
    /// <param name="value">The value to fill with</param>
    public void Fill(T value)
    {
        Array.Fill(_data, value);
        LastModified = DateTime.UtcNow;
        LastAccessed = DateTime.UtcNow;
        IsDirty = true;
    }

    private void ValidateLocalCoordinates(int localX, int localY)
    {
        if (localX < 0 || localX >= _chunkSize)
        {
            throw new ArgumentOutOfRangeException(nameof(localX), $"Local X must be in range [0, {_chunkSize})");
        }

        if (localY < 0 || localY >= _chunkSize)
        {
            throw new ArgumentOutOfRangeException(nameof(localY), $"Local Y must be in range [0, {_chunkSize})");
        }
    }

    /// <summary>
    /// Gets memory usage estimate in bytes
    /// </summary>
    public long GetMemoryUsage()
    {
        // Base object overhead + array overhead + data
        long baseSize = 64; // Approximate object overhead
        long arraySize = 24 + (_data.Length * GetElementSize());
        return baseSize + arraySize;
    }

    private static int GetElementSize()
    {
        var type = typeof(T);
        if (type == typeof(byte)) return 1;
        if (type == typeof(short) || type == typeof(ushort)) return 2;
        if (type == typeof(int) || type == typeof(uint) || type == typeof(float)) return 4;
        if (type == typeof(long) || type == typeof(ulong) || type == typeof(double)) return 8;
        return 8; // Default estimate for reference types
    }
}
