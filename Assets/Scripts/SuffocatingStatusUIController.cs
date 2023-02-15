using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SuffocatingStatusUIController : MonoBehaviour
{

    public TextMeshProUGUI textMesh;
    private float lastSuffocationTime = -1;

    void Start()
    {
        this.textMesh = this.GetComponent<TextMeshProUGUI>();
    }

    bool suffocatingNow()
    {
        float timeSinceLastSuffocation = Time.time - this.lastSuffocationTime;
        return timeSinceLastSuffocation > 0 && timeSinceLastSuffocation < 1;
    }

    void LateUpdate()
    {
        if (this.suffocatingNow())
        {
            this.textMesh.color = Color.red;
        }
        else
        {
            if(this.textMesh)
                this.textMesh.color = Color.clear;
        }

    }

    public void SuffocatingThisFrame()
    {
        this.lastSuffocationTime = Time.time;
    }
}
