using UnityEngine;

/// <summary>
/// Represents a door that can be opened and closed
/// Opens to Z + 45 degrees, closes back to original rotation
/// </summary>
public class Door : InteractableObject
{
    private Quaternion initialRotation;
    private bool isOpen = false;

    private void Awake()
    {
        initialRotation = transform.rotation;
    }

    public override void OnInteract(PlayerController player)
    {
        base.OnInteract(player);

        if (!isOpen)
        {
            OpenDoor();
        }
    }

    public override void OnSecondaryInteract(PlayerController player)
    {
        base.OnSecondaryInteract(player);

        if (isOpen)
        {
            CloseDoor();
        }
    }

    public void OpenDoor()
    {
        if (isOpen) return;

        // Rotate 45 degrees around Z axis
        transform.rotation = initialRotation * Quaternion.Euler(0, 0, 45f);
        isOpen = true;
        Debug.Log($"Door {gameObject.name} opened");
    }

    public void CloseDoor()
    {
        if (!isOpen) return;

        // Return to initial rotation
        transform.rotation = initialRotation;
        isOpen = false;
        Debug.Log($"Door {gameObject.name} closed");
    }

    public bool IsOpen => isOpen;
}
