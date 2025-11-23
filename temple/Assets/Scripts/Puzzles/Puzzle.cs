using UnityEngine;
using System;

/// <summary>
/// Base class for all puzzle types in the game
/// </summary>
public abstract class Puzzle : MonoBehaviour
{
    [SerializeField] protected string puzzleName = "Puzzle";

    protected bool isSolved = false;

    public event Action OnPuzzleSolved; // Called with key reward amount

    public string PuzzleName => puzzleName;
    public bool IsSolved => isSolved;

    public NavigationNode associatedNode;

    public LocalFacingDirection associatedDirection;

    public virtual void Initialize()
    {
        isSolved = false;
    }

    public virtual void SolvePuzzle()
    {
        if (isSolved) return;

        isSolved = true;
        GameManager.Instance.audioManager.PlayDingDongClockSound();
        OnPuzzleSolved?.Invoke();
        gameObject.SetActive(false);
    }

    public virtual void ResetPuzzle()
    {
        isSolved = false;
    }

    public void CheckActivatePuzzle()
    {
        GameManager gameManager = GameManager.Instance;

        NavigationNode playerNode = gameManager.playerController.currentNode;

        if (playerNode == associatedNode 
            && gameManager.playerController.localFacingDirection == associatedDirection
            && !isSolved)
        {
            gameObject.SetActive(true);
        }
        else{
            gameObject.SetActive(false);
        }

    }
}
