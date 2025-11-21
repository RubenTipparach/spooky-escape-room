using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public List<NavButton> navButtons;

    public Camera cameraForward;
    public Camera cameraLeft;
    public Camera cameraRight;
    public Camera cameraBack;

    void Awake()
    {
       UpdateNavButtons();
    }

    public void UpdateNavButtons()
    {
        foreach (NavButton button in navButtons)
        {
            button.SetCurrentButtonState();
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
