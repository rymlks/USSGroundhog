#nullable enable
using System.Collections;
using System.Collections.Generic;
using Consequences;
using Triggers;
using UnityEngine;

public class SwapTransformsConsequence : AbstractCancelableConsequence
{
    public Transform toSwapStart;
    public Transform toSwapEnd;
    public bool swapParentsToo = false;
    protected TransformData original;

    public override void Execute(TriggerData? data)
    {
        performSwap();
    }

    protected void performSwap()
    {
        original = new TransformData(toSwapStart);
        new TransformData(toSwapEnd).ToTransform(toSwapStart);
        original.ToTransform(toSwapEnd);
        if (swapParentsToo)
        {
            swapParents();
        }
    }

    private void swapParents()
    {
        Transform temp = toSwapStart.parent;
        toSwapStart.SetParent(toSwapEnd.parent);
        toSwapEnd.SetParent(temp);
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