using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthState : MonoBehaviour
{
    private float secondsToSuffocation;
    public float secondsAirCapacity;
    //how fast you choke vs. recover from choking; should be >1
    public float chokeFactor = 2;
    
    void Start()
    {
        Reset();
    }

    void Update()
    {
        this.secondsToSuffocation = Mathf.Min(secondsAirCapacity, secondsToSuffocation + Time.deltaTime / chokeFactor);
    }

    public void Reset()
    {
        secondsToSuffocation = secondsAirCapacity;
    }

    public void Suffocate(float deltaSeconds)
    {
        Debug.Log("Player suffocating! Time remaining: " + secondsToSuffocation);
        FindObjectOfType<SuffocatingStatusUIController>().SuffocatingThisFrame();
        this.secondsToSuffocation = Mathf.Max(0f, this.secondsToSuffocation - deltaSeconds);
        if (secondsToSuffocation <= 0f)
        {
            GameManager.instance.CommitDie("suffocation");
        }
    }
}
