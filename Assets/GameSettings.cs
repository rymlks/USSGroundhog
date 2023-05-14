using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettings : MonoBehaviour
{
    public float MasterVolumePercentage { get; set; } = 100f;

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
}