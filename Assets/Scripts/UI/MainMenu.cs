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

        void Start()
        {
            this.allMenuCameras = FindObjectsOfType<CinemachineVirtualCamera>().ToList();
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

        public void FocusMenu(string menuName)
        {
            CinemachineVirtualCamera found = GameObject.Find(menuName + "MenuVirtualCamera")
                .GetComponentInChildren<CinemachineVirtualCamera>();
            if (found == null)
            {
                Debug.Log("Cannot focus on menu " + menuName + "; menu camera not found");
            }
            else
            {
                this.allMenuCameras.ForEach(virtualCamera =>
                {
                    if (virtualCamera == found)
                    {
                        setGreaterCameraPriority(virtualCamera);
                    }
                    else
                    {
                        setLesserCameraPriority(virtualCamera);
                    }
                });
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