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
        
        var input1 = solution.ReadFile(folder, 1);
        var stopwatch1 = Stopwatch.StartNew();
        var result1 = await solution.SolvePartOne(input1);
        stopwatch1.Stop();
        var elapsedMs1 = stopwatch1.ElapsedTicks / (Stopwatch.Frequency / 1000.0);
        Console.WriteLine($"Part 1: {result1} (in {elapsedMs1:0.0} ms)");
    

        var input2 = solution.ReadFile(folder, 2);
        var stopwatch2 = Stopwatch.StartNew();
        var result2 = await solution.SolvePartTwo(input2);
        stopwatch2.Stop();
        var elapsedMs2 = stopwatch2.ElapsedTicks / (Stopwatch.Frequency / 1000.0);
        Console.WriteLine($"Part 2: {result2} (in {elapsedMs2:0.0} ms)");
    
        
    }
}
