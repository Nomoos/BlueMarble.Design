using System;
using System.Collections.Generic;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using BlueMarble.SpatialStorage.Octree;

namespace BlueMarble.Benchmarks.SpatialStorage
{
    /// <summary>
    /// Benchmark to demonstrate memory savings and performance of implicit material inheritance
    /// </summary>
    [MemoryDiagnoser]
    [SimpleJob]
    public class MaterialInheritanceBenchmark
    {
        private MaterialInheritanceNode _inheritanceTree = null!;
        private MaterialInheritanceNode _explicitTree = null!;
        private MaterialInheritanceCache _cache = null!;
        private readonly Random _random = new(42);

        [GlobalSetup]
        public void Setup()
        {
            _cache = new MaterialInheritanceCache();
            
            // Create trees for comparison
            _inheritanceTree = CreateInheritanceOptimizedTree();
            _explicitTree = CreateExplicitMaterialTree();
        }

        [Benchmark]
        public MaterialData QueryInheritanceTree()
        {
            var point = GenerateRandomPoint();
            return _inheritanceTree.GetMaterialAtPoint(point);
        }

        [Benchmark]
        public MaterialData QueryExplicitTree()
        {
            var point = GenerateRandomPoint();
            return _explicitTree.GetMaterialAtPoint(point);
        }

        [Benchmark]
        public MaterialData QueryWithCache()
        {
            var path = GenerateRandomPath();
            return _cache.GetMaterialForPath(path, _inheritanceTree);
        }

        [Benchmark]
        public long CalculateInheritanceMemoryFootprint()
        {
            return _inheritanceTree.CalculateMemoryFootprint();
        }

        [Benchmark]
        public long CalculateExplicitMemoryFootprint()
        {
            return _explicitTree.CalculateMemoryFootprint();
        }

        /// <summary>
        /// Create a large ocean region with sparse islands (optimized for inheritance)
        /// </summary>
        private MaterialInheritanceNode CreateInheritanceOptimizedTree()
        {
            var root = new BlueMarbleMaterialNode
            {
                ExplicitMaterial = MaterialData.DefaultOcean,
                PrimaryMaterial = MaterialId.Ocean,
                CellSize = 10000.0, // 10km region
                Level = 0,
                Bounds = new BoundingBox
                {
                    Min = new Vector3(-5000, -5000, -100),
                    Max = new Vector3(5000, 5000, 0)
                }
            };

            // Create 4 levels of subdivision (represents ~65,536 potential cells)
            CreateInheritanceSubdivision(root, 4, 0.05); // Only 5% of cells have explicit materials

            return root;
        }

        /// <summary>
        /// Create equivalent tree where every node has explicit material
        /// </summary>
        private MaterialInheritanceNode CreateExplicitMaterialTree()
        {
            var root = new BlueMarbleMaterialNode
            {
                ExplicitMaterial = MaterialData.DefaultOcean,
                PrimaryMaterial = MaterialId.Ocean,
                CellSize = 10000.0,
                Level = 0,
                Bounds = new BoundingBox
                {
                    Min = new Vector3(-5000, -5000, -100),
                    Max = new Vector3(5000, 5000, 0)
                }
            };

            // Create same structure but with explicit materials everywhere
            CreateExplicitSubdivision(root, 4);

            return root;
        }

        private void CreateInheritanceSubdivision(MaterialInheritanceNode node, int remainingLevels, double explicitMaterialProbability)
        {
            if (remainingLevels <= 0) return;

            node.Children = new MaterialInheritanceNode[8];

            for (int i = 0; i < 8; i++)
            {
                var child = new BlueMarbleMaterialNode
                {
                    Parent = node,
                    Level = node.Level + 1,
                    CellSize = ((BlueMarbleMaterialNode)node).CellSize / 2,
                    PrimaryMaterial = MaterialId.Ocean
                };

                // Only set explicit material occasionally (simulating sparse islands)
                if (_random.NextDouble() < explicitMaterialProbability)
                {
                    child.ExplicitMaterial = new MaterialData
                    {
                        Id = MaterialId.Rock,
                        Name = "Island",
                        Density = 2.5f
                    };
                }

                child.Bounds = CalculateChildBounds(node.Bounds, i);
                node.Children[i] = child;

                // Recursively create children
                CreateInheritanceSubdivision(child, remainingLevels - 1, explicitMaterialProbability);
            }
        }

