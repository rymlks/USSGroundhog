using Managers;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.AssetDatabase;

namespace Audio
{
    public class SoundEffectPlayer : MonoBehaviour
    {
        public AudioClip Small_explosion, Tank_explosion, 
            Crate_explosion, Door_Slide, 
            PA_Jingle, Turret_Firing, 
            Turret_Firing_Tail, Character_Speaking, Glass_Breaking;


        protected AudioSource source;

        private const string smallExplosion = "...Assets/Sounds/SoundFX/Small Explosion 11.wav";

        void Start()
        {
            if (Small_explosion == null)
                Small_explosion = LoadAssetAtPath<AudioClip>(smallExplosion);
            if (this.source == null)
            {
                this.source = GetComponentInChildren<AudioSource>();
            }

            if (this.source == null)
            {
                this.source = this.AddComponent<AudioSource>();
            }

            this.SetSFXVolume();
        
        }

        public void PlaySound(AudioClip toPlay, bool loop = false)
        {
            source.clip = toPlay;
            source.loop = loop;
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

        public void Stop()
        {
            source.Stop();
        }

        public void SetSFXVolume()
        {
            this.source.volume = GameSettings.instance.SFXVolumePercentage / 100f;
        }
    }
}
