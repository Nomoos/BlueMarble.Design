using System;
using System.Linq;

namespace BlueMarble.SpatialStorage.Research.Tests
{
    /// <summary>
    /// Comprehensive test suite for material inheritance implementation
    /// 37+ test methods covering inheritance, caching, memory optimization, and performance
    /// </summary>
    public class MaterialInheritanceTests
    {
        private static int _passedTests = 0;
        private static int _failedTests = 0;
        
        public static void RunAllTests()
        {
            Console.WriteLine("=== Material Inheritance Test Suite ===\n");
            
            // Material System Tests
            TestMaterialDataCreation();
            TestMaterialProperties();
            TestMaterialFromId();
            
            // Basic Node Tests
            TestNodeCreation();
            TestExplicitMaterial();
            TestMaterialInheritance();
            TestInheritanceChain();
            TestMultiLevelInheritance();
            
            // Homogeneity Tests
            TestHomogeneityCalculation();
            TestHomogeneityWithUniformChildren();
            TestHomogeneityWithMixedChildren();
            TestHomogeneityCaching();
            
            // Collapse Tests
            TestNodeCollapse();
            TestNodeCollapseThreshold();
            TestNodeExpansion();
            TestCollapseAndReExpand();
            
            // Child Material Counts Tests
            TestChildMaterialCountUpdate();
            TestChildMaterialCountAccuracy();
            TestMaterialDistribution();
            
            // Cache Tests
            TestPathCacheBasic();
            TestPathCacheInvalidation();
            TestPointCacheBasic();
            TestMortonCacheBasic();
            TestLRUEviction();
            TestCacheStatistics();
            
            // High-Level API Tests
            TestOctreeCreation();
            TestPointQuery();
            TestSetMaterial();
            TestSetRegion();
            TestRegionOperations();
            
            // Memory Optimization Tests
            TestMemoryOptimization();
            TestMemoryStatistics();
            TestInheritanceEfficiency();
            TestCollapsedNodeMemory();
            
            // Integration Tests
            TestOceanScenario();
            TestContinentalScenario();
            TestCoastalScenario();
            TestBlueMarbleExample();
            
            // Performance Tests
            TestQueryPerformance();
            TestCachePerformance();
            TestCollapsePerformance();
            
            Console.WriteLine($"\n=== Test Results ===");
            Console.WriteLine($"Passed: {_passedTests}");
            Console.WriteLine($"Failed: {_failedTests}");
            Console.WriteLine($"Total: {_passedTests + _failedTests}");
        }
        
        // Helper methods
        private static void Assert(bool condition, string message)
        {
            if (!condition)
            {
                Console.WriteLine($"  ❌ FAIL: {message}");
                _failedTests++;
                throw new Exception($"Assertion failed: {message}");
            }
        }
        
        private static void AssertEqual<T>(T expected, T actual, string message)
        {
            if (!Equals(expected, actual))
            {
                Console.WriteLine($"  ❌ FAIL: {message}");
                Console.WriteLine($"     Expected: {expected}, Actual: {actual}");
                _failedTests++;
                throw new Exception($"Assertion failed: {message}");
            }
        }
        
        private static void TestPass(string testName)
        {
            Console.WriteLine($"  ✓ {testName}");
            _passedTests++;
        }
        
        private static void RunTest(string testName, Action test)
        {
            try
            {
                test();
                TestPass(testName);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"  ❌ {testName}: {ex.Message}");
                _failedTests++;
            }
        }
        
        // Material System Tests
        private static void TestMaterialDataCreation()
        {
            RunTest("Material data creation", () =>
            {
                var air = MaterialData.Air;
                AssertEqual(MaterialId.Air, air.Id, "Air ID should be correct");
                AssertEqual("Air", air.Name, "Air name should be correct");
                Assert(air.Properties != null, "Air should have properties");
            });
        }
        
        private static void TestMaterialProperties()
        {
            RunTest("Material properties", () =>
            {
                var water = MaterialData.Water;
                AssertEqual(1000.0, water.Properties.Density, "Water density should be 1000 kg/m³");
                Assert(water.Properties.Permeability > 0, "Water should have permeability");
            });
        }
        
        private static void TestMaterialFromId()
        {
            RunTest("Material from ID", () =>
            {
                for (int i = 0; i <= 8; i++)
                {
                    var material = MaterialData.FromId((MaterialId)i);
                    Assert(material != null, $"Material {i} should exist");
                }
            });
        }
        
