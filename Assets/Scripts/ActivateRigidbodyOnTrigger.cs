using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateRigidbodyOnTrigger : MonoBehaviour
{
    public bool destroy = false;
    public bool reverseOnExit = false;
    public Rigidbody rb;
    public Vector3 angularForce;

    public string CustomTag = "Player";
    public string RequireItem = "";

    public void OnTriggerEnter(Collider other)
    {
        if (CustomTag == "" || other.CompareTag(CustomTag))
        {
            if (RequireItem != "" && !GameManager.instance.itemIsPossessed(RequireItem))
            {
                return;
            }
            rb.isKinematic = false;
            rb.angularVelocity = angularForce;
            if (destroy)
            {
                Destroy(gameObject);
            }
        }
    }

    public void OnTriggerStay(Collider other)
    {
        OnTriggerEnter(other);
    }

    public void OnTriggerExit(Collider other)
    {
        if ((CustomTag == "" || other.CompareTag(CustomTag)) && reverseOnExit)
        {
            rb.isKinematic = true;
        }
    }
}
