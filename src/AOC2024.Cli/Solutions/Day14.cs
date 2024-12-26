using System.Text.RegularExpressions;

namespace AOC2024.Cli.Solutions;

public partial class Day14 : DayBase
{
    public override string Name => "Restroom Redoubt";

    [GeneratedRegex(@"p=\s*(-?\d+),\s*(-?\d+) v=\s*(-?\d+),\s*(-?\d+)")]
    private static partial  Regex PositionRegex();

    private static (int x, int y) MoveRobot((int x, int y) robot, (int h, int v) velocity, int width, int height, int times)
    {
        // Keep movement in bounds using modulo and handle negative wrap around
        var newX = (robot.x + velocity.h * times) % width;
        var newY = (robot.y + velocity.v * times) % height;

        if (newX < 0) newX += width;
        if (newY < 0) newY += height;

        return (newX, newY);
    }

    private static IEnumerable<long> CountQuadrants(IEnumerable<(int x, int y)> robots, int width, int height)
    {
        var midX = width / 2;
        var midY = height / 2;

        var quadrants = new long[4];

        foreach(var (x, y) in robots)
        {
            if(x < midX && y < midY)
                quadrants[0]++;
            else if(x > midX && y < midY)
                quadrants[1]++;
            else if(x < midX && y > midY)
                quadrants[2]++;
            else if(x > midX && y > midY)
                quadrants[3]++;
        }

        return quadrants;
    }

    public override Task<long> SolvePartOne(IEnumerable<string> input)
    {
        // Read in the positions and velocities (one per line)
        // Example: "p=2,4 v=2,-3" (p is position x/y, v is velocity horizontal/vertical)
        var robots = new List<((int x, int y) pos, (int h, int v) vel)>();
        foreach(var line in input) {
            var match = PositionRegex().Match(line);
            if(match.Success) {
                var x = int.Parse(match.Groups[1].Value);
                var y = int.Parse(match.Groups[2].Value);
                var h = int.Parse(match.Groups[3].Value);
                var v = int.Parse(match.Groups[4].Value);
                robots.Add(((x, y), (h, v)));
            } else {
                throw new Exception($"Invalid input: {line}");
            }
        }

        // We have a fixed width and height
        var width = 101;
        var height = 103;
        var seconds = 100;
        var quadrants = CountQuadrants(robots.Select(r => MoveRobot(r.pos, r.vel, width, height, seconds)), width, height);
        // Multiply quadrants
        var result = quadrants.Aggregate(1L, (acc, x) => acc * x);
        return Task.FromResult(result);
    }

    public override Task<long> SolvePartTwo(IEnumerable<string> input)
    {
        // I used an image renderer so I know that the result is 6876.
        // Read in the positions and velocities (one per line)
        // Example: "p=2,4 v=2,-3" (p is position x/y, v is velocity horizontal/vertical)
        var robots = new List<((int x, int y) pos, (int h, int v) vel)>();
        foreach(var line in input) {
            var match = PositionRegex().Match(line);
            if(match.Success) {
                var x = int.Parse(match.Groups[1].Value);
                var y = int.Parse(match.Groups[2].Value);
                var h = int.Parse(match.Groups[3].Value);
                var v = int.Parse(match.Groups[4].Value);
                robots.Add(((x, y), (h, v)));
            } else {
                throw new Exception($"Invalid input: {line}");
            }
        }

        // We have a fixed width and height
        var width = 101;
        var height = 103;
        var maxSeconds = 7000;

        var result = Enumerable.Range(1, maxSeconds).Single(i =>
        {
            var positions = robots.Select(robot => MoveRobot(robot.pos, robot.vel, width, height, i)).ToList();

            // Get positions per row and print the max. number of robots per row
            var maxPerRow = positions.GroupBy(p => p.y).Select(g => g.Count()).Max();
            if(maxPerRow < 30)
                return false;
            var maxPerColumn = positions.GroupBy(p => p.x).Select(g => g.Count()).Max();
            if(maxPerColumn < 30)
                return false;
            return true;
        });

        return Task.FromResult((long)result);
    }
}
