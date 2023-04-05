using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathConsequence : MonoBehaviour, IConsequence
{
    public string deathReason;

    public void execute()
    {
        GameManager.instance.CommitDie(deathReason);
    }
}
