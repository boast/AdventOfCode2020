using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AdventOfCode2020.Days
{
    public class Day08 : Day
    {
        /// <inheritdoc />
        protected override async Task<long> Solve01Async(IEnumerable<string> input)
        {
            var instructions = ConsoleVM.Parse(input);
            var registers = new ConsoleVM.Registers();
            var seen = new HashSet<int>();

            while (seen.Add(registers.PC))
            {
                registers = ConsoleVM.Step(instructions, registers);
            }

            return await Task.FromResult(registers.ACC);
        }

        /// <inheritdoc />
        protected override async Task<long> Solve02Async(IEnumerable<string> input)
        {
            var instructions = ConsoleVM.Parse(input);

            foreach (var instruction in instructions)
            {
                if (instruction.Operation == ConsoleVM.Operation.ACC)
                {
                    continue;
                }

                var registers = new ConsoleVM.Registers();

                try
                {
                    // Swap this operation
                    instruction.Operation = instruction switch
                    {
                        {Operation: ConsoleVM.Operation.JMP} => ConsoleVM.Operation.NOP,
                        {Operation: ConsoleVM.Operation.NOP} => ConsoleVM.Operation.JMP,
                        _ => throw new InvalidOperationException($"Cannot swap operation '{instruction.Operation}'"),
                    };

                    var seen = new HashSet<int>();
                    while (seen.Add(registers.PC))
                    {
                        registers = ConsoleVM.Step(instructions, registers);
                    }

                    // Swap it back
                    instruction.Operation = instruction switch
                    {
                        {Operation: ConsoleVM.Operation.JMP} => ConsoleVM.Operation.NOP,
                        {Operation: ConsoleVM.Operation.NOP} => ConsoleVM.Operation.JMP,
                        _ => throw new InvalidOperationException($"Cannot swap operation '{instruction.Operation}'"),
                    };
                }
                catch (InvalidOperationException)
                {
                    return await Task.FromResult(registers.ACC);
                }
            }

            throw new InvalidOperationException("Cannot find a solution.");
        }
    }
}