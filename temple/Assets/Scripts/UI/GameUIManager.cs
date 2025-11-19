using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.InputSystem;

/// <summary>
/// Encapsulates UI elements and logic for a single player
/// </summary>
[System.Serializable]
public class PlayerUIContext
{
    public int playerNumber;
    public TextMeshProUGUI keyCountUI;
    public TextMeshProUGUI interactionPrompt;
    public TextMeshProUGUI nodeNameUI;
    public Transform navigationPanel;
    public TextMeshProUGUI controlsUI;

    private List<GameObject> navigationButtons = new List<GameObject>();
    private List<NavigationNode> currentConnections = new List<NavigationNode>();
    private int selectedConnectionIndex = 0;

    public void Initialize()
    {
        if (controlsUI != null)
        {
            if (playerNumber == 1)
            {
                controlsUI.text = "PLAYER 1\nNavigate: WASD\nSelect: Z\nCancel: X";
            }
            else
            {
                controlsUI.text = "PLAYER 2\nNavigate: Arrows\nSelect: M\nCancel: N";
            }
        }
    }

    public void UpdateNodeDisplay(NavigationNode node)
    {
        if (nodeNameUI != null)
            nodeNameUI.text = node.NodeName;
    }

    public void UpdateKeyCountDisplay(int totalKeys)
    {
        if (keyCountUI != null)
            keyCountUI.text = $"Keys: {totalKeys}/4";
    }

    public void UpdateNavigationOptions(NavigationNode node, GameObject navigationButtonPrefab)
    {
        ClearNavigationButtons();
        currentConnections.Clear();
        selectedConnectionIndex = 0;

        foreach (var connection in node.Connections)
        {
            if (connection.destinationNode == null) continue;
            currentConnections.Add(connection.destinationNode);
            CreateNavigationButton(connection.label, connection.destinationNode, navigationButtonPrefab);
        }

        UpdateNavigationHighlight();
    }

    private void CreateNavigationButton(string label, NavigationNode targetNode, GameObject navigationButtonPrefab)
    {
        if (navigationButtonPrefab == null)
        {
            Debug.LogWarning("Navigation button prefab not assigned!");
            return;
        }

        GameObject buttonObj = Object.Instantiate(navigationButtonPrefab, navigationPanel);
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

        navigationButtons.Add(buttonObj);
    }

    public void HandleNavigationInput(float horizontal, float vertical)
    {
        if (currentConnections.Count == 0) return;

        if (horizontal != 0)
        {
            selectedConnectionIndex += (int)Mathf.Sign(horizontal);
            selectedConnectionIndex = Mathf.Clamp(selectedConnectionIndex, 0, currentConnections.Count - 1);
            UpdateNavigationHighlight();
        }
        else if (vertical != 0)
        {
            selectedConnectionIndex += (int)Mathf.Sign(vertical);
            selectedConnectionIndex = Mathf.Clamp(selectedConnectionIndex, 0, currentConnections.Count - 1);
            UpdateNavigationHighlight();
        }
    }

    public void SelectCurrentConnection()
    {
        if (selectedConnectionIndex >= 0 && selectedConnectionIndex < currentConnections.Count)
        {
            NavigationNode selectedNode = currentConnections[selectedConnectionIndex];
            if (selectedNode.CanAccess())
            {
                GameManager.Instance.NavigateToNode(selectedNode);
            }
        }
    }

    private void UpdateNavigationHighlight()
    {
        for (int i = 0; i < navigationButtons.Count; i++)
        {
            Image buttonImage = navigationButtons[i].GetComponent<Image>();
            if (buttonImage != null)
            {
                buttonImage.color = (i == selectedConnectionIndex) ? Color.yellow : Color.white;
            }
        }
    }

    private void ClearNavigationButtons()
    {
        foreach (GameObject button in navigationButtons)
        {
            Object.Destroy(button);
        }
        navigationButtons.Clear();
    }

    public void UpdateInteractionPrompt(string promptText)
    {
        if (interactionPrompt != null)
            interactionPrompt.text = promptText;
    }

    public void ClearInteractionPrompt()
    {
        UpdateInteractionPrompt("");
    }
}

/// <summary>
/// Manages all UI elements for both players
/// Displays prompts, key counters, and navigation options with directional controls
/// </summary>
public class GameUIManager : MonoBehaviour
{
    public static GameUIManager Instance { get; private set; }

