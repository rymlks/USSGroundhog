#nullable enable
using Triggers;
using UnityEngine;

namespace Consequences
{
    public class SetLightColorConsequence : ToggleLightColorConsequence
    {
        public override void Execute(TriggerData? data)
        {
            if (!lightToToggle) return;

                this.lightToToggle.color = goingTo;
        }
    }
}