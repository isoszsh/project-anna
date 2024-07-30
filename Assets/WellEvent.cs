using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WellEvent : Event
{
    public Image dcImg;
    public Sprite desiredSprite;

    public GameObject wellDC;

    private Sprite storedSprite;

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

    public override void TriggerStartEvent()
    {
        wellDC.SetActive(true);
    }

    public override void TriggerEvent()
    {
        StartCoroutine(EnterWell());
    }


    IEnumerator EnterWell()
    {
        darkenPanel.SetActive(true);
        darkenPanel.GetComponent<Animator>().SetTrigger("Darken");
        yield return new WaitForSeconds(3);
        SceneManager.LoadSceneAsync("Level4_labirent");
    }
}
