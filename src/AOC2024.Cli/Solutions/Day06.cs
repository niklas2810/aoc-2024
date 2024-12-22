namespace AOC2024.Cli.Solutions;

public class Day06 : DayBase
{
    public override string Name => "Guard Gallivant";

    // Horizontal and vertical directions
    private static (int h, int v) TurnRight((int h, int v) direction) => direction switch
    {
        (0, -1) => (1, 0), // Up -> Right
        (1, 0) => (0, 1), // Right -> Down
        (0, 1) => (-1, 0), // Down -> Left
        (-1, 0) => (0, -1), // Left -> Up
        _ => throw new InvalidOperationException()
    };

    private static (int x, int y) Move((int x, int y) position, (int h, int v) direction) =>
        (position.x + direction.h, position.y + direction.v);

    private static List<(int x, int y, int h, int v)> GetVisited(int width, int height, (int x, int y) position, (int h, int v) direction, HashSet<(int x, int y)> barriers)
    {
        var visited = new List<(int x, int y, int h, int v)>();
        while (true)
        {
            var next = Move(position, direction);
            // Check if we moved out of the grid
            if (next.x < 0 || next.x >= width || next.y < 0 || next.y >= height)
                break;

            if (barriers.Contains(next))
            {
                direction = TurnRight(direction);
                continue;
            }
            position = next;
            if (visited.Contains((position.x, position.y, direction.h, direction.v)))
            {
                throw new InvalidOperationException("Infinite loop");
            }
            visited.Add((position.x, position.y, direction.h, direction.v));
        }
        return visited;
    }

    private static bool IsLoop(int width, int height, (int x, int y) position, (int h, int v) direction, HashSet<(int x, int y)> barriers, IEnumerable<(int x, int y, int h, int v)> alreadyVisited)
    {
        var visited = new HashSet<(int x, int y, int h, int v)>(alreadyVisited);
        while (true)
        {
            var next = Move(position, direction);
            // Check if we moved out of the grid
            if (next.x < 0 || next.x >= width || next.y < 0 || next.y >= height)
                return false;

            if (barriers.Contains(next))
            {
                direction = TurnRight(direction);
                continue;
            }
            position = next;
            if (visited.Contains((position.x, position.y, direction.h, direction.v)))
                return true;
            visited.Add((position.x, position.y, direction.h, direction.v));
        }
    }

    public override Task<long> SolvePartOne(IEnumerable<string> input)
    {
        // Parse input as 2D grid
        var grid = input.Select(x => x.ToCharArray()).ToArray();
        var width = grid[0].Length;
        var height = grid.Length;

        // Find barriers ('#') and store their coordinates for efficient lookup
        var barriers = grid.SelectMany((row, y) => row.Select((cell, x) => (cell, x, y)))
            .Where(x => x.cell == '#')
            .Select(x => (x.x, x.y))
            .ToHashSet();
        // Find start position ('^')
        var start = grid.SelectMany((row, y) => row.Select((cell, x) => (cell, x, y)))
            .Single(x => x.cell == '^');

        // Start direction is up, every time we hit a barrier we turn right
        var visited = GetVisited(width, height, (start.x, start.y), (0, -1), barriers);

        // Count unique x y positions
        var unique = (long)visited.Select(x => (x.x, x.y)).Distinct().Count();

        return Task.FromResult(unique);
    }

    public override Task<long> SolvePartTwo(IEnumerable<string> input)
    {
        // Parse input as 2D grid
        var grid = input.Select(x => x.ToCharArray()).ToArray();
        var width = grid[0].Length;
        var height = grid.Length;

        // Find barriers ('#') and store their coordinates for efficient lookup
        var barriers = grid.SelectMany((row, y) => row.Select((cell, x) => (cell, x, y)))
            .Where(x => x.cell == '#')
            .Select(x => (x.x, x.y))
            .ToHashSet();
        // Find start position ('^')
        var start = grid.SelectMany((row, y) => row.Select((cell, x) => (cell, x, y)))
            .Single(x => x.cell == '^');

        // Start direction is up, every time we hit a barrier we turn right
        var visited = GetVisited(width, height, (start.x, start.y), (0, -1), barriers);

        // Find out when each position is first visited.
        // Filter out the guard position and the position above the guard.
        var posLookupTable = visited
            .Where(e => e.x != start.x || e.y != start.y)
            .Select((pos, index) => (pos.x, pos.y, index))
            .GroupBy(e => (e.x, e.y))
            .ToDictionary(g => g.Key, g => g.Min(e => e.index));
        posLookupTable.Remove((visited[0].x, visited[0].y));

        // Count all visited positions where placing a barrier would cause a loop.
        var loopOptions = (long)posLookupTable.Keys.AsParallel().Count(v =>
        {
            var localBarriers = new HashSet<(int x, int y)>(barriers) { (v.x, v.y) };
            var minBarrier = posLookupTable[(v.x, v.y)];
            var prev = visited[minBarrier - 1]; 

            // We start traversing again directly in front of the barrier
            return IsLoop(width, height, (prev.x, prev.y), (prev.h, prev.v), localBarriers, visited[..minBarrier]);
        });

        return Task.FromResult(loopOptions);
    }
}