        private void CreateExplicitSubdivision(MaterialInheritanceNode node, int remainingLevels)
        {
            if (remainingLevels <= 0) return;

            node.Children = new MaterialInheritanceNode[8];

            for (int i = 0; i < 8; i++)
            {
                var child = new BlueMarbleMaterialNode
                {
                    Parent = node,
                    Level = node.Level + 1,
                    CellSize = ((BlueMarbleMaterialNode)node).CellSize / 2,
                    PrimaryMaterial = MaterialId.Ocean,
                    ExplicitMaterial = MaterialData.DefaultOcean // Always explicit
                };

                child.Bounds = CalculateChildBounds(node.Bounds, i);
                node.Children[i] = child;

                CreateExplicitSubdivision(child, remainingLevels - 1);
            }
        }

        private BoundingBox CalculateChildBounds(BoundingBox parentBounds, int childIndex)
        {
            var center = parentBounds.Center;
            var halfSize = new Vector3(
                (parentBounds.Max.X - parentBounds.Min.X) / 4,
                (parentBounds.Max.Y - parentBounds.Min.Y) / 4,
                (parentBounds.Max.Z - parentBounds.Min.Z) / 4
            );

            var offsetX = (childIndex & 1) == 0 ? -halfSize.X : halfSize.X;
            var offsetY = (childIndex & 2) == 0 ? -halfSize.Y : halfSize.Y;
            var offsetZ = (childIndex & 4) == 0 ? -halfSize.Z : halfSize.Z;

            var childCenter = new Vector3(
                center.X + offsetX,
                center.Y + offsetY,
                center.Z + offsetZ
            );

            return new BoundingBox
            {
                Min = new Vector3(
                    childCenter.X - halfSize.X,
                    childCenter.Y - halfSize.Y,
                    childCenter.Z - halfSize.Z
                ),
                Max = new Vector3(
                    childCenter.X + halfSize.X,
                    childCenter.Y + halfSize.Y,
                    childCenter.Z + halfSize.Z
                )
            };
        }

        private Vector3 GenerateRandomPoint()
        {
            return new Vector3(
                _random.Next(-5000, 5000),
                _random.Next(-5000, 5000),
                _random.Next(-100, 0)
            );
        }

        private string GenerateRandomPath()
        {
            var length = _random.Next(1, 8);
            var path = "";
            for (int i = 0; i < length; i++)
            {
                path += _random.Next(0, 8).ToString();
            }
            return path;
        }
    }

    /// <summary>
    /// Standalone program to run memory savings demonstration
    /// </summary>
    public static class MemorySavingsDemo
    {
        public static void RunDemo()
        {
            Console.WriteLine("=== BlueMarble Material Inheritance Memory Savings Demo ===\n");

            // Create demonstration scenarios
            RunOceanScenario();
            RunBlueMarbleScenario();
            RunLargeScaleScenario();
        }

        private static void RunOceanScenario()
        {
            Console.WriteLine("1. Ocean with Sparse Islands Scenario");
            Console.WriteLine("   Creating large ocean region (1000km²) with few islands...");

            var oceanNode = CreateOceanWithIslands();
            var report = oceanNode.CalculateMemorySavings();

            Console.WriteLine($"   Results: {report}");
            Console.WriteLine($"   Memory reduction: {report.SavingsBytes:N0} bytes saved");
            Console.WriteLine();
        }

