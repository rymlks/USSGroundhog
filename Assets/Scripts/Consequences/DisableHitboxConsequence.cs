using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Triggers;
using Consequences;

public class DisableHitboxConsequence : AbstractConsequence
{
    public GameObject toDisable;
    void Start()
    {
        if (toDisable == null) {
            toDisable = gameObject;
        }
    }

    public override void Execute(TriggerData? data)
    {
        Collider[] colliders = toDisable.GetComponentsInChildren<Collider>();

        Debug.Log(colliders);

        foreach (Collider collider in colliders) {
            collider.enabled = false;
        }
    }
}
