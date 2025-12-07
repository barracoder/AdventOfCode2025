#load "../../lib/AoC.File.csx"
#load "../../lib/AoC.Benchmark.csx"
#load "../../lib/AoC.Log.csx"
#load "../../lib/AoC.Array.csx"

using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;

using(new Timer("Stage 1"))
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
        for( int col = 0;col < arrayWidth; col++)
        {
            if(inputLines[row][col] == '^' && beams[col] != '.')
            {
                beams[col] = '.';
                beams[col-1] = '|';
                beams[col+1] = '|';
                splits++;
                LogToFile($"Split at row {row}, col {col}");
            }
        }
    }
    Console.WriteLine($"Total splits: {splits}");
}

