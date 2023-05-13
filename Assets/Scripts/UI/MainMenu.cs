using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class MainMenu : MonoBehaviour
    {
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
    }
}
