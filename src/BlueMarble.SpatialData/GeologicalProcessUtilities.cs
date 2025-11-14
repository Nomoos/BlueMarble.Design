using System.Diagnostics;

namespace BlueMarble.SpatialData;

/// <summary>
/// Utility class for cross-cutting concerns - logging and monitoring
/// Implements composition pattern for reusable functionality
/// 
/// This demonstrates the principle of using helper classes for orthogonal concerns
/// rather than building them into the inheritance hierarchy
/// </summary>
public class GeologicalProcessLogger
{
    private readonly string _processName;
    private Stopwatch? _stopwatch;
    private int _operationCount;

    public GeologicalProcessLogger(string processName)
    {
        _processName = processName;
    }

    public void StartOperation()
    {
        _stopwatch = Stopwatch.StartNew();
        _operationCount = 0;
    }

    public void LogPositionProcessed()
    {
        _operationCount++;
    }

    public void EndOperation()
    {
        _stopwatch?.Stop();
    }

    public ProcessMetrics GetMetrics()
    {
        return new ProcessMetrics
        {
            ProcessName = _processName,
            ElapsedMilliseconds = _stopwatch?.ElapsedMilliseconds ?? 0,
            OperationCount = _operationCount,
            OperationsPerSecond = CalculateOpsPerSecond()
        };
    }

    private double CalculateOpsPerSecond()
    {
        if (_stopwatch == null || _stopwatch.ElapsedMilliseconds == 0)
            return 0;

        return (_operationCount * 1000.0) / _stopwatch.ElapsedMilliseconds;
    }
}

/// <summary>
/// Cache manager for geological process results
/// Another example of composition for cross-cutting concerns
/// </summary>
public class GeologicalProcessCache
{
    private readonly Dictionary<string, CacheEntry> _cache = new();
    private readonly int _maxEntries;
    private readonly TimeSpan _expirationTime;

    public GeologicalProcessCache(int maxEntries = 1000, TimeSpan? expirationTime = null)
    {
        _maxEntries = maxEntries;
        _expirationTime = expirationTime ?? TimeSpan.FromMinutes(5);
    }

    public bool TryGet<T>(string key, out T? value)
    {
        if (_cache.TryGetValue(key, out var entry) && !entry.IsExpired(_expirationTime))
        {
            value = (T?)entry.Value;
            return true;
        }

        value = default;
        return false;
    }

    public void Set(string key, object value)
    {
        // Evict oldest entries if cache is full
        if (_cache.Count >= _maxEntries)
        {
            var oldestKey = _cache.OrderBy(e => e.Value.Timestamp).First().Key;
            _cache.Remove(oldestKey);
        }

        _cache[key] = new CacheEntry(value);
    }

    public void Clear()
    {
        _cache.Clear();
    }

    private class CacheEntry
    {
        public object Value { get; }
        public DateTime Timestamp { get; }

        public CacheEntry(object value)
        {
            Value = value;
            Timestamp = DateTime.UtcNow;
        }

        public bool IsExpired(TimeSpan expirationTime)
        {
            return DateTime.UtcNow - Timestamp > expirationTime;
        }
    }
}

/// <summary>
/// Metrics data for process monitoring
/// </summary>
public class ProcessMetrics
{
    public string ProcessName { get; set; } = string.Empty;
    public long ElapsedMilliseconds { get; set; }
    public int OperationCount { get; set; }
    public double OperationsPerSecond { get; set; }

    public override string ToString()
    {
        return $"{ProcessName}: {OperationCount} ops in {ElapsedMilliseconds}ms ({OperationsPerSecond:F2} ops/sec)";
    }
}
