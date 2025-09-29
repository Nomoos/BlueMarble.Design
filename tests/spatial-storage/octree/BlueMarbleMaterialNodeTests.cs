using System;
using Xunit;
using BlueMarble.SpatialStorage.Octree;

namespace BlueMarble.Tests.SpatialStorage.Octree
{
    /// <summary>
    /// Tests for BlueMarble-specific material inheritance scenarios
    /// </summary>
    public class BlueMarbleMaterialNodeTests
    {
        [Fact]
        public void CreateBlueMarbleExample_CreatesCorrectHierarchy()
        {
            // Act
            var root = BlueMarbleMaterialNode.CreateBlueMarbleExample();

            // Assert
            Assert.Equal(MaterialId.Air, root.PrimaryMaterial);
            Assert.Equal(16.0, root.CellSize);
            Assert.Equal(1, root.Level);
            Assert.NotNull(root.Children);

            // Check child structure
            var child = root.Children[0] as BlueMarbleMaterialNode;
            Assert.NotNull(child);
            Assert.Equal(MaterialId.Air, child.PrimaryMaterial);
            Assert.Equal(8.0, child.CellSize);
            Assert.Equal(2, child.Level);

            // Check grandchild
            var grandchild = child?.Children?[0] as BlueMarbleMaterialNode;
            Assert.NotNull(grandchild);
            Assert.Equal(MaterialId.Dirt, grandchild.PrimaryMaterial);
            Assert.Equal(4.0, grandchild.CellSize);
            Assert.Equal(3, grandchild.Level);
            Assert.NotNull(grandchild.ExplicitMaterial); // Should have explicit material since it differs from parent
        }

        [Fact]
        public void CalculateMemorySavings_ShowsSignificantSavings()
        {
            // Arrange
            var root = BlueMarbleMaterialNode.CreateBlueMarbleExample();

            // Act
            var report = root.CalculateMemorySavings();

            // Assert
            Assert.True(report.SavingsPercentage > 40, 
                $"Expected at least 40% savings, got {report.SavingsPercentage:F1}%");
            Assert.True(report.SavingsBytes > 0, "Should show actual byte savings");
            Assert.True(report.NodesWithInheritance > 0, "Should have nodes using inheritance");
            Assert.True(report.TotalNodes > report.NodesWithInheritance, "Should have both inherited and explicit nodes");
        }

        [Fact]
        public void HomogeneityThreshold_IsSetTo90Percent()
        {
            // Assert
            Assert.Equal(0.9, BlueMarbleMaterialNode.HOMOGENEITY_THRESHOLD);
        }

        [Fact]
        public void GetMaterialAtPosition_HighHomogeneity_ReturnsDominantMaterial()
        {
            // Arrange
            var node = new BlueMarbleMaterialNode
            {
                PrimaryMaterial = MaterialId.Air,
                CellSize = 16.0,
                Center = new Vector3(0, 0, 100), // High elevation = mostly air
                Bounds = new BoundingBox
                {
                    Min = new Vector3(-8, -8, 92),
                    Max = new Vector3(8, 8, 108)
                }
            };

            var position = new Vector3(0, 0, 100);

            // Act
            var material = node.GetMaterialAtPosition(position, 1);

            // Assert
            Assert.Equal(MaterialId.Air, material);
        }

        [Fact]
        public void MemorySavingsReport_FormatsCorrectly()
        {
            // Arrange
            var report = new MemorySavingsReport
            {
                ExplicitMemoryBytes = 1000,
                InheritanceMemoryBytes = 200,
                SavingsBytes = 800,
                SavingsPercentage = 80.0,
                NodesWithInheritance = 8,
                TotalNodes = 10
            };

            // Act
            var formatted = report.ToString();

            // Assert
            Assert.Contains("80.0%", formatted);
            Assert.Contains("800", formatted);
            Assert.Contains("8/10", formatted);
        }

        [Fact]
        public void MaterialData_DefaultOcean_HasCorrectProperties()
        {
            // Act
            var ocean = MaterialData.DefaultOcean;

            // Assert
            Assert.Equal(MaterialId.Ocean, ocean.Id);
            Assert.Equal("Ocean", ocean.Name);
            Assert.Equal(1.025f, ocean.Density, 0.001f);
            Assert.True(ocean.Properties.IsLiquid);
        }

        [Fact]
        public void MaterialData_Equals_WorksCorrectly()
        {
            // Arrange
            var material1 = new MaterialData
            {
                Id = MaterialId.Rock,
                Name = "Rock",
                Density = 2.5f,
                Properties = new MaterialProperties()
            };

            var material2 = new MaterialData
            {
                Id = MaterialId.Rock,
                Name = "Rock",
                Density = 2.5f,
                Properties = new MaterialProperties()
            };

            var material3 = new MaterialData
            {
                Id = MaterialId.Dirt,
                Name = "Dirt",
                Density = 1.5f,
                Properties = new MaterialProperties()
            };

            // Act & Assert
            Assert.True(material1.Equals(material2));
            Assert.False(material1.Equals(material3));
            Assert.True(material1 == material2);
            Assert.False(material1 == material3);
        }

