#nullable enable
using System.Collections;
using System.Collections.Generic;
using Consequences;
using Triggers;
using UnityEngine;

public class ToggleComponentsConsequence : MonoBehaviour, IConsequence
{
    public GameObject componentRoot;
    public string componentType = "light";
    public bool inChildren = true;

    public void Start()
    {
        if (this.componentRoot == null)
        {
            componentRoot = this.gameObject;
        }
    }

    public void execute(TriggerData? data)
    {
        if (componentType == "light" && inChildren)
        {
            foreach (Light light in this.GetComponentsInChildren<Light>(true))
            {
                light.enabled = !light.enabled;
            }
        }
    }
}