    [SerializeField] private PlayerUIContext player1UI = new PlayerUIContext();
    [SerializeField] private PlayerUIContext player2UI = new PlayerUIContext();
    [SerializeField] private GameObject navigationButtonPrefab;

    // Input System references for navigation
    private InputAction player1NavAction;
    private InputAction player2NavAction;
    private InputAction player1SelectAction;
    private InputAction player2SelectAction;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Initialize player contexts
        player1UI.playerNumber = 1;
        player2UI.playerNumber = 2;
    }

    private void OnEnable()
    {
        SetupInputActions();
    }

    private void OnDisable()
    {
        TeardownInputActions();
    }

    private void Start()
    {
        SetupInitialUI();
        SubscribeToGameEvents();
    }

    private void SetupInitialUI()
    {
        player1UI.Initialize();
        player2UI.Initialize();
        UpdateKeyCountDisplay();
    }

    private void SetupInputActions()
    {
        // Player 1 navigation
        player1NavAction = new InputAction(type: InputActionType.Value, binding: "<Keyboard>/w,<Keyboard>/a,<Keyboard>/s,<Keyboard>/d");
        player1NavAction.performed += _ => HandlePlayer1Navigation();
        player1NavAction.Enable();

        player1SelectAction = new InputAction(binding: "<Keyboard>/z");
        player1SelectAction.performed += _ => player1UI.SelectCurrentConnection();
        player1SelectAction.Enable();

        // Player 2 navigation
        player2NavAction = new InputAction(type: InputActionType.Value, binding: "<Keyboard>/upArrow,<Keyboard>/leftArrow,<Keyboard>/downArrow,<Keyboard>/rightArrow");
        player2NavAction.performed += _ => HandlePlayer2Navigation();
        player2NavAction.Enable();

        player2SelectAction = new InputAction(binding: "<Keyboard>/m");
        player2SelectAction.performed += _ => player2UI.SelectCurrentConnection();
        player2SelectAction.Enable();
    }

    private void TeardownInputActions()
    {
        player1NavAction?.Disable();
        player2NavAction?.Disable();
        player1SelectAction?.Disable();
        player2SelectAction?.Disable();
    }

    private void HandlePlayer1Navigation()
    {
        float horizontal = 0;
        float vertical = 0;

        if (Keyboard.current.wKey.isPressed) vertical += 1;
        if (Keyboard.current.sKey.isPressed) vertical -= 1;
        if (Keyboard.current.aKey.isPressed) horizontal -= 1;
        if (Keyboard.current.dKey.isPressed) horizontal += 1;

        player1UI.HandleNavigationInput(horizontal, vertical);
    }

    private void HandlePlayer2Navigation()
    {
        float horizontal = 0;
        float vertical = 0;

        if (Keyboard.current.upArrowKey.isPressed) vertical += 1;
        if (Keyboard.current.downArrowKey.isPressed) vertical -= 1;
        if (Keyboard.current.leftArrowKey.isPressed) horizontal -= 1;
        if (Keyboard.current.rightArrowKey.isPressed) horizontal += 1;

        player2UI.HandleNavigationInput(horizontal, vertical);
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
        player1UI.UpdateKeyCountDisplay(totalKeys);
        player2UI.UpdateKeyCountDisplay(totalKeys);
    }

    private void UpdateKeyCountDisplay(int keyNumber)
    {
        UpdateKeyCountDisplay();
    }

    public void UpdateInteractionPrompt(int playerNumber, string promptText)
    {
        if (playerNumber == 1)
            player1UI.UpdateInteractionPrompt(promptText);
        else if (playerNumber == 2)
            player2UI.UpdateInteractionPrompt(promptText);
    }

    public void ClearInteractionPrompt(int playerNumber)
    {
        UpdateInteractionPrompt(playerNumber, "");
    }

    private void OnNodeChanged(NavigationNode node)
    {
        player1UI.UpdateNodeDisplay(node);
        player2UI.UpdateNodeDisplay(node);
        UpdateNavigationOptions(node);
    }

    private void UpdateNavigationOptions(NavigationNode node)
    {
        player1UI.UpdateNavigationOptions(node, navigationButtonPrefab);
        player2UI.UpdateNavigationOptions(node, navigationButtonPrefab);
    }

    private void OnGameOver()
    {
        Debug.Log("Game Over - UI should display end screen");
    }

    public void DisplayMessage(string message, float duration = 3f)
    {
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

        TeardownInputActions();
    }
}
