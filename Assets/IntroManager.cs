using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroManager : MonoBehaviour
{

    public GameObject[] items;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Intro());
    }

   IEnumerator Intro()
    {
        yield return new WaitForSeconds(3);
        items[0].SetActive(true);
        yield return new WaitForSeconds(3);
        items[0].SetActive(false);
        yield return new WaitForSeconds(1);
        items[1].SetActive(true);
        yield return new WaitForSeconds(3);
        items[1].SetActive(false);
        yield return new WaitForSeconds(1);
        items[2].SetActive(true);
        yield return new WaitForSeconds(5.6f);
        items[2].SetActive(false);
        yield return new WaitForSeconds(1);
        items[3].SetActive(true);
        yield return new WaitForSeconds(54);
        SceneManager.LoadSceneAsync("Main Menu");
    }
}
