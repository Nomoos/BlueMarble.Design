using System;

namespace BlueMarble.SpatialStorage.Research
{
    /// <summary>
    /// Material identifier for geological materials in BlueMarble
    /// </summary>
    public enum MaterialId
    {
        Air = 0,
        Water = 1,
        Rock = 2,
        Dirt = 3,
        Sand = 4,
        Clay = 5,
        Gravel = 6,
        Limestone = 7,
        Granite = 8
    }

    /// <summary>
    /// Physical properties of a material optimized for geological processes
    /// </summary>
    public class MaterialProperties
    {
        public double Density { get; set; }           // kg/m³
        public double Hardness { get; set; }          // Mohs scale
        public double Porosity { get; set; }          // 0.0-1.0
        public double Permeability { get; set; }      // darcy units
        public double ThermalConductivity { get; set; } // W/(m·K)
        
        public MaterialProperties(double density, double hardness, double porosity, 
                                  double permeability, double thermalConductivity)
        {
            Density = density;
            Hardness = hardness;
            Porosity = porosity;
            Permeability = permeability;
            ThermalConductivity = thermalConductivity;
        }
    }

    /// <summary>
    /// Complete material data with properties
    /// </summary>
    public class MaterialData
    {
        public MaterialId Id { get; set; }
        public string Name { get; set; }
        public MaterialProperties Properties { get; set; }
        
        public MaterialData(MaterialId id, string name, MaterialProperties properties)
        {
            Id = id;
            Name = name;
            Properties = properties;
        }
        
        // Predefined materials optimized for geological processes
        public static readonly MaterialData Air = new MaterialData(
            MaterialId.Air, "Air", 
            new MaterialProperties(1.225, 0, 1.0, 1000, 0.024));
            
        public static readonly MaterialData Water = new MaterialData(
            MaterialId.Water, "Water",
            new MaterialProperties(1000, 0, 1.0, 1000, 0.6));
            
        public static readonly MaterialData Rock = new MaterialData(
            MaterialId.Rock, "Rock",
            new MaterialProperties(2700, 7, 0.02, 0.001, 2.5));
            
        public static readonly MaterialData Dirt = new MaterialData(
            MaterialId.Dirt, "Dirt",
            new MaterialProperties(1500, 2, 0.4, 10, 0.8));
            
        public static readonly MaterialData Sand = new MaterialData(
            MaterialId.Sand, "Sand",
            new MaterialProperties(1600, 3, 0.35, 100, 0.3));
            
        public static readonly MaterialData Clay = new MaterialData(
            MaterialId.Clay, "Clay",
            new MaterialProperties(1800, 2, 0.45, 0.01, 1.0));
            
        public static readonly MaterialData Gravel = new MaterialData(
            MaterialId.Gravel, "Gravel",
            new MaterialProperties(1750, 4, 0.3, 200, 1.2));
            
        public static readonly MaterialData Limestone = new MaterialData(
            MaterialId.Limestone, "Limestone",
            new MaterialProperties(2300, 4, 0.15, 1, 2.2));
            
        public static readonly MaterialData Granite = new MaterialData(
            MaterialId.Granite, "Granite",
            new MaterialProperties(2750, 7, 0.01, 0.0001, 3.0));
        
        public static MaterialData FromId(MaterialId id)
        {
            return id switch
            {
                MaterialId.Air => Air,
                MaterialId.Water => Water,
                MaterialId.Rock => Rock,
                MaterialId.Dirt => Dirt,
                MaterialId.Sand => Sand,
                MaterialId.Clay => Clay,
                MaterialId.Gravel => Gravel,
                MaterialId.Limestone => Limestone,
                MaterialId.Granite => Granite,
                _ => Air
            };
        }
    }
}
