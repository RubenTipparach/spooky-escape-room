using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.InputSystem;

/// <summary>
/// Menu-based UI for navigation and interactions
/// Players navigate using arrow keys (P2) and WASD (P1)
/// Maintains separate state for each player
/// </summary>
public class GameUIManager : MonoBehaviour
{
    public static GameUIManager Instance { get; private set; }

    [SerializeField] private Transform menuPanel;
    [SerializeField] private GameObject menuButtonPrefab;
    [SerializeField] private TextMeshProUGUI currentLocationText;
    [SerializeField] private TextMeshProUGUI keyCountText;

    private List<GameObject> currentMenuButtons = new List<GameObject>();
    private List<System.Action> currentMenuActions = new List<System.Action>();
    private NavigationNode currentNode;

    // Separate state for each player
    private int player1SelectedMenuIndex = 0;
    private int player2SelectedMenuIndex = 0;
    private string player1InteractionPrompt = "";
    private string player2InteractionPrompt = "";

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
        SubscribeToGameEvents();
        UpdateKeyCountDisplay();
    }

    private void Update()
    {
        HandleMenuNavigation();
    }

    private void HandleMenuNavigation()
    {
        if (currentMenuButtons.Count == 0) return;

        // Player 1 uses WASD
        if (Keyboard.current.wKey.wasPressedThisFrame)
        {
            player1SelectedMenuIndex = (player1SelectedMenuIndex - 1 + currentMenuButtons.Count) % currentMenuButtons.Count;
            UpdateMenuHighlight();
        }

        if (Keyboard.current.sKey.wasPressedThisFrame)
        {
            player1SelectedMenuIndex = (player1SelectedMenuIndex + 1) % currentMenuButtons.Count;
            UpdateMenuHighlight();
        }

        // Z to select (Player 1)
        if (Keyboard.current.zKey.wasPressedThisFrame)
        {
            SelectCurrentMenu(1);
        }

        // Player 2 uses Arrow Keys
        if (Keyboard.current.upArrowKey.wasPressedThisFrame)
        {
            player2SelectedMenuIndex = (player2SelectedMenuIndex - 1 + currentMenuButtons.Count) % currentMenuButtons.Count;
            UpdateMenuHighlight();
        }

        if (Keyboard.current.downArrowKey.wasPressedThisFrame)
        {
            player2SelectedMenuIndex = (player2SelectedMenuIndex + 1) % currentMenuButtons.Count;
            UpdateMenuHighlight();
        }

        // M to select (Player 2)
        if (Keyboard.current.mKey.wasPressedThisFrame)
        {
            SelectCurrentMenu(2);
        }
    }

    private void UpdateMenuHighlight()
    {
        for (int i = 0; i < currentMenuButtons.Count; i++)
        {
            Image buttonImage = currentMenuButtons[i].GetComponent<Image>();
            if (buttonImage != null)
            {
                // Show both players' selections (P1 in yellow, P2 in cyan)
                if (i == player1SelectedMenuIndex)
                {
                    buttonImage.color = Color.yellow;
                }
                else if (i == player2SelectedMenuIndex)
                {
                    buttonImage.color = new Color(0, 1, 1); // Cyan
                }
                else
                {
                    buttonImage.color = Color.white;
                }
            }
        }
    }

    private void SelectCurrentMenu(int playerNumber)
    {
        int selectedIndex = (playerNumber == 1) ? player1SelectedMenuIndex : player2SelectedMenuIndex;

        if (selectedIndex >= 0 && selectedIndex < currentMenuActions.Count)
        {
            currentMenuActions[selectedIndex]?.Invoke();
        }
    }

    private void SubscribeToGameEvents()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnKeyCollected += UpdateKeyCountDisplay;
            GameManager.Instance.OnNodeChanged += OnNodeChanged;
        }
    }

    private void OnNodeChanged(NavigationNode node)
    {
        currentNode = node;
        UpdateLocationDisplay();
        UpdateMenuOptions();
        UpdateKeyCountDisplay();
    }

    private void UpdateLocationDisplay()
    {
        if (currentLocationText != null)
        {
            currentLocationText.text = currentNode.NodeName;
        }
    }

    private void UpdateKeyCountDisplay()
    {
        if (GameManager.Instance == null) return;

        int totalKeys = GameManager.Instance.GetTotalKeysCollected();

        if (keyCountText != null)
        {
            keyCountText.text = $"Keys Collected: {totalKeys}/4";
        }
    }

    private void UpdateKeyCountDisplay(int keyNumber)
    {
        UpdateKeyCountDisplay();
    }

    private void UpdateMenuOptions()
    {
        ClearMenuButtons();

        if (currentNode == null) return;

        // Add navigation options (move to other rooms)
        foreach (var connection in currentNode.Connections)
        {
            if (connection.destinationNode == null) continue;

            string buttonLabel = $"> {connection.label}";
            NavigationNode destination = connection.destinationNode;

            CreateMenuButton(buttonLabel, () =>
            {
                if (destination.CanAccess())
                {
                    GameManager.Instance.NavigateToNode(destination);
                }
                else
                {
                    Debug.Log($"Cannot access {destination.NodeName} - insufficient keys");
                }
            });
        }

        // Reset both players' menu selections
        player1SelectedMenuIndex = 0;
        player2SelectedMenuIndex = 0;
        if (currentMenuButtons.Count > 0)
        {
            UpdateMenuHighlight();
        }
    }

    private void CreateMenuButton(string label, System.Action onClicked)
    {
        if (menuButtonPrefab == null)
        {
            Debug.LogWarning("Menu button prefab not assigned!");
            return;
        }

        GameObject buttonObj = Instantiate(menuButtonPrefab, menuPanel);
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
                onClicked?.Invoke();
            });
        }

        currentMenuButtons.Add(buttonObj);
        currentMenuActions.Add(onClicked);
    }

    private void ClearMenuButtons()
    {
        foreach (GameObject button in currentMenuButtons)
        {
            Destroy(button);
        }
        currentMenuButtons.Clear();
        currentMenuActions.Clear();
    }

    public void UpdateInteractionPrompt(int playerNumber, string promptText)
    {
        if (playerNumber == 1)
        {
            player1InteractionPrompt = promptText;
        }
        else if (playerNumber == 2)
        {
            player2InteractionPrompt = promptText;
        }
        Debug.Log($"Player {playerNumber} interaction: {promptText}");
    }

    public void ClearInteractionPrompt(int playerNumber)
    {
        if (playerNumber == 1)
        {
            player1InteractionPrompt = "";
        }
        else if (playerNumber == 2)
        {
            player2InteractionPrompt = "";
        }
        Debug.Log($"Player {playerNumber} interaction prompt cleared");
    }

    private void OnDestroy()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnKeyCollected -= UpdateKeyCountDisplay;
            GameManager.Instance.OnNodeChanged -= OnNodeChanged;
        }
    }
}