        private static void RunBlueMarbleScenario()
        {
            Console.WriteLine("2. BlueMarble Example Scenario");
            Console.WriteLine("   Creating 16x16m air cell with 4x4m dirt child...");

            var blueMarbleNode = BlueMarbleMaterialNode.CreateBlueMarbleExample();
            var report = blueMarbleNode.CalculateMemorySavings();

            Console.WriteLine($"   Results: {report}");
            Console.WriteLine($"   Demonstrates inheritance for small-scale heterogeneity");
            Console.WriteLine();
        }

        private static void RunLargeScaleScenario()
        {
            Console.WriteLine("3. Large Scale Global Scenario");
            Console.WriteLine("   Simulating global ocean coverage with continents...");

            var globalNode = CreateGlobalScenario();
            var report = globalNode.CalculateMemorySavings();

            Console.WriteLine($"   Results: {report}");
            Console.WriteLine($"   Global scale demonstrates petabyte to gigabyte reduction potential");
            Console.WriteLine();

            // Demonstrate cache performance
            Console.WriteLine("4. Cache Performance Test");
            var cache = new MaterialInheritanceCache();
            
            var start = DateTime.UtcNow;
            for (int i = 0; i < 1000; i++)
            {
                cache.GetMaterialForPath($"00{i % 8}", globalNode);
            }
            var end = DateTime.UtcNow;

            var stats = cache.GetStatistics();
            Console.WriteLine($"   1000 queries completed in {(end - start).TotalMilliseconds:F1}ms");
            Console.WriteLine($"   Cache statistics: {stats.CacheSize} entries, {stats.MemoryUsage:N0} bytes");
        }

        private static BlueMarbleMaterialNode CreateOceanWithIslands()
        {
            var ocean = new BlueMarbleMaterialNode
            {
                PrimaryMaterial = MaterialId.Ocean,
                ExplicitMaterial = MaterialData.DefaultOcean,
                CellSize = 100000, // 100km
                Level = 0
            };

            // Create children representing 25km x 25km regions
            ocean.Children = new MaterialInheritanceNode[64]; // 8x8 grid simulation
            for (int i = 0; i < 64; i++)
            {
                ocean.Children[i] = new BlueMarbleMaterialNode
                {
                    Parent = ocean,
                    PrimaryMaterial = MaterialId.Ocean,
                    Level = 1,
                    // Only 10% of regions have islands (explicit material)
                    ExplicitMaterial = (i % 10 == 0) ? 
                        new MaterialData { Id = MaterialId.Rock, Name = "Island", Density = 2.5f } : 
                        null // Inherit ocean
                };
            }

            return ocean;
        }

        private static BlueMarbleMaterialNode CreateGlobalScenario()
        {
            var globe = new BlueMarbleMaterialNode
            {
                PrimaryMaterial = MaterialId.Ocean,
                ExplicitMaterial = MaterialData.DefaultOcean,
                CellSize = 40000000, // ~Earth circumference
                Level = 0
            };

            // Simulate continental structure
            globe.Children = new MaterialInheritanceNode[8];
            for (int i = 0; i < 8; i++)
            {
                var continent = new BlueMarbleMaterialNode
                {
                    Parent = globe,
                    Level = 1,
                    PrimaryMaterial = i < 2 ? MaterialId.Rock : MaterialId.Ocean, // 2/8 continents
                    ExplicitMaterial = i < 2 ? 
                        new MaterialData { Id = MaterialId.Rock, Name = "Continent", Density = 2.7f } : 
                        null // Inherit ocean
                };

                // Add sub-regions
                continent.Children = new MaterialInheritanceNode[64];
                for (int j = 0; j < 64; j++)
                {
                    continent.Children[j] = new BlueMarbleMaterialNode
                    {
                        Parent = continent,
                        Level = 2,
                        PrimaryMaterial = continent.PrimaryMaterial,
                        // Most inherit from parent (ocean or continent)
                        ExplicitMaterial = (j % 20 == 0) ? 
                            new MaterialData { Id = MaterialId.Dirt, Name = "Soil", Density = 1.8f } : 
                            null
                    };
                }
            }

            return globe;
        }
    }
}