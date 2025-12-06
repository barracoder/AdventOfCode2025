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
    var inputLines = ReadInputLines("2025/6/input.txt");
    var data = ParseDelimitedTo2DArray(inputLines, ' ', ignoreBlankColumns: true);

    int rowCount = data.GetLength(0);
    int colCount = data.GetLength(1);

    Console.WriteLine($"Data has {rowCount} rows and {colCount} columns.");

    long grandTotal = 0;
    for(int col = 0; col < colCount; col++)
    {
        string operation = data[data.GetLength(0) - 1, col];
        long sum = 0;
        if(operation == "*")
        {
            sum = 1;
        }
        for(int row =0; row < rowCount -1; row++)
        {
            switch(operation)
            {
                case "*":
                    sum *= long.Parse(data[row, col]);
                    break;
                case "+":
                    sum += long.Parse(data[row, col]);
                    break;
                default:
                    throw new InvalidOperationException($"Unknown operation: {operation}");
            }
        }
        grandTotal += sum;

        // Console.WriteLine($"Column {col} sum: {sum}");
    }
    Console.WriteLine($"Grand total: {grandTotal}");
}

readonly record struct Problem(string Operation, string[] Arguments);
using(new Timer("Stage 2"))
{
    var inputLines = ReadInputLines("2025/6/input.txt");    

    long grandTotal = 0;

    IEnumerable<Problem> Problems() {
        // get the indexes of all the characters in the last row
        var operationsIndices = Enumerable.Range(0, inputLines.Last().Length)
            .Where(i => !char.IsWhiteSpace(inputLines.Last()[i]))
            .ToList();
        
        var array = new string[inputLines.Count() -1, operationsIndices.Count];
        for(int i = 0; i < operationsIndices.Count; i++)
        {
            int startIndex = operationsIndices[i];
            int endIndex = (i + 1 < operationsIndices.Count) ? operationsIndices[i + 1] : inputLines.Last().Length;
            for(int row =0; row < inputLines.Count() -1; row++)
            {
                array[row, i] = inputLines.ElementAt(row).Substring(startIndex, endIndex - startIndex - 1);
            }
        }
        
        for(int i =0; i < operationsIndices.Count; i++)
        {
            var operation = inputLines.Last()[operationsIndices[i]].ToString();
            var argumentArray = GetColumn(array, i);
            yield return new Problem(operation, argumentArray);
        }
    } 

    foreach(var problem in Problems())
    {
        
        IEnumerable<string> Arguments() {
            int width = problem.Arguments[0].Length;
            for(int col = width -1; col >=0; col--)
            {
                string argument = "";
                for(int row =0; row < problem.Arguments.GetLength(0); row++)
                {
                    argument += problem.Arguments[row][col];
                }
                yield return argument;
            }
        };

        long sum = 0;
        if(problem.Operation == "*")
        {
            sum = 1;
        }
        LogToFile($"Processing problem with operation {problem.Operation}, arguments: {string.Join(", ", Arguments())}");
        foreach(var argument in Arguments())
        {
            switch(problem.Operation)
            {
                case "*":
                    sum *= long.Parse(argument);
                    break;
                case "+":
                    sum += long.Parse(argument);
                    break;
                default:
                    throw new InvalidOperationException($"Unknown operation: {problem.Operation}");
            }
        }
        string message = $"{(problem.Operation == "*" ? "Product" : "Sum")}: {sum}";
        LogToFile(message);
        // Console.WriteLine(message);
        grandTotal += sum;
    }
    Console.WriteLine($"Grand total: {grandTotal}");
}
