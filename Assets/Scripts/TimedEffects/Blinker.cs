using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Blinker : MonoBehaviour
{
    private float nextSwitchTime;

    public float minSecondsBetweenSwitching = 2;
    public float maxSecondsBetweenSwitching = 4;
    private List<Transform> controlledObjects;

    void Start()
    {
        this.controlledObjects = this.gameObject.GetComponentsInChildren<Transform>().ToList();
        this.controlledObjects.Remove(this.transform);
        generateNextSwitchTime();
    }

    void Update()
    {
        if (Time.time >= nextSwitchTime)
        {
            doSwitch();
            generateNextSwitchTime();
        }
    }

    private void doSwitch()
    {
        foreach (Transform switchableTransform in this.controlledObjects)
        {
            switchableTransform.gameObject.SetActive(!switchableTransform.gameObject.activeSelf);
        }
    }

    private void generateNextSwitchTime()
    {
        this.nextSwitchTime = Time.time + UnityEngine.Random.Range(minSecondsBetweenSwitching, maxSecondsBetweenSwitching);
    }
}
