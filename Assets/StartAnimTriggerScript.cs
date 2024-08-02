using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartAnimTriggerScript : MonoBehaviour
{
    public GameObject camraOriginal;
    public GameObject camraAnim0;
    public GameObject camraAnim1;
    public GameObject camraAnnaFaceAngry;
    public GameObject camraBossSpeakPosition;
    public GameObject camraAnnaSpeakPosition;

    public GameObject boss;

    public GameObject gameManager;

    public GameObject backgroundMusic;

    public AudioSource witchAus;
    public AudioClip witchAuc;

    public List<GameObject> lights;

    private void Start()
    {
        camraAnim1.SetActive(false);
        camraAnnaFaceAngry.SetActive(false);
        camraBossSpeakPosition.SetActive(false);
        camraAnnaSpeakPosition.SetActive(false);
        boss.SetActive(false);
        backgroundMusic.SetActive(false);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            StartCoroutine(StartAnim());
        }
    }


    IEnumerator StartAnim()
    {
        GameManager.Instance.playerController.lockControls = true;
        GameManager.Instance.playerController.ResetVelocity();


        camraOriginal.SetActive(false);
        camraAnim0.SetActive(true);
        yield return new WaitForSeconds(2f);
        yield return LightsOn();
        camraAnim0.SetActive(false);
        camraAnim1.SetActive(true);

        yield return new WaitForSeconds(2f);
        boss.SetActive(true);
        
        camraAnim1.GetComponent<Animator>().SetTrigger("Start");
        yield return new WaitForSeconds(6f);
        camraAnim1.SetActive(false);
        camraAnnaFaceAngry.SetActive(true);
        yield return new WaitForSeconds(4f);
        camraAnnaFaceAngry.SetActive(false);

        camraBossSpeakPosition.SetActive(true);
        witchAus.PlayOneShot(witchAuc);
        yield return new WaitForSeconds(13f);// burası hocam
        camraBossSpeakPosition.SetActive(false);

        //camraAnnaSpeakPosition.SetActive(true);
        //Debug.Log("Anna konuşma başladı");
       // yield return new WaitForSeconds(2f);// burası hocam
        //camraAnnaSpeakPosition.SetActive(false);

        GameManager.Instance.playerController.lockControls = false;
        camraOriginal.SetActive(true);

        
        boss.GetComponent<WitchController>().StartEveryThink();
        backgroundMusic.SetActive(true);

        yield return new WaitForSeconds(1f);
        this.gameObject.SetActive(false);
    }

    IEnumerator LightsOn(){
        //for ile gez yarısına kadar gez ve her seferinde iki tane yak

        for (int i = 0; i < lights.Count/2; i++)
        {
            lights[i].SetActive(true);
            lights[i + lights.Count / 2].SetActive(true);
            yield return new WaitForSeconds(0.3f);
        }
        
    }
}
