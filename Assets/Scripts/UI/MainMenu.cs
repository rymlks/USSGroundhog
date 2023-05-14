using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class MainMenu : MonoBehaviour
    {
        protected List<CinemachineVirtualCamera> allMenuCameras;
        private GameSettings gameSettings;

        void Start()
        {
            this.allMenuCameras = FindObjectsOfType<CinemachineVirtualCamera>().ToList();
            this.initializeGameSettings();
        }

        public void LoadFirstLevel()
        {
            SceneManager.LoadScene("Level 1");
        }

        public void ExitProgram()
        {
            Application.Quit(0);
#if UNITY_EDITOR
            EditorApplication.ExitPlaymode();
#endif
        }

        public void FocusCamera(CinemachineVirtualCamera toFocus)
        {
            this.allMenuCameras.ForEach(virtualCamera =>
            {
                if (virtualCamera == toFocus)
                {
                    setGreaterCameraPriority(virtualCamera);
                }
                else
                {
                    setLesserCameraPriority(virtualCamera);
                }
            });
        }

        public void setMasterVolume(float volume)
        {
            initializeGameSettings();
            this.gameSettings.MasterVolumePercentage = volume;
        }

        private void initializeGameSettings()
        {
            if (this.gameSettings == null)
            {
                this.gameSettings = FindObjectOfType<GameSettings>();
                if (this.gameSettings == null)
                {
                    this.gameSettings = new GameObject("GameSettings").AddComponent<GameSettings>();
                }
            }
        }

        private static void setGreaterCameraPriority(CinemachineVirtualCamera virtualCamera)
        {
            setCameraPriority(virtualCamera, 20);
        }

        private static void setLesserCameraPriority(CinemachineVirtualCamera virtualCamera)
        {
            setCameraPriority(virtualCamera, 10);
        }

        private static void setCameraPriority(CinemachineVirtualCamera virtualCamera, int priority)
        {
            virtualCamera.Priority = priority;
        }
    }
}