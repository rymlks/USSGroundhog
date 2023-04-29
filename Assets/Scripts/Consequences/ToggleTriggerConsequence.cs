#nullable enable
using System.Collections;
using System.Collections.Generic;
using Consequences;
using Triggers;
using UnityEngine;

public class ToggleTriggerConsequence : AbstractConsequence
{

    public AbstractTrigger toToggle;
    
    void Start()
    {
        if (this.toToggle == null)
        {
            this.toToggle = GetComponent<AbstractTrigger>();
        }

    }

    public override void execute(TriggerData? data)
    {
        this.toToggle.enabled = !this.toToggle.enabled;
    }
}
