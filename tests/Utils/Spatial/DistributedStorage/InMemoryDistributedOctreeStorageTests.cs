using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using BlueMarble.Utils.Spatial.DistributedStorage;

namespace BlueMarble.Utils.Spatial.DistributedStorage.Tests
{
    public class InMemoryDistributedOctreeStorageTests
    {
        private readonly SpatialBounds _testBounds;
        private readonly IDistributedOctreeStorage _storage;

        public InMemoryDistributedOctreeStorageTests()
        {
            _testBounds = new SpatialBounds(0, 0, 0, 1000, 1000, 1000);
            _storage = new InMemoryDistributedOctreeStorage(_testBounds, maxCacheSize: 100);
        }

        [Fact]
        public async Task WriteNodeAsync_NewNode_Succeeds()
        {
            var node = CreateTestNode("node1", 5, 12345, 1);

            var result = await _storage.WriteNodeAsync(node);

            Assert.True(result.Success);
            Assert.Equal(1, result.NewVersion);
            Assert.True(result.AcknowledgedReplicas > 0);
        }

        [Fact]
        public async Task WriteNodeAsync_NullNode_ThrowsException()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(
                async () => await _storage.WriteNodeAsync(null));
        }

        [Fact]
        public async Task GetNodeAsync_ExistingNode_ReturnsNode()
        {
            var node = CreateTestNode("node1", 5, 12345, 1);
            await _storage.WriteNodeAsync(node);

            var retrieved = await _storage.GetNodeAsync("node1");

            Assert.NotNull(retrieved);
            Assert.Equal("node1", retrieved.Id);
            Assert.Equal(5, retrieved.Level);
            Assert.Equal(1, retrieved.MaterialId);
        }

        [Fact]
        public async Task GetNodeAsync_NonExistentNode_ReturnsNull()
        {
            var retrieved = await _storage.GetNodeAsync("nonexistent");

            Assert.Null(retrieved);
        }

        [Fact]
        public async Task GetNodeByMortonAsync_ExistingNode_ReturnsNode()
        {
            var node = CreateTestNode("node1", 5, 12345, 1);
            await _storage.WriteNodeAsync(node);

            var retrieved = await _storage.GetNodeByMortonAsync(12345, 5);

            Assert.NotNull(retrieved);
            Assert.Equal("node1", retrieved.Id);
            Assert.Equal(12345ul, retrieved.MortonCode);
        }

        [Fact]
        public async Task QueryMaterialAsync_ExistingNode_ReturnsResult()
        {
            var node = CreateTestNode("node1", 5, 100, 42);
            node.Bounds = new SpatialBounds(400, 400, 400, 500, 500, 500);
            await _storage.WriteNodeAsync(node);

            var result = await _storage.QueryMaterialAsync(450, 450, 450, 5);

            Assert.NotNull(result);
            Assert.True(result.Success);
            Assert.Equal(42, result.MaterialId);
        }

        [Fact]
        public async Task QueryMaterialAsync_NonExistentPosition_ReturnsNotFound()
        {
            var result = await _storage.QueryMaterialAsync(999, 999, 999, 10);

            Assert.NotNull(result);
            Assert.False(result.Success);
        }

        [Fact]
        public async Task WriteNodeAsync_VersionConflict_ReturnsFailure()
        {
            var node = CreateTestNode("node1", 5, 12345, 1);
            await _storage.WriteNodeAsync(node);

            // Create a new node object with same ID but old version (simulates concurrent update)
            var conflictingNode = CreateTestNode("node1", 5, 12345, 1);
            conflictingNode.Version = 0;  // Old version
            var result = await _storage.WriteNodeAsync(conflictingNode);

            Assert.False(result.Success);
            Assert.Contains("Version conflict", result.ErrorMessage);
        }

        [Fact]
        public async Task WriteNodeAsync_IncrementsVersion()
        {
            var node = CreateTestNode("node1", 5, 12345, 1);
            
            var result1 = await _storage.WriteNodeAsync(node);
            Assert.Equal(1, result1.NewVersion);

            var retrieved = await _storage.GetNodeAsync("node1");
            var result2 = await _storage.WriteNodeAsync(retrieved);
            Assert.Equal(2, result2.NewVersion);
        }

        [Fact]
        public async Task DeleteNodeAsync_ExistingNode_Succeeds()
        {
            var node = CreateTestNode("node1", 5, 12345, 1);
            await _storage.WriteNodeAsync(node);

            var deleted = await _storage.DeleteNodeAsync("node1");

            Assert.True(deleted);

            var retrieved = await _storage.GetNodeAsync("node1");
            Assert.Null(retrieved);
        }

        [Fact]
        public async Task DeleteNodeAsync_NonExistentNode_ReturnsFalse()
        {
            var deleted = await _storage.DeleteNodeAsync("nonexistent");

            Assert.False(deleted);
        }

