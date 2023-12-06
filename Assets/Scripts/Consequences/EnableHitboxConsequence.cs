using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Triggers;
using Consequences;

public class EnableHitboxConsequence : AbstractConsequence
{
    public GameObject toEnable;
    // Start is called before the first frame update
    void Start()
    {
        if (toEnable == null) {
            toEnable = gameObject;
        }
    }

    public override void Execute(TriggerData? data)
    {
        Collider[] colliders = toEnable.GetComponentsInChildren<Collider>();

        foreach (Collider collider in colliders) {
            collider.enabled = true;
        }
    }

}
