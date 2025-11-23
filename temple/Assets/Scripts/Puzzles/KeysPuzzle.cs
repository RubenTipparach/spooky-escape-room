using System.Collections.Generic;
using UnityEngine;

public class KeysPuzzle : Puzzle
{
    public List<GameObject> keyHoleSlots; //3d models
    //public List<Key> keys; // UI components
    public int keysNeededToSolve = 3;
    public NavigationNode unlockedNode;

    private List<int> usedKeyIds = new List<int>();
    private int keysAdded = 0;

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

    public void AddKey(int keyId)
    {
        if (isSolved)
            return;

        // Check if this key has already been used
        if (usedKeyIds.Contains(keyId))
        {
            Debug.Log($"Key {keyId} has already been used!");
            return;
        }

        // Deactivate the key from the UI list
        // if (keyId >= 0 && keyId < keys.Count && keys[keyId] != null)
        // {
        //     keys[keyId].gameObject.SetActive(false);
        // }

        // Activate the corresponding keyhole slot
        if (keyId >= 0 && keyId < keyHoleSlots.Count && keyHoleSlots[keyId] != null)
        {
            keyHoleSlots[keyId].SetActive(true);
        }

        usedKeyIds.Add(keyId);
        keysAdded++;

        Debug.Log($"Key {keyId} added. Keys added: {keysAdded}/{keysNeededToSolve}");

        // Call UIManager to remove key from inventory
        GameManager.Instance.uiManager.keyInventoryUI.RemoveKeyUI(keyId);

        // Check if puzzle is solved
        if (keysAdded >= keysNeededToSolve)
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
