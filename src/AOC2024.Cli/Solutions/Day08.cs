using System;

namespace AOC2024.Cli.Solutions;

public class Day08 : DayBase
{
    public override string Name => "Resonant Collinearity";

    private static IEnumerable<(int x, int y)> CalculateAntinodes((int x, int y)[] positions, int width, int height, bool limit)
    {
        for (var i = 0; i < positions.Length; ++i)
        {
            var (x1, y1) = positions[i];
            for (var j = i + 1; j < positions.Length; ++j)
            {
                var (x2, y2) = positions[j];
                var dx = x2 - x1;
                var dy = y2 - y1;

                var currX = x2;
                var currY = y2;
                while(true) {
                    currX += dx;
                    currY += dy;

                    if (currX < 0 || currX >= width || currY < 0 || currY >= height)
                        break;
                    yield return (currX, currY);
                    if(limit)
                        break;
                }

                currX = x1;
                currY = y1;

                while(true) {
                    currX -= dx;
                    currY -= dy;

                    if (currX < 0 || currX >= width || currY < 0 || currY >= height)
                        break;
                    yield return (currX, currY);
                    if(limit)
                        break;
                }
            }
        }
    }

    public override Task<long> SolvePartOne(IEnumerable<string> input)
    {
        // Parse input as a 2D map
        var map = input.Select(x => x.ToCharArray()).ToArray();
        var width = map[0].Length;
        var height = map.Length;

        // Empty space is '.'. Collect all other positions and group them by char
        var positions = map.SelectMany((row, y) => row.Select((c, x) => (c, x, y)))
            .Where(p => p.c != '.')
            .GroupBy(p => p.c)
            .ToDictionary(g => g.Key, g => g.Select(p => (p.x, p.y)).ToArray());

        var antinodes = new HashSet<(int x, int y)>();

        foreach (var c in positions.Keys)
        {
            foreach (var node in CalculateAntinodes(positions[c], width, height, true))
                antinodes.Add(node);
        }

        return Task.FromResult((long)antinodes.Count);
    }

    public override Task<long> SolvePartTwo(IEnumerable<string> input)
    {
                // Parse input as a 2D map
        var map = input.Select(x => x.ToCharArray()).ToArray();
        var width = map[0].Length;
        var height = map.Length;

        // Empty space is '.'. Collect all other positions and group them by char
        var positions = map.SelectMany((row, y) => row.Select((c, x) => (c, x, y)))
            .Where(p => p.c != '.')
            .GroupBy(p => p.c)
            .ToDictionary(g => g.Key, g => g.Select(p => (p.x, p.y)).ToArray());

        var antinodes = new HashSet<(int x, int y)>();

        foreach (var c in positions.Keys)
        {
            foreach (var node in CalculateAntinodes(positions[c], width, height, false))
                antinodes.Add(node);
            foreach(var (x, y) in positions[c])
                antinodes.Add((x, y));
        }

        return Task.FromResult((long)antinodes.Count);
    }
}
