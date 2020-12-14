using System;

namespace AdventOfCode2020
{
    public readonly struct Point : IEquatable<Point>
    {
        public int X { get; }
        public int Y { get; }
        public Point(int x = 0, int y = 0) => (X, Y) = (x, y);

        public static readonly Point Origin = new Point();

        public int ManhattanDistance(Point otherPoint) => Math.Abs(X - otherPoint.X) + Math.Abs(Y - otherPoint.Y);

        public static Point operator +(Point point) => point;
        public static Point operator -(Point point) => new Point(point.X * -1, point.Y * -1);

        public static Point operator +(Point point, Point otherPoint)
            => new Point(point.X + otherPoint.X, point.Y + otherPoint.Y);

        public static Point operator -(Point point, Point otherPoint)
            => new Point(point.X - otherPoint.X, point.Y - otherPoint.Y);

        public static Point operator *(Point point, int value) => new Point(point.X * value, point.Y * value);

        public static Point operator /(Point point, int value)
        {
            if (value == 0)
            {
                throw new DivideByZeroException();
            }

            return new Point(point.X / value, point.Y / value);
        }

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