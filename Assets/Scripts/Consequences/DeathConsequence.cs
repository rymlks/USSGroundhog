#nullable enable
using Triggers;
using UnityEngine;

namespace Consequences
{
    public class DeathConsequence : MonoBehaviour, IConsequence
    {
        public string deathReason;

        public void execute(TriggerData? data)
        {
            GameManager.instance.CommitDie(deathReason);
        }
    }
}
