using System.Collections;
using System.Collections.Generic;
using System.Linq;
using KinematicCharacterController.Examples;
using UnityEngine;

public class PlayerKiller : MonoBehaviour
{
    public List<string> immunityGrantingItems;
    public string deathReason = "generic hazard";

    public void Start()
    {
        this.GetComponent<Collider>().isTrigger = true;
    }

    protected bool isCollisionWithPlayer(Collider triggeredCollider)
    {
        return triggeredCollider.gameObject.CompareTag("Player");
    }

    protected bool immunityIsGranted()
    {
        return immunityGrantingItems.Any(item => GameManager.instance.itemIsPossessed(item));
    }

    protected virtual void OnTriggerEnter(Collider triggeredCollider)
    {

        if (isCollisionWithPlayer(triggeredCollider))
        {
            if (!immunityIsGranted())
            {
                DoConsequences();
            }
        }
    }

    protected virtual void DoConsequences()
    {
        GameManager.instance.CommitDie(deathReason);
    }
}
