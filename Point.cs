using System;

namespace AdventOfCode2020
{
    public readonly struct Point
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

        /// <inheritdoc />
        public override string ToString() => $"({X},{Y})";
    }
}