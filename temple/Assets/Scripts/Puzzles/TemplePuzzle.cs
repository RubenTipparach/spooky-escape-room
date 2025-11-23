using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class TemplePuzzle : Puzzle
{
    public List<GameObject> stonePieces;
    public int[] correctOrder = { 3, 5, 10, 15 };
    public float xLocalStoneStart = 0.2703533f;
    public float xLocalStoneDown = 1f;
    public float stoneAnimationDuration = 0.3f;

    private bool[] stonePressed;
    private List<int> pressedSequence = new List<int>();
    private bool isResetting = false;

    void Start()
    {
        // Initialize stone state tracking
        stonePressed = new bool[stonePieces.Count];
    }

    void OnDisable()
    {
        // Reset puzzle if disabled while in progress
        if (!isSolved)
        {
            pressedSequence.Clear();
            for (int i = 0; i < stonePressed.Length; i++)
            {
                stonePressed[i] = false;
            }
            isResetting = false;
        }
    }

    public void OnStonePressed(int stoneId)
    {
        if (isSolved || isResetting)
            return;

        // Don't add if already pressed
        if (stonePressed[stoneId])
            return;

        stonePressed[stoneId] = true;
        pressedSequence.Add(stoneId);
        AnimateStoneDown(stoneId);
        GameManager.Instance.audioManager.PlayStoneButtonPressedSound();
        Debug.Log($"Stone {stoneId} pressed. Sequence: {string.Join(", ", pressedSequence)}");

        // Check if we've pressed 4 stones
        if (pressedSequence.Count == correctOrder.Length)
        {
            // Sort the pressed sequence and check if it matches correctOrder
            var sortedSequence = new List<int>(pressedSequence);
            sortedSequence.Sort();

            bool isValid = true;
            for (int i = 0; i < correctOrder.Length; i++)
            {
                if (sortedSequence[i] != correctOrder[i])
                {
                    isValid = false;
                    break;
                }
            }

            if (isValid)
            {
                Debug.Log("All stones pressed correctly! Puzzle solved!");
                SolvePuzzle();
                PlayFinalCutscene();
            }
            else
            {
                Debug.Log($"Wrong stones! Pressed: {string.Join(", ", sortedSequence)}, Expected: {string.Join(", ", correctOrder)}. Resetting puzzle.");
                StartCoroutine(ResetPuzzleSequence());
            }
        }
    }

    private IEnumerator ResetPuzzleSequence()
    {
        isResetting = true;

        // Brief pause before animating stones back up
        yield return new WaitForSeconds(0.3f);

        // Animate all pressed stones back up quickly
        foreach (int stoneId in pressedSequence)
        {
            AnimateStoneUp(stoneId);
        }

        // Wait for animations to complete
        yield return new WaitForSeconds(stoneAnimationDuration * 0.5f);

        // Reset all stone states
        foreach (int stoneId in pressedSequence)
        {
            stonePressed[stoneId] = false;
        }

        pressedSequence.Clear();
        isResetting = false;
    }

    private void AnimateStoneDown(int stoneId)
    {
        StartCoroutine(AnimateStone(stonePieces[stoneId], xLocalStoneDown, stoneAnimationDuration));
    }

    private void AnimateStoneUp(int stoneId)
    {
        StartCoroutine(AnimateStone(stonePieces[stoneId], xLocalStoneStart, stoneAnimationDuration * 0.5f));
    }

    private IEnumerator AnimateStone(GameObject stone, float targetX, float duration)
    {
        Transform stoneTransform = stone.GetComponent<Transform>();
        if (stoneTransform == null)
            yield break;

        Vector3 startPos = stoneTransform.localPosition;
        Vector3 endPos = startPos;
        endPos.x = targetX;

        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / duration;
            Vector3 currentPos = Vector3.Lerp(startPos, endPos, progress);
            stoneTransform.localPosition = currentPos;
            yield return null;
        }

        stoneTransform.localPosition = endPos;
    }

    public void PlayFinalCutscene()
    {
        GameManager.Instance.PlayFinalCutscene();
    }
}
