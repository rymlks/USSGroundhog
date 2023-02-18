using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KinematicCharacterController.Examples;
using KinematicCharacterController;
using KinematicCharacterController.Walkthrough.RootMotionExample;

public class GameManager : MonoBehaviour
{
    public float DeathSeconds;

    public static GameManager instance;

    public Animator CharacterAnimator;

    public MyPlayer PlayerHandler;
    public GameObject playerPrefab;
    public GameObject RagdollPrefab;
    private HashSet<string> permanentItems = new HashSet<string>();
    private HashSet<string> transientItems = new HashSet<string>();

    private const Dictionary<string, object> defaultArgs = null;
    private Vector3 respawnLocation;

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

        respawnLocation = PlayerHandler.Character.transform.position;
    }

    public void CommitDie(string reason)
    {

        Debug.Log("Player has died by " + reason + "!  Respawning.");
        Dictionary<string,object> args = new Dictionary<string, object>();
        args.Add(reason, null);
        this.Respawn(args);

    }

    public void Update()
    {

        if (Input.GetKeyDown(KeyCode.K))
        {
            Respawn();
        }

        if (Input.GetKeyDown(KeyCode.P))
        {

            //playerPrefab.GetComponent<Animator>().SetBool("IsFallDead", true);

            Debug.Log("P Key Press Detected!");
            //CharacterAnimator.SetBool("IsFallDead", true);

            PlayerHandler.Character.gameObject.GetComponent<Animator>().SetBool("IsFallDead", true);

            Respawn(new Dictionary<string, object>() {
                {"suffocation", false},
            });

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

    public void Respawn(Dictionary<string, object> args = defaultArgs)
    {
        if (args == null)
        {
            args = new Dictionary<string, object>();
        }
        StartCoroutine(Respawn_Coroutine(args));
    }

    public IEnumerator Respawn_Coroutine(Dictionary<string, object> args)
    {

        void DisplaySet(HashSet<string> collection)
        {
            Debug.Log("{");
            foreach (string i in collection)
            {
                Debug.Log(i);
            }
            Debug.Log(" }");
        }

        GameObject copy;

        if (args.ContainsKey("ragdoll")) {
            copy = Instantiate(RagdollPrefab);

            foreach (var rb in copy.GetComponentsInChildren<Rigidbody>())
            {
                rb.mass = 0.001f;
            }
        } 
        else
        {
            copy = Instantiate(playerPrefab);
        }

        /*
        CapsuleCollider cap = copy.GetComponentInChildren<CapsuleCollider>();
        cap.direction = 2;
        cap.height = 0.1f;
        Vector3 center = cap.center;
        center.y = cap.radius;
        cap.center = center;
        cap.enabled = false;
        Destroy(cap);
        Rigidbody rb = copy.GetComponent<Rigidbody>();
        rb.useGravity = false;
        */
        copy.transform.position = PlayerHandler.Character.transform.position;
        copy.transform.rotation = PlayerHandler.Character.transform.rotation;
        copy.tag = "Corpse";

        PlayerHandler.Character.gameObject.SetActive(false);

        foreach (KeyValuePair<string, object> entry in args)
        {
            switch (entry.Key)
            {
                case "explosion":
                    copy.GetComponentInChildren<Rigidbody>().AddForce((Vector3)entry.Value);
                    break;
                case "suffocation":

                    Debug.Log(entry.Key + " death animation triggered!");
                    copy.GetComponentInChildren<Animator>().SetBool("IsFallDead", true);

                    break;
                case "freezing":
                    //Do nothing
                    break;
                case "ragdoll":
                    // Do nothing
                    break;
                case "burning":
                    Destroy(copy);
                    break;
                default:
                    Debug.LogError("Unknown respawn arg: " + entry.Key);
                    break;
            }
        }


        yield return new WaitForSeconds(DeathSeconds);
        PlayerHandler.Character.gameObject.GetComponent<KinematicCharacterMotor>().SetPosition(respawnLocation);
        PlayerHandler.Character.gameObject.SetActive(true);
        Debug.Log("permanent item count gathered at death: " + this.permanentItems.Count);
        DisplaySet(this.permanentItems);
        resetPlayerHealthState();

    }

    private void resetPlayerHealthState()
    {
        FindObjectOfType<PlayerHealthState>().ResetAllStatuses();
    }
}
