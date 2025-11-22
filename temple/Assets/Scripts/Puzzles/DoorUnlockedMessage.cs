using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DoorUnlockedMessage : MonoBehaviour
{

    public TextMeshProUGUI messageText;
    private const float MESSAGE_DURATION = 10f;

    internal void ShowMessage(string doorName)
    {
        StartCoroutine(DisplayMessageRoutine(doorName));
    }

    private IEnumerator DisplayMessageRoutine(string doorName)
    {
        if (messageText != null)
        {
            messageText.text = $"{doorName} Unlocked";
            messageText.gameObject.SetActive(true);
        }

        yield return new WaitForSeconds(MESSAGE_DURATION);

        if (messageText != null)
        {
            messageText.gameObject.SetActive(false);
        }
    }
}
