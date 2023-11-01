#nullable enable
using System.Collections;
using System.Collections.Generic;
using Consequences;
using StaticUtils;
using Triggers;
using UnityEngine;
using static StaticUtils.UnityUtil;

public class DestroyIntersectingObjectsWithTagConsequence : AbstractConsequence
{
    public string tagToDestroy = "Corpse";
    public Collider toDestroyWithin = null;
    void Start()
    {
        if (this.toDestroyWithin == null)
        {
            this.toDestroyWithin = this.gameObject.GetComponent<Collider>();
        }
    }

    public override void Execute(TriggerData? data)
    {
        if (data?.triggeringObject != null)
        {
            GameObject intersectingObject = data.triggeringObject.CompareTag(tagToDestroy) ? data.triggeringObject : FindParentWithTag(data.triggeringObject, tagToDestroy);
            if (intersectingObject != null)
            {
                Destroy(intersectingObject);
            }
        }

    }
}
