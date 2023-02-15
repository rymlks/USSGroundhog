using System.Collections;
using System.Collections.Generic;
using KinematicCharacterController.Examples;
using UnityEngine;

public class Explode : MonoBehaviour
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
            Debug.Log("Killing player via collider!");
            GameManager.instance.Respawn(new Dictionary<string, object>() {
                {"explosion", (triggeredCollider.transform.position - transform.position).normalized * 10},
            });
        }
    }
}