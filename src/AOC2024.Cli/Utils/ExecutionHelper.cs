using AOC2024.Cli.Solutions;
using System.Diagnostics;

namespace AOC2024.Cli.Utils;

public class ExecutionHelper
{
    public static async Task ExecuteSolution(DayBase solution, string folder) 
    {
        try {
            var input = solution.ReadFile(folder, 1);
            var stopwatch = Stopwatch.StartNew();
            var result = await solution.SolvePartOne(input);
            stopwatch.Stop();
            var elapsedMs = stopwatch.ElapsedTicks / (Stopwatch.Frequency / 1000.0);
            Console.WriteLine($"Part 1: {result} (in {elapsedMs:0.0} ms)");
        } catch (Exception e) {
            Console.WriteLine($"Part 1: {e.Message} ({e.GetType().Name})");
        }

        try {
            var input = solution.ReadFile(folder, 2);
            var stopwatch = Stopwatch.StartNew();
            var result = await solution.SolvePartTwo(input);
            stopwatch.Stop();
            var elapsedMs = stopwatch.ElapsedTicks / (Stopwatch.Frequency / 1000.0);
            Console.WriteLine($"Part 2: {result} (in {elapsedMs:0.0} ms)");
        } catch (Exception e) {
            Console.WriteLine($"Part 2: {e.Message} ({e.GetType().Name})");
        }
    }
}
