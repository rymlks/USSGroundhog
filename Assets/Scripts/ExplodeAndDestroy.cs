using System.Collections;
using System.Collections.Generic;
using KinematicCharacterController.Examples;
using UnityEngine;

public class ExplodeAndDestroy : MonoBehaviour
{
    public float explosionStrength = 10;
    public GameObject ExplosionPrefab;

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
            if (ExplosionPrefab != null)
            {
                var explode = Instantiate(ExplosionPrefab);
                explode.transform.position = transform.position;
                explode.transform.parent = null;
            }
            Debug.Log("Killing player via collider!");
            GameManager.instance.CommitDie(
                new Dictionary<string, object> {
                    {"explosion", (triggeredCollider.transform.position - transform.position).normalized * explosionStrength},
                    {"ragdoll", true},
                });
            Destroy(this.gameObject);
        }
    }
}
