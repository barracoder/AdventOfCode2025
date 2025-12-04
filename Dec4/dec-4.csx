using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;

IEnumerable<string> ReadInput()
{
    foreach (var line in File.ReadLines("Dec4/input.txt"))
    {
        yield return line;  
    }
}

string[,] BuildArray(IEnumerable<string> rows)
{
    var rowList = rows.ToList();
    int height = rowList.Count;
    int width = rowList[0].Length;
    var array = new string[height, width];

    for(int r = 0; r < height; r++)
    {
        for(int c = 0; c < width; c++)
        {
            array[r, c] = rowList[r][c].ToString();
        }
    }

    return array;
}

bool hasFewerThanNRollsAdjacent(string[,] array, int row, int col, int n)
{
    var bounds = new 
    {
        height = array.GetLength(0),
        width = array.GetLength(1)
    };

    var adjacentPosition = (int rowOffset, int colOffset) =>
    {
        if(row + rowOffset < 0 || row + rowOffset >= bounds.height ||
           col + colOffset < 0 || col + colOffset >= bounds.width)
        {
            return 0;
        }
        return array[row + rowOffset, col + colOffset] == "@" ? 1 : 0;
    };

    var count = adjacentPosition(-1, -1) +
           adjacentPosition(-1, 0) +
           adjacentPosition(-1, 1) +
           adjacentPosition(0, -1) +
           adjacentPosition(0, 1) +
           adjacentPosition(1, -1) +
           adjacentPosition(1, 0) +
           adjacentPosition(1, 1);

    // Console.WriteLine($"Position ({row}, {col}) has {count} adjacent rolls.");
    return count < n;
}

void PrintArray(string[,] array)
{
    for(int r = 0; r < array.GetLength(0); r++)
    {
        for(int c = 0; c < array.GetLength(1); c++)
        {
            Console.Write(array[r, c]);
        }
        Console.WriteLine();
    }
    Console.WriteLine();
}

var stopwatch = Stopwatch.StartNew();

var rows = ReadInput();
var array = BuildArray(rows);
int removed = 0;
int totalRemoved = 0;

do {
    removed = 0;
    PrintArray(array);
    for(int row = 0; row < array.GetLength(0); row++)
    {
        for(int col = 0; col < array.GetLength(1); col++)
        {
            if(array[row, col] == "@")
            {
                bool canRemove = hasFewerThanNRollsAdjacent(array, row, col, 4);
                // Console.WriteLine($"{row}, {col} Has adjacent rolls: {!canRemove}");

                if(canRemove)
                {
                    array[row, col] = ".";
                    removed++;
                    totalRemoved++;
                }
            }
        }
    } 
    Console.WriteLine($"Removed: {removed}");
} while(removed > 0);

stopwatch.Stop();
Console.WriteLine($"Count: {totalRemoved}");
Console.WriteLine($"Elapsed time: {stopwatch.ElapsedMilliseconds} ms");