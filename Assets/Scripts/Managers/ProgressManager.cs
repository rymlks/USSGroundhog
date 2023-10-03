using System;
using System.Collections.Generic;
using System.Linq;
using Audio;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

namespace Managers
{
    public class ProgressManager : MonoBehaviour
    {
        protected Dictionary<string, Dictionary<string, bool>> objectivesAchieved = new Dictionary<string, Dictionary<string, bool>>();
        protected static string LEVEL_COMPLETED = "completed";

        public static ProgressManager instance
        {
            get { return _instance; }
            private set => _instance = value;
        }

        private static ProgressManager _instance;

        void Awake()
        {
            Initialize();
            DontDestroyOnLoad(instance.gameObject);
        }

        public static void Initialize()
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<ProgressManager>();
                if (_instance == null)
                {
                    _instance = new GameObject("ProgressManager").AddComponent<ProgressManager>();
                }
            }
        }

        public void completeLevel(string levelName, Dictionary<string, bool> optionalObjectives)
        {
            this.objectivesAchieved[levelName] = new Dictionary<string, bool>();
            this.objectivesAchieved[levelName].Add(LEVEL_COMPLETED, true);
            this.objectivesAchieved[levelName].AddRange(optionalObjectives);
        }

        public void completeLevel(string levelName)
        {
            completeLevel(levelName, new Dictionary<string, bool>());
        }

        public bool isLevelCompleted(string levelName)
        {
            try
            {
                return this.objectivesAchieved[levelName][LEVEL_COMPLETED];
            }
            catch
            {
                return false;
            }
        }

        public bool areAllOptionalObjectivesCompleted(string levelName)
        {
            try
            {
                return this.objectivesAchieved[levelName].All(item => item.Value);
            }
            catch
            {
                return false;
            }
        }
    }
}