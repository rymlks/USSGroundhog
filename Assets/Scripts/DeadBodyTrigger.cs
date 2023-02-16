using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadBodyTrigger : MonoBehaviour
{
    public bool destroy = false;

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Corpse"))
        {
            Debug.Log("Corpse found");
            if (destroy)
            {
                Destroy(gameObject);
            }
        }
    }
}
