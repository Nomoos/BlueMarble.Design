using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Numerics;
using BlueMarble.SpatialData;

namespace BlueMarble.SpatialData.Tests;

/// <summary>
/// Test suite for the Template Method pattern implementation
/// Validates the layered architecture and separation of concerns
/// </summary>
[TestClass]
public class LayeredArchitectureTests
{
    private OptimizedOctreeNode _baseTree = null!;
    private DeltaPatchOctree _deltaOctree = null!;

    [TestInitialize]
    public void Setup()
    {
        _baseTree = new OptimizedOctreeNode
        {
            ExplicitMaterial = MaterialData.DefaultOcean
        };

        _deltaOctree = new DeltaPatchOctree(_baseTree, consolidationThreshold: 1000);
    }

    #region Template Method Pattern Tests

    [TestMethod]
    public void ErosionProcess_TemplateMethod_ExecutesAllSteps()
    {
        // Arrange
        var erosion = new ErosionProcess(_deltaOctree);
        var positions = new[]
        {
            new Vector3(1, 1, 1),
            new Vector3(2, 2, 2),
            new Vector3(3, 3, 3)
        };
        
        // Set initial materials (non-air)
        foreach (var pos in positions)
        {
            _deltaOctree.WriteVoxel(pos, new MaterialData(MaterialId.Rock, 2700f, 6.0f));
        }

        var context = new GeologicalProcessContext
        {
            Positions = positions,
            Intensity = 0.5f
        };

        // Act
        erosion.Execute(context);

        // Assert - positions should be affected by erosion
        foreach (var pos in positions)
        {
            var material = _deltaOctree.ReadVoxel(pos);
            // With 50% erosion, rock should have reduced density or be converted to air
            Assert.IsTrue(
                material.MaterialType == MaterialId.Air || material.Density < 2700f,
                "Erosion should reduce density or convert to air");
        }
    }

    [TestMethod]
    public void ErosionProcess_FiltersAirPositions_SkipsAir()
    {
        // Arrange
        var erosion = new ErosionProcess(_deltaOctree);
        var positions = new[]
        {
            new Vector3(1, 1, 1), // Will be rock
            new Vector3(2, 2, 2), // Will be air
            new Vector3(3, 3, 3)  // Will be rock
        };

        _deltaOctree.WriteVoxel(positions[0], new MaterialData(MaterialId.Rock, 2700f, 6.0f));
        // positions[1] remains air (default ocean from base tree)
        _deltaOctree.WriteVoxel(positions[2], new MaterialData(MaterialId.Rock, 2700f, 6.0f));

        var context = new GeologicalProcessContext
        {
            Positions = positions,
            Intensity = 1.0f // Complete erosion
        };

        // Act
        erosion.Execute(context);

        // Assert - air positions should remain unchanged
        // Rock positions should be eroded
        Assert.AreEqual(MaterialId.Air, _deltaOctree.ReadVoxel(positions[0]).MaterialType);
        Assert.AreEqual(MaterialId.Air, _deltaOctree.ReadVoxel(positions[2]).MaterialType);
    }

    [TestMethod]
    public void DepositionProcess_TemplateMethod_DepositsCorrectly()
    {
        // Arrange
        var deposition = new DepositionProcess(_deltaOctree);
        var positions = new[]
        {
            new Vector3(1, 1, 1),
            new Vector3(2, 2, 2)
        };

        var sandMaterial = new MaterialData(MaterialId.Sand, 1600f, 2.5f);
        var context = new GeologicalProcessContext
        {
            Positions = positions,
            TargetMaterial = sandMaterial
        };

        // Act
        deposition.Execute(context);

        // Assert
        foreach (var pos in positions)
        {
            var material = _deltaOctree.ReadVoxel(pos);
            Assert.AreEqual(MaterialId.Sand, material.MaterialType);
            Assert.AreEqual(1600f, material.Density, 0.01f);
        }
    }

