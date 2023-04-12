using System.Collections;
using System.Collections.Generic;
using Consequences;
using Triggers;
using UnityEngine;

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
