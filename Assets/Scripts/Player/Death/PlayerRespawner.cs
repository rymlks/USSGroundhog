using System.Collections;
using System.Collections.Generic;
using System.Linq;
using KinematicCharacterController;
using KinematicCharacterController.Walkthrough.RootMotionExample;
using Player.Death;
using UnityEngine;

public class PlayerRespawner : MonoBehaviour
{

    protected float deathSeconds = 3f;
    protected Vector3 respawnLocation;
    protected Transform respawnTransform;
    protected GameObject ragdollPrefab;
    protected GameObject playerPrefab;
    protected KinematicCharacterController.Walkthrough.RootMotionExample.MyPlayer player;
    protected CorpseCreator corpseCreator;
    protected CorpseAnimator corpseAnimator;

    public void Initialize(KinematicCharacterController.Walkthrough.RootMotionExample.MyPlayer player,
        GameObject playerPrefab, GameObject corpseRagdollPrefab, Vector3 startingRespawnLocation)
    {
        this.SetPlayerPrefab(playerPrefab);
        this.SetPlayerHandler(player);
        this.SetPlayerRagdollCorpsePrefab(corpseRagdollPrefab);
        this.SetRespawnLocation(startingRespawnLocation);
        if (!this.corpseCreator) this.corpseCreator = gameObject.AddComponent<CorpseCreator>();
        this.corpseAnimator = new CorpseAnimator();
    }

    public Vector3 getRespawnLocation()
    {
        if (respawnTransform != null)
        {
            return respawnTransform.position;
        }
        else return respawnLocation;
    }
    
    public void OnPlayerDeath(Dictionary<string, object> deathCharacteristics = null)
    {
        if (deathCharacteristics == null)
        {
            deathCharacteristics = new Dictionary<string, object>();
        }
        StartCoroutine(Respawn_Coroutine(deathCharacteristics));
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
        spawnAndAnimateCorpse(new DeathCharacteristics(args));
        player.Character.gameObject.SetActive(false);

        if (shouldRespawn(args))
        {
            yield return new WaitForSeconds(deathSeconds);
            player.Character.gameObject.GetComponent<KinematicCharacterMotor>().SetPosition(this.getRespawnLocation());
            resetPlayerHealthState();
            player.Character.gameObject.SetActive(true);
        }

    }

    private static bool shouldRespawn(Dictionary<string, object> args)
    {
        return !args.ContainsKey("dontRespawn") || !(bool) args["dontRespawn"];
    }

    private void spawnAndAnimateCorpse(DeathCharacteristics characteristics)
    {
        GameObject corpse = this.corpseCreator.CreateCorpse(characteristics, ragdollPrefab, playerPrefab, player.Character.transform);
        if (corpse != null)
        {
            this.corpseAnimator.AnimateCorpse(corpse, characteristics);
        }
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
