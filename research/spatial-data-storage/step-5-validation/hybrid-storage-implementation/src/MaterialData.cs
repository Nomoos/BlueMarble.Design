using System;

namespace BlueMarble.SpatialStorage
{
    /// <summary>
    /// Represents geological material properties for spatial storage
    /// Optimized for compact memory representation with 80-95% reduction in homogeneous regions
    /// </summary>
    public enum MaterialId : byte
    {
        Ocean = 0,
        Air = 1,
        Sand = 2,
        Clay = 3,
        Silt = 4,
        Rock = 5,
        Granite = 6,
        Limestone = 7,
        Sandstone = 8,
        Shale = 9,
        Basalt = 10,
        Dirt = 11,
        Vegetation = 12,
        Ice = 13,
        Snow = 14,
        Lava = 15
    }

    /// <summary>
    /// Extended material properties with density and hardness
    /// </summary>
    public struct MaterialData
    {
        public MaterialId Id { get; set; }
        public double Density { get; set; }      // kg/m³
        public double Hardness { get; set; }     // Mohs scale
        public double Homogeneity { get; set; }  // 0.0 to 1.0

        public MaterialData(MaterialId id, double density, double hardness, double homogeneity = 1.0)
        {
            Id = id;
            Density = density;
            Hardness = hardness;
            Homogeneity = homogeneity;
        }

        /// <summary>
        /// Get default material properties
        /// </summary>
        public static MaterialData GetDefault(MaterialId id)
        {
            return id switch
            {
                MaterialId.Ocean => new MaterialData(id, 1025, 0, 1.0),
                MaterialId.Air => new MaterialData(id, 1.225, 0, 1.0),
                MaterialId.Sand => new MaterialData(id, 1600, 2.5, 0.9),
                MaterialId.Clay => new MaterialData(id, 2000, 2.0, 0.85),
                MaterialId.Rock => new MaterialData(id, 2700, 6.0, 0.7),
                MaterialId.Granite => new MaterialData(id, 2750, 7.0, 0.8),
                MaterialId.Limestone => new MaterialData(id, 2600, 3.0, 0.75),
                MaterialId.Sandstone => new MaterialData(id, 2300, 4.0, 0.7),
                _ => new MaterialData(id, 2000, 5.0, 0.5)
            };
        }
    }
}