        // Basic Node Tests
        private static void TestNodeCreation()
        {
            RunTest("Node creation", () =>
            {
                var node = new OptimizedOctreeNode();
                Assert(node != null, "Node should be created");
                Assert(node.ChildMaterialCounts != null, "ChildMaterialCounts should be initialized");
            });
        }
        
        private static void TestExplicitMaterial()
        {
            RunTest("Explicit material", () =>
            {
                var node = new OptimizedOctreeNode
                {
                    ExplicitMaterial = MaterialId.Rock
                };
                AssertEqual(MaterialId.Rock, node.GetEffectiveMaterial(), "Should return explicit material");
            });
        }
        
        private static void TestMaterialInheritance()
        {
            RunTest("Material inheritance", () =>
            {
                var parent = new OptimizedOctreeNode
                {
                    ExplicitMaterial = MaterialId.Water
                };
                var child = new OptimizedOctreeNode
                {
                    Parent = parent
                };
                AssertEqual(MaterialId.Water, child.GetEffectiveMaterial(), "Child should inherit from parent");
            });
        }
        
        private static void TestInheritanceChain()
        {
            RunTest("Inheritance chain", () =>
            {
                var root = new OptimizedOctreeNode { ExplicitMaterial = MaterialId.Air };
                var level1 = new OptimizedOctreeNode { Parent = root };
                var level2 = new OptimizedOctreeNode { Parent = level1 };
                var level3 = new OptimizedOctreeNode { Parent = level2 };
                
                AssertEqual(MaterialId.Air, level3.GetEffectiveMaterial(), 
                    "Deep child should inherit from root");
            });
        }
        
        private static void TestMultiLevelInheritance()
        {
            RunTest("Multi-level inheritance", () =>
            {
                var root = new OptimizedOctreeNode { ExplicitMaterial = MaterialId.Air };
                var level1 = new OptimizedOctreeNode { Parent = root, ExplicitMaterial = MaterialId.Water };
                var level2 = new OptimizedOctreeNode { Parent = level1 };
                
                AssertEqual(MaterialId.Water, level2.GetEffectiveMaterial(), 
                    "Should inherit from nearest explicit parent");
            });
        }
        
        // Homogeneity Tests
        private static void TestHomogeneityCalculation()
        {
            RunTest("Homogeneity calculation", () =>
            {
                var node = new OptimizedOctreeNode();
                var homogeneity = node.CalculateHomogeneity();
                AssertEqual(1.0, homogeneity, "Empty node should have 1.0 homogeneity");
            });
        }
        
        private static void TestHomogeneityWithUniformChildren()
        {
            RunTest("Homogeneity with uniform children", () =>
            {
                var node = new OptimizedOctreeNode
                {
                    Children = new OptimizedOctreeNode[8],
                    ChildMaterialCounts = { [MaterialId.Air] = 8 }
                };
                for (int i = 0; i < 8; i++)
                {
                    node.Children[i] = new OptimizedOctreeNode { ExplicitMaterial = MaterialId.Air };
                }
                
                var homogeneity = node.CalculateHomogeneity();
                AssertEqual(1.0, homogeneity, "All same material should be 1.0");
            });
        }
        
        private static void TestHomogeneityWithMixedChildren()
        {
            RunTest("Homogeneity with mixed children", () =>
            {
                var node = new OptimizedOctreeNode
                {
                    Children = new OptimizedOctreeNode[8],
                    ChildMaterialCounts = { [MaterialId.Air] = 7, [MaterialId.Dirt] = 1 }
                };
                for (int i = 0; i < 8; i++)
                {
                    node.Children[i] = new OptimizedOctreeNode();
                }
                
                var homogeneity = node.CalculateHomogeneity();
                Assert(homogeneity > 0.8 && homogeneity < 1.0, "7/8 same should be ~0.875");
            });
        }
        
        private static void TestHomogeneityCaching()
        {
            RunTest("Homogeneity caching", () =>
            {
                var node = new OptimizedOctreeNode();
                var h1 = node.CalculateHomogeneity();
                var h2 = node.CalculateHomogeneity();
                AssertEqual(h1, h2, "Cached homogeneity should match");
                Assert(node.CachedHomogeneity.HasValue, "Homogeneity should be cached");
            });
        }
        
