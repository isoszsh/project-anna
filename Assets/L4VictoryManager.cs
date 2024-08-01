using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class L4VictoryManager : MonoBehaviour
{


    public GameObject apple;

    public GameObject cam1;
    public GameObject cam2;
    public GameObject cam3;
    public GameObject cam4;

    public AudioSource aus;
    public AudioClip ystClip;

    public AudioClip yesac;
    public AudioClip noac;

    public GameObject decisionPanel;

    public GameObject darkenPanel;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(AppleScene());
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    IEnumerator AppleScene()
    {
        GameManager.Instance.playerController.lockControls = true;
        yield return new WaitForSeconds(1f);
        apple.GetComponent<Animator>().SetTrigger("Fall");
        yield return new WaitForSeconds(1f);
        cam1.gameObject.SetActive(false);
        cam2.gameObject.SetActive(true);
        aus.PlayOneShot(ystClip);
        yield return new WaitForSeconds(6f);
        cam2.gameObject.SetActive(false );
        cam3.gameObject.SetActive(true);
        decisionPanel.SetActive(true);
    }


    public void Eat()
    {
        StartCoroutine(HeadYes());
    }

    public void RefuseEat()
    {
        StartCoroutine(HeadNo());
    }

    IEnumerator HeadYes()
    {
        GameManager.Instance.pr.SaveDecision("Level_4", 1);

        GameManager.Instance.playerController.playerAnimator.SetTrigger("HeadYes");
        decisionPanel.SetActive(false);

        yield return new WaitForSeconds(3);
        cam3.gameObject.SetActive(false);
        cam2.gameObject.SetActive(true);
        aus.PlayOneShot(yesac);
        yield return new WaitForSeconds(5);
        darkenPanel.SetActive(true);
        darkenPanel.GetComponent<Animator>().SetTrigger("Darken");
        yield return new WaitForSeconds(2.5f);
        SceneManager.LoadSceneAsync("lv4_to_lv5");

    }

    IEnumerator HeadNo()
    {
        GameManager.Instance.pr.SaveDecision("Level_4", 2);
        GameManager.Instance.playerController.playerAnimator.SetTrigger("HeadNo");
        decisionPanel.SetActive(false);
        yield return new WaitForSeconds(3);
        cam3.gameObject.SetActive(false) ;
        cam2.gameObject.SetActive(true);
        aus.PlayOneShot(noac);
        yield return new WaitForSeconds(3);
        darkenPanel.SetActive(true);
        darkenPanel.GetComponent<Animator>().SetTrigger("Darken");
        yield return new WaitForSeconds(2.5f);
        SceneManager.LoadSceneAsync("lv4_to_lv5");
    }
}
