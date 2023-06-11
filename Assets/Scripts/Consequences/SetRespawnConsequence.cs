using System.Collections;
using System.Collections.Generic;
using Consequences;
using Managers;
using Triggers;
using UnityEngine;

public class SetRespawnConsequence : AbstractConsequence
{
    public bool LocationTracksThisObject = false;

    public override void Execute(TriggerData? data)
    {
        if (!LocationTracksThisObject)
        {
            GameManager.instance.getRespawner().SetRespawnLocation(transform.position);
        }
        else
        {
            GameManager.instance.getRespawner().SetRespawnLocation(transform);
        }
    }
}