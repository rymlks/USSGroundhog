#nullable enable
using Triggers;
using UnityEngine;

namespace Consequences
{
    public class DamageOverTimeConsequence : AbstractConsequence
    {
        public float multiplier = 1f;
        public string damageType;
        private PlayerHealthState playerHealthStatus;

        void Start()
        {
            this.playerHealthStatus = FindObjectOfType<PlayerHealthState>();
        }

        public override void execute(TriggerData? data)
        {
            this.playerHealthStatus.Hurt(damageType, Time.deltaTime * multiplier);
        }
    }
}