        [Fact]
        public void BoundingBox_Contains_WorksCorrectly()
        {
            // Arrange
            var bounds = new BoundingBox
            {
                Min = new Vector3(0, 0, 0),
                Max = new Vector3(10, 10, 10)
            };

            // Act & Assert
            Assert.True(bounds.Contains(new Vector3(5, 5, 5))); // Inside
            Assert.True(bounds.Contains(new Vector3(0, 0, 0))); // On boundary
            Assert.True(bounds.Contains(new Vector3(10, 10, 10))); // On boundary
            Assert.False(bounds.Contains(new Vector3(-1, 5, 5))); // Outside
            Assert.False(bounds.Contains(new Vector3(11, 5, 5))); // Outside
        }

        [Fact]
        public void BoundingBox_Center_CalculatesCorrectly()
        {
            // Arrange
            var bounds = new BoundingBox
            {
                Min = new Vector3(0, 0, 0),
                Max = new Vector3(10, 20, 30)
            };

            // Act
            var center = bounds.Center;

            // Assert
            Assert.Equal(5, center.X);
            Assert.Equal(10, center.Y);
            Assert.Equal(15, center.Z);
        }

        /// <summary>
        /// Integration test that validates the core inheritance scenario:
        /// Large homogeneous ocean region with small islands
        /// </summary>
        [Fact]
        public void IntegrationTest_OceanWithIslands_DemonstratesMemorySavings()
        {
            // Arrange - Create a large ocean region with small islands
            var oceanRoot = new BlueMarbleMaterialNode
            {
                PrimaryMaterial = MaterialId.Ocean,
                CellSize = 1000.0, // 1km x 1km ocean region
                Center = new Vector3(0, 0, -10), // Below sea level
                Level = 0,
                Bounds = new BoundingBox
                {
                    Min = new Vector3(-500, -500, -20),
                    Max = new Vector3(500, 500, 0)
                }
            };

            // Create 8 child regions, most inherit ocean material
            oceanRoot.Children = new MaterialInheritanceNode[8];
            
            for (int i = 0; i < 8; i++)
            {
                oceanRoot.Children[i] = new BlueMarbleMaterialNode
                {
                    PrimaryMaterial = MaterialId.Ocean,
                    Parent = oceanRoot,
                    Level = 1,
                    // Only first child has an island (explicit material)
                    ExplicitMaterial = i == 0 ? new MaterialData { Id = MaterialId.Rock, Name = "Island", Density = 2.0f } : null
                };
            }

            // Act
            var report = oceanRoot.CalculateMemorySavings();

            // Assert - Should show significant memory savings
            Assert.True(report.SavingsPercentage >= 70, 
                $"Expected at least 70% savings for ocean scenario, got {report.SavingsPercentage:F1}%");
            
            // Verify inheritance chain works
            var inheritingChild = oceanRoot.Children[1];
            Assert.Null(inheritingChild.ExplicitMaterial);
            Assert.Equal(MaterialId.Ocean, inheritingChild.GetEffectiveMaterial().Id);

            // Verify explicit material works
            var islandChild = oceanRoot.Children[0];
            Assert.NotNull(islandChild.ExplicitMaterial);
            Assert.Equal(MaterialId.Rock, islandChild.GetEffectiveMaterial().Id);
        }

        /// <summary>
        /// Performance test to validate O(log n) inheritance lookup performance
        /// </summary>
        [Fact]
        public void PerformanceTest_DeepInheritanceChain_RemainsEfficient()
        {
            // Arrange - Create deep inheritance chain (simulating 20 levels)
            var root = new MaterialInheritanceNode
            {
                ExplicitMaterial = MaterialData.DefaultOcean
            };

            var current = root;
            for (int level = 1; level <= 20; level++)
            {
                var child = new MaterialInheritanceNode
                {
                    Parent = current,
                    Level = level,
                    ExplicitMaterial = null // Inherit from root
                };
                current.Children = new MaterialInheritanceNode[] { child };
                current = child;
            }

            // Act - Measure lookup time
            var startTime = DateTime.UtcNow;
            var material = current.GetEffectiveMaterial();
            var endTime = DateTime.UtcNow;
            var lookupTime = endTime - startTime;

            // Assert
            Assert.Equal(MaterialId.Ocean, material.Id);
            Assert.True(lookupTime.TotalMilliseconds < 10, 
                $"Inheritance lookup took {lookupTime.TotalMilliseconds}ms, should be < 10ms");
        }
    }
}