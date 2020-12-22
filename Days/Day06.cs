using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdventOfCode2020.Days
{
    internal class Day06 : Day
    {
        private static IEnumerable<IEnumerable<string>> GetGroups(IEnumerable<string> input)
        {
            var groups = new List<IEnumerable<string>>();
            var currentGroup = new List<string>();

            foreach (string line in input)
            {
                if (line != string.Empty)
                {
                    currentGroup.Add(line);
                    continue;
                }

                groups.Add(currentGroup);
                currentGroup = new List<string>();
            }

            groups.Add(currentGroup);

            return groups;
        }

        /// <inheritdoc />
        protected override async Task<object> Solve01Async(IEnumerable<string> input)
            => await Task.FromResult(
                GetGroups(input)
                    .Select(groups => groups.Aggregate((current, all) => all + current))
                    .Select(stringGroup => stringGroup.Distinct().Count())
                    .Sum()
            );

        /// <inheritdoc />
        protected override async Task<object> Solve02Async(IEnumerable<string> input)
            => await Task.FromResult(
                GetGroups(input)
                    .Select(stringGroup =>
                    {
                        string[] groupArray = stringGroup as string[] ?? stringGroup.ToArray();
                        IEnumerable<char> candidates = groupArray[0];

                        candidates = groupArray
                            .Skip(1)
                            .Aggregate(candidates, (current, currentGroup)
                                => current.Where(currentGroup.Contains));

                        return candidates.Count();
                    })
                    .Sum()
            );
    }
}