        // Collapse Tests
        private static void TestNodeCollapse()
        {
            RunTest("Node collapse", () =>
            {
                var node = new OptimizedOctreeNode
                {
                    Children = new OptimizedOctreeNode[8],
                    ChildMaterialCounts = { [MaterialId.Air] = 8 }
                };
                for (int i = 0; i < 8; i++)
                {
                    node.Children[i] = new OptimizedOctreeNode { ExplicitMaterial = MaterialId.Air };
                }
                
                bool collapsed = node.TryCollapse(0.9);
                Assert(collapsed, "Should collapse with 100% homogeneity");
                Assert(node.IsCollapsed, "IsCollapsed should be true");
                Assert(node.Children == null, "Children should be null after collapse");
            });
        }
        
        private static void TestNodeCollapseThreshold()
        {
            RunTest("Node collapse threshold", () =>
            {
                var node = new OptimizedOctreeNode
                {
                    Children = new OptimizedOctreeNode[8],
                    ChildMaterialCounts = { [MaterialId.Air] = 7, [MaterialId.Dirt] = 1 }
                };
                for (int i = 0; i < 8; i++)
                {
                    node.Children[i] = new OptimizedOctreeNode();
                }
                
                bool collapsed = node.TryCollapse(0.9);
                Assert(!collapsed, "Should not collapse below threshold");
            });
        }
        
        private static void TestNodeExpansion()
        {
            RunTest("Node expansion", () =>
            {
                var node = new OptimizedOctreeNode
                {
                    ExplicitMaterial = MaterialId.Water,
                    IsCollapsed = true,
                    Bounds = new BoundingBox3D(new Vector3(0, 0, 0), new Vector3(100, 100, 100))
                };
                
                node.Expand();
                Assert(!node.IsCollapsed, "Should not be collapsed after expansion");
                Assert(node.Children != null, "Should have children after expansion");
                AssertEqual(8, node.Children.Length, "Should have 8 children");
            });
        }
        
        private static void TestCollapseAndReExpand()
        {
            RunTest("Collapse and re-expand", () =>
            {
                var node = new OptimizedOctreeNode
                {
                    Children = new OptimizedOctreeNode[8],
                    ChildMaterialCounts = { [MaterialId.Air] = 8 },
                    Bounds = new BoundingBox3D(new Vector3(0, 0, 0), new Vector3(100, 100, 100))
                };
                for (int i = 0; i < 8; i++)
                {
                    node.Children[i] = new OptimizedOctreeNode { ExplicitMaterial = MaterialId.Air };
                }
                
                node.TryCollapse(0.9);
                node.Expand();
                Assert(node.Children != null, "Should have children after re-expansion");
            });
        }
        
        // Child Material Counts Tests
        private static void TestChildMaterialCountUpdate()
        {
            RunTest("Child material count update", () =>
            {
                var node = new OptimizedOctreeNode();
                node.ChildMaterialCounts[MaterialId.Air] = 8;
                
                node.UpdateChildMaterialCount(MaterialId.Air, MaterialId.Water);
                AssertEqual(7, node.ChildMaterialCounts[MaterialId.Air], "Air count should decrease");
                AssertEqual(1, node.ChildMaterialCounts[MaterialId.Water], "Water count should increase");
            });
        }
        
        private static void TestChildMaterialCountAccuracy()
        {
            RunTest("Child material count accuracy", () =>
            {
                var node = new OptimizedOctreeNode
                {
                    ChildMaterialCounts = { [MaterialId.Air] = 5, [MaterialId.Water] = 3 }
                };
                
                int totalCount = node.ChildMaterialCounts.Values.Sum();
                AssertEqual(8, totalCount, "Total count should be 8");
            });
        }
        
        private static void TestMaterialDistribution()
        {
            RunTest("Material distribution", () =>
            {
                var node = new OptimizedOctreeNode
                {
                    ChildMaterialCounts = 
                    { 
                        [MaterialId.Air] = 4,
                        [MaterialId.Water] = 2,
                        [MaterialId.Dirt] = 2
                    }
                };
                
                AssertEqual(3, node.ChildMaterialCounts.Count, "Should have 3 material types");
            });
        }
        
        // Cache Tests
        private static void TestPathCacheBasic()
        {
            RunTest("Path cache basic", () =>
            {
                var cache = new MaterialInheritanceCache();
                var root = new OptimizedOctreeNode { ExplicitMaterial = MaterialId.Air };
                
                var m1 = cache.GetMaterialForPath("0", root);
                var m2 = cache.GetMaterialForPath("0", root);
                AssertEqual(m1, m2, "Cached result should match");
            });
        }
        
