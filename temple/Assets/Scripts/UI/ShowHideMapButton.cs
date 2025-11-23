using UnityEngine;

public class ShowHideMapButton : MonoBehaviour
{
    public GameObject mapPanel;

    public void ShowHideMap(bool show)
    {
        mapPanel.SetActive(show);

        if (show)
        {
            GameManager.Instance.uiManager.largeStickyNoteViewer.CloseStickyNote();
        }
    }
}
