using AOC2024.Cli.Solutions;
using System.Diagnostics;

namespace AOC2024.Cli.Utils;

public static class ExecutionHelper
{

    public static async Task ExecuteAll()
    {
        var sw = Stopwatch.StartNew();
        foreach(var folder in Directory.GetDirectories("inputs"))
        {
            var folderName = Path.GetFileName(folder);
            foreach(var day in ImplementedSolutions.All)
            {
                await ExecuteSolution(day, folderName);
            }
        }
        sw.Stop();
        Console.WriteLine($"Total time: {sw.ElapsedMilliseconds} ms");
    }

    public static async Task ExecuteSolution(int day, string folder)
    {
        // Find all files in inputs\<folder> folder
        if (!Directory.Exists($"inputs/{folder}"))
        {
            Console.WriteLine($"error: Could not find folder 'inputs/{folder}'");
            return;
        }

        // Find the implementation for the given day
        var dayImpl = ImplementedSolutions.All.FirstOrDefault(d => d.DayNumber == day);
        if (dayImpl == null)
        {
            Console.WriteLine($"No implementation found for day {day}");
            return;
        }
        await ExecuteSolution(dayImpl, folder);
    }

    public static async Task ExecuteSolution(DayBase solution, string folder)
    {
        Console.WriteLine($"-- Day {solution.DayNumber}: {solution.Name} (input: '{folder}') --");
        try
        {
            var input = solution.ReadFile(folder, 1);
            var stopwatch = Stopwatch.StartNew();
            var result = await solution.SolvePartOne(input);
            stopwatch.Stop();
            var elapsedMs = stopwatch.ElapsedTicks / (Stopwatch.Frequency / 1000.0);
            Console.WriteLine($"Part 1: {result} (in {elapsedMs:0.0} ms)");
        }
        catch (Exception e)
        {
            Console.WriteLine($"Part 1: {e.Message} ({e.GetType().Name})");
        }

        try
        {
            var input = solution.ReadFile(folder, 2);
            var stopwatch = Stopwatch.StartNew();
            var result = await solution.SolvePartTwo(input);
            stopwatch.Stop();
            var elapsedMs = stopwatch.ElapsedTicks / (Stopwatch.Frequency / 1000.0);
            Console.WriteLine($"Part 2: {result} (in {elapsedMs:0.0} ms)");
        }
        catch (Exception e)
        {
            Console.WriteLine($"Part 2: {e.Message} ({e.GetType().Name})");
        }
    }
}
