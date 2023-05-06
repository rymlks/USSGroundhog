using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;

public class ActivateRigidbodyOnTrigger : MonoBehaviour
{
    public bool destroy = false;
    public bool reverseOnExit = false;
    public Rigidbody rb;
    public Vector3 angularForce;

    public string CustomTag = "Player";
    public string RequireItem = "";
    private KeyStatusUIController keyUI;

    void Start()
    {
        this.keyUI = FindObjectOfType<KeyStatusUIController>();
    }

    public void OnTriggerEnter(Collider other)
    {
        if (CustomTag == "" || other.CompareTag(CustomTag))
        {
            if (RequireItem != "" && !GameManager.instance.itemIsPossessed(RequireItem))
            {
                keyUI.ShowNextFrame();
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
