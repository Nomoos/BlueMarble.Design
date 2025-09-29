using System;
using Xunit;
using BlueMarble.SpatialStorage.Octree;

namespace BlueMarble.Tests.SpatialStorage.Octree
{
    /// <summary>
    /// Unit tests for material inheritance functionality
    /// </summary>
    public class MaterialInheritanceTests
    {
        [Fact]
        public void GetEffectiveMaterial_WithExplicitMaterial_ReturnsExplicitMaterial()
        {
            // Arrange
            var material = new MaterialData
            {
                Id = MaterialId.Rock,
                Name = "Rock",
                Density = 2.5f
            };

            var node = new MaterialInheritanceNode
            {
                ExplicitMaterial = material
            };

            // Act
            var result = node.GetEffectiveMaterial();

            // Assert
            Assert.Equal(material, result);
        }

        [Fact]
        public void GetEffectiveMaterial_WithoutExplicitMaterial_InheritsFromParent()
        {
            // Arrange
            var parentMaterial = new MaterialData
            {
                Id = MaterialId.Ocean,
                Name = "Ocean",
                Density = 1.025f
            };

            var parent = new MaterialInheritanceNode
            {
                ExplicitMaterial = parentMaterial
            };

            var child = new MaterialInheritanceNode
            {
                Parent = parent,
                ExplicitMaterial = null // Should inherit
            };

            // Act
            var result = child.GetEffectiveMaterial();

            // Assert
            Assert.Equal(parentMaterial, result);
        }

        [Fact]
        public void GetEffectiveMaterial_DeepInheritanceChain_WalksUpToFindMaterial()
        {
            // Arrange
            var rootMaterial = new MaterialData
            {
                Id = MaterialId.Air,
                Name = "Air",
                Density = 0.001f
            };

            var root = new MaterialInheritanceNode { ExplicitMaterial = rootMaterial };
            var level1 = new MaterialInheritanceNode { Parent = root };
            var level2 = new MaterialInheritanceNode { Parent = level1 };
            var level3 = new MaterialInheritanceNode { Parent = level2 };

            // Act
            var result = level3.GetEffectiveMaterial();

            // Assert
            Assert.Equal(rootMaterial, result);
        }

        [Fact]
        public void GetEffectiveMaterial_NoParentNoExplicit_ReturnsDefaultOcean()
        {
            // Arrange
            var node = new MaterialInheritanceNode
            {
                ExplicitMaterial = null,
                Parent = null
            };

            // Act
            var result = node.GetEffectiveMaterial();

            // Assert
            Assert.Equal(MaterialData.DefaultOcean.Id, result.Id);
        }

