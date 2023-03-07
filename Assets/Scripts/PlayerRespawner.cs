using System.Collections;
using System.Collections.Generic;
using KinematicCharacterController;
using KinematicCharacterController.Walkthrough.RootMotionExample;
using UnityEngine;

public class PlayerRespawner : MonoBehaviour
{

    protected float deathSeconds = 3f;
    protected Vector3 respawnLocation;
    protected Transform respawnTransform;
    protected GameObject ragdollPrefab;
    protected GameObject playerPrefab;
    protected KinematicCharacterController.Walkthrough.RootMotionExample.MyPlayer player;

    public void Initialize(KinematicCharacterController.Walkthrough.RootMotionExample.MyPlayer player,
        GameObject playerPrefab, GameObject corpseRagdollPrefab, Vector3 startingRespawnLocation)
    {
        this.SetPlayerPrefab(playerPrefab);
        this.SetPlayerHandler(player);
        this.SetPlayerRagdollCorpsePrefab(corpseRagdollPrefab);
        this.SetRespawnLocation(startingRespawnLocation);
    }

    public Vector3 getRespawnLocation()
    {
        if (respawnTransform != null)
        {
            return respawnTransform.position;
        }
        else return respawnLocation;
    }
    
    public void Respawn(Dictionary<string, object> args = null)
    {
        if (args == null)
        {
            args = new Dictionary<string, object>();
        }
        StartCoroutine(Respawn_Coroutine(args));
    }
    
    
    
    public void SetRespawnLocation(Vector3 coordinatesToSpawnAt)
    {
        this.respawnLocation = coordinatesToSpawnAt;
        this.respawnTransform = null;
    }

    public void SetRespawnLocation(Transform objectToSpawnOn)
    {
        this.respawnLocation = Vector3.negativeInfinity;
        this.respawnTransform = objectToSpawnOn;
    }
    
       public IEnumerator Respawn_Coroutine(Dictionary<string, object> args)
    {
        GameObject copy;

        if (args.ContainsKey("ragdoll")) {
            copy = Instantiate(ragdollPrefab);

            foreach (var rb in copy.GetComponentsInChildren<Rigidbody>())
            {
                rb.mass = 0.001f;
            }
        } 
        else
        {
            copy = Instantiate(playerPrefab);
        }
        
        copy.transform.position = player.Character.transform.position;
        copy.transform.rotation = player.Character.transform.rotation;
        copy.tag = "Corpse";

        player.Character.gameObject.SetActive(false);

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
                case "electrocution":

                    Debug.Log(entry.Key + " death animation triggered!");

                    copy.GetComponentInChildren<Animator>().SetBool("IsElectrocuted", true);
                    
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
                case "dontRespawn":
                    if ((bool)entry.Value == true)
                    {
                        yield break;
                    }
                    break;
                default:
                    copy.GetComponentInChildren<Animator>().SetBool("IsFallDead", true);
                    Debug.LogError("Unknown respawn arg: " + entry.Key);
                    break;
            }
        }

        yield return new WaitForSeconds(deathSeconds);
        player.Character.gameObject.GetComponent<KinematicCharacterMotor>().SetPosition(this.getRespawnLocation());
        player.Character.gameObject.SetActive(true);
        resetPlayerHealthState();

    }
    
        private void resetPlayerHealthState()
        {
            FindObjectOfType<PlayerHealthState>().ResetAllStatuses();
        }

        public void SetPlayerRagdollCorpsePrefab(GameObject ragdollPrefab)
        {
            this.ragdollPrefab = ragdollPrefab;
        }

        public void SetPlayerPrefab(GameObject playerPrefab)
        {
            this.playerPrefab = playerPrefab;
        }

        public void SetPlayerHandler(KinematicCharacterController.Walkthrough.RootMotionExample.MyPlayer playerHandler)
        {
            this.player = playerHandler;
        }
}
