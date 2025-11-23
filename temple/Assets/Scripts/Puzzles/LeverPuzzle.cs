using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverPuzzle : Puzzle
{
    public List<LeverController> levers;
    public int[] correctLeverOrder = { 1, 3, 2, 4 };
    public NavigationNode unlockedNode;

    private List<int> leversPulledOrder = new List<int>();
    private bool allLeversPulled = false;
    private bool resetInProgress = false;

    public void OnLeverButtonClicked(int leverId)
    {
        if (isSolved || resetInProgress)
            return;

        LeverController lever = GetLeverById(leverId);
        if (lever == null || lever.GetCurrentState() == LeverState.Down)
            return;

        lever.PullLeverDown();
        leversPulledOrder.Add(leverId);

        StartCoroutine(CheckAllLeversPulledAfterAnimation(lever));
    }

    private IEnumerator CheckAllLeversPulledAfterAnimation(LeverController lever)
    {
        while (lever.IsAnimating())
        {
            yield return null;
        }

        if (AreAllLeversPulled())
        {
            allLeversPulled = true;
            yield return new WaitForSeconds(0.5f);
            CheckLeverOrder();
        }
    }

    private bool AreAllLeversPulled()
    {
        foreach (LeverController lever in levers)
        {
            if (lever.GetCurrentState() != LeverState.Down)
                return false;
        }
        return true;
    }

    private void CheckLeverOrder()
    {
        if (IsCorrectOrder())
        {
            UnlockNode();
        }
        else
        {
            ResetAllLevers();
        }
    }

    private bool IsCorrectOrder()
    {
        if (leversPulledOrder.Count != correctLeverOrder.Length)
            return false;

        for (int i = 0; i < correctLeverOrder.Length; i++)
        {
            if (leversPulledOrder[i] != correctLeverOrder[i])
                return false;
        }
        return true;
    }

    private void ResetAllLevers()
    {
        StartCoroutine(ResetAllLeversRoutine());
    }

    private IEnumerator ResetAllLeversRoutine()
    {
        resetInProgress = true;

        foreach (LeverController lever in levers)
        {
            lever.PullLeverUp();
            yield return new WaitForSeconds(lever.animationDuration + 0.1f);
        }

        leversPulledOrder.Clear();
        allLeversPulled = false;
        resetInProgress = false;
    }

    private void UnlockNode()
    {
        GameManager.Instance.UnlockNode(unlockedNode);
        GameManager.Instance.uiManager.SetDoorUnlockedMessage("Bathroom");
        SolvePuzzle();
    }

    private LeverController GetLeverById(int leverId)
    {
        foreach (LeverController lever in levers)
        {
            if (lever.leverId == leverId)
                return lever;
        }
        return null;
    }
}
