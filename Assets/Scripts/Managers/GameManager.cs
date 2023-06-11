using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KinematicCharacterController.Examples;
using KinematicCharacterController;
using Inventory;
using KinematicCharacterController.Walkthrough.RootMotionExample;
using Managers;
using Player;
using Player.Death;
using Unity.VisualScripting;
using UnityEngine.TextCore.Text;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public Animator CharacterAnimator;

    public MyPlayer PlayerHandler;
    public GameObject playerPrefab;
    public GameObject RagdollPrefab;
    public bool enableCheatyDevShortcuts = false;

    protected PlayerRespawner playerRespawner;
    protected PlayerInventory playerInventory;
    private static readonly int _isCrouching = Animator.StringToHash("IsCrouching");

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
        if (playerInventory == null)
        {
            this.playerInventory = this.AddComponent<PlayerInventory>();
        }

        if (CharacterAnimator == null)
        {
            CharacterAnimator = FindObjectOfType<FinalCharacterController>().CharacterAnimator;
        }

    }

    private void initializeRespawner()
    {
        this.playerRespawner = this.gameObject.AddComponent<PlayerRespawner>();
        playerRespawner.Initialize(PlayerHandler, playerPrefab, RagdollPrefab, PlayerHandler.Character.transform.position);

    }

    public void CommitDie(string reason)
    {
        this.CommitDie(new Dictionary<string, object>(){{reason, null}});
    }

    private Dictionary<string, object> AddStanceDetails(Dictionary<string, object> rawCharacteristics)
    {
        if (CharacterAnimator != null)
        {
            if (CharacterAnimator.GetBool(_isCrouching))
            {
                rawCharacteristics.Add("stance", "crouched");
            }
            else
            {
                rawCharacteristics.Add("stance", "standing");
            }
        }
        return rawCharacteristics;
    }

    public void CommitDie(Dictionary<string, object> rawCharacteristics)
    {
        this.CommitDie(new DeathCharacteristics(AddStanceDetails(rawCharacteristics)));
    }

    protected void CommitDie(DeathCharacteristics deathCharacteristics)
    {
        if (LevelScoreManager.instance != null)
        {
            LevelScoreManager.instance.RecordScoreEvent(deathCharacteristics);
        }
        this.playerRespawner.OnPlayerDeath(deathCharacteristics);
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
#if !UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            Application.Quit();
#endif
        }

    }

    public PlayerRespawner getRespawner()
    {
        return this.playerRespawner;
    }

    public PlayerInventory getInventory()
    {
        return this.playerInventory;
    }
}