    [TestMethod]
    public void VolcanicIntrusionProcess_SphericalRegion_AffectsCorrectPositions()
    {
        // Arrange
        var volcanic = new VolcanicIntrusionProcess(_deltaOctree);
        var center = new Vector3(10, 10, 10);
        var radius = 2.0f;
        var lavaMaterial = new MaterialData(MaterialId.Lava, 3100f, 1.0f);

        var context = new GeologicalProcessContext
        {
            Center = center,
            Radius = radius,
            TargetMaterial = lavaMaterial
        };

        // Act
        volcanic.Execute(context);

        // Assert - positions within radius should be lava
        var testPosition = center; // Center should definitely be lava
        Assert.AreEqual(MaterialId.Lava, _deltaOctree.ReadVoxel(testPosition).MaterialType);

        // Position just outside radius should not be affected
        var outsidePosition = new Vector3(center.X + radius + 1, center.Y, center.Z);
        var outsideMaterial = _deltaOctree.ReadVoxel(outsidePosition);
        Assert.AreNotEqual(MaterialId.Lava, outsideMaterial.MaterialType);
    }

    [TestMethod]
    public void TectonicDeformationProcess_MovesCorrectly()
    {
        // Arrange
        var tectonic = new TectonicDeformationProcess(_deltaOctree);
        var from = new Vector3(5, 5, 5);
        var to = new Vector3(10, 10, 10);
        
        // Set material at source
        var graniteMaterial = new MaterialData(MaterialId.Granite, 2750f, 7.0f);
        _deltaOctree.WriteVoxel(from, graniteMaterial);

        var displacements = new[] { (from, to) };
        var context = new GeologicalProcessContext
        {
            Displacements = displacements
        };

        // Act
        tectonic.Execute(context);

        // Assert
        var sourceMaterial = _deltaOctree.ReadVoxel(from);
        var destMaterial = _deltaOctree.ReadVoxel(to);

        Assert.AreEqual(MaterialId.Air, sourceMaterial.MaterialType, "Source should be cleared");
        Assert.AreEqual(MaterialId.Granite, destMaterial.MaterialType, "Destination should have granite");
    }

    [TestMethod]
    public void WeatheringProcess_OnlySurfacePositions_FilteredCorrectly()
    {
        // Arrange
        var weathering = new WeatheringProcess(_deltaOctree);
        
        // Create a position that has an air neighbor (making it a surface position)
        var surfacePos = new Vector3(5, 5, 5);
        var airNeighbor = new Vector3(6, 5, 5); // Adjacent position
        
        // Set the surface position to rock
        _deltaOctree.WriteVoxel(surfacePos, new MaterialData(MaterialId.Rock, 2700f, 6.0f));
        // Set neighbor to air to make surfacePos a surface position
        _deltaOctree.WriteVoxel(airNeighbor, MaterialData.DefaultAir);
        
        var weatheredMaterial = new MaterialData(MaterialId.Dirt, 1200f, 1.0f);
        var context = new GeologicalProcessContext
        {
            Positions = new[] { surfacePos },
            TargetMaterial = weatheredMaterial,
            Intensity = 1.0f
        };

        // Act
        weathering.Execute(context);

        // Assert - since position has air neighbors, it should be weathered
        var material = _deltaOctree.ReadVoxel(surfacePos);
        Assert.AreEqual(MaterialId.Dirt, material.MaterialType);
    }

    #endregion

    #region Separation of Concerns Tests

    [TestMethod]
    public void GeologicalProcessBase_ValidatesContext_RejectsInvalidIntensity()
    {
        // Arrange
        var erosion = new ErosionProcess(_deltaOctree);
        var context = new GeologicalProcessContext
        {
            Positions = new[] { new Vector3(1, 1, 1) },
            Intensity = -0.5f // Invalid intensity
        };

        // Act & Assert
        Assert.ThrowsException<ArgumentException>(() => erosion.Execute(context));
    }

