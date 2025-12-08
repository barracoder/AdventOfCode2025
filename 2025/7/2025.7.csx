#load "../../lib/AoC.File.csx"
#load "../../lib/AoC.Benchmark.csx"
#load "../../lib/AoC.Log.csx"
#load "../../lib/AoC.Array.csx"
#load "../../lib/AoC.Math.csx"

using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;

using (new Timer("Stage 1"))
{
    var inputLines = ReadInputLines("2025/7/input.txt").ToList();
    // var manifold = StringsTo2DArray(inputLines, alignLeft: true);
    // Print2DArray(manifold);

    var startIndex = inputLines.First().IndexOf('S');
    var arrayWidth = inputLines.First().Length;

    var beams = inputLines.First().ToArray();
    int splits = 0;
    for (int row = 1; row < inputLines.Count; row++)
    {
        for (int col = 0; col < arrayWidth; col++)
        {
            if (inputLines[row][col] == '^' && beams[col] != '.')
            {
                beams[col] = '.';
                beams[col - 1] = '|';
                beams[col + 1] = '|';
                splits++;
                LogToFile($"Split at row {row}, col {col}");
            }
        }
    }
    Console.WriteLine($"Total splits: {splits}");
}


using (new Timer("Stage 2"))
{
    var gridLines = ReadInputLines("2025/7/input.txt").ToList();

    var startColumn = gridLines.First().IndexOf('S');
    var gridWidth = gridLines.First().Length;
    int gridHeight = gridLines.Count;

    long[][] pathsToBottom = new long[gridHeight][];
    for (int rowIndex = 0; rowIndex < gridHeight; rowIndex++)
    {
        pathsToBottom[rowIndex] = new long[gridWidth];
    }

    for (int col = 0; col < gridWidth; col++)
    {
        char currentCell = gridLines[gridHeight - 1][col];
        pathsToBottom[gridHeight - 1][col] = (currentCell == '.' || currentCell == '^' || currentCell == 'S') ? 1 : 0;
    }

    for (int row = gridHeight - 2; row >= 0; row--)
    {
        for (int col = 0; col < gridWidth; col++)
        {
            char currentCell = gridLines[row][col];
            if (currentCell == '.' || currentCell == 'S')
            {
                pathsToBottom[row][col] = pathsToBottom[row + 1][col];
            }
            else if (currentCell == '^')
            {
                long leftPaths = (col - 1 >= 0) ? pathsToBottom[row + 1][col - 1] : 0;
                long rightPaths = (col + 1 < gridWidth) ? pathsToBottom[row + 1][col + 1] : 0;
                pathsToBottom[row][col] = leftPaths + rightPaths;
            }
            else
            {
                pathsToBottom[row][col] = 0;
            }
        }
    }

    long totalPaths = pathsToBottom[0][startColumn];
    Console.WriteLine($"Total timelines: {totalPaths}");
}