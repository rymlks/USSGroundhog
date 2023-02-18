using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideObjectWhenDeadBodyEnters: MonoBehaviour
{
    public bool destroy = false;
    public SlideToDestination slideMe;

    public string CustomTag = "Corpse";
    public string RequireItem = "";

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(CustomTag))
        {
            if (RequireItem != "" && !GameManager.instance.itemIsPossessed(RequireItem))
            {
                return;
            }
            slideMe.start = true;
            if (destroy)
            {
                Destroy(gameObject);
            }
        }
    }
}
