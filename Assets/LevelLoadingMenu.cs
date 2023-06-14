using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoadingMenu : MonoBehaviour
{
    public void LoadLevel(string toLoadName)
    {
        SceneManager.LoadScene(toLoadName);
    }
}
