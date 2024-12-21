using System;
using System.Diagnostics;

namespace AOC2024.Cli.Solutions;

public class Day05 : DayBase
{
    public override string Name => "Print Queue";


    public override Task<long> SolvePartOne(IEnumerable<string> input)
    {
        var content = input.ToArray();

        // There should be an empty line. Everything before that are the rules, after that the jobs (multiple page numbers as a comma-separated list).
        var emptyLineIndex = Array.IndexOf(content, string.Empty);
        var rulesRaw = content.Take(emptyLineIndex).ToArray();
        var jobs = content.Skip(emptyLineIndex + 1).ToArray();

        // Parse each rule. Format: <number>|<number>. Split by '|' and parse each number.
        var rules = rulesRaw.Select(x => x.Split('|').Select(int.Parse).ToArray()).ToArray();

        // The cache contains already processed page rules. The key is the page number and the value are all pages that must be printed before that job.
        var sucessorCache = new Dictionary<int, HashSet<int>>();

        long evaluateJob(string job)
        {
            var pages = job.Split(',').Select(int.Parse).ToArray();
            var alreadyPrinted = new HashSet<int>();

            foreach (var page in pages)
            {
                if (!sucessorCache.TryGetValue(page, out var mustBeBefore))
                {
                    mustBeBefore = rules.Where(x => x[0] == page).Select(x => x[1]).ToHashSet();
                    sucessorCache[page] = mustBeBefore;
                }

                // If one of the succesors was already printed, this job is invalid!.
                if (mustBeBefore.Any(alreadyPrinted.Contains))
                {
                    return 0;
                }
                alreadyPrinted.Add(page);
            }

            if (pages.Length % 2 == 0)
                Console.WriteLine("WARNING: Even number of pages in a job!");
            return pages[pages.Length / 2];
        }

        return Task.FromResult(jobs.Sum(evaluateJob));
    }

    public override Task<long> SolvePartTwo(IEnumerable<string> input)
    {
        var content = input.ToArray();

        // There should be an empty line. Everything before that are the rules, after that the jobs (multiple page numbers as a comma-separated list).
        var emptyLineIndex = Array.IndexOf(content, string.Empty);
        var rulesRaw = content.Take(emptyLineIndex).ToArray();
        var jobs = content.Skip(emptyLineIndex + 1).ToArray();

        // Parse each rule. Format: <number>|<number>. Split by '|' and parse each number.
        var rules = rulesRaw.Select(x => x.Split('|').Select(int.Parse).ToArray()).ToArray();

        // The cache contains already processed page rules. The key is the page number and the value are all pages that must be printed before that job.
        var sucessorCache = new Dictionary<int, HashSet<int>>();        
        long evaluateJob(string job) {
            var pages = job.Split(',').Select(int.Parse).ToArray();
            var changedOnce = false;

            while (true)
            {
                var alreadyPrinted = new HashSet<int>();

                var currentIndex = -1;
                var requiredIndex = -1;
                foreach (var page in pages)
                {
                    var requirements = rules.Where(x => x[0] == page).Select(x => x[1]);
                    var mustBeBeforePage = requirements.FirstOrDefault(alreadyPrinted.Contains);

                    if (mustBeBeforePage != 0)
                    {
                        currentIndex = Array.IndexOf(pages, page);
                        requiredIndex = Array.IndexOf(pages, mustBeBeforePage);
                        break;
                    }
                    alreadyPrinted.Add(page);
                }

                if (currentIndex == -1)
                {
                    break;
                }

                changedOnce = true;
                // Move the current page to the required position.
                var temp = pages[currentIndex];
                for (var i = currentIndex; i > requiredIndex; i--)
                {
                    pages[i] = pages[i - 1];
                }
                pages[requiredIndex] = temp;
            }

            if (!changedOnce)
                return 0;

            if (pages.Length % 2 == 0)
                Console.WriteLine("WARNING: Even number of pages in a job!");
            return pages[pages.Length / 2];
        }

        return Task.FromResult(jobs.Sum(evaluateJob));
    }
}
