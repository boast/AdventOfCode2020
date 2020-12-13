using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdventOfCode2020.Days
{
    internal class Day13 : Day
    {
        /// <inheritdoc />
        protected override async Task<string> Solve01Async(IEnumerable<string> input)
        {
            var inputs = input.ToList();

            long timestamp = long.Parse(inputs[0]);

            var busIds = inputs[1]
                .Split(',')
                .Where(busId => busId != "x")
                .Select(long.Parse)
                .Select(busId => (id: busId, wait: busId - timestamp % busId));

            (long earliestId, long earliestWait) = busIds.OrderBy(busId => busId.wait).First();

            return await Task.FromResult((earliestId * earliestWait).ToString());
        }

        /// <inheritdoc />
        protected override async Task<string> Solve02Async(IEnumerable<string> input)
        {
            var busIds = input
                .ElementAt(1)
                .Split(',')
                .Select((busId, i) => (busId, i))
                .Where(bus => bus.busId != "x")
                .Select(bus => (modulus: long.Parse(bus.busId), remainder: bus.i))
                .OrderBy(bus => bus.modulus)
                .ToList();

            long baseRemainder = busIds[0].remainder;
            long increment = busIds[0].modulus;
            long solution = 0;

            foreach ((long currentModulus, int currentRemainder) in busIds.Skip(1))
            {
                while ((solution + currentRemainder - baseRemainder) % currentModulus != 0)
                {
                    solution += increment;
                }

                increment *= currentModulus;
            }

            solution -= baseRemainder;

            return await Task.FromResult(solution.ToString());
        }
    }
}