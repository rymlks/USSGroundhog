using Managers;
using UnityEngine;
using static UnityEditor.AssetDatabase;

namespace Audio
{
    public class SoundEffectPlayer : MonoBehaviour
    {
        public AudioClip Small_explosion, Tank_explosion, 
            Crate_explosion, Door_Slide, 
            PA_Jingle, Turret_Firing, 
            Turret_Firing_Tail;


        protected AudioSource source;

        private const string smallExplosion = "...Assets/Sounds/SoundFX/Small Explosion 11.wav";

        void Awake()
        {
            this.SetSFXVolume();
        }

        void Start()
        {
            if (Small_explosion == null)
                Small_explosion = LoadAssetAtPath<AudioClip>(smallExplosion);
            if (this.source == null)
            {
                this.source = GetComponentInChildren<AudioSource>();
            }
            this.SetSFXVolume();
        
        }

        public void PlaySound(AudioClip toPlay)
        {
            source.clip = toPlay;
            source.Play();
        }

        public void PlayTurretStart()
        {
            source.clip = Turret_Firing;
            source.Play();
        }

        public void PlayTurretStop()
        {
            source.clip = Turret_Firing_Tail;
            source.Play();
        }

        public void SetSFXVolume()
        {
            this.source.volume = GameSettings.instance.SFXVolumePercentage / 100f;
        }
    }
}
