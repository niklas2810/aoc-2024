// Get command line arguments

using AOC2024.Cli.Utils;

if(args.Length == 0)
{
    await ExecutionHelper.ExecuteAll();
    return;
}

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

await ExecutionHelper.ExecuteSolution(programArgs.Day, programArgs.Folder);