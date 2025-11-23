using System.Collections;
using UnityEngine;

public enum LeverState
{
    Up,
    Down
}

public class LeverController : MonoBehaviour
{
    public int leverId;
    public GameObject leverHandle;
    public float upAngle = 45f;
    public float downAngle = -45f;
    public float animationDuration = 0.5f;

    private LeverState currentState = LeverState.Up;
    private bool isAnimating = false;
    private Coroutine currentAnimation = null;

    void Start()
    {
        if (leverHandle != null)
        {
            leverHandle.transform.localRotation = Quaternion.Euler(0, upAngle, 0);
        }
    }

    void OnDisable()
    {
        // Clean up animation if the gameobject is disabled
        if (isAnimating && currentAnimation != null)
        {
            StopCoroutine(currentAnimation);
            isAnimating = false;
        }
    }

    public void PullLeverDown()
    {
        if (currentState == LeverState.Up && !isAnimating)
        {
            GameManager.Instance.audioManager.PlayLeverPullSound();
            currentAnimation = StartCoroutine(AnimateLever(upAngle, downAngle, LeverState.Down));
        }
    }

    public void PullLeverUp()
    {
        if (currentState == LeverState.Down && !isAnimating)
        {
            GameManager.Instance.audioManager.PlayLeverPullSound();
            currentAnimation = StartCoroutine(AnimateLever(downAngle, upAngle, LeverState.Up));
        }
    }

    public void ToggleLever()
    {
        if (currentState == LeverState.Up)
        {
            PullLeverDown();
        }
        else
        {
            PullLeverUp();
        }
    }

    private IEnumerator AnimateLever(float fromAngle, float toAngle, LeverState newState)
    {
        isAnimating = true;
        currentState = newState;
        float elapsedTime = 0f;

        while (elapsedTime < animationDuration)
        {
            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / animationDuration;
            float currentAngle = Mathf.Lerp(fromAngle, toAngle, progress);

            if (leverHandle != null)
            {
                leverHandle.transform.localRotation = Quaternion.Euler(0, currentAngle, 0);
            }

            yield return null;
        }

        if (leverHandle != null)
        {
            leverHandle.transform.localRotation = Quaternion.Euler(0, toAngle, 0);
        }

        isAnimating = false;
        currentAnimation = null;
    }

    public LeverState GetCurrentState()
    {
        return currentState;
    }

    public bool IsAnimating()
    {
        return isAnimating;
    }
}
