#nullable enable
using Triggers;
using UnityEngine;

namespace Consequences
{
    public class InflictStatusEffectConsequence : AbstractConsequence
    {
        public string statusEffectName = "burning";
        private PlayerHealthState _playerHealthStatus;

        void Start()
        {
            this._playerHealthStatus = FindObjectOfType<PlayerHealthState>();
        }

        public override void execute(TriggerData? data)
        {
            _playerHealthStatus.Hurt(statusEffectName, Time.deltaTime);
        }
    }
}
