using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideObjectWhenDeadBodyEnters: MonoBehaviour
{
    public bool destroy = false;
    public SlideToDestination slideMe;

    public string CustomTag = "Corpse";

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(CustomTag))
        {
            slideMe.start = true;
            if (destroy)
            {
                Destroy(gameObject);
            }
        }
    }
}
