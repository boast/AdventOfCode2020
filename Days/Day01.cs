using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdventOfCode2020.Days
{
    internal class Day01 : Day
    {
        /// <inheritdoc />
        protected override async Task<string> Solve01Async(IEnumerable<string> input)
        {
            var expenses = input.Select(int.Parse).ToList();

            for (int i = 0; i < expenses.Count - 1; i++)
            for (int j = i + 1; j < expenses.Count; j++)
                if (expenses[i] + expenses[j] == 2020)
                    return await Task.FromResult((expenses[i] * expenses[j]).ToString());

            throw new InvalidOperationException("Cannot find a solution.");
        }

        /// <inheritdoc />
        protected override async Task<string> Solve02Async(IEnumerable<string> input)
        {
            var expenses = input.Select(int.Parse).ToList();

            for (int i = 0; i < expenses.Count() - 2; i++)
            for (int j = i + 1; j < expenses.Count - 1; j++)
            for (int k = j + 1; k < expenses.Count; k++)
                if (expenses[i] + expenses[j] + expenses[k] == 2020)
                    return await Task.FromResult((expenses[i] * expenses[j] * expenses[k]).ToString());

            throw new InvalidOperationException("Cannot find a solution.");
        }
    }
}