using System;
using System.Collections;
using System.Collections.Generic;
using Triggers;
using UnityEngine;

public class TriggerOnFixedUpdate : AbstractTrigger
{
    private void FixedUpdate()
    {
        this.Engage();
    }
}
