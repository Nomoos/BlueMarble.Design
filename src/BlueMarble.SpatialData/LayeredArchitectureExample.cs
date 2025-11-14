using System.Numerics;
using BlueMarble.SpatialData;

namespace BlueMarble.SpatialData.Examples;

/// <summary>
/// Demonstration of the Template Method pattern and layered architecture
/// Shows how to use the geological process system following best practices
/// </summary>
public class LayeredArchitectureExample
{
    public static void Main(string[] args)
    {
        Console.WriteLine("=== Layered Architecture Pattern Example ===\n");

        // Initialize base storage layer
        var baseTree = new OptimizedOctreeNode
        {
            ExplicitMaterial = MaterialData.DefaultOcean
        };

        var deltaOctree = new DeltaPatchOctree(
            baseTree,
            consolidationThreshold: 1000,
            compactionStrategy: DeltaCompactionStrategy.LazyThreshold
        );

        // Example 1: Using Template Method Pattern Directly
        Console.WriteLine("Example 1: Direct Process Usage (Template Method Pattern)");
        Console.WriteLine("==========================================================");
        DirectProcessUsage(deltaOctree);

        // Example 2: Using Orchestration Layer
        Console.WriteLine("\nExample 2: Orchestration Layer Usage");
        Console.WriteLine("====================================");
        OrchestrationLayerUsage(deltaOctree);

        // Example 3: Demonstrating Layer Separation
        Console.WriteLine("\nExample 3: Layer Separation and No Skipping");
        Console.WriteLine("===========================================");
        LayerSeparationDemo(deltaOctree);

        // Example 4: Cross-Cutting Concerns
        Console.WriteLine("\nExample 4: Cross-Cutting Concerns (Logging & Caching)");
        Console.WriteLine("=====================================================");
        CrossCuttingConcernsDemo(deltaOctree);

        Console.WriteLine("\n=== All Examples Complete ===");
    }

    /// <summary>
    /// Example 1: Direct usage of geological processes following Template Method pattern
    /// </summary>
    static void DirectProcessUsage(DeltaPatchOctree deltaOctree)
    {
        // Create process instance
        var erosion = new ErosionProcess(deltaOctree);

        // Set up initial terrain
        var rockPositions = new[]
        {
            new Vector3(10, 10, 10),
            new Vector3(11, 10, 10),
            new Vector3(12, 10, 10)
        };

        foreach (var pos in rockPositions)
        {
            deltaOctree.WriteVoxel(pos, new MaterialData(MaterialId.Rock, 2700f, 6.0f));
        }

        Console.WriteLine($"Initial terrain: {rockPositions.Length} rock positions");

        // Create context (Parameter Object pattern)
        var context = new GeologicalProcessContext
        {
            Positions = rockPositions,
            Intensity = 0.5f // 50% erosion
        };

        // Execute template method
        Console.WriteLine("Executing erosion process...");
        erosion.Execute(context);

        // Check results
        foreach (var pos in rockPositions)
        {
            var material = deltaOctree.ReadVoxel(pos);
            Console.WriteLine($"  Position {pos}: {material}");
        }

        Console.WriteLine($"Spatial Locality Factor: {erosion.GetSpatialLocalityFactor() * 100}%");
    }

    /// <summary>
    /// Example 2: Using the high-level orchestration layer
    /// </summary>
    static void OrchestrationLayerUsage(DeltaPatchOctree deltaOctree)
    {
        var adapter = new GeomorphologicalOctreeAdapter(deltaOctree);

        // Scenario: Volcanic eruption followed by erosion
        Console.WriteLine("Simulating volcanic eruption...");
        
        var volcanoCenter = new Vector3(20, 20, 20);
        var lavaMaterial = new MaterialData(MaterialId.Lava, 3100f, 1.0f);
        
        adapter.ApplyVolcanicIntrusion(volcanoCenter, 3.0f, lavaMaterial);
        Console.WriteLine($"  Volcano at {volcanoCenter} with radius 3.0");

        // Check material at center
        var centerMaterial = adapter.GetMaterial(volcanoCenter);
        Console.WriteLine($"  Center material: {centerMaterial}");

        // Apply weathering to surface
        Console.WriteLine("\nApplying weathering to cooled lava...");
        var surfacePositions = new[] { volcanoCenter };
        var weatheredMaterial = new MaterialData(MaterialId.Basalt, 2900f, 6.0f);
        
        adapter.ApplyWeathering(surfacePositions, weatheredMaterial);
        
        var weatheredResult = adapter.GetMaterial(volcanoCenter);
        Console.WriteLine($"  Weathered material: {weatheredResult}");

        Console.WriteLine($"\nPending changes in delta overlay: {adapter.GetPendingChangeCount()}");
    }

