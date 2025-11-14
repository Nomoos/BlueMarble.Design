using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Numerics;
using BlueMarble.SpatialData;
using BlueMarble.SpatialData.Interfaces;
using BlueMarble.SpatialData.Tests.Mocks;

namespace BlueMarble.SpatialData.Tests;

/// <summary>
/// Unit tests for isolated layer testing with mocks
/// Demonstrates testing each layer in isolation with controlled dependencies
/// </summary>
[TestClass]
public class LayerIsolationTests
{
    #region DeltaPatchOctree Layer Tests (with mocked IOctreeStorage)

    [TestMethod]
    public void DeltaLayer_WriteVoxel_UsesOctreeStorageForBaseMaterial()
    {
        // Arrange - Create mock octree storage
        var mockOctree = new MockOctreeStorage(MaterialData.DefaultOcean);
        var deltaOctree = new DeltaPatchOctree(mockOctree);
        
        var position = new Vector3(10, 20, 30);
        var newMaterial = new MaterialData(MaterialId.Sand, 1600f, 2.5f);
        
        // Act - Write new material
        deltaOctree.WriteVoxel(position, newMaterial);
        
        // Assert - Verify mock was called to get base material
        Assert.AreEqual(1, mockOctree.GetMaterialAtCallCount, 
            "Should query octree storage once for base material");
        Assert.IsTrue(deltaOctree.HasDelta(position), 
            "Delta should be stored for modified position");
    }

    [TestMethod]
    public void DeltaLayer_ReadVoxel_FallsBackToOctreeStorage()
    {
        // Arrange
        var mockOctree = new MockOctreeStorage(MaterialData.DefaultOcean);
        var position = new Vector3(5, 5, 5);
        var expectedMaterial = new MaterialData(MaterialId.Rock, 2700f, 6.0f);
        mockOctree.SetMaterialAt(position, expectedMaterial);
        
        var deltaOctree = new DeltaPatchOctree(mockOctree);
        
        // Act - Read position without delta
        var result = deltaOctree.ReadVoxel(position);
        
        // Assert - Should get material from mock octree storage
        Assert.AreEqual(expectedMaterial, result, 
            "Should retrieve material from octree storage when no delta exists");
        Assert.AreEqual(1, mockOctree.GetMaterialAtCallCount, 
            "Should query octree storage once");
    }

    [TestMethod]
    public void DeltaLayer_ConsolidateDeltas_WritesToOctreeStorage()
    {
        // Arrange
        var mockOctree = new MockOctreeStorage(MaterialData.DefaultOcean);
        var deltaOctree = new DeltaPatchOctree(mockOctree);
        
        // Write several deltas
        var positions = new[]
        {
            new Vector3(1, 1, 1),
            new Vector3(2, 2, 2),
            new Vector3(3, 3, 3)
        };
        var material = new MaterialData(MaterialId.Sand, 1600f, 2.5f);
        
        foreach (var pos in positions)
        {
            deltaOctree.WriteVoxel(pos, material);
        }
        
        int setCallsBefore = mockOctree.SetMaterialAtCallCount;
        
        // Act - Consolidate deltas
        deltaOctree.ConsolidateDeltas();
        
        // Assert - Should write to octree storage
        Assert.AreEqual(positions.Length, mockOctree.SetMaterialAtCallCount - setCallsBefore,
            "Should call SetMaterialAt for each delta during consolidation");
        Assert.AreEqual(0, deltaOctree.ActiveDeltaCount,
            "All deltas should be cleared after consolidation");
    }

    [TestMethod]
    public void DeltaLayer_BatchWrite_OptimizesMultipleUpdates()
    {
        // Arrange
        var mockOctree = new MockOctreeStorage(MaterialData.DefaultOcean);
        var deltaOctree = new DeltaPatchOctree(mockOctree);
        
        var updates = new List<(Vector3, MaterialData)>
        {
            (new Vector3(1, 1, 1), new MaterialData(MaterialId.Sand, 1600f, 2.5f)),
            (new Vector3(2, 2, 2), new MaterialData(MaterialId.Rock, 2700f, 6.0f)),
            (new Vector3(3, 3, 3), new MaterialData(MaterialId.Clay, 1800f, 2.0f))
        };
        
        // Act - Batch write
        deltaOctree.WriteMaterialBatch(updates);
        
        // Assert - All deltas should be stored
        Assert.AreEqual(updates.Count, deltaOctree.ActiveDeltaCount,
            "All batch updates should create deltas");
        Assert.IsTrue(mockOctree.GetMaterialAtCallCount >= updates.Count,
            "Should query base material for each update");
    }

