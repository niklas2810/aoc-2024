using System;

namespace AOC2024.Cli.Solutions;

public class Day02 : DayBase
{
    public override string Name => "Red-Nosed Reports";

    private static bool IsSafe(string line)
    {
        // Each line consists  numbers separated by a space
        var numbers = line.Split(' ').Select(int.Parse).ToArray();

        // 1. The levels are either all increasing or all decreasing.
        if(numbers[0] < numbers[1]) {
            // Check if all numbers are increasing
            for (int i = 1; i < numbers.Length - 1; ++i)
            {
                if (numbers[i] >= numbers[i + 1])
                    return false;
            }
        }
        else {
            // Decreasing
            for(int i = 1; i < numbers.Length - 1; ++i)
            {
                if(numbers[i] <= numbers[i + 1])
                    return false;
            }
        }

        // 2. Any two adjacent levels differ by at least one and at most three.
        for(int i = 0; i < numbers.Length - 1; ++i)
        {
            var distance = Math.Abs(numbers[i] - numbers[i + 1]);
            if(distance < 1 || distance > 3)
                return false;
        }
        return true;
    }

    private static bool IsSafeTwo(string line) 
    {
        // If removing a single number from the list would make the list safe, return true
        // Therefore, loop through all numbers and check if the list is safe without the current number
        var numbers = line.Split(' ').Select(int.Parse).ToArray();
        for(int i = 0; i < numbers.Length; ++i)
        {
            var newNumbers = numbers.Where((_, index) => index != i).ToArray();
            if(IsSafe(string.Join(' ', newNumbers)))
                return true;
        }

        return false;
    }

    public override async Task<long> SolvePartOne(IEnumerable<string> input)
    {
        await Task.CompletedTask;
        return input.AsParallel().Where(IsSafe).Count();
    }

    public override async Task<long> SolvePartTwo(IEnumerable<string> input)
    {
        await Task.CompletedTask;
        return input.AsParallel().Where(IsSafeTwo).Count();
    }
}
