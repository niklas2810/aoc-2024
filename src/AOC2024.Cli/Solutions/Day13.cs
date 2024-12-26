namespace AOC2024.Cli.Solutions;

public class Day13 : DayBase
{
    public override string Name => "Claw Contraption";

    private static long SolveMachine((int x, int y) a, (int x, int y) b, (long x, long y) prize)
    {
        // I wrote a fancy A* algorithm to solve this, but it was too slow (and not needed).
        // Turns out this is a simple linear equation problem.

        // We start at 0,0 and need to reach point x. Which means that this is basically...
        // I.     A*a_x + B*b_x = prize_x
        // II.    A*a_y + B*b_y = prize_y
        // The problem statement told us to find the *minimum* solution, but there is only one solution in all cases.
        // We can use Cramer's rule to solve for A and B: https://de.wikipedia.org/wiki/Cramersche_Regel
        // I.     A = (prize_x * b_y - prize_y * b_x) / (a_x * b_y - a_y * b_x)  
        // II.    B = (a_x * prize_y - a_y * prize_x) / (a_x * b_y - a_y * b_x)
        var det = a.x * b.y - a.y * b.x;
        var A = (prize.x * b.y - prize.y * b.x) / det;
        var B = (a.x * prize.y - a.y * prize.x) / det;

        // We still need to check if we actually hit the prize.
        var calculatedPrize = (a.x * A + b.x * B, a.y * A + b.y * B);
        return calculatedPrize == prize ? A * 3 + B : -1;
    }

    public override Task<long> SolvePartOne(IEnumerable<string> input)
    {
        // Input constsists of three-line blocks: Button A/B and Prize. 
        var machines = new List<((int x, int y) a, (int x, int y) b, (int x, int y) prize)>();
        var lines = input.ToList();
        for (var i = 0; i < lines.Count; i += 4)
        {
            var a = lines[i][10..].Split(", ");
            var aX = int.Parse(a[0][2..]);
            var aY = int.Parse(a[1][2..]);

            var b = lines[i + 1][10..].Split(", ");
            var bX = int.Parse(b[0][2..]);
            var bY = int.Parse(b[1][2..]);

            var prize = lines[i + 2][7..].Split(", ");
            var prizeX = int.Parse(prize[0][2..]);
            var prizeY = int.Parse(prize[1][2..]);

            machines.Add(((aX, aY), (bX, bY), (prizeX, prizeY)));
        }

        var sum = machines.Select(m => SolveMachine(m.a, m.b, m.prize)).Where(score => score > 0).Sum();
        return Task.FromResult(sum);
    }

    public override Task<long> SolvePartTwo(IEnumerable<string> input)
    {
        // Input constsists of three-line blocks: Button A/B and Prize. 
        var machines = new List<((int x, int y) a, (int x, int y) b, (long x, long y) prize)>();


        var lines = input.ToList();
        for (var i = 0; i < lines.Count; i += 4)
        {
            var a = lines[i][10..].Split(", ");
            var aX = int.Parse(a[0][2..]);
            var aY = int.Parse(a[1][2..]);

            var b = lines[i + 1][10..].Split(", ");
            var bX = int.Parse(b[0][2..]);
            var bY = int.Parse(b[1][2..]);

            var prize = lines[i + 2][7..].Split(", ");
            var prizeX = long.Parse(prize[0][2..]) + 10000000000000L;
            var prizeY = long.Parse(prize[1][2..]) + 10000000000000L;

            machines.Add(((aX, aY), (bX, bY), (prizeX, prizeY)));
        }

        var sum = machines.Select(m => SolveMachine(m.a, m.b, m.prize)).Where(score => score > 0).Sum();
        return Task.FromResult(sum);
    }
}