    #endregion

    #region GeomorphologicalOctreeAdapter Layer Tests (with mocked IDeltaStorage)

    [TestMethod]
    public void GeoAdapter_ApplyErosion_UsesDeltaStorageForReadsAndWrites()
    {
        // Arrange - Create mock delta storage
        var mockDelta = new MockDeltaStorage(MaterialData.DefaultOcean);
        var geoAdapter = new GeomorphologicalOctreeAdapter(mockDelta);
        
        // Set up some rock positions
        var positions = new List<Vector3>
        {
            new Vector3(10, 10, 10),
            new Vector3(11, 10, 10),
            new Vector3(12, 10, 10)
        };
        
        var rockMaterial = new MaterialData(MaterialId.Rock, 2700f, 6.0f);
        foreach (var pos in positions)
        {
            mockDelta.WriteVoxel(pos, rockMaterial);
        }
        
        mockDelta.Clear(); // Reset call counts
        
        // Act - Apply erosion
        geoAdapter.ApplyErosion(positions, 0.5f);
        
        // Assert - Should read each position and write batch
        Assert.IsTrue(mockDelta.ReadVoxelCallCount >= positions.Count,
            "Should read voxel for each eroded position");
        Assert.AreEqual(1, mockDelta.WriteMaterialBatchCallCount,
            "Should use batch write for erosion updates");
    }

    [TestMethod]
    public void GeoAdapter_ApplyDeposition_BatchesUpdates()
    {
        // Arrange
        var mockDelta = new MockDeltaStorage(MaterialData.DefaultAir);
        var geoAdapter = new GeomorphologicalOctreeAdapter(mockDelta);
        
        var positions = new List<Vector3>();
        for (int i = 0; i < 10; i++)
        {
            positions.Add(new Vector3(i, 0, 0));
        }
        
        var sediment = new MaterialData(MaterialId.Soil, 1500f, 2.0f);
        
        // Act - Apply deposition
        geoAdapter.ApplyDeposition(positions, sediment);
        
        // Assert - Should use batch write
        Assert.AreEqual(1, mockDelta.WriteMaterialBatchCallCount,
            "Should use single batch write for deposition");
        Assert.IsTrue(mockDelta.ReadVoxelCallCount >= positions.Count,
            "Should read each position to check current material");
    }

    [TestMethod]
    public void GeoAdapter_ApplyVolcanicIntrusion_CreatesSphericalVolume()
    {
        // Arrange
        var mockDelta = new MockDeltaStorage(MaterialData.DefaultOcean);
        var geoAdapter = new GeomorphologicalOctreeAdapter(mockDelta);
        
        var center = new Vector3(0, 0, 0);
        var radius = 3f;
        var lavaMaterial = new MaterialData(MaterialId.Lava, 2800f, 3.0f);
        
        // Act - Apply volcanic intrusion
        geoAdapter.ApplyVolcanicIntrusion(center, radius, lavaMaterial);
        
        // Assert - Should write a volume of voxels
        Assert.AreEqual(1, mockDelta.WriteMaterialBatchCallCount,
            "Should use batch write for volcanic intrusion");
        Assert.IsTrue(mockDelta.ActiveDeltaCount > 0,
            "Should have created deltas for volcanic material");
    }

    [TestMethod]
    public void GeoAdapter_ApplyTectonicDeformation_DisplacesMaterial()
    {
        // Arrange
        var mockDelta = new MockDeltaStorage(MaterialData.DefaultOcean);
        var geoAdapter = new GeomorphologicalOctreeAdapter(mockDelta);
        
        // Set up source positions with material
        var sourcePositions = new List<Vector3>
        {
            new Vector3(0, 0, 0),
            new Vector3(1, 0, 0),
            new Vector3(2, 0, 0)
        };
        
        var granite = new MaterialData(MaterialId.Granite, 2700f, 7.0f);
        foreach (var pos in sourcePositions)
        {
            mockDelta.WriteVoxel(pos, granite);
        }
        
        mockDelta.Clear(); // Reset call counts
        
        // Act - Apply tectonic deformation
        var displacement = new Vector3(0, 5, 0);
        geoAdapter.ApplyTectonicDeformation(sourcePositions, displacement);
        
        // Assert
        Assert.IsTrue(mockDelta.ReadVoxelCallCount >= sourcePositions.Count,
            "Should read material from each source position");
        Assert.AreEqual(1, mockDelta.WriteMaterialBatchCallCount,
            "Should use batch write for tectonic deformation");
    }

