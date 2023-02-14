using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KinematicCharacterController.Examples;
using KinematicCharacterController;
using KinematicCharacterController.Walkthrough.RootMotionExample;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public MyPlayer PlayerHandler;
    public GameObject playerPrefab;
    private HashSet<string> permanentItems = new HashSet<string>();
    private HashSet<string> transientItems = new HashSet<string>();

    private void OnEnable()
    {
        if (instance == null)
        {
            instance = this;
        } else
        {
            Destroy(this.gameObject);
        }

        PlayerHandler = FindObjectOfType<MyPlayer>();
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            Respawn();
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

    public void Respawn()
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
        
        GameObject copy = Instantiate(playerPrefab);
        copy.transform.position = PlayerHandler.Character.transform.position;
        copy.transform.rotation = PlayerHandler.Character.transform.rotation;
        PlayerHandler.Character.gameObject.GetComponent<KinematicCharacterMotor>().SetPosition(new Vector3(-7.668f, 1.025f, 7.58f));
        Debug.Log("permanent item count gathered at death: " + this.permanentItems.Count);
        DisplaySet(this.permanentItems);
    }
}
