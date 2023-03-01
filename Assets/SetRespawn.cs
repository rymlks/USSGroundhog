using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetRespawn : MonoBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GameManager.instance.getRespawner().SetRespawnLocation(transform.position);
        }
    }
}