    [TestMethod]
    public void GeoAdapter_QueryRegion_ReadsMaterialsFromDeltaStorage()
    {
        // Arrange
        var mockDelta = new MockDeltaStorage(MaterialData.DefaultOcean);
        var geoAdapter = new GeomorphologicalOctreeAdapter(mockDelta);
        
        // Set up a small region with materials
        for (int x = 0; x < 3; x++)
        {
            for (int y = 0; y < 3; y++)
            {
                for (int z = 0; z < 3; z++)
                {
                    var pos = new Vector3(x, y, z);
                    mockDelta.WriteVoxel(pos, new MaterialData(MaterialId.Rock, 2700f, 6.0f));
                }
            }
        }
        
        mockDelta.Clear(); // Reset call counts
        
        // Act - Query region
        var result = geoAdapter.QueryRegion(new Vector3(0, 0, 0), new Vector3(2, 2, 2));
        
        // Assert
        Assert.AreEqual(27, result.Count, "Should return 3x3x3 = 27 voxels");
        Assert.AreEqual(27, mockDelta.ReadVoxelCallCount,
            "Should read each voxel in the region");
    }

    #endregion

    #region Error Handling Tests

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void GeoAdapter_WithFailingDeltaStorage_PropagatesException()
    {
        // Arrange - Mock that simulates failures
        var mockDelta = new MockDeltaStorage(MaterialData.DefaultOcean);
        mockDelta.SimulateReadFailure = true;
        var geoAdapter = new GeomorphologicalOctreeAdapter(mockDelta);
        
        // Act - This should throw due to simulated failure
        geoAdapter.ApplyErosion(new[] { new Vector3(0, 0, 0) }, 1.0f);
        
        // Assert - Exception expected
    }

    [TestMethod]
    public void DeltaLayer_HandlesEmptyBatch_Gracefully()
    {
        // Arrange
        var mockOctree = new MockOctreeStorage(MaterialData.DefaultOcean);
        var deltaOctree = new DeltaPatchOctree(mockOctree);
        
        // Act - Write empty batch
        deltaOctree.WriteMaterialBatch(new List<(Vector3, MaterialData)>());
        
        // Assert - Should handle gracefully
        Assert.AreEqual(0, deltaOctree.ActiveDeltaCount,
            "Should have no deltas after empty batch");
        Assert.AreEqual(0, mockOctree.SetMaterialAtCallCount,
            "Should not call octree storage for empty batch");
    }

    #endregion

    #region Interface Contract Tests

    [TestMethod]
    public void DeltaStorage_Interface_SatisfiesContract()
    {
        // Arrange
        IDeltaStorage deltaStorage = new DeltaPatchOctree(
            new MockOctreeStorage(MaterialData.DefaultOcean));
        
        var position = new Vector3(1, 2, 3);
        var material = new MaterialData(MaterialId.Sand, 1600f, 2.5f);
        
        // Act & Assert - Test interface contract
        deltaStorage.WriteVoxel(position, material);
        Assert.IsTrue(deltaStorage.HasDelta(position), 
            "Interface: HasDelta should return true after write");
        
        var retrieved = deltaStorage.ReadVoxel(position);
        Assert.AreEqual(material, retrieved, 
            "Interface: ReadVoxel should return written material");
        
        Assert.AreEqual(1, deltaStorage.ActiveDeltaCount,
            "Interface: ActiveDeltaCount should reflect stored deltas");
    }

    [TestMethod]
    public void GeologicalProcessor_Interface_SatisfiesContract()
    {
        // Arrange
        IGeologicalProcessor processor = new GeomorphologicalOctreeAdapter(
            new MockDeltaStorage(MaterialData.DefaultOcean));
        
        var positions = new List<Vector3> { new Vector3(0, 0, 0) };
        var material = new MaterialData(MaterialId.Sand, 1600f, 2.5f);
        
        // Act & Assert - Test interface contract
        processor.ApplyErosion(positions, 0.5f);
        processor.ApplyDeposition(positions, material);
        processor.ApplyVolcanicIntrusion(new Vector3(0, 0, 0), 5f, material);
        processor.ApplyTectonicDeformation(positions, new Vector3(0, 1, 0));
        
        var region = processor.QueryRegion(new Vector3(-1, -1, -1), new Vector3(1, 1, 1));
        Assert.IsNotNull(region, "Interface: QueryRegion should return valid result");
    }

    #endregion
}
