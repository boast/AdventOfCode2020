using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdventOfCode2020.Days
{
    internal class Day09 : Day
    {
        /// <inheritdoc />
        protected override async Task<object> Solve01Async(IEnumerable<string> input)
        {
            const int preamble = 25;

            var numbers = input.Select(long.Parse).ToList();

            return await Task.FromResult(numbers
                .Select((_, i) => (current: numbers[preamble + i], list: numbers.Skip(i).Take(preamble)))
                .First(e => !e.list
                    .SelectMany(n => e.list.SkipWhile(m => n != m), (n, m) => n + m)
                    .Contains(e.current)
                ).current);
        }

        /// <inheritdoc />
        protected override async Task<object> Solve02Async(IEnumerable<string> input)
        {
            var inputList = input.ToList();

            long target = (long) await Solve01Async(inputList);
            var numbers = inputList.Select(long.Parse).ToList();
            int currentIndex = 0;

            while (currentIndex < numbers.Count)
            {
                int innerIndex = currentIndex;
                var weakness = new List<long>();

                do
                {
                    weakness.Add(numbers[innerIndex]);
                    innerIndex++;
                } while (innerIndex < numbers.Count && weakness.Sum() < target);

                if (weakness.Sum() == target)
                {
                    return await Task.FromResult(weakness.Min() + weakness.Max());
                }

                currentIndex++;
            }

            throw new InvalidOperationException("Cannot find a solution.");
        }
    }
}