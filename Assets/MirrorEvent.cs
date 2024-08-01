using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MirrorEvent : Event
{


    public Image dcImg;
    public Sprite desiredSprite;

    public GameObject mirrorDC;

    private Sprite storedSprite;

    public GameObject playerCam;
    public GameObject mirrorCam;
    public GameObject mirrorCam2;
    public GameObject mirrorCam3;
    public GameObject mirrorCam4;

    public GameObject decisionPanel;

    public GameObject ortu;
    public GameObject beams;
    public MeshCollider insideMesh;

    public GameObject darkenPanel;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {

            storedSprite = dcImg.sprite;
            dcImg.sprite = desiredSprite;

            GameManager.Instance.playerController.currentEvent = this;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            dcImg.sprite = storedSprite;
            GameManager.Instance.playerController.currentEvent = null;
        }
    }


    public override void TriggerEvent()
    {
        StartCoroutine(OrtuCR());

    }


    IEnumerator OrtuCR()
    {
        GameManager.Instance.playerController.lockControls = true;
        insideMesh.enabled = false;
        beams.gameObject.SetActive(true);
        Renderer renderer = beams.GetComponent<Renderer>();
        renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
        mirrorDC.SetActive(false);
        playerCam.SetActive(false);
        mirrorCam.SetActive(true);
        mirrorCam.GetComponent<Animator>().SetTrigger("Start");
        yield return new WaitForSeconds(2);
        GameManager.Instance.playerController.playerAnimator.SetTrigger("Paint");
        yield return new WaitForSeconds(1);
        ortu.gameObject.SetActive(false);
        yield return new WaitForSeconds(2);
        mirrorCam.SetActive(false);
        mirrorCam2.SetActive(true);
        mirrorCam2.GetComponent<Animator>().SetTrigger("Start");
        yield return new WaitForSeconds(3);
        decisionPanel.SetActive(true);

    }


    public void LYS()
    {
        StartCoroutine(LookYourself());
    }

    public void DLYS()
    {
        StartCoroutine(DontLookYourself());
    }


    void OpenColor()
    {
        Debug.Log("ColorGeldi");
    }

    IEnumerator LookYourself()
    {
        mirrorCam2.SetActive(false);
        mirrorCam3.SetActive(true);
        mirrorCam3.GetComponent<Animator>().SetTrigger("Look");
        OpenColor();
        yield return new WaitForSeconds(5f);
        darkenPanel.SetActive(true);
        darkenPanel.GetComponent<Animator>().SetTrigger("Darken");
        yield return new WaitForSeconds(2);
        SceneManager.LoadSceneAsync("Level2");
    }

    IEnumerator DontLookYourself()
    {
        mirrorCam2.SetActive(false);
        mirrorCam4.SetActive(true);
        mirrorCam3.GetComponent<Animator>().SetTrigger("Look");
        OpenColor();
        yield return new WaitForSeconds(5f);
        darkenPanel.SetActive(true);
        darkenPanel.GetComponent<Animator>().SetTrigger("Darken");
        yield return new WaitForSeconds(2);
        SceneManager.LoadSceneAsync("Level2");
    }
}
