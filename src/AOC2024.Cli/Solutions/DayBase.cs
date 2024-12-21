using System;

namespace AOC2024.Cli.Solutions;


public abstract class DayBase
{
    private static readonly char[] digits = Enumerable.Range('0', 10).Select(i => (char)i).ToArray();



    public abstract string Name { get; }

    public DayBase() {
        DayNumber = int.Parse(GetType().Name.Substring(GetType().Name.IndexOfAny(digits), 2));
    }

    public int DayNumber { get; }

    public string DayNumberString => DayNumber.ToString("00");

    private IEnumerable<string> GetAvailableFiles(string folder)
    {
        var path = Path.Combine("inputs", folder);
        if (!Directory.Exists(path))
            return [];

        return Directory.GetFiles(path).Select(path => Path.GetFileName(path)).Where(f => f.StartsWith(DayNumberString));
    }

    public IEnumerable<string> ReadFile(string folder, int part)
    {
        var files = GetAvailableFiles(folder).ToList();

        if (files.Count == 0)
            throw new FileNotFoundException($"No files found for day {DayNumber}");

        var partName = $"{DayNumberString}-p{part}.txt";
        if (files.Contains(partName))
            return ReadFile(folder, partName);
        return ReadFile(folder, $"{DayNumberString}.txt");
    }

    private IEnumerable<string> ReadFile(string folder, string filename)
    {       
        var path = Path.Combine("inputs", folder, filename);
        if (!File.Exists(path))
            throw new FileNotFoundException($"'{path}' does not exist");

        return File.ReadAllLines(path);
    }

    public abstract Task<long> SolvePartOne(IEnumerable<string> input);
    public abstract Task<long> SolvePartTwo(IEnumerable<string> input);
}