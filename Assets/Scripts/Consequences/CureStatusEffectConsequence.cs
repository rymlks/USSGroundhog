#nullable enable
using Triggers;
using UnityEngine;

namespace Consequences
{
    public class CureStatusEffectConsequence : AbstractConsequence
    {
        public string statusEffectName = "burning";
        private PlayerHealthState _playerHealthStatus;

        void Start()
        {
            this._playerHealthStatus = FindObjectOfType<PlayerHealthState>();
        }

        public override void Execute(TriggerData? data)
        {
            _playerHealthStatus.Cure(statusEffectName);
        }
    }
}
