using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Represents a connection to another node, optionally with intermediate nodes for traversal animation
/// </summary>
[System.Serializable]
public class NodeConnection
{
    [SerializeField] public NavigationNode destinationNode;
    [SerializeField] public string label = "Go to Room";
    [SerializeField] public List<NavigationNode> intermediateNodes = new List<NavigationNode>();
}

/// <summary>
/// Represents a navigable location in the game world (rooms, hallways, etc.)
/// Players can traverse between connected nodes
/// </summary>
public class NavigationNode : MonoBehaviour
{
    [SerializeField] private string nodeName = "Room";
    [SerializeField] private string nodeDescription = "A room in the haunted house";
    [TextArea(3, 5)]
    [SerializeField] private string atmosphereDescription = "The air feels cold and still...";

    [SerializeField] private List<NodeConnection> connections = new List<NodeConnection>();

    // Camera settings for this node
    [SerializeField] private Vector3 cameraLookRotation = Vector3.zero; // Euler angles for where camera should face

    // Puzzle and key related
    [SerializeField] private bool hasPuzzle = false;
    [SerializeField] private Puzzle puzzleComponent = null;
    [SerializeField] private bool hasKey = false;
    [SerializeField] private int keyNumber = 0;
    [SerializeField] private bool isLocked = false;
    [SerializeField] private int keysRequired = 0;

    // Intermediate nodes for traversal animation
    [SerializeField] private bool isIntermediateNode = false;
    [SerializeField] private List<InteractableObject> doorsToOpen = new List<InteractableObject>();
    [SerializeField] private bool isLivingRoom = false;

    private bool hasBeenVisited = false;

    public string NodeName => nodeName;
    public string NodeDescription => nodeDescription;
    public string AtmosphereDescription => atmosphereDescription;
    public List<NodeConnection> Connections => connections;
    public Vector3 CameraLookRotation => cameraLookRotation;
    public bool HasPuzzle => hasPuzzle;
    public Puzzle PuzzleComponent => puzzleComponent;
    public bool HasKey => hasKey;
    public int KeyNumber => keyNumber;
    public bool IsLocked => isLocked;
    public int KeysRequired => keysRequired;
    public bool HasBeenVisited => hasBeenVisited;
    public bool IsIntermediateNode => isIntermediateNode;
    public List<InteractableObject> DoorsToOpen => doorsToOpen;
    public bool IsLivingRoom => isLivingRoom;

    public FacingDirection FacingDirection;
    

    public void MarkAsVisited()
    {
        hasBeenVisited = true;
    }

    public string GetConnectionDescription(int index)
    {
        if (index >= 0 && index < connections.Count)
        {
            return connections[index].label;
        }
        return "Go";
    }

    public NavigationNode GetConnectedNode(int index)
    {
        if (index >= 0 && index < connections.Count)
        {
            return connections[index].destinationNode;
        }
        return null;
    }

    public bool CanAccess()
    {
        if (!isLocked) return true;

        // Check if player has enough keys
        GameManager manager = GameManager.Instance;
        return manager != null && manager.GetTotalKeysCollected() >= keysRequired;
    }

    public void OpenDoorsOnTraversal()
    {
        // Open all doors associated with this traversal
        foreach (var door in doorsToOpen)
        {
            if (door != null)
            {
                door.OnInteract(null);
                Debug.Log($"Door opened on traversal through {nodeName}");
            }
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        // Draw this node as a cube (larger for main nodes, smaller red for intermediate)
        if (isIntermediateNode)
        {
            if (ShouldDrawIntermediateNodes())
            {
                Gizmos.color = Color.red;
                Gizmos.DrawCube(transform.position, Vector3.one * 0.5f);
            }
        }
        else
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawCube(transform.position, Vector3.one * 1f);

            // Draw label for non-intermediate nodes when selected
            if (ShouldDrawNodeLabels())
            {
                UnityEditor.Handles.color = Color.white;
                UnityEditor.Handles.Label(transform.position + Vector3.up * 0.75f, nodeName);
            }
        }

        // Draw connections to other nodes
        if (ShouldDrawNavigationPaths())
        {
            Gizmos.color = Color.cyan;
            foreach (var connection in connections)
            {
                DrawConnectionPath(connection);
            }
        }
    }

    private void OnDrawGizmos()
    {
        // Draw node cube even when not selected (for better visibility in large scenes)
        if (isIntermediateNode)
        {
            if (ShouldDrawIntermediateNodes())
            {
                Gizmos.color = new Color(1, 0, 0, 0.3f); // Red, semi-transparent
                Gizmos.DrawCube(transform.position, Vector3.one * 0.5f);
            }
        }
        else
        {
            Gizmos.color = new Color(1, 1, 0, 0.3f); // Yellow, semi-transparent
            Gizmos.DrawCube(transform.position, Vector3.one * 1f);
        }

        // Draw connections to other nodes
        if (ShouldDrawNavigationPaths())
        {
            Gizmos.color = Color.cyan;
            foreach (var connection in connections)
            {
                DrawConnectionPath(connection);
            }
        }
    }

    private bool ShouldDrawNavigationPaths()
    {
        return DebugVisualizationManager.Instance != null && DebugVisualizationManager.Instance.DrawNavigationPaths;
    }

    private bool ShouldDrawIntermediateNodes()
    {
        return DebugVisualizationManager.Instance != null && DebugVisualizationManager.Instance.DrawIntermediateNodes;
    }

    private bool ShouldDrawNodeLabels()
    {
        return DebugVisualizationManager.Instance != null && DebugVisualizationManager.Instance.DrawNodeLabels;
    }

    private void DrawConnectionPath(NodeConnection connection)
    {
        if (connection.destinationNode == null) return;

        Vector3 currentPosition = transform.position;

        // Draw lines through intermediate nodes in order
        if (connection.intermediateNodes.Count > 0)
        {
            foreach (var intermediateNode in connection.intermediateNodes)
            {
                if (intermediateNode != null)
                {
                    Gizmos.DrawLine(currentPosition, intermediateNode.transform.position);
                    currentPosition = intermediateNode.transform.position;
                }
            }
        }

        // Draw final line to destination
        Gizmos.DrawLine(currentPosition, connection.destinationNode.transform.position);
    }
#endif
}

public enum FacingDirection
{
    North,
    East,
    South,
    West
}