    /// <summary>
    /// Example 3: Demonstrates proper layer communication (no skipping)
    /// </summary>
    static void LayerSeparationDemo(DeltaPatchOctree deltaOctree)
    {
        Console.WriteLine("Layer Communication Flow:");
        Console.WriteLine("  1. Orchestration Layer (Adapter)");
        Console.WriteLine("     ↓");
        Console.WriteLine("  2. Process Layer (Concrete Process)");
        Console.WriteLine("     ↓");
        Console.WriteLine("  3. Storage Layer (DeltaPatchOctree)");
        Console.WriteLine("     ↓");
        Console.WriteLine("  4. Base Storage (OptimizedOctreeNode)");

        var adapter = new GeomorphologicalOctreeAdapter(deltaOctree);
        
        // Orchestration layer calls process layer
        Console.WriteLine("\nExecuting through proper layers:");
        var testPosition = new Vector3(30, 30, 30);
        
        // Step 1: Orchestration → Process
        Console.WriteLine("  Adapter.ApplyErosion() called");
        adapter.ApplyErosion(new[] { testPosition }, 1.0f);
        
        // Step 2: Process → Storage (happens internally)
        Console.WriteLine("  → ErosionProcess.Execute() delegated");
        
        // Step 3: Storage → Base (happens internally)
        Console.WriteLine("  → DeltaPatchOctree.WriteVoxel() called");
        Console.WriteLine("  → OptimizedOctreeNode updated");

        var result = adapter.GetMaterial(testPosition);
        Console.WriteLine($"\nResult: {result}");
    }

    /// <summary>
    /// Example 4: Using cross-cutting utilities (composition pattern)
    /// </summary>
    static void CrossCuttingConcernsDemo(DeltaPatchOctree deltaOctree)
    {
        // Logging example
        var logger = new GeologicalProcessLogger("TestProcess");
        logger.StartOperation();

        // Simulate some work
        var deposition = new DepositionProcess(deltaOctree);
        var positions = Enumerable.Range(0, 100)
            .Select(i => new Vector3(40 + i, 40, 40))
            .ToList();

        var context = new GeologicalProcessContext
        {
            Positions = positions,
            TargetMaterial = new MaterialData(MaterialId.Sand, 1600f, 2.5f)
        };

        deposition.Execute(context);

        foreach (var _ in positions)
        {
            logger.LogPositionProcessed();
        }

        logger.EndOperation();
        var metrics = logger.GetMetrics();
        Console.WriteLine($"Performance Metrics: {metrics}");

        // Caching example
        var cache = new GeologicalProcessCache(maxEntries: 1000);
        
        Console.WriteLine("\nCaching demonstration:");
        cache.Set("test_key", "test_value");
        
        if (cache.TryGet<string>("test_key", out var value))
        {
            Console.WriteLine($"  Cache hit: {value}");
        }

        if (!cache.TryGet<string>("nonexistent", out _))
        {
            Console.WriteLine("  Cache miss for nonexistent key (expected)");
        }
    }
}

/// <summary>
/// Example of extending the system with a new geological process
/// Demonstrates how easy it is to add new functionality
/// </summary>
public class GlacialErosionProcess : GeologicalProcessBase
{
    public GlacialErosionProcess(DeltaPatchOctree deltaOctree) 
        : base(deltaOctree)
    {
    }

    protected override string GetProcessName() => "Glacial Erosion";

    public override double GetSpatialLocalityFactor() => 0.75; // 75% locality

    /// <summary>
    /// Only process in cold regions (simplified example)
    /// </summary>
    protected override IEnumerable<Vector3> FilterPositions(
        IEnumerable<Vector3> positions,
        GeologicalProcessContext context)
    {
        // Filter for high altitude (cold) positions
        return positions.Where(p => p.Y > 100);
    }

    /// <summary>
    /// Glacial erosion is slower but more powerful than water erosion
    /// </summary>
    protected override IEnumerable<(Vector3 position, MaterialData material)> CalculateMaterialChanges(
        IEnumerable<Vector3> positions,
        GeologicalProcessContext context)
    {
        var updates = new List<(Vector3, MaterialData)>();
        var intensity = context.Intensity;

        foreach (var position in positions)
        {
            var current = GetCurrentMaterial(position);

            // Glacial erosion can erode harder materials
            if (current.Hardness < 8.0f) // Less than diamond hardness
            {
                // Create glacial till (mixed sediment)
                var till = new MaterialData(MaterialId.Gravel, 1800f, 3.0f);
                updates.Add((position, till));
            }
        }

        return updates;
    }
}
