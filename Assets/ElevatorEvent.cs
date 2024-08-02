using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ElevatorEvent : Event
{
    public Image dcImg;

    public Sprite desiredSprite;

    private Sprite storedSprite;

    public AudioSource aus;
    public AudioClip auc;

    public GameObject darkenPanel;

    public GameObject elevatorDC;



    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
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
        StartCoroutine(Elevate());
    }

    IEnumerator Elevate()
    {
        elevatorDC.SetActive(false);
        aus.PlayOneShot(auc);
        darkenPanel.SetActive(true);
        darkenPanel.GetComponent<Animator>().SetTrigger("Darken");
        yield return new WaitForSeconds(2.5f);
        SceneManager.LoadSceneAsync("Level5_Fight");
    }
}
