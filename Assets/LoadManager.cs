using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadManager : MonoBehaviour
{

    public string sceneName;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SceneLoader());
    }

    IEnumerator SceneLoader()
    {
        yield return new WaitForSeconds(8f);
        SceneManager.LoadSceneAsync(sceneName);
    }
}
