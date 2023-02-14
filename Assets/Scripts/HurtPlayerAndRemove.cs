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
        Debug.Log("hitted me :(");
        if (isCollisionWithPlayer(triggeredCollider))
        {
            Debug.Log("Killing player via collider!");
            GameManager.instance.Respawn();
            Destroy(this.gameObject);
        }
    }
}
