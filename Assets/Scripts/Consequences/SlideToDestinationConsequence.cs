using System.Collections;
using System.Collections.Generic;
using Consequences;
using Triggers;
using UnityEngine;

public class SlideToDestinationConsequence : SlideToDestination, IConsequence
{
    public void execute(TriggerData? data)
    {
        this.start = true;
    }
}