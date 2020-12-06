﻿using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode2020.Days
{
    internal class Day04 : Day
    {
        private readonly Regex _regexByr = new Regex("^(19[2-9](\\d))|(200[0-2])$");
        private readonly Regex _regexEcl = new Regex("^(amb)|(blu)|(brn)|(gry)|(grn)|(hzl)|(oth)$");
        private readonly Regex _regexEyr = new Regex("^(202\\d)|(2030)$");
        private readonly Regex _regexHcl = new Regex("^#[0-9a-f]{6}$");
        private readonly Regex _regexHgt = new Regex("^((59|6\\d|7[0-6])in)|((1[5-8]\\d|19[0-3])cm)$");
        private readonly Regex _regexIyr = new Regex("^(201\\d)|(2020)$");
        private readonly Regex _regexPid = new Regex("^\\d{9}$");

        private static IEnumerable<Passport> ParsePassports(IEnumerable<string> input)
        {
            string flattenInput = new string(input.SelectMany(line =>
                    line == string.Empty
                        ? "},{" // combine objects
                        : "\"" + line.Replace(":", "\":\"").Replace(" ", "\",\"") +
                          "\"," // convert to json properties (_ is space). foo:bar_ -> "foo":"bar",
            ).ToArray());

            // wrap as array and start/finish the first/last object. "foo":"bar",}{"foo":"bar", -> [{"foo":"bar",}{"foo":"bar",}]
            string wrappedJson = $"[{{{flattenInput}}}]";

            var options = new JsonSerializerOptions
            {
                AllowTrailingCommas = true,
                PropertyNameCaseInsensitive = true,
            };

            return JsonSerializer.Deserialize<IEnumerable<Passport>>(wrappedJson, options);
        }

        /// <inheritdoc />
        protected override async Task<string> Solve01Async(IEnumerable<string> input)
        {
            var passports = ParsePassports(input);

            return await Task.FromResult(passports.Count(passport =>
                passport.Byr != null &&
                passport.Iyr != null &&
                passport.Eyr != null &&
                passport.Hgt != null &&
                passport.Hcl != null &&
                passport.Ecl != null &&
                passport.Pid != null
            ).ToString());
        }

        /// <inheritdoc />
        protected override async Task<string> Solve02Async(IEnumerable<string> input)
        {
            var passports = ParsePassports(input);

            return await Task.FromResult(passports.Count(passport =>
                passport.Byr != null && _regexByr.IsMatch(passport.Byr) &&
                passport.Iyr != null && _regexIyr.IsMatch(passport.Iyr) &&
                passport.Eyr != null && _regexEyr.IsMatch(passport.Eyr) &&
                passport.Hgt != null && _regexHgt.IsMatch(passport.Hgt) &&
                passport.Hcl != null && _regexHcl.IsMatch(passport.Hcl) &&
                passport.Ecl != null && _regexEcl.IsMatch(passport.Ecl) &&
                passport.Pid != null && _regexPid.IsMatch(passport.Pid)
            ).ToString());
        }

        // ReSharper disable once ClassNeverInstantiated.Local
        [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Local")]
        private class Passport
        {
            public string? Byr { get; set; }
            public string? Iyr { get; set; }
            public string? Eyr { get; set; }
            public string? Hgt { get; set; }
            public string? Hcl { get; set; }
            public string? Ecl { get; set; }
            public string? Pid { get; set; }

            // As we ignore it, we dont even need to parse it
            //public string? Cid { get; set; }
        }
    }
}