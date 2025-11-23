using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class KeyInventoryUI : MonoBehaviour
{
    public List<GameObject> keyUIElements;
    public int keysObtained = 0;

    public void RemoveKeyUI()
    {
        if (keysObtained > 0)
        {
            keysObtained--;
        }
        UpdateKeyDisplay();
    }

    public void AddKeyUI()
    {
        if (keysObtained < keyUIElements.Count)
        {
            keysObtained++;
        }
        UpdateKeyDisplay();
    }

    private void UpdateKeyDisplay()
    {
        for (int i = 0; i < keyUIElements.Count; i++)
        {
            keyUIElements[i].SetActive(i < keysObtained);
        }
    }

    public int GetKeysObtained()
    {
        return keysObtained;
    }
}