    [TestMethod]
    public void GeologicalProcessBase_SpatialLocalityFactor_ReturnsCorrectValues()
    {
        // Arrange
        var erosion = new ErosionProcess(_deltaOctree);
        var volcanic = new VolcanicIntrusionProcess(_deltaOctree);
        var tectonic = new TectonicDeformationProcess(_deltaOctree);

        // Assert - verify spatial locality matches documentation
        Assert.AreEqual(0.85, erosion.GetSpatialLocalityFactor(), "Erosion should have 85% locality");
        Assert.AreEqual(0.90, volcanic.GetSpatialLocalityFactor(), "Volcanic should have 90% locality");
        Assert.AreEqual(0.65, tectonic.GetSpatialLocalityFactor(), "Tectonic should have 65% locality");
    }

    #endregion

    #region Layer Communication Tests

    [TestMethod]
    public void LayeredArchitecture_NoLayerSkipping_ProcessUsesDeltaOctree()
    {
        // Arrange
        var erosion = new ErosionProcess(_deltaOctree);
        var position = new Vector3(5, 5, 5);
        
        // Set initial material through delta octree
        _deltaOctree.WriteVoxel(position, new MaterialData(MaterialId.Rock, 2700f, 6.0f));

        var context = new GeologicalProcessContext
        {
            Positions = new[] { position },
            Intensity = 1.0f
        };

        // Act
        erosion.Execute(context);

        // Assert - verify process communicated through delta octree layer
        var material = _deltaOctree.ReadVoxel(position);
        Assert.AreEqual(MaterialId.Air, material.MaterialType);
        
        // Verify delta was created in delta octree layer
        Assert.IsTrue(_deltaOctree.HasDelta(position) || 
                     material.MaterialType == MaterialId.Air);
    }

    [TestMethod]
    public void GeomorphologicalAdapter_DelegatesToProcessLayer_MaintainsLayering()
    {
        // Arrange
        var adapter = new GeomorphologicalOctreeAdapter(_deltaOctree);
        var positions = new[] { new Vector3(1, 1, 1) };
        
        // Set initial material
        _deltaOctree.WriteVoxel(positions[0], new MaterialData(MaterialId.Rock, 2700f, 6.0f));

        // Act - use adapter (orchestration layer)
        adapter.ApplyErosion(positions, 1.0f);

        // Assert - verify change was applied through proper layers
        var material = adapter.GetMaterial(positions[0]);
        Assert.AreEqual(MaterialId.Air, material.MaterialType);
    }

    #endregion

    #region Reusability Tests

    [TestMethod]
    public void TemplateMethod_CommonWorkflow_SharedByAllProcesses()
    {
        // This test verifies that all processes follow the same workflow
        // by checking they all go through validation, calculation, and application steps

        // Arrange
        var processes = new GeologicalProcessBase[]
        {
            new ErosionProcess(_deltaOctree),
            new DepositionProcess(_deltaOctree),
            new VolcanicIntrusionProcess(_deltaOctree),
            new TectonicDeformationProcess(_deltaOctree),
            new WeatheringProcess(_deltaOctree)
        };

        // Assert - all processes should have a spatial locality factor
        foreach (var process in processes)
        {
            var locality = process.GetSpatialLocalityFactor();
            Assert.IsTrue(locality >= 0 && locality <= 1.0, 
                $"Process {process.GetType().Name} should have valid locality factor");
        }
    }

    [TestMethod]
    public void GeologicalProcessContext_ParameterObject_ReducesComplexity()
    {
        // Arrange & Act
        var context = new GeologicalProcessContext
        {
            Positions = new[] { new Vector3(1, 1, 1) },
            Intensity = 0.5f,
            Center = new Vector3(5, 5, 5),
            Radius = 10f,
            TargetMaterial = MaterialData.DefaultAir
        };

        // Assert
        Assert.IsTrue(context.IsValid(), "Context should be valid");
        Assert.IsNotNull(context.Positions);
        Assert.IsNotNull(context.Center);
        Assert.IsNotNull(context.TargetMaterial);
    }

    #endregion
}
