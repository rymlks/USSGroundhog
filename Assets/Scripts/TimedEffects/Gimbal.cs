using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gimbal : MonoBehaviour
{

    public Transform toRotate;
    public float angle;
    void Start()
    {
        if (toRotate == null)
        {
            toRotate = gameObject.transform;
        }
    }

    void Update()
    { 
        float sineWave = Mathf.Sin(Time.time);
        float clampedAngle = sineWave * angle;
        this.gameObject.transform.rotation = Quaternion.Euler(toRotate.eulerAngles.x, toRotate.eulerAngles.y, clampedAngle);
    }
}
