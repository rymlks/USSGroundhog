#nullable enable
using System.Collections;
using System.Collections.Generic;
using Consequences;
using Triggers;
using UnityEngine;

public class ToggleTriggerConsequence : MonoBehaviour, IConsequence
{

    public AbstractTrigger toToggle;
    
    void Start()
    {
        if (this.toToggle == null)
        {
            this.toToggle = GetComponent<AbstractTrigger>();
        }

    }

    public void execute(TriggerData? data)
    {
        this.toToggle.enabled = !this.toToggle.enabled;
    }
}
