using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;

namespace AdventOfCode2020.Days
{
    internal class Day23 : Day
    {
        private static LinkedListNode<int> Play(LinkedList<int> cups, int rounds)
        {
            // Build index for fast index based access into the linked list
            var cupsIndex = new Dictionary<int, LinkedListNode<int>>();

            var cupForIndex = cups.First!;
            cupsIndex.Add(cupForIndex!.Value, cupForIndex);

            while ((cupForIndex = cupForIndex.Next) != null)
            {
                cupsIndex.Add(cupForIndex.Value, cupForIndex);
            }

            int currentRound = 0;
            var cup = cups.First!;

            do
            {
                currentRound++;

                // Remove next 3 and remove them from the list
                var pick1 = cup!.NextOrFirst();
                var pick2 = pick1.NextOrFirst();
                var pick3 = pick2.NextOrFirst();
                var pickup = new[] {pick1, pick2, pick3};

                foreach (var picks in pickup)
                {
                    cups.Remove(picks);
                }

                // Find destination value
                int destinationCupValue = cup.Value - 1;
                while (
                    destinationCupValue < 1 ||
                    destinationCupValue == cup.Value ||
                    pickup.Any(pick => pick.Value == destinationCupValue)
                )
                {
                    destinationCupValue = destinationCupValue < 1 ? cupsIndex.Count : destinationCupValue - 1;
                }

                // Insert pickups at destination, using the index to lookup the target
                var destination = cupsIndex[destinationCupValue];

                foreach (var pick in pickup)
                {
                    cups.AddAfter(destination, pick);
                    destination = pick;
                }

                // Move to next cup
                cup = cup.NextOrFirst();
            } while (currentRound < rounds);

            return cupsIndex[1];
        }

        private static ImmutableList<int> ParseCups(IEnumerable<string> input)
            => input.First().Select(c => (int) char.GetNumericValue(c)).ToImmutableList();

        /// <inheritdoc />
        protected override async Task<object> Solve01Async(IEnumerable<string> input)
        {
            var cups = new LinkedList<int>(ParseCups(input));
            var firstCup = Play(cups, 100);

            string result = "";

            foreach (int _ in Enumerable.Range(0, 8))
            {
                firstCup = firstCup.NextOrFirst();
                result += firstCup.Value;
            }

            return await Task.FromResult(result);
        }

        /// <inheritdoc />
        protected override async Task<object> Solve02Async(IEnumerable<string> input)
        {
            var cups = new LinkedList<int>(ParseCups(input).AddRange(Enumerable.Range(10, 1_000_001 - 10)));
            var firstCup = Play(cups, 10_000_000);

            return await Task.FromResult(
                (long) firstCup.NextOrFirst().Value * firstCup.NextOrFirst().NextOrFirst().Value
            );
        }
    }

    internal static class CircularLinkedList
    {
        public static LinkedListNode<T> NextOrFirst<T>(this LinkedListNode<T> current)
        {
            return (current.Next ?? current.List!.First)!;
        }
    }
}