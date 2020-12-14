using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode2020.Days
{
    internal class Day02 : Day
    {
        private static readonly Regex PasswordListRegex =
            new Regex(@"(?<a>\d+)-(?<b>\d+) (?<letter>\w): (?<password>\w+)");

        private static IEnumerable<(string password, char character, int a, int b)> GetPasswordPolicies(
            IEnumerable<string> input)
            => input.Select(line =>
            {
                var currentGroup = PasswordListRegex.Match(line).Groups;
                return (
                    currentGroup["password"].Value,
                    char.Parse(currentGroup["letter"].Value),
                    int.Parse(currentGroup["a"].Value),
                    int.Parse(currentGroup["b"].Value)
                );
            });

        /// <inheritdoc />
        protected override async Task<long> Solve01Async(IEnumerable<string> input)
        {
            var passwords = GetPasswordPolicies(input);

            int count = passwords.Count(policy =>
            {
                (string password, char character, int a, int b) = policy;
                int occurrences = password!.Count(c => c == character);

                return occurrences >= a && occurrences <= b;
            });

            return await Task.FromResult(count);
        }

        /// <inheritdoc />
        protected override async Task<long> Solve02Async(IEnumerable<string> input)
        {
            var passwords = GetPasswordPolicies(input);

            int count = passwords.Count(policy =>
            {
                (string password, char character, int a, int b) = policy;
                return (password[a - 1] == character) ^ (password[b - 1] == character);
            });

            return await Task.FromResult(count);
        }
    }
}