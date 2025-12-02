using System;
using System.IO;

var path = "Dec2/input.txt";

IEnumerable<string> GetRanges()
{
    string line = File.ReadAllText(path).TrimEnd('\r', '\n');

    string[] values = line.Split(',', StringSplitOptions.RemoveEmptyEntries);

    foreach(var value in values)
    {
        yield return value;
    }
}

IEnumerable<long> GetDuplicatedValuesInRange(string range)
{
    var bounds = range.Split('-', StringSplitOptions.RemoveEmptyEntries);
    long start = long.Parse(bounds[0]);
    long end = long.Parse(bounds[1]);

    for (long i = start; i <= end; i++)
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

    if(!isEvenLength)
        return false;

    int halfLength = length / 2;

    for(int i = 0; i < halfLength; i++)
    {
        if(s[i] != s[i + halfLength])
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

var stopwatch = new Stopwatch();
stopwatch.Start();
var duplicatedValuesOverall = new List<long>();
foreach(var range in GetRanges())
{
    var duplicatedValues = GetDuplicatedValuesInRange(range);
    Console.WriteLine($"Duplicated values in range {range}: {string.Join(", ", duplicatedValues)}");
    duplicatedValuesOverall.AddRange(duplicatedValues);
}

Console.WriteLine($"Sum Duplicated Values: {duplicatedValuesOverall.Sum()}");

stopwatch.Stop();
Console.WriteLine($"Execution Time: {stopwatch.ElapsedMilliseconds} ms");