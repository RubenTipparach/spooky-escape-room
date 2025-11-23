using System;
using System.Collections.Generic;
using System.Linq;
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

    public List<StickyNote> stickyNotes;

    public List<Key> keys;

    public NavButton curerentNavButton;

    public GameObject navigationPanel;

    public LargeStickyNoteViewer largeStickyNoteViewer;

    public KeyInventoryUI keyInventoryUI;

    public NavButton secret_TEMPLE_NavButton;

    public GameObject playerUI;

    public void SetDoorUnlockedMessage(string doorName)
    {
        doorUnlockedMessage.ShowDoorMessage(doorName);
    }

    public void SetMessage(string message)
    {
        doorUnlockedMessage.ShowGenericMessage(message);
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
    
    void Start()
    {

        stickyNotes = FindObjectsByType<StickyNote>(FindObjectsInactive.Include, FindObjectsSortMode.None).ToList();
        keys = FindObjectsByType<Key>(FindObjectsInactive.Include, FindObjectsSortMode.None).ToList();
        CheckShowStickyNotes();
        CheckShowKeys();
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
        largeStickyNoteViewer.CloseStickyNote();
    }

    public void UpdateNavButtons()
    {
        navigationPanel.SetActive(false);
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
        largeStickyNoteViewer.CloseStickyNote();
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
        largeStickyNoteViewer.CloseStickyNote();
    }

    private void UpdateTVVisibility(LocalFacingDirection localFacingDirection)
    {
        bool isLivingRoom =gameManager.playerController.currentNode == tvController.assoociatedNode;  
        bool isFacingForward = localFacingDirection == LocalFacingDirection.Forward;

        tvController.gameObject.SetActive(isLivingRoom && isFacingForward);
    }



    // Update is called once per frame
    void Update()
    {
        
    }


    public void CheckActivatePuzzle()
    {
        foreach (Puzzle puzzle in GameManager.Instance.uiManager.puzzles)
        {
            puzzle.CheckActivatePuzzle();
        }
        CheckShowStickyNotes();
        CheckShowKeys();
    }

    private void CheckShowStickyNotes()
    {
        foreach (StickyNote stickyNote in stickyNotes)
        {
            stickyNote.CheckShowStickyNote();
        }
    }

    private void CheckShowKeys()
    {
        foreach (Key key in keys)
        {
            key.CheckShowKey();
        }
    }

    public void OpoenStickyNote(string noteText)
    {
        largeStickyNoteViewer.OpoenStickyNote(noteText);
    }
}
