using System;
using System.Diagnostics;

namespace BlueMarble.SpatialData.HeightRaster
{
    /// <summary>
    /// Demonstration program showing height raster features
    /// </summary>
    public class DemoProgram
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("=== Height Raster Surface Representation Demo ===\n");
            
            // Demo 1: Basic height raster
            Console.WriteLine("Demo 1: Basic Height Raster");
            DemoBasicHeightRaster();
            Console.WriteLine();
            
            // Demo 2: Material layers
            Console.WriteLine("Demo 2: Material Layer System");
            DemoMaterialLayers();
            Console.WriteLine();
            
            // Demo 3: User modifications
            Console.WriteLine("Demo 3: User Modifications (Delta Overlay)");
            DemoUserModifications();
            Console.WriteLine();
            
            // Demo 4: Cliff detection
            Console.WriteLine("Demo 4: Cliff Detection and Voxel Conversion");
            DemoCliffDetection();
            Console.WriteLine();
            
            // Demo 5: Storage efficiency
            Console.WriteLine("Demo 5: Storage Efficiency Analysis");
            DemoStorageEfficiency();
            Console.WriteLine();
            
            // Demo 6: Performance comparison
            Console.WriteLine("Demo 6: Performance Comparison");
            DemoPerformanceComparison();
            Console.WriteLine();
            
