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

    public MyPlayer PlayerHandler;
    public GameObject playerPrefab;
    private HashSet<string> permanentItems = new HashSet<string>();
    private HashSet<string> transientItems = new HashSet<string>();

    private const Dictionary<string, object> defaultArgs = null;

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

        GameObject copy = Instantiate(playerPrefab);
        foreach(var rb in copy.GetComponentsInChildren<Rigidbody>())
        {
            rb.mass = 0.001f;
        }
        copy.transform.position = PlayerHandler.Character.transform.position;
        copy.transform.rotation = PlayerHandler.Character.transform.rotation;
        PlayerHandler.Character.gameObject.SetActive(false);

        copy.GetComponent<MyCharacterController>().CharacterAnimator.SetBool("DeadAsphyxiated", true);

        foreach (KeyValuePair<string, object> entry in args)
        {
            switch(entry.Key)
            {
                case "explosion":
                    copy.GetComponentInChildren<Rigidbody>().AddForce((Vector3)entry.Value);
                    break;
                default:
                    Debug.LogError("Unknown respawn arg: " + entry.Key);
                    break;
            }
        }


        yield return new WaitForSeconds(DeathSeconds);
        PlayerHandler.Character.gameObject.GetComponent<KinematicCharacterMotor>().SetPosition(new Vector3(-7.668f, 1.025f, 7.58f));
        PlayerHandler.Character.gameObject.SetActive(true);
        Debug.Log("permanent item count gathered at death: " + this.permanentItems.Count);
        DisplaySet(this.permanentItems);

    }

}
