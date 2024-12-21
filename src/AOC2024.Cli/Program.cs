// Get command line arguments

using AOC2024.Cli.Solutions;
using AOC2024.Cli.Utils;

ProgramArguments? programArgs = null;

try {
    programArgs = new ProgramArguments(args);
}
catch (Exception ex)
{
    Console.WriteLine($"error: {ex.Message}");
}

if (programArgs == null)
{
    Console.WriteLine("Failed to parse program arguments");
    return;
}

// Find all files in inputs\<folder> folder
if(!Directory.Exists($"inputs/{programArgs.Folder}"))
{
    Console.WriteLine($"error: Could not find folder 'inputs/{programArgs.Folder}'");
    return;
}

// Find all implementations of DayBase
var dayBaseType = typeof(DayBase);
var dayImpls = new List<DayBase>
{
    new Day01()
};

// Find the implementation for the given day
var dayImpl = dayImpls.FirstOrDefault(d => d.DayNumber == programArgs.Day);
if (dayImpl == null)
{
    Console.WriteLine($"No implementation found for day {programArgs.Day}");
    return;
}

Console.WriteLine($"-- Day {dayImpl.DayNumber}: {dayImpl.Name} (from '{programArgs.Folder}') --");

await ExecutionHelper.ExecuteSolution(dayImpl, programArgs.Folder);