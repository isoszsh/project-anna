using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DoorController : Event
{
    public GameObject eventCanvas;
    public AudioSource doorAus;

    public Image triggerHeadImage;

    public Sprite desiredSprite;

    private Sprite storedSprite;

    public bool isActiveEvent = false;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") && isActiveEvent)
        {
            storedSprite = triggerHeadImage.sprite;
            triggerHeadImage.sprite = desiredSprite;
            GameManager.Instance.playerController.currentEvent = this;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            triggerHeadImage.sprite = storedSprite;

            GameManager.Instance.playerController.currentEvent = null;
        }
    }


    public override void TriggerStartEvent()
    {
       eventCanvas.SetActive(true);
       isActiveEvent = true;
    }

    public override void TriggerEvent()
    {
        StartCoroutine(KnockTheDoor());
    }

    IEnumerator KnockTheDoor()
    {
        GameManager.Instance.playerController.lockControls = true;
        eventCanvas.SetActive(false);
        GameManager.Instance.playerController.playerAnimator.SetTrigger("Knock");
        
        doorAus.Play();
        yield return new WaitForSeconds(2f);

        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1, LoadSceneMode.Single);
    }
}
