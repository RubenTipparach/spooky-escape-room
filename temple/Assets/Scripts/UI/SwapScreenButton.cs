using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using UnityEngine.EventSystems;

public class SwapScreenButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    public RawImage currentScreen;
    public  UnityEngine.UI.Image borderImage;

    public UIManager uiManager;

    public Color normalBorderColor;
    public Color highlightedBorderColor;

    public LocalFacingDirection localFacingDirection;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
    public void Start()
    {
        uiManager = GameManager.Instance.uiManager;
        
    }


    public void Swap()
    {
        uiManager.mainScreen.texture = currentScreen.texture;
        GameManager.Instance.playerController.localFacingDirection = localFacingDirection;
        uiManager.SwapScreen(this);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        borderImage.color = highlightedBorderColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        borderImage.color = normalBorderColor;
    }
}

public enum LocalFacingDirection
{
    Forward,
    Left,
    Right,
    Back
}
