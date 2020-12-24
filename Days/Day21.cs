using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdventOfCode2020.Days
{
    internal class Day21 : Day
    {
        private static (List<string> food, Dictionary<string, List<string>> allergens) ParseIngredients(
            IEnumerable<string> input)
        {
            var allergens = new Dictionary<string, List<string>>();
            var foods = new List<string>();

            foreach (string line in input)
            {
                string[] parts = line[..^1].Split(" (contains ");
                string[] foodPart = parts[0].Split(" ");
                string[] allergensPart = parts[1].Split(", ");

                foreach (string allergen in allergensPart)
                {
                    allergens[allergen] = !allergens.ContainsKey(allergen)
                        ? foodPart.ToList()
                        : allergens[allergen].Where(currentAllergen => foodPart.Contains(currentAllergen)).ToList();
                }

                foods.AddRange(foodPart);
            }

            return (foods, allergens);
        }

        /// <inheritdoc />
        protected override async Task<object> Solve01Async(IEnumerable<string> input)
        {
            var (foods, allergens) = ParseIngredients(input);

            var allergenicFoods = allergens.SelectMany(kvp => kvp.Value).ToHashSet();
            return await Task.FromResult(foods.Count(food => !allergenicFoods.Contains(food)));
        }

        /// <inheritdoc />
        protected override async Task<object> Solve02Async(IEnumerable<string> input)
        {
            var (_, allergens) = ParseIngredients(input);
            var dangerousIngredients = new Dictionary<string, string>();

            while (allergens.Any())
            {
                var (allergen, foods) = allergens.First(kvp => kvp.Value.Count == 1);
                allergens.Remove(allergen);

                dangerousIngredients[allergen] = foods.Single();

                foreach (var allergenKvp in allergens)
                {
                    allergenKvp.Value.Remove(dangerousIngredients[allergen]);
                }
            }

            return await Task.FromResult(
                string.Join(
                    ',',
                    dangerousIngredients
                        .OrderBy(kvp => kvp.Key)
                        .Select(kvp => kvp.Value)
                )
            );
        }
    }
}