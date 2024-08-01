using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ValveEvent : Event
{

    public Image dcImg;

    public Sprite desiredSprite;

    private Sprite storedSprite;

    public AudioSource aus;
    public AudioClip auc;

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
        StartCoroutine(OpenValve());
    }

    IEnumerator OpenValve()
    {
        GameManager.Instance.playerController.lockControls = true;
        GameManager.Instance.playerController.ResetVelocity();
        GameManager.Instance.playerController.playerAnimator.SetTrigger("Paint");
        aus.PlayOneShot(auc);
        yield return new WaitForSeconds(3);
        darkenPanel.SetActive(true);
        darkenPanel.GetComponent<Animator>().SetTrigger("Darken");
        yield return new WaitForSeconds(3);
        SceneManager.LoadSceneAsync("Level4_Victory");
    }
}
