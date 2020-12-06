using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Spectre.Console;

namespace AdventOfCode2020.Days
{
    public abstract class Day
    {
        protected abstract Task<string> Solve01Async(IEnumerable<string> input);
        protected abstract Task<string> Solve02Async(IEnumerable<string> input);

        public async Task Run()
        {
            try
            {
                var input = await File.ReadAllLinesAsync($"Input/{GetType().Name}.txt");

                AnsiConsole.Render(new Panel(await Solve01Async(input))
                {
                    Header = new PanelHeader("[bold blue]Solution 1[/]", Justify.Center),
                    Border = BoxBorder.Rounded,
                    Padding = new Padding(5, 1),
                });

                AnsiConsole.WriteLine();

                AnsiConsole.Render(new Panel(await Solve02Async(input))
                {
                    Header = new PanelHeader("[bold blue]Solution 2[/]", Justify.Center),
                    Border = BoxBorder.Rounded,
                    Padding = new Padding(5, 1),
                });
            }
            catch (Exception e)
            {
                AnsiConsole.WriteException(e, ExceptionFormats.ShortenPaths
                                              | ExceptionFormats.ShortenTypes
                                              | ExceptionFormats.ShortenMethods
                                              | ExceptionFormats.ShowLinks);
            }
        }
    }
}