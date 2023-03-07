using System.Collections;
using System.Collections.Generic;
using KinematicCharacterController.Examples;
using UnityEngine;

public class HurtPlayerAndRemove : MonoBehaviour
{

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
            GameManager.instance.CommitDie("");
            Destroy(this.gameObject);
        }
    }
}
