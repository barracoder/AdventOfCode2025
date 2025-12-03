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

// Return a sequence of (row, largestNumbers) tuples
IEnumerable<(string Row, int[] LargestNumbers)> ExamineRows(IEnumerable<string> rows, int take)
{
    foreach (var row in rows)
    {
        var largestNumber = row
            .Take(row.Length - 1)                     // take all characters -1
            .Where(char.IsDigit)                  // pick only digits
            .Select(c => c - '0')                 // char -> int
            .Max();

        var indexOfLargestNumber = row.IndexOf((char)(largestNumber + '0'));

        var secondLargest = row
            .Substring(indexOfLargestNumber + 1) 
            .Where(char.IsDigit)
            .Select(c => c - '0')
            .Max();

        yield return (row, new int[] { largestNumber, secondLargest });
    }
}

var stopwatch = Stopwatch.StartNew();
var rows = ReadInput();
var analyzedRows = ExamineRows(rows, 2);
int sumOfLargestNumbers = 0;

// Deconstruct the tuple we now return
foreach (var (row, largestNumbers) in analyzedRows)
{
    Console.WriteLine($"Row: {row}, Largest Numbers: {string.Join(", ", largestNumbers)}");
    sumOfLargestNumbers += ((largestNumbers[0] * 10) + largestNumbers[1]);
}
Console.WriteLine($"Sum of largest two-digit numbers from each row: {sumOfLargestNumbers}");

stopwatch.Stop();
Console.WriteLine($"Elapsed time: {stopwatch.ElapsedMilliseconds} ms");