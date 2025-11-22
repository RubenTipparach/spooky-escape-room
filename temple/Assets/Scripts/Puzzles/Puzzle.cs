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

    public virtual void Initialize()
    {
        isSolved = false;
    }

    public virtual void SolvePuzzle()
    {
        if (isSolved) return;

        isSolved = true;
        OnPuzzleSolved?.Invoke();
    }

    public virtual void ResetPuzzle()
    {
        isSolved = false;
    }
}
