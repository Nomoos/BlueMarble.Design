using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Numerics;
using BlueMarble.SpatialData;

namespace BlueMarble.SpatialData.Tests;

/// <summary>
/// Comprehensive test suite for Delta Overlay System
/// Validates core functionality, performance, and geological integration
/// </summary>
[TestClass]
public class DeltaOverlaySystemTests
{
    private OptimizedOctreeNode _baseTree = null!;
    private DeltaPatchOctree _deltaOctree = null!;
    private GeomorphologicalOctreeAdapter _geoAdapter = null!;

    [TestInitialize]
    public void Setup()
    {
        // Initialize base octree with default ocean material
        _baseTree = new OptimizedOctreeNode
        {
            ExplicitMaterial = MaterialData.DefaultOcean
        };

        _deltaOctree = new DeltaPatchOctree(_baseTree, consolidationThreshold: 1000);
        _geoAdapter = new GeomorphologicalOctreeAdapter(_deltaOctree);
    }

    #region Test 1: Basic Read/Write Operations

    [TestMethod]
    public void Test1_ReadWriteOperations_SingleVoxel_CorrectlyStoresAndRetrievesDelta()
    {
        // Arrange
        var position = new Vector3(10, 20, 30);
        var sandMaterial = new MaterialData(MaterialId.Sand, 1600f, 2.5f);

        // Act - Write material
        _deltaOctree.WriteVoxel(position, sandMaterial);

        // Assert - Read material back
        var retrievedMaterial = _deltaOctree.ReadVoxel(position);
        Assert.AreEqual(sandMaterial, retrievedMaterial, "Material should be stored and retrieved correctly");
        Assert.IsTrue(_deltaOctree.HasDelta(position), "Delta should exist for modified position");
    }

    [TestMethod]
    public void Test1_ReadWriteOperations_RevertToOriginal_RemovesDelta()
    {
        // Arrange
        var position = new Vector3(5, 5, 5);
        var originalMaterial = MaterialData.DefaultOcean;
        var tempMaterial = new MaterialData(MaterialId.Sand, 1600f, 2.5f);

        // Act - Modify then revert
        _deltaOctree.WriteVoxel(position, tempMaterial);
        Assert.IsTrue(_deltaOctree.HasDelta(position), "Delta should exist after modification");

        _deltaOctree.WriteVoxel(position, originalMaterial);

        // Assert - Delta should be removed
        Assert.IsFalse(_deltaOctree.HasDelta(position), "Delta should be removed when reverting to original");
        var retrievedMaterial = _deltaOctree.ReadVoxel(position);
        Assert.AreEqual(originalMaterial, retrievedMaterial, "Material should revert to original");
    }

    [TestMethod]
    public void Test1_ReadWriteOperations_MultiplePositions_IndependentDeltas()
    {
        // Arrange
        var positions = new[]
        {
            (new Vector3(1, 1, 1), new MaterialData(MaterialId.Sand, 1600f, 2.5f)),
            (new Vector3(2, 2, 2), new MaterialData(MaterialId.Rock, 2700f, 6.0f)),
            (new Vector3(3, 3, 3), new MaterialData(MaterialId.Clay, 1800f, 2.0f))
        };

        // Act - Write multiple materials
        foreach (var (pos, mat) in positions)
        {
            _deltaOctree.WriteVoxel(pos, mat);
        }

        // Assert - All materials stored independently
        foreach (var (pos, expectedMat) in positions)
        {
            var actualMat = _deltaOctree.ReadVoxel(pos);
            Assert.AreEqual(expectedMat, actualMat, $"Material at {pos} should match");
            Assert.IsTrue(_deltaOctree.HasDelta(pos), $"Delta should exist at {pos}");
        }
    }

    #endregion

    #region Test 2: Batch Operations

