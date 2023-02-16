using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideToDestination: MonoBehaviour
{
    public Transform destination;
    public float speed = 0.1f;
    public bool start = false;
    public Transform objectToSlide = null;


    private void Start()
    {
        destination.parent = null;
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

    // Update is called once per frame
    void Update()
    {
        if (start)
        {
            if ((destination.position - getTransform().position).magnitude > speed)
            {
                getTransform().position += (destination.position - getTransform().position).normalized * speed;
            }
            else
            {
                getTransform().position = destination.position;
                Destroy(this);
            }
        }
    }
}
