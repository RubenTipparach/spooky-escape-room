using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class DoorUnlockedMessage : MonoBehaviour
{
    public TextMeshProUGUI messageText;
    public float MESSAGE_DURATION = 5f;
    public CanvasGroup canvasGroup;
    public float fadeInDuration = 0.2f;
    public float fadeOutDuration = 0.5f;

    public void ShowDoorMessage(string doorName)
    {
        gameObject.SetActive(true);
        Debug.Log($"Showing door unlocked message for {doorName}");
        StartCoroutine(DisplayMessageRoutine($"{doorName} Unlocked!"));
    }

    internal void ShowGenericMessage(string message)
    {
        gameObject.SetActive(true);
        Debug.Log($"Showing generic message: {message}");
        StartCoroutine(DisplayMessageRoutine(message));
    }

    private IEnumerator DisplayMessageRoutine(string message, 
        float messageDuration = 3f)
    {
        messageText.text = message;
        canvasGroup.alpha = 0f;

        // Fade in
        yield return StartCoroutine(FadeCanvasGroup(0f, 1f, fadeInDuration));

        // Stay visible
        yield return new WaitForSeconds(messageDuration - fadeInDuration - fadeOutDuration);

        // Fade out
        yield return StartCoroutine(FadeCanvasGroup(1f, 0f, fadeOutDuration));

        gameObject.SetActive(false);
    }

    private IEnumerator FadeCanvasGroup(float fromAlpha, float toAlpha, float duration)
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / duration;
            canvasGroup.alpha = Mathf.Lerp(fromAlpha, toAlpha, progress);
            yield return null;
        }

        canvasGroup.alpha = toAlpha;
    }
}
