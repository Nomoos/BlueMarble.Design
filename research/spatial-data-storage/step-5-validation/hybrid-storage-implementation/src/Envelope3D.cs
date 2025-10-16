using System;

namespace BlueMarble.SpatialStorage
{
    /// <summary>
    /// 3D axis-aligned bounding box with efficient intersection testing
    /// Used for spatial queries and octree bounds
    /// </summary>
    public struct Envelope3D
    {
        public double MinX { get; set; }
        public double MinY { get; set; }
        public double MinZ { get; set; }
        public double MaxX { get; set; }
        public double MaxY { get; set; }
        public double MaxZ { get; set; }

        public Envelope3D(double minX, double minY, double minZ, double maxX, double maxY, double maxZ)
        {
            MinX = minX;
            MinY = minY;
            MinZ = minZ;
            MaxX = maxX;
            MaxY = maxY;
            MaxZ = maxZ;
        }

        public Envelope3D(Vector3 min, Vector3 max)
        {
            MinX = min.X;
            MinY = min.Y;
            MinZ = min.Z;
            MaxX = max.X;
            MaxY = max.Y;
            MaxZ = max.Z;
        }

        public double Width => MaxX - MinX;
        public double Height => MaxY - MinY;
        public double Depth => MaxZ - MinZ;
        public double Volume => Width * Height * Depth;

        public Vector3 Center => new Vector3(
            (MinX + MaxX) / 2,
            (MinY + MaxY) / 2,
            (MinZ + MaxZ) / 2
        );

        /// <summary>
        /// Check if this envelope contains a point
        /// </summary>
        public bool Contains(Vector3 point)
        {
            return point.X >= MinX && point.X <= MaxX &&
                   point.Y >= MinY && point.Y <= MaxY &&
                   point.Z >= MinZ && point.Z <= MaxZ;
        }

        /// <summary>
        /// Check if this envelope intersects another envelope
        /// </summary>
        public bool Intersects(Envelope3D other)
        {
            return !(other.MinX > MaxX || other.MaxX < MinX ||
                    other.MinY > MaxY || other.MaxY < MinY ||
                    other.MinZ > MaxZ || other.MaxZ < MinZ);
        }

        /// <summary>
        /// Get the intersection of two envelopes
        /// </summary>
        public Envelope3D? Intersection(Envelope3D other)
        {
            if (!Intersects(other))
                return null;

            return new Envelope3D(
                Math.Max(MinX, other.MinX),
                Math.Max(MinY, other.MinY),
                Math.Max(MinZ, other.MinZ),
                Math.Min(MaxX, other.MaxX),
                Math.Min(MaxY, other.MaxY),
                Math.Min(MaxZ, other.MaxZ)
            );
        }

        /// <summary>
        /// Expand envelope to include a point
        /// </summary>
        public void ExpandToInclude(Vector3 point)
        {
            if (point.X < MinX) MinX = point.X;
            if (point.X > MaxX) MaxX = point.X;
            if (point.Y < MinY) MinY = point.Y;
            if (point.Y > MaxY) MaxY = point.Y;
            if (point.Z < MinZ) MinZ = point.Z;
            if (point.Z > MaxZ) MaxZ = point.Z;
        }
    }

    /// <summary>
    /// Simple 3D vector struct
    /// </summary>
    public struct Vector3
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }

        public Vector3(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public double Length => Math.Sqrt(X * X + Y * Y + Z * Z);

        public static Vector3 operator +(Vector3 a, Vector3 b) =>
            new Vector3(a.X + b.X, a.Y + b.Y, a.Z + b.Z);

        public static Vector3 operator -(Vector3 a, Vector3 b) =>
            new Vector3(a.X - b.X, a.Y - b.Y, a.Z - b.Z);

        public static Vector3 operator *(Vector3 v, double scalar) =>
            new Vector3(v.X * scalar, v.Y * scalar, v.Z * scalar);
    }
}
