using System.Linq;
using Audio;
using UnityEngine;

namespace Managers
{
    public class GameSettings : MonoBehaviour
    {

        public static string[] DIFFICULTIES =
        {
            "Intern", "Middle Manager", "Chief Diversity Officer"
        };
        public static GameSettings instance {
            get
            {
                InitializeGameSettings();
                return _instance;
            }
            private set => _instance = value;
        }
        private static GameSettings _instance;
        public float SFXVolumePercentage{ get;
            protected set;
        } = 100f;

        public float MusicVolumePercentage { get;
            protected set;
        } = 100f;
        
        public string Difficulty { get;
            protected set;
        } = "Middle Manager";

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

        public void SetMusicVolume(float volume)
        {
            this.MusicVolumePercentage = volume;
            FindObjectOfType<MusicStack>().SetMusicVolume();
        }
        
        public void SetSoundEffectsVolume(float volume)
        {
            this.SFXVolumePercentage = volume;
            foreach (SoundEffectPlayer soundEffectPlayer in FindObjectsOfType<SoundEffectPlayer>())
            {
                soundEffectPlayer.SetSFXVolume();
            }
        }
        
        public void SetDifficulty(string toSet)
        {
            if (DIFFICULTIES.Contains(toSet))
            {
                this.Difficulty = toSet;
            }
            else
            {
                Debug.Log("Cannot set difficulty; unknown difficulty " + toSet);
            }
        }
    }
}