    [TestMethod]
    public void Test2_BatchOperations_LargeBatch_EfficientProcessing()
    {
        // Arrange
        var batchSize = 500;
        var updates = GenerateSparseUpdates(batchSize);

        // Act - Batch update
        var startTime = DateTime.UtcNow;
        _deltaOctree.WriteMaterialBatch(updates);
        var elapsed = (DateTime.UtcNow - startTime).TotalMilliseconds;

        // Assert - All updates applied
        Assert.AreEqual(batchSize, _deltaOctree.ActiveDeltaCount, "All deltas should be stored");

        // Verify performance target: <1ms per update on average
        var avgTimePerUpdate = elapsed / batchSize;
        Console.WriteLine($"Batch processing: {elapsed:F2}ms total, {avgTimePerUpdate:F4}ms per update");
        Assert.IsTrue(avgTimePerUpdate < 1.0, $"Average update time should be <1ms, was {avgTimePerUpdate:F4}ms");

        // Verify all updates can be read back correctly
        foreach (var (position, material) in updates)
        {
            var retrieved = _deltaOctree.ReadVoxel(position);
            Assert.AreEqual(material, retrieved, $"Material at {position} should match batch update");
        }
    }

    [TestMethod]
    public void Test2_BatchOperations_SpatialLocality_PreservesGrouping()
    {
        // Arrange - Create spatially clustered updates (erosion pattern)
        var clusterCenter = new Vector3(100, 100, 100);
        var clusterRadius = 10f;
        var updates = new List<(Vector3, MaterialData)>();

        for (int i = 0; i < 100; i++)
        {
            var offset = new Vector3(
                (float)(Random.Shared.NextDouble() * clusterRadius * 2 - clusterRadius),
                (float)(Random.Shared.NextDouble() * clusterRadius * 2 - clusterRadius),
                (float)(Random.Shared.NextDouble() * clusterRadius * 2 - clusterRadius)
            );
            var position = clusterCenter + offset;
            updates.Add((position, new MaterialData(MaterialId.Sand, 1600f, 2.5f)));
        }

        // Act
        _deltaOctree.WriteMaterialBatch(updates);

        // Assert - All updates stored
        Assert.AreEqual(updates.Count, _deltaOctree.ActiveDeltaCount);

        // Verify spatial locality is preserved
        foreach (var (position, material) in updates)
        {
            var retrieved = _deltaOctree.ReadVoxel(position);
            Assert.AreEqual(material, retrieved);
        }
    }

    #endregion

    #region Test 3: Consolidation Behavior

    [TestMethod]
    public void Test3_Consolidation_ThresholdTrigger_AutomaticallyConsolidates()
    {
        // Arrange - Create octree with low threshold
        var lowThresholdOctree = new DeltaPatchOctree(_baseTree, consolidationThreshold: 50);

        // Act - Add updates beyond threshold
        for (int i = 0; i < 100; i++)
        {
            var position = new Vector3(i, i, i);
            var material = new MaterialData(MaterialId.Sand, 1600f, 2.5f);
            lowThresholdOctree.WriteVoxel(position, material);
        }

        // Assert - Delta count should be reduced due to automatic consolidation
        Assert.IsTrue(lowThresholdOctree.ActiveDeltaCount < 100,
            $"Delta count should be reduced by consolidation, was {lowThresholdOctree.ActiveDeltaCount}");

        // Verify materials are still accessible (either from deltas or consolidated into tree)
        for (int i = 0; i < 100; i++)
        {
            var position = new Vector3(i, i, i);
            var material = lowThresholdOctree.ReadVoxel(position);
            Assert.AreEqual(MaterialId.Sand, material.MaterialType,
                $"Material at {position} should still be accessible after consolidation");
        }
    }

    [TestMethod]
    public void Test3_Consolidation_ManualTrigger_ClearsDeltas()
    {
        // Arrange - Use small coordinate space for consolidation testing
        var updates = new List<(Vector3, MaterialData)>();
        for (int i = 0; i < 50; i++)
        {
            var position = new Vector3(i, 0, 0);
            var material = new MaterialData(MaterialId.Sand, 1600f, 2.5f);
            updates.Add((position, material));
        }
        
        _deltaOctree.WriteMaterialBatch(updates);
        var initialDeltaCount = _deltaOctree.ActiveDeltaCount;

        Assert.AreEqual(50, initialDeltaCount, "Should have 50 deltas before consolidation");

        // Act - Manual consolidation
        _deltaOctree.ConsolidateDeltas();

        // Assert - Deltas cleared
        Assert.AreEqual(0, _deltaOctree.ActiveDeltaCount, "All deltas should be consolidated");

        // Verify materials still accessible from base tree
        foreach (var (position, expectedMaterial) in updates)
        {
            var material = _deltaOctree.ReadVoxel(position);
            Assert.AreEqual(expectedMaterial, material,
                $"Material at {position} should be accessible from consolidated tree");
        }
    }

