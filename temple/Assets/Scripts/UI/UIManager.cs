using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public List<NavButton> navButtons;

    public Camera cameraForward;
    public Camera cameraLeft;
    public Camera cameraRight;
    public Camera cameraBack;

    public GameManager gameManager;

    public RawImage mainScreen;
    public RawImage forwardScreen;

    public DoorUnlockedMessage doorUnlockedMessage;

    // public SwapScreenButton swapScreenButtonForward;
    // public SwapScreenButton swapScreenButtonLeft;
    // public SwapScreenButton swapScreenButtonRight;
    // public SwapScreenButton swapScreenButtonBack;

    public TVController tvController;

    public List<Puzzle> puzzles;

    public NavButton curerentNavButton;

    public void SetDoorUnlockedMessage(string doorName)
    {
        doorUnlockedMessage.ShowMessage(doorName);

    }

    void Awake()
    {
        gameManager = GameManager.Instance;
       UpdateNavButtons();

       foreach (Puzzle puzzle in puzzles)
       {
           puzzle.gameObject.SetActive(false);
       }
    }

    public void ActivatePuzzle(Puzzle actiivePuzzle)
    {
        foreach (Puzzle puzzle in puzzles)
        {
            if (puzzle == actiivePuzzle && !puzzle.IsSolved)
            {
                puzzle.gameObject.SetActive(true);
            }
            else 
            {
                puzzle.gameObject.SetActive(false);
            }
        }
    }

    public void SwapScreen(SwapScreenButton swapButton)
    {
        var currentNavFacing = gameManager.playerController.FacingDirection;
        var localFacing = swapButton.localFacingDirection;

        curerentNavButton.ChangeDirectionLocal(localFacing, currentNavFacing);
        UpdateTVVisibility(localFacing);
        CheckActivatePuzzle();
    }

    public void UpdateNavButtons()
    {

        mainScreen.texture = forwardScreen.texture;
        foreach (NavButton button in navButtons)
        {
            if (button.navigationNode == gameManager.playerController.currentNode)
            {
                button.SetCurrentButtonState(NavState.Here);
                curerentNavButton = button;
            }
            else if (!button.navigationNode.IsLocked)
            {
                button.SetCurrentButtonState(NavState.GoTo);
            }
            else
            {
                button.SetCurrentButtonState(NavState.Locked);
            }

        }
        gameManager.playerController.localFacingDirection = LocalFacingDirection.Forward;
        UpdateTVVisibility(gameManager.playerController.localFacingDirection);
        CheckActivatePuzzle();
    }

    public void UpdateNavButtonsWithoutFacingChange()
    {
        foreach (NavButton button in navButtons)
        {
            if (button.navigationNode == gameManager.playerController.currentNode)
            {
                button.SetCurrentButtonState(NavState.Here, true);
                curerentNavButton = button;
            }
            else if (!button.navigationNode.IsLocked)
            {
                button.SetCurrentButtonState(NavState.GoTo);
            }
            else
            {
                button.SetCurrentButtonState(NavState.Locked);
            }

        }
        //UpdateTVVisibility(gameManager.playerController.localFacingDirection);
        CheckActivatePuzzle();
    }

    private void UpdateTVVisibility(LocalFacingDirection localFacingDirection)
    {
        bool isLivingRoom =gameManager.playerController.currentNode == tvController.assoociatedNode;  
        bool isFacingForward = localFacingDirection == LocalFacingDirection.Forward;

        tvController.gameObject.SetActive(isLivingRoom && isFacingForward);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void CheckActivatePuzzle()
    {
        foreach (Puzzle puzzle in GameManager.Instance.uiManager.puzzles)
        {
            puzzle.CheckActivatePuzzle();
        }
    }
}
