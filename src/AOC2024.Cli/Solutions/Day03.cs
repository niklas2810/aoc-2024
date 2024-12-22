using System;
using System.Text.RegularExpressions;

namespace AOC2024.Cli.Solutions;

public partial class Day03 : DayBase
{
    public override string Name => "Mull It Over";

    [GeneratedRegex("mul\\((\\d{1,3}),(\\d{1,3})\\)")]
    private static partial Regex MulRegex();

    public override async Task<long> SolvePartOne(IEnumerable<string> input)
    {
        var content = string.Join(string.Empty, input);

        var matches = MulRegex().Matches(content);
        // For each match, multiply the two numbers and sum the results
        await Task.CompletedTask;
        return matches.Sum(m => int.Parse(m.Groups[1].Value) * int.Parse(m.Groups[2].Value));
    }

    public override Task<long> SolvePartTwo(IEnumerable<string> input)
    {
        var content = string.Join(string.Empty, input);

        /*
        There are two new instructions you'll need to handle:

        The do() instruction enables future mul instructions.
        The don't() instruction disables future mul instructions.
        */
        // Ignore all mul instructions after a don't() instruction, until a do() instruction is found
        // First, build a reduced string that only contains the mul instructions that are enabled
        // Then, use the strategy from part 1 to sum the results
        while(content.Contains("don't()"))
        {
            var dontIndex = content.IndexOf("don't()");
            var doIndex = content.IndexOf("do()", dontIndex+7);
            if(doIndex < 0)
            {
                content = content.Substring(0, dontIndex);
                break;
            }
            else
            {
                content = content.Substring(0, dontIndex) + content.Substring(doIndex + 4);
            }
            
        }
        return SolvePartOne([content]);
    }
}
