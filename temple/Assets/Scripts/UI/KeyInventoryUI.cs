using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class KeyInventoryUI : MonoBehaviour
{
    public List<GameObject> keyUIElements;
    private int keysObtained = 0;

    public void RemoveKeyUI(int keyId)
    {
        keyUIElements[keyId].SetActive(false);
        keysObtained--;
    }

    public void AddKeyUI(int keyId)
    {
        keyUIElements[keyId].SetActive(true);
        keysObtained++;
    }

    public int GetKeysObtained()
    {
        return keysObtained;
    }
}
