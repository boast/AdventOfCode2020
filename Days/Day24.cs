using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdventOfCode2020.Days
{
    internal class Day24 : Day
    {
        private enum TileSide
        {
            White,
            Black,
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
        
        // Prevent multiple enumerations of the points only
        private static readonly IEnumerable<Point> NeighbourPoints
            = Neighbours.Select(neighbour => neighbour.Value).ToList();

        private static IEnumerable<Point> GetNeighbours(Point point)
            => NeighbourPoints.Select(neighbour => point + neighbour);

        private static Dictionary<Point, TileSide> ParseTiles(IEnumerable<string> input)
        {
            var tiles = new Dictionary<Point, TileSide>
            {
                {new Point(), TileSide.White},
            };
            
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
                
                tiles[tile] = tiles.ContainsKey(tile)&& tiles[tile] == TileSide.Black ? TileSide.White : TileSide.Black;
            }

            return tiles;
        }
        
        /// <inheritdoc />
        protected override async Task<object> Solve01Async(IEnumerable<string> input)
        {
            var tiles = ParseTiles(input);
            return await Task.FromResult(tiles.Count(tile => tile.Value == TileSide.Black));
        }

        /// <inheritdoc />
        protected override async Task<object> Solve02Async(IEnumerable<string> input)
        {
            var tiles = ParseTiles(input);
            int days = 100;
            
            while (days-- > 0)
            {
                var blackTilePoints = tiles
                    .Where(tile => tile.Value == TileSide.Black)
                    .Select(tile => tile.Key)
                    .ToList();
                
                var nextTilePoints = blackTilePoints
                    .Concat(blackTilePoints.SelectMany(GetNeighbours))
                    .ToHashSet();
                
                var nextTiles = new Dictionary<Point, TileSide>();

                foreach (var nextTilePoint in nextTilePoints)
                {
                    // We iterate manually to "break" the iteration fast
                    using var neighbours = GetNeighbours(nextTilePoint).GetEnumerator();
                    int blackCount = 0;

                    while (blackCount <= 2 && neighbours.MoveNext())
                    {
                        blackCount += tiles.ContainsKey(neighbours.Current) && tiles[neighbours.Current] == TileSide.Black ? 1 : 0;
                    }

                    var previousSide = tiles.ContainsKey(nextTilePoint) ? tiles[nextTilePoint] : TileSide.White;

                    nextTiles[nextTilePoint] = previousSide switch
                    {
                        TileSide.White when blackCount == 2 => TileSide.Black,
                        TileSide.Black when blackCount == 0 || blackCount > 2 => TileSide.White,
                        _ => previousSide,
                    };
                }

                tiles = nextTiles;
            }
            
            return await Task.FromResult(tiles.Count(tile => tile.Value == TileSide.Black));
        }
    }
}