            Console.WriteLine("Demo complete!");
        }
        
        static void DemoBasicHeightRaster()
        {
            var tile = new HeightRasterTile(new Vector2Int(0, 0));
            
            // Create simple terrain - sine wave pattern
            Console.WriteLine("Creating terrain with sine wave pattern...");
            for (int y = 0; y < HeightRasterTile.TILE_SIZE; y++)
            {
                for (int x = 0; x < HeightRasterTile.TILE_SIZE; x++)
                {
                    // Sine wave: height varies from 0 to 20 meters
                    float height = 10.0f + 10.0f * (float)Math.Sin(x * 0.01) * (float)Math.Cos(y * 0.01);
                    tile.Heights[y * HeightRasterTile.TILE_SIZE + x] = height;
                }
            }
            
            // Query some heights
            Console.WriteLine("\nQuerying heights at various positions:");
            var positions = new[] {
                new Vector2(0, 0),
                new Vector2(100.5f, 100.5f),
                new Vector2(200.3f, 150.7f)
            };
            
            foreach (var pos in positions)
            {
                float height = tile.GetHeight(pos);
                Console.WriteLine($"  Height at ({pos.X:F1}, {pos.Y:F1}) = {height:F2}m");
            }
        }
        
        static void DemoMaterialLayers()
        {
            var column = new MaterialColumn();
            
            Console.WriteLine("Material column structure:");
            Console.WriteLine("  Layer 0 (0.00m): " + GetMaterialName(column.Materials[0]));
            Console.WriteLine("  Layer 1 (0.25m): " + GetMaterialName(column.Materials[1]));
            Console.WriteLine("  Layer 2 (0.50m): " + GetMaterialName(column.Materials[2]));
            Console.WriteLine("  Layer 3 (1.00m): " + GetMaterialName(column.Materials[3]));
            Console.WriteLine("  Layer 4 (2.00m): " + GetMaterialName(column.Materials[4]));
            Console.WriteLine("  Layer 5 (5.00m): " + GetMaterialName(column.Materials[5]));
            Console.WriteLine("  Layer 6 (10.0m): " + GetMaterialName(column.Materials[6]));
            Console.WriteLine("  Layer 7 (20.0m): " + GetMaterialName(column.Materials[7]));
            
            Console.WriteLine("\nQuerying materials at various depths:");
            var depths = new[] { 0.0f, 0.5f, 2.0f, 10.0f, 50.0f, 1000.0f };
            foreach (var depth in depths)
            {
                byte material = column.GetMaterialAtDepth(depth);
                Console.WriteLine($"  Depth {depth,6:F1}m: {GetMaterialName(material)}");
            }
        }
        
        static void DemoUserModifications()
        {
            var tile = new HeightRasterTile(new Vector2Int(0, 0));
            var playerId = Guid.NewGuid();
            
            // Set some base heights
            for (int i = 0; i < HeightRasterTile.TILE_SIZE * HeightRasterTile.TILE_SIZE; i++)
            {
                tile.Heights[i] = 10.0f;  // Flat terrain at 10m
            }
            
            Console.WriteLine("Placing user blocks...");
            
            // User places some blocks
            var placements = new[] {
                new Vector3(100.0f, 100.0f, 12.0f),
                new Vector3(100.0f, 100.0f, 13.0f),
                new Vector3(100.0f, 100.0f, 14.0f)
            };
            
            foreach (var pos in placements)
            {
                tile.SetMaterial(pos, MaterialId.Stone, playerId);
                Console.WriteLine($"  Placed stone at ({pos.X:F1}, {pos.Y:F1}, {pos.Z:F1})");
            }
            
            Console.WriteLine($"\nTotal modifications: {tile.UserModifications.GetModificationCount()}");
            Console.WriteLine($"Visible modifications: {tile.UserModifications.GetVisibleModifications().Count()}");
            
            // Query the placed blocks
            Console.WriteLine("\nQuerying placed blocks:");
            foreach (var pos in placements)
            {
                byte material = tile.GetMaterial(pos);
                Console.WriteLine($"  Material at ({pos.X:F1}, {pos.Y:F1}, {pos.Z:F1}) = {GetMaterialName(material)}");
            }
        }
        
        static void DemoCliffDetection()
        {
            var tile = new HeightRasterTile(new Vector2Int(0, 0));
            
            Console.WriteLine("Creating terrain with cliff...");
            
            // Create flat terrain with a steep cliff
            for (int y = 0; y < HeightRasterTile.TILE_SIZE; y++)
            {
                for (int x = 0; x < HeightRasterTile.TILE_SIZE; x++)
                {
                    float height;
                    if (x < HeightRasterTile.TILE_SIZE / 2)
                    {
                        height = 10.0f;  // Flat low area
                    }
                    else if (x < HeightRasterTile.TILE_SIZE / 2 + 5)
                    {
                        // Steep cliff - 50m rise over 5 cells (1.25m)
                        height = 10.0f + (x - HeightRasterTile.TILE_SIZE / 2) * 10.0f;
                    }
                    else
                    {
                        height = 60.0f;  // Flat high area
                    }
                    
                    tile.Heights[y * HeightRasterTile.TILE_SIZE + x] = height;
                }
            }
            
            // Detect cliffs
            Console.WriteLine("Detecting cliffs...");
            tile.DetectAndConvertCliffs();
            
            Console.WriteLine($"Voxel regions created: {tile.VoxelRegions.Count}");
            Console.WriteLine($"Percentage of tile voxelized: {100.0 * tile.VoxelRegions.Count / (HeightRasterTile.TILE_SIZE * HeightRasterTile.TILE_SIZE):F2}%");
        }
        
        static void DemoStorageEfficiency()
        {
            // Create tiles with different characteristics
            var flatTile = CreateFlatTerrain();
            var mountainTile = CreateMountainTerrain();
            var cliffTile = CreateCliffTerrain();
            
            Console.WriteLine("Storage size comparison:\n");
            
            Console.WriteLine("Flat terrain (ocean floor):");
            PrintStorageInfo(flatTile);
            
            Console.WriteLine("\nMountain terrain (rolling hills):");
            PrintStorageInfo(mountainTile);
            
            Console.WriteLine("\nCliff terrain (steep cliffs):");
            PrintStorageInfo(cliffTile);
            
            // Compare to full 3D voxels
            long full3DVoxelSize = CalculateFull3DVoxelSize();
            Console.WriteLine($"\nFull 3D voxel storage (100m depth): {full3DVoxelSize / (1024 * 1024):F1} MB");
            Console.WriteLine($"Height raster reduction: {100.0 * (1.0 - flatTile.CalculateStorageSize() / (double)full3DVoxelSize):F1}%");
        }
        
        static void DemoPerformanceComparison()
        {
            var tile = new HeightRasterTile(new Vector2Int(0, 0));
            
            // Initialize terrain
            for (int i = 0; i < HeightRasterTile.TILE_SIZE * HeightRasterTile.TILE_SIZE; i++)
            {
                tile.Heights[i] = 10.0f + (float)(i % 100) * 0.1f;
            }
            
            const int queryCount = 100000;
            var random = new Random(42);
            
            // Benchmark GetHeight
            Console.WriteLine($"Running {queryCount} GetHeight queries...");
            var sw = Stopwatch.StartNew();
            for (int i = 0; i < queryCount; i++)
            {
                float x = (float)random.NextDouble() * 250.0f;
                float y = (float)random.NextDouble() * 250.0f;
                float height = tile.GetHeight(new Vector2(x, y));
            }
            sw.Stop();
            Console.WriteLine($"  Average time: {sw.ElapsedMilliseconds * 1000.0 / queryCount:F3} microseconds");
            
            // Benchmark GetMaterial
            Console.WriteLine($"\nRunning {queryCount} GetMaterial queries...");
            sw.Restart();
            for (int i = 0; i < queryCount; i++)
            {
                float x = (float)random.NextDouble() * 250.0f;
                float y = (float)random.NextDouble() * 250.0f;
                float z = (float)random.NextDouble() * 20.0f;
                byte material = tile.GetMaterial(new Vector3(x, y, z));
            }
            sw.Stop();
            Console.WriteLine($"  Average time: {sw.ElapsedMilliseconds * 1000.0 / queryCount:F3} microseconds");
        }
        
        // Helper methods
        static HeightRasterTile CreateFlatTerrain()
        {
            var tile = new HeightRasterTile(new Vector2Int(0, 0));
            for (int i = 0; i < HeightRasterTile.TILE_SIZE * HeightRasterTile.TILE_SIZE; i++)
            {
                tile.Heights[i] = 10.0f;
            }
            return tile;
        }
        
        static HeightRasterTile CreateMountainTerrain()
        {
            var tile = new HeightRasterTile(new Vector2Int(0, 0));
            for (int y = 0; y < HeightRasterTile.TILE_SIZE; y++)
            {
                for (int x = 0; x < HeightRasterTile.TILE_SIZE; x++)
                {
                    float height = 10.0f + 20.0f * (float)Math.Sin(x * 0.02) * (float)Math.Cos(y * 0.02);
                    tile.Heights[y * HeightRasterTile.TILE_SIZE + x] = height;
                }
            }
            return tile;
        }
        
        static HeightRasterTile CreateCliffTerrain()
        {
            var tile = new HeightRasterTile(new Vector2Int(0, 0));
            for (int y = 0; y < HeightRasterTile.TILE_SIZE; y++)
            {
                for (int x = 0; x < HeightRasterTile.TILE_SIZE; x++)
                {
                    float height = x < HeightRasterTile.TILE_SIZE / 2 ? 10.0f : 60.0f;
                    tile.Heights[y * HeightRasterTile.TILE_SIZE + x] = height;
                }
            }
            tile.DetectAndConvertCliffs();
            return tile;
        }
        
        static void PrintStorageInfo(HeightRasterTile tile)
        {
            long size = tile.CalculateStorageSize();
            Console.WriteLine($"  Total size: {size / (1024 * 1024):F2} MB");
            Console.WriteLine($"  Height field: {HeightRasterTile.TILE_SIZE * HeightRasterTile.TILE_SIZE * 4 / (1024 * 1024):F2} MB");
            Console.WriteLine($"  Material layers: {HeightRasterTile.TILE_SIZE * HeightRasterTile.TILE_SIZE * 8 / (1024 * 1024):F2} MB");
            Console.WriteLine($"  Modifications: {tile.UserModifications.GetModificationCount() * 25 / 1024:F2} KB");
            Console.WriteLine($"  Voxel regions: {tile.VoxelRegions.Count * 400 / 1024:F2} KB");
        }
        
        static long CalculateFull3DVoxelSize()
        {
            // 4000 x 4000 x 400 voxels at 1 byte per voxel
            return 4000L * 4000L * 400L;
        }
        
        static string GetMaterialName(byte materialId)
        {
            return materialId switch
            {
                MaterialId.Air => "Air",
                MaterialId.Grass => "Grass",
                MaterialId.Soil => "Soil",
                MaterialId.Stone => "Stone",
                MaterialId.Bedrock => "Bedrock",
                MaterialId.DeepRock => "Deep Rock",
                MaterialId.Sand => "Sand",
                MaterialId.Water => "Water",
                _ => $"Unknown ({materialId})"
            };
        }
    }
}
