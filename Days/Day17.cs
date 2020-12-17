using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace AdventOfCode2020.Days
{
    internal class Day17 : Day
    {
        private static IEnumerable<IPoint<T>> Parse<T>(IEnumerable<string> input) where T : IPoint<T>
        {
            var inputList = input.ToList();
            for (int y = 0; y < inputList.Count; y++)
            {
                for (int x = 0; x < inputList[y].Length; x++)
                {
                    if (inputList[x][y] == '#')
                    {
                        yield return typeof(T) == typeof(Point3D)
                            ? (IPoint<T>) new Point3D(x, y, 0)
                            : (IPoint<T>) new Point4D(x, y, 0, 0);
                    }
                }
            }
        }

        /// <inheritdoc />
        protected override async Task<long> Solve01Async(IEnumerable<string> input)
        {
            var state = Parse<Point3D>(input);
            int cycles = 6;

            while (cycles-- > 0)
            {
                state = IPoint<Point3D>.Cycle(state);
            }

            return await Task.FromResult(state.Count());
        }

        /// <inheritdoc />
        protected override async Task<long> Solve02Async(IEnumerable<string> input)
        {
            var state = Parse<Point4D>(input);
            int cycles = 6;

            while (cycles-- > 0)
            {
                state = IPoint<Point4D>.Cycle(state);
            }

            return await Task.FromResult(state.Count());
        }

        private interface IPoint<T> where T : IPoint<T>
        {
            public IEnumerable<IPoint<T>> Neighbours { get; }

            [SuppressMessage("ReSharper", "PossibleMultipleEnumeration", Justification = "Performance")]
            public static ISet<IPoint<T>> Cycle(IEnumerable<IPoint<T>> state)
                => state
                    .SelectMany(point => point.Neighbours)
                    .ToHashSet()
                    .Where(point =>
                        {
                            int activeNeighborCount = 0;

                            foreach (var neighbor in point.Neighbours)
                            {
                                if (state.Contains(neighbor))
                                {
                                    activeNeighborCount++;
                                }

                                if (activeNeighborCount > 3)
                                {
                                    return false;
                                }
                            }

                            return state.Contains(point) ? activeNeighborCount is 2 or 3 : activeNeighborCount is 3;
                        }
                    )
                    .ToHashSet();
        }

        private record Point3D(int X, int Y, int Z) : IPoint<Point3D>
        {
            public IEnumerable<IPoint<Point3D>> Neighbours
            {
                get
                {
                    for (int z = -1; z <= 1; z++)
                    for (int y = -1; y <= 1; y++)
                    for (int x = -1; x <= 1; x++)
                    {
                        if (x == 0 && y == 0 && z == 0)
                        {
                            continue;
                        }

                        yield return new Point3D(x + X, y + Y, z + Z);
                    }
                }
            }
        }

        private record Point4D(int X, int Y, int Z, int W) : IPoint<Point4D>
        {
            public IEnumerable<IPoint<Point4D>> Neighbours
            {
                get
                {
                    for (int z = -1; z <= 1; z++)
                    for (int y = -1; y <= 1; y++)
                    for (int x = -1; x <= 1; x++)
                    for (int w = -1; w <= 1; w++)
                    {
                        if (x == 0 && y == 0 && z == 0 && w == 0)
                        {
                            continue;
                        }

                        yield return new Point4D(x + X, y + Y, z + Z, w + W);
                    }
                }
            }
        }
    }
}