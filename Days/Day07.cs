using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode2020.Days
{
    internal class Day07 : Day
    {
        private static IReadOnlyDictionary<string, List<(string bag, int count)>> GetBags(IEnumerable<string> input)
        {
            var bags = new Dictionary<string, List<(string bag, int count)>>();
            var regex = new Regex("^(?<bag>\\w+ \\w+) bags contain ((no other bags)|"
                                  + "(?<other1>(?<otherbagcount1>\\d+) (?<otherbag1>\\w+ \\w+) bag(s)?(, )?)"
                                  + "(?<other2>(?<otherbagcount2>\\d+) (?<otherbag2>\\w+ \\w+) bag(s)?(, )?)?"
                                  + "(?<other3>(?<otherbagcount3>\\d+) (?<otherbag3>\\w+ \\w+) bag(s)?(, )?)?"
                                  + "(?<other4>(?<otherbagcount4>\\d+) (?<otherbag4>\\w+ \\w+) bag(s)?(, )?)?)\\.$");

            foreach (string line in input)
            {
                var match = regex.Match(line);
                bags.Add(match.Groups["bag"].Value, new List<(string bag, int count)>());

                for (int i = 1; match.Groups[$"other{i}"].Success; i++)
                {
                    bags[match.Groups["bag"].Value].Add((match.Groups[$"otherbag{i}"].Value,
                        int.Parse(match.Groups[$"otherbagcount{i}"].Value)));
                }
            }

            return bags;
        }

        private static bool ContainsRecursive(string key, string value,
            IReadOnlyDictionary<string, List<(string bag, int count)>> bags)
        {
            return bags[key].Any(bag => bag.bag == value)
                   || bags[key].Any(bag => ContainsRecursive(bag.bag, value, bags));
        }

        /// <inheritdoc />
        protected override async Task<string> Solve01Async(IEnumerable<string> input)
        {
            var bags = GetBags(input);

            return await Task.FromResult(
                bags.Count(bag => ContainsRecursive(bag.Key, "shiny gold", bags))
                    .ToString()
            );
        }

        private static int CountRecursive(string key, IReadOnlyDictionary<string, List<(string bag, int count)>> bags)
            => 1 + bags[key].Sum(bag => CountRecursive(bag.bag, bags) * bag.count);

        /// <inheritdoc />
        protected override async Task<string> Solve02Async(IEnumerable<string> input)
        {
            var bags = GetBags(input);

            return await Task.FromResult((CountRecursive("shiny gold", bags) - 1).ToString());
        }
    }
}