using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using PCRE;

namespace AdventOfCode2020.Days
{
    [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
    internal class Day19 : Day
    {
        private static Dictionary<int, object> ParseRules(IEnumerable<string> input)
            => input
                .TakeWhile(line => !string.IsNullOrEmpty(line))
                .Select(line =>
                {
                    string[] parts = line.Split(": ");
                    object value = parts[1].Contains('"')
                        ? parts[1].Replace("\"", string.Empty)
                        : parts[1].Split('|').Select(part => part.Trim().Split(' '));

                    return (key: int.Parse(parts[0]), value);
                })
                .ToDictionary(kvp => kvp.key, kvp => kvp.value);

        private static string BuildRegexPart(int index, object rule)
        {
            if (rule is string value)
            {
                return $"(?<r{index}>{value})";
            }

            value = string.Join("|", ((IEnumerable<IEnumerable<string>>) rule)
                .Select(part => string.Join(string.Empty, part.Select(num => $"(?&r{num})"))));

            return $"(?<r{index}>{value})";
        }

        private static PcreRegex BuildRegex(Dictionary<int, object> rules)
            => new(
                $"(?(DEFINE){string.Join(string.Empty, rules.Keys.Select(key => BuildRegexPart(key, rules[key])))})^(?&r0)$"
            );

        /// <inheritdoc />
        protected override async Task<object> Solve01Async(IEnumerable<string> input)
        {
            var rules = ParseRules(input);
            var candidates = input.SkipWhile(line => !string.IsNullOrEmpty(line)).Skip(1);

            var regex = BuildRegex(rules);

            return await Task.FromResult(candidates.Count(candidate => regex.IsMatch(candidate)));
        }

        /// <inheritdoc />
        protected override async Task<object> Solve02Async(IEnumerable<string> input)
        {
            var rules = ParseRules(input);

            rules[8] = new List<string[]> {new[] {"42"}, new[] {"42", "8"}};
            rules[11] = new List<string[]> {new[] {"42", "31"}, new[] {"42", "11", "31"}};

            var candidates = input.SkipWhile(line => !string.IsNullOrEmpty(line)).Skip(1);

            var regex = BuildRegex(rules);

            return await Task.FromResult(candidates.Count(candidate => regex.IsMatch(candidate)));
        }
    }
}