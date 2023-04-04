using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideToDestinationConsequence : SlideToDestination, IConsequence
{
    public void execute()
    {
        this.start = true;
    }
}