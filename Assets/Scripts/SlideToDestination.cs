using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideToDestination: MonoBehaviour
{
    public Transform destination;
    public float speed = 0.1f;
    public bool start = false;
    public bool forward = true;
    public Transform objectToSlide = null;
    public Vector3 startPos;
    public bool shouldReturn = true;


    private void Start()
    {
        destination.parent = null;
        startPos = transform.position;
    }

    protected Transform getTransform()
    {
        if (this.objectToSlide)
        {
            return objectToSlide;
        }
        else
        {
            return this.transform;
        }
    }

    void Update()
    {
        if (start)
        {
            Vector3 dest = forward ? destination.position : startPos;
            if ((dest - getTransform().position).magnitude > speed)
            {
                getTransform().position += (dest - getTransform().position).normalized * speed * Time.deltaTime * 50;
            }
            else
            {
                getTransform().position = dest;
                if (shouldReturn)
                {
                    forward = !forward;
                }
            }
        }
    }
}
