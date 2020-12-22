using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdventOfCode2020.Days
{
    internal class Day13 : Day
    {
        /// <inheritdoc />
        protected override async Task<object> Solve01Async(IEnumerable<string> input)
        {
            var inputs = input.ToList();

            long timestamp = long.Parse(inputs[0]);

            var busIds = inputs[1]
                .Split(',')
                .Where(busId => busId != "x")
                .Select(long.Parse)
                .Select(busId => (id: busId, wait: busId - timestamp % busId));

            (long earliestId, long earliestWait) = busIds.OrderBy(busId => busId.wait).First();

            return await Task.FromResult(earliestId * earliestWait);
        }

        /// <inheritdoc />
        protected override async Task<object> Solve02Async(IEnumerable<string> input)
        {
            var busIds = input
                .ElementAt(1)
                .Split(',')
                .Select((busId, i) => (busId, i))
                .Where(bus => bus.busId != "x")
                .Select(bus => (modulus: long.Parse(bus.busId), remainder: bus.i))
                .ToList();

            // Chinese Reminder Theorem
            // As first remainder is 0 (we do not need to sort, only practical by hand),
            // the initial step is simple.
            long increment = busIds[0].modulus;
            long solution = 0;

            foreach ((long modulus, int remainder) in busIds.Skip(1))
            {
                while ((solution + remainder) % modulus != 0)
                {
                    solution += increment;
                }

                increment *= modulus;
            }

            return await Task.FromResult(solution);
        }
    }
}