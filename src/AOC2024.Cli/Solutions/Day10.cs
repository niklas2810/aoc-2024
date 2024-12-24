namespace AOC2024.Cli.Solutions;

public class Day10 : DayBase
{
    public override string Name => "Hoof It";

    private static IEnumerable<(int x, int y)> GetNeighbors(int x, int y, int[][] map)
    {
        var width = map[0].Length;
        var height = map.Length;

        var value = map[y][x];

        // Only consider neighbors that are exactly 1 higher.
        if (x > 0 && map[y][x - 1] == value + 1)
            yield return (x - 1, y);
        if (x < width - 1 && map[y][x + 1] == value + 1)
            yield return (x + 1, y);
        if (y > 0 && map[y - 1][x] == value + 1)
            yield return (x, y - 1);
        if (y < height - 1 && map[y + 1][x] == value + 1)
            yield return (x, y + 1);
    }

    private static IEnumerable<(int x, int y)> GetTrails(int[][] map, (int x, int y) startPosition) {
        // Create queue of lookup positions.
        var lookupPositions = new Queue<(int x, int y)>();

        // Add the starting position to the queue.
        lookupPositions.Enqueue(startPosition);

        // Step through all loolup positions.
        // We aim for a steady slope, meaning we start at 0, search for 1, then 2, etc.
        // 9 is the peak, so we stop when we find it and add it to the peaks list.
        // Increase the steps for each step we take.
        
        while(lookupPositions.Count > 0) {
            var (x, y) = lookupPositions.Dequeue();
            var value = map[y][x];

            // All neighbors will be peaks.
            if(value == 8) {
                //Console.WriteLine($"!!At {x}, {y} with value {value} and {steps} steps.");
                foreach(var neighbor in GetNeighbors(x, y, map)) {
                    yield return neighbor;
                }
            } else {
                //Console.WriteLine($"At {x}, {y} with value {value} and {steps} steps.");
                foreach(var neighbor in GetNeighbors(x, y, map)) {
                    lookupPositions.Enqueue(neighbor);
                }
            }
        }
    }

    public override Task<long> SolvePartOne(IEnumerable<string> input)
    {
        // The input is a 2D map of digits (0-9).
        var map = input.Select(x => x.Select(c => c - '0').ToArray()).ToArray();
        var width = map[0].Length;
        var height = map.Length;

        var sum = 0L;
        // A trailhead is a "0" digit. Find all on the map.
        for (var y = 0; y < height; y++)
        {
            for (var x = 0; x < width; x++)
            {
                if (map[y][x] == 0)
                {
                    var startPosition = (x, y);
                    var score = GetTrails(map, startPosition).Distinct().Count(); // Pt1: Only count distinct peaks.                 
                    sum += score;
                }
            }
        }

        return Task.FromResult(sum);
    }

    public override Task<long> SolvePartTwo(IEnumerable<string> input)
    {

        // The input is a 2D map of digits (0-9).
        var map = input.Select(x => x.Select(c => c - '0').ToArray()).ToArray();
        var width = map[0].Length;
        var height = map.Length;

        var sum = 0L;
        // A trailhead is a "0" digit. Find all on the map.
        for (var y = 0; y < height; y++)
        {
            for (var x = 0; x < width; x++)
            {
                if (map[y][x] == 0)
                {
                    var startPosition = (x, y);
                    var score = GetTrails(map, startPosition).Count(); // Pt2: Count all trails to the peak.               
                    sum += score;
                }
            }
        }

        return Task.FromResult(sum);
    }
}
