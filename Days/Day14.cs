using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode2020.Days
{
    internal class Day14 : Day
    {
        private readonly Regex _memoryRegex = new("mem\\[(?<address>\\d+)\\] = (?<value>\\d+)");
        private static string ToInt36Bin(long value) => Convert.ToString(value, 2).PadLeft(36, '0');

        /// <inheritdoc />
        protected override async Task<long> Solve01Async(IEnumerable<string> input)
        {
            string mask = string.Empty;
            var mem = new Dictionary<long, long>();

            foreach (string line in input)
            {
                if (line[..4] == "mask")
                {
                    mask = line[7..];
                }
                else
                {
                    var matchGroups = _memoryRegex.Match(line).Groups;
                    (long address, long value) = (long.Parse(matchGroups["address"].Value),
                        long.Parse(matchGroups["value"].Value));

                    mem[address] = Convert.ToInt64(
                        new string(ToInt36Bin(value)
                            .Select((bit, i) => i < mask.Length && mask[i] != 'X' ? mask[i] : bit)
                            .ToArray())
                        , 2);
                }
            }

            return await Task.FromResult(mem.Sum(kv => kv.Value));
        }

        /// <inheritdoc />
        protected override async Task<long> Solve02Async(IEnumerable<string> input)
        {
            string mask = string.Empty;
            var mem = new Dictionary<long, long>();

            foreach (string line in input)
            {
                if (line[..4] == "mask")
                {
                    mask = line[7..];
                }
                else
                {
                    var matchGroups = _memoryRegex.Match(line).Groups;
                    (int address, long value) = (int.Parse(matchGroups["address"].Value),
                        long.Parse(matchGroups["value"].Value));

                    string maskedAddress = new(ToInt36Bin(address)
                        .Select((bit, i) => mask[i] == '0' ? bit : mask[i])
                        .ToArray());

                    foreach (long resolvedAddress in ResolveFloatingAddress(maskedAddress))
                    {
                        mem[resolvedAddress] = value;
                    }
                }
            }

            return await Task.FromResult(mem.Sum(kv => kv.Value));
        }

        private static IEnumerable<long> ResolveFloatingAddress(string address)
        {
            var result = new List<long>();

            if (!address.Contains('X'))
            {
                result.Add(Convert.ToInt64(address, 2));
            }
            else
            {
                int xPos = address.IndexOf('X');
                var sb = new StringBuilder(address) {[xPos] = '0'};
                result.AddRange(ResolveFloatingAddress(sb.ToString()));
                sb[xPos] = '1';
                result.AddRange(ResolveFloatingAddress(sb.ToString()));
            }

            return result;
        }
    }
}