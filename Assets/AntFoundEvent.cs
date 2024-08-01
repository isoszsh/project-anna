using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntFoundEvent : Event
{


    public Camera antFoundCam;
    public Camera playerCam;

    public Animator antAnimator;

    public AudioSource antAus;

    public AudioClip antFoundClip;

    public GameObject scissors;

    public GameObject scissorsCam;

    public DialogueStarter antDS;
    public GameObject antDC;

    public GameObject webDC;

    public override void TriggerStartEvent()
    {
        if(antDS.findIndex == 2)
        {
            StartCoroutine(AntFoundCoroutine());

        }
    }


    IEnumerator AntFoundCoroutine()
    {
        playerCam.gameObject.SetActive(false);
        antFoundCam.gameObject.SetActive(true);
        antAus.PlayOneShot(antFoundClip);
        antAnimator.SetTrigger("ShakeAntennas");
        yield return new WaitForSeconds(3);
        antFoundCam.gameObject.SetActive(false);
        scissorsCam.gameObject.SetActive(true);
        yield return new WaitForSeconds(1);
        scissors.gameObject.SetActive(true);
        yield return new WaitForSeconds (2);
        scissorsCam.gameObject.SetActive(false);
        playerCam.gameObject.SetActive(true );
        antDS.readyToTalk = false;
        GameManager.Instance.playerController.currentEvent = null;
        antDC.SetActive(false);
        webDC.SetActive(true);
    }
}
