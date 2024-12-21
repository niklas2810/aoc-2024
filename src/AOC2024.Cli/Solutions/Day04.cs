using System;
using System.Security.Cryptography.X509Certificates;

namespace AOC2024.Cli.Solutions;

public class Day04 : DayBase
{
    public override string Name => "Ceres Search";

    public override Task<long> SolvePartOne(IEnumerable<string> input)
    {
        // Read in input as 2D grid of characters
        var grid = input.Select(x => x.ToCharArray()).ToArray();

        var width = grid[0].Length;
        var height = grid.Length;

        // We are looking for occurrences of 'XMAS' in the grid
        // This can be horizontal, vertical or diagonal
        // Also, this can all be backwards!

        long matches = 0;

        // I'm sorry. I'm so sorry.
        var checkerFunctions = new List<Func<int, int, bool>> {
            (x, y) => { // Horizontal
                if(x+3 < width && grid[y][x + 1].Equals('M') && grid[y][x + 2].Equals('A') && grid[y][x + 3].Equals('S')) {
                    return true;
                }
                return false;
            },
            (x, y) => { // Horizontal backwards
                if(x-3 >= 0 && grid[y][x - 1].Equals('M') && grid[y][x - 2].Equals('A') && grid[y][x - 3].Equals('S')) {
                    return true;
                }
                return false;
            },
            (x, y) => { // Vertical
                if(y+3 < height && grid[y + 1][x].Equals('M') && grid[y + 2][x].Equals('A') && grid[y + 3][x].Equals('S')) {
                    return true;
                }
                return false;
            },
            (x, y) => { // Vertical backwards
                if(y-3 >= 0 && grid[y - 1][x].Equals('M') && grid[y - 2][x].Equals('A') && grid[y - 3][x].Equals('S')) {
                    return true;
                }
                return false;
            },
            (x, y) => { // Diagonal down-right
                if(x+3 < width && y+3 < height && grid[y + 1][x + 1].Equals('M') && grid[y + 2][x + 2].Equals('A') && grid[y + 3][x + 3].Equals('S')) {
                    return true;
                }
                return false;
            },
            (x, y) => { // Diagonal down-left
                if(x-3 >= 0 && y+3 < height && grid[y + 1][x - 1].Equals('M') && grid[y + 2][x - 2].Equals('A') && grid[y + 3][x - 3].Equals('S')) {
                    return true;
                }
                return false;
            },
            (x, y) => { // Diagonal up-left
                if(x-3 >= 0 && y-3 >= 0 && grid[y - 1][x - 1].Equals('M') && grid[y - 2][x - 2].Equals('A') && grid[y - 3][x - 3].Equals('S')) {
                    return true;
                }
                return false;
            },
            (x, y) => { // Diagonal up-right
                if(x+3 < width && y-3 >= 0 && grid[y - 1][x + 1].Equals('M') && grid[y - 2][x + 2].Equals('A') && grid[y - 3][x + 3].Equals('S')) {
                    return true;
                }
                return false;
            }  
        };


        matches = grid.SelectMany((row, y) => row.Select((cell, x) => new { x, y, cell }))
            .Where(pos => pos.cell.Equals('X'))
            .Sum(pos => checkerFunctions.Count(f => f(pos.x, pos.y)));

        return Task.FromResult(matches);
    }

    public override Task<long> SolvePartTwo(IEnumerable<string> input)
    {
        // Read in input as 2D grid of characters
        var grid = input.Select(x => x.ToCharArray()).ToArray();

        var width = grid[0].Length;
        var height = grid.Length;

        // We are looking for 'MAS' X's, for example:
        /*
        M.S
        .A.
        M.S
        */
        // The diagonals can be forwards or backwards

        var checkerFunction = (int x, int y) => {
            if(x==0 || x==width-1 || y==0 || y==height-1) {
                return false;
            }

            // Check diagonal up-left to down-right
            if(!((grid[y-1][x-1].Equals('M') && grid[y+1][x+1].Equals('S')) || (grid[y-1][x-1].Equals('S') && grid[y+1][x+1].Equals('M')))) {
                return false;
            }

            // Check diagonal up-right to down-left
            if(!((grid[y-1][x+1].Equals('M') && grid[y+1][x-1].Equals('S')) || (grid[y-1][x+1].Equals('S') && grid[y+1][x-1].Equals('M')))) {
                return false;
            }

            return true;
        };

        long matches = grid.SelectMany((row, y) => row.Select((cell, x) => new { x, y, cell }))
            .Where(pos => pos.cell.Equals('A'))
            .Count(pos => checkerFunction(pos.x, pos.y));

        return Task.FromResult(matches);
    }
}
