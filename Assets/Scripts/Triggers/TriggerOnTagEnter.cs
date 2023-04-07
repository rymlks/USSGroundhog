using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerOnTagEnter : AbstractTrigger
{
    public string CustomTag = "";
    public string RequireItem = "";
    private KeyStatusUIController keyUI;

    new void Start()
    {
        base.Start();
        if (keyUI == null)
        {
            keyUI = FindObjectOfType<KeyStatusUIController>();
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (CustomTag == "" || other.CompareTag(CustomTag))
        {
            if (RequireItem != "" && !GameManager.instance.itemIsPossessed(RequireItem))
            {
                this.keyUI.showStatusNextFrame();
                return;
            }

            Engage();
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
}
