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

IEnumerable<(string row, int newStart, int zeroClicks)> ExamineRows(int start, IEnumerable<string> rows)
{
    foreach (var row in rows)
    {
        var dir = row[0];
        var modifier = int.Parse(row.Substring(1));

        int zeroClicks = CountZeroClicks(start, dir, modifier);

        if (dir == 'L')
            start = (start - modifier) % 100;
        else
            start = (start + modifier) % 100;
        start = (start + 100) % 100; // Ensure positive


        yield return (row, start, zeroClicks);
    }
}

int CountZeroClicks(int start, char direction, int numClicks)
{
    if (numClicks <= 0) return 0;

    int stepsUntilFirstZero;
    if (direction == 'R')
    {
        stepsUntilFirstZero = (100 - start) % 100;       
        if (stepsUntilFirstZero == 0) stepsUntilFirstZero = 100;         
    }
    else
    {
        stepsUntilFirstZero = start == 0 ? 100 : start;  
    }

    if (stepsUntilFirstZero > numClicks) return 0;
    return 1 + (numClicks - stepsUntilFirstZero) / 100;
}

// Entry point
int x = 10; // testing 
bool fullAnalysis = true; // Set to true for full analysis of all values
int initialStart = 50;
var input = fullAnalysis ? ReadInput() : ReadInput().Take(x);
int countZero = 0;
int zeroClicks = 0;

foreach (var (row, newStart, zeroClicksinRow) in ExamineRows(initialStart, input))
{
    Console.WriteLine($"{row} -> {newStart} -> Zero Clicks: {zeroClicksinRow}");
    if (newStart == 0) countZero++;
    zeroClicks += zeroClicksinRow;
}
Console.WriteLine($"Number of times newStart was 0: {countZero}");
Console.WriteLine($"Number of times zero was clicked: {zeroClicks}");