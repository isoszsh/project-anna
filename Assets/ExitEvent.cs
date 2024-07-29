using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ExitEvent : Event
{

    public GameObject exitDC;

    public Image dcImg;

    public Sprite desiredSprite;

    private Sprite storedSprite;

    public AudioSource exitAus;

    public AudioClip exitClip;

    public GameObject darkenPanel;


    public void OnTriggerEnter(Collider other)
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
        StartCoroutine(Climb());
    }

    IEnumerator Climb()
    {
        GameManager.Instance.playerController.lockControls = true;
        yield return new WaitForSeconds(1);
        exitAus.PlayOneShot(exitClip);
        yield return new WaitForSeconds(1);
        darkenPanel.SetActive(true);
        darkenPanel.GetComponent<Animator>().SetTrigger("Darken");
        yield return new WaitForSeconds(2.5f);
        SceneManager.LoadSceneAsync("Level3_Victory");
    }
}
