using UnityEngine;
using System.Collections.Generic;

public class PianoPuzzle : Puzzle
{

    private readonly int[] noteSequence = new int[] {3, 6, 10, 11};
    private const int NOTE_BUFFER_SIZE = 8;

    private readonly List<int> noteBuffer = new();

    public NavigationNode bedroom2UnlockedNode;

    private void UnlockNode()
    {
        GameManager.Instance.UnlockNode(bedroom2UnlockedNode);
        GameManager.Instance.uiManager.SetDoorUnlockedMessage("Bedroom 1");
        SolvePuzzle();
    }

    public void PlayNote(int noteIndex)
    {
        // Add note to buffer
        noteBuffer.Add(noteIndex);

        // Debug log with timestamp
        Debug.Log($"[{System.DateTime.Now:HH:mm:ss.fff}] Note played: {noteIndex} | Buffer: {string.Join(", ", noteBuffer)}");

        // Keep buffer size at max 8 notes
        if (noteBuffer.Count > NOTE_BUFFER_SIZE)
        {
            noteBuffer.RemoveAt(0);
        }

        // Check if puzzle is solved
        CheckPuzzleSolved();
    }

    private void CheckPuzzleSolved()
    {
        // Need at least 4 notes to check
        if (noteBuffer.Count < noteSequence.Length)
            return;

        // Check if the last 4 notes match the sequence
        int startIndex = noteBuffer.Count - noteSequence.Length;
        bool matches = true;

        for (int i = 0; i < noteSequence.Length; i++)
        {
            if (noteBuffer[startIndex + i] != noteSequence[i])
            {
                matches = false;
                break;
            }
        }

        if (matches)
        {
            UnlockNode();
        }
    }
}
