using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideToDestination: MonoBehaviour
{
    public Transform destination;
    public float speed = 0.1f;
    public bool start = false;

    private void Start()
    {
        destination.parent = null;
    }

    // Update is called once per frame
    void Update()
    {
        if (start)
        {
            if ((destination.position - transform.position).magnitude > speed)
            {
                transform.position += (destination.position - transform.position).normalized * speed;
            }
            else
            {
                transform.position = destination.position;
                Destroy(this);
            }
        }
    }
}
