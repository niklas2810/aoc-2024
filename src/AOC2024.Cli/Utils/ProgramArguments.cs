using System;

public class ProgramArguments
{
    public int Day { get; }
    public string DayString { get; }
    public string Folder { get; }

    public ProgramArguments(string[] args)
    {
        if (args.Length < 2)
        {
            throw new ArgumentException("Please provide day number as argument");
        }

        var dayString = args[0];
        if (!int.TryParse(dayString, out var day))
        {
            throw new ArgumentException("argument must be a number", "day");
        }

        Day = day;
        DayString = day < 10 ? $"0{day}" : day.ToString();
        Folder = args[1];
    }
}
