using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class LevelLoadingMenu : MonoBehaviour
    {
        public void LoadLevel(string toLoadName)
        {
            SceneManager.LoadScene(toLoadName);
        }
    }
}
