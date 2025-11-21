using UnityEngine;

/// <summary>
/// Base class for interactable objects in the game
/// Provides common functionality for interactive elements
/// </summary>
public abstract class InteractableObject : MonoBehaviour, IInteractable
{
    [SerializeField] protected string objectName = "Object";
    [SerializeField] protected string interactionPromptPrimary = "Press Z/M to interact";
    [SerializeField] protected string interactionPromptSecondary = "Press X/N for secondary action";
    [SerializeField] protected bool showPromptWhenNear = true;

    protected bool canInteract = true;
    protected PlayerController lastPlayerNear;

    protected virtual void OnTriggerEnter(Collider collision)
    {
        PlayerController player = collision.GetComponent<PlayerController>();
        if (player != null && showPromptWhenNear)
        {
            lastPlayerNear = player;
            //GameUIManager.Instance?.UpdateInteractionPrompt(player.PlayerNumber, interactionPromptPrimary);
        }
    }

    protected virtual void OnTriggerExit(Collider collision)
    {
        PlayerController player = collision.GetComponent<PlayerController>();
        if (player != null && player == lastPlayerNear)
        {
            //GameUIManager.Instance?.ClearInteractionPrompt(player.PlayerNumber);
            lastPlayerNear = null;
        }
    }

    public virtual void OnInteract(PlayerController player)
    {
        if (!canInteract)
        {
            Debug.Log($"{objectName}: Cannot interact right now");
            return;
        }

        Debug.Log($"Player {player.PlayerNumber} interacted with {objectName}");
    }

    public virtual void OnSecondaryInteract(PlayerController player)
    {
        if (!canInteract)
        {
            Debug.Log($"{objectName}: Cannot perform secondary interaction right now");
            return;
        }

        Debug.Log($"Player {player.PlayerNumber} secondary interacted with {objectName}");
    }

    public virtual string GetInteractionPrompt()
    {
        return interactionPromptPrimary;
    }

    public void SetCanInteract(bool canInteract)
    {
        this.canInteract = canInteract;
    }

    public bool CanInteract => canInteract;
}
