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
    var (circuits, unusedJunctions) = BuildCircuits(1000);

    Console.WriteLine($"Total circuits formed: {circuits.Count}");
    Console.WriteLine($"Sizes of circuits: {string.Join(", ", circuits.Select(c => c.Count).OrderBy(s => s))}");
    Console.WriteLine($"Product of largest differences: {circuits.Select(c => c.Count).OrderByDescending(s => s).Take(3).Aggregate(1, (a, b) => a * b)}");
}

using (new Timer("Stage 2"))
{
    var (circuits, unusedJunctions) = BuildCircuits(1000);
    circuits.AddRange(unusedJunctions.Select(j => new HashSet<Vector3> { j }));

    Console.WriteLine($"Total circuits formed: {circuits.Count}");
    Console.WriteLine($"Sizes of circuits: {string.Join(", ", circuits.Select(c => c.Count).OrderBy(s => s))}");

    (Vector3 lhs, Vector3 rhs) lastConnectedPair = (new Vector3(), new Vector3());
    while (circuits.Count > 1)
    {
        var closestPair = GetClosestPairBetweenCircuits(circuits);
        var lhsCircuit = circuits.First(c => c.Contains(closestPair.lhs));
        var rhsCircuit = circuits.First(c => c.Contains(closestPair.rhs));
        lhsCircuit.UnionWith(rhsCircuit);
        circuits.Remove(rhsCircuit);
        lastConnectedPair = (closestPair.lhs, closestPair.rhs);
        // Console.WriteLine($"Connecting {closestPair.lhs} and {closestPair.rhs} with distance {closestPair.distance}. Circuits remaining: {circuits.Count}");
    }

    Console.WriteLine($"Final connection between {lastConnectedPair} and {lastConnectedPair.rhs}");
    Console.WriteLine($"Final circuits size: {circuits[0].Count}");
    Console.WriteLine($"Product of final X: {Convert.ToInt64(lastConnectedPair.lhs.X) * Convert.ToInt64(lastConnectedPair.rhs.X)}");
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

(List<HashSet<Vector3>>, HashSet<Vector3>) BuildCircuits(int closestCount)
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

    var used = shortestDifferenceBetweenAnyTwoVectors.Take(closestCount).ToList();
    var circuits = new List<HashSet<Vector3>>();
    var unusedJunctionBoxes = vectors.Select(v => v).ToHashSet();
    foreach (var nextClosest in used)
    {
        Console.WriteLine($"Next closest difference: {nextClosest.Difference} between {nextClosest.Vectors[0]} and {nextClosest.Vectors[1]}");
    }

    foreach (var connection in used)
    {
        var (v1, v2) = (connection.Vectors[0], connection.Vectors[1]);
        var existingCircuit = false;
        for (int i = 0; i < circuits.Count; i++)
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

        if (!existingCircuit)
        {
            circuits.Add(new HashSet<Vector3> { v1, v2 });
        }

        unusedJunctionBoxes.Remove(v1);
        unusedJunctionBoxes.Remove(v2);
    }

    circuits = UnionConnectedSets(circuits);

    return (circuits, unusedJunctionBoxes);
}

// get the shortest distance between any two vectors in two lists
IEnumerable<(Vector3 circuitVector, Vector3 unusedVector, float distance)> ShortestDistancesBetweenVectors(List<HashSet<Vector3>> circuits, List<Vector3> unusedJunctions)
{
    foreach (var junction in unusedJunctions)
    {
        float shortestDistance = float.MaxValue;
        (Vector3, Vector3, float) closestPair = (new Vector3(), new Vector3(), 0);
        foreach (var circuit in circuits)
        {
            foreach (var vector in circuit)
            {
                var distance = (vector - junction).LengthSquared();
                if (distance < shortestDistance)
                {
                    shortestDistance = distance;
                    closestPair = (vector, junction, distance);
                }
            }

        }
        yield return closestPair;
    }
}

(Vector3, Vector3) GetAllCombinations(List<HashSet<Vector3>> circuits, List<Vector3> unusedJunctions)
{
    float shortestDistance = float.MaxValue;
    (Vector3, Vector3) closestPair = (new Vector3(), new Vector3());
    foreach (var circuit in circuits)
    {
        foreach (var junction in unusedJunctions)
        {
            foreach (var vector in circuit)
            {
                var distance = (vector - junction).LengthSquared();
                if (distance < shortestDistance)
                {
                    shortestDistance = distance;
                    closestPair = (vector, junction);
                }
            }
        }
    }
    return closestPair;
}

(Vector3 lhs, Vector3 rhs, float distance) GetClosestPairBetweenCircuits(List<HashSet<Vector3>> circuits)
{
    float shortestDistance = float.MaxValue;
    (Vector3, Vector3, float) closestPair = (new Vector3(), new Vector3(), 0);
    for (int i = 0; i < circuits.Count; i++)
    {
        for (int j = i + 1; j < circuits.Count; j++)
        {
            foreach (var lhs in circuits[i])
            {
                foreach (var rhs in circuits[j])
                {
                    var distance = Vector3.Distance(lhs, rhs);
                    if (distance < shortestDistance)
                    {
                        shortestDistance = distance;
                        closestPair = (lhs, rhs, distance);
                    }
                }
            }
        }
    }
    return closestPair;
}