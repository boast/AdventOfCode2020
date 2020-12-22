using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdventOfCode2020.Days
{
    internal class Day20 : Day
    {
        /// <inheritdoc />
        protected override async Task<object> Solve01Async(IEnumerable<string> input)
        {
            var map = RestoreTiles(input);
            return await Task.FromResult(1L * map[0, 0].Id * map[11, 11].Id * map[0, 11].Id * map[11, 0].Id);
        }

        /// <inheritdoc />
        protected override async Task<object> Solve02Async(IEnumerable<string> input)
        {
            var map = RestoreTiles(input);
            var image = new List<string>();
            for (int rowIndex = 0; rowIndex < 12; rowIndex++)
            {
                for (int i = 1; i < 9; i++)
                {
                    string st = "";
                    for (int colIndex = 0; colIndex < 12; colIndex++)
                    {
                        st += map[rowIndex, colIndex].Row(i).Substring(1, 8);
                    }

                    image.Add(st);
                }
            }

            var bigTile = new Tile(-1, image.ToArray());

            string[] monster =
            {
                "                  # ",
                "#    ##    ##    ###",
                " #  #  #  #  #  #   ",
            };

            for (int i = 0; i < 9; i++)
            {
                int CountMatches()
                {
                    int res = 0;
                    for (int rowIndex = 0; rowIndex < bigTile.Size; rowIndex++)
                    {
                        for (int colIndex = 0; colIndex < bigTile.Size; colIndex++)
                        {
                            bool Match()
                            {
                                int monsterColCount = monster[0].Length;
                                int monsterRowCount = monster.Length;
                                if (colIndex + monsterColCount >= bigTile.Size)
                                {
                                    return false;
                                }

                                if (rowIndex + monsterRowCount >= bigTile.Size)
                                {
                                    return false;
                                }

                                for (int monsterColIndex = 0; monsterColIndex < monsterColCount; monsterColIndex++)
                                {
                                    for (int monsterRowIndex = 0; monsterRowIndex < monsterRowCount; monsterRowIndex++)
                                    {
                                        if (monster[monsterRowIndex][monsterColIndex] == '#' &&
                                            bigTile[rowIndex + monsterRowIndex, colIndex + monsterColIndex] != '#')
                                        {
                                            return false;
                                        }
                                    }
                                }

                                return true;
                            }

                            if (Match())
                            {
                                res++;
                            }
                        }
                    }

                    return res;
                }

                int matchesCount = CountMatches();

                if (matchesCount > 0)
                {
                    int hashCount = 0;
                    for (int rowIndex = 0; rowIndex < bigTile.Size; rowIndex++)
                    {
                        for (int colIndex = 0; colIndex < bigTile.Size; colIndex++)
                        {
                            if (bigTile[rowIndex, colIndex] == '#')
                                hashCount++;
                        }
                    }

                    return await Task.FromResult(hashCount - matchesCount * 15);
                }

                bigTile.ChangePosition();
            }

            throw new Exception();
        }

        private static IEnumerable<Tile> Parse(IEnumerable<string> input)
        {
            var tiles = new List<Tile>();
            int tileId = 0;
            List<string> content = new();

            foreach (string line in input)
            {
                if (string.IsNullOrEmpty(line))
                {
                    tiles.Add(new Tile(tileId, content.ToArray()));
                    content = new List<string>();
                    continue;
                }

                if (line.Contains("Tile"))
                {
                    tileId = int.Parse(line.Substring(5, 4));
                }
                else
                {
                    content.Add(line);
                }
            }

            tiles.Add(new Tile(tileId, content.ToArray()));

            return tiles;
        }

        private static Tile[,] RestoreTiles(IEnumerable<string> input)
        {
            var tiles = Parse(input).ToList();

            Tile FindTile(string? topPattern, string? leftPattern)
            {
                foreach (var tile in tiles)
                {
                    for (int i = 0; i < 8; i++)
                    {
                        bool topMatch = topPattern != null
                            ? tile.Top() == topPattern
                            : !tiles.Any(tileB => tileB.Id != tile.Id && tileB.Edges.Contains(tile.Top()));
                        bool leftMatch = leftPattern != null
                            ? tile.Left() == leftPattern
                            : !tiles.Any(tileB => tileB.Id != tile.Id && tileB.Edges.Contains(tile.Left()));

                        if (topMatch && leftMatch)
                        {
                            return tile;
                        }

                        tile.ChangePosition();
                    }
                }

                throw new Exception();
            }

            var mtx = new Tile[12, 12];
            for (int rowIndex = 0; rowIndex < 12; rowIndex++)
            {
                for (int colIndex = 0; colIndex < 12; colIndex++)
                {
                    string? topPattern = rowIndex == 0 ? null : mtx[rowIndex - 1, colIndex].Bottom();
                    string? leftPattern = colIndex == 0 ? null : mtx[rowIndex, colIndex - 1].Right();

                    var tile = FindTile(topPattern, leftPattern);
                    mtx[rowIndex, colIndex] = tile;
                    tiles.Remove(tile);
                }
            }

            return mtx;
        }

        private class Tile
        {
            private readonly string[] _image;
            public readonly string[] Edges;
            public readonly int Id;
            public readonly int Size;

            private int _position;

            public Tile(int title, string[] image)
            {
                Id = title;
                _image = image;
                Size = image.Length;

                Edges = new[]
                {
                    Edge(0, 0, 0, 1),
                    Edge(0, 0, 1, 0),
                    Edge(Size - 1, 0, 0, 1),
                    Edge(Size - 1, 0, -1, 0),
                    Edge(0, Size - 1, 0, -1),
                    Edge(0, Size - 1, 1, 0),
                    Edge(Size - 1, Size - 1, 0, -1),
                    Edge(Size - 1, Size - 1, -1, 0),
                };
            }

            public char this[int rowIndex, int colIndex]
            {
                get
                {
                    for (int i = 0; i < _position % 4; i++)
                    {
                        (rowIndex, colIndex) = (colIndex, Size - 1 - rowIndex);
                    }

                    if (_position % 8 >= 4)
                    {
                        colIndex = Size - 1 - colIndex;
                    }

                    return _image[rowIndex][colIndex];
                }
            }

            public void ChangePosition()
            {
                _position++;
                _position %= 8;
            }

            private string Edge(int rowIndex, int colIndex, int deltaRow, int deltaCol)
            {
                string st = "";

                for (int i = 0; i < Size; i++)
                {
                    st += this[rowIndex, colIndex];
                    rowIndex += deltaRow;
                    colIndex += deltaCol;
                }

                return st;
            }

            public string Row(int rowIndex) => Edge(rowIndex, 0, 0, 1);
            public string Top() => Edge(0, 0, 0, 1);
            public string Bottom() => Edge(Size - 1, 0, 0, 1);
            public string Left() => Edge(0, 0, 1, 0);
            public string Right() => Edge(0, Size - 1, 1, 0);
        }
    }
}