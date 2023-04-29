#nullable enable
using Triggers;
using UnityEngine;

namespace Consequences
{
    public class DeathConsequence : AbstractConsequence
    {
        public string deathReason;

        public override void execute(TriggerData? data)
        {
            GameManager.instance.CommitDie(deathReason);
        }
    }
}
