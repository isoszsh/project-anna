using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EnterShipEvent : Event
{

    public GameObject darkPanel;
    public AudioSource aus;
    public AudioClip chain;

    public Image dcImg;
    public Sprite desiredSprite;

    private Sprite storedSprite;

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
        StartCoroutine(EnterShip());

    }


    IEnumerator EnterShip()
    {
        GameManager.Instance.playerController.playerAnimator.SetTrigger("Paint");
        yield return new WaitForSeconds(1);
        aus.PlayOneShot(chain);
        darkPanel.gameObject.SetActive(true);
        darkPanel.GetComponent<Animator>().SetTrigger("Darken");
        yield return new WaitForSeconds(2);
        SceneManager.LoadSceneAsync("Level3_Ship");

    }
}