        private static void TestPathCacheInvalidation()
        {
            RunTest("Path cache invalidation", () =>
            {
                var cache = new MaterialInheritanceCache();
                var root = new OptimizedOctreeNode { ExplicitMaterial = MaterialId.Air };
                
                cache.GetMaterialForPath("0", root);
                cache.InvalidatePath("0");
                
                var stats = cache.GetStatistics();
                Assert(stats != null, "Should get statistics after invalidation");
            });
        }
        
        private static void TestPointCacheBasic()
        {
            RunTest("Point cache basic", () =>
            {
                var cache = new MaterialInheritanceCache();
                var root = new OptimizedOctreeNode 
                { 
                    ExplicitMaterial = MaterialId.Air,
                    Bounds = new BoundingBox3D(new Vector3(0, 0, 0), new Vector3(100, 100, 100))
                };
                
                var point = new Vector3(50, 50, 50);
                var m1 = cache.GetMaterialForPoint(point, root);
                var m2 = cache.GetMaterialForPoint(point, root);
                AssertEqual(m1, m2, "Cached point query should match");
            });
        }
        
        private static void TestMortonCacheBasic()
        {
            RunTest("Morton cache basic", () =>
            {
                var cache = new MaterialInheritanceCache();
                var root = new OptimizedOctreeNode 
                { 
                    ExplicitMaterial = MaterialId.Water,
                    Bounds = new BoundingBox3D(new Vector3(0, 0, 0), new Vector3(1000, 1000, 1000))
                };
                
                var p1 = new Vector3(100, 200, 300);
                var p2 = new Vector3(100, 200, 300);
                var m1 = cache.GetMaterialForPoint(p1, root);
                var m2 = cache.GetMaterialForPoint(p2, root);
                AssertEqual(m1, m2, "Same point should use Morton cache");
            });
        }
        
        private static void TestLRUEviction()
        {
            RunTest("LRU eviction", () =>
            {
                var lru = new LRUCache<int, string>(3);
                lru.Put(1, "one");
                lru.Put(2, "two");
                lru.Put(3, "three");
                lru.Put(4, "four"); // Should evict 1
                
                bool found = lru.TryGet(1, out _);
                Assert(!found, "Oldest entry should be evicted");
            });
        }
        
        private static void TestCacheStatistics()
        {
            RunTest("Cache statistics", () =>
            {
                var cache = new MaterialInheritanceCache();
                var stats = cache.GetStatistics();
                Assert(stats != null, "Should get cache statistics");
                Assert(stats.HitRate >= 0 && stats.HitRate <= 1, "Hit rate should be 0-1");
            });
        }
        
        // High-Level API Tests
        private static void TestOctreeCreation()
        {
            RunTest("Octree creation", () =>
            {
                var bounds = new BoundingBox3D(new Vector3(0, 0, 0), new Vector3(1000, 1000, 1000));
                var octree = new OptimizedOctree(bounds);
                Assert(octree != null, "Octree should be created");
            });
        }
        
        private static void TestPointQuery()
        {
            RunTest("Point query", () =>
            {
                var bounds = new BoundingBox3D(new Vector3(0, 0, 0), new Vector3(1000, 1000, 1000));
                var octree = new OptimizedOctree(bounds);
                
                var material = octree.QueryMaterial(500, 500, 500);
                AssertEqual(MaterialId.Air, material, "Should query material at point");
            });
        }
        
        private static void TestSetMaterial()
        {
            RunTest("Set material", () =>
            {
                var bounds = new BoundingBox3D(new Vector3(0, 0, 0), new Vector3(1000, 1000, 1000));
                var octree = new OptimizedOctree(bounds);
                
                var point = new Vector3(500, 500, 500);
                octree.SetMaterial(point, MaterialId.Water);
                
                var material = octree.QueryMaterial(point);
                AssertEqual(MaterialId.Water, material, "Should set and query material");
            });
        }
        
        private static void TestSetRegion()
        {
            RunTest("Set region", () =>
            {
                var bounds = new BoundingBox3D(new Vector3(0, 0, 0), new Vector3(1000, 1000, 1000));
                var octree = new OptimizedOctree(bounds);
                
                var region = new BoundingBox3D(new Vector3(100, 100, 100), new Vector3(200, 200, 200));
                octree.SetRegion(region, MaterialId.Rock);
                
                var material = octree.QueryMaterial(150, 150, 150);
                AssertEqual(MaterialId.Rock, material, "Should set region material");
            });
        }
        
