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
        
        private float _pausedAtTime;
        private float _secondsToPause;
        protected Stack<Tuple<AudioClip, float>> stackedMusic;

        void Start()
        {
            stackedMusic = new Stack<Tuple<AudioClip, float>>();
            stackedMusic.Push(new Tuple<AudioClip, float>(musicStart, -1f));
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
                PopMusicFromStack();
            }
        }

        public void PushMusicToStack(AudioClip toAdd, float secondsToPlay)
        {
            this.musicSource.Pause();
            this.musicSource.clip = toAdd;
            this.musicSource.Play();
            this._pausedAtTime = Time.time;
            this._secondsToPause = secondsToPlay;

        }

        protected void PlayTopOfStack()
        {
            Tuple<AudioClip, float> clipAndTimeToPlay = this.stackedMusic.Peek();
            this.musicSource.clip = clipAndTimeToPlay.Item1;
            this.musicSource.time = clipAndTimeToPlay.Item2;
            this.musicSource.Play();
        }

        protected void PopMusicFromStack()
        {
            this.musicSource.Pause();
            this.stackedMusic.Pop();
            this.PlayTopOfStack();
        }

    }
}
