using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PressLevel : MonoBehaviour
{
    // Bu metod sahne adýný kullanarak sahne yükler
    public void LoadLevelByName(string levelName)
    {
        SceneManager.LoadScene(levelName);
    }

    // Bu metod sahne indeksini kullanarak sahne yükler
    public void LoadLevelByIndex(int levelIndex)
    {
        SceneManager.LoadScene(levelIndex);
    }
}
