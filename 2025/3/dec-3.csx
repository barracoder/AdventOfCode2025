using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;

IEnumerable<string> ReadInput()
{
    foreach (var line in File.ReadLines("Dec3/input.txt"))
    {
        yield return line;
    }
}

string GetLargestSubsequence(string s, int take)
{
    if (take == 0) return "";
    if (s.Length < take) return ""; 

    char maxDigit = '0';
    int maxIndex = -1;
    int maxPossibleStart = s.Length - take;
    for (int i = 0; i <= maxPossibleStart; i++)
    {
        if (s[i] > maxDigit)
        {
            maxDigit = s[i];
            maxIndex = i;
        }
    }

    return maxDigit + GetLargestSubsequence(s.Substring(maxIndex + 1), --take);
}
IEnumerable<(string Row, string LargestSubsequence)> ExamineRows(IEnumerable<string> rows, int take)
{
    foreach (var row in rows)
    {
        yield return (row, GetLargestSubsequence(row, take));
    }
}

var stopwatch = Stopwatch.StartNew();
var rows = ReadInput();
var analyzedRows = ExamineRows(rows, 12);
long sumOfLargestNumbers = 0; 

foreach (var (row, largestSubsequence) in analyzedRows)
{
    Console.WriteLine($"Row: {row}, Largest Subsequence: {largestSubsequence}");
    if (long.TryParse(largestSubsequence, out long num))
    {
        sumOfLargestNumbers += num;
    }
}

Console.WriteLine($"Sum of Largest Numbers: {sumOfLargestNumbers}");
stopwatch.Stop();
Console.WriteLine($"Elapsed time: {stopwatch.ElapsedMilliseconds} ms");