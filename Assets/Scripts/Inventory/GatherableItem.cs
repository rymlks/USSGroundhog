using System;
using System.Collections;
using System.Collections.Generic;
using KinematicCharacterController.Examples;
using UnityEngine;

public class GatherableItem : MonoBehaviour
{
    public String itemName;
    public bool persistsThroughDeath = false;
    public bool destroyOnCollect = false;

    public void Start()
    {
        this.GetComponent<Collider>().isTrigger = true;
    }

    bool isCollisionWithPlayer(Collider triggeredCollider)
    {
        return triggeredCollider.gameObject.CompareTag("Player");
    }

    void OnTriggerEnter(Collider triggeredCollider)
    {
        if (isCollisionWithPlayer(triggeredCollider))
        {
            Debug.Log("Player gathered permanent item "+itemName+"!");
            GameManager.instance.Gather(itemName, persistsThroughDeath);
            if (destroyOnCollect)
            {
                Destroy(gameObject);
            }
        }
    }
}
