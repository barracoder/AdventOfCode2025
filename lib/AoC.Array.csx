using System;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;


record class TreeNode<T>(T Value, TreeNode<T> Left, TreeNode<T> Right);
    

static string[,] ParseDelimitedTo2DArray(IEnumerable<string> lines, char delimiter, bool ignoreBlankColumns = true)
{
    var lineList = lines.ToList();
    int rowCount = lineList.Count;
    int colCount = lineList[0].Split(delimiter).Length;
    if (ignoreBlankColumns)
    {
        var firstRowCols = lineList[0].Split(delimiter);
        colCount = firstRowCols.Count(c => !string.IsNullOrWhiteSpace(c));
    }

    var result = new string[rowCount, colCount];

    for (int i = 0; i < rowCount; i++)
    {
        var cols = lineList[i].Split(delimiter);
        if (ignoreBlankColumns)
        {
            cols = cols.Where(c => !string.IsNullOrWhiteSpace(c)).ToArray();
        }
        
        for (int j = 0; j < colCount; j++)
        {
            result[i, j] = cols[j];
        }
    }

    return result;
}

static string[,] StringsTo2DArray(IEnumerable<string> lines, bool alignLeft = false)
{
    var lineList = lines.ToList();
    int rowCount = lineList.Count;
    int colCount = lines.Select(line => line.Length).Max();

    var result = new string[rowCount, colCount];

    for (int row = 0; row < rowCount; row++)
    {
        for (int col = 0; col < lineList[row].Length; col++)
        {
            if (alignLeft)
            {
                result[row, col] = lineList[row][col].ToString();
            }
            else
            {
                result[row, colCount - col - 1] = lineList[row].Reverse().ElementAt(col).ToString();
            }
        }
    }

    return result;
}

static void Print2DArray<T>(T[,] array)
{
    int rowCount = array.GetLength(0);
    int colCount = array.GetLength(1);

    for (int i = 0; i < rowCount; i++)
    {
        for (int j = 0; j < colCount; j++)
        {
            var value = array[i, j]?.ToString() ?? "";
            Console.Write(string.IsNullOrWhiteSpace(value) ? " " : value);
        }
        Console.WriteLine();
    }
}

static T[] GetColumn<T>(T[,] array, int columnIndex)
{
    int rowCount = array.GetLength(0);
    T[] result = new T[rowCount];
    for (int i = 0; i < rowCount; i++)
    {
        result[i] = array[i, columnIndex];
    }
    return result;
}
int CountRootToLeafPaths<T>(TreeNode<T> root)
{
    if (root == null) return 0;
    if (root.Left == null && root.Right == null) return 1;
    return CountRootToLeafPaths(root.Left) + CountRootToLeafPaths(root.Right);
}

string PrettyPrintTreeNode<T>(TreeNode<T> root)
{
    if (root == null) return "null";
    
    var sb = new System.Text.StringBuilder();
    TreePrintHelper(root, "", true, sb);
    return sb.ToString();
}

void TreePrintHelper<T>(TreeNode<T> node, string prefix, bool isLeft, System.Text.StringBuilder sb)
{
    if (node == null)
    {
        sb.AppendLine($"{prefix}{(isLeft ? "├── " : "└── ")}null");
        return;
    }

    sb.AppendLine($"{prefix}{(isLeft ? "├── " : "└── ")}{node.Value}");

    var children = new[] { node.Left, node.Right };
    for (int i = 0; i < children.Length; i++)
    {
        bool isLast = i == children.Length - 1;
        TreePrintHelper(children[i], prefix + (isLeft ? "│   " : "    "), isLast, sb);
    }
}