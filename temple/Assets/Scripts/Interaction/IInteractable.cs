using UnityEngine;

/// <summary>
/// Interface for objects that can be interacted with by players
/// </summary>
public interface IInteractable
{
    /// <summary>
    /// Called when player presses primary interaction button (Z for P1, M for P2)
    /// </summary>
    void OnInteract(PlayerController player);

    /// <summary>
    /// Called when player presses secondary interaction button (X for P1, N for P2)
    /// </summary>
    void OnSecondaryInteract(PlayerController player);

    /// <summary>
    /// Returns the interaction prompt text to display
    /// </summary>
    string GetInteractionPrompt();
}
