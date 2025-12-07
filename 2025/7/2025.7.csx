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
    var inputLines = ReadInputLines("2025/7/input.txt").ToList();

    var startIndex = inputLines.First().IndexOf('S');
    var arrayWidth = inputLines.First().Length;

    var beams = inputLines.First().ToArray();
    int splits = 0;
    int timelines = 1;
    TreeNode<int> root = new TreeNode<int>(startIndex, null, null);
    TreeNode<int> currentNode = root;
    // build a tree
    TreeNode<int> BuildTree(int row, int beamIndex)
    {
        if (row >= inputLines.Count)
        {
            return null;
        }

        if (inputLines[row][beamIndex] == '^')
        {
            splits++;
            LogToFile($"Split at row {row}, col {beamIndex}");
            var leftChild = BuildTree(row + 1, beamIndex - 1);
            var rightChild = BuildTree(row + 1, beamIndex + 1);
            timelines += 1;
            return new TreeNode<int>(beamIndex, leftChild, rightChild);
        }
        else if (inputLines[row][beamIndex] == '.')
        {
            return BuildTree(row + 1, beamIndex);
        }
        else
        {
            return null;
        }
    }
    root = new TreeNode<int>(startIndex, BuildTree(1, startIndex), null); 
    // int timelines = CountRootToLeafPaths(root);
    // Console.WriteLine(PrettyPrintTreeNode(root));
    Console.WriteLine($"Total timelines: {timelines}");
}