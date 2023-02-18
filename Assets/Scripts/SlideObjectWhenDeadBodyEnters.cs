using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideObjectWhenDeadBodyEnters: MonoBehaviour
{
    public bool destroy = false;
    public bool reverseOnExit = false;
    public SlideToDestination slideMe;

    public string CustomTag = "Corpse";
    public string RequireItem = "";

    public void OnTriggerEnter(Collider other)
    {
        if (CustomTag == "" || other.CompareTag(CustomTag))
        {
            if (RequireItem != "" && !GameManager.instance.itemIsPossessed(RequireItem))
            {
                return;
            }
            slideMe.start = true;
            slideMe.forward = true;
            if (destroy)
            {
                Destroy(gameObject);
            }
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if ((CustomTag == "" || other.CompareTag(CustomTag)) && reverseOnExit)
        {
            slideMe.start = true;
            slideMe.forward = false;
        }
    }
}
