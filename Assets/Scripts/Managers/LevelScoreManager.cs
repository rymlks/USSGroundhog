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

        [Tooltip("The number of deaths considered ideal for this level assuming zero explicit mistakes.")]
        public int levelParDeaths = 5;

        [Tooltip("How to adjust par deaths for easier difficulty")]
        public int breakForEasy = 2;

        [Tooltip("How to adjust par deaths for harder difficulty")]
        public int penaltyForHard = -1;

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

        public int getLevelParDeathsAtCurrentDifficulty()
        {
            return levelParDeaths + getParPenaltiesAndBonuses();
        }

        protected int getParPenaltiesAndBonuses()
        {
            if (GameSettings.instance.Difficulty == GameSettings.DIFFICULTIES[0])
            {
                return this.breakForEasy;
            }
            else if (GameSettings.instance.Difficulty == GameSettings.DIFFICULTIES[2])
            {
                return this.penaltyForHard;
            }
            return 0;
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