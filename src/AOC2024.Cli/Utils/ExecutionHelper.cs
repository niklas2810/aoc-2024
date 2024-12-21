using System;
using AOC2024.Cli.Solutions;
using System.Diagnostics;

namespace AOC2024.Cli.Utils;

public class ExecutionHelper
{

    private static bool TryGetInputFile(DayBase solution, string folder, int part, out IEnumerable<string> input)
    {
        try
        {
            input = solution.ReadFile(folder, part);
            return true;
        }
        catch (FileNotFoundException e)
        {
            Console.WriteLine($"Failed to load input for part {part}: {e.Message} ({e.GetType().Name})");
            input = [];
            return false;
        }
    }

    public static async Task ExecuteSolution(DayBase solution, string folder) 
    {
        try {
            var stopwatch = Stopwatch.StartNew();
            var input = solution.ReadFile(folder, 1);
            var result = await solution.SolvePartOne(input);
            stopwatch.Stop();
            Console.WriteLine($"Part 1: {result} (in {stopwatch.ElapsedMilliseconds:0.0} ms)");
        } catch (Exception e) {
            Console.WriteLine($"Part 1: {e.Message} ({e.GetType().Name})");
        }

        try {
            var stopwatch = Stopwatch.StartNew();
            var input = solution.ReadFile(folder, 2);
            var result = await solution.SolvePartTwo(input);
            stopwatch.Stop();
            Console.WriteLine($"Part 2: {result} (in {stopwatch.ElapsedMilliseconds:0.0} ms)");
        } catch (Exception e) {
            Console.WriteLine($"Part 2: {e.Message} ({e.GetType().Name})");
        }
    }
}
