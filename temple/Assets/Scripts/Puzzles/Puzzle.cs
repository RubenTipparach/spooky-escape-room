using UnityEngine;
using System;

/// <summary>
/// Base class for all puzzle types in the game
/// </summary>
public abstract class Puzzle : MonoBehaviour
{
    [SerializeField] protected string puzzleName = "Puzzle";
    [SerializeField] protected int keyReward = 0;

    protected bool isSolved = false;
    protected int attemptCount = 0;

    public event Action<int> OnPuzzleSolved; // Called with key reward amount
    public event Action OnPuzzleAttempt;

    public string PuzzleName => puzzleName;
    public bool IsSolved => isSolved;
    public int AttemptCount => attemptCount;
    public int KeyReward => keyReward;

    public virtual void Initialize()
    {
        isSolved = false;
        attemptCount = 0;
    }

    public virtual void AttemptPuzzle()
    {
        attemptCount++;
        OnPuzzleAttempt?.Invoke();
    }

    public virtual void SolvePuzzle()
    {
        if (isSolved) return;

        isSolved = true;
        Debug.Log($"Puzzle '{puzzleName}' solved! Reward: Key {keyReward}");
        OnPuzzleSolved?.Invoke(keyReward);
    }

    public virtual void ResetPuzzle()
    {
        isSolved = false;
        attemptCount = 0;
    }
}
