using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthState : MonoBehaviour
{
    private float secondsToSuffocation;
    public float secondsAirCapacity;

    void Start()
    {
        Reset();
    }

    public void Reset()
    {
        secondsToSuffocation = secondsAirCapacity;
    }

    public void Suffocate(float deltaSeconds)
    {
        Debug.Log("Player suffocating! Time remaining: " + secondsToSuffocation);
        this.secondsToSuffocation = Mathf.Max(0f, this.secondsToSuffocation - deltaSeconds);
        if (secondsToSuffocation <= 0f)
        {
            GameManager.instance.CommitDie("suffocation");
        }
    }
}