    [TestMethod]
    public void Test3_Consolidation_PreservesData_NoDataLoss()
    {
        // Arrange - Create simple pattern in small coordinate space
        var updates = new List<(Vector3, MaterialData)>();
        for (int x = 0; x < 5; x++)
        {
            for (int y = 0; y < 5; y++)
            {
                var position = new Vector3(x, y, 0);
                // Alternate between two distinct materials with different densities
                MaterialData material;
                if ((x + y) % 2 == 0)
                {
                    material = new MaterialData(MaterialId.Sand, 1600f, 2.5f);
                }
                else
                {
                    material = new MaterialData(MaterialId.Rock, 2700f, 6.0f);
                }
                updates.Add((position, material));
            }
        }

        _deltaOctree.WriteMaterialBatch(updates);

        // Act - Consolidate
        _deltaOctree.ConsolidateDeltas();

        // Assert - Verify all materials preserved
        foreach (var (position, expectedMaterial) in updates)
        {
            var actualMaterial = _deltaOctree.ReadVoxel(position);
            Assert.AreEqual(expectedMaterial, actualMaterial,
                $"Material at {position} should be preserved after consolidation");
        }
    }

    #endregion

    #region Test 4: Geological Process Integration

    [TestMethod]
    public void Test4_GeologicalProcesses_Erosion_RemovesMaterial()
    {
        // Arrange - Create surface with rock
        var surfacePositions = new List<Vector3>();
        for (int x = 0; x < 5; x++)
        {
            for (int z = 0; z < 5; z++)
            {
                var position = new Vector3(x, 10, z);
                surfacePositions.Add(position);
                _deltaOctree.WriteVoxel(position, new MaterialData(MaterialId.Rock, 2700f, 6.0f));
            }
        }

        // Act - Apply erosion
        _geoAdapter.ApplyErosion(surfacePositions, erosionRate: 1.0f);

        // Assert - Material converted to air
        foreach (var position in surfacePositions)
        {
            var material = _geoAdapter.GetMaterial(position);
            Assert.AreEqual(MaterialId.Air, material.MaterialType,
                $"Eroded position {position} should be air");
        }
    }

    [TestMethod]
    public void Test4_GeologicalProcesses_Deposition_AddsMaterial()
    {
        // Arrange - Empty space (air)
        var depositionSites = new List<(Vector3, MaterialData)>();
        var sediment = new MaterialData(MaterialId.Sand, 1600f, 2.5f);

        for (int x = 0; x < 5; x++)
        {
            for (int z = 0; z < 5; z++)
            {
                var position = new Vector3(x, 5, z);
                depositionSites.Add((position, sediment));
            }
        }

        // Act - Apply deposition
        _geoAdapter.ApplyDeposition(depositionSites);

        // Assert - Material deposited
        foreach (var (position, expectedMaterial) in depositionSites)
        {
            var material = _geoAdapter.GetMaterial(position);
            Assert.AreEqual(expectedMaterial, material,
                $"Deposition at {position} should place sediment");
        }
    }

