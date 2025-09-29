using System;
using System.Collections.Generic;

namespace BlueMarble.SpatialStorage.Octree
{
    /// <summary>
    /// Represents material properties for octree nodes
    /// </summary>
    public class MaterialData : IEquatable<MaterialData>
    {
        public MaterialId Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public float Density { get; set; }
        public MaterialProperties Properties { get; set; } = new();

        /// <summary>
        /// Default ocean material for global fallback
        /// </summary>
        public static MaterialData DefaultOcean => new MaterialData
        {
            Id = MaterialId.Ocean,
            Name = "Ocean",
            Density = 1.025f,
            Properties = new MaterialProperties { IsLiquid = true, Temperature = 15.0f }
        };

        public bool Equals(MaterialData? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Id == other.Id && 
                   Name == other.Name && 
                   Math.Abs(Density - other.Density) < 0.001f;
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as MaterialData);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Name, Density);
        }

        public static bool operator ==(MaterialData? left, MaterialData? right)
        {
            return EqualityComparer<MaterialData>.Default.Equals(left, right);
        }

        public static bool operator !=(MaterialData? left, MaterialData? right)
        {
            return !(left == right);
        }
    }

    /// <summary>
    /// Material identifier enumeration
    /// </summary>
    public enum MaterialId
    {
        Air = 0,
        Ocean = 1,
        Rock = 2,
        Dirt = 3,
        Sand = 4,
        Ice = 5,
        Vegetation = 6
    }

    /// <summary>
    /// Additional material properties for simulation
    /// </summary>
    public class MaterialProperties
    {
        public bool IsLiquid { get; set; }
        public bool IsSolid { get; set; } = true;
        public float Temperature { get; set; }
        public float Conductivity { get; set; }
        public float Porosity { get; set; }
    }
}