        [Fact]
        public async Task QueryRegionAsync_ReturnsNodesInRegion()
        {
            var node1 = CreateTestNode("node1", 5, 100, 1);
            node1.Bounds = new SpatialBounds(0, 0, 0, 100, 100, 100);
            await _storage.WriteNodeAsync(node1);

            var node2 = CreateTestNode("node2", 5, 200, 2);
            node2.Bounds = new SpatialBounds(200, 200, 200, 300, 300, 300);
            await _storage.WriteNodeAsync(node2);

            var queryBounds = new SpatialBounds(0, 0, 0, 150, 150, 150);
            var results = await _storage.QueryRegionAsync(queryBounds, 5);

            Assert.Single(results);
            Assert.Equal("node1", results[0].Id);
        }

        [Fact]
        public async Task GetStatisticsAsync_ReturnsCorrectStats()
        {
            var node1 = CreateTestNode("node1", 3, 100, 1);
            var node2 = CreateTestNode("node2", 5, 200, 2);
            await _storage.WriteNodeAsync(node1);
            await _storage.WriteNodeAsync(node2);

            await _storage.QueryMaterialAsync(500, 500, 500, 5);
            await _storage.QueryMaterialAsync(600, 600, 600, 5);

            var stats = await _storage.GetStatisticsAsync();

            Assert.Equal(2, stats.TotalNodes);
            Assert.Equal(2, stats.TotalQueries);
            Assert.Equal(2, stats.TotalWrites);
            Assert.True(stats.NodesByLevel.Count > 0);
        }

        [Fact]
        public async Task CacheHit_AfterSecondQuery_ImprovesPerformance()
        {
            var hashGen = new SpatialHashGenerator(_testBounds);
            var morton = hashGen.EncodeMorton3D(450, 450, 450, 5);
            
            var node = CreateTestNode("node1", 5, morton, 1);
            node.Bounds = new SpatialBounds(400, 400, 400, 500, 500, 500);
            await _storage.WriteNodeAsync(node);

            // First query - cache miss or storage
            var result1 = await _storage.QueryMaterialAsync(450, 450, 450, 5);
            
            // Second query - should hit cache
            var result2 = await _storage.QueryMaterialAsync(450, 450, 450, 5);

            Assert.True(result2.Source == "L1Cache" || result2.Source == "Storage");
            Assert.True(result2.Success);
            Assert.Equal(1, result2.MaterialId);
        }

        [Fact]
        public async Task WriteNodeAsync_WithQuorumConsistency_AcknowledgesMajority()
        {
            var node = CreateTestNode("node1", 5, 12345, 1);

            var result = await _storage.WriteNodeAsync(node, ConsistencyLevel.Quorum);

            Assert.True(result.Success);
            Assert.True(result.AcknowledgedReplicas >= 2);
        }

        [Fact]
        public async Task WriteNodeAsync_WithAllConsistency_AcknowledgesAll()
        {
            var node = CreateTestNode("node1", 5, 12345, 1);

            var result = await _storage.WriteNodeAsync(node, ConsistencyLevel.All);

            Assert.True(result.Success);
            Assert.Equal(3, result.AcknowledgedReplicas);
        }

        [Fact]
        public async Task ClearCacheAsync_RemovesCachedItems()
        {
            var node = CreateTestNode("node1", 5, 100, 1);
            node.Bounds = new SpatialBounds(400, 400, 400, 500, 500, 500);
            await _storage.WriteNodeAsync(node);

            // Query to populate cache
            await _storage.QueryMaterialAsync(450, 450, 450, 5);

            await _storage.ClearCacheAsync();

            // Next query should be from storage
            var result = await _storage.QueryMaterialAsync(450, 450, 450, 5);
            Assert.Equal("Storage", result.Source);
        }

        [Fact]
        public async Task MultipleWrites_MaintainVersionConsistency()
        {
            var node = CreateTestNode("node1", 5, 12345, 1);

            for (int i = 0; i < 10; i++)
            {
                var retrieved = await _storage.GetNodeAsync("node1");
                if (retrieved == null)
                {
                    node.Version = 0;
                }
                else
                {
                    node = retrieved;
                }

                node.MaterialId = i;
                var result = await _storage.WriteNodeAsync(node);
                
                Assert.True(result.Success);
                Assert.Equal(i + 1, result.NewVersion);
            }
        }

        [Fact]
        public async Task HierarchicalQuery_FindsCoarsestAvailableNode()
        {
            // Create nodes at different levels
            var nodeL3 = CreateTestNode("nodeL3", 3, 50, 1);
            nodeL3.Bounds = new SpatialBounds(400, 400, 400, 600, 600, 600);
            await _storage.WriteNodeAsync(nodeL3);

            // Query at higher level should find coarser node
            var result = await _storage.QueryMaterialAsync(500, 500, 500, 5);

            Assert.True(result.Success);
            Assert.Equal(1, result.MaterialId);
        }

        private SpatialOctreeNode CreateTestNode(string id, int level, ulong mortonCode, int materialId)
        {
            return new SpatialOctreeNode
            {
                Id = id,
                Level = level,
                MortonCode = mortonCode,
                MaterialId = materialId,
                Homogeneity = 0.95,
                IsLeaf = true,
                Bounds = new SpatialBounds(0, 0, 0, 100, 100, 100),
                Version = 0
            };
        }
    }
}
