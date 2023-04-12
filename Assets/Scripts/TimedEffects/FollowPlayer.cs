using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public GameObject player;
    public float speed;

    // Update is called once per frame
    void Update()
    {
        transform.position += ((player.transform.position + new Vector3(0, 0.75f, 0)) - transform.position).normalized * speed * Time.deltaTime;
    }
}
