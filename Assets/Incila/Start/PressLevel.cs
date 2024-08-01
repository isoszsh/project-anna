using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PressLevel : MonoBehaviour
{
    // Bu metod sahne ad�n� kullanarak sahne y�kler
    public void LoadLevelByName(string levelName)
    {
        SceneManager.LoadScene(levelName);
    }

    // Bu metod sahne indeksini kullanarak sahne y�kler
    public void LoadLevelByIndex(int levelIndex)
    {
        SceneManager.LoadScene(levelIndex);
    }
}
