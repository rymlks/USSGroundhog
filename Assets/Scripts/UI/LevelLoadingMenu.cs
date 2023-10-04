using System;
using Managers;
using StaticUtils;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
    public class LevelLoadingMenu : MonoBehaviour
    {
        public String[] LevelNames = {"USS Groundhog", "Punxsutawney Station", "Level 3", "Level 4", "Level 5", "Level 6", "Level 7", "Level 8"};
        public String[] LevelFilenames = {"Level 1", "Level 2", "Level 3", "Level 4", "Level 5", "Level 6", "Level 7", "Level 8"};
        public GameObject locationMenuButtonsParent;

        public Color accessibleColor = new Color(80, 207, 255);
        public Color unknownColor = new Color(135, 46, 46);
        public Color passedColor = Color.green;
        public Color completedColor = Color.yellow;

        public void Start()
        {
            if (this.locationMenuButtonsParent == null)
            {
                this.locationMenuButtonsParent = GameObject.Find("LocationMenuButtons");
            }

            setupButtonsForState();
            EnableCursor();
        }

        private static void EnableCursor()
        {
            Cursor.lockState = CursorLockMode.Confined;
        }

        public void LoadLevel(string toLoadName)
        {
            SceneManager.LoadScene(toLoadName);
        }

        protected void setupButtonsForState()
        {
            Button[] buttonsToConfigure = locationMenuButtonsParent.GetComponentsInChildren<Button>();
            ProgressManager progress = ProgressManager.instance;

            for (int i = 0; i < buttonsToConfigure.Length; i++)
            {
                bool isAvailable = progress.isLevelAvailable(i);
                Color buttonColor = progress.areAllOptionalObjectivesCompleted(i) ? completedColor :
                    progress.isLevelCompleted(i) ? passedColor :
                    isAvailable ? accessibleColor : unknownColor;
                String buttonText = isAvailable && i < LevelNames.Length ? LevelNames[i] : "????";
                buttonsToConfigure[i].colors =
                    UnityUtil.getBlockWithHighlightColorChanged(buttonsToConfigure[i].colors, buttonColor);
                buttonsToConfigure[i].GetComponentInChildren<TextMeshProUGUI>().text = buttonText;
                buttonsToConfigure[i].onClick.RemoveAllListeners();
                if (!isAvailable) continue;
                setButtonListenerToLoadLevel(buttonsToConfigure[i], i);
            }
        }

        protected void setButtonListenerToLoadLevel(Button button, int levelToLoad)
        {
            button.onClick.AddListener(() => { LoadLevel(LevelFilenames[levelToLoad]); });
        }
    }
}