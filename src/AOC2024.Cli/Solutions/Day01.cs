using System;

namespace AOC2024.Cli.Solutions;

public class Day01 : DayBase
{

    public override string Name => "Historian Hysteria";

    public override async Task<long> SolvePartOne(IEnumerable<string> input)
    {
        var left = new List<int>();
        var right = new List<int>();

        foreach (var line in input)
        {
            if (line.Length == 0)
                continue;

            left.Add(int.Parse(line[..line.IndexOf(' ')]));
            right.Add(int.Parse(line[(line.LastIndexOf(' ') + 1)..]));
        }

        var leftIndices = new HashSet<int>();
        var rightIndices = new HashSet<int>();

        var sum = 0L;

        // Calculate distances
        for (int i = 0; i < left.Count; ++i)
        {
            // Find **smallest** number in both lists which aren't in the indices yet. Also save the index.
            var (minLeftNum, minLeftIndex) = left.Select((num, index) => (num, index)).Where(x => !leftIndices.Contains(x.index)).MinBy(pair => pair.num);
            leftIndices.Add(minLeftIndex);

            var (minRightNum, minRightIndex) = right.Select((num, index) => (num, index)).Where(x => !rightIndices.Contains(x.index)).MinBy(pair => pair.num);
            rightIndices.Add(minRightIndex);

            // Calculate distance
            var distance = Math.Abs(minLeftNum - minRightNum);
            sum += distance;
        }

        await Task.CompletedTask;
        return sum;
    }

    public override async Task<long> SolvePartTwo(IEnumerable<string> input)
    {
        var left = new List<int>();
        var right = new List<int>();

        foreach (var line in input)
        {
            if (line.Length == 0)
                continue;

            left.Add(int.Parse(line[..line.IndexOf(' ')]));
            right.Add(int.Parse(line[(line.LastIndexOf(' ') + 1)..]));
        }

        var sum = 0L;
        var cache = new Dictionary<int, int>();
        for (int i = 0; i < left.Count; ++i)
        {
            var num = left[i];
            if (!cache.TryGetValue(num, out var similarity))
            {
                similarity = right.Count(x => x == num) * num;
                cache[num] = similarity;
            }
            sum += similarity;
        }

        await Task.CompletedTask;
        return sum;
    }
}