    [TestMethod]
    public void Test4_GeologicalProcesses_VolcanicIntrusion_CreatesVolume()
    {
        // Arrange
        var center = new Vector3(50, 50, 50);
        var radius = 5f;
        var lava = new MaterialData(MaterialId.Lava, 3100f, 2.0f);

        // Act - Apply volcanic intrusion
        _geoAdapter.ApplyVolcanicIntrusion(center, radius, lava);

        // Assert - Volume filled with lava
        int lavaVoxelCount = 0;
        for (float x = center.X - radius; x <= center.X + radius; x++)
        {
            for (float y = center.Y - radius; y <= center.Y + radius; y++)
            {
                for (float z = center.Z - radius; z <= center.Z + radius; z++)
                {
                    var position = new Vector3(x, y, z);
                    if (Vector3.Distance(position, center) <= radius)
                    {
                        var material = _geoAdapter.GetMaterial(position);
                        if (material.MaterialType == MaterialId.Lava)
                        {
                            lavaVoxelCount++;
                        }
                    }
                }
            }
        }

        Assert.IsTrue(lavaVoxelCount > 0, "Volcanic intrusion should create lava voxels");
        Console.WriteLine($"Volcanic intrusion created {lavaVoxelCount} lava voxels");
    }

    [TestMethod]
    public void Test4_GeologicalProcesses_TectonicDeformation_DisplacesMaterial()
    {
        // Arrange - Place material at source positions
        var sourcePositions = new List<Vector3>
        {
            new Vector3(10, 10, 10),
            new Vector3(11, 10, 10),
            new Vector3(12, 10, 10)
        };

        var granite = new MaterialData(MaterialId.Granite, 2750f, 7.0f);
        foreach (var pos in sourcePositions)
        {
            _deltaOctree.WriteVoxel(pos, granite);
        }

        // Act - Tectonic displacement
        var displacements = sourcePositions.Select(pos => (pos, pos + new Vector3(0, 5, 0))).ToList();
        _geoAdapter.ApplyTectonicDeformation(displacements);

        // Assert - Material moved to new positions
        foreach (var (from, to) in displacements)
        {
            var fromMaterial = _geoAdapter.GetMaterial(from);
            var toMaterial = _geoAdapter.GetMaterial(to);

            Assert.AreEqual(MaterialId.Air, fromMaterial.MaterialType,
                $"Source position {from} should be cleared to air");
            Assert.AreEqual(MaterialId.Granite, toMaterial.MaterialType,
                $"Destination position {to} should have displaced material");
        }
    }

    [TestMethod]
    public void Test4_GeologicalProcesses_RegionQuery_ReturnsCorrectData()
    {
        // Arrange - Create 3x3x3 cube of material
        var minBounds = new Vector3(0, 0, 0);
        var maxBounds = new Vector3(2, 2, 2);

        for (float x = minBounds.X; x <= maxBounds.X; x++)
        {
            for (float y = minBounds.Y; y <= maxBounds.Y; y++)
            {
                for (float z = minBounds.Z; z <= maxBounds.Z; z++)
                {
                    var position = new Vector3(x, y, z);
                    _deltaOctree.WriteVoxel(position, new MaterialData(MaterialId.Rock, 2700f, 6.0f));
                }
            }
        }

        // Act - Query region
        var results = _geoAdapter.QueryRegion(minBounds, maxBounds).ToList();

        // Assert - Correct number of voxels and all rock
        Assert.AreEqual(27, results.Count, "Should return 3x3x3 = 27 voxels");
        Assert.IsTrue(results.All(r => r.material.MaterialType == MaterialId.Rock),
            "All queried materials should be rock");
    }

    #endregion

    #region Test 5: Edge Cases and Performance

    [TestMethod]
    public void Test5_EdgeCases_SamePositionMultipleWrites_LastWriteWins()
    {
        // Arrange
        var position = new Vector3(7, 7, 7);
        var material1 = new MaterialData(MaterialId.Sand, 1600f, 2.5f);
        var material2 = new MaterialData(MaterialId.Rock, 2700f, 6.0f);
        var material3 = new MaterialData(MaterialId.Clay, 1800f, 2.0f);

        // Act - Multiple writes to same position
        _deltaOctree.WriteVoxel(position, material1);
        _deltaOctree.WriteVoxel(position, material2);
        _deltaOctree.WriteVoxel(position, material3);

        // Assert - Last write wins, only one delta
        Assert.AreEqual(1, _deltaOctree.ActiveDeltaCount, "Should only have one delta for position");
        var finalMaterial = _deltaOctree.ReadVoxel(position);
        Assert.AreEqual(material3, finalMaterial, "Last written material should be retrieved");
    }

