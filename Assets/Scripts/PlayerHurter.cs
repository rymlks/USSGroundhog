using System.Collections;
using System.Collections.Generic;
using System.Linq;
using KinematicCharacterController.Examples;
using UnityEngine;

public class PlayerHurter : MonoBehaviour
{
    public List<string> immunityGrantingItems;

    public void Start()
    {
        this.GetComponent<Collider>().isTrigger = true;
    }

    bool isCollisionWithPlayer(Collider triggeredCollider)
    {
        return triggeredCollider.gameObject.CompareTag("Player");
    }

    bool immunityIsGranted()
    {
        return immunityGrantingItems.Any(item => GameManager.instance.itemIsPossessed(item));
    }

    void OnTriggerEnter(Collider triggeredCollider)
    {
        if (isCollisionWithPlayer(triggeredCollider))
        {
            if(!immunityIsGranted()){}
            Debug.Log("Killing player via collider!");
            GameManager.instance.Respawn();
        }
    }
}
