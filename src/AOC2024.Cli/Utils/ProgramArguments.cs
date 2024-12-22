using System;

public class ProgramArguments
{
    public int Day { get; }
    public string Folder { get; }

    public ProgramArguments(string[] args)
    {
        if (args.Length < 2)
        {
            throw new ArgumentException("Syntax: aoc2024 <day> <folder>");
        }

        var dayString = args[0];
        if (!int.TryParse(dayString, out var day))
        {
            throw new ArgumentException("argument must be a number", "day");
        }

        Day = day;
        Folder = args[1];
    }
}