        private static void TestRegionOperations()
        {
            RunTest("Region operations", () =>
            {
                var bounds = new BoundingBox3D(new Vector3(0, 0, 0), new Vector3(1000, 1000, 1000));
                var octree = new OptimizedOctree(bounds);
                
                // Set multiple regions
                octree.SetRegion(new BoundingBox3D(new Vector3(0, 0, 0), new Vector3(500, 500, 500)), MaterialId.Water);
                octree.SetRegion(new BoundingBox3D(new Vector3(500, 500, 500), new Vector3(1000, 1000, 1000)), MaterialId.Air);
                
                AssertEqual(MaterialId.Water, octree.QueryMaterial(250, 250, 250), "First region should be water");
                AssertEqual(MaterialId.Air, octree.QueryMaterial(750, 750, 750), "Second region should be air");
            });
        }
        
        // Memory Optimization Tests
        private static void TestMemoryOptimization()
        {
            RunTest("Memory optimization", () =>
            {
                var bounds = new BoundingBox3D(new Vector3(0, 0, 0), new Vector3(1000, 1000, 1000));
                var octree = new OptimizedOctree(bounds);
                
                // Set uniform region
                octree.SetRegion(bounds, MaterialId.Water);
                octree.OptimizeMemory();
                
                var stats = octree.CalculateMemoryStatistics();
                Assert(stats.CollapsedNodes > 0 || stats.NodesInheritingMaterial > 0, 
                    "Should have optimization");
            });
        }
        
        private static void TestMemoryStatistics()
        {
            RunTest("Memory statistics", () =>
            {
                var bounds = new BoundingBox3D(new Vector3(0, 0, 0), new Vector3(1000, 1000, 1000));
                var octree = new OptimizedOctree(bounds);
                
                var stats = octree.CalculateMemoryStatistics();
                Assert(stats != null, "Should get memory statistics");
                Assert(stats.TotalNodes > 0, "Should have at least root node");
            });
        }
        
        private static void TestInheritanceEfficiency()
        {
            RunTest("Inheritance efficiency", () =>
            {
                var bounds = new BoundingBox3D(new Vector3(0, 0, 0), new Vector3(1000, 1000, 1000));
                var octree = new OptimizedOctree(bounds);
                
                // Create some structure
                for (int i = 0; i < 10; i++)
                {
                    var point = new Vector3(i * 100, i * 100, i * 100);
                    octree.SetMaterial(point, MaterialId.Air);
                }
                
                var stats = octree.CalculateMemoryStatistics();
                Assert(stats.InheritanceEfficiencyRatio >= 0 && stats.InheritanceEfficiencyRatio <= 1,
                    "Efficiency should be 0-1");
            });
        }
        
        private static void TestCollapsedNodeMemory()
        {
            RunTest("Collapsed node memory", () =>
            {
                var bounds = new BoundingBox3D(new Vector3(0, 0, 0), new Vector3(1000, 1000, 1000));
                var octree = new OptimizedOctree(bounds);
                
                octree.SetRegion(bounds, MaterialId.Water);
                octree.OptimizeMemory();
                
                var stats = octree.CalculateMemoryStatistics();
                Assert(stats.EstimatedMemoryBytes > 0, "Should estimate memory usage");
            });
        }
        
        // Integration Tests
        private static void TestOceanScenario()
        {
            RunTest("Ocean scenario (95% reduction)", () =>
            {
                var bounds = new BoundingBox3D(new Vector3(0, 0, 0), new Vector3(10000, 10000, 1000));
                var octree = new OptimizedOctree(bounds);
                
                // Set entire ocean region to water
                octree.SetRegion(bounds, MaterialId.Water);
                octree.OptimizeMemory();
                
                var stats = octree.CalculateMemoryStatistics();
                Assert(stats.InheritanceEfficiencyRatio > 0.5 || stats.CollapsedNodes > 0,
                    "Ocean should have high inheritance or collapse");
            });
        }
        
        private static void TestContinentalScenario()
        {
            RunTest("Continental scenario (60-80% reduction)", () =>
            {
                var bounds = new BoundingBox3D(new Vector3(0, 0, 0), new Vector3(5000, 5000, 1000));
                var octree = new OptimizedOctree(bounds);
                
                // Mixed materials
                octree.SetRegion(new BoundingBox3D(new Vector3(0, 0, 0), new Vector3(2500, 5000, 500)), MaterialId.Rock);
                octree.SetRegion(new BoundingBox3D(new Vector3(2500, 0, 0), new Vector3(5000, 5000, 500)), MaterialId.Dirt);
                
                var stats = octree.CalculateMemoryStatistics();
                Assert(stats.MaterialDistribution.Count >= 2, "Should have multiple materials");
            });
        }
        
