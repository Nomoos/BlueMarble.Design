using System;
using System.Collections.Generic;

namespace BlueMarble.SpatialStorage.Octree
{
    /// <summary>
    /// BlueMarble-specific material node implementation that addresses the specific requirement:
    /// "if there is air in 90% 16×16m material this cell will be air and if child has different material then this child will exist"
    /// </summary>
    public class BlueMarbleMaterialNode : MaterialInheritanceNode
    {
        public const double HOMOGENEITY_THRESHOLD = 0.9; // 90% threshold as specified
        public double CellSize { get; set; }
        public Vector3 Center { get; set; }

        /// <summary>
        /// Default material for this node (primary material)
        /// </summary>
        public MaterialId PrimaryMaterial { get; set; }

        /// <summary>
        /// Material overrides for specific positions within this node
        /// </summary>
        public Dictionary<Vector3, MaterialId> MaterialOverrides { get; set; } = new();

        /// <summary>
        /// Implements BlueMarble's specific inheritance rule:
        /// 16x16m air cell with 8x8m air child containing 4x4m dirt child
        /// </summary>
        public MaterialId GetMaterialAtPosition(Vector3 position, int targetLOD)
        {
            // Check for explicit override at this position
            if (MaterialOverrides.TryGetValue(position, out var explicitMaterial))
                return explicitMaterial;

            // Calculate homogeneity for this cell
            var cellBounds = CalculateCellBounds(position, targetLOD);
            var materialsInCell = SampleMaterialsInBounds(cellBounds);
            var homogeneity = CalculateHomogeneity(materialsInCell);

            if (homogeneity >= HOMOGENEITY_THRESHOLD)
            {
                // Use dominant material for homogeneous regions
                return GetDominantMaterial(materialsInCell);
            }
            else
            {
                // Heterogeneous - check children or use default
                return GetChildMaterial(position, targetLOD + 1) ?? PrimaryMaterial;
            }
        }

        /// <summary>
        /// Example implementation of the specified scenario:
        /// 16x16m cell (Level 1) - 90% air with child containing dirt
        /// </summary>
        public static BlueMarbleMaterialNode CreateBlueMarbleExample()
        {
            // 16x16m cell (Level 1) - 90% air
            var root = new BlueMarbleMaterialNode
            {
                PrimaryMaterial = MaterialId.Air,
                CellSize = 16.0,
                Center = new Vector3(0, 0, 100), // Above sea level
                Level = 1,
                Bounds = new BoundingBox
                {
                    Min = new Vector3(-8, -8, 92),
                    Max = new Vector3(8, 8, 108)
                }
            };

            // Create children for demonstration (normally would be based on homogeneity)
            // This demonstrates the inheritance scenario described in the research
            root.Children = new MaterialInheritanceNode[8];

                // 8x8m child (Level 2) - still mostly air
                root.Children[0] = new BlueMarbleMaterialNode
                {
                    PrimaryMaterial = MaterialId.Air,
                    CellSize = 8.0,
                    Center = new Vector3(4, 4, 100),
                    Level = 2,
                    Parent = root,
                    Bounds = new BoundingBox
                    {
                        Min = new Vector3(0, 0, 96),
                        Max = new Vector3(8, 8, 104)
                    }
                };

                // This 8x8m cell contains dirt, so create its children
                if (root.Children[0] is BlueMarbleMaterialNode childNode)
                {
                    childNode.Children = new MaterialInheritanceNode[8];

                    // 4x4m child (Level 3) - contains dirt
                    childNode.Children[0] = new BlueMarbleMaterialNode
                {
                    PrimaryMaterial = MaterialId.Dirt,
                    CellSize = 4.0,
                    Center = new Vector3(2, 2, 96), // Slightly below air level
                    Level = 3,
                    Parent = root.Children[0],
                    // Set explicit material since it differs from parent (air)
                    ExplicitMaterial = new MaterialData
                    {
                        Id = MaterialId.Dirt,
                        Name = "Dirt",
                        Density = 1.5f
                    },
                    Bounds = new BoundingBox
                    {
                        Min = new Vector3(0, 0, 94),
                        Max = new Vector3(4, 4, 98)
                    }
                };
            }

            return root;
        }

