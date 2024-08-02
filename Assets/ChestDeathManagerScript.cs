using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChestDeathManagerScript : MonoBehaviour
{
    public bool firstTime = true;
    public GameObject darkenPanel;

    public void LoadDeathScene()
    {
        StartCoroutine(LoadDeathSceneCoroutine());
    }

    IEnumerator LoadDeathSceneCoroutine()
    {
        darkenPanel.SetActive(true);
        darkenPanel.GetComponent<Animator>().SetTrigger("Darken");

        yield return new WaitForSeconds(2);
            
        if (firstTime)
        {
            SceneManager.LoadScene("SecondChestScene");
        }
        else
        {
            string sceneName = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene(sceneName);
        }
    }
}
