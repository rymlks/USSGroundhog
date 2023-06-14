#nullable enable
using System;
using Managers;
using Triggers;

namespace Consequences
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
