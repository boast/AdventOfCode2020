using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdventOfCode2020.Days
{
    internal class Day05 : Day
    {
        private static IEnumerable<int> GetSeatIds(IEnumerable<string> input)
        {
            return input.Select(line =>
                Convert.ToInt32(line
                    .Replace('B', '1')
                    .Replace('F', '0')
                    .Replace('R', '1')
                    .Replace('L', '0'), 2));
        }

        /// <inheritdoc />
        protected override async Task<long> Solve01Async(IEnumerable<string> input)
            => await Task.FromResult(GetSeatIds(input).Max());

        /// <inheritdoc />
        protected override async Task<long> Solve02Async(IEnumerable<string> input)
        {
            var seats = GetSeatIds(input).OrderBy(seatId => seatId).ToArray();

            // The next seat is two ids apart, we have one seat missing
            for (int i = 0; i < seats.Length - 1; i++)
            {
                // The missing seat is the next id
                if (seats[i + 1] == seats[i] + 2)
                {
                    return await Task.FromResult(seats[i] + 1);
                }
            }

            throw new InvalidOperationException("Cannot find a solution.");
        }
    }
}