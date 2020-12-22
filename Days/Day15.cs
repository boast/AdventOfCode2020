using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdventOfCode2020.Days
{
    internal class Day15 : Day
    {
        private static long IterateUntil(IList<int> numbers, int limit)
        {
            // We can increase the dictionary now, as we know if will fill to about 10% of the limit parameter
            var lastSeen = new Dictionary<int, (int prev, int curr)>(limit / 100 * 15);

            int number = numbers.Last();
            int count = numbers.Count + 1;

            for (int i = 1; i <= numbers.Count; i++)
            {
                lastSeen.Add(numbers[i - 1], (0, i));
            }

            do
            {
                (int prev, int curr) = lastSeen[number];
                int next = prev == 0 ? 0 : curr - prev;
                lastSeen[next] = (lastSeen.ContainsKey(next) ? lastSeen[next].curr : 0, count);
                number = next;
            } while (count++ < limit);

            return number;
        }

        private static List<int> ParseInput(IEnumerable<string> input)
            => input
                .ElementAt(0)
                .Split(',')
                .Select(int.Parse)
                .ToList();

        /// <inheritdoc />
        protected override async Task<object> Solve01Async(IEnumerable<string> input)
            => await Task.FromResult(IterateUntil(ParseInput(input), 2020));

        /// <inheritdoc />
        protected override async Task<object> Solve02Async(IEnumerable<string> input)
            => await Task.FromResult(IterateUntil(ParseInput(input), 30000000));
    }
}