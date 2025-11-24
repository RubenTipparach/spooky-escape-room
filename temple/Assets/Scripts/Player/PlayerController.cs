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

    public Transform playerMover;
    public Transform playerMover2;

    bool movePlayer = false;

    public Camera mainCamera;


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

    public void StartPlayerMover()
    {
        movePlayer = true;
    }


    public void EndPlayerMover()
    {
        movePlayer = false;
    }

    void Update()
    {
        if(movePlayer)
        {
            transform.position = playerMover.position;
        }

        // Rotate camera based on facing direction + local facing direction
        if (mainCamera != null)
        {
            float baseRotation = GetBaseRotationAngle(FacingDirection);
            float localRotation = GetLocalRotationAngle(localFacingDirection);
            float totalRotation = baseRotation + localRotation;

            mainCamera.transform.rotation = Quaternion.Euler(0, totalRotation + 90, 0);
        }
    }

    private float GetBaseRotationAngle(FacingDirection facingDirection)
    {
        return facingDirection switch
        {
            FacingDirection.North => 0,
            FacingDirection.East => 90,
            FacingDirection.South => 180,
            FacingDirection.West => -90,
            _ => 0
        };
    }

    private float GetLocalRotationAngle(LocalFacingDirection localFacingDirection)
    {
        return localFacingDirection switch
        {
            LocalFacingDirection.Forward => 0,
            LocalFacingDirection.Left => -90,
            LocalFacingDirection.Right => 90,
            LocalFacingDirection.Back => 180,
            _ => 0
        };
    }
}