using System.Collections.Generic;
using Analytics;
using Audio;
using Inventory;
using Player.Death;
using UI;
using UnityEngine;

namespace Managers
{
    public class LevelScoreManager : MonoBehaviour
    {
        public static LevelScoreManager instance;
        public ScoreScreenUIController uiController;
        public AudioClip scoreScreenAudio;
        public float secondsScorePersists = 40f;

        [Tooltip("The number of deaths considered ideal for this level assuming zero explicit mistakes and perfect foreknowledge.")]
        public int levelMinDeaths = 3;

        
        [Tooltip("The 'par' number of deaths for this level on Normal difficulty.")]
        public int levelParDeathsNormal = 5;

        [Tooltip("How to adjust par deaths for easier difficulty")]
        public int levelParDeathsEasy = 8;

        [Tooltip("How to adjust par deaths for harder difficulty")]
        public int levelParDeathsHard = 3;

        [Tooltip("Which level should be reported as completed if the player wins")]
        public string levelName;
        
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

        protected int getLevelParDeathsAtCurrentDifficulty()
        {
            if (GameSettings.instance.Difficulty == GameSettings.DIFFICULTIES[0])
            {
                return this.levelParDeathsEasy;
            }
            else if (GameSettings.instance.Difficulty == GameSettings.DIFFICULTIES[2])
            {
                return this.levelParDeathsHard;
            }
            return this.levelParDeathsNormal;
        }

        public LevelScore getLevelScore()
        {
            return new LevelScore(this.getLevelParDeathsAtCurrentDifficulty(), Time.time, allDeaths, FindObjectOfType<PlayerInventory>().permanentItemsPosessed());
        }

        public void RecordScoreEvent(DeathCharacteristics death)
        {
            this.allDeaths.Add(Time.time, death);
            if (!death.shouldRespawn())
            {
                ProgressManager.instance.completeLevel(levelName);
                uiController.secondsMessagePersists = secondsScorePersists;
                FindObjectOfType<MusicStack>().PushMusicToStack(scoreScreenAudio, secondsScorePersists);
                uiController.ShowNextFrame();
            }
        }
    }
}