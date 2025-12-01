using System.IO;
using System.Collections.Generic;
using System.Linq;

IEnumerable<string> ReadInput()
{
    foreach (var line in File.ReadLines("Dec1/docs/input.txt"))
    {
        yield return line;
    }
}

IEnumerable<(string row, int newStart)> ExamineRows(int start, IEnumerable<string> rows)
{
    foreach (var row in rows)
    {
        var dir = row[0];
        var modifier = int.Parse(row.Substring(1));
        if (dir == 'L')
            start = (start - modifier) % 100;
        else
            start = (start + modifier) % 100;
        start = (start + 100) % 100; // Ensure positive
        yield return (row, start);
    }
}

// Entry point
int x = 10; // Number of values to take for testing
bool fullAnalysis = false; // Set to true for full analysis of all values
int initialStart = 50;
var input = fullAnalysis ? ReadInput() : ReadInput().Take(x);
int countZero = 0;
foreach (var (row, newStart) in ExamineRows(initialStart, input))
{
    Console.WriteLine($"{row} -> {newStart}");
    if (newStart == 0) countZero++;
}
Console.WriteLine($"Number of times newStart was 0: {countZero}");
