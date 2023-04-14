#nullable enable
using Triggers;
using UnityEngine;

namespace Consequences
{
    public class CureStatusEffectConsequence : MonoBehaviour, IConsequence
    {
        public string statusEffectName = "burning";
        private PlayerHealthState _playerHealthStatus;

        void Start()
        {
            this._playerHealthStatus = FindObjectOfType<PlayerHealthState>();
        }

        public void execute(TriggerData? data)
        {
            _playerHealthStatus.Cure(statusEffectName);
        }
    }
}
