using UnityEngine;

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