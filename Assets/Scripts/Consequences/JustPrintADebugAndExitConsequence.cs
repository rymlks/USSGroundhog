#nullable enable
using System;
using System.Collections.Generic;
using Managers;
using Triggers;
using UnityEngine;

namespace Consequences
{
    public class JustPrintADebugAndExitConsequence : AbstractConsequence
    {
        public override void Execute(TriggerData? data)
        {
            Debug.Log(this.transform.rotation * new Vector3(0, 1, 0));
        }
    }
}