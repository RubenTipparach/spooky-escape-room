using UnityEngine;
using System;
using System.Collections.Generic;

/// <summary>
/// Central game manager handling game state, key collection, and progression
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private NavigationNode startingNode;
    [SerializeField] private NavigationNode masterBedroomNode;
    [SerializeField] private NavigationNode basementNode;

    [SerializeField] private Transform player1Transform;
    [SerializeField] private Transform player2Transform;

    // Game state
    private Dictionary<int, bool> keysCollected = new Dictionary<int, bool>();
    private NavigationNode currentNode;
    private bool gameOver = false;
    private bool demonEncountered = false;

    // Navigation
    private NavigationHandler navigationHandler;

    // Events
    public event Action<int> OnKeyCollected; // Called with key number
    public event Action<int> OnKeyCountChanged; // Called with total key count
    public event Action<NavigationNode> OnNodeChanged;
    public event Action OnDemonEncounter;
    public event Action OnGameOver;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Get or create navigation handler
        navigationHandler = GetComponent<NavigationHandler>();
        if (navigationHandler == null)
        {
            navigationHandler = gameObject.AddComponent<NavigationHandler>();
        }

        InitializeGameState();
    }

    private void InitializeGameState()
    {
        // Initialize keys 1-4
        for (int i = 1; i <= 4; i++)
        {
            keysCollected[i] = false;
        }

        if (startingNode != null)
        {
            currentNode = startingNode;
        }

        gameOver = false;
        demonEncountered = false;
    }

    private void Start()
    {
        // Position both players at the starting node and initialize UI
        if (startingNode != null)
        {
            // Directly position players without animation on startup
            if (player1Transform != null)
            {
                player1Transform.position = startingNode.transform.position;
            }
            if (player2Transform != null)
            {
                player2Transform.position = startingNode.transform.position;
            }

            // Trigger node change event to initialize menu and UI
            currentNode = startingNode;
            currentNode.MarkAsVisited();
            OnNodeChanged?.Invoke(currentNode);
            Debug.Log($"Game started at: {startingNode.NodeName}");
        }
    }

    /// <summary>
    /// Collect a key and trigger associated events
    /// </summary>
    public void CollectKey(int keyNumber)
    {
        if (keyNumber < 1 || keyNumber > 4)
        {
            Debug.LogWarning($"Invalid key number: {keyNumber}");
            return;
        }

        if (keysCollected[keyNumber])
        {
            Debug.Log($"Key {keyNumber} already collected");
            return;
        }

        keysCollected[keyNumber] = true;
        Debug.Log($"Key {keyNumber} collected! Total keys: {GetTotalKeysCollected()}");

        OnKeyCollected?.Invoke(keyNumber);
        OnKeyCountChanged?.Invoke(GetTotalKeysCollected());

        // Check if master bedroom should now be accessible
        if (GetTotalKeysCollected() == 3 && masterBedroomNode != null)
        {
            Debug.Log("Master bedroom is now unlocked!");
        }

        // Check if basement should now be accessible
        if (GetTotalKeysCollected() == 4 && basementNode != null)
        {
            Debug.Log("Basement is now unlocked! The final ritual awaits...");
        }
    }

    public bool HasKey(int keyNumber)
    {
        return keysCollected.ContainsKey(keyNumber) && keysCollected[keyNumber];
    }

    public int GetTotalKeysCollected()
    {
        int count = 0;
        foreach (var key in keysCollected.Values)
        {
            if (key) count++;
        }
        return count;
    }

    public void NavigateToNode(NavigationNode node, int playerNumber = 0)
    {
        if (node == null)
        {
            Debug.LogWarning("Cannot navigate to null node");
            return;
        }

        if (!node.CanAccess())
        {
            Debug.LogWarning($"Cannot access {node.NodeName} - locked and insufficient keys");
            return;
        }

        // Move the player smoothly to the node
        if (playerNumber == 1 && player1Transform != null)
        {
            NavigatePlayerToNode(player1Transform, node);
        }
        else if (playerNumber == 2 && player2Transform != null)
        {
            NavigatePlayerToNode(player2Transform, node);
        }
        else if (playerNumber == 0)
        {
            // Navigate both players
            if (player1Transform != null)
                NavigatePlayerToNode(player1Transform, node);
            if (player2Transform != null)
                NavigatePlayerToNode(player2Transform, node);
        }

        currentNode = node;
        currentNode.MarkAsVisited();
        OnNodeChanged?.Invoke(currentNode);
        Debug.Log($"Navigated to: {node.NodeName}");
    }

    private void NavigatePlayerToNode(Transform playerTransform, NavigationNode destinationNode)
    {
        if (navigationHandler == null) return;

        // Find the connection to get intermediate nodes
        List<NavigationNode> intermediateNodes = null;
        foreach (var connection in currentNode.Connections)
        {
            if (connection.destinationNode == destinationNode)
            {
                intermediateNodes = connection.intermediateNodes;
                break;
            }
        }

        navigationHandler.NavigateToNode(playerTransform, destinationNode, intermediateNodes);
    }

    public NavigationNode GetCurrentNode()
    {
        return currentNode;
    }

    public void TriggerDemonEncounter()
    {
        if (demonEncountered) return;

        demonEncountered = true;
        OnDemonEncounter?.Invoke();
        Debug.Log("A demon appears! The ritual has been activated!");
    }

    public void EndGame(bool playerWon)
    {
        if (gameOver) return;

        gameOver = true;
        OnGameOver?.Invoke();

        if (playerWon)
        {
            Debug.Log("VICTORY! Players escaped the haunted house!");
        }
        else
        {
            Debug.Log("GAME OVER! The demon has consumed the players...");
        }
    }

    public bool IsGameOver()
    {
        return gameOver;
    }

    public bool HasDemonBeenEncountered()
    {
        return demonEncountered;
    }

    /// <summary>
    /// Reset game to initial state (for replaying)
    /// </summary>
    public void ResetGame()
    {
        InitializeGameState();
        Debug.Log("Game reset");
    }

#if UNITY_EDITOR
    private void OnGUI()
    {
        if (GUILayout.Button("Dev: Collect Key 1"))
            CollectKey(1);
        if (GUILayout.Button("Dev: Collect Key 2"))
            CollectKey(2);
        if (GUILayout.Button("Dev: Collect Key 3"))
            CollectKey(3);
        if (GUILayout.Button("Dev: Collect Key 4"))
            CollectKey(4);
    }
#endif
}
