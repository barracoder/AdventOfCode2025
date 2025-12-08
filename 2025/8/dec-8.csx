#load "../../lib/AoC.File.csx"
#load "../../lib/AoC.Benchmark.csx"
#load "../../lib/AoC.Log.csx"
#load "../../lib/AoC.Array.csx"
#load "../../lib/AoC.Math.csx"
#load "../../lib/AoC.Vector.csx"

using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;

using (new Timer("Stage 1"))
{
    int productOfLargestDifferences = 0;
    var vectors = ReadInputLines("2025/8/input.txt")
        .Select(line =>
        {
            var parts = line.Split(',').Select(int.Parse).ToArray();
            return new Vector3(parts[0], parts[1], parts[2]);
        })
        .ToList();

    var shortestDifferenceBetweenAnyTwoVectors = vectors
        .SelectMany((v1, index1) => vectors.Skip(index1 + 1).Select(v2 =>
        {
            return new
            {
                Vectors = new Vector3[] { v1, v2 },
                Difference = (v1 - v2).LengthSquared()
            };
        }))
        .OrderBy(pair => pair.Difference);

    // Console.WriteLine($"Shortest 3D difference: {shortestDifferenceBetweenAnyTwoVectors} between any two vectors");

    var closestCount = 1000;
    var used = shortestDifferenceBetweenAnyTwoVectors.Take(closestCount).ToList();
    var circuits = new List<HashSet<Vector3>>();
    foreach( var nextClosest in used)
    {
        Console.WriteLine($"Next closest difference: {nextClosest.Difference} between {nextClosest.Vectors[0]} and {nextClosest.Vectors[1]}");
    }

    foreach (var connection in used)
    {
        var (v1, v2) = (connection.Vectors[0], connection.Vectors[1]);
        var existingCircuit = false;
        for (int i=0; i<circuits.Count; i++)
        {
            var circuit = circuits[i];
            if (circuit.Contains(v1) || circuit.Contains(v2))
            {
                circuit.Add(v1);
                circuit.Add(v2);
                existingCircuit = true;
                break;
            }
        }

        if(!existingCircuit)
        {
            circuits.Add( new HashSet<Vector3> { v1, v2 } );
        }
    }

    circuits = UnionConnectedSets(circuits);

    Console.WriteLine($"Total circuits formed: {circuits.Count}");
    Console.WriteLine($"Sizes of circuits: {string.Join(", ", circuits.Select(c => c.Count).OrderBy(s => s))}");
    Console.WriteLine($"Product of largest differences: {circuits.Select(c => c.Count).OrderByDescending(s => s).Take(3).Aggregate(1, (a,b) => a*b)}");
}

public static List<HashSet<T>> UnionConnectedSets<T>(List<HashSet<T>> sets)
{
    var components = new List<HashSet<T>>();
    var processed = new HashSet<HashSet<T>>(ReferenceEqualityComparer.Instance);

    foreach (var current in sets)
    {
        if (processed.Contains(current)) continue;

        var component = new HashSet<T>(current);
        var queue = new Queue<HashSet<T>>(new[] { current });
        processed.Add(current);

        while (queue.Count > 0)
        {
            var set = queue.Dequeue();
            foreach (var other in sets)
            {
                if (processed.Contains(other)) continue;
                if (set.Overlaps(other))
                {
                    component.UnionWith(other);
                    processed.Add(other);
                    queue.Enqueue(other);
                }
            }
        }

        components.Add(component);
    }

    return components;
}