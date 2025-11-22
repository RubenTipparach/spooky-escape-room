using UnityEngine;
using UnityEngine.SocialPlatforms;

/// <summary>
/// Simple player representation
/// All interaction and navigation is handled through the UI menu system
/// </summary>
public class PlayerController : MonoBehaviour
{
    public NavigationNode currentNode;

    public FacingDirection FacingDirection { get; internal set; }

    public LocalFacingDirection localFacingDirection;

    public void SetCurrentgNode(NavigationNode navigationNode)
    {
        currentNode = navigationNode;
        transform.position = navigationNode.transform.position;
        FacingDirection = navigationNode.FacingDirection;
        
        switch(navigationNode.FacingDirection)
            {
                case FacingDirection.North:
                    transform.rotation = Quaternion.Euler(0, 0, 0);
                    break;
                case FacingDirection.East:
                    transform.rotation = Quaternion.Euler(0, 90, 0);
                    break;
                case FacingDirection.South: 
                    transform.rotation = Quaternion.Euler(0, 180, 0);
                    break;
                case FacingDirection.West:
                    transform.rotation = Quaternion.Euler(0, -90, 0);
                    break;
            }

        // TODO activate current puzzles.
    }
}