using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace AdventOfCode2020.Days
{
    internal class Day03 : Day
    {
        private static long GetTreeBySlope(Map map, int xSlope, int ySlope)
        {
            long treeCount = 0;
            int x = 0, y = 0;

            while (y < map.YMax)
            {
                treeCount += map.GetCell(x, y) == '#' ? 1 : 0;

                x += xSlope;
                y += ySlope;
            }

            return treeCount;
        }

        /// <inheritdoc />
        protected override async Task<string> Solve01Async(IEnumerable<string> input)
            => await Task.FromResult(GetTreeBySlope(new Map(input), 3, 1).ToString());

        /// <inheritdoc />
        protected override async Task<string> Solve02Async(IEnumerable<string> input)
        {
            var map = new Map(input);

            long treeCountSlope1 = GetTreeBySlope(map, 1, 1);
            long treeCountSlope2 = GetTreeBySlope(map, 3, 1);
            long treeCountSlope3 = GetTreeBySlope(map, 5, 1);
            long treeCountSlope4 = GetTreeBySlope(map, 7, 1);
            long treeCountSlope5 = GetTreeBySlope(map, 1, 2);

            return await Task.FromResult(
                (treeCountSlope1 * treeCountSlope2 * treeCountSlope3 * treeCountSlope4 * treeCountSlope5).ToString()
            );
        }

        private class Map
        {
            private readonly Dictionary<Point, char> _map;

            public Map(IEnumerable<string> input)
            {
                var inputArray = input as string[] ?? input.ToArray();

                YMax = inputArray.Length;
                XMax = inputArray[0].Length;

                _map = new Dictionary<Point, char>();

                for (int y = 0; y < YMax; y++)
                {
                    for (int x = 0; x < XMax; x++)
                    {
                        _map.Add(new Point(x, y), inputArray[y][x]);
                    }
                }
            }

            private int XMax { get; }
            public int YMax { get; }

            public char GetCell(int x, int y)
            {
                if (y >= YMax || y < 0)
                {
                    throw new ArgumentException($"y invalid : {y}", nameof(y));
                }

                if (x < 0)
                {
                    throw new ArgumentException($"x invalid : {x}", nameof(x));
                }

                if (x >= XMax)
                {
                    x %= XMax;
                }

                return _map[new Point(x, y)];
            }
        }
    }
}