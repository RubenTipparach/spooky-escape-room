using UnityEngine;

public class StickyNote : MonoBehaviour
{
    [TextArea(5, 10)]
    public string noteText;

    public NavigationNode associatedNode;
    public LocalFacingDirection associatedDirection;

    public void CheckShowStickyNote()
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

    public void OnClickStickyNote  ()
    {
        UIManager uiManager = GameManager.Instance.uiManager;
        uiManager.OpoenStickyNote(noteText);
    }
}
