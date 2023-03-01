using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRespawner : MonoBehaviour
{
    protected Vector3 respawnLocation;
    protected Transform respawnTransform;

    public Vector3 getRespawnLocation()
    {
        if (respawnTransform != null)
        {
            return respawnTransform.position;
        }
        else return respawnLocation;
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
}
