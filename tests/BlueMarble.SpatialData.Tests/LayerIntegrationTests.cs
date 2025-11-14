using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Numerics;
using BlueMarble.SpatialData;

namespace BlueMarble.SpatialData.Tests;

/// <summary>
/// Integration tests for layer interaction
/// Tests the full stack from MaterialData through all layers
/// </summary>
[TestClass]
public class LayerIntegrationTests
{
    #region Full Stack Integration Tests

    [TestMethod]
    public void FullStack_MaterialToOctreeToDeltoToGeo_WorksEndToEnd()
    {
        // Arrange - Build full stack
        var baseTree = new OptimizedOctreeNode
        {
            ExplicitMaterial = MaterialData.DefaultOcean
        };
        var deltaOctree = new DeltaPatchOctree(baseTree);
        var geoAdapter = new GeomorphologicalOctreeAdapter(deltaOctree);
        
        // Act - Apply geological process through full stack
        var positions = new List<Vector3>
        {
            new Vector3(10, 10, 10),
            new Vector3(11, 10, 10),
            new Vector3(12, 10, 10)
        };
        var sandMaterial = new MaterialData(MaterialId.Sand, 1600f, 2.5f);
        
        geoAdapter.ApplyDeposition(positions, sandMaterial);
        
        // Assert - Verify data flows through all layers
        foreach (var pos in positions)
        {
            var material = geoAdapter.GetMaterial(pos);
            Assert.AreEqual(sandMaterial, material,
                $"Material at {pos} should flow through all layers correctly");
        }
    }

    [TestMethod]
    public void Integration_ErosionThenDeposition_HandlesSequentialProcesses()
    {
        // Arrange
        var baseTree = new OptimizedOctreeNode
        {
            ExplicitMaterial = new MaterialData(MaterialId.Rock, 2700f, 6.0f)
        };
        var deltaOctree = new DeltaPatchOctree(baseTree);
        var geoAdapter = new GeomorphologicalOctreeAdapter(deltaOctree);
        
        var positions = new List<Vector3>
        {
            new Vector3(5, 5, 5),
            new Vector3(5, 5, 6),
            new Vector3(5, 5, 7)
        };
        
        // Act - Apply erosion then deposition
        geoAdapter.ApplyErosion(positions, 1.0f); // Complete erosion (convert to air)
        
        var sandMaterial = new MaterialData(MaterialId.Sand, 1600f, 2.5f);
        geoAdapter.ApplyDeposition(positions, sandMaterial);
        
        // Assert - Should have sand after both processes
        foreach (var pos in positions)
        {
            var material = geoAdapter.GetMaterial(pos);
            Assert.AreEqual(MaterialId.Sand, material.MaterialType,
                "Sequential processes should result in sand deposition");
        }
    }

    [TestMethod]
    public void Integration_VolcanicIntrusionWithConsolidation_PersistsChanges()
    {
        // Arrange
        var baseTree = new OptimizedOctreeNode
        {
            ExplicitMaterial = MaterialData.DefaultOcean
        };
        var deltaOctree = new DeltaPatchOctree(baseTree, consolidationThreshold: 10);
        var geoAdapter = new GeomorphologicalOctreeAdapter(deltaOctree);
        
        // Act - Apply volcanic intrusion
        var center = new Vector3(0, 0, 0);
        var lavaMaterial = new MaterialData(MaterialId.Lava, 2800f, 3.0f);
        geoAdapter.ApplyVolcanicIntrusion(center, 3f, lavaMaterial);
        
        int deltasBeforeConsolidation = deltaOctree.ActiveDeltaCount;
        
        // Force consolidation
        geoAdapter.ConsolidateChanges();
        
        // Assert - Changes should persist even after consolidation
        Assert.IsTrue(deltasBeforeConsolidation > 0, 
            "Should have created deltas for volcanic intrusion");
        Assert.AreEqual(0, deltaOctree.ActiveDeltaCount,
            "Deltas should be consolidated");
        
        // Verify material is still accessible
        var materialAtCenter = geoAdapter.GetMaterial(center);
        Assert.AreEqual(MaterialId.Lava, materialAtCenter.MaterialType,
            "Volcanic material should persist after consolidation");
    }

