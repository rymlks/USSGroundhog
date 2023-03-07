using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KinematicCharacterController.Examples;
using KinematicCharacterController;
using Assets.Scripts;
using KinematicCharacterController.Walkthrough.RootMotionExample;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public Animator CharacterAnimator;

    public MyPlayer PlayerHandler;
    public GameObject playerPrefab;
    public GameObject RagdollPrefab;
    public bool enableCheatyDevShortcuts = false;
    
    private HashSet<string> permanentItems = new HashSet<string>();
    private HashSet<string> transientItems = new HashSet<string>();
    
    protected PlayerRespawner playerRespawner;

    private void OnEnable()
    {
        if (instance == null)
        {
            instance = this;
        } else
        {
            Destroy(this.gameObject);
        }
        
        if (PlayerHandler == null)
        {
            PlayerHandler = FindObjectOfType<MyPlayer>();
        }

        if (playerRespawner == null)
        {
            initializeRespawner();
        }

    }

    private void initializeRespawner()
    {
        this.playerRespawner = this.gameObject.AddComponent<PlayerRespawner>();
        playerRespawner.Initialize(PlayerHandler, playerPrefab, RagdollPrefab, PlayerHandler.Character.transform.position);

    }

    public void CommitDie(string reason)
    {
        this.playerRespawner.Respawn(new Dictionary<string, object>(){{reason, null}});

    }
    
    public void CommitDie(Dictionary<string, object> reason)
    {
        this.playerRespawner.Respawn(reason);

    }

    private bool devToolsEnabled()
    {
        return this.enableCheatyDevShortcuts;
    }

    public void Update()
    {
        if (devToolsEnabled())
        {
            if (Input.GetKeyDown(KeyCode.K))
            {
                CommitDie("Keyboard shortcut");
            }

            if (Input.GetKeyDown(KeyCode.P))
            {
                CommitDie("electrocution");

            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
            Application.Quit();
        }

    }

    public void Gather(string gatheredItemName, bool persistsThroughDeath = false)
    {
        if (persistsThroughDeath)
        {
            this.permanentItems.Add(gatheredItemName);
        }
        else
        {
            this.transientItems.Add(gatheredItemName);
        }
    }

    public bool itemIsPossessed(string itemName)
    {
        return this.permanentItems.Contains(itemName) || this.transientItems.Contains(itemName);
    }

    public PlayerRespawner getRespawner()
    {
        return this.playerRespawner;
    }
}
