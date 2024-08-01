using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryManager : MonoBehaviour
{

    public AudioSource aus;
    public AudioClip horn;

    public GameObject darkenPanel;


    void Start()
    {
        StartCoroutine(Victory());
    }


    IEnumerator Victory()
    {
        aus.PlayOneShot(horn);
        yield return new WaitForSeconds(0.5f);
        GameManager.Instance.playerController.playerAnimator.SetTrigger("Wave");
        yield return new WaitForSeconds(1);
        darkenPanel.SetActive(true);
        darkenPanel.GetComponent<Animator>().SetTrigger("Darken");
        yield return new WaitForSeconds(3);
        SceneManager.LoadSceneAsync("lv3_to_lv4");

    }

    
}
