using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdventOfCode2020.Days
{
    internal class Day22 : Day
    {
        

        private static (Queue<int> deck1, Queue<int> deck2) ParseDecks(IEnumerable<string> input)
        {
            var inputList = input.ToList();

            var deck1 = new Queue<int>(inputList.Skip(1).TakeWhile(line => !string.IsNullOrEmpty(line))
                .Select(int.Parse));
            var deck2 = new Queue<int>(inputList.SkipWhile(line => !string.IsNullOrEmpty(line)).Skip(2)
                .Select(int.Parse));

            return (deck1, deck2);
        }

        private static long GetSequenceHashCode<T>(IEnumerable<T> sequence)
            => sequence.Aggregate(487, (current, item) => current * 31 + item!.GetHashCode());

        private static string HashDecks(IEnumerable<int> deck1, IEnumerable<int> deck2)
            => GetSequenceHashCode(deck1) + "-" + GetSequenceHashCode(deck2);

        private static (Queue<int> deck1, Queue<int> deck2) Play(Queue<int> deck1, Queue<int> deck2, bool part2 = false)
        {
            var seenGames = new HashSet<string>();

            while (deck1.Any() && deck2.Any())
            {
                if (part2 && !seenGames.Add(HashDecks(deck1, deck2)))
                {
                    return (deck1, new Queue<int>());
                }
                
                int card1 = deck1.Dequeue();
                int card2 = deck2.Dequeue();

                if (part2 && card1 <= deck1.Count && card2 <= deck2.Count)
                {
                    var (subDeck1, _) = Play(
                        new Queue<int>(deck1.Take(card1)),
                        new Queue<int>(deck2.Take(card2)),
                        true
                    );

                    if (subDeck1.Any())
                    {
                        deck1.Enqueue(card1);
                        deck1.Enqueue(card2);
                    }
                    else
                    {
                        deck2.Enqueue(card2);
                        deck2.Enqueue(card1);
                    }
                }
                else if (card1 > card2)
                {
                    deck1.Enqueue(card1);
                    deck1.Enqueue(card2);
                }
                else
                {
                    deck2.Enqueue(card2);
                    deck2.Enqueue(card1);
                }
            }

            return (deck1, deck2);
        }

        private static int Score(IEnumerable<int> deck)
        {
            int i = 1;
            return deck.Reverse().Aggregate(0, (acc, card) => acc + card * i++);
        }

        /// <inheritdoc />
        protected override async Task<object> Solve01Async(IEnumerable<string> input)
        {
            (Queue<int> deck1, Queue<int> deck2) = ParseDecks(input);
            (deck1, deck2) = Play(deck1, deck2);

            var winner = deck1.Any() ? deck1 : deck2;

            return await Task.FromResult(Score(winner));
        }

        /// <inheritdoc />
        protected override async Task<object> Solve02Async(IEnumerable<string> input)
        {
            (Queue<int> deck1, Queue<int> deck2) = ParseDecks(input);
            (deck1, deck2) = Play(deck1, deck2, true);

            var winner = deck1.Any() ? deck1 : deck2;

            return await Task.FromResult(Score(winner));
        }
    }
}