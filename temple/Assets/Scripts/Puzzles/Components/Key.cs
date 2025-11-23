using UnityEngine;

public class Key : MonoBehaviour
{
    public int keyId;
    public NavigationNode associatedNode;
    public LocalFacingDirection associatedDirection;

    public void CheckShowKey()
    {
        GameManager gameManager = GameManager.Instance;

        NavigationNode playerNode = gameManager.playerController.currentNode;

        if (playerNode == associatedNode
            && gameManager.playerController.localFacingDirection == associatedDirection)
        {
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    public void PickupKey()
    {
        // Add key to inventory UI
        GameManager.Instance.uiManager.keyInventoryUI.AddKeyUI(keyId);

        // Disable the key gameobject
        gameObject.SetActive(false);
    }
}
