using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdventOfCode2020.Days
{
    internal class Day12 : Day
    {
        private static readonly Point MoveNorth = new(0, 1);
        private static readonly Point MoveSouth = new(0, -1);
        private static readonly Point MoveEast = new(1);
        private static readonly Point MoveWest = new(-1);

        private static Direction Reverse(Direction direction)
            => direction switch
            {
                Direction.N => Direction.S,
                Direction.S => Direction.N,
                Direction.E => Direction.W,
                Direction.W => Direction.E,
                _ => throw new ArgumentOutOfRangeException(nameof(direction)),
            };


        private static Direction TurnLeft(Direction direction)
            => direction switch
            {
                Direction.N => Direction.W,
                Direction.S => Direction.E,
                Direction.E => Direction.N,
                Direction.W => Direction.S,
                _ => throw new ArgumentOutOfRangeException(nameof(direction)),
            };

        private static Direction TurnRight(Direction direction)
            => TurnLeft(Reverse(direction));

        private static int ManhattanDistance(Point a, Point b)
            => Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y);

        private static IEnumerable<(string instruction, int value)> GetInstructions(IEnumerable<string> input)
            => input.Select(line => (instruction: line[0].ToString(), value: int.Parse(line.Substring(1))));

        private static Point Move(Point point, Direction direction, int value)
            => point + direction switch
            {
                Direction.N => MoveNorth,
                Direction.S => MoveSouth,
                Direction.E => MoveEast,
                Direction.W => MoveWest,
                _ => throw new ArgumentOutOfRangeException(nameof(direction)),
            } * value;

        /// <inheritdoc />
        protected override async Task<long> Solve01Async(IEnumerable<string> input)
        {
            var instructions = GetInstructions(input);

            var ship = Point.Origin;
            var shipDirection = Direction.E;

            foreach ((string instruction, int value) in instructions)
            {
                if (Enum.TryParse(instruction, out Direction instructionShipDirection))
                {
                    ship = Move(ship, instructionShipDirection, value);
                }
                else if (Enum.TryParse(instruction, out Movement instructionShipMovement))
                {
                    switch (instructionShipMovement)
                    {
                        case Movement.L:
                            shipDirection = value switch
                            {
                                90 => TurnLeft(shipDirection),
                                180 => Reverse(shipDirection),
                                270 => TurnRight(shipDirection),
                                _ => throw new ArgumentOutOfRangeException(nameof(value), "Invalid turn value"),
                            };
                            break;
                        case Movement.R:
                            shipDirection = value switch
                            {
                                90 => TurnRight(shipDirection),
                                180 => Reverse(shipDirection),
                                270 => TurnLeft(shipDirection),
                                _ => throw new ArgumentOutOfRangeException(nameof(value), "Invalid turn value"),
                            };
                            break;
                        case Movement.F:
                            ship = Move(ship, shipDirection, value);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException(nameof(instructionShipMovement), "Invalid ship movement");
                    }
                }
                else
                {
                    throw new ArgumentOutOfRangeException(nameof(instruction), "Invalid instruction");
                }
            }

            return await Task.FromResult(Point.Origin.ManhattanDistance(ship));
        }

        /// <inheritdoc />
        protected override async Task<long> Solve02Async(IEnumerable<string> input)
        {
            var instructions = GetInstructions(input);

            var ship = Point.Origin;
            var waypoint = new Point(10, 1);

            foreach ((string instruction, int value) in instructions)
            {
                if (Enum.TryParse(instruction, out Direction instructionShipDirection))
                {
                    waypoint = Move(waypoint, instructionShipDirection, value);
                }
                else if (Enum.TryParse(instruction, out Movement instructionShipMovement))
                {
                    switch (instructionShipMovement)
                    {
                        case Movement.L:
                            waypoint = value switch
                            {
                                90 => new Point(-1 * waypoint.Y, waypoint.X),
                                180 => -waypoint,
                                270 => new Point(waypoint.Y, -1 * waypoint.X),
                                _ => throw new ArgumentOutOfRangeException(nameof(value), "Invalid turn value"),
                            };
                            break;
                        case Movement.R:
                            waypoint = value switch
                            {
                                90 => new Point(waypoint.Y, -1 * waypoint.X),
                                180 => -waypoint,
                                270 => new Point(-1 * waypoint.Y, waypoint.X),
                                _ => throw new ArgumentOutOfRangeException(nameof(value), "Invalid turn value"),
                            };
                            break;
                        case Movement.F:
                            ship += waypoint * value;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException(nameof(instructionShipMovement), "Invalid ship movement");
                    }
                }
                else
                {
                    throw new ArgumentOutOfRangeException(nameof(instruction), "Invalid instruction");
                }
            }

            return await Task.FromResult(Point.Origin.ManhattanDistance(ship));
        }

        private enum Direction
        {
            N,
            S,
            E,
            W,
        }

        private enum Movement
        {
            L,
            R,
            F,
        }
    }
}