using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DoorUnlockedMessage : MonoBehaviour
{

    public TextMeshProUGUI messageText;
    public float MESSAGE_DURATION = 5f;

    public void ShowMessage(string doorName)
    {
        gameObject.SetActive(true);
        Debug.Log($"Showing door unlocked message for {doorName}");
        StartCoroutine(DisplayMessageRoutine(doorName));
    }

    private IEnumerator DisplayMessageRoutine(string doorName)
    {
        messageText.text = $"{doorName} Unlocked!";

        yield return new WaitForSeconds(MESSAGE_DURATION);

        gameObject.SetActive(false);
    }
}
