using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grabber : MonoBehaviour
{
    public string tagToGrab;
    protected Transform hooked = null;
    
    public void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag(tagToGrab))
        {
            other.transform.SetParent(this.transform);
        }

        if (Input.GetKeyUp(KeyCode.End))
        {
            ReleaseHookedObject();
        }
    }

    public void ReleaseHookedObject()
    {
        if (hooked != null)
        {
            this.hooked.transform.SetParent(null);
            this.hooked = null;
        }
    }
}
