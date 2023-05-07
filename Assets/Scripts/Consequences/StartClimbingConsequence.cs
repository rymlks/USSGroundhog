using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Triggers;

namespace Consequences
{
    public class StartClimbingConsequence : AbstractConsequence
    {
        public override void execute(TriggerData? data)
        {
            Debug.Log($"Gonna climb {name}");
            GameManager.instance.PlayerHandler.StartClimbing(gameObject);
        }
    }
}