        [Fact]
        public void RequiresExplicitMaterial_SameMaterialAsParent_ReturnsFalse()
        {
            // Arrange
            var material = MaterialData.DefaultOcean;
            var parent = new MaterialInheritanceNode { ExplicitMaterial = material };
            var child = new MaterialInheritanceNode 
            { 
                Parent = parent,
                ExplicitMaterial = material
            };

            // Act
            var result = child.RequiresExplicitMaterial();

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void RequiresExplicitMaterial_DifferentMaterialThanParent_ReturnsTrue()
        {
            // Arrange
            var parentMaterial = MaterialData.DefaultOcean;
            var childMaterial = new MaterialData
            {
                Id = MaterialId.Rock,
                Name = "Rock",
                Density = 2.5f
            };

            var parent = new MaterialInheritanceNode { ExplicitMaterial = parentMaterial };
            var child = new MaterialInheritanceNode 
            { 
                Parent = parent,
                ExplicitMaterial = childMaterial
            };

            // Act
            var result = child.RequiresExplicitMaterial();

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void SetMaterial_SameAsInherited_ClearsExplicitMaterial()
        {
            // Arrange
            var material = MaterialData.DefaultOcean;
            var parent = new MaterialInheritanceNode { ExplicitMaterial = material };
            var child = new MaterialInheritanceNode { Parent = parent };

            // Act
            child.SetMaterial(material);

            // Assert
            Assert.Null(child.ExplicitMaterial);
            Assert.Equal(material.Id, child.GetEffectiveMaterial().Id);
        }

        [Fact]
        public void SetMaterial_DifferentFromInherited_SetsExplicitMaterial()
        {
            // Arrange
            var parentMaterial = MaterialData.DefaultOcean;
            var childMaterial = new MaterialData
            {
                Id = MaterialId.Dirt,
                Name = "Dirt",
                Density = 1.5f
            };

            var parent = new MaterialInheritanceNode { ExplicitMaterial = parentMaterial };
            var child = new MaterialInheritanceNode { Parent = parent };

            // Act
            child.SetMaterial(childMaterial);

            // Assert
            Assert.NotNull(child.ExplicitMaterial);
            Assert.Equal(childMaterial, child.ExplicitMaterial);
            Assert.Equal(childMaterial, child.GetEffectiveMaterial());
        }

        [Fact]
        public void OptimizeInheritance_RemovesUnnecessaryExplicitMaterials()
        {
            // Arrange
            var material = MaterialData.DefaultOcean;
            var parent = new MaterialInheritanceNode { ExplicitMaterial = material };
            var child = new MaterialInheritanceNode 
            { 
                Parent = parent,
                ExplicitMaterial = material // Same as parent - should be removed
            };

            // Act
            child.OptimizeInheritance();

            // Assert
            Assert.Null(child.ExplicitMaterial);
            Assert.Equal(material.Id, child.GetEffectiveMaterial().Id);
        }

        [Fact]
        public void CalculateMemoryFootprint_WithInheritance_UsesSmallerFootprint()
        {
            // Arrange
            var parent = new MaterialInheritanceNode 
            { 
                ExplicitMaterial = MaterialData.DefaultOcean 
            };

            var childWithExplicit = new MaterialInheritanceNode 
            { 
                Parent = parent,
                ExplicitMaterial = new MaterialData
                {
                    Id = MaterialId.Rock,
                    Name = "Rock",
                    Density = 2.5f
                }
            };

            var childWithInheritance = new MaterialInheritanceNode 
            { 
                Parent = parent,
                ExplicitMaterial = null // Inherits from parent
            };

            // Act
            var explicitFootprint = childWithExplicit.CalculateMemoryFootprint();
            var inheritanceFootprint = childWithInheritance.CalculateMemoryFootprint();

            // Assert
            Assert.True(inheritanceFootprint < explicitFootprint, 
                $"Inheritance footprint ({inheritanceFootprint}) should be smaller than explicit ({explicitFootprint})");
        }

        [Fact]
        public void GetMaterialAtPoint_WithinBounds_ReturnsCorrectMaterial()
        {
            // Arrange
            var material = new MaterialData
            {
                Id = MaterialId.Rock,
                Name = "Rock",
                Density = 2.5f
            };

            var node = new MaterialInheritanceNode
            {
                ExplicitMaterial = material,
                Bounds = new BoundingBox
                {
                    Min = new Vector3(0, 0, 0),
                    Max = new Vector3(10, 10, 10)
                }
            };

            var point = new Vector3(5, 5, 5);

            // Act
            var result = node.GetMaterialAtPoint(point);

            // Assert
            Assert.Equal(material, result);
        }

        [Fact]
        public void GetMaterialAtPoint_OutsideBounds_ThrowsArgumentException()
        {
            // Arrange
            var node = new MaterialInheritanceNode
            {
                Bounds = new BoundingBox
                {
                    Min = new Vector3(0, 0, 0),
                    Max = new Vector3(10, 10, 10)
                }
            };

            var point = new Vector3(15, 15, 15); // Outside bounds

            // Act & Assert
            Assert.Throws<ArgumentException>(() => node.GetMaterialAtPoint(point));
        }
    }
}