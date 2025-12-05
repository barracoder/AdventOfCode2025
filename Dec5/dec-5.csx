using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

readonly record struct Range(long Start, long End);

IEnumerable<string> ReadInput(string filePath)
{
    return File.ReadLines(filePath);
}

(IEnumerable<string>, IEnumerable<string>) SplitInputOnEmptyLine(IEnumerable<string> input)
{
    var firstPart = new List<string>();
    var secondPart = new List<string>();
    var isFirstPart = true;

    foreach(var line in input)
    {
        if(string.IsNullOrWhiteSpace(line))
        {
            isFirstPart = false;
            continue;
        }

        if(isFirstPart)
        {
            firstPart.Add(line);
        }
        else
        {
            secondPart.Add(line);
        }
    }

    Console.WriteLine($"First part count: {firstPart.Count}");
    Console.WriteLine($"Second part count: {secondPart.Count}");
    return (firstPart, secondPart);
}

IEnumerable<Range> BuildFreshRange(IEnumerable<string> rows)
{
    foreach(var row in rows)
    {
        var parts = row.Split('-');
        long start = long.Parse(parts[0]);
        long end = long.Parse(parts[1]);
        if(end < start)
        {
            start = long.Parse(parts[1]);
            end = long.Parse(parts[0]);
        }

        yield return new Range(start, end);
    }
}

void PrintEnumerable<T>(IEnumerable<T> items)
{
    foreach(var item in items)
    {
        Console.WriteLine(item);
    }
}

var stopwatch = Stopwatch.StartNew();

var input = ReadInput("Dec5/input.txt");
var (freshIds, availableIds) = SplitInputOnEmptyLine(input);
var freshRange = BuildFreshRange(freshIds);

// PrintEnumerable<string>(freshIds);
// PrintEnumerable<long>(freshRange);

var checkedFreshIds = availableIds
    .Select(id => long.Parse(id))
    .Where(id => freshRange.Any(range => id >= range.Start && id <= range.End));

var spoiltIds = availableIds.Select(id => long.Parse(id)).Except(checkedFreshIds);

Console.WriteLine($"Number of spoilt IDs: {spoiltIds.Count()}");
Console.WriteLine($"Number of fresh IDs: {checkedFreshIds.Count()}");


stopwatch.Stop();
Console.WriteLine($"Execution Time: {stopwatch.ElapsedMilliseconds} ms");