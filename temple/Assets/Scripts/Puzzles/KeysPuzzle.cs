using System.Collections.Generic;
using UnityEngine;

public class KeysPuzzle : Puzzle
{
    public List<GameObject> keyHoleSlots; //3d models
    public int keysNeededToSolve = 3;
    public NavigationNode unlockedNode;

    private int keysInsertedCount = 0;

    void Start()
    {
        InitializeKeySlots();
    }

    private void InitializeKeySlots()
    {
        // Deactivate all keyhole slots initially
        foreach (GameObject slot in keyHoleSlots)
        {
            if (slot != null)
            {
                slot.SetActive(false);
            }
        }
    }

    public void CheckKeyInventory()
    {
        if (isSolved)
            return;
        var keyInventoryUI = GameManager.Instance.uiManager.keyInventoryUI;
        int keysObtained = keyInventoryUI.GetKeysObtained();

        Debug.Log($"CheckKeyInventory: keysObtained={keysObtained}, keysInsertedCount={keysInsertedCount}");

        if (keysObtained == 0 && keysInsertedCount == 0)
        {
            Debug.Log("No keys in inventory");
            var uiManager = GameManager.Instance.uiManager;
            uiManager.SetMessage("No keys in your inventory.");
        }
        else if (keysObtained > 0 && keysInsertedCount < keysNeededToSolve)
        {
            // Insert the next key into the puzzle
            int keyIndex = keysInsertedCount;
            if (keyIndex < keyHoleSlots.Count && keyHoleSlots[keyIndex] != null)
            {
                keyHoleSlots[keyIndex].SetActive(true);
                keyInventoryUI.RemoveKeyUI();
                keysInsertedCount++;
                Debug.Log($"Key added to puzzle! Total: {keysInsertedCount}/{keysNeededToSolve}");
            }
        }

        // Check if puzzle is solved
        if (keysInsertedCount >= keysNeededToSolve)
        {
            SolvePuzzleCheck();
        }
    }

    private void SolvePuzzleCheck()
    {
        Debug.Log("All keys have been added! Puzzle solved!");
        UnlockNode();
    }

    private void UnlockNode()
    {
        GameManager.Instance.UnlockNode(unlockedNode);
        GameManager.Instance.uiManager.SetDoorUnlockedMessage("Basement");
        SolvePuzzle();
    }
}