    [TestMethod]
    public void Integration_TectonicDeformation_MovesLargeMassCorrectly()
    {
        // Arrange
        var baseTree = new OptimizedOctreeNode
        {
            ExplicitMaterial = MaterialData.DefaultOcean
        };
        var deltaOctree = new DeltaPatchOctree(baseTree);
        var geoAdapter = new GeomorphologicalOctreeAdapter(deltaOctree);
        
        // Create a column of rock
        var columnPositions = new List<Vector3>();
        for (int y = 0; y < 5; y++)
        {
            var pos = new Vector3(0, y, 0);
            columnPositions.Add(pos);
            geoAdapter.SetMaterial(pos, new MaterialData(MaterialId.Granite, 2700f, 7.0f));
        }
        
        // Act - Apply tectonic deformation to move the column
        var displacement = new Vector3(5, 0, 0);
        geoAdapter.ApplyTectonicDeformation(columnPositions, displacement);
        
        // Assert - Source should be air, destination should have granite
        foreach (var sourcePos in columnPositions)
        {
            var destPos = sourcePos + displacement;
            
            var sourceMaterial = geoAdapter.GetMaterial(sourcePos);
            var destMaterial = geoAdapter.GetMaterial(destPos);
            
            Assert.AreEqual(MaterialId.Air, sourceMaterial.MaterialType,
                $"Source position {sourcePos} should be cleared to air");
            Assert.AreEqual(MaterialId.Granite, destMaterial.MaterialType,
                $"Destination position {destPos} should have displaced granite");
        }
    }

    [TestMethod]
    public void Integration_QueryRegion_ReturnsConsistentData()
    {
        // Arrange
        var baseTree = new OptimizedOctreeNode
        {
            ExplicitMaterial = MaterialData.DefaultOcean
        };
        var deltaOctree = new DeltaPatchOctree(baseTree);
        var geoAdapter = new GeomorphologicalOctreeAdapter(deltaOctree);
        
        // Create a 5x5x5 cube of rock
        var rockMaterial = new MaterialData(MaterialId.Rock, 2700f, 6.0f);
        for (int x = 0; x < 5; x++)
        {
            for (int y = 0; y < 5; y++)
            {
                for (int z = 0; z < 5; z++)
                {
                    geoAdapter.SetMaterial(new Vector3(x, y, z), rockMaterial);
                }
            }
        }
        
        // Act - Query the region
        var region = geoAdapter.QueryRegion(new Vector3(0, 0, 0), new Vector3(4, 4, 4));
        
        // Assert
        Assert.AreEqual(125, region.Count, "Should return 5x5x5 = 125 voxels");
        Assert.IsTrue(region.All(kvp => kvp.Value.MaterialType == MaterialId.Rock),
            "All queried materials should be rock");
    }

    #endregion

    #region Layer Boundary Tests

    [TestMethod]
    public void LayerBoundary_DeltaOctreeReadFallback_UsesBaseTreeCorrectly()
    {
        // Arrange - Base tree with rock, no deltas
        var baseTree = new OptimizedOctreeNode
        {
            ExplicitMaterial = new MaterialData(MaterialId.Rock, 2700f, 6.0f)
        };
        var deltaOctree = new DeltaPatchOctree(baseTree);
        
        // Act - Read position without delta
        var position = new Vector3(100, 100, 100);
        var material = deltaOctree.ReadVoxel(position);
        
        // Assert - Should get material from base tree
        Assert.AreEqual(MaterialId.Rock, material.MaterialType,
            "Should fall back to base tree material when no delta exists");
    }

    [TestMethod]
    public void LayerBoundary_DeltaOverridesBase_HasCorrectPrecedence()
    {
        // Arrange
        var baseTree = new OptimizedOctreeNode
        {
            ExplicitMaterial = new MaterialData(MaterialId.Rock, 2700f, 6.0f)
        };
        var deltaOctree = new DeltaPatchOctree(baseTree);
        
        // Act - Override base material with delta
        var position = new Vector3(0, 0, 0);
        var sandMaterial = new MaterialData(MaterialId.Sand, 1600f, 2.5f);
        deltaOctree.WriteVoxel(position, sandMaterial);
        
        var material = deltaOctree.ReadVoxel(position);
        
        // Assert - Delta should override base
        Assert.AreEqual(MaterialId.Sand, material.MaterialType,
            "Delta should take precedence over base tree material");
    }