    [TestMethod]
    public void Test5_EdgeCases_NegativeCoordinates_HandledCorrectly()
    {
        // Arrange
        var negativePositions = new[]
        {
            new Vector3(-10, -10, -10),
            new Vector3(-5, 0, 5),
            new Vector3(5, -5, 0)
        };

        var material = new MaterialData(MaterialId.Rock, 2700f, 6.0f);

        // Act
        foreach (var position in negativePositions)
        {
            _deltaOctree.WriteVoxel(position, material);
        }

        // Assert
        foreach (var position in negativePositions)
        {
            var retrieved = _deltaOctree.ReadVoxel(position);
            Assert.AreEqual(material, retrieved,
                $"Negative coordinate {position} should be handled correctly");
        }
    }

    [TestMethod]
    public void Test5_EdgeCases_LargeCoordinates_HandledCorrectly()
    {
        // Arrange
        var largePositions = new[]
        {
            new Vector3(10000, 10000, 10000),
            new Vector3(50000, 50000, 50000)
        };

        var material = new MaterialData(MaterialId.Rock, 2700f, 6.0f);

        // Act
        foreach (var position in largePositions)
        {
            _deltaOctree.WriteVoxel(position, material);
        }

        // Assert
        foreach (var position in largePositions)
        {
            var retrieved = _deltaOctree.ReadVoxel(position);
            Assert.AreEqual(material, retrieved,
                $"Large coordinate {position} should be handled correctly");
        }
    }

    [TestMethod]
    public void Test5_Performance_SparseUpdateSpeed_MeetsTarget()
    {
        // Arrange - 1000 sparse updates
        var updates = GenerateSparseUpdates(1000);

        // Act - Measure update time
        var startTime = DateTime.UtcNow;

        foreach (var (position, material) in updates)
        {
            _deltaOctree.WriteVoxel(position, material);
        }

        var elapsed = (DateTime.UtcNow - startTime).TotalMilliseconds;
        var avgTime = elapsed / 1000.0;

        // Assert - Should be <1ms per update on average
        Console.WriteLine($"Performance: {elapsed:F2}ms for 1000 updates, {avgTime:F4}ms per update");
        Assert.IsTrue(avgTime < 1.0, $"Average update time should be <1ms, was {avgTime:F4}ms");
    }

    [TestMethod]
    public void Test5_Performance_DeltaCountTracking_Accurate()
    {
        // Arrange
        var updateCount = 100;
        var updates = GenerateSparseUpdates(updateCount);

        // Act
        _deltaOctree.WriteMaterialBatch(updates);

        // Assert
        Assert.AreEqual(updateCount, _deltaOctree.ActiveDeltaCount,
            "Active delta count should match number of updates");

        // Consolidate and verify count goes to zero
        _deltaOctree.ConsolidateDeltas();
        Assert.AreEqual(0, _deltaOctree.ActiveDeltaCount,
            "Active delta count should be zero after consolidation");
    }

    #endregion

    #region Helper Methods

    private List<(Vector3, MaterialData)> GenerateSparseUpdates(int count)
    {
        var updates = new List<(Vector3, MaterialData)>();
        var usedPositions = new HashSet<Vector3>();
        var materials = new[]
        {
            new MaterialData(MaterialId.Sand, 1600f, 2.5f),
            new MaterialData(MaterialId.Rock, 2700f, 6.0f),
            new MaterialData(MaterialId.Clay, 1800f, 2.0f),
            new MaterialData(MaterialId.Granite, 2750f, 7.0f)
        };

        int attempts = 0;
        while (updates.Count < count && attempts < count * 10)
        {
            attempts++;
            var position = new Vector3(
                Random.Shared.Next(-1000, 1000),
                Random.Shared.Next(-1000, 1000),
                Random.Shared.Next(-1000, 1000)
            );

            // Ensure unique positions
            if (!usedPositions.Add(position))
                continue;

            var material = materials[Random.Shared.Next(materials.Length)];
            updates.Add((position, material));
        }

        return updates;
    }

    #endregion
}
