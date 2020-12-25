using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;

namespace AdventOfCode2020.Days
{
    internal class Day25 : Day
    {
        /// <inheritdoc />
        protected override async Task<object> Solve01Async(IEnumerable<string> input)
        {
            BigInteger[] publicKeys = input.Select(BigInteger.Parse).ToArray();

            var loopSize = BigInteger.Zero;
            while (BigInteger.ModPow(7, ++loopSize, 20201227) != publicKeys[0])
            {
            }

            return await Task.FromResult(BigInteger.ModPow(publicKeys[1], loopSize, 20201227));
        }

        /// <inheritdoc />
        protected override async Task<object> Solve02Async(IEnumerable<string> input)
            => await Task.FromResult("Merry Christmas!");
    }
}