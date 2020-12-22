using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode2020.Days
{
    internal class Day16 : Day
    {
        private static readonly Regex RangesRegex =
            new("(?<name>.+): (?<range1Start>\\d+)-(?<range1End>\\d+) or (?<range2Start>\\d+)-(?<range2End>\\d+)");

        private static (
            IList<(string name, int range1Start, int range1End, int range2Start, int range2End)> ranges,
            IList<int> ticket,
            IEnumerable<IList<int>> nearbyTickets
            ) Parse(IEnumerable<string> input)
        {
            // Prevent multiple enumeration
            var inputList = input.ToList();

            var ranges = inputList
                .TakeWhile(line => !string.IsNullOrEmpty(line))
                .Select(line =>
                {
                    var match = RangesRegex.Match(line);

                    return (
                        name: match.Groups["name"].Value,
                        range1Start: int.Parse(match.Groups["range1Start"].Value),
                        range1End: int.Parse(match.Groups["range1End"].Value),
                        range2Start: int.Parse(match.Groups["range2Start"].Value),
                        range2End: int.Parse(match.Groups["range2End"].Value)
                    );
                })
                .ToList();

            var ticket = inputList
                .SkipWhile(line => line != "your ticket:")
                .Skip(1)
                .First()
                .Split(",")
                .Select(int.Parse)
                .ToList();

            var nearbyTickets = inputList
                .SkipWhile(line => line != "nearby tickets:")
                .Skip(1)
                .Select(line => line.Split(",").Select(int.Parse).ToList());

            return (ranges, ticket, nearbyTickets);
        }

        private static bool RangeMatch(
            (string name, int range1Start, int range1End, int range2Start, int range2End) range, int value)
            =>
                value >= range.range1Start && value <= range.range1End ||
                value >= range.range2Start && value <= range.range2End;

        /// <inheritdoc />
        protected override async Task<object> Solve01Async(IEnumerable<string> input)
        {
            var (ranges, _, nearbyTickets) = Parse(input);
            var invalidValues = new List<int>();

            foreach (var nearbyTicket in nearbyTickets)
            {
                invalidValues.AddRange(nearbyTicket.Where(number => ranges.All(range => !RangeMatch(range, number))));
            }

            return await Task.FromResult(invalidValues.Sum());
        }

        /// <inheritdoc />
        protected override async Task<object> Solve02Async(IEnumerable<string> input)
        {
            var (ranges, ticket, nearbyTickets) = Parse(input);
            var validTickets = nearbyTickets
                .Where(nearbyTicket => nearbyTicket.All(number => ranges.Any(range => RangeMatch(range, number))))
                .Append(ticket)
                .ToList();

            var candidates = ranges.ToDictionary(range => range.name, _ => new List<int>());

            for (int i = 0; i < ticket.Count; i++)
            {
                var fieldValues = validTickets.Select(validTicket => validTicket[i]).ToList();

                foreach (var range in ranges)
                {
                    if (fieldValues.All(fieldValue => RangeMatch(range, fieldValue)))
                    {
                        candidates[range.name].Add(i);
                    }
                }
            }

            var fieldPositions = new Dictionary<string, int>();

            while (candidates.Any())
            {
                if (candidates.Any(candidate => candidate.Value.Count == 1))
                {
                    var (fieldName, fieldCandidates) = candidates.First(candidate => candidate.Value.Count == 1);
                    int positionIndex = fieldCandidates.First();

                    fieldPositions.Add(fieldName, positionIndex);

                    foreach (var (_, value) in candidates)
                    {
                        value.Remove(positionIndex);
                    }

                    candidates.Remove(fieldName);
                }
                else
                {
                    throw new InvalidOperationException("Cannot find a solution.");
                }
            }

            var departureIndexes = fieldPositions
                .Where(fieldPosition => fieldPosition.Key.StartsWith("departure"))
                .Select(kv => kv.Value);

            return await Task.FromResult(
                ticket
                    .Where((_, i) => departureIndexes.Contains(i))
                    .Aggregate(1L, (acc, val) => acc * val)
            );
        }
    }
}