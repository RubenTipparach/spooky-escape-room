using UnityEngine;
using System;
using System.Collections.Generic;

/// <summary>
/// Central game manager handling game state and key collection
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    // Game state
    private Dictionary<int, bool> keysCollected = new Dictionary<int, bool>();

    // Events
    public event Action<int> OnKeyCollected;

    public PlayerController playerController;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Initialize keys 1-4
        for (int i = 1; i <= 4; i++)
        {
            keysCollected[i] = false;
        }
    }

    public void CollectKey(int keyNumber)
    {
        if (keyNumber < 1 || keyNumber > 4)
        {
            Debug.LogWarning($"Invalid key number: {keyNumber}");
            return;
        }

        if (keysCollected[keyNumber])
        {
            Debug.Log($"Key {keyNumber} already collected");
            return;
        }

        keysCollected[keyNumber] = true;
        Debug.Log($"Key {keyNumber} collected! Total keys: {GetTotalKeysCollected()}");
        OnKeyCollected?.Invoke(keyNumber);
    }

    public bool HasKey(int keyNumber)
    {
        return keysCollected.ContainsKey(keyNumber) && keysCollected[keyNumber];
    }

    public int GetTotalKeysCollected()
    {
        int count = 0;
        foreach (var key in keysCollected.Values)
        {
            if (key) count++;
        }
        return count;
    }

    public void TraverseToNode(NavigationNode navigationNode)
    {
        throw new NotImplementedException();
    }
}
