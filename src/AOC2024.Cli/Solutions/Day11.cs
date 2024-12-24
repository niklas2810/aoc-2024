namespace AOC2024.Cli.Solutions;

public class Day11 : DayBase
{
    public override string Name => "Plutonian Pebbles";

    private static long fullCounter = 0;

    private static Dictionary<long, long>[]? valueCache;

    private static long CountPebbles(IEnumerable<long> pebbles, int blinks) {
        valueCache = new Dictionary<long, long>[blinks + 1];
        for(var i = 0; i < valueCache.Length; ++i) {
            valueCache[i] = [];
        }

        return pebbles.Select(x => GetPeppleCount(x, blinks)).Sum();
    }

    private static long GetPeppleCount(long num, int stepsLeft) {
        if(stepsLeft < 1) {
            ++fullCounter;
            if(fullCounter % 1000000 == 0)
                Console.WriteLine(fullCounter);
            return 1;
        }


        if(valueCache[^stepsLeft].TryGetValue(num, out var cachedValue))
            return cachedValue;
        
        var result = 0L;


        var numString = num.ToString();
        if(num == 0)
            result =  GetPeppleCount(1, stepsLeft - 1);
        else if(numString.Length % 2 == 0) {
            var half = numString.Length / 2;
            result = GetPeppleCount(long.Parse(numString[..half]), stepsLeft - 1) + GetPeppleCount(long.Parse(numString[half..]), stepsLeft - 1);
        }
        else {
            result = GetPeppleCount(num * 2024, stepsLeft - 1);
        }
        
        valueCache[^stepsLeft].Add(num, result);
        return result;
    }

    public override Task<long> SolvePartOne(IEnumerable<string> input)
    {
        // Input is a single line of longs.
        var inputParsed = input.Single().Split(' ').Select(long.Parse);
        var count = CountPebbles(inputParsed, 25);
        return Task.FromResult(count);
    }

    public override Task<long> SolvePartTwo(IEnumerable<string> input)
    {
        // Input is a single line of longs.
        var inputParsed = input.Single().Split(' ').Select(long.Parse);
        var count = CountPebbles(inputParsed, 75);
        return Task.FromResult(count);
    }
}
