#nullable enable
using System;
using Consequences;
using Triggers;
using UnityEngine;

namespace Inventory
{
    public class GatherItemConsequence : AbstractConsequence
    {
        public String itemName;
        public bool persistsThroughDeath = false;

        public override void Execute(TriggerData? data)
        {
            GameManager.instance.getInventory().Gather(itemName, persistsThroughDeath);
        }
    }
}
