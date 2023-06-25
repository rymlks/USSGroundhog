using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Triggers;
using Managers;

namespace Consequences
{
    public class ToggleClimbingConsequence : AbstractConsequence
    {
        public override void Execute(TriggerData? data)
        {
            GameManager.instance.PlayerHandler.ToggleClimbing(gameObject);
        }
    }
}
