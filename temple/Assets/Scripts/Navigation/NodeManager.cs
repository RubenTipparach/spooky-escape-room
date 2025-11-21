using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Manages all navigation nodes in the game
/// Handles node registration and setup
/// </summary>
public class NodeManager : MonoBehaviour
{
    public static NodeManager Instance { get; private set; }

    private Dictionary<string, NavigationNode> nodesRegistry = new Dictionary<string, NavigationNode>();
    private List<NavigationNode> allNodes = new List<NavigationNode>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        RegisterAllNodes();
    }

    private void RegisterAllNodes()
    {
        // Find all navigation nodes in the scene
        NavigationNode[] nodes = FindObjectsOfType<NavigationNode>();

        foreach (NavigationNode node in nodes)
        {
            RegisterNode(node);
        }

        Debug.Log($"Registered {allNodes.Count} navigation nodes");
    }

    private void RegisterNode(NavigationNode node)
    {
        if (node == null) return;

        // Skip intermediate nodes - they're only used for traversal animation
        if (node.IsIntermediateNode) return;

        if (nodesRegistry.ContainsKey(node.NodeName))
        {
            Debug.LogWarning($"Node '{node.NodeName}' already registered! {node.gameObject.name}");
            return;
        }

        nodesRegistry[node.NodeName] = node;
        allNodes.Add(node);
        Debug.Log($"Registered node: {node.NodeName}");
    }

    public NavigationNode GetNode(string nodeName)
    {
        if (nodesRegistry.ContainsKey(nodeName))
        {
            return nodesRegistry[nodeName];
        }

        Debug.LogWarning($"Node '{nodeName}' not found in registry");
        return null;
    }

    public List<NavigationNode> GetAllNodes()
    {
        return new List<NavigationNode>(allNodes);
    }

    public void PrintNodeMap()
    {
        Debug.Log("=== Node Map ===");
        foreach (var node in allNodes)
        {
            Debug.Log($"Node: {node.NodeName}");
            Debug.Log($"  Connections: {node.Connections.Count}");
            foreach (var connection in node.Connections)
            {
                if (connection.destinationNode != null)
                {
                    Debug.Log($"    -> {connection.label}: {connection.destinationNode.NodeName}");
                }
            }
        }
    }
}
