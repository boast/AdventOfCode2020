using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdventOfCode2020.Days
{
    internal class Day10 : Day
    {
        /// <inheritdoc />
        protected override async Task<string> Solve01Async(IEnumerable<string> input)
        {
            var adapters = input.Select(int.Parse).OrderBy(adapter => adapter);

            int current = 0;
            int diff1 = 0;
            int diff3 = 1;

            foreach (int adapter in adapters)
            {
                switch (adapter - current)
                {
                    case 1:
                        diff1++;
                        break;
                    case 3:
                        diff3++;
                        break;
                }

                current = adapter;
            }

            return await Task.FromResult((diff1 * diff3).ToString());
        }

        /// <inheritdoc />
        protected override async Task<string> Solve02Async(IEnumerable<string> input)
        {
            // As the last one has only ever one variant, it does not change the global variant count
            var adapters = input.Select(long.Parse).Append(0).OrderBy(adapter => adapter).ToList();
            // Initialize all possible variants with 0, except the first as 1
            var variants = adapters.Select(adapter => adapter == 0L ? 1L : 0L).ToList();

            for (int i = 0; i < adapters.Count; i++)
            {
                variants[i] += Enumerable
                    // Check last 3 adapters
                    .Range(i - 3, 3)
                    // Check if candidate is valid and the candidate adapter is in valid distance
                    .Where(candidate => candidate >= 0 && adapters[i] <= adapters[candidate] + 3)
                    // Get from all candidates the possible variants
                    .Select(candidate => variants[candidate])
                    .Sum();
            }

            return await Task.FromResult(variants.Last().ToString());
        }
    }
}