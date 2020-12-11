using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdventOfCode2020.Days
{
    internal class Day11 : Day
    {
        private const char Occupied = '#';
        private const char Empty = 'L';
        private const char Floor = '.';

        private static char[][] GetLayout(IEnumerable<string> input)
            => input.Select(line => line.ToCharArray()).ToArray();

        private static string HashLayout(IEnumerable<char[]> layout)
            => new string(layout.SelectMany(rows => rows).ToArray());

        private static bool IsSeat(char c)
            => c == Occupied || c == Empty;

        private static IEnumerable<char> GetNeighbours(int x, int y, char[][] layout, bool requireAllSeats = false)
        {
            var neighbours = new List<char>();
            var noSeats = new List<(int x, int y)>();

            int yMax = layout.Length;
            int xMax = layout[0].Length;

            for (int i = x - 1; i <= x + 1; i++)
            {
                for (int j = y - 1; j <= y + 1; j++)
                {
                    if (
                        i == x && j == y ||
                        i < 0 || i >= xMax ||
                        j < 0 || j >= yMax
                    )
                    {
                        continue;
                    }

                    if (!requireAllSeats || IsSeat(layout[j][i]))
                    {
                        neighbours.Add(layout[j][i]);
                    }
                    else
                    {
                        noSeats.Add((i, j));
                    }
                }
            }

            // Part2: If its not a seat, "walk" (delta of x and delta of y to where
            // we are currently looking) into this direction until we exit the grid
            // (while exits) or we found a seat (add to list and break).

            // ReSharper disable once UseDeconstruction
            foreach (var seat in noSeats)
            {
                int seatX = seat.x;
                int seatY = seat.y;
                int deltaX = seatX - x;
                int deltaY = seatY - y;

                while (seatX >= 0 && seatX < xMax && seatY >= 0 && seatY < yMax)
                {
                    if (IsSeat(layout[seatY][seatX]))
                    {
                        neighbours.Add(layout[seatY][seatX]);
                        break;
                    }

                    seatX += deltaX;
                    seatY += deltaY;
                }
            }

            return neighbours;
        }

        private static char[][] Step(char[][] layout, bool part2 = false)
        {
            int ySize = layout.Length;
            int xSize = layout[0].Length;

            var nextLayout = new char[ySize][];

            for (int y = 0; y < ySize; y++)
            {
                nextLayout[y] = new char[xSize];

                for (int x = 0; x < xSize; x++)
                {
                    nextLayout[y][x] = layout[y][x] switch
                    {
                        Floor => Floor,
                        Empty => GetNeighbours(x, y, layout, part2).All(neighbours => neighbours != Occupied)
                            ? Occupied
                            : Empty,
                        Occupied => GetNeighbours(x, y, layout, part2).Count(neighbours => neighbours == '#') >=
                                    (part2 ? 5 : 4)
                            ? Empty
                            : Occupied,
                        _ => throw new InvalidOperationException($"Invalid Seat: {layout[y][x]} at {y}, {x}"),
                    };
                }
            }

            return nextLayout;
        }

        /// <inheritdoc />
        protected override async Task<string> Solve01Async(IEnumerable<string> input)
        {
            var layout = GetLayout(input);
            var seen = new HashSet<string>();

            while (seen.Add(HashLayout(layout)))
            {
                layout = Step(layout);
            }

            return await Task.FromResult(HashLayout(layout).Count(c => c == '#').ToString());
        }

        /// <inheritdoc />
        protected override async Task<string> Solve02Async(IEnumerable<string> input)
        {
            var layout = GetLayout(input);
            var seen = new HashSet<string>();

            while (seen.Add(HashLayout(layout)))
            {
                layout = Step(layout, true);
            }

            return await Task.FromResult(HashLayout(layout).Count(c => c == '#').ToString());
        }
    }
}