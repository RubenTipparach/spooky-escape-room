using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.Playables;

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

    public NavigationNode startingNode;

    public UIManager uiManager;

    public NavigationNode special_TEMPLE_Node;

    public PlayableDirector basementCutsceneDirector;

    public PlayableDirector TempleCutsceneDirector;

    private bool basementCutscenePlayed = false;
    private bool bedroom2ScreamPlayed = false;

    public GameObject corpse;
    public AudioManager audioManager;

    public void TurnOnHud()
    {
        uiManager.playerUI.SetActive(true);
        // also map player to special node.
        // and enable temple nav button
        playerController.SetCurrentgNode(special_TEMPLE_Node);
        uiManager.secret_TEMPLE_NavButton.gameObject.SetActive(true);
        uiManager.navigationPanel.SetActive(false);

        uiManager.CheckActivatePuzzle();
        playerController.EndPlayerMover();
    }

    public void TurnOffHud()
    {
        uiManager.playerUI.SetActive(false);
        playerController.StartPlayerMover();

    }


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
        playerController.transform.rotation = Quaternion.Euler(0, 0, 0);
        playerController.SetCurrentgNode(startingNode);

        // Start random doorbell sounds
        //audioManager?.StartRandomDoorbell();
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
       playerController.SetCurrentgNode(navigationNode);
       uiManager.UpdateNavButtons();

       // Play basement cutscene on first entry
       if (navigationNode.NodeName == "Basement" && !basementCutscenePlayed && basementCutsceneDirector != null)
       {
           basementCutsceneDirector.Play();
           basementCutscenePlayed = true;
       }

       // Play scream on first entry to Bedroom 2
       if (navigationNode.NodeName == "Bedroom 2" && !bedroom2ScreamPlayed)
       {
           audioManager.PlayScream2();
           bedroom2ScreamPlayed = true;
       }
    }

    public void UnlockNode(NavigationNode bedroom2UnlockedNode)
    {
        bedroom2UnlockedNode.Unlock();
        uiManager.UpdateNavButtonsWithoutFacingChange();
    }

    internal void PlayFinalCutscene()
    {
        TempleCutsceneDirector.Play();
        uiManager.playerUI.SetActive(false);
        playerController.playerMover = playerController.playerMover2;
        playerController.StartPlayerMover();
    }

}
