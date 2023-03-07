using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoASpinny : MonoBehaviour
{
    public float speed;

    private float rotato = 0;

    // Update is called once per frame
    void FixedUpdate()
    {
        rotato += speed;
        transform.rotation = Quaternion.Euler(0, rotato, 0);
    }
}
