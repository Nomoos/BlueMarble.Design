using System;
using System.Collections.Generic;

namespace BlueMarble.Utils.Spatial.DistributedStorage
{
    /// <summary>
    /// Generates Morton codes and consistent hashes for spatial-aware node distribution.
    /// Implements Z-order curves for spatial locality preservation.
    /// </summary>
    public class SpatialHashGenerator
    {
        private readonly SpatialBounds _worldBounds;
        private readonly int _replicationFactor;

        public SpatialHashGenerator(SpatialBounds worldBounds, int replicationFactor = 3)
        {
            _worldBounds = worldBounds ?? throw new ArgumentNullException(nameof(worldBounds));
            _replicationFactor = replicationFactor;
        }

        /// <summary>
        /// Generate hierarchical spatial hash keys for a 3D position.
        /// Returns keys for each octree level from root to specified max level.
        /// </summary>
        public List<SpatialHashKey> GenerateHierarchicalKeys(double x, double y, double z, int maxLevel)
        {
            var keys = new List<SpatialHashKey>();

            for (int level = 0; level <= maxLevel; level++)
            {
                var mortonCode = EncodeMorton3D(x, y, z, level);
                var regionKey = GenerateRegionKey(mortonCode, level);
                var nodeHashes = GetNodeHashes(regionKey, _replicationFactor);

                keys.Add(new SpatialHashKey
                {
                    Level = level,
                    MortonCode = mortonCode,
                    RegionKey = regionKey,
                    PrimaryNodeHash = nodeHashes.Primary,
                    ReplicaNodeHashes = nodeHashes.Replicas,
                    CacheKey = $"octree:{level:D2}:{mortonCode:X16}",
                    SpatialRegion = CalculateSpatialBounds(mortonCode, level)
                });
            }

            return keys;
        }

        /// <summary>
        /// Encode 3D position to Morton code (Z-order curve) with level-appropriate precision.
        /// </summary>
        public ulong EncodeMorton3D(double x, double y, double z, int level)
        {
            // Normalize coordinates to octree level resolution
            var resolution = Math.Pow(2, level);
            var size = _worldBounds.GetSize();

            var normalizedX = (uint)Math.Floor((x - _worldBounds.MinX) / size.X * resolution);
            var normalizedY = (uint)Math.Floor((y - _worldBounds.MinY) / size.Y * resolution);
            var normalizedZ = (uint)Math.Floor((z - _worldBounds.MinZ) / size.Z * resolution);

            // Clamp to valid range
            var maxCoord = (uint)resolution - 1;
            normalizedX = Math.Min(normalizedX, maxCoord);
            normalizedY = Math.Min(normalizedY, maxCoord);
            normalizedZ = Math.Min(normalizedZ, maxCoord);

            // Interleave coordinates to create Morton code
            return InterleaveCoordinates(normalizedX, normalizedY, normalizedZ);
        }

        /// <summary>
        /// Decode Morton code back to normalized 3D coordinates at a given level.
        /// </summary>
        public (uint X, uint Y, uint Z) DecodeMorton3D(ulong mortonCode)
        {
            return DeinterleaveCoordinates(mortonCode);
        }

        /// <summary>
        /// Interleave three 21-bit coordinates into a 63-bit Morton code.
        /// </summary>
        private ulong InterleaveCoordinates(uint x, uint y, uint z)
        {
            ulong result = 0;
            for (int i = 0; i < 21; i++)
            {
                result |= ((ulong)(x & (1u << i)) << (2 * i));
                result |= ((ulong)(y & (1u << i)) << (2 * i + 1));
                result |= ((ulong)(z & (1u << i)) << (2 * i + 2));
            }
            return result;
        }

        /// <summary>
        /// Deinterleave a 63-bit Morton code into three 21-bit coordinates.
        /// </summary>
        private (uint X, uint Y, uint Z) DeinterleaveCoordinates(ulong mortonCode)
        {
            uint x = 0, y = 0, z = 0;
            for (int i = 0; i < 21; i++)
            {
                x |= (uint)((mortonCode & (1ul << (3 * i))) >> (2 * i));
                y |= (uint)((mortonCode & (1ul << (3 * i + 1))) >> (2 * i + 1));
                z |= (uint)((mortonCode & (1ul << (3 * i + 2))) >> (2 * i + 2));
            }
            return (x, y, z);
        }

        /// <summary>
        /// Generate region key for consistent hash distribution.
        /// Balances spatial locality with load distribution.
        /// </summary>
        private string GenerateRegionKey(ulong mortonCode, int level)
        {
            return $"{level:D2}:{mortonCode:X16}";
        }

        /// <summary>
        /// Get primary and replica node hashes using consistent hashing (jump hash).
        /// </summary>
        private (ulong Primary, List<ulong> Replicas) GetNodeHashes(string regionKey, int replicationFactor)
        {
            var primaryHash = ComputeHash(regionKey);
            var replicas = new List<ulong>();

            for (int i = 1; i < replicationFactor; i++)
            {
                var replicaKey = $"{regionKey}:replica{i}";
                replicas.Add(ComputeHash(replicaKey));
            }

            return (primaryHash, replicas);
        }

        /// <summary>
        /// Compute hash using FNV-1a algorithm for consistent distribution.
        /// </summary>
        private ulong ComputeHash(string key)
        {
            const ulong FNV_OFFSET_BASIS = 14695981039346656037;
            const ulong FNV_PRIME = 1099511628211;

            ulong hash = FNV_OFFSET_BASIS;
            foreach (char c in key)
            {
                hash ^= c;
                hash *= FNV_PRIME;
            }
            return hash;
        }

        /// <summary>
        /// Calculate spatial bounds for a Morton code at a given level.
        /// </summary>
        private SpatialBounds CalculateSpatialBounds(ulong mortonCode, int level)
        {
            var resolution = Math.Pow(2, level);
            var size = _worldBounds.GetSize();
            var cellSize = (size.X / resolution, size.Y / resolution, size.Z / resolution);

            var (x, y, z) = DecodeMorton3D(mortonCode);

            return new SpatialBounds(
                _worldBounds.MinX + x * cellSize.Item1,
                _worldBounds.MinY + y * cellSize.Item2,
                _worldBounds.MinZ + z * cellSize.Item3,
                _worldBounds.MinX + (x + 1) * cellSize.Item1,
                _worldBounds.MinY + (y + 1) * cellSize.Item2,
                _worldBounds.MinZ + (z + 1) * cellSize.Item3
            );
        }

        /// <summary>
        /// Jump consistent hash - assigns key to one of numBuckets buckets.
        /// Provides minimal key redistribution when bucket count changes.
        /// </summary>
        public static int JumpConsistentHash(ulong key, int numBuckets)
        {
            long b = -1;
            long j = 0;

            while (j < numBuckets)
            {
                b = j;
                key = key * 2862933555777941757UL + 1;
                j = (long)((b + 1) * (double)(1L << 31) / ((key >> 33) + 1));
            }

            return (int)b;
        }
    }

    /// <summary>
    /// Represents a spatial hash key with hierarchical information.
    /// </summary>
    public class SpatialHashKey
    {
        public int Level { get; set; }
        public ulong MortonCode { get; set; }
        public string RegionKey { get; set; }
        public ulong PrimaryNodeHash { get; set; }
        public List<ulong> ReplicaNodeHashes { get; set; }
        public string CacheKey { get; set; }
        public SpatialBounds SpatialRegion { get; set; }

        public SpatialHashKey()
        {
            ReplicaNodeHashes = new List<ulong>();
        }
    }
}
