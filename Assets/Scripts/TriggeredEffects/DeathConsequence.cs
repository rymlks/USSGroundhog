using System.Collections;
using System.Collections.Generic;
using Consequences;
using Triggers;
using UnityEngine;

public class DeathConsequence : MonoBehaviour, IConsequence
{
    public string deathReason;

    public void execute(TriggerData? data)
    {
        GameManager.instance.CommitDie(deathReason);
    }
}
