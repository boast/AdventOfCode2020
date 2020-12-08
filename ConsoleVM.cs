using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode2020
{
    internal static class ConsoleVM
    {
        private static readonly Regex ParseRegex = new Regex("(?<operation>\\w+) (?<argument>(\\+|-)\\d+)");

        private static Operation OperationFromString(string value) => Enum.Parse<Operation>(value, true);

        [SuppressMessage("ReSharper", "ReturnTypeCanBeEnumerable.Global",
            Justification = "Indexed access required implicit")]
        internal static IList<Instruction> Parse(IEnumerable<string> input)
            => input.Select(line =>
            {
                var matchGroups = ParseRegex.Match(line).Groups;
                return new Instruction(OperationFromString(matchGroups["operation"].Value),
                    long.Parse(matchGroups["argument"].Value));
            }).ToList();

        [SuppressMessage("ReSharper", "ParameterTypeCanBeEnumerable.Global",
            Justification = "Indexed access required implicit")]
        internal static Registers Step(IList<Instruction> instructions, Registers registers)
        {
            (int pc, long acc) = registers;

            var instruction = instructions.ElementAtOrDefault(pc) ??
                              throw new InvalidOperationException($"No instruction at pc '{pc}'");

            switch (instruction.Operation)
            {
                case Operation.ACC:
                    acc += instruction.Argument;
                    goto case Operation.NOP;
                case Operation.JMP:
                    pc += (int) instruction.Argument;
                    break;
                case Operation.NOP:
                    pc++;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return new Registers(pc, acc);
        }

        [SuppressMessage("ReSharper", "InconsistentNaming")]
        internal enum Operation
        {
            ACC,
            JMP,
            NOP,
        }

        internal class Instruction
        {
            public Instruction(Operation operation, long argument) => (Operation, Argument) = (operation, argument);
            public Operation Operation { get; set; }
            public long Argument { get; }
        }

        [SuppressMessage("ReSharper", "InconsistentNaming")]
        internal class Registers
        {
            public Registers()
            {
            }

            public Registers(int pc, long acc) => (PC, ACC) = (pc, acc);
            public int PC { get; }
            public long ACC { get; }

            public void Deconstruct(out int pc, out long acc) => (pc, acc) = (PC, ACC);
        }
    }
}