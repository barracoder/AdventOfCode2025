using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

public class GraphNode<T>
{
    public T Value { get; }
    public T[] Neighbors { get; set; }               

    public GraphNode(T value, params T[] neighbors)
    {
        Value = value;
        Neighbors = neighbors;
    }
}

public class Graph<T> where T : notnull
{
    private readonly GraphNode<T>[] _nodes;                    
    private readonly Dictionary<T, GraphNode<T>> _valueToNode; 

    public Graph(IEnumerable<(T value, IEnumerable<T> neighbors)> edges)
    {
        var nodeList = new List<GraphNode<T>>();
        var tempMap = new Dictionary<T, GraphNode<T>>();

        foreach (var (value, _) in edges.DistinctBy(e => e.value))
        {
            var node = new GraphNode<T>(value);
            nodeList.Add(node);
            tempMap[value] = node;
        }

        for (int i = 0; i < nodeList.Count; i++)
        {
            var (value, neighborValues) = edges.ElementAt(i);
            var node = nodeList[i];
            node.Neighbors = neighborValues
                .Select(nv => Array.IndexOf(nodeList.ToArray(), tempMap[nv])) 
                .Where(idx => idx >= 0)
                .ToArray();
        }

        _nodes = nodeList.ToArray();
        _valueToNode = tempMap;
    }

    public bool Contains(T value) => _valueToNode.ContainsKey(value);

    public bool TryGetNode(T value, [MaybeNullWhen(false)] out GraphNode<T> node)
        => _valueToNode.TryGetValue(value, out node);
}

public class GraphBuilder
{
    public static List<HashSet<T>> BuildConnectedComponents(List<(T node1, T node2)> edges)
    {
        // Build adjacency list
        var graph = new Dictionary<T, HashSet<T>>();
        
        foreach (var (n1, n2) in edges)
        {
            if (!graph.ContainsKey(n1))
                graph[n1] = new HashSet<T>();
            if (!graph.ContainsKey(n2))
                graph[n2] = new HashSet<T>();
                
            graph[n1].Add(n2);
            graph[n2].Add(n1); // undirected graph
        }

        var visited = new HashSet<T>();
        var components = new List<HashSet<T>>();

        foreach (var node in graph.Keys)
        {
            if (!visited.Contains(node))
            {
                var component = new HashSet<T>();
                Dfs(node, graph, visited, component);
                components.Add(component);
            }
        }

        // Also handle isolated nodes that might not appear in edges
        // (if your input only has edges, this part is optional)
        // But if you have nodes that never appear, you need to track all nodes separately

        return components;
    }

    private static void DepthFirstSearch(T node, Dictionary<T, HashSet<T>> graph, HashSet<T> visited, HashSet<T> component)
    {
        visited.Add(node);
        component.Add(node);

        if (graph.TryGetValue(node, out var neighbors))
        {
            foreach (var neighbor in neighbors)
            {
                if (!visited.Contains(neighbor))
                {
                    DepthFirstSearch(neighbor, graph, visited, component);
                }
            }
        }
    }
}