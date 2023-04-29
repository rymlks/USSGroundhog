using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Audio
{
    public class MusicStack : MonoBehaviour
    {
        public AudioSource musicSource;
        public AudioClip musicStart;
        private AudioClip _pausedTrack;
        private float _pausedAtTime;
        private float _secondsToPause;

        void Start()
        {
            musicSource.clip = musicStart;
            musicSource.Play();
            this._pausedAtTime = float.PositiveInfinity;
        }

        void Update()
        {
            checkForMusicExpiry();
        }

        void checkForMusicExpiry()
        {
            if (this._pausedAtTime + this._secondsToPause <= Time.time)
            {
                restoreMainMusic();
            }
        }

        public void AddMusicToStack(AudioClip toAdd, float secondsToPlay)
        {
            this.musicSource.Pause();
            this._pausedTrack = this.musicSource.clip;
            this.musicSource.clip = toAdd;
            this.musicSource.Play();
            this._pausedAtTime = Time.time;
            this._secondsToPause = secondsToPlay;

        }

        protected void restoreMainMusic()
        {
            this.musicSource.Pause();
            this.musicSource.clip = this._pausedTrack;
            this.musicSource.Play();
        }

    }
}
