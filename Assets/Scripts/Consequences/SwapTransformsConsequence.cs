#nullable enable
using System.Collections;
using System.Collections.Generic;
using Consequences;
using Triggers;
using UnityEngine;

public class SwapTransformsConsequence : AbstractCancelableConsequence
{
    public Transform toSwap;
    public Transform toSwapWith;
    public bool swapParentsToo = false;
    protected TransformData original;

    public override void Execute(TriggerData? data)
    {
        performSwap();
    }

    protected void performSwap()
    {
        original = new TransformData(toSwap);
        new TransformData(toSwapWith).ToTransform(toSwap);
        original.ToTransform(toSwapWith);
        if (swapParentsToo)
        {
            swapParents();
        }
    }

    private void swapParents()
    {
        Transform temp = toSwap.parent;
        toSwap.SetParent(toSwapWith.parent);
        toSwapWith.SetParent(temp);
    }

    public override void Cancel(TriggerData? data)
    {
        performSwap();
    }
}

public class TransformData
{
    private Vector3 position;
    private Quaternion rotation;
    private Vector3 scale;

    public TransformData(Transform transform)
    {
        this.position = transform.position;
        this.rotation = transform.rotation;
        this.scale = transform.localScale;
    }

    public void ToTransform(Transform transform)
    {
        transform.position = position;
        transform.rotation = rotation;
        transform.localScale = scale;
    }
}