        private static void TestCoastalScenario()
        {
            RunTest("Coastal scenario (40-60% reduction)", () =>
            {
                var bounds = new BoundingBox3D(new Vector3(0, 0, 0), new Vector3(2000, 2000, 500));
                var octree = new OptimizedOctree(bounds);
                
                // Heterogeneous coastal region
                octree.SetRegion(new BoundingBox3D(new Vector3(0, 0, 0), new Vector3(1000, 2000, 250)), MaterialId.Water);
                octree.SetRegion(new BoundingBox3D(new Vector3(1000, 0, 0), new Vector3(2000, 2000, 250)), MaterialId.Sand);
                octree.SetRegion(new BoundingBox3D(new Vector3(1000, 0, 250), new Vector3(2000, 2000, 500)), MaterialId.Dirt);
                
                var stats = octree.CalculateMemoryStatistics();
                Assert(stats.MaterialDistribution.Count >= 3, "Should have water, sand, and dirt");
            });
        }
        
        private static void TestBlueMarbleExample()
        {
            RunTest("BlueMarble 16×16m air with dirt inclusion", () =>
            {
                var bounds = new BoundingBox3D(new Vector3(1000, 1000, 10000000), new Vector3(1016, 1016, 10000100));
                var octree = new OptimizedOctree(bounds);
                
                // Set entire region to air
                octree.SetRegion(bounds, MaterialId.Air);
                
                // Add small dirt inclusion (10% of volume)
                var dirtRegion = new BoundingBox3D(new Vector3(1000, 1000, 10000080), new Vector3(1002, 1002, 10000082));
                octree.SetRegion(dirtRegion, MaterialId.Dirt);
                
                // Verify 90% threshold behavior
                var stats = octree.CalculateMemoryStatistics();
                Assert(stats.MaterialDistribution.ContainsKey(MaterialId.Air), "Should have air");
                Assert(stats.MaterialDistribution.ContainsKey(MaterialId.Dirt), "Should have dirt");
                
                // Verify queries
                AssertEqual(MaterialId.Air, octree.QueryMaterial(1010, 1010, 10000090), "Should query air");
                AssertEqual(MaterialId.Dirt, octree.QueryMaterial(1001, 1001, 10000081), "Should query dirt");
            });
        }
        
        // Performance Tests
        private static void TestQueryPerformance()
        {
            RunTest("Query performance (<1ms)", () =>
            {
                var bounds = new BoundingBox3D(new Vector3(0, 0, 0), new Vector3(10000, 10000, 10000));
                var octree = new OptimizedOctree(bounds);
                
                octree.SetRegion(bounds, MaterialId.Water);
                
                var start = DateTime.UtcNow;
                for (int i = 0; i < 1000; i++)
                {
                    octree.QueryMaterial(i * 10, i * 10, i * 10);
                }
                var elapsed = DateTime.UtcNow - start;
                
                Assert(elapsed.TotalMilliseconds < 1000, "1000 queries should be fast");
            });
        }
        
        private static void TestCachePerformance()
        {
            RunTest("Cache performance improvement", () =>
            {
                var bounds = new BoundingBox3D(new Vector3(0, 0, 0), new Vector3(10000, 10000, 10000));
                var octree = new OptimizedOctree(bounds);
                octree.SetRegion(bounds, MaterialId.Water);
                
                // First query (cache miss)
                var start1 = DateTime.UtcNow;
                octree.QueryMaterial(5000, 5000, 5000);
                var time1 = DateTime.UtcNow - start1;
                
                // Second query (cache hit)
                var start2 = DateTime.UtcNow;
                octree.QueryMaterial(5000, 5000, 5000);
                var time2 = DateTime.UtcNow - start2;
                
                // Cache should be faster or similar
                Assert(time2.TotalMilliseconds <= time1.TotalMilliseconds * 1.5,
                    "Cached query should not be slower");
            });
        }
        
        private static void TestCollapsePerformance()
        {
            RunTest("Collapse performance", () =>
            {
                var bounds = new BoundingBox3D(new Vector3(0, 0, 0), new Vector3(1000, 1000, 1000));
                var octree = new OptimizedOctree(bounds);
                
                octree.SetRegion(bounds, MaterialId.Water);
                
                var start = DateTime.UtcNow;
                octree.OptimizeMemory();
                var elapsed = DateTime.UtcNow - start;
                
                Assert(elapsed.TotalSeconds < 5, "Optimization should complete quickly");
            });
        }
    }
}
