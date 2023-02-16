using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    public float activationTimeSeconds = 0.01f;
    public String[] tagsToRecognize;
    private Dictionary<Collider, float> objectsWithinCollider = new Dictionary<Collider, float>();
    public GameObject affectedGameObject;

    bool tagMatches(Collider collider)
    {
        return tagsToRecognize.Any(collider.CompareTag);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (tagMatches(other) && !objectsWithinCollider.ContainsKey(other))
        {
            objectsWithinCollider.Add(other, 0);
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (tagMatches(other))
        {
            objectsWithinCollider[other] += Time.deltaTime;
        }

        if (objectsWithinCollider[other] > activationTimeSeconds)
        {
            DoConsequence();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (tagMatches(other))
        {
            objectsWithinCollider.Remove(other);
        }
    }

    void DoConsequence()
    {
        IConsequence consequence = this.GetComponent<IConsequence>();
        consequence?.execute();
    }
}
