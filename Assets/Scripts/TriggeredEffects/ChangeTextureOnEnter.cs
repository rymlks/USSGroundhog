using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeTextureOnEnter : MonoBehaviour
{
    public Material newMat;
    public string CustomTag = "Player";

    public void OnTriggerEnter(Collider other)
    {
        if (CustomTag == "" || other.CompareTag(CustomTag))
        {
            GetComponentInChildren<SkinnedMeshRenderer>().material = newMat;
        }
    }
}
