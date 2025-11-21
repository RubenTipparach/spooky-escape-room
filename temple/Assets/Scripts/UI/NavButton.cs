using UnityEngine;

public class NavButton : MonoBehaviour
{

    public GameObject here_icon;
    public GameObject goto_icon;
    public GameObject locked_icon;

    public NavigationNode navigationNode;
    public NavState navState;

    public void SetCurrentButtonState()
    {
        
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GoToNavNode()
    {
        if(navState == NavState.GoTo)
        {
            GameManager.Instance.TraverseToNode(navigationNode);
            navState = NavState.Here;
        }
    }
}

public enum NavState
{
    Here,
    GoTo,
    Locked
}