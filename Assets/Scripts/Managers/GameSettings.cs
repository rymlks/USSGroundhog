using Audio;
using UnityEngine;

namespace Managers
{
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
    
        public float MasterVolumePercentage { get;
            protected set;
        } = 100f;

        void Awake()
        {
            InitializeGameSettings();
            DontDestroyOnLoad(instance.gameObject);
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

        public void SetMasterVolume(float volume)
        {
            this.MasterVolumePercentage = volume;
            FindObjectOfType<MusicStack>().SetMusicVolume();
        }
    }
}