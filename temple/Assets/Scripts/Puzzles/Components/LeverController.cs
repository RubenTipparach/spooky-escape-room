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

    void Start()
    {
        if (leverHandle != null)
        {
            leverHandle.transform.localRotation = Quaternion.Euler(0, upAngle, 0);
        }
    }

    public void PullLeverDown()
    {
        if (currentState == LeverState.Up && !isAnimating)
        {
            StartCoroutine(AnimateLever(upAngle, downAngle, LeverState.Down));
        }
    }

    public void PullLeverUp()
    {
        if (currentState == LeverState.Down && !isAnimating)
        {
            StartCoroutine(AnimateLever(downAngle, upAngle, LeverState.Up));
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

        currentState = newState;
        isAnimating = false;
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
