using System;

namespace AOC2024.Cli.Solutions;

public class Day12 : DayBase
{
    public override string Name => "Garden Groups";

    private static int CalculatePerimeter(HashSet<(int x, int y)> region, bool onlySides)
    {
        var perimeter = 0;

        // Inspect every cell in the region. If the neighboring cell is not in the region, increment the perimeter

        foreach(var (x, y) in region)
        {
            // Up
            if(!region.Contains((x, y-1)))
            {
                if(!onlySides) // We want the full perimeter
                    perimeter++;

                // Only count if left-most cell of the row (otherwise we are inside a line)
                else if(!region.Contains((x-1, y)) || region.Contains((x-1, y-1)))
                    perimeter++;
                
            }

            // Right
            if(!region.Contains((x+1, y)))
            {
                if(!onlySides) // We want the full perimeter
                    perimeter++;

                // Only count if top-most cell of the column (otherwise we are inside a line)
                else if(!region.Contains((x, y-1)) || region.Contains((x+1, y-1)))
                    perimeter++;
            }

            // Down
            if(!region.Contains((x, y+1)))
            {
                if(!onlySides) // We want the full perimeter
                    perimeter++;

                // Only count if left-most cell of the row (otherwise we are inside a line)
                else if(!region.Contains((x-1, y)) || region.Contains((x-1, y+1)))
                    perimeter++;
            }

            // Left
            if(!region.Contains((x-1, y)))
            {
                if(!onlySides) // We want the full perimeter
                    perimeter++;

                // Only count if top-most cell of the column (otherwise we are inside a line)
                else if(!region.Contains((x, y-1)) || region.Contains((x-1, y-1)))
                    perimeter++;
            }
        }

        return perimeter;
    }

    public override Task<long> SolvePartOne(IEnumerable<string> input)
    {
        // Input is a 2d grid, each cell is a char
        var grid = input.Select(x => x.ToCharArray()).ToArray();
        var height = grid.Length;
        var width = grid[0].Length;

        // Find contigous regions of the same characters in the grid. Calculate their area and the perimeter
        var result = 0L;
        var visited = new bool[height, width];

        for (var y = 0; y < height; y++)
        {
            for (var x = 0; x < width; x++)
            {
                if (visited[y, x])
                    continue;

                var region = new HashSet<(int, int)>();
                var queue = new Queue<(int, int)>();
                queue.Enqueue((x, y));
                visited[y, x] = true;

                var c = grid[y][x];
                while (queue.Count > 0)
                {
                    var (cx, cy) = queue.Dequeue();
                    region.Add((cx, cy));

                    // Check neighbors. If they are the same character, add them to the queue.
                    // For each other neighbor, increment the perimeter
                    foreach (var (dx, dy) in new[] { (0, 1), (0, -1), (1, 0), (-1, 0) })
                    {
                        var nx = cx + dx;
                        var ny = cy + dy;

                        if (nx < 0 || nx >= height || ny < 0 || ny >= width)
                            continue;
                        
                        if (grid[ny][nx] != c)
                            continue;
                        
                        if (visited[ny, nx])
                            continue;
                        
                        visited[ny, nx] = true;
                        queue.Enqueue((nx, ny));
                    }
                }

                var area = region.Count;
                var perimeter = CalculatePerimeter(region, false);
                result += area * perimeter;
            }
        }

        return Task.FromResult(result);
    }

    public override Task<long> SolvePartTwo(IEnumerable<string> input)
    {
        // Input is a 2d grid, each cell is a char
        var grid = input.Select(x => x.ToCharArray()).ToArray();
        var height = grid.Length;
        var width = grid[0].Length;

        // Find contigous regions of the same characters in the grid. Calculate their area and the perimeter
        var result = 0L;
        var visited = new bool[height, width];

        for (var y = 0; y < height; y++)
        {
            for (var x = 0; x < width; x++)
            {
                if (visited[y, x])
                    continue;

                var region = new HashSet<(int, int)>();
                var queue = new Queue<(int, int)>();
                queue.Enqueue((x, y));
                visited[y, x] = true;

                var c = grid[y][x];
                while (queue.Count > 0)
                {
                    var (cx, cy) = queue.Dequeue();
                    region.Add((cx, cy));

                    // Check neighbors. If they are the same character, add them to the queue.
                    // For each other neighbor, increment the perimeter
                    foreach (var (dx, dy) in new[] { (0, 1), (0, -1), (1, 0), (-1, 0) })
                    {
                        var nx = cx + dx;
                        var ny = cy + dy;

                        if (nx < 0 || nx >= height || ny < 0 || ny >= width)
                            continue;
                        
                        if (grid[ny][nx] != c)
                            continue;
                        
                        if (visited[ny, nx])
                            continue;
                        
                        visited[ny, nx] = true;
                        queue.Enqueue((nx, ny));
                    }
                }

                var area = region.Count;
                var perimeter = CalculatePerimeter(region, true);
                result += area * perimeter;
            }
        }


        return Task.FromResult(result);
    }
}
