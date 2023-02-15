using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SuffocatingStatusUIController : MonoBehaviour
{

    public TextMeshProUGUI textMesh;
    private bool suffocatingNow = false;
    void Start()
    {
        this.suffocatingNow = false;
        this.textMesh = this.GetComponent<TextMeshProUGUI>();
    }

    void LateUpdate()
    {
        if (this.suffocatingNow)
        {
            this.textMesh.color = Color.red;
        }
        else
        {
            this.textMesh.color = Color.clear;
        }

        this.suffocatingNow = false;
    }

    public void SuffocatingThisFrame()
    {
        this.suffocatingNow = true;
    }
}
