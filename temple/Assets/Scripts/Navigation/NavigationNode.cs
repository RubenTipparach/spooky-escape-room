using UnityEngine;
using System.Collections.Generic;

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

    [SerializeField] private List<NavigationNode> connectedNodes = new List<NavigationNode>();
    [SerializeField] private List<string> connectionLabels = new List<string>();

    // Puzzle and key related
    [SerializeField] private bool hasPuzzle = false;
    [SerializeField] private Puzzle puzzleComponent = null;
    [SerializeField] private bool hasKey = false;
    [SerializeField] private int keyNumber = 0;
    [SerializeField] private bool isLocked = false;
    [SerializeField] private int keysRequired = 0;

    // Player spawn points when entering this node
    [SerializeField] private Transform player1SpawnPoint;
    [SerializeField] private Transform player2SpawnPoint;

    private bool hasBeenVisited = false;

    public string NodeName => nodeName;
    public string NodeDescription => nodeDescription;
    public string AtmosphereDescription => atmosphereDescription;
    public List<NavigationNode> ConnectedNodes => connectedNodes;
    public List<string> ConnectionLabels => connectionLabels;
    public bool HasPuzzle => hasPuzzle;
    public Puzzle PuzzleComponent => puzzleComponent;
    public bool HasKey => hasKey;
    public int KeyNumber => keyNumber;
    public bool IsLocked => isLocked;
    public int KeysRequired => keysRequired;
    public bool HasBeenVisited => hasBeenVisited;

    private void OnValidate()
    {
        // Ensure labels match connected nodes count
        while (connectionLabels.Count < connectedNodes.Count)
        {
            connectionLabels.Add("Go to Room");
        }
        while (connectionLabels.Count > connectedNodes.Count)
        {
            connectionLabels.RemoveAt(connectionLabels.Count - 1);
        }
    }

    public Transform GetSpawnPoint(int playerNumber)
    {
        return playerNumber == 1 ? player1SpawnPoint : player2SpawnPoint;
    }

    public void MarkAsVisited()
    {
        hasBeenVisited = true;
    }

    public string GetConnectionDescription(int index)
    {
        if (index >= 0 && index < connectionLabels.Count)
        {
            return connectionLabels[index];
        }
        return "Go";
    }

    public bool CanAccess()
    {
        if (!isLocked) return true;

        // Check if player has enough keys
        GameManager manager = GameManager.Instance;
        return manager != null && manager.GetTotalKeysCollected() >= keysRequired;
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        // Draw connections to other nodes for debugging
        Gizmos.color = Color.cyan;
        foreach (var connectedNode in connectedNodes)
        {
            if (connectedNode != null)
            {
                Gizmos.DrawLine(transform.position, connectedNode.transform.position);
                Gizmos.DrawSphere(connectedNode.transform.position, 0.5f);
            }
        }

        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, 0.5f);
    }
#endif
}
