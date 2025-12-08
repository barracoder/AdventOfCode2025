using System;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

var path = "Dec2/input.txt";

readonly record struct Range(long Start, long End);

IEnumerable<Range> GetRanges()
{
    string line = File.ReadAllText(path).TrimEnd('\r', '\n');

    foreach (var value in line.Split(',', StringSplitOptions.RemoveEmptyEntries))
    {
        var bounds = value.Split('-', StringSplitOptions.RemoveEmptyEntries);

        long start = long.Parse(bounds[0]);
        long end   = long.Parse(bounds[1]);

        yield return new Range(start, end);
    }
}

IEnumerable<long> GetDuplicatedValuesInRange(Range range)
{
    for (long i = range.Start; i <= range.End; i++)
    {
        var candidate = i.ToString();
        var isEvenLength = candidate.Length % 2 == 0;
        if (!isEvenLength)
            continue;

        if (isDuplicated(candidate))
        {
            yield return i;
        }
    }
}

// Determines if a given string is two halves of the same value
bool isDuplicated(ReadOnlySpan<char> s)
{
    int length = s.Length;
    var isEvenLength = length % 2 == 0;

    if (!isEvenLength)
        return false;

    int halfLength = length / 2;

    for (int i = 0; i < halfLength; i++)
    {
        if (s[i] != s[i + halfLength])
            return false;
    }

    return true;
}

bool IsPalindrome(ReadOnlySpan<char> s)
{
    int i = 0;
    int j = s.Length - 1;

    while (i < j)
    {
        if (s[i] != s[j])
            return false;

        i++;
        j--;
    }

    return true;
}

bool IsComposedOfRepeatedSubstring(ReadOnlySpan<char> s, out int repeatLength, out int repeatCount)
{
    int n = s.Length;
    repeatLength = 0;
    repeatCount = 0;

    if (n == 0)
        return false;

    // Try all possible base lengths that divide n
    for (int len = 1; len <= n / 2; len++)
    {
        if (n % len != 0)
            continue;

        bool isRepeated = true;
        int count = n / len;

        // Compare each "block" to the first block
        for (int block = 1; block < count && isRepeated; block++)
        {
            for (int i = 0; i < len; i++)
            {
                if (s[i] != s[block * len + i])
                {
                    isRepeated = false;
                    break;
                }
            }
        }

        if (isRepeated)
        {
            repeatLength = len;
            repeatCount = count;
            return true;
        }
    }

    return false;
}

IEnumerable<long> GetComposedOfRepeatedSubstringValuesInRange(Range range)
{
    for (long i = range.Start; i <= range.End; i++)
    {
        var candidate = i.ToString();

        if (IsComposedOfRepeatedSubstring(candidate.AsSpan(), out int repeatLength, out int repeatCount))
        {
            yield return i;
        }
    }
}

var stopwatch = new Stopwatch();
stopwatch.Start();
var duplicatedValuesOverall = new List<long>();

foreach (var range in GetRanges())
{
    var duplicatedValues = GetDuplicatedValuesInRange(range);
    Console.WriteLine($"Duplicated values in range {range.Start}-{range.End}: {string.Join(", ", duplicatedValues)}");
    duplicatedValuesOverall.AddRange(duplicatedValues);
}

Console.WriteLine($"Sum Duplicated Values: {duplicatedValuesOverall.Sum()}");

var composedOfRepeatedSubstringValuesOverall = new List<long>();
foreach (var range in GetRanges())
{
    var composedOfRepeatedSubstringValues = GetComposedOfRepeatedSubstringValuesInRange(range);
    Console.WriteLine($"Composed of repeated substring values in range {range.Start}-{range.End}: {string.Join(", ", composedOfRepeatedSubstringValues)}");
    composedOfRepeatedSubstringValuesOverall.AddRange(composedOfRepeatedSubstringValues);
}
Console.WriteLine($"Sum Composed of Repeated Substring Values: {composedOfRepeatedSubstringValuesOverall.Sum()}");

stopwatch.Stop();
Console.WriteLine($"Execution Time: {stopwatch.ElapsedMilliseconds} ms");