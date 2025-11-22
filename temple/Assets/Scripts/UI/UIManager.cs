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

    // public SwapScreenButton swapScreenButtonForward;
    // public SwapScreenButton swapScreenButtonLeft;
    // public SwapScreenButton swapScreenButtonRight;
    // public SwapScreenButton swapScreenButtonBack;

    public NavButton curerentNavButton;

    void Awake()
    {
        gameManager = GameManager.Instance;
       UpdateNavButtons();
    }

    public void SwapScreen(SwapScreenButton swapButton)
    {
        var currentNavFacing = gameManager.playerController.FacingDirection;
        var localFacing = swapButton.localFacingDirection;

        curerentNavButton.ChangeDirectionLocal(localFacing, currentNavFacing);
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
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
