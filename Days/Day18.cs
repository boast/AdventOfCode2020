using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace AdventOfCode2020.Days
{
    internal class Day18 : Day
    {
        private static Queue<Token> Tokenize(string input)
        {
            var queue = new Queue<Token>();

            foreach (char c in input.Where(c => c != ' '))
            {
                var token = c switch
                {
                    '+' => TokenType.Addition,
                    '*' => TokenType.Multiplication,
                    '(' => TokenType.OpenParenthesis,
                    ')' => TokenType.CloseParenthesis,
                    _ => TokenType.Digit,
                };
                queue.Enqueue(new Token(token, token == TokenType.Digit ? (long) char.GetNumericValue(c) : null));
            }

            return queue;
        }

        [SuppressMessage("ReSharper", "SwitchStatementHandlesSomeKnownEnumValuesWithDefault")]
        [SuppressMessage("ReSharper", "SwitchExpressionHandlesSomeKnownEnumValuesWithExceptionInDefault")]
        private static long Solve(Queue<Token> tokens)
        {
            long total = 0;
            var operand = TokenType.Addition;

            while (tokens.TryDequeue(out var token))
            {
                switch (token.Type)
                {
                    case TokenType.Digit:
                        total = operand switch
                        {
                            TokenType.Addition => total + (long) token.Value!,
                            TokenType.Multiplication => total * (long) token.Value!,
                            _ => throw new ArgumentOutOfRangeException(nameof(token), "Invalid operand"),
                        };
                        break;
                    case TokenType.Addition:
                        operand = TokenType.Addition;
                        break;
                    case TokenType.Multiplication:
                        operand = TokenType.Multiplication;
                        break;
                    case TokenType.OpenParenthesis:
                        total = operand switch
                        {
                            TokenType.Addition => total + Solve(tokens),
                            TokenType.Multiplication => total * Solve(tokens),
                            _ => throw new ArgumentOutOfRangeException(nameof(token), "Invalid operand"),
                        };
                        break;
                    case TokenType.CloseParenthesis:
                        return total;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(token), "Invalid token");
                }
            }

            return total;
        }

        /// <inheritdoc />
        protected override async Task<object> Solve01Async(IEnumerable<string> input)
            => await Task.FromResult(
                input
                    .Select(Tokenize)
                    .Select(Solve)
                    .Sum()
            );

        /// <inheritdoc />
        protected override async Task<object> Solve02Async(IEnumerable<string> input)
            => await Task.FromResult(
                input
                    // We do not need to change the rules, just bracket the multiplication so it is evaluated "first"
                    // so addition has precedence (meaning evaluated last).
                    .Select(line =>
                        "("
                        + line
                            .Replace("(", "((")
                            .Replace(")", "))")
                            .Replace("*", ")*(")
                        + ")")
                    .Select(Tokenize)
                    .Select(Solve)
                    .Sum()
            );

        private enum TokenType
        {
            Digit,
            Addition,
            Multiplication,
            OpenParenthesis,
            CloseParenthesis,
        }

        private record Token(TokenType Type, long? Value);
    }
}