// Get command line arguments

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
var inputFiles = Directory.GetFiles($"inputs/{programArgs.Folder}", $"{programArgs.DayString}*.txt");

if (inputFiles.Length == 0)
{
    Console.WriteLine($"No input files found for day '{programArgs.DayString}' in folder 'inputs/{programArgs.Folder}'");
    return;
}

Console.WriteLine($"-- Day {programArgs.Day}, source '{programArgs.Folder}' --");

Console.WriteLine("<Execute code here>");