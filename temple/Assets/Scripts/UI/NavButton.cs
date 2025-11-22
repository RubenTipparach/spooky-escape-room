using UnityEngine;
using UnityEngine.SocialPlatforms;

public class NavButton : MonoBehaviour
{

    public GameObject here_icon;
    public GameObject goto_icon;
    public GameObject locked_icon;

    public NavigationNode navigationNode;
    public NavState navState;

    public void SetCurrentButtonState(NavState state ,bool changeDirection = true)
    {
        navState = state;
 
        switch (navState)
        {
            case NavState.Here:
                here_icon.SetActive(true);
                goto_icon.SetActive(false);
                locked_icon.SetActive(false);
                if (changeDirection)
                {
                    ChangeDirection();
                }
                break;
            case NavState.GoTo:
                here_icon.SetActive(false);
                goto_icon.SetActive(true);
                locked_icon.SetActive(false);
                break;
            case NavState.Locked:
                here_icon.SetActive(false);
                goto_icon.SetActive(false);
                locked_icon.SetActive(true);
                break;
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

    public void ChangeDirection()
    {

        if(navState == NavState.Here)
        {
            Debug.Log("Changing Direction of Here Icon");

            switch(GameManager.Instance.playerController.FacingDirection)
            {
                case FacingDirection.North:
                    here_icon.transform.rotation = Quaternion.Euler(0, 0, 0);
                    break;
                case FacingDirection.East:
                    here_icon.transform.rotation = Quaternion.Euler(0, 0, -90);
                    break;
                case FacingDirection.South: 
                    here_icon.transform.rotation = Quaternion.Euler(0, 0, 180);
                    break;
                case FacingDirection.West:
                    here_icon.transform.rotation = Quaternion.Euler(0, 0, 90);
                    break;
            }
        }
    }

    public void ChangeDirectionLocal(LocalFacingDirection localFacingDirection, FacingDirection currentFacingDirection)
    {
        if(navState == NavState.Here)
        {
            Debug.Log("Changing Direction of Local");

            float rotationAngle = GetLocalRotationAngle(localFacingDirection, currentFacingDirection);
            here_icon.transform.rotation = Quaternion.Euler(0, 0, rotationAngle);
        }
    }

    private float GetLocalRotationAngle(LocalFacingDirection localFacingDirection, FacingDirection currentFacingDirection)
    {
        // Base angles for each local direction (assuming Forward points up, which is 0 degrees)
        float localAngle = localFacingDirection switch
        {
            LocalFacingDirection.Forward => 0,
            LocalFacingDirection.Left => 90,
            LocalFacingDirection.Right => -90,
            LocalFacingDirection.Back => 180,
            _ => 0
        };

        // Add the current facing direction offset
        float facingAngle = currentFacingDirection switch
        {
            FacingDirection.North => 0,
            FacingDirection.East => -90,
            FacingDirection.South => 180,
            FacingDirection.West => 90,
            _ => 0
        };

        // Combine the angles and normalize to 0-360 range
        float finalAngle = localAngle + facingAngle;
        return finalAngle;
    }

    public void GoToNavNode()
    {
        if(navState == NavState.GoTo)
        {
            GameManager.Instance.TraverseToNode(navigationNode);
            navState = NavState.Here;
            switch(navigationNode.FacingDirection)
            {
                case FacingDirection.North:
                    here_icon.transform.rotation = Quaternion.Euler(0, 0, 0);
                    break;
                case FacingDirection.East:
                    here_icon.transform.rotation = Quaternion.Euler(0, 0, -90);
                    break;
                case FacingDirection.South: 
                    here_icon.transform.rotation = Quaternion.Euler(0, 0, 180);
                    break;
                case FacingDirection.West:
                    here_icon.transform.rotation = Quaternion.Euler(0, 0, 90);
                    break;
            }
        }
    }
}


public enum NavState
{
    Here,
    GoTo,
    Locked
}