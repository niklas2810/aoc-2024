namespace AOC2024.Cli.Solutions;

public class Day07 : DayBase
{
    public override string Name => "Bridge Repair";

    private static bool CheckOptions(int[] numbers, long expected, bool useConcat  = false)
    {
        // 4+2 => 6
        // 4*2 => 8 
        // 4|2 => 42
        char[] operators = useConcat ? ['+', '*', '|'] : ['+', '*'];
        var operatorCount = numbers.Length - 1;
        var max = (int)Math.Pow(operators.Length, operatorCount);
        // Check all permutations of operators
        return Enumerable.Range(0, max).Any(i =>
        {
            var temp = i;
            // Evaluate the expression
            long result = numbers[0];
            for (var j = 0; j < operatorCount; j++)
            {
                switch (operators[temp % operators.Length])
                {
                    case '+':
                        result += numbers[j + 1];
                        break;
                    case '*':
                        result *= numbers[j + 1];
                        break;
                    case '|':
                        result = long.Parse($"{result}{numbers[j + 1]}");
                        break;
                    default:
                        throw new InvalidOperationException("Invalid operator");
                }
                temp /= operators.Length;
            }
            return result == expected;
        });
    }

    public override Task<long> SolvePartOne(IEnumerable<string> input)
    {
        // Evaluate Each line individually
        // Separated by ":" => Left side is expected solution int, right side is space-separated list of ints
        var sum = input.AsParallel().Select(line => {

            var parts = line.Split(":");
            var expected = long.Parse(parts[0]);
            var numbers = parts[1].Trim().Split(" ").Select(int.Parse).ToArray();

            // Combine the numbers using + or * and check if it matches the expected value
            // We need to check all permutations for the operators
            // We cannot assume the number of operators is fixed
            if (CheckOptions(numbers, expected))
                return expected;
            return 0L;
        }).Sum();
        return Task.FromResult(sum);
    }

    public override Task<long> SolvePartTwo(IEnumerable<string> input)
    {
        // Evaluate Each line individually
        // Separated by ":" => Left side is expected solution int, right side is space-separated list of ints
        var sum = input.AsParallel().Select(line =>
        {
            var parts = line.Split(":");
            var expected = long.Parse(parts[0]);
            var numbers = parts[1].Trim().Split(" ").Select(int.Parse).ToArray();

            if (CheckOptions(numbers, expected, true))
                return expected;
            return 0L;
        }).Sum();
        return Task.FromResult(sum);
    }
}
