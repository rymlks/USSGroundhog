using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettings : MonoBehaviour
{
    public static GameSettings instance {
        get
        {
            InitializeGameSettings();
            return _instance;
        }
        private set => _instance = value;
    }
    private static GameSettings _instance;
    
    public float MasterVolumePercentage { get; set; } = 100f;

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        InitializeGameSettings();
    }
    
    public static void InitializeGameSettings()
    {
        if (_instance == null)
        {
            _instance = FindObjectOfType<GameSettings>();
            if (_instance == null)
            {
                _instance = new GameObject("GameSettings").AddComponent<GameSettings>();
            }
        }
    }
}