    [TestMethod]
    public void LayerBoundary_GeoAdapterToDeltas_PreservesDataIntegrity()
    {
        // Arrange
        var baseTree = new OptimizedOctreeNode
        {
            ExplicitMaterial = MaterialData.DefaultOcean
        };
        var deltaOctree = new DeltaPatchOctree(baseTree);
        var geoAdapter = new GeomorphologicalOctreeAdapter(deltaOctree);
        
        // Act - Write through geo adapter
        var position = new Vector3(5, 5, 5);
        var material = new MaterialData(MaterialId.Granite, 2700f, 7.0f);
        geoAdapter.SetMaterial(position, material);
        
        // Read directly from delta octree
        var deltaReadMaterial = deltaOctree.ReadVoxel(position);
        
        // Read through geo adapter
        var geoReadMaterial = geoAdapter.GetMaterial(position);
        
        // Assert - Both should return same material
        Assert.AreEqual(material, deltaReadMaterial,
            "Reading directly from delta octree should return written material");
        Assert.AreEqual(material, geoReadMaterial,
            "Reading through geo adapter should return same material");
        Assert.AreEqual(deltaReadMaterial, geoReadMaterial,
            "Both read paths should return identical materials");
    }

    #endregion

    #region Performance Integration Tests

    [TestMethod]
    public void Performance_LargeScaleErosion_HandlesEfficiently()
    {
        // Arrange
        var baseTree = new OptimizedOctreeNode
        {
            ExplicitMaterial = new MaterialData(MaterialId.Rock, 2700f, 6.0f)
        };
        var deltaOctree = new DeltaPatchOctree(baseTree);
        var geoAdapter = new GeomorphologicalOctreeAdapter(deltaOctree);
        
        // Act - Apply erosion to large area
        var positions = new List<Vector3>();
        for (int x = 0; x < 20; x++)
        {
            for (int z = 0; z < 20; z++)
            {
                positions.Add(new Vector3(x, 10, z));
            }
        }
        
        var stopwatch = System.Diagnostics.Stopwatch.StartNew();
        geoAdapter.ApplyErosion(positions, 0.5f);
        stopwatch.Stop();
        
        // Assert - Should complete in reasonable time
        Assert.IsTrue(stopwatch.ElapsedMilliseconds < 1000,
            $"Large scale erosion (400 positions) should complete in <1s, took {stopwatch.ElapsedMilliseconds}ms");
        Assert.AreEqual(400, deltaOctree.ActiveDeltaCount,
            "Should create delta for each eroded position");
    }

    [TestMethod]
    public void Performance_DeltaConsolidation_MaintainsDataIntegrity()
    {
        // Arrange
        var baseTree = new OptimizedOctreeNode
        {
            ExplicitMaterial = MaterialData.DefaultOcean
        };
        var deltaOctree = new DeltaPatchOctree(baseTree, consolidationThreshold: 50);
        var geoAdapter = new GeomorphologicalOctreeAdapter(deltaOctree);
        
        // Create test data
        var testPositions = new Dictionary<Vector3, MaterialData>();
        for (int i = 0; i < 100; i++)
        {
            var pos = new Vector3(i, 0, 0);
            var material = new MaterialData(MaterialId.Sand, 1600f + i, 2.5f);
            testPositions[pos] = material;
            geoAdapter.SetMaterial(pos, material);
        }
        
        // Act - Force consolidation
        geoAdapter.ConsolidateChanges();
        
        // Assert - All data should still be accessible with correct values
        foreach (var kvp in testPositions)
        {
            var retrievedMaterial = geoAdapter.GetMaterial(kvp.Key);
            Assert.AreEqual(kvp.Value, retrievedMaterial,
                $"Material at {kvp.Key} should be preserved after consolidation");
        }
    }

    #endregion
}
