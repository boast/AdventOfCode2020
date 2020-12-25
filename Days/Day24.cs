using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdventOfCode2020.Days
{
    internal class Day24 : Day
    {
        // Using "Axial Coordinate" system for the hexagonal grid.
        // It allows standard operations (+-*/) from cartesian coordinates.
        private static readonly Dictionary<NeighbourDirection, Point> Neighbours = new()
        {
            {NeighbourDirection.E, new Point(1)},
            {NeighbourDirection.Se, new Point(0, 1)},
            {NeighbourDirection.Sw, new Point(-1, 1)},
            {NeighbourDirection.W, new Point(-1)},
            {NeighbourDirection.Nw, new Point(0, -1)},
            {NeighbourDirection.Ne, new Point(1, -1)},
        };

        private static IEnumerable<Point> GetNeighbours(Point point)
            => Neighbours.Values.Select(neighbour => point + neighbour);

        private static HashSet<Point> ParseTiles(IEnumerable<string> input)
        {
            var tiles = new HashSet<Point>();

            foreach (var directions in input)
            {
                var tile = Point.Origin;
                string buffer = string.Empty;

                foreach (char direction in directions)
                {
                    buffer += direction;

                    if (!Enum.TryParse(buffer, true, out NeighbourDirection neighbourDirection))
                    {
                        continue;
                    }

                    tile += Neighbours[neighbourDirection];
                    buffer = string.Empty;
                }

                if (!tiles.Add(tile))
                {
                    tiles.Remove(tile);
                }
            }

            return tiles;
        }

        /// <inheritdoc />
        protected override async Task<object> Solve01Async(IEnumerable<string> input)
        {
            var tiles = ParseTiles(input);
            return await Task.FromResult(tiles.Count);
        }

        /// <inheritdoc />
        protected override async Task<object> Solve02Async(IEnumerable<string> input)
        {
            var tiles = ParseTiles(input);
            int days = 100;

            while (days-- > 0)
            {
                var nextCandidates = tiles
                    .Concat(tiles.SelectMany(GetNeighbours))
                    .ToHashSet();

                var nextTiles = new HashSet<Point>();

                foreach (var nextCandidate in nextCandidates)
                {
                    // We iterate manually to "break" the iteration fast
                    using var neighbours = GetNeighbours(nextCandidate).GetEnumerator();
                    int count = 0;

                    while (count <= 2 && neighbours.MoveNext())
                    {
                        count += tiles.Contains(neighbours.Current) ? 1 : 0;
                    }

                    switch (tiles.Contains(nextCandidate))
                    {
                        case true when count == 1 || count == 2:
                        case false when count == 2:
                            nextTiles.Add(nextCandidate);
                            break;
                    }
                }

                tiles = nextTiles;
            }

            return await Task.FromResult(tiles.Count);
        }

        private enum NeighbourDirection
        {
            E,
            Se,
            Sw,
            W,
            Nw,
            Ne,
        }
    }
}