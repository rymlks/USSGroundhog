using System.Collections.Generic;
using Analytics;
using Audio;
using Inventory;
using Player.Death;
using UI;
using UnityEngine;

namespace Managers
{
    public class ScoreManager : MonoBehaviour
    {
        public static ScoreManager instance;
        public ScoreScreenUIController uiController;
        public AudioClip scoreScreenAudio;
        public float secondsScorePersists = 40f;

        [Tooltip("The number of deaths considered ideal for this level assuming zero explicit mistakes.")]
        public int levelParDeaths = 5;

        protected Dictionary<float, DeathCharacteristics> allDeaths;

        private void OnEnable()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(this.gameObject);
            }

            if (this.uiController == null)
            {
                this.uiController = FindObjectOfType<ScoreScreenUIController>();
            }

            this.allDeaths = new Dictionary<float, DeathCharacteristics>();
        }

        public LevelScore getLevelScore()
        {
            return new LevelScore(levelParDeaths, Time.time, allDeaths, FindObjectOfType<PlayerInventory>().permanentItemsPosessed());
        }

        public void RecordScoreEvent(DeathCharacteristics death)
        {
            this.allDeaths.Add(Time.time, death);
            if (!death.shouldRespawn())
            {
                uiController.secondsMessagePersists = secondsScorePersists;
                FindObjectOfType<MusicStack>().PushMusicToStack(scoreScreenAudio, secondsScorePersists);
                uiController.ShowNextFrame();
            }
        }
    }
}