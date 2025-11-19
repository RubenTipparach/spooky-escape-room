using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.UI;

/// <summary>
/// Manages all UI elements for both players
/// Displays prompts, key counters, and navigation options
/// </summary>
public class GameUIManager : MonoBehaviour
{
    public static GameUIManager Instance { get; private set; }

    [SerializeField] private TextMeshProUGUI player1KeyCountUI;
    [SerializeField] private TextMeshProUGUI player2KeyCountUI;

    [SerializeField] private TextMeshProUGUI player1InteractionPrompt;
    [SerializeField] private TextMeshProUGUI player2InteractionPrompt;

    [SerializeField] private TextMeshProUGUI player1NodeNameUI;
    [SerializeField] private TextMeshProUGUI player2NodeNameUI;

    [SerializeField] private Transform player1NavigationPanel;
    [SerializeField] private Transform player2NavigationPanel;

    [SerializeField] private GameObject navigationButtonPrefab;

    // Control scheme text
    [SerializeField] private TextMeshProUGUI player1ControlsUI;
    [SerializeField] private TextMeshProUGUI player2ControlsUI;

    private Dictionary<Transform, List<GameObject>> navigationButtons = new Dictionary<Transform, List<GameObject>>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        SetupInitialUI();
        SubscribeToGameEvents();
    }

    private void SetupInitialUI()
    {
        // Setup control scheme displays
        if (player1ControlsUI != null)
        {
            player1ControlsUI.text = "PLAYER 1\nMovement: WASD\nInteract: Z\nAlt Interact: X";
        }

        if (player2ControlsUI != null)
        {
            player2ControlsUI.text = "PLAYER 2\nMovement: Arrows\nInteract: M\nAlt Interact: N";
        }

        // Initialize navigation button lists
        if (player1NavigationPanel != null)
            navigationButtons[player1NavigationPanel] = new List<GameObject>();
        if (player2NavigationPanel != null)
            navigationButtons[player2NavigationPanel] = new List<GameObject>();

        UpdateKeyCountDisplay();
    }

    private void SubscribeToGameEvents()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnKeyCollected += UpdateKeyCountDisplay;
            GameManager.Instance.OnNodeChanged += OnNodeChanged;
            GameManager.Instance.OnGameOver += OnGameOver;
        }
    }

    private void UpdateKeyCountDisplay()
    {
        if (GameManager.Instance == null) return;

        int totalKeys = GameManager.Instance.GetTotalKeysCollected();

        if (player1KeyCountUI != null)
            player1KeyCountUI.text = $"Keys: {totalKeys}/4";

        if (player2KeyCountUI != null)
            player2KeyCountUI.text = $"Keys: {totalKeys}/4";
    }

    private void UpdateKeyCountDisplay(int keyNumber)
    {
        UpdateKeyCountDisplay();
    }

    public void UpdateInteractionPrompt(int playerNumber, string promptText)
    {
        if (playerNumber == 1 && player1InteractionPrompt != null)
        {
            player1InteractionPrompt.text = promptText;
        }
        else if (playerNumber == 2 && player2InteractionPrompt != null)
        {
            player2InteractionPrompt.text = promptText;
        }
    }

    public void ClearInteractionPrompt(int playerNumber)
    {
        UpdateInteractionPrompt(playerNumber, "");
    }

    private void OnNodeChanged(NavigationNode node)
    {
        // Update both players' node name display
        if (player1NodeNameUI != null)
            player1NodeNameUI.text = node.NodeName;

        if (player2NodeNameUI != null)
            player2NodeNameUI.text = node.NodeName;

        // Update navigation options
        UpdateNavigationOptions(node);
    }

    private void UpdateNavigationOptions(NavigationNode node)
    {
        // Clear existing buttons
        ClearNavigationButtons(player1NavigationPanel);
        ClearNavigationButtons(player2NavigationPanel);

        // Create buttons for each connection
        foreach (var connection in node.Connections)
        {
            if (connection.destinationNode == null) continue;

            if (player1NavigationPanel != null)
            {
                CreateNavigationButton(player1NavigationPanel, connection.label, connection.destinationNode, 1);
            }

            if (player2NavigationPanel != null)
            {
                CreateNavigationButton(player2NavigationPanel, connection.label, connection.destinationNode, 2);
            }
        }
    }

    private void CreateNavigationButton(Transform parentPanel, string label, NavigationNode targetNode, int playerNumber)
    {
        if (navigationButtonPrefab == null)
        {
            Debug.LogWarning("Navigation button prefab not assigned!");
            return;
        }

        GameObject buttonObj = Instantiate(navigationButtonPrefab, parentPanel);
        TextMeshProUGUI buttonText = buttonObj.GetComponentInChildren<TextMeshProUGUI>();

        if (buttonText != null)
        {
            buttonText.text = label;
        }

        Button button = buttonObj.GetComponent<Button>();
        if (button != null)
        {
            button.onClick.AddListener(() =>
            {
                if (targetNode.CanAccess())
                {
                    GameManager.Instance.NavigateToNode(targetNode);
                }
                else
                {
                    Debug.Log($"Cannot access {targetNode.NodeName} - insufficient keys");
                }
            });
        }

        if (!navigationButtons.ContainsKey(parentPanel))
            navigationButtons[parentPanel] = new List<GameObject>();

        navigationButtons[parentPanel].Add(buttonObj);
    }

    private void ClearNavigationButtons(Transform parentPanel)
    {
        if (parentPanel == null || !navigationButtons.ContainsKey(parentPanel))
            return;

        foreach (GameObject button in navigationButtons[parentPanel])
        {
            Destroy(button);
        }

        navigationButtons[parentPanel].Clear();
    }

    private void OnGameOver()
    {
        // Display game over message
        // This can be expanded with a game over screen
        Debug.Log("Game Over - UI should display end screen");
    }

    public void DisplayMessage(string message, float duration = 3f)
    {
        // Placeholder for general message display
        Debug.Log($"Message: {message}");
    }

    private void OnDestroy()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnKeyCollected -= UpdateKeyCountDisplay;
            GameManager.Instance.OnNodeChanged -= OnNodeChanged;
            GameManager.Instance.OnGameOver -= OnGameOver;
        }
    }
}
