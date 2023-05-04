using UnityEngine;

namespace Audio
{
    public class MusicTrackPlayData
    {
        public AudioClip clip;
        public float currentTimeIndex = 0;
        public float timeIndexToPlayTo = -1;

        public MusicTrackPlayData(AudioClip music, float timeIndex, float timeToPlay)
        {
            this.clip = music;
            this.currentTimeIndex = timeIndex;
            this.timeIndexToPlayTo = timeToPlay;
        }

        public bool shouldPlayForever()
        {
            return timeIndexToPlayTo < 0;
        }
    }
}
