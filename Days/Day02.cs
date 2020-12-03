using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode2020.Days
{
    internal class Day02 : Day
    {
        private static readonly Regex PasswordListRegex = new Regex(@"(?<a>\d+)-(?<b>\d+) (?<letter>\w): (?<password>\w+)");
        private static IEnumerable<(string password, char character, int a, int b)> GetPasswordPolicies(IEnumerable<string> input)
        {
            return input.Select(line =>
            {
                var group = PasswordListRegex.Match(line).Groups;
                return (group["password"].Value, char.Parse(group["letter"].Value), int.Parse(group["a"].Value), int.Parse(group["b"].Value));
            });
        }
        
        /// <inheritdoc />
        protected override async Task<string> Solve01Async(IEnumerable<string> input)
        {
            var passwords = GetPasswordPolicies(input);

            int count = passwords.Count(policy =>
            {
                (string password, char character, int a, int b) = policy;
                int occurrences = password!.Count(c => c == character);

                return occurrences >= a && occurrences <= b;
            });

            return await Task.FromResult(count.ToString());
        }

        /// <inheritdoc />
        protected override async Task<string> Solve02Async(IEnumerable<string> input)
        {
            var passwords = GetPasswordPolicies(input);

            int count = passwords.Count(policy =>
            {
                (string password, char character, int a, int b) = policy;
                return password[a - 1] == character ^ password[b - 1] == character;
            });

            return await Task.FromResult(count.ToString());
        }
    }
}