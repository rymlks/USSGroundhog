using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoASpinny : MonoBehaviour
{
    public float speed;
    public float max = float.PositiveInfinity;

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
    }


    void FixedUpdate()
    {
        rotato += speed;
        if (Mathf.Abs(rotato) >= Mathf.Abs(max))
        {
            rotato = max;
        }
        transform.localRotation = Quaternion.Euler(0, rotato, 0);
    }
}
