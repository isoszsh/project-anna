using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class LevelLoader : MonoBehaviour
{
    public void LoadNextLevel()
    {
        // mevcut sahnenin indeksi
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        // bir sonraki sahne indeksi
        int nextSceneIndex = currentSceneIndex + 1;

        // eðer bir sonraki sahne mevcutsa o sahne
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            Debug.Log("Tüm seviyeler tamamlandý!");
        }
    }
}
