using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoASpinny : MonoBehaviour
{
    public float speed;
    public float max = float.PositiveInfinity;
    public Vector3 axis = Vector3.up;

    protected Quaternion initialRotation;
    
    private float rotato = 0;

    void Start()
    {
        if (max <= 0)
        {
            max = float.PositiveInfinity;
        }
        if(speed == 0){
            speed = 1;
        }

        this.initialRotation = this.transform.localRotation;
    }


    void FixedUpdate()
    {
        rotato += speed;
        if (Mathf.Abs(rotato) >= Mathf.Abs(max))
        {
            rotato = max;
        }
        transform.localRotation = initialRotation * Quaternion.Euler(rotato * axis.x, rotato * axis.y, rotato * axis.z);
    }
}
