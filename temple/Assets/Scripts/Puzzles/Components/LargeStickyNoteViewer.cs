using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LargeStickyNoteViewer : MonoBehaviour
{
    public string message;
    public TextMeshProUGUI textBox;

    private void SetMessage(string newMessage)
    {
        message = newMessage;
        if (textBox != null)
        {
            textBox.text = message;
        }
    }
    public void OpoenStickyNote(String newMessage)
    {
        SetMessage(newMessage);
        gameObject.SetActive(true);
    }
   
    public void CloseStickyNote()
    {
        gameObject.SetActive(false);
    }
}
