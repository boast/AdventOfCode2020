using System;

namespace AdventOfCode2020
{
    public readonly struct Point : IEquatable<Point>
    {
        public int X { get; }
        public int Y { get; }
        public Point(int x = 0, int y = 0) => (X, Y) = (x, y);
        public void Deconstruct(out int x, out int y) => (x, y) = (X, Y);

        public static readonly Point Origin = new();

        public int ManhattanDistance(Point otherPoint) => Math.Abs(X - otherPoint.X) + Math.Abs(Y - otherPoint.Y);

        public static Point operator +(Point point) => point;
        public static Point operator -(Point point) => new(point.X * -1, point.Y * -1);

        public static Point operator +(Point point, Point otherPoint)
            => new(point.X + otherPoint.X, point.Y + otherPoint.Y);

        public static Point operator -(Point point, Point otherPoint)
            => new(point.X - otherPoint.X, point.Y - otherPoint.Y);

        public static Point operator *(Point point, int value) => new(point.X * value, point.Y * value);
        public static Point operator *(int value, Point point) => new(point.X * value, point.Y * value);

        public static Point operator /(Point point, int value)
            => value == 0 ? throw new DivideByZeroException() : new Point(point.X / value, point.Y / value);
        public static Point operator /(int value, Point point)
            => point.X == 0 || point.Y == 0 ? throw new DivideByZeroException() : new Point(value / point.X, value / point.Y);

        public static bool operator ==(Point point, Point other) => point.Equals(other);
        public static bool operator !=(Point point, Point other) => !point.Equals(other);

        /// <inheritdoc />
        public bool Equals(Point other) => X == other.X && Y == other.Y;

        /// <inheritdoc />
        public override bool Equals(object? obj) => obj is Point other && Equals(other);

        /// <inheritdoc />
        public override int GetHashCode() => HashCode.Combine(X, Y);

        /// <inheritdoc />
        public override string ToString() => $"({X},{Y})";
    }
}