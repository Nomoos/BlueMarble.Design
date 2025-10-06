using System.Numerics;

namespace BlueMarble.SpatialData;

/// <summary>
/// Represents geological material with physical properties
/// Core data structure for octree voxel storage
/// </summary>
public struct MaterialData : IEquatable<MaterialData>
{
    /// <summary>
    /// Material type identifier
    /// </summary>
    public MaterialId MaterialType { get; set; }

    /// <summary>
    /// Material density in kg/m³
    /// </summary>
    public float Density { get; set; }

    /// <summary>
    /// Material hardness on Mohs scale (0-10)
    /// </summary>
    public float Hardness { get; set; }

    /// <summary>
    /// Additional material properties as packed bits
    /// </summary>
    public uint Properties { get; set; }

    public MaterialData(MaterialId materialType, float density, float hardness, uint properties = 0)
    {
        MaterialType = materialType;
        Density = density;
        Hardness = hardness;
        Properties = properties;
    }

    public bool Equals(MaterialData other)
    {
        return MaterialType == other.MaterialType &&
               Math.Abs(Density - other.Density) < 0.001f &&
               Math.Abs(Hardness - other.Hardness) < 0.001f &&
               Properties == other.Properties;
    }

    public override bool Equals(object? obj) => obj is MaterialData other && Equals(other);

    public override int GetHashCode() => HashCode.Combine(MaterialType, Density, Hardness, Properties);

    public static bool operator ==(MaterialData left, MaterialData right) => left.Equals(right);
    public static bool operator !=(MaterialData left, MaterialData right) => !left.Equals(right);

    /// <summary>
    /// Default material for ocean/water
    /// </summary>
    public static MaterialData DefaultOcean => new(MaterialId.Water, 1000f, 0f);

    /// <summary>
    /// Default material for air
    /// </summary>
    public static MaterialData DefaultAir => new(MaterialId.Air, 1.225f, 0f);

    public override string ToString() => $"{MaterialType} (ρ={Density:F1}, H={Hardness:F1})";
}

/// <summary>
/// Material type identifiers
/// </summary>
public enum MaterialId : ushort
{
    Air = 0,
    Water = 1,
    Soil = 2,
    Sand = 3,
    Rock = 4,
    Granite = 5,
    Basalt = 6,
    Limestone = 7,
    Sandstone = 8,
    Clay = 9,
    Gravel = 10,
    Ice = 11,
    Snow = 12,
    Lava = 13,
    Magma = 14,
    Dirt = 15
}