        /// <summary>
        /// Calculate memory savings compared to explicit storage
        /// </summary>
        public MemorySavingsReport CalculateMemorySavings()
        {
            var explicitMemory = CalculateExplicitMemoryUsage();
            var inheritanceMemory = CalculateMemoryFootprint();
            var savings = explicitMemory - inheritanceMemory;
            var savingsPercentage = (double)savings / explicitMemory * 100;

            return new MemorySavingsReport
            {
                ExplicitMemoryBytes = explicitMemory,
                InheritanceMemoryBytes = inheritanceMemory,
                SavingsBytes = savings,
                SavingsPercentage = savingsPercentage,
                NodesWithInheritance = CountInheritanceNodes(),
                TotalNodes = CountTotalNodes()
            };
        }

        private BoundingBox CalculateCellBounds(Vector3 position, int lod)
        {
            var cellSize = CellSize / Math.Pow(2, lod);
            var halfSize = cellSize / 2;

            return new BoundingBox
            {
                Min = new Vector3((float)(position.X - halfSize), (float)(position.Y - halfSize), (float)(position.Z - halfSize)),
                Max = new Vector3((float)(position.X + halfSize), (float)(position.Y + halfSize), (float)(position.Z + halfSize))
            };
        }

        private Dictionary<MaterialId, int> SampleMaterialsInBounds(BoundingBox bounds)
        {
            // Simplified sampling - in real implementation would sample the actual terrain
            var materials = new Dictionary<MaterialId, int>();

            // Sample based on elevation and position
            var elevation = bounds.Center.Z;
            if (elevation > 50) // Above ground
            {
                materials[MaterialId.Air] = 90;
                materials[MaterialId.Dirt] = 10;
            }
            else if (elevation > 0) // Ground level
            {
                materials[MaterialId.Dirt] = 70;
                materials[MaterialId.Rock] = 20;
                materials[MaterialId.Air] = 10;
            }
            else // Below sea level
            {
                materials[MaterialId.Ocean] = 95;
                materials[MaterialId.Sand] = 5;
            }

            return materials;
        }

        private double CalculateHomogeneity(Dictionary<MaterialId, int> materials)
        {
            if (materials.Count <= 1) return 1.0;

            var total = 0;
            var max = 0;
            foreach (var count in materials.Values)
            {
                total += count;
                if (count > max) max = count;
            }

            return total > 0 ? (double)max / total : 1.0;
        }

        private MaterialId GetDominantMaterial(Dictionary<MaterialId, int> materials)
        {
            var max = 0;
            var dominant = PrimaryMaterial;

            foreach (var kvp in materials)
            {
                if (kvp.Value > max)
                {
                    max = kvp.Value;
                    dominant = kvp.Key;
                }
            }

            return dominant;
        }

        private MaterialId? GetChildMaterial(Vector3 position, int lod)
        {
            // Simplified child lookup - would traverse to appropriate child node
            return null;
        }

        private static double CalculateAirPercentage(Vector3 center, double cellSize)
        {
            // Simplified calculation based on elevation
            return center.Z > 50 ? 0.95 : 0.1; // 95% air if above ground, 10% otherwise
        }

        private long CalculateExplicitMemoryUsage()
        {
            // Calculate memory if every node stored explicit material
            var nodeCount = CountTotalNodes();
            var materialSize = 64; // Estimated bytes per MaterialData
            return nodeCount * materialSize;
        }

        private int CountInheritanceNodes()
        {
            int count = ExplicitMaterial == null ? 1 : 0;

            if (Children != null)
            {
                foreach (var child in Children)
                {
                    if (child is BlueMarbleMaterialNode bmChild)
                        count += bmChild.CountInheritanceNodes();
                }
            }

            return count;
        }

        private int CountTotalNodes()
        {
            int count = 1;

            if (Children != null)
            {
                foreach (var child in Children)
                {
                    if (child is BlueMarbleMaterialNode bmChild)
                        count += bmChild.CountTotalNodes();
                }
            }

            return count;
        }
    }

    /// <summary>
    /// Report showing memory savings from inheritance optimization
    /// </summary>
    public class MemorySavingsReport
    {
        public long ExplicitMemoryBytes { get; set; }
        public long InheritanceMemoryBytes { get; set; }
        public long SavingsBytes { get; set; }
        public double SavingsPercentage { get; set; }
        public int NodesWithInheritance { get; set; }
        public int TotalNodes { get; set; }

        public override string ToString()
        {
            return $"Memory Savings: {SavingsPercentage:F1}% " +
                   $"({SavingsBytes:N0} bytes saved, " +
                   $"{NodesWithInheritance}/{TotalNodes} nodes using inheritance)";
        }
    }
}