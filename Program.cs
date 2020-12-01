using System.Threading.Tasks;
using AdventOfCode2020.Days;

namespace AdventOfCode2020
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var d = new Day01();
            await d.Run();
        }
    }
}