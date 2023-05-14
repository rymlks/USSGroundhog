using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Audio
{
    public class MusicStack : MonoBehaviour
    {
        public AudioSource musicSource;
        public AudioClip musicStart;
        
        protected Stack<MusicTrackPlayData> stackedMusic;
        private float _lastPlayedTime;

        void Start()
        {
            stackedMusic = new Stack<MusicTrackPlayData>();
            stackedMusic.Push(new MusicTrackPlayData(musicStart, 0f, -1f));
            SetMusicVolume();
            PlayTopOfStack();
        }

        private void SetMusicVolume()
        {
            this.musicSource.volume = FindObjectOfType<GameSettings>().MasterVolumePercentage / 100f;
        }

        void Update()
        {
            checkForMusicExpiry();
        }

        void checkForMusicExpiry()
        {
             MusicTrackPlayData top = stackedMusic.Peek();
            if (!top.shouldPlayForever() && this._lastPlayedTime + top.timeIndexToPlayTo <= Time.time)
            {
                PopMusicFromStack();
            }
        }

        public void PushMusicToStack(AudioClip toAdd, float secondsToPlay)
        {
            this.musicSource.Pause();
            this.stackedMusic.Peek().currentTimeIndex = this.musicSource.time;
            this.stackedMusic.Push(new MusicTrackPlayData(toAdd, 0f, secondsToPlay));
            this.PlayTopOfStack();
        }

        protected void PlayTopOfStack()
        {
            MusicTrackPlayData clipAndTimeToPlay = this.stackedMusic.Peek();
            this.musicSource.clip = clipAndTimeToPlay.clip;
            this.musicSource.time = clipAndTimeToPlay.currentTimeIndex;
            this.musicSource.Play();
            this._lastPlayedTime = Time.time;
        }

        protected void PopMusicFromStack()
        {
            this.musicSource.Pause();
            this.stackedMusic.Pop();
            this.PlayTopOfStack();
        }
    }
}
