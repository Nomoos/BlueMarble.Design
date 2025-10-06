using System;
using Xunit;
using BlueMarble.Utils.Spatial.DistributedStorage;

namespace BlueMarble.Utils.Spatial.DistributedStorage.Tests
{
    public class SpatialOctreeNodeTests
    {
        [Fact]
        public void Constructor_InitializesDefaultValues()
        {
            var node = new SpatialOctreeNode();

            Assert.NotNull(node.ChildNodeIds);
            Assert.Equal(8, node.ChildNodeIds.Length);
            Assert.NotNull(node.ReplicaHashes);
            Assert.Equal(0, node.Version);
            Assert.True(node.LastModified > DateTime.MinValue);
        }

        [Fact]
        public void ShouldSubdivide_WithLowHomogeneity_ReturnsTrue()
        {
            var node = new SpatialOctreeNode
            {
                IsLeaf = false,
                Homogeneity = 0.5
            };

            Assert.True(node.ShouldSubdivide(0.9));
        }

        [Fact]
        public void ShouldSubdivide_WithHighHomogeneity_ReturnsFalse()
        {
            var node = new SpatialOctreeNode
            {
                IsLeaf = false,
                Homogeneity = 0.95
            };

            Assert.False(node.ShouldSubdivide(0.9));
        }

        [Fact]
        public void ShouldSubdivide_WithLeafNode_ReturnsFalse()
        {
            var node = new SpatialOctreeNode
            {
                IsLeaf = true,
                Homogeneity = 0.5
            };

            Assert.False(node.ShouldSubdivide(0.9));
        }

        [Fact]
        public void CanCompress_WithHighHomogeneity_ReturnsTrue()
        {
            var node = new SpatialOctreeNode
            {
                Homogeneity = 0.95
            };

            Assert.True(node.CanCompress(0.9));
        }

        [Fact]
        public void CanCompress_WithLowHomogeneity_ReturnsFalse()
        {
            var node = new SpatialOctreeNode
            {
                Homogeneity = 0.5
            };

            Assert.False(node.CanCompress(0.9));
        }

        [Theory]
        [InlineData(0.0)]
        [InlineData(0.5)]
        [InlineData(0.89)]
        [InlineData(0.90)]
        [InlineData(1.0)]
        public void ShouldSubdivide_WithCustomThreshold_WorksCorrectly(double homogeneity)
        {
            const double threshold = 0.9;
            var node = new SpatialOctreeNode
            {
                IsLeaf = false,
                Homogeneity = homogeneity
            };

            var expected = homogeneity < threshold;
            Assert.Equal(expected, node.ShouldSubdivide(threshold));
        }
    }

    public class SpatialBoundsTests
    {
        [Fact]
        public void Constructor_WithParameters_SetsValues()
        {
            var bounds = new SpatialBounds(1, 2, 3, 10, 20, 30);

            Assert.Equal(1, bounds.MinX);
            Assert.Equal(2, bounds.MinY);
            Assert.Equal(3, bounds.MinZ);
            Assert.Equal(10, bounds.MaxX);
            Assert.Equal(20, bounds.MaxY);
            Assert.Equal(30, bounds.MaxZ);
        }

        [Fact]
        public void GetSize_ReturnsCorrectDimensions()
        {
            var bounds = new SpatialBounds(0, 0, 0, 100, 200, 300);
            var size = bounds.GetSize();

            Assert.Equal(100, size.X);
            Assert.Equal(200, size.Y);
            Assert.Equal(300, size.Z);
        }

        [Fact]
        public void GetCenter_ReturnsCorrectCenter()
        {
            var bounds = new SpatialBounds(0, 0, 0, 100, 200, 300);
            var center = bounds.GetCenter();

            Assert.Equal(50, center.X);
            Assert.Equal(100, center.Y);
            Assert.Equal(150, center.Z);
        }

        [Theory]
        [InlineData(50, 50, 50, true)]
        [InlineData(0, 0, 0, true)]
        [InlineData(100, 100, 100, true)]
        [InlineData(-1, 50, 50, false)]
        [InlineData(50, 101, 50, false)]
        [InlineData(50, 50, 101, false)]
        public void Contains_ChecksPointCorrectly(double x, double y, double z, bool expected)
        {
            var bounds = new SpatialBounds(0, 0, 0, 100, 100, 100);
            Assert.Equal(expected, bounds.Contains(x, y, z));
        }

        [Fact]
        public void Intersects_WithOverlappingBounds_ReturnsTrue()
        {
            var bounds1 = new SpatialBounds(0, 0, 0, 100, 100, 100);
            var bounds2 = new SpatialBounds(50, 50, 50, 150, 150, 150);

            Assert.True(bounds1.Intersects(bounds2));
            Assert.True(bounds2.Intersects(bounds1));
        }

        [Fact]
        public void Intersects_WithNonOverlappingBounds_ReturnsFalse()
        {
            var bounds1 = new SpatialBounds(0, 0, 0, 10, 10, 10);
            var bounds2 = new SpatialBounds(20, 20, 20, 30, 30, 30);

            Assert.False(bounds1.Intersects(bounds2));
            Assert.False(bounds2.Intersects(bounds1));
        }

        [Fact]
        public void Intersects_WithTouchingBounds_ReturnsTrue()
        {
            var bounds1 = new SpatialBounds(0, 0, 0, 10, 10, 10);
            var bounds2 = new SpatialBounds(10, 10, 10, 20, 20, 20);

            Assert.True(bounds1.Intersects(bounds2));
        }

        [Fact]
        public void Intersects_WithIdenticalBounds_ReturnsTrue()
        {
            var bounds1 = new SpatialBounds(0, 0, 0, 100, 100, 100);
            var bounds2 = new SpatialBounds(0, 0, 0, 100, 100, 100);

            Assert.True(bounds1.Intersects(bounds2));
        }

        [Fact]
        public void Intersects_WithContainedBounds_ReturnsTrue()
        {
            var bounds1 = new SpatialBounds(0, 0, 0, 100, 100, 100);
            var bounds2 = new SpatialBounds(25, 25, 25, 75, 75, 75);

            Assert.True(bounds1.Intersects(bounds2));
            Assert.True(bounds2.Intersects(bounds1));
        }
    }
}
