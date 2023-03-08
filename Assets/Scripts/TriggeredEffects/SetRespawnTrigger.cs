using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetRespawnTrigger : MonoBehaviour
{
    public bool LocationTracksThisObject = false;

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if(!LocationTracksThisObject)
                GameManager.instance.getRespawner().SetRespawnLocation(transform.position);
            else
                GameManager.instance.getRespawner().SetRespawnLocation(transform);
        }
    }
}
